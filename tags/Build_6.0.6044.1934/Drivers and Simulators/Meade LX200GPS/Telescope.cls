VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Telescope"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ASCOM Telescope driver for Meade LX200 and Autostar"
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
'   =============
'   TELESCOPE.CLS
'   =============
'
' Implementation of ASCOM Meade telescope driver Telescope class
'
' Written:  22-Aug-00   Robert B. Denny <rdenny@dc3.com>
' Aditions: 02-Feb-02   Chris Creery <ccreery@cyanogen.com>
'           21-Apr-02   Larry Weber
'           2003        Jon Brewster and Bob Denny, various (see edit trails)
'           2005        Bob Denny - LX200GPS, various changes, see edit trails
'
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 22-Aug-00 rbd     Initial edit
' 28-Aug-00 rbd     Integration testing completed. Add setup dialog.
' 14-Oct-00 rbd     Slew settling time
' 15-Oct-00 rbd     Fix Com Port registry setting name
' 13-Nov-00 rbd     Description returns String not Double, Park() does
'                   not take a command. ByVal on parameters and Lets.
'                   Allow Description when not connected.
' 14-Nov-00 rbd     TargetDeclination used HoursToHMS to format for
'                   Autostar, which barfs if there are spaces in the
'                   sexagesimal (DegreesToDMS generates spaces).
' 21-Nov-00 rbd     1.0.3 - DriverInfo is a String. Add DriverInfo
'                   code to take from project properties.
' 11-Dec-00 rbd     1.0.4 - For Autostar, increase timeout to 10 sec.
'                   Add a 1 sec delay to AbortSlew to give Autostar
'                   time to digest it. I suspect a problem with it.
'                   Remove code to stop in-progress slew when starting
'                   new one. Too slow on Autostar. Can't abort slew
'                   then start new one in Autostar anyway, it goes
'                   wild.
' 12-Dec-00 rbd     The @#$%^& Autostar's 2.0i revision broke the
'                   long format for setting target RA! And switching
'                   between short and long format in the Autostar
'                   is time-sensitive, requiring a delay. As a result,
'                   I had to change this to run the Autostar in SHORT
'                   format. ARGH!
' 13-Jan-01 rbd     Fill in missing ERR_SOURCE argument in calls to
'                   Err.Raise for "not implemented". There were many!
'                   Specific messages for not implemented.
' 21-Jan-01 rbd     New Helper.Profile object, remove Registry.bas.
'                   No more General subkey, simplify registry logic.
'                   No more Win32 here, call new Util.LoadDLL() and
'                   Util.WaitForMilliseconds()
' 25-Jan-01 rbd     New properties CanSlew, CanSlewAsync, CanSync,
'                   CanPark, CanFindHome, CanSetPark, CanUnpark
'                   Interface V1.1
' 28-Jan-00 rbd     Add description and version info display to
'                   SetupDialog box.
' 06-Feb-01 rbd     Add Tracking, CanSetTracking
' 08-Jun-01 rbd     New Profile.DeviceType to generalize Chooser and
'                   Profile.
' 08-Jun-01 rbd     Telescope 1.4: Add PulseGuide() & CanPulseGuide
' 11-Jun-01 rbd     Make it possible to call Unpark() even if never
'                   parked. Unpark() in this case is a no-op. Add
'                   code to set slow max-slew speed. This is really
'                   essential to avoid premature drive wear in
'                   automated operations.
' 05-Oct-01 rbd     1.4.2 Add ByVals where missing.
' 08-Oct-01 rbd     1.4.3 Change MSG_UNKNOWN_MEADE to refer to ASCOM
'                   web site.
' 12-Oct-01 rbd     1.4.4 Fix slew settle time > 30 sec.
' 02-Nov-01 rbd     1.4.5 Flex delimiters, for variations in Autostar
' 07-Nov-01 rbd     1.4.6 No decimal digits on minutes for Autostar
'                   (2.2 Autostar firmware rejects them)
' 05-Dec-01 rbd     1.4.7 Add Property Let for Lat/Long and UTCDate
'                   (LX200 only, Autostar does not recompute w/o
'                   handbox alignment).
' 04-May-02 rbd     1.5.1 For the LX200 GPS. This is a beast which
'                   has the characteristic of freezing for 500 or
'                   or more milliseconds every 1500 or so! Also, it
'                   doesn't implement distance bars. Rather than
'                   go through all the testing I did for the LX200
'                   classic and Autostar I, I implemented the slewing
'                   detection using RA/Dec watching for only the GPS.
'                   Some time, the distance bar logic should just be
'                   removed, using the ra/dec watching logic for all
'                   scope models. ** INCREASE SCOPE TIMEOUT TO 45
'                   SECONDS FOR AUTOSTAR II and SLOW SERIAL CAMS.
'                   Remove leading # from command strings. This was
'                   supposed to make the LX200 Classic forget about
'                   old trash in its buffer. I don't believe it is
'                   really needed, and it makes the LX200GPS deaf to
'                   commands under certain (unclear) circumstances.
' 07-May-02 rbd     1.8.2 Merge Chris Creery and Larry Weber's
'                   additions for focuser control into this. Version
'                   jumped to 1.8.x per incoming driver from Creery.
' 18-Jun-02 rbd     1.8.4 Change to out-of-proc server to support
'                    ownership of Telescope and Focuser by
'                    different processes. Add logic to prevent
'                    creating multiple instances of interfaces.
' 18-Jun-02 rbd     1.8.4 Change OpenConnection() and CloseConnection()
'                   See comments in Common module. Initial GS test
'                   now sends leading # as required by LX200 GPS
'                   moved to Common module. Change interface to Setup
'                   dialog. It knows where to store stuff now.
' 23-Jul-02 rbd     1.8.7 - Improve "Invalid Object Use" message.
' 15-Aug-02 rbd     2.0.1 - GPS now has distance bars. Remove old (and
'                   flaky) motion method of slew detection.
' 27-Sep-02 rbd     2.1.0 - Fix get_Telescope.Connected per Larry
'                   Weber. Update Min version for 1.3t.
' 04-Dec-02 rbd     2.1.1 - Remember and restore initial state of
'                   short/long mode in scope.
' 04-Jan-03 rbd     2.1.2 - Fix Get_SiderealTime() to return hours
'                   not days, operating serial timeout now 30 sec.
' 08-Mar-03 jab     get tracking now checks parked status
' 08-Mar-03 jab     get aperture, focal length, and elevation working
' 08-Mar-03 jab     slewing was uninitialized, and park flag wasn't
'                   always set right
' 09-Mar-03 jab     going to Land takes time next command was being
'                   swallowed, added 1 sec delay
' 11-Mar-03 jab     Allow elevation to default to 0 if no user input
' 12-Mar-03 jab     unpark change mode takes time, and needs a wait
' 21-Mar-03 rbd     2.2.1 - Version changed for Platform 2.2
' 17-May-03 rbd     2.2.2 - Now requires 1.6b of AutoStar II (GPS).
'                   Fix version matcher to use real last character of
'                   MIN_VER_STRING instead of always 'o'.
'                   Shorten extra wait to 1 sec got GPS. It seems to
'                   give a proper indication with the dist. bars.
' 06-Jun-03 rbd     2.2.3 - Suspend settle time when parking Classic.
' 24-Jun-03 rbd     2.3.1 - Version change for Platform 2.3
' 09-Aug-03 rbd     2.3.2 - Parking Classic for southern hemi
' 03-Sep-03 rbd     3.0.1 - Version for Platform 3.0, try long format
'                   on Autostar I, more parking
' 09-Sep-03 rbd     3.0.2 - Long format on Autostar 1
' 28-sep-03 jab     added control of auto unpark
' 25-Oct-o3 jab     setup dialog now works while connected
' 03-Jan-04 jab     convert all Alt/Az coordinates based on polar or alt/az
'                   mounting prior to sending through as target coords
' 20-Oct-04 jab     Use the undocumented Mgxddd command for GPS pulseguide
' 23-Nov-04 rbd     4.0.1 - Version change for Platform 4.0
' 03-Jan-05 rbd     4.1.0 - New LX200GPS startup code for connecting
'                   to parked scope. Move buffer-clear on startup to
'                   AFTER sending the "butt-kick" to GPS. Add wait
'                   loop for "I" in Unpark(), new reboot-fail error.
' 22-Feb-05 rbd     4.1.1 - Skip Auto Set Time for Autostar 495/497
' 20-Sep-05 rbd     4.1.2 - Rework initialization for LX200GPS, require
'                   firmware 3.0i. Comment out all jab BreadCrumb
'                   registry writes to minimize needless registry I/O.
'                   Cleanups on SetupDialog form. LX200GPS 3.0i does
'                   support setting UTCDate, so we can enable set-time
'                   on connect. This makes it possible to init the scope
'                   without the HBX. There's much more to init...
'                   LX200GPS now takes Longitude with same sign convention
'                   (reversed from normal) as the LX200 Classic. Much
'                   of the logic in this driver is a mystery to me!!!
'                   Enable tracking control for LX200GPS, LAND mode
'                   now works (3.0i). Save Alt/Az at time tracking is
'                   turned off, then use this to calculate RA/Dec.
'                   This works for both the Classic and GPS and the GPS
'                   returns wrong coords while in LAND mode. So this
'                   is a universal solution (Autostar I has no tracking
'                   control).
' 24-Sep-05 rbd     4.1.3 - Correct tooltip on setup dialog. Add new
'                   undocumented commands to step past the daylight
'                   savings, time, and date prompts after handbox
'                   reboot. GUESS WHAT? HANDS-OFF STARTUP!
' 17-Sep-05 rbd     4.1.4 - Increase delay between EK13s to 2 sec (per
'                   Widener/Schultz problems)
' 28-Sep-05 rbd     4.1.5 - Reduce serial timeout when disconnecting
'                   to avoid long delay with client's UI dead. Remove
'                   setting timeout to 30 sec before setting long
'                   format. WHY WAS THIS THERE???
' 10-Oct-05 rbd     4.1.6 - New Autostar II firmware, takes much more
'                   tiome to initialize. Extend timeout to 240 sec
'                   and make it wait 15 sec initially after "I"
'                   command. Fix problem where Auto Set Time = 1 caused
'                   bogue "enter" commands to be sent to the scope.
' 11-Oct-05 rbd     4.1.7 - More fixes for auto-set-time. Look at
'                   autostar display to be sure it says Daylight
'                   Savings before sending the three enter commands.
'                   Verified with LX200GPS 4.0h firmware. Still usable
'                   with 3.0i. Honor AutoUnpark/Track for LX200GPS for
'                   tracking at least! If not checked, connect returns
'                   with tracking off (since we can now control it!)
' 13-Oct-05 rbd     4.1.8 - Add AutoReboot switch for LX200GPS, avoids
'                   long delays for other scope types if connecting to
'                   nothing or a dead scope.
' 27-Dec-05 rbd     4.1.9 - Make receive timeout a constant, increase
'                   to 5 seconds for Ray Gralak's PEMPro.
' 29-Jun-06 rbd     4.1.10..12 - Add logic to confirm a sync by reading
'                   back the actual coordinates till they match the
'                   target coordinates. Don't return till this
'                   condition is met.
' 07-Aug-06 rbd     4.1.13 - Add max slew rate setting, optional sync
'                   sync delay.
' 12-Aug-06 jab     set tracking now returns immediatly if there is no
'                   attempted state change no matter what parking or
'                   sleeping states say
' 14-Sep-06 jab     added Fix() around mdy_vb() to filter out extra daylight
'                   savings time offset
' 15-Sep-06 jab     GPS will no long report True on CanUnpark
' 15-Sep-06 jab     Pealed out the LX200GPS into its own driver
' 16-Sep-06 jab     Leave tracking state along on disconnect, we used to
'                   turn on tracking!
' 16-Sep-06 jab     Alt / Az / RA / Dec reporting all updated to be acurate
'                   independent of tracking state.
' 21-Sep-06 jab     Moved InvalidCoords to a global called g_bBusy
'                   so that focuser can participate
' 04-Oct-06 jab     Change mount type and tracking state control to use
'                   "GW" command.  Old ^f technique now fails.
' 07-Oct-06 jab     added code to make park synchronous
' 13-Oct-06 jab     expanded version regular expresion to cover patch kits
' 04-Jan-06 jab     don't check slewing faster than every 500ms
' 09-Jun-07 jab     Added ITelescope, took away standard Enums
'---------------------------------------------------------------------

