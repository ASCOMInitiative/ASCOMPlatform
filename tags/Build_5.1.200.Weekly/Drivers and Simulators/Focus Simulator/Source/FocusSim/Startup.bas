Attribute VB_Name = "Startup"
'---------------------------------------------------------------------
' Copyright © 2000-2002 SPACE.com Inc., New York, NY
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". SPACE.COM, INC. MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'   ============
'   STARTUP.BAS
'   ============
'
' ASCOM focuser simulator main startup module
'
' From Scope Simulator written 27-Jun-00   Robert B. Denny <rdenny@dc3.com>
' Structure retained, otherwise rewriten into Focus Simulator
' by Jon Brewster in Feb 2003
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 24-Feb-03 jab     Initial edit - Starting from Telescope Simulator
' 28-Feb-03 jab     completed addition of temperature simulation
' 01-mar-03 jab     reworked button / timer interaction for race conditions
' 06-mar-03 jab     added registry versioning
' 15-Mar-03 jab     kill server if launched via client and last client bails,
'                   also only allow disconnect if last clients commands it,
'                   no disconnects allowed if launched manually, start
'                   minimized if client launched
' 16-Mar-03 jab     relative focuser is no longer clipped by 0 and MaxStep
' 28-Mar-03 jab     check for negative window start positions
' 12-Apr-07 rbd     5.0.1 - Refactor startup into separate function, called
'                   from Sub Main() and also Telescope.Class_Initialize().
'                   This was required because object creation PUMPS EVENTS,
'                   allowing Telescope property and method calls to be serviced
'                   before initialization completes. Remove g_bRunExecutable
'                   and always use App.StartMode directly. Add one-instance
'                   lock.
' -----------------------------------------------------------------------------
Option Explicit

Public Const ALERT_TITLE As String = "ASCOM Focuser Simulator"

Enum ComponentStates
    csLoaded = 0
    csStarting = 1
    csRunning = 2
End Enum

' ---------------------
' Simulation Parameters
' ---------------------

'
' Timer interval (sec.)
'
Public Const TIMER_INTERVAL = 0.25      ' 4 tix/sec
Private g_iTimerCount As Integer
'
' ASCOM Identifiers
'
Public Const ID As String = "FocusSim.Focuser"
Private Const DESC As String = "Focuser Simulator"
Private Const RegVer As String = "1.1"

' ---------
' Variables
' ---------
Public g_ComponentState As ComponentStates
Public g_Profile As DriverHelper.Profile
Public g_iConnections As Integer

' ---------------
' State Variables
' ---------------

Public g_bAbsolute As Boolean   ' Absolute or relative
Public g_bSynchronous As Boolean   ' wait during move
Public g_bLinked As Boolean     ' Focuser is connected
Public g_lMaxInc As Long        ' Maximum delta position
Public g_lMaxStep As Long       ' Maximum position
Public g_lPosition As Long      ' Current position
Public g_dStepSize As Double    ' Step size in microns
Public g_bTempComp As Boolean   ' Is temperature compensation on
Public g_dTemp As Double        ' Focuser probe temperature
Public g_dTempMax As Double     ' maximum temperature
Public g_dTempMin As Double     ' minimum temperature
Public g_dTempDelta As Double   ' amount of temp shift per second
Public g_lTempPeriod As Long    ' seconds for full temp cycle
Public g_lTempSteps As Long     ' steps per degree of compensation
Public g_lTPos As Long          ' position at start of temp comp
Public g_dTTemp As Double       ' temperature at start of temp comp
'
' Interface Capabilities (setup checkboxes)
'
Public g_bCanTempComp As Boolean    ' Are we simulating comensation
Public g_bCanTemp As Boolean        ' Are we simulating a probe
Public g_bCanStepSize As Boolean    ' Step size is returnable
Public g_bCanHalt As Boolean        ' Movement is stopable
'
' Move simulation
'
Public g_bMoving As Boolean       ' Whether focuser is moving
Public g_lDeltaMove As Long       ' Distance remaining to move
Public g_lMoveRate As Long        ' Delta steps/timer tick

' ----------------------
' Other global variables
' ----------------------

Public g_handBox As frmHandBox             ' Hand box
Public g_show As frmShow                   ' Traffic window
Public g_timer As VB.Timer                 ' Handy reference to timer
Public g_bAlwaysTop As Boolean             ' True to keep sim topmost window
Public g_bSetupAdvanced As Boolean         ' True to display advanced options

