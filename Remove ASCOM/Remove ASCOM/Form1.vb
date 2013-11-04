Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.IO
Imports Microsoft.Win32
Imports System.Text
Imports System.Globalization

Public Class Form1
    Private TL As TraceLogger

    'Constants for use with SHGetSpecialFolderPath
    Const CSIDL_COMMON_STARTMENU As Integer = 22 ' 0x0016  
    Const CSIDL_COMMON_DESKTOPDIRECTORY = 25 ' 0x0019; 
    Const CSIDL_PROGRAM_FILES As Integer = 38    '0x0026 
    Const CSIDL_PROGRAM_FILESX86 As Integer = 42    '0x002a
    Const CSIDL_WINDOWS As Integer = 36    ' 0x0024
    Const CSIDL_SYSTEM As Integer = 37    ' 0x0025,
    Const CSIDL_SYSTEMX86 As Integer = 41    ' 0x0029,
    Const CSIDL_PROGRAM_FILES_COMMON = 43 ' 0x002b 
    Const CSIDL_PROGRAM_FILES_COMMONX86 As Integer = 44    ' 0x002c

    <DllImport("kernel32.dll", SetLastError:=True, CallingConvention:=CallingConvention.Winapi)> _
    Public Shared Function IsWow64Process(<[In]()> hProcess As IntPtr, <Out()> ByRef wow64Process As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("Shell32.dll")> _
    Private Shared Function SHGetSpecialFolderPath(<[In]()> hwndOwner As IntPtr, <Out()> lpszPath As StringBuilder, <[In]()> nFolder As Integer, <[In]()> fCreate As Integer) As Integer
    End Function

    Const PLATFORM6_INSTALL_KEY As String = "{8961E141-B307-4882-ABAD-77A3E76A40C1}"

#Region "Form Load and Close"
    Private Sub Form1_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            LblAction.Text = ""
            lblResult.Text = ""
            'If Date.Now > Date.Parse("09/24/2011", CultureInfo.InvariantCulture).AddDays(7.0) Then
            ' MsgBox("This application has expired, please post on ASCOM Talk (http://tech.groups.yahoo.com/group/ASCOM-Talk/messages) if you still need to remove your entire ASCOM Platform.", MsgBoxStyle.Critical, "Application Expired")
            ' btnRemove.Enabled = False
            ' End If
        Catch ex As Exception
            MsgBox("Load Exception 1: " & ex.ToString)
        End Try

        Try
            TL = New TraceLogger("", "ForceRemove")
            TL.Enabled = True
            TL.LogMessage("ForceRemove", "Program started")
        Catch ex As Exception
            MsgBox("Load Exception 2: " & ex.ToString)
        End Try
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            TL.Enabled = False
            TL.Dispose()
            TL = Nothing
        Catch
        End Try
    End Sub

    Private Sub btnExit_Click(sender As System.Object, e As System.EventArgs) Handles btnExit.Click
        End
    End Sub
#End Region

    Private Sub btnRemove_Click(sender As System.Object, e As System.EventArgs) Handles btnRemove.Click
        Dim dlgResult As MsgBoxResult
        Try
            Status("")
            Action("")
            dlgResult = MsgBox("Are you sure you want to FORCE remove your entire ASCOM Platform?", MsgBoxStyle.Critical Or MsgBoxStyle.YesNo, "Remove ASCOM")
            TL.LogMessage("ForceRemove", "Are you sure you want to FORCE remove your entire ASCOM Platform?")
            If dlgResult = MsgBoxResult.Yes Then
                TL.LogMessage("ForceRemove", "User said ""Yes""")
                TL.BlankLine()

                'RemoveInstallers()
                'RemoveProfile()
                'RemoveFiles()
                'RemoveGAC()
                'RemoveDekstopFilesAndLinks()
                RemoveGUIDs()

                Status("Completed")
                MsgBox("Platform has been removed", MsgBoxStyle.Information, "Remove ASCOM")
            Else
                TL.LogMessage("ForceRemove", "User said ""No""")
            End If
            Action("")
        Catch ex As Exception
            TL.LogMessageCrLf("ForceRemove", "Exception: " & ex.ToString)
        End Try
        TL.LogMessage("ForceRemove", "End of process")
    End Sub

    Private Sub RemoveInstallers()
        Dim InstallerKeys As New SortedList(Of String, String)
        Dim RKey As RegistryKey, SubKeys(), UninstallKey, UninstallProgram As String
        Dim Vals() As String = {""}
        Dim SplitChars() As Char = {" "}

        Const PLATFORM41 As String = "Platform 4.1"
        Const PLATFORM50A As String = "Platform 5.0A"
        Const PLATFORM50A_PRODUCT As String = "Platform 5.0A Product"
        Const PLATFORM50B As String = "Platform 5.0B"
        Const PLATFORM50B_PRODUCT As String = "Platform 5.0B Product"
        Const PLATFORM55 As String = "Platform 5.5"
        Const PLATFORM60 As String = "Platform 6.0"
        Const UNINSTALL_STRING As String = "UninstallString"
        Const ARGS60 As String = "/s MODIFY=FALSE REMOVE=TRUE UNINSTALL=YES"
        Const ARGS55 As String = "/VERYSILENT /NORESTART /LOG"

        Try
            Status("Removing installer references")
            TL.LogMessage("RemoveInstallers", "Started")
            TL.BlankLine()

            If Is64Bit() Then
                TL.LogMessage("RemoveInstallers", "64bit OS")
                InstallerKeys.Add(PLATFORM41, "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\ASCOM Platform 4.1")
                InstallerKeys.Add(PLATFORM50A, "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{075F543B-97C5-4118-9D54-93910DE03FE9}")
                InstallerKeys.Add(PLATFORM50A_PRODUCT, "SOFTWARE\Classes\Installer\Products\B345F5705C798114D9453919D00EF39E")
                InstallerKeys.Add(PLATFORM50B, "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{14C10725-0018-4534-AE5E-547C08B737B7}")
                InstallerKeys.Add(PLATFORM50B_PRODUCT, "SOFTWARE\Classes\Installer\Products\52701C4181004354EAE545C7807B737B")
                InstallerKeys.Add(PLATFORM55, "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\ASCOM.Platform.NET.Components_is1")
                InstallerKeys.Add(PLATFORM60, "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" & PLATFORM6_INSTALL_KEY)
                UninstallKey = "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            Else
                TL.LogMessage("RemoveInstallers", "32bit OS")
                InstallerKeys.Add(PLATFORM41, "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\ASCOM Platform 4.1")
                InstallerKeys.Add(PLATFORM50A, "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{075F543B-97C5-4118-9D54-93910DE03FE9}")
                InstallerKeys.Add(PLATFORM50A_PRODUCT, "SOFTWARE\Classes\Installer\Products\B345F5705C798114D9453919D00EF39E")
                InstallerKeys.Add(PLATFORM50B, "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{14C10725-0018-4534-AE5E-547C08B737B7}")
                InstallerKeys.Add(PLATFORM50B_PRODUCT, "SOFTWARE\Classes\Installer\Products\52701C4181004354EAE545C7807B737B")
                InstallerKeys.Add(PLATFORM55, "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\ASCOM.Platform.NET.Components_is1")
                InstallerKeys.Add(PLATFORM60, "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" & PLATFORM6_INSTALL_KEY)
                UninstallKey = "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
            End If

            If Not (Registry.LocalMachine.OpenSubKey(InstallerKeys(PLATFORM60)) Is Nothing) Then ' Try and uninstall Platform 6
                TL.LogMessage("Uninstall", PLATFORM60)
                UninstallProgram = Registry.LocalMachine.OpenSubKey(InstallerKeys(PLATFORM60)).GetValue(UNINSTALL_STRING)
                RunProcess(PLATFORM60, UninstallProgram, ARGS60)
            Else
                TL.LogMessage("Uninstall", "Installer key for Platform 6 not present")
            End If

            If Not (Registry.LocalMachine.OpenSubKey(InstallerKeys(PLATFORM55)) Is Nothing) Then ' Try and uninstall Platform 5.5
                TL.LogMessage("Uninstall", PLATFORM55)
                UninstallProgram = Registry.LocalMachine.OpenSubKey(InstallerKeys(PLATFORM55)).GetValue(UNINSTALL_STRING)
                RunProcess(PLATFORM55, UninstallProgram, ARGS55)
            Else
                TL.LogMessage("Uninstall", "Installer key for Platform 5.5 not present")
            End If

            If Not (Registry.LocalMachine.OpenSubKey(InstallerKeys(PLATFORM50B)) Is Nothing) Then ' Try and uninstall Platform 5.0B
                TL.LogMessage("Uninstall", PLATFORM50B)
                UninstallProgram = Registry.LocalMachine.OpenSubKey(InstallerKeys(PLATFORM50B)).GetValue(UNINSTALL_STRING)
                RunProcess(PLATFORM50B, "MsiExec.exe", SplitKey(UninstallProgram))
            Else
                TL.LogMessage("Uninstall", "Installer key for Platform 5.0B not present")
            End If

            If Not (Registry.LocalMachine.OpenSubKey(InstallerKeys(PLATFORM50A)) Is Nothing) Then ' Try and uninstall Platform 5.0A
                TL.LogMessage("Uninstall", PLATFORM50A)

                Try
                    'Now have to fix a missing registry key that fouls up the uninstaller - this was fixed in 5B but prevents 5A from uninstalling on 64bit systems
                    TL.LogMessage("Uninstall", "  Fixing missing AppId")
                    RKey = Registry.ClassesRoot.CreateSubKey("AppID\{DF2EB077-4D59-4231-9CB4-C61AD4ECB874}")
                    RKey.SetValue("", "Fixed registry key value")
                    RKey.Close()
                    RKey = Nothing
                    TL.LogMessage("Uninstall", "  Successfully set AppID\{DF2EB077-4D59-4231-9CB4-C61AD4ECB874}")
                Catch ex As Exception
                    TL.LogMessageCrLf("Uninstall", "Exception: " & ex.ToString)
                End Try

                UninstallProgram = Registry.LocalMachine.OpenSubKey(InstallerKeys(PLATFORM50A)).GetValue(UNINSTALL_STRING)
                RunProcess(PLATFORM50A, "MsiExec.exe", SplitKey(UninstallProgram))
            Else
                TL.LogMessage("Uninstall", "Installer key for Platform 5.0A not present")
            End If

            If Not (Registry.LocalMachine.OpenSubKey(InstallerKeys(PLATFORM41)) Is Nothing) Then ' Try and uninstall Platform 4.1
                Try
                    TL.LogMessage("Uninstall", PLATFORM41)
                    UninstallProgram = Registry.LocalMachine.OpenSubKey(InstallerKeys(PLATFORM41)).GetValue(UNINSTALL_STRING)
                    TL.LogMessage("Uninstall", "  Found uninstall string: """ & UninstallProgram & """")
                    Vals = UninstallProgram.Split(SplitChars, System.StringSplitOptions.RemoveEmptyEntries)
                    TL.LogMessage("Uninstall", "  Found uninstall values: """ + Vals(0) + """, """ + Vals(1) + """")
                Catch ex As Exception
                    TL.LogMessageCrLf("Uninstall", "Exception: " & ex.ToString)
                End Try
                RunProcess(PLATFORM41, Vals(0), "/S /Z " & Vals(1))
            Else
                TL.LogMessage("Uninstall", "Installer key for Platform 4.1 not present")
            End If
            TL.BlankLine()

            For Each Installer In InstallerKeys
                Try
                    'TL.LogMessage("RemoveInstallers", "Removing reference to: " & Installer.Key)
                    Registry.LocalMachine.DeleteSubKeyTree(Installer.Value)
                    TL.LogMessage("RemoveInstallers", "Reference to " & Installer.Key & " - Removed OK")
                Catch ex2 As ArgumentException
                    TL.LogMessage("RemoveInstallers", "Reference to " & Installer.Key & " - Not present")
                Catch ex2 As Exception
                    TL.LogMessageCrLf("RemoveInstallers", "Exception 2: " & ex2.ToString)
                End Try
            Next
            TL.BlankLine()

            TL.LogMessage("RemoveInstallers", "Removing installer references")
            RKey = Registry.LocalMachine.OpenSubKey(UninstallKey, True)
            SubKeys = RKey.GetSubKeyNames
            For Each SubKey As String In SubKeys
                Try
                    If SubKey.ToUpper.Contains("ASCOM PLATFORM") Then
                        TL.LogMessage("RemoveInstallers", "Removing Platform installer reference: " & SubKey)
                        RKey.DeleteSubKeyTree(SubKey)
                    End If
                Catch ex3 As Exception
                    TL.LogMessageCrLf("RemoveInstallers", "Exception 3: " & ex3.ToString)
                End Try
            Next

            ' Check for any Products remaining
            TL.BlankLine()
            Try
                RKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Classes\Installer\Products", True)
                SubKeys = RKey.GetSubKeyNames
                Dim ProductDescription As String

                For Each SubKey In SubKeys
                    ProductDescription = RKey.OpenSubKey(SubKey).GetValue("ProductName", "Product description not present")
                    TL.LogMessage("RemoveInstallers", "Found Product: " & ProductDescription)
                    If ProductDescription.ToUpper.Contains("ASCOM PLATFORM") Then
                        TL.LogMessage("RemoveInstallers", "  Deleting: " & ProductDescription)
                        RKey.DeleteSubKeyTree(SubKey)
                    End If
                Next

            Catch ex4 As Exception
                TL.LogMessageCrLf("RemoveInstallers", "Exception 4: " & ex4.ToString)
            End Try

            TL.LogMessage("RemoveInstallers", "Completed")
        Catch ex1 As Exception
            TL.LogMessageCrLf("RemoveInstallers", "Exception 1: " & ex1.ToString)
        End Try
        TL.BlankLine()
    End Sub

    Private Sub RemoveProfile()
        Dim RKey As RegistryKey

        Try
            Status("Removing RemoveProfile Entries")
            TL.LogMessage("RemoveProfile", "Started")

            If Is64Bit() Then
                Try
                    TL.LogMessage("RemoveProfile", "Removing Profile from 32bit registry location on a 64bit OS")
                    RKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Wow6432Node", True)
                    RKey.DeleteSubKeyTree("ASCOM")
                    TL.LogMessage("RemoveProfile", "  Removed OK")
                Catch ex2 As ArgumentException
                    TL.LogMessage("RemoveProfile", "  Not present")
                Catch ex1 As Exception
                    TL.LogMessageCrLf("RemoveProfile", "Exception 1: " & ex1.ToString)
                End Try
            Else
                Try
                    TL.LogMessage("RemoveProfile", "Removing Profile from registry on a 32bit OS")
                    RKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
                    RKey.DeleteSubKeyTree("ASCOM")
                    TL.LogMessage("RemoveProfile", "  Removed OK")
                Catch ex2 As ArgumentException
                    TL.LogMessage("RemoveProfile", "  Not present")
                Catch ex1 As Exception
                    TL.LogMessageCrLf("RemoveProfile", "Exception 1: " & ex1.ToString)
                End Try
            End If

            Try
                TL.LogMessage("RemoveProfile", "Removing ASCOM User preferences")
                RKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", True)
                RKey.DeleteSubKeyTree("ASCOM")
                TL.LogMessage("RemoveProfile", "  Removed OK")
            Catch ex2 As ArgumentException
                TL.LogMessage("RemoveProfile", "  Not present")
            Catch ex3 As Exception
                TL.LogMessageCrLf("RemoveProfile", "Exception 3: " & ex3.ToString)
            End Try

            TL.LogMessage("RemoveProfile", "Completed")
        Catch ex As Exception
            TL.LogMessageCrLf("RemoveProfile", "Exception: " & ex.ToString)
        End Try
        TL.BlankLine()
    End Sub

    Private Sub RemoveFiles()
        Dim Path As New StringBuilder(260), rc As Integer, ASCOMDirectory As String
        Dim DirInfo As DirectoryInfo, FileInfos() As FileInfo, DirInfos() As DirectoryInfo
        Dim Found As Boolean

        Try
            Status("Removing files")
            TL.LogMessage("RemoveFiles", "Started")

            If Is64Bit() Then
                Try
                    TL.LogMessage("RemoveFiles", "Removing ASCOM 32bit Common Files from a 64bit OS")
                    rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_PROGRAM_FILES_COMMONX86, 0)
                    ASCOMDirectory = Path.ToString() & "\ASCOM"
                    TL.LogMessage("RemoveFiles", "  ASCOM directory: " & ASCOMDirectory)
                    RemoveFilesRecurse(ASCOMDirectory)
                    TL.LogMessage("RemoveFiles", "  Removed OK")
                Catch ex As DirectoryNotFoundException
                    TL.LogMessage("RemoveFiles", "  Not present")
                Catch ex As Exception
                    TL.LogMessageCrLf("RemoveFiles", "Exception: " & ex.ToString)
                End Try
                TL.BlankLine()

                Try
                    TL.LogMessage("RemoveFiles", "Removing ASCOM 64bit Common Files from a 64bit OS")
                    rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_PROGRAM_FILES_COMMON, 0)
                    ASCOMDirectory = Path.ToString() & "\ASCOM"
                    TL.LogMessage("RemoveFiles", "  ASCOM directory: " & ASCOMDirectory)
                    RemoveFilesRecurse(ASCOMDirectory)
                    TL.LogMessage("RemoveFiles", "  Removed OK")
                Catch ex As DirectoryNotFoundException
                    TL.LogMessage("RemoveFiles", "  Not present")
                Catch ex As Exception
                    TL.LogMessageCrLf("RemoveFiles", "Exception: " & ex.ToString)
                End Try
                TL.BlankLine()

                Try
                    TL.LogMessage("RemoveFiles", "Removing ASCOM 32bit Program Files from a 64bit OS")
                    rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_PROGRAM_FILESX86, 0)
                    ASCOMDirectory = Path.ToString() & "\ASCOM"
                    TL.LogMessage("RemoveFiles", "  ASCOM directory: " & ASCOMDirectory)
                    RemoveFilesRecurse(ASCOMDirectory)
                    TL.LogMessage("RemoveFiles", "  Removed OK")
                Catch ex As DirectoryNotFoundException
                    TL.LogMessage("RemoveFiles", "  Not present")
                Catch ex As Exception
                    TL.LogMessageCrLf("RemoveFiles", "Exception: " & ex.ToString)
                End Try
                TL.BlankLine()

                Try
                    TL.LogMessage("RemoveFiles", "Removing ASCOM 64bit Program Files from a 64bit OS")
                    rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_PROGRAM_FILES, 0)
                    ASCOMDirectory = Path.ToString() & "\ASCOM"
                    TL.LogMessage("RemoveFiles", "  ASCOM directory: " & ASCOMDirectory)
                    RemoveFilesRecurse(ASCOMDirectory)
                    TL.LogMessage("RemoveFiles", "  Removed OK")
                Catch ex As DirectoryNotFoundException
                    TL.LogMessage("RemoveFiles", "  Not present")
                Catch ex As Exception
                    TL.LogMessageCrLf("RemoveFiles", "Exception: " & ex.ToString)
                End Try
                TL.BlankLine()
            Else
                Try
                    TL.LogMessage("RemoveFiles", "Removing ASCOM Common Files from a 32 bit OS")
                    rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_PROGRAM_FILES_COMMON, 0)
                    ASCOMDirectory = Path.ToString() & "\ASCOM"
                    TL.LogMessage("RemoveFiles", "  ASCOM directory: " & ASCOMDirectory)
                    RemoveFilesRecurse(ASCOMDirectory)
                    TL.LogMessage("RemoveFiles", "  Removed OK")
                Catch ex As DirectoryNotFoundException
                    TL.LogMessage("RemoveFiles", "  Not present")
                Catch ex As Exception
                    TL.LogMessageCrLf("RemoveFiles", "Exception: " & ex.ToString)
                End Try
                TL.BlankLine()

                Try
                    TL.LogMessage("RemoveFiles", "Removing ASCOM Program Files from a 32bit OS")
                    rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_PROGRAM_FILES, 0)
                    ASCOMDirectory = Path.ToString() & "\ASCOM"
                    TL.LogMessage("RemoveFiles", "  ASCOM directory: " & ASCOMDirectory)
                    RemoveFilesRecurse(ASCOMDirectory)
                    TL.LogMessage("RemoveFiles", "  Removed OK")
                Catch ex As DirectoryNotFoundException
                    TL.LogMessage("RemoveFiles", "  Not present")
                Catch ex As Exception
                    TL.LogMessageCrLf("RemoveFiles", "Exception: " & ex.ToString)
                End Try
                TL.BlankLine()
            End If

            TL.LogMessage("RemoveFiles", "Removing ASCOM 5.5 Profile Files")
            Try
                ASCOMDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\ASCOM"
                RemoveFilesRecurse(ASCOMDirectory)
            Catch ex As Exception
                TL.LogMessageCrLf("RemoveFiles", "Exception: " & ex.ToString)
            End Try
            TL.BlankLine()

            ' Remove any InstallAware install files remaining
            TL.LogMessage("RemoveFiles", "Removing InstallAware Installer Files")
            Try
                ASCOMDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                DirInfo = New DirectoryInfo(ASCOMDirectory) ' Get a directory info for the common application data directory
                DirInfos = DirInfo.GetDirectories ' Get a list of directories within the common application data directory

                Action(Microsoft.VisualBasic.Left(ASCOMDirectory, 70))
                Try ' Get file details for each directory in this folder
                    For Each DirInfo In DirInfos
                        Try
                            TL.LogMessageCrLf("RemoveFiles", "  Processing directory - " & "#" & DirInfo.Name & "#" & DirInfo.FullName & "#")
                            FileInfos = DirInfo.GetFiles ' Get the list of files in this directory
                            Found = False
                            For Each MyFile As FileInfo In FileInfos ' Now delete them
                                TL.LogMessageCrLf("RemoveFiles", "  Processing file - " & "#" & MyFile.Name & "#" & MyFile.FullName & "#")

                                If MyFile.Name.ToUpper = PLATFORM6_INSTALL_KEY.ToUpper Then
                                    Found = True
                                    TL.LogMessageCrLf("RemoveFiles", "  Found install directory directory - " & DirInfo.Name)
                                End If
                            Next
                            If Found Then
                                TL.LogMessageCrLf("RemoveFiles", "  Removing directory - " & DirInfo.FullName)
                                RemoveFilesRecurse(DirInfo.FullName)
                            End If
                        Catch ex As UnauthorizedAccessException
                            TL.LogMessage("RemoveFiles 2", "UnauthorizedAccessException for directory; " & DirInfo.FullName)
                        Catch ex As Exception
                            TL.LogMessageCrLf("RemoveFiles 2", "Exception: " & ex.ToString)
                        End Try

                    Next
                Catch ex As UnauthorizedAccessException
                    TL.LogMessage("RemoveFiles", "UnauthorizedAccessException for directory; " & DirInfo.FullName)
                Catch ex As Exception
                    TL.LogMessageCrLf("RemoveFiles", "Exception: " & ex.ToString)
                End Try

            Catch ex As Exception
                TL.LogMessageCrLf("RemoveFiles", "Exception: " & ex.ToString)
            End Try

        Catch ex1 As Exception
            TL.LogMessageCrLf("RemoveFiles", "Exception 1: " & ex1.ToString)
        End Try

        TL.LogMessage("RemoveFiles", "Completed")
        TL.BlankLine()
    End Sub

    Private Sub RemoveGAC()
        Dim pCache As IAssemblyCache, Outcome As REMOVE_OUTCOME
        Dim ae As IAssemblyEnum
        Dim an As IAssemblyName = Nothing
        Dim ASCOMAssemblyNames As Generic.SortedList(Of String, String)
        Dim assname As AssemblyName
        Dim RKey As RegistryKey, SubKeys(), Values() As String

        TL.LogMessage("RemoveGAC", "Started")
        Status("Removing GAC references")

        Try
            RKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Classes\Installer\Assemblies", True)
            SubKeys = RKey.GetSubKeyNames
            For Each SubKey As String In SubKeys
                Try
                    If SubKey.ToUpper.Contains("ASCOM") Then
                        TL.LogMessage("RemoveGAC", "Removing application reference: " & SubKey)
                        RKey.DeleteSubKeyTree(SubKey)
                    End If
                Catch ex4 As Exception
                    TL.LogMessageCrLf("RemoveGAC", "Exception 4: " & ex4.ToString)
                End Try
            Next
            TL.BlankLine()

            TL.LogMessage("RemoveGAC", "Removing Assembly Global Values")
            RKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Classes\Installer\Assemblies\Global", True)
            Values = RKey.GetValueNames
            For Each Value As String In Values
                Try
                    If Value.ToUpper.Contains("ASCOM") Then
                        TL.LogMessage("RemoveGAC", "Removing global value: " & Value)
                        RKey.DeleteValue(Value)
                    End If
                Catch ex4 As Exception
                    TL.LogMessageCrLf("RemoveGAC", "Exception 4: " & ex4.ToString)
                End Try
            Next
        Catch ex As NullReferenceException
            TL.LogMessageCrLf("RemoveGAC", "Registry Key HKLM\SOFTWARE\Classes\Installer\Assemblies does not exist")
        Catch ex As Exception
            TL.LogMessageCrLf("RemoveGAC", "Exception 3: " & ex.ToString)
        End Try
        TL.BlankLine()

        Try
            Status("Scanning Assemblies")
            ASCOMAssemblyNames = New Generic.SortedList(Of String, String)

            TL.LogMessage("RemoveGAC", "Assemblies registered in the GAC")
            ae = AssemblyCache.CreateGACEnum ' Get an enumerator for the GAC assemblies

            Do While (AssemblyCache.GetNextAssembly(ae, an) = 0) 'Enumerate the assemblies
                Try
                    assname = GetAssemblyName(an)
                    If InStr(assname.FullName, "ASCOM") > 0 Then 'Extra information for ASCOM files
                        TL.LogMessage("RemoveGAC", "Found: " & assname.FullName)
                        ASCOMAssemblyNames.Add(assname.FullName, assname.Name) 'Convert the fusion representation to a standard AssemblyName and get its full name
                    Else
                        TL.LogMessage("RemoveGAC", "Also : " & assname.FullName)
                    End If
                Catch ex As Exception
                    'Ignore an exceptions here due to duplicate names, these are all MS assemblies
                End Try

            Loop

            ' Get an IAssemblyCache interface
            pCache = AssemblyCache.CreateAssemblyCache()
            For Each AssemblyName In ASCOMAssemblyNames
                Try
                    TL.LogMessage("RemoveGAC", "Removing " & AssemblyName.Key)
                    TL.LogMessage("RemoveGAC", "  AssemblyCache.RemoveGAC - " & AssemblyName.Value)
                    Outcome = AssemblyCache.RemoveGAC(AssemblyName.Value)
                    If Outcome.ReturnCode = 0 Then
                        TL.LogMessage("RemoveGAC", "  OK - Uninstalled with no error!")
                    Else
                        TL.LogMessage("RemoveGAC #####", "  Bad RC: " & Outcome.ReturnCode)
                    End If

                    Select Case Outcome.Disposition
                        Case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_ALREADY_UNINSTALLED
                            TL.LogMessage("RemoveGAC #####", "  Outcome: Assembly already uninstalled")
                        Case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_DELETE_PENDING
                            TL.LogMessage("RemoveGAC #####", "   Outcome: Delete currently pending")
                        Case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_HAS_INSTALL_REFERENCES
                            TL.LogMessage("RemoveGAC #####", "  Outcome: Assembly has remaining install references")
                        Case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_REFERENCE_NOT_FOUND
                            TL.LogMessage("RemoveGAC #####", "  Outcome: Unable to find assembly - " & AssemblyName.Value & " - " & AssemblyName.Key)
                        Case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_STILL_IN_USE
                            TL.LogMessage("RemoveGA #####C", "  Outcome: Assembly still in use")
                        Case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNINSTALLED
                            TL.LogMessage("RemoveGAC", "  Outcome: Assembly uninstalled")
                        Case Else
                            TL.LogMessage("RemoveGAC #####", "  Unknown uninstall outcome code: " & Outcome.Disposition)
                    End Select
                Catch ex As Exception
                    TL.LogMessageCrLf("RemoveGAC", "Exception 2: " & ex.ToString)
                End Try
            Next
            TL.LogMessage("RemoveGAC", "Completed")

        Catch ex As Exception
            TL.LogMessageCrLf("RemoveGAC", "Exception 1: " & ex.ToString)
        End Try
        TL.BlankLine()
    End Sub

    Private Sub RemoveDekstopFilesAndLinks()
        Dim Path As New StringBuilder(260), rc As Integer, DesktopDirectory, StartMenuDirectory As String

        Status("Removing dekstop files and links")
        TL.LogMessage("RemoveDekstopFilesAndLinks", "Started")

        Try
            rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_COMMON_DESKTOPDIRECTORY, 0)
            DesktopDirectory = Path.ToString()
            TL.LogMessage("RemoveDekstopFilesAndLinks", "Desktop directory: " & DesktopDirectory)
            File.Delete(DesktopDirectory & "\ASCOM Diagnostics.lnk")
            File.Delete(DesktopDirectory & "\ASCOM Profile Explorer.lnk")
            File.Delete(DesktopDirectory & "\Profile Explorer.lnk")
            File.Delete(DesktopDirectory & "\ProfileExplorer.lnk")
        Catch ex As Exception
            TL.LogMessageCrLf("RemoveDekstopFilesAndLinks", "Exception: " & ex.ToString)
        End Try

        Try
            rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_COMMON_STARTMENU, 0)
            StartMenuDirectory = Path.ToString() & "\Programs\ASCOM Platform"
            TL.LogMessage("RemoveDekstopFilesAndLinks", "Start Menu directory: " & StartMenuDirectory)
            RemoveFilesRecurse(StartMenuDirectory)
        Catch ex As Exception
            TL.LogMessageCrLf("RemoveDekstopFilesAndLinks", "Exception: " & ex.ToString)
        End Try

        Try
            rc = SHGetSpecialFolderPath(IntPtr.Zero, Path, CSIDL_COMMON_STARTMENU, 0)
            StartMenuDirectory = Path.ToString() & "\Programs\ASCOM Platform 6"
            TL.LogMessage("RemoveDekstopFilesAndLinks", "Start Menu directory (P6): " & StartMenuDirectory)
            RemoveFilesRecurse(StartMenuDirectory)
        Catch ex As Exception
            TL.LogMessageCrLf("RemoveDekstopFilesAndLinks", "Exception: " & ex.ToString)
        End Try

        TL.LogMessage("RemoveDekstopFilesAndLinks", "Completed")
        TL.BlankLine()

    End Sub

    Private Sub RemoveGUIDs()

        TL.LogMessage("RemoveGUIDs", "Started")
        Status("Removing GUIDs")

        For Each Guid As Generic.KeyValuePair(Of String, String) In GUIDList.Members(TL)
            CleanGUID(Guid.Key)
        Next
        TL.LogMessage("RemoveGUIDs", "Completed")
    End Sub

#Region "Support Routines"

    Sub CleanGUID(GUIDKey As String)
        TL.LogMessage("CleanGUID", "Cleaning GUID: " & GUIDKey)
        CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("CLSID", True), GUIDKey)
        CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("Interface", True), GUIDKey)
        CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("Record", True), GUIDKey)
        CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("AppID", True), GUIDKey)
        CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("TypeLib", True), GUIDKey)
        If Is64Bit() Then
            CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("Wow6432Node\CLSID", True), GUIDKey)
            CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("Wow6432Node\Interface", True), GUIDKey)
            CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("Wow6432Node\AppID", True), GUIDKey)
            CleanRegistryLocation(Registry.ClassesRoot.OpenSubKey("Wow6432Node\TypeLib", True), GUIDKey)
        End If
    End Sub

    Sub CleanRegistryLocation(BaseKey As RegistryKey, GUIDSubKey As String)
        Try
            GUIDSubKey = "{" & GUIDSubKey & "}"
            BaseKey.DeleteSubKeyTree(GUIDSubKey)
            TL.LogMessage("CleanRegistryLocation", "  SubKey removed: " & BaseKey.Name & "\" & GUIDSubKey)
        Catch ex As ArgumentException
            TL.LogMessage("CleanRegistryLocation", "    SubKey does not exist: " & BaseKey.Name & "\" & GUIDSubKey)
        Catch ex As Exception
            TL.LogMessageCrLf("CleanRegistryLocation", ex.ToString())
        End Try
    End Sub


    Private Sub RemoveFilesRecurse(ByVal Folder As String)
        Dim DirInfo As DirectoryInfo
        Dim FileInfos() As FileInfo
        Dim DirInfos() As DirectoryInfo
        Try
            TL.LogMessageCrLf("RemoveFilesRecurse", "Processing folder - " & Folder)

            If Directory.Exists(Folder) Then
                DirInfo = New DirectoryInfo(Folder)
                Action(Microsoft.VisualBasic.Left(Folder, 70))
                Try ' Get file details for files in this folder
                    FileInfos = DirInfo.GetFiles
                    For Each MyFile As FileInfo In FileInfos ' Now delete them
                        TL.LogMessageCrLf("RemoveFilesRecurse", "  Erasing file - " & MyFile.Name)
                        MyFile.Attributes = FileAttributes.Normal
                        MyFile.Delete()
                    Next
                Catch ex As UnauthorizedAccessException
                    TL.LogMessage("RecurseProgramFiles 1", "UnauthorizedAccessException for directory; " & Folder)
                Catch ex As Exception
                    TL.LogMessageCrLf("RecurseProgramFiles 1", "Exception: " & ex.ToString)
                End Try

                Try ' Iterate over the sub directories of this folder
                    DirInfos = DirInfo.GetDirectories
                    For Each Directory As DirectoryInfo In DirInfos
                        RemoveFilesRecurse(Directory.FullName) ' Recursively process this sub directory
                    Next
                Catch ex As UnauthorizedAccessException
                    TL.LogMessage("RecurseProgramFiles 2", "UnauthorizedAccessException for directory; " & Folder)
                Catch ex As Exception
                    TL.LogMessageCrLf("RecurseProgramFiles 2", "Exception: " & ex.ToString)
                End Try

                TL.LogMessageCrLf("RemoveFilesRecurse", "Deleting folder - " & Folder)
                Directory.Delete(Folder) ' This directory should now be empty so remove it
            Else
                TL.LogMessageCrLf("RemoveFilesRecurse", "  Folder does not exist")
            End If
        Catch ex As UnauthorizedAccessException
            TL.LogMessage("RecurseProgramFiles 3", "UnauthorizedAccessException for directory; " & Folder)
        Catch ex As Exception
            TL.LogMessageCrLf("RecurseProgramFiles 3", "Exception: " & ex.ToString)
        End Try
    End Sub

    Private Sub Status(ByVal Msg As String)
        Application.DoEvents()
        lblResult.Text = Msg
        Application.DoEvents()
    End Sub

    Private Sub Action(ByVal Msg As String)
        Application.DoEvents()
        LblAction.Text = Msg
        Application.DoEvents()
    End Sub

    Private Function GetAssemblyName(ByVal nameRef As IAssemblyName) As AssemblyName
        Dim AssName As New AssemblyName()
        Try
            AssName.Name = AssemblyCache.GetName(nameRef)
            AssName.Version = AssemblyCache.GetVersion(nameRef)
            AssName.CultureInfo = AssemblyCache.GetCulture(nameRef)
            AssName.SetPublicKeyToken(AssemblyCache.GetPublicKeyToken(nameRef))
        Catch ex As Exception
            TL.LogMessageCrLf("GetAssemblyName", "Exception: " & ex.ToString)
        End Try
        Return AssName
    End Function

    Private Function Is64Bit() As Boolean
        Return (IntPtr.Size = 8)
    End Function

    'run the uninstaller
    Public Sub RunProcess(ByVal InstallerName As String, ByVal processToRun As String, ByVal args As String)
        Dim startInfo As ProcessStartInfo, myProcess As Process

        Try
            TL.LogMessage("RunProcess", "  Process: " & processToRun)
            TL.LogMessage("RunProcess", "  Args: " & args)

            startInfo = New ProcessStartInfo(processToRun)
            startInfo.Arguments = args

            Dim StartTime As Date
            StartTime = Now
            myProcess = Process.Start(startInfo)
            Do
                Threading.Thread.Sleep(10)
                Action("Removing: " & InstallerName & " - " & Now.Subtract(StartTime).Seconds)
                Application.DoEvents()
            Loop Until myProcess.HasExited

            myProcess.WaitForExit()

            TL.LogMessage("RunProcess", "  Completed - exit code: " & myProcess.ExitCode.ToString)

            myProcess.Close()
            myProcess.Dispose()
            myProcess = Nothing
        Catch e As Exception
            TL.LogMessageCrLf("RunProcess", "Exception: " & e.ToString())
        End Try
    End Sub

    'split the installer string and select the first argument, converting /I to /q /x
    Public Shared Function SplitKey(keyToSplit As String) As String
        Dim SplitChars() As Char = {" "}
        Dim s As String() = keyToSplit.Split(SplitChars)
        s(1) = s(1).Replace("/I", "/q /x ")
        Return s(1)
    End Function

#End Region

End Class
