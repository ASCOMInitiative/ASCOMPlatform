VERSION 5.00
Begin VB.Form frmHandBox 
   BackColor       =   &H00000000&
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Pipe"
   ClientHeight    =   9075
   ClientLeft      =   45
   ClientTop       =   375
   ClientWidth     =   5955
   Icon            =   "frmHandBox.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   9075
   ScaleWidth      =   5955
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txtTraffic 
      BackColor       =   &H00404040&
      ForeColor       =   &H00FFFFFF&
      Height          =   5205
      Left            =   120
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   17
      TabStop         =   0   'False
      Top             =   3720
      Width           =   5715
   End
   Begin VB.Frame frmTrafficControl 
      BackColor       =   &H00000000&
      Caption         =   "Traffic Control"
      ForeColor       =   &H00FFFFFF&
      Height          =   2655
      Left            =   120
      TabIndex        =   16
      Top             =   840
      Width           =   5715
      Begin VB.CommandButton cmdUnsetAll 
         Caption         =   "Un-Set All"
         Height          =   315
         Left            =   4200
         MaskColor       =   &H8000000F&
         TabIndex        =   12
         ToolTipText     =   "Set all traffic control checkboxes"
         Top             =   1440
         Width           =   1155
      End
      Begin VB.CommandButton cmdSetAll 
         Caption         =   "Set All"
         Height          =   315
         Left            =   4200
         MaskColor       =   &H8000000F&
         TabIndex        =   11
         ToolTipText     =   "Set all traffic control checkboxes"
         Top             =   960
         Width           =   1155
      End
      Begin VB.CommandButton cmdMark 
         Caption         =   "Mark"
         Height          =   405
         Left            =   2400
         MaskColor       =   &H8000000F&
         TabIndex        =   13
         ToolTipText     =   "Place a Mark in the log for discovery later"
         Top             =   1920
         Width           =   795
      End
      Begin VB.CommandButton cmdClear 
         Caption         =   "Clear"
         Height          =   405
         Left            =   4560
         MaskColor       =   &H8000000F&
         TabIndex        =   15
         ToolTipText     =   "Clear window and current log file"
         Top             =   1920
         Width           =   795
      End
      Begin VB.CheckBox chkSlew 
         BackColor       =   &H00000000&
         Caption         =   "Slews, Syncs, Moves, Park/Unpark, FindHome"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   240
         TabIndex        =   4
         Top             =   360
         Value           =   1  'Checked
         Width           =   3945
      End
      Begin VB.CheckBox chkPoll 
         BackColor       =   &H00000000&
         Caption         =   "Polls: Tracking, Slewing, Moving, At's"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   240
         TabIndex        =   5
         Top             =   660
         Width           =   3585
      End
      Begin VB.CheckBox chkCoord 
         BackColor       =   &H00000000&
         Caption         =   "Get: Alt/Az, RA/Dec, Targets, Position"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   240
         TabIndex        =   6
         Top             =   960
         Value           =   1  'Checked
         Width           =   3705
      End
      Begin VB.CheckBox chkCap 
         BackColor       =   &H00000000&
         Caption         =   "Capabilities: Can Flags, AlignmentMode, etc."
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   240
         TabIndex        =   7
         Top             =   1260
         Width           =   3705
      End
      Begin VB.CheckBox chkTime 
         BackColor       =   &H00000000&
         Caption         =   "UTC, Siderial Time"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   240
         TabIndex        =   8
         Top             =   1560
         Width           =   1860
      End
      Begin VB.CheckBox chkOther 
         BackColor       =   &H00000000&
         Caption         =   "All Other"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   240
         TabIndex        =   10
         Top             =   2160
         Width           =   1380
      End
      Begin VB.CommandButton cmdDisable 
         Caption         =   "Disable"
         Height          =   405
         Left            =   3480
         MaskColor       =   &H8000000F&
         TabIndex        =   14
         ToolTipText     =   "Stop or Restart logging"
         Top             =   1920
         Width           =   795
      End
      Begin VB.CheckBox chkShutter 
         BackColor       =   &H00000000&
         Caption         =   "Dome Shutter Calls"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   240
         TabIndex        =   9
         Top             =   1860
         Width           =   1785
      End
      Begin VB.Image imgBrewster 
         Height          =   555
         Left            =   4320
         MouseIcon       =   "frmHandBox.frx":08CA
         MousePointer    =   99  'Custom
         Picture         =   "frmHandBox.frx":0A1C
         ToolTipText     =   "Click to go to Jon Brewster's astro web site"
         Top             =   240
         Width           =   1170
      End
   End
   Begin VB.CommandButton cmdConnectFocuser 
      Caption         =   "Connect Focuser"
      Height          =   450
      Left            =   3280
      TabIndex        =   2
      ToolTipText     =   "Take control of the focuser"
      Top             =   240
      Width           =   1005
   End
   Begin VB.CommandButton cmdConnectDome 
      Caption         =   "Connect Dome"
      Height          =   450
      Left            =   1760
      TabIndex        =   1
      ToolTipText     =   "Take control of the dome"
      Top             =   240
      Width           =   1005
   End
   Begin VB.CommandButton cmdConnectScope 
      Caption         =   "Connect Scope"
      Height          =   450
      Left            =   240
      TabIndex        =   0
      ToolTipText     =   "Take control of the scope"
      Top             =   240
      Width           =   1005
   End
   Begin VB.CommandButton cmdSetup 
      Caption         =   "Setup"
      Height          =   450
      Left            =   4800
      TabIndex        =   3
      ToolTipText     =   "Bring up Pipe setup dialog"
      Top             =   240
      Width           =   1005
   End
