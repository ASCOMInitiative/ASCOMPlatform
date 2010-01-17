' Initial release
' Version 1.0.1.0 - Released

' Fixed issue where all setup log files were not recorded
' Added Conform logs to list of retrieved setup logs
' Added drive scan, reporting available space
' Version 1.0.2.0 - Released 15/10/09 Peter Simpson

Imports System.xml
Imports ASCOM.Utilities
Imports Microsoft.Win32
Imports System.IO
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.GAC
Imports System.Environment

Public Class DiagnosticsForm

    Private Const COMPONENT_CATEGORIES = "Component Categories"
    Private Const Indent As Integer = 3 ' Display indent for recursive loop output

    Private Const CSIDL_PROGRAM_FILES As Integer = 38 '0x0026
    Private Const CSIDL_PROGRAM_FILESX86 As Integer = 42 '0x002a,
    Private Const CSIDL_WINDOWS As Integer = 36 ' 0x0024,
    Private Const CSIDL_PROGRAM_FILES_COMMONX86 As Integer = 44 ' 0x002c,

    Private TL As TraceLogger
    Private Utl As Util
    Private ASCOMXMLAccess As ASCOM.Utilities.XMLAccess
    Private RecursionLevel As Integer

    Private LastLogFile As String ' Name of last diagnostics log file

    'DLL to provide the path to Program Files(x86)\Common Files folder location that is not avialable through the .NET framework
    <DllImport("shell32.dll")> _
    Shared Function SHGetSpecialFolderPath(ByVal hwndOwner As IntPtr, _
        <Out()> ByVal lpszPath As System.Text.StringBuilder, _
        ByVal nFolder As Integer, _
        ByVal fCreate As Boolean) As Boolean
    End Function

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Initialise form
        lblTitle.Text = lblTitle.Text & " " & Application.ProductVersion
        lblResult.Text = ""
        lblAction.Text = ""

        lblMessage.Text = "Your diagnostic log will be created in:" & vbCrLf & vbCrLf & _
        System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\ASCOM\Logs " & Format(Now, "yyyy-MM-dd")

        btnLastLog.Enabled = False 'Disable last log button

        Utl = New Util 'Get an ASCOM Utilities object
    End Sub

    Sub Status(ByVal Msg As String)
        lblResult.Text = Msg
        Application.DoEvents()
    End Sub

    Sub Action(ByVal Msg As String)
        lblAction.Text = Msg
        Application.DoEvents()
    End Sub


    Private Sub btnCOM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCOM.Click
        Dim ASCOMPath As String
        Dim PathShell As New System.Text.StringBuilder(260)

        Try
            Status("Diagnostics running...")

            TL = New TraceLogger("", "Diagnostics")
            TL.Enabled = True
            TL.LogMessage("Diagnostics", "Version " & Application.ProductVersion & " - starting diagnostic run")
            TL.LogMessage("", "")
            LastLogFile = TL.LogFileName
            Try

                ScanInstalledPlatform()

                RunningVersions(TL) 'Log diagnostic information
                TL.LogMessage("", "")

                ScanDrives() 'Scan PC drives and report information

                ScanFrameworks() 'Report on installed .NET Framework versions

                ScanSerial() 'Report serial port information

                ScanASCOMDrivers() 'Report installed driver versions

                ScanProgramFiles() 'Search for copies of Helper and Helper2.DLL in the wrong places

                ScanProfile() 'Report profile information

                ScanRegistry() 'Scan Old ASCOM Registry Profile

                ScanProfileFiles() 'List contents of Profile files

                ScanCOMRegistration() 'Report Com Registration

                'Scan files on 32 and 64bit systems
                TL.LogMessage("Files", "")
                ASCOMPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) & "\ASCOM\"
                Call ScanFiles(ASCOMPath) 'Scan 32bit files on 32bit OS and 64bit files on 64bit OS

                If System.IntPtr.Size = 8 Then 'We are on a 64bit OS so look in the 32bit locations for files as well
                    SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILES_COMMONX86, False)
                    ASCOMPath = PathShell.ToString & "\ASCOM\"
                    Call ScanFiles(ASCOMPath)
                End If

                'List GAC contents
                ScanGac()

                'List setup files
                ScanLogs()

                TL.LogMessage("Diagnostics", "Completed diagnostic run")
                TL.Enabled = False
                TL.Dispose()
                TL = Nothing
                lblResult.Text = "Diagnostic log created OK"
            Catch ex As Exception
                lblResult.Text = "Diagnostics exception, please see log"
                TL.LogMessage("DiagException", ex.ToString)
                TL.Enabled = False
                TL.Dispose()
                TL = Nothing
            End Try
            btnLastLog.Enabled = True
        Catch ex1 As Exception
            lblResult.Text = "Can't create log: " & ex1.Message
        End Try
    End Sub

    Sub ScanRegistry()
        Dim Key As RegistryKey
        Try
            TL.LogMessage("ScanRegistry", "Start")
            If IntPtr.Size = 8 Then '64bit OS so look in Wow64node
                Key = Registry.LocalMachine.OpenSubKey("Software\Wow6432Node\ASCOM")
            Else '32 bit OS
                Key = Registry.LocalMachine.OpenSubKey("Software\ASCOM")
            End If
            TL.LogMessage("Registry Profile", "Profile Root")
            RecursionLevel = -1
            RecurseRegistry(Key)
            TL.BlankLine()
        Catch ex As Exception
            TL.LogMessage("ScanRegistry", "Exception: " & ex.ToString)
        End Try
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
            TL.LogMessage("RecurseRegistry 1", "Exception: " & ex.ToString)
        End Try
        Try
            SubKeys = Key.GetSubKeyNames
            For Each SubKey As String In SubKeys
                TL.LogMessage("Registry Profile Key", Space(RecursionLevel * 2) & SubKey)
                RecurseRegistry(Key.OpenSubKey(SubKey))
            Next
        Catch ex As Exception
            TL.LogMessage("RecurseRegistry 2", "Exception: " & ex.ToString)
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
            TL.LogMessage("ScanDrives", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub ScanProgramFiles()
        Dim BaseDir As String
        Dim PathShell As New System.Text.StringBuilder(260)
        Try
            BaseDir = System.Environment.GetFolderPath(SpecialFolder.ProgramFiles)

            Status("Scanning ProgramFiles Directory for Helper DLLs")
            TL.LogMessage("ProgramFiles Scan", "Searching for Helper.DLL etc.")

            RecurseProgramFiles(BaseDir) ' This is the 32bit path on a 32bit OS and 64bit path on a 64bit OS

            TL.BlankLine()

            'If on a 64bit OS, now scan the 32bit path

            If IntPtr.Size = 8 Then 'We are on a 64bit OS
                BaseDir = System.Environment.GetFolderPath(SpecialFolder.ProgramFiles)
                BaseDir = SHGetSpecialFolderPath(IntPtr.Zero, PathShell, CSIDL_PROGRAM_FILESX86, False)

                Status("Scanning ProgramFiles(x86) Directory for Helper DLLs")
                TL.LogMessage("ProgramFiles(x86) Scan", "Searching for Helper.DLL etc. on 32bit path")

                RecurseProgramFiles(PathShell.ToString) ' This is the 32bit path on a 32bit OS and 64bit path on a 64bit OS

                TL.BlankLine()
            End If
        Catch ex As Exception
            TL.LogMessage("ScanProgramFiles", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub RecurseProgramFiles(ByVal Folder As String)
        Dim Files(), Directories() As String

        'TL.LogMessage("Folder", Folder)
        'Process files in this directory
        Try
            Action(Microsoft.VisualBasic.Left(Folder, 70))
            Files = Directory.GetFiles(Folder)
            For Each MyFile As String In Files
                If MyFile.ToUpper.Contains("\HELPER.DLL") Then
                    TL.LogMessage("Helper.DLL", MyFile)
                    FileDetails(Folder & "\", "HELPER.DLL")
                End If
                If MyFile.ToUpper.Contains("\HELPER2.DLL") Then
                    TL.LogMessage("Helper2.DLL", MyFile)
                    FileDetails(Folder & "\", "HELPER2.DLL")
                End If
            Next
        Catch ex As Exception
            TL.LogMessage("RecurseProgramFiles 1", "Exception: " & ex.ToString)
        End Try

        Try
            Directories = Directory.GetDirectories(Folder)
            For Each Directory As String In Directories
                'TL.LogMessage("Directory", Directory)
                RecurseProgramFiles(Directory)
            Next
            Action("")
        Catch ex As Exception
            TL.LogMessage("RecurseProgramFiles 2", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub ScanProfileFiles()
        Dim BaseDir As String
        Try
            BaseDir = System.Environment.GetFolderPath(SpecialFolder.CommonApplicationData) & "\ASCOM\Profile"

            Status("Scanning Profile Files")
            TL.LogMessage("Scanning Profile Files", "")

            RecurseProfileFiles(BaseDir)

            TL.BlankLine()
        Catch ex As Exception
            TL.LogMessage("ScanProfileFiles", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub RecurseProfileFiles(ByVal Folder As String)
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
            TL.LogMessage("RecurseProfileFiles 1", "Exception: " & ex.ToString)
        End Try

        Try
            Directories = Directory.GetDirectories(Folder)
            For Each Directory As String In Directories
                TL.LogMessage("Directory", Directory)
                RecurseProfileFiles(Directory)
            Next
        Catch ex As Exception
            TL.LogMessage("RecurseProfileFiles 2", "Exception: " & ex.ToString)
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
            TL.LogMessage("Frameworks", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub ScanLogs()
        Const NumLine As Integer = 30 'Number of lines to read from file to see if it is an ASCOM log

        Dim TempFiles(NumLine + 1) As String
        Dim SR As StreamReader = Nothing
        Dim Lines(30) As String, LineCount As Integer = 0
        Dim ASCOMFile As Boolean

        Try
            Status("Scanning setup logs")
            TL.LogMessage("SetupFile", "Starting scan")
            'Get an array of setup filenames from the Temp directory
            TempFiles = Directory.GetFiles(Path.GetFullPath(GetEnvironmentVariable("Temp")), "Setup Log*.txt", SearchOption.TopDirectoryOnly)
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
                    TL.LogMessage("SetupFile", "Exception 1: " & ex1.ToString)
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
            TL.LogMessage("SetupFile", "Exception 2: " & ex2.ToString)
        End Try
    End Sub

    Sub ScanCOMRegistration()
        Try
            Status("Scanning Registry")
            TL.LogMessage("COMRegistration", "") 'Report COM registation
            GetCOMRegistration("DriverHelper.Chooser")
            GetCOMRegistration("DriverHelper.Profile")
            GetCOMRegistration("DriverHelper.Serial")
            GetCOMRegistration("DriverHelper.Timer")
            GetCOMRegistration("DriverHelper.Util")
            GetCOMRegistration("DriverHelper2.Util")

            GetCOMRegistration("DriverHelper.ChooserSupport")
            GetCOMRegistration("DriverHelper.ProfileAccess")
            GetCOMRegistration("DriverHelper.SerialSupport")
            GetCOMRegistration("ScopeSim.Telescope")

            GetCOMRegistration("ASCOM.Utilities.Chooser")
            GetCOMRegistration("ASCOM.Utilities.KeyValuePair")
            GetCOMRegistration("ASCOM.Utilities.Profile")
            GetCOMRegistration("ASCOM.Utilities.Serial")
            GetCOMRegistration("ASCOM.Utilities.Timer")
            GetCOMRegistration("ASCOM.Utilities.TraceLogger")
            GetCOMRegistration("ASCOM.Utilities.Util")

            GetCOMRegistration("ASCOM.Astrometry.Kepler.Ephemeris")
            GetCOMRegistration("ASCOM.Astrometry.NOVAS.NOVAS2COM")
            GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.Earth")
            GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.Planet")
            GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.PositionVector")
            GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.Site")
            GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.Star")
            GetCOMRegistration("ASCOM.Astrometry.NOVASCOM.VelocityVector")
            GetCOMRegistration("ASCOM.Astrometry.Transform.Transform")
            TL.LogMessage("", "")
        Catch ex As Exception
            TL.LogMessage("ScanCOMRegistration", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub ScanGac()
        Dim ae As IAssemblyEnum
        Dim an As IAssemblyName = Nothing
        Dim name As AssemblyName
        Dim ass As Assembly
        Try
            Status("Scanning Assemblies")

            TL.LogMessage("Assemblies", "Assemblies registered in the GAC")
            ae = AssemblyCache.CreateGACEnum ' Get an enumerator for the GAC assemblies

            Do While (AssemblyCache.GetNextAssembly(ae, an) = 0) 'Enumerate the assemblies
                Try
                    name = GetAssemblyName(an) 'Convert the fusion representation to a standard AssemblyName
                    If InStr(name.FullName, "ASCOM") > 0 Then 'Extra information for ASCOM files
                        TL.LogMessage("Assemblies", name.Name)
                        ass = Assembly.Load(name.FullName)
                        AssemblyInfo(TL, name.Name, ass) ' Get file version and other information
                    Else
                        TL.LogMessage("Assemblies", name.FullName)
                    End If
                Catch ex As Exception
                    TL.LogMessage("Assemblies", "Exception: " & ex.ToString)
                End Try
            Loop
            TL.LogMessage("", "")
        Catch ex As Exception
            TL.LogMessage("ScanGac", "Exception: " & ex.ToString)
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
            TL.LogMessage("GetAssemblyName", "Exception: " & ex.ToString)
        End Try
        Return AssName
    End Function

    Sub ScanFiles(ByVal ASCOMPath As String)
        Dim ASCOMPathTel, ASCOMPathInt, ASCOMPathNet, ASCOMPathUtl As String

        Try
            Status("Scanning Files")

            ASCOMPathInt = ASCOMPath & "Interface\" 'Create folder paths
            ASCOMPathTel = ASCOMPath & "Telescope\"
            ASCOMPathNet = ASCOMPath & ".net\"
            ASCOMPathUtl = ASCOMPath & "Utilities\"

            FileDetails(ASCOMPath, "Helper.dll") 'Report on files
            FileDetails(ASCOMPath, "Helper2.dll")
            FileDetails(ASCOMPathNet, "ASCOM.Utilities.dll")
            FileDetails(ASCOMPathUtl, "Helper.dll")
            FileDetails(ASCOMPathUtl, "Helper2.dll")

            FileDetails(ASCOMPath, "Astro32.dll")
            FileDetails(ASCOMPathTel, "ScopeSim.exe")
            FileDetails(ASCOMPathInt, "Helper.tlb")
            FileDetails(ASCOMPathInt, "Helper2.tlb")
            FileDetails(ASCOMPathInt, "ASCOMMasterInterfaces.tlb")

            FileDetails(ASCOMPathNet, "ASCOM.Astrometry.dll")
            FileDetails(ASCOMPathNet, "ASCOM.Attributes.dll")
            FileDetails(ASCOMPathNet, "ASCOM.DriverAccess.dll")
            FileDetails(ASCOMPathNet, "ASCOM.Exceptions.dll")
            FileDetails(ASCOMPathNet, "ASCOM.IConform.dll")
            FileDetails(ASCOMPathNet, "ASCOM.Kepler.dll")
            FileDetails(ASCOMPathNet, "ASCOM.NOVAS.dll")
            FileDetails(ASCOMPathNet, "NOVAS-C64.dll")
            FileDetails(ASCOMPathNet, "NOVAS-C.dll")
            FileDetails(ASCOMPathNet, "policy.1.0.ASCOM.DriverAccess.dll")
            FileDetails(ASCOMPathNet, "policy.5.5.ASCOM.Astrometry.dll")
            FileDetails(ASCOMPathNet, "policy.5.5.ASCOM.Utilities.dll")
            FileDetails(ASCOMPathUtl, "EraseProfile.exe")
            FileDetails(ASCOMPathUtl, "MigrateProfile.exe")
        Catch ex As Exception
            TL.LogMessage("ScanFiles", "Exception: " & ex.ToString)
        End Try

    End Sub

    Sub FileDetails(ByVal FPath As String, ByVal FName As String)
        Dim FullPath As String
        Dim Att As FileAttributes, FVInfo As FileVersionInfo, FInfo As FileInfo

        Try
            FullPath = FPath & FName 'Create full filename from path and simple filename
            TL.LogMessage("FileDetails", FName & " " & FullPath)
            If File.Exists(FullPath) Then

                FVInfo = FileVersionInfo.GetVersionInfo(FullPath)
                FInfo = Microsoft.VisualBasic.FileIO.FileSystem.GetFileInfo(FullPath)

                TL.LogMessage("FileDetails", "   File Version:    " & FVInfo.FileMajorPart & "." & FVInfo.FileMinorPart & "." & FVInfo.FileBuildPart & "." & FVInfo.FilePrivatePart)
                TL.LogMessage("FileDetails", "   Product Version: " & FVInfo.ProductMajorPart & "." & FVInfo.ProductMinorPart & "." & FVInfo.ProductBuildPart & "." & FVInfo.ProductPrivatePart)

                TL.LogMessage("FileDetails", "   Description:     " & FVInfo.FileDescription)
                TL.LogMessage("FileDetails", "   Company Name:    " & FVInfo.CompanyName)

                TL.LogMessage("FileDetails", "   Last Write Time: " & File.GetLastWriteTime(FullPath))
                TL.LogMessage("FileDetails", "   Creation Time:   " & File.GetCreationTime(FullPath))

                TL.LogMessage("FileDetails", "   File Length:     " & Format(FInfo.Length, "#,0."))

                Att = File.GetAttributes(FullPath)
                TL.LogMessage("FileDetails", "   Attributes:      " & Att.ToString())

            Else
                TL.LogMessage("FileDetails", "   ### Unable to find file: " & FullPath)
            End If
        Catch ex As Exception
            TL.LogMessage("FileDetails", "### Exception: " & ex.ToString)
        End Try

        TL.LogMessage("", "")
    End Sub

    Sub GetCOMRegistration(ByVal ProgID As String)
        Dim RKey As RegistryKey
        Try
            TL.LogMessage("ProgID", ProgID)
            RKey = Registry.ClassesRoot.OpenSubKey(ProgID)
            ProcessSubKey(RKey, 1, "None")
            RKey.Close()
            TL.LogMessage("Finished", "")
        Catch ex As Exception
            TL.LogMessage("Exception", ex.ToString)
        End Try
        TL.LogMessage("", "")
    End Sub

    Sub ProcessSubKey(ByVal p_Key As RegistryKey, ByVal p_Depth As Integer, ByVal p_Container As String)
        Dim ValueNames(), SubKeys() As String
        Dim RKey As RegistryKey
        Dim Container As String
        'TL.LogMessage("Start of ProcessSubKey", p_Container & " " & p_Depth)

        If p_Depth > 12 Then
            TL.LogMessage("RecursionTrap", "Recursion depth has exceeded 12 so terminating at this point as we may be in an infinite loop")
        Else
            Try
                ValueNames = p_Key.GetValueNames
                'TL.LogMessage("Start of ProcessSubKey", "Found " & ValueNames.Length & " values")
                For Each ValueName As String In ValueNames
                    Select Case ValueName.ToUpper
                        Case ""
                            TL.LogMessage("KeyValue", Space(p_Depth * Indent) & "*** Default *** = " & p_Key.GetValue(ValueName))
                        Case "APPID"
                            p_Container = "AppId"
                            TL.LogMessage("KeyValue", Space(p_Depth * Indent) & ValueName.ToString & " = " & p_Key.GetValue(ValueName))
                        Case Else
                            TL.LogMessage("KeyValue", Space(p_Depth * Indent) & ValueName.ToString & " = " & p_Key.GetValue(ValueName))
                    End Select
                    If Microsoft.VisualBasic.Left(p_Key.GetValue(ValueName), 1) = "{" Then
                        'TL.LogMessage("ClassExpand", "Expanding " & p_Key.GetValue(ValueName))
                        Select Case p_Container.ToUpper
                            Case "CLSID"
                                RKey = Registry.ClassesRoot.OpenSubKey("CLSID").OpenSubKey(p_Key.GetValue(ValueName))
                                If RKey Is Nothing Then 'Check in 32 bit registry on a 64bit system
                                    RKey = Registry.ClassesRoot.OpenSubKey("Wow6432Node\CLSID").OpenSubKey(p_Key.GetValue(ValueName))
                                    If Not (RKey Is Nothing) Then TL.LogMessage("NewSubKey", Space(p_Depth * Indent) & "Found under Wow6432Node")
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
                                TL.LogMessage("NewSubKey", Space((p_Depth + 1) * Indent) & p_Container & "\" & p_Key.GetValue(ValueName))
                                ProcessSubKey(RKey, p_Depth + 1, "None")
                                RKey.Close()
                            Else
                                TL.LogMessage("IgnoreKey", Space((p_Depth + 1) * Indent) & p_Container & "\" & p_Key.GetValue(ValueName))
                            End If
                        Else
                            TL.LogMessage("KeyValue", "### Unable to open subkey: " & ValueName & "\" & p_Key.GetValue(ValueName) & " in container: " & p_Container)
                        End If
                    End If
                Next
            Catch ex As Exception
                TL.LogMessage("ProcessSubKey Exception 1", ex.ToString)
            End Try
            Try
                SubKeys = p_Key.GetSubKeyNames
                For Each SubKey In SubKeys
                    TL.LogMessage("ProcessSubKey", Space(p_Depth * Indent) & SubKey)
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
                TL.LogMessage("ProcessSubKey Exception 2", ex.ToString)
            End Try
            ' TL.LogMessage("End of ProcessSubKey", p_Container & " " & p_Depth)
        End If

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        End 'Close the program
    End Sub

    Private Sub ChooserToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChooserToolStripMenuItem1.Click
        Dim Chooser As Object, Chosen As String

        Chooser = CreateObject("DriverHelper.Chooser")
        Chooser.DeviceType = "Telescope"
        Chosen = Chooser.Choose("ScopeSim.Telescope")

    End Sub

    Sub ScanSerial()
        Dim SerialRegKey As RegistryKey, SerialDevices() As String
        Try
            'First list out the ports we can see through .NET
            Status("Scanning Serial Ports")
            For Each Port As String In System.IO.Ports.SerialPort.GetPortNames
                TL.LogMessage("Serial Ports (.NET)", Port)
            Next
            TL.BlankLine()

            SerialRegKey = Registry.LocalMachine.OpenSubKey("HARDWARE\DEVICEMAP\SERIALCOMM")
            SerialDevices = SerialRegKey.GetValueNames
            For Each SerialDevice As String In SerialDevices
                TL.LogMessage("Serial Ports (Registry)", SerialRegKey.GetValue(SerialDevice).ToString & " - " & SerialDevice)
            Next
            TL.BlankLine()

            For i As Integer = 1 To 30
                Call SerialPortDetails(i)
            Next

            TL.BlankLine()
        Catch ex As Exception
            TL.LogMessage("ScanSerial", ex.ToString)
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
            TL.LogMessage("Serial Port Test ", ex.Message)
        End Try

        SerPort.Dispose()
        SerPort = Nothing
    End Sub

    Sub ScanProfile()

        Dim ASCOMProfile As New Utilities.Profile, DeviceTypes() As String, Devices As ArrayList

        Try
            ASCOMXMLAccess = New ASCOM.Utilities.XMLAccess
            RecursionLevel = -1 'Initialise recursion level so the first increment makes this zero

            Status("Scanning Profile")

            DeviceTypes = ASCOMProfile.RegisteredDeviceTypes
            For Each DeviceType As String In DeviceTypes
                Devices = ASCOMProfile.RegisteredDevices(DeviceType)
                TL.LogMessage("Registered Device Type", DeviceType)
                For Each Device As KeyValuePair In Devices
                    TL.LogMessage("Registered Devices", "   " & Device.Key & " - " & Device.Value)
                Next
            Next
            TL.BlankLine()
        Catch ex As Exception
            TL.LogMessage("RegisteredDevices", "Exception: " & ex.ToString)
        End Try

        Try
            TL.LogMessage("Profile", "Recusrsing Profile")
            RecurseProfile("\") 'Scan recurively over the profile
        Catch ex As Exception
            TL.LogMessage("ScanProfile", ex.Message)
        End Try

        TL.BlankLine()

        ASCOMXMLAccess.Dispose() 'Clean up
        ASCOMXMLAccess = Nothing

    End Sub

    Sub RecurseProfile(ByVal ASCOMKey As String)
        Dim SubKeys, Values As New Generic.SortedList(Of String, String)
        Dim NextKey, DisplayName, DisplayValue As String
        Try
            TL.LogMessage("RecurseProfile", ASCOMKey)
            Values = ASCOMXMLAccess.EnumProfile(ASCOMKey)
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
                TL.LogMessage("Profile", Space(3 * (RecursionLevel + 1)) & DisplayName & " = " & DisplayValue)
            Next
        Catch ex As Exception
            TL.LogMessage("Profile 1", "Exception: " & ex.ToString)
        End Try

        Try
            RecursionLevel += 1 'Increment recursion level
            SubKeys = ASCOMXMLAccess.EnumKeys(ASCOMKey)

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

                TL.LogMessage("Profile Key", Space(3 * RecursionLevel) & NextKey & "\" & kvp.Key & " - " & DisplayValue)
                RecurseProfile(NextKey & "\" & kvp.Key)
            Next

        Catch ex As Exception
            TL.LogMessage("Profile 2", "Exception: " & ex.ToString)
        Finally
            RecursionLevel -= 1
        End Try


    End Sub

    Private Sub ChooserNETToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChooserNETToolStripMenuItem.Click
        Dim Chooser As ASCOM.Utilities.Chooser, Chosen As String

        Chooser = New ASCOM.Utilities.Chooser
        Chooser.DeviceType = "Telescope"
        Chosen = Chooser.Choose("ScopeSim.Telescope")
        Chooser.Dispose()

    End Sub

    Private Sub ConnectToDeviceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConnectToDeviceToolStripMenuItem.Click
        ConnectForm.Visible = True
    End Sub

    Private Sub ListAvailableCOMPortsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListAvailableCOMPortsToolStripMenuItem.Click
        SerialForm.Visible = True
    End Sub

    Private Sub ScanInstalledPlatform()
        Dim RegKey As RegistryKey

        RegKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\microsoft\Windows\Currentversion\uninstall\ASCOM.platform.NET.Components_is1", False)

        TL.LogMessage("Installed Platform", "ASCOM PlatformVersion: " & Utl.PlatformVersion)
        TL.BlankLine()

        TL.LogMessage("Installed Platform", RegKey.GetValue("DisplayName"))
        TL.LogMessage("Installed Platform", "Inno Setup App Path - " & RegKey.GetValue("Inno Setup: App Path"))
        TL.LogMessage("Installed Platform", "Inno Setup Version - " & RegKey.GetValue("Inno Setup: Setup Version"))
        TL.LogMessage("Installed Platform", "Install Date - " & RegKey.GetValue("InstallDate"))
        TL.LogMessage("Installed Platform", "Install Location - " & RegKey.GetValue("InstallLocation"))

        TL.BlankLine()
    End Sub
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
                RecurseASCOMDrivers(BaseDir & "\Focuser") 'Check focuser drivers
                RecurseASCOMDrivers(BaseDir & "\Dome") 'Check telescope drivers
                RecurseASCOMDrivers(BaseDir & "\Rotator") 'Check focuser drivers
                RecurseASCOMDrivers(BaseDir & "\Camera") 'Check telescope drivers
                RecurseASCOMDrivers(BaseDir & "\Switch") 'Check focuser drivers

                BaseDir = Environment.GetFolderPath(SpecialFolder.CommonProgramFiles) & "\ASCOM"

                RecurseASCOMDrivers(BaseDir & "\Telescope") 'Check telescope drivers
                RecurseASCOMDrivers(BaseDir & "\Focuser") 'Check focuser drivers
                RecurseASCOMDrivers(BaseDir & "\Dome") 'Check telescope drivers
                RecurseASCOMDrivers(BaseDir & "\Rotator") 'Check focuser drivers
                RecurseASCOMDrivers(BaseDir & "\Camera") 'Check telescope drivers
                RecurseASCOMDrivers(BaseDir & "\Switch") 'Check focuser drivers
            Else '32 bit OS
                BaseDir = Environment.GetFolderPath(SpecialFolder.CommonProgramFiles) & "\ASCOM"

                RecurseASCOMDrivers(BaseDir & "\Telescope") 'Check telescope drivers
                RecurseASCOMDrivers(BaseDir & "\Focuser") 'Check focuser drivers
                RecurseASCOMDrivers(BaseDir & "\Dome") 'Check telescope drivers
                RecurseASCOMDrivers(BaseDir & "\Rotator") 'Check focuser drivers
                RecurseASCOMDrivers(BaseDir & "\Camera") 'Check telescope drivers
                RecurseASCOMDrivers(BaseDir & "\Switch") 'Check focuser drivers
            End If

            TL.BlankLine()

        Catch ex As Exception
            TL.LogMessage("ScanProgramFiles", "Exception: " & ex.ToString)
        End Try
    End Sub
    Sub RecurseASCOMDrivers(ByVal Folder As String)
        Dim Files(), Directories() As String

        Try
            Action(Microsoft.VisualBasic.Left(Folder, 70))
            Files = Directory.GetFiles(Folder)
            For Each MyFile As String In Files
                If MyFile.ToUpper.Contains(".EXE") Or MyFile.ToUpper.Contains(".DLL") Then
                    TL.LogMessage("Driver", MyFile)
                    'FileDetails(Folder & "\", MyFile)
                    FileDetails("", MyFile)
                End If
            Next
        Catch ex As DirectoryNotFoundException
            TL.LogMessage("Driver", "Directory not present: " & Folder)
            Exit Sub
        Catch ex As Exception
            TL.LogMessage("RecurseASCOMDrivers 1", "Exception: " & ex.ToString)
        End Try

        Try
            Directories = Directory.GetDirectories(Folder)
            For Each Directory As String In Directories
                'TL.LogMessage("Directory", Directory)
                RecurseASCOMDrivers(Directory)
            Next
            Action("")
        Catch ex As DirectoryNotFoundException
            TL.LogMessage("Driver", "Directory not present: " & Folder)
        Catch ex As Exception
            TL.LogMessage("RecurseASCOMDrivers 2", "Exception: " & ex.ToString)
        End Try
    End Sub

    Private Sub btnLastLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLastLog.Click
        Shell("notepad " & LastLogFile, AppWinStyle.NormalFocus)
    End Sub
End Class
