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
Attribute VB_Description = "ASCOM Telescope Driver for Astronomy Command Language"
'---------------------------------------------------------------------
'   =============
'   TELESCOPE.CLS
'   =============
'
' Implementation of the ASCOM telescope driver Telescope class
' for Merlin Controls' Astronomy Command Language (ACL).
'
' Written:  12-Oct-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 12-Oct-00 rbd     Initial edit, hardwired for Puckett Obs. COMSOFT
'                   is adding the ability to change the lat/long of
'                   of PC-TCS via ACL. Based on the ASCOM VB telescope
'                   driver template.
' 13-Oct-00 rbd     Description returns String not Double. Park() does
'                   not take a command.
' 21-Nov-00 rbd     1.0.1 PC-TCS now supports lat/long/ST.
'                   DriverInfo is a String, add standard code.
' 12-Dec-00 rbd     Fix SiderealTime property
' 13-Jan-01 rbd     Fill in missing ERR_SOURCE argument in calls to
'                   Err.Raise for "not implemented". There were many!
'                   Specific messages for not implemented.
' 21-Jan-01 rbd     Use Helper.Profile for persistence, no more
'                   Registry.bas module. No More General key.
'                   Use new Helper.Util.LoadDLL() and
'                   Helper.Util.WaitForMilliseconds() to eliminate the
'                   Win32.bas module from here.
' 25-Jan-01 rbd     New properties CanSlew, CanSlewAsync, CanSync,
'                   CanPark, CanFindHome, CanSetPark, CanUnpark
'                   Interface V1.1
' 28-Jan-00 rbd     Add description and version info display to
'                   SetupDialog box.
' 31-Jan-00 rbd     Fix Sidereal time to send back hours.
' 06-Feb-01 rbd     Add Tracking, CanSetTracking. Reword the
'                   not implemented messsages for -let
' 30-Apr-01 rbd     Back off on slew check rate while waiting for
'                   slew to stop. Now 1Hz.
' 08-Jun-01 rbd     New Profile.DeviceType to generalize Chooser and
'                   Profile.
' 08-Jun-01 rbd     Telescope 1.4: Add PulseGuide() & CanPulseGuide
' 05-Oct-01 rbd     1.4.2 - Add ByVals for parameters where missing.
' 14-Oct-01 rbd     1.4.3 - Fix overflow in slew settling
' 09-May-02 rbd     1.4.5 - Park & FindPark must be synchronous. Add
'                   tracking control. Implement CommandBlind() and
'                   CommandBool().
' 12-Jun-02 rbd     1.4.6 - Remove PEC control per T. Puckett. Turn
'                   only the tracking bit on and off.
' 08-Aug-02 rbd     2.0.1 - For DFM... When slewing, do not call
'                   AbortSlew() unless actually slewing. Also save
'                   target RA/Dec locally. If slewing, Call AbortSlew()
'                   then re-send the target RA/Dec before starting
'                   new slew. The DFM TCS invalidates the target RA/Dec
'                   if it receives an ACL 'stop' command.
' 21-Mar-03 rbd     2.2.1 - Version change for Platform 2.2
' 24-Jun-03 rbd     2.3.1 - Version change for Platform 2.3
' 03-Sep-03 rbd     3.0.1 - Version change for Platform 3.0
' 23-Nov-04 rbd     4.0.1 - Version change for Platform 4.0
' 29-Mar-06 rbd     4.0.3 - PC-TCS driver (new), shared serial for
'                   Telescope, Dome, Focuser
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

Private Enum StatusBits
    stTrackingEnabled = 1
    stSlewInProgress = 2
    stHomeInProgress = 4
    stLastHomeSearchFailed = 8
    stEquatorialMode = 16
    stPassedHiAzRaLimit = 256
    stPassedLoAzRaLimit = 512
    stPassedHiAltDecLimit = 1024
    stPassedLoAltDecLimit = 2048
    stHitHardLimit = 32768
    stPECEnabled = 8388608
End Enum

Private Const ID As String = "ACL.Telescope"
Private Const DESC As String = "ACL Based Telescope"

Private NL As String                ' Newline

Private m_eAlignMode As ACL.AlignmentModes
Private m_bAbortSlew As Boolean
Private m_iSettleTime As Integer
Private m_lSlewStartTix As Long
Private m_lSlewSettleTix As Long

