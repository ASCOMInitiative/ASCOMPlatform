Attribute VB_Name = "Globals"
' -----------------------------------------------------------------------------
'   =============
'    Globals.BAS
'   =============
'
' ASCOM scope simulator globals module
'
' Written:  04-Mar-04   Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 04-Mar-04 rbd     initial pull out from Startup module, and V2 features
' 24-Jun-06 rbd     Disconnect on park variables
' 12-Apr-07 rbd     5.0.1 - Remove old reentrancy test code. eliminate
'                   g_bRunExecutable, use App.StartMode directly, to avoid
'                   startup problems with refactored initialization.
' -----------------------------------------------------------------------------

Option Explicit

Enum ComponentStates
    csLoaded = 0
    csStarting = 1
    csRunning = 2
End Enum

' ------------------------------
' Control objects and variables
' ------------------------------
Public g_ComponentState As ComponentStates
Public g_iConnections As Integer
Public g_handBox As frmHandBox             ' Hand box
Public g_Profile As DriverHelper.Profile
Public g_show As frmShow                   ' Traffic window
'Public g_timer As VB.timer                 ' Handy reference to timer
' convert other simulators to API timers ???
Public g_ltimerID As Long                   ' timer ID
Public g_bInTimer As Boolean                ' reentrance flag
Public g_Util As DriverHelper.Util

'-------------------------------------
' Interface Capabilities (persistent)
'-------------------------------------
Public g_bCanAlignMode As Boolean
Public g_bCanAltAz As Boolean
Public g_bCanDateTime As Boolean
Public g_bCanDoesRefraction As Boolean
Public g_bCanEqu As Boolean
Public g_bCanFindHome As Boolean
Public g_bCanLatLongElev As Boolean
Public g_bCanOptics As Boolean
Public g_bCanPark As Boolean
Public g_bCanPulseGuide As Boolean
Public g_bCanSetEquRates As Boolean
Public g_bCanSetGuideRates As Boolean
Public g_bCanSetPark As Boolean
Public g_bCanSetSOP As Boolean
Public g_bCanSetTracking As Boolean
Public g_bCanSiderealTime As Boolean
Public g_bCanSlew As Boolean
Public g_bCanSlewAltAz As Boolean
Public g_bCanSlewAltAzAsync As Boolean
Public g_bCanSlewAsync As Boolean
Public g_bCanSOP As Boolean
Public g_bCanSync As Boolean
Public g_bCanSyncAltAz As Boolean
Public g_bCanTrackingRates As Boolean
Public g_bCanUnpark As Boolean
Public g_bDualAxisPulseGuide As Boolean
Public g_iNumMoveAxis As Integer
Public g_bV1 As Boolean

' -----------------------------
' State Variables (persistent)
' -----------------------------

' GUI controls
Public g_bAlwaysTop As Boolean              ' True to keep sim topmost window
Public g_bSetupAdvanced As Boolean         ' True to display advanced options in setup window

' Optics
Public g_dAperture As Double
Public g_dApertureArea As Double
Public g_dFocalLength As Double

' Geography
Public g_dLatitude As Double                ' Site Latitude
Public g_dLongitude As Double               ' Site longitude
Public g_dSiteElevation As Double           ' Site elevation
Public g_dParkAltitude As Double            ' park Altitude
Public g_dParkAzimuth As Double             ' park Azimuth

' other changable state
Public g_eAlignMode As AlignmentModes       ' Alignment mode
Public g_bAutoTrack As Boolean              ' Auto unpark/track on startup
Public g_bDiscPark As Boolean               ' Disconnect on park
Public g_bNoCoordAtPark As Boolean          ' Coordinates will be invalid at park
Public g_dDateDelta As Double               ' difference for scope time
Public g_bDoRefraction As Boolean           ' do refraction correction
Public g_eEquSystem As EquatorialCoordinateType

' ---------------------------------
' State Variables (not persistent)
' ---------------------------------
Public g_dAltitude As Double                ' Altitude
Public g_dAzimuth As Double                 ' Azimuth
Public g_dRightAscension As Double          ' RA
Public g_dDeclination As Double             ' Dec
Public g_dTargetRightAscension As Double    ' Target RA
Public g_dTargetDeclination As Double       ' Target Dec

Public g_bConnected As Boolean              ' Whether scope is connected
Public g_bTracking As Boolean               ' Whether scope tracking (equatorial drive)
Public g_bAtPark As Boolean                 ' Parked state
Public g_bAtHome As Boolean                 ' Homed state
Public g_SOP As PierSide                    ' Scope side for GEM's

Public g_AxisRates As AxisRates             ' Collection of supported axis rates
Public g_AxisRatesEmpty As AxisRates        ' Empty Collection of drive rates
Public g_dDeclinationRate As Double
Public g_dRightAscensionRate As Double
Public g_dGuideRateDeclination As Double
Public g_dGuideRateRightAscension As Double
Public g_eTrackingRate As DriveRates        ' Current drive rate
Public g_TrackingRatesFull As TrackingRates ' Collection of supported drive rates
Public g_TrackingRatesSimple As TrackingRates   ' Simple one rate collection

'--------------------------------------
' Slew simulation (rate is persistent)
'--------------------------------------
Public g_dDeltaAz As Double                 ' Distance remaining to slew
Public g_dDeltaAlt As Double
Public g_dDeltaRA As Double
Public g_dDeltaDec As Double
Public g_dSlewRateFast As Double            ' Fast slew rate (actually delta angle/timer tick)
Public g_dSlewRateMed As Double             ' Medium slew rate
Public g_dSlewRateSlow As Double            ' Slow slew rate
Public g_Slewing As slewType                ' Slew in progress
Public g_lSlewSettleTime As Long            ' Slew settle time, millisec
Public g_lsettleTix As Long                 ' current end of settle
Public g_lPulseGuideTixDec As Long          ' current end of pulse guide
Public g_lPulseGuideTixRA As Long           ' current end of pulse guide
