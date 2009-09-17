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
' ASCOM Dome simulator main startup module
'
' Written:  20-Jun-03   Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 20-Jun-03 jab     Initial edit
' 27-Jun-03 jab     Initial release
' 03-Sep-03 jab     Additional checks for home/park positions
' 31-Jan-04 jab     Treat home/park as state, not position
' 03-Dec-04 rbd     4.0.2 - Add "Start up with shutter error" mode, dome
'                   azimuth,altitude, and shutter state are now persistent.
' 06-Dec-04 rbd     4.0.2 - More non-standard behavior - AtHome/AtPark by
'                   position, Slewing = True while opening/closing shutter.
'                   Do HW_INIT() whether or not started as an exe. Fix AzScale
'                   so that it doesn't round to the nearest degree. The Mod
'                   operator rounds...
' 12-Apr-07 rbd     5.0.1 - Refactor startup into separate function, called
'                   from Sub Main() and also Telescope.Class_Initialize().
'                   This was required because object creation PUMPS EVENTS,
'                   allowing Telescope property and method calls to be serviced
'                   before initialization completes. Remove g_bRunExecutable
'                   and always use App.StartMode directly. Add one-instance
'                   lock.
' 02-Jun-07 jab     Fixed bug wherein is program shutdown while shutter
'                   was opening or closing, then that state would never
'                   clear on restart
' 02-Jun-07 jab     Shutter ranging on Setup Dialog close did not check for
'                   shutter being open - caused slewing indicator to turn on
' 12-Jun-07 jab     Separated connect state out so GUI is always valid and
'                   wake up NOT connected.
' -----------------------------------------------------------------------------

Option Explicit

Enum ComponentStates
    csLoaded = 0
    csStarting = 1
    csRunning = 2
End Enum

'----------
' Constants
'----------

Public Const ALERT_TITLE As String = "ASCOM Dome Simulator"
Public Const INSTRUMENT_NAME As String = "Simulator"
Public Const INSTRUMENT_DESCRIPTION As String = "ASCOM Dome Simulator"
    
Public Const INVALID_COORDINATE As Double = -100000#

'
' Timer interval (sec.)
'
Public Const TIMER_INTERVAL = 0.25        ' seconds per tick
'
' Tolerance on Park and Home positions
'
Public Const PARK_HOME_TOL = 1#           ' Tolerance (deg) for Park/Home position
'
' ASCOM Identifiers
'
Public Const ID As String = "DomeSim.Dome"
Private Const DESC As String = "Dome Simulator"
Private Const RegVer As String = "1.0"

' ---------
' Variables
' ---------
Public g_ComponentState As ComponentStates
Public g_Profile As DriverHelper.Profile
Public g_Chooser As DriverHelper.Chooser
Public g_iConnections As Integer

' ---------------
' State Variables
' ---------------
Public g_dAltRate As Double                 ' degrees per sec
Public g_dAzRate As Double                  ' degrees per sec
Public g_dStepSize As Double                ' degrees per GUI step
Public g_dDomeAlt As Double                 ' Current Alt for Dome
Public g_dDomeAz As Double                  ' Current Az for Dome
Public g_dMinAlt As Double                  ' degrees altitude limit
Public g_dMaxAlt As Double                  ' degrees altitude limit
' Non-standard behaviors
Public g_bStartShutterError As Boolean      ' Start up in "shutter error" condition
Public g_bStandardAtHome As Boolean         ' False (non-std) means AtHome true whenever az = home
Public g_bStandardAtPark As Boolean         ' False (non-std) means AtPark true whenever az = home
Public g_bSlewingOpenClose As Boolean       ' Slewing true when shutter opening/closing

Public g_dSetPark As Double                 ' Park position
Public g_dSetHome As Double                 ' Home position
Public g_dTargetAlt As Double               ' Target Alt
Public g_dTargetAz As Double                ' Target Az
Public g_dOCDelay As Double                 ' Target Az
Public g_dOCProgress As Double              ' Target Az

Public g_bConnected As Boolean              ' Whether dome is connected
Public g_bAtHome As Boolean                 ' Home state
Public g_bAtPark As Boolean                 ' Park state
Public g_eShutterState As ShutterState      ' shutter status
Public g_eSlewing As Going                  ' Move in progress

