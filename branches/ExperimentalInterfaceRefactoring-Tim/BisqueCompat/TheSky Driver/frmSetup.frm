VERSION 5.00
Begin VB.Form frmSetup 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "ASCOM Driver for TheSky(tm)"
   ClientHeight    =   5595
   ClientLeft      =   90
   ClientTop       =   330
   ClientWidth     =   4335
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5595
   ScaleWidth      =   4335
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.CheckBox chkTrackOffs 
      Caption         =   "Enable Tracking Offsets"
      Height          =   300
      Left            =   345
      TabIndex        =   5
      Top             =   3300
      Width           =   3105
   End
   Begin VB.ComboBox cbAlignmentMode 
      Height          =   315
      ItemData        =   "frmSetup.frx":0000
      Left            =   360
      List            =   "frmSetup.frx":0010
      TabIndex        =   7
      Text            =   "cbAlignmentMode"
      Top             =   4200
      Width           =   2895
   End
   Begin VB.CheckBox chkPulseGuide 
      Caption         =   "Enable PulseGuide (experimental)"
      Height          =   300
      Left            =   345
      TabIndex        =   6
      Top             =   3660
      Width           =   3105
   End
   Begin VB.CheckBox chkInitHome 
      Caption         =   "Find Home on initial connect"
      Height          =   435
      Left            =   345
      TabIndex        =   3
      Top             =   2535
      Width           =   3930
   End
   Begin VB.CheckBox chkSlewStartDelay 
      Caption         =   "Slew-start delay for CPU-hogging cameras"
      Height          =   435
      Left            =   345
      TabIndex        =   4
      Top             =   2880
      Width           =   3930
   End
   Begin VB.CommandButton CmdCancel 
      Caption         =   "&Cancel"
      Height          =   360
      Left            =   2100
      TabIndex        =   8
      Top             =   5025
      Width           =   975
   End
   Begin VB.Frame Frame1 
      Caption         =   "TheSky Version Selector"
      Height          =   990
      Left            =   120
      TabIndex        =   12
      Top             =   1125
      Width           =   4095
      Begin VB.OptionButton rbSky5 
         Caption         =   "TheSky Version 5.0.110 or later"
         Height          =   195
         Left            =   210
         TabIndex        =   1
         Top             =   615
         Width           =   2865
      End
      Begin VB.OptionButton rbSky6 
         Caption         =   "TheSky Version 6.0.0.40 or later"
         Height          =   195
         Left            =   210
         TabIndex        =   0
         Top             =   315
         Width           =   2895
      End
   End
   Begin VB.CheckBox chkTPOINT 
      Caption         =   "Inhibit SYNC to protect TPOINT model"
      Height          =   435
      Left            =   345
      TabIndex        =   2
      Top             =   2205
      Width           =   3930
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   360
      Left            =   3180
      TabIndex        =   9
      Top             =   5025
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
      TabIndex        =   11
      ToolTipText     =   "Click to go to the ASCOM web site"
      Top             =   150
      Width           =   720
   End
   Begin VB.Label Label2 
      Caption         =   "Mount type:"
      Height          =   255
      Left            =   345
      TabIndex        =   14
      Top             =   3990
      Width           =   1575
   End
   Begin VB.Label Label1 
      Caption         =   "This driver uses the Active Scripting features of TheSky, converting the interface into the ASCOM Standard for telescope control."
      Height          =   930
      Left            =   1020
      TabIndex        =   13
      Top             =   135
      Width           =   3225
   End
   Begin VB.Label lblVersion 
      Caption         =   "<runtime version>"
      Height          =   240
      Left            =   360
      TabIndex        =   10
      Top             =   4680
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

Public m_Profile As DriverHelper.Profile
Public m_DriverID As String
Public m_bSlewDelay As Boolean

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
    
    '
    ' Load checkbox states from config data
    '
    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Telescope"
    buf = m_Profile.GetValue(ID, "InhibitSync")
    If buf = "" Then buf = "False"                  ' Default to allowing Sync
    If CBool(buf) Then
        Me.chkTPOINT.Value = 1
    Else
        Me.chkTPOINT.Value = 0
    End If
    buf = m_Profile.GetValue(ID, "TheSky6")
    If buf = "" Then buf = "False"                  ' Default to TheSky V5
    If CBool(buf) Then
        Me.rbSky6.Value = True
    Else
        Me.rbSky5.Value = True
    End If
    buf = m_Profile.GetValue(ID, "SlewDelay")
    If buf = "" Then buf = "True"                   ' Default to slew-start delay
    If CBool(buf) Then
        Me.chkSlewStartDelay.Value = 1
    Else
        Me.chkSlewStartDelay.Value = 0
    End If
    buf = m_Profile.GetValue(ID, "FindHome")
    If buf = "" Then buf = "False"                  ' Default to not doing Find Home
    If CBool(buf) Then
        Me.chkInitHome.Value = 1
    Else
        Me.chkInitHome.Value = 0
    End If
    buf = m_Profile.GetValue(ID, "TrackOffsets")
    If buf = "" Then buf = "False"                   ' Default to no tracking offsets
    If CBool(buf) Then
        Me.chkTrackOffs.Value = 1
    Else
        Me.chkTrackOffs.Value = 0
    End If
    buf = m_Profile.GetValue(ID, "PulseGuide")
    If buf = "" Then buf = "False"                   ' Default to no Pulse Guide
    If CBool(buf) Then
        Me.chkPulseGuide.Value = 1
    Else
        Me.chkPulseGuide.Value = 0
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
    
    '
    ' Store config from checkboxes
    '
    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Telescope"
    If Me.chkTPOINT.Value = 1 Then
        m_Profile.WriteValue ID, "InhibitSync", "True"
    Else
        m_Profile.WriteValue ID, "InhibitSync", "False"
    End If
    If Me.rbSky6.Value Then
        m_Profile.WriteValue ID, "TheSky6", "True"
    Else
        m_Profile.WriteValue ID, "TheSky6", "False"
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


