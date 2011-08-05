VERSION 5.00
Begin VB.Form frmSetup 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "<runtime caption>"
   ClientHeight    =   3465
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   3750
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3465
   ScaleWidth      =   3750
   Begin VB.ComboBox lbChannel 
      Height          =   315
      ItemData        =   "frmSetup.frx":0000
      Left            =   1380
      List            =   "frmSetup.frx":0010
      Style           =   2  'Dropdown List
      TabIndex        =   1
      ToolTipText     =   "COM port for the Dome"
      Top             =   1170
      Width           =   615
   End
   Begin VB.CheckBox chkCanSetShutter 
      Caption         =   "&Enable Shutter Support"
      Height          =   255
      Left            =   210
      TabIndex        =   3
      ToolTipText     =   "select for shutter control"
      Top             =   1995
      Value           =   1  'Checked
      Width           =   3030
   End
   Begin VB.CheckBox chkAutoUnpark 
      Caption         =   "&Auto Home ( Unpark on Connect )"
      Height          =   255
      Left            =   210
      TabIndex        =   2
      ToolTipText     =   "On connect find home position"
      Top             =   1635
      Value           =   1  'Checked
      Width           =   3030
   End
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   2700
      MouseIcon       =   "frmSetup.frx":0020
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":0172
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   9
      Top             =   135
      Width           =   720
   End
   Begin VB.ComboBox lbPort 
      Height          =   315
      ItemData        =   "frmSetup.frx":1036
      Left            =   1065
      List            =   "frmSetup.frx":1071
      Style           =   2  'Dropdown List
      TabIndex        =   0
      ToolTipText     =   "COM port for the Dome"
      Top             =   240
      Width           =   945
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   345
      Left            =   2565
      TabIndex        =   5
      ToolTipText     =   "Reject changes"
      Top             =   2925
      Width           =   990
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   345
      Left            =   2565
      TabIndex        =   4
      ToolTipText     =   "Accept changes"
      Top             =   2445
      Width           =   990
   End
   Begin VB.Label Label4 
      Caption         =   "(default channel 1)"
      Height          =   285
      Left            =   2025
      TabIndex        =   12
      Top             =   1215
      Width           =   1545
   End
   Begin VB.Label Label3 
      Caption         =   "MNCP Channel:"
      Height          =   225
      Left            =   210
      TabIndex        =   11
      Top             =   1215
      Width           =   1215
   End
   Begin VB.Label lblLastModified 
      Caption         =   "<last modified>"
      Height          =   420
      Left            =   450
      TabIndex        =   10
      Top             =   2835
      Width           =   1935
   End
   Begin VB.Label lblDriverInfo 
      Caption         =   "<run time version>"
      Height          =   270
      Left            =   450
      TabIndex        =   8
      Top             =   2475
      Width           =   1995
   End
   Begin VB.Label Label2 
      Caption         =   "( Set your ACL controller for 9600 baud )"
      Height          =   390
      Left            =   450
      TabIndex        =   7
      Top             =   660
      Width           =   1845
   End
   Begin VB.Label Label1 
      Caption         =   "Serial Port:"
      Height          =   225
      Left            =   210
      TabIndex        =   6
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
' 13-Jan-04 jab     ACL Dome 1.0.0 - initial version (copied from
'                   ACL telescope driver and modified for dome)
' 24-Nov-04 rbd     4.0.2 - COM ports to 16
' 08-Oct-08 rbd     5.0.2 - Add MNCP channel control, implement ASCOM 5
'                   early binding interface.
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

Private Sub Form_Load()
    Dim port As Long
    Dim chan As Long
    Dim buf As String
    Dim fs, F
    Dim DLM As String
    
    ' get initial COM port
    buf = m_Profile.GetValue(m_DriverID, "COM Port")
    If buf = "" Then _
        buf = "1"              ' Default COM1
    port = CInt(buf)
    lbPort.ListIndex = port - 1             ' Select current port
    
    ' get initial channel
    buf = m_Profile.GetValue(m_DriverID, "MNCP Channel")
    If buf = "" Then _
        buf = "1"              ' Default channel 1
    chan = CInt(buf)
    lbChannel.ListIndex = chan - 1          ' Select current channel
    
    ' get AutoUnpark state
    buf = m_Profile.GetValue(m_DriverID, "AutoUnpark")
    chkAutoUnpark.Value = IIf(buf = "0", 0, 1)
    
    ' get shutter support state
    buf = m_Profile.GetValue(m_DriverID, "CanSetShutter")
    chkCanSetShutter.Value = IIf(buf = "0", 0, 1)
    
    ' version
    Me.Caption = App.Title & " Setup"
    lblDriverInfo = "Version: " & _
        App.Major & "." & App.Minor & "." & App.Revision
    
    ' date stamp
    On Error Resume Next
    Set fs = CreateObject("Scripting.FileSystemObject")
    Set F = fs.GetFile(App.Path & "\" & "ACL Dome Driver" & ".dll")
    DLM = F.DateLastModified
    lblLastModified = "Modified:" & vbCrLf & DLM
    On Error GoTo 0

    '
    ' Assure window pops up on top of others.
    '
    SetWindowPos Me.hWnd, HWND_TOPMOST, 0, 0, 0, 0, (SWP_NOMOVE + SWP_NOSIZE)

End Sub

Private Sub cmdOK_Click()
    Dim buf As String
    
    m_Profile.WriteValue m_DriverID, "COM Port", CStr(lbPort.ListIndex + 1)
    m_Profile.WriteValue m_DriverID, "MNCP Channel", CStr(lbChannel.ListIndex + 1)
    m_Profile.WriteValue m_DriverID, "AutoUnpark", CStr(chkAutoUnpark.Value)
    m_Profile.WriteValue m_DriverID, "CanSetShutter", CStr(chkCanSetShutter.Value)
    
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
