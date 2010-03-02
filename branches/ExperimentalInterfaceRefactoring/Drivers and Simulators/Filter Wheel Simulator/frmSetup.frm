VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "comdlg32.ocx"
Begin VB.Form frmSetup 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "ASCOM Filter Wheel Simulator Setup"
   ClientHeight    =   7800
   ClientLeft      =   45
   ClientTop       =   285
   ClientWidth     =   5190
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   7766.379
   ScaleMode       =   0  'User
   ScaleWidth      =   5190
   StartUpPosition =   2  'CenterScreen
   Begin MSComDlg.CommonDialog CommonDialog1 
      Left            =   3000
      Top             =   6240
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.CheckBox chkImplementsOffsets 
      BackColor       =   &H00000000&
      Caption         =   "Implements focus offsets"
      ForeColor       =   &H00FFFFFF&
      Height          =   255
      Left            =   360
      TabIndex        =   39
      ToolTipText     =   "Use focus offsets defined above"
      Top             =   6600
      Value           =   1  'Checked
      Width           =   2415
   End
   Begin VB.CheckBox chkImplementsNames 
      BackColor       =   &H00000000&
      Caption         =   "Implements filter names"
      ForeColor       =   &H00FFFFFF&
      Height          =   255
      Left            =   360
      TabIndex        =   38
      ToolTipText     =   "Use filter names defined above"
      Top             =   6240
      Value           =   1  'Checked
      Width           =   2415
   End
   Begin VB.PictureBox picASCOM 
      AutoSize        =   -1  'True
      BorderStyle     =   0  'None
      Height          =   840
      Left            =   4080
      MouseIcon       =   "frmSetup.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "frmSetup.frx":0152
      ScaleHeight     =   840
      ScaleWidth      =   720
      TabIndex        =   43
      TabStop         =   0   'False
      Top             =   480
      Width           =   720
   End
   Begin VB.CheckBox chkAlwaysTop 
      BackColor       =   &H00000000&
      Caption         =   "Always on Top"
      ForeColor       =   &H00FFFFFF&
      Height          =   255
      Left            =   360
      TabIndex        =   40
      ToolTipText     =   "Filter wheel handset will float on top"
      Top             =   6960
      Value           =   1  'Checked
      Width           =   1575
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "&Cancel"
      Height          =   345
      Left            =   3030
      TabIndex        =   0
      Top             =   6990
      Width           =   930
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "&OK"
      Height          =   345
      Left            =   4110
      TabIndex        =   1
      Top             =   6990
      Width           =   930
   End
   Begin VB.Frame Frame1 
      BackColor       =   &H00000000&
      Caption         =   "Filter Wheel Settings"
      ForeColor       =   &H00FFFFFF&
      Height          =   1260
      Left            =   120
      TabIndex        =   42
      Top             =   360
      Width           =   3510
      Begin VB.ComboBox cmbTime 
         Height          =   315
         ItemData        =   "frmSetup.frx":1016
         Left            =   2280
         List            =   "frmSetup.frx":1032
         TabIndex        =   5
         Text            =   "2.0"
         ToolTipText     =   "Time is cumulative across multiple slots"
         Top             =   840
         Width           =   735
      End
      Begin VB.ComboBox cmbSlots 
         Height          =   315
         ItemData        =   "frmSetup.frx":105E
         Left            =   2280
         List            =   "frmSetup.frx":107A
         TabIndex        =   3
         Text            =   "4"
         Top             =   360
         Width           =   735
      End
      Begin VB.Label SlotsL 
         BackColor       =   &H00000000&
         Caption         =   "Time between slots (secs):"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   1
         Left            =   120
         TabIndex        =   4
         Top             =   840
         Width           =   1965
      End
      Begin VB.Label SlotsL 
         BackColor       =   &H00000000&
         Caption         =   "Number of Filter Slots:"
         ForeColor       =   &H00FFFFFF&
         Height          =   225
         Index           =   0
         Left            =   165
         TabIndex        =   2
         Top             =   360
         Width           =   1770
      End
   End
   Begin VB.Frame Frame2 
      BackColor       =   &H00000000&
      Caption         =   "Filter Setup"
      ForeColor       =   &H00FFFFFF&
      Height          =   4215
      Index           =   1
      Left            =   120
      TabIndex        =   44
      Top             =   1800
      Width           =   4935
      Begin VB.PictureBox picFilterColour 
         Height          =   285
         Index           =   1
         Left            =   4360
         ScaleHeight     =   225
         ScaleWidth      =   225
         TabIndex        =   13
         ToolTipText     =   "Click to change displayed filter colour"
         Top             =   1245
         Width           =   285
      End
      Begin VB.PictureBox picFilterColour 
         Height          =   285
         Index           =   7
         Left            =   4360
         ScaleHeight     =   225
         ScaleWidth      =   225
         TabIndex        =   37
         ToolTipText     =   "Click to change displayed filter colour"
         Top             =   3675
         Width           =   285
      End
      Begin VB.PictureBox picFilterColour 
         Height          =   285
         Index           =   6
         Left            =   4360
         ScaleHeight     =   225
         ScaleWidth      =   225
         TabIndex        =   33
         ToolTipText     =   "Click to change displayed filter colour"
         Top             =   3270
         Width           =   285
      End
      Begin VB.PictureBox picFilterColour 
         Height          =   285
         Index           =   5
         Left            =   4360
         ScaleHeight     =   225
         ScaleWidth      =   225
         TabIndex        =   29
         ToolTipText     =   "Click to change displayed filter colour"
         Top             =   2865
         Width           =   285
      End
      Begin VB.PictureBox picFilterColour 
         Height          =   285
         Index           =   4
         Left            =   4360
         ScaleHeight     =   225
         ScaleWidth      =   225
         TabIndex        =   26
         ToolTipText     =   "Click to change displayed filter colour"
         Top             =   2460
         Width           =   285
      End
      Begin VB.PictureBox picFilterColour 
         Height          =   285
         Index           =   3
         Left            =   4360
         ScaleHeight     =   225
         ScaleWidth      =   225
         TabIndex        =   21
         ToolTipText     =   "Click to change displayed filter colour"
         Top             =   2055
         Width           =   285
      End
      Begin VB.PictureBox picFilterColour 
         Height          =   285
         Index           =   2
         Left            =   4360
         ScaleHeight     =   225
         ScaleWidth      =   225
         TabIndex        =   17
         ToolTipText     =   "Click to change displayed filter colour"
         Top             =   1650
         Width           =   285
      End
      Begin VB.PictureBox picFilterColour 
         Height          =   285
         Index           =   0
         Left            =   4360
         ScaleHeight     =   225
         ScaleWidth      =   225
         TabIndex        =   9
         ToolTipText     =   "Click to change displayed filter colour"
         Top             =   840
         Width           =   285
      End
      Begin VB.TextBox OffsetTL 
         Height          =   285
         Index           =   7
         Left            =   2640
         TabIndex        =   36
         Top             =   3675
         Width           =   1575
      End
      Begin VB.TextBox OffsetTL 
         Height          =   285
         Index           =   6
         Left            =   2640
         TabIndex        =   32
         Top             =   3270
         Width           =   1575
      End
      Begin VB.TextBox OffsetTL 
         Height          =   285
         Index           =   5
         Left            =   2640
         TabIndex        =   28
         Top             =   2865
         Width           =   1575
      End
      Begin VB.TextBox OffsetTL 
         Height          =   285
         Index           =   4
         Left            =   2640
         TabIndex        =   24
         Top             =   2460
         Width           =   1575
      End
      Begin VB.TextBox OffsetTL 
         Height          =   285
         Index           =   3
         Left            =   2640
         TabIndex        =   20
         Top             =   2055
         Width           =   1575
      End
      Begin VB.TextBox OffsetTL 
         Height          =   285
         Index           =   2
         Left            =   2640
         TabIndex        =   16
         Top             =   1650
         Width           =   1575
      End
      Begin VB.TextBox OffsetTL 
         Height          =   285
         Index           =   1
         Left            =   2640
         TabIndex        =   12
         Top             =   1245
         Width           =   1575
      End
      Begin VB.TextBox OffsetTL 
         Height          =   285
         Index           =   0
         Left            =   2640
         TabIndex        =   8
         Top             =   840
         Width           =   1575
      End
      Begin VB.TextBox SlotNameTS 
         Height          =   285
         Index           =   7
         Left            =   840
         TabIndex        =   35
         Top             =   3660
         Width           =   1575
      End
      Begin VB.TextBox SlotNameTS 
         Height          =   285
         Index           =   6
         Left            =   840
         TabIndex        =   31
         Top             =   3255
         Width           =   1575
      End
      Begin VB.TextBox SlotNameTS 
         Height          =   285
         Index           =   5
         Left            =   840
         TabIndex        =   27
         Top             =   2850
         Width           =   1575
      End
      Begin VB.TextBox SlotNameTS 
         Height          =   285
         Index           =   4
         Left            =   840
         TabIndex        =   23
         Top             =   2445
         Width           =   1575
      End
      Begin VB.TextBox SlotNameTS 
         Height          =   285
         Index           =   3
         Left            =   840
         TabIndex        =   19
         Top             =   2040
         Width           =   1575
      End
      Begin VB.TextBox SlotNameTS 
         Height          =   285
         Index           =   2
         Left            =   840
         TabIndex        =   15
         Top             =   1635
         Width           =   1575
      End
      Begin VB.TextBox SlotNameTS 
         Height          =   285
         Index           =   1
         Left            =   840
         TabIndex        =   11
         Top             =   1230
         Width           =   1575
      End
      Begin VB.TextBox SlotNameTS 
         ForeColor       =   &H00000000&
         Height          =   285
         Index           =   0
         Left            =   840
         TabIndex        =   7
         Top             =   825
         Width           =   1575
      End
      Begin VB.Label Label2 
         BackColor       =   &H00000000&
         Caption         =   "Colour"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   4320
         TabIndex        =   47
         Top             =   480
         Width           =   495
      End
      Begin VB.Label FocusOffsetL 
         Alignment       =   2  'Center
         BackColor       =   &H00000000&
         Caption         =   "Focus Offset"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   2760
         TabIndex        =   46
         Top             =   480
         Width           =   1455
      End
      Begin VB.Label FilterNameL 
         Alignment       =   2  'Center
         BackColor       =   &H00000000&
         Caption         =   "Filter Name"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   1200
         TabIndex        =   45
         Top             =   480
         Width           =   975
      End
      Begin VB.Label Label1 
         BackColor       =   &H00000000&
         Caption         =   "Slot 7:"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Left            =   240
         TabIndex        =   34
         Top             =   3705
         Width           =   855
      End
      Begin VB.Label Slot6L 
         BackColor       =   &H00000000&
         Caption         =   "Slot 6:"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Left            =   240
         TabIndex        =   30
         Top             =   3300
         Width           =   855
      End
      Begin VB.Label Slot5L 
         BackColor       =   &H00000000&
         Caption         =   "Slot 5:"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Left            =   240
         TabIndex        =   25
         Top             =   2895
         Width           =   855
      End
      Begin VB.Label Slot0L 
         BackColor       =   &H00000000&
         Caption         =   "Slot 0:"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Index           =   0
         Left            =   240
         TabIndex        =   6
         Top             =   870
         Width           =   855
      End
      Begin VB.Label Slot1L 
         BackColor       =   &H00000000&
         Caption         =   "Slot 1:"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Index           =   1
         Left            =   240
         TabIndex        =   10
         Top             =   1275
         Width           =   855
      End
      Begin VB.Label Slot2L 
         BackColor       =   &H00000000&
         Caption         =   "Slot 2:"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Index           =   1
         Left            =   240
         TabIndex        =   14
         Top             =   1680
         Width           =   855
      End
      Begin VB.Label Slot3L 
         BackColor       =   &H00000000&
         Caption         =   "Slot 3:"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Index           =   0
         Left            =   240
         TabIndex        =   18
         Top             =   2085
         Width           =   855
      End
      Begin VB.Label Slot4L 
         BackColor       =   &H00000000&
         Caption         =   "Slot 4:"
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Index           =   1
         Left            =   240
         TabIndex        =   22
         Top             =   2490
         Width           =   855
      End
   End
   Begin VB.Label lblDriverInfo 
      BackColor       =   &H00000000&
      Caption         =   "<run time - version etc.>"
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   120
      TabIndex        =   41
      Top             =   7440
      Width           =   4815
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
' ASCOM Filter Wheel Simulator setup form
'
' From Focus Simulator, from Scope Simulator written 28-Jun-00
' Robert B. Denny <rdenny@dc3.com>
' Structure retained, otherwise rewriten into Filter Wheel Simulator
' by Mark Crossley in Nov 2008
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 18-Nov-08 mpc     Initial edit - Starting from Focuser Simulator
' -----------------------------------------------------------------------------
Option Explicit

