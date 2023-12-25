using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using ASCOM.Tools;
using static ASCOM.Utilities.Global;

namespace ASCOM.DynamicRemoteClients
{
    static class CreateAlpacaClients
    {
        private const int LOCALSERVER_WAIT_TIME = 5000; // Length of time (milliseconds) to wait for the local server to (un)register its drivers

        // Constants used by the generated dynamic client driver assembly
        public const string ALPACA_CLIENT_LOCAL_SERVER_PATH = @"\ASCOM\AlpacaDynamicClients\"; // Relative path from CommonFiles
        public const string ALPACA_CLIENT_LOCAL_SERVER = @"DynamicClientServer.exe"; // Name of the remote client local server application
        public const string DRIVER_PROGID_BASE = "ASCOM.AlpacaDynamic";
        public const string LOCALHOST_NAME_IPV4 = "127.0.0.1";

        public const string IPADDRESS_PROFILENAME = "IP Address";
        public const string PORTNUMBER_PROFILENAME = "Port Number";
        public const string REMOTE_DEVICE_NUMBER_PROFILENAME = "Remote Device Number";
        public const string UNIQUEID_PROFILENAME = "UniqueID";

        // List of supported device types - this must be kept in sync with the device type numeric up-down controls on the form dialogue!
        private static readonly List<string> supportedDeviceTypes = new() { "Camera", "CoverCalibrator", "Dome", "FilterWheel", "Focuser", "ObservingConditions", "Rotator", "SafetyMonitor", "Switch", "Telescope" };

        static Utilities.TraceLogger TL;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string errMsg;

