VERSION 5.00
Begin VB.Form frmHandBox 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Focuser Simulator"
   ClientHeight    =   4425
   ClientLeft      =   45
   ClientTop       =   255
   ClientWidth     =   1980
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   4425
   ScaleWidth      =   1980
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdTraffic 
      Caption         =   "Traffic"
      Height          =   330
      Left            =   1080
      TabIndex        =   9
      ToolTipText     =   "Bring up Setup dialog"
      Top             =   3000
      Width           =   825
   End
   Begin VB.CheckBox TempCompCB 
      BackColor       =   &H00000000&
      Caption         =   "Temperature compensation"
      ForeColor       =   &H00FFFFFF&
      Height          =   495
      Left            =   323
      TabIndex        =   7
      ToolTipText     =   "Turn on/off temperature compensation"
      Top             =   2280
      Width           =   1335
   End
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   630
      MouseIcon       =   "frmHandBox.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "frmHandBox.frx":0152
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   4
      TabStop         =   0   'False
      ToolTipText     =   "Click to go to the ASCOM web site"
      Top             =   3480
      Width           =   720
   End
   Begin VB.Timer timer 
      Enabled         =   0   'False
      Interval        =   250
      Left            =   1440
      Top             =   3840
   End
   Begin VB.CommandButton cmdSetup 
      Caption         =   "Setup"
      Height          =   330
      Left            =   120
      TabIndex        =   8
      ToolTipText     =   "Bring up Setup dialog"
      Top             =   3000
      Width           =   825
   End
   Begin VB.CommandButton cmdIn 
      Caption         =   "In"
      Height          =   390
      Left            =   285
      TabIndex        =   5
      ToolTipText     =   "Click to move focuser"
      Top             =   1230
      Width           =   420
   End
   Begin VB.CommandButton cmdOut 
      Caption         =   "Out"
      Height          =   390
      Left            =   1275
      TabIndex        =   6
      ToolTipText     =   "Click to move focuser"
      Top             =   1230
      Width           =   420
   End
   Begin VB.Label PositionTF 
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
      Left            =   870
      TabIndex        =   1
      Top             =   750
      Width           =   945
   End
   Begin VB.Label PositionL 
      BackColor       =   &H00000000&
      Caption         =   "Position:"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   120
      TabIndex        =   0
      Top             =   750
      Width           =   690
   End
   Begin VB.Image imgBrewster 
      Appearance      =   0  'Flat
      Height          =   555
      Left            =   405
      MouseIcon       =   "frmHandBox.frx":1016
      MousePointer    =   99  'Custom
      Picture         =   "frmHandBox.frx":1168
      ToolTipText     =   "Click to go to astro.brewsters.net"
      Top             =   120
      Width           =   1170
   End
   Begin VB.Label TempTF 
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
      Left            =   960
      TabIndex        =   2
      Top             =   1905
      Width           =   840
   End
   Begin VB.Label TempL 
      BackColor       =   &H00000000&
      Caption         =   "Temp (°C):"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   120
      TabIndex        =   3
      Top             =   1905
      Width           =   1245
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
' ASCOM Focuser Simulator hand box form
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
' 28-Feb-03 jab     completed addition of temperature simulation
' 01-Mar-03 jab     reworked button / timer interaction for race conditions
' 02-Mar-03 jab     added more accurate temperature compensation
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

Private Sub imgBrewster_Click()

    DisplayWebPage "http://astro.brewsters.net/"
    
End Sub

Private Sub cmdIn_MouseDown(button As Integer, Shift As Integer, X As Single, Y As Single)

    downclick = True
    BtnState = 1
    BtnDebounce = 1
    
End Sub

Private Sub cmdIn_MouseUp(button As Integer, Shift As Integer, X As Single, Y As Single)

    BtnState = 0
    
    'race condition handling
    If getting Then _
        BtnDebounce = 0
    downclick = False

End Sub

Private Sub cmdOut_MouseDown(button As Integer, Shift As Integer, X As Single, Y As Single)

    downclick = True
    BtnState = 2
    BtnDebounce = 2
    
End Sub

Private Sub cmdOut_MouseUp(button As Integer, Shift As Integer, X As Single, Y As Single)

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

Private Sub TempCompCB_Click()

    If TempCompCB.Value = 0 Then
        g_bTempComp = False
        
    ElseIf TempCompCB.Value = 1 Then
        ' remember current values for accurate compensation
        g_dTTemp = g_dTemp
        g_lTPos = g_lPosition
        
        g_bTempComp = True
    End If
    
    LabelButtons
    
End Sub

Private Sub cmdTraffic_Click()

    If g_show Is Nothing Then
        Set g_show = New frmShow
        g_show.TextOffset = 1425
    End If
    
    g_show.Show
    
End Sub

Private Sub timer_Timer()

    timer_tick
    
End Sub

' =================
' PUBLIC PROPERTIES
' =================

Public Property Let Position(val As Long)

    PositionTF.Caption = val
    
End Property

Public Property Let temp(val As Double)

    ' only display temp if probe enabled
    If (g_bCanTemp) Then
        TempTF.Caption = Format$(val, "0.0")
    Else
        TempTF.Caption = "--.-"
    End If
    
End Property

Public Property Let TempComp(val As Integer)

    TempCompCB.Value = val
    LabelButtons
    
End Property

Public Property Get ButtonState() As Integer

    ' race condition check, ignore if down just occuring
    If downclick Then
        downclick = False
        ButtonState = 0     ' button not realy down yet
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

    ' only enable temp comp if currently supported
    TempCompCB.Enabled = g_bCanTemp And g_bCanTempComp
    
    '
    ' disable move buttons if really compensating for temerature
    '
    If g_bTempComp And g_bCanTemp And g_bCanTempComp Then
        cmdIn.Enabled = False
        cmdOut.Enabled = False
    Else
        cmdIn.Enabled = True
        cmdOut.Enabled = True
    End If
      
End Function
