//
// ASCOM.DynamicRemoteClients Local COM Server
//
// This is the core of a managed COM Local Server, capable of serving
// multiple instances of multiple interfaces, within a single
// executable. This implements the equivalent functionality of VB6
// which has been extensively used in ASCOM for drivers that provide
// multiple interfaces to multiple clients (e.g. Meade Telescope
// and Focuser) as well as hubs (e.g., POTH).
//
// Written by: Robert B. Denny (Version 1.0.1, 29-May-2007)
// Modified by Chris Rowland and Peter Simpson to allow use with multiple devices of the same type March 2011
//
//
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using ASCOM.Utilities;
using Microsoft.Win32;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;

namespace ASCOM.DynamicRemoteClients
{
    public static class Server
    {

        #region Access to kernel32.dll, user32.dll, and ole32.dll functions
        [Flags]
        enum CLSCTX : uint
        {
            CLSCTX_INPROC_SERVER = 0x1,
            CLSCTX_INPROC_HANDLER = 0x2,
            CLSCTX_LOCAL_SERVER = 0x4,
            CLSCTX_INPROC_SERVER16 = 0x8,
            CLSCTX_REMOTE_SERVER = 0x10,
            CLSCTX_INPROC_HANDLER16 = 0x20,
            CLSCTX_RESERVED1 = 0x40,
            CLSCTX_RESERVED2 = 0x80,
            CLSCTX_RESERVED3 = 0x100,
            CLSCTX_RESERVED4 = 0x200,
            CLSCTX_NO_CODE_DOWNLOAD = 0x400,
            CLSCTX_RESERVED5 = 0x800,
            CLSCTX_NO_CUSTOM_MARSHAL = 0x1000,
            CLSCTX_ENABLE_CODE_DOWNLOAD = 0x2000,
            CLSCTX_NO_FAILURE_LOG = 0x4000,
            CLSCTX_DISABLE_AAA = 0x8000,
            CLSCTX_ENABLE_AAA = 0x10000,
            CLSCTX_FROM_DEFAULT_CONTEXT = 0x20000,
            CLSCTX_INPROC = CLSCTX_INPROC_SERVER | CLSCTX_INPROC_HANDLER,
            CLSCTX_SERVER = CLSCTX_INPROC_SERVER | CLSCTX_LOCAL_SERVER | CLSCTX_REMOTE_SERVER,
            CLSCTX_ALL = CLSCTX_SERVER | CLSCTX_INPROC_HANDLER
        }

        [Flags]
        enum COINIT : uint
        {
            /// Initializes the thread for multi-threaded object concurrency.
            COINIT_MULTITHREADED = 0x0,
            /// Initializes the thread for apartment-threaded object concurrency. 
            COINIT_APARTMENTTHREADED = 0x2,
            /// Disables DDE for Ole1 support.
            COINIT_DISABLE_OLE1DDE = 0x4,
            /// Trades memory for speed.
            COINIT_SPEED_OVER_MEMORY = 0x8
        }

        [Flags]
        enum REGCLS : uint
        {
            REGCLS_SINGLEUSE = 0,
            REGCLS_MULTIPLEUSE = 1,
            REGCLS_MULTI_SEPARATE = 2,
            REGCLS_SUSPENDED = 4,
            REGCLS_SURROGATE = 8
        }

        #endregion

        #region Private Data
        private static int objsInUse;                       // Keeps a count on the total number of objects alive.
        private static int serverLocks;                     // Keeps a lock count on this application.
        private static LocalServerForm s_MainForm = null;               // Reference to our main form
        private static ArrayList s_ComObjectAssys;              // Dynamically loaded assemblies containing served COM objects
        private static ArrayList s_ComObjectTypes;              // Served COM object types
        private static ArrayList s_ClassFactories;              // Served COM object class factories
        private static string s_appId = "{31506222-DA7E-4900-A414-843BB3E1BD16}";	// Our AppId
        private static readonly Object lockObject = new object();

        private const string LOCAL_SERVER_NAME = "dynamic Driver Local Server";


        private static TraceLogger TL;
        #endregion

        // This property returns the main thread's id.
        public static uint MainThreadId { get; private set; }   // Stores the main thread's thread id.

        // Used to tell if started by COM or manually
        public static bool StartedByCOM { get; private set; }   // True if server started by COM (-embedding)


        #region Server Lock, Object Counting, and AutoQuit on COM startup
        // Returns the total number of objects alive currently.
        public static int ObjectsCount
        {
            get
            {
                lock (lockObject)
                {
                    return objsInUse;
                }
            }
        }

