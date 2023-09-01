using ASCOM.Alpaca.Clients;
using ASCOM.Common;
using ASCOM.Common.DeviceInterfaces;
using ASCOM.Common.Interfaces;
using ASCOM.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCOM.DynamicClients
{
    /// <summary>
    /// Driver to access the Alpaca SafetyMonitor simulator.
    /// </summary>
    public class SafetyMonitor : ReferenceCountedObjectBase, DeviceInterface.ISafetyMonitorV3, IDisposable
    {
        // Set the device type of this device
        private const DeviceTypes deviceType = DeviceTypes.SafetyMonitor;

        internal static string driverProgId; // ASCOM DeviceID (COM ProgID) for this driver, the value is retrieved from the ServedClassName attribute in the class initialiser.
        internal static string driverDisplayName; // The value is retrieved from the ServedClassName attribute in the class initialiser.

        internal bool connectedState; // The connected state from this driver's perspective)
        internal TraceLogger TL; // Trace logger object to hold diagnostic information just for this instance of the driver, as opposed to the local server's log, which includes activity from all driver instances.
        private bool disposedValue;

        private AlpacaSafetyMonitor client;

        DriverState state; // State variable for this driver

        #region Initialisation and Dispose

        /// <summary>
        /// Initializes a new instance of the <see cref="SafetyMonitor"/> class. Must be public to successfully register for COM.
        /// </summary>
        public SafetyMonitor()
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
                state = new DriverState(driverProgId, deviceType, driverDisplayName);

                // Set up a trace logger
                TL = new TraceLogger(state.ProgId[6..], false)
                {
                    Enabled = state.TraceState
                };
                if (state.DebugTraceState)
                    TL.SetMinimumLoggingLevel(LogLevel.Debug);

                LogMessage(nameof(SafetyMonitor), $"Starting driver initialisation for ProgID: {driverProgId}, Description: {driverDisplayName}");

                // Initialise connected to false
                connectedState = false;

                // Create a client
                client = Server.GetClient<AlpacaSafetyMonitor>(state, TL);
                LogMessage(nameof(SafetyMonitor), $"Alpaca client created successfully");

                LogMessage(nameof(SafetyMonitor), "Completed initialisation");
            }
            catch (Exception ex)
            {
                LogMessage(nameof(SafetyMonitor), $"Initialisation exception: {ex}");
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
        ~SafetyMonitor()
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
                AlpacaSafetyMonitor newclient = Server.SetupDialogue<AlpacaSafetyMonitor>(state, TL);
                if (newclient != null)
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

                    // Replace client
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
                    ArrayList actions = new(client.SupportedActions.ToList<string>());
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
                LogMessage("CommandBlind", $"Calling method - Command: {command}, Raw: {raw}");
                bool commandBoolResponse = client.CommandBool(command, raw);
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
                try
                {
                    // Returns the driver's connection state rather than the local server's connected state, which could be different because there may be other client connections still active.
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
                    if (value == connectedState)
                    {
                        LogMessage("Connected Set", $"Device already in requested state, ignoring request to set Connected to {value}");
                        return;
                    }

                    if (value) // Set Connected = TRUE
                    {
                        if (!client.Connected) // Device is not connected
                        {
                            LogMessage("Connected Set", "Connecting to device");
                            client.Connected = true;
                            connectedState = true;
                            LogMessage("Connected Set", $"Connected to device OK, connected state: {connectedState}");
                        }
                        else // Device is already connected
                        {
                            connectedState = true;
                            LogMessage("Connected Set", $"Client already connected, setting connected state to true, connected state: {connectedState}");
                        }
                    }
                    else // Set Connected = FALSE
                    {
                        connectedState = false;
                        LogMessage("Connected Set", $"Setting Connected to false, connected state: {connectedState}");
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
            if (!client.Connected) // Client is not connected
            {
                // Connect client
                client.Connect();
                do
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                } while (client.Connecting);

                //Set Connected to True
                connectedState = true;
            }
            else // Client is already connected so just set the Connected state to TRUE
            {
                connectedState = true;
            }
        }

        public void Disconnect()
        {
            connectedState = false;
        }

        public bool Connecting => client.Connecting;

        public ArrayList DeviceState
        {
            get
            {
                ArrayList returnValue = new();

                List<StateValue> deviceState = OperationalStateProperty.Clean(client.DeviceState, DeviceTypes.SafetyMonitor, TL);

                LogMessage("DeviceState", $"Received {deviceState.Count} values");
                foreach (StateValue value in deviceState)
                {
                    LogMessage("DeviceState", $"  {value.Name} = {value.Value} - Kind: {value.Value.GetType().Name}");
                    returnValue.Add(value);
                }

                LogMessage("DeviceState", $"Return value has {returnValue.Count} values");

                return returnValue;
            }
        }

        #endregion

        #region ISafetyMonitor Implementation

        /// <summary>
        /// Indicates whether the monitored state is safe for use.
        /// </summary>
        /// <value>True if the state is safe, False if it is unsafe.</value>
        public bool IsSafe
        {
            get
            {
                try
                {
                    // IsSafe is required to deliver false when the device is not connected so there is no need to test whether or not the driver is connected.

                    bool isSafe = client.IsSafe;
                    LogMessage("IsSafe", isSafe.ToString());
                    return isSafe;
                }
                catch (Exception ex)
                {
                    LogMessage("IsSafe", $"Threw an exception: \r\n{ex}");
                    throw;
                }
            }
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
    }
}