'---------------------------------------------------------------------
'
' Main() - Simulator main entry point
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

    If g_ComponentState = csRunning Then
        Exit Sub
    ElseIf g_ComponentState = csStarting Then
        Err.Raise SCODE_START_CONFLICT, ERR_SOURCE, MSG_START_CONFLICT
    End If
    g_ComponentState = csStarting
    
    Set g_handBox = New frmHandBox
    Set g_timer = g_handBox.Timer
    Set g_Profile = New DriverHelper.Profile
    g_Profile.DeviceType = "Focuser"            ' We're a Focuser driver
    g_Profile.Register ID, DESC                 ' Self reg (skips if already reg)
      
    g_iConnections = 0                          ' zero connections currently
    
    '
    ' initialize variables that are not persistent
    '
    g_bLinked = False       ' Start unlinked
    g_bMoving = False       ' Start unmoving
    g_bTempComp = False     ' Start with temperature compesation off
    g_lDeltaMove = 0        ' Amount to move 0 since not moving
    g_lMoveRate = 500       ' set steps per tick
    g_iTimerCount = 0

    '
    ' Persistent settings - Create on first start as determined by
    ' existence of most recently added setting
    '
    If g_Profile.GetValue(ID, "RegVer") <> RegVer Then
        g_Profile.WriteValue ID, "RegVer", RegVer
        g_Profile.WriteValue ID, "Absolute", "True"
        g_Profile.WriteValue ID, "Synchronous", "False"
        g_Profile.WriteValue ID, "MaxStep", "50000"
        g_Profile.WriteValue ID, "Position", "25000"
        g_Profile.WriteValue ID, "StepSize", "20.0"
        g_Profile.WriteValue ID, "Temp", "5.0"
        g_Profile.WriteValue ID, "MaxInc", "50000"
        g_Profile.WriteValue ID, "AlwaysOnTop", "True"
        g_Profile.WriteValue ID, "Left", "100"
        g_Profile.WriteValue ID, "Top", "100"
        g_Profile.WriteValue ID, "AdvancedSetup", "False"
        g_Profile.WriteValue ID, "CanTempComp", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanTemp", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanStepSize", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanHalt", "True", "Capabilities"
        g_Profile.WriteValue ID, "TempMax", "5.0", "Capabilities"
        g_Profile.WriteValue ID, "TempMin", "-5.0", "Capabilities"
        g_Profile.WriteValue ID, "TempPeriod", "500", "Capabilities"
        g_Profile.WriteValue ID, "TempSteps", "10", "Capabilities"
    End If
    
    g_bAbsolute = CBool(g_Profile.GetValue(ID, "Absolute"))
    g_bSynchronous = CBool(g_Profile.GetValue(ID, "Synchronous"))
    g_lMaxStep = CLng(g_Profile.GetValue(ID, "MaxStep"))
    g_lPosition = CLng(g_Profile.GetValue(ID, "Position"))
    g_dStepSize = CDbl(g_Profile.GetValue(ID, "StepSize"))
    g_dTemp = CDbl(g_Profile.GetValue(ID, "Temp"))
    g_lMaxInc = CLng(g_Profile.GetValue(ID, "MaxInc"))
    g_bAlwaysTop = CBool(g_Profile.GetValue(ID, "AlwaysOnTop"))
    g_bSetupAdvanced = CBool(g_Profile.GetValue(ID, "AdvancedSetup"))
    g_bCanTempComp = CBool(g_Profile.GetValue(ID, "CanTempComp", "Capabilities"))
    g_bCanTemp = CBool(g_Profile.GetValue(ID, "CanTemp", "Capabilities"))
    g_bCanStepSize = CBool(g_Profile.GetValue(ID, "CanStepSize", "Capabilities"))
    g_bCanHalt = CBool(g_Profile.GetValue(ID, "CanHalt", "Capabilities"))
    g_dTempMax = CDbl(g_Profile.GetValue(ID, "TempMax", "Capabilities"))
    g_dTempMin = CDbl(g_Profile.GetValue(ID, "TempMin", "Capabilities"))
    g_lTempPeriod = CLng(g_Profile.GetValue(ID, "TempPeriod", "Capabilities"))
    g_lTempSteps = CLng(g_Profile.GetValue(ID, "TempSteps", "Capabilities"))

    Load frmSetup
    
    '
    ' Show the handbox now and start the timer
    '
    Load g_handBox
    
    dependencies
    
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
    
    If App.StartMode = vbSModeStandalone Then
        g_handBox.WindowState = vbNormal
    Else
        g_handBox.WindowState = vbMinimized
    End If
    
    g_handBox.Show
    
    g_timer.Interval = (TIMER_INTERVAL * 1000)
    g_timer.Enabled = True
    
    g_ComponentState = csRunning

