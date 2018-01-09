VERSION 5.00
Begin VB.Form frmSetup 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Dome and Telescope Setup"
   ClientHeight    =   4530
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   6000
   Icon            =   "frmSetup.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4530
   ScaleWidth      =   6000
   ShowInTaskbar   =   0   'False
   Begin VB.CheckBox AutoHomeCheck 
      Caption         =   "Automatic home on first connection"
      Height          =   375
      Left            =   2880
      TabIndex        =   24
      Top             =   120
      Width           =   1695
   End
   Begin VB.CommandButton SetupDome 
      Caption         =   "Setup Dome"
      Height          =   375
      Left            =   1440
      TabIndex        =   23
      Top             =   120
      Width           =   1230
   End
   Begin VB.CheckBox HomeCheck 
      Caption         =   "Sync Home Azimuth To"
      Height          =   255
      Left            =   3915
      TabIndex        =   21
      Top             =   1230
      Width           =   1995
   End
   Begin VB.CommandButton SetupScope 
      Caption         =   "Setup Scope"
      Height          =   375
      Left            =   1440
      TabIndex        =   20
      Top             =   600
      Width           =   1230
   End
   Begin VB.CheckBox GermanCheck 
      Caption         =   "German Equatorial"
      Height          =   255
      Left            =   2880
      TabIndex        =   19
      Top             =   660
      Width           =   1695
   End
   Begin VB.CommandButton SelectTelescope 
      Caption         =   "Select Scope"
      Height          =   375
      Left            =   105
      TabIndex        =   18
      Top             =   600
      Width           =   1230
   End
   Begin VB.TextBox HomeAzimuth 
      Height          =   315
      Left            =   3915
      TabIndex        =   17
      Text            =   "Text1"
      Top             =   1560
      Width           =   825
   End
   Begin VB.CommandButton SelectDome 
      Caption         =   "Select Dome"
      Height          =   375
      Left            =   120
      TabIndex        =   16
      Top             =   120
      Width           =   1230
   End
   Begin VB.TextBox DomeParam 
      Height          =   315
      Index           =   6
      Left            =   120
      TabIndex        =   14
      Top             =   4080
      Width           =   975
   End
   Begin VB.TextBox DomeParam 
      Height          =   315
      Index           =   5
      Left            =   120
      TabIndex        =   12
      Top             =   3600
      Width           =   975
   End
   Begin VB.TextBox DomeParam 
      Height          =   315
      Index           =   4
      Left            =   120
      TabIndex        =   10
      Top             =   3120
      Width           =   975
   End
   Begin VB.TextBox DomeParam 
      Height          =   315
      Index           =   3
      Left            =   120
      TabIndex        =   8
      Top             =   2610
      Width           =   975
   End
   Begin VB.TextBox DomeParam 
      Height          =   315
      Index           =   2
      Left            =   120
      TabIndex        =   6
      Top             =   2130
      Width           =   975
   End
   Begin VB.TextBox DomeParam 
      Height          =   315
      Index           =   1
      Left            =   120
      TabIndex        =   4
      Top             =   1680
      Width           =   975
   End
   Begin VB.TextBox DomeParam 
      Height          =   315
      Index           =   0
      Left            =   120
      TabIndex        =   2
      Top             =   1200
      Width           =   975
   End
   Begin VB.CommandButton CancelButton 
      Caption         =   "Cancel"
      Height          =   375
      Left            =   4680
      TabIndex        =   1
      Top             =   600
      Width           =   1215
   End
   Begin VB.CommandButton OKButton 
      Caption         =   "OK"
      Height          =   375
      Left            =   4680
      TabIndex        =   0
      Top             =   120
      Width           =   1215
   End
   Begin VB.Label Label2 
      Caption         =   "Degrees"
      Height          =   225
      Left            =   4995
      TabIndex        =   22
      Top             =   1605
      Width           =   720
   End
   Begin VB.Label Label1 
      Caption         =   "Distance from mount pivot point to telescope optical axis, inches"
      Height          =   255
      Index           =   6
      Left            =   1200
      TabIndex        =   15
      Top             =   4110
      Width           =   4575
   End
   Begin VB.Label Label1 
      Caption         =   "Height of mount pivot point above dome equator, inches"
      Height          =   255
      Index           =   5
      Left            =   1200
      TabIndex        =   13
      Top             =   3630
      Width           =   4215
   End
   Begin VB.Label Label1 
      Caption         =   "Distance from dome center to mount pivot point, inches, north is +"
      Height          =   255
      Index           =   4
      Left            =   1200
      TabIndex        =   11
      Top             =   3150
      Width           =   4695
   End
   Begin VB.Label Label1 
      Caption         =   "Distance from dome center to scope pivot point, inches, east is +"
      Height          =   255
      Index           =   3
      Left            =   1200
      TabIndex        =   9
      Top             =   2640
      Width           =   4695
   End
   Begin VB.Label Label1 
      Caption         =   "Longitude, degrees, east is +"
      Height          =   255
      Index           =   2
      Left            =   1200
      TabIndex        =   7
      Top             =   1710
      Width           =   2130
   End
   Begin VB.Label Label1 
      Caption         =   "Latitiude, degrees, north is +"
      Height          =   255
      Index           =   1
      Left            =   1200
      TabIndex        =   5
      Top             =   1230
      Width           =   2115
   End
   Begin VB.Label Label1 
      Caption         =   "Radius of dome at equator, inches"
      Height          =   255
      Index           =   0
      Left            =   1200
      TabIndex        =   3
      Top             =   2160
      Width           =   2655
   End
