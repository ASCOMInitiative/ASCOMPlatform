VERSION 5.00
Begin VB.Form frmHandBox 
   BackColor       =   &H00000000&
   BorderStyle     =   1  'Fixed Single
   Caption         =   "POTH Handset"
   ClientHeight    =   5325
   ClientLeft      =   45
   ClientTop       =   375
   ClientWidth     =   6135
   Icon            =   "frmHandBox.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   5325
   ScaleWidth      =   6135
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame frmFocus 
      BackColor       =   &H00000000&
      BorderStyle     =   0  'None
      ForeColor       =   &H00FFFFFF&
      Height          =   4605
      Left            =   4155
      TabIndex        =   51
      Top             =   660
      Width           =   1965
      Begin VB.CommandButton cmdFocuserMove 
         Caption         =   "Move:"
         Height          =   450
         Left            =   120
         TabIndex        =   26
         ToolTipText     =   "Move focuser to value"
         Top             =   600
         Width           =   695
      End
      Begin VB.CheckBox chkFocuserAbsMove 
         BackColor       =   &H00000000&
         Caption         =   "Abs Move"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   1080
         TabIndex        =   29
         ToolTipText     =   "Toggle absolute / relative focusing for absolute focusers"
         Top             =   1500
         Width           =   855
      End
      Begin VB.CommandButton cmdFocuserResetPosition 
         Caption         =   "Zero"
         Height          =   450
         Left            =   1165
         TabIndex        =   31
         ToolTipText     =   "Zero the relative focuser point"
         Top             =   2040
         Width           =   590
      End
      Begin VB.CommandButton cmdConnectFocuser 
         Caption         =   "Connect Focuser"
         Height          =   450
         Left            =   120
         TabIndex        =   30
         ToolTipText     =   "Take control of the focuser"
         Top             =   2040
         Width           =   1005
      End
      Begin VB.CommandButton cmdFocuserIn 
         Caption         =   "In"
         Height          =   375
         Left            =   555
         TabIndex        =   32
         ToolTipText     =   "Move focuser in by last increment above"
         Top             =   3100
         Width           =   615
      End
      Begin VB.CommandButton cmdFocuserOut 
         Caption         =   "Out"
         Height          =   375
         Left            =   555
         TabIndex        =   34
         ToolTipText     =   "move focuser out by last increment above"
         Top             =   4160
         Width           =   615
      End
      Begin VB.CheckBox chkFocuserTempComp 
         BackColor       =   &H00000000&
         Caption         =   "Temp Comp"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   120
         TabIndex        =   28
         ToolTipText     =   "Toggle temperature compensation"
         Top             =   1500
         Width           =   855
      End
      Begin VB.CommandButton cmdFocuserHalt 
         Caption         =   "Halt"
         Height          =   450
         Left            =   555
         TabIndex        =   33
         ToolTipText     =   "Halt focuser movement"
         Top             =   3600
         Width           =   615
      End
      Begin VB.TextBox txtFocuserMove 
         Height          =   315
         Left            =   905
         TabIndex        =   27
         ToolTipText     =   "In relative mode - increment, in absolute mode - position"
         Top             =   668
         Width           =   850
      End
      Begin VB.Label txtFocuserSteps 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00000000&
         Caption         =   "------"
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
         Left            =   840
         TabIndex        =   57
         Top             =   285
         Width           =   915
      End
      Begin VB.Label Label5 
         BackColor       =   &H00000000&
         Caption         =   "Steps:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Left            =   120
         TabIndex        =   56
         Top             =   300
         Width           =   450
      End
      Begin VB.Shape shpFocuserError 
         BorderColor     =   &H00E0E0E0&
         BorderStyle     =   0  'Transparent
         FillStyle       =   0  'Solid
         Height          =   135
         Left            =   960
         Shape           =   3  'Circle
         Top             =   1800
         Width           =   135
      End
      Begin VB.Label txtFocuserTemperature 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00000000&
         Caption         =   "---.-"
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
         Left            =   840
         TabIndex        =   55
         Top             =   1150
         Width           =   915
      End
      Begin VB.Label lblTemperature 
         BackColor       =   &H00000000&
         Caption         =   "Temp:"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   120
         TabIndex        =   54
         Top             =   1150
         Width           =   570
      End
      Begin VB.Label lblPosition 
         BackColor       =   &H00000000&
         Caption         =   "Microns:"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   120
         TabIndex        =   53
         Top             =   0
         Width           =   675
      End
      Begin VB.Label txtFocuserPosition 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00000000&
         Caption         =   "------"
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
         Left            =   840
         TabIndex        =   52
         Top             =   0
         Width           =   915
      End
   End
   Begin VB.Frame frmDome 
      BackColor       =   &H00000000&
      BorderStyle     =   0  'None
      ForeColor       =   &H00FFFFFF&
      Height          =   4605
      Left            =   2050
      TabIndex        =   46
      Top             =   660
      Width           =   2105
      Begin VB.CheckBox chkSlave 
         BackColor       =   &H00000000&
         Caption         =   "Slave Dome"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   180
         TabIndex        =   16
         ToolTipText     =   "Cause dome to track scope"
         Top             =   1500
         Width           =   735
      End
      Begin VB.CommandButton cmdOpenDome 
         Caption         =   "Open Dome"
         Height          =   435
         Left            =   1170
         TabIndex        =   17
         ToolTipText     =   "Open/Close the dome shutters"
         Top             =   1500
         Width           =   705
      End
      Begin VB.CommandButton cmdGoto 
         Caption         =   "Sync:"
         Height          =   330
         Index           =   1
         Left            =   135
         TabIndex        =   15
         ToolTipText     =   "Sync dome to value"
         Top             =   1020
         Width           =   825
      End
      Begin VB.CommandButton cmdGoto 
         Caption         =   "Goto:"
         Height          =   330
         Index           =   0
         Left            =   135
         TabIndex        =   13
         ToolTipText     =   "Slew dome to value"
         Top             =   660
         Width           =   825
      End
      Begin VB.TextBox txtNewAz 
         Height          =   315
         Left            =   1095
         TabIndex        =   14
         ToolTipText     =   "Value to slew / sync dome to"
         Top             =   840
         Width           =   780
      End
      Begin VB.ComboBox cbJogDome 
         Height          =   315
         ItemData        =   "frmHandBox.frx":08CA
         Left            =   120
         List            =   "frmHandBox.frx":08E9
         Style           =   2  'Dropdown List
         TabIndex        =   20
         ToolTipText     =   "Amount to jog dome by"
         Top             =   3120
         Width           =   615
      End
      Begin VB.CommandButton cmdParkDome 
         Caption         =   "Park Dome"
         Height          =   450
         Left            =   1170
         TabIndex        =   19
         ToolTipText     =   "Slew dome to it's park position"
         Top             =   2040
         Width           =   705
      End
      Begin VB.CommandButton cmdSlew 
         Caption         =   "Q"
         BeginProperty Font 
            Name            =   "Wingdings 3"
            Size            =   14.25
            Charset         =   2
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   435
         Index           =   8
         Left            =   1380
         TabIndex        =   24
         ToolTipText     =   "Jog dome counter-clockwise"
         Top             =   3615
         Width           =   495
      End
      Begin VB.CommandButton cmdSlew 
         Caption         =   "P"
         BeginProperty Font 
            Name            =   "Wingdings 3"
            Size            =   14.25
            Charset         =   2
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   435
         Index           =   9
         Left            =   240
         TabIndex        =   22
         ToolTipText     =   "Jog dome clockwise"
         Top             =   3615
         Width           =   495
      End
      Begin VB.CommandButton cmdSlew 
         Caption         =   "Ä"
         BeginProperty Font 
            Name            =   "Wingdings 2"
            Size            =   18
            Charset         =   2
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   450
         Index           =   10
         Left            =   780
         TabIndex        =   23
         ToolTipText     =   "Halt all dome and shutter motion"
         Top             =   3600
         Width           =   540
      End
      Begin VB.CommandButton cmdSlew 
         Caption         =   "¤"
         BeginProperty Font 
            Name            =   "Wingdings 3"
            Size            =   14.25
            Charset         =   2
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   435
         Index           =   7
         Left            =   810
         TabIndex        =   25
         ToolTipText     =   "Jog shutter opening down"
         Top             =   4140
         Width           =   495
      End
      Begin VB.CommandButton cmdSlew 
         Caption         =   "£"
         BeginProperty Font 
            Name            =   "Wingdings 3"
            Size            =   14.25
            Charset         =   2
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   435
         Index           =   6
         Left            =   810
         TabIndex        =   21
         ToolTipText     =   "Jog shutter opening up"
         Top             =   3060
         Width           =   495
      End
      Begin VB.CommandButton cmdConnectDome 
         Caption         =   "Connect Dome"
         Height          =   450
         Left            =   120
         TabIndex        =   18
         ToolTipText     =   "Take control of the dome"
         Top             =   2040
         Width           =   1005
      End
      Begin VB.Label txtShutter 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00000000&
         Caption         =   "---.-"
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
         Left            =   840
         TabIndex        =   50
         Top             =   0
         Width           =   1035
      End
      Begin VB.Label lblShutter 
         BackColor       =   &H00000000&
         Caption         =   "Shutter:"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   180
         TabIndex        =   49
         Top             =   0
         Width           =   570
      End
      Begin VB.Label txtDomeAzimuth 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00000000&
         Caption         =   "---.-"
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
         TabIndex        =   48
         Top             =   285
         Width           =   915
      End
      Begin VB.Label lblDomeAz 
         BackColor       =   &H00000000&
         Caption         =   "Az:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Left            =   180
         TabIndex        =   47
         Top             =   300
         Width           =   570
      End
      Begin VB.Shape shpDomeError 
         BorderColor     =   &H00E0E0E0&
         BorderStyle     =   0  'Transparent
         FillStyle       =   0  'Solid
         Height          =   135
         Left            =   960
         Shape           =   3  'Circle
         Top             =   1860
         Width           =   135
      End
   End
   Begin VB.CommandButton cmdFlip 
      Caption         =   "Flip"
      Height          =   330
      Left            =   120
      TabIndex        =   11
      ToolTipText     =   "Flip the scope to the other side of the pier"
      Top             =   4860
      Width           =   675
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "N"
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
      Left            =   900
      TabIndex        =   7
      ToolTipText     =   "Jog scope direction indicated"
      Top             =   3780
      Width           =   420
   End
   Begin VB.ComboBox cbJog 
      Height          =   315
      ItemData        =   "frmHandBox.frx":0905
      Left            =   120
      List            =   "frmHandBox.frx":0939
      Style           =   2  'Dropdown List
      TabIndex        =   6
      ToolTipText     =   "Amount to jog scope by"
      Top             =   3780
      Width           =   675
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "E"
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
      Left            =   1440
      TabIndex        =   10
      ToolTipText     =   "Jog scope direction indicated"
      Top             =   4290
      Width           =   420
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "W"
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
      Left            =   360
      TabIndex        =   8
      ToolTipText     =   "Jog scope direction indicated"
      Top             =   4290
      Width           =   420
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "Ä"
      BeginProperty Font 
         Name            =   "Wingdings 2"
         Size            =   18
         Charset         =   2
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   450
      Index           =   5
      Left            =   840
      TabIndex        =   9
      ToolTipText     =   "Halt all scope motion"
      Top             =   4260
      Width           =   540
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "S"
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
      Left            =   900
      TabIndex        =   12
      ToolTipText     =   "Jog scope direction indicated"
      Top             =   4800
      Width           =   420
   End
   Begin VB.CommandButton cmdParkScope 
      Caption         =   "Unpark Scope"
      Height          =   450
      Left            =   1170
      TabIndex        =   3
      ToolTipText     =   "Slew scope to it's park position, or unpark"
      Top             =   2700
      Width           =   705
   End
   Begin VB.CommandButton cmdConnectScope 
      Caption         =   "Connect Scope"
      Height          =   450
      Left            =   120
      TabIndex        =   2
      ToolTipText     =   "Take control of the scope"
      Top             =   2700
      Width           =   1005
   End
   Begin VB.CommandButton cmdTraffic 
      Caption         =   "Traffic"
      Height          =   330
      Left            =   120
      TabIndex        =   4
      ToolTipText     =   "Bring up traffic monitor window"
      Top             =   3240
      Width           =   1005
   End
   Begin VB.CommandButton cmdSetup 
      Caption         =   "Setup"
      Height          =   330
      Left            =   1170
      TabIndex        =   5
      ToolTipText     =   "Bring up POTH setup dialog"
      Top             =   3240
      Width           =   705
   End
   Begin VB.CheckBox chkTracking 
      BackColor       =   &H00000000&
      Caption         =   "Track"
      ForeColor       =   &H00FFFFFF&
      Height          =   195
      Left            =   1140
      TabIndex        =   1
      ToolTipText     =   "Turn on/off scope tracking"
      Top             =   2280
      Width           =   765
   End
   Begin VB.CheckBox chkQuiet 
      BackColor       =   &H00000000&
      Caption         =   "Quiet Scope"
      ForeColor       =   &H00FFFFFF&
      Height          =   375
      Left            =   180
      TabIndex        =   0
      ToolTipText     =   "Minimize scope I/O traffic"
      Top             =   2190
      Width           =   765
   End
   Begin VB.Label lblSlewing 
      BackColor       =   &H00000000&
      Caption         =   "Slewing"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   1230
      TabIndex        =   45
      Top             =   2265
      Width           =   585
   End
   Begin VB.Shape shpPark 
      BorderColor     =   &H00E0E0E0&
      BorderStyle     =   0  'Transparent
      FillColor       =   &H0000FF00&
      FillStyle       =   0  'Solid
      Height          =   135
      Left            =   1740
      Shape           =   3  'Circle
      Top             =   2520
      Width           =   135
   End
   Begin VB.Shape shpWest 
      BorderColor     =   &H00E0E0E0&
      BorderStyle     =   0  'Transparent
      FillColor       =   &H0000FF00&
      FillStyle       =   0  'Solid
      Height          =   135
      Left            =   360
      Shape           =   3  'Circle
      Top             =   4140
      Width           =   135
   End
   Begin VB.Shape shpEast 
      BorderColor     =   &H00E0E0E0&
      BorderStyle     =   0  'Transparent
      FillColor       =   &H0000FF00&
      FillStyle       =   0  'Solid
      Height          =   135
      Left            =   1680
      Shape           =   3  'Circle
      Top             =   4140
      Width           =   180
   End
   Begin VB.Shape shpScopeError 
      BorderColor     =   &H00E0E0E0&
      BorderStyle     =   0  'Transparent
      FillStyle       =   0  'Solid
      Height          =   135
      Left            =   960
      Shape           =   3  'Circle
      Top             =   2520
      Width           =   135
   End
   Begin VB.Shape ShpError 
      BorderColor     =   &H00E0E0E0&
      BorderStyle     =   0  'Transparent
      FillStyle       =   0  'Solid
      Height          =   135
      Left            =   120
      Shape           =   3  'Circle
      Top             =   60
      Width           =   255
   End
   Begin VB.Image imgBrewster 
      Height          =   555
      Left            =   2317
      MouseIcon       =   "frmHandBox.frx":096E
      MousePointer    =   99  'Custom
      Picture         =   "frmHandBox.frx":0AC0
      ToolTipText     =   "Click to go to Jon Brewster's astro web site"
      Top             =   60
      Width           =   1170
   End
   Begin VB.Label txtAltitude 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
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
      Left            =   555
      TabIndex        =   44
      Top             =   1890
      Width           =   1275
   End
   Begin VB.Label Label7 
      BackColor       =   &H00000000&
      Caption         =   "Alt"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   165
      TabIndex        =   43
      Top             =   1905
      Width           =   345
   End
   Begin VB.Label txtAzimuth 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
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
      Left            =   555
      TabIndex        =   42
      Top             =   1590
      Width           =   1275
   End
   Begin VB.Label Label3 
      BackColor       =   &H00000000&
      Caption         =   "Az"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   165
      TabIndex        =   41
      Top             =   1605
      Width           =   345
   End
   Begin VB.Label txtLST 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
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
      Left            =   555
      TabIndex        =   40
      Top             =   690
      Width           =   1275
   End
   Begin VB.Label Label1 
      BackColor       =   &H00000000&
      Caption         =   "LST"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   180
      TabIndex        =   39
      Top             =   705
      Width           =   345
   End
   Begin VB.Label Label4 
      BackColor       =   &H00000000&
      Caption         =   "Dec"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   165
      TabIndex        =   38
      Top             =   1305
      Width           =   345
   End
   Begin VB.Label txtDec 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
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
      Left            =   555
      TabIndex        =   37
      Top             =   1290
      Width           =   1275
   End
   Begin VB.Label lblRA 
      BackColor       =   &H00000000&
      Caption         =   "RA"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   180
      TabIndex        =   36
      ToolTipText     =   "Click to toggle between RA and HA display"
      Top             =   1005
      Width           =   345
   End
   Begin VB.Label txtRA 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
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
      Left            =   555
      TabIndex        =   35
      ToolTipText     =   "Click to toggle between RA and HA display"
      Top             =   990
      Width           =   1275
   End
