VERSION 5.00
Begin VB.Form frmShow 
   BackColor       =   &H00000000&
   Caption         =   "ASCOM API Traffic"
   ClientHeight    =   5805
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   5310
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
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   5805
   ScaleWidth      =   5310
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdDisable 
      Caption         =   "Disable"
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
      Left            =   4200
      MaskColor       =   &H8000000F&
      TabIndex        =   2
      Top             =   120
      Width           =   915
   End
   Begin VB.CheckBox chkOther 
      BackColor       =   &H00000000&
      Caption         =   "Other"
      ForeColor       =   &H00FFFFFF&
      Height          =   195
      Left            =   2490
      TabIndex        =   1
      Top             =   345
      Width           =   1350
   End
   Begin VB.CheckBox chkActivity 
      BackColor       =   &H00000000&
      Caption         =   "Switch Activity"
      ForeColor       =   &H00FFFFFF&
      Height          =   195
      Left            =   120
      TabIndex        =   0
      Top             =   345
      Value           =   1  'Checked
      Width           =   2175
   End
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
      Height          =   255
      Left            =   4200
      MaskColor       =   &H8000000F&
      TabIndex        =   3
      Top             =   480
      Width           =   915
   End
   Begin VB.TextBox txtTraffic 
      BackColor       =   &H00404040&
      ForeColor       =   &H00FFFFFF&
      Height          =   4845
      Left            =   90
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   4
      Top             =   840
      Width           =   5115
   End
End
Attribute VB_Name = "frmShow"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'---------------------------------------------------------------------
'   ===========
'   FRMSHOW.FRM
'   ===========
'
' ASCOM Jornaling form
'
' Written:  31-May-03   Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 31-may-03 jab     Initial edit
' 17-Jun-17 jab     Modified for general use (LOGLENGTH, TextOffset)
' -----------------------------------------------------------------------------

Option Explicit

Private Const LOGLENGTH = 2000
Private m_iTextOffset As Integer
Private m_bDisable As Boolean
Private m_bBOL As Boolean

Private Sub cmdClear_Click()

  m_bBOL = True
  txtTraffic.Text = ""

End Sub

Private Sub Form_Load()

    cmdClear_Click
    m_bDisable = False
    Me.cmdDisable.Caption = "Disable"

End Sub

Private Sub Form_Unload(Cancel As Integer)

    If Not g_show Is Nothing Then _
        Set g_show = Nothing
    
End Sub

Private Sub Form_Resize()

    If Width <> 5415 Then _
        Width = 5415
    If Height < m_iTextOffset + 400 Then
        Height = m_iTextOffset + 400
    Else
        txtTraffic.Height = Height - m_iTextOffset
    End If
  
End Sub

Private Sub cmdDisable_Click()

    m_bDisable = Not m_bDisable
    Me.cmdDisable.Caption = IIf(m_bDisable, "Enable", "Disable")
    
End Sub

Public Sub TrafficChar(val As String)

    If m_bDisable Then _
        Exit Sub

    If m_bBOL Then
        m_bBOL = False
        txtTraffic.Text = Right(txtTraffic.Text & val, LOGLENGTH)
    Else
        txtTraffic.Text = Right(txtTraffic.Text & " " & val, LOGLENGTH)
    End If
     
    txtTraffic.SelStart = Len(txtTraffic.Text)
    
End Sub

Public Sub TrafficLine(val As String)

    If m_bDisable Then _
        Exit Sub
        
    If m_bBOL Then
        val = val & vbCrLf
    Else
        val = vbCrLf & val & vbCrLf
    End If
    
    m_bBOL = True
     
    txtTraffic.Text = Right(txtTraffic.Text & val, LOGLENGTH)
    txtTraffic.SelStart = Len(txtTraffic.Text)
    
End Sub

Public Sub TrafficStart(val As String)

    If m_bDisable Then _
        Exit Sub
        
    If Not m_bBOL Then _
        val = vbCrLf & val
   
    m_bBOL = False
     
    txtTraffic.Text = Right(txtTraffic.Text & val, LOGLENGTH)
    txtTraffic.SelStart = Len(txtTraffic.Text)
    
End Sub

Public Sub TrafficEnd(val As String)

    If m_bDisable Then _
        Exit Sub
    
    val = val & vbCrLf
   
    m_bBOL = True
     
    txtTraffic.Text = Right(txtTraffic.Text & val, LOGLENGTH)
    txtTraffic.SelStart = Len(txtTraffic.Text)
    
End Sub

Public Property Let TextOffset(newVal As Integer)
        
    m_iTextOffset = newVal
    txtTraffic.Height = Height - m_iTextOffset
    
End Property


