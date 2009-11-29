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
Attribute VB_Description = "ASCOM Telescope driver for Software Bisque TheSky"
'---------------------------------------------------------------------
'   =============
'   TELESCOPE.CLS
'   =============
'
' Implementation of the ASCOM TheSky driver Telescope interface
'
' Written:  19-Nov-00   Robert B. Denny (rbd) <rdenny@dc3.com>
' Additional Authors:   Chris Rowland   (cdr) <chris.rowland@dsl.pipex.com>
'                       Scott Kephart   (bsk) <scott@casa3d.net>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 19-Nov-00 rbd     Initial edit, from template project
' 13-Jan-01 rbd     Fill in missing ERR_SOURCE argument in calls to
'                   Err.Raise for "not implemented". There were many!
'                   Specific not implemented messages.
' 21-Jan-01 rbd     Eliminate the WindowsAPI.bas module. Self-
'                   registration. Slewing always returns False.
' 25-Jan-01 rbd     New properties CanSlew, CanSlewAsync, CanSync,
'                   CanPark, CanFindHome, CanSetPark, CanUnpark
'                   Interface V1.1 - Enable FindHome() (was missing)
' 28-Jan-00 rbd     Add description and version info display to
'                   SetupDialog box.
' 06-Feb-01 rbd     Add Tracking, CanSetTracking
' 10-Mar-00 rbd     Implement SlewSettleTime
' 08-Jun-01 rbd     New Profile.DeviceType to generalize Chooser and
'                   Profile.
' 08-Jun-01 rbd     Telescope 1.4: Add PulseGuide() & CanPulseGuide
'                   Add Tracking control
' 17-Jul-01 rbd     1.4.2: Tracking control is a loser. Not supported
'                   on some versions. Disable for now. Add support for
'                   Get UTCDate (needed by ACP2).
' 06-Aug-01 rbd     1.4.3: Fake SiderealTime added, then removed.
' 09-Aug-01 rbd     1.4.4: Turn on async slewing, depending on whether
'                   installed Bisque components support it. Test this
'                   at run time.
' 05-Oct-01 rbd     1.4.5: Add ByVals as needed
' 14-Oct-01 rbd     1.4.6: Fix overflow on slew settling time
' 03-Dec-01 rbd     1.4.7: IsSlewComplete is busted. Fake it.
' 27-Jul-02 rbd     2.0.1: IsSlewComplete works in latest TheSky.
' 31-Jul-02 rbd     2.0.1: Implement AbortSlew(), call
'                   CheckConnected before FindHome(), implement
'                   RightAscensionRate and DeclinationRate. Add
'                   rate limiter for calls to GetRaDec and GetAzAlt.
'                   Remove unused Serial object.
' 28-Aug-02 rbd     2.0.1: Remove Not Not in SlewToTarget
' 21-Mar-03 rbd     2.2.1: Version change for Platform 2.2
' 31-Mar-03 rbs     2.2.1: TheSky now independent of Orchestrate etc.
'                   New ProgID. Force users to update to .100 of TheSky.
'                   Fix slew settling.
' 09-Apr-03 rbd     2.2.2: Made the driver adaptive to RASCOM or sky
'                   objects. Tries RASCOM first, then sky. Now late
'                   bound for this reason. Fixed RightAscensionRate
'                   and DeclinationRate. The RASCOM properties for
'                   the rates are not implemented, requiring a local
'                   shadow, and needed to convert to ASCOM rate units.
'                   Removed obsolete commments in the header regarding
'                   various problems with TheSky, RASCOM, etc. that
'                   have been fixed or improved in the required
'                   versions.
' 22-Apr-03 rbd     2.2.3 - Don't adjust dec rates for sidereal time.
' 12-May-03 rbd     2.2.4 - Add 3sec delay in SlewToTargetAsync. See
'                   the notes above the implementation of same. Reduce
'                   polling rate to 0.5 Hz per feedback from users,
'                   who reported "bad behavior" if polled too fast.
' 02-Jun-03 rbd     2.2.5 - Cannot use WaitForMilliseconds() for the
'                   delay in SlewToTargetAsync(). Client prog may pump
'                   events and try to poll Slewing, which will result
'                   in a "command in progress" error from TheSky.
' 24-Jun-03 rbd     2.3.1 - Version change for Platform 2.3
' 07-Aug-03 rbd     2.3.2 - Move ID & DESC to common constants for
'                   new setup dialog. Add config items to select
'                   TheSky or RASCOM interface, control of CanSync
'                   for TPOINT protection. Move creeation and release
'                   on the TheSky/RASCOM object into the Connected
'                   property so TheSky doesn't start when using the
'                   SetupDialog method, thus avoiding a chicken and
'                   egg problem with interface selection. Safety
'                   Connected = False by testing validity of m_Bisque
'                   object before calling disconnect().
' 08-Aug-03 rbd     2.3.3 - Remove the "Use Orchestrate" checkbox and
'                   always use TheSky.RASCOMTele.
' 03-Sep-03 rbd     3.0.1 - For Platform 3.0
' 23-Apr-04 rbd     3.1.1 - For TheSky V6
' 23-Apr-04 rbd     3.1.2 - Reorg setup dialog, add V5/V6 selector,
'                   change startup for either/or.
' 21-May-04 rbd     3.1.3 - Fix disconnect behavior. Don't release
'                   TheSky on disconnect, it doesn't exit anyway.
'                   Make Park() synchronous as it should be.
' 01-Nov-04 rbd     4.0 - Wait 500ms after setting tracking rates
' 25-Nov-04 rbd     4.0.2 - Add switch for slew-start delay, fix
'                   SetupDialog() to pick up Sky 5/6 changes.
' 01-Feb-05 rbd     4.1.1 - Add logic to correct crazy coordinates
'                   coming from TheSky (e.g. Dec outside +/- 90).
'                   Polling now connected into single function that
'                   includes crazy coordinate correction.
' 26-Dec-05 rbd     4.1.2 - Add Find Home on first connect feature
' 04-Jan-06 rbd     4.1.3 - Check LastSlewError and raise error
'                   if not successful.
' 03-Oct-07 cdr     4.1.2 - Add PulseGuide option using the SetTracking method.
' 17-Oct-07 cdr     4.1.3 - try 0.5 sidereal rate and change the dec stop command
' 07-Nov-07 cdr     4.1.4 - remove the 15 arc sec per second offset from the Ra rate
' 20-Nov-07 cdr     4.1.5 - reverse the guide east and west directions.
' 04-Jan-07 rbd     5.1.0 - Merge Chris Rowland's PulseGuide code in, he started
'                   with 4.1.1 (unfortunately). For Platform 5.0.
' 19-Feb-09 bsk     5.1.2 - Add support for the Alignment mode combo box
'                   POTH really wants to know what type of mount is in use.
' 18-Mar-09 rbd     5.1.2 - Merge Scott Kephart's addition of Alignment Mode. Add code
'                   in SetupDialog to capture and update AlignmentMode selection.
' 25-Mar-09 rbd     5.1.3 - Add control for trask offsets
'---------------------------------------------------------------------

