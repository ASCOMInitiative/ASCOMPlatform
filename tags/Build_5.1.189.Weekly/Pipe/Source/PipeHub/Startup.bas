Attribute VB_Name = "Startup"
' -----------------------------------------------------------------------------
'   =============
'    STARTUP.BAS
'   =============
'
' Pipe hub main startup module
'
' Written: Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 01-Sep-06 jab     Initial edit
' 03-Jun-07 jab     Refactor Startup into separate function, called from
'                   Sub Main() and also Class Initialize().  This was required
'                   because object creation PUMPS EVENTS, allowing property
'                   and method calls to be services before initialization
'                   completes.
' 06-Jun-07 jab     Create "Hub" as simple version. Use App.Title to
'                   distinguish one Pipe from Hub (existence of GIU and
'                   setup dialog complexity)
' -----------------------------------------------------------------------------

Option Explicit

'---------------------------------------------------------------------
'
' Main() - Pipe main entry point
'
'---------------------------------------------------------------------

Sub Main()
    Dim buf As String

    ' only run one copy at a time
    If App.PrevInstance Then
       buf = App.Title
       App.Title = "... duplicate instance."
       On Error Resume Next
       AppActivate buf
       On Error GoTo 0
       End
    End If

    If App.StartMode = vbSModeStandalone Then
        DoStartupIf
    End If
    
End Sub

'---------------------------------------------------------------------
'
' DoStartupIf() - Startup code
'
' When started as an ActiveX component, this must be called from the
' Class_Initialize() function. Any CreateObject() or New for an out
' of proc compoent YIELDS. This is a disaster in Sub Main() if we
' are started via ActiveX.
'---------------------------------------------------------------------