Private m_Serial As DriverHelper.Serial
Private m_Util As DriverHelper.Util
Private m_MNCP As MNCP
Private m_bConnected As Boolean

Private m_dObjectRA As Double
Private m_dObjectDec As Double
Private m_bObjRAValid As Boolean
Private m_bObjDecValid As Boolean

Private m_Profile As DriverHelper.Profile

'
' Locally used Win32 API
'
Private Declare Function GetTickCount Lib "kernel32" () As Long

Private Sub Class_Initialize()

    NL = Chr$(10)
    Set m_Serial = New DriverHelper.Serial
    Set m_Util = New DriverHelper.Util
    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Telescope"      ' We're a Telescope driver
    m_bConnected = False
    m_bObjRAValid = False
    m_bObjDecValid = False
    m_iSettleTime = 0                       ' Default no slew settling time
    m_Profile.Register ID, DESC             ' Self-register if needed
    On Error Resume Next
    m_Profile.DeleteSubKey ID, "General"    ' Delete old beta fossil stuff
    
End Sub

Private Sub Class_Terminate()
    
    On Error Resume Next
    Me.Connected = False                    ' Force disconnect
    
End Sub

Public Property Get AlignmentMode() As ACL.AlignmentModes
    Dim stat As Long
    
    If (GetStatus() And stEquatorialMode) <> 0 Then
        AlignmentMode = algPolar
    Else
        AlignmentMode = algAltAz
    End If
    
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

    CanFindHome = True

End Property

Public Property Get CanPark() As Boolean

    CanPark = True
    
End Property

Public Property Get CanPulseGuide() As Boolean

    CanPulseGuide = False
    
End Property

Public Property Get CanSlew() As Boolean

    CanSlew = True
    
End Property

Public Property Get CanSetPark() As Boolean

    CanSetPark = True

End Property

Public Property Get CanSetTracking() As Boolean

    CanSetTracking = True

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

    Connected = m_bConnected
    
End Property

Public Property Let Connected(ByVal newVal As Boolean)
    Dim buf As String
    
    m_bConnected = False                                ' Assume failure
    
    If newVal Then                                      ' CONNECTING
        m_Util.LoadDLL App.Path & "\astro32.dll"        ' Assure this is loaded and available
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
        m_Serial.ReceiveTimeout = 2                     ' 2 second timeout
        m_Serial.Connected = True                       ' Grab the serial port
        '
        ' (2) Initialize the MNCP protocol.
        '
        Set m_MNCP = New MNCP
        Set m_MNCP.SerialObject = m_Serial
        m_MNCP.Address = 1
        m_MNCP.Retries = 4
        m_MNCP.ResetSlave                               ' Try to talk to the slave
        '
        ' (3) Determine that there is an ACL scope there.
        '
        Me.CommandString "status"                       ' Do a status
        m_bConnected = True                             ' Hooray! We're connected
    Else                                                ' DISCONNECTING
        On Error Resume Next                            ' Best efforts...
        m_Serial.ClearBuffers                           ' Clear serial buffers
        m_Serial.Connected = False                      ' Release COM port
    End If
    
End Property

Public Property Get Declination() As Double

    CheckConnected
    Declination = m_Util.DMSToDegrees(Me.CommandString("get dec"))

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

    CheckConnected
    Description = "ACL based telescope"
    
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
    
    Name = "ACL"

End Property

Public Property Get RightAscension() As Double

    CheckConnected
    RightAscension = m_Util.DMSToDegrees(Me.CommandString("get ra"))

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

    CheckConnected
    SiderealTime = m_Util.HMSToHours(Me.CommandString("get sidereal"))

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

    CheckConnected
    SiteLatitude = CDbl(Me.CommandString("get latitude"))

End Property

Public Property Let SiteLatitude(ByVal newVal As Double)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Changing of SiteLatitude" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get SiteLongitude() As Double

    CheckConnected
    SiteLongitude = CDbl(Me.CommandString("get longitude"))

End Property

Public Property Let SiteLongitude(ByVal newVal As Double)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Changing of SiteLongitude" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get Slewing() As Boolean

    If (GetStatus() And stSlewInProgress) <> 0 Then
        m_lSlewSettleTix = GetTickCount() + (CLng(m_iSettleTime) * 1000&)  ' Re-up the settling ticks
        Slewing = True
    ElseIf (GetTickCount() < m_lSlewSettleTix) And Not m_bAbortSlew Then
        Slewing = True                              ' Fake slewing for settle time
    Else
        Slewing = False
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
    If Not m_bObjDecValid Then _
        Err.Raise SCODE_PROP_NOT_SET, ERR_SOURCE, MSG_PROP_NOT_SET
    TargetDeclination = m_dObjectDec

