Attribute VB_Name = "Startup"
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
'   ============
'   STARTUP.BAS
'   ============
'
' ASCOM Filter Wheel simulator main startup module
'
' From Focus Simulator, from Scope Simulator written 27-Jun-00
' Robert B. Denny <rdenny@dc3.com>
' Structure retained, otherwise rewriten into Filter Wheel Simulator
' by Mark Crossley in Nov 2008
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 18-Nov-08 mpc     Initial edit - Starting from Focus Simulator
' -----------------------------------------------------------------------------
Option Explicit

Public Const ALERT_TITLE As String = "ASCOM Filter Wheel Simulator"

' ---------------------
' Simulation Parameters
' ---------------------

'
' Timer interval (sec.)
'
Public Const TIMER_INTERVAL = 0.25      ' 4 tix/sec
Public Const MOVE_INTERVAL = 2000       ' 2 secs per filter slot
Private timer_count As Integer
'
' ASCOM Identifiers
'
Public Const ID As String = "FilterWheelSim.FilterWheel"
Private Const DESC As String = "Filter Wheel Simulator"
Private Const RegVer As String = "0.1"

' ---------
' Variables
' ---------
Public g_Profile As DriverHelper.Profile
Public g_bRunExecutable As Boolean
Public g_iConnections As Integer

' ---------------
' State Variables
' ---------------
Public g_bLinked As Boolean     ' Filter wheel is connected
Public g_iPosition As Integer   ' Current wheel position

' ----------------------
' Filter Wheel variables
'-----------------------
Public g_iSlots As Integer                    ' Number of slots in the filter wheel
Public g_asFilterNames() As String            ' Filter names
Public g_alFocusOffsets() As Long             ' Focuser offsets for each filter
Public g_acFilterColours() As ColorConstants  ' Filter colours for display
Public g_bImplementsNames As Boolean          ' Report configured names or defaults
Public g_bImplementsOffsets As Boolean        ' Report configured offsets or zeros

' ---------------
' Move simulation
' ---------------
Public g_bMoving As Boolean        ' Whether filter wheel is moving
Public g_iTimeInterval As Integer  ' Millisecs to move between filters

' ----------------------
' Other global variables
' ----------------------
Public g_handBox As frmHandBox             ' Hand box
Public g_show As frmShow                   ' Traffic window
Public g_timer As VB.timer                 ' Handy reference to handbox timer
Public g_timerMove As VB.timer             ' Reference to move timer
Public g_bAlwaysTop As Boolean             ' True to keep sim topmost window

'---------------------------------------------------------------------
'
' Main() - Simulator main entry point
'
'---------------------------------------------------------------------

