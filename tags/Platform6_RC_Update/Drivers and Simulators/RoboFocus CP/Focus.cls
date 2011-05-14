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
Attribute VB_Description = "ASCOM Focuser driver for RoboFocus server"
'===========
' Focus.CLS
'===========
'
' Implementation of the ASCOM RoboFocus Focuser class
'
' Initial code by Jon Brewster in Apr 2003
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 05-Apr-03 jab     Initial edit
' 05-Jun-07 jab     Using new ASCOM standard interfaces!
' 25-Jul-08 jab     Return real temperature units if calibrated
'---------------------------------------------------------------------

Option Explicit
Implements IFocuser

' ------------
' Class Events
' ------------

Private Sub Class_Initialize()

    ' count references
    g_iFocuserConnections = g_iFocuserConnections + 1

    ' get access to vendor specific API
    If fc Is Nothing Then _
        Set fc = New FocusControl
            
End Sub

Private Sub Class_Terminate()

    If g_iFocuserConnections > 0 Then
        g_iFocuserConnections = g_iFocuserConnections - 1
        
        ' if launched as server then reference counting will determine behavior
        If Not g_bRunExecutable Then
            If g_iFocuserConnections <= 0 And g_iSwitchConnections <= 0 Then
                Set fc = Nothing    ' last reference, so shut down
                                    ' "end" in fc's terminate will do the deed
            End If
        End If
    End If
    
End Sub

' ----------
' Properties
' ----------

Public Property Get Absolute() As Boolean

    ' from an ASCOM API perspective, we're always an absolute device
    Absolute = True

End Property

Public Property Get IsMoving() As Boolean

    Dim Out As String

'    Out = IIf(cmdActive, "T", "t")
'    frmShow!txtSent.Text = frmShow!txtSent.Text & Out & vbCrLf
'    frmShow!txtSent.SelStart = Len(frmShow!txtSent.Text)
'    frmShow!txtReceived.Text = frmShow!txtReceived.Text & Out & vbCrLf
'    frmShow!txtReceived.SelStart = Len(frmShow!txtReceived.Text)

    check_connected
    IsMoving = cmdActive
    
End Property

Public Property Get Link() As Boolean

    Link = fc.getCommStatus()

End Property

Public Property Let Link(ByVal newVal As Boolean)
    Dim val As Boolean

    ' If no change is requested, then get out
    If newVal = fc.getCommStatus() Then _
        Exit Property
    
    If newVal Then
        val = fc.actOpenComm()
        If Not val Then _
            Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, _
                "Property Link. " & MSG_NOT_CONNECTED
    Else
        If Not g_bRunExecutable Then
            If g_iFocuserConnections <= 1 And g_iSwitchConnections <= 0 Then _
                val = fc.actCloseComm()   ' allow close if last client
        End If
    End If
    
End Property

Public Property Get MaxIncrement() As Long

    check_connected
    
    ' there is no MaxIncrement restrictions for RoboFocus, so use full MaxTravel
    MaxIncrement = fc.getMaxTravel()

End Property

Public Property Get MaxStep() As Long

    check_connected
    
    ' direct map to MaxTravel
    MaxStep = fc.getMaxTravel()

End Property

Public Property Get Position() As Long
    
    check_connected
    
    ' this does not poll the HW (good)
    Position = fc.getPosition()

End Property

Public Property Get StepSize() As Double
    
    If g_uPerStep <= 0 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property StepSize" & MSG_VAL_OUTOFRANGE

    StepSize = g_uPerStep

End Property

Public Property Get TempComp() As Boolean

    check_connected
    
    ' I'm glad the global flag exists!
    TempComp = autoCorrect

'    frmShow!txtSent.Text = frmShow!txtSent.Text & "get " & TempComp & vbCrLf
'    frmShow!txtSent.SelStart = Len(frmShow!txtSent.Text)
    
End Property

Public Property Let TempComp(ByVal newVal As Boolean)

    check_connected
    
'    frmShow!txtSent.Text = frmShow!txtSent.Text & _
'        autoCorrect & " -> " & newVal & vbCrLf
'    frmShow!txtSent.SelStart = Len(frmShow!txtSent.Text)
    
    ' if we're changing state, then toggle
    If newVal <> autoCorrect Then _
        frmTemp.cmdAuto_Click
    
End Property

Public Property Get TempCompAvailable() As Boolean

    TempCompAvailable = True

End Property

Public Property Get Temperature() As Double

    check_connected
    
    ' gblLastTemp may not have been set yet, so catch this condition
    If gblLastTemp = -1 Then _
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Property Temperature." & MSG_VAL_OUTOFRANGE
    
    Select Case tempDisplayUnits
  
        Case 0
            ' raw units - no conversion
            Temperature = CDbl(gblLastTemp)
    
        Case 1
            ' Farenheit
            Temperature = CDbl(Round(degreesF(CSng(gblLastTemp) * gblTempConvBeta - 273), 1))
    
        Case 2
            ' Celsius
            Temperature = CDbl(Round(CSng(gblLastTemp) * gblTempConvBeta - 273, 1))

    End Select
    
End Property

' -------
' Methods
' -------

Public Sub Halt()
    Dim rVal As Boolean

    check_connected
    
    rVal = fc.actStop()
    
    If Not rVal Then _
        Err.Raise SCODE_MOVEMENT_FAIL, ERR_SOURCE, _
            "Halt action." & MSG_MOVEMENT_FAIL
    
End Sub

Public Sub SetupDialog()

    ' We'll do it, but the usual behavior is to not return until complete.
    ' The implementation of RoboFocus code has this dialog as non-modal
    ' for the application ...
    
    frmConfig.Show
    Call SetWindowPos(frmConfig.hWnd, HWND_TOPMOST, 0, 0, 0, 0, FLAGS)
    
    While frmConfig.Visible
        DoEvents    ' wait till window goes away
    Wend
        
End Sub

Public Sub Move(ByVal val As Long)
    Dim rVal As Boolean
    
'    frmShow!txtSent.Text = frmShow!txtSent.Text & "move " & Val & vbCrLf
'    frmShow!txtSent.SelStart = Len(frmShow!txtSent.Text)
    
    check_connected

    ' temperature compensation check
    If autoCorrect Then _
        Err.Raise SCODE_MOVE_WHILE_COMP, ERR_SOURCE, _
            "Move action" & vbCrLf & MSG_MOVE_WHILE_COMP

    rVal = fc.actGoToPosition(val)

'    frmShow!txtSent.Text = frmShow!txtSent.Text & "moving " & cmdActive & vbCrLf
'    frmShow!txtSent.SelStart = Len(frmShow!txtSent.Text)

    If Not rVal Then _
        Err.Raise SCODE_MOVEMENT_FAIL, ERR_SOURCE, _
            "Move action." & MSG_MOVEMENT_FAIL

End Sub

'---------------------------------------------------------------------
'
' check_connected() - Raise an error if the focuser is not connected
'
'---------------------------------------------------------------------
Private Sub check_connected()

    If Not fc.getCommStatus() Then _
        Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
        
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