End
Attribute VB_Name = "frmHandBox"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' -----------------------------------------------------------------------------
'   ==============
'   FRMHANDBOX.FRM
'   ==============
'
' POTH hand box form
'
' Written: Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 22-Mar-03 jab     Initial edit
' 07-Sep-03 jab     Beta release - much more robust, getting ready for V2
' 10-Sep-03 jab     label the traffic window
' 25-Sep-03 jab     Finished new V2 spec support (V2 definition changed)
' 10-Jan-06 dpp     Focuser implementation
' 31-Mar-06 dpp     Correction in CheckEnable routine.
' 30-Aug-06 jab     further refinement of CheckEnable to catch more cases
' 31-Aug-06 jab     new gui layout
' 10-Sep-06 jab     major changes to handle new focuser gui/functionality
' 11-Sep-06 jab     fixed some double / long conversion issues and an In/Out
'                   stepsize bug
' -----------------------------------------------------------------------------
Option Explicit

Private m_bDomeMode As Boolean
Private m_bFocusMode As Boolean
Private m_bMotionMode As Boolean
Private BtnState As Integer
Private Southern As Boolean
Private Mode As AlignmentModes
Private m_dlastST As Double

' ======
' EVENTS
' ======

Private Sub Form_Load()

'    FloatWindow Me.hwnd, False
    m_bDomeMode = False
    m_bFocusMode = False
    m_bMotionMode = False
    BtnState = 0
    Southern = False
    Mode = algPolar
    m_dlastST = EMPTY_PARAMETER
    
    cbJog.ListIndex = 4
    cbJogDome.ListIndex = 1
    
