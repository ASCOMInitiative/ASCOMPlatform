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
Attribute VB_Description = "ASCOM Focuser driver for Focuser Simulator"
'---------------------------------------------------------------------
' Copyright © 2000-2002 SPACE.com Inc., New York, NY
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
'   ===========
'   Focuser.CLS
'   ===========
'
' Implementation of the ASCOM focus simulator Focuser class
'
' From Scope Simulator written 28-Jun-00   Robert B. Denny <rdenny@dc3.com>
' Structure retained, otherwise rewriten into Focus Simulator
' by Jon Brewster in Feb 2003
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 24-Feb-03 jab     Initial edit - Starting from Telescope Simulator
' 02-Mar-03 jab     added CanTemp dependencies
' 02-Mar-03 jab     added more accurate temperature compensation
' 04-Mar-03 jab     setting TempComp to false is not an error if
'                   CanTempComp is false as per ASCOM spec
' 06-Mar-03 jab     changed move to asynchronous behaviour
' 15-Mar-03 jab     kill server if launched via client and last client bails,
'                   also only allow disconnect if last clients commands it,
'                   no disconnects allowed if launched manually
' 23-Nov-04 rbd     4.0.1 - Version change for Platform 4.0
' 12-Apr-07 rbd     5.0.1 - New startup refactoring
' 01-May-07 rbd     5.0.2 - Using new ASCOM standard interfaces!
' 03-Jun-07 jab     5.0.3 converted back to public as was, and IFocuser
'                   calls the old public code (fixed many issues)
'---------------------------------------------------------------------

Option Explicit
Implements IFocuser

' ------------
' Class Events
' ------------

Private Sub Class_Initialize()

    If App.StartMode <> vbSModeStandalone Then                    ' If loaded via ActiveX
        DoStartupIf
        g_iConnections = g_iConnections + 1
    End If
    
End Sub

Private Sub Class_Terminate()
    
    If App.StartMode <> vbSModeStandalone Then
        If g_iConnections > 1 Then
            g_iConnections = g_iConnections - 1
        Else
            Unload g_handBox
        End If
    End If
    
End Sub

' ----------
' Properties
' ----------

Public Property Get Absolute() As Boolean

    Absolute = g_bAbsolute
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Absolute: " & Absolute
    End If

End Property

Public Property Get IsMoving() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkMoving.Value = 1 Then _
            g_show.TrafficChar _
                IIf(g_bMoving, "IsMoving: True", "IsMoving: False")
    End If
    
    check_connected
    IsMoving = g_bMoving
    
End Property

Public Property Get Link() As Boolean

    Link = g_bLinked
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Link: " & Link
    End If
      
End Property

Public Property Let Link(ByVal newVal As Boolean)

    Dim out As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Link: " & g_bLinked & " -> " & newVal
    End If
    
    If newVal Then
        g_bLinked = True
        out = " (done)"
    Else
        If App.StartMode = vbSModeStandalone Then      ' ignore clear if run as executable
            out = " (ignored, not auto launch)"
        Else
            If g_iConnections < 2 Then
                g_bLinked = False         ' allow clear if last client
                out = " (done)"
            Else
                out = " (ignored, not last user)"
            End If
        End If
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd out
    End If
    
End Property

Public Property Get MaxIncrement() As Long

    MaxIncrement = g_lMaxInc
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "MaxIncrement: " & MaxIncrement
    End If

End Property

Public Property Get MaxStep() As Long

    MaxStep = g_lMaxStep

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "MaxStep: " & MaxStep
    End If

End Property

Public Property Get Position() As Long

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Position: "
    End If
        
    If Not g_bAbsolute Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property Position" & MSG_NOT_IMPLEMENTED
            
    check_connected
    Position = g_lPosition
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(Position)
    End If

End Property

Public Property Get StepSize() As Double
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "StepSize: "
    End If
    
    If Not g_bCanStepSize Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property StepSize" & MSG_NOT_IMPLEMENTED
    
    StepSize = g_dStepSize
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd CStr(StepSize)
    End If

End Property

Public Property Get TempComp() As Boolean

    If Not g_show Is Nothing Then
        If g_show.chkTemp.Value = 1 Then _
            g_show.TrafficStart "TempComp: "
    End If
    
    check_connected
    
    ' do not throw error if not implemented, just return false
    TempComp = g_bTempComp And g_bCanTemp And g_bCanTempComp
    
    If Not g_show Is Nothing Then
        If g_show.chkTemp.Value = 1 Then _
            g_show.TrafficEnd CStr(TempComp)
    End If

