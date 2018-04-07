'These items are shared between the ASCOM.Utilities and ASCOM.Astrometry assemblies

Imports Microsoft.Win32
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Globalization
Imports ASCOM.Utilities
Imports System.Collections.Generic
Imports System.Diagnostics
Imports ASCOM.Utilities.Serial

#Region "Registry Utility Code"
Module RegistryCommonCode
    Friend Function GetWaitType(ByVal p_Name As String, ByVal p_DefaultValue As ASCOM.Utilities.Serial.WaitType) As WaitType
        Dim l_Value As WaitType
        Dim m_HKCU, m_SettingsKey As RegistryKey

        m_HKCU = Registry.CurrentUser
        m_HKCU.CreateSubKey(REGISTRY_UTILITIES_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_UTILITIES_FOLDER, True)

        Try
            If m_SettingsKey.GetValueKind(p_Name) = RegistryValueKind.String Then ' Value does exist
                l_Value = CType([Enum].Parse(GetType(WaitType), m_SettingsKey.GetValue(p_Name).ToString), WaitType)
            End If
        Catch ex As System.IO.IOException 'Value doesn't exist so create it
            Try
                SetName(p_Name, p_DefaultValue.ToString)
                l_Value = p_DefaultValue
            Catch ex1 As Exception ' Unable to create value so just return the default value
                l_Value = p_DefaultValue
            End Try
        Catch ex As Exception
            l_Value = p_DefaultValue
        End Try

        m_SettingsKey.Flush() 'Clean up registry keys
        m_SettingsKey.Close()
        m_SettingsKey = Nothing
        m_HKCU.Flush()
        m_HKCU.Close()
        m_HKCU = Nothing

        Return l_Value
    End Function

    Friend Function GetBool(ByVal p_Name As String, ByVal p_DefaultValue As Boolean) As Boolean
        Dim l_Value As Boolean
        Dim m_HKCU, m_SettingsKey As RegistryKey

        m_HKCU = Registry.CurrentUser
        m_HKCU.CreateSubKey(REGISTRY_UTILITIES_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_UTILITIES_FOLDER, True)

        Try
            If m_SettingsKey.GetValueKind(p_Name) = RegistryValueKind.String Then ' Value does exist
                l_Value = CBool(m_SettingsKey.GetValue(p_Name))
            End If
        Catch ex As System.IO.IOException 'Value doesn't exist so create it
            Try
                SetName(p_Name, p_DefaultValue.ToString)
                l_Value = p_DefaultValue
            Catch ex1 As Exception ' Unable to create value so just return the default value
                l_Value = p_DefaultValue
            End Try
        Catch ex As Exception
            l_Value = p_DefaultValue
        End Try
        m_SettingsKey.Flush() 'Clean up registry keys
        m_SettingsKey.Close()
        m_SettingsKey = Nothing
        m_HKCU.Flush()
        m_HKCU.Close()
        m_HKCU = Nothing

        Return l_Value
    End Function
    Friend Function GetString(ByVal p_Name As String, ByVal p_DefaultValue As String) As String
        Dim l_Value As String = ""
        Dim m_HKCU, m_SettingsKey As RegistryKey

        m_HKCU = Registry.CurrentUser
        m_HKCU.CreateSubKey(REGISTRY_UTILITIES_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_UTILITIES_FOLDER, True)

        Try
            If m_SettingsKey.GetValueKind(p_Name) = RegistryValueKind.String Then ' Value does exist
                l_Value = m_SettingsKey.GetValue(p_Name).ToString
            End If
        Catch ex As System.IO.IOException 'Value doesn't exist so create it
            Try
                SetName(p_Name, p_DefaultValue.ToString)
                l_Value = p_DefaultValue
            Catch ex1 As Exception ' Unable to create value so just return the default value
                l_Value = p_DefaultValue
            End Try
        Catch ex As Exception
            l_Value = p_DefaultValue
        End Try
        m_SettingsKey.Flush() 'Clean up registry keys
        m_SettingsKey.Close()
        m_SettingsKey = Nothing
        m_HKCU.Flush()
        m_HKCU.Close()
        m_HKCU = Nothing

        Return l_Value
    End Function
    Friend Function GetDouble(ByVal p_Key As RegistryKey, ByVal p_Name As String, ByVal p_DefaultValue As Double) As Double
        Dim l_Value As Double
        Dim m_HKCU, m_SettingsKey As RegistryKey

        m_HKCU = Registry.CurrentUser
        m_HKCU.CreateSubKey(REGISTRY_UTILITIES_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_UTILITIES_FOLDER, True)

        'LogMsg("GetDouble", GlobalVarsAndCode.MessageLevel.msgDebug, p_Name.ToString & " " & p_DefaultValue.ToString)
        Try
            If p_Key.GetValueKind(p_Name) = RegistryValueKind.String Then ' Value does exist
                l_Value = CDbl(p_Key.GetValue(p_Name))
            End If
        Catch ex As System.IO.IOException 'Value doesn't exist so create it
            Try
                SetName(p_Name, p_DefaultValue.ToString)
                l_Value = p_DefaultValue
            Catch ex1 As Exception ' Unable to create value so just return the default value
                l_Value = p_DefaultValue
            End Try
        Catch ex As Exception
            l_Value = p_DefaultValue
        End Try
        m_SettingsKey.Flush() 'Clean up registry keys
        m_SettingsKey.Close()
        m_SettingsKey = Nothing
        m_HKCU.Flush()
        m_HKCU.Close()
        m_HKCU = Nothing

        Return l_Value
    End Function
    Friend Function GetDate(ByVal p_Name As String, ByVal p_DefaultValue As Date) As Date
        Dim l_Value As Date
        Dim m_HKCU, m_SettingsKey As RegistryKey

        m_HKCU = Registry.CurrentUser
        m_HKCU.CreateSubKey(REGISTRY_UTILITIES_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_UTILITIES_FOLDER, True)

        Try
            If m_SettingsKey.GetValueKind(p_Name) = RegistryValueKind.String Then ' Value does exist
                l_Value = CDate(m_SettingsKey.GetValue(p_Name))
            End If
        Catch ex As System.IO.IOException 'Value doesn't exist so create it
            Try
                SetName(p_Name, p_DefaultValue.ToString)
                l_Value = p_DefaultValue
            Catch ex1 As Exception ' Unable to create value so just return the default value
                l_Value = p_DefaultValue
            End Try
        Catch ex As Exception
            l_Value = p_DefaultValue
        End Try
        m_SettingsKey.Flush() 'Clean up registry keys
        m_SettingsKey.Close()
        m_SettingsKey = Nothing
        m_HKCU.Flush()
        m_HKCU.Close()
        m_HKCU = Nothing

        Return l_Value
    End Function
    Friend Sub SetName(ByVal p_Name As String, ByVal p_Value As String)
        Dim m_HKCU, m_SettingsKey As RegistryKey

        m_HKCU = Registry.CurrentUser
        m_HKCU.CreateSubKey(REGISTRY_UTILITIES_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_UTILITIES_FOLDER, True)

        m_SettingsKey.SetValue(p_Name, p_Value.ToString, RegistryValueKind.String)
        m_SettingsKey.Flush() 'Clean up registry keys
        m_SettingsKey.Close()
        m_SettingsKey = Nothing
        m_HKCU.Flush()
        m_HKCU.Close()
        m_HKCU = Nothing

    End Sub
End Module
#End Region

