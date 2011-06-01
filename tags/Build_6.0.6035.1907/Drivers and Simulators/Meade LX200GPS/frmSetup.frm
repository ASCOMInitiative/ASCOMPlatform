VERSION 5.00
Begin VB.Form frmSetup 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "ASCOM Meade LX200GPS/R Setup"
   ClientHeight    =   7455
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   6015
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   7455
   ScaleWidth      =   6015
   Begin VB.Frame Frame3 
      Caption         =   "Parked Behavior"
      Height          =   1245
      Left            =   135
      TabIndex        =   41
      Top             =   2895
      Width           =   5730
      Begin VB.OptionButton optParkedBehavior 
         Alignment       =   1  'Right Justify
         Caption         =   "Report Coordinates As"
         Height          =   255
         Index           =   1
         Left            =   210
         TabIndex        =   12
         Top             =   570
         Width           =   2310
      End
      Begin VB.OptionButton optParkedBehavior 
         Alignment       =   1  'Right Justify
         Caption         =   "Last Good Position"
         Height          =   255
         Index           =   2
         Left            =   210
         TabIndex        =   13
         Top             =   870
         Width           =   2310
      End
      Begin VB.OptionButton optParkedBehavior 
         Alignment       =   1  'Right Justify
         Caption         =   "No Coordinates"
         Height          =   255
         Index           =   0
         Left            =   210
         TabIndex        =   11
         Top             =   270
         Width           =   2310
      End
      Begin VB.TextBox txtParkAlt 
         Height          =   285
         Left            =   2970
         TabIndex        =   14
         Top             =   555
         Width           =   795
      End
      Begin VB.TextBox txtParkAz 
         Height          =   285
         Left            =   4305
         TabIndex        =   15
         Top             =   555
         Width           =   795
      End
      Begin VB.Label Label10 
         Caption         =   "Altitude (deg)"
         Height          =   225
         Left            =   2970
         TabIndex        =   43
         Top             =   270
         Width           =   1035
      End
      Begin VB.Label Label11 
         Caption         =   "Azimuth (deg)"
         Height          =   225
         Left            =   4305
         TabIndex        =   42
         Top             =   270
         Width           =   1005
      End
   End
   Begin VB.CheckBox chkAutoTrack 
      Caption         =   "AutoTrack On Connect"
      Height          =   300
      Left            =   3390
      TabIndex        =   7
      ToolTipText     =   "On connect - Make sure scope is tracking"
      Top             =   1050
      Width           =   2250
   End
   Begin VB.TextBox txtMaxSlew 
      Height          =   285
      Left            =   4695
      TabIndex        =   40
      ToolTipText     =   "1-5  or A or N (see help)"
      Top             =   2370
      Width           =   300
   End
   Begin VB.CheckBox chkSyncDelay 
      Caption         =   "Delay After Sync"
      Height          =   300
      Left            =   3390
      TabIndex        =   9
      ToolTipText     =   "Adds a 5 second delay after SYNC for coordinate update"
      Top             =   1928
      Width           =   2280
   End
   Begin VB.CheckBox chkAutoReboot 
      Caption         =   "Auto Reboot / Init / Unpark"
      Height          =   300
      Left            =   3390
      TabIndex        =   5
      ToolTipText     =   "On connect - autmatically reboot LX200GPS"
      Top             =   172
      Width           =   2355
   End
   Begin VB.CheckBox chkBeep 
      Caption         =   "Beep at slew completion"
      Height          =   300
      Left            =   3390
      TabIndex        =   8
      ToolTipText     =   "PC will beep on slew completion"
      Top             =   1489
      Width           =   2280
   End
   Begin VB.CheckBox chkAutoSetTime 
      Caption         =   "Auto Set Time"
      Height          =   300
      Left            =   3390
      TabIndex        =   6
      ToolTipText     =   "On connect - set time to PC clock"
      Top             =   611
      Width           =   2250
   End
   Begin VB.TextBox txtApertureArea 
      Height          =   285
      Left            =   1950
      TabIndex        =   3
      Top             =   1950
      Width           =   975
   End
   Begin VB.CommandButton cmdHelp 
      Caption         =   "&Help"
      Height          =   345
      Left            =   2535
      TabIndex        =   22
      Top             =   6960
      Width           =   990
   End
   Begin VB.TextBox txtElevation 
      Height          =   285
      Left            =   1935
      TabIndex        =   1
      Top             =   1095
      Width           =   975
   End
   Begin VB.TextBox txtFocalLength 
      Height          =   285
      Left            =   1950
      TabIndex        =   4
      Top             =   2370
      Width           =   975
   End
   Begin VB.TextBox txtAperture 
      Height          =   285
      Left            =   1950
      TabIndex        =   2
      Top             =   1515
      Width           =   975
   End
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   135
      MouseIcon       =   "frmSetup.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":0152
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   34
      Top             =   150
      Width           =   720
   End
   Begin VB.PictureBox picIcon 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   480
      Left            =   135
      Picture         =   "frmSetup.frx":1016
      ScaleHeight     =   480
      ScaleWidth      =   480
      TabIndex        =   32
      Top             =   6015
      Width           =   480
   End
   Begin VB.Frame Frame1 
      Caption         =   "Focuser Basics"
      Height          =   1500
      Left            =   135
      TabIndex        =   30
      Top             =   4410
      Width           =   2865
      Begin VB.TextBox txtMaxIncrement 
         Height          =   315
         Left            =   1560
         TabIndex        =   16
         Text            =   "Text1"
         Top             =   285
         Width           =   975
      End
      Begin VB.CheckBox chkFlip 
         Caption         =   "Reverse In/Out"
         Height          =   225
         Left            =   225
         TabIndex        =   17
         Top             =   705
         Width           =   1770
      End
      Begin VB.CheckBox chkDynamicBraking 
         Caption         =   "Dynamic Braking"
         Height          =   225
         Left            =   225
         TabIndex        =   18
         Top             =   1065
         Width           =   1815
      End
      Begin VB.Label Label2 
         Alignment       =   1  'Right Justify
         Caption         =   "Max Increment:"
         Height          =   225
         Left            =   165
         TabIndex        =   31
         Top             =   345
         Width           =   1155
      End
   End
   Begin VB.Frame lblBacklash 
      Caption         =   "Focuser Backlash Compensation"
      Height          =   1515
      Left            =   3120
      TabIndex        =   26
      Top             =   4410
      Width           =   2745
      Begin VB.Frame Frame2 
         Caption         =   "Final Dir."
         Height          =   975
         Left            =   150
         TabIndex        =   28
         Top             =   360
         Width           =   945
         Begin VB.OptionButton optFinalDirectionOut 
            Alignment       =   1  'Right Justify
            Caption         =   "Out"
            Height          =   255
            Left            =   135
            TabIndex        =   20
            Top             =   540
            Width           =   615
         End
         Begin VB.OptionButton optFinalDirectionIn 
            Alignment       =   1  'Right Justify
            Caption         =   "In"
            Height          =   255
            Left            =   135
            TabIndex        =   19
            Top             =   300
            Width           =   615
         End
      End
      Begin VB.TextBox txtBacklashSteps 
         Height          =   285
         Left            =   1680
         TabIndex        =   21
         Text            =   "0"
         Top             =   420
         Width           =   870
      End
      Begin VB.Label Label3 
         Alignment       =   2  'Center
         Caption         =   "Steps (ms)"
         Height          =   480
         Left            =   1125
         TabIndex        =   27
         Top             =   360
         Width           =   615
      End
   End
   Begin VB.ComboBox lbPort 
      Height          =   315
      ItemData        =   "frmSetup.frx":1458
      Left            =   1920
      List            =   "frmSetup.frx":1493
      Style           =   2  'Dropdown List
      TabIndex        =   0
      Top             =   165
      Width           =   985
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   345
      Left            =   4875
      TabIndex        =   24
      Top             =   6960
      Width           =   990
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   345
      Left            =   3690
      TabIndex        =   23
      Top             =   6960
      Width           =   990
   End
   Begin VB.Label Label8 
      Alignment       =   1  'Right Justify
      Caption         =   "Max Slew Speed:"
      Height          =   225
      Left            =   3300
      TabIndex        =   10
      Top             =   2400
      Width           =   1335
   End
   Begin VB.Label Label9 
      Caption         =   "(see help!)"
      Height          =   255
      Left            =   5040
      TabIndex        =   39
      Top             =   2400
      Width           =   810
   End
   Begin VB.Label Label7 
      Caption         =   "Aperture Area (m^2):"
      Height          =   225
      Left            =   420
      TabIndex        =   38
      Top             =   1980
      Width           =   1470
   End
   Begin VB.Label Label6 
      Alignment       =   1  'Right Justify
      Caption         =   "Elevation (m):"
      Height          =   225
      Left            =   855
      TabIndex        =   37
      Top             =   1125
      Width           =   1035
   End
   Begin VB.Label Label5 
      Alignment       =   1  'Right Justify
      Caption         =   "Focal Length (m):"
      Height          =   225
      Left            =   615
      TabIndex        =   36
      Top             =   2400
      Width           =   1275
   End
   Begin VB.Label Label4 
      Caption         =   "Aperture (m):"
      Height          =   225
      Left            =   990
      TabIndex        =   35
      Top             =   1545
      Width           =   915
   End
   Begin VB.Label lblDriverNote 
      Caption         =   "<runtime driver note>"
      Height          =   870
      Left            =   780
      TabIndex        =   33
      Top             =   6030
      Width           =   5100
   End
   Begin VB.Label lblDriverInfo 
      Caption         =   "<runtime driver info>"
      Height          =   345
      Left            =   165
      TabIndex        =   29
      Top             =   7020
      Width           =   2370
   End
   Begin VB.Label Label1 
      Caption         =   "Serial Port:"
      Height          =   225
      Left            =   1005
      TabIndex        =   25
      Top             =   225
      Width           =   795
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
'   =============
'   FRMSETUP.FRM
'   =============
'
' Setup form for ASCOM Meade telescope driver
'
' Written:  22-Aug-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 22-Aug-00 rbd     Initial edit
' 21-Jan-01 rbd     New Helper.Profile object for registry stuff
'                   No more General subkey
' ??-Feb-02 ??      For combined Telescope & Focuser (Sheets & Weber)
' 18-Jun-02 rbd     Move focuser-related settings to the Focuser
'                   section of the profile data. We use our own
'                   Profile object now. Remove "Focuser " from
'                   registry value names for Focuser, now stored
'                   in the Focuser section so no longer need that
'                   additional labelling. Error handling cleanups.
'                   Fix backlash compensation final direction to
'                   be aware of the "Reverse In/Out" setting.
' 24-Jul-02 rbd     1.8.7 - Add "Classic" alert on selecting dynamic
'                   braking
' 26-jul-02 rbd     2.0.1 - for ASCOM 2, ASCOM logo and hot lonk
' 08-mar-03 jab     support for focal length and aperture
' 11-mar-03 jab     added elevation to dialog
' 28-sep-03 jab     added control of auto unpark
' 25-oct-o3 jab     setup dialog now works while connected
' 24-Nov-04 rbd     4.0.2 - COM Ports to 16
' 10-Oct-05 rbd     4.1.7 - Moved help display code to Common.bas,
'                   force help page to display on first call to
'                   SetupDialog() after installation. Pop SetupDialog
'                   to top, but don't leave it topmost. On first use
'                   of SetupDialog(), force user to say yes/no to
'                   seeing the Help document. If he answers yes, then
'                   display it. Once he answers yes, clear the flag so
'                   that question will never again be asked.
' 13-Oct-05 rbd     4.1.8 - Add AutoReboot switch for LX200GPS, avoids
'                   long delays for other scope types if connecting to
'                   nothing or a dead scope.
' 07-Aug-06 rbd     4.1.13 - Add Max Slew Rate setting, Sync Delay
'                   checkbox
' 09-Aug-06 rbd     4.1.14 - Add "N" to leave the max-slew speed in the
'                   handbox. Add tool-tip to txtMaxSlew, fix tool-tips
'                   opn some checkboxes.
' 15-Sep-06 jab     4.1.15  Pealed out the LX200GPS into its own driver
'---------------------------------------------------------------------

