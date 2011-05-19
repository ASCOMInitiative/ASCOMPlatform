VERSION 5.00
Begin VB.Form frmWarnBeforeLoad 
   Caption         =   "Warning!"
   ClientHeight    =   3375
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   6900
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   ScaleHeight     =   3375
   ScaleWidth      =   6900
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdNoReset 
      Caption         =   "Load the file, but leave step size unchanged."
      Height          =   435
      Left            =   1673
      TabIndex        =   5
      Top             =   1620
      Width           =   3555
   End
   Begin VB.CheckBox chkWarnBeforeLoad 
      Caption         =   "Show this warning every time."
      Height          =   435
      Left            =   90
      TabIndex        =   3
      Top             =   2820
      Width           =   1635
   End
   Begin VB.CommandButton cmdSkip 
      Caption         =   "Cancel - Do not load the file."
      Height          =   435
      Left            =   1673
      TabIndex        =   2
      Top             =   2130
      Width           =   3555
   End
   Begin VB.CommandButton cmdReset 
      Caption         =   "Load the file and use the file's step size."
      Height          =   435
      Left            =   1673
      TabIndex        =   1
      Top             =   1110
      Width           =   3555
   End
   Begin VB.Label Label2 
      Alignment       =   2  'Center
      Caption         =   "What do you want to do?"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   285
      Left            =   915
      TabIndex        =   4
      Top             =   600
      Width           =   5085
   End
   Begin VB.Label Label1 
      Alignment       =   2  'Center
      Caption         =   "Loading a file will replace the current Step Size with the value in the file."
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   285
      Left            =   173
      TabIndex        =   0
      Top             =   210
      Width           =   6555
   End
End
Attribute VB_Name = "frmWarnBeforeLoad"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub chkWarnBeforeLoad_Click()
  Call iniPutData("Temp Comp", "Warn Before Load", chkWarnBeforeLoad.Value, glIniFile)
End Sub

Private Sub cmdReset_Click()
  warnResult = True
  resetStepSize = True
  Unload Me
End Sub

Private Sub cmdNoReset_Click()
  warnResult = True
  resetStepSize = False
  Unload Me
End Sub

Private Sub cmdSkip_Click()
  warnResult = False
  Unload Me
End Sub

Private Sub Form_Load()
  chkWarnBeforeLoad.Value = IniGetData("Temp Comp", "Warn Before Load", 1, glIniFile)
End Sub

