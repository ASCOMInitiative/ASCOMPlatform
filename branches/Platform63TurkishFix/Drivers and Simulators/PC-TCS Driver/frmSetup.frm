VERSION 5.00
Begin VB.Form frmSetup 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "<runtime caption>"
   ClientHeight    =   2190
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   3780
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2190
   ScaleWidth      =   3780
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   2700
      MouseIcon       =   "frmSetup.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":0152
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   6
      Top             =   135
      Width           =   720
   End
   Begin VB.ComboBox lbPort 
      Height          =   315
      ItemData        =   "frmSetup.frx":1016
      Left            =   1065
      List            =   "frmSetup.frx":1051
      Style           =   2  'Dropdown List
      TabIndex        =   0
      Top             =   240
      Width           =   945
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   345
      Left            =   2565
      TabIndex        =   3
      Top             =   1650
      Width           =   990
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   345
      Left            =   2565
      TabIndex        =   1
      Top             =   1170
      Width           =   990
   End
   Begin VB.Label lblDriverInfo 
      Caption         =   "<run time version>"
      Height          =   270
      Left            =   225
      TabIndex        =   5
      Top             =   1710
      Width           =   1335
   End
   Begin VB.Label Label2 
      Caption         =   "Set your ACL controller for 9600 baud."
      Height          =   390
      Left            =   225
      TabIndex        =   4
      Top             =   735
      Width           =   1905
   End
   Begin VB.Label Label1 
      Caption         =   "Serial Port:"
      Height          =   225
      Left            =   210
      TabIndex        =   2
      Top             =   285
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
' Implements the setup form. Hardwired for PC-TCS, saves COM
' port selecteion under its Telescope section in the registry.
'
' Written:  29-Mar-06   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 29-Mar-06 rbd     4.0.3 - Initial edit for PC-TCS driver
' 30-Aug-06 rbd     4.0.6 - Remove broken dynamic port code. Revert to
'                   static list of COM1-COM16 (maybe later).
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

Private m_Profile As DriverHelper.Profile

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
    
    Set m_Profile = New DriverHelper.Profile
    m_Profile.DeviceType = "Telescope"
    buf = m_Profile.GetValue(SCOPE_ID, "COM Port")
    If buf = "" Then buf = "1"              ' Default COM1
    port = CInt(buf)
    lbPort.ListIndex = port - 1             ' Select current port
    
    Me.Caption = App.Title & " Setup"
    lblDriverInfo = "Version " & App.Major & "." & _
                App.Minor & "." & App.Revision

    '
    ' Assure window pops up on top of others.
    '
    SetWindowPos Me.hWnd, HWND_TOPMOST, 0, 0, 0, 0, (SWP_NOMOVE + SWP_NOSIZE)

End Sub

Private Sub cmdOK_Click()
    Dim buf As String
    
    m_Profile.WriteValue SCOPE_ID, "COM Port", CStr(lbPort.ListIndex + 1)
    
    Me.Hide
    
End Sub

Private Sub cmdCancel_Click()

    Me.Hide
    
End Sub

'Private Sub lbPort_DropDown()
'
'     Dim i As Long
'     lbPort.Clear
'     For i = 1 To 16
'         If PortExists("COM" & i) Then
'            lbPort.AddItem "COM" & i
'         End If
'     Next i
'
'End Sub

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

'Public Function PortExists(ByVal PortName As String) As Boolean
'     Dim cc As COMMCONFIG, ccsize As Long
'     ccsize = LenB(cc)     'gets the size of COMMCONFIG structure
'     If GetDefaultCommConfig(PortName & Chr(0), cc, ccsize) = 0 Then
'        PortExists = False
'     Else
'        PortExists = True
'     End If
'End Function
'