Private m_bResult As Boolean
Private m_bAllowUnload As Boolean


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

Private Sub cmbSlots_KeyPress(KeyAscii As Integer)
    If KeyAscii < 48 Or KeyAscii > 57 Then
        If Not KeyAscii = 8 Then KeyAscii = 0
    End If
End Sub

Private Sub cmbSlots_Validate(Cancel As Boolean)
    Dim i
    i = CInt("0" & cmbSlots.Text)    ' Make blanks = 0
    If i < 1 Or i > 8 Then
        Cancel = True
        MsgBox "Range of slot values is 1-8", vbExclamation + vbOKOnly, "Input Error"
    Else
        g_iSlots = i
        setup_dep
    End If
End Sub

Private Sub cmbTime_KeyPress(KeyAscii As Integer)
    ' Only allow one occurrence of the period
    If KeyAscii = 46 And InStr(cmbTime.Text, ".") > 0 Then
        KeyAscii = 0
    ' Now carry out numerals and period check
    ElseIf KeyAscii < 46 Or KeyAscii > 57 Or KeyAscii = 47 Then
        If Not KeyAscii = 8 Then KeyAscii = 0
    End If
End Sub

Private Sub cmbTime_Validate(Cancel As Boolean)
    Dim i
    i = CDbl("0" & cmbTime.Text)        ' Make blanks = 0
    If i < 0.1 Or i > 8 Then
        MsgBox "Range time values is 0.1-8.0", vbExclamation + vbOKOnly, "Input Error"
        Cancel = True
    End If
