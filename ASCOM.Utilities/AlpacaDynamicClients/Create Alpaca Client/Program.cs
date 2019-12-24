using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ASCOM.Utilities;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using ASCOM.Remote;

namespace ASCOM.DynamicRemoteClients
{
    static class Program
    {
        const string ALREADY_RUN = "Client installer already run";

        static TraceLogger TL;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            // Set the unhandled exception mode to force all exceptions to go through our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            TL = new TraceLogger("", "DynamicClients");
            TL.Enabled = true;

            try
            {
                string parameter = ""; // Initialise the supplied parameter to empty string
                if (args.Length > 0) parameter = args[0]; // Copy any supplied parameter to the parameter variable

                TL.LogMessage("Main", string.Format(@"Supplied parameter: ""{0}""", parameter));
                parameter = parameter.TrimStart(' ', '-', '/', '\\'); // Remove any parameter prefixes and leading spaces
                parameter = parameter.TrimEnd(' '); // Remove any trailing spaces

                TL.LogMessage("Main", string.Format(@"Trimmed parameter: ""{0}""", parameter));

                switch (parameter.ToUpperInvariant()) // Act on the supplied parameter, if any
                {
                    case "": // Run the application in user interactive mode
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        TL.LogMessage("Main", "Starting application form");
                        Application.Run(new Form1(TL));
                        break;

                    case "INSTALLERSETUP": // Called by the installer to create drivers on first time use
                        TL.LogMessage("Main", "Running installer setup");

                        // Find if there are any driver files already installed, indicating that this is not a first time install
                        string localServerPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + Form1.REMOTE_SERVER_PATH;
                        string deviceType = "*";
                        string searchPattern = string.Format(Form1.REMOTE_CLIENT_DRIVER_NAME_TEMPLATE, deviceType);

                        TL.LogMessage("Main", "About to create base key");
                        RegistryKey remoteRegistryKey = RegistryKey.OpenBaseKey(SharedConstants.ASCOM_REMOTE_CONFIGURATION_HIVE, RegistryView.Default).CreateSubKey(SharedConstants.ASCOM_REMOTE_CONFIGURATION_KEY, true);
                        bool alreadyRun = bool.Parse((string)remoteRegistryKey.GetValue(ALREADY_RUN, "false"));
                        TL.LogMessage("Main", string.Format("Already run: {0}", alreadyRun));

                        if (!alreadyRun) // We have not run yet 
                        {
                            TL.LogMessage("Main", string.Format("First time setup - migrating profiles and creating dynamic drivers"));

                            // Only attempt first time setup if the local server executable is present
                            string localServerExe = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + Form1.REMOTE_SERVER_PATH + Form1.REMOTE_SERVER; // Get the local server path
                            if (File.Exists(localServerExe)) // Local server does exist
                            {
                                // Migrate any ASCOM.WebX.DEVICE profile entries to ASCOM.RemoteX.DEVICE profile entries
                                TL.LogMessage("Main", string.Format("Migrating any ASCOM.WebX.DEVICETYPE profiles to ASCOM.RemoteX.DEVICETYPE"));
                                MigrateProfiles("Camera");
                                MigrateProfiles("Dome");
                                MigrateProfiles("FilterWheel");
                                MigrateProfiles("Focuser");
                                MigrateProfiles("ObservingConditions");
                                MigrateProfiles("Rotator");
                                MigrateProfiles("SafetyMonitor");
                                MigrateProfiles("Switch");
                                MigrateProfiles("Telescope");

                                // Remove any driver or pdb driver files left in the , local server directory
                                DeleteFiles(localServerPath, @"ascom\.remote\d\.\w+\.dll", "ASCOM.RemoteX.DLL");
                                DeleteFiles(localServerPath, @"ascom\.remote\d\.\w+\.pdb", "ASCOM.RemoteX.PDB");
                                DeleteFiles(localServerPath, @"ascom\.web\d\.\w+\.dll", "ASCOM.WebX.DLL");
                                DeleteFiles(localServerPath, @"ascom\.web\d\.\w+\.pdb", "ASCOM.WebX.PDB");

                                // Create the required drivers
                                TL.LogMessage("Main", string.Format("Creating one remote client driver of each device type"));
                                Form1.CreateDriver("Camera", 1, localServerPath, TL);
                                Form1.CreateDriver("Dome", 1, localServerPath, TL);
                                Form1.CreateDriver("FilterWheel", 1, localServerPath, TL);
                                Form1.CreateDriver("Focuser", 1, localServerPath, TL);
                                Form1.CreateDriver("ObservingConditions", 1, localServerPath, TL);
                                Form1.CreateDriver("Rotator", 1, localServerPath, TL);
                                Form1.CreateDriver("SafetyMonitor", 1, localServerPath, TL);
                                Form1.CreateDriver("Switch", 1, localServerPath, TL);
                                Form1.CreateDriver("Telescope", 1, localServerPath, TL);

                                // Register the drivers
                                TL.LogMessage("Main", "Registering drivers");
                                Form1.RunLocalServer(localServerExe, "-regserver", TL);

                                // Record that we have run once on this PC
                                TL.LogMessage("Main", string.Format("Setting already run to true"));
                                remoteRegistryKey.SetValue(ALREADY_RUN, "true");
                                TL.LogMessage("Main", string.Format("Set already run to true"));
                            }
                            else // Local server can not be found so report the issue
                            {
                                string errorMessage = string.Format("Could not find local server {0}, unable to register drivers", localServerExe);
                                TL.LogMessage("Main", errorMessage);
                                MessageBox.Show(errorMessage);
                            }
                        }
                        else // Drivers are already installed so no action required
                        {
                            TL.LogMessage("Main", string.Format("This application has already run successful so this is not a first time installation - no action taken"));
                        }
                        break;

                    default: // Unrecognised parameter so flag this to the user
                        string errMsg = string.Format("Unrecognised parameter: {0}, the only valid value is /InstallerSetup", parameter);

                        MessageBox.Show(errMsg);
                        break;
                }
            }
            catch (Exception ex)
            {
                string errMsg = ("DynamicRemoteClients exception: " + ex.ToString());
                TL.LogMessageCrLf("Main", errMsg);
                MessageBox.Show(errMsg);
            }

