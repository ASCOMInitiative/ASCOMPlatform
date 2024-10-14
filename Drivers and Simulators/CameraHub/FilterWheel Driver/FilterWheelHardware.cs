using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCOM.HostHub
{
    /// <summary>
    /// ASCOM Host Hub main functional class shared by all instances of the driver class.
    /// </summary>
    [HardwareClass()] // Attribute to flag this as a device hardware class that needs to be disposed by the local server when it exits.
    internal static class FilterWheelHardware
    {
        /// <summary>
        /// Type of connection Connect/Disconnect or Connecting=
        /// </summary>
        internal enum ConnectType
        {
            Connect_Disconnect,
            Connected
        }

#if DEBUG
        private static DriverAccess.FilterWheel filterWheelDevice; // Filter wheel device being hosted
#else
        private static dynamic filterWheelDevice; // Filter wheel device being hosted
#endif
        private static string filterWheelDescription = ""; // The value is set by the driver's class initialiser.

        private static List<Guid> uniqueIds = new List<Guid>(); // List of driver instance unique IDs

        private static bool runOnce = false; // Flag to enable "one-off" activities only to run once.
        internal static Util utilities; // ASCOM Utilities object for use as required
        internal static TraceLogger TL; // Local server's trace logger object for diagnostic log with information that you specify

        private static int? interfaceVersion;

        /// <summary>
        /// Initializes a new instance of the device Hardware class.
        /// </summary>
        static FilterWheelHardware()
        {
            try
            {
                // Create the hardware trace logger in the static initialiser.
                // All other initialisation should go in the InitialiseHardware method.
                TL = new TraceLogger("", "HostHub.FilterWheel.Proxy");
                bool logState = Settings.FilterWheelDriverLogging;
                LogMessage("FilterWheelHardware", $"Hosted ProgID: {Settings.FilterWheelHostedProgId}.");
                LogMessage("FilterWheelHardware", $"Log activity: {Settings.FilterWheelDriverLogging}.");
                LogMessage("FilterWheelHardware", $"Debug Trace state: {Settings.FilterWheelHardwareLogging}.");

                LogMessage("FilterWheelHardware", $"Static initialiser completed.");
            }
            catch (Exception ex)
            {
                try { LogMessage("FilterWheelHardware", $"Initialisation exception: {ex}"); } catch { }
                MessageBox.Show($"{ex.Message}", "Exception creating ASCOM.HostHub.FilterWheel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// Place device initialisation code here
        /// </summary>
        /// <remarks>Called every time a new instance of the driver is created.</remarks>
        internal static void InitialiseFilterWheel()
        {
            // This method will be called every time a new ASCOM client loads your driver
            LogMessage("InitialiseFilterWheel", $"Start.");

            // Make sure that "one off" activities are only undertaken once
            if (runOnce == false)
            {
                LogMessage("InitialiseFilterWheel", $"Starting one-off initialisation.");

                filterWheelDescription = FilterWheel.filterWheelDescription; // Get this device's Chooser description

                LogMessage("InitialiseFilterWheel", $"ProgID: {FilterWheel.ProgId}, Description: {filterWheelDescription}");

                utilities = new Util(); //Initialise ASCOM Utilities object

                LogMessage("InitialiseFilterWheel", "Completed basic initialisation");

                // Add your own "one off" device initialisation here e.g. validating existence of hardware and setting up communications

                CreateFilterWheelInstance();

                if (string.IsNullOrEmpty(Settings.FilterWheelHostedProgId))
                    throw new InvalidValueException("The filter wheel ProgID is null or empty");

                LogMessage("InitialiseFilterWheel", $"One-off initialisation complete.");
                runOnce = true; // Set the flag to ensure that this code is not run again
            }
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

            // Test whether the filter wheel is already connected
            if (!filterWheelDevice.Connected) // Filter wheel hardware is not connected so connect
            {
                LogMessage("Connect", $"First connection request - Connecting to hardware...");

                switch (connectType)
                {
                    case ConnectType.Connected:
                        filterWheelDevice.Connected = true;
                        LogMessage("Connect", $"Filter wheel connected OK.");
                        break;

                    case ConnectType.Connect_Disconnect:
                        filterWheelDevice.Connect();
                        LogMessage("Connect", $"Connect completed OK - Connecting: {filterWheelDevice.Connecting}.");
                        break;

                    default:
                        throw new InvalidOperationException($"HostHub.Connect - Unknown connection type: {connectType}");
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
            if (uniqueIds.Count == 0) // No instances remain connected so disconnect the filter wheel device
            {
                LogMessage("Disconnect", $"Last disconnection request - Disconnecting hardware...");

                switch (connectType)
                {
                    case ConnectType.Connected:
                        filterWheelDevice.Connected = false;
                        LogMessage("Disconnect", $"Filter wheel disconnected OK.");
                        break;

                    case ConnectType.Connect_Disconnect:
                        filterWheelDevice.Disconnect();
                        LogMessage("Disconnect", $"Disconnect completed OK - Connecting: {filterWheelDevice.Connecting}.");
                        break;

                    default:
                        throw new InvalidOperationException($"HostHub.Connect - Unknown connection type: {connectType}");
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
        /// IFilterWheelV3 and later Connecting property
        /// </summary>
        public static bool Connecting
        {
            get
            {
                return filterWheelDevice.Connecting;
            }
        }

        /// <summary>
        /// IFilterWheelV3 and later DeviceState property
        /// </summary>
        public static IStateValueCollection DeviceState
        {
            get
            {
                return filterWheelDevice.DeviceState;
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

        public static void CreateFilterWheelInstance()
        {
            // Remove any current instance and replace with a new one
            if (!(filterWheelDevice is null)) // There is an existing instance
            {
                try { filterWheelDevice.Connected = false; } catch { }

                try { filterWheelDevice.Dispose(); } catch { }

                try
                {
                    int remainingCount;

                    do
                    {
                        remainingCount = Marshal.ReleaseComObject(filterWheelDevice);
                        LogMessage("CreateFilterWheelInstance", $"Released COM object wrapper, remaining count: {remainingCount}.");
                    } while (remainingCount > 0);
                }
                catch { }

                filterWheelDevice = null;

                // ALlow some time to dispose of the driver
                System.Threading.Thread.Sleep(1000);
            }
            try
            {
                // Create an instance of the filter wheel
                try
                {
#if DEBUG
                    LogMessage("CreateFilterWheelInstance", $"Creating DriverAccess FilterWheel device.");
                    filterWheelDevice = new DriverAccess.FilterWheel(hostedFilterWheelProgId);
#else
                    // Get the Type of this ProgID
                    Type filterWheelType = Type.GetTypeFromProgID(Settings.FilterWheelHostedProgId);
                    LogMessage("CreateFilterWheelInstance", $"Created Type for ProgID: {Settings.FilterWheelHostedProgId} OK.");
                    filterWheelDevice = Activator.CreateInstance(filterWheelType);
#endif
                    LogMessage("CreateFilterWheelInstance", $"Created COM object for ProgID: {Settings.FilterWheelHostedProgId} OK.");
                }
                catch (Exception ex1)
                {
                    throw new InvalidOperationException($"Unable to create an instance of the filter wheel with ProgID {Settings.FilterWheelHostedProgId}: {ex1.Message}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unable to create Type for ProgID {Settings.FilterWheelHostedProgId}: {ex.Message}");
            }
        }

        // PUBLIC COM INTERFACE IMPLEMENTATION

        #region Common properties and methods.

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        public static ArrayList SupportedActions
        {
            get
            {
                ArrayList actions = filterWheelDevice.SupportedActions;
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
            return filterWheelDevice.Action(actionName, actionParameters);
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
            filterWheelDevice.CommandBlind(command, raw);
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
            return filterWheelDevice.CommandBool(command, raw);
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
            return filterWheelDevice.CommandString(command, raw);
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
            try { LogMessage("FilterWheelHardware.Dispose", $"Disposing of assets and closing down."); } catch { }

            if (!(filterWheelDevice is null))
            {
#if DEBUG
                try { filterWheelDevice.Dispose(); } catch (Exception) { }
                try { LogMessage("FilterWheelHardware.Dispose", $"Disposed DriverAccess filter wheel object."); } catch { }
                try { filterWheelDevice = null; } catch (Exception) { }
#else
                try { Marshal.ReleaseComObject(filterWheelDevice); } catch (Exception) { }
                try { LogMessage("FilterWheelHardware.Dispose", $"Released filter wheel COM object."); } catch { }
                try { filterWheelDevice = null; } catch (Exception) { }
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
                string description = filterWheelDevice.Description;
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
                string driverInfo = filterWheelDevice.DriverInfo;
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
                string driverVersion = filterWheelDevice.DriverVersion;
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
                short interfaceVersion = filterWheelDevice.InterfaceVersion;
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
                string name = filterWheelDevice.Name;
                LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region IFilterWheel implementation

        public static int[] FocusOffsets
        {
            get
            {
                int[] focusOffsets = filterWheelDevice.FocusOffsets;
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

        public static string[] Names
        {
            get
            {
                string[] names= filterWheelDevice.Names;
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

        public static short Position
        {
            get
            {
                short position = filterWheelDevice.Position;
                LogMessage("Position Get", position.ToString());
                return position;
            }

            set
            {
                filterWheelDevice.Position = value;
                LogMessage("Position Set", value.ToString());
            }
        }

        #endregion

        #region Private properties and methods

        /// <summary>
        /// Release memory allocated to the large arrays on the large object heap.
        /// </summary>
        private static void ReleaseArrayMemory()
        {
            // Clear out any previous memory allocations
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(2, GCCollectionMode.Forced, true, true);
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private static void CheckConnected(string message)
        {
            if (!filterWheelDevice.Connected)
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
            int iVersion = filterWheelDevice.InterfaceVersion;

            // Check whether the device is connected
            if (filterWheelDevice.Connected) // Filter wheel is connected so save the value for future use
            {
                interfaceVersion = InterfaceVersion;
                return interfaceVersion.Value;
            }
            else // Filter wheel is not connected so return the reported value but don't save it
            {
                return iVersion;
            }
        }

        #endregion
    }
}
