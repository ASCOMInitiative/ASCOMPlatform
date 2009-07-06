Attribute VB_Name = "Startup"
'   ===========
'   STARTUP.BAS
'   ===========
'
' ASCOM switch simulator main startup module
'
' Initial code by Jon Brewster in Feb 2007
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 29-Jan-07 jab     Initial edit
' 02-Jun-07 jab     Added naming lables to the switches
' -----------------------------------------------------------------------------

Option Explicit

Public Const ALERT_TITLE As String = "ASCOM Switch Simulator"

' ---------------------
' Simulation Parameters
' ---------------------

Public Const NUM_SWITCHES As Integer = 9    ' 0-8 as available switch "slots"

'
' ASCOM Identifiers
'
Public Const ID As String = "SwitchSim.Switch"
Public Const DESC As String = "Switch Simulator"
Private Const RegVer As String = "1.2"

' ---------
' Variables
' ---------

Public g_Profile As DriverHelper.Profile
Public g_bRunExecutable As Boolean
Public g_iConnections As Integer

' ---------------
' State Variables
' ---------------

Public g_bConnected As Boolean  ' Focuser is connected
Public g_iMaxSwitch As Integer  ' Maximum valid switch number
Public g_bZero As Boolean       ' Include switch zero

Public g_bCanGetSwitch(NUM_SWITCHES) As Boolean    ' allowed to "get" array
Public g_bCanSetSwitch(NUM_SWITCHES) As Boolean    ' allowed to "set" array

Public g_bSwitchState(NUM_SWITCHES) As Boolean     ' actual switch state array
Public g_sSwitchName(NUM_SWITCHES) As String       ' switch name array

' ----------------------
' Other global variables
' ----------------------

Public g_handBox As frmHandBox             ' Hand box
Public g_show As frmShow                   ' Traffic window

'---------------------------------------------------------------------
'
' Main() - Simulator main entry point
'
'---------------------------------------------------------------------

Sub Main()
    Dim i As Integer
    Dim buf As String
    Dim oldRegVer As Double
  
    ' only run one copy at a time
    If App.PrevInstance Then
       buf = App.Title
       App.Title = "... duplicate instance."
       On Error Resume Next
       AppActivate buf
       On Error GoTo 0
       End
    End If
    
    Set g_handBox = New frmHandBox
    Set g_Profile = New DriverHelper.Profile
    g_Profile.DeviceType = "Switch"          ' We're a Switch driver
    
    g_Profile.Register ID, DESC ' Self reg (skips if already reg)
    
    ' check if all we're doing is registering Pipe with ASCOM
    If InStr(Command$, "-r") >= 1 Then
        Exit Sub
    End If
    
    If App.StartMode = vbSModeStandalone Then
        g_bRunExecutable = True                     ' launched via double click
    Else
        g_bRunExecutable = False                    ' running as server only
    End If
    
    '
    ' initialize variables that are not persistent
    '
    g_iConnections = 0                              ' zero connections currently
    g_bConnected = False       ' Start unconnected
    
    ' get the registry format version so we can only update whats changed
    oldRegVer = 0#
    On Error Resume Next
    oldRegVer = CDbl(g_Profile.GetValue(ID, "RegVer"))
    On Error GoTo 0
    
    ' Persistent settings - Create on first start, or update
    If oldRegVer < 1.1 Then
        
        g_Profile.WriteValue ID, "MaxSwitch", CStr(8)
        g_Profile.WriteValue ID, "Zero", "False"
        
        ' enable all other switches
        For i = 0 To (NUM_SWITCHES - 1)
            g_Profile.WriteValue ID, CStr(i), "True", "CanGetSwitch"
            g_Profile.WriteValue ID, CStr(i), "True", "CanSetSwitch"
            g_Profile.WriteValue ID, CStr(i), "True", "SwitchState"
        Next i

        g_Profile.WriteValue ID, "Left", "100"
        g_Profile.WriteValue ID, "Top", "100"
    End If
    
    If oldRegVer < 1.2 Then
        
        ' initialize names
        For i = 0 To (NUM_SWITCHES - 1)
            g_Profile.WriteValue ID, CStr(i), CStr(i), "SwitchName"
        Next i
    End If
    
    g_Profile.WriteValue ID, "RegVer", RegVer
    
    g_iMaxSwitch = CInt(g_Profile.GetValue(ID, "MaxSwitch"))
    g_bZero = CBool(g_Profile.GetValue(ID, "Zero"))
    
    For i = 0 To (NUM_SWITCHES - 1)
        g_bCanGetSwitch(i) = CBool(g_Profile.GetValue(ID, CStr(i), "CanGetSwitch"))
        g_bCanSetSwitch(i) = CBool(g_Profile.GetValue(ID, CStr(i), "CanSetSwitch"))
        g_bSwitchState(i) = CBool(g_Profile.GetValue(ID, CStr(i), "SwitchState"))
        g_sSwitchName(i) = g_Profile.GetValue(ID, CStr(i), "SwitchName")
    Next i

    Load frmSetup
    Load g_handBox
    
    g_handBox.Left = CLng(g_Profile.GetValue(ID, "Left")) * Screen.TwipsPerPixelX
    g_handBox.Top = CLng(g_Profile.GetValue(ID, "Top")) * Screen.TwipsPerPixelY
    '
    ' Fix bad positions (which shouldn't ever happen, ha ha)
    '
    If g_handBox.Left < 0 Then
        g_handBox.Left = 100 * Screen.TwipsPerPixelX
        g_Profile.WriteValue ID, "Left", CStr(g_handBox.Left \ Screen.TwipsPerPixelX)
    End If
    If g_handBox.Top < 0 Then
        g_handBox.Top = 100 * Screen.TwipsPerPixelY
        g_Profile.WriteValue ID, "Top", CStr(g_handBox.Top \ Screen.TwipsPerPixelY)
    End If
    
    If g_bRunExecutable Then
        g_handBox.WindowState = vbNormal
    Else
        g_handBox.WindowState = vbMinimized
    End If
    
    g_handBox.Show
    
