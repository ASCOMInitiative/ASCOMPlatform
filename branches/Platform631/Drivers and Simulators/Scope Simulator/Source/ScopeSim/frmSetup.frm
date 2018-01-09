VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmSetup 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "ASCOM Telescope Simulator Setup"
   ClientHeight    =   7290
   ClientLeft      =   45
   ClientTop       =   285
   ClientWidth     =   9570
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   7258.578
   ScaleMode       =   0  'User
   ScaleWidth      =   9570
   StartUpPosition =   2  'CenterScreen
   Begin VB.CheckBox chkNoCoordAtPark 
      BackColor       =   &H00000000&
      Caption         =   "No Coordinates when Parked"
      ForeColor       =   &H00FFFFFF&
      Height          =   300
      Left            =   4005
      TabIndex        =   9
      ToolTipText     =   "Turn tacking on at Unpark and start up"
      Top             =   525
      Width           =   2475
   End
   Begin VB.CheckBox chkDiscPark 
      BackColor       =   &H00000000&
      Caption         =   "Disconnect on Park"
      ForeColor       =   &H00FFFFFF&
      Height          =   300
      Left            =   4005
      TabIndex        =   10
      ToolTipText     =   "Turn tacking on at Unpark and start up"
      Top             =   840
      Width           =   1905
   End
   Begin VB.CommandButton cmdHelp 
      Caption         =   "&Help"
      Height          =   345
      Left            =   6525
      TabIndex        =   23
      Top             =   3510
      Width           =   1230
   End
   Begin VB.CheckBox chkAutoTrack 
      BackColor       =   &H00000000&
      Caption         =   "Auto Unpark/Track on Start"
      ForeColor       =   &H00FFFFFF&
      Height          =   270
      Left            =   4005
      TabIndex        =   8
      ToolTipText     =   "Turn tacking on at Unpark and start up"
      Top             =   240
      Width           =   2430
   End
   Begin VB.CommandButton cmdResetParkPosition 
      Caption         =   "&Reset Park Position"
      Height          =   525
      Left            =   8190
      TabIndex        =   17
      ToolTipText     =   "Reset park position to default coodinates"
      Top             =   405
      Width           =   1230
   End
   Begin VB.CommandButton cmdSetParkPosition 
      Caption         =   "&Set Park Position"
      Height          =   525
      Left            =   6600
      TabIndex        =   16
      ToolTipText     =   "Set park position to current coordinates"
      Top             =   405
      Width           =   1230
   End
   Begin VB.CheckBox chkDoRefraction 
      BackColor       =   &H00000000&
      Caption         =   "Refraction On"
      ForeColor       =   &H00FFFFFF&
      Height          =   195
      Left            =   4050
      TabIndex        =   14
      TabStop         =   0   'False
      Top             =   3060
      Visible         =   0   'False
      Width           =   1320
   End
   Begin VB.Frame Frame4 
      BackColor       =   &H00000000&
      Caption         =   "Optics"
      ForeColor       =   &H00FFFFFF&
      Height          =   1605
      Left            =   6480
      TabIndex        =   65
      Top             =   1215
      Width           =   2970
      Begin VB.TextBox ApertureAreaTF 
         Height          =   315
         Left            =   1755
         TabIndex        =   19
         Top             =   675
         Width           =   1050
      End
      Begin VB.TextBox FocalLengthTF 
         Height          =   315
         Left            =   1755
         TabIndex        =   20
         Top             =   1125
         Width           =   1050
      End
      Begin VB.TextBox ApertureTF 
         Height          =   315
         Left            =   1755
         TabIndex        =   18
         Top             =   240
         Width           =   1050
      End
      Begin VB.Label lblApertureArea 
         BackColor       =   &H00000000&
         Caption         =   "Aperture Area (m^2):"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   1
         Left            =   135
         TabIndex        =   68
         Top             =   720
         Width           =   1485
      End
      Begin VB.Label Label6 
         BackColor       =   &H00000000&
         Caption         =   "Focal Length (m):"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   0
         Left            =   135
         TabIndex        =   67
         Top             =   1170
         Width           =   1245
      End
      Begin VB.Label Label5 
         BackColor       =   &H00000000&
         Caption         =   "Aperture (m):"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   0
         Left            =   135
         TabIndex        =   66
         Top             =   285
         Width           =   930
      End
   End
   Begin MSComctlLib.Slider sldSlewRate 
      Height          =   300
      Left            =   1395
      TabIndex        =   7
      ToolTipText     =   "Sets slewing speed (highest will be instant)"
      Top             =   3105
      Width           =   1995
      _ExtentX        =   3519
      _ExtentY        =   529
      _Version        =   393216
      LargeChange     =   10
      SmallChange     =   5
      Min             =   1
      Max             =   50
      SelStart        =   5
      TickStyle       =   3
      TickFrequency   =   5
      Value           =   5
   End
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   180
      MouseIcon       =   "frmSetup.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":0152
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   63
      Top             =   2970
      Width           =   720
   End
   Begin VB.CheckBox chkAlwaysTop 
      BackColor       =   &H00000000&
      Caption         =   "Always on Top"
      ForeColor       =   &H00FFFFFF&
      Height          =   255
      Left            =   6525
      TabIndex        =   21
      Top             =   3015
      Value           =   1  'Checked
      Width           =   1575
   End
   Begin VB.CommandButton cmdAdvanced 
      Caption         =   "&Advanced >>"
      Height          =   345
      Left            =   4050
      TabIndex        =   15
      Top             =   3510
      Width           =   1230
   End
   Begin VB.Frame Frame3 
      BackColor       =   &H00000000&
      Caption         =   "Interface Capabilities"
      ForeColor       =   &H00FFFFFF&
      Height          =   2880
      Left            =   120
      TabIndex        =   62
      Top             =   4275
      Width           =   9315
      Begin VB.CheckBox chkCanSetSOP 
         BackColor       =   &H00000000&
         Caption         =   "Set Pier Side"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   4950
         TabIndex        =   43
         Top             =   900
         Width           =   1905
      End
      Begin VB.CheckBox chkCanSlewAltAzAsync 
         BackColor       =   &H00000000&
         Caption         =   "Alt / Az Asynchronous"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   270
         TabIndex        =   28
         Top             =   1170
         Width           =   2085
      End
      Begin VB.ComboBox cbNumMoveAxis 
         Height          =   315
         ItemData        =   "frmSetup.frx":1016
         Left            =   3960
         List            =   "frmSetup.frx":1026
         Style           =   2  'Dropdown List
         TabIndex        =   37
         Top             =   1575
         Width           =   555
      End
      Begin VB.ComboBox cbEquSystem 
         Height          =   315
         ItemData        =   "frmSetup.frx":1036
         Left            =   5940
         List            =   "frmSetup.frx":1049
         Style           =   2  'Dropdown List
         TabIndex        =   45
         Top             =   1575
         Width           =   915
      End
      Begin VB.CheckBox chkDualAxisPulseGuide 
         BackColor       =   &H00000000&
         Caption         =   "Dual Axis Pulse Guide"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   7290
         TabIndex        =   52
         Top             =   1440
         Width           =   1905
      End
      Begin VB.CheckBox chkCanPulseGuide 
         BackColor       =   &H00000000&
         Caption         =   "Pulse Guide"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   7290
         TabIndex        =   51
         Top             =   1170
         Width           =   1905
      End
      Begin VB.CheckBox chkV1 
         BackColor       =   &H00000000&
         Caption         =   "Version 1 only"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   7290
         TabIndex        =   54
         Top             =   2520
         Width           =   1905
      End
      Begin VB.CheckBox chkCanDoesRefraction 
         BackColor       =   &H00000000&
         Caption         =   "Refraction Support"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   4950
         TabIndex        =   44
         Top             =   1170
         Width           =   1905
      End
      Begin VB.CheckBox chkCanSetGuideRates 
         BackColor       =   &H00000000&
         Caption         =   "Guide Rates"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   7290
         TabIndex        =   50
         Top             =   900
         Width           =   1905
      End
      Begin VB.CheckBox chkCanSOP 
         BackColor       =   &H00000000&
         Caption         =   "Side Of Pier"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   4950
         TabIndex        =   42
         Top             =   630
         Width           =   1905
      End
      Begin VB.CheckBox chkCanTrackingRates 
         BackColor       =   &H00000000&
         Caption         =   "Tracking Rates"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   7290
         TabIndex        =   49
         Top             =   630
         Width           =   1905
      End
      Begin VB.CheckBox chkCanSetPark 
         BackColor       =   &H00000000&
         Caption         =   "Set Park Position"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   2610
         TabIndex        =   35
         Top             =   900
         Width           =   1905
      End
      Begin VB.CheckBox chkCanSetEquRates 
         BackColor       =   &H00000000&
         Caption         =   "RA / Dec Rates"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   7290
         TabIndex        =   53
         Top             =   1710
         Width           =   1545
      End
      Begin VB.CheckBox chkCanFindHome 
         BackColor       =   &H00000000&
         Caption         =   "Find Home"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   2610
         TabIndex        =   33
         Top             =   360
         Width           =   1305
      End
      Begin VB.CheckBox chkCanSyncAltAz 
         BackColor       =   &H00000000&
         Caption         =   "Alt / Az Sync"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   270
         TabIndex        =   27
         Top             =   900
         Width           =   1425
      End
      Begin VB.CheckBox chkCanSlewAltAz 
         BackColor       =   &H00000000&
         Caption         =   "Alt / Az Slewing"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   270
         TabIndex        =   26
         Top             =   630
         Width           =   1425
      End
      Begin VB.CheckBox chkCanUnpark 
         BackColor       =   &H00000000&
         Caption         =   "Unparking"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   2610
         TabIndex        =   36
         Top             =   1170
         Width           =   1305
      End
      Begin VB.CheckBox chkCanOptics 
         BackColor       =   &H00000000&
         Caption         =   "Optics"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   2610
         TabIndex        =   40
         Top             =   2520
         Width           =   1890
      End
      Begin VB.CheckBox chkCanSetTracking 
         BackColor       =   &H00000000&
         Caption         =   "Track on / off Support"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   7290
         TabIndex        =   48
         Top             =   360
         Width           =   1995
      End
      Begin VB.CheckBox chkCanAlignMode 
         BackColor       =   &H00000000&
         Caption         =   "Alignment Mode"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   4950
         TabIndex        =   41
         Top             =   360
         Width           =   1905
      End
      Begin VB.CheckBox chkCanPark 
         BackColor       =   &H00000000&
         Caption         =   "Parking"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   2610
         TabIndex        =   34
         Top             =   630
         Width           =   1905
      End
      Begin VB.CheckBox chkCanSiderealTime 
         BackColor       =   &H00000000&
         Caption         =   "Sidereal Time"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   4995
         TabIndex        =   47
         Top             =   2520
         Width           =   1905
      End
      Begin VB.CheckBox chkCanDateTime 
         BackColor       =   &H00000000&
         Caption         =   "Date/Time (UTC)"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   4995
         TabIndex        =   46
         Top             =   2250
         Width           =   1905
      End
      Begin VB.CheckBox chkCanLatLongElev 
         BackColor       =   &H00000000&
         Caption         =   "Lat/Long/Elevation"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   2610
         TabIndex        =   39
         Top             =   2250
         Width           =   1905
      End
      Begin VB.CheckBox chkCanAltAz 
         BackColor       =   &H00000000&
         Caption         =   "Alt / Az Coordinates"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   270
         TabIndex        =   25
         Top             =   360
         Width           =   1905
      End
      Begin VB.CheckBox chkCanEqu 
         BackColor       =   &H00000000&
         Caption         =   "Equatorial Coordinates"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   270
         TabIndex        =   29
         Top             =   1710
         Width           =   1980
      End
      Begin VB.CheckBox chkCanSync 
         BackColor       =   &H00000000&
         Caption         =   "Equatorial Sync"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   270
         TabIndex        =   31
         Top             =   2250
         Width           =   2160
      End
      Begin VB.CheckBox chkCanSlewAsync 
         BackColor       =   &H00000000&
         Caption         =   "Equatorial Asynchronous"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   270
         TabIndex        =   32
         Top             =   2520
         Width           =   2085
      End
      Begin VB.CheckBox chkCanSlew 
         BackColor       =   &H00000000&
         Caption         =   "Equatorial Slewing"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   270
         TabIndex        =   30
         Top             =   1980
         Width           =   1905
      End
      Begin VB.Label Label7 
         BackColor       =   &H00000000&
         Caption         =   "Number of Axis for MoveAxis"
         ForeColor       =   &H00FFFFFF&
         Height          =   450
         Left            =   2700
         TabIndex        =   38
         Top             =   1530
         Width           =   1155
      End
      Begin VB.Label lblEquatorialSystem 
         BackColor       =   &H00000000&
         Caption         =   "Equatorial System:"
         ForeColor       =   &H00FFFFFF&
         Height          =   405
         Left            =   5040
         TabIndex        =   69
         Top             =   1530
         Width           =   795
      End
   End
   Begin VB.Frame Frame2 
      BackColor       =   &H00000000&
      Caption         =   "Mount Type"
      ForeColor       =   &H00FFFFFF&
      Height          =   1620
      Left            =   4005
      TabIndex        =   60
      Top             =   1215
      Width           =   2010
      Begin VB.OptionButton optAlign 
         BackColor       =   &H00000000&
         Caption         =   "Equatorial"
         ForeColor       =   &H00FFFFFF&
         Height          =   240
         Index           =   1
         Left            =   195
         TabIndex        =   12
         Top             =   705
         Width           =   1215
      End
      Begin VB.OptionButton optAlign 
         BackColor       =   &H00000000&
         Caption         =   "German equatorial"
         ForeColor       =   &H00FFFFFF&
         Height          =   240
         Index           =   2
         Left            =   195
         TabIndex        =   13
         Top             =   1080
         Width           =   1635
      End
      Begin VB.OptionButton optAlign 
         BackColor       =   &H00000000&
         Caption         =   "Alt-Azimuth"
         ForeColor       =   &H00FFFFFF&
         Height          =   240
         Index           =   0
         Left            =   195
         TabIndex        =   11
         Top             =   300
         Width           =   1215
      End
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "&Cancel"
      Height          =   345
      Left            =   8190
      TabIndex        =   22
      Top             =   3015
      Width           =   1230
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   345
      Left            =   8190
      TabIndex        =   24
      Top             =   3510
      Width           =   1230
   End
   Begin VB.Frame Frame1 
      BackColor       =   &H00000000&
      Caption         =   "Site Information"
      ForeColor       =   &H00FFFFFF&
      Height          =   1620
      Left            =   150
      TabIndex        =   56
      Top             =   1215
      Width           =   3270
      Begin VB.TextBox txtElevation 
         Height          =   315
         Left            =   1275
         TabIndex        =   6
         Text            =   "Text1"
         Top             =   1125
         Width           =   885
      End
      Begin VB.ComboBox cbNS 
         Height          =   315
         ItemData        =   "frmSetup.frx":1070
         Left            =   1275
         List            =   "frmSetup.frx":107A
         Style           =   2  'Dropdown List
         TabIndex        =   0
         Top             =   315
         Width           =   555
      End
      Begin VB.ComboBox cbEW 
         Height          =   315
         ItemData        =   "frmSetup.frx":1084
         Left            =   1275
         List            =   "frmSetup.frx":108E
         Style           =   2  'Dropdown List
         TabIndex        =   3
         Top             =   720
         Width           =   555
      End
      Begin VB.TextBox txtLongMin 
         Height          =   315
         Left            =   2505
         TabIndex        =   5
         Text            =   "Text1"
         Top             =   720
         Width           =   570
      End
      Begin VB.TextBox txtLongDeg 
         Height          =   315
         Left            =   1920
         TabIndex        =   4
         Text            =   "Text1"
         Top             =   720
         Width           =   480
      End
      Begin VB.TextBox txtLatMin 
         Height          =   315
         Left            =   2505
         TabIndex        =   2
         Text            =   "Text1"
         Top             =   315
         Width           =   570
      End
      Begin VB.TextBox txtLatDeg 
         Height          =   315
         Left            =   1920
         TabIndex        =   1
         Text            =   "Text1"
         Top             =   315
         Width           =   480
      End
      Begin VB.Label Label4 
         BackColor       =   &H00000000&
         Caption         =   "Elevation (m):"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   180
         TabIndex        =   59
         Top             =   1155
         Width           =   990
      End
      Begin VB.Label Label3 
         BackColor       =   &H00000000&
         Caption         =   "Latitude:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Left            =   165
         TabIndex        =   58
         Top             =   360
         Width           =   690
      End
      Begin VB.Label Label2 
         BackColor       =   &H00000000&
         Caption         =   "Longitude:"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   180
         TabIndex        =   57
         Top             =   765
         Width           =   765
      End
   End
   Begin VB.Label Label1 
      BackColor       =   &H00000000&
      Caption         =   "Slew Rate (deg/sec)"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   1395
      TabIndex        =   64
      Top             =   3555
      Width           =   1875
   End
   Begin VB.Label lblDriverInfo 
      BackColor       =   &H00000000&
      Caption         =   "<run time - version etc.>"
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   180
      TabIndex        =   61
      Top             =   3960
      Width           =   5580
   End
   Begin VB.Label lblTimeZone 
      Alignment       =   1  'Right Justify
      BackColor       =   &H00000000&
      Caption         =   "<run time - time zone and UTC offset>"
      ForeColor       =   &H00FFFFFF&
      Height          =   285
      Left            =   5700
      TabIndex        =   55
      Top             =   3960
      Width           =   3705
   End
   Begin VB.Image imgSpaceSoftware 
      Height          =   1455
      Left            =   60
      MouseIcon       =   "frmSetup.frx":1098
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":11EA
      Top             =   -45
      Width           =   4140
   End
