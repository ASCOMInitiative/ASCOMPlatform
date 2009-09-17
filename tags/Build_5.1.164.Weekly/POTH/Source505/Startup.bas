Attribute VB_Name = "Startup"
' -----------------------------------------------------------------------------
'   ===========
'   STARTUP.BAS
'   ===========
'
' Plain old telescope hub main startup module
'
' Written: Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 22-Mar-03 jab     Initial edit
' 07-Sep-03 jab     Beta release - much more robust, getting ready for V2
' 25-Sep-03 jab     Pitched all the globals into Public.bas
' 25-Sep-03 jab     Finished new V2 spec support (V2 definition changed)
' 14-May-04 jab     Converted to API timer due to uSoft bug in VB timer object
' 23-Nov-04 rbd     4.0.1 - Version change for Platform 4.0
' 10-Jan-06 dpp     Focuser implementation, change strategy to read registry
' 07-Apr-06 dpp     Save in registry the Geometry
' 30-May-06 dpp     All registry parameters are now tested for errors
' 31-Aug-06 jab     Registry now checks for old versions and brings them forward
' 14-Oct-06 jab     check coordinates after PulseGuiding when quiet mode on
' 03-Jun-07 jab     Refactor Startup into separate function, called from
'                   Sub Main() and also Class Initialize().  This was required
'                   because object creation PUMPS EVENTS, allowing property
'                   and method calls to be services before initialization
'                   completes.
' -----------------------------------------------------------------------------

Option Explicit

Private quiet_time As Double
Private quiet_time_reset As Double
Private temperature_time As Double
Private temperature_time_reset As Double