            TL.Enabled = false;
            TL.Dispose();
            TL = null;
        }

        /// <summary>
        /// Migrate profile names from previous client version
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <remarks>The first ASCOM Remote release used device ProgIds of the form ASCOM.WebX.DEVICETYPE. This and future versions use ProgIDs of the form ASCOM.RemoteX.DEVICETYPE. This routine renames the 
        /// Profile entries of any original release drivers to match the new form.</remarks>
        private static void MigrateProfiles(string DeviceType)
        {
            const string IP_ADDRESS_NOT_PRESENT = "IP Address NOT present";
            try
            {
                TL.LogMessage("MigrateProfiles", string.Format("Migrating device type {0}", DeviceType));

                for (int i = 1; i <= 2; i++)
                {
                    string webProgIdKeyName = string.Format(@"SOFTWARE\ASCOM\{0} Drivers\ASCOM.Web{1}.{0}", DeviceType, i);
                    TL.LogMessage("MigrateProfiles", string.Format("Processing registry value: {0}", webProgIdKeyName));

                    RegistryKey webProgIdKey = Registry.LocalMachine.OpenSubKey(webProgIdKeyName, true); // Open the key for writing
                    if (!(webProgIdKey == null)) // ProgID exists
                    {
                        string ipAddress = (string)webProgIdKey.GetValue("IP Address", IP_ADDRESS_NOT_PRESENT);
                        TL.LogMessage("MigrateProfiles", string.Format("Found IP Address: {0}", ipAddress));

                        if (!(ipAddress == IP_ADDRESS_NOT_PRESENT))// IP Address value exists so we need to try and rename this profile
                        {
                            // Does a current ASCOM>remoteX profile exist, if so then we can't migrate so leave as is
                            string remoteProgIdkeyName = string.Format(@"SOFTWARE\ASCOM\{0} Drivers\ASCOM.Remote{1}.{0}", DeviceType, i);
                            TL.LogMessage("MigrateProfiles", string.Format("Checking whether registry key {0} exists", remoteProgIdkeyName));

                            RegistryKey remoteProgIdKey = Registry.LocalMachine.OpenSubKey(remoteProgIdkeyName, true); // Open the key for writing
                            if (remoteProgIdKey == null) // The "Remote" Profile does not exist so we can just rename the "Web" profile
                            {
                                TL.LogMessage("MigrateProfiles", string.Format("The registry key {0} does not exist - creating it", remoteProgIdkeyName));
                                Registry.LocalMachine.CreateSubKey(remoteProgIdkeyName, true);
                                remoteProgIdKey = Registry.LocalMachine.OpenSubKey(remoteProgIdkeyName, true); // Open the key for writing
                                TL.LogMessage("MigrateProfiles", string.Format("Registry key {0} created OK", remoteProgIdkeyName));
                                string[] valueNames = webProgIdKey.GetValueNames();
                                foreach (string valueName in valueNames)
                                {
                                    string value;

                                    if (valueName == "") // Special handling for the default value - need to change chooser description to ASCOM Remote Client X
                                    {
                                        value = string.Format("ASCOM Remote Client {0}", i);
                                        TL.LogMessage("MigrateProfiles", string.Format("Changing Chooser description to {0} ", value));
                                    }
                                    else
                                    {
                                        value = (string)webProgIdKey.GetValue(valueName);
                                        TL.LogMessage("MigrateProfiles", string.Format("Found Web registry value name {0} = {1}", valueName, value));
                                    }
                                    TL.LogMessage("MigrateProfiles", string.Format("Setting Remote registry value {0} to {1}", valueName, value));
                                    remoteProgIdKey.SetValue(valueName, value);
                                }
                                TL.LogMessage("MigrateProfiles", string.Format("Driver successfully migrated - deleting Profile {0}", webProgIdKeyName));
                                Registry.LocalMachine.DeleteSubKey(webProgIdKeyName);
                                TL.LogMessage("MigrateProfiles", string.Format("Successfully deleted Profile {0}", webProgIdKeyName));
                            }
                            else // The "Remote" profile already exists so we can't migrate the old "Web" profile
                            {
                                TL.LogMessage("MigrateProfiles", string.Format("The {0} key already exists so we cannot migrate the {1} profile - no action taken", remoteProgIdkeyName, webProgIdKeyName));
                            }
                        }
                        else // No IP address value means that this profile is unconfigured so just delete it.
                        {
                            TL.LogMessage("MigrateProfiles", string.Format("Driver not configured - deleting Profile {0}", webProgIdKeyName));
                            Registry.LocalMachine.DeleteSubKey(webProgIdKeyName);
                            TL.LogMessage("MigrateProfiles", string.Format("Successfully deleted Profile {0}", webProgIdKeyName));
                        }
                    }
                    else // ProgID doesn't exist
                    {
                        TL.LogMessage("MigrateProfiles", string.Format("ProgId {0} does not exist - no action taken", webProgIdKeyName));
                    }
                }
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("MigrateProfiles", ex.ToString());
            }
        }

