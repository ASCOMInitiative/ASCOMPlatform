VERSION 5.00
Begin VB.Form frmHelp 
   Appearance      =   0  'Flat
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "About RoboFocus"
   ClientHeight    =   4995
   ClientLeft      =   1680
   ClientTop       =   2055
   ClientWidth     =   4965
   ForeColor       =   &H80000008&
   Icon            =   "frmHelp.frx":0000
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   333
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   331
   ShowInTaskbar   =   0   'False
   Begin VB.CommandButton cmdShowDocs 
      Caption         =   "Read Doc File"
      Height          =   375
      Left            =   2250
      TabIndex        =   6
      Top             =   4530
      Width           =   1485
   End
   Begin VB.TextBox Text1 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H00FFFF00&
      BorderStyle     =   0  'None
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2775
      Left            =   120
      MultiLine       =   -1  'True
      TabIndex        =   3
      Text            =   "frmHelp.frx":0E42
      Top             =   1650
      Width           =   4725
   End
   Begin VB.Timer timStartup 
      Enabled         =   0   'False
      Interval        =   1000
      Left            =   150
      Top             =   4560
   End
   Begin VB.CommandButton cmdClose 
      Appearance      =   0  'Flat
      Cancel          =   -1  'True
      Caption         =   "Close"
      Default         =   -1  'True
      Height          =   375
      Left            =   3990
      MaskColor       =   &H8000000F&
      TabIndex        =   0
      Top             =   4530
      Width           =   885
   End
   Begin VB.Label lblProductRev 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H00FF0000&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000016&
      Height          =   315
      Left            =   120
      TabIndex        =   5
      Top             =   840
      Width           =   4725
   End
   Begin VB.Label lblFirmwareDesc 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H00FF0000&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FFFFFF&
      Height          =   375
      Left            =   120
      TabIndex        =   4
      Top             =   480
      Width           =   4725
   End
   Begin VB.Label lbl 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      Caption         =   "Copyright (C) 2001, Technical Innovations, All Rights Reserved"
      ForeColor       =   &H80000008&
      Height          =   255
      Index           =   0
      Left            =   120
      TabIndex        =   2
      Top             =   1260
      Width           =   4725
   End
   Begin VB.Label lblProductDesc 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H00FF0000&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   13.5
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FFFFFF&
      Height          =   495
      Left            =   120
      TabIndex        =   1
      Top             =   60
      Width           =   4725
   End
End
Attribute VB_Name = "frmHelp"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdClose_Click()
  
  On Error GoTo errHandler
  
  Unload Me

Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmHelp cmdClose_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub cmdShowDocs_Click()
  
  Dim fn$, rv%
   
  On Error GoTo errorHandler
  
  fn = App.Path + "\robofocusins3.doc"
  rv = ShellExecute(0, "open", fn, "", Format$(App.Path), 1)
  
  ' ShellExecute returns greater then 32 if successful (jab)
  If rv < 33 Then
    Select Case rv
      Case 2
        Call showMsg("Unable to open " & vbCrLf & fn & _
                                vbCrLf & vbCrLf & Err.Description, "", 3)
      Case 31
        Call showMsg("To view the online documentation, you need to install the Word Viewer provided on the RoboFocus CD-ROM.", "", 3)
        
      Case Else
        Call showMsg("Error #" & rv & " trying to open " & vbCrLf & fn & _
                                vbCrLf & vbCrLf & Err.Description, "", 3)
    End Select
  End If
  
  On Error GoTo 0
  
  Exit Sub
  
errorHandler:
  Call showMsg("Error in frmHelp cmdShowDocs_Click: " & Err.Description, "", 3)
  Resume Next
  
End Sub

Private Sub Form_Load()
  
  On Error GoTo errHandler
  
  lblProductDesc.Caption = App.FileDescription & " " & _
        App.Major & "." & App.Minor & "." & App.Revision
  
'  lblProductRev.Caption = "Control Program Revision: " & _
'                                          left(gblDateLastModified, 8)
  lblProductRev.Caption = "Control Program Revision: " & _
                                          gblDateLastModified
  Call setFormPositionIni(Me, "0 2000 5055 5370")

Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmHelp Form_Load: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