Option Explicit

Private Const POLL_TIX As Long = 2000
' Ratio of from synodic (solar) to sidereal (stellar) rate
Private Const SIDRATE As Double = 0.9972695677
Private Const degrad As Double = 0.0174532925199

Public Enum AlignmentModes
    algUnknown = -1
    algAltAz = 0
    algPolar = 1
    algGermanPolar = 2
End Enum

Public Enum GuideDirections
    guideNorth = 0
    guideSouth = 1
    guideEast = 2
    guideWest = 3
End Enum

Private m_Util As DriverHelper.Util
Private m_Profile As DriverHelper.Profile
Private m_iSettleTime As Integer
Private m_bAbortSlew As Boolean
Private m_lAsyncEndTix As Long
Private m_Bisque As Object
Private m_lNextRaDecTix As Long
Private m_dRA As Double                         ' Cached/corrected coordinates
Private m_dDec As Double
Private m_dTgtRA As Double
Private m_dTgtDec As Double
Private m_dRADeltaRate As Double                ' RA seconds per sidereal second
Private m_dDecDeltaRate As Double               ' Dec arcseconds per sidereal second
Private m_dAlt As Double                        ' Cached coordinates
Private m_dAz As Double
Private m_lNextAzAltTix As Long
Private m_bInhibitSync As Boolean
Private m_bUseSky6 As Boolean
Private m_bSlewDelay As Boolean
Private m_bFindHome As Boolean                  ' Find Home on first conn
Private m_bFirstConn As Boolean                 ' This is True after first conn
Private m_bSlewCompleted As Boolean             ' Flag used to get LastSlewError after asnyc slew
Private m_bTrackOffsets As Boolean
Private m_bPulseGuide As Boolean
Private m_iAlignmentMode As AlignmentModes

