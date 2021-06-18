//
// $safeprojectname$ Local COM Server
//
// This is the core of a managed COM Local Server, capable of serving
// multiple instances of multiple interfaces, within a single
// executable. This implementes the equivalent functionality of VB6
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
using System.Drawing;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using ASCOM.Utilities;
using Microsoft.Win32;
using System.Text;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;
using ASCOM;

namespace ASCOM.TEMPLATEDEVICENAME.Server
{
    public static class Server
    {

        #region kernel32.dll and user32.dll functions

        // Post a Windows Message to a specific thread (identified by its thread id). Used to post a WM_QUIT message to the main thread in order to terminate this application.)
        [DllImport("user32.dll")]
        static extern bool PostThreadMessage(uint idThread, uint Msg, UIntPtr wParam, IntPtr lParam);

        // Obtain the thread id of the calling thread allowing us to post the WM_QUIT message to the main thread.
        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        #endregion

        #region Public Data

        public static frmMain localServerMainForm = null; // Reference to our main form. Changed to public for access in simulator
        public static uint MainThreadId { get; private set; } // Stores the main thread's thread id.
        public static bool StartedByCOM { get; private set; } // True if server started by COM (-embedding)

        #endregion

        #region Private Data

        private static int driversInUseCount; // Keeps a count on the total number of objects alive.
        private static int serverLockCount; // Keeps a lock count on this application.
        private static ArrayList driverTypes; // Served COM object types
        private static ArrayList classFactories; // Served COM object class factories
        private static string localServerAppId = "{$guid1$}"; // Our AppId
        private static readonly Object lockObject = new object(); // Counter lock object
        private static TraceLogger TL; // TraceLogger for the local server (not the served driver, which has its own) - primarily to help debug local server issues

        #endregion

        #region Server Lock, Object Counting, and AutoQuit on COM start-up

        /// <summary>
        /// Returns the total number of objects alive currently. 
        /// </summary>
        public static int ObjectCount
        {
            get
            {
                lock (lockObject)
                {
                    return driversInUseCount; // Return the object count
                }
            }
        }

        /// <summary>
        /// Performs a thread-safe incrementation of the object count. 
        /// </summary>
        /// <returns></returns>
        public static int IncrementObjectCount()
        {
            return Interlocked.Increment(ref driversInUseCount); // Increment the object count.
        }

        /// <summary>
        /// Performs a thread-safe decrementation the objects count.
        /// </summary>
        /// <returns></returns>
        public static int DecrementObjectCount()
        {
            return Interlocked.Decrement(ref driversInUseCount); // Decrement the object count.
        }

        /// <summary>
        /// Returns the current server lock count.
        /// </summary>
        public static int ServerLockCount
        {
            get
            {
                lock (lockObject)
                {
                    return serverLockCount; // Return the lock count
                }
            }
        }

        /// <summary>
        /// Performs a thread-safe incrementation of the server lock count. 
        /// </summary>
        /// <returns></returns>
        public static int IncrementServerLockCount()
        {
            return Interlocked.Increment(ref serverLockCount); // Increment the server lock count for this server.
        }

        /// <summary>
        /// Performs a thread-safe decrementation the server lock count.
        /// </summary>
        /// <returns></returns>
        public static int DecrementServerLockLock()
        {
            return Interlocked.Decrement(ref serverLockCount); // Decrement the server lock count for this server.
        }

        /// <summary>
        /// Test whether the objects count and server lock count have both dropped to zero and, if so, terminate the application.
        /// </summary>
        /// <remarks>
        /// If the counts are zero, the application is terminated by posting a WM_QUIT message to the main thread's message loop, causing it to terminate and exit.
        /// </remarks>
        public static void ExitIf()
        {
            lock (lockObject)
            {
                if ((ObjectCount <= 0) && (ServerLockCount <= 0))
                {
                    if (StartedByCOM)
                    {
                        UIntPtr wParam = new UIntPtr(0);
                        IntPtr lParam = new IntPtr(0);
                        PostThreadMessage(MainThreadId, 0x0012, wParam, lParam);
                    }
                }
            }
        }

        #endregion

        #region Dynamic Driver Assembly Loader

