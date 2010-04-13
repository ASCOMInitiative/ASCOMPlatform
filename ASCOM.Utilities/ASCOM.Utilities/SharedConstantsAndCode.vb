'These items are shared between the ASCOM.Utilities and ASCOM.Astrometry assemblies

Imports Microsoft.Win32
Imports System.Diagnostics

Module SharedConstants
    Friend Const PLATFORM_VERSION As String = "6.0" 'This is the master platform version and is set during profile migration

    ' Trace settings values, these are used to persist trace values on a per user basis
    Friend Const TRACE_TRANSFORM As String = "Trace Transform", TRACE_TRANSFORM_DEFAULT As Boolean = False
    Friend Const REGISTRY_UTILITIES_FOLDER As String = "Software\ASCOM\Utilities"

    'Settings for the ASCOM Windows event log
    Friend Const EVENT_SOURCE As String = "ASCOM Platform" 'Name of the the event source
    Friend Const EVENTLOG_NAME As String = "ASCOM" 'Name of the event log as it appears in Windows event viewer
End Module

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