'These items are shared between the ASCOM.Utilities and ASCOM.Astrometry assemblies

Imports Microsoft.Win32
Imports System.Reflection
Imports System.Runtime.InteropServices

#Region "Registry Utility Code"
Module RegistryCommonCode
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
            SetName(p_Name, p_DefaultValue.ToString)
            l_Value = p_DefaultValue
        Catch ex As Exception
            'LogMsg("GetBool", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
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
            SetName(p_Name, p_DefaultValue.ToString)
            l_Value = p_DefaultValue
        Catch ex As Exception
            'LogMsg("GetString", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
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
            SetName(p_Name, p_DefaultValue.ToString)
            l_Value = p_DefaultValue
        Catch ex As Exception
            'LogMsg("GetDouble", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
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
            SetName(p_Name, p_DefaultValue.ToString)
            l_Value = p_DefaultValue
        Catch ex As Exception
            'LogMsg("GetDate", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
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
    Friend Sub LogEvent(ByVal Caller As String, ByVal Msg As String, ByVal Severity As EventLogEntryType, ByVal Id As Integer, ByVal Except As String)
        Dim ELog As EventLog, MsgTxt As String

        If Not EventLog.SourceExists(EVENT_SOURCE) Then 'Create the event log of it doesn't exist
            EventLog.CreateEventSource(EVENT_SOURCE, EVENTLOG_NAME)
            LogEvent("LogEvent", "Event log created", EventLogEntryType.Information, 0, Nothing) 'Place an initial entry
        End If
        ELog = New EventLog(EVENTLOG_NAME, ".", EVENT_SOURCE) 'Create a pointer to the event log

        MsgTxt = Caller & " - " & Msg 'Format the message to be logged
        If Not Except Is Nothing Then MsgTxt += vbCrLf & Except
        ELog.WriteEntry(MsgTxt, Severity, Id) 'Write the message to the error log
        ELog.Close()
        ELog.Dispose()
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
            AssemblyInfo(TL, "Executing Assembly", Assembly.GetExecutingAssembly)
            AssemblyInfo(TL, "Entry Assembly", Assembly.GetEntryAssembly)
            AssemblyInfo(TL, "Calling Assembly", Assembly.GetCallingAssembly)
            TL.LogMessage("Versions", "")
        Catch ex As Exception 'Just log the exception, we don't want the caller to know this diagnostic code failed
            TL.LogMessage("Versions", "Exception: " & ex.ToString)
        End Try
    End Sub

    Friend Enum Bitness
        Bits32
        Bits64
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

        'Dim processHandle As Long = GetProcessHandle(System.Diagnostics.Process.GetCurrentProcess().Id)
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
    <DllImport("Kernel32.dll", SetLastError:=True, CallingConvention:=CallingConvention.Winapi)> _
        Private Function IsWow64Process( _
              ByVal hProcess As System.IntPtr, _
              ByRef wow64Process As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

End Module
#End Region