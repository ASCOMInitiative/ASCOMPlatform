VERSION 5.00
Begin VB.Form frmHandBox 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "FilterWheel Simulator"
   ClientHeight    =   4515
   ClientLeft      =   45
   ClientTop       =   255
   ClientWidth     =   2265
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   4515
   ScaleWidth      =   2265
   StartUpPosition =   3  'Windows Default
   Begin VB.PictureBox picFilterMoving 
      Appearance      =   0  'Flat
      BackColor       =   &H00404040&
      BorderStyle     =   0  'None
      ForeColor       =   &H80000008&
      Height          =   1050
      Left            =   570
      Picture         =   "frmHandBox.frx":0000
      ScaleHeight     =   1050
      ScaleMode       =   0  'User
      ScaleWidth      =   1050
      TabIndex        =   6
      TabStop         =   0   'False
      Top             =   240
      Width           =   1050
   End
   Begin VB.PictureBox picFilterStop 
      Appearance      =   0  'Flat
      BackColor       =   &H8000000D&
      BorderStyle     =   0  'None
      ForeColor       =   &H80000008&
      Height          =   1050
      Left            =   570
      Picture         =   "frmHandBox.frx":029A
      ScaleHeight     =   1050
      ScaleMode       =   0  'User
      ScaleWidth      =   1050
      TabIndex        =   12
      TabStop         =   0   'False
      Top             =   240
      Width           =   1050
   End
   Begin VB.Timer TimerMove 
      Enabled         =   0   'False
      Left            =   1800
      Top             =   0
   End
   Begin VB.CommandButton cmdTraffic 
      Caption         =   "Traffic"
      Height          =   330
      Left            =   1320
      TabIndex        =   3
      ToolTipText     =   "Bring up traffic dialog"
      Top             =   4080
      Width           =   825
   End
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   772
      MouseIcon       =   "frmHandBox.frx":0387
      MousePointer    =   99  'Custom
      Picture         =   "frmHandBox.frx":04D9
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   5
      TabStop         =   0   'False
      ToolTipText     =   "Click to go to the ASCOM web site"
      Top             =   3120
      Width           =   720
   End
   Begin VB.Timer timer 
      Enabled         =   0   'False
      Interval        =   250
      Left            =   0
      Top             =   0
   End
   Begin VB.CommandButton cmdSetup 
      Caption         =   "Setup"
      Height          =   330
      Left            =   120
      TabIndex        =   2
      ToolTipText     =   "Bring up Setup dialog"
      Top             =   4080
      Width           =   825
   End
   Begin VB.CommandButton cmdPrev 
      Caption         =   "Prev"
      Height          =   390
      Left            =   420
      TabIndex        =   0
      ToolTipText     =   "Click to move filter wheel"
      Top             =   1470
      Width           =   540
   End
   Begin VB.CommandButton cmdNext 
      Caption         =   "Next"
      Height          =   390
      Left            =   1290
      TabIndex        =   1
      ToolTipText     =   "Click to move filter wheel"
      Top             =   1470
      Width           =   540
   End
   Begin VB.Label OffsetL 
      BackColor       =   &H00000000&
      Caption         =   "Offset:"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   120
      TabIndex        =   11
      Top             =   2760
      Width           =   645
   End
   Begin VB.Label OffsetTL 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
      Caption         =   "0"
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H000000FF&
      Height          =   255
      Left            =   795
      TabIndex        =   10
      Top             =   2760
      Width           =   1350
   End
   Begin VB.Label PositionTI 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
      Caption         =   "0"
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H000000FF&
      Height          =   255
      Left            =   915
      TabIndex        =   7
      Top             =   2070
      Width           =   1230
   End
   Begin VB.Label PositionL 
      BackColor       =   &H00000000&
      Caption         =   "Position:"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   120
      TabIndex        =   4
      Top             =   2070
      Width           =   690
   End
   Begin VB.Label NameTS 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
      Caption         =   "Luminance"
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H000000FF&
      Height          =   255
      Left            =   750
      TabIndex        =   8
      Top             =   2385
      Width           =   1395
   End
   Begin VB.Label NameL 
      BackColor       =   &H00000000&
      Caption         =   "Name:"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   120
      TabIndex        =   9
      Top             =   2385
      Width           =   645
   End