Private Declare Function GetTickCount Lib "kernel32" () As Long
Private Declare Sub SleepEx Lib "kernel32" (ByVal dwMilliseconds As Long, _
                        ByVal bAlertable As Boolean)


Private Sub Class_Initialize()
    Dim buf As String
    
    Set m_Util = New DriverHelper.Util
    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Telescope"              ' We're a Telescope driver
    m_Profile.Register ID, DESC                     ' Self-register (MUST DO THIS FIRST!)
    '
    ' Get the config settings for interface and sync supresssion
    '
    buf = m_Profile.GetValue(ID, "InhibitSync")
    If buf = "" Then buf = "False"                  ' Default to allowing Sync
    m_bInhibitSync = CBool(buf)
    buf = m_Profile.GetValue(ID, "TheSky6")
    If buf = "" Then buf = "False"                  ' Default to V5
    m_bUseSky6 = CBool(buf)
    buf = m_Profile.GetValue(ID, "SlewDelay")
    If buf = "" Then buf = "True"                   ' Default to do delay
    m_bSlewDelay = CBool(buf)
    buf = m_Profile.GetValue(ID, "FindHome")
    If buf = "" Then buf = "False"                  ' Default no FindHome() call!
    m_bFindHome = CBool(buf)
    buf = m_Profile.GetValue(ID, "TrackOffsets")
    If buf = "" Then buf = "False"                   ' Default to no track offsets
    m_bTrackOffsets = CBool(buf)
    buf = m_Profile.GetValue(ID, "PulseGuide")
    If buf = "" Then buf = "False"                   ' Default to not PulseGuide
    m_bPulseGuide = CBool(buf)
    
    buf = m_Profile.GetValue(ID, "AlignmentMode")    ' Default to algUnknown (maybe the default should be GermanPolar, for the Paramount?)
    Select Case buf
        Case "algUnknown"
            m_iAlignmentMode = algUnknown
        Case "algAltAz"
            m_iAlignmentMode = algAltAz
        Case "algPolar"
            m_iAlignmentMode = algPolar
        Case "algGermanPolar"
            m_iAlignmentMode = algGermanPolar
        Case Else
            m_iAlignmentMode = algUnknown
    End Select

    Set m_Profile = Nothing                         ' Release Profile
    
    
    Set m_Bisque = Nothing                          ' [sentinel]
    m_bFirstConn = False                            ' Loading driver defines "first conn" epoch
    m_bSlewCompleted = True                         ' Initialize this crazy flag
    m_dTgtRA = 0#                                   ' Start with defined values
    m_dTgtDec = 0#
    m_dRADeltaRate = 0#
    m_dDecDeltaRate = 0#
    m_iSettleTime = 0                               ' Default 0 slew settle time
    m_lNextRaDecTix = 0&                            ' Force first RA/Dec poll
    m_lNextAzAltTix = 0&                            ' Force first Az/Alt poll
    m_Util.LoadDLL App.Path & "\astro32.dll"        ' Assure this is loaded and available

End Sub

Private Sub Class_Terminate()
    
    On Error Resume Next
    m_Bisque.Disconnect
    
End Sub

Public Property Get AlignmentMode() As AlignmentModes

    ' bsk - if alignment mode is unknown, go ahead and return "NOT_IMPLEMENTED". That way
    ' if some app depends on the prior behavior somehow (unlikely, but hey, it could happen)
    ' we can revert back to the old way by changing the combobox.
    If m_iAlignmentMode = algUnknown Then Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property AlignmentMode" & MSG_NOT_IMPLEMENTED
    
    AlignmentMode = m_iAlignmentMode
    
End Property


Public Property Get Altitude() As Double

    GetAltAz
    Altitude = m_dAlt
    
End Property

Public Property Get ApertureDiameter() As Double

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property ApertureDiameter" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get Azimuth() As Double
    
    GetAltAz
    Azimuth = m_dAz

End Property

Public Property Get CanFindHome() As Boolean

    CanFindHome = True

End Property

Public Property Get CanPark() As Boolean

    CanPark = True

End Property

Public Property Get CanPulseGuide() As Boolean
    ' cdr attempt to implement pulse guiding using the SetTracking command
    CanPulseGuide = m_bPulseGuide
    
End Property

