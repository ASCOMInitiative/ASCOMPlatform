VERSION 5.00
Object = "{648A5603-2C6E-101B-82B6-000000000014}#1.1#0"; "MSCOMM32.OCX"
Begin VB.Form frmCtrlHolder 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Control holder form"
   ClientHeight    =   1935
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4425
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1935
   ScaleWidth      =   4425
   StartUpPosition =   3  'Windows Default
   Begin MSCommLib.MSComm SerialPort 
      Left            =   420
      Top             =   645
      _ExtentX        =   1005
      _ExtentY        =   1005
      _Version        =   393216
   End
   Begin VB.Label Label2 
      Caption         =   "Serial (COM) port interface control"
      Height          =   480
      Left            =   165
      TabIndex        =   1
      Top             =   1335
      Width           =   1275
   End
   Begin VB.Label Label1 
      Caption         =   "This form is never seen, it exists simply to hold instances of custom controls (OCXes) needed by the telescope driver."
      Height          =   465
      Left            =   120
      TabIndex        =   0
      Top             =   105
      Width           =   4425
   End
End
Attribute VB_Name = "frmCtrlHolder"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
