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
Attribute VB_Description = "ASCOM Telescope Driver for Generic LX200 Protocol"
'---------------------------------------------------------------------
' Copyright © 2001-2002 SPACE.com Inc., New York, NY
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
' Implementation of ASCOM Simple LX200 telescope driver Telescope class
' This driver is intended to work with virtually any mount or scope
' that uses the "LX200-like" protocol. The following LX200 commands
' are used by this driver:
'
'   GR          Get RA
'   GD          Get Dec
'   Sr          Set target RA
'   Sd          Set target Dec
'   MS          Slew to target
'   Q           Stop motion/slew
'   CM          Sync/match to target
'   U           Toggle long/short format        (optional)
'
' If the U command is not supported, the driver assumes that the device
' is capable only of short format commands and data. This is a severe
' limitation of such a device.
'
' Written:  27-Jan-2001   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 27-Jan-01 rbd     Initial edit - after Meade Driver (much simplified)
' 28-Jan-01 rbd     Add description and version info display to
'                   SetupDialog box.
' 01-Feb-01 rbd     Allow CM (sync) to return any string including ""
'                   Add readable Target RA/Dec. Trap errors setting
'                   target RA/Dec & raise errors.
' 06-Feb-01 rbd     Add Tracking, CanSetTracking
' 08-Jun-01 rbd     New Profile.DeviceType to generalize Chooser and
'                   Profile.
' 08-Jun-01 rbd     Telescope 1.4: Add PulseGuide() & CanPulseGuide
' 19-Aug-01 rbd     1.4.2 Fix error message in Let TargetDeclination
' 02-Sep-01 rbd     1.4.3 Flex delimiters for TargetRA/Dec
' 05-Oct-01 rbd     1.4.4 - Add ByVals for parameters where missing.
' 12-Oct-01 rbd     1.4.5 - Fix flex delimiters, must get RA/Dec
'                   AFTER switching to long mode!!!
' 20-Oct-01 rbd     1.4.6 - Close possible race conditions with timer
' 21-Oct-01 rbd     1.4.7 - Put 100ms gap between GR and GD for FS2.
' 14-Jan-02 rbd     1.4.8 - Remove m_iMissedPolls and correct LOST_
'                   SCOPE loop to use local variable.
' 06-Mar-03 rbd     2.2.0 - Remove timer, use Ajai's simple slew
'                   detection logic. Timer was a can of worms. Trim
'                   delimiters to one character to prevent build-up
'                   from trailing spaces on obscure controller.
'                   Slewing logic tolerances adjust to short/long
'                   format.
' 07-Mar-03 rbd     2.2.0 - Add setup for encoder resolution, needed
'                   by slew detection logic. Remove annoying beep.
' 14-Mar-03 rbd     2.2.1 - Remove 4 sec minimum settle time. Just use
'                   the configured settling time.
' 21-Mar-03 rbd     2.2.2 - Set slewing flag immediately on calling
'                   any slew method. Avoids delay in Slewing() while
'                   getting it underway.
' 28-Mar-03 rbd     2.2.2 - Fix setup dialog to read registry for
'                   encoder resolution on open. Was always showing 60.
'                   Fix initial slew distance check. Broken badly!
' 24-Jun-03 rbd     2.3.1 - Version change for Platform 2.3
' 03-Sep-03 rbd     3.0.1 - Version change for Platform 3.0
' 08-Mar-04 rbd     3.0.2 - Set long format BEFORE detecting delimiters
'                   Don't add trailing blanks if no trailing delimiters.
'                   Detect short/long format and adjust delimiters.
' 23-Nov-04 rbd     4.0.1 - Version change for Platform 4.0
' 04-Jan-08 rbd     5.0.1 - Version change for Platform 5.0
'---------------------------------------------------------------------

Option Explicit

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

Private Const ID As String = "LXP.Telescope"
Private Const DESC As String = "Generic LX200 Type Scopes"


Private m_Serial As DriverHelper.Serial
Private m_Util As DriverHelper.Util
Private m_Profile As DriverHelper.Profile

'
' State Variables
'
Private m_dPrevRA As Double
Private m_dPrevDec As Double
Private m_bAbortSlew As Boolean
Private m_dRARes As Double                          ' degrees
Private m_dDecRes As Double                         ' degrees
Private m_bSlewing As Boolean
Private m_bStartingSlew As Boolean                  ' Super ugly!
Private m_lAsyncEndTix As Long
Private m_bLongFormat As Boolean
Private m_iSettleTime As Integer
Private m_dTargetRA As Double
Private m_bTargetRAValid As Boolean
Private m_dTargetDec As Double
Private m_bTargetDecValid As Boolean
Private m_sRADelimHrs As String
Private m_sRADelimMin As String
Private m_sRADelimSec As String
Private m_sDecDelimDeg As String
Private m_sDecDelimMin As String
Private m_sDecDelimSec As String

