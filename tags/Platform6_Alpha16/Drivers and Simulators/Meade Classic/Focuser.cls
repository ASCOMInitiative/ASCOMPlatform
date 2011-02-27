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
Attribute VB_Description = "ASCOM Focuser Driver for Meade LX200 and Autostar"
'---------------------------------------------------------------------
'   ===========
'   FOCUSER.CLS
'   ===========
'
' Implementation of ASCOM Meade focuser driver.
'
' Written:  February 2nd, 2002   Chris Creery <ccreery@cyanogen.com>
'           April 20th, 2002     Larry Weber  <larryweber@idsi.net>
'
' When        Who  What
' ----------  ---  --------------------------------------------------
' 2002/02/02  CC   1.5.0 Initial edit
' 2002/04/20  LFW  1.8.1 Added Focuser Dynamic Braking
' 2002/04/21  LFW  1.8.1 Added Focuser Backlash Compensation
' 07-May-2002 rbd  1.8.3 Variable and function name changes, code
'                        cleanup for public release, remove lfw marks,
'                        Remove leading # from serial commands, goofs
'                        up the LX200GPS. Do not know if this will work
'                        with the GPS, as its timing is way slow.
'                        Removed lfw edits, Larry's features are a
'                        success!
' 18-Jun-2002 rbd  1.8.4 Change to out-of-proc server to support
'                        ownership of Telescope and Focuser by
'                        different processes. Add logic to prevent
'                        creating multiple instances of interfaces.
' 18-Jun-2002 rbd  1.8.4 YEAH! I found the "fail to reconnect" problem
'                        with the LX200 GPS. Must send a leading #
'                        when (re)connecting to the GPS. Cannot(!!)
'                        always send it as noted in 07-May comment.
'                        Clean up redundant logic in Link property.
'                        Fix error recovery where serial would not
'                        be closed if the scope and focuser detect
'                        doesn't work. Err.Raise in Link(), ERR_SOURCE
'                        missing. Move common LX200 "GS" test code
'                        to Common.OpenConnection(). Move the Focuser
'                        specific registry settings under the Focuser
'                        area in the ASCOM registry tree. Setup dialog
'                        stores settings in proper places now.
' 23-Jul-02 rbd    1.8.7 - Improve "Invalid Object Use" message.
' 28-Jul-02 LFW    2.0.1 Fixed MaxIncrement error message in Sub Move.
' 25-oct-o3 jab     setup dialog now works while connected
' 21-Sep-06 jab     Connection check for move
' 29-Sep-06 jab     Temperature needed to be a double (even though not supported)
' 30-Sep-06 jab     GPS split out to new driver
' 09-Jun-07 jab     Added IFocuser
'---------------------------------------------------------------------

Option Explicit
Implements IFocuser

' Constants (for internal use)
Private Const FOCUS_CMD_IN = "F-"
Private Const FOCUS_CMD_OUT = "F+"
Private Const FOCUS_CMD_STOP = "FQ"
Private Const FOCUS_CMD_FAST = "FF"
Private Const FOCUS_CMD_SLOW = "FS"
Private Const FOCUS_CMD_GET_SIDEREAL_TIME = "GS"

' Member variables
Private m_Profile As DriverHelper.Profile
Private WithEvents m_PulseTimer As DriverHelper.Timer
Attribute m_PulseTimer.VB_VarHelpID = -1
Private WithEvents m_BacklashIntermediateDelay As DriverHelper.Timer
Attribute m_BacklashIntermediateDelay.VB_VarHelpID = -1
Private m_MoveInCmd As String
Private m_MoveOutCmd As String
Private m_MaxIncrement As Long
Private m_LastMoveCmd As String
Private m_BacklashSteps As String
Private m_UseDynamicBraking As Boolean
Private m_DontClearBuffers As Boolean
Private m_InitialMoveDirectionIsOut As Boolean
Private m_FinalMoveDirectionIsOut As Boolean
Private m_EnableBacklashIntermediateDelay As Boolean