#Region "Windows event log code"
Module EventLogCode
    ''' <summary>
    ''' Add an event record to the ASCOM Windows event log
    ''' </summary>
    ''' <param name="Caller">Name of routine creating the event</param>
    ''' <param name="Msg">Event message</param>
    ''' <param name="Severity">Event severity</param>
    ''' <param name="Id">Id number</param>
    ''' <param name="Except">Initiating exception or Nothing</param>
    ''' <remarks></remarks>
    Friend Sub LogEvent(ByVal Caller As String, ByVal Msg As String, ByVal Severity As EventLogEntryType, ByVal Id As EventLogErrors, ByVal Except As String)
        Dim ELog As EventLog, MsgTxt As String

        ' During Platform 6 RC testing a report was received showing that a failure in this code had caused a bad Profile migration
        ' There was no problem with the migration code, the issue was caused by the event log code throwing an unexpected exception back to MigrateProfile
        ' It is wrong that an error in logging code should cause a client process to fail, so this code has been 
        ' made more robust and ultimately will swallow exceptions silently rather than throwing an unexpected exception back to the caller

        Try
            If Not EventLog.SourceExists(EVENT_SOURCE) Then 'Create the event log if it doesn't exist
                EventLog.CreateEventSource(EVENT_SOURCE, EVENTLOG_NAME)
                ELog = New EventLog(EVENTLOG_NAME, ".", EVENT_SOURCE) 'Create a pointer to the event log
                ELog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0) 'Force the policy to overwrite oldest
                ELog.MaximumKilobytes = 1024 ' Set the maximum log size to 1024kb, the Win 7 minimum size
                ELog.Close() 'Force the log file to be created by closing the log
                ELog.Dispose()
                ELog = Nothing

                'MSDN documentation advises waiting before writing, first time to a newly created event log file but doesn't say how long...
                ' Waiting 3 seconds to allow the log to be created by the OS
                Threading.Thread.Sleep(3000)

                'Try and create the initial log message
                ELog = New EventLog(EVENTLOG_NAME, ".", EVENT_SOURCE) 'Create a pointer to the event log
                ELog.WriteEntry("Successfully created event log - Policy: " & ELog.OverflowAction.ToString & ", Size: " & ELog.MaximumKilobytes & "kb", EventLogEntryType.Information, EventLogErrors.EventLogCreated)
                ELog.Close()
                ELog.Dispose()
            End If

            ' Write the event to the log
            ELog = New EventLog(EVENTLOG_NAME, ".", EVENT_SOURCE) 'Create a pointer to the event log

            MsgTxt = Caller & " - " & Msg 'Format the message to be logged
            If Not Except Is Nothing Then MsgTxt += vbCrLf & Except
            ELog.WriteEntry(MsgTxt, Severity, Id) 'Write the message to the error log

            ELog.Close()
            ELog.Dispose()
        Catch ex As System.ComponentModel.Win32Exception ' Special handling because these exceptions contain error codes we may want to know
            Try
                Dim TodaysDateTime As String = Format(Now(), "dd MMMM yyyy HH:mm:ss.fff")
                Dim ErrorLog As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & GlobalConstants.EVENTLOG_ERRORS
                Dim MessageLog As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & GlobalConstants.EVENTLOG_MESSAGES

                ' Write to backup eventlog message and error logs
                File.AppendAllText(ErrorLog, TodaysDateTime & " ErrorCode: 0x" & Hex(ex.ErrorCode) & " NativeErrorCode: 0x" & Hex(ex.NativeErrorCode) & " " & ex.ToString & vbCrLf)
                File.AppendAllText(MessageLog, TodaysDateTime & " " & Caller & " " & Msg & " " & Severity.ToString & " " & Id.ToString & " " & Except & vbCrLf)
            Catch ex1 As Exception ' Ignore exceptions here, the PC seems to be in a catastrophic failure!

            End Try
        Catch ex As Exception ' Catch all other exceptions
            'Somthing bad happened when writing to the event log so try and log it in a log file on the file system
            Try
                Dim TodaysDateTime As String = Format(Now(), "dd MMMM yyyy HH:mm:ss.fff")
                Dim ErrorLog As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & GlobalConstants.EVENTLOG_ERRORS
                Dim MessageLog As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & GlobalConstants.EVENTLOG_MESSAGES

                ' Write to backup eventlog message and error logs
                File.AppendAllText(ErrorLog, TodaysDateTime & " " & ex.ToString & vbCrLf)
                File.AppendAllText(MessageLog, TodaysDateTime & " " & Caller & " " & Msg & " " & Severity.ToString & " " & Id.ToString & " " & Except & vbCrLf)
            Catch ex1 As Exception ' Ignore exceptions here, the PC seems to be in a catastrophic failure!

            End Try
        End Try
    End Sub
End Module

#End Region

#Region "Version Code"

Module VersionCode
    Friend Sub RunningVersions(ByVal TL As TraceLogger)
        Dim Assemblies() As Assembly 'Define an array of assembly information
        Dim AppDom As System.AppDomain = AppDomain.CurrentDomain

        'Get Operating system information
        Dim OS As System.OperatingSystem = System.Environment.OSVersion

        Try
            TL.LogMessage("Versions", "Main Process: " & Process.GetCurrentProcess().MainModule.FileName) 'Get the name of the executable without path or file extension
            Dim FV As FileVersionInfo
            FV = Process.GetCurrentProcess().MainModule.FileVersionInfo 'Get the name of the executable without path or file extension
            TL.LogMessageCrLf("Versions", "  Product:  " & FV.ProductName & " " & FV.ProductVersion)
            TL.LogMessageCrLf("Versions", "  File:     " & FV.FileDescription & " " & FV.FileVersion)
            TL.LogMessageCrLf("Versions", "  Language: " & FV.Language)
            TL.BlankLine()
        Catch ex As Exception
            TL.LogMessage("Versions", "Exception EX0: " & ex.ToString)
        End Try

        Try 'Make sure this code never throws an exception back to the caller
            TL.LogMessage("Versions", "OS Version: " & OS.Platform & ", Service Pack: " & OS.ServicePack & ", Full: " & OS.VersionString)
            Select Case OSBits()
                Case Bitness.Bits32
                    TL.LogMessage("Versions", "Operating system is 32bit")
                Case Bitness.Bits64
                    TL.LogMessage("Versions", "Operating system is 64bit")
                Case Else
                    TL.LogMessage("Versions", "Operating system is unknown bits, PTR length is: " & System.IntPtr.Size)
            End Select

            Select Case ApplicationBits()
                Case Bitness.Bits32
                    TL.LogMessage("Versions", "Application is 32bit")
                Case Bitness.Bits64
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
            AssemblyInfo(TL, "Entry Assembly", Assembly.GetEntryAssembly)
            AssemblyInfo(TL, "Executing Assembly", Assembly.GetExecutingAssembly)
            TL.BlankLine()
        Catch ex As Exception 'Just log the exception, we don't want the caller to know this diagnostic code failed
            TL.LogMessageCrLf("Versions", "Unexpected exception: " & ex.ToString)
        End Try
    End Sub

    Friend Enum Bitness
        Bits32
        Bits64
        BitsMSIL
        BitsUnknown
    End Enum

    Friend Function OSBits() As Bitness
        If IsWow64() Then 'Application is under WoW64 so OS must be 64bit
            Return Bitness.Bits64
        Else 'Could be 32bit or 64bit Use IntPtr
            Select Case System.IntPtr.Size
                Case 4
                    Return Bitness.Bits32
                Case 8
                    Return Bitness.Bits64
                Case Else
                    Return Bitness.BitsUnknown
            End Select
        End If

    End Function

    Friend Function ApplicationBits() As Bitness
        Select Case System.IntPtr.Size
            Case 4
                Return Bitness.Bits32
            Case 8
                Return Bitness.Bits64
            Case Else
                Return Bitness.BitsUnknown
        End Select
    End Function

    Friend Sub AssemblyInfo(ByVal TL As TraceLogger, ByVal AssName As String, ByVal Ass As Assembly)
        Dim FileVer As FileVersionInfo
        Dim AssblyName As AssemblyName, Vers As Version, VerString, FVer, FName As String
        Dim Location As String = Nothing

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
                If String.IsNullOrEmpty(Location) Then
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
                If Not String.IsNullOrEmpty(Location) Then
                    TL.LogMessage("Versions", AssName & " Location: " & Location)
                Else
                    TL.LogMessage("Versions", AssName & " Location is null or empty, cannot display location")
                End If

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

    ''' <summary>
    ''' Returns true when the application is 32bit and running on a 64bit OS
    ''' </summary>
    ''' <returns>True when the application is 32bit and running on a 64bit OS</returns>
    ''' <remarks></remarks>
    Private Function IsWow64() As Boolean
        Dim value As IntPtr
        value = System.Diagnostics.Process.GetCurrentProcess.Handle

        Dim retVal As Boolean
        If IsWow64Process(value, retVal) Then
            Return retVal
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Determines whether the specified process is running under WOW64 i.e. is a 32bit application running on a 64bit OS.
    ''' </summary>
    ''' <param name="hProcess">A handle to the process. The handle must have the PROCESS_QUERY_INFORMATION or PROCESS_QUERY_LIMITED_INFORMATION access right. 
    ''' For more information, see Process Security and Access Rights.Windows Server 2003 and Windows XP:  
    ''' The handle must have the PROCESS_QUERY_INFORMATION access right.</param>
    ''' <param name="wow64Process">A pointer to a value that is set to TRUE if the process is running under WOW64. If the process is running under 
    ''' 32-bit Windows, the value is set to FALSE. If the process is a 64-bit application running under 64-bit Windows, the value is also set to FALSE.</param>
    ''' <returns>If the function succeeds, the return value is a nonzero value. If the function fails, the return value is zero. To get extended 
    ''' error information, call GetLastError.</returns>
    ''' <remarks></remarks>
    <DllImport("Kernel32.dll", SetLastError:=True, CallingConvention:=CallingConvention.Winapi)>
    Private Function IsWow64Process(
              ByVal hProcess As System.IntPtr,
              ByRef wow64Process As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    ''' <summary>
    ''' Return a message when a driver is not compatible with the requested 32/64bit application type. Returns an empty string if the driver is compatible
    ''' </summary>
    ''' <param name="ProgID">ProgID of the driver to be assessed</param>
    ''' <param name="RequiredBitness">Application bitness for which application compatibility should be tested</param>
    ''' <returns>String compatibility message or empty string if driver is fully compatible</returns>
    ''' <remarks></remarks>
    Friend Function DriverCompatibilityMessage(ByVal ProgID As String, ByVal RequiredBitness As Bitness, ByVal TL As TraceLogger) As String
        'Dim Drivers32Bit, Drivers64Bit As Generic.SortedList(Of String, String)
        Dim ProfileStore As RegistryAccess
        Dim InProcServer As PEReader = Nothing, Registered64Bit As Boolean, InprocServerBitness As Bitness
        Dim RK, RKInprocServer32 As RegistryKey, CLSID, InprocFilePath, CodeBase As String
        Dim RK32 As RegistryKey = Nothing
        Dim RK64 As RegistryKey = Nothing
        Dim AssemblyFullName As String, LoadedAssembly As Assembly
        Dim peKind As PortableExecutableKinds, machine As ImageFileMachine, Modules() As [Module]

        ProfileStore = New RegistryAccess("DriverCompatibilityMessage") 'Get access to the profile store

        DriverCompatibilityMessage = "" 'Set default return value as OK
        TL.LogMessage("DriverCompatibility", "     ProgID: " & ProgID & ", Bitness: " & RequiredBitness.ToString)
        'Parse the COM registry section to determine whether this ProgID is an in-process DLL server.
        'If it is then parse the executable to determine whether it is a 32bit only driver and gie a suitable message if it is
        'Picks up some COM registration issues as well as a by-product.
        If RequiredBitness = Bitness.Bits64 Then 'We have a 64bit application so check to see whether this is a 32bit only driver
            RK = Registry.ClassesRoot.OpenSubKey(ProgID & "\CLSID", False) 'Look in the 64bit section first
            If Not RK Is Nothing Then ' ProgID is registered and has a CLSID!
                CLSID = RK.GetValue("").ToString 'Get the CLSID for this ProgID
                RK.Close()

                RK = Registry.ClassesRoot.OpenSubKey("CLSID\" & CLSID) ' Check the 64bit registry section for this CLSID
                If RK Is Nothing Then 'We don't have an entry in the 64bit CLSID registry section so try the 32bit section
                    TL.LogMessage("DriverCompatibility", "     No entry in the 64bit registry, checking the 32bit registry")
                    RK = Registry.ClassesRoot.OpenSubKey("Wow6432Node\CLSID\" & CLSID) 'Check the 32bit registry section
                    Registered64Bit = False
                Else
                    TL.LogMessage("DriverCompatibility", "     Found entry in the 64bit registry")
                    Registered64Bit = True
                End If
                If Not RK Is Nothing Then 'We have a CLSID entry so process it
                    RKInprocServer32 = RK.OpenSubKey("InprocServer32")
                    RK.Close()
                    If Not RKInprocServer32 Is Nothing Then ' This is an in process server so test for compatibility
                        InprocFilePath = RKInprocServer32.GetValue("", "").ToString ' Get the file location from the default position
                        CodeBase = RKInprocServer32.GetValue("CodeBase", "").ToString 'Get the codebase if present to override the default value
                        If CodeBase <> "" Then InprocFilePath = CodeBase

                        If (Trim(InprocFilePath).ToUpperInvariant = "MSCOREE.DLL") Then ' We have an assembly, most likely in the GAC so get the actual file location of the assembly
                            'If this assembly is in the GAC, we should have an "Assembly" registry entry with the full assmbly name, 
                            TL.LogMessage("DriverCompatibility", "     Found MSCOREE.DLL")

                            AssemblyFullName = RKInprocServer32.GetValue("Assembly", "").ToString 'Get the full name
                            TL.LogMessage("DriverCompatibility", "     Found full name: " & AssemblyFullName)
                            If AssemblyFullName <> "" Then 'We did get an assembly full name so now try and load it to the reflection only context
                                Try
                                    LoadedAssembly = Assembly.ReflectionOnlyLoad(AssemblyFullName)
                                    'OK that wen't well so we have an MSIL version!
                                    InprocFilePath = LoadedAssembly.CodeBase 'Get the codebase for testing below
                                    TL.LogMessage("DriverCompatibilityMSIL", "     Found file path: " & InprocFilePath)
                                    TL.LogMessage("DriverCompatibilityMSIL", "     Found full name: " & LoadedAssembly.FullName & " ")
                                    Modules = LoadedAssembly.GetLoadedModules()
                                    Modules(0).GetPEKind(peKind, machine)
                                    If (peKind And PortableExecutableKinds.Required32Bit) <> 0 Then TL.LogMessage("DriverCompatibilityMSIL", "     Kind Required32bit")
                                    If (peKind And PortableExecutableKinds.PE32Plus) <> 0 Then TL.LogMessage("DriverCompatibilityMSIL", "     Kind PE32Plus")
                                    If (peKind And PortableExecutableKinds.ILOnly) <> 0 Then TL.LogMessage("DriverCompatibilityMSIL", "     Kind ILOnly")
                                    If (peKind And PortableExecutableKinds.NotAPortableExecutableImage) <> 0 Then TL.LogMessage("DriverCompatibilityMSIL", "     Kind Not PE Executable")

                                Catch ex As IOException
                                    'That failed so try to load an x86 version
                                    TL.LogMessageCrLf("DriverCompatibility", "Could not find file, trying x86 version - " & ex.Message)

                                    Try
                                        LoadedAssembly = Assembly.ReflectionOnlyLoad(AssemblyFullName & ", processorArchitecture=x86")
                                        'OK that wen't well so we have an x86 only version!
                                        InprocFilePath = LoadedAssembly.CodeBase 'Get the codebase for testing below
                                        TL.LogMessage("DriverCompatibilityX86", "     Found file path: " & InprocFilePath)
                                        Modules = LoadedAssembly.GetLoadedModules()
                                        Modules(0).GetPEKind(peKind, machine)
                                        If (peKind And PortableExecutableKinds.Required32Bit) <> 0 Then TL.LogMessage("DriverCompatibilityX86", "     Kind Required32bit")
                                        If (peKind And PortableExecutableKinds.PE32Plus) <> 0 Then TL.LogMessage("DriverCompatibilityX86", "     Kind PE32Plus")
                                        If (peKind And PortableExecutableKinds.ILOnly) <> 0 Then TL.LogMessage("DriverCompatibilityX86", "     Kind ILOnly")
                                        If (peKind And PortableExecutableKinds.NotAPortableExecutableImage) <> 0 Then TL.LogMessage("DriverCompatibilityX86", "     Kind Not PE Executable")

                                    Catch ex1 As IOException
                                        'That failed so try to load an x64 version
                                        TL.LogMessageCrLf("DriverCompatibilityX64", "Could not find file, trying x64 version - " & ex.Message)

                                        Try
                                            LoadedAssembly = Assembly.ReflectionOnlyLoad(AssemblyFullName & ", processorArchitecture=x64")
                                            'OK that wen't well so we have an x64 only version!
                                            InprocFilePath = LoadedAssembly.CodeBase 'Get the codebase for testing below
                                            TL.LogMessage("DriverCompatibilityX64", "     Found file path: " & InprocFilePath)
                                            Modules = LoadedAssembly.GetLoadedModules()
                                            Modules(0).GetPEKind(peKind, machine)
                                            If (peKind And PortableExecutableKinds.Required32Bit) <> 0 Then TL.LogMessage("DriverCompatibilityX64", "     Kind Required32bit")
                                            If (peKind And PortableExecutableKinds.PE32Plus) <> 0 Then TL.LogMessage("DriverCompatibilityX64", "     Kind PE32Plus")
                                            If (peKind And PortableExecutableKinds.ILOnly) <> 0 Then TL.LogMessage("DriverCompatibilityX64", "     Kind ILOnly")
                                            If (peKind And PortableExecutableKinds.NotAPortableExecutableImage) <> 0 Then TL.LogMessage("DriverCompatibilityX64", "     Kind Not PE Executable")

                                        Catch ex2 As Exception
                                            'Ignore exceptions here and leave MSCOREE.DLL as the InprocFilePath, this will fail below and generate an "incompatible driver" message
                                            TL.LogMessageCrLf("DriverCompatibilityX64", ex1.ToString)
                                        End Try

                                    Catch ex1 As Exception
                                        'Ignore exceptions here and leave MSCOREE.DLL as the InprocFilePath, this will fail below and generate an "incompatible driver" message
                                        TL.LogMessageCrLf("DriverCompatibilityX32", ex1.ToString)
                                    End Try

                                Catch ex As Exception
                                    'Ignore exceptions here and leave MSCOREE.DLL as the InprocFilePath, this will fail below and generate an "incompatible driver" message
                                    TL.LogMessageCrLf("DriverCompatibility", ex.ToString)
                                End Try
                            Else
                                'No Assembly entry so we can't load the assembly, we'll just have to take a chance!
                                TL.LogMessage("DriverCompatibility", "'AssemblyFullName is null so we can't load the assembly, we'll just have to take a chance!")
                                InprocFilePath = "" 'Set to null to bypass tests
                                TL.LogMessage("DriverCompatibility", "     Set InprocFilePath to null string")
                            End If
                        End If

                        If (Right(Trim(InprocFilePath), 4).ToUpperInvariant = ".DLL") Then ' We have a path to the server and it is a dll
                            ' We have an assembly or other technology DLL, outside the GAC, in the file system
                            Try
                                InProcServer = New PEReader(InprocFilePath, TL) 'Get hold of the executable so we can determine its characteristics
                                InprocServerBitness = InProcServer.BitNess
                                If InprocServerBitness = Bitness.Bits32 Then '32bit driver executable
                                    If Registered64Bit Then '32bit driver executable registered in 64bit COM
                                        DriverCompatibilityMessage = "This 32bit only driver won't work in a 64bit application even though it is registered as a 64bit COM driver." & vbCrLf & DRIVER_AUTHOR_MESSAGE_DRIVER
                                    Else '32bit driver executable registered in 32bit COM
                                        DriverCompatibilityMessage = "This 32bit only driver won't work in a 64bit application even though it is correctly registered as a 32bit COM driver." & vbCrLf & DRIVER_AUTHOR_MESSAGE_DRIVER
                                    End If
                                Else '64bit driver
                                    If Registered64Bit Then '64bit driver executable registered in 64bit COM section
                                        'This is the only OK combination, no message for this!
                                    Else '64bit driver executable registered in 32bit COM
                                        DriverCompatibilityMessage = "This 64bit capable driver is only registered as a 32bit COM driver." & vbCrLf & DRIVER_AUTHOR_MESSAGE_INSTALLER
                                    End If
                                End If
                            Catch ex As FileNotFoundException 'Cannot open the file
                                DriverCompatibilityMessage = "Cannot find the driver executable: " & vbCrLf & """" & InprocFilePath & """"
                            Catch ex As Exception 'Some other exception so log it
                                LogEvent("DriverCompatibilityMessage", "Exception parsing " & ProgID & ", """ & InprocFilePath & """", EventLogEntryType.Error, EventLogErrors.DriverCompatibilityException, ex.ToString)
                                DriverCompatibilityMessage = "PEReader Exception, please check ASCOM application Event Log for details"
                            End Try

                            If Not InProcServer Is Nothing Then 'Clean up the PEReader class
                                InProcServer.Dispose()
                                InProcServer = Nothing
                            End If
                        Else
                            'No codebase so can't test this driver, don't give an error message, just have to take a chance!
                            TL.LogMessage("DriverCompatibility", "No codebase so can't test this driver, don't give an error message, just have to take a chance!")
                        End If
                        RKInprocServer32.Close() 'Clean up the InProcServer registry key
                    Else 'This is not an inprocess DLL so no need to test further and no error message to return
                        'Please leave this empty clause here so the logic is clear!
                    End If
                Else 'Cannot find a CLSID entry
                    DriverCompatibilityMessage = "Unable to find a CLSID entry for this driver, please re-install."
                End If
            Else 'No COM ProgID registry entry
                DriverCompatibilityMessage = "This driver is not registered for COM (can't find ProgID), please re-install."
            End If
        Else 'We are running a 32bit application test so make sure the executable is not 64bit only
            RK = Registry.ClassesRoot.OpenSubKey(ProgID & "\CLSID", False) 'Look in the 32bit registry

            If Not RK Is Nothing Then ' ProgID is registered and has a CLSID!
                TL.LogMessage("DriverCompatibility", "     Found 32bit ProgID registration")
                CLSID = RK.GetValue("").ToString 'Get the CLSID for this ProgID
                RK.Close()
                RK = Nothing

                If OSBits() = Bitness.Bits64 Then ' We want to test as if we are a 32bit app on a 64bit OS
                    Try
                        RK32 = ProfileStore.OpenSubKey(Registry.ClassesRoot, "CLSID\" & CLSID, False, RegistryAccess.RegWow64Options.KEY_WOW64_32KEY)
                    Catch ex As Exception 'Ignore any exceptions, they just mean the operation wasn't successful
                    End Try

                    Try
                        RK64 = ProfileStore.OpenSubKey(Registry.ClassesRoot, "CLSID\" & CLSID, False, RegistryAccess.RegWow64Options.KEY_WOW64_64KEY)
                    Catch ex As Exception 'Ignore any exceptions, they just mean the operation wasn't successful
                    End Try

                Else ' We are running on a 32bit OS
                    RK = Registry.ClassesRoot.OpenSubKey("CLSID\" & CLSID) ' Check the 32bit registry section for this CLSID
                    TL.LogMessage("DriverCompatibility", "     Running on a 32bit OS, 32Bit Registered: " & (Not RK Is Nothing))
                End If

                If OSBits() = Bitness.Bits64 Then
                    TL.LogMessage("DriverCompatibility", "     Running on a 64bit OS, 32bit Registered: " & (Not RK32 Is Nothing) & ", 64Bit Registered: " & (Not RK64 Is Nothing))
                    If Not RK32 Is Nothing Then 'We are testing as a 32bit app so if there is a 32bit key return this
                        RK = RK32
                    Else 'Otherwise return the 64bit key
                        RK = RK64
                    End If
                End If

                If Not RK Is Nothing Then 'We have a CLSID entry so process it
                    TL.LogMessage("DriverCompatibility", "     Found CLSID entry")
                    RKInprocServer32 = RK.OpenSubKey("InprocServer32")
                    RK.Close()
                    If Not RKInprocServer32 Is Nothing Then ' This is an in process server so test for compatibility
                        InprocFilePath = RKInprocServer32.GetValue("", "").ToString ' Get the file location from the default position
                        CodeBase = RKInprocServer32.GetValue("CodeBase", "").ToString 'Get the codebase if present to override the default value
                        If CodeBase <> "" Then InprocFilePath = CodeBase

                        If (Trim(InprocFilePath).ToUpperInvariant = "MSCOREE.DLL") Then ' We have an assembly, most likely in the GAC so get the actual file location of the assembly
                            'If this assembly is in the GAC, we should have an "Assembly" registry entry with the full assmbly name, 
                            TL.LogMessage("DriverCompatibility", "     Found MSCOREE.DLL")

                            AssemblyFullName = RKInprocServer32.GetValue("Assembly", "").ToString 'Get the full name
                            TL.LogMessage("DriverCompatibility", "     Found full name: " & AssemblyFullName)
                            If AssemblyFullName <> "" Then 'We did get an assembly full name so now try and load it to the reflection only context
                                Try
                                    LoadedAssembly = Assembly.ReflectionOnlyLoad(AssemblyFullName)
                                    'OK that wen't well so we have an MSIL version!
                                    InprocFilePath = LoadedAssembly.CodeBase 'Get the codebase for testing below
                                    TL.LogMessage("DriverCompatibilityMSIL", "     Found file path: " & InprocFilePath)
                                    TL.LogMessage("DriverCompatibilityMSIL", "     Found full name: " & LoadedAssembly.FullName & " ")
                                    Modules = LoadedAssembly.GetLoadedModules()
                                    Modules(0).GetPEKind(peKind, machine)
                                    If (peKind And PortableExecutableKinds.Required32Bit) <> 0 Then TL.LogMessage("DriverCompatibilityMSIL", "     Kind Required32bit")
                                    If (peKind And PortableExecutableKinds.PE32Plus) <> 0 Then TL.LogMessage("DriverCompatibilityMSIL", "     Kind PE32Plus")
                                    If (peKind And PortableExecutableKinds.ILOnly) <> 0 Then TL.LogMessage("DriverCompatibilityMSIL", "     Kind ILOnly")
                                    If (peKind And PortableExecutableKinds.NotAPortableExecutableImage) <> 0 Then TL.LogMessage("DriverCompatibilityMSIL", "     Kind Not PE Executable")

                                Catch ex As IOException
                                    'That failed so try to load an x86 version
                                    TL.LogMessageCrLf("DriverCompatibility", "Could not find file, trying x86 version - " & ex.Message)

                                    Try
                                        LoadedAssembly = Assembly.ReflectionOnlyLoad(AssemblyFullName & ", processorArchitecture=x86")
                                        'OK that wen't well so we have an x86 only version!
                                        InprocFilePath = LoadedAssembly.CodeBase 'Get the codebase for testing below
                                        TL.LogMessage("DriverCompatibilityX86", "     Found file path: " & InprocFilePath)
                                        Modules = LoadedAssembly.GetLoadedModules()
                                        Modules(0).GetPEKind(peKind, machine)
                                        If (peKind And PortableExecutableKinds.Required32Bit) <> 0 Then TL.LogMessage("DriverCompatibilityX86", "     Kind Required32bit")
                                        If (peKind And PortableExecutableKinds.PE32Plus) <> 0 Then TL.LogMessage("DriverCompatibilityX86", "     Kind PE32Plus")
                                        If (peKind And PortableExecutableKinds.ILOnly) <> 0 Then TL.LogMessage("DriverCompatibilityX86", "     Kind ILOnly")
                                        If (peKind And PortableExecutableKinds.NotAPortableExecutableImage) <> 0 Then TL.LogMessage("DriverCompatibilityX86", "     Kind Not PE Executable")

                                    Catch ex1 As IOException
                                        'That failed so try to load an x64 version
                                        TL.LogMessageCrLf("DriverCompatibilityX64", "Could not find file, trying x64 version - " & ex.Message)

                                        Try
                                            LoadedAssembly = Assembly.ReflectionOnlyLoad(AssemblyFullName & ", processorArchitecture=x64")
                                            'OK that wen't well so we have an x64 only version!
                                            InprocFilePath = LoadedAssembly.CodeBase 'Get the codebase for testing below
                                            TL.LogMessage("DriverCompatibilityX64", "     Found file path: " & InprocFilePath)
                                            Modules = LoadedAssembly.GetLoadedModules()
                                            Modules(0).GetPEKind(peKind, machine)
                                            If (peKind And PortableExecutableKinds.Required32Bit) <> 0 Then TL.LogMessage("DriverCompatibilityX64", "     Kind Required32bit")
                                            If (peKind And PortableExecutableKinds.PE32Plus) <> 0 Then TL.LogMessage("DriverCompatibilityX64", "     Kind PE32Plus")
                                            If (peKind And PortableExecutableKinds.ILOnly) <> 0 Then TL.LogMessage("DriverCompatibilityX64", "     Kind ILOnly")
                                            If (peKind And PortableExecutableKinds.NotAPortableExecutableImage) <> 0 Then TL.LogMessage("DriverCompatibilityX64", "     Kind Not PE Executable")

                                        Catch ex2 As Exception
                                            'Ignore exceptions here and leave MSCOREE.DLL as the InprocFilePath, this will fail below and generate an "incompatible driver" message
                                            TL.LogMessageCrLf("DriverCompatibilityX64", ex1.ToString)
                                        End Try

                                    Catch ex1 As Exception
                                        'Ignore exceptions here and leave MSCOREE.DLL as the InprocFilePath, this will fail below and generate an "incompatible driver" message
                                        TL.LogMessageCrLf("DriverCompatibilityX32", ex1.ToString)
                                    End Try

                                Catch ex As Exception
                                    'Ignore exceptions here and leave MSCOREE.DLL as the InprocFilePath, this will fail below and generate an "incompatible driver" message
                                    TL.LogMessageCrLf("DriverCompatibility", ex.ToString)
                                End Try
                            Else
                                'No Assembly entry so we can't load the assembly, we'll just have to take a chance!
                                TL.LogMessage("DriverCompatibility", "'AssemblyFullName is null so we can't load the assembly, we'll just have to take a chance!")
                                InprocFilePath = "" 'Set to null to bypass tests
                                TL.LogMessage("DriverCompatibility", "     Set InprocFilePath to null string")
                            End If
                        End If

                        If (Right(Trim(InprocFilePath), 4).ToUpperInvariant = ".DLL") Then ' We do have a path to the server and it is a dll
                            ' We have an assembly or other technology DLL, outside the GAC, in the file system
                            Try
                                InProcServer = New PEReader(InprocFilePath, TL) 'Get hold of the executable so we can determine its characteristics
                                If InProcServer.BitNess = Bitness.Bits64 Then '64bit only driver executable
                                    DriverCompatibilityMessage = "This is a 64bit only driver and is not compatible with this 32bit application." & vbCrLf & DRIVER_AUTHOR_MESSAGE_DRIVER
                                End If
                            Catch ex As FileNotFoundException 'Cannot open the file
                                DriverCompatibilityMessage = "Cannot find the driver executable: " & vbCrLf & """" & InprocFilePath & """"
                            Catch ex As Exception 'Some other exception so log it
                                LogEvent("DriverCompatibilityMessage", "Exception parsing " & ProgID & ", """ & InprocFilePath & """", EventLogEntryType.Error, EventLogErrors.DriverCompatibilityException, ex.ToString)
                                DriverCompatibilityMessage = "PEReader Exception, please check ASCOM application Event Log for details"
                            End Try

                            If Not InProcServer Is Nothing Then 'Clean up the PEReader class
                                InProcServer.Dispose()
                                InProcServer = Nothing
                            End If
                        Else
                            'No codebase or not a DLL so can't test this driver, don't give an error message, just have to take a chance!
                            TL.LogMessage("DriverCompatibility", "No codebase or not a DLL so can't test this driver, don't give an error message, just have to take a chance!")
                        End If
                        RKInprocServer32.Close() 'Clean up the InProcServer registry key
                    Else 'This is not an inprocess DLL so no need to test further and no error message to return
                        'Please leave this empty clause here so the logic is clear!
                        TL.LogMessage("DriverCompatibility", "This is not an inprocess DLL so no need to test further and no error message to return")
                    End If
                Else 'Cannot find a CLSID entry
                    DriverCompatibilityMessage = "Unable to find a CLSID entry for this driver, please re-install."
                    TL.LogMessage("DriverCompatibility", "     Could not find CLSID entry!")
                End If
            Else 'No COM ProgID registry entry
                DriverCompatibilityMessage = "This driver is not registered for COM (can't find ProgID), please re-install."
            End If

        End If
        TL.LogMessage("DriverCompatibility", "     Returning: """ & DriverCompatibilityMessage & """")
        Return DriverCompatibilityMessage
    End Function

End Module
#End Region

#Region "Old Code"
'Try 'Get the list of 32bit only drivers
'Drivers32Bit = ProfileStore.EnumProfile(DRIVERS_32BIT)
'Catch ex1 As Exception
'Ignore any exceptions from this call e.g. if there are no 32bit only devices installed
'Just create an empty list
'Drivers32Bit = New Generic.SortedList(Of String, String)
'LogEvent("ChooserForm", "Exception creating SortedList of 32bit only applications", EventLogEntryType.Error, EventLogErrors.Chooser32BitOnlyException, ex1.ToString)
'End Try

'Try 'Get the list of 64bit only drivers
'Drivers64Bit = ProfileStore.EnumProfile(DRIVERS_64BIT)
'Catch ex1 As Exception
'Ignore any exceptions from this call e.g. if there are no 64bit only devices installed
'Just create an empty list
'Drivers64Bit = New Generic.SortedList(Of String, String)
'LogEvent("ChooserForm", "Exception creating SortedList of 64bit only applications", EventLogEntryType.Error, EventLogErrors.Chooser64BitOnlyException, ex1.ToString)
'End Try

'If (ApplicationBits() = Bitness.Bits64) And (Drivers32Bit.ContainsKey(ProgID)) Then 'This is a 32bit driver being accessed by a 64bit application
' DriverCompatibilityMessage = "This 32bit driver is not compatible with your 64bit application." & vbCrLf & _
'               "Please contact the driver author to see if there is a 64bit compatible version."
' End If
'If (ApplicationBits() = Bitness.Bits32) And (Drivers64Bit.ContainsKey(ProgID)) Then 'This is a 64bit driver being accessed by a 32bit application
' DriverCompatibilityMessage = "This 64bit driver is not compatible with your 32bit application." & vbCrLf & _
'               "Please contact the driver author to see if there is a 32bit compatible version."
' End If


#End Region

#Region "Force Platform version code"
Module AscomSharedCode
    Friend Function ConditionPlatformVersion(ByVal PlatformVersion As String, ByVal Profile As RegistryAccess, ByVal TL As TraceLogger) As String
        Dim ModuleFileName, ForcedFileNameKey As String, ForcedFileNames, ForcedSeparators As SortedList(Of String, String)
        Dim PC As PerformanceCounter

        ConditionPlatformVersion = PlatformVersion ' Set default action to return the supplied vaue
        Try
            ModuleFileName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName) 'Get the name of the executable without path or file extension
            If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "  ModuleFileName: """ & ModuleFileName & """ """ &
                                                    Process.GetCurrentProcess().MainModule.FileName & """")
            If Left(ModuleFileName.ToUpperInvariant, 3) = "IS-" Then ' Likely to be an old Inno installer so try and get the parent's name
                If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "    Inno installer temporary executable detected, searching for parent process!")
                If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "    Old Module Filename: " & ModuleFileName)
                PC = New PerformanceCounter("Process", "Creating Process ID", Process.GetCurrentProcess().ProcessName)
                ModuleFileName = Path.GetFileNameWithoutExtension(Process.GetProcessById(CInt(PC.NextValue())).MainModule.FileName)
                If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "    New Module Filename: " & ModuleFileName)
                PC.Close()
                PC.Dispose()
            End If

            'Force any particular platform version number this application requires
            ForcedFileNames = Profile.EnumProfile(PLATFORM_VERSION_EXCEPTIONS) 'Get the list of filenames requiring specific versions

            For Each ForcedFileName As KeyValuePair(Of String, String) In ForcedFileNames ' Check each forced file in turn 
                If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "  ForcedFileName: """ & ForcedFileName.Key & """ """ &
                                                        ForcedFileName.Value & """ """ &
                                                        UCase(Path.GetFileNameWithoutExtension(ForcedFileName.Key)) & """ """ &
                                                        UCase(Path.GetFileName(ForcedFileName.Key)) & """ """ &
                                                        UCase(ForcedFileName.Key) & """ """ &
                                                        ForcedFileName.Key & """ """ &
                                                        UCase(ModuleFileName) & """")
                If ForcedFileName.Key.Contains(".") Then
                    ForcedFileNameKey = Path.GetFileNameWithoutExtension(ForcedFileName.Key)
                Else
                    ForcedFileNameKey = ForcedFileName.Key
                End If

                ' If the current file matches a forced file name then return the required Platform version
                ' 6.0 SP1 Check now uses StartsWith in order to catch situations where people rename the installer after download
                If ForcedFileNameKey <> "" Then ' Ignore the empty string "Default" value name
                    ' Test made completely local independent to fix an issue in the Turkish locale where capital I is returned as a dotted i. Line below was: If UCase(ModuleFileName).StartsWith(UCase(ForcedFileNameKey)) Then
                    If ModuleFileName.StartsWith(ForcedFileNameKey, StringComparison.OrdinalIgnoreCase) Then ' Should now be completely locale independent, including the Turkish locale.
                        ConditionPlatformVersion = ForcedFileName.Value
                        If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "  Matched file: """ & ModuleFileName & """ """ & ForcedFileNameKey & """")
                    End If
                End If
            Next

            ForcedSeparators = Profile.EnumProfile(PLATFORM_VERSION_SEPARATOR_EXCEPTIONS) 'Get the list of filenames requiring specific versions

            For Each ForcedSeparator As KeyValuePair(Of String, String) In ForcedSeparators ' Check each forced file in turn 
                If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "  ForcedFileName: """ & ForcedSeparator.Key & """ """ &
                                                        ForcedSeparator.Value & """ """ &
                                                        UCase(Path.GetFileNameWithoutExtension(ForcedSeparator.Key)) & """ """ &
                                                        UCase(Path.GetFileName(ForcedSeparator.Key)) & """ """ &
                                                        UCase(ForcedSeparator.Key) & """ """ &
                                                        ForcedSeparator.Key & """ """ &
                                                        UCase(ModuleFileName) & """")
                If ForcedSeparator.Key.Contains(".") Then
                Else
                End If

                If UCase(Path.GetFileNameWithoutExtension(ForcedSeparator.Key)) = UCase(ModuleFileName) Then ' If the current file matches a forced file name then return the required Platform version
                    If String.IsNullOrEmpty(ForcedSeparator.Value) Then ' Replace with the current locale decimal separator
                        ConditionPlatformVersion = ConditionPlatformVersion.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator())
                        If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "  String IsNullOrEmpty: """ & ConditionPlatformVersion & """")
                    Else 'Replace with the fixed value provided
                        ConditionPlatformVersion = ConditionPlatformVersion.Replace(".", ForcedSeparator.Value)
                        If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "  String Is: """ & ForcedSeparator.Value & """ """ & ConditionPlatformVersion & """")
                    End If

                    If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "  Matched file: """ & ModuleFileName & """ """ & ForcedSeparator.Key & """")
                End If
            Next

        Catch ex As Exception
            If Not TL Is Nothing Then TL.LogMessageCrLf("ConditionPlatformVersion", "Exception: " & ex.ToString)
            LogEvent("ConditionPlatformVersion", "Exception: ", EventLogEntryType.Error, EventLogErrors.VB6HelperProfileException, ex.ToString)
        End Try
        If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "  Returning: """ & ConditionPlatformVersion & """")
    End Function
End Module
#End Region

''' <summary>
''' 
''' </summary>
Friend Class PEReader
    Implements IDisposable

#Region "Constants"
    Friend Const CLR_HEADER As Integer = 14 ' Header number of the CLR information, if present
    Private Const MAX_HEADERS_TO_CHECK As Integer = 1000 ' Safety limit to ensure that we don't lock up the machine if we get a PE image that indicates it has a huge number of header directories

    ' Possible error codes when an assembly is loaded for reflection
    Private Const COR_E_BADIMAGEFORMAT As Integer = &H8007000B
    Private Const CLDB_E_FILE_OLDVER As Integer = &H80131107
    Private Const CLDB_E_INDEX_NOTFOUND As Integer = &H80131124
    Private Const CLDB_E_FILE_CORRUPT As Integer = &H8013110E
    Private Const COR_E_NEWER_RUNTIME As Integer = &H8013101B
    Private Const COR_E_ASSEMBLYEXPECTED As Integer = &H80131018
    Private Const ERROR_BAD_EXE_FORMAT As Integer = &H800700C1
    Private Const ERROR_EXE_MARKED_INVALID As Integer = &H800700C0
    Private Const CORSEC_E_INVALID_IMAGE_FORMAT As Integer = &H8013141D
    Private Const ERROR_NOACCESS As Integer = &H800703E6
    Private Const ERROR_INVALID_ORDINAL As Integer = &H800700B6
    Private Const ERROR_INVALID_DLL As Integer = &H80070482
    Private Const ERROR_FILE_CORRUPT As Integer = &H80070570
    Private Const COR_E_LOADING_REFERENCE_ASSEMBLY As Integer = &H80131058
    Private Const META_E_BAD_SIGNATURE As Integer = &H80131192

    ' Executable machine types
    Private Const IMAGE_FILE_MACHINE_I386 As UShort = &H14C ' x86
    Private Const IMAGE_FILE_MACHINE_IA64 As UShort = &H200 ' Intel(Itanium)
    Private Const IMAGE_FILE_MACHINE_AMD64 As UShort = &H8664 ' x64

#End Region

#Region "Enums"
    Friend Enum CLR_FLAGS
        CLR_FLAGS_ILONLY = &H1
        CLR_FLAGS_32BITREQUIRED = &H2
        CLR_FLAGS_IL_LIBRARY = &H4
        CLR_FLAGS_STRONGNAMESIGNED = &H8
        CLR_FLAGS_NATIVE_ENTRYPOINT = &H10
        CLR_FLAGS_TRACKDEBUGDATA = &H10000
    End Enum

    Friend Enum SubSystemType
        NATIVE = 1 'The binary doesn't need a subsystem. This is used for drivers.
        WINDOWS_GUI = 2 'The image is a Win32 graphical binary. (It can still open a console with AllocConsole() but won't get one automatically at startup.)
        WINDOWS_CUI = 3 'The binary is a Win32 console binary. (It will get a console per default at startup, or inherit the parent's console.)
        UNKNOWN_4 = 4 'Unknown allocation
        OS2_CUI = 5 'The binary is a OS/2 console binary. (OS/2 binaries will be in OS/2 format, so this value will seldom be used in a PE file.)
        UNKNOWN_6 = 6 'Unknown allocation
        POSIX_CUI = 7 'The binary uses the POSIX console subsystem.
        NATIVE_WINDOWS = 8
        WINDOWS_CE_GUI = 9
        EFI_APPLICATION = 10 'Extensible Firmware Interface (EFI) application.
        EFI_BOOT_SERVICE_DRIVER = 11 'EFI driver with boot services.
        EFI_RUNTIME_DRIVER = 12 'EFI driver with run-time services.
        EFI_ROM = 13 'EFI ROM image.
        XBOX = 14 'Xbox sy stem.
        UNKNOWN_15 = 15 'Unknown allocation
        WINDOWS_BOOT_APPLICATION = 16 'Boot application.
    End Enum
#End Region

#Region "Structs"

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure IMAGE_DOS_HEADER
        Friend e_magic As UInt16 'Magic number
        Friend e_cblp As UInt16 'Bytes on last page of file
        Friend e_cp As UInt16 'Pages in file
        Friend e_crlc As UInt16 'Relocations
        Friend e_cparhdr As UInt16 'Size of header in paragraphs
        Friend e_minalloc As UInt16 'Minimum extra paragraphs needed
        Friend e_maxalloc As UInt16 'Maximum extra paragraphs needed
        Friend e_ss As UInt16 'Initial (relative) SS value
        Friend e_sp As UInt16 'Initial SP value
        Friend e_csum As UInt16 'Checksum
        Friend e_ip As UInt16 'Initial IP value
        Friend e_cs As UInt16 'Initial (relative) CS value
        Friend e_lfarlc As UInt16 'File address of relocation table
        Friend e_ovno As UInt16 'Overlay number
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)>
        Friend e_res1 As UInt16() 'Reserved words
        Friend e_oemid As UInt16 'OEM identifier (for e_oeminfo)
        Friend e_oeminfo As UInt16 '
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=10)>
        Friend e_res2 As UInt16() 'Reserved words
        Friend e_lfanew As UInt32 'File address of new exe header
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure IMAGE_NT_HEADERS
        Friend Signature As UInt32
        Friend FileHeader As IMAGE_FILE_HEADER
        Friend OptionalHeader32 As IMAGE_OPTIONAL_HEADER32
        Friend OptionalHeader64 As IMAGE_OPTIONAL_HEADER64
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure IMAGE_FILE_HEADER
        Friend Machine As UInt16
        Friend NumberOfSections As UInt16
        Friend TimeDateStamp As UInt32
        Friend PointerToSymbolTable As UInt32
        Friend NumberOfSymbols As UInt32
        Friend SizeOfOptionalHeader As UInt16
        Friend Characteristics As UInt16
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure IMAGE_OPTIONAL_HEADER32
        Friend Magic As UInt16
        Friend MajorLinkerVersion As [Byte]
        Friend MinorLinkerVersion As [Byte]
        Friend SizeOfCode As UInt32
        Friend SizeOfInitializedData As UInt32
        Friend SizeOfUninitializedData As UInt32
        Friend AddressOfEntryPoint As UInt32
        Friend BaseOfCode As UInt32
        Friend BaseOfData As UInt32
        Friend ImageBase As UInt32
        Friend SectionAlignment As UInt32
        Friend FileAlignment As UInt32
        Friend MajorOperatingSystemVersion As UInt16
        Friend MinorOperatingSystemVersion As UInt16
        Friend MajorImageVersion As UInt16
        Friend MinorImageVersion As UInt16
        Friend MajorSubsystemVersion As UInt16
        Friend MinorSubsystemVersion As UInt16
        Friend Win32VersionValue As UInt32
        Friend SizeOfImage As UInt32
        Friend SizeOfHeaders As UInt32
        Friend CheckSum As UInt32
        Friend Subsystem As UInt16
        Friend DllCharacteristics As UInt16
        Friend SizeOfStackReserve As UInt32
        Friend SizeOfStackCommit As UInt32
        Friend SizeOfHeapReserve As UInt32
        Friend SizeOfHeapCommit As UInt32
        Friend LoaderFlags As UInt32
        Friend NumberOfRvaAndSizes As UInt32
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)>
        Friend DataDirectory As IMAGE_DATA_DIRECTORY()
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure IMAGE_OPTIONAL_HEADER64
        Friend Magic As UInt16
        Friend MajorLinkerVersion As [Byte]
        Friend MinorLinkerVersion As [Byte]
        Friend SizeOfCode As UInt32
        Friend SizeOfInitializedData As UInt32
        Friend SizeOfUninitializedData As UInt32
        Friend AddressOfEntryPoint As UInt32
        Friend BaseOfCode As UInt32
        Friend ImageBase As UInt64
        Friend SectionAlignment As UInt32
        Friend FileAlignment As UInt32
        Friend MajorOperatingSystemVersion As UInt16
        Friend MinorOperatingSystemVersion As UInt16
        Friend MajorImageVersion As UInt16
        Friend MinorImageVersion As UInt16
        Friend MajorSubsystemVersion As UInt16
        Friend MinorSubsystemVersion As UInt16
        Friend Win32VersionValue As UInt32
        Friend SizeOfImage As UInt32
        Friend SizeOfHeaders As UInt32
        Friend CheckSum As UInt32
        Friend Subsystem As UInt16
        Friend DllCharacteristics As UInt16
        Friend SizeOfStackReserve As UInt64
        Friend SizeOfStackCommit As UInt64
        Friend SizeOfHeapReserve As UInt64
        Friend SizeOfHeapCommit As UInt64
        Friend LoaderFlags As UInt32
        Friend NumberOfRvaAndSizes As UInt32
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)>
        Friend DataDirectory As IMAGE_DATA_DIRECTORY()
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure IMAGE_DATA_DIRECTORY
        Friend VirtualAddress As UInt32
        Friend Size As UInt32
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure IMAGE_SECTION_HEADER
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=8)>
        Friend Name As String
        Friend Misc As Misc
        Friend VirtualAddress As UInt32
        Friend SizeOfRawData As UInt32
        Friend PointerToRawData As UInt32
        Friend PointerToRelocations As UInt32
        Friend PointerToLinenumbers As UInt32
        Friend NumberOfRelocations As UInt16
        Friend NumberOfLinenumbers As UInt16
        Friend Characteristics As UInt32
    End Structure

    <StructLayout(LayoutKind.Explicit)>
    Friend Structure Misc
        <FieldOffset(0)>
        Friend PhysicalAddress As UInt32
        <FieldOffset(0)>
        Friend VirtualSize As UInt32
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure IMAGE_COR20_HEADER
        Friend cb As UInt32
        Friend MajorRuntimeVersion As UInt16
        Friend MinorRuntimeVersion As UInt16
        Friend MetaData As IMAGE_DATA_DIRECTORY       '// Symbol table and startup information
        Friend Flags As UInt32
        Friend EntryPointToken As UInt32
        Friend Resources As IMAGE_DATA_DIRECTORY        '// Binding information
        Friend StrongNameSignature As IMAGE_DATA_DIRECTORY
        Friend CodeManagerTable As IMAGE_DATA_DIRECTORY        '// Regular fixup and binding information
        Friend VTableFixups As IMAGE_DATA_DIRECTORY
        Friend ExportAddressTableJumps As IMAGE_DATA_DIRECTORY
        Friend ManagedNativeHeader As IMAGE_DATA_DIRECTORY        '// Precompiled image info (internal use only - set to zero)
    End Structure
#End Region

#Region "Fields"
    Private ReadOnly dosHeader As IMAGE_DOS_HEADER
    Private ntHeaders As IMAGE_NT_HEADERS
    Private ReadOnly CLR As IMAGE_COR20_HEADER
    Private ReadOnly sectionHeaders As Generic.IList(Of IMAGE_SECTION_HEADER) = New Generic.List(Of IMAGE_SECTION_HEADER)
    Private TextBase As UInteger
    Private reader As BinaryReader
    Private stream As Stream
    Private IsAssembly As Boolean = False
    Private AssemblyInfo As AssemblyName
    Private SuppliedAssembly As Assembly
    Private AssemblyDeterminationType As String
    Private OS32BitCompatible As Boolean = False
    Private ExecutableBitness As Bitness

    Private TL As TraceLogger
#End Region

    Friend Sub New(ByVal FileName As String, TLogger As TraceLogger)
        TL = TLogger ' Save the TraceLogger instance we have been passed

        TL.LogMessage("PEReader", "Running within CLR version: " & RuntimeEnvironment.GetSystemVersion())

        If Left(FileName, 5).ToUpperInvariant = "FILE:" Then
            'Convert uri to local path if required, uri paths are not supported by FileStream - this method allows file names with # characters to be passed through
            Dim u As Uri = New Uri(FileName)
            FileName = u.LocalPath + Uri.UnescapeDataString(u.Fragment).Replace("/", "\\")
        End If
        TL.LogMessage("PEReader", "Filename to check: " & FileName)
        If Not File.Exists(FileName) Then Throw New FileNotFoundException("PEReader - File not found: " & FileName)

        ' Determine whether this is an assembly by testing whether we can load the file as an assembly, if so then it IS an assembly!
        TL.LogMessage("PEReader", "Determining whether this is an assembly")
        Try
            SuppliedAssembly = Assembly.ReflectionOnlyLoadFrom(FileName)
            IsAssembly = True ' We got here without an exception so it must be an assembly
            TL.LogMessage("PEReader.IsAssembly", "Found an assembly because it loaded Ok to the reflection context: " & IsAssembly)
        Catch ex As System.IO.FileNotFoundException
            TL.LogMessage("PEReader.IsAssembly", "FileNotFoundException: File not found so this is NOT an assembly: " & IsAssembly)
        Catch ex1 As System.BadImageFormatException

            ' There are multiple reasons why this can occur so now determine what actually happened by examining the hResult
            Dim hResult As Integer = Marshal.GetHRForException(ex1)

            Select Case hResult
                Case COR_E_BADIMAGEFORMAT
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - COR_E_BADIMAGEFORMAT. Setting IsAssembly to: " & IsAssembly)
                Case CLDB_E_FILE_OLDVER
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - CLDB_E_FILE_OLDVER. Setting IsAssembly to: " & IsAssembly)
                Case CLDB_E_INDEX_NOTFOUND
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - CLDB_E_INDEX_NOTFOUND. Setting IsAssembly to: " & IsAssembly)
                Case CLDB_E_FILE_CORRUPT
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - CLDB_E_FILE_CORRUPT. Setting IsAssembly to: " & IsAssembly)
                Case COR_E_NEWER_RUNTIME ' This is an assembly but it requires a newer runtime than is currently running, so flag it as an assembly even though we can't load it
                    IsAssembly = True
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - COR_E_NEWER_RUNTIME. Setting IsAssembly to: " & IsAssembly)
                Case COR_E_ASSEMBLYEXPECTED
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - COR_E_ASSEMBLYEXPECTED. Setting IsAssembly to: " & IsAssembly)
                Case ERROR_BAD_EXE_FORMAT
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - ERROR_BAD_EXE_FORMAT. Setting IsAssembly to: " & IsAssembly)
                Case ERROR_EXE_MARKED_INVALID
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - ERROR_EXE_MARKED_INVALID. Setting IsAssembly to: " & IsAssembly)
                Case CORSEC_E_INVALID_IMAGE_FORMAT
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - CORSEC_E_INVALID_IMAGE_FORMAT. Setting IsAssembly to: " & IsAssembly)
                Case ERROR_NOACCESS
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - ERROR_NOACCESS. Setting IsAssembly to: " & IsAssembly)
                Case ERROR_INVALID_ORDINAL
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - ERROR_INVALID_ORDINAL. Setting IsAssembly to: " & IsAssembly)
                Case ERROR_INVALID_DLL
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - ERROR_INVALID_DLL. Setting IsAssembly to: " & IsAssembly)
                Case ERROR_FILE_CORRUPT
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - ERROR_FILE_CORRUPT. Setting IsAssembly to: " & IsAssembly)
                Case COR_E_LOADING_REFERENCE_ASSEMBLY
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - COR_E_LOADING_REFERENCE_ASSEMBLY. Setting IsAssembly to: " & IsAssembly)
                Case META_E_BAD_SIGNATURE
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - META_E_BAD_SIGNATURE. Setting IsAssembly to: " & IsAssembly)
                Case Else
                    TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " & hResult.ToString("X8") & " - Meaning of error code is unknown. Setting IsAssembly to: " & IsAssembly)
            End Select

        Catch ex2 As System.IO.FileLoadException ' This is an assembly but that has already been loaded so flag it as an assembly
            IsAssembly = True
            TL.LogMessage("PEReader.IsAssembly", "FileLoadException: Assembly already loaded so this is an assembly: " & IsAssembly)
        End Try

        TL.LogMessage("PEReader", "Determining PE Machine type")
        stream = New FileStream(FileName, FileMode.Open, FileAccess.Read)
        reader = New BinaryReader(stream)

        reader.BaseStream.Seek(0, SeekOrigin.Begin) ' Reset reader position, just in case
        dosHeader = MarshalBytesTo(Of IMAGE_DOS_HEADER)(reader) ' Read MS-DOS header section
        If dosHeader.e_magic <> &H5A4D Then ' MS-DOS magic number should read 'MZ'
            Throw New InvalidOperationException("File is not a portable executable.")
        End If

        reader.BaseStream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin) ' Skip MS-DOS stub and seek reader to NT Headers
        ntHeaders.Signature = MarshalBytesTo(Of UInt32)(reader) ' Read NT Headers
        If ntHeaders.Signature <> &H4550 Then ' Make sure we have 'PE' in the pe signature 
            Throw New InvalidOperationException("Invalid portable executable signature in NT header.")
        End If
        ntHeaders.FileHeader = MarshalBytesTo(Of IMAGE_FILE_HEADER)(reader) ' Read the IMAGE_FILE_HEADER which starts 4 bytes on from the start of the signature (already here by reading the signature itself)

        ' Determine whether this executable is flagged as a 32bit or 64bit and set OS32BitCompatible accordingly
        Select Case ntHeaders.FileHeader.Machine
            Case IMAGE_FILE_MACHINE_I386
                OS32BitCompatible = True
                TL.LogMessage("PEReader.MachineType", "Machine - found ""Intel 32bit"" executable. Characteristics: " & ntHeaders.FileHeader.Characteristics.ToString("X8") & ", OS32BitCompatible: " & OS32BitCompatible)
            Case IMAGE_FILE_MACHINE_IA64
                OS32BitCompatible = False
                TL.LogMessage("PEReader.MachineType", "Machine - found ""Itanium 64bit"" executable. Characteristics: " & ntHeaders.FileHeader.Characteristics.ToString("X8") & ", OS32BitCompatible: " & OS32BitCompatible)
            Case IMAGE_FILE_MACHINE_AMD64
                OS32BitCompatible = False
                TL.LogMessage("PEReader.MachineType", "Machine - found ""Intel 64bit"" executable. Characteristics: " & ntHeaders.FileHeader.Characteristics.ToString("X8") & ", OS32BitCompatible: " & OS32BitCompatible)
            Case Else
                TL.LogMessage("PEReader.MachineType", "Found Unknown machine type: " & ntHeaders.FileHeader.Machine.ToString("X8") & ". Characteristics: " & ntHeaders.FileHeader.Characteristics.ToString("X8") & ", OS32BitCompatible: " & OS32BitCompatible)
        End Select

        If OS32BitCompatible Then ' Read optional 32bit header
            TL.LogMessage("PEReader.MachineType", "Reading optional 32bit header")
            ntHeaders.OptionalHeader32 = MarshalBytesTo(Of IMAGE_OPTIONAL_HEADER32)(reader)
        Else ' Read optional 64bit header
            TL.LogMessage("PEReader.MachineType", "Reading optional 64bit header")
            ntHeaders.OptionalHeader64 = MarshalBytesTo(Of IMAGE_OPTIONAL_HEADER64)(reader)
        End If

        If IsAssembly Then
            TL.LogMessage("PEReader", "This is an assembly, determining Bitness through the CLR header")
            ' Find the CLR header
            Dim NumberOfHeadersToCheck As Integer = MAX_HEADERS_TO_CHECK
            If OS32BitCompatible Then ' We have a 32bit assembly
                TL.LogMessage("PEReader.Bitness", "This is a 32 bit assembly, reading the CLR Header")
                If ntHeaders.OptionalHeader32.NumberOfRvaAndSizes < MAX_HEADERS_TO_CHECK Then NumberOfHeadersToCheck = CInt(ntHeaders.OptionalHeader32.NumberOfRvaAndSizes)
                TL.LogMessage("PEReader.Bitness", "Checking " & NumberOfHeadersToCheck & " headers")

                For i As Integer = 0 To NumberOfHeadersToCheck - 1
                    If ntHeaders.OptionalHeader32.DataDirectory(i).Size > 0 Then
                        sectionHeaders.Add(MarshalBytesTo(Of IMAGE_SECTION_HEADER)(reader))
                    End If
                Next

                For Each SectionHeader As IMAGE_SECTION_HEADER In sectionHeaders
                    If SectionHeader.Name = ".text" Then TextBase = SectionHeader.PointerToRawData
                Next

                If NumberOfHeadersToCheck >= CLR_HEADER + 1 Then ' Only test if the number of headers meets or exceeds the lcoation of the CLR header
                    If ntHeaders.OptionalHeader32.DataDirectory(CLR_HEADER).VirtualAddress > 0 Then
                        reader.BaseStream.Seek(ntHeaders.OptionalHeader32.DataDirectory(CLR_HEADER).VirtualAddress - ntHeaders.OptionalHeader32.BaseOfCode + TextBase, SeekOrigin.Begin)
                        CLR = MarshalBytesTo(Of IMAGE_COR20_HEADER)(reader)
                    End If
                End If
            Else ' We have a 64bit assembly
                TL.LogMessage("PEReader.Bitness", "This is a 64 bit assembly, reading the CLR Header")
                If ntHeaders.OptionalHeader64.NumberOfRvaAndSizes < MAX_HEADERS_TO_CHECK Then NumberOfHeadersToCheck = CInt(ntHeaders.OptionalHeader64.NumberOfRvaAndSizes)
                TL.LogMessage("PEReader.Bitness", "Checking " & NumberOfHeadersToCheck & " headers")

                For i As Integer = 0 To NumberOfHeadersToCheck - 1
                    If ntHeaders.OptionalHeader64.DataDirectory(i).Size > 0 Then
                        sectionHeaders.Add(MarshalBytesTo(Of IMAGE_SECTION_HEADER)(reader))
                    End If
                Next

                For Each SectionHeader As IMAGE_SECTION_HEADER In sectionHeaders
                    If SectionHeader.Name = ".text" Then
                        TL.LogMessage("PEReader.Bitness", "Found TEXT section")
                        TextBase = SectionHeader.PointerToRawData
                    End If
                Next

                If NumberOfHeadersToCheck >= CLR_HEADER + 1 Then ' Only test if the number of headers meets or exceeds the location of the CLR header
                    If ntHeaders.OptionalHeader64.DataDirectory(CLR_HEADER).VirtualAddress > 0 Then
                        reader.BaseStream.Seek(ntHeaders.OptionalHeader64.DataDirectory(CLR_HEADER).VirtualAddress - ntHeaders.OptionalHeader64.BaseOfCode + TextBase, SeekOrigin.Begin)
                        CLR = MarshalBytesTo(Of IMAGE_COR20_HEADER)(reader)
                        TL.LogMessage("PEReader.Bitness", "Read CLR header successfully")
                    End If
                End If

            End If

            ' Determine the bitness from the CLR header
            If OS32BitCompatible Then ' Could be an x86 or MSIL assembly so determine which
                If ((CLR.Flags And CLR_FLAGS.CLR_FLAGS_32BITREQUIRED) > 0) Then
                    TL.LogMessage("PEReader.Bitness", "Found ""32bit Required"" assembly")
                    ExecutableBitness = Bitness.Bits32
                Else
                    TL.LogMessage("PEReader.Bitness", "Found ""MSIL"" assembly")
                    ExecutableBitness = Bitness.BitsMSIL
                End If
            Else ' Must be an x64 assmebly
                TL.LogMessage("PEReader.Bitness", "Found ""64bit Required"" assembly")
                ExecutableBitness = Bitness.Bits64
            End If

            TL.LogMessage("PEReader", "Assembly required Runtime version: " & CLR.MajorRuntimeVersion & "." & CLR.MinorRuntimeVersion)
        Else ' Not an assembly so just use the FileHeader.Machine value to determine bitness
            TL.LogMessage("PEReader", "This is not an assembly, determining Bitness through the executable bitness flag")
            If OS32BitCompatible Then
                TL.LogMessage("PEReader.Bitness", "Found 32bit executable")
                ExecutableBitness = Bitness.Bits32
            Else
                TL.LogMessage("PEReader.Bitness", "Found 64bit executable")
                ExecutableBitness = Bitness.Bits64
            End If

        End If
    End Sub

    Friend ReadOnly Property BitNess As Bitness
        Get
            TL.LogMessage("PE.BitNess", "Returning: " & ExecutableBitness)
            Return ExecutableBitness
        End Get
    End Property

    Friend Function IsDotNetAssembly() As Boolean
        TL.LogMessage("PE.IsDotNetAssembly", "Returning: " & IsAssembly)
        Return IsAssembly
    End Function

    Friend Function SubSystem() As SubSystemType
        If OS32BitCompatible Then
            TL.LogMessage("PE.SubSystem", "Returning 32bit value: " & CType(ntHeaders.OptionalHeader32.Subsystem, SubSystemType).ToString())
            Return CType(ntHeaders.OptionalHeader32.Subsystem, SubSystemType) 'Return the 32bit header field
        Else
            TL.LogMessage("PE.SubSystem", "Returning 64bit value: " & CType(ntHeaders.OptionalHeader64.Subsystem, SubSystemType).ToString())
            Return CType(ntHeaders.OptionalHeader64.Subsystem, SubSystemType) 'Return the 64bit field
        End If
    End Function

    Private Shared Function MarshalBytesTo(Of T)(ByVal reader As BinaryReader) As T
        ' Unmanaged data
        Dim bytes As Byte() = reader.ReadBytes(Marshal.SizeOf(GetType(T)))

        ' Create a pointer to the unmanaged data pinned in memory to be accessed by unmanaged code
        Dim handle As GCHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned)

        ' Use our previously created pointer to unmanaged data and marshal to the specified type
        Dim theStructure As T = DirectCast(Marshal.PtrToStructure(handle.AddrOfPinnedObject(), GetType(T)), T)

        ' Deallocate pointer
        handle.Free()

        Return theStructure
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                Try
                    reader.Close()
                    stream.Close()
                    stream.Dispose()
                    stream = Nothing
                Catch ex As Exception ' Swallow any exceptions here
                End Try
            End If

        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