'---------------------------------------------------------------------
'
' Main() - POTH main entry point
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
    Dim oldRegVer As Double
    
    If g_ComponentState = csRunning Then
        Exit Sub
    ElseIf g_ComponentState = csStarting Then
        Err.Raise SCODE_START_CONFLICT, ERR_SOURCE, MSG_START_CONFLICT
    End If
    g_ComponentState = csStarting

    ' get access to scope persisted data
    Set g_Profile = New DriverHelper.Profile
    g_Profile.DeviceType = "Telescope"              ' We're a telescope driver
    g_Profile.Register ID, DESC                     ' Self reg (skips if already reg)
        
    ' get access to dome persisted data
    Set g_DomeProfile = New DriverHelper.Profile
    g_DomeProfile.DeviceType = "Dome"               ' and a dome driver
    g_DomeProfile.Register IDDOME, DESC             ' Self reg (skips if already reg)
    
    ' get access to focuser persisted data
    Set g_FocuserProfile = New DriverHelper.Profile
    g_FocuserProfile.DeviceType = "Focuser"               ' and a focuser driver
    g_FocuserProfile.Register IDFOCUSER, DESC             ' Self reg (skips if already reg)
    
    ' check if all we're doing is registering POTH with ASCOM
    If InStr(Command$, "-r") >= 1 Then
        Exit Sub
    End If
    
    ' get handles for main objects
    Set g_handBox = New frmHandBox
    Set g_setupDlg = New frmSetup
    Set g_Util = New DriverHelper.Util
    LoadDLL "astro32.dll"                           ' Load the astronomy functions DLL

    Set g_AxisRatesEmpty = New AxisRates            ' empty for when no axis support
    Set g_TrackingRatesSimple = New TrackingRates   ' create default TrackingRates
    g_TrackingRatesSimple.Add driveSidereal         ' Sidereal only
    
    g_iConnections = 0                              ' zero scope connections currently
    g_iDomeConnections = 0                          ' zero dome connections currently
    g_iFocuserConnections = 0                       ' zero focuser connections currently
        
    ' get the registry format version so we can only update whats changed
    oldRegVer = 0#
    On Error Resume Next
    oldRegVer = CDbl(g_Profile.GetValue(ID, "RegVer"))
    On Error GoTo 0
    
    ' Persistent settings for the scope - Create on first start, or update
    If oldRegVer < 2.4 Then
        g_Profile.WriteValue ID, "ScopeID", "ScopeSim.Telescope"
        g_Profile.WriteValue ID, "ScopeName", "Simulator"
        
        g_Profile.WriteValue ID, "Aperture", Str(EMPTY_PARAMETER)
        g_Profile.WriteValue ID, "ApertureArea", Str(EMPTY_PARAMETER)
        g_Profile.WriteValue ID, "FocalLength", Str(EMPTY_PARAMETER)
        g_Profile.WriteValue ID, "Longitude", Str(EMPTY_PARAMETER)
        g_Profile.WriteValue ID, "Latitude", Str(EMPTY_PARAMETER)
        g_Profile.WriteValue ID, "Elevation", Str(EMPTY_PARAMETER)
        g_Profile.WriteValue ID, "SlewSettleTime", "0"
        g_Profile.WriteValue ID, "MeridianDelay", "0.0"
        g_Profile.WriteValue ID, "EquSystem", Str(equLocalTopocentric)
        g_Profile.WriteValue ID, "DoesRefraction", Str(refUnknown)
        g_Profile.WriteValue ID, "AutoUnpark", "False"
        
        g_Profile.WriteValue ID, "Left", "100"
        g_Profile.WriteValue ID, "Top", "100"
        g_Profile.WriteValue ID, "AdvancedSetup", "False"
        g_Profile.WriteValue ID, "DomeMode", "False"
        g_Profile.WriteValue ID, "MotionControl", "False"
    End If
    
    If oldRegVer < 4.7 Then
        g_Profile.WriteValue ID, "FocusMode", "False"
    End If
    
    If oldRegVer < 4.9 Then
        g_Profile.WriteValue ID, "QuietMode", "False"
    End If
    
    If oldRegVer < 5# Then
        g_Profile.WriteValue ID, "HAMode", False
    End If
    
    If oldRegVer < 5.1 Then
        g_Profile.WriteValue ID, "Backlash", False
        
        ' To initialize the new east side delay negate the west side delay
        ' This will keep the same behavior as the single variable case
        g_dMeridianDelay = 0
        On Error Resume Next
        g_dMeridianDelay = val(g_Profile.GetValue(ID, "MeridianDelay"))
        On Error GoTo 0
        g_Profile.WriteValue ID, "MeridianDelayEast", Str(-g_dMeridianDelay)
    End If
    
    If oldRegVer < 5.2 Then
        g_Profile.WriteValue ID, "Simple", False
    End If
    
    g_Profile.WriteValue ID, "RegVer", RegVer
    
    ' if we need to, then create initial persisted state for the dome
    ' do keep version 2.4 and 4.6 as sufficient
    If oldRegVer < 2.4 Then
        g_DomeProfile.WriteValue IDDOME, "DomeID", "DomeSim.Dome"
        g_DomeProfile.WriteValue IDDOME, "DomeName", "Simulator"
        
        g_DomeProfile.WriteValue IDDOME, "Radius", "1"
        g_DomeProfile.WriteValue IDDOME, "PosEW", "0"
        g_DomeProfile.WriteValue IDDOME, "PosNS", "0"
        g_DomeProfile.WriteValue IDDOME, "PosUD", "0"
        g_DomeProfile.WriteValue IDDOME, "GEMOffset", "0"
        g_DomeProfile.WriteValue IDDOME, "Slop", "2"
        g_DomeProfile.WriteValue IDDOME, "Freq", "5"
    End If
    
    g_DomeProfile.WriteValue IDDOME, "RegVer", RegVer
    
    ' if we need to, then create initial persisted state for the focuser
    If oldRegVer < "4.6" Then
        g_FocuserProfile.WriteValue IDFOCUSER, "FocuserID", "FocusSim.Focuser"
        g_FocuserProfile.WriteValue IDFOCUSER, "FocuserName", "FocusSim.Focuser"
        
        g_FocuserProfile.WriteValue IDFOCUSER, "FocuserRelativePosition", "0"
        g_FocuserProfile.WriteValue IDFOCUSER, "FocuserRelativeIncrement", "0"
    End If
    
    If oldRegVer < 4.8 Then
        g_FocuserProfile.WriteValue IDFOCUSER, "FocuserMaxIncrement", Str(EMPTY_PARAMETER)
        g_FocuserProfile.WriteValue IDFOCUSER, "FocuserMaxStep", Str(EMPTY_PARAMETER)
        g_FocuserProfile.WriteValue IDFOCUSER, "FocuserStepSize", Str(EMPTY_PARAMETER)
        g_FocuserProfile.WriteValue IDFOCUSER, "FocuserAbsMove", "True"
        g_FocuserProfile.WriteValue IDFOCUSER, "FocuserMoveMicrons", "False"
    End If
        
    g_FocuserProfile.WriteValue IDFOCUSER, "RegVer", RegVer
    
    ' find out if we're forcing classic late binding
    ' be careful, this registry entry may not exist
    If g_Profile.GetValue(ID, "ForceLate") = "True" Then
        g_bForceLate = True
    Else
        g_bForceLate = False
    End If
    
    ' remind ourself of the last devices used
    g_sScopeID = g_Profile.GetValue(ID, "ScopeID")
    g_sScopeName = g_Profile.GetValue(ID, "ScopeName")
    
    g_sDomeID = g_DomeProfile.GetValue(IDDOME, "DomeID")
    g_sDomeName = g_DomeProfile.GetValue(IDDOME, "DomeName")
        
    g_sFocuserID = g_FocuserProfile.GetValue(IDFOCUSER, "FocuserID")
    g_sFocuserName = g_FocuserProfile.GetValue(IDFOCUSER, "FocuserName")
      
    ' set up any gui state
    g_bSetupAdvanced = CBool(g_Profile.GetValue(ID, "AdvancedSetup"))
    g_bDomeMode = CBool(g_Profile.GetValue(ID, "DomeMode"))
    g_bFocusMode = CBool(g_Profile.GetValue(ID, "FocusMode"))
    g_bMotionControl = CBool(g_Profile.GetValue(ID, "MotionControl"))
    g_bHAMode = CBool(g_Profile.GetValue(ID, "HAMode"))
    g_handBox.Left = CLng(g_Profile.GetValue(ID, "Left")) * Screen.TwipsPerPixelX
    g_handBox.Top = CLng(g_Profile.GetValue(ID, "Top")) * Screen.TwipsPerPixelY
    
    ' get the dialogs created
    Load g_setupDlg
    Load g_handBox
    
    g_setupDlg.setDefaults True
    g_setupDlg.setDomeDefaults True
    g_setupDlg.setFocuserDefaults True
    
    ' initialize all the handbox modes
    g_handBox.Quiet
    g_handBox.SouthernHemisphere = (g_dLatitude < 0)
    g_handBox.AlignMode = g_eAlignMode
    g_handBox.ParkCaption = Not g_bAtPark
    g_setupDlg.LEDHome g_bAtHome
    ScopeTracking trackClear
    g_handBox.CheckWin True
    g_handBox.CheckHAMode
    
    ' Fix bad positions (which shouldn't ever happen, ha ha)
    If g_handBox.Left < 0 Then
        g_handBox.Left = 100 * Screen.TwipsPerPixelX
        g_Profile.WriteValue ID, "Left", Str(g_handBox.Left \ Screen.TwipsPerPixelX)
    End If
    If g_handBox.Top < 0 Then
        g_handBox.Top = 100 * Screen.TwipsPerPixelY
        g_Profile.WriteValue ID, "Top", Str(g_handBox.Top \ Screen.TwipsPerPixelY)
    End If
    
    ' place handset in ASCOM defined mode (servers are minimized)
    If App.StartMode = vbSModeStandalone Then
        g_handBox.WindowState = vbNormal
    Else
        g_handBox.WindowState = vbMinimized
    End If
            
    ' we're open for business
    g_handBox.Show
    
    ' get timers going
    quiet_time_reset = 5 / TIMER_INTERVAL           ' check coords every 5 sec
    quiet_time = 0
    
    slave_time = 0
    
    temperature_time_reset = 2.5 / TIMER_INTERVAL   ' not persisted or resetable
    temperature_time = 0
    
    g_ltimerID = SetTimer(0, 0, TIMER_INTERVAL * 1000, AddressOf timer_tick)
    If g_ltimerID = 0 Then
        MsgBox "Timer not created. Exiting program."
        End
    End If

    g_ComponentState = csRunning
    
