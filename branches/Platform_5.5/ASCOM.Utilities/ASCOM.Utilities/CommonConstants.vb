'Common constants for the ASCOM.Utilities namesapce

Imports System.Reflection
Imports System.Runtime.InteropServices
Imports ASCOM.Utilities

#Region "Common Constants"

Module CommonConstants
    'NOTE: Platform version number is defined in the MigrateProfile utilitiy
    Friend Const SERIAL_FILE_NAME_VARNAME As String = "SerTraceFile" 'Constant naming the profile trace file variable name
    Friend Const SERIAL_AUTO_FILENAME As String = "C:\SerialTraceAuto.txt" 'Special value to indicate use of automatic trace filenames
    Friend Const SERIAL_DEFAULT_FILENAME As String = "C:\SerialTrace.txt" 'Default manual trace filename
    Friend Const SERIAL_DEBUG_TRACE_VARNAME As String = "SerDebugTrace" 'Constant naming the profile trace file variable name
    Friend Const SERIAL_FORCED_COMPORTS_VARNAME As String = "ForcedCOMPorts" 'Constant listing COM ports that will be forced to be present
    Friend Const SERIAL_IGNORED_COMPORTS_VARNAME As String = "IgnoredCOMPorts" 'Constant listing COM ports that will be ignored if present

    'Utilities configuration constants
    Friend Const TRACE_XMLACCESS As String = "Trace XMLAccess", TRACE_XMLACCESS_DEFAULT As Boolean = False
    Friend Const TRACE_PROFILE As String = "Trace Profile", TRACE_PROFILE_DEFAULT As Boolean = False
    Friend Const SERIAL_TRACE_DEBUG As String = "Serial Trace Debug", SERIAL_TRACE_DEBUG_DEFAULT As Boolean = False

    Friend Const PROFILE_MUTEX_NAME As String = "ASCOMProfileMutex" 'Name and timout value for the Profile mutex than ensures only one profile action happens at a time
    Friend Const PROFILE_MUTEX_TIMEOUT As Integer = 5000
End Module

#End Region

#Region "Version Code"

