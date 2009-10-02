Imports ASCOM.Utilities
Imports Microsoft.Win32

Public Class Form1

    Private Const COMPONENT_CATEGORIES = "Component Categories"
    Dim TL As TraceLogger

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TL = New TraceLogger("", "Diagnostics")
        TL.Enabled = True
    End Sub

    Private Sub btnCOM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCOM.Click

        GetCOMRegistration("DriverHelper.Chooser")
        GetCOMRegistration("DriverHelper.ChooserSupport")
        GetCOMRegistration("DriverHelper.Profile")
        GetCOMRegistration("ASCOM.GeminiTelescope.Telescope")
        GetCOMRegistration("ScopeSim.Telescope")
    End Sub

    Sub GetCOMRegistration(ByVal ProgID As String)
        Dim RKey As RegistryKey
        TL.LogMessage("Start", "Processing ProgID: " & ProgID)
        RKey = Registry.ClassesRoot.OpenSubKey(ProgID)
        ProcessSubKey(RKey, 1, "None")
        RKey.Close()

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
                    Select Case ValueName
                        Case ""
                            TL.LogMessage("KeyValue", Space(p_Depth * 3) & "*** Default *** = " & p_Key.GetValue(ValueName))
                        Case "AppId"
                            p_Container = "AppId"
                            TL.LogMessage("KeyValue", Space(p_Depth * 3) & ValueName.ToString & " = " & p_Key.GetValue(ValueName))
                        Case Else
                            TL.LogMessage("KeyValue", Space(p_Depth * 3) & ValueName.ToString & " = " & p_Key.GetValue(ValueName))
                    End Select
                    If Microsoft.VisualBasic.Left(p_Key.GetValue(ValueName), 1) = "{" Then
                        'TL.LogMessage("ClassExpand", "Expanding " & p_Key.GetValue(ValueName))
                        Select Case p_Container
                            Case "CLSID"
                                RKey = Registry.ClassesRoot.OpenSubKey("CLSID").OpenSubKey(p_Key.GetValue(ValueName))
                            Case "TypeLib"
                                RKey = Registry.ClassesRoot.OpenSubKey("TypeLib").OpenSubKey(p_Key.GetValue(ValueName))
                            Case "AppId"
                                RKey = Registry.ClassesRoot.OpenSubKey("AppId").OpenSubKey(p_Key.GetValue(ValueName))
                            Case "AppID"
                                'Do nothing
                                RKey = Nothing
                            Case Else
                                RKey = p_Key.OpenSubKey(p_Key.GetValue(ValueName))
                        End Select
                        If Not RKey Is Nothing Then
                            ProcessSubKey(RKey, p_Depth + 1, "None")
                            RKey.Close()
                        End If
                    End If
                Next
            Catch ex As Exception
                TL.LogMessage("Ex1", ex.ToString)
            End Try
            Try
                SubKeys = p_Key.GetSubKeyNames
                For Each SubKey In SubKeys
                    TL.LogMessage("ProcessSubKey", Space(p_Depth * 3) & SubKey)
                    RKey = p_Key.OpenSubKey(SubKey)
                    Select Case SubKey.ToUpper
                        Case "TYPELIB"
                            'TL.LogMessage("Container", "TypeLib...")
                            Container = "TypeLib"
                            'RootKey = Registry.ClassesRoot.OpenSubKey("TypeLib").OpenSubKey(SubKey)
                        Case "CLSID"
                            'TL.LogMessage("Container", "CLSID...")
                            Container = "CLSID"
                            'RootKey = Registry.ClassesRoot.OpenSubKey("CLSID").OpenSubKey(SubKey)
                        Case "IMPLEMENTED CATEGORIES"
                            Container = COMPONENT_CATEGORIES
                            'TL.LogMessage("Container", "Component Categories...")
                        Case Else
                            'TL.LogMessage("Container", "Other...")
                            Container = "None"
                            'RootKey = RKey
                    End Select
                    If Microsoft.VisualBasic.Left(SubKey, 1) = "{" Then
                        Select Case p_Container
                            Case COMPONENT_CATEGORIES
                                'TL.LogMessage("ImpCat", "ImpCat")
                                RKey = Registry.ClassesRoot.OpenSubKey(COMPONENT_CATEGORIES).OpenSubKey(SubKey)
                                Container = "None"
                            Case Else

                        End Select
                    End If
                    ProcessSubKey(RKey, p_Depth + 1, Container)
                    RKey.Close()
                Next
            Catch ex As Exception
                TL.LogMessage("Ex2", ex.ToString)
            End Try
            ' TL.LogMessage("End of ProcessSubKey", p_Container & " " & p_Depth)
        End If

    End Sub


    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        End
    End Sub
End Class
