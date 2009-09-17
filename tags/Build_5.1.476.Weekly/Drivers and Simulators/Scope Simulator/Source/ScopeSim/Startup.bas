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
' ASCOM scope simulator main startup module
'
' Written:  27-Jun-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 27-Jun-00 rbd     Initial edit, from the code in ACP.
' 21-Jan-01 rbd     Eliminate Registry.bas, use DriverHelper.Profile
' 28-Jan-01 rbd     Interface capabilities, visual facelift, SpaceSoftware
'                   logo in Setup Dialog, click to go to StarryNight.
' 03-Feb-01 rbd     Implement Telescope.SlewSettleTime
' 05-Feb-01 rbd     Fix Park/Unpark checkbox persistence. Fix setup dialog
'                   runtime error. Add advanced/basic mode to setup form,
'                   this mode is persistent. Close telescope connection
'                   if hand box form is unloaded, allowing for graceful
'                   behavior of clients.
' 10-Mar-01 rbd     Prevent negative RA on startup calculation from persistent
'                   Alt/Az.
' 11-Mar-01 rbd     Add persistence for main form screen location. Add a third
'                   speed for the handbox manual controls, make slewing slow
'                   down as it approaches the target. Fix RA slewing to be
'                   physical not coordinate rate. Make timer interval a
'                   proper manifest constant.
' 13-Mar-01 rbd     Add capability control to Set Tracking and optics
'                   (aperture and focal length)
' 26-Apr-01 rbd     Fix initial main window location, was offscreen!
' 08-Jun-01 rbd     New Profile.DeviceType to generalize Chooser and
'                   Profile.
' 11-Oct-02 rbd     2.1.1 - Variable slew rate
' 06-Mar-03 jab     Add date handling and registry versioning
' 08-Mar-03 jab     Add GUI handling of aperture and focal length and get
'                   rid of CanCanOptics bug
' 15-Mar-03 jab     kill server if launched via client and last client bails,
'                   also only allow disconnect if last clients commands it,
'                   no disconnects allowed if launched manually, start
'                   minimized if client launched
' 21-Mar-03 rbd     2.2.1 - Version change for Platform 2.2
' 19-Apr-03 rbd     2.2.2 - Load frmSetup earlier to avoid "stuck behind"
'                   condition
' 16-Jun-03 jab     ASCOM Journaling
' 19-Jun-03 jab     updated window location saving code
' 24-Jun-03 rbd     2.3.2 - Left version for for Platform 2.3
' 05-Mar-04 jab     debounced the motion keys so they never get missed
' 05-Mar-04 jab     added V2, and finished missing V1 features
' 14-May-04 jab     Converted to API timer due to uSoft bug in VB timer object
' 01-Dec-04 rbd     4.0.2 - Remove trailing quote from registry key name
'                   Capabilities for the CanFindHome value in initial startup.
' 24-Jul-06 rbd     4.0.5 - Disconnect on park
' 05-Mar-07 rbd     5.0.1 - Oops, forgot to initialize disconnect on park in
'                   registry. Found with new XML-based helper. My bug.
' 12-Apr-07 rbd     5.0.1 - Remove old reentrancy test code, refactor startup
'                   into separate function, called from Sub Main() and also
'                   Telescope.Class_Initialize(). This was required because
'                   object creation PUMPS EVENTS, allowing Telescope property
'                   and method calls to be services before initialization
'                   completes.
' 02-Jun-07 jab     5.0.5 - Added setting default registry entry for disconnect
'                   on park.  Also, improved registry versioning.  Also added
'                   No Coordinates When Parked state.  Updated pulse guiding
'                   logic for more faithful simulation.
' -----------------------------------------------------------------------------