End Sub

Private Sub Form_Unload(Cancel As Integer)

    On Error Resume Next
    
    ' 1st thing! shut down the timer
    KillTimer 0, g_ltimerID
    
    ' dump the setup dialog
    g_setupDlg.AllowUnload = True
    Unload g_setupDlg
    Set g_setupDlg = Nothing
    
    ' ditch the traffic window
    If Not g_show Is Nothing Then
        Unload g_show
        Set g_show = Nothing
    End If
    
    ' Take care of the HW
    DoShutdown
    
    ' clean out any other objects
    Set g_Profile = Nothing
    Set g_DomeProfile = Nothing
    Set g_FocuserProfile = Nothing
    
    On Error GoTo 0
    
End Sub

Private Sub imgBrewster_Click()

    DisplayWebPage "http://astro.brewsters.net/"
    
End Sub

Private Sub lblRA_Click()

    txtRA_Click
    
End Sub

Private Sub txtRA_Click()

    g_bHAMode = Not g_bHAMode
    CheckHAMode
    
End Sub

Private Sub cmdParkDome_Click()

    If Not g_bDomeConnected Then
        MsgBox "You must connect to the Dome first.", vbExclamation
        Exit Sub
    End If
    
    If Not g_bDomePark Then
        MsgBox "No park support for this dome.", vbExclamation
        Exit Sub
    End If
    
    On Error GoTo Catch
        DomePark
    GoTo Final
