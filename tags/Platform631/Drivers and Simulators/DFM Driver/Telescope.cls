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
Attribute VB_Description = "ASCOM Telescope Driver for DFM Telescope Control System"
'---------------------------------------------------------------------
'   =============
'   TELESCOPE.CLS
'   =============
'
' Implementation of the ASCOM telescope driver Telescope class
' for DFM Engineering's Astronomy Command Language (ACL).
'
' Written:  13-Oct-08   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 13-Oct-08 rbd     5.0.1 - Initial edit, using the existing ASCOM
'                   ACL driver as a base. Previous edit trails from
'                   sources have been removed. Add track offset supt.
' 14-Oct-08 rbd     5.0.1 - Reset track rates to defaults on disconnect
'                   or (last chance) object destructor.
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

Private Const ID As String = "DFM.Telescope"
Private Const DESC As String = "DFM Telescope"

Private Const SIDRATE As Double = 0.9972695677          ' UTC seconds per sidereal sec

Private NL As String                ' Newline

Private m_eAlignMode As DFM.AlignmentModes
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

Private m_dDefTrackRateRA As Double                     ' Read at connect, TCS can adjust default rate
Private m_dCurTrackRateRA As Double                     ' Absolute rates per TCS, arcsec per UTC second
Private m_dCurTrackRateDE As Double
Private m_lLastRatesTix As Long

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
    m_Profile.DeviceType = "Telescope"                  ' We're a Telescope driver
    m_bConnected = False
    m_bObjRAValid = False
    m_bObjDecValid = False
    m_iSettleTime = 0                                   ' Default no slew settling time
    m_Profile.Register ID, DESC                         ' Self-register if needed
    On Error Resume Next
    m_Profile.DeleteSubKey ID, "General"                ' Delete old beta fossil stuff
    
End Sub

Private Sub Class_Terminate()
    
    On Error Resume Next
    If m_bConnected Then
        Me.RightAscensionRate = 0#                      ' Leave TCS at default rate
        Me.DeclinationRate = 0#
    End If
    Me.Connected = False                                ' Force disconnect
    
End Sub

Public Property Get AlignmentMode() As DFM.AlignmentModes
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
        m_Serial.Speed = ps9600                         ' DFMTCS uses 9600
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
        ' (3) Determine that there is a DFM TCS there.
        '
        Me.CommandString "status"                       ' Do a status
        '
        ' (4) Read the tracking rates, to establish the default or
        '     "sidereal" tracking rate. The TCS operator may adjust
        '     this a bit. We want to return to the default when
        '     canceling track offsets.
        '
        m_bConnected = True                             ' Hooray! We're connected
        GetTrackRates                                   ' Populate m_dCurTrackXX globals (needs m_bConnected)
        m_dDefTrackRateRA = m_dCurTrackRateRA           ' Save as default RA rate
    Else                                                ' DISCONNECTING
        On Error Resume Next                            ' Best efforts...
        If m_bConnected Then
            Me.RightAscensionRate = 0#                  ' Leave TCS at default rate
            Me.DeclinationRate = 0#
        End If
        m_bConnected = False
        m_Serial.ClearBuffers                           ' Clear serial buffers
        m_Serial.Connected = False                      ' Release COM port
    End If
    
End Property

Public Property Get Declination() As Double

    CheckConnected
    Declination = m_Util.DMSToDegrees(Me.CommandString("get dec"))

End Property

Public Property Get DeclinationRate() As Double

    CheckConnected
    GetTrackRates
    DeclinationRate = m_dCurTrackRateDE
    
End Property

Public Property Let DeclinationRate(ByVal newVal As Double)

    CheckConnected
    m_dCurTrackRateDE = newVal
    CommandBlind "set x_dfmtrackrates = " & _
            Format$(m_dCurTrackRateRA, "0.000") & " " & _
            Format$(m_dCurTrackRateDE, "0.000")
            
    m_lLastRatesTix = GetTickCount()                    ' Update poll timer
            
End Property

Public Property Get Description() As String

    CheckConnected
    Description = "DFM Telescope"
    
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
    
    Name = "DFM"