End
Attribute VB_Name = "frmHandBox"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' -----------------------------------------------------------------------------
'   ==============
'   FRMHANDBOX.FRM
'   ==============
'
' Pipe hand box form
'
' Written: Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 03-Sep-06 jab     Initial edit
' -----------------------------------------------------------------------------
Option Explicit

Private Const LOGLENGTH = 2000
Private m_bDisable As Boolean
Private m_bBOL As Boolean

Private fso As Object
Private log As Object

' ======
' EVENTS
' ======

Private Sub Form_Load()

    On Error Resume Next
    
    ' start out disabled
    m_bDisable = True
    
    If App.Title = "Pipe" Then
        m_bDisable = False
        Me.cmdDisable.Caption = "Disable"
        
        ' get the old file log saved
        Set fso = CreateObject("Scripting.FileSystemObject")
        
        Set log = fso.GetFile(App.Path & "\Pipe.bak")
        log.Delete
        Set log = Nothing
        
        Set log = fso.GetFile(App.Path & "\Pipe.log")
        log.Move (App.Path & "\Pipe.bak")
        Set log = Nothing
        
        ' initialize reporting field and file for logging
        cmdClear_Click
    End If
    
    On Error GoTo 0
    
End Sub

Private Sub Form_Unload(Cancel As Integer)

    On Error Resume Next
  
    ' Take care of the HW
    DoShutdown
    
    If App.Title = "Pipe" Then
        ' clean out local objects
        log.Close
        Set log = Nothing
        Set fso = Nothing
    End If
    
    On Error GoTo 0
    
End Sub

Private Sub Form_Resize()

    Dim TextOffset As Integer
        
    If WindowState = vbNormal Or WindowState = vbMaximized Then
        TextOffset = txtTraffic.Top + 600
        
        If Width < 5415 Then _
            Width = 5415
        If Height < TextOffset + 400 Then _
            Height = TextOffset + 400
            
        txtTraffic.Height = Height - TextOffset
        txtTraffic.Width = Width - 300
    End If
  
End Sub

Public Sub cmdSetAll_Click()

    chkSlew.Value = 1
    chkPoll.Value = 1
    chkCoord.Value = 1
    chkCap.Value = 1
    chkTime.Value = 1
    chkShutter.Value = 1
    chkOther.Value = 1
    
End Sub

Private Sub cmdUnsetAll_Click()

    chkSlew.Value = 0
    chkPoll.Value = 0
    chkCoord.Value = 0
    chkCap.Value = 0
    chkTime.Value = 0
    chkShutter.Value = 0
    chkOther.Value = 0
    
End Sub