'
' Locally used Win32 API
'
Private Declare Function GetTickCount Lib "kernel32" () As Long
Private Declare Function Sleep Lib "kernel32" (ms As Long) As Long

Private Sub Class_Initialize()

    Set m_Serial = New DriverHelper.Serial
    Set m_Util = New DriverHelper.Util
    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Telescope"      ' We're a Telescope driver
    m_iSettleTime = 2                       ' Default 2 sec settling time
    m_Profile.Register ID, DESC
    m_dPrevRA = 0#
    m_dPrevDec = 0#
    m_bTargetRAValid = False
    m_bTargetDecValid = False
    
End Sub

Private Sub Class_Terminate()
    
    On Error Resume Next
    Me.Connected = False                ' Force disconnect

End Sub

Public Property Get AlignmentMode() As LXP.AlignmentModes

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property AlignmentMode" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get Altitude() As Double

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property Altitude" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get ApertureDiameter() As Double

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property ApertureDiameter" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get Azimuth() As Double

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property Azimuth" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get CanFindHome() As Boolean

    CanFindHome = False

End Property

Public Property Get CanPark() As Boolean

    CanPark = False
    
End Property

Public Property Get CanPulseGuide() As Boolean

    CanPulseGuide = True
    
End Property

Public Property Get CanSetPark() As Boolean

    CanSetPark = False

End Property

Public Property Get CanSetTracking() As Boolean

    CanSetTracking = False

End Property

Public Property Get CanSlew() As Boolean

    CanSlew = True
    
End Property

Public Property Get CanSlewAsync() As Boolean

    CanSlewAsync = True

End Property

Public Property Get CanSync() As Boolean

    CanSync = True
    
End Property

Public Property Get CanUnpark() As Boolean

    CanUnpark = False

End Property

Public Property Get Connected() As Boolean

    Connected = m_Serial.Connected
    
End Property