        // This method performs a thread-safe incrementation of the objects count.
        public static int CountObject()
        {
            // Increment the global count of objects.
            return Interlocked.Increment(ref objsInUse);
        }

        // This method performs a thread-safe decrementation the objects count.
        public static int UncountObject()
        {
            // Decrement the global count of objects.
            return Interlocked.Decrement(ref objsInUse);
        }

        // Returns the current server lock count.
        public static int ServerLockCount
        {
            get
            {
                lock (lockObject)
                {
                    return serverLocks;
                }
            }
        }

        // This method performs a thread-safe incrementation the 
        // server lock count.
        public static int CountLock()
        {
            // Increment the global lock count of this server.
            return Interlocked.Increment(ref serverLocks);
        }

        // This method performs a thread-safe decrementation the 
        // server lock count.
        public static int UncountLock()
        {
            // Decrement the global lock count of this server.
            return Interlocked.Decrement(ref serverLocks);
        }

        // AttemptToTerminateServer() will check to see if the objects count and the server 
        // lock count have both dropped to zero.
        //
        // If so, and if we were started by COM, we post a WM_QUIT message to the main thread's
        // message loop. This will cause the message loop to exit and hence the termination 
        // of this application. If hand-started, then just trace that it WOULD exit now.
        //
        public static void ExitIf()
        {
            lock (lockObject)
            {
                if ((ObjectsCount <= 0) && (ServerLockCount <= 0))
                {
                    if (StartedByCOM)
                    {
                        UIntPtr wParam = new UIntPtr(0);
                        IntPtr lParam = new IntPtr(0);
                        NativeMethods.PostThreadMessage(MainThreadId, 0x0012, wParam, lParam);
                    }
                }
            }
        }
        #endregion


        #region Dynamic Driver Assembly Loader

        /// <summary>
        /// Load the assemblies that contain the classes that we will serve via COM. These will be located in the same folder as our executable.
        /// </summary>
        /// <returns></returns>
        private static bool LoadComObjectAssemblies()
        {
            s_ComObjectAssys = new ArrayList();
            s_ComObjectTypes = new ArrayList();

            // put everything into one folder, the same as the server.
            string assyPath = Assembly.GetEntryAssembly().Location;
            assyPath = Path.GetDirectoryName(assyPath);

            DirectoryInfo d = new DirectoryInfo(assyPath);
            foreach (FileInfo fi in d.GetFiles("*.dll"))
            {
                TL.LogMessage("LoadComObjectAssemblies", $"Found file: {fi.FullName}");

                string aPath = fi.FullName;
                //
                // First try to load the assembly and get the types for
                // the class and the class factory. If this doesn't work ????
                //
                try
                {
                    Assembly so = Assembly.LoadFrom(aPath);
                    //PWGS Get the types in the assembly
                    Type[] types = so.GetTypes();
                    foreach (Type type in types)
                    {
                        // PWGS Now checks the type rather than the assembly
                        // Check to see if the type has the ServedClassName attribute, only use it if it does.
                        MemberInfo info = type;

                        object[] attrbutes = info.GetCustomAttributes(typeof(ServedClassNameAttribute), false);
                        if (attrbutes.Length > 0)
                        {
                            TL.LogMessage("LoadComObjectAssemblies", $"Type: {type.Name} has ServedClassAttribute");
                            s_ComObjectTypes.Add(type); //PWGS - much simpler
                            s_ComObjectAssys.Add(so);
                        }
                    }
                }
                catch (BadImageFormatException)
                {
                    TL.LogMessage("LoadComObjectAssemblies", $"Preceding type is not an assembly");
                    // Probably an attempt to load a Win32 DLL (i.e. not a .net assembly)
                    // Just swallow the exception and continue to the next item.
                    continue;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to load served COM class assembly " + fi.Name + " - " + e.Message, LOCAL_SERVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    TL.LogMessageCrLf("LoadComObjectAssemblies", $"Exception while getting types: {e.ToString()}");
                    return false;
                }

            }
            return true;
        }
        #endregion

