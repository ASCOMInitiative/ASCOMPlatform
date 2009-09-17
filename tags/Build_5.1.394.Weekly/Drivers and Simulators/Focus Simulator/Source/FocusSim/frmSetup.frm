VERSION 5.00
Begin VB.Form frmSetup 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "ASCOM Focuser Simulator Setup"
   ClientHeight    =   7140
   ClientLeft      =   45
   ClientTop       =   285
   ClientWidth     =   5535
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   7109.225
   ScaleMode       =   0  'User
   ScaleWidth      =   5535
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdResetPos 
      Caption         =   "Reset Position"
      Height          =   345
      Left            =   4035
      TabIndex        =   6
      Top             =   2760
      Width           =   1350
   End
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   270
      MouseIcon       =   "frmSetup.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":0152
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   24
      Top             =   2910
      Width           =   720
   End
   Begin VB.CheckBox chkAlwaysTop 
      BackColor       =   &H00000000&
      Caption         =   "Always on Top"
      ForeColor       =   &H00FFFFFF&
      Height          =   255
      Left            =   1425
      TabIndex        =   5
      ToolTipText     =   "Focus handset will float on top"
      Top             =   2895
      Value           =   1  'Checked
      Width           =   1575
   End
   Begin VB.CommandButton cmdAdvanced 
      Caption         =   "Advanced >>"
      Height          =   345
      Left            =   1440
      TabIndex        =   7
      Top             =   3390
      Width           =   1350
   End
   Begin VB.Frame Frame3 
      BackColor       =   &H00000000&
      Caption         =   "Capabilities"
      ForeColor       =   &H00FFFFFF&
      Height          =   2655
      Left            =   120
      TabIndex        =   23
      Top             =   4080
      Width           =   2175
      Begin VB.CheckBox chkSynchronous 
         BackColor       =   &H00000000&
         Caption         =   "Synchronous"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   240
         TabIndex        =   14
         ToolTipText     =   "Allow Halt programatically"
         Top             =   2220
         Width           =   1365
      End
      Begin VB.CheckBox chkCanTemp 
         BackColor       =   &H00000000&
         Caption         =   "Temperature Probe"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   240
         TabIndex        =   10
         ToolTipText     =   "Simulate a temperture probe"
         Top             =   300
         Width           =   1665
      End
      Begin VB.CheckBox chkCanTempComp 
         BackColor       =   &H00000000&
         Caption         =   "Temperature Compensation"
         ForeColor       =   &H00FFFFFF&
         Height          =   435
         Left            =   240
         TabIndex        =   11
         ToolTipText     =   "Allow temperature compensation"
         Top             =   708
         Width           =   1380
      End
      Begin VB.CheckBox chkCanHalt 
         BackColor       =   &H00000000&
         Caption         =   "Halt"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   240
         TabIndex        =   13
         ToolTipText     =   "Allow Halt programatically"
         Top             =   1794
         Width           =   645
      End
      Begin VB.CheckBox chkCanStepSize 
         BackColor       =   &H00000000&
         Caption         =   "Step Size"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   240
         TabIndex        =   12
         ToolTipText     =   "Allow Step Size to be programatically returned"
         Top             =   1371
         Width           =   1065
      End
   End
   Begin VB.Frame Frame2 
      BackColor       =   &H00000000&
      Caption         =   "Focuser Type"
      ForeColor       =   &H00FFFFFF&
      Height          =   1335
      Index           =   0
      Left            =   3840
      TabIndex        =   21
      Top             =   960
      Width           =   1575
      Begin VB.OptionButton optType 
         BackColor       =   &H00000000&
         Caption         =   "Relative"
         ForeColor       =   &H00FFFFFF&
         Height          =   240
         Index           =   1
         Left            =   195
         TabIndex        =   4
         ToolTipText     =   "Simulate a relative focuser"
         Top             =   825
         Width           =   1215
      End
      Begin VB.OptionButton optType 
         BackColor       =   &H00000000&
         Caption         =   "Absolute"
         ForeColor       =   &H00FFFFFF&
         Height          =   240
         Index           =   0
         Left            =   195
         TabIndex        =   3
         ToolTipText     =   "Simulate on absolute focuser"
         Top             =   345
         Width           =   1215
      End
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "&Cancel"
      Height          =   345
      Left            =   3390
      TabIndex        =   8
      Top             =   3390
      Width           =   930
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   345
      Left            =   4470
      TabIndex        =   9
      Top             =   3390
      Width           =   930
   End
   Begin VB.Frame Frame1 
      BackColor       =   &H00000000&
      Caption         =   "Focuser Settings"
      ForeColor       =   &H00FFFFFF&
      Height          =   1620
      Left            =   120
      TabIndex        =   20
      Top             =   960
      Width           =   3510
      Begin VB.TextBox MaxIncTF 
         Height          =   285
         Left            =   1800
         TabIndex        =   2
         ToolTipText     =   "Programatic limit for the  move method"
         Top             =   1200
         Width           =   1410
      End
      Begin VB.TextBox MaxStepTF 
         Height          =   315
         Left            =   1800
         TabIndex        =   0
         ToolTipText     =   "Focuser can't go beyond this"
         Top             =   240
         Width           =   1410
      End
      Begin VB.TextBox StepSizeTF 
         Height          =   315
         Left            =   1800
         TabIndex        =   1
         ToolTipText     =   "One step equals X microns at the focus plane"
         Top             =   720
         Width           =   1410
      End
      Begin VB.Label MaxIncL 
         BackColor       =   &H00000000&
         Caption         =   "Maximum increment:"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   165
         TabIndex        =   27
         Top             =   1200
         Width           =   1485
      End
      Begin VB.Label StepSizeL 
         BackColor       =   &H00000000&
         Caption         =   "Step Size (microns):"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   165
         TabIndex        =   26
         Top             =   795
         Width           =   1485
      End
      Begin VB.Label MaxStepL 
         BackColor       =   &H00000000&
         Caption         =   "Maximum Position:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   0
         Left            =   165
         TabIndex        =   25
         Top             =   360
         Width           =   1410
      End
   End
   Begin VB.Frame Frame4 
      BackColor       =   &H00000000&
      Caption         =   "Temperature Simulation"
      ForeColor       =   &H00FFFFFF&
      Height          =   2655
      Index           =   1
      Left            =   2520
      TabIndex        =   28
      Top             =   4080
      Width           =   2895
      Begin VB.TextBox TempCurTF 
         Height          =   315
         Left            =   1785
         TabIndex        =   15
         ToolTipText     =   "Starting temperature for the sawtooth cycle"
         Top             =   225
         Width           =   930
      End
      Begin VB.TextBox TempStepsTF 
         Height          =   315
         Left            =   1800
         TabIndex        =   19
         ToolTipText     =   "Focuser position change per degree C"
         Top             =   2160
         Width           =   930
      End
      Begin VB.TextBox TempPeriodTF 
         Height          =   315
         Left            =   1800
         TabIndex        =   18
         ToolTipText     =   "Full saw tooth cycle period"
         Top             =   1680
         Width           =   930
      End
      Begin VB.TextBox TempMinTF 
         Height          =   315
         Left            =   1785
         TabIndex        =   17
         ToolTipText     =   "Minimum temperature for the cycle"
         Top             =   1200
         Width           =   930
      End
      Begin VB.TextBox TempMaxTF 
         Height          =   315
         Left            =   1800
         TabIndex        =   16
         ToolTipText     =   "Maximum temperature for the cycle"
         Top             =   705
         Width           =   930
      End
      Begin VB.Label TempCurL 
         BackColor       =   &H00000000&
         Caption         =   "Current Temperature:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   0
         Left            =   120
         TabIndex        =   33
         Top             =   285
         Width           =   1605
      End
      Begin VB.Label TempMaxL 
         BackColor       =   &H00000000&
         Caption         =   "Maximum Temperature:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   1
         Left            =   120
         TabIndex        =   32
         Top             =   765
         Width           =   1650
      End
      Begin VB.Label TempMinL 
         BackColor       =   &H00000000&
         Caption         =   "Minimum Temperature:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   1
         Left            =   120
         TabIndex        =   31
         Top             =   1245
         Width           =   1650
      End
      Begin VB.Label TempPeriodL 
         BackColor       =   &H00000000&
         Caption         =   "Cycle Period (sec):"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   0
         Left            =   120
         TabIndex        =   30
         Top             =   1725
         Width           =   1650
      End
      Begin VB.Label TempStepsL 
         BackColor       =   &H00000000&
         Caption         =   "Steps / °C:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   1
         Left            =   120
         TabIndex        =   29
         Top             =   2205
         Width           =   1530
      End
   End
   Begin VB.Image imgBrewster 
      Height          =   555
      Left            =   2190
      MouseIcon       =   "frmSetup.frx":1016
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":1168
      Top             =   240
      Width           =   1170
   End
   Begin VB.Label lblDriverInfo 
      BackColor       =   &H00000000&
      Caption         =   "<run time - version etc.>"
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   120
      TabIndex        =   22
      Top             =   6840
      Width           =   5295
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
' ASCOM Focuser Simulator setup form
'
' From Scope Simulator written 28-Jun-00   Robert B. Denny <rdenny@dc3.com>
' Structure retained, otherwise rewriten into Focus Simulator
' by Jon Brewster in Feb 2003
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 24-Feb-03 jab     Initial edit - Starting from Telescope Simulator
' 28-Feb-03 jab     completed addition of temperature simulation
' 01-mar-03 jab     reworked button / timer interaction for race conditions
' 16-Mar-03 jab     relative focuser is no longer clipped by 0 and MaxStep,
'                   and therefore a reset command was added, also MaxStep
'                   is disabled for reletive focuser
' -----------------------------------------------------------------------------
Option Explicit

