VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "FilterWheel"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "ASCOM FilterWheel driver for Filter Wheel Simulator"
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
'   ===============
'   FilterWheel.CLS
'   ===============
'
' Implementation of the ASCOM Filter Wheel simulator FilterWheel class
'
' From Focus Simulator, from Scope Simulator written 28-Jun-00
' Robert B. Denny <rdenny@dc3.com>
' Structure retained, otherwise rewriten into Filter Wheel Simulator
' by Mark Crossley in Nov 2008
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 18-Nov-08 mpc     Initial edit - Starting from Focuser Simulator
'---------------------------------------------------------------------

Option Explicit

Implements IFilterWheel


' ------------
' Class Events
' ------------

Private Sub Class_Initialize()

    If Not g_bRunExecutable Then _
        g_iConnections = g_iConnections + 1
    
End Sub

Private Sub Class_Terminate()
    
    If Not g_bRunExecutable Then
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

Public Property Get FocusOffsets() As Long()

    Dim i As Integer
    Dim temp() As Long
    ReDim temp(g_iSlots - 1)
    
    check_connected
    
    If Not g_show Is Nothing Then
        If g_show.chkName.Value = 1 Then _
            g_show.TrafficLine "FocusOffsets: "
    End If
    
    For i = 0 To g_iSlots - 1
        If g_bImplementsOffsets Then
            temp(i) = g_alFocusOffsets(i)
        Else
            temp(i) = 0                         ' Spec says we must return 0 if focus offsets not supported
        End If
        If Not g_show Is Nothing Then
            If g_show.chkName.Value = 1 Then _
                g_show.TrafficLine "  Offset(" & i & ") = " & temp(i)
        End If
    Next
    
    FocusOffsets = temp
        
End Property

Public Property Get Names() As String()
        
    Dim i As Integer
    Dim temp() As String
    ReDim temp(g_iSlots - 1)
    
    check_connected
    
    If Not g_show Is Nothing Then
        If g_show.chkName.Value = 1 Then _
            g_show.TrafficLine "Names: "
        End If

    For i = 0 To g_iSlots - 1
        If g_bImplementsNames Then
            temp(i) = g_asFilterNames(i)
        Else
            temp(i) = "Filter " & i + 1         ' Spec. says we return "Filter 1" etc if names not supported
        End If
        If Not g_show Is Nothing Then
            If g_show.chkName.Value = 1 Then _
                g_show.TrafficLine "  Filter(" & i & ") = " & temp(i)
        End If
    Next

    Names = temp
    
End Property

Public Property Get Connected() As Boolean

    Connected = g_bLinked
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficLine "Connected: " & g_bLinked
    End If
      
End Property

Public Property Let Connected(ByVal newVal As Boolean)

    Dim out As String

    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficStart "Connected: " & g_bLinked & " -> " & newVal
    End If
    
    If newVal Then
        g_bLinked = True
        out = " (done)"
    Else
        If Not g_bRunExecutable Then      ' ignore clear if run as executable
            If g_iConnections < 2 Then
                g_bLinked = False         ' allow clear if last client
                out = " (done)"
            Else
                out = " (ignored, not last user)"
            End If
        Else
            out = " (ignored, not auto launch)"
        End If
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkOther.Value = 1 Then _
            g_show.TrafficEnd out
    End If
    
End Property

Public Property Get Position() As Integer

    If Not g_show Is Nothing Then
        If g_show.chkPosition.Value = 1 Then _
            g_show.TrafficStart "Position: "
    End If
        
    check_connected
    
    If g_bMoving Then   ' Spec. says we must return -1 is position not determined
        Position = -1
    Else
        Position = g_iPosition
    End If
    
    If Not g_show Is Nothing Then
        If g_show.chkPosition.Value = 1 Then _
            g_show.TrafficEnd CStr(Position)
    End If

End Property

Public Property Let Position(ByVal val As Integer)

    Dim Time As Integer
    
    If Not g_show Is Nothing Then
        If g_show.chkPosition.Value = 1 Then _
            g_show.TrafficStart "Position: " & val
    End If
      
    check_connected

    ' check if we are already there!
    If val = g_iPosition Then
        If Not g_show Is Nothing Then
            If g_show.chkPosition.Value = 1 Then _
                g_show.TrafficEnd " (no move required)"
        End If
        Exit Property
    End If


    ' position range check
    If val > g_iSlots - 1 Or val < 0 Then
        If Not g_show Is Nothing Then
            If g_show.chkPosition.Value = 1 Then _
                g_show.TrafficEnd " (aborting, range)"
        End If
        Err.Raise SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
            "Position: " & MSG_VAL_OUTOFRANGE
    End If
    
    ' do the move
    
    ' Find the shortest distance between two filter positions
    Time = Min(Abs(g_iPosition - val), g_iSlots - Abs(g_iPosition - val)) * g_iTimeInterval
    g_handBox.TimerMove.Interval = Time
    g_handBox.TimerMove.enabled = True
    g_iPosition = val

    ' trigger the "motor"
    g_bMoving = True
    
    ' update the handbox
    g_handBox.Moving = True

    
    If Not g_show Is Nothing Then
        If g_show.chkMove.Value = 1 Then _
            g_show.TrafficEnd " (started)"
    End If
    
    ' move code in Startup.bas journals end of move
End Property



' -------
' Methods
' -------

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


'---------------------------------------------------------------------
'
' check_connected() - Raise an error if the focuser is not connected
'
'---------------------------------------------------------------------
Private Sub check_connected()

    If Not g_bLinked Then _
        Err.Raise SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED
        
End Sub

'---------------------------------------------------------------------
'
' Min() - returns the smallest of two numbers
'
'---------------------------------------------------------------------
Private Function Min(a, b) As Variant
    Min = IIf(a <= b, a, b)
End Function


' ==============================
' Implementation of IFilterWheel
' ==============================

Private Property Get IFilterWheel_FocusOffsets() As Long()

    IFilterWheel_FocusOffsets = FocusOffsets
    
End Property

Private Property Get IFilterWheel_Names() As String()
    
    IFilterWheel_Names = Names
    
End Property

Private Property Get IFilterWheel_Connected() As Boolean

    IFilterWheel_Connected = Connected
    
End Property

Private Property Let IFilterWheel_Connected(ByVal newVal As Boolean)

    Connected = newVal
    
End Property

Private Property Get IFilterWheel_Position() As Integer

    IFilterWheel_Position = Position
    
End Property

Private Property Let IFilterWheel_Position(ByVal newVal As Integer)

    Position = newVal
    
End Property

Private Sub IFilterWheel_SetupDialog()

    SetupDialog
    
End Sub