Public Property Let Connected(ByVal newVal As Boolean)
    Dim buf As String, sRA As String, sDec As String
    Dim bits() As String, rx As RegExp, mt As MatchCollection
    Dim tmo As Long
    
    If newVal Then                                      ' CONNECTING
        '
        ' (1) Set up the communications link. Default to COM1.
        '
        buf = m_Profile.GetValue(ID, "COM Port")
        If buf = "" Then                                ' Default to COM1
            buf = "1"
            m_Profile.WriteValue ID, "COM Port", buf
        End If
        m_Serial.port = CInt(buf)                       ' Set port
        m_Serial.Speed = ps9600                         ' LX200 uses 9600
        m_Serial.ReceiveTimeout = 5                     ' 5 second timeout
        m_Serial.Connected = True                       ' Grab the serial port
        '
        ' (2) Determine that there is an LX200 scope there, initialize
        '     some variables for below...
        '
        On Error GoTo NO_SCOPE                          ' -- error trap --
        sRA = Me.CommandString("GR")
        m_dPrevRA = m_Util.HMSToHours(sRA)
        sDec = Me.CommandString("GD")
        m_dPrevDec = m_Util.DMSToDegrees(sDec)
        On Error GoTo 0                                 ' -- end trap --
        m_Serial.ClearBuffers                           ' Toss any junk remaining in buffers
        '
        ' (3) Final timeout & try for long format.
        '
        m_Serial.ReceiveTimeout = 10                    ' Switch to 10 sec timeout
        m_bLongFormat = SetLongFormat(True)             ' Try for long format
        
        '
        ' (4) Initialize slew detection parameters. The registry values may be
        '     missing if the SetupDialog() had not yet been used. We default
        '     to 60 arcsec. Avoid use of decimal values for international use.
        '
        m_bSlewing = False
        m_bAbortSlew = False
        buf = m_Profile.GetValue(ID, "RA Resolution")
        If buf = "" Then
            buf = "60"
            m_Profile.WriteValue ID, "RA Resolution", buf
        End If
        m_dRARes = CDbl(buf) / 3600#
        buf = m_Profile.GetValue(ID, "Dec Resolution")
        If buf = "" Then
            buf = "60"
            m_Profile.WriteValue ID, "Dec Resolution", buf
        End If
        m_dDecRes = CDbl(buf) / 3600#
        
        '
        ' (5) Get the delimiters that this implementation uses. Re-read the
        '     RA and Dec now that we might have Long Format set!
        '
        sRA = Me.CommandString("GR")
        m_dPrevRA = m_Util.HMSToHours(sRA)
        sDec = Me.CommandString("GD")
        m_dPrevDec = m_Util.DMSToDegrees(sDec)
        Set rx = New RegExp
        rx.Global = True
        rx.Pattern = "[\+\-]*[0-9\.]+([^0-9\.]+)[0-9\.]*([^0-9\.]*)[0-9\.]*([^0-9\.]*)"
        Set mt = rx.Execute(sRA)
        m_sRADelimHrs = DelimTrim(mt(0).SubMatches(0))  ' Hours delimiter
        If m_bLongFormat Then
            m_sRADelimMin = DelimTrim(mt(0).SubMatches(1))  ' Minutes delimiter
            m_sRADelimSec = Trim$(mt(0).SubMatches(2))  ' Don't append trailing blanks
        Else
            m_sRADelimMin = Trim(mt(0).SubMatches(1))   ' Don't append trailing blanks
            m_sRADelimSec = ""
        End If
        Set mt = rx.Execute(sDec)
        m_sDecDelimDeg = DelimTrim(mt(0).SubMatches(0)) ' Degrees delimiter
        If m_bLongFormat Then
            m_sDecDelimMin = DelimTrim(mt(0).SubMatches(1)) ' Minutes delimiter
            m_sDecDelimSec = Trim$(mt(0).SubMatches(2)) ' Don't append trailing blanks
        Else
            m_sDecDelimMin = Trim(mt(0).SubMatches(1))  ' Don't append trailing blanks
            m_sDecDelimSec = ""
        End If
    Else                                                ' DISCONNECTING
        On Error Resume Next                            ' Best efforts...
        If m_Serial.Connected Then Me.CommandBlind "Q"  ' Stop all motion
        m_bSlewing = False                              ' No longer slewing
        If m_Serial.Connected Then SetLongFormat False
        m_Serial.ClearBuffers                           ' Clear serial buffers
        m_Serial.Connected = False                      ' Release COM port
    End If

    Exit Property
    
NO_SCOPE:
    m_Serial.ClearBuffers
    m_Serial.Connected = False                          ' Release the port
    Err.Raise SCODE_NO_SCOPE, ERR_SOURCE, MSG_NO_SCOPE

End Property

Public Property Get Declination() As Double

    CheckConnected
    Declination = m_Util.DMSToDegrees(Me.CommandString("GD"))

End Property

Public Property Get DeclinationRate() As Double

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property DeclinationRate" & MSG_NOT_IMPLEMENTED

End Property

Public Property Let DeclinationRate(ByVal newVal As Double)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property DeclinationRate" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get Description() As String

    Description = DESC
    
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

    Name = "LXP"
    
End Property

Public Property Get RightAscension() As Double

    CheckConnected
    RightAscension = m_Util.HMSToHours(Me.CommandString("GR"))

End Property

Public Property Get RightAscensionRate() As Double

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property RightAscensionRate" & MSG_NOT_IMPLEMENTED

End Property

Public Property Let RightAscensionRate(ByVal newVal As Double)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property RightAscensionRate" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get SiderealTime() As Double

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property SiderealTime" & MSG_NOT_IMPLEMENTED

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
    Dim buf As String, bMoving As Boolean
    Dim RA As Double, Dec As Double
    
    If Not m_bSlewing Then                          ' Short-circuit when not slewing
        Slewing = False
        Exit Property                               ' No slew now
    End If
    
    If m_bStartingSlew Then                         ' Starting ... say we're slewing
        Slewing = True                              ' This is really horrible
        Exit Property                               ' We need a real slewing flag from the scope!
    End If
    
    '
    ' This is subtle: If we detect more than resolution of motion, we -know- it's
    ' moving, and remember the last position. If not, all we know is that it didn't
    ' move more than resolution between requests for the Slewing property. Some
    ' client could call this at a high rate, in which case the scope would likely
    ' NOT move that far. So we don't update our "last" coordinates if it didn't
    ' move far enough. This way, it has to remain within resolution for 3 seconds
    ' plus the settling time, or it will reset the end-time and wait another
    ' 3 sec plus settling, etc.
    '
    RA = Me.RightAscension
    Dec = Me.Declination
    bMoving = TestSlew(RA, Dec, m_dPrevRA, m_dPrevDec, m_dRARes, m_dDecRes)
    If bMoving Then
        m_dPrevRA = RA
        m_dPrevDec = Dec
    End If
   
    If (Not m_bAbortSlew) And bMoving Then
        '
        ' This adds settle time after we see that the scope has
        ' not moved one arcmin.
        '
        m_lAsyncEndTix = GetTickCount() + (CLng(m_iSettleTime) * 1000&)
        m_bSlewing = True
    Else
        '
        ' This implements the trailing settle time
        ' (skip it on a slew abort)
        '
        If m_bAbortSlew Or (GetTickCount() > m_lAsyncEndTix) Then
            Me.CommandBlind "Q"                     ' Force the scope to stop (FS2 buglet)
            m_bSlewing = False
            m_bAbortSlew = False
        Else
            m_bSlewing = True
        End If
    End If
    
    Slewing = m_bSlewing

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

    If Not m_bTargetDecValid Then
        Err.Raise SCODE_PROPNOTSET, ERR_SOURCE, _
            "Property TargetDeclination " & MSG_PROPNOTSET
    Else
        TargetDeclination = m_dTargetDec
    End If