Option Explicit
Implements ITelescope

Private Const MIN_VERSION As Single = 4#
Private Const MIN_VER_STRING As String = "4.2g"
Private Const SIDRATE As Long = 0.9972695677   ' synodic/solar seconds per sidereal second

' conversions
Private Const DEG_RAD As Double = 0.0174532925
Private Const RAD_DEG As Double = 57.2957795
Private Const HRS_RAD As Double = 0.2617993881
Private Const RAD_HRS As Double = 3.81971863

Private Const INVALID_PARAMETER As Double = -10000#

Private ConnectState As String              ' connect process tracking for debug
Private m_bSyncDelay As Boolean

Private m_AxisRates As AxisRates
Private m_TrackingRates As TrackingRates

Private m_eTrackingRate As DriveRates
Private m_eAlignMode As AlignmentModes
Private m_bSlewing As Boolean
Private m_bTracking As Boolean
Private m_bAbortSlew As Boolean
Private m_bParked As Boolean
Private m_lSlewCheckTix As Long
Private m_lAsyncEndTix As Long
Private m_iRaBar As Integer
Private m_iDecBar As Integer
Private m_bLongFormat As Boolean
Private m_bOrigLongFormat As Boolean
Private m_dDecRate As Double
Private m_dRARate As Double
Private m_bOffsetTracking As Boolean

Private m_sRADelimHrs As String
Private m_sRADelimMin As String
Private m_sRADelimSec As String
Private m_sDecDelimDeg As String
Private m_sDecDelimMin As String
Private m_sDecDelimSec As String
Private m_sAzDelimDeg As String
Private m_sAzDelimMin As String
Private m_sAzDelimSec As String

Private m_Util As DriverHelper.Util
Private m_Profile As DriverHelper.Profile
Private m_iSettleTime As Integer

Private m_dAlt As Double
Private m_dAz As Double
Private m_dRA As Double
Private m_dDec As Double
Private m_dTargetRA As Double   ' monitored for read before write
Private m_dTargetDec As Double  ' monitored for read before write
Private m_dLat As Double        ' cached since we use it a lot
Private m_dLong As Double       ' cached since we need it when we're asleep
Private m_dGuideRate As Double  ' used for both Dec and RA
'
' Locally used Win32 API
'
Private Declare Function GetTickCount Lib "kernel32" () As Long

Private Sub Class_Initialize()

    Dim buf As String

    If g_bTelescopeCreated Then _
        Err.Raise 425, App.Title, "Invalid object use: Only one program can use this Telescope."  ' Prevent multiple use
    g_bTelescopeCreated = True
    
    Set m_Util = New DriverHelper.Util
    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Telescope"              ' We're a Telescope driver
    
    ' Assure this is loaded and available
    m_Util.LoadDLL App.Path & "\astro32.dll"
    
    m_iSettleTime = 0
    m_bParked = False
    m_bSlewing = False
    m_bTracking = False
    m_bAbortSlew = False
    g_bBusy = False
    g_bAsleep = False
    m_dRARate = 0#
    m_dDecRate = 0#
    m_bOffsetTracking = False
    
    ConnectState = vbCrLf & vbCrLf
    
End Sub

Private Sub Class_Terminate()
    
    On Error Resume Next
    Me.Connected = False                            ' Force disconnect
    g_bTelescopeCreated = False
    g_bBusy = False
    
End Sub

Public Property Get AlignmentMode() As AlignmentModes

    CheckConnected False, m_bTracking, False
    
    AlignmentMode = m_eAlignMode
    
End Property

Public Property Get Altitude() As Double

    Dim Alt As Double
        
    CheckConnected False, m_bTracking, False
    
    ' if scope is parked or indeterminate, return pre-stored value
    If m_bParked Or g_bAsleep Or g_bBusy Then
        If m_dAlt = INVALID_PARAMETER Then _
            Err.Raise SCODE_ASLEEP, ERR_SOURCE, MSG_ASLEEP
        
        Altitude = m_dAlt
        Exit Property
    End If
        
    Alt = m_Util.DMSToDegrees(Me.CommandString("GA"))
 
    m_dAlt = Alt
    Altitude = Alt

End Property

Public Property Get ApertureArea() As Double

    Dim buf As String
    Dim testVal As Double
    
    buf = m_Profile.GetValue(SCOPE_ID, "ApertureArea")
    
    ' make sure our error gets thrown, not val's
    On Error Resume Next
        testVal = -1
        If buf <> "" Then _
            testVal = val(buf)
    On Error GoTo 0

    If testVal <= 0 Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
                "Property ApertureArea" & MSG_PROP_RANGE_ERROR
                
    ApertureArea = testVal
            
End Property

Public Property Get ApertureDiameter() As Double

    Dim buf As String
    Dim testVal As Double
    
    buf = m_Profile.GetValue(SCOPE_ID, "Aperture")
    
    ' make sure our error gets thrown, not CDbl's
    On Error Resume Next
        testVal = -1
        If buf <> "" Then _
            testVal = val(buf)
    On Error GoTo 0

    If testVal <= 0 Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
                "Property ApertureDiameter" & MSG_PROP_RANGE_ERROR
                
    ApertureDiameter = testVal

End Property

Public Property Get AtHome() As Boolean

    AtHome = False
    
End Property

Public Property Get AtPark() As Boolean
   
    CheckConnected False, m_bTracking, False
    
    AtPark = m_bParked
    
End Property

Public Property Get Azimuth() As Double
    
    Dim Az As Double
    
    CheckConnected False, m_bTracking, False
    
    ' if scope is parked or indeterminate, return pre-stored value
    If m_bParked Or g_bAsleep Or g_bBusy Then
        If m_dAz = INVALID_PARAMETER Then _
            Err.Raise SCODE_ASLEEP, ERR_SOURCE, MSG_ASLEEP
        
        Azimuth = m_dAz
        Exit Property
    End If
    
    Az = m_Util.DMSToDegrees(Me.CommandString("GZ"))
    
    m_dAz = Az
    Azimuth = Az
    
End Property

Public Property Get CanFindHome() As Boolean

    CanFindHome = False

End Property

Public Property Get CanPark() As Boolean
    
    CanPark = True
    
End Property

Public Property Get CanPulseGuide() As Boolean

    CanPulseGuide = True
    
End Property

Public Property Get CanSetDeclinationRate() As Boolean
    
    CanSetDeclinationRate = False

End Property

Public Property Get CanSetGuideRates() As Boolean

    ' claims to work, more testing needed before we declare victory ???
    CanSetGuideRates = True

End Property

Public Property Get CanSetPark() As Boolean

    CanSetPark = False

End Property

Public Property Get CanSetPierSide() As Boolean

    CanSetPierSide = False
    
End Property

Public Property Get CanSetRightAscensionRate() As Boolean
    
    CanSetRightAscensionRate = False

End Property


Public Property Get CanSetTracking() As Boolean
    
    CanSetTracking = True

End Property

Public Property Get CanSlew() As Boolean

    CanSlew = True
    
End Property

Public Property Get CanSlewAltAz() As Boolean
    
    CanSlewAltAz = True
        
End Property

Public Property Get CanSlewAltAzAsync() As Boolean
    
    CanSlewAltAzAsync = True
        
End Property

Public Property Get CanSlewAsync() As Boolean
    
    CanSlewAsync = True
    
End Property

Public Property Get CanSync() As Boolean

    CanSync = True
    
End Property

Public Property Get CanSyncAltAz() As Boolean

    CanSyncAltAz = False
    
End Property

Public Property Get CanUnpark() As Boolean

    CanUnpark = True
    
End Property

Public Property Get Connected() As Boolean

    Connected = g_bTelescopeConnected
    
End Property

