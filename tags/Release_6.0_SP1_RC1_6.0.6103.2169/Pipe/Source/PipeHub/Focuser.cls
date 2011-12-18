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
Attribute VB_Description = "ASCOM Focuser driver for Pipe and Hub"
' -----------------------------------------------------------------------------'
'  ===========
'  FOCUSER.CLS
'  ===========
'
' Implementation of the ASCOM Focuser Class
'
' Written: Jon Brewster and Pierre de Ponthière
'
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 02-Sep-06 jab     Initial edit
' 03-Jun-07 jab     5.0.0 - Implement new standard IFocuser interface
' -----------------------------------------------------------------------------'

Option Explicit
Implements IFocuser

' ------------
' Class Events
' ------------

Private Sub Class_Initialize()
    
    If App.StartMode <> vbSModeStandalone Then _
        DoStartupIf
    
    g_iFocuserConnections = g_iFocuserConnections + 1
    
    check_object
    
End Sub

Private Sub Class_Terminate()

    ' decrement user count
    If g_iFocuserConnections > 0 Then
        g_iFocuserConnections = g_iFocuserConnections - 1
    
        ' go away if no more users
        If g_iFocuserConnections <= 0 Then
            ' If we've never forced a connect, or we're not connected now, then unattach
            If Not g_bFocuserManual Or Not g_bFocuserConnected Then
                FocuserSave
                FocuserDelete
                g_setupDlg.UpdateFocuser False, False
            End If
            
            ' if this was the last thing going, then the whole program is going down
            If g_iConnections <= 0 And g_iDomeConnections <= 0 And _
                    App.StartMode <> vbSModeStandalone Then
                Unload g_handBox
            End If
        End If
    End If
    
End Sub

' ----------
' Properties
' ----------

Public Property Get Absolute() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "Focuser Absolute: "
    End If
    
    On Error GoTo CatchError
    Absolute = g_Focuser.Absolute
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(Absolute)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get IsMoving() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficChar "Focuser IsMoving:"
    End If
    
    On Error GoTo CatchError
    IsMoving = g_Focuser.IsMoving
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkPoll.Value = 1 Then _
            g_handBox.TrafficChar IIf(IsMoving, "True", "False")
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkPoll.Value
    ' does not return

End Property

Public Property Get Link() As Boolean

    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Focuser Link: "
    End If
    
    On Error GoTo CatchError
    Link = g_Focuser.Link
    g_setupDlg.UpdateFocuser True, Link
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(Link)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Let Link(ByVal newVal As Boolean)
    Dim out As String
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Focuser Link: -> " & newVal
    End If
    
    On Error GoTo CatchError
    
    out = " (done)"     ' seed
    ' if trying to disconnect and (not last or we're manual), ignor
    If Not newVal And (g_iFocuserConnections > 1 Or g_bFocuserManual) Then
        If g_iFocuserConnections > 1 Then
            out = " (ignored, not last user)"
        ElseIf g_bFocuserManual Then
            out = " (ignored, manual connection)"
        End If
    Else
        g_Focuser.Link = newVal
        g_setupDlg.UpdateFocuser True, newVal
    End If
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd out
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get MaxIncrement() As Long
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "Focuser MaxIncrement: "
    End If
    
    On Error GoTo CatchError
    MaxIncrement = g_Focuser.MaxIncrement
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(MaxIncrement)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get MaxStep() As Long
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "Focuser MaxStep: "
    End If
    
    On Error GoTo CatchError
    MaxStep = g_Focuser.MaxStep
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(MaxStep)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get Position() As Long
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficStart "Focuser Position: "
    End If
    
    On Error GoTo CatchError
    Position = g_Focuser.Position
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCoord.Value = 1 Then _
            g_handBox.TrafficEnd CStr(Position)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCoord.Value
    ' does not return

End Property

Public Property Get StepSize() As Double
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Focuser StepSize: "
    End If
    
    On Error GoTo CatchError
    StepSize = g_Focuser.StepSize
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(StepSize)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get TempComp() As Boolean
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Focuser TempComp: "
    End If
    
    On Error GoTo CatchError
    TempComp = g_Focuser.TempComp
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(TempComp)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Let TempComp(ByVal newVal As Boolean)
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Focuser TempComp: -> " & newVal
    End If
    
    On Error GoTo CatchError
    g_Focuser.TempComp = newVal
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

Public Property Get TempCompAvailable() As Boolean
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficStart "Focuser TempCompAvailable: "
    End If
    
    On Error GoTo CatchError
    TempCompAvailable = g_Focuser.TempCompAvailable
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkCap.Value = 1 Then _
            g_handBox.TrafficEnd CStr(TempCompAvailable)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkCap.Value
    ' does not return

End Property

Public Property Get Temperature() As Double
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Focuser Temperature: "
    End If
    
    On Error GoTo CatchError
    Temperature = g_Focuser.Temperature
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd CStr(Temperature)
    End If
    
    Exit Property
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkOther.Value
    ' does not return

End Property

'-----------------
' Methods
'-----------------

Public Sub Halt()

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "Focuser Halt"
    End If
    
    On Error GoTo CatchError
    g_Focuser.Halt
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If
    
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub Move(pos As Long)

    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficStart "Focuser Move: " & CStr(pos)
    End If
    
    On Error GoTo CatchError
    g_Focuser.Move pos
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkSlew.Value = 1 Then _
            g_handBox.TrafficEnd " (initiated)"
    End If
            
    Exit Sub
    
CatchError:
    report_error Err.Number, Err.Source, Err.Description, g_handBox.chkSlew.Value
    ' does not return

End Sub

Public Sub SetupDialog()
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficStart "Focuser SetupDialog"
    End If
    
    On Error Resume Next
    DoSetup
    On Error GoTo 0
    
    If Not g_handBox Is Nothing Then
        If g_handBox.chkOther.Value = 1 Then _
            g_handBox.TrafficEnd " (done)"
    End If

End Sub

' ================
' Private Routines
' ================

'---------------------------------------------------------------------
'
' check_object() - Raise an error if the focuser is not defined/loaded
'
'---------------------------------------------------------------------

Private Sub check_object()

    If (g_Focuser Is Nothing) Then
        FocuserCreate (g_sFocuserID)    ' it will throw an error for failure
        g_setupDlg.UpdateFocuser False, g_bFocuserConnected
    End If
        
End Sub

'---------------------------------------------------------------------
'
' report_error() - Log errors, then raise them
'
'---------------------------------------------------------------------

Private Sub report_error(tNum As Long, tSrc As String, tMsg As String, logIt As Integer)
    
    ' log errors if the user wants them
    If logIt = 1 Then _
        g_handBox.TrafficEnd " (Error) - " & _
            Hex(tNum) & " - " & tSrc & " - " & tMsg
    
    Err.Raise tNum, tSrc, tMsg
    
    ' We're gone now, we should never get here...
    g_handBox.TrafficLine "Internal Pipe error: shouldn't be at end of error report."
        
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
