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
Attribute VB_Description = "ASCOM Telescope Driver for COMSOFT PC-TCS"
'---------------------------------------------------------------------
'   =============
'   TELESCOPE.CLS
'   =============
'
' Implementation of the ASCOM telescope driver Telescope class
' for COMSOFT PC-TCS (which uses Merlin ACL).
'
' Written:  29-Mar-06   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 29-Mar-06 rbd     4.0.3 - From ACL driver, shared MNCP, etc.
' 30-Aug-06 rbd     4.0.6 - Move ASCOM registration of all interfaces
'                   to app startup.
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

Private NL As String                ' Newline

Private m_eAlignMode As AlignmentModes
Private m_bAbortSlew As Boolean
Private m_iSettleTime As Integer
Private m_lSlewStartTix As Long
Private m_lSlewSettleTix As Long

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
    m_bObjRAValid = False
    m_bObjDecValid = False
    m_iSettleTime = 0                                       ' Default no slew settling time
    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Telescope"                      ' We're a Telescope driver
    
End Sub

Private Sub Class_Terminate()
    
    On Error Resume Next
    CloseConnection devTelescope
    
End Sub

Public Property Get AlignmentMode() As AlignmentModes
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

    Connected = IsConnected(devTelescope)
    
End Property

Public Property Let Connected(ByVal newVal As Boolean)
    
    If newVal Then
        OpenConnection devTelescope
    Else
        CloseConnection devTelescope
    End If
    
End Property

Public Property Get Declination() As Double

    CheckConnected
    Declination = g_Util.DMSToDegrees(C_CommandString("get dec"))

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
    Description = "PC-TCS Controlled telescope"
    
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
    
    Name = "PC-TCS"

End Property

Public Property Get RightAscension() As Double

    CheckConnected
    RightAscension = g_Util.DMSToDegrees(C_CommandString("get ra"))

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
    SiderealTime = g_Util.HMSToHours(C_CommandString("get sidereal"))

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
    SiteLatitude = CDbl(C_CommandString("get latitude"))

End Property

Public Property Let SiteLatitude(ByVal newVal As Double)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Changing of SiteLatitude" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get SiteLongitude() As Double

    CheckConnected
    SiteLongitude = CDbl(C_CommandString("get longitude"))

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
    C_CommandString "set objectdec=" & m_dObjectDec
    
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
    C_CommandString "set objectra=" & m_dObjectRA

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
    
    C_CommandString "set flag[0]=" & bv
   
End Property

Public Property Get UTCDate() As Date

    UTCDate = CDate(CDbl(Now()) + (CDbl(utc_offs()) / 86400#))

End Property

Public Property Let UTCDate(ByVal newVal As Date)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property UTCDate" & MSG_NOT_IMPLEMENTED

End Property

Public Sub AbortSlew()

    C_CommandString "stop"                                    ' Halt the slew
    m_bAbortSlew = True                                     ' Raise the abort flag
    
End Sub


Public Sub FindHome()

    C_CommandString "home find"
    While Me.Slewing                                        ' Wait for slew to complete
        g_Util.WaitForMilliseconds 1000                     ' Be responsive to AbortSlew()
    Wend

End Sub

Public Sub Park()

    C_CommandString "home park"
    While Me.Slewing                                        ' Wait for slew to complete
        g_Util.WaitForMilliseconds 1000                     ' Be responsive to AbortSlew()
    Wend

End Sub

Public Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Long)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method PulseGuide()" & MSG_NOT_IMPLEMENTED

End Sub

Public Sub SetPark()

    C_CommandString "home setup"

End Sub

Public Sub SetupDialog()

    If SerialConnected Then _
        Err.Raise SCODE_SETUP_CONNECTED, _
                    ERR_SOURCE, _
                    MSG_SETUP_CONNECTED
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

    Me.SlewToTargetAsync                                    ' Start slew
    While Me.Slewing                                        ' Wait for slew to complete
        g_Util.WaitForMilliseconds 1000         ' Be responsive to AbortSlew()
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
            g_Util.WaitForMilliseconds 1000
        Wend
        Me.TargetRightAscension = m_dObjectRA
        Me.TargetDeclination = m_dObjectDec
        g_Util.WaitForMilliseconds 500
    End If
    C_CommandString "goto"                                ' Start the slew
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
    C_CommandString ("sync")

End Sub

Public Sub Unpark()

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method Unpark()" & MSG_NOT_IMPLEMENTED

End Sub

Public Function CommandString(ByVal cmd As String) As String

    CommandString = C_CommandString(cmd)

End Function

Public Function CommandBool(ByVal cmd As String) As Boolean

    CommandBool = C_CommandBool(cmd)

End Function

Public Sub CommandBlind(ByVal cmd As String)

    C_CommandBlind cmd

End Sub

' ========================
' PRIVATE HELPER FUNCTIONS
' ========================

'
' CheckConnected() - Raise an error if the scope is not connected
'
Private Sub CheckConnected()

    If Not IsConnected(devTelescope) Then _
        Err.Raise SCODE_NOT_CONNECTED, _
                    ERR_SOURCE, _
                    MSG_NOT_CONNECTED
End Sub

Private Function GetStatus() As Long

    CheckConnected
    GetStatus = CLng("&H" & C_CommandString("status"))   ' Returns hex status word
    
End Function