Private Sub Class_Initialize()
    
    If g_bFocuserCreated Then _
        Err.Raise 425, App.Title, "Invalid object use: Only one program can use this Focuser."  ' Prevent multiple use
    g_bFocuserCreated = True
    
    Set m_PulseTimer = New DriverHelper.Timer
    Set m_BacklashIntermediateDelay = New DriverHelper.Timer
    
    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Focuser"                ' This is a Focuser driver

End Sub

Private Sub Class_Terminate()

    On Error Resume Next
    Me.Link = False                ' Force disconnect
    g_bFocuserCreated = False
    
    Set m_PulseTimer = Nothing
    Set m_BacklashIntermediateDelay = Nothing
    Set m_Profile = Nothing
    
End Sub

Public Property Get Absolute() As Boolean

    Absolute = False

End Property

Private Function CalculatePulseLength(Increment As Long) As Long
    ' Returns the duration of the pulse (in milliseconds) for a given position
    ' increment.  'Increment' can be positive or negative.

    ' TODO: implement this properly
    CalculatePulseLength = Abs(Increment)

End Function

Public Property Get IsMoving() As Boolean

    IsMoving = m_PulseTimer.Enabled Or m_BacklashIntermediateDelay.Enabled
    
End Property

Public Property Get Link() As Boolean

    Link = IsConnected(devFocuser)
    
End Property

Public Property Let Link(ByVal newVal As Boolean)
    Dim Buffer As String

    ' Unless the link "state" is changing, just return.
    If newVal = g_bFocuserConnected Then Exit Property
    
    If Not newVal Then                          ' Closing connection
        CloseConnection devFocuser
        Exit Property                               ' DONE
    End If

    '
    ' Rest of code is for Me.Link = True
    '
    ' Check to see if the user has flipped in the in/out directions for
    ' focuser.  If so, flip the commands sent to the 'scope. This also
    ' changes the meaning of m_FinalMoveDirectionIsOut to really meaning
    ' in.
    '
    Buffer = m_Profile.GetValue(FOCUSER_ID, "Flip Dir")
    If Buffer = "" Then Buffer = "0"
    Dim Flip As Boolean
    Flip = CBool(Buffer)
    If Flip Then
        m_MoveInCmd = FOCUS_CMD_OUT
        m_MoveOutCmd = FOCUS_CMD_IN
    Else
        m_MoveInCmd = FOCUS_CMD_IN
        m_MoveOutCmd = FOCUS_CMD_OUT
    End If
    
    ' Load Dynamic Braking switch
    Buffer = m_Profile.GetValue(FOCUSER_ID, "Dynamic Braking")
    If Buffer = "" Then Buffer = "1"
    If Buffer = "1" Then
        m_UseDynamicBraking = True
    Else
        m_UseDynamicBraking = False
    End If
                  
    ' Load Focuser Backlash Steps
    Buffer = m_Profile.GetValue(FOCUSER_ID, "Backlash Steps")
    If (Buffer = "") Then Buffer = 3000
    m_BacklashSteps = CLng(Buffer)
    
    ' Load Focuser Final Direction
    Buffer = m_Profile.GetValue(FOCUSER_ID, "Final Direction")
    If (Buffer = "") Then Buffer = "Out"
    If Buffer = "In" Then
        m_FinalMoveDirectionIsOut = False
    Else
        m_FinalMoveDirectionIsOut = True
    End If
        
    ' Load the maximum increment (ie. actuation duration in ms) from
    ' the registry.
    Buffer = m_Profile.GetValue(FOCUSER_ID, "Max Increment")
    If (Buffer = "") Then Buffer = 7000
    m_MaxIncrement = CLng(Buffer)
    
    OpenConnection devFocuser                       ' May raise error (bad port#?)
    
    '
    ' Set the focuser mode to "fast" movement because it is more reliable
    ' on all versions of LX-200.
    '
    On Error GoTo FOCUSER_FAILURE                   ' If it barfs on FOCUS_CMD_FAST, no focuser?
    SendCommand FOCUS_CMD_FAST
    
    Exit Property                                   ' DONE
    
FOCUSER_FAILURE:
    ' Clean up.
    g_bFocuserConnected = False
    CloseConnection devFocuser                      ' Release port for focuser
    Err.Raise SCODE_FOCUS_CONNECT_FAILED, _
                ERR_SOURCE, _
                MSG_FOCUS_CONNECT_FAILED
    ' fall out of property
    