End Sub

Private Sub OffsetTL_KeyPress(Index As Integer, KeyAscii As Integer)
    If KeyAscii < 48 Or KeyAscii > 57 Then
        If Not KeyAscii = 8 Then KeyAscii = 0
    End If
End Sub

Private Sub OffsetTL_Validate(Index As Integer, Cancel As Boolean)
    If OffsetTL(Index).Text = "" Then OffsetTL(Index).Text = "0"
End Sub

Private Sub chkImplementsNames_Click()
    g_bImplementsNames = (chkImplementsNames = 1)
    setup_dep
End Sub

Private Sub chkImplementsOffsets_Click()
    g_bImplementsOffsets = (chkImplementsOffsets = 1)
    setup_dep
End Sub

'
' Make sure GUI elements are in sync
'

Private Sub setup_dep()

    Dim i As Integer

    For i = 0 To 7
        If g_iSlots > i Then
            ControlEnable SlotNameTS(i), g_bImplementsNames
            ControlEnable OffsetTL(i), g_bImplementsOffsets
            ControlEnable picFilterColour(i), True
        Else
            ControlEnable SlotNameTS(i), False
            ControlEnable OffsetTL(i), False
            ControlEnable picFilterColour(i), False
        End If
    Next i
    
    Me.Refresh
    
