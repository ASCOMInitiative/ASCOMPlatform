// Uncomment to debug this code, otherwise leave false!
//#define DEBUG_TRACE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ASCOM.Astrometry;
using ASCOM.Astrometry.Exceptions;
using ASCOM.DeviceInterface;
using ASCOM.Internal;
using ASCOM.Utilities.Exceptions;
using ASCOM.Utilities.Video;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using PlatformUpdateChecker;
using Semver;

//using Semver;
using static ASCOM.Utilities.Global;
using static ASCOM.Utilities.RegistryAccess;
using static ASCOM.Utilities.Serial;

namespace ASCOM.Utilities
{
    /// <summary>
    /// ASCOM diagnostic tool
    /// </summary>
    public partial class DiagnosticsForm : Form
    {

        #region Constants and Enums

        // Controls to reduce the scope of tests to be run - only set to false to speed up testing during development. Must all be set True for production builds!
        private const bool TEST_ASTROMETRY = true;
        private const bool TEST_CACHE = true;
        private const bool TEST_LOGS_AND_APPLICATIONS = true;
        private const bool TEST_REGISTRY = true;
        private const bool TEST_SIMULATORS = true;
        private const bool TEST_UTILITIES = true;
        private const bool TEST_SCAN_DRIVES = true;
        private const bool CREATE_DEBUG_COLSOLE = false;

        // Current number of leap seconds - Used to test NOVAS 3.1 DeltaT calculation - Needs to be updated when the number of leap seconds changes
        private const double CURRENT_LEAP_SECONDS = 37.0d;

        private const string ASCOM_PLATFORM_NAME = "ASCOM Platform 7";
        private const string INST_DISPLAY_NAME = "DisplayName";
        private const string INST_DISPLAY_VERSION = "DisplayVersion";
        private const string INST_INSTALL_DATE = "InstallDate";
        private const string INST_INSTALL_LOCATION = "InstallLocation";
        private const string INST_INSTALL_SOURCE = "InstallSource";

        private const string DRIVER_CONNECT_APPLICATION_64BIT = @"\DriverConnect64\ASCOM.DriverConnect.exe";
        private const string DRIVER_CONNECT_APPLICATION_32BIT = @"\DriverConnect32\ASCOM.DriverConnect.exe";

        private const string COMPONENT_CATEGORIES = "Component Categories";
        private const string ASCOM_ROOT_KEY = " (ASCOM Root Key)";
        private const string TestTelescopeDescription = "This is a test telescope";
        private const string RevisedTestTelescopeDescription = "Updated description for test telescope!!!";
        private const string NewTestTelescopeDescription = "New description for test telescope!!!";
        private const double TOLERANCE_E2 = 0.01d; // Used in evaluating precision match of double values
        private const double TOLERANCE_E3 = 0.001d; // Used in evaluating precision match of double values
        private const double TOLERANCE_E4 = 0.0001d; // Used in evaluating precision match of double values
        private const double TOLERANCE_E5 = 0.00001d; // Used in evaluating precision match of double values
        private const double TOLERANCE_E6 = 0.000001d; // Used in evaluating precision match of double values
        private const double TOLERANCE_E7 = 0.0000001d; // Used in evaluating precision match of double values
        private const double TOLERANCE_E8 = 0.00000001d; // Used in evaluating precision match of double values
        private const double TOLERANCE_E9 = 0.000000001d;
        private const double TOLERANCE_100_MILLISECONDS = 0.000027777778d; // 1 arc second in hours also 1 second in degrees
        private const double TOLERANCE_1_SECOND = 0.00027777778d; // 1 arc second in hours also 1 second in degrees
        private const double TOLERANCE_5_SECONDS = 0.00138888888d; // 1 arc second in hours also 1 second in degrees

        private const double RADIANS_TO_HOURS = 12.0d / Math.PI;
        private const double RADIANS_TO_DEGREES = 180.0d / Math.PI;

        private const int DOME_SLEW_TIMEOUT = 240;
        private const string INST_UNINSTALL_STRING = "UninstallString";
        private const string INST_DISPLAY_ICON = "DisplayIcon";
        private const string INST_NOT_KNOWN = "Not known";

        private const string TEST_DATE = "Thursday, 30 December 2010 09:00:00"; // Arbitrary test date used to generate NOVASCOM test data, it must conform to the "F" date format for the invariant culture
        private const double J2000 = 2451545.0d; // Julian day for J2000 epoch
        private const int INDENT = 3; // Display indent for recursive loop output

        private const int CSIDL_PROGRAM_FILES = 38; // 0x0026
        private const int CSIDL_PROGRAM_FILESX86 = 42; // 0x002a,
        private const int CSIDL_WINDOWS = 36; // 0x0024,
        private const int CSIDL_PROGRAM_FILES_COMMONX86 = 44; // 0x002c,
        private const int CSIDL_SYSTEM = 37; // 0x0025,
        private const int CSIDL_SYSTEMX86 = 41; // 0x0029,

        private const string OPTIONS_REGISTRYKEY_BASE = @"Software\ASCOM\Diagnostics";
        private const string OPTIONS_AUTOVIEW_REGISTRYKEY = "Diagnostics Auto View Log"; private const bool OPTIONS_AUTOVIEW_REGISTRYKEY_DEFAULT = false;
        private const string OPTIONS_DIAGNOSTICS_TRACE = "Diagnostics Trace"; private const bool OPTIONS_DIAGNOSTICS_TRACE_DEFAULT = true;

        #region DLL Call Definitions
        [DllImport("kernel32.dll")]
        internal static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        internal static extern bool FreeConsole();
        #endregion

        private enum DoubleType
        {
            Number,
            Hours0To24,
            Hours0To24InRadians,
            HoursMinus12ToPlus12,
            HoursMinus12ToPlus12InRadians,
            Degrees0To360,
            Degrees0To360InRadians,
            DegreesMinus180ToPlus180,
            DegreesMinus180ToPlus180InRadians
        }

        private enum TransformExceptionTestType
        {
            SiteLatitude,
            SiteLongitude,
            SiteElevation,
            SiteTemperature,
            SitePressure,
            JulianDateUTC,
            JulianDateTT,
            SetJ2000RA,
            SetJ2000Dec,
            SetApparentRA,
            SetApparentDec,
            SetTopocentricRA,
            SetTopocentricDec,
            SetAzElAzimuth,
            SetAzElElevation
        }

        #endregion

        #region Variables

        private int NMatches, NNonMatches, NExceptions;
        private List<string> ErrorList = new();

        private TraceLogger TL; // Logger for Diagnostics reports
        private static TraceLogger tlInternal; // Logger for internal Diagnostic operation
        private RegistryAccess ASCOMRegistryAccess;
        private Timer _ASCOMTimer;

        private Timer ASCOMTimer
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ASCOMTimer;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ASCOMTimer != null)
                {
                    _ASCOMTimer.Tick -= Cnt_TickNet;
                }

                _ASCOMTimer = value;
                if (_ASCOMTimer != null)
                {
                    _ASCOMTimer.Tick += Cnt_TickNet;
                }
            }
        }
        private int RecursionLevel;
        private Stopwatch sw, s1, s2;
        private dynamic DrvHlpUtil;
        private Util AscomUtil;
        private dynamic g_Util2;
        private string DecimalSeparator = "";
        private string ThousandsSeparator = "";
        private string[] AbbreviatedMonthNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames; // List of month names in current culture language
        private DateTime StartTime;
        private int NumberOfTicks;

        private Astrometry.NOVAS.NOVAS3 Nov3;
        private Astrometry.NOVAS.NOVAS31 Nov31;
        private Astrometry.Transform.Transform transform;
        private Astrometry.AstroUtils.AstroUtils AstroUtil;
        private dynamic DeviceObject; // Device test object

        private string LastLogFile; // Name of last diagnostics log file

        private const int ArrayCopySize = 2;
        private int[] IntArray1D = new int[3];
        private int[,] IntArray2D = new int[3, 3];
        private int[,,] IntArray3D = new int[3, 3, 3];

        private Version DiagnosticsVersion; // Assembly version number of this executable

        private EarthRotationDataForm ERDForm; // Variable to hold the earth rotation data form handle so that we can ensure that a new instance of the form is always used

        private RegistryKey regKey; // Key to support reading the Windows 10 version and build information

        #endregion

        #region Initialisation, Form load and overall process

        /// <summary>
        /// Diagnostics form initiator
        /// </summary>
        public DiagnosticsForm()
        {
            InitializeComponent();
        }

        private void DiagnosticsForm_Load(object sender, EventArgs e)
        {
            // Initialise form
            SortedList<string, string> InstallInformation;

            try
            {
                tlInternal = new TraceLogger("DiagnosticsOperation");
                tlInternal.Enabled = Utilities.Global.GetBool(OPTIONS_DIAGNOSTICS_TRACE, OPTIONS_DIAGNOSTICS_TRACE_DEFAULT);

                DiagnosticsVersion = Assembly.GetExecutingAssembly().GetName().Version;
                InstallInformation = GetInstallInformation(Utilities.Global.PLATFORM_INSTALLER_PROPDUCT_CODE, false, true, false); // Retrieve the current install information
                lblTitle.Text = InstallInformation[INST_DISPLAY_NAME] + " - " + InstallInformation[INST_DISPLAY_VERSION];
                lblResult.Text = "";
                lblAction.Text = "";

                lblMessage.Text = "Your diagnostic log will be created in:" + "\r\n" + "\r\n" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ASCOM\Logs " + Strings.Format(DateTime.Now, "yyyy-MM-dd");

                btnViewLastLog.Enabled = false; // Disable last log button
                sw = new Stopwatch();

                if (Environment.Is64BitOperatingSystem) // We are on a 64bit OS so make both 32 and 64bit Chooser forms available
                {
                    ChooseAndConnectToDevice32bitApplicationToolStripMenuItem.Visible = true;
                    ChooseAndConnectToDevice64bitApplicationToolStripMenuItem.Visible = true;
                }
                else // We are on a 32bit OS so just make a 32bit Chooser form available
                {
                    ChooseAndConnectToDevice32bitApplicationToolStripMenuItem.Visible = true;
                    ChooseAndConnectToDevice64bitApplicationToolStripMenuItem.Visible = false;
                }

                RefreshTraceItems(); // Get current values for the trace menu settings
                MenuAutoViewLog.Checked = Utilities.Global.GetBool(OPTIONS_AUTOVIEW_REGISTRYKEY, OPTIONS_AUTOVIEW_REGISTRYKEY_DEFAULT); // Get the auto view log setting

                // Define the update checker task
                LogInternal("Load", "About to define update task");
                Task updateCheckTask = new Task(() => DiagnosticsUpdateCheck());

                // Run the update checker task without waiting for it to complete
                updateCheckTask.Start();
                LogInternal("Load", "Update task has started");

                BringToFront();
                KeyPreview = true; // Ensure that key press events are sent to the form so that the key press event handler can respond to them

                LogInternal("Load", "Complete");
                LogInternal(" ", " ");
            }
            catch (Exception ex)
            {
                Utilities.Global.LogEvent("Diagnostics Load", "Exception", EventLogEntryType.Error, EventLogErrors.DiagnosticsLoadException, ex.ToString());
                Interaction.MsgBox(ex.ToString());
            }
        }

        private void RunDiagnostics(object sender, EventArgs e)
        {
            string ASCOMPath;
            string ApplicationPath = "Path Not Set!";
            string SuccessMessage;

            try
            {
                Status("Diagnostics running...");

                TL = new TraceLogger("", "Diagnostics") { Enabled = true };

                transform = new Astrometry.Transform.Transform(); // Create a new Transform component for this run
                AstroUtil = new Astrometry.AstroUtils.AstroUtils(tlInternal);
                Nov3 = new Astrometry.NOVAS.NOVAS3();
                Nov31 = new Astrometry.NOVAS.NOVAS31();
                AscomUtil = new Util();

                btnExit.Enabled = false; // Disable buttons during run
                btnViewLastLog.Enabled = false;
                btnRunDiagnostics.Enabled = false;

                ErrorList.Clear(); // Remove any errors from previous runs
                NMatches = 0;
                NNonMatches = 0;
                NExceptions = 0;

                // Log Diagnostics version information
                TL.LogMessage("Diagnostics", "Version " + DiagnosticsVersion.ToString() + ", " + Application.ProductVersion);
                TL.BlankLine();
                TL.LogMessage("Date", DateTime.Now.ToString());
                TL.LogMessage("TimeZoneName", GetTimeZoneName());
                TL.LogMessage("TimeZoneOffset", TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours.ToString());
                TL.LogMessage("UTCDate", Conversions.ToString(DateTime.UtcNow));
                TL.LogMessage("Julian date", (DateTime.UtcNow.ToOADate() + Astrometry.GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString());
                TL.BlankLine();
                TL.LogMessage("CurrentCulture", CultureInfo.CurrentCulture.EnglishName + " " + CultureInfo.CurrentCulture.Name + " Decimal Separator \"" + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "\"" + " Number Group Separator \"" + CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + "\"");
                TL.LogMessage("CurrentUICulture", CultureInfo.CurrentUICulture.EnglishName + " " + CultureInfo.CurrentUICulture.Name + " Decimal Separator \"" + CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator + "\"" + " Number Group Separator \"" + CultureInfo.CurrentUICulture.NumberFormat.NumberGroupSeparator + "\"");

                try
                {
                    regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion"); // Open the OS version registry key
                    string productName = regKey.GetValue("ProductName", "").ToString();
                    string currentMajorVersionNumber = regKey.GetValue("CurrentMajorVersionNumber", "").ToString();
                    string currentMinorVersionNumber = regKey.GetValue("CurrentMinorVersionNumber", "").ToString();
                    string currentType = regKey.GetValue("CurrentType", "").ToString();
                    string currentBuildNumber = regKey.GetValue("currentBuildNumber", "").ToString();
                    string ubr = regKey.GetValue("UBR", "").ToString();

                    TL.BlankLine();
                    TL.LogMessage("OS Version", $"{productName} {currentType} {currentMajorVersionNumber}.{currentMinorVersionNumber}.{currentBuildNumber}.{ubr}");
                    TL.BlankLine();
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("OS Version", $"Exception reading OS version information: {ex}");
                }

                if (RunningInVM(true))
                {
                    TL.LogMessage("Environment", "Diagnostics is running in a virtual machine");
                }

                else
                {
                    TL.LogMessage("Environment", "Diagnostics is running on a real PC");
                }

                TL.BlankLine();

                LastLogFile = TL.LogFileName;
                try
                {
                    try
                    {
                        ApplicationPath = Assembly.GetEntryAssembly().Location;
                        ApplicationPath = ApplicationPath.Remove(ApplicationPath.LastIndexOf(@"\", StringComparison.OrdinalIgnoreCase));
                        Directory.SetCurrentDirectory(ApplicationPath);
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessage("Diagnostics", "ERROR - Unexpected exception setting current directory. You are likely to get four fails in ReadEph as a result.");
                        TL.LogMessage("Diagnostics", "Application Path: " + ApplicationPath);
                        LogException("Diagnostics", ex.ToString());
                    }

                    DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    ThousandsSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;

                    try // Try and create a registry access object
                    {
                        ASCOMRegistryAccess = new RegistryAccess();
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessage("Diagnostics", "ERROR - Unexpected exception creating New RegistryAccess object, later steps will show errors");
                        LogException("Diagnostics", ex.ToString());
                    }

                    try
                    {
                        ScanInstalledPlatform();
                    }
                    catch (Exception ex)
                    {
                        LogException("ScanInstalledPlatform", ex.ToString());
                    }
                    try
                    {
                        Utilities.Global.RunningVersions(TL); // Log diagnostic information
                    }
                    catch (Exception ex)
                    {
                        LogException("RunningVersions", ex.ToString());
                    }

                    if (TEST_SCAN_DRIVES)
                    {
                        try
                        {
                            ScanDrives(); // Scan PC drives and report information
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanDrives", ex.ToString());
                        }
                    }

                    try
                    {
                        ScanFrameworks(); // Report on installed .NET Framework versions
                    }
                    catch (Exception ex)
                    {
                        LogException("ScanFrameworks", ex.ToString());
                    }

                    try
                    {
                        ScanSerial(); // Report serial port information
                    }
                    catch (Exception ex)
                    {
                        LogException("ScanSerial", ex.ToString());
                    }

                    if (TEST_REGISTRY)
                    {
                        // Scan registry security rights
                        try
                        {
                            ScanRegistrySecurity();
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanRegistrySecurity", ex.ToString());
                        }
                        try
                        {
                            ScanASCOMDrivers();
                            Action(""); // Report installed driver versions
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanASCOMDrivers", ex.ToString());
                        }

                        try
                        {
                            ScanDriverExceptions();
                            Action(""); // Report drivers listed as exceptions
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanDriverExceptions", ex.ToString());
                        }

                        try
                        {
                            ScanProgramFiles(); // Search for copies of Helper and Helper2.DLL in the wrong places
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanProgramFiles", ex.ToString());
                        }

                        try
                        {
                            ScanProfile();
                            Action(""); // Report profile information
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanProfile", ex.ToString());
                        }

                        try
                        {
                            ScanRegistry(); // Scan Old ASCOM Registry Profile
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanInstalledPlatform", ex.ToString());
                        }

                        try
                        {
                            ScanProfile55Files();
                            Action(""); // List contents of Profile 5.5 XML files
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanProfile55Files", ex.ToString());
                        }

                        try
                        {
                            ScanCOMRegistration(); // Report COM Registration
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanCOMRegistration", ex.ToString());
                        }

                        try
                        {
                            ScanForHelperHijacking();
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanInstalledPlatform", ex.ToString());
                        }

                        // Scan files on 32 and 64bit systems
                        TL.LogMessage("Platform Files", "");
                        ASCOMPath = GetASCOMPath(); // Get relevant 32 or 64bit path to ACOM files
                        try
                        {
                            ScanPlatformFiles(ASCOMPath);
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanPlatformFiles", ex.ToString());
                        }

                        try
                        {
                            ScanDeveloperFiles();
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanDeveloperFiles", ex.ToString());
                        }

                        // List GAC contents
                        try
                        {
                            ScanGac();
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanGac", ex.ToString());
                        }
                    }

                    if (TEST_LOGS_AND_APPLICATIONS)
                    {
                        // List setup files
                        try
                        {
                            ScanLogs();
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanLogs", ex.ToString());
                        }

                        // List Platform 6 and 7 install logs
                        try
                        {
                            ScanPlatform6Logs();
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanPlatform6Logs", ex.ToString());
                        }

                        // Scan event log messages
                        try
                        {
                            ScanEventLog();
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanEventLog", ex.ToString());
                        }

                        // Scan for ASCOM Applications
                        try
                        {
                            ScanApplications();
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanApplications", ex.ToString());
                        }
                    }

                    TL.BlankLine();
                    TL.LogMessage("Diagnostics", "Completed diagnostic run, starting function testing run");
                    TL.BlankLine();
                    TL.BlankLine();

                    if (TEST_UTILITIES)
                    {
                        try
                        {
                            // Functional tests
                            UtilTests();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("UtilTests", ex.ToString());
                        }
                        try
                        {
                            ProfileTests();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("ProfileTests", ex.ToString());
                        }
                        try
                        {
                            TimerTests();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("TimerTests", ex.ToString());
                        }
                        try
                        {
                            VideoUtilsTests();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("VideoUtilsTests", ex.ToString());
                        }
                    }

                    if (TEST_CACHE)
                    {
                        try
                        {
                            CacheTests();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("CacheTests", ex.ToString());
                        }
                    }

                    if (TEST_ASTROMETRY)
                    {
                        try
                        {
                            NovasComTests();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("NovasComTests", ex.ToString());
                        }
                        try
                        {
                            KeplerTests();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("KeplerTests", ex.ToString());
                        }

                        try
                        {
                            TransformTest();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("TransformTest", ex.ToString());
                        }
                        try
                        {
                            NOVAS2Tests();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("NOVAS2Tests", ex.ToString());
                        }
                        try
                        {
                            NOVAS3Tests();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("NOVAS3Tests", ex.ToString());
                        }
                        try
                        {
                            NOVAS31Tests();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("NOVAS31Tests", ex.ToString());
                        }
                        try
                        {
                            AstroUtilsTests();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("AstroUtilsTests", ex.ToString());
                        }
                        try
                        {
                            SOFATests();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("SOFATests", ex.ToString());
                        }
                    }

                    if (TEST_SIMULATORS)
                    {
                        try
                        {
                            SimulatorTests();
                            Action("");
                        }
                        catch (Exception ex)
                        {
                            LogException("SimulatorTests", ex.ToString());
                        }
                    }

                    // Check that none of the debug assist flags are set!
                    CompareBoolean("Test Configuration", "Astrometry Tests Enabled", TEST_ASTROMETRY, true);
                    CompareBoolean("Test Configuration", "Cache Tests Enabled", TEST_CACHE, true);
                    CompareBoolean("Test Configuration", "Logs and Applications Tests Enabled", TEST_LOGS_AND_APPLICATIONS, true);
                    CompareBoolean("Test Configuration", "Registry Tests Enabled", TEST_REGISTRY, true);
                    CompareBoolean("Test Configuration", "Simulators Tests Enabled", TEST_SIMULATORS, true);
                    CompareBoolean("Test Configuration", "Utilities Tests Enabled", TEST_UTILITIES, true);
                    CompareBoolean("Test Configuration", "Utilities Tests Enabled", TEST_SCAN_DRIVES, true);
                    CompareBoolean("Test Configuration", "Create Debug Console Disabled", CREATE_DEBUG_COLSOLE, false);

                    // Check the Product Version string to make sure that it can be parsed by SemVer
                    try
                    {
                        string productVersionString = UpdateCheck.GetCurrentPlatformVersion();
                        bool parsedOk = SemVersion.TryParse(productVersionString, SemVersionStyles.Strict, out _);
                        if (parsedOk)
                        {
                            TL.LogMessage("SemVer", $"The product version was parsed OK by SemVer: {productVersionString}");
                            NMatches += 1;
                        }
                        else
                        {
                            string errMsg = $"##### The product version was NOT parsed OK by SemVer: {productVersionString}";
                            TL.LogMessageCrLf("SemVer", errMsg);
                            NNonMatches += 1;
                            ErrorList.Add($"SemVer - {errMsg}");
                        }
                    }
                    catch (Exception ex)
                    {
                        string errMsg = $"##### Exception parsing the product version: {ex.Message}";
                        TL.LogMessageCrLf("SemVer", $"{errMsg}\r\n{ex}");
                        NNonMatches += 1;
                        ErrorList.Add($"SemVer - {errMsg}");
                    }

                    if (NNonMatches == 0 & NExceptions == 0)
                    {
                        SuccessMessage = "Congratulations, all " + NMatches + " function tests passed!";
                    }
                    else
                    {
                        SuccessMessage = "Completed function testing run: " + NMatches + " matches, " + NNonMatches + " fail(s), " + NExceptions + " exception(s).";
                        TL.BlankLine();
                        TL.LogMessage("Error", "Error List");
                        foreach (string ErrorMessage in ErrorList)
                            TL.LogMessageCrLf("Error", ErrorMessage);
                        TL.BlankLine();
                    }

                    try
                    {
                        AstroUtil.Dispose();
                        TL.LogMessage("AstroUtilTests", "AstroUtils Disposed OK");
                    }
                    catch (Exception ex)
                    {
                        LogException("AstroUtilTests", "AstroUtils: " + ex.ToString());
                    }
                    AstroUtil = null;

                    try
                    {
                        transform.Dispose();
                        TL.LogMessage("Diagnostics", "Transform Disposed OK");
                    }
                    catch (Exception ex)
                    {
                        LogException("Diagnostics", "Transform: " + ex.ToString());
                    }
                    transform = null;

                    TL.BlankLine();
                    TL.LogMessage("Diagnostics", SuccessMessage);
                    TL.Enabled = false;
                    TL.Dispose();
                    TL = null;
                    Status("Diagnostic log created OK");
                    Action(SuccessMessage);
                }
                catch (Exception ex)
                {
                    Status("Diagnostics exception, please see log");
                    LogException("DiagException", ex.ToString());
                    TL.Enabled = false;
                    TL.Dispose();
                    Action("");
                    TL = null;
                }
                finally
                {
                    try
                    {
                        ASCOMRegistryAccess.Dispose();
                    }
                    catch
                    {
                    } // Clean up registry access object
                    ASCOMRegistryAccess = (RegistryAccess)null;
                }

                btnViewLastLog.Enabled = true; // Enable the view log control and set focus to it
                ActiveControl = btnViewLastLog;

                if (MenuAutoViewLog.Checked)
                    Process.Start(LastLogFile); // If auto log opening is enabled, open the last log in the system's default text editor
            }

            catch (Exception ex1)
            {
                lblResult.Text = "Can't create log: " + ex1.Message;
            }
            btnExit.Enabled = true; // Enable buttons during run
            btnRunDiagnostics.Enabled = true;
        }

        #endregion

        #region Tests

        private void SOFATests()
        {
            Astrometry.SOFA.SOFA SOFA;
            double t1 = default, t2 = default, date1, date2;
            int j;
            double rc, dc, pr, pd, px, rv, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, aob = default, zob = default, hob = default, dob = default, rob = default, eo = default;
            double ri = default, di = default, a = default, u1 = default, u2 = default, a1 = default, a2 = default, ob1, ob2;

            Status("Testing SOFA");
            TL.LogMessage("SOFATests", "Starting test");
            SOFA = new Astrometry.SOFA.SOFA();

            // SOFA version tests
            CompareInteger("SOFATests", "SOFA release number", SOFA.SofaReleaseNumber(), 19);
            Compare("SOFATests", "SOFA issue date", SOFA.SofaIssueDate(), "2023-10-11");
            Compare("SOFATests", "SOFA revision date", SOFA.SofaRevisionDate(), "2023-10-11");

            // Af2a tests
            j = SOFA.Af2a("-", 45, 13, 27.2d, ref a);

            CompareDouble("SOFATests", "Af2a", a, -0.78931157943136443d, 0.000000000001d);
            CompareInteger("SOFATests", "Af2a-status", j, 0);

            // Anp tests
            CompareDouble("SOFATests", "Anp", SOFA.Anp(-0.1d), 6.1831853071795866d, 0.000000000001d);

            // Atci13 tests
            rc = 2.71d;
            dc = 0.174d;
            pr = 0.00001d;
            pd = 0.000005d;
            px = 0.1d;
            rv = 55.0d;
            date1 = 2456165.5d;
            date2 = 0.401182685d;

            SOFA.CelestialToIntermediate(rc, dc, pr, pd, px, rv, date1, date2, ref ri, ref di, ref eo);

            CompareDouble("SOFATests", "CelestialToIntermediate-ri", ri, 2.7101215729690389d, TOLERANCE_100_MILLISECONDS, DoubleType.Hours0To24InRadians);
            CompareDouble("SOFATests", "CelestialToIntermediate-di", di, 0.17293713672182304d, TOLERANCE_100_MILLISECONDS, DoubleType.DegreesMinus180ToPlus180InRadians);
            CompareDouble("SOFATests", "CelestialToIntermediate-eo", eo, -0.0029006187126573756d, TOLERANCE_E8);

            // Atco13 tests
            rc = 2.71d;
            dc = 0.174d;
            pr = 0.00001d;
            pd = 0.000005d;
            px = 0.1d;
            rv = 55.0d;
            utc1 = 2456384.5d;
            utc2 = 0.969254051d;
            dut1 = 0.1550675d;
            elong = -0.527800806d;
            phi = -1.2345856d;
            hm = 2738.0d;
            xp = 0.000000247230737d;
            yp = 0.00000182640464d;
            phpa = 731.0d;
            tc = 12.8d;
            rh = 0.59d;
            wl = 0.55d;

            j = SOFA.CelestialToObserved(rc, dc, pr, pd, px, rv, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref aob, ref zob, ref hob, ref dob, ref rob, ref eo);

            CompareDouble("SOFATests", "CelestialToObserved-aob", aob, 0.0925177448538656d, TOLERANCE_100_MILLISECONDS, DoubleType.Degrees0To360InRadians);
            CompareDouble("SOFATests", "CelestialToObserved-zob", zob, 1.4076614052567671d, TOLERANCE_100_MILLISECONDS, DoubleType.DegreesMinus180ToPlus180InRadians);
            CompareDouble("SOFATests", "CelestialToObserved-hob", hob, -0.0926515443143121d, TOLERANCE_100_MILLISECONDS, DoubleType.HoursMinus12ToPlus12InRadians);
            CompareDouble("SOFATests", "CelestialToObserved-dob", dob, 0.17166265600755917d, TOLERANCE_100_MILLISECONDS, DoubleType.DegreesMinus180ToPlus180InRadians);
            CompareDouble("SOFATests", "CelestialToObserved-rob", rob, 2.7102604535030976d, TOLERANCE_100_MILLISECONDS, DoubleType.Hours0To24InRadians);
            CompareDouble("SOFATests", "CelestialToObserved-eo", eo, -0.0030205483548024128d, TOLERANCE_E8);
            CompareInteger("SOFATests", "CelestialToObserved-status", j, 0);

            // Dtf2d tests

            j = SOFA.Dtf2d("UTC", 1994, 6, 30, 23, 59, 60.13599d, ref u1, ref u2);

            CompareDouble("SOFATests", "Dtf2d", u1 + u2, 2449534.49999d, 0.000001d);
            CompareInteger("SOFATests", "Dtf2d-status", j, 0);

            // Eo06a tests
            eo = SOFA.Eo06a(2400000.5d, 53736.0d);

            CompareDouble("SOFATests", "Eo06a-eo", eo, -0.0013328823719418337d, 0.000000000000001d);

            // Atic13 tests
            ri = 2.7101215729690389d;
            di = 0.17293713672182304d;
            date1 = 2456165.5d;
            date2 = 0.401182685d;

            SOFA.IntermediateToCelestial(ri, di, date1, date2, ref rc, ref dc, ref eo);

            CompareDouble("SOFATests", "IntermediateToCelestial-rc", rc, 2.7101265045313747d, TOLERANCE_100_MILLISECONDS, DoubleType.Hours0To24InRadians);
            CompareDouble("SOFATests", "IntermediateToCelestial-dc", dc, 0.17406325376283424d, TOLERANCE_100_MILLISECONDS, DoubleType.DegreesMinus180ToPlus180InRadians);
            CompareDouble("SOFATests", "IntermediateToCelestial-eo", eo, -0.0029006187126573756d, 0.00000000000001d);

            // Atio13 tests
            ri = 2.7101215729690389d;
            di = 0.17293713672182304d;
            utc1 = 2456384.5d;
            utc2 = 0.969254051d;
            dut1 = 0.1550675d;
            elong = -0.527800806d;
            phi = -1.2345856d;
            hm = 2738.0d;
            xp = 0.000000247230737d;
            yp = 0.00000182640464d;
            phpa = 731.0d;
            tc = 12.8d;
            rh = 0.59d;
            wl = 0.55d;

            j = SOFA.IntermediateToObserved(ri, di, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref aob, ref zob, ref hob, ref dob, ref rob);

            CompareDouble("SOFATests", "IntermediateToObserved-aob", aob, 0.0923395222479499d, TOLERANCE_100_MILLISECONDS, DoubleType.Degrees0To360InRadians);
            CompareDouble("SOFATests", "IntermediateToObserved-zob", zob, 1.4077587045137225d, TOLERANCE_100_MILLISECONDS, DoubleType.DegreesMinus180ToPlus180InRadians);
            CompareDouble("SOFATests", "IntermediateToObserved-hob", hob, -0.092476198797820056d, TOLERANCE_100_MILLISECONDS, DoubleType.HoursMinus12ToPlus12InRadians);
            CompareDouble("SOFATests", "IntermediateToObserved-dob", dob, 0.17176534357582651d, TOLERANCE_100_MILLISECONDS, DoubleType.DegreesMinus180ToPlus180InRadians);
            CompareDouble("SOFATests", "IntermediateToObserved-rob", rob, 2.7100851079868864d, TOLERANCE_100_MILLISECONDS, DoubleType.Hours0To24InRadians);
            CompareInteger("SOFATests", "IntermediateToObserved-status", j, 0);

            // Atoc13 tests
            utc1 = 2456384.5d;
            utc2 = 0.969254051d;
            dut1 = 0.1550675d;
            elong = -0.527800806d;
            phi = -1.2345856d;
            hm = 2738.0d;
            xp = 0.000000247230737d;
            yp = 0.00000182640464d;
            phpa = 731.0d;
            tc = 12.8d;
            rh = 0.59d;
            wl = 0.55d;

            ob1 = 2.7100851079868864d;
            ob2 = 0.17176534357582651d;
            j = SOFA.ObservedToCelestial("R", ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref rc, ref dc);
            CompareDouble("SOFATests", "ObservedToCelestial-R-rc", rc, 2.7099567446610004d, TOLERANCE_100_MILLISECONDS, DoubleType.Hours0To24InRadians);
            CompareDouble("SOFATests", "ObservedToCelestial-R-dc", dc, 0.17416965008953986d, TOLERANCE_100_MILLISECONDS, DoubleType.DegreesMinus180ToPlus180InRadians);
            CompareInteger("SOFATests", "ObservedToCelestial-R-status", j, 0);

            ob1 = -0.092476198797820056d;
            ob2 = 0.17176534357582651d;
            j = SOFA.ObservedToCelestial("H", ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref rc, ref dc);
            CompareDouble("SOFATests", "ObservedToCelestial-H-rc", rc, 2.7099567446610004d, TOLERANCE_100_MILLISECONDS, DoubleType.Hours0To24InRadians);
            CompareDouble("SOFATests", "ObservedToCelestial-H-dc", dc, 0.17416965008953986d, TOLERANCE_100_MILLISECONDS, DoubleType.DegreesMinus180ToPlus180InRadians);
            CompareInteger("SOFATests", "ObservedToCelestial-H-status", j, 0);

            ob1 = 0.0923395222479499d;
            ob2 = 1.4077587045137225d;
            j = SOFA.ObservedToCelestial("A", ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref rc, ref dc);
            CompareDouble("SOFATests", "ObservedToCelestial-A-rc", rc, 2.7099567446610004d, TOLERANCE_100_MILLISECONDS, DoubleType.Hours0To24InRadians);
            CompareDouble("SOFATests", "ObservedToCelestial-A-dc", dc, 0.17416965008953986d, TOLERANCE_100_MILLISECONDS, DoubleType.DegreesMinus180ToPlus180InRadians);
            CompareInteger("SOFATests", "ObservedToCelestial-A-status", j, 0);

            // Atoi13 tests
            utc1 = 2456384.5d;
            utc2 = 0.969254051d;
            dut1 = 0.1550675d;
            elong = -0.527800806d;
            phi = -1.2345856d;
            hm = 2738.0d;
            xp = 0.000000247230737d;
            yp = 0.00000182640464d;
            phpa = 731.0d;
            tc = 12.8d;
            rh = 0.59d;
            wl = 0.55d;

            ob1 = 2.7100851079868864d;
            ob2 = 0.17176534357582651d;
            j = SOFA.ObservedToIntermediate("R", ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref ri, ref di);
            CompareDouble("SOFATests", "ObservedToIntermediate-ri", ri, 2.7101215744491358d, TOLERANCE_100_MILLISECONDS, DoubleType.Hours0To24InRadians);
            CompareDouble("SOFATests", "ObservedToIntermediate-di", di, 0.17293718391145677d, TOLERANCE_100_MILLISECONDS, DoubleType.DegreesMinus180ToPlus180InRadians);
            CompareInteger("SOFATests", "ObservedToIntermediate-status", j, 0);

            ob1 = -0.092476198797820056d;
            ob2 = 0.17176534357582651d;
            j = SOFA.ObservedToIntermediate("H", ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref ri, ref di);
            CompareDouble("SOFATests", "ObservedToIntermediate-ri", ri, 2.7101215744491358d, TOLERANCE_100_MILLISECONDS, DoubleType.Hours0To24InRadians);
            CompareDouble("SOFATests", "ObservedToIntermediate-di", di, 0.17293718391145677d, TOLERANCE_100_MILLISECONDS, DoubleType.DegreesMinus180ToPlus180InRadians);
            CompareInteger("SOFATests", "ObservedToIntermediate-status", j, 0);

            ob1 = 0.0923395222479499d;
            ob2 = 1.4077587045137225d;
            j = SOFA.ObservedToIntermediate("A", ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ref ri, ref di);
            CompareDouble("SOFATests", "ObservedToIntermediate-ri", ri, 2.7101215744491358d, TOLERANCE_100_MILLISECONDS, DoubleType.Hours0To24InRadians);
            CompareDouble("SOFATests", "ObservedToIntermediate-di", di, 0.17293718391145677d, TOLERANCE_100_MILLISECONDS, DoubleType.DegreesMinus180ToPlus180InRadians);
            CompareInteger("SOFATests", "ObservedToIntermediate-status", j, 0);

            // TaiTT tests
            j = SOFA.TaiTt(2453750.5d, 0.892482639d, ref t1, ref t2);
            CompareDouble("SOFATests", "TaiTT-t1", t1, 2453750.5d, 0.000001d);
            CompareDouble("SOFATests", "TaiTT-t2", t2, 0.892855139d, 0.000000000001d);
            CompareInteger("SOFATests", "TaiTT-status", j, 0);

            // TaiUtc tests
            j = SOFA.TaiUtc(2453750.5d, 0.892482639d, ref u1, ref u2);
            CompareDouble("SOFATests", "TaiUtc-u1", u1, 2453750.5d, 0.000001d);
            CompareDouble("SOFATests", "TaiUtc-u2", u2, 0.89210069455555552d, 0.000000000001d);
            CompareInteger("SOFATests", "TaiUtc-status", j, 0);

            // Tf2a tests
            j = SOFA.Tf2a("+", 4, 58, 20.2d, ref a);

            CompareDouble("SOFATests", "Tf2a", a, 1.3017392781895374d, 0.000000000001d);
            CompareInteger("SOFATests", "Tf2a-status", j, 0);

            // TTTai tests
            j = SOFA.TtTai(2453750.5d, 0.892482639d, ref a1, ref a2);
            CompareDouble("SOFATests", "TtTai-a1", a1, 2453750.5d, 0.000001d);
            CompareDouble("SOFATests", "TtTai-a2", a2, 0.892110139d, 0.000000000001d);
            CompareInteger("SOFATests", "TtTai-status", j, 0);

            // UtcTai tests
            j = SOFA.UtcTai(2453750.5d, 0.892100694d, ref u1, ref u2);

            CompareDouble("SOFATests", "UtcTai-u1", u1, 2453750.5d, 0.000001d);
            CompareDouble("SOFATests", "UtcTai-u2", u2, 0.89248263844444442d, 0.000000000001d);
            CompareInteger("SOFATests", "UtcTai-status", j, 0);

            Status("");
            Action("");

            TL.BlankLine();

        }

        private enum ApplicationList
        {
            ACPApplication,
            ACPFiles,
            Alcyone,
            CCDWare,
            DiffractionLtd,
            FocusMax,
            GeminiControlCenter,
            MaximDL,
            Pinpoint,
            StarryNight,
            SWBisque,
            TheSkyX
        }

        private void ScanApplications()
        {
            Status("Scanning Applications");
            TL.LogMessage("ScanApplications", "Starting scan");
            foreach (ApplicationList App in Enum.GetValues(typeof(ApplicationList)))
                ScanApplication(App);
            Status("");
            Action("");
        }

        private void ScanApplication(ApplicationList Application)
        {
            Action(Application.ToString());
            switch (Application)
            {
                case ApplicationList.ACPApplication:
                    {
                        GetApplicationViaAppid(Application, "acp.exe");
                        break;
                    }
                case ApplicationList.ACPFiles:
                    {
                        GetApplicationViaDirectory(Application, "ACP Obs Control");
                        break;
                    }
                case ApplicationList.Alcyone:
                    {
                        GetApplicationViaDirectory(Application, "Alcyone");
                        break;
                    }
                case ApplicationList.CCDWare:
                    {
                        GetApplicationViaDirectory(Application, "CCDWare");
                        break;
                    }
                case ApplicationList.DiffractionLtd:
                    {
                        GetApplicationViaDirectory(Application, "Diffraction Limited");
                        break;
                    }
                case ApplicationList.FocusMax:
                    {
                        GetApplicationViaDirectory(Application, "FocusMax");
                        break;
                    }
                case ApplicationList.GeminiControlCenter:
                    {
                        GetApplicationViaDirectory(Application, "Gemini Control Center");
                        break;
                    }
                case ApplicationList.MaximDL:
                    {
                        GetApplicationViaProgID(Application, "Maxim.Application");
                        break;
                    }
                case ApplicationList.Pinpoint:
                    {
                        GetApplicationViaDirectory(Application, "Pinpoint");
                        break;
                    }
                case ApplicationList.StarryNight:
                    {
                        GetApplicationViaSubDirectories(Application, "*Starry Night*");
                        break;
                    }
                case ApplicationList.TheSkyX:
                    {
                        GetApplicationViaProgID(Application, "TheSkyXAdaptor.TheSky");
                        break;
                    }
                case ApplicationList.SWBisque:
                    {
                        GetApplicationViaDirectory(Application, "Software Bisque");
                        break;
                    }

                default:
                    {
                        LogError("ScanApplication", "Unimplemented application test for: " + Application.ToString());
                        break;
                    }
            }
        }

        private void GetApplicationViaSubDirectories(ApplicationList Application, string AppDirectory)
        {
            var PathShell = new StringBuilder(260);
            List<string> Directories;
            if (Utilities.Global.ApplicationBits() == Bitness.Bits64)
            {
                // Find the program files (x86) path
                SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILESX86, false);
            }
            else // 32bits
            {
                SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILES, false);
            }
            try
            {
                Directories = [.. Directory.GetDirectories(PathShell.ToString(), AppDirectory, SearchOption.TopDirectoryOnly)];
                foreach (string Dir in Directories)
                    GetApplicationViaDirectory(Application, Path.GetFileName(Dir));
            }
            catch (DirectoryNotFoundException)
            {
                TL.LogMessage("ScanApplication", "Application " + Application.ToString() + " not installed in " + PathShell.ToString() + @"\" + AppDirectory);
            }
            catch (Exception ex)
            {
                LogError("GetApplicationViaSubDirectories", "Exception: " + ex.ToString());
            }
        }

        private void GetApplicationViaDirectory(ApplicationList Application, string AppDirectory)
        {
            var PathShell = new StringBuilder(260);
            string AppPath;
            List<string> Executables;
            if (Utilities.Global.ApplicationBits() == Bitness.Bits64)
            {
                // Find the program files (x86) path
                SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILESX86, false);
            }
            else // 32bits
            {
                SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILES, false);
            }
            AppPath = PathShell.ToString() + @"\" + AppDirectory;
            try
            {
                Executables = [.. Directory.GetFiles(AppPath, "*.exe", SearchOption.AllDirectories)];
                Executables.AddRange([.. Directory.GetFiles(AppPath, "*.dll", SearchOption.AllDirectories)]);
                if (Executables.Count == 0) // No executables found
                {
                    TL.LogMessage("ScanApplication", "Application " + Application.ToString() + " not found in " + AppPath);
                }
                else // Some executables were found
                {
                    TL.LogMessage("ScanApplication", "Found " + Application.ToString());

                    foreach (string Executable in Executables)
                        FileDetails(Path.GetDirectoryName(Executable) + @"\", Path.GetFileName(Executable));
                }
            }
            catch (DirectoryNotFoundException)
            {
                TL.LogMessage("ScanApplication", "Application " + Application.ToString() + " not installed in " + AppPath);
            }
            catch (Exception ex)
            {
                LogError("GetApplicationViaDirectory", "Exception: " + ex.ToString());
            }
        }

        private void GetApplicationViaProgID(ApplicationList Application, string ProgID)
        {
            RegistryAccess Reg;
            RegistryKey AppKey;
            string CLSIDString;
            string FileName;
            Reg = new RegistryAccess();

            try
            {
                AppKey = Reg.OpenSubKey3264(RegistryHive.ClassesRoot, ProgID + @"\CLSID", false, RegWow64Options.KEY_WOW64_32KEY);
                CLSIDString = Conversions.ToString(AppKey.GetValue("", ""));
                AppKey.Close();
                if (!string.IsNullOrEmpty(CLSIDString)) // Got a GUID value so try and process it
                {
                    AppKey = Reg.OpenSubKey3264(RegistryHive.ClassesRoot, @"CLSID\" + CLSIDString + @"\LocalServer32", false, RegWow64Options.KEY_WOW64_32KEY);
                    FileName = Conversions.ToString(AppKey.GetValue("", ""));
                    FileName = FileName.Trim(['"']); // TrimChars)
                    if (!string.IsNullOrEmpty(FileName)) // We have a file name so see if it exists
                    {

                        if (File.Exists(FileName)) // Get details
                        {
                            TL.LogMessage("ScanApplication", "Found " + Application.ToString());
                            FileDetails(Path.GetDirectoryName(FileName) + @"\", Path.GetFileName(FileName));
                        }
                        else
                        {
                            TL.LogMessage("ScanApplication", "Cannot find executable: " + FileName + " " + Application.ToString() + " not found");
                        }
                    }

                    else
                    {
                        TL.LogMessage("ScanApplication", "CLSID entry found but this has no file name value " + Application.ToString() + " not found");
                    }
                }
                else // No valid value so assume not installed
                {
                    TL.LogMessage("ScanApplication", "AppID entry found but this has no AppID value " + Application.ToString() + " not found");
                }
            }
            catch (ProfilePersistenceException) // Key does not exist
            {
                TL.LogMessage("ScanApplication", "Application " + Application.ToString() + " not found");
            }

        }

        private void GetApplicationViaAppid(ApplicationList Application, string Executable)
        {
            RegistryAccess Reg;
            RegistryKey AppKey;
            string CLSIDString;
            string FileName;
            Reg = new RegistryAccess();

            AppKey = Registry.ClassesRoot.OpenSubKey(@"AppId\" + Executable, false);
            if (AppKey is not null)
            {
                CLSIDString = Conversions.ToString(AppKey.GetValue("AppID", ""));
                AppKey.Close();
                if (!string.IsNullOrEmpty(CLSIDString)) // Got a GUID value so try and process it
                {
                    try
                    {
                        AppKey = Reg.OpenSubKey3264(RegistryHive.ClassesRoot, @"CLSID\" + CLSIDString + @"\LocalServer32", false, RegWow64Options.KEY_WOW64_32KEY);
                        FileName = Conversions.ToString(AppKey.GetValue("", ""));
                        if (!string.IsNullOrEmpty(FileName)) // We have a file name so see if it exists
                        {
                            if (File.Exists(FileName)) // Get details
                            {
                                TL.LogMessage("ScanApplication", "Found " + Application.ToString());
                                FileDetails(Path.GetDirectoryName(FileName) + @"\", Path.GetFileName(FileName));
                            }
                            else
                            {
                                TL.LogMessage("ScanApplication", "Cannot find executable: " + FileName + " " + Application.ToString() + " not found");
                            }
                        }

                        else
                        {
                            TL.LogMessage("ScanApplication", "CLSID entry found but this has no file name value " + Application.ToString() + " not found");
                        }
                    }
                    catch (ProfilePersistenceException) // Key does not exist
                    {
                        TL.LogMessage("ScanApplication", "Application " + Application.ToString() + " not found");
                    }
                }
                else // No valid value so assume not installed
                {
                    TL.LogMessage("ScanApplication", "AppID entry found but this has no AppID value " + Application.ToString() + " not found");
                }
            }
            else
            {
                TL.LogMessage("ScanApplication", "Application " + Application.ToString() + " not found");
            }
        }

        private string GetASCOMPath()
        {
            var PathShell = new StringBuilder(260);
            string ASCOMPath;

            if (IntPtr.Size == 8) // We are on a 64bit OS so look in the 32bit locations for files
            {
                SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILES_COMMONX86, false);
                ASCOMPath = PathShell.ToString() + @"\ASCOM\";
            }
            else // we are on a 32bit OS so look in the standard position
            {
                ASCOMPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + @"\ASCOM\";
            }
            return ASCOMPath;
        }

        private void ScanDriverExceptions()
        {
            this.ListDrivers(Utilities.Global.PLATFORM_VERSION_EXCEPTIONS, "ForcedPlatformVersion");
            this.ListDrivers(Utilities.Global.PLATFORM_VERSION_SEPARATOR_EXCEPTIONS, "ForcedPlatformSeparator");
            this.ListDrivers(Utilities.Global.DRIVERS_32BIT, "Non64BitDrivers");
            this.ListDrivers(Utilities.Global.DRIVERS_64BIT, "Non32BitDrivers");
            TL.BlankLine();
        }

        private void ListDrivers(string DriverCategory, string Description)
        {
            RegistryAccess Prof;
            SortedList<string, string> Contents;

            try
            {
                Prof = new RegistryAccess();
                Contents = Prof.EnumProfile(DriverCategory);
                foreach (KeyValuePair<string, string> ContentItem in Contents)
                    TL.LogMessage(Description, ContentItem.Key + " \"" + ContentItem.Value + "\"");
                TL.BlankLine();
            }
            catch (Exception)
            {

            }
        }

        private void SimulatorTests()
        {
            SimulatorDescriptor Sim;
            string DiagnosticsMajorMinorVersionNumber;
            string DiagnosticsMajorNumber;

            DiagnosticsMajorMinorVersionNumber = DiagnosticsVersion.Major.ToString() + "." + DiagnosticsVersion.Minor.ToString();
            DiagnosticsMajorNumber = DiagnosticsVersion.Major.ToString();

            if (true)
            {

                // Telescope Simulator - Platform 6
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "ASCOM.Simulator.Telescope",
                    Description = "Platform 6 Telescope Simulator",
                    DeviceType = "Telescope",
                    Name = "Simulator",
                    DriverVersion = "7.0",
                    InterfaceVersion = 3,
                    IsPlatform5 = false,
                    SixtyFourBit = true,
                    AxisRates = new double[,] { { 0.0d, 0.5d }, { 1.0d / 3.0d, 1.0d } }, // Axis rates relative to MaxRate
                    AxisRatesRelative = true
                };
                TestSimulator(Sim);

                // CoverCalibrator Simulator
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "ASCOM.Simulator.CoverCalibrator",
                    Description = "Platform 6 CoverCalibrator Simulator",
                    DeviceType = "CoverCalibrator",
                    Name = "CoverCalibrator Simulator",
                    DriverVersion = DiagnosticsMajorMinorVersionNumber,
                    InterfaceVersion = 1,
                    IsPlatform5 = false,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

                // Telescope Simulator - Platform 5
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "ScopeSim.Telescope",
                    Description = "Platform 5 Telescope Simulator",
                    DeviceType = "Telescope",
                    Name = "Simulator",
                    DriverVersion = "5.0",
                    InterfaceVersion = 2,
                    IsPlatform5 = true,
                    SixtyFourBit = true,
                    AxisRates = new double[,] { { 0.0d }, { 8.0d } }, // Absolute axis rates
                    AxisRatesRelative = false
                };
                TestSimulator(Sim);

                // Camera Simulator - Platform 5
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "CCDSimulator.Camera",
                    Description = "Platform 5 Camera Simulator",
                    DeviceType = "Camera",
                    Name = "ASCOM CCD camera simulator",
                    DriverVersion = "5.0",
                    InterfaceVersion = 2,
                    SixtyFourBit = false,
                    IsPlatform5 = true
                };
                TestSimulator(Sim);

                // Camera Simulator - Platform 6
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "ASCOM.Simulator.Camera",
                    Description = "Platform 6 Camera Simulator",
                    DeviceType = "Camera",
                    Name = "Sim ",
                    DriverVersion = DiagnosticsMajorMinorVersionNumber,
                    InterfaceVersion = 2,
                    IsPlatform5 = false,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

                Sim = new SimulatorDescriptor()
                {
                    ProgID = "ASCOM.Simulator.Dome",
                    Description = "Platform 6 Dome Simulator",
                    DeviceType = "Dome",
                    Name = "Simulator",
                    DriverVersion = DiagnosticsMajorMinorVersionNumber,
                    InterfaceVersion = 2,
                    IsPlatform5 = false,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

                // Dome Simulator - Platform 5
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "DomeSim.Dome",
                    Description = "Dome Simulator",
                    DeviceType = "Dome",
                    Name = "Simulator",
                    DriverVersion = "5.0",
                    InterfaceVersion = 1,
                    IsPlatform5 = true,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

                // FilterWheel Simulator - Platform 5
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "FilterWheelSim.FilterWheel",
                    Description = "Platform 5 FilterWheel Simulator",
                    DeviceType = "FilterWheel",
                    Name = "xxxx",
                    DriverVersion = "5.0",
                    InterfaceVersion = 1,
                    IsPlatform5 = true,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

                // FilterWheel Simulator - Platform 6
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "ASCOM.Simulator.FilterWheel",
                    Description = "Platform 6 FilterWheel Simulator",
                    DeviceType = "FilterWheel",
                    Name = "Filter Wheel Simulator .NET",
                    DriverVersion = DiagnosticsMajorNumber + ".0",
                    InterfaceVersion = 2,
                    IsPlatform5 = false,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

                // Focuser Simulator - Platform 5
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "FocusSim.Focuser",
                    Description = "Platform 5 Focuser Simulator",
                    DeviceType = "Focuser",
                    Name = "Simulator",
                    DriverVersion = "5.0",
                    InterfaceVersion = 1,
                    IsPlatform5 = true,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

                // Focuser Simulator - Platform 6
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "ASCOM.Simulator.Focuser",
                    Description = "Platform 6 Focuser Simulator",
                    DeviceType = "Focuser",
                    Name = "ASCOM.Simulator.Focuser",
                    DriverVersion = DiagnosticsMajorMinorVersionNumber,
                    InterfaceVersion = 3,
                    IsPlatform5 = false,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

                // SafetyMonitor Simulator
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "ASCOM.Simulator.SafetyMonitor",
                    Description = "Platform 6 Safety Monitor Simulator",
                    DeviceType = "SafetyMonitor",
                    Name = "ASCOM.Simulator.SafetyMonitor",
                    DriverVersion = DiagnosticsMajorNumber + ".0",
                    InterfaceVersion = 2,
                    IsPlatform5 = false,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

                // Switch Simulator - Platform 5
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "SwitchSim.Switch",
                    Description = "Platform 5 Switch Simulator",
                    DeviceType = "Switch",
                    Name = "Switch Simulator",
                    DriverVersion = "5.0",
                    InterfaceVersion = 1,
                    IsPlatform5 = true,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

                // Switch Simulator - Platform 6
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "ASCOM.Simulator.Switch",
                    Description = "Platform 6 Switch Simulator",
                    DeviceType = "Switch",
                    Name = "ASCOM Switch V2 Simulator",
                    DriverVersion = "7.0",
                    InterfaceVersion = 2,
                    IsPlatform5 = false,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

                // Video Simulator
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "ASCOM.Simulator.Video",
                    Description = "Platform 6 Video Simulator",
                    DeviceType = "Video",
                    Name = "Video Simulator",
                    DriverVersion = DiagnosticsMajorMinorVersionNumber,
                    InterfaceVersion = 1,
                    IsPlatform5 = false,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

                // ObservingConditions Simulator
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "ASCOM.Simulator.ObservingConditions",
                    Description = "Platform 6 ObservingConditions Simulator",
                    DeviceType = "ObservingConditions",
                    Name = "ASCOM Observing Conditions Simulator",
                    DriverVersion = DiagnosticsMajorMinorVersionNumber,
                    InterfaceVersion = 1,
                    IsPlatform5 = false,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

                // ObservingConditions Hub
                Sim = new SimulatorDescriptor()
                {
                    ProgID = "ASCOM.OCH.ObservingConditions",
                    Description = "Platform 6 ObservingConditions Hub",
                    DeviceType = "ObservingConditionsHub",
                    Name = "ASCOM Observing Conditions Hub (OCH)",
                    DriverVersion = DiagnosticsMajorMinorVersionNumber,
                    InterfaceVersion = 1,
                    IsPlatform5 = false,
                    SixtyFourBit = true
                };
                TestSimulator(Sim);

            }
            TL.BlankLine();
        }

        private void TestSimulator(SimulatorDescriptor Sim)
        {
            string RetValString;
            object DeviceAxisRates;
            int ct;
            Type DeviceType;
            double MaxSlewRate;
            string returnString;
            CoverStatus coverState;
            CalibratorStatus calibratorState;
            const string MAX_SLEW_RATE_PROFILE_NAME = "MaxSlewRate"; // Name of the Profile variable holding the maximum slew rate

            try
            {
                Status(Sim.Description);

                if (Utilities.Global.ApplicationBits() == Bitness.Bits64 & !Sim.SixtyFourBit) // We are on a 64 bit OS and are testing a 32bit only app - so skip the test!
                {
                    TL.LogMessage("TestSimulator", Sim.ProgID + " " + Sim.Description + " - Skipping test as this driver is not 64bit compatible");
                }
                else
                {
                    try
                    {
                        TL.LogMessage("TestSimulator", "CreateObject for Device: " + Sim.ProgID + " " + Sim.Description);
                        DeviceType = Type.GetTypeFromProgID(Sim.ProgID);
                        DeviceObject = Activator.CreateInstance(DeviceType);

                        switch (Sim.DeviceType ?? "")
                        {
                            case "Focuser":
                                {
                                    try
                                    {
                                        DeviceObject.Connected = true;
                                        Compare("TestSimulator", "Connected OK", "True", "True");
                                    }
                                    catch (RuntimeBinderException) // Could be a Platform 5 driver that uses "Link" instead of "Connected"
                                    {
                                        DeviceObject.Link = true; // Try Link, if it fails the outer try will catch the exception
                                        Compare("TestSimulator", "Linked OK", "True", "True");
                                    }

                                    // Disable temperature compensation if its available
                                    try
                                    {
                                        DeviceObject.TempComp = false;
                                        Compare("TestSimulator", "Temperature compensation disabled OK", "True", "True");
                                    }
                                    catch (Exception ex1)
                                    {
                                        LogException("TestSimulator", "Exception setting temperature compensation: " + ex1.ToString());
                                    }

                                    break;
                                }

                            case "ObservingConditionsHub":
                                {
                                    // The ObservingConditions Hub is un-configured on initial installation and so has a special test mode that fakes a valid configuration
                                    // This unpublicised Action initiates the test mode
                                    returnString = Conversions.ToString(DeviceObject.Action("SetTestMode", ""));
                                    TL.LogMessage("TestSimulator", "Observing conditions hub test mode request returned: " + returnString);
                                    DeviceObject.Connected = true;
                                    Compare("TestSimulator", "Connected OK", "True", "True"); // Everything else should be Connected 
                                    break;
                                }

                            default:
                                {
                                    DeviceObject.Connected = true;
                                    Compare("TestSimulator", "Connected OK", "True", "True");
                                    break;
                                }
                        }

                        Thread.Sleep(1000);

                        try
                        {
                            int interfaceVersion = DeviceObject.InterfaceVersion;
                            Compare("TestSimulator", $"Can read {Sim.DeviceType} interface version: {interfaceVersion}", Conversions.ToString(true), Conversions.ToString(true));
                        }
                        catch (COMException ex1)
                        {
                            if (ex1.ErrorCode == int.MinValue + 0x00040400 & Sim.DeviceType == "Telescope")
                            {
                                Compare("TestSimulator", "Simulator is in Interface V1 mode", "True", "True");
                            }
                        }
                        catch (NotSupportedException ex1)
                        {
                            if (Sim.IsPlatform5)
                            {
                                Compare("TestSimulator", "InterfaceVersion member is not present in Platform 5 Simulator", "True", "True");
                            }
                            else
                            {
                                LogException("TestSimulator", "InterfaceVersion Exception: " + ex1.ToString());
                            }
                        }
                        catch (RuntimeBinderException ex1)
                        {
                            if (Sim.IsPlatform5)
                            {
                                Compare("TestSimulator", "InterfaceVersion member is not present in Platform 5 Simulator", "True", "True");
                            }
                            else
                            {
                                LogException("TestSimulator", "InterfaceVersion Exception: " + ex1.ToString());
                            }
                        }
                        catch (Exception ex1)
                        {
                            LogException("TestSimulator", "InterfaceVersion Exception: " + ex1.ToString());
                        }

                        try
                        {
                            RetValString = DeviceObject.Description;
                            Compare("TestSimulator", "Description member is present in Platform 6 Simulator", "True", "True");
                            NMatches += 1;
                        }
                        catch (NotSupportedException ex1)
                        {
                            if (Sim.IsPlatform5)
                            {
                                Compare("TestSimulator", "Description member is not present in Platform 5 Simulator", "True", "True");
                            }
                            else
                            {
                                LogException("TestSimulator", "Description Exception: " + ex1.ToString());
                            }
                        }
                        catch (RuntimeBinderException ex1)
                        {
                            if (Sim.IsPlatform5)
                            {
                                Compare("TestSimulator", "Description member is not present in Platform 5 Simulator", "True", "True");
                            }
                            else
                            {
                                LogException("TestSimulator", "Description Exception: " + ex1.ToString());
                            }
                        }
                        catch (Exception ex1)
                        {
                            LogException("TestSimulator", "Description Exception: " + ex1.ToString());
                        }

                        try
                        {
                            RetValString = DeviceObject.DriverInfo;
                            Compare("TestSimulator", "DriverInfo member is present in Platform 6 Simulator", "True", "True");
                        }
                        catch (NotSupportedException ex1)
                        {
                            if (Sim.IsPlatform5)
                            {
                                Compare("TestSimulator", "DriverInfo member is not present in Platform 5 Simulator", "True", "True");
                            }
                            else
                            {
                                LogException("TestSimulator", "DriverInfo Exception: " + ex1.ToString());
                            }
                        }
                        catch (RuntimeBinderException ex1)
                        {
                            if (Sim.IsPlatform5)
                            {
                                Compare("TestSimulator", "DriverInfo member is not present in Platform 5 Simulator", "True", "True");
                            }
                            else
                            {
                                LogException("TestSimulator", "DriverInfo Exception: " + ex1.ToString());
                            }
                        }
                        catch (Exception ex1)
                        {
                            LogException("TestSimulator", "DriverInfo Exception: " + ex1.ToString());
                        }

                        try
                        {
                            RetValString = DeviceObject.Name;
                            Compare("TestSimulator", "Name member is present in Platform 6 Simulator", "True", "True");
                        }
                        catch (NotSupportedException ex1)
                        {
                            if (Sim.IsPlatform5)
                            {
                                Compare("TestSimulator", "Name member is not present in Platform 5 Simulator", "True", "True");
                            }
                            else
                            {
                                LogException("TestSimulator", "Name Exception: " + ex1.ToString());
                            }
                        }
                        catch (RuntimeBinderException ex1)
                        {
                            if (Sim.IsPlatform5)
                            {
                                Compare("TestSimulator", "Name member is not present in Platform 5 Simulator", "True", "True");
                            }
                            else
                            {
                                LogException("TestSimulator", "Name Exception: " + ex1.ToString());
                            }
                        }
                        catch (Exception ex1)
                        {
                            LogException("TestSimulator", "Name Exception: " + ex1.ToString());
                        }

                        try
                        {
                            // Compare("TestSimulator", Sim.DeviceType + " " + "DriverVersion", DeviceObject.DriverVersion, Sim.DriverVersion);
                        }
                        catch (COMException ex1)
                        {
                            if (ex1.ErrorCode == int.MinValue + 0x00040400 & Sim.DeviceType == "Telescope")
                            {
                                Compare("TestSimulator", "Simulator is in Interface V1 mode", "True", "True");
                            }
                        }
                        catch (NotSupportedException ex1)
                        {
                            if (Sim.IsPlatform5)
                            {
                                Compare("TestSimulator", "DriverVersion member is not present in Platform 5 Simulator", "True", "True");
                            }
                            else
                            {
                                LogException("TestSimulator", "DriverVersion Exception: " + ex1.ToString());
                            }
                        }
                        catch (RuntimeBinderException ex1)
                        {
                            if (Sim.IsPlatform5)
                            {
                                Compare("TestSimulator", "DriverVersion member is not present in Platform 5 Simulator", "True", "True");
                            }
                            else
                            {
                                LogException("TestSimulator", "DriverVersion Exception: " + ex1.ToString());
                            }
                        }
                        catch (Exception ex1)
                        {
                            LogException("TestSimulator", "DriverVersion Exception: " + ex1.ToString());
                        }

                        switch (Sim.DeviceType ?? "")
                        {
                            case "Telescope":
                                {
                                    DeviceTest("Telescope", "UnPark");
                                    DeviceTest("Telescope", "TrackingTrue");
                                    DeviceTest("Telescope", "SiderealTime");
                                    DeviceTest("Telescope", "TargetDeclination");
                                    DeviceTest("Telescope", "TargetRightAscension");
                                    DeviceTest("Telescope", "Slew");
                                    DeviceTest("Telescope", "TrackingRates");
                                    DeviceAxisRates = DeviceTest("Telescope", "AxisRates");
                                    try
                                    {
                                        // The maximum slew rate is a user configurable value so we need to read it here in order to conduct slew rate value tests
                                        // Get the maximum slew rate stored in the simulator Profile for use in relative rates tests
                                        using (Profile profileSlew = new())
                                        {
                                            // Handle the possibility that the Platform 6 simulator has never been started and so a max slew rate doesn't exist.
                                            string maxSlewRateString = profileSlew.GetValue(Sim.ProgID, MAX_SLEW_RATE_PROFILE_NAME); // Get the max slew rate string

                                            // Check whether the max slew rate has a value
                                            if (!string.IsNullOrEmpty(maxSlewRateString)) // There is a value
                                                MaxSlewRate = Conversions.ToDouble(maxSlewRateString);
                                            else // There is no value so set a low value that should be OK
                                                MaxSlewRate = 1.0;
                                        }

                                        ct = Conversions.ToInteger(DeviceObject.InterfaceVersion());
                                        ct = 0;
                                        foreach (dynamic AxRte in (IEnumerable)DeviceAxisRates)
                                        {
                                            // Get the minimum rate
                                            double minimum = AxRte.Minimum;

                                            // If we get here a maximum value could be read OK
                                            TL.LogMessage("TestSimulator", $"Got minimum rate OK: {AxRte.Minimum}");
                                            NMatches += 1;

                                            // Get the maximum rate
                                            double maximum = AxRte.Maximum;

                                            // If we get here a maximum value could be read OK
                                            TL.LogMessage("TestSimulator", $"Got maximum rate OK: {AxRte.Maximum}");
                                            NMatches += 1;
                                        }
                                    }
                                    catch (COMException ex1)
                                    {
                                        if (ex1.ErrorCode == int.MinValue + 0x00040400)
                                        {
                                            Compare("TestSimulator", "TrackingRates - Simulator is in Interface V1 mode", "True", "True");
                                        }
                                    }

                                    break;
                                }

                            case "Camera":
                                {
                                    DeviceTest("Camera", "StartExposure");
                                    break;
                                }
                            case "CoverCalibrator":
                                {
                                    coverState = (CoverStatus)Conversions.ToInteger(DeviceObject.CoverState); // Confirm that these  properties can be read and then they can be used to determine which tests to apply
                                    calibratorState = (CalibratorStatus)Conversions.ToInteger(DeviceObject.CalibratorState);

                                    // If we get here we have successfully read the two status properties
                                    NMatches += 2;
                                    TL.LogMessage("CoverCalibrator", $"CoverState: {coverState}, CalibratorState: {calibratorState}");

                                    if (calibratorState != CalibratorStatus.NotPresent) // The Calibrator capability is active so test these properties
                                    {
                                        DeviceTest("CoverCalibrator", "Brightness");
                                        DeviceTest("CoverCalibrator", "MaxBrightness");
                                    }

                                    break;
                                }
                            case "FilterWheel":
                                {
                                    DeviceTest("FilterWheel", "Position");
                                    break;
                                }
                            case "Focuser":
                                {
                                    DeviceTest("Focuser", "Move");
                                    break;
                                }
                            case "SafetyMonitor":
                                {
                                    DeviceTest("SafetyMonitor", "IsSafe");
                                    break;
                                }
                            case "Switch":
                                {
                                    if (Sim.IsPlatform5)
                                    {
                                        DeviceTest("Switch", "GetSwitch");
                                        DeviceTest("Switch", "GetSwitchName");
                                    }
                                    else // Is Platform v6.1
                                    {
                                        DeviceTest("Switch", "MaxSwitch");
                                        DeviceTest("Switch", "CanWrite");
                                        DeviceTest("Switch", "GetSwitch");
                                        DeviceTest("Switch", "GetSwitchDescription");
                                        DeviceTest("Switch", "GetSwitchName");
                                        DeviceTest("Switch", "GetSwitchValue");
                                        DeviceTest("Switch", "MaxSwitchValue");
                                        DeviceTest("Switch", "MinSwitchValue");
                                        DeviceTest("Switch", "SwitchStep");
                                    }

                                    break;
                                }
                            case "Dome":
                                {
                                    DeviceTest("Dome", "ShutterStatus");
                                    DeviceTest("Dome", "Slewing");
                                    DeviceTest("Dome", "OpenShutter");
                                    DeviceTest("Dome", "CloseShutter");
                                    DeviceTest("Dome", "SlewToAltitude");
                                    DeviceTest("Dome", "SlewToAzimuth");
                                    break;
                                }
                            case "Video":
                                {
                                    DeviceTest("Video", "BitDepth");
                                    DeviceTest("Video", "CanConfigureDeviceProperties");
                                    DeviceTest("Video", "ExposureMin");
                                    DeviceTest("Video", "Height");
                                    DeviceTest("Video", "Width");
                                    break;
                                }
                            case "ObservingConditions":
                            case "ObservingConditionsHub":
                                {
                                    DeviceTest("ObservingConditions", "AveragePeriod");
                                    DeviceTest("ObservingConditions", "TimeSinceLastUpdate");
                                    break;
                                }

                            default:
                                {
                                    LogException("TestSimulator", "Unknown device type: " + Sim.DeviceType);
                                    break;
                                }
                        }

                        // Disconnect the device
                        switch (Sim.DeviceType ?? "")
                        {
                            case "Focuser":
                                {
                                    try
                                    {
                                        DeviceObject.Connected = false;
                                        NMatches += 1;
                                    }
                                    catch (RuntimeBinderException) // Could be a Platform 5 driver that uses "Link" instead of "Connected"
                                    {
                                        TL.LogMessage("TestSimulator", "Focuser Connected member missing, using Link instead");
                                        DeviceObject.Link = false; // Try Link, if it fails the outer try will catch the exception
                                        NMatches += 1;
                                    } // Everything else should be Connected 

                                    break;
                                }

                            default:
                                {
                                    DeviceObject.Connected = false;
                                    NMatches += 1;
                                    break;
                                }
                        }
                        TL.LogMessage("TestSimulator", "Completed Device: " + Sim.ProgID + " OK");
                    }
                    catch (Exception ex)
                    {
                        LogException("TestSimulator", "Exception: " + ex.ToString());
                    }
                    finally
                    {
                        try
                        {
                            Marshal.ReleaseComObject(DeviceObject);
                        }
                        catch
                        {
                        } // Always try and make sure we are properly tidied up!
                        try
                        {
                            DeviceObject.Dispose();
                        }
                        catch
                        {
                        }
                        DeviceObject = null;
                    }
                }
                TL.BlankLine();
            }
            catch (Exception ex1)
            {
                LogException("TestSimulator", "Overall Exception: " + ex1.ToString());
            }
        }

        private object DeviceTest(string Device, string Test)
        {
            object RetVal = null;
            double SiderealTime, RetValDouble, TargetRA;
            DateTime StartTime;
            object DeviceTrackingRates;
            int FocuserMax, FocuserPosition;
            int FocuserUpperPortion, FocuserTargetPosition;
            bool canUnpark = default, canSetTracking = default, canReadSiderealTime = default, canSetTargetRightAscension = default, canSetTargetDeclination = default, canReadShutterStatus = default, slewing, canReadSlewing = default;
            ShutterState shutterStatus;

            const string PossibleDriveRates = "driveSidereal,driveKing,driveLunar,driveSolar";

            Action(Test);
            try
            {
                StartTime = DateTime.Now;
                switch (Device ?? "")
                {
                    case "CoverCalibrator":
                        {
                            switch (Test ?? "")
                            {
                                case "Brightness":
                                    {
                                        CompareBoolean("DeviceTest", Test, Conversions.ToBoolean(Operators.ConditionalCompareObjectGreaterEqual(DeviceObject.Brightness, 0, false)), true);
                                        break;
                                    }
                                case "MaxBrightness":
                                    {
                                        CompareBoolean("DeviceTest", Test, Conversions.ToBoolean(Operators.ConditionalCompareObjectGreaterEqual(DeviceObject.MaxBrightness, 1, false)), true);
                                        break;
                                    }

                                default:
                                    {
                                        LogException("DeviceTest", "Unknown Test: " + Test);
                                        break;
                                    }
                            }

                            break;
                        }

                    case "SafetyMonitor":
                        {
                            switch (Test ?? "")
                            {
                                case "IsSafe":
                                    {
                                        Compare("DeviceTest", Test, Conversions.ToString(DeviceObject.IsSafe), "False");
                                        break;
                                    }

                                default:
                                    {
                                        LogException("DeviceTest", "Unknown Test: " + Test);
                                        break;
                                    }
                            }

                            break;
                        }

                    case "Switch":
                        {
                            switch (Test ?? "")
                            {
                                case "MaxSwitch":
                                    {
                                        CompareBoolean("DeviceTest", Test, Conversions.ToBoolean(Operators.ConditionalCompareObjectGreater(DeviceObject.MaxSwitch, 0, false)), true);
                                        break;
                                    }
                                case "CanWrite":
                                    {
                                        Compare("DeviceTest", Test, Interaction.IIf(Conversions.ToBoolean(DeviceObject.CanWrite((object)0)), "OK", "OK").ToString(), "OK");
                                        break;
                                    }
                                case "GetSwitch":
                                    {
                                        Compare("DeviceTest", Test, Interaction.IIf(Conversions.ToBoolean(DeviceObject.GetSwitch((object)0)), "OK", "OK").ToString(), "OK");
                                        break;
                                    }
                                case "GetSwitchName":
                                    {
                                        CompareBoolean("DeviceTest", Test, string.IsNullOrEmpty(Conversions.ToString(DeviceObject.GetSwitchName((object)0))), false);
                                        break;
                                    }
                                case "GetSwitchDescription":
                                    {
                                        CompareBoolean("DeviceTest", Test, string.IsNullOrEmpty(Conversions.ToString(DeviceObject.GetSwitchDescription((object)0))), false);
                                        break;
                                    }
                                case "GetSwitchValue":
                                    {
                                        CompareBoolean("DeviceTest", Test, Information.IsNumeric(DeviceObject.GetSwitchValue((object)0)), true);
                                        break;
                                    }
                                case "MaxSwitchValue":
                                    {
                                        CompareBoolean("DeviceTest", Test, Information.IsNumeric(DeviceObject.MaxSwitchValue((object)0)), true);
                                        break;
                                    }
                                case "MinSwitchValue":
                                    {
                                        CompareBoolean("DeviceTest", Test, Information.IsNumeric(DeviceObject.MinSwitchValue((object)0)), true);
                                        break;
                                    }
                                case "SwitchStep":
                                    {
                                        CompareBoolean("DeviceTest", Test, Information.IsNumeric(DeviceObject.SwitchStep((object)0)), true);
                                        break;
                                    }

                                default:
                                    {
                                        LogException("DeviceTest", "Unknown Test: " + Test);
                                        break;
                                    }
                            }

                            break;
                        }

                    case "FilterWheel":
                        {
                            switch (Test ?? "")
                            {
                                case "Position":
                                    {
                                        int numberOfOffsets, testFilter;
                                        testFilter = 0; // Initialise test filter number

                                        // Determine a valid filter wheel position to run the test
                                        numberOfOffsets = ((Array)DeviceObject.FocusOffsets).Length;

                                        switch (numberOfOffsets)
                                        {
                                            case 0: // No filtgers so this is an error because we can't run the test.
                                                {
                                                    LogError("DeviceTest", "There are no filters defined, unable to test the FilterWheel position property.");
                                                    break;
                                                }
                                            case 1: // Only 1 so choose position 0 - the only option!
                                                {
                                                    testFilter = 0;
                                                    break;
                                                }
                                            case 2: // 2 filters so choose the lat one, position 1
                                                {
                                                    testFilter = 1; // More than 2 filters so go with one less than maximum (note filter position is 0 based!)
                                                    break;
                                                }

                                            default:
                                                {
                                                    testFilter = numberOfOffsets - 2;
                                                    break;
                                                }
                                        }
                                        TL.LogMessage("DeviceTest", $"Number of filter wheel filters: {numberOfOffsets}, Chosen wheel: {testFilter}");

                                        // Select the desired filter
                                        DeviceObject.Position = (object)testFilter;

                                        // Wait for the wheel to stop moving
                                        do
                                        {
                                            Thread.Sleep(100);
                                            Application.DoEvents();
                                            Action(Test + " " + DateTime.Now.Subtract(StartTime).Seconds);
                                        }
                                        while (!Operators.ConditionalCompareObjectGreater(DeviceObject.Position, -1, false));

                                        // Test the outcome.
                                        CompareDouble("DeviceTest", Test, Conversions.ToDouble(DeviceObject.Position), testFilter, 0.000001d);
                                        break;
                                    }

                                default:
                                    {
                                        LogException("DeviceTest", "Unknown Test: " + Test);
                                        break;
                                    }
                            }

                            break;
                        }

                    case "Focuser":
                        {
                            switch (Test ?? "")
                            {
                                case "Move":
                                    {
                                        // Find the larger of either 0 to Position or Position to MaxStep and then move to half of that
                                        // Calculate the upper portion size, the lower portion size is given by Position
                                        // 0.................................................Pos..........................Max
                                        // Lower Portion                               Upper Portion

                                        FocuserMax = Conversions.ToInteger(DeviceObject.MaxStep);
                                        FocuserPosition = Conversions.ToInteger(DeviceObject.Position);
                                        TL.LogMessage("DeviceTest", "Focuser Position: " + FocuserPosition + ", Focuser Maximum: " + FocuserMax);

                                        FocuserUpperPortion = FocuserMax - FocuserPosition;

                                        if (FocuserUpperPortion > FocuserPosition) // Upper portion is larger
                                        {
                                            FocuserTargetPosition = FocuserPosition + (int)Math.Round(FocuserUpperPortion / 2d);
                                            TL.LogMessage("DeviceTest", "Moving upward to: " + FocuserTargetPosition.ToString());
                                            DeviceObject.Move(FocuserTargetPosition);
                                        }
                                        else // Lower portion is larger
                                        {
                                            FocuserTargetPosition = (int)Math.Round(FocuserPosition / 2d);
                                            TL.LogMessage("DeviceTest", "Moving downward to: " + FocuserTargetPosition.ToString());
                                            DeviceObject.Move(FocuserTargetPosition);
                                        }

                                        do
                                        {
                                            Thread.Sleep(200);
                                            Application.DoEvents();
                                            Action(Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Test + " ", DeviceObject.Position), " / "), FocuserTargetPosition))); // Now.Subtract(StartTime).Seconds)
                                        }
                                        while (DeviceObject.IsMoving);
                                        CompareInteger("DeviceTest", Test, Conversions.ToInteger(DeviceObject.Position), FocuserTargetPosition);
                                        TL.LogMessage("DeviceTest", string.Format("Temperature compensation is available: {0} and enabled: {1}", DeviceObject.TempCompAvailable, DeviceObject.TempComp));
                                        break;
                                    }

                                default:
                                    {
                                        LogException("DeviceTest", "Unknown Test: " + Test);
                                        break;
                                    }
                            }

                            break;
                        }

                    case "Camera":
                        {
                            switch (Test ?? "")
                            {
                                case "StartExposure":
                                    {
                                        StartTime = DateTime.Now;
                                        DeviceObject.StartExposure(3.0d, true);
                                        TL.LogMessage(Device, "Start exposure duration: " + DateTime.Now.Subtract(StartTime).TotalSeconds);

                                        // Wait until exposure phase is complete and the simulator moves to the Downloading state
                                        do
                                        {
                                            Thread.Sleep(100);
                                            Application.DoEvents();
                                            Action(Test + " " + DateTime.Now.Subtract(StartTime).Seconds + " seconds");
                                        }
                                        while (!Operators.OrObject(Operators.ConditionalCompareObjectNotEqual(DeviceObject.CameraState, CameraStates.cameraExposing, false), DateTime.Now.Subtract(StartTime).TotalSeconds > 15.0d));
                                        CompareDouble(Device, "StartExposure", DateTime.Now.Subtract(StartTime).TotalSeconds, 3.0d, 0.2d);

                                        // Wait until the camera is idle before testing ImageReady
                                        do
                                        {
                                            Thread.Sleep(100);
                                            Application.DoEvents();
                                            Action(Test + " " + DateTime.Now.Subtract(StartTime).Seconds + " seconds");
                                        }
                                        while (!Operators.OrObject(Operators.ConditionalCompareObjectEqual(DeviceObject.CameraState, CameraStates.cameraIdle, false), DateTime.Now.Subtract(StartTime).TotalSeconds > 15.0d));
                                        Compare(Device, "ImageReady", Conversions.ToString(DeviceObject.ImageReady), Conversions.ToString(true));
                                        break;
                                    }

                                default:
                                    {
                                        LogException("DeviceTest", "Unknown Test: " + Test);
                                        break;
                                    }
                            }

                            break;
                        }

                    case "Telescope":
                        {
                            switch (Test ?? "")
                            {
                                case "UnPark":
                                    {
                                        canUnpark = Conversions.ToBoolean(DeviceObject.CanUnpark);
                                        Compare(Device, "CanUnPark - Simulator does return a value from CanUnpark.", "True", "True");
                                        if (canUnpark) // Test Unpark if it is supported
                                        {
                                            try
                                            {
                                                DeviceObject.UnPark();
                                                Compare(Device, Test, Conversions.ToString(DeviceObject.AtPark), "False");
                                            }
                                            catch (COMException ex1)
                                            {
                                                if (ex1.ErrorCode == int.MinValue + 0x00040400)
                                                {
                                                    Compare(Device, "UnPark - Simulator is in Interface V1 mode", "True", "True");
                                                }
                                            }
                                        }

                                        break;
                                    }
                                case "TrackingTrue":
                                    {
                                        if (canUnpark)
                                            DeviceObject.UnPark();
                                        canSetTracking = Conversions.ToBoolean(DeviceObject.CanSetTracking);
                                        Compare(Device, "CanSetTracking - Simulator does return a value from CanSetTracking.", "True", "True");
                                        if (canSetTracking)
                                        {
                                            DeviceObject.Tracking = (object)true;
                                            Compare(Device, Test, Conversions.ToString(DeviceObject.Tracking), "True");
                                        }
                                        else
                                        {
                                            TL.LogMessage(Device, "Tracking test skipped because CanSetTrackling is False");
                                        }

                                        break;
                                    }
                                case "SiderealTime":
                                    {
                                        try
                                        {
                                            SiderealTime = Conversions.ToDouble(DeviceObject.SiderealTime);
                                            canReadSiderealTime = true;
                                            Compare(Device, "SiderealTime - Simulator does return a value from SiderealTime.", "True", "True");

                                            TL.LogMessage(Device, "Received Sidereal time from telescope: " + SiderealTime);
                                            RetValDouble = Conversions.ToDouble(DeviceObject.SiderealTime);
                                            CompareDouble(Device, Test, RetValDouble, SiderealTime, TOLERANCE_5_SECONDS, DoubleType.Hours0To24);
                                        }

                                        catch (COMException ex) when (ex.ErrorCode == int.MinValue + 0x00040400)
                                        {
                                        }
                                        catch (PropertyNotImplementedException)
                                        {
                                            Compare(Device, "SiderealTime - Property is configured not to return a value.", "True", "True");
                                        }

                                        break;
                                    }
                                case "TargetDeclination":
                                    {
                                        try
                                        {
                                            DeviceObject.TargetDeclination = (object)0.0d;
                                            canSetTargetDeclination = true;
                                            RetValDouble = Conversions.ToDouble(DeviceObject.TargetDeclination);
                                            CompareDouble(Device, Test, RetValDouble, 0.0d, TOLERANCE_5_SECONDS, DoubleType.DegreesMinus180ToPlus180);
                                        }
                                        catch (COMException ex) when (ex.ErrorCode == int.MinValue + 0x00040400)
                                        {
                                        }
                                        catch (PropertyNotImplementedException)
                                        {
                                            Compare(Device, "TargetDeclination - Property is configured not to return a value.", "True", "True");
                                        }

                                        break;
                                    }
                                case "TargetRightAscension":
                                    {
                                        if (canReadSiderealTime)
                                        {
                                            SiderealTime = Conversions.ToDouble(DeviceObject.SiderealTime);
                                            TL.LogMessage(Device, "Received Sidereal time from telescope: " + AscomUtil.HoursToHMS(SiderealTime, ":", ":", "", 3));
                                            try
                                            {
                                                DeviceObject.TargetRightAscension = (object)SiderealTime;
                                                canSetTargetRightAscension = true;
                                                TL.LogMessage(Device, Conversions.ToString(Operators.ConcatenateObject("Target RA set to: ", DeviceObject.TargetRightAscension)));
                                                RetValDouble = Conversions.ToDouble(DeviceObject.TargetRightAscension);
                                                CompareDouble(Device, Test, RetValDouble, SiderealTime, TOLERANCE_5_SECONDS, DoubleType.Hours0To24);
                                            }
                                            catch (COMException ex) when (ex.ErrorCode == int.MinValue + 0x00040400)
                                            {
                                            }
                                            catch (PropertyNotImplementedException)
                                            {
                                                Compare(Device, "TargetRightAscension - Property is configured not to return a value.", "True", "True");
                                            }
                                        }
                                        else
                                        {
                                            TL.LogMessage(Device, "TargetRightAscension test skipped because can't read sidereal time");
                                        }

                                        break;
                                    }
                                case "Slew":
                                    {
                                        if (canUnpark & canSetTracking & canReadSiderealTime & canSetTargetRightAscension & canSetTargetDeclination)
                                        {
                                            DeviceObject.UnPark();
                                            DeviceObject.Tracking = (object)true;
                                            SiderealTime = Conversions.ToDouble(DeviceObject.SiderealTime);
                                            TL.LogMessage(Device, "Received Sidereal time from telescope: " + AscomUtil.HoursToHMS(SiderealTime, ":", ":", "", 3));
                                            TargetRA = AstroUtil.ConditionRA(SiderealTime - 1.0d); // Set the RA target to be 1 hour before zenith
                                            TL.LogMessage(Device, "Target RA calculated as: " + AscomUtil.HoursToHMS(TargetRA, ":", ":", "", 3));
                                            DeviceObject.TargetRightAscension = (object)TargetRA;
                                            TL.LogMessage(Device, "Target RA set to: " + AscomUtil.HoursToHMS(Conversions.ToDouble(DeviceObject.TargetRightAscension), ":", ":", "", 3));
                                            DeviceObject.TargetDeclination = (object)0.0d;
                                            TL.LogMessage(Device, "Target Dec set to: " + AscomUtil.DegreesToDMS(Conversions.ToDouble(DeviceObject.TargetDeclination), ":", ":", "", 3));
                                            TL.LogMessage(Device, "Pre-slew RA is: " + AscomUtil.HoursToHMS(Conversions.ToDouble(DeviceObject.RightAscension), ":", ":", "", 3));
                                            TL.LogMessage(Device, "Pre-slew Dec is: " + AscomUtil.DegreesToDMS(Conversions.ToDouble(DeviceObject.Declination), ":", ":", "", 3));
                                            TL.LogMessage(Device, string.Format("Pre-slew Az/Alt is: {0} {1}", AscomUtil.DegreesToDMS(Conversions.ToDouble(DeviceObject.Azimuth), ":", ":", "", 3), AscomUtil.DegreesToDMS(Conversions.ToDouble(DeviceObject.Altitude), ":", ":", "", 3)));
                                            DeviceObject.SlewToTarget();
                                            Thread.Sleep(1000); // Wait a short while to ensure the simulator has stabilised
                                            TL.LogMessage(Device, "Post-slew RA is: " + AscomUtil.HoursToHMS(Conversions.ToDouble(DeviceObject.RightAscension), ":", ":", "", 3));
                                            TL.LogMessage(Device, "Post-slew Dec is: " + AscomUtil.DegreesToDMS(Conversions.ToDouble(DeviceObject.Declination), ":", ":", "", 3));
                                            TL.LogMessage(Device, string.Format("Post-slew Az/Alt is: {0} {1}", AscomUtil.DegreesToDMS(Conversions.ToDouble(DeviceObject.Azimuth), ":", ":", "", 3), AscomUtil.DegreesToDMS(Conversions.ToDouble(DeviceObject.Altitude), ":", ":", "", 3)));
                                            CompareDouble(Device, Test + " RA", Conversions.ToDouble(DeviceObject.RightAscension), TargetRA, TOLERANCE_5_SECONDS, DoubleType.Hours0To24);
                                            CompareDouble(Device, Test + " Dec", Conversions.ToDouble(DeviceObject.Declination), 0.0d, TOLERANCE_5_SECONDS, DoubleType.DegreesMinus180ToPlus180);
                                        }
                                        else
                                        {
                                            TL.LogMessage(Device, $"Slew test skipped because CanUnpark: {canUnpark}, CanSetTracking: {canSetTracking}, CanReadSidferalTime: {canReadSiderealTime}, CanSetTargetRightAscension: {canSetTargetRightAscension}, CanSetTargetDeclination: {canSetTargetDeclination}");
                                        }

                                        break;
                                    }
                                case "TrackingRates":
                                    {
                                        try
                                        {
                                            DeviceTrackingRates = DeviceObject.TrackingRates;
                                            foreach (DriveRates TrackingRate in (IEnumerable)DeviceTrackingRates)
                                            {
                                                if (PossibleDriveRates.Contains(TrackingRate.ToString()))
                                                {
                                                    NMatches += 1;
                                                    TL.LogMessage(Device, "Matched Tracking Rate = " + TrackingRate.ToString());
                                                }
                                                else
                                                {
                                                    LogException(Device, "Found unexpected tracking rate: \"" + TrackingRate.ToString() + "\"");
                                                }
                                            }
                                        }
                                        catch (COMException ex1)
                                        {
                                            if (ex1.ErrorCode == int.MinValue + 0x00040400)
                                            {
                                                Compare(Device, "TrackingRates - Simulator is in Interface V1 mode", "True", "True");
                                            }
                                        }

                                        break;
                                    }
                                case "AxisRates":
                                    {
                                        try
                                        {
                                            RetVal = DeviceObject.AxisRates(TelescopeAxes.axisPrimary);
                                            Compare(Device, "AxisRates returned OK", "True", "True");
                                        }
                                        catch (COMException ex1)
                                        {
                                            if (ex1.ErrorCode == int.MinValue + 0x00040400)
                                            {
                                                Compare(Device, "AxisRates - Simulator is in Interface V1 mode", "True", "True");
                                            }
                                        }

                                        break;
                                    }

                                default:
                                    {
                                        LogException("DeviceTest", "Unknown Test: " + Test);
                                        break;
                                    }
                            }

                            break;
                        }

                    case "Dome":
                        {
                            switch (Test ?? "")
                            {
                                case "ShutterStatus":
                                    {
                                        try
                                        {
                                            shutterStatus = (ShutterState)DeviceObject.ShutterStatus;
                                            canReadShutterStatus = true;
                                            Compare(Device, "ShutterStatus - Simulator can read the shutter status", "True", "True");
                                        }
                                        catch (RuntimeBinderException)
                                        {
                                            Compare(Device, "ShutterStatus - Simulator ShutterStatus property is not accessible", "True", "True");
                                        }
                                        catch (COMException ex) when (ex.ErrorCode == int.MinValue + 0x00040400)
                                        {
                                            Compare(Device, "ShutterStatus - Simulator ShutterStatus property is not accessible", "True", "True");
                                        }
                                        catch (MethodNotImplementedException)
                                        {
                                            Compare(Device, "ShutterStatus - Simulator ShutterStatus property is not accessible", "True", "True");
                                        }
                                        break;
                                    }
                                case "Slewing":
                                    {
                                        try
                                        {
                                            slewing = DeviceObject.Slewing;
                                            canReadSlewing = true;
                                            Compare(Device, "Slewing - Simulator can read the Slewing status", "True", "True");
                                        }
                                        catch (COMException ex) when (ex.ErrorCode == int.MinValue + 0x00040400)
                                        {
                                        }
                                        catch (MethodNotImplementedException)
                                        {
                                            Compare(Device, "Slewing - Simulator Slewing property is not accessible", "True", "True");
                                        }

                                        break;
                                    }
                                case "OpenShutter":
                                    {
                                        if (canReadShutterStatus)
                                        {
                                            try
                                            {
                                                StartTime = DateTime.Now;
                                                DeviceObject.OpenShutter();
                                                Compare(Device, "OpenShutter - Simulator can open the shutter", "True", "True");
                                                while (Operators.AndObject(!Operators.ConditionalCompareObjectEqual(DeviceObject.ShutterStatus, ShutterState.shutterOpen, false), DateTime.Now.Subtract(StartTime).TotalSeconds < DOME_SLEW_TIMEOUT))
                                                {
                                                    Thread.Sleep(100);
                                                    Action(Test + " " + DateTime.Now.Subtract(StartTime).Seconds + " seconds / " + DOME_SLEW_TIMEOUT);
                                                    Application.DoEvents();
                                                }
                                                Compare(Device, Test + " Timeout", Conversions.ToString(DateTime.Now.Subtract(StartTime).TotalSeconds >= DOME_SLEW_TIMEOUT), "False");
                                                Compare(Device, Test, Conversions.ToInteger(DeviceObject.ShutterStatus).ToString(), ((int)ShutterState.shutterOpen).ToString());
                                            }
                                            catch (COMException ex) when (ex.ErrorCode == int.MinValue + 0x00040400)
                                            {
                                            }
                                            catch (MethodNotImplementedException)
                                            {
                                                Compare(Device, "OpenShutter - Simulator open shutter is disabled", "True", "True");
                                            }
                                        }
                                        else
                                        {
                                            Compare(Device, "OpenShutter - Skipping test because simulator ShutterStatus property is not accessible", "True", "True");
                                        }

                                        break;
                                    }
                                case "CloseShutter":
                                    {
                                        if (canReadShutterStatus)
                                        {
                                            try
                                            {
                                                StartTime = DateTime.Now;
                                                DeviceObject.CloseShutter();
                                                Compare(Device, "OpenShutter - Simulator can close the shutter", "True", "True");
                                                while (Operators.AndObject(!Operators.ConditionalCompareObjectEqual(DeviceObject.ShutterStatus, ShutterState.shutterClosed, false), DateTime.Now.Subtract(StartTime).TotalSeconds < DOME_SLEW_TIMEOUT))
                                                {
                                                    Thread.Sleep(100);
                                                    Action(Test + " " + DateTime.Now.Subtract(StartTime).Seconds + " seconds / " + DOME_SLEW_TIMEOUT);
                                                    Application.DoEvents();
                                                }
                                                Compare(Device, Test + " Timeout", Conversions.ToString(DateTime.Now.Subtract(StartTime).TotalSeconds >= DOME_SLEW_TIMEOUT), "False");
                                                Compare(Device, Test, Conversions.ToInteger(DeviceObject.ShutterStatus).ToString(), ((int)ShutterState.shutterClosed).ToString());
                                            }
                                            catch (COMException ex) when (ex.ErrorCode == int.MinValue + 0x00040400)
                                            {
                                            }
                                            catch (MethodNotImplementedException)
                                            {
                                                Compare(Device, "CloseShutter - Simulator close shutter is disabled", "True", "True");
                                            }
                                        }
                                        else
                                        {
                                            Compare(Device, "CloseShutter - Skipping test because simulator ShutterStatus property is not accessible", "True", "True");
                                        }

                                        break;
                                    }
                                case "SlewToAltitude":
                                    {
                                        if (canReadSlewing)
                                        {
                                            try
                                            {
                                                StartTime = DateTime.Now;
                                                DeviceObject.SlewToAltitude((object)45.0d);
                                                do
                                                {
                                                    Thread.Sleep(100);
                                                    Application.DoEvents();
                                                    Action(Test + " " + DateTime.Now.Subtract(StartTime).Seconds + " seconds / " + DOME_SLEW_TIMEOUT);
                                                }
                                                while (!Operators.OrObject(Operators.ConditionalCompareObjectEqual(DeviceObject.Slewing, false, false), DateTime.Now.Subtract(StartTime).TotalSeconds > DOME_SLEW_TIMEOUT));
                                                this.Compare(Device, Test + " Not Complete", DeviceObject.Slewing.ToString(), "False");
                                                CompareDouble(Device, Test, Conversions.ToDouble(DeviceObject.Altitude), 45.0d, TOLERANCE_5_SECONDS, DoubleType.DegreesMinus180ToPlus180);
                                            }
                                            catch (COMException ex) when (ex.ErrorCode == int.MinValue + 0x00040400)
                                            {
                                            }
                                            catch (MethodNotImplementedException)
                                            {
                                                Compare(Device, "SlewToAltitude - Simulator SlewToAltitude method is disabled", "True", "True");
                                            }
                                        }
                                        else
                                        {
                                            Compare(Device, "SlewToAltitude - Skipping test because simulator Slewing property is not accessible", "True", "True");
                                        }

                                        break;
                                    }
                                case "SlewToAzimuth":
                                    {
                                        if (canReadSlewing)
                                        {
                                            try
                                            {
                                                StartTime = DateTime.Now;
                                                DeviceObject.SlewToAzimuth((object)225.0d);
                                                do
                                                {
                                                    Thread.Sleep(100);
                                                    Application.DoEvents();
                                                    Action(Test + " " + DateTime.Now.Subtract(StartTime).Seconds + " seconds / " + DOME_SLEW_TIMEOUT);
                                                }
                                                while (!Operators.OrObject(Operators.ConditionalCompareObjectEqual(DeviceObject.Slewing, false, false), DateTime.Now.Subtract(StartTime).TotalSeconds > DOME_SLEW_TIMEOUT));
                                                this.Compare(Device, Test + " Not Complete", DeviceObject.Slewing.ToString(), "False");
                                                CompareDouble(Device, Test, Conversions.ToDouble(DeviceObject.Azimuth), 225.0d, TOLERANCE_5_SECONDS, DoubleType.Degrees0To360);
                                            }
                                            catch (COMException ex) when (ex.ErrorCode == int.MinValue + 0x00040400)
                                            {
                                            }
                                            catch (MethodNotImplementedException)
                                            {
                                                Compare(Device, "SlewToAzimuth - Simulator SlewToAzimuth method is disabled", "True", "True");
                                            }
                                        }
                                        else
                                        {
                                            Compare(Device, "SlewToAzimuth - Skipping test because simulator Slewing property is not accessible", "True", "True");
                                        }

                                        break;
                                    }

                                default:
                                    {
                                        LogException("DeviceTest", "Unknown Dome Test: " + Test);
                                        break;
                                    }
                            }

                            break;
                        }

                    case "Video":
                        {
                            switch (Test ?? "")
                            {
                                case "FrameNumber":
                                    {
                                        Compare(Device, Test, Operators.ConditionalCompareObjectGreaterEqual(DeviceObject.FrameNumber, 0, false).ToString(), "True");
                                        break;
                                    }
                                case "BitDepth":
                                    {
                                        Compare(Device, Test, Operators.ConditionalCompareObjectGreaterEqual(DeviceObject.BitDepth, 0, false).ToString(), "True");
                                        break;
                                    }
                                case "CanConfigureDeviceProperties":
                                    {
                                        this.Compare(Device, Test, DeviceObject.CanConfigureDeviceProperties.ToString(), "True");
                                        break;
                                    }
                                case "ExposureMin":
                                    {
                                        Compare(Device, Test, Operators.ConditionalCompareObjectGreaterEqual(DeviceObject.BitDepth, 0.0d, false).ToString(), "True");
                                        break;
                                    }
                                case "Height":
                                    {
                                        Compare(Device, Test, Operators.ConditionalCompareObjectGreaterEqual(DeviceObject.BitDepth, 0, false).ToString(), "True");
                                        break;
                                    }
                                case "Width":
                                    {
                                        Compare(Device, Test, Operators.ConditionalCompareObjectGreaterEqual(DeviceObject.BitDepth, 0, false).ToString(), "True");
                                        break;
                                    }

                                default:
                                    {
                                        LogException("DeviceTest", "Unknown Video Test: " + Test);
                                        break;
                                    }
                            }

                            break;
                        }

                    case "ObservingConditions":
                        {
                            switch (Test ?? "")
                            {
                                case "AveragePeriod":
                                    {
                                        Compare(Device, Test, Operators.ConditionalCompareObjectGreaterEqual(DeviceObject.AveragePeriod, 0.0d, false).ToString(), "True");
                                        break;
                                    }
                                case "TimeSinceLastUpdate":
                                    {
                                        Compare(Device, Test, Information.IsNumeric(DeviceObject.TimeSinceLastUpdate("")).ToString(), "True");
                                        break;
                                    }

                                default:
                                    {
                                        LogException("DeviceTest", "Unknown ObservingConditions Test: " + Test);
                                        break;
                                    }
                            }

                            break;
                        }

                    default:
                        {
                            LogException("DeviceTest", "Unknown Device: " + Device);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                LogException("DeviceTest", Device + " " + Test + " exception: " + ex.ToString());
            }
            return RetVal;
        }

        private enum NOVAS3Functions
        {
            PlanetEphemeris,
            ReadEph,
            SolarSystem,
            State,
            DeltaT,
            // =============================================
            CheckoutStarsFull,
            // =============================================
            Aberration,
            AppPlanet,
            AppStar,
            AstroPlanet,
            AstroStar,
            Bary2Obs,
            CalDate,
            Cel2Ter, // Only from NOVAS 3.1 onwards
            CelPole,
            CioArray,
            CioBasis,
            CioLocation,
            CioRa,
            DLight,
            Ecl2EquVec,
            EeCt,
            Ephemeris,
            Equ2Ecl,
            Equ2EclVec,
            Equ2Gal,
            Equ2Hor,
            Era,
            ETilt,
            FrameTie,
            FundArgs,
            Gcrs2Equ,
            GeoPosVel,
            GravDef,
            GravVec,
            IraEquinox,
            JulianDate,
            LightTime,
            LimbAngle,
            LocalPlanet,
            LocalStar,
            MakeCatEntry,
            MakeInSpace,
            MakeObject,
            MakeObserver,
            MakeObserverAtGeocenter,
            MakeObserverInSpace,
            MakeObserverOnSurface,
            MakeOnSurface,
            MeanObliq,
            MeanStar,
            NormAng,
            Nutation,
            NutationAngles,
            Place,
            Precession,
            ProperMotion,
            RaDec2Vector,
            RadVel,
            Refract,
            SiderealTime,
            Spin,
            StarVectors,
            Tdb2Tt,
            Ter2Cel,
            Terra,
            TopoPlanet,
            TopoStar,
            TransformCat,
            TransformHip,
            Vector2RaDec,
            VirtualPlanet,
            VirtualStar,
            Wobble
        }

        private void NOVAS3Tests()
        {
            Status("NOVAS 3 Tests");

            NOVAS3Test(NOVAS3Functions.PlanetEphemeris);
            NOVAS3Test(NOVAS3Functions.ReadEph);
            NOVAS3Test(NOVAS3Functions.SolarSystem);
            NOVAS3Test(NOVAS3Functions.State);

            NOVAS3Test(NOVAS3Functions.Aberration);
            NOVAS3Test(NOVAS3Functions.AppPlanet);
            NOVAS3Test(NOVAS3Functions.AppStar);
            NOVAS3Test(NOVAS3Functions.AstroPlanet);
            NOVAS3Test(NOVAS3Functions.AstroStar);
            NOVAS3Test(NOVAS3Functions.Bary2Obs);
            NOVAS3Test(NOVAS3Functions.CalDate);
            NOVAS3Test(NOVAS3Functions.CelPole);
            NOVAS3Test(NOVAS3Functions.CioArray);
            NOVAS3Test(NOVAS3Functions.CioBasis);
            NOVAS3Test(NOVAS3Functions.CioLocation);
            NOVAS3Test(NOVAS3Functions.CioRa);
            NOVAS3Test(NOVAS3Functions.DLight);
            NOVAS3Test(NOVAS3Functions.Ecl2EquVec);
            NOVAS3Test(NOVAS3Functions.EeCt);
            NOVAS3Test(NOVAS3Functions.Ephemeris);
            NOVAS3Test(NOVAS3Functions.Equ2Ecl);
            NOVAS3Test(NOVAS3Functions.Equ2EclVec);
            NOVAS3Test(NOVAS3Functions.Equ2Gal);
            NOVAS3Test(NOVAS3Functions.Equ2Hor);
            NOVAS3Test(NOVAS3Functions.Era);
            NOVAS3Test(NOVAS3Functions.ETilt);
            NOVAS3Test(NOVAS3Functions.FrameTie);
            NOVAS3Test(NOVAS3Functions.FundArgs);
            NOVAS3Test(NOVAS3Functions.Gcrs2Equ);
            NOVAS3Test(NOVAS3Functions.GeoPosVel);
            NOVAS3Test(NOVAS3Functions.GravDef);
            NOVAS3Test(NOVAS3Functions.GravVec);
            NOVAS3Test(NOVAS3Functions.IraEquinox);
            NOVAS3Test(NOVAS3Functions.JulianDate);
            NOVAS3Test(NOVAS3Functions.LightTime);
            NOVAS3Test(NOVAS3Functions.LimbAngle);
            NOVAS3Test(NOVAS3Functions.LocalPlanet);
            NOVAS3Test(NOVAS3Functions.LocalStar);
            NOVAS3Test(NOVAS3Functions.MakeCatEntry);
            NOVAS3Test(NOVAS3Functions.MakeInSpace);
            NOVAS3Test(NOVAS3Functions.MakeObject);
            NOVAS3Test(NOVAS3Functions.MakeObserver);
            NOVAS3Test(NOVAS3Functions.MakeObserverAtGeocenter);
            NOVAS3Test(NOVAS3Functions.MakeObserverInSpace);
            NOVAS3Test(NOVAS3Functions.MakeObserverOnSurface);
            NOVAS3Test(NOVAS3Functions.MakeOnSurface);
            NOVAS3Test(NOVAS3Functions.MeanObliq);
            NOVAS3Test(NOVAS3Functions.MeanStar);
            NOVAS3Test(NOVAS3Functions.NormAng);
            NOVAS3Test(NOVAS3Functions.Nutation);
            NOVAS3Test(NOVAS3Functions.NutationAngles);
            NOVAS3Test(NOVAS3Functions.Place);
            NOVAS3Test(NOVAS3Functions.Precession);
            NOVAS3Test(NOVAS3Functions.ProperMotion);
            NOVAS3Test(NOVAS3Functions.RaDec2Vector);
            NOVAS3Test(NOVAS3Functions.RadVel);
            NOVAS3Test(NOVAS3Functions.Refract);
            NOVAS3Test(NOVAS3Functions.SiderealTime);
            NOVAS3Test(NOVAS3Functions.Spin);
            NOVAS3Test(NOVAS3Functions.StarVectors);
            NOVAS3Test(NOVAS3Functions.Tdb2Tt);
            NOVAS3Test(NOVAS3Functions.Ter2Cel);
            NOVAS3Test(NOVAS3Functions.Terra);
            NOVAS3Test(NOVAS3Functions.TopoPlanet);
            NOVAS3Test(NOVAS3Functions.TopoStar);
            NOVAS3Test(NOVAS3Functions.TransformCat);
            NOVAS3Test(NOVAS3Functions.TransformHip);
            NOVAS3Test(NOVAS3Functions.Vector2RaDec);
            NOVAS3Test(NOVAS3Functions.VirtualPlanet);
            NOVAS3Test(NOVAS3Functions.VirtualStar);
            NOVAS3Test(NOVAS3Functions.Wobble);
            TL.BlankLine();

            CheckoutStarsFull();

            TL.BlankLine();

        }

        private void CheckoutStarsFull()
        {
            // Port of the NOVAS 3 ChecoutStarsFull.c program to confirm correct implementation

            const int N_STARS = 3;
            const int N_TIMES = 4;


            // /*
            // Main function to check out many parts of NOVAS-C by calling
            // function 'topo_star' with version 1 of function 'solarsystem'.

            // For use with NOVAS-C Version 3.
            // */

            var accuracy = default(short);
            short i, j, rc;

            // /*
            // 'deltat' is the difference in time scales, TT - UT1.

            // The(Array) 'tjd' contains four selected Julian dates at which the
            // star positions will be evaluated.
            // */

            double deltat = 60.0d;
            double[] tjd = [2450203.5d, 2450203.5d, 2450417.5d, 2450300.5d];
            double ra = default, dec = default;

            // /*
            // Hipparcos (ICRS) catalog data for three selected stars.
            // */
            var stars = new CatEntry3[3];
            Nov3.MakeCatEntry("POLARIS", "HIP", 0, 2.530301028d, 89.264109444d, 44.22d, -11.75d, 7.56d, -17.4d, ref stars[0]);
            Nov3.MakeCatEntry("Delta ORI", "HIP", 1, 5.533444639d, -0.299091944d, 1.67d, 0.56d, 3.56d, 16.0d, ref stars[1]);
            Nov3.MakeCatEntry("Theta CAR", "HIP", 2, 10.715944806d, -64.39445d, -18.87d, 12.06d, 7.43d, 24.0d, ref stars[2]);

            // /*
            // The(Observer) 's terrestrial coordinates (latitude, longitude, height).
            // */
            var geo_loc = new OnSurface()
            {
                Latitude = 45.0d,
                Longitude = -75.0d,
                Height = 0.0d,
                Temperature = 10.0d,
                Pressure = 1010.0d
            };

            // /*
            // Compute the topocentric places of the three stars at the four
            // selected Julian dates.
            // */

            var ExpectedResults = new string[4, 3];
            ExpectedResults[0, 0] = "2450203" + DecimalSeparator + "5 POLARIS RA = 2" + DecimalSeparator + "446988922 Dec = 89" + DecimalSeparator + "24635149";
            ExpectedResults[0, 1] = "2450203" + DecimalSeparator + "5 Delta ORI RA = 5" + DecimalSeparator + "530110723 Dec = -0" + DecimalSeparator + "30571737";
            ExpectedResults[0, 2] = "2450203" + DecimalSeparator + "5 Theta CAR RA = 10" + DecimalSeparator + "714525513 Dec = -64" + DecimalSeparator + "38130590";
            ExpectedResults[1, 0] = "2450203" + DecimalSeparator + "5 POLARIS RA = 2" + DecimalSeparator + "446988922 Dec = 89" + DecimalSeparator + "24635149";
            ExpectedResults[1, 1] = "2450203" + DecimalSeparator + "5 Delta ORI RA = 5" + DecimalSeparator + "530110723 Dec = -0" + DecimalSeparator + "30571737";
            ExpectedResults[1, 2] = "2450203" + DecimalSeparator + "5 Theta CAR RA = 10" + DecimalSeparator + "714525513 Dec = -64" + DecimalSeparator + "38130590";
            ExpectedResults[2, 0] = "2450417" + DecimalSeparator + "5 POLARIS RA = 2" + DecimalSeparator + "509480139 Dec = 89" + DecimalSeparator + "25196813";
            ExpectedResults[2, 1] = "2450417" + DecimalSeparator + "5 Delta ORI RA = 5" + DecimalSeparator + "531195904 Dec = -0" + DecimalSeparator + "30301961";
            ExpectedResults[2, 2] = "2450417" + DecimalSeparator + "5 Theta CAR RA = 10" + DecimalSeparator + "714444761 Dec = -64" + DecimalSeparator + "37366514";
            ExpectedResults[3, 0] = "2450300" + DecimalSeparator + "5 POLARIS RA = 2" + DecimalSeparator + "481177532 Dec = 89" + DecimalSeparator + "24254404";
            ExpectedResults[3, 1] = "2450300" + DecimalSeparator + "5 Delta ORI RA = 5" + DecimalSeparator + "530372288 Dec = -0" + DecimalSeparator + "30231606";
            ExpectedResults[3, 2] = "2450300" + DecimalSeparator + "5 Theta CAR RA = 10" + DecimalSeparator + "713575394 Dec = -64" + DecimalSeparator + "37966995";

            for (i = 0; i <= N_TIMES - 1; i++)
            {

                for (j = 0; j <= N_STARS - 1; j++)
                {
                    rc = Nov3.TopoStar(tjd[i], deltat, stars[j], geo_loc, (Accuracy)accuracy, ref ra, ref dec);
                    if (rc != 0)
                    {
                        LogRC(NOVAS3Functions.CheckoutStarsFull, "Main loop", rc, "Error " + rc + " from topo_star", "");
                    }
                    else
                    {

                        LogRC(NOVAS3Functions.CheckoutStarsFull, "Main loop", rc, tjd[i] + " " + stars[j].StarName + " RA = " + Strings.Format(ra, "0.000000000") + " Dec = " + Strings.Format(dec, "0.00000000"), ExpectedResults[i, j]);
                    }
                }
            }

        }

        private void NOVAS3Test(NOVAS3Functions TestFunction)
        {
            int rc;
            var CatEnt = new CatEntry3();
            var ObjectJupiter = new Object3();
            var Observer = new Observer();
            SkyPos skypos = new(), SkyPos1 = new();
            var OnSurf = new OnSurface();
            double RA = default, Dec = default, Dis = default, JD, GST = default, JDTest;
            BodyDescription BodyJupiter = new(), BodyEarth = new();
            var Si = new SiteInfo();
            double[] Pos = new double[3], Pos1 = new double[3], Pos2 = new double[3], Vel = new double[3], PosObj = new double[3], PosObs = new double[3], PosBody = new double[3], VelObs = new double[3];
            var Utl = new Util();

            const double DeltaT = 66.8d;

            Action(TestFunction.ToString());
            JDTest = TestJulianDate();

            Pos1[0] = 1d;
            Pos1[1] = 1d;
            Pos1[2] = 1d;
            PosObs[0] = 0.0001d;
            PosObs[1] = 0.0001d;
            PosObs[2] = 0.0001d;
            Pos2[0] = 0.0001d;
            Pos2[1] = 0.0001d;
            Pos2[2] = 0.0001d;
            Pos[0] = 100.0d;
            Pos[1] = 100.0d;
            Pos[2] = 100.0d;
            PosBody[0] = 0.01d;
            PosBody[1] = 0.01d;
            PosBody[2] = 0.01d;
            Vel[0] = 1000d;
            Vel[1] = 1000d;
            Vel[2] = 1000d;
            VelObs[0] = 500d;
            VelObs[1] = 500d;
            VelObs[2] = 500d;

            Si.Height = 80d;
            Si.Latitude = 51d;
            Si.Longitude = 0d;
            Si.Pressure = 1010d;
            Si.Temperature = 25d;

            BodyJupiter.Name = "Jupiter";
            BodyJupiter.Number = Body.Jupiter;
            BodyJupiter.Type = BodyType.MajorPlanet;

            BodyEarth.Name = "Earth";
            BodyEarth.Number = Body.Earth;
            BodyEarth.Type = BodyType.MajorPlanet;

            CatEnt.Catalog = "GMB";
            CatEnt.Dec = 23.23d;
            CatEnt.Parallax = 10.0d;
            CatEnt.ProMoDec = 20.0d;
            CatEnt.ProMoRA = 30.0d;
            CatEnt.RA = 39.39d;
            CatEnt.RadialVelocity = 40.0d;
            CatEnt.StarName = "GMB 1830";
            CatEnt.StarNumber = 1830;

            ObjectJupiter.Name = "Jupiter";
            ObjectJupiter.Number = Body.Jupiter;
            ObjectJupiter.Star = new CatEntry3();
            ObjectJupiter.Type = Astrometry.ObjectType.MajorPlanetSunOrMoon;

            Observer.OnSurf.Height = 80d;
            Observer.OnSurf.Latitude = 51.0d;
            Observer.OnSurf.Longitude = 0.0d;
            Observer.OnSurf.Pressure = 1010d;
            Observer.OnSurf.Temperature = 20d;
            Observer.Where = ObserverLocation.EarthSurface;

            OnSurf.Height = 80d;
            OnSurf.Latitude = 51.0d;
            OnSurf.Longitude = 0.0d;
            OnSurf.Pressure = 1000d;
            OnSurf.Temperature = 5d;

            try
            {
                switch (TestFunction)
                {
                    case NOVAS3Functions.PlanetEphemeris:
                        {
                            var JDArr = new double[2];
                            JDArr[0] = JDTest;
                            JDArr[1] = 0d;
                            rc = Nov3.PlanetEphemeris(ref JDArr, Target.Jupiter, Target.Earth, ref Pos, ref Vel);
                            LogRC(TestFunction, "Ju Ea", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            rc = Nov3.PlanetEphemeris(ref JDArr, Target.Earth, Target.Jupiter, ref Pos, ref Vel);
                            LogRC(TestFunction, "Ea Ju", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            rc = Nov3.PlanetEphemeris(ref JDArr, Target.Jupiter, Target.Mercury, ref Pos, ref Vel);
                            LogRC(TestFunction, "Ju Me", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            rc = Nov3.PlanetEphemeris(ref JDArr, Target.Moon, Target.Earth, ref Pos, ref Vel);
                            LogRC(TestFunction, "Mo Ea", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            rc = Nov3.PlanetEphemeris(ref JDArr, Target.SolarSystemBarycentre, Target.Moon, ref Pos, ref Vel);
                            LogRC(TestFunction, "SS Mo", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            break;
                        }
                    case NOVAS3Functions.SolarSystem:
                        {
                            rc = Nov3.SolarSystem(JDTest, Body.Neptune, Origin.Barycentric, ref Pos, ref Vel);
                            LogRC(TestFunction, "Neptune", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            break;
                        }
                    case NOVAS3Functions.State:
                        {
                            var JDArr = new double[2];
                            JDArr[0] = JDTest;
                            JDArr[1] = 0d;
                            rc = Nov3.State(ref JDArr, Target.Pluto, ref Pos, ref Vel);
                            LogRC(TestFunction, "Pluto", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            break;
                        }
                    case NOVAS3Functions.Aberration:
                        {
                            rc = 0;
                            Nov3.RaDec2Vector(20.0d, 40.0d, 100d, ref Pos);
                            LogRC(TestFunction, "X, Y, Z", rc, Pos[0] + " " + Pos[1] + " " + Pos[2], "");

                            Nov3.Aberration(Pos, Pos, 10.0d, ref Pos2);
                            LogRC(TestFunction, "X, Y, Z", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.AppPlanet:
                        {
                            rc = Nov3.AppPlanet(JDTest, ObjectJupiter, Accuracy.Full, ref RA, ref Dec, ref Dis);
                            LogRC(TestFunction, "Jupiter", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3) + " " + Dis, "");
                            ObjectJupiter.Number = Body.Moon;
                            ObjectJupiter.Name = "Moon";
                            ObjectJupiter.Type = Astrometry.ObjectType.MajorPlanetSunOrMoon;
                            rc = Nov3.AppPlanet(JDTest, ObjectJupiter, Accuracy.Full, ref RA, ref Dec, ref Dis);
                            LogRC(TestFunction, "Moon", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3) + " " + Dis, "");
                            break;
                        }
                    case NOVAS3Functions.AppStar:
                        {
                            rc = Nov3.AppStar(JDTest, CatEnt, Accuracy.Full, ref RA, ref Dec);
                            LogRC(TestFunction, "RA Dec", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3), "");
                            break;
                        }
                    case NOVAS3Functions.AstroPlanet:
                        {
                            rc = Nov3.AstroPlanet(JDTest, ObjectJupiter, Accuracy.Full, ref RA, ref Dec, ref Dis);
                            LogRC(TestFunction, "Jupiter", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3) + " " + Dis, "");
                            break;
                        }
                    case NOVAS3Functions.AstroStar:
                        {
                            CatEnt.Catalog = "FK6";
                            CatEnt.Dec = 45.45d;
                            CatEnt.Parallax = 0.0d;
                            CatEnt.ProMoDec = 0.0d;
                            CatEnt.ProMoRA = 0.0d;
                            CatEnt.RA = 15.15d;
                            CatEnt.RadialVelocity = 0.0d;
                            CatEnt.StarName = "GMB 1830";
                            CatEnt.StarNumber = 1307;

                            rc = Nov3.AstroStar(Utl.JulianDate, CatEnt, Accuracy.Reduced, ref RA, ref Dec);
                            LogRC(TestFunction, "Reduced accuracy:", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3), "");
                            rc = Nov3.AstroStar(Utl.JulianDate, CatEnt, Accuracy.Full, ref RA, ref Dec);
                            LogRC(TestFunction, "Full accuracy:   ", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3), "");
                            break;
                        }
                    case NOVAS3Functions.Bary2Obs:
                        {
                            var LightTime = default(double);
                            rc = 0;
                            Nov3.RaDec2Vector(20.0d, 40.0d, 100d, ref Pos);

                            Nov3.Bary2Obs(Pos, PosObs, ref Pos2, ref LightTime);
                            LogRC(TestFunction, "X, Y, Z, LightTime", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2] + " " + LightTime, "");
                            break;
                        }
                    case NOVAS3Functions.CalDate:
                        {
                            short Year = default, Month = default, Day = default;
                            var Hour = default(double);
                            rc = 0;
                            Nov3.CalDate(JDTest, ref Year, ref Month, ref Day, ref Hour);
                            LogRC(TestFunction, "Year Month Day Hour", rc, Year + " " + Month + " " + Day + " " + Strings.Format(Hour, "0.0"), "2010 12 30 9" + DecimalSeparator + "0");
                            break;
                        }
                    case NOVAS3Functions.CelPole:
                        {
                            double DPole1 = default, DPole2 = default;
                            rc = Nov3.CelPole(JDTest, PoleOffsetCorrection.ReferredToMeanEclipticOfDate, DPole1, DPole2);
                            LogRC(TestFunction, "Mean Ecliptic Of Date", rc, DPole1 + " " + DPole2, "");
                            rc = Nov3.CelPole(JDTest, PoleOffsetCorrection.ReferredToGCRSAxes, DPole1, DPole2);
                            LogRC(TestFunction, "GCRS Axes", rc, DPole1 + " " + DPole2, "");
                            break;
                        }
                    case NOVAS3Functions.CioArray:
                        {
                            var Cio = new ArrayList();
                            rc = Nov3.CioArray(JDTest, 20, ref Cio);
                            LogRC(TestFunction, "RC", rc, rc.ToString(), "");
                            rc = 0;
                            foreach (RAOfCio CioA in Cio)
                                LogRC(TestFunction, "CIO Array", rc, CioA.JdTdb + " " + CioA.RACio, "");
                            break;
                        }
                    case NOVAS3Functions.CioBasis:
                        {
                            double x = default, y = default, z = default;
                            rc = Nov3.CioBasis(JDTest, 20.0d, ReferenceSystem.GCRS, Accuracy.Full, ref x, ref y, ref z);
                            LogRC(TestFunction, "CIO Basis", rc, x + " " + y + " " + z, "");
                            break;
                        }
                    case NOVAS3Functions.CioLocation:
                        {
                            var RAofCIO = default(double);
                            var RefSys = default(ReferenceSystem);
                            rc = Nov3.CioLocation(JDTest, Accuracy.Full, ref RAofCIO, ref RefSys);
                            LogRC(TestFunction, "CIO Location", rc, RAofCIO + " " + RefSys.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.CioRa:
                        {
                            rc = Nov3.CioRa(JDTest, Accuracy.Full, ref RA);
                            LogRC(TestFunction, "CIO RA", rc, RA.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.DLight:
                        {
                            double DLight;
                            rc = 0;
                            DLight = Nov3.DLight(Pos1, PosObs);
                            LogRC(TestFunction, "D Light", rc, DLight.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.Ecl2EquVec:
                        {
                            rc = Nov3.Ecl2EquVec(JDTest, CoordSys.CIOOfDate, Accuracy.Full, Pos1, ref Pos2);
                            LogRC(TestFunction, "X, Y, Z", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.EeCt:
                        {
                            rc = 0;
                            JD = Nov3.EeCt(JDTest, 0.0d, Accuracy.Full);
                            LogRC(TestFunction, "J Date", rc, JD.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.Ephemeris:
                        {
                            var JD1 = new double[2];
                            JD1[0] = JDTest;
                            rc = Nov3.Ephemeris(JD1, ObjectJupiter, Origin.Barycentric, Accuracy.Full, ref Pos, ref Vel);
                            LogRC(TestFunction, "X, Y, Z", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            break;
                        }
                    case NOVAS3Functions.Equ2Ecl:
                        {
                            double ELon = default, ELat = default;
                            RA = 16.0d;
                            Dec = 40.0d;
                            rc = Nov3.Equ2Ecl(JDTest, CoordSys.EquinoxOfDate, Accuracy.Full, RA, Dec, ref ELon, ref ELat);
                            LogRC(TestFunction, "E Lon E Lat", rc, ELon + " " + ELat, "");
                            break;
                        }
                    case NOVAS3Functions.Equ2EclVec:
                        {
                            rc = Nov3.Equ2EclVec(JDTest, CoordSys.EquinoxOfDate, Accuracy.Full, Pos1, ref Pos2);
                            LogRC(TestFunction, "X, Y, Z", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.Equ2Gal:
                        {
                            double GLat = default, GLong = default;
                            rc = 0;
                            Nov3.Equ2Gal(12.456d, 40.0d, ref GLong, ref GLat);
                            LogRC(TestFunction, "G Long, G Lat", rc, GLong + " " + GLat, "");
                            break;
                        }
                    case NOVAS3Functions.Equ2Hor:
                        {
                            double ZD = default, Az = default, RaR = default, DecR = default;
                            rc = 0;
                            Nov3.Equ2Hor(JDTest, 0.0d, Accuracy.Full, 30.0d, 50.0d, OnSurf, RA, Dec, RefractionOption.LocationRefraction, ref ZD, ref Az, ref RaR, ref DecR);
                            LogRC(TestFunction, "ZD Az RaR DecR", rc, ZD + " " + Az + " " + RaR + " " + DecR, "");
                            break;
                        }
                    case NOVAS3Functions.Era:
                        {
                            double Era;
                            rc = 0;
                            Era = Nov3.Era(JDTest, 0.0d);
                            LogRC(TestFunction, "Era", rc, Era.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.ETilt:
                        {
                            double Mobl = default, Tobl = default, Ee = default, DEps = default, DPsi = default;
                            rc = 0;
                            Nov3.ETilt(JDTest, Accuracy.Full, ref Mobl, ref Tobl, ref Ee, ref DPsi, ref DEps);
                            LogRC(TestFunction, "Mobl, Tobl, Ee, DPsi, DEps", rc, Strings.Format(Mobl, "0.00000000") + " " + Strings.Format(Tobl, "0.00000000") + " " + Strings.Format(Ee, "0.00000000") + " " + Strings.Format(DPsi, "0.00000000") + " " + Strings.Format(DEps, "0.00000000"), "");
                            break;
                        }
                    case NOVAS3Functions.FrameTie:
                        {
                            rc = 0;
                            Nov3.FrameTie(Pos1, FrameConversionDirection.DynamicalToICRS, ref Pos2);
                            LogRC(TestFunction, "X, Y, Z", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.FundArgs:
                        {
                            var A = new double[5];
                            rc = 0;
                            Nov3.FundArgs(JDTest, ref A);
                            LogRC(TestFunction, "A", rc, Strings.Format(A[0], "0.00000000") + " " + Strings.Format(A[1], "0.00000000") + " " + Strings.Format(A[2], "0.00000000") + " " + Strings.Format(A[3], "0.00000000") + " " + Strings.Format(A[4], "0.00000000"), "");
                            break;
                        }
                    case NOVAS3Functions.Gcrs2Equ:
                        {
                            double RaG, DecG;
                            RaG = 11.5d;
                            DecG = 40.0d;
                            rc = Nov3.Gcrs2Equ(JDTest, CoordSys.EquinoxOfDate, Accuracy.Full, RaG, DecG, ref RA, ref Dec);
                            LogRC(TestFunction, "RaG 11.5, DecG 40.0", rc, RA + " " + Dec, "");
                            break;
                        }
                    case NOVAS3Functions.GeoPosVel:
                        {
                            rc = Nov3.GeoPosVel(JDTest, 0.0d, Accuracy.Full, Observer, ref Pos, ref Vel);
                            LogRC(TestFunction, "Pos, Vel", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            break;
                        }
                    case NOVAS3Functions.GravDef:
                        {
                            rc = Nov3.GravDef(JDTest, EarthDeflection.AddEarthDeflection, Accuracy.Full, Pos1, PosObs, ref Pos2);
                            LogRC(TestFunction, "X, Y, Z", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.GravVec:
                        {
                            var RMass = default(double);
                            rc = 0;
                            Nov3.GravVec(Pos1, PosObs, PosBody, RMass, ref Pos2);
                            LogRC(TestFunction, "X, Y, Z", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.IraEquinox:
                        {
                            double Ira;
                            rc = 0;
                            Ira = Nov3.IraEquinox(JDTest, EquinoxType.MeanEquinox, Accuracy.Full);
                            LogRC(TestFunction, "Ira", rc, Ira.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.JulianDate:
                        {
                            JD = Nov3.JulianDate(2000, 1, 1, 12d);
                            LogRC(TestFunction, "J2000: ", 0, JD.ToString(), Astrometry.NOVAS.NOVAS2.JulianDate(2000, 1, 1, 12.0d).ToString());
                            JD = Nov3.JulianDate(2010, 1, 2, 0.0d);
                            LogRC(TestFunction, "J2010: ", 0, JD.ToString(), Astrometry.NOVAS.NOVAS2.JulianDate(2010, 1, 2, 0.0d).ToString());
                            break;
                        }
                    case NOVAS3Functions.LightTime:
                        {
                            double TLight0, TLight = default;
                            TLight0 = 0.0d;
                            rc = Nov3.LightTime(JDTest, ObjectJupiter, PosObs, TLight0, Accuracy.Full, ref Pos, ref TLight);
                            LogRC(TestFunction, "X, Y, Z", rc, Pos[0] + " " + Pos[1] + " " + Pos[2] + " " + TLight, "");
                            break;
                        }
                    case NOVAS3Functions.LimbAngle:
                        {
                            double LimbAng = default, NadirAngle = default;
                            rc = 0;
                            Nov3.LimbAngle(PosObj, PosObs, ref LimbAng, ref NadirAngle);
                            LogRC(TestFunction, "LimbAng, NadirAngle", rc, LimbAng + " " + NadirAngle, "");
                            break;
                        }
                    case NOVAS3Functions.LocalPlanet:
                        {
                            rc = Nov3.LocalPlanet(JDTest, ObjectJupiter, 0.0d, OnSurf, Accuracy.Full, ref RA, ref Dec, ref Dis);
                            LogRC(TestFunction, "Ra, Dec, Dis", rc, RA + " " + Dec + " " + Dis, "");
                            break;
                        }
                    case NOVAS3Functions.LocalStar:
                        {
                            rc = Nov3.LocalStar(JDTest, 0.0d, CatEnt, OnSurf, Accuracy.Full, ref RA, ref Dec);
                            LogRC(TestFunction, "Ra, Dec", rc, RA + " " + Dec, "");
                            break;
                        }
                    case NOVAS3Functions.MakeCatEntry:
                        {
                            Nov3.MakeCatEntry("A Star", "FK6", 1234545, 11.0d, 12d, 0d, 13.0d, 14.0d, 15.0d, ref CatEnt);
                            rc = 0;
                            LogRC(TestFunction, "CatEnt", rc, CatEnt.Catalog + " " + CatEnt.Dec + " " + CatEnt.Parallax + " " + CatEnt.StarName + " " + CatEnt.StarNumber, "");
                            break;
                        }
                    case NOVAS3Functions.MakeInSpace:
                        {
                            var Insp = new InSpace();
                            double[] PosOrg = [1d, 2d, 3d], VelOrg = [4d, 5d, 6d];
                            Insp.ScPos = Pos;
                            Insp.ScVel = Vel;
                            rc = 0;
                            Nov3.MakeInSpace(PosOrg, VelOrg, ref Insp);
                            LogRC(TestFunction, "Pos, Vel", rc, Insp.ScPos[0].ToString() + Insp.ScPos[1] + Insp.ScPos[2] + Insp.ScVel[0] + Insp.ScVel[1] + Insp.ScVel[2], "123456");
                            break;
                        }
                    case NOVAS3Functions.MakeObject:
                        {
                            rc = Nov3.MakeObject(Astrometry.ObjectType.MajorPlanetSunOrMoon, 7, "Uranus", CatEnt, ref ObjectJupiter);
                            LogRC(TestFunction, "Object3", rc, ObjectJupiter.Name + " " + ((int)ObjectJupiter.Number).ToString() + " " + ObjectJupiter.Type.ToString() + " " + ObjectJupiter.Star.RA, "");
                            break;
                        }
                    case NOVAS3Functions.MakeObserver:
                        {
                            var Obs = new Observer();
                            OnSurf.Latitude = 51.0d;
                            Nov3.MakeObserver(ObserverLocation.EarthSurface, OnSurf, new InSpace(), ref Obs);
                            rc = 0;
                            LogRC(TestFunction, "Observer", rc, Obs.Where.ToString() + " " + Obs.OnSurf.Latitude, "");
                            break;
                        }
                    case NOVAS3Functions.MakeObserverAtGeocenter:
                        {
                            var Obs = new Observer();
                            Nov3.MakeObserverAtGeocenter(ref Obs);
                            rc = 0;
                            LogRC(TestFunction, "Observer", rc, Obs.Where.ToString() + " " + Obs.OnSurf.Latitude, "");
                            break;
                        }
                    case NOVAS3Functions.MakeObserverInSpace:
                        {
                            var Obs = new Observer();
                            Nov3.MakeObserverInSpace(Pos, Vel, ref Obs);
                            rc = 0;
                            LogRC(TestFunction, "Observer", rc, Obs.Where.ToString() + " " + Obs.OnSurf.Latitude, "");
                            break;
                        }
                    case NOVAS3Functions.MakeObserverOnSurface:
                        {
                            var Obs = new Observer();
                            Nov3.MakeObserverOnSurface(51.0d, 0.0d, 80.0d, 25.0d, 1010d, ref Obs);
                            rc = 0;
                            LogRC(TestFunction, "Observer", rc, Obs.Where.ToString() + " " + Obs.OnSurf.Latitude, "");
                            break;
                        }
                    case NOVAS3Functions.MakeOnSurface:
                        {
                            Nov3.MakeOnSurface(51.0d, 0.0d, 80.0d, 25d, 0d, ref OnSurf);
                            rc = 0;
                            LogRC(TestFunction, "OnSurface", rc, OnSurf.Latitude + " " + OnSurf.Height, "");
                            break;
                        }
                    case NOVAS3Functions.MeanObliq:
                        {
                            double MO;
                            MO = Nov3.MeanObliq(JDTest);
                            rc = 0;
                            LogRC(TestFunction, "Mean Obl", rc, MO.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.MeanStar:
                        {
                            double IRa = default, IDec = default;
                            rc = Nov3.MeanStar(JDTest, RA, Dec, Accuracy.Full, ref IRa, ref IDec);
                            LogRC(TestFunction, "IRa, IDec", rc, IRa + " " + IDec, "");
                            break;
                        }
                    case NOVAS3Functions.NormAng:
                        {
                            double NA;
                            NA = Nov3.NormAng(4d * 3.142d);
                            rc = 0;
                            LogRC(TestFunction, "Norm Ang", rc, NA.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.Nutation:
                        {
                            rc = 0;
                            Nov3.Nutation(JDTest, NutationDirection.MeanToTrue, Accuracy.Full, Pos, ref Pos2);
                            LogRC(TestFunction, "Pos, Pos2", rc, Strings.Format(Pos[0], "0.00000000") + " " + Strings.Format(Pos[1], "0.00000000") + " " + Strings.Format(Pos[2], "0.00000000") + " " + Strings.Format(Pos2[0], "0.00000000") + " " + Strings.Format(Pos2[1], "0.00000000") + " " + Strings.Format(Pos2[2], "0.00000000"), "");
                            break;
                        }
                    case NOVAS3Functions.NutationAngles:
                        {
                            double DPsi = default, DEps = default;
                            rc = 0;
                            Nov3.NutationAngles(JDTest, Accuracy.Full, ref DPsi, ref DEps);
                            LogRC(TestFunction, "DPsi, DEps", rc, DPsi + " " + DEps, "");
                            break;
                        }
                    case NOVAS3Functions.Place:
                        {
                            ObjectJupiter.Name = "Planet";
                            // Obj.Star = CatEnt
                            ObjectJupiter.Type = 0;
                            OnSurf.Height = 80d;
                            OnSurf.Latitude = Utl.DMSToDegrees("51:04:43");
                            OnSurf.Longitude = -Utl.DMSToDegrees("00:17:40");
                            OnSurf.Pressure = 1010d;
                            OnSurf.Temperature = 10d;

                            Observer.Where = ObserverLocation.EarthSurface;
                            Observer.OnSurf = OnSurf;

                            for (short i = 1; i <= 11; i++)
                            {
                                if (i != 3) // Skip earth test as not relevant - viewing earth from earth has no meaning!
                                {
                                    ObjectJupiter.Number = (Body)i;
                                    JD = Utl.JulianDate;
                                    rc = Nov3.Place(JDTest, ObjectJupiter, Observer, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, ref skypos);
                                    rc = Nov3.Place(JDTest, ObjectJupiter, Observer, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Reduced, ref SkyPos1);
                                    LogRC(TestFunction, "Planet " + Strings.Right(" " + i.ToString(), 2) + "", rc, Utl.HoursToHMS(SkyPos1.RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(SkyPos1.Dec, ":", ":", "", 3), Utl.HoursToHMS(skypos.RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(skypos.Dec, ":", ":", "", 3));
                                }
                            }

                            break;
                        }
                    case NOVAS3Functions.Precession:
                        {
                            rc = Nov3.Precession(JDTest, Pos, J2000, ref Pos2);
                            LogRC(TestFunction, "Pos2", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.ProperMotion:
                        {
                            rc = 0;
                            Nov3.ProperMotion(JDTest, Pos, Vel, J2000, ref Pos2);
                            LogRC(TestFunction, "Pos2", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.RaDec2Vector:
                        {
                            rc = 0;
                            Nov3.RaDec2Vector(11.0d, 12.0d, 13.0d, ref Pos);
                            LogRC(TestFunction, "Pos", rc, Pos[0] + " " + Pos[1] + " " + Pos[2], "");
                            break;
                        }
                    case NOVAS3Functions.RadVel:
                        {
                            var Rv = default(double);
                            rc = 0;
                            Nov3.RadVel(ObjectJupiter, Pos, Vel, VelObs, 12.0d, 14.0d, 16.0d, ref Rv);
                            LogRC(TestFunction, "Rv", rc, Rv.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.ReadEph:
                        {
                            var Err = default(int);
                            var Eph = new double[6];
                            rc = 0;
                            Eph = Nov3.ReadEph(99, "missingasteroid", JDTest, ref Err);
                            LogRC(TestFunction, "Expect error 4", rc, Err.ToString() + Eph[0] + Eph[1] + Eph[2] + Eph[3] + Eph[4] + Eph[5], "4000000");
                            JD = 2453415.5d;
                            Eph = Nov3.ReadEph(1, "Ceres", JD, ref Err);
                            LogRC(TestFunction, "JD Before " + JD, rc, Err + " " + Strings.Format(Eph[0], "0.00000") + " " + Strings.Format(Eph[1], "0.00000") + " " + Strings.Format(Eph[2], "0.00000") + " " + Strings.Format(Eph[3], "0.00000") + " " + Strings.Format(Eph[4], "0.00000") + " " + Strings.Format(Eph[5], "0.00000"), "3 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000");
                            JD = 2453425.5d;
                            Eph = Nov3.ReadEph(1, "Ceres", JD, ref Err);
                            LogRC(TestFunction, "JD Start  " + JD, rc, Err + " " + Strings.Format(Eph[0], "0.00000") + " " + Strings.Format(Eph[1], "0.00000") + " " + Strings.Format(Eph[2], "0.00000") + " " + Strings.Format(Eph[3], "0.00000") + " " + Strings.Format(Eph[4], "0.00000") + " " + Strings.Format(Eph[5], "0.00000"), "0 -2" + DecimalSeparator + "23084 -1" + DecimalSeparator + "38495 -0" + DecimalSeparator + "19822 0" + DecimalSeparator + "00482 -0" + DecimalSeparator + "00838 -0" + DecimalSeparator + "00493");
                            JD = 2454400.5d;
                            Eph = Nov3.ReadEph(1, "Ceres", JD, ref Err);
                            LogRC(TestFunction, "JD Mid    " + JD, rc, Err + " " + Strings.Format(Eph[0], "0.00000") + " " + Strings.Format(Eph[1], "0.00000") + " " + Strings.Format(Eph[2], "0.00000") + " " + Strings.Format(Eph[3], "0.00000") + " " + Strings.Format(Eph[4], "0.00000") + " " + Strings.Format(Eph[5], "0.00000"), "0 2" + DecimalSeparator + "02286 1" + DecimalSeparator + "91181 0" + DecimalSeparator + "48869 -0" + DecimalSeparator + "00736 0" + DecimalSeparator + "00559 0" + DecimalSeparator + "00413");
                            JD = 2455370.5d; // Fails (screws up the DLL for subsequent calls) beyond JD = 2455389.5, which is just below the theoretical end of 2455402.5
                            Eph = Nov3.ReadEph(1, "Ceres", JD, ref Err);
                            LogRC(TestFunction, "JD End    " + JD, rc, Err + " " + Strings.Format(Eph[0], "0.00000") + " " + Strings.Format(Eph[1], "0.00000") + " " + Strings.Format(Eph[2], "0.00000") + " " + Strings.Format(Eph[3], "0.00000") + " " + Strings.Format(Eph[4], "0.00000") + " " + Strings.Format(Eph[5], "0.00000"), "0 -0" + DecimalSeparator + "08799 -2" + DecimalSeparator + "57887 -1" + DecimalSeparator + "19703 0" + DecimalSeparator + "00983 -0" + DecimalSeparator + "00018 -0" + DecimalSeparator + "00209");
                            JD = 2455410.5d;
                            Eph = Nov3.ReadEph(1, "Ceres", JD, ref Err);
                            LogRC(TestFunction, "JD After  " + JD, rc, Err + " " + Strings.Format(Eph[0], "0.00000") + " " + Strings.Format(Eph[1], "0.00000") + " " + Strings.Format(Eph[2], "0.00000") + " " + Strings.Format(Eph[3], "0.00000") + " " + Strings.Format(Eph[4], "0.00000") + " " + Strings.Format(Eph[5], "0.00000"), "3 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000");
                            break;
                        }
                    case NOVAS3Functions.Refract:
                        {
                            double Refracted;
                            rc = 0;
                            Refracted = Nov3.Refract(OnSurf, RefractionOption.NoRefraction, 70.0d);
                            LogRC(TestFunction, "No refraction Zd 70.0", rc, Refracted.ToString(), "");
                            Refracted = Nov3.Refract(OnSurf, RefractionOption.StandardRefraction, 70.0d);
                            LogRC(TestFunction, "Standard Zd 70.0     ", rc, Refracted.ToString(), "");
                            Refracted = Nov3.Refract(OnSurf, RefractionOption.LocationRefraction, 70.0d);
                            LogRC(TestFunction, "Location Zd 70.0     ", rc, Refracted.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.SiderealTime:
                        {
                            double MObl = default, TObl = default, ee = default, DPSI = default, DEps = default, GST2 = default;
                            JD = Utl.JulianDate;
                            rc = Nov3.SiderealTime(JD, 0.0d, DeltaT, GstType.GreenwichMeanSiderealTime, Method.EquinoxBased, Accuracy.Reduced, ref GST);
                            LogRC(TestFunction, "Local Mean Equinox    ", rc, Utl.HoursToHMS(GST - 24.0d * Utl.DMSToDegrees("00:17:40") / 360d, ":", ":", "", 3), "");
                            rc = Nov3.SiderealTime(JD, 0.0d, DeltaT, GstType.GreenwichApparentSiderealTime, Method.EquinoxBased, Accuracy.Full, ref GST);
                            LogRC(TestFunction, "Local Apparent Equinox", rc, Utl.HoursToHMS(GST - 24.0d * Utl.DMSToDegrees("00:17:40") / 360d, ":", ":", "", 3), "");
                            rc = Nov3.SiderealTime(JD, 0.0d, DeltaT, GstType.GreenwichMeanSiderealTime, Method.CIOBased, Accuracy.Reduced, ref GST);
                            LogRC(TestFunction, "Local Mean CIO        ", rc, Utl.HoursToHMS(GST - 24.0d * Utl.DMSToDegrees("00:17:40") / 360d, ":", ":", "", 3), "");
                            rc = Nov3.SiderealTime(JD, 0.0d, DeltaT, GstType.GreenwichApparentSiderealTime, Method.CIOBased, Accuracy.Reduced, ref GST);
                            LogRC(TestFunction, "Local Apparent CIO    ", rc, Utl.HoursToHMS(GST - 24.0d * Utl.DMSToDegrees("00:17:40") / 360d, ":", ":", "", 3), "");
                            Astrometry.NOVAS.NOVAS2.EarthTilt(JD, ref MObl, ref TObl, ref ee, ref DPSI, ref DEps);
                            Astrometry.NOVAS.NOVAS2.SiderealTime(JD, 0.0d, ee, ref GST2);
                            rc = Nov3.SiderealTime(JD, 0.0d, DeltaT, GstType.GreenwichApparentSiderealTime, Method.EquinoxBased, Accuracy.Full, ref GST);
                            LogRCDouble(TestFunction, "Novas3", "GAST Equinox          ", rc, GST, GST2, TOLERANCE_E4);
                            break;
                        }
                    case NOVAS3Functions.Spin:
                        {
                            rc = 0;
                            Nov3.Spin(20.0d, Pos1, ref Pos2);
                            LogRC(TestFunction, "Pos2", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.StarVectors:
                        {
                            rc = 0;
                            Nov3.StarVectors(CatEnt, ref Pos, ref Vel);
                            LogRC(TestFunction, "Pos, Vel", rc, Strings.Format(Pos[0], "0.000") + " " + Strings.Format(Pos[1], "0.000") + " " + Strings.Format(Pos[2], "0.000") + " " + Strings.Format(Vel[0], "0.00000000") + " " + Strings.Format(Vel[1], "0.00000000") + " " + Strings.Format(Vel[2], "0.00000000"), "");
                            break;
                        }
                    case NOVAS3Functions.Tdb2Tt:
                        {
                            double TT = default, Secdiff = default;
                            rc = 0;
                            Nov3.Tdb2Tt(JDTest, ref TT, ref Secdiff);
                            LogRC(TestFunction, "Pos, Vel", rc, TT + " " + Secdiff, "");
                            break;
                        }
                    case NOVAS3Functions.Ter2Cel:
                        {
                            rc = Nov3.Ter2Cel(JDTest, 0.0d, 0.0d, Method.EquinoxBased, Accuracy.Full, OutputVectorOption.ReferredToEquatorAndEquinoxOfDate, 0.0d, 0.0d, Pos, ref Pos2);
                            LogRC(TestFunction, "Pos2", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.Terra:
                        {
                            rc = 0;
                            Nov3.Terra(OnSurf, 0.0d, ref Pos, ref Vel);
                            LogRC(TestFunction, "Pos, Vel", rc, Strings.Format(Pos[0], "0.00000000") + " " + Strings.Format(Pos[1], "0.00000000") + " " + Strings.Format(Pos[2], "0.00000000") + " " + Strings.Format(Vel[0], "0.00000000") + " " + Strings.Format(Vel[1], "0.00000000") + " " + Strings.Format(Vel[2], "0.00000000"), "");
                            break;
                        }
                    case NOVAS3Functions.TopoPlanet:
                        {
                            rc = Nov3.TopoStar(JDTest, 0.0d, CatEnt, OnSurf, Accuracy.Full, ref RA, ref Dec);
                            LogRC(TestFunction, "RA, Dec", rc, RA + " " + Dec, "");
                            break;
                        }
                    case NOVAS3Functions.TopoStar:
                        {
                            CatEnt.Catalog = "HIP";
                            CatEnt.Dec = Utl.DMSToDegrees("16:30:31");
                            CatEnt.Parallax = 0.0d;
                            CatEnt.ProMoDec = 0.0d;
                            CatEnt.ProMoRA = 0.0d;
                            CatEnt.RA = Utl.HMSToHours("04:35:55.2");
                            CatEnt.RadialVelocity = 0.0d;
                            CatEnt.StarName = "Aldebaran";
                            CatEnt.StarNumber = 21421;

                            OnSurf.Height = 80d;
                            OnSurf.Latitude = 51.0d;
                            OnSurf.Longitude = 0.0d;
                            OnSurf.Pressure = 1010d;
                            OnSurf.Temperature = 10d;

                            rc = Nov3.TopoStar(Utl.JulianDate, 0.0d, CatEnt, OnSurf, Accuracy.Reduced, ref RA, ref Dec);
                            LogRC(TestFunction, "Reduced accuracy", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3), "");

                            rc = Nov3.TopoStar(Utl.JulianDate, 0.0d, CatEnt, OnSurf, Accuracy.Full, ref RA, ref Dec);
                            LogRC(TestFunction, "Full accuracy   ", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3), "");
                            break;
                        }
                    case NOVAS3Functions.TransformCat:
                        {
                            var NewCat = new CatEntry3();
                            rc = Nov3.TransformCat(TransformationOption3.ChangeEquatorAndEquinoxAndEpoch, JDTest, CatEnt, J2000, "PWGS", ref NewCat);
                            LogRC(TestFunction, "NewCat", rc, NewCat.RA + " " + NewCat.Dec + " " + NewCat.Catalog + " " + NewCat.StarName, "");
                            break;
                        }
                    case NOVAS3Functions.TransformHip:
                        {
                            var HipCat = new CatEntry3();
                            rc = 0;
                            Nov3.TransformHip(CatEnt, ref HipCat);
                            LogRC(TestFunction, "HipCat", rc, HipCat.RA + " " + HipCat.Dec + " " + HipCat.Catalog + " " + HipCat.StarName, "");
                            break;
                        }
                    case NOVAS3Functions.Vector2RaDec:
                        {
                            rc = Nov3.Vector2RaDec(Pos, ref RA, ref Dec);
                            LogRC(TestFunction, "RA, Dec", rc, RA + " " + Dec, "");
                            break;
                        }
                    case NOVAS3Functions.VirtualPlanet:
                        {
                            rc = Nov3.VirtualPlanet(JDTest, ObjectJupiter, Accuracy.Full, ref RA, ref Dec, ref Dis);
                            LogRC(TestFunction, "RA, Dec, Dis", rc, RA + " " + Dec + " " + Dis, "");
                            break;
                        }
                    case NOVAS3Functions.VirtualStar:
                        {
                            rc = Nov3.VirtualStar(JDTest, CatEnt, Accuracy.Full, ref RA, ref Dec);
                            LogRC(TestFunction, "RA, Dec", rc, RA + " " + Dec, "");
                            break;
                        }
                    case NOVAS3Functions.Wobble:
                        {
                            rc = 0;
                            Nov3.Wobble(JDTest, 1.0d, 1.0d, Pos1, ref Pos2);
                            LogRC(TestFunction, "Pos2", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }

                    default:
                        {
                            TL.LogMessage(TestFunction.ToString(), "Test not implemented");
                            break;
                        }
                }
            }

            catch (Exception ex)
            {
                LogException("Novas3", TestFunction.ToString() + " - " + ex.ToString());
            }
            Action("");
        }

        private void NOVAS31Tests()
        {
            Status("NOVAS 3.1 Tests");

            NOVAS31Test(NOVAS3Functions.PlanetEphemeris);
            NOVAS31Test(NOVAS3Functions.ReadEph);
            NOVAS31Test(NOVAS3Functions.SolarSystem);
            NOVAS31Test(NOVAS3Functions.State);
            NOVAS31Test(NOVAS3Functions.DeltaT);

            NOVAS31Test(NOVAS3Functions.Aberration);
            NOVAS31Test(NOVAS3Functions.AppPlanet);
            NOVAS31Test(NOVAS3Functions.AppStar);
            NOVAS31Test(NOVAS3Functions.AstroPlanet);
            NOVAS31Test(NOVAS3Functions.AstroStar);
            NOVAS31Test(NOVAS3Functions.Bary2Obs);
            NOVAS31Test(NOVAS3Functions.CalDate);
            NOVAS31Test(NOVAS3Functions.CelPole);
            NOVAS31Test(NOVAS3Functions.CioArray);
            NOVAS31Test(NOVAS3Functions.CioBasis);
            NOVAS31Test(NOVAS3Functions.CioLocation);
            NOVAS31Test(NOVAS3Functions.CioRa);
            NOVAS31Test(NOVAS3Functions.DLight);
            NOVAS31Test(NOVAS3Functions.Ecl2EquVec);
            NOVAS31Test(NOVAS3Functions.EeCt);
            NOVAS31Test(NOVAS3Functions.Ephemeris);
            NOVAS31Test(NOVAS3Functions.Equ2Ecl);
            NOVAS31Test(NOVAS3Functions.Equ2EclVec);
            NOVAS31Test(NOVAS3Functions.Equ2Gal);
            NOVAS31Test(NOVAS3Functions.Equ2Hor);
            NOVAS31Test(NOVAS3Functions.Era);
            NOVAS31Test(NOVAS3Functions.ETilt);
            NOVAS31Test(NOVAS3Functions.FrameTie);
            NOVAS31Test(NOVAS3Functions.FundArgs);
            NOVAS31Test(NOVAS3Functions.Gcrs2Equ);
            NOVAS31Test(NOVAS3Functions.GeoPosVel);
            NOVAS31Test(NOVAS3Functions.GravDef);
            NOVAS31Test(NOVAS3Functions.GravVec);
            NOVAS31Test(NOVAS3Functions.IraEquinox);
            NOVAS31Test(NOVAS3Functions.JulianDate);
            NOVAS31Test(NOVAS3Functions.LightTime);
            NOVAS31Test(NOVAS3Functions.LimbAngle);
            NOVAS31Test(NOVAS3Functions.LocalPlanet);
            NOVAS31Test(NOVAS3Functions.LocalStar);
            NOVAS31Test(NOVAS3Functions.MakeCatEntry);
            NOVAS31Test(NOVAS3Functions.MakeInSpace);
            NOVAS31Test(NOVAS3Functions.MakeObject);
            NOVAS31Test(NOVAS3Functions.MakeObserver);
            NOVAS31Test(NOVAS3Functions.MakeObserverAtGeocenter);
            NOVAS31Test(NOVAS3Functions.MakeObserverInSpace);
            NOVAS31Test(NOVAS3Functions.MakeObserverOnSurface);
            NOVAS31Test(NOVAS3Functions.MakeOnSurface);
            NOVAS31Test(NOVAS3Functions.MeanObliq);
            NOVAS31Test(NOVAS3Functions.MeanStar);
            NOVAS31Test(NOVAS3Functions.NormAng);
            NOVAS31Test(NOVAS3Functions.Nutation);
            NOVAS31Test(NOVAS3Functions.NutationAngles);
            NOVAS31Test(NOVAS3Functions.Place);
            NOVAS31Test(NOVAS3Functions.Precession);
            NOVAS31Test(NOVAS3Functions.ProperMotion);
            NOVAS31Test(NOVAS3Functions.RaDec2Vector);
            NOVAS31Test(NOVAS3Functions.RadVel);
            NOVAS31Test(NOVAS3Functions.Refract);
            NOVAS31Test(NOVAS3Functions.SiderealTime);
            NOVAS31Test(NOVAS3Functions.Spin);
            NOVAS31Test(NOVAS3Functions.StarVectors);
            NOVAS31Test(NOVAS3Functions.Tdb2Tt);
            NOVAS31Test(NOVAS3Functions.Ter2Cel);
            NOVAS31Test(NOVAS3Functions.Terra);
            NOVAS31Test(NOVAS3Functions.TopoPlanet);
            NOVAS31Test(NOVAS3Functions.TopoStar);
            NOVAS31Test(NOVAS3Functions.TransformCat);
            NOVAS31Test(NOVAS3Functions.TransformHip);
            NOVAS31Test(NOVAS3Functions.Vector2RaDec);
            NOVAS31Test(NOVAS3Functions.VirtualPlanet);
            NOVAS31Test(NOVAS3Functions.VirtualStar);
            NOVAS31Test(NOVAS3Functions.Wobble);
            TL.BlankLine();

            CheckoutStarsFull31();

            TL.BlankLine();

        }

        private void CheckoutStarsFull31()
        {
            // Port of the NOVAS 3 ChecoutStarsFull.c program to confirm correct implementation

            const int N_STARS = 3;
            const int N_TIMES = 4;

            // /*
            // Main function to check out many parts of NOVAS-C by calling
            // function 'topo_star' with version 1 of function 'solar system'.

            // For use with NOVAS-C Version 3.
            // */

            short accuracy = 0;
            short i, j, rc;

            // /*
            // 'deltat' is the difference in time scales, TT - UT1.

            // The(Array) 'tjd' contains four selected Julian dates at which the
            // star positions will be evaluated.
            // */

            double deltat = 60.0d;
            double[] tjd = [2450203.5d, 2450203.5d, 2450417.5d, 2450300.5d];
            double ra = default, dec = default;

            // /*
            // Hipparcos (ICRS) catalog data for three selected stars.
            // */
            var stars = new CatEntry3[3];
            Nov31.MakeCatEntry("POLARIS", "HIP", 0, 2.530301028d, 89.264109444d, 44.22d, -11.75d, 7.56d, -17.4d, ref stars[0]);
            Nov31.MakeCatEntry("Delta ORI", "HIP", 1, 5.533444639d, -0.299091944d, 1.67d, 0.56d, 3.56d, 16.0d, ref stars[1]);
            Nov31.MakeCatEntry("Theta CAR", "HIP", 2, 10.715944806d, -64.39445d, -18.87d, 12.06d, 7.43d, 24.0d, ref stars[2]);

            // /*
            // The(Observer) 's terrestrial coordinates (latitude, longitude, height).
            // */
            var geo_loc = new OnSurface()
            {
                Latitude = 45.0d,
                Longitude = -75.0d,
                Height = 0.0d,
                Temperature = 10.0d,
                Pressure = 1010.0d
            };

            // /*
            // Compute the topocentric places of the three stars at the four
            // selected Julian dates.
            // */

            var ExpectedResults = new string[4, 3];
            ExpectedResults[0, 0] = "2450203" + DecimalSeparator + "5 POLARIS RA = 2" + DecimalSeparator + "446988922 Dec = 89" + DecimalSeparator + "24635149";
            ExpectedResults[0, 1] = "2450203" + DecimalSeparator + "5 Delta ORI RA = 5" + DecimalSeparator + "530110723 Dec = -0" + DecimalSeparator + "30571737";
            ExpectedResults[0, 2] = "2450203" + DecimalSeparator + "5 Theta CAR RA = 10" + DecimalSeparator + "714525513 Dec = -64" + DecimalSeparator + "38130590";
            ExpectedResults[1, 0] = "2450203" + DecimalSeparator + "5 POLARIS RA = 2" + DecimalSeparator + "446988922 Dec = 89" + DecimalSeparator + "24635149";
            ExpectedResults[1, 1] = "2450203" + DecimalSeparator + "5 Delta ORI RA = 5" + DecimalSeparator + "530110723 Dec = -0" + DecimalSeparator + "30571737";
            ExpectedResults[1, 2] = "2450203" + DecimalSeparator + "5 Theta CAR RA = 10" + DecimalSeparator + "714525513 Dec = -64" + DecimalSeparator + "38130590";
            ExpectedResults[2, 0] = "2450417" + DecimalSeparator + "5 POLARIS RA = 2" + DecimalSeparator + "509480139 Dec = 89" + DecimalSeparator + "25196813";
            ExpectedResults[2, 1] = "2450417" + DecimalSeparator + "5 Delta ORI RA = 5" + DecimalSeparator + "531195904 Dec = -0" + DecimalSeparator + "30301961";
            ExpectedResults[2, 2] = "2450417" + DecimalSeparator + "5 Theta CAR RA = 10" + DecimalSeparator + "714444761 Dec = -64" + DecimalSeparator + "37366514";
            ExpectedResults[3, 0] = "2450300" + DecimalSeparator + "5 POLARIS RA = 2" + DecimalSeparator + "481177532 Dec = 89" + DecimalSeparator + "24254404";
            ExpectedResults[3, 1] = "2450300" + DecimalSeparator + "5 Delta ORI RA = 5" + DecimalSeparator + "530372288 Dec = -0" + DecimalSeparator + "30231606";
            ExpectedResults[3, 2] = "2450300" + DecimalSeparator + "5 Theta CAR RA = 10" + DecimalSeparator + "713575394 Dec = -64" + DecimalSeparator + "37966995";

            for (i = 0; i <= N_TIMES - 1; i++)
            {

                for (j = 0; j <= N_STARS - 1; j++)
                {
                    rc = Nov31.TopoStar(tjd[i], deltat, stars[j], geo_loc, (Accuracy)accuracy, ref ra, ref dec);
                    if (rc != 0)
                    {
                        LogRC31(NOVAS3Functions.CheckoutStarsFull, "Main loop", rc, "Error " + rc + " from topo_star", "");
                    }
                    else
                    {

                        LogRC31(NOVAS3Functions.CheckoutStarsFull, "Main loop", rc, tjd[i] + " " + stars[j].StarName + " RA = " + Strings.Format(ra, "0.000000000") + " Dec = " + Strings.Format(dec, "0.00000000"), ExpectedResults[i, j]);
                    }
                }
            }

        }

        private void NOVAS31Test(NOVAS3Functions TestFunction)
        {
            int rc;
            var CatEnt = new CatEntry3();
            var ObjectJupiter = new Object3();
            var Observer = new Observer();
            SkyPos skypos = new(), SkyPos1 = new();
            var OnSurf = new OnSurface();
            double RA = default, Dec = default, Dis = default, JD, GST = default, JDTest, DeltaTResult1, DeltaTResult2;
            BodyDescription BodyJupiter = new(), BodyEarth = new();
            var Si = new SiteInfo();
            double[] Pos = new double[3], Pos1 = new double[3], Pos2 = new double[3], Vel = new double[3], PosObj = new double[3], PosObs = new double[3], PosBody = new double[3], VelObs = new double[3];
            var Utl = new Util();

            const double DeltaT = 66.8d;

            Action(TestFunction.ToString());
            JDTest = TestJulianDate();

            Pos1[0] = 1d;
            Pos1[1] = 1d;
            Pos1[2] = 1d;
            PosObs[0] = 0.0001d;
            PosObs[1] = 0.0001d;
            PosObs[2] = 0.0001d;
            Pos2[0] = 0.0001d;
            Pos2[1] = 0.0001d;
            Pos2[2] = 0.0001d;
            Pos[0] = 100.0d;
            Pos[1] = 100.0d;
            Pos[2] = 100.0d;
            PosBody[0] = 0.01d;
            PosBody[1] = 0.01d;
            PosBody[2] = 0.01d;
            Vel[0] = 1000d;
            Vel[1] = 1000d;
            Vel[2] = 1000d;
            VelObs[0] = 500d;
            VelObs[1] = 500d;
            VelObs[2] = 500d;

            Si.Height = 80d;
            Si.Latitude = 51d;
            Si.Longitude = 0d;
            Si.Pressure = 1010d;
            Si.Temperature = 25d;

            BodyJupiter.Name = "Jupiter";
            BodyJupiter.Number = Body.Jupiter;
            BodyJupiter.Type = BodyType.MajorPlanet;

            BodyEarth.Name = "Earth";
            BodyEarth.Number = Body.Earth;
            BodyEarth.Type = BodyType.MajorPlanet;

            CatEnt.Catalog = "GMB";
            CatEnt.Dec = 23.23d;
            CatEnt.Parallax = 10.0d;
            CatEnt.ProMoDec = 20.0d;
            CatEnt.ProMoRA = 30.0d;
            CatEnt.RA = 39.39d;
            CatEnt.RadialVelocity = 40.0d;
            CatEnt.StarName = "GMB 1830";
            CatEnt.StarNumber = 1830;

            ObjectJupiter.Name = "Jupiter";
            ObjectJupiter.Number = Body.Jupiter;
            ObjectJupiter.Star = new CatEntry3();
            ObjectJupiter.Type = Astrometry.ObjectType.MajorPlanetSunOrMoon;

            Observer.OnSurf.Height = 80d;
            Observer.OnSurf.Latitude = 51.0d;
            Observer.OnSurf.Longitude = 0.0d;
            Observer.OnSurf.Pressure = 1010d;
            Observer.OnSurf.Temperature = 20d;
            Observer.Where = ObserverLocation.EarthSurface;

            OnSurf.Height = 80d;
            OnSurf.Latitude = 51.0d;
            OnSurf.Longitude = 0.0d;
            OnSurf.Pressure = 1000d;
            OnSurf.Temperature = 5d;

            try
            {
                switch (TestFunction)
                {
                    case NOVAS3Functions.PlanetEphemeris:
                        {
                            var JDArr = new double[2];
                            JDArr[0] = JDTest;
                            JDArr[1] = 0d;
                            rc = Nov31.PlanetEphemeris(ref JDArr, Target.Jupiter, Target.Earth, ref Pos, ref Vel);
                            LogRC31(TestFunction, "Ju Ea", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            rc = Nov31.PlanetEphemeris(ref JDArr, Target.Earth, Target.Jupiter, ref Pos, ref Vel);
                            LogRC31(TestFunction, "Ea Ju", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            rc = Nov31.PlanetEphemeris(ref JDArr, Target.Jupiter, Target.Mercury, ref Pos, ref Vel);
                            LogRC31(TestFunction, "Ju Me", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            rc = Nov31.PlanetEphemeris(ref JDArr, Target.Moon, Target.Earth, ref Pos, ref Vel);
                            LogRC31(TestFunction, "Mo Ea", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            rc = Nov31.PlanetEphemeris(ref JDArr, Target.SolarSystemBarycentre, Target.Moon, ref Pos, ref Vel);
                            LogRC31(TestFunction, "SS Mo", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            break;
                        }
                    case NOVAS3Functions.SolarSystem:
                        {
                            rc = Nov31.SolarSystem(JDTest, Body.Neptune, Origin.Barycentric, ref Pos, ref Vel);
                            LogRC31(TestFunction, "Neptune", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            break;
                        }
                    case NOVAS3Functions.State:
                        {
                            var JDArr = new double[2];
                            JDArr[0] = JDTest;
                            JDArr[1] = 0d;
                            rc = Nov31.State(ref JDArr, Target.Pluto, ref Pos, ref Vel);
                            LogRC31(TestFunction, "Pluto", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            break;
                        }
                    case NOVAS3Functions.DeltaT:
                        {
                            DeltaTResult1 = Nov31.DeltaT(JDTest);
                            DeltaTResult2 = Nov31.DeltaT(AstroUtil.JulianDateUtc);
                            TL.LogMessage("NOVAS31DeltaT", string.Format("DeltaT (JD {0}): {1}, DeltaT (JD {2}): {3}", JDTest, DeltaTResult1, AstroUtil.JulianDateUtc, DeltaTResult2));

                            // Confirm DeltaT validation limits - supplied Julian date must be in the range [00:00:00 1 January 0100] to [23:59:59.999 31 December 9999]

                            bool WorkedOK;

                            try
                            {
                                JD = Nov31.JulianDate(100, 1, 1, 0.0d);
                                DeltaTResult1 = Nov31.DeltaT(JD);
                                WorkedOK = true;
                                TL.LogMessage("NOVAS31DeltaT", string.Format("Received DeltaT = {0} for Julian date 00:00:00 1 January 0100", DeltaTResult1));
                            }
                            catch (Exception ex)
                            {
                                WorkedOK = false;
                                LogException("NOVAS31DeltaT - Minimum Value", ex.ToString());
                            }
                            CompareBoolean("NOVAS31DeltaT", "Minimum value OK (00:00:00 1 January 0100)", WorkedOK, true);

                            try
                            {
                                JD = Nov31.JulianDate(9999, 12, 31, 23.999d);
                                DeltaTResult1 = Nov31.DeltaT(JD);
                                WorkedOK = true;
                                TL.LogMessage("NOVAS31DeltaT", string.Format("Received DeltaT = {0} for Julian date 23:59:59.999 31 December 9999", DeltaTResult1));
                            }
                            catch (Exception ex)
                            {
                                WorkedOK = false;
                                LogException("NOVAS31DeltaT - Maximum Value", ex.ToString());
                            }
                            CompareBoolean("NOVAS31DeltaT", "Maximum value OK (23:59:59.999 31 December 9999)", WorkedOK, true);

                            try
                            {
                                JD = Nov31.JulianDate(1, 1, 1, 0.0d);
                                DeltaTResult1 = Nov31.DeltaT(JD);
                                WorkedOK = false;
                                TL.LogMessage("NOVAS31DeltaT", string.Format("Received DeltaT = {0} for Julian date 00:00:00 1 January 0001", DeltaTResult1));
                            }
                            catch (InvalidValueException ex)
                            {
                                WorkedOK = true;
                                TL.LogMessage("NOVAS31DeltaT", string.Format("Received an InvalidValueException as expected when the supplied JD is below the minimum value: {0}", ex.Message));
                            }
                            catch (Exception)
                            {
                                WorkedOK = false;
                            }
                            CompareBoolean("NOVAS31DeltaT", "JD below the minimum value threw InvalidValueException as expected (00:00:00 1 January 0001)", WorkedOK, true);

                            // Check that the current value is valid
                            try
                            {
                                JD = Utl.JulianDate;

                                DeltaTResult1 = Math.Abs(Nov31.DeltaT(JD) - CURRENT_LEAP_SECONDS - 32.184d);
                                TL.LogMessage("NOVAS31DeltaT", $"DeltaT value: {Nov31.DeltaT(JD)}, Difference: {DeltaTResult1}");
                                CompareBoolean("NOVAS31DeltaT", "Current DeltaT is within 0.5 seconds of expected value.", DeltaTResult1 < 0.5d, true);
                            }
                            catch (Exception)
                            {
                                WorkedOK = false;
                            }

                            break;
                        }

                    case NOVAS3Functions.Aberration:
                        {
                            rc = 0;
                            Nov31.RaDec2Vector(20.0d, 40.0d, 100d, ref Pos);
                            LogRC31(TestFunction, "X, Y, Z", rc, Pos[0] + " " + Pos[1] + " " + Pos[2], "");

                            Nov31.Aberration(Pos, Pos, 10.0d, ref Pos2);
                            LogRC31(TestFunction, "X, Y, Z", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.AppPlanet:
                        {
                            rc = Nov31.AppPlanet(JDTest, ObjectJupiter, Accuracy.Full, ref RA, ref Dec, ref Dis);
                            LogRC31(TestFunction, "Jupiter", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3) + " " + Dis, "");
                            ObjectJupiter.Number = Body.Moon;
                            ObjectJupiter.Name = "Moon";
                            ObjectJupiter.Type = Astrometry.ObjectType.MajorPlanetSunOrMoon;
                            rc = Nov31.AppPlanet(JDTest, ObjectJupiter, Accuracy.Full, ref RA, ref Dec, ref Dis);
                            LogRC31(TestFunction, "Moon", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3) + " " + Dis, "");
                            break;
                        }
                    case NOVAS3Functions.AppStar:
                        {
                            rc = Nov31.AppStar(JDTest, CatEnt, Accuracy.Full, ref RA, ref Dec);
                            LogRC31(TestFunction, "RA Dec", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3), "");
                            break;
                        }
                    case NOVAS3Functions.AstroPlanet:
                        {
                            rc = Nov31.AstroPlanet(JDTest, ObjectJupiter, Accuracy.Full, ref RA, ref Dec, ref Dis);
                            LogRC31(TestFunction, "Jupiter", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3) + " " + Dis, "");
                            break;
                        }
                    case NOVAS3Functions.AstroStar:
                        {
                            CatEnt.Catalog = "FK6";
                            CatEnt.Dec = 45.45d;
                            CatEnt.Parallax = 0.0d;
                            CatEnt.ProMoDec = 0.0d;
                            CatEnt.ProMoRA = 0.0d;
                            CatEnt.RA = 15.15d;
                            CatEnt.RadialVelocity = 0.0d;
                            CatEnt.StarName = "GMB 1830";
                            CatEnt.StarNumber = 1307;

                            rc = Nov31.AstroStar(Utl.JulianDate, CatEnt, Accuracy.Reduced, ref RA, ref Dec);
                            LogRC31(TestFunction, "Reduced accuracy:", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3), "");
                            rc = Nov31.AstroStar(Utl.JulianDate, CatEnt, Accuracy.Full, ref RA, ref Dec);
                            LogRC31(TestFunction, "Full accuracy:   ", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3), "");
                            break;
                        }
                    case NOVAS3Functions.Bary2Obs:
                        {
                            var LightTime = default(double);
                            rc = 0;
                            Nov31.RaDec2Vector(20.0d, 40.0d, 100d, ref Pos);

                            Nov31.Bary2Obs(Pos, PosObs, ref Pos2, ref LightTime);
                            LogRC31(TestFunction, "X, Y, Z, LightTime", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2] + " " + LightTime, "");
                            break;
                        }
                    case NOVAS3Functions.CalDate:
                        {
                            short Year = default, Month = default, Day = default;
                            var Hour = default(double);
                            rc = 0;
                            Nov31.CalDate(JDTest, ref Year, ref Month, ref Day, ref Hour);
                            LogRC31(TestFunction, "Year Month Day Hour", rc, Year + " " + Month + " " + Day + " " + Strings.Format(Hour, "0.0"), "2010 12 30 9" + DecimalSeparator + "0");
                            break;
                        }
                    case NOVAS3Functions.Cel2Ter:
                        {
                            rc = Nov31.Cel2Ter(JDTest, 0.0d, 0.0d, Method.EquinoxBased, Accuracy.Full, OutputVectorOption.ReferredToEquatorAndEquinoxOfDate, 0.0d, 0.0d, Pos, ref Pos2);
                            LogRC31(TestFunction, "Pos2", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.CelPole:
                        {
                            double DPole1 = default, DPole2 = default;
                            rc = Nov31.CelPole(JDTest, PoleOffsetCorrection.ReferredToMeanEclipticOfDate, DPole1, DPole2);
                            LogRC31(TestFunction, "Mean Ecliptic Of Date", rc, DPole1 + " " + DPole2, "");
                            rc = Nov31.CelPole(JDTest, PoleOffsetCorrection.ReferredToGCRSAxes, DPole1, DPole2);
                            LogRC31(TestFunction, "GCRS Axes", rc, DPole1 + " " + DPole2, "");
                            break;
                        }
                    case NOVAS3Functions.CioArray:
                        {
                            var Cio = new ArrayList();
                            rc = Nov31.CioArray(JDTest, 20, ref Cio);
                            LogRC31(TestFunction, "RC", rc, rc.ToString(), "");
                            rc = 0;
                            foreach (RAOfCio CioA in Cio)
                                LogRC31(TestFunction, "CIO Array", rc, CioA.JdTdb + " " + CioA.RACio, "");
                            break;
                        }
                    case NOVAS3Functions.CioBasis:
                        {
                            double x = default, y = default, z = default;
                            rc = Nov31.CioBasis(JDTest, 20.0d, ReferenceSystem.GCRS, Accuracy.Full, ref x, ref y, ref z);
                            LogRC31(TestFunction, "CIO Basis", rc, x + " " + y + " " + z, "");
                            break;
                        }
                    case NOVAS3Functions.CioLocation:
                        {
                            var RAofCIO = default(double);
                            var RefSys = default(ReferenceSystem);
                            rc = Nov31.CioLocation(JDTest, Accuracy.Full, ref RAofCIO, ref RefSys);
                            LogRC31(TestFunction, "CIO Location", rc, RAofCIO + " " + RefSys.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.CioRa:
                        {
                            rc = Nov31.CioRa(JDTest, Accuracy.Full, ref RA);
                            LogRC31(TestFunction, "CIO RA", rc, RA.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.DLight:
                        {
                            double DLight;
                            rc = 0;
                            DLight = Nov31.DLight(Pos1, PosObs);
                            LogRC31(TestFunction, "D Light", rc, DLight.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.Ecl2EquVec:
                        {
                            rc = Nov31.Ecl2EquVec(JDTest, CoordSys.CIOOfDate, Accuracy.Full, Pos1, ref Pos2);
                            LogRC31(TestFunction, "X, Y, Z", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.EeCt:
                        {
                            rc = 0;
                            JD = Nov31.EeCt(JDTest, 0.0d, Accuracy.Full);
                            LogRC31(TestFunction, "J Date", rc, JD.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.Ephemeris:
                        {
                            var JD1 = new double[2];
                            JD1[0] = JDTest;
                            rc = Nov31.Ephemeris(JD1, ObjectJupiter, Origin.Barycentric, Accuracy.Full, ref Pos, ref Vel);
                            LogRC31(TestFunction, "X, Y, Z", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            break;
                        }
                    case NOVAS3Functions.Equ2Ecl:
                        {
                            double ELon = default, ELat = default;
                            RA = 16.0d;
                            Dec = 40.0d;
                            rc = Nov31.Equ2Ecl(JDTest, CoordSys.EquinoxOfDate, Accuracy.Full, RA, Dec, ref ELon, ref ELat);
                            LogRC31(TestFunction, "E Lon E Lat", rc, ELon + " " + ELat, "");
                            break;
                        }
                    case NOVAS3Functions.Equ2EclVec:
                        {
                            rc = Nov31.Equ2EclVec(JDTest, CoordSys.EquinoxOfDate, Accuracy.Full, Pos1, ref Pos2);
                            LogRC31(TestFunction, "X, Y, Z", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.Equ2Gal:
                        {
                            double GLat = default, GLong = default;
                            rc = 0;
                            Nov31.Equ2Gal(12.456d, 40.0d, ref GLong, ref GLat);
                            LogRC31(TestFunction, "G Long, G Lat", rc, GLong + " " + GLat, "");
                            break;
                        }
                    case NOVAS3Functions.Equ2Hor:
                        {
                            double ZD = default, Az = default, RaR = default, DecR = default;
                            rc = 0;
                            Nov31.Equ2Hor(JDTest, 0.0d, Accuracy.Full, 30.0d, 50.0d, OnSurf, RA, Dec, RefractionOption.LocationRefraction, ref ZD, ref Az, ref RaR, ref DecR);
                            LogRC31(TestFunction, "ZD Az RaR DecR", rc, ZD + " " + Az + " " + RaR + " " + DecR, "");
                            break;
                        }
                    case NOVAS3Functions.Era:
                        {
                            double Era;
                            rc = 0;
                            Era = Nov31.Era(JDTest, 0.0d);
                            LogRC31(TestFunction, "Era", rc, Era.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.ETilt:
                        {
                            double Mobl = default, Tobl = default, Ee = default, DEps = default, DPsi = default;
                            rc = 0;
                            Nov31.ETilt(JDTest, Accuracy.Full, ref Mobl, ref Tobl, ref Ee, ref DPsi, ref DEps);
                            LogRC31(TestFunction, "Mobl, Tobl, Ee, DPsi, DEps", rc, Strings.Format(Mobl, "0.00000000") + " " + Strings.Format(Tobl, "0.00000000") + " " + Strings.Format(Ee, "0.00000000") + " " + Strings.Format(DPsi, "0.00000000") + " " + Strings.Format(DEps, "0.00000000"), "");
                            break;
                        }
                    case NOVAS3Functions.FrameTie:
                        {
                            rc = 0;
                            Nov31.FrameTie(Pos1, FrameConversionDirection.DynamicalToICRS, ref Pos2);
                            LogRC31(TestFunction, "X, Y, Z", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.FundArgs:
                        {
                            var A = new double[5];
                            rc = 0;
                            Nov31.FundArgs(JDTest, ref A);
                            LogRC31(TestFunction, "A", rc, Strings.Format(A[0], "0.00000000") + " " + Strings.Format(A[1], "0.00000000") + " " + Strings.Format(A[2], "0.00000000") + " " + Strings.Format(A[3], "0.00000000") + " " + Strings.Format(A[4], "0.00000000"), "");
                            break;
                        }
                    case NOVAS3Functions.Gcrs2Equ:
                        {
                            double RaG, DecG;
                            RaG = 11.5d;
                            DecG = 40.0d;
                            rc = Nov31.Gcrs2Equ(JDTest, CoordSys.EquinoxOfDate, Accuracy.Full, RaG, DecG, ref RA, ref Dec);
                            LogRC31(TestFunction, "RaG 11.5, DecG 40.0", rc, RA + " " + Dec, "");
                            break;
                        }
                    case NOVAS3Functions.GeoPosVel:
                        {
                            rc = Nov31.GeoPosVel(JDTest, 0.0d, Accuracy.Full, Observer, ref Pos, ref Vel);
                            LogRC31(TestFunction, "Pos, Vel", rc, Strings.Format(Pos[0], "0.0000000000") + " " + Strings.Format(Pos[1], "0.0000000000") + " " + Strings.Format(Pos[2], "0.0000000000") + " " + Strings.Format(Vel[0], "0.0000000000") + " " + Strings.Format(Vel[1], "0.0000000000") + " " + Strings.Format(Vel[2], "0.0000000000"), "");
                            break;
                        }
                    case NOVAS3Functions.GravDef:
                        {
                            rc = Nov31.GravDef(JDTest, EarthDeflection.AddEarthDeflection, Accuracy.Full, Pos1, PosObs, ref Pos2);
                            LogRC31(TestFunction, "X, Y, Z", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.GravVec:
                        {
                            var RMass = default(double);
                            rc = 0;
                            Nov31.GravVec(Pos1, PosObs, PosBody, RMass, ref Pos2);
                            LogRC31(TestFunction, "X, Y, Z", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.IraEquinox:
                        {
                            double Ira;
                            rc = 0;
                            Ira = Nov31.IraEquinox(JDTest, EquinoxType.MeanEquinox, Accuracy.Full);
                            LogRC31(TestFunction, "Ira", rc, Ira.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.JulianDate:
                        {
                            JD = Nov31.JulianDate(2000, 1, 1, 12d);
                            LogRC31(TestFunction, "J2000: ", 0, JD.ToString(), Astrometry.NOVAS.NOVAS2.JulianDate(2000, 1, 1, 12.0d).ToString());
                            JD = Nov31.JulianDate(2010, 1, 2, 0.0d);
                            LogRC31(TestFunction, "J2010: ", 0, JD.ToString(), Astrometry.NOVAS.NOVAS2.JulianDate(2010, 1, 2, 0.0d).ToString());
                            break;
                        }
                    case NOVAS3Functions.LightTime:
                        {
                            double TLight0, TLight = default;
                            TLight0 = 0.0d;
                            rc = Nov31.LightTime(JDTest, ObjectJupiter, PosObs, TLight0, Accuracy.Full, ref Pos, ref TLight);
                            LogRC31(TestFunction, "X, Y, Z", rc, Pos[0] + " " + Pos[1] + " " + Pos[2] + " " + TLight, "");
                            break;
                        }
                    case NOVAS3Functions.LimbAngle:
                        {
                            double LimbAng = default, NadirAngle = default;
                            rc = 0;
                            Nov31.LimbAngle(PosObj, PosObs, ref LimbAng, ref NadirAngle);
                            LogRC31(TestFunction, "LimbAng, NadirAngle", rc, LimbAng + " " + NadirAngle, "");
                            break;
                        }
                    case NOVAS3Functions.LocalPlanet:
                        {
                            rc = Nov31.LocalPlanet(JDTest, ObjectJupiter, 0.0d, OnSurf, Accuracy.Full, ref RA, ref Dec, ref Dis);
                            LogRC31(TestFunction, "Ra, Dec, Dis", rc, RA + " " + Dec + " " + Dis, "");
                            break;
                        }
                    case NOVAS3Functions.LocalStar:
                        {
                            rc = Nov31.LocalStar(JDTest, 0.0d, CatEnt, OnSurf, Accuracy.Full, ref RA, ref Dec);
                            LogRC31(TestFunction, "Ra, Dec", rc, RA + " " + Dec, "");
                            break;
                        }
                    case NOVAS3Functions.MakeCatEntry:
                        {
                            Nov31.MakeCatEntry("A Star", "FK6", 1234545, 11.0d, 12d, 0d, 13.0d, 14.0d, 15.0d, ref CatEnt);
                            rc = 0;
                            LogRC31(TestFunction, "CatEnt", rc, CatEnt.Catalog + " " + CatEnt.Dec + " " + CatEnt.Parallax + " " + CatEnt.StarName + " " + CatEnt.StarNumber, "");
                            break;
                        }
                    case NOVAS3Functions.MakeInSpace:
                        {
                            var Insp = new InSpace();
                            double[] PosOrg = [1d, 2d, 3d], VelOrg = [4d, 5d, 6d];
                            Insp.ScPos = Pos;
                            Insp.ScVel = Vel;
                            rc = 0;
                            Nov31.MakeInSpace(PosOrg, VelOrg, ref Insp);
                            LogRC31(TestFunction, "Pos, Vel", rc, Insp.ScPos[0].ToString() + Insp.ScPos[1] + Insp.ScPos[2] + Insp.ScVel[0] + Insp.ScVel[1] + Insp.ScVel[2], "123456");
                            break;
                        }
                    case NOVAS3Functions.MakeObject:
                        {
                            rc = Nov31.MakeObject(Astrometry.ObjectType.MajorPlanetSunOrMoon, 7, "Uranus", CatEnt, ref ObjectJupiter);
                            LogRC31(TestFunction, "Object3", rc, ObjectJupiter.Name + " " + ((int)ObjectJupiter.Number).ToString() + " " + ObjectJupiter.Type.ToString() + " " + ObjectJupiter.Star.RA, "");
                            break;
                        }
                    case NOVAS3Functions.MakeObserver:
                        {
                            var Obs = new Observer();
                            OnSurf.Latitude = 51.0d;
                            Nov31.MakeObserver(ObserverLocation.EarthSurface, OnSurf, new InSpace(), ref Obs);
                            rc = 0;
                            LogRC31(TestFunction, "Observer", rc, Obs.Where.ToString() + " " + Obs.OnSurf.Latitude, "");
                            break;
                        }
                    case NOVAS3Functions.MakeObserverAtGeocenter:
                        {
                            var Obs = new Observer();
                            Nov31.MakeObserverAtGeocenter(ref Obs);
                            rc = 0;
                            LogRC31(TestFunction, "Observer", rc, Obs.Where.ToString() + " " + Obs.OnSurf.Latitude, "");
                            break;
                        }
                    case NOVAS3Functions.MakeObserverInSpace:
                        {
                            var Obs = new Observer();
                            Nov31.MakeObserverInSpace(Pos, Vel, ref Obs);
                            rc = 0;
                            LogRC31(TestFunction, "Observer", rc, Obs.Where.ToString() + " " + Obs.OnSurf.Latitude, "");
                            break;
                        }
                    case NOVAS3Functions.MakeObserverOnSurface:
                        {
                            var Obs = new Observer();
                            Nov31.MakeObserverOnSurface(51.0d, 0.0d, 80.0d, 25.0d, 1010d, ref Obs);
                            rc = 0;
                            LogRC31(TestFunction, "Observer", rc, Obs.Where.ToString() + " " + Obs.OnSurf.Latitude, "");
                            break;
                        }
                    case NOVAS3Functions.MakeOnSurface:
                        {
                            Nov31.MakeOnSurface(51.0d, 0.0d, 80.0d, 25d, 0d, ref OnSurf);
                            rc = 0;
                            LogRC31(TestFunction, "OnSurface", rc, OnSurf.Latitude + " " + OnSurf.Height, "");
                            break;
                        }
                    case NOVAS3Functions.MeanObliq:
                        {
                            double MO;
                            MO = Nov31.MeanObliq(JDTest);
                            rc = 0;
                            LogRC31(TestFunction, "Mean Obl", rc, MO.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.MeanStar:
                        {
                            double IRa = default, IDec = default;
                            rc = Nov31.MeanStar(JDTest, RA, Dec, Accuracy.Full, ref IRa, ref IDec);
                            LogRC31(TestFunction, "IRa, IDec", rc, IRa + " " + IDec, "");
                            break;
                        }
                    case NOVAS3Functions.NormAng:
                        {
                            double NA;
                            NA = Nov31.NormAng(4d * 3.142d);
                            rc = 0;
                            LogRC31(TestFunction, "Norm Ang", rc, NA.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.Nutation:
                        {
                            rc = 0;
                            Nov31.Nutation(JDTest, NutationDirection.MeanToTrue, Accuracy.Full, Pos, ref Pos2);
                            LogRC31(TestFunction, "Pos, Pos2", rc, Strings.Format(Pos[0], "0.00000000") + " " + Strings.Format(Pos[1], "0.00000000") + " " + Strings.Format(Pos[2], "0.00000000") + " " + Strings.Format(Pos2[0], "0.00000000") + " " + Strings.Format(Pos2[1], "0.00000000") + " " + Strings.Format(Pos2[2], "0.00000000"), "");
                            break;
                        }
                    case NOVAS3Functions.NutationAngles:
                        {
                            double DPsi = default, DEps = default;
                            rc = 0;
                            Nov31.NutationAngles(JDTest, Accuracy.Full, ref DPsi, ref DEps);
                            LogRC31(TestFunction, "DPsi, DEps", rc, DPsi + " " + DEps, "");
                            break;
                        }
                    case NOVAS3Functions.Place:
                        {
                            ObjectJupiter.Name = "Planet";
                            // Obj.Star = CatEnt
                            ObjectJupiter.Type = 0;
                            OnSurf.Height = 80d;
                            OnSurf.Latitude = Utl.DMSToDegrees("51:04:43");
                            OnSurf.Longitude = -Utl.DMSToDegrees("00:17:40");
                            OnSurf.Pressure = 1010d;
                            OnSurf.Temperature = 10d;

                            Observer.Where = ObserverLocation.EarthSurface;
                            Observer.OnSurf = OnSurf;

                            for (short i = 1; i <= 11; i++)
                            {
                                if (i != 3) // Skip earth test as not relevant - viewing earth from earth has no meaning!
                                {
                                    ObjectJupiter.Number = (Body)i;
                                    JD = Utl.JulianDate;
                                    rc = Nov31.Place(JDTest, ObjectJupiter, Observer, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, ref skypos);
                                    rc = Nov31.Place(JDTest, ObjectJupiter, Observer, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Reduced, ref SkyPos1);
                                    LogRC31(TestFunction, "Planet " + Strings.Right(" " + i.ToString(), 2) + "", rc, Utl.HoursToHMS(SkyPos1.RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(SkyPos1.Dec, ":", ":", "", 3), Utl.HoursToHMS(skypos.RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(skypos.Dec, ":", ":", "", 3));
                                }
                            }

                            break;
                        }
                    case NOVAS3Functions.Precession:
                        {
                            rc = Nov31.Precession(JDTest, Pos, J2000, ref Pos2);
                            LogRC31(TestFunction, "Pos2", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.ProperMotion:
                        {
                            rc = 0;
                            Nov31.ProperMotion(JDTest, Pos, Vel, J2000, ref Pos2);
                            LogRC31(TestFunction, "Pos2", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.RaDec2Vector:
                        {
                            rc = 0;
                            Nov31.RaDec2Vector(11.0d, 12.0d, 13.0d, ref Pos);
                            LogRC31(TestFunction, "Pos", rc, Pos[0] + " " + Pos[1] + " " + Pos[2], "");
                            break;
                        }
                    case NOVAS3Functions.RadVel:
                        {
                            var Rv = default(double);
                            rc = 0;
                            Nov31.RadVel(ObjectJupiter, Pos, Vel, VelObs, 12.0d, 14.0d, 16.0d, ref Rv);
                            LogRC31(TestFunction, "Rv", rc, Rv.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.ReadEph:
                        {
                            var Err = default(int);
                            var Eph = new double[6];
                            rc = 0;
                            Eph = Nov31.ReadEph(99, "missingasteroid", JDTest, ref Err);
                            LogRC31(TestFunction, "Expect error 4", rc, Err.ToString() + Eph[0] + Eph[1] + Eph[2] + Eph[3] + Eph[4] + Eph[5], "4000000");
                            JD = 2453415.5d;
                            Eph = Nov31.ReadEph(1, "Ceres", JD, ref Err);
                            LogRC31(TestFunction, "JD Before " + JD, rc, Err + " " + Strings.Format(Eph[0], "0.00000") + " " + Strings.Format(Eph[1], "0.00000") + " " + Strings.Format(Eph[2], "0.00000") + " " + Strings.Format(Eph[3], "0.00000") + " " + Strings.Format(Eph[4], "0.00000") + " " + Strings.Format(Eph[5], "0.00000"), "3 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000");
                            JD = 2453425.5d;
                            Eph = Nov31.ReadEph(1, "Ceres", JD, ref Err);
                            LogRC31(TestFunction, "JD Start  " + JD, rc, Err + " " + Strings.Format(Eph[0], "0.00000") + " " + Strings.Format(Eph[1], "0.00000") + " " + Strings.Format(Eph[2], "0.00000") + " " + Strings.Format(Eph[3], "0.00000") + " " + Strings.Format(Eph[4], "0.00000") + " " + Strings.Format(Eph[5], "0.00000"), "0 -2" + DecimalSeparator + "23084 -1" + DecimalSeparator + "38495 -0" + DecimalSeparator + "19822 0" + DecimalSeparator + "00482 -0" + DecimalSeparator + "00838 -0" + DecimalSeparator + "00493");
                            JD = 2454400.5d;
                            Eph = Nov31.ReadEph(1, "Ceres", JD, ref Err);
                            LogRC31(TestFunction, "JD Mid    " + JD, rc, Err + " " + Strings.Format(Eph[0], "0.00000") + " " + Strings.Format(Eph[1], "0.00000") + " " + Strings.Format(Eph[2], "0.00000") + " " + Strings.Format(Eph[3], "0.00000") + " " + Strings.Format(Eph[4], "0.00000") + " " + Strings.Format(Eph[5], "0.00000"), "0 2" + DecimalSeparator + "02286 1" + DecimalSeparator + "91181 0" + DecimalSeparator + "48869 -0" + DecimalSeparator + "00736 0" + DecimalSeparator + "00559 0" + DecimalSeparator + "00413");
                            JD = 2455370.5d; // Fails (screws up the DLL for subsequent calls) beyond JD = 2455389.5, which is just below the theoretical end of 2455402.5
                            Eph = Nov31.ReadEph(1, "Ceres", JD, ref Err);
                            LogRC31(TestFunction, "JD End    " + JD, rc, Err + " " + Strings.Format(Eph[0], "0.00000") + " " + Strings.Format(Eph[1], "0.00000") + " " + Strings.Format(Eph[2], "0.00000") + " " + Strings.Format(Eph[3], "0.00000") + " " + Strings.Format(Eph[4], "0.00000") + " " + Strings.Format(Eph[5], "0.00000"), "0 -0" + DecimalSeparator + "08799 -2" + DecimalSeparator + "57887 -1" + DecimalSeparator + "19703 0" + DecimalSeparator + "00983 -0" + DecimalSeparator + "00018 -0" + DecimalSeparator + "00209");
                            JD = 2455410.5d;
                            Eph = Nov31.ReadEph(1, "Ceres", JD, ref Err);
                            LogRC31(TestFunction, "JD After  " + JD, rc, Err + " " + Strings.Format(Eph[0], "0.00000") + " " + Strings.Format(Eph[1], "0.00000") + " " + Strings.Format(Eph[2], "0.00000") + " " + Strings.Format(Eph[3], "0.00000") + " " + Strings.Format(Eph[4], "0.00000") + " " + Strings.Format(Eph[5], "0.00000"), "3 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000 0" + DecimalSeparator + "00000");
                            break;
                        }
                    case NOVAS3Functions.Refract:
                        {
                            double Refracted;
                            rc = 0;
                            Refracted = Nov31.Refract(OnSurf, RefractionOption.NoRefraction, 70.0d);
                            LogRC31(TestFunction, "No refraction Zd 70.0", rc, Refracted.ToString(), "");
                            Refracted = Nov31.Refract(OnSurf, RefractionOption.StandardRefraction, 70.0d);
                            LogRC31(TestFunction, "Standard Zd 70.0     ", rc, Refracted.ToString(), "");
                            Refracted = Nov31.Refract(OnSurf, RefractionOption.LocationRefraction, 70.0d);
                            LogRC31(TestFunction, "Location Zd 70.0     ", rc, Refracted.ToString(), "");
                            break;
                        }
                    case NOVAS3Functions.SiderealTime:
                        {
                            double MObl = default, TObl = default, ee = default, DPSI = default, DEps = default, GST2 = default;
                            JD = Utl.JulianDate;
                            rc = Nov31.SiderealTime(JD, 0.0d, DeltaT, GstType.GreenwichMeanSiderealTime, Method.EquinoxBased, Accuracy.Reduced, ref GST);
                            LogRC31(TestFunction, "Local Mean Equinox    ", rc, Utl.HoursToHMS(GST - 24.0d * Utl.DMSToDegrees("00:17:40") / 360d, ":", ":", "", 3), "");
                            rc = Nov31.SiderealTime(JD, 0.0d, DeltaT, GstType.GreenwichApparentSiderealTime, Method.EquinoxBased, Accuracy.Full, ref GST);
                            LogRC31(TestFunction, "Local Apparent Equinox", rc, Utl.HoursToHMS(GST - 24.0d * Utl.DMSToDegrees("00:17:40") / 360d, ":", ":", "", 3), "");
                            rc = Nov31.SiderealTime(JD, 0.0d, DeltaT, GstType.GreenwichMeanSiderealTime, Method.CIOBased, Accuracy.Reduced, ref GST);
                            LogRC31(TestFunction, "Local Mean CIO        ", rc, Utl.HoursToHMS(GST - 24.0d * Utl.DMSToDegrees("00:17:40") / 360d, ":", ":", "", 3), "");
                            rc = Nov31.SiderealTime(JD, 0.0d, DeltaT, GstType.GreenwichApparentSiderealTime, Method.CIOBased, Accuracy.Reduced, ref GST);
                            LogRC31(TestFunction, "Local Apparent CIO    ", rc, Utl.HoursToHMS(GST - 24.0d * Utl.DMSToDegrees("00:17:40") / 360d, ":", ":", "", 3), "");
                            Astrometry.NOVAS.NOVAS2.EarthTilt(JD, ref MObl, ref TObl, ref ee, ref DPSI, ref DEps);
                            Astrometry.NOVAS.NOVAS2.SiderealTime(JD, 0.0d, ee, ref GST2);
                            rc = Nov31.SiderealTime(JD, 0.0d, DeltaT, GstType.GreenwichApparentSiderealTime, Method.EquinoxBased, Accuracy.Full, ref GST);
                            LogRCDouble(TestFunction, "Novas31", "GAST Equinox          ", rc, GST, GST2, TOLERANCE_E4);
                            break;
                        }
                    case NOVAS3Functions.Spin:
                        {
                            rc = 0;
                            Nov31.Spin(20.0d, Pos1, ref Pos2);
                            LogRC31(TestFunction, "Pos2", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.StarVectors:
                        {
                            rc = 0;
                            Nov31.StarVectors(CatEnt, ref Pos, ref Vel);
                            LogRC31(TestFunction, "Pos, Vel", rc, Strings.Format(Pos[0], "0.000") + " " + Strings.Format(Pos[1], "0.000") + " " + Strings.Format(Pos[2], "0.000") + " " + Strings.Format(Vel[0], "0.00000000") + " " + Strings.Format(Vel[1], "0.00000000") + " " + Strings.Format(Vel[2], "0.00000000"), "");
                            break;
                        }
                    case NOVAS3Functions.Tdb2Tt:
                        {
                            double TT = default, Secdiff = default;
                            rc = 0;
                            Nov31.Tdb2Tt(JDTest, ref TT, ref Secdiff);
                            LogRC31(TestFunction, "Pos, Vel", rc, TT + " " + Secdiff, "");
                            break;
                        }
                    case NOVAS3Functions.Ter2Cel:
                        {
                            rc = Nov31.Ter2Cel(JDTest, 0.0d, 0.0d, Method.EquinoxBased, Accuracy.Full, OutputVectorOption.ReferredToEquatorAndEquinoxOfDate, 0.0d, 0.0d, Pos, ref Pos2);
                            LogRC31(TestFunction, "Pos2", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }
                    case NOVAS3Functions.Terra:
                        {
                            rc = 0;
                            Nov31.Terra(OnSurf, 0.0d, ref Pos, ref Vel);
                            LogRC31(TestFunction, "Pos, Vel", rc, Strings.Format(Pos[0], "0.00000000") + " " + Strings.Format(Pos[1], "0.00000000") + " " + Strings.Format(Pos[2], "0.00000000") + " " + Strings.Format(Vel[0], "0.00000000") + " " + Strings.Format(Vel[1], "0.00000000") + " " + Strings.Format(Vel[2], "0.00000000"), "");
                            break;
                        }
                    case NOVAS3Functions.TopoPlanet:
                        {
                            rc = Nov31.TopoStar(JDTest, 0.0d, CatEnt, OnSurf, Accuracy.Full, ref RA, ref Dec);
                            LogRC31(TestFunction, "RA, Dec", rc, RA + " " + Dec, "");
                            break;
                        }
                    case NOVAS3Functions.TopoStar:
                        {
                            CatEnt.Catalog = "HIP";
                            CatEnt.Dec = Utl.DMSToDegrees("16:30:31");
                            CatEnt.Parallax = 0.0d;
                            CatEnt.ProMoDec = 0.0d;
                            CatEnt.ProMoRA = 0.0d;
                            CatEnt.RA = Utl.HMSToHours("04:35:55.2");
                            CatEnt.RadialVelocity = 0.0d;
                            CatEnt.StarName = "Aldebaran";
                            CatEnt.StarNumber = 21421;

                            OnSurf.Height = 80d;
                            OnSurf.Latitude = 51.0d;
                            OnSurf.Longitude = 0.0d;
                            OnSurf.Pressure = 1010d;
                            OnSurf.Temperature = 10d;

                            rc = Nov31.TopoStar(Utl.JulianDate, 0.0d, CatEnt, OnSurf, Accuracy.Reduced, ref RA, ref Dec);
                            LogRC31(TestFunction, "Reduced accuracy", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3), "");

                            rc = Nov31.TopoStar(Utl.JulianDate, 0.0d, CatEnt, OnSurf, Accuracy.Full, ref RA, ref Dec);
                            LogRC31(TestFunction, "Full accuracy   ", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) + " " + Utl.HoursToHMS(Dec, ":", ":", "", 3), "");
                            break;
                        }
                    case NOVAS3Functions.TransformCat:
                        {
                            var NewCat = new CatEntry3();
                            rc = Nov31.TransformCat(TransformationOption3.ChangeEquatorAndEquinoxAndEpoch, JDTest, CatEnt, J2000, "PGS", ref NewCat);
                            LogRC31(TestFunction, "NewCat", rc, NewCat.RA + " " + NewCat.Dec + " " + NewCat.Catalog + " " + NewCat.StarName, "");
                            break;
                        }
                    case NOVAS3Functions.TransformHip:
                        {
                            var HipCat = new CatEntry3();
                            rc = 0;
                            Nov31.TransformHip(CatEnt, ref HipCat);
                            LogRC31(TestFunction, "HipCat", rc, HipCat.RA + " " + HipCat.Dec + " " + HipCat.Catalog + " " + HipCat.StarName, "");
                            break;
                        }
                    case NOVAS3Functions.Vector2RaDec:
                        {
                            rc = Nov31.Vector2RaDec(Pos, ref RA, ref Dec);
                            LogRC31(TestFunction, "RA, Dec", rc, RA + " " + Dec, "");
                            break;
                        }
                    case NOVAS3Functions.VirtualPlanet:
                        {
                            rc = Nov31.VirtualPlanet(JDTest, ObjectJupiter, Accuracy.Full, ref RA, ref Dec, ref Dis);
                            LogRC31(TestFunction, "RA, Dec, Dis", rc, RA + " " + Dec + " " + Dis, "");
                            break;
                        }
                    case NOVAS3Functions.VirtualStar:
                        {
                            rc = Nov31.VirtualStar(JDTest, CatEnt, Accuracy.Full, ref RA, ref Dec);
                            LogRC31(TestFunction, "RA, Dec", rc, RA + " " + Dec, "");
                            break;
                        }
                    case NOVAS3Functions.Wobble:
                        {
                            rc = 0;
                            Nov31.Wobble(JDTest, TransformationDirection.ITRSToTerrestrialIntermediate, 1.0d, 1.0d, Pos1, ref Pos2);
                            LogRC31(TestFunction, "ITRS2Terr Pos2", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            Nov31.Wobble(JDTest, TransformationDirection.TerrestrialIntermediateToITRS, 1.0d, 1.0d, Pos1, ref Pos2);
                            LogRC31(TestFunction, "Terr2ITRS Pos2", rc, Pos2[0] + " " + Pos2[1] + " " + Pos2[2], "");
                            break;
                        }

                    default:
                        {
                            TL.LogMessage(TestFunction.ToString(), "Test not implemented");
                            break;
                        }
                }
            }

            catch (Exception ex)
            {
                LogException("Novas31", TestFunction.ToString() + " - " + ex.ToString());
            }
            Action("");
        }

        private void LogRC31(NOVAS3Functions Test, string Note, int rc, string msg, string Comparison)
        {
            string LMsg;
            if (!string.IsNullOrEmpty(Note))
            {
                Note += ": ";
                LMsg = Note + msg;
            }
            else
            {
                LMsg = msg;
            }

            if (rc == int.MaxValue) // Test is not implemented
            {
                TL.LogMessage("Novas31 *****", "Test " + Test.ToString() + " is not implemented");
            }

            else if (rc == 0)
            {

                if (string.IsNullOrEmpty(Comparison)) // No comparison so it must be a success!
                {
                    Compare("Novas31", Test.ToString() + " - " + LMsg + " RC", rc.ToString(), "0");
                }
                else // Check comparison value and respond accordingly
                {
                    Compare("Novas31", Test.ToString() + " - " + LMsg + " RC", msg, Comparison);
                }
            }
            else
            {
                Compare("Novas31", Test.ToString() + " RC", rc.ToString(), "0");
            }
        }

        private void LogRC(NOVAS3Functions Test, string Note, int rc, string msg, string Comparison)
        {
            string LMsg;
            if (!string.IsNullOrEmpty(Note))
            {
                Note += ": ";
                LMsg = Note + msg;
            }
            else
            {
                LMsg = msg;
            }

            if (rc == int.MaxValue) // Test is not implemented
            {
                TL.LogMessage("Novas3 *****", "Test " + Test.ToString() + " is not implemented");
            }

            else if (rc == 0)
            {

                if (string.IsNullOrEmpty(Comparison)) // No comparison so it must be a success!
                {
                    Compare("Novas3", Test.ToString() + " - " + LMsg + " RC", rc.ToString(), "0");
                }
                else // Check comparison value and respond accordingly
                {
                    Compare("Novas3", Test.ToString() + " - " + LMsg + " RC", msg, Comparison);
                }
            }
            else
            {
                Compare("Novas3", Test.ToString() + " RC", rc.ToString(), "0");
            }
        }

        private void LogRCDouble(NOVAS3Functions Test, string Component, string Note, int rc, double msg, double Comparison, double Tolerance)
        {

            if (rc == int.MaxValue) // Test is not implemented
            {
                TL.LogMessage(Component + " *****", "Test " + Test.ToString() + " is not implemented");
            }
            else if (rc == 0)
            {
                CompareDouble(Component, Test.ToString() + " - " + Note, msg, Comparison, Tolerance);
            }
            else
            {
                Compare(Component, Test.ToString() + " RC", rc.ToString(), "0");
            }
        }

        private enum NOVAS2Functions
        {
            Abberation,
            App_Planet,
            App_Star,
            Astro_Planet,
            Astro_Star,
            Bary_To_Geo,
            Cal_Date,
            Cel_Pole,
            EarthTilt,
            Ephemeris,
            Equ2Hor,
            Fund_Args,
            Get_Earth,
            Julian_Date,
            Local_Planet,
            Local_Star,
            Make_Cat_Entry,
            Mean_Star,
            Nutate,
            Nutation_Angles,
            PNSW,
            Precession,
            Proper_Motion,
            RADEC2Vector,
            Refract,
            Set_Body,
            Sideral_Time,
            SolarSystem,
            Spin,
            StarVectors,
            Sun_Field,
            Tdb2Tdt,
            Terra,
            Topo_Planet,
            Topo_Star,
            Transform_Cat,
            Transform_Hip,
            Vector2RADEC,
            Virtual_Planet,
            Virtual_Star,
            Wobble,
            // Not in DLL  from Tim
            Sun_Eph
        }

        private void NOVAS2Tests()
        {
            // Dim transform As Transform.Transform = New Transform.Transform
            var u = new Util();
            BodyDescription EarthBody = new(), SunBody = new();
            var StarStruct = new CatEntry();
            var LocationStruct = new SiteInfo();
            double[] POS, POS2, POSEarth, VEL2, POSNow, VEL;
            POS = new double[4];
            VEL = new double[4];
            POSNow = new double[4];
            POS2 = new double[4];
            VEL2 = new double[4];
            POSEarth = new double[4];
            double[] FundArgsValue = new double[5];
            double ZenithDistance = default, Azimuth = default, Tdb = default, LightTime = default, RATarget = default, DECTarget = default, JD, Distance = default;
            double RANow = default, DECNow = default, Hour = default, TdtJd = default, SecDiff = default, LongNutation = default, ObliqNutation = default, GreenwichSiderealTime = default, MObl = default, TObl = default, Eq = default, DPsi = default, DEpsilon = default;
            short RC, Year = default, Month = default, day = default;
            string Fmt;

            const double SiteLat = 51.0d + 4.0d / 60.0d + 43.0d / 3600.0d;
            const double SiteLong = 0.0d - 17.0d / 60.0d - 40.0d / 3600.0d;
            const double SiteElev = 80.0d;

            const double StarRAJ2000 = 12.0d;
            const double StarDecJ2000 = 30.0d;

            JD = TestJulianDate();

            EarthBody.Name = "Earth";
            EarthBody.Type = BodyType.MajorPlanet;
            EarthBody.Number = (Body)3;

            SunBody.Name = "Sun";
            SunBody.Type = BodyType.Sun;
            SunBody.Number = Body.Sun;

            LocationStruct.Height = SiteElev;
            LocationStruct.Latitude = SiteLat;
            LocationStruct.Longitude = SiteLong;
            LocationStruct.Pressure = 1000.0d;
            LocationStruct.Temperature = 10.0d;

            StarStruct.Dec = StarDecJ2000;
            StarStruct.RA = StarRAJ2000;
            StarStruct.Parallax = 2.0d;
            StarStruct.ProMoDec = 1.5d;
            StarStruct.ProMoRA = 2.5d;
            StarStruct.RadialVelocity = 3d;

            Astrometry.NOVAS.NOVAS2.StarVectors(StarStruct, ref POS, ref VEL);
            Astrometry.NOVAS.NOVAS2.Vector2RADec(POS, ref RATarget, ref DECTarget);
            CompareDouble("Novas2Tests", "J2000 RA Target", RATarget, StarRAJ2000, TOLERANCE_E9, DoubleType.Hours0To24);
            CompareDouble("Novas2Tests", "J2000 Dec Target", DECTarget, StarDecJ2000, TOLERANCE_E9, DoubleType.Degrees0To360);

            Astrometry.NOVAS.NOVAS2.Precession(J2000, POS, u.JulianDate, ref POSNow);
            Astrometry.NOVAS.NOVAS2.Vector2RADec(POSNow, ref RANow, ref DECNow);
            RC = Astrometry.NOVAS.NOVAS2.TopoStar(JD, ref EarthBody, 0d, ref StarStruct, ref LocationStruct, ref RANow, ref DECNow);
            Compare("Novas2Tests", "TopoStar RC", RC.ToString(), 0.ToString());
            CompareDouble("Novas2Tests", "Topo RA", RANow, 12.0098595883453d, TOLERANCE_E9, DoubleType.Hours0To24);
            CompareDouble("Novas2Tests", "Topo Dec", DECNow, 29.933637435611d, TOLERANCE_E9, DoubleType.Degrees0To360);

            Astrometry.NOVAS.NOVAS2.RADec2Vector(StarRAJ2000, StarDecJ2000, 10000000000.0d, ref POS);
            Astrometry.NOVAS.NOVAS2.Vector2RADec(POS, ref RATarget, ref DECTarget);
            CompareDouble("Novas2Tests", "RADec2Vector", RATarget, StarRAJ2000, TOLERANCE_E9, DoubleType.Hours0To24);
            CompareDouble("Novas2Tests", "RADec2Vector", DECTarget, StarDecJ2000, TOLERANCE_E9, DoubleType.Degrees0To360);

            CompareDouble("Novas2Tests", "JulianDate", Astrometry.NOVAS.NOVAS2.JulianDate(2010, 12, 30, 9.0d), TestJulianDate(), TOLERANCE_E9);

            RC = Astrometry.NOVAS.NOVAS2.AstroPlanet(JD, ref SunBody, ref EarthBody, ref RATarget, ref DECTarget, ref Distance);
            Compare("Novas2Tests", "AstroPlanet RC", RC.ToString(), 0.ToString());
            CompareDouble("Novas2Tests", "AstroPlanet RA", RATarget, 18.6090529142058d, TOLERANCE_E9, DoubleType.Hours0To24);
            CompareDouble("Novas2Tests", "AstroPlanet Dec", DECTarget, -23.172110257017d, TOLERANCE_E9, DoubleType.Degrees0To360);
            CompareDouble("Novas2Tests", "AstroPlanet List", Distance, 0.983376046291495d, TOLERANCE_E9);

            RC = Astrometry.NOVAS.NOVAS2.VirtualPlanet(JD, ref SunBody, ref EarthBody, ref RANow, ref DECNow, ref Distance);
            Compare("Novas2Tests", "VirtualPlanet RC", RC.ToString(), 0.ToString());
            CompareDouble("Novas2Tests", "VirtualPlanet RA", RANow, 18.6086339599669d, TOLERANCE_E9, DoubleType.Hours0To24);
            CompareDouble("Novas2Tests", "VirtualPlanet Dec", DECNow, -23.1724757087899d, TOLERANCE_E9, DoubleType.Degrees0To360);
            CompareDouble("Novas2Tests", "VirtualPlanet List", Distance, 0.983376046291495d, TOLERANCE_E9);

            RC = Astrometry.NOVAS.NOVAS2.AppPlanet(JD, ref SunBody, ref EarthBody, ref RANow, ref DECNow, ref Distance);
            Compare("Novas2Tests", "AppPlanet RC", RC.ToString(), 0.ToString());
            CompareDouble("Novas2Tests", "AppPlanet RA", RANow, 18.620097981585d, TOLERANCE_E9, DoubleType.Hours0To24);
            CompareDouble("Novas2Tests", "AppPlanet Dec", DECNow, -23.162343811122d, TOLERANCE_E9, DoubleType.Degrees0To360);
            CompareDouble("Novas2Tests", "AppPlanet List", Distance, 0.983376046291495d, TOLERANCE_E9);

            RC = Astrometry.NOVAS.NOVAS2.TopoPlanet(JD, ref SunBody, ref EarthBody, 0.0d, ref LocationStruct, ref RANow, ref DECNow, ref Distance);
            Compare("Novas2Tests", "TopoPlanet RC", RC.ToString(), 0.ToString());
            CompareDouble("Novas2Tests", "TopoPlanet RA", RANow, 18.6201822342814d, TOLERANCE_E9, DoubleType.Hours0To24);
            CompareDouble("Novas2Tests", "TopoPlanet Dec", DECNow, -23.1645247136453d, TOLERANCE_E9, DoubleType.Degrees0To360);
            CompareDouble("Novas2Tests", "TopoPlanet List", Distance, 0.983371860482251d, TOLERANCE_E9);
            TL.BlankLine();

            Astrometry.NOVAS.NOVAS2.Equ2Hor(JD, 0.0d, 0.0d, 0.0d, ref LocationStruct, StarRAJ2000, StarDecJ2000, RefractionOption.LocationRefraction, ref ZenithDistance, ref Azimuth, ref RANow, ref DECNow);
            TL.LogMessage("Novas2Tests", "Equ2Hor RA - " + u.HoursToHMS(RANow, ":", ":", "", 3) + "  DEC: " + u.DegreesToDMS(DECNow, ":", ":", "", 3));
            TL.LogMessage("Novas2Tests", RANow + " " + DECNow + " " + ZenithDistance + " " + Azimuth);

            Astrometry.NOVAS.NOVAS2.EarthTilt(u.JulianDate, ref MObl, ref TObl, ref Eq, ref DPsi, ref DEpsilon);
            TL.LogMessage("Novas2Tests", "EarthTilt - Equation of Equinoxes - " + Eq + " DPsi: " + DPsi + " DEps: " + DEpsilon);
            Astrometry.NOVAS.NOVAS2.CelPole(10.0d, 10.0d);
            Astrometry.NOVAS.NOVAS2.EarthTilt(u.JulianDate, ref MObl, ref TObl, ref Eq, ref DPsi, ref DEpsilon);
            TL.LogMessage("Novas2Tests", "CelPole - Set Equation of Equinoxes - " + Eq + " DPsi: " + DPsi + " DEps: " + DEpsilon);
            Astrometry.NOVAS.NOVAS2.CelPole(0.0d, 0.0d);
            Astrometry.NOVAS.NOVAS2.EarthTilt(u.JulianDate, ref MObl, ref TObl, ref Eq, ref DPsi, ref DEpsilon);
            TL.LogMessage("Novas2Tests", "CelPole - Unset Equation of Equinoxes - " + Eq + " DPsi: " + DPsi + " DEps: " + DEpsilon);

            Astrometry.NOVAS.NOVAS2.SiderealTime(u.JulianDate, 0.0d, Eq, ref GreenwichSiderealTime);
            TL.LogMessage("Novas2Tests", "SiderealTime - " + u.HoursToHMS(GreenwichSiderealTime));

            Astrometry.NOVAS.NOVAS2.Ephemeris(u.JulianDate, ref EarthBody, Origin.Heliocentric, ref POS, ref VEL);
            Fmt = "0.000000000000";
            TL.LogMessage("Novas2Tests", "Ephemeris Ea - " + Strings.Format(POS[0], Fmt) + " " + Strings.Format(POS[1], Fmt) + " " + Strings.Format(POS[2], Fmt) + " " + Strings.Format(VEL[0], Fmt) + " " + Strings.Format(VEL[1], Fmt) + " " + Strings.Format(VEL[2], Fmt));

            Astrometry.NOVAS.NOVAS2.Ephemeris(u.JulianDate, ref SunBody, Origin.Heliocentric, ref POS, ref VEL);
            TL.LogMessage("Novas2Tests", "Ephemeris Pl - " + Strings.Format(POS[0], Fmt) + " " + Strings.Format(POS[1], Fmt) + " " + Strings.Format(POS[2], Fmt) + " " + Strings.Format(VEL[0], Fmt) + " " + Strings.Format(VEL[1], Fmt) + " " + Strings.Format(VEL[2], Fmt));

            Astrometry.NOVAS.NOVAS2.SolarSystem(u.JulianDate, Body.Earth, Origin.Heliocentric, ref POS, ref VEL);
            TL.LogMessage("Novas2Tests", "SolarSystem Ea - " + Strings.Format(POS[0], Fmt) + " " + Strings.Format(POS[1], Fmt) + " " + Strings.Format(POS[2], Fmt) + " " + Strings.Format(VEL[0], Fmt) + " " + Strings.Format(VEL[1], Fmt) + " " + Strings.Format(VEL[2], Fmt));

            RC = Astrometry.NOVAS.NOVAS2.Vector2RADec(POS, ref RANow, ref DECNow);
            Compare("Novas2Tests", "Vector2RADec RC", RC.ToString(), 0.ToString());
            TL.LogMessage("Novas2Tests", "Vector2RADec RA - " + u.HoursToHMS(RANow, ":", ":", "", 3) + "  DEC: " + u.DegreesToDMS(DECNow, ":", ":", "", 3));

            Astrometry.NOVAS.NOVAS2.StarVectors(StarStruct, ref POS, ref VEL);

            RC = Astrometry.NOVAS.NOVAS2.Vector2RADec(POS, ref RANow, ref DECNow);
            Compare("Novas2Tests", "Vector2RADec RC", RC.ToString(), 0.ToString());
            TL.LogMessage("Novas2Tests", "StarVectors - " + Strings.Format(POS[0], Fmt) + " " + Strings.Format(POS[1], Fmt) + " " + Strings.Format(POS[2], Fmt) + " " + Strings.Format(VEL[0], Fmt) + " " + Strings.Format(VEL[1], Fmt) + " " + Strings.Format(VEL[2], Fmt));
            TL.LogMessage("Novas2Tests", "StarVectors RA - " + u.HoursToHMS(RANow, ":", ":", "", 3) + "  DEC: " + u.DegreesToDMS(DECNow, ":", ":", "", 3));

            Astrometry.NOVAS.NOVAS2.RADec2Vector(12.0d, 30.0d, 1000d, ref POS);
            TL.LogMessage("Novas2Tests", "RADec2Vector - " + Strings.Format(POS[0], Fmt) + " " + Strings.Format(POS[1], Fmt) + " " + Strings.Format(POS[2], Fmt));

            RC = Astrometry.NOVAS.NOVAS2.GetEarth(u.JulianDate, ref EarthBody, ref Tdb, ref POS, ref VEL, ref POS2, ref VEL2);
            Compare("Novas2Tests", "GetEarth RC", RC.ToString(), 0.ToString());
            TL.LogMessage("Novas2Tests", "GetEarth TDB - " + Tdb);
            TL.LogMessage("Novas2Tests", "GetEarth - " + Strings.Format(POS[0], Fmt) + " " + Strings.Format(POS[1], Fmt) + " " + Strings.Format(POS[2], Fmt) + " " + Strings.Format(VEL[0], Fmt) + " " + Strings.Format(VEL[1], Fmt) + " " + Strings.Format(VEL[2], Fmt));
            TL.LogMessage("Novas2Tests", "GetEarth - " + Strings.Format(POS2[0], Fmt) + " " + Strings.Format(POS2[1], Fmt) + " " + Strings.Format(POS2[2], Fmt) + " " + Strings.Format(VEL2[0], Fmt) + " " + Strings.Format(VEL2[1], Fmt) + " " + Strings.Format(VEL2[2], Fmt));

            RC = Astrometry.NOVAS.NOVAS2.MeanStar(u.JulianDate, ref EarthBody, 12.0d, 30.0d, ref RANow, ref DECNow);
            Compare("Novas2Tests", "MeanStar RC", RC.ToString(), 0.ToString());
            TL.LogMessage("Novas2Tests", "MeanStar RA -  " + u.HoursToHMS(RANow, ":", ":", "", 3) + "  DEC: " + u.DegreesToDMS(DECNow, ":", ":", "", 3));

            Astrometry.NOVAS.NOVAS2.StarVectors(StarStruct, ref POS, ref VEL);
            TL.LogMessage("Novas2Tests", "Pnsw In - " + Strings.Format(POS[0], Fmt) + " " + Strings.Format(POS[1], Fmt) + " " + Strings.Format(POS[2], Fmt));

            Astrometry.NOVAS.NOVAS2.Pnsw(u.JulianDate, 15.0d, 2.5d, 5d, POS, ref POSNow);
            TL.LogMessage("Novas2Tests", "Pnsw Out" + Strings.Format(POSNow[0], Fmt) + " " + Strings.Format(POSNow[1], Fmt) + " " + Strings.Format(POSNow[2], Fmt));

            Astrometry.NOVAS.NOVAS2.Spin(u.JulianDate, POS, ref POSNow);
            TL.LogMessage("Novas2Tests", "Spin - " + Strings.Format(POSNow[0], Fmt) + " " + Strings.Format(POSNow[1], Fmt) + " " + Strings.Format(POSNow[2], Fmt));

            Astrometry.NOVAS.NOVAS2.Wobble(2.5d, 5.0d, POS, ref POSNow);
            TL.LogMessage("Novas2Tests", "Wobble - " + Strings.Format(POSNow[0], Fmt) + " " + Strings.Format(POSNow[1], Fmt) + " " + Strings.Format(POSNow[2], Fmt));

            Astrometry.NOVAS.NOVAS2.Terra(ref LocationStruct, 15.0d, ref POS, ref VEL);
            TL.LogMessage("Novas2Tests", "Terra - " + Strings.Format(POS[0], Fmt) + " " + Strings.Format(POS[1], Fmt) + " " + Strings.Format(POS[2], Fmt) + " " + Strings.Format(VEL[0], Fmt) + " " + Strings.Format(VEL[1], Fmt) + " " + Strings.Format(VEL[2], Fmt));

            Astrometry.NOVAS.NOVAS2.ProperMotion(J2000, POS, VEL, u.JulianDate, ref POSNow);
            TL.LogMessage("Novas2Tests", "ProperMotion - " + Strings.Format(POSNow[0], Fmt) + " " + Strings.Format(POSNow[1], Fmt) + " " + Strings.Format(POSNow[2], Fmt));

            RC = Astrometry.NOVAS.NOVAS2.GetEarth(u.JulianDate, ref EarthBody, ref Tdb, ref POSEarth, ref VEL, ref POS2, ref VEL2);
            Compare("Novas2Tests", "GetEarth RC", RC.ToString(), 0.ToString());
            Astrometry.NOVAS.NOVAS2.BaryToGeo(POS, POSEarth, ref POSNow, ref LightTime);
            TL.LogMessage("Novas2Tests", "BaryToGeo - " + Strings.Format(POSNow[0], Fmt) + " " + Strings.Format(POSNow[1], Fmt) + " " + Strings.Format(POSNow[2], Fmt) + " LightTime: " + LightTime);

            RC = Astrometry.NOVAS.NOVAS2.SunField(POS, POSEarth, ref POSNow);
            Compare("Novas2Tests", "SunField RC", RC.ToString(), 0.ToString());
            TL.LogMessage("Novas2Tests", "SunField - " + Strings.Format(POSNow[0], Fmt) + " " + Strings.Format(POSNow[1], Fmt) + " " + Strings.Format(POSNow[2], Fmt));

            RC = Astrometry.NOVAS.NOVAS2.Aberration(POS, VEL, LightTime, ref POSNow);
            Compare("Novas2Tests", "Aberration RC", RC.ToString(), 0.ToString());
            TL.LogMessage("Novas2Tests", "Aberration - " + Strings.Format(POSNow[0], Fmt) + " " + Strings.Format(POSNow[1], Fmt) + " " + Strings.Format(POSNow[2], Fmt));

            RC = Astrometry.NOVAS.NOVAS2.Nutate(u.JulianDate, NutationDirection.MeanToTrue, POS, ref POSNow);
            Compare("Novas2Tests", "Nutate RC", RC.ToString(), 0.ToString());
            TL.LogMessage("Novas2Tests", "Nutate - " + Strings.Format(POSNow[0], Fmt) + " " + Strings.Format(POSNow[1], Fmt) + " " + Strings.Format(POSNow[2], Fmt));

            RC = Astrometry.NOVAS.NOVAS2.NutationAngles(1.0d, ref LongNutation, ref ObliqNutation);
            Compare("Novas2Tests", "NutationAngles RC", RC.ToString(), 0.ToString());
            TL.LogMessage("Novas2Tests", "NutationAngles - Long Nutation: " + LongNutation + " Oblique Nutation: " + ObliqNutation);

            Astrometry.NOVAS.NOVAS2.FundArgs(1.0d, ref FundArgsValue);
            TL.LogMessage("Novas2Tests", "FundArgs - " + FundArgsValue[0] + " " + FundArgsValue[1] + " " + FundArgsValue[2] + " " + FundArgsValue[3] + " " + FundArgsValue[4]);

            Astrometry.NOVAS.NOVAS2.Tdb2Tdt(u.JulianDate, ref TdtJd, ref SecDiff);
            TL.LogMessage("Novas2Tests", "Tdb2Tdt - TDT JD: " + TdtJd + " Sec Diff: " + SecDiff);

            Astrometry.NOVAS.NOVAS2.SetBody(BodyType.MajorPlanet, Body.Earth, "Earth", ref SunBody);
            TL.LogMessage("Novas2Tests", "SetBody - Name: " + SunBody.Name + " Number: " + ((int)SunBody.Number).ToString() + " Type: " + ((int)SunBody.Type).ToString());

            Astrometry.NOVAS.NOVAS2.MakeCatEntry("HIP", "PStar", 23045, 15.0d, 30.0d, 1d, 2d, 3d, 4d, ref StarStruct);
            TL.LogMessage("Novas2Tests", "MakeCatEntry - Cat: " + StarStruct.Catalog + " Name: " + StarStruct.StarName + " Number: " + StarStruct.StarNumber + " " + StarStruct.RA + " " + StarStruct.Dec + " " + StarStruct.ProMoRA + " " + StarStruct.ProMoDec + " " + StarStruct.Parallax + " " + StarStruct.RadialVelocity);

            ZenithDistance = Astrometry.NOVAS.NOVAS2.Refract(ref LocationStruct, 2, 65.0d);
            TL.LogMessage("Novas2Tests", "Refract (65deg) - Refracted Offset (Degrees): " + u.DegreesToDMS(ZenithDistance, ":", ":"));

            JD = Astrometry.NOVAS.NOVAS2.JulianDate(2009, 6, 7, 12.0d);
            TL.LogMessage("Novas2Tests", "JulianDate - JD (6/7/2009 12:00): " + JD);

            Astrometry.NOVAS.NOVAS2.CalDate(JD, ref Year, ref Month, ref day, ref Hour);
            TL.LogMessage("Novas2Tests", "CalDate - " + day + " " + Month + " " + Year + " " + u.HoursToHMS(Hour));
            TL.BlankLine();

            Status("Novas2Static Tests");
            NOVAS2StaticTest(NOVAS2Functions.Abberation);
            NOVAS2StaticTest(NOVAS2Functions.App_Planet);
            NOVAS2StaticTest(NOVAS2Functions.App_Star);
            NOVAS2StaticTest(NOVAS2Functions.Astro_Planet);
            NOVAS2StaticTest(NOVAS2Functions.Astro_Star);
            NOVAS2StaticTest(NOVAS2Functions.Bary_To_Geo);
            NOVAS2StaticTest(NOVAS2Functions.Cal_Date);
            NOVAS2StaticTest(NOVAS2Functions.Cel_Pole);
            NOVAS2StaticTest(NOVAS2Functions.EarthTilt);
            NOVAS2StaticTest(NOVAS2Functions.Ephemeris);
            NOVAS2StaticTest(NOVAS2Functions.Equ2Hor);
            NOVAS2StaticTest(NOVAS2Functions.Fund_Args);
            NOVAS2StaticTest(NOVAS2Functions.Get_Earth);
            NOVAS2StaticTest(NOVAS2Functions.Julian_Date);
            NOVAS2StaticTest(NOVAS2Functions.Local_Planet);
            NOVAS2StaticTest(NOVAS2Functions.Local_Star);
            NOVAS2StaticTest(NOVAS2Functions.Make_Cat_Entry);
            NOVAS2StaticTest(NOVAS2Functions.Mean_Star);
            NOVAS2StaticTest(NOVAS2Functions.Nutate);
            NOVAS2StaticTest(NOVAS2Functions.Nutation_Angles);
            NOVAS2StaticTest(NOVAS2Functions.PNSW);
            NOVAS2StaticTest(NOVAS2Functions.Precession);
            NOVAS2StaticTest(NOVAS2Functions.Proper_Motion);
            NOVAS2StaticTest(NOVAS2Functions.RADEC2Vector);
            NOVAS2StaticTest(NOVAS2Functions.Refract);
            NOVAS2StaticTest(NOVAS2Functions.Set_Body);
            NOVAS2StaticTest(NOVAS2Functions.Sideral_Time);
            NOVAS2StaticTest(NOVAS2Functions.SolarSystem);
            NOVAS2StaticTest(NOVAS2Functions.Spin);
            NOVAS2StaticTest(NOVAS2Functions.StarVectors);
            NOVAS2StaticTest(NOVAS2Functions.Sun_Field);
            NOVAS2StaticTest(NOVAS2Functions.Tdb2Tdt);
            NOVAS2StaticTest(NOVAS2Functions.Terra);
            NOVAS2StaticTest(NOVAS2Functions.Topo_Planet);
            NOVAS2StaticTest(NOVAS2Functions.Topo_Star);
            NOVAS2StaticTest(NOVAS2Functions.Transform_Cat);
            NOVAS2StaticTest(NOVAS2Functions.Transform_Hip);
            NOVAS2StaticTest(NOVAS2Functions.Vector2RADEC);
            NOVAS2StaticTest(NOVAS2Functions.Virtual_Planet);
            NOVAS2StaticTest(NOVAS2Functions.Virtual_Star);
            NOVAS2StaticTest(NOVAS2Functions.Wobble);
            TL.BlankLine();
        }

        private void CheckRC(short RC, string Section, string Name)
        {
            Compare(Section, Name + " Return Code", RC.ToString(), "0");
        }

        private void NOVAS2StaticTest(NOVAS2Functions TestFunction)
        {
            double RA = default, DEC = default, Dis = default, LightTime = default, Hour = default, MObl = default, TObl = default, Eq = default, DPsi = default, DEps = default;
            double[] POS = new double[3], VEL = new double[3], POS2 = new double[3], VEL2 = new double[3], EarthVector = new double[3];
            double RadVel = default, JD, TDB = default, DeltaT = default, x = default, y = default, ZD = default, Az = default, rar = default, decr = default;
            double[] a = new double[5];
            var SiteInfo = new SiteInfo();
            double Gast = default, MRA = default, MDEC = default, LongNutation = default, ObliqueNutation = default, TdtJD = default, SecDiff = default, ST = default;
            short Year = default, Month = default, Day = default;
            var CatName = new byte[3];
            int rc;
            BodyDescription SS_Object = new(), Earth = new();
            CatEntry Star = new(), NewCat = new();
            var Utl = new Util();
            Action(TestFunction.ToString());
            Earth.Number = Body.Earth;
            Earth.Name = "Earth";
            Earth.Type = BodyType.MajorPlanet;

            SS_Object.Name = "Sun";
            SS_Object.Number = Body.Sun;
            SS_Object.Type = BodyType.Sun;

            Star.Dec = 40.0d;
            Star.RA = 23d;
            Star.StarName = "Test Star";
            Star.Catalog = "HIP";

            SiteInfo.Height = 80.0d;
            SiteInfo.Latitude = 51.0d;
            SiteInfo.Longitude = 0.0d;
            SiteInfo.Pressure = 1020d;
            SiteInfo.Temperature = 25.0d;

            CatName[0] = (byte)Strings.Asc("P");
            CatName[1] = (byte)Strings.Asc("S");
            CatName[2] = (byte)Strings.Asc("1");

            rc = int.MaxValue; // Initialise to a silly value

            try
            {
                switch (TestFunction)
                {
                    case NOVAS2Functions.Abberation:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.Aberration(POS, VEL, LightTime, ref POS2);
                            break;
                        }
                    case NOVAS2Functions.App_Planet:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.AppPlanet(Utl.JulianDate, ref SS_Object, ref Earth, ref RA, ref DEC, ref Dis);
                            break;
                        }
                    case NOVAS2Functions.App_Star:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.AppStar(Utl.JulianDate, ref Earth, ref Star, ref RA, ref DEC);
                            break;
                        }
                    case NOVAS2Functions.Astro_Planet:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.AstroPlanet(Utl.JulianDate, ref SS_Object, ref Earth, ref RA, ref DEC, ref Dis);
                            break;
                        }
                    case NOVAS2Functions.Astro_Star:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.AstroStar(Utl.JulianDate, ref Earth, ref Star, ref RA, ref DEC);
                            break;
                        }
                    case NOVAS2Functions.Bary_To_Geo:
                        {
                            Astrometry.NOVAS.NOVAS2.BaryToGeo(POS, EarthVector, ref POS2, ref LightTime);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Cal_Date:
                        {
                            Astrometry.NOVAS.NOVAS2.CalDate(Utl.JulianDate, ref Year, ref Month, ref Day, ref Hour);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Cel_Pole:
                        {
                            Astrometry.NOVAS.NOVAS2.CelPole(0.0d, 0.0d);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.EarthTilt:
                        {
                            Astrometry.NOVAS.NOVAS2.EarthTilt(Utl.JulianDate, ref MObl, ref TObl, ref Eq, ref DPsi, ref DEps);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Ephemeris:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.Ephemeris(Utl.JulianDate, ref SS_Object, Origin.Barycentric, ref POS, ref VEL);
                            break;
                        }
                    case NOVAS2Functions.Equ2Hor:
                        {
                            Astrometry.NOVAS.NOVAS2.Equ2Hor(Utl.JulianDate, DeltaT, x, y, ref SiteInfo, RA, DEC, RefractionOption.LocationRefraction, ref ZD, ref Az, ref rar, ref decr);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Fund_Args:
                        {
                            Astrometry.NOVAS.NOVAS2.FundArgs(0.1d, ref a);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Get_Earth:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.GetEarth(Utl.JulianDate, ref Earth, ref TDB, ref POS, ref VEL, ref POS2, ref VEL2);
                            break;
                        }
                    case NOVAS2Functions.Julian_Date:
                        {
                            JD = Astrometry.NOVAS.NOVAS2.JulianDate(2010, 9, 4, 5d);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Local_Planet:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.LocalPlanet(Utl.JulianDate, ref SS_Object, ref Earth, 0.0d, ref SiteInfo, ref RA, ref DEC, ref Dis);
                            break;
                        }
                    case NOVAS2Functions.Local_Star:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.LocalStar(Utl.JulianDate, ref Earth, 0.0d, ref Star, ref SiteInfo, ref RA, ref DEC);
                            break;
                        }
                    case NOVAS2Functions.Make_Cat_Entry:
                        {
                            Astrometry.NOVAS.NOVAS2.MakeCatEntry("ABC", "TestStarName", 12345, 7.0d, 65.0d, 0.0d, 0.0d, 0.0d, RadVel, ref Star);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Mean_Star:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.MeanStar(Utl.JulianDate, ref Earth, RA, DEC, ref MRA, ref MDEC);
                            break;
                        }
                    case NOVAS2Functions.Nutate:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.Nutate(Utl.JulianDate, NutationDirection.MeanToTrue, POS, ref POS2);
                            break;
                        }
                    case NOVAS2Functions.Nutation_Angles:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.NutationAngles(TDB, ref LongNutation, ref ObliqueNutation);
                            break;
                        }
                    case NOVAS2Functions.PNSW:
                        {
                            Astrometry.NOVAS.NOVAS2.Pnsw(Utl.JulianDate, Gast, x, y, POS, ref POS2);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Precession:
                        {
                            Astrometry.NOVAS.NOVAS2.Precession(Utl.JulianDate, POS, Utl.JulianDate + 100.0d, ref POS2);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Proper_Motion:
                        {
                            Astrometry.NOVAS.NOVAS2.ProperMotion(Utl.JulianDate, POS, VEL, Utl.JulianDate + 100.0d, ref POS);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.RADEC2Vector:
                        {
                            Astrometry.NOVAS.NOVAS2.RADec2Vector(RA, DEC, Dis, ref POS);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Refract:
                        {
                            ZD = Astrometry.NOVAS.NOVAS2.Refract(ref SiteInfo, 0, 75.0d);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Set_Body:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.SetBody(BodyType.MajorPlanet, Body.Moon, "Moon", ref SS_Object);
                            break;
                        }
                    case NOVAS2Functions.Sideral_Time:
                        {
                            Astrometry.NOVAS.NOVAS2.SiderealTime(Utl.JulianDate, 0.123d, 65.0d, ref Gast);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.SolarSystem:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.SolarSystem(Utl.JulianDate, Body.Earth, Origin.Barycentric, ref POS, ref VEL);
                            break;
                        }
                    case NOVAS2Functions.Spin:
                        {
                            Astrometry.NOVAS.NOVAS2.Spin(12.0d, POS, ref POS2);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.StarVectors:
                        {
                            Astrometry.NOVAS.NOVAS2.StarVectors(Star, ref POS, ref VEL);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Sun_Field:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.SunField(POS, POS, ref POS2);
                            break;
                        }
                    case NOVAS2Functions.Tdb2Tdt:
                        {
                            Astrometry.NOVAS.NOVAS2.Tdb2Tdt(TDB, ref TdtJD, ref SecDiff);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Terra:
                        {
                            Astrometry.NOVAS.NOVAS2.Terra(ref SiteInfo, ST, ref POS, ref VEL);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Topo_Planet:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.TopoPlanet(Utl.JulianDate, ref SS_Object, ref Earth, 0.0d, ref SiteInfo, ref RA, ref DEC, ref Dis);
                            break;
                        }
                    case NOVAS2Functions.Topo_Star:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.TopoStar(Utl.JulianDate, ref Earth, DeltaT, ref Star, ref SiteInfo, ref RA, ref DEC);
                            break;
                        }
                    case NOVAS2Functions.Transform_Cat:
                        {
                            Astrometry.NOVAS.NOVAS2.TransformCat(TransformationOption.ChangeEquatorAndEquinoxAndEpoch, Utl.JulianDate, ref Star, Utl.JulianDate + 1000d, ref CatName, ref NewCat);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Transform_Hip:
                        {
                            Astrometry.NOVAS.NOVAS2.TransformHip(ref Star, ref NewCat);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Vector2RADEC:
                        {
                            Astrometry.NOVAS.NOVAS2.RADec2Vector(23.0d, 50.0d, 100000000d, ref POS);
                            rc = Astrometry.NOVAS.NOVAS2.Vector2RADec(POS, ref RA, ref DEC);
                            break;
                        }
                    case NOVAS2Functions.Virtual_Planet:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.VirtualPlanet(Utl.JulianDate, ref SS_Object, ref Earth, ref RA, ref DEC, ref Dis);
                            break;
                        }
                    case NOVAS2Functions.Virtual_Star:
                        {
                            rc = Astrometry.NOVAS.NOVAS2.VirtualStar(Utl.JulianDate, ref Earth, ref Star, ref RA, ref DEC);
                            break;
                        }
                    case NOVAS2Functions.Wobble:
                        {
                            Astrometry.NOVAS.NOVAS2.Wobble(x, y, POS, ref POS2);
                            rc = 0;
                            break;
                        }
                    case NOVAS2Functions.Sun_Eph:
                        {
                            // Not in DLL  from Tim
                            Astrometry.NOVAS.NOVAS2.SunEph(Utl.JulianDate, ref RA, ref DEC, ref Dis);
                            rc = 0;
                            break;
                        }
                }
                switch (rc)
                {
                    case 0:
                        {
                            TL.LogMessage("Novas2Static", TestFunction.ToString() + " - Passed");
                            NMatches += 1;
                            break;
                        }
                    case int.MaxValue:
                        {
                            TL.LogMessage("Novas2Static", TestFunction.ToString() + " - Test not implemented");
                            break;
                        }

                    default:
                        {
                            string Msg = "Non zero return code: " + rc;
                            TL.LogMessage("Novas2Static", TestFunction.ToString() + " - " + Msg);
                            NNonMatches += 1;
                            ErrorList.Add("Novas2Static - " + TestFunction.ToString() + ": " + Msg);
                            break;
                        }
                }

                Utl.Dispose();
                Utl = null;
            }
            catch (Exception ex)
            {
                LogException("NOVAS2StaticTest", ex.ToString());
            }
        }

        private void TransformTest()
        {
            // Confirm that site property read before write generates an error
            TransformInitalGetTest(transform, TransformExceptionTestType.SiteLatitude);
            TransformInitalGetTest(transform, TransformExceptionTestType.SiteLongitude);
            TransformInitalGetTest(transform, TransformExceptionTestType.SiteElevation);
            // Cannot test SiteTemperature because a coding omission in Transform caused it never to be forced to be set. Now it cannot be forced to be set because it could break existing applications. :(
            // TransformInitalGetTest(transform, TransformExceptionTestType.SiteTemperature)
            TransformInitalGetTest(transform, TransformExceptionTestType.SitePressure);

            // Set parameters ready for transformation
            transform.SiteTemperature = 20.0d;
            transform.SiteElevation = 1500d;
            transform.SiteLatitude = 0.0d;
            transform.SiteLongitude = 0.0d;
            transform.SetJ2000(0.0d, 0.0d); // Set coordinates 0.0,0.0

            // Transform acceptable parameter range tests
            TransformExceptionTest(transform, TransformExceptionTestType.SiteLatitude, 0.0d, -91.0d, 91.0d);
            TransformExceptionTest(transform, TransformExceptionTestType.SiteLongitude, 0.0d, -181.0d, 181.0d);
            TransformExceptionTest(transform, TransformExceptionTestType.SiteElevation, 0.0d, -301.0d, 10001.0d);
            TransformExceptionTest(transform, TransformExceptionTestType.SiteTemperature, 0.0d, -275.0d, 101.0d);
            TransformExceptionTest(transform, TransformExceptionTestType.SitePressure, 1013.25d, -0.1d, 1200.1d);
            TransformExceptionTest(transform, TransformExceptionTestType.JulianDateTT, 2451545.0d, -1.0d, 6000000.0d);
            TransformExceptionTest(transform, TransformExceptionTestType.JulianDateTT, 0.0d, -1.0d, 6000000.0d); // Confirm that the special JulianDateTT value of 0.0 passes
            TransformExceptionTest(transform, TransformExceptionTestType.JulianDateUTC, 2451545.0d, -1.0d, 6000000.0d);
            TransformExceptionTest(transform, TransformExceptionTestType.JulianDateUTC, 0.0d, -1.0d, 6000000.0d); // Confirm that the special JulianDateUTC value of 0.0 passes
            TransformExceptionTest(transform, TransformExceptionTestType.SetJ2000RA, 0.0d, -1.0d, 24.0d);
            TransformExceptionTest(transform, TransformExceptionTestType.SetJ2000Dec, 0.0d, -91.0d, 91.0d);
            TransformExceptionTest(transform, TransformExceptionTestType.SetApparentRA, 0.0d, -1.0d, 24.0d);
            TransformExceptionTest(transform, TransformExceptionTestType.SetApparentDec, 0.0d, -91.0d, 91.0d);
            TransformExceptionTest(transform, TransformExceptionTestType.SetTopocentricRA, 0.0d, -1.0d, 24.0d);
            TransformExceptionTest(transform, TransformExceptionTestType.SetTopocentricDec, 0.0d, -91.0d, 91.0d);
            TransformExceptionTest(transform, TransformExceptionTestType.SetAzElAzimuth, 0.0d, -1.0d, 360.0d);
            TransformExceptionTest(transform, TransformExceptionTestType.SetAzElElevation, 0.0d, -91.0d, 91.0d);

            TransformTest2000("Deneb", "20:41:25.916", "45:16:49.23", TOLERANCE_E5, TOLERANCE_E4);
            TransformTest2000("Polaris", "02:31:51.263", "89:15:50.68", TOLERANCE_E5, TOLERANCE_E4);
            TransformTest2000("Arcturus", "14:15:38.943", "19:10:37.93", TOLERANCE_E5, TOLERANCE_E4);

            // Confirm that Transform works if set with J2000 coordinates but refraction correction is off and that it throws an exception if refraction correction is on
            Astrometry.Transform.Transform tr;

            try
            {
                tr = new Astrometry.Transform.Transform();
                tr.SiteElevation = 1500d;
                tr.SiteLatitude = 0.0d;
                tr.SiteLongitude = 0.0d;
                tr.SetJ2000(0.0d, 0.0d);
                tr.Refraction = false;

                // Confirm that Transform operates correctly when J2000 coordinates are set but refraction correction is disabled and site temperature is not set
                try
                {
                    double rightAscension;
                    rightAscension = tr.RATopocentric;
                    TL.LogMessage("TransformTest", "Transform works correctly when site temperature has not been set and J2000 coordinates are set.");
                    NMatches += 1;
                }
                catch (TransformUninitialisedException)
                {
                    LogError("TransformTest", "Received a TransformUninitialisedException when this operation should have worked!");
                }
                catch (Exception ex)
                {
                    LogException("TransformTest1", ex.ToString());
                }

                // Confirm that Transform throws an exception when J2000 coordinates are set and refraction correction is enabled but site temperature is not set
                // Try
                // Dim rightAscension As Double
                // tr.Refraction = True
                // rightAscension = tr.RATopocentric
                // LogError("TransformTest", "Did not receive a TransformUninitialisedException when J2000 coordinates are set and refraction is enabled.")
                // Catch ex As TransformUninitialisedException
                // TL.LogMessage("TransformTest", "Transform threw a TransformUninitialisedException when refraction correction is enabled and J2000 coordinates are set but site temperature has not been set.")
                // NMatches += 1
                // Catch ex As Exception
                // LogException("TransformTest1", ex.ToString())
                // End Try
                tr.Dispose();
            }
            catch (Exception ex)
            {
                LogException("TransformTest", ex.ToString());
            }

            TL.BlankLine();
        }

        private void TransformInitalGetTest(Astrometry.Transform.Transform TR, TransformExceptionTestType test)
        {
            double value;

            try
            {
                switch (test)
                {
                    case TransformExceptionTestType.SiteLatitude:
                        {
                            value = TR.SiteLatitude;
                            break;
                        }
                    case TransformExceptionTestType.SiteLongitude:
                        {
                            value = TR.SiteLongitude;
                            break;
                        }
                    case TransformExceptionTestType.SiteElevation:
                        {
                            value = TR.SiteElevation;
                            break;
                        }
                    case TransformExceptionTestType.SiteTemperature:
                        {
                            value = TR.SiteTemperature;
                            break;
                        }
                    case TransformExceptionTestType.SitePressure:
                        {
                            value = TR.SitePressure;
                            break;
                        }
                }
                LogError("TransformInitalGetTest", $"Initial read of Transform.{test} did not raise a TransformUninitialisedException as expected.");
            }
            catch (TransformUninitialisedException)
            {
                TL.LogMessage("TransformInitalGetTest", $"A TransformUninitialisedException was generated as expected when reading Transform{test} for the first time.");
                NMatches += 1;
            }
            catch (Exception ex)
            {
                LogError("TransformInitalGetTest", $"Unexpected exception when reading Transform.{test} for the first time: {ex}");
            }

        }

        private void TransformExceptionTest(Astrometry.Transform.Transform TR, TransformExceptionTestType test, double InRange, double OutofRangeLow, double OutofRangeHigh)
        {
            TransformExceptionTester(TR, test, InRange, true);
            TransformExceptionTester(TR, test, OutofRangeLow, false);
            TransformExceptionTester(TR, test, OutofRangeHigh, false);
        }

        private void TransformExceptionTester(Astrometry.Transform.Transform TR, TransformExceptionTestType test, double Value, bool ExpectedPass)
        {
            try
            {
                switch (test)
                {
                    case TransformExceptionTestType.JulianDateTT:
                        {
                            TR.JulianDateTT = Value;
                            break;
                        }
                    case TransformExceptionTestType.JulianDateUTC:
                        {
                            TR.JulianDateUTC = Value;
                            break;
                        }
                    case TransformExceptionTestType.SiteLatitude:
                        {
                            TR.SiteLatitude = Value;
                            break;
                        }
                    case TransformExceptionTestType.SiteLongitude:
                        {
                            TR.SiteLongitude = Value;
                            break;
                        }
                    case TransformExceptionTestType.SiteElevation:
                        {
                            TR.SiteElevation = Value;
                            break;
                        }
                    case TransformExceptionTestType.SiteTemperature:
                        {
                            TR.SiteTemperature = Value;
                            break;
                        }
                    case TransformExceptionTestType.SitePressure:
                        {
                            TR.SitePressure = Value;
                            break;
                        }
                    case TransformExceptionTestType.SetJ2000RA:
                        {
                            TR.SetJ2000(Value, 0.0d);
                            break;
                        }
                    case TransformExceptionTestType.SetJ2000Dec:
                        {
                            TR.SetJ2000(0.0d, Value);
                            break;
                        }
                    case TransformExceptionTestType.SetApparentRA:
                        {
                            TR.SetApparent(Value, 0.0d);
                            break;
                        }
                    case TransformExceptionTestType.SetApparentDec:
                        {
                            TR.SetApparent(0.0d, Value);
                            break;
                        }
                    case TransformExceptionTestType.SetTopocentricRA:
                        {
                            TR.SetTopocentric(Value, 0.0d);
                            break;
                        }
                    case TransformExceptionTestType.SetTopocentricDec:
                        {
                            TR.SetTopocentric(0.0d, Value);
                            break;
                        }
                    case TransformExceptionTestType.SetAzElAzimuth:
                        {
                            TR.SetAzimuthElevation(Value, 45.0d);
                            break;
                        }
                    case TransformExceptionTestType.SetAzElElevation:
                        {
                            TR.SetAzimuthElevation(45.0d, Value);
                            break;
                        }
                }
                if (ExpectedPass)
                {
                    TL.LogMessage("TransformExceptionTester", string.Format("Test {0} with valid value {1} passed as expected", test.ToString(), Value));
                    NMatches += 1;
                }
                else
                {
                    LogError("TransformExceptionTester", string.Format("No exception generated on invalid {0} value of {1}", test.ToString(), Value));
                }
            }
            catch (Exception)
            {
                if (!ExpectedPass)
                {
                    TL.LogMessage("TransformExceptionTester", string.Format("Exception generated as expected for {0} invalid value of {1}", test.ToString(), Value));
                    NMatches += 1;
                }
                else
                {
                    LogError("TransformExceptionTester", string.Format("Unexpected exception generated on valid {0} value of {1}", test.ToString(), Value));
                }
            }

        }

        private void TransformTest2000(string Name, string AstroRAString, string AstroDECString, double RATolerance, double DecTolerance)
        {
            var Util = new Util();
            double AstroRA, AstroDEC;
            double SiteLat, SiteLong, SiteElev;
            // Dim transform As Transform.Transform = New Transform.Transform
            double RA = default, DEC = default;
            short rc;
            var OnSurface3 = new OnSurface();
            var Cat3 = new CatEntry3();

            try
            {
                // RA and DEC
                AstroRA = Util.HMSToHours(AstroRAString);
                AstroDEC = Util.DMSToDegrees(AstroDECString);

                // Site parameters
                SiteElev = 80.0d;
                SiteLat = 51.0d + 4.0d / 60.0d + 43.0d / 3600.0d;
                SiteLong = 0.0d - 17.0d / 60.0d - 40.0d / 3600.0d;

                // Set up Transform component
                transform.SiteElevation = 1580.0d;
                transform.SiteLatitude = SiteLat;
                transform.SiteLongitude = SiteLong;
                transform.SiteTemperature = 10.0d;
                transform.Refraction = false;
                transform.SetJ2000(AstroRA, AstroDEC);

                Cat3.RA = AstroRA;
                Cat3.Dec = AstroDEC;
                OnSurface3.Height = SiteElev;
                OnSurface3.Latitude = SiteLat;
                OnSurface3.Longitude = SiteLong;
                OnSurface3.Pressure = 1000.0d;
                OnSurface3.Temperature = 10.0d;

                rc = Nov31.TopoStar(AstroUtil.JulianDateTT(0.0d), AstroUtil.DeltaT(), Cat3, OnSurface3, Accuracy.Full, ref RA, ref DEC);
                TL.LogMessage("TransformTest", Name + " Novas31 RA/DEC Actual  : " + Util.HoursToHMS(transform.RATopocentric, ":", ":", "", 3) + " " + Util.DegreesToDMS(transform.DECTopocentric, ":", ":", "", 3));
                TL.LogMessage("TransformTest", Name + " Novas31 RA/DEC Expected: " + Util.HoursToHMS(RA, ":", ":", "", 3) + " " + Util.DegreesToDMS(DEC, ":", ":", "", 3));
                CompareDouble("TransformTest", Name + " Novas31 Topocentric RA", transform.RATopocentric, RA, RATolerance, DoubleType.Hours0To24);
                CompareDouble("TransformTest", Name + " Novas31 Topocentric Dec", transform.DECTopocentric, DEC, DecTolerance, DoubleType.Degrees0To360);
            }

            catch (Exception ex)
            {
                LogException("TransformTest2000 Exception", ex.ToString());
            }

        }

        private void NovasComTests()
        {
            double JD;
            var EA = new Astrometry.NOVASCOM.Earth();

            // Astrometry test data for planets obtained from the original 32bit  components
            // The data is for the arbitrary test date Thursday, 30 December 2010 09:00:00" 
            double[] Mercury = [-0.146477263357071d, -0.739730529540394d, -0.275237058490435d, -0.146552680905756d, -0.73971718813053d, -0.275232768188589d, -0.144373027430296d, -0.740086172152297d, -0.275392756115203d, -0.146535954004373d, -0.739695817952422d, -0.27526565650813d, -0.144631609584528d, -0.740282847942203d, -0.274694144671298d];

            double[] Venus = [-0.372100971951828d, -0.449343256389233d, -0.154902566021356d, -0.372134026409355d, -0.449318563980132d, -0.154894786229779d, -0.370822518399929d, -0.450260651717891d, -0.155303914026952d, -0.372117531588399d, -0.449297391719315d, -0.154927789061455d, -0.370858459989012d, -0.450324120624622d, -0.154965842632926d];

            double[] Earth = [-0.147896190667482d, 0.892857938625284d, 0.387075601638547d, -0.0173032744107731d, -0.00236387487195205d, -0.00102513648587834d, -0.143537477185852d, 0.892578667572019d, 0.386954522818712d, -0.0173040812547209d, -0.00235851019511069d, -0.00102281061381548d, 2455560.875d, 1.06091018181116d, 23.437861319031d, 17.3476127157785d, -0.0796612008211573d, 23.4378391909196d];

            double[] Mars = [0.693859781031977d, -2.07097170353203d, -0.942316778727103d, 0.693632762626122d, -2.07103494950845d, -0.942344911675691d, 0.699920307729267d, -2.06926836488007d, -0.941576391467848d, 0.693650157043109d, -2.07101326327704d, -0.942377215846966d, 0.694585522035482d, -2.07602310877394d, -0.930591370257697d];

            double[] Jupiter = [5.05143975731352d, -0.264744225667142d, -0.237337980646129d, 5.05143226054377d, -0.264839400889264d, -0.237391349301542d, 5.05234611594448d, -0.252028458651431d, -0.231824986961043d, 5.05144816590343d, -0.264820597160675d, -0.237424161148292d, 5.05236200971188d, -0.252009614615973d, -0.231857781296635d];

            double[] Saturn = [-9.26931711919579d, -2.66882658902422d, -0.715270438185988d, -9.2693570741403d, -2.66869165249896d, -0.715256117911115d, -9.26176606870019d, -2.69218837105073d, -0.725464045762982d, -9.2693389177143d, -2.66867733760923d, -0.71528954188717d, -9.26144908836291d, -2.69455677562765d, -0.720448748375586d];

            double[] Uranus = [20.2306046509148d, -0.944778087940209d, -0.693874737147122d, 20.2305809175823d, -0.945147799559502d, -0.694063183048263d, 20.233665104794d, -0.893841634639764d, -0.671770710247389d, 20.2305964586587d, -0.945137337897977d, -0.694095638580723d, 20.2336806550414d, -0.893831133568696d, -0.671803148655406d];

            double[] Neptune = [25.5771370144156d, -15.409403535665d, -6.96191248591339d, 25.5759958639324d, -15.4109792947898d, -6.96261684888268d, 25.6226500915255d, -15.3460649228334d, -6.93440511594961d, 25.5760116932439d, -15.4109610670775d, -6.96264327213014d, 25.6226659036666d, -15.3460466550117d, -6.93443152177256d];

            double[] Pluto = [2.92990303317673d, -31.0320730022551d, -10.6309560278551d, 2.92658680141819d, -31.0323439400104d, -10.6310785882777d, 3.01698355034971d, -31.0248119250537d, -10.627792242015d, 2.92662685257385d, -31.0323223789174d, -10.6311050132883d, 2.99849040874885d, -31.0401256627947d, -10.5882115993866d];

            try
            {
                Status("NovasCom Tests");
                // Create the Julian date corresponding to the arbitrary test date
                JD = TestJulianDate();
                TL.LogMessage("NovasCom Tests", "Julian Date = " + JD + " = " + TEST_DATE);
                CompareDouble("NovasCom", "JulianDate", JD, 2455560.875d, TOLERANCE_E9);

                var s = new Astrometry.NOVASCOM.Site()
                {
                    Height = 80.0d,
                    Latitude = 51.0d,
                    Longitude = 0.0d,
                    Pressure = 1000.0d,
                    Temperature = 10.0d
                };
                var pv = new Astrometry.NOVASCOM.PositionVector();
                pv.SetFromSite(s, 11.0d);
                CompareDouble("NovasCom", "SetFromSite X", pv.x, -0.0000259698466733494d, TOLERANCE_E9);
                CompareDouble("NovasCom", "SetFromSite Y", pv.y, 0.00000695859944368407d, TOLERANCE_E9);
                CompareDouble("NovasCom", "SetFromSite Z", pv.z, 0.0000329791401243054d, TOLERANCE_E9);
                CompareDouble("NovasCom", "SetFromSite LightTime", pv.LightTime, 0.000000245746690359414d, TOLERANCE_E9);

                var st = new Astrometry.NOVASCOM.Star();
                st.Set(9.0d, 25.0d, 0.0d, 0.0d, 0.0d, 0.0d);

                JD = TestJulianDate();
                pv = st.GetAstrometricPosition(JD);
                double[] AstroResults = [9.0d, 25.0d, 2062648062470.13d, 0.0d, 11912861640.6606d, -1321861174769.38d, 1321861174768.63d, 871712738743.913d, 9.0d, 25.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d];
                ComparePosVec("NovasCom Astrometric", st, pv, AstroResults, false, TOLERANCE_E9);

                double[] TopoNoReractResults = [9.01140781883559d, 24.9535152700125d, 2062648062470.13d, 14.2403113213804d, 11912861640.6606d, -1326304233902.68d, 1318405625773.8d, 870195790998.564d, 9.0d, 25.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d];
                pv = st.GetTopocentricPosition(JD, s, false);
                ComparePosVec("NovasCom Topo/NoRefract", st, pv, TopoNoReractResults, true, TOLERANCE_E7);
                pv = st.GetTopocentricPosition(JD, s, true);

                double[] TopoReractResults = [9.01438008140491d, 25.0016930437008d, 2062648062470.13d, 14.3031953401364d, 11912861640.6606d, -1326809918883.5d, 1316857267239.29d, 871767977436.204d, 9.0d, 25.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d];

                ComparePosVec("NovasCom Topo/Refract", st, pv, TopoReractResults, true, TOLERANCE_E7);

                NovasComTest("Mercury", (double)Body.Mercury, JD, Mercury, TOLERANCE_E6); // Test Neptune prediction
                NovasComTest("Venus", (double)Body.Venus, JD, Venus, TOLERANCE_E7); // Test Neptune prediction
                NovasComTest("Mars", (double)Body.Mars, JD, Mars, TOLERANCE_E8); // Test Neptune prediction
                NovasComTest("Jupiter", (double)Body.Jupiter, JD, Jupiter, TOLERANCE_E8); // Test Neptune prediction
                NovasComTest("Saturn", (double)Body.Saturn, JD, Saturn, TOLERANCE_E9); // Test Neptune prediction
                NovasComTest("Uranus", (double)Body.Uranus, JD, Uranus, TOLERANCE_E9); // Test Uranus prediction
                NovasComTest("Neptune", (double)Body.Neptune, JD, Neptune, TOLERANCE_E9); // Test Neptune prediction
                NovasComTest("Pluto", (double)Body.Pluto, JD, Pluto, TOLERANCE_E9); // Test Pluto Prediction

                Action("Earth");
                EA.SetForTime(JD); // Test earth properties
                CompareDouble("NovasCom", "Earth BaryPos x", EA.BarycentricPosition.x, Earth[0], TOLERANCE_E9);
                CompareDouble("NovasCom", "Earth BaryPos y", EA.BarycentricPosition.y, Earth[1], TOLERANCE_E9);
                CompareDouble("NovasCom", "Earth BaryPos z", EA.BarycentricPosition.z, Earth[2], TOLERANCE_E9);
                CompareDouble("NovasCom", "Earth BaryVel x", EA.BarycentricVelocity.x, Earth[3], TOLERANCE_E9);
                CompareDouble("NovasCom", "Earth BaryVel y", EA.BarycentricVelocity.y, Earth[4], TOLERANCE_E9);
                CompareDouble("NovasCom", "Earth BaryVel z", EA.BarycentricVelocity.z, Earth[5], TOLERANCE_E9);
                CompareDouble("NovasCom", "Earth HeliPos x", EA.HeliocentricPosition.x, Earth[6], TOLERANCE_E9);
                CompareDouble("NovasCom", "Earth HeliPos y", EA.HeliocentricPosition.y, Earth[7], TOLERANCE_E9);
                CompareDouble("NovasCom", "Earth HeliPos z", EA.HeliocentricPosition.z, Earth[8], TOLERANCE_E9);
                CompareDouble("NovasCom", "Earth HeliVel x", EA.HeliocentricVelocity.x, Earth[9], TOLERANCE_E9);
                CompareDouble("NovasCom", "Earth HeliVel y", EA.HeliocentricVelocity.y, Earth[10], TOLERANCE_E9);
                CompareDouble("NovasCom", "Earth HeliVel z", EA.HeliocentricVelocity.z, Earth[11], TOLERANCE_E9);
                CompareDouble("NovasCom", "Barycentric Time", EA.BarycentricTime, Earth[12], TOLERANCE_E9);
                CompareDouble("NovasCom", "Equation Of Equinoxes", EA.EquationOfEquinoxes, Earth[13], TOLERANCE_E9);
                CompareDouble("NovasCom", "Mean Obliquity", EA.MeanObliquity, Earth[14], TOLERANCE_E9);
                CompareDouble("NovasCom", "Nutation in Longitude", EA.NutationInLongitude, Earth[15], TOLERANCE_E9);
                CompareDouble("NovasCom", "Nutation in Obliquity", EA.NutationInObliquity, Earth[16], TOLERANCE_E9);
                CompareDouble("NovasCom", "True Obliquity", EA.TrueObliquity, Earth[17], TOLERANCE_E9);

                TL.BlankLine();
            }
            catch (Exception ex)
            {
                LogException("NovasComTests Exception", ex.ToString());
            }
            Status("");
        }

        private void ComparePosVec(string TestName, Astrometry.NOVASCOM.Star st, Astrometry.NOVASCOM.PositionVector pv, double[] Results, bool TestAzEl, double Tolerance)
        {
            CompareDouble(TestName, "RA Pos", pv.RightAscension, Results[0], Tolerance);
            CompareDouble(TestName, "DEC Pos", pv.Declination, Results[1], Tolerance);
            CompareDouble(TestName, "List", pv.Distance, Results[2], Tolerance);
            if (TestAzEl)
            {
                CompareDouble(TestName, "Elev", pv.Elevation, Results[3], Tolerance);
            }
            CompareDouble(TestName, "LightT", pv.LightTime, Results[4], Tolerance);
            CompareDouble(TestName, "x", pv.x, Results[5], Tolerance);
            CompareDouble(TestName, "y", pv.y, Results[6], Tolerance);
            CompareDouble(TestName, "z", pv.z, Results[7], Tolerance);
            CompareDouble(TestName, "RA", st.RightAscension, Results[8], Tolerance);
            CompareDouble(TestName, "DEC", st.Declination, Results[9], Tolerance);
            CompareDouble(TestName, "Number", st.Number, Results[10], Tolerance);
            CompareDouble(TestName, "Parallax", st.Parallax, Results[11], Tolerance);
            CompareDouble(TestName, "PrMoDec", st.ProperMotionDec, Results[12], Tolerance);
            CompareDouble(TestName, "PrMoRA", st.ProperMotionRA, Results[13], Tolerance);
            CompareDouble(TestName, "RadVel", st.RadialVelocity, Results[14], Tolerance);
        }

        private double TestJulianDate()
        {
            // Create the Julian date corresponding to the arbitrary test date
            var Util = new Util();
            double JD;
            JD = Util.DateUTCToJulian(DateTime.ParseExact(TEST_DATE, "F", CultureInfo.InvariantCulture));
            Util.Dispose();
            return JD;
        }

        private void NovasComTest(string p_Name, double p_Num, double JD, double[] Results, double Tolerance)
        {
            var pl = new Astrometry.NOVASCOM.Planet();
            Astrometry.Kepler.Ephemeris KE = new();
            Astrometry.NOVASCOM.PositionVector pv;
            var site = new Astrometry.NOVASCOM.Site();

            try
            {
                Action(p_Name);

                site.Height = 80d;
                site.Latitude = 51.0d;
                site.Longitude = 0.0d;
                site.Pressure = 1000d;
                site.Temperature = 10.0d;

                KE.BodyType = BodyType.MajorPlanet;
                KE.Number = (Body)3;

                pl.Name = p_Name;
                pl.Number = (int)Math.Round(p_Num);
                pl.Type = BodyType.MajorPlanet;

                pv = pl.GetAstrometricPosition(JD);
                CompareDouble("NovasCom", p_Name + " Astro x", pv.x, Results[0], Tolerance);
                CompareDouble("NovasCom", p_Name + " Astro y", pv.y, Results[1], Tolerance);
                CompareDouble("NovasCom", p_Name + " Astro z", pv.z, Results[2], Tolerance);

                pv = pl.GetVirtualPosition(JD);
                CompareDouble("NovasCom", p_Name + " Virtual x", pv.x, Results[3], Tolerance);
                CompareDouble("NovasCom", p_Name + " Virtual y", pv.y, Results[4], Tolerance);
                CompareDouble("NovasCom", p_Name + " Virtual z", pv.z, Results[5], Tolerance);

                pv = pl.GetApparentPosition(JD);
                CompareDouble("NovasCom", p_Name + " Apparent x", pv.x, Results[6], Tolerance);
                CompareDouble("NovasCom", p_Name + " Apparent y", pv.y, Results[7], Tolerance);
                CompareDouble("NovasCom", p_Name + " Apparent z", pv.z, Results[8], Tolerance);

                pv = pl.GetLocalPosition(JD, site);
                CompareDouble("NovasCom", p_Name + " Local x", pv.x, Results[9], Tolerance);
                CompareDouble("NovasCom", p_Name + " Local y", pv.y, Results[10], Tolerance);
                CompareDouble("NovasCom", p_Name + " Local z", pv.z, Results[11], Tolerance);

                pv = pl.GetTopocentricPosition(JD, site, true);
                CompareDouble("NovasCom", p_Name + " Topo x", pv.x, Results[12], Tolerance);
                CompareDouble("NovasCom", p_Name + " Topo y", pv.y, Results[13], Tolerance);
                CompareDouble("NovasCom", p_Name + " Topo z", pv.z, Results[14], Tolerance);
                Action("");
            }
            catch (Exception ex)
            {
                LogException("NovasComTest Exception", ex.ToString());
            }
        }

        private void KeplerTests()
        {
            double JD;
            double[,] MercuryPosVecs = new double[,] { { -0.273826054895093d, -0.332907079792611d, -0.149433886467295d, 0.0168077277855921d, -0.0131641564589086d, -0.00877483629689174d }, { 0.341715100611224d, -0.15606206441965d, -0.118796704430727d, 0.00818341889620433d, 0.0231859105741514d, 0.0115367662530341d }, { -0.290111477510344d, 0.152752021696643d, 0.11167615364006d, -0.0208610666648984d, -0.0207283399022831d, -0.00890975564191571d }, { -0.0948016996541467d, -0.407064938162915d, -0.207618339106762d, 0.0218998992953613d, -0.00301004943316363d, -0.00387841048587606d }, { 0.335104649167322d, 0.0711444942030144d, 0.00326561005720837d, -0.0109228475729584d, 0.0251353246085599d, 0.0145593566074213d } };
            Status("Kepler Tests");
            JD = TestJulianDate();
            KeplerTest("Mercury", Body.Mercury, JD, MercuryPosVecs, TOLERANCE_E9); // Test Mercury position vectors
            TL.BlankLine();
        }

        private void KeplerTest(string p_Name, Body p_KepNum, double JD, double[,] Results, double Tolerance)
        {
            var K = new Astrometry.Kepler.Ephemeris();
            double[] POSVEC;
            int JDIndex = 0;

            const double STEPSIZE = 1000.0d;

            for (double jdate = -2.0d * STEPSIZE; jdate <= +2.0d * STEPSIZE; jdate += STEPSIZE)
            {
                K.BodyType = BodyType.MajorPlanet;
                K.Number = p_KepNum;
                K.Name = p_Name;
                POSVEC = K.GetPositionAndVelocity(JD + jdate);
                for (int i = 0; i <= 5; i++)
                    CompareDouble("Kepler", p_Name + " " + jdate + " PV(" + i + ")", POSVEC[i], Results[JDIndex, i], Tolerance);
                JDIndex += 1;
            }
        }

        private void TimerTests()
        {
            const double RunTime = 10.0d; // Test runtime in seconds
            const int TimerInterval = 3000; // Timer interval
            double ElapsedTime;
            int LastSecond = -1;

            TL.LogMessage("TimerTests", "Started");
            Status("Timer tests");
            try
            {
                ASCOMTimer = new Timer()
                {
                    Interval = TimerInterval,
                    Enabled = true
                };
                StartTime = DateTime.Now;
                sw.Reset();
                sw.Start();
                NumberOfTicks = 0; // Initialise counter
                do
                {
                    Application.DoEvents();
                    ElapsedTime = DateTime.Now.Subtract(StartTime).TotalSeconds;
                    if (Math.Floor(ElapsedTime) != LastSecond)
                    {
                        TL.LogMessage("TimerTests", "Seconds - " + Math.Floor(ElapsedTime));
                        Action("Seconds - " + (int)Math.Round(ElapsedTime) + " / " + (int)Math.Round(RunTime));
                        LastSecond = (int)Math.Round(Math.Floor(ElapsedTime));
                    }
                }
                while (DateTime.Now.Subtract(StartTime).TotalSeconds <= RunTime);
                sw.Stop();
            }
            catch (Exception ex)
            {
                LogException("TimerTests Exception", ex.ToString());
            }
            finally
            {
                try
                {
                    ASCOMTimer.Enabled = false;
                }
                catch
                {
                }
            }

            TL.LogMessage("TimerTests", "Finished");
            TL.BlankLine();
        }

        private void LogException(string FailingModule, string Msg)
        {
            TL.LogMessageCrLf(FailingModule, "##### " + Msg);
            NExceptions += 1;
            ErrorList.Add(FailingModule + " - " + Msg);
        }

        private void Cnt_TickNet()
        {
            double Duration;

            Duration = DateTime.Now.Subtract(StartTime).TotalSeconds;
            NumberOfTicks += 1;
            CompareDouble("TimerTests Tick ", "Tick", Duration, NumberOfTicks * 3.0d, 0.1d);
            // TL.LogMessage("TimerTests", "Fired Net - " & Now.Subtract(StartTime).Seconds & " seconds")
            // Application.DoEvents()
        }

        private void ProfileTests()
        {
            string RetVal = "";
            var RetValProfileKey = new ASCOMProfile();

            // Dim DrvHlpProf As Object
            Profile AscomUtlProf;
            const string TestScope = "Test Telescope";

            ArrayList keys, values;
            try
            {
                Status("Profile tests");
                TL.LogMessage("ProfileTest", "Creating ASCOM.Utilities.Profile");
                sw.Reset();
                sw.Start();
                AscomUtlProf = new Profile();
                sw.Stop();
                TL.LogMessage("ProfileTest", "ASCOM.Utilities.Profile Created OK in " + sw.ElapsedMilliseconds + " milliseconds");
                AscomUtlProf.DeviceType = "Telescope";

                Compare("ProfileTest", "DeviceType", AscomUtlProf.DeviceType, "Telescope");

                try
                {
                    AscomUtlProf.Unregister(TestScope);
                }
                catch
                {
                } // Ensure the test scope is not registered
                Compare("ProfileTest", "IsRegistered when not registered should be False", AscomUtlProf.IsRegistered(TestScope).ToString(), "False");

                AscomUtlProf.Register(TestScope, "This is a test telescope");
                TL.LogMessage("ProfileTest", TestScope + " registered OK");
                Compare("ProfileTest", "IsRegistered when registered should be True", AscomUtlProf.IsRegistered(TestScope).ToString(), "True");

                Compare("ProfileTest", "Get Default Value", "123456", AscomUtlProf.GetValue(TestScope, "Test Name Default", "", "123456"));
                Compare("ProfileTest", "Get Defaulted Value", "123456", AscomUtlProf.GetValue(TestScope, "Test Name Default"));

                Compare("ProfileTest", "Get Default Value SubKey", "123456", AscomUtlProf.GetValue(TestScope, "Test Name Default", "SubKeyDefault", "123456"));
                Compare("ProfileTest", "Get Defaulted Value SubKey", "123456", AscomUtlProf.GetValue(TestScope, "Test Name Default", "SubKeyDefault"));

                AscomUtlProf.WriteValue(TestScope, "Test Name", "Test Value");
                AscomUtlProf.WriteValue(TestScope, "Root Test Name", "Test Value in Root key");

                AscomUtlProf.WriteValue(TestScope, "Test Name", "Test Value SubKey 2", @"SubKey1\SubKey2");
                AscomUtlProf.WriteValue(TestScope, "SubKey2 Test Name", "Test Value in SubKey 2", @"SubKey1\SubKey2");
                AscomUtlProf.WriteValue(TestScope, "SubKey2 Test Name1", "Test Value in SubKey 2", @"SubKey1\SubKey2");
                AscomUtlProf.WriteValue(TestScope, "SubKey2a Test Name2a", "Test Value in SubKey 2a", @"SubKey1\SubKey2\SubKey2a");
                AscomUtlProf.WriteValue(TestScope, "SubKey2b Test Name2b", "Test Value in SubKey 2b", @"SubKey1\SubKey2\SubKey2a\SubKey2b");
                AscomUtlProf.WriteValue(TestScope, "SubKey2c Test Name2c", "Test Value in SubKey 2c", @"SubKey1\SubKey2\SubKey2c");
                AscomUtlProf.WriteValue(TestScope, "", "Null Key in SubKey2", @"SubKey1\SubKey2");
                AscomUtlProf.CreateSubKey(TestScope, "SubKey2");
                AscomUtlProf.WriteValue(TestScope, "SubKey3 Test Name", "Test Value SubKey 3", "SubKey3");
                AscomUtlProf.WriteValue(TestScope, "SubKey4 Test Name", "Test Value SubKey 4", "SubKey4");
                Compare("ProfileTest", "GetValue", AscomUtlProf.GetValue(TestScope, "Test Name"), "Test Value");
                Compare("ProfileTest", "GetValue SubKey", AscomUtlProf.GetValue(TestScope, "Test Name", @"SubKey1\SubKey2"), "Test Value SubKey 2");

                // Null value write test
                try
                {
                    AscomUtlProf.WriteValue(TestScope, "Results 1", null);
                    Compare("ProfileTest", "Null value write test", "\"" + AscomUtlProf.GetValue(TestScope, "Results 1") + "\"", "\"\"");
                }
                catch (Exception ex)
                {
                    LogException("Null Value Write Test 1 Exception: ", ex.ToString());
                }
                TL.BlankLine();

                TL.LogMessage("ProfileTest", "Testing Profile.SubKeys");
                keys = AscomUtlProf.SubKeys(TestScope, "");
                List<KeyValuePair> keysList = new(keys.Cast<KeyValuePair>());
                this.Compare("ProfileTest", "Create SubKey1", keysList[0].Key.ToString() + keysList[0].Value.ToString(), "SubKey1");
                this.Compare("ProfileTest", "Create SubKey2", keysList[1].Key.ToString() + keysList[1].Value.ToString(), "SubKey2");
                this.Compare("ProfileTest", "Create SubKey3", keysList[2].Key.ToString() + keysList[2].Value.ToString(), "SubKey3");
                this.Compare("ProfileTest", "Create SubKey4", keysList[3].Key.ToString() + keysList[3].Value.ToString(), "SubKey4");
                this.Compare("ProfileTest", "Create SubKeyDefault", keysList[4].Key.ToString() + keysList[4].Value.ToString(), "SubKeyDefault");
                Compare("ProfileTest", "SubKey Count", keys.Count.ToString(), "5");
                TL.BlankLine();

                TL.LogMessage("ProfileTest", "Testing Profile.Values");
                values = AscomUtlProf.Values(TestScope, @"SubKey1\SubKey2");
                keysList = new List<KeyValuePair>(values.Cast<KeyValuePair>());
                this.Compare("ProfileTest", @"SubKey1\SubKey2 Value 0", keysList[0].Key.ToString() + " " + keysList[0].Value.ToString(), " Null Key in SubKey2");
                this.Compare("ProfileTest", @"SubKey1\SubKey2 Value 1", keysList[1].Key.ToString() + " " + keysList[1].Value.ToString(), "SubKey2 Test Name Test Value in SubKey 2");
                this.Compare("ProfileTest", @"SubKey1\SubKey2 Value 2", keysList[2].Key.ToString() + " " + keysList[2].Value.ToString(), "SubKey2 Test Name1 Test Value in SubKey 2");
                this.Compare("ProfileTest", @"SubKey1\SubKey2 Value 3", keysList[3].Key.ToString() + " " + keysList[3].Value.ToString(), "Test Name Test Value SubKey 2");
                Compare("ProfileTest", @"SubKey1\SubKey2 Count", values.Count.ToString(), "4");
                TL.BlankLine();

                TL.LogMessage("ProfileTest", "Testing Profile.DeleteSubKey - SubKey2");
                AscomUtlProf.DeleteSubKey(TestScope, "Subkey2");
                keys = AscomUtlProf.SubKeys(TestScope, "");
                keysList = new List<KeyValuePair>(keys.Cast<KeyValuePair>());
                this.Compare("ProfileTest", "Create SubKey1", keysList[0].Key.ToString() + keysList[0].Value.ToString(), "SubKey1");
                this.Compare("ProfileTest", "Create SubKey3", keysList[1].Key.ToString() + keysList[1].Value.ToString(), "SubKey3");
                this.Compare("ProfileTest", "Create SubKey4", keysList[2].Key.ToString() + keysList[2].Value.ToString(), "SubKey4");
                this.Compare("ProfileTest", "Create SubKeyDefault", keysList[3].Key.ToString() + keysList[3].Value.ToString(), "SubKeyDefault");
                Compare("ProfileTest", "SubKey Count", keys.Count.ToString(), "4");
                TL.BlankLine();

                TL.LogMessage("ProfileTest", @"Testing Profile.DeleteValue - SubKey1\SubKey2\Test Name");
                AscomUtlProf.DeleteValue(TestScope, "Test Name", @"SubKey1\SubKey2");
                values = AscomUtlProf.Values(TestScope, @"SubKey1\SubKey2");
                keysList = new List<KeyValuePair>(values.Cast<KeyValuePair>());
                this.Compare("ProfileTest", @"SubKey1\SubKey2 Value 0", keysList[0].Key.ToString() + " " + keysList[0].Value.ToString(), " Null Key in SubKey2");
                this.Compare("ProfileTest", @"SubKey1\SubKey2 Value 1", keysList[1].Key.ToString() + " " + keysList[1].Value.ToString(), "SubKey2 Test Name Test Value in SubKey 2");
                this.Compare("ProfileTest", @"SubKey1\SubKey2 Value 2", keysList[2].Key.ToString() + " " + keysList[2].Value.ToString(), "SubKey2 Test Name1 Test Value in SubKey 2");
                Compare("ProfileTest", @"SubKey1\SubKey2 Count", values.Count.ToString(), "3");
                TL.BlankLine();

                TL.LogMessage("ProfileTest", "Bulk Profile operation tests");
                try
                {
                    Compare("ProfileTest", "XML Read", AscomUtlProf.GetProfileXML(TestScope), XMLTestString);
                }
                catch (Exception ex)
                {
                    LogException("GetProfileXML", ex.ToString());
                }

                try
                {
                    RetVal = AscomUtlProf.GetProfileXML(TestScope);
                    RetVal = RetVal.Replace(TestTelescopeDescription, RevisedTestTelescopeDescription);
                    AscomUtlProf.SetProfileXML(TestScope, RetVal);
                    Compare("ProfileTest", "XML Write", AscomUtlProf.GetValue(TestScope, ""), RevisedTestTelescopeDescription);
                }
                catch (Exception ex)
                {
                    LogException("SetProfileXML", ex.ToString());
                }

                try
                {
                    RetValProfileKey = AscomUtlProf.GetProfile(TestScope);
                    foreach (string subkey in RetValProfileKey.ProfileValues.Keys)
                    {
                        // TL.LogMessage("SetProfileXML", "Found: " & subkey)
                        foreach (string valuename in RetValProfileKey.ProfileValues[subkey].Keys)
                        {
                            // TL.LogMessage("SetProfileXML", "Found Value: " & valuename & " = " & RetValProfileKey.ProfileValues.Item(subkey).Item(valuename))
                        }
                    }
                    Compare("ProfileTest", "ASCOMProfile Read", RetValProfileKey.ProfileValues[@"SubKey1\SubKey2\SubKey2c"]["SubKey2c Test Name2c"], "Test Value in SubKey 2c");
                    RetValProfileKey.SetValue("", NewTestTelescopeDescription);
                    RetValProfileKey.SetValue("NewName", "New value");

                    RetValProfileKey.SetValue("NewName 2", "New value 2", @"\New Subkey 2");
                    RetValProfileKey.SetValue("Newname 3", "New value 3", "New Subkey 3");
                    AscomUtlProf.SetProfile(TestScope, RetValProfileKey);

                    Compare("ProfileTest", "ASCOMProfile Write", AscomUtlProf.GetValue(TestScope, ""), NewTestTelescopeDescription);
                    Compare("ProfileTest", "ASCOMProfile Write", AscomUtlProf.GetValue(TestScope, "NewName"), "New value");
                    Compare("ProfileTest", "ASCOMProfile Write", AscomUtlProf.GetValue(TestScope, "NewName 2", @"\New Subkey 2"), "New value 2");
                    Compare("ProfileTest", "ASCOMProfile Write", AscomUtlProf.GetValue(TestScope, "NewName 3", "New Subkey 3"), "New value 3");

                    TL.BlankLine();
                }
                catch (Exception ex)
                {
                    LogException("SetProfile", ex.ToString());
                }

                // Registered device types test
                ArrayList DevTypes;
                try
                {
                    DevTypes = AscomUtlProf.RegisteredDeviceTypes;
                    TL.LogMessage("ProfileTest", "DeviceTypes - found " + DevTypes.Count + " device types");
                    foreach (string DeviceType in DevTypes)
                        TL.LogMessage("ProfileTest", "DeviceTypes - " + DeviceType);
                    Compare("ProfileTest", "DeviceTypes Camera", DevTypes.Contains("Camera").ToString(), "True");
                    Compare("ProfileTest", "DeviceTypes Dome", DevTypes.Contains("Dome").ToString(), "True");
                    Compare("ProfileTest", "DeviceTypes FilterWheel", DevTypes.Contains("FilterWheel").ToString(), "True");
                    Compare("ProfileTest", "DeviceTypes Focuser", DevTypes.Contains("Focuser").ToString(), "True");
                    Compare("ProfileTest", "DeviceTypes Rotator", DevTypes.Contains("Rotator").ToString(), "True");
                    Compare("ProfileTest", "DeviceTypes SafetyMonitor", DevTypes.Contains("SafetyMonitor").ToString(), "True");
                    Compare("ProfileTest", "DeviceTypes Switch", DevTypes.Contains("Switch").ToString(), "True");
                    Compare("ProfileTest", "DeviceTypes Telescope", DevTypes.Contains("Telescope").ToString(), "True");
                    TL.BlankLine();
                }
                catch (Exception ex)
                {
                    LogException("RegisteredDeviceTypes", ex.ToString());
                }

                // Registered devices tests
                try
                {
                    TL.LogMessage("ProfileTest", "Installed Simulator Devices");
                    keys = AscomUtlProf.RegisteredDevices("Camera");
                    CheckSimulator(keys, "Camera", "ASCOM.Simulator.Camera", false);
                    CheckSimulator(keys, "Camera", "CCDSimulator.Camera", false);

                    keys = AscomUtlProf.RegisteredDevices("CoverCalibrator");
                    CheckSimulator(keys, "CoverCalibrator", "ASCOM.Simulator.CoverCalibrator", false);

                    keys = AscomUtlProf.RegisteredDevices("Dome");
                    CheckSimulator(keys, "Dome", "DomeSim.Dome", false);
                    CheckSimulator(keys, "Dome", "ASCOM.DeviceHub.Dome", false);

                    keys = AscomUtlProf.RegisteredDevices("FilterWheel");
                    CheckSimulator(keys, "FilterWheel", "ASCOM.Simulator.FilterWheel", false);
                    CheckSimulator(keys, "FilterWheel", "FilterWheelSim.FilterWheel", false);

                    keys = AscomUtlProf.RegisteredDevices("Focuser");
                    CheckSimulator(keys, "Focuser", "FocusSim.Focuser", false);
                    CheckSimulator(keys, "Focuser", "ASCOM.Simulator.Focuser", false);
                    CheckSimulator(keys, "Focuser", "ASCOM.DeviceHub.Focuser", false);

                    keys = AscomUtlProf.RegisteredDevices("ObservingConditions");
                    CheckSimulator(keys, "ObservingConditions", "ASCOM.OCH.ObservingConditions", false);
                    CheckSimulator(keys, "ObservingConditions", "ASCOM.Simulator.ObservingConditions", false);

                    keys = AscomUtlProf.RegisteredDevices("Rotator");
                    CheckSimulator(keys, "Rotator", "ASCOM.Simulator.Rotator", false);

                    keys = AscomUtlProf.RegisteredDevices("SafetyMonitor");
                    CheckSimulator(keys, "SafetyMonitor", "ASCOM.Simulator.SafetyMonitor", false);

                    keys = AscomUtlProf.RegisteredDevices("Switch");
                    CheckSimulator(keys, "Switch", "SwitchSim.Switch", false);
                    CheckSimulator(keys, "Switch", "ASCOM.Simulator.Switch", false);

                    keys = AscomUtlProf.RegisteredDevices("Telescope");
                    CheckSimulator(keys, "Telescope", "ASCOM.Simulator.Telescope", false);
                    CheckSimulator(keys, "Telescope", "ScopeSim.Telescope", false);
                    CheckSimulator(keys, "Telescope", "ASCOM.DeviceHub.Telescope", false);

                    // Devices that may or may not be present because they are now optional
                    CheckSimulator(keys, "Dome", "Hub.Dome", true);
                    CheckSimulator(keys, "Dome", "Pipe.Dome", true);
                    CheckSimulator(keys, "Dome", "POTH.Dome", true);
                    CheckSimulator(keys, "Focuser", "Hub.Focuser", true);
                    CheckSimulator(keys, "Focuser", "Pipe.Focuser", true);
                    CheckSimulator(keys, "Focuser", "POTH.Focuser", true);
                    CheckSimulator(keys, "Telescope", "Hub.Telescope", true);
                    CheckSimulator(keys, "Telescope", "Pipe.Telescope", true);
                    CheckSimulator(keys, "Telescope", "POTH.Telescope", true);
                    CheckSimulator(keys, "Telescope", "ASCOMDome.Telescope", true);

                    DevTypes = AscomUtlProf.RegisteredDeviceTypes;
                    foreach (string DevType in DevTypes)
                    {
                        // TL.LogMessage("RegisteredDevices", "Found " & DevType)
                        keys = AscomUtlProf.RegisteredDevices(DevType);
                        foreach (KeyValuePair kvp in keys)
                        {
                            // TL.LogMessage("RegisteredDevices", "  " & kvp.Key & " - " & kvp.Value)
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogException("RegisteredDevices", ex.ToString());
                }

                // Empty string
                try
                {
                    keys = AscomUtlProf.RegisteredDevices("");
                    TL.LogMessage("RegisteredDevices EmptyString", "Found " + keys.Count + " devices");
                    foreach (KeyValuePair<string, string> kvp in keys)
                        TL.LogMessage("RegisteredDevices EmptyString", "  " + kvp.Key + " - " + kvp.Value);
                }
                catch (Exceptions.InvalidValueException)
                {
                    Compare("ProfileTest", "RegisteredDevices with an empty string", "InvalidValueException", "InvalidValueException");
                }
                catch (Exception ex)
                {
                    LogException("RegisteredDevices EmptyString", ex.ToString());
                }

                // Nothing
                try
                {
                    keys = AscomUtlProf.RegisteredDevices(null);
                    TL.LogMessage("RegisteredDevices Nothing", "Found " + keys.Count + " devices");
                    foreach (KeyValuePair<string, string> kvp in keys)
                        TL.LogMessage("RegisteredDevices Nothing", "  " + kvp.Key + " - " + kvp.Value);
                }
                catch (Exceptions.InvalidValueException)
                {
                    Compare("ProfileTest", "RegisteredDevices with a null value", "InvalidValueException", "InvalidValueException");
                }
                catch (Exception ex)
                {
                    LogException("RegisteredDevices Nothing", ex.ToString());
                }

                // Bad value
                try
                {
                    keys = AscomUtlProf.RegisteredDevices("asdwer vbn tyu");
                    Compare("ProfileTest", "RegisteredDevices with an Unknown DeviceType", keys.Count.ToString(), "0");
                    foreach (KeyValuePair<string, string> kvp in keys)
                        TL.LogMessage("RegisteredDevices Bad", "  " + kvp.Key + " - " + kvp.Value);
                }
                catch (Exceptions.InvalidValueException)
                {
                    LogException("ProfileTest", "RegisteredDevices Unknown DeviceType incorrectly generated an InvalidValueException");
                }
                catch (Exception ex)
                {
                    LogException("RegisteredDevices Bad", ex.ToString());
                }
                TL.BlankLine();

                Status("Profile performance tests");
                // Timing tests
                sw.Reset();
                sw.Start();
                for (int i = 1; i <= 100; i++)
                    AscomUtlProf.WriteValue(TestScope, "Test Name " + i.ToString(), "Test Value");
                sw.Stop();
                TL.LogMessage("ProfilePerformance", "Writevalue : " + sw.ElapsedMilliseconds / 100d + " milliseconds");

                sw.Reset();
                sw.Start();
                for (int i = 1; i <= 100; i++)
                    keys = AscomUtlProf.SubKeys(TestScope, "");
                sw.Stop();
                TL.LogMessage("ProfilePerformance", "SubKeys : " + sw.ElapsedMilliseconds / 100d + " milliseconds");

                sw.Reset();
                sw.Start();
                for (int i = 1; i <= 100; i++)
                    keys = AscomUtlProf.Values(TestScope, @"SubKey1\SubKey2");
                sw.Stop();
                TL.LogMessage("ProfilePerformance", "Values : " + sw.ElapsedMilliseconds / 100d + " milliseconds");

                sw.Reset();
                sw.Start();
                for (int i = 1; i <= 100; i++)
                    RetVal = AscomUtlProf.GetValue(TestScope, "Test Name " + i.ToString());
                sw.Stop();
                TL.LogMessage("ProfilePerformance", "GetValue : " + sw.ElapsedMilliseconds / 100d + " milliseconds");

                sw.Reset();
                for (int i = 1; i <= 100; i++)
                {
                    AscomUtlProf.WriteValue(TestScope, "Test Name", "Test Value SubKey 2", @"SubKey1\SubKey2");
                    sw.Start();
                    AscomUtlProf.DeleteValue(TestScope, "Test Name", @"SubKey1\SubKey2");
                    sw.Stop();
                }
                TL.LogMessage("ProfilePerformance", "Delete : " + sw.ElapsedMilliseconds / 100d + " milliseconds");

                sw.Reset();
                sw.Start();
                for (int i = 1; i <= 100; i++)
                    RetVal = AscomUtlProf.GetProfileXML(TestScope);
                sw.Stop();
                TL.LogMessage("ProfilePerformance", "GetProfileXML : " + sw.ElapsedMilliseconds / 100d + " milliseconds");

                sw.Reset();
                sw.Start();
                for (int i = 1; i <= 100; i++)
                    RetVal = AscomUtlProf.GetProfileXML("ScopeSim.Telescope");
                sw.Stop();
                TL.LogMessage("ProfilePerformance", "GetProfileXML : " + sw.ElapsedMilliseconds / 100d + " milliseconds");

                sw.Reset();
                sw.Start();
                for (int i = 1; i <= 100; i++)
                    RetValProfileKey = AscomUtlProf.GetProfile(TestScope);
                sw.Stop();
                TL.LogMessage("ProfilePerformance", "GetProfile : " + sw.ElapsedMilliseconds / 100d + " milliseconds");

                sw.Reset();
                sw.Start();
                for (int i = 1; i <= 100; i++)
                    RetValProfileKey = AscomUtlProf.GetProfile("ScopeSim.Telescope");
                sw.Stop();
                TL.LogMessage("ProfilePerformance", "GetProfile : " + sw.ElapsedMilliseconds / 100d + " milliseconds");

                sw.Reset();
                sw.Start();
                for (int i = 1; i <= 100; i++)
                    AscomUtlProf.SetProfile("ScopeSim.Telescope", RetValProfileKey);
                sw.Stop();
                TL.LogMessage("ProfilePerformance", "SetProfile : " + sw.ElapsedMilliseconds / 100d + " milliseconds");

                sw.Reset();
                sw.Start();
                for (int i = 1; i <= 100; i++)
                    AscomUtlProf.SetProfileXML("ScopeSim.Telescope", RetVal);
                sw.Stop();
                TL.LogMessage("ProfilePerformance", "SetProfileXML : " + sw.ElapsedMilliseconds / 100d + " milliseconds");

                TL.BlankLine();

                AscomUtlProf.Unregister(TestScope);
                Compare("ProfileTest", "Test telescope registered after unregister", Conversions.ToString(AscomUtlProf.IsRegistered(TestScope)), "False");

                AscomUtlProf.Dispose();
                TL.LogMessage("ProfileTest", "Profile Disposed OK");
                AscomUtlProf = null;
                TL.BlankLine();

                Status("Profile multi-tasking tests");

                Profile P1 = new(), P2 = new();
                string R1, R2;

                P1.Register(TestScope, "Multi access tester");

                P1.WriteValue(TestScope, TestScope, "1");
                R1 = P1.GetValue(TestScope, TestScope);
                R2 = P2.GetValue(TestScope, TestScope);
                Compare("ProfileMultiAccess", "MultiAccess", R1, R2);

                P1.WriteValue(TestScope, TestScope, "2");
                R1 = P1.GetValue(TestScope, TestScope);
                R2 = P2.GetValue(TestScope, TestScope);
                Compare("ProfileMultiAccess", "MultiAccess", R1, R2);

                P2.Dispose();
                P1.Dispose();

                // Multiple writes to the same value - single threaded
                TL.LogMessage("ProfileMultiAccess", "MultiWrite - SingleThread Started");
                Action("MultiWrite - SingleThread Started");
                try
                {
                    var P = new Profile[101];
                    for (int i = 1; i <= 100; i++)
                    {
                        P[i] = new Profile();
                        P[i].WriteValue(TestScope, TestScope, "27");
                    }
                }
                catch (Exception ex)
                {
                    LogException("MultiWrite - SingleThread", ex.ToString());
                }

                TL.LogMessage("ProfileMultiAccess", "MultiWrite - SingleThread Finished");

                TL.LogMessage("ProfileMultiAccess", "MultiWrite - MultiThread Started");
                Action("MultiWrite - MultiThread Started");
                // Multiple writes -multi-threaded
                const int NThreads = 3;

                var ProfileThreads = new Thread[4];
                for (int i = 0; i <= NThreads; i++)
                {
                    ProfileThreads[i] = new Thread((_) => this.ProfileThread(999));
                    ProfileThreads[i].Start(i);
                }
                for (int i = 0; i <= NThreads; i++)
                    ProfileThreads[i].Join();

                P1 = new Profile();
                P1.Unregister(TestScope);

                TL.LogMessage("ProfileMultiAccess", "MultiWrite - MultiThread Finished");
                TL.BlankLine();
                TL.BlankLine();
            }

            catch (Exception ex)
            {
                LogException("Exception", ex.ToString());
            }

        }

        private void CheckSimulator(ArrayList Devices, string DeviceType, string DeviceName, bool CanBeMissing)
        {
            bool Found = false;

            // Search for the device and record if it is present
            foreach (var Device in Devices)
            {
                if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(((KeyValuePair)Device).Key, DeviceName, false)))
                    Found = true;
            }

            // Assess the search outcome
            if (Found)
            {
                // Expected device is present so record a success
                Compare("ProfileTest", DeviceType, DeviceName, DeviceName);
            }
            else if (CanBeMissing)
            {
            }
            // Ignore the fact that the device is not present
            else
            {
                // Record that an expected device is missing
                Compare("ProfileTest", DeviceType, "", DeviceName);

            }
        }

        private void ProfileThread(int inst)
        {
            var TL = new TraceLogger("", "ProfileTrace " + inst.ToString());
            const string ts = "Test Telescope";
            TL.Enabled = true;
            // TL.LogMessage("MultiWrite - MultiThread", "ThreadStart")
            TL.LogMessage("Started", "");
            try
            {
                var P = new Profile[101];
                for (int i = 1; i <= 100; i++)
                {
                    P[i] = new Profile();
                    P[i].WriteValue(ts, ts, i.ToString());
                    TL.LogMessage("Written", i.ToString());
                    P[i].Dispose();
                }
            }
            catch (Exception ex)
            {
                LogException("MultiWrite - MultiThread", ex.ToString());
            }
            // TL.LogMessage("MultiWrite - MultiThread", "ThreadEnd")
            TL.Enabled = false;
            TL.Dispose();

        }

        private void Compare(string p_Section, string p_Name, string p_New, string p_Orig)
        {
            string ErrMsg;
            if ((p_New ?? "") == (p_Orig ?? ""))
            {
                if (p_New?.Length > 200)
                    p_New = p_New.Substring(1, 200) + "...";
                TL.LogMessage(p_Section, "Matched " + p_Name + " = " + p_New);
                NMatches += 1;
            }
            else
            {
                ErrMsg = "##### NOT Matched - " + p_Name + " - Received: \"" + p_New + "\", Expected: \"" + p_Orig + "\"";
                TL.LogMessageCrLf(p_Section, ErrMsg);
                NNonMatches += 1;
                ErrorList.Add(p_Section + " - " + ErrMsg);
            }
        }

        private void CompareBoolean(string p_Section, string p_Name, bool p_New, bool p_Orig)
        {
            string ErrMsg;
            if (p_New == p_Orig)
            {
                TL.LogMessage(p_Section, "Matched " + p_Name + " = " + p_New);
                NMatches += 1;
            }
            else
            {
                ErrMsg = "##### NOT Matched - " + p_Name + " - Received: \"" + p_New + "\", Expected: \"" + p_Orig + "\"";
                TL.LogMessageCrLf(p_Section, ErrMsg);
                NNonMatches += 1;
                ErrorList.Add(p_Section + " - " + ErrMsg);
            }
        }

        private void CompareString(string p_Section, string p_Name, string p_New, string p_Orig)
        {
            string ErrMsg;
            if ((p_New ?? "") == (p_Orig ?? ""))
            {
                TL.LogMessage(p_Section, "Matched " + p_Name + " = " + p_New);
                NMatches += 1;
            }
            else
            {
                ErrMsg = "##### NOT Matched - " + p_Name + " - Received: \"" + p_New + "\", Expected: \"" + p_Orig + "\"";
                TL.LogMessageCrLf(p_Section, ErrMsg);
                NNonMatches += 1;
                ErrorList.Add(p_Section + " - " + ErrMsg);
            }
        }

        private void CompareLongInteger(string p_Section, string p_Name, long p_New, long p_Orig)
        {
            string ErrMsg;
            if (p_New == p_Orig)
            {
                TL.LogMessage(p_Section, "Matched " + p_Name + " = " + p_New);
                NMatches += 1;
            }
            else
            {
                ErrMsg = "##### NOT Matched - " + p_Name + " - Received: \"" + p_New + "\", Expected: \"" + p_Orig + "\"";
                TL.LogMessageCrLf(p_Section, ErrMsg);
                NNonMatches += 1;
                ErrorList.Add(p_Section + " - " + ErrMsg);
            }
        }

        private void CompareInteger(string p_Section, string p_Name, int p_New, int p_Orig)
        {
            string ErrMsg;
            if (p_New == p_Orig)
            {
                TL.LogMessage(p_Section, "Matched " + p_Name + " = " + p_New);
                NMatches += 1;
            }
            else
            {
                ErrMsg = "##### NOT Matched - " + p_Name + " - Received: \"" + p_New + "\", Expected: \"" + p_Orig + "\"";
                TL.LogMessageCrLf(p_Section, ErrMsg);
                NNonMatches += 1;
                ErrorList.Add(p_Section + " - " + ErrMsg);
            }
        }

        private void CompareDouble(string p_Section, string p_Name, double p_New, double p_Orig, double p_Tolerance)
        {
            CompareDouble(p_Section, p_Name, p_New, p_Orig, p_Tolerance, DoubleType.Number);
        }

        private void CompareDouble(string SectionName, string ValueName, double ActualValue, double ExpectedValue, double Tolerance, DoubleType CompareType)
        {
            double Divisor, ComparisonValue, LowerValue, HigherValue;
            string DisplayNew;
            string DisplayOriginal;
            string DisplayTolerance;

            Divisor = ExpectedValue;
            if (Divisor == 0.0d)
                Divisor = 1.0d; // Deal with possible divide by zero error

            if (ActualValue > ExpectedValue) // Get the values to be compared into lower and higher values for use in the HMS and DMS comparisons
            {
                HigherValue = ActualValue;
                LowerValue = ExpectedValue;
            }
            else
            {
                HigherValue = ExpectedValue;
                LowerValue = ActualValue;
            }

            switch (CompareType)
            {
                case DoubleType.Degrees0To360:
                    {
                        if (HigherValue > 270.0d & LowerValue < 90.0d) // We are comparing across the 0/360 degree discontinuity so we need to add the distances from 0/360 discontinuity of each value
                        {
                            ComparisonValue = 360.0d - HigherValue + LowerValue; // Calculate the distance of the high value from 360.0 degrees and add this to the lower value to get the difference between the two values.
                        }
                        else
                        {
                            // No need for special action because both numbers are on the same side of the 0/360 degree discontinuity
                            ComparisonValue = Math.Abs(ActualValue - ExpectedValue);
                        }
                        DisplayNew = AscomUtil.DegreesToDMS(ActualValue, ":", ":", "", 3);
                        DisplayOriginal = AscomUtil.DegreesToDMS(ExpectedValue, ":", ":", "", 3);
                        DisplayTolerance = AscomUtil.DegreesToDMS(Tolerance, ":", ":", "", 3) + " Seconds";
                        break;
                    }

                case DoubleType.Degrees0To360InRadians:
                    {
                        ActualValue *= RADIANS_TO_DEGREES; // Convert from radians to degrees
                        ExpectedValue *= RADIANS_TO_DEGREES;
                        if (HigherValue > 270.0d & LowerValue < 90.0d) // We are comparing across the 0/2Pi radian discontinuity so we need to add the distances from 0/2Pi discontinuity of each value
                        {
                            ComparisonValue = 360.0d - HigherValue + LowerValue; // Calculate the distance of the high value from 360.0 degrees and add this to the lower value to get the difference between the two values.
                        }
                        else
                        {
                            // No need for special action because both numbers are on the same side of the 0/360 degree discontinuity
                            ComparisonValue = Math.Abs(ActualValue - ExpectedValue);
                        }
                        DisplayNew = AscomUtil.DegreesToDMS(ActualValue, ":", ":", "", 3);
                        DisplayOriginal = AscomUtil.DegreesToDMS(ExpectedValue, ":", ":", "", 3);
                        DisplayTolerance = AscomUtil.DegreesToDMS(Tolerance, ":", ":", "", 3) + " Seconds";
                        break;
                    }

                case DoubleType.DegreesMinus180ToPlus180:
                    {
                        if (HigherValue > 90.0d & LowerValue < -90.0d) // We are comparing across the -180/+180 degree discontinuity so we need to make both numbers fall onto a continuous stream
                        {
                            ComparisonValue = 180.0d - HigherValue + (180.0d + LowerValue); // Calculate the distance of the high value from 180.0 degrees and add this to the lower value to get the difference between the two values.
                        }
                        else
                        {
                            // No need for special action because both numbers are on the same side of the 0/360 degree discontinuity
                            ComparisonValue = Math.Abs(ActualValue - ExpectedValue);
                        }
                        DisplayNew = AscomUtil.DegreesToDMS(ActualValue, ":", ":", "", 3);
                        DisplayOriginal = AscomUtil.DegreesToDMS(ExpectedValue, ":", ":", "", 3);
                        DisplayTolerance = AscomUtil.DegreesToDMS(Tolerance, ":", ":", "", 3) + " Seconds";
                        break;
                    }

                case DoubleType.DegreesMinus180ToPlus180InRadians:
                    {
                        ActualValue *= RADIANS_TO_DEGREES; // Convert from radians to degrees
                        ExpectedValue *= RADIANS_TO_DEGREES;
                        if (HigherValue > 0.5d * Math.PI & LowerValue < -0.5d * Math.PI) // We are comparing across the -Pi/+Pi degree discontinuity so we need to make both numbers fall onto a continuous stream
                        {
                            ComparisonValue = Math.PI - HigherValue + (Math.PI + LowerValue); // Calculate the distance of the high value from Pi radians and add this to the lower value to get the difference between the two values.
                        }
                        else
                        {
                            // No need for special action because both numbers are on the same side of the 0/360 degree discontinuity
                            ComparisonValue = Math.Abs(ActualValue - ExpectedValue);
                        }
                        DisplayNew = AscomUtil.DegreesToDMS(ActualValue, ":", ":", "", 3);
                        DisplayOriginal = AscomUtil.DegreesToDMS(ExpectedValue, ":", ":", "", 3);
                        DisplayTolerance = AscomUtil.DegreesToDMS(Tolerance, ":", ":", "", 3) + " Seconds";
                        break;
                    }

                case DoubleType.Hours0To24:
                    {
                        if (HigherValue > 18.0d & LowerValue < 6.0d) // We are comparing across the 0/24 hour discontinuity so we need to make both numbers fall onto a continuous stream
                        {
                            ComparisonValue = 24.0d - HigherValue + LowerValue;  // Calculate the distance of the high value from 24.0 hours and add this to the lower value to get the difference between the two values.
                        }
                        else
                        {
                            // No need for special action because both numbers are on the same side of the 0/24 hour discontinuity
                            ComparisonValue = Math.Abs(ActualValue - ExpectedValue);
                        }
                        DisplayNew = AscomUtil.HoursToHMS(ActualValue, ":", ":", "", 3);
                        DisplayOriginal = AscomUtil.HoursToHMS(ExpectedValue, ":", ":", "", 3);
                        DisplayTolerance = AscomUtil.HoursToHMS(Tolerance, ":", ":", "", 3) + " ArcSeconds";
                        break;
                    }

                case DoubleType.Hours0To24InRadians:
                    {
                        ActualValue *= RADIANS_TO_HOURS; // Convert from radians to hours
                        ExpectedValue *= RADIANS_TO_HOURS;
                        if (HigherValue > 18.0d & LowerValue < 6.0d) // We are comparing across the 0/24 hour discontinuity so we need to make both numbers fall onto a continuous stream
                        {
                            ComparisonValue = 24.0d - HigherValue + LowerValue;  // Calculate the distance of the high value from 24.0 hours and add this to the lower value to get the difference between the two values.
                        }
                        else
                        {
                            // No need for special action because both numbers are on the same side of the 0/360 degree discontinuity
                            ComparisonValue = Math.Abs(ActualValue - ExpectedValue);
                        }
                        DisplayNew = AscomUtil.HoursToHMS(ActualValue, ":", ":", "", 3);
                        DisplayOriginal = AscomUtil.HoursToHMS(ExpectedValue, ":", ":", "", 3);
                        DisplayTolerance = AscomUtil.HoursToHMS(Tolerance, ":", ":", "", 3) + " ArcSeconds";
                        break;
                    }

                case DoubleType.HoursMinus12ToPlus12:
                    {
                        if (HigherValue > 6.0d & LowerValue < -6.0d) // We are comparing across the -12.0/+12.0 hour discontinuity so we need to make both numbers fall onto a continuous stream
                        {
                            ComparisonValue = 12.0d - HigherValue + (12.0d + LowerValue); // Calculate the distance of the high value from 12.0 hours and add this to the lower value to get the difference between the two values.
                        }
                        else
                        {
                            // No need for special action because both numbers are on the same side of the 0/360 degree discontinuity
                            ComparisonValue = Math.Abs(ActualValue - ExpectedValue);
                        }
                        DisplayNew = AscomUtil.HoursToHMS(ActualValue, ":", ":", "", 3);
                        DisplayOriginal = AscomUtil.HoursToHMS(ExpectedValue, ":", ":", "", 3);
                        DisplayTolerance = AscomUtil.HoursToHMS(Tolerance, ":", ":", "", 3) + " ArcSeconds";
                        break;
                    }

                case DoubleType.HoursMinus12ToPlus12InRadians:
                    {
                        ActualValue *= RADIANS_TO_HOURS; // Convert from radians to hours
                        ExpectedValue *= RADIANS_TO_HOURS;
                        if (HigherValue > 6.0d & LowerValue < -6.0d) // We are comparing across the -12.0/+12.0 hour discontinuity so we need to make both numbers fall onto a continuous stream
                        {
                            ComparisonValue = 12.0d - HigherValue + (12.0d + LowerValue); // Calculate the distance of the high value from 12.0 hours and add this to the lower value to get the difference between the two values.
                        }
                        else
                        {
                            // No need for special action because both numbers are on the same side of the 0/360 degree discontinuity
                            ComparisonValue = Math.Abs(ActualValue - ExpectedValue);
                        }
                        DisplayNew = AscomUtil.HoursToHMS(ActualValue, ":", ":", "", 3);
                        DisplayOriginal = AscomUtil.HoursToHMS(ExpectedValue, ":", ":", "", 3);
                        DisplayTolerance = AscomUtil.HoursToHMS(Tolerance, ":", ":", "", 3) + " ArcSeconds";
                        break;
                    }

                case DoubleType.Number:
                    {
                        ComparisonValue = Math.Abs((ActualValue - ExpectedValue) / Divisor); // For other numbers we will look at the % difference
                        DisplayNew = ActualValue.ToString();
                        DisplayOriginal = ExpectedValue.ToString();
                        DisplayTolerance = Tolerance.ToString("0.0E0");
                        break;
                    }

                default:
                    {
                        LogError(SectionName, string.Format("The DoubleType value: {0} is not configured in Sub CompareDouble - this must be fixed before release!", CompareType.ToString()));
                        return;
                    }
            }

            if (ComparisonValue < Tolerance)
            {
                TL.LogMessage(SectionName, string.Format("Matched {0} {1} = {2} within tolerance of {3}", ValueName, DisplayNew, DisplayOriginal, DisplayTolerance));
                NMatches += 1;
            }
            else
            {
                LogError(SectionName, string.Format("##### NOT Matched - {0} Received: {1}, Expected: {2} within tolerance of {3}", ValueName, DisplayNew, DisplayOriginal, DisplayTolerance));
            }
        }

        private void CompareWithin(string p_Section, string p_Name, double p_Value, double p_LowerBound, double p_UpperBound)
        {
            string ErrMsg;
            if (p_Value >= p_LowerBound & p_Value <= p_UpperBound)
            {
                TL.LogMessage(p_Section, "Matched " + p_Name + " value: " + p_Value + " is within the range: " + p_LowerBound + " to " + p_UpperBound);
                NMatches += 1;
            }
            else
            {
                ErrMsg = "##### NOT Matched - " + p_Name + " value: " + p_Value + " is outside the range: " + p_LowerBound + " to " + p_UpperBound;
                NMatches += 1;
                TL.LogMessageCrLf(p_Section, ErrMsg);
                NNonMatches += 1;
                ErrorList.Add(p_Section + " - " + ErrMsg);
            }
        }

        private void VideoUtilsTests()
        {
            NativeHelpers NH;
            int rc;
            var frame = new int[101, 101];
            var frameOut = new int[101, 101];
            var frameOut2 = new int[101, 101];
            uint codec;
            var bitmap = new Bitmap(100, 100, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            byte[] byteArray;
            var frameColour = new int[101, 101, 4];

            try
            {
                TL.LogMessage("VideoUtilsTests", "Creating NativeHelpers RC");
                NH = new NativeHelpers();

                InitFrame2D(ref frame); // Initialise the frame array
                CompareLongInteger("VideoUtilsTests", "InitFrame CheckSum", CheckSum2DFrame(frame), 111088890000L);

                TL.LogMessage("VideoUtilsTests", "Opening video file RC");
                rc = NH.CreateNewAviFile("TestAVIFile.avi", 100, 100, 8, 25d, false);
                CompareInteger("VideoUtilsTests", "CreateNewAviFile", rc, 0);
                rc = NH.InitFrameIntegration(100, 100, 8);
                CompareInteger("VideoUtilsTests", "InitFrameIntegration RC", rc, 0);
                rc = NH.AddFrameForIntegration(ref frame);
                CompareInteger("VideoUtilsTests", "AddFrameForIntegration RC", rc, 0);
                rc = NH.AddFrameForIntegration(ref frame);
                CompareInteger("VideoUtilsTests", "AddFrameForIntegration RC", rc, 0);
                rc = NH.GetResultingIntegratedFrame(ref frameOut);
                CompareInteger("VideoUtilsTests", "GetResultingIntegratedFrame RC", rc, 0);
                CompareLongInteger("VideoUtilsTests", "GetResultingIntegratedFrame CheckSum", CheckSum2DFrame(frameOut), 6366267148L);

                rc = NH.SetWhiteBalance(50);
                CompareInteger("VideoUtilsTests", "SetWhiteBalance RC", rc, 0);
                rc = NH.SetGamma(0.45d);
                CompareInteger("VideoUtilsTests", "SetGamma RC", rc, 0);
                rc = NH.ApplyGammaBrightness(100, 100, 8, ref frameOut, ref frameOut2, (short)42);
                CompareInteger("VideoUtilsTests", "ApplyGammaBrightness RC", rc, 0);
                rc = NH.AviFileAddFrame(frameOut2);
                CompareInteger("VideoUtilsTests", "AviFileAddFrame RC", rc, 0);
                codec = NH.GetUsedAviCompression();
                CompareInteger("VideoUtilsTests", "GetUsedAviCompression Codec", (int)codec, 0);
                rc = NH.AviFileClose();
                CompareInteger("VideoUtilsTests", "AviFileClose RC", rc, 0);

                byteArray = NH.GetBitmapBytes(bitmap);
                CompareInteger("VideoUtilsTests", "GetBitmapBytes Length", byteArray.Length, 30055);
                CompareLongInteger("VideoUtilsTests", "GetBitmapBytes CheckSum", CheckSumByteArray(byteArray), 13006L);

                var hBitmap = bitmap.GetHbitmap();
                ClearByteArray(ref byteArray);
                CompareInteger("VideoUtilsTests", "GetBitmapBytes Clear", (int)CheckSumByteArray(byteArray), 0);
                rc = NH.GetBitmapBytes(100, 100, hBitmap, ref byteArray);
                CompareInteger("VideoUtilsTests", "GetBitmapBytes RC", rc, 0);
                CompareInteger("VideoUtilsTests", "GetBitmapBytes Length", byteArray.Length, 30055);
                CompareLongInteger("VideoUtilsTests", "GetBitmapBytes CheckSum", CheckSumByteArray(byteArray), 13006L);

                rc = NH.GetBitmapPixels(100, 100, 8, Video.FlipMode.None, frame, ref byteArray);
                CompareInteger("VideoUtilsTests", "GetBitmapPixels Monochrome FlipMode.None RC", rc, 0);
                CompareLongInteger("VideoUtilsTests", "GetBitmapBytes CheckSum", CheckSumByteArray(byteArray), 53216891806L);

                rc = NH.GetBitmapPixels(100, 100, 8, Video.FlipMode.FlipHorizontally, frame, ref byteArray);
                CompareInteger("VideoUtilsTests", "GetBitmapPixels Monochrome FlipMode.FlipHorizontally RC", rc, 0);
                CompareLongInteger("VideoUtilsTests", "GetBitmapBytes CheckSum", CheckSumByteArray(byteArray), 53218149598L);

                rc = NH.GetBitmapPixels(100, 100, 8, Video.FlipMode.FlipVertically, frame, ref byteArray);
                CompareInteger("VideoUtilsTests", "GetBitmapPixels Monochrome FlipMode.FlipHorizontally RC", rc, 0);
                CompareLongInteger("VideoUtilsTests", "GetBitmapBytes CheckSum", CheckSumByteArray(byteArray), 53216912219L);

                rc = NH.GetBitmapPixels(100, 100, 8, Video.FlipMode.FlipBoth, frame, ref byteArray);
                CompareInteger("VideoUtilsTests", "GetBitmapPixels Monochrome FlipMode.FlipHorizontally RC", rc, 0);
                CompareLongInteger("VideoUtilsTests", "GetBitmapBytes CheckSum", CheckSumByteArray(byteArray), 53218170011L);

                InitFrame3D(ref frameColour);
                CompareLongInteger("VideoUtilsTests", "InitFrame3D frameColour", CheckSum3DFrame(frameColour), 888711120000L);

                rc = NH.GetColourBitmapPixels(100, 100, 8, Video.FlipMode.None, frameColour, true, ref byteArray);
                CompareInteger("VideoUtilsTests", "GetBitmapPixels Colour FlipMode.None RC", rc, 0);
                CompareLongInteger("VideoUtilsTests", "GetBitmapPixels CheckSum", CheckSumByteArray(byteArray), 26404898591L);

                rc = NH.GetColourBitmapPixels(100, 100, 8, Video.FlipMode.FlipHorizontally, frameColour, true, ref byteArray);
                CompareInteger("VideoUtilsTests", "GetBitmapPixels Colour FlipMode.FlipHorizontally RC", rc, 0);
                CompareLongInteger("VideoUtilsTests", "GetBitmapPixels CheckSum", CheckSumByteArray(byteArray), 26407095983L);

                rc = NH.GetColourBitmapPixels(100, 100, 8, Video.FlipMode.FlipVertically, frameColour, true, ref byteArray);
                CompareInteger("VideoUtilsTests", "GetBitmapPixels Colour FlipMode.FlipVertically RC", rc, 0);
                CompareLongInteger("VideoUtilsTests", "GetBitmapPixels CheckSum", CheckSumByteArray(byteArray), 26404919004L);

                rc = NH.GetColourBitmapPixels(100, 100, 8, Video.FlipMode.FlipBoth, frameColour, true, ref byteArray);
                CompareInteger("VideoUtilsTests", "GetBitmapPixels Colour FlipMode.FlipBoth RC", rc, 0);
                CompareLongInteger("VideoUtilsTests", "GetBitmapPixels CheckSum", CheckSumByteArray(byteArray), 26407116396L);

                object argbm = bitmap;
                InitBitMap(ref argbm);
                bitmap = (Bitmap)argbm;

                frameOut2 = (int[,])NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.R, Video.FlipMode.None, out byteArray);
                CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap R CheckSum", CheckSum2DFrame(frameOut2), 16932324037L);
                CompareLongInteger("VideoUtilsTests", "ByteArray R CheckSum", CheckSumByteArray(byteArray), 311520295793L);

                frameOut2 = (int[,])NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.G, Video.FlipMode.None, out byteArray);
                CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap G CheckSum", CheckSum2DFrame(frameOut2), 8427336832L);
                CompareLongInteger("VideoUtilsTests", "ByteArray G CheckSum", CheckSumByteArray(byteArray), 154312224338L);

                frameOut2 = (int[,])NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.B, Video.FlipMode.None, out byteArray);
                CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap B CheckSum", CheckSum2DFrame(frameOut2), 16836541447L);
                CompareLongInteger("VideoUtilsTests", "ByteArray B CheckSum", CheckSumByteArray(byteArray), 306852460868L);

                frameOut2 = (int[,])NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.GrayScale, Video.FlipMode.None, out byteArray);
                CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap GrayScale CheckSum", CheckSum2DFrame(frameOut2), 11900843419L);
                CompareLongInteger("VideoUtilsTests", "ByteArray GrayScale CheckSum", CheckSumByteArray(byteArray), 217653761783L);

                frameOut2 = (int[,])NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.R, Video.FlipMode.FlipHorizontally, out byteArray);
                CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap FlipHorizontally CheckSum", CheckSum2DFrame(frameOut2), 16912983912L);
                CompareLongInteger("VideoUtilsTests", "ByteArray FlipHorizontally CheckSum", CheckSumByteArray(byteArray), 311587913189L);

                frameOut2 = (int[,])NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.R, Video.FlipMode.FlipVertically, out byteArray);
                CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap FlipVertically CheckSum", CheckSum2DFrame(frameOut2), 17104549605L);
                CompareLongInteger("VideoUtilsTests", "ByteArray FlipVertically CheckSum", CheckSumByteArray(byteArray), 311496655781L);

                frameOut2 = (int[,])NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.R, Video.FlipMode.FlipBoth, out byteArray);
                CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap FlipBoth CheckSum", CheckSum2DFrame(frameOut2), 17082845076L);
                CompareLongInteger("VideoUtilsTests", "ByteArray FlipBoth CheckSum", CheckSumByteArray(byteArray), 311564273177L);

                frameColour = (int[,,])NH.GetColourPixelsFromBitmap(bitmap, Video.FlipMode.None, out byteArray);
                CompareLongInteger("VideoUtilsTests", "GetColourPixelsFromBitmap FlipNone CheckSum", CheckSum3DFrame(frameColour), 33499743156L);
                CompareLongInteger("VideoUtilsTests", "ByteArray CheckSum", CheckSumByteArray(byteArray), 257561711588L);

                frameColour = (int[,,])NH.GetColourPixelsFromBitmap(bitmap, Video.FlipMode.FlipHorizontally, out byteArray);
                CompareLongInteger("VideoUtilsTests", "GetColourPixelsFromBitmap FlipHorizontally CheckSum", CheckSum3DFrame(frameColour), 34205855310L);
                CompareLongInteger("VideoUtilsTests", "ByteArray FlipHorizontally CheckSum", CheckSumByteArray(byteArray), 257594389904L);

                frameColour = (int[,,])NH.GetColourPixelsFromBitmap(bitmap, Video.FlipMode.FlipVertically, out byteArray);
                CompareLongInteger("VideoUtilsTests", "GetColourPixelsFromBitmap FlipVertically CheckSum", CheckSum3DFrame(frameColour), 33494570010L);
                CompareLongInteger("VideoUtilsTests", "ByteArray FlipVertically CheckSum", CheckSumByteArray(byteArray), 257629407986L);

                frameColour = (int[,,])NH.GetColourPixelsFromBitmap(bitmap, Video.FlipMode.FlipBoth, out byteArray);
                CompareLongInteger("VideoUtilsTests", "GetColourPixelsFromBitmap FlipBoth CheckSum", CheckSum3DFrame(frameColour), 34198379820L);
                CompareLongInteger("VideoUtilsTests", "ByteArray FlipBoth CheckSum", CheckSumByteArray(byteArray), 257662086302L);
                frameColour = (int[,,])NH.GetColourPixelsFromBitmap(bitmap, Video.FlipMode.None, out byteArray);

                byteArray = NH.PrepareBitmapForDisplay(frameOut, 100, 100, Video.FlipMode.None);
                CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay None CheckSum", CheckSumByteArray(byteArray), 105141385513L);
                byteArray = NH.PrepareBitmapForDisplay(frameOut, 100, 100, Video.FlipMode.FlipHorizontally);
                CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay FlipHorizontally CheckSum", CheckSumByteArray(byteArray), 105136497235L);
                byteArray = NH.PrepareBitmapForDisplay(frameOut, 100, 100, Video.FlipMode.FlipVertically);
                CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay FlipVertically CheckSum", CheckSumByteArray(byteArray), 105141405926L);
                byteArray = NH.PrepareBitmapForDisplay(frameOut, 100, 100, Video.FlipMode.FlipBoth);
                CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay FlipBoth CheckSum", CheckSumByteArray(byteArray), 105136517648L);

                byteArray = NH.PrepareColourBitmapForDisplay(frameColour, frameColour.GetUpperBound(0), frameColour.GetUpperBound(1), Video.FlipMode.None, true);
                CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay None CheckSum", CheckSumByteArray(byteArray), 35988823L);
                byteArray = NH.PrepareColourBitmapForDisplay(frameColour, frameColour.GetUpperBound(0), frameColour.GetUpperBound(1), Video.FlipMode.FlipHorizontally, true);
                CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay FlipHorizontally CheckSum", CheckSumByteArray(byteArray), 35988058L);
                byteArray = NH.PrepareColourBitmapForDisplay(frameColour, frameColour.GetUpperBound(0), frameColour.GetUpperBound(1), Video.FlipMode.FlipVertically, true);
                CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay FlipVertically CheckSum", CheckSumByteArray(byteArray), 36009282L);
                byteArray = NH.PrepareColourBitmapForDisplay(frameColour, frameColour.GetUpperBound(0), frameColour.GetUpperBound(1), Video.FlipMode.FlipBoth, true);
                CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay FlipBoth CheckSum", CheckSumByteArray(byteArray), 36008517L);
            }

            catch (Exception ex)
            {
                LogException("VideoUtilTests", "Exception: " + ex.ToString());
            }

            TL.BlankLine();

        }

        /// <summary>
        /// Initialise a frame with a known set of values
        /// </summary>
        /// <param name="frame">The frame to initialise</param>
        /// <remarks></remarks>
        private void InitFrame2D(ref int[,] frame)
        {
            int i, j;

            var loopTo = frame.GetUpperBound(0) - 1;
            for (i = 0; i <= loopTo; i++)
            {
                var loopTo1 = frame.GetUpperBound(1) - 1;
                for (j = 0; j <= loopTo1; j++)
                    frame[i, j] = i * j;
            }
        }

        /// <summary>
        /// Initialise a frame with a known set of values
        /// </summary>
        /// <param name="frame">The frame to initialise</param>
        /// <remarks></remarks>
        private void InitFrame3D(ref int[,,] frame)
        {
            int i, j, k;

            var loopTo = frame.GetUpperBound(0) - 1;
            for (i = 0; i <= loopTo; i++)
            {
                var loopTo1 = frame.GetUpperBound(1) - 1;
                for (j = 0; j <= loopTo1; j++)
                {
                    var loopTo2 = frame.GetUpperBound(2) - 1;
                    for (k = 0; k <= loopTo2; k++)
                        frame[i, j, k] = i * j * k;
                }
            }
        }

        /// <summary>
        /// Initialise a bitmap with a wavy, asymmetric  graphic
        /// </summary>
        /// <param name="bm">The bitmap to initialise</param>
        /// <remarks></remarks>
        private void InitBitMap(ref object bm)
        {
            // Define arbitrary constants for the image creation
            const int X_SIZE = 201;
            const int Y_SIZE = 100;
            const int UNIT_SIZE = 3;

            Graphics wavyImage;
            int offset = 0;
            int counter = 0;

            bm = new Bitmap(X_SIZE, Y_SIZE);
            wavyImage = Graphics.FromImage((Image)bm);

            for (int j = 0; j <= Y_SIZE - 1; j += UNIT_SIZE)
            {
                for (int i = 0; i <= X_SIZE - 1; i += 3 * UNIT_SIZE)
                {
                    wavyImage.FillRectangle(Brushes.Red, i + offset, j, UNIT_SIZE, UNIT_SIZE);
                    wavyImage.FillRectangle(Brushes.White, i + offset + UNIT_SIZE, j, UNIT_SIZE, UNIT_SIZE);
                    wavyImage.FillRectangle(Brushes.Blue, i + offset + 2 * UNIT_SIZE, j, UNIT_SIZE, UNIT_SIZE);
                }

                // Add some horizontal variability
                counter += 1;
                Math.DivRem(counter, 3, out offset);
            }
        }

        /// <summary>
        /// Initialise a byte array to 0
        /// </summary>
        /// <param name="byteArray">The byte array to initialise</param>
        /// <remarks></remarks>
        private void ClearByteArray(ref byte[] byteArray)
        {
            int i;

            var loopTo = (int)(byteArray.LongLength - 1L);
            for (i = 0; i <= loopTo; i++)
                byteArray[i] = 0;
        }

        /// <summary>
        /// Calculate a checksum for the supplied frame array
        /// </summary>
        /// <param name="frame">The frame to checksum</param>
        /// <returns>The checksum of the frame</returns>
        /// <remarks></remarks>
        private long CheckSum2DFrame(int[,] frame)
        {
            long sum;

            sum = 0L;
            for (int i = 0, loopTo = frame.GetUpperBound(0) - 1; i <= loopTo; i++)
            {
                for (int j = 0, loopTo1 = frame.GetUpperBound(1) - 1; j <= loopTo1; j++)
                    sum += Convert.ToInt64(frame[i, j]) * Convert.ToInt64(i + 1) * Convert.ToInt64(j + 1);
            }

            return sum;
        }

        /// <summary>
        /// Calculate a checksum for the supplied frame array
        /// </summary>
        /// <param name="frame">The frame to checksum</param>
        /// <returns>The checksum of the frame</returns>
        /// <remarks></remarks>
        private long CheckSum3DFrame(int[,,] frame)
        {
            long sum;

            sum = 0L;
            for (int i = 0, loopTo = frame.GetUpperBound(0) - 1; i <= loopTo; i++)
            {
                for (int j = 0, loopTo1 = frame.GetUpperBound(1) - 1; j <= loopTo1; j++)
                {
                    for (int k = 0, loopTo2 = frame.GetUpperBound(2) - 1; k <= loopTo2; k++)
                        sum += Convert.ToInt64(frame[i, j, k]) * Convert.ToInt64(i + 1) * Convert.ToInt64(j + 1) * Convert.ToInt64(k + 1);
                }
            }

            return sum;
        }

        /// <summary>
        /// Calculate a checksum for the supplied byte array
        /// </summary>
        /// <param name="byteArray">The byte array to checksum</param>
        /// <returns>The checksum of the byte array</returns>
        /// <remarks></remarks>
        private long CheckSumByteArray(byte[] byteArray)
        {
            long sum;

            sum = 0L;
            for (long i = 0L, loopTo = byteArray.LongLength - 1L; i <= loopTo; i++)
                sum += Convert.ToInt64(byteArray[(int)i]) * Convert.ToInt64(i + 1L);

            return sum;
        }

        private void CacheTests()
        {
            if (CacheTest("CacheTest", false))
                CacheTest("CacheTestLogged", true);
            // CacheTest("CacheTestLogged", True)
        }

        private bool CacheTest(string TestName, bool LogCache)
        {
            Cache cache = null;
            Cache cache1 = null;
            Cache cache2 = null;
            Cache cache3 = null;
            Cache cache4 = null;
            double returnDouble;
            int returnInt = default, removedItemCount, numerOfClearItemsToTest;
            bool returnBool, errorOccured = default;
            string returnString;
            KeyValuePair inputObject, returnObject;

            const int NUMBER_OF_CLEAR_CACHE_ITEMS = 1000;
            const int TEST_INTEGER = 23;
            const string TEST_INTEGER_KEY = "IntKey";
            const double TEST_DOUBLE = 45.639d;
            const string TEST_DOUBLE_KEY = "TEST_DOUBLE_KEY";
            const bool TEST_BOOL = true;
            const string TEST_BOOL_KEY = "BoolKey";
            const string TEST_STRING = "Cache test string.";
            const string TEST_STRING_KEY = "StringKey";
            const string TEST_OBJECT = "TestObjectValue23";
            const string TEST_OBJECT_KEY = "TestObjectKey23";

            Status("Running Cache functional tests");

            // Test cached doubles
            try
            {
                LogTimerResolution("Start of cache tests");
                cache = new Cache((TraceLogger)Interaction.IIf(LogCache, TL, null));
                LogTimerResolution("After creating cache");

                try
                {
                    Action("Test Doubles");
                    cache.SetDouble(TEST_DOUBLE_KEY, TEST_DOUBLE, 0.1d); // Set a value with a 100ms lifetime
                    returnDouble = cache.GetDouble(TEST_DOUBLE_KEY);
                    CompareDouble(TestName, "Get Double", returnDouble, TEST_DOUBLE, TOLERANCE_E6);
                }
                catch (Exception ex)
                {
                    LogException(TestName, "Error getting double value: " + ex.ToString());
                    errorOccured = true;
                }
                Thread.Sleep(150); // Now wait until the value has timed out and been removed from the cache then test again to make sure an exception is generated
                try
                {
                    returnDouble = cache.GetDouble(TEST_DOUBLE_KEY);
                }
                catch (NotInCacheException)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, "InvalidOperationException thrown as expected for expired double value.");
                }
                catch (Exception ex)
                {
                    LogException(TestName, "Error getting expired double value: " + ex.ToString());
                    errorOccured = true;
                }

                // Test cached int
                try
                {
                    Action("Test Integers");
                    cache.SetInt(TEST_INTEGER_KEY, TEST_INTEGER, 0.1d); // Set a value with a 100ms lifetime
                    returnInt = cache.GetInt(TEST_INTEGER_KEY);
                    CompareInteger(TestName, "Get Int", returnInt, TEST_INTEGER);
                }
                catch (Exception ex)
                {
                    LogException(TestName, "Error getting int value: " + ex.ToString());
                    errorOccured = true;
                }
                Thread.Sleep(150); // Now wait until the value has timed out and been removed from the cache then test again to make sure an exception is generated
                try
                {
                    returnInt = cache.GetInt(TEST_INTEGER_KEY);
                }
                catch (NotInCacheException)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, "InvalidOperationException thrown as expected for expired int value.");
                }
                catch (Exception ex)
                {
                    LogException(TestName, "Error getting expired int value: " + ex.ToString());
                    errorOccured = true;
                }

                // Test cached bool
                try
                {
                    Action("Test Booleans");
                    cache.SetBool(TEST_BOOL_KEY, TEST_BOOL, 0.1d); // Set a value with a 100ms lifetime
                    returnBool = cache.GetBool(TEST_BOOL_KEY);
                    CompareBoolean(TestName, "Get Int", returnBool, TEST_BOOL);
                }
                catch (Exception ex)
                {
                    LogException(TestName, "Error getting boolean value: " + ex.ToString());
                    errorOccured = true;
                }
                Thread.Sleep(150); // Now wait until the value has timed out and been removed from the cache then test again to make sure an exception is generated
                try
                {
                    returnBool = cache.GetBool(TEST_BOOL_KEY);
                }
                catch (NotInCacheException)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, "InvalidOperationException thrown as expected for expired boolean value.");
                }
                catch (Exception ex)
                {
                    LogException(TestName, "Error getting expired boolean value: " + ex.ToString());
                    errorOccured = true;
                }

                // Test cached string
                try
                {
                    Action("Test Strings");
                    cache.SetString(TEST_STRING_KEY, TEST_STRING, 0.1d); // Set a value with a 100ms lifetime
                    returnString = cache.GetString(TEST_STRING_KEY);
                    CompareString(TestName, "Get String", returnString, TEST_STRING);
                }
                catch (Exception ex)
                {
                    LogException(TestName, "Error getting string value: " + ex.ToString());
                    errorOccured = true;
                }
                Thread.Sleep(150); // Now wait until the value has timed out and been removed from the cache then test again to make sure an exception is generated
                try
                {
                    returnString = cache.GetString(TEST_STRING_KEY);
                }
                catch (NotInCacheException)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, "InvalidOperationException thrown as expected for expired string value.");
                }
                catch (Exception ex)
                {
                    LogException(TestName, "Exception getting expired string value: " + ex.ToString());
                    errorOccured = true;
                }

                // Test cached object
                try
                {
                    Action("Test Objects");
                    inputObject = new KeyValuePair()
                    {
                        Key = TEST_OBJECT_KEY,
                        Value = TEST_OBJECT
                    }; // Create a test KeyValuePair object

                    cache.Set(TEST_OBJECT_KEY, inputObject, 0.1d); // Set a value with a 100ms lifetime
                    returnObject = (KeyValuePair)cache.Get(TEST_OBJECT_KEY);

                    CompareString(TestName, "Get Object Key", returnObject.Key, TEST_OBJECT_KEY);
                    CompareString(TestName, "Get Object Value", returnObject.Value, TEST_OBJECT);
                }
                catch (Exception ex)
                {
                    LogException(TestName, "Error getting object: " + ex.ToString());
                    errorOccured = true;
                }
                Thread.Sleep(150); // Now wait until the value has timed out and been removed from the cache then test again to make sure an exception is generated
                try
                {
                    returnObject = (KeyValuePair)cache.Get(TEST_OBJECT_KEY);
                }
                catch (NotInCacheException)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, "InvalidOperationException thrown as expected for expired object.");
                }
                catch (Exception ex)
                {
                    LogException(TestName, "Exception getting expired object: " + ex.ToString());
                    errorOccured = true;
                }

                // Test throttling
                TestThrottling(cache, 2.0d, 0);
                TestThrottling(cache, 2.0d, 20);
                TestThrottling(cache, 7.0d, 0);
                TestThrottling(cache, 10.0d, 0);
                TestThrottling(cache, 11.0d, 0);
                TestThrottling(cache, 11.0d, 20);
                TestThrottling(cache, 50.0d, 0);
                TestThrottling(cache, 100.0d, 0);
            }
            catch (Exception ex1)
            {
                LogException(TestName, "Error creating ASCOM Cache, further cache testing abandoned! " + ex1.ToString());
                errorOccured = true;
            }
            finally
            {
                LogTimerResolution("Before disposing of cache");
                try
                {
                    cache.Dispose();
                    cache = null;
                }
                catch
                {
                }
            }

            // Test cache item removal
            try
            {
                cache = new Cache((TraceLogger)Interaction.IIf(LogCache, TL, null)); // Create a new cache with nothing in it
                Action("Test item removal");
                cache.SetInt(TEST_INTEGER_KEY, TEST_INTEGER, 0.1d); // Set a value with a 100ms lifetime
                returnInt = cache.GetInt(TEST_INTEGER_KEY); // Check that it is there
                CompareInteger(TestName, "Get Int", returnInt, TEST_INTEGER);

                // Now remove the item and check that it is gone
                cache.Remove(TEST_INTEGER_KEY);
                try
                {
                    returnInt = cache.GetInt(TEST_INTEGER_KEY);
                }
                catch (NotInCacheException)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, "InvalidOperationException thrown as expected for removed int value.");
                }
            }
            catch (Exception ex)
            {
                LogException(TestName, "Error getting or removing item: " + ex.ToString());
                errorOccured = true;
            }
            finally
            {
                try
                {
                    cache.Dispose();
                    cache = null;
                }
                catch
                {
                }
            }

            // Test clearing of entire cache
            try
            {
                cache = new Cache((TraceLogger)Interaction.IIf(LogCache, TL, null)); // Create a new cache with nothing in it
                Action("Clear cache - Populating cache");
                sw.Restart();
                for (int i = 1; i <= NUMBER_OF_CLEAR_CACHE_ITEMS; i++) // Create 100 entries in the cache
                    cache.SetInt(TEST_INTEGER_KEY + i, TEST_INTEGER, 10.0d); // Set values with a 10s lifetime
                sw.Stop();
                NMatches += 1;
                TL.LogMessage(TestName, string.Format("Cache populated {0} items in {1} milliseconds ({2} milliseconds per item).", NUMBER_OF_CLEAR_CACHE_ITEMS, sw.Elapsed.TotalMilliseconds.ToString("0.000"), (sw.Elapsed.TotalMilliseconds / NUMBER_OF_CLEAR_CACHE_ITEMS).ToString("0.000")));

                // Check that all can be read
                Action("Clear cache - Reading items");
                sw.Restart();
                for (int i = 1; i <= NUMBER_OF_CLEAR_CACHE_ITEMS; i++)
                    returnInt = cache.GetInt(TEST_INTEGER_KEY + i); // Check that it is there
                sw.Stop();
                NMatches += 1;
                TL.LogMessage(TestName, string.Format("Cache read back {0} items OK in {1} milliseconds ({2} milliseconds per item).", NUMBER_OF_CLEAR_CACHE_ITEMS, sw.Elapsed.TotalMilliseconds.ToString("0.000"), (sw.Elapsed.TotalMilliseconds / NUMBER_OF_CLEAR_CACHE_ITEMS).ToString("0.000")));

                // Now clear the cache and check that all values are gone
                Action("Clear cache - clearing cache");
                sw.Restart();
                cache.ClearCache();
                sw.Stop();
                NMatches += 1;
                TL.LogMessage(TestName, string.Format("Cache cleared of {0} items in {1} milliseconds ({2} milliseconds per item).", NUMBER_OF_CLEAR_CACHE_ITEMS, sw.Elapsed.TotalMilliseconds.ToString("0.000"), (sw.Elapsed.TotalMilliseconds / NUMBER_OF_CLEAR_CACHE_ITEMS).ToString("0.000")));

                Action("Clear cache - Confirming cache is empty");

                if (Debugger.IsAttached)
                {

                }

                removedItemCount = 0; // Initialise count of number of items removed

                // The "item is removed" test is very slow when run in Visual Studio, which attaches a debugger that slows exception handling, so reduce the number of tests in this circumstance.
                numerOfClearItemsToTest = Conversions.ToInteger(Interaction.IIf(Debugger.IsAttached, NUMBER_OF_CLEAR_CACHE_ITEMS / 50d, NUMBER_OF_CLEAR_CACHE_ITEMS));
                sw.Restart();

                for (int i = 1, loopTo = numerOfClearItemsToTest; i <= loopTo; i++)
                {
                    try
                    {
                        returnInt = cache.GetInt(TEST_INTEGER_KEY);
                    }
                    catch (NotInCacheException)
                    {
                        removedItemCount += 1;
                    }
                }
                sw.Stop();

                CompareInteger(TestName, "Clear cache", removedItemCount, numerOfClearItemsToTest);
                TL.LogMessage(TestName, string.Format("Checked {0} items not present in {1} milliseconds ({2} milliseconds per item).", numerOfClearItemsToTest, sw.Elapsed.TotalMilliseconds.ToString("0.000"), (sw.Elapsed.TotalMilliseconds / numerOfClearItemsToTest).ToString("0.000")));
            }
            catch (Exception ex)
            {
                LogException(TestName, "Error testing Cache.ClearCache: " + ex.ToString());
                errorOccured = true;
            }
            finally
            {
                try
                {
                    cache.Dispose();
                    cache = null;
                }
                catch
                {
                }
            }

            // Test invalid value handling
            try
            {
                cache = new Cache((TraceLogger)Interaction.IIf(LogCache, TL, null)); // Create a new cache with nothing in it

                // Test PumpMessagesInterval
                try
                {
                    Action("Test invalid values - PumpMessagesInterval");
                    cache.PumpMessagesInterval = -1;
                    LogError(TestName, "InvalidValueException was not thrown when supplying a negative value for PumpMessagesInterval.");
                    errorOccured = true;
                }
                catch (InvalidValueException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidValueException correctly generated when supplying a negative value for PumpMessagesInterval: \"{0}\"", ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, "An unexpected exception was thrown when supplying a negative value for PumpMessagesInterval." + ex.ToString());
                    errorOccured = true;
                }

                // Test GetDouble Key - empty string
                try
                {
                    Action("Test invalid values - GetDouble Key Empty");
                    cache.GetDouble("");
                    LogError(TestName, "InvalidValueException was not thrown when supplying an empty GetDouble key string.");
                    errorOccured = true;
                }
                catch (InvalidValueException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidValueException correctly generated when supplying an empty GetDouble key string: \"{0}\"", ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, "An unexpected exception was thrown when supplying an empty GetDouble key string." + ex.ToString());
                    errorOccured = true;
                }

                // Test GetDouble Key - null value
                try
                {
                    Action("Test invalid values - GetDouble Key Null");
                    cache.GetDouble(null);
                    LogError(TestName, "InvalidValueException was not thrown when supplying a null value as GetDouble key.");
                    errorOccured = true;
                }
                catch (InvalidValueException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidValueException correctly generated when supplying a null value as GetDouble key: \"{0}\"", ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, "An unexpected exception was thrown when supplying a null value as GetDouble key." + ex.ToString());
                    errorOccured = true;
                }

                // Test GetDouble MaximumCallFrequency
                try
                {
                    Action("Test invalid values - GetDouble MaximumCallFrequency");
                    cache.GetDouble(TEST_DOUBLE_KEY, -1.0d);
                    LogError(TestName, "InvalidValueException was not thrown when supplying a GetDouble negative MaximumCallFrequency value.");
                    errorOccured = true;
                }
                catch (InvalidValueException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidValueException correctly generated when supplying a GetDouble negative MaximumCallFrequency value: \"{0}\"", ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, "An unexpected exception was thrown when supplying a negative GetDouble MaximumCallFrequency value." + ex.ToString());
                    errorOccured = true;
                }

                // Test SetDouble Key - empty string
                try
                {
                    Action("Test invalid values - SetDouble Key Empty");
                    cache.SetDouble("", 0.0d, 0.0d);
                    LogError(TestName, "InvalidValueException was not thrown when supplying an empty SetDouble key string.");
                    errorOccured = true;
                }
                catch (InvalidValueException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidValueException correctly generated when supplying an empty SetDouble key string: \"{0}\"", ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, "An unexpected exception was thrown when supplying an empty SetDouble key string." + ex.ToString());
                    errorOccured = true;
                }

                // Test SetDouble Key - null value
                try
                {
                    Action("Test invalid values - SetDouble Key Null");
                    cache.SetDouble(null, 0.0d, 0.0d);
                    LogError(TestName, "InvalidValueException was not thrown when supplying a null value as SetDouble key.");
                    errorOccured = true;
                }
                catch (InvalidValueException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidValueException correctly generated when supplying a null value as SetDouble key: \"{0}\"", ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, "An unexpected exception was thrown when supplying a null value as SetDouble key." + ex.ToString());
                    errorOccured = true;
                }

                // Test SetDouble CacheTime
                try
                {
                    Action("Test invalid values - CacheTime");
                    cache.SetDouble(TEST_DOUBLE_KEY, 123.45d, -1.0d);
                    LogError(TestName, "InvalidValueException was not thrown when supplying a negative CacheTime value.");
                    errorOccured = true;
                }
                catch (InvalidValueException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidValueException correctly generated when supplying a negative SetDouble CacheTime value: \"{0}\"", ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, "An unexpected exception was thrown when supplying a negative CacheTime value." + ex.ToString());
                    errorOccured = true;
                }

                // Test SetDouble MaximumCallFrequency
                try
                {
                    Action("Test invalid values - SetDouble MaximumCallFrequency");
                    cache.SetDouble(TEST_DOUBLE_KEY, 0.0d, 1.0d, -1.0d);
                    LogError(TestName, "InvalidValueException was not thrown when supplying a negative SetDouble MaximumCallFrequency value.");
                    errorOccured = true;
                }
                catch (InvalidValueException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidValueException correctly generated when supplying a negative SetDouble MaximumCallFrequency value: \"{0}\"", ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, "An unexpected exception was thrown when supplying a negative SetDouble MaximumCallFrequency value." + ex.ToString());
                    errorOccured = true;
                }

                // Test Cache.Remove Key - empty string
                try
                {
                    Action("Test invalid values - Cache.Remove Key Empty");
                    cache.Remove("");
                    LogError(TestName, "InvalidValueException was not thrown when supplying an empty Cache.Remove key string.");
                    errorOccured = true;
                }
                catch (InvalidValueException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidValueException correctly generated when supplying an empty Cache.Remove key string: \"{0}\"", ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, "An unexpected exception was thrown when supplying an empty Cache.Remove key string." + ex.ToString());
                    errorOccured = true;
                }

                // Test Cache.Remove Keys - null value
                try
                {
                    Action("Test invalid values - Cache.Remove Key Null");
                    cache.Remove(null);
                    LogError(TestName, "InvalidValueException was not thrown when supplying a null value as Cache.Remove key.");
                    errorOccured = true;
                }
                catch (InvalidValueException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidValueException correctly generated when supplying a null value as Cache.Remove key: \"{0}\"", ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, "An unexpected exception was thrown when supplying a null value as Cache.Remove key." + ex.ToString());
                    errorOccured = true;
                }

                try
                {
                    cache.SetDouble(TEST_DOUBLE_KEY, TEST_DOUBLE, 1.0d);
                    returnInt = cache.GetInt(TEST_DOUBLE_KEY);
                    LogError(TestName, string.Format("Getting a double value as an integer worked but it should have thrown an InvalidCastException! Retrieved value: \"{0}\"", returnInt));
                }
                catch (InvalidCastException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidCastException correctly generated when getting a double value as an integer. {0} - {1}", ex.GetType().Name, ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, string.Format("An unexpected exception was thrown when setting a double value and retrieving it as an integer. {0} - {1}", ex.GetType().Name, ex.Message));
                }

                try
                {
                    cache.SetInt(TEST_INTEGER_KEY, TEST_INTEGER, 1.0d);
                    returnDouble = cache.GetDouble(TEST_INTEGER_KEY);
                    LogError(TestName, string.Format("Getting an integer value as a double worked but it should have thrown an InvalidCastException! Retrieved value: \"{0}\"", returnInt));
                }
                catch (InvalidCastException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidCastException correctly generated when getting an integer value as a double. {0} - {1}", ex.GetType().Name, ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, string.Format("An unexpected exception was thrown when setting an integer value and retrieving it as a double. {0} - {1}", ex.GetType().Name, ex.Message));
                }

                try
                {
                    cache.SetString(TEST_STRING_KEY, TEST_STRING, 1.0d);
                    returnDouble = cache.GetDouble(TEST_STRING_KEY);
                    LogError(TestName, string.Format("Getting a string value as a double worked but it should have thrown an InvalidCastException! Retrieved value: \"{0}\"", returnInt));
                }
                catch (InvalidCastException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidCastException correctly generated when getting a string value as a double. {0} - {1}", ex.GetType().Name, ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, string.Format("An unexpected exception was thrown when setting a string value and retrieving it as a double. {0} - {1}", ex.GetType().Name, ex.Message));
                }

                try
                {
                    cache.Set(TEST_DOUBLE_KEY, TEST_DOUBLE, 1.0d);
                    returnString = cache.GetString(TEST_DOUBLE_KEY);
                    LogError(TestName, string.Format("Getting a double object as a string worked but it should have thrown an InvalidCastException! Retrieved value: \"{0}\"", returnInt));
                }
                catch (InvalidCastException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidCastException correctly generated when getting a double object as a string. {0} - {1}", ex.GetType().Name, ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, string.Format("An unexpected exception was thrown when setting a string value and retrieving it as a double. {0} - {1}", ex.GetType().Name, ex.Message));
                }

                try
                {
                    cache.Set(TEST_DOUBLE_KEY, TEST_DOUBLE, 1.0d);
                    returnInt = cache.GetInt(TEST_DOUBLE_KEY);
                    LogError(TestName, string.Format("Getting a double object as an integer worked but it should have thrown an InvalidCastException! Retrieved value: \"{0}\"", returnInt));
                }
                catch (InvalidCastException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidCastException correctly generated when getting a double object as an integer. {0} - {1}", ex.GetType().Name, ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, string.Format("An unexpected exception was thrown when setting a double object and retrieving it as an integer. {0} - {1}", ex.GetType().Name, ex.Message));
                }

                try
                {
                    cache.Set(TEST_STRING_KEY, TEST_STRING, 1.0d);
                    returnBool = Conversions.ToBoolean(cache.Get(TEST_STRING_KEY));
                    LogError(TestName, string.Format("Getting a string object as a boolean worked but it should have thrown an InvalidCastException! Retrieved value: \"{0}\"", returnBool));
                }
                catch (InvalidCastException ex)
                {
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("InvalidCastException correctly generated when getting a string object as a boolean object. {0} - {1}", ex.GetType().Name, ex.Message));
                }
                catch (Exception ex)
                {
                    LogError(TestName, string.Format("An unexpected exception was thrown when getting a string object as a boolean object. {0} - {1}", ex.GetType().Name, ex.Message));
                }

                try
                {
                    cache.Set(TEST_DOUBLE_KEY, TEST_DOUBLE, 1.0d);
                    returnInt = Conversions.ToInteger(cache.Get(TEST_DOUBLE_KEY));
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("Successfully retrieved a double object: {0} as an integer object: {1}", TEST_DOUBLE, returnInt));
                }
                catch (Exception ex)
                {
                    LogError(TestName, string.Format("An unexpected exception was thrown when setting a double object and retrieving it as an integer object. {0} - {1}", ex.GetType().Name, ex.Message));
                }

                try
                {
                    cache.Set(TEST_DOUBLE_KEY, TEST_DOUBLE, 1.0d);
                    returnString = Conversions.ToString(cache.Get(TEST_DOUBLE_KEY));
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("Successfully retrieved a double object: {0} as a string object: {1}", TEST_DOUBLE, returnString));
                }
                catch (Exception ex)
                {
                    LogError(TestName, string.Format("An unexpected exception was thrown when setting a double object and retrieving it as a string object. {0} - {1}", ex.GetType().Name, ex.Message));
                }

                try
                {
                    cache.Set(TEST_DOUBLE_KEY, TEST_DOUBLE, 1.0d);
                    returnBool = Conversions.ToBoolean(cache.Get(TEST_DOUBLE_KEY));
                    NMatches += 1;
                    TL.LogMessage(TestName, string.Format("Successfully retrieved a double object: {0} as a boolean object: {1}", TEST_DOUBLE, returnBool));
                }
                catch (Exception ex)
                {
                    LogError(TestName, string.Format("An unexpected exception was thrown when setting a double object and retrieving it as a boolean object. {0} - {1}", ex.GetType().Name, ex.Message));
                }
            }

            catch (Exception ex)
            {
                LogException(TestName, "Error testing Cache invalid values: " + ex.ToString());
                errorOccured = true;
            }
            finally
            {
                try
                {
                    cache.Dispose();
                    cache = null;
                }
                catch
                {
                }
            }

            // Multiple concurrent cache test
            try
            {
                TL.LogMessage(TestName, string.Format("Creating four caches"));
                cache1 = new Cache();
                cache2 = new Cache();
                cache3 = new Cache();
                cache4 = new Cache();
                TL.LogMessage(TestName, string.Format("Setting cache values"));
                cache1.Set(TEST_DOUBLE_KEY + "1", TEST_DOUBLE, 10.0d);
                cache2.Set(TEST_DOUBLE_KEY + "2", TEST_DOUBLE, 10.0d);
                cache3.Set(TEST_DOUBLE_KEY + "3", TEST_DOUBLE, 10.0d);
                cache4.Set(TEST_DOUBLE_KEY + "4", TEST_DOUBLE, 10.0d);
                TL.LogMessage(TestName, string.Format("Getting cache values"));
                returnDouble = cache1.GetDouble(TEST_DOUBLE_KEY + "1", 2d);
                CompareDouble(TestName, "Get Cache1 Double", returnDouble, TEST_DOUBLE, TOLERANCE_E6);
                returnDouble = cache1.GetDouble(TEST_DOUBLE_KEY + "1", 2d);
                CompareDouble(TestName, "Get Cache1 Double", returnDouble, TEST_DOUBLE, TOLERANCE_E6);
                returnDouble = cache2.GetDouble(TEST_DOUBLE_KEY + "2", 2d);
                CompareDouble(TestName, "Get Cache2 Double", returnDouble, TEST_DOUBLE, TOLERANCE_E6);
                returnDouble = cache2.GetDouble(TEST_DOUBLE_KEY + "2", 2d);
                CompareDouble(TestName, "Get Cache2 Double", returnDouble, TEST_DOUBLE, TOLERANCE_E6);
                returnDouble = cache3.GetDouble(TEST_DOUBLE_KEY + "3", 2d);
                CompareDouble(TestName, "Get Cache3 Double", returnDouble, TEST_DOUBLE, TOLERANCE_E6);
                returnDouble = cache3.GetDouble(TEST_DOUBLE_KEY + "3", 2d);
                CompareDouble(TestName, "Get Cache3 Double", returnDouble, TEST_DOUBLE, TOLERANCE_E6);
                returnDouble = cache4.GetDouble(TEST_DOUBLE_KEY + "4", 2d);
                CompareDouble(TestName, "Get Cache4 Double", returnDouble, TEST_DOUBLE, TOLERANCE_E6);
                returnDouble = cache4.GetDouble(TEST_DOUBLE_KEY + "4", 2d);
                CompareDouble(TestName, "Get Cache4 Double", returnDouble, TEST_DOUBLE, TOLERANCE_E6);
            }
            catch (Exception ex)
            {
                LogException(TestName, "Error testing Cache concurrent caches: " + ex.ToString());
                errorOccured = true;
            }
            finally
            {
                try
                {
                    cache1.Dispose();
                    cache1 = null;
                }
                catch
                {
                }
                try
                {
                    cache2.Dispose();
                    cache2 = null;
                }
                catch
                {
                }
                try
                {
                    cache3.Dispose();
                    cache3 = null;
                }
                catch
                {
                }
                try
                {
                    cache4.Dispose();
                    cache4 = null;
                }
                catch
                {
                }
            }

            LogTimerResolution("End of cache tests");
            TL.BlankLine();

            return errorOccured;
        }

        private void LogTimerResolution(string message)
        {
            int MinimumResolution = default, MaximimResolution = default, CurrentResolution = default;

            _ = NtQueryTimerResolution(ref MinimumResolution, ref MaximimResolution, ref CurrentResolution);
            TL.LogMessage("TimerResolution", string.Format("{0} - Current resolution: {1}, Minimum resolution: {2}, Maximum resolution: {3}", message, CurrentResolution / 10000.0d, MinimumResolution / 10000.0d, MaximimResolution / 10000.0d));
        }

        private void TestThrottling(Cache cache, double CallsPerSecond, int PumpMessagesInterval)
        {
            double returnDouble;
            int numberOfLoops;
            int throttleTarget;

            // Set upper and lower test pass limits. These are quite wide to allow for systems where sleep time precision is low e.g. when timer resolution is 15.67ms e.g. on laptops and low power devices
            // They also don't need to be that precise given that the purpose is only to slow down the rate of client calls
            const double ACCEPTABLE_LOWER_BOUND = 4500.0d; // Minimum overall test time for a pass (milliseconds) = 110% of expected 5000ms
            const double ACCEPTABLE_UPPER_BOUND = 6000.0d; // Maximum overall test time for a pass (milliseconds) = 120% of expected 5000ms

            const double testDouble = 45.639d;
            const string TEST_DOUBLE_KEY = "TEST_DOUBLE_KEY";

            // Test throttling 
            numberOfLoops = (int)Math.Round(5d * CallsPerSecond);
            try
            {
                cache.SetDouble(TEST_DOUBLE_KEY, testDouble, 100.0d); // Set a value with a 100 second lifetime, so it doesn't time out within the text
                cache.PumpMessagesInterval = PumpMessagesInterval;
                returnDouble = cache.GetDouble(TEST_DOUBLE_KEY); // Do a first get outside the loop so that all subsequent gets will be throttled 

                // Make NUMBER_OF_THROTTLING_TEST_LOOPS calls that should be limited to CallsPerSecond calls per second, i.e. the overall test should take about 10 / CallsPerSecond seconds
                sw.Restart();
                for (int i = 1, loopTo = numberOfLoops; i <= loopTo; i++)
                {
                    Action(string.Format("Throttling test ({0} per second, pump = {1}ms)  {2}/{3}", CallsPerSecond, PumpMessagesInterval, i, numberOfLoops));
                    returnDouble = cache.GetDouble(TEST_DOUBLE_KEY, CallsPerSecond); // Get the value with a maximum rate of 2 calls per second
                }
                sw.Stop();

                throttleTarget = (int)Math.Round(numberOfLoops * 1000.0d / CallsPerSecond);
                NMatches += 1;
                if (sw.ElapsedMilliseconds > ACCEPTABLE_LOWER_BOUND & sw.ElapsedMilliseconds < ACCEPTABLE_UPPER_BOUND) // elapsed time is within +-10% of expected 5 seconds
                {
                    TL.LogMessage("TestThrottling", string.Format("Throttling at {0} calls per second, pump interval = {1}: {2} milliseconds is inside the expected range of {3} to {4} milliseconds (target = {5})", CallsPerSecond, PumpMessagesInterval, sw.ElapsedMilliseconds, ACCEPTABLE_LOWER_BOUND, ACCEPTABLE_UPPER_BOUND, throttleTarget));
                }
                else // Outside the range so log the information
                {
                    TL.LogMessage("TestThrottling", string.Format("Throttling at {0} calls per second, pump interval = {1}: {2} milliseconds is outside the expected range of {3} to {4} milliseconds (target = {5})", CallsPerSecond, PumpMessagesInterval, sw.ElapsedMilliseconds, ACCEPTABLE_LOWER_BOUND, ACCEPTABLE_UPPER_BOUND, throttleTarget));
                }
            }
            catch (Exception ex)
            {
                LogException("TestThrottling", "Exception while testing throttling: " + ex.ToString());
            }

        }

        private void UtilTests()
        {
            double t;
            string ts;
            Type HelperType;
            int i;
            bool Is64Bit;
            // Dim MyVersion As Version
            Util Utl;

            DateTime TestDate = DateTime.Parse("2010-06-01 16:37:00");
            const double TestJulianDate = 2455551.0d;

            try
            {
                Utl = new Util();
                Is64Bit = IntPtr.Size == 8; // Create a simple variable to record whether or not we are 64bit
                Status("Running Utilities functional tests");
                TL.LogMessage("UtilTests", "Creating ASCOM.Utilities.Util");
                sw.Reset();
                sw.Start();
                TL.LogMessage("UtilTests", "ASCOM.Utilities.Util Created OK in " + sw.ElapsedMilliseconds + " milliseconds");
                if (!Is64Bit)
                {
                    TL.LogMessage("UtilTests", "Creating DriverHelper.Util");
                    // DrvHlpUtil = CreateObject("DriverHelper.Util")
                    HelperType = Type.GetTypeFromProgID("DriverHelper.Util");
                    DrvHlpUtil = Activator.CreateInstance(HelperType);
                    TL.LogMessage("UtilTests", "DriverHelper.Util Created OK");

                    TL.LogMessage("UtilTests", "Creating DriverHelper2.Util");
                    // g_Util2 = CreateObject("DriverHelper2.Util")
                    HelperType = Type.GetTypeFromProgID("DriverHelper2.Util");
                    g_Util2 = Activator.CreateInstance(HelperType);
                    TL.LogMessage("UtilTests", "DriverHelper2.Util Created OK");
                }
                else
                {
                    TL.LogMessage("UtilTests", "Running 64bit so avoiding use of 32bit DriverHelper components");
                }
                TL.BlankLine();

                Compare("UtilTests", "IsMinimumRequiredVersion 5.0", Utl.IsMinimumRequiredVersion(5, 0).ToString(), "True");
                Compare("UtilTests", "IsMinimumRequiredVersion 5.4", Utl.IsMinimumRequiredVersion(5, 4).ToString(), "True");
                Compare("UtilTests", "IsMinimumRequiredVersion 5.5", Utl.IsMinimumRequiredVersion(5, 5).ToString(), "True");
                Compare("UtilTests", "IsMinimumRequiredVersion 5.6", Utl.IsMinimumRequiredVersion(5, 6).ToString(), "True");
                Compare("UtilTests", "IsMinimumRequiredVersion 6.0", Utl.IsMinimumRequiredVersion(6, 0).ToString(), "True");
                Compare("UtilTests", "IsMinimumRequiredVersion 6.1", Utl.IsMinimumRequiredVersion(6, 1).ToString(), "True");
                Compare("UtilTests", "IsMinimumRequiredVersion 6.2", Utl.IsMinimumRequiredVersion(6, 2).ToString(), "True");
                Compare("UtilTests", "IsMinimumRequiredVersion 6.3", Utl.IsMinimumRequiredVersion(6, 3).ToString(), "True");
                Compare("UtilTests", "IsMinimumRequiredVersion 6.4", Utl.IsMinimumRequiredVersion(6, 4).ToString(), "True");
                Compare("UtilTests", "IsMinimumRequiredVersion 6.5", Utl.IsMinimumRequiredVersion(6, 5).ToString(), "True");
                Compare("UtilTests", "IsMinimumRequiredVersion 6.6", Utl.IsMinimumRequiredVersion(6, 6).ToString(), "True");
                Compare("UtilTests", "IsMinimumRequiredVersion 7.0", Utl.IsMinimumRequiredVersion(7, 0).ToString(), "True");
                Compare("UtilTests", "IsMinimumRequiredVersion 7.1", Utl.IsMinimumRequiredVersion(7, 1).ToString(), "False");

                // Check that the platform version properties return the correct values
                FileVersionInfo FV;
                FV = Process.GetCurrentProcess().MainModule.FileVersionInfo; // Get this assembly's file version number against which to compare the Util version numbers

                TL.LogMessageCrLf("Versions", "  Product:  " + FV.ProductName + " " + FV.ProductVersion);
                TL.LogMessageCrLf("Versions", "  File:     " + FV.FileDescription + " " + FV.FileVersion);

                CompareInteger("UtilTests", "Major Version", Utl.MajorVersion, FV.FileMajorPart);
                CompareInteger("UtilTests", "Minor Version", Utl.MinorVersion, FV.FileMinorPart);
                CompareInteger("UtilTests", "Service Pack", Utl.ServicePack, FV.FileBuildPart);
                CompareInteger("UtilTests", "Build Number", Utl.BuildNumber, FV.FilePrivatePart);

                TL.BlankLine();

                IntArray1D[ArrayCopySize - 1] = ArrayCopySize;
                IntArray2D[ArrayCopySize - 1, ArrayCopySize - 1] = ArrayCopySize;
                IntArray3D[ArrayCopySize - 1, ArrayCopySize - 1, ArrayCopySize - 1] = ArrayCopySize;

                CheckArray(IntArray1D);
                CheckArray(IntArray2D);
                CheckArray(IntArray3D);

                TL.BlankLine();
                if (Is64Bit) // Run tests just on the new 64bit component
                {
                    t = 30.123456789d;
                    Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, ":").ToString(), "30:07'");
                    t = 60.987654321d;
                    Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, ":", ":", "", 4).ToString(), "60:59:15" + DecimalSeparator + "5556");
                    t = 50.123453456d;
                    Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t).ToString(), "03:20");
                    t = 70.763245689d;
                    Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t).ToString(), "04:43:03");
                    ts = "43:56:78" + DecimalSeparator + "2567";
                    Compare("UtilTests", "DMSToDegrees", Utl.DMSToDegrees(ts).ToString(), "43" + DecimalSeparator + "9550713055555");
                    ts = "14:39:23";
                    Compare("UtilTests", "HMSToDegrees", Utl.HMSToDegrees(ts).ToString(), "219" + DecimalSeparator + "845833333333");
                    ts = "14:37:23";
                    Compare("UtilTests", "HMSToHours", Utl.HMSToHours(ts).ToString(), "14" + DecimalSeparator + "6230555555556");
                    t = 15.567234086d;
                    Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t), "15:34");
                    t = 9.4367290317d;
                    Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t), "09:26:12");
                    TL.BlankLine();

                    this.Compare("UtilTests", "Platform Version", Utl.PlatformVersion.ToString(), ASCOMRegistryAccess.GetProfile("", "PlatformVersion"));
                    TL.BlankLine();

                    Compare("UtilTests", "TimeZoneName", Utl.TimeZoneName.ToString(), GetTimeZoneName());
                    CompareDouble("UtilTests", "TimeZoneOffset", Utl.TimeZoneOffset, -TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours, 0.017d); // 1 minute tolerance
                    Compare("UtilTests", "UTCDate", Utl.UTCDate.ToString(), Conversions.ToString(DateTime.UtcNow));
                    this.CompareDouble("UtilTests", "Julian date", Utl.JulianDate, DateTime.UtcNow.ToOADate() + Astrometry.GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET, 0.00002d); // 1 second tolerance
                    TL.BlankLine();

                    Compare("UtilTests", "DateJulianToLocal", Strings.Format(Utl.DateJulianToLocal(TestJulianDate).Subtract(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now)), "dd MMM yyyy hh:mm:ss.ffff"), "20 " + AbbreviatedMonthNames[11] + " 2010 12:00:00.0000");
                    Compare("UtilTests", "DateJulianToUTC", Strings.Format(Utl.DateJulianToUTC(TestJulianDate), "dd MMM yyyy hh:mm:ss.ffff"), "20 " + AbbreviatedMonthNames[11] + " 2010 12:00:00.0000");
                    Compare("UtilTests", "DateLocalToJulian", Utl.DateLocalToJulian(TestDate.Add(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now))).ToString(), "2455349" + DecimalSeparator + "19236111");
                    Compare("UtilTests", "DateLocalToUTC", Strings.Format(Utl.DateLocalToUTC(TestDate.Add(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now))), "dd MMM yyyy hh:mm:ss.ffff"), "01 " + AbbreviatedMonthNames[5] + " 2010 04:37:00.0000");
                    Compare("UtilTests", "DateUTCToJulian", Utl.DateUTCToJulian(TestDate).ToString(), "2455349" + DecimalSeparator + "19236111");
                    Compare("UtilTests", "DateUTCToLocal", Strings.Format(Utl.DateUTCToLocal(TestDate.Subtract(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now))), "dd MMM yyyy hh:mm:ss.ffff"), "01 " + AbbreviatedMonthNames[5] + " 2010 04:37:00.0000");
                    TL.BlankLine();

                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t), "43° 07'");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, "-"), "43-07'");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, "-", ";"), "43-07;");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, "-", ";", 3), "43-07" + DecimalSeparator + "434;");
                    TL.BlankLine();

                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t), "43° 07' 26\"");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-"), "43-07' 26\"");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-", ";"), "43-07;26\"");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-", ";", "#"), "43-07;26#");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-", ";", "#", 3), "43-07;26" + DecimalSeparator + "021#");
                    TL.BlankLine();

                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t), "02:52");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t, "-"), "02-52");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t, "-", ";"), "02-52;");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t, "-", ";", 3), "02-52" + DecimalSeparator + "496;");
                    TL.BlankLine();

                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t), "02:52:30");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-"), "02-52:30");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-", ";"), "02-52;30");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-", ";", "#"), "02-52;30#");
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-", ";", "#", 3), "02-52;29" + DecimalSeparator + "735#");
                    TL.BlankLine();

                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t), "03:07");
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t, "-"), "03-07");
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t, "-", ";"), "03-07;");
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t, "-", ";", 3), "03-07" + DecimalSeparator + "434;");
                    TL.BlankLine();

                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t), "03:07:26");
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-"), "03-07:26");
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-", ";"), "03-07;26");
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-", ";", "#"), "03-07;26#");
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-", ";", "#", 3), "03-07;26" + DecimalSeparator + "021#");
                }
                else // Run tests to compare original 32bit only and new 32/64bit capable components
                {
                    t = 30.123456789d;
                    this.Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, ":").ToString(), DrvHlpUtil.DegreesToDM(t, ":").ToString());
                    t = 60.987654321d;
                    this.Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, ":", ":", "", 4).ToString(), DrvHlpUtil.DegreesToDMS(t, ":", ":", "", (object)4).ToString());
                    t = 50.123453456d;
                    this.Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t).ToString(), DrvHlpUtil.DegreesToHM(t).ToString());
                    t = 70.763245689d;
                    this.Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t).ToString(), DrvHlpUtil.DegreesToHMS(t).ToString());
                    ts = "43:56:78" + DecimalSeparator + "2567";
                    this.Compare("UtilTests", "DMSToDegrees", Utl.DMSToDegrees(ts).ToString(), DrvHlpUtil.DMSToDegrees(ts).ToString());
                    ts = "14:39:23";
                    Compare("UtilTests", "HMSToDegrees", Utl.HMSToDegrees(ts).ToString(), Conversions.ToString(DrvHlpUtil.HMSToDegrees(ts)));
                    ts = "14:37:23";
                    Compare("UtilTests", "HMSToHours", Utl.HMSToHours(ts).ToString(), Conversions.ToString(DrvHlpUtil.HMSToHours(ts)));
                    t = 15.567234086d;
                    Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t), Conversions.ToString(DrvHlpUtil.HoursToHM(t)));
                    t = 9.4367290317d;
                    Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t), Conversions.ToString(DrvHlpUtil.HoursToHMS(t)));
                    TL.BlankLine();

                    this.Compare("UtilTests", "Platform Version", Utl.PlatformVersion.ToString(), g_Util2.PlatformVersion.ToString());
                    Compare("UtilTests", "SerialTrace", Conversions.ToString(Utl.SerialTrace), Conversions.ToString(g_Util2.SerialTrace));
                    Compare("UtilTests", "Trace File", Utl.SerialTraceFile, Conversions.ToString(g_Util2.SerialTraceFile));
                    TL.BlankLine();

                    this.Compare("UtilTests", "TimeZoneName", Utl.TimeZoneName.ToString(), g_Util2.TimeZoneName.ToString());
                    CompareDouble("UtilTests", "TimeZoneOffset", Utl.TimeZoneOffset, Conversions.ToDouble(g_Util2.TimeZoneOffset), 0.017d); // 1 minute tolerance
                    this.Compare("UtilTests", "UTCDate", Utl.UTCDate.ToString(), g_Util2.UTCDate.ToString());
                    CompareDouble("UtilTests", "Julian date", Utl.JulianDate, Conversions.ToDouble(g_Util2.JulianDate), 0.00002d); // 1 second tolerance
                    TL.BlankLine();

                    Compare("UtilTests", "DateJulianToLocal", Strings.Format(Utl.DateJulianToLocal(TestJulianDate), "dd MMM yyyy hh:mm:ss.ffff"), Strings.Format(g_Util2.DateJulianToLocal((object)TestJulianDate), "dd MMM yyyy hh:mm:ss.ffff"));
                    Compare("UtilTests", "DateJulianToUTC", Strings.Format(Utl.DateJulianToUTC(TestJulianDate), "dd MMM yyyy hh:mm:ss.ffff"), Strings.Format(g_Util2.DateJulianToUTC((object)TestJulianDate), "dd MMM yyyy hh:mm:ss.ffff"));
                    Compare("UtilTests", "DateLocalToJulian", Utl.DateLocalToJulian(TestDate).ToString(), Conversions.ToString(g_Util2.DateLocalToJulian((object)TestDate)));
                    Compare("UtilTests", "DateLocalToUTC", Strings.Format(Utl.DateLocalToUTC(TestDate), "dd MMM yyyy hh:mm:ss.ffff"), Strings.Format(g_Util2.DateLocalToUTC((object)TestDate), "dd MMM yyyy hh:mm:ss.ffff"));
                    this.Compare("UtilTests", "DateUTCToJulian", Utl.DateUTCToJulian(TestDate).ToString(), g_Util2.DateUTCToJulian((object)TestDate).ToString());
                    Compare("UtilTests", "DateUTCToLocal", Strings.Format(Utl.DateUTCToLocal(TestDate), "dd MMM yyyy hh:mm:ss.ffff"), Strings.Format(g_Util2.DateUTCToLocal((object)TestDate), "dd MMM yyyy hh:mm:ss.ffff"));
                    TL.BlankLine();

                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t), Conversions.ToString(DrvHlpUtil.DegreesToDM(t)));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, "-"), Conversions.ToString(DrvHlpUtil.DegreesToDM(t, "-")));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, "-", ";"), Conversions.ToString(DrvHlpUtil.DegreesToDM(t, "-", ";")));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, "-", ";", 3), Conversions.ToString(DrvHlpUtil.DegreesToDM(t, "-", ";", (object)3)));
                    TL.BlankLine();

                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t), Conversions.ToString(DrvHlpUtil.DegreesToDMS(t)));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-"), Conversions.ToString(DrvHlpUtil.DegreesToDMS(t, "-")));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-", ";"), Conversions.ToString(DrvHlpUtil.DegreesToDMS(t, "-", ";")));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-", ";", "#"), Conversions.ToString(DrvHlpUtil.DegreesToDMS(t, "-", ";", "#")));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-", ";", "#", 3), Conversions.ToString(DrvHlpUtil.DegreesToDMS(t, "-", ";", "#", (object)3)));
                    TL.BlankLine();

                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t), Conversions.ToString(DrvHlpUtil.DegreesToHM(t)));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t, "-"), Conversions.ToString(DrvHlpUtil.DegreesToHM(t, "-")));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t, "-", ";"), Conversions.ToString(DrvHlpUtil.DegreesToHM(t, "-", ";")));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t, "-", ";", 3), Conversions.ToString(DrvHlpUtil.DegreesToHM(t, "-", ";", (object)3)));
                    TL.BlankLine();

                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t), Conversions.ToString(DrvHlpUtil.DegreesToHMS(t)));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-"), Conversions.ToString(DrvHlpUtil.DegreesToHMS(t, "-")));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-", ";"), Conversions.ToString(DrvHlpUtil.DegreesToHMS(t, "-", ";")));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-", ";", "#"), Conversions.ToString(DrvHlpUtil.DegreesToHMS(t, "-", ";", "#")));
                    t = 43.123894628d;
                    Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-", ";", "#", 3), Conversions.ToString(DrvHlpUtil.DegreesToHMS(t, "-", ";", "#", (object)3)));
                    TL.BlankLine();

                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t), Conversions.ToString(DrvHlpUtil.HoursToHM(t)));
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t, "-"), Conversions.ToString(DrvHlpUtil.HoursToHM(t, "-")));
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t, "-", ";"), Conversions.ToString(DrvHlpUtil.HoursToHM(t, "-", ";")));
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t, "-", ";", 3), Conversions.ToString(DrvHlpUtil.HoursToHM(t, "-", ";", (object)3)));
                    TL.BlankLine();

                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t), Conversions.ToString(DrvHlpUtil.HoursToHMS(t)));
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-"), Conversions.ToString(DrvHlpUtil.HoursToHMS(t, "-")));
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-", ";"), Conversions.ToString(DrvHlpUtil.HoursToHMS(t, "-", ";")));
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-", ";", "#"), Conversions.ToString(DrvHlpUtil.HoursToHMS(t, "-", ";", "#")));
                    t = 3.123894628d;
                    Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-", ";", "#", 3), Conversions.ToString(DrvHlpUtil.HoursToHMS(t, "-", ";", "#", (object)3)));
                }
                TL.BlankLine();
                Status("Running Utilities timing tests");
                TL.LogMessage("UtilTests", "Timing tests");
                for (i = 0; i <= 5; i++)
                    TimingTest(i, Is64Bit);

                for (i = 10; i <= 50; i += 10)
                    TimingTest(i, Is64Bit);

                TimingTest(100, Is64Bit);
                TimingTest(500, Is64Bit);
                TimingTest(1000, Is64Bit);
                TimingTest(2000, Is64Bit);
                TL.BlankLine();

                // Test conversion functions
                CompareDouble("UtilTests", "DewPoint2Humidity", Utl.DewPoint2Humidity(20.0d, 25.0d), 73.81d, TOLERANCE_E4);
                CompareDouble("UtilTests", "Humidity2DewPoint", Utl.Humidity2DewPoint(45.0d, 5.0d), -5.948d, TOLERANCE_E4);

                // Test that invalid values are rejected by dewpoint and humidity conversion functions
                double result;
                string ErrMsg;

                try
                {
                    result = Utl.DewPoint2Humidity(-300.0d, 25.0d);
                    ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when dew point < absolute zero";
                    TL.LogMessageCrLf("TestInvalidOperation", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                catch (InvalidValueException) // Expected behaviour for bad value
                {
                    TL.LogMessage("UtilTests", "DewPoint2Humidity - InvalidValueException thrown as expected when dew point < absolute zero");
                    NMatches += 1;
                }

                catch (Exception ex)
                {
                    ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when dew point < absolute zero" + ", instead received: " + ex.Message;
                    TL.LogMessageCrLf("UtilTests", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                try
                {
                    result = Utl.DewPoint2Humidity(101.0d, 25.0d);
                    ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when dew point > 100.0C";
                    TL.LogMessageCrLf("TestInvalidOperation", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                catch (InvalidValueException) // Expected behaviour for bad value
                {
                    TL.LogMessage("UtilTests", "DewPoint2Humidity - InvalidValueException thrown as expected when dew point > 100.0C");
                    NMatches += 1;
                }

                catch (Exception ex)
                {
                    ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when dew point > 100.0C" + ", instead received: " + ex.Message;
                    TL.LogMessageCrLf("UtilTests", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                try
                {
                    result = Utl.DewPoint2Humidity(25.0d, -300.0d);
                    ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when ambient temperature < absolute zero";
                    TL.LogMessageCrLf("TestInvalidOperation", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                catch (InvalidValueException) // Expected behaviour for bad value
                {
                    TL.LogMessage("UtilTests", "DewPoint2Humidity - InvalidValueException thrown as expected when ambient temperature < absolute zero");
                    NMatches += 1;
                }

                catch (Exception ex)
                {
                    ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when ambient temperature < absolute zero" + ", instead received: " + ex.Message;
                    TL.LogMessageCrLf("UtilTests", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                try
                {
                    result = Utl.DewPoint2Humidity(25.0d, 101.0d);
                    ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when ambient temperature > 100.0C";
                    TL.LogMessageCrLf("TestInvalidOperation", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                catch (InvalidValueException) // Expected behaviour for bad value
                {
                    TL.LogMessage("UtilTests", "DewPoint2Humidity - InvalidValueException thrown as expected when ambient temperature > 100.0C");
                    NMatches += 1;
                }

                catch (Exception ex)
                {
                    ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when ambient temperature > 100.0C" + ", instead received: " + ex.Message;
                    TL.LogMessageCrLf("UtilTests", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                try
                {
                    result = Utl.Humidity2DewPoint(-1.0d, 25.0d);
                    ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when humidity < 0.0";
                    TL.LogMessageCrLf("TestInvalidOperation", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                catch (InvalidValueException) // Expected behaviour for bad value
                {
                    TL.LogMessage("UtilTests", "Humidity2DewPoint - InvalidValueException thrown as expected when humidity < 0.0");
                    NMatches += 1;
                }

                catch (Exception ex)
                {
                    ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when humidity < 0.0" + ", instead received: " + ex.Message;
                    TL.LogMessageCrLf("UtilTests", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                try
                {
                    result = Utl.DewPoint2Humidity(101.0d, 25.0d);
                    ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when humidity > 100.0%";
                    TL.LogMessageCrLf("TestInvalidOperation", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                catch (InvalidValueException) // Expected behaviour for bad value
                {
                    TL.LogMessage("UtilTests", "Humidity2DewPoint - InvalidValueException thrown as expected when humidity > 100.0%");
                    NMatches += 1;
                }

                catch (Exception ex)
                {
                    ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when humidity > 100.0%" + ", instead received: " + ex.Message;
                    TL.LogMessageCrLf("UtilTests", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                try
                {
                    result = Utl.DewPoint2Humidity(25.0d, -300.0d);
                    ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when ambient temperature < absolute zero";
                    TL.LogMessageCrLf("TestInvalidOperation", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                catch (InvalidValueException) // Expected behaviour for bad value
                {
                    TL.LogMessage("UtilTests", "Humidity2DewPoint - InvalidValueException thrown as expected when ambient temperature < absolute zero");
                    NMatches += 1;
                }

                catch (Exception ex)
                {
                    ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when ambient temperature < absolute zero" + ", instead received: " + ex.Message;
                    TL.LogMessageCrLf("UtilTests", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                try
                {
                    result = Utl.DewPoint2Humidity(25.0d, 101.0d);
                    ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when ambient temperature > 100.0C";
                    TL.LogMessageCrLf("TestInvalidOperation", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                catch (InvalidValueException) // Expected behaviour for bad value
                {
                    TL.LogMessage("UtilTests", "Humidity2DewPoint - InvalidValueException thrown as expected when ambient temperature > 100.0C");
                    NMatches += 1;
                }

                catch (Exception ex)
                {
                    ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when ambient temperature > 100.0C" + ", instead received: " + ex.Message;
                    TL.LogMessageCrLf("UtilTests", ErrMsg);
                    NNonMatches += 1;
                    ErrorList.Add("UtilTests" + " - " + ErrMsg);
                }

                CompareDouble("UtilTests", "metresPerSecond==>metresPerSecond", Utl.ConvertUnits(10.0d, Units.metresPerSecond, Units.metresPerSecond), 10.0d, TOLERANCE_E4);
                CompareDouble("UtilTests", "metresPerSecond==>milesPerHour", Utl.ConvertUnits(10.0d, Units.metresPerSecond, Units.milesPerHour), 22.36936d, TOLERANCE_E4);
                CompareDouble("UtilTests", "metresPerSecond==>knots", Utl.ConvertUnits(10.0d, Units.metresPerSecond, Units.knots), 19.438461718d, TOLERANCE_E4); // These are Knots and not UK or US admiralty knots, which are slightly different!
                CompareDouble("UtilTests", "milesPerHour==>metresPerSecond", Utl.ConvertUnits(10.0d, Units.milesPerHour, Units.metresPerSecond), 4.4704d, TOLERANCE_E4);
                CompareDouble("UtilTests", "knots==>metresPerSecond", Utl.ConvertUnits(10.0d, Units.knots, Units.metresPerSecond), 5.14444d, TOLERANCE_E4);

                CompareDouble("UtilTests", "Degrees K==>Degrees K", Utl.ConvertUnits(300.0d, Units.degreesKelvin, Units.degreesKelvin), 300.0d, TOLERANCE_E4);
                CompareDouble("UtilTests", "Degrees K==>Degrees F", Utl.ConvertUnits(300.0d, Units.degreesKelvin, Units.degreesFahrenheit), 80.33d, TOLERANCE_E4);
                CompareDouble("UtilTests", "Degrees K==>Degrees C", Utl.ConvertUnits(300.0d, Units.degreesKelvin, Units.degreesCelsius), 26.85d, TOLERANCE_E4);
                CompareDouble("UtilTests", "Degrees F==>Degrees K", Utl.ConvertUnits(70.0d, Units.degreesFahrenheit, Units.degreesKelvin), 294.26d, TOLERANCE_E4);
                CompareDouble("UtilTests", "Degrees C==>Degrees K", Utl.ConvertUnits(20.0d, Units.degreesCelsius, Units.degreesKelvin), 293.15d, TOLERANCE_E4);

                CompareDouble("UtilTests", "hPa==>hPa", Utl.ConvertUnits(1000.0d, Units.hPa, Units.hPa), 1000.0d, TOLERANCE_E4);
                CompareDouble("UtilTests", "hPa==>mBar", Utl.ConvertUnits(1000.0d, Units.hPa, Units.mBar), 1000.0d, TOLERANCE_E4);
                CompareDouble("UtilTests", "hPa==>mmHg", Utl.ConvertUnits(1000.0d, Units.hPa, Units.mmHg), 750.0615d, TOLERANCE_E4);
                CompareDouble("UtilTests", "hPa==>inHg", Utl.ConvertUnits(1000.0d, Units.hPa, Units.inHg), 29.53d, TOLERANCE_E4);
                CompareDouble("UtilTests", "mBar==>hPa", Utl.ConvertUnits(1000.0d, Units.mBar, Units.hPa), 1000.0d, TOLERANCE_E4);
                CompareDouble("UtilTests", "mmHg==>hPa", Utl.ConvertUnits(800.0d, Units.mmHg, Units.hPa), 1066.58d, TOLERANCE_E4);
                CompareDouble("UtilTests", "inHg==>hPa", Utl.ConvertUnits(30.0d, Units.inHg, Units.hPa), 1015.92d, TOLERANCE_E4);

                CompareDouble("UtilTests", "mmPerHour==>mmPerHour", Utl.ConvertUnits(100.0d, Units.mmPerHour, Units.mmPerHour), 100.0d, TOLERANCE_E4);
                CompareDouble("UtilTests", "mmPerHour==>inPerHour", Utl.ConvertUnits(100.0d, Units.mmPerHour, Units.inPerHour), 3.937d, TOLERANCE_E4);
                CompareDouble("UtilTests", "inPerHour==>mmPerHour", Utl.ConvertUnits(10.0d, Units.inPerHour, Units.mmPerHour), 254.0d, TOLERANCE_E4);

                CompareDouble("UtilTests", "ConvertPressure 0.0 ==> 0.0", Utl.ConvertPressure(1000.0d, 0.0d, 0.0d), 1000.0d, TOLERANCE_E4);
                CompareDouble("UtilTests", "ConvertPressure 3000.0 ==> 3000.0", Utl.ConvertPressure(1000.0d, 3000.0d, 3000.0d), 1000.0d, TOLERANCE_E4);
                CompareDouble("UtilTests", "ConvertPressure 0.0 ==> 3000.0", Utl.ConvertPressure(1000.0d, 0.0d, 3000.0d), 691.92d, TOLERANCE_E4);
                CompareDouble("UtilTests", "ConvertPressure 3000.0 ==> 0.0", Utl.ConvertPressure(700.0d, 3000.0d, 0.0d), 1011.68d, TOLERANCE_E4);
                CompareDouble("UtilTests", "ConvertPressure 3000.0 ==> 1000.0", Utl.ConvertPressure(700.0d, 3000.0d, 1000.0d), 897.35d, TOLERANCE_E4);

                TestUnitsInvalidOperation(Utl, Units.metresPerSecond, Units.degreesCelsius);
                TestUnitsInvalidOperation(Utl, Units.metresPerSecond, Units.hPa);
                TestUnitsInvalidOperation(Utl, Units.metresPerSecond, Units.mmPerHour);

                TestUnitsInvalidOperation(Utl, Units.degreesCelsius, Units.milesPerHour);
                TestUnitsInvalidOperation(Utl, Units.degreesCelsius, Units.mBar);
                TestUnitsInvalidOperation(Utl, Units.degreesCelsius, Units.inPerHour);

                TestUnitsInvalidOperation(Utl, Units.mmHg, Units.knots);
                TestUnitsInvalidOperation(Utl, Units.mmHg, Units.degreesFahrenheit);
                TestUnitsInvalidOperation(Utl, Units.mmHg, Units.mmPerHour);

                TestUnitsInvalidOperation(Utl, Units.inPerHour, Units.metresPerSecond);
                TestUnitsInvalidOperation(Utl, Units.inPerHour, Units.degreesKelvin);
                TestUnitsInvalidOperation(Utl, Units.inPerHour, Units.hPa);






                try
                {
                    Utl.Dispose();
                    TL.LogMessage("UtilTests", "ASCOM.Utilities.Dispose, Disposed OK");
                }
                catch (Exception ex)
                {
                    LogException("UtilTests", "ASCOM.Utilities.Dispose Exception: " + ex.ToString());
                }

                if (!Is64Bit)
                {
                    try
                    {
                        Marshal.ReleaseComObject(DrvHlpUtil);
                        TL.LogMessage("UtilTests", "Helper Util.Release OK");
                    }
                    catch (Exception ex)
                    {
                        LogException("UtilTests", "Helper Util.Release Exception: " + ex.ToString());
                    }
                }

                Utl = null;
                DrvHlpUtil = null;

                TL.LogMessage("UtilTests", "Finished");
                TL.BlankLine();
            }

            catch (Exception ex)
            {
                LogException("UtilTests", "Exception: " + ex.ToString());
            }

        }

        private void TestUnitsInvalidOperation(Util Util, Units FromUnit, Units ToUnit)
        {
            double result;
            string ErrMsg;

            try
            {
                result = Util.ConvertUnits(0.0d, FromUnit, ToUnit);
                ErrMsg = "##### InvalidOperationException not thrown - FromUnit: " + FromUnit.ToString() + ", ToUnit: " + ToUnit.ToString();
                TL.LogMessageCrLf("TestInvalidOperation", ErrMsg);
                NNonMatches += 1;
                ErrorList.Add("TestInvalidOperation" + " - " + ErrMsg);
            }

            catch (InvalidOperationException) // Expected behaviour for mismatched types of unit
            {
                TL.LogMessage("TestInvalidOperation", "InvalidOperationException thrown as expected for FromUnit: " + FromUnit.ToString() + ", ToUnit: " + ToUnit.ToString());
                NMatches += 1;
            }

            catch (Exception ex)
            {
                ErrMsg = "##### InvalidOperationException not thrown - FromUnit: " + FromUnit.ToString() + ", ToUnit: " + ToUnit.ToString() + ", instead received: " + ex.Message;
                TL.LogMessageCrLf("TestInvalidOperation", ErrMsg);
                NNonMatches += 1;
                ErrorList.Add("TestInvalidOperation" + " - " + ErrMsg);
            }
        }

        private void CheckArray(object InputObject)
        {
            Array InputArray, ReturnArray;
            object ReturnObject;
            Type InputType, ReturnType;
            string InputElementTypeName, ReturnElementTypeName;

            InputArray = (Array)InputObject;
            InputType = InputArray.GetType();
            InputElementTypeName = InputType.GetElementType().Name;
            TL.LogMessage("UtilTests", "Input array Type: " + InputType.Name + ", Element Type: " + InputElementTypeName + ", Array Rank: " + InputArray.Rank + ", Array Length: " + InputArray.LongLength);

            ReturnObject = AscomUtil.ArrayToVariantArray(InputObject);
            ReturnArray = (Array)ReturnObject;
            ReturnType = ReturnArray.GetType();
            ReturnElementTypeName = ReturnType.GetElementType().Name;
            TL.LogMessage("UtilTests", "Return array Type: " + ReturnType.Name + ", Element Type: " + ReturnElementTypeName + ", Array Rank: " + ReturnArray.Rank + ", Array Length: " + ReturnArray.LongLength);

            CompareBoolean("UtilTests", "CheckArray", ReturnType.IsArray, true);
            CompareInteger("UtilTests", "CheckArray", ReturnArray.Rank, InputArray.Rank);
            Compare("UtilTests", "CheckArray", ReturnElementTypeName, typeof(object).Name);

            switch (ReturnArray.Rank)
            {
                case 1:
                    {
                        CompareInteger("UtilTests", "CheckArray", Conversions.ToInteger(ReturnArray.GetValue(ArrayCopySize - 1)), ArrayCopySize);// (int)(ArrayCopySize - 1)]), ArrayCopySize);
                        break;
                    }
                case 2:
                    {
                        CompareInteger("UtilTests", "CheckArray", Conversions.ToInteger(ReturnArray.GetValue(ArrayCopySize - 1, ArrayCopySize - 1)), ArrayCopySize);
                        break;
                    }
                case 3:
                    {
                        CompareInteger("UtilTests", "CheckArray", Conversions.ToInteger(ReturnArray.GetValue(ArrayCopySize - 1, ArrayCopySize - 1, ArrayCopySize - 1)), ArrayCopySize);
                        break;
                    }

                default:
                    {
                        LogError("UtilTests:CheckArray", "Returned array rank is outside expected range of 1..3: " + ReturnArray.Rank);
                        break;
                    }
            }

        }

        private string GetTimeZoneName()
        {
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now))
            {
                return TimeZone.CurrentTimeZone.DaylightName;
            }
            else
            {
                return TimeZone.CurrentTimeZone.StandardName;
            }
        }

        private void TimingTest(int p_NumberOfMilliSeconds, bool Is64Bit)
        {
            Action("TimingTest " + p_NumberOfMilliSeconds + "ms");
            s1 = Stopwatch.StartNew(); // Test time using new ASCOM component
            AscomUtil.WaitForMilliseconds(p_NumberOfMilliSeconds);
            s1.Stop();
            Application.DoEvents();

            if (Is64Bit)
            {
                TL.LogMessage("UtilTests - Timing", "Timer test (64bit): " + p_NumberOfMilliSeconds.ToString() + " milliseconds - ASCOM.Utillities.WaitForMilliSeconds: " + Strings.Format(s1.ElapsedTicks * 1000.0d / Stopwatch.Frequency, "0.00") + "ms");
            }
            else
            {
                Thread.Sleep(100);
                s2 = Stopwatch.StartNew(); // Test time using original Platform 5 component
                DrvHlpUtil.WaitForMilliseconds(p_NumberOfMilliSeconds);
                s2.Stop();
                TL.LogMessage("UtilTests - Timing", "Timer test: " + p_NumberOfMilliSeconds.ToString() + " milliseconds - ASCOM.Utillities.WaitForMilliSeconds: " + Strings.Format(s1.ElapsedTicks * 1000.0d / Stopwatch.Frequency, "0.00") + "ms DriverHelper.Util.WaitForMilliSeconds: " + Strings.Format(s2.ElapsedTicks * 1000.0d / Stopwatch.Frequency, "0.00") + "ms");
            }
            Application.DoEvents();
        }

        private void ScanEventLog()
        {
            EventLog ELog;
            EventLogEntryCollection Entries;
            EventLog[] EventLogs;
            string ErrorLog, MessageLog;
            StreamReader SR = null;

            bool Found;
            try
            {
                TL.LogMessage("ScanEventLog", "Start");
                EventLogs = EventLog.GetEventLogs();
                Found = false;
                foreach (EventLog EventLog in EventLogs)
                {
                    try
                    {
                        TL.LogMessage("ScanEventLog", "Found log: " + EventLog.LogDisplayName);
                        if (EventLog.LogDisplayName.ToUpperInvariant() == "ASCOM")
                            Found = true;
                    }
                    catch
                    {
                    }
                }
                TL.BlankLine();

                TL.LogMessage("ScanEventLog", "ASCOM Log entries");
                if (Found)
                {
                    ELog = new EventLog(Utilities.Global.EVENTLOG_NAME, ".", Utilities.Global.EVENT_SOURCE);
                    Entries = ELog.Entries;
                    foreach (EventLogEntry Entry in Entries)
                        TL.LogMessageCrLf("ScanEventLog", Conversions.ToString(Entry.TimeGenerated) + " " + Entry.Source + " " + Entry.EntryType.ToString() + " " + Entry.UserName + " " + Entry.InstanceId + " " + Entry.Message.Trim(['\n', '\r']));
                    TL.LogMessage("ScanEventLog", "ASCOM Log entries complete");
                    TL.BlankLine();
                }
                else
                {
                    LogError("ScanEventLog", "ASCOM event log does not exist!");
                    TL.BlankLine();
                }

                ErrorLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Utilities.Global.EVENTLOG_ERRORS;
                MessageLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Utilities.Global.EVENTLOG_MESSAGES;

                if (File.Exists(MessageLog) | File.Exists(ErrorLog))
                {
                    LogError("ScanEventLog", "Errors have occurred while writing to the ASCOM event log, please see detail earlier in this log.");
                    TL.LogMessage("", "");

                    if (File.Exists(MessageLog))
                    {
                        try
                        {
                            TL.LogMessage("ScanEventLog Found", Utilities.Global.EVENTLOG_MESSAGES);
                            SR = File.OpenText(MessageLog);

                            while (!SR.EndOfStream) // include the file
                                TL.LogMessage("ScanEventLog", SR.ReadLine());

                            TL.LogMessage("", "");
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanEventLog", "Exception: " + ex.ToString());
                            if (SR is not null) // Clean up streamreader
                            {
                                try
                                {
                                    SR.Close();
                                }
                                catch
                                {
                                }
                                try
                                {
                                    SR.Dispose();
                                }
                                catch
                                {
                                }
                                SR = null;
                            }
                        }
                    }

                    if (File.Exists(ErrorLog))
                    {
                        try
                        {
                            TL.LogMessage("ScanEventLog Found", Utilities.Global.EVENTLOG_ERRORS);
                            SR = File.OpenText(ErrorLog);

                            while (!SR.EndOfStream) // include the file
                                TL.LogMessage("ScanEventLog", SR.ReadLine());

                            TL.LogMessage("", "");
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                        }
                        catch (Exception ex)
                        {
                            LogException("ScanEventLog", "Exception: " + ex.ToString());
                            if (SR is not null) // Clean up streamreader
                            {
                                try
                                {
                                    SR.Close();
                                }
                                catch
                                {
                                }
                                try
                                {
                                    SR.Dispose();
                                }
                                catch
                                {
                                }
                                SR = null;
                            }
                        }
                    }

                }
                TL.LogMessage("", "");
            }
            catch (Exception ex)
            {
                LogException("ScanEventLog", "Exception: " + ex.ToString());
            }
        }

        private void ScanRegistrySecurity()
        {
            RegistryKey Key;
            try
            {
                Status("Scanning Registry Security");
                TL.LogMessage("RegistrySecurity", "Start");

                RegistryRights(Registry.CurrentUser, "", false);
                RegistryRights(Registry.CurrentUser, @"SOFTWARE\ASCOM", false);
                RegistryRights(Registry.ClassesRoot, "", false);
                RegistryRights(Registry.ClassesRoot, "DriverHelper.Util", false);

                RegistryRights(Registry.LocalMachine, "", false);
                RegistryRights(Registry.LocalMachine, "SOFTWARE", false);

                if (Utilities.Global.OSBits() == Bitness.Bits64) // 64bit OS so look in Wow64node for ASCOM profile store
                {
                    try
                    {
                        // List the 32bit registry
                        TL.LogMessage("RegistrySecurity", "Machine Profile Root (64bit OS - 32bit Registry)");
                        RegistryRights(Registry.LocalMachine, @"SOFTWARE\WOW6432Node\ASCOM", false);
                        RegistryRights(Registry.LocalMachine, @"SOFTWARE\WOW6432Node\ASCOM\Telescope Drivers", false);
                        RegistryRights(Registry.LocalMachine, @"SOFTWARE\WOW6432Node\ASCOM\Telescope Drivers\ASCOM.Simulator.Telescope", false);
                        Key = ASCOMRegistryAccess.OpenSubKey3264(RegistryHive.LocalMachine, Utilities.Global.REGISTRY_ROOT_KEY_NAME, false, RegWow64Options.KEY_WOW64_32KEY);
                        RecursionLevel = -1;
                        RecurseRegistrySecurity(Key);
                    }
                    catch (Exception ex)
                    {
                        LogException("RegistrySecurity", "Exception: " + ex.ToString());
                    }
                    TL.BlankLine();
                }
                else // 32 bit OS
                {
                    try
                    {
                        // List the registry (only one view on a 32bit machine)
                        TL.LogMessage("RegistrySecurity", "Machine Profile Root (32bit OS)");
                        RegistryRights(Registry.LocalMachine, @"SOFTWARE\ASCOM", false);
                        RegistryRights(Registry.LocalMachine, @"SOFTWARE\ASCOM\Telescope Drivers", false);
                        RegistryRights(Registry.LocalMachine, @"SOFTWARE\ASCOM\Telescope Drivers\ASCOM.Simulator.Telescope", false);
                        Key = Registry.LocalMachine.OpenSubKey(Utilities.Global.REGISTRY_ROOT_KEY_NAME);
                        RecursionLevel = -1;
                        RecurseRegistrySecurity(Key);
                    }
                    catch (Exception ex)
                    {
                        LogException("RegistrySecurity", "Exception: " + ex.ToString());
                    }
                }

                TL.BlankLine();
            }

            catch (Exception ex)
            {
                LogException("RegistrySecurity", "Exception: " + ex.ToString());
            }
        }

        private void RecurseRegistrySecurity(RegistryKey Key)
        {
            string[] SubKeys;
            RegistrySecurity sec;
            bool FoundFullAccess = false;
            bool debugSwitch = false;
            string builtInUsers;

            try
            {
                builtInUsers = GetBuiltInUsers().ToUpperInvariant();
                RecursionLevel += 1;

                try
                {
                    if (debugSwitch)
                        TL.LogMessage("RegistrySecurityDbg", "Entered ReadRegistryRights");
                    if (debugSwitch)
                        TL.LogMessage("RegistrySecurityRec", "Processing key: " + Key.Name.ToString());

                    if (debugSwitch)
                        TL.LogMessage("RegistrySecurityDbg", "Getting access control");
                    sec = Key.GetAccessControl();
                    if (debugSwitch)
                        TL.LogMessage("RegistrySecurityDbg", "Starting iteration of security rules");

                    foreach (RegistryAccessRule RegRule in sec.GetAccessRules(true, true, typeof(NTAccount))) // Iterate over the rule set and list them
                    {
                        if (debugSwitch)
                        {
                            TL.LogMessage("RegistrySecurityDbg", "Before printing rule");
                            TL.LogMessage("RegistrySecurityRec", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(RegRule.AccessControlType.ToString() + " " + RegRule.IdentityReference.ToString() + " " + RegRule.RegistryRights.ToString() + " / ", Interaction.IIf(Conversions.ToBoolean(RegRule.IsInherited.ToString()), "Inherited", "NotInherited")), " / "), RegRule.InheritanceFlags.ToString()), " / "), RegRule.PropagationFlags.ToString())));
                            TL.LogMessage("RegistrySecurityDbg", "After printing rule");
                        }
                        if ((RegRule.IdentityReference.ToString().ToUpperInvariant() ?? "") == (builtInUsers ?? "") & RegRule.RegistryRights == System.Security.AccessControl.RegistryRights.FullControl)
                        {
                            FoundFullAccess = true;
                        }
                        if (debugSwitch)
                            TL.LogMessage("RegistrySecurityDbg", "After testing for FullAccess");
                    }

                    if (debugSwitch)
                        TL.LogMessage("RegistrySecurityDbg", "Completed iteration of security rules");
                    if (FoundFullAccess)
                    {
                        NMatches += 1;
                        TL.LogMessage("RegistrySecurityRec", "OK - SubKey " + Key.Name + @" does have full registry access rights for BUILTIN\Users");
                    }
                    else
                    {
                        LogError("RegistrySecurityRec", "Subkey " + Key.Name + @" does not have full access rights for BUILTIN\Users!");
                    }

                    if (debugSwitch)
                        TL.LogMessage("RegistrySecurityDbg", "End of Try-Catch code");
                }
                catch (NullReferenceException)
                {
                    LogException("RegistrySecurityRec", "The subkey: " + Key.Name + " does not exist.");
                }
                catch (Exception ex)
                {
                    LogException("RegistrySecurityRec", ex.ToString());
                }
                if (debugSwitch)
                    TL.LogMessage("RegistrySecurityDbg", "Exited ReadRegistryRights");
            }

            catch (Exception ex)
            {
                LogException("RegistrySecurityRec 1", "Exception: " + ex.ToString());
            }

            try
            {
                SubKeys = Key.GetSubKeyNames();
                foreach (string SubKey in SubKeys)
                {
                    if (debugSwitch)
                        TL.LogMessage("RegistrySecurityRec", "Recursing to Profile Key: " + SubKey);
                    RecurseRegistrySecurity(Key.OpenSubKey(SubKey));
                }
            }
            catch (Exception ex)
            {
                LogException("RegistrySecurityRec 2", "Exception: " + ex.ToString());
            }
            RecursionLevel -= 1;

        }

        private void RegistryRights(RegistryKey Key, string SubKey, bool ConfirmFullAccess)
        {
            RegistrySecurity sec;
            RegistryKey sKey;
            bool foundFullAccess = false;
            bool debugSwitch = false;
            string builtInUsers;

            try
            {
                builtInUsers = GetBuiltInUsers().ToUpperInvariant();
                if (debugSwitch)
                    TL.LogMessage("RegistrySecurityDbg", "Entered ReadRegistryRights");
                TL.LogMessage("RegistryRights", Conversions.ToString(Interaction.IIf(string.IsNullOrEmpty(SubKey), Key.Name.ToString(), Key.Name.ToString() + @"\" + SubKey)));
                if (string.IsNullOrEmpty(SubKey) | (SubKey ?? "") == ASCOM_ROOT_KEY)
                {
                    sKey = Key;
                }
                else
                {
                    sKey = Key.OpenSubKey(SubKey);
                }

                if (debugSwitch)
                    TL.LogMessage("RegistryRightsDbg", "Getting access control");
                sec = sKey.GetAccessControl(); // System.Security.AccessControl.AccessControlSections.All)
                if (debugSwitch)
                    TL.LogMessage("RegistryRightsDbg", "Starting iteration of security rules");

                foreach (RegistryAccessRule RegRule in sec.GetAccessRules(true, true, typeof(NTAccount))) // Iterate over the rule set and list them
                {
                    if (debugSwitch)
                        TL.LogMessage("RegistryRightsDbg", "Before printing rule");
                    try
                    {
                        TL.LogMessage("RegistryRights", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(RegRule.AccessControlType.ToString() + " " + RegRule.IdentityReference.ToString() + " " + RegRule.RegistryRights.ToString() + " / ", Interaction.IIf(Conversions.ToBoolean(RegRule.IsInherited.ToString()), "Inherited", "NotInherited")), " / "), RegRule.InheritanceFlags.ToString()), " / "), RegRule.PropagationFlags.ToString())));
                        if (debugSwitch)
                            TL.LogMessage("RegistryRightsDbg", "After printing rule");
                    }
                    catch (Exception ex1)
                    {
                        LogException("RegistryRights", "Issue formatting registry rights: " + ex1.ToString());
                    }

                    if ((RegRule.IdentityReference.ToString().ToUpperInvariant() ?? "") == (builtInUsers ?? "") & RegRule.RegistryRights == System.Security.AccessControl.RegistryRights.FullControl)
                    {
                        foundFullAccess = true;
                    }
                    if (debugSwitch)
                        TL.LogMessage("RegistryRightsDbg", "After testing for FullAccess");
                }

                if (debugSwitch)
                    TL.LogMessage("RegistryRightsDbg", "Completed iteration of security rules");
                if (ConfirmFullAccess) // Check whether full access is available if required
                {
                    if (foundFullAccess)
                    {
                        NMatches += 1;
                        TL.BlankLine();
                        TL.LogMessage("RegistryRights", "OK - SubKey " + SubKey + @" does have full registry access rights for BUILTIN\Users");
                    }
                    else
                    {
                        LogError("RegistryRights", "Subkey " + SubKey + @" does not have full access rights for BUILTIN\Users!");
                    }
                }
                if (debugSwitch)
                    TL.LogMessage("RegistryRightsDbg", "End of Try-Catch code");
            }
            catch (NullReferenceException)
            {
                LogException("RegistryRights", "The subkey: " + Key.Name + @"\" + SubKey + " does not exist.");
            }
            catch (Exception ex)
            {
                LogException("RegistryRights", "Issue reading registry rights: " + ex.ToString());
            }
            if (debugSwitch)
                TL.LogMessage("RegistryRightsDbg", "Exited ReadRegistryRights");
            TL.BlankLine();
        }

        /// <summary>
        /// Returns the localised text name of the BUILTIN\Users group. This varies by locale so has to be derived on the users system.
        /// </summary>
        /// <returns>Localised name of the BUILTIN\Users group</returns>
        /// <remarks>This uses the WMI features and is pretty obscure - sorry, it was the only way I could find to do this! Peter</remarks>
        private string GetBuiltInUsers()
        {
            string builtInUsers;

            builtInUsers = new SecurityIdentifier("S-1-5-32-545").Translate(typeof(NTAccount)).ToString(); // S-1-5-32-545 Is the locale independent descriptor For the BUILTIN\Users group
            TL.LogMessage("GetBuiltInUsers", $@"Localised name of BUILTIN\\users group is: '{builtInUsers}'");

            return builtInUsers;
        }

        private void ScanRegistry()
        {
            RegistryKey Key;
            Status("Scanning Registry");
            // TL.LogMessage("ScanRegistry", "Start")
            if (Utilities.Global.OSBits() == Bitness.Bits64)
            {
                try
                {
                    // List the 32bit registry
                    TL.LogMessage("ScanRegistry", "Machine Profile Root (64bit OS - 32bit Registry)");
                    Key = ASCOMRegistryAccess.OpenSubKey3264(RegistryHive.LocalMachine, Utilities.Global.REGISTRY_ROOT_KEY_NAME, false, RegWow64Options.KEY_WOW64_32KEY);
                    RecursionLevel = -1;
                    RecurseRegistry(Key);
                }
                catch (Exception ex)
                {
                    LogException("ScanRegistry", "Exception: " + ex.ToString());
                }
                TL.BlankLine();

                try
                {
                    // List the 64bit registry
                    TL.LogMessage("ScanRegistry", "Machine Profile Root (64bit OS - 64bit Registry)");
                    Key = ASCOMRegistryAccess.OpenSubKey3264(RegistryHive.LocalMachine, Utilities.Global.REGISTRY_ROOT_KEY_NAME, false, RegWow64Options.KEY_WOW64_64KEY);
                    RecursionLevel = -1;
                    RecurseRegistry(Key);
                }
                catch (ProfilePersistenceException ex)
                {
                    if (Strings.InStr(ex.Message, "0x2") > 0)
                    {
                        TL.LogMessage("ScanRegistry", "Key not found");
                    }
                    else
                    {
                        LogException("ScanRegistry", "ProfilePersistenceException: " + ex.ToString());
                    }
                }
                catch (Exception ex)
                {
                    LogException("ScanRegistry", "Exception: " + ex.ToString());
                }
            }
            else // 32 bit OS
            {
                try
                {
                    // List the registry (only one view on a 32bit machine)
                    TL.LogMessage("ScanRegistry", "Machine Profile Root (32bit OS)");
                    Key = Registry.LocalMachine.OpenSubKey(Utilities.Global.REGISTRY_ROOT_KEY_NAME);
                    RecursionLevel = -1;
                    RecurseRegistry(Key);
                }
                catch (Exception ex)
                {
                    LogException("ScanRegistry", "Exception: " + ex.ToString());
                }
            }
            TL.BlankLine();
            TL.BlankLine();

            try
            {
                // List the user registry
                TL.LogMessage("ScanRegistry", "User Profile Root");
                Key = Registry.CurrentUser.OpenSubKey(Utilities.Global.REGISTRY_ROOT_KEY_NAME);
                RecursionLevel = -1;
                RecurseRegistry(Key);
            }
            catch (Exception ex)
            {
                LogException("ScanRegistry", "Exception: " + ex.ToString());
            }
            TL.BlankLine();
            TL.BlankLine();
        }

        private void RecurseRegistry(RegistryKey Key)
        {
            string[] ValueNames, SubKeys;
            string DisplayName;
            try
            {
                RecursionLevel += 1;
                ValueNames = Key.GetValueNames();
                foreach (string ValueName in ValueNames)
                {
                    if (string.IsNullOrEmpty(ValueName))
                    {
                        DisplayName = "*** Default Value ***";
                    }
                    else
                    {
                        DisplayName = ValueName;
                    }
                    TL.LogMessage("Registry Profile", Conversions.ToString(Operators.ConcatenateObject(Strings.Space(RecursionLevel * 2) + "   " + DisplayName + " = ", Key.GetValue(ValueName))));
                }
            }
            catch (Exception ex)
            {
                LogException("RecurseRegistry 1", "Exception: " + ex.ToString());
            }
            try
            {
                SubKeys = Key.GetSubKeyNames();
                foreach (string SubKey in SubKeys)
                {
                    TL.BlankLine();
                    TL.LogMessage("Registry Profile Key", Strings.Space(RecursionLevel * 2) + SubKey);
                    RecurseRegistry(Key.OpenSubKey(SubKey));
                }
            }
            catch (Exception ex)
            {
                LogException("RecurseRegistry 2", "Exception: " + ex.ToString());
            }
            RecursionLevel -= 1;
        }

        private void ScanDrives()
        {
            string[] Drives;
            DriveInfo Drive;
            try
            {
                Status("Scanning drives");
                Drives = Directory.GetLogicalDrives();
                foreach (string DriveName in Drives)
                {
                    Drive = new DriveInfo(DriveName);
                    if (Drive.IsReady)
                    {
                        TL.LogMessage("Drives", "Drive " + DriveName + " available space: " + Strings.Format(Drive.AvailableFreeSpace, "#,0.") + " bytes, capacity: " + Strings.Format(Drive.TotalSize, "#,0.") + " bytes, format: " + Drive.DriveFormat);
                    }
                    else
                    {
                        TL.LogMessage("Drives", "Skipping drive " + DriveName + " because it is not ready");
                    }
                }
                TL.LogMessage("", "");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("ScanDrives", "Exception: " + ex.ToString());
            }
        }

        private void ScanProgramFiles()
        {
            string BaseDir;
            var PathShell = new StringBuilder(260);
            try
            {
                BaseDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

                Status("Scanning ProgramFiles Directory for Helper DLLs");
                TL.LogMessage("ProgramFiles Scan", "Searching for Helper.DLL etc.");

                RecurseProgramFiles(BaseDir); // This is the 32bit path on a 32bit OS and 64bit path on a 64bit OS

                TL.BlankLine();

                // If on a 64bit OS, now scan the 32bit path

                if (IntPtr.Size == 8) // We are on a 64bit OS
                {
                    BaseDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    BaseDir = Conversions.ToString(SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILESX86, false));

                    Status("Scanning ProgramFiles(x86) Directory for Helper DLLs");
                    TL.LogMessage("ProgramFiles(x86) Scan", "Searching for Helper.DLL etc. on 32bit path");

                    RecurseProgramFiles(PathShell.ToString()); // This is the 32bit path on a 32bit OS and 64bit path on a 64bit OS

                    TL.BlankLine();
                }
            }
            catch (Exception ex)
            {
                LogException("ScanProgramFiles", "Exception: " + ex.ToString());
            }
        }

        private void RecurseProgramFiles(string Folder)
        {
            DirectoryInfo DirInfo;
            FileInfo[] FileInfos;
            DirectoryInfo[] DirInfos;
            try
            {
                DirInfo = new DirectoryInfo(Folder);
                Action(Strings.Left(Folder, 70));

                try // Get file details for files in this folder
                {
                    FileInfos = DirInfo.GetFiles();
                    foreach (FileInfo MyFile in FileInfos)
                    {
                        if (MyFile.FullName.EndsWith(@"\HELPER.DLL", StringComparison.OrdinalIgnoreCase))
                        {
                            FileDetails(Folder + @"\", "Helper.dll");
                        }
                        if (MyFile.FullName.EndsWith(@"\HELPER2.DLL", StringComparison.OrdinalIgnoreCase))
                        {
                            FileDetails(Folder + @"\", "Helper2.dll");
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    TL.LogMessage("RecurseProgramFiles 1", "UnauthorizedAccessException for directory; " + Folder);
                }
                catch (PathTooLongException)
                {
                    TL.LogMessage("RecurseProgramFiles 1", "PathTooLongException in directory; " + Folder);
                }
                catch (Exception ex)
                {
                    LogException("RecurseProgramFiles 1", "Exception: " + ex.ToString());
                }

                try // Iterate over the sub directories of this folder
                {
                    DirInfos = DirInfo.GetDirectories();
                    foreach (DirectoryInfo Directory in DirInfos)
                        RecurseProgramFiles(Directory.FullName); // Recursively process this sub directory
                }
                catch (UnauthorizedAccessException)
                {
                    TL.LogMessage("RecurseProgramFiles 2", "UnauthorizedAccessException for directory; " + Folder);
                }
                catch (PathTooLongException)
                {
                    TL.LogMessage("RecurseProgramFiles 2", "PathTooLongException in directory; " + Folder);
                }
                catch (Exception ex)
                {
                    LogException("RecurseProgramFiles 2", "Exception: " + ex.ToString());
                }

                Action("");
            }
            catch (UnauthorizedAccessException)
            {
                TL.LogMessage("RecurseProgramFiles 3", "UnauthorizedAccessException for directory; " + Folder);
            }
            catch (PathTooLongException)
            {
                TL.LogMessage("RecurseProgramFiles 3", "PathTooLongException in directory; " + Folder);
            }
            catch (Exception ex)
            {
                LogException("RecurseProgramFiles 3", "Exception: " + ex.ToString());
            }
        }

        private void ScanProfile55Files()
        {
            AllUsersFileSystemProvider ProfileStore;
            string[] Files;
            try
            {
                Status("Scanning Profile 5.5 Files");
                TL.LogMessage("Scanning Profile 5.5", "");

                ProfileStore = new AllUsersFileSystemProvider();
                Files = Directory.GetFiles(ProfileStore.BasePath); // Check that directory exists

                RecurseProfile55Files(ProfileStore.BasePath);

                TL.BlankLine();
            }
            catch (DirectoryNotFoundException)
            {
                TL.LogMessage("ScanProfileFiles", "Profile 5.5 file store not present");
                TL.BlankLine();
            }
            catch (Exception ex)
            {
                LogException("ScanProfileFiles", "Exception: " + ex.ToString());
            }
        }

        private void RecurseProfile55Files(string Folder)
        {
            string[] Files, Directories;

            try
            {
                // TL.LogMessage("Folder", Folder)
                // Process files in this directory
                Files = Directory.GetFiles(Folder);
                foreach (string MyFile in Files)
                {
                    TL.LogMessage("File", MyFile);
                    using (var sr = File.OpenText(MyFile))
                    {
                        string input;
                        input = sr.ReadLine();
                        while (input is not null)
                        {
                            TL.LogMessage("", "  " + input);
                            input = sr.ReadLine();
                        }
                        Console.WriteLine("The end of the stream has been reached.");
                    }

                }
            }
            catch (Exception ex)
            {
                LogException("RecurseProfileFiles 1", "Exception: " + ex.ToString());
            }

            try
            {
                Directories = Directory.GetDirectories(Folder);
                foreach (string Directory in Directories)
                {
                    TL.LogMessage("Directory", Directory);
                    RecurseProfile55Files(Directory);
                }
            }
            catch (Exception ex)
            {
                LogException("RecurseProfileFiles 2", "Exception: " + ex.ToString());
            }

        }

        private void ScanFrameworks()
        {
            string FrameworkPath, FrameworkFile;
            string[] FrameworkDirectories;
            var PathShell = new StringBuilder(260);

            try
            {
                Status("Scanning Frameworks");

                SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_WINDOWS, false);
                FrameworkPath = PathShell.ToString() + @"\Microsoft.NET\Framework";

                FrameworkDirectories = Directory.GetDirectories(FrameworkPath);
                foreach (string Directory in FrameworkDirectories)
                {
                    FrameworkFile = Directory + @"\mscorlib.dll";
                    FileVersionInfo FVInfo;
                    FileInfo FInfo;
                    if (File.Exists(FrameworkFile))
                    {

                        FVInfo = FileVersionInfo.GetVersionInfo(FrameworkFile);
                        FInfo = Microsoft.VisualBasic.FileIO.FileSystem.GetFileInfo(FrameworkFile);

                        TL.LogMessage("Frameworks", Directory.ToString() + " - Version: " + FVInfo.FileMajorPart + "." + FVInfo.FileMinorPart + " " + FVInfo.FileBuildPart + " " + FVInfo.FilePrivatePart);
                    }

                    else
                    {
                        TL.LogMessage("Frameworks", Directory.ToString());
                    }
                }
                TL.BlankLine();
            }
            catch (Exception ex)
            {
                LogException("Frameworks", "Exception: " + ex.ToString());
            }
        }

        private void ScanPlatform6Logs()
        {

            string[] fileList;
            var setupFiles = new List<KeyValuePair<DateTime, string>>();
            StreamReader streamReader = null;
            FileInfo fileInfo;

            try
            {
                Status("Scanning Platform 7 install logs");
                TL.LogMessage("ScanPlatform6Logs", "Starting scan");

                // Get a list of setup files in the ASCOM directory in creation date order
                fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ASCOM", "ASCOMPlatform6*.txt", SearchOption.TopDirectoryOnly);
                foreach (var foundFile in fileList)
                {
                    fileInfo = new FileInfo(foundFile);
                    setupFiles.Add(new KeyValuePair<DateTime, string>(fileInfo.LastWriteTime, foundFile));
                }

                // Get a list of VC++ log files in the ASCOM directory in creation date order
                fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ASCOM", "VcRedist*.txt", SearchOption.TopDirectoryOnly);
                foreach (var foundFile in fileList)
                {
                    fileInfo = new FileInfo(foundFile);
                    setupFiles.Add(new KeyValuePair<DateTime, string>(fileInfo.LastWriteTime, foundFile));
                }

                fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ASCOM", "ASCOM.UninstallASCOM.*.txt", SearchOption.AllDirectories);
                foreach (var foundFile in fileList)
                {
                    fileInfo = new FileInfo(foundFile);
                    setupFiles.Add(new KeyValuePair<DateTime, string>(fileInfo.LastWriteTime, foundFile));
                }

                fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ASCOM", "ASCOM.FinaliseInstall.*.txt", SearchOption.AllDirectories);
                foreach (var foundFile in fileList)
                {
                    fileInfo = new FileInfo(foundFile);
                    setupFiles.Add(new KeyValuePair<DateTime, string>(fileInfo.LastWriteTime, foundFile));
                }

                fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ASCOM", "ASCOM.InstallTemplates*.txt", SearchOption.AllDirectories);
                foreach (var foundFile in fileList)
                {
                    fileInfo = new FileInfo(foundFile);
                    setupFiles.Add(new KeyValuePair<DateTime, string>(fileInfo.LastWriteTime, foundFile));
                }

                fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ASCOM", "ASCOM.ValidatePlatform*.txt", SearchOption.AllDirectories);
                foreach (var foundFile in fileList)
                {
                    fileInfo = new FileInfo(foundFile);
                    setupFiles.Add(new KeyValuePair<DateTime, string>(fileInfo.LastWriteTime, foundFile));
                }

                fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ASCOM", "ASCOM.EarthRotationUpdate*.txt", SearchOption.AllDirectories);
                foreach (var foundFile in fileList)
                {
                    fileInfo = new FileInfo(foundFile);
                    setupFiles.Add(new KeyValuePair<DateTime, string>(fileInfo.LastWriteTime, foundFile));
                }

                fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ASCOM", "ASCOM.SetProfileACL*.txt", SearchOption.AllDirectories);
                foreach (var foundFile in fileList)
                {
                    fileInfo = new FileInfo(foundFile);
                    setupFiles.Add(new KeyValuePair<DateTime, string>(fileInfo.LastWriteTime, foundFile));
                }

                // Sort into inverse date order i.e. latest file first
                setupFiles = [.. setupFiles.OrderBy(o => DateTime.MaxValue - o.Key)];

                TL.LogMessage("ScanPlatform6Logs", "Found the following installation logs:");
                foreach (var foundFile in setupFiles)
                    TL.LogMessage("ScanPlatform6Logs", $"  Date: {foundFile.Key:dd MMM yyyy HH:mm:ss} Log: {foundFile.Value}");
                TL.BlankLine();

                // Iterate over results
                foreach (var foundFile in setupFiles)
                {
                    try
                    {
                        fileInfo = new FileInfo(foundFile.Value);
                        TL.LogMessage("InstallLog Found", string.Format("{0}, Created: {1}, Last updated: {2}", foundFile.Value, fileInfo.CreationTime.ToString("dd MMM yyyy HH:mm:ss"), fileInfo.LastWriteTime.ToString("dd MMM yyyy HH:mm:ss")));

                        streamReader = File.OpenText(foundFile.Value);

                        while (!streamReader.EndOfStream) // include the file
                            TL.LogMessage("InstallLog", streamReader.ReadLine());

                        TL.LogMessage("", "");
                        streamReader.Close();
                        streamReader.Dispose();
                        streamReader = null;
                    }
                    catch (Exception ex1)
                    {
                        LogException("ScanPlatform6Logs", "Exception 1: " + ex1.ToString());
                        if (streamReader is not null) // Clean up streamreader
                        {
                            streamReader.Close();
                            streamReader.Dispose();
                            streamReader = null;
                        }
                    }
                }
                TL.BlankLine();

                Status("Scanning Migration logs");
                TL.LogMessage("ScanMigrationLogs", "Starting scan");
                // Get an array of setup and uninstall filenames from the Temp directory
                fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ASCOM", "ASCOM.ProfileMigrationLog*.txt", SearchOption.TopDirectoryOnly);

                foreach (string tempFile in fileList) // Iterate over results
                {
                    try
                    {
                        TL.LogMessage("MigrationLog Found", tempFile);
                        fileInfo = new FileInfo(tempFile);
                        TL.LogMessage("InstallLog", "File created: " + fileInfo.CreationTime.ToString("dd MMM yyyy hh:mm:ss"));
                        TL.LogMessage("InstallLog", "File last updated: " + fileInfo.LastWriteTime.ToString("dd MMM yyyy hh:mm:ss"));
                        streamReader = File.OpenText(tempFile);

                        while (!streamReader.EndOfStream) // include the file
                            TL.LogMessage("MigrationLog", streamReader.ReadLine());

                        TL.LogMessage("", "");
                        streamReader.Close();
                        streamReader.Dispose();
                        streamReader = null;
                    }
                    catch (Exception ex2)
                    {
                        LogException("ScanPlatform6Logs", "Exception 2: " + ex2.ToString());
                        if (streamReader is not null) // Clean up streamreader
                        {
                            streamReader.Close();
                            streamReader.Dispose();
                            streamReader = null;
                        }
                    }
                }

                TL.LogMessage("ScanPlatform6Logs", "Completed scan");
                TL.BlankLine();
                TL.BlankLine();
            }
            catch (Exception ex)
            {
                LogException("ScanPlatform6Logs", "Exception: " + ex.ToString());
            }
        }

        private void ScanLogs()
        {
            const int NumLine = 30; // Number of lines to read from file to see if it is an ASCOM log

            string[] TempFiles;
            string[] SetupFiles, UninstallFiles;
            var TempFilesList = new List<string>();
            StreamReader SR = null;
            var Lines = new string[31];
            int LineCount;
            bool ASCOMFile;

            try
            {
                Status("Scanning setup logs");
                TL.LogMessage("SetupFile", "Starting scan");
                // Get an array of setup and uninstall filenames from the Temp directory
                SetupFiles = Directory.GetFiles(Path.GetFullPath(Environment.GetEnvironmentVariable("Temp")), "Setup Log*.txt", SearchOption.TopDirectoryOnly);
                UninstallFiles = Directory.GetFiles(Path.GetFullPath(Environment.GetEnvironmentVariable("Temp")), "Uninstall Log*.txt", SearchOption.TopDirectoryOnly);
                TempFilesList.AddRange(SetupFiles);
                TempFilesList.AddRange(UninstallFiles);
                TempFiles = [.. TempFilesList];

                foreach (string TempFile in TempFiles) // Iterate over results
                {
                    try
                    {
                        TL.LogMessage("SetupFile", TempFile);
                        SR = File.OpenText(TempFile);

                        // Search for word ASCOM in first part of file
                        ASCOMFile = false; // Initialise found flag
                        LineCount = 0;
                        Array.Clear(Lines, 1, NumLine); // Clear out the array ready for next run
                        while (!(LineCount == NumLine | SR.EndOfStream))
                        {
                            LineCount += 1;
                            Lines[LineCount] = SR.ReadLine();
                            if (Strings.InStr(Lines[LineCount].ToUpperInvariant(), "ASCOM") > 0)
                                ASCOMFile = true;
                            if (Strings.InStr(Lines[LineCount].ToUpperInvariant(), "CONFORM") > 0)
                                ASCOMFile = true;
                        }

                        if (ASCOMFile) // This is an ASCOM setup so list it
                        {

                            for (int i = 1; i <= NumLine; i++) // Include the lines read earlier
                                TL.LogMessage("SetupFile", Lines[i]);

                            while (!SR.EndOfStream) // include the rest of the file
                                TL.LogMessage("SetupFile", SR.ReadLine());
                        }
                        TL.LogMessage("", "");
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                    }
                    catch (Exception ex1)
                    {
                        LogException("SetupFile", "Exception 1: " + ex1.ToString());
                        if (SR is not null) // Clean up streamreader
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                        }
                    }
                }
                TL.BlankLine();
                TL.LogMessage("SetupFile", "Completed scan");
                TL.BlankLine();
            }
            catch (Exception ex2)
            {
                LogException("SetupFile", "Exception 2: " + ex2.ToString());
            }
        }

        private void ScanCOMRegistration()
        {
            try
            {
                Status("Scanning Registry");
                TL.LogMessage("COMRegistration", ""); // Report COM registration

                // Original Platform 5 helpers
                GetCOMRegistration("DriverHelper.Chooser");
                GetCOMRegistration("DriverHelper.Profile");
                GetCOMRegistration("DriverHelper.Serial");
                GetCOMRegistration("DriverHelper.Timer");
                GetCOMRegistration("DriverHelper.Util");
                GetCOMRegistration("DriverHelper2.Util");

                // Platform 5 helper support components
                GetCOMRegistration("DriverHelper.ChooserSupport");
                GetCOMRegistration("DriverHelper.ProfileAccess");
                GetCOMRegistration("DriverHelper.SerialSupport");

                // Utilities
                GetCOMRegistration("ASCOM.Utilities.ASCOMProfile");
                GetCOMRegistration("ASCOM.Utilities.Chooser");
                GetCOMRegistration("ASCOM.Utilities.KeyValuePair");
                GetCOMRegistration("ASCOM.Utilities.Profile");
                GetCOMRegistration("ASCOM.Utilities.Serial");
                GetCOMRegistration("ASCOM.Utilities.Timer");
                GetCOMRegistration("ASCOM.Utilities.TraceLogger");
                GetCOMRegistration("ASCOM.Utilities.Util");

                // Astrometry
                GetCOMRegistration("ASCOM.Astrometry.Kepler.Ephemeris");
                GetCOMRegistration("ASCOM.Astrometry.NOVAS.NOVAS2COM");
                GetCOMRegistration("ASCOM.Astrometry.NOVAS.NOVAS3");
                GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.Earth");
                GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.Planet");
                GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.PositionVector");
                GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.Site");
                GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.Star");
                GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.VelocityVector");
                GetCOMRegistration("ASCOM.Astrometry.Transform.Transform");

                // Get COM registration for all registered devices 
                var PR = new Profile();
                ArrayList DeviceTypes;
                ArrayList Devices;
                DeviceTypes = PR.RegisteredDeviceTypes;
                TL.LogMessage("DriverCOMRegistration", "Start of process");

                foreach (string DeviceType in DeviceTypes)
                {
                    TL.LogMessage("DriverCOMRegistration", "Starting " + DeviceType + " device type");
                    Devices = PR.RegisteredDevices(DeviceType);
                    foreach (KeyValuePair Device in Devices)
                        GetCOMRegistration(Device.Key);
                    TL.BlankLine();

                }
                TL.LogMessage("DriverCOMRegistration", "Completed process");
                PR.Dispose();
                PR = null;

                // Exceptions
                GetCOMRegistration("ASCOM.DriverException");
                GetCOMRegistration("ASCOM.InvalidOperationException");
                GetCOMRegistration("ASCOM.InvalidValueException");
                GetCOMRegistration("ASCOM.MethodNotImplementedException");
                GetCOMRegistration("ASCOM.NotConnectedException");
                GetCOMRegistration("ASCOM.NotImplementedException");
                GetCOMRegistration("ASCOM.ParkedException");
                GetCOMRegistration("ASCOM.PropertyNotImplementedException");
                GetCOMRegistration("ASCOM.SlavedException");
                GetCOMRegistration("ASCOM.ValueNotSetException");

                TL.LogMessage("", "");
            }
            catch (Exception ex)
            {
                LogException("ScanCOMRegistration", "Exception: " + ex.ToString());
            }
        }

        private void ScanForHelperHijacking()
        {
            // Scan files on 32 and 64bit systems
            TL.LogMessage("HelperHijacking", "");
            this.CheckCOMRegistration("DriverHelper.Chooser", "Helper.dll", Bitness.Bits32);
            this.CheckCOMRegistration("DriverHelper.Profile", "Helper.dll", Bitness.Bits32);
            this.CheckCOMRegistration("DriverHelper.Serial", "Helper.dll", Bitness.Bits32);
            this.CheckCOMRegistration("DriverHelper.Timer", "Helper.dll", Bitness.Bits32);
            this.CheckCOMRegistration("DriverHelper.Util", "Helper.dll", Bitness.Bits32);
            this.CheckCOMRegistration("DriverHelper2.Util", "Helper2.dll", Bitness.Bits32);
            TL.BlankLine();
            TL.BlankLine();
        }

        private void CheckCOMRegistration(string ProgID, string COMFile, Bitness Bitness)
        {
            RegistryKey RKeyProgID, RKeyCLSIDValue, RKeyCLSID, RKeyInprocServer32;
            string CLSID, InprocServer32, ASCOMPath;
            var RegAccess = new RegistryAccess();
            var PathShell = new StringBuilder(260);
            string RegSvr32Path;
            Process P;
            ProcessStartInfo Info;
            MsgBoxResult Result;

            try
            {
                RKeyProgID = Registry.ClassesRoot.OpenSubKey(ProgID);
                if (RKeyProgID is not null) // Found ProgId
                {
                    RKeyCLSIDValue = RKeyProgID.OpenSubKey("CLSID");
                    if (RKeyCLSIDValue is not null) // Found CLSID Key
                    {
                        CLSID = RKeyCLSIDValue.GetValue("").ToString(); // Get the CLSID

                        switch (Utilities.Global.ApplicationBits())
                        {
                            case Bitness.Bits32: // We are a 32bit application so look in the default registry position
                                {
                                    RKeyCLSID = Registry.ClassesRoot.OpenSubKey(@"CLSID\" + CLSID, false);
                                    break;
                                }
                            case Bitness.Bits64: // We are a 64bit application so look in the 32bit registry section
                                {
                                    switch (Bitness)
                                    {
                                        case Bitness.Bits32: // Open the 32bit registry
                                            {
                                                RKeyCLSID = RegAccess.OpenSubKey3264(RegistryHive.ClassesRoot, @"CLSID\" + CLSID, false, RegWow64Options.KEY_WOW64_32KEY);
                                                break;
                                            }
                                        case Bitness.Bits64: // Open the 64bit registry
                                            {
                                                RKeyCLSID = Registry.ClassesRoot.OpenSubKey(@"CLSID\" + CLSID, false);
                                                break;
                                            }

                                        default:
                                            {
                                                RKeyCLSID = null;
                                                this.Compare("HelperHijacking", "Requested Bitness", Utilities.Global.ApplicationBits().ToString(), Bitness.Bits64.ToString());
                                                break;
                                            }
                                    }

                                    break;
                                }

                            default:
                                {
                                    this.Compare("HelperHijacking", "Requested Bitness", Utilities.Global.ApplicationBits().ToString(), Bitness.Bits64.ToString());
                                    RKeyCLSID = null;
                                    break;
                                }
                        }
                        if (RKeyCLSID is not null) // CLSID value does exist
                        {
                            RKeyInprocServer32 = RKeyCLSID.OpenSubKey("InprocServer32", false);
                            if (RKeyInprocServer32 is not null)
                            {
                                InprocServer32 = Conversions.ToString(RKeyInprocServer32.GetValue("", false));
                                ASCOMPath = GetASCOMPath() + COMFile;
                                if ((ASCOMPath ?? "") != (InprocServer32 ?? "")) // We have a hijacked COM registration so offer to re-register the correct file
                                {

                                    Utilities.Global.LogEvent("Diagnostics:HelperHijacking", "Hijacked COM Setting for: " + ProgID + ", Actual Path: " + InprocServer32 + ", Expected Path: " + ASCOMPath, EventLogEntryType.Error, EventLogErrors.DiagnosticsHijackedCOMRegistration, "");
                                    TL.LogMessage("HelperHijacking", "ISSUE, " + ProgID + " has been hijacked");
                                    TL.LogMessage("HelperHijacking", "  Actual Path: " + InprocServer32 + ", Expected Path: " + ASCOMPath);
                                    Result = Interaction.MsgBox("The COM component \"" + ProgID + "\" is not properly registered. Would you like to fix this? (Strongly recommend Yes!)", MsgBoxStyle.YesNo | MsgBoxStyle.Critical, "COM Registration Issue Detected");

                                    if (Result == MsgBoxResult.Yes)
                                    {
                                        TL.LogMessage("HelperHijacking", "  Fixing COM Registration");
                                        if (Utilities.Global.OSBits() == Bitness.Bits64) // We are running on a 64bit OS
                                        {
                                            switch (Bitness)
                                            {
                                                case Bitness.Bits32: // Run the 32bit Regedit
                                                    {
                                                        SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_SYSTEMX86, false); // Get the 32bit system directory
                                                        break;
                                                    }
                                                case Bitness.Bits64: // Run the 64bit Regedit
                                                    {
                                                        SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_SYSTEM, false); // Get the 64bit system directory
                                                        break;
                                                    }
                                            }
                                        }
                                        else // We are running on a 32bit OS
                                        {
                                            SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_SYSTEM, false);
                                        } // Get the system directory
                                        RegSvr32Path = PathShell.ToString() + @"\RegSvr32.exe"; // Construct the full path to RegSvr32.exe
                                        Info = new ProcessStartInfo()
                                        {
                                            FileName = RegSvr32Path, // Populate the ProcessStartInfo with the full path to RegSvr32.exe 
                                            Arguments = "/s \"" + ASCOMPath + "\"" // And the start parameter specifying the file to COM register
                                        };
                                        TL.LogMessage("HelperHijacking", "  RegSvr32 Path: \"" + RegSvr32Path + "\", COM Path: \"" + ASCOMPath + "\"");

                                        P = new Process() { StartInfo = Info }; // Set the start info
                                                                                // Create the process
                                        P.Start(); // Start the process and wait for it to finish
                                        TL.LogMessage("HelperHijacking", "  Started registration");
                                        P.WaitForExit();
                                        TL.LogMessage("HelperHijacking", "  Finished registration, Return code: " + P.ExitCode); // ASCOM-217 changed + to & concatenator
                                        P.Dispose();

                                        // Reread the COM information to check whether it is now fixed
                                        InprocServer32 = Conversions.ToString(RKeyInprocServer32.GetValue("", false));
                                        ASCOMPath = GetASCOMPath() + COMFile;
                                        if ((ASCOMPath ?? "") != (InprocServer32 ?? "")) // We have a hijacked COM registration so offer to re-register the correct file
                                        {
                                            Interaction.MsgBox("Diagnostics was NOT able to fix the issue. Please report this on ASCOM-Talk", MsgBoxStyle.Exclamation, "Issue Remains");
                                            LogError("HelperHijacking", "  Unable to fix " + ProgID + " registration");
                                        }
                                        else
                                        {
                                            Interaction.MsgBox("Diagnostics successfully FIXED the issue", MsgBoxStyle.Information, "Issue Fixed");
                                            TL.LogMessage("HelperHijacking", "  Successfully fixed " + ProgID + " registration");
                                            NMatches += 1;
                                        }
                                    }
                                    else
                                    {
                                        TL.LogMessage("HelperHijacking", "  Not fixing COM registration, no action taken");
                                    }
                                }
                                else // Matches expected value so has not been hijacked
                                {
                                    TL.LogMessage("HelperHijacking", "OK, " + ProgID + " has not been hijacked");
                                }
                            }
                            else
                            {
                                LogError("HelperHijacking", @"Unable to find registered CLSID\InprocServer32: " + CLSID + "InprocServer32");
                            }
                        }
                        else // CLSID value does not exist
                        {
                            LogError("HelperHijacking", "Unable to find registered CLSID: " + CLSID);
                        }
                    }
                    else // CLSID is missing
                    {
                        LogError("HelperHijacking", @"Unable to find ProgID\CLSID: " + ProgID + @"\CLSID");
                    }
                }
                else // Cannot find ProgID so give error message
                {
                    LogError("HelperHijacking", "Unable to find registered ProgID: " + ProgID);
                }
            }
            catch (Exception ex)
            {
                LogException("HelperHijacking", "Exception: " + ex.ToString());
            }

        }

        private void LogError(string Section, string Message)
        {
            NNonMatches += 1;
            ErrorList.Add(Section + " - " + Message);
            TL.LogMessage(Section, Message);
        }

        private void ScanGac()
        {
            IAssemblyEnum ae;
            Assembly ass;
            SortedList<string, string> AssemblyNames;
            AssemblyName assname;
            List<string> AscomGACPaths;
            FileVersionInfo FVInfo = null;
            string MyName, FVer;
            string FileLocation;
            FileVersionInfo FileVer;
            var DiagnosticsVersion = new Version("0.0.0.0");
            Version FileVersion;
            int VersionComparison;
            Uri assemblyURI;
            string localPath;

            try
            {
                Status("Scanning Assemblies");
                AssemblyNames = new SortedList<string, string>();

                TL.LogMessage("Assemblies", "Assemblies registered in the GAC");
                ae = AssemblyCache.CreateGACEnum(); // Get an enumerator for the GAC assemblies

                while (AssemblyCache.GetNextAssembly(ae, out IAssemblyName an) == 0) // Enumerate the assemblies
                {
                    try
                    {
                        assname = GetAssemblyName(an);
                        AssemblyNames.Add(assname.FullName, assname.Name); // Convert the fusion representation to a standard AssemblyName and get its full name
                    }
                    catch (Exception)
                    {
                        // Ignore an exceptions here due to duplicate names, these are all MS assemblies
                    }
                }

                AscomGACPaths = new List<string>();

                foreach (KeyValuePair<string, string> AssemblyName in AssemblyNames) // Process each assembly in turn
                {
                    try
                    {
                        if (Strings.InStr(AssemblyName.Key, "ASCOM") > 0) // Extra information for ASCOM files
                        {
                            TL.LogMessage("Assemblies", AssemblyName.Value);
                            ass = Assembly.Load(AssemblyName.Key);
                            Utilities.Global.AssemblyInfo(TL, AssemblyName.Value, ass); // Get file version and other information

                            try
                            {
                                assemblyURI = new Uri(ass.GetName().CodeBase);
                                localPath = assemblyURI.LocalPath;
                                if (localPath.ToUpperInvariant().Contains(@"\ASCOM.DRIVERACCESS\6") | localPath.ToUpperInvariant().Contains(@"\ASCOM.UTILITIES\6") | localPath.ToUpperInvariant().Contains("ASCOM.ALPACASHAREDRESOURCES") | localPath.ToUpperInvariant().Contains(@"\ASCOM.ATTRIBUTES\6") | localPath.ToUpperInvariant().Contains(@"\ASCOM.CACHE") | localPath.ToUpperInvariant().Contains(@"\ASCOM.CONTROLS") | localPath.ToUpperInvariant().Contains(@"\ASCOM.DRIVERACCESS\6") | localPath.ToUpperInvariant().Contains(@"\ASCOM.EXCEPTIONS\6") | localPath.ToUpperInvariant().Contains(@"\ASCOM.INTERNAL.EXTENSIONS\6") | localPath.ToUpperInvariant().Contains(@"\ASCOM.INTERNAL.FUSIONLIB\6") | localPath.ToUpperInvariant().Contains(@"\ASCOM.NEWTONSOFT.JSON\6") | localPath.ToUpperInvariant().Contains(@"\ASCOM.SETTINGSPROVIDER\6") | localPath.ToUpperInvariant().Contains(@"\ASCOM.UTILITIES.SUPPORT\6") | localPath.ToUpperInvariant().Contains(@"\ASCOM.UTILITIES.VIDEO") | localPath.ToUpperInvariant().Contains(@"\ASCOM.ASTROMETRY\6") | localPath.ToUpperInvariant().Contains(@"\ASCOM.DEVICEINTERFACES\6"))
                                {
                                    AscomGACPaths.Add(localPath);
                                }
                                FileDetails(Path.GetDirectoryName(localPath) + @"\", Path.GetFileName(localPath));
                            }
                            catch (Exception)
                            {

                            }
                        }

                        else
                        {
                            TL.LogMessage("Assemblies", AssemblyName.Key);
                        } // Non-ASCOM assembly
                    }
                    catch (Exception ex)
                    {
                        LogException("Assemblies", "Exception 2: " + ex.ToString());
                    }
                }

                try
                {
                    ass = Assembly.GetExecutingAssembly();
                    MyName = ass.FullName;
                    FileLocation = ass.Location;
                    if (string.IsNullOrEmpty(FileLocation))
                    {
                        TL.LogMessage("FileDetails", MyName + "Assembly location is missing, cannot determine file version");
                    }
                    else
                    {
                        FileVer = FileVersionInfo.GetVersionInfo(FileLocation);
                        if (FileVer is null)
                        {
                            TL.LogMessage("FileDetails", MyName + " File version object is null, cannot determine file version number");
                        }
                        else
                        {
                            FVer = FileVer.FileVersion;
                            if (!string.IsNullOrEmpty(FVer))
                            {
                                DiagnosticsVersion = new Version(FVer);
                            }
                            else
                            {
                                TL.LogMessage("FileDetails", MyName + " File version string is null or empty, cannot determine file version");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogException("FileDetails", "Exception EX2: " + ex.ToString());
                }
                TL.LogMessage("", "");

                foreach (string AscomGACPath in AscomGACPaths)
                {
                    try
                    {
                        FVInfo = FileVersionInfo.GetVersionInfo(AscomGACPath);
                        TL.LogMessage("GACFileVersion", FVInfo.FileName);
                        FileVersion = new Version(FVInfo.FileVersion);
                        VersionComparison = DiagnosticsVersion.CompareTo(FileVersion);

                        switch (VersionComparison)
                        {
                            case var @case when @case < 0:
                                {
                                    TL.LogMessage("GACFileVersion", "   ##### Diagnostics is older than this assembly! Diagnostics: " + DiagnosticsVersion.ToString() + ", Assembly: " + FileVersion.ToString());
                                    NNonMatches += 1;
                                    ErrorList.Add("Diagnostics is older than this assembly! Diagnostics: " + DiagnosticsVersion.ToString() + ", Assembly: " + FileVersion.ToString() + " " + AscomGACPath);
                                    break;
                                }
                            case 0:
                                {
                                    TL.LogMessage("GACFileVersion", "   Diagnostics is the same version as this assembly! Diagnostics: " + DiagnosticsVersion.ToString() + ", Assembly: " + FileVersion.ToString());
                                    NMatches += 1;
                                    break;
                                }
                            case var case1 when case1 > 0:
                                {
                                    TL.LogMessage("GACFileVersion", "   ##### Diagnostics is newer than this assembly! Diagnostics: " + DiagnosticsVersion.ToString() + ", Assembly: " + FileVersion.ToString());
                                    NNonMatches += 1;
                                    ErrorList.Add("Diagnostics is newer than this assembly! Diagnostics: " + DiagnosticsVersion.ToString() + ", Assembly: " + FileVersion.ToString() + " " + AscomGACPath);
                                    break;
                                }
                        }
                    }
                    catch (FormatException)
                    {
                        LogException("GACFileVersion", "  ##### File version is in an invalid format: " + FVInfo.FileVersion);
                    }
                }

                TL.LogMessage("", "");
            }
            catch (Exception ex)
            {
                LogException("ScanGac", "Exception: " + ex.ToString());
            }
        }

        private AssemblyName GetAssemblyName(IAssemblyName nameRef)
        {
            var AssName = new AssemblyName();
            try
            {
                AssName.Name = AssemblyCache.GetName(nameRef);
                AssName.Version = AssemblyCache.GetVersion(nameRef);
                AssName.CultureInfo = AssemblyCache.GetCulture(nameRef);
                AssName.SetPublicKeyToken(AssemblyCache.GetPublicKeyToken(nameRef));
            }
            catch (Exception ex)
            {
                LogException("GetAssemblyName", "Exception: " + ex.ToString());
            }
            return AssName;
        }

        private void ScanDeveloperFiles()
        {
            string ASCOMPath = @"C:\Program Files\ASCOM\Platform 7 Developer Components\"; // Default location
            var PathShell = new StringBuilder(260);
            string ASCOMPathComponents5, ASCOMPathComponents55, ASCOMPathComponents7, ASCOMPathDocs, ASCOMPathInstallerGenerator, ASCOMPathResources;

            try
            {
                Status("Scanning Developer Files");

                if (IntPtr.Size == 8) // We are on a 64bit OS so look in the 32bit locations for files
                {
                    SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILESX86, false);
                    ASCOMPath = PathShell.ToString() + @"\ASCOM\Platform 7 Developer Components\";
                }
                else // 32bit system so look in the normal Program Files place
                {
                    ASCOMPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + @"\ASCOM\Platform 7 Developer Components\""";
                }
            }
            catch (ProfilePersistenceException ex)
            {
                if (Strings.InStr(ex.Message, "0x2") > 0)
                {
                    TL.LogMessage("Developer Files", "Installation registry key not present");
                }
                else
                {
                    LogException("ScanDeveloperFiles", "ProfilePersistenceException: " + ex.ToString());
                }
            }
            catch (Exception ex)
            {
                LogException("ScanDeveloperFiles", "Registry Exception: " + ex.ToString());
            }

            try
            {

                if (Directory.Exists(ASCOMPath))
                {
                    ASCOMPathComponents5 = ASCOMPath + @"Components\Platform5\";
                    ASCOMPathComponents55 = ASCOMPath + @"Components\Platform55\";
                    ASCOMPathComponents7 = ASCOMPath + @"Components\Platform7\";
                    ASCOMPathDocs = ASCOMPath + @"Developer Documentation\";
                    ASCOMPathInstallerGenerator = ASCOMPath + @"Installer Generator\";
                    ASCOMPathResources = ASCOMPath + @"Installer Generator\Resources\";
                    TL.LogMessage("Developer Files", "Start of scan");
                    FileDetails(ASCOMPathComponents5, "ASCOM.Exceptions.dll");
                    FileDetails(ASCOMPathComponents55, "ASCOM.Astrometry.dll");
                    FileDetails(ASCOMPathComponents55, "ASCOM.Attributes.dll");
                    FileDetails(ASCOMPathComponents55, "ASCOM.DriverAccess.dll");
                    FileDetails(ASCOMPathComponents55, "ASCOM.Exceptions.dll");
                    FileDetails(ASCOMPathComponents55, "ASCOM.Utilities.dll");
                    FileDetails(ASCOMPathComponents7, "ASCOM.Astrometry.dll");
                    FileDetails(ASCOMPathComponents7, "ASCOM.Attributes.dll");
                    FileDetails(ASCOMPathComponents7, "ASCOM.Controls.dll");
                    FileDetails(ASCOMPathComponents7, "ASCOM.DeviceInterfaces.dll");
                    FileDetails(ASCOMPathComponents7, "ASCOM.DriverAccess.dll");
                    FileDetails(ASCOMPathComponents7, "ASCOM.Exceptions.dll");
                    FileDetails(ASCOMPathComponents7, "ASCOM.Internal.Extensions.dll");
                    FileDetails(ASCOMPathComponents7, "ASCOM.SettingsProvider.dll");
                    FileDetails(ASCOMPathComponents7, "ASCOM.Utilities.dll");
                    FileDetails(ASCOMPathDocs, "Algorithms.pdf");
                    FileDetails(ASCOMPathDocs, "Bug72T-sm.jpg");
                    FileDetails(ASCOMPathDocs, "NOVAS_C3.1_Guide.pdf");
                    FileDetails(ASCOMPathDocs, "Platform 6 Client-Driver Interaction V2.pdf");
                    FileDetails(ASCOMPathDocs, "Platform Evolution.pdf");
                    FileDetails(ASCOMPathDocs, "PlatformDeveloperHelp.chm");
                    FileDetails(ASCOMPathDocs, "Script56.chm");
                    if (File.Exists(ASCOMPathDocs + "Templates.html"))
                        FileDetails(ASCOMPathDocs, "Templates.html");
                    FileDetails(ASCOMPathDocs, "tip.gif");
                    FileDetails(ASCOMPathDocs, "wsh-56.chm");
                    FileDetails(ASCOMPathInstallerGenerator, "InstallerGen.exe");
                    FileDetails(ASCOMPathInstallerGenerator, "InstallerGen.pdb");
                    FileDetails(ASCOMPathInstallerGenerator, "Microsoft.Samples.WinForms.Extras.dll");
                    FileDetails(ASCOMPathInstallerGenerator, "Microsoft.Samples.WinForms.Extras.pdb");
                    FileDetails(ASCOMPathInstallerGenerator, "TemplateSubstitutionParameters.txt");
                    FileDetails(ASCOMPathResources, "CreativeCommons.txt");
                    FileDetails(ASCOMPathResources, "DriverInstallTemplate.iss");
                    FileDetails(ASCOMPathResources, "WizardImage.bmp");
                }
                else
                {
                    TL.LogMessage("Developer Files", "Files not installed");
                }
            }
            catch (Exception ex)
            {
                LogException("ScanDeveloperFiles", "File Exception: " + ex.ToString());
            }
            TL.BlankLine();
            Status("");

        }

        private void ScanPlatformFiles(string ASCOMPath)
        {
            try
            {
                Status("Scanning Platform Files");

                ScanDirectory(ASCOMPath, SearchOption.TopDirectoryOnly);
                ScanDirectory(ASCOMPath + ".net", SearchOption.AllDirectories);
                ScanDirectory(ASCOMPath + "Interface", SearchOption.AllDirectories);
                ScanDirectory(ASCOMPath + "Platform", SearchOption.AllDirectories);
                ScanDirectory(ASCOMPath + "Astrometry", SearchOption.AllDirectories);
                ScanDirectory(ASCOMPath + "VideoUtilities", SearchOption.AllDirectories);
            }

            catch (Exception ex)
            {
                LogException("ScanFiles", "Exception: " + ex.ToString());
            }
            TL.BlankLine();

        }

        private void ScanDirectory(string DirectoryName, SearchOption Search)
        {
            string[] FullFileNames;

            TL.LogMessage("ScanFiles", "Scanning directory: " + DirectoryName + ", " + Search.ToString());
            Status("Scanning directory " + DirectoryName);

            try
            {
                FullFileNames = Directory.GetFiles(DirectoryName, "*.*", Search);
                foreach (string FullFileName in FullFileNames)
                    FileDetails(Path.GetDirectoryName(FullFileName) + Path.DirectorySeparatorChar, Path.GetFileName(FullFileName));
            }
            catch (Exception ex)
            {
                LogException("ScanDirectory", "Exception: " + ex.ToString());
            }
            TL.BlankLine();
        }

        private void FileDetails(string FPath, string FName)
        {
            string FullPath;
            FileAttributes Att;
            FileVersionInfo FVInfo;
            FileInfo FInfo;
            Assembly Ass;
            string AssVer = "";
            string CompareName;
            Assembly[] ReflectionAssemblies;
            string Framework = "";
            PEReader PE;

            try
            {
                FullPath = FPath + FName; // Create full filename from path and simple filename
                if (File.Exists(FullPath))
                {
                    TL.LogMessage("FileDetails", FullPath);

                    // Try to get assembly version info if present
                    try
                    {
                        Ass = Assembly.ReflectionOnlyLoadFrom(FullPath);
                        AssVer = Ass.FullName;
                        Framework = Ass.ImageRuntimeVersion;
                    }
                    catch (FileLoadException ex) // Deal with possibility that this assembly has already been loaded
                    {
                        TL.LogMessageCrLf("ErrorDiagnostics", $"A FileLoadException was thrown. Fusion log: {"\r\n"}'{ex.FusionLog}'");

                        ReflectionAssemblies = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies(); // Get list of assemblies already loaded to the reflection only context
                        CompareName = Path.GetFileNameWithoutExtension(FName);
                        foreach (Assembly ReflectionAss in ReflectionAssemblies) // Find the assembly already there and get its full name
                        {
                            if (ReflectionAss.FullName.ToUpperInvariant().Contains(CompareName.ToUpperInvariant()))
                            {
                                AssVer = ReflectionAss.FullName;
                            }
                        }

                        if (string.IsNullOrEmpty(AssVer))
                        {
                            TL.LogMessage("ErrorDiagnostics", CompareName);
                            foreach (Assembly ReflectionAss in ReflectionAssemblies) // List the assemblies already loaded
                                TL.LogMessage("ErrorDiagnostics", ReflectionAss.FullName);
                        }
                    }
                    catch (BadImageFormatException)
                    {
                        AssVer = "Not an assembly";
                    }
                    catch (Exception ex)
                    {
                        LogException("FileDetails", "Exception: " + ex.ToString());
                        AssVer = "Not an assembly: " + ex.ToString();
                    }

                    TL.LogMessage("FileDetails", "   Assembly Version:   " + AssVer);
                    TL.LogMessage("FileDetails", "   Assembly Framework: " + Framework);

                    FVInfo = FileVersionInfo.GetVersionInfo(FullPath);
                    FInfo = Microsoft.VisualBasic.FileIO.FileSystem.GetFileInfo(FullPath);

                    TL.LogMessage("FileDetails", "   File Version:       " + FVInfo.FileMajorPart + "." + FVInfo.FileMinorPart + "." + FVInfo.FileBuildPart + "." + FVInfo.FilePrivatePart);
                    TL.LogMessage("FileDetails", "   Product Version:    " + FVInfo.ProductMajorPart + "." + FVInfo.ProductMinorPart + "." + FVInfo.ProductBuildPart + "." + FVInfo.ProductPrivatePart);

                    TL.LogMessage("FileDetails", "   Description:        " + FVInfo.FileDescription);
                    TL.LogMessage("FileDetails", "   Company Name:       " + FVInfo.CompanyName);

                    TL.LogMessage("FileDetails", "   Last Write Time:    " + Conversions.ToString(File.GetLastWriteTime(FullPath)));
                    TL.LogMessage("FileDetails", "   Creation Time:      " + Conversions.ToString(File.GetCreationTime(FullPath)));

                    TL.LogMessage("FileDetails", "   File Length:        " + Strings.Format(FInfo.Length, "#,0.").Replace(ThousandsSeparator, ","));

                    Att = File.GetAttributes(FullPath);
                    TL.LogMessage("FileDetails", "   Attributes:         " + Att.ToString());
                    try
                    {
                        PE = new PEReader(FullPath, TL);
                        TL.LogMessage("FileDetails", "   .NET Assembly:      " + PE.IsDotNetAssembly());
                        TL.LogMessage("FileDetails", "   Bitness:            " + PE.BitNess.ToString());
                        TL.LogMessage("FileDetails", "   Subsystem:          " + PE.SubSystem().ToString());
                        PE.Dispose();
                        PE = (PEReader)null;
                    }
                    catch (InvalidOperationException)
                    {
                        TL.LogMessage("FileDetails", "   .NET Assembly:      Not a valid PE executable");
                    }
                    NMatches += 1;
                }
                else
                {
                    TL.LogMessage("FileDetails", "   ### Unable to find file: " + FullPath);
                    NNonMatches += 1;
                    ErrorList.Add("FileDetails - Unable to find file: " + FullPath);
                }
            }
            catch (Exception ex)
            {
                LogException("FileDetails", "### Exception: " + ex.ToString());
            }

            TL.LogMessage("", "");
        }

        private void GetCOMRegistration(string ProgID)
        {
            RegistryKey RKey;
            try
            {
                TL.LogMessage("ProgID", ProgID);
                RKey = Registry.ClassesRoot.OpenSubKey(ProgID);
                if (RKey is not null) // Registry key exists so process it
                {
                    ProcessSubKey(RKey, 1, "None");
                    RKey.Close();
                    TL.LogMessage("Finished", "");
                    NMatches += 1;
                }
                else
                {
                    TL.LogMessage("Finished", "*** ProgID " + ProgID + " not found");
                    NNonMatches += 1;
                    ErrorList.Add("GetCOMRegistration - *** ProgID " + ProgID + " not found");
                }
            }
            catch (Exception ex)
            {
                LogException("Exception", ex.ToString());
            }
            TL.LogMessage("", "");
        }

        private void ProcessSubKey(RegistryKey p_Key, int p_Depth, string p_Container)
        {
            string[] ValueNames, SubKeys;
            RegistryKey RKey;
            RegistryValueKind ValueKind;
            string Container;
            // TL.LogMessage("Start of ProcessSubKey", p_Container & " " & p_Depth)

            if (p_Depth > 12)
            {
                TL.LogMessage("RecursionTrap", "Recursion depth has exceeded 12 so terminating at this point as we may be in an infinite loop");
            }
            else
            {
                try
                {
                    ValueNames = p_Key.GetValueNames();
                    // TL.LogMessage("Start of ProcessSubKey", "Found " & ValueNames.Length & " values")
                    foreach (string ValueName in ValueNames)
                    {
                        ValueKind = p_Key.GetValueKind(ValueName);
                        switch (ValueName.ToUpperInvariant() ?? "")
                        {
                            case var @case when @case == "":
                                {
                                    TL.LogMessage("KeyValue", Conversions.ToString(Operators.ConcatenateObject(Strings.Space(p_Depth * INDENT) + "*** Default *** = ", p_Key.GetValue(ValueName))));
                                    break;
                                }
                            case "APPID":
                                {
                                    p_Container = "AppId";
                                    TL.LogMessage("KeyValue", Conversions.ToString(Operators.ConcatenateObject(Strings.Space(p_Depth * INDENT) + ValueName.ToString() + " = ", p_Key.GetValue(ValueName))));
                                    break;
                                }

                            default:
                                {
                                    switch (ValueKind)
                                    {
                                        case RegistryValueKind.String:
                                        case RegistryValueKind.ExpandString:
                                            {
                                                TL.LogMessage("KeyValue", Conversions.ToString(Operators.ConcatenateObject(Strings.Space(p_Depth * INDENT) + ValueName.ToString() + " = ", p_Key.GetValue(ValueName))));
                                                break;
                                            }
                                        case RegistryValueKind.MultiString:
                                            {
                                                TL.LogMessage("KeyValue", Conversions.ToString(Operators.ConcatenateObject(Strings.Space(p_Depth * INDENT) + ValueName.ToString() + " = ", p_Key.GetValue(ValueName, 0))));
                                                break;
                                            }
                                        case RegistryValueKind.DWord:
                                            {
                                                TL.LogMessage("KeyValue", Strings.Space(p_Depth * INDENT) + ValueName.ToString() + " = " + p_Key.GetValue(ValueName).ToString());
                                                break;
                                            }

                                        default:
                                            {
                                                TL.LogMessage("KeyValue", Conversions.ToString(Operators.ConcatenateObject(Strings.Space(p_Depth * INDENT) + ValueName.ToString() + " = ", p_Key.GetValue(ValueName))));
                                                break;
                                            }
                                    }

                                    break;
                                }
                        }
                        if (ValueKind != RegistryValueKind.MultiString) // Don't try and process these, they don't lead anywhere anyway!
                        {
                            if (Strings.Left(Conversions.ToString(p_Key.GetValue(ValueName)), 1) == "{")
                            {
                                // TL.LogMessage("ClassExpand", "Expanding " & p_Key.GetValue(ValueName))
                                switch (p_Container.ToUpperInvariant() ?? "")
                                {
                                    case "CLSID":
                                        {
                                            RKey = Registry.ClassesRoot.OpenSubKey("CLSID").OpenSubKey(Conversions.ToString(p_Key.GetValue(ValueName)));
                                            if (RKey is null) // Check in 32 bit registry on a 64bit system
                                            {
                                                RKey = Registry.ClassesRoot.OpenSubKey(@"Wow6432Node\CLSID").OpenSubKey(Conversions.ToString(p_Key.GetValue(ValueName)));
                                                if (RKey is not null)
                                                    TL.LogMessage("NewSubKey", Strings.Space(p_Depth * INDENT) + "Found under Wow6432Node");
                                            }

                                            break;
                                        }
                                    case "TYPELIB":
                                        {
                                            RKey = Registry.ClassesRoot.OpenSubKey("TypeLib").OpenSubKey(Conversions.ToString(p_Key.GetValue(ValueName)));
                                            RKey ??= Registry.ClassesRoot.OpenSubKey(@"Wow6432Node\TypeLib").OpenSubKey(Conversions.ToString(p_Key.GetValue(ValueName)));

                                            break;
                                        }
                                    case "APPID":
                                        {
                                            RKey = Registry.ClassesRoot.OpenSubKey("AppId").OpenSubKey(Conversions.ToString(p_Key.GetValue(ValueName)));
                                            RKey ??= Registry.ClassesRoot.OpenSubKey(@"Wow6432Node\AppId").OpenSubKey(Conversions.ToString(p_Key.GetValue(ValueName)));

                                            break;
                                        }

                                    default:
                                        {
                                            RKey = p_Key.OpenSubKey(Conversions.ToString(p_Key.GetValue(ValueName)));
                                            break;
                                        }
                                }

                                if (RKey is not null)
                                {
                                    if ((RKey.Name ?? "") != (p_Key.Name ?? "")) // We are in an infinite loop so kill it by setting rkey = Nothing
                                    {
                                        TL.LogMessage("NewSubKey", Conversions.ToString(Operators.ConcatenateObject(Strings.Space((p_Depth + 1) * INDENT) + p_Container + @"\", p_Key.GetValue(ValueName))));
                                        ProcessSubKey(RKey, p_Depth + 1, "None");
                                        RKey.Close();
                                    }
                                    else
                                    {
                                        TL.LogMessage("IgnoreKey", Conversions.ToString(Operators.ConcatenateObject(Strings.Space((p_Depth + 1) * INDENT) + p_Container + @"\", p_Key.GetValue(ValueName))));
                                    }
                                }
                                else
                                {
                                    TL.LogMessage("KeyValue", Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("### Unable to open subkey: " + ValueName + @"\", p_Key.GetValue(ValueName)), " in container: "), p_Container)));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogException("ProcessSubKey Exception 1", ex.ToString());
                }
                try
                {
                    SubKeys = p_Key.GetSubKeyNames();
                    foreach (var SubKey in SubKeys)
                    {
                        TL.LogMessage("ProcessSubKey", Strings.Space(p_Depth * INDENT) + SubKey);
                        RKey = p_Key.OpenSubKey(SubKey);
                        switch (SubKey.ToUpperInvariant() ?? "")
                        {
                            case "TYPELIB":
                                // TL.LogMessage("Container", "TypeLib...")
                                Container = "TypeLib";
                                break;

                            case "CLSID":
                                // TL.LogMessage("Container", "CLSID...")
                                Container = "CLSID";
                                break;

                            case "IMPLEMENTED CATEGORIES":
                                // TL.LogMessage("Container", "Component Categories...")
                                Container = COMPONENT_CATEGORIES;
                                break;

                            default:
                                // TL.LogMessage("Container", "Other...")
                                Container = "None";
                                break;
                        }
                        if (Strings.Left(SubKey, 1) == "{")
                        {
                            switch (p_Container ?? "")
                            {
                                case COMPONENT_CATEGORIES:
                                    // TL.LogMessage("ImpCat", "ImpCat")
                                    RKey = Registry.ClassesRoot.OpenSubKey(COMPONENT_CATEGORIES).OpenSubKey(SubKey);
                                    Container = "None";
                                    break;

                                default:
                                    // Do nothing
                                    break;
                            }
                        }

                        // Process the sub-key if it is present
                        if (!(RKey == null))
                        {
                            ProcessSubKey(RKey, p_Depth + 1, Container);
                            RKey.Close();
                        }
                        else // Sub-key cannot be opened so log message
                        {
                            TL.LogMessage("ProcessSubKey", $"Unable to open subkey {SubKey}, ignoring and continuing with next key.");
                        }

                    }
                }
                catch (Exception ex)
                {
                    LogException("ProcessSubKey Exception 2", ex.ToString());
                }
                // TL.LogMessage("End of ProcessSubKey", p_Container & " " & p_Depth)
            }

        }

        private void ScanSerial()
        {
            RegistryKey SerialRegKey = null;
            string[] SerialDevices;
            try
            {
                // First list out the ports we can see through .NET
                Status("Scanning Serial Ports");
                if (System.IO.Ports.SerialPort.GetPortNames().Length > 0)
                {
                    foreach (string Port in System.IO.Ports.SerialPort.GetPortNames())
                        TL.LogMessage("Serial Ports (.NET)", Port);
                }
                else
                {
                    TL.LogMessage("Serial Ports (.NET)", "No ports found");
                }
                TL.BlankLine();
                try
                {
                    SerialRegKey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\SERIALCOMM");
                }
                catch (Exception ex1)
                {
                    LogException("ScanSerial", @"Exception opening HARDWARE\DEVICEMAP\SERIALCOMM : " + ex1.ToString());
                }
                if (SerialRegKey is not null)
                {
                    SerialDevices = SerialRegKey.GetValueNames();
                    foreach (string SerialDevice in SerialDevices)
                        TL.LogMessage("Serial Ports (Registry)", SerialRegKey.GetValue(SerialDevice).ToString() + " - " + SerialDevice);
                    TL.BlankLine();
                }
                else
                {
                    TL.LogMessage("Serial Ports (Registry)", "No ports found");
                }

                for (int i = 1; i <= 30; i++)
                    SerialPortDetails(i);

                TL.BlankLine();
                TL.BlankLine();
            }
            catch (Exception ex)
            {
                LogException("ScanSerial", ex.ToString());
            }

        }

        private void SerialPortDetails(int PortNumber)
        {
            // List specific details of a particular serial port
            string PortName;
            var SerPort = new System.IO.Ports.SerialPort();

            try
            {
                PortName = "COM" + PortNumber.ToString(); // String version of the port name
                SerPort.PortName = PortName;
                SerPort.BaudRate = 9600;
                SerPort.Open();
                SerPort.Close();
                TL.LogMessage("Serial Port Test ", PortName + " opened OK");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("Serial Port Test ", ex.Message);
            }

            SerPort.Dispose();
        }

        private void ScanProfile()
        {
            Profile ASCOMProfile;
            ArrayList DeviceTypes;
            ArrayList Devices;
            string CompatibiityMessage32Bit, CompatibiityMessage64Bit;
            try
            {
                ASCOMProfile = new Profile();
                RecursionLevel = -1; // Initialise recursion level so the first increment makes this zero

                Status("Scanning Profile");

                DeviceTypes = ASCOMProfile.RegisteredDeviceTypes;
                foreach (string DeviceType in DeviceTypes)
                {
                    Devices = ASCOMProfile.RegisteredDevices(DeviceType);
                    TL.LogMessage("Registered Device Type", DeviceType);
                    foreach (KeyValuePair Device in Devices)
                    {
                        TL.LogMessage("Registered Devices", "   " + Device.Key + " - " + Device.Value);
                        CompatibiityMessage32Bit = Utilities.Global.DriverCompatibilityMessage(Device.Key, Bitness.Bits32, TL);
                        if (string.IsNullOrEmpty(CompatibiityMessage32Bit))
                            CompatibiityMessage32Bit = "OK for 32bit applications";
                        TL.LogMessage("Registered Devices", "       " + CompatibiityMessage32Bit);

                        if (Utilities.Global.ApplicationBits() == Bitness.Bits64) // We are on a 64bit system so test this
                        {
                            CompatibiityMessage64Bit = Utilities.Global.DriverCompatibilityMessage(Device.Key, Bitness.Bits64, TL);
                            if (string.IsNullOrEmpty(CompatibiityMessage64Bit))
                                CompatibiityMessage64Bit = "OK for 64bit applications";
                            TL.LogMessage("Registered Devices", "       " + CompatibiityMessage64Bit);
                        }
                        TL.BlankLine();
                    }
                }
                TL.BlankLine();
                TL.BlankLine();
            }
            catch (Exception ex)
            {
                LogException("RegisteredDevices", "Exception: " + ex.ToString());
            }

            try
            {
                TL.LogMessage("Profile", "Recursing Profile");
                RecurseProfile(@"\"); // Scan recursively over the profile
            }
            catch (Exception ex)
            {
                LogException("ScanProfile", ex.Message);
            }

            TL.BlankLine();
            TL.BlankLine();
        }

        private void RecurseProfile(string ASCOMKey)
        {
            SortedList<string, string> SubKeys, Values;
            string NextKey, DisplayName, DisplayValue;

            // List values in this key
            try
            {
                // TL.LogMessage("RecurseProfile", Space(3 * (If(RecursionLevel < 0, 0, RecursionLevel))) & ASCOMKey)
                Values = ASCOMRegistryAccess.EnumProfile(ASCOMKey);
                foreach (KeyValuePair<string, string> kvp in Values)
                {
                    if (string.IsNullOrEmpty(kvp.Key))
                    {
                        DisplayName = "*** Default Value ***";
                    }
                    else
                    {
                        DisplayName = kvp.Key;
                    }
                    if (string.IsNullOrEmpty(kvp.Value))
                    {
                        DisplayValue = "*** Not Set ***";
                    }
                    else
                    {
                        DisplayValue = kvp.Value;
                    }
                    TL.LogMessage("Profile Value", Strings.Space(3 * (RecursionLevel + 1)) + DisplayName + " = " + DisplayValue);
                }
            }
            catch (Exception ex)
            {
                LogException("Profile 1", "Exception: " + ex.ToString());
            }

            // Now recurse through all subkeys of this key
            try
            {
                RecursionLevel += 1; // Increment recursion level
                SubKeys = ASCOMRegistryAccess.EnumKeys(ASCOMKey);

                foreach (KeyValuePair<string, string> kvp in SubKeys)
                {
                    if (ASCOMKey == @"\")
                    {
                        NextKey = "";
                    }
                    else
                    {
                        NextKey = ASCOMKey;
                    }
                    if (string.IsNullOrEmpty(kvp.Value))
                    {
                        DisplayValue = "*** Not Set ***";
                    }
                    else
                    {
                        DisplayValue = kvp.Value;
                    }
                    TL.BlankLine();
                    TL.LogMessage("Profile Key", Strings.Space(3 * RecursionLevel) + NextKey + @"\" + kvp.Key + " - " + DisplayValue);
                    RecurseProfile(NextKey + @"\" + kvp.Key);
                }
            }

            catch (Exception ex)
            {
                LogException("Profile 2", "Exception: " + ex.ToString());
            }
            finally
            {
                RecursionLevel -= 1;
            }

        }

        private void ScanInstalledPlatform()
        {
            RegistryKey RegKey;
            SortedList<string, string> platformInfo, developerInfo;

            GetInstalledComponent("Platform 5A", "{075F543B-97C5-4118-9D54-93910DE03FE9}", false, true, true);
            GetInstalledComponent("Platform 5B", "{14C10725-0018-4534-AE5E-547C08B737B7}", false, true, true);

            try // Platform 5.5 Inno installer setup, should always be absent in Platform 7!
            {
                RegKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\microsoft\Windows\Currentversion\uninstall\ASCOM.platform.NET.Components_is1", false);

                TL.LogMessage("Platform 5.5", Conversions.ToString(RegKey?.GetValue("DisplayName")));
                TL.LogMessage("Platform 5.5", Conversions.ToString(Operators.ConcatenateObject("Inno Setup App Path - ", RegKey?.GetValue("Inno Setup: App Path"))));
                TL.LogMessage("Platform 5.5", Conversions.ToString(Operators.ConcatenateObject("Inno Setup Version - ", RegKey?.GetValue("Inno Setup: Setup Version"))));
                TL.LogMessage("Platform 5.5", Conversions.ToString(Operators.ConcatenateObject("Install Date - ", RegKey?.GetValue("InstallDate"))));
                TL.LogMessage("Platform 5.5", Conversions.ToString(Operators.ConcatenateObject("Install Location - ", RegKey?.GetValue("InstallLocation"))));
                RegKey?.Close();
            }
            catch (NullReferenceException)
            {
                TL.LogMessage("Platform 5.5", "Not Installed");
            }
            catch (Exception ex)
            {
                LogException("Platform 5.5", "Exception: " + ex.ToString());
            }
            TL.BlankLine();

            platformInfo = this.GetInstalledComponent("Platform 7", Utilities.Global.PLATFORM_INSTALLER_PROPDUCT_CODE, true, false, true);
            developerInfo = this.GetInstalledComponent("Platform 7 Developer", Utilities.Global.DEVELOPER_INSTALLER_PROPDUCT_CODE, false, true, true);

            try
            {
                if ((developerInfo[INST_DISPLAY_VERSION] ?? "") != INST_NOT_KNOWN)
                {
                    Compare("Platform 7", "Developer and Platform Version Numbers", developerInfo[INST_DISPLAY_VERSION], platformInfo[INST_DISPLAY_VERSION]);
                }
            }
            catch (KeyNotFoundException)
            {
                // Ignore errors due to the key being missing if the developer tools are not installed
            }

            TL.BlankLine();
        }

        /// <summary>
        /// Report details of an installed product
        /// </summary>
        /// <param name="Name">Presentation name of product</param>
        /// <param name="ProductCode">Installer GUID uniquely identifying the product</param>
        /// <param name="Required">Flag determining whether to report an error or return a status message if the product isn't installed</param>
        /// <param name="Force32">Flag forcing use of 32bit registry on a 64bit OS</param>
        /// <param name="MSIInstaller">True if the installer is an MSI based installer, False if an InstallAware Native installer</param>
        /// <remarks></remarks>
        private SortedList<string, string> GetInstalledComponent(string Name, string ProductCode, bool Required, bool Force32, bool MSIInstaller)
        {
            var InstallInfo = new SortedList<string, string>();

            try // Platform 7 installer GUID, should always be present in Platform 6
            {
                InstallInfo = GetInstallInformation(ProductCode, Required, Force32, MSIInstaller);
                if (InstallInfo.Count > 0)
                {
                    TL.LogMessage(Name, InstallInfo[INST_DISPLAY_NAME]);
                    TL.LogMessage(Name, "Version - " + InstallInfo[INST_DISPLAY_VERSION]);
                    TL.LogMessage(Name, "Install Date - " + InstallInfo[INST_INSTALL_DATE]);
                    TL.LogMessage(Name, "Install Location - " + InstallInfo[INST_INSTALL_LOCATION]);
                    // TL.LogMessage(Name, "Install Source - " & InstallInfo(INST_INSTALL_SOURCE))
                    NMatches += 1;
                }
                else
                {
                    TL.LogMessage(Name, "Not installed");
                }
            }
            catch (Exception ex)
            {
                LogException(Name, "Exception: " + ex.ToString());
            }

            TL.BlankLine();

            return InstallInfo;
        }

        /// <summary>
        /// Gets installation information about a product identified by its product GUID
        /// </summary>
        /// <param name="ProductCode">Installer GUID uniquely identifying the product</param>
        /// <param name="Required">Flag determining whether to report an error or return a status message if the product isn't installed</param>
        /// <param name="Force32">Flag forcing use of 32bit registry on a 64bit OS</param>
        /// <param name="MSIInstaller">True if the installer is an MSI based installer, False if an InstallAware Native installer</param>
        /// <returns>Generic Sorted List of key value pairs. If not found returns an empty list</returns>
        /// <remarks></remarks>
        private SortedList<string, string> GetInstallInformation(string ProductCode, bool Required, bool Force32, bool MSIInstaller)
        {
            RegistryKey RegKey;
            var RetVal = new SortedList<string, string>();
            string UninstallString;
            RegistryKey UninstallKey;
            string[] UninstallSubKeyNames;


#if DEBUG_TRACE
            TL.LogMessage("GetInstallInformation", $"Product: {ProductCode}, Required: {Required}, Force32: {Force32}, MSIInstaller: {MSIInstaller}");
#endif
            try
            {
                if (MSIInstaller)
                {
                    // RegKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" & PLATFORM_INSTALLER_PROPDUCT_CODE, False)
                    if (Utilities.Global.ApplicationBits() == Bitness.Bits32 | Utilities.Global.ApplicationBits() == Bitness.Bits64 & !Force32) // use 32bit access on 32bit and 64nit access on 64bit
                    {
                        RegKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + ProductCode, false);
                    }
                    else // 64bit OS but requires 32bit registry
                    {
                        RegKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + ProductCode, false);
                    }
#if DEBUG_TRACE
                    if (RegKey is not null)
                    {
                        TL.LogMessage("GetInstallInformation", "  MSI Installer: found Reg Key: " + RegKey.Name);
                    }
                    else
                    {
                        TL.LogMessage("GetInstallInformation", "  MSI Installer: Regkey is nothing!!");
                    }
#endif
                }

                else
                {
                    if (Utilities.Global.ApplicationBits() == Bitness.Bits32 | Utilities.Global.ApplicationBits() == Bitness.Bits64 & !Force32) // use 32bit access on 32bit and 64nit access on 64bit
                    {
                        UninstallKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\", false);
                        RegKey = UninstallKey.OpenSubKey(ProductCode, false);
                    }
                    else // 64bit OS but requires 32bit registry
                    {
                        UninstallKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\", false);
                        RegKey = UninstallKey.OpenSubKey(ProductCode, false);
                    }
#if DEBUG_TRACE
                    if (UninstallKey is not null)
                    {
                        TL.LogMessage("GetInstallInformation", "  Native Installer: found Uninstall Key: " + UninstallKey.Name);
                    }
                    else
                    {
                        TL.LogMessage("GetInstallInformation", "  Uninstall is nothing!!");
                    }

                    if (RegKey is not null)
                    {
                        TL.LogMessage("GetInstallInformation", "  Native Installer: found Reg Key: " + RegKey.Name);
                    }
                    else
                    {
                        TL.LogMessage("GetInstallInformation", "  Native Installer: Regkey is nothing!!");
                    }
#endif
                    // Get the uninstall string associated with this GUID
                    UninstallString = Conversions.ToString(RegKey.GetValue(INST_UNINSTALL_STRING, ""));
                    RegKey.Close();
                    RegKey = null;
#if DEBUG_TRACE
                    TL.LogMessage("GetInstallInformation", "  Native Installer: found Uninstall String: " + UninstallString);
#endif
                    UninstallSubKeyNames = UninstallKey.GetSubKeyNames();
                    foreach (string SubKey in UninstallSubKeyNames)
                    {
#if DEBUG_TRACE
                        TL.LogMessage("GetInstallInformation", "  Native Installer: searching subkey : " + SubKey);
#endif
                        if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(UninstallKey.OpenSubKey(SubKey).GetValue(INST_DISPLAY_ICON, ""), UninstallString, false)))
                        {
                            RegKey = UninstallKey.OpenSubKey(SubKey);
#if DEBUG_TRACE
                            TL.LogMessage("GetInstallInformation", "    Native Installer Found: " + RegKey.Name);
#endif
                            break;
                        }
                    }
                }

                try { RetVal.Add(INST_DISPLAY_NAME, Conversions.ToString(RegKey?.GetValue(INST_DISPLAY_NAME, INST_NOT_KNOWN))); } catch { }
                try { RetVal.Add(INST_DISPLAY_VERSION, Conversions.ToString(RegKey?.GetValue(INST_DISPLAY_VERSION, INST_NOT_KNOWN))); } catch { }
                try { RetVal.Add(INST_INSTALL_DATE, Conversions.ToString(RegKey?.GetValue(INST_INSTALL_DATE, INST_NOT_KNOWN))); } catch { }
                try { RetVal.Add(INST_INSTALL_SOURCE, Conversions.ToString(RegKey?.GetValue(INST_INSTALL_SOURCE, INST_NOT_KNOWN))); } catch { }
                try { RetVal.Add(INST_INSTALL_LOCATION, Conversions.ToString(RegKey?.GetValue(INST_INSTALL_LOCATION, INST_NOT_KNOWN))); } catch { }

                RegKey?.Close();
            }
#if DEBUG_TRACE
            catch (Exception ex)
            {
                TL.LogMessageCrLf("Exception", ex.ToString());
#else
            catch
            {
#endif
                // If Not RetVal.ContainsKey(INST_DISPLAY_NAME) Then RetVal.Add(INST_DISPLAY_NAME, "Unknown name")
                // If Not RetVal.ContainsKey(INST_DISPLAY_VERSION) Then RetVal.Add(INST_DISPLAY_VERSION, "Unknown version")
                // If Not RetVal.ContainsKey(INST_INSTALL_DATE) Then RetVal.Add(INST_INSTALL_DATE, "Unknown install date")
                // If Not RetVal.ContainsKey(INST_INSTALL_LOCATION) Then RetVal.Add(INST_INSTALL_LOCATION, "Unknown install location")
                // If Not RetVal.ContainsKey(INST_INSTALL_SOURCE) Then RetVal.Add(INST_INSTALL_SOURCE, "Unknown install source")
            }

            return RetVal;
        }

        private void ScanASCOMDrivers()
        {
            string BaseDir;
            var PathShell = new StringBuilder(260);
            try
            {

                Status("Scanning for ASCOM Drivers");
                TL.LogMessage("ASCOM Drivers Scan", "Searching for installed drivers");

                if (IntPtr.Size == 8) // We are on a 64bit OS so look in the 64bit locations for files as well
                {
                    BaseDir = Conversions.ToString(SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILES_COMMONX86, false));
                    BaseDir = PathShell.ToString() + @"\ASCOM";

                    RecurseASCOMDrivers(BaseDir + @"\Telescope"); // Check telescope drivers
                    RecurseASCOMDrivers(BaseDir + @"\Camera"); // Check camera drivers
                    RecurseASCOMDrivers(BaseDir + @"\Dome"); // Check dome drivers
                    RecurseASCOMDrivers(BaseDir + @"\FilterWheel"); // Check filterWheel drivers
                    RecurseASCOMDrivers(BaseDir + @"\Focuser"); // Check focuser drivers
                    RecurseASCOMDrivers(BaseDir + @"\Rotator"); // Check rotator drivers
                    RecurseASCOMDrivers(BaseDir + @"\SafetyMonitor"); // Check safetymonitor drivers
                    RecurseASCOMDrivers(BaseDir + @"\Switch"); // Check switch drivers
                    RecurseASCOMDrivers(BaseDir + @"\Video"); // Check switch drivers

                    BaseDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + @"\ASCOM";

                    RecurseASCOMDrivers(BaseDir + @"\Telescope"); // Check telescope drivers
                    RecurseASCOMDrivers(BaseDir + @"\Camera"); // Check camera drivers
                    RecurseASCOMDrivers(BaseDir + @"\Dome"); // Check dome drivers
                    RecurseASCOMDrivers(BaseDir + @"\FilterWheel"); // Check filterWheel drivers
                    RecurseASCOMDrivers(BaseDir + @"\Focuser"); // Check focuser drivers
                    RecurseASCOMDrivers(BaseDir + @"\Rotator"); // Check rotator drivers
                    RecurseASCOMDrivers(BaseDir + @"\SafetyMonitor"); // Check safetymonitor drivers
                    RecurseASCOMDrivers(BaseDir + @"\Switch"); // Check switch drivers
                    RecurseASCOMDrivers(BaseDir + @"\Video"); // Check switch drivers
                }
                else // 32 bit OS
                {
                    BaseDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + @"\ASCOM";

                    RecurseASCOMDrivers(BaseDir + @"\Telescope"); // Check telescope drivers
                    RecurseASCOMDrivers(BaseDir + @"\Camera"); // Check camera drivers
                    RecurseASCOMDrivers(BaseDir + @"\Dome"); // Check dome drivers
                    RecurseASCOMDrivers(BaseDir + @"\FilterWheel"); // Check filterWheel drivers
                    RecurseASCOMDrivers(BaseDir + @"\Focuser"); // Check focuser drivers
                    RecurseASCOMDrivers(BaseDir + @"\Rotator"); // Check rotator drivers
                    RecurseASCOMDrivers(BaseDir + @"\SafetyMonitor"); // Check safetymonitor drivers
                    RecurseASCOMDrivers(BaseDir + @"\Switch"); // Check switch drivers
                    RecurseASCOMDrivers(BaseDir + @"\Video");
                } // Check switch drivers

                TL.BlankLine();
            }

            catch (Exception ex)
            {
                LogException("ScanProgramFiles", "Exception: " + ex.ToString());
            }
        }

        private void RecurseASCOMDrivers(string Folder)
        {
            string[] Files, Directories;

            try
            {
                Action(Strings.Left(Folder, 70));
                Files = Directory.GetFiles(Folder);
                foreach (string MyFile in Files)
                {
                    if (MyFile.ToUpperInvariant().Contains(".EXE") | MyFile.ToUpperInvariant().Contains(".DLL"))
                    {
                        // TL.LogMessage("Driver", MyFile)
                        // FileDetails(Folder & "\", MyFile)
                        FileDetails("", MyFile);
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                TL.LogMessageCrLf("RecurseASCOMDrivers", "Directory not present: " + Folder);
                return;
            }
            catch (Exception ex)
            {
                LogException("RecurseASCOMDrivers 1", "Exception: " + ex.ToString());
            }

            try
            {
                Directories = Directory.GetDirectories(Folder);
                foreach (string Directory in Directories)
                    // TL.LogMessage("Directory", Directory)
                    RecurseASCOMDrivers(Directory);
                Action("");
            }
            catch (DirectoryNotFoundException)
            {
                TL.LogMessage("RecurseASCOMDrivers", "Directory not present: " + Folder);
            }
            catch (Exception ex)
            {
                LogException("RecurseASCOMDrivers 2", "Exception: " + ex.ToString());
            }
        }

        private void AstroUtilsTests()
        {
            RiseSet Events;
            // Dim Nov31 As New NOVAS.NOVAS31
            double TimeDifference, DUT1, DT0, DTDUT1, JDUtc;

            try
            {
                Status("Running Astro Utilities functional tests");
                TL.LogMessage("AstroUtilTests", "Creating ASCOM.Astrometry.AstroUtils.AstroUtils");
                sw.Reset();
                sw.Start();
                // Dim AstroUtil As New AstroUtils.AstroUtils
                TL.LogMessage("AstroUtilTests", "ASCOM.Astrometry.AstroUtils.AstroUtils Created OK in " + sw.ElapsedMilliseconds + " milliseconds");

                TL.LogMessage("AstroUtilTests", string.Format("AstroUtils.DeltaT: {0}", AstroUtil.DeltaT()));

                // Earth rotation data tests
                this.CompareInteger("AstroUtilTests", "Historic Offset Development Test Value", Astrometry.GlobalItems.TEST_HISTORIC_DAYS_OFFSET, 0);
                this.CompareInteger("AstroUtilTests", "UTC Days Offset Development Test Value", Astrometry.GlobalItems.TEST_UTC_DAYS_OFFSET, 0);
                this.CompareInteger("AstroUtilTests", "UTC Hours Offset Development Test Value", Astrometry.GlobalItems.TEST_UTC_HOURS_OFFSET, 0);
                this.CompareInteger("AstroUtilTests", "UTC Minutes Offset Development Test Value", Astrometry.GlobalItems.TEST_UTC_MINUTES_OFFSET, 0);

                JDUtc = AstroUtil.JulianDateUtc;
                DUT1 = AstroUtil.DeltaUT(JDUtc);
                if (DUT1 > -0.9d & DUT1 < 0.9d) // We have a valid value so proceed to test
                {
                    TL.LogMessage("AstroUtilTests", string.Format("AstroUtils.DeltaUT1 returned a valid value: {0}", DUT1));
                    DT0 = AstroUtil.JulianDateUT1(0.0d);
                    DTDUT1 = AstroUtil.JulianDateUT1(DUT1);
                    TimeDifference = (DT0 - DTDUT1) * 24.0d * 60.0d * 60.0d;
                    TL.LogMessage("AstroUtilTests", string.Format("AstroUtils.JulianDateUT1(0.0): {0}", DT0));
                    TL.LogMessage("AstroUtilTests", string.Format("AstroUtils.JulianDateUT1(DeltaUT1): {0}", DTDUT1));
                    TL.LogMessage("AstroUtilTests", string.Format("AstroUtils.JulianDateUT1 Difference: {0} seconds", TimeDifference));
                    CompareDouble("AstroUtilTests", "JulianDateUT1", DT0, DTDUT1, TOLERANCE_E3);

                    DT0 = AstroUtil.JulianDateTT(0.0d);
                    DTDUT1 = AstroUtil.JulianDateTT(DUT1);
                    TimeDifference = (DT0 - DTDUT1) * 24.0d * 60.0d * 60.0d;
                    TL.LogMessage("AstroUtilTests", string.Format("AstroUtils.JulianDateTT(0.0): {0}", DT0));
                    TL.LogMessage("AstroUtilTests", string.Format("AstroUtils.JulianDateTT(DeltaUT1): {0}", DTDUT1));
                    TL.LogMessage("AstroUtilTests", string.Format("AstroUtils.JulianDateTT Difference: {0} seconds", TimeDifference));
                    CompareDouble("AstroUtilTests", "JulianDateTT", DT0, DTDUT1, TOLERANCE_E3);
                }
                else // Invalid value produced so skip this test, which will raise an exception if DUT1 is outside the valid range
                {
                    TL.LogMessage("AstroUtilTests", string.Format("DeltaUT1 tests skipped because the returned value for DeltaUT1 is outside the valid range -0.9 to +0.9: {0}", DUT1));
                }
                TL.LogMessage("", "");

                // Range Tests
                CompareDouble("AstroUtilTests", "ConditionHA -12.0", AstroUtil.ConditionHA(-12.0d), -12.0d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "ConditionHA 0.0", AstroUtil.ConditionHA(0.0d), 0.0d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "ConditionHA 12.0", AstroUtil.ConditionHA(12.0d), 12.0d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "ConditionHA -13.0", AstroUtil.ConditionHA(-13.0d), 11.0d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "ConditionHA 13.0", AstroUtil.ConditionHA(13.0d), -11.0d, TOLERANCE_E6);

                CompareDouble("AstroUtilTests", "ConditionRA 0.0", AstroUtil.ConditionRA(0.0d), 0.0d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "ConditionRA 12.0", AstroUtil.ConditionRA(12.0d), 12.0d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "ConditionRA 23.999", AstroUtil.ConditionRA(23.999d), 23.999d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "ConditionRA -1.0", AstroUtil.ConditionRA(-1.0d), 23.0d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "ConditionRA 25.0", AstroUtil.ConditionRA(25.0d), 1.0d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "Range 0:359.999 0.0", AstroUtil.Range(0.0d, 0.0d, true, 360.0d, false), 0.0d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "Range 0:359.999 359.0", AstroUtil.Range(359.0d, 0.0d, true, 360.0d, false), 359.0d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "Range 0:359.999 -30.0", AstroUtil.Range(-30.0d, 0.0d, true, 360.0d, false), 330.0d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "Range 0:359.999 390.0", AstroUtil.Range(390.0d, 0.0d, true, 360.0d, false), 30.0d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "Range 0:359.999 360.0", AstroUtil.Range(360.0d, 0.0d, true, 360.0d, false), 0.0d, TOLERANCE_E6);
                CompareDouble("AstroUtilTests", "Range 0:360.0 360.0", AstroUtil.Range(360.0d, 0.0d, true, 360.0d, true), 360.0d, TOLERANCE_E6);

                CompareWithin("AstroUtilTests", "DeltaT", AstroUtil.DeltaT(), 67.0d, 71.0d); // Upper bound increased because DeltaT has reached the original upper bound value! PWGS 9/10/2017
                CompareWithin("AstroUtilTests", "DeltaUT", AstroUtil.DeltaUT(AstroUtil.JulianDateTT(0.0d)), -1.0d, 1.0d);
                Events = GetEvents(EventType.SunRiseSunset, 5, 8, 2012, 51.0d, -60.0d, -5.0d);

                CompareBoolean("AstroUtilTests", "Events Sun Risen at Midnight", Events.RisenAtMidnight, false);
                CompareInteger("AstroUtilTests", "Events Sun Rise Count", Events.RiseTime.Count, 1);
                CompareInteger("AstroUtilTests", "Events Sun Set Count", Events.SetTime.Count, 1);
                CompareDouble("AstroUtilTests", "Events Sun Rise", Events.RiseTime[0], 3.54149185233954d, TOLERANCE_E5);
                CompareDouble("AstroUtilTests", "Events Sun Set", Events.SetTime[0], 18.6397113427123d, TOLERANCE_E5);

                Events = GetEvents(EventType.SunRiseSunset, 19, 3, 2012, -80.0d, 85.0d, 11.0d);

                CompareBoolean("AstroUtilTests", "Events Sun Risen at Midnight", Events.RisenAtMidnight, true);
                CompareInteger("AstroUtilTests", "Events Sun Rise Count", Events.RiseTime.Count, 1);
                CompareInteger("AstroUtilTests", "Events Sun Set Count", Events.SetTime.Count, 2);
                CompareDouble("AstroUtilTests", "Events Sun Rise", Events.RiseTime[0], 10.9587287503168d, TOLERANCE_E5);
                CompareDouble("AstroUtilTests", "Events Sun Set", Events.SetTime[0], 0.0368674126801114d, TOLERANCE_E2); // Smaller tolerance because the expected value is smaller
                CompareDouble("AstroUtilTests", "Events Sun Set", Events.SetTime[1], 23.8850069460075d, TOLERANCE_E5);

                Events = GetEvents(EventType.MoonRiseMoonSet, 5, 8, 2012, 51.0d, -60.0d, -5.0d);

                CompareBoolean("AstroUtilTests", "Events Moon Risen at Midnight", Events.RisenAtMidnight, true);
                CompareInteger("AstroUtilTests", "Events Moon Rise Count", Events.RiseTime.Count, 1);
                CompareInteger("AstroUtilTests", "Events Moon Set Count", Events.SetTime.Count, 1);
                CompareDouble("AstroUtilTests", "Events Moon Rise", Events.RiseTime[0], 19.6212523985916d, TOLERANCE_E5);
                CompareDouble("AstroUtilTests", "Events Moon Set", Events.SetTime[0], 7.84782661271154d, TOLERANCE_E4);

                Events = GetEvents(EventType.MoonRiseMoonSet, 15, 1, 2012, -80.0d, 85.0d, 11.0d);

                CompareBoolean("AstroUtilTests", "Events Moon Risen at Midnight", Events.RisenAtMidnight, false);
                CompareInteger("AstroUtilTests", "Events Moon Rise Count", Events.RiseTime.Count, 2);
                CompareInteger("AstroUtilTests", "Events Moon Set Count", Events.SetTime.Count, 1);
                CompareDouble("AstroUtilTests", "Events Moon Rise", Events.RiseTime[0], 1.83185022577189d, TOLERANCE_E4);
                CompareDouble("AstroUtilTests", "Events Moon Rise", Events.RiseTime[1], 23.803310377656d, TOLERANCE_E5);
                CompareDouble("AstroUtilTests", "Events Moon Set", Events.SetTime[0], 20.2772693138778d, TOLERANCE_E5);

                Events = GetEvents(EventType.AstronomicalTwilight, 18, 5, 2012, 51.0d, 0.0d, 0.0d);

                CompareBoolean("AstroUtilTests", "Events Astronomical Twilight Sun Risen at Midnight", Events.RisenAtMidnight, false);
                CompareInteger("AstroUtilTests", "Events Astronomical Twilight Start Count", Events.RiseTime.Count, 1);
                CompareInteger("AstroUtilTests", "Events Astronomical Twilight End Count", Events.SetTime.Count, 1);
                CompareDouble("AstroUtilTests", "Events Astronomical Twilight Start", Events.RiseTime[0], 1.01115193589165d, TOLERANCE_E4);
                CompareDouble("AstroUtilTests", "Events Astronomical Twilight End", Events.SetTime[0], 22.9472021943943d, TOLERANCE_E5);

                CompareDouble("AstroUtilTests", "Moon Illumination", AstroUtil.MoonIllumination(Nov31.JulianDate(2012, 8, 5, 12.0d)), 0.872250725459045d, TOLERANCE_E5);
                CompareDouble("AstroUtilTests", "Moon Phase", AstroUtil.MoonPhase(Nov31.JulianDate(2012, 8, 5, 12.0d)), -142.145753888332d, TOLERANCE_E5);

                CompareWithin("AstroUtilTests", "DeltaUT1 - Day of start of leap second January 1961", AstroUtil.DeltaUT(2437300.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day of start of leap second March 1965", AstroUtil.DeltaUT(2438820.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day of start of leap second January 1966", AstroUtil.DeltaUT(2439126.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day of start of leap second 10", AstroUtil.DeltaUT(2441317.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day before leap second 15", AstroUtil.DeltaUT(2442777.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day of start of leap second 15", AstroUtil.DeltaUT(2442778.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day before leap second 20", AstroUtil.DeltaUT(2444785.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day of start of leap second 20", AstroUtil.DeltaUT(2444786.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day before leap second 25", AstroUtil.DeltaUT(2447891.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day of start of leap second 25", AstroUtil.DeltaUT(2447892.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day before leap second 30", AstroUtil.DeltaUT(2450082.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day of start of leap second 30", AstroUtil.DeltaUT(2450083.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day before leap second 35", AstroUtil.DeltaUT(2456108.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day of start of leap second 35", AstroUtil.DeltaUT(2456109.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day before leap second 37", AstroUtil.DeltaUT(2457753.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Day of start of leap second 37", AstroUtil.DeltaUT(2457754.5d), -0.9d, 0.9d);
                CompareWithin("AstroUtilTests", "DeltaUT1 - Today", AstroUtil.DeltaUT(AstroUtil.JulianDateUtc), -0.9d, 0.9d);

                var parameters = new EarthRotationParameters();

                foreach (KeyValuePair<double, double> HistoricLeapSecond in parameters.DownloadedLeapSeconds)
                {
                    var LeapSecondDateTime = DateTime.FromOADate(HistoricLeapSecond.Key - Astrometry.GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET);
                    TL.LogMessage("AstroUtilTests", string.Format("Found historic leap second value {0} which came into effect on JD {1} ({2})", HistoricLeapSecond.Value, HistoricLeapSecond.Key, LeapSecondDateTime.ToString(Astrometry.GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                }

                parameters.Dispose();
                parameters = null;

                TL.BlankLine();

                TL.LogMessage("AstroUtilTests", "Finished");
                TL.BlankLine();
            }

            catch (Exception ex)
            {
                LogException("AstroUtilTests", "Exception: " + ex.ToString());
            }

        }

        private struct RiseSet
        {
            public bool RisenAtMidnight;
            public List<double> RiseTime;
            public List<double> SetTime;
        }

        private RiseSet GetEvents(EventType TypeOfEvent, int Day, int Month, int Year, double Latitude, double Longitude, double TimeZone)
        {
            ArrayList Events;
            var Retval = new RiseSet();
            int NumberOfRises, NumberOfSets;

            Events = AstroUtil.EventTimes(TypeOfEvent, Day, Month, Year, Latitude, Longitude, TimeZone);
            Retval.RisenAtMidnight = Conversions.ToBoolean(Events[0]);
            Retval.RiseTime = new List<double>();
            Retval.SetTime = new List<double>();

            NumberOfRises = Conversions.ToInteger(Events[1]); // Retrieve the number of sunrises
            NumberOfSets = Conversions.ToInteger(Events[2]); // Retrieve the number of sunsets

            if (NumberOfRises > 0 | NumberOfSets > 0) // Moon either rises or sets this day
            {
                switch (NumberOfRises)
                {
                    case 0: // No sunrises so no action
                        {
                            break;
                        }
                    case 1: // 1 sunrise so add the value
                        {
                            Retval.RiseTime.Add(Conversions.ToDouble(Events[3]));
                            break;
                        }
                    case 2: // 2 sunrises so add both values
                        {
                            Retval.RiseTime.Add(Conversions.ToDouble(Events[3]));
                            Retval.RiseTime.Add(Conversions.ToDouble(Events[4]));
                            break;
                        }
                }
                switch (NumberOfSets)
                {
                    case 0: // No sunsets so no action
                        {
                            break;
                        }
                    case 1: // 1 sunset so add the value
                        {
                            Retval.SetTime.Add(Conversions.ToDouble(Events[NumberOfRises + 3]));
                            break;
                        }
                    case 2: // 2 sunsets so build up message lines 1 and 2
                        {
                            Retval.SetTime.Add(Conversions.ToDouble(Events[NumberOfRises + 3]));
                            Retval.SetTime.Add(Conversions.ToDouble(Events[NumberOfRises + 4]));
                            break;
                        }
                }
            }
            else
            {
            } // Moon neither rises or sets this day so no further action required here
            return Retval;
        }

        #endregion

        #region XML  test String
        private const string XMLTestString = "<?xml version=\"1.0\"?>" + "\r\n" + "<ASCOMProfile>" + "\r\n" + "  <SubKey>" + "\r\n" + "    <SubKeyName />" + "\r\n" + "    <DefaultValue>" + TestTelescopeDescription + "</DefaultValue>" + "\r\n" + "    <Values>" + "\r\n" + "      <Value>" + "\r\n" + "        <Name>Results 1</Name>" + "\r\n" + "        <Data />" + "\r\n" + "      </Value>" + "\r\n" + "      <Value>" + "\r\n" + "        <Name>Root Test Name</Name>" + "\r\n" + "        <Data>Test Value in Root key</Data>" + "\r\n" + "      </Value>" + "\r\n" + "      <Value>" + "\r\n" + "        <Name>Test Name</Name>" + "\r\n" + "        <Data>Test Value</Data>" + "\r\n" + "      </Value>" + "\r\n" + "      <Value>" + "\r\n" + "        <Name>Test Name Default</Name>" + "\r\n" + "        <Data>123456</Data>" + "\r\n" + "      </Value>" + "\r\n" + "    </Values>" + "\r\n" + "  </SubKey>" + "\r\n" + "  <SubKey>" + "\r\n" + "    <SubKeyName>SubKey1</SubKeyName>" + "\r\n" + "    <DefaultValue />" + "\r\n" + "    <Values />" + "\r\n" + "  </SubKey>" + "\r\n" + "  <SubKey>" + "\r\n" + @"    <SubKeyName>SubKey1\SubKey2</SubKeyName>" + "\r\n" + "    <DefaultValue>Null Key in SubKey2</DefaultValue>" + "\r\n" + "    <Values>" + "\r\n" + "      <Value>" + "\r\n" + "        <Name>SubKey2 Test Name</Name>" + "\r\n" + "        <Data>Test Value in SubKey 2</Data>" + "\r\n" + "      </Value>" + "\r\n" + "      <Value>" + "\r\n" + "        <Name>SubKey2 Test Name1</Name>" + "\r\n" + "        <Data>Test Value in SubKey 2</Data>" + "\r\n" + "      </Value>" + "\r\n" + "    </Values>" + "\r\n" + "  </SubKey>" + "\r\n" + "  <SubKey>" + "\r\n" + @"    <SubKeyName>SubKey1\SubKey2\SubKey2a</SubKeyName>" + "\r\n" + "    <DefaultValue />" + "\r\n" + "    <Values>" + "\r\n" + "      <Value>" + "\r\n" + "        <Name>SubKey2a Test Name2a</Name>" + "\r\n" + "        <Data>Test Value in SubKey 2a</Data>" + "\r\n" + "      </Value>" + "\r\n" + "    </Values>" + "\r\n" + "  </SubKey>" + "\r\n" + "  <SubKey>" + "\r\n" + @"    <SubKeyName>SubKey1\SubKey2\SubKey2a\SubKey2b</SubKeyName>" + "\r\n" + "    <DefaultValue />" + "\r\n" + "    <Values>" + "\r\n" + "      <Value>" + "\r\n" + "        <Name>SubKey2b Test Name2b</Name>" + "\r\n" + "        <Data>Test Value in SubKey 2b</Data>" + "\r\n" + "      </Value>" + "\r\n" + "    </Values>" + "\r\n" + "  </SubKey>" + "\r\n" + "  <SubKey>" + "\r\n" + @"    <SubKeyName>SubKey1\SubKey2\SubKey2c</SubKeyName>" + "\r\n" + "    <DefaultValue />" + "\r\n" + "    <Values>" + "\r\n" + "      <Value>" + "\r\n" + "        <Name>SubKey2c Test Name2c</Name>" + "\r\n" + "        <Data>Test Value in SubKey 2c</Data>" + "\r\n" + "      </Value>" + "\r\n" + "    </Values>" + "\r\n" + "  </SubKey>" + "\r\n" + "  <SubKey>" + "\r\n" + "    <SubKeyName>SubKey3</SubKeyName>" + "\r\n" + "    <DefaultValue />" + "\r\n" + "    <Values>" + "\r\n" + "      <Value>" + "\r\n" + "        <Name>SubKey3 Test Name</Name>" + "\r\n" + "        <Data>Test Value SubKey 3</Data>" + "\r\n" + "      </Value>" + "\r\n" + "    </Values>" + "\r\n" + "  </SubKey>" + "\r\n" + "  <SubKey>" + "\r\n" + "    <SubKeyName>SubKey4</SubKeyName>" + "\r\n" + "    <DefaultValue />" + "\r\n" + "    <Values>" + "\r\n" + "      <Value>" + "\r\n" + "        <Name>SubKey4 Test Name</Name>" + "\r\n" + "        <Data>Test Value SubKey 4</Data>" + "\r\n" + "      </Value>" + "\r\n" + "    </Values>" + "\r\n" + "  </SubKey>" + "\r\n" + "  <SubKey>" + "\r\n" + "    <SubKeyName>SubKeyDefault</SubKeyName>" + "\r\n" + "    <DefaultValue />" + "\r\n" + "    <Values>" + "\r\n" + "      <Value>" + "\r\n" + "        <Name>Test Name Default</Name>" + "\r\n" + "        <Data>123456</Data>" + "\r\n" + "      </Value>" + "\r\n" + "    </Values>" + "\r\n" + "  </SubKey>" + "\r\n" + "</ASCOMProfile>";
        #endregion

        #region Button event handlers

        private void BtnLastLog_Click(object sender, EventArgs e)
        {
            Process.Start(LastLogFile); // Open in the system's default text editor
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0); // Close the program
        }

        #endregion

        #region Serial menu event handlers

        private void MenuSerialTraceEnabled_Click(object sender, EventArgs e)
        {
            RegistryAccess ProfileStore;

            ProfileStore = new RegistryAccess(Utilities.VB6COMErrors.ERR_SOURCE_CHOOSER); // Get access to the profile store
            if (MenuSerialTraceEnabled.Checked)
            {
                MenuSerialTraceEnabled.Checked = false; // Uncheck the enabled flag, make it inaccessible and clear the trace file name
                ProfileStore.WriteProfile("", Utilities.Global.SERIAL_FILE_NAME_VARNAME, "");

                // Enable the set trace options
                MenuUseTraceManualFilename.Enabled = true;
                MenuUseTraceManualFilename.Checked = false;
                MenuUseTraceAutoFilenames.Enabled = true;
                MenuUseTraceAutoFilenames.Checked = false;
            }
            else
            {
                MenuSerialTraceEnabled.Checked = true; // Enable the trace enabled flag
                ProfileStore.WriteProfile("", Utilities.Global.SERIAL_FILE_NAME_VARNAME, Utilities.Global.SERIAL_AUTO_FILENAME);

                MenuUseTraceAutoFilenames.Enabled = false;
                MenuUseTraceAutoFilenames.Checked = true; // Enable the auto trace name flag
                MenuUseTraceManualFilename.Checked = false; // Unset the manual file flag
                MenuUseTraceManualFilename.Enabled = false;
            }
            ProfileStore.Dispose();
        }

        private void MenuAutoTraceFilenames_Click(object sender, EventArgs e)
        {
            RegistryAccess ProfileStore;
            ProfileStore = new RegistryAccess(Utilities.VB6COMErrors.ERR_SOURCE_CHOOSER); // Get access to the profile store
                                                                                          // Auto filenames currently disabled, so enable them
            MenuUseTraceAutoFilenames.Checked = true; // Enable the auto trace name flag
            MenuUseTraceAutoFilenames.Enabled = false;
            MenuUseTraceManualFilename.Checked = false; // Unset the manual file flag
            MenuUseTraceManualFilename.Enabled = false;
            MenuSerialTraceEnabled.Enabled = true; // Set the trace enabled flag
            MenuSerialTraceEnabled.Checked = true; // Enable the trace enabled flag
            ProfileStore.WriteProfile("", Utilities.Global.SERIAL_FILE_NAME_VARNAME, Utilities.Global.SERIAL_AUTO_FILENAME);
            ProfileStore.Dispose();
        }

        private void MenuIncludeSerialTraceDebugInformation_Click(object sender, EventArgs e)
        {
            MenuIncludeSerialTraceDebugInformation.Checked = !MenuIncludeSerialTraceDebugInformation.Checked; // Invert selection
            Utilities.Global.SetName(Utilities.Global.SERIAL_TRACE_DEBUG, MenuIncludeSerialTraceDebugInformation.Checked.ToString());
        }

        private void MenuSimulatorTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuSimulatorTraceEnabled.Checked = !MenuSimulatorTraceEnabled.Checked; // Invert the selection
            Utilities.Global.SetName(Utilities.Global.SIMULATOR_TRACE, MenuSimulatorTraceEnabled.Checked.ToString());
        }

        private void MenuUseTraceManualFilename_Click(object sender, EventArgs e)
        {
            RegistryAccess ProfileStore;
            DialogResult RetVal;

            ProfileStore = new RegistryAccess(Utilities.VB6COMErrors.ERR_SOURCE_CHOOSER); // Get access to the profile store

            // Set up the manual serial trace file name entry dialogue
            SerialTraceFileName.FileName = "SerialTrace.txt";
            SerialTraceFileName.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            SerialTraceFileName.FilterIndex = 1;
            SerialTraceFileName.RestoreDirectory = true;
            RetVal = SerialTraceFileName.ShowDialog(); // Show the dialogue and retrieve the result

            switch (RetVal) // Handle the outcome from the user
            {
                case DialogResult.OK:
                    {
                        // Save the result
                        ProfileStore.WriteProfile("", Utilities.Global.SERIAL_FILE_NAME_VARNAME, SerialTraceFileName.FileName);
                        // Check and enable the serial trace enabled flag
                        MenuSerialTraceEnabled.Enabled = true;
                        MenuSerialTraceEnabled.Checked = true;
                        // Enable manual serial trace file flag
                        MenuUseTraceAutoFilenames.Checked = false;
                        MenuUseTraceAutoFilenames.Enabled = false;
                        MenuUseTraceManualFilename.Checked = true;
                        MenuUseTraceManualFilename.Enabled = false; // Ignore everything else
                        break;
                    }

                default:
                    {
                        break;
                    }

            }

            ProfileStore.Dispose();
        }

        #endregion

        #region Other menu event handlers

        /// <summary>
        /// Refresh the trace menu items based on current values stored in the user's registry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void MnuTrace_DropDownOpening(object sender, EventArgs e)
        {
            RefreshTraceItems();
        }

        /// <summary>
        /// Refresh the trace menu items based on values stored in the user's registry
        /// </summary>
        /// <remarks></remarks>
        private void RefreshTraceItems()
        {
            RegistryAccess ProfileStore;
            string TraceFileName;
            WaitType TypeOfWait;

            ProfileStore = new RegistryAccess("Diagnostics"); // Get access to the profile store

            TraceFileName = ProfileStore.GetProfile("", Utilities.Global.SERIAL_FILE_NAME_VARNAME, "");
            switch (TraceFileName ?? "") // Trace is disabled
            {
                case var @case when @case == "":
                    {
                        MenuUseTraceAutoFilenames.Enabled = true; // Auto filenames are enabled but unchecked
                        MenuUseTraceAutoFilenames.Checked = false;
                        MenuUseTraceManualFilename.Enabled = true; // Manual trace filename is enabled but unchecked
                        MenuUseTraceManualFilename.Checked = false;
                        MenuSerialTraceEnabled.Checked = false; // The trace enabled flag is unchecked and disabled
                        MenuSerialTraceEnabled.Enabled = true;
                        break;
                    }
                case Utilities.Global.SERIAL_AUTO_FILENAME: // Tracing is on using an automatic filename
                    {
                        MenuUseTraceAutoFilenames.Enabled = false; // Auto filenames are disabled and checked
                        MenuUseTraceAutoFilenames.Checked = true;
                        MenuUseTraceManualFilename.Enabled = false; // Manual trace filename is dis enabled and unchecked
                        MenuUseTraceManualFilename.Checked = false;
                        MenuSerialTraceEnabled.Checked = true; // The trace enabled flag is checked and enabled
                        MenuSerialTraceEnabled.Enabled = true; // Tracing using some other fixed filename
                        break;
                    }

                default:
                    {
                        MenuUseTraceAutoFilenames.Enabled = false; // Auto filenames are disabled and unchecked
                        MenuUseTraceAutoFilenames.Checked = false;
                        MenuUseTraceManualFilename.Enabled = false; // Manual trace filename is disabled enabled and checked
                        MenuUseTraceManualFilename.Checked = true;
                        MenuSerialTraceEnabled.Checked = true; // The trace enabled flag is checked and enabled
                        MenuSerialTraceEnabled.Enabled = true;
                        break;
                    }
            }

            // Set Profile trace checked state on menu item 
            MenuProfileTraceEnabled.Checked = Utilities.Global.GetBool(Utilities.Global.TRACE_PROFILE, Utilities.Global.TRACE_PROFILE_DEFAULT);
            MenuRegistryTraceEnabled.Checked = Utilities.Global.GetBool(Utilities.Global.TRACE_XMLACCESS, Utilities.Global.TRACE_XMLACCESS_DEFAULT);
            MenuUtilTraceEnabled.Checked = Utilities.Global.GetBool(Utilities.Global.TRACE_UTIL, Utilities.Global.TRACE_UTIL_DEFAULT);
            MenuTimerTraceEnabled.Checked = Utilities.Global.GetBool(Utilities.Global.TRACE_TIMER, Utilities.Global.TRACE_TIMER_DEFAULT);
            MenuTransformTraceEnabled.Checked = Utilities.Global.GetBool(Utilities.Global.TRACE_TRANSFORM, Utilities.Global.TRACE_TRANSFORM_DEFAULT);
            MenuIncludeSerialTraceDebugInformation.Checked = Utilities.Global.GetBool(Utilities.Global.SERIAL_TRACE_DEBUG, Utilities.Global.SERIAL_TRACE_DEBUG_DEFAULT);
            MenuSimulatorTraceEnabled.Checked = Utilities.Global.GetBool(Utilities.Global.SIMULATOR_TRACE, Utilities.Global.SIMULATOR_TRACE_DEFAULT);
            MenuDriverAccessTraceEnabled.Checked = Utilities.Global.GetBool(Utilities.Global.DRIVERACCESS_TRACE, Utilities.Global.DRIVERACCESS_TRACE_DEFAULT);
            MenuThrowAbandonedMutexExceptions.Checked = Utilities.Global.GetBool(Utilities.Global.ABANDONED_MUTEXT_TRACE, Utilities.Global.ABANDONED_MUTEX_TRACE_DEFAULT);
            MenuAstroUtilsTraceEnabled.Checked = Utilities.Global.GetBool(Utilities.Global.ASTROUTILS_TRACE, Utilities.Global.ASTROUTILS_TRACE_DEFAULT);
            MenuNovasTraceEnabled.Checked = Utilities.Global.GetBool(Utilities.Global.NOVAS_TRACE, Utilities.Global.NOVAS_TRACE_DEFAULT);
            MenuCacheTraceEnabled.Checked = Utilities.Global.GetBool(Utilities.Global.TRACE_CACHE, Utilities.Global.TRACE_CACHE_DEFAULT);
            MenuEarthRotationDataFormTraceEnabled.Checked = Utilities.Global.GetBool(Utilities.Global.TRACE_EARTHROTATION_DATA_FORM, Utilities.Global.TRACE_EARTHROTATION_DATA_FORM_DEFAULT);
            MenuDiagnosticsTraceEnabled.Checked = Utilities.Global.GetBool(OPTIONS_DIAGNOSTICS_TRACE, OPTIONS_DIAGNOSTICS_TRACE_DEFAULT);

            TypeOfWait = Utilities.Global.GetWaitType(Utilities.Global.SERIAL_WAIT_TYPE, Utilities.Global.SERIAL_WAIT_TYPE_DEFAULT);

            MenuWaitTypeManualResetEvent.Checked = false;
            MenuWaitTypeSleep.Checked = false;
            MenuWaitTypeWaitForSingleObject.Checked = false;
            switch (TypeOfWait)
            {
                case Serial.WaitType.ManualResetEvent:
                    {
                        MenuWaitTypeManualResetEvent.Checked = true;
                        break;
                    }
                case Serial.WaitType.Sleep:
                    {
                        MenuWaitTypeSleep.Checked = true;
                        break;
                    }
                case Serial.WaitType.WaitForSingleObject:
                    {
                        MenuWaitTypeWaitForSingleObject.Checked = true;
                        break;
                    }
            }

            // Set the check for updates check marks
            OptionsCheckForPlatformReleases.Checked = Utilities.Global.GetBool(Utilities.Global.CHECK_FOR_RELEASE_UPDATES, Utilities.Global.CHECK_FOR_RELEASE_UPDATES_DEFAULT);
            OptionsCheckForPlatformPreReleases.Checked = Utilities.Global.GetBool(Utilities.Global.CHECK_FOR_RELEASE_CANDIDATES, Utilities.Global.CHECK_FOR_RELEASE_CANDIDATES_DEFAULT);

            // Set the Is using Omni-Simulators check mark
            OptionsUseOmniSimulators.Checked = SimulatorManager.IsUsingOmniSimulators(tlInternal);
            LogInternal(" ", " ");
        }

        private void ChooserToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dynamic Chooser;
            Type ChooserType;

            if (Utilities.Global.ApplicationBits() == Bitness.Bits32)
            {
                // Chooser = CreateObject("DriverHelper.Chooser")
                ChooserType = Type.GetTypeFromProgID("DriverHelper.Chooser");
                Chooser = Activator.CreateInstance(ChooserType);
                Chooser.DeviceType = "Telescope";
            }
            else
            {
                Interaction.MsgBox("This component is 32bit only and cannot run on a 64bit system");
            }
        }

        private void ChooserNETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Chooser Chooser;

            Chooser = new Chooser() { DeviceType = "Telescope" };
            Chooser.Dispose();

        }

        private void ListAvailableCOMPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SerialForm serialForm = new();
            serialForm.ShowDialog();
        }

        private void ChooseAndConncectToDevice64bitApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ConnectForm.Visible = True
            Process proc;
            ProcessStartInfo procStartInfo;
            try
            {
                procStartInfo = new ProcessStartInfo(Application.StartupPath + DRIVER_CONNECT_APPLICATION_64BIT);
                proc = new Process() { StartInfo = procStartInfo };
                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + " - " + Application.StartupPath + DRIVER_CONNECT_APPLICATION_64BIT, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ChooseAndConnectToDevice32BitApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process proc;
            ProcessStartInfo procStartInfo;

            try
            {
                procStartInfo = new ProcessStartInfo(Application.StartupPath + DRIVER_CONNECT_APPLICATION_32BIT);
                proc = new Process() { StartInfo = procStartInfo };
                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + " - " + Application.StartupPath + DRIVER_CONNECT_APPLICATION_32BIT, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void MenuProfileTraceEnabled_Click_1(object sender, EventArgs e)
        {
            MenuProfileTraceEnabled.Checked = !MenuProfileTraceEnabled.Checked; // Invert the selection
            Utilities.Global.SetName(Utilities.Global.TRACE_PROFILE, MenuProfileTraceEnabled.Checked.ToString());
        }

        private void MenuRegistryTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuRegistryTraceEnabled.Checked = !MenuRegistryTraceEnabled.Checked; // Invert the selection
            Utilities.Global.SetName(Utilities.Global.TRACE_XMLACCESS, MenuRegistryTraceEnabled.Checked.ToString());
        }

        private void MenuUtilTraceEnabled_Click_1(object sender, EventArgs e)
        {
            MenuUtilTraceEnabled.Checked = !MenuUtilTraceEnabled.Checked; // Invert the selection
            Utilities.Global.SetName(Utilities.Global.TRACE_UTIL, MenuUtilTraceEnabled.Checked.ToString());
        }

        private void MenuTimerTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuTimerTraceEnabled.Checked = !MenuTimerTraceEnabled.Checked; // Invert the selection
            Utilities.Global.SetName(Utilities.Global.TRACE_TIMER, MenuTimerTraceEnabled.Checked.ToString());
        }

        private void MenuTransformTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuTransformTraceEnabled.Checked = !MenuTransformTraceEnabled.Checked; // Invert the selection
            Utilities.Global.SetName(Utilities.Global.TRACE_TRANSFORM, MenuTransformTraceEnabled.Checked.ToString());
        }

        private void MenuDriverAccessTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuDriverAccessTraceEnabled.Checked = !MenuDriverAccessTraceEnabled.Checked; // Invert the selection
            Utilities.Global.SetName(Utilities.Global.DRIVERACCESS_TRACE, MenuDriverAccessTraceEnabled.Checked.ToString());
        }

        private void MenuCacheTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuCacheTraceEnabled.Checked = !MenuCacheTraceEnabled.Checked; // Invert the selection
            Utilities.Global.SetName(Utilities.Global.TRACE_CACHE, MenuCacheTraceEnabled.Checked.ToString());
        }

        private void MenuAstroUtilsTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuAstroUtilsTraceEnabled.Checked = !MenuAstroUtilsTraceEnabled.Checked; // Invert the selection
            Utilities.Global.SetName(Utilities.Global.ASTROUTILS_TRACE, MenuAstroUtilsTraceEnabled.Checked.ToString());
        }

        private void MenuThrowAbandonedMutexExceptions_Click(object sender, EventArgs e)
        {
            MenuThrowAbandonedMutexExceptions.Checked = !MenuThrowAbandonedMutexExceptions.Checked; // Invert the selection
            Utilities.Global.SetName(Utilities.Global.ABANDONED_MUTEXT_TRACE, MenuThrowAbandonedMutexExceptions.Checked.ToString());
        }

        private void MenuNovasTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuNovasTraceEnabled.Checked = !MenuNovasTraceEnabled.Checked; // Invert selection
            Utilities.Global.SetName(Utilities.Global.NOVAS_TRACE, MenuNovasTraceEnabled.Checked.ToString());
        }

        private void EarthRotationDataUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ERDForm is not null) // We always want a fresh instance of this form so remove any previously used instance
            {
                try
                {
                    ERDForm.Dispose();
                }
                catch
                {
                }
                try
                {
                    ERDForm = null;
                }
                catch
                {
                }
            }
            ERDForm = new EarthRotationDataForm(); // Create a new instance and display it
            ERDForm.ShowDialog();
        }

        private void MenuEarthRotationScheduledJobTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuEarthRotationDataFormTraceEnabled.Checked = !MenuEarthRotationDataFormTraceEnabled.Checked; // Invert the selection
            Utilities.Global.SetName(Utilities.Global.TRACE_EARTHROTATION_DATA_FORM, MenuEarthRotationDataFormTraceEnabled.Checked.ToString());
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VersionForm versionForm = new();
            versionForm.ShowDialog();
        }

        private void MenuWaitTypeManualResetEvent_Click(object sender, EventArgs e)
        {
            MenuWaitTypeManualResetEvent.Checked = true;
            MenuWaitTypeSleep.Checked = false;
            MenuWaitTypeWaitForSingleObject.Checked = false;
            Utilities.Global.SetName(Utilities.Global.SERIAL_WAIT_TYPE, Serial.WaitType.ManualResetEvent.ToString());
        }

        private void MenuWaitTypeSleep_Click(object sender, EventArgs e)
        {
            MenuWaitTypeManualResetEvent.Checked = false;
            MenuWaitTypeSleep.Checked = true;
            MenuWaitTypeWaitForSingleObject.Checked = false;
            Utilities.Global.SetName(Utilities.Global.SERIAL_WAIT_TYPE, Serial.WaitType.Sleep.ToString());
        }

        private void MenuWaitTypeWaitForSingleObject_Click(object sender, EventArgs e)
        {
            MenuWaitTypeManualResetEvent.Checked = false;
            MenuWaitTypeSleep.Checked = false;
            MenuWaitTypeWaitForSingleObject.Checked = true;
            Utilities.Global.SetName(Utilities.Global.SERIAL_WAIT_TYPE, Serial.WaitType.WaitForSingleObject.ToString());
        }

        private void MenuAutoViewLog_Click(object sender, EventArgs e)
        {
            MenuAutoViewLog.Checked = !MenuAutoViewLog.Checked; // Auto view log option clicked so invert its checked status
            Utilities.Global.SetName(OPTIONS_AUTOVIEW_REGISTRYKEY, MenuAutoViewLog.Checked.ToString()); // Set the new value in the registry
        }

        private void SetLogFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a folder browser dialogue
            FolderBrowserDialog folderDlg = new();

            // Set the starting path to the current value using the Documents folder as a fall-back if no location has yet been set
            folderDlg.SelectedPath = Global.GetString(TRACELOGGER_DEFAULT_FOLDER, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            folderDlg.Description = "Select ASCOM Log File Path";

            // Show the FolderBrowserDialog and save the new value if required
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                Global.SetName(TRACELOGGER_DEFAULT_FOLDER, folderDlg.SelectedPath);
            }
        }

        private void DiagnosticsForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Handle the Form KeyDown event to determine which key was pressed
            // Test whether the F5 key was pressed to start the test
            if (e.KeyCode == Keys.F5) // F5 was pressed so start the Diagnostics test
            {
                RunDiagnostics(new object(), new EventArgs());
            }
        }

        /// <summary>
        /// Click handler for Options - Swap COM simulators from Platform 6 to Omni-Simulators and vice versa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OptionsUseOmniSimulators_Click(object sender, EventArgs e)
        {
            LogInternal("UseOmniSimulators", $"Menu entry clicked");
            LogInternal(" ", " ");

            // Check whether OmniSims are in use
            if (SimulatorManager.IsUsingOmniSimulators(tlInternal)) // Omni-Simulators are in use
            {
                // Swap to Platform simulators
                SimulatorManager.SetPlatform6Simulators(false, tlInternal);
            }
            else // Platform simulators are in use
            {
                // Swap to Omni-Simulators
                SimulatorManager.SetOmniSimulators(false, tlInternal);
            }

            // Update the state of the simulator option checked flag as appropriate
            OptionsUseOmniSimulators.Checked = SimulatorManager.IsUsingOmniSimulators(tlInternal);

            LogInternal(" ", " ");
            LogInternal("UseOmniSimulators", $"Complete");
            LogInternal(" ", " ");
        }

        private void MenuDiagnosticsTraceEnabled_Click(object sender, EventArgs e)
        {
            MenuDiagnosticsTraceEnabled.Checked = !MenuDiagnosticsTraceEnabled.Checked;
            Utilities.Global.SetName(OPTIONS_DIAGNOSTICS_TRACE, MenuDiagnosticsTraceEnabled.Checked.ToString()); // Set the new value in the registry
        }

        // Check for updates handlers
        private void OptionsCheckForPlatformReleases_Click(object sender, EventArgs e)
        {
            OptionsCheckForPlatformReleases.Checked = !OptionsCheckForPlatformReleases.Checked;
            Utilities.Global.SetName(Utilities.Global.CHECK_FOR_RELEASE_UPDATES, OptionsCheckForPlatformReleases.Checked.ToString());
        }

        private void OptionsCheckForPlatformPreReleases_Click(object sender, EventArgs e)
        {
            OptionsCheckForPlatformPreReleases.Checked = !OptionsCheckForPlatformPreReleases.Checked;
            Utilities.Global.SetName(Utilities.Global.CHECK_FOR_RELEASE_CANDIDATES, OptionsCheckForPlatformPreReleases.Checked.ToString());
        }

        private void BtnUpdateAvailable_Click(object sender, EventArgs e)
        {
            Process.Start(PlatformUpdateChecker.UpdateCheck.GetPlatformDownloadUrl());
        }

        private void BtnPreReleaseUpdateAvailable_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ASCOMInitiative/ASCOMPlatform/releases");
        }

        #endregion

        #region Utility Code

        // DLL to provide the path to Program Files(x86)\Common Files folder location that is not available through the .NET framework
        [DllImport("shell32.dll")]
        private static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, StringBuilder lpszPath, int nFolder, bool fCreate);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern uint NtQueryTimerResolution(ref int MinimumResolution, ref int MaximumResolution, ref int CurrentResolution);

        private void Status(string Msg)
        {
            Application.DoEvents();
            lblResult.Text = Msg;
            Application.DoEvents();
        }

        private void Action(string Msg)
        {
            Application.DoEvents();
            lblAction.Text = Msg;
            Application.DoEvents();
        }

        private bool RunningInVM(bool WriteToLog)
        {
            using (ManagementObjectSearcher searcher = new("Select * from Win32_ComputerSystem"))
            {
                using (ManagementObjectCollection items = searcher.Get())
                {
                    foreach (ManagementBaseObject item in items)
                    {
                        // Extract manufacturer and model
                        string manufacturer = item["Manufacturer"].ToString().ToLowerInvariant();
                        string model = item["Model"].ToString().ToLowerInvariant();
                        if (WriteToLog)
                            TL.LogMessage("RunningInVM", $"Found Manufacturer: {manufacturer}, Model: {model}");
                        // Determine whether we are in a VM
                        if ((manufacturer == "microsoft corporation" && model.Contains("virtual"))
                            || manufacturer.Contains("vmware")
                            || model == "virtualbox")
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Display an update available button on the Diagnostics form.
        /// </summary>
        /// <param name="message">Dummy parameter required to match the CheckForUpdates method signature.</param>
        /// <remarks>
        /// The CheckForUpdates reelaseAction parameter is a one parameter action but the parameter is only used by the PlatformUpdateChecker executable and not by the Diagnostics form.
        /// The message parameter is only included so that this call matches the CheckForUpdates signature.
        /// </remarks>
        private void ShowUpdateAvailable(SemVersion message)
        {
            LogInternal("CheckForUpdates", $"Making update button visible");
            BtnUpdateAvailable.Invoke(new Action(() => BtnUpdateAvailable.Visible = true));
            LogInternal("CheckForUpdates", $"Update button now visible");
        }

        private void DiagnosticsUpdateCheck()
        {
            try
            {
                LogInternal("DiagnosticsUpdateCheck", "Diagnostics is checking for updates");

                // Check whether updates are to be checked at all
                if (OptionsCheckForPlatformPreReleases.Checked | OptionsCheckForPlatformPreReleases.Checked) // Either release or pre-release updates are to be checked
                {
                    // Delay for a few seconds to allow the GUI to initialise
                    LogInternal("DiagnosticsUpdateCheck", $"Entered Delaying...");
                    Thread.Sleep(1000);
                    LogInternal("DiagnosticsUpdateCheck", $"Running update check");

                    // Check for updates, running the ShowUpdateAvailable method if an update is available. 
                    // The CheckForUpdates relaseAction parameter is a one parameter action but the parameter is only used by the PlatformUpdateChecker executable and not by the Diagnostics form
                    PlatformUpdateChecker.UpdateCheck.CheckForUpdates((x) => ShowUpdateAvailable(new SemVersion(0)), tlInternal);

                    LogInternal("DiagnosticsUpdateCheck", $"Update check complete");
                    LogInternal(" ", $" ");
                }
                else // Updates are not to be checked at all
                {
                    LogInternal("DiagnosticsUpdateCheck", $"Not checking GitHub because checking for release updates is disabled and checking for pre-release updates is disabled.");
                }
            }
            catch (Exception ex)
            {
                LogInternal("DiagnosticsUpdateCheck", $"Exception - {ex.Message}\r\n{ex}");
            }
        }

        private static void LogInternal(string member, string message)
        {
            //Debug.WriteLine($"{DateTime.Now:HH:mm:ss.fff} {member,-20}{message}");
            tlInternal?.LogMessageCrLf(member, message);
        }

        #endregion

    }
}