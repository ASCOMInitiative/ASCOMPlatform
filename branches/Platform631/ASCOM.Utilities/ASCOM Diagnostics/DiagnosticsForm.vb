Imports ASCOM.Astrometry
Imports ASCOM.DeviceInterface
Imports ASCOM.Internal
Imports ASCOM.Utilities.Exceptions
Imports ASCOM.Utilities.Video
Imports ASCOM.Utilities.Serial
Imports Microsoft.Win32
Imports System
Imports System.Globalization
Imports System.IO
Imports System.Management
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Security.AccessControl
Imports System.Security.Principal
Imports System.Threading

Public Class DiagnosticsForm

#Region "Constants and Variables"
    Private Const ASCOM_PLATFORM_NAME As String = "ASCOM Platform 6"
    Private Const INST_DISPLAY_NAME As String = "DisplayName"
    Private Const INST_DISPLAY_VERSION As String = "DisplayVersion"
    Private Const INST_INSTALL_DATE As String = "InstallDate"
    Private Const INST_INSTALL_LOCATION As String = "InstallLocation"
    Private Const INST_INSTALL_SOURCE As String = "InstallSource"

    Private Const DRIVER_CONNECT_APPLICATION_64BIT = "\DriverConnect64\ASCOM.DriverConnect.exe"
    Private Const DRIVER_CONNECT_APPLICATION_32BIT = "\DriverConnect32\ASCOM.DriverConnect.exe"

    Private Const COMPONENT_CATEGORIES = "Component Categories"
    Private Const ASCOM_ROOT_KEY As String = " (ASCOM Root Key)"
    Private Const TestTelescopeDescription As String = "This is a test telescope"
    Private Const RevisedTestTelescopeDescription As String = "Updated description for test telescope!!!"
    Private Const NewTestTelescopeDescription As String = "New description for test telescope!!!"
    Private Const TOLERANCE_E2 As Double = 0.01 ' Used in evaluating precision match of double values
    Private Const TOLERANCE_E3 As Double = 0.001 ' Used in evaluating precision match of double values
    Private Const TOLERANCE_E4 As Double = 0.0001 ' Used in evaluating precision match of double values
    Private Const TOLERANCE_E5 As Double = 0.00001 ' Used in evaluating precision match of double values
    Private Const TOLERANCE_E6 As Double = 0.000001 ' Used in evaluating precision match of double values
    Private Const TOLERANCE_E7 As Double = 0.0000001 ' Used in evaluating precision match of double values
    Private Const TOLERANCE_E8 As Double = 0.00000001 ' Used in evaluating precision match of double values
    Private Const TOLERANCE_E9 As Double = 0.000000001
    Private Const DOME_SLEW_TIMEOUT As Integer = 240
    Private Const INST_UNINSTALL_STRING As String = "UninstallString"
    Private Const INST_DISPLAY_ICON As String = "DisplayIcon"

    'Astrometry test data for planets obtained from the original 32bit  components
    'The data is for the arbitary test date Thursday, 30 December 2010 09:00:00" 
    Private Const TEST_DATE As String = "Thursday, 30 December 2010 09:00:00" ' Arbitary test date used to generate data above, it must conform to the "F" date format for the invariant culture
    Private Const J2000 As Double = 2451545.0 'Julian day for J2000 epoch
    Private Const INDENT As Integer = 3 ' Display indent for recursive loop output

    Private Const CSIDL_PROGRAM_FILES As Integer = 38 '0x0026
    Private Const CSIDL_PROGRAM_FILESX86 As Integer = 42 '0x002a,
    Private Const CSIDL_WINDOWS As Integer = 36 ' 0x0024,
    Private Const CSIDL_PROGRAM_FILES_COMMONX86 As Integer = 44 ' 0x002c,
    Private Const CSIDL_SYSTEM As Integer = 37 ' 0x0025,
    Private Const CSIDL_SYSTEMX86 As Integer = 41 ' 0x0029,

    Private NMatches, NNonMatches, NExceptions As Integer
    Private ErrorList As New Generic.List(Of String)

    Private TL As TraceLogger
    Private ASCOMRegistryAccess As ASCOM.Utilities.RegistryAccess
    Private WithEvents ASCOMTimer As ASCOM.Utilities.Timer
    Private RecursionLevel As Integer
    Private g_CountWarning, g_CountIssue, g_CountError As Integer
    Private sw, s1, s2 As Stopwatch
    Private DrvHlpUtil As Object
    Private AscomUtil As ASCOM.Utilities.Util
    Private g_Util2 As Object
    Private DecimalSeparator As String = "", ThousandsSeparator As String = ""
    Private AbbreviatedMonthNames() As String = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames ' List of monthnames in current culture language
    Private StartTime As Date
    Private NumberOfTicks As Integer

    Private Nov3 As ASCOM.Astrometry.NOVAS.NOVAS3, Nov31 As ASCOM.Astrometry.NOVAS.NOVAS31
    Private AstroUtil As AstroUtils.AstroUtils
    Private DeviceObject As Object ' Device test object

    Private LastLogFile As String ' Name of last diagnostics log file

    Private Const ArrayCopySize As Integer = 2
    Private IntArray1D(ArrayCopySize) As Integer, IntArray2D(ArrayCopySize, ArrayCopySize) As Integer, IntArray3D(ArrayCopySize, ArrayCopySize, ArrayCopySize) As Integer

    Private DiagnosticsVersion As Version ' Assembly version number of this executable

#End Region

    Private Sub DiagnosticsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Initialise form
        Dim InstallInformation As Generic.SortedList(Of String, String)

        Try
            DiagnosticsVersion = Assembly.GetExecutingAssembly.GetName.Version
            InstallInformation = GetInstallInformation(PLATFORM_INSTALLER_PROPDUCT_CODE, False, True, False) 'Retrieve the current install information
            lblTitle.Text = InstallInformation.Item(INST_DISPLAY_NAME) & " - " & InstallInformation.Item(INST_DISPLAY_VERSION)
            lblResult.Text = ""
            lblAction.Text = ""

            lblMessage.Text = "Your diagnostic log will be created in:" & vbCrLf & vbCrLf &
            System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\ASCOM\Logs " & Format(Now, "yyyy-MM-dd")

            btnLastLog.Enabled = False 'Disable last log button
            sw = New Stopwatch

            If Environment.Is64BitOperatingSystem Then ' We are on a 64bit OS so make both 32 and 64bit Chooser forms available
                ChooseAndConnectToDevice32bitApplicationToolStripMenuItem.Visible = True
                ChooseAndConnectToDevice64bitApplicationToolStripMenuItem.Visible = True
            Else ' We are on a 32bit OS so just make a 32bit Chooser form available
                ChooseAndConnectToDevice32bitApplicationToolStripMenuItem.Visible = True
                ChooseAndConnectToDevice64bitApplicationToolStripMenuItem.Visible = False
            End If

            RefreshTraceItems() ' Get current values for the trace menu settings
            AstroUtil = New AstroUtils.AstroUtils
            Nov3 = New NOVAS.NOVAS3
            Nov31 = New NOVAS.NOVAS31
            AscomUtil = New ASCOM.Utilities.Util
            Me.BringToFront()
        Catch ex As Exception
            EventLogCode.LogEvent("Diagnostics Load", "Exception", EventLogEntryType.Error, EventLogErrors.DiagnosticsLoadException, ex.ToString)
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub btnCOM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCOM.Click
        Dim ASCOMPath As String, ApplicationPath As String = "Path Not Set!"
        Dim PathShell As New System.Text.StringBuilder(260)
        Dim SuccessMessage As String
        Try
            Status("Diagnostics running...")

            TL = New TraceLogger("", "Diagnostics")
            TL.Enabled = True

            btnExit.Enabled = False ' Disable buttons during run
            btnLastLog.Enabled = False
            btnCOM.Enabled = False

            ErrorList.Clear() 'Remove any errors from previous runs
            NMatches = 0
            NNonMatches = 0
            NExceptions = 0

            'Log Diagnostics version information
            TL.LogMessage("Diagnostics", "Version " & DiagnosticsVersion.ToString & ", " & Application.ProductVersion)
            TL.BlankLine()
            TL.LogMessage("Date", Date.Now.ToString)
            TL.LogMessage("TimeZoneName", GetTimeZoneName)
            TL.LogMessage("TimeZoneOffset", TimeZone.CurrentTimeZone.GetUtcOffset(Now).Hours)
            TL.LogMessage("UTCDate", Date.UtcNow)
            TL.LogMessage("Julian date", Date.UtcNow.ToOADate() + 2415018.5)
            TL.BlankLine()
            TL.LogMessage("CurrentCulture", CultureInfo.CurrentCulture.EnglishName &
                                            " " & CultureInfo.CurrentCulture.Name &
                                            " Decimal Separator """ & CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator & """" &
                                            " Number Group Separator """ & CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator & """")
            TL.LogMessage("CurrentUICulture", CultureInfo.CurrentUICulture.EnglishName &
                                            " " & CultureInfo.CurrentUICulture.Name &
                                            " Decimal Separator """ & CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator & """" &
                                            " Number Group Separator """ & CultureInfo.CurrentUICulture.NumberFormat.NumberGroupSeparator & """")
            TL.BlankLine()

            LastLogFile = TL.LogFileName
            Try
                Try
                    ApplicationPath = Assembly.GetEntryAssembly.Location
                    ApplicationPath = ApplicationPath.Remove(ApplicationPath.LastIndexOf("\"))
                    Directory.SetCurrentDirectory(ApplicationPath)
                Catch ex As Exception
                    TL.LogMessage("Diagnostics", "ERROR - Unexpected exception setting current directory. You are likely to get four fails in ReadEph as a result.")
                    TL.LogMessage("Diagnostics", "Application Path: " & ApplicationPath)
                    LogException("Diagnostics", ex.ToString)
                End Try

                DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
                ThousandsSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator

                Try 'Try and create a registryaccess object
                    ASCOMRegistryAccess = New ASCOM.Utilities.RegistryAccess
                Catch ex As Exception
                    TL.LogMessage("Diagnostics", "ERROR - Unexpected exception creating New RegistryAccess object, later steps will show errors")
                    LogException("Diagnostics", ex.ToString)
                End Try

                Try
                    ScanInstalledPlatform()
                Catch ex As Exception
                    LogException("ScanInstalledPlatform", ex.ToString)
                End Try
                Try
                    RunningVersions(TL) 'Log diagnostic information
                Catch ex As Exception
                    LogException("RunningVersions", ex.ToString)
                End Try

                Try
                    ScanDrives() 'Scan PC drives and report information
                Catch ex As Exception
                    LogException("ScanDrives", ex.ToString)
                End Try

                Try
                    ScanFrameworks() 'Report on installed .NET Framework versions
                Catch ex As Exception
                    LogException("ScanFrameworks", ex.ToString)
                End Try

                Try
                    ScanSerial() 'Report serial port information
                Catch ex As Exception
                    LogException("ScanSerial", ex.ToString)
                End Try

                'Scan registry security rights
                Try
                    ScanRegistrySecurity()
                Catch ex As Exception
                    LogException("ScanRegistrySecurity", ex.ToString)
                End Try

                Try
                    ScanASCOMDrivers() : Action("") 'Report installed driver versions
                Catch ex As Exception
                    LogException("ScanASCOMDrivers", ex.ToString)
                End Try

                Try
                    ScanDriverExceptions() : Action("") 'Report drivers listed as exceptions
                Catch ex As Exception
                    LogException("ScanDriverExceptions", ex.ToString)
                End Try

                Try
                    ScanProgramFiles() 'Search for copies of Helper and Helper2.DLL in the wrong places
                Catch ex As Exception
                    LogException("ScanProgramFiles", ex.ToString)
                End Try

                Try
                    ScanProfile() : Action("") 'Report profile information
                Catch ex As Exception
                    LogException("ScanProfile", ex.ToString)
                End Try

                Try
                    ScanRegistry() 'Scan Old ASCOM Registry Profile
                Catch ex As Exception
                    LogException("ScanInstalledPlatform", ex.ToString)
                End Try

                Try
                    ScanProfile55Files() : Action("") 'List contents of Profile 5.5 XML files
                Catch ex As Exception
                    LogException("ScanProfile55Files", ex.ToString)
                End Try

                Try
                    ScanCOMRegistration() 'Report Com Registration
                Catch ex As Exception
                    LogException("ScanCOMRegistration", ex.ToString)
                End Try

                Try
                    ScanForHelperHijacking()
                Catch ex As Exception
                    LogException("ScanInstalledPlatform", ex.ToString)
                End Try

                'Scan files on 32 and 64bit systems
                TL.LogMessage("Platform Files", "")
                ASCOMPath = GetASCOMPath() 'Get relevant 32 or 64bit path to ACOM files
                Try
                    Call ScanPlatformFiles(ASCOMPath) : Action("")
                Catch ex As Exception
                    LogException("ScanPlatformFiles", ex.ToString)
                End Try

                Try
                    ScanDeveloperFiles()
                Catch ex As Exception
                    LogException("ScanDeveloperFiles", ex.ToString)
                End Try

                'List GAC contents
                Try
                    ScanGac()
                Catch ex As Exception
                    LogException("ScanGac", ex.ToString)
                End Try

                'List setup files
                Try
                    ScanLogs()
                Catch ex As Exception
                    LogException("ScanLogs", ex.ToString)
                End Try

                'List Platform 6 install logs
                Try
                    ScanPlatform6Logs()
                Catch ex As Exception
                    LogException("ScanPlatform6Logs", ex.ToString)
                End Try

                'Scan event log messages
                Try
                    ScanEventLog()
                Catch ex As Exception
                    LogException("ScanEventLog", ex.ToString)
                End Try

                'Scan for ASCOM Applications
                Try
                    ScanApplications()
                Catch ex As Exception
                    LogException("ScanApplications", ex.ToString)
                End Try

                TL.BlankLine()
                TL.LogMessage("Diagnostics", "Completed diagnostic run, starting function testing run")
                TL.BlankLine()
                TL.BlankLine()
                Try
                    'Functional tests
                    UtilTests() : Action("")
                Catch ex As Exception
                    LogException("UtilTests", ex.ToString)
                End Try
                Try
                    ProfileTests() : Action("")
                Catch ex As Exception
                    LogException("ProfileTests", ex.ToString)
                End Try
                Try
                    TimerTests() : Action("")
                Catch ex As Exception
                    LogException("TimerTests", ex.ToString)
                End Try
                Try
                    NovasComTests() : Action("")
                Catch ex As Exception
                    LogException("NovasComTests", ex.ToString)
                End Try
                Try
                    KeplerTests() : Action("")
                Catch ex As Exception
                    LogException("KeplerTests", ex.ToString)
                End Try

                Try
                    TransformTest() : Action("")
                Catch ex As Exception
                    LogException("TransformTest", ex.ToString)
                End Try
                Try
                    NOVAS2Tests() : Action("")
                Catch ex As Exception
                    LogException("NOVAS2Tests", ex.ToString)
                End Try
                Try
                    NOVAS3Tests() : Action("")
                Catch ex As Exception
                    LogException("NOVAS3Tests", ex.ToString)
                End Try
                Try
                    NOVAS31Tests() : Action("")
                Catch ex As Exception
                    LogException("NOVAS31Tests", ex.ToString)
                End Try

                Try
                    SimulatorTests() : Action("")
                Catch ex As Exception
                    LogException("SimulatorTests", ex.ToString)
                End Try
                Try
                    AstroUtilsTests() : Action("")
                Catch ex As Exception
                    LogException("AstroUtilsTests", ex.ToString)
                End Try
                Try
                    VideoUtilsTests() : Action("")
                Catch ex As Exception
                    LogException("VideoUtilsTests", ex.ToString)
                End Try

                Try
                    SOFATests() : Action("")
                Catch ex As Exception
                    LogException("SOFATests", ex.ToString)
                End Try
                If (NNonMatches = 0) And (NExceptions = 0) Then
                    SuccessMessage = "Congratulations, all " & NMatches & " function tests passed!"
                Else
                    SuccessMessage = "Completed function testing run: " & NMatches & " matches, " & NNonMatches & " fail(s), " & NExceptions & " exception(s)."
                    TL.BlankLine()
                    TL.LogMessage("Error", "Error List")
                    For Each ErrorMessage As String In ErrorList
                        TL.LogMessageCrLf("Error", ErrorMessage)
                    Next
                    TL.BlankLine()
                    TL.BlankLine()
                End If

                TL.LogMessage("Diagnostics", SuccessMessage)
                TL.Enabled = False
                TL.Dispose()
                TL = Nothing
                Status("Diagnostic log created OK")
                Action(SuccessMessage)
            Catch ex As Exception
                Status("Diagnostics exception, please see log")
                LogException("DiagException", ex.ToString)
                TL.Enabled = False
                TL.Dispose()
                Action("")
                TL = Nothing
            Finally
                Try : ASCOMRegistryAccess.Dispose() : Catch : End Try 'Clean up registryaccess object
                ASCOMRegistryAccess = Nothing
            End Try
            btnLastLog.Enabled = True

        Catch ex1 As Exception
            lblResult.Text = "Can't create log: " & ex1.Message
        End Try
        btnExit.Enabled = True ' Enable buttons during run
        btnCOM.Enabled = True
    End Sub

    Private Sub SOFATests()
        Dim SOFA As SOFA.SOFA
        Dim t1, t2, date1, date2 As Double, j As Integer
        Dim rc, dc, pr, pd, px, rv, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, aob, zob, hob, dob, rob, eo As Double
        Dim ri, di, a, u1, u2, a1, a2, ob1, ob2 As Double

        Status("Testing SOFA")
        TL.LogMessage("SOFATests", "Starting test")
        SOFA = New SOFA.SOFA

        'Af2a tests
        j = SOFA.Af2a("-", 45, 13, 27.2, a)

        CompareDouble("SOFATests", "Af2a", a, -0.78931157943136443, 0.000000000001)
        CompareInteger("SOFATests", "Af2a-status", j, 0)

        'Anp tests
        CompareDouble("SOFATests", "Anp", SOFA.Anp(-0.1), 6.1831853071795866, 0.000000000001)

        'Atci13 tests
        rc = 2.71
        dc = 0.174
        pr = 0.00001
        pd = 0.000005
        px = 0.1
        rv = 55.0
        date1 = 2456165.5
        date2 = 0.401182685

        SOFA.CelestialToIntermediate(rc, dc, pr, pd, px, rv, date1, date2, ri, di, eo)

        CompareDouble("SOFATests", "CelestialToIntermediate-r1", ri, 2.7101215729690389, 0.000000000001, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("SOFATests", "CelestialToIntermediate-r1", di, 0.17293713672182304, 0.000000000001, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("SOFATests", "CelestialToIntermediate-r1", eo, -0.0029006187126573756, 0.00000000000001)

        'Atco13 tests
        rc = 2.71
        dc = 0.174
        pr = 0.00001
        pd = 0.000005
        px = 0.1
        rv = 55.0
        utc1 = 2456384.5
        utc2 = 0.969254051
        dut1 = 0.1550675
        elong = -0.527800806
        phi = -1.2345856
        hm = 2738.0
        xp = 0.000000247230737
        yp = 0.00000182640464
        phpa = 731.0
        tc = 12.8
        rh = 0.59
        wl = 0.55

        j = SOFA.CelestialToObserved(rc, dc, pr, pd, px, rv, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, aob, zob, hob, dob, rob, eo)

        CompareDouble("SOFATests", "CelestialToObserved-aob", aob, 0.0925177448535823, 0.000000000001, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareDouble("SOFATests", "CelestialToObserved-zob", zob, 1.4076614052567671, 0.000000000001, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareDouble("SOFATests", "CelestialToObserved-hob", hob, -0.092651544314031581, 0.000000000001, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("SOFATests", "CelestialToObserved-dob", dob, 0.17166265600755917, 0.000000000001, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareDouble("SOFATests", "CelestialToObserved-rob", rob, 2.7102604535030976, 0.000000000001, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("SOFATests", "CelestialToObserved-eo", eo, -0.0030205483548024128, 0.00000000000001)
        CompareInteger("SOFATests", "CelestialToObserved-status", j, 0)

        'Dtf2d tests

        j = SOFA.Dtf2d("UTC", 1994, 6, 30, 23, 59, 60.13599, u1, u2)

        CompareDouble("SOFATests", "Dtf2d", u1 + u2, 2449534.49999, 0.000001)
        CompareInteger("SOFATests", "Dtf2d-status", j, 0)

        'Eo06a tests
        eo = SOFA.Eo06a(2400000.5, 53736.0)

        CompareDouble("SOFATests", "Eo06a-eo", eo, -0.0013328823719418338, 0.000000000000001)

        'Atic13 tests
        ri = 2.7101215729690389
        di = 0.17293713672182304
        date1 = 2456165.5
        date2 = 0.401182685

        SOFA.IntermediateToCelestial(ri, di, date1, date2, rc, dc, eo)

        CompareDouble("SOFATests", "IntermediateToCelestial-rc", rc, 2.7101265045313747, 0.000000000001, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("SOFATests", "IntermediateToCelestial-dc", dc, 0.17406325376283424, 0.000000000001, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareDouble("SOFATests", "IntermediateToCelestial-eo", eo, -0.0029006187126573756, 0.00000000000001)

        'Atio13 tests
        ri = 2.7101215729690389
        di = 0.17293713672182304
        utc1 = 2456384.5
        utc2 = 0.969254051
        dut1 = 0.1550675
        elong = -0.527800806
        phi = -1.2345856
        hm = 2738.0
        xp = 0.000000247230737
        yp = 0.00000182640464
        phpa = 731.0
        tc = 12.8
        rh = 0.59
        wl = 0.55

        j = SOFA.IntermediateToObserved(ri, di, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, aob, zob, hob, dob, rob)

        CompareDouble("SOFATests", "IntermediateToObserved-aob", aob, 0.0923395222479499, 0.000000000001, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareDouble("SOFATests", "IntermediateToObserved-zob", zob, 1.4077587045137225, 0.000000000001, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareDouble("SOFATests", "IntermediateToObserved-hob", hob, -0.092476198797820056, 0.000000000001, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("SOFATests", "IntermediateToObserved-dob", dob, 0.17176534357582651, 0.000000000001, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareDouble("SOFATests", "IntermediateToObserved-rob", rob, 2.7100851079868864, 0.000000000001, DoubleDisplayAs.HoursMinutesSeconds)
        CompareInteger("SOFATests", "IntermediateToObserved-status", j, 0)

        'Atoc13 tests
        utc1 = 2456384.5
        utc2 = 0.969254051
        dut1 = 0.1550675
        elong = -0.527800806
        phi = -1.2345856
        hm = 2738.0
        xp = 0.000000247230737
        yp = 0.00000182640464
        phpa = 731.0
        tc = 12.8
        rh = 0.59
        wl = 0.55

        ob1 = 2.7100851079868864
        ob2 = 0.17176534357582651
        j = SOFA.ObservedToCelestial("R", ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, rc, dc)
        CompareDouble("SOFATests", "ObservedToCelestial-R-rc", rc, 2.7099567446610004, 0.000000000001, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("SOFATests", "ObservedToCelestial-R-dc", dc, 0.17416965008953986, 0.000000000001, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareInteger("SOFATests", "ObservedToCelestial-R-status", j, 0)

        ob1 = -0.092476198797820056
        ob2 = 0.17176534357582651
        j = SOFA.ObservedToCelestial("H", ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, rc, dc)
        CompareDouble("SOFATests", "ObservedToCelestial-H-rc", rc, 2.7099567446610004, 0.000000000001, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("SOFATests", "ObservedToCelestial-H-dc", dc, 0.17416965008953986, 0.000000000001, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareInteger("SOFATests", "ObservedToCelestial-H-status", j, 0)

        ob1 = 0.0923395222479499
        ob2 = 1.4077587045137225
        j = SOFA.ObservedToCelestial("A", ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, rc, dc)
        CompareDouble("SOFATests", "ObservedToCelestial-A-rc", rc, 2.7099567446610004, 0.000000000001, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("SOFATests", "ObservedToCelestial-A-dc", dc, 0.17416965008953986, 0.000000000001, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareInteger("SOFATests", "ObservedToCelestial-A-status", j, 0)

        'Atoi13 tests
        utc1 = 2456384.5
        utc2 = 0.969254051
        dut1 = 0.1550675
        elong = -0.527800806
        phi = -1.2345856
        hm = 2738.0
        xp = 0.000000247230737
        yp = 0.00000182640464
        phpa = 731.0
        tc = 12.8
        rh = 0.59
        wl = 0.55

        ob1 = 2.7100851079868864
        ob2 = 0.17176534357582651
        j = SOFA.ObservedToIntermediate("R", ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ri, di)
        CompareDouble("SOFATests", "ObservedToIntermediate-ri", ri, 2.7101215744491358, 0.000000000001, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("SOFATests", "ObservedToIntermediate-di", di, 0.17293718391145677, 0.000000000001, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareInteger("SOFATests", "ObservedToIntermediate-status", j, 0)

        ob1 = -0.092476198797820056
        ob2 = 0.17176534357582651
        j = SOFA.ObservedToIntermediate("H", ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ri, di)
        CompareDouble("SOFATests", "ObservedToIntermediate-ri", ri, 2.7101215744491358, 0.000000000001, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("SOFATests", "ObservedToIntermediate-di", di, 0.17293718391145677, 0.000000000001, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareInteger("SOFATests", "ObservedToIntermediate-status", j, 0)

        ob1 = 0.0923395222479499
        ob2 = 1.4077587045137225
        j = SOFA.ObservedToIntermediate("A", ob1, ob2, utc1, utc2, dut1, elong, phi, hm, xp, yp, phpa, tc, rh, wl, ri, di)
        CompareDouble("SOFATests", "ObservedToIntermediate-ri", ri, 2.7101215744491358, 0.000000000001, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("SOFATests", "ObservedToIntermediate-di", di, 0.17293718391145677, 0.000000000001, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareInteger("SOFATests", "ObservedToIntermediate-status", j, 0)

        ' TaiTT tests
        j = SOFA.TaiTt(2453750.5, 0.892482639, t1, t2)
        CompareDouble("SOFATests", "TaiTT-t1", t1, 2453750.5, 0.000001)
        CompareDouble("SOFATests", "TaiTT-t2", t2, 0.892855139, 0.000000000001)
        CompareInteger("SOFATests", "TaiTT-status", j, 0)

        ' TaiUtc tests
        j = SOFA.TaiUtc(2453750.5, 0.892482639, u1, u2)
        CompareDouble("SOFATests", "TaiUtc-u1", u1, 2453750.5, 0.000001)
        CompareDouble("SOFATests", "TaiUtc-u2", u2, 0.89210069455555552, 0.000000000001)
        CompareInteger("SOFATests", "TaiUtc-status", j, 0)

        ' Tf2a tests
        j = SOFA.Tf2a("+", 4, 58, 20.2, a)

        CompareDouble("SOFATests", "Tf2a", a, 1.3017392781895374, 0.000000000001)
        CompareInteger("SOFATests", "Tf2a-status", j, 0)

        ' TTTai tests
        j = SOFA.TtTai(2453750.5, 0.892482639, a1, a2)
        CompareDouble("SOFATests", "TtTai-a1", a1, 2453750.5, 0.000001)
        CompareDouble("SOFATests", "TtTai-a2", a2, 0.892110139, 0.000000000001)
        CompareInteger("SOFATests", "TtTai-status", j, 0)

        ' UtcTai tests
        j = SOFA.UtcTai(2453750.5, 0.892100694, u1, u2)

        CompareDouble("SOFATests", "UtcTai-u1", u1, 2453750.5, 0.000001)
        CompareDouble("SOFATests", "UtcTai-u2", u2, 0.89248263844444442, 0.000000000001)
        CompareInteger("SOFATests", "UtcTai-status", j, 0)

        Status("")
        Action("")

        TL.BlankLine()

    End Sub

    Private Enum ApplicationList
        ACPApplication
        ACPFiles
        Alcyone
        CCDWare
        DiffractionLtd
        FocusMax
        GeminiControlCenter
        MaximDL
        Pinpoint
        StarryNight
        SWBisque
        TheSkyX
    End Enum

    Private Sub ScanApplications()
        Status("Scanning Applications")
        TL.LogMessage("ScanApplications", "Starting scan")
        For Each App As ApplicationList In System.Enum.GetValues(GetType(ApplicationList))
            ScanApplication(App)
        Next
        Status("")
        Action("")
    End Sub

    Private Sub ScanApplication(ByVal Application As ApplicationList)
        Action(Application.ToString)
        Select Case Application
            Case ApplicationList.ACPApplication
                GetApplicationViaAppid(Application, "acp.exe")
            Case ApplicationList.ACPFiles
                GetApplicationViaDirectory(Application, "ACP Obs Control")
            Case ApplicationList.Alcyone
                GetApplicationViaDirectory(Application, "Alcyone")
            Case ApplicationList.CCDWare
                GetApplicationViaDirectory(Application, "CCDWare")
            Case ApplicationList.DiffractionLtd
                GetApplicationViaDirectory(Application, "Diffraction Limited")
            Case ApplicationList.FocusMax
                GetApplicationViaDirectory(Application, "FocusMax")
            Case ApplicationList.GeminiControlCenter
                GetApplicationViaDirectory(Application, "Gemini Control Center")
            Case ApplicationList.MaximDL
                GetApplicationViaProgID(Application, "Maxim.Application")
            Case ApplicationList.Pinpoint
                GetApplicationViaDirectory(Application, "Pinpoint")
            Case ApplicationList.StarryNight
                GetApplicationViaSubDirectories(Application, "*Starry Night*")
            Case ApplicationList.TheSkyX
                GetApplicationViaProgID(Application, "TheSkyXAdaptor.TheSky")
            Case ApplicationList.SWBisque
                GetApplicationViaDirectory(Application, "Software Bisque")
            Case Else
                LogError("ScanApplication", "Unimplemented application test for: " & Application.ToString)
        End Select
    End Sub

    Private Sub GetApplicationViaSubDirectories(ByVal Application As ApplicationList, ByVal AppDirectory As String)
        Dim PathShell As New System.Text.StringBuilder(260), Directories As Generic.List(Of String)
        If ApplicationBits() = Bitness.Bits64 Then
            'Find the programfiles (x86) path
            SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILESX86, False)
        Else '32bits
            SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILES, False)
        End If
        Try
            Directories = Directory.GetDirectories(PathShell.ToString, AppDirectory, IO.SearchOption.TopDirectoryOnly).ToList
            For Each Dir As String In Directories
                GetApplicationViaDirectory(Application, Path.GetFileName(Dir))
            Next
        Catch ex As DirectoryNotFoundException
            TL.LogMessage("ScanApplication", "Application " & Application.ToString & " not installed in " & PathShell.ToString & "\" & AppDirectory)
        Catch ex As Exception
            LogError("GetApplicationViaSubDirectories", "Exception: " & ex.ToString)
        End Try
    End Sub

    Private Sub GetApplicationViaDirectory(ByVal Application As ApplicationList, ByVal AppDirectory As String)
        Dim PathShell As New System.Text.StringBuilder(260), AppPath As String, Executables As Generic.List(Of String)
        If ApplicationBits() = Bitness.Bits64 Then
            'Find the programfiles (x86) path
            SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILESX86, False)
        Else '32bits
            SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILES, False)
        End If
        AppPath = PathShell.ToString & "\" & AppDirectory
        Try
            Executables = Directory.GetFiles(AppPath, "*.exe", IO.SearchOption.AllDirectories).ToList
            Executables.AddRange(Directory.GetFiles(AppPath, "*.dll", IO.SearchOption.AllDirectories).ToList)
            If Executables.Count = 0 Then 'No executables found
                TL.LogMessage("ScanApplication", "Application " & Application.ToString & " not found in " & AppPath)
            Else ' Some exectables were found
                TL.LogMessage("ScanApplication", "Found " & Application.ToString)

                For Each Executable As String In Executables
                    FileDetails(Path.GetDirectoryName(Executable) & "\", Path.GetFileName(Executable))
                Next
            End If
        Catch ex As DirectoryNotFoundException
            TL.LogMessage("ScanApplication", "Application " & Application.ToString & " not installed in " & AppPath)
        Catch ex As Exception
            LogError("GetApplicationViaDirectory", "Exception: " & ex.ToString)
        End Try
    End Sub

    Private Sub GetApplicationViaProgID(ByVal Application As ApplicationList, ByVal ProgID As String)
        Dim Reg As RegistryAccess, AppKey As RegistryKey, CLSIDString As String, FileName As String
        Reg = New RegistryAccess

        Try
            AppKey = Reg.OpenSubKey(Registry.ClassesRoot, ProgID & "\CLSID", False, RegistryAccess.RegWow64Options.KEY_WOW64_32KEY)
            CLSIDString = AppKey.GetValue("", "")
            AppKey.Close()
            If Not String.IsNullOrEmpty(CLSIDString) Then 'Got a GUID value so try and process it
                AppKey = Reg.OpenSubKey(Registry.ClassesRoot, "CLSID\" & CLSIDString & "\LocalServer32", False, RegistryAccess.RegWow64Options.KEY_WOW64_32KEY)
                FileName = AppKey.GetValue("", "")
                FileName = FileName.Trim(New Char() {""""}) 'TrimChars)
                If Not String.IsNullOrEmpty(FileName) Then 'We have a file name so see if it exists

                    If File.Exists(FileName) Then 'Get details
                        TL.LogMessage("ScanApplication", "Found " & Application.ToString)
                        FileDetails(Path.GetDirectoryName(FileName) & "\", Path.GetFileName(FileName))
                    Else
                        TL.LogMessage("ScanApplication", "Cannot find executable: " & FileName & " " & Application.ToString & " not found")
                    End If

                Else
                    TL.LogMessage("ScanApplication", "CLSID entry found but this has no file name value " & Application.ToString & " not found")
                End If
            Else 'No valid value so assume not installed
                TL.LogMessage("ScanApplication", "AppID entry found but this has no AppID value " & Application.ToString & " not found")
            End If
        Catch ex As ProfilePersistenceException 'Key does not exist
            TL.LogMessage("ScanApplication", "Application " & Application.ToString & " not found")
        End Try

    End Sub

    Private Sub GetApplicationViaAppid(ByVal Application As ApplicationList, ByVal Executable As String)
        Dim Reg As RegistryAccess, AppKey As RegistryKey, CLSIDString As String, FileName As String
        Reg = New RegistryAccess

        AppKey = Registry.ClassesRoot.OpenSubKey("AppId\" & Executable, False)
        If Not AppKey Is Nothing Then
            CLSIDString = AppKey.GetValue("AppID", "")
            AppKey.Close()
            If Not String.IsNullOrEmpty(CLSIDString) Then 'Got a GUID value so try and process it
                Try
                    AppKey = Reg.OpenSubKey(Registry.ClassesRoot, "CLSID\" & CLSIDString & "\LocalServer32", False, RegistryAccess.RegWow64Options.KEY_WOW64_32KEY)
                    FileName = AppKey.GetValue("", "")
                    If Not String.IsNullOrEmpty(FileName) Then 'We have a file name so see if it exists
                        If File.Exists(FileName) Then 'Get details
                            TL.LogMessage("ScanApplication", "Found " & Application.ToString)
                            FileDetails(Path.GetDirectoryName(FileName) & "\", Path.GetFileName(FileName))
                        Else
                            TL.LogMessage("ScanApplication", "Cannot find executable: " & FileName & " " & Application.ToString & " not found")
                        End If

                    Else
                        TL.LogMessage("ScanApplication", "CLSID entry found but this has no file name value " & Application.ToString & " not found")
                    End If
                Catch ex As ProfilePersistenceException 'Key does not exist
                    TL.LogMessage("ScanApplication", "Application " & Application.ToString & " not found")
                End Try
            Else 'No valid value so assume not installed
                TL.LogMessage("ScanApplication", "AppID entry found but this has no AppID value " & Application.ToString & " not found")
            End If
        Else
            TL.LogMessage("ScanApplication", "Application " & Application.ToString & " not found")
        End If
    End Sub

    Private Function GetASCOMPath() As String
        Dim PathShell As New System.Text.StringBuilder(260), ASCOMPath As String

        If System.IntPtr.Size = 8 Then 'We are on a 64bit OS so look in the 32bit locations for files
            SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILES_COMMONX86, False)
            ASCOMPath = PathShell.ToString & "\ASCOM\"
        Else 'we are on a 32bit OS so look in the standard position
            ASCOMPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) & "\ASCOM\"
        End If
        Return ASCOMPath
    End Function

    Private Sub ScanDriverExceptions()
        ListDrivers(PLATFORM_VERSION_EXCEPTIONS, "ForcedPlatformVersion")
        ListDrivers(PLATFORM_VERSION_SEPARATOR_EXCEPTIONS, "ForcedPlatformSeparator")
        ListDrivers(DRIVERS_32BIT, "Non64BitDrivers")
        ListDrivers(DRIVERS_64BIT, "Non32BitDrivers")
        TL.BlankLine()
    End Sub

    Private Sub ListDrivers(ByVal DriverCategory As String, ByVal Description As String)
        Dim Prof As RegistryAccess, Contents As Generic.SortedList(Of String, String)

        Try
            Prof = New RegistryAccess()
            Contents = Prof.EnumProfile(DriverCategory)
            For Each ContentItem As Generic.KeyValuePair(Of String, String) In Contents
                TL.LogMessage(Description, ContentItem.Key & " """ & ContentItem.Value & """")
            Next
            TL.BlankLine()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SimulatorTests()
        Dim Sim As SimulatorDescriptor
        Dim DiagnosticsFullVersionNumber As String
        Dim DiagnosticsMajorMinorVersionNumber As String
        Dim DiagnosticsMajorNumber As String

        DiagnosticsFullVersionNumber = DiagnosticsVersion.ToString()
        DiagnosticsMajorMinorVersionNumber = DiagnosticsVersion.Major.ToString() & "." & DiagnosticsVersion.Minor.ToString()
        DiagnosticsMajorNumber = DiagnosticsVersion.Major.ToString()

        Sim = New SimulatorDescriptor
        Sim.ProgID = "ASCOM.Simulator.Telescope"
        Sim.Description = "Platform 6 Telescope Simulator"
        Sim.DeviceType = "Telescope"
        Sim.Name = "Simulator"
        Sim.DriverVersion = DiagnosticsFullVersionNumber
        Sim.InterfaceVersion = 3
        Sim.IsPlatform5 = False
        Sim.SixtyFourBit = True
        Sim.AxisRates = New Double(,) {{0.0, 0.5}, {1.0 / 3.0, 1.0}} ' Axis rates relative to MaxRate
        Sim.AxisRatesRelative = True
        TestSimulator(Sim)
        Sim = Nothing

        Sim = New SimulatorDescriptor
        Sim.ProgID = "ScopeSim.Telescope"
        Sim.Description = "Platform 5 Telescope Simulator"
        Sim.DeviceType = "Telescope"
        Sim.Name = "Simulator"
        Sim.DriverVersion = "5.0"
        Sim.InterfaceVersion = 2
        Sim.IsPlatform5 = True
        Sim.SixtyFourBit = True
        Sim.AxisRates = New Double(,) {{0.0}, {8.0}} ' Absolute axis rates
        Sim.AxisRatesRelative = False
        TestSimulator(Sim)
        Sim = Nothing

        If True Then
            Sim = New SimulatorDescriptor
            Sim.ProgID = "CCDSimulator.Camera"
            Sim.Description = "Platform 5 Camera Simulator"
            Sim.DeviceType = "Camera"
            Sim.Name = "ASCOM CCD camera simulator"
            Sim.DriverVersion = "5.0"
            Sim.InterfaceVersion = 2
            Sim.SixtyFourBit = False
            Sim.IsPlatform5 = True
            TestSimulator(Sim)
            Sim = Nothing

            Sim = New SimulatorDescriptor
            Sim.ProgID = "ASCOM.Simulator.Camera"
            Sim.Description = "Platform 6 Camera Simulator"
            Sim.DeviceType = "Camera"
            Sim.Name = "Sim "
            Sim.DriverVersion = DiagnosticsMajorMinorVersionNumber
            Sim.InterfaceVersion = 2
            Sim.IsPlatform5 = False
            Sim.SixtyFourBit = True
            TestSimulator(Sim)
            Sim = Nothing

            Sim = New SimulatorDescriptor
            Sim.ProgID = "FilterWheelSim.FilterWheel"
            Sim.Description = "Platform 5 FilterWheel Simulator"
            Sim.DeviceType = "FilterWheel"
            Sim.Name = "xxxx"
            Sim.DriverVersion = "5.0"
            Sim.InterfaceVersion = 1
            Sim.IsPlatform5 = True
            Sim.SixtyFourBit = True
            TestSimulator(Sim)
            Sim = Nothing

            Sim = New SimulatorDescriptor
            Sim.ProgID = "ASCOM.Simulator.FilterWheel"
            Sim.Description = "Platform 6 FilterWheel Simulator"
            Sim.DeviceType = "FilterWheel"
            Sim.Name = "Filter Wheel Simulator .NET"
            Sim.DriverVersion = DiagnosticsMajorNumber & ".0"
            Sim.InterfaceVersion = 2
            Sim.IsPlatform5 = False
            Sim.SixtyFourBit = True
            TestSimulator(Sim)
            Sim = Nothing

            Sim = New SimulatorDescriptor
            Sim.ProgID = "FocusSim.Focuser"
            Sim.Description = "Platform 5 Focuser Simulator"
            Sim.DeviceType = "Focuser"
            Sim.Name = "Simulator"
            Sim.DriverVersion = "5.0"
            Sim.InterfaceVersion = 1
            Sim.IsPlatform5 = True
            Sim.SixtyFourBit = True
            TestSimulator(Sim)
            Sim = Nothing

            Sim = New SimulatorDescriptor
            Sim.ProgID = "ASCOM.Simulator.Focuser"
            Sim.Description = "Platform 6 Focuser Simulator"
            Sim.DeviceType = "Focuser"
            Sim.Name = "ASCOM.Simulator.Focuser"
            Sim.DriverVersion = DiagnosticsMajorNumber & ".0"
            Sim.InterfaceVersion = 2
            Sim.IsPlatform5 = False
            Sim.SixtyFourBit = True
            TestSimulator(Sim)
            Sim = Nothing

            Sim = New SimulatorDescriptor
            Sim.ProgID = "ASCOM.Simulator.SafetyMonitor"
            Sim.Description = "Platform 6 Safety Monitor Simulator"
            Sim.DeviceType = "SafetyMonitor"
            Sim.Name = "ASCOM.Simulator.SafetyMonitor"
            Sim.DriverVersion = DiagnosticsMajorNumber & ".0"
            Sim.InterfaceVersion = 2
            Sim.IsPlatform5 = False
            Sim.SixtyFourBit = True
            TestSimulator(Sim)
            Sim = Nothing

            Sim = New SimulatorDescriptor
            Sim.ProgID = "SwitchSim.Switch"
            Sim.Description = "Platform 5 Switch Simulator"
            Sim.DeviceType = "Switch"
            Sim.Name = "Switch Simulator"
            Sim.DriverVersion = "5.0"
            Sim.InterfaceVersion = 1
            Sim.IsPlatform5 = True
            Sim.SixtyFourBit = True
            TestSimulator(Sim)
            Sim = Nothing

            Sim = New SimulatorDescriptor
            Sim.ProgID = "ASCOM.Simulator.Switch"
            Sim.Description = "Platform 6 Switch Simulator"
            Sim.DeviceType = "Switch"
            Sim.Name = "ASCOM Switch V2 Simulator"
            Sim.DriverVersion = DiagnosticsMajorMinorVersionNumber
            Sim.InterfaceVersion = 2
            Sim.IsPlatform5 = False
            Sim.SixtyFourBit = True
            TestSimulator(Sim)
            Sim = Nothing

            Sim = New SimulatorDescriptor
            Sim.ProgID = "DomeSim.Dome"
            Sim.Description = "Dome Simulator"
            Sim.DeviceType = "Dome"
            Sim.Name = "Simulator"
            Sim.DriverVersion = "5.0"
            Sim.InterfaceVersion = 1
            Sim.IsPlatform5 = True
            Sim.SixtyFourBit = True
            TestSimulator(Sim)
            Sim = Nothing

            Sim = New SimulatorDescriptor
            Sim.ProgID = "ASCOM.Simulator.Dome"
            Sim.Description = "Platform 6 Dome Simulator"
            Sim.DeviceType = "Dome"
            Sim.Name = "Simulator"
            Sim.DriverVersion = DiagnosticsMajorMinorVersionNumber
            Sim.InterfaceVersion = 2
            Sim.IsPlatform5 = False
            Sim.SixtyFourBit = True
            TestSimulator(Sim)
            Sim = Nothing

            Sim = New SimulatorDescriptor
            Sim.ProgID = "ASCOM.Simulator.Video"
            Sim.Description = "Platform 6 Video Simulator"
            Sim.DeviceType = "Video"
            Sim.Name = "Video Simulator"
            Sim.DriverVersion = DiagnosticsMajorMinorVersionNumber
            Sim.InterfaceVersion = 1
            Sim.IsPlatform5 = False
            Sim.SixtyFourBit = True
            TestSimulator(Sim)
            Sim = Nothing

            Sim = New SimulatorDescriptor
            Sim.ProgID = "ASCOM.Simulator.ObservingConditions"
            Sim.Description = "Platform 6 ObservingConditions Simulator"
            Sim.DeviceType = "ObservingConditions"
            Sim.Name = "ASCOM Observing Conditions Simulator"
            Sim.DriverVersion = DiagnosticsMajorMinorVersionNumber
            Sim.InterfaceVersion = 1
            Sim.IsPlatform5 = False
            Sim.SixtyFourBit = True
            TestSimulator(Sim)
            Sim = Nothing

            Sim = New SimulatorDescriptor
            Sim.ProgID = "ASCOM.OCH.ObservingConditions"
            Sim.Description = "Platform 6 ObservingConditions Hub"
            Sim.DeviceType = "ObservingConditionsHub"
            Sim.Name = "ASCOM Observing Conditions Hub (OCH)"
            Sim.DriverVersion = DiagnosticsMajorMinorVersionNumber
            Sim.InterfaceVersion = 1
            Sim.IsPlatform5 = False
            Sim.SixtyFourBit = True
            TestSimulator(Sim)
            Sim = Nothing

        End If
        TL.BlankLine()
    End Sub

    Private Sub TestSimulator(ByVal Sim As SimulatorDescriptor)
        Dim RetValString As String, DeviceAxisRates As Object, ct As Integer, DeviceType As Type
        Dim prof As Profile, MaxSlewRate As Double
        Dim returnString As String

        Const MAX_SLEW_RATE_PROFILE_NAME As String = "MaxSlewRate" ' Name of the Profile variable holding the maximum slew rate

        Try
            Status(Sim.Description)

            If (ApplicationBits() = Bitness.Bits64) And (Not Sim.SixtyFourBit) Then ' We are on a 64 bit OS and are testing a 32bit only app - so skip the test!
                TL.LogMessage("TestSimulator", Sim.ProgID & " " & Sim.Description & " - Skipping test as this driver is not 64bit compatible")
            Else
                Try
                    TL.LogMessage("TestSimulator", "CreateObject for Device: " & Sim.ProgID & " " & Sim.Description)
                    DeviceType = Type.GetTypeFromProgID(Sim.ProgID)
                    DeviceObject = Activator.CreateInstance(DeviceType)
                    Select Case Sim.DeviceType
                        Case "Focuser"
                            Try
                                DeviceObject.Connected = True
                                Compare("TestSimulator", "Connected OK", "True", "True")
                            Catch ex1 As MissingMemberException ' Could be a Platform 5 driver that uses "Link" instead of "Connected"
                                DeviceObject.Link = True ' Try Link, if it fails the outer try will catch the exception
                                Compare("TestSimulator", "Linked OK", "True", "True")
                            End Try
                        Case "ObservingConditionsHub"
                            ' The ObservingConditions Hub is unconfigured on initial installation and so has a special test mode that fakes a valid configuration
                            ' This unpublicised Action initiates the test mode
                            returnString = DeviceObject.Action("SetTestMode", "")
                            TL.LogMessage("TestSimulator", "Observing conditions hub test mode request returned: " & returnString)
                            DeviceObject.Connected = True
                            Compare("TestSimulator", "Connected OK", "True", "True")
                        Case Else ' Everything else should be Connected 
                            DeviceObject.Connected = True
                            Compare("TestSimulator", "Connected OK", "True", "True")
                    End Select

                    Thread.Sleep(1000)

                    Try
                        RetValString = DeviceObject.Description
                        Compare("TestSimulator", "Description member is present in Platform 6 Simulator", "True", "True")
                        NMatches += 1
                    Catch ex1 As MissingMemberException
                        If Sim.IsPlatform5 Then
                            Compare("TestSimulator", "Description member is not present in Platform 5 Simulator", "True", "True")
                        Else
                            LogException("TestSimulator", "Description Exception: " & ex1.ToString)
                        End If
                    Catch ex1 As Exception
                        LogException("TestSimulator", "Description Exception: " & ex1.ToString)
                    End Try

                    Try
                        RetValString = DeviceObject.DriverInfo
                        Compare("TestSimulator", "DriverInfo member is present in Platform 6 Simulator", "True", "True")
                    Catch ex1 As MissingMemberException
                        If Sim.IsPlatform5 Then
                            Compare("TestSimulator", "DriverInfo member is not present in Platform 5 Simulator", "True", "True")
                        Else
                            LogException("TestSimulator", "DriverInfo Exception: " & ex1.ToString)
                        End If
                    Catch ex1 As Exception
                        LogException("TestSimulator", "DriverInfo Exception: " & ex1.ToString)
                    End Try

                    Try
                        Compare("TestSimulator", Sim.DeviceType & " " & "Name", DeviceObject.Name, Sim.Name)
                    Catch ex1 As MissingMemberException
                        If Sim.IsPlatform5 Then
                            Compare("TestSimulator", "Name member is not present in Platform 5 Simulator", "True", "True")
                        Else
                            LogException("TestSimulator", "Name Exception: " & ex1.ToString)
                        End If
                    Catch ex1 As Exception
                        LogException("TestSimulator", "Name Exception: " & ex1.ToString)
                    End Try

                    Try
                        Compare("TestSimulator", Sim.DeviceType & " " & "InterfaceVersion", DeviceObject.Interfaceversion, Sim.InterfaceVersion)
                    Catch ex1 As COMException
                        If ex1.ErrorCode = &H80040400 And Sim.DeviceType = "Telescope" Then
                            Compare("TestSimulator", "Simulator is in Interface V1 mode", "True", "True")
                        End If
                    Catch ex1 As MissingMemberException
                        If Sim.IsPlatform5 Then
                            Compare("TestSimulator", "Interfaceversion member is not present in Platform 5 Simulator", "True", "True")
                        Else
                            LogException("TestSimulator", "Interfaceversion Exception: " & ex1.ToString)
                        End If
                    Catch ex1 As Exception
                        LogException("TestSimulator", "Interfaceversion Exception: " & ex1.ToString)
                    End Try

                    Try
                        Compare("TestSimulator", Sim.DeviceType & " " & "DriverVersion", DeviceObject.DriverVersion, Sim.DriverVersion)
                    Catch ex1 As COMException
                        If ex1.ErrorCode = &H80040400 And Sim.DeviceType = "Telescope" Then
                            Compare("TestSimulator", "Simulator is in Interface V1 mode", "True", "True")
                        End If
                    Catch ex1 As MissingMemberException
                        If Sim.IsPlatform5 Then
                            Compare("TestSimulator", "DriverVersion member is not present in Platform 5 Simulator", "True", "True")
                        Else
                            LogException("TestSimulator", "DriverVersion Exception: " & ex1.ToString)
                        End If
                    Catch ex1 As Exception
                        LogException("TestSimulator", "DriverVersion Exception: " & ex1.ToString)
                    End Try

                    Select Case Sim.DeviceType
                        Case "Telescope"
                            DeviceTest("Telescope", "UnPark")
                            DeviceTest("Telescope", "TrackingTrue")
                            DeviceTest("Telescope", "SiderealTime")
                            DeviceTest("Telescope", "RightAscension")
                            DeviceTest("Telescope", "TargetDeclination")
                            DeviceTest("Telescope", "TargetRightAscension")
                            DeviceTest("Telescope", "Slew")
                            DeviceTest("Telescope", "TrackingRates")
                            DeviceAxisRates = DeviceTest("Telescope", "AxisRates")
                            Try
                                ' The maximum slew rate is a user configurable value so we need to read it here in order to conduct slew rate value tests
                                ' Get the maximum slew rate stored in the simulator Profile for use in relative rates tests
                                prof = New Profile()

                                MaxSlewRate = CDbl(prof.GetValue(Sim.ProgID, MAX_SLEW_RATE_PROFILE_NAME))
                                prof.Dispose()

                                ct = DeviceObject.InterfaceVersion()
                                ct = 0
                                For Each AxRte As Object In DeviceAxisRates
                                    If Sim.AxisRatesRelative Then ' Relative axis rates so multiply the provided fractions of MaxRate by MaxRate
                                        CompareDouble("TestSimulator", "AxisRate Minimum", AxRte.Minimum, Sim.AxisRates(0, ct) * MaxSlewRate, 0.000001)
                                        CompareDouble("TestSimulator", "AxisRate Maximum", AxRte.Maximum, Sim.AxisRates(1, ct) * MaxSlewRate, 0.000001)
                                    Else ' Abolute axis rates so test as given
                                        CompareDouble("TestSimulator", "AxisRate Minimum", AxRte.Minimum, Sim.AxisRates(0, ct), 0.000001)
                                        CompareDouble("TestSimulator", "AxisRate Maximum", AxRte.Maximum, Sim.AxisRates(1, ct), 0.000001)
                                    End If
                                    ct += 1
                                Next
                            Catch ex1 As COMException
                                If ex1.ErrorCode = &H80040400 Then
                                    Compare("TestSimulator", "TrackingRates - Simulator is in Interface V1 mode", "True", "True")
                                End If
                            End Try

                        Case "Camera"
                            DeviceTest("Camera", "StartExposure")
                        Case "FilterWheel"
                            DeviceTest("FilterWheel", "Position")
                        Case "Focuser"
                            DeviceTest("Focuser", "Move")
                        Case "SafetyMonitor"
                            DeviceTest("SafetyMonitor", "IsSafe")
                        Case "Switch"
                            If Sim.IsPlatform5 Then
                                DeviceTest("Switch", "GetSwitch")
                                DeviceTest("Switch", "GetSwitchName")
                            Else ' Is Platform v6.1
                                DeviceTest("Switch", "MaxSwitch")
                                DeviceTest("Switch", "CanWrite")
                                DeviceTest("Switch", "GetSwitch")
                                DeviceTest("Switch", "GetSwitchDescription")
                                DeviceTest("Switch", "GetSwitchName")
                                DeviceTest("Switch", "GetSwitchValue")
                                DeviceTest("Switch", "MaxSwitchValue")
                                DeviceTest("Switch", "MinSwitchValue")
                                DeviceTest("Switch", "SwitchStep")
                            End If
                        Case "Dome"
                            DeviceTest("Dome", "OpenShutter")
                            DeviceTest("Dome", "Slewing")
                            DeviceTest("Dome", "ShutterStatus")
                            DeviceTest("Dome", "SlewToAltitude")
                            DeviceTest("Dome", "SlewToAzimuth")
                            DeviceTest("Dome", "CloseShutter")
                        Case "Video"
                            DeviceTest("Video", "BitDepth")
                            DeviceTest("Video", "CanConfigureDeviceProperties")
                            DeviceTest("Video", "ExposureMin")
                            DeviceTest("Video", "Height")
                            DeviceTest("Video", "Width")
                        Case "ObservingConditions", "ObservingConditionsHub"
                            DeviceTest("ObservingConditions", "AveragePeriod")
                            DeviceTest("ObservingConditions", "TimeSinceLastUpdate")
                        Case Else
                            LogException("TestSimulator", "Unknown device type: " & Sim.DeviceType)
                    End Select

                    ' Disconnect the device
                    Select Case Sim.DeviceType
                        Case "Focuser"
                            Try
                                DeviceObject.Connected = False
                                NMatches += 1
                            Catch ex1 As MissingMemberException ' Could be a Platform 5 driver that uses "Link" instead of "Connected"
                                TL.LogMessage("TestSimulator", "Focuser Connected member missing, using Link instead")
                                DeviceObject.Link = False ' Try Link, if it fails the outer try will catch the exception
                                NMatches += 1
                            End Try

                        Case Else ' Everything else should be Connected 
                            DeviceObject.Connected = False
                            NMatches += 1
                    End Select
                    TL.LogMessage("TestSimulator", "Completed Device: " & Sim.ProgID & " OK")
                Catch ex As Exception
                    LogException("TestSimulator", "Exception: " & ex.ToString)
                Finally
                    Try : Marshal.ReleaseComObject(DeviceObject) : Catch : End Try 'Always try and make sure we are properly tidied up!
                    Try : DeviceObject.Dispose() : Catch : End Try
                    DeviceObject = Nothing
                End Try
            End If
            TL.BlankLine()
        Catch ex1 As Exception
            LogException("TestSimulator", "Overall Exception: " & ex1.ToString)
        End Try
    End Sub

    Private Function DeviceTest(ByVal Device As String, ByVal Test As String) As Object
        Dim RetVal As Object = Nothing, SiderealTime, RetValDouble As Double, StartTime As Date
        Dim DeviceTrackingRates As Object
        Dim FocuserMax, FocuserPosition As Integer, FocuserUpperPortion, FocuserTargetPosition As Integer

        Const PossibleDriveRates As String = "driveSidereal,driveKing,driveLunar,driveSolar"

        Action(Test)
        Try
            StartTime = Now
            Select Case Device
                Case "SafetyMonitor"
                    Select Case Test
                        Case "IsSafe"
                            Compare("DeviceTest", Test, DeviceObject.IsSafe, "False")
                        Case Else
                            LogException("DeviceTest", "Unknown Test: " & Test)
                    End Select

                Case "Switch"
                    Select Case Test
                        Case "MaxSwitch"
                            CompareBoolean("DeviceTest", Test, DeviceObject.MaxSwitch > 0, True)
                        Case "CanWrite"
                            Compare("DeviceTest", Test, IIf(DeviceObject.CanWrite(0), "OK", "OK").ToString(), "OK")
                        Case "GetSwitch"
                            Compare("DeviceTest", Test, IIf(DeviceObject.GetSwitch(0), "OK", "OK").ToString(), "OK")
                        Case "GetSwitchName"
                            CompareBoolean("DeviceTest", Test, String.IsNullOrEmpty(DeviceObject.GetSwitchName(0)), False)
                        Case "GetSwitchDescription"
                            CompareBoolean("DeviceTest", Test, String.IsNullOrEmpty(DeviceObject.GetSwitchDescription(0)), False)
                        Case "GetSwitchValue"
                            CompareBoolean("DeviceTest", Test, IsNumeric(DeviceObject.GetSwitchValue(0)), True)
                        Case "MaxSwitchValue"
                            CompareBoolean("DeviceTest", Test, IsNumeric(DeviceObject.MaxSwitchValue(0)), True)
                        Case "MinSwitchValue"
                            CompareBoolean("DeviceTest", Test, IsNumeric(DeviceObject.MinSwitchValue(0)), True)
                        Case "SwitchStep"
                            CompareBoolean("DeviceTest", Test, IsNumeric(DeviceObject.SwitchStep(0)), True)
                        Case Else
                            LogException("DeviceTest", "Unknown Test: " & Test)
                    End Select
                Case "FilterWheel"
                    Select Case Test
                        Case "Position"
                            DeviceObject.Position = 3
                            Do
                                Thread.Sleep(100)
                                Application.DoEvents()
                                Action(Test & " " & Now.Subtract(StartTime).Seconds)
                            Loop Until DeviceObject.Position > -1
                            CompareDouble("DeviceTest", Test, CDbl(DeviceObject.Position), 3.0, 0.000001)
                        Case Else
                            LogException("DeviceTest", "Unknown Test: " & Test)
                    End Select
                Case "Focuser"
                    Select Case Test
                        Case "Move"
                            ' Find the larger of either 0 to Position or Position to MaxStep and then move to half of that
                            ' Calculate the upper portion size, the lower portion size is given by Position
                            ' 0.................................................Pos..........................Max
                            '                 Lower Portion                               Upper Portion

                            FocuserMax = DeviceObject.MaxStep
                            FocuserPosition = DeviceObject.Position
                            TL.LogMessage("DeviceTest", "Focuser Position: " & FocuserPosition & ", Focuser Maximum: " & FocuserMax)

                            FocuserUpperPortion = FocuserMax - FocuserPosition

                            If FocuserUpperPortion > FocuserPosition Then ' Upper portion is larger
                                FocuserTargetPosition = FocuserPosition + CInt(FocuserUpperPortion / 2)
                                TL.LogMessage("DeviceTest", "Moving upward to: " & FocuserTargetPosition.ToString())
                                DeviceObject.Move(FocuserTargetPosition)
                            Else ' Lower portion is larger
                                FocuserTargetPosition = CInt(FocuserPosition / 2)
                                TL.LogMessage("DeviceTest", "Moving downward to: " & FocuserTargetPosition.ToString())
                                DeviceObject.Move(FocuserTargetPosition)
                            End If

                            Do
                                Thread.Sleep(200)
                                Application.DoEvents()
                                Action(Test & " " & DeviceObject.Position & " / " & FocuserTargetPosition) 'Now.Subtract(StartTime).Seconds)
                            Loop Until Not DeviceObject.IsMoving
                            CompareInteger("DeviceTest", Test, DeviceObject.Position, FocuserTargetPosition)
                        Case Else
                            LogException("DeviceTest", "Unknown Test: " & Test)
                    End Select
                Case "Camera"
                    Select Case Test
                        Case "StartExposure"
                            StartTime = Now
                            DeviceObject.StartExposure(3.0, True)
                            TL.LogMessage(Device, "Start exposure duration: " & Now.Subtract(StartTime).TotalSeconds)
                            Do
                                Thread.Sleep(100)
                                Application.DoEvents()
                                Action(Test & " " & Now.Subtract(StartTime).Seconds & " seconds")
                            Loop Until ((DeviceObject.CameraState = CameraStates.cameraIdle) Or (Now.Subtract(StartTime).TotalSeconds) > 5.0)
                            CompareDouble(Device, "StartExposure", Now.Subtract(StartTime).TotalSeconds, 3.0, 0.2)
                            Compare(Device, "ImageReady", DeviceObject.ImageReady, True)
                        Case Else
                            LogException("DeviceTest", "Unknown Test: " & Test)
                    End Select
                Case "Telescope"
                    Select Case Test
                        Case "UnPark"
                            DeviceObject.UnPark()
                            Try
                                Compare(Device, Test, DeviceObject.AtPark, "False")
                            Catch ex1 As COMException
                                If ex1.ErrorCode = &H80040400 Then
                                    Compare("TestSimulator", "UnPark - Simulator is in Interface V1 mode", "True", "True")
                                End If
                            End Try
                        Case "TrackingTrue"
                            DeviceObject.UnPark()
                            DeviceObject.Tracking = True
                            Compare(Device, Test, DeviceObject.Tracking, "True")
                        Case "SiderealTime"
                            SiderealTime = DeviceObject.SiderealTime
                            TL.LogMessage(Device, "Received Sidereal time from telescope: " & SiderealTime)
                            RetValDouble = DeviceObject.SiderealTime
                            CompareDouble(Device, Test, RetValDouble, SiderealTime, 0.000001)
                        Case "TargetDeclination"
                            DeviceObject.TargetDeclination = 0.0
                            RetValDouble = DeviceObject.TargetDeclination
                            CompareDouble(Device, Test, RetValDouble, 0.0, TOLERANCE_E4)
                        Case "TargetRightAscension"
                            SiderealTime = DeviceObject.SiderealTime
                            TL.LogMessage(Device, "Received Sidereal time from telescope: " & SiderealTime)
                            DeviceObject.TargetRightAscension = SiderealTime
                            TL.LogMessage(Device, "Target RA set to: " & DeviceObject.TargetRightAscension)
                            RetValDouble = DeviceObject.TargetRightAscension
                            CompareDouble(Device, Test, RetValDouble, SiderealTime, TOLERANCE_E4)
                        Case "Slew"
                            DeviceObject.UnPark()
                            DeviceObject.Tracking = True
                            SiderealTime = DeviceObject.SiderealTime
                            TL.LogMessage(Device, "Received Sidereal time from telescope: " & SiderealTime)
                            DeviceObject.TargetRightAscension = SiderealTime
                            TL.LogMessage(Device, "Target RA set to: " & DeviceObject.TargetRightAscension)
                            DeviceObject.TargetDeclination = 0.0
                            TL.LogMessage(Device, "Pre slew RA is: " & DeviceObject.RightAscension)
                            DeviceObject.SlewToTarget()
                            TL.LogMessage(Device, "Post slew RA is: " & DeviceObject.RightAscension)
                            CompareDouble(Device, Test, DeviceObject.RightAscension, SiderealTime, TOLERANCE_E4)
                            CompareDouble(Device, Test, DeviceObject.Declination, 0.0, TOLERANCE_E4)
                        Case "RightAscension"
                            SiderealTime = DeviceObject.SiderealTime
                            DeviceObject.TargetRightAscension = SiderealTime
                            RetValDouble = DeviceObject.TargetRightAscension
                            CompareDouble(Device, Test, RetValDouble, SiderealTime, TOLERANCE_E4)
                        Case "TrackingRates"
                            Try
                                DeviceTrackingRates = DeviceObject.TrackingRates
                                For Each TrackingRate As DriveRates In DeviceTrackingRates
                                    If PossibleDriveRates.Contains(TrackingRate.ToString) Then
                                        NMatches += 1
                                        TL.LogMessage(Device, "Matched Tracking Rate = " & TrackingRate.ToString)
                                    Else
                                        LogException(Device, "Found unexpected tracking rate: """ & TrackingRate.ToString & """")
                                    End If
                                Next
                            Catch ex1 As COMException
                                If ex1.ErrorCode = &H80040400 Then
                                    Compare("TestSimulator", "TrackingRates - Simulator is in Interface V1 mode", "True", "True")
                                End If
                            End Try
                        Case "AxisRates"
                            Try
                                RetVal = DeviceObject.AxisRates(TelescopeAxes.axisPrimary)
                            Catch ex1 As COMException
                                If ex1.ErrorCode = &H80040400 Then
                                    Compare("TestSimulator", "AxisRates - Simulator is in Interface V1 mode", "True", "True")
                                End If
                            End Try
                        Case Else
                            LogException("DeviceTest", "Unknown Test: " & Test)
                    End Select
                Case "Dome"
                    Select Case Test
                        Case "OpenShutter"
                            StartTime = Now
                            DeviceObject.OpenShutter()
                            Do While (Not (DeviceObject.ShutterStatus = ShutterState.shutterOpen)) And (Now.Subtract(StartTime).TotalSeconds < DOME_SLEW_TIMEOUT)
                                Thread.Sleep(100)
                                Action(Test & " " & Now.Subtract(StartTime).Seconds & " seconds / " & DOME_SLEW_TIMEOUT)
                                Application.DoEvents()
                            Loop
                            Compare(Device, Test & " Timeout", Now.Subtract(StartTime).TotalSeconds >= DOME_SLEW_TIMEOUT, "False")
                            Compare(Device, Test, CInt(DeviceObject.ShutterStatus), CInt(ShutterState.shutterOpen))
                        Case "ShutterStatus"
                            Compare(Device, Test, CInt(DeviceObject.ShutterStatus), 0)
                        Case "Slewing"
                            Compare(Device, Test, DeviceObject.Slewing.ToString, "False")
                        Case "CloseShutter"
                            StartTime = Now
                            DeviceObject.CloseShutter()
                            Do While (Not (DeviceObject.ShutterStatus = ShutterState.shutterClosed)) And (Now.Subtract(StartTime).TotalSeconds < DOME_SLEW_TIMEOUT)
                                Thread.Sleep(100)
                                Action(Test & " " & Now.Subtract(StartTime).Seconds & " seconds / " & DOME_SLEW_TIMEOUT)
                                Application.DoEvents()
                            Loop
                            Compare(Device, Test & " Timeout", Now.Subtract(StartTime).TotalSeconds >= DOME_SLEW_TIMEOUT, "False")
                            Compare(Device, Test, CInt(DeviceObject.ShutterStatus), CInt(ShutterState.shutterClosed))
                        Case "SlewToAltitude"
                            StartTime = Now
                            DeviceObject.SlewToAltitude(45.0)
                            Do
                                Thread.Sleep(100)
                                Application.DoEvents()
                                Action(Test & " " & Now.Subtract(StartTime).Seconds & " seconds / " & DOME_SLEW_TIMEOUT)
                            Loop Until ((DeviceObject.Slewing = False) Or (Now.Subtract(StartTime).TotalSeconds) > DOME_SLEW_TIMEOUT)
                            Compare(Device, Test & " Not Complete", DeviceObject.Slewing.ToString, "False")
                            CompareDouble(Device, Test, DeviceObject.Altitude, 45.0, 0.00001)
                        Case "SlewToAzimuth"
                            StartTime = Now
                            DeviceObject.SlewToAzimuth(225.0)
                            Do
                                Thread.Sleep(100)
                                Application.DoEvents()
                                Action(Test & " " & Now.Subtract(StartTime).Seconds & " seconds / " & DOME_SLEW_TIMEOUT)
                            Loop Until ((DeviceObject.Slewing = False) Or (Now.Subtract(StartTime).TotalSeconds) > DOME_SLEW_TIMEOUT)
                            Compare(Device, Test & " Not Complete", DeviceObject.Slewing.ToString, "False")
                            CompareDouble(Device, Test, DeviceObject.Azimuth, 225.0, 0.00001)
                        Case Else
                            LogException("DeviceTest", "Unknown Dome Test: " & Test)
                    End Select

                Case "Video"
                    Select Case Test
                        Case "FrameNumber"
                            Compare(Device, Test, (DeviceObject.FrameNumber >= 0).ToString(), "True")
                        Case "BitDepth"
                            Compare(Device, Test, (DeviceObject.BitDepth >= 0).ToString(), "True")
                        Case "CanConfigureDeviceProperties"
                            Compare(Device, Test, DeviceObject.CanConfigureDeviceProperties.ToString(), "True")
                        Case "ExposureMin"
                            Compare(Device, Test, (DeviceObject.BitDepth >= 0.0).ToString(), "True")
                        Case "Height"
                            Compare(Device, Test, (DeviceObject.BitDepth >= 0).ToString(), "True")
                        Case "Width"
                            Compare(Device, Test, (DeviceObject.BitDepth >= 0).ToString(), "True")
                        Case Else
                            LogException("DeviceTest", "Unknown Video Test: " & Test)
                    End Select

                Case "ObservingConditions"
                    Select Case Test
                        Case "AveragePeriod"
                            Compare(Device, Test, (DeviceObject.AveragePeriod >= 0.0).ToString(), "True")
                        Case "TimeSinceLastUpdate"
                            Compare(Device, Test, IsNumeric(DeviceObject.TimeSinceLastUpdate("")).ToString(), "True")
                        Case Else
                            LogException("DeviceTest", "Unknown ObservingConditions Test: " & Test)
                    End Select
                Case Else
                    LogException("DeviceTest", "Unknown Device: " & Device)
            End Select
        Catch ex As Exception
            LogException("DeviceTest", Device & " " & Test & " exception: " & ex.ToString)
        End Try
        Return RetVal
    End Function

    Enum NOVAS3Functions
        PlanetEphemeris
        ReadEph
        SolarSystem
        State
        '=============================================
        CheckoutStarsFull
        '=============================================
        Aberration
        AppPlanet
        AppStar
        AstroPlanet
        AstroStar
        Bary2Obs
        CalDate
        Cel2Ter ' Only from NOVAS 3.1 onwards
        CelPole
        CioArray
        CioBasis
        CioLocation
        CioRa
        DLight
        Ecl2EquVec
        EeCt
        Ephemeris
        Equ2Ecl
        Equ2EclVec
        Equ2Gal
        Equ2Hor
        Era
        ETilt
        FrameTie
        FundArgs
        Gcrs2Equ
        GeoPosVel
        GravDef
        GravVec
        IraEquinox
        JulianDate
        LightTime
        LimbAngle
        LocalPlanet
        LocalStar
        MakeCatEntry
        MakeInSpace
        MakeObject
        MakeObserver
        MakeObserverAtGeocenter
        MakeObserverInSpace
        MakeObserverOnSurface
        MakeOnSurface
        MeanObliq
        MeanStar
        NormAng
        Nutation
        NutationAngles
        Place
        Precession
        ProperMotion
        RaDec2Vector
        RadVel
        Refract
        SiderealTime
        Spin
        StarVectors
        Tdb2Tt
        Ter2Cel
        Terra
        TopoPlanet
        TopoStar
        TransformCat
        TransformHip
        Vector2RaDec
        VirtualPlanet
        VirtualStar
        Wobble
    End Enum

    Sub NOVAS3Tests()
        Status("NOVAS 3 Tests")

        NOVAS3Test(NOVAS3Functions.PlanetEphemeris)
        NOVAS3Test(NOVAS3Functions.ReadEph)
        NOVAS3Test(NOVAS3Functions.SolarSystem)
        NOVAS3Test(NOVAS3Functions.State)

        NOVAS3Test(NOVAS3Functions.Aberration)
        NOVAS3Test(NOVAS3Functions.AppPlanet)
        NOVAS3Test(NOVAS3Functions.AppStar)
        NOVAS3Test(NOVAS3Functions.AstroPlanet)
        NOVAS3Test(NOVAS3Functions.AstroStar)
        NOVAS3Test(NOVAS3Functions.Bary2Obs)
        NOVAS3Test(NOVAS3Functions.CalDate)
        NOVAS3Test(NOVAS3Functions.CelPole)
        NOVAS3Test(NOVAS3Functions.CioArray)
        NOVAS3Test(NOVAS3Functions.CioBasis)
        NOVAS3Test(NOVAS3Functions.CioLocation)
        NOVAS3Test(NOVAS3Functions.CioRa)
        NOVAS3Test(NOVAS3Functions.DLight)
        NOVAS3Test(NOVAS3Functions.Ecl2EquVec)
        NOVAS3Test(NOVAS3Functions.EeCt)
        NOVAS3Test(NOVAS3Functions.Ephemeris)
        NOVAS3Test(NOVAS3Functions.Equ2Ecl)
        NOVAS3Test(NOVAS3Functions.Equ2EclVec)
        NOVAS3Test(NOVAS3Functions.Equ2Gal)
        NOVAS3Test(NOVAS3Functions.Equ2Hor)
        NOVAS3Test(NOVAS3Functions.Era)
        NOVAS3Test(NOVAS3Functions.ETilt)
        NOVAS3Test(NOVAS3Functions.FrameTie)
        NOVAS3Test(NOVAS3Functions.FundArgs)
        NOVAS3Test(NOVAS3Functions.Gcrs2Equ)
        NOVAS3Test(NOVAS3Functions.GeoPosVel)
        NOVAS3Test(NOVAS3Functions.GravDef)
        NOVAS3Test(NOVAS3Functions.GravVec)
        NOVAS3Test(NOVAS3Functions.IraEquinox)
        NOVAS3Test(NOVAS3Functions.JulianDate)
        NOVAS3Test(NOVAS3Functions.LightTime)
        NOVAS3Test(NOVAS3Functions.LimbAngle)
        NOVAS3Test(NOVAS3Functions.LocalPlanet)
        NOVAS3Test(NOVAS3Functions.LocalStar)
        NOVAS3Test(NOVAS3Functions.MakeCatEntry)
        NOVAS3Test(NOVAS3Functions.MakeInSpace)
        NOVAS3Test(NOVAS3Functions.MakeObject)
        NOVAS3Test(NOVAS3Functions.MakeObserver)
        NOVAS3Test(NOVAS3Functions.MakeObserverAtGeocenter)
        NOVAS3Test(NOVAS3Functions.MakeObserverInSpace)
        NOVAS3Test(NOVAS3Functions.MakeObserverOnSurface)
        NOVAS3Test(NOVAS3Functions.MakeOnSurface)
        NOVAS3Test(NOVAS3Functions.MeanObliq)
        NOVAS3Test(NOVAS3Functions.MeanStar)
        NOVAS3Test(NOVAS3Functions.NormAng)
        NOVAS3Test(NOVAS3Functions.Nutation)
        NOVAS3Test(NOVAS3Functions.NutationAngles)
        NOVAS3Test(NOVAS3Functions.Place)
        NOVAS3Test(NOVAS3Functions.Precession)
        NOVAS3Test(NOVAS3Functions.ProperMotion)
        NOVAS3Test(NOVAS3Functions.RaDec2Vector)
        NOVAS3Test(NOVAS3Functions.RadVel)
        NOVAS3Test(NOVAS3Functions.Refract)
        NOVAS3Test(NOVAS3Functions.SiderealTime)
        NOVAS3Test(NOVAS3Functions.Spin)
        NOVAS3Test(NOVAS3Functions.StarVectors)
        NOVAS3Test(NOVAS3Functions.Tdb2Tt)
        NOVAS3Test(NOVAS3Functions.Ter2Cel)
        NOVAS3Test(NOVAS3Functions.Terra)
        NOVAS3Test(NOVAS3Functions.TopoPlanet)
        NOVAS3Test(NOVAS3Functions.TopoStar)
        NOVAS3Test(NOVAS3Functions.TransformCat)
        NOVAS3Test(NOVAS3Functions.TransformHip)
        NOVAS3Test(NOVAS3Functions.Vector2RaDec)
        NOVAS3Test(NOVAS3Functions.VirtualPlanet)
        NOVAS3Test(NOVAS3Functions.VirtualStar)
        NOVAS3Test(NOVAS3Functions.Wobble)
        TL.BlankLine()

        CheckoutStarsFull()

        TL.BlankLine()

    End Sub

    Sub CheckoutStarsFull()
        'Port of the NOVAS 3 ChecoutStarsFull.c program to confirm correct iplementation

        Const N_STARS As Integer = 3
        Const N_TIMES As Integer = 4


        '/*
        'Main function to check out many parts of NOVAS-C by calling
        'function 'topo_star' with version 1 of function 'solarsystem'.

        'For use with NOVAS-C Version 3.
        '*/

        Dim [error] As Short = 0
        Dim accuracy As Short = 0
        Dim i, j, rc As Short

        '/*
        ''deltat' is the difference in time scales, TT - UT1.

        'The(Array) 'tjd' contains four selected Julian dates at which the
        'star positions will be evaluated.
        '*/

        Dim deltat As Double = 60.0
        Dim tjd() As Double = {2450203.5, 2450203.5, 2450417.5, 2450300.5}
        Dim ra, dec As Double

        '/*
        'Hipparcos (ICRS) catalog data for three selected stars.
        '*/
        Dim stars(N_STARS - 1) As ASCOM.Astrometry.CatEntry3
        Nov3.MakeCatEntry("POLARIS", "HIP", 0, 2.530301028, 89.264109444, 44.22, -11.75, 7.56, -17.4, stars(0))
        Nov3.MakeCatEntry("Delta ORI", "HIP", 1, 5.533444639, -0.299091944, 1.67, 0.56, 3.56, 16.0, stars(1))
        Nov3.MakeCatEntry("Theta CAR", "HIP", 2, 10.715944806, -64.39445, -18.87, 12.06, 7.43, 24.0, stars(2))

        '/*
        'The(Observer) 's terrestrial coordinates (latitude, longitude, height).
        '*/
        Dim geo_loc As New OnSurface
        geo_loc.Latitude = 45.0
        geo_loc.Longitude = -75.0
        geo_loc.Height = 0.0
        geo_loc.Temperature = 10.0
        geo_loc.Pressure = 1010.0

        '/*
        'Compute the topocentric places of the three stars at the four
        'selected Julian dates.
        '*/

        Dim ExpectedResults(N_TIMES - 1, N_STARS - 1) As String
        ExpectedResults(0, 0) = "2450203" & DecimalSeparator & "5 POLARIS RA = 2" & DecimalSeparator & "446988922 Dec = 89" & DecimalSeparator & "24635149"
        ExpectedResults(0, 1) = "2450203" & DecimalSeparator & "5 Delta ORI RA = 5" & DecimalSeparator & "530110723 Dec = -0" & DecimalSeparator & "30571737"
        ExpectedResults(0, 2) = "2450203" & DecimalSeparator & "5 Theta CAR RA = 10" & DecimalSeparator & "714525513 Dec = -64" & DecimalSeparator & "38130590"
        ExpectedResults(1, 0) = "2450203" & DecimalSeparator & "5 POLARIS RA = 2" & DecimalSeparator & "446988922 Dec = 89" & DecimalSeparator & "24635149"
        ExpectedResults(1, 1) = "2450203" & DecimalSeparator & "5 Delta ORI RA = 5" & DecimalSeparator & "530110723 Dec = -0" & DecimalSeparator & "30571737"
        ExpectedResults(1, 2) = "2450203" & DecimalSeparator & "5 Theta CAR RA = 10" & DecimalSeparator & "714525513 Dec = -64" & DecimalSeparator & "38130590"
        ExpectedResults(2, 0) = "2450417" & DecimalSeparator & "5 POLARIS RA = 2" & DecimalSeparator & "509480139 Dec = 89" & DecimalSeparator & "25196813"
        ExpectedResults(2, 1) = "2450417" & DecimalSeparator & "5 Delta ORI RA = 5" & DecimalSeparator & "531195904 Dec = -0" & DecimalSeparator & "30301961"
        ExpectedResults(2, 2) = "2450417" & DecimalSeparator & "5 Theta CAR RA = 10" & DecimalSeparator & "714444761 Dec = -64" & DecimalSeparator & "37366514"
        ExpectedResults(3, 0) = "2450300" & DecimalSeparator & "5 POLARIS RA = 2" & DecimalSeparator & "481177532 Dec = 89" & DecimalSeparator & "24254404"
        ExpectedResults(3, 1) = "2450300" & DecimalSeparator & "5 Delta ORI RA = 5" & DecimalSeparator & "530372288 Dec = -0" & DecimalSeparator & "30231606"
        ExpectedResults(3, 2) = "2450300" & DecimalSeparator & "5 Theta CAR RA = 10" & DecimalSeparator & "713575394 Dec = -64" & DecimalSeparator & "37966995"

        For i = 0 To N_TIMES - 1

            For j = 0 To N_STARS - 1
                rc = Nov3.TopoStar(tjd(i), deltat, stars(j), geo_loc, accuracy, ra, dec)
                If rc <> 0 Then
                    LogRC(NOVAS3Functions.CheckoutStarsFull, "Main loop", rc, "Error " & rc & " from topo_star", "")
                Else

                    LogRC(NOVAS3Functions.CheckoutStarsFull, "Main loop", rc, tjd(i) & " " & stars(j).StarName & " RA = " & Format(ra, "0.000000000") & " Dec = " & Format(dec, "0.00000000"), ExpectedResults(i, j))
                End If
            Next
        Next


        'Ephem_Close ();
        'return (0);



    End Sub

    Sub NOVAS3Test(ByVal TestFunction As NOVAS3Functions)
        Dim rc As Integer, CatEnt As New CatEntry3, ObjectJupiter As New Object3, Observer As New Observer, skypos, SkyPos1 As New SkyPos
        Dim OnSurf As New OnSurface
        Dim RA, Dec, Dis, JD, GST, JDTest As Double
        Dim BodyJupiter, BodyEarth As New BodyDescription
        Dim Si As New SiteInfo, Pos(2), Pos1(2), Pos2(2), Vel(2), PosObj(2), PosObs(2), PosBody(2), VelObs(2) As Double
        Dim Utl As New Util

        Const DeltaT As Double = 66.8

        Action(TestFunction.ToString)

        rc = Integer.MaxValue 'Initialise to a silly value

        JDTest = TestJulianDate()

        Pos1(0) = 1
        Pos1(1) = 1
        Pos1(2) = 1
        PosObs(0) = 0.0001
        PosObs(1) = 0.0001
        PosObs(2) = 0.0001
        Pos2(0) = 0.0001
        Pos2(1) = 0.0001
        Pos2(2) = 0.0001
        Pos(0) = 100.0
        Pos(1) = 100.0
        Pos(2) = 100.0
        PosBody(0) = 0.01
        PosBody(1) = 0.01
        PosBody(2) = 0.01
        Vel(0) = 1000
        Vel(1) = 1000
        Vel(2) = 1000
        VelObs(0) = 500
        VelObs(1) = 500
        VelObs(2) = 500

        Si.Height = 80
        Si.Latitude = 51
        Si.Longitude = 0
        Si.Pressure = 1010
        Si.Temperature = 25

        BodyJupiter.Name = "Jupiter"
        BodyJupiter.Number = Body.Jupiter
        BodyJupiter.Type = BodyType.MajorPlanet

        BodyEarth.Name = "Earth"
        BodyEarth.Number = Body.Earth
        BodyEarth.Type = BodyType.MajorPlanet

        CatEnt.Catalog = "GMB"
        CatEnt.Dec = 23.23
        CatEnt.Parallax = 10.0
        CatEnt.ProMoDec = 20.0
        CatEnt.ProMoRA = 30.0
        CatEnt.RA = 39.39
        CatEnt.RadialVelocity = 40.0
        CatEnt.StarName = "GMB 1830"
        CatEnt.StarNumber = 1830

        ObjectJupiter.Name = "Jupiter"
        ObjectJupiter.Number = Body.Jupiter
        ObjectJupiter.Star = New CatEntry3
        ObjectJupiter.Type = ObjectType.MajorPlanetSunOrMoon

        Observer.OnSurf.Height = 80
        Observer.OnSurf.Latitude = 51.0
        Observer.OnSurf.Longitude = 0.0
        Observer.OnSurf.Pressure = 1010
        Observer.OnSurf.Temperature = 20
        Observer.Where = ObserverLocation.EarthSurface

        OnSurf.Height = 80
        OnSurf.Latitude = 51.0
        OnSurf.Longitude = 0.0
        OnSurf.Pressure = 1000
        OnSurf.Temperature = 5

        Try
            Select Case TestFunction
                Case NOVAS3Functions.PlanetEphemeris
                    Dim JDArr(1) As Double
                    JDArr(0) = JDTest
                    JDArr(1) = 0
                    rc = Nov3.PlanetEphemeris(JDArr, Target.Jupiter, Target.Earth, Pos, Vel)
                    LogRC(TestFunction, "Ju Ea", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                    rc = Nov3.PlanetEphemeris(JDArr, Target.Earth, Target.Jupiter, Pos, Vel)
                    LogRC(TestFunction, "Ea Ju", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                    rc = Nov3.PlanetEphemeris(JDArr, Target.Jupiter, Target.Mercury, Pos, Vel)
                    LogRC(TestFunction, "Ju Me", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                    rc = Nov3.PlanetEphemeris(JDArr, Target.Moon, Target.Earth, Pos, Vel)
                    LogRC(TestFunction, "Mo Ea", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                    rc = Nov3.PlanetEphemeris(JDArr, Target.SolarSystemBarycentre, Target.Moon, Pos, Vel)
                    LogRC(TestFunction, "SS Mo", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                Case NOVAS3Functions.SolarSystem
                    rc = Nov3.SolarSystem(JDTest, Body.Neptune, Origin.Barycentric, Pos, Vel)
                    LogRC(TestFunction, "Neptune", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                Case NOVAS3Functions.State
                    Dim JDArr(1) As Double
                    JDArr(0) = JDTest
                    JDArr(1) = 0
                    rc = Nov3.State(JDArr, Target.Pluto, Pos, Vel)
                    LogRC(TestFunction, "Pluto", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                Case NOVAS3Functions.Aberration
                    rc = 0
                    Nov3.RaDec2Vector(20.0, 40.0, 100, Pos)
                    LogRC(TestFunction, "X, Y, Z", rc, Pos(0) & " " & Pos(1) & " " & Pos(2), "")

                    Nov3.Aberration(Pos, Pos, 10.0, Pos2)
                    LogRC(TestFunction, "X, Y, Z", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.AppPlanet
                    rc = Nov3.AppPlanet(JDTest, ObjectJupiter, Accuracy.Full, RA, Dec, Dis)
                    LogRC(TestFunction, "Jupiter", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3) & " " & Dis, "")
                    ObjectJupiter.Number = Body.Moon
                    ObjectJupiter.Name = "Moon"
                    ObjectJupiter.Type = ObjectType.MajorPlanetSunOrMoon
                    rc = Nov3.AppPlanet(JDTest, ObjectJupiter, Accuracy.Full, RA, Dec, Dis)
                    LogRC(TestFunction, "Moon", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3) & " " & Dis, "")
                Case NOVAS3Functions.AppStar
                    rc = Nov3.AppStar(JDTest, CatEnt, Accuracy.Full, RA, Dec)
                    LogRC(TestFunction, "RA Dec", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3), "")
                Case NOVAS3Functions.AstroPlanet
                    rc = Nov3.AstroPlanet(JDTest, ObjectJupiter, Accuracy.Full, RA, Dec, Dis)
                    LogRC(TestFunction, "Jupiter", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3) & " " & Dis, "")
                Case NOVAS3Functions.AstroStar
                    CatEnt.Catalog = "FK6"
                    CatEnt.Dec = 45.45
                    CatEnt.Parallax = 0.0
                    CatEnt.ProMoDec = 0.0
                    CatEnt.ProMoRA = 0.0
                    CatEnt.RA = 15.15
                    CatEnt.RadialVelocity = 0.0
                    CatEnt.StarName = "GMB 1830"
                    CatEnt.StarNumber = 1307

                    rc = Nov3.AstroStar(Utl.JulianDate, CatEnt, Accuracy.Reduced, RA, Dec)
                    LogRC(TestFunction, "Reduced accuracy:", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3), "")
                    rc = Nov3.AstroStar(Utl.JulianDate, CatEnt, Accuracy.Full, RA, Dec)
                    LogRC(TestFunction, "Full accuracy:   ", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3), "")
                Case NOVAS3Functions.Bary2Obs
                    Dim LightTime As Double
                    rc = 0
                    Nov3.RaDec2Vector(20.0, 40.0, 100, Pos)

                    Nov3.Bary2Obs(Pos, PosObs, Pos2, LightTime)
                    LogRC(TestFunction, "X, Y, Z, LightTime", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2) & " " & LightTime, "")
                Case NOVAS3Functions.CalDate
                    Dim Year, Month, Day As Short, Hour As Double
                    rc = 0
                    Nov3.CalDate(JDTest, Year, Month, Day, Hour)
                    LogRC(TestFunction, "Year Month Day Hour", rc, Year & " " & Month & " " & Day & " " & Format(Hour, "0.0"), "2010 12 30 9" & DecimalSeparator & "0")
                Case NOVAS3Functions.CelPole
                    Dim DPole1, DPole2 As Double
                    rc = Nov3.CelPole(JDTest, PoleOffsetCorrection.ReferredToMeanEclipticOfDate, DPole1, DPole2)
                    LogRC(TestFunction, "Mean Ecliptic Of Date", rc, DPole1 & " " & DPole2, "")
                    rc = Nov3.CelPole(JDTest, PoleOffsetCorrection.ReferredToGCRSAxes, DPole1, DPole2)
                    LogRC(TestFunction, "GCRS Axes", rc, DPole1 & " " & DPole2, "")
                Case NOVAS3Functions.CioArray
                    Dim Cio As New ArrayList
                    rc = Nov3.CioArray(JDTest, 20, Cio)
                    LogRC(TestFunction, "RC", rc, rc, "")
                    rc = 0
                    For Each CioA As ASCOM.Astrometry.RAOfCio In Cio
                        LogRC(TestFunction, "CIO Array", rc, CioA.JdTdb & " " & CioA.RACio, "")
                    Next
                Case NOVAS3Functions.CioBasis
                    Dim x, y, z As Double
                    rc = Nov3.CioBasis(JDTest, 20.0, ReferenceSystem.GCRS, Accuracy.Full, x, y, z)
                    LogRC(TestFunction, "CIO Basis", rc, x & " " & y & " " & z, "")
                Case NOVAS3Functions.CioLocation
                    Dim RAofCIO As Double, RefSys As ASCOM.Astrometry.ReferenceSystem
                    rc = Nov3.CioLocation(JDTest, Accuracy.Full, RAofCIO, RefSys)
                    LogRC(TestFunction, "CIO Location", rc, RAofCIO & " " & RefSys.ToString, "")
                Case NOVAS3Functions.CioRa
                    rc = Nov3.CioRa(JDTest, Accuracy.Full, RA)
                    LogRC(TestFunction, "CIO RA", rc, RA, "")
                Case NOVAS3Functions.DLight
                    Dim DLight As Double
                    rc = 0
                    DLight = Nov3.DLight(Pos1, PosObs)
                    LogRC(TestFunction, "D Light", rc, DLight, "")
                Case NOVAS3Functions.Ecl2EquVec
                    rc = Nov3.Ecl2EquVec(JDTest, CoordSys.CIOOfDate, Accuracy.Full, Pos1, Pos2)
                    LogRC(TestFunction, "X, Y, Z", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.EeCt
                    rc = 0
                    JD = Nov3.EeCt(JDTest, 0.0, Accuracy.Full)
                    LogRC(TestFunction, "J Date", rc, JD, "")
                Case NOVAS3Functions.Ephemeris
                    Dim JD1(1) As Double
                    JD1(0) = JDTest
                    rc = Nov3.Ephemeris(JD1, ObjectJupiter, Origin.Barycentric, Accuracy.Full, Pos, Vel)
                    LogRC(TestFunction, "X, Y, Z", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                Case NOVAS3Functions.Equ2Ecl
                    Dim ELon, ELat As Double
                    RA = 16.0
                    Dec = 40.0
                    rc = Nov3.Equ2Ecl(JDTest, CoordSys.EquinoxOfDate, Accuracy.Full, RA, Dec, ELon, ELat)
                    LogRC(TestFunction, "E Lon E Lat", rc, ELon & " " & ELat, "")
                Case NOVAS3Functions.Equ2EclVec
                    rc = Nov3.Equ2EclVec(JDTest, CoordSys.EquinoxOfDate, Accuracy.Full, Pos1, Pos2)
                    LogRC(TestFunction, "X, Y, Z", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.Equ2Gal
                    Dim GLat, GLong As Double
                    rc = 0
                    Nov3.Equ2Gal(12.456, 40.0, GLong, GLat)
                    LogRC(TestFunction, "G Long, G Lat", rc, GLong & " " & GLat, "")
                Case NOVAS3Functions.Equ2Hor
                    Dim ZD, Az, RaR, DecR As Double
                    rc = 0
                    Nov3.Equ2Hor(JDTest, 0.0, Accuracy.Full, 30.0, 50.0, OnSurf, RA, Dec, RefractionOption.LocationRefraction, ZD, Az, RaR, DecR)
                    LogRC(TestFunction, "ZD Az RaR DecR", rc, ZD & " " & Az & " " & RaR & " " & DecR, "")
                Case NOVAS3Functions.Era
                    Dim Era As Double
                    rc = 0
                    Era = Nov3.Era(JDTest, 0.0)
                    LogRC(TestFunction, "Era", rc, Era, "")
                Case NOVAS3Functions.ETilt
                    Dim Mobl, Tobl, Ee, DEps, DPsi As Double
                    rc = 0
                    Nov3.ETilt(JDTest, Accuracy.Full, Mobl, Tobl, Ee, DPsi, DEps)
                    LogRC(TestFunction, "Mobl, Tobl, Ee, DPsi, DEps", rc, Format(Mobl, "0.00000000") & " " & Format(Tobl, "0.00000000") & " " & Format(Ee, "0.00000000") & " " & Format(DPsi, "0.00000000") & " " & Format(DEps, "0.00000000"), "")
                Case NOVAS3Functions.FrameTie
                    rc = 0
                    Nov3.FrameTie(Pos1, FrameConversionDirection.DynamicalToICRS, Pos2)
                    LogRC(TestFunction, "X, Y, Z", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.FundArgs
                    Dim A(4) As Double
                    rc = 0
                    Nov3.FundArgs(JDTest, A)
                    LogRC(TestFunction, "A", rc, Format(A(0), "0.00000000") & " " & Format(A(1), "0.00000000") & " " & Format(A(2), "0.00000000") & " " & Format(A(3), "0.00000000") & " " & Format(A(4), "0.00000000"), "")
                Case NOVAS3Functions.Gcrs2Equ
                    Dim RaG, DecG As Double
                    RaG = 11.5
                    DecG = 40.0
                    rc = Nov3.Gcrs2Equ(JDTest, CoordSys.EquinoxOfDate, Accuracy.Full, RaG, DecG, RA, Dec)
                    LogRC(TestFunction, "RaG 11.5, DecG 40.0", rc, RA & " " & Dec, "")
                Case NOVAS3Functions.GeoPosVel
                    rc = Nov3.GeoPosVel(JDTest, 0.0, Accuracy.Full, Observer, Pos, Vel)
                    LogRC(TestFunction, "Pos, Vel", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                Case NOVAS3Functions.GravDef
                    rc = Nov3.GravDef(JDTest, EarthDeflection.AddEarthDeflection, Accuracy.Full, Pos1, PosObs, Pos2)
                    LogRC(TestFunction, "X, Y, Z", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.GravVec
                    Dim RMass As Double
                    rc = 0
                    Nov3.GravVec(Pos1, PosObs, PosBody, RMass, Pos2)
                    LogRC(TestFunction, "X, Y, Z", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.IraEquinox
                    Dim Ira As Double
                    rc = 0
                    Ira = Nov3.IraEquinox(JDTest, EquinoxType.MeanEquinox, Accuracy.Full)
                    LogRC(TestFunction, "Ira", rc, Ira, "")
                Case NOVAS3Functions.JulianDate
                    JD = Nov3.JulianDate(2000, 1, 1, 12)
                    LogRC(TestFunction, "J2000: ", 0, JD, NOVAS.NOVAS2.JulianDate(2000, 1, 1, 12.0))
                    JD = Nov3.JulianDate(2010, 1, 2, 0.0)
                    LogRC(TestFunction, "J2010: ", 0, JD, NOVAS.NOVAS2.JulianDate(2010, 1, 2, 0.0))
                Case NOVAS3Functions.LightTime
                    Dim TLight0, TLight As Double
                    TLight0 = 0.0
                    rc = Nov3.LightTime(JDTest, ObjectJupiter, PosObs, TLight0, Accuracy.Full, Pos, TLight)
                    LogRC(TestFunction, "X, Y, Z", rc, Pos(0) & " " & Pos(1) & " " & Pos(2) & " " & TLight, "")
                Case NOVAS3Functions.LimbAngle
                    Dim LimbAng, NadirAngle As Double
                    rc = 0
                    Nov3.LimbAngle(PosObj, PosObs, LimbAng, NadirAngle)
                    LogRC(TestFunction, "LimbAng, NadirAngle", rc, LimbAng & " " & NadirAngle, "")
                Case NOVAS3Functions.LocalPlanet
                    rc = Nov3.LocalPlanet(JDTest, ObjectJupiter, 0.0, OnSurf, Accuracy.Full, RA, Dec, Dis)
                    LogRC(TestFunction, "Ra, Dec, Dis", rc, RA & " " & Dec & " " & Dis, "")
                Case NOVAS3Functions.LocalStar
                    rc = Nov3.LocalStar(JDTest, 0.0, CatEnt, OnSurf, Accuracy.Full, RA, Dec)
                    LogRC(TestFunction, "Ra, Dec", rc, RA & " " & Dec, "")
                Case NOVAS3Functions.MakeCatEntry
                    Nov3.MakeCatEntry("A Star", "FK6", 1234545, 11.0, 12, 0, 13.0, 14.0, 15.0, CatEnt)
                    rc = 0
                    LogRC(TestFunction, "CatEnt", rc, CatEnt.Catalog & " " & CatEnt.Dec & " " & CatEnt.Parallax & " " & CatEnt.StarName & " " & CatEnt.StarNumber, "")
                Case NOVAS3Functions.MakeInSpace
                    Dim Insp As New InSpace
                    Dim PosOrg(2), VelOrg(2) As Double
                    PosOrg(0) = 1
                    PosOrg(1) = 2
                    PosOrg(2) = 3
                    VelOrg(0) = 4
                    VelOrg(1) = 5
                    VelOrg(2) = 6
                    Insp.ScPos = Pos
                    Insp.ScVel = Vel
                    rc = 0
                    Nov3.MakeInSpace(PosOrg, VelOrg, Insp)
                    LogRC(TestFunction, "Pos, Vel", rc, Insp.ScPos(0) & Insp.ScPos(1) & Insp.ScPos(2) & Insp.ScVel(0) & Insp.ScVel(1) & Insp.ScVel(2), "123456")
                Case NOVAS3Functions.MakeObject
                    rc = Nov3.MakeObject(ObjectType.MajorPlanetSunOrMoon, 7, "Uranus", CatEnt, ObjectJupiter)
                    LogRC(TestFunction, "Object3", rc, ObjectJupiter.Name & " " & ObjectJupiter.Number & " " & ObjectJupiter.Type.ToString & " " & ObjectJupiter.Star.RA, "")
                Case NOVAS3Functions.MakeObserver
                    Dim Obs As New Observer
                    OnSurf.Latitude = 51.0
                    Nov3.MakeObserver(ObserverLocation.EarthSurface, OnSurf, New InSpace, Obs)
                    rc = 0
                    LogRC(TestFunction, "Observer", rc, Obs.Where.ToString & " " & Obs.OnSurf.Latitude, "")
                Case NOVAS3Functions.MakeObserverAtGeocenter
                    Dim Obs As New Observer
                    Nov3.MakeObserverAtGeocenter(Obs)
                    rc = 0
                    LogRC(TestFunction, "Observer", rc, Obs.Where.ToString & " " & Obs.OnSurf.Latitude, "")
                Case NOVAS3Functions.MakeObserverInSpace
                    Dim Obs As New Observer
                    Nov3.MakeObserverInSpace(Pos, Vel, Obs)
                    rc = 0
                    LogRC(TestFunction, "Observer", rc, Obs.Where.ToString & " " & Obs.OnSurf.Latitude, "")
                Case NOVAS3Functions.MakeObserverOnSurface
                    Dim Obs As New Observer
                    Nov3.MakeObserverOnSurface(51.0, 0.0, 80.0, 25.0, 1010, Obs)
                    rc = 0
                    LogRC(TestFunction, "Observer", rc, Obs.Where.ToString & " " & Obs.OnSurf.Latitude, "")
                Case NOVAS3Functions.MakeOnSurface
                    Nov3.MakeOnSurface(51.0, 0.0, 80.0, 25, 0, OnSurf)
                    rc = 0
                    LogRC(TestFunction, "OnSurface", rc, OnSurf.Latitude & " " & OnSurf.Height, "")
                Case NOVAS3Functions.MeanObliq
                    Dim MO As Double
                    MO = Nov3.MeanObliq(JDTest)
                    rc = 0
                    LogRC(TestFunction, "Mean Obl", rc, MO, "")
                Case NOVAS3Functions.MeanStar
                    Dim IRa, IDec As Double
                    rc = Nov3.MeanStar(JDTest, RA, Dec, Accuracy.Full, IRa, IDec)
                    LogRC(TestFunction, "IRa, IDec", rc, IRa & " " & IDec, "")
                Case NOVAS3Functions.NormAng
                    Dim NA As Double
                    NA = Nov3.NormAng(4 * 3.142)
                    rc = 0
                    LogRC(TestFunction, "Norm Ang", rc, NA, "")
                Case NOVAS3Functions.Nutation
                    Dim Obs As New Observer
                    rc = 0
                    Nov3.Nutation(JDTest, NutationDirection.MeanToTrue, Accuracy.Full, Pos, Pos2)
                    LogRC(TestFunction, "Pos, Pos2", rc, Format(Pos(0), "0.00000000") & " " & Format(Pos(1), "0.00000000") & " " & Format(Pos(2), "0.00000000") & " " & Format(Pos2(0), "0.00000000") & " " & Format(Pos2(1), "0.00000000") & " " & Format(Pos2(2), "0.00000000"), "")
                Case NOVAS3Functions.NutationAngles
                    Dim DPsi, DEps As Double
                    rc = 0
                    Nov3.NutationAngles(JDTest, Accuracy.Full, DPsi, DEps)
                    LogRC(TestFunction, "DPsi, DEps", rc, DPsi & " " & DEps, "")
                Case NOVAS3Functions.Place
                    ObjectJupiter.Name = "Planet"
                    'Obj.Star = CatEnt
                    ObjectJupiter.Type = 0
                    OnSurf.Height = 80
                    OnSurf.Latitude = Utl.DMSToDegrees("51:04:43")
                    OnSurf.Longitude = -Utl.DMSToDegrees("00:17:40")
                    OnSurf.Pressure = 1010
                    OnSurf.Temperature = 10

                    Observer.Where = ObserverLocation.EarthSurface
                    Observer.OnSurf = OnSurf

                    For i As Short = 1 To 11
                        If i <> 3 Then 'Skip earth test as not relevant - viewing earth from earth has no meaning!
                            ObjectJupiter.Number = i
                            JD = Utl.JulianDate
                            rc = Nov3.Place(JDTest, ObjectJupiter, Observer, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, skypos)
                            rc = Nov3.Place(JDTest, ObjectJupiter, Observer, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Reduced, SkyPos1)
                            LogRC(TestFunction, "Planet " & Microsoft.VisualBasic.Right(" " & i.ToString, 2) & "", rc, Utl.HoursToHMS(SkyPos1.RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(SkyPos1.Dec, ":", ":", "", 3), Utl.HoursToHMS(skypos.RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(skypos.Dec, ":", ":", "", 3))
                        End If
                    Next
                Case NOVAS3Functions.Precession
                    rc = Nov3.Precession(JDTest, Pos, J2000, Pos2)
                    LogRC(TestFunction, "Pos2", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.ProperMotion
                    rc = 0
                    Nov3.ProperMotion(JDTest, Pos, Vel, J2000, Pos2)
                    LogRC(TestFunction, "Pos2", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.RaDec2Vector
                    rc = 0
                    Nov3.RaDec2Vector(11.0, 12.0, 13.0, Pos)
                    LogRC(TestFunction, "Pos", rc, Pos(0) & " " & Pos(1) & " " & Pos(2), "")
                Case NOVAS3Functions.RadVel
                    Dim Rv As Double
                    rc = 0
                    Nov3.RadVel(ObjectJupiter, Pos, Vel, VelObs, 12.0, 14.0, 16.0, Rv)
                    LogRC(TestFunction, "Rv", rc, Rv, "")
                Case NOVAS3Functions.ReadEph
                    Dim Err As Integer, Eph(5) As Double
                    rc = 0
                    Eph = Nov3.ReadEph(99, "missingasteroid", JDTest, Err)
                    LogRC(TestFunction, "Expect error 4", rc, Err & Eph(0) & Eph(1) & Eph(2) & Eph(3) & Eph(4) & Eph(5), "4000000")
                    JD = 2453415.5
                    Eph = Nov3.ReadEph(1, "Ceres", JD, Err)
                    LogRC(TestFunction, "JD Before " & JD, rc, Err & " " & Format(Eph(0), "0.00000") & " " & Format(Eph(1), "0.00000") & " " & Format(Eph(2), "0.00000") & " " & Format(Eph(3), "0.00000") & " " & Format(Eph(4), "0.00000") & " " & Format(Eph(5), "0.00000"), "3 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000")
                    JD = 2453425.5
                    Eph = Nov3.ReadEph(1, "Ceres", JD, Err)
                    LogRC(TestFunction, "JD Start  " & JD, rc, Err & " " & Format(Eph(0), "0.00000") & " " & Format(Eph(1), "0.00000") & " " & Format(Eph(2), "0.00000") & " " & Format(Eph(3), "0.00000") & " " & Format(Eph(4), "0.00000") & " " & Format(Eph(5), "0.00000"), "0 -2" & DecimalSeparator & "23084 -1" & DecimalSeparator & "38495 -0" & DecimalSeparator & "19822 0" & DecimalSeparator & "00482 -0" & DecimalSeparator & "00838 -0" & DecimalSeparator & "00493")
                    JD = 2454400.5
                    Eph = Nov3.ReadEph(1, "Ceres", JD, Err)
                    LogRC(TestFunction, "JD Mid    " & JD, rc, Err & " " & Format(Eph(0), "0.00000") & " " & Format(Eph(1), "0.00000") & " " & Format(Eph(2), "0.00000") & " " & Format(Eph(3), "0.00000") & " " & Format(Eph(4), "0.00000") & " " & Format(Eph(5), "0.00000"), "0 2" & DecimalSeparator & "02286 1" & DecimalSeparator & "91181 0" & DecimalSeparator & "48869 -0" & DecimalSeparator & "00736 0" & DecimalSeparator & "00559 0" & DecimalSeparator & "00413")
                    JD = 2455370.5 'Fails (screws up the DLL for subsequent calls) beyond JD = 2455389.5, which is just below the theoretical end of 2455402.5
                    Eph = Nov3.ReadEph(1, "Ceres", JD, Err)
                    LogRC(TestFunction, "JD End    " & JD, rc, Err & " " & Format(Eph(0), "0.00000") & " " & Format(Eph(1), "0.00000") & " " & Format(Eph(2), "0.00000") & " " & Format(Eph(3), "0.00000") & " " & Format(Eph(4), "0.00000") & " " & Format(Eph(5), "0.00000"), "0 -0" & DecimalSeparator & "08799 -2" & DecimalSeparator & "57887 -1" & DecimalSeparator & "19703 0" & DecimalSeparator & "00983 -0" & DecimalSeparator & "00018 -0" & DecimalSeparator & "00209")
                    JD = 2455410.5
                    Eph = Nov3.ReadEph(1, "Ceres", JD, Err)
                    LogRC(TestFunction, "JD After  " & JD, rc, Err & " " & Format(Eph(0), "0.00000") & " " & Format(Eph(1), "0.00000") & " " & Format(Eph(2), "0.00000") & " " & Format(Eph(3), "0.00000") & " " & Format(Eph(4), "0.00000") & " " & Format(Eph(5), "0.00000"), "3 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000")
                Case NOVAS3Functions.Refract
                    Dim Refracted As Double
                    rc = 0
                    Refracted = Nov3.Refract(OnSurf, RefractionOption.NoRefraction, 70.0)
                    LogRC(TestFunction, "No refraction Zd 70.0", rc, Refracted, "")
                    Refracted = Nov3.Refract(OnSurf, RefractionOption.StandardRefraction, 70.0)
                    LogRC(TestFunction, "Standard Zd 70.0     ", rc, Refracted, "")
                    Refracted = Nov3.Refract(OnSurf, RefractionOption.LocationRefraction, 70.0)
                    LogRC(TestFunction, "Location Zd 70.0     ", rc, Refracted, "")
                Case NOVAS3Functions.SiderealTime
                    Dim MObl, TObl, ee, DPSI, DEps, GST2 As Double
                    JD = Utl.JulianDate
                    rc = Nov3.SiderealTime(JD, 0.0, DeltaT, GstType.GreenwichMeanSiderealTime, Method.EquinoxBased, Accuracy.Reduced, GST)
                    LogRC(TestFunction, "Local Mean Equinox    ", rc, Utl.HoursToHMS(GST - (24.0 * Utl.DMSToDegrees("00:17:40") / 360), ":", ":", "", 3), "")
                    rc = Nov3.SiderealTime(JD, 0.0, DeltaT, GstType.GreenwichApparentSiderealTime, Method.EquinoxBased, Accuracy.Full, GST)
                    LogRC(TestFunction, "Local Apparent Equinox", rc, Utl.HoursToHMS(GST - (24.0 * Utl.DMSToDegrees("00:17:40") / 360), ":", ":", "", 3), "")
                    rc = Nov3.SiderealTime(JD, 0.0, DeltaT, GstType.GreenwichMeanSiderealTime, Method.CIOBased, Accuracy.Reduced, GST)
                    LogRC(TestFunction, "Local Mean CIO        ", rc, Utl.HoursToHMS(GST - (24.0 * Utl.DMSToDegrees("00:17:40") / 360), ":", ":", "", 3), "")
                    rc = Nov3.SiderealTime(JD, 0.0, DeltaT, GstType.GreenwichApparentSiderealTime, Method.CIOBased, Accuracy.Reduced, GST)
                    LogRC(TestFunction, "Local Apparent CIO    ", rc, Utl.HoursToHMS(GST - (24.0 * Utl.DMSToDegrees("00:17:40") / 360), ":", ":", "", 3), "")
                    NOVAS.NOVAS2.EarthTilt(JD, MObl, TObl, ee, DPSI, DEps)
                    NOVAS.NOVAS2.SiderealTime(JD, 0.0, ee, GST2)
                    rc = Nov3.SiderealTime(JD, 0.0, DeltaT, GstType.GreenwichApparentSiderealTime, Method.EquinoxBased, Accuracy.Full, GST)
                    LogRCDouble(TestFunction, "Novas3", "GAST Equinox          ", rc, GST, GST2, TOLERANCE_E6)
                Case NOVAS3Functions.Spin
                    rc = 0
                    Nov3.Spin(20.0, Pos1, Pos2)
                    LogRC(TestFunction, "Pos2", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.StarVectors
                    rc = 0
                    Nov3.StarVectors(CatEnt, Pos, Vel)
                    LogRC(TestFunction, "Pos, Vel", rc, Format(Pos(0), "0.000") & " " & Format(Pos(1), "0.000") & " " & Format(Pos(2), "0.000") & " " & Format(Vel(0), "0.00000000") & " " & Format(Vel(1), "0.00000000") & " " & Format(Vel(2), "0.00000000"), "")
                Case NOVAS3Functions.Tdb2Tt
                    Dim TT, Secdiff As Double
                    rc = 0
                    Nov3.Tdb2Tt(JDTest, TT, Secdiff)
                    LogRC(TestFunction, "Pos, Vel", rc, TT & " " & Secdiff, "")
                Case NOVAS3Functions.Ter2Cel
                    rc = Nov3.Ter2Cel(JDTest, 0.0, 0.0, Method.EquinoxBased, Accuracy.Full, OutputVectorOption.ReferredToEquatorAndEquinoxOfDate, 0.0, 0.0, Pos, Pos2)
                    LogRC(TestFunction, "Pos2", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.Terra
                    rc = 0
                    Nov3.Terra(OnSurf, 0.0, Pos, Vel)
                    LogRC(TestFunction, "Pos, Vel", rc, Format(Pos(0), "0.00000000") & " " & Format(Pos(1), "0.00000000") & " " & Format(Pos(2), "0.00000000") & " " & Format(Vel(0), "0.00000000") & " " & Format(Vel(1), "0.00000000") & " " & Format(Vel(2), "0.00000000"), "")
                Case NOVAS3Functions.TopoPlanet
                    rc = Nov3.TopoStar(JDTest, 0.0, CatEnt, OnSurf, Accuracy.Full, RA, Dec)
                    LogRC(TestFunction, "RA, Dec", rc, RA & " " & Dec, "")
                Case NOVAS3Functions.TopoStar
                    CatEnt.Catalog = "HIP"
                    CatEnt.Dec = Utl.DMSToDegrees("16:30:31")
                    CatEnt.Parallax = 0.0
                    CatEnt.ProMoDec = 0.0
                    CatEnt.ProMoRA = 0.0
                    CatEnt.RA = Utl.HMSToHours("04:35:55.2")
                    CatEnt.RadialVelocity = 0.0
                    CatEnt.StarName = "Aldebaran"
                    CatEnt.StarNumber = 21421

                    OnSurf.Height = 80
                    OnSurf.Latitude = 51.0
                    OnSurf.Longitude = 0.0
                    OnSurf.Pressure = 1010
                    OnSurf.Temperature = 10

                    rc = Nov3.TopoStar(Utl.JulianDate, 0.0, CatEnt, OnSurf, Accuracy.Reduced, RA, Dec)
                    LogRC(TestFunction, "Reduced accuracy", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3), "")

                    rc = Nov3.TopoStar(Utl.JulianDate, 0.0, CatEnt, OnSurf, Accuracy.Full, RA, Dec)
                    LogRC(TestFunction, "Full accuracy   ", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3), "")
                Case NOVAS3Functions.TransformCat
                    Dim NewCat As New CatEntry3
                    rc = Nov3.TransformCat(TransformationOption3.ChangeEquatorAndEquinoxAndEpoch, JDTest, CatEnt, J2000, "PWGS", NewCat)
                    LogRC(TestFunction, "NewCat", rc, NewCat.RA & " " & NewCat.Dec & " " & NewCat.Catalog & " " & NewCat.StarName, "")
                Case NOVAS3Functions.TransformHip
                    Dim HipCat As New CatEntry3
                    rc = 0
                    Nov3.TransformHip(CatEnt, HipCat)
                    LogRC(TestFunction, "HipCat", rc, HipCat.RA & " " & HipCat.Dec & " " & HipCat.Catalog & " " & HipCat.StarName, "")
                Case NOVAS3Functions.Vector2RaDec
                    rc = Nov3.Vector2RaDec(Pos, RA, Dec)
                    LogRC(TestFunction, "RA, Dec", rc, RA & " " & Dec, "")
                Case NOVAS3Functions.VirtualPlanet
                    rc = Nov3.VirtualPlanet(JDTest, ObjectJupiter, Accuracy.Full, RA, Dec, Dis)
                    LogRC(TestFunction, "RA, Dec, Dis", rc, RA & " " & Dec & " " & Dis, "")
                Case NOVAS3Functions.VirtualStar
                    rc = Nov3.VirtualStar(JDTest, CatEnt, Accuracy.Full, RA, Dec)
                    LogRC(TestFunction, "RA, Dec", rc, RA & " " & Dec, "")
                Case NOVAS3Functions.Wobble
                    rc = 0
                    Nov3.Wobble(JDTest, 1.0, 1.0, Pos1, Pos2)
                    LogRC(TestFunction, "Pos2", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case Else
                    TL.LogMessage(TestFunction.ToString, "Test not implemented")
            End Select

        Catch ex As Exception
            LogException("Novas3", TestFunction.ToString & " - " & ex.ToString)
        End Try
        Action("")
    End Sub

    Sub NOVAS31Tests()
        Status("NOVAS 3.1 Tests")

        NOVAS31Test(NOVAS3Functions.PlanetEphemeris)
        NOVAS31Test(NOVAS3Functions.ReadEph)
        NOVAS31Test(NOVAS3Functions.SolarSystem)
        NOVAS31Test(NOVAS3Functions.State)

        NOVAS31Test(NOVAS3Functions.Aberration)
        NOVAS31Test(NOVAS3Functions.AppPlanet)
        NOVAS31Test(NOVAS3Functions.AppStar)
        NOVAS31Test(NOVAS3Functions.AstroPlanet)
        NOVAS31Test(NOVAS3Functions.AstroStar)
        NOVAS31Test(NOVAS3Functions.Bary2Obs)
        NOVAS31Test(NOVAS3Functions.CalDate)
        NOVAS31Test(NOVAS3Functions.CelPole)
        NOVAS31Test(NOVAS3Functions.CioArray)
        NOVAS31Test(NOVAS3Functions.CioBasis)
        NOVAS31Test(NOVAS3Functions.CioLocation)
        NOVAS31Test(NOVAS3Functions.CioRa)
        NOVAS31Test(NOVAS3Functions.DLight)
        NOVAS31Test(NOVAS3Functions.Ecl2EquVec)
        NOVAS31Test(NOVAS3Functions.EeCt)
        NOVAS31Test(NOVAS3Functions.Ephemeris)
        NOVAS31Test(NOVAS3Functions.Equ2Ecl)
        NOVAS31Test(NOVAS3Functions.Equ2EclVec)
        NOVAS31Test(NOVAS3Functions.Equ2Gal)
        NOVAS31Test(NOVAS3Functions.Equ2Hor)
        NOVAS31Test(NOVAS3Functions.Era)
        NOVAS31Test(NOVAS3Functions.ETilt)
        NOVAS31Test(NOVAS3Functions.FrameTie)
        NOVAS31Test(NOVAS3Functions.FundArgs)
        NOVAS31Test(NOVAS3Functions.Gcrs2Equ)
        NOVAS31Test(NOVAS3Functions.GeoPosVel)
        NOVAS31Test(NOVAS3Functions.GravDef)
        NOVAS31Test(NOVAS3Functions.GravVec)
        NOVAS31Test(NOVAS3Functions.IraEquinox)
        NOVAS31Test(NOVAS3Functions.JulianDate)
        NOVAS31Test(NOVAS3Functions.LightTime)
        NOVAS31Test(NOVAS3Functions.LimbAngle)
        NOVAS31Test(NOVAS3Functions.LocalPlanet)
        NOVAS31Test(NOVAS3Functions.LocalStar)
        NOVAS31Test(NOVAS3Functions.MakeCatEntry)
        NOVAS31Test(NOVAS3Functions.MakeInSpace)
        NOVAS31Test(NOVAS3Functions.MakeObject)
        NOVAS31Test(NOVAS3Functions.MakeObserver)
        NOVAS31Test(NOVAS3Functions.MakeObserverAtGeocenter)
        NOVAS31Test(NOVAS3Functions.MakeObserverInSpace)
        NOVAS31Test(NOVAS3Functions.MakeObserverOnSurface)
        NOVAS31Test(NOVAS3Functions.MakeOnSurface)
        NOVAS31Test(NOVAS3Functions.MeanObliq)
        NOVAS31Test(NOVAS3Functions.MeanStar)
        NOVAS31Test(NOVAS3Functions.NormAng)
        NOVAS31Test(NOVAS3Functions.Nutation)
        NOVAS31Test(NOVAS3Functions.NutationAngles)
        NOVAS31Test(NOVAS3Functions.Place)
        NOVAS31Test(NOVAS3Functions.Precession)
        NOVAS31Test(NOVAS3Functions.ProperMotion)
        NOVAS31Test(NOVAS3Functions.RaDec2Vector)
        NOVAS31Test(NOVAS3Functions.RadVel)
        NOVAS31Test(NOVAS3Functions.Refract)
        NOVAS31Test(NOVAS3Functions.SiderealTime)
        NOVAS31Test(NOVAS3Functions.Spin)
        NOVAS31Test(NOVAS3Functions.StarVectors)
        NOVAS31Test(NOVAS3Functions.Tdb2Tt)
        NOVAS31Test(NOVAS3Functions.Ter2Cel)
        NOVAS31Test(NOVAS3Functions.Terra)
        NOVAS31Test(NOVAS3Functions.TopoPlanet)
        NOVAS31Test(NOVAS3Functions.TopoStar)
        NOVAS31Test(NOVAS3Functions.TransformCat)
        NOVAS31Test(NOVAS3Functions.TransformHip)
        NOVAS31Test(NOVAS3Functions.Vector2RaDec)
        NOVAS31Test(NOVAS3Functions.VirtualPlanet)
        NOVAS31Test(NOVAS3Functions.VirtualStar)
        NOVAS31Test(NOVAS3Functions.Wobble)
        TL.BlankLine()

        CheckoutStarsFull31()

        TL.BlankLine()

    End Sub

    Sub CheckoutStarsFull31()
        'Port of the NOVAS 3 ChecoutStarsFull.c program to confirm correct iplementation

        Const N_STARS As Integer = 3
        Const N_TIMES As Integer = 4

        '/*
        'Main function to check out many parts of NOVAS-C by calling
        'function 'topo_star' with version 1 of function 'solarsystem'.

        'For use with NOVAS-C Version 3.
        '*/

        Dim [error] As Short = 0
        Dim accuracy As Short = 0
        Dim i, j, rc As Short

        '/*
        ''deltat' is the difference in time scales, TT - UT1.

        'The(Array) 'tjd' contains four selected Julian dates at which the
        'star positions will be evaluated.
        '*/

        Dim deltat As Double = 60.0
        Dim tjd() As Double = {2450203.5, 2450203.5, 2450417.5, 2450300.5}
        Dim ra, dec As Double

        '/*
        'Hipparcos (ICRS) catalog data for three selected stars.
        '*/
        Dim stars(N_STARS - 1) As ASCOM.Astrometry.CatEntry3
        Nov31.MakeCatEntry("POLARIS", "HIP", 0, 2.530301028, 89.264109444, 44.22, -11.75, 7.56, -17.4, stars(0))
        Nov31.MakeCatEntry("Delta ORI", "HIP", 1, 5.533444639, -0.299091944, 1.67, 0.56, 3.56, 16.0, stars(1))
        Nov31.MakeCatEntry("Theta CAR", "HIP", 2, 10.715944806, -64.39445, -18.87, 12.06, 7.43, 24.0, stars(2))

        '/*
        'The(Observer) 's terrestrial coordinates (latitude, longitude, height).
        '*/
        Dim geo_loc As New OnSurface
        geo_loc.Latitude = 45.0
        geo_loc.Longitude = -75.0
        geo_loc.Height = 0.0
        geo_loc.Temperature = 10.0
        geo_loc.Pressure = 1010.0

        '/*
        'Compute the topocentric places of the three stars at the four
        'selected Julian dates.
        '*/

        Dim ExpectedResults(N_TIMES - 1, N_STARS - 1) As String
        ExpectedResults(0, 0) = "2450203" & DecimalSeparator & "5 POLARIS RA = 2" & DecimalSeparator & "446988922 Dec = 89" & DecimalSeparator & "24635149"
        ExpectedResults(0, 1) = "2450203" & DecimalSeparator & "5 Delta ORI RA = 5" & DecimalSeparator & "530110723 Dec = -0" & DecimalSeparator & "30571737"
        ExpectedResults(0, 2) = "2450203" & DecimalSeparator & "5 Theta CAR RA = 10" & DecimalSeparator & "714525513 Dec = -64" & DecimalSeparator & "38130590"
        ExpectedResults(1, 0) = "2450203" & DecimalSeparator & "5 POLARIS RA = 2" & DecimalSeparator & "446988922 Dec = 89" & DecimalSeparator & "24635149"
        ExpectedResults(1, 1) = "2450203" & DecimalSeparator & "5 Delta ORI RA = 5" & DecimalSeparator & "530110723 Dec = -0" & DecimalSeparator & "30571737"
        ExpectedResults(1, 2) = "2450203" & DecimalSeparator & "5 Theta CAR RA = 10" & DecimalSeparator & "714525513 Dec = -64" & DecimalSeparator & "38130590"
        ExpectedResults(2, 0) = "2450417" & DecimalSeparator & "5 POLARIS RA = 2" & DecimalSeparator & "509480139 Dec = 89" & DecimalSeparator & "25196813"
        ExpectedResults(2, 1) = "2450417" & DecimalSeparator & "5 Delta ORI RA = 5" & DecimalSeparator & "531195904 Dec = -0" & DecimalSeparator & "30301961"
        ExpectedResults(2, 2) = "2450417" & DecimalSeparator & "5 Theta CAR RA = 10" & DecimalSeparator & "714444761 Dec = -64" & DecimalSeparator & "37366514"
        ExpectedResults(3, 0) = "2450300" & DecimalSeparator & "5 POLARIS RA = 2" & DecimalSeparator & "481177532 Dec = 89" & DecimalSeparator & "24254404"
        ExpectedResults(3, 1) = "2450300" & DecimalSeparator & "5 Delta ORI RA = 5" & DecimalSeparator & "530372288 Dec = -0" & DecimalSeparator & "30231606"
        ExpectedResults(3, 2) = "2450300" & DecimalSeparator & "5 Theta CAR RA = 10" & DecimalSeparator & "713575394 Dec = -64" & DecimalSeparator & "37966995"

        For i = 0 To N_TIMES - 1

            For j = 0 To N_STARS - 1
                rc = Nov31.TopoStar(tjd(i), deltat, stars(j), geo_loc, accuracy, ra, dec)
                If rc <> 0 Then
                    LogRC31(NOVAS3Functions.CheckoutStarsFull, "Main loop", rc, "Error " & rc & " from topo_star", "")
                Else

                    LogRC31(NOVAS3Functions.CheckoutStarsFull, "Main loop", rc, tjd(i) & " " & stars(j).StarName & " RA = " & Format(ra, "0.000000000") & " Dec = " & Format(dec, "0.00000000"), ExpectedResults(i, j))
                End If
            Next
        Next

    End Sub

    Sub NOVAS31Test(ByVal TestFunction As NOVAS3Functions)
        Dim rc As Integer, CatEnt As New CatEntry3, ObjectJupiter As New Object3, Observer As New Observer, skypos, SkyPos1 As New SkyPos
        Dim OnSurf As New OnSurface
        Dim RA, Dec, Dis, JD, GST, JDTest As Double
        Dim BodyJupiter, BodyEarth As New BodyDescription
        Dim Si As New SiteInfo, Pos(2), Pos1(2), Pos2(2), Vel(2), PosObj(2), PosObs(2), PosBody(2), VelObs(2) As Double
        Dim Utl As New Util

        Const DeltaT As Double = 66.8

        Action(TestFunction.ToString)

        rc = Integer.MaxValue 'Initialise to a silly value

        JDTest = TestJulianDate()

        Pos1(0) = 1
        Pos1(1) = 1
        Pos1(2) = 1
        PosObs(0) = 0.0001
        PosObs(1) = 0.0001
        PosObs(2) = 0.0001
        Pos2(0) = 0.0001
        Pos2(1) = 0.0001
        Pos2(2) = 0.0001
        Pos(0) = 100.0
        Pos(1) = 100.0
        Pos(2) = 100.0
        PosBody(0) = 0.01
        PosBody(1) = 0.01
        PosBody(2) = 0.01
        Vel(0) = 1000
        Vel(1) = 1000
        Vel(2) = 1000
        VelObs(0) = 500
        VelObs(1) = 500
        VelObs(2) = 500

        Si.Height = 80
        Si.Latitude = 51
        Si.Longitude = 0
        Si.Pressure = 1010
        Si.Temperature = 25

        BodyJupiter.Name = "Jupiter"
        BodyJupiter.Number = Body.Jupiter
        BodyJupiter.Type = BodyType.MajorPlanet

        BodyEarth.Name = "Earth"
        BodyEarth.Number = Body.Earth
        BodyEarth.Type = BodyType.MajorPlanet

        CatEnt.Catalog = "GMB"
        CatEnt.Dec = 23.23
        CatEnt.Parallax = 10.0
        CatEnt.ProMoDec = 20.0
        CatEnt.ProMoRA = 30.0
        CatEnt.RA = 39.39
        CatEnt.RadialVelocity = 40.0
        CatEnt.StarName = "GMB 1830"
        CatEnt.StarNumber = 1830

        ObjectJupiter.Name = "Jupiter"
        ObjectJupiter.Number = Body.Jupiter
        ObjectJupiter.Star = New CatEntry3
        ObjectJupiter.Type = ObjectType.MajorPlanetSunOrMoon

        Observer.OnSurf.Height = 80
        Observer.OnSurf.Latitude = 51.0
        Observer.OnSurf.Longitude = 0.0
        Observer.OnSurf.Pressure = 1010
        Observer.OnSurf.Temperature = 20
        Observer.Where = ObserverLocation.EarthSurface

        OnSurf.Height = 80
        OnSurf.Latitude = 51.0
        OnSurf.Longitude = 0.0
        OnSurf.Pressure = 1000
        OnSurf.Temperature = 5

        Try
            Select Case TestFunction
                Case NOVAS3Functions.PlanetEphemeris
                    Dim JDArr(1) As Double
                    JDArr(0) = JDTest
                    JDArr(1) = 0
                    rc = Nov31.PlanetEphemeris(JDArr, Target.Jupiter, Target.Earth, Pos, Vel)
                    LogRC31(TestFunction, "Ju Ea", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                    rc = Nov31.PlanetEphemeris(JDArr, Target.Earth, Target.Jupiter, Pos, Vel)
                    LogRC31(TestFunction, "Ea Ju", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                    rc = Nov31.PlanetEphemeris(JDArr, Target.Jupiter, Target.Mercury, Pos, Vel)
                    LogRC31(TestFunction, "Ju Me", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                    rc = Nov31.PlanetEphemeris(JDArr, Target.Moon, Target.Earth, Pos, Vel)
                    LogRC31(TestFunction, "Mo Ea", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                    rc = Nov31.PlanetEphemeris(JDArr, Target.SolarSystemBarycentre, Target.Moon, Pos, Vel)
                    LogRC31(TestFunction, "SS Mo", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                Case NOVAS3Functions.SolarSystem
                    rc = Nov31.SolarSystem(JDTest, Body.Neptune, Origin.Barycentric, Pos, Vel)
                    LogRC31(TestFunction, "Neptune", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                Case NOVAS3Functions.State
                    Dim JDArr(1) As Double
                    JDArr(0) = JDTest
                    JDArr(1) = 0
                    rc = Nov31.State(JDArr, Target.Pluto, Pos, Vel)
                    LogRC31(TestFunction, "Pluto", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                Case NOVAS3Functions.Aberration
                    rc = 0
                    Nov31.RaDec2Vector(20.0, 40.0, 100, Pos)
                    LogRC31(TestFunction, "X, Y, Z", rc, Pos(0) & " " & Pos(1) & " " & Pos(2), "")

                    Nov31.Aberration(Pos, Pos, 10.0, Pos2)
                    LogRC31(TestFunction, "X, Y, Z", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.AppPlanet
                    rc = Nov31.AppPlanet(JDTest, ObjectJupiter, Accuracy.Full, RA, Dec, Dis)
                    LogRC31(TestFunction, "Jupiter", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3) & " " & Dis, "")
                    ObjectJupiter.Number = Body.Moon
                    ObjectJupiter.Name = "Moon"
                    ObjectJupiter.Type = ObjectType.MajorPlanetSunOrMoon
                    rc = Nov31.AppPlanet(JDTest, ObjectJupiter, Accuracy.Full, RA, Dec, Dis)
                    LogRC31(TestFunction, "Moon", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3) & " " & Dis, "")
                Case NOVAS3Functions.AppStar
                    rc = Nov31.AppStar(JDTest, CatEnt, Accuracy.Full, RA, Dec)
                    LogRC31(TestFunction, "RA Dec", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3), "")
                Case NOVAS3Functions.AstroPlanet
                    rc = Nov31.AstroPlanet(JDTest, ObjectJupiter, Accuracy.Full, RA, Dec, Dis)
                    LogRC31(TestFunction, "Jupiter", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3) & " " & Dis, "")
                Case NOVAS3Functions.AstroStar
                    CatEnt.Catalog = "FK6"
                    CatEnt.Dec = 45.45
                    CatEnt.Parallax = 0.0
                    CatEnt.ProMoDec = 0.0
                    CatEnt.ProMoRA = 0.0
                    CatEnt.RA = 15.15
                    CatEnt.RadialVelocity = 0.0
                    CatEnt.StarName = "GMB 1830"
                    CatEnt.StarNumber = 1307

                    rc = Nov31.AstroStar(Utl.JulianDate, CatEnt, Accuracy.Reduced, RA, Dec)
                    LogRC31(TestFunction, "Reduced accuracy:", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3), "")
                    rc = Nov31.AstroStar(Utl.JulianDate, CatEnt, Accuracy.Full, RA, Dec)
                    LogRC31(TestFunction, "Full accuracy:   ", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3), "")
                Case NOVAS3Functions.Bary2Obs
                    Dim LightTime As Double
                    rc = 0
                    Nov31.RaDec2Vector(20.0, 40.0, 100, Pos)

                    Nov31.Bary2Obs(Pos, PosObs, Pos2, LightTime)
                    LogRC31(TestFunction, "X, Y, Z, LightTime", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2) & " " & LightTime, "")
                Case NOVAS3Functions.CalDate
                    Dim Year, Month, Day As Short, Hour As Double
                    rc = 0
                    Nov31.CalDate(JDTest, Year, Month, Day, Hour)
                    LogRC31(TestFunction, "Year Month Day Hour", rc, Year & " " & Month & " " & Day & " " & Format(Hour, "0.0"), "2010 12 30 9" & DecimalSeparator & "0")
                Case NOVAS3Functions.Cel2Ter
                    rc = Nov31.Cel2Ter(JDTest, 0.0, 0.0, Method.EquinoxBased, Accuracy.Full, OutputVectorOption.ReferredToEquatorAndEquinoxOfDate, 0.0, 0.0, Pos, Pos2)
                    LogRC31(TestFunction, "Pos2", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.CelPole
                    Dim DPole1, DPole2 As Double
                    rc = Nov31.CelPole(JDTest, PoleOffsetCorrection.ReferredToMeanEclipticOfDate, DPole1, DPole2)
                    LogRC31(TestFunction, "Mean Ecliptic Of Date", rc, DPole1 & " " & DPole2, "")
                    rc = Nov31.CelPole(JDTest, PoleOffsetCorrection.ReferredToGCRSAxes, DPole1, DPole2)
                    LogRC31(TestFunction, "GCRS Axes", rc, DPole1 & " " & DPole2, "")
                Case NOVAS3Functions.CioArray
                    Dim Cio As New ArrayList
                    rc = Nov31.CioArray(JDTest, 20, Cio)
                    LogRC31(TestFunction, "RC", rc, rc, "")
                    rc = 0
                    For Each CioA As ASCOM.Astrometry.RAOfCio In Cio
                        LogRC31(TestFunction, "CIO Array", rc, CioA.JdTdb & " " & CioA.RACio, "")
                    Next
                Case NOVAS3Functions.CioBasis
                    Dim x, y, z As Double
                    rc = Nov31.CioBasis(JDTest, 20.0, ReferenceSystem.GCRS, Accuracy.Full, x, y, z)
                    LogRC31(TestFunction, "CIO Basis", rc, x & " " & y & " " & z, "")
                Case NOVAS3Functions.CioLocation
                    Dim RAofCIO As Double, RefSys As ASCOM.Astrometry.ReferenceSystem
                    rc = Nov31.CioLocation(JDTest, Accuracy.Full, RAofCIO, RefSys)
                    LogRC31(TestFunction, "CIO Location", rc, RAofCIO & " " & RefSys.ToString, "")
                Case NOVAS3Functions.CioRa
                    rc = Nov31.CioRa(JDTest, Accuracy.Full, RA)
                    LogRC31(TestFunction, "CIO RA", rc, RA, "")
                Case NOVAS3Functions.DLight
                    Dim DLight As Double
                    rc = 0
                    DLight = Nov31.DLight(Pos1, PosObs)
                    LogRC31(TestFunction, "D Light", rc, DLight, "")
                Case NOVAS3Functions.Ecl2EquVec
                    rc = Nov31.Ecl2EquVec(JDTest, CoordSys.CIOOfDate, Accuracy.Full, Pos1, Pos2)
                    LogRC31(TestFunction, "X, Y, Z", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.EeCt
                    rc = 0
                    JD = Nov31.EeCt(JDTest, 0.0, Accuracy.Full)
                    LogRC31(TestFunction, "J Date", rc, JD, "")
                Case NOVAS3Functions.Ephemeris
                    Dim JD1(1) As Double
                    JD1(0) = JDTest
                    rc = Nov31.Ephemeris(JD1, ObjectJupiter, Origin.Barycentric, Accuracy.Full, Pos, Vel)
                    LogRC31(TestFunction, "X, Y, Z", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                Case NOVAS3Functions.Equ2Ecl
                    Dim ELon, ELat As Double
                    RA = 16.0
                    Dec = 40.0
                    rc = Nov31.Equ2Ecl(JDTest, CoordSys.EquinoxOfDate, Accuracy.Full, RA, Dec, ELon, ELat)
                    LogRC31(TestFunction, "E Lon E Lat", rc, ELon & " " & ELat, "")
                Case NOVAS3Functions.Equ2EclVec
                    rc = Nov31.Equ2EclVec(JDTest, CoordSys.EquinoxOfDate, Accuracy.Full, Pos1, Pos2)
                    LogRC31(TestFunction, "X, Y, Z", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.Equ2Gal
                    Dim GLat, GLong As Double
                    rc = 0
                    Nov31.Equ2Gal(12.456, 40.0, GLong, GLat)
                    LogRC31(TestFunction, "G Long, G Lat", rc, GLong & " " & GLat, "")
                Case NOVAS3Functions.Equ2Hor
                    Dim ZD, Az, RaR, DecR As Double
                    rc = 0
                    Nov31.Equ2Hor(JDTest, 0.0, Accuracy.Full, 30.0, 50.0, OnSurf, RA, Dec, RefractionOption.LocationRefraction, ZD, Az, RaR, DecR)
                    LogRC31(TestFunction, "ZD Az RaR DecR", rc, ZD & " " & Az & " " & RaR & " " & DecR, "")
                Case NOVAS3Functions.Era
                    Dim Era As Double
                    rc = 0
                    Era = Nov31.Era(JDTest, 0.0)
                    LogRC31(TestFunction, "Era", rc, Era, "")
                Case NOVAS3Functions.ETilt
                    Dim Mobl, Tobl, Ee, DEps, DPsi As Double
                    rc = 0
                    Nov31.ETilt(JDTest, Accuracy.Full, Mobl, Tobl, Ee, DPsi, DEps)
                    LogRC31(TestFunction, "Mobl, Tobl, Ee, DPsi, DEps", rc, Format(Mobl, "0.00000000") & " " & Format(Tobl, "0.00000000") & " " & Format(Ee, "0.00000000") & " " & Format(DPsi, "0.00000000") & " " & Format(DEps, "0.00000000"), "")
                Case NOVAS3Functions.FrameTie
                    rc = 0
                    Nov31.FrameTie(Pos1, FrameConversionDirection.DynamicalToICRS, Pos2)
                    LogRC31(TestFunction, "X, Y, Z", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.FundArgs
                    Dim A(4) As Double
                    rc = 0
                    Nov31.FundArgs(JDTest, A)
                    LogRC31(TestFunction, "A", rc, Format(A(0), "0.00000000") & " " & Format(A(1), "0.00000000") & " " & Format(A(2), "0.00000000") & " " & Format(A(3), "0.00000000") & " " & Format(A(4), "0.00000000"), "")
                Case NOVAS3Functions.Gcrs2Equ
                    Dim RaG, DecG As Double
                    RaG = 11.5
                    DecG = 40.0
                    rc = Nov31.Gcrs2Equ(JDTest, CoordSys.EquinoxOfDate, Accuracy.Full, RaG, DecG, RA, Dec)
                    LogRC31(TestFunction, "RaG 11.5, DecG 40.0", rc, RA & " " & Dec, "")
                Case NOVAS3Functions.GeoPosVel
                    rc = Nov31.GeoPosVel(JDTest, 0.0, Accuracy.Full, Observer, Pos, Vel)
                    LogRC31(TestFunction, "Pos, Vel", rc, Format(Pos(0), "0.0000000000") & " " & Format(Pos(1), "0.0000000000") & " " & Format(Pos(2), "0.0000000000") & " " & Format(Vel(0), "0.0000000000") & " " & Format(Vel(1), "0.0000000000") & " " & Format(Vel(2), "0.0000000000"), "")
                Case NOVAS3Functions.GravDef
                    rc = Nov31.GravDef(JDTest, EarthDeflection.AddEarthDeflection, Accuracy.Full, Pos1, PosObs, Pos2)
                    LogRC31(TestFunction, "X, Y, Z", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.GravVec
                    Dim RMass As Double
                    rc = 0
                    Nov31.GravVec(Pos1, PosObs, PosBody, RMass, Pos2)
                    LogRC31(TestFunction, "X, Y, Z", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.IraEquinox
                    Dim Ira As Double
                    rc = 0
                    Ira = Nov31.IraEquinox(JDTest, EquinoxType.MeanEquinox, Accuracy.Full)
                    LogRC31(TestFunction, "Ira", rc, Ira, "")
                Case NOVAS3Functions.JulianDate
                    JD = Nov31.JulianDate(2000, 1, 1, 12)
                    LogRC31(TestFunction, "J2000: ", 0, JD, NOVAS.NOVAS2.JulianDate(2000, 1, 1, 12.0))
                    JD = Nov31.JulianDate(2010, 1, 2, 0.0)
                    LogRC31(TestFunction, "J2010: ", 0, JD, NOVAS.NOVAS2.JulianDate(2010, 1, 2, 0.0))
                Case NOVAS3Functions.LightTime
                    Dim TLight0, TLight As Double
                    TLight0 = 0.0
                    rc = Nov31.LightTime(JDTest, ObjectJupiter, PosObs, TLight0, Accuracy.Full, Pos, TLight)
                    LogRC31(TestFunction, "X, Y, Z", rc, Pos(0) & " " & Pos(1) & " " & Pos(2) & " " & TLight, "")
                Case NOVAS3Functions.LimbAngle
                    Dim LimbAng, NadirAngle As Double
                    rc = 0
                    Nov31.LimbAngle(PosObj, PosObs, LimbAng, NadirAngle)
                    LogRC31(TestFunction, "LimbAng, NadirAngle", rc, LimbAng & " " & NadirAngle, "")
                Case NOVAS3Functions.LocalPlanet
                    rc = Nov31.LocalPlanet(JDTest, ObjectJupiter, 0.0, OnSurf, Accuracy.Full, RA, Dec, Dis)
                    LogRC31(TestFunction, "Ra, Dec, Dis", rc, RA & " " & Dec & " " & Dis, "")
                Case NOVAS3Functions.LocalStar
                    rc = Nov31.LocalStar(JDTest, 0.0, CatEnt, OnSurf, Accuracy.Full, RA, Dec)
                    LogRC31(TestFunction, "Ra, Dec", rc, RA & " " & Dec, "")
                Case NOVAS3Functions.MakeCatEntry
                    Nov31.MakeCatEntry("A Star", "FK6", 1234545, 11.0, 12, 0, 13.0, 14.0, 15.0, CatEnt)
                    rc = 0
                    LogRC31(TestFunction, "CatEnt", rc, CatEnt.Catalog & " " & CatEnt.Dec & " " & CatEnt.Parallax & " " & CatEnt.StarName & " " & CatEnt.StarNumber, "")
                Case NOVAS3Functions.MakeInSpace
                    Dim Insp As New InSpace
                    Dim PosOrg(2), VelOrg(2) As Double
                    PosOrg(0) = 1
                    PosOrg(1) = 2
                    PosOrg(2) = 3
                    VelOrg(0) = 4
                    VelOrg(1) = 5
                    VelOrg(2) = 6
                    Insp.ScPos = Pos
                    Insp.ScVel = Vel
                    rc = 0
                    Nov31.MakeInSpace(PosOrg, VelOrg, Insp)
                    LogRC31(TestFunction, "Pos, Vel", rc, Insp.ScPos(0) & Insp.ScPos(1) & Insp.ScPos(2) & Insp.ScVel(0) & Insp.ScVel(1) & Insp.ScVel(2), "123456")
                Case NOVAS3Functions.MakeObject
                    rc = Nov31.MakeObject(ObjectType.MajorPlanetSunOrMoon, 7, "Uranus", CatEnt, ObjectJupiter)
                    LogRC31(TestFunction, "Object3", rc, ObjectJupiter.Name & " " & ObjectJupiter.Number & " " & ObjectJupiter.Type.ToString & " " & ObjectJupiter.Star.RA, "")
                Case NOVAS3Functions.MakeObserver
                    Dim Obs As New Observer
                    OnSurf.Latitude = 51.0
                    Nov31.MakeObserver(ObserverLocation.EarthSurface, OnSurf, New InSpace, Obs)
                    rc = 0
                    LogRC31(TestFunction, "Observer", rc, Obs.Where.ToString & " " & Obs.OnSurf.Latitude, "")
                Case NOVAS3Functions.MakeObserverAtGeocenter
                    Dim Obs As New Observer
                    Nov31.MakeObserverAtGeocenter(Obs)
                    rc = 0
                    LogRC31(TestFunction, "Observer", rc, Obs.Where.ToString & " " & Obs.OnSurf.Latitude, "")
                Case NOVAS3Functions.MakeObserverInSpace
                    Dim Obs As New Observer
                    Nov31.MakeObserverInSpace(Pos, Vel, Obs)
                    rc = 0
                    LogRC31(TestFunction, "Observer", rc, Obs.Where.ToString & " " & Obs.OnSurf.Latitude, "")
                Case NOVAS3Functions.MakeObserverOnSurface
                    Dim Obs As New Observer
                    Nov31.MakeObserverOnSurface(51.0, 0.0, 80.0, 25.0, 1010, Obs)
                    rc = 0
                    LogRC31(TestFunction, "Observer", rc, Obs.Where.ToString & " " & Obs.OnSurf.Latitude, "")
                Case NOVAS3Functions.MakeOnSurface
                    Nov31.MakeOnSurface(51.0, 0.0, 80.0, 25, 0, OnSurf)
                    rc = 0
                    LogRC31(TestFunction, "OnSurface", rc, OnSurf.Latitude & " " & OnSurf.Height, "")
                Case NOVAS3Functions.MeanObliq
                    Dim MO As Double
                    MO = Nov31.MeanObliq(JDTest)
                    rc = 0
                    LogRC31(TestFunction, "Mean Obl", rc, MO, "")
                Case NOVAS3Functions.MeanStar
                    Dim IRa, IDec As Double
                    rc = Nov31.MeanStar(JDTest, RA, Dec, Accuracy.Full, IRa, IDec)
                    LogRC31(TestFunction, "IRa, IDec", rc, IRa & " " & IDec, "")
                Case NOVAS3Functions.NormAng
                    Dim NA As Double
                    NA = Nov31.NormAng(4 * 3.142)
                    rc = 0
                    LogRC31(TestFunction, "Norm Ang", rc, NA, "")
                Case NOVAS3Functions.Nutation
                    Dim Obs As New Observer
                    rc = 0
                    Nov31.Nutation(JDTest, NutationDirection.MeanToTrue, Accuracy.Full, Pos, Pos2)
                    LogRC31(TestFunction, "Pos, Pos2", rc, Format(Pos(0), "0.00000000") & " " & Format(Pos(1), "0.00000000") & " " & Format(Pos(2), "0.00000000") & " " & Format(Pos2(0), "0.00000000") & " " & Format(Pos2(1), "0.00000000") & " " & Format(Pos2(2), "0.00000000"), "")
                Case NOVAS3Functions.NutationAngles
                    Dim DPsi, DEps As Double
                    rc = 0
                    Nov31.NutationAngles(JDTest, Accuracy.Full, DPsi, DEps)
                    LogRC31(TestFunction, "DPsi, DEps", rc, DPsi & " " & DEps, "")
                Case NOVAS3Functions.Place
                    ObjectJupiter.Name = "Planet"
                    'Obj.Star = CatEnt
                    ObjectJupiter.Type = 0
                    OnSurf.Height = 80
                    OnSurf.Latitude = Utl.DMSToDegrees("51:04:43")
                    OnSurf.Longitude = -Utl.DMSToDegrees("00:17:40")
                    OnSurf.Pressure = 1010
                    OnSurf.Temperature = 10

                    Observer.Where = ObserverLocation.EarthSurface
                    Observer.OnSurf = OnSurf

                    For i As Short = 1 To 11
                        If i <> 3 Then 'Skip earth test as not relevant - viewing earth from earth has no meaning!
                            ObjectJupiter.Number = i
                            JD = Utl.JulianDate
                            rc = Nov31.Place(JDTest, ObjectJupiter, Observer, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Full, skypos)
                            rc = Nov31.Place(JDTest, ObjectJupiter, Observer, DeltaT, CoordSys.EquinoxOfDate, Accuracy.Reduced, SkyPos1)
                            LogRC31(TestFunction, "Planet " & Microsoft.VisualBasic.Right(" " & i.ToString, 2) & "", rc, Utl.HoursToHMS(SkyPos1.RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(SkyPos1.Dec, ":", ":", "", 3), Utl.HoursToHMS(skypos.RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(skypos.Dec, ":", ":", "", 3))
                        End If
                    Next
                Case NOVAS3Functions.Precession
                    rc = Nov31.Precession(JDTest, Pos, J2000, Pos2)
                    LogRC31(TestFunction, "Pos2", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.ProperMotion
                    rc = 0
                    Nov31.ProperMotion(JDTest, Pos, Vel, J2000, Pos2)
                    LogRC31(TestFunction, "Pos2", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.RaDec2Vector
                    rc = 0
                    Nov31.RaDec2Vector(11.0, 12.0, 13.0, Pos)
                    LogRC31(TestFunction, "Pos", rc, Pos(0) & " " & Pos(1) & " " & Pos(2), "")
                Case NOVAS3Functions.RadVel
                    Dim Rv As Double
                    rc = 0
                    Nov31.RadVel(ObjectJupiter, Pos, Vel, VelObs, 12.0, 14.0, 16.0, Rv)
                    LogRC31(TestFunction, "Rv", rc, Rv, "")
                Case NOVAS3Functions.ReadEph
                    Dim Err As Integer, Eph(5) As Double
                    rc = 0
                    Eph = Nov31.ReadEph(99, "missingasteroid", JDTest, Err)
                    LogRC31(TestFunction, "Expect error 4", rc, Err & Eph(0) & Eph(1) & Eph(2) & Eph(3) & Eph(4) & Eph(5), "4000000")
                    JD = 2453415.5
                    Eph = Nov31.ReadEph(1, "Ceres", JD, Err)
                    LogRC31(TestFunction, "JD Before " & JD, rc, Err & " " & Format(Eph(0), "0.00000") & " " & Format(Eph(1), "0.00000") & " " & Format(Eph(2), "0.00000") & " " & Format(Eph(3), "0.00000") & " " & Format(Eph(4), "0.00000") & " " & Format(Eph(5), "0.00000"), "3 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000")
                    JD = 2453425.5
                    Eph = Nov31.ReadEph(1, "Ceres", JD, Err)
                    LogRC31(TestFunction, "JD Start  " & JD, rc, Err & " " & Format(Eph(0), "0.00000") & " " & Format(Eph(1), "0.00000") & " " & Format(Eph(2), "0.00000") & " " & Format(Eph(3), "0.00000") & " " & Format(Eph(4), "0.00000") & " " & Format(Eph(5), "0.00000"), "0 -2" & DecimalSeparator & "23084 -1" & DecimalSeparator & "38495 -0" & DecimalSeparator & "19822 0" & DecimalSeparator & "00482 -0" & DecimalSeparator & "00838 -0" & DecimalSeparator & "00493")
                    JD = 2454400.5
                    Eph = Nov31.ReadEph(1, "Ceres", JD, Err)
                    LogRC31(TestFunction, "JD Mid    " & JD, rc, Err & " " & Format(Eph(0), "0.00000") & " " & Format(Eph(1), "0.00000") & " " & Format(Eph(2), "0.00000") & " " & Format(Eph(3), "0.00000") & " " & Format(Eph(4), "0.00000") & " " & Format(Eph(5), "0.00000"), "0 2" & DecimalSeparator & "02286 1" & DecimalSeparator & "91181 0" & DecimalSeparator & "48869 -0" & DecimalSeparator & "00736 0" & DecimalSeparator & "00559 0" & DecimalSeparator & "00413")
                    JD = 2455370.5 'Fails (screws up the DLL for subsequent calls) beyond JD = 2455389.5, which is just below the theoretical end of 2455402.5
                    Eph = Nov31.ReadEph(1, "Ceres", JD, Err)
                    LogRC31(TestFunction, "JD End    " & JD, rc, Err & " " & Format(Eph(0), "0.00000") & " " & Format(Eph(1), "0.00000") & " " & Format(Eph(2), "0.00000") & " " & Format(Eph(3), "0.00000") & " " & Format(Eph(4), "0.00000") & " " & Format(Eph(5), "0.00000"), "0 -0" & DecimalSeparator & "08799 -2" & DecimalSeparator & "57887 -1" & DecimalSeparator & "19703 0" & DecimalSeparator & "00983 -0" & DecimalSeparator & "00018 -0" & DecimalSeparator & "00209")
                    JD = 2455410.5
                    Eph = Nov31.ReadEph(1, "Ceres", JD, Err)
                    LogRC31(TestFunction, "JD After  " & JD, rc, Err & " " & Format(Eph(0), "0.00000") & " " & Format(Eph(1), "0.00000") & " " & Format(Eph(2), "0.00000") & " " & Format(Eph(3), "0.00000") & " " & Format(Eph(4), "0.00000") & " " & Format(Eph(5), "0.00000"), "3 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000 0" & DecimalSeparator & "00000")
                Case NOVAS3Functions.Refract
                    Dim Refracted As Double
                    rc = 0
                    Refracted = Nov31.Refract(OnSurf, RefractionOption.NoRefraction, 70.0)
                    LogRC31(TestFunction, "No refraction Zd 70.0", rc, Refracted, "")
                    Refracted = Nov31.Refract(OnSurf, RefractionOption.StandardRefraction, 70.0)
                    LogRC31(TestFunction, "Standard Zd 70.0     ", rc, Refracted, "")
                    Refracted = Nov31.Refract(OnSurf, RefractionOption.LocationRefraction, 70.0)
                    LogRC31(TestFunction, "Location Zd 70.0     ", rc, Refracted, "")
                Case NOVAS3Functions.SiderealTime
                    Dim MObl, TObl, ee, DPSI, DEps, GST2 As Double
                    JD = Utl.JulianDate
                    rc = Nov31.SiderealTime(JD, 0.0, DeltaT, GstType.GreenwichMeanSiderealTime, Method.EquinoxBased, Accuracy.Reduced, GST)
                    LogRC31(TestFunction, "Local Mean Equinox    ", rc, Utl.HoursToHMS(GST - (24.0 * Utl.DMSToDegrees("00:17:40") / 360), ":", ":", "", 3), "")
                    rc = Nov31.SiderealTime(JD, 0.0, DeltaT, GstType.GreenwichApparentSiderealTime, Method.EquinoxBased, Accuracy.Full, GST)
                    LogRC31(TestFunction, "Local Apparent Equinox", rc, Utl.HoursToHMS(GST - (24.0 * Utl.DMSToDegrees("00:17:40") / 360), ":", ":", "", 3), "")
                    rc = Nov31.SiderealTime(JD, 0.0, DeltaT, GstType.GreenwichMeanSiderealTime, Method.CIOBased, Accuracy.Reduced, GST)
                    LogRC31(TestFunction, "Local Mean CIO        ", rc, Utl.HoursToHMS(GST - (24.0 * Utl.DMSToDegrees("00:17:40") / 360), ":", ":", "", 3), "")
                    rc = Nov31.SiderealTime(JD, 0.0, DeltaT, GstType.GreenwichApparentSiderealTime, Method.CIOBased, Accuracy.Reduced, GST)
                    LogRC31(TestFunction, "Local Apparent CIO    ", rc, Utl.HoursToHMS(GST - (24.0 * Utl.DMSToDegrees("00:17:40") / 360), ":", ":", "", 3), "")
                    NOVAS.NOVAS2.EarthTilt(JD, MObl, TObl, ee, DPSI, DEps)
                    NOVAS.NOVAS2.SiderealTime(JD, 0.0, ee, GST2)
                    rc = Nov31.SiderealTime(JD, 0.0, DeltaT, GstType.GreenwichApparentSiderealTime, Method.EquinoxBased, Accuracy.Full, GST)
                    LogRCDouble(TestFunction, "Novas31", "GAST Equinox          ", rc, GST, GST2, TOLERANCE_E6)
                Case NOVAS3Functions.Spin
                    rc = 0
                    Nov31.Spin(20.0, Pos1, Pos2)
                    LogRC31(TestFunction, "Pos2", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.StarVectors
                    rc = 0
                    Nov31.StarVectors(CatEnt, Pos, Vel)
                    LogRC31(TestFunction, "Pos, Vel", rc, Format(Pos(0), "0.000") & " " & Format(Pos(1), "0.000") & " " & Format(Pos(2), "0.000") & " " & Format(Vel(0), "0.00000000") & " " & Format(Vel(1), "0.00000000") & " " & Format(Vel(2), "0.00000000"), "")
                Case NOVAS3Functions.Tdb2Tt
                    Dim TT, Secdiff As Double
                    rc = 0
                    Nov31.Tdb2Tt(JDTest, TT, Secdiff)
                    LogRC31(TestFunction, "Pos, Vel", rc, TT & " " & Secdiff, "")
                Case NOVAS3Functions.Ter2Cel
                    rc = Nov31.Ter2Cel(JDTest, 0.0, 0.0, Method.EquinoxBased, Accuracy.Full, OutputVectorOption.ReferredToEquatorAndEquinoxOfDate, 0.0, 0.0, Pos, Pos2)
                    LogRC31(TestFunction, "Pos2", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case NOVAS3Functions.Terra
                    rc = 0
                    Nov31.Terra(OnSurf, 0.0, Pos, Vel)
                    LogRC31(TestFunction, "Pos, Vel", rc, Format(Pos(0), "0.00000000") & " " & Format(Pos(1), "0.00000000") & " " & Format(Pos(2), "0.00000000") & " " & Format(Vel(0), "0.00000000") & " " & Format(Vel(1), "0.00000000") & " " & Format(Vel(2), "0.00000000"), "")
                Case NOVAS3Functions.TopoPlanet
                    rc = Nov31.TopoStar(JDTest, 0.0, CatEnt, OnSurf, Accuracy.Full, RA, Dec)
                    LogRC31(TestFunction, "RA, Dec", rc, RA & " " & Dec, "")
                Case NOVAS3Functions.TopoStar
                    CatEnt.Catalog = "HIP"
                    CatEnt.Dec = Utl.DMSToDegrees("16:30:31")
                    CatEnt.Parallax = 0.0
                    CatEnt.ProMoDec = 0.0
                    CatEnt.ProMoRA = 0.0
                    CatEnt.RA = Utl.HMSToHours("04:35:55.2")
                    CatEnt.RadialVelocity = 0.0
                    CatEnt.StarName = "Aldebaran"
                    CatEnt.StarNumber = 21421

                    OnSurf.Height = 80
                    OnSurf.Latitude = 51.0
                    OnSurf.Longitude = 0.0
                    OnSurf.Pressure = 1010
                    OnSurf.Temperature = 10

                    rc = Nov31.TopoStar(Utl.JulianDate, 0.0, CatEnt, OnSurf, Accuracy.Reduced, RA, Dec)
                    LogRC31(TestFunction, "Reduced accuracy", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3), "")

                    rc = Nov31.TopoStar(Utl.JulianDate, 0.0, CatEnt, OnSurf, Accuracy.Full, RA, Dec)
                    LogRC31(TestFunction, "Full accuracy   ", rc, Utl.HoursToHMS(RA, ":", ":", "", 3) & " " & Utl.HoursToHMS(Dec, ":", ":", "", 3), "")
                Case NOVAS3Functions.TransformCat
                    Dim NewCat As New CatEntry3
                    rc = Nov31.TransformCat(TransformationOption3.ChangeEquatorAndEquinoxAndEpoch, JDTest, CatEnt, J2000, "PGS", NewCat)
                    LogRC31(TestFunction, "NewCat", rc, NewCat.RA & " " & NewCat.Dec & " " & NewCat.Catalog & " " & NewCat.StarName, "")
                Case NOVAS3Functions.TransformHip
                    Dim HipCat As New CatEntry3
                    rc = 0
                    Nov31.TransformHip(CatEnt, HipCat)
                    LogRC31(TestFunction, "HipCat", rc, HipCat.RA & " " & HipCat.Dec & " " & HipCat.Catalog & " " & HipCat.StarName, "")
                Case NOVAS3Functions.Vector2RaDec
                    rc = Nov31.Vector2RaDec(Pos, RA, Dec)
                    LogRC31(TestFunction, "RA, Dec", rc, RA & " " & Dec, "")
                Case NOVAS3Functions.VirtualPlanet
                    rc = Nov31.VirtualPlanet(JDTest, ObjectJupiter, Accuracy.Full, RA, Dec, Dis)
                    LogRC31(TestFunction, "RA, Dec, Dis", rc, RA & " " & Dec & " " & Dis, "")
                Case NOVAS3Functions.VirtualStar
                    rc = Nov31.VirtualStar(JDTest, CatEnt, Accuracy.Full, RA, Dec)
                    LogRC31(TestFunction, "RA, Dec", rc, RA & " " & Dec, "")
                Case NOVAS3Functions.Wobble
                    rc = 0
                    Nov31.Wobble(JDTest, TransformationDirection.ITRSToTerrestrialIntermediate, 1.0, 1.0, Pos1, Pos2)
                    LogRC31(TestFunction, "ITRS2Terr Pos2", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                    Nov31.Wobble(JDTest, TransformationDirection.TerrestrialIntermediateToITRS, 1.0, 1.0, Pos1, Pos2)
                    LogRC31(TestFunction, "Terr2ITRS Pos2", rc, Pos2(0) & " " & Pos2(1) & " " & Pos2(2), "")
                Case Else
                    TL.LogMessage(TestFunction.ToString, "Test not implemented")
            End Select

        Catch ex As Exception
            LogException("Novas31", TestFunction.ToString & " - " & ex.ToString)
        End Try
        Action("")
    End Sub

    Sub LogRC31(ByVal Test As NOVAS3Functions, ByVal Note As String, ByVal rc As Integer, ByVal msg As String, ByVal Comparison As String)
        Dim LMsg As String
        If Note <> "" Then
            Note = Note & ": "
            LMsg = Note & msg
        Else
            LMsg = msg
        End If

        If rc = Integer.MaxValue Then ' Test is not implemented
            TL.LogMessage("Novas31 *****", "Test " & Test.ToString & " is not implemented")
        Else

            If rc = 0 Then

                If String.IsNullOrEmpty(Comparison) Then 'No comparison so it must be a success!
                    Compare("Novas31", Test.ToString & " - " & LMsg & " RC", rc.ToString, "0")
                Else 'Check comparison value and respond accordingly
                    Compare("Novas31", Test.ToString & " - " & LMsg & " RC", msg, Comparison)
                End If
            Else
                Compare("Novas31", Test.ToString & " RC", rc.ToString, "0")
            End If
        End If
    End Sub

    Sub LogRC(ByVal Test As NOVAS3Functions, ByVal Note As String, ByVal rc As Integer, ByVal msg As String, ByVal Comparison As String)
        Dim LMsg As String
        If Note <> "" Then
            Note = Note & ": "
            LMsg = Note & msg
        Else
            LMsg = msg
        End If

        If rc = Integer.MaxValue Then ' Test is not implemented
            TL.LogMessage("Novas3 *****", "Test " & Test.ToString & " is not implemented")
        Else

            If rc = 0 Then

                If String.IsNullOrEmpty(Comparison) Then 'No comparison so it must be a success!
                    Compare("Novas3", Test.ToString & " - " & LMsg & " RC", rc.ToString, "0")
                Else 'Check comparison value and respond accordingly
                    Compare("Novas3", Test.ToString & " - " & LMsg & " RC", msg, Comparison)
                End If
            Else
                Compare("Novas3", Test.ToString & " RC", rc.ToString, "0")
            End If
        End If
    End Sub

    Sub LogRCDouble(ByVal Test As NOVAS3Functions, ByVal Component As String, ByVal Note As String, ByVal rc As Integer, ByVal msg As Double, ByVal Comparison As Double, ByVal Tolerance As Double)

        If rc = Integer.MaxValue Then ' Test is not implemented
            TL.LogMessage(Component & " *****", "Test " & Test.ToString & " is not implemented")
        Else
            If rc = 0 Then
                CompareDouble(Component, Test.ToString & " - " & Note, msg, Comparison, Tolerance)
            Else
                Compare(Component, Test.ToString & " RC", rc.ToString, "0")
            End If
        End If
    End Sub

    Enum NOVAS2Functions
        Abberation
        App_Planet
        App_Star
        Astro_Planet
        Astro_Star
        Bary_To_Geo
        Cal_Date
        Cel_Pole
        EarthTilt
        Ephemeris
        Equ2Hor
        Fund_Args
        Get_Earth
        Julian_Date
        Local_Planet
        Local_Star
        Make_Cat_Entry
        Mean_Star
        Nutate
        Nutation_Angles
        PNSW
        Precession
        Proper_Motion
        RADEC2Vector
        Refract
        Set_Body
        Sideral_Time
        SolarSystem
        Spin
        StarVectors
        Sun_Field
        Tdb2Tdt
        Terra
        Topo_Planet
        Topo_Star
        Transform_Cat
        Transform_Hip
        Vector2RADEC
        Virtual_Planet
        Virtual_Star
        Wobble
        'Not in DLL  from Tim
        Sun_Eph
    End Enum

    Sub NOVAS2Tests()
        Dim T As Transform.Transform = New Transform.Transform
        Dim u As New Util
        Dim EarthBody, SunBody As New BodyDescription, StarStruct As New CatEntry, LocationStruct As New SiteInfo
        Dim Star(), SKYPOS(), POS(), POS2(), POSEarth(), VEL2(), POSNow(), VEL() As Double
        ReDim Star(10), SKYPOS(10), POS(3), VEL(3), POSNow(3), POS2(3), VEL2(3), POSEarth(3)
        Dim P(5), FundArgsValue(4) As Double
        Dim ZenithDistance, Azimuth, Tdb, LightTime, RATarget, DECTarget, JD, Distance As Double
        Dim RANow, DECNow, Hour, TdtJd, SecDiff, LongNutation, ObliqNutation, GreenwichSiderealTime, MObl, TObl, Eq, DPsi, DEpsilon As Double
        Dim RC, Year, Month, day As Short
        Dim Fmt As String

        Const SiteLat = 51.0 + (4.0 / 60.0) + (43.0 / 3600.0)
        Const SiteLong = 0.0 - (17.0 / 60.0) - (40.0 / 3600.0)
        Const SiteElev = 80.0

        Const StarRAJ2000 = 12.0
        Const StarDecJ2000 = 30.0

        JD = TestJulianDate()

        EarthBody.Name = "Earth"
        EarthBody.Type = BodyType.MajorPlanet
        EarthBody.Number = 3

        SunBody.Name = "Sun"
        SunBody.Type = BodyType.Sun
        SunBody.Number = Body.Sun

        LocationStruct.Height = SiteElev
        LocationStruct.Latitude = SiteLat
        LocationStruct.Longitude = SiteLong
        LocationStruct.Pressure = 1000.0
        LocationStruct.Temperature = 10.0

        StarStruct.Dec = StarDecJ2000
        StarStruct.RA = StarRAJ2000
        StarStruct.Parallax = 2.0
        StarStruct.ProMoDec = 1.5
        StarStruct.ProMoRA = 2.5
        StarStruct.RadialVelocity = 3

        NOVAS.NOVAS2.StarVectors(StarStruct, POS, VEL)
        NOVAS.NOVAS2.Vector2RADec(POS, RATarget, DECTarget)
        CompareDouble("Novas2Tests", "J2000 RA Target", RATarget, StarRAJ2000, TOLERANCE_E9, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("Novas2Tests", "J2000 Dec Target", DECTarget, StarDecJ2000, TOLERANCE_E9, DoubleDisplayAs.DegreesMinutesSeconds)

        NOVAS.NOVAS2.Precession(J2000, POS, u.JulianDate, POSNow)
        NOVAS.NOVAS2.Vector2RADec(POSNow, RANow, DECNow)
        RC = NOVAS.NOVAS2.TopoStar(JD, EarthBody, 0, StarStruct, LocationStruct, RANow, DECNow)
        Compare("Novas2Tests", "TopoStar RC", RC, 0)
        CompareDouble("Novas2Tests", "Topo RA", RANow, 12.0098595883453, TOLERANCE_E9, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("Novas2Tests", "Topo Dec", DECNow, 29.933637435611, TOLERANCE_E9, DoubleDisplayAs.DegreesMinutesSeconds)

        NOVAS.NOVAS2.RADec2Vector(StarRAJ2000, StarDecJ2000, 10000000000.0, POS)
        NOVAS.NOVAS2.Vector2RADec(POS, RATarget, DECTarget)
        CompareDouble("Novas2Tests", "RADec2Vector", RATarget, StarRAJ2000, TOLERANCE_E9, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("Novas2Tests", "RADec2Vector", DECTarget, StarDecJ2000, TOLERANCE_E9, DoubleDisplayAs.DegreesMinutesSeconds)

        CompareDouble("Novas2Tests", "JulianDate", NOVAS.NOVAS2.JulianDate(2010, 12, 30, 9.0), TestJulianDate, TOLERANCE_E9)

        RC = NOVAS.NOVAS2.AstroPlanet(JD, SunBody, EarthBody, RATarget, DECTarget, Distance)
        Compare("Novas2Tests", "AstroPlanet RC", RC, 0)
        CompareDouble("Novas2Tests", "AstroPlanet RA", RATarget, 18.6090529142058, TOLERANCE_E9, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("Novas2Tests", "AstroPlanet Dec", DECTarget, -23.172110257017, TOLERANCE_E9, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareDouble("Novas2Tests", "AstroPlanet Dist", Distance, 0.983376046291495, TOLERANCE_E9)

        RC = NOVAS.NOVAS2.VirtualPlanet(JD, SunBody, EarthBody, RANow, DECNow, Distance)
        Compare("Novas2Tests", "VirtualPlanet RC", RC, 0)
        CompareDouble("Novas2Tests", "VirtualPlanet RA", RANow, 18.6086339599669, TOLERANCE_E9, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("Novas2Tests", "VirtualPlanet Dec", DECNow, -23.1724757087899, TOLERANCE_E9, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareDouble("Novas2Tests", "VirtualPlanet Dist", Distance, 0.983376046291495, TOLERANCE_E9)

        RC = NOVAS.NOVAS2.AppPlanet(JD, SunBody, EarthBody, RANow, DECNow, Distance)
        Compare("Novas2Tests", "AppPlanet RC", RC, 0)
        CompareDouble("Novas2Tests", "AppPlanet RA", RANow, 18.620097981585, TOLERANCE_E9, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("Novas2Tests", "AppPlanet Dec", DECNow, -23.162343811122, TOLERANCE_E9, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareDouble("Novas2Tests", "AppPlanet Dist", Distance, 0.983376046291495, TOLERANCE_E9)

        RC = NOVAS.NOVAS2.TopoPlanet(JD, SunBody, EarthBody, 0.0, LocationStruct, RANow, DECNow, Distance)
        Compare("Novas2Tests", "TopoPlanet RC", RC, 0)
        CompareDouble("Novas2Tests", "TopoPlanet RA", RANow, 18.6201822342814, TOLERANCE_E9, DoubleDisplayAs.HoursMinutesSeconds)
        CompareDouble("Novas2Tests", "TopoPlanet Dec", DECNow, -23.1645247136453, TOLERANCE_E9, DoubleDisplayAs.DegreesMinutesSeconds)
        CompareDouble("Novas2Tests", "TopoPlanet Dist", Distance, 0.983371860482251, TOLERANCE_E9)
        TL.BlankLine()

        NOVAS.NOVAS2.Equ2Hor(JD, 0.0, 0.0, 0.0, LocationStruct, StarRAJ2000, StarDecJ2000, RefractionOption.LocationRefraction, ZenithDistance, Azimuth, RANow, DECNow)
        TL.LogMessage("Novas2Tests", "Equ2Hor RA - " & u.HoursToHMS(RANow, ":", ":", "", 3) & "  DEC: " & u.DegreesToDMS(DECNow, ":", ":", "", 3))
        TL.LogMessage("Novas2Tests", RANow & " " & DECNow & " " & ZenithDistance & " " & Azimuth)

        NOVAS.NOVAS2.EarthTilt(u.JulianDate, MObl, TObl, Eq, DPsi, DEpsilon)
        TL.LogMessage("Novas2Tests", "EarthTilt - Equation of Equinoxes - " & Eq & " DPsi: " & DPsi & " DEps: " & DEpsilon)
        NOVAS.NOVAS2.CelPole(10.0, 10.0)
        NOVAS.NOVAS2.EarthTilt(u.JulianDate, MObl, TObl, Eq, DPsi, DEpsilon)
        TL.LogMessage("Novas2Tests", "CelPole - Set Equation of Equinoxes - " & Eq & " DPsi: " & DPsi & " DEps: " & DEpsilon)
        NOVAS.NOVAS2.CelPole(0.0, 0.0)
        NOVAS.NOVAS2.EarthTilt(u.JulianDate, MObl, TObl, Eq, DPsi, DEpsilon)
        TL.LogMessage("Novas2Tests", "CelPole - Unset Equation of Equinoxes - " & Eq & " DPsi: " & DPsi & " DEps: " & DEpsilon)

        NOVAS.NOVAS2.SiderealTime(u.JulianDate, 0.0, Eq, GreenwichSiderealTime)
        TL.LogMessage("Novas2Tests", "SiderealTime - " & u.HoursToHMS(GreenwichSiderealTime))

        NOVAS.NOVAS2.Ephemeris(u.JulianDate, EarthBody, Origin.Heliocentric, POS, VEL)
        Fmt = "0.000000000000"
        TL.LogMessage("Novas2Tests", "Ephemeris Ea - " & Format(POS(0), Fmt) & " " & Format(POS(1), Fmt) & " " & Format(POS(2), Fmt) & " " & Format(VEL(0), Fmt) & " " & Format(VEL(1), Fmt) & " " & Format(VEL(2), Fmt))

        NOVAS.NOVAS2.Ephemeris(u.JulianDate, SunBody, Origin.Heliocentric, POS, VEL)
        TL.LogMessage("Novas2Tests", "Ephemeris Pl - " & Format(POS(0), Fmt) & " " & Format(POS(1), Fmt) & " " & Format(POS(2), Fmt) & " " & Format(VEL(0), Fmt) & " " & Format(VEL(1), Fmt) & " " & Format(VEL(2), Fmt))

        NOVAS.NOVAS2.SolarSystem(u.JulianDate, Body.Earth, Origin.Heliocentric, POS, VEL)
        TL.LogMessage("Novas2Tests", "SolarSystem Ea - " & Format(POS(0), Fmt) & " " & Format(POS(1), Fmt) & " " & Format(POS(2), Fmt) & " " & Format(VEL(0), Fmt) & " " & Format(VEL(1), Fmt) & " " & Format(VEL(2), Fmt))

        RC = NOVAS.NOVAS2.Vector2RADec(POS, RANow, DECNow)
        Compare("Novas2Tests", "Vector2RADec RC", RC, 0)
        TL.LogMessage("Novas2Tests", "Vector2RADec RA - " & u.HoursToHMS(RANow, ":", ":", "", 3) & "  DEC: " & u.DegreesToDMS(DECNow, ":", ":", "", 3))

        NOVAS.NOVAS2.StarVectors(StarStruct, POS, VEL)

        RC = NOVAS.NOVAS2.Vector2RADec(POS, RANow, DECNow)
        Compare("Novas2Tests", "Vector2RADec RC", RC, 0)
        TL.LogMessage("Novas2Tests", "StarVectors - " & Format(POS(0), Fmt) & " " & Format(POS(1), Fmt) & " " & Format(POS(2), Fmt) & " " & Format(VEL(0), Fmt) & " " & Format(VEL(1), Fmt) & " " & Format(VEL(2), Fmt))
        TL.LogMessage("Novas2Tests", "StarVectors RA - " & u.HoursToHMS(RANow, ":", ":", "", 3) & "  DEC: " & u.DegreesToDMS(DECNow, ":", ":", "", 3))

        NOVAS.NOVAS2.RADec2Vector(12.0, 30.0, 1000, POS)
        TL.LogMessage("Novas2Tests", "RADec2Vector - " & Format(POS(0), Fmt) & " " & Format(POS(1), Fmt) & " " & Format(POS(2), Fmt))

        RC = NOVAS.NOVAS2.GetEarth(u.JulianDate, EarthBody, Tdb, POS, VEL, POS2, VEL2)
        Compare("Novas2Tests", "GetEarth RC", RC, 0)
        TL.LogMessage("Novas2Tests", "GetEarth TDB - " & Tdb)
        TL.LogMessage("Novas2Tests", "GetEarth - " & Format(POS(0), Fmt) & " " & Format(POS(1), Fmt) & " " & Format(POS(2), Fmt) & " " & Format(VEL(0), Fmt) & " " & Format(VEL(1), Fmt) & " " & Format(VEL(2), Fmt))
        TL.LogMessage("Novas2Tests", "GetEarth - " & Format(POS2(0), Fmt) & " " & Format(POS2(1), Fmt) & " " & Format(POS2(2), Fmt) & " " & Format(VEL2(0), Fmt) & " " & Format(VEL2(1), Fmt) & " " & Format(VEL2(2), Fmt))

        RC = NOVAS.NOVAS2.MeanStar(u.JulianDate, EarthBody, 12.0, 30.0, RANow, DECNow)
        Compare("Novas2Tests", "MeanStar RC", RC, 0)
        TL.LogMessage("Novas2Tests", "MeanStar RA -  " & u.HoursToHMS(RANow, ":", ":", "", 3) & "  DEC: " & u.DegreesToDMS(DECNow, ":", ":", "", 3))

        NOVAS.NOVAS2.StarVectors(StarStruct, POS, VEL)
        TL.LogMessage("Novas2Tests", "Pnsw In - " & Format(POS(0), Fmt) & " " & Format(POS(1), Fmt) & " " & Format(POS(2), Fmt))

        NOVAS.NOVAS2.Pnsw(u.JulianDate, 15.0, 2.5, 5, POS, POSNow)
        TL.LogMessage("Novas2Tests", "Pnsw Out" & Format(POSNow(0), Fmt) & " " & Format(POSNow(1), Fmt) & " " & Format(POSNow(2), Fmt))

        NOVAS.NOVAS2.Spin(u.JulianDate, POS, POSNow)
        TL.LogMessage("Novas2Tests", "Spin - " & Format(POSNow(0), Fmt) & " " & Format(POSNow(1), Fmt) & " " & Format(POSNow(2), Fmt))

        NOVAS.NOVAS2.Wobble(2.5, 5.0, POS, POSNow)
        TL.LogMessage("Novas2Tests", "Wobble - " & Format(POSNow(0), Fmt) & " " & Format(POSNow(1), Fmt) & " " & Format(POSNow(2), Fmt))

        NOVAS.NOVAS2.Terra(LocationStruct, 15.0, POS, VEL)
        TL.LogMessage("Novas2Tests", "Terra - " & Format(POS(0), Fmt) & " " & Format(POS(1), Fmt) & " " & Format(POS(2), Fmt) & " " & Format(VEL(0), Fmt) & " " & Format(VEL(1), Fmt) & " " & Format(VEL(2), Fmt))

        NOVAS.NOVAS2.ProperMotion(J2000, POS, VEL, u.JulianDate, POSNow)
        TL.LogMessage("Novas2Tests", "ProperMotion - " & Format(POSNow(0), Fmt) & " " & Format(POSNow(1), Fmt) & " " & Format(POSNow(2), Fmt))

        RC = NOVAS.NOVAS2.GetEarth(u.JulianDate, EarthBody, Tdb, POSEarth, VEL, POS2, VEL2)
        Compare("Novas2Tests", "GetEarth RC", RC, 0)
        NOVAS.NOVAS2.BaryToGeo(POS, POSEarth, POSNow, LightTime)
        TL.LogMessage("Novas2Tests", "BaryToGeo - " & Format(POSNow(0), Fmt) & " " & Format(POSNow(1), Fmt) & " " & Format(POSNow(2), Fmt) & " LightTime: " & LightTime)

        RC = NOVAS.NOVAS2.SunField(POS, POSEarth, POSNow)
        Compare("Novas2Tests", "SunField RC", RC, 0)
        TL.LogMessage("Novas2Tests", "SunField - " & Format(POSNow(0), Fmt) & " " & Format(POSNow(1), Fmt) & " " & Format(POSNow(2), Fmt))

        RC = NOVAS.NOVAS2.Aberration(POS, VEL, LightTime, POSNow)
        Compare("Novas2Tests", "Aberration RC", RC, 0)
        TL.LogMessage("Novas2Tests", "Aberration - " & Format(POSNow(0), Fmt) & " " & Format(POSNow(1), Fmt) & " " & Format(POSNow(2), Fmt))

        RC = NOVAS.NOVAS2.Nutate(u.JulianDate, NutationDirection.MeanToTrue, POS, POSNow)
        Compare("Novas2Tests", "Nutate RC", RC, 0)
        TL.LogMessage("Novas2Tests", "Nutate - " & Format(POSNow(0), Fmt) & " " & Format(POSNow(1), Fmt) & " " & Format(POSNow(2), Fmt))

        RC = NOVAS.NOVAS2.NutationAngles(1.0, LongNutation, ObliqNutation)
        Compare("Novas2Tests", "NutationAngles RC", RC, 0)
        TL.LogMessage("Novas2Tests", "NutationAngles - Long Nutation: " & LongNutation & " Oblique Nutation: " & ObliqNutation)

        NOVAS.NOVAS2.FundArgs(1.0, FundArgsValue)
        TL.LogMessage("Novas2Tests", "FundArgs - " & FundArgsValue(0) & " " & FundArgsValue(1) & " " & FundArgsValue(2) & " " & FundArgsValue(3) & " " & FundArgsValue(4))

        NOVAS.NOVAS2.Tdb2Tdt(u.JulianDate, TdtJd, SecDiff)
        TL.LogMessage("Novas2Tests", "Tdb2Tdt - TDT JD: " & TdtJd & " Sec Diff: " & SecDiff)

        NOVAS.NOVAS2.SetBody(BodyType.MajorPlanet, Body.Earth, "Earth", SunBody)
        TL.LogMessage("Novas2Tests", "SetBody - Name: " & SunBody.Name & " Number: " & SunBody.Number & " Type: " & SunBody.Type)

        NOVAS.NOVAS2.MakeCatEntry("HIP", "PStar", 23045, 15.0, 30.0, 1, 2, 3, 4, StarStruct)
        TL.LogMessage("Novas2Tests", "MakeCatEntry - Cat: " & StarStruct.Catalog & " Name: " & StarStruct.StarName & " Number: " & StarStruct.StarNumber & " " & StarStruct.RA & " " & StarStruct.Dec & " " & StarStruct.ProMoRA & " " & StarStruct.ProMoDec & " " & StarStruct.Parallax & " " & StarStruct.RadialVelocity)

        ZenithDistance = NOVAS.NOVAS2.Refract(LocationStruct, 2, 65.0)
        TL.LogMessage("Novas2Tests", "Refract (65deg) - Refracted Offset (Degrees): " & u.DegreesToDMS(ZenithDistance, ":", ":"))

        JD = NOVAS.NOVAS2.JulianDate(2009, 6, 7, 12.0)
        TL.LogMessage("Novas2Tests", "JulianDate - JD (6/7/2009 12:00): " & JD)

        NOVAS.NOVAS2.CalDate(JD, Year, Month, day, Hour)
        TL.LogMessage("Novas2Tests", "CalDate - " & day & " " & Month & " " & Year & " " & u.HoursToHMS(Hour))
        TL.BlankLine()

        Status("Novas2Static Tests")
        NOVAS2StaticTest(NOVAS2Functions.Abberation)
        NOVAS2StaticTest(NOVAS2Functions.App_Planet)
        NOVAS2StaticTest(NOVAS2Functions.App_Star)
        NOVAS2StaticTest(NOVAS2Functions.Astro_Planet)
        NOVAS2StaticTest(NOVAS2Functions.Astro_Star)
        NOVAS2StaticTest(NOVAS2Functions.Bary_To_Geo)
        NOVAS2StaticTest(NOVAS2Functions.Cal_Date)
        NOVAS2StaticTest(NOVAS2Functions.Cel_Pole)
        NOVAS2StaticTest(NOVAS2Functions.EarthTilt)
        NOVAS2StaticTest(NOVAS2Functions.Ephemeris)
        NOVAS2StaticTest(NOVAS2Functions.Equ2Hor)
        NOVAS2StaticTest(NOVAS2Functions.Fund_Args)
        NOVAS2StaticTest(NOVAS2Functions.Get_Earth)
        NOVAS2StaticTest(NOVAS2Functions.Julian_Date)
        NOVAS2StaticTest(NOVAS2Functions.Local_Planet)
        NOVAS2StaticTest(NOVAS2Functions.Local_Star)
        NOVAS2StaticTest(NOVAS2Functions.Make_Cat_Entry)
        NOVAS2StaticTest(NOVAS2Functions.Mean_Star)
        NOVAS2StaticTest(NOVAS2Functions.Nutate)
        NOVAS2StaticTest(NOVAS2Functions.Nutation_Angles)
        NOVAS2StaticTest(NOVAS2Functions.PNSW)
        NOVAS2StaticTest(NOVAS2Functions.Precession)
        NOVAS2StaticTest(NOVAS2Functions.Proper_Motion)
        NOVAS2StaticTest(NOVAS2Functions.RADEC2Vector)
        NOVAS2StaticTest(NOVAS2Functions.Refract)
        NOVAS2StaticTest(NOVAS2Functions.Set_Body)
        NOVAS2StaticTest(NOVAS2Functions.Sideral_Time)
        NOVAS2StaticTest(NOVAS2Functions.SolarSystem)
        NOVAS2StaticTest(NOVAS2Functions.Spin)
        NOVAS2StaticTest(NOVAS2Functions.StarVectors)
        NOVAS2StaticTest(NOVAS2Functions.Sun_Field)
        NOVAS2StaticTest(NOVAS2Functions.Tdb2Tdt)
        NOVAS2StaticTest(NOVAS2Functions.Terra)
        NOVAS2StaticTest(NOVAS2Functions.Topo_Planet)
        NOVAS2StaticTest(NOVAS2Functions.Topo_Star)
        NOVAS2StaticTest(NOVAS2Functions.Transform_Cat)
        NOVAS2StaticTest(NOVAS2Functions.Transform_Hip)
        NOVAS2StaticTest(NOVAS2Functions.Vector2RADEC)
        NOVAS2StaticTest(NOVAS2Functions.Virtual_Planet)
        NOVAS2StaticTest(NOVAS2Functions.Virtual_Star)
        NOVAS2StaticTest(NOVAS2Functions.Wobble)
        TL.BlankLine()
    End Sub

    Sub CheckRC(ByVal RC As Short, ByVal Section As String, ByVal Name As String)
        Compare(Section, Name & " Return Code", RC.ToString, "0")
    End Sub

    Sub NOVAS2StaticTest(ByVal TestFunction As NOVAS2Functions)
        Dim RA, DEC, Dis, POS(2), VEL(2), POS2(2), VEL2(2), EarthVector(2), LightTime, Hour, MObl, TObl, Eq, DPsi, DEps As Double
        Dim RadVel, JD, TDB, DeltaT, x, y, ZD, Az, rar, decr, a(4) As Double, SiteInfo As New ASCOM.Astrometry.SiteInfo
        Dim Gast, MRA, MDEC, LongNutation, ObliqueNutation, TdtJD, SecDiff, ST As Double
        Dim Year, Month, Day As Short
        Dim CatName(2) As Byte
        Dim rc As Integer
        Dim SS_Object, Earth As New ASCOM.Astrometry.BodyDescription, Star, NewCat As New ASCOM.Astrometry.CatEntry
        Dim Utl As New ASCOM.Utilities.Util
        Action(TestFunction.ToString)
        Earth.Number = Body.Earth
        Earth.Name = "Earth"
        Earth.Type = BodyType.MajorPlanet

        SS_Object.Name = "Sun"
        SS_Object.Number = Body.Sun
        SS_Object.Type = BodyType.Sun

        Star.Dec = 40.0
        Star.RA = 23
        Star.StarName = "Test Star"
        Star.Catalog = "HIP"

        SiteInfo.Height = 80.0
        SiteInfo.Latitude = 51.0
        SiteInfo.Longitude = 0.0
        SiteInfo.Pressure = 1020
        SiteInfo.Temperature = 25.0

        CatName(0) = Asc("P")
        CatName(1) = Asc("S")
        CatName(2) = Asc("1")

        rc = Integer.MaxValue 'Initialise to a silly value

        Try
            Select Case TestFunction
                Case NOVAS2Functions.Abberation
                    rc = NOVAS.NOVAS2.Aberration(POS, VEL, LightTime, POS2)
                Case NOVAS2Functions.App_Planet
                    rc = NOVAS.NOVAS2.AppPlanet(Utl.JulianDate, SS_Object, Earth, RA, DEC, Dis)
                Case NOVAS2Functions.App_Star
                    rc = NOVAS.NOVAS2.AppStar(Utl.JulianDate, Earth, Star, RA, DEC)
                Case NOVAS2Functions.Astro_Planet
                    rc = NOVAS.NOVAS2.AstroPlanet(Utl.JulianDate, SS_Object, Earth, RA, DEC, Dis)
                Case NOVAS2Functions.Astro_Star
                    rc = NOVAS.NOVAS2.AstroStar(Utl.JulianDate, Earth, Star, RA, DEC)
                Case NOVAS2Functions.Bary_To_Geo
                    NOVAS.NOVAS2.BaryToGeo(POS, EarthVector, POS2, LightTime)
                    rc = 0
                Case NOVAS2Functions.Cal_Date
                    NOVAS.NOVAS2.CalDate(Utl.JulianDate, Year, Month, Day, Hour)
                    rc = 0
                Case NOVAS2Functions.Cel_Pole
                    NOVAS.NOVAS2.CelPole(0.0, 0.0)
                    rc = 0
                Case NOVAS2Functions.EarthTilt
                    NOVAS.NOVAS2.EarthTilt(Utl.JulianDate, MObl, TObl, Eq, DPsi, DEps)
                    rc = 0
                Case NOVAS2Functions.Ephemeris
                    rc = NOVAS.NOVAS2.Ephemeris(Utl.JulianDate, SS_Object, Origin.Barycentric, POS, VEL)
                Case NOVAS2Functions.Equ2Hor
                    NOVAS.NOVAS2.Equ2Hor(Utl.JulianDate, DeltaT, x, y, SiteInfo, RA, DEC, RefractionOption.LocationRefraction, ZD, Az, rar, decr)
                    rc = 0
                Case NOVAS2Functions.Fund_Args
                    NOVAS.NOVAS2.FundArgs(0.1, a)
                    rc = 0
                Case NOVAS2Functions.Get_Earth
                    rc = NOVAS.NOVAS2.GetEarth(Utl.JulianDate, Earth, TDB, POS, VEL, POS2, VEL2)
                Case NOVAS2Functions.Julian_Date
                    JD = NOVAS.NOVAS2.JulianDate(2010, 9, 4, 5)
                    rc = 0
                Case NOVAS2Functions.Local_Planet
                    rc = NOVAS.NOVAS2.LocalPlanet(Utl.JulianDate, SS_Object, Earth, 0.0, SiteInfo, RA, DEC, Dis)
                Case NOVAS2Functions.Local_Star
                    rc = NOVAS.NOVAS2.LocalStar(Utl.JulianDate, Earth, 0.0, Star, SiteInfo, RA, DEC)
                Case NOVAS2Functions.Make_Cat_Entry
                    NOVAS.NOVAS2.MakeCatEntry("ABC", "TestStarName", 12345, 7.0, 65.0, 0.0, 0.0, 0.0, RadVel, Star)
                    rc = 0
                Case NOVAS2Functions.Mean_Star
                    rc = NOVAS.NOVAS2.MeanStar(Utl.JulianDate, Earth, RA, DEC, MRA, MDEC)
                Case NOVAS2Functions.Nutate
                    rc = NOVAS.NOVAS2.Nutate(Utl.JulianDate, NutationDirection.MeanToTrue, POS, POS2)
                Case NOVAS2Functions.Nutation_Angles
                    rc = NOVAS.NOVAS2.NutationAngles(TDB, LongNutation, ObliqueNutation)
                Case NOVAS2Functions.PNSW
                    NOVAS.NOVAS2.Pnsw(Utl.JulianDate, Gast, x, y, POS, POS2)
                    rc = 0
                Case NOVAS2Functions.Precession
                    NOVAS.NOVAS2.Precession(Utl.JulianDate, POS, Utl.JulianDate + 100.0, POS2)
                    rc = 0
                Case NOVAS2Functions.Proper_Motion
                    NOVAS.NOVAS2.ProperMotion(Utl.JulianDate, POS, VEL, Utl.JulianDate + 100.0, POS)
                    rc = 0
                Case NOVAS2Functions.RADEC2Vector
                    NOVAS.NOVAS2.RADec2Vector(RA, DEC, Dis, POS)
                    rc = 0
                Case NOVAS2Functions.Refract
                    ZD = NOVAS.NOVAS2.Refract(SiteInfo, 0, 75.0)
                    rc = 0
                Case NOVAS2Functions.Set_Body
                    rc = NOVAS.NOVAS2.SetBody(BodyType.MajorPlanet, Body.Moon, "Moon", SS_Object)
                Case NOVAS2Functions.Sideral_Time
                    NOVAS.NOVAS2.SiderealTime(Utl.JulianDate, 0.123, 65.0, Gast)
                    rc = 0
                Case NOVAS2Functions.SolarSystem
                    rc = NOVAS.NOVAS2.SolarSystem(Utl.JulianDate, Body.Earth, Origin.Barycentric, POS, VEL)
                Case NOVAS2Functions.Spin
                    NOVAS.NOVAS2.Spin(12.0, POS, POS2)
                    rc = 0
                Case NOVAS2Functions.StarVectors
                    NOVAS.NOVAS2.StarVectors(Star, POS, VEL)
                    rc = 0
                Case NOVAS2Functions.Sun_Field
                    rc = NOVAS.NOVAS2.SunField(POS, POS, POS2)
                Case NOVAS2Functions.Tdb2Tdt
                    NOVAS.NOVAS2.Tdb2Tdt(TDB, TdtJD, SecDiff)
                    rc = 0
                Case NOVAS2Functions.Terra
                    NOVAS.NOVAS2.Terra(SiteInfo, ST, POS, VEL)
                    rc = 0
                Case NOVAS2Functions.Topo_Planet
                    rc = NOVAS.NOVAS2.TopoPlanet(Utl.JulianDate, SS_Object, Earth, 0.0, SiteInfo, RA, DEC, Dis)
                Case NOVAS2Functions.Topo_Star
                    rc = NOVAS.NOVAS2.TopoStar(Utl.JulianDate, Earth, DeltaT, Star, SiteInfo, RA, DEC)
                Case NOVAS2Functions.Transform_Cat
                    NOVAS.NOVAS2.TransformCat(TransformationOption.ChangeEquatorAndEquinoxAndEpoch, Utl.JulianDate, Star, Utl.JulianDate + 1000, CatName, NewCat)
                    rc = 0
                Case NOVAS2Functions.Transform_Hip
                    NOVAS.NOVAS2.TransformHip(Star, NewCat)
                    rc = 0
                Case NOVAS2Functions.Vector2RADEC
                    NOVAS.NOVAS2.RADec2Vector(23.0, 50.0, 100000000, POS)
                    rc = NOVAS.NOVAS2.Vector2RADec(POS, RA, DEC)
                Case NOVAS2Functions.Virtual_Planet
                    rc = NOVAS.NOVAS2.VirtualPlanet(Utl.JulianDate, SS_Object, Earth, RA, DEC, Dis)
                Case NOVAS2Functions.Virtual_Star
                    rc = NOVAS.NOVAS2.VirtualStar(Utl.JulianDate, Earth, Star, RA, DEC)
                Case NOVAS2Functions.Wobble
                    NOVAS.NOVAS2.Wobble(x, y, POS, POS2)
                    rc = 0
                Case NOVAS2Functions.Sun_Eph
                    'Not in DLL  from Tim
                    NOVAS.NOVAS2.SunEph(Utl.JulianDate, RA, DEC, Dis)
                    rc = 0
            End Select
            Select Case rc
                Case 0
                    TL.LogMessage("Novas2Static", TestFunction.ToString & " - Passed")
                    NMatches += 1
                Case Integer.MaxValue
                    TL.LogMessage("Novas2Static", TestFunction.ToString & " - Test not implemented")
                Case Else
                    Dim Msg As String = "Non zero return code: " & rc
                    TL.LogMessage("Novas2Static", TestFunction.ToString & " - " & Msg)
                    NNonMatches += 1
                    ErrorList.Add("Novas2Static - " & TestFunction.ToString & ": " & Msg)
            End Select

            Utl.Dispose()
            Utl = Nothing
        Catch ex As Exception
            LogException("NOVAS2StaticTest", ex.ToString)
        End Try
    End Sub

    Sub TransformTest()
        TransformTest2000("Deneb", "20:41:25.916", "45:16:49.23", TOLERANCE_E5, TOLERANCE_E4)
        TransformTest2000("Polaris", "02:31:51.263", "89:15:50.68", TOLERANCE_E5, TOLERANCE_E4)
        TransformTest2000("Arcturus", "14:15:38.943", "19:10:37.93", TOLERANCE_E5, TOLERANCE_E4)
        TL.BlankLine()
    End Sub

    Sub TransformTest2000(ByVal Name As String, ByVal AstroRAString As String, ByVal AstroDECString As String, ByVal RATolerance As Double, DecTolerance As Double)
        Dim Util As New Util
        Dim AstroRA, AstroDEC As Double
        Dim SiteLat, SiteLong, SiteElev As Double
        Dim TR As Transform.Transform = New Transform.Transform
        Dim RA, DEC As Double
        Dim rc As Short
        Dim OnSurface3 As New OnSurface, Cat3 As New CatEntry3

        Try
            'RA and DEC
            AstroRA = Util.HMSToHours(AstroRAString)
            AstroDEC = Util.DMSToDegrees(AstroDECString)

            'Site parameters
            SiteElev = 80.0
            SiteLat = 51.0 + (4.0 / 60.0) + (43.0 / 3600.0)
            SiteLong = 0.0 - (17.0 / 60.0) - (40.0 / 3600.0)

            'Set up Transform component
            TR.SiteElevation = 80.0
            TR.SiteLatitude = SiteLat
            TR.SiteLongitude = SiteLong
            TR.SiteTemperature = 10.0
            TR.Refraction = False
            TR.SetJ2000(AstroRA, AstroDEC)

            Cat3.RA = AstroRA
            Cat3.Dec = AstroDEC
            OnSurface3.Height = SiteElev
            OnSurface3.Latitude = SiteLat
            OnSurface3.Longitude = SiteLong
            OnSurface3.Pressure = 1000.0
            OnSurface3.Temperature = 10.0

            rc = Nov31.TopoStar(AstroUtil.JulianDateTT(0.0), AstroUtil.DeltaT, Cat3, OnSurface3, Accuracy.Full, RA, DEC)
            TL.LogMessage("TransformTest", Name & " Novas31 RA/DEC Actual  : " & Util.HoursToHMS(TR.RATopocentric, ":", ":", "", 3) & " " & Util.DegreesToDMS(TR.DECTopocentric, ":", ":", "", 3))
            TL.LogMessage("TransformTest", Name & " Novas31 RA/DEC Expected: " & Util.HoursToHMS(RA, ":", ":", "", 3) & " " & Util.DegreesToDMS(DEC, ":", ":", "", 3))
            CompareDouble("TransformTest", Name & " Novas31 Topocentric RA", TR.RATopocentric, RA, RATolerance, DoubleDisplayAs.HoursMinutesSeconds)
            CompareDouble("TransformTest", Name & " Novas31 Topocentric Dec", TR.DECTopocentric, DEC, DecTolerance, DoubleDisplayAs.DegreesMinutesSeconds)

        Catch ex As Exception
            LogException("TransformTest2000 Exception", ex.ToString)
        End Try

    End Sub

    Sub NovasComTests()
        Dim JD As Double
        Dim EA As New ASCOM.Astrometry.NOVASCOM.Earth

        Dim Mercury() As Double = New Double() {-0.146477263357071, -0.739730529540394, -0.275237058490435,
                                                -0.146552680905756, -0.73971718813053, -0.275232768188589,
                                                -0.144373027430296, -0.740086172152297, -0.275392756115203,
                                                -0.146535954004373, -0.739695817952422, -0.27526565650813,
                                                -0.144631609584528, -0.740282847942203, -0.274694144671298}

        Dim Venus() As Double = New Double() {-0.372100971951828, -0.449343256389233, -0.154902566021356,
                                              -0.372134026409355, -0.449318563980132, -0.154894786229779,
                                              -0.370822518399929, -0.450260651717891, -0.155303914026952,
                                              -0.372117531588399, -0.449297391719315, -0.154927789061455,
                                              -0.370858459989012, -0.450324120624622, -0.154965842632926}

        Dim Earth() As Double = New Double() {-0.147896190667482, 0.892857938625284, 0.387075601638547,
                                             -0.0173032744107731, -0.00236387487195205, -0.00102513648587834,
                                             -0.143537477185852, 0.892578667572019, 0.386954522818712,
                                             -0.0173040812547209, -0.00235851019511069, -0.00102281061381548,
                                             2455560.875, 1.06091018181116, 23.437861319031,
                                             17.3476127157785, -0.0796612008211573, 23.4378391909196}

        Dim Mars() As Double = New Double() {0.693859781031977, -2.07097170353203, -0.942316778727103,
                                             0.693632762626122, -2.07103494950845, -0.942344911675691,
                                             0.699920307729267, -2.06926836488007, -0.941576391467848,
                                             0.693650157043109, -2.07101326327704, -0.942377215846966,
                                             0.694585522035482, -2.07602310877394, -0.930591370257697}

        Dim Jupiter() As Double = New Double() {5.05143975731352, -0.264744225667142, -0.237337980646129,
                                                5.05143226054377, -0.264839400889264, -0.237391349301542,
                                                5.05234611594448, -0.252028458651431, -0.231824986961043,
                                                5.05144816590343, -0.264820597160675, -0.237424161148292,
                                                5.05236200971188, -0.252009614615973, -0.231857781296635}

        Dim Saturn() As Double = New Double() {-9.26931711919579, -2.66882658902422, -0.715270438185988,
                                               -9.2693570741403, -2.66869165249896, -0.715256117911115,
                                               -9.26176606870019, -2.69218837105073, -0.725464045762982,
                                               -9.2693389177143, -2.66867733760923, -0.71528954188717,
                                               -9.26144908836291, -2.69455677562765, -0.720448748375586}

        Dim Uranus() As Double = New Double() {20.2306046509148, -0.944778087940209, -0.693874737147122,
                                                  20.2305809175823, -0.945147799559502, -0.694063183048263,
                                                  20.233665104794, -0.893841634639764, -0.671770710247389,
                                                  20.2305964586587, -0.945137337897977, -0.694095638580723,
                                                  20.2336806550414, -0.893831133568696, -0.671803148655406}

        Dim Neptune() As Double = New Double() {25.5771370144156, -15.409403535665, -6.96191248591339,
                                                25.5759958639324, -15.4109792947898, -6.96261684888268,
                                                25.6226500915255, -15.3460649228334, -6.93440511594961,
                                                25.5760116932439, -15.4109610670775, -6.96264327213014,
                                                25.6226659036666, -15.3460466550117, -6.93443152177256}

        Dim Pluto() As Double = New Double() {2.92990303317673, -31.0320730022551, -10.6309560278551,
                                              2.92658680141819, -31.0323439400104, -10.6310785882777,
                                              3.01698355034971, -31.0248119250537, -10.627792242015,
                                              2.92662685257385, -31.0323223789174, -10.6311050132883,
                                              2.99849040874885, -31.0401256627947, -10.5882115993866}

        Try
            Status("NovasCom Tests")
            'Create the Julian date corresponding to the arbitary test date
            JD = TestJulianDate()
            TL.LogMessage("NovasCom Tests", "Julian Date = " & JD & " = " & TEST_DATE)
            CompareDouble("NovasCom", "JulianDate", JD, 2455560.875, TOLERANCE_E9)

            Dim s As New NOVASCOM.Site
            s.Height = 80.0
            s.Latitude = 51.0
            s.Longitude = 0.0
            s.Pressure = 1000.0
            s.Temperature = 10.0
            Dim pv As New NOVASCOM.PositionVector
            pv.SetFromSite(s, 11.0)
            CompareDouble("NovasCom", "SetFromSite X", pv.x, -0.0000259698466733494, TOLERANCE_E9)
            CompareDouble("NovasCom", "SetFromSite Y", pv.y, 0.00000695859944368407, TOLERANCE_E9)
            CompareDouble("NovasCom", "SetFromSite Z", pv.z, 0.0000329791401243054, TOLERANCE_E9)
            CompareDouble("NovasCom", "SetFromSite LightTime", pv.LightTime, 0.000000245746690359414, TOLERANCE_E9)

            Dim st As New NOVASCOM.Star
            st.Set(9.0, 25.0, 0.0, 0.0, 0.0, 0.0)

            JD = TestJulianDate()
            pv = st.GetAstrometricPosition(JD)
            Dim AstroResults() As Double = New Double() {9.0, 25.0, 2062648062470.13, 0.0, 11912861640.6606, -1321861174769.38, 1321861174768.63, 871712738743.913,
                                                    9.0, 25.0, 0.0, 0.0, 0.0, 0.0, 0.0}
            ComparePosVec("NovasCom Astrometric", st, pv, AstroResults, False, TOLERANCE_E9)

            Dim TopoNoReractResults() As Double = New Double() {9.01140781883559, 24.9535152700125, 2062648062470.13, 14.2403113213804, 11912861640.6606, -1326304233902.68, 1318405625773.8, 870195790998.564,
                                        9.0, 25.0, 0.0, 0.0, 0.0, 0.0, 0.0}
            pv = st.GetTopocentricPosition(JD, s, False)
            ComparePosVec("NovasCom Topo/NoRefract", st, pv, TopoNoReractResults, True, TOLERANCE_E9)
            pv = st.GetTopocentricPosition(JD, s, True)

            Dim TopoReractResults() As Double = New Double() {9.01438008140491, 25.0016930437008, 2062648062470.13, 14.3031953401364, 11912861640.6606, -1326809918883.5, 1316857267239.29, 871767977436.204,
                                                              9.0, 25.0, 0.0, 0.0, 0.0, 0.0, 0.0}

            ComparePosVec("NovasCom Topo/Refract", st, pv, TopoReractResults, True, TOLERANCE_E9)

            NovasComTest("Mercury", Body.Mercury, JD, Mercury, TOLERANCE_E6) 'Test Neptune prediction
            NovasComTest("Venus", Body.Venus, JD, Venus, TOLERANCE_E7) 'Test Neptune prediction
            NovasComTest("Mars", Body.Mars, JD, Mars, TOLERANCE_E8) 'Test Neptune prediction
            NovasComTest("Jupiter", Body.Jupiter, JD, Jupiter, TOLERANCE_E8) 'Test Neptune prediction
            NovasComTest("Saturn", Body.Saturn, JD, Saturn, TOLERANCE_E9) 'Test Neptune prediction
            NovasComTest("Uranus", Body.Uranus, JD, Uranus, TOLERANCE_E9) ' Test Uranus prediction
            NovasComTest("Neptune", Body.Neptune, JD, Neptune, TOLERANCE_E9) 'Test Neptune prediction
            NovasComTest("Pluto", Body.Pluto, JD, Pluto, TOLERANCE_E9) 'Test Pluto Prediction

            Action("Earth")
            EA.SetForTime(JD) ' Test earth properties
            CompareDouble("NovasCom", "Earth BaryPos x", EA.BarycentricPosition.x, Earth(0), TOLERANCE_E9)
            CompareDouble("NovasCom", "Earth BaryPos y", EA.BarycentricPosition.y, Earth(1), TOLERANCE_E9)
            CompareDouble("NovasCom", "Earth BaryPos z", EA.BarycentricPosition.z, Earth(2), TOLERANCE_E9)
            CompareDouble("NovasCom", "Earth BaryVel x", EA.BarycentricVelocity.x, Earth(3), TOLERANCE_E9)
            CompareDouble("NovasCom", "Earth BaryVel y", EA.BarycentricVelocity.y, Earth(4), TOLERANCE_E9)
            CompareDouble("NovasCom", "Earth BaryVel z", EA.BarycentricVelocity.z, Earth(5), TOLERANCE_E9)
            CompareDouble("NovasCom", "Earth HeliPos x", EA.HeliocentricPosition.x, Earth(6), TOLERANCE_E9)
            CompareDouble("NovasCom", "Earth HeliPos y", EA.HeliocentricPosition.y, Earth(7), TOLERANCE_E9)
            CompareDouble("NovasCom", "Earth HeliPos z", EA.HeliocentricPosition.z, Earth(8), TOLERANCE_E9)
            CompareDouble("NovasCom", "Earth HeliVel x", EA.HeliocentricVelocity.x, Earth(9), TOLERANCE_E9)
            CompareDouble("NovasCom", "Earth HeliVel y", EA.HeliocentricVelocity.y, Earth(10), TOLERANCE_E9)
            CompareDouble("NovasCom", "Earth HeliVel z", EA.HeliocentricVelocity.z, Earth(11), TOLERANCE_E9)
            CompareDouble("NovasCom", "Barycentric Time", EA.BarycentricTime, Earth(12), TOLERANCE_E9)
            CompareDouble("NovasCom", "Equation Of Equinoxes", EA.EquationOfEquinoxes, Earth(13), TOLERANCE_E9)
            CompareDouble("NovasCom", "Mean Obliquity", EA.MeanObliquity, Earth(14), TOLERANCE_E9)
            CompareDouble("NovasCom", "Nutation in Longitude", EA.NutationInLongitude, Earth(15), TOLERANCE_E9)
            CompareDouble("NovasCom", "Nutation in Obliquity", EA.NutationInObliquity, Earth(16), TOLERANCE_E9)
            CompareDouble("NovasCom", "True Obliquity", EA.TrueObliquity, Earth(17), TOLERANCE_E9)

            TL.BlankLine()
        Catch ex As Exception
            LogException("NovasComTests Exception", ex.ToString)
        End Try
        Status("")
    End Sub

    Sub ComparePosVec(ByVal TestName As String, ByVal st As NOVASCOM.Star, ByVal pv As NOVASCOM.PositionVector, ByVal Results() As Double, ByVal TestAzEl As Boolean, ByVal Tolerance As Double)
        CompareDouble(TestName, "RA Pos", pv.RightAscension, Results(0), Tolerance)
        CompareDouble(TestName, "DEC Pos", pv.Declination, Results(1), Tolerance)
        CompareDouble(TestName, "Dist", pv.Distance, Results(2), Tolerance)
        If TestAzEl Then
            CompareDouble(TestName, "Elev", pv.Elevation, Results(3), Tolerance)
        End If
        CompareDouble(TestName, "LightT", pv.LightTime, Results(4), Tolerance)
        CompareDouble(TestName, "x", pv.x, Results(5), Tolerance)
        CompareDouble(TestName, "y", pv.y, Results(6), Tolerance)
        CompareDouble(TestName, "z", pv.z, Results(7), Tolerance)
        CompareDouble(TestName, "RA", st.RightAscension, Results(8), Tolerance)
        CompareDouble(TestName, "DEC", st.Declination, Results(9), Tolerance)
        CompareDouble(TestName, "Number", st.Number, Results(10), Tolerance)
        CompareDouble(TestName, "Parallax", st.Parallax, Results(11), Tolerance)
        CompareDouble(TestName, "PrMoDec", st.ProperMotionDec, Results(12), Tolerance)
        CompareDouble(TestName, "PrMoRA", st.ProperMotionRA, Results(13), Tolerance)
        CompareDouble(TestName, "RadVel", st.RadialVelocity, Results(14), Tolerance)
    End Sub

    Function TestJulianDate() As Double
        'Create the Julian date corresponding to the arbitary test date
        Dim Util As New ASCOM.Utilities.Util
        Dim JD As Double
        JD = Util.DateUTCToJulian(Date.ParseExact(TEST_DATE, "F", System.Globalization.CultureInfo.InvariantCulture))
        Util.Dispose()
        Util = Nothing
        Return JD
    End Function

    Sub NovasComTest(ByVal p_Name As String, ByVal p_Num As Double, ByVal JD As Double, ByVal Results() As Double, ByVal Tolerance As Double)
        Dim pl As New NOVASCOM.Planet
        Dim K, KE As New Kepler.Ephemeris

        Dim POSVECO(5) As Double
        Dim pv As NOVASCOM.PositionVector
        Dim site As New NOVASCOM.Site

        Try
            Action(p_Name)

            site.Height = 80
            site.Latitude = 51.0
            site.Longitude = 0.0
            site.Pressure = 1000
            site.Temperature = 10.0

            KE.BodyType = BodyType.MajorPlanet
            KE.Number = 3

            pl.Name = p_Name
            pl.Number = p_Num
            pl.Type = BodyType.MajorPlanet

            pv = pl.GetAstrometricPosition(JD)
            CompareDouble("NovasCom", p_Name & " Astro x", pv.x, Results(0), Tolerance)
            CompareDouble("NovasCom", p_Name & " Astro y", pv.y, Results(1), Tolerance)
            CompareDouble("NovasCom", p_Name & " Astro z", pv.z, Results(2), Tolerance)

            pv = pl.GetVirtualPosition(JD)
            CompareDouble("NovasCom", p_Name & " Virtual x", pv.x, Results(3), Tolerance)
            CompareDouble("NovasCom", p_Name & " Virtual y", pv.y, Results(4), Tolerance)
            CompareDouble("NovasCom", p_Name & " Virtual z", pv.z, Results(5), Tolerance)

            pv = pl.GetApparentPosition(JD)
            CompareDouble("NovasCom", p_Name & " Apparent x", pv.x, Results(6), Tolerance)
            CompareDouble("NovasCom", p_Name & " Apparent y", pv.y, Results(7), Tolerance)
            CompareDouble("NovasCom", p_Name & " Apparent z", pv.z, Results(8), Tolerance)

            pv = pl.GetLocalPosition(JD, site)
            CompareDouble("NovasCom", p_Name & " Local x", pv.x, Results(9), Tolerance)
            CompareDouble("NovasCom", p_Name & " Local y", pv.y, Results(10), Tolerance)
            CompareDouble("NovasCom", p_Name & " Local z", pv.z, Results(11), Tolerance)

            pv = pl.GetTopocentricPosition(JD, site, True)
            CompareDouble("NovasCom", p_Name & " Topo x", pv.x, Results(12), Tolerance)
            CompareDouble("NovasCom", p_Name & " Topo y", pv.y, Results(13), Tolerance)
            CompareDouble("NovasCom", p_Name & " Topo z", pv.z, Results(14), Tolerance)
            Action("")
        Catch ex As Exception
            LogException("NovasComTest Exception", ex.ToString)
        End Try
    End Sub

    Sub KeplerTests()
        Dim JD As Double
        Dim MercuryPosVecs(,) As Double = New Double(,) {{-0.273826054895093, -0.332907079792611, -0.149433886467295, 0.0168077277855921, -0.0131641564589086, -0.00877483629689174},
                                                 {0.341715100611224, -0.15606206441965, -0.118796704430727, 0.00818341889620433, 0.0231859105741514, 0.0115367662530341},
                                                 {-0.290111477510344, 0.152752021696643, 0.11167615364006, -0.0208610666648984, -0.0207283399022831, -0.00890975564191571},
                                                 {-0.0948016996541467, -0.407064938162915, -0.207618339106762, 0.0218998992953613, -0.00301004943316363, -0.00387841048587606},
                                                 {0.335104649167322, 0.0711444942030144, 0.00326561005720837, -0.0109228475729584, 0.0251353246085599, 0.0145593566074213}}
        Status("Kepler Tests")
        JD = TestJulianDate()
        KeplerTest("Mercury", Body.Mercury, JD, MercuryPosVecs, TOLERANCE_E9) 'Test Mercury position vectors
        TL.BlankLine()
    End Sub

    Sub KeplerTest(ByVal p_Name As String, ByVal p_KepNum As Body, ByVal JD As Double, ByVal Results(,) As Double, ByVal Tolerance As Double)
        Dim K As New Kepler.Ephemeris
        Dim POSVEC() As Double
        Dim u As New Util
        Dim JDIndex As Integer = 0

        Const STEPSIZE As Double = 1000.0

        For jdate As Double = -2.0 * STEPSIZE To +2.0 * STEPSIZE Step STEPSIZE
            K.BodyType = BodyType.MajorPlanet
            K.Number = p_KepNum
            K.Name = p_Name
            POSVEC = K.GetPositionAndVelocity(JD + jdate)
            For i = 0 To 5
                CompareDouble("Kepler", p_Name & " " & jdate & " PV(" & i & ")", POSVEC(i), Results(JDIndex, i), Tolerance)
            Next
            JDIndex += 1
        Next
    End Sub

    Private Sub TimerTests()
        Const RunTime As Double = 10.0 ' Test runtime in seconds
        Const TimerInterval As Integer = 3000 'Timer interval
        Dim ElapsedTime As Double, LastSecond As Integer = -1

        TL.LogMessage("TimerTests", "Started")
        Status("Timer tests")
        Try
            ASCOMTimer = New ASCOM.Utilities.Timer
            ASCOMTimer.Interval = TimerInterval
            ASCOMTimer.Enabled = True
            StartTime = Now
            sw.Reset() : sw.Start()
            NumberOfTicks = 0 'Initialise counter
            Do
                Application.DoEvents()
                ElapsedTime = Now.Subtract(StartTime).TotalSeconds
                If Math.Floor(ElapsedTime) <> LastSecond Then
                    TL.LogMessage("TimerTests", "Seconds - " & Math.Floor(ElapsedTime))
                    Action("Seconds - " & CInt(ElapsedTime) & " / " & CInt(RunTime))
                    LastSecond = Math.Floor(ElapsedTime)
                End If
            Loop Until Now.Subtract(StartTime).TotalSeconds > RunTime
            sw.Stop()
        Catch ex As Exception
            LogException("TimerTests Exception", ex.ToString)
        Finally
            Try : ASCOMTimer.Enabled = False : Catch : End Try
        End Try

        TL.LogMessage("TimerTests", "Finished")
        TL.BlankLine()
    End Sub

    Private Sub LogException(ByVal FailingModule As String, ByVal Msg As String)
        TL.LogMessageCrLf(FailingModule, "##### " & Msg)
        NExceptions += 1
        ErrorList.Add(FailingModule & " - " & Msg)
    End Sub

    Private Sub cnt_TickNet() Handles ASCOMTimer.Tick
        Dim Duration As Double

        Duration = Now.Subtract(StartTime).TotalSeconds
        NumberOfTicks += 1
        CompareDouble("TimerTests Tick ", "Tick", Duration, NumberOfTicks * 3.0, 0.1)
        'TL.LogMessage("TimerTests", "Fired Net - " & Now.Subtract(StartTime).Seconds & " seconds")
        '        Application.DoEvents()
    End Sub

    Sub ProfileTests()
        Dim RetVal As String = "", RetValProfileKey As New ASCOMProfile

        'Dim DrvHlpProf As Object
        Dim AscomUtlProf As ASCOM.Utilities.Profile
        Const TestScope As String = "Test Telescope"

        Dim keys, values As ArrayList
        Try
            Status("Profile tests")
            TL.LogMessage("ProfileTest", "Creating ASCOM.Utilities.Profile")
            sw.Reset() : sw.Start()
            AscomUtlProf = New ASCOM.Utilities.Profile
            sw.Stop()
            TL.LogMessage("ProfileTest", "ASCOM.Utilities.Profile Created OK in " & sw.ElapsedMilliseconds & " milliseconds")
            AscomUtlProf.DeviceType = "Telescope"

            Compare("ProfileTest", "DeviceType", AscomUtlProf.DeviceType, "Telescope")

            Try : AscomUtlProf.Unregister(TestScope) : Catch : End Try 'Esnure the test scope is not registered
            Compare("ProfileTest", "IsRegistered when not registered should be False", AscomUtlProf.IsRegistered(TestScope).ToString, "False")

            AscomUtlProf.Register(TestScope, "This is a test telescope")
            TL.LogMessage("ProfileTest", TestScope & " registered OK")
            Compare("ProfileTest", "IsRegistered when registered should be True", AscomUtlProf.IsRegistered(TestScope).ToString, "True")

            Compare("ProfileTest", "Get Default Value", "123456", AscomUtlProf.GetValue(TestScope, "Test Name Default", "", "123456"))
            Compare("ProfileTest", "Get Defaulted Value", "123456", AscomUtlProf.GetValue(TestScope, "Test Name Default"))

            Compare("ProfileTest", "Get Default Value SubKey", "123456", AscomUtlProf.GetValue(TestScope, "Test Name Default", "SubKeyDefault", "123456"))
            Compare("ProfileTest", "Get Defaulted Value SubKey", "123456", AscomUtlProf.GetValue(TestScope, "Test Name Default", "SubKeyDefault"))

            AscomUtlProf.WriteValue(TestScope, "Test Name", "Test Value")
            AscomUtlProf.WriteValue(TestScope, "Root Test Name", "Test Value in Root key")

            AscomUtlProf.WriteValue(TestScope, "Test Name", "Test Value SubKey 2", "SubKey1\SubKey2")
            AscomUtlProf.WriteValue(TestScope, "SubKey2 Test Name", "Test Value in SubKey 2", "SubKey1\SubKey2")
            AscomUtlProf.WriteValue(TestScope, "SubKey2 Test Name1", "Test Value in SubKey 2", "SubKey1\SubKey2")
            AscomUtlProf.WriteValue(TestScope, "SubKey2a Test Name2a", "Test Value in SubKey 2a", "SubKey1\SubKey2\SubKey2a")
            AscomUtlProf.WriteValue(TestScope, "SubKey2b Test Name2b", "Test Value in SubKey 2b", "SubKey1\SubKey2\SubKey2a\SubKey2b")
            AscomUtlProf.WriteValue(TestScope, "SubKey2c Test Name2c", "Test Value in SubKey 2c", "SubKey1\SubKey2\SubKey2c")
            AscomUtlProf.WriteValue(TestScope, "", "Null Key in SubKey2", "SubKey1\SubKey2")
            AscomUtlProf.CreateSubKey(TestScope, "SubKey2")
            AscomUtlProf.WriteValue(TestScope, "SubKey3 Test Name", "Test Value SubKey 3", "SubKey3")
            AscomUtlProf.WriteValue(TestScope, "SubKey4 Test Name", "Test Value SubKey 4", "SubKey4")
            Compare("ProfileTest", "GetValue", AscomUtlProf.GetValue(TestScope, "Test Name"), "Test Value")
            Compare("ProfileTest", "GetValue SubKey", AscomUtlProf.GetValue(TestScope, "Test Name", "SubKey1\SubKey2"), "Test Value SubKey 2")

            'Null value write test
            Try
                AscomUtlProf.WriteValue(TestScope, "Results 1", Nothing)
                Compare("ProfileTest", "Null value write test", """" & AscomUtlProf.GetValue(TestScope, "Results 1") & """", """""")
            Catch ex As Exception
                LogException("Null Value Write Test 1 Exception: ", ex.ToString)
            End Try
            TL.BlankLine()

            TL.LogMessage("ProfileTest", "Testing Profile.SubKeys")
            keys = AscomUtlProf.SubKeys(TestScope, "")
            Compare("ProfileTest", "Create SubKey1", keys(0).Key.ToString & keys(0).Value.ToString, "SubKey1")
            Compare("ProfileTest", "Create SubKey2", keys(1).Key.ToString & keys(1).Value.ToString, "SubKey2")
            Compare("ProfileTest", "Create SubKey3", keys(2).Key.ToString & keys(2).Value.ToString, "SubKey3")
            Compare("ProfileTest", "Create SubKey4", keys(3).Key.ToString & keys(3).Value.ToString, "SubKey4")
            Compare("ProfileTest", "Create SubKeyDefault", keys(4).Key.ToString & keys(4).Value.ToString, "SubKeyDefault")
            Compare("ProfileTest", "SubKey Count", keys.Count.ToString, "5")
            TL.BlankLine()

            TL.LogMessage("ProfileTest", "Testing Profile.Values")
            values = AscomUtlProf.Values(TestScope, "SubKey1\SubKey2")
            Compare("ProfileTest", "SubKey1\SubKey2 Value 0", values(0).Key.ToString & " " & values(0).Value.ToString, " Null Key in SubKey2")
            Compare("ProfileTest", "SubKey1\SubKey2 Value 1", values(1).Key.ToString & " " & values(1).Value.ToString, "SubKey2 Test Name Test Value in SubKey 2")
            Compare("ProfileTest", "SubKey1\SubKey2 Value 2", values(2).Key.ToString & " " & values(2).Value.ToString, "SubKey2 Test Name1 Test Value in SubKey 2")
            Compare("ProfileTest", "SubKey1\SubKey2 Value 3", values(3).Key.ToString & " " & values(3).Value.ToString, "Test Name Test Value SubKey 2")
            Compare("ProfileTest", "SubKey1\SubKey2 Count", values.Count.ToString, "4")
            TL.BlankLine()

            TL.LogMessage("ProfileTest", "Testing Profile.DeleteSubKey - SubKey2")
            AscomUtlProf.DeleteSubKey(TestScope, "Subkey2")
            keys = AscomUtlProf.SubKeys(TestScope, "")
            Compare("ProfileTest", "Create SubKey1", keys(0).Key.ToString & keys(0).Value.ToString, "SubKey1")
            Compare("ProfileTest", "Create SubKey3", keys(1).Key.ToString & keys(1).Value.ToString, "SubKey3")
            Compare("ProfileTest", "Create SubKey4", keys(2).Key.ToString & keys(2).Value.ToString, "SubKey4")
            Compare("ProfileTest", "Create SubKeyDefault", keys(3).Key.ToString & keys(3).Value.ToString, "SubKeyDefault")
            Compare("ProfileTest", "SubKey Count", keys.Count.ToString, "4")
            TL.BlankLine()

            TL.LogMessage("ProfileTest", "Testing Profile.DeleteValue - SubKey1\SubKey2\Test Name")
            AscomUtlProf.DeleteValue(TestScope, "Test Name", "SubKey1\SubKey2")
            values = AscomUtlProf.Values(TestScope, "SubKey1\SubKey2")
            Compare("ProfileTest", "SubKey1\SubKey2 Value 0", values(0).Key.ToString & " " & values(0).Value.ToString, " Null Key in SubKey2")
            Compare("ProfileTest", "SubKey1\SubKey2 Value 1", values(1).Key.ToString & " " & values(1).Value.ToString, "SubKey2 Test Name Test Value in SubKey 2")
            Compare("ProfileTest", "SubKey1\SubKey2 Value 2", values(2).Key.ToString & " " & values(2).Value.ToString, "SubKey2 Test Name1 Test Value in SubKey 2")
            Compare("ProfileTest", "SubKey1\SubKey2 Count", values.Count.ToString, "3")
            TL.BlankLine()

            TL.LogMessage("ProfileTest", "Bulk Profile operation tests")
            Try
                Compare("ProfileTest", "XML Read", AscomUtlProf.GetProfileXML(TestScope), XMLTestString)
            Catch ex As Exception
                LogException("GetProfileXML", ex.ToString)
            End Try

            Try
                RetVal = AscomUtlProf.GetProfileXML(TestScope)
                RetVal = RetVal.Replace(TestTelescopeDescription, RevisedTestTelescopeDescription)
                AscomUtlProf.SetProfileXML(TestScope, RetVal)
                Compare("ProfileTest", "XML Write", AscomUtlProf.GetValue(TestScope, ""), RevisedTestTelescopeDescription)
            Catch ex As Exception
                LogException("SetProfileXML", ex.ToString)
            End Try

            Try
                RetValProfileKey = AscomUtlProf.GetProfile(TestScope)
                For Each subkey As String In RetValProfileKey.ProfileValues.Keys
                    'TL.LogMessage("SetProfileXML", "Found: " & subkey)
                    For Each valuename As String In RetValProfileKey.ProfileValues.Item(subkey).Keys
                        'TL.LogMessage("SetProfileXML", "Found Value: " & valuename & " = " & RetValProfileKey.ProfileValues.Item(subkey).Item(valuename))
                    Next
                Next
                Compare("ProfileTest", "ASCOMProfile Read", RetValProfileKey.ProfileValues.Item("SubKey1\SubKey2\SubKey2c").Item("SubKey2c Test Name2c"), "Test Value in SubKey 2c")
                RetValProfileKey.SetValue("", NewTestTelescopeDescription)
                RetValProfileKey.SetValue("NewName", "New value")

                RetValProfileKey.SetValue("NewName 2", "New value 2", "\New Subkey 2")
                RetValProfileKey.SetValue("Newname 3", "New value 3", "New Subkey 3")
                AscomUtlProf.SetProfile(TestScope, RetValProfileKey)

                Compare("ProfileTest", "ASCOMProfile Write", AscomUtlProf.GetValue(TestScope, ""), NewTestTelescopeDescription)
                Compare("ProfileTest", "ASCOMProfile Write", AscomUtlProf.GetValue(TestScope, "NewName"), "New value")
                Compare("ProfileTest", "ASCOMProfile Write", AscomUtlProf.GetValue(TestScope, "NewName 2", "\New Subkey 2"), "New value 2")
                Compare("ProfileTest", "ASCOMProfile Write", AscomUtlProf.GetValue(TestScope, "NewName 3", "New Subkey 3"), "New value 3")

                TL.BlankLine()
            Catch ex As Exception
                LogException("SetProfile", ex.ToString)
            End Try

            'Registered device types test
            Dim DevTypes As ArrayList
            Try
                DevTypes = AscomUtlProf.RegisteredDeviceTypes
                TL.LogMessage("ProfileTest", "DeviceTypes - found " & DevTypes.Count & " device types")
                For Each DeviceType As String In DevTypes
                    TL.LogMessage("ProfileTest", "DeviceTypes - " & DeviceType)
                Next
                Compare("ProfileTest", "DeviceTypes Camera", DevTypes.Contains("Camera").ToString, "True")
                Compare("ProfileTest", "DeviceTypes Dome", DevTypes.Contains("Dome").ToString, "True")
                Compare("ProfileTest", "DeviceTypes FilterWheel", DevTypes.Contains("FilterWheel").ToString, "True")
                Compare("ProfileTest", "DeviceTypes Focuser", DevTypes.Contains("Focuser").ToString, "True")
                Compare("ProfileTest", "DeviceTypes Rotator", DevTypes.Contains("Rotator").ToString, "True")
                Compare("ProfileTest", "DeviceTypes SafetyMonitor", DevTypes.Contains("SafetyMonitor").ToString, "True")
                Compare("ProfileTest", "DeviceTypes Switch", DevTypes.Contains("Switch").ToString, "True")
                Compare("ProfileTest", "DeviceTypes Telescope", DevTypes.Contains("Telescope").ToString, "True")
                TL.BlankLine()
            Catch ex As Exception
                LogException("RegisteredDeviceTypes", ex.ToString)
            End Try

            'Registered devices tests
            Try
                TL.LogMessage("ProfileTest", "Installed Simulator Devices")
                keys = AscomUtlProf.RegisteredDevices("Camera")
                CheckSimulator(keys, "Camera", "ASCOM.Simulator.Camera")
                CheckSimulator(keys, "Camera", "CCDSimulator.Camera")
                keys = AscomUtlProf.RegisteredDevices("Dome")
                CheckSimulator(keys, "Dome", "DomeSim.Dome")
                CheckSimulator(keys, "Dome", "Hub.Dome")
                CheckSimulator(keys, "Dome", "Pipe.Dome")
                CheckSimulator(keys, "Dome", "POTH.Dome")
                keys = AscomUtlProf.RegisteredDevices("FilterWheel")
                CheckSimulator(keys, "FilterWheel", "ASCOM.Simulator.FilterWheel")
                CheckSimulator(keys, "FilterWheel", "FilterWheelSim.FilterWheel")
                keys = AscomUtlProf.RegisteredDevices("Focuser")
                CheckSimulator(keys, "Focuser", "FocusSim.Focuser")
                CheckSimulator(keys, "Focuser", "Hub.Focuser")
                CheckSimulator(keys, "Focuser", "Pipe.Focuser")
                CheckSimulator(keys, "Focuser", "POTH.Focuser")
                keys = AscomUtlProf.RegisteredDevices("Rotator")
                CheckSimulator(keys, "Rotator", "ASCOM.Simulator.Rotator")
                keys = AscomUtlProf.RegisteredDevices("SafetyMonitor")
                CheckSimulator(keys, "SafetyMonitor", "ASCOM.Simulator.SafetyMonitor")
                keys = AscomUtlProf.RegisteredDevices("Switch")
                CheckSimulator(keys, "Switch", "SwitchSim.Switch")
                keys = AscomUtlProf.RegisteredDevices("Telescope")
                CheckSimulator(keys, "Telescope", "ASCOM.Simulator.Telescope")
                CheckSimulator(keys, "Telescope", "ASCOMDome.Telescope")
                CheckSimulator(keys, "Telescope", "Hub.Telescope")
                CheckSimulator(keys, "Telescope", "Pipe.Telescope")
                CheckSimulator(keys, "Telescope", "POTH.Telescope")
                CheckSimulator(keys, "Telescope", "ScopeSim.Telescope")

                DevTypes = AscomUtlProf.RegisteredDeviceTypes
                For Each DevType As String In DevTypes
                    'TL.LogMessage("RegisteredDevices", "Found " & DevType)
                    keys = AscomUtlProf.RegisteredDevices(DevType)
                    For Each kvp As KeyValuePair In keys
                        'TL.LogMessage("RegisteredDevices", "  " & kvp.Key & " - " & kvp.Value)
                    Next
                Next
            Catch ex As Exception
                LogException("RegisteredDevices", ex.ToString)
            End Try

            'Empty string
            Try
                keys = AscomUtlProf.RegisteredDevices("")
                TL.LogMessage("RegisteredDevices EmptyString", "Found " & keys.Count & " devices")
                For Each kvp As KeyValuePair(Of String, String) In keys
                    TL.LogMessage("RegisteredDevices EmptyString", "  " & kvp.Key & " - " & kvp.Value)
                Next
            Catch ex As ASCOM.Utilities.Exceptions.InvalidValueException
                Compare("ProfileTest", "RegisteredDevices with an empty string", "InvalidValueException", "InvalidValueException")
            Catch ex As Exception
                LogException("RegisteredDevices EmptyString", ex.ToString)
            End Try

            'Nothing
            Try
                keys = AscomUtlProf.RegisteredDevices(Nothing)
                TL.LogMessage("RegisteredDevices Nothing", "Found " & keys.Count & " devices")
                For Each kvp As KeyValuePair(Of String, String) In keys
                    TL.LogMessage("RegisteredDevices Nothing", "  " & kvp.Key & " - " & kvp.Value)
                Next
            Catch ex As ASCOM.Utilities.Exceptions.InvalidValueException
                Compare("ProfileTest", "RegisteredDevices with a null value", "InvalidValueException", "InvalidValueException")
            Catch ex As Exception
                LogException("RegisteredDevices Nothing", ex.ToString)
            End Try

            'Bad value
            Try
                keys = AscomUtlProf.RegisteredDevices("asdwer vbn tyu")
                Compare("ProfileTest", "RegisteredDevices with an Unknown DeviceType", keys.Count.ToString, "0")
                For Each kvp As KeyValuePair(Of String, String) In keys
                    TL.LogMessage("RegisteredDevices Bad", "  " & kvp.Key & " - " & kvp.Value)
                Next
            Catch ex As ASCOM.Utilities.Exceptions.InvalidValueException
                LogException("ProfileTest", "RegisteredDevices Unknown DeviceType incorrectly generated an InvalidValueException")
            Catch ex As Exception
                LogException("RegisteredDevices Bad", ex.ToString)
            End Try
            TL.BlankLine()

            Status("Profile performance tests")
            'Timing tests
            sw.Reset() : sw.Start()
            For i = 1 To 100
                AscomUtlProf.WriteValue(TestScope, "Test Name " & i.ToString, "Test Value")
            Next
            sw.Stop() : TL.LogMessage("ProfilePerformance", "Writevalue : " & sw.ElapsedMilliseconds / 100 & " milliseconds")

            sw.Reset() : sw.Start()
            For i = 1 To 100
                keys = AscomUtlProf.SubKeys(TestScope, "")
            Next
            sw.Stop() : TL.LogMessage("ProfilePerformance", "SubKeys : " & sw.ElapsedMilliseconds / 100 & " milliseconds")

            sw.Reset() : sw.Start()
            For i = 1 To 100
                keys = AscomUtlProf.Values(TestScope, "SubKey1\SubKey2")
            Next
            sw.Stop() : TL.LogMessage("ProfilePerformance", "Values : " & sw.ElapsedMilliseconds / 100 & " milliseconds")

            sw.Reset() : sw.Start()
            For i = 1 To 100
                RetVal = AscomUtlProf.GetValue(TestScope, "Test Name " & i.ToString)
            Next
            sw.Stop() : TL.LogMessage("ProfilePerformance", "GetValue : " & sw.ElapsedMilliseconds / 100 & " milliseconds")

            sw.Reset()
            For i = 1 To 100
                AscomUtlProf.WriteValue(TestScope, "Test Name", "Test Value SubKey 2", "SubKey1\SubKey2")
                sw.Start()
                AscomUtlProf.DeleteValue(TestScope, "Test Name", "SubKey1\SubKey2")
                sw.Stop()
            Next
            TL.LogMessage("ProfilePerformance", "Delete : " & sw.ElapsedMilliseconds / 100 & " milliseconds")

            sw.Reset() : sw.Start()
            For i = 1 To 100
                RetVal = AscomUtlProf.GetProfileXML(TestScope)
            Next
            sw.Stop() : TL.LogMessage("ProfilePerformance", "GetProfileXML : " & sw.ElapsedMilliseconds / 100 & " milliseconds")

            sw.Reset() : sw.Start()
            For i = 1 To 100
                RetVal = AscomUtlProf.GetProfileXML("ScopeSim.Telescope")
            Next
            sw.Stop() : TL.LogMessage("ProfilePerformance", "GetProfileXML : " & sw.ElapsedMilliseconds / 100 & " milliseconds")

            sw.Reset() : sw.Start()
            For i = 1 To 100
                RetValProfileKey = AscomUtlProf.GetProfile(TestScope)
            Next
            sw.Stop() : TL.LogMessage("ProfilePerformance", "GetProfile : " & sw.ElapsedMilliseconds / 100 & " milliseconds")

            sw.Reset() : sw.Start()
            For i = 1 To 100
                RetValProfileKey = AscomUtlProf.GetProfile("ScopeSim.Telescope")
            Next
            sw.Stop() : TL.LogMessage("ProfilePerformance", "GetProfile : " & sw.ElapsedMilliseconds / 100 & " milliseconds")

            sw.Reset() : sw.Start()
            For i = 1 To 100
                AscomUtlProf.SetProfile("ScopeSim.Telescope", RetValProfileKey)
            Next
            sw.Stop() : TL.LogMessage("ProfilePerformance", "SetProfile : " & sw.ElapsedMilliseconds / 100 & " milliseconds")

            sw.Reset() : sw.Start()
            For i = 1 To 100
                AscomUtlProf.SetProfileXML("ScopeSim.Telescope", RetVal)
            Next
            sw.Stop() : TL.LogMessage("ProfilePerformance", "SetProfileXML : " & sw.ElapsedMilliseconds / 100 & " milliseconds")

            TL.BlankLine()

            AscomUtlProf.Unregister(TestScope)
            Compare("ProfileTest", "Test telescope registered after unregister", AscomUtlProf.IsRegistered(TestScope), "False")

            AscomUtlProf.Dispose()
            TL.LogMessage("ProfileTest", "Profile Disposed OK")
            AscomUtlProf = Nothing
            TL.BlankLine()

            Status("Profile multi-tasking tests")

            Dim P1, P2 As New Profile, R1, R2 As String

            P1.Register(TestScope, "Multi access tester")

            P1.WriteValue(TestScope, TestScope, "1")
            R1 = P1.GetValue(TestScope, TestScope)
            R2 = P2.GetValue(TestScope, TestScope)
            Compare("ProfileMultiAccess", "MultiAccess", R1, R2)

            P1.WriteValue(TestScope, TestScope, "2")
            R1 = P1.GetValue(TestScope, TestScope)
            R2 = P2.GetValue(TestScope, TestScope)
            Compare("ProfileMultiAccess", "MultiAccess", R1, R2)

            P2.Dispose()
            P1.Dispose()

            'Multiple writes to the same value - single threaded
            TL.LogMessage("ProfileMultiAccess", "MultiWrite - SingleThread Started")
            Action("MultiWrite - SingleThread Started")
            Try
                Dim P(100) As Profile
                For i = 1 To 100
                    P(i) = New Profile
                    P(i).WriteValue(TestScope, TestScope, "27")
                Next
            Catch ex As Exception
                LogException("MultiWrite - SingleThread", ex.ToString)
            End Try

            TL.LogMessage("ProfileMultiAccess", "MultiWrite - SingleThread Finished")

            TL.LogMessage("ProfileMultiAccess", "MultiWrite - MultiThread Started")
            Action("MultiWrite - MultiThread Started")
            'Multiple writes -multi-threaded
            Const NThreads As Integer = 3

            Dim ProfileThreads(NThreads) As Thread
            For i = 0 To NThreads
                ProfileThreads(i) = New Thread(AddressOf ProfileThread)
                ProfileThreads(i).Start(i)
            Next
            For i = 0 To NThreads
                ProfileThreads(i).Join()
            Next

            P1 = New Profile
            P1.Unregister(TestScope)

            TL.LogMessage("ProfileMultiAccess", "MultiWrite - MultiThread Finished")
            TL.BlankLine()
            TL.BlankLine()

        Catch ex As Exception
            LogException("Exception", ex.ToString)
        End Try

    End Sub

    Sub CheckSimulator(ByVal Devices As ArrayList, ByVal DeviceType As String, ByVal DeviceName As String)
        Dim Found As Boolean = False
        For Each Device In Devices
            If Device.Key = DeviceName Then Found = True
        Next

        If Found Then
            Compare("ProfileTest", DeviceType, DeviceName, DeviceName)
        Else
            Compare("ProfileTest", DeviceType, "", DeviceName)
        End If
    End Sub

    Sub ProfileThread(ByVal inst As Integer)
        Dim TL As New TraceLogger("", "ProfileTrace " & inst.ToString)
        Const ts As String = "Test Telescope"
        TL.Enabled = True
        'TL.LogMessage("MultiWrite - MultiThread", "ThreadStart")
        TL.LogMessage("Started", "")
        Try
            Dim P(100) As Profile
            For i = 1 To 100
                P(i) = New Profile
                P(i).WriteValue(ts, ts, i.ToString)
                TL.LogMessage("Written", i.ToString)
                P(i).Dispose()
            Next
        Catch ex As Exception
            LogException("MultiWrite - MultiThread", ex.ToString)
        End Try
        'TL.LogMessage("MultiWrite - MultiThread", "ThreadEnd")
        TL.Enabled = False
        TL.Dispose()
        TL = Nothing

    End Sub

    Private Sub Compare(ByVal p_Section As String, ByVal p_Name As String, ByVal p_New As String, ByVal p_Orig As String)
        Dim ErrMsg As String
        If p_New = p_Orig Then
            If p_New.Length > 200 Then p_New = p_New.Substring(1, 200) & "..."
            TL.LogMessage(p_Section, "Matched " & p_Name & " = " & p_New)
            NMatches += 1
        Else
            ErrMsg = "##### NOT Matched - " & p_Name & " - Received: """ & p_New & """, Expected: """ & p_Orig & """"
            TL.LogMessageCrLf(p_Section, ErrMsg)
            NNonMatches += 1
            ErrorList.Add(p_Section & " - " & ErrMsg)
        End If
    End Sub

    Private Sub CompareBoolean(ByVal p_Section As String, ByVal p_Name As String, ByVal p_New As Boolean, ByVal p_Orig As Boolean)
        Dim ErrMsg As String
        If p_New = p_Orig Then
            TL.LogMessage(p_Section, "Matched " & p_Name & " = " & p_New)
            NMatches += 1
        Else
            ErrMsg = "##### NOT Matched - " & p_Name & " - Received: """ & p_New & """, Expected: """ & p_Orig & """"
            TL.LogMessageCrLf(p_Section, ErrMsg)
            NNonMatches += 1
            ErrorList.Add(p_Section & " - " & ErrMsg)
        End If
    End Sub

    Private Sub CompareLongInteger(ByVal p_Section As String, ByVal p_Name As String, ByVal p_New As Int64, ByVal p_Orig As Int64)
        Dim ErrMsg As String
        If p_New = p_Orig Then
            TL.LogMessage(p_Section, "Matched " & p_Name & " = " & p_New)
            NMatches += 1
        Else
            ErrMsg = "##### NOT Matched - " & p_Name & " - Received: """ & p_New & """, Expected: """ & p_Orig & """"
            TL.LogMessageCrLf(p_Section, ErrMsg)
            NNonMatches += 1
            ErrorList.Add(p_Section & " - " & ErrMsg)
        End If
    End Sub

    Private Sub CompareInteger(ByVal p_Section As String, ByVal p_Name As String, ByVal p_New As Integer, ByVal p_Orig As Integer)
        Dim ErrMsg As String
        If p_New = p_Orig Then
            TL.LogMessage(p_Section, "Matched " & p_Name & " = " & p_New)
            NMatches += 1
        Else
            ErrMsg = "##### NOT Matched - " & p_Name & " - Received: """ & p_New & """, Expected: """ & p_Orig & """"
            TL.LogMessageCrLf(p_Section, ErrMsg)
            NNonMatches += 1
            ErrorList.Add(p_Section & " - " & ErrMsg)
        End If
    End Sub

    Private Enum DoubleDisplayAs
        Number
        HoursMinutesSeconds
        DegreesMinutesSeconds
    End Enum

    Private Sub CompareDouble(ByVal p_Section As String, ByVal p_Name As String, ByVal p_New As Double, ByVal p_Orig As Double, ByVal p_Tolerance As Double)
        CompareDouble(p_Section, p_Name, p_New, p_Orig, p_Tolerance, DoubleDisplayAs.Number)
    End Sub

    Private Sub CompareDouble(ByVal p_Section As String, ByVal p_Name As String, ByVal p_New As Double, ByVal p_Orig As Double, ByVal p_Tolerance As Double, p_DisplayAs As DoubleDisplayAs)
        Dim ErrMsg As String, Divisor As Double
        Dim DisplayNew, DisplayOriginal, DisplayTolerance As String

        Divisor = p_Orig
        If Divisor = 0.0 Then Divisor = 1.0 'Deal withpossible divide by zero error

        If System.Math.Abs((p_New - p_Orig) / Divisor) < p_Tolerance Then
            Select Case p_DisplayAs
                Case DoubleDisplayAs.DegreesMinutesSeconds
                    DisplayNew = AscomUtil.DegreesToDMS(p_New, ":", ":", "", 3)
                    DisplayTolerance = AscomUtil.DegreesToDMS(p_Tolerance, ":", ":", "", 3)
                Case DoubleDisplayAs.HoursMinutesSeconds
                    DisplayNew = AscomUtil.HoursToHMS(p_New, ":", ":", "", 3)
                    DisplayTolerance = AscomUtil.HoursToHMS(p_Tolerance, ":", ":", "", 3)
                Case DoubleDisplayAs.Number
                    DisplayNew = p_New.ToString
                    DisplayTolerance = p_Tolerance.ToString("0.0E0")
                Case Else
                    ErrMsg = "The DoubleDisplayAs value: " & p_DisplayAs.ToString & " is not configured in Sub CompareDouble"
                    TL.LogMessage(p_Section, ErrMsg)
                    NNonMatches += 1
                    ErrorList.Add(p_Section & " - " & ErrMsg)
                    DisplayNew = p_New.ToString
                    DisplayTolerance = p_Tolerance.ToString
            End Select

            TL.LogMessage(p_Section, "Matched " & p_Name & " = " & DisplayNew & " within tolerance of " & DisplayTolerance)
            NMatches += 1
        Else

            Select Case p_DisplayAs
                Case DoubleDisplayAs.DegreesMinutesSeconds
                    DisplayNew = AscomUtil.DegreesToDMS(p_New, ":", ":", "", 3)
                    DisplayOriginal = AscomUtil.DegreesToDMS(p_Orig, ":", ":", "", 3)
                    DisplayTolerance = AscomUtil.DegreesToDMS(p_Tolerance, ":", ":", "", 3)
                Case DoubleDisplayAs.HoursMinutesSeconds
                    DisplayNew = AscomUtil.HoursToHMS(p_New, ":", ":", "", 3)
                    DisplayOriginal = AscomUtil.HoursToHMS(p_Orig, ":", ":", "", 3)
                    DisplayTolerance = AscomUtil.HoursToHMS(p_Tolerance, ":", ":", "", 3)
                Case DoubleDisplayAs.Number
                    DisplayNew = p_New.ToString
                    DisplayOriginal = p_Orig.ToString
                    DisplayTolerance = p_Tolerance.ToString
                Case Else
                    ErrMsg = "The DoubleDisplayAs value: " & p_DisplayAs.ToString & " is not configured in Sub CompareDouble"
                    TL.LogMessage(p_Section, ErrMsg)
                    NNonMatches += 1
                    ErrorList.Add(p_Section & " - " & ErrMsg)
                    DisplayNew = p_New.ToString
                    DisplayOriginal = p_Orig.ToString
                    DisplayTolerance = p_Tolerance.ToString
            End Select

            ErrMsg = "##### NOT Matched - " & p_Name & " - Received: " & DisplayNew & ", Expected: " & DisplayOriginal & " within tolerance of " & DisplayTolerance
            TL.LogMessage(p_Section, ErrMsg)
            NNonMatches += 1
            ErrorList.Add(p_Section & " - " & ErrMsg)
        End If
    End Sub

    Private Sub CompareWithin(p_Section As String, p_Name As String, p_Value As Double, p_LowerBound As Double, p_UpperBound As Double)
        Dim ErrMsg As String
        If (p_Value >= p_LowerBound) And (p_Value <= p_UpperBound) Then
            TL.LogMessage(p_Section, "Matched " & p_Name & " value: " & p_Value & " is within the range: " & p_LowerBound & " to " & p_UpperBound)
            NMatches += 1
        Else
            ErrMsg = "##### NOT Matched - " & p_Name & " value: " & p_Value & " is outside the range: " & p_LowerBound & " to " & p_UpperBound
            NMatches += 1
            TL.LogMessageCrLf(p_Section, ErrMsg)
            NNonMatches += 1
            ErrorList.Add(p_Section & " - " & ErrMsg)
        End If
    End Sub

    Private Sub VideoUtilsTests()
        Dim NH As Video.NativeHelpers
        Dim rc As Integer
        Dim frame(100, 100) As Integer
        Dim frameOut(100, 100) As Integer
        Dim frameOut2(100, 100) As Integer
        Dim codec As UInteger
        Dim bitmap As New Bitmap(100, 100, Imaging.PixelFormat.Format24bppRgb)
        Dim byteArray() As Byte
        Dim frameColour(100, 100, 3) As Integer

        Try
            TL.LogMessage("VideoUtilsTests", "Creating NativeHelpers RC")
            NH = New NativeHelpers

            InitFrame2D(frame) ' Iniitalise the frame array
            CompareLongInteger("VideoUtilsTests", "InitFrame CheckSum", CheckSum2DFrame(frame), 111088890000)

            TL.LogMessage("VideoUtilsTests", "Opening video file RC")
            rc = NH.CreateNewAviFile("TestAVIFile.avi", 100, 100, 8, 25, False)
            CompareInteger("VideoUtilsTests", "CreateNewAviFile", rc, 0)
            rc = NH.InitFrameIntegration(100, 100, 8)
            CompareInteger("VideoUtilsTests", "InitFrameIntegration RC", rc, 0)
            rc = NH.AddFrameForIntegration(frame)
            CompareInteger("VideoUtilsTests", "AddFrameForIntegration RC", rc, 0)
            rc = NH.AddFrameForIntegration(frame)
            CompareInteger("VideoUtilsTests", "AddFrameForIntegration RC", rc, 0)
            rc = NH.GetResultingIntegratedFrame(frameOut)
            CompareInteger("VideoUtilsTests", "GetResultingIntegratedFrame RC", rc, 0)
            CompareLongInteger("VideoUtilsTests", "GetResultingIntegratedFrame CheckSum", CheckSum2DFrame(frameOut), 6366267148)

            rc = NH.SetWhiteBalance(50)
            CompareInteger("VideoUtilsTests", "SetWhiteBalance RC", rc, 0)
            rc = NH.SetGamma(0.45)
            CompareInteger("VideoUtilsTests", "SetGamma RC", rc, 0)
            rc = NH.ApplyGammaBrightness(100, 100, 8, frameOut, frameOut2, 42)
            CompareInteger("VideoUtilsTests", "ApplyGammaBrightness RC", rc, 0)
            rc = NH.AviFileAddFrame(frameOut2)
            CompareInteger("VideoUtilsTests", "AviFileAddFrame RC", rc, 0)
            codec = NH.GetUsedAviCompression()
            CompareInteger("VideoUtilsTests", "GetUsedAviCompression Codec", codec, 0)
            rc = NH.AviFileClose
            CompareInteger("VideoUtilsTests", "AviFileClose RC", rc, 0)

            byteArray = NH.GetBitmapBytes(bitmap)
            CompareInteger("VideoUtilsTests", "GetBitmapBytes Length", byteArray.Length, 30055)
            CompareLongInteger("VideoUtilsTests", "GetBitmapBytes CheckSum", CheckSumByteArray(byteArray), 13006)

            Dim hBitmap As IntPtr = bitmap.GetHbitmap()
            ClearByteArray(byteArray)
            CompareInteger("VideoUtilsTests", "GetBitmapBytes Clear", CheckSumByteArray(byteArray), 0)
            rc = NH.GetBitmapBytes(100, 100, hBitmap, byteArray)
            CompareInteger("VideoUtilsTests", "GetBitmapBytes RC", rc, 0)
            CompareInteger("VideoUtilsTests", "GetBitmapBytes Length", byteArray.Length, 30055)
            CompareLongInteger("VideoUtilsTests", "GetBitmapBytes CheckSum", CheckSumByteArray(byteArray), 13006)

            rc = NH.GetBitmapPixels(100, 100, 8, FlipMode.None, frame, byteArray)
            CompareInteger("VideoUtilsTests", "GetBitmapPixels Monochrome FlipMode.None RC", rc, 0)
            CompareLongInteger("VideoUtilsTests", "GetBitmapBytes CheckSum", CheckSumByteArray(byteArray), 53216891806)

            rc = NH.GetBitmapPixels(100, 100, 8, FlipMode.FlipHorizontally, frame, byteArray)
            CompareInteger("VideoUtilsTests", "GetBitmapPixels Monochrome FlipMode.FlipHorizontally RC", rc, 0)
            CompareLongInteger("VideoUtilsTests", "GetBitmapBytes CheckSum", CheckSumByteArray(byteArray), 53218149598)

            rc = NH.GetBitmapPixels(100, 100, 8, FlipMode.FlipVertically, frame, byteArray)
            CompareInteger("VideoUtilsTests", "GetBitmapPixels Monochrome FlipMode.FlipHorizontally RC", rc, 0)
            CompareLongInteger("VideoUtilsTests", "GetBitmapBytes CheckSum", CheckSumByteArray(byteArray), 53216912219)

            rc = NH.GetBitmapPixels(100, 100, 8, FlipMode.FlipBoth, frame, byteArray)
            CompareInteger("VideoUtilsTests", "GetBitmapPixels Monochrome FlipMode.FlipHorizontally RC", rc, 0)
            CompareLongInteger("VideoUtilsTests", "GetBitmapBytes CheckSum", CheckSumByteArray(byteArray), 53218170011)

            InitFrame3D(frameColour)
            CompareLongInteger("VideoUtilsTests", "InitFrame3D frameColour", CheckSum3DFrame(frameColour), 888711120000)

            rc = NH.GetColourBitmapPixels(100, 100, 8, FlipMode.None, frameColour, byteArray)
            CompareInteger("VideoUtilsTests", "GetBitmapPixels Colour FlipMode.None RC", rc, 0)
            CompareLongInteger("VideoUtilsTests", "GetBitmapPixels CheckSum", CheckSumByteArray(byteArray), 26404898591)

            rc = NH.GetColourBitmapPixels(100, 100, 8, FlipMode.FlipHorizontally, frameColour, byteArray)
            CompareInteger("VideoUtilsTests", "GetBitmapPixels Colour FlipMode.FlipHorizontally RC", rc, 0)
            CompareLongInteger("VideoUtilsTests", "GetBitmapPixels CheckSum", CheckSumByteArray(byteArray), 26412476543)

            rc = NH.GetColourBitmapPixels(100, 100, 8, FlipMode.FlipVertically, frameColour, byteArray)
            CompareInteger("VideoUtilsTests", "GetBitmapPixels Colour FlipMode.FlipVertically RC", rc, 0)
            CompareLongInteger("VideoUtilsTests", "GetBitmapPixels CheckSum", CheckSumByteArray(byteArray), 26404919004)

            rc = NH.GetColourBitmapPixels(100, 100, 8, FlipMode.FlipBoth, frameColour, byteArray)
            CompareInteger("VideoUtilsTests", "GetBitmapPixels Colour FlipMode.FlipBoth RC", rc, 0)
            CompareLongInteger("VideoUtilsTests", "GetBitmapPixels CheckSum", CheckSumByteArray(byteArray), 26412496956)

            InitBitMap(bitmap)

            frameOut2 = NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.R, FlipMode.None, byteArray)
            CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap R CheckSum", CheckSum2DFrame(frameOut2), 7554559)
            CompareLongInteger("VideoUtilsTests", "ByteArray R CheckSum", CheckSumByteArray(byteArray), 163723333)

            frameOut2 = NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.G, FlipMode.None, byteArray)
            CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap G CheckSum", CheckSum2DFrame(frameOut2), 14535572)
            CompareLongInteger("VideoUtilsTests", "ByteArray G CheckSum", CheckSumByteArray(byteArray), 294949228)

            frameOut2 = NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.B, FlipMode.None, byteArray)
            CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap B CheckSum", CheckSum2DFrame(frameOut2), 23493433)
            CompareLongInteger("VideoUtilsTests", "ByteArray B CheckSum", CheckSumByteArray(byteArray), 428153317)

            frameOut2 = NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.GrayScale, FlipMode.None, byteArray)
            CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap GrayScale CheckSum", CheckSum2DFrame(frameOut2), 16319054)
            CompareLongInteger("VideoUtilsTests", "ByteArray GrayScale CheckSum", CheckSumByteArray(byteArray), 317929171)

            frameOut2 = NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.R, FlipMode.FlipHorizontally, byteArray)
            CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap FlipHorizontally CheckSum", CheckSum2DFrame(frameOut2), 7057805)
            CompareLongInteger("VideoUtilsTests", "ByteArray FlipHorizontally CheckSum", CheckSumByteArray(byteArray), 163358554)

            frameOut2 = NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.R, FlipMode.FlipVertically, byteArray)
            CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap FlipVertically CheckSum", CheckSum2DFrame(frameOut2), 7059917)
            CompareLongInteger("VideoUtilsTests", "ByteArray FlipVertically CheckSum", CheckSumByteArray(byteArray), 175359397)

            frameOut2 = NH.GetMonochromePixelsFromBitmap(bitmap, LumaConversionMode.R, FlipMode.FlipBoth, byteArray)
            CompareLongInteger("VideoUtilsTests", "GetMonochromePixelsFromBitmap FlipBoth CheckSum", CheckSum2DFrame(frameOut2), 6219148)
            CompareLongInteger("VideoUtilsTests", "ByteArray FlipBoth CheckSum", CheckSumByteArray(byteArray), 174994618)

            frameColour = NH.GetColourPixelsFromBitmap(bitmap, FlipMode.None, byteArray)
            CompareLongInteger("VideoUtilsTests", "GetColourPixelsFromBitmap FlipNone CheckSum", CheckSum3DFrame(frameColour), 55234894)
            CompareLongInteger("VideoUtilsTests", "ByteArray CheckSum", CheckSumByteArray(byteArray), 295553892)

            frameColour = NH.GetColourPixelsFromBitmap(bitmap, FlipMode.FlipHorizontally, byteArray)
            CompareLongInteger("VideoUtilsTests", "GetColourPixelsFromBitmap FlipHorizontally CheckSum", CheckSum3DFrame(frameColour), 54805394)
            CompareLongInteger("VideoUtilsTests", "ByteArray FlipHorizontally CheckSum", CheckSumByteArray(byteArray), 295221231)

            frameColour = NH.GetColourPixelsFromBitmap(bitmap, FlipMode.FlipVertically, byteArray)
            CompareLongInteger("VideoUtilsTests", "GetColourPixelsFromBitmap FlipVertically CheckSum", CheckSum3DFrame(frameColour), 54831409)
            CompareLongInteger("VideoUtilsTests", "ByteArray FlipVertically CheckSum", CheckSumByteArray(byteArray), 304432836)

            frameColour = NH.GetColourPixelsFromBitmap(bitmap, FlipMode.FlipBoth, byteArray)
            CompareLongInteger("VideoUtilsTests", "GetColourPixelsFromBitmap FlipBoth CheckSum", CheckSum3DFrame(frameColour), 53990701)
            CompareLongInteger("VideoUtilsTests", "ByteArray FlipBoth CheckSum", CheckSumByteArray(byteArray), 304100175)
            frameColour = NH.GetColourPixelsFromBitmap(bitmap, FlipMode.None, byteArray)

            byteArray = NH.PrepareBitmapForDisplay(frameOut, 100, 100, FlipMode.None)
            CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay None CheckSum", CheckSumByteArray(byteArray), 105141385513)
            byteArray = NH.PrepareBitmapForDisplay(frameOut, 100, 100, FlipMode.FlipHorizontally)
            CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay FlipHorizontally CheckSum", CheckSumByteArray(byteArray), 105136497235)
            byteArray = NH.PrepareBitmapForDisplay(frameOut, 100, 100, FlipMode.FlipVertically)
            CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay FlipVertically CheckSum", CheckSumByteArray(byteArray), 105141405926)
            byteArray = NH.PrepareBitmapForDisplay(frameOut, 100, 100, FlipMode.FlipBoth)
            CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay FlipBoth CheckSum", CheckSumByteArray(byteArray), 105136517648)

            byteArray = NH.PrepareColourBitmapForDisplay(frameColour, frameColour.GetUpperBound(0), frameColour.GetUpperBound(1), FlipMode.None)
            CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay None CheckSum", CheckSumByteArray(byteArray), 243107340)
            byteArray = NH.PrepareColourBitmapForDisplay(frameColour, frameColour.GetUpperBound(0), frameColour.GetUpperBound(1), FlipMode.FlipHorizontally)
            CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay FlipHorizontally CheckSum", CheckSumByteArray(byteArray), 243407028)
            byteArray = NH.PrepareColourBitmapForDisplay(frameColour, frameColour.GetUpperBound(0), frameColour.GetUpperBound(1), FlipMode.FlipVertically)
            CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay FlipVertically CheckSum", CheckSumByteArray(byteArray), 243130927)
            byteArray = NH.PrepareColourBitmapForDisplay(frameColour, frameColour.GetUpperBound(0), frameColour.GetUpperBound(1), FlipMode.FlipBoth)
            CompareLongInteger("VideoUtilsTests", "PrepareBitmapForDisplay FlipBoth CheckSum", CheckSumByteArray(byteArray), 243430615)

        Catch ex As Exception
            LogException("VideoUtilTests", "Exception: " & ex.ToString)
        End Try

        TL.BlankLine()

    End Sub

    ''' <summary>
    ''' Initialise a frame with a known set of values
    ''' </summary>
    ''' <param name="frame">The frame to initialise</param>
    ''' <remarks></remarks>
    Sub InitFrame2D(ByRef frame As Integer(,))
        Dim i, j As Integer

        For i = 0 To frame.GetUpperBound(0) - 1
            For j = 0 To frame.GetUpperBound(1) - 1
                frame(i, j) = i * j
            Next
        Next
    End Sub

    ''' <summary>
    ''' Initialise a frame with a known set of values
    ''' </summary>
    ''' <param name="frame">The frame to initialise</param>
    ''' <remarks></remarks>
    Sub InitFrame3D(ByRef frame As Integer(,,))
        Dim i, j, k As Integer

        For i = 0 To frame.GetUpperBound(0) - 1
            For j = 0 To frame.GetUpperBound(1) - 1
                For k = 0 To frame.GetUpperBound(2) - 1
                    frame(i, j, k) = i * j * k
                Next
            Next
        Next
    End Sub

    ''' <summary>
    ''' Initialise a new bitmap with the ASCOM icon
    ''' </summary>
    ''' <param name="bm">The bitmap to initialise</param>
    ''' <remarks></remarks>
    Sub InitBitMap(ByRef bm)
        bm = New Bitmap(Me.Icon.ToBitmap())
    End Sub

    ''' <summary>
    ''' Initialise a byte array to 0
    ''' </summary>
    ''' <param name="byteArray">The byte array to initialise</param>
    ''' <remarks></remarks>
    Sub ClearByteArray(ByRef byteArray As Byte())
        Dim i As Integer

        For i = 0 To byteArray.LongLength - 1
            byteArray(i) = 0
        Next
    End Sub

    ''' <summary>
    ''' Calculate a checksum for the supplied frame array
    ''' </summary>
    ''' <param name="frame">The frame to checksum</param>
    ''' <returns>The checksum of the frame</returns>
    ''' <remarks></remarks>
    Function CheckSum2DFrame(frame As Integer(,)) As Int64
        Dim sum As Int64

        sum = 0
        For i = 0 To frame.GetUpperBound(0) - 1
            For j = 0 To frame.GetUpperBound(1) - 1
                sum += Convert.ToInt64(frame(i, j)) * Convert.ToInt64(i + 1) * Convert.ToInt64(j + 1)
            Next
        Next

        Return sum
    End Function

    ''' <summary>
    ''' Calculate a checksum for the supplied frame array
    ''' </summary>
    ''' <param name="frame">The frame to checksum</param>
    ''' <returns>The checksum of the frame</returns>
    ''' <remarks></remarks>
    Function CheckSum3DFrame(frame As Integer(,,)) As Int64
        Dim sum As Int64

        sum = 0
        For i = 0 To frame.GetUpperBound(0) - 1
            For j = 0 To frame.GetUpperBound(1) - 1
                For k = 0 To frame.GetUpperBound(2) - 1
                    sum += Convert.ToInt64(frame(i, j, k)) * Convert.ToInt64(i + 1) * Convert.ToInt64(j + 1) * Convert.ToInt64(k + 1)
                Next
            Next
        Next

        Return sum
    End Function

    ''' <summary>
    ''' Calculate a checksum for the supplied byte array
    ''' </summary>
    ''' <param name="byteArray">The byte array to checksum</param>
    ''' <returns>The checksum of the byte array</returns>
    ''' <remarks></remarks>
    Function CheckSumByteArray(byteArray As Byte()) As Int64
        Dim sum As Int64

        sum = 0
        For i = 0 To byteArray.LongLength - 1
            sum += Convert.ToInt64(byteArray(i)) * Convert.ToInt64(i + 1)
        Next

        Return sum
    End Function

    Sub UtilTests()
        Dim t As Double
        Dim ts As String
        Dim HelperType As Type
        Dim i As Integer, Is64Bit As Boolean
        Dim MyVersion As Version
        Dim Utl As Util

        Const TestDate As Date = #6/1/2010 4:37:00 PM#
        Const TestJulianDate As Double = 2455551.0

        Try
            Utl = New Util
            Is64Bit = (IntPtr.Size = 8) 'Create a simple variable to record whether or not we are 64bit
            Status("Running Utilities functional tests")
            TL.LogMessage("UtilTests", "Creating ASCOM.Utilities.Util")
            sw.Reset() : sw.Start()
            TL.LogMessage("UtilTests", "ASCOM.Utilities.Util Created OK in " & sw.ElapsedMilliseconds & " milliseconds")
            If Not Is64Bit Then
                TL.LogMessage("UtilTests", "Creating DriverHelper.Util")
                'DrvHlpUtil = CreateObject("DriverHelper.Util")
                HelperType = Type.GetTypeFromProgID("DriverHelper.Util")
                DrvHlpUtil = Activator.CreateInstance(HelperType)
                TL.LogMessage("UtilTests", "DriverHelper.Util Created OK")

                TL.LogMessage("UtilTests", "Creating DriverHelper2.Util")
                'g_Util2 = CreateObject("DriverHelper2.Util")
                HelperType = Type.GetTypeFromProgID("DriverHelper2.Util")
                g_Util2 = Activator.CreateInstance(HelperType)
                TL.LogMessage("UtilTests", "DriverHelper2.Util Created OK")
            Else
                TL.LogMessage("UtilTests", "Running 64bit so avoiding use of 32bit DriverHelper components")
            End If
            TL.BlankLine()

            Compare("UtilTests", "IsMinimumRequiredVersion 5.0", Utl.IsMinimumRequiredVersion(5, 0).ToString, "True")
            Compare("UtilTests", "IsMinimumRequiredVersion 5.4", Utl.IsMinimumRequiredVersion(5, 4).ToString, "True")
            Compare("UtilTests", "IsMinimumRequiredVersion 5.5", Utl.IsMinimumRequiredVersion(5, 5).ToString, "True")
            Compare("UtilTests", "IsMinimumRequiredVersion 5.6", Utl.IsMinimumRequiredVersion(5, 6).ToString, "True")
            Compare("UtilTests", "IsMinimumRequiredVersion 6.0", Utl.IsMinimumRequiredVersion(6, 0).ToString, "True")
            Compare("UtilTests", "IsMinimumRequiredVersion 6.1", Utl.IsMinimumRequiredVersion(6, 1).ToString, "True")
            Compare("UtilTests", "IsMinimumRequiredVersion 6.2", Utl.IsMinimumRequiredVersion(6, 2).ToString, "True")
            Compare("UtilTests", "IsMinimumRequiredVersion 6.3", Utl.IsMinimumRequiredVersion(6, 3).ToString, "True")
            Compare("UtilTests", "IsMinimumRequiredVersion 6.4", Utl.IsMinimumRequiredVersion(6, 4).ToString, "False")

            ' Check that the platform version properties return the correct values
            MyVersion = New Version(Application.ProductVersion) ' Get this assembly's version number against which to compare the Util version numbers
            CompareInteger("UtilTests", "Major Version", Utl.MajorVersion, MyVersion.Major)
            CompareInteger("UtilTests", "Minor Version", Utl.MinorVersion, MyVersion.Minor)
            CompareInteger("UtilTests", "Service Pack", Utl.ServicePack, MyVersion.Build)
            CompareInteger("UtilTests", "Build Number", Utl.BuildNumber, MyVersion.Revision)

            TL.BlankLine()

            IntArray1D(ArrayCopySize - 1) = ArrayCopySize
            IntArray2D(ArrayCopySize - 1, ArrayCopySize - 1) = ArrayCopySize
            IntArray3D(ArrayCopySize - 1, ArrayCopySize - 1, ArrayCopySize - 1) = ArrayCopySize

            CheckArray(IntArray1D)
            CheckArray(IntArray2D)
            CheckArray(IntArray3D)

            TL.BlankLine()
            If Is64Bit Then ' Run tests just on the new 64bit component
                t = 30.123456789 : Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, ":").ToString, "30:07'")
                t = 60.987654321 : Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, ":", ":", "", 4).ToString, "60:59:15" & DecimalSeparator & "5556")
                t = 50.123453456 : Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t).ToString, "03:20")
                t = 70.763245689 : Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t).ToString, "04:43:03")
                ts = "43:56:78" & DecimalSeparator & "2567" : Compare("UtilTests", "DMSToDegrees", Utl.DMSToDegrees(ts).ToString, "43" & DecimalSeparator & "9550713055555")
                ts = "14:39:23" : Compare("UtilTests", "HMSToDegrees", Utl.HMSToDegrees(ts).ToString, "219" & DecimalSeparator & "845833333333")
                ts = "14:37:23" : Compare("UtilTests", "HMSToHours", Utl.HMSToHours(ts).ToString, "14" & DecimalSeparator & "6230555555556")
                t = 15.567234086 : Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t), "15:34")
                t = 9.4367290317 : Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t), "09:26:12")
                TL.BlankLine()

                Compare("UtilTests", "Platform Version", Utl.PlatformVersion.ToString, ASCOMRegistryAccess.GetProfile("", "PlatformVersion"))
                TL.BlankLine()

                Compare("UtilTests", "TimeZoneName", Utl.TimeZoneName.ToString, GetTimeZoneName)
                CompareDouble("UtilTests", "TimeZoneOffset", Utl.TimeZoneOffset, -CDbl(TimeZone.CurrentTimeZone.GetUtcOffset(Now).Hours), 0.017) '1 minute tolerance
                Compare("UtilTests", "UTCDate", Utl.UTCDate.ToString, Date.UtcNow)
                CompareDouble("UtilTests", "Julian date", Utl.JulianDate, Date.UtcNow.ToOADate() + 2415018.5, 0.00002) '1 second tolerance
                TL.BlankLine()

                Compare("UtilTests", "DateJulianToLocal", Format(Utl.DateJulianToLocal(TestJulianDate).Subtract(TimeZone.CurrentTimeZone.GetUtcOffset(Now)), "dd MMM yyyy hh:mm:ss.ffff"), "20 " & AbbreviatedMonthNames(11) & " 2010 12:00:00.0000")
                Compare("UtilTests", "DateJulianToUTC", Format(Utl.DateJulianToUTC(TestJulianDate), "dd MMM yyyy hh:mm:ss.ffff"), "20 " & AbbreviatedMonthNames(11) & " 2010 12:00:00.0000")
                Compare("UtilTests", "DateLocalToJulian", Utl.DateLocalToJulian(TestDate.Add(TimeZone.CurrentTimeZone.GetUtcOffset(Now))), "2455349" & DecimalSeparator & "19236111")
                Compare("UtilTests", "DateLocalToUTC", Format(Utl.DateLocalToUTC(TestDate.Add(TimeZone.CurrentTimeZone.GetUtcOffset(Now))), "dd MMM yyyy hh:mm:ss.ffff"), "01 " & AbbreviatedMonthNames(5) & " 2010 04:37:00.0000")
                Compare("UtilTests", "DateUTCToJulian", Utl.DateUTCToJulian(TestDate).ToString, "2455349" & DecimalSeparator & "19236111")
                Compare("UtilTests", "DateUTCToLocal", Format(Utl.DateUTCToLocal(TestDate.Subtract(TimeZone.CurrentTimeZone.GetUtcOffset(Now))), "dd MMM yyyy hh:mm:ss.ffff"), "01 " & AbbreviatedMonthNames(5) & " 2010 04:37:00.0000")
                TL.BlankLine()

                t = 43.123894628 : Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t), "43° 07'")
                t = 43.123894628 : Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, "-"), "43-07'")
                t = 43.123894628 : Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, "-", ";"), "43-07;")
                t = 43.123894628 : Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, "-", ";", 3), "43-07" & DecimalSeparator & "434;")
                TL.BlankLine()

                t = 43.123894628 : Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t), "43° 07' 26""")
                t = 43.123894628 : Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-"), "43-07' 26""")
                t = 43.123894628 : Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-", ";"), "43-07;26""")
                t = 43.123894628 : Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-", ";", "#"), "43-07;26#")
                t = 43.123894628 : Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-", ";", "#", 3), "43-07;26" & DecimalSeparator & "021#")
                TL.BlankLine()

                t = 43.123894628 : Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t), "02:52")
                t = 43.123894628 : Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t, "-"), "02-52")
                t = 43.123894628 : Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t, "-", ";"), "02-52;")
                t = 43.123894628 : Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t, "-", ";", 3), "02-52" & DecimalSeparator & "496;")
                TL.BlankLine()

                t = 43.123894628 : Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t), "02:52:30")
                t = 43.123894628 : Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-"), "02-52:30")
                t = 43.123894628 : Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-", ";"), "02-52;30")
                t = 43.123894628 : Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-", ";", "#"), "02-52;30#")
                t = 43.123894628 : Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-", ";", "#", 3), "02-52;29" & DecimalSeparator & "735#")
                TL.BlankLine()

                t = 3.123894628 : Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t), "03:07")
                t = 3.123894628 : Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t, "-"), "03-07")
                t = 3.123894628 : Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t, "-", ";"), "03-07;")
                t = 3.123894628 : Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t, "-", ";", 3), "03-07" & DecimalSeparator & "434;")
                TL.BlankLine()

                t = 3.123894628 : Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t), "03:07:26")
                t = 3.123894628 : Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-"), "03-07:26")
                t = 3.123894628 : Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-", ";"), "03-07;26")
                t = 3.123894628 : Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-", ";", "#"), "03-07;26#")
                t = 3.123894628 : Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-", ";", "#", 3), "03-07;26" & DecimalSeparator & "021#")
            Else 'Run teststo compare original 32bit only and new 32/64bit capabale components
                t = 30.123456789 : Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, ":").ToString, DrvHlpUtil.DegreesToDM(t, ":").ToString)
                t = 60.987654321 : Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, ":", ":", "", 4).ToString, DrvHlpUtil.DegreesToDMS(t, ":", ":", "", 4).ToString)
                t = 50.123453456 : Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t).ToString, DrvHlpUtil.DegreesToHM(t).ToString)
                t = 70.763245689 : Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t).ToString, DrvHlpUtil.DegreesToHMS(t).ToString)
                ts = "43:56:78" & DecimalSeparator & "2567" : Compare("UtilTests", "DMSToDegrees", Utl.DMSToDegrees(ts).ToString, DrvHlpUtil.DMSToDegrees(ts).ToString)
                ts = "14:39:23" : Compare("UtilTests", "HMSToDegrees", Utl.HMSToDegrees(ts).ToString, DrvHlpUtil.HMSToDegrees(ts))
                ts = "14:37:23" : Compare("UtilTests", "HMSToHours", Utl.HMSToHours(ts).ToString, DrvHlpUtil.HMSToHours(ts))
                t = 15.567234086 : Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t), DrvHlpUtil.HoursToHM(t))
                t = 9.4367290317 : Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t), DrvHlpUtil.HoursToHMS(t))
                TL.BlankLine()

                Compare("UtilTests", "Platform Version", Utl.PlatformVersion.ToString, g_Util2.PlatformVersion.ToString)
                Compare("UtilTests", "SerialTrace", Utl.SerialTrace, g_Util2.SerialTrace)
                Compare("UtilTests", "Trace File", Utl.SerialTraceFile, g_Util2.SerialTraceFile)
                TL.BlankLine()

                Compare("UtilTests", "TimeZoneName", Utl.TimeZoneName.ToString, g_Util2.TimeZoneName.ToString)
                CompareDouble("UtilTests", "TimeZoneOffset", Utl.TimeZoneOffset, g_Util2.TimeZoneOffset, 0.017) '1 minute tolerance
                Compare("UtilTests", "UTCDate", Utl.UTCDate.ToString, g_Util2.UTCDate.ToString)
                CompareDouble("UtilTests", "Julian date", Utl.JulianDate, g_Util2.JulianDate, 0.00002) '1 second tolerance
                TL.BlankLine()

                Compare("UtilTests", "DateJulianToLocal", Format(Utl.DateJulianToLocal(TestJulianDate), "dd MMM yyyy hh:mm:ss.ffff"), Format(g_Util2.DateJulianToLocal(TestJulianDate), "dd MMM yyyy hh:mm:ss.ffff"))
                Compare("UtilTests", "DateJulianToUTC", Format(Utl.DateJulianToUTC(TestJulianDate), "dd MMM yyyy hh:mm:ss.ffff"), Format(g_Util2.DateJulianToUTC(TestJulianDate), "dd MMM yyyy hh:mm:ss.ffff"))
                Compare("UtilTests", "DateLocalToJulian", Utl.DateLocalToJulian(TestDate), g_Util2.DateLocalToJulian(TestDate))
                Compare("UtilTests", "DateLocalToUTC", Format(Utl.DateLocalToUTC(TestDate), "dd MMM yyyy hh:mm:ss.ffff"), Format(g_Util2.DateLocalToUTC(TestDate), "dd MMM yyyy hh:mm:ss.ffff"))
                Compare("UtilTests", "DateUTCToJulian", Utl.DateUTCToJulian(TestDate).ToString, g_Util2.DateUTCToJulian(TestDate).ToString)
                Compare("UtilTests", "DateUTCToLocal", Format(Utl.DateUTCToLocal(TestDate), "dd MMM yyyy hh:mm:ss.ffff"), Format(g_Util2.DateUTCToLocal(TestDate), "dd MMM yyyy hh:mm:ss.ffff"))
                TL.BlankLine()

                t = 43.123894628 : Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t), DrvHlpUtil.DegreesToDM(t))
                t = 43.123894628 : Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, "-"), DrvHlpUtil.DegreesToDM(t, "-"))
                t = 43.123894628 : Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, "-", ";"), DrvHlpUtil.DegreesToDM(t, "-", ";"))
                t = 43.123894628 : Compare("UtilTests", "DegreesToDM", Utl.DegreesToDM(t, "-", ";", 3), DrvHlpUtil.DegreesToDM(t, "-", ";", 3))
                TL.BlankLine()

                t = 43.123894628 : Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t), DrvHlpUtil.DegreesToDMS(t))
                t = 43.123894628 : Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-"), DrvHlpUtil.DegreesToDMS(t, "-"))
                t = 43.123894628 : Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-", ";"), DrvHlpUtil.DegreesToDMS(t, "-", ";"))
                t = 43.123894628 : Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-", ";", "#"), DrvHlpUtil.DegreesToDMS(t, "-", ";", "#"))
                t = 43.123894628 : Compare("UtilTests", "DegreesToDMS", Utl.DegreesToDMS(t, "-", ";", "#", 3), DrvHlpUtil.DegreesToDMS(t, "-", ";", "#", 3))
                TL.BlankLine()

                t = 43.123894628 : Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t), DrvHlpUtil.DegreesToHM(t))
                t = 43.123894628 : Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t, "-"), DrvHlpUtil.DegreesToHM(t, "-"))
                t = 43.123894628 : Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t, "-", ";"), DrvHlpUtil.DegreesToHM(t, "-", ";"))
                t = 43.123894628 : Compare("UtilTests", "DegreesToHM", Utl.DegreesToHM(t, "-", ";", 3), DrvHlpUtil.DegreesToHM(t, "-", ";", 3))
                TL.BlankLine()

                t = 43.123894628 : Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t), DrvHlpUtil.DegreesToHMS(t))
                t = 43.123894628 : Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-"), DrvHlpUtil.DegreesToHMS(t, "-"))
                t = 43.123894628 : Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-", ";"), DrvHlpUtil.DegreesToHMS(t, "-", ";"))
                t = 43.123894628 : Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-", ";", "#"), DrvHlpUtil.DegreesToHMS(t, "-", ";", "#"))
                t = 43.123894628 : Compare("UtilTests", "DegreesToHMS", Utl.DegreesToHMS(t, "-", ";", "#", 3), DrvHlpUtil.DegreesToHMS(t, "-", ";", "#", 3))
                TL.BlankLine()

                t = 3.123894628 : Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t), DrvHlpUtil.HoursToHM(t))
                t = 3.123894628 : Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t, "-"), DrvHlpUtil.HoursToHM(t, "-"))
                t = 3.123894628 : Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t, "-", ";"), DrvHlpUtil.HoursToHM(t, "-", ";"))
                t = 3.123894628 : Compare("UtilTests", "HoursToHM", Utl.HoursToHM(t, "-", ";", 3), DrvHlpUtil.HoursToHM(t, "-", ";", 3))
                TL.BlankLine()

                t = 3.123894628 : Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t), DrvHlpUtil.HoursToHMS(t))
                t = 3.123894628 : Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-"), DrvHlpUtil.HoursToHMS(t, "-"))
                t = 3.123894628 : Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-", ";"), DrvHlpUtil.HoursToHMS(t, "-", ";"))
                t = 3.123894628 : Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-", ";", "#"), DrvHlpUtil.HoursToHMS(t, "-", ";", "#"))
                t = 3.123894628 : Compare("UtilTests", "HoursToHMS", Utl.HoursToHMS(t, "-", ";", "#", 3), DrvHlpUtil.HoursToHMS(t, "-", ";", "#", 3))
            End If
            TL.BlankLine()
            Status("Running Utilities timing tests")
            TL.LogMessage("UtilTests", "Timing tests")
            For i = 0 To 5
                TimingTest(i, Is64Bit)
            Next

            For i = 10 To 50 Step 10
                TimingTest(i, Is64Bit)
            Next

            TimingTest(100, Is64Bit)
            TimingTest(500, Is64Bit)
            TimingTest(1000, Is64Bit)
            TimingTest(2000, Is64Bit)
            TL.BlankLine()

            ' Test conversion funcitons
            CompareDouble("UtilTests", "DewPoint2Humidity", Utl.DewPoint2Humidity(20.0, 25.0), 73.81, TOLERANCE_E4)
            CompareDouble("UtilTests", "Humidity2DewPoint", Utl.Humidity2DewPoint(45.0, 5.0), -5.948, TOLERANCE_E4)

            'Test that invalid values are rejected by dewpoint and humidity conversion functions
            Dim result As Double, ErrMsg As String

            Try
                result = Utl.DewPoint2Humidity(-300.0, 25.0)
                ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when dew point < absolute zero"
                TL.LogMessageCrLf("TestInvalidOperation", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)

            Catch ex As InvalidValueException ' Expected behaviour for bad value
                TL.LogMessage("UtilTests", "DewPoint2Humidity - InvalidValueException thrown as expected when dew point < absolute zero")
                NMatches += 1

            Catch ex As Exception
                ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when dew point < absolute zero" & ", instead received: " & ex.Message
                TL.LogMessageCrLf("UtilTests", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)
            End Try

            Try
                result = Utl.DewPoint2Humidity(101.0, 25.0)
                ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when dew point > 100.0C"
                TL.LogMessageCrLf("TestInvalidOperation", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)

            Catch ex As InvalidValueException ' Expected behaviour for bad value
                TL.LogMessage("UtilTests", "DewPoint2Humidity - InvalidValueException thrown as expected when dew point > 100.0C")
                NMatches += 1

            Catch ex As Exception
                ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when dew point > 100.0C" & ", instead received: " & ex.Message
                TL.LogMessageCrLf("UtilTests", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)
            End Try

            Try
                result = Utl.DewPoint2Humidity(25.0, -300.0)
                ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when ambient temperature < absolute zero"
                TL.LogMessageCrLf("TestInvalidOperation", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)

            Catch ex As InvalidValueException ' Expected behaviour for bad value
                TL.LogMessage("UtilTests", "DewPoint2Humidity - InvalidValueException thrown as expected when ambient temperature < absolute zero")
                NMatches += 1

            Catch ex As Exception
                ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when ambient temperature < absolute zero" & ", instead received: " & ex.Message
                TL.LogMessageCrLf("UtilTests", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)
            End Try

            Try
                result = Utl.DewPoint2Humidity(25.0, 101.0)
                ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when ambient temperature > 100.0C"
                TL.LogMessageCrLf("TestInvalidOperation", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)

            Catch ex As InvalidValueException ' Expected behaviour for bad value
                TL.LogMessage("UtilTests", "DewPoint2Humidity - InvalidValueException thrown as expected when ambient temperature > 100.0C")
                NMatches += 1

            Catch ex As Exception
                ErrMsg = "##### DewPoint2Humidity - InvalidValueException not thrown when ambient temperature > 100.0C" & ", instead received: " & ex.Message
                TL.LogMessageCrLf("UtilTests", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)
            End Try

            Try
                result = Utl.Humidity2DewPoint(-1.0, 25.0)
                ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when humidity < 0.0"
                TL.LogMessageCrLf("TestInvalidOperation", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)

            Catch ex As InvalidValueException ' Expected behaviour for bad value
                TL.LogMessage("UtilTests", "Humidity2DewPoint - InvalidValueException thrown as expected when humidity < 0.0")
                NMatches += 1

            Catch ex As Exception
                ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when humidity < 0.0" & ", instead received: " & ex.Message
                TL.LogMessageCrLf("UtilTests", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)
            End Try

            Try
                result = Utl.DewPoint2Humidity(101.0, 25.0)
                ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when humidity > 100.0%"
                TL.LogMessageCrLf("TestInvalidOperation", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)

            Catch ex As InvalidValueException ' Expected behaviour for bad value
                TL.LogMessage("UtilTests", "Humidity2DewPoint - InvalidValueException thrown as expected when humidity > 100.0%")
                NMatches += 1

            Catch ex As Exception
                ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when humidity > 100.0%" & ", instead received: " & ex.Message
                TL.LogMessageCrLf("UtilTests", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)
            End Try

            Try
                result = Utl.DewPoint2Humidity(25.0, -300.0)
                ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when ambient temperature < absolute zero"
                TL.LogMessageCrLf("TestInvalidOperation", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)

            Catch ex As InvalidValueException ' Expected behaviour for bad value
                TL.LogMessage("UtilTests", "Humidity2DewPoint - InvalidValueException thrown as expected when ambient temperature < absolute zero")
                NMatches += 1

            Catch ex As Exception
                ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when ambient temperature < absolute zero" & ", instead received: " & ex.Message
                TL.LogMessageCrLf("UtilTests", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)
            End Try

            Try
                result = Utl.DewPoint2Humidity(25.0, 101.0)
                ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when ambient temperature > 100.0C"
                TL.LogMessageCrLf("TestInvalidOperation", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)

            Catch ex As InvalidValueException ' Expected behaviour for bad value
                TL.LogMessage("UtilTests", "Humidity2DewPoint - InvalidValueException thrown as expected when ambient temperature > 100.0C")
                NMatches += 1

            Catch ex As Exception
                ErrMsg = "##### Humidity2DewPoint - InvalidValueException not thrown when ambient temperature > 100.0C" & ", instead received: " & ex.Message
                TL.LogMessageCrLf("UtilTests", ErrMsg)
                NNonMatches += 1
                ErrorList.Add("UtilTests" & " - " & ErrMsg)
            End Try

            CompareDouble("UtilTests", "metresPerSecond==>metresPerSecond", Utl.ConvertUnits(10.0, Units.metresPerSecond, Units.metresPerSecond), 10.0, TOLERANCE_E4)
            CompareDouble("UtilTests", "metresPerSecond==>milesPerHour", Utl.ConvertUnits(10.0, Units.metresPerSecond, Units.milesPerHour), 22.36936, TOLERANCE_E4)
            CompareDouble("UtilTests", "metresPerSecond==>knots", Utl.ConvertUnits(10.0, Units.metresPerSecond, Units.knots), 19.438461718, TOLERANCE_E4) ' These are Knots and not UK or US admiralty knots, which are slightly different!
            CompareDouble("UtilTests", "milesPerHour==>metresPerSecond", Utl.ConvertUnits(10.0, Units.milesPerHour, Units.metresPerSecond), 4.4704, TOLERANCE_E4)
            CompareDouble("UtilTests", "knots==>metresPerSecond", Utl.ConvertUnits(10.0, Units.knots, Units.metresPerSecond), 5.14444, TOLERANCE_E4)

            CompareDouble("UtilTests", "Degrees K==>Degrees K", Utl.ConvertUnits(300.0, Units.degreesKelvin, Units.degreesKelvin), 300.0, TOLERANCE_E4)
            CompareDouble("UtilTests", "Degrees K==>Degrees F", Utl.ConvertUnits(300.0, Units.degreesKelvin, Units.degreesFarenheit), 80.33, TOLERANCE_E4)
            CompareDouble("UtilTests", "Degrees K==>Degrees C", Utl.ConvertUnits(300.0, Units.degreesKelvin, Units.degreesCelsius), 26.85, TOLERANCE_E4)
            CompareDouble("UtilTests", "Degrees F==>Degrees K", Utl.ConvertUnits(70.0, Units.degreesFarenheit, Units.degreesKelvin), 294.26, TOLERANCE_E4)
            CompareDouble("UtilTests", "Degrees C==>Degrees K", Utl.ConvertUnits(20.0, Units.degreesCelsius, Units.degreesKelvin), 293.15, TOLERANCE_E4)

            CompareDouble("UtilTests", "hPa==>hPa", Utl.ConvertUnits(1000.0, Units.hPa, Units.hPa), 1000.0, TOLERANCE_E4)
            CompareDouble("UtilTests", "hPa==>mBar", Utl.ConvertUnits(1000.0, Units.hPa, Units.mBar), 1000.0, TOLERANCE_E4)
            CompareDouble("UtilTests", "hPa==>mmHg", Utl.ConvertUnits(1000.0, Units.hPa, Units.mmHg), 750.0615, TOLERANCE_E4)
            CompareDouble("UtilTests", "hPa==>inHg", Utl.ConvertUnits(1000.0, Units.hPa, Units.inHg), 29.53, TOLERANCE_E4)
            CompareDouble("UtilTests", "mBar==>hPa", Utl.ConvertUnits(1000.0, Units.mBar, Units.hPa), 1000.0, TOLERANCE_E4)
            CompareDouble("UtilTests", "mmHg==>hPa", Utl.ConvertUnits(800.0, Units.mmHg, Units.hPa), 1066.58, TOLERANCE_E4)
            CompareDouble("UtilTests", "inHg==>hPa", Utl.ConvertUnits(30.0, Units.inHg, Units.hPa), 1015.92, TOLERANCE_E4)

            CompareDouble("UtilTests", "mmPerHour==>mmPerHour", Utl.ConvertUnits(100.0, Units.mmPerHour, Units.mmPerHour), 100.0, TOLERANCE_E4)
            CompareDouble("UtilTests", "mmPerHour==>inPerHour", Utl.ConvertUnits(100.0, Units.mmPerHour, Units.inPerHour), 3.937, TOLERANCE_E4)
            CompareDouble("UtilTests", "inPerHour==>mmPerHour", Utl.ConvertUnits(10.0, Units.inPerHour, Units.mmPerHour), 254.0, TOLERANCE_E4)

            CompareDouble("UtilTests", "ConvertPressure 0.0 ==> 0.0", Utl.ConvertPressure(1000.0, 0.0, 0.0), 1000.0, TOLERANCE_E4)
            CompareDouble("UtilTests", "ConvertPressure 3000.0 ==> 3000.0", Utl.ConvertPressure(1000.0, 3000.0, 3000.0), 1000.0, TOLERANCE_E4)
            CompareDouble("UtilTests", "ConvertPressure 0.0 ==> 3000.0", Utl.ConvertPressure(1000.0, 0.0, 3000.0), 691.92, TOLERANCE_E4)
            CompareDouble("UtilTests", "ConvertPressure 3000.0 ==> 0.0", Utl.ConvertPressure(700.0, 3000.0, 0.0), 1011.68, TOLERANCE_E4)
            CompareDouble("UtilTests", "ConvertPressure 3000.0 ==> 1000.0", Utl.ConvertPressure(700.0, 3000.0, 1000.0), 897.35, TOLERANCE_E4)

            TestUnitsInvalidOperation(Utl, Units.metresPerSecond, Units.degreesCelsius)
            TestUnitsInvalidOperation(Utl, Units.metresPerSecond, Units.hPa)
            TestUnitsInvalidOperation(Utl, Units.metresPerSecond, Units.mmPerHour)

            TestUnitsInvalidOperation(Utl, Units.degreesCelsius, Units.milesPerHour)
            TestUnitsInvalidOperation(Utl, Units.degreesCelsius, Units.mBar)
            TestUnitsInvalidOperation(Utl, Units.degreesCelsius, Units.inPerHour)

            TestUnitsInvalidOperation(Utl, Units.mmHg, Units.knots)
            TestUnitsInvalidOperation(Utl, Units.mmHg, Units.degreesFarenheit)
            TestUnitsInvalidOperation(Utl, Units.mmHg, Units.mmPerHour)

            TestUnitsInvalidOperation(Utl, Units.inPerHour, Units.metresPerSecond)
            TestUnitsInvalidOperation(Utl, Units.inPerHour, Units.degreesKelvin)
            TestUnitsInvalidOperation(Utl, Units.inPerHour, Units.hPa)






            Try
                Utl.Dispose()
                TL.LogMessage("UtilTests", "ASCOM.Utilities.Dispose, Disposed OK")
            Catch ex As Exception
                LogException("UtilTests", "ASCOM.Utilities.Dispose Exception: " & ex.ToString)
            End Try

            If Not Is64Bit Then
                Try
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(DrvHlpUtil)
                    TL.LogMessage("UtilTests", "Helper Util.Release OK")
                Catch ex As Exception
                    LogException("UtilTests", "Helper Util.Release Exception: " & ex.ToString)
                End Try
            End If

            Utl = Nothing
            DrvHlpUtil = Nothing

            TL.LogMessage("UtilTests", "Finished")
            TL.BlankLine()

        Catch ex As Exception
            LogException("UtilTests", "Exception: " & ex.ToString)
        End Try

    End Sub

    Private Sub TestUnitsInvalidOperation(Util As Util, FromUnit As Units, ToUnit As Units)
        Dim result As Double, ErrMsg As String

        Try
            result = Util.ConvertUnits(0.0, FromUnit, ToUnit)
            ErrMsg = "##### InvalidOperationException not thrown - FromUnit: " & FromUnit.ToString() & ", ToUnit: " & ToUnit.ToString()
            TL.LogMessageCrLf("TestInvalidOperation", ErrMsg)
            NNonMatches += 1
            ErrorList.Add("TestInvalidOperation" & " - " & ErrMsg)

        Catch ex As InvalidOperationException ' Expected behaviour for mismatched types of unit
            TL.LogMessage("TestInvalidOperation", "InvalidOperationException thrown as expected for FromUnit: " & FromUnit.ToString() & ", ToUnit: " & ToUnit.ToString())
            NMatches += 1

        Catch ex As Exception
            ErrMsg = "##### InvalidOperationException not thrown - FromUnit: " & FromUnit.ToString() & ", ToUnit: " & ToUnit.ToString() & ", instead received: " & ex.Message
            TL.LogMessageCrLf("TestInvalidOperation", ErrMsg)
            NNonMatches += 1
            ErrorList.Add("TestInvalidOperation" & " - " & ErrMsg)
        End Try
    End Sub

    Private Sub CheckArray(InputObject As Object)
        Dim InputArray, ReturnArray As Array
        Dim ReturnObject As Object
        Dim InputType, ReturnType As Type
        Dim InputElementTypeName, ReturnElementTypeName As String

        InputArray = CType(InputObject, Array)
        InputType = InputArray.GetType
        InputElementTypeName = InputType.GetElementType.Name
        TL.LogMessage("UtilTests", "Input array Type: " & InputType.Name & ", Element Type: " & InputElementTypeName & ", Array Rank: " & InputArray.Rank & ", Array Length: " & InputArray.LongLength)

        ReturnObject = AscomUtil.ArrayToVariantArray(InputObject)
        ReturnArray = CType(ReturnObject, Array)
        ReturnType = ReturnArray.GetType
        ReturnElementTypeName = ReturnType.GetElementType.Name
        TL.LogMessage("UtilTests", "Return array Type: " & ReturnType.Name & ", Element Type: " & ReturnElementTypeName & ", Array Rank: " & ReturnArray.Rank & ", Array Length: " & ReturnArray.LongLength)

        CompareBoolean("UtilTests", "CheckArray", ReturnType.IsArray, True)
        CompareInteger("UtilTests", "CheckArray", ReturnArray.Rank, InputArray.Rank)
        Compare("UtilTests", "CheckArray", ReturnElementTypeName, GetType(Object).Name)

        Select Case ReturnArray.Rank
            Case 1 : CompareInteger("UtilTests", "CheckArray", ReturnArray(ArrayCopySize - 1), ArrayCopySize)
            Case 2 : CompareInteger("UtilTests", "CheckArray", CType(ReturnArray(ArrayCopySize - 1, ArrayCopySize - 1), Int32), ArrayCopySize)
            Case 3 : CompareInteger("UtilTests", "CheckArray", CType(ReturnArray(ArrayCopySize - 1, ArrayCopySize - 1, ArrayCopySize - 1), Int32), ArrayCopySize)
            Case Else
                LogError("UtilTests:CheckArray", "Returned array rank is outside expected range of 1..3: " & ReturnArray.Rank)
        End Select

        ReturnArray = CType(ReturnObject, Array)

    End Sub

    Private Function GetTimeZoneName() As String
        If TimeZone.CurrentTimeZone.IsDaylightSavingTime(Now) Then
            Return TimeZone.CurrentTimeZone.DaylightName
        Else
            Return TimeZone.CurrentTimeZone.StandardName
        End If
    End Function

    Sub TimingTest(ByVal p_NumberOfMilliSeconds As Integer, ByVal Is64Bit As Boolean)
        Action("TimingTest " & p_NumberOfMilliSeconds & "ms")
        s1 = Stopwatch.StartNew 'Test time using new ASCOM component
        AscomUtil.WaitForMilliseconds(p_NumberOfMilliSeconds)
        s1.Stop()
        Application.DoEvents()

        If Is64Bit Then
            TL.LogMessage("UtilTests - Timing", "Timer test (64bit): " & p_NumberOfMilliSeconds.ToString & " milliseconds - ASCOM.Utillities.WaitForMilliSeconds: " & Format(s1.ElapsedTicks * 1000.0 / Stopwatch.Frequency, "0.00") & "ms")
        Else
            System.Threading.Thread.Sleep(100)
            s2 = Stopwatch.StartNew 'Test time using original Platform 5 component
            DrvHlpUtil.WaitForMilliseconds(p_NumberOfMilliSeconds)
            s2.Stop()
            TL.LogMessage("UtilTests - Timing", "Timer test: " & p_NumberOfMilliSeconds.ToString & " milliseconds - ASCOM.Utillities.WaitForMilliSeconds: " & Format(s1.ElapsedTicks * 1000.0 / Stopwatch.Frequency, "0.00") & "ms DriverHelper.Util.WaitForMilliSeconds: " & Format(s2.ElapsedTicks * 1000.0 / Stopwatch.Frequency, "0.00") & "ms")
        End If
        Application.DoEvents()
    End Sub

    Sub ScanEventLog()
        Dim ELog As EventLog
        Dim Entries As EventLogEntryCollection
        Dim EventLogs() As EventLog
        Dim ErrorLog, MessageLog As String
        Dim SR As StreamReader = Nothing

        Dim Found As Boolean
        Try
            TL.LogMessage("ScanEventLog", "Start")
            EventLogs = EventLog.GetEventLogs()
            Found = False
            For Each EventLog As EventLog In EventLogs
                Try
                    TL.LogMessage("ScanEventLog", "Found log: " & EventLog.LogDisplayName)
                    If EventLog.LogDisplayName.ToUpper = "ASCOM" Then Found = True
                Catch
                End Try
            Next
            TL.BlankLine()

            TL.LogMessage("ScanEventLog", "ASCOM Log entries")
            If Found Then
                ELog = New EventLog(EVENTLOG_NAME, ".", EVENT_SOURCE)
                Entries = ELog.Entries
                For Each Entry As EventLogEntry In Entries
                    TL.LogMessageCrLf("ScanEventLog", Entry.TimeGenerated & " " &
                                                      Entry.Source & " " &
                                                      Entry.EntryType.ToString & " " &
                                                      Entry.UserName & " " &
                                                      Entry.InstanceId & " " &
                                                      Entry.Message.Trim(New Char() {Chr(10), Chr(13)}))
                Next
                TL.LogMessage("ScanEventLog", "ASCOM Log entries complete")
                TL.BlankLine()
            Else
                LogError("ScanEventLog", "ASCOM event log does not exist!")
                TL.BlankLine()
            End If

            ErrorLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & GlobalConstants.EVENTLOG_ERRORS
            MessageLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & GlobalConstants.EVENTLOG_MESSAGES

            If File.Exists(MessageLog) Or File.Exists(ErrorLog) Then
                LogError("ScanEventLog", "Errors have occured while writing to the ASCOM event log, please see detail earlier in this log.")
                TL.LogMessage("", "")

                If File.Exists(MessageLog) Then
                    Try
                        TL.LogMessage("ScanEventLog Found", GlobalConstants.EVENTLOG_MESSAGES)
                        SR = File.OpenText(MessageLog)

                        Do Until SR.EndOfStream 'include the file
                            TL.LogMessage("ScanEventLog", SR.ReadLine())
                        Loop

                        TL.LogMessage("", "")
                        SR.Close()
                        SR.Dispose()
                        SR = Nothing
                    Catch ex As Exception
                        LogException("ScanEventLog", "Exception: " & ex.ToString)
                        If Not (SR Is Nothing) Then 'Clean up streamreader
                            Try : SR.Close() : Catch : End Try
                            Try : SR.Dispose() : Catch : End Try
                            SR = Nothing
                        End If
                    End Try
                End If

                If File.Exists(ErrorLog) Then
                    Try
                        TL.LogMessage("ScanEventLog Found", GlobalConstants.EVENTLOG_ERRORS)
                        SR = File.OpenText(ErrorLog)

                        Do Until SR.EndOfStream 'include the file
                            TL.LogMessage("ScanEventLog", SR.ReadLine())
                        Loop

                        TL.LogMessage("", "")
                        SR.Close()
                        SR.Dispose()
                        SR = Nothing
                    Catch ex As Exception
                        LogException("ScanEventLog", "Exception: " & ex.ToString)
                        If Not (SR Is Nothing) Then 'Clean up streamreader
                            Try : SR.Close() : Catch : End Try
                            Try : SR.Dispose() : Catch : End Try
                            SR = Nothing
                        End If
                    End Try
                End If

            End If
            TL.LogMessage("", "")
        Catch ex As Exception
            LogException("ScanEventLog", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub ScanRegistrySecurity()
        Dim Key As RegistryKey
        Try
            Status("Scanning Registry Security")
            TL.LogMessage("RegistrySecurity", "Start")

            RegistryRights(Registry.CurrentUser, "", False)
            RegistryRights(Registry.CurrentUser, "SOFTWARE\ASCOM", False)
            RegistryRights(Registry.ClassesRoot, "", False)
            RegistryRights(Registry.ClassesRoot, "DriverHelper.Util", False)

            RegistryRights(Registry.LocalMachine, "", False)
            RegistryRights(Registry.LocalMachine, "SOFTWARE", False)

            If OSBits() = Bitness.Bits64 Then '64bit OS so look in Wow64node for ASCOM profile store
                Try
                    'List the 32bit registry
                    TL.LogMessage("RegistrySecurity", "Machine Profile Root (64bit OS - 32bit Registry)")
                    RegistryRights(Registry.LocalMachine, "SOFTWARE\WOW6432Node\ASCOM", False)
                    RegistryRights(Registry.LocalMachine, "SOFTWARE\WOW6432Node\ASCOM\Telescope Drivers", False)
                    RegistryRights(Registry.LocalMachine, "SOFTWARE\WOW6432Node\ASCOM\Telescope Drivers\ASCOM.Simulator.Telescope", False)
                    Key = ASCOMRegistryAccess.OpenSubKey(Registry.LocalMachine, REGISTRY_ROOT_KEY_NAME, False, RegistryAccess.RegWow64Options.KEY_WOW64_32KEY)
                    RecursionLevel = -1
                    RecurseRegistrySecurity(Key)
                Catch ex As Exception
                    LogException("RegistrySecurity", "Exception: " & ex.ToString)
                End Try
                TL.BlankLine()
            Else '32 bit OS
                Try
                    'List the registry (only one view on a 32bit machine)
                    TL.LogMessage("RegistrySecurity", "Machine Profile Root (32bit OS)")
                    RegistryRights(Registry.LocalMachine, "SOFTWARE\ASCOM", False)
                    RegistryRights(Registry.LocalMachine, "SOFTWARE\ASCOM\Telescope Drivers", False)
                    RegistryRights(Registry.LocalMachine, "SOFTWARE\ASCOM\Telescope Drivers\ASCOM.Simulator.Telescope", False)
                    Key = Registry.LocalMachine.OpenSubKey(REGISTRY_ROOT_KEY_NAME)
                    RecursionLevel = -1
                    RecurseRegistrySecurity(Key)
                Catch ex As Exception
                    LogException("RegistrySecurity", "Exception: " & ex.ToString)
                End Try
            End If

            TL.BlankLine()

        Catch ex As Exception
            LogException("RegistrySecurity", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub RecurseRegistrySecurity(ByVal Key As RegistryKey)
        Dim SubKeys() As String
        Dim sec As System.Security.AccessControl.RegistrySecurity
        Dim FoundFullAccess As Boolean = False
        Dim debugSwitch As Boolean = False
        Dim builtInUsers As String

        Try
            builtInUsers = GetBuiltInUsers().ToUpper()
            RecursionLevel += 1

            Try
                If (debugSwitch) Then TL.LogMessage("RegistrySecurityDbg", "Entered ReadRegistryRights")
                If (debugSwitch) Then TL.LogMessage("RegistrySecurityRec", "Processing key: " & Key.Name.ToString)

                If (debugSwitch) Then TL.LogMessage("RegistrySecurityDbg", "Getting access control")
                sec = Key.GetAccessControl()
                If (debugSwitch) Then TL.LogMessage("RegistrySecurityDbg", "Starting iteration of security rules")

                For Each RegRule As RegistryAccessRule In sec.GetAccessRules(True, True, GetType(NTAccount)) 'Iterate over the rule set and list them
                    If (debugSwitch) Then
                        TL.LogMessage("RegistrySecurityDbg", "Before printing rule")
                        TL.LogMessage("RegistrySecurityRec", RegRule.AccessControlType.ToString() & " " &
                                                      RegRule.IdentityReference.ToString() & " " &
                                                      RegRule.RegistryRights.ToString() & " / " &
                                                      IIf(RegRule.IsInherited.ToString(), "Inherited", "NotInherited") & " / " &
                                                      RegRule.InheritanceFlags.ToString() & " / " &
                                                      RegRule.PropagationFlags.ToString())
                        TL.LogMessage("RegistrySecurityDbg", "After printing rule")
                    End If
                    If (RegRule.IdentityReference.ToString.ToUpper = builtInUsers) And (RegRule.RegistryRights = Global.System.Security.AccessControl.RegistryRights.FullControl) Then
                        FoundFullAccess = True
                    End If
                    If (debugSwitch) Then TL.LogMessage("RegistrySecurityDbg", "After testing for FullAccess")
                Next

                If (debugSwitch) Then TL.LogMessage("RegistrySecurityDbg", "Completed iteration of security rules")
                If FoundFullAccess Then
                    NMatches += 1
                    TL.LogMessage("RegistrySecurityRec", "OK - SubKey " & Key.Name & " does have full registry access rights for BUILTIN\Users")
                Else
                    LogError("RegistrySecurityRec", "Subkey " & Key.Name & " does not have full access rights for BUILTIN\Users!")
                End If

                If (debugSwitch) Then TL.LogMessage("RegistrySecurityDbg", "End of Try-Catch code")
            Catch ex As NullReferenceException
                LogException("RegistrySecurityRec", "The subkey: " & Key.Name & " does not exist.")
            Catch ex As Exception
                LogException("RegistrySecurityRec", ex.ToString)
            End Try
            If (debugSwitch) Then TL.LogMessage("RegistrySecurityDbg", "Exited ReadRegistryRights")

        Catch ex As Exception
            LogException("RegistrySecurityRec 1", "Exception: " & ex.ToString)
        End Try

        Try
            SubKeys = Key.GetSubKeyNames
            For Each SubKey As String In SubKeys
                If (debugSwitch) Then TL.LogMessage("RegistrySecurityRec", "Recursing to Profile Key: " & SubKey)
                RecurseRegistrySecurity(Key.OpenSubKey(SubKey))
            Next
        Catch ex As Exception
            LogException("RegistrySecurityRec 2", "Exception: " & ex.ToString)
        End Try
        RecursionLevel -= 1

    End Sub

    Private Sub RegistryRights(ByVal Key As RegistryKey, ByVal SubKey As String, ByVal ConfirmFullAccess As Boolean)
        Dim sec As System.Security.AccessControl.RegistrySecurity
        Dim sKey As RegistryKey
        Dim foundFullAccess As Boolean = False
        Dim debugSwitch As Boolean = False
        Dim builtInUsers As String

        Try
            builtInUsers = GetBuiltInUsers().ToUpper()
            If (debugSwitch) Then TL.LogMessage("RegistrySecurityDbg", "Entered ReadRegistryRights")
            TL.LogMessage("RegistryRights", IIf(SubKey = "", Key.Name.ToString, Key.Name.ToString & "\" & SubKey))
            If (SubKey = "") Or (SubKey = ASCOM_ROOT_KEY) Then
                sKey = Key
            Else
                sKey = Key.OpenSubKey(SubKey)
            End If

            If (debugSwitch) Then TL.LogMessage("RegistryRightsDbg", "Getting access control")
            sec = sKey.GetAccessControl() 'System.Security.AccessControl.AccessControlSections.All)
            If (debugSwitch) Then TL.LogMessage("RegistryRightsDbg", "Starting iteration of security rules")

            For Each RegRule As RegistryAccessRule In sec.GetAccessRules(True, True, GetType(NTAccount)) 'Iterate over the rule set and list them
                If (debugSwitch) Then TL.LogMessage("RegistryRightsDbg", "Before printing rule")
                Try
                    TL.LogMessage("RegistryRights", RegRule.AccessControlType.ToString() & " " &
                                                  RegRule.IdentityReference.ToString() & " " &
                                                  RegRule.RegistryRights.ToString() & " / " &
                                                  IIf(RegRule.IsInherited.ToString(), "Inherited", "NotInherited") & " / " &
                                                  RegRule.InheritanceFlags.ToString() & " / " &
                                                  RegRule.PropagationFlags.ToString())
                    If (debugSwitch) Then TL.LogMessage("RegistryRightsDbg", "After printing rule")
                Catch ex1 As Exception
                    LogException("RegistryRights", "Issue formatting registry rights: " & ex1.ToString)
                End Try

                If (RegRule.IdentityReference.ToString.ToUpper = builtInUsers) And (RegRule.RegistryRights = Global.System.Security.AccessControl.RegistryRights.FullControl) Then
                    foundFullAccess = True
                End If
                If (debugSwitch) Then TL.LogMessage("RegistryRightsDbg", "After testing for FullAccess")
            Next

            If (debugSwitch) Then TL.LogMessage("RegistryRightsDbg", "Completed iteration of security rules")
            If ConfirmFullAccess Then 'Check whether full access is availble if required
                If foundFullAccess Then
                    NMatches += 1
                    TL.BlankLine()
                    TL.LogMessage("RegistryRights", "OK - SubKey " & SubKey & " does have full registry access rights for BUILTIN\Users")
                Else
                    LogError("RegistryRights", "Subkey " & SubKey & " does not have full access rights for BUILTIN\Users!")
                End If
            End If
            If (debugSwitch) Then TL.LogMessage("RegistryRightsDbg", "End of Try-Catch code")
        Catch ex As NullReferenceException
            LogException("RegistryRights", "The subkey: " & Key.Name & "\" & SubKey & " does not exist.")
        Catch ex As Exception
            LogException("RegistryRights", "Issue reading registry rights: " & ex.ToString)
        End Try
        If (debugSwitch) Then TL.LogMessage("RegistryRightsDbg", "Exited ReadRegistryRights")
        TL.BlankLine()
    End Sub

    ''' <summary>
    ''' Returns the localised text name of the BUILTIN\Users group. This varies by locale so has to be derrived on the users system.
    ''' </summary>
    ''' <returns>Localised name of the BUILTIN\Users group</returns>
    ''' <remarks>This uses the WMI features and is pretty obscure - sorry, it was the only way I could find to do this! Peter</remarks>
    Private Function GetBuiltInUsers() As String
        Dim Searcher As ManagementObjectSearcher
        Dim Group As String = "Unknown" ' Initialise to some values
        Dim Name As String = "Unknown"

        Try

            Searcher = New ManagementObjectSearcher(New ManagementScope("\\localhost\root\cimv2"),
                                                    New WqlObjectQuery("Select * From Win32_Account Where SID = 'S-1-5-32'"),
                                                    Nothing)

            For Each wmiClass In Searcher.Get
                Dim p As PropertyDataCollection
                p = wmiClass.Properties
                For Each pr In p
                    If pr.Name = "Name" Then Group = pr.Value
                Next
            Next
            Searcher.Dispose()
        Catch ex As Exception
            LogException("GetBuiltInUsers 1", ex.ToString)
        End Try

        Try
            Searcher = New ManagementObjectSearcher(New ManagementScope("\\localhost\root\cimv2"),
                                                    New WqlObjectQuery("Select * From Win32_Group Where SID = 'S-1-5-32-545'"),
                                                    Nothing)

            For Each wmiClass In Searcher.Get
                Dim p As PropertyDataCollection
                p = wmiClass.Properties
                For Each pr In p
                    If pr.Name = "Name" Then Name = pr.Value
                Next
            Next
            Searcher.Dispose()
        Catch ex As Exception
            LogException("GetBuiltInUsers 2", ex.ToString)
        End Try

        Return Group & "\" & Name
    End Function

    Sub ScanRegistry()
        Dim Key As RegistryKey
        Status("Scanning Registry")
        'TL.LogMessage("ScanRegistry", "Start")
        If OSBits() = Bitness.Bits64 Then
            Try
                'List the 32bit registry
                TL.LogMessage("ScanRegistry", "Machine Profile Root (64bit OS - 32bit Registry)")
                Key = ASCOMRegistryAccess.OpenSubKey(Registry.LocalMachine, REGISTRY_ROOT_KEY_NAME, False, RegistryAccess.RegWow64Options.KEY_WOW64_32KEY)
                RecursionLevel = -1
                RecurseRegistry(Key)
            Catch ex As Exception
                LogException("ScanRegistry", "Exception: " & ex.ToString)
            End Try
            TL.BlankLine()

            Try
                'List the 64bit registry
                TL.LogMessage("ScanRegistry", "Machine Profile Root (64bit OS - 64bit Registry)")
                Key = ASCOMRegistryAccess.OpenSubKey(Registry.LocalMachine, REGISTRY_ROOT_KEY_NAME, False, RegistryAccess.RegWow64Options.KEY_WOW64_64KEY)
                RecursionLevel = -1
                RecurseRegistry(Key)
            Catch ex As ProfilePersistenceException
                If InStr(ex.Message, "0x2") > 0 Then
                    TL.LogMessage("ScanRegistry", "Key not found")
                Else
                    LogException("ScanRegistry", "ProfilePersistenceException: " & ex.ToString)
                End If
            Catch ex As Exception
                LogException("ScanRegistry", "Exception: " & ex.ToString)
            End Try
        Else '32 bit OS
            Try
                'List the registry (only one view on a 32bit machine)
                TL.LogMessage("ScanRegistry", "Machine Profile Root (32bit OS)")
                Key = Registry.LocalMachine.OpenSubKey(REGISTRY_ROOT_KEY_NAME)
                RecursionLevel = -1
                RecurseRegistry(Key)
            Catch ex As Exception
                LogException("ScanRegistry", "Exception: " & ex.ToString)
            End Try
        End If
        TL.BlankLine()
        TL.BlankLine()

        Try
            'List the user registry
            TL.LogMessage("ScanRegistry", "User Profile Root")
            Key = Registry.CurrentUser.OpenSubKey(REGISTRY_ROOT_KEY_NAME)
            RecursionLevel = -1
            RecurseRegistry(Key)
        Catch ex As Exception
            LogException("ScanRegistry", "Exception: " & ex.ToString)
        End Try
        TL.BlankLine()
        TL.BlankLine()
    End Sub

    Sub RecurseRegistry(ByVal Key As RegistryKey)
        Dim ValueNames(), SubKeys(), DisplayName As String
        Try
            RecursionLevel += 1
            ValueNames = Key.GetValueNames
            For Each ValueName As String In ValueNames
                If ValueName = "" Then
                    DisplayName = "*** Default Value ***"
                Else
                    DisplayName = ValueName
                End If
                TL.LogMessage("Registry Profile", Space(RecursionLevel * 2) & "   " & DisplayName & " = " & Key.GetValue(ValueName))
            Next
        Catch ex As Exception
            LogException("RecurseRegistry 1", "Exception: " & ex.ToString)
        End Try
        Try
            SubKeys = Key.GetSubKeyNames
            For Each SubKey As String In SubKeys
                TL.BlankLine()
                TL.LogMessage("Registry Profile Key", Space(RecursionLevel * 2) & SubKey)
                RecurseRegistry(Key.OpenSubKey(SubKey))
            Next
        Catch ex As Exception
            LogException("RecurseRegistry 2", "Exception: " & ex.ToString)
        End Try
        RecursionLevel -= 1
    End Sub

    Sub ScanDrives()
        Dim Drives() As String, Drive As DriveInfo
        Try
            Status("Scanning drives")
            Drives = Directory.GetLogicalDrives
            For Each DriveName As String In Drives
                Drive = New DriveInfo(DriveName)
                If Drive.IsReady Then
                    TL.LogMessage("Drives", "Drive " & DriveName & " available space: " & Format(Drive.AvailableFreeSpace, "#,0.") & " bytes, capacity: " & Format(Drive.TotalSize, "#,0.") & " bytes, format: " & Drive.DriveFormat)
                Else
                    TL.LogMessage("Drives", "Skipping drive " & DriveName & " because it is not ready")
                End If
            Next
            TL.LogMessage("", "")
        Catch ex As Exception
            TL.LogMessageCrLf("ScanDrives", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub ScanProgramFiles()
        Dim BaseDir As String
        Dim PathShell As New System.Text.StringBuilder(260)
        Try
            BaseDir = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)

            Status("Scanning ProgramFiles Directory for Helper DLLs")
            TL.LogMessage("ProgramFiles Scan", "Searching for Helper.DLL etc.")

            RecurseProgramFiles(BaseDir) ' This is the 32bit path on a 32bit OS and 64bit path on a 64bit OS

            TL.BlankLine()

            'If on a 64bit OS, now scan the 32bit path

            If IntPtr.Size = 8 Then 'We are on a 64bit OS
                BaseDir = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
                BaseDir = SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILESX86, False)

                Status("Scanning ProgramFiles(x86) Directory for Helper DLLs")
                TL.LogMessage("ProgramFiles(x86) Scan", "Searching for Helper.DLL etc. on 32bit path")

                RecurseProgramFiles(PathShell.ToString) ' This is the 32bit path on a 32bit OS and 64bit path on a 64bit OS

                TL.BlankLine()
            End If
        Catch ex As Exception
            LogException("ScanProgramFiles", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub RecurseProgramFiles(ByVal Folder As String)
        Dim DirInfo As DirectoryInfo
        Dim FileInfos() As FileInfo
        Dim DirInfos() As DirectoryInfo
        Try
            DirInfo = New DirectoryInfo(Folder)
            Action(Microsoft.VisualBasic.Left(Folder, 70))

            Try ' Get file details for files in this folder
                FileInfos = DirInfo.GetFiles
                For Each MyFile As FileInfo In FileInfos
                    If MyFile.FullName.ToUpper.EndsWith("\HELPER.DLL") Then
                        FileDetails(Folder & "\", "Helper.dll")
                    End If
                    If MyFile.FullName.ToUpper.EndsWith("\HELPER2.DLL") Then
                        FileDetails(Folder & "\", "Helper2.dll")
                    End If
                Next
            Catch ex As UnauthorizedAccessException
                TL.LogMessage("RecurseProgramFiles 1", "UnauthorizedAccessException for directory; " & Folder)
            Catch ex As PathTooLongException
                TL.LogMessage("RecurseProgramFiles 1", "PathTooLongException in directory; " & Folder)
            Catch ex As Exception
                LogException("RecurseProgramFiles 1", "Exception: " & ex.ToString)
            End Try

            Try ' Iterate over the sub directories of this folder
                DirInfos = DirInfo.GetDirectories
                For Each Directory As DirectoryInfo In DirInfos
                    RecurseProgramFiles(Directory.FullName) ' Recursively process this sub directory
                Next
            Catch ex As UnauthorizedAccessException
                TL.LogMessage("RecurseProgramFiles 2", "UnauthorizedAccessException for directory; " & Folder)
            Catch ex As PathTooLongException
                TL.LogMessage("RecurseProgramFiles 2", "PathTooLongException in directory; " & Folder)
            Catch ex As Exception
                LogException("RecurseProgramFiles 2", "Exception: " & ex.ToString)
            End Try

            Action("")
        Catch ex As UnauthorizedAccessException
            TL.LogMessage("RecurseProgramFiles 3", "UnauthorizedAccessException for directory; " & Folder)
        Catch ex As PathTooLongException
            TL.LogMessage("RecurseProgramFiles 3", "PathTooLongException in directory; " & Folder)
        Catch ex As Exception
            LogException("RecurseProgramFiles 3", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub ScanProfile55Files()
        Dim ProfileStore As AllUsersFileSystemProvider, Files() As String
        Try
            Status("Scanning Profile 5.5 Files")
            TL.LogMessage("Scanning Profile 5.5", "")

            ProfileStore = New AllUsersFileSystemProvider
            Files = Directory.GetFiles(ProfileStore.BasePath) 'Check that directory exists

            RecurseProfile55Files(ProfileStore.BasePath)

            TL.BlankLine()
        Catch ex As DirectoryNotFoundException
            TL.LogMessage("ScanProfileFiles", "Profile 5.5 filestore not present")
            TL.BlankLine()
        Catch ex As Exception
            LogException("ScanProfileFiles", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub RecurseProfile55Files(ByVal Folder As String)
        Dim Files(), Directories() As String

        Try
            'TL.LogMessage("Folder", Folder)
            'Process files in this directory
            Files = Directory.GetFiles(Folder)
            For Each MyFile As String In Files
                TL.LogMessage("File", MyFile)
                Using sr As StreamReader = File.OpenText(MyFile)
                    Dim input As String
                    input = sr.ReadLine()
                    While Not input Is Nothing
                        TL.LogMessage("", "  " & input)
                        input = sr.ReadLine()
                    End While
                    Console.WriteLine("The end of the stream has been reached.")
                End Using

            Next
        Catch ex As Exception
            LogException("RecurseProfileFiles 1", "Exception: " & ex.ToString)
        End Try

        Try
            Directories = Directory.GetDirectories(Folder)
            For Each Directory As String In Directories
                TL.LogMessage("Directory", Directory)
                RecurseProfile55Files(Directory)
            Next
        Catch ex As Exception
            LogException("RecurseProfileFiles 2", "Exception: " & ex.ToString)
        End Try

    End Sub

    Sub ScanFrameworks()
        Dim FrameworkPath, FrameworkFile, FrameworkDirectories() As String
        Dim PathShell As New System.Text.StringBuilder(260)

        Try
            Status("Scanning Frameworks")

            SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_WINDOWS, False)
            FrameworkPath = PathShell.ToString & "\Microsoft.NET\Framework"

            FrameworkDirectories = Directory.GetDirectories(FrameworkPath)
            For Each Directory As String In FrameworkDirectories
                FrameworkFile = Directory & "\mscorlib.dll"
                Dim FVInfo As FileVersionInfo, FInfo As FileInfo
                If File.Exists(FrameworkFile) Then

                    FVInfo = FileVersionInfo.GetVersionInfo(FrameworkFile)
                    FInfo = Microsoft.VisualBasic.FileIO.FileSystem.GetFileInfo(FrameworkFile)

                    TL.LogMessage("Frameworks", Directory.ToString & " - Version: " & FVInfo.FileMajorPart & "." & FVInfo.FileMinorPart & " " & FVInfo.FileBuildPart & " " & FVInfo.FilePrivatePart)

                Else
                    TL.LogMessage("Frameworks", Directory.ToString)
                End If
            Next
            TL.BlankLine()
        Catch ex As Exception
            LogException("Frameworks", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub ScanPlatform6Logs()

        Dim fileList() As String
        Dim setupFiles As SortedList(Of DateTime, String) = New SortedList(Of DateTime, String)
        Dim streamReader As StreamReader = Nothing
        Dim fileInfo As FileInfo

        Try
            Status("Scanning Platform 6 install logs")
            TL.LogMessage("ScanPlatform6Logs", "Starting scan")

            'Get a list of setup files in the ASCOM directory and sub directories in creation date order
            fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\ASCOM", "ASCOMPlatform6Install*.txt", SearchOption.TopDirectoryOnly)
            For Each foundFile In fileList
                fileInfo = New FileInfo(foundFile)
                setupFiles.Add(fileInfo.CreationTime, foundFile)
            Next

            fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\ASCOM", "ASCOM.UninstallASCOM.*.txt", SearchOption.AllDirectories)
            For Each foundFile In fileList
                fileInfo = New FileInfo(foundFile)
                setupFiles.Add(fileInfo.CreationTime, foundFile)
            Next

            fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\ASCOM", "ASCOM.FinaliseInstall.*.txt", SearchOption.AllDirectories)
            For Each foundFile In fileList
                fileInfo = New FileInfo(foundFile)
                setupFiles.Add(fileInfo.CreationTime, foundFile)
            Next

            TL.LogMessage("ScanPlatform6Logs", "Found the following installation logs:")
            For Each foundFile In setupFiles
                TL.LogMessage("ScanPlatform6Logs", String.Format("  Date: {0} Log: {1}", foundFile.Key.ToString("dd MMM yyyy HH:mm:ss"), foundFile.Value))
            Next

            TL.BlankLine()

            For Each foundFile In setupFiles 'Iterate over results
                Try
                    fileInfo = New FileInfo(foundFile.Value)
                    TL.LogMessage("InstallLog Found", String.Format("{0}, Created: {1}, Last updated: {2}", foundFile.Value, fileInfo.CreationTime.ToString("dd MMM yyyy HH:mm:ss"), fileInfo.LastWriteTime.ToString("dd MMM yyyy HH:mm:ss")))

                    streamReader = File.OpenText(foundFile.Value)

                    Do Until streamReader.EndOfStream 'include the file
                        TL.LogMessage("InstallLog", streamReader.ReadLine())
                    Loop

                    TL.LogMessage("", "")
                    streamReader.Close()
                    streamReader.Dispose()
                    streamReader = Nothing
                Catch ex1 As Exception
                    LogException("ScanPlatform6Logs", "Exception 1: " & ex1.ToString)
                    If Not (streamReader Is Nothing) Then 'Clean up streamreader
                        streamReader.Close()
                        streamReader.Dispose()
                        streamReader = Nothing
                    End If
                End Try
            Next
            TL.BlankLine()

            Status("Scanning Migration logs")
            TL.LogMessage("ScanMigrationLogs", "Starting scan")
            'Get an array of setup and uninstall filenames from the Temp directory
            fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\ASCOM", "ASCOM.ProfileMigrationLog*.txt", SearchOption.TopDirectoryOnly)

            For Each tempFile As String In fileList 'Iterate over results
                Try
                    TL.LogMessage("MigrationLog Found", tempFile)
                    fileInfo = New FileInfo(tempFile)
                    TL.LogMessage("InstallLog", "File created: " & fileInfo.CreationTime.ToString("dd MMM yyyy hh:mm:ss"))
                    TL.LogMessage("InstallLog", "File last updated: " & fileInfo.LastWriteTime.ToString("dd MMM yyyy hh:mm:ss"))
                    streamReader = File.OpenText(tempFile)

                    Do Until streamReader.EndOfStream 'include the file
                        TL.LogMessage("MigrationLog", streamReader.ReadLine())
                    Loop

                    TL.LogMessage("", "")
                    streamReader.Close()
                    streamReader.Dispose()
                    streamReader = Nothing
                Catch ex2 As Exception
                    LogException("ScanPlatform6Logs", "Exception 2: " & ex2.ToString)
                    If Not (streamReader Is Nothing) Then 'Clean up streamreader
                        streamReader.Close()
                        streamReader.Dispose()
                        streamReader = Nothing
                    End If
                End Try
            Next

            TL.LogMessage("ScanPlatform6Logs", "Completed scan")
            TL.BlankLine()
            TL.BlankLine()
        Catch ex As Exception
            LogException("ScanPlatform6Logs", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub ScanLogs()
        Const NumLine As Integer = 30 'Number of lines to read from file to see if it is an ASCOM log

        Dim TempFiles() As String
        Dim SetupFiles(), UninstallFiles() As String
        Dim TempFilesList As New List(Of String)
        Dim SR As StreamReader = Nothing
        Dim Lines(30) As String, LineCount As Integer = 0
        Dim ASCOMFile As Boolean

        Try
            Status("Scanning setup logs")
            TL.LogMessage("SetupFile", "Starting scan")
            'Get an array of setup and uninstall filenames from the Temp directory
            SetupFiles = Directory.GetFiles(Path.GetFullPath(Environment.GetEnvironmentVariable("Temp")), "Setup Log*.txt", SearchOption.TopDirectoryOnly)
            UninstallFiles = Directory.GetFiles(Path.GetFullPath(Environment.GetEnvironmentVariable("Temp")), "Uninstall Log*.txt", SearchOption.TopDirectoryOnly)
            TempFilesList.AddRange(SetupFiles)
            TempFilesList.AddRange(UninstallFiles)
            TempFiles = TempFilesList.ToArray

            For Each TempFile As String In TempFiles 'Iterate over results
                Try
                    TL.LogMessage("SetupFile", TempFile)
                    SR = File.OpenText(TempFile)

                    'Search for word ASCOM in first part of file
                    ASCOMFile = False 'Initialise found flag
                    LineCount = 0
                    Array.Clear(Lines, 1, NumLine) 'Clear out the array ready for next run
                    Do Until (LineCount = NumLine) Or SR.EndOfStream
                        LineCount += 1
                        Lines(LineCount) = SR.ReadLine
                        If InStr(Lines(LineCount).ToUpper, "ASCOM") > 0 Then ASCOMFile = True
                        If InStr(Lines(LineCount).ToUpper, "CONFORM") > 0 Then ASCOMFile = True
                    Loop

                    If ASCOMFile Then 'This is an ASCOM setup so list it

                        For i = 1 To NumLine 'Include the lines read earlier
                            TL.LogMessage("SetupFile", Lines(i))
                        Next

                        Do Until SR.EndOfStream 'include the rest of the file
                            TL.LogMessage("SetupFile", SR.ReadLine())
                        Loop
                    End If
                    TL.LogMessage("", "")
                    SR.Close()
                    SR.Dispose()
                    SR = Nothing
                Catch ex1 As Exception
                    LogException("SetupFile", "Exception 1: " & ex1.ToString)
                    If Not (SR Is Nothing) Then 'Clean up streamreader
                        SR.Close()
                        SR.Dispose()
                        SR = Nothing
                    End If
                End Try
            Next
            TL.BlankLine()
            TL.LogMessage("SetupFile", "Completed scan")
            TL.BlankLine()
        Catch ex2 As Exception
            LogException("SetupFile", "Exception 2: " & ex2.ToString)
        End Try
    End Sub

    Sub ScanCOMRegistration()
        Try
            Status("Scanning Registry")
            TL.LogMessage("COMRegistration", "") 'Report COM registation

            'Original Platform 5 helpers
            GetCOMRegistration("DriverHelper.Chooser")
            GetCOMRegistration("DriverHelper.Profile")
            GetCOMRegistration("DriverHelper.Serial")
            GetCOMRegistration("DriverHelper.Timer")
            GetCOMRegistration("DriverHelper.Util")
            GetCOMRegistration("DriverHelper2.Util")

            'Platform 5 helper support components
            GetCOMRegistration("DriverHelper.ChooserSupport")
            GetCOMRegistration("DriverHelper.ProfileAccess")
            GetCOMRegistration("DriverHelper.SerialSupport")

            'Utlities
            GetCOMRegistration("ASCOM.Utilities.ASCOMProfile")
            GetCOMRegistration("ASCOM.Utilities.Chooser")
            GetCOMRegistration("ASCOM.Utilities.KeyValuePair")
            GetCOMRegistration("ASCOM.Utilities.Profile")
            GetCOMRegistration("ASCOM.Utilities.Serial")
            GetCOMRegistration("ASCOM.Utilities.Timer")
            GetCOMRegistration("ASCOM.Utilities.TraceLogger")
            GetCOMRegistration("ASCOM.Utilities.Util")

            'Astrometry
            GetCOMRegistration("ASCOM.Astrometry.Kepler.Ephemeris")
            GetCOMRegistration("ASCOM.Astrometry.NOVAS.NOVAS2COM")
            GetCOMRegistration("ASCOM.Astrometry.NOVAS.NOVAS3")
            GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.Earth")
            GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.Planet")
            GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.PositionVector")
            GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.Site")
            GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.Star")
            GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.VelocityVector")
            GetCOMRegistration("ASCOM.Astrometry.Transform.Transform")

            'Get COM registration for all registered devices 
            Dim PR As New Profile
            Dim DeviceTypes As ArrayList, Devices As ArrayList
            DeviceTypes = PR.RegisteredDeviceTypes
            TL.LogMessage("DriverCOMRegistration", "Start of process")

            For Each DeviceType As String In DeviceTypes
                TL.LogMessage("DriverCOMRegistration", "Starting " & DeviceType & " device type")
                Devices = PR.RegisteredDevices(DeviceType)
                For Each Device As ASCOM.Utilities.KeyValuePair In Devices
                    GetCOMRegistration(Device.Key)
                Next
                TL.BlankLine()

            Next
            TL.LogMessage("DriverCOMRegistration", "Completed process")
            PR.Dispose()
            PR = Nothing

            'Exceptions
            GetCOMRegistration("ASCOM.DriverException")
            GetCOMRegistration("ASCOM.InvalidOperationException")
            GetCOMRegistration("ASCOM.InvalidValueException")
            GetCOMRegistration("ASCOM.MethodNotImplementedException")
            GetCOMRegistration("ASCOM.NotConnectedException")
            GetCOMRegistration("ASCOM.NotImplementedException")
            GetCOMRegistration("ASCOM.ParkedException")
            GetCOMRegistration("ASCOM.PropertyNotImplementedException")
            GetCOMRegistration("ASCOM.SlavedException")
            GetCOMRegistration("ASCOM.ValueNotSetException")

            TL.LogMessage("", "")
        Catch ex As Exception
            LogException("ScanCOMRegistration", "Exception: " & ex.ToString)
        End Try
    End Sub

    Private Sub ScanForHelperHijacking()
        'Scan files on 32 and 64bit systems
        TL.LogMessage("HelperHijacking", "")
        CheckCOMRegistration("DriverHelper.Chooser", "Helper.dll", Bitness.Bits32)
        CheckCOMRegistration("DriverHelper.Profile", "Helper.dll", Bitness.Bits32)
        CheckCOMRegistration("DriverHelper.Serial", "Helper.dll", Bitness.Bits32)
        CheckCOMRegistration("DriverHelper.Timer", "Helper.dll", Bitness.Bits32)
        CheckCOMRegistration("DriverHelper.Util", "Helper.dll", Bitness.Bits32)
        CheckCOMRegistration("DriverHelper2.Util", "Helper2.dll", Bitness.Bits32)
        TL.BlankLine()
        TL.BlankLine()
    End Sub

    Private Sub CheckCOMRegistration(ByVal ProgID As String, ByVal COMFile As String, ByVal Bitness As Bitness)
        Dim RKeyProgID, RKeyCLSIDValue, RKeyCLSID, RKeyInprocServer32 As RegistryKey, CLSID, InprocServer32, ASCOMPath As String
        Dim RegAccess As New RegistryAccess
        Dim PathShell As New System.Text.StringBuilder(260), RegSvr32Path As String
        Dim P As Process, Info As ProcessStartInfo
        Dim Result As MsgBoxResult

        Try
            RKeyProgID = Registry.ClassesRoot.OpenSubKey(ProgID)
            If Not RKeyProgID Is Nothing Then ' Found ProgId
                RKeyCLSIDValue = RKeyProgID.OpenSubKey("CLSID")
                If Not RKeyCLSIDValue Is Nothing Then 'Found CLSID Key
                    CLSID = RKeyCLSIDValue.GetValue("").ToString 'Get the CLSID

                    Select Case (ApplicationBits())
                        Case VersionCode.Bitness.Bits32 ' We are a 32bit application so look in the default registry position
                            RKeyCLSID = Registry.ClassesRoot.OpenSubKey("CLSID\" & CLSID, False)
                        Case VersionCode.Bitness.Bits64 ' We are a 64bit application so look in the 32bit registry section
                            Select Case Bitness
                                Case VersionCode.Bitness.Bits32 ' Open the 32bit registry
                                    RKeyCLSID = RegAccess.OpenSubKey(Registry.ClassesRoot, "CLSID\" & CLSID, False, RegistryAccess.RegWow64Options.KEY_WOW64_32KEY)
                                Case VersionCode.Bitness.Bits64 'Open the 64bit registry
                                    RKeyCLSID = Registry.ClassesRoot.OpenSubKey("CLSID\" & CLSID, False)
                                Case Else
                                    RKeyCLSID = Nothing
                                    Compare("HelperHijacking", "Requested Bitness", ApplicationBits.ToString, Bitness.Bits64.ToString)
                            End Select
                        Case Else
                            Compare("HelperHijacking", "Requested Bitness", ApplicationBits.ToString, Bitness.Bits64.ToString)
                            RKeyCLSID = Nothing
                    End Select
                    If Not RKeyCLSID Is Nothing Then ' CLSID value does exist
                        RKeyInprocServer32 = RKeyCLSID.OpenSubKey("InprocServer32", False)
                        If Not RKeyInprocServer32 Is Nothing Then
                            InprocServer32 = RKeyInprocServer32.GetValue("", False)
                            ASCOMPath = GetASCOMPath() & COMFile
                            If ASCOMPath <> InprocServer32 Then ' We have a hijacked COM registration so offer to re-register the correct file

                                LogEvent("Diagnostics:HelperHijacking", "Hijacked COM Setting for: " & ProgID & ", Actual Path: " & InprocServer32 & ", Expected Path: " & ASCOMPath, EventLogEntryType.Error, EventLogErrors.DiagnosticsHijackedCOMRegistration, "")
                                TL.LogMessage("HelperHijacking", "ISSUE, " & ProgID & " has been hijacked")
                                TL.LogMessage("HelperHijacking", "  Actual Path: " & InprocServer32 & ", Expected Path: " & ASCOMPath)
                                Result = MsgBox("The COM component """ & ProgID & """ is not properly registered. Would you like to fix this? (Strongly recommend Yes!)", MsgBoxStyle.YesNo Or MsgBoxStyle.Critical, "COM Registration Issue Detected")

                                If Result = MsgBoxResult.Yes Then
                                    TL.LogMessage("HelperHijacking", "  Fixing COM Registration")
                                    If OSBits() = VersionCode.Bitness.Bits64 Then ' We are running on a 64bit OS
                                        Select Case Bitness
                                            Case VersionCode.Bitness.Bits32 ' Run the 32bit Regedit
                                                SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_SYSTEMX86, False) ' Get the 32bit system directory
                                            Case VersionCode.Bitness.Bits64 ' Run the 64bit Regedit
                                                SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_SYSTEM, False) ' Get the 64bit system directory
                                        End Select
                                    Else ' We are running on a 32bit OS
                                        SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_SYSTEM, False) ' Get the system directory
                                    End If
                                    RegSvr32Path = PathShell.ToString & "\RegSvr32.exe" 'Construct the full path to RegSvr32.exe
                                    Info = New ProcessStartInfo
                                    Info.FileName = RegSvr32Path 'Populate the ProcessStartInfo with the full path to RegSvr32.exe 
                                    Info.Arguments = "/s """ & ASCOMPath & """" ' And the start parameter specifying the file to COM register
                                    TL.LogMessage("HelperHijacking", "  RegSvr32 Path: """ & RegSvr32Path & """, COM Path: """ & ASCOMPath & """")

                                    P = New Process ' Create the process
                                    P.StartInfo = Info ' Set the start info
                                    P.Start() 'Start the process and wait for it to finish
                                    TL.LogMessage("HelperHijacking", "  Started registration")
                                    P.WaitForExit()
                                    TL.LogMessage("HelperHijacking", "  Finished registration, Return code: " & P.ExitCode) ' ASCOM-217 changed + to & concatenator
                                    P.Dispose()

                                    'Reread the COM information to check whether it is now fixed
                                    InprocServer32 = RKeyInprocServer32.GetValue("", False)
                                    ASCOMPath = GetASCOMPath() & COMFile
                                    If ASCOMPath <> InprocServer32 Then ' We have a hijacked COM registration so offer to re-register the correct file
                                        MsgBox("Diagnostics was NOT able to fix the issue. Please report this on ASCOM-Talk", MsgBoxStyle.Exclamation, "Issue Remains")
                                        LogError("HelperHijacking", "  Unable to fix " & ProgID & " registration")
                                    Else
                                        MsgBox("Diagnostics successfully FIXED the issue", MsgBoxStyle.Information, "Issue Fixed")
                                        TL.LogMessage("HelperHijacking", "  Successfully fixed " & ProgID & " registration")
                                        NMatches += 1
                                    End If
                                Else
                                    TL.LogMessage("HelperHijacking", "  Not fixing COM registration, no action taken")
                                End If
                            Else ' Matches expected value so has not been hijacked
                                TL.LogMessage("HelperHijacking", "OK, " & ProgID & " has not been hijacked")
                            End If
                        Else
                            LogError("HelperHijacking", "Unable to find registered CLSID\InprocServer32: " + CLSID & "InprocServer32")
                        End If
                    Else 'CLSID value dfoes not exist
                        LogError("HelperHijacking", "Unable to find registered CLSID: " + CLSID)
                    End If
                Else 'CLSID is missing
                    LogError("HelperHijacking", "Unable to find ProgID\CLSID: " + ProgID & "\CLSID")
                End If
            Else ' Cannot find ProgID so gve error message
                LogError("HelperHijacking", "Unable to find registered ProgID: " + ProgID)
            End If
        Catch ex As Exception
            LogException("HelperHijacking", "Exception: " & ex.ToString)
        End Try

    End Sub

    Private Sub LogError(ByVal Section As String, ByVal Message As String)
        NNonMatches += 1
        ErrorList.Add(Section & " - " & Message)
        TL.LogMessage(Section, Message)
    End Sub

    Sub ScanGac()
        Dim ae As IAssemblyEnum
        Dim an As IAssemblyName = Nothing
        Dim ass As Assembly
        Dim AssemblyNames As Generic.SortedList(Of String, String)
        Dim assname As AssemblyName
        Dim AscomGACPaths As Generic.List(Of String)
        Dim FVInfo As FileVersionInfo = Nothing
        Dim MyName, FVer As String
        Dim FileLocation As String
        Dim FileVer As FileVersionInfo
        Dim DiagnosticsVersion As New Version("0.0.0.0")
        Dim FileVersion As New Version("0.0.0.0")
        Dim VersionComparison As Integer
        Dim assemblyURI As Uri = Nothing
        Dim localPath As String

        Try
            Status("Scanning Assemblies")
            AssemblyNames = New Generic.SortedList(Of String, String)

            TL.LogMessage("Assemblies", "Assemblies registered in the GAC")
            ae = AssemblyCache.CreateGACEnum ' Get an enumerator for the GAC assemblies

            Do While (AssemblyCache.GetNextAssembly(ae, an) = 0) 'Enumerate the assemblies
                Try
                    assname = GetAssemblyName(an)
                    AssemblyNames.Add(assname.FullName, assname.Name) 'Convert the fusion representation to a standard AssemblyName and get its full name
                Catch ex As Exception
                    'Ignore an exceptions here due to duplicate names, these are all MS assemblies
                End Try
            Loop

            AscomGACPaths = New Generic.List(Of String)

            For Each AssemblyName As Generic.KeyValuePair(Of String, String) In AssemblyNames ' Process each assembly in turn
                Try
                    If InStr(AssemblyName.Key, "ASCOM") > 0 Then 'Extra information for ASCOM files
                        TL.LogMessage("Assemblies", AssemblyName.Value)
                        ass = Assembly.Load(AssemblyName.Key)
                        AssemblyInfo(TL, AssemblyName.Value, ass) ' Get file version and other information

                        Try
                            assemblyURI = New Uri(ass.GetName.CodeBase)
                            localPath = assemblyURI.LocalPath
                            If (localPath.ToUpper.Contains("\ASCOM.DRIVERACCESS\6") Or
                                localPath.ToUpper.Contains("\ASCOM.UTILITIES\6") Or
                                localPath.ToUpper.Contains("\ASCOM.ASTROMETRY\6") Or
                                localPath.ToUpper.Contains("\ASCOM.DEVICEINTERFACES\6")) Then
                                AscomGACPaths.Add(localPath)
                            End If
                            FileDetails(Path.GetDirectoryName(localPath) & "\", Path.GetFileName(localPath))
                        Catch ex As Exception

                        End Try

                    Else
                        TL.LogMessage("Assemblies", AssemblyName.Key) ' Non-ASCOM assembly
                    End If
                Catch ex As Exception
                    LogException("Assemblies", "Exception 2: " & ex.ToString)
                End Try
            Next

            Try
                ass = Assembly.GetExecutingAssembly
                MyName = ass.FullName
                FileLocation = ass.Location
                If String.IsNullOrEmpty(FileLocation) Then
                    TL.LogMessage("FileDetails", MyName & "Assembly location is missing, cannot determine file version")
                Else
                    FileVer = FileVersionInfo.GetVersionInfo(FileLocation)
                    If FileVer Is Nothing Then
                        TL.LogMessage("FileDetails", MyName & " File version object is null, cannot determine file version number")
                    Else
                        FVer = FileVer.FileVersion
                        If Not String.IsNullOrEmpty(FVer) Then
                            DiagnosticsVersion = New Version(FVer)
                        Else
                            TL.LogMessage("FileDetails", MyName & " File version string is null or empty, cannot determine file version")
                        End If
                    End If
                End If
            Catch ex As Exception
                LogException("FileDetails", "Exception EX2: " & ex.ToString)
            End Try
            TL.LogMessage("", "")

            For Each AscomGACPath As String In AscomGACPaths
                Try
                    FVInfo = FileVersionInfo.GetVersionInfo(AscomGACPath)
                    TL.LogMessage("GACFileVersion", FVInfo.FileName)
                    FileVersion = New Version(FVInfo.FileVersion)
                    VersionComparison = DiagnosticsVersion.CompareTo(FileVersion)

                    Select Case VersionComparison
                        Case Is < 0
                            TL.LogMessage("GACFileVersion", "   ##### Diagnostics is older than this assembly! Diagnostics: " & DiagnosticsVersion.ToString & ", Assembly: " & FileVersion.ToString)
                            NNonMatches += 1
                            ErrorList.Add("Diagnostics is older than this assembly! Diagnostics: " & DiagnosticsVersion.ToString & ", Assembly: " & FileVersion.ToString & " " & AscomGACPath)
                        Case 0
                            TL.LogMessage("GACFileVersion", "   Diagnostics is the same version as this assembly! Diagnostics: " & DiagnosticsVersion.ToString & ", Assembly: " & FileVersion.ToString)
                            NMatches += 1
                        Case Is > 0
                            TL.LogMessage("GACFileVersion", "   ##### Diagnostics is newer than this assembly! Diagnostics: " & DiagnosticsVersion.ToString & ", Assembly: " & FileVersion.ToString)
                            NNonMatches += 1
                            ErrorList.Add("Diagnostics is newer than this assembly! Diagnostics: " & DiagnosticsVersion.ToString & ", Assembly: " & FileVersion.ToString & " " & AscomGACPath)
                    End Select
                Catch ex As FormatException
                    LogException("GACFileVersion", "  ##### File version is in an invalid format: " & FVInfo.FileVersion)
                End Try
            Next

            TL.LogMessage("", "")
        Catch ex As Exception
            LogException("ScanGac", "Exception: " & ex.ToString)
        End Try
    End Sub

    Private Function GetAssemblyName(ByVal nameRef As IAssemblyName) As AssemblyName
        Dim AssName As New AssemblyName()
        Try
            AssName.Name = AssemblyCache.GetName(nameRef)
            AssName.Version = AssemblyCache.GetVersion(nameRef)
            AssName.CultureInfo = AssemblyCache.GetCulture(nameRef)
            AssName.SetPublicKeyToken(AssemblyCache.GetPublicKeyToken(nameRef))
        Catch ex As Exception
            LogException("GetAssemblyName", "Exception: " & ex.ToString)
        End Try
        Return AssName
    End Function

    Sub ScanDeveloperFiles()
        Dim ASCOMPath As String = "C:\Program Files\ASCOM\Platform 6 Developer Components\" ' Default location
        Dim PathShell As New System.Text.StringBuilder(260)
        Dim ASCOMPathComponents5, ASCOMPathComponents55, ASCOMPathComponents6, ASCOMPathDocs, ASCOMPathInstallerGenerator, ASCOMPathResources As String

        Try
            Status("Scanning Developer Files")

            If System.IntPtr.Size = 8 Then 'We are on a 64bit OS so look in the 32bit locations for files
                SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILESX86, False)
                ASCOMPath = PathShell.ToString & "\ASCOM\Platform 6 Developer Components\"
            Else '32bit system so look in the normal Program Files place
                ASCOMPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) & "\ASCOM\Platform 6 Developer Components\"""
            End If
        Catch ex As ProfilePersistenceException
            If InStr(ex.Message, "0x2") > 0 Then
                TL.LogMessage("Developer Files", "Installation registry key not present")
            Else
                LogException("ScanDeveloperFiles", "ProfilePersistenceException: " & ex.ToString)
            End If
        Catch ex As Exception
            LogException("ScanDeveloperFiles", "Registry Exception: " & ex.ToString)
        End Try

        Try

            If Directory.Exists(ASCOMPath) Then
                ASCOMPathComponents5 = ASCOMPath & "Components\Platform5\"
                ASCOMPathComponents55 = ASCOMPath & "Components\Platform55\"
                ASCOMPathComponents6 = ASCOMPath & "Components\Platform6\"
                ASCOMPathDocs = ASCOMPath & "Developer Documentation\"
                ASCOMPathInstallerGenerator = ASCOMPath & "Installer Generator\"
                ASCOMPathResources = ASCOMPath & "Installer Generator\Resources\"
                TL.LogMessage("Developer Files", "Start of scan")
                FileDetails(ASCOMPathComponents5, "ASCOM.Exceptions.dll")
                FileDetails(ASCOMPathComponents55, "ASCOM.Astrometry.dll")
                FileDetails(ASCOMPathComponents55, "ASCOM.Attributes.dll")
                FileDetails(ASCOMPathComponents55, "ASCOM.DriverAccess.dll")
                FileDetails(ASCOMPathComponents55, "ASCOM.Exceptions.dll")
                FileDetails(ASCOMPathComponents55, "ASCOM.Utilities.dll")
                FileDetails(ASCOMPathComponents6, "ASCOM.Astrometry.dll")
                FileDetails(ASCOMPathComponents6, "ASCOM.Attributes.dll")
                FileDetails(ASCOMPathComponents6, "ASCOM.Controls.dll")
                FileDetails(ASCOMPathComponents6, "ASCOM.DeviceInterfaces.dll")
                FileDetails(ASCOMPathComponents6, "ASCOM.DriverAccess.dll")
                FileDetails(ASCOMPathComponents6, "ASCOM.Exceptions.dll")
                FileDetails(ASCOMPathComponents6, "ASCOM.Internal.Extensions.dll")
                FileDetails(ASCOMPathComponents6, "ASCOM.SettingsProvider.dll")
                FileDetails(ASCOMPathComponents6, "ASCOM.Utilities.dll")
                FileDetails(ASCOMPathDocs, "Algorithms.pdf")
                FileDetails(ASCOMPathDocs, "Bug72T-sm.jpg")
                FileDetails(ASCOMPathDocs, "DriverInstallers.html")
                FileDetails(ASCOMPathDocs, "NOVAS_C3.0_Guide.pdf")
                FileDetails(ASCOMPathDocs, "Platform 6 Client-Driver Interaction V2.pdf")
                FileDetails(ASCOMPathDocs, "Platform Evolution.pdf")
                FileDetails(ASCOMPathDocs, "PlatformDeveloperHelp.chm")
                FileDetails(ASCOMPathDocs, "Script56.chm")
                If File.Exists(ASCOMPathDocs & "Templates.html") Then FileDetails(ASCOMPathDocs, "Templates.html")
                FileDetails(ASCOMPathDocs, "tip.gif")
                FileDetails(ASCOMPathDocs, "wsh-56.chm")
                FileDetails(ASCOMPathInstallerGenerator, "InstallerGen.exe")
                FileDetails(ASCOMPathInstallerGenerator, "InstallerGen.pdb")
                FileDetails(ASCOMPathInstallerGenerator, "Microsoft.Samples.WinForms.Extras.dll")
                FileDetails(ASCOMPathInstallerGenerator, "Microsoft.Samples.WinForms.Extras.pdb")
                FileDetails(ASCOMPathInstallerGenerator, "TemplateSubstitutionParameters.txt")
                FileDetails(ASCOMPathResources, "CreativeCommons.txt")
                FileDetails(ASCOMPathResources, "DriverInstallTemplate.iss")
                FileDetails(ASCOMPathResources, "WizardImage.bmp")
            Else
                TL.LogMessage("Developer Files", "Files not installed")
            End If
        Catch ex As Exception
            LogException("ScanDeveloperFiles", "File Exception: " & ex.ToString)
        End Try
        TL.BlankLine()
        Status("")

    End Sub

    Sub ScanPlatformFiles(ByVal ASCOMPath As String)
        Try
            Status("Scanning Platform Files")

            ScanDirectory(ASCOMPath, SearchOption.TopDirectoryOnly)
            ScanDirectory(ASCOMPath & ".net", SearchOption.AllDirectories)
            ScanDirectory(ASCOMPath & "Interface", SearchOption.AllDirectories)
            ScanDirectory(ASCOMPath & "Platform", SearchOption.AllDirectories)
            ScanDirectory(ASCOMPath & "Astrometry", SearchOption.AllDirectories)
            ScanDirectory(ASCOMPath & "VideoUtilities", SearchOption.AllDirectories)

        Catch ex As Exception
            LogException("ScanFiles", "Exception: " & ex.ToString)
        End Try
        TL.BlankLine()

    End Sub

    Private Sub ScanDirectory(DirectoryName As String, Search As SearchOption)
        Dim FullFileNames() As String

        TL.LogMessage("ScanFiles", "Scanning directory: " & DirectoryName & ", " & Search.ToString)
        Status("Scanning directory " & DirectoryName)

        Try
            FullFileNames = Directory.GetFiles(DirectoryName, "*.*", Search)
            For Each FullFileName As String In FullFileNames
                FileDetails(Path.GetDirectoryName(FullFileName) & Path.DirectorySeparatorChar, Path.GetFileName(FullFileName))
            Next
        Catch ex As Exception
            LogException("ScanDirectory", "Exception: " & ex.ToString)
        End Try
        TL.BlankLine()
    End Sub

    Sub FileDetails(ByVal FPath As String, ByVal FName As String)
        Dim FullPath As String
        Dim Att As FileAttributes, FVInfo As FileVersionInfo, FInfo As FileInfo
        Dim Ass As Assembly, AssVer As String = "", CompareName As String
        Dim ReflectionAssemblies() As Assembly = Nothing
        Dim Framework As String = "", PE As PEReader

        Try
            FullPath = FPath & FName 'Create full filename from path and simple filename
            If File.Exists(FullPath) Then
                TL.LogMessage("FileDetails", FullPath)
                'Try to get assembly version info if present
                Try
                    Ass = Assembly.ReflectionOnlyLoadFrom(FullPath)
                    AssVer = Ass.FullName
                    Framework = Ass.ImageRuntimeVersion
                Catch ex As FileLoadException ' Deal with possibility that this assembly has already been loaded
                    ReflectionAssemblies = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies() ' Get list of assemblies already loaded to the reflection only context
                    CompareName = Path.GetFileNameWithoutExtension(FName)
                    For Each ReflectionAss As Assembly In ReflectionAssemblies ' Find the assembly already there and get its full name
                        If ReflectionAss.FullName.ToUpper.Contains(CompareName.ToUpper) Then
                            AssVer = ReflectionAss.FullName
                        End If
                    Next
                    If String.IsNullOrEmpty(AssVer) Then
                        TL.LogMessage("ErrorDiagnosticsCmp", CompareName)
                        For Each ReflectionAss As Assembly In ReflectionAssemblies ' List the assemblies already loaded
                            TL.LogMessage("ErrorDiagnostics", ReflectionAss.FullName)
                        Next
                        LogException("FileDetails", "FileLoadException: " & ex.ToString)
                    End If
                Catch ex As BadImageFormatException
                    AssVer = "Not an assembly"
                Catch ex As Exception
                    LogException("FileDetails", "Exception: " & ex.ToString)
                    AssVer = "Not an assembly: " & ex.ToString
                End Try

                TL.LogMessage("FileDetails", "   Assembly Version:   " & AssVer)
                TL.LogMessage("FileDetails", "   Assembly Framework: " & Framework)

                FVInfo = FileVersionInfo.GetVersionInfo(FullPath)
                FInfo = Microsoft.VisualBasic.FileIO.FileSystem.GetFileInfo(FullPath)

                TL.LogMessage("FileDetails", "   File Version:       " & FVInfo.FileMajorPart & "." & FVInfo.FileMinorPart & "." & FVInfo.FileBuildPart & "." & FVInfo.FilePrivatePart)
                TL.LogMessage("FileDetails", "   Product Version:    " & FVInfo.ProductMajorPart & "." & FVInfo.ProductMinorPart & "." & FVInfo.ProductBuildPart & "." & FVInfo.ProductPrivatePart)

                TL.LogMessage("FileDetails", "   Description:        " & FVInfo.FileDescription)
                TL.LogMessage("FileDetails", "   Company Name:       " & FVInfo.CompanyName)

                TL.LogMessage("FileDetails", "   Last Write Time:    " & File.GetLastWriteTime(FullPath))
                TL.LogMessage("FileDetails", "   Creation Time:      " & File.GetCreationTime(FullPath))

                TL.LogMessage("FileDetails", "   File Length:        " & Format(FInfo.Length, "#,0.").Replace(ThousandsSeparator, ","))

                Att = File.GetAttributes(FullPath)
                TL.LogMessage("FileDetails", "   Attributes:         " & Att.ToString())
                Try
                    PE = New PEReader(FullPath, TL)
                    TL.LogMessage("FileDetails", "   .NET Assembly:      " & PE.IsDotNetAssembly)
                    TL.LogMessage("FileDetails", "   Bitness:            " & PE.BitNess.ToString)
                    TL.LogMessage("FileDetails", "   Subsystem:          " & PE.SubSystem.ToString)
                    PE.Dispose()
                    PE = Nothing
                Catch ex As ASCOM.InvalidOperationException
                    TL.LogMessage("FileDetails", "   .NET Assembly:      Not a valid PE executable")
                End Try
                NMatches += 1
            Else
                TL.LogMessage("FileDetails", "   ### Unable to find file: " & FullPath)
                NNonMatches += 1
                ErrorList.Add("FileDetails - Unable to find file: " & FullPath)
            End If
        Catch ex As Exception
            LogException("FileDetails", "### Exception: " & ex.ToString)
        End Try

        TL.LogMessage("", "")
    End Sub

    Sub GetCOMRegistration(ByVal ProgID As String)
        Dim RKey As RegistryKey
        Try
            TL.LogMessage("ProgID", ProgID)
            RKey = Registry.ClassesRoot.OpenSubKey(ProgID)
            If Not (RKey Is Nothing) Then ' Registry key exists so process it
                ProcessSubKey(RKey, 1, "None")
                RKey.Close()
                TL.LogMessage("Finished", "")
                NMatches += 1
            Else
                TL.LogMessage("Finished", "*** ProgID " & ProgID & " not found")
                NNonMatches += 1
                ErrorList.Add("GetCOMRegistration - *** ProgID " & ProgID & " not found")
            End If
        Catch ex As Exception
            LogException("Exception", ex.ToString)
        End Try
        TL.LogMessage("", "")
    End Sub

    Sub ProcessSubKey(ByVal p_Key As RegistryKey, ByVal p_Depth As Integer, ByVal p_Container As String)
        Dim ValueNames(), SubKeys() As String
        Dim RKey As RegistryKey, ValueKind As RegistryValueKind
        Dim Container As String
        'TL.LogMessage("Start of ProcessSubKey", p_Container & " " & p_Depth)

        If p_Depth > 12 Then
            TL.LogMessage("RecursionTrap", "Recursion depth has exceeded 12 so terminating at this point as we may be in an infinite loop")
        Else
            Try
                ValueNames = p_Key.GetValueNames
                'TL.LogMessage("Start of ProcessSubKey", "Found " & ValueNames.Length & " values")
                For Each ValueName As String In ValueNames
                    ValueKind = p_Key.GetValueKind(ValueName)
                    Select Case ValueName.ToUpper
                        Case ""
                            TL.LogMessage("KeyValue", Space(p_Depth * INDENT) & "*** Default *** = " & p_Key.GetValue(ValueName))
                        Case "APPID"
                            p_Container = "AppId"
                            TL.LogMessage("KeyValue", Space(p_Depth * INDENT) & ValueName.ToString & " = " & p_Key.GetValue(ValueName))
                        Case Else
                            Select Case ValueKind
                                Case RegistryValueKind.String, RegistryValueKind.ExpandString
                                    TL.LogMessage("KeyValue", Space(p_Depth * INDENT) & ValueName.ToString & " = " & p_Key.GetValue(ValueName))
                                Case RegistryValueKind.MultiString
                                    TL.LogMessage("KeyValue", Space(p_Depth * INDENT) & ValueName.ToString & " = " & p_Key.GetValue(ValueName)(0))
                                Case RegistryValueKind.DWord
                                    TL.LogMessage("KeyValue", Space(p_Depth * INDENT) & ValueName.ToString & " = " & p_Key.GetValue(ValueName).ToString)
                                Case Else
                                    TL.LogMessage("KeyValue", Space(p_Depth * INDENT) & ValueName.ToString & " = " & p_Key.GetValue(ValueName))
                            End Select
                    End Select
                    If ValueKind <> RegistryValueKind.MultiString Then 'Don't try and process these, they don't lead anywhere anyway!
                        If Microsoft.VisualBasic.Left(p_Key.GetValue(ValueName), 1) = "{" Then
                            'TL.LogMessage("ClassExpand", "Expanding " & p_Key.GetValue(ValueName))
                            Select Case p_Container.ToUpper
                                Case "CLSID"
                                    RKey = Registry.ClassesRoot.OpenSubKey("CLSID").OpenSubKey(p_Key.GetValue(ValueName))
                                    If RKey Is Nothing Then 'Check in 32 bit registry on a 64bit system
                                        RKey = Registry.ClassesRoot.OpenSubKey("Wow6432Node\CLSID").OpenSubKey(p_Key.GetValue(ValueName))
                                        If Not (RKey Is Nothing) Then TL.LogMessage("NewSubKey", Space(p_Depth * INDENT) & "Found under Wow6432Node")
                                    End If
                                Case "TYPELIB"
                                    RKey = Registry.ClassesRoot.OpenSubKey("TypeLib").OpenSubKey(p_Key.GetValue(ValueName))
                                    If RKey Is Nothing Then
                                        RKey = Registry.ClassesRoot.OpenSubKey("Wow6432Node\TypeLib").OpenSubKey(p_Key.GetValue(ValueName))
                                    End If
                                Case "APPID"
                                    RKey = Registry.ClassesRoot.OpenSubKey("AppId").OpenSubKey(p_Key.GetValue(ValueName))
                                    If RKey Is Nothing Then
                                        RKey = Registry.ClassesRoot.OpenSubKey("Wow6432Node\AppId").OpenSubKey(p_Key.GetValue(ValueName))
                                    End If
                                Case Else
                                    RKey = p_Key.OpenSubKey(p_Key.GetValue(ValueName))
                            End Select

                            If Not RKey Is Nothing Then
                                If RKey.Name <> p_Key.Name Then 'We are in an infinite loop so kill it by settig rkey = Nothing
                                    TL.LogMessage("NewSubKey", Space((p_Depth + 1) * INDENT) & p_Container & "\" & p_Key.GetValue(ValueName))
                                    ProcessSubKey(RKey, p_Depth + 1, "None")
                                    RKey.Close()
                                Else
                                    TL.LogMessage("IgnoreKey", Space((p_Depth + 1) * INDENT) & p_Container & "\" & p_Key.GetValue(ValueName))
                                End If
                            Else
                                TL.LogMessage("KeyValue", "### Unable to open subkey: " & ValueName & "\" & p_Key.GetValue(ValueName) & " in container: " & p_Container)
                            End If
                        End If
                    End If
                Next
            Catch ex As Exception
                LogException("ProcessSubKey Exception 1", ex.ToString)
            End Try
            Try
                SubKeys = p_Key.GetSubKeyNames
                For Each SubKey In SubKeys
                    TL.LogMessage("ProcessSubKey", Space(p_Depth * INDENT) & SubKey)
                    RKey = p_Key.OpenSubKey(SubKey)
                    Select Case SubKey.ToUpper
                        Case "TYPELIB"
                            'TL.LogMessage("Container", "TypeLib...")
                            Container = "TypeLib"
                        Case "CLSID"
                            'TL.LogMessage("Container", "CLSID...")
                            Container = "CLSID"
                        Case "IMPLEMENTED CATEGORIES"
                            'TL.LogMessage("Container", "Component Categories...")
                            Container = COMPONENT_CATEGORIES
                        Case Else
                            'TL.LogMessage("Container", "Other...")
                            Container = "None"
                    End Select
                    If Microsoft.VisualBasic.Left(SubKey, 1) = "{" Then
                        Select Case p_Container
                            Case COMPONENT_CATEGORIES
                                'TL.LogMessage("ImpCat", "ImpCat")
                                RKey = Registry.ClassesRoot.OpenSubKey(COMPONENT_CATEGORIES).OpenSubKey(SubKey)
                                Container = "None"
                            Case Else
                                'Do nothing
                        End Select
                    End If
                    ProcessSubKey(RKey, p_Depth + 1, Container)
                    RKey.Close()
                Next
            Catch ex As Exception
                LogException("ProcessSubKey Exception 2", ex.ToString)
            End Try
            ' TL.LogMessage("End of ProcessSubKey", p_Container & " " & p_Depth)
        End If

    End Sub

    Sub ScanSerial()
        Dim SerialRegKey As RegistryKey = Nothing, SerialDevices() As String
        Try
            'First list out the ports we can see through .NET
            Status("Scanning Serial Ports")
            If System.IO.Ports.SerialPort.GetPortNames.Length > 0 Then
                For Each Port As String In System.IO.Ports.SerialPort.GetPortNames
                    TL.LogMessage("Serial Ports (.NET)", Port)
                Next
            Else
                TL.LogMessage("Serial Ports (.NET)", "No ports found")
            End If
            TL.BlankLine()
            Try
                SerialRegKey = Registry.LocalMachine.OpenSubKey("HARDWARE\DEVICEMAP\SERIALCOMM")
            Catch ex1 As Exception
                LogException("ScanSerial", "Exception opening HARDWARE\DEVICEMAP\SERIALCOMM : " & ex1.ToString)
            End Try
            If Not (SerialRegKey Is Nothing) Then
                SerialDevices = SerialRegKey.GetValueNames
                For Each SerialDevice As String In SerialDevices
                    TL.LogMessage("Serial Ports (Registry)", SerialRegKey.GetValue(SerialDevice).ToString & " - " & SerialDevice)
                Next
                TL.BlankLine()
            Else
                TL.LogMessage("Serial Ports (Registry)", "No ports found")
            End If

            For i As Integer = 1 To 30
                Call SerialPortDetails(i)
            Next

            TL.BlankLine()
            TL.BlankLine()
        Catch ex As Exception
            LogException("ScanSerial", ex.ToString)
        End Try

    End Sub

    Sub SerialPortDetails(ByVal PortNumber As Integer)
        'List specific details of a particular serial port
        Dim PortName As String, SerPort As New System.IO.Ports.SerialPort

        Try
            PortName = "COM" & PortNumber.ToString 'String version of the port name
            SerPort.PortName = PortName
            SerPort.BaudRate = 9600
            SerPort.Open()
            SerPort.Close()
            TL.LogMessage("Serial Port Test ", PortName & " opened OK")
        Catch ex As Exception
            TL.LogMessageCrLf("Serial Port Test ", ex.Message)
        End Try

        SerPort.Dispose()
        SerPort = Nothing
    End Sub

    Sub ScanProfile()
        Dim ASCOMProfile As Utilities.Profile, DeviceTypes As ArrayList, Devices As ArrayList
        Dim CompatibiityMessage32Bit, CompatibiityMessage64Bit As String
        Try
            ASCOMProfile = New Utilities.Profile
            RecursionLevel = -1 'Initialise recursion level so the first increment makes this zero

            Status("Scanning Profile")

            DeviceTypes = ASCOMProfile.RegisteredDeviceTypes
            For Each DeviceType As String In DeviceTypes
                Devices = ASCOMProfile.RegisteredDevices(DeviceType)
                TL.LogMessage("Registered Device Type", DeviceType)
                For Each Device As KeyValuePair In Devices
                    TL.LogMessage("Registered Devices", "   " & Device.Key & " - " & Device.Value)
                    CompatibiityMessage32Bit = DriverCompatibilityMessage(Device.Key, Bitness.Bits32, TL)
                    If CompatibiityMessage32Bit = "" Then CompatibiityMessage32Bit = "OK for 32bit applications"
                    TL.LogMessage("Registered Devices", "       " & CompatibiityMessage32Bit)

                    If ApplicationBits() = Bitness.Bits64 Then 'We are on a 64bit system so test this
                        CompatibiityMessage64Bit = DriverCompatibilityMessage(Device.Key, Bitness.Bits64, TL)
                        If CompatibiityMessage64Bit = "" Then CompatibiityMessage64Bit = "OK for 64bit applications"
                        TL.LogMessage("Registered Devices", "       " & CompatibiityMessage64Bit)
                    End If

                Next
            Next
            TL.BlankLine()
            TL.BlankLine()
        Catch ex As Exception
            LogException("RegisteredDevices", "Exception: " & ex.ToString)
        End Try

        Try
            TL.LogMessage("Profile", "Recusrsing Profile")
            RecurseProfile("\") 'Scan recurively over the profile
        Catch ex As Exception
            LogException("ScanProfile", ex.Message)
        End Try

        TL.BlankLine()
        TL.BlankLine()
    End Sub

    Sub RecurseProfile(ByVal ASCOMKey As String)
        Dim SubKeys, Values As New Generic.SortedList(Of String, String)
        Dim NextKey, DisplayName, DisplayValue As String

        'List values in this key
        Try
            'TL.LogMessage("RecurseProfile", Space(3 * (If(RecursionLevel < 0, 0, RecursionLevel))) & ASCOMKey)
            Values = ASCOMRegistryAccess.EnumProfile(ASCOMKey)
            For Each kvp As KeyValuePair(Of String, String) In Values
                If String.IsNullOrEmpty(kvp.Key) Then
                    DisplayName = "*** Default Value ***"
                Else
                    DisplayName = kvp.Key
                End If
                If String.IsNullOrEmpty(kvp.Value) Then
                    DisplayValue = "*** Not Set ***"
                Else
                    DisplayValue = kvp.Value
                End If
                TL.LogMessage("Profile Value", Space(3 * (RecursionLevel + 1)) & DisplayName & " = " & DisplayValue)
            Next
        Catch ex As Exception
            LogException("Profile 1", "Exception: " & ex.ToString)
        End Try

        'Now recurse through all subkeys of this key
        Try
            RecursionLevel += 1 'Increment recursion level
            SubKeys = ASCOMRegistryAccess.EnumKeys(ASCOMKey)

            For Each kvp As KeyValuePair(Of String, String) In SubKeys
                If ASCOMKey = "\" Then
                    NextKey = ""
                Else
                    NextKey = ASCOMKey
                End If
                If String.IsNullOrEmpty(kvp.Value) Then
                    DisplayValue = "*** Not Set ***"
                Else
                    DisplayValue = kvp.Value
                End If
                TL.BlankLine()
                TL.LogMessage("Profile Key", Space(3 * RecursionLevel) & NextKey & "\" & kvp.Key & " - " & DisplayValue)
                RecurseProfile(NextKey & "\" & kvp.Key)
            Next

        Catch ex As Exception
            LogException("Profile 2", "Exception: " & ex.ToString)
        Finally
            RecursionLevel -= 1
        End Try

    End Sub

    Private Sub ScanInstalledPlatform()
        Dim RegKey As RegistryKey

        GetInstalledComponent("Platform 5A", "{075F543B-97C5-4118-9D54-93910DE03FE9}", False, True, True)
        GetInstalledComponent("Platform 5B", "{14C10725-0018-4534-AE5E-547C08B737B7}", False, True, True)

        Try ' Platform 5.5 Inno installer setup, should always be absent in Platform 6!
            RegKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\microsoft\Windows\Currentversion\uninstall\ASCOM.platform.NET.Components_is1", False)

            TL.LogMessage("Platform 5.5", RegKey.GetValue("DisplayName"))
            TL.LogMessage("Platform 5.5", "Inno Setup App Path - " & RegKey.GetValue("Inno Setup: App Path"))
            TL.LogMessage("Platform 5.5", "Inno Setup Version - " & RegKey.GetValue("Inno Setup: Setup Version"))
            TL.LogMessage("Platform 5.5", "Install Date - " & RegKey.GetValue("InstallDate"))
            TL.LogMessage("Platform 5.5", "Install Location - " & RegKey.GetValue("InstallLocation"))
            RegKey.Close()
        Catch ex As NullReferenceException
            TL.LogMessage("Platform 5.5", "Not Installed")
        Catch ex As Exception
            LogException("Platform 5.5", "Execption: " & ex.ToString)
        End Try
        TL.BlankLine()

        GetInstalledComponent("Platform 6", PLATFORM_INSTALLER_PROPDUCT_CODE, True, False, True)
        GetInstalledComponent("Platform 6 Developer", DEVELOPER_INSTALLER_PROPDUCT_CODE, False, True, True)

        TL.BlankLine()
    End Sub

    ''' <summary>
    ''' Report details of an installed product
    ''' </summary>
    ''' <param name="Name">Presentation name of product</param>
    ''' <param name="ProductCode">Installer GUID uniquely identifying the product</param>
    ''' <param name="Required">Flag determining whether to report an error or return a status message if the product isn't installed</param>
    ''' <param name="Force32">Flag forcing use of 32bit registry on a 64bit OS</param>
    ''' <param name="MSIInstaller">True if the installer is an MSI based installer, False if an Installaware Native installer</param>
    ''' <remarks></remarks>
    Private Sub GetInstalledComponent(ByVal Name As String, ByVal ProductCode As String, ByVal Required As Boolean, ByVal Force32 As Boolean, ByVal MSIInstaller As Boolean)
        Dim InstallInfo As Generic.SortedList(Of String, String)
        Try ' Platform 6 installer GUID, should always be present in Platform 6
            InstallInfo = GetInstallInformation(ProductCode, Required, Force32, MSIInstaller)
            If InstallInfo.Count > 0 Then
                TL.LogMessage(Name, InstallInfo(INST_DISPLAY_NAME))
                TL.LogMessage(Name, "Version - " & InstallInfo(INST_DISPLAY_VERSION))
                TL.LogMessage(Name, "Install Date - " & InstallInfo(INST_INSTALL_DATE))
                TL.LogMessage(Name, "Install Location - " & InstallInfo(INST_INSTALL_LOCATION))
                'TL.LogMessage(Name, "Install Source - " & InstallInfo(INST_INSTALL_SOURCE))
                NMatches += 1
            Else
                TL.LogMessage(Name, "Not installed")
            End If

            'Catch ex As ProfilePersistenceException
            '    If ex.Message.Contains("as it does not exist") Then
            ' If Required Then ' Must be present so log an error if its not
            ' LogException(Name, "Exception: " & ex.Message)
            ' Else ' Optional so just record absence
            ' End If
            ' Else
            ' LogException(Name, "Exception: " & ex.ToString)
            ' End If
            'Catch ex As NullReferenceException
            '    If Required Then ' Must be present so log an error if its not
            ' LogException(Name, "Exception: " & ex.Message)
            ' Else ' Optional so just record absence
            ' TL.LogMessage(Name, "Not installed")
            ' End If
        Catch ex As Exception
            LogException(Name, "Exception: " & ex.ToString)
        End Try
        TL.BlankLine()

    End Sub

    ''' <summary>
    ''' Gets installation informaiton about a product identified by its product GUID
    ''' </summary>
    ''' <param name="ProductCode">Installer GUID uniquely identifying the product</param>
    ''' <param name="Required">Flag determining whether to report an error or return a status message if the product isn't installed</param>
    ''' <param name="Force32">Flag forcing use of 32bit registry on a 64bit OS</param>
    ''' <param name="MSIInstaller">True if the installer is an MSI based installer, False if an Installaware Native installer</param>
    ''' <returns>Generic Sorted List of key value pairs. If not found retruns an empty list</returns>
    ''' <remarks></remarks>
    Private Function GetInstallInformation(ByVal ProductCode As String, ByVal Required As Boolean, ByVal Force32 As Boolean, ByVal MSIInstaller As Boolean) As Generic.SortedList(Of String, String)
        Dim RegKey As RegistryKey
        Dim RetVal As New Generic.SortedList(Of String, String)
        Dim UninstallString As String
        Dim UninstallKey As RegistryKey, UninstallSubKeyNames() As String
        Dim PlatformName As String = ""

        Const DebugTRace As Boolean = False ' Set to true to debug this code, otherwise leave false!

        If DebugTRace Then TL.LogMessage("GetInstallInformation", "Product: " & ProductCode & ", Required: " & Required & ", Forece32: " & Force32 & ", MSIInstaller: " & MSIInstaller)
        Try
            If MSIInstaller Then
                'RegKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" & PLATFORM_INSTALLER_PROPDUCT_CODE, False)
                If (ApplicationBits() = Bitness.Bits32) Or (ApplicationBits() = Bitness.Bits64 And Not Force32) Then 'use 32bit access on 32bit and 64nit access on 64bit
                    RegKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" & ProductCode, False)
                Else '64bit OS but requires 32bit registry
                    RegKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" & ProductCode, False)
                End If
                If DebugTRace Then
                    If Not RegKey Is Nothing Then
                        TL.LogMessage("GetInstallInformation", "  MSI Installer: found Reg Key: " & RegKey.Name)
                    Else
                        TL.LogMessage("GetInstallInformation", "  MSI Installer: Regkey is nothing!!")
                    End If
                End If

            Else
                If (ApplicationBits() = Bitness.Bits32) Or (ApplicationBits() = Bitness.Bits64 And Not Force32) Then 'use 32bit access on 32bit and 64nit access on 64bit
                    UninstallKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\", False)
                    RegKey = UninstallKey.OpenSubKey(ProductCode, False)
                Else '64bit OS but requires 32bit registry
                    UninstallKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\", False)
                    RegKey = UninstallKey.OpenSubKey(ProductCode, False)
                End If
                If DebugTRace Then
                    If Not UninstallKey Is Nothing Then
                        TL.LogMessage("GetInstallInformation", "  Native Installer: found Uninstall Key: " & UninstallKey.Name)
                    Else
                        TL.LogMessage("GetInstallInformation", "  Uninstall is nothing!!")
                    End If

                    If Not RegKey Is Nothing Then
                        TL.LogMessage("GetInstallInformation", "  Native Installer: found Reg Key: " & RegKey.Name)
                    Else
                        TL.LogMessage("GetInstallInformation", "  Native Installer: Regkey is nothing!!")
                    End If
                End If
                'Get the uninstall string associated with this GUID
                UninstallString = RegKey.GetValue(INST_UNINSTALL_STRING, "")
                RegKey.Close()
                RegKey = Nothing
                If DebugTRace Then TL.LogMessage("GetInstallInformation", "  Native Installer: found Uninstall String: " & UninstallString)

                UninstallSubKeyNames = UninstallKey.GetSubKeyNames
                For Each SubKey As String In UninstallSubKeyNames
                    If DebugTRace Then TL.LogMessage("GetInstallInformation", "  Native Installer: searching subkey : " & SubKey)
                    If UninstallKey.OpenSubKey(SubKey).GetValue(INST_DISPLAY_ICON, "") = UninstallString Then
                        RegKey = UninstallKey.OpenSubKey(SubKey)
                        If DebugTRace Then TL.LogMessage("GetInstallInformation", "    Native Installer Found: " & RegKey.Name)
                        Exit For
                    End If
                Next
            End If

            RetVal.Add(INST_DISPLAY_NAME, RegKey.GetValue(INST_DISPLAY_NAME, "Not known"))
            RetVal.Add(INST_DISPLAY_VERSION, RegKey.GetValue(INST_DISPLAY_VERSION, "Not known"))
            RetVal.Add(INST_INSTALL_DATE, RegKey.GetValue(INST_INSTALL_DATE, "Not known"))
            RetVal.Add(INST_INSTALL_SOURCE, RegKey.GetValue(INST_INSTALL_SOURCE, "Not known"))
            RetVal.Add(INST_INSTALL_LOCATION, RegKey.GetValue(INST_INSTALL_LOCATION, "Not known"))

            RegKey.Close()
        Catch ex As Exception
            If DebugTRace Then TL.LogMessageCrLf("Exception", ex.ToString)
            'If Not RetVal.ContainsKey(INST_DISPLAY_NAME) Then RetVal.Add(INST_DISPLAY_NAME, "Unknown name")
            'If Not RetVal.ContainsKey(INST_DISPLAY_VERSION) Then RetVal.Add(INST_DISPLAY_VERSION, "Unknown version")
            'If Not RetVal.ContainsKey(INST_INSTALL_DATE) Then RetVal.Add(INST_INSTALL_DATE, "Unknown install date")
            'If Not RetVal.ContainsKey(INST_INSTALL_LOCATION) Then RetVal.Add(INST_INSTALL_LOCATION, "Unknown install location")
            'If Not RetVal.ContainsKey(INST_INSTALL_SOURCE) Then RetVal.Add(INST_INSTALL_SOURCE, "Unknown install source")
        End Try

        Return RetVal
    End Function

    Sub ScanASCOMDrivers()
        Dim BaseDir As String
        Dim PathShell As New System.Text.StringBuilder(260)
        Try

            Status("Scanning for ASCOM Drivers")
            TL.LogMessage("ASCOM Drivers Scan", "Searching for installed drivers")

            If System.IntPtr.Size = 8 Then 'We are on a 64bit OS so look in the 64bit locations for files as well
                BaseDir = SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILES_COMMONX86, False)
                BaseDir = PathShell.ToString & "\ASCOM"

                RecurseASCOMDrivers(BaseDir & "\Telescope") 'Check telescope drivers
                RecurseASCOMDrivers(BaseDir & "\Camera") 'Check camera drivers
                RecurseASCOMDrivers(BaseDir & "\Dome") 'Check dome drivers
                RecurseASCOMDrivers(BaseDir & "\FilterWheel") 'Check filterWheel drivers
                RecurseASCOMDrivers(BaseDir & "\Focuser") 'Check focuser drivers
                RecurseASCOMDrivers(BaseDir & "\Rotator") 'Check rotator drivers
                RecurseASCOMDrivers(BaseDir & "\SafetyMonitor") 'Check safetymonitor drivers
                RecurseASCOMDrivers(BaseDir & "\Switch") 'Check switch drivers
                RecurseASCOMDrivers(BaseDir & "\Video") 'Check switch drivers

                BaseDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) & "\ASCOM"

                RecurseASCOMDrivers(BaseDir & "\Telescope") 'Check telescope drivers
                RecurseASCOMDrivers(BaseDir & "\Camera") 'Check camera drivers
                RecurseASCOMDrivers(BaseDir & "\Dome") 'Check dome drivers
                RecurseASCOMDrivers(BaseDir & "\FilterWheel") 'Check filterWheel drivers
                RecurseASCOMDrivers(BaseDir & "\Focuser") 'Check focuser drivers
                RecurseASCOMDrivers(BaseDir & "\Rotator") 'Check rotator drivers
                RecurseASCOMDrivers(BaseDir & "\SafetyMonitor") 'Check safetymonitor drivers
                RecurseASCOMDrivers(BaseDir & "\Switch") 'Check switch drivers
                RecurseASCOMDrivers(BaseDir & "\Video") 'Check switch drivers
            Else '32 bit OS
                BaseDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) & "\ASCOM"

                RecurseASCOMDrivers(BaseDir & "\Telescope") 'Check telescope drivers
                RecurseASCOMDrivers(BaseDir & "\Camera") 'Check camera drivers
                RecurseASCOMDrivers(BaseDir & "\Dome") 'Check dome drivers
                RecurseASCOMDrivers(BaseDir & "\FilterWheel") 'Check filterWheel drivers
                RecurseASCOMDrivers(BaseDir & "\Focuser") 'Check focuser drivers
                RecurseASCOMDrivers(BaseDir & "\Rotator") 'Check rotator drivers
                RecurseASCOMDrivers(BaseDir & "\SafetyMonitor") 'Check safetymonitor drivers
                RecurseASCOMDrivers(BaseDir & "\Switch") 'Check switch drivers
                RecurseASCOMDrivers(BaseDir & "\Video") 'Check switch drivers
            End If

            TL.BlankLine()

        Catch ex As Exception
            LogException("ScanProgramFiles", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub RecurseASCOMDrivers(ByVal Folder As String)
        Dim Files(), Directories() As String

        Try
            Action(Microsoft.VisualBasic.Left(Folder, 70))
            Files = Directory.GetFiles(Folder)
            For Each MyFile As String In Files
                If MyFile.ToUpper.Contains(".EXE") Or MyFile.ToUpper.Contains(".DLL") Then
                    'TL.LogMessage("Driver", MyFile)
                    'FileDetails(Folder & "\", MyFile)
                    FileDetails("", MyFile)
                End If
            Next
        Catch ex As DirectoryNotFoundException
            TL.LogMessageCrLf("RecurseASCOMDrivers", "Directory not present: " & Folder)
            Exit Sub
        Catch ex As Exception
            LogException("RecurseASCOMDrivers 1", "Exception: " & ex.ToString)
        End Try

        Try
            Directories = Directory.GetDirectories(Folder)
            For Each Directory As String In Directories
                'TL.LogMessage("Directory", Directory)
                RecurseASCOMDrivers(Directory)
            Next
            Action("")
        Catch ex As DirectoryNotFoundException
            TL.LogMessage("RecurseASCOMDrivers", "Directory not present: " & Folder)
        Catch ex As Exception
            LogException("RecurseASCOMDrivers 2", "Exception: " & ex.ToString)
        End Try
    End Sub

    Private Sub AstroUtilsTests()
        Dim Events As RiseSet
        Dim Nov31 As New NOVAS.NOVAS31

        Try
            Status("Running Astro Utilities functional tests")
            TL.LogMessage("AstroUtilTests", "Creating ASCOM.Astrometry.AstroUtils.AstroUtils")
            sw.Reset() : sw.Start()
            Dim AstroUtil2 As New ASCOM.Astrometry.AstroUtils.AstroUtils
            TL.LogMessage("AstroUtilTests", "ASCOM.Astrometry.AstroUtils.AstroUtils Created OK in " & sw.ElapsedMilliseconds & " milliseconds")

            'Range Tests
            CompareDouble("AstroUtilTests", "ConditionHA -12.0", AstroUtil2.ConditionHA(-12.0), -12.0, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "ConditionHA 0.0", AstroUtil2.ConditionHA(0.0), 0.0, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "ConditionHA 12.0", AstroUtil2.ConditionHA(12.0), 12.0, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "ConditionHA -13.0", AstroUtil2.ConditionHA(-13.0), 11.0, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "ConditionHA 13.0", AstroUtil2.ConditionHA(13.0), -11.0, TOLERANCE_E6)

            CompareDouble("AstroUtilTests", "ConditionRA 0.0", AstroUtil2.ConditionRA(0.0), 0.0, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "ConditionRA 12.0", AstroUtil2.ConditionRA(12.0), 12.0, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "ConditionRA 23.999", AstroUtil2.ConditionRA(23.999), 23.999, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "ConditionRA -1.0", AstroUtil2.ConditionRA(-1.0), 23.0, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "ConditionRA 25.0", AstroUtil2.ConditionRA(25.0), 1.0, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "Range 0:359.999 0.0", AstroUtil2.Range(0.0, 0.0, True, 360.0, False), 0.0, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "Range 0:359.999 359.0", AstroUtil2.Range(359.0, 0.0, True, 360.0, False), 359.0, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "Range 0:359.999 -30.0", AstroUtil2.Range(-30.0, 0.0, True, 360.0, False), 330.0, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "Range 0:359.999 390.0", AstroUtil2.Range(390.0, 0.0, True, 360.0, False), 30.0, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "Range 0:359.999 360.0", AstroUtil2.Range(360.0, 0.0, True, 360.0, False), 0.0, TOLERANCE_E6)
            CompareDouble("AstroUtilTests", "Range 0:360.0 360.0", AstroUtil2.Range(360.0, 0.0, True, 360.0, True), 360.0, TOLERANCE_E6)

            CompareWithin("AstroUtilTests", "DeltaT", AstroUtil2.DeltaT(), 67.0, 71.0) ' Upper bound increased because DeltaT has reached the original upper bound value! PWGS 9/10/2017
            CompareWithin("AstroUtilTests", "DeltaUT", AstroUtil2.DeltaUT(AstroUtil2.JulianDateTT(0.0)), -1.0, 1.0)
            Events = GetEvents(ASCOM.Astrometry.EventType.SunRiseSunset, 5, 8, 2012, 51.0, -60.0, -5.0)

            CompareBoolean("AstroUtilTests", "Events Sun Risen at Midnight", Events.RisenAtMidnight, False)
            CompareInteger("AstroUtilTests", "Events Sun Rise Count", Events.RiseTime.Count, 1)
            CompareInteger("AstroUtilTests", "Events Sun Set Count", Events.SetTime.Count, 1)
            CompareDouble("AstroUtilTests", "Events Sun Rise", Events.RiseTime(0), 3.54149185233954, TOLERANCE_E5)
            CompareDouble("AstroUtilTests", "Events Sun Set", Events.SetTime(0), 18.6397113427123, TOLERANCE_E5)

            Events = GetEvents(ASCOM.Astrometry.EventType.SunRiseSunset, 19, 3, 2012, -80.0, 85.0, 11.0)

            CompareBoolean("AstroUtilTests", "Events Sun Risen at Midnight", Events.RisenAtMidnight, True)
            CompareInteger("AstroUtilTests", "Events Sun Rise Count", Events.RiseTime.Count, 1)
            CompareInteger("AstroUtilTests", "Events Sun Set Count", Events.SetTime.Count, 2)
            CompareDouble("AstroUtilTests", "Events Sun Rise", Events.RiseTime(0), 10.9587287503168, TOLERANCE_E5)
            CompareDouble("AstroUtilTests", "Events Sun Set", Events.SetTime(0), 0.0368674126801114, TOLERANCE_E2) ' Smaller tolerance becuase the expected value is smaller
            CompareDouble("AstroUtilTests", "Events Sun Set", Events.SetTime(1), 23.8850069460075, TOLERANCE_E5)

            Events = GetEvents(ASCOM.Astrometry.EventType.MoonRiseMoonSet, 5, 8, 2012, 51.0, -60.0, -5.0)

            CompareBoolean("AstroUtilTests", "Events Moon Risen at Midnight", Events.RisenAtMidnight, True)
            CompareInteger("AstroUtilTests", "Events Moon Rise Count", Events.RiseTime.Count, 1)
            CompareInteger("AstroUtilTests", "Events Moon Set Count", Events.SetTime.Count, 1)
            CompareDouble("AstroUtilTests", "Events Moon Rise", Events.RiseTime(0), 19.6212523985916, TOLERANCE_E5)
            CompareDouble("AstroUtilTests", "Events Moon Set", Events.SetTime(0), 7.84782661271154, TOLERANCE_E4)

            Events = GetEvents(ASCOM.Astrometry.EventType.MoonRiseMoonSet, 15, 1, 2012, -80.0, 85.0, 11.0)

            CompareBoolean("AstroUtilTests", "Events Moon Risen at Midnight", Events.RisenAtMidnight, False)
            CompareInteger("AstroUtilTests", "Events Moon Rise Count", Events.RiseTime.Count, 2)
            CompareInteger("AstroUtilTests", "Events Moon Set Count", Events.SetTime.Count, 1)
            CompareDouble("AstroUtilTests", "Events Moon Rise", Events.RiseTime(0), 1.83185022577189, TOLERANCE_E4)
            CompareDouble("AstroUtilTests", "Events Moon Rise", Events.RiseTime(1), 23.803310377656, TOLERANCE_E5)
            CompareDouble("AstroUtilTests", "Events Moon Set", Events.SetTime(0), 20.2772693138778, TOLERANCE_E5)

            Events = GetEvents(ASCOM.Astrometry.EventType.AstronomicalTwilight, 18, 5, 2012, 51.0, 0.0, 0.0)

            CompareBoolean("AstroUtilTests", "Events Astronomical Twighlight Sun Risen at Midnight", Events.RisenAtMidnight, False)
            CompareInteger("AstroUtilTests", "Events Astronomical Twighlight Start Count", Events.RiseTime.Count, 1)
            CompareInteger("AstroUtilTests", "Events Astronomical Twighlight End Count", Events.SetTime.Count, 1)
            CompareDouble("AstroUtilTests", "Events Astronomical Twighlight Start", Events.RiseTime(0), 1.01115193589165, TOLERANCE_E4)
            CompareDouble("AstroUtilTests", "Events Astronomical Twighlight End", Events.SetTime(0), 22.9472021943943, TOLERANCE_E5)

            CompareDouble("AstroUtilTests", "Moon Illumination", AstroUtil2.MoonIllumination(Nov31.JulianDate(2012, 8, 5, 12.0)), 0.872250725459045, TOLERANCE_E5)
            CompareDouble("AstroUtilTests", "Moon Phase", AstroUtil2.MoonPhase(Nov31.JulianDate(2012, 8, 5, 12.0)), -142.145753888332, TOLERANCE_E5)
            TL.BlankLine()

            Try
                AstroUtil2.Dispose()
                TL.LogMessage("AstroUtilTests", "ASCOM.Astrometry.AstroUtils.AstroUtils, Disposed OK")
            Catch ex As Exception
                LogException("AstroUtilTests", "ASCOM.Astrometry.AstroUtils.AstroUtils: " & ex.ToString)
            End Try
            AstroUtil2 = Nothing
            TL.LogMessage("AstroUtilTests", "Finished")
            TL.BlankLine()

        Catch ex As Exception
            LogException("AstroUtilTests", "Exception: " & ex.ToString)
        End Try

    End Sub

    Private Structure RiseSet
        Public RisenAtMidnight As Boolean
        Public RiseTime As Generic.List(Of Double)
        Public SetTime As Generic.List(Of Double)
    End Structure

    Private Function GetEvents(TypeOfEvent As EventType, Day As Integer, Month As Integer, Year As Integer, Latitude As Double, Longitude As Double, TimeZone As Double) As RiseSet
        Dim Events As ArrayList
        Dim Retval As New RiseSet
        Dim NumberOfRises, NumberOfSets As Integer

        Events = AstroUtil.EventTimes(TypeOfEvent, Day, Month, Year, Latitude, Longitude, TimeZone)
        Retval.RisenAtMidnight = CBool(Events(0))
        Retval.RiseTime = New Generic.List(Of Double)
        Retval.SetTime = New Generic.List(Of Double)

        NumberOfRises = CInt(Events(1)) ' Retrieve the number of sunrises
        NumberOfSets = CInt(Events(2)) ' Retrieve the number of sunsets

        If (NumberOfRises > 0) Or (NumberOfSets > 0) Then ' Moon either rises or sets this day
            Select Case NumberOfRises
                Case 0 ' No sunrises so no action
                Case 1 ' 1 sunrise so add the value
                    Retval.RiseTime.Add(CDbl(Events(3)))
                Case 2 ' 2 sunrises so add both values
                    Retval.RiseTime.Add(CDbl(Events(3)))
                    Retval.RiseTime.Add(CDbl(Events(4)))
            End Select
            Select Case NumberOfSets
                Case 0 ' No sunsets so no action
                Case 1 ' 1 sunset so add the value
                    Retval.SetTime.Add(CDbl(Events(NumberOfRises + 3)))
                Case 2 ' 2 sunsets so build up message lines 1 and 2
                    Retval.SetTime.Add(CDbl(Events(NumberOfRises + 3)))
                    Retval.SetTime.Add(CDbl(Events(NumberOfRises + 4)))
            End Select
        Else ' Moon neither rises or sets this day so no further action required here
        End If
        Return Retval
    End Function

#Region "XML  test String"
    Const XMLTestString As String = "<?xml version=""1.0""?>" & vbCrLf &
                                    "<ASCOMProfile>" & vbCrLf &
                                    "  <SubKey>" & vbCrLf &
                                    "    <SubKeyName />" & vbCrLf &
                                    "    <DefaultValue>" & TestTelescopeDescription & "</DefaultValue>" & vbCrLf &
                                    "    <Values>" & vbCrLf &
                                    "      <Value>" & vbCrLf &
                                    "        <Name>Results 1</Name>" & vbCrLf &
                                    "        <Data />" & vbCrLf &
                                    "      </Value>" & vbCrLf &
                                    "      <Value>" & vbCrLf &
                                    "        <Name>Root Test Name</Name>" & vbCrLf &
                                    "        <Data>Test Value in Root key</Data>" & vbCrLf &
                                    "      </Value>" & vbCrLf &
                                    "      <Value>" & vbCrLf &
                                    "        <Name>Test Name</Name>" & vbCrLf &
                                    "        <Data>Test Value</Data>" & vbCrLf &
                                    "      </Value>" & vbCrLf &
                                    "      <Value>" & vbCrLf &
                                    "        <Name>Test Name Default</Name>" & vbCrLf &
                                    "        <Data>123456</Data>" & vbCrLf &
                                    "      </Value>" & vbCrLf &
                                    "    </Values>" & vbCrLf &
                                    "  </SubKey>" & vbCrLf &
                                    "  <SubKey>" & vbCrLf &
                                    "    <SubKeyName>SubKey1</SubKeyName>" & vbCrLf &
                                    "    <DefaultValue />" & vbCrLf &
                                    "    <Values />" & vbCrLf &
                                    "  </SubKey>" & vbCrLf &
                                    "  <SubKey>" & vbCrLf &
                                    "    <SubKeyName>SubKey1\SubKey2</SubKeyName>" & vbCrLf &
                                    "    <DefaultValue>Null Key in SubKey2</DefaultValue>" & vbCrLf &
                                    "    <Values>" & vbCrLf &
                                    "      <Value>" & vbCrLf &
                                    "        <Name>SubKey2 Test Name</Name>" & vbCrLf &
                                    "        <Data>Test Value in SubKey 2</Data>" & vbCrLf &
                                    "      </Value>" & vbCrLf &
                                    "      <Value>" & vbCrLf &
                                    "        <Name>SubKey2 Test Name1</Name>" & vbCrLf &
                                    "        <Data>Test Value in SubKey 2</Data>" & vbCrLf &
                                    "      </Value>" & vbCrLf &
                                    "    </Values>" & vbCrLf &
                                    "  </SubKey>" & vbCrLf &
                                    "  <SubKey>" & vbCrLf &
                                    "    <SubKeyName>SubKey1\SubKey2\SubKey2a</SubKeyName>" & vbCrLf &
                                    "    <DefaultValue />" & vbCrLf &
                                    "    <Values>" & vbCrLf &
                                    "      <Value>" & vbCrLf &
                                    "        <Name>SubKey2a Test Name2a</Name>" & vbCrLf &
                                    "        <Data>Test Value in SubKey 2a</Data>" & vbCrLf &
                                    "      </Value>" & vbCrLf &
                                    "    </Values>" & vbCrLf &
                                    "  </SubKey>" & vbCrLf &
                                    "  <SubKey>" & vbCrLf &
                                    "    <SubKeyName>SubKey1\SubKey2\SubKey2a\SubKey2b</SubKeyName>" & vbCrLf &
                                    "    <DefaultValue />" & vbCrLf &
                                    "    <Values>" & vbCrLf &
                                    "      <Value>" & vbCrLf &
                                    "        <Name>SubKey2b Test Name2b</Name>" & vbCrLf &
                                    "        <Data>Test Value in SubKey 2b</Data>" & vbCrLf &
                                    "      </Value>" & vbCrLf &
                                    "    </Values>" & vbCrLf &
                                    "  </SubKey>" & vbCrLf &
                                    "  <SubKey>" & vbCrLf &
                                    "    <SubKeyName>SubKey1\SubKey2\SubKey2c</SubKeyName>" & vbCrLf &
                                    "    <DefaultValue />" & vbCrLf &
                                    "    <Values>" & vbCrLf &
                                    "      <Value>" & vbCrLf &
                                    "        <Name>SubKey2c Test Name2c</Name>" & vbCrLf &
                                    "        <Data>Test Value in SubKey 2c</Data>" & vbCrLf &
                                    "      </Value>" & vbCrLf &
                                    "    </Values>" & vbCrLf &
                                    "  </SubKey>" & vbCrLf &
                                    "  <SubKey>" & vbCrLf &
                                    "    <SubKeyName>SubKey3</SubKeyName>" & vbCrLf &
                                    "    <DefaultValue />" & vbCrLf &
                                    "    <Values>" & vbCrLf &
                                    "      <Value>" & vbCrLf &
                                    "        <Name>SubKey3 Test Name</Name>" & vbCrLf &
                                    "        <Data>Test Value SubKey 3</Data>" & vbCrLf &
                                    "      </Value>" & vbCrLf &
                                    "    </Values>" & vbCrLf &
                                    "  </SubKey>" & vbCrLf &
                                    "  <SubKey>" & vbCrLf &
                                    "    <SubKeyName>SubKey4</SubKeyName>" & vbCrLf &
                                    "    <DefaultValue />" & vbCrLf &
                                    "    <Values>" & vbCrLf &
                                    "      <Value>" & vbCrLf &
                                    "        <Name>SubKey4 Test Name</Name>" & vbCrLf &
                                    "        <Data>Test Value SubKey 4</Data>" & vbCrLf &
                                    "      </Value>" & vbCrLf &
                                    "    </Values>" & vbCrLf &
                                    "  </SubKey>" & vbCrLf &
                                    "  <SubKey>" & vbCrLf &
                                    "    <SubKeyName>SubKeyDefault</SubKeyName>" & vbCrLf &
                                    "    <DefaultValue />" & vbCrLf &
                                    "    <Values>" & vbCrLf &
                                    "      <Value>" & vbCrLf &
                                    "        <Name>Test Name Default</Name>" & vbCrLf &
                                    "        <Data>123456</Data>" & vbCrLf &
                                    "      </Value>" & vbCrLf &
                                    "    </Values>" & vbCrLf &
                                    "  </SubKey>" & vbCrLf &
                                    "</ASCOMProfile>"
#End Region

#Region "Button event handlers"

    Private Sub btnLastLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLastLog.Click
        'Shell("notepad " & LastLogFile, AppWinStyle.NormalFocus)
        Process.Start(LastLogFile) ' Open in the system's default text editor
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        End 'Close the program
    End Sub

#End Region

#Region "Serial menu event handlers"

    Private Sub MenuSerialTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSerialTraceEnabled.Click
        Dim ProfileStore As RegistryAccess

        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
        If MenuSerialTraceEnabled.Checked Then
            MenuSerialTraceEnabled.Checked = False 'Uncheck the enabled flag, make it inaccessible and clear the trace file name
            ProfileStore.WriteProfile("", SERIAL_FILE_NAME_VARNAME, "")

            'Enable the set trace options
            MenuUseTraceManualFilename.Enabled = True
            MenuUseTraceManualFilename.Checked = False
            MenuUseTraceAutoFilenames.Enabled = True
            MenuUseTraceAutoFilenames.Checked = False
        Else
            MenuSerialTraceEnabled.Checked = True 'Enable the trace enabled flag
            ProfileStore.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SERIAL_AUTO_FILENAME)

            MenuUseTraceAutoFilenames.Enabled = False
            MenuUseTraceAutoFilenames.Checked = True 'Enable the auto tracename flag
            MenuUseTraceManualFilename.Checked = False 'Unset the manual file flag
            MenuUseTraceManualFilename.Enabled = False
        End If
        ProfileStore.Dispose()
        ProfileStore = Nothing
    End Sub

    Private Sub MenuAutoTraceFilenames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuUseTraceAutoFilenames.Click
        Dim ProfileStore As RegistryAccess
        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store
        'Auto filenames currently disabled, so enable them
        MenuUseTraceAutoFilenames.Checked = True 'Enable the auto tracename flag
        MenuUseTraceAutoFilenames.Enabled = False
        MenuUseTraceManualFilename.Checked = False 'Unset the manual file flag
        MenuUseTraceManualFilename.Enabled = False
        MenuSerialTraceEnabled.Enabled = True 'Set the trace enabled flag
        MenuSerialTraceEnabled.Checked = True 'Enable the trace enabled flag
        ProfileStore.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SERIAL_AUTO_FILENAME)
        ProfileStore.Dispose()
        ProfileStore = Nothing
    End Sub

    Private Sub MenuIncludeSerialTraceDebugInformation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuIncludeSerialTraceDebugInformation.Click
        MenuIncludeSerialTraceDebugInformation.Checked = Not MenuIncludeSerialTraceDebugInformation.Checked 'Invert selection
        SetName(SERIAL_TRACE_DEBUG, MenuIncludeSerialTraceDebugInformation.Checked.ToString)
    End Sub

    Private Sub MenuSimulatorTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuSimulatorTraceEnabled.Click
        MenuSimulatorTraceEnabled.Checked = Not MenuSimulatorTraceEnabled.Checked 'Invert the selection
        SetName(SIMULATOR_TRACE, MenuSimulatorTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuUseTraceManualFilename_Click(sender As System.Object, e As System.EventArgs) Handles MenuUseTraceManualFilename.Click
        Dim ProfileStore As RegistryAccess
        Dim RetVal As System.Windows.Forms.DialogResult

        ProfileStore = New RegistryAccess(ERR_SOURCE_CHOOSER) 'Get access to the profile store

        ' Set up the manual serial trace file name entry dialogue
        SerialTraceFileName.FileName = "SerialTrace.txt"
        SerialTraceFileName.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        SerialTraceFileName.FilterIndex = 1
        SerialTraceFileName.RestoreDirectory = True
        RetVal = SerialTraceFileName.ShowDialog() ' Show the dialogue and retrieve the result

        Select Case RetVal ' Handle the outcome from the user
            Case Windows.Forms.DialogResult.OK
                'Save the reault
                ProfileStore.WriteProfile("", SERIAL_FILE_NAME_VARNAME, SerialTraceFileName.FileName)
                'Check and enable the serial trace enabled flag
                MenuSerialTraceEnabled.Enabled = True
                MenuSerialTraceEnabled.Checked = True
                'Enable maual serial trace file flag
                MenuUseTraceAutoFilenames.Checked = False
                MenuUseTraceAutoFilenames.Enabled = False
                MenuUseTraceManualFilename.Checked = True
                MenuUseTraceManualFilename.Enabled = False
            Case Else 'Ignore everything else

        End Select

        ProfileStore.Dispose()
        ProfileStore = Nothing
    End Sub

#End Region

#Region "Other menu event handlers"

    ''' <summary>
    ''' Refresh the trace menu items based on current values stored in the user's registry
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub mnuTrace_DropDownOpening(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuTrace.DropDownOpening
        RefreshTraceItems()
    End Sub

    ''' <summary>
    ''' Refresh the trace menu items based on values stored in the user's registry
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RefreshTraceItems()
        Dim ProfileStore As RegistryAccess, TraceFileName As String
        Dim TypeOfWait As Serial.WaitType

        ProfileStore = New RegistryAccess("Diagnostics") 'Get access to the profile store

        TraceFileName = ProfileStore.GetProfile("", SERIAL_FILE_NAME_VARNAME, "")
        Select Case TraceFileName
            Case "" 'Trace is disabled
                MenuUseTraceAutoFilenames.Enabled = True 'Autofilenames are enabled but unchecked
                MenuUseTraceAutoFilenames.Checked = False
                MenuUseTraceManualFilename.Enabled = True 'Manual trace filename is enabled but unchecked
                MenuUseTraceManualFilename.Checked = False
                MenuSerialTraceEnabled.Checked = False 'The trace enabled flag is unchecked and disabled
                MenuSerialTraceEnabled.Enabled = True
            Case SERIAL_AUTO_FILENAME 'Tracing is on using an automatic filename
                MenuUseTraceAutoFilenames.Enabled = False 'Autofilenames are disabled and checked
                MenuUseTraceAutoFilenames.Checked = True
                MenuUseTraceManualFilename.Enabled = False 'Manual trace filename is dis enabled and unchecked
                MenuUseTraceManualFilename.Checked = False
                MenuSerialTraceEnabled.Checked = True 'The trace enabled flag is checked and enabled
                MenuSerialTraceEnabled.Enabled = True
            Case Else 'Tracing using some other fixed filename
                MenuUseTraceAutoFilenames.Enabled = False 'Autofilenames are disabled and unchecked
                MenuUseTraceAutoFilenames.Checked = False
                MenuUseTraceManualFilename.Enabled = False 'Manual trace filename is disabled enabled and checked
                MenuUseTraceManualFilename.Checked = True
                MenuSerialTraceEnabled.Checked = True 'The trace enabled flag is checked and enabled
                MenuSerialTraceEnabled.Enabled = True
        End Select

        'Set Profile trace checked state on menu item 
        MenuProfileTraceEnabled.Checked = GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT)
        MenuUtilTraceEnabled.Checked = GetBool(TRACE_UTIL, TRACE_UTIL_DEFAULT)
        MenuTimerTraceEnabled.Checked = GetBool(TRACE_TIMER, TRACE_TIMER_DEFAULT)
        MenuTransformTraceEnabled.Checked = GetBool(TRACE_TRANSFORM, TRACE_TRANSFORM_DEFAULT)
        MenuIncludeSerialTraceDebugInformation.Checked = GetBool(SERIAL_TRACE_DEBUG, SERIAL_TRACE_DEBUG_DEFAULT)
        MenuSimulatorTraceEnabled.Checked = GetBool(SIMULATOR_TRACE, SIMULATOR_TRACE_DEFAULT)
        MenuDriverAccessTraceEnabled.Checked = GetBool(DRIVERACCESS_TRACE, DRIVERACCESS_TRACE_DEFAULT)
        MenuThrowAbandonedMutexExceptions.Checked = GetBool(ABANDONED_MUTEXT_TRACE, ABANDONED_MUTEX_TRACE_DEFAULT)
        MenuAstroUtilsTraceEnabled.Checked = GetBool(ASTROUTILS_TRACE, ASTROUTILS_TRACE_DEFAULT)
        MenuNovasTraceEnabled.Checked = GetBool(NOVAS_TRACE, NOVAS_TRACE_DEFAULT)

        TypeOfWait = GetWaitType(SERIAL_WAIT_TYPE, SERIAL_WAIT_TYPE_DEFAULT)

        MenuWaitTypeManualResetEvent.Checked = False
        MenuWaitTypeSleep.Checked = False
        MenuWaitTypeWaitForSingleObject.Checked = False
        Select Case TypeOfWait
            Case Serial.WaitType.ManualResetEvent
                MenuWaitTypeManualResetEvent.Checked = True
            Case Serial.WaitType.Sleep
                MenuWaitTypeSleep.Checked = True
            Case Serial.WaitType.WaitForSingleObject
                MenuWaitTypeWaitForSingleObject.Checked = True
        End Select


    End Sub

    Private Sub ChooserToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChooserToolStripMenuItem1.Click
        Dim Chooser As Object, Chosen As String
        Dim ChooserType As Type

        If ApplicationBits() = Bitness.Bits32 Then
            'Chooser = CreateObject("DriverHelper.Chooser")
            ChooserType = Type.GetTypeFromProgID("DriverHelper.Chooser")
            Chooser = Activator.CreateInstance(ChooserType)
            Chooser.DeviceType = "Telescope"
            Chosen = Chooser.Choose("ScopeSim.Telescope")
        Else
            MsgBox("This component is 32bit only and cannot run on a 64bit system")
        End If
    End Sub

    Private Sub ChooserNETToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChooserNETToolStripMenuItem.Click
        Dim Chooser As ASCOM.Utilities.Chooser, Chosen As String

        Chooser = New ASCOM.Utilities.Chooser
        Chooser.DeviceType = "Telescope"
        Chosen = Chooser.Choose("ScopeSim.Telescope")
        Chooser.Dispose()

    End Sub

    Private Sub ListAvailableCOMPortsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListAvailableCOMPortsToolStripMenuItem.Click
        SerialForm.Visible = True
    End Sub

    Private Sub ChooseAndConncectToDevice64bitApplicationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChooseAndConnectToDevice64bitApplicationToolStripMenuItem.Click
        'ConnectForm.Visible = True
        Dim proc As Process, procStartInfo As ProcessStartInfo
        Try
            procStartInfo = New ProcessStartInfo(Application.StartupPath & DRIVER_CONNECT_APPLICATION_64BIT)
            proc = New Process
            proc.StartInfo = procStartInfo
            proc.Start()
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString() & " - " & Application.StartupPath & DRIVER_CONNECT_APPLICATION_64BIT, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub ChooseAndConnectToDevice32BitApplicationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChooseAndConnectToDevice32bitApplicationToolStripMenuItem.Click
        Dim proc As Process, procStartInfo As ProcessStartInfo

        Try
            procStartInfo = New ProcessStartInfo(Application.StartupPath & DRIVER_CONNECT_APPLICATION_32BIT)
            proc = New Process
            proc.StartInfo = procStartInfo
            proc.Start()
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString() & " - " & Application.StartupPath & DRIVER_CONNECT_APPLICATION_32BIT, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub MenuProfileTraceEnabled_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuProfileTraceEnabled.Click
        MenuProfileTraceEnabled.Checked = Not MenuProfileTraceEnabled.Checked 'Invert the selection
        SetName(TRACE_XMLACCESS, MenuProfileTraceEnabled.Checked.ToString)
        SetName(TRACE_PROFILE, MenuProfileTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuUtilTraceEnabled_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuUtilTraceEnabled.Click
        MenuUtilTraceEnabled.Checked = Not MenuUtilTraceEnabled.Checked 'Invert the selection
        SetName(TRACE_UTIL, MenuUtilTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuTimerTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTimerTraceEnabled.Click
        MenuTimerTraceEnabled.Checked = Not MenuTimerTraceEnabled.Checked 'Invert the selection
        SetName(TRACE_TIMER, MenuTimerTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuTransformTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuTransformTraceEnabled.Click
        MenuTransformTraceEnabled.Checked = Not MenuTransformTraceEnabled.Checked 'Invert the selection
        SetName(TRACE_TRANSFORM, MenuTransformTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuDriverAccessTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDriverAccessTraceEnabled.Click
        MenuDriverAccessTraceEnabled.Checked = Not MenuDriverAccessTraceEnabled.Checked 'Invert the selection
        SetName(DRIVERACCESS_TRACE, MenuDriverAccessTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuAstroUtilsTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAstroUtilsTraceEnabled.Click
        MenuAstroUtilsTraceEnabled.Checked = Not MenuAstroUtilsTraceEnabled.Checked 'Invert the selection
        SetName(ASTROUTILS_TRACE, MenuAstroUtilsTraceEnabled.Checked.ToString)
    End Sub

    Private Sub MenuThrowAbandonedMutexExceptions_Click(sender As System.Object, e As System.EventArgs) Handles MenuThrowAbandonedMutexExceptions.Click
        MenuThrowAbandonedMutexExceptions.Checked = Not MenuThrowAbandonedMutexExceptions.Checked 'Invert the selection
        SetName(ABANDONED_MUTEXT_TRACE, MenuThrowAbandonedMutexExceptions.Checked.ToString)
    End Sub

    Private Sub MenuNovasTraceEnabled_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuNovasTraceEnabled.Click
        MenuNovasTraceEnabled.Checked = Not MenuNovasTraceEnabled.Checked 'Invert selection
        SetName(NOVAS_TRACE, MenuNovasTraceEnabled.Checked.ToString)
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        VersionForm.ShowDialog()
    End Sub

    Private Sub MenuWaitTypeManualResetEvent_Click(sender As System.Object, e As System.EventArgs) Handles MenuWaitTypeManualResetEvent.Click
        MenuWaitTypeManualResetEvent.Checked = True
        MenuWaitTypeSleep.Checked = False
        MenuWaitTypeWaitForSingleObject.Checked = False
        SetName(SERIAL_WAIT_TYPE, Serial.WaitType.ManualResetEvent.ToString)
    End Sub

    Private Sub MenuWaitTypeSleep_Click(sender As System.Object, e As System.EventArgs) Handles MenuWaitTypeSleep.Click
        MenuWaitTypeManualResetEvent.Checked = False
        MenuWaitTypeSleep.Checked = True
        MenuWaitTypeWaitForSingleObject.Checked = False
        SetName(SERIAL_WAIT_TYPE, Serial.WaitType.Sleep.ToString)
    End Sub

    Private Sub MenuWaitTypeWaitForSingleObject_Click(sender As System.Object, e As System.EventArgs) Handles MenuWaitTypeWaitForSingleObject.Click
        MenuWaitTypeManualResetEvent.Checked = False
        MenuWaitTypeSleep.Checked = False
        MenuWaitTypeWaitForSingleObject.Checked = True
        SetName(SERIAL_WAIT_TYPE, Serial.WaitType.WaitForSingleObject.ToString)
    End Sub

#End Region

#Region "Utility Code"

    'DLL to provide the path to Program Files(x86)\Common Files folder location that is not avialable through the .NET framework
    <DllImport("shell32.dll")>
    Shared Function SHGetSpecialFolderPath(ByVal hwndOwner As IntPtr,
        <Out()> ByVal lpszPath As System.Text.StringBuilder,
        ByVal nFolder As Integer,
        ByVal fCreate As Boolean) As Boolean
    End Function

    Private Sub Status(ByVal Msg As String)
        Application.DoEvents()
        lblResult.Text = Msg
        Application.DoEvents()
    End Sub

    Private Sub Action(ByVal Msg As String)
        Application.DoEvents()
        lblAction.Text = Msg
        Application.DoEvents()
    End Sub

#End Region

End Class
