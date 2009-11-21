VERSION 5.00
Begin VB.Form frmShow 
   Caption         =   "Show Data Traffic"
   ClientHeight    =   6180
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   5340
   BeginProperty Font 
      Name            =   "Courier"
      Size            =   9.75
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "frmShow.frx":0000
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   6180
   ScaleWidth      =   5340
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdClear 
      Caption         =   "Clear"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   285
      Left            =   4590
      MaskColor       =   &H8000000F&
      TabIndex        =   5
      Top             =   5820
      Width           =   675
   End
   Begin VB.CheckBox Docked 
      Caption         =   "Docked"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   315
      Left            =   4470
      TabIndex        =   4
      Top             =   0
      Width           =   885
   End
   Begin VB.TextBox txtReceived 
      Height          =   5385
      Left            =   2730
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   1
      Top             =   330
      Width           =   2475
   End
   Begin VB.TextBox txtSent 
      Height          =   5385
      Left            =   90
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   0
      Top             =   330
      Width           =   2475
   End
   Begin VB.Label Label2 
      Caption         =   "Received"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Left            =   2730
      TabIndex        =   3
      Top             =   60
      Width           =   795
   End
   Begin VB.Label Label1 
      Caption         =   "Sent"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Left            =   90
      TabIndex        =   2
      Top             =   60
      Width           =   405
   End
End
Attribute VB_Name = "frmShow"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Option Explicit

Private Sub cmdClear_Click()

  txtSent.Text = ""
  txtReceived.Text = ""
  commInputString = ""

End Sub

Private Sub Form_Load()

  On Error GoTo errHandler
  
  Docked.Visible = False
  
  Call setFormPositionIni(Me, "0 2000 5340 3105")
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmShow Form_Load: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub Form_Resize()

  On Error GoTo errHandler
  
  If Me.WindowState = vbMinimized Then
    'MsgBox "minimized"
  Else
    If height < 1500 Then
      height = 1500
      
      Caption = "Ouch! Don't pinch the data!"
      BackColor = &HFF&
      
      Exit Sub
    Else
      
      Caption = "Show Data Traffic"
      BackColor = LightGray
      
      txtSent.height = height - 1200
      txtReceived.height = height - 1200
      cmdClear.top = height - 750
    End If
  End If

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmShow Form_Resize: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub Form_Unload(Cancel As Integer)

  On Error GoTo errHandler
  
  Call saveFormPosition(Me)
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmShow Form_Unload: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

