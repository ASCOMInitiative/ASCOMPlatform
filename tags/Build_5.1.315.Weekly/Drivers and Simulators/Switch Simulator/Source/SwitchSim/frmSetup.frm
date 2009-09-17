VERSION 5.00
Begin VB.Form frmSetup 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "ASCOM Focuser Simulator Setup"
   ClientHeight    =   6885
   ClientLeft      =   45
   ClientTop       =   285
   ClientWidth     =   6705
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   6855.324
   ScaleMode       =   0  'User
   ScaleWidth      =   6705
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame Frame3 
      BackColor       =   &H00000000&
      Caption         =   "Name"
      ForeColor       =   &H00FFFFFF&
      Height          =   4800
      Left            =   2520
      TabIndex        =   27
      Top             =   1005
      Width           =   2085
      Begin VB.TextBox txtName 
         Height          =   315
         Index           =   8
         Left            =   240
         TabIndex        =   36
         Top             =   4200
         Width           =   1605
      End
      Begin VB.TextBox txtName 
         Height          =   315
         Index           =   7
         Left            =   240
         TabIndex        =   35
         Top             =   3720
         Width           =   1605
      End
      Begin VB.TextBox txtName 
         Height          =   315
         Index           =   6
         Left            =   240
         TabIndex        =   34
         Top             =   3240
         Width           =   1605
      End
      Begin VB.TextBox txtName 
         Height          =   315
         Index           =   5
         Left            =   240
         TabIndex        =   33
         Top             =   2760
         Width           =   1605
      End
      Begin VB.TextBox txtName 
         Height          =   315
         Index           =   4
         Left            =   240
         TabIndex        =   32
         Top             =   2280
         Width           =   1605
      End
      Begin VB.TextBox txtName 
         Height          =   315
         Index           =   3
         Left            =   240
         TabIndex        =   31
         Top             =   1800
         Width           =   1605
      End
      Begin VB.TextBox txtName 
         Height          =   315
         Index           =   2
         Left            =   240
         TabIndex        =   30
         Top             =   1320
         Width           =   1605
      End
      Begin VB.TextBox txtName 
         Height          =   315
         Index           =   1
         Left            =   240
         TabIndex        =   29
         Top             =   840
         Width           =   1605
      End
      Begin VB.TextBox txtName 
         Height          =   315
         Index           =   0
         Left            =   240
         TabIndex        =   28
         Top             =   360
         Width           =   1605
      End
   End
   Begin VB.CheckBox chkZero 
      BackColor       =   &H00000000&
      Caption         =   "Include Switch &Zero"
      ForeColor       =   &H00FFFFFF&
      Height          =   420
      Left            =   4980
      TabIndex        =   19
      Top             =   1470
      Width           =   1230
   End
   Begin VB.Frame Frame2 
      BackColor       =   &H00000000&
      Caption         =   "Set"
      ForeColor       =   &H00FFFFFF&
      Height          =   4800
      Left            =   1350
      TabIndex        =   26
      Top             =   1005
      Width           =   885
      Begin VB.CheckBox chkSet 
         BackColor       =   &H00000000&
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   0
         Left            =   195
         TabIndex        =   9
         Top             =   435
         Width           =   555
      End
      Begin VB.CheckBox chkSet 
         BackColor       =   &H00000000&
         Caption         =   "1"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   1
         Left            =   195
         TabIndex        =   10
         Top             =   913
         Width           =   555
      End
      Begin VB.CheckBox chkSet 
         BackColor       =   &H00000000&
         Caption         =   "2"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   2
         Left            =   195
         TabIndex        =   11
         Top             =   1391
         Width           =   555
      End
      Begin VB.CheckBox chkSet 
         BackColor       =   &H00000000&
         Caption         =   "3"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   3
         Left            =   195
         TabIndex        =   12
         Top             =   1869
         Width           =   555
      End
      Begin VB.CheckBox chkSet 
         BackColor       =   &H00000000&
         Caption         =   "4"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   4
         Left            =   195
         TabIndex        =   13
         Top             =   2347
         Width           =   555
      End
      Begin VB.CheckBox chkSet 
         BackColor       =   &H00000000&
         Caption         =   "5"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   5
         Left            =   195
         TabIndex        =   14
         Top             =   2825
         Width           =   555
      End
      Begin VB.CheckBox chkSet 
         BackColor       =   &H00000000&
         Caption         =   "6"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   6
         Left            =   195
         TabIndex        =   15
         Top             =   3303
         Width           =   555
      End
      Begin VB.CheckBox chkSet 
         BackColor       =   &H00000000&
         Caption         =   "7"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   7
         Left            =   195
         TabIndex        =   16
         Top             =   3781
         Width           =   555
      End
      Begin VB.CheckBox chkSet 
         BackColor       =   &H00000000&
         Caption         =   "8"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   8
         Left            =   195
         TabIndex        =   17
         Top             =   4260
         Width           =   555
      End
   End
   Begin VB.ComboBox cbMaxSwitch 
      Height          =   315
      ItemData        =   "frmSetup.frx":0000
      Left            =   6000
      List            =   "frmSetup.frx":001F
      Style           =   2  'Dropdown List
      TabIndex        =   18
      Top             =   1005
      Width           =   555
   End
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   5430
      MouseIcon       =   "frmSetup.frx":003E
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":0190
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   24
      TabStop         =   0   'False
      Top             =   2880
      Width           =   720
   End
   Begin VB.Frame Frame1 
      BackColor       =   &H00000000&
      Caption         =   "Get"
      ForeColor       =   &H00FFFFFF&
      Height          =   4800
      Left            =   195
      TabIndex        =   23
      Top             =   1005
      Width           =   885
      Begin VB.CheckBox chkGet 
         BackColor       =   &H00000000&
         Caption         =   "8"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   8
         Left            =   195
         TabIndex        =   8
         Top             =   4260
         Width           =   555
      End
      Begin VB.CheckBox chkGet 
         BackColor       =   &H00000000&
         Caption         =   "7"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   7
         Left            =   195
         TabIndex        =   7
         Top             =   3781
         Width           =   555
      End
      Begin VB.CheckBox chkGet 
         BackColor       =   &H00000000&
         Caption         =   "6"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   6
         Left            =   195
         TabIndex        =   6
         Top             =   3303
         Width           =   555
      End
      Begin VB.CheckBox chkGet 
         BackColor       =   &H00000000&
         Caption         =   "5"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   5
         Left            =   195
         TabIndex        =   5
         Top             =   2825
         Width           =   555
      End
      Begin VB.CheckBox chkGet 
         BackColor       =   &H00000000&
         Caption         =   "4"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   4
         Left            =   195
         TabIndex        =   4
         Top             =   2347
         Width           =   555
      End
      Begin VB.CheckBox chkGet 
         BackColor       =   &H00000000&
         Caption         =   "3"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   3
         Left            =   195
         TabIndex        =   3
         Top             =   1869
         Width           =   555
      End
      Begin VB.CheckBox chkGet 
         BackColor       =   &H00000000&
         Caption         =   "2"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   2
         Left            =   195
         TabIndex        =   2
         Top             =   1391
         Width           =   555
      End
      Begin VB.CheckBox chkGet 
         BackColor       =   &H00000000&
         Caption         =   "1"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   1
         Left            =   195
         TabIndex        =   1
         Top             =   913
         Width           =   555
      End
      Begin VB.CheckBox chkGet 
         BackColor       =   &H00000000&
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   0
         Left            =   195
         TabIndex        =   0
         Top             =   435
         Width           =   555
      End
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "&Cancel"
      Height          =   345
      Left            =   5370
      TabIndex        =   20
      Top             =   4980
      Width           =   1185
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   345
      Left            =   5370
      TabIndex        =   21
      Top             =   5565
      Width           =   1185
   End
   Begin VB.Image imgBrewster 
      Height          =   555
      Left            =   2767
      MouseIcon       =   "frmSetup.frx":1054
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":11A6
      Top             =   240
      Width           =   1170
   End
   Begin VB.Label Label1 
      BackColor       =   &H00000000&
      Caption         =   "&MaxSwitch:"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   4980
      TabIndex        =   25
      Top             =   1020
      Width           =   945
   End
   Begin VB.Label lblDriverInfo 
      BackColor       =   &H00000000&
      Caption         =   "<run time - version etc.>"
      ForeColor       =   &H00FFFFFF&
      Height          =   480
      Left            =   240
      TabIndex        =   22
      Top             =   6120
      Width           =   5895
   End