End Sub

'---------------------------------------------------------------------
'
' DoShutdown() - Handle handbox form Unload event
'
' Also saves state and closes the telescope and dome connections.
'---------------------------------------------------------------------

Sub DoShutdown()

    On Error Resume Next
                    
    ' take care of scope and dome
    ScopeSave
    DomeSave
    FocuserSave
    ScopeDelete
    DomeDelete
    FocuserDelete
    
    ' save GUI state
    g_Profile.WriteValue ID, "AdvancedSetup", CStr(g_bSetupAdvanced)
    g_Profile.WriteValue ID, "DomeMode", CStr(g_bDomeMode)
    g_Profile.WriteValue ID, "FocusMode", CStr(g_bFocusMode)
    g_Profile.WriteValue ID, "MotionControl", CStr(g_bMotionControl)
    g_Profile.WriteValue ID, "HAMode", CStr(g_bHAMode)
    
    ' save windowing state
    g_handBox.Visible = True
    g_handBox.WindowState = vbNormal
    g_Profile.WriteValue ID, "Left", Str(g_handBox.Left \ Screen.TwipsPerPixelX)
    g_Profile.WriteValue ID, "Top", Str(g_handBox.Top \ Screen.TwipsPerPixelY)
    
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
    g_setupDlg.MotionControl = g_bMotionControl
    g_setupDlg.HAMode = g_bHAMode

    ' bounce the windows around
    g_handBox.Visible = False                       ' May float over setup
    g_setupDlg.AllowUnload = False
    FloatWindow g_setupDlg.hwnd, True
    
    g_setupDlg.Show 1                               ' model (waits for dismiss)
        
    If g_setupDlg Is Nothing Then
        Exit Sub                                    ' POTH killed while setup up
    End If
    g_setupDlg.AllowUnload = True

    ' update handset in case of change
    g_handBox.Quiet
    g_handBox.SouthernHemisphere = (g_dLatitude < 0)
    g_handBox.AlignMode = g_eAlignMode
    g_handBox.CheckWin False
    g_handBox.CheckHAMode
    