Catch:
        MsgBox "Error Parking.", vbExclamation
    Resume Next
Final:
    On Error GoTo 0
    
End Sub

Private Sub chkSlave_Click()
    
    g_bSlaveSlew = False
    g_bSlaved = (chkSlave.Value = 1)
    
    ' set up for checking now
    If g_bSlaved Then _
        slave_time = 0
    
End Sub

Private Sub chkFocuserTempComp_Click()

    g_bFocuserTempComp = IIf(chkFocuserTempComp.Value = 1, True, False)
    'Following test is needed because at startup this routine is called (without user's action)
    ' when setting chkFocuserTempComp.Value = 2
    If g_bFocuserConnected And Not (chkFocuserTempComp.Value = 2) Then
        g_Focuser.TempComp = g_bFocuserTempComp
    End If
    
    CheckFocuserEnable
    
End Sub

Private Sub cmdConnectScope_Click()
   
    g_setupDlg.ConnectScope True
    g_setupDlg.UpdateGlobals
    
    ' these get updated on exiting setup dialog, so they have to
    ' be purposful here
    Me.SouthernHemisphere = (g_dLatitude < 0)
    Me.AlignMode = g_eAlignMode
    
End Sub

Private Sub cmdConnectDome_Click()

    g_setupDlg.ConnectDome True
    g_setupDlg.UpdateDomeGlobals
    
End Sub

Private Sub cmdConnectFocuser_Click()

    g_setupDlg.ConnectFocuser True
    g_setupDlg.UpdateFocuserGlobals
    
End Sub

Private Sub cmdGoto_Click(Index As Integer)
    Dim Az As Double
    
    Az = INVALID_PARAMETER
    On Error Resume Next
    Az = CDbl(txtNewAz.Text)
    On Error GoTo 0
    If Az < -360 Or Az > 360 Then
        MsgBox "Input value must be between" & _
            vbCrLf & "+/- 360", vbExclamation
        Exit Sub
    End If
    
    Az = AzScale(Az)
    
    On Error GoTo ErrorHandler
    
    If Index = 0 Then
        g_Dome.SlewToAzimuth Az
    Else
        g_Dome.SyncToAzimuth Az
    End If
    
    On Error GoTo 0
    Exit Sub
    
ErrorHandler:

    If Index = 0 Then
        MsgBox "Error attempting to slew.", vbExclamation
    Else
        MsgBox "Error attempting to sync.", vbExclamation
    End If
    
    Resume Next
        
End Sub

Private Sub cmdOpenDome_Click()
    Dim curState As ShutterState
    
    If Not g_bDomeConnected Then
        MsgBox "You must connect to the Dome first.", vbExclamation
        Exit Sub
    End If
    
    If Not g_bDomeSetShutter Then
        MsgBox "No shutter support for this dome.", vbExclamation
        Exit Sub
    End If
    
    On Error GoTo ErrorHandler
    
    Select Case g_Dome.ShutterStatus
        Case shutterOpen:
            g_Dome.CloseShutter
        Case shutterClosed:
            g_Dome.OpenShutter
        Case shutterOpening:
            g_Dome.CloseShutter
        Case shutterClosing:
            g_Dome.OpenShutter
        Case shutterError:
            g_Dome.CloseShutter
    End Select
    
    On Error GoTo 0
    Exit Sub
    
ErrorHandler:

    MsgBox "Error controlling shutter.", vbExclamation
    Resume Next
        
End Sub

Private Sub cmdSlew_MouseDown(Index As Integer, button As Integer, Shift As Integer, X As Single, Y As Single)

    ' sometimes if simulating SOP (weak driver), state can be ambiguous or wrong
    ' allow user to force state by using [alt] east or west
    If Shift = 4 Then
        If g_eAlignMode = algGermanPolar Then
            If Index = 4 Then
                g_SOP = pierWest
                LEDPier g_SOP
            ElseIf Index = 3 Then
                g_SOP = pierEast
                LEDPier g_SOP
            End If
        End If
        
        Exit Sub
    End If
    
'    BtnState = index + (Shift * 8) + (button * 64)
    BtnState = Index
    
End Sub

Private Sub cmdSetup_Click()

    DoSetup                         ' May change our topmost state
    
End Sub

Private Sub chkQuiet_Click()
    Dim newVal As Boolean

    If Not g_bConnected Or chkQuiet.Value = 2 Or Not chkQuiet.Enabled Then _
        Exit Sub
        
    newVal = IIf(chkQuiet.Value = 0, False, True)
    
    If newVal <> g_bQuiet Then _
        g_bQuiet = newVal
        
    On Error Resume Next
    ScopeCoords g_bQuiet, False
    On Error GoTo 0

End Sub