End
Attribute VB_Name = "frmSetup"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'   ============
'   FRMSETUP.FRM
'   ============
'
' ASCOM Switch Simulator setup form
'
' Initial code by Jon Brewster in Feb 2007
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 29-Jan-07 jab     Initial edit
' 02-Jun-07 jab     Added naming lables to the switches
' -----------------------------------------------------------------------------

Option Explicit

Private m_bResult As Boolean
Private m_bAllowUnload As Boolean

' ======
' EVENTS
' ======

Private Sub Form_Load()
    
    Dim fs, F
    Dim DLM As String
     
    FloatWindow Me.hwnd, True                       ' Setup window always floats
    m_bAllowUnload = True                           ' Start out allowing unload
    
    Set fs = CreateObject("Scripting.FileSystemObject")
    Set F = fs.GetFile(App.Path & "\" & App.EXEName & ".exe")
    DLM = F.DateLastModified
    
    lblDriverInfo = App.FileDescription & " " & _
        App.Major & "." & App.Minor & "." & App.Revision & vbCrLf & _
        "Modified " & DLM
    
End Sub

Private Sub Form_Unload(Cancel As Integer)

    Me.Hide                                     ' Assure we don't unload
    Cancel = Not m_bAllowUnload                 ' Unless our flag permits it
    
End Sub

Private Sub cbMaxSwitch_Click()
    Dim i As Integer

    chkGet(0).Enabled = (chkZero.Value = 1)
    chkSet(0).Enabled = (chkZero.Value = 1)
    
    For i = 1 To (NUM_SWITCHES - 1)
        chkGet(i).Enabled = (i <= cbMaxSwitch.ListIndex)
        chkSet(i).Enabled = (i <= cbMaxSwitch.ListIndex)
        txtName(i).Enabled = (i <= cbMaxSwitch.ListIndex)
    Next i
    
End Sub

Private Sub chkZero_Click()

    cbMaxSwitch_Click

End Sub

Private Sub cmdOK_Click()

    m_bResult = True
    Me.Hide

End Sub

Private Sub cmdCancel_Click()

    m_bResult = False
    Me.Hide

End Sub

Private Sub imgBrewster_Click()

    DisplayWebPage "http://astro.brewsters.net/"
    
End Sub

Private Sub picASCOM_Click()

    DisplayWebPage "http://ascom-standards.org/"
    
End Sub

' =================
' PUBLIC PROPERTIES
' =================

Public Property Get Result() As Boolean

    Result = m_bResult              ' Set by OK or Cancel button
    
End Property

Public Property Let AllowUnload(b As Boolean)

    m_bAllowUnload = b
    
End Property