Sub DoStartupIf()

    Dim buf As String
    
    If g_ComponentState = csRunning Then
        Exit Sub
    ElseIf g_ComponentState = csStarting Then
        Err.Raise SCODE_START_CONFLICT, App.Title, MSG_START_CONFLICT
    End If
    g_ComponentState = csStarting
    
    ' set up the who are we strings
    If App.Title = "Pipe" Then
        g_sSCOPE = PIPE_SCOPE
        g_sDOME = PIPE_DOME
        g_sFOCUSER = PIPE_FOCUSER
        g_sDESC = PIPE_DESC
    Else
        g_sSCOPE = HUB_SCOPE
        g_sDOME = HUB_DOME
        g_sFOCUSER = HUB_FOCUSER
        g_sDESC = HUB_DESC
    End If
    
    ' get access to scope persisted data
    Set g_Profile = New DriverHelper.Profile
    g_Profile.DeviceType = "Telescope"              ' We're a telescope driver
    g_Profile.Register g_sSCOPE, g_sDESC            ' Self reg (skips if already reg)
        
    ' get access to dome persisted data
    Set g_DomeProfile = New DriverHelper.Profile
    g_DomeProfile.DeviceType = "Dome"               ' and a dome driver
    g_DomeProfile.Register g_sDOME, g_sDESC         ' Self reg (skips if already reg)
    
    ' get access to focuser persisted data
    Set g_FocuserProfile = New DriverHelper.Profile
    g_FocuserProfile.DeviceType = "Focuser"         ' and a focuser driver
    g_FocuserProfile.Register g_sFOCUSER, g_sDESC   ' Self reg (skips if already reg)
    
    ' check if all we're doing is registering Pipe with ASCOM
    If InStr(Command$, "-r") >= 1 Then
        Exit Sub
    End If
    
    ' Hub must only be run as ActiveX component
    If App.StartMode = vbSModeStandalone And App.Title = "Hub" Then
        MsgBox _
        "This ASCOM driver can not be run stand alone." & vbCrLf & _
        "It is an autostart ActiveX component only.", _
        (vbOKOnly + vbExclamation + vbMsgBoxSetForeground), App.Title
        End
    End If
        
    ' get handles for main objects
    Set g_handBox = New frmHandBox
    Set g_setupDlg = New frmSetup
    Set g_Util = New DriverHelper.Util
    
    g_iConnections = 0                              ' zero scope connections currently
    g_iDomeConnections = 0                          ' zero dome connections currently
    g_iFocuserConnections = 0                       ' zero focuser connections currently
    
    g_bConnected = False
    g_bDomeConnected = False
    g_bFocuserConnected = False
    
    g_bManual = False
    g_bDomeManual = False
    g_bFocuserManual = False
    
    ' Persistent settings for the scope - Create on first start
    If g_Profile.GetValue(g_sSCOPE, "RegVer") <> RegVer Then
        g_Profile.WriteValue g_sSCOPE, "RegVer", RegVer
        g_Profile.WriteValue g_sSCOPE, "ScopeID", "ScopeSim.Telescope"
        g_Profile.WriteValue g_sSCOPE, "ScopeName", "Simulator"
 
        g_Profile.WriteValue g_sSCOPE, "Left", "100"
        g_Profile.WriteValue g_sSCOPE, "Top", "100"
        g_Profile.WriteValue g_sSCOPE, "AdvancedSetup", "False"
        g_Profile.WriteValue g_sSCOPE, "DomeMode", "False"
        g_Profile.WriteValue g_sSCOPE, "FocusMode", "False"
    End If
    
    ' if we need to, then create initial persisted state for the dome
    If g_DomeProfile.GetValue(g_sDOME, "RegVer") <> RegVer Then
        g_DomeProfile.WriteValue g_sDOME, "RegVer", RegVer
        g_DomeProfile.WriteValue g_sDOME, "DomeID", "DomeSim.Dome"
        g_DomeProfile.WriteValue g_sDOME, "DomeName", "Simulator"
    End If
    
    ' if we need to, then create initial persisted state for the focuser
    If g_FocuserProfile.GetValue(g_sFOCUSER, "RegVer") <> RegVer Then
        g_FocuserProfile.WriteValue g_sFOCUSER, "RegVer", RegVer
        g_FocuserProfile.WriteValue g_sFOCUSER, "FocuserID", "FocusSim.Focuser"
        g_FocuserProfile.WriteValue g_sFOCUSER, "FocuserName", "FocusSim.Focuser"
    End If
    
    ' find out if we're forcing classic late binding
    ' be careful, this registry entry may not exist
    If g_Profile.GetValue(g_sSCOPE, "ForceLate") = "True" Then
        g_bForceLate = True
    Else
        g_bForceLate = False
    End If
    
    ' remind ourself of the last devices used
    g_sScopeID = g_Profile.GetValue(g_sSCOPE, "ScopeID")
    g_sScopeName = g_Profile.GetValue(g_sSCOPE, "ScopeName")
    
    g_sDomeID = g_DomeProfile.GetValue(g_sDOME, "DomeID")
    g_sDomeName = g_DomeProfile.GetValue(g_sDOME, "DomeName")
    
    g_sFocuserID = g_FocuserProfile.GetValue(g_sFOCUSER, "FocuserID")
    g_sFocuserName = g_FocuserProfile.GetValue(g_sFOCUSER, "FocuserName")
    
    ' set up any gui state
    If App.Title = "Pipe" Then
        g_handBox.Left = CLng(g_Profile.GetValue(g_sSCOPE, "Left")) * Screen.TwipsPerPixelX
        g_handBox.Top = CLng(g_Profile.GetValue(g_sSCOPE, "Top")) * Screen.TwipsPerPixelY
    End If
    
    g_bSetupAdvanced = CBool(g_Profile.GetValue(g_sSCOPE, "AdvancedSetup"))
    g_bDomeMode = CBool(g_Profile.GetValue(g_sSCOPE, "DomeMode"))
    g_bFocusMode = CBool(g_Profile.GetValue(g_sSCOPE, "FocusMode"))

    ' get the dialogs created
    Load g_setupDlg
    Load g_handBox
    
    If App.Title = "Pipe" Then
         ' Fix bad positions (which shouldn't ever happen, ha ha)
        If g_handBox.Left < 0 Then
            g_handBox.Left = 100 * Screen.TwipsPerPixelX
            g_Profile.WriteValue g_sSCOPE, "Left", Str(g_handBox.Left \ Screen.TwipsPerPixelX)
        End If
        If g_handBox.Top < 0 Then
            g_handBox.Top = 100 * Screen.TwipsPerPixelY
            g_Profile.WriteValue g_sSCOPE, "Top", Str(g_handBox.Top \ Screen.TwipsPerPixelY)
        End If
        
        ' place handset in ASCOM defined mode (servers are minimized)
        If App.StartMode = vbSModeStandalone Then
            g_handBox.WindowState = vbNormal
        Else
            g_handBox.WindowState = vbMinimized
        End If
                
        ' we're open for business
        g_handBox.Show 0
    End If

    g_ComponentState = csRunning
    
