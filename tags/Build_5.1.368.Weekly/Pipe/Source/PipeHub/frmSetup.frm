VERSION 5.00
Begin VB.Form frmSetup 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Pipe Setup"
   ClientHeight    =   9090
   ClientLeft      =   5160
   ClientTop       =   1620
   ClientWidth     =   14400
   Icon            =   "frmSetup.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   9050.815
   ScaleMode       =   0  'User
   ScaleWidth      =   14400
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame frmFocuserStep 
      BackColor       =   &H00000000&
      Caption         =   "Step Control"
      ForeColor       =   &H00FFFFFF&
      Height          =   1335
      Left            =   11070
      TabIndex        =   89
      Top             =   3090
      Width           =   2895
      Begin VB.Label txtMaxIncrement 
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
         Left            =   1560
         TabIndex        =   95
         Top             =   360
         Width           =   1155
      End
      Begin VB.Label lblMaxIncrement 
         BackColor       =   &H00000000&
         Caption         =   "Max Increment:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   1
         Left            =   240
         TabIndex        =   94
         Top             =   360
         Width           =   1170
      End
      Begin VB.Label lblStepSize 
         BackColor       =   &H00000000&
         Caption         =   "Microns / Step:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   1
         Left            =   240
         TabIndex        =   93
         Top             =   960
         Width           =   1170
      End
      Begin VB.Label lblMaxStep 
         BackColor       =   &H00000000&
         Caption         =   "Max Step:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   1
         Left            =   240
         TabIndex        =   92
         Top             =   660
         Width           =   1170
      End
      Begin VB.Label txtStepSize 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00000000&
         Caption         =   "---"
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
         Left            =   1560
         TabIndex        =   91
         Top             =   960
         Width           =   1155
      End
      Begin VB.Label txtMaxStep 
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
         Left            =   1560
         TabIndex        =   90
         Top             =   660
         Width           =   1155
      End
   End
   Begin VB.Frame Frame1 
      BackColor       =   &H00000000&
      Caption         =   "Site Information"
      ForeColor       =   &H00FFFFFF&
      Height          =   1335
      Left            =   3000
      TabIndex        =   82
      Top             =   3090
      Width           =   2955
      Begin VB.Label txtLongitude 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00000000&
         Caption         =   "--- --.-- -"
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
         Left            =   1140
         TabIndex        =   88
         Top             =   660
         Width           =   1575
      End
      Begin VB.Label txtlatitude 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00000000&
         Caption         =   "-- --.-- -"
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
         Left            =   1140
         TabIndex        =   87
         Top             =   360
         Width           =   1575
      End
      Begin VB.Label txtElevation 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00000000&
         Caption         =   "----.-"
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
         Left            =   1560
         TabIndex        =   86
         Top             =   960
         Width           =   1155
      End
      Begin VB.Label Label4 
         BackColor       =   &H00000000&
         Caption         =   "Elevation (m):"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   240
         TabIndex        =   85
         Top             =   960
         Width           =   990
      End
      Begin VB.Label Label3 
         BackColor       =   &H00000000&
         Caption         =   "Latitude:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Left            =   240
         TabIndex        =   84
         Top             =   360
         Width           =   690
      End
      Begin VB.Label Label2 
         BackColor       =   &H00000000&
         Caption         =   "Longitude:"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   240
         TabIndex        =   83
         Top             =   675
         Width           =   765
      End
   End
   Begin VB.Frame Frame4 
      BackColor       =   &H00000000&
      Caption         =   "Optics"
      ForeColor       =   &H00FFFFFF&
      Height          =   1335
      Left            =   180
      TabIndex        =   75
      Top             =   3090
      Width           =   2595
      Begin VB.Label txtAperture 
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
         Left            =   1440
         TabIndex        =   81
         Top             =   360
         Width           =   915
      End
      Begin VB.Label txtApertureArea 
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
         Left            =   1440
         TabIndex        =   80
         Top             =   660
         Width           =   915
      End
      Begin VB.Label txtFocalLength 
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
         Left            =   1440
         TabIndex        =   79
         Top             =   960
         Width           =   915
      End
      Begin VB.Label lblApertureArea 
         BackColor       =   &H00000000&
         Caption         =   "Ap, Area (m^2):"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Index           =   0
         Left            =   120
         TabIndex        =   78
         Top             =   630
         Width           =   1530
      End
      Begin VB.Label Label6 
         BackColor       =   &H00000000&
         Caption         =   "Focal Length (m):"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Index           =   0
         Left            =   120
         TabIndex        =   77
         Top             =   960
         Width           =   1365
      End
      Begin VB.Label Label5 
         BackColor       =   &H00000000&
         Caption         =   "Aperture (m):"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   0
         Left            =   120
         TabIndex        =   76
         Top             =   360
         Width           =   930
      End
   End
   Begin VB.CommandButton cmdFocus 
      Caption         =   "&Focus >>"
      Height          =   345
      Left            =   4680
      TabIndex        =   4
      Top             =   2160
      Width           =   1245
   End
   Begin VB.Frame frmFocuserCap 
      BackColor       =   &H00000000&
      Caption         =   "Focuser Capabilities (output only)"
      ForeColor       =   &H00FFFFFF&
      Height          =   3060
      Left            =   11070
      TabIndex        =   66
      Top             =   5520
      Width           =   2895
      Begin VB.CheckBox chkFocuserAbsolute 
         BackColor       =   &H00000000&
         Caption         =   "Absolute / Relative"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   240
         TabIndex        =   71
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
         TabIndex        =   70
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
         TabIndex        =   69
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
         TabIndex        =   68
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
         TabIndex        =   67
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
      TabIndex        =   64
      Top             =   120
      Width           =   2895
      Begin VB.CommandButton cmdFocuserSetup 
         Caption         =   "Setup"
         Height          =   375
         Left            =   120
         TabIndex        =   14
         Top             =   1320
         Width           =   1095
      End
      Begin VB.CommandButton cmdFocuserConnect 
         Caption         =   "Connect"
         Height          =   375
         Left            =   1680
         TabIndex        =   13
         Top             =   360
         Width           =   1095
      End
      Begin VB.CommandButton cmdFocuserChooser 
         Caption         =   "Choose Focuser"
         Height          =   375
         Left            =   120
         TabIndex        =   12
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
         TabIndex        =   65
         Top             =   960
         Width           =   2655
      End
   End
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   960
      MouseIcon       =   "frmSetup.frx":08CA
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":0A1C
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   63
      TabStop         =   0   'False
      Top             =   960
      Width           =   720
   End
   Begin VB.CommandButton cmdHelp 
      Caption         =   "&Help"
      Height          =   345
      Left            =   3200
      TabIndex        =   3
      Top             =   2580
      Width           =   1245
   End
   Begin VB.Frame frmV2 
      BackColor       =   &H00000000&
      Caption         =   "Coordinate Management"
      ForeColor       =   &H00FFFFFF&
      Height          =   735
      Left            =   180
      TabIndex        =   53
      Top             =   4560
      Width           =   5805
      Begin VB.Label txtRefraction 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00000000&
         Caption         =   "---"
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
         Left            =   5040
         TabIndex        =   74
         Top             =   375
         Width           =   555
      End
      Begin VB.Label txtEquSystem 
         Alignment       =   1  'Right Justify
         Appearance      =   0  'Flat
         BackColor       =   &H00000000&
         Caption         =   "-----"
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
         Left            =   2400
         TabIndex        =   73
         Top             =   360
         Width           =   795
      End
      Begin VB.Label lblRefraction 
         BackColor       =   &H00000000&
         Caption         =   "Does Refraction:"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Left            =   3720
         TabIndex        =   55
         Top             =   360
         Width           =   1335
      End
      Begin VB.Label lblEquSystem 
         BackColor       =   &H00000000&
         Caption         =   "Equatorial System:"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Left            =   960
         TabIndex        =   54
         Top             =   360
         Width           =   1455
      End
   End
   Begin VB.Frame frmDomeCon 
      BackColor       =   &H00000000&
      Caption         =   "Dome Connection"
      ForeColor       =   &H00FFFFFF&
      Height          =   1875
      Left            =   6240
      TabIndex        =   15
      Top             =   120
      Visible         =   0   'False
      Width           =   4575
      Begin VB.CommandButton cmdDomeSetup 
         Caption         =   "Setup"
         Height          =   375
         Left            =   240
         TabIndex        =   11
         Top             =   1320
         Width           =   855
      End
      Begin VB.CommandButton cmdDomeChooser 
         Caption         =   "Choose Dome"
         Height          =   375
         Left            =   240
         TabIndex        =   9
         Top             =   360
         Width           =   1275
      End
      Begin VB.CommandButton cmdDomeConnect 
         Caption         =   "Connect"
         Height          =   375
         Left            =   3360
         TabIndex        =   10
         Top             =   360
         Width           =   975
      End
      Begin VB.Label txtDomeChooser 
         Alignment       =   2  'Center
         BackColor       =   &H00000000&
         Caption         =   "(dome name)"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   840
         TabIndex        =   45
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
      TabIndex        =   37
      Top             =   5520
      Width           =   4635
      Begin VB.CheckBox chkDomeSlave 
         BackColor       =   &H00000000&
         Caption         =   "Can Slave"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   2280
         TabIndex        =   72
         TabStop         =   0   'False
         Top             =   953
         Width           =   1695
      End
      Begin VB.CheckBox chkDomeFindHome 
         BackColor       =   &H00000000&
         Caption         =   "Can Find Home"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   240
         TabIndex        =   44
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
         TabIndex        =   43
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
         TabIndex        =   42
         TabStop         =   0   'False
         Top             =   360
         Width           =   2115
      End
      Begin VB.CheckBox chkDomeSyncAzimuth 
         BackColor       =   &H00000000&
         Caption         =   "Can Sync Azimuth"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   2280
         TabIndex        =   41
         TabStop         =   0   'False
         Top             =   1253
         Width           =   1695
      End
      Begin VB.CheckBox chkDomeSetAltitude 
         BackColor       =   &H00000000&
         Caption         =   "Can Slew Altitude"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   240
         TabIndex        =   40
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
         TabIndex        =   39
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
         TabIndex        =   38
         TabStop         =   0   'False
         Top             =   660
         Width           =   1425
      End
   End
   Begin VB.CommandButton cmdDome 
      Caption         =   "&Dome >>"
      Height          =   345
      Left            =   4680
      TabIndex        =   8
      Top             =   2580
      Width           =   1245
   End
   Begin VB.Frame frmScopeCon 
      BackColor       =   &H00000000&
      Caption         =   "Scope Connection"
      ForeColor       =   &H00FFFFFF&
      Height          =   1875
      Left            =   2535
      TabIndex        =   21
      Top             =   120
      Width           =   3435
      Begin VB.CommandButton cmdConnect 
         Caption         =   "Connect"
         Height          =   375
         Left            =   2250
         TabIndex        =   1
         Top             =   360
         Width           =   975
      End
      Begin VB.CommandButton cmdChooser 
         Caption         =   "Choose Scope"
         Height          =   375
         Left            =   240
         TabIndex        =   0
         Top             =   360
         Width           =   1410
      End
      Begin VB.CommandButton cmdSetup 
         Caption         =   "Setup"
         Height          =   375
         Left            =   240
         TabIndex        =   2
         Top             =   1320
         Width           =   855
      End
      Begin VB.Label txtChooser 
         Alignment       =   2  'Center
         BackColor       =   &H00000000&
         Caption         =   "(scope name)"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   240
         TabIndex        =   22
         Top             =   900
         Width           =   2955
      End
   End
   Begin VB.CommandButton cmdAdvanced 
      Caption         =   "&Advanced >>"
      Height          =   345
      Left            =   3200
      TabIndex        =   7
      Top             =   2160
      Width           =   1245
   End
   Begin VB.Frame frmScopeCap 
      BackColor       =   &H00000000&
      Caption         =   "Scope Capabilities  (output only)"
      ForeColor       =   &H00FFFFFF&
      Height          =   3060
      Left            =   180
      TabIndex        =   20
      Top             =   5520
      Width           =   5820
      Begin VB.CheckBox chkCanSetDeclinationRate 
         BackColor       =   &H00000000&
         Caption         =   "Can Set Dec Rate"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   1860
         TabIndex        =   61
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
         TabIndex        =   60
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
         TabIndex        =   59
         TabStop         =   0   'False
         Top             =   1380
         Width           =   1665
      End
      Begin VB.CheckBox chkCanEquSystem 
         BackColor       =   &H00000000&
         Caption         =   "Can Equ. System"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   58
         TabStop         =   0   'False
         Top             =   2100
         Width           =   1665
      End
      Begin VB.CheckBox chkCanSyncAltAz 
         BackColor       =   &H00000000&
         Caption         =   "Can Sync Alt / Az"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   3540
         TabIndex        =   57
         TabStop         =   0   'False
         Top             =   2100
         Width           =   2040
      End
      Begin VB.CheckBox chkCanDoesRefraction 
         BackColor       =   &H00000000&
         Caption         =   "Can Does Refrac"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   56
         TabStop         =   0   'False
         Top             =   1140
         Width           =   1665
      End
      Begin VB.CheckBox chkCanSideOfPier 
         BackColor       =   &H00000000&
         Caption         =   "Can Side Of Pier"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   1860
         TabIndex        =   52
         TabStop         =   0   'False
         Top             =   2603
         Width           =   1665
      End
      Begin VB.CheckBox chkCanSlewAltAz 
         BackColor       =   &H00000000&
         Caption         =   "Can Slew Alt / Az"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   3540
         TabIndex        =   51
         TabStop         =   0   'False
         Top             =   1140
         Width           =   2085
      End
      Begin VB.CheckBox chkCanSlewAltAzAsync 
         BackColor       =   &H00000000&
         Caption         =   "Can Slew Alt / Az Async"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   3540
         TabIndex        =   50
         TabStop         =   0   'False
         Top             =   1380
         Width           =   2145
      End
      Begin VB.CheckBox chkCanElevation 
         BackColor       =   &H00000000&
         Caption         =   "Can Elevation"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   36
         TabStop         =   0   'False
         Top             =   1620
         Width           =   1665
      End
      Begin VB.CheckBox chkCanLatLong 
         BackColor       =   &H00000000&
         Caption         =   "Can Lat / Long"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   35
         TabStop         =   0   'False
         Top             =   2610
         Width           =   1545
      End
      Begin VB.CheckBox chkCanOptics 
         BackColor       =   &H00000000&
         Caption         =   "Can Optics"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   1860
         TabIndex        =   34
         TabStop         =   0   'False
         Top             =   653
         Width           =   1425
      End
      Begin VB.CheckBox chkCanSiderealTime 
         BackColor       =   &H00000000&
         Caption         =   "Can Sidereal"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   3540
         TabIndex        =   33
         TabStop         =   0   'False
         Top             =   653
         Width           =   1545
      End
      Begin VB.CheckBox chkCanDateTime 
         BackColor       =   &H00000000&
         Caption         =   "Can Date / Time"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   32
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
         TabIndex        =   31
         TabStop         =   0   'False
         Top             =   1860
         Width           =   1665
      End
      Begin VB.CheckBox chkCanAltAz 
         BackColor       =   &H00000000&
         Caption         =   "Can Alt / Az"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   180
         TabIndex        =   30
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
         TabIndex        =   27
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
         TabIndex        =   26
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
         TabIndex        =   25
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
         TabIndex        =   24
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
         TabIndex        =   23
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
         TabIndex        =   19
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
         TabIndex        =   18
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
         TabIndex        =   17
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
         TabIndex        =   16
         TabStop         =   0   'False
         Top             =   900
         Width           =   1545
      End
      Begin VB.CheckBox chkCanSetPierSide 
         BackColor       =   &H00000000&
         Caption         =   "Can Set Pier Side"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   1860
         TabIndex        =   62
         TabStop         =   0   'False
         Top             =   1853
         Width           =   1716
      End
      Begin VB.Label lblVersion 
         BackColor       =   &H00000000&
         Caption         =   "(version #)"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Left            =   4560
         TabIndex        =   49
         Top             =   300
         Width           =   825
      End
      Begin VB.Label lblVersionLabel 
         BackColor       =   &H00000000&
         Caption         =   "Interface Version:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Left            =   3120
         TabIndex        =   48
         Top             =   300
         Width           =   1365
      End
      Begin VB.Label lblMountType 
         BackColor       =   &H00000000&
         Caption         =   "(alignment output)"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Left            =   1260
         TabIndex        =   29
         Top             =   300
         Width           =   1425
      End
      Begin VB.Label lblMountTypeLabel 
         BackColor       =   &H00000000&
         Caption         =   "Mount Type:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Left            =   180
         TabIndex        =   28
         Top             =   300
         Width           =   1065
      End
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "&Cancel"
      Height          =   345
      Left            =   1720
      TabIndex        =   6
      Top             =   2580
      Width           =   1245
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   345
      Left            =   240
      TabIndex        =   5
      Top             =   2580
      Width           =   1245
   End
   Begin VB.Label lblDriverInfo 
      BackColor       =   &H00000000&
      Caption         =   "<version, etc.>"
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   120
      TabIndex        =   47
      Top             =   1935
      Width           =   2175
   End
   Begin VB.Label lblLastModified 
      BackColor       =   &H00000000&
      Caption         =   "<last modified>"
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   120
      TabIndex        =   46
      Top             =   2250
      Width           =   3015
   End
   Begin VB.Image imgBrewster 
      Height          =   555
      Left            =   720
      MouseIcon       =   "frmSetup.frx":18E0
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":1A32
      Top             =   225
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
' Pipe setup form
'
' Written: Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 02-Sep-06 jab     Initial edit
' 12-Oct-06 jab     More robust connect (look for error)
' 06-Jun-07 jab     "Hub" creation changes
' -----------------------------------------------------------------------------