Private Sub cmdFlip_Click()
    
    If g_eAlignMode <> algGermanPolar Then
        MsgBox "Flip not supported for this scope.", vbExclamation
        Exit Sub
    End If
    
    If g_bAtPark Then
        MsgBox "Can not flip while parked.", vbExclamation
        Exit Sub
    End If
    
    If Not g_bCanSetPierSide Then
        MsgBox "Commanded flip not supported for this scope.", vbExclamation
        Exit Sub
    End If
    
    On Error GoTo FlipError
    
        If g_SOP = pierEast Then
            ScopeSetSOP pierWest
        ElseIf g_SOP = pierWest Then
            ScopeSetSOP pierEast
        End If
        
    On Error GoTo 0
    
    Exit Sub

FlipError:

    MsgBox "Flip Error.", vbExclamation
    Resume Next
        
End Sub

Private Sub cmdParkScope_Click()
    Dim tmpAtPark As Boolean
    
    If Not g_bConnected Then
        MsgBox "You must connect to the Scope first.", vbExclamation
        Exit Sub
    End If
    
    tmpAtPark = g_bAtPark
    On Error GoTo CatchAtPark
        ScopeTracking trackRead
        ScopeAtPark g_bAtPark, False
    GoTo FinalAtPark
CatchAtPark:
        MsgBox "Error checking park state.", vbExclamation
        Exit Sub
    Resume Next
FinalAtPark:
    On Error GoTo 0
    
    If g_bAtPark <> tmpAtPark Then
        MsgBox "Scope already in desired park state.", vbExclamation
        Exit Sub
    End If
    
    If g_bAtPark Then

        If Not g_bCanUnpark Then
            MsgBox "No Unpark support for this scope.", vbExclamation
            Exit Sub
        End If
        
        On Error GoTo CatchUnpark
            ScopeUnpark
        GoTo FinalUnpark
CatchUnpark:
            MsgBox "Error Unparking. " + Err.Description, vbExclamation
        Resume Next
FinalUnpark:
'            ParkCaption = True
        On Error GoTo 0
        
    Else
        
        If Not g_bCanPark Then
            MsgBox "No park support for this scope.", vbExclamation
            Exit Sub
        End If
        
        On Error GoTo CatchPark
            ScopePark
        GoTo FinalPark
CatchPark:
            MsgBox "Error Parking. " + Err.Description, vbExclamation
        Resume Next
FinalPark:
'            ParkCaption = False
        On Error GoTo 0
        
    End If
    
End Sub

Private Sub chkTracking_Click()
    Dim guiTracking As Boolean

    ' tracking gui not updated when state changes out from under POTH ???
    ' (Through POTH is fine)
    
    If chkTracking.Value = 2 Or Not chkTracking.Enabled Then _
        Exit Sub
        
    If Not g_bConnected Then
        MsgBox "You must connect to the scope first.", vbExclamation
        Exit Sub
    End If
        
    If Not g_bCanSetTracking Then
        MsgBox "No tracking change support for this scope.", vbExclamation
        Exit Sub
    End If
    
    If g_bAtPark Then
        MsgBox "Can not change tracking state while parked.", vbExclamation
        Exit Sub
    End If
    
    ' only change state if we have to
    On Error GoTo Catch
        guiTracking = (chkTracking.Value = 1)
        If guiTracking <> g_bTracking Then
            g_Scope.Tracking = IIf(guiTracking, True, False)
            ScopeTracking trackRead
            ScopeAtPark g_bAtPark, False
            ScopeAtHome g_bAtHome, False
        End If
    GoTo Final
Catch:
        MsgBox "Error checking / changing tracking state.", vbExclamation
    Resume Next
Final:
    On Error GoTo 0
    
End Sub

Private Sub cmdTraffic_Click()

    If g_show Is Nothing Then _
        Set g_show = New frmShow
    
    g_show.Caption = App.EXENAME & " ASCOM Traffic"
    g_show.Show
    
End Sub

Private Sub chkFocuserAbsMove_Click()
    Dim newVal As Boolean
    
    On Error Resume Next
    
    If Not g_bFocuserConnected Or chkFocuserAbsMove.Value = 2 Or Not chkFocuserAbsMove.Enabled Then _
        Exit Sub
    
    newVal = IIf(chkFocuserAbsMove.Value = 0, False, True)
    
    If newVal <> g_bFocuserAbsMove Then
        If newVal Then
            ' moving to absolute mode, save the increment then seed with current position
            ' the next statement could generate nice user error boxes, but as this is a
            ' side effect of changeing abs/rel, just use the old value (no replacement)
            g_lFocuserIncrement = CLng(txtFocuserMove.Text) ' blind convert
            
            txtFocuserMove.Text = IIf(g_bFocuserMoveMicrons And g_dFocuserStepSizeInMicrons > 0, _
                txtFocuserPosition.Caption, txtFocuserSteps.Caption)
        Else
            ' moveing to relative, just put it back
            txtFocuserMove.Text = g_lFocuserIncrement
        End If
        g_bFocuserAbsMove = newVal
    End If
    
    FocuserMoveCaption
    
    On Error GoTo 0
    
End Sub

Private Sub cmdFocuserMove_Click()
    Dim newPos As Long

    ' watch for error since the conversion may fail due to user typing garbage
    On Error Resume Next
    
    newPos = CLng(txtFocuserMove.Text)

    If Err Then
        MsgBox "There must be a correct value in the Move field.", vbExclamation
        txtFocuserMove.SetFocus
        Exit Sub
    End If
    
    If Not g_bFocuserAbsMove Or Not g_bFocuserAbsolute Then
        
        If newPos > 0 Then
            cmdFocuserOut_Click
        Else
            cmdFocuserIn_Click
        End If
    Else
        ' convert user request to steps
        If g_bFocuserMoveMicrons And g_dFocuserStepSizeInMicrons > 0 Then
            newPos = CLng(newPos / g_dFocuserStepSizeInMicrons)     ' CLng rounds
        End If
        
        If newPos < 0 Or newPos > g_lFocuserMaxStep Then
            MsgBox "New position is outside of range.  Aborting.", vbExclamation
            txtFocuserMove.SetFocus
            Exit Sub
        End If
        
        Err.clear
        g_Focuser.Move newPos
        If Err Then _
            MsgBox "Focuser Error - " & Err.Description, vbExclamation
    End If
    
    On Error GoTo 0
    
End Sub

Private Sub cmdFocuserHalt_Click()

    On Error Resume Next
    If g_bFocuserConnected Then _
        g_Focuser.Halt