Public Sub cmdClear_Click()

    On Error Resume Next
    
    txtTraffic.Text = ""
    txtTraffic.SelStart = 0
    
    log.Close
    Set log = Nothing
    Set log = fso.CreateTextFile(App.Path & "\Pipe.log", True)
    
    m_bBOL = True
    
    On Error GoTo 0

End Sub

Private Sub cmdDisable_Click()

    On Error Resume Next
    
    m_bDisable = Not m_bDisable
    Me.cmdDisable.Caption = IIf(m_bDisable, "Enable", "Disable")
    
    If m_bDisable Then
        If Not m_bBOL Then
            log.WriteBlankLines (1)
            txtTraffic.Text = Right(txtTraffic.Text & vbCrLf, LOGLENGTH)
        End If
        log.WriteLine ("--------- Logging Disabled ---------")
        txtTraffic.Text = Right(txtTraffic.Text & vbCrLf, LOGLENGTH)
        txtTraffic.SelStart = Len(txtTraffic.Text)
    Else
        log.WriteBlankLines (2)
        log.WriteLine ("-------- Logging Re-enabled --------")
    End If
    
    m_bBOL = True
    
    On Error GoTo 0
    
End Sub

Private Sub cmdMark_Click()

    Dim val As String

    On Error Resume Next
    
    val = IIf(m_bBOL, "", vbCrLf)
    val = val & vbCrLf & "--------------- Mark ---------------" & vbCrLf & vbCrLf
    
    log.Write (val)
    
    txtTraffic.Text = Right(txtTraffic.Text & val, LOGLENGTH)
    txtTraffic.SelStart = Len(txtTraffic.Text)
    
    m_bBOL = True
    
    On Error GoTo 0
    
End Sub

Private Sub imgBrewster_Click()

    DisplayWebPage "http://astro.brewsters.net/"
    
End Sub

Private Sub cmdConnectFocuser_Click()

    g_setupDlg.ConnectFocuser True
    
End Sub

Private Sub cmdConnectDome_Click()

    g_setupDlg.ConnectDome True
    
End Sub

Private Sub cmdConnectScope_Click()
   
    g_setupDlg.ConnectScope True
    
End Sub

Private Sub cmdSetup_Click()

    DoSetup                         ' May change our topmost state
    
End Sub

' ===============
' PUBLIC ROUTINES
' ===============

Public Sub TrafficChar(val As String)

    If m_bDisable Then _
        Exit Sub

    On Error Resume Next
    
    If m_bBOL Then
        log.Write (val)
        txtTraffic.Text = Right(txtTraffic.Text & val, LOGLENGTH)
        m_bBOL = False
    Else
        log.Write (" " & val)
        txtTraffic.Text = Right(txtTraffic.Text & " " & val, LOGLENGTH)
    End If
     
    txtTraffic.SelStart = Len(txtTraffic.Text)
    
    On Error GoTo 0
    
End Sub

Public Sub TrafficLine(val As String)

    If m_bDisable Then _
        Exit Sub

    On Error Resume Next
        
    If m_bBOL Then
        log.WriteLine (val)
        val = val & vbCrLf
    Else
        log.WriteLine (vbCrLf & val)
        val = vbCrLf & val & vbCrLf
    End If
     
    txtTraffic.Text = Right(txtTraffic.Text & val, LOGLENGTH)
    txtTraffic.SelStart = Len(txtTraffic.Text)
    m_bBOL = True
    
    On Error GoTo 0
    
End Sub

Public Sub TrafficStart(val As String)

    If m_bDisable Then _
        Exit Sub

    On Error Resume Next
    
    If Not m_bBOL Then _
        val = vbCrLf & val
   
    log.Write (val)
    txtTraffic.Text = Right(txtTraffic.Text & val, LOGLENGTH)
    txtTraffic.SelStart = Len(txtTraffic.Text)
    
    m_bBOL = False
    
    On Error GoTo 0
    
End Sub

Public Sub TrafficEnd(val As String)

    If m_bDisable Then _
        Exit Sub

    On Error Resume Next
    
    log.WriteLine (val)
    txtTraffic.Text = Right(txtTraffic.Text & val & vbCrLf, LOGLENGTH)
    txtTraffic.SelStart = Len(txtTraffic.Text)
    
    m_bBOL = True
    
    On Error GoTo 0
    
End Sub
