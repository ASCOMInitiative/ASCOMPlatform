// This application enables the user to specify the number and type of remote client drivers that will be configured on their client machine,
// the user thus ends up with only devices that they actually want and need.
// The application uses dynamic compilation i.e. the drivers are compiled on the user's machine at run time rather than being pre-compiled at installer build time.
// Most of the heavy lifting is done through pre-compiled base classes that are called from the dynamically compiled top level shell classes.
// This enables the user to specify what are normally hard coded specifics such as the device type, GUID and device number.

// The application generates required code and stores this in memory. When the class is complete it is compiled and the resultant assembly persisted to disk
// into the same directory as the remote client local server, which is then called to register the driver assembly.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ASCOM.Utilities;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Reflection;

namespace ASCOM.DynamicRemoteClients
{
    public partial class Form1 : Form
    {
        // Constants only used within this form
        private const string DEVICE_NUMBER = "DeviceNumber"; // Regular expression device number placeholder name
        private const string DEVICE_TYPE = "DeviceType"; // Regular expression device type placeholder name
        private const string NUMERIC_UPDOWN_CONTROLNAME_PREXIX = "Num"; // Prefix to numeric up-down controls that enables them to be identified
        private const string BASE_CLASS_POSTFIX = "BaseClass"; // Postfix to the device type to create the base class name e.g. "CamerabaseClass". Must match the last characters of the device base class names!

        private const string REGEX_FORMAT_STRING = @"^ascom\.remote(?'" + DEVICE_NUMBER + @"'\d+)\.(?'" + DEVICE_TYPE + @"'[a-z]+)$"; // Regular expression for extracting device type and number
        private const int LOCALSERVER_WAIT_TIME = 5000; // Length of time (milliseconds) to wait for the local server to (un)register its drivers

        // Constants shared with the main program
        internal const string REMOTE_SERVER_PATH = @"\ASCOM\RemoteClients\"; // Relative path from CommonFiles\ASCOM
        internal const string REMOTE_SERVER = @"ASCOM.RemoteClientLocalServer.exe"; // Name of the remote client local server application
        internal const string REMOTE_CLIENT_DRIVER_NAME_TEMPLATE = @"ASCOM.Remote*.{0}.*"; // Template for finding remote client driver files

        // List of supported device types - this must be kept in sync with the device type numeric up-down controls on the form dialogue!
        private readonly List<string> supportedDeviceTypes = new List<string>() { "Camera", "Dome", "FilterWheel", "Focuser", "ObservingConditions", "Rotator", "SafetyMonitor", "Switch", "Telescope" };

        // Global variables within this class
        TraceLogger TL;
        Profile profile;
        List<DriverRegistration> remoteDrivers;
        Dictionary<string, int> deviceTypeSummary;

        /// <summary>
        /// Initialises the form
        /// </summary>
        public Form1(TraceLogger TLParameter)
        {
            try
            {
                InitializeComponent();

                TL = TLParameter; // Save the supplied trace logger

                Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
                LblVersionNumber.Text = "Version " + assemblyVersion.ToString();
                TL.LogMessage("Initialise", string.Format("Application Version: {0}", assemblyVersion.ToString()));

                profile = new Profile();
                remoteDrivers = new List<DriverRegistration>();
                deviceTypeSummary = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase); // Create a dictionary using a case insensitive key comparer

                ReadConfiguration(); // Get the current configuration

                // Add event handlers for the number of devices numeric up-down controls
                NumCamera.ValueChanged += NumValueChanged;
                NumDome.ValueChanged += NumValueChanged;
                NumFilterWheel.ValueChanged += NumValueChanged;
                NumFocuser.ValueChanged += NumValueChanged;
                NumObservingConditions.ValueChanged += NumValueChanged;
                NumRotator.ValueChanged += NumValueChanged;
                NumSafetyMonitor.ValueChanged += NumValueChanged;
                NumSwitch.ValueChanged += NumValueChanged;
                NumTelescope.ValueChanged += NumValueChanged;
                TL.LogMessage("Initialise", string.Format("Initialisation completed"));
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("initialise - Exception", ex.ToString());
                MessageBox.Show("Sorry, en error occurred on start up, please report this error message on the ASCOM Talk forum hosted at Groups.Io.\r\n\n" + ex.Message, "ASCOM Dynamic Clients", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DisableControls(false); // Disable all controls except exit
            }
        }