End Property

Public Property Get RightAscension() As Double

    CheckConnected
    RightAscension = m_Util.DMSToDegrees(Me.CommandString("get ra"))

End Property

Public Property Get RightAscensionRate() As Double

    CheckConnected
    GetTrackRates
    RightAscensionRate = (m_dCurTrackRateRA - m_dDefTrackRateRA) * SIDRATE

End Property

Public Property Let RightAscensionRate(ByVal newVal As Double)

    CheckConnected
    m_dCurTrackRateRA = m_dDefTrackRateRA + (newVal / SIDRATE)
    CommandBlind "set x_dfmtrackrates = " & _
            Format$(m_dCurTrackRateRA, "0.000") & " " & _
            Format$(m_dCurTrackRateDE, "0.000")         ' Checks connected

    m_lLastRatesTix = GetTickCount()                    ' Update poll timer
            
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
        Slewing = True                                  ' Fake slewing for settle time
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
    
    If newVal Then                                      ' Turn tracking XXX=>and PEC<= on
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

    Me.CommandString "stop"                             ' Halt the slew
    m_bAbortSlew = True                                 ' Raise the abort flag
    
End Sub

Public Sub CommandBlind(ByVal Command As String)

    Me.CommandString Command            ' Just toss any results
    
End Sub

Public Function CommandBool(ByVal Command As String) As Boolean
    Dim c As Long
    Dim d As String, s As String
    
    On Error GoTo CMDBOOL_ERR                           ' This is sort of cheesy
    
    Me.CommandString Command                            ' Only goto errors are non-fatal
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
    
    m_MNCP.ToSlave Command                              ' No NL at end...
    buf = m_MNCP.FromSlave()
    If LCase$(Left$(buf, 5)) = "error" Then             ' Returned an error message
        Select Case CInt(Trim$(Mid$(buf, 6)))           ' Numeric error code
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
        Err.Raise SCODE_NOT_ACL, ERR_SOURCE, MSG_NOT_ACL ' If not "ok" then not ACL!
    '
    ' OK was received, the remainder is the returned string
    '
    CommandString = Trim$(Mid$(buf, 3))                 ' Remove "ok " and trim
    
End Function

Public Sub FindHome()

    Me.CommandString "home find"
    While Me.Slewing                                    ' Wait for slew to complete
        m_Util.WaitForMilliseconds 1000                 ' Be responsive to AbortSlew()
    Wend

End Sub

Public Sub Park()

    Me.CommandString "home park"
    While Me.Slewing                                    ' Wait for slew to complete
        m_Util.WaitForMilliseconds 1000                 ' Be responsive to AbortSlew()
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

    frmSetup.m_DriverID = ID                            ' Form calls Profile object methods
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

    Me.SlewToTargetAsync                                ' Start slew
    While Me.Slewing                                    ' Wait for slew to complete
        m_Util.WaitForMilliseconds 1000                 ' Be responsive to AbortSlew()
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

'
' Get the status word
'
Private Function GetStatus() As Long

    CheckConnected
    GetStatus = CLng("&H" & CommandString("status"))   ' Returns hex status word
    
End Function

'
' Get tracking rates, put into globals
'
' ACL returns "xx.xxx yy.yyy" where xx.xxx is the total RA rate (e.g. 15.001)
' and yy.yyy is the total Dec rate (e.g. 00.000).
'
Private Sub GetTrackRates()
    Dim buf As String
    Dim bits() As String
    
    CheckConnected
    If (GetTickCount() - m_lLastRatesTix) < 1000 Then Exit Sub
    
    buf = CommandString("get x_dfmtrackrates")
    bits = Split(buf, " ")
    If UBound(bits) <> 1 Then _
        Err.Raise SCODE_TRKRNG_FMT_ERROR, _
                    ERR_SOURCE, _
                    MSG_TRKRNG_FMT_ERROR
    m_dCurTrackRateRA = CDbl(Trim$(bits(0)))
    m_dCurTrackRateDE = CDbl(Trim$(bits(1)))
    
    m_lLastRatesTix = GetTickCount()
    
End Sub
