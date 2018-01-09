VERSION 5.00
Begin VB.Form frmSetup 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Camera Simulator Setup"
   ClientHeight    =   5340
   ClientLeft      =   2760
   ClientTop       =   3690
   ClientWidth     =   3495
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5340
   ScaleWidth      =   3495
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.TextBox txtSensorName 
      Height          =   315
      Left            =   2000
      TabIndex        =   21
      Text            =   "xgY"
      Top             =   3800
      Width           =   1200
   End
   Begin VB.CheckBox chkCanFastReadout 
      BackColor       =   &H80000008&
      Caption         =   "Can Fast Readout"
      ForeColor       =   &H8000000E&
      Height          =   300
      Left            =   1700
      MaskColor       =   &H00000000&
      TabIndex        =   20
      Top             =   2540
      UseMaskColor    =   -1  'True
      Width           =   1800
   End
   Begin VB.TextBox txtBayerOffsetY 
      Height          =   315
      Left            =   2950
      TabIndex        =   18
      Text            =   "xgY"
      Top             =   3350
      Width           =   250
   End
   Begin VB.TextBox txtBayerOffsetX 
      Height          =   315
      Left            =   2200
      TabIndex        =   16
      Text            =   "xgY"
      Top             =   3350
      Width           =   250
   End
   Begin VB.ComboBox ComboSensorType 
      Height          =   315
      ItemData        =   "frmSetup.frx":0000
      Left            =   1920
      List            =   "frmSetup.frx":0002
      Style           =   2  'Dropdown List
      TabIndex        =   14
      Top             =   2920
      Width           =   1300
   End
   Begin VB.CheckBox chkShowStars 
      BackColor       =   &H80000008&
      Caption         =   "Show Stars"
      ForeColor       =   &H8000000E&
      Height          =   300
      Left            =   180
      MaskColor       =   &H00000000&
      TabIndex        =   13
      Top             =   2540
      UseMaskColor    =   -1  'True
      Width           =   1200
   End
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
      Top             =   4630
      Width           =   990
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   345
      Left            =   1020
      TabIndex        =   0
      Top             =   4630
      Width           =   990
   End
   Begin VB.Label lblSensorName 
      BackColor       =   &H00000000&
      Caption         =   "Sensor Name"
      ForeColor       =   &H00FFFFFF&
      Height          =   220
      Left            =   900
      TabIndex        =   22
      Top             =   3850
      Width           =   1000
   End
   Begin VB.Label lblBayerOffsetY 
      BackColor       =   &H00000000&
      Caption         =   "Y:"
      ForeColor       =   &H00FFFFFF&
      Height          =   220
      Left            =   2750
      TabIndex        =   19
      Top             =   3400
      Width           =   250
   End
   Begin VB.Label lblBayerOffsetX 
      BackColor       =   &H00000000&
      Caption         =   "Bayer Offset   X:"
      ForeColor       =   &H00FFFFFF&
      Height          =   220
      Left            =   1000
      TabIndex        =   17
      Top             =   3400
      Width           =   1200
   End
   Begin VB.Label LabelSensorType 
      BackColor       =   &H00000000&
      Caption         =   "Sensor Type"
      ForeColor       =   &H00FFFFFF&
      Height          =   250
      Left            =   300
      TabIndex        =   15
      Top             =   2980
      Width           =   1500
   End
   Begin VB.Label lblDriverInfo 
      BackColor       =   &H00000000&
      Caption         =   "<version, etc.>"
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   165
      TabIndex        =   12
      Top             =   5010
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
' 20-July-08 lfw    Added user option to show stars in image
' 31-Dec-09 cdr     5.5.0 Upgrade to the V2 Camera standard
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

Private Sub ComboSensorType_Click()
    If ComboSensorType.ListIndex > 1 Then
        lblBayerOffsetX.Visible = True
        lblBayerOffsetY.Visible = True
        txtBayerOffsetX.Visible = True
        txtBayerOffsetY.Visible = True
    Else
        lblBayerOffsetX.Visible = False
        lblBayerOffsetY.Visible = False
        txtBayerOffsetX.Visible = False
        txtBayerOffsetY.Visible = False
    End If
End Sub

Private Sub Form_Load()
    Dim fs, F
    Dim DLM As String
    
    m_Cancel = True
    
    Me.txtDetWidth.Text = CStr(ReadProfileInt("DetectorWidth", 512))
    Me.txtDetHeight.Text = CStr(ReadProfileInt("DetectorHeight", 512))
    Me.txtPixWidth.Text = CStr(ReadProfileDbl("PixelWidth", 24#))
    Me.txtPixHeight.Text = CStr(ReadProfileDbl("PixelHeight", 24#))
    
    Me.chkShowStars.value = ReadProfileInt("ShowStars", 0)             'lfw1
    
    ' load the Sensor Type combo
    With ComboSensorType
        .Clear
        .AddItem "Monochrome"
        .AddItem "Color"
        .AddItem "RGGB"
        .AddItem "CMYG"
        .AddItem "CMYG2"
        .AddItem "LRGB"
        .ListIndex = ReadProfileInt("SensorType", 0)
    End With
    Me.txtBayerOffsetX.Text = CStr(ReadProfileInt("BayerOffsetX", 0))
    Me.txtBayerOffsetY.Text = CStr(ReadProfileInt("BayerOffsetY", 0))
    Me.chkCanFastReadout.value = ReadProfileInt("CanFastReadout", 0)
    Me.txtSensorName.Text = ReadProfileStr("SensorName", "")
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
    m_Profile.WriteValue m_DriverID, "PixelWidth", Str(Me.txtPixWidth.Text)
    m_Profile.WriteValue m_DriverID, "PixelHeight", Str(Me.txtPixHeight.Text)
    m_Profile.WriteValue m_DriverID, "ShowStars", Me.chkShowStars.value             'lfw1
    m_Profile.WriteValue m_DriverID, "SensorType", Me.ComboSensorType.ListIndex
    If Me.ComboSensorType.ListIndex > 0 Then
        m_Profile.WriteValue m_DriverID, "BayerOffsetX", Me.txtBayerOffsetX.Text
        m_Profile.WriteValue m_DriverID, "BayerOffsetY", Me.txtBayerOffsetY.Text
    End If
    m_Profile.WriteValue m_DriverID, "CanFastReadout", Me.chkCanFastReadout.value             'lfw1
    m_Profile.WriteValue m_DriverID, "SensorName", Me.txtSensorName.Text
    
    m_Cancel = False
    Me.Hide
End Sub

Private Sub cmdCancel_Click()
    Me.Hide
End Sub

Private Function ReadProfileInt(name As String, default As Integer) As Integer
    Dim buf As String
    buf = m_Profile.GetValue(m_DriverID, name)
    If buf = "" Then
        ReadProfileInt = default
    Else
        ReadProfileInt = CInt(buf)
    End If
End Function

Private Function ReadProfileDbl(name As String, default As Double) As Double
    Dim buf As String
    buf = m_Profile.GetValue(m_DriverID, name)
    If buf = "" Then
        ReadProfileDbl = default
    Else
        ReadProfileDbl = Val(buf)
    End If
End Function

Private Function ReadProfileStr(name As String, default As String) As String
    Dim buf As String
    buf = m_Profile.GetValue(m_DriverID, name)
    If buf = "" Then
        ReadProfileStr = default
    Else
        ReadProfileStr = buf
    End If
End Function