Private m_bResult As Boolean
Private m_bAllowUnload As Boolean
Private m_bAdvancedMode As Boolean

' ======
' EVENTS
' ======

Private Sub Form_Load()
    
    FloatWindow Me.hwnd, True                   ' Setup window always floats
    m_bAllowUnload = True                       ' Start out allowing unload

    lblDriverInfo = App.FileDescription & _
                " Version " & App.Major & "." & _
                App.Minor & "." & App.Revision
                
    setup_dep
    
End Sub

Private Sub Form_Unload(Cancel As Integer)

    Me.Hide                                     ' Assure we don't unload
    Cancel = Not m_bAllowUnload                 ' Unless our flag permits it
    
End Sub

Private Sub chkCanStepSize_Click()

    setup_dep

End Sub

Private Sub chkCanTemp_Click()

    setup_dep

End Sub

Private Sub chkCanTempComp_Click()

    setup_dep

End Sub

Private Sub optType_Click(Index As Integer)
    
    setup_dep

End Sub

Private Sub cmdResetPos_Click()

If optType(1).Value Then
        g_lPosition = 0
    Else
        g_lPosition = MaxStepTF / 2
    End If

End Sub

'
' Make sure GUI elements are in sync with repect to dependencies
'

Private Sub setup_dep()

    If optType(1).Value Then
        MaxStepTF.Enabled = False
    Else
        MaxStepTF.Enabled = True
    End If

    If chkCanStepSize.Value = 0 Then
        StepSizeTF.Enabled = False
    Else
        StepSizeTF.Enabled = True
    End If
    
    If chkCanTemp.Value = 0 Then
        chkCanTempComp.Enabled = False
        TempCurTF.Enabled = False
        TempMaxTF.Enabled = False
        TempMinTF.Enabled = False
        TempPeriodTF.Enabled = False
        TempStepsTF.Enabled = False
    Else
        chkCanTempComp.Enabled = True
        TempCurTF.Enabled = True
        TempMaxTF.Enabled = True
        TempMinTF.Enabled = True
        TempPeriodTF.Enabled = True
        If chkCanTempComp.Value = 0 Then
            TempStepsTF.Enabled = False
        Else
            TempStepsTF.Enabled = True
        End If
    End If