Option Explicit

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

    Dim buf As String
    Dim oldRegVer As Double
    
    If g_ComponentState = csRunning Then
        Exit Sub
    ElseIf g_ComponentState = csStarting Then
        Err.Raise SCODE_START_CONFLICT, ERR_SOURCE, MSG_START_CONFLICT
    End If
    g_ComponentState = csStarting
    
    Set g_handBox = New frmHandBox
    Set g_Util = New DriverHelper.Util
    Set g_Profile = New DriverHelper.Profile
    g_Profile.DeviceType = "Telescope"              ' We're a Telescope driver
    g_Profile.Register ID, DESC                     ' Self reg (skips if already reg)
    
    LoadDLL "astro32.dll"                           ' Load the astronomy functions DLL
    
    g_iConnections = 0                              ' zero connections currently
    g_bConnected = False                            ' Not yet connected
    g_Slewing = slewNone                            ' Not slewing
    g_bTracking = False                             ' Drive is not tracking
    g_bAtPark = False
    g_bAtHome = False
    g_dTargetRightAscension = INVALID_COORDINATE    ' No target coordinates
    g_dTargetDeclination = INVALID_COORDINATE
    g_dDeltaRA = 0#
    g_dDeltaDec = 0#
    g_dDeltaAz = 0#
    g_dDeltaAlt = 0#
    
    g_dGuideRateRightAscension = 15# * (1# / 3600#) / SIDRATE
    g_dGuideRateDeclination = g_dGuideRateRightAscension
    g_dDeclinationRate = 0#
    g_dRightAscensionRate = 0#
    g_eTrackingRate = driveSidereal
    
    g_lSlewSettleTime = 0                           ' No settling time
    g_lsettleTix = 0                                ' not curently settling
    g_lPulseGuideTixRA = 0                          ' not currently pulse guiding
    g_lPulseGuideTixDec = 0                         ' not currently pulse guiding
    
    '---------------------------------------------
    ' Persistent settings - Create on first start
    '---------------------------------------------
    
    ' get the registry format version so we can only update whats changed
    oldRegVer = 0#
    On Error Resume Next
    oldRegVer = CDbl(g_Profile.GetValue(ID, "RegVer"))
    On Error GoTo 0
    
    ' Persistent settings for the scope - Create on first start, or update
    If oldRegVer < 2.1 Then
        
        On Error Resume Next
        g_Profile.DeleteSubKey ID, "Site"           ' Remove old fossil beta stuff
        g_Profile.DeleteSubKey ID, "CanCanOptics"   ' Remove old typo artifact
        On Error GoTo 0

        ' Start with all capabilities turned on
        g_Profile.WriteValue ID, "CanAlignMode", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanAltAz", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanDateTime", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanDoesRefraction", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanEqu", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanFindHome", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanLatLongElev", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanOptics", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanPark", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanPulseGuide", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSetEquRates", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSetGuideRates", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSetPark", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSetSOP", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSetTracking", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSiderealTime", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSlew", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSlewAltAz", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSlewAltAzAsync", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSlewAsync", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSOP", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSync", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanSyncAltAz", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanTrackingRates", "True", "Capabilities"
        g_Profile.WriteValue ID, "CanUnpark", "True", "Capabilities"
        g_Profile.WriteValue ID, "DualAxisPulseGuide", "True", "Capabilities"
        g_Profile.WriteValue ID, "NumMoveAxis", "2", "Capabilities"
        g_Profile.WriteValue ID, "V1", "False", "Capabilities"
        
        ' GUI controls
        g_Profile.WriteValue ID, "AlwaysOnTop", "True"
        g_Profile.WriteValue ID, "AdvancedSetup", "False"
        g_Profile.WriteValue ID, "Left", "100"
        g_Profile.WriteValue ID, "Top", "100"
        
        ' Optics
        g_Profile.WriteValue ID, "Aperture", Str(INSTRUMENT_APERTURE)
        g_Profile.WriteValue ID, "ApertureArea", Str(INSTRUMENT_APERTURE_AREA)
        g_Profile.WriteValue ID, "FocalLength", Str(INSTRUMENT_FOCAL_LENGTH)
                
        ' Geography
        '
        ' Based on the UTC offset, create a longitude somewhere in
        ' the time zone, a latitude between 0 and 60 and a site
        ' elevation between 0 and 1000 metres. This gives the
        ' client some geo position without having to open the
        ' Setup dialog.
        '
        Randomize
        g_dLongitude = ((CSng(-utc_offs()) / 3600#) + Rnd() - 0.5) * 15#
        g_Profile.WriteValue ID, "Longitude", Str(g_dLongitude)
        g_dLatitude = 60# * Rnd()                   ' Lat between 0 and 60 deg
        g_Profile.WriteValue ID, "Latitude", Str(g_dLatitude)
        g_dSiteElevation = 1000# * Rnd()            ' Elevation between 0 and 1000  m
        g_Profile.WriteValue ID, "Elevation", Str(g_dSiteElevation)

        ' Start the scope at a "parked" location
        If g_dLatitude >= 0 Then
            g_dAzimuth = 180#
        Else
            g_dAzimuth = 0#
        End If
        g_dAltitude = 90# - Abs(g_dLatitude)
        
        ' save Alt/Az for restart.
        g_Profile.WriteValue ID, "StartAltitude", Str(g_dAltitude)
        g_Profile.WriteValue ID, "StartAzimuth", Str(g_dAzimuth)
        
        ' save as initial park position as well
        g_Profile.WriteValue ID, "ParkAltitude", Str(g_dAltitude)
        g_Profile.WriteValue ID, "ParkAzimuth", Str(g_dAzimuth)
        
        ' other changable state
        g_Profile.WriteValue ID, "AlignMode", CStr(algPolar)
        g_Profile.WriteValue ID, "AutoTrack", "False"
        g_Profile.WriteValue ID, "DateDelta", "0"
        g_Profile.WriteValue ID, "DoRefraction", "True"
        g_Profile.WriteValue ID, "EquSystem", "1"
        g_Profile.WriteValue ID, "MaxSlewRate", "20"     ' max slew rate deg/sec
        
    End If
    
    If oldRegVer < 2.2 Then
        ' a version existed that used DiscPark but didn't initialize it
        If g_Profile.GetValue(ID, "DiscPark") = "" Then
            g_Profile.WriteValue ID, "DiscPark", "False"
        End If
        
        g_Profile.WriteValue ID, "NoCoordAtPark", "False"
    End If
    
    g_Profile.WriteValue ID, "RegVer", RegVer
    
    '---------------------------------------------------
    ' Get all the state variable fetched and calculated
    '---------------------------------------------------
    
    ' Capabilities
    g_bCanAlignMode = CBool(g_Profile.GetValue(ID, "CanAlignMode", "Capabilities"))
    g_bCanAltAz = CBool(g_Profile.GetValue(ID, "CanAltAz", "Capabilities"))
    g_bCanDateTime = CBool(g_Profile.GetValue(ID, "CanDateTime", "Capabilities"))
    g_bCanDoesRefraction = CBool(g_Profile.GetValue(ID, "CanDoesRefraction", "Capabilities"))
    g_bCanEqu = CBool(g_Profile.GetValue(ID, "CanEqu", "Capabilities"))
    g_bCanFindHome = CBool(g_Profile.GetValue(ID, "CanFindHome", "Capabilities"))
    g_bCanLatLongElev = CBool(g_Profile.GetValue(ID, "CanLatLongElev", "Capabilities"))
    g_bCanOptics = CBool(g_Profile.GetValue(ID, "CanOptics", "Capabilities"))
    g_bCanPark = CBool(g_Profile.GetValue(ID, "CanPark", "Capabilities"))
    g_bDualAxisPulseGuide = CBool(g_Profile.GetValue(ID, "DualAxisPulseGuide", "Capabilities"))
    g_bCanSetEquRates = CBool(g_Profile.GetValue(ID, "CanSetEquRates", "Capabilities"))
    g_bCanSetGuideRates = CBool(g_Profile.GetValue(ID, "CanSetGuideRates", "Capabilities"))
    g_bCanSetPark = CBool(g_Profile.GetValue(ID, "CanSetPark", "Capabilities"))
    g_bCanPulseGuide = CBool(g_Profile.GetValue(ID, "CanPulseGuide", "Capabilities"))
    g_bCanSetSOP = CBool(g_Profile.GetValue(ID, "CanSetSOP", "Capabilities"))
    g_bCanSetTracking = CBool(g_Profile.GetValue(ID, "CanSetTracking", "Capabilities"))
    g_bCanSiderealTime = CBool(g_Profile.GetValue(ID, "CanSiderealTime", "Capabilities"))
    g_bCanSlew = CBool(g_Profile.GetValue(ID, "CanSlew", "Capabilities"))
    g_bCanSlewAltAz = CBool(g_Profile.GetValue(ID, "CanSlewAltAz", "Capabilities"))
    g_bCanSlewAltAzAsync = CBool(g_Profile.GetValue(ID, "CanSlewAltAzAsync", "Capabilities"))
    g_bCanSlewAsync = CBool(g_Profile.GetValue(ID, "CanSlewAsync", "Capabilities"))
    g_bCanSOP = CBool(g_Profile.GetValue(ID, "CanSOP", "Capabilities"))
    g_bCanSync = CBool(g_Profile.GetValue(ID, "CanSync", "Capabilities"))
    g_bCanSyncAltAz = CBool(g_Profile.GetValue(ID, "CanSyncAltAz", "Capabilities"))
    g_bCanTrackingRates = CBool(g_Profile.GetValue(ID, "CanTrackingRates", "Capabilities"))
    g_bCanUnpark = CBool(g_Profile.GetValue(ID, "CanUnpark", "Capabilities"))
    g_iNumMoveAxis = CInt(g_Profile.GetValue(ID, "NumMoveAxis", "Capabilities"))
    g_bV1 = CBool(g_Profile.GetValue(ID, "V1", "Capabilities"))

    ' GUI controls
    g_bAlwaysTop = CBool(g_Profile.GetValue(ID, "AlwaysOnTop"))
    g_bSetupAdvanced = CBool(g_Profile.GetValue(ID, "AdvancedSetup"))
    
    ' Optics
    g_dAperture = val(g_Profile.GetValue(ID, "Aperture"))
    g_dApertureArea = val(g_Profile.GetValue(ID, "ApertureArea"))
    g_dFocalLength = val(g_Profile.GetValue(ID, "FocalLength"))
    
    ' Geography
    g_dLatitude = val(g_Profile.GetValue(ID, "Latitude"))
    g_dLongitude = val(g_Profile.GetValue(ID, "Longitude"))
    g_dSiteElevation = val(g_Profile.GetValue(ID, "Elevation"))
    
    ' Park position
    g_dParkAltitude = val(g_Profile.GetValue(ID, "ParkAltitude"))
    g_dParkAzimuth = val(g_Profile.GetValue(ID, "ParkAzimuth"))
    
    ' other changable state
    g_eAlignMode = CInt(g_Profile.GetValue(ID, "AlignMode"))
    g_bAutoTrack = CBool(g_Profile.GetValue(ID, "AutoTrack"))
    g_bDiscPark = CBool(g_Profile.GetValue(ID, "DiscPark"))
    g_bNoCoordAtPark = CBool(g_Profile.GetValue(ID, "NoCoordAtPark"))
    g_dDateDelta = val(g_Profile.GetValue(ID, "DateDelta"))
    g_bDoRefraction = CBool(g_Profile.GetValue(ID, "DoRefraction"))
    g_eEquSystem = CInt(g_Profile.GetValue(ID, "EquSystem"))
    
    ' get rates ready to go
    g_dSlewRateFast = val(g_Profile.GetValue(ID, "MaxSlewRate")) * TIMER_INTERVAL
    g_dSlewRateMed = g_dSlewRateFast * 0.1
    g_dSlewRateSlow = g_dSlewRateFast * 0.02
    
    Set g_TrackingRatesFull = New TrackingRates
    g_TrackingRatesFull.Add driveSidereal
    g_TrackingRatesFull.Add driveLunar
    g_TrackingRatesFull.Add driveSolar
    g_TrackingRatesFull.Add driveKing
    
    Set g_TrackingRatesSimple = New TrackingRates
    g_TrackingRatesSimple.Add driveSidereal
    
    Set g_AxisRates = New AxisRates
    g_AxisRates.Add 8, 0                    ' the same for all axis
    Set g_AxisRatesEmpty = New AxisRates    ' empty for when no axis support
    
    ' Put scope position together
    g_dAltitude = val(g_Profile.GetValue(ID, "StartAltitude"))
    g_dAzimuth = val(g_Profile.GetValue(ID, "StartAzimuth"))
    
    calc_radec                  ' Calculate current equatorial coordinates
    
    If g_dAltitude = g_dParkAltitude And g_dAzimuth = g_dParkAzimuth Then
        If g_bAutoTrack Then
            g_bTracking = True
        Else
            g_bAtPark = True
        End If
    Else
        g_bTracking = g_bAutoTrack
    End If
    
    g_SOP = SOP(g_dAzimuth)     ' pier side is recalculated, its not persistent

    '-------------------
    ' Load setup dialog
    '-------------------
    Load frmSetup               ' Need this in Z-Order
    
    '-----------------------
    ' Get the handbox going
    '-----------------------
    Load g_handBox
    
    g_handBox.LEDPier g_SOP
    g_handBox.LEDHome g_bAtHome
    g_handBox.LEDPark g_bAtPark
    g_handBox.ParkCaption       ' get [Park] button in right state
    g_handBox.Tracking          ' set tracking checkbox to intial state
    g_handBox.SouthernHemisphere = (g_dLatitude < 0)
    g_handBox.AlignMode = g_eAlignMode
    g_handBox.Left = CLng(g_Profile.GetValue(ID, "Left")) * Screen.TwipsPerPixelX
    g_handBox.Top = CLng(g_Profile.GetValue(ID, "Top")) * Screen.TwipsPerPixelY
    
    ' Fix bad positions (which shouldn't ever happen, ha ha)
    If g_handBox.Left < 0 Then
        g_handBox.Left = 100 * Screen.TwipsPerPixelX
        g_Profile.WriteValue ID, "Left", CStr(g_handBox.Left \ Screen.TwipsPerPixelX)
    End If
    If g_handBox.Top < 0 Then
        g_handBox.Top = 100 * Screen.TwipsPerPixelY
        g_Profile.WriteValue ID, "Top", CStr(g_handBox.Top \ Screen.TwipsPerPixelY)
    End If
    
    ' set window state
    If App.StartMode = vbSModeStandalone Then
        g_handBox.WindowState = vbNormal
    Else
        g_handBox.WindowState = vbMinimized
    End If
    
    g_handBox.Show
    
    '------------------
    ' begin simulation
    '------------------
    
    g_bInTimer = False

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
' Save current alt/az coordinates for restoration on next start.
' Also "closes" the telescope connection.
'---------------------------------------------------------------------
Sub DoShutdown()

    ' shut down simulation
    g_bConnected = False
    KillTimer 0, g_ltimerID
    
    ' save persistent data that can change outside the setup dialog
    g_Profile.WriteValue ID, "StartAzimuth", Str(g_dAzimuth)
    g_Profile.WriteValue ID, "StartAltitude", Str(g_dAltitude)
    g_Profile.WriteValue ID, "ParkAzimuth", Str(g_dParkAzimuth)
    g_Profile.WriteValue ID, "ParkAltitude", Str(g_dParkAltitude)
    g_Profile.WriteValue ID, "Elevation", Str(g_dSiteElevation)
    g_Profile.WriteValue ID, "Latitude", Str(g_dLatitude)
    g_Profile.WriteValue ID, "Longitude", Str(g_dLongitude)
    g_Profile.WriteValue ID, "DateDelta", Str(g_dDateDelta)
    
    ' take care of window position
    g_handBox.Visible = True
    g_handBox.WindowState = vbNormal
    g_Profile.WriteValue ID, "Left", CStr(g_handBox.Left \ Screen.TwipsPerPixelX)
    g_Profile.WriteValue ID, "Top", CStr(g_handBox.Top \ Screen.TwipsPerPixelY)
    
End Sub

'---------------------------------------------------------------------
'
' DoSetup() - Handle handbox Setup button click
'
' When the geographic location is changed, this assumes that the
' scope alt/az remains constant, and it recomputes equatorial
' coordinates for the new site location.
'---------------------------------------------------------------------

Sub DoSetup()

    Dim ans As Boolean
    Dim i As Integer

    With frmSetup
        .AllowUnload = False                        ' Assure not unloaded
        
        .chkCanAlignMode.Value = IIf(g_bCanAlignMode, 1, 0)
        .chkCanAltAz.Value = IIf(g_bCanAltAz, 1, 0)
        .chkCanDateTime.Value = IIf(g_bCanDateTime, 1, 0)
        .chkCanDoesRefraction.Value = IIf(g_bCanDoesRefraction, 1, 0)
        .chkCanEqu.Value = IIf(g_bCanEqu, 1, 0)
        .chkCanFindHome.Value = IIf(g_bCanFindHome, 1, 0)
        .chkCanLatLongElev.Value = IIf(g_bCanLatLongElev, 1, 0)
        .chkCanOptics.Value = IIf(g_bCanOptics, 1, 0)
        .chkCanPark.Value = IIf(g_bCanPark, 1, 0)
        .chkCanPulseGuide.Value = IIf(g_bCanPulseGuide, 1, 0)
        .chkCanSetEquRates.Value = IIf(g_bCanSetEquRates, 1, 0)
        .chkCanSetGuideRates.Value = IIf(g_bCanSetGuideRates, 1, 0)
        .chkCanSetPark.Value = IIf(g_bCanSetPark, 1, 0)
        .chkCanSetSOP.Value = IIf(g_bCanSetSOP, 1, 0)
        .chkCanSetTracking.Value = IIf(g_bCanSetTracking, 1, 0)
        .chkCanSiderealTime.Value = IIf(g_bCanSiderealTime, 1, 0)
        .chkCanSlew.Value = IIf(g_bCanSlew, 1, 0)
        .chkCanSlewAltAz.Value = IIf(g_bCanSlewAltAz, 1, 0)
        .chkCanSlewAltAzAsync.Value = IIf(g_bCanSlewAltAzAsync, 1, 0)
        .chkCanSlewAsync.Value = IIf(g_bCanSlewAsync, 1, 0)
        .chkCanSOP.Value = IIf(g_bCanSOP, 1, 0)
        .chkCanSync.Value = IIf(g_bCanSync, 1, 0)
        .chkCanSyncAltAz.Value = IIf(g_bCanSyncAltAz, 1, 0)
        .chkCanTrackingRates.Value = IIf(g_bCanTrackingRates, 1, 0)
        .chkCanUnpark.Value = IIf(g_bCanUnpark, 1, 0)
        .chkDoRefraction = IIf(g_bDoRefraction, 1, 0)
        .chkDualAxisPulseGuide.Value = IIf(g_bDualAxisPulseGuide, 1, 0)
        .chkV1.Value = IIf(g_bV1, 1, 0)
        
        .chkAlwaysTop = IIf(g_bAlwaysTop, 1, 0)
        .AdvancedMode = g_bSetupAdvanced
        
        .Aperture = g_dAperture
        .ApertureArea = g_dApertureArea
        .FocalLength = g_dFocalLength
        
        .Longitude = g_dLongitude
        .Latitude = g_dLatitude
        .Elevation = g_dSiteElevation
        
        .AlignMode = g_eAlignMode
        .chkAutoTrack.Value = IIf(g_bAutoTrack, 1, 0)
        .chkDiscPark.Value = IIf(g_bDiscPark, 1, 0)
        .chkNoCoordAtPark.Value = IIf(g_bNoCoordAtPark, 1, 0)
        .sldSlewRate.Value = g_dSlewRateFast / TIMER_INTERVAL
        
        ' set NumMoveAxis list item
        For i = .cbNumMoveAxis.ListCount - 1 To 0 Step -1
            If .cbNumMoveAxis.ItemData(i) = g_iNumMoveAxis Then _
                Exit For
        Next i
        If i < 0 Then _
            i = 0
        .cbNumMoveAxis.ListIndex = i
        
        ' set Equatorial Coordinates list item
        For i = .cbEquSystem.ListCount - 1 To 0 Step -1
            If .cbEquSystem.ItemData(i) = g_eEquSystem Then _
                Exit For
        Next i
        If i < 0 Then _
            i = 0
        .cbEquSystem.ListIndex = i
    
    End With
    
    '-----------------
    g_handBox.Visible = False                       ' May float over setup
    FloatWindow frmSetup.hWnd, True
    frmSetup.Show 1
    '-----------------
    
    With frmSetup
        If .Result Then             ' Unless cancelled
            
            ' Capabilities
            g_bCanAlignMode = (.chkCanAlignMode.Value = 1)
            g_Profile.WriteValue ID, "CanAlignMode", CStr(g_bCanAlignMode), "Capabilities"
            g_bCanAltAz = (.chkCanAltAz.Value = 1)
            g_Profile.WriteValue ID, "CanAltAz", CStr(g_bCanAltAz), "Capabilities"
            g_bCanDateTime = (.chkCanDateTime.Value = 1)
            g_Profile.WriteValue ID, "CanDateTime", CStr(g_bCanDateTime), "Capabilities"
            g_bCanDoesRefraction = (.chkCanDoesRefraction.Value = 1)
            g_Profile.WriteValue ID, "CanDoesRefraction", CStr(g_bCanDoesRefraction), "Capabilities"
            g_bCanEqu = (.chkCanEqu.Value = 1)
            g_Profile.WriteValue ID, "CanEqu", CStr(g_bCanEqu), "Capabilities"
            g_bCanFindHome = (.chkCanFindHome.Value = 1)
            g_Profile.WriteValue ID, "CanFindHome", CStr(g_bCanFindHome), "Capabilities"
            g_bCanLatLongElev = (.chkCanLatLongElev.Value = 1)
            g_Profile.WriteValue ID, "CanLatLongElev", CStr(g_bCanLatLongElev), "Capabilities"
            g_bCanOptics = (.chkCanOptics.Value = 1)
            g_Profile.WriteValue ID, "CanOptics", CStr(g_bCanOptics), "Capabilities"
            g_bCanPark = (.chkCanPark.Value = 1)
            g_Profile.WriteValue ID, "CanPark", CStr(g_bCanPark), "Capabilities"
            g_bCanPulseGuide = (.chkCanPulseGuide.Value = 1)
            g_Profile.WriteValue ID, "CanPulseGuide", CStr(g_bCanPulseGuide), "Capabilities"
            g_bCanSetEquRates = (.chkCanSetEquRates.Value = 1)
            g_Profile.WriteValue ID, "CanSetEquRates", CStr(g_bCanSetEquRates), "Capabilities"
            g_bCanSetGuideRates = (.chkCanSetGuideRates.Value = 1)
            g_Profile.WriteValue ID, "CanSetGuideRates", CStr(g_bCanSetGuideRates), "Capabilities"
            g_bCanSetPark = (.chkCanSetPark.Value = 1)
            g_Profile.WriteValue ID, "CanSetPark", CStr(g_bCanSetPark), "Capabilities"
            g_bCanSetTracking = (.chkCanSetTracking.Value = 1)
            g_bCanSetSOP = (.chkCanSetSOP.Value = 1)
            g_Profile.WriteValue ID, "CanSetSOP", CStr(g_bCanSetSOP), "Capabilities"
            g_Profile.WriteValue ID, "CanSetTracking", CStr(g_bCanSetTracking), "Capabilities"
            g_bCanSiderealTime = (.chkCanSiderealTime.Value = 1)
            g_Profile.WriteValue ID, "CanSiderealTime", CStr(g_bCanSiderealTime), "Capabilities"
            g_bCanSlew = (.chkCanSlew.Value = 1)
            g_Profile.WriteValue ID, "CanSlew", CStr(g_bCanSlew), "Capabilities"
            g_bCanSlewAltAz = (.chkCanSlewAltAz.Value = 1)
            g_Profile.WriteValue ID, "CanSlewAltAz", CStr(g_bCanSlewAltAz), "Capabilities"
            g_bCanSlewAltAzAsync = (.chkCanSlewAltAzAsync.Value = 1)
            g_Profile.WriteValue ID, "CanSlewAltAzAsync", CStr(g_bCanSlewAltAzAsync), "Capabilities"
            g_bCanSlewAsync = (.chkCanSlewAsync.Value = 1)
            g_Profile.WriteValue ID, "CanSlewAsync", CStr(g_bCanSlewAsync), "Capabilities"
            g_bCanSOP = (.chkCanSOP.Value = 1)
            g_Profile.WriteValue ID, "CanSOP", CStr(g_bCanSOP), "Capabilities"
            g_bCanSync = (.chkCanSync.Value = 1)
            g_Profile.WriteValue ID, "CanSync", CStr(g_bCanSync), "Capabilities"
            g_bCanSyncAltAz = (.chkCanSyncAltAz.Value = 1)
            g_Profile.WriteValue ID, "CanSyncAltAz", CStr(g_bCanSyncAltAz), "Capabilities"
            g_bCanTrackingRates = (.chkCanTrackingRates.Value = 1)
            g_Profile.WriteValue ID, "CanTrackingRates", CStr(g_bCanTrackingRates), "Capabilities"
            g_bCanUnpark = (.chkCanUnpark.Value = 1)
            g_Profile.WriteValue ID, "CanUnpark", CStr(g_bCanUnpark), "Capabilities"
            g_bDualAxisPulseGuide = (.chkDualAxisPulseGuide.Value = 1)
            g_Profile.WriteValue ID, "DualAxisPulseGuide", CStr(g_bDualAxisPulseGuide), "Capabilities"
            g_iNumMoveAxis = .cbNumMoveAxis.ItemData(.cbNumMoveAxis.ListIndex)
            g_Profile.WriteValue ID, "NumMoveAxis", CStr(g_iNumMoveAxis), "Capabilities"
            g_bV1 = (.chkV1.Value = 1)
            g_Profile.WriteValue ID, "V1", CStr(g_bV1), "Capabilities"
            
            ' GUI
            g_bSetupAdvanced = .AdvancedMode
            g_Profile.WriteValue ID, "AdvancedSetup", CStr(g_bSetupAdvanced)
            g_bAlwaysTop = (.chkAlwaysTop = 1)
            g_Profile.WriteValue ID, "AlwaysOnTop", CStr(g_bAlwaysTop)
            
            ' Optics
            g_dAperture = .Aperture
            g_Profile.WriteValue ID, "Aperture", Str(g_dAperture)
            g_dApertureArea = .ApertureArea
            g_Profile.WriteValue ID, "ApertureArea", Str(g_dApertureArea)
            g_dFocalLength = .FocalLength
            g_Profile.WriteValue ID, "FocalLength", Str(g_dFocalLength)
            
            ' Geography
            g_dLatitude = .Latitude
            g_dLongitude = .Longitude
            g_dSiteElevation = .Elevation
                  
            ' some changes require recalcs, so just do them
            If g_eAlignMode <> .AlignMode Then _
                g_SOP = pierUnknown
            g_eAlignMode = .AlignMode
            If g_eAlignMode = algGermanPolar And g_SOP = pierUnknown Then _
                g_SOP = SOP(g_dAzimuth)
            g_Profile.WriteValue ID, "AlignMode", CStr(g_eAlignMode)
            g_handBox.LEDPier g_SOP
            calc_radec                          ' Recompute equatorial
            g_handBox.SouthernHemisphere = (g_dLatitude < 0)
            g_handBox.AlignMode = g_eAlignMode
            
            g_bAutoTrack = (.chkAutoTrack.Value = 1)
            g_Profile.WriteValue ID, "AutoTrack", CStr(g_bAutoTrack)
            g_bDiscPark = (.chkDiscPark.Value = 1)
            g_Profile.WriteValue ID, "DiscPark", CStr(g_bDiscPark)
            g_bNoCoordAtPark = (.chkNoCoordAtPark.Value = 1)
            g_Profile.WriteValue ID, "NoCoordAtPark", CStr(g_bNoCoordAtPark)
            g_bDoRefraction = (.chkDoRefraction.Value = 1)
            g_Profile.WriteValue ID, "DoRefraction", CStr(g_bDoRefraction)
            g_eEquSystem = .cbEquSystem.ItemData(.cbEquSystem.ListIndex)
            g_Profile.WriteValue ID, "EquSystem", CStr(g_eEquSystem)
            g_dSlewRateFast = .sldSlewRate.Value * TIMER_INTERVAL
            g_Profile.WriteValue ID, "MaxSlewRate", Str(val(.sldSlewRate.Value))
            g_dSlewRateMed = g_dSlewRateFast * 0.1
            g_dSlewRateSlow = g_dSlewRateFast * 0.02
            
        End If
        .AllowUnload = True                     ' OK to unload now
    End With
'   ------------------------
    FloatWindow g_handBox.hWnd, g_bAlwaysTop
    g_handBox.Visible = True
'   ------------------------

End Sub

'---------------------------------------------------------------------
'
' timer_tick() - Called when timer fires event
'
' Implements slewing and handbox control.
'---------------------------------------------------------------------

Sub timer_tick(ByVal hWnd As Long, _
                ByVal uMsg As Long, _
                ByVal idEvent As Long, _
                ByVal dwTime As Long)
    Dim step As Double
    Dim slow As Boolean
    Dim Y As Double, z As Double
    Dim Button As Integer
    Dim RARate As Double
    Dim DecRate As Double
    
    ' watch for reentrancy
    If g_bInTimer Then
        Exit Sub
    Else
        g_bInTimer = True
    End If
    
    z = Cos(g_dDeclination * DEG_RAD) * 15#
    If z < 0.001 Then z = 0.001

    '-----------------------------
    ' Handle hand-box state first
    '-----------------------------
    Button = g_handBox.ButtonState
    If Button <> 0 Then              ' Handbox buttons have prio
        g_Slewing = slewNone                        ' Immediately stop slewing
        ChangeHome False
        ChangePark False
        
        Select Case (Button And &H18)
            Case &H0: step = g_dSlewRateFast        ' No SHIFT, no COMMAND, full speed
            Case &H8: step = g_dSlewRateMed         ' SHIFT key, medium rate
            Case &H10: step = g_dSlewRateSlow       ' CTRL key, slow rate
            Case Else: step = g_dSlewRateSlow       ' SHIFT_CTRL, still slow speed
        End Select
        
        Select Case Button And 7     ' (mask off shift bit)
            Case 1                                  ' Top button
                If g_eAlignMode = algAltAz Then
                    g_dAltitude = g_dAltitude + step
                    RangeAltAz
                    calc_radec
                Else
                    If g_dLatitude < 0 Then         ' Southern Hemisphere (typ)
                        g_dDeclination = g_dDeclination - step
                    Else
                        g_dDeclination = g_dDeclination + step
                    End If
                    RangeRADec
                    calc_altaz
                End If
            Case 2                                  ' Bottom button
                If g_eAlignMode = algAltAz Then
                    g_dAltitude = g_dAltitude - step
                    RangeAltAz
                    calc_radec
                Else
                    If g_dLatitude < 0 Then         ' Southern Hemisphere (typ)
                        g_dDeclination = g_dDeclination + step
                    Else
                        g_dDeclination = g_dDeclination - step
                    End If
                    RangeRADec
                    calc_altaz
                End If
            Case 3                                  ' Right button
                If g_eAlignMode = algAltAz Then
                    g_dAzimuth = g_dAzimuth + step
                    RangeAltAz
                    calc_radec
                Else
                    g_dRightAscension = g_dRightAscension + (step / z)
                    RangeRADec
                    calc_altaz
                End If
            Case 4                                  ' Left button
                If g_eAlignMode = algAltAz Then
                    g_dAzimuth = g_dAzimuth - step
                    RangeAltAz
                    calc_radec
                Else
                    g_dRightAscension = g_dRightAscension - (step / z)
                    RangeRADec
                    calc_altaz
                End If
        End Select
     End If
    
    '--------------------
    ' Equatorial slewing
    '--------------------
    If g_Slewing = slewRADec Then
        g_lsettleTix = dwTime + g_lSlewSettleTime  ' still slewing
        
        ' Step RA
        Y = Abs(g_dDeltaRA) * z                     ' Angular RA distance left
        If g_dSlewRateFast / TIMER_INTERVAL >= 50 Then
            step = Y / z                            ' Top speed is instant slew
        ElseIf Y > 2 * g_dSlewRateFast Then
            step = g_dSlewRateFast / z              ' Large steps
        ElseIf Y > 2 * g_dSlewRateMed Then
            step = g_dSlewRateMed / z               ' Medium steps
        ElseIf Y > 2 * g_dSlewRateSlow Then
            step = g_dSlewRateSlow / z              ' Small step
        Else                                        ' Last step
            step = Y / z                            ' Jump to target
        End If
        step = step * Sgn(g_dDeltaRA)
        g_dRightAscension = g_dRightAscension + step
        g_dDeltaRA = g_dDeltaRA - step
        
        ' Step dec
        Y = Abs(g_dDeltaDec)                        ' Dec distance left
        If g_dSlewRateFast / TIMER_INTERVAL >= 50 Then
            step = Y                                ' Top speed is instant slew
        ElseIf Y > 2 * g_dSlewRateFast Then
            step = g_dSlewRateFast                  ' Large steps
        ElseIf Y > 2 * g_dSlewRateMed Then
            step = g_dSlewRateMed                   ' Medium steps
        ElseIf Y > 2 * g_dSlewRateSlow Then
            step = g_dSlewRateSlow                  ' Small step
        Else                                        ' Last step
            step = Y                                ' Jump to target
        End If
        step = step * Sgn(g_dDeltaDec)
        g_dDeclination = g_dDeclination + step
        g_dDeltaDec = g_dDeltaDec - step
        
        RangeRADec
        calc_altaz
        
        ' check for end of slew and go into settling time
        If Abs(g_dDeltaRA) < 0.0000001 And Abs(g_dDeltaDec) < 0.000001 Then _
            g_Slewing = slewSettle
    
    '----------------
    ' Alt Az slewing
    '----------------
    ElseIf g_Slewing >= slewAltAz Then
        g_lsettleTix = dwTime + g_lSlewSettleTime  ' still slewing

        ' Step alt
        Y = Abs(g_dDeltaAlt)                        ' Alt distance left
        If g_dSlewRateFast / TIMER_INTERVAL >= 50 Then
            step = Y                                ' Top speed is instant slew
        ElseIf Y > 2 * g_dSlewRateFast Then
            step = g_dSlewRateFast                  ' Large steps
        ElseIf Y > 2 * g_dSlewRateMed Then
            step = g_dSlewRateMed                   ' Medium steps
        ElseIf Y > 2 * g_dSlewRateSlow Then
            step = g_dSlewRateSlow                  ' Small step
        Else                                        ' Last step
            step = Y                                ' Jump to target
        End If
        step = step * Sgn(g_dDeltaAlt)
        g_dAltitude = g_dAltitude + step
        g_dDeltaAlt = g_dDeltaAlt - step
        
        ' Step az
        Y = Abs(g_dDeltaAz)                         ' Alt distance left
        If g_dSlewRateFast / TIMER_INTERVAL >= 50 Then
            step = Y                                ' Top speed is instant slew
        ElseIf Y > 2 * g_dSlewRateFast Then
            step = g_dSlewRateFast                  ' Large steps
        ElseIf Y > 2 * g_dSlewRateMed Then
            step = g_dSlewRateMed                   ' Medium steps
        ElseIf Y > 2 * g_dSlewRateSlow Then
            step = g_dSlewRateSlow                  ' Small step
        Else                                        ' Last step
            step = Y                                ' Jump to target
        End If
        step = step * Sgn(g_dDeltaAz)
        g_dAzimuth = g_dAzimuth + step
        g_dDeltaAz = g_dDeltaAz - step
        
        RangeAltAz
        calc_radec
    
        ' check for end of slew
        If Abs(g_dDeltaAlt) < 0.0000001 And Abs(g_dDeltaAz) < 0.000001 Then
        
            ' check for end of special alt/az slews
            If g_Slewing = slewPark Then
                g_Slewing = slewNone
                ChangePark True
            ElseIf g_Slewing = slewHome Then
                g_Slewing = slewNone
                ChangeHome True
            Else
                ' go into settling time for normal alt/az only
                g_Slewing = slewSettle
            End If
        End If
    
    '---------------------
    ' simple axis slewing
    '---------------------
    ElseIf g_Slewing = slewMoveAxis Then
    
        If g_eAlignMode = algAltAz Then
            g_dAzimuth = g_dAzimuth + g_dDeltaAz * TIMER_INTERVAL
            g_dAltitude = g_dAltitude + g_dDeltaAlt * TIMER_INTERVAL
            
            RangeAltAz
            calc_radec
        Else
            g_dRightAscension = g_dRightAscension + (g_dDeltaAz * TIMER_INTERVAL) / 15#
            g_dDeclination = g_dDeclination + g_dDeltaAlt * TIMER_INTERVAL
            
            RangeRADec
            calc_altaz
        End If
    
    '------------------------------------------------
    ' normal tracking with PulseGuide and GuideRates
    '------------------------------------------------
    ElseIf g_bTracking Or g_lPulseGuideTixRA > 0 Or g_lPulseGuideTixDec > 0 Then
        ChangeHome False
        ChangePark False
        
        ' check for end of RA direction PulseGuide
        If g_lPulseGuideTixRA > 0 Then
            If g_lPulseGuideTixRA + (TIMER_INTERVAL / 2) <= dwTime Then
                If Not g_show Is Nothing Then
                    If g_show.chkSlew.Value = 1 Then _
                        g_show.TrafficLine "(PulseGuide in RA complete)"
                End If
                g_lPulseGuideTixRA = 0            ' not pulseguiding anymore
            End If
        End If
        
        ' check for end of Dec direction PulseGuide
        If g_lPulseGuideTixDec > 0 Then
            If g_lPulseGuideTixDec + (TIMER_INTERVAL / 2) <= dwTime Then
                If Not g_show Is Nothing Then
                    If g_show.chkSlew.Value = 1 Then _
                        g_show.TrafficLine "(PulseGuide in Dec complete)"
                End If
                g_lPulseGuideTixDec = 0            ' not pulseguiding anymore
            End If
        End If
        
        ' calculae effective drive rates
        RARate = IIf(g_bTracking, g_dRightAscensionRate, 15#)   ' arc-sec / sid-sec
        RARate = (RARate / SIDRATE) / 3600#                     ' RA hours / sec
        If g_lPulseGuideTixRA > 0 Then _
            RARate = RARate + (g_dGuideRateRightAscension / 15#)
            
        DecRate = g_dDeclinationRate / 3600#                    ' deg / sec
        If g_lPulseGuideTixDec > 0 Then _
            DecRate = DecRate + g_dGuideRateDeclination
        
        ' change this to GetTickCount ??? we may miss a timer
        g_dRightAscension = g_dRightAscension + (RARate * TIMER_INTERVAL)
        g_dDeclination = g_dDeclination + (DecRate * TIMER_INTERVAL)

'        g_dRightAscension = g_dRightAscension + _
'            ((g_dRightAscensionRate / SIDRATE) * TIMER_INTERVAL) / 3600#
'        g_dDeclination = g_dDeclination + _
'            (g_dDeclinationRate * TIMER_INTERVAL) / 3600#

        RangeRADec
        calc_altaz                                  ' Calculate alt-az
    
    '-------------------------
    ' not tracking or slewing
    '-------------------------
    Else
        calc_radec                                  ' Not tracking, calc new ra-dec
    End If
    
    '-----------------------
    ' settletime monitoring
    '-----------------------
    If g_Slewing = slewSettle Then
        If dwTime > g_lsettleTix Then
            If Not g_show Is Nothing Then
                If g_show.chkSlew.Value = 1 Then _
                    g_show.TrafficLine "(Slew complete)"
            End If
            g_Slewing = slewNone
        End If
    End If
    
    '----------------------------
    ' Update the handbox display
    '----------------------------
    g_handBox.SiderealTime = now_lst(g_dLongitude * DEG_RAD)
    g_handBox.RightAscension = g_dRightAscension
    g_handBox.Declination = g_dDeclination
    g_handBox.Azimuth = g_dAzimuth
    g_handBox.Altitude = g_dAltitude
    
    g_bInTimer = False
    
End Sub