'
' Dome Capabilities
'
Public g_bCanFindHome As Boolean
Public g_bCanPark As Boolean
Public g_bCanSetAltitude As Boolean
Public g_bCanSetAzimuth As Boolean
Public g_bCanSetPark As Boolean
Public g_bCanSetShutter As Boolean
Public g_bCanSyncAzimuth As Boolean

' ----------------------
' Other global variables
' ----------------------

Public g_handBox As frmHandBox              ' Hand box
Public g_show As frmShow                    ' Traffic window
Public g_timer As VB.timer                  ' Handy reference to timer

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
    Set g_timer = g_handBox.timer
    Set g_Profile = New DriverHelper.Profile
    
    g_Profile.DeviceType = "Dome"                   ' We're a Dome driver
    g_Profile.Register ID, DESC                     ' Self reg (skips if already reg)
    
    g_iConnections = 0                              ' zero connections currently
    g_bConnected = False                            ' Not yet connected

    g_dOCProgress = 0
    g_dOCDelay = 0
    
    '
    ' Persistent settings - Create on first start
    '
    If g_Profile.GetValue(ID, "RegVer") <> RegVer Then
        g_Profile.WriteValue ID, "RegVer", RegVer
        
        g_Profile.WriteValue ID, "OCDelay", "3"
        g_Profile.WriteValue ID, "SetPark", "180"
        g_Profile.WriteValue ID, "SetHome", "0"
        g_Profile.WriteValue ID, "AltRate", "30"
        g_Profile.WriteValue ID, "AzRate", "15"
        g_Profile.WriteValue ID, "StepSize", "5"
        g_Profile.WriteValue ID, "MaxAlt", "90"
        g_Profile.WriteValue ID, "MinAlt", "0"
        g_Profile.WriteValue ID, "StartShutterError", "False"
        g_Profile.WriteValue ID, "SlewingOpenClose", "False"
        g_Profile.WriteValue ID, "NonFragileAtHome", "False"
        g_Profile.WriteValue ID, "NonFragileAtPark", "False"

        g_Profile.WriteValue ID, "DomeAz", CStr(INVALID_COORDINATE), "State"
        g_Profile.WriteValue ID, "DomeAlt", CStr(INVALID_COORDINATE), "State"
        g_Profile.WriteValue ID, "ShutterState", "1", "State"       ' ShutterClosed
    
        g_Profile.WriteValue ID, "Left", "100"
        g_Profile.WriteValue ID, "Top", "100"
                
        g_Profile.WriteValue ID, "CanFindHome", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanPark", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSetAltitude", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSetAzimuth", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSetPark", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSetShutter", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSyncAzimuth", "True", "Capabilities"
    End If
    
    g_dOCDelay = CDbl(g_Profile.GetValue(ID, "OCDelay"))
    g_dSetPark = CDbl(g_Profile.GetValue(ID, "SetPark"))
    g_dSetHome = CDbl(g_Profile.GetValue(ID, "SetHome"))
    g_dAltRate = CDbl(g_Profile.GetValue(ID, "AltRate"))
    g_dAzRate = CDbl(g_Profile.GetValue(ID, "AzRate"))
    g_dStepSize = CDbl(g_Profile.GetValue(ID, "StepSize"))
    g_dMaxAlt = CDbl(g_Profile.GetValue(ID, "MaxAlt"))
    g_dMinAlt = CDbl(g_Profile.GetValue(ID, "MinAlt"))
    g_bStartShutterError = CBool(g_Profile.GetValue(ID, "StartShutterError"))
    g_bSlewingOpenClose = CBool(g_Profile.GetValue(ID, "SlewingOpenClose"))
    g_bStandardAtHome = Not CBool(g_Profile.GetValue(ID, "NonFragileAtHome"))
    g_bStandardAtPark = Not CBool(g_Profile.GetValue(ID, "NonFragileAtPark"))
    
    g_bCanFindHome = CBool(g_Profile.GetValue(ID, "CanFindHome", "Capabilities"))
    g_bCanPark = CBool(g_Profile.GetValue(ID, "CanPark", "Capabilities"))
    g_bCanSetAltitude = CBool(g_Profile.GetValue(ID, "CanSetAltitude", "Capabilities"))
    g_bCanSetAzimuth = CBool(g_Profile.GetValue(ID, "CanSetAzimuth", "Capabilities"))
    g_bCanSetPark = CBool(g_Profile.GetValue(ID, "CanSetPark", "Capabilities"))
    g_bCanSetShutter = CBool(g_Profile.GetValue(ID, "CanSetShutter", "Capabilities"))
    g_bCanSyncAzimuth = CBool(g_Profile.GetValue(ID, "CanSyncAzimuth", "Capabilities"))
    
    ' get and range dome state
    g_dDomeAz = CDbl(g_Profile.GetValue(ID, "DomeAz", "State"))
    g_dDomeAlt = CDbl(g_Profile.GetValue(ID, "DomeAlt", "State"))
    If g_dDomeAlt < g_dMinAlt Then _
        g_dDomeAlt = g_dMinAlt
    If g_dDomeAlt > g_dMaxAlt Then _
        g_dDomeAlt = g_dMaxAlt
    If g_dDomeAz < 0 Or g_dDomeAz >= 360 Then _
        g_dDomeAz = g_dSetPark
    g_dTargetAlt = g_dDomeAlt
    g_dTargetAz = g_dDomeAz
    
    If g_bStartShutterError Then
        g_eShutterState = shutterError
    Else
        g_eShutterState = CDbl(g_Profile.GetValue(ID, "ShutterState", "State"))
    End If
    
    g_eSlewing = slewNowhere
    g_bAtPark = HW_AtPark                   ' its OK to wake up parked
    If g_bStandardAtHome Then
        g_bAtHome = False                   ' Standard: home is set by home() method, never wake up homed!
    Else
        g_bAtHome = HW_AtHome               ' Non standard, position, ok to wake up homed
    End If

    Load frmSetup
    Load g_handBox
    
    With g_handBox
        .DomeAz = g_dDomeAz
        .Shutter = g_dDomeAlt
        .LabelButtons
        .RefreshLEDs
        .Left = CLng(g_Profile.GetValue(ID, "Left")) * Screen.TwipsPerPixelX
        .Top = CLng(g_Profile.GetValue(ID, "Top")) * Screen.TwipsPerPixelY
        
        If .Left < 0 Then _
            .Left = 0
        If .Top < 0 Then _
            .Top = 0
    End With
        
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
'---------------------------------------------------------------------
Sub DoShutdown()

    g_timer.Enabled = False
    
    g_bConnected = False
    g_eSlewing = slewNowhere
    If g_eShutterState = shutterClosing Then _
        g_eShutterState = shutterClosed
    If g_eShutterState = shutterOpening Then _
        g_eShutterState = shutterOpen
    
    g_Profile.WriteValue ID, "OCDelay", CStr(g_dOCDelay)
    g_Profile.WriteValue ID, "SetPark", CStr(g_dSetPark)
    g_Profile.WriteValue ID, "SetHome", CStr(g_dSetHome)
    g_Profile.WriteValue ID, "AltRate", CStr(g_dAltRate)
    g_Profile.WriteValue ID, "AzRate", CStr(g_dAzRate)
    g_Profile.WriteValue ID, "StepSize", CStr(g_dStepSize)
    g_Profile.WriteValue ID, "MaxAlt", CStr(g_dMaxAlt)
    g_Profile.WriteValue ID, "MinAlt", CStr(g_dMinAlt)
    g_Profile.WriteValue ID, "StartShutterError", CStr(g_bStartShutterError)
    g_Profile.WriteValue ID, "SlewingOpenClose", CStr(g_bSlewingOpenClose)
    g_Profile.WriteValue ID, "NonFragileAtHome", CStr(Not g_bStandardAtHome)
    g_Profile.WriteValue ID, "NonFragileAtPark", CStr(Not g_bStandardAtPark)
    
    g_Profile.WriteValue ID, "DomeAz", CStr(g_dDomeAz), "State"
    g_Profile.WriteValue ID, "DomeAlt", CStr(g_dDomeAlt), "State"
    
    g_Profile.WriteValue ID, "ShutterState", CStr(g_eShutterState), "State"
    
    g_Profile.WriteValue ID, "CanFindHome", CStr(g_bCanFindHome), "Capabilities"
    g_Profile.WriteValue ID, "CanPark", CStr(g_bCanPark), "Capabilities"
    g_Profile.WriteValue ID, "CanSetAltitude", CStr(g_bCanSetAltitude), "Capabilities"
    g_Profile.WriteValue ID, "CanSetAzimuth", CStr(g_bCanSetAzimuth), "Capabilities"
    g_Profile.WriteValue ID, "CanSetPark", CStr(g_bCanSetPark), "Capabilities"
    g_Profile.WriteValue ID, "CanSetShutter", CStr(g_bCanSetShutter), "Capabilities"
    g_Profile.WriteValue ID, "CanSyncAzimuth", CStr(g_bCanSyncAzimuth), "Capabilities"
  
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
    Dim ans As Boolean
    
    With frmSetup
        .AllowUnload = False                        ' Assure not unloaded
        .OCDelay = g_dOCDelay
        .AltRate = g_dAltRate
        .AzRate = g_dAzRate
        .StepSize = g_dStepSize
        .MaxAlt = g_dMaxAlt
        .MinAlt = g_dMinAlt
        .Park = g_dSetPark
        .Home = g_dSetHome
        .chkStartShutterError.Value = IIf(g_bStartShutterError, 1, 0)
        .chkSlewingOpenClose.Value = IIf(g_bSlewingOpenClose, 1, 0)
        .chkNonFragileAtHome.Value = IIf(g_bStandardAtHome, 0, 1)   ' Reversed
        .chkNonFragileAtPark.Value = IIf(g_bStandardAtPark, 0, 1)   ' Reversed
        
        .chkCanFindHome.Value = IIf(g_bCanFindHome, 1, 0)
        .chkCanPark.Value = IIf(g_bCanPark, 1, 0)
        .chkCanSetAltitude.Value = IIf(g_bCanSetAltitude, 1, 0)
        .chkCanSetAzimuth.Value = IIf(g_bCanSetAzimuth, 1, 0)
        .chkCanSetShutter.Value = IIf(g_bCanSetShutter, 1, 0)
        .chkCanSetPark.Value = IIf(g_bCanSetPark, 1, 0)
        .chkCanSyncAzimuth.Value = IIf(g_bCanSyncAzimuth, 1, 0)
    End With
    
    g_handBox.Visible = False                       ' May float over setup
    FloatWindow frmSetup.hwnd, True
    frmSetup.Show 1
    
    With frmSetup
        If .Result Then             ' Unless cancelled
            g_dOCDelay = .OCDelay
            g_dAltRate = .AltRate
            g_dAzRate = .AzRate
            g_dStepSize = .StepSize
            g_dMaxAlt = .MaxAlt
            g_dMinAlt = .MinAlt
            g_dSetPark = .Park
            g_dSetHome = .Home
            g_bStartShutterError = (.chkStartShutterError.Value = 1)
            g_bSlewingOpenClose = (.chkSlewingOpenClose.Value = 1)
            g_bStandardAtHome = (.chkNonFragileAtHome.Value = 0)    ' Reversed
            g_bStandardAtPark = (.chkNonFragileAtPark.Value = 0)    ' Reversed
            
            g_bCanFindHome = (.chkCanFindHome.Value = 1)
            g_bCanPark = (.chkCanPark.Value = 1)
            g_bCanSetAltitude = (.chkCanSetAltitude.Value = 1)
            g_bCanSetAzimuth = (.chkCanSetAzimuth.Value = 1)
            g_bCanSetShutter = (.chkCanSetShutter.Value = 1)
            g_bCanSetPark = (.chkCanSetPark.Value = 1)
            g_bCanSyncAzimuth = (.chkCanSyncAzimuth.Value = 1)
        End If
        
        .AllowUnload = True                     ' OK to unload now
    End With
    
    ' range the shutter
    If g_eShutterState = shutterOpen Then
        If g_dMinAlt > g_dDomeAlt Then HW_MoveShutter g_dMinAlt
        If g_dMaxAlt < g_dDomeAlt Then HW_MoveShutter g_dMaxAlt
    End If
       
    ' check for home / park changes (standard: state is fragile, can override with position semantics)
    If g_bStandardAtHome Then
        If g_dDomeAz <> g_dSetHome Then g_bAtHome = False       ' Fragile (standard)
    Else
        g_bAtHome = HW_AtHome                                   ' Position (non-standard)
    End If
    
    If g_bStandardAtPark Then
        If g_dDomeAz <> g_dSetPark Then g_bAtPark = False       ' Fragile (standard)
    Else
        g_bAtPark = HW_AtPark                                   ' Position (non-standard)
    End If
    
    ' update handbox displays
    With g_handBox
        .DomeAz = g_dDomeAz
        .LabelButtons
        .RefreshLEDs
    End With
    
    g_handBox.Visible = True
    
