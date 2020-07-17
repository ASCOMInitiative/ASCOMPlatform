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
using System.CodeDom;
using System.CodeDom.Compiler;

namespace ASCOM.DynamicRemoteClients
{
    static class CreateAlpacaClients
    {
        private const string BASE_CLASS_POSTFIX = "BaseClass"; // Postfix to the device type to create the base class name e.g. "CamerabaseClass". Must match the last characters of the device base class names!
        private const int LOCALSERVER_WAIT_TIME = 5000; // Length of time (milliseconds) to wait for the local server to (un)register its drivers

        // List of supported device types - this must be kept in sync with the device type numeric up-down controls on the form dialogue!
        private static readonly List<string> supportedDeviceTypes = new List<string>() { "Camera", "CoverCalibrator", "Dome", "FilterWheel", "Focuser", "ObservingConditions", "Rotator", "SafetyMonitor", "Switch", "Telescope" };

        static TraceLogger TL;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string errMsg;

            // Add unhandled exception handlers           
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException); // Add the event handler for handling UI thread exceptions to the event.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException); // Set the unhandled exception mode to force all exceptions to go through our handler.
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException); // Add the event handler for handling non-UI thread exceptions to the event. 

            TL = new TraceLogger("", "AlpacaDynamicClientManager");
            TL.Enabled = RegistryCommonCode.GetBool(GlobalConstants.TRACE_UTIL, GlobalConstants.TRACE_UTIL_DEFAULT);

            try
            {
                string commandParameter = ""; // Initialise the supplied parameter to empty string

                TL.LogMessage("Main", $"Number of parameters: {args.Count()}");
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
                            TL.LogMessageCrLf("CreateAlpacaClient", errMsg);
                            MessageBox.Show(errMsg, "ASCOM Dynamic Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Validate that the supplied device type is one that is supported for Alpaca
                        if (!supportedDeviceTypes.Contains(args[1], StringComparer.OrdinalIgnoreCase))
                        {
                            errMsg = $"The supplied ASCOM device type '{args[1]}' is not supported: The command format is \"/CreateAlpacaClient ASCOMDeviceType AlpacaDeviceUniqueID\" e.g. /CreateAlpacaClient Telescope 84DC2495-CBCE-4A9C-A703-E342C0E1F651";
                            TL.LogMessageCrLf("CreateAlpacaClient", errMsg);
                            MessageBox.Show(errMsg, "ASCOM Dynamic Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Validate that the supplied device number is an integer
                        int comDevicenumber;
                        bool comDevicenunberIsInteger = int.TryParse(args[2], out comDevicenumber);
                        if (!comDevicenunberIsInteger)
                        {
                            errMsg = $"The supplied COM device number is not an integer: {args[2]}";
                            TL.LogMessageCrLf("CreateAlpacaClient", errMsg);
                            MessageBox.Show(errMsg, "ASCOM Dynamic Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string localServerPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + SharedConstants.ALPACA_CLIENT_LOCAL_SERVER_PATH;
                        TL.LogMessage("CreateAlpacaClient", $"Alpaca local server folder: {localServerPath}");

                        // The supplied parameters pass validation so run the create device form to obtain the device description and create the driver
                        CreateAlpacaClient(args[1], comDevicenumber, args[3], args[4], localServerPath); // Call the execution method with correctly cased device type and unique ID parameters
                        string localServerExe = $"{localServerPath}\\{SharedConstants.ALPACA_CLIENT_LOCAL_SERVER}";
                        TL.LogMessage("CreateAlpacaClient", $"Alpaca local server exe name: {localServerExe}");
                        RunLocalServer(localServerExe, "-regserver", TL);

                        break;

                    case "CREATENAMEDCLIENT":

                        // Validate supplied parameters before passing to the execution method
                        if (args.Length < 4)
                        {
                            // Validate the number of parameters - must be 4: Command DeviceType COMDeviceNumber ProgID
                            errMsg = $"The CreateAlpacaClient command requires 3 parameters: DeviceType COMDeviceNumber ProgID e.g. /CreateAlpacaClient Telescope 1 ASCOM.AlpacaDynamic1.Telescope";
                            TL.LogMessageCrLf("CreateAlpacaClient", errMsg);
                            MessageBox.Show(errMsg, "ASCOM Dynamic Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Validate that the supplied device type is one that is supported for Alpaca
                        if (!supportedDeviceTypes.Contains(args[1], StringComparer.OrdinalIgnoreCase))
                        {
                            errMsg = $"The supplied ASCOM device type '{args[1]}' is not supported: The command format is \"/CreateAlpacaClient ASCOMDeviceType AlpacaDeviceUniqueID\" e.g. /CreateAlpacaClient Telescope 84DC2495-CBCE-4A9C-A703-E342C0E1F651";
                            TL.LogMessageCrLf("CreateAlpacaClient", errMsg);
                            MessageBox.Show(errMsg, "ASCOM Dynamic Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Validate that the supplied device number is an integer
                        comDevicenunberIsInteger = int.TryParse(args[2], out comDevicenumber);
                        if (!comDevicenunberIsInteger)
                        {
                            errMsg = $"The supplied COM device number is not an integer: {args[2]}";
                            TL.LogMessageCrLf("CreateAlpacaClient", errMsg);
                            MessageBox.Show(errMsg, "ASCOM Dynamic Client Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        localServerPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + SharedConstants.ALPACA_CLIENT_LOCAL_SERVER_PATH;
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
                TL.LogMessageCrLf("Main", errMsg);
                MessageBox.Show(errMsg, "ASCOM Dynamic Clients", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            TL.Enabled = false;
            TL.Dispose();
            TL = null;
        }

        #region Support code

        /// <summary>
        /// Create a new Alpaca client executable using the supplied parameters
        /// </summary>
        /// <param name="DeviceType">The ASCOM device type to create</param>
        /// <param name="DeviceNumber">The number of this device type to create</param>
        /// <param name="OutputDirectory">The directory in which to place the compiled assembly</param>
        /// <remarks>
        /// This subroutine creates compiler source line definitions (not source code as such) and stores them in memory
        /// When complete, the compiler is called and the resultant assembly is stored in the specified output directory.
        /// The code created has no function as such it is just a shell with all of the heavy lifting undertaken by an inherited base class that is supplied pre-compiled
        /// The resultant assembly for Camera device 1 has this form:
        /// 
        /// using System;
        /// using System.Runtime.InteropServices;
        /// namespace ASCOM.DynamicRemoteClients
        /// {
        ///     [Guid("70495DF9-C01E-4987-AE49-E12967202C7F")]             <====> The GUID is dynamically created on the user's machine so that it is unique for every driver
        ///     [ProgId(DRIVER_PROGID)]
        ///     [ServedClassName(DRIVER_DISPLAY_NAME)]
        ///     [ClassInterface(ClassInterfaceType.None)]
        ///     public class Camera : CameraBaseClass                      <====> Created from supplied parameters - all executable code is in the base class
        ///     {
        ///         private const string DRIVER_NUMBER = "1";              <====> Created from supplied parameters
        ///         private const string DEVICE_TYPE = "Camera";           <====> Created from supplied parameters
        ///         private const string DRIVER_DISPLAY_NAME = SharedConstants.DRIVER_DISPLAY_NAME + " " + DRIVER_NUMBER;
        ///         private const string DRIVER_PROGID = SharedConstants.DRIVER_PROGID_BASE + DRIVER_NUMBER + "." + DEVICE_TYPE;
        ///         public Camera() : base(DRIVER_NUMBER, DRIVER_DISPLAY_NAME, DRIVER_PROGID)
        ///         {
        ///         }
        ///     }
        /// }
        /// </remarks>
        internal static void CreateAlpacaClient(string DeviceType, int DeviceNumber, string ProgId, string DisplayName, string localServerPath)
        {
            TL.LogMessage("CreateAlpacaClient", $"Creating new ProgID: for {DeviceType} device {DeviceNumber} with ProgID: {ProgId} and display name: {DisplayName}");

            try
            {
                // Generate the container unit
                CodeCompileUnit program = new CodeCompileUnit();

                // Generate the namespace
                CodeNamespace ns = new CodeNamespace("ASCOM.DynamicRemoteClients");

                // Add required imports
                ns.Imports.Add(new CodeNamespaceImport("System"));
                ns.Imports.Add(new CodeNamespaceImport("System.Runtime.InteropServices"));

                // Declare the device class
                CodeTypeDeclaration deviceClass = new CodeTypeDeclaration()
                {
                    Name = DeviceType,
                    IsClass = true
                };

                // Add the class base type
                deviceClass.BaseTypes.Add(new CodeTypeReference { BaseType = DeviceType + BASE_CLASS_POSTFIX });
                TL.LogMessage("CreateAlpacaClient", "Created base type");

                // Create custom attributes to decorate the class
                CodeAttributeDeclaration guidAttribute = new CodeAttributeDeclaration("Guid", new CodeAttributeArgument(new CodePrimitiveExpression(Guid.NewGuid().ToString())));
                CodeAttributeDeclaration progIdAttribute = new CodeAttributeDeclaration("ProgId", new CodeAttributeArgument(new CodeArgumentReferenceExpression("DRIVER_PROGID")));
                CodeAttributeDeclaration servedClassNameAttribute = new CodeAttributeDeclaration("ServedClassName", new CodeAttributeArgument(new CodeArgumentReferenceExpression("DRIVER_DISPLAY_NAME")));
                CodeAttributeDeclaration classInterfaceAttribute = new CodeAttributeDeclaration("ClassInterface", new CodeAttributeArgument(new CodeArgumentReferenceExpression("ClassInterfaceType.None")));
                CodeAttributeDeclarationCollection customAttributes = new CodeAttributeDeclarationCollection() { guidAttribute, progIdAttribute, servedClassNameAttribute, classInterfaceAttribute };
                TL.LogMessage("CreateAlpacaClient", "Created custom attributes");

                // Add the custom attributes to the class
                deviceClass.CustomAttributes = customAttributes;

                // Create some class level private constants
                CodeMemberField driverNumberConst = new CodeMemberField(typeof(string), "DRIVER_NUMBER");
                driverNumberConst.Attributes = MemberAttributes.Private | MemberAttributes.Const;
                driverNumberConst.InitExpression = new CodePrimitiveExpression(DeviceNumber.ToString());

                CodeMemberField deviceTypeConst = new CodeMemberField(typeof(string), "DEVICE_TYPE");
                deviceTypeConst.Attributes = MemberAttributes.Private | MemberAttributes.Const;
                deviceTypeConst.InitExpression = new CodePrimitiveExpression(DeviceType);

                CodeMemberField driverDisplayNameConst = new CodeMemberField(typeof(string), "DRIVER_DISPLAY_NAME");
                driverDisplayNameConst.Attributes = (MemberAttributes.Private | MemberAttributes.Const);
                driverDisplayNameConst.InitExpression = new CodePrimitiveExpression(DisplayName);

                CodeMemberField driverProgIDConst = new CodeMemberField(typeof(string), "DRIVER_PROGID");
                driverProgIDConst.Attributes = MemberAttributes.Private | MemberAttributes.Const;
                driverProgIDConst.InitExpression = new CodePrimitiveExpression(ProgId);

                // Add the constants to the class
                deviceClass.Members.AddRange(new CodeMemberField[] { driverNumberConst, deviceTypeConst, driverDisplayNameConst, driverProgIDConst });
                TL.LogMessage("CreateAlpacaClient", "Added constants to class");

                // Declare the class constructor
                CodeConstructor constructor = new CodeConstructor();
                constructor.Attributes = MemberAttributes.Public | MemberAttributes.Final;

                // Add a call to the base class with required parameters
                constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("DRIVER_NUMBER"));
                constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("DRIVER_DISPLAY_NAME"));
                constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("DRIVER_PROGID"));
                deviceClass.Members.Add(constructor);
                TL.LogMessage("CreateAlpacaClient", "Added base constructor");

                // Add the class to the namespace
                ns.Types.Add(deviceClass);
                TL.LogMessage("CreateAlpacaClient", "Added class to name space");

                // Add the namespace to the program, which is now complete
                program.Namespaces.Add(ns);
                TL.LogMessage("CreateAlpacaClient", "Added name space to program");

                // Construct the path to the output DLL
                String dllName = $"{localServerPath.TrimEnd('\\')}\\{SharedConstants.DRIVER_PROGID_BASE}{DeviceNumber}.{DeviceType}.dll";
                TL.LogMessage("CreateAlpacaClient", string.Format("Output file name: {0}", dllName));

                // Create relevant compiler options to shape the compilation
                CompilerParameters cp = new CompilerParameters()
                {
                    GenerateExecutable = false,    // Specify output of a DLL
                    OutputAssembly = dllName,      // Specify the assembly file name to generate
                    GenerateInMemory = false,      // Save the assembly as a physical file.
                    TreatWarningsAsErrors = false, // Don't treat warnings as errors.
                    IncludeDebugInformation = true // Include debug information
                };
                TL.LogMessage("CreateAlpacaClient", "Created compiler parameters");

                // Add required assembly references to make sure the compilation succeeds
                cp.ReferencedAssemblies.Add(@"ASCOM.Attributes.dll");                    // Must be present in the current directory because the compiler doesn't use the GAC
                cp.ReferencedAssemblies.Add(@"ASCOM.DeviceInterfaces.dll");              // Must be present in the current directory because the compiler doesn't use the GAC
                cp.ReferencedAssemblies.Add(@"ASCOM.Newtonsoft.Json.dll");               // Must be present in the current directory because the compiler doesn't use the GAC
                cp.ReferencedAssemblies.Add(@"RestSharp.dll");                           // Must be present in the current directory
                cp.ReferencedAssemblies.Add(@"ASCOM.AlpacaClientDeviceBaseClasses.dll"); // Must be present in the current directory
                cp.ReferencedAssemblies.Add(SharedConstants.ALPACA_CLIENT_LOCAL_SERVER); // Must be present in the current directory

                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                cp.ReferencedAssemblies.Add(executingAssembly.Location);

                foreach (AssemblyName assemblyName in executingAssembly.GetReferencedAssemblies())
                {
                    cp.ReferencedAssemblies.Add(Assembly.Load(assemblyName).Location);
                }

                TL.LogMessage("CreateAlpacaClient", "Added assembly references");

                // Create formatting options for the generated code that will be logged into the trace logger
                CodeGeneratorOptions codeGeneratorOptions = new CodeGeneratorOptions()
                {
                    BracingStyle = "C",
                    IndentString = "    ",
                    VerbatimOrder = true,
                    BlankLinesBetweenMembers = false
                };

                // Get a code provider so that we can compile the program
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                TL.LogMessage("CreateAlpacaClient", "Created CSharp provider");

                // Write the generated code to the trace logger
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(outputStream))
                    {
                        provider.GenerateCodeFromNamespace(ns, writer, codeGeneratorOptions);
                    }

                    MemoryStream actualStream = new MemoryStream(outputStream.ToArray());
                    using (StreamReader reader = new StreamReader(actualStream))
                    {
                        do
                        {
                            TL.LogMessage("GeneratedCode", reader.ReadLine());
                        } while (!reader.EndOfStream);
                    }
                }
                provider.Dispose();

                // Compile the source contained in the "program" variable
                CompilerResults cr = provider.CompileAssemblyFromDom(cp, program);
                TL.LogMessage("CreateAlpacaClient", string.Format("Compiled assembly - {0} errors", cr.Errors.Count));

                // Report success or errors
                if (cr.Errors.Count > 0)
                {
                    // Display compilation errors.
                    foreach (CompilerError ce in cr.Errors)
                    {
                        TL.LogMessage("CreateAlpacaClient", string.Format("Compiler error: {0}", ce.ToString()));
                    }
                }
                else
                {
                    // Display a successful compilation message.
                    TL.LogMessage("CreateAlpacaClient", "Assembly compiled OK!");
                }

                TL.BlankLine();
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("CreateAlpacaClient", ex.ToString());
            }

        }

        /// <summary>
        /// Extension method to return a Pascal cased string
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        //public static string ToPascalCase(this string sourceString)
        //{
        //    // Check for empty string.  
        //    if (string.IsNullOrEmpty(sourceString))
        //    {
        //        return string.Empty;
        //    }

        //    // Check for a single character string
        //    if (sourceString.Length == 1)
        //    {
        //        return sourceString.ToUpperInvariant();
        //    }

        //    // Return the multi character string with first letter in upper case and the remaining characters in lower case.  
        //    return sourceString.Substring(0, 1).ToUpperInvariant() + sourceString.Substring(1).ToLowerInvariant();
        //}

        /// <summary>
        /// Run the local server to register and unregister DynamicRemoteClient clients
        /// </summary>
        /// <param name="localServerExe"></param>
        /// <param name="serverParameter"></param>
        internal static void RunLocalServer(string localServerExe, string serverParameter, TraceLogger TL)
        {
            bool exitOK;
            int exitCode = int.MinValue;

            // Set local server run time values
            ProcessStartInfo start = new ProcessStartInfo();
            start.Arguments = serverParameter; // Specify the server command parameter
            start.FileName = localServerExe; // Set the full local server executable path
            start.WindowStyle = ProcessWindowStyle.Hidden; // Don't show a window while the command runs
            start.CreateNoWindow = true;

            // Run the external process & wait for it to finish
            TL.LogMessage("RunLocalServer", $"Starting server with parameter {serverParameter}...");
            using (Process proc = Process.Start(start))
            {
                exitOK = proc.WaitForExit(LOCALSERVER_WAIT_TIME);
                if (exitOK) exitCode = proc.ExitCode; // Save the exit code
            }

            if (exitOK) TL.LogMessage("RunLocalServer", $"Local server exited OK with return code: {exitCode}");
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
            TraceLogger TL = new TraceLogger("DynamicClientThreadException")
            {
                Enabled = true
            };
            TL.LogMessage("Main", string.Format("ASCOM Dynamic Client Manager - Thread exception. Version: {0}", assemblyVersion.ToString()));
            TL.LogMessageCrLf("Main", e.Exception.ToString());

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
            TraceLogger TL = new TraceLogger("DynamicClientUnhandledException")
            {
                Enabled = true
            };
            TL.LogMessage("Main", string.Format("ASCOM Dynamic Client Manager - Unhandled exception. Version: {0}", assemblyVersion.ToString()));
            TL.LogMessageCrLf("Main", exception.ToString());

            // Display the exception in the default .txt editor and exit
            Process.Start(TL.LogFileName);

            TL.Enabled = false;
            TL.Dispose();

            Environment.Exit(0);
        }

        #endregion
    }
}