Option Explicit

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
    
    If App.Title = "Hub" Then
        Caption = "Hub Setup"
        cmdAdvanced.Visible = False
        cmdConnect.Visible = False
        cmdDomeConnect.Visible = False
        cmdFocuserConnect.Visible = False
    End If
        
    FloatWindow Me.hwnd, True                       ' Setup window always floats
    m_bAllowUnload = True                           ' Start out allowing unload
    m_bAdvancedMode = False
    
    lblDriverInfo = App.Title & " " & _
        App.Major & "." & App.Minor & "." & App.Revision
        
    Set fs = CreateObject("Scripting.FileSystemObject")
    Set F = fs.GetFile(App.Path & "\" & App.EXEName & ".exe")
    DLM = F.DateLastModified
    
    lblLastModified = "Modified " & DLM
        
    setDefaults
    setDomeDefaults
    setFocuserDefaults
    
End Sub

Private Sub Form_Unload(Cancel As Integer)

    Me.Hide
    Cancel = CInt(Not m_bAllowUnload)
    
End Sub

Private Sub cmdOK_Click()
    Dim val As Double
    Dim vali As Integer
     
    ScopeSave
    DomeSave
    FocuserSave
    
    g_bSetupAdvanced = Me.AdvancedMode
    g_bDomeMode = Me.DomeMode
    g_bFocusMode = Me.FocusMode
    
    Me.Hide
    
End Sub

Private Sub cmdCancel_Click()
    
    Me.Hide

End Sub

Private Sub cmdHelp_Click()

    DisplayWebPage App.Path & "/PipeHubHelp.htm"
    
End Sub

Private Sub cmdAdvanced_Click()

    SetFormMode Not m_bAdvancedMode                ' Toggle form mode
    
End Sub

Private Sub cmdDome_Click()

    SetDeviceMode Not m_bDomeMode, m_bFocusMode    ' Toggle dome mode
    
End Sub

Private Sub cmdFocus_Click()

    SetDeviceMode m_bDomeMode, Not m_bFocusMode    ' Toggle focus mode
    
End Sub

Private Sub imgBrewster_Click()

    DisplayWebPage "http://astro.brewsters.net/"
    
End Sub

Private Sub picASCOM_Click()

    DisplayWebPage "http://ascom-standards.org/"
    
End Sub

Private Sub cmdChooser_Click()
    
    Dim newScopeID As String
    Dim g_Chooser As DriverHelper.Chooser
    
    ' deal with the chooser
    Set g_Chooser = New DriverHelper.Chooser
    g_Chooser.DeviceType = "Telescope"
    FloatWindow Me.hwnd, False
    newScopeID = g_Chooser.Choose(g_sScopeID)
    FloatWindow Me.hwnd, True
    Set g_Chooser = Nothing
    
    ' see if we got something usable
    If newScopeID = "" Then _
        Exit Sub
    If newScopeID = g_sSCOPE Then
        MsgBox "Yeah right...  Try again.", vbExclamation
        Exit Sub
    End If

    ' if new device, shut down any existing connection
    If newScopeID <> g_sScopeID Then
        If Not g_Scope Is Nothing Then
            If g_bConnected Then _
                cmdConnect_Click
            ScopeSave
            ScopeDelete
        End If
    End If
    
    ' if new device being requested
    On Error Resume Next
    If g_Scope Is Nothing Then
    
        g_sScopeID = newScopeID
        ScopeCreate (newScopeID)
        
        If g_Scope Is Nothing Then
            UpdateScope False, g_bConnected
            MsgBox "Error opening Scope.", vbExclamation
            Exit Sub
        End If
    End If
    On Error GoTo 0
    
    UpdateScope False, g_bConnected
    
End Sub

Private Sub cmdConnect_Click()

    ConnectScope True
 
End Sub

Private Sub cmdSetup_Click()

    If g_sScopeID = "" Then
        MsgBox "You must select a telescope first", vbExclamation
        Exit Sub
    End If
    
    On Error Resume Next
    
    If g_Scope Is Nothing Then
        ScopeCreate (g_sScopeID)
        UpdateScope False, g_bConnected
    End If
    
    If g_Scope Is Nothing Then
        MsgBox "Error opening scope driver.", vbExclamation
        Exit Sub
    End If
    
    FloatWindow Me.hwnd, False
    g_Scope.SetupDialog
    FloatWindow Me.hwnd, True
    
    ' refresh state in case scope data changed
    UpdateScope False, g_bConnected
    
    On Error GoTo 0
    
End Sub

Private Sub cmdDomeChooser_Click()

    Dim newDomeID As String
    Dim g_DomeChooser As DriverHelper.Chooser
    
    ' deal with the chooser
    Set g_DomeChooser = New DriverHelper.Chooser
    g_DomeChooser.DeviceType = "Dome"
    FloatWindow Me.hwnd, False
    newDomeID = g_DomeChooser.Choose(g_sDomeID)
    FloatWindow Me.hwnd, True
    Set g_DomeChooser = Nothing
    
    ' see if we got something usable
    If newDomeID = "" Then _
        Exit Sub
    If newDomeID = g_sDOME Then
        MsgBox "Yeah right...  Try again.", vbExclamation
        Exit Sub
    End If
    
    ' if new device, shut down any existing connection
    If newDomeID <> g_sDomeID Then
        If Not g_Dome Is Nothing Then
            If g_bDomeConnected Then _
                cmdDomeConnect_Click
            DomeSave
            DomeDelete
        End If
    End If
    
    ' if new device being requested
    On Error Resume Next
    If g_Dome Is Nothing Then
        
        g_sDomeID = newDomeID
        DomeCreate (newDomeID)
        
        If g_Dome Is Nothing Then
            UpdateDome False, g_bDomeConnected
            MsgBox "Error opening Dome.", vbExclamation
            Exit Sub
        End If
    End If
    On Error GoTo 0
    
    UpdateDome False, g_bDomeConnected
    
End Sub

Private Sub cmdDomeConnect_Click()
    
    ConnectDome True
    
End Sub

Private Sub cmdDomeSetup_Click()

    If g_sDomeID = "" Then
        MsgBox "You must select a dome first", vbExclamation
        Exit Sub
    End If
    
    On Error Resume Next
    
    If g_Dome Is Nothing Then
        DomeCreate (g_sDomeID)
        UpdateDome False, g_bDomeConnected
    End If
    
    If g_Dome Is Nothing Then
        MsgBox "Error opening dome driver.", vbExclamation
        Exit Sub
    End If
    
    FloatWindow Me.hwnd, False
    g_Dome.SetupDialog
    FloatWindow Me.hwnd, True
    
    ' refresh state in case dome data changed
    UpdateDome False, g_bDomeConnected
    
    On Error GoTo 0
    
End Sub

Private Sub cmdFocuserChooser_Click()

    Dim newFocuserID As String
    Dim g_FocuserChooser As DriverHelper.Chooser
    
    ' deal with the chooser
    Set g_FocuserChooser = New DriverHelper.Chooser
    g_FocuserChooser.DeviceType = "Focuser"
    FloatWindow Me.hwnd, False
    newFocuserID = g_FocuserChooser.Choose(g_sFocuserID)
    FloatWindow Me.hwnd, True
    Set g_FocuserChooser = Nothing
    
    ' see if we got something usable
    If newFocuserID = "" Then _
        Exit Sub
    If newFocuserID = g_sFOCUSER Then
        MsgBox "Yeah right...  Try again.", vbExclamation
        Exit Sub
    End If

    ' if new device, shut down any existing connection
    If newFocuserID <> g_sFocuserID Then
        If Not g_Focuser Is Nothing Then
            If g_bFocuserConnected Then _
                cmdFocuserConnect_Click
            FocuserSave
            FocuserDelete
        End If
    End If
    
    ' if new device being requested
    On Error Resume Next
    If g_Focuser Is Nothing Then
    
        g_sFocuserID = newFocuserID
        FocuserCreate (newFocuserID)
        
        If g_Focuser Is Nothing Then
            UpdateFocuser False, g_bFocuserConnected
            MsgBox "Error opening Focuser.", vbExclamation
            Exit Sub
        End If
    End If
    On Error GoTo 0
    

    UpdateFocuser False, g_bFocuserConnected
    
End Sub

Private Sub cmdFocuserConnect_Click()
    
    ConnectFocuser True
    
End Sub

Private Sub cmdFocuserSetup_Click()

    If g_sFocuserID = "" Then
        MsgBox "You must select a Focuser first", vbExclamation
        Exit Sub
    End If
    
    On Error Resume Next
    
    If g_Focuser Is Nothing Then
        FocuserCreate (g_sFocuserID)
        UpdateFocuser False, g_bFocuserConnected
    End If
    
    If g_Focuser Is Nothing Then
        MsgBox "Error opening Focuser driver.", vbExclamation
        Exit Sub
    End If
    
    FloatWindow Me.hwnd, False
    g_Focuser.SetupDialog
    FloatWindow Me.hwnd, True
    
    ' refresh state in case Focuser data changed
    UpdateFocuser False, g_bFocuserConnected
    
    On Error GoTo 0
    
End Sub

' =============
' PUBLIC ACCESS
' =============

Public Sub ConnectScope(prompt As Boolean)
    
    On Error Resume Next
    
    If g_bConnected Then
        g_Scope.Connected = False
        UpdateScope True, False
        If g_iConnections <= 0 Then
            ScopeSave
            ScopeDelete
        End If
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
    
    FloatWindow Me.hwnd, False
    Err.Clear
    g_Scope.Connected = True
    If Err.Number <> 0 Then
        MsgBox "Error connecting to scope." & vbCrLf & _
        "Odd behavior may result.", _
        vbExclamation
    End If
    FloatWindow Me.hwnd, True
    g_bManual = True
    UpdateScope False, g_Scope.Connected
    
    On Error GoTo 0
    
End Sub

Public Sub UpdateScope(check As Boolean, newVal As Boolean)
    
    If check Then
        If newVal = g_bConnected Then _
            Exit Sub
    End If
    
    g_bConnected = newVal
    
    If newVal Then
        ReadScope
    Else
        setDefaults
    End If
    
End Sub

Public Sub ConnectDome(prompt As Boolean)

    On Error Resume Next
    
    If g_bDomeConnected Then
        g_Dome.Connected = False
        UpdateDome True, False
        If g_iDomeConnections <= 0 Then
            DomeSave
            DomeDelete
        End If
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
    
    FloatWindow Me.hwnd, False
    Err.Clear
    g_Dome.Connected = True
    If Err.Number <> 0 Then
        MsgBox "Error connecting to dome." & vbCrLf & _
        "Odd behavior may result.", _
        vbExclamation
    End If
    FloatWindow Me.hwnd, True
    g_bDomeManual = True
    UpdateDome False, g_Dome.Connected
    
    On Error GoTo 0
    
End Sub

Public Sub UpdateDome(check As Boolean, newVal As Boolean)
    
    If check Then
        If newVal = g_bDomeConnected Then _
            Exit Sub
    End If
    
    g_bDomeConnected = newVal
    
    If newVal Then
        ReadDome
    Else
        setDomeDefaults
    End If
    
End Sub

Public Sub ConnectFocuser(prompt As Boolean)

    On Error Resume Next
    
    If g_bFocuserConnected Then
        g_Focuser.Link = False
        UpdateFocuser True, False
        If g_iFocuserConnections <= 0 Then
            FocuserSave
            FocuserDelete
        End If
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
    
    FloatWindow Me.hwnd, False
    Err.Clear
    g_Focuser.Link = True
    If Err.Number <> 0 Then
        MsgBox "Error connecting to focuser." & vbCrLf & _
        "Odd behavior may result.", _
        vbExclamation
    End If
    FloatWindow Me.hwnd, True
    g_bFocuserManual = True
    UpdateFocuser False, g_Focuser.Link
    
    On Error GoTo 0
    
End Sub

Public Sub UpdateFocuser(check As Boolean, newVal As Boolean)
    
    If check Then
        If newVal = g_bFocuserConnected Then _
            Exit Sub
    End If
    
    g_bFocuserConnected = newVal
    
    If newVal Then
        ReadFocuser
    Else
        setFocuserDefaults
    End If
    
End Sub

' =================
' PUBLIC PROPERTIES
' =================

Public Property Let ConnectCaption(val As Boolean)

    cmdConnect.Caption = IIf(val, "Connect", "Disconnect")
    g_handBox.cmdConnectScope.Caption = IIf(val, "Connect Scope", "Disconnect Scope")
    
End Property

Public Property Let FocuserConnectCaption(val As Boolean)

    cmdFocuserConnect.Caption = IIf(val, "Connect", "Disconnect")
    g_handBox.cmdConnectFocuser.Caption = IIf(val, "Connect Focuser", "Disconnect Focuser")
    
End Property

Public Property Let DomeConnectCaption(val As Boolean)

    cmdDomeConnect.Caption = IIf(val, "Connect", "Disconnect")
    g_handBox.cmdConnectDome.Caption = IIf(val, "Connect Dome", "Disconnect Dome")
    
End Property

Public Property Let Aperture(ByVal newVal As Double)

    If newVal = EMPTY_PARAMETER Then
        txtAperture.Caption = "-.---"
    Else
        txtAperture.Caption = Format$(newVal, "0.000")
    End If
    
End Property

Public Property Let ApertureArea(ByVal newVal As Double)

    If newVal = EMPTY_PARAMETER Then
        txtApertureArea.Caption = "-.-----"
    Else
        txtApertureArea.Caption = Format$(newVal, "0.00000")
    End If
    
End Property

Public Property Let FocalLength(ByVal newVal As Double)

    If newVal = EMPTY_PARAMETER Then
        txtFocalLength.Caption = "-.---"
    Else
        txtFocalLength.Caption = Format$(newVal, "0.000")
    End If
    
End Property

Public Property Let Latitude(ByVal newVal As Double)

    Dim u As Integer
    Dim s, d As String
    
    If newVal = EMPTY_PARAMETER Then
        s = "-- --.-- -"
    Else
        If newVal < 0 Then
            newVal = -newVal
            d = " S"
        Else
            d = " N"
        End If
        u = Fix(newVal)                       ' Degrees
        s = s & Format$(u, "00") & " "
        newVal = (newVal - u) * 60            ' Minutes
        s = s & Format$(newVal, "00.00") & d
    End If
    
    txtlatitude = s

End Property

Public Property Let Longitude(ByVal newVal As Double)
    
    Dim u As Integer
    Dim s, d As String

    If newVal = EMPTY_PARAMETER Then
        s = "--- --.-- -"
    Else
        If newVal < 0 Then
            newVal = -newVal
            d = " W"
        Else
            d = " E"
        End If
        u = Fix(newVal)                       ' Degrees
        s = Format$(u, "000") & " "
        newVal = (newVal - u) * 60            ' Minutes
        s = s & Format$(newVal, "00.00") & d
    End If
    
    txtLongitude = s
    
End Property

Public Property Let Elevation(ByVal newVal As Double)

    If newVal = EMPTY_PARAMETER Then
        txtElevation.Caption = "----.-"
    Else
        txtElevation.Caption = Format$(newVal, "0.0")
    End If

End Property

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

'
' LOCAL UTILITIES
'

Private Sub ReadScope()

    Dim tmpB As Boolean
    Dim tmpI As Integer
    Dim tmpD As Double
    Dim tmpSideOfPier As PierSide
    Dim tmpEquSystem As EquatorialCoordinateType
    Dim tmpUTCDate As Date
    Dim tmpAlignMode As AlignmentModes
    Dim tmpRef As Boolean
    Dim tmpS As String
    
    Dim version As Integer
    Dim i As Integer
    
    ConnectCaption = Not g_bConnected
    txtChooser.Caption = g_sScopeName
    
    If g_Scope Is Nothing Or Not g_bConnected Or Not m_bAdvancedMode Then
        Exit Sub
    End If
    
    ' assume API version 1 unless g_scope actually works...
    On Error Resume Next
    version = 1
    version = g_Scope.InterfaceVersion
    lblVersion = IIf(version > 0, version, "(Unknown)")
    
    On Error GoTo CatchCanAltAz
    tmpB = True
    tmpD = g_Scope.Azimuth
    tmpD = g_Scope.Altitude
    GoTo FinalCanAltAz
CatchCanAltAz:
    tmpB = False
    Resume Next
FinalCanAltAz:
    On Error GoTo 0
    chkCanAltAz = IIf(tmpB, 1, 0)
    
    On Error GoTo CatchCanEqu
    tmpB = True
    tmpD = g_Scope.RightAscension
    tmpD = g_Scope.Declination
    GoTo FinalCanEqu
CatchCanEqu:
    tmpB = False
    Resume Next
FinalCanEqu:
    On Error GoTo 0
    chkCanEqu = IIf(tmpB, 1, 0)
    
    On Error GoTo CatchCanDateTime
    tmpB = True
    tmpD = g_Scope.UTCDate
    GoTo FinalCanDateTime
CatchCanDateTime:
    tmpB = False
    Resume Next
FinalCanDateTime:
    On Error GoTo 0
    chkCanDateTime = IIf(tmpB, 1, 0)
    
    On Error GoTo CatchOptics
    tmpB = True
    tmpD = EMPTY_PARAMETER
    tmpD = g_Scope.ApertureDiameter
    Me.Aperture = tmpD
    If version >= 2 Then
        tmpD = EMPTY_PARAMETER
        tmpD = g_Scope.ApertureArea
        Me.ApertureArea = tmpD
    End If
    tmpD = EMPTY_PARAMETER
    tmpD = g_Scope.FocalLength
    Me.FocalLength = tmpD
    GoTo FinalOptics
CatchOptics:
    tmpB = False
    Resume Next
FinalOptics:
    chkCanOptics = IIf(tmpB, 1, 0)
    On Error GoTo 0
    
    On Error GoTo CatchLatLong
    tmpB = True
    tmpD = EMPTY_PARAMETER
    tmpD = g_Scope.SiteLatitude
    Me.Latitude = tmpD
    tmpD = EMPTY_PARAMETER
    tmpD = g_Scope.SiteLongitude
    Me.Longitude = tmpD
    GoTo FinalLatLong
CatchLatLong:
    tmpB = False
    Resume Next
FinalLatLong:
    chkCanLatLong = IIf(tmpB, 1, 0)
    On Error GoTo 0
        
    On Error GoTo CatchElev
    tmpB = True
    tmpD = g_Scope.SiteElevation
    Me.Elevation = tmpD
    GoTo FinalElev
CatchElev:
    tmpB = False
    Resume Next
FinalElev:
    chkCanElevation = IIf(tmpB, 1, 0)
    On Error GoTo 0
     
    On Error GoTo CatchAM
    tmpB = True
    tmpAlignMode = ALG_UNKNOWN
    tmpAlignMode = g_Scope.AlignmentMode
    GoTo FinalAM
CatchAM:
    tmpB = False
    Resume Next
FinalAM:
    If tmpB Then
        If tmpAlignMode = algAltAz Then
            lblMountType = "Alt-Azimuth"
        ElseIf tmpAlignMode = algPolar Then
            lblMountType = "Equatorial"
        Else
            lblMountType = "German Equatorial"
        End If
    Else
        lblMountType = "(Unknown)"
    End If
    On Error GoTo 0
    
    On Error GoTo CatchPier
    tmpB = True
    If tmpAlignMode = algGermanPolar Then
        tmpSideOfPier = g_Scope.SideOfPier
    Else
        tmpB = False
    End If
    GoTo FinalPier
CatchPier:
    tmpB = False
    Resume Next
FinalPier:
    chkCanSideOfPier = IIf(tmpB, 1, 0)
    On Error GoTo 0
        
    On Error Resume Next
        chkCanFindHome = IIf(g_Scope.CanFindHome, 1, 0)
        chkCanPark = IIf(g_Scope.CanPark, 1, 0)
        chkCanPulseGuide = IIf(g_Scope.CanPulseGuide, 1, 0)
        chkCanSetDeclinationRate = IIf(g_Scope.CanSetDeclinationRate, 1, 0)
        chkCanSetGuideRates = IIf(g_Scope.CanSetGuideRates, 1, 0)
        chkCanSetPark = IIf(g_Scope.CanSetPark, 1, 0)
        chkCanSetPierSide = IIf(g_Scope.CanSetPierSide, 1, 0)
        chkCanSetRightAscensionRate = IIf(g_Scope.CanSetRightAscensionRate, 1, 0)
        chkCanSetTracking = IIf(g_Scope.CanSetTracking, 1, 0)
        chkCanSlew = IIf(g_Scope.CanSlew, 1, 0)
        chkCanSlewAsync = IIf(g_Scope.CanSlewAsync, 1, 0)
        chkCanSlewAltAz = IIf(g_Scope.CanSlewAltAz, 1, 0)
        chkCanSlewAltAzAsync = IIf(g_Scope.CanSlewAltAzAsync, 1, 0)
        chkCanSync = IIf(g_Scope.CanSync, 1, 0)
        chkCanSyncAltAz = IIf(g_Scope.CanSyncAltAz, 1, 0)
        chkCanUnpark = IIf(g_Scope.CanUnpark, 1, 0)
    On Error GoTo 0
    
    On Error GoTo CatchDoesRefraction
    tmpB = True
    If version > 1 Then
        tmpRef = g_Scope.DoesRefraction
    Else
        tmpB = False
    End If
    GoTo FinalDoesRefraction
CatchDoesRefraction:
    tmpB = False
    Resume Next
FinalDoesRefraction:
    txtRefraction = IIf(tmpB, IIf(tmpRef, "Yes", " No"), "---")
    chkCanDoesRefraction = IIf(tmpB, 1, 0)
    On Error GoTo 0
        
    On Error GoTo CatchEquSystem
    tmpB = True
    tmpEquSystem = equOther
    If version > 1 Then
        tmpEquSystem = g_Scope.EquatorialSystem
    Else
        tmpB = False
    End If
    GoTo FinalEquSystem
CatchEquSystem:
    tmpB = False
    Resume Next
FinalEquSystem:
    tmpS = "-----"
    If tmpB Then
    
        Select Case tmpEquSystem
            Case equLocalTopocentric:   tmpS = "Local"
            Case equB1950:              tmpS = "B1950"
            Case equJ2000:              tmpS = "J2000"
            Case equJ2050:              tmpS = "J2050"
            Case equOther:              tmpS = "Other"
        End Select
        
    End If
    txtEquSystem = tmpS
    chkCanEquSystem = IIf(tmpB, 1, 0)
    On Error GoTo 0
        
    On Error GoTo CatchCanSiderealTime
    tmpB = True
    tmpD = g_Scope.SiderealTime
    GoTo FinalCanSiderealTime
CatchCanSiderealTime:
    tmpB = False
    Resume Next
FinalCanSiderealTime:
    chkCanSiderealTime = IIf(tmpB, 1, 0)
    On Error GoTo 0
    
    On Error Resume Next
    On Error GoTo 0
    
End Sub

Private Sub ReadDome()
    
    DomeConnectCaption = Not g_bDomeConnected
    txtDomeChooser.Caption = g_sDomeName
    
    If g_Dome Is Nothing Or Not g_bDomeConnected Or Not m_bAdvancedMode Then
        Exit Sub
    End If
        
    On Error Resume Next
    
    chkDomeFindHome = IIf(g_Dome.CanFindHome, 1, 0)
    chkDomePark = IIf(g_Dome.CanPark, 1, 0)
    chkDomeSetAltitude = IIf(g_Dome.CanSetAltitude, 1, 0)
    chkDomeSetAzimuth = IIf(g_Dome.CanSetAzimuth, 1, 0)
    chkDomeSetPark = IIf(g_Dome.CanSetPark, 1, 0)
    chkDomeSetShutter = IIf(g_Dome.CanSetShutter, 1, 0)
    chkDomeSyncAzimuth = IIf(g_Dome.CanSyncAzimuth, 1, 0)
        
    On Error GoTo 0
    
End Sub

Private Sub ReadFocuser()
    Dim tmpD As Double
    Dim tmpL As Long

    FocuserConnectCaption = Not g_bFocuserConnected
    txtFocuserChooser.Caption = g_sFocuserName
    
    If g_Focuser Is Nothing Or Not g_bFocuserConnected Or Not m_bAdvancedMode Then
        Exit Sub
    End If
        
    On Error Resume Next
    
    Err.Clear
    tmpL = g_Focuser.MaxIncrement
    txtMaxIncrement = IIf(Err, "------", tmpL)
    
    Err.Clear
    tmpL = g_Focuser.MaxStep
    txtMaxStep = IIf(Err, "------", tmpL)
    
    Err.Clear
    tmpL = g_Focuser.StepSize
    txtStepSize = IIf(Err, "---", tmpL)
    chkFocuserStepSize = IIf(Err, 0, 1)
    
    Err.Clear
    tmpD = g_Focuser.Temperature
    chkTemperatureProbe = IIf(Err, 0, 1)
    
    chkFocuserTemperatureCompensate = IIf(g_Focuser.TempCompAvailable, 1, 0)
    
    Err.Clear
    g_Focuser.Halt
    chkFocuserHalt = IIf(Err, 0, 1)
    
    chkFocuserAbsolute = IIf(g_Focuser.Absolute, 1, 0)
    
    On Error GoTo 0
    
End Sub

Private Sub setDefaults()
    
    ConnectCaption = True
    
    Me.Aperture = EMPTY_PARAMETER
    Me.ApertureArea = EMPTY_PARAMETER
    Me.FocalLength = EMPTY_PARAMETER
    
    Me.Latitude = EMPTY_PARAMETER
    Me.Longitude = EMPTY_PARAMETER
    Me.Elevation = EMPTY_PARAMETER
    
    ' set DoesRefraction list item to "unknown"
    txtRefraction = "---"
    
    ' set Equatorial Coordinates list item to "unknown"
    txtEquSystem = "-----"
    
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
    
End Sub

Private Sub setDomeDefaults()
    
    DomeConnectCaption = True
    
    chkDomeFindHome = 2
    chkDomePark = 2
    chkDomeSetAltitude = 2
    chkDomeSetAzimuth = 2
    chkDomeSetShutter = 2
    chkDomeSetPark = 2
    chkDomeSyncAzimuth = 2
    
    txtDomeChooser.Caption = g_sDomeName
        
End Sub

Private Sub setFocuserDefaults()
    
    FocuserConnectCaption = True
    
    txtMaxIncrement = "------"
    txtMaxStep = "------"
    txtStepSize = "---"
    
    chkTemperatureProbe = 2
    chkFocuserTemperatureCompensate = 2
    chkFocuserHalt = 2
    chkFocuserStepSize = 2
    chkFocuserAbsolute = 2
    
    txtFocuserChooser.Caption = g_sFocuserName

End Sub

Private Sub SetDeviceMode(DomeMode As Boolean, FocusMode As Boolean)
    
    If DomeMode Then
        Me.frmDomeCon.Visible = True
        Me.frmDomeCap.Visible = True
        Me.Width = 11100
        Me.cmdDome.Caption = "<< No &Dome"
        
        Me.frmFocuserCon.Left = 11070
        Me.frmFocuserStep.Left = 11070
        Me.frmFocuserCap.Left = 11070
    Else
        Me.frmDomeCon.Visible = False
        Me.frmDomeCap.Visible = False
        Me.Width = 6200
        Me.cmdDome.Caption = "&Dome >>"
        
        Me.frmFocuserCon.Left = 6240
        Me.frmFocuserStep.Left = 6240
        Me.frmFocuserCap.Left = 6240
    End If
    
    If FocusMode Then
        Me.frmFocuserCon.Visible = True
        Me.frmFocuserStep.Visible = True
        Me.frmFocuserCap.Visible = True
        Me.Width = Me.Width + 3200
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
        Me.Height = 9120
        Me.cmdAdvanced.Caption = "<< B&asic"
    Else
        Me.Height = 3450
        Me.cmdAdvanced.Caption = "&Advanced >>"
    End If
    
    m_bAdvancedMode = Advanced
    
    If Advanced Then
        UpdateScope False, g_bConnected
        UpdateDome False, g_bDomeConnected
        UpdateFocuser False, g_bFocuserConnected
    End If
    
End Sub