Option Explicit

Private oProfile As DriverHelper.Profile
Public m_DriverID As String

Private m_bInLoad As Boolean

Private Sub Form_Load()
    Dim Port As Long
    Dim buf As String
    
    m_bInLoad = True
    
    Set oProfile = New DriverHelper.Profile
    
    '
    ' Enable or disable fields that are dependent on connect state
    '
    lbPort.Enabled = Not (g_bTelescopeConnected Or g_bFocuserConnected)
    txtMaxIncrement.Enabled = Not g_bFocuserConnected
    chkFlip.Enabled = Not g_bFocuserConnected
    chkDynamicBraking.Enabled = Not g_bFocuserConnected
    txtBacklashSteps.Enabled = Not g_bFocuserConnected
    optFinalDirectionIn.Enabled = Not g_bFocuserConnected
    optFinalDirectionOut.Enabled = Not g_bFocuserConnected
        
    '
    ' First do the Telescope items
    '
    oProfile.DeviceType = "Telescope"
    buf = oProfile.GetValue(SCOPE_ID, "COM Port")
    If buf = "" Then buf = "1"              ' Default COM1
    Port = CInt(buf)
    lbPort.ListIndex = Port - 1             ' Select current port
    
    On Error Resume Next
    buf = oProfile.GetValue(SCOPE_ID, "Aperture")
    txtAperture.Text = "User Input"
    If buf <> "" Then _
        txtAperture.Text = CStr(val(buf))
    
    buf = oProfile.GetValue(SCOPE_ID, "ApertureArea")
    txtApertureArea.Text = "User Input"
    If buf <> "" Then _
        txtApertureArea.Text = CStr(val(buf))
        
    buf = oProfile.GetValue(SCOPE_ID, "FocalLength")
    txtFocalLength.Text = "User Input"
    If buf <> "" Then _
        txtFocalLength.Text = CStr(val(buf))
    
    buf = oProfile.GetValue(SCOPE_ID, "SiteElevation")
    txtElevation.Text = "User Input"
    If buf <> "" Then _
        txtElevation.Text = CStr(val(buf))
    
    buf = oProfile.GetValue(SCOPE_ID, "Beep")
    If buf = "" Then buf = "1"
    chkBeep.Value = CLng(buf)
    
    buf = oProfile.GetValue(SCOPE_ID, "AutoTrack")
    If buf = "" Then buf = "0"
    chkAutoTrack.Value = CLng(buf)
    
    buf = oProfile.GetValue(SCOPE_ID, "AutoSetTime")
    If buf = "" Then buf = "0"
    chkAutoSetTime.Value = CLng(buf)
        
    buf = oProfile.GetValue(SCOPE_ID, "AutoReboot")
    If buf = "" Then buf = "1"
    chkAutoReboot.Value = CLng(buf)
        
    buf = oProfile.GetValue(SCOPE_ID, "SyncDelay")
    If buf = "" Then buf = "0"
    chkSyncDelay.Value = CLng(buf)
    
    buf = oProfile.GetValue(SCOPE_ID, "MaxSlew")
    txtMaxSlew.Text = "A"                                   ' "A" means "auto"
    If buf <> "" Then txtMaxSlew = buf
    
    buf = oProfile.GetValue(SCOPE_ID, "ParkedBehavior")
    If buf = "" Then buf = "2"
    optParkedBehavior(CInt(buf)).Value = True
    buf = oProfile.GetValue(SCOPE_ID, "ParkAlt")
    If buf = "" Then buf = "0"
    txtParkAlt = buf
    buf = oProfile.GetValue(SCOPE_ID, "ParkAz")
    If buf = "" Then buf = "180"
    txtParkAz = buf
    
    '
    ' Now do the Focuser items
    '
    oProfile.DeviceType = "Focuser"
    buf = oProfile.GetValue(FOCUSER_ID, "Max Increment")
    If buf = "" Then buf = "7000"
    txtMaxIncrement = buf

    buf = oProfile.GetValue(FOCUSER_ID, "Flip Dir")
    If buf = "" Then buf = "0"
    chkFlip.Value = CLng(buf)
    
    buf = oProfile.GetValue(FOCUSER_ID, "Dynamic Braking")
    If buf = "" Then buf = "1"
    chkDynamicBraking.Value = CLng(buf)
    
    buf = oProfile.GetValue(FOCUSER_ID, "Backlash Steps")
    If buf = "" Then buf = "3000"
    txtBacklashSteps = buf
    
    buf = oProfile.GetValue(FOCUSER_ID, "Final Direction")
    If buf = "" Then buf = "Out"
    If buf = "In" Then
        optFinalDirectionIn.Value = True
    Else
        optFinalDirectionOut.Value = True
    End If
    
    On Error GoTo 0
    
    '
    ' Set up the version display
    '
    lblDriverInfo = "Version " & App.Major & "." & _
                             App.Minor & "." & App.Revision

    '
    ' Finally, display the appropriate note for the driver flavor.
    '
    lblDriverNote.Caption = _
        "This driver supports the Meade LX200GPS and LX200R Telescopes."
       
    '
    ' Assure window pops up on top of others, but don't force it to top.
    '
    SetWindowPos Me.hWnd, HWND_TOPMOST, 0, 0, 0, 0, (SWP_NOMOVE + SWP_NOSIZE)
    DoEvents
    SetWindowPos Me.hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, (SWP_NOMOVE + SWP_NOSIZE)
    DoEvents
    m_bInLoad = False
    
    oProfile.DeviceType = "Telescope"
    If oProfile.GetValue(SCOPE_ID, "HelpAdvised") = "" Then      ' Never seen help advise
        If MsgBox("It is HIGHLY recommended that you read the Help for Meade LX200GPS / LX200R.  " & _
                  "Do you want to see the Help document now?", _
                  (vbYesNo + vbQuestion + vbMsgBoxSetForeground), _
                  "ASCOM Meade LX200GPS/R Driver") = vbYes Then
            ShowHelp
            oProfile.WriteValue SCOPE_ID, "HelpAdvised", "True"
        End If
    End If
    