End Property

Public Property Let TargetDeclination(ByVal newVal As Double)

    CheckConnected
    If newVal < -90# Or newVal > 90# Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, MSG_PROP_RANGE_ERROR
    m_dObjectDec = newVal
    m_bObjDecValid = True
    Me.CommandString "set objectdec=" & m_dObjectDec
    
End Property

Public Property Get TargetRightAscension() As Double

    CheckConnected
    If Not m_bObjRAValid Then _
        Err.Raise SCODE_PROP_NOT_SET, ERR_SOURCE, MSG_PROP_NOT_SET
    TargetRightAscension = m_dObjectRA

End Property

Public Property Let TargetRightAscension(ByVal newVal As Double)

    CheckConnected
    If newVal < 0# Or newVal >= 24# Then _
        Err.Raise SCODE_PROP_RANGE_ERROR, ERR_SOURCE, MSG_PROP_RANGE_ERROR
    m_dObjectRA = newVal
    m_bObjRAValid = True
    Me.CommandString "set objectra=" & m_dObjectRA

End Property

Public Property Get Tracking() As Boolean

    If (GetStatus() And stTrackingEnabled) <> 0 Then
        Tracking = True
    Else
        Tracking = False
    End If
    
End Property

Public Property Let Tracking(ByVal newVal As Boolean)
    Dim bv As Integer
    
    If newVal Then                  ' Turn tracking XXX=>and PEC<= on
        bv = 1
    Else
        bv = 0
    End If
    
    Me.CommandString "set flag[0]=" & bv
   
End Property