End Sub

Private Sub cmdFocuserIn_Click()
    Dim inc As Long

    inc = FocuserIncrementTest
    If inc < 0 Then _
        Exit Sub                    ' an error occured
        
    If g_bFocuserConnected Then
        On Error Resume Next        ' error trap for out of range move
        If g_bFocuserAbsolute Then
            g_Focuser.Move g_lFocuserPosition - inc
        Else
            g_Focuser.Move -inc
            If Err = 0 Then _
                g_lFocuserPosition = g_lFocuserPosition - inc
        End If
    End If
    
End Sub

Private Sub cmdFocuserOut_Click()
    Dim inc As Long
    
    inc = FocuserIncrementTest
    If inc < 0 Then _
        Exit Sub                    ' an error occured
    
    If g_bFocuserConnected Then
        On Error Resume Next        ' error trap for out of range move
        If g_bFocuserAbsolute Then
            g_Focuser.Move g_lFocuserPosition + inc
        Else
            g_Focuser.Move inc
            If Err = 0 Then _
                g_lFocuserPosition = g_lFocuserPosition + inc
        End If
    End If
    
End Sub

Private Sub cmdFocuserResetPosition_Click()

    If Not g_bFocuserAbsolute Then _
        g_lFocuserPosition = 0
        
    FocuserDisplay
        
End Sub

' =================
' PUBLIC PROPERTIES
' =================

Public Property Let ParkCaption(val As Boolean)

    If val Then
        cmdParkScope.Caption = "Park Scope"
        shpPark.FillColor = &H0&
    Else
        cmdParkScope.Caption = "Unpark Scope"
        shpPark.FillColor = &HFF00&
    End If
    
    CheckEnable
    
End Property

Public Property Let ParkDomeCaption(val As Boolean)

    ' not used yet ??? synthesize an Unpark ???
    cmdParkDome.Caption = IIf(val, "Park Dome", "Unpark Dome")
    
End Property

Public Property Let RightAscension(RA As Double)
    Dim HA
    
    ' proceed if there is a real value
    If RA >= -24 Then
        ' if we want hour angle instead, and have sidereal time
        If g_bHAMode And m_dlastST > 0 Then

            ' calculate and range HA
            HA = m_dlastST - RA
            
            If HA < -12# Then
                HA = HA + 24#
            End If
            
            If HA > 12 Then
                HA = HA - 24#
            End If
            
            txtRA.Caption = FmtSexa(HA, True)
        Else
            txtRA.Caption = FmtSexa(RA, False)
        End If
    Else
        txtRA.Caption = "--:--:--"
    End If
    
End Property

Public Property Let Declination(Dec As Double)

    If Dec >= -90 Then
        txtDec.Caption = FmtSexa(Dec, True)
    Else
        txtDec.Caption = "--:--:--"
    End If
        
End Property

Public Property Let SiderealTime(ST As Double)

    If ST > 0 Then
        m_dlastST = ST
        txtLST.Caption = FmtSexa(ST, False)
    Else
        txtLST.Caption = "--:--:--"
    End If
    
End Property

Public Property Let AlignMode(m As AlignmentModes)

    Mode = m
    LabelButtons
    
End Property

Public Property Let Altitude(Alt As Double)

    If Alt >= -90 Then
        txtAltitude.Caption = FmtSexa(Alt, False)
    Else
        txtAltitude.Caption = "--:--:--"
    End If
    
End Property

Public Property Let Azimuth(Az As Double)

    If Az >= -360 Then
        txtAzimuth.Caption = FmtSexa(Az, False)
    Else
        txtAzimuth.Caption = "--:--:--"
    End If
    
End Property

Public Property Get ButtonState() As Integer

    ButtonState = BtnState
    BtnState = 0
    
End Property

Public Property Let DomeAzimuth(Az As Double)

    If Az >= -360 Then
        txtDomeAzimuth.Caption = Format(Az, "000.0")
    Else
        txtDomeAzimuth.Caption = "---.-"
    End If
    
End Property

Public Property Let SouthernHemisphere(S As Boolean)

    Southern = S
    LabelButtons
    
End Property

' ===============
' PUBLIC ROUTINES
' ===============

Public Sub FocuserMoveCaption()

    If g_bFocuserConnected Then
        cmdFocuserMove.Caption = IIf(g_bFocuserAbsMove, "Abs Move:", "Rel Move:")
    Else
        cmdFocuserMove.Caption = "Move:"
    End If
    
End Sub

Public Sub CheckEnable()

    Dim ShouldEnable As Boolean
    
    CheckSlave
    
    cmdParkScope.Enabled = (g_bAtPark And g_bCanUnpark) Or _
                           ((Not g_bAtPark) And g_bCanPark)
                           
    ' we will enable motion keys if the scope can slew and at lease one of two other
    ' cases is true:
    '    if parked and can't unpark (give the user some hope)
    '    if not parked (great, most normal state)
    ShouldEnable = g_bCanSlew And _
        ((g_bAtPark And Not g_bCanUnpark) Or (Not g_bAtPark))
        
    cmdSlew(1).Enabled = ShouldEnable
    cmdSlew(2).Enabled = ShouldEnable
    cmdSlew(3).Enabled = ShouldEnable
    cmdSlew(4).Enabled = ShouldEnable
    cmdSlew(5).Enabled = g_bConnected And ShouldEnable
    cbJog.Enabled = ShouldEnable
    cmdFlip.Enabled = g_bCanSetPierSide And ShouldEnable
    
    chkTracking.Enabled = g_bCanSetTracking And Not g_bAtPark
    
    chkQuiet.Enabled = g_bConnected
    chkQuiet.Value = IIf(g_bQuiet, 1, 0)
    
End Sub

Public Sub CheckDomeEnable()

    CheckSlave
    
    cmdGoto(0).Enabled = g_bDomeSetAzimuth
    cmdGoto(1).Enabled = g_bDomeSyncAzimuth
    txtNewAz.Enabled = g_bDomeSetAzimuth Or g_bDomeSyncAzimuth
    
    cmdOpenDome.Enabled = g_bDomeSetShutter
    cmdParkDome.Enabled = g_bDomePark
    
    cmdSlew(6).Enabled = g_bDomeSetAltitude
    cmdSlew(7).Enabled = g_bDomeSetAltitude
    cmdSlew(8).Enabled = g_bDomeSetAzimuth
    cmdSlew(9).Enabled = g_bDomeSetAzimuth
    cmdSlew(10).Enabled = g_bDomeConnected
    cbJogDome.Enabled = g_bDomeSetAltitude Or g_bDomeSetAzimuth
    