Sub Main()
  
    Dim i As Integer
    
    Set g_handBox = New frmHandBox
    Set g_timer = g_handBox.timer
    Set g_timerMove = g_handBox.TimerMove
    Set g_Profile = New DriverHelper.Profile
    g_Profile.DeviceType = "FilterWheel"            ' We're a filter wheel driver
      
    If App.StartMode = vbSModeStandalone Then
        g_bRunExecutable = True                     ' launched via double click
    Else
        g_bRunExecutable = False                    ' running as server only
    End If
    
    g_iConnections = 0                              ' zero connections currently
        
    '
    ' initialize variables that are not persistent
    '
    g_bLinked = False       ' Start unlinked
    g_bMoving = False       ' Start unmoving
    timer_count = 0
    ReDim g_asFilterNames(7)
    ReDim g_alFocusOffsets(7)
    ReDim g_acFilterColours(7)


    g_Profile.Register ID, DESC ' Self reg (skips if already reg)
        
    '
    ' Persistent settings - Create on first start as determined by
    ' existence of most recently added setting
    '
    If g_Profile.GetValue(ID, "RegVer") <> RegVer Then
        g_Profile.WriteValue ID, "RegVer", RegVer
        g_Profile.WriteValue ID, "Position", "0"
        g_Profile.WriteValue ID, "Slots", "4"
        g_Profile.WriteValue ID, "Time", "1000"
        g_Profile.WriteValue ID, "ImplementsNames", "True"
        g_Profile.WriteValue ID, "ImplementsOffsets", "True"
        g_Profile.WriteValue ID, "AlwaysOnTop", "True"
        g_Profile.WriteValue ID, "Left", "100"
        g_Profile.WriteValue ID, "Top", "100"
        For i = 0 To 7
            g_Profile.WriteValue ID, CStr(i), "Filter " & CStr(i), "FilterNames"
            g_Profile.WriteValue ID, CStr(i), "0", "FocusOffsets"
            g_Profile.WriteValue ID, CStr(i), "16777215", "FilterColours"
        Next i
    End If
    
    g_iPosition = CInt(g_Profile.GetValue(ID, "Position"))
    g_iSlots = CInt(g_Profile.GetValue(ID, "Slots"))
    g_iTimeInterval = CInt(g_Profile.GetValue(ID, "Time"))
    g_bImplementsNames = CBool(g_Profile.GetValue(ID, "ImplementsNames"))
    g_bImplementsOffsets = CBool(g_Profile.GetValue(ID, "ImplementsOffsets"))
    g_bAlwaysTop = CBool(g_Profile.GetValue(ID, "AlwaysOnTop"))
    For i = 0 To 7
        g_asFilterNames(i) = CStr(g_Profile.GetValue(ID, CStr(i), "FilterNames"))
        g_alFocusOffsets(i) = CLng(g_Profile.GetValue(ID, CStr(i), "FocusOffsets"))
        g_acFilterColours(i) = g_Profile.GetValue(ID, CStr(i), "FilterColours")
    Next i

    Load frmSetup
    
    '
    ' Show the handbox now and start the timer
    '
    Load g_handBox
    
    g_handBox.Left = CLng(g_Profile.GetValue(ID, "Left")) * Screen.TwipsPerPixelX
    g_handBox.Top = CLng(g_Profile.GetValue(ID, "Top")) * Screen.TwipsPerPixelY
    '
    ' Fix bad positions (which shouldn't ever happen, ha ha)
    '
    If g_handBox.Left < 0 Then
        g_handBox.Left = 100 * Screen.TwipsPerPixelX
        g_Profile.WriteValue ID, "Left", CStr(g_handBox.Left \ Screen.TwipsPerPixelX)
    End If
    If g_handBox.Top < 0 Then
        g_handBox.Top = 100 * Screen.TwipsPerPixelY
        g_Profile.WriteValue ID, "Top", CStr(g_handBox.Top \ Screen.TwipsPerPixelY)
    End If
    
    If g_bRunExecutable Then
        g_handBox.WindowState = vbNormal
    Else
        g_handBox.WindowState = vbMinimized
    End If
       
    g_handBox.Show
    
    g_timer.Interval = (TIMER_INTERVAL * 1000)
    g_timer.enabled = True
    g_timerMove.Interval = g_iTimeInterval
    
End Sub

'---------------------------------------------------------------------
'
' DoShutdown() - Handle handbox form Unload event
'
' Save form values for restoration on next start.
' Also "closes" the focuser connection.
'---------------------------------------------------------------------

Sub DoShutdown()

    Dim i As Integer

    g_bMoving = False
    g_bLinked = False
    
    g_timer.enabled = False
    
    g_Profile.WriteValue ID, "Position", CStr(g_iPosition)
    
    g_handBox.Visible = True
    g_handBox.WindowState = vbNormal
    g_Profile.WriteValue ID, "Left", CStr(g_handBox.Left \ Screen.TwipsPerPixelX)
    g_Profile.WriteValue ID, "Top", CStr(g_handBox.Top \ Screen.TwipsPerPixelY)
        
End Sub

'---------------------------------------------------------------------
'
' DoSetup() - Handle handbox Setup button click
'
'---------------------------------------------------------------------

