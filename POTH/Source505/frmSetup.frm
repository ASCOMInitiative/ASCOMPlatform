VERSION 5.00
Begin VB.Form frmSetup 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "POTH Setup"
   ClientHeight    =   10275
   ClientLeft      =   5160
   ClientTop       =   1620
   ClientWidth     =   14175
   Icon            =   "frmSetup.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   10230.71
   ScaleMode       =   0  'User
   ScaleWidth      =   14175
   StartUpPosition =   2  'CenterScreen
   Begin VB.CheckBox chkSimple 
      BackColor       =   &H00000000&
      Caption         =   "&Simple Scope"
      ForeColor       =   &H00FFFFFF&
      Height          =   450
      Left            =   240
      TabIndex        =   131
      ToolTipText     =   "Toggle simple mode (do only minimal commands)"
      Top             =   1245
      Visible         =   0   'False
      Width           =   1095
   End
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   1485
      MouseIcon       =   "frmSetup.frx":08CA
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":0A1C
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   115
      TabStop         =   0   'False
      Top             =   1005
      Width           =   720
   End
   Begin VB.CheckBox chkBacklash 
      BackColor       =   &H00000000&
      Caption         =   "&Backlash Removal"
      ForeColor       =   &H00FFFFFF&
      Height          =   450
      Left            =   240
      TabIndex        =   128
      ToolTipText     =   "Toggle backlash removal for short slews"
      Top             =   1770
      Visible         =   0   'False
      Width           =   1095
   End
   Begin VB.CheckBox chkHAMode 
      BackColor       =   &H00000000&
      Caption         =   "Hour &Angle"
      ForeColor       =   &H00FFFFFF&
      Height          =   465
      Left            =   2280
      TabIndex        =   7
      ToolTipText     =   "Display Hour Angle instead of Right Ascension"
      Top             =   2280
      Width           =   735
   End
   Begin VB.Frame frmFocuserStep 
      BackColor       =   &H00000000&
      Caption         =   "Step Control"
      ForeColor       =   &H00FFFFFF&
      Height          =   3139
      Left            =   11070
      TabIndex        =   124
      Top             =   3390
      Width           =   2895
      Begin VB.CheckBox chkFocuserMoveMicrons 
         BackColor       =   &H00000000&
         Caption         =   "Manual moves are in microns.  Higher level SW will still command via steps."
         ForeColor       =   &H00FFFFFF&
         Height          =   585
         Left            =   240
         TabIndex        =   47
         ToolTipText     =   "Command the focuser in Microns for Handset requested moves"
         Top             =   2160
         Width           =   2535
      End
      Begin VB.TextBox txtStepSize 
         Height          =   315
         Left            =   1560
         TabIndex        =   46
         Top             =   1515
         Width           =   1050
      End
      Begin VB.TextBox txtMaxStep 
         Height          =   315
         Left            =   1560
         TabIndex        =   45
         Top             =   915
         Width           =   1050
      End
      Begin VB.TextBox txtMaxIncrement 
         Height          =   315
         Left            =   1560
         TabIndex        =   44
         Top             =   315
         Width           =   1050
      End
      Begin VB.Label lblMaxStep 
         BackColor       =   &H00000000&
         Caption         =   "Max Step:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   1
         Left            =   240
         TabIndex        =   127
         Top             =   960
         Width           =   1170
      End
      Begin VB.Label lblStepSize 
         BackColor       =   &H00000000&
         Caption         =   "Microns / Step:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   1
         Left            =   240
         TabIndex        =   126
         Top             =   1560
         Width           =   1170
      End
      Begin VB.Label lblMaxIncrement 
         BackColor       =   &H00000000&
         Caption         =   "Max Increment:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   1
         Left            =   240
         TabIndex        =   125
         Top             =   360
         Width           =   1170
      End
   End
   Begin VB.CommandButton cmdFocus 
      Caption         =   "&Focus >>"
      Height          =   345
      Left            =   4635
      TabIndex        =   9
      ToolTipText     =   "Toggle focuser control access"
      Top             =   2400
      Width           =   1365
   End
   Begin VB.Frame frmFocuserCap 
      BackColor       =   &H00000000&
      Caption         =   "Focuser Capabilities (output only)"
      ForeColor       =   &H00FFFFFF&
      Height          =   2220
      Left            =   11070
      TabIndex        =   118
      Top             =   6720
      Width           =   2895
      Begin VB.CheckBox chkFocuserAbsolute 
         BackColor       =   &H00000000&
         Caption         =   "Absolute Focuser"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   240
         TabIndex        =   123
         TabStop         =   0   'False
         Top             =   1800
         Width           =   1935
      End
      Begin VB.CheckBox chkFocuserStepSize 
         BackColor       =   &H00000000&
         Caption         =   "Can StepSize"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   240
         TabIndex        =   122
         TabStop         =   0   'False
         Top             =   1440
         Width           =   1455
      End
      Begin VB.CheckBox chkFocuserHalt 
         BackColor       =   &H00000000&
         Caption         =   "Can Halt"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   240
         TabIndex        =   121
         TabStop         =   0   'False
         Top             =   1080
         Width           =   1455
      End
      Begin VB.CheckBox chkFocuserTemperatureCompensate 
         BackColor       =   &H00000000&
         Caption         =   "CanTemp Comp"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   240
         TabIndex        =   120
         TabStop         =   0   'False
         Top             =   720
         Width           =   1455
      End
      Begin VB.CheckBox chkTemperatureProbe 
         BackColor       =   &H00000000&
         Caption         =   "CanTemp Probe"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   240
         TabIndex        =   119
         TabStop         =   0   'False
         Top             =   360
         Width           =   1455
      End
   End
   Begin VB.Frame frmFocuserCon 
      BackColor       =   &H00000000&
      Caption         =   "Focuser Connection"
      ForeColor       =   &H00FFFFFF&
      Height          =   1875
      Left            =   11070
      TabIndex        =   116
      Top             =   240
      Width           =   2895
      Begin VB.CommandButton cmdFocuserSetup 
         Caption         =   "Setup"
         Height          =   375
         Left            =   1080
         TabIndex        =   43
         ToolTipText     =   "Bring up setup dialog for the focuser driver"
         Top             =   1320
         Width           =   1095
      End
      Begin VB.CommandButton cmdFocuserConnect 
         Caption         =   "Connect"
         Height          =   375
         Left            =   1680
         TabIndex        =   42
         ToolTipText     =   "Take control of the focuser"
         Top             =   360
         Width           =   1095
      End
      Begin VB.CommandButton cmdFocuserChooser 
         Caption         =   "Choose Focuser"
         Height          =   375
         Left            =   120
         TabIndex        =   41
         ToolTipText     =   "Bring up list of focusers"
         Top             =   360
         Width           =   1335
      End
      Begin VB.Label txtFocuserChooser 
         Alignment       =   2  'Center
         BackColor       =   &H00000000&
         Caption         =   "(focuser name)"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   120
         TabIndex        =   117
         Top             =   960
         Width           =   2655
      End
   End
   Begin VB.CheckBox chkAutoUnpark 
      BackColor       =   &H00000000&
      Caption         =   "Auto &Track"
      ForeColor       =   &H00FFFFFF&
      Height          =   465
      Left            =   1320
      TabIndex        =   6
      ToolTipText     =   "Control scope state at Connect time, and after Unpark"
      Top             =   2280
      Width           =   735
   End
   Begin VB.CommandButton cmdHelp 
      Caption         =   "&Help"
      Height          =   345
      Left            =   3150
      TabIndex        =   12
      ToolTipText     =   "Get POTH help"
      Top             =   2820
      Width           =   1365
   End
   Begin VB.Frame frmV2 
      BackColor       =   &H00000000&
      Caption         =   "Coordinate Management"
      ForeColor       =   &H00FFFFFF&
      Height          =   2055
      Left            =   180
      TabIndex        =   103
      Top             =   3390
      Width           =   2595
      Begin VB.TextBox txtMeridianDelay 
         Height          =   315
         Left            =   1800
         TabIndex        =   15
         ToolTipText     =   "Extra hours in RA West before flip to East"
         Top             =   480
         Width           =   615
      End
      Begin VB.ComboBox cbRefraction 
         Height          =   315
         ItemData        =   "frmSetup.frx":18E0
         Left            =   1440
         List            =   "frmSetup.frx":18ED
         Style           =   2  'Dropdown List
         TabIndex        =   17
         ToolTipText     =   "Does the scope correct for atmospheric refraction"
         Top             =   1530
         Width           =   795
      End
      Begin VB.ComboBox cbEquSystem 
         Height          =   315
         ItemData        =   "frmSetup.frx":18FF
         Left            =   1320
         List            =   "frmSetup.frx":1912
         Style           =   2  'Dropdown List
         TabIndex        =   16
         ToolTipText     =   "Equatorial System"
         Top             =   1005
         Width           =   915
      End
      Begin VB.TextBox txtMeridianDelayEast 
         Height          =   315
         Left            =   1040
         TabIndex        =   14
         ToolTipText     =   "Extra hours in RA East before flip to West"
         Top             =   480
         Width           =   615
      End
      Begin VB.Label Label7 
         BackColor       =   &H00000000&
         Caption         =   "West"
         ForeColor       =   &H00FFFFFF&
         Height          =   270
         Left            =   1860
         TabIndex        =   130
         Top             =   240
         Width           =   495
      End
      Begin VB.Label Label1 
         BackColor       =   &H00000000&
         Caption         =   "East"
         ForeColor       =   &H00FFFFFF&
         Height          =   270
         Left            =   1100
         TabIndex        =   129
         Top             =   240
         Width           =   495
      End
      Begin VB.Label lblRefraction 
         BackColor       =   &H00000000&
         Caption         =   "Does Refraction:"
         ForeColor       =   &H00FFFFFF&
         Height          =   405
         Left            =   120
         TabIndex        =   106
         Top             =   1485
         Width           =   1095
      End
      Begin VB.Label lblEquSystem 
         BackColor       =   &H00000000&
         Caption         =   "Equatorial System: System:"
         ForeColor       =   &H00FFFFFF&
         Height          =   405
         Left            =   120
         TabIndex        =   105
         Top             =   960
         Width           =   870
      End
      Begin VB.Label lblMeridianDelay 
         BackColor       =   &H00000000&
         Caption         =   "GEM Flip Delay (hrs):"
         ForeColor       =   &H00FFFFFF&
         Height          =   390
         Left            =   120
         TabIndex        =   104
         Top             =   442
         Width           =   855
      End
   End
   Begin VB.Frame frmSlaving 
      BackColor       =   &H00000000&
      Caption         =   "Slave Control"
      ForeColor       =   &H00FFFFFF&
      Height          =   885
      Left            =   6240
      TabIndex        =   94
      Top             =   5640
      Width           =   4635
      Begin VB.TextBox txtFreq 
         Height          =   315
         Left            =   3780
         TabIndex        =   40
         ToolTipText     =   "How often to check for dome sync movement"
         Top             =   360
         Width           =   690
      End
      Begin VB.TextBox txtSlop 
         Height          =   315
         Left            =   1440
         TabIndex        =   39
         ToolTipText     =   "How close to centered does the dome need to be prior to movement"
         Top             =   360
         Width           =   690
      End
      Begin VB.Label lblFreq 
         BackColor       =   &H00000000&
         Caption         =   "Slave Frequency (sec):"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   2400
         TabIndex        =   96
         Top             =   300
         Width           =   1275
      End
      Begin VB.Label lblPrecision 
         BackColor       =   &H00000000&
         Caption         =   "Slave Precision (deg):"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   180
         TabIndex        =   95
         Top             =   300
         Width           =   1335
      End
   End
   Begin VB.Frame frmGeometry 
      BackColor       =   &H00000000&
      Caption         =   "Geometry"
      ForeColor       =   &H00FFFFFF&
      Height          =   2055
      Left            =   6240
      TabIndex        =   88
      Top             =   3390
      Width           =   4635
      Begin VB.TextBox txtGEMOffset 
         Height          =   315
         Left            =   3780
         TabIndex        =   38
         Top             =   840
         Width           =   690
      End
      Begin VB.TextBox txtPosEW 
         Height          =   315
         Left            =   1440
         TabIndex        =   34
         Top             =   360
         Width           =   690
      End
      Begin VB.TextBox txtPosNS 
         Height          =   315
         Left            =   1440
         TabIndex        =   35
         Top             =   840
         Width           =   690
      End
      Begin VB.TextBox txtRadius 
         Height          =   315
         Left            =   3780
         TabIndex        =   37
         Top             =   360
         Width           =   690
      End
      Begin VB.TextBox txtPosUD 
         Height          =   315
         Left            =   1440
         TabIndex        =   36
         Top             =   1320
         Width           =   690
      End
      Begin VB.Label lblGEMOffset 
         BackColor       =   &H00000000&
         Caption         =   "GEM Axis Offset (mm):"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   2400
         TabIndex        =   93
         Top             =   780
         Width           =   1095
      End
      Begin VB.Label lblPosEW 
         BackColor       =   &H00000000&
         Caption         =   "Scope Position +E/-W (mm):"
         ForeColor       =   &H00FFFFFF&
         Height          =   495
         Left            =   180
         TabIndex        =   92
         Top             =   300
         Width           =   1335
      End
      Begin VB.Label lblPosNS 
         BackColor       =   &H00000000&
         Caption         =   "Scope Position +N/-S (mm):"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   180
         TabIndex        =   91
         Top             =   780
         Width           =   1215
      End
      Begin VB.Label lblRadius 
         BackColor       =   &H00000000&
         Caption         =   "Dome Radius (m):"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   2400
         TabIndex        =   90
         Top             =   420
         Width           =   1335
      End
      Begin VB.Label lblPosUD 
         BackColor       =   &H00000000&
         Caption         =   "Scope Position +U/-D (mm):"
         ForeColor       =   &H00FFFFFF&
         Height          =   375
         Left            =   180
         TabIndex        =   89
         Top             =   1260
         Width           =   1095
      End
   End
   Begin VB.CheckBox chkShowControls 
      BackColor       =   &H00000000&
      Caption         =   "&Motion Controls"
      ForeColor       =   &H00FFFFFF&
      Height          =   435
      Left            =   240
      TabIndex        =   5
      ToolTipText     =   "Toggle main window motion controls"
      Top             =   2295
      Width           =   975
   End
   Begin VB.Frame frmDomeCon 
      BackColor       =   &H00000000&
      Caption         =   "Dome Connection"
      ForeColor       =   &H00FFFFFF&
      Height          =   1875
      Left            =   6240
      TabIndex        =   84
      Top             =   240
      Visible         =   0   'False
      Width           =   3255
      Begin VB.CommandButton cmdDomeHome 
         Caption         =   "Home"
         Height          =   375
         Left            =   2385
         TabIndex        =   33
         ToolTipText     =   "Find dome's home position by slewing"
         Top             =   1320
         Width           =   705
      End
      Begin VB.CommandButton cmdDomeSetPark 
         Caption         =   "Set Park"
         Height          =   375
         Left            =   1260
         TabIndex        =   32
         ToolTipText     =   "Set dome's park position to current position"
         Top             =   1320
         Width           =   1005
      End
      Begin VB.CommandButton cmdDomeSetup 
         Caption         =   "Setup"
         Height          =   375
         Left            =   240
         TabIndex        =   31
         ToolTipText     =   "Bring up setup dialog for the dome driver"
         Top             =   1320
         Width           =   855
      End
      Begin VB.CommandButton cmdDomeChooser 
         Caption         =   "Choose Dome"
         Height          =   375
         Left            =   240
         TabIndex        =   29
         ToolTipText     =   "Bring up list of domes"
         Top             =   360
         Width           =   1275
      End
      Begin VB.CommandButton cmdDomeConnect 
         Caption         =   "Connect"
         Height          =   375
         Left            =   2115
         TabIndex        =   30
         ToolTipText     =   "Take control of the dome"
         Top             =   360
         Width           =   975
      End
      Begin VB.Label txtDomeChooser 
         Alignment       =   2  'Center
         BackColor       =   &H00000000&
         Caption         =   "(dome name)"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   240
         TabIndex        =   85
         Top             =   900
         Width           =   2865
      End
   End
   Begin VB.Frame frmDomeCap 
      BackColor       =   &H00000000&
      Caption         =   "Dome Capabilities  (output only)"
      ForeColor       =   &H00FFFFFF&
      Height          =   1620
      Left            =   6240
      TabIndex        =   76
      Top             =   6720
      Width           =   4635
      Begin VB.CheckBox chkDomeFindHome 
         BackColor       =   &H00000000&
         Caption         =   "Can Find Home"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   240
         TabIndex        =   83
         TabStop         =   0   'False
         Top             =   360
         Width           =   1485
      End
      Begin VB.CheckBox chkDomePark 
         BackColor       =   &H00000000&
         Caption         =   "Can Park"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   240
         TabIndex        =   82
         TabStop         =   0   'False
         Top             =   660
         Width           =   1545
      End
      Begin VB.CheckBox chkDomeSetShutter 
         BackColor       =   &H00000000&
         Caption         =   "Can Open/Close Shutter"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   2280
         TabIndex        =   81
         TabStop         =   0   'False
         Top             =   360
         Width           =   2115
      End
      Begin VB.CheckBox chkDomeSyncAzimuth 
         BackColor       =   &H00000000&
         Caption         =   "Can Sync Azimuth"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   2280
         TabIndex        =   80
         TabStop         =   0   'False
         Top             =   960
         Width           =   1695
      End
      Begin VB.CheckBox chkDomeSetAltitude 
         BackColor       =   &H00000000&
         Caption         =   "Can Slew Altitude"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   240
         TabIndex        =   79
         TabStop         =   0   'False
         Top             =   960
         Width           =   1725
      End
      Begin VB.CheckBox chkDomeSetAzimuth 
         BackColor       =   &H00000000&
         Caption         =   "Can Slew Azimuth"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   240
         TabIndex        =   78
         TabStop         =   0   'False
         Top             =   1260
         Width           =   1845
      End
      Begin VB.CheckBox chkDomeSetPark 
         BackColor       =   &H00000000&
         Caption         =   "Can Set Park"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   2280
         TabIndex        =   77
         TabStop         =   0   'False
         Top             =   660
         Width           =   1425
      End
   End
   Begin VB.CommandButton cmdDome 
      Caption         =   "&Dome >>"
      Height          =   345
      Left            =   4635
      TabIndex        =   13
      ToolTipText     =   "Toggle dome control access"
      Top             =   2820
      Width           =   1365
   End
   Begin VB.Frame frmScopeCon 
      BackColor       =   &H00000000&
      Caption         =   "Scope Connection"
      ForeColor       =   &H00FFFFFF&
      Height          =   1875
      Left            =   2565
      TabIndex        =   59
      Top             =   240
      Width           =   3435
      Begin VB.CommandButton cmdHome 
         Caption         =   "Home"
         Height          =   375
         Left            =   2520
         TabIndex        =   4
         ToolTipText     =   "Find scope's home position by slewing"
         Top             =   1320
         Width           =   705
      End
      Begin VB.CommandButton cmdSetPark 
         Caption         =   "Set Park"
         Height          =   375
         Left            =   1305
         TabIndex        =   3
         ToolTipText     =   "Set scope's park position to current position"
         Top             =   1320
         Width           =   1005
      End
      Begin VB.CommandButton cmdConnect 
         Caption         =   "Connect"
         Height          =   375
         Left            =   2250
         TabIndex        =   1
         ToolTipText     =   "Take control of the scope"
         Top             =   360
         Width           =   975
      End
      Begin VB.CommandButton cmdChooser 
         Caption         =   "Choose Scope"
         Height          =   375
         Left            =   240
         TabIndex        =   0
         ToolTipText     =   "Bring up list of scopes"
         Top             =   360
         Width           =   1410
      End
      Begin VB.CommandButton cmdSetup 
         Caption         =   "Setup"
         Height          =   375
         Left            =   240
         TabIndex        =   2
         ToolTipText     =   "Bring up setup dialog for the scope driver"
         Top             =   1320
         Width           =   855
      End
      Begin VB.Shape shpHome 
         BorderColor     =   &H00E0E0E0&
         BorderStyle     =   0  'Transparent
         FillColor       =   &H0000FF00&
         FillStyle       =   0  'Solid
         Height          =   135
         Left            =   3105
         Shape           =   3  'Circle
         Top             =   1125
         Width           =   135
      End
      Begin VB.Label txtChooser 
         Alignment       =   2  'Center
         BackColor       =   &H00000000&
         Caption         =   "(scope name)"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   240
         TabIndex        =   60
         Top             =   900
         Width           =   2955
      End
   End
   Begin VB.Frame frmOptics 
      BackColor       =   &H00000000&
      Caption         =   "Optics"
      ForeColor       =   &H00FFFFFF&
      Height          =   885
      Left            =   180
      TabIndex        =   56
      Top             =   5640
      Width           =   5820
      Begin VB.TextBox txtApertureArea 
         Height          =   315
         Left            =   3000
         TabIndex        =   27
         ToolTipText     =   "Area of the objective"
         Top             =   360
         Width           =   750
      End
      Begin VB.TextBox txtFocalLength 
         Height          =   315
         Left            =   4920
         TabIndex        =   28
         ToolTipText     =   "Focal length of the scope"
         Top             =   360
         Width           =   750
      End
      Begin VB.TextBox txtAperture 
         Height          =   315
         Left            =   1080
         TabIndex        =   26
         ToolTipText     =   "Diameter of the objective"
         Top             =   360
         Width           =   750
      End
      Begin VB.Label lblApertureArea 
         BackColor       =   &H00000000&
         Caption         =   "Aperture Area (m^2):"
         ForeColor       =   &H00FFFFFF&
         Height          =   405
         Index           =   0
         Left            =   2040
         TabIndex        =   111
         Top             =   315
         Width           =   930
      End
      Begin VB.Label Label6 
         BackColor       =   &H00000000&
         Caption         =   "Focal Length (m):"
         ForeColor       =   &H00FFFFFF&
         Height          =   450
         Index           =   0
         Left            =   3960
         TabIndex        =   58
         Top             =   292
         Width           =   885
      End
      Begin VB.Label Label5 
         BackColor       =   &H00000000&
         Caption         =   "Aperture (m):"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   0
         Left            =   120
         TabIndex        =   57
         Top             =   405
         Width           =   930
      End
   End
   Begin VB.CommandButton cmdAdvanced 
      Caption         =   "&Advanced >>"
      Height          =   345
      Left            =   3150
      TabIndex        =   8
      ToolTipText     =   "Toggle access to capabilities review"
      Top             =   2400
      Width           =   1365
   End
   Begin VB.Frame frmScopeCap 
      BackColor       =   &H00000000&
      Caption         =   "Scope Capabilities  (output only)"
      ForeColor       =   &H00FFFFFF&
      Height          =   3300
      Left            =   180
      TabIndex        =   55
      Top             =   6720
      Width           =   5820
      Begin VB.CheckBox chkCanSetDeclinationRate 
         BackColor       =   &H00000000&
         Caption         =   "Can Set Dec Rate"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   1860
         TabIndex        =   113
         TabStop         =   0   'False
         Top             =   1373
         Width           =   1665
      End
      Begin VB.CheckBox chkCanSetRightAscensionRate 
         BackColor       =   &H00000000&
         Caption         =   "Can Set RA Rate"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   1860
         TabIndex        =   112
         TabStop         =   0   'False
         Top             =   2093
         Width           =   1575
      End
      Begin VB.CheckBox chkCanSetGuideRates 
         BackColor       =   &H00000000&
         Caption         =   "Can Guide Rates"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   110
         TabStop         =   0   'False
         Top             =   1380
         Width           =   1665
      End
      Begin VB.CheckBox chkCanEquSystem 
         BackColor       =   &H00000000&
         Caption         =   "Can Equ. System *"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   109
         TabStop         =   0   'False
         Top             =   2100
         Width           =   1665
      End
      Begin VB.CheckBox chkCanSyncAltAz 
         BackColor       =   &H00000000&
         Caption         =   "Can Sync Alt / Az *"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   3540
         TabIndex        =   108
         TabStop         =   0   'False
         Top             =   2100
         Width           =   2040
      End
      Begin VB.CheckBox chkCanDoesRefraction 
         BackColor       =   &H00000000&
         Caption         =   "Can Does Refrac *"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   107
         TabStop         =   0   'False
         Top             =   1140
         Width           =   1665
      End
      Begin VB.CheckBox chkCanSideOfPier 
         BackColor       =   &H00000000&
         Caption         =   "Can Side Of Pier *"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   1860
         TabIndex        =   101
         TabStop         =   0   'False
         Top             =   2603
         Width           =   1665
      End
      Begin VB.CheckBox chkCanSlewAltAz 
         BackColor       =   &H00000000&
         Caption         =   "Can Slew Alt / Az *"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   3540
         TabIndex        =   100
         TabStop         =   0   'False
         Top             =   1140
         Width           =   2085
      End
      Begin VB.CheckBox chkCanSlewAltAzAsync 
         BackColor       =   &H00000000&
         Caption         =   "Can Slew Alt / Az Async *"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   3540
         TabIndex        =   99
         TabStop         =   0   'False
         Top             =   1380
         Width           =   2145
      End
      Begin VB.CheckBox chkCanElevation 
         BackColor       =   &H00000000&
         Caption         =   "Can Elevation *"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   75
         TabStop         =   0   'False
         Top             =   1620
         Width           =   1665
      End
      Begin VB.CheckBox chkCanLatLong 
         BackColor       =   &H00000000&
         Caption         =   "Can Lat / Long *"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   74
         TabStop         =   0   'False
         Top             =   2610
         Width           =   1545
      End
      Begin VB.CheckBox chkCanOptics 
         BackColor       =   &H00000000&
         Caption         =   "Can Optics *"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   1860
         TabIndex        =   73
         TabStop         =   0   'False
         Top             =   653
         Width           =   1425
      End
      Begin VB.CheckBox chkCanSiderealTime 
         BackColor       =   &H00000000&
         Caption         =   "Can Sidereal *"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   3540
         TabIndex        =   72
         TabStop         =   0   'False
         Top             =   653
         Width           =   1545
      End
      Begin VB.CheckBox chkCanDateTime 
         BackColor       =   &H00000000&
         Caption         =   "Can Date / Time *"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   71
         TabStop         =   0   'False
         Top             =   900
         Width           =   1665
      End
      Begin VB.CheckBox chkCanEqu 
         BackColor       =   &H00000000&
         Caption         =   "Can Equatorial"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   70
         TabStop         =   0   'False
         Top             =   1860
         Width           =   1665
      End
      Begin VB.CheckBox chkCanAltAz 
         BackColor       =   &H00000000&
         Caption         =   "Can Alt / Az *"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   69
         TabStop         =   0   'False
         Top             =   660
         Width           =   1665
      End
      Begin VB.CheckBox chkCanFindHome 
         BackColor       =   &H00000000&
         Caption         =   "Can Find Home"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   180
         TabIndex        =   65
         TabStop         =   0   'False
         Top             =   2340
         Width           =   1620
      End
      Begin VB.CheckBox chkCanPark 
         BackColor       =   &H00000000&
         Caption         =   "Can Park"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   1860
         TabIndex        =   64
         TabStop         =   0   'False
         Top             =   893
         Width           =   1665
      End
      Begin VB.CheckBox chkCanPulseGuide 
         BackColor       =   &H00000000&
         Caption         =   "Can Pulse Guide"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   1860
         TabIndex        =   63
         TabStop         =   0   'False
         Top             =   1133
         Width           =   1515
      End
      Begin VB.CheckBox chkCanSetPark 
         BackColor       =   &H00000000&
         Caption         =   "Can Set Park"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   1860
         TabIndex        =   62
         TabStop         =   0   'False
         Top             =   1613
         Width           =   1665
      End
      Begin VB.CheckBox chkCanSetTracking 
         BackColor       =   &H00000000&
         Caption         =   "Can Set Tracking"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   1860
         TabIndex        =   61
         TabStop         =   0   'False
         Top             =   2340
         Width           =   1665
      End
      Begin VB.CheckBox chkCanUnpark 
         BackColor       =   &H00000000&
         Caption         =   "Can Unpark"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   3540
         TabIndex        =   51
         TabStop         =   0   'False
         Top             =   2347
         Width           =   1365
      End
      Begin VB.CheckBox chkCanSync 
         BackColor       =   &H00000000&
         Caption         =   "Can Sync"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   3540
         TabIndex        =   50
         TabStop         =   0   'False
         Top             =   1860
         Width           =   1320
      End
      Begin VB.CheckBox chkCanSlewAsync 
         BackColor       =   &H00000000&
         Caption         =   "Can Slew Async"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   3540
         TabIndex        =   49
         TabStop         =   0   'False
         Top             =   1620
         Width           =   1545
      End
      Begin VB.CheckBox chkCanSlew 
         BackColor       =   &H00000000&
         Caption         =   "Can Slew"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   3540
         TabIndex        =   48
         TabStop         =   0   'False
         Top             =   900
         Width           =   1545
      End
      Begin VB.CheckBox chkCanSetPierSide 
         BackColor       =   &H00000000&
         Caption         =   "Can Set Pier Side *"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   1860
         TabIndex        =   114
         TabStop         =   0   'False
         Top             =   1853
         Width           =   1716
      End
      Begin VB.Label lblAst 
         BackColor       =   &H00000000&
         Caption         =   "* Simulated if needed"
         ForeColor       =   &H00FFFFFF&
         Height          =   228
         Left            =   3912
         TabIndex        =   102
         Top             =   2700
         Width           =   1740
      End
      Begin VB.Label lblVersion 
         BackColor       =   &H00000000&
         Caption         =   "(version #)"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Left            =   4560
         TabIndex        =   98
         Top             =   300
         Width           =   825
      End
      Begin VB.Label lblVersionLabel 
         BackColor       =   &H00000000&
         Caption         =   "Interface Version:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Left            =   3120
         TabIndex        =   97
         Top             =   300
         Width           =   1365
      End
      Begin VB.Label lblMountType 
         BackColor       =   &H00000000&
         Caption         =   "(alignment output)"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Left            =   1260
         TabIndex        =   67
         Top             =   300
         Width           =   1425
      End
      Begin VB.Label lblMountTypeLabel 
         BackColor       =   &H00000000&
         Caption         =   "Mount Type:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Left            =   180
         TabIndex        =   66
         Top             =   300
         Width           =   1065
      End
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "&Cancel"
      Height          =   345
      Left            =   1665
      TabIndex        =   11
      ToolTipText     =   "Cancel this dialog"
      Top             =   2820
      Width           =   1365
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   345
      Left            =   180
      TabIndex        =   10
      ToolTipText     =   "Commit any changes and return to the main window"
      Top             =   2820
      Width           =   1365
   End
   Begin VB.Frame frmSite 
      BackColor       =   &H00000000&
      Caption         =   "Site Information"
      ForeColor       =   &H00FFFFFF&
      Height          =   2055
      Left            =   2925
      TabIndex        =   52
      Top             =   3390
      Width           =   3075
      Begin VB.CommandButton cmdUpdate 
         Caption         =   "&Update Scope (including Time)"
         Height          =   375
         Left            =   180
         TabIndex        =   25
         ToolTipText     =   "Send geographic coordinates and current time to the scope"
         Top             =   1545
         Width           =   2715
      End
      Begin VB.TextBox txtElevation 
         Height          =   315
         Left            =   1260
         TabIndex        =   24
         Text            =   "Text1"
         Top             =   1125
         Width           =   885
      End
      Begin VB.ComboBox cbNS 
         Height          =   315
         ItemData        =   "frmSetup.frx":1939
         Left            =   1020
         List            =   "frmSetup.frx":1946
         Style           =   2  'Dropdown List
         TabIndex        =   18
         Top             =   300
         Width           =   555
      End
      Begin VB.ComboBox cbEW 
         Height          =   315
         ItemData        =   "frmSetup.frx":1953
         Left            =   1020
         List            =   "frmSetup.frx":1960
         Style           =   2  'Dropdown List
         TabIndex        =   21
         Top             =   705
         Width           =   555
      End
      Begin VB.TextBox txtLongMin 
         Height          =   315
         Left            =   2280
         TabIndex        =   23
         Text            =   "Text1"
         Top             =   705
         Width           =   570
      End
      Begin VB.TextBox txtLongDeg 
         Height          =   315
         Left            =   1680
         TabIndex        =   22
         Text            =   "Text1"
         Top             =   705
         Width           =   480
      End
      Begin VB.TextBox txtLatMin 
         Height          =   315
         Left            =   2280
         TabIndex        =   20
         Text            =   "Text1"
         Top             =   300
         Width           =   570
      End
      Begin VB.TextBox txtLatDeg 
         Height          =   315
         Left            =   1680
         TabIndex        =   19
         Text            =   "Text1"
         Top             =   300
         Width           =   480
      End
      Begin VB.Label Label4 
         BackColor       =   &H00000000&
         Caption         =   "Elevation (m):"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   180
         TabIndex        =   68
         Top             =   1185
         Width           =   990
      End
      Begin VB.Label Label3 
         BackColor       =   &H00000000&
         Caption         =   "Latitude:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Left            =   180
         TabIndex        =   54
         Top             =   345
         Width           =   690
      End
      Begin VB.Label Label2 
         BackColor       =   &H00000000&
         Caption         =   "Longitude:"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   180
         TabIndex        =   53
         Top             =   750
         Width           =   765
      End
   End
   Begin VB.Label lblDriverInfo 
      Alignment       =   1  'Right Justify
      BackColor       =   &H00000000&
      Caption         =   "<version, etc.>"
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   6240
      TabIndex        =   87
      Top             =   2595
      Width           =   4575
   End
   Begin VB.Label lblLastModified 
      Alignment       =   1  'Right Justify
      BackColor       =   &H00000000&
      Caption         =   "<last modified>"
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   6240
      TabIndex        =   86
      Top             =   2895
      Width           =   4575
   End
   Begin VB.Image imgBrewster 
      Height          =   555
      Left            =   270
      MouseIcon       =   "frmSetup.frx":196D
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":1ABF
      Top             =   375
      Width           =   1170
   End