        /// <summary>
        /// Populates the list of ASCOM drivers by searching for driver classes within the local server executable.
        /// </summary>
        /// <returns>True if successful, otherwise False</returns>
        private static bool PopulateAscomDriverAssemblyList()
        {
            driverTypes = new ArrayList();

            try
            {

                // Get the types contained within the local server assembly
                Assembly so = Assembly.GetExecutingAssembly(); // Get the local server assembly 
                Type[] types = so.GetTypes(); // Get the types in the assembly

                // Iterate over the types identifying those which are drivers
                foreach (Type type in types)
                {
                    TL.LogMessage("LoadComObjectAssemblies", $"Found type: {type.Name}");

                    // Check to see if this type has the ServedClassName attribute, which indicates that this is a driver class.
                    object[] attrbutes = type.GetCustomAttributes(typeof(ServedClassNameAttribute), false);
                    if (attrbutes.Length > 0) // There is a ServedCl;assName attribute on this class so it is a driver
                    {
                        TL.LogMessage("LoadComObjectAssemblies", $"{type.Name} is a driver assembly");
                        driverTypes.Add(type); // Add the driver type to the list
                    }
                }

                // Log discovered drivers
                TL.LogMessage("LoadComObjectAssemblies", $"Found {driverTypes.Count} drivers");
                foreach (Type type in driverTypes)
                {
                    TL.LogMessage("LoadComObjectAssemblies", $"Found Driver : {type.Name}");
                }

            }

            catch (Exception e)
            {
                TL.LogMessageCrLf("LoadComObjectAssemblies", $"Exception: {e}");
                MessageBox.Show($"Failed to load served COM class assembly from within this local server - {e.Message}", "Rotator Simulator", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        #endregion

        #region COM Registration and Unregistration

        /// <summary>
        /// Register drivers contained in this local server.
        /// </summary>
        /// <remarks>
        /// Do everything to register this for COM. Never use REGASM on this exe assembly! It would create InProcServer32 entries which would prevent proper activation!
        /// Using the list of COM object types generated during dynamic assembly loading, this method registers each driver for COM and registers it for ASCOM. 
        /// It also adds DCOM info for the local server itself, so it can be activated via an outbound connection from TheSky.
        /// </remarks>
        private static void RegisterObjects()
        {
            if (!IsAdministrator)
            {
                ElevateSelf("/register");
                return;
            }

            // If we reach here, we're running elevated

            // Initialise variables
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Attribute assemblyTitleAttribute = Attribute.GetCustomAttribute(executingAssembly, typeof(AssemblyTitleAttribute));
            string assemblyTitle = ((AssemblyTitleAttribute)assemblyTitleAttribute).Title;
            assemblyTitleAttribute = Attribute.GetCustomAttribute(executingAssembly, typeof(AssemblyDescriptionAttribute));
            string assemblyDescription = ((AssemblyDescriptionAttribute)assemblyTitleAttribute).Description;

            // Set the local server's DCOM/AppID information
            try
            {
                // Set HKCR\APPID\appid
                using (RegistryKey appIdKey = Registry.ClassesRoot.CreateSubKey($"APPID\\{localServerAppId}"))
                {
                    appIdKey.SetValue(null, assemblyDescription);
                    appIdKey.SetValue("AppID", localServerAppId);
                    appIdKey.SetValue("AuthenticationLevel", 1, RegistryValueKind.DWord);
                    appIdKey.SetValue("RunAs", "Interactive User", RegistryValueKind.String); // Added to ensure that only one copy of the local server runs if the user uses both elevated and non-elevated clients concurrently
                }

                // Set HKCR\APPID\exename.ext
                using (RegistryKey exeNameKey = Registry.ClassesRoot.CreateSubKey($"APPID\\{Application.ExecutablePath.Substring(Application.ExecutablePath.LastIndexOf('\\') + 1)}"))
                {
                    exeNameKey.SetValue("AppID", localServerAppId);
                }
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("RegisterObjects", $"Setting AppID exception: {ex}");
                MessageBox.Show("Error while registering the server:\n" + ex.ToString(), "$safeprojectname$", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // Register each discovered driver
            foreach (Type driverType in driverTypes)
            {
                TL.LogMessage("RegisterObjects", $"Creating COM registration for {driverType.Name}");
                bool bFail = false;
                try
                {
                    // HKCR\CLSID\clsid
                    string clsId = Marshal.GenerateGuidForType(driverType).ToString("B");
                    string progId = Marshal.GenerateProgIdForType(driverType);
                    string deviceType = driverType.Name; // Generate device type from the Class name
                    TL.LogMessage("RegisterObjects", $"Assembly title: {assemblyTitle}, ASsembly description: {assemblyDescription}, CLSID: {clsId}, ProgID: {progId}, Device type: {deviceType}");

                    using (RegistryKey clsIdKey = Registry.ClassesRoot.CreateSubKey($"CLSID\\{clsId}"))
                    {
                        clsIdKey.SetValue(null, progId);
                        clsIdKey.SetValue("AppId", localServerAppId);
                        using (RegistryKey implementedCategoriesKey = clsIdKey.CreateSubKey("Implemented Categories"))
                        {
                            implementedCategoriesKey.CreateSubKey("{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}");
                        }

                        using (RegistryKey progIdKey = clsIdKey.CreateSubKey("ProgId"))
                        {
                            progIdKey.SetValue(null, progId);
                        }
                        clsIdKey.CreateSubKey("Programmable");

                        using (RegistryKey localServer32Key = clsIdKey.CreateSubKey("LocalServer32"))
                        {
                            localServer32Key.SetValue(null, Application.ExecutablePath);
                        }
                    }

                    // HKCR\CLSID\progid
                    using (RegistryKey progIdKey = Registry.ClassesRoot.CreateSubKey(progId))
                    {
                        progIdKey.SetValue(null, assemblyTitle);
                        using (RegistryKey clsIdKey = progIdKey.CreateSubKey("CLSID"))
                        {
                            clsIdKey.SetValue(null, clsId);
                        }
                    }

                    // Pull the display name from the ServedClassName attribute.
                    assemblyTitleAttribute = Attribute.GetCustomAttribute(driverType, typeof(ServedClassNameAttribute));
                    string chooserName = ((ServedClassNameAttribute)assemblyTitleAttribute).DisplayName ?? "MultiServer";
                    TL.LogMessage("RegisterObjects", $"Registering {chooserName} ({driverType.Name}) in Profile");

                    using (var profile = new Profile())
                    {
                        profile.DeviceType = deviceType;
                        profile.Register(progId, chooserName);
                    }
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("RegisterObjects", $"Driver registration exception: {ex}");
                    MessageBox.Show("Error while registering the server:\n" + ex.ToString(), "$safeprojectname$", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    bFail = true;
                }
                finally
                {
                }
                if (bFail) break;
            }
        }

        /// <summary>
        /// Unregister drivers contained in this local server
        /// </summary>
        private static void UnregisterObjects()
        {
            if (!IsAdministrator)
            {
                ElevateSelf("/unregister");
                return;
            }

            // Delete the Local Server's DCOM/AppID information
            Registry.ClassesRoot.DeleteSubKey($"APPID\\{localServerAppId}", false);
            Registry.ClassesRoot.DeleteSubKey($"APPID\\{Application.ExecutablePath.Substring(Application.ExecutablePath.LastIndexOf('\\') + 1)}", false);

            // Delete each driver's COM registration
            foreach (Type driverType in driverTypes)
            {
                TL.LogMessage("UnregisterObjects", $"Removing COM registration for {driverType.Name}");

                string clsId = Marshal.GenerateGuidForType(driverType).ToString("B");
                string progId = Marshal.GenerateProgIdForType(driverType);

                // Remove ProgID entries
                Registry.ClassesRoot.DeleteSubKey($"{progId}\\CLSID", false);
                Registry.ClassesRoot.DeleteSubKey(progId, false);

                // Remove CLSID entries
                Registry.ClassesRoot.DeleteSubKey($"CLSID\\{clsId}\\Implemented Categories\\{{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}}", false);
                Registry.ClassesRoot.DeleteSubKey($"CLSID\\{clsId}\\Implemented Categories", false);
                Registry.ClassesRoot.DeleteSubKey($"CLSID\\{clsId}\\ProgId", false);
                Registry.ClassesRoot.DeleteSubKey($"CLSID\\{clsId}\\LocalServer32", false);
                Registry.ClassesRoot.DeleteSubKey($"CLSID\\{clsId}\\Programmable", false);
                Registry.ClassesRoot.DeleteSubKey($"CLSID\\{clsId}", false);

                // Uncomment the following lines to remove ASCOM Profile information when unregistering.
                // Unregistering often occurs during version upgrades and, if the code below is enabled, will result in loss of all device configuration during the upgrade.
                // For this reason, enabling this capability is not recommended.
                //try
                //{
                //    TL.LogMessage("UnregisterObjects", $"Deleting ASCOM Profile registration for {driverType.Name} ({progId})");
                //    using (var profile = new Profile())
                //    {
                //        profile.DeviceType = driverType.Name;
                //        profile.Unregister(progId);
                //    }
                //}
                //catch (Exception) { }
            }
        }

        /// <summary>
        /// Test whether the session is running with elevated credentials
        /// </summary>
        private static bool IsAdministrator
        {
            get
            {
                WindowsIdentity userIdentity = WindowsIdentity.GetCurrent();
                WindowsPrincipal userPrincipal = new WindowsPrincipal(userIdentity);
                return userPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        /// <summary>
        /// Elevate privileges by re-running ourselves with elevation dialogue
        /// </summary>
        /// <param name="argument">Argument to pass to ourselves</param>
        private static void ElevateSelf(string argument)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.Arguments = argument;
            processStartInfo.WorkingDirectory = Environment.CurrentDirectory;
            processStartInfo.FileName = Application.ExecutablePath;
            processStartInfo.Verb = "runas";
            try
            {
                Process.Start(processStartInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("The $safeprojectname$ was not " + (argument == "/register" ? "registered" : "unregistered because you did not allow it.", "$safeprojectname$", MessageBoxButtons.OK, MessageBoxIcon.Warning));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "$safeprojectname$", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return;
        }

        #endregion

        #region Class Factory Support

        /// <summary>
        /// Register the class factories of drivers that this local server serves.
        /// </summary>
        /// <remarks>This requires the class factory name to be equal to the served class name + "ClassFactory".</remarks>
        /// <returns>True if there are no errors, otherwise false.</returns>
        private static bool RegisterClassFactories()
        {
            classFactories = new ArrayList();
            foreach (Type driverType in driverTypes)
            {
                ClassFactory factory = new ClassFactory(driverType); // Use default context & flags
                classFactories.Add(factory);

                if (!factory.RegisterClassObject())
                {
                    MessageBox.Show("Failed to register class factory for " + driverType.Name, "$safeprojectname$", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            ClassFactory.ResumeClassObjects(); // Served objects now go live
            return true;
        }

        /// <summary>
        /// Revoke the class factories
        /// </summary>
        private static void RevokeClassFactories()
        {
            ClassFactory.SuspendClassObjects(); // Prevent race conditions
            foreach (ClassFactory factory in classFactories)
            {
                factory.RevokeClassObject();
            }
        }

        #endregion

        #region Command Line Arguments

        /// <summary>
        ///Process the command-line arguments returning true to continue execution or false to terminate the application immediately.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool ProcessArguments(string[] args)
        {
            bool returnStatus = true;

            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "-embedding":
                        StartedByCOM = true; // Indicate COM started us and continue
                        returnStatus = true; // Continue on return
                        break;

                    case "-register":
                    case @"/register":
                    case "-regserver": // Emulate VB6
                    case @"/regserver":
                        RegisterObjects(); // Register each served object
                        returnStatus = false; // Terminate on return
                        break;

                    case "-unregister":
                    case @"/unregister":
                    case "-unregserver": // Emulate VB6
                    case @"/unregserver":
                        UnregisterObjects(); //Unregister each served object
                        returnStatus = false; // Terminate on return
                        break;

                    default:
                        MessageBox.Show("Unknown argument: " + args[0] + "\nValid are : -register, -unregister and -embedding", "$safeprojectname$", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                }
            }
            else
            {
                StartedByCOM = false;
            }

            return returnStatus;
        }

        #endregion

        #region SERVER ENTRY POINT (main)

        /// <summary>
        /// Main server entry point
        /// </summary>
        /// <param name="args">Command line parameters</param>
        [STAThread]
        static void Main(string[] args)
        {
            // Create a trace logger for the local server executable itself as opposed to the served drivers.
            TL = new TraceLogger("", "ASCOM.TEMPLATEDEVICENAME.Server")
            {
                Enabled = true // Enable to debug the local server itself as opposed to the driver, which has its own trace logger.
            };

            TL.LogMessage("Main", $"Server started");

            if (!PopulateAscomDriverAssemblyList()) return; // Load served COM class assemblies, get types

            if (!ProcessArguments(args)) return; // Process command line arguments to Register/Unregister

            // Initialize critical member variables.
            driversInUseCount = 0;
            serverLockCount = 0;
            MainThreadId = GetCurrentThreadId();
            Thread.CurrentThread.Name = "Main Thread";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            localServerMainForm = new frmMain();
            if (StartedByCOM) localServerMainForm.WindowState = FormWindowState.Minimized;

            // Register the class factories of the served objects
            RegisterClassFactories();
            TL.LogMessage("Main", $"Registered class factories");

            // Start up the garbage collection thread.
            GarbageCollection GarbageCollector = new GarbageCollection(1000);
            Thread GCThread = new Thread(new ThreadStart(GarbageCollector.GCWatch));
            GCThread.Name = "Garbage Collection Thread";
            GCThread.Start();
            TL.LogMessage("Main", $"Garbage collector thread started");

            //
            // Start the message loop. This serializes incoming calls to our
            // served COM objects, making this act like the VB6 equivalent!
            //
            try
            {
                TL.LogMessage("Main", $"Starting main form");
                Application.Run(localServerMainForm);
                TL.LogMessage("Main", $"Main form has ended");
            }
            finally
            {
                // Revoke the class factories immediately without waiting until the thread has stopped
                TL.LogMessage("Main", $"Revoking class factories");
                RevokeClassFactories();
                TL.LogMessage("Main", $"Class factories revoked");

                // Now stop the Garbage Collector thread.
                TL.LogMessage("Main", $"Stopping garbage collector");
                GarbageCollector.StopThread();
                GarbageCollector.WaitForThreadToStop();
            }

            TL.LogMessage("Main", $"Local server closing");
            TL.Dispose();

        }

        #endregion
    }
}