'    FloatWindow g_handBox.hwnd, False
    g_handBox.Visible = True
    
    On Error GoTo 0

End Sub

'---------------------------------------------------------------------
'
' timer_tick() - Called when timer fires event
'
' Implements handbox control and dome slaving.
' dwTime is the number of milliseconds since boot, same as GetTickCount()
'
'---------------------------------------------------------------------

Sub timer_tick(ByVal hwnd As Long, _
                ByVal uMsg As Long, _
                ByVal idEvent As Long, _
                ByVal dwTime As Long)

    Dim step As Double
    Dim getreal As Boolean
    Dim button As Integer
    Dim curDomeAz As Double
    Dim curDomeAlt As Double
    Dim newDomeAz As Double
    Dim newDomeAlt As Double
    Dim doAlt As Boolean
    Dim tRA As Double
    Dim tDec As Double
    Dim z As Double
    Dim tAz As Double
    Dim tAlt As Double
    Dim nowST As Double
    
    On Error GoTo ErrorHandler
    
    ' ST will be needed several times
    nowST = ScopeST()
        
    '----------------------
    ' Handle buttons first
    '----------------------
    
    button = g_handBox.ButtonState
    
    ' check scope control buttons
    If g_bConnected And button >= 1 And button <= 5 And _
            ((Not g_bAtPark) Or (Not g_bCanUnpark)) And _
            (Not g_bSimple) And _
            g_dRightAscension >= -360 And g_dDeclination >= -90 Then
        
        ' Immediately stop slewing
        ScopeAbortSlew
    
        ' if we're not just halting, then we're slewing
        If button <> 5 Then
            
            ' how far
            step = g_handBox.cbJog.ItemData(g_handBox.cbJog.ListIndex) / 60#
            z = Cos(g_dDeclination * DEG_RAD) * 15#
            If z < 0.001 Then _
                z = 0.001
            
            ' from where
            tRA = g_dRightAscension
            tDec = g_dDeclination
            
            ' which direction
            Select Case button
                Case 1                                  ' Top button
