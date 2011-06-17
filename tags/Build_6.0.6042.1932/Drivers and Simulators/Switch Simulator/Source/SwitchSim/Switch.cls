VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Switch"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ASCOM Switch driver for Switch Simulator"
'============
' Switch.CLS
'============
'
' Implementation of the ASCOM Sitch class
'
' Initial code by Jon Brewster in Feb 2007
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 29-Jan-07 jab     Initial edit
' 02-Jun-07 jab     Added naming lables to the switches
' 06-Jun-07 jab     Added ISwitch
' -----------------------------------------------------------------------------

Option Explicit
Implements ISwitch

' ------------
' Class Events
' ------------

Private Sub Class_Initialize()

    If Not g_bRunExecutable Then _
        g_iConnections = g_iConnections + 1
    
End Sub

Private Sub Class_Terminate()
    
    If Not g_bRunExecutable Then
        If g_iConnections > 1 Then
            g_iConnections = g_iConnections - 1
        Else
            Unload g_handBox
        End If
    End If
    
End Sub

' ----------
' Properties
' ----------

Public Property Get Connected() As Boolean

    Connected = g_bConnected
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Connected: " & Connected
    End If

End Property

Public Property Let Connected(ByVal newVal As Boolean)
    Dim out As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Connected: " & g_bConnected & " -> " & newVal
    End If
    
    If newVal Then
        g_bConnected = True
        out = " (done)"
    Else
        If Not g_bRunExecutable Then      ' ignore clear if run as executable
            If g_iConnections < 2 Then
                g_bConnected = False      ' allow clear if last client
                out = " (done)"
            Else
                out = " (ignored, not last user)"
            End If
        Else
            out = " (ignored, not auto launch)"
        End If
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd out
    End If
    
End Property

Public Property Get Description() As String

    Description = DESC
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Description: " & Description
    End If
    
End Property

Public Property Get DriverInfo() As String

    '
    ' Use the Project/Properties sheet, Make tab, to set these
    ' items. That way they will show in the Version tab of the
    ' Explorer property sheet, and the exact same data will
    ' show in Telescope.DriverInfo.
    '
    DriverInfo = App.FileDescription & " " & _
                App.Major & "." & App.Minor & "." & App.Revision
    If App.CompanyName <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.CompanyName
    If App.LegalCopyright <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.LegalCopyright
    If App.Comments <> "" Then _
        DriverInfo = DriverInfo & vbCrLf & App.Comments
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "DriverInfo: " & DriverInfo
    End If
    
End Property

Public Property Get DriverVersion() As String
            
    DriverVersion = App.Major & "." & App.Minor
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "DriverVersion: " & DriverVersion
    End If
    
End Property

Public Property Get InterfaceVersion() As Integer
            
    InterfaceVersion = 1
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "InterfaceVersion: " & InterfaceVersion
    End If
    
End Property

Public Property Get MaxSwitch() As Integer

    MaxSwitch = g_iMaxSwitch
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "MaxSwitch: " & MaxSwitch
    End If

End Property

Public Property Get Name() As String
    
    Name = DESC
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Name: " & Name
    End If
    
End Property

' -------
' Methods
' -------

Public Function GetSwitch(ByVal SID As Integer) As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkActivity.Value = 1 Then _
            g_show.TrafficStart "GetSwitch " & SID & ": "
    End If
    
    check_connected
    
    If SID < (IIf(g_bZero, 0, 1)) Or SID > g_iMaxSwitch Then
        If Not g_show Is Nothing Then
            If g_show.chkActivity.Value = 1 Then _
                g_show.TrafficEnd " (error, out of range)"
        End If
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "GetSwitch: " & MSG_VAL_OUTOFRANGE
    End If
    
    If Not g_bCanGetSwitch(SID) Then
        If Not g_show Is Nothing Then
            If g_show.chkActivity.Value = 1 Then _
                g_show.TrafficEnd " (error, not enabled)"
        End If
        Err.Raise SCODE_SWITCH_NOT_ENABLED, ERR_SOURCE, _
            "GetSwitch: " & MSG_SWITCH_NOT_ENABLED
    End If
    
    GetSwitch = g_bSwitchState(SID)
    
    If Not g_show Is Nothing Then
        If g_show.chkActivity.Value = 1 Then _
            g_show.TrafficEnd IIf(GetSwitch, "On", "Off")
    End If
    
End Function

