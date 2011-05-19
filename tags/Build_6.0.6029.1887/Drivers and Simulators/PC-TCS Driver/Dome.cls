VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Dome"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ASCOM Dome driver for COMSOFT PC-TCS"
'---------------------------------------------------------------------
'   ========
'   DOME.CLS
'   ========
'
' Implementation of the ASCOM Dome Class for PC-TCS
'
' Written:  29-Mar-06   Bob Denny
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 29-Mar-06 rbd     Initial edit
' 30-Aug-06 rbd     4.0.6 - Move ASCOM registration of all interfaces
'                   to app startup.
'---------------------------------------------------------------------

Option Explicit

Public Enum ShutterState
    shutterOpen = 0
    shutterClosed = 1
    shutterOpening = 2
    shutterClosing = 3
    shutterError = 4
End Enum

Private m_Profile As DriverHelper.Profile
Private m_fAzimuth As Single
Private m_bSlaved As Boolean
Private m_bMoving As Boolean
Private m_bParked As Boolean
Private m_bHomed As Boolean
Private m_eShutter As ShutterState

' ------------
' Class Events
' ------------

Private Sub Class_Initialize()

    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Dome"                         ' We're a Dome driver
    
End Sub

Private Sub Class_Terminate()

    On Error Resume Next
    CloseConnection devDome

End Sub

' ----------
' Properties
' ----------

Public Property Get Altitude() As Double

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
        "Property Altitude" & MSG_NOT_IMPLEMENTED
    
End Property

Public Property Get AtHome() As Boolean

    GetStatus
    AtHome = m_bHomed
    
End Property

Public Property Get AtPark() As Boolean

    GetStatus
    AtPark = m_bParked
    
End Property

Public Property Get Azimuth() As Double

    Azimuth = CDbl(C_CommandString("dome getaz")) / 10#
    
End Property

Public Property Get CanFindHome() As Boolean

    CanFindHome = True

End Property

Public Property Get CanPark() As Boolean

    CanPark = True
    
End Property

Public Property Get CanSetAltitude() As Boolean

    CanSetAltitude = False
    
End Property

Public Property Get CanSetAzimuth() As Boolean

    CanSetAzimuth = True
    
End Property

Public Property Get CanSetPark() As Boolean

    CanSetPark = True
    
End Property

Public Property Get CanSetShutter() As Boolean

    CanSetShutter = True
    
End Property

Public Property Get CanSlave() As Boolean

    CanSlave = True
    
End Property

Public Property Get CanSyncAzimuth() As Boolean

    CanSyncAzimuth = False
    
End Property

Public Property Get Connected() As Boolean

    Connected = IsConnected(devDome)
    
End Property

Public Property Let Connected(ByVal newVal As Boolean)
    
    If newVal Then
        OpenConnection devDome
    Else
        CloseConnection devDome
    End If
    
End Property

Public Property Get Description() As String

    Description = "PC-TCS Controlled dome"
    
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

Public Property Get InterfaceVersion() As Integer

    InterfaceVersion = 1
    
End Property

Public Property Get Name() As String

    Name = "PC-TCS"
    
End Property

Public Property Get ShutterStatus() As ShutterState

    GetStatus
    ShutterStatus = m_eShutter
    
End Property

Public Property Get Slaved() As Boolean

    GetStatus
    Slaved = m_bSlaved
    
End Property

Public Property Let Slaved(ByVal newVal As Boolean)
        
    CheckConnected
    If newVal Then
        C_CommandString "dome on"
    Else
        C_CommandString "dome off"
    End If
    
End Property

Public Property Get Slewing() As Boolean

    GetStatus
    Slewing = m_bMoving
    
End Property

' -------
' Methods
' -------

Public Sub AbortSlew()

    CheckConnected
    C_CommandString "stop"
    
End Sub

Public Sub CloseShutter()

    CheckConnected
    C_CommandString "dome close"
    
End Sub

Public Sub FindHome()

    CheckConnected
    C_CommandString "dome home"
    
End Sub

Public Sub OpenShutter()

    CheckConnected
    C_CommandString "dome open"
    
End Sub

Public Sub Park()
 
    CheckConnected
    C_CommandString "dome stow"
    
End Sub

Public Sub SetPark()

    CheckConnected
    C_CommandString "dome init"
    
End Sub

Public Sub SetupDialog()
    
    If SerialConnected Then _
        Err.Raise SCODE_SETUP_CONNECTED, _
                    ERR_SOURCE, _
                    MSG_SETUP_CONNECTED
    frmSetup.Show 1
    Unload frmSetup
    
End Sub

Public Sub SlewToAltitude(ByVal Alt As Double)

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
        "Method SlewToAltitude" & MSG_NOT_IMPLEMENTED
        
End Sub

Public Sub SlewToAzimuth(ByVal Az As Double)

    CheckConnected
    C_CommandString "dome setaz " & g_Util.FormatVar(CInt(Az * 10#), "0000")
    
End Sub

Public Sub SyncToAzimuth(ByVal Az As Double)
      
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
        "Method SyncToAzimuth" & MSG_NOT_IMPLEMENTED
 
End Sub

' ========================
' PRIVATE HELPER FUNCTIONS
' ========================

'
' CheckConnected() - Raise an error if the scope is not connected
'
Private Sub CheckConnected()

    If Not IsConnected(devFocuser) Then _
        Err.Raise SCODE_NOT_CONNECTED, _
                    ERR_SOURCE, _
                    MSG_NOT_CONNECTED
End Sub

'
' PC-TCS returns "m +nnnnnnn" where 'm' is 1 or 0, depending
' on whether it is moving or not respectively, and the
' '+nnnnnnn" is the current position.
'
Private Sub GetStatus()
    Dim buf As String
    
    CheckConnected
    buf = C_CommandString("dome status")
    
End Sub