'                    If g_eAlignMode = algAltAz Then
'                        calc_radec g_dAzimuth, g_dAltitude + step, tRA, tDec
'                    Else
                        If g_dLatitude < 0 Then         ' Southern Hemisphere
                            tDec = g_dDeclination - step
                        Else
                            tDec = g_dDeclination + step
                        End If
'                    End If
                Case 2                                  ' Bottom button
'                    If g_eAlignMode = algAltAz Then
'                        calc_radec g_dAzimuth, g_dAltitude - step, tRA, tDec
'                    Else
                        If g_dLatitude < 0 Then         ' Southern Hemisphere
                            tDec = g_dDeclination + step
                        Else
                            tDec = g_dDeclination - step
                        End If
'                    End If
                Case 3                                  ' Right button
'                    If g_eAlignMode = algAltAz Then
'                        calc_radec g_dAzimuth + step, g_dAltitude, tRA, tDec
'                    Else
                        tRA = g_dRightAscension + (step / z)
'                    End If
                Case 4                                  ' Left button
'                    If g_eAlignMode = algAltAz Then
'                        calc_radec g_dAzimuth - step, g_dAltitude, tRA, tDec
'                    Else
                        tRA = g_dRightAscension - (step / z)
'                    End If
            End Select
        
            ' Range the results
            If tDec > 90# Then
                tDec = 90#
            ElseIf tDec < -90# Then
                tDec = -90#
            End If
            
            ' These may be large if near poles.
            While tRA < 0
                tRA = tRA + 24#
            Wend
            While tRA >= 24#
                tRA = tRA - 24#
            Wend
            
            If g_bTracking Then
                If g_bCanSlewAsync Then
                    ScopePreSlew tRA, tDec, True, True, True
                    g_Scope.SlewToCoordinatesAsync tRA, tDec
                    ScopePostSlew True, True
                End If
            Else
                If g_bCanSlewAltAzAsync Then
                    calc_altaz tRA, tDec, tAz, tAlt
                    ScopePreSlew tRA, tDec, True, True, True
                    g_Scope.SlewToAltAzAsync tAz, tAlt
                    ScopePostSlew True, True
                End If
            End If
            
        End If
    End If
   
   ' check dome control buttons
    If g_bDomeConnected And button >= 6 And button <= 10 Then
    
        ' halt dome and kill slaving
        DomeAbortSlew
        
        ' if we're not just halting, then we're slewing
        If button <> 10 Then
            
            ' how far
            step = g_handBox.cbJogDome.ItemData(g_handBox.cbJogDome.ListIndex) / 60#
            
            ' check if altitude is involved
            '   (should keep track of shutter status ??? only doing this
            '   if a button is pressed)
            doAlt = g_bDomeSetAltitude
            If doAlt And g_bDomeSetShutter Then _
                doAlt = doAlt And (g_Dome.ShutterStatus = shutterOpen)
            
            ' from where
            If g_bDomeSetAzimuth Then _
                tAz = g_Dome.Azimuth
            If doAlt Then _
                tAlt = g_Dome.Altitude
                
            ' which direction
            Select Case button
                Case 6                                  ' Top button
                    If doAlt Then
                        tAlt = tAlt + step
                    End If
                Case 7                                  ' Bottom button
                    If doAlt Then
                        tAlt = tAlt - step
                    End If
                Case 8                                  ' Right button
                    If g_bDomeSetAzimuth Then
                        tAz = tAz - step
                    End If
                Case 9                                  ' Left button
                    If g_bDomeSetAzimuth Then
                        tAz = tAz + step
                    End If
            End Select
            
            ' spin the dome
            If g_bDomeSetAzimuth Then _
                g_Dome.SlewToAzimuth AzScale(tAz)
                
            If doAlt Then
                g_Dome.SlewToAltitude AltScale(tAlt)
            End If
                
        End If
    End If
    
    '---------------------
    ' monitor scope state
    '---------------------
    
    If g_bConnected Then

        getreal = g_bSlewing                ' only be quiet if not slewing
        If g_bAsyncSlewing Then
            slave_time = 0                  ' force dome update
            ScopeSlewing slewFetch          ' actual slew state from scope
            g_bAsyncSlewing = g_bSlewing    ' async complete if not slewing
            If g_bSlaveSlew Then _
                g_bSlaveSlew = g_bSlewing   ' allow dome slaving if slew complete
            If getreal And Not g_bSlewing Then
                If Not g_show Is Nothing Then
                    If g_show.chkSlew.Value = 1 Then _
                        g_show.TrafficLine " (slew complete)"
                End If
            End If
            
        ElseIf g_bMonSlewing And Not g_bSlewing Then
        
            ScopeSlewing slewFetch          ' actual slew state from scope
            If g_bSlewing Then              ' have we begun slewing?
                slave_time = 0              ' update dome
                g_bAsyncSlewing = True      ' watch for slew end
                getreal = True              ' don't be quiet
                If g_bAtHome Then _
                    ScopeAtHome False, True
                If g_bAtPark Then _
                    ScopeAtPark False, True
                If Not g_show Is Nothing Then
                    If g_show.chkSlew.Value = 1 Then _
                        g_show.TrafficLine "Slew detected"
                End If
            End If
        End If
        
        ' if pulseguiding, make sure we refresh coordinates
        If g_lPulseGuideTix > 0 Then
            getreal = True                      ' make sure quiet mode reads coords
            If g_lPulseGuideTix < dwTime Then
                ' The real scope may not really be done yet
                If Not g_Scope.IsPulseGuiding Then
                    g_lPulseGuideTix = 0        ' not pulseguiding anymore
                End If
            End If
        End If