End Sub

Private Sub cmdOK_Click()
    Dim buf As String
    Dim MaxIncrement As Long
    Dim val As Double
    
    '
    ' First, do the Telescope-related items
    '
    oProfile.DeviceType = "Telescope"
    oProfile.WriteValue SCOPE_ID, "COM Port", Str(lbPort.ListIndex + 1)
    
    ' don't force user to fill out field, do check for valid entry
    If txtAperture.Text = "User Input" Then
        oProfile.WriteValue SCOPE_ID, "Aperture", ""
    Else
        On Error Resume Next
            val = 0
            val = CDbl(txtAperture.Text)
        On Error GoTo 0
        If val > 0 Then
            oProfile.WriteValue SCOPE_ID, "Aperture", Str(val)
        Else
            MsgBox "You must enter a positive number for Aperture", _
                vbExclamation
            txtAperture.SetFocus                    ' Put cursor in this box
            Exit Sub
        End If
    End If
    
    ' don't force user to fill out field, do check for valid entry
    If txtApertureArea.Text = "User Input" Then
        oProfile.WriteValue SCOPE_ID, "ApertureArea", ""
    Else
        On Error Resume Next
            val = 0
            val = CDbl(txtApertureArea.Text)
        On Error GoTo 0
        If val > 0 Then
            oProfile.WriteValue SCOPE_ID, "ApertureArea", Str(val)
        Else
            MsgBox "You must enter a positive number for Aperture Area", _
                vbExclamation
            txtApertureArea.SetFocus                ' Put cursor in this box
            Exit Sub
        End If
    End If
    
    ' don't force user to fill out field, do check fo valid entry
    If txtFocalLength.Text = "User Input" Then
        oProfile.WriteValue SCOPE_ID, "FocalLength", ""
    Else
        On Error Resume Next
            val = 0
            val = CDbl(txtFocalLength.Text)
        On Error GoTo 0
        If val > 0 Then
            oProfile.WriteValue SCOPE_ID, "FocalLength", Str(val)
        Else
            MsgBox "You must enter a positive number for Focal Length", _
                vbExclamation
            txtFocalLength.SetFocus                  ' Put cursor in this box
            Exit Sub
        End If
    End If
    
    ' don't force user to fill out field, do check fo valid entry
    If txtElevation.Text = "User Input" Then
        oProfile.WriteValue SCOPE_ID, "SiteElevation", ""
    Else
        On Error Resume Next
            val = -1000000    ' illigal number to catch failed convertion
            val = CDbl(txtElevation.Text)
        On Error GoTo 0
        
        If val = -1000000 Then
            MsgBox "You must enter a number for Elevation", _
                vbExclamation
            txtElevation.SetFocus                  ' Put cursor in this box
            Exit Sub
        Else
            oProfile.WriteValue SCOPE_ID, "SiteElevation", Str(val)
        End If
    End If

    oProfile.WriteValue SCOPE_ID, "Beep", CStr(chkBeep.Value)
    oProfile.WriteValue SCOPE_ID, "AutoTrack", CStr(chkAutoTrack.Value)
    oProfile.WriteValue SCOPE_ID, "AutoSetTime", CStr(chkAutoSetTime.Value)
    oProfile.WriteValue SCOPE_ID, "AutoReboot", CStr(chkAutoReboot.Value)
    oProfile.WriteValue SCOPE_ID, "SyncDelay", CStr(chkSyncDelay.Value)
    
    txtMaxSlew.Text = UCase$(Trim$(txtMaxSlew.Text))
    If txtMaxSlew.Text = "A" Or txtMaxSlew.Text = "N" Then
        oProfile.WriteValue SCOPE_ID, "MaxSlew", txtMaxSlew.Text
    Else
        buf = "You must enter a number from 1 to 5 or ""A"" (for auto) or ""N"" (for ""use handbox setting"") for Max Slew Speed"
        On Error Resume Next                        ' Catch non-numeric (except "-")
        val = CInt(txtMaxSlew.Text)
        If Err.Number <> 0 Then
            MsgBox buf, vbExclamation
            txtMaxSlew.SetFocus                     ' Put cursor in this box
            Exit Sub
        End If
        On Error GoTo 0
        If val < 1 Or val > 5 Then                  ' Check range of input
            MsgBox buf, vbExclamation
            txtMaxSlew.SetFocus                     ' Put cursor in this box
            Exit Sub
        End If
        oProfile.WriteValue SCOPE_ID, "MaxSlew", Str(val)
    End If
    
    ' collect coordinate behavior when parked
    If optParkedBehavior(0).Value Then
        oProfile.WriteValue SCOPE_ID, "ParkedBehavior", CStr(0)
    ElseIf optParkedBehavior(1).Value Then
        oProfile.WriteValue SCOPE_ID, "ParkedBehavior", CStr(1)
    Else
        oProfile.WriteValue SCOPE_ID, "ParkedBehavior", CStr(2)
    End If
    
    On Error Resume Next
        val = 0
        val = CDbl(txtParkAlt.Text)
    On Error GoTo 0
    If val >= -90 And val <= 90 Then
        oProfile.WriteValue SCOPE_ID, "ParkAlt", Str(val)
    Else
        MsgBox "You must enter a valid number for Park Altitude.", _
            vbExclamation
        Exit Sub
    End If
    
    On Error Resume Next
        val = 0
        val = CDbl(txtParkAz.Text)
    On Error GoTo 0
    If val < 0 Then _
        val = val + 360     ' allows for -90 and such
    If val >= 0 And val < 360 Then
        oProfile.WriteValue SCOPE_ID, "ParkAz", Str(val)
    Else
        MsgBox "You must enter a valid number for Park Azimuth.", _
            vbExclamation
        Exit Sub
    End If
    
    '
    ' Then do the Focuser-related items
    '
    oProfile.DeviceType = "Focuser"
    '
    ' Save the "number" in the max increment check box, blindly.
    ' If this value is invalid then we default to 100 ms.
    '
    On Error Resume Next
    MaxIncrement = (-1)
    MaxIncrement = CLng(txtMaxIncrement)
    On Error GoTo 0
    If MaxIncrement > 0 Then
        oProfile.WriteValue FOCUSER_ID, "Max Increment", CStr(txtMaxIncrement)
    Else
        MsgBox "You must enter a non-negative number for the maximum number of " & _
                "milliseconds to actuate the focuser.", vbExclamation
        txtMaxIncrement.SetFocus                    ' Put cursor in this box
        Exit Sub
    End If
    '
    ' Invalid entries in txtBacklashSteps are eliminated with
    ' Sub txtBacklashSteps_KeyPress
    '
    oProfile.WriteValue FOCUSER_ID, "Backlash Steps", CStr(txtBacklashSteps)
    If optFinalDirectionOut.Value = True Then
        oProfile.WriteValue FOCUSER_ID, "Final Direction", "Out"
    Else
        oProfile.WriteValue FOCUSER_ID, "Final Direction", "In"
    End If
    oProfile.WriteValue FOCUSER_ID, "Flip Dir", CStr(chkFlip.Value)
    oProfile.WriteValue FOCUSER_ID, "Dynamic Braking", CStr(chkDynamicBraking.Value)
    
    Me.Hide
    
