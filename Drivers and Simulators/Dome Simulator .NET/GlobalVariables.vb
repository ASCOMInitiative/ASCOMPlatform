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
'   GlobalVariables.vb
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
' 23-Jun-09 rbt     Ported Startub.BAS into Visual Basic .NET
' -----------------------------------------------------------------------------

Module GlobalVariables
#Region "Error Constants"
    Public Const ERR_SOURCE As String = "ASCOM Dome Simulator"

    Public Const SCODE_NOT_IMPLEMENTED As Long = vbObjectError + &H400
    Public Const MSG_NOT_IMPLEMENTED As String = _
        " is not implemented by this dome driver object."
    Public Const SCODE_DLL_LOADFAIL As Long = vbObjectError + &H401
    ' Error message for above generated at run time
    Public Const SCODE_NOT_CONNECTED As Long = vbObjectError + &H402
    Public Const MSG_NOT_CONNECTED As String = _
        "The dome is not connected"
    Public Const SCODE_PROP_NOT_SET As Long = vbObjectError + &H403
    Public Const MSG_PROP_NOT_SET As String = _
        "This property has not yet been set"
    Public Const SCODE_NO_TARGET_COORDS As Long = vbObjectError + &H404
    Public Const MSG_NO_TARGET_COORDS As String = _
        "Target coordinates have not yet been set"
    Public Const SCODE_VAL_OUTOFRANGE As Long = vbObjectError + &H405
    Public Const MSG_VAL_OUTOFRANGE As String = _
        "The property value is out of range"
    Public Const SCODE_SHUTTER_NOT_OPEN As Long = vbObjectError + &H406
    Public Const MSG_SHUTTER_NOT_OPEN As String = _
        "The shutter is not open"
    Public Const SCODE_SHUTTER_ERROR As Long = vbObjectError + &H406
    Public Const MSG_SHUTTER_ERROR As String = _
        "The shutter is in an unknown state, close it."
    Public Const SCODE_START_CONFLICT As Long = vbObjectError + &H407
    Public Const MSG_START_CONFLICT As String = _
        "Failed: Attempt to create Dome during manual startup"
#End Region


#Region "Variables"
    '----------
    ' Constants
    '----------

    Public Const ALERT_TITLE As String = "ASCOM Dome Simulator"
    Public Const INSTRUMENT_NAME As String = "Simulator"
    Public Const INSTRUMENT_DESCRIPTION As String = "ASCOM Dome Simulator"

    Public Const INVALID_COORDINATE As Double = -100000.0#

    '
    ' Timer interval (sec.)
    '
    Public Const TIMER_INTERVAL = 0.25        ' seconds per tick
    '
    ' Tolerance on Park and Home positions
    '
    Public Const PARK_HOME_TOL = 1.0#           ' Tolerance (deg) for Park/Home position
    '
    ' ASCOM Identifiers
    '
    Public Const ID As String = "ASCOM.DomeSimulator.Dome"
    Private Const DESC As String = "Dome Simulator"
    Private Const RegVer As String = "1.0"


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


    ' ---------
    ' Variables
    ' ---------
    Public g_Profile As HelperNET.Profile
    Public g_trafficDialog As ShowTrafficForm           ' Traffic window

    Public WithEvents g_timer As New HelperNET.Timer
    ' ----------------------------------------------------------
    ' Driver ID and descriptive string that shows in the Chooser
    ' ----------------------------------------------------------
    Public g_csDriverID As String = "ASCOM.DomeSimulator.Dome"
    Public g_csDriverDescription As String = "Dome Simulator"

    ' ----------------------
    ' Other global variables
    ' ----------------------

    Public g_handBox As HandboxForm              ' Hand box
    Public g_show As ShowTrafficForm                   ' Traffic window