Module VersionCode
    Friend Sub RunningVersions(ByVal TL As TraceLogger)
        Dim Assemblies() As Assembly 'Define an array of assembly information
        Dim AppDom As System.AppDomain = AppDomain.CurrentDomain

        'Get Operating system information
        Dim OS As System.OperatingSystem = System.Environment.OSVersion

        Try 'Make sure this code never throws an exception back to the caller
            TL.LogMessage("Versions", "OS Version: " & OS.Platform & ", Service Pack: " & OS.ServicePack & ", Full: " & OS.VersionString)
            If IsWow64() Then 'Application is under WoW64 so OS must be 64bit
                TL.LogMessage("Versions", "Operating system is 64bit")
            Else 'Could be 32bit or 64bit Use IntPtr
                Select Case System.IntPtr.Size
                    Case 4
                        TL.LogMessage("Versions", "Operating system is 32bit")
                    Case 8
                        TL.LogMessage("Versions", "Operating system is 64bit")
                    Case Else
                        TL.LogMessage("Versions", "Operating system is unknown bits, PTR length is: " & System.IntPtr.Size)
                End Select
            End If
            Select Case System.IntPtr.Size
                Case 4
                    TL.LogMessage("Versions", "Application is 32bit")
                Case 8
                    TL.LogMessage("Versions", "Application is 64bit")
                Case Else
                    TL.LogMessage("Versions", "Application is unknown bits, PTR length is: " & System.IntPtr.Size)
            End Select
            TL.LogMessage("Versions", "")

            'Get common language runtime version
            TL.LogMessage("Versions", "CLR version: " & System.Environment.Version.ToString)

            'Get file system information
            Dim UserDomainName As String = System.Environment.UserDomainName
            Dim UserName As String = System.Environment.UserName
            Dim MachineName As String = System.Environment.MachineName
            Dim ProcCount As Integer = System.Environment.ProcessorCount
            Dim SysDir As String = System.Environment.SystemDirectory
            Dim WorkSet As Long = System.Environment.WorkingSet
            TL.LogMessage("Versions", "Machine name: " & MachineName & " UserName: " & UserName & " DomainName: " & UserDomainName)
            TL.LogMessage("Versions", "Number of processors: " & ProcCount & " System directory: " & SysDir & " Working set size: " & WorkSet & " bytes")
            TL.LogMessage("Versions", "")

            'Get fully qualified paths to particular directories in a non OS specific way
            'There are many more options in the SpecialFolders Enum than are shown here!
            TL.LogMessage("Versions", "My Documents:            " & System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
            TL.LogMessage("Versions", "Application Data:        " & System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
            TL.LogMessage("Versions", "Common Application Data: " & System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData))
            TL.LogMessage("Versions", "Program Files:           " & System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles))
            TL.LogMessage("Versions", "Common Files:            " & System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles))
            TL.LogMessage("Versions", "System:                  " & System.Environment.GetFolderPath(Environment.SpecialFolder.System))
            TL.LogMessage("Versions", "Current:                 " & System.Environment.CurrentDirectory)
            TL.LogMessage("Versions", "")

            'Get loaded assemblies
            Assemblies = AppDom.GetAssemblies 'Get a list of loaded assemblies
            For Each FoundAssembly As Assembly In Assemblies
                TL.LogMessage("Versions", "Loaded Assemblies: " & FoundAssembly.GetName.Name & " " & FoundAssembly.GetName.Version.ToString)
            Next
            TL.LogMessage("Versions", "")

            'Get assembly versions
            AssemblyInfo(TL, "Executing Assembly", Assembly.GetExecutingAssembly)
            AssemblyInfo(TL, "Entry Assembly", Assembly.GetEntryAssembly)
            AssemblyInfo(TL, "Calling Assembly", Assembly.GetCallingAssembly)
            TL.LogMessage("Versions", "")
        Catch ex As Exception 'Just log the exception, we don't want the caller to know this diagnostic code failed
            TL.LogMessage("Versions", "Exception: " & ex.ToString)
        End Try
    End Sub

    Sub AssemblyInfo(ByVal TL As TraceLogger, ByVal AssName As String, ByVal Ass As Assembly)
        Dim FileVer As FileVersionInfo
        Dim AssblyName As AssemblyName, Vers As Version, VerString As String
        Dim Location, FVer, FName As String

        AssName = Left(AssName & ":" & Space(20), 19)

        If Not Ass Is Nothing Then
            Try
                AssblyName = Ass.GetName
                If AssblyName Is Nothing Then
                    TL.LogMessage("Versions", AssName & " Assembly name is missing, cannot determine version")
                Else
                    Vers = AssblyName.Version
                    If Vers Is Nothing Then
                        TL.LogMessage("Versions", AssName & " Assembly version is missing, cannot determine version")
                    Else
                        VerString = Vers.ToString
                        If Not String.IsNullOrEmpty(VerString) Then
                            TL.LogMessage("Versions", AssName & " AssemblyVersion: " & VerString)
                        Else
                            TL.LogMessage("Versions", AssName & " Assembly version string is null or empty, cannot determine assembly version")
                        End If
                    End If
                End If
            Catch ex As Exception
                TL.LogMessage("AssemblyInfo", "Exception EX1: " & ex.ToString)
            End Try

            Try
                Location = Ass.Location
                If Location Is Nothing Then
                    TL.LogMessage("Versions", AssName & "Assembly location is missing, cannot determine file version")
                Else
                    FileVer = FileVersionInfo.GetVersionInfo(Location)
                    If FileVer Is Nothing Then
                        TL.LogMessage("Versions", AssName & " File version object is null, cannot determine file version number")
                    Else
                        FVer = FileVer.FileVersion
                        If Not String.IsNullOrEmpty(FVer) Then
                            TL.LogMessage("Versions", AssName & " FileVersion: " & FVer)
                        Else
                            TL.LogMessage("Versions", AssName & " File version string is null or empty, cannot determine file version")
                        End If
                    End If
                End If
            Catch ex As Exception
                TL.LogMessage("AssemblyInfo", "Exception EX2: " & ex.ToString)
            End Try

            Try
                AssblyName = Ass.GetName
                If AssblyName Is Nothing Then
                    TL.LogMessage("Versions", AssName & " Assembly name is missing, cannot determine full name")
                Else
                    FName = AssblyName.FullName
                    If Not String.IsNullOrEmpty(FName) Then
                        TL.LogMessage("Versions", AssName & " Name: " & FName)
                    Else
                        TL.LogMessage("Versions", AssName & " Full name string is null or empty, cannot determine full name")
                    End If

                End If
            Catch ex As Exception
                TL.LogMessage("AssemblyInfo", "Exception EX3: " & ex.ToString)
            End Try

            Try
                TL.LogMessage("Versions", AssName & " CodeBase: " & Ass.GetName.CodeBase)
            Catch ex As Exception
                TL.LogMessage("AssemblyInfo", "Exception EX4: " & ex.ToString)
            End Try

            Try
                TL.LogMessage("Versions", AssName & " Location: " & Ass.Location)
            Catch ex As Exception
                TL.LogMessage("AssemblyInfo", "Exception EX5: " & ex.ToString)
            End Try

            Try
                TL.LogMessage("Versions", AssName & " From GAC: " & Ass.GlobalAssemblyCache.ToString)
            Catch ex As Exception
                TL.LogMessage("AssemblyInfo", "Exception EX6: " & ex.ToString)
            End Try
        Else
            TL.LogMessage("Versions", AssName & " No assembly found")
        End If
    End Sub

    Private Function IsWow64() As Boolean

        Dim value As IntPtr
        value = System.Diagnostics.Process.GetCurrentProcess.Handle

        'Dim processHandle As Long = GetProcessHandle(System.Diagnostics.Process.GetCurrentProcess().Id)
        Dim retVal As Boolean
        If IsWow64Process(value, retVal) Then
            Return retVal
        Else
            Return False
        End If
    End Function

    <DllImport("Kernel32.dll", SetLastError:=True, CallingConvention:=CallingConvention.Winapi)> _
          Public Function IsWow64Process( _
          ByVal hProcess As System.IntPtr, _
          ByRef wow64Process As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

End Module
#End Region