End Sub

Private Sub chkAutoReboot_Click()

    If chkAutoReboot.Value = 1 Then                         ' Turning on AutoReboot
    
        chkAutoSetTime.Value = 1                            ' Force AutoSetTime
        
        If Not m_bInLoad Then MsgBox "If no response from the scope is detected, " & _
               "rebooting will be attempted, and may take as long as 4 minutes.", _
               (vbOKOnly + vbInformation + vbMsgBoxSetForeground), _
               "ASCOM Meade LX200GPS/R Driver"
    End If
    
End Sub

Private Sub chkAutoSetTime_Click()

    If chkAutoSetTime.Value = 0 And chkAutoReboot.Value = 1 Then
    
        chkAutoSetTime.Value = 1                            ' Force AutoSetTime
        
        If Not m_bInLoad Then MsgBox "Since Auto Reboot is checked, " & _
               "Auto Set Time must happen.", _
               (vbOKOnly + vbInformation + vbMsgBoxSetForeground), _
               "ASCOM Meade LX200GPS/R Driver"
    End If
    
End Sub

Private Sub cmdCancel_Click()

    Me.Hide
    
End Sub

Private Sub optParkedBehavior_Click(Index As Integer)

    If Index = 1 Then
        txtParkAlt.Enabled = True
        txtParkAz.Enabled = True
    Else
        txtParkAlt.Enabled = False
        txtParkAz.Enabled = False
    End If
    
End Sub

Private Sub chkDynamicBraking_Click()

    If Not m_bInLoad And (chkDynamicBraking.Value = 1) Then _
        MsgBox "Dynamic braking works only on the LX200 Classic.", _
            (vbOKOnly + vbInformation + vbMsgBoxSetForeground), _
            App.Title

End Sub

Private Sub txtBacklashSteps_KeyPress(KeyAscii As Integer)

    Select Case KeyAscii
        Case vbKeyBack              ' Allow these control chars
        Case vbKeyReturn
        Case vbKeyEnd
        Case 48 To 57               ' Allow 0 - 9
        Case Else                   ' Cancel anything else
            KeyAscii = 0
    End Select
    
End Sub

Private Sub picASCOM_Click()
    Dim z As Long

    z = ShellExecute(0, "Open", "http://ASCOM-Standards.org/", 0, 0, SW_SHOWNORMAL)
    If (z > 0) And (z <= 32) Then
        MsgBox _
            "It doesn't appear that you have a web browser installed " & _
            "on your system.", (vbOKOnly + vbExclamation + vbMsgBoxSetForeground), ERR_SOURCE
        Exit Sub
    End If
    
End Sub

Private Sub cmdHelp_Click()

    ShowHelp
    
End Sub