#End Region
    ' ---------
    ' UTILITIES
    ' ---------

    ' range the azimuth parameter, full floating point (cannot use Mod)
    Public Function AzScale(ByVal Az As Double) As Double

        AzScale = Az
        Do While AzScale < 0.0#
            AzScale = AzScale + 360.0#
        Loop
        Do While AzScale >= 360.0#
            AzScale = AzScale - 360.0#
        Loop

    End Function

    Private Sub Timer_Tick() Handles g_timer.Tick

        Dim slew As Double
        Dim distance As Double

        '
        ' Handle hand-box state first
        '

        If g_handBox.BtnState <> 0 Then
            Select Case (g_handBox.BtnState)
                Case 1 ' Go clockwise
                    HW_Run(True)
                Case 2 ' step clockwise
                    HW_Move(AzScale(g_dDomeAz + g_dStepSize))
                Case 3 ' Go counter clockwise
                    HW_Run(False)
                Case 4 ' step counter clockwise
                    HW_Move(AzScale(g_dDomeAz - g_dStepSize))
                Case 5 ' shutter up
                    If g_eShutterState = ShutterState.shutterOpen Then _
                        HW_MoveShutter(g_dMaxAlt)
                Case 6 ' shutter down
                    If g_eShutterState = ShutterState.shutterOpen Then _
                        HW_MoveShutter(g_dMinAlt)
                Case 7 ' shutter open
                    If g_eShutterState = ShutterState.shutterClosed Then _
                        HW_OpenShutter()
                Case 8 ' shutter close
                    If g_eShutterState = ShutterState.shutterOpen Or _
                            g_eShutterState = ShutterState.shutterError Then _
                        HW_CloseShutter()
                Case Else ' other - halt
                    HW_Halt()
            End Select

            g_handBox.BtnState = 0
        End If

        ' Azimuth slew simulation
        If g_eSlewing <> Going.slewNowhere Then
            slew = g_dAzRate * TIMER_INTERVAL
            If g_eSlewing > Going.slewCW Then
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
            If System.Math.Abs(distance) < System.Math.Abs(slew) Then
                g_dDomeAz = g_dTargetAz
                If Not g_show Is Nothing Then
                    If g_show.chkSlew.Checked Then _
                        g_show.TrafficLine("(Slew complete)")
                End If

                ' Handle standard (fragile) and non-standard park/home changes
                If g_bStandardAtHome Then
                    If g_eSlewing = Going.slewHome Then g_bAtHome = True ' Fragile (standard)
                Else
                    g_bAtHome = HW_AtHome                               ' Position (non-standard)
                End If

                If g_bStandardAtPark Then
                    If g_eSlewing = Going.slewPark Then g_bAtPark = True ' Fragile (standard)
                Else
                    g_bAtPark = HW_AtPark                               ' Position (non-standard)
                End If

                g_eSlewing = Going.slewNowhere
            Else
                g_dDomeAz = AzScale(g_dDomeAz + slew)
            End If
        End If

        ' shutter altitude control simulation
        If (g_dDomeAlt <> g_dTargetAlt) And g_eShutterState = ShutterState.shutterOpen Then
            slew = g_dAltRate * TIMER_INTERVAL
            distance = g_dTargetAlt - g_dDomeAlt
            If distance < 0 Then _
                slew = -slew

            ' Are we there yet ?
            If System.Math.Abs(distance) < System.Math.Abs(slew) Then
                g_dDomeAlt = g_dTargetAlt
                If Not g_show Is Nothing Then
                    If g_show.chkShutter.Checked Then _
                        g_show.TrafficLine("(Shutter complete)")
                End If
            Else
                g_dDomeAlt = g_dDomeAlt + slew
            End If
        End If

        ' shutter open/close simulation
        If g_dOCProgress > 0 Then
            g_dOCProgress = g_dOCProgress - TIMER_INTERVAL
            If g_dOCProgress <= 0 Then
                If g_eShutterState = ShutterState.shutterOpening Then
                    g_eShutterState = ShutterState.shutterOpen
                Else
                    g_eShutterState = ShutterState.shutterClosed
                End If
                If Not g_show Is Nothing Then
                    If g_show.chkShutter.Checked Then _
                        g_show.TrafficLine("(Shutter complete)")
                End If
            End If
        End If

        If g_dDomeAz = INVALID_COORDINATE Then
            g_handBox.txtDomeAz.Text = "---.-"
        Else

            g_handBox.txtDomeAz.Text = Format$(AzScale(g_dDomeAz), "000.0")
        End If
        'Shutter = g_dDomeAlt
        If g_dDomeAlt = INVALID_COORDINATE Or Not g_bCanSetShutter Then
            g_handBox.txtShutter.Text = "----"
        Else
            Select Case g_eShutterState
                Case ShutterState.shutterOpen
                    If g_bCanSetAltitude Then
                        g_handBox.txtShutter.Text = Format$(g_dDomeAlt, "0.0")
                    Else
                        g_handBox.txtShutter.Text = "Open"
                    End If
                Case ShutterState.shutterClosed : g_handBox.txtShutter.Text = "Closed"
                Case ShutterState.shutterOpening : g_handBox.txtShutter.Text = "Opening"
                Case ShutterState.shutterClosing : g_handBox.txtShutter.Text = "Closing"
                Case ShutterState.shutterError : g_handBox.txtShutter.Text = "Error"
            End Select
        End If
        g_handBox.RefreshLEDs()
    End Sub
End Module
