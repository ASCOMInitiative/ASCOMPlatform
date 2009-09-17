VERSION 5.00
Begin VB.Form frmHandBox 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Scope Simulator"
   ClientHeight    =   4905
   ClientLeft      =   45
   ClientTop       =   255
   ClientWidth     =   1980
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   4905
   ScaleWidth      =   1980
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdFlip 
      Caption         =   "&Flip"
      Height          =   330
      Left            =   1365
      TabIndex        =   5
      ToolTipText     =   "Flip the scope to the other side of the pier"
      Top             =   3180
      Width           =   510
   End
   Begin VB.CommandButton cmdHome 
      Caption         =   "&Home"
      Height          =   330
      Left            =   1050
      TabIndex        =   10
      ToolTipText     =   "Find scope's home position by slewing"
      Top             =   4050
      Width           =   825
   End
   Begin VB.CommandButton cmdPark 
      Caption         =   "Un&park"
      Height          =   330
      Left            =   120
      TabIndex        =   7
      ToolTipText     =   "Slew scope to it's park position, or unpark"
      Top             =   4050
      Width           =   825
   End
   Begin VB.CheckBox chkTracking 
      BackColor       =   &H00000000&
      Caption         =   "&Track"
      ForeColor       =   &H00FFFFFF&
      Height          =   195
      Left            =   135
      TabIndex        =   6
      ToolTipText     =   "Turn on/off scope tracking"
      Top             =   3645
      Width           =   765
   End
   Begin VB.CommandButton cmdTraffic 
      Caption         =   "Tr&affic"
      Height          =   330
      Left            =   1050
      TabIndex        =   9
      ToolTipText     =   "Bring up the ASCOM commands traffic window"
      Top             =   4470
      Width           =   825
   End
   Begin VB.CommandButton cmdSetup 
      Caption         =   "&Setup"
      Height          =   330
      Left            =   120
      TabIndex        =   8
      ToolTipText     =   "Bring up the setup dialog"
      Top             =   4470
      Width           =   825
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "Ä"
      BeginProperty Font 
         Name            =   "Wingdings 2"
         Size            =   18
         Charset         =   2
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   0
      Left            =   780
      TabIndex        =   2
      ToolTipText     =   "Abort any slew"
      Top             =   2685
      Width           =   420
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "S"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   2
      Left            =   780
      TabIndex        =   4
      ToolTipText     =   "Shift-click for slow speed Ctrl-click for very slow"
      Top             =   3120
      Width           =   420
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "W"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   4
      Left            =   285
      TabIndex        =   1
      ToolTipText     =   "Shift-click for slow speed Ctrl-click for very slow"
      Top             =   2685
      Width           =   420
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "E"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   3
      Left            =   1260
      TabIndex        =   3
      ToolTipText     =   "Shift-click for slow speed Ctrl-click for very slow"
      Top             =   2670
      Width           =   420
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "N"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   390
      Index           =   1
      Left            =   780
      TabIndex        =   0
      ToolTipText     =   "Shift-click for slow speed Ctrl-click for very slow"
      Top             =   2250
      Width           =   420
   End
   Begin VB.Shape shpEast 
      BorderColor     =   &H00E0E0E0&
      BorderStyle     =   0  'Transparent
      FillColor       =   &H0000FF00&
      FillStyle       =   0  'Solid
      Height          =   135
      Left            =   1530
      Shape           =   3  'Circle
      Top             =   2520
      Width           =   180
   End
   Begin VB.Shape shpWest 
      BorderColor     =   &H00E0E0E0&
      BorderStyle     =   0  'Transparent
      FillColor       =   &H0000FF00&
      FillStyle       =   0  'Solid
      Height          =   135
      Left            =   270
      Shape           =   3  'Circle
      Top             =   2520
      Width           =   135
   End
   Begin VB.Shape shpHome 
      BorderColor     =   &H00E0E0E0&
      BorderStyle     =   0  'Transparent
      FillColor       =   &H0000FF00&
      FillStyle       =   0  'Solid
      Height          =   135
      Left            =   1735
      Shape           =   3  'Circle
      Top             =   3885
      Width           =   135
   End
   Begin VB.Shape shpPark 
      BorderColor     =   &H00E0E0E0&
      BorderStyle     =   0  'Transparent
      FillColor       =   &H0000FF00&
      FillStyle       =   0  'Solid
      Height          =   135
      Left            =   810
      Shape           =   3  'Circle
      Top             =   3885
      Width           =   135
   End
   Begin VB.Image imgSpaceSoftware 
      Height          =   735
      Left            =   0
      MouseIcon       =   "frmHandBox.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "frmHandBox.frx":0152
      ToolTipText     =   "Click to go to the SPACE.com web site"
      Top             =   -45
      Width           =   2070
   End
   Begin VB.Label lblAltitude 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H000000FF&
      Height          =   255
      Left            =   555
      TabIndex        =   20
      Top             =   1890
      Width           =   1275
   End
   Begin VB.Label Label7 
      BackColor       =   &H00000000&
      Caption         =   "Alt"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   165
      TabIndex        =   19
      Top             =   1905
      Width           =   345
   End
   Begin VB.Label lblAzimuth 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H000000FF&
      Height          =   255
      Left            =   555
      TabIndex        =   18
      Top             =   1590
      Width           =   1275
   End
   Begin VB.Label Label3 
      BackColor       =   &H00000000&
      Caption         =   "Az"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   165
      TabIndex        =   17
      Top             =   1605
      Width           =   345
   End
   Begin VB.Label lblLST 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H000000FF&
      Height          =   255
      Left            =   555
      TabIndex        =   16
      Top             =   690
      Width           =   1275
   End
   Begin VB.Label Label1 
      BackColor       =   &H00000000&
      Caption         =   "LST"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   165
      TabIndex        =   15
      Top             =   705
      Width           =   345
   End
   Begin VB.Label Label4 
      BackColor       =   &H00000000&
      Caption         =   "Dec"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   165
      TabIndex        =   14
      Top             =   1305
      Width           =   345
   End
   Begin VB.Label lblDec 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H000000FF&
      Height          =   255
      Left            =   555
      TabIndex        =   13
      Top             =   1290
      Width           =   1275
   End
   Begin VB.Label Label2 
      BackColor       =   &H00000000&
      Caption         =   "RA"
      ForeColor       =   &H00FFFFFF&
      Height          =   225
      Left            =   165
      TabIndex        =   12
      Top             =   1005
      Width           =   345
   End
   Begin VB.Label lblRA 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
      BeginProperty Font 
         Name            =   "Courier"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H000000FF&
      Height          =   255
      Left            =   555
      TabIndex        =   11
      Top             =   990
      Width           =   1275
   End
