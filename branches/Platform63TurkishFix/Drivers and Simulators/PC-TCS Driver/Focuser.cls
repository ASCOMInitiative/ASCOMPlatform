VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Focuser"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ASCOM Focuser driver for COMSOFT PC-TCS"
'---------------------------------------------------------------------
'   ===========
'   FOCUSER.CLS
'   ===========
'
' Implementation of the ASCOM focuser driver Focuser class
' for COMSOFT PC-TCS (which uses Merlin ACL).
'
' Written:  29-Mar-06   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 29-Mar-06 rbd     4.0.3 - Initial edit.
' 02-Apr-06 rbd     4.0.4 - Implement MaxStep, add 25000 bias to
'                   position so is always positive.
' 09-Apr-06 rbd     4.0.5 - Implement MaxIncrement = MaxStep
' 30-Aug-06 rbd     4.0.6 - Move ASCOM registration of all interfaces
'                   to app startup.
'---------------------------------------------------------------------

Option Explicit

Private Const FOCUS_BIAS As Long = 25000                    ' Shift PC-TCS focus pos' so won't go neg
Private Const MAX_FOCUSPOS As Long = 50000                  ' Should be 2 * FOCUS_BIAS
Private Const FOCUS_STEP = 100

Private m_Profile As DriverHelper.Profile
Private m_lPosition As Long                                 ' Call GetStatus() to set these 2
Private m_bMoving As Boolean

Private Sub Class_Initialize()

    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Focuser"                        ' We're a Focuser driver
    
End Sub

Private Sub Class_Terminate()

    On Error Resume Next
    CloseConnection devFocuser

End Sub

Public Property Get Link() As Boolean

    Link = IsConnected(devFocuser)
    
End Property

Public Property Let Link(ByVal newVal As Boolean)

    If newVal Then
        OpenConnection devFocuser
    Else
        CloseConnection devFocuser
    End If
    
End Property

Public Property Get Absolute() As Boolean

    Absolute = True
    
End Property

Public Property Get StepSize() As Single

    StepSize = 50                                       ' ???
    
End Property

Public Property Get IsMoving() As Boolean

    GetStatus
    IsMoving = m_bMoving
    
End Property

Public Property Get Position() As Long
    
    GetStatus
    Position = m_lPosition + FOCUS_BIAS
    
End Property

Public Property Get TempComp() As Boolean

    TempComp = False
    
End Property

Public Property Let TempComp(ByVal newVal As Boolean)
    
    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property Let TempComp" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get TempCompAvailable() As Boolean

    TempCompAvailable = False
    
End Property

Public Property Get Temperature() As Single

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property Get Temperature" & MSG_NOT_IMPLEMENTED

End Property

Public Property Get MaxIncrement() As Long

    MaxIncrement = MAX_FOCUSPOS

End Property

Public Property Get MaxStep() As Long

    MaxStep = MAX_FOCUSPOS

End Property

Public Sub Move(ByVal newVal As Long)

    CheckConnected
    newVal = newVal - FOCUS_BIAS
    C_CommandString "focus " & newVal
    g_Util.WaitForMilliseconds 1000                     ' wait for moving status
  
End Sub

Public Sub SetupDialog()

    If SerialConnected Then _
        Err.Raise SCODE_SETUP_CONNECTED, _
                    ERR_SOURCE, _
                    MSG_SETUP_CONNECTED
    frmSetup.Show 1
    Unload frmSetup
    
End Sub

Public Sub Halt()

    Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method Halt" & MSG_NOT_IMPLEMENTED

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
    buf = C_CommandString("focus status")
    m_bMoving = Left$(buf, 1) = "m"
    m_lPosition = CLng(Trim$(Mid$(buf, 2)))
    
End Sub