Public Property Get CanSetPark() As Boolean

    CanSetPark = True

End Property

Public Property Get CanSetTracking() As Boolean

    CanSetTracking = True

End Property

Public Property Get CanSlew() As Boolean

    CanSlew = True

End Property

Public Property Get CanSlewAsync() As Boolean

    CanSlewAsync = True                             ' .100 supports async

End Property

Public Property Get CanSync() As Boolean

    CanSync = Not m_bInhibitSync

End Property

Public Property Get CanUnpark() As Boolean

    CanUnpark = False

End Property

Public Property Get Connected() As Boolean

    If m_Bisque.IsConnected = 1 Then
        Connected = True
    Else
        Connected = False
    End If
    
End Property

Public Property Let Connected(ByVal newVal As Boolean)
    Dim buf
    
    If newVal Then
        On Error GoTo NO_SKY
        If m_bUseSky6 Then
            Set m_Bisque = CreateObject("TheSky6.RASCOMTele")
        Else
            Set m_Bisque = CreateObject("TheSky.RASCOMTele")
        End If
        On Error GoTo 0
        m_Bisque.Connect
        m_Bisque.Asynchronous = 1                   ' Need this after Connect()
        '
        ' If find home selected and first connection then call FindHome()
        '
        If m_bFindHome And Not m_bFirstConn Then m_Bisque.FindHome
        m_bFirstConn = True                         ' We've connected once...
    Else
        If Not m_Bisque Is Nothing Then m_Bisque.Disconnect
''        Set m_Bisque = Nothing
    End If
    
    Exit Property

NO_SKY:
    If m_bUseSky6 Then
        buf = "V6"
    Else
        buf = "V5"
    End If
    MsgBox "TheSky " & buf & " was selected, but it does not appear to be " & _
            "installed or configured for scripting.", _
            (vbOKOnly + vbCritical + vbMsgBoxSetForeground), App.FileDescription
    Exit Property
    
End Property

Public Property Get Declination() As Double
    
    GetAndFixRaDec
    Declination = m_dDec
    
End Property

Public Property Get DeclinationRate() As Double

    If m_bTrackOffsets Then
        CheckConnected
        DeclinationRate = m_dDecDeltaRate
    Else
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property DeclinationRate" & MSG_NOT_IMPLEMENTED
    End If
    
End Property