End Property

Public Property Let TargetDeclination(ByVal newVal As Double)
    Dim sexa As String
    
    CheckConnected
    If newVal < -90# Or newVal > 90# Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, MSG_PROP_RANGE_ERROR
    If Not m_bLongFormat Then
        sexa = m_Util.DegreesToDM(newVal, m_sDecDelimDeg, _
                            m_sDecDelimMin, 1)              ' Short format, 1 decimal digit
    Else
        sexa = m_Util.DegreesToDMS(newVal, m_sDecDelimDeg, _
                            m_sDecDelimMin, m_sDecDelimSec) ' Long format, whole seconds
    End If
    If Left$(sexa, 1) <> "-" Then sexa = "+" & sexa         ' Both need leading '+'
    If Not Me.CommandBool("Sd " & sexa) Then
            Err.Raise SCODE_TGTSETERR, ERR_SOURCE, _
                MSG_TGTSETERR & "TargetDeclination = " & sexa
    End If
    
    m_dTargetDec = newVal                                   ' Set shadow variables
    m_bTargetDecValid = True
    
End Property

Public Property Get TargetRightAscension() As Double

    If Not m_bTargetRAValid Then
        Err.Raise SCODE_PROPNOTSET, ERR_SOURCE, _
            "Property TargetRightAscension " & MSG_PROPNOTSET
    Else
        TargetRightAscension = m_dTargetRA
    End If
    
End Property

Public Property Let TargetRightAscension(ByVal newVal As Double)
    Dim sexa As String
    
    CheckConnected
    If newVal < 0# Or newVal >= 24# Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, MSG_PROP_RANGE_ERROR
    If Not m_bLongFormat Then
        sexa = m_Util.HoursToHM(newVal, m_sRADelimHrs, _
                            m_sRADelimMin, 1)              ' Short format, 1 decimal digit
    Else
        sexa = m_Util.HoursToHMS(newVal, m_sRADelimHrs, _
                            m_sRADelimMin, m_sRADelimSec)   ' Long format, whole seconds
    End If
    If Not Me.CommandBool("Sr " & sexa) Then
            Err.Raise SCODE_TGTSETERR, ERR_SOURCE, _
                MSG_TGTSETERR & "TargetRightAscension = " & sexa
    End If
    
    m_dTargetRA = newVal                                   ' Set shadow variables
    m_bTargetRAValid = True

End Property

Public Property Get Tracking() As Boolean

    Tracking = True
    
End Property

Public Property Let Tracking(ByVal newVal As Boolean)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Control of Tracking" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get UTCDate() As Date

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property UTCDate" & MSG_NOT_IMPLEMENTED

End Property

Public Property Let UTCDate(ByVal newVal As Date)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property UTCDate" & MSG_NOT_IMPLEMENTED

End Property

Public Sub AbortSlew()

    Me.CommandBlind "Q"                             ' Halt the slew
    m_bAbortSlew = True                             ' Raise the abort flag
    
End Sub

Public Sub CommandBlind(ByVal Command As String)

    m_Serial.ClearBuffers                           ' Clear remaining junk in buffer
    m_Serial.Transmit "#:" & Command & "#"
    
End Sub