End Sub

'---------------------------------------------------------------------
'
' DoShutdown() - Handle handbox form Unload event
'
' Save form values for restoration on next start.
'---------------------------------------------------------------------

Sub DoShutdown()
    Dim i As Integer

    g_bConnected = False
    
    g_Profile.WriteValue ID, "MaxSwitch", CStr(g_iMaxSwitch)
    g_Profile.WriteValue ID, "Zero", CStr(g_bZero)
    
    For i = 0 To (NUM_SWITCHES - 1)
        g_Profile.WriteValue ID, CStr(i), CStr(g_bCanGetSwitch(i)), "CanGetSwitch"
        g_Profile.WriteValue ID, CStr(i), CStr(g_bCanSetSwitch(i)), "CanSetSwitch"
        g_Profile.WriteValue ID, CStr(i), CStr(g_bSwitchState(i)), "SwitchState"
        g_Profile.WriteValue ID, CStr(i), g_sSwitchName(i), "SwitchName"
    Next i
    
    g_handBox.Visible = True
    g_handBox.WindowState = vbNormal
    g_Profile.WriteValue ID, "Left", CStr(g_handBox.Left \ Screen.TwipsPerPixelX)
    g_Profile.WriteValue ID, "Top", CStr(g_handBox.Top \ Screen.TwipsPerPixelY)
        
End Sub

'---------------------------------------------------------------------
'
' DoSetup() - Handle handbox Setup button click
'
'---------------------------------------------------------------------

Sub DoSetup()
    Dim i As Integer
    
    With frmSetup
        .AllowUnload = False                        ' Assure not unloaded
        
        .cbMaxSwitch.ListIndex = g_iMaxSwitch
        .chkZero.Value = IIf(g_bZero, 1, 0)
        
        For i = 0 To (NUM_SWITCHES - 1)
            .chkGet(i) = IIf(g_bCanGetSwitch(i), 1, 0)
            .chkSet(i) = IIf(g_bCanSetSwitch(i), 1, 0)
            .txtName(i) = g_sSwitchName(i)
        Next i

    End With

    g_handBox.Visible = False                       ' May float over setup
    FloatWindow frmSetup.hwnd, True
    frmSetup.Show 1

    With frmSetup
        If .Result Then             ' Unless cancelled
        
            g_iMaxSwitch = .cbMaxSwitch.ListIndex
            g_bZero = (.chkZero.Value = 1)
            
            For i = 0 To (NUM_SWITCHES - 1)
                g_bCanGetSwitch(i) = (.chkGet(i) = 1)
                g_bCanSetSwitch(i) = (.chkSet(i) = 1)
                g_sSwitchName(i) = .txtName(i)
            Next i
                        
        End If
        .AllowUnload = True                     ' OK to unload now
    End With

    g_handBox.Visible = True
    g_handBox.Buttons

End Sub
