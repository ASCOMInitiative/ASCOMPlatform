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
Attribute VB_Description = "ASCOM Switch driver for RoboFocus server"
'============
' Switch.CLS
'============
'
' Implementation of the ASCOM RoboFocus Focuser class
'
' Initial code by Jon Brewster in Apr 2003
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 29-Jan-07 jab     Initial edit
' 02-Jun-07 jab     Added naming lables to the switches
' 06-Jun-07 jab     Added ISwitch
'---------------------------------------------------------------------

Option Explicit
Implements ISwitch

' ------------
' Class Events
' ------------

Private Sub Class_Initialize()

    ' count references
    g_iSwitchConnections = g_iSwitchConnections + 1

    ' get access to vendor specific API
    If fc Is Nothing Then _
        Set fc = New FocusControl
            
End Sub

Private Sub Class_Terminate()

    If g_iSwitchConnections > 0 Then
        g_iSwitchConnections = g_iSwitchConnections - 1
        
        ' if launched as server then reference counting will determine behavior
        If Not g_bRunExecutable Then
            If g_iFocuserConnections <= 0 And g_iSwitchConnections <= 0 Then
                Set fc = Nothing    ' last reference, so shut down
                                    ' "end" in fc's terminate will do the deed
            End If
        End If
    End If
    
End Sub

' ----------
' Properties
' ----------

Public Property Get Connected() As Boolean

    Connected = fc.getCommStatus()

End Property

Public Property Let Connected(ByVal newVal As Boolean)
    Dim val As Boolean

    ' If no change is requested, then get out
    If newVal = fc.getCommStatus() Then _
        Exit Property
    
    If newVal Then
        val = fc.actOpenComm()
        If Not val Then _
            Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, _
                "Property Connected. " & MSG_NOT_CONNECTED
    Else
        If Not g_bRunExecutable Then
            If g_iFocuserConnections <= 0 And g_iSwitchConnections <= 1 Then _
                val = fc.actCloseComm()   ' allow close if last client
        End If
    End If
    
End Property

Public Property Get Description() As String

    Description = DESC
       
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
    
End Property

Public Property Get DriverVersion() As String
            
    DriverVersion = App.Major & "." & App.Minor
    
End Property

Public Property Get InterfaceVersion() As Integer
            
    InterfaceVersion = 1
       
End Property

Public Property Get MaxSwitch() As Integer

    MaxSwitch = 4

End Property

Public Property Get Name() As String
    
    Name = DESC
    
End Property

' -------
' Methods
' -------

Public Function GetSwitch(ByVal id As Integer) As Boolean

    check_connected
    
    If id < 1 Or id > 4 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "GetSwitch" & MSG_VAL_OUTOFRANGE
            
    GetSwitch = fc.getUserButtonState(id)
    
End Function

Public Function GetSwitchName(ByVal id As Integer) As String
    
    check_connected
    
    If id < 1 Or id > 4 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "GetSwitchName" & MSG_VAL_OUTOFRANGE
            
    GetSwitchName = fc.getUserButtonCaption(id)
    
End Function

Public Sub SetSwitch(ByVal id As Integer, ByVal val As Boolean)

    check_connected
    
    If id < 1 Or id > 4 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "SetSwitch" & MSG_VAL_OUTOFRANGE
    
    If Me.GetSwitch(id) <> val Then _
        Call frmMain.userButton_Click(id)
  
End Sub

Public Sub SetSwitchName(ByVal id As Integer, ByVal val As String)

    Dim retVal As String
    
    check_connected

    If id < 1 Or id > 4 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "SetSwitchName" & MSG_VAL_OUTOFRANGE
    
    retVal = fc.setUserButtonCaption(id, val)
    
End Sub

Public Sub SetupDialog()

    ' We'll do it, but the usual behavior is to not return until complete.
    ' The implementation of RoboFocus code has this dialog as non-modal
    ' for the application ...
    
    frmConfig.Show
    Call SetWindowPos(frmConfig.hWnd, HWND_TOPMOST, 0, 0, 0, 0, FLAGS)
    
    While frmConfig.Visible
        DoEvents    ' wait till window goes away
    Wend
        
End Sub

'---------------------------------------------------------------------
'
' check_connected() - Raise an error if the focuser is not connected
'
'---------------------------------------------------------------------
Private Sub check_connected()

    If Not fc.getCommStatus() Then _
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

Private Function ISwitch_GetSwitch(ByVal id As Integer) As Boolean
    ISwitch_GetSwitch = GetSwitch(id)
End Function

Private Function ISwitch_GetSwitchName(ByVal id As Integer) As String
    ISwitch_GetSwitchName = GetSwitchName(id)
End Function

Private Property Get ISwitch_InterfaceVersion() As Integer
    ISwitch_InterfaceVersion = InterfaceVersion
End Property

Private Property Get ISwitch_MaxSwitch() As Integer
    ISwitch_MaxSwitch = MaxSwitch
End Property

Private Property Get ISwitch_Name() As String
    ISwitch_Name = Name
End Property

Private Sub ISwitch_SetupDialog()
    SetupDialog
End Sub

Private Sub ISwitch_SetSwitch(ByVal id As Integer, ByVal val As Boolean)
    Call SetSwitch(id, val)
End Sub

Private Sub ISwitch_SetSwitchName(ByVal id As Integer, ByVal val As String)
    Call SetSwitchName(id, val)
End Sub