End
Attribute VB_Name = "frmSetup"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' -----------------------------------------------------------------------------
'   ============
'   FRMSETUP.FRM
'   ============
'
' POTH setup form
'
' Written: Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 22-Mar-03 jab     Initial edit
' 07-Sep-03 jab     Beta release - much more robust, getting ready for V2
' 25-Sep-03 jab     Finished new V2 spec support (V2 definition changed)
' 10-Jan-06 dpp     Focuser implementation
' 31-Aug-06 jab     new gui layout
' 10-Sep-06 jab     major changes to handle new focuser gui/functionality
' 17-Nov-08 dpp     improve SiteLatitude/Longitude and UTC behaviour
' -----------------------------------------------------------------------------
Option Explicit

Private Enum ConnectState
    conConnected = -1
    conNotConnected = 0
    conConnecting = 2
End Enum

Private m_bAllowUnload As Boolean
Private m_bAdvancedMode As Boolean
Private m_bDomeMode As Boolean
Private m_bFocusMode As Boolean

' ======
' EVENTS
' ======

Private Sub Form_Load()

    Dim fs, F
    Dim DLM As String
     
    FloatWindow Me.hwnd, True                       ' Setup window always floats
    m_bAllowUnload = True                           ' Start out allowing unload
    
    lblDriverInfo = App.FileDescription & " " & _
        App.Major & "." & App.Minor & "." & App.Revision
        
    Set fs = CreateObject("Scripting.FileSystemObject")
    Set F = fs.GetFile(App.Path & "\" & App.EXEName & ".exe")
    DLM = F.DateLastModified
    
    lblLastModified = "Modified " & DLM
    
End Sub

Private Sub Form_Unload(Cancel As Integer)

    Me.Hide
    Form_Clean True, True, True
    Cancel = CInt(Not m_bAllowUnload)
    
End Sub

Private Sub Form_Clean _
        (cleanScope As Boolean, cleanDome As Boolean, cleanFocuser As Boolean)
    
    Dim I As Integer
        
    If cleanScope Then
        Me.Aperture = g_dAperture
        Me.ApertureArea = g_dApertureArea
        Me.FocalLength = g_dFocalLength
        Me.Latitude = g_dLatitude
        Me.Longitude = g_dLongitude
        Me.Elevation = g_dElevation
        txtMeridianDelay.Text = CStr(g_dMeridianDelay)
        txtMeridianDelayEast.Text = CStr(g_dMeridianDelayEast)
        chkAutoUnpark = IIf(g_bAutoUnpark, 1, 0)
        chkBacklash = IIf(g_bBacklash, 1, 0)
        chkSimple = IIf(g_bSimple, 1, 0)
        
        ' set DoesRefraction list item
        For I = cbRefraction.ListCount - 1 To 0 Step -1
            If cbRefraction.ItemData(I) = g_eDoesRefraction Then _
                Exit For
        Next I
        If I < 0 Then _
            I = 0
        cbRefraction.ListIndex = I
        
        ' set Equatorial Coordinates list item
        For I = cbEquSystem.ListCount - 1 To 0 Step -1
            If cbEquSystem.ItemData(I) = g_eEquSystem Then _
                Exit For
        Next I
        If I < 0 Then _
            I = 0
        cbEquSystem.ListIndex = I
    End If
    
    If cleanDome Then
        txtRadius.Text = CStr(g_dRadius)
        txtPosEW.Text = CStr(g_iPosEW)
        txtPosNS.Text = CStr(g_iPosNS)
        txtPosUD.Text = CStr(g_iPosUD)
        txtGEMOffset.Text = CStr(g_iGEMOffset)
        txtSlop.Text = CStr(g_iSlop)
        txtFreq.Text = CStr(slave_time_reset)
    End If
      
    If cleanFocuser Then
        Me.MaxIncrement = g_lFocuserMaxIncrement
        Me.MaxStep = g_lFocuserMaxStep
        Me.StepSize = g_dFocuserStepSizeInMicrons
        chkFocuserMoveMicrons = IIf(g_bFocuserMoveMicrons, 1, 0)
    End If
    
End Sub

Private Sub cmdCancel_Click()
    
    Me.Hide
    Form_Clean True, True, True

End Sub

Private Sub cmdChooser_Click()
    
    Dim newScopeID As String
    Dim g_Chooser As DriverHelper.Chooser
    
    
    Set g_Chooser = New DriverHelper.Chooser
    g_Chooser.DeviceType = "Telescope"
    FloatWindow Me.hwnd, False
    newScopeID = g_Chooser.Choose(g_sScopeID)
    FloatWindow Me.hwnd, True
    Set g_Chooser = Nothing
    
    If newScopeID = "" Then _
        Exit Sub
    If newScopeID = ID Then
        MsgBox "Yeah right...  Try again.", vbExclamation
        Exit Sub
    End If

    If newScopeID <> g_sScopeID Then
        If Not g_Scope Is Nothing Then
            If g_bConnected Then _
                cmdConnect_Click
            
            ScopeDelete
            setDefaults False
        End If
    End If
    
    On Error Resume Next
    If g_Scope Is Nothing Then
        ScopeCreate (newScopeID)
        setDefaults False
    End If
    On Error GoTo 0
    
    If g_Scope Is Nothing Then
        MsgBox "Error opening Scope.", vbExclamation
        Exit Sub
    End If

    g_sScopeID = newScopeID
    
End Sub

Private Sub cmdConnect_Click()

    FloatWindow Me.hwnd, False
    ConnectScope True
    UpdateGlobals
    FloatWindow Me.hwnd, True
 
End Sub

Private Sub cmdFocuserSetup_Click()

    If g_sFocuserID = "" Then
        MsgBox "You must select a Focuser first", vbExclamation
        Exit Sub
    End If
    
    On Error Resume Next
    
    If g_Focuser Is Nothing Then
        FocuserCreate (g_sFocuserID)
        setFocuserDefaults False
    End If
    
    If g_Focuser Is Nothing Then
        MsgBox "Error opening Focuser driver.", vbExclamation
        Exit Sub
    End If
    
    FloatWindow Me.hwnd, False
    g_Focuser.SetupDialog
    FloatWindow Me.hwnd, True
    
    ' refresh state in case Focuser data changed
    If g_bFocuserConnected Then _
        ReadFocuser True
    
    On Error GoTo 0
    
End Sub

Private Sub cmdFocuserChooser_Click()

    Dim newFocuserID As String
    Dim g_FocuserChooser As DriverHelper.Chooser
    
    Set g_FocuserChooser = New DriverHelper.Chooser
    g_FocuserChooser.DeviceType = "Focuser"
    FloatWindow Me.hwnd, False
    newFocuserID = g_FocuserChooser.Choose(g_sFocuserID)
    FloatWindow Me.hwnd, True
    Set g_FocuserChooser = Nothing
    
    If newFocuserID = "" Then _
        Exit Sub
    If newFocuserID = IDFOCUSER Then
        MsgBox "Yeah right...  Try again.", vbExclamation
        Exit Sub
    End If

    If newFocuserID <> g_sFocuserID Then
        If Not g_Focuser Is Nothing Then
            If g_bFocuserConnected Then _
                cmdFocuserConnect_Click
            FocuserDelete
            setFocuserDefaults False
        End If
    End If
    
    On Error Resume Next
        FocuserCreate (newFocuserID)
        setFocuserDefaults False
    On Error GoTo 0
    
    If g_Focuser Is Nothing Then
        MsgBox "Error opening Focuser.", vbExclamation
        Exit Sub
    End If

    g_sFocuserID = newFocuserID
    
End Sub

Private Sub cmdFocuserConnect_Click()

    FloatWindow Me.hwnd, False
    ConnectFocuser True
    UpdateFocuserGlobals
    FloatWindow Me.hwnd, True
    
End Sub

Private Sub cmdDomeChooser_Click()

    Dim newDomeID As String
    Dim g_DomeChooser As DriverHelper.Chooser
    
    Set g_DomeChooser = New DriverHelper.Chooser
    g_DomeChooser.DeviceType = "Dome"
    FloatWindow Me.hwnd, False
    newDomeID = g_DomeChooser.Choose(g_sDomeID)
    FloatWindow Me.hwnd, True
    Set g_DomeChooser = Nothing
    
    If newDomeID = "" Then _
        Exit Sub
    If newDomeID = IDDOME Then
        MsgBox "Yeah right...  Try again.", vbExclamation
        Exit Sub
    End If

    If newDomeID <> g_sDomeID Then
        If Not g_Dome Is Nothing Then
            If g_bDomeConnected Then _
                cmdDomeConnect_Click
            DomeDelete
            setDomeDefaults False
        End If
    End If
    
    On Error Resume Next
        DomeCreate (newDomeID)
        setDomeDefaults False
    On Error GoTo 0
    
    If g_Dome Is Nothing Then
        MsgBox "Error opening Dome.", vbExclamation
        Exit Sub
    End If

    g_sDomeID = newDomeID
        
End Sub

Private Sub cmdDomeSetPark_Click()

    If Not g_bDomeConnected Then
        MsgBox "You must connect to the dome first.", vbExclamation
        Exit Sub
    End If
    
    If Not g_bDomeSetPark Then
        MsgBox "No set park support for this dome.", vbExclamation
        Exit Sub
    End If
    
    On Error GoTo Catch
        g_Dome.SetPark
    GoTo Final
Catch:
        MsgBox "Error setting park position.", vbExclamation
    Resume Next
Final:
    On Error GoTo 0
    
End Sub

Private Sub cmdDomeHome_Click()

    If Not g_bDomeConnected Then
        MsgBox "You must connect to the dome first.", vbExclamation
        Exit Sub
    End If
    
    If Not g_bDomeFindHome Then
        MsgBox "No home support for this dome.", vbExclamation
        Exit Sub
    End If
    
    On Error GoTo Catch
        DomeFindHome
    GoTo Final
Catch:
        MsgBox "Error Homing.", vbExclamation
    Resume Next
Final:
    On Error GoTo 0
    
End Sub

Private Sub cmdDomeSetup_Click()

    If g_sDomeID = "" Then
        MsgBox "You must select a dome first", vbExclamation
        Exit Sub
    End If
    
    On Error Resume Next
    
    If g_Dome Is Nothing Then
        DomeCreate (g_sDomeID)
        setDomeDefaults False
    End If
    
    If g_Dome Is Nothing Then
        MsgBox "Error opening dome driver.", vbExclamation
        Exit Sub
    End If
    
    FloatWindow Me.hwnd, False
    g_Dome.SetupDialog
    FloatWindow Me.hwnd, True
    
    ' refresh state in case dome data changed
    If g_bDomeConnected Then _
        ReadDome True
    
    On Error GoTo 0
    
End Sub

Public Sub ConnectScope(prompt As Boolean)
    
    On Error Resume Next
    
    If g_bConnected Then
        ScopeDelete
        setDefaults False
        ConnectCaption = conNotConnected
        Exit Sub
    End If
    
    If g_Scope Is Nothing Then
        If g_sScopeID = "" Then
            If prompt Then _
                MsgBox "You must select a telescope first.", vbExclamation
            Exit Sub
        End If
        
        ScopeCreate (g_sScopeID)
        
        If g_Scope Is Nothing Then
            If prompt Then _
                MsgBox "Error opening scope driver.", vbExclamation
        End If
    End If
    
    ConnectCaption = conConnecting
    ReadScope prompt
    If Not g_bConnected Then _
        setDefaults False
    
    ConnectCaption = g_bConnected
    
    On Error GoTo 0
    
End Sub

Private Sub cmdDomeConnect_Click()

    FloatWindow Me.hwnd, False
    ConnectDome True
    UpdateDomeGlobals
    FloatWindow Me.hwnd, True
    
End Sub

Public Sub ConnectDome(prompt As Boolean)

    On Error Resume Next
    
    If g_bDomeConnected Then
        DomeDelete
        setDomeDefaults False
        DomeConnectCaption = conNotConnected
        Exit Sub
    End If
    
    If g_Dome Is Nothing Then
        If g_sDomeID = "" Then
            If prompt Then _
                MsgBox "You must select a dome first.", vbExclamation
            Exit Sub
        End If
        
        DomeCreate (g_sDomeID)
       
        If g_Dome Is Nothing Then
            If prompt Then _
                MsgBox "Error opening dome driver.", vbExclamation
        End If
    End If
    
    DomeConnectCaption = conConnecting
    ReadDome prompt
    If Not g_bDomeConnected Then _
        setDomeDefaults False
        
    DomeConnectCaption = g_bDomeConnected
    
    On Error GoTo 0
    
End Sub

Public Sub ConnectFocuser(prompt As Boolean)

    On Error Resume Next
    
    If g_bFocuserConnected Then
        g_handBox.SaveFocusMove
        FocuserDelete
        setFocuserDefaults False
        FocuserConnectCaption = conNotConnected
        Exit Sub
    End If
    
    If g_Focuser Is Nothing Then
        If g_sFocuserID = "" Then
            If prompt Then _
                MsgBox "You must select a Focuser first.", vbExclamation
            Exit Sub
        End If
        
        FocuserCreate (g_sFocuserID)
       
        If g_Focuser Is Nothing Then
            If prompt Then _
                MsgBox "Error opening Focuser driver.", vbExclamation
        End If
    End If
    
    FocuserConnectCaption = conConnecting
    ReadFocuser prompt
    g_handBox.SetFocusMove        ' get the focuser move field filled
    If Not g_bFocuserConnected Then _
        setFocuserDefaults False
        
    FocuserConnectCaption = g_bFocuserConnected
    
    On Error GoTo 0
    
End Sub

Private Sub cmdUpdate_Click()

    Dim CurDate As Double
    Dim UTCDate As Date
    Dim val As Double

    If Not g_bConnected Then
        MsgBox "You must [connect] to the telescope first", vbExclamation
        Exit Sub
    End If
    
    If getLatitude(True, False) = INVALID_PARAMETER Then _
        Exit Sub
    If getLongitude(True, False) = INVALID_PARAMETER Then _
        Exit Sub
    If getElevation(True, False) = INVALID_PARAMETER Then _
        Exit Sub
    On Error Resume Next 'DPP added Nov 2008, if Lat or Long cannot be set we could still set UTCDate
    val = getLatitude(True, True)
    val = getLongitude(True, True)
    val = getElevation(True, True)
    
    If g_bCanDateTime Then
        CurDate = CDbl(Now()) + (CDbl(utc_offs()) / 86400#)
        UTCDate = CDate(CurDate)
        On Error GoTo CatchUTC
            g_Scope.UTCDate = UTCDate
            MsgBox "Date has been updated", vbOKOnly 'DPP added Nov 2008
            If g_bCanSiderealTime Then 'DPP added Nov 2008
                val = g_Scope.SiderealTime
                MsgBox "New difference in Sidereal time is " & Format((val - now_lst(g_dLongitude * DEG_RAD)) * 3600, "##0.0"), vbOKOnly
            End If
        GoTo FinalUTC
CatchUTC:
        MsgBox "Date/Time change error, proceeding.", vbExclamation
        Resume Next
FinalUTC:
        On Error GoTo 0
    
    Else
        MsgBox "Date/Time change not supported by this scope, simulating date/time.", _
            vbExclamation
    End If
    
End Sub

Private Sub cmdSetup_Click()

    If g_sScopeID = "" Then
        MsgBox "You must select a telescope first", vbExclamation
        Exit Sub
    End If
    
    On Error Resume Next
    
    If g_Scope Is Nothing Then
        ScopeCreate (g_sScopeID)
        setDefaults False
    End If
    
    If g_Scope Is Nothing Then
        MsgBox "Error opening scope driver.", vbExclamation
        Exit Sub
    End If
    
    FloatWindow Me.hwnd, False
    g_Scope.SetupDialog
    FloatWindow Me.hwnd, True
    
    ' refresh state in case scope data changed
    If g_bConnected Then _
        ReadScope True
    
    On Error GoTo 0
    
End Sub

Private Sub cmdOK_Click()
    Dim val As Double
    Dim vali As Integer
     
    If getAperture(True, False) = INVALID_PARAMETER Then _
        Exit Sub
    If getApertureArea(True, False) = INVALID_PARAMETER Then _
        Exit Sub
    If getFocalLength(True, False) = INVALID_PARAMETER Then _
        Exit Sub
    If getLatitude(True, False) = INVALID_PARAMETER Then _
        Exit Sub
    If getLongitude(True, False) = INVALID_PARAMETER Then _
        Exit Sub
    If getElevation(True, False) = INVALID_PARAMETER Then _
        Exit Sub
    
    On Error Resume Next
    
    val = INVALID_PARAMETER
    val = CDbl(txtMeridianDelay.Text)
    If val <= -12# Or val >= 12# Then
        txtMeridianDelay.SetFocus
        MsgBox "GEM Flip Delay West is invalid.", vbExclamation
        Exit Sub
    End If
    
    val = INVALID_PARAMETER
    val = CDbl(txtMeridianDelayEast.Text)
    If val <= -12# Or val >= 12# Then
        txtMeridianDelayEast.SetFocus
        MsgBox "GEM Flip Delay East is invalid.", vbExclamation
        Exit Sub
    End If
    
    val = INVALID_PARAMETER
    val = CDbl(txtRadius.Text)
    If val < 0 Then
        txtRadius.SetFocus
        MsgBox "Radius is invalid.", vbExclamation
        Exit Sub
    End If
    
    vali = INVALID_PARAMETER
    vali = CInt(txtPosEW.Text)
    If vali = INVALID_PARAMETER Then
        txtPosEW.SetFocus
        MsgBox "PosEW is invalid.", vbExclamation
        Exit Sub
    End If
    
    vali = INVALID_PARAMETER
    vali = CInt(txtPosNS.Text)
    If vali = INVALID_PARAMETER Then
        txtPosNS.SetFocus
        MsgBox "PosNS is invalid.", vbExclamation
        Exit Sub
    End If
    
    vali = INVALID_PARAMETER
    vali = CInt(txtPosUD.Text)
    If vali = INVALID_PARAMETER Then
        txtPosUD.SetFocus
        MsgBox "PosUD is invalid.", vbExclamation
        Exit Sub
    End If
    
    vali = INVALID_PARAMETER
    vali = CInt(txtGEMOffset.Text)
    If vali = INVALID_PARAMETER Then
        txtGEMOffset.SetFocus
        MsgBox "GEM Offset is invalid.", vbExclamation
        Exit Sub
    End If
    
    vali = INVALID_PARAMETER
    vali = CInt(txtSlop.Text)
    If vali < 0 Then
        txtSlop.SetFocus
        MsgBox "Precision is invalid.", vbExclamation
        Exit Sub
    End If
    
    val = INVALID_PARAMETER
    val = CInt(txtFreq.Text)
    If val < 0 Then
        txtFreq.SetFocus
        MsgBox "Slave Frequency is invalid.", vbExclamation
        Exit Sub
    End If
        
    On Error GoTo 0
    
    If getMaxIncrement(True, False) = INVALID_PARAMETER Then _
        Exit Sub
    If getMaxStep(True, False) = INVALID_PARAMETER Then _
        Exit Sub
    If getStepSize(True, False) = INVALID_PARAMETER Then _
        Exit Sub
        
    UpdateGlobals
    UpdateDomeGlobals
    UpdateFocuserGlobals
    
    ScopeSave
    DomeSave
    FocuserSave
    
    g_bSetupAdvanced = Me.AdvancedMode
    g_bDomeMode = Me.DomeMode
    g_bFocusMode = Me.FocusMode
    g_bMotionControl = Me.MotionControl
    g_bHAMode = Me.HAMode
    
    g_bCanAltAz = (chkCanAltAz = 1)
    g_bCanSiderealTime = (chkCanSiderealTime = 1)
        
    Me.Hide
    
End Sub

Private Sub cmdSetPark_Click()

    If Not g_bConnected Then
        MsgBox "You must connect to the scope first.", vbExclamation
        Exit Sub
    End If
    
    If Not g_bCanSetPark Then
        MsgBox "No set park support for this scope.", vbExclamation
        Exit Sub
    End If
    
    On Error GoTo Catch
        g_Scope.SetPark
    GoTo Final
Catch:
        MsgBox "Error setting park position.", vbExclamation
    Resume Next
Final:
    On Error GoTo 0
    
End Sub

Private Sub cmdAdvanced_Click()

    SetFormMode Not m_bAdvancedMode             ' Toggle form mode
    
End Sub

Private Sub cmdDome_Click()

    SetDeviceMode Not m_bDomeMode, m_bFocusMode    ' Toggle dome mode
    
End Sub

Private Sub cmdFocus_Click()

    SetDeviceMode m_bDomeMode, Not m_bFocusMode    ' Toggle focus mode
    
End Sub

Private Sub cmdHelp_Click()

    DisplayWebPage App.Path & "/POTHHelp.htm"
    
End Sub

Private Sub cmdHome_Click()

    If Not g_bConnected Then
        MsgBox "You must connect to the scope first.", vbExclamation
        Exit Sub
    End If
        
    If Not g_bCanFindHome Then
        MsgBox "No home support for this scope.", vbExclamation
        Exit Sub
    End If
    
    ScopeAtPark g_bAtPark, False
    
    If g_bAtPark Then
        MsgBox "Can not home while parked.", vbExclamation
        Exit Sub
    End If
    
    On Error GoTo Catch
        ScopeHome
    GoTo Final
Catch:
        MsgBox "Error finding home.", vbExclamation
    Resume Next
Final:
    On Error GoTo 0
    
End Sub

Private Sub chkAutoUnpark_Click()

    If chkSimple.Value = 1 And chkAutoUnpark.Value = 1 Then
        chkAutoUnpark = 0
        MsgBox "Auto Track not allowed in Simple Scope mode.", vbExclamation
    End If
    
End Sub

Private Sub chkSimple_Click()

    If chkSimple.Value = 1 And chkAutoUnpark.Value = 1 Then
        chkAutoUnpark = 0
        MsgBox "Disabling Auto Track.  Not allowed in Simple Scope mode.", vbExclamation
    End If
    
End Sub

Private Sub imgBrewster_Click()

    DisplayWebPage "http://astro.brewsters.net/"
    
End Sub

Private Property Let ConnectCaption(val As ConnectState)

    Select Case val
        Case conConnected
            cmdConnect.Caption = "Disconnect"
            g_handBox.cmdConnectScope.Caption = "Disconnect Scope"
        Case conNotConnected
            cmdConnect.Caption = "Connect"
            g_handBox.cmdConnectScope.Caption = "Connect Scope"
        Case conConnecting
            cmdConnect.Caption = "Connecting"
            g_handBox.cmdConnectScope.Caption = "Connecting Scope"
    End Select
    
End Property

Private Property Let DomeConnectCaption(val As ConnectState)
    
    Select Case val
        Case conConnected
            cmdDomeConnect.Caption = "Disconnect"
            g_handBox.cmdConnectDome.Caption = "Disconnect Dome"
        Case conNotConnected
            cmdDomeConnect.Caption = "Connect"
            g_handBox.cmdConnectDome.Caption = "Connect Dome"
        Case conConnecting
            cmdDomeConnect.Caption = "Connecting"
            g_handBox.cmdConnectDome.Caption = "Connecting Dome"
    End Select
    
End Property

Private Property Let FocuserConnectCaption(val As ConnectState)
    
    Select Case val
        Case conConnected
            cmdFocuserConnect.Caption = "Disconnect"
            g_handBox.cmdConnectFocuser.Caption = "Disconnect Focuser"
        Case conNotConnected
            cmdFocuserConnect.Caption = "Connect"
            g_handBox.cmdConnectFocuser.Caption = "Connect Focuser"
        Case conConnecting
            cmdFocuserConnect.Caption = "Connecting"
            g_handBox.cmdConnectFocuser.Caption = "Connecting Focuser"
    End Select
    
End Property

' =================
' PUBLIC PROPERTIES
' =================

Public Property Let Aperture(ByVal newVal As Double)

    If newVal = EMPTY_PARAMETER Then
        txtAperture.Text = "N/A"
    Else
        txtAperture.Text = Format$(newVal, "0.000")
    End If
    
End Property

Private Function getAperture(prompt As Boolean, send As Boolean) As Double

    getAperture = EMPTY_PARAMETER
    
    If txtAperture.Text = "N/A" Or txtAperture.Text = "" Then _
        Exit Function
    
    On Error Resume Next
        getAperture = CDbl(txtAperture.Text)
    On Error GoTo 0
    
    If getAperture <= 0 Then
        getAperture = INVALID_PARAMETER
        If prompt Then
            MsgBox "You must enter a valid number for Aperture", _
                vbExclamation
            txtAperture.SetFocus                ' Put cursor in this box
        End If
        Exit Function
    End If
    
    On Error Resume Next
    
    If send And prompt Then
        If g_bCanOptics Then
            If getAperture <> g_Scope.ApertureDiameter Then _
                MsgBox "Aperture change not supported by ASCOM, simulating the change", _
                    vbExclamation
        Else
            MsgBox "Aperture not supported by this scope, simulating the change", _
                vbExclamation
        End If
    End If
    
    On Error GoTo 0
                        
End Function

Public Property Let ApertureArea(ByVal newVal As Double)

    If newVal = EMPTY_PARAMETER Then
        txtApertureArea.Text = "N/A"
    Else
        txtApertureArea.Text = Format$(newVal, "0.000000")
    End If
    
End Property

Private Function getApertureArea(prompt As Boolean, send As Boolean) As Double

    getApertureArea = EMPTY_PARAMETER
    
    If txtApertureArea.Text = "N/A" Or txtApertureArea.Text = "" Then _
        Exit Function
    
    On Error Resume Next
        getApertureArea = CDbl(txtApertureArea.Text)
    On Error GoTo 0
    
    If getApertureArea <= 0 Then
        getApertureArea = INVALID_PARAMETER
        If prompt Then
            MsgBox "You must enter a valid number for Aperture Area", _
                vbExclamation
            txtApertureArea.SetFocus                ' Put cursor in this box
        End If
        Exit Function
    End If
    
    On Error Resume Next
    
    If send And prompt Then
        If g_bCanOptics Then
            If getApertureArea <> g_Scope.ApertureArea Then _
                MsgBox "Aperture Area change not supported by ASCOM, simulating the change", _
                    vbExclamation
        Else
            MsgBox "Aperture Area not supported by this scope, simulating the change", _
                vbExclamation
        End If
    End If
    
    On Error GoTo 0
                        
End Function

Public Property Let FocalLength(ByVal newVal As Double)

    If newVal = EMPTY_PARAMETER Then
        txtFocalLength.Text = "N/A"
    Else
        txtFocalLength.Text = Format$(newVal, "0.000")
    End If
    
End Property

Private Function getFocalLength(prompt As Boolean, send As Boolean) As Double

    getFocalLength = EMPTY_PARAMETER
    
    If txtFocalLength.Text = "N/A" Or txtFocalLength.Text = "" Then _
        Exit Function
    
    On Error Resume Next
        getFocalLength = CDbl(txtFocalLength.Text)
    On Error GoTo 0
    
    If getFocalLength <= 0 Then
        getFocalLength = INVALID_PARAMETER
        If prompt Then
            MsgBox "You must enter a valid number for Focal Length", _
                vbExclamation
            txtFocalLength.SetFocus             ' Put cursor in this box
        End If
        Exit Function
    End If

    On Error Resume Next
    
    If send And prompt Then
        If g_bCanOptics Then
            If getFocalLength <> g_Scope.FocalLength Then _
                MsgBox "Focal length change not supported by ASCOM, simulating the change", _
                    vbExclamation
        Else
            MsgBox "Focal length not supported by this scope, simulating the change", _
                vbExclamation
        End If
    End If
    
    On Error GoTo 0
            
End Function

Public Property Let Latitude(ByVal newVal As Double)

    Dim u As Integer
    
    If newVal = EMPTY_PARAMETER Then
        cbNS.ListIndex = 2
        txtLatDeg = "N/A"
        txtLatMin = ""
    Else
        If newVal < 0 Then
            newVal = -newVal
            cbNS.ListIndex = 1
        Else
            cbNS.ListIndex = 0
        End If
        u = Fix(newVal)                       ' Degrees
        txtLatDeg = Format$(u, "00")
        newVal = (newVal - u) * 60            ' Minutes
        txtLatMin = Format$(newVal, "00.00")
    End If

End Property
            
Private Function getLatitude(prompt As Boolean, send As Boolean) As Double
    
    getLatitude = EMPTY_PARAMETER
    
    If txtLatDeg = "N/A" Or txtLatDeg = "" Then _
        Exit Function
    
    On Error Resume Next
        getLatitude = CDbl(txtLatDeg) + (CDbl(txtLatMin) / 60#)
        If cbNS.Text = "S" Then _
            getLatitude = -getLatitude
    On Error GoTo 0
          
    If getLatitude > 90 Or getLatitude < -90 Then
        getLatitude = INVALID_PARAMETER
        If prompt Then
            MsgBox "You must enter a valid number for Latitude", _
                vbExclamation
            cbNS.SetFocus                       ' Put cursor in this box
        End If
        Exit Function
    End If

    If send Then
        
        On Error GoTo Catch
            If g_bCanLatLong Then
                If getLatitude <> g_Scope.SiteLatitude Then _
                    g_Scope.SiteLatitude = getLatitude
            ElseIf prompt Then
                MsgBox "Latitude change not supported by this scope, simulating the change", _
                    vbExclamation
            End If
        GoTo Final
Catch:
        If prompt Then _
            MsgBox "Latitude change error, proceeding.", vbExclamation
        Resume Next
Final:
        On Error GoTo 0
        
    End If
      
End Function

Public Property Let Longitude(ByVal newVal As Double)

    Dim u As Integer

    If newVal = EMPTY_PARAMETER Then
        cbEW.ListIndex = 2
        txtLongDeg = "N/A"
        txtLongMin = ""
    Else
        If newVal < 0 Then
            newVal = -newVal
            cbEW.ListIndex = 1
        Else
            cbEW.ListIndex = 0
        End If
        u = Fix(newVal)                       ' Degrees
        txtLongDeg = Format$(u, "000")
        newVal = (newVal - u) * 60            ' Minutes
        txtLongMin = Format$(newVal, "00.00")
    End If
    
End Property

Private Function getLongitude(prompt As Boolean, send As Boolean) As Double

    getLongitude = EMPTY_PARAMETER
    
    If txtLongDeg = "N/A" Or txtLongDeg = "" Then _
        Exit Function
    
    On Error Resume Next
        getLongitude = CDbl(txtLongDeg) + (CDbl(txtLongMin) / 60#)
        If cbEW.Text = "W" Then _
            getLongitude = -getLongitude
    On Error GoTo 0
    
    If getLongitude > 360 Or getLongitude < -180 Then
        getLongitude = INVALID_PARAMETER
        If prompt Then
            MsgBox "You must enter a valid number for Longitude", _
                vbExclamation
            cbEW.SetFocus                       ' Put cursor in this box
        End If
        Exit Function
    End If
   
    If send Then
    
        On Error GoTo Catch
            If g_bCanLatLong Then
                If getLongitude <> g_Scope.SiteLongitude Then _
                    g_Scope.SiteLongitude = getLongitude
            ElseIf prompt Then
                MsgBox "Longitude change not supported by this scope, simulating the change", _
                    vbExclamation
            End If
        GoTo Final
Catch:
        If prompt Then _
            MsgBox "Longitude change error, proceeding.", vbExclamation
        Resume Next
Final:
        On Error GoTo 0
        
    End If
 
End Function

Public Property Let Elevation(ByVal newVal As Double)

    If newVal = EMPTY_PARAMETER Then
        txtElevation.Text = "N/A"
    Else
        txtElevation.Text = Format$(newVal, "0.0")
    End If

End Property

Private Function getElevation(prompt As Boolean, send As Boolean) As Double

    getElevation = EMPTY_PARAMETER
    
    If txtElevation.Text = "N/A" Or txtElevation.Text = "" Then _
        Exit Function
    
    On Error Resume Next
        getElevation = CDbl(txtElevation.Text)
    On Error GoTo 0
    
    If getElevation < -300 Or getElevation > 10000 Then
        getElevation = INVALID_PARAMETER
        If prompt Then
            MsgBox "You must enter a valid number for Elevation", _
                vbExclamation
            txtElevation.SetFocus               ' Put cursor in this box
        End If
        Exit Function
    End If
    
    If send Then
        
        On Error GoTo Catch
            If g_bCanElevation Then
                If getElevation <> g_Scope.SiteElevation Then _
                    g_Scope.SiteElevation = getElevation
            ElseIf prompt Then
                MsgBox "Elevation change not supported by this scope, simulating the change", _
                    vbExclamation
            End If
        GoTo Final
Catch:
        If prompt Then _
            MsgBox "Elevation change error, proceeding.", vbExclamation
        Resume Next
Final:
        On Error GoTo 0
    
    End If
    
End Function

Public Property Let MaxIncrement(ByVal newVal As Long)

    If newVal < 0 Then
        txtMaxIncrement.Text = "N/A"
    Else
        txtMaxIncrement.Text = CStr(newVal)
    End If
    
End Property

Private Function getMaxIncrement(prompt As Boolean, send As Boolean) As Long

    Dim test As Long

    getMaxIncrement = EMPTY_PARAMETER
    
    If txtMaxIncrement.Text = "N/A" Or txtMaxIncrement.Text = "" Then _
        Exit Function
    
    On Error Resume Next
        getMaxIncrement = CLng(txtMaxIncrement.Text)
    On Error GoTo 0
    
    If getMaxIncrement <= 0 Then
        getMaxIncrement = INVALID_PARAMETER
        If prompt Then
            MsgBox "You must enter a valid number for Max Increment", _
                vbExclamation
            txtMaxIncrement.SetFocus                ' Put cursor in this box
        End If
        Exit Function
    End If
    
    On Error Resume Next
    
    If send And prompt Then
        test = INVALID_PARAMETER
        test = g_Focuser.MaxIncrement
        If Not Err Then
            If getMaxIncrement <> test Then _
                MsgBox "Max Increment change not supported by ASCOM, simulating the change", _
                    vbExclamation
        Else
            MsgBox "MaxIncrement not supported by this focuser, simulating the change", _
                vbExclamation
        End If
    End If

    On Error GoTo 0
                        
End Function

Public Property Let MaxStep(ByVal newVal As Long)

    If newVal < 0 Then
        txtMaxStep.Text = "N/A"
    Else
        txtMaxStep.Text = CStr(newVal)
    End If
    
End Property

Private Function getMaxStep(prompt As Boolean, send As Boolean) As Double

    Dim test As Long
    
    getMaxStep = EMPTY_PARAMETER
    
    If txtMaxStep.Text = "N/A" Or txtMaxStep.Text = "" Then _
        Exit Function
    
    On Error Resume Next
        getMaxStep = CLng(txtMaxStep.Text)
    On Error GoTo 0
    
    If getMaxStep <= 0 Then
        getMaxStep = INVALID_PARAMETER
        If prompt Then
            MsgBox "You must enter a valid number for Max Step", _
                vbExclamation
            txtMaxStep.SetFocus                ' Put cursor in this box
        End If
        Exit Function
    End If
    
    On Error Resume Next
    
    If send And prompt Then
        test = INVALID_PARAMETER
        test = g_Focuser.MaxStep
        If Not Err Then
            If getMaxStep <> test Then _
                MsgBox "Max Step change not supported by ASCOM, simulating the change", _
                    vbExclamation
        Else
            MsgBox "MaxStep not supported by this focuser, simulating the change", _
                vbExclamation
        End If
    End If
    
    On Error GoTo 0
                        
End Function

Public Property Let StepSize(ByVal newVal As Double)

    If newVal < 0 Then
        txtStepSize.Text = "N/A"
    Else
        txtStepSize.Text = CStr(newVal)
    End If
    
End Property

Private Function getStepSize(prompt As Boolean, send As Boolean) As Double

    getStepSize = EMPTY_PARAMETER
    
    If txtStepSize.Text = "N/A" Or txtStepSize.Text = "" Then _
        Exit Function
    
    On Error Resume Next
        getStepSize = CDbl(txtStepSize.Text)
    On Error GoTo 0
    
    If getStepSize <= 0 Then
        getStepSize = INVALID_PARAMETER
        If prompt Then
            MsgBox "You must enter a valid number for Step Size", _
                vbExclamation
            txtStepSize.SetFocus                ' Put cursor in this box
        End If
        Exit Function
    End If
    
    On Error Resume Next
    
    If send And prompt Then
        If g_bFocuserStepSize Then
            If getStepSize <> g_Focuser.StepSize Then _
                MsgBox "Step Size change not supported by ASCOM, simulating the change", _
                    vbExclamation
        Else
            MsgBox "Step Size not supported by this focuser, simulating the change", _
                vbExclamation
        End If
    End If
    
    On Error GoTo 0
                        
End Function

Public Property Get AdvancedMode() As Boolean

    AdvancedMode = m_bAdvancedMode
    
End Property

Public Property Let AdvancedMode(B As Boolean)

    SetFormMode B
    
End Property

Public Property Let AllowUnload(B As Boolean)

    m_bAllowUnload = B
    
End Property

Public Property Get DomeMode() As Boolean

    DomeMode = m_bDomeMode
    
End Property

Public Property Let DomeMode(B As Boolean)

    SetDeviceMode B, m_bFocusMode
    
End Property

Public Property Get FocusMode() As Boolean

    FocusMode = m_bFocusMode
    
End Property

Public Property Let FocusMode(B As Boolean)

    SetDeviceMode m_bDomeMode, B
    
End Property

Public Property Get MotionControl() As Boolean

    MotionControl = (chkShowControls = 1)
    
End Property

Public Property Let MotionControl(val As Boolean)

    chkShowControls = IIf(val, 1, 0)
    
End Property

Public Property Get HAMode() As Boolean

    HAMode = (chkHAMode = 1)
    
End Property

Public Property Let HAMode(val As Boolean)

    chkHAMode = IIf(val, 1, 0)
    
End Property

Public Sub LEDHome(state As Boolean)

    If state Then
        shpHome.FillColor = &HFF00&
    Else
        shpHome.FillColor = &H0&
    End If
    
End Sub

Public Sub UpdateGlobals()
    
    On Error Resume Next
    
    g_dAperture = getAperture(False, False)
    g_dApertureArea = getApertureArea(False, False)
    g_dFocalLength = getFocalLength(False, False)
    g_dLatitude = getLatitude(False, False)
    g_dLongitude = getLongitude(False, False)
    g_dElevation = getElevation(False, False)
    g_dMeridianDelay = CDbl(txtMeridianDelay.Text)
    g_dMeridianDelayEast = CDbl(txtMeridianDelayEast.Text)
    g_eDoesRefraction = cbRefraction.ItemData(cbRefraction.ListIndex)
    g_eEquSystem = cbEquSystem.ItemData(cbEquSystem.ListIndex)
    g_bAutoUnpark = IIf(chkAutoUnpark = 1, True, False)
    g_bBacklash = IIf(chkBacklash = 1, True, False)
    g_bSimple = IIf(chkSimple = 1, True, False)
    
    g_bConnected = g_Scope.Connected
    
    ScopeTracking trackInitial
    ScopeCoords True, False
    ScopeAtHome g_bAtHome, False
    ScopeAtPark g_bAtPark, False
    
    g_handBox.Quiet
    g_handBox.CheckEnable
    g_handBox.ParkCaption = Not g_bAtPark
    
    If g_Scope Is Nothing Then _
        Exit Sub
        
    If Not g_bConnected Then _
        Exit Sub
    
    ScopeSlewing slewFetch
    If g_bSlewing Then _
        g_bAsyncSlewing = True
    g_bMonSlewing = True
           
    On Error GoTo 0
    
End Sub

Public Sub UpdateDomeGlobals()
     
    On Error Resume Next
    
    g_dRadius = CDbl(txtRadius.Text)
    g_iPosEW = CInt(txtPosEW.Text)
    g_iPosNS = CInt(txtPosNS.Text)
    g_iPosUD = CInt(txtPosUD.Text)
    g_iGEMOffset = CInt(txtGEMOffset.Text)
    g_iSlop = CInt(txtSlop.Text)
    slave_time_reset = CDbl(txtFreq.Text)
    
    g_bDomeConnected = g_Dome.Connected
    g_handBox.CheckDomeEnable
    
    If g_Dome Is Nothing Then _
        Exit Sub
        
    If Not g_bDomeConnected Then _
        Exit Sub
        
    ' anything to fetch ???
    
    On Error GoTo 0
    
End Sub

Public Sub UpdateFocuserGlobals()
     
    On Error Resume Next
        
    g_lFocuserMaxIncrement = getMaxIncrement(False, False)
    g_lFocuserMaxStep = getMaxStep(False, False)
    g_dFocuserStepSizeInMicrons = getStepSize(False, False)
    g_bFocuserMoveMicrons = IIf(chkFocuserMoveMicrons = 1, True, False)
    
    g_bFocuserConnected = g_Focuser.Connected
    g_handBox.CheckFocuserEnable
    
    If g_Focuser Is Nothing Then _
        Exit Sub
        
    If Not g_bFocuserConnected Then _
        Exit Sub
        
    ' anything to fetch ???
    ' could fetch temp and position?
    
    On Error GoTo 0
    
End Sub

'
' LOCAL UTILITIES
'

Private Sub ReadScope(prompt As Boolean)

    Dim UTCDate As Date
    Dim tmpSiderealTime As Double
    Dim tmpAperture As Double
    Dim tmpApertureArea As Double
    Dim tmpFocalLength As Double
    Dim tmpLongitude As Double
    Dim tmpLatitude As Double
    Dim tmpElevation As Double
    Dim tmpSideOfPier As PierSide
    Dim tmpCompST As Double
    Dim tmpDoesRefraction As Boolean
    Dim tmpEquSystem As EquatorialCoordinateType
    Dim I As Integer
    Dim wasConnected As Boolean
    
    '---------------
    ' get connected
    '---------------
    
    On Error Resume Next
    
    ' do we have a scope?
    If g_Scope Is Nothing Then
        g_bConnected = False
        Exit Sub
    End If
    
    ' keep track of old state, and attempt connection
    If g_bConnected Then
        wasConnected = True
    Else
        g_Scope.Connected = True
        If Err Then 'DPP added Nov 2008
            MsgBox Err.Description, vbExclamation
        End If
        wasConnected = False
    End If
    
    ' retrieve current state
    g_bConnected = g_Scope.Connected
    
    ' verify that we succeeded
    If Not g_bConnected Then
        If prompt Then _
            MsgBox "Error connecting to telescope.", vbExclamation
        Exit Sub
    End If
    
    If chkSimple Then
    
        On Error GoTo 0
        
        '--------------------------------------
        ' do coordinate support checking next,
        ' since there is a bail out condition
        '--------------------------------------
        
        g_bCanAltAz = False
        
        On Error GoTo SimpleCatchCanEqu
        g_bCanEqu = True
        g_dRightAscension = g_Scope.RightAscension
        g_dDeclination = g_Scope.Declination
        GoTo SimpleFinalCanEqu
SimpleCatchCanEqu:
        g_bCanEqu = False
        Resume Next
SimpleFinalCanEqu:
        On Error GoTo 0
        
        If Not g_bCanEqu Then
            If prompt Then
                    MsgBox "Simple scope driver not capable of providing " & _
                        "coordinates, disconnecting", vbExclamation
            End If
            ConnectScope False
            Exit Sub
        End If
          
        '-------------------------
        ' figure out capabilities
        '-------------------------
        
        ' assume API version 1 for simple mode
        On Error Resume Next
        g_iVersion = 1
          
        g_bCanDateTime = False
        g_bCanOptics = False
        g_bCanLatLong = False
        g_bCanElevation = False
        g_bCanAlignMode = False
        g_eAlignMode = ALG_UNKNOWN
        g_bCanSideOfPier = False
        g_bCanFindHome = False
        g_bCanPark = False
        g_bCanPulseGuide = False
        g_bCanSetDeclinationRate = False
        g_bCanSetGuideRates = False
        g_bCanSetPark = False
        g_bCanSetPierSide = False
        g_bCanSetRightAscensionRate = False
        g_bCanSetTracking = False
        g_bCanSlew = g_Scope.CanSlew
        g_bCanSlewAsync = g_Scope.CanSlewAsync
        g_bCanSlewAltAz = False
        g_bCanSlewAltAzAsync = False
        g_bCanSync = False
        g_bCanSyncAltAz = False
        g_bCanUnpark = False
        
        g_bCanDoesRefraction = False
        g_bCanEquSystem = False
        
        tmpLongitude = getLongitude(False, False)
        tmpLatitude = getLatitude(False, False)
        g_bCanSiderealTime = False
        On Error GoTo 0
    
    Else
        
        ' take care of AutoUnpark
        If (Not wasConnected) And (chkAutoUnpark = 1) Then
            If g_Scope.CanUnpark Then _
                g_Scope.Unpark
            If g_Scope.CanSetTracking Then _
                g_Scope.Tracking = True
        End If
        
        On Error GoTo 0
        
        '--------------------------------------
        ' do coordinate support checking next,
        ' since there is a bail out condition
        '--------------------------------------
        
        On Error GoTo CatchCanAltAz
        g_bCanAltAz = True
        g_dAzimuth = g_Scope.Azimuth
        g_dAltitude = g_Scope.Altitude
        GoTo FinalCanAltAz
CatchCanAltAz:
        g_bCanAltAz = False
        Resume Next
FinalCanAltAz:
        On Error GoTo 0
        
        On Error GoTo CatchCanEqu
        g_bCanEqu = True
        g_dRightAscension = g_Scope.RightAscension
        g_dDeclination = g_Scope.Declination
        GoTo FinalCanEqu
CatchCanEqu:
        g_bCanEqu = False
        Resume Next
FinalCanEqu:
        On Error GoTo 0
        
        If (Not g_bCanAltAz) And (Not g_bCanEqu) Then
            If prompt Then
                If g_bAutoUnpark Then
                    MsgBox "Scope driver not capable of providing Equatorial or Alt/Az " & _
                        "coordinates, disconnecting", vbExclamation
                Else
                    MsgBox "Scope driver not capable of providing Equatorial or Alt/Az " & vbCrLf & _
                        "coordinates, disconnecting.  Try turning on AutoTrack.", vbExclamation
                End If
            End If
            ConnectScope False
            Exit Sub
        End If
          
        '-------------------------
        ' figure out capabilities
        '-------------------------
        
        ' assume API version 1 unless g_scope actually works...
        On Error Resume Next
        g_iVersion = 1
        g_iVersion = g_Scope.InterfaceVersion
          
        On Error GoTo CatchCanDateTime
        g_bCanDateTime = True
        UTCDate = g_Scope.UTCDate
        GoTo FinalCanDateTime
CatchCanDateTime:
        g_bCanDateTime = False
        Resume Next
FinalCanDateTime:
        On Error GoTo 0
        
        On Error GoTo CatchOptics
        g_bCanOptics = True
        tmpAperture = g_Scope.ApertureDiameter
        If g_iVersion >= 2 Then _
            tmpApertureArea = g_Scope.ApertureArea
        tmpFocalLength = g_Scope.FocalLength
        GoTo FinalOptics
CatchOptics:
        g_bCanOptics = False
        Resume Next
FinalOptics:
        If g_bCanOptics Then
            g_dAperture = tmpAperture
            If g_iVersion >= 2 Then _
                g_dApertureArea = tmpApertureArea
            g_dFocalLength = tmpFocalLength
            Me.Aperture = tmpAperture
            If g_iVersion >= 2 Then _
                Me.ApertureArea = tmpApertureArea
            Me.FocalLength = tmpFocalLength
        End If
        On Error GoTo 0
        
        On Error GoTo CatchLatLong
        g_bCanLatLong = True
        tmpLatitude = g_Scope.SiteLatitude
        tmpLongitude = g_Scope.SiteLongitude
        GoTo FinalLatLong
CatchLatLong:
        g_bCanLatLong = False
        Resume Next
        
'
        
FinalLatLong:
        If g_bCanLatLong Then
            g_dLatitude = tmpLatitude
            g_dLongitude = tmpLongitude
            Me.Latitude = tmpLatitude
            Me.Longitude = tmpLongitude
        End If
        On Error GoTo 0
            
        On Error GoTo CatchElev
        g_bCanElevation = True
        tmpElevation = g_Scope.SiteElevation
        GoTo FinalElev
CatchElev:
        g_bCanElevation = False
        Resume Next
FinalElev:
        If g_bCanElevation Then
            g_dElevation = tmpElevation
            Me.Elevation = tmpElevation
        End If
        On Error GoTo 0
         
        On Error GoTo CatchAM
        g_bCanAlignMode = True
        g_eAlignMode = g_Scope.AlignmentMode
        GoTo FinalAM
CatchAM:
        g_bCanAlignMode = False
        g_eAlignMode = ALG_UNKNOWN
        Resume Next
FinalAM:
        On Error GoTo 0
        
        On Error GoTo CatchPier
        g_bCanSideOfPier = True
        If g_eAlignMode = algGermanPolar Then
            tmpSideOfPier = g_Scope.SideOfPier     ' don't save it, just checking
        Else
            g_bCanSideOfPier = False
        End If
        GoTo FinalPier
CatchPier:
        g_bCanSideOfPier = False
        Resume Next
FinalPier:
        On Error GoTo 0
            
        On Error Resume Next
            g_bCanFindHome = g_Scope.CanFindHome
            g_bCanPark = g_Scope.CanPark
            g_bCanPulseGuide = g_Scope.CanPulseGuide
            g_bCanSetDeclinationRate = g_Scope.CanSetDeclinationRate
            g_bCanSetGuideRates = g_Scope.CanSetGuideRates
            g_bCanSetPark = g_Scope.CanSetPark
            g_bCanSetPierSide = g_Scope.CanSetPierSide
            g_bCanSetRightAscensionRate = g_Scope.CanSetRightAscensionRate
            g_bCanSetTracking = g_Scope.CanSetTracking
            g_bCanSlew = g_Scope.CanSlew
            g_bCanSlewAsync = g_Scope.CanSlewAsync
            g_bCanSlewAltAz = g_Scope.CanSlewAltAz
            g_bCanSlewAltAzAsync = g_Scope.CanSlewAltAzAsync
            g_bCanSync = g_Scope.CanSync
            g_bCanSyncAltAz = g_Scope.CanSyncAltAz
            g_bCanUnpark = g_Scope.CanUnpark
        On Error GoTo 0
        
        On Error GoTo CatchDoesRefraction
        g_bCanDoesRefraction = True
        If g_iVersion > 1 Then
            tmpDoesRefraction = g_Scope.DoesRefraction
        Else
            g_bCanDoesRefraction = False
        End If
        GoTo FinalDoesRefraction
CatchDoesRefraction:
        g_bCanDoesRefraction = False
        Resume Next
FinalDoesRefraction:
        If g_bCanDoesRefraction Then _
            cbRefraction.ListIndex = IIf(tmpDoesRefraction, refYes, refNo)
        On Error GoTo 0
            
        On Error GoTo CatchEquSystem
        g_bCanEquSystem = True
        If g_iVersion > 1 Then
            tmpEquSystem = g_Scope.EquatorialSystem
        Else
            g_bCanEquSystem = False
        End If
        GoTo FinalEquSystem
CatchEquSystem:
        g_bCanEquSystem = False
        Resume Next
FinalEquSystem:
        If g_bCanEquSystem Then
            ' set Equatorial Coordinates list item
            For I = cbEquSystem.ListCount - 1 To 0 Step -1
                If cbEquSystem.ItemData(I) = tmpEquSystem Then _
                    Exit For
            Next I
            If I < 0 Then _
                I = 0
            cbEquSystem.ListIndex = I
        End If
        On Error GoTo 0
        
        tmpLongitude = getLongitude(False, False)
        tmpLatitude = getLatitude(False, False)
        
        On Error GoTo CatchCanSiderealTime
        g_bCanSiderealTime = True
        tmpSiderealTime = g_Scope.SiderealTime
        GoTo FinalCanSiderealTime
CatchCanSiderealTime:
        g_bCanSiderealTime = False
        Resume Next
FinalCanSiderealTime:
        On Error GoTo 0

    End If
    
    tmpCompST = now_lst(tmpLongitude * DEG_RAD)
    
    If tmpLongitude >= -360 And g_bCanSiderealTime Then
        If Abs((tmpCompST - tmpSiderealTime) * 3600) > 15 Then
            If prompt Then _
                MsgBox "Scope driver Sidereal Time and computer Sidereal Time" & _
                    vbCrLf & "differ by more than 15 seconds. Differ by " & _
                    Format((tmpCompST - tmpSiderealTime) * 3600, "###.0") & " seconds" & vbCrLf & _
                    "Recommend [Update Scope].", vbExclamation
                    'DPP added info on difference Nov 2008
        End If
    End If
    
    If tmpLongitude < -360 Or tmpLatitude < -90 Then
        If prompt Then _
            MsgBox "Will need a valid Latitude and Longitude to" & vbCrLf & _
               "operate Quiet mode.  Disabling Quiet mode.", vbExclamation
    End If
    
    '-----------------------
    ' update all labels for
    ' stuff we just read
    '-----------------------
    
    cmdHome.Enabled = g_bCanFindHome
    cmdSetPark.Enabled = g_bCanSetPark
    cmdUpdate.Enabled = True
    chkSimple.Enabled = False
    
    If g_iVersion > 0 Then
        lblVersion = g_iVersion
    Else
        lblVersion = "(Unknown)"
    End If
    
    If g_bCanAlignMode Then
        If g_eAlignMode = algAltAz Then
            lblMountType = "Alt-Azimuth"
        ElseIf g_eAlignMode = algPolar Then
            lblMountType = "Equatorial"
        Else
            lblMountType = "German Equatorial"
        End If
    Else
        lblMountType = "(Unknown)"
    End If
    
    chkCanAltAz = IIf(g_bCanAltAz, 1, 0)
    chkCanEqu = IIf(g_bCanEqu, 1, 0)
    chkCanEquSystem = IIf(g_bCanEquSystem, 1, 0)
    chkCanDateTime = IIf(g_bCanDateTime, 1, 0)
    chkCanDoesRefraction = IIf(g_bCanDoesRefraction, 1, 0)
    chkCanSiderealTime = IIf(g_bCanSiderealTime, 1, 0)
    
    chkCanOptics = IIf(g_bCanOptics, 1, 0)
    chkCanLatLong = IIf(g_bCanLatLong, 1, 0)
    chkCanElevation = IIf(g_bCanElevation, 1, 0)
    chkCanSideOfPier = IIf(g_bCanSideOfPier, 1, 0)
    
    chkCanFindHome = IIf(g_bCanFindHome, 1, 0)
    chkCanPark = IIf(g_bCanPark, 1, 0)
    chkCanPulseGuide = IIf(g_bCanPulseGuide, 1, 0)
    chkCanSetDeclinationRate = IIf(g_bCanSetDeclinationRate, 1, 0)
    chkCanSetGuideRates = IIf(g_bCanSetGuideRates, 1, 0)
    chkCanSetPark = IIf(g_bCanSetPark, 1, 0)
    chkCanSetPierSide = IIf(g_bCanSetPierSide, 1, 0)
    chkCanSetRightAscensionRate = IIf(g_bCanSetRightAscensionRate, 1, 0)
    chkCanSetTracking = IIf(g_bCanSetTracking, 1, 0)
    chkCanSlew = IIf(g_bCanSlew, 1, 0)
    chkCanSlewAsync = IIf(g_bCanSlewAsync And Not g_bSimple, 1, 0)
    chkCanSlewAltAz = IIf(g_bCanSlewAltAz, 1, 0)
    chkCanSlewAltAzAsync = IIf(g_bCanSlewAltAzAsync, 1, 0)
    chkCanSync = IIf(g_bCanSync, 1, 0)
    chkCanSyncAltAz = IIf(g_bCanSyncAltAz, 1, 0)
    chkCanUnpark = IIf(g_bCanUnpark, 1, 0)
    
    '---------------
    ' get the state
    '---------------
    
    On Error Resume Next
    
    ScopeTracking trackInitial
    
    If Not wasConnected Then
        If Not g_bTracking And g_iVersion < 2 Then
            g_bAtPark = True
        End If
        
        ScopeSOP True
        
        ' real connection requires direct setting since ScopeSOP will check for
        ' difference, and if still unknown, will not light ambiguity state
        g_handBox.LEDPier g_SOP
    End If
    
    ScopeAtHome g_bAtHome, False
    ScopeAtPark g_bAtPark, False
    
    On Error GoTo 0
    
End Sub

Private Sub ReadDome(prompt As Boolean)
    
    g_bDomeConnected = False
    If g_Dome Is Nothing Then _
        Exit Sub
        
    On Error Resume Next
    
    g_Dome.Connected = True
    g_bDomeConnected = g_Dome.Connected
    
    If Not g_bDomeConnected Then
        If prompt Then _
            MsgBox "Error connecting to dome.", vbExclamation
        Exit Sub
    End If
        
    g_bDomeFindHome = g_Dome.CanFindHome
    g_bDomePark = g_Dome.CanPark
    g_bDomeSetAltitude = g_Dome.CanSetAltitude
    g_bDomeSetAzimuth = g_Dome.CanSetAzimuth
    g_bDomeSetPark = g_Dome.CanSetPark
    g_bDomeSetShutter = g_Dome.CanSetShutter
    g_bDomeSyncAzimuth = g_Dome.CanSyncAzimuth
    
    cmdDomeHome.Enabled = g_bDomeFindHome
    cmdDomeSetPark.Enabled = g_bDomeSetPark
    
    chkDomeFindHome = IIf(g_bDomeFindHome, 1, 0)
    chkDomePark = IIf(g_bDomePark, 1, 0)
    chkDomeSetAltitude = IIf(g_bDomeSetAltitude, 1, 0)
    chkDomeSetAzimuth = IIf(g_bDomeSetAzimuth, 1, 0)
    chkDomeSetPark = IIf(g_bDomeSetPark, 1, 0)
    chkDomeSetShutter = IIf(g_bDomeSetShutter, 1, 0)
    chkDomeSyncAzimuth = IIf(g_bDomeSyncAzimuth, 1, 0)
        
    On Error GoTo 0
    
End Sub

Private Sub ReadFocuser(prompt As Boolean)

    g_bFocuserConnected = False
    If g_Focuser Is Nothing Then _
        Exit Sub
        
    On Error Resume Next
    
    g_Focuser.Link = True 'Connected is replaced by Link for Focuser
    g_bFocuserConnected = g_Focuser.Link
    
    If Not g_bFocuserConnected Then
        If prompt Then _
            MsgBox "Error connecting to Focuser.", vbExclamation
        Exit Sub
    End If
        
    g_bFocuserTempCompAvailable = g_Focuser.TempCompAvailable
    
    g_dFocuserTemperature = g_Focuser.Temperature
    If Err Then
        g_bFocuserTempProbe = False
    Else
         g_bFocuserTempProbe = True
    End If
    
    g_lFocuserMaxIncrement = g_Focuser.MaxIncrement
    Me.MaxIncrement = g_lFocuserMaxIncrement
    g_lFocuserMaxStep = g_Focuser.MaxStep
    Me.MaxStep = g_lFocuserMaxStep
    
    g_bFocuserAbsolute = g_Focuser.Absolute
    chkFocuserAbsolute = IIf(g_bFocuserAbsolute, 1, 0)
    
    chkFocuserTemperatureCompensate = IIf(g_bFocuserTempCompAvailable, 1, 0)
    chkTemperatureProbe = IIf(g_bFocuserTempProbe, 1, 0)
    
    Err.clear
    g_Focuser.Halt
    If Err Then
        g_bFocuserHalt = False
        chkFocuserHalt = 0
    Else
        g_bFocuserHalt = True
        chkFocuserHalt = 1
    End If
    
    Err.clear
    g_dFocuserStepSizeInMicrons = g_Focuser.StepSize
    If Err Then
        g_bFocuserStepSize = False
        chkFocuserStepSize = 0
    Else
        Me.StepSize = g_dFocuserStepSizeInMicrons
        g_bFocuserStepSize = True
        chkFocuserStepSize = 1
    End If
    
    ' fetch position to seed focuser move field later
    If g_bFocuserAbsolute Then _
        g_lFocuserPosition = g_Focuser.Position
            
    On Error GoTo 0
    
End Sub

' seed the form with persisted data
Public Sub setDefaults(clear As Boolean)
        
    ScopeClean clear
    
    If clear Then _
        Form_Clean True, False, False
    
    cmdHome.Enabled = False
    cmdSetPark.Enabled = False
    cmdUpdate.Enabled = False
    chkSimple.Enabled = True
    
    chkCanAltAz = 2
    chkCanEqu = 2
    chkCanEquSystem = 2
    chkCanDateTime = 2
    chkCanDoesRefraction = 2
    chkCanSiderealTime = 2
    
    chkCanOptics = 2
    chkCanLatLong = 2
    chkCanElevation = 2
    chkCanSideOfPier = 2
    
    chkCanFindHome = 2
    chkCanPark = 2
    chkCanPulseGuide = 2
    chkCanSetDeclinationRate = 2
    chkCanSetGuideRates = 2
    chkCanSetPark = 2
    chkCanSetPierSide = 2
    chkCanSetRightAscensionRate = 2
    chkCanSetTracking = 2
    chkCanSlew = 2
    chkCanSlewAsync = 2
    chkCanSlewAltAz = 2
    chkCanSlewAltAzAsync = 2
    chkCanSync = 2
    chkCanSyncAltAz = 2
    chkCanUnpark = 2
    lblVersion = "(Unknown)"
    lblMountType = "(Unknown)"
    
    txtChooser.Caption = g_sScopeName
    
    ScopeSlewing slewInit
    ScopeTracking trackClear
    g_handBox.LEDPier g_SOP
    g_handBox.ParkCaption = Not g_bAtPark
    LEDHome g_bAtHome
    
    g_handBox.CheckEnable
    
End Sub

Public Sub setDomeDefaults(clear As Boolean)

    DomeClean clear
    
    If clear Then _
        Form_Clean False, True, False
    
    cmdDomeHome.Enabled = False
    cmdDomeSetPark.Enabled = False
    
    chkDomeFindHome = 2
    chkDomePark = 2
    chkDomeSetAltitude = 2
    chkDomeSetAzimuth = 2
    chkDomeSetShutter = 2
    chkDomeSetPark = 2
    chkDomeSyncAzimuth = 2
    
    txtDomeChooser.Caption = g_sDomeName
    
    g_handBox.CheckDomeEnable
    
End Sub

Public Sub setFocuserDefaults(clear As Boolean)

    FocuserClean clear
    
    If clear Then _
        Form_Clean False, False, True
    
    chkTemperatureProbe = 2
    chkFocuserTemperatureCompensate = 2
    chkFocuserHalt = 2
    chkFocuserStepSize = 2
    chkFocuserAbsolute = 2
    
    txtFocuserChooser.Caption = g_sFocuserName
'    g_handBox.chkFocuserTempComp = 2
'    g_handBox.chkFocuserAbsMove = 2
'    g_handBox.cmdFocuserMove.Caption = "Move"
'    g_handBox.txtFocuserMove.Text = ""
'    g_handBox.txtFocuserMove.Text = g_lFocuserIncrement
'    g_handBox.txtFocuserPosition = "------"
'    g_handBox.txtFocuserSteps = "------"
'    g_handBox.txtFocuserTemperature = "---.-"
    g_handBox.CheckFocuserEnable
    
End Sub

Private Sub SetDeviceMode(DomeMode As Boolean, FocusMode As Boolean)
    
    If DomeMode Then
        Me.frmDomeCon.Visible = True
        Me.frmGeometry.Visible = True
        Me.frmSlaving.Visible = True
        Me.frmDomeCap.Visible = True
        Me.lblDriverInfo.Visible = True
        Me.lblLastModified.Visible = True
        Me.Width = 11100
        Me.cmdDome.Caption = "<< No &Dome"
        
        Me.frmFocuserCon.Left = 11070
        Me.frmFocuserStep.Left = 11070
        Me.frmFocuserCap.Left = 11070
    Else
        Me.frmDomeCon.Visible = False
        Me.frmGeometry.Visible = False
        Me.frmSlaving.Visible = False
        Me.frmDomeCap.Visible = False
        Me.lblDriverInfo.Visible = False
        Me.lblLastModified.Visible = False
        Me.Width = 6250
        Me.cmdDome.Caption = "&Dome >>"
        
        Me.frmFocuserCon.Left = 6240
        Me.frmFocuserStep.Left = 6240
        Me.frmFocuserCap.Left = 6240
    End If
    
    If FocusMode Then
        Me.frmFocuserCon.Visible = True
        Me.frmFocuserStep.Visible = True
        Me.frmFocuserCap.Visible = True
        If DomeMode Then
            Me.Width = Me.Width + 3100
        Else
            Me.Width = Me.Width + 3150
        End If
        Me.cmdFocus.Caption = "<< No &Focus"
    Else
        Me.frmFocuserCon.Visible = False
        Me.frmFocuserStep.Visible = False
        Me.frmFocuserCap.Visible = False
        Me.cmdFocus.Caption = "&Focus >>"
    End If
    
    m_bDomeMode = DomeMode
    m_bFocusMode = FocusMode
    
End Sub

Private Sub SetFormMode(Advanced As Boolean)

    If Advanced Then
        Me.Height = 10350
        Me.cmdAdvanced.Caption = "<< B&asic"
        
        ' make sure tab stops are enabled
        Me.frmV2.Enabled = True
        Me.frmSite.Enabled = True
        Me.frmOptics.Enabled = True
        Me.frmV2.Enabled = True
        Me.frmFocuserStep.Enabled = True
        Me.frmGeometry.Enabled = True
        Me.frmSlaving.Enabled = True
    Else
        Me.Height = 3675
        Me.cmdAdvanced.Caption = "&Advanced >>"
        
        ' make sure tab stops are disabled
        Me.frmV2.Enabled = False
        Me.frmSite.Enabled = False
        Me.frmOptics.Enabled = False
        Me.frmV2.Enabled = False
        Me.frmFocuserStep.Enabled = False
        Me.frmGeometry.Enabled = False
        Me.frmSlaving.Enabled = False
    End If
    
    m_bAdvancedMode = Advanced
    
End Sub

Private Sub picASCOM_Click()

    DisplayWebPage "http://ascom-standards.org/"
    
End Sub