        #region COM Registration and Unregistration
        //
        // Test if running elevated
        //
        private static bool IsAdministrator
        {
            get
            {
                WindowsIdentity i = WindowsIdentity.GetCurrent();
                WindowsPrincipal p = new WindowsPrincipal(i);
                return p.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        //
        // Elevate by re-running ourselves with elevation dialog
        //
        private static void ElevateSelf(string arg)
        {
            ProcessStartInfo si = new ProcessStartInfo
            {
                Arguments = arg,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Application.ExecutablePath,
                Verb = "runas"
            };
            try { Process.Start(si); }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("The driver was not " + (arg == "/register" ? "registered" : "unregistered") + " because you did not allow it.", LOCAL_SERVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), LOCAL_SERVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return;
        }

        //
        // Do everything to register this for COM. Never use REGASM on
        // this exe assembly! It would create InProcServer32 entries 
        // which would prevent proper activation!
        //
        // Using the list of COM object types generated during dynamic
        // assembly loading, it registers each one for COM as served by our
        // exe/local server, as well as registering it for ASCOM. It also
        // adds DCOM info for the local server itself, so it can be activated
        // via an outbound connection from TheSky.
        //
        private static void RegisterObjects()
        {
            if (!IsAdministrator)
            {
                ElevateSelf("/register");
                return;
            }
            // If reached here, we're running elevated

            Assembly assy = Assembly.GetExecutingAssembly();
            Attribute attr = Attribute.GetCustomAttribute(assy, typeof(AssemblyTitleAttribute));
            string assyTitle = ((AssemblyTitleAttribute)attr).Title;
            attr = Attribute.GetCustomAttribute(assy, typeof(AssemblyDescriptionAttribute));
            string assyDescription = ((AssemblyDescriptionAttribute)attr).Description;
            TL.LogMessage("RegisterObjects", $"ASsembly description: {assyDescription}");

            // Set the local server's DCOM/AppID information
            try
            {
                // Set HKCR\APPID\appid
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey("APPID\\" + s_appId))
                {
                    key.SetValue(null, assyDescription);
                    key.SetValue("AppID", s_appId);
                    key.SetValue("AuthenticationLevel", 1, RegistryValueKind.DWord);
                    TL.LogMessage("RegisterObjects", $"Set APPID: {assyDescription} {s_appId} Authentication level: 1");
                }

                // Set HKCR\APPID\exename.ext
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(string.Format("APPID\\{0}", Application.ExecutablePath.Substring(Application.ExecutablePath.LastIndexOf('\\') + 1))))
                {
                    key.SetValue("AppID", s_appId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while registering the server:\n" + ex.ToString(), LOCAL_SERVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                TL.LogMessageCrLf("RegisterObjects", $"Exception while registering AppID: {ex.ToString()}");
                return;
            }
            finally
            {
            }

            TL.LogMessage("RegisterObjects", "Registering types");

            // COM register each of the driver assemblies
            foreach (Type type in s_ComObjectTypes)
            {
                TL.LogMessage("RegisterObjects", string.Format("Processing type: {0}, is a COM object: {1}", type.FullName, type.IsCOMObject));
                bool bFail = false;
                try
                {
                    // Set HKCR\CLSID\clsid
                    string clsid = Marshal.GenerateGuidForType(type).ToString("B");
                    string progid = Marshal.GenerateProgIdForType(type);
                    TL.LogMessage("RegisterObjects", $"ProgID: {progid} CLSID: {clsid} ");

                    // Generate device type from the Class name
                    string deviceType = type.Name;
                    using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(string.Format("CLSID\\{0}", clsid)))
                    {
                        key.SetValue(null, progid);
                        key.SetValue("AppId", s_appId);
                        using (RegistryKey key2 = key.CreateSubKey("Implemented Categories"))
                        {
                            key2.CreateSubKey("{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}");
                        }
                        using (RegistryKey key2 = key.CreateSubKey("ProgId"))
                        {
                            key2.SetValue(null, progid);
                        }
                        key.CreateSubKey("Programmable");
                        using (RegistryKey key2 = key.CreateSubKey("LocalServer32"))
                        {
                            key2.SetValue(null, Application.ExecutablePath);
                        }
                    }

                    // Set HKCR\APPID\clsid for TheSkyX DCOM
                    using (RegistryKey key = Registry.ClassesRoot.CreateSubKey("APPID\\" + clsid))
                    {
                        key.SetValue(null, assyDescription);
                        key.SetValue("AppID", clsid);
                        key.SetValue("AuthenticationLevel", 1, RegistryValueKind.DWord);
                    }

                    // Set HKCR\CLSID\progid
                    using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(progid))
                    {
                        key.SetValue(null, assyTitle);
                        using (RegistryKey key2 = key.CreateSubKey("CLSID"))
                        {
                            key2.SetValue(null, clsid);
                        }
                    }

                    // Register the driver in the ASCOM Profile if it is not already registered
                    assy = type.Assembly;
                    // Pull the display name from the ServedClassName attribute.
                    attr = Attribute.GetCustomAttribute(type, typeof(ServedClassNameAttribute));
                    string chooserName = ((ServedClassNameAttribute)attr).DisplayName ?? "MultiServer";
                    using (var profile = new ASCOM.Utilities.Profile())
                    {
                        profile.DeviceType = deviceType;
                        TL.LogMessage("RegisterObjects", $"About to Check whether {progid} of device type: {deviceType} is registered. IsRegistered: {profile.IsRegistered(progid)}");
                        if (!profile.IsRegistered(progid)) // Device is not ASCOM registered so register it and set some initial values so that the driver will appear valid to the dynamic device manager
                        {
                            TL.LogMessage("RegisterObjects", $"ProgID: {progid} profile is not registered, setting default values ");
                            profile.Register(progid, chooserName);
                            profile.WriteValue(progid, SharedConstants.IPADDRESS_PROFILENAME, SharedConstants.IPADDRESS_DEFAULT);
                            profile.WriteValue(progid, SharedConstants.PORTNUMBER_PROFILENAME, SharedConstants.PORTNUMBER_DEFAULT.ToString());
                            profile.WriteValue(progid, SharedConstants.REMOTE_DEVICE_NUMBER_PROFILENAME, SharedConstants.REMOTE_DEVICE_NUMBER_DEFAULT.ToString());
                            profile.WriteValue(progid, SharedConstants.UNIQUEID_PROFILENAME, SharedConstants.UNIQUEID_DEFAULT);
                        }
                        else
                        {
                            TL.LogMessage("RegisterObjects", $"ProgID: {progid} profile is already registered.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while registering the server:\n" + ex.ToString(), LOCAL_SERVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    TL.LogMessageCrLf("RegisterObjects", $"Exception while registering objects: {ex.ToString()}");

                    bFail = true;
                }
                finally
                {
                }
                if (bFail) break;
            }
        }

        //
        // Remove all traces of this from the registry. 
        //
        // **TODO** If the above does AppID/DCOM stuff, this would have
        // to remove that stuff too.
        //
        private static void UnregisterObjects()
        {
            // Ensure that we are running as with admin privilege
            if (!IsAdministrator)
            {
                ElevateSelf("/unregister");
                return;
            }

            // Remove the local server's DCOM/AppID information
            Registry.ClassesRoot.DeleteSubKey(string.Format("APPID\\{0}", s_appId), false);
            Registry.ClassesRoot.DeleteSubKey(string.Format("APPID\\{0}", Application.ExecutablePath.Substring(Application.ExecutablePath.LastIndexOf('\\') + 1)), false);

            // COM unregister each of the driver assemblies
            foreach (Type type in s_ComObjectTypes)
            {
                TL.LogMessage("RegisterObjects", string.Format("Processing type: {0}, is a COM object: {1}", type.FullName, type.IsCOMObject));

                string clsid = Marshal.GenerateGuidForType(type).ToString("B");
                string progid = Marshal.GenerateProgIdForType(type);

                // Remove HKCR\progid
                Registry.ClassesRoot.DeleteSubKey(String.Format("{0}\\CLSID", progid), false);
                Registry.ClassesRoot.DeleteSubKey(progid, false);

                // Remove HKCR\CLSID\clsid
                Registry.ClassesRoot.DeleteSubKey(String.Format("CLSID\\{0}\\Implemented Categories\\{{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}}", clsid), false);
                Registry.ClassesRoot.DeleteSubKey(String.Format("CLSID\\{0}\\Implemented Categories", clsid), false);
                Registry.ClassesRoot.DeleteSubKey(String.Format("CLSID\\{0}\\ProgId", clsid), false);
                Registry.ClassesRoot.DeleteSubKey(String.Format("CLSID\\{0}\\LocalServer32", clsid), false);
                Registry.ClassesRoot.DeleteSubKey(String.Format("CLSID\\{0}\\Programmable", clsid), false);
                Registry.ClassesRoot.DeleteSubKey(String.Format("CLSID\\{0}", clsid), false);

            }
        }
        #endregion

        #region Class Factory Support
        //
        // On startup, we register the class factories of the COM objects
        // that we serve. This requires the class factory name to be
        // equal to the served class name + "ClassFactory".
        //
        private static bool RegisterClassFactories()
        {
            s_ClassFactories = new ArrayList();
            foreach (Type type in s_ComObjectTypes)
            {
                ClassFactory factory = new ClassFactory(type);                  // Use default context & flags
                s_ClassFactories.Add(factory);
                if (!factory.RegisterClassObject())
                {
                    MessageBox.Show("Failed to register class factory for " + type.Name, "Dynamic Driver Local Server", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            ClassFactory.ResumeClassObjects();                                  // Served objects now go live
            return true;
        }

        private static void RevokeClassFactories()
        {
            ClassFactory.SuspendClassObjects();                                 // Prevent race conditions
            foreach (ClassFactory factory in s_ClassFactories)
                factory.RevokeClassObject();
        }
        #endregion

        #region Command Line Arguments
        //
        // ProcessArguments() will process the command-line arguments
        // If the return value is true, we carry on and start this application.
        // If the return value is false, we terminate this application immediately.
        //
        private static bool ProcessArguments(string[] args)
        {
            bool bRet = true;

            //
            //**TODO** -Embedding is "ActiveX start". Prohibit non_AX starting?
            //
            if (args.Length > 0)
            {

                switch (args[0].ToLower())
                {
                    case "-embedding":
                        StartedByCOM = true;                                        // Indicate COM started us
                        break;

                    case "-register":
                    case @"/register":
                    case "-regserver":                                          // Emulate VB6
                    case @"/regserver":
                        RegisterObjects();                                      // Register each served object
                        bRet = false;
                        break;

                    case "-unregister":
                    case @"/unregister":
                    case "-unregserver":                                        // Emulate VB6
                    case @"/unregserver":
                        UnregisterObjects();                                    //Unregister each served object
                        bRet = false;
                        break;

                    default:
                        MessageBox.Show("Unknown argument: " + args[0] + "\nValid are : -register, -unregister and -embedding", LOCAL_SERVER_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                }
            }
            else
                StartedByCOM = false;

            return bRet;
        }
        #endregion

        #region SERVER ENTRY POINT (main)
        //
        // ==================
        // SERVER ENTRY POINT
        // ==================
        //
        [STAThread]
        static void Main(string[] args)
        {
            TL = new TraceLogger("DynamicClientServer");
            TL.Enabled = RegistryCommonCode.GetBool(GlobalConstants.SIMULATOR_TRACE, GlobalConstants.SIMULATOR_TRACE_DEFAULT);

            TL.LogMessage("Main", $"Dynamic client version: {Application.ProductVersion}");
            TL.LogMessage("Main", $"Running as a {(string)(Environment.Is64BitProcess ? "64bit" : "32bit")} process on a {(string)(Environment.Is64BitOperatingSystem ? "64bit" : "32bit")} operating system.");

            if (!LoadComObjectAssemblies()) return;                     // Load served COM class assemblies, get types

            if (!ProcessArguments(args)) return;                        // Register/Unregister

            // Initialize critical member variables.
            objsInUse = 0;
            serverLocks = 0;
            MainThreadId = NativeMethods.GetCurrentThreadId();
            Thread.CurrentThread.Name = "Main Thread";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            s_MainForm = new LocalServerForm();

            TL.LogMessage("Main", $"Started by COM: {StartedByCOM}");
            if (StartedByCOM)
            {
                s_MainForm.WindowState = FormWindowState.Minimized;
                s_MainForm.Visible = false;
                s_MainForm.Hide();
                s_MainForm.ShowInTaskbar = false;
            }

            // Register the class factories of the served objects
            RegisterClassFactories();

            // Start up the garbage collection thread.
            GarbageCollection GarbageCollector = new GarbageCollection(1000);
            Thread GCThread = new Thread(new ThreadStart(GarbageCollector.GCWatch))
            {
                Name = "Garbage Collection Thread"
            };
            GCThread.Start();

            //
            // Start the message loop. This serializes incoming calls to our
            // served COM objects, making this act like the VB6 equivalent!
            //
            try
            {
                Application.Run(s_MainForm);
            }
            finally
            {
                // Revoke the class factories immediately.
                // Don't wait until the thread has stopped before
                // we perform revocation!!!
                RevokeClassFactories();

                // Now stop the Garbage Collector thread.
                GarbageCollector.StopThread();
                GarbageCollector.WaitForThreadToStop();
                GarbageCollector.Dispose();

                TL.Enabled = false;

                TL.Dispose();
            }
        }
        #endregion
    }
}