End Property

Public Property Get MaxIncrement() As Long

    MaxIncrement = MaxStep

End Property

Public Property Get MaxStep() As Long
    
    MaxStep = m_MaxIncrement

End Property

Public Property Get Position() As Long
    ' Return the focuser's absolute position. Since the Meade electric focuser is relative,
    ' this property is meaningless; throw an exception to indicate the problem.
    Err.Raise SCODE_NOT_IMPLEMENTED, _
                ERR_SOURCE, _
                "Property Position" & MSG_NOT_IMPLEMENTED
End Property

Private Function SendCommand(CmdStr As String, Optional ResponseExpected As Boolean = False) As String
    SendCommand = ""
   
    If m_DontClearBuffers = False Then g_SharedSerial.ClearBuffers
    m_DontClearBuffers = False

    g_SharedSerial.Transmit ":" & CmdStr & "#"
    
    ' If this command expects a response (some are "blind") then extract it from
    ' the receive buffer.
    If ResponseExpected Then
        Dim Buffer As String
        Buffer = g_SharedSerial.ReceiveTerminated("#")
        If Buffer <> "" Then
            SendCommand = Left(Buffer, Len(Buffer) - 1)   ' Strip '#'
        End If
    End If
    
End Function

Public Property Get StepSize() As Single
' ASCOM Spec:
'   Returns the step size in Microns for the focuser. Throws an exception if the
'   focuser does not intrinsically know what the step size is.

    Err.Raise SCODE_NOT_IMPLEMENTED, _
                ERR_SOURCE, _
                "Property StepSize " & MSG_NOT_IMPLEMENTED
End Property

Public Property Get TempComp() As Boolean
' ASCOM Spec:
'   If TempCompAvailable is True, then setting TempComp to True puts the focuser
'   into temperature tracking mode. While in temperature tracking mode, Move commands
'   will be rejected by the focuser. Set to False to turn off temperature tracking.
'   An exception will be raised if TempCompAvailable is False and an attempt is made
'   to set TempComp to true.

    TempComp = False
    
End Property

Public Property Let TempComp(ByVal newVal As Boolean)
' ASCOM Spec:
'   If TempCompAvailable is True, then setting TempComp to True puts the focuser
'   into temperature tracking mode. While in temperature tracking mode, Move commands
'   will be rejected by the focuser. Set to False to turn off temperature tracking.
'   An exception will be raised if TempCompAvailable is False and an attempt is made
'   to set TempComp to true.
    
    Err.Raise SCODE_NOT_IMPLEMENTED, _
                ERR_SOURCE, _
                "Property TempComp" & MSG_NOT_IMPLEMENTED
              
End Property

Public Property Get TempCompAvailable() As Boolean
' ASCOM Spec:
'   Returns True if the focuser has a built-in temperature compensation mode that
'   can be activated by TempComp. Returns False if such a mode is not available.
       
    TempCompAvailable = False
    
End Property

Public Property Get Temperature() As Double
' ASCOM Spec:
'   Returns the current ambient temperature as measured by the focuser.
'   Throws an exception if ambient temperature is not available.
'   Commonly available on focusers with a built-in temperature compensation mode.
    
    Err.Raise SCODE_NOT_IMPLEMENTED, _
              ERR_SOURCE, _
              "Property Temperature" & MSG_NOT_IMPLEMENTED
              
End Property

Public Sub Halt()
    
    CheckConnected

    ' Stop any movement
    m_PulseTimer.Enabled = False
    m_BacklashIntermediateDelay.Enabled = False
    m_EnableBacklashIntermediateDelay = False
    SendCommand FOCUS_CMD_STOP
    
End Sub

Public Sub SetupDialog()

    ' Show the focuser setup dialog.
    
'    ' Can't perform setup when connected.
'    If IsConnected(devFocuser) Then
'        Err.Raise SCODE_SETUP_CONNECTED, _
'                    ERR_SOURCE, _
'                    MSG_SETUP_CONNECTED
'    End If
    
    frmSetup.Show 1
    Unload frmSetup
    
