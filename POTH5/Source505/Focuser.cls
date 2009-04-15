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
Attribute VB_Description = "ASCOM Focuser driver for POTH"
' -----------------------------------------------------------------------------'
'   ===========
'   FOCUSER.CLS
'   ===========
'
' Implementation of the ASCOM Focuser Class
'
' Written: Pierre de Ponthière
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 10-Jan-06 dpp     Initial edit
' 07-Sep-06 jab     Fixed object deletion code to be more robust
' 30-Sep-06 jab     Changed Mave and Halt to "Sub" instead of "Function"
' 03-Jun-07 jab     5.0.1 - Implement new standard IFocuser interface
' -----------------------------------------------------------------------------'

Option Explicit
Implements IFocuser

' ------------
' Class Events
' ------------

Private Sub Class_Initialize()
    
    If App.StartMode <> vbSModeStandalone Then
        DoStartupIf
        g_iFocuserConnections = g_iFocuserConnections + 1
    End If
    
    FocuserCreate (g_sFocuserID)
      
End Sub

Private Sub Class_Terminate()
    
    ' if fired up to be a server, then see if this is the last user
    If App.StartMode <> vbSModeStandalone Then
    
        ' decrement user count
        If g_iFocuserConnections > 0 Then
            g_iFocuserConnections = g_iFocuserConnections - 1
            
            ' go away if no more users
            If g_iFocuserConnections <= 0 Then
                If g_iConnections <= 0 And g_iDomeConnections <= 0 Then
                    Unload g_handBox
                Else
                    FocuserSave
                    FocuserDelete
                End If
            End If
        End If
    End If
    
End Sub

' ----------
' Properties
' ----------

Public Property Get Absolute() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Focuser Absolute: " & g_bFocuserAbsolute
    End If
    
    check_connected
    Absolute = g_bFocuserAbsolute
    
End Property

Public Property Get IsMoving() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar "Focuser IsMoving: "
    End If
    
    check_connected
    IsMoving = g_Focuser.IsMoving
    
    If Not g_show Is Nothing Then
        If g_show.chkPoll.Value = 1 Then _
            g_show.TrafficChar IIf(IsMoving, "True ", "False ")
    End If
    
End Property

Public Property Let Link(ByVal newVal As Boolean)
    
    Dim out As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Focuser Link: " & _
                g_bFocuserConnected & " -> " & newVal
    End If
    
    out = " (done)"
    
    If g_bFocuserConnected = newVal Then
        If Not g_show Is Nothing Then
            If g_show.chkOther.Value = 1 Then _
                g_show.TrafficEnd " no change"
        End If
        Exit Property
    End If
    
    ' only with a state change does anything happen
    If g_bFocuserConnected <> newVal Then
        If newVal Then
            g_setupDlg.ConnectFocuser False
            g_setupDlg.UpdateFocuserGlobals
            If g_bFocuserConnected Then
                out = " (connected)"
            Else
                out = " (error)"
            End If
        Else
            If App.StartMode = vbSModeStandalone Then
                out = " (ignored, not auto launch)"
            Else
                If g_iFocuserConnections <= 1 Then
                    g_setupDlg.ConnectFocuser False
                    g_setupDlg.UpdateFocuserGlobals
                Else
                    out = " (ignored, not last user)"
                End If
            End If
        End If
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd out
    End If
    
End Property

Public Property Get Link() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Focuser Link: " & Link
    End If
    
    Link = g_bFocuserConnected
    
End Property

Public Property Get MaxIncrement() As Long
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Focuser MaxIncrement: " & g_lFocuserMaxIncrement
    End If
    
    check_connected
    MaxIncrement = g_lFocuserMaxIncrement
    
End Property

Public Property Get MaxStep() As Long
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Focuser MaxMaxStep: " & MaxStep
    End If
    
    check_connected
    MaxStep = g_lFocuserMaxStep

End Property

Public Property Get Position() As Long
    
    If Not g_show Is Nothing Then
        If g_show.chkCoord.Value = 1 Then _
            g_show.TrafficLine "Focuser Position: " & g_lFocuserPosition
    End If
    
    check_connected
    Position = g_lFocuserPosition
    
End Property

Public Property Get StepSize() As Double
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Focuser StepSize: " & g_dFocuserStepSizeInMicrons
    End If
    
    check_connected
    StepSize = g_dFocuserStepSizeInMicrons
    
End Property

Public Property Get TempComp() As Boolean
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Focuser TempComp: " & g_bFocuserTempComp
    End If
    
    check_connected
    TempComp = g_bFocuserTempComp
    
End Property

Public Property Let TempComp(ByVal newVal As Boolean)
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Focuser TempComp: " & g_bFocuserTempComp & " -> " & newVal
    End If
    
    check_connected
    
    g_bFocuserTempComp = newVal
    g_Focuser.TempComp = newVal
    g_handBox.CheckFocuserEnable
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get TempCompAvailable() As Boolean
    
    If Not g_show Is Nothing Then
        If g_show.chkCap.Value = 1 Then _
            g_show.TrafficLine "Focuser TempCompAvailable: " & g_bFocuserTempCompAvailable
    End If
    
    check_connected
    TempCompAvailable = g_bFocuserTempCompAvailable
    
End Property

Public Property Get Temperature() As Double
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Focuser Temperature: " & g_dFocuserTemperature
    End If
    
    check_connected
    Temperature = g_dFocuserTemperature
    
End Property

'-----------------
' Methods
'-----------------

Public Sub Halt()
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficStart "Focuser Halt "
    End If
    
    check_connected
    g_Focuser.Halt
    
    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub Move(pos As Long)

    If Not g_show Is Nothing Then
        If g_show.chkSlew.Value = 1 Then _
            g_show.TrafficLine "Focuser Move: " & pos
    End If
    
    check_connected
    g_Focuser.Move (pos)
    
    If Not g_bFocuserAbsolute Then _
        g_lFocuserPosition = g_lFocuserPosition + pos
            
End Sub

Public Sub SetupDialog()
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Focuser SetupDialog"
    End If
    
    On Error Resume Next
    DoSetup
    On Error GoTo 0
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

' ================
' Private Routines
' ================

'---------------------------------------------------------------------
'
' check_connected() - Raise an error if the focuser is not connected
'
'---------------------------------------------------------------------

Private Sub check_connected()

    If (Not g_bFocuserConnected) Or (g_Focuser Is Nothing) Then _
        Err.Raise SCODE_FOCUSER_NOT_CONNECTED, ERR_SOURCE, _
            MSG_FOCUSER_NOT_CONNECTED
        
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
