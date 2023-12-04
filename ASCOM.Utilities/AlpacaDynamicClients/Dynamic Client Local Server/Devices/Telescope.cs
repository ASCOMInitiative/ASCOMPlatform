using ASCOM.Alpaca.Clients;
using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ASCOM.Tools;

namespace ASCOM.DynamicClients
{
    /// <summary>
    /// Driver to access the Alpaca SafetyMonitor simulator.
    /// </summary>
    public class Telescope : ReferenceCountedObjectBase, DeviceInterface.ITelescopeV4, IDisposable
    {
        // Set the device type of this device
        private const DeviceTypes deviceType = DeviceTypes.Telescope;

        // The ASCOM Library Alpaca client that is used to communicate with the Alpaca device.
        private AlpacaTelescope client;

        internal static string driverProgId; // ASCOM DeviceID (COM ProgID) for this driver, the value is retrieved from the ServedClassName attribute in the class initialiser.
        internal static string driverDisplayName; // The device description in the Chooser, the value is retrieved from the ServedClassName attribute in the class initialiser.

        internal bool connectedState; // The connected state from this driver's perspective
        internal TraceLogger TL; // Trace logger object to present diagnostic information for this instance of the driver
        private bool disposedValue;

        readonly DynamicClientState state; // Holds state information for this Dynamic Client driver (not the remote Alpaca device)

        private bool asyncConnectDisconnect; // Flag indicating whether an asynchronous Connect or Disconnect operation is in progress
        private bool newConnectedState; // The state to which connectedState will be set when an asynchronous Connect / Disconnect operation completes

        #region Initialisation and Dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="SafetyMonitor"/> class. Must be public to successfully register for COM.
        /// </summary>
        public Telescope()
        {
            try
            {
                // Pull the ProgID from the ProgID class attribute.
                Attribute attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ProgIdAttribute));
                driverProgId = ((ProgIdAttribute)attr).Value ?? "ASCOM.PROGID NOT SET!";  // Get the driver ProgIDfrom the ProgID attribute.