End
Attribute VB_Name = "frmHandBox"
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
'   ==============
'   FRMHANDBOX.FRM
'   ==============
'
' ASCOM Scope Simulator hand box form
'
' Written:  28-Jun-00   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 28-Jun-00 rbd     Initial edit
' 28-Jan-01 rbd     ASCOM and SPACE logos, clickable.
' 05-Feb-01 rbd     FloatWindow now takes boolean to float/unfloat. Add
'                   "always on top" control. Fix setup dialog runtime error.
' 11-Mar-01 rbd     Add mouse buttons to ButtonState property (bits 6-8)
' 27-Jul-02 rbd     ASCOM logo and hot link
' 16-Jun-03 jab     ASCOM Journaling, also required that FmtSexa() be moved
'                   out to Startup.bas and made public
' 10-Sep-03 jab     label the traffic window
' 05-Mar-04 jab     added more controls (pier side control, home, park, track
'                   halt).  Also fixed key debounce problem.
' -----------------------------------------------------------------------------
Option Explicit

Private BtnState As Integer
Private Debounce As Integer
Private Southern As Boolean
Private Mode As AlignmentModes

' ======
' EVENTS
' ======

Private Sub Form_Load()

    FloatWindow Me.hWnd, g_bAlwaysTop
    Southern = False
    Mode = algPolar
    LabelButtons
    BtnState = 0
    Debounce = 0
    
End Sub

Private Sub Form_Unload(Cancel As Integer)

    On Error Resume Next
    Unload frmSetup
    If Not g_show Is Nothing Then _
        Unload g_show
    DoShutdown              ' Saves Alt/Az & window pos for next start
    
End Sub

Private Sub cmdHome_Click()
    
    If g_bAtPark Then
        MsgBox "Can not home while parked.", vbExclamation
        Exit Sub
    End If
    
    On Error GoTo HomeError
    doHome
    On Error GoTo 0
    
    Exit Sub

HomeError:

    MsgBox "Home Error.", vbExclamation
    Resume Next
        
End Sub

Private Sub chkTracking_Click()
    
    Dim newVal As Boolean
    
    If g_bAtPark Then
        MsgBox "Can not change tracking state while parked.", vbExclamation
        Exit Sub
    End If
    
    newVal = IIf(chkTracking.Value = 1, True, False)
    
    If newVal Then
        ChangeHome False
        ChangePark False
    End If
    
    g_bTracking = newVal
    
End Sub