            // Add un-handled exception handlers           
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException); // Add the event handler for handling UI thread exceptions to the event.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException); // Set the un-handled exception mode to force all exceptions to go through our handler.
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException); // Add the event handler for handling non-UI thread exceptions to the event. 

            TL = new Utilities.TraceLogger("", "AlpacaDynamicClientManager")
            {
                Enabled = GetBool(TRACE_UTIL, TRACE_UTIL_DEFAULT)
            };

            try
            {
                string commandParameter = ""; // Initialise the supplied parameter to empty string

                TL.LogMessage("Main", $"Number of parameters: {args.Length}");
                foreach (string arg in args)
                {
                    TL.LogMessage("Main", $"Received parameter: \"{arg}\"");
                }

                if (args.Length > 0) commandParameter = args[0]; // Copy any supplied command parameter to the parameter variable

                TL.LogMessage("Main", string.Format(@"Supplied parameter: ""{0}""", commandParameter));
                commandParameter = commandParameter.TrimStart(' ', '-', '/', '\\'); // Remove any parameter prefixes and leading spaces
                commandParameter = commandParameter.TrimEnd(' '); // Remove any trailing spaces

                TL.LogMessage("Main", string.Format(@"Trimmed parameter: ""{0}""", commandParameter));

                switch (commandParameter.ToUpperInvariant()) // Act on the supplied parameter, if any
                {
                    case "":
                    case "MANAGEDEVICES":

                        // Run the application in user interactive mode
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        TL.LogMessage("Main", "Starting device management form");
                        Application.Run(new ManageDevicesForm(TL));

                        break;

                    case "CREATEALPACACLIENT":

                        // Validate supplied parameters before passing to the execution method
                        if (args.Length < 5)
                        {
                            // Validate the number of parameters - must be 5: Command DeviceType COMDeviceNumber ProgID DeviceName
                            errMsg = $"The CreateAlpacaClient command requires 4 parameters: DeviceType COMDeviceNumber ProgID DeviceName e.g. /CreateAlpacaClient Telescope 1 ASCOM.AlpacaDynamic1.Telescope \"Device Chooser description\"";
                            TL.LogMessage("CreateAlpacaClient", errMsg);
                            MessageBox.Show(errMsg, "ASCOM Dynamic Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Validate that the supplied device type is one that is supported for Alpaca
                        if (!supportedDeviceTypes.Contains(args[1], StringComparer.OrdinalIgnoreCase))
                        {
                            errMsg = $"The supplied ASCOM device type '{args[1]}' is not supported: The command format is \"/CreateAlpacaClient ASCOMDeviceType AlpacaDeviceUniqueID\" e.g. /CreateAlpacaClient Telescope 84DC2495-CBCE-4A9C-A703-E342C0E1F651";
                            TL.LogMessage("CreateAlpacaClient", errMsg);
                            MessageBox.Show(errMsg, "ASCOM Dynamic Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Validate that the supplied device number is an integer
                        int comDevicenumber;
                        bool comDevicenunberIsInteger = int.TryParse(args[2], out comDevicenumber);
                        if (!comDevicenunberIsInteger)
                        {
                            errMsg = $"The supplied COM device number is not an integer: {args[2]}";
                            TL.LogMessage("CreateAlpacaClient", errMsg);
                            MessageBox.Show(errMsg, "ASCOM Dynamic Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Get the local server folder
                        string localServerPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + ALPACA_CLIENT_LOCAL_SERVER_PATH;
                        TL.LogMessage("CreateAlpacaClient", $"Alpaca local server folder: {localServerPath}");

                        // Run the create device form to obtain the device description and create the driver
                        // CreateAlpacaClient(args[1], comDevicenumber, args[3], args[4], localServerPath); // Call the execution method with correctly cased device type and unique ID parameters

                        // Register the dynamic clients
                        string localServerExe = $"{localServerPath}{ALPACA_CLIENT_LOCAL_SERVER}";
                        TL.LogMessage("CreateAlpacaClient", $"Alpaca local server exe name: {localServerExe}");
                        RunLocalServer(localServerExe, "-regserver", TL);
                        break;

                    case "CREATENAMEDCLIENT":

                        // Validate supplied parameters before passing to the execution method
                        if (args.Length < 4)
                        {
                            // Validate the number of parameters - must be 4: Command DeviceType COMDeviceNumber ProgID
                            errMsg = $"The CreateAlpacaClient command requires 3 parameters: DeviceType COMDeviceNumber ProgID e.g. /CreateAlpacaClient Telescope 1 ASCOM.AlpacaDynamic1.Telescope";
                            TL.LogMessage("CreateAlpacaClient", errMsg);
                            MessageBox.Show(errMsg, "ASCOM Dynamic Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Validate that the supplied device type is one that is supported for Alpaca
                        if (!supportedDeviceTypes.Contains(args[1], StringComparer.OrdinalIgnoreCase))
                        {
                            errMsg = $"The supplied ASCOM device type '{args[1]}' is not supported: The command format is \"/CreateAlpacaClient ASCOMDeviceType AlpacaDeviceUniqueID\" e.g. /CreateAlpacaClient Telescope 84DC2495-CBCE-4A9C-A703-E342C0E1F651";
                            TL.LogMessage("CreateAlpacaClient", errMsg);
                            MessageBox.Show(errMsg, "ASCOM Dynamic Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Validate that the supplied device number is an integer
                        comDevicenunberIsInteger = int.TryParse(args[2], out comDevicenumber);
                        if (!comDevicenunberIsInteger)
                        {
                            errMsg = $"The supplied COM device number is not an integer: {args[2]}";
                            TL.LogMessage("CreateAlpacaClient", errMsg);
                            MessageBox.Show(errMsg, "ASCOM Dynamic Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        localServerPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + ALPACA_CLIENT_LOCAL_SERVER_PATH;
                        TL.LogMessage("CreateAlpacaClient", $"Alpaca local server folder: {localServerPath}");

                        // The supplied parameters pass validation so run the create device form to obtain the device description and create the driver
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        TL.LogMessage("Main", "Starting device creation form");
                        Application.Run(new CreateDeviceForm(args[1], comDevicenumber, args[3], localServerPath, TL));

                        break;

                    default: // Unrecognised parameter so flag this to the user
                        errMsg = $"Unrecognised command: '{commandParameter}', the valid command are:\r\n" +
                            $"/CreateAlpacaClient DeviceType COMDeviceNumber ProgID DeviceName\r\n" +
                            $"CreateNamedClient DeviceType COMDeviceNumber ProgID\r\n" +
                            $"/ManageDevices";
                        TL.LogMessage("Main", errMsg);
                        MessageBox.Show(errMsg, "ASCOM Dynamic Clients", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                errMsg = ("DynamicRemoteClients exception: " + ex.ToString());
                TL.LogMessage("Main", errMsg);
                MessageBox.Show(errMsg, "ASCOM Dynamic Clients", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            TL.Enabled = false;
            TL.Dispose();
            TL = null;
        }

        #region Support code

        /// <summary>
        /// Run the local server to register and unregister DynamicRemoteClient clients
        /// </summary>
        /// <param name="localServerExe"></param>
        /// <param name="serverParameter"></param>
        internal static void RunLocalServer(string localServerExe, string serverParameter, Utilities.TraceLogger TL)
        {
            bool exitOK;
            int exitCode = int.MinValue;

            // Set local server run time values
            ProcessStartInfo start = new()
            {
                Arguments = serverParameter, // Specify the server command parameter
                FileName = localServerExe, // Set the full local server executable path
                WindowStyle = ProcessWindowStyle.Hidden, // Don't show a window while the command runs
                CreateNoWindow = true
            };

            // Run the external process & wait for it to finish
            TL.LogMessage("RunLocalServer", $"Starting server with parameter {serverParameter}...");
            using (Process proc = Process.Start(start))
            {
                exitOK = proc.WaitForExit(LOCALSERVER_WAIT_TIME);
                if (exitOK) exitCode = proc.ExitCode; // Save the exit code
            }

            if (exitOK) TL.LogMessage("RunLocalServer", $"Local server exited OK with return code: {exitCode:X8}");
            else
            {
                string errorMessage = $"local server did not complete within {LOCALSERVER_WAIT_TIME} milliseconds, return code: {exitCode}";
                TL.LogMessage("RunLocalServer", errorMessage);
                MessageBox.Show(errorMessage, "ASCOM Dynamic Clients", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Unhandled exception handlers

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

            // Create a trace logger and log the exception 
            TraceLogger TL = new("DynamicClientThreadException",true)
            {
                Enabled = true
            };
            TL.LogMessage("Main", string.Format("ASCOM Dynamic Client Manager - Thread exception. Version: {0}", assemblyVersion.ToString()));
            TL.LogMessage("Main", e.Exception.ToString());

            // Display the exception in the default .txt editor and exit
            Process.Start(TL.LogFileName);

            TL.Enabled = false;
            TL.Dispose();

            Environment.Exit(0);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = (Exception)e.ExceptionObject;

            Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

            // Create a trace logger and log the exception 
            TraceLogger TL = new("DynamicClientUnhandledException",true)
            {
                Enabled = true
            };
            TL.LogMessage("Main", string.Format("ASCOM Dynamic Client Manager - Unhandled exception. Version: {0}", assemblyVersion.ToString()));
            TL.LogMessage("Main", exception.ToString());

            // Display the exception in the default .txt editor and exit
            Process.Start(TL.LogFileName);

            TL.Enabled = false;
            TL.Dispose();

            Environment.Exit(0);
        }

        #endregion
    }
}
