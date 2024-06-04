using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Win32;

using ASCOM.DeviceHub.MvvmMessenger;
using ASCOM.Utilities;

namespace ASCOM.DeviceHub
{
    public static class Server
    {
        #region Private Data

        private static int _objsInUse;                      // Keeps a count on the total number of objects alive.
        private static int _scopesInUse;                    // Keeps a count on the total number of telescopes alive.
        private static int _domesInUse;                     // Keeps a count on the total number of domes alive.
        private static int _focusersInUse;                  // Keeps a count on the total number of focusers alive.
        private static int _serverLocks;                    // Keeps a lock count on this application.
        private static List<Type> _driverTypes;             // Served COM object types
        private static List<ClassFactory> _classFactories;  // Served COM object class factories
        private static readonly string _appId = "{4f90ea04-044f-444e-963e-b52db2a87575}";   // Our AppId
        private static readonly Object _lockObject = new object();
        private const string _uacRegistryKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
        private const string _uacRegistryValue = "EnableLUA";

        private const int GARBAGE_COLLECTION_WAIT = 60000; // Time between regular garbage collections

        private static TraceLogger Logger { get; set; }

        private static GarbageCollection GarbageCollector { get; set; }

        private static MainWindow MainWindow { get; set; }          // Reference to the main view
        private static MainWindowViewModel ViewModel { get; set; }  // Reference to the main view model

        #endregion Private Data

        #region Private Properties

        private static Task GCTask { get; set; } // The garbage collection task

        // Used to end periodic garbage collection.

        private static CancellationTokenSource GCTokenSource { get; set; }

        #endregion Private Properties

        #region Public Properties

        // Used to tell if started by COM or manually

        public static bool StartedByCOM { get; private set; }   // True if server started by COM (-embedding)
        public static int ScopesInUse => _scopesInUse;
        public static int DomesInUse => _domesInUse;
        public static int FocusersInUse => _focusersInUse;

        #endregion Public Properties

        #region Startup Method

        internal static void Startup(string[] args)
        {
            string logId = "Main";

            // Uncomment the following lines to allow the Visual Studio Debugger to be 
            // attached to the server for debugging.

            //int procId = Process.GetCurrentProcess().Id;
            //MessageBox.Show( $"Attach the debugger to process #{procId} now." );

            Logger = new TraceLogger("", "DeviceHubLocalServer")
            {
                // Server logging can be enabled by setting ServerLoggingEnabled to True in the application config file.

                Enabled = Properties.Settings.Default.ServerLoggingEnabled
            };

            Logger.LogMessage(logId, "Server started");

            Globals.UISyncContext = TaskScheduler.FromCurrentSynchronizationContext();

            Logger.LogMessage(logId, $"Loading drivers");

            if (!PopulateListOfAscomDrivers())
            {
                App.Current.Shutdown();

                return;
            }

            Logger.LogMessage(logId, "Processing command-line arguments.");


            if (!ProcessArguments(args))    // Register/Unregister
            {
                App.Current.Shutdown();

                return;
            }

            // Now that we are past the registration code, make sure that we are NOT
            // running As Administrator.

            if (IsElevated && IsUacEnabled)
            {
                MessageBox.Show("You cannot run this application with elevated access...Terminating!");
                App.Current.Shutdown();

                return;
            }

            // Initialize critical member variables.

            Logger.LogMessage(logId, "Initializing veriables");

            _objsInUse = 0;
            _scopesInUse = 0;
            _domesInUse = 0;
            _focusersInUse = 0;
            _serverLocks = 0;

            // Application Startup

            // Initialize our non-U/I services.

            Logger.LogMessage(logId, "Initializing the service injector");

            ServiceInjector.InjectServices();

            // Create Device Hub App Settings and seed with default values.

            Logger.LogMessage(logId, "Initializing persisted settings.");

            AppSettingsManager.CreateInitialAppSettings();

            // Load the saved position of the main window. Do this before the
            // window object is created so that the code-behind can pick up
            // the location.			

            AppSettingsManager.LoadMainWindowSettings();

            // Create the View

            Logger.LogMessage(logId, "Creating the main view");

            MainWindow = new MainWindow();

            // Create the U/I services.

            Logger.LogMessage(logId, "Injecting the user interface services");

            ServiceInjector.InjectUIServices(MainWindow);

            try
            {
                // Create the ViewModel
                // Any errors starting the view models or device managers will raise an exception and short circuit
                // the rest of the startup.

                ViewModel = new MainWindowViewModel();

                Logger.LogMessage(logId, "Setting the data context for the main view");

                MainWindow.DataContext = ViewModel;
                MainWindow.Closing += MainWindow_Closing;

                // Load the saved settings to ensure that everyone up-to-date. Be sure to do this
                // after the main window is created so we can set its location.

                Logger.LogMessage(logId, "Loading the application settings");

                AppSettingsManager.LoadAppSettings();

                Logger.LogMessage(logId, "Loading the device driver settings");

                LoadDeviceSettings();

                // Register the class factories of the served objects

                Logger.LogMessage(logId, "Registering class factories");

                RegisterClassFactories();

                Logger.LogMessage(logId, "Starting garbage collection");

                StartGarbageCollection(GARBAGE_COLLECTION_WAIT);    // Collect garbage periodically

                try
                {
                    Logger.LogMessage(logId, "Starting main view");

                    ShowMainWindow();

                    Logger.LogMessage(logId, "The main view has closed");
                }
                finally
                {
                    Logger.LogMessage(logId, "Saving the application settings");
                    AppSettingsManager.SaveAppSettings();

                    // Revoke the class factories immediately.
                    // Don't wait until the thread has stopped before
                    // we perform revocation!!!

                    RevokeClassFactories();

                    Logger.LogMessage(logId, "Disposing the main view and viewmodel.");

                    MainWindow.DataContext = null;
                    MainWindow = null;
                    ViewModel.Dispose();
                    ViewModel = null;

                    Logger.LogMessage(logId, "Unregistering all services");

                    ServiceContainer.Instance.ClearAllServices();

                    // Now stop the Garbage Collector task.

                    Logger.LogMessage(logId, "Stopping garbage collection");

                    StopGarbageCollection();
                }
            }
            catch (Exception)
            {
                // Any exception from starting the view models or device managers will bring us here. 
                // The exception was already logged via the AppLogger so we have nothing more to do but close the Logger and return.
            }
            finally
            {
                Logger.LogMessage(logId, "Local server is shutting down");
                Logger.Dispose();
            }
        }

