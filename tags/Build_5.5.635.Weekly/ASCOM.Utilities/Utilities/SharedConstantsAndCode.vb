'These items are shared between the ASCOM.Utilities and ASCOM.Astrometry assemblies

Imports Microsoft.Win32
'Imports ASCOM.Utilities

Module SharedConstants
    Friend Const TRACE_TRANSFORM As String = "Trace Transform", TRACE_TRANSFORM_DEFAULT As Boolean = False
    Friend Const REGISTRY_CONFORM_FOLDER As String = "Software\ASCOM\Utilities"
End Module

#Region "Registry Utility Code"
Module RegistryCommonCode
    Friend Function GetBool(ByVal p_Name As String, ByVal p_DefaultValue As Boolean) As Boolean
        Dim l_Value As Boolean
        Dim m_HKCU, m_SettingsKey As RegistryKey

        m_HKCU = Registry.CurrentUser
        m_HKCU.CreateSubKey(REGISTRY_CONFORM_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_CONFORM_FOLDER, True)

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
        m_HKCU.CreateSubKey(REGISTRY_CONFORM_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_CONFORM_FOLDER, True)

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
        m_HKCU.CreateSubKey(REGISTRY_CONFORM_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_CONFORM_FOLDER, True)

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
        m_HKCU.CreateSubKey(REGISTRY_CONFORM_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_CONFORM_FOLDER, True)

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
        m_HKCU.CreateSubKey(REGISTRY_CONFORM_FOLDER)
        m_SettingsKey = m_HKCU.OpenSubKey(REGISTRY_CONFORM_FOLDER, True)

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