        /// <summary>
        /// Deletes files based on supplied Regex definition
        /// </summary>
        /// <param name="path">Directory containing files to be deleted</param>
        /// <param name="fileSpecifier">Regex expression specifying the files to delete</param>
        private static void DeleteFiles(string path, string fileSpecifier, string description)
        {
            IEnumerable<string> files = Directory.EnumerateFiles(path).Where(name => Regex.IsMatch(name, fileSpecifier, RegexOptions.IgnoreCase));
            TL.LogMessage("DeleteFiles", string.Format("Found {0} {1} files to delete", files.Count(), description));
            if (files.Count() != 0) // Delete extraneous pdb files
            {
                foreach (string file in files)
                {
                    TL.LogMessage("DeleteFiles", string.Format("Deleting file {0}", file));
                    try
                    {
                        File.Delete(file);
                        TL.LogMessage("DeleteFiles", string.Format("Successfully deleted file {0}", file));
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = string.Format("Unable to delete file {0} - {1}", file, ex.Message);
                        TL.LogMessage("DeleteFiles", errorMessage);
                    }
                }
            }
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

            // Create a trace logger and log the exception 
            TraceLogger TL = new TraceLogger("DynamicClientThreadException")
            {
                Enabled = true
            };
            TL.LogMessage("Main", string.Format("ASCOM Remote Dynamic Client Manager - Thread exception. Version: {0}", assemblyVersion.ToString()));
            TL.LogMessageCrLf("Main", e.Exception.ToString());

            // Display the exception in the default .txt editor and exit
            Process.Start(TL.LogFileName);

            TL.Enabled = false;
            TL.Dispose();
            TL = null;

            Environment.Exit(0);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = (Exception)e.ExceptionObject;

            Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

            // Create a trace logger and log the exception 
            TraceLogger TL = new TraceLogger("DynamicClientUnhandledException")
            {
                Enabled = true
            };
            TL.LogMessage("Main", string.Format("ASCOM Remote Dynamic Client Manager - Unhandled exception. Version: {0}", assemblyVersion.ToString()));
            TL.LogMessageCrLf("Main", exception.ToString());

            // Display the exception in the default .txt editor and exit
            Process.Start(TL.LogFileName);

            TL.Enabled = false;
            TL.Dispose();
            TL = null;

            Environment.Exit(0);
        }

    }
}