End Sub


Public Sub Move(ByVal NewPosition As Long)
    ' This is a relative positioning focuser. The Move command tells the focuser to move in a
    ' relative direction, and Position is an integer between -MaxIncrement and +MaxIncrement.
    
    ' Sanity check
    CheckConnected
    
    ' Bound check.
    If (NewPosition < (-MaxIncrement)) Or (NewPosition > MaxIncrement) Then
        Err.Raise SCODE_MOVE_POSITION_GREATER_THAN_MAXINCREMENT, ERR_SOURCE, _
        MSG_MOVE_POSITION_GREATER_THAN_MAXINCREMENT
    
    End If
    If (NewPosition = 0) Then
        ' A relative position of '0' means don't move; so don't.
        Exit Sub
    End If
    
    Dim CmdStr As String
    If NewPosition > 0 Then
'     Desired move direction is OUT
        If m_FinalMoveDirectionIsOut = True Then
            CmdStr = m_MoveOutCmd
        Else    'May need backlash compensation
            If m_BacklashSteps <> 0 Then
                If m_MoveOutCmd = FOCUS_CMD_OUT Then
                    m_InitialMoveDirectionIsOut = True
                Else
                    m_InitialMoveDirectionIsOut = False
                End If
                MoveWithBacklashCompensation NewPosition
                Exit Sub
            Else
                CmdStr = m_MoveOutCmd
            End If
        End If
    Else
'     Desired move direction is IN
        If m_FinalMoveDirectionIsOut = False Then
            CmdStr = m_MoveInCmd
        Else    'May need backlash compensation
            If m_BacklashSteps <> 0 Then
                If m_MoveInCmd = FOCUS_CMD_IN Then
                    m_InitialMoveDirectionIsOut = False
                Else
                    m_InitialMoveDirectionIsOut = True
                End If
                MoveWithBacklashCompensation NewPosition
                Exit Sub
            Else
                CmdStr = m_MoveInCmd
            End If
        End If

    End If
            
    ' Determine the pulse length, and start the focuser moving.
    m_PulseTimer.Interval = CalculatePulseLength(NewPosition)
    SendCommand CmdStr
    If m_UseDynamicBraking = True Then m_LastMoveCmd = CmdStr
    m_PulseTimer.Enabled = True
    
End Sub

Private Sub m_BacklashIntermediateDelay_Tick()  'A new sub
' This is executed at the end of the 1 second delay between the two moves of the Backlash Compensation
' Now that the Intermediate Delay is over, we can make the second Backlash move
    m_PulseTimer.Interval = CalculatePulseLength(val(m_BacklashSteps))
    If m_InitialMoveDirectionIsOut = True Then
        SendCommand FOCUS_CMD_IN
        If m_UseDynamicBraking = True Then m_LastMoveCmd = FOCUS_CMD_IN
    Else
        SendCommand FOCUS_CMD_OUT
        If m_UseDynamicBraking = True Then m_LastMoveCmd = FOCUS_CMD_OUT
    End If
    m_PulseTimer.Enabled = True  'Start second move timer
    m_BacklashIntermediateDelay.Enabled = False ' stop the Intermediate Delay timer

End Sub

Private Sub m_PulseTimer_Tick()
'   This is executed at the end of the focuser move

    m_DontClearBuffers = True         'Clearing buffers causes 100 ms time error
    
' Do dynamic Braking of focuser by reversing move direction for a short period
'  only if m_LastMoveCmd is either "FOCUS_CMD_IN" or "FOCUS_CMD_OUT".

' The the LX-200 circuitry does not allow the focuser to move in the reverse direction
' as long as the stop after the reversal comes before about 200 ms. The reversal
' activation period is not critical.  It can range between 1 and 200 ms with no change
' in focuser performance.
    
    Select Case m_LastMoveCmd
        Case FOCUS_CMD_IN
        '  Use multiple commands in send to get reliable timing
            SendCommand FOCUS_CMD_OUT & "#:" & FOCUS_CMD_STOP
        Case FOCUS_CMD_OUT
        '  Use multiple commands in send to get reliable timing
            SendCommand FOCUS_CMD_IN & "#:" & FOCUS_CMD_STOP
        Case Else
            SendCommand FOCUS_CMD_STOP
    End Select
    m_LastMoveCmd = ""

