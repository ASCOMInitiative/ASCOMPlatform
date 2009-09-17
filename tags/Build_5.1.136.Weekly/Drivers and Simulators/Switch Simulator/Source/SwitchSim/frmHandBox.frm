VERSION 5.00
Begin VB.Form frmHandBox 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Switch Simulator"
   ClientHeight    =   6060
   ClientLeft      =   45
   ClientTop       =   255
   ClientWidth     =   2910
   Icon            =   "frmHandBox.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   6060
   ScaleWidth      =   2910
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdToggle 
      Caption         =   "0"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   0
      Left            =   285
      TabIndex        =   10
      ToolTipText     =   "Click to toggle switch"
      Top             =   1425
      Width           =   1620
   End
   Begin VB.CommandButton cmdToggle 
      Caption         =   "8"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   8
      Left            =   285
      TabIndex        =   9
      ToolTipText     =   "Click to toggle switch"
      Top             =   5545
      Width           =   1620
   End
   Begin VB.CommandButton cmdToggle 
      Caption         =   "7"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   7
      Left            =   285
      TabIndex        =   8
      ToolTipText     =   "Click to toggle switch"
      Top             =   5030
      Width           =   1620
   End
   Begin VB.CommandButton cmdToggle 
      Caption         =   "6"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   6
      Left            =   285
      TabIndex        =   7
      ToolTipText     =   "Click to toggle switch"
      Top             =   4515
      Width           =   1620
   End
   Begin VB.CommandButton cmdToggle 
      Caption         =   "5"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   5
      Left            =   285
      TabIndex        =   6
      ToolTipText     =   "Click to toggle switch"
      Top             =   4000
      Width           =   1620
   End
   Begin VB.CommandButton cmdToggle 
      Caption         =   "4"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   4
      Left            =   285
      TabIndex        =   5
      ToolTipText     =   "Click to toggle switch"
      Top             =   3485
      Width           =   1620
   End
   Begin VB.CommandButton cmdToggle 
      Caption         =   "3"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   3
      Left            =   285
      TabIndex        =   4
      ToolTipText     =   "Click to toggle switch"
      Top             =   2970
      Width           =   1620
   End
   Begin VB.CommandButton cmdToggle 
      Caption         =   "2"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   2
      Left            =   285
      TabIndex        =   3
      ToolTipText     =   "Click to toggle switch"
      Top             =   2455
      Width           =   1620
   End
   Begin VB.CommandButton cmdTraffic 
      Caption         =   "Traffic"
      Height          =   330
      Left            =   285
      TabIndex        =   2
      ToolTipText     =   "Bring up Traffic dialog"
      Top             =   870
      Width           =   825
   End
   Begin VB.CommandButton cmdSetup 
      Caption         =   "Setup"
      Height          =   330
      Left            =   1770
      TabIndex        =   1
      ToolTipText     =   "Bring up Setup dialog"
      Top             =   870
      Width           =   825
   End
   Begin VB.CommandButton cmdToggle 
      Caption         =   "1"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   1
      Left            =   285
      TabIndex        =   0
      ToolTipText     =   "Click to toggle switch"
      Top             =   1940
      Width           =   1620
   End
   Begin VB.Shape shpLED 
      BorderColor     =   &H00E0E0E0&
      BorderWidth     =   4
      FillColor       =   &H000000FF&
      FillStyle       =   0  'Solid
      Height          =   345
      Index           =   0
      Left            =   2265
      Shape           =   3  'Circle
      Top             =   1455
      Width           =   330
   End
   Begin VB.Shape shpLED 
      BorderColor     =   &H00E0E0E0&
      BorderWidth     =   4
      FillColor       =   &H000000FF&
      FillStyle       =   0  'Solid
      Height          =   345
      Index           =   8
      Left            =   2265
      Shape           =   3  'Circle
      Top             =   5580
      Width           =   330
   End
   Begin VB.Shape shpLED 
      BorderColor     =   &H00E0E0E0&
      BorderWidth     =   4
      FillColor       =   &H000000FF&
      FillStyle       =   0  'Solid
      Height          =   345
      Index           =   7
      Left            =   2265
      Shape           =   3  'Circle
      Top             =   5055
      Width           =   330
   End
   Begin VB.Shape shpLED 
      BorderColor     =   &H00E0E0E0&
      BorderWidth     =   4
      FillColor       =   &H000000FF&
      FillStyle       =   0  'Solid
      Height          =   345
      Index           =   5
      Left            =   2265
      Shape           =   3  'Circle
      Top             =   4035
      Width           =   330
   End
   Begin VB.Shape shpLED 
      BorderColor     =   &H00E0E0E0&
      BorderWidth     =   4
      FillColor       =   &H000000FF&
      FillStyle       =   0  'Solid
      Height          =   345
      Index           =   6
      Left            =   2265
      Shape           =   3  'Circle
      Top             =   4545
      Width           =   330
   End
   Begin VB.Shape shpLED 
      BorderColor     =   &H00E0E0E0&
      BorderWidth     =   4
      FillColor       =   &H000000FF&
      FillStyle       =   0  'Solid
      Height          =   345
      Index           =   4
      Left            =   2265
      Shape           =   3  'Circle
      Top             =   3510
      Width           =   330
   End
   Begin VB.Shape shpLED 
      BorderColor     =   &H00E0E0E0&
      BorderWidth     =   4
      FillColor       =   &H000000FF&
      FillStyle       =   0  'Solid
      Height          =   345
      Index           =   3
      Left            =   2265
      Shape           =   3  'Circle
      Top             =   3000
      Width           =   330
   End
   Begin VB.Shape shpLED 
      BorderColor     =   &H00E0E0E0&
      BorderWidth     =   4
      FillColor       =   &H000000FF&
      FillStyle       =   0  'Solid
      Height          =   345
      Index           =   1
      Left            =   2265
      Shape           =   3  'Circle
      Top             =   1965
      Width           =   330
   End
   Begin VB.Shape shpLED 
      BorderColor     =   &H00E0E0E0&
      BorderWidth     =   4
      FillColor       =   &H000000FF&
      FillStyle       =   0  'Solid
      Height          =   345
      Index           =   2
      Left            =   2265
      Shape           =   3  'Circle
      Top             =   2490
      Width           =   330
   End
   Begin VB.Image imgBrewster 
      Appearance      =   0  'Flat
      Height          =   555
      Left            =   870
      MouseIcon       =   "frmHandBox.frx":08CA
      MousePointer    =   99  'Custom
      Picture         =   "frmHandBox.frx":0A1C
      ToolTipText     =   "Click to go to astro.brewsters.net"
      Top             =   120
      Width           =   1170
   End