End Sub

'---------------------------------------------------------------------
'
' DoShutdown() - Handle handbox form Unload event
'
' Save form values for restoration on next start.
' Also "closes" the focuser connection.
'---------------------------------------------------------------------

Sub DoShutdown()
    g_bMoving = False
    g_bLinked = False
    
    g_timer.Enabled = False
    
    g_Profile.WriteValue ID, "Position", CStr(g_lPosition)
    g_Profile.WriteValue ID, "Temp", CStr(g_dTemp)
    
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

    g_timer.Enabled = False                         ' don't want races
    
    With frmSetup
        .AllowUnload = False                        ' Assure not unloaded
        .AdvancedMode = g_bSetupAdvanced            ' Initialize form mode
        .MaxStep = g_lMaxStep
        .StepSize = g_dStepSize
        .MaxInc = g_lMaxInc
        .Absolute = g_bAbsolute
        .Synchronous = g_bSynchronous
        .chkAlwaysTop = IIf(g_bAlwaysTop, 1, 0)
        .chkCanTempComp.Value = IIf(g_bCanTempComp, 1, 0)
        .chkCanTemp.Value = IIf(g_bCanTemp, 1, 0)
        .chkCanStepSize.Value = IIf(g_bCanStepSize, 1, 0)
        .chkCanHalt.Value = IIf(g_bCanHalt, 1, 0)
        .TempCur = g_dTemp
        .TempMax = g_dTempMax
        .TempMin = g_dTempMin
        .TempPeriod = g_lTempPeriod
        .TempSteps = g_lTempSteps
    End With

    g_handBox.Visible = False                       ' May float over setup
    FloatWindow frmSetup.hwnd, True
    frmSetup.Show 1

    With frmSetup
        If .Result Then             ' Unless cancelled
            g_lMaxStep = .MaxStep
            g_Profile.WriteValue ID, "MaxStep", CStr(g_lMaxStep)
            g_dStepSize = .StepSize
            g_Profile.WriteValue ID, "StepSize", CStr(g_dStepSize)
            g_lMaxInc = .MaxInc
            g_Profile.WriteValue ID, "MaxInc", CStr(g_lMaxInc)
            g_bAbsolute = .Absolute
            g_Profile.WriteValue ID, "Absolute", CStr(g_bAbsolute)
            g_bSynchronous = .Synchronous
            g_Profile.WriteValue ID, "Synchronous", CStr(g_bSynchronous)
            g_bAlwaysTop = (.chkAlwaysTop = 1)
            g_Profile.WriteValue ID, "AlwaysOnTop", CStr(g_bAlwaysTop)
            g_bSetupAdvanced = .AdvancedMode
            g_Profile.WriteValue ID, "AdvancedSetup", CStr(g_bSetupAdvanced)
            g_bCanTempComp = (.chkCanTempComp.Value = 1)
            g_Profile.WriteValue ID, "CanTempComp", CStr(g_bCanTempComp), "Capabilities"
            g_bCanTemp = (.chkCanTemp.Value = 1)
            g_Profile.WriteValue ID, "CanTemp", CStr(g_bCanTemp), "Capabilities"
            g_bCanStepSize = (.chkCanStepSize.Value = 1)
            g_Profile.WriteValue ID, "CanStepSize", CStr(g_bCanStepSize), "Capabilities"
            g_bCanHalt = (.chkCanHalt.Value = 1)
            g_Profile.WriteValue ID, "CanHalt", CStr(g_bCanHalt), "Capabilities"
            g_dTempMax = .TempMax
            g_Profile.WriteValue ID, "TempMax", CStr(g_dTempMax), "Capabilities"
            g_dTempMin = .TempMin
            g_Profile.WriteValue ID, "TempMin", CStr(g_dTempMin), "Capabilities"
            g_lTempPeriod = .TempPeriod
            g_Profile.WriteValue ID, "TempPeriod", CStr(g_lTempPeriod), "Capabilities"
            g_lTempSteps = .TempSteps
            g_Profile.WriteValue ID, "TempSteps", CStr(g_lTempSteps), "Capabilities"

            g_dTemp = .TempCur
            dependencies
                        
        End If
        .AllowUnload = True                     ' OK to unload now
    End With

    FloatWindow g_handBox.hwnd, g_bAlwaysTop
    g_handBox.Visible = True
    g_timer.Enabled = True                      ' restart time