                // Pull the display name from the ServedClassName class attribute.
                attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ServedClassNameAttribute));
                driverDisplayName = ((ServedClassNameAttribute)attr).DisplayName ?? "DISPLAY NAME NOT SET!";  // Get the driver description that displays in the ASCOM Chooser from the ServedClassName attribute.

                // Read the device's configuration from the Profile and assign its ProgID, device type and display name that has been read from the class
                state = new DynamicClientState(driverProgId, deviceType, driverDisplayName);

                // Set up a trace logger
                TL = new TraceLogger(state.ProgId.Substring(6), false)
                {
                    Enabled = state.TraceState
                };
                if (state.DebugTraceState)
                    TL.SetMinimumLoggingLevel(LogLevel.Debug);

                LogMessage(deviceType.ToString(), $"Starting driver initialisation for ProgID: {driverProgId}, Description: {driverDisplayName}");

                // Create a client
                client = Server.GetClient<AlpacaTelescope>(state, TL);
                LogMessage(deviceType.ToString(), $"Alpaca client created successfully");

                // Initialise connected to false
                connectedState = false;

                LogMessage(deviceType.ToString(), "Completed initialisation");
            }
            catch (Exception ex)
            {
                LogMessage(deviceType.ToString(), $"Initialisation exception: {ex}");
                MessageBox.Show($"{ex.Message}", "Exception creating ASCOM.AlpacaSim.SafetyMonitor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Dispose of large or scarce resources created or used within this driver file
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose of the client
                    try { client?.Dispose(); } catch { }

                    // Dispose of the trace logger object
                    try
                    {
                        if (!(TL is null))
                        {
                            TL.Enabled = false;
                            TL.Dispose();
                        }
                    }
                    catch { }
                }

                // Flag that Dispose() has already run and disposed of all resources
                disposedValue = true;
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

        #endregion

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            if (connectedState) // Don't show if already connected
            {
                MessageBox.Show("Already connected, just press OK");
            }
            else // Show dialogue
            {
                AlpacaTelescope newclient = Server.SetupDialogue<AlpacaTelescope>(state, TL);
                if (!(newclient is null))
                {
                    // Dispose of the old client
                    try
                    {
                        client?.Dispose();
                    }
                    catch (Exception ex)
                    {
                        LogMessage("SetupDialog", $"Ignoring exception when disposing current client: {ex.Message}.\r\n{ex}");
                    }

                    // Replace original client with new client
                    client = newclient;
                }
            }
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
                    ArrayList actions = new ArrayList(client.SupportedActions.ToList<string>());
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
                LogMessage("", $"Calling Action: {actionName} with parameters: {actionParameters}");
                string actionResponse = client.Action(actionName, actionParameters);
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
                client.CommandBlind(command, raw);
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
                bool commandBoolResponse = client.CommandBool(command, raw);
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
                string commandStringResponse = client.CommandString(command, raw);
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
                // Returns the driver's connection state rather than the local server's connected state, which could be different because there may be other client connections still active.
                LogMessage("Connected Get", connectedState.ToString());
                return connectedState;
            }
            set
            {
                // Handle local connection management if required.
                if (state.ManageConnectLocally) // Connected state will be set locally but will not be passed to the remote device
                {
                    connectedState = value;
                }
                else // Connected state will be set locally and passed to the to the remote device
                {
                    try
                    {
                        if (value) // Set Connected = TRUE
                        {
                            LogMessage("Connected Set", "Connecting to device");
                            client.Connected = true;
                            connectedState = true;
                            LogMessage("Connected Set", $"Connected to device OK, connected state: {connectedState}");
                        }
                        else // Set Connected = FALSE
                        {
                            LogMessage("Connected Set", "Disconnecting from device");
                            client.Connected = false;
                            connectedState = false;
                            LogMessage("Connected Set", $"Disconnected from device OK, connected state: {connectedState}");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogMessage("Connected Set", $"Threw an exception: {ex.Message}\r\n{ex}");
                        throw;
                    }
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
                    string description = client.Description;
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
                    string driverInfo = client.DriverInfo;
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
                    string driverVersion = client.DriverVersion;
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
                    short interfaceVersion = client.InterfaceVersion;
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
                    string name = client.Name;
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

        public void Connect()
        {
            // Handle local connection management if required.
            if (state.ManageConnectLocally) // Connected state will be set locally immediately but will not be passed to the remote device
            {
                connectedState = true;
            }
            else // Connect will be passed to the to the remote device
            {
                try
                {
                    // Call the client's Connect method
                    client.Connect();

                    // Flag that an asynchronous Connect operation is underway and that the Connected state should be TRUE on completion
                    asyncConnectDisconnect = true;
                    newConnectedState = true;
                }
                catch (Exception ex)
                {
                    LogMessage("Connect", $"Threw an exception: {ex.Message}\r\n{ex}");
                    throw;
                }
            }
        }

        public void Disconnect()
        {
            // Handle local connection management if required.
            if (state.ManageConnectLocally) // Connected state will be set locally immediately but will not be passed to the remote device
            {
                connectedState = false;
            }
            else // Disconnect will be passed to the to the remote device
            {
                try
                {
                    // Call the client's Disconnect method
                    client.Disconnect();

                    // Flag that an asynchronous Disconnect operation is underway and that the Connected state should be FALSE on completion
                    asyncConnectDisconnect = true;
                    newConnectedState = false;
                }
                catch (Exception ex)
                {
                    LogMessage("Disconnect", $"Threw an exception: {ex.Message}\r\n{ex}");
                    throw;
                }
            }
        }

        public bool Connecting
        {
            get
            {
                // Handle local connection management if required.
                if (state.ManageConnectLocally) // Connect and Disconnect operations always complete immediately, so Connecting will always be FALSE
                {
                    return false;
                }
                else // Get the connecting state from the remote device
                {
                    try
                    {
                        // Get the connecting state from the remote device
                        bool connecting = client.Connecting;

                        // If the operation is complete check whether we need to update the local Connected state
                        if (!connecting) // Operation is complete
                        {
                            // Test whether an async operation is underway from this client's perspective
                            if (asyncConnectDisconnect) // An async operation was underway but the device indicates that the operation has completed
                            {
                                // Update the Connected state and unset the async operation underway flag
                                connectedState = newConnectedState;
                                asyncConnectDisconnect = false;
                            }
                            else
                            {
                                // No async operation underway so no action required.
                            }
                        }
                        else // Operation is still underway
                        {
                            // The asynchronous Connect or Disconnect is still underway so no action required
                        }

                        // Return the Connecting state from the device
                        return connecting;
                    }
                    catch (Exception ex)
                    {
                        LogMessage("Connecting", $"Threw an exception: {ex.Message}\r\n{ex}");
                        throw;
                    }
                }
            }
        }

        public ArrayList DeviceState
        {
            get
            {
                try
                {
                    // Initialise the return ArrayList
                    ArrayList returnValue = new ArrayList();

                    // Get the device state from the Alpaca device
                    List<StateValue> deviceState = client.DeviceState;
                    LogMessage("DeviceState", $"Received {deviceState.Count} values");

                    // Parse the returned values and store in the ArrayList
                    foreach (StateValue value in deviceState)
                    {
                        LogMessage("DeviceState", $"  {value.Name} = {value.Value} - Kind: {value.Value.GetType().Name}");
                        returnValue.Add(value);
                    }

                    LogMessage("DeviceState", $"Return value has {returnValue.Count} values");

                    return returnValue;
                }
                catch (Exception ex)
                {
                    LogMessage("DeviceState", $"Threw an exception: {ex.Message}\r\n{ex}");
                    throw;
                }
            }
        }

        #endregion

        #region ITelescope Implementation
        public void AbortSlew()
        {
            client.AbortSlew();
            TL.LogMessage("AbortSlew", "Slew aborted OK");
        }

        public DeviceInterface.AlignmentModes AlignmentMode
        {
            get
            {
                return (DeviceInterface.AlignmentModes)client.AlignmentMode;
            }
        }

        public double Altitude
        {
            get
            {
                return client.Altitude;
            }
        }

        public double ApertureArea
        {
            get
            {
                return client.ApertureArea;
            }
        }

        public double ApertureDiameter
        {
            get
            {
                return client.ApertureDiameter;
            }
        }

        public bool AtHome
        {
            get
            {
                return client.AtHome;
            }
        }

        public bool AtPark
        {
            get
            {
                return client.AtPark;
            }
        }

        public DeviceInterface.IAxisRates AxisRates(DeviceInterface.TelescopeAxes Axis)
        {
            try
            {

                // Create a new axis rate collection object to hold the client's response values
                AxisRates returnValue = new AxisRates(Axis);

                // Query the client and iterate over the returned rate collection
                foreach (IRate axisRate in client.AxisRates((TelescopeAxis)Axis))
                {
                    returnValue.Add(axisRate.Minimum, axisRate.Maximum, TL);
                }

                // Return the rate collection object to the caller
                return returnValue;
            }
            catch (Exception ex)
            {
                TL.LogMessage(LogLevel.Error, "AxisRates", $"Exception: {ex.Message}\r\n{ex}");
                throw;
            }
        }

        public double Azimuth
        {
            get
            {
                return client.Azimuth;
            }
        }

        public bool CanFindHome
        {
            get
            {
                return client.CanFindHome;
            }
        }

        public bool CanMoveAxis(DeviceInterface.TelescopeAxes Axis)
        {
            return client.CanMoveAxis((TelescopeAxis)Axis);
        }

        public bool CanPark
        {
            get
            {
                return client.CanPark;
            }
        }

        public bool CanPulseGuide
        {
            get
            {
                return client.CanPulseGuide;
            }
        }

        public bool CanSetDeclinationRate
        {
            get
            {
                return client.CanSetDeclinationRate;
            }
        }

        public bool CanSetGuideRates
        {
            get
            {
                return client.CanSetGuideRates;
            }
        }

        public bool CanSetPark
        {
            get
            {
                return client.CanSetPark;
            }
        }

        public bool CanSetPierSide
        {
            get
            {
                return client.CanSetPierSide;
            }
        }

        public bool CanSetRightAscensionRate
        {
            get
            {
                return client.CanSetRightAscensionRate;
            }
        }

        public bool CanSetTracking
        {
            get
            {
                return client.CanSetTracking;
            }
        }

        public bool CanSlew
        {
            get
            {
                return client.CanSlew;
            }
        }

        public bool CanSlewAltAz
        {
            get
            {
                return client.CanSlewAltAz;
            }
        }

        public bool CanSlewAltAzAsync
        {
            get
            {
                return client.CanSlewAltAzAsync;
            }
        }

        public bool CanSlewAsync
        {
            get
            {
                return client.CanSlewAsync;
            }
        }

        public bool CanSync
        {
            get
            {
                return client.CanSync;
            }
        }

        public bool CanSyncAltAz
        {
            get
            {
                return client.CanSyncAltAz;
            }
        }

        public bool CanUnpark
        {
            get
            {
                return client.CanUnpark;
            }
        }

        public double Declination
        {
            get
            {
                return client.Declination;
            }
        }

        public double DeclinationRate
        {
            get
            {
                return client.DeclinationRate;
            }
            set
            {
                client.DeclinationRate = value;
            }
        }

        public DeviceInterface.PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            return (DeviceInterface.PierSide)client.DestinationSideOfPier(RightAscension, Declination);
        }

        public bool DoesRefraction
        {
            get
            {
                return client.DoesRefraction;
            }
            set
            {
                client.DoesRefraction = value;
            }
        }

        public DeviceInterface.EquatorialCoordinateType EquatorialSystem
        {
            get
            {
                return (DeviceInterface.EquatorialCoordinateType)client.EquatorialSystem;
            }
        }

        public void FindHome()
        {
            client.FindHome();
            TL.LogMessage("FindHome", "Home found OK");
        }

        public double FocalLength
        {
            get
            {
                return client.FocalLength;
            }
        }

        public double GuideRateDeclination
        {
            get
            {
                return client.GuideRateDeclination;
            }
            set
            {
                client.GuideRateDeclination = value;
            }
        }

        public double GuideRateRightAscension
        {
            get
            {
                return client.GuideRateRightAscension;
            }
            set
            {
                client.GuideRateRightAscension = value;
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                return client.IsPulseGuiding;
            }
        }

        public void MoveAxis(DeviceInterface.TelescopeAxes Axis, double Rate)
        {
            client.MoveAxis((TelescopeAxis)Axis, Rate);
        }

        public void Park()
        {
            client.Park();
            TL.LogMessage("Park", "Parked OK");
        }

        public void PulseGuide(DeviceInterface.GuideDirections Direction, int Duration)
        {
            client.PulseGuide((GuideDirection)Direction, Duration);
        }

        public double RightAscension
        {
            get
            {
                return client.RightAscension;
            }
        }

        public double RightAscensionRate
        {
            get
            {
                return client.RightAscensionRate;
            }
            set
            {
                client.RightAscensionRate = value;
            }
        }

        public void SetPark()
        {
            client.SetPark();
            TL.LogMessage("SetPark", "Park set OK");
        }

        public DeviceInterface.PierSide SideOfPier
        {
            get
            {
                return (DeviceInterface.PierSide)client.SideOfPier;
            }
            set
            {
                client.SideOfPier = (PointingState)value;
            }
        }

        public double SiderealTime
        {
            get
            {
                return client.SiderealTime;
            }
        }

        public double SiteElevation
        {
            get
            {
                return client.SiteElevation;
            }
            set
            {
                client.SiteElevation = value;
            }
        }

        public double SiteLatitude
        {
            get
            {
                return client.SiteLatitude;
            }
            set
            {
                client.SiteLatitude = value;
            }
        }

        public double SiteLongitude
        {
            get
            {
                return client.SiteLongitude;
            }
            set
            {
                client.SiteLongitude = value;
            }
        }

        public short SlewSettleTime
        {
            get
            {
                return client.SlewSettleTime;
            }
            set
            {
                client.SlewSettleTime = value;
            }
        }

        public void SlewToAltAz(double azimuth, double altitude)
        {
            client.SlewToAltAz(azimuth, altitude);
        }

        public void SlewToAltAzAsync(double azimuth, double altitude)
        {
            client.SlewToAltAzAsync(azimuth, altitude);
        }

        public void SlewToCoordinates(double rightAscension, double declination)
        {
            client.SlewToCoordinates(rightAscension, declination);
        }

        public void SlewToCoordinatesAsync(double rightAscension, double declination)
        {
            client.SlewToCoordinatesAsync(rightAscension, declination);
        }

        public void SlewToTarget()
        {
            client.SlewToTarget();
            TL.LogMessage("SlewToTarget", "Slew completed OK");
        }

        public void SlewToTargetAsync()
        {
            client.SlewToTargetAsync();
            TL.LogMessage("SlewToTargetAsync", "Slew completed OK");
        }

        public bool Slewing
        {
            get
            {
                return client.Slewing;
            }
        }

        public void SyncToAltAz(double azimuth, double altitude)
        {
            client.SyncToAltAz(azimuth, altitude);
        }

        public void SyncToCoordinates(double rightAscension, double declination)
        {
            client.SyncToCoordinates(rightAscension, declination);
        }

        public void SyncToTarget()
        {
            client.SyncToTarget();
            TL.LogMessage("SyncToTarget", "Sync completed OK");
        }

        public double TargetDeclination
        {
            get
            {
                return client.TargetDeclination;
            }
            set
            {
                client.TargetDeclination = value;
            }
        }

        public double TargetRightAscension
        {
            get
            {
                return client.TargetRightAscension;
            }
            set
            {
                client.TargetRightAscension = value;
            }
        }

        public bool Tracking
        {
            get
            {
                return client.Tracking;
            }
            set
            {
                client.Tracking = value;
            }
        }

        public DeviceInterface.DriveRates TrackingRate
        {
            get
            {
                return (DeviceInterface.DriveRates)client.TrackingRate;
            }
            set
            {
                client.TrackingRate = (DriveRate)value;
            }
        }

        public DeviceInterface.ITrackingRates TrackingRates
        {
            get
            {
                // Create a intermediate list to hold the drive rates from the client because this works better than an array in the following foreach loop
                List<DeviceInterface.DriveRates> ratesList = new List<DeviceInterface.DriveRates>();

                // Get the tracking rates from the client and copy them to the intermediate list
                foreach (DriveRate driveRate in client.TrackingRates)
                {
                    ratesList.Add((DeviceInterface.DriveRates)driveRate);
                }

                // Create a new TrackingRates return object
                TrackingRates trackingRates = new TrackingRates();

                // Populate the TrackingRates object with the list of drive rates
                trackingRates.SetRates(ratesList.ToArray());

                // Return the TrackingRates object
                return trackingRates;
            }
        }

        public DateTime UTCDate
        {
            get
            {
                return client.UTCDate;
            }
            set
            {
                string utcDateString = value.ToString(SharedConstants.ISO8601_DATE_FORMAT_STRING) + "Z";
                TL.LogMessage("UTCDate", "Sending date string: " + utcDateString);
                client.UTCDate = value;
            }
        }

        public void Unpark()
        {
            client.Unpark();
            TL.LogMessage("Unpark", "Unparked OK");
        }

        #endregion

        #region Private properties and methods

        // Useful properties and methods that can be used as required to help with driver development

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!connectedState)
            {
                throw new NotConnectedException($"{driverDisplayName} ({driverProgId}) is not connected: {message}");
            }
        }

        /// <summary>
        /// Log helper function that writes informational messages to the driver
        /// </summary>
        /// <param name="identifier">Identifier such as method name</param>
        /// <param name="message">Message to be logged.</param>
        private void LogMessage(string identifier, string message)
        {
            // Write to the log for this specific instance (if enabled by the driver having a TraceLogger instance)
            TL?.LogMessage(LogLevel.Information, identifier, message);
        }

        /// <summary>
        /// Log helper function that writes debug messages to the driver
        /// </summary>
        /// <param name="identifier">Identifier such as method name</param>
        /// <param name="message">Message to be logged.</param>
        private void LogDebug(string identifier, string message)
        {
            // Write to the log for this specific instance (if enabled by the driver having a TraceLogger instance)
            TL?.LogMessage(LogLevel.Debug, identifier, message);
        }

        #endregion
        public DeviceInterface.ComTest GetComTest()
        {
            throw new System.NotImplementedException();
        }

    }
}
