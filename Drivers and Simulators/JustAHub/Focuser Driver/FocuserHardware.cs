using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace ASCOM.JustAHub
{
    /// <summary>
    /// ASCOM JustAHub Focuser main functional class shared by all instances of the driver class.
    /// </summary>
    [HardwareClass()] // Attribute to flag this as a device hardware class that needs to be disposed by the local server when it exits.
    internal static class FocuserHardware
    {
#if DEBUG
        private static DriverAccess.Focuser device; // Focuser device being hosted
#else
        private static dynamic device; // Focuser device being hosted
#endif

        private static readonly List<Guid> uniqueIds = new List<Guid>(); // List of driver instance unique IDs

        private static bool runOnce = false; // Flag to enable "one-off" activities only to run once.
        internal static Util utilities; // ASCOM Utilities object for use as required
        internal static TraceLogger TL; // Local server's trace logger object for diagnostic log with information that you specify

        private static int? interfaceVersion;

        /// <summary>
        /// Initializes a new instance of the device Hardware class.
        /// </summary>
        static FocuserHardware()
        {
            try
            {
                // Create the hardware trace logger in the static initialiser.
                // All other initialisation should go in the InitialiseHardware method.
                TL = new TraceLogger("", "JustAHub.Focuser.Proxy")
                {
                    Enabled = Settings.FocuserHardwareLogging
                };

                LogMessage("JustAHub", $"Static initialiser completed.");
            }
            catch (Exception ex)
            {
                try { LogMessage("JustAHub", $"Initialisation exception: {ex}"); } catch { }
                MessageBox.Show($"{ex.Message}", $"Exception creating {Focuser.ChooserDescription} ({Focuser.ProgId})", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// Place device initialisation code here
        /// </summary>
        /// <remarks>Called every time a new instance of the driver is created.</remarks>
        internal static void Initialise()
        {
            // This method will be called every time a new ASCOM client loads your driver
            LogMessage("Initialise", $"Start.");

            // Make sure that "one off" activities are only undertaken once
            if (!runOnce)
            {
                LogMessage("Initialise", $"Starting one-off initialisation.");

                if (string.IsNullOrEmpty(Settings.FocuserHostedProgId))
                    throw new InvalidValueException("The configured Focuser ProgID in JustAHub is null or empty");

                LogMessage("Initialise", $"Hosted ProgID: {Settings.FocuserHostedProgId}");

                //Initialise ASCOM Utilities object
                utilities = new Util();

                CreateInstance();
                LogMessage("Initialise", "Completed one-off initialisation");

                // Set the flag to ensure that this code is not run again
                runOnce = true;
            }
            else
            {
                LogMessage("Initialise", "One-off initialisation has already run.");
            }

            LogMessage("Initialise", $"Complete.");
        }

        /// <summary>
        /// Connect to the hardware if not already connected
        /// </summary>
        /// <param name="uniqueId">Unique ID identifying the calling driver instance.</param>
        /// <remarks>
        /// The unique ID is stored to record that the driver instance is connected and to ensure that multiple calls from the same driver are ignored.
        /// If this is the first driver instance to connect, the hardware link to the device is established
        /// </remarks>
        public static void Connect(Guid uniqueId, ConnectType connectType)
        {
            LogMessage("Connect", $"Unique ID: {uniqueId}, Connect type: {connectType}");

            // Check whether this driver instance has already connected
            if (uniqueIds.Contains(uniqueId)) // Instance already connected
            {
                // Ignore the request, the unique ID is already in the list
                LogMessage("Connect", $"Ignoring request to connect because the device is already connected.");
                return;
            }

            // Driver instance not yet connected

            // Test whether the Focuser is already connected
            if (!device.Connected) // Focuser hardware is not connected so connect
            {
                LogMessage("Connect", $"First connection request - Connecting to hardware...");

                switch (connectType)
                {
                    case ConnectType.Connected:
                        device.Connected = true;
                        LogMessage("Connect", $"Focuser connected OK.");
                        break;

                    case ConnectType.Connect_Disconnect:
                        device.Connect();
                        LogMessage("Connect", $"Connect completed OK - Connecting: {device.Connecting}.");
                        break;

                    default:
                        throw new InvalidOperationException($"JustAHub.Connect - Unknown connection type: {connectType}");
                }
            }

            // Add the driver unique ID to the connected list
            uniqueIds.Add(uniqueId);
            LogMessage("Connect", $"Unique id {uniqueId} added to the connection list.");

            // Log the current connected state
            LogMessage("Connect", $"Currently connected driver ids:");
            foreach (Guid id in uniqueIds)
            {
                LogMessage("Connect", $" ID {id} is connected");
            }
        }

        /// <summary>
        /// Disconnect from the hardware if this is the last driver instance that is connected
        /// </summary>
        /// <param name="uniqueId">Unique ID identifying the calling driver instance.</param>
        /// <remarks>
        /// The list of connected driver instance IDs is queried to determine whether this driver instance is connected and, if so, it is removed from the connection list. 
        /// The unique ID ensures that multiple calls from the same driver are ignored.
        /// If this is the last connected driver instance, the link to the device hardware is disconnected.
        /// </remarks>
        public static void Disconnect(Guid uniqueId, ConnectType connectType)
        {
            LogMessage("Disconnect", $"Unique ID: {uniqueId}, Disconnect type: {connectType}");

            if (!uniqueIds.Contains(uniqueId)) // Instance already disconnected
            {
                // Ignore the request, the unique ID is absent from the list
                LogMessage("Disconnect", $"Ignoring request to disconnect because the device is already disconnected.");
                return;
            }

            // Driver instance currently connected

            // Remove the driver unique ID from the connected list
            uniqueIds.Remove(uniqueId);
            LogMessage("Disconnect", $"Unique id {uniqueId} removed from the connection list.");

            // Test whether any instances are still connected
            if (uniqueIds.Count == 0) // No instances remain connected so disconnect the Focuser device
            {
                LogMessage("Disconnect", $"Last disconnection request - Disconnecting hardware...");

                switch (connectType)
                {
                    case ConnectType.Connected:
                        device.Connected = false;
                        LogMessage("Disconnect", $"Focuser disconnected OK.");
                        break;

                    case ConnectType.Connect_Disconnect:
                        device.Disconnect();
                        LogMessage("Disconnect", $"Disconnect completed OK - Connecting: {device.Connecting}.");
                        break;

                    default:
                        throw new InvalidOperationException($"JustAHub.Connect - Unknown connection type: {connectType}");
                }
            }

            // Log the current connected state
            if (uniqueIds.Count > 0)
            {
                LogMessage("Disconnect", $"Remaining connected driver IDs:");
                foreach (Guid id in uniqueIds)
                {
                    LogMessage("Disconnect", $"  ID {id} is connected");
                }
            }
            else
                LogMessage("Disconnect", $"No connected devices.");
        }

        /// <summary>
        /// IFocuserV4 and later Connecting property
        /// </summary>
        public static bool Connecting
        {
            get
            {
                return device.Connecting;
            }
        }

        /// <summary>
        /// IFocuserV4 and later DeviceState property
        /// </summary>
        public static IStateValueCollection DeviceState
        {
            get
            {
                return device.DeviceState;
            }
        }

        /// <summary>
        /// Test whether a driver instance is connected, identified by its unique ID
        /// </summary>
        /// <param name="uniqueId">The driver's unique ID</param>
        /// <returns>True if the driver instance is connected</returns>
        public static bool IsConnected(Guid uniqueId)
        {
            return uniqueIds.Contains(uniqueId);
        }

        public static void CreateInstance()
        {
            // Remove any current instance and replace with a new one
            if (!(device is null)) // There is an existing instance
            {
                try { device.Connected = false; } catch { }

                try { device.Dispose(); } catch { }

                try
                {
                    int remainingCount;

                    do
                    {
                        remainingCount = Marshal.ReleaseComObject(device);
                        LogMessage("CreateInstance", $"Released COM object wrapper, remaining count: {remainingCount}.");
                    } while (remainingCount > 0);
                }
                catch { }

                device = null;

                // Allow some time to dispose of the driver
                System.Threading.Thread.Sleep(1000);
            }
            try
            {
                // Create an instance of the Focuser
                try
                {
#if DEBUG
                    LogMessage("CreateInstance", $"Creating DriverAccess Focuser device.");
                    device = new DriverAccess.Focuser(Settings.FocuserHostedProgId);
#else
                    // Get the Type of this ProgID
                    Type type = Type.GetTypeFromProgID(Settings.FocuserHostedProgId);
                    LogMessage("CreateInstance", $"Created Type for ProgID: {Settings.FocuserHostedProgId} OK.");
                    device = Activator.CreateInstance(type);
#endif
                    LogMessage("CreateInstance", $"Created COM object for ProgID: {Settings.FocuserHostedProgId} OK.");
                }
                catch (Exception ex1)
                {
                    throw new InvalidOperationException($"Unable to create an instance of the Focuser with ProgID {Settings.FocuserHostedProgId}: {ex1.Message}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unable to create Type for ProgID {Settings.FocuserHostedProgId}: {ex.Message}");
            }
        }

        #region Common properties and methods.

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        public static ArrayList SupportedActions
        {
            get
            {
                ArrayList actions = device.SupportedActions;
                LogMessage("SupportedActions Get", $"Returning ArrayList of length: {actions.Count}");
                return actions;
            }
        }

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        public static string Action(string actionName, string actionParameters)
        {
            return device.Action(actionName, actionParameters);
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
        public static void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            device.CommandBlind(command, raw);
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
        public static bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            return device.CommandBool(command, raw);
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
        public static string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            return device.CommandString(command, raw);
        }

        /// <summary>
        /// Deterministically release both managed and unmanaged resources that are used by this class.
        /// </summary>
        /// <remarks>
        /// TODO: Release any managed or unmanaged resources that are used in this class.
        /// 
        /// Do not call this method from the Dispose method in your driver class.
        ///
        /// This is because this hardware class is decorated with the <see cref="HardwareClassAttribute"/> attribute and this Dispose() method will be called 
        /// automatically by the  local server executable when it is irretrievably shutting down. This gives you the opportunity to release managed and unmanaged 
        /// resources in a timely fashion and avoid any time delay between local server close down and garbage collection by the .NET runtime.
        ///
        /// For the same reason, do not call the SharedResources.Dispose() method from this method. Any resources used in the static shared resources class
        /// itself should be released in the SharedResources.Dispose() method as usual. The SharedResources.Dispose() method will be called automatically 
        /// by the local server just before it shuts down.
        /// 
        /// </remarks>
        public static void Dispose()
        {
            try { LogMessage("JustAHub.Dispose", $"Disposing of assets and closing down."); } catch { }

            if (!(device is null))
            {
#if DEBUG
                try { device.Dispose(); } catch (Exception) { }
                try { LogMessage("JustAHub.Dispose", $"Disposed DriverAccess Focuser object."); } catch { }
                try { device = null; } catch (Exception) { }
#else
                try { Marshal.ReleaseComObject(device); } catch (Exception) { }
                try { LogMessage("JustAHub.Dispose", $"Released Focuser COM object."); } catch { }
                try { device = null; } catch (Exception) { }
#endif
            }

            try
            {
                // Clean up the trace logger and utility objects
                TL.Enabled = false;
                TL.Dispose();
                TL = null;
            }
            catch { }

            try
            {
                utilities.Dispose();
                utilities = null;
            }
            catch { }

        }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        public static string Description
        {
            get
            {
                string description = device.Description;
                LogMessage("Description Get", description);
                return description;
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        public static string DriverInfo
        {
            get
            {
                string driverInfo = device.DriverInfo;
                LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver formatted as 'm.n'.
        /// </summary>
        public static string DriverVersion
        {
            get
            {
                string driverVersion = device.DriverVersion;
                LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        /// <summary>
        /// The interface version number that this device supports.
        /// </summary>
        public static short InterfaceVersion
        {
            get
            {
                short interfaceVersion = device.InterfaceVersion;
                LogMessage("InterfaceVersion Get", interfaceVersion.ToString());
                return interfaceVersion;
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        public static string Name
        {
            get
            {
                string name = device.Name;
                LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region IFocuser Implementation

        public static bool Absolute => device.Absolute;

        public static bool IsMoving => device.IsMoving;

        public static int MaxIncrement => device.MaxIncrement;

        public static int MaxStep => device.MaxStep;

        public static int Position => device.Position;

        public static double StepSize => device.StepSize;

        public static bool TempComp { get => device.TempComp; set => device.TempComp = value; }

        public static bool TempCompAvailable => device.TempCompAvailable;

        public static double Temperature => device.Temperature;

        public static void Halt()
        {
            device.Halt();
        }

        public static void Move(int position)
        {
            device.Move(position);
        }

        #endregion

        #region Private properties and methods

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private static void CheckConnected(string message)
        {
            if (!device.Connected)
            {
                throw new NotConnectedException(message);
            }
        }

        /// <summary>
        /// Log helper function that takes identifier and message strings
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        internal static void LogMessage(string identifier, string message)
        {
            TL.LogMessageCrLf(identifier, message);
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            LogMessage(identifier, msg);
        }

        /// <summary>
        /// Get the device interface version
        /// </summary>
        /// <returns></returns>
        internal static int GetInterfaceVersion()
        {
            // Check whether we already know the device's interface version
            if (interfaceVersion.HasValue) // We do have the interface version so return it
            {
                return interfaceVersion.Value;
            }

            // We don't have the interface version so get it from the device but only store it if we are connected because it may change when connected

            // Get the interface version
            int iVersion = device.InterfaceVersion;

            // Check whether the device is connected
            if (device.Connected) // Focuser is connected so save the value for future use
            {
                interfaceVersion = InterfaceVersion;
                return interfaceVersion.Value;
            }
            else // Focuser is not connected so return the reported value but don't save it
            {
                return iVersion;
            }
        }

        #endregion
    }
}
