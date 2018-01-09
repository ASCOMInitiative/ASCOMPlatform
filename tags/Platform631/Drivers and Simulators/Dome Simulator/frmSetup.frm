VERSION 5.00
Begin VB.Form frmSetup 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Dome Simulator Setup"
   ClientHeight    =   7065
   ClientLeft      =   45
   ClientTop       =   285
   ClientWidth     =   5415
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   7034.547
   ScaleMode       =   0  'User
   ScaleWidth      =   5415
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame NonStd 
      BackColor       =   &H00000000&
      Caption         =   "Non-Standard Behavior"
      ForeColor       =   &H00FFFFFF&
      Height          =   960
      Left            =   240
      TabIndex        =   34
      Top             =   5655
      Width           =   4935
      Begin VB.CheckBox chkStartShutterError 
         BackColor       =   &H00000000&
         Caption         =   "Start up with shutter error"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   2730
         TabIndex        =   19
         Top             =   330
         Width           =   2115
      End
      Begin VB.CheckBox chkNonFragileAtHome 
         BackColor       =   &H00000000&
         Caption         =   "AtHome without FindHome"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   240
         TabIndex        =   17
         Top             =   360
         Width           =   2310
      End
      Begin VB.CheckBox chkNonFragileAtPark 
         BackColor       =   &H00000000&
         Caption         =   "AtPark without Park()"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   240
         TabIndex        =   18
         Top             =   615
         Width           =   2010
      End
      Begin VB.CheckBox chkSlewingOpenClose 
         BackColor       =   &H00000000&
         Caption         =   "Slewing on open/close"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   2730
         TabIndex        =   20
         Top             =   600
         Width           =   2130
      End
   End
   Begin VB.Frame frmShutterControl 
      BackColor       =   &H00000000&
      Caption         =   "Shutter Control"
      ForeColor       =   &H00FFFFFF&
      Height          =   2445
      Left            =   240
      TabIndex        =   28
      Top             =   840
      Width           =   2175
      Begin VB.TextBox txtAltRate 
         Height          =   315
         Left            =   1260
         TabIndex        =   0
         Top             =   360
         Width           =   765
      End
      Begin VB.TextBox txtOCDelay 
         Height          =   315
         Left            =   1260
         TabIndex        =   1
         Top             =   900
         Width           =   765
      End
      Begin VB.TextBox txtMaxAlt 
         Height          =   315
         Left            =   1260
         TabIndex        =   2
         Top             =   1440
         Width           =   765
      End
      Begin VB.TextBox txtMinAlt 
         Height          =   315
         Left            =   1260
         TabIndex        =   3
         Top             =   1980
         Width           =   765
      End
      Begin VB.Label lblAltRate 
         BackColor       =   &H00000000&
         Caption         =   "Slew Rate (deg/sec):"
         ForeColor       =   &H00FFFFFF&
         Height          =   465
         Left            =   120
         TabIndex        =   33
         Top             =   300
         Width           =   930
      End
      Begin VB.Label lblOCDelay 
         BackColor       =   &H00000000&
         Caption         =   "Open/Close time (sec):"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   120
         TabIndex        =   31
         Top             =   840
         Width           =   885
      End
      Begin VB.Label lblMax 
         BackColor       =   &H00000000&
         Caption         =   "Maximum Altitude (deg):"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   120
         TabIndex        =   30
         Top             =   1380
         Width           =   1005
      End
      Begin VB.Label lblMin 
         BackColor       =   &H00000000&
         Caption         =   "Minimum Altitude (deg):"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   120
         TabIndex        =   29
         Top             =   1920
         Width           =   1005
      End
   End
   Begin VB.Frame InterfaceCap 
      BackColor       =   &H00000000&
      Caption         =   "Interface Capabilities"
      ForeColor       =   &H00FFFFFF&
      Height          =   1440
      Left            =   240
      TabIndex        =   27
      Top             =   4020
      Width           =   4935
      Begin VB.CheckBox chkCanSetPark 
         BackColor       =   &H00000000&
         Caption         =   "Set Park"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   2730
         TabIndex        =   15
         Top             =   600
         Width           =   1425
      End
      Begin VB.CheckBox chkCanSetAzimuth 
         BackColor       =   &H00000000&
         Caption         =   "Slew Azimuth"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   240
         TabIndex        =   13
         Top             =   1080
         Width           =   1545
      End
      Begin VB.CheckBox chkCanSetAltitude 
         BackColor       =   &H00000000&
         Caption         =   "Slew Altitude"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   240
         TabIndex        =   12
         Top             =   840
         Width           =   1485
      End
      Begin VB.CheckBox chkCanSyncAzimuth 
         BackColor       =   &H00000000&
         Caption         =   "Sync Azimuth"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   2730
         TabIndex        =   16
         Top             =   840
         Width           =   1695
      End
      Begin VB.CheckBox chkCanSetShutter 
         BackColor       =   &H00000000&
         Caption         =   "Open/Close Shutter"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   2730
         TabIndex        =   14
         Top             =   360
         Width           =   1905
      End
      Begin VB.CheckBox chkCanPark 
         BackColor       =   &H00000000&
         Caption         =   "Park"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   240
         TabIndex        =   11
         Top             =   600
         Width           =   1545
      End
      Begin VB.CheckBox chkCanFindHome 
         BackColor       =   &H00000000&
         Caption         =   "Find Home"
         ForeColor       =   &H00FFFFFF&
         Height          =   180
         Left            =   240
         TabIndex        =   10
         Top             =   360
         Width           =   1485
      End
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "&Cancel"
      Height          =   345
      Left            =   3000
      TabIndex        =   8
      Top             =   3555
      Width           =   930
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   345
      Left            =   4260
      TabIndex        =   9
      Top             =   3555
      Width           =   930
   End
   Begin VB.Frame MotionControl 
      BackColor       =   &H00000000&
      Caption         =   "Azimuth Control"
      ForeColor       =   &H00FFFFFF&
      Height          =   2445
      Left            =   2880
      TabIndex        =   21
      Top             =   840
      Width           =   2295
      Begin VB.TextBox txtStepSize 
         Height          =   315
         Left            =   1320
         TabIndex        =   5
         Top             =   900
         Width           =   765
      End
      Begin VB.TextBox txtHome 
         Height          =   315
         Left            =   1320
         TabIndex        =   7
         Top             =   1980
         Width           =   765
      End
      Begin VB.TextBox txtAzRate 
         Height          =   315
         Left            =   1320
         TabIndex        =   4
         Top             =   360
         Width           =   765
      End
      Begin VB.TextBox txtPark 
         Height          =   315
         Left            =   1320
         TabIndex        =   6
         Top             =   1440
         Width           =   765
      End
      Begin VB.Label lblHomePosition 
         BackColor       =   &H00000000&
         Caption         =   "Home Position (deg):"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   165
         TabIndex        =   32
         Top             =   1920
         Width           =   1095
      End
      Begin VB.Label lblParkPosition 
         BackColor       =   &H00000000&
         Caption         =   "Park Position (deg):"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   165
         TabIndex        =   25
         Top             =   1380
         Width           =   1095
      End
      Begin VB.Label lblAzRate 
         BackColor       =   &H00000000&
         Caption         =   "Slew Rate (deg/sec):"
         ForeColor       =   &H00FFFFFF&
         Height          =   405
         Left            =   165
         TabIndex        =   23
         Top             =   300
         Width           =   930
      End
      Begin VB.Label lblStepSize 
         BackColor       =   &H00000000&
         Caption         =   "Step Size (deg):"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   165
         TabIndex        =   22
         Top             =   840
         Width           =   765
      End
   End
   Begin VB.Label lblLastModified 
      Alignment       =   1  'Right Justify
      BackColor       =   &H00000000&
      Caption         =   "<last modified>"
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   2460
      TabIndex        =   26
      Top             =   6735
      Width           =   2715
   End
   Begin VB.Image imgBrewster 
      Height          =   555
      Left            =   2115
      MouseIcon       =   "frmSetup.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":0152
      Top             =   180
      Width           =   1170
   End
   Begin VB.Label lblDriverInfo 
      BackColor       =   &H00000000&
      Caption         =   "<version, etc.>"
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   240
      TabIndex        =   24
      Top             =   6735
      Width           =   2175
   End