End Property

Public Property Let TempComp(ByVal newVal As Boolean)

    If Not g_show Is Nothing Then
        If g_show.chkTemp.Value = 1 Then _
            g_show.TrafficStart "TempComp: " & g_bTempComp & " -> " & newVal
    End If
    
    ' only bomb if trying to set TempComp
    If newVal Then
        If (Not g_bCanTempComp) Or (Not g_bCanTemp) Then _
            Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Property TempComp" & MSG_NOT_IMPLEMENTED
    End If
    
    check_connected
    
    If newVal Then
        ' remember current values for accurate compensation
        g_dTTemp = g_dTemp
        g_lTPos = g_lPosition
    End If
    
    g_bTempComp = newVal
        
    ' update the handbox
    If g_bTempComp Then
        g_handBox.TempComp = 1
    Else
        g_handBox.TempComp = 0
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkTemp.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Property

Public Property Get TempCompAvailable() As Boolean

    TempCompAvailable = g_bCanTemp And g_bCanTempComp
    
    If Not g_show Is Nothing Then
        If g_show.chkTemp.Value = 1 Then _
            g_show.TrafficLine "TempCompAvailable: " & TempCompAvailable
    End If
    
End Property

Public Property Get Temperature() As Double
   
    If Not g_show Is Nothing Then
        If g_show.chkTemp.Value = 1 Then _
            g_show.TrafficStart "Temperature: "
    End If
    
    If Not g_bCanTemp Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Property Temperature" & MSG_NOT_IMPLEMENTED
    
    check_connected
    Temperature = g_dTemp
    
    If Not g_show Is Nothing Then
        If g_show.chkTemp.Value = 1 Then _
            g_show.TrafficEnd CStr(Temperature)
    End If
    
End Property

' -------
' Methods
' -------

Public Sub Halt()
    
    If Not g_show Is Nothing Then
        If g_show.chkMove.Value = 1 Then _
            g_show.TrafficStart "Halt"
    End If
    
    If Not g_bCanHalt Then _
        Err.Raise SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Halt" & MSG_NOT_IMPLEMENTED
    
    check_connected
    g_bMoving = False
    
    If Not g_show Is Nothing Then
        If g_show.chkMove.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub SetupDialog()
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "SetupDialog"
    End If
    
    DoSetup
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd " (done)"
    End If
    
End Sub

Public Sub Move(ByVal val As Long)

    If Not g_show Is Nothing Then
        If g_show.chkMove.Value = 1 Then _
            g_show.TrafficStart "Move: " & val
    End If
      
    check_connected

    ' temperature compensation check
    If g_bCanTemp And g_bCanTempComp And g_bTempComp Then
        If Not g_show Is Nothing Then
            If g_show.chkMove.Value = 1 Then _
                g_show.TrafficEnd " (aborting, TempComp)"
        End If
        Err.Raise SCODE_MOVE_WHILE_COMP, ERR_SOURCE, _
            "Move" & MSG_MOVE_WHILE_COMP
    End If

    ' calculate the move
    If g_bAbsolute Then
        If Abs(val - g_lPosition) > g_lMaxInc Then
            If Not g_show Is Nothing Then
                If g_show.chkMove.Value = 1 Then _
                    g_show.TrafficEnd " (aborting, range)"
            End If
            Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
                "Move: " & MSG_VAL_OUTOFRANGE
        End If
        g_lDeltaMove = val - g_lPosition
    Else
        If Abs(val) > g_lMaxInc Then
            If Not g_show Is Nothing Then
                If g_show.chkMove.Value = 1 Then _
                    g_show.TrafficEnd " (aborting, range)"
            End If
            Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
                "Move: " & MSG_VAL_OUTOFRANGE
        End If
        g_lDeltaMove = val
    End If
    
    ' trigger the "motor"
    g_bMoving = True
    
'    If Not g_show Is Nothing Then
'        If g_show.chkMove.Value = 1 Then _
'            g_show.TrafficEnd " (started)"
'    End If
    
    If g_bSynchronous Then
        While g_bMoving
            DoEvents
        Wend
    End If
  
    ' move code in Startup.bas journals end of move
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

    If Not g_bLinked Then _
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