End
Attribute VB_Name = "frmHandBox"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
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
'   ==============
'   FRMHANDBOX.FRM
'   ==============
'
' ASCOM Filter Wheel Simulator hand box form
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
' 18-Nov-08 mpc     Initial edit - Starting from Focus Simulator
' -----------------------------------------------------------------------------
Option Explicit

' button handling variables
Private BtnState As Integer         ' current state
Private BtnDebounce As Integer      ' hold key for fetch even of up occured
Private downclick As Boolean        ' button just went down - state unclear
Private getting As Boolean          ' timer going off - fetch in progress


' ======
' EVENTS
' ======

Private Sub Form_Load()

    FloatWindow Me.hwnd, g_bAlwaysTop
    
    'initialize variables
    BtnState = 0
    BtnDebounce = 0
    downclick = False
    getting = False
    
    LabelButtons
    
End Sub

Private Sub Form_Unload(Cancel As Integer)

    ' force clean state
    BtnState = 0
    BtnDebounce = 0
    getting = False
    downclick = False
    On Error Resume Next
    Unload frmSetup
    If Not g_show Is Nothing Then _
        Unload g_show
    DoShutdown              ' Saves parameters for next start
    
End Sub

Private Sub cmdPrev_MouseDown(button As Integer, Shift As Integer, X As Single, Y As Single)

    downclick = True
    BtnState = 1
    BtnDebounce = 1
    
End Sub

Private Sub cmdPrev_MouseUp(button As Integer, Shift As Integer, X As Single, Y As Single)

    BtnState = 0
    
    'race condition handling
    If getting Then _
        BtnDebounce = 0
    downclick = False

End Sub

Private Sub cmdNext_MouseDown(button As Integer, Shift As Integer, X As Single, Y As Single)

    downclick = True
    BtnState = 2
    BtnDebounce = 2
    
End Sub

Private Sub cmdNext_MouseUp(button As Integer, Shift As Integer, X As Single, Y As Single)

    BtnState = 0
    
    'race condition handling
    If getting Then _
        BtnDebounce = 0
    downclick = False

End Sub

Private Sub picASCOM_Click()

    DisplayWebPage "http://ascom-standards.org/"
    
End Sub

Private Sub cmdSetup_Click()

    DoSetup                         ' May change our topmost state
    
End Sub


Private Sub cmdTraffic_Click()

    If g_show Is Nothing Then
        Set g_show = New frmShow
        g_show.TextOffset = 1425
    End If
    
    g_show.Show
    
End Sub

Private Sub timer_Timer()
    ' handle this in Startup.bas
    timer_tick
    
End Sub

Private Sub TimerMove_Timer()
    ' handle this in Startup.bas
    timer_move

End Sub


' =================
' PUBLIC PROPERTIES
' =================

Public Property Let Position(ByVal val As String)

    PositionTI.Caption = val
    
End Property

Public Property Let FilterName(ByVal val As String)
    
    NameTS.Caption = val
    
End Property

Public Property Let FilterFocusOffset(ByVal val As String)

    OffsetTL.Caption = val

End Property

Public Property Let FilterColour(ByVal val As ColorConstants)

    picFilterStop.BackColor = val
    
End Property

Public Property Let Moving(val As Boolean)

    If val Then
        picFilterMoving.Visible = True
        Position = "moving"
        FilterName = ""
        FilterFocusOffset = ""
    Else
        picFilterMoving.Visible = False
        Position = g_iPosition
        FilterName = g_asFilterNames(g_iPosition)
        FilterFocusOffset = g_alFocusOffsets(g_iPosition)
        FilterColour = g_acFilterColours(g_iPosition)
    End If
    
End Property


Public Property Get ButtonState() As Integer

    ' race condition check, ignore if down just occuring
    If downclick Then
        downclick = False
        ButtonState = 0     ' button not really down yet
        Exit Sub
    End If
    
    ButtonState = BtnDebounce
    
    ' race condition, make sure not to return 2 presses
    getting = True
    If BtnState = 0 Then _
        BtnDebounce = 0
    getting = False
    
End Property

' ==============
' LOCAL ROUTINES
' ==============

Private Function LabelButtons()
      
    PositionTI.Caption = CStr(g_iPosition)
    NameTS.Caption = CStr(g_asFilterNames(g_iPosition))
    OffsetTL.Caption = CStr(g_alFocusOffsets(g_iPosition))
    picFilterMoving.Visible = g_bMoving
    picFilterStop.BackColor = g_acFilterColours(g_iPosition)
    
End Function