End Sub

'---------------------------------------------------------------------
'
' timer_tick() - Called by timer
'
' Implements slewing and handbox control.
'---------------------------------------------------------------------
Sub timer_tick()
    Dim Button As Integer
    Dim slew As Double
    Dim distance As Double
        
    '
    ' Handle hand-box state first
    '
    Button = g_handBox.ButtonState
    If Button <> 0 Then
        Select Case (Button)
            Case 1: ' Go clockwise
                HW_Run True
            Case 2: ' step clockwise
                HW_Move AzScale(g_dDomeAz + g_dStepSize)
            Case 3: ' Go counter clockwise
                HW_Run False
            Case 4: ' step counter clockwise
                HW_Move AzScale(g_dDomeAz - g_dStepSize)
            Case 5: ' shutter up
                If g_eShutterState = shutterOpen Then _
                    HW_MoveShutter g_dMaxAlt
            Case 6: ' shutter down
                If g_eShutterState = shutterOpen Then _
                    HW_MoveShutter g_dMinAlt
            Case 7: ' shutter open
                If g_eShutterState = shutterClosed Then _
                    HW_OpenShutter
            Case 8: ' shutter close
                If g_eShutterState = shutterOpen Or _
                        g_eShutterState = shutterError Then _
                    HW_CloseShutter
            Case Else: ' other - halt
                HW_Halt
        End Select
    End If
   
    ' Azimuth slew simulation
    If g_eSlewing <> slewNowhere Then
        slew = g_dAzRate * TIMER_INTERVAL
        If g_eSlewing > slewCW Then
            distance = g_dTargetAz - g_dDomeAz
            If distance < 0 Then _
                slew = -slew
            If distance > 180 Then _
                slew = -slew
            If distance < -180 Then _
                slew = -slew
        Else
            distance = slew * 2
            slew = slew * g_eSlewing
        End If
        
        ' Are we there yet ?
        If Abs(distance) < Abs(slew) Then
            g_dDomeAz = g_dTargetAz
            If Not g_show Is Nothing Then
                If g_show.chkSlew.Value = 1 Then _
                    g_show.TrafficLine "(Slew complete)"
            End If

            ' Handle standard (fragile) and non-standard park/home changes
            If g_bStandardAtHome Then
                If g_eSlewing = slewHome Then g_bAtHome = True      ' Fragile (standard)
            Else
                g_bAtHome = HW_AtHome                               ' Position (non-standard)
            End If

            If g_bStandardAtPark Then
                If g_eSlewing = slewPark Then g_bAtPark = True      ' Fragile (standard)
            Else
                g_bAtPark = HW_AtPark                               ' Position (non-standard)
            End If
            
            g_eSlewing = slewNowhere
        Else
            g_dDomeAz = AzScale(g_dDomeAz + slew)
        End If
    End If
  
    ' shutter altitude control simulation
    If (g_dDomeAlt <> g_dTargetAlt) And g_eShutterState = shutterOpen Then
        slew = g_dAltRate * TIMER_INTERVAL
        distance = g_dTargetAlt - g_dDomeAlt
        If distance < 0 Then _
            slew = -slew
        
        ' Are we there yet ?
        If Abs(distance) < Abs(slew) Then
            g_dDomeAlt = g_dTargetAlt
            If Not g_show Is Nothing Then
                If g_show.chkShutter.Value = 1 Then _
                    g_show.TrafficLine "(Shutter complete)"
            End If
        Else
            g_dDomeAlt = g_dDomeAlt + slew
        End If
    End If

    ' shutter open/close simulation
    If g_dOCProgress > 0 Then
        g_dOCProgress = g_dOCProgress - TIMER_INTERVAL
        If g_dOCProgress <= 0 Then
            If g_eShutterState = shutterOpening Then
                g_eShutterState = shutterOpen
            Else
                g_eShutterState = shutterClosed
            End If
            If Not g_show Is Nothing Then
                If g_show.chkShutter.Value = 1 Then _
                    g_show.TrafficLine "(Shutter complete)"
            End If
        End If
    End If
    
    g_handBox.DomeAz = g_dDomeAz
    g_handBox.Shutter = g_dDomeAlt
    g_handBox.RefreshLEDs
    
End Sub

' ---------
' UTILITIES
' ---------

' range the azimuth parameter, full floating point (cannot use Mod)
Public Function AzScale(Az As Double) As Double
    
    AzScale = Az
    Do While AzScale < 0#
        AzScale = AzScale + 360#
    Loop
    Do While AzScale >= 360#
        AzScale = AzScale - 360#
    Loop
    
End Function