        #endregion  Startup Method

        #region Server Lock, Object Counting, and AutoQuit on COM startup

        // Returns the total number of objects alive currently.

        public static int ObjectsCount
        {
            get
            {
                return _objsInUse;
            }
        }

        // This method performs a thread-safe increment of the objects count.

        public static int CountObject(DeviceTypeEnum devType)
        {
            string logId = "CountObject";
            string name = devType.GetDisplayName();

            int retval;

            Logger.LogMessage(logId, $"Called with DeviceType: {devType}");

            // Increment the count of objects for the specified device type.

            if (devType == DeviceTypeEnum.Telescope)
            {
                Logger.LogMessage(logId, $"Current telescope object count: {_scopesInUse}");
                Interlocked.Increment(ref _scopesInUse);
                Logger.LogMessage(logId, $"New telescope object count: {_scopesInUse}");
            }
            else if (devType == DeviceTypeEnum.Dome)
            {
                Logger.LogMessage(logId, $"Current dome object count: {_domesInUse}");
                Interlocked.Increment(ref _domesInUse);
                Logger.LogMessage(logId, $"New dome object count: {_domesInUse}");
            }
            else if (devType == DeviceTypeEnum.Focuser)
            {
                Logger.LogMessage(logId, $"Current focuser object count: {_focusersInUse}");
                Interlocked.Increment(ref _focusersInUse);
                Logger.LogMessage(logId, $"New focuser object count: {_focusersInUse}");
            }

            // Increment the global count of objects.

            Logger.LogMessage(logId, $"Current object count: {_objsInUse}");
            retval = Interlocked.Increment(ref _objsInUse);
            Logger.LogMessage(logId, $"New object count: {_objsInUse}");

            Logger.LogMessage(logId, $"Scopes in use: {_scopesInUse}, Domes in use: {_domesInUse}, Focusers in use: {_focusersInUse}");
            Messenger.Default.Send(new ObjectCountMessage(_scopesInUse, _domesInUse, _focusersInUse));

            return retval;
        }

        // This method performs a thread-safe decrement of the objects count.