End Sub

Public Sub CheckFocuserEnable()

    If Not g_bFocuserConnected Then
        cmdFocuserIn.Enabled = False
        cmdFocuserOut.Enabled = False
        cmdFocuserHalt.Enabled = False
        cmdFocuserMove.Enabled = False
        txtFocuserMove.Enabled = False
        FocuserMoveCaption
        cmdFocuserResetPosition.Enabled = False
        chkFocuserTempComp.Enabled = False
        chkFocuserAbsMove.Enabled = False
        txtFocuserPosition = "------"
        txtFocuserSteps = "------"
        txtFocuserTemperature = "---.-"
        txtFocuserMove.Text = ""
        Exit Sub
    End If
    
    cmdFocuserMove.Enabled = True
    txtFocuserMove.Enabled = True
    
    If g_bFocuserAbsolute Then
        chkFocuserAbsMove.Enabled = True
        chkFocuserAbsMove.Value = IIf(g_bFocuserAbsMove, 1, 0)
        FocuserMoveCaption
        cmdFocuserResetPosition.Enabled = False
    Else
        chkFocuserAbsMove.Enabled = False
        cmdFocuserMove.Caption = "Rel Move:"
        cmdFocuserResetPosition.Enabled = True
    End If

    If Not g_bFocuserTempCompAvailable Then
        chkFocuserTempComp.Enabled = False
        g_bFocuserTempComp = False
        cmdFocuserIn.Enabled = True
        cmdFocuserOut.Enabled = True
        If g_bFocuserHalt Then _
            cmdFocuserHalt.Enabled = True
    Else
        chkFocuserTempComp.Enabled = True
        If g_bFocuserTempComp Then
            chkFocuserTempComp.Value = 1
            cmdFocuserIn.Enabled = False
            cmdFocuserOut.Enabled = False
            cmdFocuserHalt.Enabled = False
        Else
            chkFocuserTempComp.Value = 0
            cmdFocuserIn.Enabled = True
            cmdFocuserOut.Enabled = True
            If g_bFocuserHalt Then _
                cmdFocuserHalt.Enabled = True
        End If
    End If
    
End Sub

Public Sub CheckHAMode()

    lblRA.Caption = IIf(g_bHAMode, "HA", "RA")
    
End Sub

Private Sub CheckSlave()

    If g_bConnected And g_bDomeConnected And g_bDomeSetAzimuth Then
        If Not chkSlave.Enabled Then
            chkSlave.Enabled = True
            chkSlave.Value = 0
        End If
    Else
        chkSlave.Enabled = False
        chkSlave.Value = 2
    End If
        
End Sub

Public Sub CheckWin(force As Boolean)

    If force Then
        WinMode
    ElseIf (m_bDomeMode <> g_bDomeMode) Or _
            (m_bFocusMode <> g_bFocusMode) Or _
            (m_bMotionMode <> g_bMotionControl) Then
        If Visible And WindowState = vbNormal Then _
            WinMode
    End If
    
End Sub

Public Sub ErrorLED()

    ShpError.FillColor = &HFF&
    
End Sub

Public Sub ErrorLEDFocuser(state As Boolean)

    If state Then
        shpFocuserError.FillColor = &HFF&
    Else
        shpFocuserError.FillColor = &H0&
    End If
    
End Sub

Public Sub ErrorLEDDome(state As Boolean)

    If state Then
        shpDomeError.FillColor = &HFF&
    Else
        shpDomeError.FillColor = &H0&
    End If
    
End Sub

Public Sub ErrorLEDScope(state As Boolean)

    If state Then
        shpScopeError.FillColor = &HFF&
    Else
        shpScopeError.FillColor = &H0&
    End If
    
End Sub

Public Sub Quiet()

    If g_bConnected Then
        If ((g_dLongitude < -360) Or (g_dLatitude < -90)) Then
            If chkQuiet.Value = 1 Then
                chkQuiet.Value = 0
            End If
            chkQuiet.Enabled = False
        Else
            chkQuiet.Enabled = True
        End If
    Else
        chkQuiet.Value = 2
        chkQuiet.Enabled = False
    End If
        
    On Error Resume Next
    ScopeCoords g_bQuiet, False
    On Error GoTo 0
    
End Sub

Public Sub CheckShutter()

    On Error GoTo ErrorHandler
    
    If Not g_bDomeConnected Or Not g_bDomeSetShutter Then
        txtShutter.Caption = "---.-"
    Else
        Select Case g_Dome.ShutterStatus
            Case shutterOpen:
                ' could update this smarter ???
                cmdOpenDome.Caption = "Close Dome"
                If g_bDomeSetAltitude Then
                    txtShutter.Caption = Format$(g_Dome.Altitude, "0.0")
                Else
                    txtShutter.Caption = "Open"
                End If
            Case shutterClosed: txtShutter.Caption = "Closed"
                cmdOpenDome.Caption = "Open Dome"
            Case shutterOpening: txtShutter.Caption = "Opening"
                cmdOpenDome.Caption = "Close Dome"
            Case shutterClosing: txtShutter.Caption = "Closing"
                cmdOpenDome.Caption = "Open Dome"
            Case shutterError: txtShutter.Caption = "Unknown"
                cmdOpenDome.Caption = "Close Dome"
        End Select
    End If

    On Error GoTo 0
    Exit Sub

ErrorHandler:

    txtShutter.Caption = "Error"
    cmdOpenDome.Caption = "Close Dome"
    
    On Error GoTo 0
    Exit Sub
       
End Sub

Public Sub Slave()

    If chkSlave.Enabled Then _
        chkSlave.Value = IIf(g_bSlaved, 1, 0)
        
End Sub

