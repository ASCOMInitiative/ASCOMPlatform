VERSION 5.00
Begin VB.Form frmSetup 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "<runtime caption>"
   ClientHeight    =   2925
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   3540
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2925
   ScaleWidth      =   3540
   Begin VB.Frame Frame1 
      Caption         =   "Encoder Resolution"
      Height          =   990
      Left            =   165
      TabIndex        =   6
      Top             =   1350
      Width           =   1785
      Begin VB.TextBox txtDecRes 
         Height          =   285
         Left            =   645
         TabIndex        =   10
         Text            =   "60"
         Top             =   585
         Width           =   390
      End
      Begin VB.TextBox txtRARes 
         Height          =   285
         Left            =   645
         TabIndex        =   9
         Text            =   "60"
         Top             =   270
         Width           =   390
      End
      Begin VB.Label Label4 
         Caption         =   "integer arcsec"
         BeginProperty Font 
            Name            =   "Small Fonts"
            Size            =   6.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   360
         Left            =   1155
         TabIndex        =   11
         Top             =   375
         Width           =   495
         WordWrap        =   -1  'True
      End
      Begin VB.Label Label3 
         Caption         =   "Dec:"
         Height          =   210
         Left            =   195
         TabIndex        =   8
         Top             =   630
         Width           =   360
      End
      Begin VB.Label Label2 
         Caption         =   "RA:"
         Height          =   210
         Left            =   210
         TabIndex        =   7
         Top             =   315
         Width           =   300
      End
   End
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   2505
      MouseIcon       =   "frmSetup.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":0152
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   5
      Top             =   900
      Width           =   720
   End
   Begin VB.ComboBox lbPort 
      Height          =   315
      ItemData        =   "frmSetup.frx":1016
      Left            =   1020
      List            =   "frmSetup.frx":1051
      Style           =   2  'Dropdown List
      TabIndex        =   0
      Top             =   915
      Width           =   945
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   345
      Left            =   2370
      TabIndex        =   3
      Top             =   2415
      Width           =   990
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   345
      Left            =   2370
      TabIndex        =   1
      Top             =   1935
      Width           =   990
   End
   Begin VB.Label Label5 
      Caption         =   "Do not use this for Meade LX200 Classic or GPS. Use the Meade LX200 and Autostar selection."
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   720
      Left            =   165
      TabIndex        =   12
      Top             =   75
      Width           =   3420
   End
   Begin VB.Label lblDriverInfo 
      Caption         =   "<run time version>"
      Height          =   270
      Left            =   180
      TabIndex        =   4
      Top             =   2475
      Width           =   1335
   End
   Begin VB.Label Label1 
      Caption         =   "Serial Port:"
      Height          =   225
      Left            =   165
      TabIndex        =   2
      Top             =   960
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
'   ============
'   FRMSETUP.FRM
'   ============
'
' Implements the setup form
'
' Written:  12-Oct-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 15-Oct-00 rbd     Initial edit.
' 21-Jan-01 rbd     New Helper.Profile object, no more General subkey.
' 28-Jan-00 rbd     Add description and version info display to
'                   SetupDialog box.
' 25-Jul-02 rbd     2.0.1 - Add new ASCOM icon and hotlink
' 07-Mar-03 rbd     2.2.0 - Add encoder resolution items
' 05-Jan-04 rbd     3.0.2 - Add dynamic COM port discovery to selector
' 24-Nov-04 rbd     4.0.2 - COM Ports to 16
' 24-Jun-10 fq      5.5.1 - Fix COM port handling
'---------------------------------------------------------------------


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

Private Const COM_MAXPORTS          As Long = 16

Public m_Profile As DriverHelper.Profile
Public m_DriverID As String

Private Declare Function SetWindowPos Lib "user32.dll" ( _
                ByVal hWnd As Long, _
                ByVal hWndInsertAfter As Long, _
                ByVal X As Long, _
                ByVal Y As Long, _
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

Private Type DCB
     DCBlength As Long
     BaudRate As Long
     fBitFields As Long
     wReserved As Integer
     XonLim As Integer
     XoffLim As Integer
     ByteSize As Byte
     Parity As Byte
     StopBits As Byte
     XonChar As Byte
     XoffChar As Byte
     ErrorChar As Byte
     EofChar As Byte
     EvtChar As Byte
     wReserved1 As Integer
End Type

Private Type COMMCONFIG
     dwSize As Long
     wVersion As Integer
     wReserved As Integer
     dcbx As DCB
     dwProviderSubType As Long
     dwProviderOffset As Long
     dwProviderSize As Long
     wcProviderData As Byte
End Type

Private Declare Function GetDefaultCommConfig Lib "kernel32" Alias "GetDefaultCommConfigA" ( _
                ByVal lpszName As String, _
                lpCC As COMMCONFIG, _
                lpdwSize As Long) As Long

Private Sub Form_Load()
    Dim port As Long
    Dim buf As String
    Dim I As Long
    
    ' Get current port
    buf = m_Profile.GetValue(m_DriverID, "COM Port")
    If buf = "" Then buf = "1"                         ' Default COM1
    port = CInt(buf)
    
    ' Populate listbox
    lbPort.Clear
    For I = 1 To COM_MAXPORTS
        If PortExists("COM" & I) Then
            lbPort.AddItem "COM" & I
            If I = port Then
                lbPort.ListIndex = lbPort.NewIndex     ' Select current port
            End If
        End If
    Next I

    Me.Caption = App.Title & " Setup"
    lblDriverInfo = "Version " & App.Major & "." & _
                App.Minor & "." & App.Revision

    txtRARes.Text = m_Profile.GetValue(m_DriverID, "RA Resolution")
    If txtRARes.Text = "" Then txtRARes.Text = "60"
    txtDecRes.Text = m_Profile.GetValue(m_DriverID, "Dec Resolution")
    If txtDecRes.Text = "" Then txtDecRes.Text = "60"
    
    '
    ' Assure window pops up on top of others.
    '
    SetWindowPos Me.hWnd, HWND_TOPMOST, 0, 0, 0, 0, (SWP_NOMOVE + SWP_NOSIZE)

End Sub

Private Sub cmdOK_Click()
    Dim buf As String
    
    m_Profile.WriteValue m_DriverID, "COM Port", Mid(lbPort.Text, 4)
    m_Profile.WriteValue m_DriverID, "RA Resolution", txtRARes.Text
    m_Profile.WriteValue m_DriverID, "Dec Resolution", txtDecRes.Text
    
    Me.Hide
    
End Sub

Private Sub cmdCancel_Click()

    Me.Hide
    
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

Public Function PortExists(ByVal PortName As String) As Boolean
     Dim cc As COMMCONFIG, ccsize As Long
     ccsize = LenB(cc)     'gets the size of COMMCONFIG structure
     If GetDefaultCommConfig(PortName & Chr(0), cc, ccsize) = 0 Then
        PortExists = False
     Else
        PortExists = True
     End If
End Function