        public static int UncountObject(DeviceTypeEnum devType)
        {
            int retval;
            string logId = "UncountObject";

            Logger.LogMessage(logId, $"Called with DeviceType: {devType}");

            // Decrement the count of objects for the specified device type.

            if (devType == DeviceTypeEnum.Telescope)
            {
                Logger.LogMessage(logId, $"Current telescope object count: {_scopesInUse}");
                Interlocked.Decrement(ref _scopesInUse);
                Logger.LogMessage(logId, $"New telescope object count: {_scopesInUse}");
            }
            else if (devType == DeviceTypeEnum.Dome)
            {
                Logger.LogMessage(logId, $"Current dome object count: {_domesInUse}");
                Interlocked.Decrement(ref _domesInUse);
                Logger.LogMessage(logId, $"New dome object count: {_domesInUse}");
            }
            else if (devType == DeviceTypeEnum.Focuser)
            {
                Logger.LogMessage(logId, $"Current focuser object count: {_focusersInUse}");
                Interlocked.Decrement(ref _focusersInUse);
                Logger.LogMessage(logId, $"New focuser object count: {_focusersInUse}");
            }

            // Decrement the global count of objects.

            Logger.LogMessage(logId, $"Current object count: {_objsInUse}");
            retval = Interlocked.Decrement(ref _objsInUse);
            Logger.LogMessage(logId, $"New object count: {_objsInUse}");

            Logger.LogMessage(logId, $"Sending object count message...");
            Messenger.Default.Send(new ObjectCountMessage(_scopesInUse, _domesInUse, _focusersInUse));
            Logger.LogMessage(logId, $"Object count message sent. Returning count: {retval}.");

            return retval;
        }

        // Returns the current server lock count.

        public static int ServerLockCount
        {
            get
            {
                return _serverLocks;
            }
        }

        // This method performs a thread-safe incrementation the 
        // server lock count.

        public static int CountLock()
        {
            // Increment the global lock count of this server.

            Logger.LogMessage("CountLock", $"Current server lock count: {_serverLocks}");
            int newCount = Interlocked.Increment(ref _serverLocks);
            Logger.LogMessage("CountLock", $"New server lock count: {newCount}");

            return newCount;
        }

        // This method performs a thread-safe decrementation the 
        // server lock count.

        public static int UncountLock()
        {
            // Decrement the global lock count of this server.

            Logger.LogMessage("UncountLock", $"Current server lock count: {_serverLocks}");
            int newCount = Interlocked.Decrement(ref _serverLocks);
            Logger.LogMessage("UncountLock", $"New server lock count: {newCount}");

            return newCount;
        }

        // ExitIf() will check to see if the objects count and the server 
        // lock count have both dropped to zero.
        //
        // If so, and if we were started by COM, we fire a Shutdown event to terminate
        // the app.

        public static void ExitIf()
        {
            lock (_lockObject)
            {
                Logger.LogMessage("ExitIf", $"Object count: {ObjectsCount}, Server lock count: {ServerLockCount}");

                if ((ObjectsCount <= 0) && (ServerLockCount <= 0))
                {
                    if (StartedByCOM)
                    {
                        Logger.LogMessage("ExitIf", "Server started by COM so shutting down now that all clients have disconnected.");

                        Task.Factory.StartNew(() =>
                        {
                            Logger.LogMessage("ExitIf", "Shutting down app...");
                            App.Current.Shutdown();
                            try { Logger.LogMessage("ExitIf", "App shut down"); } catch { }
                        }, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext);
                    }
                }
            }
        }

        public static void DisconnectTelescopeIf()
        {
            Logger.LogMessage("DisconnectTelescopeIf", "Creating disconnect task...");
            Task discon = new Task(() =>
            {
                // The device manager decides whether to disconnect.

                Logger.LogMessage("DisconnectTelescopeIfTask", "Disconnecting telescope...");
                TelescopeManager.Instance.DisconnectTelescope();
                Logger.LogMessage("DisconnectTelescopeIfTask", "Telescope disconnected");
            });

            Logger.LogMessage("DisconnectTelescopeIf", "Starting disconnect task...");
            discon.Start(Globals.UISyncContext);
            Logger.LogMessage("DisconnectTelescopeIf", "Disconnect task started...");
            discon.Wait();

            Logger.LogMessage("DisconnectTelescopeIf", "Disconnect task completed, forcing garbage collection...");
            ForceGarbageCollection();
            Logger.LogMessage("DisconnectTelescopeIf", "Forced garbage collection complete.");
        }

        public static void DisconnectDomeIf()
        {
            Logger.LogMessage("DisconnectDomeIf", "Creating disconnect task...");
            Task discon = new Task(() =>
            {
                // The device manager decides whether to disconnect.

                Logger.LogMessage("DisconnectDomeIfTask", "Disconnecting dome...");
                DomeManager.Instance.DisconnectDome();
                Logger.LogMessage("DisconnectDomeIfTask", "Dome disconnected");
            });

            Logger.LogMessage("DisconnectDomeIf", "Starting disconnect task...");
            discon.Start(Globals.UISyncContext);
            Logger.LogMessage("DisconnectDomeIf", "Disconnect task started...");
            discon.Wait();

            Logger.LogMessage("DisconnectDomeIf", "Disconnect task completed, forcing garbage collection...");
            ForceGarbageCollection();
            Logger.LogMessage("DisconnectDomeIf", "Forced garbage collection complete.");
        }