' Then code below seemed like a good idea for a while, but it
' invalidates the basic notion of quiet mode

'        ' wake up once in while to recheck state even when quiet
'        quiet_time = quiet_time - TIMER_INTERVAL
'        If quiet_time <= 0 Then
'            quiet_time = quiet_time_reset
'            getreal = True
'        End If
        
        If g_bQuiet And Not getreal Then
        
            If g_bTracking Then
                ScopeAtHome g_bAtHome, True
                ScopeAtPark g_bAtHome, True
                calc_altaz g_dRightAscension, g_dDeclination, g_dAzimuth, g_dAltitude                              ' Calculate alt-az
            Else
                calc_radec g_dAzimuth, g_dAltitude, g_dRightAscension, g_dDeclination                              ' Not tracking, calc new ra-dec
            End If
            
            ' SOP is unchanged
        Else
            ' read AtHome, AtPark, Tracking (right now we don't see the change) ???
            ScopeCoords True, False
        End If
    
    End If
    
    '--------------------------
    ' Update the scope display
    '--------------------------
    
    g_handBox.SiderealTime = nowST
    g_handBox.RightAscension = g_dRightAscension
    g_handBox.Declination = g_dDeclination
    g_handBox.Azimuth = g_dAzimuth
    g_handBox.Altitude = g_dAltitude
        
    '---------------------
    ' Handle dome slaving
    '---------------------
    
    If g_bConnected And g_bDomeConnected Then
        slave_time = slave_time - TIMER_INTERVAL    ' change this to GetTickCount ???
        If g_bSlaved And Not g_bSlaveSlew And slave_time <= 0 Then
            If Not g_Dome.Slewing Then