End
Attribute VB_Name = "frmSetup"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'---------------------------------------------------------------------
' Copyright © 2003 Diffraction Limited
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". DIFFRACTION LIMITED. MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'
'   =============
'   Telescope.cls
'   =============
'
' Written:  2003/06/24   Douglas B. George <dgeorge@cyanogen.com>
'
' Edits:
'
' When       Who     What
' ---------  ---     --------------------------------------------------
' 2003/06/24 dbg     Initial edit
' 2004/11/15 dbg     Fix loading of Sync Home To Azimuth from registry
'                    and allow changing setup while connected except for
'                    dome and telescope selection
' -----------------------------------------------------------------------------

Option Explicit

Const DP_LAT = 0
Const DP_LON = 1
Const DP_RADIUS = 2
Const DP_EAST = 3
Const DP_NORTH = 4
Const DP_HEIGHT = 5
Const DP_OPTICAL = 6

Private Sub CancelButton_Click()
    Me.Hide
End Sub


Private Sub Form_Activate()
    LoadData
    SetControls
End Sub

Private Sub HomeCheck_Click()
    SetControls
End Sub

Private Sub ParkCheck_Click()
    SetControls
End Sub

Private Sub SelectTelescope_Click()
    Dim chsr As New DriverHelper.Chooser
    ' Retrieve the ProgID of the previously chosen
    ' telescope, or set it to ""
    Dim ScopeProgID As String
    ScopeProgID = Profile.GetValue(DOMEID, "Scope")
    ScopeProgID = chsr.Choose(ScopeProgID)
    If ScopeProgID <> "" Then Profile.WriteValue DOMEID, "Scope", ScopeProgID
End Sub

Private Sub SelectDome_Click()
    Dim chsr As New DriverHelper.Chooser
    ' Retrieve the ProgID of the previously chosen
    ' dome, or set it to ""
    Dim DomeProgID As String
    DomeProgID = Profile.GetValue(DOMEID, "Dome")
    chsr.DeviceType = "Dome"
    DomeProgID = chsr.Choose(DomeProgID)
    If DomeProgID <> "" Then Profile.WriteValue DOMEID, "Dome", DomeProgID
End Sub

Private Sub SetupDome_Click()
    Dim DomeProgID As String
    DomeProgID = Profile.GetValue(DOMEID, "Dome")
    If Len(DomeProgID) = 0 Then
        Err.Raise SCODE_NOT_SELECTED, ERR_SOURCE, MSG_NOT_SELECTED
        frmMain.UpdateStatus
        Exit Sub
    End If
    On Error Resume Next
    Dim ADome As Object
    Set ADome = CreateObject(DomeProgID)
    ADome.SetupDialog
    Set ADome = Nothing
End Sub

Private Sub SetupScope_Click()
    Dim ScopeProgID As String
    ScopeProgID = Profile.GetValue(DOMEID, "Scope")
    If Len(ScopeProgID) = 0 Then
        Err.Raise SCODE_NOT_SELECTED, ERR_SOURCE, MSG_NOT_SELECTED
        frmMain.UpdateStatus
        Exit Sub
    End If
    On Error Resume Next
    Dim AScope As Object
    Set AScope = CreateObject(ScopeProgID)
    AScope.SetupDialog
    Set AScope = Nothing
End Sub