Public Property Get UTCDate() As Date

    UTCDate = CDate(CDbl(Now()) + (CDbl(utc_offs()) / 86400#))

End Property

Public Property Let UTCDate(ByVal newVal As Date)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property UTCDate" & MSG_NOT_IMPLEMENTED

End Property

Public Sub AbortSlew()

    Me.CommandString "stop"                                 ' Halt the slew
    m_bAbortSlew = True                                     ' Raise the abort flag
    
End Sub

Public Sub CommandBlind(ByVal Command As String)

    Me.CommandString Command            ' Just toss any results
    
End Sub

Public Function CommandBool(ByVal Command As String) As Boolean
    Dim c As Long
    Dim d As String, s As String
    
    On Error GoTo CMDBOOL_ERR           ' This is sort of cheesy
    
    Me.CommandString Command            ' Only goto errors are non-fatal
    CommandBool = True
    Exit Function
    
CMDBOOL_ERR:
    c = Err.Number
    d = Err.Description
    s = Err.Source
    Resume CMDBOOL_RSUM
    
CMDBOOL_RSUM:
    On Error GoTo 0
    CommandBool = False
    If c <> SCODE_ACLERR Then
        Err.Raise c, s, d
    ElseIf LCase$(Left$(d, 4)) <> "goto" Then
        Err.Raise c, s, d
    Else
        CommandBool = True
    End If
    
End Function

Public Function CommandString(ByVal Command As String) As String
    Dim buf As String
    
    m_MNCP.ToSlave Command                                  ' No NL at end...
    buf = m_MNCP.FromSlave()
    If LCase$(Left$(buf, 5)) = "error" Then                 ' Returned an error message
        Select Case CInt(Trim$(Mid$(buf, 6)))               ' Numeric error code
            ' ACL core errors
            Case 0:     buf = "No error"
            Case 1:     buf = "Undefined error"
            Case 2:     buf = "Syntax error"
            Case 10:    buf = "Type Mismatch"
            Case 12:    buf = "Value is read-only"
            Case 13:    buf = "Unsupported command"
            Case 14:    buf = "Unsupported identifier"
            Case 15:    buf = "Command inactive"
            ' ACL Telescope errors
            Case 100:   buf = "Goto - illegal parameter(s)"
            Case 101:   buf = "Goto - object below horizon"
            Case 102:   buf = "Goto - object outside limits"
            ' Others are reserved, should never be seen (ha ha ha)
            Case Else:  buf = "Reserved error code, you shouldn't see this!"
        End Select
        Err.Raise SCODE_ACLERR, ERR_SOURCE, _
                "Low level ACL error from telescope: """ & buf & """"
    End If
    If LCase$(Left$(buf, 2)) <> "ok" Then _
        Err.Raise SCODE_NOT_ACL, ERR_SOURCE, MSG_NOT_ACL    ' If not "ok" then not ACL!
    '
    ' OK was received, the remainder is the returned string
    '
    CommandString = Trim$(Mid$(buf, 3))             ' Remove "ok " and trim
    
End Function

Public Sub FindHome()

    Me.CommandString "home find"
    While Me.Slewing                            ' Wait for slew to complete
        m_Util.WaitForMilliseconds 1000         ' Be responsive to AbortSlew()
    Wend

End Sub

Public Sub Park()

    Me.CommandString "home park"
    While Me.Slewing                            ' Wait for slew to complete
        m_Util.WaitForMilliseconds 1000         ' Be responsive to AbortSlew()
    Wend

End Sub

Public Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Long)

    '** insert your implementation here and remove the following line
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method PulseGuide()" & MSG_NOT_IMPLEMENTED

End Sub

Public Sub SetPark()

    Me.CommandString "home setup"

End Sub

Public Sub SetupDialog()

    If m_bConnected Then _
        Err.Raise SCODE_SETUP_CONNECTED, _
                    ERR_SOURCE, _
                    MSG_SETUP_CONNECTED

    frmSetup.m_DriverID = ID                    ' Form calls Profile object methods
    Set frmSetup.m_Profile = m_Profile
    frmSetup.Show 1
    Unload frmSetup
    
End Sub

Public Sub SlewToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)

    Me.TargetRightAscension = RightAscension
    Me.TargetDeclination = Declination
    Me.SlewToTarget
    
End Sub

Public Sub SlewToCoordinatesAsync(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
    
    Me.TargetRightAscension = RightAscension
    Me.TargetDeclination = Declination
    Me.SlewToTargetAsync
    
End Sub

Public Sub SlewToTarget()

    Me.SlewToTargetAsync                        ' Start slew
    While Me.Slewing                            ' Wait for slew to complete
        m_Util.WaitForMilliseconds 1000         ' Be responsive to AbortSlew()
    Wend

End Sub

Public Sub SlewToTargetAsync()
    Dim stat As Integer
    Dim msg As String
    Dim code As Long
    Dim buf As String
    
    CheckConnected
    
    If (Not m_bObjRAValid) Or (Not m_bObjDecValid) Then _
        Err.Raise SCODE_TARGET_NOT_SET, ERR_SOURCE, MSG_TARGET_NOT_SET
        
    '
    ' This is where all the work is done for slewing. The other
    ' methods eventually call this one.
    '
    If Me.Slewing Then                                  ' If slewing
        Me.AbortSlew                                    ' Stop any slew in progress!
        While Me.Slewing                                ' Complete slew cycle as necessary
            m_Util.WaitForMilliseconds 1000
        Wend
        Me.TargetRightAscension = m_dObjectRA
        Me.TargetDeclination = m_dObjectDec
        m_Util.WaitForMilliseconds 500
    End If
    Me.CommandString "goto"                             ' Start the slew
    m_bAbortSlew = False
    m_lSlewSettleTix = GetTickCount() + (CLng(m_iSettleTime) * 1000&)  ' Re-up the settling ticks
    m_lSlewStartTix = GetTickCount()                    ' Mark time of slew start
    
End Sub

Public Sub SyncToCoordinates(ByVal RightAscension As Double, _
                                    ByVal Declination As Double)
                                    
    Me.TargetRightAscension = RightAscension
    Me.TargetDeclination = Declination
    Me.SyncToTarget
    
End Sub

Public Sub SyncToTarget()

    CheckConnected
    Me.CommandString ("sync")

End Sub

Public Sub Unpark()

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method Unpark()" & MSG_NOT_IMPLEMENTED

End Sub

' ========================
' PRIVATE HELPER FUNCTIONS
' ========================

'
' CheckConnected() - Raise an error if the scope is not connected
'
Private Sub CheckConnected()

    If Not m_bConnected Then _
        Err.Raise SCODE_NOT_CONNECTED, _
                    ERR_SOURCE, _
                    MSG_NOT_CONNECTED
End Sub

Private Function GetStatus() As Long

    CheckConnected
    GetStatus = CLng("&H" & CommandString("status"))   ' Returns hex status word
    
End Function
