Attribute VB_Name = "Public"
' -----------------------------------------------------------------------------
'   ============
'   Public.BAS
'   ============
'
' Plain old telescope hub public definitions and variables
'
' Written: Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 22-Mar-03 jab     Initial edit
' 07-Sep-03 jab     Beta release - much more robust, getting ready for V2
' 25-Sep-03 jab     Separated out of Startup.bas
' 25-Sep-03 jab     Finished new V2 spec support (V2 definition changed)
' 10-Jan-06 dpp     Focuser implementation
' 31-Aug-06 jab     added focuser gui state so had to change RegVer to 4.7
' 09-Sep-06 jab     added more focuser state to registry - bumped RegVe to 4.8
' 04-Jun-07 jab     Eliminate g_bRunExecutable, use App.StartMode directly, to
'                   avoid startup problems with refactored initialization.
' -----------------------------------------------------------------------------

Option Explicit

'-----------------------------------
' startup / shutdown state managment
'-----------------------------------

Enum ComponentStates
    csLoaded = 0
    csStarting = 1
    csRunning = 2
End Enum

'-----------
' constants
'-----------

' conversions
Public Const DEG_RAD As Double = 0.0174532925
Public Const RAD_DEG As Double = 57.2957795
Public Const HRS_RAD As Double = 0.2617993881
Public Const RAD_HRS As Double = 3.81971863

' naming
Public Const ID As String = "POTH.Telescope"
Public Const IDDOME As String = "POTH.Dome"
Public Const IDFOCUSER As String = "POTH.Focuser"
Public Const DESC As String = "POTH (Plain Old Telescope Handset)"

' registry version - only change if registry needs to change
' in an incompatible way - do not track program version
Public Const RegVer As String = "5.2"

' parameter management constants
Public Const INVALID_PARAMETER As Double = -10000#
Public Const EMPTY_PARAMETER As Double = -20000#
Public Const ALG_UNKNOWN As Double = -1

' timer interval
Public Const TIMER_INTERVAL = 1#           ' main ticker in sec per cycle

Public g_Util As DriverHelper.Util

'---------------------
' API timer functions
'---------------------

Declare Function SetTimer Lib "user32" _
       (ByVal hwnd As Long, _
       ByVal nIDEvent As Long, _
       ByVal uElapse As Long, _
       ByVal lpTimerFunc As Long) As Long

Declare Function KillTimer Lib "user32" _
       (ByVal hwnd As Long, _
       ByVal nIDEvent As Long) As Long

' ------------------------
' connectivity management
' ------------------------

Public g_Profile As DriverHelper.Profile
Public g_sScopeID As String
Public g_sScopeName As String
Public g_Scope As Object
Public g_IScope As ITelescope
Public g_bForceLate                         ' Force classic late binding

Public g_DomeProfile As DriverHelper.Profile
Public g_sDomeID As String
Public g_sDomeName As String
Public g_Dome As Object
Public g_IDome As IDome

Public g_FocuserProfile As DriverHelper.Profile
Public g_sFocuserID As String
Public g_sFocuserName As String
Public g_Focuser As Object
Public g_IFocuser As IFocuser

Public g_ComponentState As ComponentStates
Public g_iConnections As Integer
Public g_iDomeConnections As Integer
Public g_iFocuserConnections As Integer

'----------------
' timer controls
'----------------

Public g_ltimerID As Long                   ' timer ID
Public slave_time As Double                 ' dome slave check counter
Public slave_time_reset As Double           ' reset value for dome slave check

' ----------------------
' Scope variables
' ----------------------

Public g_AxisRatesEmpty As AxisRates        ' Empty Collection of drive rates
Public g_TrackingRatesSimple As TrackingRates   ' Simple one rate collection

' refraction state enumeration
Public Enum Refraction
    refUnknown = 0
    refYes = 1
    refNo = 2
End Enum

' tracking commands for internal state management
Public Enum TrackCommand
    trackClear = 0
    trackInitial = 1
    trackRead = 2
End Enum

' scope motion management
Public g_bSlewing As Boolean
Public g_bAsyncSlewing As Boolean
Public g_bMonSlewing As Boolean
Public g_bTracking As Boolean               ' Whether scope tracking (equatorial drive)
Public g_bQuiet As Boolean
Public g_bBacklash As Boolean               ' Backlash removal for short slews
Public g_bSimple As Boolean                 ' Only use simple commands
Public g_bAtHome As Boolean
Public g_bAtPark As Boolean
Public g_SOP As PierSide

