'These items are shared between the ASCOM.Utilities and ASCOM.Astrometry assemblies

Imports Microsoft.Win32
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Globalization
Imports ASCOM.Utilities
Imports System.Collections.Generic
Imports System.Diagnostics

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
    Friend Sub LogEvent(ByVal Caller As String, ByVal Msg As String, ByVal Severity As EventLogEntryType, ByVal Id As EventLogErrors, ByVal Except As String)
        Dim ELog As EventLog, MsgTxt As String

        If Not EventLog.SourceExists(EVENT_SOURCE) Then 'Create the event log of it doesn't exist
            EventLog.CreateEventSource(EVENT_SOURCE, EVENTLOG_NAME)
            LogEvent("LogEvent", "Event log created", EventLogEntryType.Information, EventLogErrors.EventLogCreated, Nothing) 'Place an initial entry
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
    <DllImport("Kernel32.dll", SetLastError:=True, CallingConvention:=CallingConvention.Winapi)> _
        Private Function IsWow64Process( _
              ByVal hProcess As System.IntPtr, _
              ByRef wow64Process As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    ''' <summary>
    ''' Return a message when a driver is not compatible with the requested 32/64bit application type. Returns anempty string if the driver is compatible
    ''' </summary>
    ''' <param name="ProgID">ProgID of the driver to be assessed</param>
    ''' <param name="RequiredBitness">Application bitness for which application compatibility should be tested</param>
    ''' <returns>String compatibility message or empty string if driver is fully compatible</returns>
    ''' <remarks></remarks>
    Friend Function DriverCompatibilityMessage(ByVal ProgID As String, ByVal RequiredBitness As Bitness) As String
        'Dim Drivers32Bit, Drivers64Bit As Generic.SortedList(Of String, String)
        Dim ProfileStore As RegistryAccess
        Dim InProcServer As PEReader = Nothing, Registered64Bit As Boolean, InprocServerBitness As Bitness
        Dim RK, RKInprocServer32 As RegistryKey, CLSID, InprocFilePath, CodeBase As String

        ProfileStore = New RegistryAccess("DriverCompatibilityMessage") 'Get access to the profile store

        DriverCompatibilityMessage = "" 'Set default return value as OK

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

        'Parse the COM registry section to determine whether this ProgID is an in-process DLL server.
        'If it is then parse the executable to determine whether it is a 32bit only driver and gie a suitable message if it is
        'Picks up some COM registration issues as well as a by-product.
        If ApplicationBits() = Bitness.Bits64 Then 'We have a 64bit application so check to see whether this is a 32bit only driver
            RK = Registry.ClassesRoot.OpenSubKey(ProgID & "\CLSID", False) 'Look in the 64bit section first
            If Not RK Is Nothing Then ' ProgID is registered and has a CLSID!
                CLSID = RK.GetValue("").ToString 'Get the CLSID for this ProgID
                RK.Close()

                RK = Registry.ClassesRoot.OpenSubKey("CLSID\" & CLSID) ' Check the 64bit registry section for this CLSID
                If RK Is Nothing Then 'We don't have an entry in the 64bit CLSID registry section so try the 32bit section
                    RK = Registry.ClassesRoot.OpenSubKey("Wow6432Node\CLSID\" & CLSID) 'Check the 32bit registry section
                    Registered64Bit = False
                Else
                    Registered64Bit = True
                End If
                If Not RK Is Nothing Then 'We have a CLSID entry so process it
                    RKInprocServer32 = RK.OpenSubKey("InprocServer32")
                    RK.Close()
                    If Not RKInprocServer32 Is Nothing Then ' This is an in process server so test for compatibility
                        InprocFilePath = RKInprocServer32.GetValue("", "").ToString ' Get the file location from the default position
                        CodeBase = RKInprocServer32.GetValue("CodeBase", "").ToString 'Get the codebase if present to override the default value
                        If CodeBase <> "" Then InprocFilePath = CodeBase

                        If (InprocFilePath <> "") And (Right(Trim(InprocFilePath), 4).ToUpper = ".DLL") Then ' We do have a path to the server and it is a dll
                            Try
                                InProcServer = New PEReader(InprocFilePath) 'Get hold of the executable so we can determine its characteristics
                                InprocServerBitness = InProcServer.BitNess
                                If InprocServerBitness = Bitness.Bits32 Then '32bit driver executable
                                    If Registered64Bit Then '32bit driver executable registered in 64bit COM
                                        DriverCompatibilityMessage = "This driver is a 32bit executable but is registered as a 64bit COM application." & vbCrLf & DRIVER_AUTHOR_MESSAGE_DRIVER
                                    Else '32bit driver executable registered in 32bit COM
                                        DriverCompatibilityMessage = "This driver is a 32bit executable and is only registered as a 32bit COM application." & vbCrLf & DRIVER_AUTHOR_MESSAGE_DRIVER
                                    End If
                                Else '64bit driver
                                    If Registered64Bit Then '64bit driver executable registered in 64bit COM section
                                        'This is the only OK combination, no message for this!
                                    Else '64bit driver executable registered in 32bit COM
                                        DriverCompatibilityMessage = "This driver is a 64bit executable but is only registered as a 32bit COM application." & vbCrLf & DRIVER_AUTHOR_MESSAGE_INSTALLER
                                    End If
                                End If
                            Catch ex As System.IO.FileNotFoundException 'Cannot open the file
                                DriverCompatibilityMessage = ProgID & " - Cannot find the driver: """ & vbCrLf & InprocFilePath & """"
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
                        End If
                        RKInprocServer32.Close() 'Clean up the InProcServer registry key
                    Else 'This is not an inprocess DLL so no need to test further and no error message to return
                        'Please leave this empty clause here so the logic is clear!
                    End If
                Else 'Cannot find a CLSID entry
                    DriverCompatibilityMessage = "Unable to find a CLSID entry for this driver (ProgID: " & ProgID & "), please re-install."
                End If
            Else 'No COM ProgID registry entry
                DriverCompatibilityMessage = "This driver (ProgID: " & ProgID & ") does not appear to be registered for COM, please re-install."
            End If
        Else 'We are a 32bit application so make sure the executable is not 64bit only
            RK = Registry.ClassesRoot.OpenSubKey(ProgID & "\CLSID", False) 'Look in the 32bit registry
            If Not RK Is Nothing Then ' ProgID is registered and has a CLSID!
                CLSID = RK.GetValue("").ToString 'Get the CLSID for this ProgID
                RK.Close()

                RK = Registry.ClassesRoot.OpenSubKey("CLSID\" & CLSID) ' Check the 32bit registry section for this CLSID
                If Not RK Is Nothing Then 'We have a CLSID entry so process it
                    RKInprocServer32 = RK.OpenSubKey("InprocServer32")
                    RK.Close()
                    If Not RKInprocServer32 Is Nothing Then ' This is an in process server so test for compatibility
                        InprocFilePath = RKInprocServer32.GetValue("", "").ToString ' Get the file location from the default position
                        CodeBase = RKInprocServer32.GetValue("CodeBase", "").ToString 'Get the codebase if present to override the default value
                        If CodeBase <> "" Then InprocFilePath = CodeBase

                        If (InprocFilePath <> "") And (Right(Trim(InprocFilePath), 4).ToUpper = ".DLL") Then ' We do have a path to the server and it is a dll
                            Try
                                InProcServer = New PEReader(InprocFilePath) 'Get hold of the executable so we can determine its characteristics
                                If InProcServer.BitNess = Bitness.Bits64 Then '64bit only driver executable
                                    DriverCompatibilityMessage = "This driver is a 64bit executable and is not compatible with this 32bit application." & vbCrLf & DRIVER_AUTHOR_MESSAGE_DRIVER
                                End If
                            Catch ex As System.IO.FileNotFoundException 'Cannot open the file
                                DriverCompatibilityMessage = ProgID & " - Cannot find the driver: """ & InprocFilePath & """"
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
                        End If
                        RKInprocServer32.Close() 'Clean up the InProcServer registry key
                    Else 'This is not an inprocess DLL so no need to test further and no error message to return
                        'Please leave this empty clause here so the logic is clear!
                    End If
                Else 'Cannot find a CLSID entry
                    DriverCompatibilityMessage = "Unable to find a CLSID entry for this driver (ProgID: " & ProgID & "), please re-install."
                End If
            Else 'No COM ProgID registry entry
                DriverCompatibilityMessage = "This driver (ProgID: " & ProgID & ") does not appear to be registered for COM, please re-install."
            End If

        End If

        Return DriverCompatibilityMessage
    End Function

End Module
#End Region

#Region "Old Code"
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
        Dim ModuleFileName, ForcedFileNameKey, ForcedSeparatorKey As String, ForcedFileNames, ForcedSeparators As Generic.SortedList(Of String, String)
        Dim pc As PerformanceCounter

        ConditionPlatformVersion = PlatformVersion ' Set default action to return the supplied vaue
        Try
            ModuleFileName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName) 'Get the name of the executable without path or file extension
            If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "  ModuleFileName: """ & ModuleFileName & """ """ & _
                                                    Process.GetCurrentProcess().MainModule.FileName & """")
            If Left(ModuleFileName.ToUpper, 3) = "IS-" Then ' Likely to be an old Inno installer so try and get the parent's name
                If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "    Inno installer temporary executable detected, searching for parent process!")
                If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "    Old Module Filename: " & ModuleFileName)
                pc = New PerformanceCounter("Process", "Creating Process ID", Process.GetCurrentProcess().ProcessName)
                ModuleFileName = Path.GetFileNameWithoutExtension(Process.GetProcessById(CInt(pc.NextValue())).MainModule.FileName)
                If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "    New Module Filename: " & ModuleFileName)
                pc.Close()
                pc.Dispose()
            End If

            'Force any particular platform version numnber this application requires
            ForcedFileNames = Profile.EnumProfile(PLATFORM_VERSION_EXCEPTIONS) 'Get the list of filenames requiring specific versions

            For Each ForcedFileName As Generic.KeyValuePair(Of String, String) In ForcedFileNames ' Check each forced file in turn 
                If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "  ForcedFileName: """ & ForcedFileName.Key & """ """ & _
                                                        ForcedFileName.Value & """ """ & _
                                                        UCase(Path.GetFileNameWithoutExtension(ForcedFileName.Key)) & """ """ & _
                                                        UCase(Path.GetFileName(ForcedFileName.Key)) & """ """ & _
                                                        UCase(ForcedFileName.Key) & """ """ & _
                                                        ForcedFileName.Key & """ """ & _
                                                        UCase(ModuleFileName) & """")
                If ForcedFileName.Key.Contains(".") Then
                    ForcedFileNameKey = Path.GetFileNameWithoutExtension(ForcedFileName.Key)
                Else
                    ForcedFileNameKey = ForcedFileName.Key
                End If

                If UCase(ForcedFileNameKey) = UCase(ModuleFileName) Then ' If the current file matches a forced file name then return the required Platform version
                    ConditionPlatformVersion = ForcedFileName.Value
                    If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "  Matched file: """ & ModuleFileName & """ """ & ForcedFileNameKey & """")
                End If
            Next

            ForcedSeparators = Profile.EnumProfile(PLATFORM_VERSION_SEPARATOR_EXCEPTIONS) 'Get the list of filenames requiring specific versions

            For Each ForcedSeparator As Generic.KeyValuePair(Of String, String) In ForcedSeparators ' Check each forced file in turn 
                If Not TL Is Nothing Then TL.LogMessage("ConditionPlatformVersion", "  ForcedFileName: """ & ForcedSeparator.Key & """ """ & _
                                                        ForcedSeparator.Value & """ """ & _
                                                        UCase(Path.GetFileNameWithoutExtension(ForcedSeparator.Key)) & """ """ & _
                                                        UCase(Path.GetFileName(ForcedSeparator.Key)) & """ """ & _
                                                        UCase(ForcedSeparator.Key) & """ """ & _
                                                        ForcedSeparator.Key & """ """ & _
                                                        UCase(ModuleFileName) & """")
                If ForcedSeparator.Key.Contains(".") Then
                    ForcedSeparatorKey = Path.GetFileNameWithoutExtension(ForcedSeparator.Key)
                Else
                    ForcedSeparatorKey = ForcedSeparator.Key
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

Friend Class PEReader
    Implements IDisposable

#Region "Constants"
    Friend Const CLR_HEADER As Integer = 14
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
#End Region

#Region "Structs"

    <StructLayout(LayoutKind.Sequential)> _
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
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> _
        Friend e_res1 As UInt16() 'Reserved words
        Friend e_oemid As UInt16 'OEM identifier (for e_oeminfo)
        Friend e_oeminfo As UInt16 '
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=10)> _
        Friend e_res2 As UInt16() 'Reserved words
        Friend e_lfanew As UInt32 'File address of new exe header
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure IMAGE_NT_HEADERS
        Friend Signature As UInt32
        Friend FileHeader As IMAGE_FILE_HEADER
        Friend OptionalHeader32 As IMAGE_OPTIONAL_HEADER32
        Friend OptionalHeader64 As IMAGE_OPTIONAL_HEADER64
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure IMAGE_FILE_HEADER
        Friend Machine As UInt16
        Friend NumberOfSections As UInt16
        Friend TimeDateStamp As UInt32
        Friend PointerToSymbolTable As UInt32
        Friend NumberOfSymbols As UInt32
        Friend SizeOfOptionalHeader As UInt16
        Friend Characteristics As UInt16
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
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
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)> _
        Friend DataDirectory As IMAGE_DATA_DIRECTORY()
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
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
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)> _
        Friend DataDirectory As IMAGE_DATA_DIRECTORY()
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure IMAGE_DATA_DIRECTORY
        Friend VirtualAddress As UInt32
        Friend Size As UInt32
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure IMAGE_SECTION_HEADER
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=8)> _
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

    <StructLayout(LayoutKind.Explicit)> _
    Friend Structure Misc
        <FieldOffset(0)> _
        Friend PhysicalAddress As UInt32
        <FieldOffset(0)> _
        Friend VirtualSize As UInt32
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
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
    Private ReadOnly _dosHeader As IMAGE_DOS_HEADER
    Private _ntHeaders As IMAGE_NT_HEADERS
    Private ReadOnly CLR As IMAGE_COR20_HEADER
    Private ReadOnly _sectionHeaders As Generic.IList(Of IMAGE_SECTION_HEADER) = New Generic.List(Of IMAGE_SECTION_HEADER)
    Private TextBase As UInteger
    Private reader As BinaryReader
    Private stream As Stream
#End Region

    Friend Sub New(ByVal FileName As String)
        If Left(FileName, 5).ToUpper = "FILE:" Then FileName = New Uri(FileName).LocalPath 'Convert uri to local path if required, uri paths are not supported by FileStream

        stream = New FileStream(FileName, FileMode.Open, FileAccess.Read)
        reader = New BinaryReader(stream)

        reader.BaseStream.Seek(0, SeekOrigin.Begin) ' Reset reader position, just in case
        _dosHeader = MarshalBytesTo(Of IMAGE_DOS_HEADER)(reader) ' Read MS-DOS header section
        If _dosHeader.e_magic <> &H5A4D Then ' MS-DOS magic number should read 'MZ'
            Throw New InvalidOperationException("File is not a portable executable.")
        End If

        reader.BaseStream.Seek(_dosHeader.e_lfanew, SeekOrigin.Begin) ' Skip MS-DOS stub and seek reader to NT Headers
        _ntHeaders.Signature = MarshalBytesTo(Of UInt32)(reader) ' Read NT Headers
        If _ntHeaders.Signature <> &H4550 Then ' Make sure we have 'PE' in the pe signature. Options: 
            Throw New InvalidOperationException("Invalid portable executable signature in NT header.")
        End If
        _ntHeaders.FileHeader = MarshalBytesTo(Of IMAGE_FILE_HEADER)(reader) ' This starts 4 bytes on from the start of the signature (already here by reading the signature itself)


        If Is32bitCode() Then ' Read optional headers
            _ntHeaders.OptionalHeader32 = MarshalBytesTo(Of IMAGE_OPTIONAL_HEADER32)(reader)
            If _ntHeaders.OptionalHeader32.NumberOfRvaAndSizes <> &H10 Then ' Should have 16 data directories
                Throw New InvalidOperationException("Invalid number of data directories in NT header")
            End If

            For i As Integer = 0 To CInt(_ntHeaders.OptionalHeader32.NumberOfRvaAndSizes) - 1
                If _ntHeaders.OptionalHeader32.DataDirectory(i).Size > 0 Then
                    _sectionHeaders.Add(MarshalBytesTo(Of IMAGE_SECTION_HEADER)(reader))
                End If
            Next

            For Each SectionHeader As IMAGE_SECTION_HEADER In _sectionHeaders
                If SectionHeader.Name = ".text" Then TextBase = SectionHeader.PointerToRawData
            Next

            'MsgBox("CLR_HEADER Virtual Address: " & Hex(_ntHeaders.OptionalHeader32.DataDirectory(CLR_HEADER).VirtualAddress) & " BaseOfCode" & Hex(_ntHeaders.OptionalHeader32.BaseOfCode) & " Pointer to raw data: " & Hex(TextBase) & "Ans: " & Hex(_ntHeaders.OptionalHeader32.DataDirectory(CLR_HEADER).VirtualAddress - _ntHeaders.OptionalHeader32.BaseOfCode + TextBase))

            If _ntHeaders.OptionalHeader32.DataDirectory(CLR_HEADER).VirtualAddress > 0 Then
                reader.BaseStream.Seek(_ntHeaders.OptionalHeader32.DataDirectory(CLR_HEADER).VirtualAddress - _ntHeaders.OptionalHeader32.BaseOfCode + TextBase, SeekOrigin.Begin)
                CLR = MarshalBytesTo(Of IMAGE_COR20_HEADER)(reader)
            End If
        Else
            _ntHeaders.OptionalHeader64 = MarshalBytesTo(Of IMAGE_OPTIONAL_HEADER64)(reader)
            If _ntHeaders.OptionalHeader64.NumberOfRvaAndSizes <> &H10 Then ' Should have 16 data directories
                Throw New InvalidOperationException("Invalid number of data directories in NT header")
            End If

        End If

    End Sub

    Friend Function GetDOSHeader() As IMAGE_DOS_HEADER
        Return _dosHeader
    End Function

    Friend Function GetPESignature() As UInt32
        Return _ntHeaders.Signature
    End Function

    Friend Function GetFileHeader() As IMAGE_FILE_HEADER
        Return _ntHeaders.FileHeader
    End Function

    Friend Function GetOptionalHeaders32() As IMAGE_OPTIONAL_HEADER32
        Return _ntHeaders.OptionalHeader32
    End Function

    Friend Function GetOptionalHeaders64() As IMAGE_OPTIONAL_HEADER64
        Return _ntHeaders.OptionalHeader64
    End Function

    Friend Function Is32bitCode() As Boolean
        Return ((_ntHeaders.FileHeader.Characteristics And &H100) = &H100)
    End Function

    Friend ReadOnly Property BitNess As Bitness
        Get
            Dim RetVal As Bitness
            If IsDotNetAssembly() Then
                If Is32bitCode() Then
                    If ((CLR.Flags And CLR_FLAGS.CLR_FLAGS_32BITREQUIRED) > 0) Then
                        RetVal = BitNess.Bits32
                    Else
                        RetVal = BitNess.BitsMSIL
                    End If
                Else
                    RetVal = BitNess.Bits64
                End If

            Else
                If Is32bitCode() Then
                    RetVal = BitNess.Bits32
                Else
                    RetVal = BitNess.Bits64
                End If

            End If
            Return RetVal
        End Get
    End Property

    Friend Function IsDotNetAssembly() As Boolean
        If Is32bitCode() Then
            Return (_ntHeaders.OptionalHeader32.DataDirectory(CLR_HEADER).Size > 0)
        Else
            Return (_ntHeaders.OptionalHeader64.DataDirectory(CLR_HEADER).Size > 0)
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
                reader.Close()

                Stream.Close()
                Stream.Dispose()
                Stream = Nothing
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

