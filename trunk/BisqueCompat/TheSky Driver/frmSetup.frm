VERSION 5.00
Begin VB.Form frmSetup 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "ASCOM Driver for TheSky(tm)"
   ClientHeight    =   5805
   ClientLeft      =   90
   ClientTop       =   330
   ClientWidth     =   4335
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5805
   ScaleWidth      =   4335
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.CheckBox chkTrackOffs 
      Caption         =   "Enable Tracking Offsets"
      Height          =   300
      Left            =   345
      TabIndex        =   6
      ToolTipText     =   "Enables tracking of solar system objects of mount supports it"
      Top             =   3540
      Width           =   3105
   End
   Begin VB.ComboBox cbAlignmentMode 
      Height          =   315
      ItemData        =   "frmSetup.frx":0000
      Left            =   360
      List            =   "frmSetup.frx":0010
      TabIndex        =   8
      Text            =   "cbAlignmentMode"
      Top             =   4440
      Width           =   2895
   End
   Begin VB.CheckBox chkPulseGuide 
      Caption         =   "Enable PulseGuide"
      Height          =   300
      Left            =   345
      TabIndex        =   7
      ToolTipText     =   "Uses tracking offsets for guide motions"
      Top             =   3900
      Width           =   3105
   End
   Begin VB.CheckBox chkInitHome 
      Caption         =   "Find Home on initial connect"
      Height          =   435
      Left            =   345
      TabIndex        =   4
      ToolTipText     =   "Useful for Paramount to start in known state"
      Top             =   2775
      Width           =   3930
   End
   Begin VB.CheckBox chkSlewStartDelay 
      Caption         =   "Slew-start delay for CPU-hogging cameras"
      Height          =   435
      Left            =   345
      TabIndex        =   5
      ToolTipText     =   "Enable this is slews fail after downlooading an image"
      Top             =   3120
      Width           =   3930
   End
   Begin VB.CommandButton CmdCancel 
      Caption         =   "&Cancel"
      Height          =   360
      Left            =   2100
      TabIndex        =   9
      Top             =   5265
      Width           =   975
   End
   Begin VB.Frame Frame1 
      Caption         =   "TheSky Version Selector"
      Height          =   1275
      Left            =   120
      TabIndex        =   13
      Top             =   1125
      Width           =   4095
      Begin VB.OptionButton rbSkyX 
         Caption         =   "TheSky X Pro 10.1.6 or later"
         Height          =   195
         Left            =   225
         TabIndex        =   0
         Top             =   300
         Width           =   2895
      End
      Begin VB.OptionButton rbSky5 
         Caption         =   "TheSky Version 5.0.110 or later"
         Height          =   195
         Left            =   225
         TabIndex        =   2
         Top             =   900
         Width           =   2865
      End
      Begin VB.OptionButton rbSky6 
         Caption         =   "TheSky Version 6.0.0.40 or later"
         Height          =   195
         Left            =   225
         TabIndex        =   1
         Top             =   600
         Width           =   2895
      End
   End
   Begin VB.CheckBox chkTPOINT 
      Caption         =   "Inhibit SYNC to protect TPOINT model"
      Height          =   435
      Left            =   345
      TabIndex        =   3
      ToolTipText     =   "Avoids ""sync into model"" which can degrade TPOINT model"
      Top             =   2445
      Width           =   3930
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   360
      Left            =   3180
      TabIndex        =   10
      Top             =   5265
      Width           =   975
   End
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   120
      MouseIcon       =   "frmSetup.frx":0044
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":0196
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   12
      ToolTipText     =   "Click to go to the ASCOM web site"
      Top             =   150
      Width           =   720
   End
   Begin VB.Label Label2 
      Caption         =   "Mount type:"
      Height          =   255
      Left            =   345
      TabIndex        =   15
      Top             =   4230
      Width           =   1575
   End
   Begin VB.Label Label1 
      Caption         =   "This driver uses the Active Scripting features of TheSky, converting the interface into the ASCOM Standard for telescope control."
      Height          =   930
      Left            =   1020
      TabIndex        =   14
      Top             =   135
      Width           =   3225
   End
   Begin VB.Label lblVersion 
      Caption         =   "<runtime version>"
      Height          =   240
      Left            =   360
      TabIndex        =   11
      Top             =   4920
      Width           =   2655
   End
End
Attribute VB_Name = "frmSetup"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' 26-Dec-05 rbd 4.1.2 - Add FindHome checkbox
' 19-Feb-09 bsk 5.1.2 - Add Mount Type combobox
' 25-Mar-09 rbd 5.1.3 - Add Tracking Rate checkbox
' 14-May-10 rbd 5.2.1 - Add support for TheSky X
' 16-Jun-10 rbd 5.2.4 - Disable PulseGuide if Tracking Offsets disabled

