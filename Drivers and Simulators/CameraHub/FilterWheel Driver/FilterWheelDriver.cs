using ASCOM.DeviceInterface;
using ASCOM.LocalServer;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCOM.CameraHub
{
    /// <summary>
    /// ASCOM Driver for the Camera Hub.
    /// </summary>
    [ComVisible(true)]
    [Guid("B26F2567-59F9-4A85-AF02-9AD44B752322")]
    [ProgId("ASCOM.CameraHub.FilterWheel")]
    [ServedClassName("ASCOM Camera Hub Filter Wheel")] // Driver description that appears in the Chooser, customise as required
    [ClassInterface(ClassInterfaceType.None)]
    public class FilterWheel : ReferenceCountedObjectBase, IFilterWheelV3, IDisposable
    {
        internal static string filterWheelDescription; // Chooser descriptive text. The value is retrieved from the ServedClassName attribute in the class initialiser.

        private Guid uniqueId; // A unique ID for this instance of the driver

        internal TraceLogger tl; // Trace logger object to hold diagnostic information just for this instance of the driver, as opposed to the local server's log, which includes activity from all driver instances.
        private bool disposedValue;

        #region Initialisation and Dispose

        static FilterWheel()
        {
            // Pull the ProgID from the ProgID class attribute.
            Attribute attr = Attribute.GetCustomAttribute(typeof(FilterWheel), typeof(ProgIdAttribute));
            FilterWheelProgId = ((ProgIdAttribute)attr).Value ?? "PROGID NOT SET!";  // Get the driver ProgIDfrom the ProgID attribute.
                                                                                // Pull the display name from the ServedClassName class attribute.
            attr = Attribute.GetCustomAttribute(typeof(FilterWheel), typeof(ServedClassNameAttribute));
            filterWheelDescription = ((ServedClassNameAttribute)attr).DisplayName ?? "DISPLAY NAME NOT SET!";  // Get the driver description that displays in the ASCOM Chooser from the ServedClassName attribute.

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ASCOM.FilterWheel"/> class. Must be public to successfully register for COM.
        /// </summary>
        public FilterWheel()
        {
            try
            {
                // Pull the ProgID from the ProgID class attribute.
                Attribute attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ProgIdAttribute));
                FilterWheelProgId = ((ProgIdAttribute)attr).Value ?? "PROGID NOT SET!";  // Get the driver ProgIDfrom the ProgID attribute.

                // Pull the display name from the ServedClassName class attribute.
                attr = Attribute.GetCustomAttribute(this.GetType(), typeof(ServedClassNameAttribute));
                filterWheelDescription = ((ServedClassNameAttribute)attr).DisplayName ?? "DISPLAY NAME NOT SET!";  // Get the driver description that displays in the ASCOM Chooser from the ServedClassName attribute.

                // LOGGING CONFIGURATION
                // By default all driver logging will appear in Hardware log file
                // If you would like each instance of the driver to have its own log file as well, uncomment the lines below

                tl = new TraceLogger("", "CameraHub.FilterWheel.Driver"); // Remove the leading ASCOM. from the ProgId because this will be added back by TraceLogger.
                SetTraceState();

                // Initialise the hardware if required
                FilterWheelHardware.InitialiseFilterWheel();

                LogMessage("FilterWheel", "Starting driver initialisation");
                LogMessage("FilterWheel", $"ProgID: {FilterWheelProgId}, Description: {filterWheelDescription}");

                // Create a unique ID to identify this driver instance
                uniqueId = Guid.NewGuid();

                LogMessage("FilterWheel", "Completed initialisation");
            }
            catch (Exception ex)
            {
                LogMessage("FilterWheel", $"Initialisation exception: {ex}");
                MessageBox.Show($"{ex.Message}", "Exception creating ASCOM.CameraHub.FilterWheel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Class destructor called automatically by the .NET runtime when the object is finalised in order to release resources that are NOT managed by the .NET runtime.
        /// </summary>
        /// <remarks>See the Dispose(bool disposing) remarks for further information.</remarks>
        ~FilterWheel()
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

        // PUBLIC COM INTERFACE FILTERWHEEL IMPLEMENTATION

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            Server.SetupDialog();
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
                    ArrayList actions = FilterWheelHardware.SupportedActions;
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
                string actionResponse = FilterWheelHardware.Action(actionName, actionParameters);
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
                FilterWheelHardware.CommandBlind(command, raw);
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
                LogMessage("CommandBlind", $"Calling method - Command: {command}, Raw: {raw}");
                bool commandBoolResponse = FilterWheelHardware.CommandBool(command, raw);
                LogMessage("CommandBlind", $"Returning: {commandBoolResponse}.");
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
                string commandStringResponse = FilterWheelHardware.CommandString(command, raw);
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
                    bool connectedState = FilterWheelHardware.IsConnected(uniqueId);
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
                        FilterWheelHardware.Connect(uniqueId, FilterWheelHardware.ConnectType.Connected);
                    }
                    else // Request to disconnect
                    {
                        LogMessage("Connected Set", "Disconnecting from device");
                        FilterWheelHardware.Disconnect(uniqueId, FilterWheelHardware.ConnectType.Connected);
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
                    string description = FilterWheelHardware.Description;
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
                    string driverInfo = FilterWheelHardware.DriverInfo;
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
                    string driverVersion = FilterWheelHardware.DriverVersion;
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
                    short interfaceVersion = FilterWheelHardware.InterfaceVersion;
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
                    string name = FilterWheelHardware.Name;
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

        #region IFilterWheel Implementation

        public int[] FocusOffsets
        {
            get
            {
                int[] focusOffsets = FilterWheelHardware.FocusOffsets;
                if (focusOffsets == null)
                {
                    LogMessage("FocusOffsets Get", $"Received a null value.");
                    return focusOffsets;
                }
                else
                {
                    LogMessage("FocusOffsets Get", $"Received {focusOffsets.Length} offsets.");
                    return focusOffsets;
                }
            }
        }

        public string[] Names
        {
            get
            {
                string[] names = FilterWheelHardware.Names;
                if (names == null)
                {
                    LogMessage("Names Get", $"Received a null value.");
                    return names;
                }
                else
                {
                    LogMessage("Names Get", $"Received {names.Length} names.");
                    return names;
                }
            }
        }

        public short Position
        {
            get
            {
                try
                {
                    short position = FilterWheelHardware.Position;
                    LogMessage("Position Get", position.ToString());
                    return position;
                }
                catch (Exception ex)
                {
                    LogMessage("Position", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }

            set
            {
                FilterWheelHardware.Position = value;
                LogMessage("Position Set", value.ToString());
            }
        }

        #endregion

        #region Private properties and methods

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!FilterWheelHardware.IsConnected(uniqueId))
            {
                throw new NotConnectedException($"{filterWheelDescription} ({FilterWheelProgId}) is not connected: {message}");
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
            if (tl != null)
            {
                tl.LogMessageCrLf(identifier, message); // Write to the individual driver log
            }

            // Write to the common hardware log shared by all running instances of the driver.
            FilterWheelHardware.LogMessage(identifier, message); // Write to the local server logger
        }

        /// <summary>
        /// Read the trace state from the driver's Profile and enable / disable the trace log accordingly.
        /// </summary>
        private void SetTraceState()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "FilterWheel";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(FilterWheelProgId, FilterWheelHardware.TRACE_STATE_PROFILE_NAME, string.Empty, FilterWheelHardware.TRACE_STATE_DEFAULT));
            }
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
            if (Common.DeviceInterfaces.DeviceCapabilities.HasConnectAndDeviceState(Common.DeviceTypes.FilterWheel, FilterWheelHardware.GetInterfaceVersion())) // Platform 7 or later device so Connect
            {
                LogMessage("Connect", "Issuing Connect command");
                FilterWheelHardware.Connect(uniqueId, FilterWheelHardware.ConnectType.Connect_Disconnect);
                return;
            }

            // Platform 6 or earlier so reject the method call
            LogMessage("Connect", "Throwing a MethodNotImplementedException because the connected filter wheel is a Platform 6 or earlier device.");
            throw new MethodNotImplementedException($"Connect is not implemented in this IFilterWheelV{FilterWheelHardware.GetInterfaceVersion()} device.");
        }

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        public void Disconnect()
        {
            // Check whether the connected device is Platform 7 or later
            if (Common.DeviceInterfaces.DeviceCapabilities.HasConnectAndDeviceState(Common.DeviceTypes.FilterWheel, FilterWheelHardware.GetInterfaceVersion())) // Platform 7 or later device so Disconnect
            {
                LogMessage("Disconnect", "Issuing Disconnect command");
                FilterWheelHardware.Disconnect(uniqueId, FilterWheelHardware.ConnectType.Connect_Disconnect);
                return;
            }

            // Platform 6 or earlier so reject the method call
            LogMessage("Disconnect", "Throwing a MethodNotImplementedException because the connected filter wheel is a Platform 6 or earlier device.");
            throw new MethodNotImplementedException($"Disconnect is not implemented in this IFilterWheelV{FilterWheelHardware.GetInterfaceVersion()} device.");
        }

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        public bool Connecting
        {
            get
            {
                // Check whether the connected device is Platform 7 or later
                if (Common.DeviceInterfaces.DeviceCapabilities.HasConnectAndDeviceState(Common.DeviceTypes.FilterWheel, FilterWheelHardware.GetInterfaceVersion())) // Platform 7 or later device so call Connecting
                {
                    LogMessage("Connecting", "Calling Connecting property");
                    return FilterWheelHardware.Connecting;
                }

                // Platform 6 or earlier so reject the method call
                LogMessage("Connecting", "Throwing a PropertyNotImplementedException because the connected filter wheel is a Platform 6 or earlier device.");
                throw new PropertyNotImplementedException($"Connecting is not implemented in this IFilterWheelV{FilterWheelHardware.GetInterfaceVersion()} device.");
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
                if (Common.DeviceInterfaces.DeviceCapabilities.HasConnectAndDeviceState(Common.DeviceTypes.FilterWheel, FilterWheelHardware.GetInterfaceVersion())) // Platform 7 or later device so call Connecting
                {
                    LogMessage("DeviceState", "Calling DeviceState property");
                    return FilterWheelHardware.DeviceState;
                }

                // Platform 6 or earlier so return an empty ArrayList because this feature isn't supported
                LogMessage("DeviceState", "Returning an empty ArrayList because this feature isn't supported.");
                return new StateValueCollection();
            }
        }

        #endregion

        #region Internal members

        internal static string FilterWheelProgId { get; private set; }

        #endregion
    }
}