        /// <summary>
        /// Reads the current device configuration from the Profile and saves this for use elsewhere
        /// </summary>
        private void ReadConfiguration()
        {
            ArrayList deviceTypes = profile.RegisteredDeviceTypes;

            Regex regex = new Regex(REGEX_FORMAT_STRING, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            deviceTypeSummary.Clear();
            remoteDrivers.Clear();

            // List all the up-down controls present
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.GetType() == typeof(NumericUpDown))
                {
                    TL.LogMessage("ReadConfiguration", string.Format("Found NumericUpDown control {0}", ctrl.Name));
                    ctrl.BackColor = SystemColors.Window;
                }
            }

            // Extract a list of the remote client drivers from the list of devices in the Profile
            foreach (string deviceType in deviceTypes)
            {
                ArrayList devices = profile.RegisteredDevices(deviceType);
                foreach (KeyValuePair device in devices)
                {
                    Match match = regex.Match(device.Key);
                    if (match.Success)
                    {
                        DriverRegistration foundDriver = new DriverRegistration();
                        foundDriver.ProgId = match.Groups["0"].Value;
                        foundDriver.Number = int.Parse(match.Groups[DEVICE_NUMBER].Value, CultureInfo.InvariantCulture);
                        foundDriver.DeviceType = match.Groups[DEVICE_TYPE].Value;
                        remoteDrivers.Add(foundDriver);
                        TL.LogMessage("ReadConfiguration", string.Format("{0} - {1} - {2}", foundDriver.ProgId, foundDriver.Number, foundDriver.DeviceType));
                    }
                }

                TL.BlankLine();
            }

            // List the remote client drivers and create summary counts of client drivers of each device type 
            foreach (string deviceType in deviceTypes)
            {
                List<DriverRegistration> result = (from s in remoteDrivers where s.DeviceType.Equals(deviceType, StringComparison.InvariantCultureIgnoreCase) select s).ToList();
                foreach (DriverRegistration driver in result)
                {
                    TL.LogMessage("ReadConfiguration", string.Format("{0} driver: {1} - {2} - {3}", deviceType, driver.ProgId, driver.Number, driver.DeviceType));
                }

                deviceTypeSummary.Add(deviceType, result.Count);
            }

            // List the summary information
            foreach (string deviceType in deviceTypes)
            {
                TL.LogMessage("ReadConfiguration", string.Format("There are {0} {1} remote drivers", deviceTypeSummary[deviceType], deviceType));
            }

            // Set the numeric up-down controls to the current number of drivers of each type
            NumCamera.Value = deviceTypeSummary["Camera"];
            NumDome.Value = deviceTypeSummary["Dome"];
            NumFilterWheel.Value = deviceTypeSummary["FilterWheel"];
            NumFocuser.Value = deviceTypeSummary["Focuser"];
            NumObservingConditions.Value = deviceTypeSummary["ObservingConditions"];
            NumRotator.Value = deviceTypeSummary["Rotator"];
            NumSafetyMonitor.Value = deviceTypeSummary["SafetyMonitor"];
            NumSwitch.Value = deviceTypeSummary["Switch"];
            NumTelescope.Value = deviceTypeSummary["Telescope"];
        }