End Sub

Private Sub cmdOK_Click()

    m_bResult = True
    Me.Hide

End Sub

Private Sub cmdCancel_Click()

    m_bResult = False
    Me.Hide

End Sub

Private Sub cmdAdvanced_Click()

    SetFormMode Not m_bAdvancedMode             ' Toggle form mode
    
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

Public Property Get MaxStep() As Long

    MaxStep = CLng(MaxStepTF)
        
End Property

Public Property Let MaxStep(ByVal val As Long)

    MaxStepTF = Format$(val, "0")     ' No decimal digits

End Property

Public Property Get StepSize() As Double

    StepSize = CDbl(StepSizeTF)
        
End Property

Public Property Let StepSize(ByVal val As Double)

    StepSizeTF = Format$(val, "0")     ' No decimal digits

End Property

Public Property Get MaxInc() As Long

    MaxInc = CLng(MaxIncTF)
        
End Property

Public Property Let MaxInc(ByVal val As Long)

    MaxIncTF = Format$(val, "0")     ' No decimal digits

End Property

Public Property Get TempCur() As Double

    TempCur = CDbl(TempCurTF)
        
End Property

Public Property Let TempCur(ByVal val As Double)

    TempCurTF = Format$(val, "0.0")     ' One decimal digit

End Property

