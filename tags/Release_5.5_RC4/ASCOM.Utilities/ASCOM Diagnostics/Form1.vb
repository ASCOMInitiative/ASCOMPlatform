' Initial release
' Version 1.0.1.0 - Released

' Fixed issue where all setup log files were not recorded
' Added Conform logs to list of retrieved setup logs
' Added drive scan, reporting available space
' Version 1.0.2.0 - Released 15/10/09 Peter Simpson

Imports ASCOM.Utilities
Imports Microsoft.Win32
Imports System.IO
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.GAC
Imports System.Environment

Public Class Form1

    Private Const COMPONENT_CATEGORIES = "Component Categories"
    Private Const Indent As Integer = 3 ' Display indent for recursive loop output

    Dim TL As TraceLogger

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

        lblMessage.Text = "Your diagnostic log will be created in:" & vbCrLf & vbCrLf & _
        System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\ASCOM\Logs " & Format(Now, "yyyy-MM-dd")
    End Sub

    Sub Status(ByVal Msg As String)
        lblResult.Text = Msg
        Application.DoEvents()
    End Sub

    Private Sub btnCOM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCOM.Click
        Dim ASCOMPath As String
        Dim PathShell As New System.Text.StringBuilder(260)
        Dim Drives() As String, Drive As DriveInfo

        Try
            Status("Diagnostics running...")

            TL = New TraceLogger("", "Diagnostics")
            TL.Enabled = True
            TL.LogMessage("Diagnostics", "Version " & Application.ProductVersion & " - starting diagnostic run")
            TL.LogMessage("", "")
            Try

                RunningVersions(TL) 'Log diagnostic information
                TL.LogMessage("", "")
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

                ScanRegistry() 'Report Com Registration

                'Scan files on 32 and 64bit systems
                TL.LogMessage("Files", "")
                ASCOMPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) & "\ASCOM\"
                Call ScanFiles(ASCOMPath) 'Scan 32bit files on 32bit OS and 64bit files on 64bit OS

                If System.IntPtr.Size = 8 Then 'We are on a 64bit OS so look in the 32bit locations for files as well
                    SHGetSpecialFolderPath(IntPtr.Zero, PathShell, 44, False)
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
        Catch ex1 As Exception
            lblResult.Text = "Can't create log: " & ex1.Message
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
        Catch ex2 As Exception
            TL.LogMessage("SetupFile", "Exception 2: " & ex2.ToString)
        End Try
        TL.LogMessage("", "")
        TL.LogMessage("SetupFile", "Completed scan")
        TL.LogMessage("", "")
    End Sub

    Sub ScanRegistry()
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
    End Sub

    Sub ScanGac()
        Dim ae As IAssemblyEnum
        Dim an As IAssemblyName = Nothing
        Dim name As AssemblyName
        Dim ass As Assembly

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
    End Sub

    Private Function GetAssemblyName(ByVal nameRef As IAssemblyName) As AssemblyName
        Dim AssName As New AssemblyName()
        AssName.Name = AssemblyCache.GetName(nameRef)
        AssName.Version = AssemblyCache.GetVersion(nameRef)
        AssName.CultureInfo = AssemblyCache.GetCulture(nameRef)
        AssName.SetPublicKeyToken(AssemblyCache.GetPublicKeyToken(nameRef))
        Return AssName
    End Function

    Sub ScanFiles(ByVal ASCOMPath As String)
        Dim ASCOMPathTel, ASCOMPathInt, ASCOMPathNet, ASCOMPathUtl As String

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

    End Sub

    Sub FileDetails(ByVal FPath As String, ByVal FName As String)
        Dim FullPath As String
        Dim Att As FileAttributes, FVInfo As FileVersionInfo, FInfo As FileInfo
        FullPath = FPath & FName 'Create full filename from path and simple filename
        TL.LogMessage("FileDetails", FName & " " & FullPath)
        Try
            If File.Exists(FullPath) Then

                FVInfo = FileVersionInfo.GetVersionInfo(FullPath)
                FInfo = Microsoft.VisualBasic.FileIO.FileSystem.GetFileInfo(FullPath)

                TL.LogMessage("FileDetails", "   File Version:    " & FVInfo.FileMajorPart & "." & FVInfo.FileMinorPart & " " & FVInfo.FileBuildPart & " " & FVInfo.FilePrivatePart)
                TL.LogMessage("FileDetails", "   Product Version: " & FVInfo.ProductMajorPart & "." & FVInfo.ProductMinorPart & " " & FVInfo.ProductBuildPart & " " & FVInfo.ProductPrivatePart)

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
        TL.LogMessage("ProgID", ProgID)
        Try
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
End Class
