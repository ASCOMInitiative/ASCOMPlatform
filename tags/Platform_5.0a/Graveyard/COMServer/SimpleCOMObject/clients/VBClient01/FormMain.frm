VERSION 5.00
Begin VB.Form FormMain 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Form Main"
   ClientHeight    =   3090
   ClientLeft      =   45
   ClientTop       =   435
   ClientWidth     =   4680
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3090
   ScaleWidth      =   4680
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command_Invoke 
      Caption         =   "Invoke"
      Height          =   375
      Left            =   240
      TabIndex        =   2
      Top             =   960
      Width           =   4215
   End
   Begin VB.TextBox Text_LongPropertyValue 
      Height          =   375
      Left            =   2160
      TabIndex        =   1
      Top             =   360
      Width           =   2295
   End
   Begin VB.Label Label1 
      Caption         =   "Long Property Value :"
      Height          =   255
      Left            =   240
      TabIndex        =   0
      Top             =   360
      Width           =   1815
   End
End
Attribute VB_Name = "FormMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim SimpleCOMObject_CSharpImpl As SimpleCOMObject


Private Sub Command_Invoke_Click()
  SimpleCOMObject_CSharpImpl.LongProperty = Val(Text_LongPropertyValue)
  SimpleCOMObject_CSharpImpl.Method01 "From CSharp Impl. The Long Property Value is : "
End Sub

Private Sub Form_Load()
  Text_LongPropertyValue = "0"
  
  Set SimpleCOMObject_CSharpImpl = CreateObject("SimpleCOMObject_CSharpImpl.SimpleCOMObject")
End Sub