Option Explicit

Private Const SWP_NOSIZE            As Long = &H1
Private Const SWP_NOMOVE            As Long = &H2
Private Const SWP_NOZORDER          As Long = &H4
Private Const SWP_NOREDRAW          As Long = &H8
Private Const SWP_NOACTIVATE        As Long = &H10
Private Const SWP_FRAMECHANGED      As Long = &H20
Private Const SWP_SHOWWINDOW        As Long = &H40
Private Const SWP_HIDEWINDOW        As Long = &H80
Private Const SWP_NOCOPYBITS        As Long = &H100
Private Const SWP_NOOWNERZORDER     As Long = &H200
Private Const SWP_NOSENDCHANGING    As Long = &H400

Private Const SWP_DRAWFRAME         As Long = SWP_FRAMECHANGED
Private Const SWP_NOREPOSITION      As Long = SWP_NOOWNERZORDER

Private Const HWND_TOP              As Long = 0
Private Const HWND_BOTTOM           As Long = 1
Private Const HWND_TOPMOST          As Long = -1
Private Const HWND_NOTOPMOST        As Long = -2

Private Const SW_SHOWNORMAL         As Long = 1

'Public Enum TheSkyType
'    TheSky5 = 0
'    TheSky6 = 1
'    TheSkyX = 2
'End Enum
    
Public m_Profile As DriverHelper.Profile
Public m_DriverID As String
Public m_bSlewDelay As Boolean
Public m_eTheSkyType As TheSkyType

Private Declare Function SetWindowPos Lib "user32.dll" ( _
                ByVal hWnd As Long, _
                ByVal hWndInsertAfter As Long, _
                ByVal X As Long, _
                ByVal y As Long, _
                ByVal cx As Long, _
                ByVal cy As Long, _
                ByVal uFLags As Long) As Long

Private Declare Function ShellExecute Lib "shell32" Alias "ShellExecuteA" ( _
                ByVal hWnd As Long, _
                ByVal lpOperation As String, _
                ByVal lpFile As String, _
                ByVal lpParameters As String, _
                ByVal lpDirectory As String, _
                ByVal nShowCmd As Long) As Long


Private Sub Form_Load()
    Dim port As Long
    Dim buf As String
    
    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Telescope"
    
    buf = m_Profile.GetValue(ID, "TheSkyType")
    If buf <> "" Then
        Select Case buf
            Case "TheSky5":
                m_eTheSkyType = TheSky5
                Me.rbSky5.Value = True
            Case "TheSky6":
                m_eTheSkyType = TheSky6
                Me.rbSky6.Value = True
            Case "TheSkyX":
                m_eTheSkyType = TheSkyX
                Me.rbSkyX.Value = True
        End Select
    Else
        '
        ' Read the old registry data (TheSky6 or not)
        '
        buf = m_Profile.GetValue(ID, "TheSky6")
        If buf = "" Then buf = "False"                  ' Default to TheSky V5
        If CBool(buf) Then
            m_eTheSkyType = TheSky6
            Me.rbSky6.Value = True
        Else
            m_eTheSkyType = TheSky5
            Me.rbSky5.Value = True
        End If
    End If
    
    buf = m_Profile.GetValue(ID, "InhibitSync")
    If buf = "" Then buf = "False"                      ' Default to allowing Sync
    If CBool(buf) Then
        Me.chkTPOINT.Value = 1
    Else
        Me.chkTPOINT.Value = 0
    End If
    
    buf = m_Profile.GetValue(ID, "SlewDelay")
    If buf = "" Then buf = "True"                       ' Default to slew-start delay
    If CBool(buf) Then
        Me.chkSlewStartDelay.Value = 1
    Else
        Me.chkSlewStartDelay.Value = 0
    End If
    
    buf = m_Profile.GetValue(ID, "FindHome")
    If buf = "" Then buf = "False"                      ' Default to not doing Find Home
    If CBool(buf) Then
        Me.chkInitHome.Value = 1
    Else
        Me.chkInitHome.Value = 0
    End If
    
    buf = m_Profile.GetValue(ID, "TrackOffsets")
    If buf = "" Then buf = "False"                      ' Default to no tracking offsets
    If CBool(buf) Then
        Me.chkTrackOffs.Value = 1
    Else
        Me.chkTrackOffs.Value = 0
    End If
    
    If Me.chkTrackOffs.Value = 1 Then                   ' No Pulse Guide if no Track Offsets
        buf = m_Profile.GetValue(ID, "PulseGuide")
        If buf = "" Then buf = "False"                  ' Default to no Pulse Guide
        If CBool(buf) Then
            Me.chkPulseGuide.Value = 1
        Else
            Me.chkPulseGuide.Value = 0
        End If
    Else
        Me.chkPulseGuide.Value = 0
        Me.chkPulseGuide.Enabled = False
    End If
    
    buf = m_Profile.GetValue(ID, "AlignmentMode")
    Select Case buf
        Case "algUnknown"
            Me.cbAlignmentMode.ListIndex = 0
        Case "algAltAz"
            Me.cbAlignmentMode.ListIndex = 1
        Case "algPolar"
            Me.cbAlignmentMode.ListIndex = 2
        Case "algGermanPolar"
            Me.cbAlignmentMode.ListIndex = 3
        Case Else
            Me.cbAlignmentMode.ListIndex = 0
    End Select
    
    Set m_Profile = Nothing
    
    Me.lblVersion = "Version " & App.Major & "." & _
                App.Minor & "." & App.Revision


    '
    ' Assure window pops up on top of others.
    '
    SetWindowPos Me.hWnd, HWND_TOPMOST, 0, 0, 0, 0, (SWP_NOMOVE + SWP_NOSIZE)