'                If Not g_show Is Nothing Then
'                    If g_show.chkSlew.Value = 1 Then _
'                        g_show.TrafficStart "Slave check:"
'                End If
                
                slave_time = slave_time_reset
                
                ' should set up quiet mode ???
                DomeCoord nowST, g_dRightAscension, g_dDeclination, _
                    newDomeAz, newDomeAlt, g_SOP
                
                ' check Azimuth
                If g_bDomeSetAzimuth And newDomeAz >= -360 Then
                    curDomeAz = g_Dome.Azimuth
                    
                    ' condition old dome coords for proper point in cycle
                    If curDomeAz >= newDomeAz Then
                        While curDomeAz - newDomeAz > 180
                            curDomeAz = curDomeAz - 360
                        Wend
                    Else
                        While newDomeAz - curDomeAz > 180
                            curDomeAz = curDomeAz + 360
                        Wend
                    End If
        
                    ' create some slop
                    If Abs(newDomeAz - curDomeAz) > g_iSlop Then
'                        If Not g_show Is Nothing Then
'                            If g_show.chkSlew.Value = 1 Then _
'                                g_show.TrafficEnd "(slaving Az)"
'                        End If
                        g_Dome.SlewToAzimuth AzScale(newDomeAz)
                        slave_time = 0
                    Else
'                        If Not g_show Is Nothing Then
'                            If g_show.chkSlew.Value = 1 Then _
'                                g_show.TrafficEnd "(AZ close enough)"
'                        End If
                    End If
                End If
                
                ' check Altitude
                doAlt = g_bDomeSetAltitude And newDomeAlt >= -90
                If doAlt And g_bDomeSetShutter Then _
                    doAlt = doAlt And (g_Dome.ShutterStatus = shutterOpen)
                If doAlt Then
                    curDomeAlt = g_Dome.Altitude
                    
                    ' if out of range, ensure no motion command
                    If curDomeAlt > 90 Or curDomeAlt < -90 Then _
                        newDomeAlt = curDomeAlt
        
                    ' create some slop
                    If Abs(newDomeAlt - curDomeAlt) > g_iSlop Then
'                        If Not g_show Is Nothing Then
'                            If g_show.chkSlew.Value = 1 Then _
'                                g_show.TrafficEnd "(slaving Alt)"
'                        End If
                        g_Dome.SlewToAltitude newDomeAlt
                        slave_time = 0
                    Else
'                        If Not g_show Is Nothing Then
'                            If g_show.chkSlew.Value = 1 Then _
'                                g_show.TrafficEnd "(Alt close enough)"
'                        End If
                    End If
                End If
            End If
        End If
    End If
    
    '---------------------------------------------
    ' Check dome state and update handset display
    '---------------------------------------------
    
    If g_bDomeConnected Then
        If g_bDomeSetAzimuth Then _
            g_dDomeAzimuth = g_Dome.Azimuth
    End If
    
    g_handBox.DomeAzimuth = g_dDomeAzimuth
    g_handBox.CheckShutter

    '------------------------------------------------
    ' Check focuser state and update handset display
    '------------------------------------------------
    
    If g_bFocuserConnected Then
        If g_bFocuserAbsolute Then
            On Error Resume Next
            g_lFocuserPosition = g_Focuser.Position
            If Err Then
                If Not g_show Is Nothing Then
                    If g_show.chkCoord.Value = 1 Then _
                        g_show.TrafficLine "Focuser position not available"
                End If
            End If
            On Error GoTo ErrorHandler
        End If
            
        temperature_time = temperature_time - TIMER_INTERVAL
        If temperature_time <= 0 Then
            temperature_time = temperature_time_reset
            If Not (g_bFocuserTempComp = g_Focuser.TempComp) Then
                g_bFocuserTempComp = Not g_bFocuserTempComp
                g_handBox.CheckFocuserEnable
            End If
            
            If g_bFocuserTempProbe Then
                On Error Resume Next
                g_dFocuserTemperature = g_Focuser.Temperature
                If Err Then
                    If Not g_show Is Nothing Then
                        If g_show.chkOther.Value = 1 Then _
                            g_show.TrafficLine "Focuser temperature not available"
                    End If
                End If
                On Error GoTo ErrorHandler
            End If
        End If ' temperature_time
        g_handBox.FocuserDisplay
    End If
    
    '---------------
    ' Final cleanup
    '---------------
    
    g_handBox.CheckWin False
    
    On Error GoTo 0
    
    Exit Sub