End Sub

'
' Enable/disable controls
'
Private Sub ControlEnable(control As control, enabled As Boolean)

    If enabled Then
        control.enabled = True
        control.ForeColor = &H0
    Else
        control.enabled = False
        control.ForeColor = &HC0C0C0
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


Private Sub picASCOM_Click()

    DisplayWebPage "http://ascom-standards.org/"
    
End Sub

' =================
' PUBLIC PROPERTIES
' =================

Public Property Get Result() As Boolean

    Result = m_bResult              ' Set by OK or Cancel button
    
End Property

Public Property Let AllowUnload(ByVal val As Boolean)

    m_bAllowUnload = val

End Property


Public Property Get Slots() As Integer

    Slots = CInt(cmbSlots.Text)
        
End Property

Public Property Let Slots(ByVal val As Integer)

    cmbSlots.Text = CStr(val)

End Property

Public Property Get Time() As Integer
    ' pass the time back in millisecs
    Time = CInt(cmbTime.Text * 1000)
End Property

Public Property Let Time(ByVal val As Integer)
     'we store the time in millisecs, convert to seconds for display
    cmbTime.Text = Format$(val / 1000, "0.0")
End Property

Public Property Get Names() As String()

    Dim i As Integer
    Dim temp() As String
    ReDim temp(7)
    
    For i = 0 To 7
        temp(i) = SlotNameTS(i).Text
    Next i
        
    Names = temp
    
End Property

Public Property Let Names(ByRef val() As String)

    Dim i As Integer

    For i = 0 To 7
        SlotNameTS(i).Text = val(i)
    Next i

End Property

Public Property Get Offsets() As Long()

    Dim i As Integer
    Dim temp() As Long
    ReDim temp(7)
    
    For i = 0 To 7
       temp(i) = CLng(OffsetTL(i).Text)
    Next i
    
    Offsets = temp
        
End Property

Public Property Let Offsets(ByRef val() As Long)

    Dim i As Integer
    
    For i = 0 To 7
        OffsetTL(i).Text = Format$(val(i), "0")     ' No decimal digits
    Next i

End Property

Public Property Get Colours() As ColorConstants()
    
    Dim i As Integer
    Dim temp() As ColorConstants
    ReDim temp(7)
    
    For i = 0 To 7
        temp(i) = picFilterColour(i).BackColor
    Next i
    
    Colours = temp
    
End Property

Public Property Let Colours(ByRef val() As ColorConstants)

    Dim i As Integer
    
    For i = 0 To 7
        picFilterColour(i).BackColor = val(i)
    Next
    
End Property


Private Sub picFilterColour_Click(Index As Integer)
    
' if you cancel the dialog it generates error 32755
' we'll treat any error as a 'cancel'
On Local Error GoTo GetColorError
    With CommonDialog1
        .CancelError = True
        .ShowColor  'Display Color Selection Dialog
        picFilterColour(Index).BackColor = .Color
    End With
        
    Exit Sub
    
GetColorError:

End Sub
