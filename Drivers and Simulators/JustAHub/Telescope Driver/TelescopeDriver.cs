using ASCOM.DeviceInterface;
using ASCOM.LocalServer;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCOM.JustAHub
{
    /// <summary>
    /// ASCOM Telescope Driver for JustAHub.
    /// </summary>
    [ComVisible(true)]
    [Guid("E22DB8A8-B31E-4C12-8CBB-1E4EA2960CEA")]
    [ProgId("ASCOM.JustAHub.Telescope")]
    [ServedClassName("JustAHub Telescope")] // Driver description that appears in the Chooser, customise as required
    [ClassInterface(ClassInterfaceType.None)]
    public class Telescope : ReferenceCountedObjectBase, ITelescopeV4, IDisposable
    {
        private Guid uniqueId; // A unique ID for this instance of the driver

        internal TraceLogger tl; // Trace logger object to hold diagnostic information just for this instance of the driver, as opposed to the local server's log, which includes activity from all driver instances.
        private bool disposedValue;

        #region Initialisation and Dispose

        static Telescope()
        {
            // Pull the ProgID from the ProgID class attribute.
            Attribute attr = Attribute.GetCustomAttribute(typeof(Telescope), typeof(ProgIdAttribute));

            // Get the driver ProgIDfrom the ProgID attribute.
            ProgId = ((ProgIdAttribute)attr).Value ?? "PROGID NOT SET!";

            // Pull the display name from the ServedClassName class attribute.
            attr = Attribute.GetCustomAttribute(typeof(Telescope), typeof(ServedClassNameAttribute));

            // Get the driver description that displays in the ASCOM Chooser from the ServedClassName attribute.
            ChooserDescription = ((ServedClassNameAttribute)attr).DisplayName ?? "DISPLAY NAME NOT SET!";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASCOM.Telescope"/> class. Must be public to successfully register for COM.
        /// </summary>
        public Telescope()
        {
            try
            {
                tl = new TraceLogger("", "JustAHub.Telescope.Driver")
                {
                    Enabled = Settings.TelescopeDriverLogging
                };

                // Initialise the hardware if required
                TelescopeHardware.Initialise();

                LogMessage("Telescope", "Starting driver initialisation");
                LogMessage("Telescope", $"ProgID: {ProgId}, Description: {ChooserDescription}");

                // Create a unique ID to identify this driver instance
                uniqueId = Guid.NewGuid();

                LogMessage("Telescope", "Completed initialisation");
            }
            catch (Exception ex)
            {
                LogMessage("Telescope", $"Initialisation exception: {ex}");
                MessageBox.Show($"{ex.Message}", "Exception creating ASCOM.JustAHub.Telescope", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Class destructor called automatically by the .NET runtime when the object is finalised in order to release resources that are NOT managed by the .NET runtime.
        /// </summary>
        /// <remarks>See the Dispose(bool disposing) remarks for further information.</remarks>
        ~Telescope()
        {
            // Please do not change this code.
            // The Dispose(false) method is called here just to release unmanaged resources. Managed resources will be dealt with automatically by the .NET runtime.

            Dispose(false);
        }

        /// <summary>
        /// Deterministically dispose of any managed and unmanaged resources used in this instance of the driver.
        /// </summary>
        /// <remarks>
        /// Do not dispose of items in this method, put clean-up code in the 'Dispose(bool disposing)' method instead.
        /// </remarks>
        public void Dispose()
        {
            // Please do not change the code in this method.

            // Release resources now.
            Dispose(disposing: true);

            // Do not add GC.SuppressFinalize(this); here because it breaks the ReferenceCountedObjectBase COM connection counting mechanic
        }

        /// <summary>
        /// Dispose of large or scarce resources created or used within this driver file
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to enable you to release finite system resources back to the operating system as soon as possible, so that other applications work as effectively as possible.
        ///
        /// NOTES
        /// 1) Do not call the TelescopeHardware.Dispose() method from this method. Any resources used in the static TelescopeHardware class itself, 
        ///    which is shared between all instances of the driver, should be released in the TelescopeHardware.Dispose() method as usual. 
        ///    The TelescopeHardware.Dispose() method will be called automatically by the local server just before it shuts down.
        /// 2) You do not need to release every .NET resource you use in your driver because the .NET runtime is very effective at reclaiming these resources. 
        /// 3) Strong candidates for release here are:
        ///     a) Objects that have a large memory footprint (> 1Mb) such as images
        ///     b) Objects that consume finite OS resources such as file handles, synchronisation object handles, memory allocations requested directly from the operating system (NativeMemory methods) etc.
        /// 4) Please ensure that you do not return exceptions from this method
        /// 5) Be aware that Dispose() can be called more than once:
        ///     a) By the client application
        ///     b) Automatically, by the .NET runtime during finalisation
        /// 6) Because of 5) above, you should make sure that your code is tolerant of multiple calls.    
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        // Dispose of managed objects here

                        // Clean up the trace logger object
                        if (!(tl is null))
                        {
                            tl.Enabled = false;
                            tl.Dispose();
                            tl = null;
                        }
                    }
                    catch (Exception)
                    {
                        // Any exception is not re-thrown because Microsoft's best practice says not to return exceptions from the Dispose method. 
                    }
                }

                try
                {
                    // Dispose of unmanaged objects, if any, here (OS handles etc.)
                }
                catch (Exception)
                {
                    // Any exception is not re-thrown because Microsoft's best practice says not to return exceptions from the Dispose method. 
                }

                // Flag that Dispose() has already run and disposed of all resources
                disposedValue = true;
            }
        }

        #endregion

        // PUBLIC COM INTERFACE ITelescopeV3 IMPLEMENTATION

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            Server.SetupDialog("Telescope");
            // Update the trace setting in case it was changed
            tl.Enabled = Settings.TelescopeDriverLogging;
        }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        public ArrayList SupportedActions
        {
            get
            {
                try
                {
                    CheckConnected($"SupportedActions");
                    ArrayList actions = TelescopeHardware.SupportedActions;
                    LogMessage("SupportedActions", $"Returning {actions.Count} actions.");
                    return actions;
                }
                catch (Exception ex)
                {
                    LogMessage("SupportedActions", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        public string Action(string actionName, string actionParameters)
        {
            try
            {
                CheckConnected($"Action {actionName} - {actionParameters}");
                LogMessage("Action", $"Calling Action: {actionName} with parameters: {actionParameters}");
                string actionResponse = TelescopeHardware.Action(actionName, actionParameters);
                LogMessage("Action", $"Completed.");
                return actionResponse;
            }
            catch (Exception ex)
            {
                LogMessage("Action", $"Threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        public void CommandBlind(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandBlind: {command}, Raw: {raw}");
                LogMessage("CommandBlind", $"Calling method - Command: {command}, Raw: {raw}");
                TelescopeHardware.CommandBlind(command, raw);
                LogMessage("CommandBlind", $"Completed.");
            }
            catch (Exception ex)
            {
                LogMessage("CommandBlind", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        public bool CommandBool(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandBool: {command}, Raw: {raw}");
                LogMessage("CommandBool", $"Calling method - Command: {command}, Raw: {raw}");
                bool commandBoolResponse = TelescopeHardware.CommandBool(command, raw);
                LogMessage("CommandBool", $"Returning: {commandBoolResponse}.");
                return commandBoolResponse;
            }
            catch (Exception ex)
            {
                LogMessage("CommandBool", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        public string CommandString(string command, bool raw)
        {
            try
            {
                CheckConnected($"CommandString: {command}, Raw: {raw}");
                LogMessage("CommandString", $"Calling method - Command: {command}, Raw: {raw}");
                string commandStringResponse = TelescopeHardware.CommandString(command, raw);
                LogMessage("CommandString", $"Returning: {commandStringResponse}.");
                return commandStringResponse;
            }
            catch (Exception ex)
            {
                LogMessage("CommandString", $"Command: {command}, Raw: {raw} threw an exception: \r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        public bool Connected
        {
            get
            {
                try
                {
                    // Return the driver's connection state which could be different to the hardware connected state, because there may be other client connections still active.
                    bool connectedState = TelescopeHardware.IsConnected(uniqueId);
                    LogMessage("Connected Get", connectedState.ToString());
                    return connectedState;
                }
                catch (Exception ex)
                {
                    LogMessage("Connected Get", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
            set
            {
                try
                {
                    // Handle connect and disconnect requests
                    if (value) // Request to connect
                    {
                        LogMessage("Connected Set", "Connecting to device");
                        TelescopeHardware.Connect(uniqueId, ConnectType.Connected);
                    }
                    else // Request to disconnect
                    {
                        LogMessage("Connected Set", "Disconnecting from device");
                        TelescopeHardware.Disconnect(uniqueId, ConnectType.Connected);
                    }
                }
                catch (Exception ex)
                {
                    LogMessage("Connected Set", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                try
                {
                    CheckConnected($"Description");
                    string description = TelescopeHardware.Description;
                    LogMessage("Description", description);
                    return description;
                }
                catch (Exception ex)
                {
                    LogMessage("Description", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        public string DriverInfo
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string driverInfo = TelescopeHardware.DriverInfo;
                    LogMessage("DriverInfo", driverInfo);
                    return driverInfo;
                }
                catch (Exception ex)
                {
                    LogMessage("DriverInfo", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver formatted as 'm.n'.
        /// </summary>
        public string DriverVersion
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string driverVersion = TelescopeHardware.DriverVersion;
                    LogMessage("DriverVersion", driverVersion);
                    return driverVersion;
                }
                catch (Exception ex)
                {
                    LogMessage("DriverVersion", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// The interface version number that this device supports.
        /// </summary>
        public short InterfaceVersion
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    short interfaceVersion = TelescopeHardware.InterfaceVersion;
                    LogMessage("InterfaceVersion", interfaceVersion.ToString());
                    return interfaceVersion;
                }
                catch (Exception ex)
                {
                    LogMessage("InterfaceVersion", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        public string Name
        {
            get
            {
                try
                {
                    // This should work regardless of whether or not the driver is Connected, hence no CheckConnected method.
                    string name = TelescopeHardware.Name;
                    LogMessage("Name Get", name);
                    return name;
                }
                catch (Exception ex)
                {
                    LogMessage("Name", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
        }

        #endregion

        #region ITelescope Implementation

        public AlignmentModes AlignmentMode //=> (AlignmentModes)(GetValue(() => TelescopeHardware.AlignmentMode));
        {
            get
            {
                try
                {
                    CheckConnected("AlignmentMode");
                    AlignmentModes alignmentMode = TelescopeHardware.AlignmentMode;
                    LogMessage("AlignmentMode", alignmentMode.ToString());
                    return alignmentMode;
                }
                catch (Exception ex)
                {
                    LogMessage("AlignmentMode", $"Threw an exception: {ex.Message}\r\n{ex}");
                    throw;
                }
            }
        }

        public double Altitude => GetValue(() => TelescopeHardware.Altitude);

        public double ApertureDiameter => GetValue(() => TelescopeHardware.ApertureDiameter);

        public double Azimuth => GetValue(() => TelescopeHardware.Azimuth);

        public bool CanFindHome => GetValue(() => TelescopeHardware.CanFindHome);

        public bool CanPark => GetValue(() => TelescopeHardware.CanPark);

        public bool CanPulseGuide => GetValue(() => TelescopeHardware.CanPulseGuide);

        public bool CanSetPark => GetValue(() => TelescopeHardware.CanSetPark);
        public bool CanSetTracking => GetValue(() => TelescopeHardware.CanSetTracking);

        public bool CanSlew => GetValue(() => TelescopeHardware.CanSlew);

        public bool CanSlewAsync => GetValue(() => TelescopeHardware.CanSlewAsync);

        public bool CanSync => GetValue(() => TelescopeHardware.CanSync);

        public bool CanUnpark => GetValue(() => TelescopeHardware.CanUnpark);

        public double Declination => GetValue(() => TelescopeHardware.Declination);

        public double DeclinationRate { get => GetValue(() => TelescopeHardware.DeclinationRate); set => CallMember(() => TelescopeHardware.DeclinationRate = value); }

        public double FocalLength => GetValue(() => TelescopeHardware.FocalLength);

        public double RightAscension => GetValue(() => TelescopeHardware.RightAscension);

        public double RightAscensionRate { get => GetValue(() => TelescopeHardware.RightAscensionRate); set => CallMember(() => TelescopeHardware.RightAscensionRate = value); }

        public double SiderealTime => GetValue(() => TelescopeHardware.SiderealTime);

        public double SiteElevation { get => GetValue(() => TelescopeHardware.SiteElevation); set => CallMember(() => TelescopeHardware.SiteElevation = value); }
        public double SiteLatitude { get => GetValue(() => TelescopeHardware.SiteLatitude); set => CallMember(() => TelescopeHardware.SiteLatitude = value); }
        public double SiteLongitude { get => GetValue(() => TelescopeHardware.SiteLongitude); set => CallMember(() => TelescopeHardware.SiteLongitude = value); }

        public bool Slewing => GetValue(() => TelescopeHardware.Slewing);

        public short SlewSettleTime { get => GetValue(() => TelescopeHardware.SlewSettleTime); set => CallMember(() => TelescopeHardware.SlewSettleTime = value); }
        public double TargetDeclination { get => GetValue(() => TelescopeHardware.TargetDeclination); set => CallMember(() => TelescopeHardware.TargetDeclination = value); }
        public double TargetRightAscension { get => GetValue(() => TelescopeHardware.TargetRightAscension); set => CallMember(() => TelescopeHardware.TargetRightAscension = value); }
        public bool Tracking { get => GetValue(() => TelescopeHardware.Tracking); set => CallMember(() => TelescopeHardware.Tracking = value); }
        public DateTime UTCDate { get => GetValue(() => TelescopeHardware.UTCDate); set => CallMember(() => TelescopeHardware.UTCDate = value); }

        public double ApertureArea => GetValue(() => TelescopeHardware.ApertureArea);

        public bool AtHome => GetValue(() => TelescopeHardware.AtHome);

        public bool AtPark => GetValue(() => TelescopeHardware.AtPark);

        public bool CanSetDeclinationRate => GetValue(() => TelescopeHardware.CanSetDeclinationRate);

        public bool CanSetGuideRates => GetValue(() => TelescopeHardware.CanSetGuideRates);

        public bool CanSetPierSide => GetValue(() => TelescopeHardware.CanSetPierSide);

        public bool CanSetRightAscensionRate => GetValue(() => TelescopeHardware.CanSetRightAscensionRate);

        public bool CanSlewAltAz => GetValue(() => TelescopeHardware.CanSlewAltAz);

        public bool CanSlewAltAzAsync => GetValue(() => TelescopeHardware.CanSlewAltAzAsync);

        public bool CanSyncAltAz => GetValue(() => TelescopeHardware.CanSyncAltAz);

        public bool DoesRefraction { get => GetValue(() => TelescopeHardware.DoesRefraction); set => CallMember(() => TelescopeHardware.DoesRefraction = value); }

        public EquatorialCoordinateType EquatorialSystem => GetValue(() => TelescopeHardware.EquatorialSystem);

        public double GuideRateDeclination { get => GetValue(() => TelescopeHardware.GuideRateDeclination); set => CallMember(() => TelescopeHardware.GuideRateDeclination = value); }
        public double GuideRateRightAscension { get => GetValue(() => TelescopeHardware.GuideRateRightAscension); set => CallMember(() => TelescopeHardware.GuideRateRightAscension = value); }

        public bool IsPulseGuiding => GetValue(() => TelescopeHardware.IsPulseGuiding);

        public PierSide SideOfPier { get => GetValue(() => TelescopeHardware.SideOfPier); set => CallMember(() => TelescopeHardware.SideOfPier = value); }
        public DriveRates TrackingRate { get => GetValue(() => TelescopeHardware.TrackingRate); set => CallMember(() => TelescopeHardware.TrackingRate = value); }

        public ITrackingRates TrackingRates => GetValue(() => TelescopeHardware.TrackingRates);

        public void AbortSlew()
        {
            CallMember(() => TelescopeHardware.AbortSlew());
        }

        public void FindHome()
        {
            CallMember(() => TelescopeHardware.FindHome());
        }

        public void Park()
        {
            CallMember(() => TelescopeHardware.Park());
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            CallMember(() => TelescopeHardware.PulseGuide(Direction, Duration));
        }

        public void SetPark()
        {
            CallMember(() => TelescopeHardware.SetPark());
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            CallMember(() => TelescopeHardware.SlewToCoordinates(RightAscension, Declination));
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            CallMember(() => TelescopeHardware.SlewToCoordinatesAsync(RightAscension, Declination));
        }

        public void SlewToTarget()
        {
            CallMember(() => TelescopeHardware.SlewToTarget());
        }

        public void SlewToTargetAsync()
        {
            CallMember(() => TelescopeHardware.SlewToTargetAsync());
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            CallMember(() => TelescopeHardware.SyncToCoordinates(RightAscension, Declination));
        }

        public void SyncToTarget()
        {
            CallMember(() => TelescopeHardware.SyncToTarget());
        }

        public void Unpark()
        {
            CallMember(() => TelescopeHardware.Unpark());
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
            return GetValue(() => TelescopeHardware.AxisRates(Axis));
        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            return GetValue(() => TelescopeHardware.CanMoveAxis(Axis));
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            return GetValue(() => TelescopeHardware.DestinationSideOfPier(RightAscension, Declination));
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            CallMember(() => TelescopeHardware.MoveAxis(Axis, Rate));
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            CallMember(() => TelescopeHardware.SlewToAltAz(Azimuth, Altitude));
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            CallMember(() => TelescopeHardware.SlewToAltAzAsync(Azimuth, Altitude));
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            CallMember(() => TelescopeHardware.SyncToAltAz(Azimuth, Altitude));
        }

        #endregion

        #region Private properties and methods

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string memberName)
        {
            if (!TelescopeHardware.IsConnected(uniqueId))
            {
                throw new NotConnectedException($"{ChooserDescription} ({ProgId}) is not connected. Called from: {memberName}");
            }
        }

        /// <summary>
        /// Log helper function that writes to the driver or local server loggers as required
        /// </summary>
        /// <param name="identifier">Identifier such as method name</param>
        /// <param name="message">Message to be logged.</param>
        private void LogMessage(string identifier, string message)
        {
            // This code is currently set to write messages to an individual driver log AND to the shared hardware log.

            // Write to the individual log for this specific instance (if enabled by the driver having a TraceLogger instance)
            tl?.LogMessageCrLf(identifier, message); // Write to the individual driver log

            // Write to the common hardware log shared by all running instances of the driver.
            TelescopeHardware.LogMessage(identifier, message); // Write to the local server logger
        }

        #endregion

        #region Connect / Disconnect and DeviceState members

        /// <summary>
        /// Connect to the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>This is a mandatory method and must not throw a <see cref="MethodNotImplementedException"/>.</b></p></remarks>
        public void Connect()
        {
            // Check whether the connected device is Platform 7 or later
            if (Common.DeviceInterfaces.DeviceCapabilities.HasConnectAndDeviceState(Common.DeviceTypes.Telescope, TelescopeHardware.GetInterfaceVersion())) // Platform 7 or later device so Connect
            {
                LogMessage("Connect", "Issuing Connect command");
                TelescopeHardware.Connect(uniqueId, ConnectType.Connect_Disconnect);
                return;
            }

            // Platform 6 or earlier so reject the method call
            LogMessage("Connect", "Throwing a MethodNotImplementedException because the connected Telescope is a Platform 6 or earlier device.");
            throw new MethodNotImplementedException($"Connect is not implemented in this ITelescopeV{TelescopeHardware.GetInterfaceVersion()} device.");
        }

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        public void Disconnect()
        {
            // Check whether the connected device is Platform 7 or later
            if (Common.DeviceInterfaces.DeviceCapabilities.HasConnectAndDeviceState(Common.DeviceTypes.Telescope, TelescopeHardware.GetInterfaceVersion())) // Platform 7 or later device so Disconnect
            {
                LogMessage("Disconnect", "Issuing Disconnect command");
                TelescopeHardware.Disconnect(uniqueId, ConnectType.Connect_Disconnect);
                return;
            }

            // Platform 6 or earlier so reject the method call
            LogMessage("Disconnect", "Throwing a MethodNotImplementedException because the connected Telescope is a Platform 6 or earlier device.");
            throw new MethodNotImplementedException($"Disconnect is not implemented in this ITelescopeV{TelescopeHardware.GetInterfaceVersion()} device.");
        }

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        public bool Connecting
        {
            get
            {
                // Check whether the connected device is Platform 7 or later
                if (Common.DeviceInterfaces.DeviceCapabilities.HasConnectAndDeviceState(Common.DeviceTypes.Telescope, TelescopeHardware.GetInterfaceVersion())) // Platform 7 or later device so call Connecting
                {
                    LogMessage("Connecting", "Calling Connecting property");
                    return TelescopeHardware.Connecting;
                }

                // Platform 6 or earlier so reject the method call
                LogMessage("Connecting", "Throwing a PropertyNotImplementedException because the connected Telescope is a Platform 6 or earlier device.");
                throw new PropertyNotImplementedException($"Connecting is not implemented in this ITelescopeV{TelescopeHardware.GetInterfaceVersion()} device.");
            }
        }

        /// <summary>
        /// Returns the device's operational state in a single call.
        /// </summary>
        public IStateValueCollection DeviceState
        {
            get
            {
                CheckConnected("DeviceState");

                // Check whether the connected device is Platform 7 or later
                if (Common.DeviceInterfaces.DeviceCapabilities.HasConnectAndDeviceState(Common.DeviceTypes.Telescope, TelescopeHardware.GetInterfaceVersion())) // Platform 7 or later device so call Connecting
                {
                    LogMessage("DeviceState", "Calling DeviceState property");
                    return TelescopeHardware.DeviceState;
                }

                // Platform 6 or earlier so return an empty ArrayList because this feature isn't supported
                LogMessage("DeviceState", "Returning an empty ArrayList because this feature isn't supported.");
                return new StateValueCollection();
            }
        }

        #endregion

        #region Internal members

        internal static string ProgId { get; private set; }
        internal static string ChooserDescription { get; private set; }

        /// <summary>
        /// Get a value from an interface member and log the result
        /// </summary>
        /// <typeparam name="T">The type of the return value</typeparam>
        /// <param name="interfacemember">Function representing the function to be performed</param>
        /// <param name="callerMemberName">The name of the member that called this function (determined automatically by the compiler)</param>
        /// <returns>The member's value or an exception</returns>
        internal T GetValue<T>(Func<T> interfacemember, [CallerMemberName] string callerMemberName = "UnknownCaller")
        {
            try
            {
                // Check whether the device is connected
                CheckConnected(callerMemberName);

                // Call the member and log the result
                T result = interfacemember();
                LogMessage(callerMemberName, result.ToString());

                // Return the result
                return result;
            }
            catch (Exception ex)
            {
                LogMessage(callerMemberName, $"Threw an exception: {ex.Message}\r\n{ex}");
                throw;
            }
        }

        /// <summary>
        /// Call an interface member and log the result
        /// </summary>
        /// <param name="interfacemember">Function representing the function to be performed</param>
        /// <param name="callerMemberName">The name of the member that called this function (determined automatically by the compiler)</param>
        internal void CallMember(Action interfacemember, [CallerMemberName] string callerMemberName = "UnknownCaller")
        {
            try
            {
                // Check whether the device is connected
                CheckConnected(callerMemberName);

                // Call the member and log the result
                LogMessage(callerMemberName, $"Calling {callerMemberName}...");
                interfacemember();
                LogMessage(callerMemberName, $"{callerMemberName} returned OK");
            }
            catch (Exception ex)
            {
                LogMessage(callerMemberName, $"Threw an exception:  {ex.Message}\r\n{ex}");
                throw;
            }
        }

        #endregion
    }
}