Public Property Let Connected(ByVal newVal As Boolean)
    Dim buf As String, sRA As String, sDec As String, sAz As String
    Dim rx As RegExp
    Dim mt As MatchCollection
    Dim tmo As Long
    Dim ver As Single, rev As String
    Dim Alt As Double
    Dim Az As Double
    Dim GPSRebooted As Boolean
    Dim I As Integer
    Dim DoReboot As Boolean
    
    ConnectState = "  in connect" & vbCrLf
    
    ' don't try to change state if already there
    If newVal = g_bTelescopeConnected Then _
        Exit Property
        
    If newVal Then                                      ' CONNECTING
            
        Set rx = New RegExp                             ' We use this in a couple of places
        GPSRebooted = False                             ' Assume we aren't rebooting a GPS
        '
        ' (1) Set up the communications link. Default to COM1, set 5 sec. timeout
        '
        ConnectState = ConnectState & "  opening port, sending a # to wake things up" & vbCrLf
        OpenConnection devTelescope                     ' Open port and setup serial object
        '
        ' (2) First try to read the Sidereal Time. If nothing comes back,
        '     try treating it as an LX200GPS/R if AutoReboot flag is set.
        '     and try rebooting it. If it succeeds (no error raised) then
        '     set the GPSRebooted flag, which will force the date/time
        '     to be set below, regardless of the auto set time setting.
        '     This is needed because the LX200GPS does not have a live
        '     clock (that runs when the power is off or when parked).
        '
        DoReboot = (m_Profile.GetValue(SCOPE_ID, "AutoReboot") = "1")
        ConnectState = ConnectState & "  sending first GS" & vbCrLf
        g_SharedSerial.ClearBuffers
        g_SharedSerial.Transmit ":GS#"                  ' Try for the Sidereal Time
        On Error Resume Next
        Call g_SharedSerial.ReceiveTerminated("#")
        If Err.Number <> 0 Then                         ' No response
            On Error GoTo 0
            If DoReboot Then
                RebootGPS
                ConnectState = ConnectState & "  got nothing back, attempting reboot" & vbCrLf
                GPSRebooted = True                      ' We rebooted, need date/time below!
            Else
                GoTo NO_SCOPE                           ' Dead line
            End If
        End If
        On Error GoTo 0
        g_bAsleep = False                               ' Just in case...
        
        ' if we make it to here, the scope is up and running

        '
        ' (4) Try a version handshake. This works only on the Autostar I/II.
        '     Depending on the response (or lack thereof), determine the
        '     model of LX200. If it's a GPS/R, require a "recent enough" version
        '     of the firmware. Receive timeout is still 5 seconds
        '
        On Error Resume Next                            ' -- error trap --
            buf = Me.CommandString("GVF")               ' Read version
        On Error GoTo 0                                 ' -- end trap --
        If Err Then
            BadScopeType                                ' not an Autostar II
            Exit Property
        End If
        
        If InStr(buf, "Autostar") > 0 Then              ' Is it an Autostar I?
            BadScopeType
            Exit Property
        ElseIf Left$(buf, 7) = "Version" Then           ' Autostar II
            
            On Error Resume Next                        ' At least TRY this stuff
                buf = Mid$(buf, 8)                      ' Version if any (n.nx)
                rx.Global = False                       ' Split number & letter
                rx.Pattern = "\s*([0-9]*)\D([0-9]*)([a-z]|[A-Z])"
                Set mt = rx.Execute(buf)
                ver = CSng(mt(0).SubMatches(0) & "." & mt(0).SubMatches(1))  ' Isolated version number
                rev = LCase(mt(0).SubMatches(2))        ' Revision letter
            On Error GoTo 0
            
            If Err.Number <> 0 Or rev = "" Then
                BadScopeType
                Exit Property
            End If
            
            If ver < MIN_VERSION Or _
                ((ver = MIN_VERSION) And Asc(rev) < Asc(Right$(MIN_VER_STRING, 1))) Then
                MsgBox "Your Autostar II firmware (" & buf & ") should be updated to " & _
                        MIN_VER_STRING & " or later. Some things may not work.", _
                        (vbOKOnly + vbExclamation + vbMsgBoxSetForeground), _
                        ERR_SOURCE
                ConnectState = ConnectState & "  have an old version" & vbCrLf
                '** Allow connection ???
                'CloseConnection devTelescope
                'Exit Property                           ' CONNECT FAILED!
            Else
                ' all good
                ConnectState = ConnectState & "  have a good version" & vbCrLf
            End If
        Else
            BadScopeType
            Exit Property
        End If

        g_SharedSerial.ClearBuffers
       
        '
        ' (6) Assure Long Format mode
        '
        m_bOrigLongFormat = IsLongFormat()              ' Remember initial format
        SetLongFormat True                              ' Switch to long
        '
        ' (7) Get the sexagesimal delimiters that this implementation uses
        '
        sRA = Me.CommandString("GR")
        m_Util.WaitForMilliseconds 200
        sDec = Me.CommandString("GD")
        m_Util.WaitForMilliseconds 200
        sAz = Me.CommandString("GZ")
        
        Set rx = New RegExp
        rx.Global = True
        rx.Pattern = "[\+\-]*[0-9\.]+([^0-9\.]+)[0-9\.]*([^0-9\.]*)[0-9\.]*([^0-9\.]*)"
        
        Set mt = rx.Execute(sRA)
        m_sRADelimHrs = mt(0).SubMatches(0)
        m_sRADelimMin = mt(0).SubMatches(1)
        m_sRADelimSec = mt(0).SubMatches(2)
        
        Set mt = rx.Execute(sDec)
        m_sDecDelimDeg = mt(0).SubMatches(0)
        m_sDecDelimMin = mt(0).SubMatches(1)
        m_sDecDelimSec = mt(0).SubMatches(2)
        
        Set mt = rx.Execute(sAz)                        ' apparently Az is different than Dec (jab)
        m_sAzDelimDeg = mt(0).SubMatches(0)
        m_sAzDelimMin = mt(0).SubMatches(1)
        m_sAzDelimSec = mt(0).SubMatches(2)
        '
        ' (8) Set up rates collections
        '
        Set m_AxisRates = New AxisRates                 ' empty for when no axis support
        Set m_TrackingRates = New TrackingRates
        m_TrackingRates.Add driveSidereal
        
        ' The GPS claim more support, but these commands just do not work (yet)
'            m_TrackingRates.Add driveLunar
'            m_TrackingRates.Add driveSolar

        '
        ' (9) Get the sync delay setting
        '
        m_bSyncDelay = (m_Profile.GetValue(SCOPE_ID, "SyncDelay") = "1")
        
        '
        ' (10) do shared stuff that unpark also does
        '
        Reconnect GPSRebooted
        
    Else                                                ' DISCONNECTING
        g_SharedSerial.ReceiveTimeout = RECEIVE_TIMEOUT ' Restore timeout for vaporized scope
        On Error GoTo SKIP_IT                           ' Best efforts...
        If g_SharedSerial.Connected Then
            SetLongFormat m_bOrigLongFormat
        End If
SKIP_IT:
        CloseConnection devTelescope                    ' Release port for telescope
    End If

    Exit Property
    
NO_SCOPE:

    CloseConnection devTelescope                        ' Release port for telescope
    Err.Raise SCODE_NO_SCOPE, ERR_SOURCE, MSG_NO_SCOPE

End Property

Public Property Get Declination() As Double

    Dim dec As Double
    
    CheckConnected False, m_bTracking, False
    
    ' if scope is parked or indeterminate, return pre-stored value
    If m_bParked Or g_bAsleep Or g_bBusy Then
        If m_dDec = INVALID_PARAMETER Then _
            Err.Raise SCODE_ASLEEP, ERR_SOURCE, MSG_ASLEEP
        
        Declination = m_dDec
        Exit Property
    End If
    
    dec = m_Util.DMSToDegrees(Me.CommandString("GD"))
    
    m_dDec = dec
    Declination = dec
 
End Property

Public Property Get DeclinationRate() As Double
    
    DeclinationRate = 0#
    
End Property

Public Property Let DeclinationRate(ByVal newVal As Double)
    
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
        "Property DeclinationRate" & MSG_NOT_IMPLEMENTED
        
End Property

Public Property Get Description() As String

    Description = "LX200GPS and LX200R"
    
    If g_SharedSerial.Connected Then
        Description = Description & " " & CommandString("GVF")
    End If
    
End Property

Public Property Get DoesRefraction() As Boolean
    
    DoesRefraction = True
            
End Property

Public Property Let DoesRefraction(ByVal newVal As Boolean)
        
    If Not newVal Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
            "Property DoesRefraction" & MSG_PROP_RANGE_ERROR
            
End Property