Public Function GetSwitchName(ByVal SID As Integer) As String

    If Not g_show Is Nothing Then
        If g_show.chkActivity.Value = 1 Then _
            g_show.TrafficStart "GetSwitchName " & SID & ": "
    End If
    
    check_connected
    
    If SID < (IIf(g_bZero, 0, 1)) Or SID > g_iMaxSwitch Then
        If Not g_show Is Nothing Then
            If g_show.chkActivity.Value = 1 Then _
                g_show.TrafficEnd " (error, out of range)"
        End If
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "GetSwitchName: " & MSG_VAL_OUTOFRANGE
    End If
    
    GetSwitchName = g_sSwitchName(SID)
    If Len(GetSwitchName) <= 0 Then _
        GetSwitchName = CStr(SID)
    
    If Not g_show Is Nothing Then
        If g_show.chkActivity.Value = 1 Then _
            g_show.TrafficEnd """" & GetSwitchName & """"
    End If
    
End Function

Public Sub SetSwitch(ByVal SID As Integer, ByVal val As Boolean)

    If Not g_show Is Nothing Then
        If g_show.chkActivity.Value = 1 Then _
            g_show.TrafficStart "SetSwitch " & SID & ": -> " & IIf(val, "On", "Off")
    End If
    
    check_connected
    
    If SID < (IIf(g_bZero, 0, 1)) Or SID > g_iMaxSwitch Then
        If Not g_show Is Nothing Then
            If g_show.chkActivity.Value = 1 Then _
                g_show.TrafficEnd " (error, out of range)"
        End If
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "SetSwitch: " & MSG_VAL_OUTOFRANGE
    End If

    If Not g_bCanSetSwitch(SID) Then
        If Not g_show Is Nothing Then
            If g_show.chkActivity.Value = 1 Then _
                g_show.TrafficEnd " (error, not enabled)"
        End If
        Err.Raise SCODE_SWITCH_NOT_ENABLED, ERR_SOURCE, _
            "SetSwitch: " & MSG_SWITCH_NOT_ENABLED
    End If
    
    g_bSwitchState(SID) = val
    g_handBox.SetLED SID, val
    
    If Not g_show Is Nothing Then
        If g_show.chkActivity.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SetSwitchName(ByVal SID As Integer, ByVal val As String)

    If Not g_show Is Nothing Then
        If g_show.chkActivity.Value = 1 Then _
            g_show.TrafficStart "SetSwitchName " & SID & ": -> """ & val & """"
    End If
    
    check_connected
    
    If SID < (IIf(g_bZero, 0, 1)) Or SID > g_iMaxSwitch Or Len(val) > 20 Then
        If Not g_show Is Nothing Then
            If g_show.chkActivity.Value = 1 Then _
                g_show.TrafficEnd " (error, out of range)"
        End If
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "SetSwitchName: " & MSG_VAL_OUTOFRANGE
    End If
    
    g_sSwitchName(SID) = val
    g_handBox.DisplayName SID
    
    If Not g_show Is Nothing Then
        If g_show.chkActivity.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SetupDialog()
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SetupDialog"
    End If
    
    DoSetup
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

'---------------------------------------------------------------------
'
' check_connected() - Raise an error if the focuser is not connected
'
'---------------------------------------------------------------------
Private Sub check_connected()

    If Not g_bConnected Then _
        Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
        
End Sub

' =========================
' Implementation of ISwitch
' =========================

Private Property Let ISwitch_Connected(ByVal RHS As Boolean)
    Connected = RHS
End Property

Private Property Get ISwitch_Connected() As Boolean
    ISwitch_Connected = Connected
End Property

Private Property Get ISwitch_Description() As String
    ISwitch_Description = Description
End Property

Private Property Get ISwitch_DriverInfo() As String
    ISwitch_DriverInfo = DriverInfo
End Property

Private Property Get ISwitch_DriverVersion() As String
    ISwitch_DriverVersion = DriverVersion
End Property

Private Function ISwitch_GetSwitch(ByVal ID As Integer) As Boolean
    ISwitch_GetSwitch = GetSwitch(ID)
End Function

Private Function ISwitch_GetSwitchName(ByVal ID As Integer) As String
    ISwitch_GetSwitchName = GetSwitchName(ID)
End Function

Private Property Get ISwitch_InterfaceVersion() As Integer
    ISwitch_InterfaceVersion = InterfaceVersion
End Property

Private Property Get ISwitch_MaxSwitch() As Integer
    ISwitch_MaxSwitch = MaxSwitch
End Property

Private Property Get ISwitch_Name() As String
    ISwitch_Name = "I_" & Name
End Property

Private Sub ISwitch_SetupDialog()
    SetupDialog
End Sub

Private Sub ISwitch_SetSwitch(ByVal ID As Integer, ByVal val As Boolean)
    Call SetSwitch(ID, val)
End Sub

Private Sub ISwitch_SetSwitchName(ByVal ID As Integer, ByVal val As String)
    Call SetSwitchName(ID, val)
End Sub

