VERSION 5.00
Begin VB.Form frmSetup 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Camera Simulator Setup"
   ClientHeight    =   3510
   ClientLeft      =   2760
   ClientTop       =   3690
   ClientWidth     =   3495
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3510
   ScaleWidth      =   3495
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame frmShutterControl 
      BackColor       =   &H00000000&
      Caption         =   "CCD Geometry"
      ForeColor       =   &H00FFFFFF&
      Height          =   2310
      Left            =   180
      TabIndex        =   2
      Top             =   180
      Width           =   3075
      Begin VB.TextBox txtPixHeight 
         Height          =   315
         Left            =   2040
         TabIndex        =   6
         Text            =   "xgY"
         Top             =   1755
         Width           =   765
      End
      Begin VB.TextBox txtPixWidth 
         Height          =   315
         Left            =   2040
         TabIndex        =   5
         Text            =   "xgY"
         Top             =   1290
         Width           =   765
      End
      Begin VB.TextBox txtDetHeight 
         Height          =   315
         Left            =   2040
         TabIndex        =   4
         Text            =   "xgY"
         Top             =   810
         Width           =   765
      End
      Begin VB.TextBox txtDetWidth 
         Height          =   315
         Left            =   2040
         TabIndex        =   3
         Text            =   "xgY"
         Top             =   330
         Width           =   765
      End
      Begin VB.Label lblMin 
         BackColor       =   &H00000000&
         Caption         =   "Pixel height (microns):"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Left            =   270
         TabIndex        =   10
         Top             =   1800
         Width           =   1605
      End
      Begin VB.Label lblMax 
         BackColor       =   &H00000000&
         Caption         =   "Pixel width (microns):"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Left            =   255
         TabIndex        =   9
         Top             =   1335
         Width           =   1800
      End
      Begin VB.Label lblOCDelay 
         BackColor       =   &H00000000&
         Caption         =   "Detector height (pixels):"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Left            =   255
         TabIndex        =   8
         Top             =   855
         Width           =   1725
      End
      Begin VB.Label lblAltRate 
         BackColor       =   &H00000000&
         Caption         =   "Detector width (pixels):"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Left            =   255
         TabIndex        =   7
         Top             =   375
         Width           =   1710
      End
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
      Height          =   345
      Left            =   2265
      TabIndex        =   1
      Top             =   2730
      Width           =   990
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   345
      Left            =   1020
      TabIndex        =   0
      Top             =   2730
      Width           =   990
   End
   Begin VB.Label lblDriverInfo 
      BackColor       =   &H00000000&
      Caption         =   "<version, etc.>"
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   165
      TabIndex        =   12
      Top             =   3210
      Width           =   3150
   End
   Begin VB.Label lblLastModified 
      Alignment       =   1  'Right Justify
      BackColor       =   &H00000000&
      Caption         =   "<last modified>"
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   2220
      TabIndex        =   11
      Top             =   0
      Width           =   2715
   End
End
Attribute VB_Name = "frmSetup"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'---------------------------------------------------------------------
' Copyright © 2000-2001 SPACE.com Inc., New York, NY
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
' Setup form for ASCOM CCD camera simulator driver
'
' Written:  22-Aug-00   Robert B. Denny <rdenny@dc3.com>
' Modified by:          Matthias Busch <Matthias.Busch@easysky.de>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 22-Aug-00 rbd     Initial edit
' 21-Jan-01 rbd     New Helper.Profile object for registry stuff
'                   No more General subkey
' 06-Feb-02 mab     Copied from Meade LX200 driver
' 14-Oct-07 rbd     5.0.1 - Remove dead code, add detector config and
'                   driver version/last-mod.
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

Public m_Profile As DriverHelper.Profile
Public m_DriverID As String
Public m_Cancel As Boolean

Private Declare Function SetWindowPos Lib "user32.dll" ( _
                ByVal hWnd As Long, _
                ByVal hWndInsertAfter As Long, _
                ByVal X As Long, _
                ByVal y As Long, _
                ByVal cx As Long, _
                ByVal cy As Long, _
                ByVal uFLags As Long) As Long

Private Sub Form_Load()
    Dim fs, F
    Dim DLM As String
    
    m_Cancel = True
    
    Me.txtDetWidth.Text = m_Profile.GetValue(m_DriverID, "DetectorWidth")
    If Me.txtDetWidth.Text = "" Then Me.txtDetWidth.Text = "512"
    Me.txtDetHeight.Text = m_Profile.GetValue(m_DriverID, "DetectorHeight")
    If Me.txtDetHeight.Text = "" Then Me.txtDetHeight.Text = "512"
    Me.txtPixWidth.Text = m_Profile.GetValue(m_DriverID, "PixelWidth")
    If Me.txtPixWidth.Text = "" Then Me.txtPixWidth.Text = "24"
    Me.txtPixHeight.Text = m_Profile.GetValue(m_DriverID, "PixelHeight")
    If Me.txtPixHeight.Text = "" Then Me.txtPixHeight.Text = "24"
    
    '
    ' Assure window pops up on top of others.
    '
    SetWindowPos Me.hWnd, HWND_TOPMOST, 0, 0, 0, 0, (SWP_NOMOVE + SWP_NOSIZE)

    lblDriverInfo = App.FileDescription & " " & _
        App.Major & "." & App.Minor & "." & App.Revision
        
End Sub

Private Sub cmdOK_Click()

    m_Profile.WriteValue m_DriverID, "DetectorWidth", Me.txtDetWidth.Text
    m_Profile.WriteValue m_DriverID, "DetectorHeight", Me.txtDetHeight.Text
    m_Profile.WriteValue m_DriverID, "PixelWidth", Me.txtPixWidth.Text
    m_Profile.WriteValue m_DriverID, "PixelHeight", Me.txtPixHeight.Text
    
    m_Cancel = False
    
    Me.Hide
    
End Sub

Private Sub cmdCancel_Click()

    Me.Hide
    
End Sub