End Sub

Private Sub ShowWebPage(url As String)
    Dim z As Long

    z = ShellExecute(0, "Open", url, 0, 0, SW_SHOWNORMAL)
    If (z > 0) And (z <= 32) Then
        MsgBox _
            "It doesn't appear that you have a web browser installed " & _
            "on your system.", (vbOKOnly + vbExclamation + vbMsgBoxSetForeground), ERR_SOURCE
        Exit Sub
    End If
    
End Sub

Private Sub cmdOK_Click()
    Dim buf As String
    
    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Telescope"
    
    Select Case m_eTheSkyType
        Case TheSky5: buf = "TheSky5"
        Case TheSky6: buf = "TheSky6"
        Case TheSkyX: buf = "TheSkyX"
    End Select
    m_Profile.WriteValue ID, "TheSkyType", buf
    
    If Me.chkTPOINT.Value = 1 Then
        m_Profile.WriteValue ID, "InhibitSync", "True"
    Else
        m_Profile.WriteValue ID, "InhibitSync", "False"
    End If
    
    If Me.chkSlewStartDelay.Value = 1 Then
        m_Profile.WriteValue ID, "SlewDelay", "True"
    Else
        m_Profile.WriteValue ID, "SlewDelay", "False"
    End If
    
    If Me.chkInitHome.Value = 1 Then
        m_Profile.WriteValue ID, "FindHome", "True"
    Else
        m_Profile.WriteValue ID, "FindHome", "False"
    End If
    
    If Me.chkTrackOffs.Value = 1 Then
        m_Profile.WriteValue ID, "TrackOffsets", "True"
    Else
        m_Profile.WriteValue ID, "TrackOffsets", "False"
    End If
    
    If Me.chkPulseGuide.Value = 1 Then
        m_Profile.WriteValue ID, "PulseGuide", "True"
    Else
        m_Profile.WriteValue ID, "PulseGuide", "False"
    End If
    
    Select Case Me.cbAlignmentMode.ListIndex
        Case 0
            m_Profile.WriteValue ID, "AlignmentMode", "algUnknown"
        Case 1
            m_Profile.WriteValue ID, "AlignmentMode", "algAltAz"
        Case 2
            m_Profile.WriteValue ID, "AlignmentMode", "algPolar"
        Case 3
            m_Profile.WriteValue ID, "AlignmentMode", "algGermanPolar"
        Case Else
            m_Profile.WriteValue ID, "AlignmentMode", "algUnknown"
    End Select
    
    Me.Hide

End Sub

Private Sub cmdCancel_Click()

    Me.Hide
    
End Sub

Private Sub picASCOM_Click()

    Call ShowWebPage("http://ASCOM-Standards.org/")
    
End Sub

Private Sub rbSky5_Click()
    m_eTheSkyType = TheSky5
End Sub

Private Sub rbSky6_Click()
    m_eTheSkyType = TheSky6
End Sub

Private Sub rbSkyX_Click()
    m_eTheSkyType = TheSkyX
End Sub

Private Sub chkTrackOffs_Click()
    If Me.chkTrackOffs.Value = 0 Then
        Me.chkPulseGuide.Value = 0
        Me.chkPulseGuide.Enabled = False
    Else
        Me.chkPulseGuide.Enabled = True
    End If
End Sub