End Sub

'---------------------------------------------------------------------
'
' DoShutdown - Handle handbox form Unload event
'
' Also saves state and closes the telescope and dome connections.
'---------------------------------------------------------------------

Sub DoShutdown()

    On Error Resume Next
    
    ' dump the setup dialog 1st
    g_setupDlg.AllowUnload = True
    Unload g_setupDlg
    Set g_setupDlg = Nothing
    
    ' take care of scope, dome and focuser
    ScopeSave
    DomeSave
    FocuserSave
    ScopeDelete
    DomeDelete
    FocuserDelete
    
    ' save GUI state
    g_Profile.WriteValue g_sSCOPE, "AdvancedSetup", CStr(g_bSetupAdvanced)
    g_Profile.WriteValue g_sSCOPE, "DomeMode", CStr(g_bDomeMode)
    g_Profile.WriteValue g_sSCOPE, "FocusMode", CStr(g_bFocusMode)
    
    If App.Title = "Pipe" Then
        ' save windowing state
        g_handBox.Visible = True
        g_handBox.WindowState = vbNormal
        g_Profile.WriteValue g_sSCOPE, "Left", Str(g_handBox.Left \ Screen.TwipsPerPixelX)
        g_Profile.WriteValue g_sSCOPE, "Top", Str(g_handBox.Top \ Screen.TwipsPerPixelY)
    End If
    
    Set g_Profile = Nothing
    Set g_DomeProfile = Nothing
    Set g_FocuserProfile = Nothing
    
    On Error GoTo 0
     
End Sub

'---------------------------------------------------------------------
'
' DoSetup() - Handle Setup dialog
'
'---------------------------------------------------------------------

Sub DoSetup()
    
    On Error Resume Next
    
    ' refresh GUI modes
    g_setupDlg.AdvancedMode = g_bSetupAdvanced
    g_setupDlg.DomeMode = g_bDomeMode
    g_setupDlg.FocusMode = g_bFocusMode

    ' bounce the windows around
    If App.Title = "Pipe" Then _
        g_handBox.Visible = False                   ' May float over setup
    g_setupDlg.AllowUnload = False
    FloatWindow g_setupDlg.hwnd, True
    
    g_setupDlg.Show 1                               ' model (waits for dismiss)
    
    If g_setupDlg Is Nothing Then
        Exit Sub                                    ' Pipe killed while setup up
    End If
    g_setupDlg.AllowUnload = True
    
    If App.Title = "Pipe" Then _
        g_handBox.Visible = True
    
    On Error GoTo 0

End Sub


' =============
'   Utilites
' =============

' convert to sexagescimal string
Public Function FmtSexa(ByVal n As Double, ShowPlus As Boolean) As String
    Dim sg As String
    Dim us As String, ms As String, ss As String
    Dim u As Integer, m As Integer
    Dim fmt

    sg = "+"                                ' Assume positive
    If n < 0 Then                           ' Check neg.
        n = -n                              ' Make pos.
        sg = "-"                            ' Remember sign
    End If

    m = Fix(n)                              ' Units (deg or hr)
    us = Format$(m, "00")

    n = (n - m) * 60#
    m = Fix(n)                              ' Minutes
    ms = Format$(m, "00")

    n = (n - m) * 60#
    m = Fix(n)                              ' Minutes
    ss = Format$(m, "00")

    FmtSexa = us & ":" & ms & ":" & ss
    If ShowPlus Or (sg = "-") Then FmtSexa = sg & FmtSexa
    
End Function