Public Sub LoadData()
    On Error Resume Next
    
    ' Defaults
    DomeParam(DP_LAT).Text = 45 ' Approximate location of Ottawa, Canada
    DomeParam(DP_LON).Text = 75
    DomeParam(DP_RADIUS).Text = 72
    DomeParam(DP_EAST).Text = 0
    DomeParam(DP_NORTH).Text = 0
    DomeParam(DP_HEIGHT).Text = 12
    DomeParam(DP_OPTICAL).Text = 0
    
    DomeParam(DP_LAT).Text = Profile.GetValue(DOMEID, "DomeLatitude")
    DomeParam(DP_LON).Text = Profile.GetValue(DOMEID, "DomeLongitude")
    DomeParam(DP_RADIUS).Text = Profile.GetValue(DOMEID, "DomeRadius")
    DomeParam(DP_EAST).Text = Profile.GetValue(DOMEID, "OffsetEast")
    DomeParam(DP_NORTH).Text = Profile.GetValue(DOMEID, "OffsetNorth")
    DomeParam(DP_HEIGHT).Text = Profile.GetValue(DOMEID, "OffsetHeight")
    DomeParam(DP_OPTICAL).Text = Profile.GetValue(DOMEID, "OffsetOptical")
    HomeAzimuth.Text = Profile.GetValue(DOMEID, "HomeAzimuth")
    
    If Len(HomeAzimuth.Text) = 0 Then HomeAzimuth.Text = "0"
    
    Dim i
    For i = 0 To DomeParam.Count - 1
        If Len(DomeParam(i).Text) = 0 Then
            Select Case i
            Case 0
                DomeParam(i).Text = 45
            Case 1
                DomeParam(i).Text = -75
            Case 2
                DomeParam(i).Text = 72
            Case Else
                DomeParam(i).Text = 0
            End Select
        End If
    Next
    
    ' Transfer to dome calculations
    DomeRadius = DomeParam(DP_RADIUS)
    Latitude = DomeParam(DP_LAT)
    Longitude = DomeParam(DP_LON)
    OffsetEast = DomeParam(DP_EAST)
    OffsetNorth = DomeParam(DP_NORTH)
    OffsetHeight = DomeParam(DP_HEIGHT)
    OffsetOptical = DomeParam(DP_OPTICAL)
    
    ' Do same for check boxes
    GermanCheck.Value = Profile.GetValue(DOMEID, "IsGerman")
    IsGerman = GermanCheck.Value
    HomeCheck.Value = Profile.GetValue(DOMEID, "HomeAzimuthCheck")
    UseHomeAzimuth = HomeCheck.Value
    AutoHomeCheck.Value = Profile.GetValue(DOMEID, "AutoHomeCheck")
    AutoHome = AutoHomeCheck.Value
    
End Sub

Private Sub Form_Load()
    LoadData
    SetControls
End Sub

Private Sub SetControls()
    HomeAzimuth.Enabled = HomeCheck.Value
    SelectDome.Enabled = Not DomeIsConnected
    SelectTelescope.Enabled = Not DomeIsConnected
End Sub

Private Sub OKButton_Click()
    
    ' Force input to be numerical
    Dim i As Integer
    Dim Temp As Double
    For i = 0 To DomeParam.Count - 1
        Temp = val(DomeParam(i).Text)
        DomeParam(i).Text = Temp
    Next

    Temp = val(HomeAzimuth.Text)
    HomeAzimuth.Text = Temp
    
    IsGerman = GermanCheck.Value <> 0
    UseHomeAzimuth = HomeCheck.Value <> 0
    AutoHome = AutoHomeCheck.Value <> 0

    Profile.WriteValue DOMEID, "DomeLatitude", DomeParam(DP_LAT).Text
    Profile.WriteValue DOMEID, "DomeLongitude", DomeParam(DP_LON).Text
    Profile.WriteValue DOMEID, "DomeRadius", DomeParam(DP_RADIUS).Text
    Profile.WriteValue DOMEID, "OffsetEast", DomeParam(DP_EAST).Text
    Profile.WriteValue DOMEID, "OffsetNorth", DomeParam(DP_NORTH).Text
    Profile.WriteValue DOMEID, "OffsetHeight", DomeParam(DP_HEIGHT).Text
    Profile.WriteValue DOMEID, "OffsetOptical", DomeParam(DP_OPTICAL).Text
    Profile.WriteValue DOMEID, "IsGerman", GermanCheck.Value
    Profile.WriteValue DOMEID, "HomeAzimuth", HomeAzimuth.Text
    Profile.WriteValue DOMEID, "HomeAzimuthCheck", HomeCheck.Value
    Profile.WriteValue DOMEID, "AutoHomeCheck", AutoHomeCheck.Value

    DomeRadius = DomeParam(DP_RADIUS)
    Latitude = DomeParam(DP_LAT)
    Longitude = DomeParam(DP_LON)
    OffsetEast = DomeParam(DP_EAST)
    OffsetNorth = DomeParam(DP_NORTH)
    OffsetHeight = DomeParam(DP_HEIGHT)
    OffsetOptical = DomeParam(DP_OPTICAL)
    
    Me.Hide
End Sub