Public Function CommandBool(ByVal Command As String) As Boolean
    Dim buf As String
    
    m_Serial.ClearBuffers                           ' Clear remaining junk in buffer
    m_Serial.Transmit "#:" & Command & "#"
    buf = m_Serial.ReceiveCounted(1)                ' Just a 1 or 0
    If buf = "1" Then
        CommandBool = True
    Else
        CommandBool = False
    End If

End Function

Public Function CommandString(ByVal Command As String) As String
    Dim buf As String
    
    m_Serial.ClearBuffers                           ' Clear remaining junk in buffer
    m_Serial.Transmit "#:" & Command & "#"
    buf = m_Serial.ReceiveTerminated("#")
    If buf <> "" Then                   ' Overflow protection
        CommandString = Left$(buf, Len(buf) - 1)   ' Strip '#'
    Else
        CommandString = ""
    End If

End Function

Public Sub FindHome()

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method FindHome()" & MSG_NOT_IMPLEMENTED

End Sub

Public Sub Park()
    
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method Park()" & MSG_NOT_IMPLEMENTED

End Sub

Public Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Long)
    Dim d As String
    
    Me.CommandBlind "RG"                ' Assure we're in Guide rate
    Select Case Direction
        Case guideNorth:    d = "n"
        Case guideSouth:    d = "s"
        Case guideEast:     d = "e"
        Case guideWest:     d = "w"
    End Select
    Me.CommandBlind "M" & d
    m_Util.WaitForMilliseconds Duration
    Me.CommandBlind "Q" & d
    m_Util.WaitForMilliseconds 200
    Me.CommandBlind "Q" & d
    
End Sub

Public Sub SetPark()

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method SetPark()" & MSG_NOT_IMPLEMENTED

End Sub

Public Sub SetupDialog()

    If m_Serial.Connected Then _
        Err.Raise SCODE_SETUP_CONNECTED, _
                    ERR_SOURCE, _
                    MSG_SETUP_CONNECTED

    frmSetup.m_DriverID = ID                    ' Form uses Helper.Profile methods
    Set frmSetup.m_Profile = m_Profile
    frmSetup.Show 1
    Unload frmSetup
    
End Sub

Public Sub SlewToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)

    CheckConnected
    Me.TargetRightAscension = RightAscension
    Me.TargetDeclination = Declination
    Me.SlewToTarget
    
End Sub