End
Attribute VB_Name = "frmSetup"
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
'   ============
'   FRMSETUP.FRM
'   ============
'
' ASCOM Scope Simulator setup form
'
' Written:  28-Jun-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 28-Jun-00 rbd     Initial edit
' 28-Jan-01 rbd     SPACE Software logo, capabilities checkboxes.
' 05-Feb-01 rbd     Add "Always on top" option, FloatWindow() now takes
'                   boolean to turn on or off. Add Advanced/Basic control
'                   to hide/show the capabilities checkboxes, default to
'                   checked (always on top). Fix phantom unloading during
'                   Setup call, caused run time errors due to junk in
'                   form fields.
' 13-Mar-01 rbd     Add tracking and optics capability checkboxes
' 02-Jan-02 rbd     1.4.4 Prevent type mismatch errors by formatting elevation
'                   zero as '0' not ''
' 27-Jul-02 rbd     ASCOM logo and hot link
' 04-Oct-02 mb      2.1.0 Lat/Long E/W and N/S combo boxes fixed for south/east
' 11-Oct-02 rbd     2.1.1 Slew Speed, change "Alignment Mode" to "Mount Type"
' 08-Mar-03 jab     added focal length and aperture, then desensitize when
'                   capabilities unchecked
' 04-Mar-04 rbd     improved error handling for text fields
' 05-Mar-04 jab     added all V2 features, and completed V1
' 01-Dec-04 rbd     4.0.2 - Relabel AutoTrack checkbox for clarity, move
'                   version, time zone etc. to above Interface Capabilities
'                   so they show in Basic form mode.
' 24-Jul-06 rbd     4.0.5 - Disconnect on park
' -----------------------------------------------------------------------------
Option Explicit