End
Attribute VB_Name = "frmHandBox"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'   ==============
'   FRMHANDBOX.FRM
'   ==============
'
' ASCOM Switch Simulator hand box form
'
' Initial code by Jon Brewster in Feb 2007
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 29-Jan-07 jab     Initial edit
' 02-Jun-07 jab     Added naming lables to the switches
' -----------------------------------------------------------------------------

' ======
' EVENTS
' ======

Private OrgHeight As Long

Private Sub Form_Load()
    
    OrgHeight = Height
    Buttons
    
End Sub

Private Sub Form_Unload(Cancel As Integer)

    On Error Resume Next
    Unload frmSetup
    If Not g_show Is Nothing Then _
        Unload g_show
    DoShutdown              ' Saves parameters for next start
    
End Sub

Private Sub Form_Resize()

    Buttons
        
End Sub

Private Sub cmdToggle_Click(Index As Integer)

    Dim trueIndex As Integer
    Dim val As Boolean
    
    trueIndex = Index + IIf(g_bZero, 0, 1)
    
    val = Not g_bSwitchState(trueIndex)

    g_bSwitchState(trueIndex) = val
    g_handBox.SetLED trueIndex, val
    
End Sub

Private Sub imgBrewster_Click()

    DisplayWebPage "http://astro.brewsters.net/"
    
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

Public Sub SetLED(val As Integer, state As Boolean)

    Dim trueIndex As Integer
    
    trueIndex = val - IIf(g_bZero, 0, 1)
    If trueIndex >= 0 Then _
        shpLED(trueIndex).FillColor = IIf(state, &HFF&, &H0&)
    
End Sub

Public Sub DisplayName(val As Integer)

    Dim trueIndex As Integer
    
    trueIndex = val - IIf(g_bZero, 0, 1)
    If trueIndex >= 0 Then _
        cmdToggle(trueIndex).Caption = g_sSwitchName(val)
    
End Sub

Public Sub Buttons()

    Dim i As Integer
    Dim offset As Integer
    Dim CtoC As Long

    If (Not Visible) Or Not (WindowState = vbNormal) Then _
        Exit Sub
        
    offset = IIf(g_bZero, 0, 1)
        
    For i = offset To (NUM_SWITCHES - 1)
        DisplayName i
        cmdToggle(i - offset).Enabled = (i <= g_iMaxSwitch)
        SetLED i, g_bSwitchState(i)
    Next i
    
    CtoC = cmdToggle(1).Top - cmdToggle(0).Top
    Height = OrgHeight - _
        CtoC * (NUM_SWITCHES - 1 - g_iMaxSwitch + IIf(g_bZero, 0, 1))
    
'    Height = cmdToggle(NUM_SWITCHES - 1).Top + Top - _
'        CtoC * (NUM_SWITCHES - 2 - g_iMaxSwitch - IIf(g_bZero, 0, 1))
    
End Sub