'
' http://www.bisque.com/help/TheSkyV6/telescope_settracking_method.htm
'
Public Property Let DeclinationRate(ByVal newVal As Double)
    
    If m_bTrackOffsets Then
        CheckConnected
        m_dDecDeltaRate = newVal
        
        m_Bisque.SetTracking m_Bisque.IsTracking, 0&, _
                (m_dRADeltaRate * 15# / SIDRATE), _
                newVal
        m_Util.WaitForMilliseconds 2000                 ' Paramount requires this
    Else
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property DeclinationRate" & MSG_NOT_IMPLEMENTED
    End If

End Property

Public Property Get Description() As String

    Description = "Telescope controlled by TheSky"
    
End Property

Public Property Get DriverInfo() As String

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
        
End Property

Public Property Get FocalLength() As Double

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property FocalLength" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get Name() As String

    Name = "TheSky"
    
End Property

Public Property Get RightAscension() As Double
    
    GetAndFixRaDec
    RightAscension = m_dRA
    
End Property

Public Property Get RightAscensionRate() As Double

    If m_bTrackOffsets Then
        CheckConnected
        RightAscensionRate = m_dRADeltaRate
    Else
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property RightAscensionRate" & MSG_NOT_IMPLEMENTED
    End If
    
End Property

'
' http://www.bisque.com/help/TheSkyV6/telescope_settracking_method.htm
'
Public Property Let RightAscensionRate(ByVal newVal As Double)

    If m_bTrackOffsets Then
        CheckConnected
        m_dRADeltaRate = newVal
        m_Bisque.SetTracking m_Bisque.IsTracking, 0&, _
                (newVal * 15# / SIDRATE), _
                m_dDecDeltaRate
        m_Util.WaitForMilliseconds 5000                 ' Paramount requires this
    Else
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property RightAscensionRate" & MSG_NOT_IMPLEMENTED
    End If

End Property

Public Property Get SiderealTime() As Double

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property SiderealTIme" & MSG_NOT_IMPLEMENTED
    
End Property

Public Property Get SiteElevation() As Double

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property SiteElevation" & MSG_NOT_IMPLEMENTED

End Property

Public Property Let SiteElevation(ByVal newVal As Double)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property SiteElevation" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get SiteLatitude() As Double

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property SiteLatitude" & MSG_NOT_IMPLEMENTED
    
End Property

Public Property Let SiteLatitude(ByVal newVal As Double)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property SiteLatitude" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get SiteLongitude() As Double

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property SiteLongitude" & MSG_NOT_IMPLEMENTED
    
End Property

Public Property Let SiteLongitude(ByVal newVal As Double)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property SiteLongitude" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get Slewing() As Boolean
    Dim lLastSlewError As Long
    
    CheckConnected
    If (Not m_bAbortSlew) And (Not m_Bisque.IsSlewComplete = 1) Then
        '
        ' This adds settle time after we see that the scope has
        ' not moved one arcmin.
        '
        m_lAsyncEndTix = GetTickCount() + (CLng(m_iSettleTime) * 1000&)
        Slewing = True
    Else
        '
        ' This implements the trailing settle time
        ' (skip it on a slew abort)
        '
        If m_bAbortSlew Or (GetTickCount() > m_lAsyncEndTix) Then
            m_bAbortSlew = False
            Slewing = False
            If Not m_bSlewCompleted Then
                m_bSlewCompleted = True
                lLastSlewError = m_Bisque.GetLastSlewError
                If lLastSlewError <> 0 Then
                    Err.Raise SCODE_SLEW_FAILURE, ERR_SOURCE, _
                        MSG_SLEW_FAILURE & lLastSlewError
                End If
            End If
        Else
            Slewing = True
        End If
    End If

End Property

Public Property Get SlewSettleTime() As Integer

    SlewSettleTime = m_iSettleTime
    
End Property

Public Property Let SlewSettleTime(ByVal newVal As Integer)

    If (newVal < 0) Or (newVal > 100) Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, MSG_PROP_RANGE_ERROR
        
    m_iSettleTime = newVal
    
End Property

Public Property Get TargetDeclination() As Double

    CheckConnected
    TargetDeclination = m_dTgtDec
    
End Property

Public Property Let TargetDeclination(ByVal newVal As Double)

    CheckConnected
    If newVal < -90# Or newVal > 90# Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, MSG_PROP_RANGE_ERROR
    m_dTgtDec = newVal
    
End Property

Public Property Get TargetRightAscension() As Double

    CheckConnected
    TargetRightAscension = m_dTgtRA
    
End Property

Public Property Let TargetRightAscension(ByVal newVal As Double)

    CheckConnected
    If newVal < 0# Or newVal >= 24# Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, MSG_PROP_RANGE_ERROR
    m_dTgtRA = newVal
    
End Property

Public Property Get Tracking() As Boolean

    CheckConnected
    If m_Bisque.IsTracking = 0 Then
        Tracking = False
    Else
        Tracking = True
    End If
   
End Property

Public Property Let Tracking(ByVal newVal As Boolean)
    Dim t As Long
    
    CheckConnected
    If newVal Then
        t = 1&
    Else
        t = 0&
    End If
    m_Bisque.SetTracking t, 1&, 0#, 0#
    
End Property

Public Property Get UTCDate() As Date

    UTCDate = CDate(CDbl(Now()) + (CDbl(utc_offs()) / 86400#))

End Property

Public Property Let UTCDate(ByVal newVal As Date)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property UTCDate" & MSG_NOT_IMPLEMENTED

End Property

Public Sub AbortSlew()
    
    CheckConnected
    m_bAbortSlew = True
    m_Bisque.Abort
    
End Sub

Public Sub CommandBlind(ByVal Command As String)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method CommandBlind()" & MSG_NOT_IMPLEMENTED

End Sub

Public Function CommandBool(ByVal Command As String) As Boolean

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method CommandBool()" & MSG_NOT_IMPLEMENTED

End Function

Public Function CommandString(ByVal Command As String) As String

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method CommandString()" & MSG_NOT_IMPLEMENTED

End Function

Public Sub FindHome()

    CheckConnected
    m_Bisque.FindHome
    
End Sub

Public Sub Park()

    '
    ' When the Park completes, the IsConnected property goes to 0.
    ' Thus there is a race condition here with checking IsSlewComplete.
    ' If you check IsSlewComplete when IsConnected is 0, TheSky raises
    ' an error. VB has no short-circuit Boolean, so we just assume it's
    ' parked if we get an error (no connection from IsSlewing). Sleazy
    ' but safe, and that logic should get hit only rarely anyway.
    ' I suppose I could rely on just waiting for IsConnected to go to 0,
    ' but that seems like shaky ground.
    '
    CheckConnected
    m_Bisque.Park
    On Error GoTo PARK_DONE
    Do While m_Bisque.IsConnected = 1 And m_Bisque.IsSlewComplete = 0
        m_Util.WaitForMilliseconds 1000
    Loop
PARK_DONE:

End Sub

Public Sub PulseGuide(ByVal Direction As GuideDirections, _
                                            ByVal Duration As Long)

    If Not m_bPulseGuide Then
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method PulseGuide()" & MSG_NOT_IMPLEMENTED
    End If
    ' cdr try to implement pulseguide by using the SetTracking command
    CheckConnected
    If Duration > 10 Then
        Dim RaRate As Double, DecRate As Double
        Dim rate As Double, dur As Long
        ' avoid short guide times by setting the guide time to 100 millisecs and reducing the rate
        ' try a maximum rate of 50% sidereal
        If Duration < 100 Then
            dur = 100
            rate = 0.075 * Duration
        Else
            dur = Duration
            rate = 7.5
        End If
        Select Case Direction
        Case GuideDirections.guideNorth
            DecRate = rate   ' in arc sec per sec
        Case GuideDirections.guideSouth
            DecRate = -rate
        Case GuideDirections.guideEast
            RaRate = rate      ' in arc sec per sec
        Case GuideDirections.guideWest
            RaRate = -rate
        End Select
        m_Bisque.SetTracking m_Bisque.IsTracking, 0&, RaRate, DecRate
        SleepEx dur, True
        Select Case Direction
        Case GuideDirections.guideNorth, GuideDirections.guideSouth
            ' set rate to 0
            m_Bisque.SetTracking m_Bisque.IsTracking, 0&, 0&, 0&
        Case GuideDirections.guideEast, GuideDirections.guideWest
            ' turn tracking on
            m_Bisque.SetTracking m_Bisque.IsTracking, 1&, 0&, 0&
        End Select
    Else
        ' turn guiding off
        m_Bisque.SetTracking m_Bisque.IsTracking, 1&, 0&, 0&
    End If

End Sub

Public Sub SetPark()

    CheckConnected
    m_Bisque.SetParkPosition                ' Docs are incorrect

End Sub

Public Sub SetupDialog()
    Dim buf As String
    Dim an
    frmSetup.Show 1
    '
    ' Update the config settings (may not have changed!!)
    '
    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Telescope"              ' We're a Telescope driver
    buf = m_Profile.GetValue(ID, "InhibitSync")
    If buf = "" Then buf = "False"                  ' Default to allowing Sync
    m_bInhibitSync = CBool(buf)
    buf = m_Profile.GetValue(ID, "TheSky6")
    If buf = "" Then buf = "False"                  ' Default to V5
    m_bUseSky6 = CBool(buf)
    buf = m_Profile.GetValue(ID, "SlewDelay")
    If buf = "" Then buf = "True"                  ' Default to do delay
    m_bSlewDelay = CBool(buf)
    buf = m_Profile.GetValue(ID, "PulseGuide")
    If buf = "" Then buf = "False"                  ' Default to not PulseGuide
    m_bPulseGuide = CBool(buf)
    buf = m_Profile.GetValue(ID, "AlignmentMode")    ' Default to algUnknown (maybe the default should be GermanPolar, for the Paramount?)
    Select Case buf
        Case "algUnknown"
            m_iAlignmentMode = algUnknown
        Case "algAltAz"
            m_iAlignmentMode = algAltAz
        Case "algPolar"
            m_iAlignmentMode = algPolar
        Case "algGermanPolar"
            m_iAlignmentMode = algGermanPolar
        Case Else
            m_iAlignmentMode = algUnknown
    End Select
    Set m_Profile = Nothing                         ' Release Profile
    
End Sub

Public Sub SlewToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)

    CheckConnected
    m_dTgtRA = RightAscension
    m_dTgtDec = Declination
    SlewToTarget
    
End Sub

Public Sub SlewToCoordinatesAsync(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
    
    CheckConnected
    m_dTgtRA = RightAscension
    m_dTgtDec = Declination
    SlewToTargetAsync

End Sub

Public Sub SlewToTarget()
    Dim lLastSlewError As Long
    
    CheckConnected
    m_bSlewCompleted = False
    m_Bisque.SlewToRaDec m_dTgtRA, m_dTgtDec, "ASCOM target"
    Do While Not m_Bisque.IsSlewComplete = 1
        m_Util.WaitForMilliseconds 1000
    Loop
    m_bSlewCompleted = True
    lLastSlewError = m_Bisque.GetLastSlewError
    If lLastSlewError <> 0 Then
        Err.Raise SCODE_SLEW_FAILURE, ERR_SOURCE, _
            MSG_SLEW_FAILURE & lLastSlewError
    End If
    m_Util.WaitForMilliseconds CLng(m_iSettleTime) * 1000&
    
End Sub

'
' NOTE: With imagers that seize the CPU, it is necessary to give a few
' seconds for TheSky to start a slew. Unfortunately we have no way to
' know if the particular imager in use is one those nasty imagers, so
' we always delay 4 sec after issuing the command to TheSky. If you
' shorten this, you may encounter random slew failures, particularly
' when crossing the meridian from east to west on the Paramount.
'
' 4.0.2 - Make this delay a user option
'
Public Sub SlewToTargetAsync()

    CheckConnected
    m_bSlewCompleted = False
    m_lAsyncEndTix = GetTickCount() + (CLng(m_iSettleTime) * 1000&)
    m_Bisque.SlewToRaDec m_dTgtRA, m_dTgtDec, "ASCOM target"
    If m_bSlewDelay Then WaitNoEvents 4000              ' (see note above)
    
End Sub

Public Sub SyncToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
    CheckConnected
    m_dTgtRA = RightAscension
    m_dTgtDec = Declination
    Me.SyncToTarget

End Sub

Public Sub SyncToTarget()
    
    If m_bInhibitSync Then Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Cannot sync because TPOINT is in use."
    CheckConnected
    m_Bisque.Sync m_dTgtRA, m_dTgtDec, "ASCOM target"

End Sub

Public Sub Unpark()

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method Unpark()" & MSG_NOT_IMPLEMENTED

End Sub

' -----------------
' PRIVATE FUNCTIONS
' -----------------

'
' CheckConnected() - Raise an error if the scope is not connected
'
Private Sub CheckConnected()

    If Not m_Bisque.IsConnected = 1 Then _
        Err.Raise SCODE_NOT_CONNECTED, _
                    ERR_SOURCE, _
                    MSG_NOT_CONNECTED
End Sub

Private Sub GetAltAz()
    Dim t As Long
    
    CheckConnected
    t = GetTickCount()
    If t > m_lNextAzAltTix Then
        Call m_Bisque.GetAzAlt
        m_lNextAzAltTix = t + POLL_TIX
    End If
    m_dAz = m_Bisque.dAz
    m_dAlt = m_Bisque.dAlt
    
End Sub

'
' GetAndFixRaDec() - Poll and cache RA/Dec from TheSky. Attempt to
' fix up occasional crazy coordinates. I think this may have to do
' with TPOINT, because these same out of range coordinates will
' sometimes appear in TPOINT run mapping point data. I'm doing my
' best to catch this here, and it may not work in all cases. I posted
' a note to SoftBisqueForum but go no response after a couple of days.
'
' The problem can be spotted by looking for Declination outside the
' range of -90 to +90.
'
Private Sub GetAndFixRaDec()
    Dim t As Long
    
    CheckConnected
    t = GetTickCount()
    If t > m_lNextRaDecTix Then
        '
        ' Poll TheSky
        '
        Call m_Bisque.GetRaDec
        m_lNextRaDecTix = t + POLL_TIX
        m_dRA = m_Bisque.dRA
        m_dDec = m_Bisque.dDec
        '
        ' Patch crazy coordinates
        '
        If m_dDec > 90 Then
            m_dDec = 180# - m_dDec
            m_dRA = m_dRA - 12#
        ElseIf m_dDec < -90 Then
            m_dDec = -180# - m_dDec
            m_dRA = m_dRA - 12#
        End If
        If m_dRA < 0 Then m_dRA = m_dRA + 24#
    End If
    
End Sub

Private Sub WaitNoEvents(Delay As Long)
    Dim tEnd
    
    tEnd = GetTickCount() + Delay
    While GetTickCount() < tEnd
        SleepEx 100, True
    Wend
End Sub