        /// <summary>
        /// Event handler for all numeric up-down controls - sets the background yellow if it is changes from the current value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numControl = (NumericUpDown)sender;
            string deviceName = numControl.Name.Substring(3);
            // MessageBox.Show(deviceName);
            if (numControl.Value != deviceTypeSummary[deviceName])
            {
                numControl.BackColor = Color.Yellow;
            }
            else
            {
                numControl.BackColor = SystemColors.Window;
            }
        }

        /// <summary>
        /// Exit button handler - just closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExit_Click(object sender, EventArgs e)
        {
            TL.LogMessage("Exit", "Closing the application");

            TL.Enabled = false;
            TL.Dispose();
            TL = null;

            Application.Exit();
        }

        /// <summary>
        /// Revise the number of remote clients and clean up the Profile if required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks> Approach to revising the number of remote clients:
        /// Prerequisites
        ///   Confirm that local server exists
        ///     If not then exit
        ///   Unregister drivers
        /// Remove driver executables numbered higher than required
        ///   For each driver number > n
        ///     Delete driver
        /// Remove Profile information for drivers numbered higher than required
        ///   For each driver number > n
        ///     Unregister driver
        /// Install required drivers
        ///   For each driver 1..n
        ///     Check that driver file exists
        ///       If not then create driver
        ///       Else no action
        /// Register drivers
        /// </remarks>
        private void BtnApply_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable controls so that the process can't be stopped part way through 
                DisableControls(true);

                string localServerExe = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + REMOTE_SERVER_PATH + REMOTE_SERVER;
                if (File.Exists(localServerExe)) // Local server does exist
                {
                    TL.LogMessage("Apply", string.Format("Found local server {0}", localServerExe));
                    ArrayList deviceTypes = profile.RegisteredDeviceTypes;

                    // Unregister existing drivers
                    RunLocalServer(localServerExe, "-unregserver", TL);

                    // Iterate over all of the installed device types
                    foreach (string deviceType in deviceTypes)
                    {
                        // Only attempt to process a device type if it is one that we support, otherwise ignore it
                        if (supportedDeviceTypes.Contains(deviceType, StringComparer.OrdinalIgnoreCase))
                        {
                            // This device type is recognised so process it
                            TL.LogMessage("Apply", string.Format("Processing device type: \"{0}\"", deviceType));
                            Control[] c = this.Controls.Find(NUMERIC_UPDOWN_CONTROLNAME_PREXIX + deviceType, true);
                            NumericUpDown numControl = (NumericUpDown)c[0];

                            // Delete files above the number required
                            string localServerPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + REMOTE_SERVER_PATH;
                            string searchPattern = string.Format(REMOTE_CLIENT_DRIVER_NAME_TEMPLATE, deviceType);

                            TL.LogMessage("Apply", string.Format("Searching for {0} driver files in {1} using pattern: {2}", deviceType, localServerPath, searchPattern));

                            List<string> files = Directory.GetFiles(localServerPath, searchPattern, SearchOption.TopDirectoryOnly).ToList();
                            TL.LogMessage("Apply", string.Format("Found {0} driver files", files.Count));

                            foreach (string file in files)
                            {
                                TL.LogMessage("Apply", string.Format("Found driver file {0}", file));
                                try
                                {
                                    File.Delete(file);
                                    TL.LogMessage("Apply", string.Format("Successfully deleted driver file {0}", file));
                                }
                                catch (Exception ex)
                                {
                                    string errorMessage = string.Format("Unable to delete driver file {0} - {1}", file, ex.Message);
                                    TL.LogMessage("Apply", errorMessage);
                                    MessageBox.Show(errorMessage);
                                }
                            }

                            // Unregister drivers
                            List<DriverRegistration> result = (from s in remoteDrivers where s.DeviceType.Equals(deviceType, StringComparison.InvariantCultureIgnoreCase) select s).ToList();
                            foreach (DriverRegistration driver in result)
                            {
                                if (driver.Number > numControl.Value)
                                {
                                    TL.LogMessage("Apply", string.Format("Removing driver Profile registration for {0} driver: {1} - {2} - {3}", deviceType, driver.ProgId, driver.Number, driver.DeviceType));
                                    profile.DeviceType = deviceType;
                                    profile.Unregister(driver.ProgId);
                                }
                                else
                                {
                                    TL.LogMessage("Apply", string.Format("Leaving driver Profile in place for {0} driver: {1} - {2} - {3}", deviceType, driver.ProgId, driver.Number, driver.DeviceType));
                                }
                            }

                            // Create required number of drivers
                            for (int i = 1; i <= numControl.Value; i++)
                            {
                                CreateDriver(deviceType, i, localServerPath, TL);
                            }
                        }
                        else
                        {
                            TL.LogMessage("Apply", string.Format("Ignoring unsupported device type: \"{0}\"", deviceType));
                            TL.BlankLine();
                        }
                    }

                    // Register the drivers
                    RunLocalServer(localServerExe, "-regserver", TL);
                    ReadConfiguration();
                }
                else // Local server can not be found
                {
                    string errorMessage = string.Format("Could not find local server {0}", localServerExe);
                    TL.LogMessage("Apply", errorMessage);
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("Apply - Exception", ex.ToString());
                MessageBox.Show("Sorry, en error occurred during Apply, please report this error message on the ASCOM Talk forum hosted at Groups.Io.\r\n\n" + ex.Message, "ASCOM Dynamic Clients", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls(); // Ensure that controls are re-enabled regardless of whatever happened
            }
        }

        /// <summary>
        /// Run the local server to register and unregister remote clients
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
            TL.LogMessage("RunLocalServer", string.Format("Starting server with parameter {0}...", serverParameter));
            using (Process proc = Process.Start(start))
            {
                exitOK = proc.WaitForExit(LOCALSERVER_WAIT_TIME);
                if (exitOK) exitCode = proc.ExitCode; // Save the exit code
            }

            if (exitOK) TL.LogMessage("RunLocalServer", string.Format("Local server exited OK with return code: {0}", exitCode));
            else
            {
                string errorMessage = string.Format("local server did not complete within {0} milliseconds, return code: {0}", LOCALSERVER_WAIT_TIME, exitCode);
                TL.LogMessage("RunLocalServer", errorMessage);
                MessageBox.Show(errorMessage);
            }
        }

        /// <summary>
        /// Create a compiled remote client driver assembly
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
        /// namespace ASCOM.Remote
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
        internal static void CreateDriver(string DeviceType, int DeviceNumber, string OutputDirectory, TraceLogger TL)
        {
            TL.LogMessage("CreateDriver", string.Format("Creating {0} {1} in {2}", DeviceType, DeviceNumber, OutputDirectory));
            try
            {
                // Generate the container unit
                CodeCompileUnit program = new CodeCompileUnit();

                // Generate the namespace
                CodeNamespace ns = new CodeNamespace("ASCOM.Remote");

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
                TL.LogMessage("CreateDriver", "Created base type");

                // Create custom attributes to decorate the class
                CodeAttributeDeclaration guidAttribute = new CodeAttributeDeclaration("Guid", new CodeAttributeArgument(new CodePrimitiveExpression(Guid.NewGuid().ToString())));
                CodeAttributeDeclaration progIdAttribute = new CodeAttributeDeclaration("ProgId", new CodeAttributeArgument(new CodeArgumentReferenceExpression("DRIVER_PROGID")));
                CodeAttributeDeclaration servedClassNameAttribute = new CodeAttributeDeclaration("ServedClassName", new CodeAttributeArgument(new CodeArgumentReferenceExpression("DRIVER_DISPLAY_NAME")));
                CodeAttributeDeclaration classInterfaceAttribute = new CodeAttributeDeclaration("ClassInterface", new CodeAttributeArgument(new CodeArgumentReferenceExpression("ClassInterfaceType.None")));
                CodeAttributeDeclarationCollection customAttributes = new CodeAttributeDeclarationCollection() { guidAttribute, progIdAttribute, servedClassNameAttribute, classInterfaceAttribute };
                TL.LogMessage("CreateDriver", "Created custom attributes");

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
                driverDisplayNameConst.InitExpression = new CodeArgumentReferenceExpression(@"SharedConstants.DRIVER_DISPLAY_NAME + "" "" + DRIVER_NUMBER");

                CodeMemberField driverProgIDConst = new CodeMemberField(typeof(string), "DRIVER_PROGID");
                driverProgIDConst.Attributes = MemberAttributes.Private | MemberAttributes.Const;
                driverProgIDConst.InitExpression = new CodeArgumentReferenceExpression(@"SharedConstants.DRIVER_PROGID_BASE + DRIVER_NUMBER + ""."" + DEVICE_TYPE");

                // Add the constants to the class
                deviceClass.Members.AddRange(new CodeMemberField[] { driverNumberConst, deviceTypeConst, driverDisplayNameConst, driverProgIDConst });
                TL.LogMessage("CreateDriver", "Added constants to class");

                // Declare the class constructor
                CodeConstructor constructor = new CodeConstructor();
                constructor.Attributes = MemberAttributes.Public | MemberAttributes.Final;

                // Add a call to the base class with required parameters
                constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("DRIVER_NUMBER"));
                constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("DRIVER_DISPLAY_NAME"));
                constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("DRIVER_PROGID"));
                deviceClass.Members.Add(constructor);
                TL.LogMessage("CreateDriver", "Added base constructor");

                // Add the class to the namespace
                ns.Types.Add(deviceClass);
                TL.LogMessage("CreateDriver", "Added class to name space");

                // Add the namespace to the program, which is now complete
                program.Namespaces.Add(ns);
                TL.LogMessage("CreateDriver", "Added name space to program");

                // Get a code provider so that we can compile the program
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                TL.LogMessage("CreateDriver", "Created CSharp provider");

                // Construct the path to the output DLL
                String dllName = String.Format(@"{0}\ASCOM.Remote{1}.{2}.dll", OutputDirectory.TrimEnd('\\'), DeviceNumber, DeviceType);
                TL.LogMessage("CreateDriver", string.Format("Output file name: {0}", dllName));

                // Create relevant compiler options to shape the compilation
                CompilerParameters cp = new CompilerParameters()
                {
                    GenerateExecutable = false,    // Specify output of a DLL
                    OutputAssembly = dllName,      // Specify the assembly file name to generate
                    GenerateInMemory = false,      // Save the assembly as a physical file.
                    TreatWarningsAsErrors = false, // Don't treat warnings as errors.
                    IncludeDebugInformation = true // Include debug information
                };
                TL.LogMessage("CreateDriver", "Created compiler parameters");

                // Copy required assemblies to the application's working directory
                Assembly a = Assembly.Load(@"ASCOM.Attributes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7, processorArchitecture=MSIL");
                TL.LogMessage("CreateDriver", string.Format("Copying ASCOM.Attributes assembly from {0} to {1}", a.Location, Application.StartupPath));
                File.Copy(a.Location, Application.StartupPath + "\\" + Path.GetFileName(a.Location), true);
                TL.LogMessage("CreateDriver", string.Format("Copied ASCOM.Attributes assembly OK"));

                a = Assembly.Load(@"ASCOM.DeviceInterfaces, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7, processorArchitecture=MSIL");
                TL.LogMessage("CreateDriver", string.Format("Copying ASCOM.DeviceInterfaces assembly from {0} to {1}", a.Location, Application.StartupPath));
                File.Copy(a.Location, Application.StartupPath + "\\" + Path.GetFileName(a.Location), true);
                TL.LogMessage("CreateDriver", string.Format("Copied ASCOM.DeviceInterfaces assembly OK"));

                // Add required assembly references to make sure the compilation succeeds
                cp.ReferencedAssemblies.Add(@"ASCOM.Attributes.dll");              // Has to be copied from the GAC to the local directory because the compiler doesn't use the GAC
                cp.ReferencedAssemblies.Add(@"ASCOM.DeviceInterfaces.dll");        // Has to be copied from the GAC to the local directory because the compiler doesn't use the GAC
                cp.ReferencedAssemblies.Add(@"RestSharp.dll");                     // Must be present in the current directory
                cp.ReferencedAssemblies.Add(@"Newtonsoft.Json.dll");               // Must be present in the current directory
                cp.ReferencedAssemblies.Add(@"ASCOM.RemoteClientBaseClasses.dll"); // Must be present in the current directory
                cp.ReferencedAssemblies.Add(@"ASCOM.RemoteClientLocalServer.exe"); // Must be present in the current directory

                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                cp.ReferencedAssemblies.Add(executingAssembly.Location);

                foreach (AssemblyName assemblyName in executingAssembly.GetReferencedAssemblies())
                {
                    cp.ReferencedAssemblies.Add(Assembly.Load(assemblyName).Location);
                }


                TL.LogMessage("CreateDriver", "Added assembly references");

                // Create formatting options for the generated code that will be logged into the trace logger
                CodeGeneratorOptions codeGeneratorOptions = new CodeGeneratorOptions()
                {
                    BracingStyle = "C",
                    IndentString = "    ",
                    VerbatimOrder = true,
                    BlankLinesBetweenMembers = false
                };

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

                // Compile the source contained in the "program" variable
                CompilerResults cr = provider.CompileAssemblyFromDom(cp, program);
                TL.LogMessage("CreateDriver", string.Format("Compiled assembly - {0} errors", cr.Errors.Count));

                // Report success or errors
                if (cr.Errors.Count > 0)
                {
                    // Display compilation errors.
                    foreach (CompilerError ce in cr.Errors)
                    {
                        TL.LogMessage("CreateDriver", string.Format("Compiler error: {0}", ce.ToString()));
                    }
                }
                else
                {
                    // Display a successful compilation message.
                    TL.LogMessage("CreateDriver", "Assembly compiled OK!");
                }

                TL.BlankLine();
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("CreateDriver", ex.ToString());
            }
        }

        /// <summary>
        /// Disables all UI controls - used when drivers are being created
        /// </summary>
        private void DisableControls(bool DisableExit)
        {
            if (DisableExit) BtnExit.Enabled = false;
            BtnApply.Enabled = false;
            NumCamera.Enabled = false;
            NumDome.Enabled = false;
            NumFilterWheel.Enabled = false;
            NumFocuser.Enabled = false;
            NumObservingConditions.Enabled = false;
            NumRotator.Enabled = false;
            NumSafetyMonitor.Enabled = false;
            NumSwitch.Enabled = false;
            NumTelescope.Enabled = false;
            NumVideo.Enabled = false;
        }

        /// <summary>
        /// Enables all UI controls - used after drivers have been created
        /// </summary>
        private void EnableControls()
        {
            BtnApply.Enabled = true;
            BtnExit.Enabled = true;
            NumCamera.Enabled = true;
            NumDome.Enabled = true;
            NumFilterWheel.Enabled = true;
            NumFocuser.Enabled = true;
            NumObservingConditions.Enabled = true;
            NumRotator.Enabled = true;
            NumSafetyMonitor.Enabled = true;
            NumSwitch.Enabled = true;
            NumTelescope.Enabled = true;
            NumVideo.Enabled = true;
        }
    }
}