'------------------------------------------------------------
' if an error occurs, light an LED and check connected state
' then continue on
'------------------------------------------------------------

ErrorHandler:

    If Not g_show Is Nothing Then
        g_show.TrafficLine "Unexpected Error - " & Hex(Err.Number) & " - " & _
            Err.Source & " - " & Err.Description
    End If

    g_handBox.ErrorLED
    ScopeConnected
    DomeConnected
    FocuserConnected
    
    Resume Next
    
End Sub

' =============
'   Utilites
' =============

' range the altitude parameter
Public Function AltScale(ByVal Alt As Double) As Double

    AltScale = Alt
    If AltScale > 90 Then _
        AltScale = 90
    If AltScale < 0 Then _
        AltScale = 0

End Function

' range the azimuth parameter
Public Function AzScale(ByVal Az As Double) As Double

    While Az < 0
        Az = Az + 360
    Wend
    While Az >= 360
        Az = Az - 360
    Wend
    
    AzScale = Az

End Function

' range the azimuth parameter
Public Function HAScale(ByVal HA As Double) As Double

    While HA < -12
        HA = HA + 24
    Wend
    While HA >= 12
        HA = HA - 24
    Wend
    
    HAScale = HA

End Function

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

'---------------------------------------------------------------------
'
' calc_altaz() - Calculate current azimuth and altitude
'
'---------------------------------------------------------------------

Public Sub calc_altaz(ByVal RA As Double, ByVal Dec As Double, _
            Az As Double, Alt As Double)
        
    Dim tAz As Double
    Dim tAlt As Double

    If (g_dLatitude < -90#) Or (g_dLongitude < -360#) Or _
            (RA < -24#) Or (Dec < -90#) Then
        Alt = INVALID_PARAMETER
        Az = INVALID_PARAMETER
        Exit Sub
    End If
    
    ' backwards HA (work around) ???
    hadec_aa (g_dLatitude * DEG_RAD), _
            ((RA - now_lst(g_dLongitude * DEG_RAD)) * HRS_RAD), _
             (Dec * DEG_RAD), tAlt, tAz
             
    Alt = tAlt * RAD_DEG
    Az = 360# - (tAz * RAD_DEG)
    
End Sub

'---------------------------------------------------------------------
'
' calc_radec() - Calculate current right ascension and declination
'
'---------------------------------------------------------------------

Public Sub calc_radec(ByVal Az As Double, ByVal Alt As Double, _
        RA As Double, Dec As Double)
        
    Dim dHA As Double
    Dim tRA As Double
    Dim tDec As Double
    
    If (g_dLatitude < -90#) Or (g_dLongitude < -360#) Or _
            (Alt < -90#) Or (Az < -360#) Then
        RA = INVALID_PARAMETER
        Dec = INVALID_PARAMETER
        Exit Sub
    End If
    
    aa_hadec (g_dLatitude * DEG_RAD), _
            (Alt * DEG_RAD), _
            ((360# - Az) * DEG_RAD), _
            dHA, tDec
            
    ' backwards HA (work around) ???
    tRA = (dHA * RAD_HRS) + now_lst(g_dLongitude * DEG_RAD)
    If tRA < 0 Then tRA = tRA + 24#
    If tRA >= 24 Then tRA = tRA - 24#
    
    RA = tRA
    Dec = tDec * RAD_DEG
    
End Sub