End
Attribute VB_Name = "frmSetup"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'---------------------------------------------------------------------
' Copyright © 2000-2002 SPACE.com Inc., New York, NY
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
'   ============
'   FRMSETUP.FRM
'   ============
'
' ASCOM Dome Simulator setup form
'
' Written:  20-Jun-03   Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 20-Jun-03 jab     Initial edit
' 27-Jun-03 jab     Initial release
' 03-Dec-04 rbd     Add "Start up with shutter error" mode
' 06-Dec-04 rbd     More non-standard behavior controls, ugh.
' -----------------------------------------------------------------------------
Option Explicit

Private m_bResult As Boolean
Private m_bAllowUnload As Boolean

' ======
' EVENTS
' ======

Private Sub Form_Load()
    Dim tzName As String
    Dim l As Long
    Dim fs, F
    Dim DLM As String
    
    
    FloatWindow Me.hwnd, True                       ' Setup window always floats
    m_bAllowUnload = True                           ' Start out allowing unload
    
    lblDriverInfo = App.FileDescription & " " & _
        App.Major & "." & App.Minor & "." & App.Revision
        
    Set fs = CreateObject("Scripting.FileSystemObject")
    Set F = fs.GetFile(App.Path & "\DomeSim.exe")
    DLM = F.DateLastModified
    
    lblLastModified = "Modified " & DLM
        