Sub DoSetup()

    Dim i As Integer

    g_timer.enabled = False                         ' don't want races
    g_timerMove.enabled = False
    
    With frmSetup
        .AllowUnload = False                        ' Assure not unloaded
        .Slots = g_iSlots
        .Time = g_iTimeInterval
        .Names = g_asFilterNames
        .Offsets = g_alFocusOffsets
        .Colours = g_acFilterColours
        .chkAlwaysTop = IIf(g_bAlwaysTop, 1, 0)
        .chkImplementsNames = IIf(g_bImplementsNames, 1, 0)
        .chkImplementsOffsets = IIf(g_bImplementsOffsets, 1, 0)
    End With

    g_handBox.Visible = False                       ' May float over setup
    FloatWindow frmSetup.hwnd, True
    frmSetup.Show 1

    With frmSetup
        If .Result Then             ' Unless cancelled
            g_iSlots = .Slots
            g_Profile.WriteValue ID, "Slots", CStr(g_iSlots)
            If g_iPosition >= g_iSlots Then g_iPosition = 0 ' Reduced the number of slots?
            g_iTimeInterval = .Time
            g_Profile.WriteValue ID, "Time", CStr(g_iTimeInterval)
            g_asFilterNames = .Names
            g_alFocusOffsets = .Offsets
            g_acFilterColours = .Colours
            For i = 0 To 7
                g_Profile.WriteValue ID, i, CStr(g_asFilterNames(i)), "FilterNames"
                g_Profile.WriteValue ID, i, CStr(g_alFocusOffsets(i)), "FocusOffsets"
                g_Profile.WriteValue ID, i, CStr(g_acFilterColours(i)), "FilterColours"
            Next i
            g_bAlwaysTop = (.chkAlwaysTop = 1)
            g_Profile.WriteValue ID, "AlwaysOnTop", CStr(g_bAlwaysTop)
            g_bImplementsNames = (.chkImplementsNames = 1)
            g_Profile.WriteValue ID, "ImplementsNames", CStr(g_bImplementsNames)
            g_bImplementsOffsets = (.chkImplementsOffsets = 1)
            g_Profile.WriteValue ID, "ImplementsOffsets", CStr(g_bImplementsOffsets)
        End If
        .AllowUnload = True                     ' OK to unload now
    End With

    FloatWindow g_handBox.hwnd, g_bAlwaysTop
    g_handBox.Position = g_iPosition
    g_handBox.FilterName = g_asFilterNames(g_iPosition)
    g_handBox.FilterFocusOffset = CStr(g_alFocusOffsets(g_iPosition))
    g_handBox.FilterColour = g_acFilterColours(g_iPosition)
    g_handBox.Visible = True
    g_timer.enabled = True                      ' restart time

End Sub


'---------------------------------------------------------------------
'
' timer_tick() - Called when timer in frmHandbox fires event
'
' Implements handbox control.
'---------------------------------------------------------------------

Sub timer_tick()
    Dim button As Long
    
    ' Kept this timer based polling of the keys because I did think
    ' about implementing a filter movement animation. Ha!
    
    ' Only process the button if we are not already moving
    If Not g_bMoving Then
    
        button = g_handBox.ButtonState      ' Only fetch once due to debounce
        
        If button <> 0 Then                 ' Handbox buttons have priority

            Select Case button
                Case 1                                  ' Prev button
                    g_iPosition = g_iPosition - 1
                Case 2                                  ' Next button
                    g_iPosition = g_iPosition + 1
            End Select

            ' make sure position stays in range
            If g_iPosition > g_iSlots - 1 Then
                g_iPosition = 0
            ElseIf g_iPosition < 0 Then
                g_iPosition = g_iSlots - 1
            End If
    
            g_handBox.TimerMove.Interval = g_iTimeInterval
            g_handBox.TimerMove.enabled = True
    
            ' trigger the "motor"
            g_bMoving = True
                
            g_handBox.Moving = True
            
        End If
    End If
    
    
    
      
End Sub


'---------------------------------------------------------------------
'
' timer_move() - Called when the move timer in frmHandbox fires event
'
' Implements moving.
'---------------------------------------------------------------------
Sub timer_move()

    ' disable the timer now it has fired
    g_handBox.TimerMove.enabled = False

    '
    ' move complete, update everything
    '
    If g_bMoving Then
        If Not g_show Is Nothing Then
            If g_show.chkMove.Value = 1 Then
                g_show.TrafficLine " (move complete)"
            End If
        End If
        g_bMoving = False
    End If
    
    g_handBox.Moving = False

End Sub
