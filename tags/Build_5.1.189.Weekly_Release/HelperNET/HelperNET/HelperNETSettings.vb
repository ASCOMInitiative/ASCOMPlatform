Option Strict On
Imports Microsoft.Win32

'Class to manage state storage between Conform runs

'To add a new saved value:
'1) Decide on the variable name and its default value
'2) Create appropriately named constants similar to those below
'3) Create a property of the relevant type
'4) Create Get and Set code based on the patterns already implemented
'5) If the property is of a type not already handled,you will need to create a GetXXX function in the Utility code region

Friend Class HelperNETSettings
    Implements IDisposable

    Private m_HKCU, m_SettingsKey As RegistryKey
    Private Const REGISTRY_CONFORM_FOLDER As String = "Software\ASCOM\HelperNET"

    Private Const TRACE_XMLACCESS As String = "Trace XMLAccess", TRACE_XMLACCESS_DEFAULT As Boolean = False
    Private Const TRACE_PROFILE As String = "Trace Profile", TRACE_PROFILE_DEFAULT As Boolean = False
    Private Const PROFILE_ROOT_EDIT As String = "Profile Root Edit", PROFILE_ROOT_EDIT_DEFAULT As Boolean = False

#Region "New and Finalize"
    Sub New()
        m_HKCU = Registry.CurrentUser
        m_HKCU.CreateSubKey(REGISTRY_CONFORM_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_CONFORM_FOLDER, True)
    End Sub

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
            m_SettingsKey.Flush()
            m_SettingsKey.Close()
            m_SettingsKey = Nothing
            m_HKCU.Flush()
            m_HKCU.Close()
            m_HKCU = Nothing
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
    End Sub
#End Region

#Region "Parameters"
    Property TraceXMLAccess() As Boolean
        Get
            Return GetBool(TRACE_XMLACCESS, TRACE_XMLACCESS_DEFAULT)
        End Get
        Set(ByVal value As Boolean)
            SetName(m_SettingsKey, TRACE_XMLACCESS, value.ToString)
        End Set
    End Property

    Property TraceProfile() As Boolean
        Get
            Return GetBool(TRACE_PROFILE, TRACE_PROFILE_DEFAULT)
        End Get
        Set(ByVal value As Boolean)
            SetName(m_SettingsKey, TRACE_PROFILE, value.ToString)
        End Set
    End Property

    Property ProfileRootEdit() As Boolean
        Get
            Return GetBool(PROFILE_ROOT_EDIT, PROFILE_ROOT_EDIT_DEFAULT)
        End Get
        Set(ByVal value As Boolean)
            SetName(m_SettingsKey, PROFILE_ROOT_EDIT, value.ToString)
        End Set
    End Property

#End Region

#Region "Utility Code"
    Private Function GetBool(ByVal p_Name As String, ByVal p_DefaultValue As Boolean) As Boolean
        Dim l_Value As Boolean
        Try
            If m_SettingsKey.GetValueKind(p_Name) = RegistryValueKind.String Then ' Value does exist
                l_Value = CBool(m_SettingsKey.GetValue(p_Name))
            End If
        Catch ex As System.IO.IOException 'Value doesn't exist so create it
            SetName(m_SettingsKey, p_Name, p_DefaultValue.ToString)
            l_Value = p_DefaultValue
        Catch ex As Exception
            'LogMsg("GetBool", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
            l_Value = p_DefaultValue
        End Try
        Return l_Value
    End Function
    Private Function GetString(ByVal p_Name As String, ByVal p_DefaultValue As String) As String
        Dim l_Value As String
        l_Value = ""
        Try
            If m_SettingsKey.GetValueKind(p_Name) = RegistryValueKind.String Then ' Value does exist
                l_Value = m_SettingsKey.GetValue(p_Name).ToString
            End If
        Catch ex As System.IO.IOException 'Value doesn't exist so create it
            SetName(m_SettingsKey, p_Name, p_DefaultValue.ToString)
            l_Value = p_DefaultValue
        Catch ex As Exception
            'LogMsg("GetString", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
            l_Value = p_DefaultValue
        End Try
        Return l_Value
    End Function
    Private Function GetDouble(ByVal p_Key As RegistryKey, ByVal p_Name As String, ByVal p_DefaultValue As Double) As Double
        Dim l_Value As Double
        'LogMsg("GetDouble", GlobalVarsAndCode.MessageLevel.msgDebug, p_Name.ToString & " " & p_DefaultValue.ToString)
        Try
            If p_Key.GetValueKind(p_Name) = RegistryValueKind.String Then ' Value does exist
                l_Value = CDbl(p_Key.GetValue(p_Name))
            End If
        Catch ex As System.IO.IOException 'Value doesn't exist so create it
            SetName(p_Key, p_Name, p_DefaultValue.ToString)
            l_Value = p_DefaultValue
        Catch ex As Exception
            'LogMsg("GetDouble", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
            l_Value = p_DefaultValue
        End Try
        Return l_Value
    End Function
    Private Function GetDate(ByVal p_Name As String, ByVal p_DefaultValue As Date) As Date
        Dim l_Value As Date
        Try
            If m_SettingsKey.GetValueKind(p_Name) = RegistryValueKind.String Then ' Value does exist
                l_Value = CDate(m_SettingsKey.GetValue(p_Name))
            End If
        Catch ex As System.IO.IOException 'Value doesn't exist so create it
            SetName(m_SettingsKey, p_Name, p_DefaultValue.ToString)
            l_Value = p_DefaultValue
        Catch ex As Exception
            'LogMsg("GetDate", GlobalVarsAndCode.MessageLevel.msgError, "Unexpected exception: " & ex.ToString)
            l_Value = p_DefaultValue
        End Try
        Return l_Value
    End Function
    Private Sub SetName(ByVal p_Key As RegistryKey, ByVal p_Name As String, ByVal p_Value As String)
        p_Key.SetValue(p_Name, p_Value.ToString, RegistryValueKind.String)
        p_Key.Flush()
    End Sub
#End Region


End Class