Public Sub SlewToCoordinatesAsync(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
    
    CheckConnected
    Me.TargetRightAscension = RightAscension
    Me.TargetDeclination = Declination
    Me.SlewToTargetAsync
    
End Sub

Public Sub SlewToTarget()

    CheckConnected
    Me.SlewToTargetAsync                        ' Start slew
    While Me.Slewing                            ' Wait for slew to complete
        m_Util.WaitForMilliseconds 500          ' Be responsive to AbortSlew()
        DoEvents                                ' Assure Timer runs!
    Wend

End Sub

Public Sub SlewToTargetAsync()
    Dim stat As Integer
    Dim msg As String
    Dim code As Long
    Dim buf As String
    Dim I As Integer
    Dim tmo As Long
    Dim RA As Double, Dec As Double
    
    CheckConnected
    m_bAbortSlew = False
    m_bStartingSlew = True                              ' We're TRYING to get the thing to slew...
    '
    ' This is where all the work is done for slewing. The other
    ' methods eventually call this one. At least one controller
    ' (FS2) needs *2* slew commands to work. There are timing gaps
    ' between reading the RA & Dec and issuing the MS commands,
    ' again for some controllers that choke on closely spaced
    ' commands.
    '
    m_dPrevRA = Me.RightAscension
    m_dPrevDec = Me.Declination
    tmo = m_Serial.ReceiveTimeout
    m_Serial.ReceiveTimeout = 5
    If TestSlew(m_dPrevRA, m_dPrevDec, Me.TargetRightAscension, Me.TargetDeclination, _
                    m_dRARes * 2#, m_dDecRes * 2#) Then
        For I = 0 To 4                                 ' Total of 20 sec...
            On Error GoTo CMD_TIMEOUT
            m_Util.WaitForMilliseconds 200
            Me.CommandBlind "MS"
            stat = CInt(m_Serial.ReceiveCounted(1))     ' Get only the start-status digit
            m_Serial.ReceiveTimeout = tmo               ' Restore timeout
            If stat <> 0 Then   ' Failed to start slew
                Select Case stat
                    Case 1:                             ' Below horizon
                        msg = MSG_BELOW_HORIZ
                        code = SCODE_BELOW_HORIZ
                    Case 2:                             ' Below minimum elevation
                        msg = MSG_BELOW_MIN_EL
                        code = SCODE_BELOW_MIN_EL
                    Case Else
                        msg = MSG_SLEW_FAIL
                        code = SCODE_SLEW_FAIL
                End Select
                m_bSlewing = False                      ' Clear slewing flag
                m_bStartingSlew = False
                m_Serial.ReceiveTimeout = tmo           ' Restore timeout
                Err.Raise code, ERR_SOURCE, msg         ' FAILED
            End If
            m_Util.WaitForMilliseconds 1000             ' Wait for scope to move some
            If TestSlew(m_dPrevRA, m_dPrevDec, Me.RightAscension, Me.Declination, _
                                    m_dRARes, m_dDecRes) Then Exit For
            GoTo CMD_RESUME
CMD_TIMEOUT:
            m_Util.WaitForMilliseconds 500
            Resume CMD_RESUME
CMD_RESUME:
            On Error GoTo 0
        Next I
        m_Serial.ReceiveTimeout = tmo                   ' Restore timeout
        If I <= 6 Then                                  ' Success
            m_lAsyncEndTix = GetTickCount() + (CLng(m_iSettleTime + 4) * 1000&)
            m_bSlewing = True                           ' But still say we're slewing
            m_bStartingSlew = False
            Exit Sub                                    ' Succeeded
        Else
            m_bSlewing = False
            m_bStartingSlew = False
            Err.Raise SCODE_SLEW_FAIL, ERR_SOURCE, MSG_SLEW_FAIL    ' Failed!
        End If
    End If
    m_bSlewing = False                                  ' Too small to slew
    m_bStartingSlew = False
End Sub

Public Sub SyncToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
                                    
    CheckConnected
    Me.TargetRightAscension = RightAscension
    Me.TargetDeclination = Declination
    Me.SyncToTarget
    
End Sub

Public Sub SyncToTarget()

    Me.CommandString "CM"     ' Accept anything including ""

End Sub

Public Sub Unpark()

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method Unpark()" & MSG_NOT_IMPLEMENTED

End Sub


' ========================
' PRIVATE HELPER FUNCTIONS
' ========================
'
'
' CheckConnected() - Raise an error if the scope is not connected
'
Private Sub CheckConnected()

    If Not m_Serial.Connected Then
        Err.Raise SCODE_NOT_CONNECTED, _
                    ERR_SOURCE, _
                    MSG_NOT_CONNECTED
    End If
    
End Sub
'
' SetLongFormat() - Set scope "long" format on or off
'
' The scope has only a toggle for the format, so we need to
' first test the format and then conditionally issue the
' toggle to set the desired format.
'
' Returns the format after the function is called. Some
' LX200 emulators don't support long format.
'
Private Function SetLongFormat(b As Boolean) As Boolean
    Dim buf As String
    
    buf = Me.CommandString("GR")            ' Get RA
    '
    ' Scope has some sort of race condition with "U", so
    ' we just wait a half sec. here and then do it.
    '
    m_Util.WaitForMilliseconds 500
    If ((b And (Len(buf) <= 7)) Or (Not b And (Len(buf) > 7))) Then
        Me.CommandBlind "U"                 ' change setting
    End If
    
    buf = Me.CommandString("GR")            ' Check final result
    If Len(buf) <= 7 Then
        SetLongFormat = False               ' Now/still in short format
    Else
        SetLongFormat = True                ' Now in long format
    End If
    
End Function

'
' Test to see if the differences in RA and Dec exceed the
' corresponding resolution.
'
Private Function TestSlew(ra1 As Double, dec1 As Double, _
                            ra2 As Double, dec2 As Double, _
                            raRes As Double, decRes As Double) As Boolean
    Dim diff As Double

    diff = ra2 - ra1
    If diff > 12# Then diff = 24# - diff
    If diff < -12# Then diff = diff + 24#
    If (Abs(diff) * 15#) > raRes Then
        TestSlew = True                     ' Difference in RA is enough
        Exit Function
    End If
    
    diff = Abs(dec2 - dec1)
    If diff > decRes Then
        TestSlew = True                     ' Difference in Dec is enough
        Exit Function
    End If
    
    TestSlew = False
    
End Function

'
' Trim leading/trailing spaces from delimiters. If the result is
' "" then the delimiter was a single space, add that back in.
'
Private Function DelimTrim(s As String) As String

    DelimTrim = Trim$(s)
    If DelimTrim = "" Then DelimTrim = " "
    
End Function