Public Sub LEDPier(newVal As PierSide)

    If g_eAlignMode = algGermanPolar Then
        ' Gem, make control visible
        cmdFlip.Visible = True
        
        If newVal = pierUnknown Then
            ' declare a problem
            
            shpEast.FillColor = &HFFFF&
            shpWest.FillColor = &HFFFF&
        Else
            ' light up the right one
            
            If newVal = pierEast Then
                shpEast.FillColor = &HFF00&
            Else
                shpEast.FillColor = &H0&
            End If
            
            If newVal = pierWest Then
                shpWest.FillColor = &HFF00&
            Else
                shpWest.FillColor = &H0&
            End If
        End If
        
    Else
        ' not GEM, make controls, and state go away
        cmdFlip.Visible = False
        shpEast.FillColor = &H0&
        shpWest.FillColor = &H0&
    End If
    
'    ' old version
'    If newVal = pierEast Then
'        shpEast.FillColor = &HFF00&
'    Else
'        shpEast.FillColor = &H0&
'    End If
'
'    If newVal = pierWest Then
'        shpWest.FillColor = &HFF00&
'    Else
'        shpWest.FillColor = &H0&
'    End If
'
'    If newVal = pierUnknown Then
'        cmdFlip.Visible = False
'    Else
'        cmdFlip.Visible = True
'    End If
    
End Sub

Private Function FocuserIncrementTest() As Long
    Dim fetch As Long
    Dim conversion As Long

    ' watch for error since the conversion may fail due to user typing garbage
    ' always return positive, let in/out button determin direction
    ' use -1 to signal failure
    
    FocuserIncrementTest = -1
    On Error Resume Next
    
    If Not g_bFocuserAbsMove Or Not g_bFocuserAbsolute Then
    
        ' we're in relative mode, so start with fetching from the txt field
        fetch = CLng(txtFocuserMove.Text)
    
        If Err Then
            MsgBox "There must be a valid Increment in the Move field.", vbExclamation
            txtFocuserMove.SetFocus
            Exit Function
        End If
        
        ' convert request to steps
        If g_bFocuserMoveMicrons And g_dFocuserStepSizeInMicrons > 0 Then
            conversion = Abs(CLng(fetch / g_dFocuserStepSizeInMicrons))     ' CLng rounds
        Else
            conversion = Abs(fetch)
        End If
            
        If conversion > g_lFocuserMaxIncrement Then
            MsgBox "Increment (" & conversion & " steps) is larger then Max Increment (" & _
                g_lFocuserMaxIncrement & " steps).  Aborted.", vbExclamation
            txtFocuserMove.SetFocus         ' put cursor in the field
            Exit Function
        End If
        
        g_lFocuserIncrement = fetch         ' save in user units with sign
                                            ' only after determining no error
    Else
        ' absolute mode, convert stored value to steps
        
        If g_bFocuserMoveMicrons And g_dFocuserStepSizeInMicrons > 0 Then
            conversion = Abs(CLng(g_lFocuserIncrement / g_dFocuserStepSizeInMicrons))   ' CLng rounds
        Else
            conversion = Abs(g_lFocuserIncrement)
        
            If conversion > g_lFocuserMaxIncrement Then
                MsgBox "Increment (" & conversion & " steps) is larger then Max Increment (" & _
                    g_lFocuserMaxIncrement & " steps).  Aborted.", vbExclamation
                Exit Function
            End If
        End If
    End If
    
    FocuserIncrementTest = Abs(conversion)
    
    On Error GoTo 0
    
End Function

Public Sub FocuserDisplay()
    
    If Not g_bFocuserConnected Then
        txtFocuserPosition = "-----"
        txtFocuserSteps = "-----"
        txtFocuserTemperature = "---.--"
    Else
        txtFocuserPosition = IIf(g_dFocuserStepSizeInMicrons > 0, _
            CLng(g_lFocuserPosition * g_dFocuserStepSizeInMicrons), "------")
            
        txtFocuserSteps = g_lFocuserPosition
            
        txtFocuserTemperature = IIf(g_bFocuserTempProbe, _
            Format(g_dFocuserTemperature, "###.##"), "---.--")
    End If
    
End Sub

Public Sub SetFocusMove()

    If g_bFocuserConnected Then
        ' if we're really in "relative" mode...
        If Not g_bFocuserAbsMove Or Not g_bFocuserAbsolute Then
            g_handBox.txtFocuserMove.Text = g_lFocuserIncrement
        Else
            g_handBox.txtFocuserMove.Text = g_lFocuserPosition
        End If
    Else
        g_handBox.txtFocuserMove.Text = ""
    End If
    
End Sub

Public Sub SaveFocusMove()

    ' watch for error since the conversion may fail due to user typing garbage
    On Error Resume Next
    
    If g_bFocuserConnected Then
        ' if we're really in "relative" mode...
        If Not g_bFocuserAbsMove Or Not g_bFocuserAbsolute Then
            g_lFocuserIncrement = CLng(g_handBox.txtFocuserMove.Text)
        End If
        ' don't save absolute position, on reconnect we set it to actual
    End If
    
    On Error GoTo 0
    
End Sub

' ==============
' LOCAL ROUTINES
' ==============

Private Function LabelButtons()

    If Southern Then
        cmdSlew(1).Caption = "S"
        cmdSlew(2).Caption = "N"
        cmdSlew(3).Caption = "E"
        cmdSlew(4).Caption = "W"
    Else
        cmdSlew(1).Caption = "N"
        cmdSlew(2).Caption = "S"
        cmdSlew(3).Caption = "E"
        cmdSlew(4).Caption = "W"
    End If
    
End Function

' what looks like the use of undeclared variables is really "Me."
Private Sub WinMode()

    m_bDomeMode = g_bDomeMode
    m_bFocusMode = g_bFocusMode
    
    If m_bDomeMode Then
        Width = frmDome.Left + frmDome.Width
        frmDome.Visible = True
        frmFocus.Left = Width
        imgBrewster.Left = (Width - imgBrewster.Width) / 2
    Else
        Width = frmDome.Left      ' minimum width
        frmDome.Visible = False      ' in case focuser needs to be there
        frmFocus.Left = Width     ' in case focuser is not there
        imgBrewster.Left = (Width - imgBrewster.Width) / 2
    End If
    
    If m_bFocusMode Then
        Width = Width + frmFocus.Width    ' open up to see focuser controls
        frmFocus.Visible = True
        imgBrewster.Left = (Width - imgBrewster.Width) / 2
    Else
        frmFocus.Visible = False
    End If
    
    m_bMotionMode = g_bMotionControl
    If m_bMotionMode Then
        Height = 5820
    Else
        Height = 4200
    End If
    
End Sub