End Sub

'---------------------------------------------------------------------
'
' dependencies() - Called to make sure that the GUI bits are synced
'                  and any computational dependencies are handled
'
'---------------------------------------------------------------------

Sub dependencies()

    ' GUI consistency
    If g_bCanTempComp And g_bCanTemp Then
        If g_bTempComp Then
            g_handBox.TempComp = 1
        Else
            g_handBox.TempComp = 0
        End If
    Else
        g_bTempComp = False
        g_handBox.TempComp = 2
    End If
    
    ' calculate temp change per second
    If g_lTempPeriod = 0 Then
        g_dTempDelta = 0
    Else
        g_dTempDelta = ((g_dTempMax - g_dTempMin) * 2#) / CDbl(g_lTempPeriod)
    End If
    
    ' make sure position is in range
    If g_bAbsolute Then
        If g_lPosition < 0 Then _
            g_lPosition = 0
        If g_lPosition > g_lMaxStep Then _
            g_lPosition = g_lMaxStep
    End If

    ' update handbox
    g_handBox.Position = g_lPosition
    g_handBox.temp = g_dTemp
    
End Sub

'---------------------------------------------------------------------
'
' timer_tick() - Called when timer in frmCtrlHolder fires event
'
' Implements moving and handbox control. DEPENDS ON 250MS TIMER TICKS!!
'---------------------------------------------------------------------

Sub timer_tick()
    Dim step As Long
    Dim button As Long
    
    '
    ' hand-box buttons first
    '
    button = g_handBox.ButtonState      ' Only fetch once due to debounce
    If button <> 0 Then                 ' Handbox buttons have prio
        g_bMoving = False               ' Immediately stop moving

        Select Case button
            Case 1                                  ' In button
                g_lPosition = g_lPosition - g_lMoveRate
            Case 2                                  ' Out button
                g_lPosition = g_lPosition + g_lMoveRate
        End Select
    End If
    
    '
    ' move focuser as commanded by last programmatic call
    '
    If g_bMoving Then

'        ' Journaling for actual "movement" possible but not really helpful
'        If Not g_show Is Nothing Then
'            If g_show.chkMoving.Value = 1 Then _
'                g_show.TrafficChar " " & IIf(g_lDeltaMove < 0, "-", "+")
'        End If
        
        step = g_lMoveRate
        If step > Abs(g_lDeltaMove) Then _
            step = Abs(g_lDeltaMove)
        step = step * Sgn(g_lDeltaMove)
        
        g_lPosition = g_lPosition + step
        g_lDeltaMove = g_lDeltaMove - step
        
        If g_lDeltaMove = 0 Then
            g_bMoving = False
            
            If Not g_show Is Nothing Then
                If g_show.chkMove.Value = 1 Then _
                    g_show.TrafficLine " (move complete)"
            End If
        End If
    End If
    
    '
    ' simulate temperature change once every second
    '
    If g_bCanTemp Then
        g_iTimerCount = g_iTimerCount + 1
        If g_iTimerCount > 4 Then

            ' modify temperature
            g_dTemp = g_dTemp + g_dTempDelta
            If g_dTempDelta < 0 Then
                If g_dTemp < g_dTempMin Then
                    g_dTemp = g_dTempMin
                    g_dTempDelta = g_dTempDelta * -1
                End If
            Else
                If g_dTemp > g_dTempMax Then
                    g_dTemp = g_dTempMax
                    g_dTempDelta = g_dTempDelta * -1
                End If
            End If
        
            ' do temperature compensation
            If g_bTempComp Then
'                ' Journaling for actual tempcomp possible but not really helpful
'                If Not g_show Is Nothing Then
'                    If g_show.chkTemp.Value = 1 Then _
'                        g_show.TrafficLine "Temperture Compensation moved"
'                End If
                g_lPosition = g_lTPos + ((g_dTemp - g_dTTemp) * g_lTempSteps)
            End If
            
            ' update handbox
            g_handBox.temp = g_dTemp

            g_iTimerCount = 0
        End If
    End If

    '
    ' make sure position stays in range
    '
    If g_bAbsolute Then
        If g_lPosition > g_lMaxStep Then
            g_lPosition = g_lMaxStep
        ElseIf g_lPosition < 0 Then
            g_lPosition = 0
        End If
    End If
    
    g_handBox.Position = g_lPosition
      
End Sub