        public static void DisconnectFocuserIf()
        {
            Logger.LogMessage("DisconnectFocuserIf", "Creating disconnect task...");
            Task discon = new Task(() =>
            {
                // The device manager decides whether to disconnect.

                Logger.LogMessage("DisconnectFocuserIfTask", "Disconnecting focuser...");
                FocuserManager.Instance.DisconnectFocuser();
                Logger.LogMessage("DisconnectFocuserIfTask", "Focuser disconnected");
            });

            Logger.LogMessage("DisconnectFocuserIf", "Starting disconnect task...");
            discon.Start(Globals.UISyncContext);
            Logger.LogMessage("DisconnectFocuserIf", "Disconnect task started...");
            discon.Wait();

            Logger.LogMessage("DisconnectFocuserIf", "Disconnect task completed, forcing garbage collection...");
            ForceGarbageCollection();
            Logger.LogMessage("DisconnectFocuserIf", "Forced garbage collection complete.");
        }

        #endregion Server Lock, Object Counting, and AutoQuit on COM startup

        #region Helper Methods

        private static void LoadDeviceSettings()
        {
            TelescopeSettings scopeSettings = TelescopeSettings.FromProfile();
            TelescopeManager.SetTelescopeID(scopeSettings.TelescopeID);
            TelescopeManager.Instance.SetFastUpdatePeriod(scopeSettings.FastUpdatePeriod);

            DomeSettings domeSettings = DomeSettings.FromProfile();
            DomeManager.SetDomeID(domeSettings.DomeID);
            DomeManager.Instance.SetFastUpdatePeriod(domeSettings.FastUpdatePeriod);
            Globals.DomeLayout = domeSettings.DomeLayout;
            Globals.DomeAzimuthAdjustment = domeSettings.AzimuthAdjustment;
            Globals.UsePOTHDomeSlaveCalculation = domeSettings.UsePOTHDomeSlaveCalculation;

            FocuserSettings focuserSettings = FocuserSettings.FromProfile();
            FocuserManager.SetFocuserID(focuserSettings.FocuserID);
            FocuserManager.Instance.SetFastUpdatePeriod(focuserSettings.FastUpdatePeriod);
            Globals.FocuserTemperatureOffset = focuserSettings.TemperatureOffset;
        }

        private static void SaveDomeSettings()
        {
            DomeSettings domeSettings = DomeSettings.FromProfile();
            domeSettings.AzimuthAdjustment = Globals.DomeAzimuthAdjustment;
            domeSettings.UsePOTHDomeSlaveCalculation = Globals.UsePOTHDomeSlaveCalculation;
            domeSettings.ToProfile();
        }

        private static void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool cancelClosing = false;

            IMessageBoxService msgSvc = ServiceContainer.Instance.GetService<IMessageBoxService>();

            // Check for connected client apps.

            if (ObjectsCount > 0)
            {
                string text = $"Device Hub has {ObjectsCount} client connection{(ObjectsCount > 1 ? "s" : "")}. "
                                + "Are you sure that you want to shut down ? ";
                string title = "Confirm Forced Shutdown";

                MessageBoxResult result = msgSvc.Show(text, title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.None);

                cancelClosing = (result != MessageBoxResult.Yes);
            }
            else if (!StartedByCOM) // Check for device connections
            {
                bool deviceConnections = TelescopeManager.Instance.Connected
                                        || DomeManager.Instance.Connected
                                        || FocuserManager.Instance.Connected;

                if (deviceConnections)
                {
                    string text = "Device Hub is still connected to one or more devices. Are you sure that you want to shut down?";
                    string title = "Confirm Forced Shutdown";

                    MessageBoxResult result = msgSvc.Show(text, title, MessageBoxButton.YesNo, MessageBoxImage.Question, System.Windows.MessageBoxResult.No, System.Windows.MessageBoxOptions.None);

                    cancelClosing = (result != MessageBoxResult.Yes);
                }
            }

            e.Cancel = cancelClosing;
        }

        private static void ShowMainWindow()
        {
            Messenger.Default.Send(new ObjectCountMessage(_scopesInUse, _domesInUse, _focusersInUse));
            MainWindow.AdjustWindowPlacement();
            MainWindow.ShowDialog();
        }