'  The next two lines are used to possibly trigger the Backlash Compensation sequence
    If m_EnableBacklashIntermediateDelay = True Then m_BacklashIntermediateDelay.Enabled = True
    m_EnableBacklashIntermediateDelay = False  'reset flag
        
    m_PulseTimer.Enabled = False ' stop the timer
    
End Sub

Private Sub MoveWithBacklashCompensation(NewPosition As Long)      'A new sub

' This takes absolute value of NewPosition and moves in direction specified by m_InitialMoveDirectionIsOut
' and uses Backlash Compensation by:
'   1. Moving in the specified direction by NewPosition plus m_BacklashSteps.
'   2. Pausing for one second to allow primary mirror to settle.
'   3. Moving in opposite direction by m_BacklashSteps.
    
'  Setup first move
    
    m_PulseTimer.Interval = CalculatePulseLength(Abs(NewPosition) + val(m_BacklashSteps))
    If m_UseDynamicBraking = True Then
        If m_InitialMoveDirectionIsOut = True Then
            m_LastMoveCmd = FOCUS_CMD_OUT
        Else
            m_LastMoveCmd = FOCUS_CMD_IN
        End If
    End If
        
    m_BacklashIntermediateDelay.Interval = 1000     'Set up pause for 1 second to allow mirror to settle
    If m_BacklashSteps <> 0 Then  'Need to do Backlash Compensation
        m_EnableBacklashIntermediateDelay = True    'Flag that will start intermediate delay at end of Pulse timer
    Else  'No need to do Backlash Compensation
        m_EnableBacklashIntermediateDelay = False
    End If

'  Start first move

    If m_InitialMoveDirectionIsOut = True Then
        SendCommand FOCUS_CMD_OUT
    Else
        SendCommand FOCUS_CMD_IN
    End If
    
    m_PulseTimer.Enabled = True  'Start first move timer

' The Intermediate Delay will be started after the Pulse Timer completes
' The second move will be executed after the Intermediate Delay completes
    
End Sub

'
' CheckConnected() - Raise an error if the focuser is not connected
'
Private Sub CheckConnected()

    If Not IsConnected(devFocuser) Then _
        Err.Raise SCODE_FOCUSER_NOT_CONNECTED, ERR_SOURCE, MSG_FOCUSER_NOT_CONNECTED
        
End Sub

' ==========================
' Implementation of IFocuser
' ==========================

Private Property Get IFocuser_Absolute() As Boolean
    IFocuser_Absolute = Absolute
End Property

Private Sub IFocuser_Halt()
    Call Halt
End Sub

Private Property Get IFocuser_IsMoving() As Boolean
    IFocuser_IsMoving = IsMoving
End Property

Private Property Let IFocuser_Link(ByVal RHS As Boolean)
    Link = RHS
End Property

Private Property Get IFocuser_Link() As Boolean
    IFocuser_Link = Link
End Property

Private Property Get IFocuser_MaxIncrement() As Long
    IFocuser_MaxIncrement = MaxIncrement
End Property

Private Property Get IFocuser_MaxStep() As Long
    IFocuser_MaxStep = MaxStep
End Property

Private Sub IFocuser_Move(ByVal val As Long)
    Call Move(val)
End Sub

Private Property Get IFocuser_Position() As Long
    IFocuser_Position = Position
End Property

Private Sub IFocuser_SetupDialog()
    Call SetupDialog
End Sub

Private Property Get IFocuser_StepSize() As Double
    IFocuser_StepSize = StepSize
End Property

Private Property Let IFocuser_TempComp(ByVal RHS As Boolean)
    TempComp = RHS
End Property

Private Property Get IFocuser_TempComp() As Boolean
    IFocuser_TempComp = TempComp
End Property

Private Property Get IFocuser_TempCompAvailable() As Boolean
    IFocuser_TempCompAvailable = TempCompAvailable
End Property

Private Property Get IFocuser_Temperature() As Double
    IFocuser_Temperature = Temperature
End Property