End Sub

Private Sub Form_Unload(Cancel As Integer)

    Me.Hide                                     ' Assure we don't unload
    Cancel = Not m_bAllowUnload                 ' Unless our flag permits it
    
End Sub

Private Sub cmdCancel_Click()

    m_bResult = False
    Me.Hide

End Sub

Private Sub cmdOK_Click()

    m_bResult = True
    Me.Hide

End Sub


Private Sub imgBrewster_Click()

    DisplayWebPage "http://astro.brewsters.net/"
    
End Sub


' =================
' PUBLIC PROPERTIES
' =================

Public Property Let AllowUnload(b As Boolean)

    m_bAllowUnload = b
    
End Property

Public Property Let Home(newVal As Double)

    If newVal < -360 Or newVal > 360 Then
        txtHome.Text = "000.0"
    Else
        newVal = AzScale(newVal)
        txtHome.Text = Format$(newVal, "000.0")
    End If
    
End Property

Public Property Get Home() As Double

    Home = 0
    On Error Resume Next
    Home = CDbl(txtHome.Text)
    On Error GoTo 0
    
    Home = AzScale(Home)
    
End Property

Public Property Let OCDelay(newVal As Double)

    If newVal < 0 Then
        txtOCDelay.Text = "0"
    Else
        txtOCDelay.Text = Format$(newVal, "0")
    End If
    
End Property

Public Property Get OCDelay() As Double

    OCDelay = 0
    On Error Resume Next
    OCDelay = CDbl(txtOCDelay.Text)
    On Error GoTo 0
    
    If OCDelay > 30 Then _
        OCDelay = 30
        
End Property

Public Property Let Park(newVal As Double)

    If newVal < -360 Or newVal > 360 Then
        txtPark.Text = "000.0"
    Else
        newVal = AzScale(newVal)
        txtPark.Text = Format$(newVal, "000.0")
    End If
    
End Property

Public Property Get Park() As Double

    Park = 180
    On Error Resume Next
    Park = CDbl(txtPark.Text)
    On Error GoTo 0
    
    Park = AzScale(Park)
    
End Property

Public Property Get Result() As Boolean

    Result = m_bResult              ' Set by OK or Cancel button
    
End Property

Public Property Let AltRate(newVal As Double)
    
    txtAltRate.Text = Format$(newVal, "0.0")
        
End Property

Public Property Get AltRate() As Double

    AltRate = 10
    On Error Resume Next
    AltRate = CDbl(txtAltRate.Text)
    On Error GoTo 0
    
    If AltRate < 1 Then _
        AltRate = 1
    If AltRate > 90 Then _
        AltRate = 90

End Property

Public Property Let AzRate(newVal As Double)
    
    txtAzRate.Text = Format$(newVal, "0.0")
        
End Property

Public Property Get AzRate() As Double

    AzRate = 10
    On Error Resume Next
    AzRate = CDbl(txtAzRate.Text)
    On Error GoTo 0
    
    If AzRate < 1 Then _
        AzRate = 1
    If AzRate > 90 Then _
        AzRate = 90

End Property

Public Property Get MaxAlt() As Double

    MaxAlt = 90
    On Error Resume Next
    MaxAlt = CDbl(txtMaxAlt.Text)
    On Error GoTo 0
    
    If MaxAlt < 0 Then _
        MaxAlt = 0
    If MaxAlt > 90 Then _
        MaxAlt = 90

End Property

Public Property Let MaxAlt(newVal As Double)
    
    txtMaxAlt.Text = Format$(newVal, "0.0")
        
End Property

Public Property Get MinAlt() As Double

    MinAlt = 0
    On Error Resume Next
    MinAlt = CDbl(txtMinAlt.Text)
    On Error GoTo 0
    
    If MinAlt < 0 Then _
        MinAlt = 0
    If MinAlt > 90 Then _
        MinAlt = 90

End Property

Public Property Let MinAlt(newVal As Double)
    
    txtMinAlt.Text = Format$(newVal, "0.0")
        
End Property

Public Property Let StepSize(newVal As Double)
    Dim index As Integer
    
    txtStepSize.Text = Format$(newVal, "0.0")
    
End Property

Public Property Get StepSize() As Double

    StepSize = 1
    On Error Resume Next
    StepSize = CDbl(txtStepSize.Text)
    On Error GoTo 0
    
    If StepSize < 1 Then _
        StepSize = 1
    If StepSize > 90 Then _
        StepSize = 90

End Property

'
' LOCAL UTILITIES
'