Private Sub cmdPark_Click()
    
    If g_bAtPark Then
        doUnpark
    Else
        On Error GoTo ParkError
        doPark
        On Error GoTo 0
    End If
    
    Exit Sub

ParkError:

    MsgBox "Park Error.", vbExclamation
    Resume Next
        
End Sub

Private Sub cmdFlip_Click()
    
    If g_bAtPark Then
        MsgBox "Can not flip while parked.", vbExclamation
        Exit Sub
    End If
    
    If g_eAlignMode = algGermanPolar Then
        On Error GoTo FlipError
        
            If g_SOP = pierEast Then
                doFlip pierWest
            ElseIf g_SOP = pierWest Then
                doFlip pierEast
            End If
            
        On Error GoTo 0
    End If
    
    Exit Sub

FlipError:

    MsgBox "Flip Error.", vbExclamation
    Resume Next
        
End Sub

Private Sub cmdSetup_Click()

    DoSetup                         ' May change our topmost state
    
End Sub

Private Sub cmdSlew_MouseDown(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)

    Debounce = Index + (Shift * 8) + (Button * 64)
    BtnState = Debounce

End Sub

Private Sub cmdSlew_MouseUp(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)

    BtnState = 0

End Sub

Private Sub cmdTraffic_Click()

    If g_show Is Nothing Then _
        Set g_show = New frmShow
    
    g_show.Caption = "Scope Simulator ASCOM Traffic"
    g_show.Show
    
End Sub

Private Sub imgSpaceSoftware_Click()

    DisplayWebPage "http://www.starrynight.com/"
    
End Sub

Private Sub picASCOM_Click()

    DisplayWebPage "http://ascom-standards.org/"
    
End Sub

' =================
' PUBLIC PROPERTIES
' =================

Public Sub ParkCaption()

    cmdPark.Caption = IIf(g_bAtPark, "Un&park", "&Park")
    chkTracking.Enabled = Not g_bAtPark
    
End Sub

Public Sub Tracking()

    chkTracking.Value = IIf(g_bTracking, 1, 0)
    
End Sub

Public Property Let RightAscension(RA As Double)

    lblRA.Caption = FmtSexa(RA, False)
    
End Property

Public Property Let Declination(Dec As Double)

    lblDec.Caption = FmtSexa(Dec, True)
    
End Property

Public Property Let SiderealTime(ST As Double)

    lblLST.Caption = FmtSexa(ST, False)
    
End Property

Public Property Let Azimuth(Az As Double)

    lblAzimuth.Caption = FmtSexa(Az, False)
    
End Property

Public Property Let Altitude(Alt As Double)

    lblAltitude.Caption = FmtSexa(Alt, False)
    
End Property

Public Property Let SouthernHemisphere(S As Boolean)

    Southern = S
    LabelButtons
    
End Property

Public Property Let AlignMode(m As AlignmentModes)

    Mode = m
    LabelButtons
    
End Property

Public Property Get ButtonState() As Integer

    ButtonState = Debounce
    Debounce = BtnState
    
End Property

Public Sub LEDPier(newVal As PierSide)

    If newVal = pierEast Then
        shpEast.FillColor = &HFF00&
    Else
        shpEast.FillColor = &H0&
    End If
    
    If newVal = pierWest Then
        shpWest.FillColor = &HFF00&
    Else
        shpWest.FillColor = &H0&
    End If
    
    If newVal = pierUnknown Then
        cmdFlip.Visible = False
    Else
        cmdFlip.Visible = True
    End If
    
End Sub

Public Sub LEDHome(state As Boolean)

    If state Then
        shpHome.FillColor = &HFF00&
    Else
        shpHome.FillColor = &H0&
    End If
    
End Sub

Public Sub LEDPark(state As Boolean)

    If state Then
        shpPark.FillColor = &HFF00&
    Else
        shpPark.FillColor = &H0&
    End If
    
End Sub

' ==============
' LOCAL ROUTINES
' ==============

' When parked, disable these buttons ???

Private Function LabelButtons()

    If Mode = algAltAz Then
        cmdSlew(1).Caption = "U"
        cmdSlew(2).Caption = "D"
        cmdSlew(3).Caption = "R"
        cmdSlew(4).Caption = "L"
    ElseIf Southern Then
        cmdSlew(1).Caption = "S"
        cmdSlew(2).Caption = "N"
        cmdSlew(3).Caption = "E"
        cmdSlew(4).Caption = "W"
    Else
        cmdSlew(1).Caption = "N"
        cmdSlew(2).Caption = "S"
        cmdSlew(3).Caption = "E"
        cmdSlew(4).Caption = "W"
    End If
    
End Function