Private m_bResult As Boolean
Private m_bAllowUnload As Boolean
Private m_bAdvancedMode As Boolean

' ======
' EVENTS
' ======

Private Sub Form_Load()
    Dim tzName As String
    Dim l As Long
    Dim fs, F
    Dim DLM As String
    
    FloatWindow Me.hWnd, True                       ' Setup window always floats
    m_bAllowUnload = True                           ' Start out allowing unload

    ' Make real time zone label
    tzName = String$(256, Chr$(0))
    l = tz_name(tzName, DATE_LOCALTZ)               ' Get TZ name, DST flag
    tzName = Left$(tzName, InStr(tzName, Chr$(0)))  ' Trim the TZ name
    lblTimeZone = "Time zone is " & tzName
    If l <> 0 Then _
        lblTimeZone = lblTimeZone & " (currently DST)"
    
    ' Get time last modified
    Set fs = CreateObject("Scripting.FileSystemObject")
    Set F = fs.GetFile(App.Path & "\" & "ScopeSim.exe")
    DLM = F.DateLastModified
    
    lblDriverInfo = App.FileDescription & _
                " Version " & App.Major & "." & _
                App.Minor & "." & App.Revision & "  Modified: " & DLM
    
    setdeps
    
End Sub

Private Sub Form_Unload(Cancel As Integer)

    Me.Hide                                     ' Assure we don't unload
    Cancel = Not m_bAllowUnload                 ' Unless our flag permits it
    
End Sub

Private Sub chkCanSlew_Click()

    setdeps

End Sub

Private Sub chkCanOptics_Click()

    setdeps

End Sub

Private Sub chkCanLatLongElev_Click()

    setdeps

End Sub

Private Sub chkV1_Click()

    setdeps

End Sub

Private Sub chkCanPulseGuide_Click()

    setdeps

End Sub

Private Sub chkCanSetPark_Click()

    setdeps

End Sub

Private Sub chkCanDoesRefraction_Click()

    setdeps

End Sub

Private Sub setdeps()

    Dim V2 As Boolean
    Dim CanOptics As Boolean
    Dim CanLatLongElev As Boolean
    Dim CanPulseGuide As Boolean
    Dim CanSetPark As Boolean
    Dim CanDoesRefraction As Boolean

    V2 = (chkV1.Value = 0)
    CanOptics = (chkCanOptics.Value = 1)
    CanLatLongElev = (chkCanLatLongElev.Value = 1)
    CanPulseGuide = (chkCanPulseGuide.Value = 1)
    CanSetPark = (chkCanSetPark.Value = 1)
    CanDoesRefraction = (chkCanDoesRefraction.Value = 1)
        
    cbNS.Enabled = CanLatLongElev
    txtLatDeg.Enabled = CanLatLongElev
    txtLatMin.Enabled = CanLatLongElev
    cbEW.Enabled = CanLatLongElev
    txtLongDeg.Enabled = CanLatLongElev
    txtLongMin.Enabled = CanLatLongElev
    txtElevation.Enabled = CanLatLongElev
    
    ApertureTF.Enabled = CanOptics
    FocalLengthTF.Enabled = CanOptics
    ApertureAreaTF.Enabled = V2 And CanOptics
    
    chkCanSlewAltAz.Enabled = V2
    chkCanSyncAltAz.Enabled = V2
    chkCanTrackingRates.Enabled = V2
    chkCanSOP.Enabled = V2
    chkCanSetSOP.Enabled = V2
    chkCanSetGuideRates.Enabled = V2
    chkCanDoesRefraction.Enabled = V2
    chkDualAxisPulseGuide.Enabled = V2 And CanPulseGuide
    
    chkDoRefraction.Enabled = V2 And CanDoesRefraction
    
    cmdSetParkPosition.Enabled = CanSetPark
    cmdResetParkPosition.Enabled = CanSetPark

End Sub


Private Sub cmdOK_Click()

    m_bResult = True
    Me.Hide

End Sub

Private Sub cmdCancel_Click()

    m_bResult = False
    Me.Hide

End Sub

Private Sub cmdAdvanced_Click()

    SetFormMode Not m_bAdvancedMode             ' Toggle form mode
    
End Sub

Private Sub imgSpaceSoftware_Click()

    DisplayWebPage "http://www.starrynight.com/"
    
End Sub

Private Sub picASCOM_Click()

    DisplayWebPage "http://ascom-standards.org/"
    
End Sub

Private Sub cmdHelp_Click()

    DisplayWebPage App.Path & "/ScopeSimHelp.htm"
        
End Sub

' =================
' PUBLIC PROPERTIES
' =================

Public Property Get Result() As Boolean

    Result = m_bResult              ' Set by OK or Cancel button
    
End Property

Public Property Let Latitude(ByVal lat As Double)
    Dim u As Integer
    
    If lat < 0 Then
        lat = -lat
        cbNS.ListIndex = 1
    Else
        cbNS.ListIndex = 0
    End If
    u = Fix(lat)                    ' Degrees
    txtLatDeg = Format$(u, "00")
    lat = (lat - u) * 60            ' Minutes
    txtLatMin = Format$(lat, "00.00")

End Property

Public Property Get Latitude() As Double

    On Error Resume Next
    Latitude = CDbl(txtLatDeg) + (CDbl(txtLatMin) / 60#)
    If cbNS.Text = "S" Then Latitude = -Latitude
    On Error GoTo 0

End Property

Public Property Let Longitude(ByVal lon As Double)
    Dim u As Integer
    
    If lon < 0 Then
        lon = -lon
        cbEW.ListIndex = 1
    Else
        cbEW.ListIndex = 0
    End If
    u = Fix(lon)                    ' Degrees
    txtLongDeg = Format$(u, "000")
    lon = (lon - u) * 60            ' Minutes
    txtLongMin = Format$(lon, "00.00")
    
End Property

Public Property Get Longitude() As Double

    On Error Resume Next
    Longitude = CDbl(txtLongDeg) + (CDbl(txtLongMin) / 60#)
    If cbEW.Text = "W" Then Longitude = -Longitude  ' W is neg
    On Error GoTo 0
        
End Property

Public Property Let Elevation(el As Double)

    txtElevation = Format$(el, "0")     ' No decimal digits
    
End Property

Public Property Get Elevation() As Double

    On Error Resume Next
    Elevation = CDbl(txtElevation)
    On Error GoTo 0
    
End Property

Public Property Let Aperture(newVal As Double)

    ' do the same error handling for lat/long/elev ???
    
    If newVal <= 0 Then
        ApertureTF = "N/A"
    Else
        ApertureTF = Format$(newVal, "0.000")
    End If
    
End Property

Public Property Get Aperture() As Double
    
    Aperture = INVALID_PARAMETER
    
    If ApertureTF.Text = "N/A" Or ApertureTF.Text = "" Then _
        Exit Function
    
    On Error Resume Next
        Aperture = CDbl(ApertureTF)
    On Error GoTo 0
    
    If Aperture <= 0 Then
        Aperture = INVALID_PARAMETER
        MsgBox "Warning, Invalid Aperture Diameter", vbExclamation
    End If
    
End Property

Public Property Let ApertureArea(newVal As Double)

    If newVal <= 0 Then
        ApertureAreaTF = "N/A"
    Else
        ApertureAreaTF = Format$(newVal, "0.000000")
    End If
    
End Property

Public Property Get ApertureArea() As Double

    ApertureArea = INVALID_PARAMETER
    
    If ApertureAreaTF.Text = "N/A" Or ApertureAreaTF.Text = "" Then _
        Exit Function
    
    On Error Resume Next
        ApertureArea = CDbl(ApertureAreaTF)
    On Error GoTo 0
    
    If ApertureArea <= 0 Then
        ApertureArea = INVALID_PARAMETER
        MsgBox "Warning, Invalid Aperture Area", vbExclamation
    End If
    
End Property

Public Property Let FocalLength(newVal As Double)

    If newVal <= 0 Then
        FocalLengthTF = "N/A"
    Else
        FocalLengthTF = Format$(newVal, "0.000")
    End If
    
End Property

Public Property Get FocalLength() As Double
    
    FocalLength = INVALID_PARAMETER
    
    If FocalLengthTF.Text = "N/A" Or FocalLengthTF.Text = "" Then _
        Exit Function
    
    On Error Resume Next
        FocalLength = CDbl(FocalLengthTF)
    On Error GoTo 0
    
    If FocalLength <= 0 Then
        FocalLength = INVALID_PARAMETER
        MsgBox "Warning, Invalid Focal Length", vbExclamation
    End If
        
End Property

Public Property Let AlignMode(Mode As AlignmentModes)

    If Mode = algAltAz Then
        optAlign(0).Value = True
    ElseIf Mode = algPolar Then
        optAlign(1).Value = True
    Else
        optAlign(2).Value = True
    End If
    
End Property

Public Property Get AlignMode() As AlignmentModes

    If optAlign(0).Value Then
        AlignMode = algAltAz
    ElseIf optAlign(1).Value Then
        AlignMode = algPolar
    Else
        AlignMode = algGermanPolar
    End If
    
End Property

Public Property Let AllowUnload(b As Boolean)

    m_bAllowUnload = b
    
End Property

Public Property Get AdvancedMode() As Boolean

    AdvancedMode = m_bAdvancedMode
    
End Property

Public Property Let AdvancedMode(b As Boolean)

    SetFormMode b
    
End Property

Private Sub cmdResetParkPosition_Click()

    If g_dLatitude >= 0 Then
        g_dParkAzimuth = 180#
    Else
        g_dParkAzimuth = 0#
    End If
    
    g_dParkAltitude = 90# - Abs(g_dLatitude)
        
End Sub

Private Sub cmdSetParkPosition_Click()

    g_dParkAzimuth = g_dAzimuth
    g_dParkAltitude = g_dAltitude
    
End Sub

'
' LOCAL UTILITIES
'
Private Sub SetFormMode(Advanced As Boolean)

    If Advanced Then                        ' Basic display now
        Me.Height = 7650
        Me.cmdAdvanced.Caption = "<< &Basic"
    Else
        Me.Height = 4540
        Me.cmdAdvanced.Caption = "&Advanced >>"
    End If
    m_bAdvancedMode = Advanced
    
End Sub