        private static void StartGarbageCollection(int interval)
        {
            string msgId = "StartGarbageCollection";

            // Start up the garbage collection thread.

            Logger.LogMessage(msgId, $"Creating garbage collector with interval: {interval} milliseconds");

            GarbageCollector = new GarbageCollection(interval);

            Logger.LogMessage(msgId, "Starting garbage collector thread");

            GCTokenSource = new CancellationTokenSource();
            GCTask = Task.Factory.StartNew(() => GarbageCollector.GCWatch(GCTokenSource.Token),
                                            GCTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            Logger.LogMessage(msgId, "garbage collector thread started OK");
        }

        private static void StopGarbageCollection()
        {
            string msgId = "StopGarbageCollection";

            Logger.LogMessage(msgId, "Stopping garbage collector thread");

            GCTokenSource.Cancel();
            Logger.LogMessage(msgId, "Cancelled token, waiting for garbage collection task to complete...");
            GCTask.Wait();

            Logger.LogMessage(msgId, "Task completed, garbage collector thread stopped OK");

            GCTask = null;
            GCTokenSource.Dispose();
            GCTokenSource = null;
            GarbageCollector = null;

            Logger.LogMessage(msgId, "Garbage collection stopped.");
        }

        public static void ForceGarbageCollection()
        {
            if (GCTask != null && GarbageCollector != null)
            {
                GarbageCollector.ForceGCNow();
            }
        }

        private static string GetAssemblyTitle()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Attribute attribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute));
            return ((AssemblyTitleAttribute)attribute).Title;
        }

        private static string GetAssemblyDescription()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Attribute attribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute));
            return ((AssemblyDescriptionAttribute)attribute).Description;
        }

        #endregion Helper Methods

        #region Driver Classes Finder

        /// <summary>
        /// Populates the list of ASCOM drivers by searching for driver classes within the local server executable.
        /// </summary>
        /// <returns>True if successful, otherwise False</returns>
        private static bool PopulateListOfAscomDrivers()
        {
            string msgId = "PopulateListOfAscomDrivers";

            // Initialize the driver types list.

            _driverTypes = new List<Type>();

            try
            {
                // Get the types contained within the local server assembly.

                Assembly so = Assembly.GetExecutingAssembly(); // Get the local server assembly 
                Type[] types = so.GetTypes(); // Get the types in the assembly

                // Iterate over the types identifying those which are drivers.

                foreach (Type type in types)
                {
                    Logger.LogMessage(msgId, $"Found type: {type.Name}");

                    // Check to see if this type has the ServedClassName attribute, which indicates that this is a driver class.

                    object[] attrbutes = type.GetCustomAttributes(typeof(ServedClassNameAttribute), false);

                    if (attrbutes.Length > 0) // There is a ServedClassName attribute on this class so it is a driver
                    {
                        Logger.LogMessage(msgId, $"{type.Name} is an ASCOM driver");
                        _driverTypes.Add(type); // Add the driver type to the list
                    }
                }

                Logger.BlankLine();

                // Log discovered drivers.

                Logger.LogMessage(msgId, $"Found {_driverTypes.Count} drivers");

                foreach (Type type in _driverTypes)
                {
                    Logger.LogMessage(msgId, $"Found Driver : {type.Name}");
                }

                Logger.BlankLine();
            }
            catch (Exception e)
            {
                Logger.LogMessageCrLf(msgId, $"Exception: {e}");
                string caption = GetAssemblyTitle();
                MessageBox.Show($"Failed to load served COM class assembly from within this local server - {e.Message}", caption, MessageBoxButton.OK, MessageBoxImage.Stop);

                return false;
            }

            return true;
        }

        #endregion Driver Classes Finder

        #region COM Registration and Unregistration

        // Test if UAC is enabled.

        public static bool IsUacEnabled
        {
            get
            {
                using (RegistryKey uacKey = Registry.LocalMachine.OpenSubKey(_uacRegistryKey, false))
                {
                    bool result = uacKey.GetValue(_uacRegistryValue).Equals(1);
                    return result;
                }
            }
        }

        // Test if running elevated

        private static bool IsElevated
        {
            get
            {
                WindowsIdentity i = WindowsIdentity.GetCurrent();
                WindowsPrincipal p = new WindowsPrincipal(i);

                bool isAdministrator = p.IsInRole(WindowsBuiltInRole.Administrator);

                Logger.LogMessage("IsElevated", $"Running as {(isAdministrator ? "Administrator" : "Self")}");

                return isAdministrator;
            }
        }

        // Elevate by re-running ourselves with elevation dialog

        private static void ElevateSelf(string arg)
        {
            string logId = "ElevateSelf";

            ProcessStartInfo si = new ProcessStartInfo
            {
                Arguments = arg,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = GetExecutablePath(),
                Verb = "runas"
            };

            try
            {
                Logger.LogMessage(logId, "Starting elevation process.");
                Process.Start(si);
            }

            catch (System.ComponentModel.Win32Exception)
            {
                string msg = $"The ASCOM.DeviceHub drivers were not {(arg == "/register" ? "registered" : "unregistered")} because you did not allow it.";
                Logger.LogMessage(logId, msg);
                MessageBox.Show(msg, "Device Hub", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Device Hub", MessageBoxButton.OK, MessageBoxImage.Stop);
            }

            return;
        }

        private static string GetExecutablePath()
        {
            string path = Assembly.GetEntryAssembly().Location;

            return path;
        }

        private static string GetExecutableFileName()
        {
            string path = GetExecutablePath();
            string name = Path.GetFileName(path);

            return name;
        }

        // Do everything to register this for COM. Never use REGASM on
        // this exe assembly! It would create InProcServer32 entries 
        // which would prevent proper activation!

        // Using the list of COM object types generated during dynamic
        // assembly loading, it registers each one for COM as served by our
        // exe/local server, as well as registering it for ASCOM. It also
        // adds DCOM info for the local server itself, so it can be activated
        // via an outbound connection from TheSky.

        private static void RegisterObjects(bool rerunningAsElevated = false)
        {
            string logId = "RegisterObjects";

            if (rerunningAsElevated)
            {
                if (!IsElevated)
                {
                    MessageBox.Show("Unable to gain elevated privileges to register the Device Hub.\n\n"
                        + "Possible fixes are to enable User Account Control and/or add your user account\n"
                        + "to the local Administrators group. Then re-install the Device Hub",
                        "DeviceHub", MessageBoxButton.OK, MessageBoxImage.Stop);

                    return;
                }
            }
            else
            {
                if (!IsElevated)
                {
                    // Here we want to elevate a copy of ourselves, unless we are running under the VS Debugger.

                    RerunAsAdministrator("/regElevated");

                    return;
                }
            }

            // If reached here, we're running elevated

            string assemblyTitle = GetAssemblyTitle();
            string assemblyDescription = GetAssemblyDescription();

            // Local server's DCOM/AppID information

            try
            {
                Logger.LogMessage(logId, "Setting local server's APPID");

                // HKCR\APPID\appid

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey("APPID\\" + _appId))
                {
                    key.SetValue(null, assemblyDescription);
                    key.SetValue("AppID", _appId);
                    key.SetValue("AuthenticationLevel", 1, RegistryValueKind.DWord);
                    key.SetValue("RunAs", "Interactive User", RegistryValueKind.String); // Added to ensure that only one copy of the local server ...
                }

                // HKCR\APPID\exename.ext

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey($"APPID\\{GetExecutableFileName()}"))
                {
                    key.SetValue("AppID", _appId);
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessageCrLf(logId, $"Setting AppID exception: {ex}");

                MessageBox.Show("Error while registering the server:\n" + ex.ToString(),
                        "DeviceHub", MessageBoxButton.OK, MessageBoxImage.Stop);

                return;
            }

            // For each of the driver types

            foreach (Type driverType in _driverTypes)
            {
                Logger.LogMessage(logId, $"Creating COM registration for {driverType.Name}");

                bool bFail = false;

                try
                {
                    // HKCR\CLSID\clsid

                    string clsid = Marshal.GenerateGuidForType(driverType).ToString("B");
                    string progid = Marshal.GenerateProgIdForType(driverType);
                    string deviceType = driverType.Name;    // Generate device type from the class name.
                    Logger.LogMessage(logId, $"Assembly title: {assemblyTitle}, Assembly description: {assemblyDescription}, CLSID: {clsid}, ProgID: {progid}, Device type: {deviceType}");

                    using (RegistryKey clsIdKey = Registry.ClassesRoot.CreateSubKey($"CLSID\\{clsid}"))
                    {
                        clsIdKey.SetValue(null, progid);                     // Could be assyTitle/Desc??, but .NET components show ProgId here
                        clsIdKey.SetValue("AppId", _appId);

                        using (RegistryKey key2 = clsIdKey.CreateSubKey("Implemented Categories"))
                        {
                            key2.CreateSubKey("{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}");
                        }

                        using (RegistryKey progIdKey = clsIdKey.CreateSubKey("ProgId"))
                        {
                            progIdKey.SetValue(null, progid);
                        }

                        clsIdKey.CreateSubKey("Programmable");

                        using (RegistryKey localServer32Key = clsIdKey.CreateSubKey("LocalServer32"))
                        {
                            string path = GetExecutablePath();
                            localServer32Key.SetValue(null, path);
                        }
                    }

                    // HKCR\CLSID\progid

                    using (RegistryKey progIdKey = Registry.ClassesRoot.CreateSubKey(progid))
                    {
                        progIdKey.SetValue(null, assemblyTitle);

                        using (RegistryKey clsIdKey = progIdKey.CreateSubKey("CLSID"))
                        {
                            clsIdKey.SetValue(null, clsid);
                        }
                    }

                    // Pull the display name from the ServedClassName attribute.

                    Attribute assemblyAttribute = Attribute.GetCustomAttribute(driverType, typeof(ServedClassNameAttribute));
                    string chooserName = ((ServedClassNameAttribute)assemblyAttribute).DisplayName ?? "MultiServer";
                    Logger.LogMessage(logId, $"Registering {chooserName} ({driverType.Name}) in Profile");

                    using (var P = new ASCOM.Utilities.Profile())
                    {
                        P.DeviceType = deviceType;
                        P.Register(progid, chooserName);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogMessageCrLf(logId, $"Driver registration exception: {ex}");
                    string caption = GetAssemblyTitle();
                    MessageBox.Show("Error while registering the server:\n" + ex.ToString(), caption, MessageBoxButton.OK, MessageBoxImage.Stop);

                    bFail = true;
                }
                finally
                { }

                if (bFail)
                {
                    break;
                }
            }
        }

        private static void RerunAsAdministrator(string args)
        {
            if (Debugger.IsAttached)
            {
                MessageBox.Show("You must run Visual Studio As Administrator to debug an ASCOM driver.",
                                "Run As Administrator", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                ElevateSelf(args);
            }
        }

        // Remove all traces of this from the registry. 

        private static void UnregisterObjects(bool keepProfile, List<Type> _driverTypes, bool rerunningAsElevated = false)
        {
            string logId = "UnregisterObjects";

            if (rerunningAsElevated)
            {
                if (!IsElevated)
                {
                    MessageBox.Show("Unable to gain elevated privileges to unregister the Device Hub.\n\n"
                        + "If User Account Control is disabled, please enable it before uninstalling the Device Hub",
                        "DeviceHub", MessageBoxButton.OK, MessageBoxImage.Stop);

                    return;
                }
            }
            else
            {
                if (!IsElevated)
                {
                    // Here we want to elevate a copy of ourselves, unless we are running under the VS Debugger.

                    RerunAsAdministrator(keepProfile ? "/unregElevated" : "/unregElevated_full");

                    return;
                }
            }

            // Local server's DCOM/AppID information

            Registry.ClassesRoot.DeleteSubKey($"APPID\\{_appId}", false);
            Registry.ClassesRoot.DeleteSubKey($"APPID\\{GetExecutableFileName()}", false);

            // For each of the driver assemblies

            foreach (Type driverType in _driverTypes)
            {
                string clsid = Marshal.GenerateGuidForType(driverType).ToString("B");
                string progid = Marshal.GenerateProgIdForType(driverType);
                string deviceType = driverType.Name;

                // Remove ProgID entries

                Registry.ClassesRoot.DeleteSubKey($"{progid}\\CLSID", false);
                Registry.ClassesRoot.DeleteSubKey(progid, false);

                // Remove ClsID entries

                Registry.ClassesRoot.DeleteSubKey($"CLSID\\{clsid}\\Implemented Categories\\{{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}}", false);
                Registry.ClassesRoot.DeleteSubKey($"CLSID\\{clsid}\\Implemented Categories", false);
                Registry.ClassesRoot.DeleteSubKey($"CLSID\\{clsid}\\ProgId", false);
                Registry.ClassesRoot.DeleteSubKey($"CLSID\\{clsid}\\LocalServer32", false);
                Registry.ClassesRoot.DeleteSubKey($"CLSID\\{clsid}\\Programmable", false);
                Registry.ClassesRoot.DeleteSubKey($"CLSID\\{clsid}", false);

                if (!keepProfile)
                {
                    // Delete any profile settings for the device.

                    Logger.LogMessage(logId, $"Deleting ASCOM Profile registration for {driverType.Name} ({progid})");

                    try
                    {
                        using (var P = new ASCOM.Utilities.Profile())
                        {
                            P.DeviceType = deviceType;
                            P.Unregister(progid);
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        #endregion  COM Registration and Unregistration

        #region Class Factory Support

        // On startup, we register the class factories of the COM objects
        // that we serve. This requires the class factory name to be
        // equal to the served class name + "ClassFactory".

        private static bool RegisterClassFactories()
        {
            string logId = "RegisterClassFactories";

            Logger.LogMessage(logId, "Registering class factories");

            _classFactories = new List<ClassFactory>();

            foreach (Type driverType in _driverTypes)
            {
                Logger.LogMessage(logId, $"Creating class factory for {driverType.Name}");

                ClassFactory factory = new ClassFactory(driverType); // Use default context & flags
                _classFactories.Add(factory);

                Logger.LogMessage(logId, $"Registering class factory for: {driverType.Name}");

                if (!factory.RegisterClassObject())
                {
                    string msg = $"Failed to register class factory for {driverType.Name}";
                    Logger.LogMessage(logId, msg);
                    string caption = GetAssemblyTitle();
                    MessageBox.Show(msg, caption, MessageBoxButton.OK, MessageBoxImage.Stop);

                    return false;
                }

                Logger.LogMessage(logId, $"Registered class factory OK for: {driverType.Name}");
            }

            Logger.LogMessage(logId, "Making class factories live");

            ClassFactory.ResumeClassObjects(); // Served objects now go live

            Logger.LogMessage(logId, "Class factories live OK");

            return true;
        }

        private static void RevokeClassFactories()
        {
            string logId = "RevokeClassFactories";

            Logger.LogMessage(logId, "Suspending class factories.");

            ClassFactory.SuspendClassObjects();  // Prevent race conditions

            Logger.LogMessage(logId, "Class factories suspended OK.");

            foreach (ClassFactory factory in _classFactories)
            {
                factory.RevokeClassObject();
            }
        }

        #endregion Class Factory Support

        #region Command Line Arguments

        // ProcessArguments() will process the command-line arguments
        // If the return value is true, we carry on and start this application.
        // If the return value is false, we terminate this application immediately.

        private static bool ProcessArguments(string[] args)
        {
            string msgId = "ProcessArguments";
            bool retval = true;

            if (args.Length > 0)
            {
                string arg = args[0].ToLower();
                bool keepProfile = !arg.EndsWith("_full");

                switch (arg)
                {
                    case "-embedding":
                        Logger.LogMessage(msgId, $"Started by COM: {args[0]}");
                        StartedByCOM = true;                                        // Indicate COM started us

                        break;

                    case "-register":
                    case @"/register":
                    case "-regserver":                                          // Emulate VB6
                    case @"/regserver":
                        Logger.LogMessage(msgId, $"Registering drivers: {args[0]}");
                        RegisterObjects();                                      // Register each served object
                        retval = false;

                        break;

                    case "-unregister":
                    case @"/unregister":
                    case "-unregserver":                                        // Emulate VB6
                    case @"/unregserver":
                    case "-unregister_full":
                    case @"/unregister_full":
                    case "-unregserver_full":                                        // Emulate VB6
                    case @"/unregserver_full":
                        Logger.LogMessage(msgId, $"Unregistering drivers: {args[0]}");
                        UnregisterObjects(keepProfile, _driverTypes);                                    //Unregister each served object
                        retval = false;

                        break;

                    case "-regelevated":
                    case @"/regelevated":
                        Logger.LogMessage(msgId, $"Registering drivers while elevated: {args[0]}");
                        RegisterObjects(true);
                        retval = false;

                        break;

                    case "-unregelevated":
                    case @"/unregelevated":
                    case "-unregelevated_full":
                    case @"/unregelevated_full":
                        Logger.LogMessage(msgId, $"Unregistering drivers while elevated: {args[0]}");
                        UnregisterObjects(keepProfile, _driverTypes, true);
                        retval = false;

                        break;
                    case "-enableapplogging":
                    case @"/enableapplogging":
                        Logger.LogMessage(msgId, "Enabling application startup logging.");
                        Globals.ForceAppLogging = true;
                        break;

                    default:
                        string msg = $"Unknown argument: {args[0]}";
                        Logger.LogMessage(msgId, msg);
                        msg += "\nValid are : -register, -unregister, unregister_full, -embedding, and -enableapplogging";
                        string caption = GetAssemblyTitle();
                        MessageBox.Show(msg, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);

                        break;
                }
            }
            else
            {
                StartedByCOM = false;
            }

            return retval;
        }

        #endregion Command Line Arguments
    }
}