Public Property Get TempMax() As Double

    TempMax = CDbl(TempMaxTF)
        
End Property

Public Property Let TempMax(ByVal val As Double)

    TempMaxTF = Format$(val, "0.0")     ' One decimal digit

End Property

Public Property Get TempMin() As Double

    TempMin = CDbl(TempMinTF)
        
End Property

Public Property Let TempMin(ByVal val As Double)

    TempMinTF = Format$(val, "0.0")     ' One decimal digit

End Property

Public Property Get TempPeriod() As Long

    TempPeriod = CLng(TempPeriodTF)
        
End Property

Public Property Let TempPeriod(ByVal val As Long)

    TempPeriodTF = Format$(val, "0")     ' No decimal digits

End Property

Public Property Get TempSteps() As Long

    TempSteps = CLng(TempStepsTF)
        
End Property

Public Property Let TempSteps(ByVal val As Long)

    TempStepsTF = Format$(val, "0")     ' No decimal digits

End Property

Public Property Get Absolute() As Boolean

    If optType(1).Value Then
        Absolute = False
    Else
        Absolute = True
    End If
    
End Property

Public Property Let Absolute(val As Boolean)

    If val Then
        optType(0).Value = True
    Else
        optType(1).Value = True
    End If
    
End Property

Public Property Get Synchronous() As Boolean

    If chkSynchronous.Value = 1 Then
        Synchronous = True
    Else
        Synchronous = False
    End If
    
End Property

Public Property Let Synchronous(val As Boolean)

    If val Then
        chkSynchronous.Value = 1
    Else
        chkSynchronous.Value = 0
    End If
    
End Property

Public Property Let AllowUnload(b As Boolean)

    m_bAllowUnload = b
    
End Property

Public Property Get AdvancedMode() As Boolean

    AdvancedMode = m_bAdvancedMode
    
End Property

Public Property Let AdvancedMode(b As Boolean)

    SetFormMode b
    
End Property

'
' LOCAL UTILITIES
'

Private Sub SetFormMode(Advanced As Boolean)

    If Advanced Then                        ' Basic display
        Me.Height = 7500
        Me.cmdAdvanced.Caption = "<< Basic"
    Else
        Me.Height = 4275
        Me.cmdAdvanced.Caption = "Advanced >>"
    End If
    m_bAdvancedMode = Advanced
    
End Sub