Public Property Get DriverInfo() As String
    Dim fs, F
    Dim DLM As String
    
    On Error Resume Next
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
        
    Set fs = CreateObject("Scripting.FileSystemObject")
    Set F = fs.GetFile(App.Path & "\" & App.EXEName & ".exe")
    DLM = F.DateLastModified
    DriverInfo = DriverInfo & vbCrLf & "Last Modified " & DLM
    
    On Error GoTo 0
    
End Property

Public Property Get DriverVersion() As String
    
    DriverVersion = App.Major & "." & App.Minor
        
End Property

Public Property Get EquatorialSystem() As EquatorialCoordinateType
      
    EquatorialSystem = equLocalTopocentric
    
End Property

Public Property Get FocalLength() As Double

    Dim buf As String
    Dim testVal As Double
    
    buf = m_Profile.GetValue(SCOPE_ID, "FocalLength")
    
    ' make sure our error gets thrown, not CDbl's
    On Error Resume Next
        testVal = -1
        If buf <> "" Then _
            testVal = val(buf)
    On Error GoTo 0
    
    If testVal <= 0 Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
                "Property FocalLength" & MSG_PROP_RANGE_ERROR
                
    FocalLength = testVal

End Property

Public Property Get GuideRateDeclination() As Double

    CheckConnected False, m_bTracking, False
    
    GuideRateDeclination = m_dGuideRate
    
End Property

Public Property Let GuideRateDeclination(ByVal newVal As Double)
                    
    Me.GuideRateRightAscension = newVal
    
End Property

Public Property Get GuideRateRightAscension() As Double

    CheckConnected False, m_bTracking, False

    GuideRateRightAscension = m_dGuideRate

End Property

Public Property Let GuideRateRightAscension(ByVal newVal As Double)

    Dim asVal As Double     ' newVal in arc sec
    
    CheckConnected False, m_bTracking, True
    
    asVal = newVal * 3600#
    
    ' The range check below is from a Meade document
    If asVal > 15.0417 Or asVal <= 0# Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
            "Property GuideRate" & MSG_PROP_RANGE_ERROR
    
    ' setup -> telescope -> guiderate should show a percentage that changes on handset
    ' 10 -> 66% which is the default
    Me.CommandBlind "Rg" & Format$(asVal, "00.0")
    
    m_dGuideRate = newVal
    
End Property

Public Property Get InterfaceVersion() As Integer

    InterfaceVersion = 2
        
End Property

Public Property Get IsPulseGuiding() As Boolean
      
    CheckConnected False, m_bTracking, False
    
    ' pulse guiding is synchronous for this driver
    IsPulseGuiding = False
        
End Property

Public Property Get Name() As String

    Name = "LX200GPS/R"

End Property

Public Property Get RightAscension() As Double

    Dim RA As Double
    
    CheckConnected False, m_bTracking, False
    
    ' if scope is parked or indeterminate, return pre-stored value
    If m_bParked Or g_bAsleep Or g_bBusy Then
        If m_dRA = INVALID_PARAMETER Then _
            Err.Raise SCODE_ASLEEP, ERR_SOURCE, MSG_ASLEEP
        
        ' if parked then stored m_dRA is really hour angle
        If m_bParked Then
            RA = RangeRA(now_lst(m_dLong * DEG_RAD) - m_dRA)
        Else
            RA = m_dRA
        End If
            
        RightAscension = RA
        Exit Property
    End If
    
    RA = m_Util.HMSToHours(Me.CommandString("GR"))
   
    m_dRA = RA
    RightAscension = RA
       
End Property

Public Property Get RightAscensionRate() As Double
    
    RightAscensionRate = 0#

End Property

Public Property Let RightAscensionRate(ByVal newVal As Double)
    
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
        "Property RightAscensionRate" & MSG_NOT_IMPLEMENTED
       
End Property

Public Property Get SideOfPier() As PierSide
    
    CheckConnected False, m_bTracking, False
    
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
        "Property SideOfPier" & MSG_NOT_IMPLEMENTED
        
End Property

Public Property Let SideOfPier(ByVal newVal As PierSide)
    
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
        "Property SideOfPier" & MSG_NOT_IMPLEMENTED
        
    ' but if we do ever implement, its defined as asynchronous
    
End Property

Public Property Get SiderealTime() As Double

    CheckConnected False, m_bTracking, True
    
    SiderealTime = CDbl(CDate(Me.CommandString("GS"))) * 24#

End Property

Public Property Get SiteElevation() As Double

    Dim buf As String
    Dim elev As Double
    
    buf = m_Profile.GetValue(SCOPE_ID, "SiteElevation")
    
    ' if not set at all, return 0
    If buf = "" Then
        elev = 0
    Else
        On Error Resume Next
            elev = -1000000    ' illigal number to catch failed convertion
            elev = val(buf)   ' don't allow conversion to error out
        On Error GoTo 0
        
        If elev = -1000000 Then _
            Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
                "Property Elevation" & MSG_PROP_RANGE_ERROR
    End If
    
    SiteElevation = elev

End Property

Public Property Let SiteElevation(ByVal newVal As Double)

    If newVal < -300 Or newVal > 10000 Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
            "Property SiteElevation " & MSG_PROP_RANGE_ERROR
            
    m_Profile.WriteValue SCOPE_ID, "SiteElevation", Str(newVal)

End Property

Public Property Get SiteLatitude() As Double

    CheckConnected False, m_bTracking, True
    
    SiteLatitude = m_Util.DMSToDegrees(Me.CommandString("Gt"))
    
    m_dLat = SiteLatitude   ' refresh the global, we could have just returned
                            ' the global, but this is more robust
    
End Property

Public Property Let SiteLatitude(ByVal newVal As Double)

    '
    ' Autostar II cannot recalculate its transform after alignment,
    ' thus changing the site location or time will invalidate its
    ' internal model.
    '
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
        "Property Let SiteLatitude (Autostar)" & MSG_NOT_IMPLEMENTED
    
End Property

Public Property Get SiteLongitude() As Double

    CheckConnected False, m_bTracking, True
    '
    ' Convert from 0-359 westward to +/- 180 +E
    '
    SiteLongitude = -m_Util.DMSToDegrees(Me.CommandString("Gg"))
    If SiteLongitude < -180# Then _
        SiteLongitude = SiteLongitude + 360#

    m_dLong = SiteLongitude   ' refresh the global, we could have just returned
                              ' the global, but this is more robust
                              
End Property

Public Property Let SiteLongitude(ByVal newVal As Double)

    '
    ' Autostar II cannot recalculate its transform after alignment,
    ' thus changing the site location or time will invalidate its
    ' internal model.
    '
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
        "Property Let SiteLongitude (Autostar I)" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get Slewing() As Boolean
    Dim buf As String, mvg As Boolean, tmo As Integer
    Dim rb As Integer, db As Integer, RA As Double, dec As Double
    
    CheckConnected False, m_bTracking, False
    
    ' This has a dual purpose: It not only returns to the caller
    ' the slewing status, but it also updates the end-interval
    ' ticks and implements the completion interval stuff. The
    ' m_bSlewing flag is not cleared until a set time after the
    ' scope distance bars indicate that the slew has completed.
    '
    If g_bAsleep Or Not m_bSlewing Then             ' Short-circuit when not slewing
        m_bSlewing = False
        Slewing = False
        Exit Property                               ' No slew now
    End If
    
    ' OK, we think we may still be slewing, but don't check any more often than
    ' 500ms as the scope gets confused
    If GetTickCount() - m_lSlewCheckTix < 500 Then
        Slewing = True
        Exit Property
    Else
        m_lSlewCheckTix = GetTickCount()            ' Mark time of this check
    End If
    
    buf = Trim$(Me.CommandString("D"))              ' Get distance bars
    If (buf <> "") Then                             ' Moving
        mvg = True
    Else                                            ' Not moving
        mvg = False
    End If
    
    If (Not m_bAbortSlew) And mvg Then
        m_lAsyncEndTix = GetTickCount() + (CLng(m_iSettleTime) * 1000&)
        m_lAsyncEndTix = m_lAsyncEndTix + 1000          ' Start with a minimum of 1 sec

        Slewing = True
    Else
        '
        ' This implements the trailing 4/11 seconds to allow vernier
        ' (skip it on a slew abort)
        '
        If (Not m_bAbortSlew) And (GetTickCount() < m_lAsyncEndTix) Then
            Slewing = True
        Else
            If Not m_bAbortSlew Then
                buf = m_Profile.GetValue(SCOPE_ID, "Beep")
                If buf = "1" Then _
                    Beep
            End If
            Slewing = False
            m_bAbortSlew = False
        End If
    End If
    
    m_bSlewing = Slewing                            ' Adjust our flag

End Property

Public Property Get SlewSettleTime() As Integer

    SlewSettleTime = m_iSettleTime
    
End Property

Public Property Let SlewSettleTime(ByVal newVal As Integer)

    If (newVal < 0) Or (newVal > 100) Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
            "Property SlewSettleTime " & MSG_PROP_RANGE_ERROR
        
    m_iSettleTime = newVal
    
End Property

' used for internal purposes only
Private Property Let TargetAltitude(ByVal newVal As Double)

    Dim sexa As String
    
'    CheckConnected False, m_bTracking, True
    
    If newVal < -90# Or newVal > 90# Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
            "Property TargetAltitude " & MSG_PROP_RANGE_ERROR
        
    If Not m_bLongFormat Then
        ' Short format, 1 decimal digit
        sexa = m_Util.DegreesToDM(newVal, m_sDecDelimDeg, m_sDecDelimMin, 1)
    Else
        sexa = m_Util.DegreesToDMS(newVal, m_sDecDelimDeg, _
            m_sDecDelimMin, m_sDecDelimSec) ' Long format, whole seconds
    End If
    If Left$(sexa, 1) <> "-" Then sexa = "+" & sexa         ' Both need leading '+'
    
    If Not Me.CommandBool("Sa " & sexa) Then
        Err.Raise SCODE_BAD_TARGET, ERR_SOURCE, MSG_BAD_TARGET
    End If
    
End Property

' used for internal purposes only
Private Property Let TargetAzimuth(ByVal newVal As Double)

    Dim sexa As String
    
    If newVal < 0# Or newVal >= 360# Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
            "Property TargetAzimuth " & MSG_PROP_RANGE_ERROR
         
    If Not m_bLongFormat Then
        ' Short format, 1 decimal digit
        sexa = m_Util.DegreesToDM(newVal, m_sAzDelimDeg, m_sAzDelimMin, 1)
    Else
        sexa = m_Util.DegreesToDMS(newVal, m_sAzDelimDeg, _
            m_sAzDelimMin, m_sAzDelimSec) ' Long format, whole seconds
    End If
    
    If Not Me.CommandBool("Sz " & sexa) Then
        Err.Raise SCODE_BAD_TARGET, ERR_SOURCE, MSG_BAD_TARGET
    End If
    
End Property

Public Property Get TargetDeclination() As Double

    CheckConnected False, m_bTracking, False
    
    If m_dTargetDec = INVALID_PARAMETER Then _
        Err.Raise SCODE_BAD_TARGET, ERR_SOURCE, MSG_BAD_TARGET
    
    TargetDeclination = m_dTargetDec
    
'    TargetDeclination = m_Util.DMSToDegrees(Me.CommandString("Gd"))

End Property

Public Property Let TargetDeclination(ByVal newVal As Double)

    Dim sexa As String
    
    CheckConnected False, m_bTracking, True
    
    If newVal < -90# Or newVal > 90# Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
            "Property TargetDeclination " & MSG_PROP_RANGE_ERROR
        
    If Not m_bLongFormat Then
        ' Short format, 1 decimal digit
        sexa = m_Util.DegreesToDM(newVal, m_sDecDelimDeg, m_sDecDelimMin, 1)
    Else
        sexa = m_Util.DegreesToDMS(newVal, m_sDecDelimDeg, _
            m_sDecDelimMin, m_sDecDelimSec) ' Long format, whole seconds
    End If
    If Left$(sexa, 1) <> "-" Then sexa = "+" & sexa         ' Both need leading '+'
    
    If Not Me.CommandBool("Sd " & sexa) Then
        Err.Raise SCODE_BAD_TARGET, ERR_SOURCE, MSG_BAD_TARGET
    End If
    
    m_dTargetDec = newVal
    
End Property

Public Property Get TargetRightAscension() As Double

    CheckConnected False, m_bTracking, False
    
    If m_dTargetRA = INVALID_PARAMETER Then _
        Err.Raise SCODE_BAD_TARGET, ERR_SOURCE, MSG_BAD_TARGET
        
    TargetRightAscension = m_dTargetRA
    
'    TargetRightAscension = m_Util.HMSToHours(Me.CommandString("Gr"))

End Property

Public Property Let TargetRightAscension(ByVal newVal As Double)
    Dim sexa As String
    
    CheckConnected False, m_bTracking, True
    
    If newVal < 0# Or newVal >= 24# Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
            "Property TargetRightAscension " & MSG_PROP_RANGE_ERROR
    
    If Not m_bLongFormat Then
        sexa = m_Util.HoursToHM(newVal, m_sRADelimHrs, _
                            m_sRADelimMin, 1)              ' Short format, 1 decimal digit
    Else
        sexa = m_Util.HoursToHMS(newVal, m_sRADelimHrs, _
                            m_sRADelimMin, m_sRADelimSec)   ' Long format, whole seconds
    End If
    
    If Not Me.CommandBool("Sr " & sexa) Then
        Err.Raise SCODE_BAD_TARGET, ERR_SOURCE, MSG_BAD_TARGET
    End If
    
    m_dTargetRA = newVal
    
End Property

Public Property Get Tracking() As Boolean

    CheckConnected False, m_bTracking, False
    
    Tracking = (Not m_bParked) And m_bTracking  ' a robust test
    
End Property

Public Property Let Tracking(ByVal newVal As Boolean)

    If newVal = m_bTracking Then _
        Exit Property
    
    CheckConnected True, m_bTracking, True
    
    SetTracking newVal
    
End Property

Public Property Get TrackingRate() As DriveRates

    CheckConnected False, m_bTracking, False
    
    TrackingRate = m_eTrackingRate
    
End Property

Public Property Let TrackingRate(ByVal newVal As DriveRates)

    CheckConnected False, m_bTracking, True
    
    ' GPS should be more capable ???
    ' but the new commands fail under test... all code commented out for now
    
'    ' only allow if scope is awake
'    If g_bAsleep Then _
'        Err.Raise SCODE_ASLEEP, ERR_SOURCE, MSG_ASLEEP
'    Select Case newVal
'        Case driveSidereal
'            Me.CommandBlind "TQ"
'        Case driveLunar
'            Me.CommandBlind "TL"
'        Case driveSolar
'            ' untested syntax ???
'            Me.CommandBlind "TM"
'            If Not Me.CommandBool("T060.164") Then _
'                Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
'                    "Property TrackingRate " & MSG_PROP_RANGE_ERROR
'        Case Else
'            Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
'                "Property TrackingRate " & MSG_PROP_RANGE_ERROR
'    End Select

        If newVal <> driveSidereal Then _
            Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, _
                "Property TrackingRate " & MSG_PROP_RANGE_ERROR
        
    m_eTrackingRate = newVal
    
End Property

Public Property Get TrackingRates() As Object

    CheckConnected False, m_bTracking, False
    
    Set TrackingRates = m_TrackingRates
    
End Property

Public Property Get UTCDate() As Date

    CheckConnected False, m_bTracking, True
    
    UTCDate = CDate(Me.CommandString("GL"))                   ' Local time
    ' DO NOT USE CDate() here!!! Scope always returns MM/DD/YY even in Europe
    UTCDate = UTCDate + Fix(mdy_vb(Me.CommandString("GC")))   ' Calendar date (added Fix to filter out extra daylight savings)
    UTCDate = UTCDate + (CDbl(Me.CommandString("GG")) / 24)   ' Now UTC

End Property

Public Property Let UTCDate(ByVal newVal As Date)
    Dim buf As String
    Dim tbuf As String
    Dim I As Integer
    Dim en As Boolean
    Dim UTCOffset As Integer
    Dim dateVal As Date                             ' Local except for LX200GPS
    
    CheckConnected False, m_bTracking, True
    
    '
    ' Set the scope's UTC offset to that of the PC, then convert
    ' UTC to local time and set that into the scope.
    '
    UTCOffset = CInt(utc_offs() / 3600)
    buf = Format$(UTCOffset, "00")
    If UTCOffset >= 0 Then buf = "+" & buf          ' Must have explicit +
    If Not Me.CommandBool("SG " & buf) Then
        Err.Raise SCODE_BAD_UTCOFFSET, ERR_SOURCE, MSG_BAD_UTCOFFSET
    End If
    
    '
    ' Next do the time, as date alerts planetary changes
    ' The LX200 recalculates ST when the LT changes. The
    ' Autostar does not.
    '
    newVal = CDate(CDbl(newVal) - (UTCOffset / 24#))    ' Convert to local time
    If Not Me.CommandBool("SL " & Format$(newVal, "hh:nn:ss")) Then
        Err.Raise SCODE_BAD_DATETIME, ERR_SOURCE, MSG_BAD_DATETIME
    End If

    '
    ' Make date string in scope format. Check the scope's
    ' date and if the same, skip the date set. Otherwise
    ' set it. We do things here so that various local date
    ' formats like 05.12.98 will work...
    '
    If Fix(mdy_vb(Me.CommandString("GC"))) = CDate(Fix(newVal)) Then
        GoTo TS_OK                      ' That's it
    End If
    '
    ' OK, we're going to change the date...
    '
    g_bBusy = True
    newVal = newVal + (UTCOffset / 24#) ' LX200GPS takes UTC (wart)
    g_SharedSerial.Transmit ":SC " & vb_mdy(Fix(newVal)) & "#"
    '
    ' Wait for the OK boolean then bail if the date was bad
    '
    buf = g_SharedSerial.ReceiveCounted(1)
    If Left$(buf, 1) = "0" Then     ' Illegal date
        g_bBusy = False
        Err.Raise SCODE_BAD_DATETIME, ERR_SOURCE, MSG_BAD_DATETIME
    End If
    '
    ' Wait for the "Updating..." then toss the blanks message
    '
    g_SharedSerial.ReceiveTerminated "#"
    g_SharedSerial.ReceiveTerminated "#"
    
    '
    ' The LX200GPS needs 15 seconds before it's data is valid
    ' Pop up a modeless dialog to inform  the user?
    '
    m_Util.WaitForMilliseconds 15000
    g_bBusy = False
    
TS_OK:

End Property

' -------
' Methods
' -------

Public Sub AbortSlew()

    CheckConnected True, m_bTracking, True
    
    g_bBusy = True
    Me.CommandBlind "Q"                             ' Halt the slew
    m_bAbortSlew = True                             ' Raise the abort flag
    m_Util.WaitForMilliseconds 1000                 ' Give the Autostar a second to react
    g_bBusy = False
    
End Sub

Public Function AxisRates(ByVal Axis As TelescopeAxes) As Object

    ' m_AxisRates is not set up till connect
    CheckConnected False, m_bTracking, False
    
    ' return empty collection for all Axis (that is proper for not supported)
    ' could support coarse rates for gps, but this wouldn't meet
    ' the real need at 0.1 deg/sec
    
    Set AxisRates = m_AxisRates

End Function

Public Function CanMoveAxis(ByVal Axis As TelescopeAxes) As Boolean
    
    ' not supported at this time, poor precision in the protocol (0.1 deg/sec)
    CanMoveAxis = False

End Function

Public Sub CommandBlind(ByVal Command As String, _
        Optional ByVal Raw As Boolean = False)
    
    g_SharedSerial.ClearBuffers                           ' Clear remaining junk in buffer
    
    If Raw Then
        g_SharedSerial.Transmit Command
    Else
        g_SharedSerial.Transmit ":" & Command & "#"
    End If
    
End Sub

Public Function CommandBool(ByVal Command As String, _
        Optional ByVal Raw As Boolean = False) As Boolean
    Dim buf As String
    
    g_SharedSerial.ClearBuffers                           ' Clear remaining junk in buffer
    
    If Raw Then
        g_SharedSerial.Transmit Command
    Else
        g_SharedSerial.Transmit ":" & Command & "#"
    End If
    
    buf = g_SharedSerial.ReceiveCounted(1)                ' Just a 1 or 0
    
    If buf = "1" Then
        CommandBool = True
    Else
        CommandBool = False
    End If

End Function

Public Function CommandString(ByVal Command As String, _
        Optional ByVal Raw As Boolean = False) As String
    Dim buf As String
    
    ' Check for driver debug commands
    If InStr(1, Command, "Meade Debug", vbTextCompare) = 1 Then
    
        ' fetch connect trace
        If InStr(1, Command, "Meade Debug ConnectState", vbTextCompare) = 1 Then
            CommandString = ConnectState
            ConnectState = vbCrLf & vbCrLf
        End If
        
        Exit Function
    End If
    
    g_SharedSerial.ClearBuffers                           ' Clear remaining junk in buffer
    
    If Raw Then
        g_SharedSerial.Transmit Command
    Else
        g_SharedSerial.Transmit ":" & Command & "#"
    End If
    
    ' Check for GW command which does not end with a # (at least not now)
    If Command = "GW" Or Command = ":GW#" Then
        buf = g_SharedSerial.ReceiveCounted(3)
    Else
        buf = g_SharedSerial.ReceiveTerminated("#")
    End If
    
    If buf <> "" Then                   ' Overflow protection
        If Raw Or Command = "GW" Or Command = ":GW#" Then
            CommandString = buf
        Else
            CommandString = Left$(buf, Len(buf) - 1)   ' Strip '#'
        End If
    Else
        CommandString = ""
    End If

End Function

Public Function DestinationSideOfPier(ByVal RightAscension As Double, _
                             ByVal Declination As Double) As PierSide

    ' The Meade protocol does not have a pier flip command that we can find
    
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
        "Method DestinationSideOfPier()" & MSG_NOT_IMPLEMENTED
               
End Function

Public Sub FindHome()

    ' some meade scopes could do this (gps) ???
    ' maybe kick off gps alignment (after meade firmware stabilizes) ???
    
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method FindHome()" & MSG_NOT_IMPLEMENTED

End Sub

Public Sub MoveAxis(ByVal Axis As TelescopeAxes, ByVal Rate As Double)
    
    ' not supported at this time, poor precision in the protocol (0.1 deg/sec)
    
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
        "Method MoveAxis" & MSG_NOT_IMPLEMENTED

End Sub

Public Sub Park()
    Dim I As Integer
    Dim buf As String
    Dim parkAlt As Double
    Dim parkAz As Double
    Dim tRa As Double
    Dim tDec As Double
    Dim tAlt As Double
    Dim tAz As Double
    
    CheckConnected False, m_bTracking, False
    
    ' bail if already good
    If m_bParked Then _
        Exit Sub
    
    ' we shouldn't already be asleep if we wern't parked
    ' but check anyway
    If g_bAsleep Then _
        Err.Raise SCODE_ASLEEP, ERR_SOURCE, MSG_ASLEEP
    
    parkAlt = INVALID_PARAMETER
    parkAz = INVALID_PARAMETER
    tAlt = INVALID_PARAMETER
    tAz = INVALID_PARAMETER
    tDec = INVALID_PARAMETER
    tRa = INVALID_PARAMETER
    
    ' prepare coordinates for parked state.  Scope will be asleep.
    ' It's really not OK for the scope to go dark
    On Error Resume Next
    buf = m_Profile.GetValue(SCOPE_ID, "ParkedBehavior")
    If buf = "2" Or buf = "" Then
        tAlt = Me.Altitude
        tAz = Me.Azimuth
        calc_hadec tAz, tAlt, m_dLat, tRa, tDec
    ElseIf buf = "1" Then
        buf = m_Profile.GetValue(SCOPE_ID, "ParkAlt")
        If buf <> "" Then parkAlt = CDbl(buf)
        buf = m_Profile.GetValue(SCOPE_ID, "ParkAz")
        If buf <> "" Then parkAz = CDbl(buf)
        If parkAlt >= -90 And parkAlt <= 90 And parkAz >= 0 And parkAz < 360 Then
            tAlt = parkAlt
            tAz = parkAz
            calc_hadec tAz, tAlt, m_dLat, tRa, tDec
        End If
    End If
    On Error GoTo 0
    
    ' put in the park coordinates
    g_bAsleep = True
    m_dRA = tRa
    m_dDec = tDec
    m_dAz = tAz
    m_dAlt = tAlt
    
    Me.CommandBlind "hP"
    m_bTracking = False
    
    ' watch for scope to go to sleep so that Park is synchronous
    For I = 1 To 30                             ' For up to 60 sec
        m_Util.WaitForMilliseconds 2000         ' Wait 2 sec
        g_SharedSerial.ClearBuffers
        g_SharedSerial.Transmit ":GS#"          ' Try for the Sidereal Time
        On Error Resume Next
        Call g_SharedSerial.ReceiveTerminated("#")
        If Err.Number <> 0 Then Exit For        ' failure means we're asleep
        On Error GoTo 0
    Next I
    On Error GoTo 0
    
    ' update state
    m_bParked = True
    
End Sub

Public Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Long)
    Dim d As String
    Dim ms As String
    
    If Duration < 0# Then _
        Err.Raise SCODE_PULSE_DUR, ERR_SOURCE, _
            "Method PulseGuide() " & MSG_PULSE_DUR
            
    CheckConnected True, m_bTracking, True
    
    Select Case Direction
        Case guideNorth:    d = "n"
        Case guideSouth:    d = "s"
        Case guideEast:     d = "e"
        Case guideWest:     d = "w"
    End Select
        
    If Duration <= 9999# Then
        ms = Format$(Duration, "0000")
        Me.CommandBlind "Mg" & d & ms
        m_Util.WaitForMilliseconds Duration
        
    Else
        Me.CommandBlind "RG"                ' Assure we're in Guide rate
        
        Me.CommandBlind "M" & d
        m_Util.WaitForMilliseconds Duration
        Me.CommandBlind "Q" & d
        
'        ' classic only !!!, this is needed since once in a while one is not enough
'        m_Util.WaitForMilliseconds 200
'        Me.CommandBlind "Q" & d
    End If
    
End Sub

Public Sub SetPark()

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method SetPark()" & MSG_NOT_IMPLEMENTED
    
End Sub

Public Sub SetupDialog()
    
    frmSetup.Show 1
    Unload frmSetup
    
End Sub

Public Sub SlewToAltAz(ByVal Azimuth As Double, _
                                    ByVal Altitude As Double)
    
    Me.SlewToAltAzAsync Azimuth, Altitude
    
    While Me.Slewing                            ' Wait for slew to complete
        m_Util.WaitForMilliseconds 500          ' Be responsive to AbortSlew()
    Wend
    
End Sub

Public Sub SlewToAltAzAsync(ByVal Azimuth As Double, ByVal Altitude As Double)
    
    Dim ha As Double
    Dim dec As Double
    
    g_bBusy = False
    CheckConnected True, False, True
            
    If m_eAlignMode = algAltAz Then
        TargetAltitude = Altitude
        TargetAzimuth = Azimuth
    Else
        ' convert to scope centric Alt/Az
        calc_hadec Azimuth, Altitude, m_dLat, ha, dec
        TargetAltitude = dec
        TargetAzimuth = RangeAz((ha * 15#) + 180#)
    End If
    
    DoSlewAsync False
        
End Sub

Public Sub SlewToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
    
    SlewToCoordinatesAsync RightAscension, Declination
        
    While Me.Slewing                            ' Wait for slew to complete
        m_Util.WaitForMilliseconds 500          ' Be responsive to AbortSlew()
    Wend
    
End Sub

Public Sub SlewToCoordinatesAsync(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
    
    g_bBusy = False
    CheckConnected True, True, True
            
    Me.TargetRightAscension = RightAscension
    Me.TargetDeclination = Declination
    
    DoSlewAsync True
    
End Sub

Public Sub SlewToTarget()
    
    SlewToTargetAsync
    
    While Me.Slewing                            ' Wait for slew to complete
        m_Util.WaitForMilliseconds 500          ' Be responsive to AbortSlew()
    Wend
        
End Sub

Public Sub SlewToTargetAsync()
    
    g_bBusy = False
    CheckConnected True, True, True
        
    If m_dTargetRA = INVALID_PARAMETER Or m_dTargetDec = INVALID_PARAMETER Then _
        Err.Raise SCODE_BAD_TARGET, ERR_SOURCE, MSG_BAD_TARGET

    DoSlewAsync True                            ' Start slew

End Sub

Public Sub SyncToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double)
    
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
        "Method SyncToAltAz()" & MSG_NOT_IMPLEMENTED
    
   ' unfortunatly, there is no altaz sync command code. We could convert
   ' to RA/DEC, but that fails when in LAND mode...
    
End Sub

Public Sub SyncToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
    
    g_bBusy = False
    CheckConnected True, True, True
        
    Me.TargetRightAscension = RightAscension
    Me.TargetDeclination = Declination
    
    Call Me.SyncToTarget
    
End Sub

Public Sub SyncToTarget()
'    Dim nTry As Integer
    
    CheckConnected True, True, True
        
    If m_dTargetRA = INVALID_PARAMETER Or m_dTargetDec = INVALID_PARAMETER Then _
        Err.Raise SCODE_BAD_TARGET, ERR_SOURCE, MSG_BAD_TARGET
      
    g_bBusy = True
    If Me.CommandString("CM") = "" Then
        g_bBusy = False
        Err.Raise SCODE_SYNC_FAILED, ERR_SOURCE, MSG_SYNC_FAILED
    End If
    
'    m_Util.WaitForMilliseconds 2000                         ' Start by waiting 2 seconds
'    nTry = 0
'    While EquDist(m_dTargetRA, m_dTargetDec, Me.RightAscension, Me.Declination) > 0.00833
'        m_Util.WaitForMilliseconds 2000                     ' Continue waiting till coorinates match
'        nTry = nTry + 1
'        If nTry >= 10 Then _
'            Err.Raise SCODE_SYNC_FAILED, ERR_SOURCE, MSG_SYNC_FAILED    ' 20 sec. - Too long!
'    Wend

    If m_bSyncDelay Then
        m_Util.WaitForMilliseconds 5000 ' **TEMP**
    End If
    
    g_bBusy = False

End Sub

Public Sub Unpark()

    CheckConnected False, m_bTracking, False
    
    ' No op if not parked
    If Not m_bParked Then _
        Exit Sub
        
'    ' autostar II can't come back from park state
'    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
'        "Method Unpark()" & MSG_NOT_IMPLEMENTED
    
    On Error GoTo Cleanup
    g_bBusy = True                 ' handle race condition
    
    ' get it back online
    RebootGPS
    
    ' force it back to long mode (don't place this call into Reconnect
    ' since connect needed it in a different place)
    SetLongFormat True
    
    ' do common stuff wrt connect (it will clear the Busy flag
    ' as it gets to the right place
    Reconnect True

Cleanup:

    g_bBusy = False
    
End Sub

' ========================
' PRIVATE HELPER FUNCTIONS
' ========================

'
' CheckConnected() - Raise an error if the scope is not connected
'
Private Sub CheckConnected(Park As Boolean, Track As Boolean, Asleep As Boolean)

    If Not IsConnected(devTelescope) Then _
        Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
    
    ' only allow if scope needed to be awake
    If Asleep And g_bAsleep Then _
        Err.Raise SCODE_ASLEEP, ERR_SOURCE, MSG_ASLEEP
        
    ' Global error checking for all slews and syncs
    If Park And m_bParked Then _
        Err.Raise SCODE_PARKED, ERR_SOURCE, MSG_PARKED
    
    ' tracking checks for alt/az vs equ commands
    If Track <> m_bTracking Then _
        Err.Raise SCODE_WRONG_TRACKING, ERR_SOURCE, MSG_WRONG_TRACKING
        
End Sub

Private Sub DoSlewAsync(Polar As Boolean)
    Dim stat As Integer
    Dim msg As String
    Dim code As Long
    Dim buf As String
    Dim I As Integer
    Dim tmo As Integer
        
    '
    ' This is where all the work is done for slewing. The other
    ' methods eventually call this one. The LX200 GPS has a bug
    ' where once in a blue moon it will not respond to the MS
    ' command. Therefore, we retry it 5 times before giving up.
    '
    On Error GoTo CMD_VAPORIZED
    tmo = g_SharedSerial.ReceiveTimeout
    g_SharedSerial.ReceiveTimeout = 5
    For I = 0 To 6                                  ' Total of 30 sec...
        If Polar Then
            Me.CommandBlind "MS"
        Else
            Me.CommandBlind "MA"
        End If
        m_bAbortSlew = False
        m_lSlewCheckTix = GetTickCount()            ' Mark time of slew start
        m_bSlewing = True                           ' Could be a race here!
        stat = CInt(g_SharedSerial.ReceiveCounted(1))     ' Get only the start-status digit
        On Error GoTo 0                             ' Got a response...
        g_SharedSerial.ReceiveTimeout = tmo         ' Restore timeout
        If stat <> 0 Then   ' Failed to start slew
            msg = MSG_SLEW_FAIL
            code = SCODE_SLEW_FAIL
            If Polar Then
                Select Case stat
                    Case 1:                             ' Below horizon
                        msg = MSG_BELOW_HORIZ
                        code = SCODE_BELOW_HORIZ
                    Case 2:                             ' Below minimum elevation
                        msg = MSG_BELOW_MIN_EL
                        code = SCODE_BELOW_MIN_EL
                End Select
            End If
            m_bSlewing = False                      ' Clear slewing flag
            Err.Raise code, ERR_SOURCE, msg         ' FAILED
        End If
        GoTo CMD_ACCEPTED
CMD_VAPORIZED:
        Resume CMD_RETRY
CMD_RETRY:
    Next I
    On Error GoTo 0                                 ' Assure errors back on
    g_SharedSerial.ReceiveTimeout = tmo             ' Restore timeout
    Err.Raise SCODE_SLEW_FAIL, ERR_SOURCE, MSG_SLEW_FAIL    ' Failed!
    '
CMD_ACCEPTED:

    g_bBusy = True
    
    m_lAsyncEndTix = GetTickCount() + (CLng(m_iSettleTime) * 1000&)
    m_lAsyncEndTix = m_lAsyncEndTix + 1000  ' Start with a minimum of 1 sec
    
    m_Util.WaitForMilliseconds 2000         ' Get past GPS' dead time
    g_bBusy = False
        
End Sub

'
' IsLongFormat() - Return True if scope is in long format
'
Private Function IsLongFormat() As Boolean
    Dim buf As String
    
    buf = Me.CommandString("GZ")            ' Get azimuth
    If Len(buf) <= 6 Then
        IsLongFormat = False                ' In short format
    Else
        IsLongFormat = True                 ' In long format
    End If

End Function

'
' SetLongFormat() - Set scope "long" format on or off
'
' The scope has only a toggle for the format, so we need to
' first test the format and then conditionally issue the
' toggle to set the desired format.
'
' Returns the format after the function is called. May
' fail on older LX200 scopes (V2.x ROMs).
'
Private Function SetLongFormat(b As Boolean) As Boolean
    Dim buf As String
    
    buf = Me.CommandString("GZ")            ' Get azimuth
    '
    ' Scope has some sort of race condition with "U", so
    ' we just wait a half sec. here and then do it.
    '
    m_Util.WaitForMilliseconds 500
    If ((b And (Len(buf) <= 6)) Or (Not b And (Len(buf) > 6))) Then
        Me.CommandBlind "U"                 ' change setting
    End If
    
    m_bLongFormat = IsLongFormat()          ' Return actual result
    SetLongFormat = m_bLongFormat
    
End Function

' centralized tracking control
Private Sub SetTracking(newVal As Boolean)
    Dim X As Double
    
    ' only continue if we need to
    If newVal = m_bTracking Then _
        Exit Sub
    
    If newVal Then
        ' LX200 Classic/GPS: Changing from LAND to astronomical
        ' mode starts sidereal tracking.

        g_bBusy = True                 ' handle race condition
        Select Case m_eAlignMode
            Case algAltAz:          Me.CommandBlind "AA"    ' go to alt/az
            Case algPolar:          Me.CommandBlind "AP"    ' really go to polar
            Case algGermanPolar:    Me.CommandBlind "AP"    ' expect this to change to "AG" in future...
            Case Else:              Me.CommandBlind "AP"    ' best effort
        End Select
        m_bTracking = True                      ' OK, not tracking anymore
        m_Util.WaitForMilliseconds 1000         ' Mode takes time on the GPS
        g_bBusy = False                ' coordinates valid again
        Me.TrackingRate = m_eTrackingRate
    Else
        ' turn off tracking by going to LAND Mode
        g_bBusy = True                 ' handle race condition
        Me.CommandBlind "AL"                    ' Switch to LAND mode
        m_Util.WaitForMilliseconds 1000         ' Land takes time
        m_bTracking = False                     ' OK, not tracking anymore
        g_bBusy = False                ' coordinates valid again
    End If
    
End Sub

'
' Reboot an LX200GPS. This just gets the controller into
' its power-on state, normally from having been parked.
' Used during connect to auto-unpark. The Handbox will
' end up stuck at the "Daylight Savings" prompt, which
' must be dealt with later.
'
Private Sub RebootGPS()
    Dim I As Integer
    
    g_SharedSerial.Transmit ":I#"               ' Reboot possible parked LX200GPS
    m_Util.WaitForMilliseconds 15000            ' Wait 15 sec minimum
    For I = 1 To 45                             ' For up to 235 sec more
        g_SharedSerial.ClearBuffers
        g_SharedSerial.Transmit ":GS#"          ' Try for the Sidereal Time
        On Error Resume Next
        Call g_SharedSerial.ReceiveTerminated("#")  ' Timeouts are 5 sec
        If Err.Number = 0 Then Exit For         ' Got it!
        On Error GoTo 0
    Next I
    On Error GoTo 0
    If I > 45 Then                              ' Scope never responded, prolly no scope!
        CloseConnection devTelescope
        Err.Raise SCODE_GPSREBOOT_FAIL, ERR_SOURCE, _
            "Connecting - " & MSG_GPSREBOOT_FAIL
    End If
    
    m_Util.WaitForMilliseconds 10000            ' Need this too :-(((
    
    m_bTracking = True
    g_bAsleep = False
    
End Sub

Private Sub Reconnect(GPSRebooted As Boolean)
    Dim buf As String
    Dim GW As String
    Dim CurDate As Double
    
    '
    ' (1) Get the alignment mode.  Do this early since we may have to bail out.
    '
    GW = Me.CommandString("GW")
    ConnectState = ConnectState & "  GW = " & GW & vbCrLf
    Select Case Mid(GW, 1, 1)
        Case "A"
            m_eAlignMode = algAltAz
        Case "P"
            m_eAlignMode = algPolar
        Case Else
            BadScopeType    ' will not return
    End Select
        
    '
    ' (2) Set medium slew speed. This is ESSENTIAL to preserve the drives
    '     when using in "industrial strength" applications. Not supported
    '     on the Autostar I. Limit setting if given by user. Allow user
    '     override or bypassing this.
    '
    buf = m_Profile.GetValue(SCOPE_ID, "MaxSlew")   ' "A" is auto
    If buf = "" Then buf = "A"
    If buf <> "N" Then
        If buf = "A" Then buf = "5"
        If val(buf) > 5 Then buf = "5"               ' Max speed 5 for GPS
        Me.CommandBool "Sw " & buf
    End If

    '
    ' (3) Set up initialize tracking rates
    '
    Me.GuideRateRightAscension = 10# / 3600#        ' Dec rate ties to RA rate
    Me.TrackingRate = driveSidereal                 ' set a default
    
    '
    ' (4) Optionally set time from PC
    '
    buf = m_Profile.GetValue(SCOPE_ID, "AutoSetTime")
    If GPSRebooted Or buf = "1" Then
        ConnectState = ConnectState & "  setting time" & vbCrLf
        CurDate = CDbl(Now()) + (CDbl(utc_offs()) / 86400#) ' UTC
        Me.UTCDate = CurDate
        buf = CommandString("ED")           ' Check handbox state
        If InStr(buf, "Daylight") > 0 Then  ' If sitting at "Daylight Savings?"
            CommandBlind "EK13"             ' Send three Enter commands...
            m_Util.WaitForMilliseconds 2000 ' ...to tell handbox to use date/time...
            CommandBlind "EK13"             ' ...and get to menu root.
            m_Util.WaitForMilliseconds 2000
            CommandBlind "EK13"
            m_Util.WaitForMilliseconds 1000
        End If
    End If
    
    '
    ' (5) Buffer the scope's latitude and longitude
    '
    m_dLat = Me.SiteLatitude                ' get a global version of latitude
    m_dLong = Me.SiteLongitude              ' get a global version of longitude
          
    '
    ' (6) Deal with tracking state uing the GW return state.  If byte 2 is "T"
    '     then we're tracking
    '
        
    On Error Resume Next
        If Mid(GW, 2, 1) = "T" Then
            m_bTracking = True
            ConnectState = ConnectState & "  we seem to be tracking" & vbCrLf
        Else
            m_bTracking = False
            ConnectState = ConnectState & "  we're not tracking" & vbCrLf
            buf = m_Profile.GetValue(SCOPE_ID, "AutoTrack")
            If buf = "1" Then
                ConnectState = ConnectState & "  auto track turning tracking on" & vbCrLf
                SetTracking True
                m_bTracking = True
            End If
        End If
    On Error GoTo 0
        
    '
    ' (7) Seed the coordinates for possible indeterminate state later
    '
    m_dTargetRA = INVALID_PARAMETER
    m_dTargetDec = INVALID_PARAMETER
    
    m_bParked = False       ' if we make it to here, we're not parked
    g_bBusy = False         ' no race condition, any call to coords that beats me is fine
    
    m_dAlt = Me.Altitude
    m_dAz = Me.Azimuth
    m_dDec = Me.Declination
    m_dRA = Me.RightAscension

End Sub

'' checks that a guiding rate commanded is in range
'Private Function RateIsInRange(Rate As Double) As Boolean
''    Dim r As range
''
''    For Each r In m_GuideRanges
''        If Abs(Rate) <= r.Maximum And Abs(Rate) >= r.Minimum Then
''            RateIsInRange = True
''            Exit Function
''        End If
''    Next
'    RateIsInRange = False
'End Function

'
' Special formatter for Lat/Long
'
Private Function DegtoDM(ByVal d As Double, ByVal p As Integer) As String
    Dim h As String
    Dim m As String
    Dim df As String
    Dim n As Boolean
    
    If d < 0# Then
        d = -d
        n = True
    Else
        n = False
    End If
    
    If p = 2 Then df = "00" Else df = "000"
    
    h = Format$(Fix(d), df)             ' Whole degrees
    d = (d - CDbl(h)) * 60#             ' Minutes
    m = Format$(CInt(d), "00")          ' Whole minutes (rounded)
    If m = "60" Then
        m = "00"
        h = Format$((CInt(h) + 1), df)
    End If

    DegtoDM = h & Chr$(223) & m
    If n Then DegtoDM = "-" & DegtoDM
       
End Function

'
' Convert VB style date/time to MM/DD/YY for use as input to the scope
'
Private Function vb_mdy(dt As Date) As String
    Dim yy As Integer
    
    dt = CVDate(Fix(dt))
    yy = Year(dt) Mod 100
    vb_mdy = Format$(Month(dt), "00") & "/" & _
            Format$(Day(dt), "00") & "/" & _
            Format$(yy, "00")

End Function

Private Function RangeAlt(ByVal Alt As Double)

    If Alt > 90# Then _
        Alt = 90#
    If Alt < -90# Then _
        Alt = -90#
             
    RangeAlt = Alt
    
End Function

Private Function RangeAz(ByVal Az As Double)

    While Az >= 360#
        Az = Az - 360#
    Wend
    While Az < 0#
        Az = Az + 360#
    Wend
             
    RangeAz = Az
    
End Function

Private Function RangeHA(ByVal ha As Double)
    
    While ha > 12#
        ha = ha - 24#
    Wend
    While ha <= -12#
        ha = ha + 24#
    Wend
             
    RangeHA = ha
    
End Function

Private Function RangeRA(ByVal RA As Double)

    While RA >= 24#
        RA = RA - 24#
    Wend
    While RA < 0#
        RA = RA + 24#
    Wend
             
    RangeRA = RA
    
End Function

' ---------
' EquDist() - Return equatorial distance between objects, degrees
' ---------

Private Function EquDist(ra1, dec1, ra2, dec2)

    Dim x1, x2, y1, y2, z1, z2, PI, k, r
    Dim ca1, sa1, cd1, sd1, ca2, sa2, cd2, sd2
    Dim a1rad, d1rad, a2rad, d2rad

    PI = 4# * Atn(1#)
    k = PI / 180#

    a1rad = 15# * k * ra1
    a2rad = 15# * k * ra2
    d1rad = k * dec1
    d2rad = k * dec2
      
    ca1 = Cos(a1rad)
    ca2 = Cos(a2rad)
    sa1 = Sin(a1rad)
    sa2 = Sin(a2rad)

    cd1 = Cos(d1rad)
    cd2 = Cos(d2rad)
    sd1 = Sin(d1rad)
    sd2 = Sin(d2rad)

    x1 = cd1 * ca1
    x2 = cd2 * ca2
    y1 = cd1 * sa1
    y2 = cd2 * sa2
    z1 = sd1
    z2 = sd2

    r = x1 * x2 + y1 * y2 + z1 * z2

    If r > 1# Then r = 1#
    If r < -1# Then r = -1#

    EquDist = (1# / k) * Arccos(r)
        
End Function

Private Function Arccos(X)

    If X = 1# Then
      Arccos = 0#
    ElseIf X = -1# Then
      Arccos = 4# * Atn(1#)
    Else
      Arccos = 2# * Atn(1#) - Atn(X / Sqr(1# - X * X))
    End If

End Function

'---------------------------------------------------------------------
'
' calc_altaz() - Calculate current azimuth and altitude
'
'---------------------------------------------------------------------

Private Sub calc_altaz(ByVal ha As Double, ByVal dec As Double, _
        lat As Double, Az As Double, Alt As Double)
        
    Dim tAz As Double
    Dim tAlt As Double
    
    hadec_aa lat * DEG_RAD, ha * HRS_RAD, dec * DEG_RAD, tAlt, tAz
             
    Alt = tAlt * RAD_DEG
    
    Az = 360# - (tAz * RAD_DEG)
    
End Sub

'---------------------------------------------------------------------
'
' calc_hadec() - Calculate current hour angle  and declination
'
'---------------------------------------------------------------------

Private Sub calc_hadec(ByVal Az As Double, ByVal Alt As Double, _
        lat As Double, ha As Double, dec As Double)
        
    Dim tHA As Double
    Dim tRa As Double
    Dim tDec As Double
    
    aa_hadec lat * DEG_RAD, Alt * DEG_RAD, (360# - Az) * DEG_RAD, tHA, tDec
    
    ha = -(tHA * RAD_HRS)
    
    dec = tDec * RAD_DEG
    
End Sub

Private Sub BadScopeType()

        CloseConnection devTelescope                ' Release port for telescope
        Err.Raise SCODE_NOT_LX200GPS, ERR_SOURCE, MSG_NOT_LX200GPS

End Sub

' ============================
' Implementation of ITelescope
' ============================

Private Sub ITelescope_AbortSlew()
    AbortSlew
End Sub

Private Property Get ITelescope_AlignmentMode() As AlignmentModes
    ITelescope_AlignmentMode = AlignmentMode
End Property

Private Property Get ITelescope_Altitude() As Double
    ITelescope_Altitude = Altitude
End Property

Private Property Get ITelescope_ApertureArea() As Double
    ITelescope_ApertureArea = ApertureArea
End Property

Private Property Get ITelescope_ApertureDiameter() As Double
    ITelescope_ApertureDiameter = ApertureDiameter
End Property

Private Property Get ITelescope_AtHome() As Boolean
    ITelescope_AtHome = AtHome
End Property

Private Property Get ITelescope_AtPark() As Boolean
    ITelescope_AtPark = AtPark
End Property

Private Function ITelescope_AxisRates(ByVal Axis As TelescopeAxes) As IAxisRates
    '
    ' Note that this more or less "casts" our internal AxisRates
    ' object's interface to AxisRates.
    '
    Set ITelescope_AxisRates = AxisRates(Axis)
End Function

Private Property Get ITelescope_Azimuth() As Double
    ITelescope_Azimuth = Azimuth
End Property

Private Property Get ITelescope_CanFindHome() As Boolean
    ITelescope_CanFindHome = CanFindHome
End Property

Private Function ITelescope_CanMoveAxis(ByVal Axis As TelescopeAxes) As Boolean
    ITelescope_CanMoveAxis = CanMoveAxis(Axis)
End Function

Private Property Get ITelescope_CanPark() As Boolean
    ITelescope_CanPark = CanPark
End Property

Private Property Get ITelescope_CanPulseGuide() As Boolean
    ITelescope_CanPulseGuide = CanPulseGuide
End Property

Private Property Get ITelescope_CanSetDeclinationRate() As Boolean
    ITelescope_CanSetDeclinationRate = CanSetDeclinationRate
End Property

Private Property Get ITelescope_CanSetGuideRates() As Boolean
    ITelescope_CanSetGuideRates = CanSetGuideRates
End Property

Private Property Get ITelescope_CanSetPark() As Boolean
    ITelescope_CanSetPark = CanSetPark
End Property

Private Property Get ITelescope_CanSetPierSide() As Boolean
    ITelescope_CanSetPierSide = CanSetPierSide
End Property

Private Property Get ITelescope_CanSetRightAscensionRate() As Boolean
    ITelescope_CanSetRightAscensionRate = CanSetRightAscensionRate
End Property

Private Property Get ITelescope_CanSetTracking() As Boolean
    ITelescope_CanSetTracking = CanSetTracking
End Property

Private Property Get ITelescope_CanSlew() As Boolean
    ITelescope_CanSlew = CanSlew
End Property

Private Property Get ITelescope_CanSlewAltAz() As Boolean
    ITelescope_CanSlewAltAz = CanSlewAltAz
End Property

Private Property Get ITelescope_CanSlewAltAzAsync() As Boolean
    ITelescope_CanSlewAltAzAsync = CanSlewAltAzAsync
End Property

Private Property Get ITelescope_CanSlewAsync() As Boolean
    ITelescope_CanSlewAsync = CanSlewAsync
End Property

Private Property Get ITelescope_CanSync() As Boolean
    ITelescope_CanSync = CanSync
End Property

Private Property Get ITelescope_CanSyncAltAz() As Boolean
    ITelescope_CanSyncAltAz = CanSyncAltAz
End Property

Private Property Get ITelescope_CanUnpark() As Boolean
    ITelescope_CanUnpark = CanUnpark
End Property

Private Sub ITelescope_CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False)
    CommandBlind Command, Raw
End Sub

Private Function ITelescope_CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean
    ITelescope_CommandBool = CommandBool(Command, Raw)
End Function

Private Function ITelescope_CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String
    ITelescope_CommandString = CommandString(Command, Raw)
End Function

Private Property Let ITelescope_Connected(ByVal RHS As Boolean)
    Connected = RHS
End Property

Private Property Get ITelescope_Connected() As Boolean
    ITelescope_Connected = Connected
End Property

Private Property Get ITelescope_Declination() As Double
    ITelescope_Declination = Declination
End Property

Private Property Let ITelescope_DeclinationRate(ByVal RHS As Double)
    DeclinationRate = RHS
End Property

Private Property Get ITelescope_DeclinationRate() As Double
    ITelescope_DeclinationRate = DeclinationRate
End Property

Private Property Get ITelescope_Description() As String
    ITelescope_Description = Description
End Property

Private Function ITelescope_DestinationSideOfPier(ByVal RightAscension As Double, ByVal Declination As Double) As PierSide
    ITelescope_DestinationSideOfPier = DestinationSideOfPier(RightAscension, Declination)
End Function

Private Property Let ITelescope_DoesRefraction(ByVal RHS As Boolean)
    DoesRefraction = RHS
End Property

Private Property Get ITelescope_DoesRefraction() As Boolean
    ITelescope_DoesRefraction = DoesRefraction
End Property

Private Property Get ITelescope_DriverInfo() As String
    ITelescope_DriverInfo = DriverInfo
End Property

Private Property Get ITelescope_DriverVersion() As String
    ITelescope_DriverVersion = DriverVersion
End Property

Private Property Get ITelescope_EquatorialSystem() As EquatorialCoordinateType
    ITelescope_EquatorialSystem = EquatorialSystem
End Property

Private Sub ITelescope_FindHome()
    FindHome
End Sub

Private Property Get ITelescope_FocalLength() As Double
    ITelescope_FocalLength = FocalLength
End Property

Private Property Let ITelescope_GuideRateDeclination(ByVal RHS As Double)
    GuideRateDeclination = RHS
End Property

Private Property Get ITelescope_GuideRateDeclination() As Double
    ITelescope_GuideRateDeclination = GuideRateDeclination
End Property

Private Property Let ITelescope_GuideRateRightAscension(ByVal RHS As Double)
    GuideRateRightAscension = RHS
End Property

Private Property Get ITelescope_GuideRateRightAscension() As Double
    ITelescope_GuideRateRightAscension = GuideRateRightAscension
End Property

Private Property Get ITelescope_InterfaceVersion() As Integer
    ITelescope_InterfaceVersion = InterfaceVersion
End Property

Private Property Get ITelescope_IsPulseGuiding() As Boolean
    ITelescope_IsPulseGuiding = IsPulseGuiding
End Property

Private Sub ITelescope_MoveAxis(ByVal Axis As TelescopeAxes, ByVal Rate As Double)
    MoveAxis Axis, Rate
End Sub

Private Property Get ITelescope_Name() As String
    ITelescope_Name = Name
End Property

Private Sub ITelescope_Park()
    Park
End Sub

Private Sub ITelescope_PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Long)
    PulseGuide Direction, Duration
End Sub

Private Property Get ITelescope_RightAscension() As Double
    ITelescope_RightAscension = RightAscension
End Property

Private Property Let ITelescope_RightAscensionRate(ByVal RHS As Double)
    RightAscensionRate = RHS
End Property

Private Property Get ITelescope_RightAscensionRate() As Double
    ITelescope_RightAscensionRate = RightAscensionRate
End Property

Private Sub ITelescope_SetPark()
    SetPark
End Sub

Private Sub ITelescope_SetupDialog()
    SetupDialog
End Sub

Private Property Let ITelescope_SideOfPier(ByVal RHS As PierSide)
    SideOfPier = RHS
End Property

Private Property Get ITelescope_SideOfPier() As PierSide
    ITelescope_SideOfPier = SideOfPier
End Property

Private Property Get ITelescope_SiderealTime() As Double
    ITelescope_SiderealTime = SiderealTime
End Property

Private Property Let ITelescope_SiteElevation(ByVal RHS As Double)
    SiteElevation = RHS
End Property

Private Property Get ITelescope_SiteElevation() As Double
    ITelescope_SiteElevation = SiteElevation
End Property

Private Property Let ITelescope_SiteLatitude(ByVal RHS As Double)
    SiteLatitude = RHS
End Property

Private Property Get ITelescope_SiteLatitude() As Double
    ITelescope_SiteLatitude = SiteLatitude
End Property

Private Property Let ITelescope_SiteLongitude(ByVal RHS As Double)
    SiteLongitude = RHS
End Property

Private Property Get ITelescope_SiteLongitude() As Double
    ITelescope_SiteLongitude = SiteLongitude
End Property

Private Property Get ITelescope_Slewing() As Boolean
    ITelescope_Slewing = Slewing
End Property

Private Property Let ITelescope_SlewSettleTime(ByVal RHS As Integer)
    SlewSettleTime = RHS
End Property

Private Property Get ITelescope_SlewSettleTime() As Integer
    ITelescope_SlewSettleTime = SlewSettleTime
End Property

Private Sub ITelescope_SlewToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double)
    SlewToAltAz Azimuth, Altitude
End Sub

Private Sub ITelescope_SlewToAltAzAsync(ByVal Azimuth As Double, ByVal Altitude As Double)
    SlewToAltAzAsync Azimuth, Altitude
End Sub

Private Sub ITelescope_SlewToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double)
    SlewToCoordinates RightAscension, Declination
End Sub

Private Sub ITelescope_SlewToCoordinatesAsync(ByVal RightAscension As Double, ByVal Declination As Double)
    SlewToCoordinatesAsync RightAscension, Declination
End Sub

Private Sub ITelescope_SlewToTarget()
    SlewToTarget
End Sub

Private Sub ITelescope_SlewToTargetAsync()
    SlewToTargetAsync
End Sub

Private Sub ITelescope_SyncToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double)
    SyncToAltAz Azimuth, Altitude
End Sub

Private Sub ITelescope_SyncToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double)
    SyncToCoordinates RightAscension, Declination
End Sub

Private Sub ITelescope_SyncToTarget()
    SyncToTarget
End Sub

Private Property Let ITelescope_TargetDeclination(ByVal RHS As Double)
    TargetDeclination = RHS
End Property

Private Property Get ITelescope_TargetDeclination() As Double
    ITelescope_TargetDeclination = TargetDeclination
End Property

Private Property Let ITelescope_TargetRightAscension(ByVal RHS As Double)
    TargetRightAscension = RHS
End Property

Private Property Get ITelescope_TargetRightAscension() As Double
    ITelescope_TargetRightAscension = TargetRightAscension
End Property

Private Property Let ITelescope_Tracking(ByVal RHS As Boolean)
    Tracking = RHS
End Property

Private Property Get ITelescope_Tracking() As Boolean
    ITelescope_Tracking = Tracking
End Property

Private Property Let ITelescope_TrackingRate(ByVal RHS As DriveRates)
    TrackingRate = RHS
End Property

Private Property Get ITelescope_TrackingRate() As DriveRates
    ITelescope_TrackingRate = TrackingRate
End Property

Private Property Get ITelescope_TrackingRates() As ITrackingRates
    '
    ' Note that this more or less "casts" our internal TrackingRates
    ' object's interface to ITrackingRates.
    '
    Set ITelescope_TrackingRates = TrackingRates
End Property

Private Sub ITelescope_Unpark()
    Unpark
End Sub

Private Property Let ITelescope_UTCDate(ByVal RHS As Date)
    UTCDate = RHS
End Property

Private Property Get ITelescope_UTCDate() As Date
    ITelescope_UTCDate = UTCDate
End Property