' scope state
Public g_iVersion As Integer
Public g_eAlignMode As AlignmentModes       ' Alignment mode
Public g_dAperture As Double                ' in meters
Public g_dApertureArea As Double            ' in meters
Public g_eDoesRefraction As Refraction
Public g_eEquSystem As EquatorialCoordinateType
Public g_bAutoUnpark As Boolean             ' conditional support of V2 semantics
Public g_dFocalLength As Double             ' in meters
Public g_dLongitude As Double               ' Site longitude
Public g_dLatitude As Double                ' Site Latitude
Public g_dElevation As Double               ' Site elevation in meters
Public g_dRightAscension As Double          ' RA
Public g_dDeclination As Double             ' Dec
Public g_dAltitude As Double                ' Altitude
Public g_dAzimuth As Double                 ' Azimuth
Public g_dTargetRA As Double
Public g_dTargetDec As Double
Public g_bConnected As Boolean              ' Whether scope is connected
Public g_lSlewSettleTime As Long            ' Slew settle time, millisec
Public g_lPulseGuideTix As Long             ' current end of pulse guide
Public g_dMeridianDelay As Double           ' hour to delay where flip occurs
Public g_dMeridianDelayEast As Double       ' hour to delay where flip occurs

' scope geometry wrt dome
Public g_dRadius As Double                  ' dome radius in m
Public g_iPosEW As Integer                  ' scope offset in mm
Public g_iPosNS As Integer                  ' scope offset in mm
Public g_iPosUD As Integer                  ' scope offset in mm
Public g_iGEMOffset As Integer              ' GEM axis offset
Public g_iSlop As Integer                   ' precision for dome moves

' simulated "can" flags based on first trial uses
Public g_bCanAlignMode As Boolean
Public g_bCanAltAz As Boolean
Public g_bCanDateTime As Boolean
Public g_bCanDoesRefraction As Boolean
Public g_bCanElevation As Boolean
Public g_bCanEqu As Boolean
Public g_bCanEquSystem As Boolean
Public g_bCanLatLong As Boolean
Public g_bCanOptics As Boolean
Public g_bCanSideOfPier As Boolean
Public g_bCanSiderealTime As Boolean

' real can flags
Public g_bCanFindHome As Boolean
Public g_bCanPark As Boolean
Public g_bCanPulseGuide As Boolean
Public g_bCanSetDeclinationRate As Boolean
Public g_bCanSetGuideRates As Boolean
Public g_bCanSetPark As Boolean
Public g_bCanSetPierSide As Boolean
Public g_bCanSetRightAscensionRate As Boolean
Public g_bCanSetTracking As Boolean
Public g_bCanSlew As Boolean
Public g_bCanSlewAltAz As Boolean
Public g_bCanSlewAltAzAsync As Boolean
Public g_bCanSlewAsync As Boolean
Public g_bCanSync As Boolean
Public g_bCanSyncAltAz As Boolean
Public g_bCanUnpark As Boolean

'----------------
' dome variables
'----------------

' dome motion management
Public g_bDomeConnected As Boolean         ' Whether dome is connected
Public g_bSlaved As Boolean
Public g_bSlaveSlew As Boolean             ' Slave slew in progress (don't bounce dome)

' dome state
Public g_dDomeAltitude As Double            ' Dome Altitude
Public g_dDomeAzimuth As Double             ' Dome Azimuth

' dome "can" flags
Public g_bDomeFindHome As Boolean
Public g_bDomePark As Boolean
Public g_bDomeSetAltitude As Boolean
Public g_bDomeSetAzimuth As Boolean
Public g_bDomeSetPark As Boolean
Public g_bDomeSetShutter As Boolean
Public g_bDomeSyncAzimuth As Boolean

'-----------------------
' focuser variables
'-----------------------

' focuser motion management
Public g_bFocuserConnected As Boolean       ' Whether focuser is connected

' focuser state
Public g_lFocuserPosition As Long
Public g_dFocuserTemperature As Double
Public g_bFocuserTempComp As Boolean
Public g_dFocuserStepSizeInMicrons As Double
Public g_lFocuserIncrement As Long
' focuser "can" flags and max value
Public g_bFocuserTempProbe As Boolean
Public g_bFocuserTempCompAvailable As Boolean
Public g_bFocuserAbsolute As Boolean
Public g_bFocuserHalt As Boolean
Public g_bFocuserStepSize As Boolean
Public g_bFocuserSynchronous As Boolean
Public g_lFocuserMaxStep As Long
Public g_lFocuserMaxIncrement As Long
Public g_bFocuserMoveMicrons As Boolean
Public g_bFocuserAbsMove As Boolean

' --------------------------
' GUI handles and variables
' --------------------------

Public g_setupDlg As frmSetup              ' Setup dialog
Public g_handBox As frmHandBox             ' Hand box
Public g_show As frmShow                   ' Traffic window

Public g_bSetupAdvanced As Boolean         ' True to display advanced options in setup window
Public g_bDomeMode As Boolean
Public g_bFocusMode As Boolean
Public g_bMotionControl As Boolean
Public g_bHAMode As Boolean

