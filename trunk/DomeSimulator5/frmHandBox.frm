VERSION 5.00
Begin VB.Form frmHandBox 
   BackColor       =   &H00000000&
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Dome Simulator"
   ClientHeight    =   6165
   ClientLeft      =   45
   ClientTop       =   255
   ClientWidth     =   2025
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   ScaleHeight     =   6165
   ScaleWidth      =   2025
   StartUpPosition =   3  'Windows Default
   Begin VB.PictureBox Picture1 
      BackColor       =   &H00000040&
      BorderStyle     =   0  'None
      Height          =   285
      Left            =   165
      ScaleHeight     =   285
      ScaleWidth      =   1680
      TabIndex        =   20
      Top             =   1650
      Width           =   1680
      Begin VB.Label lblSLEW 
         BackStyle       =   0  'Transparent
         Caption         =   "SLEW"
         BeginProperty Font 
            Name            =   "Small Fonts"
            Size            =   6.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00404080&
         Height          =   165
         Left            =   75
         TabIndex        =   23
         ToolTipText     =   "Dome.Slewing is True"
         Top             =   60
         Width           =   450
      End
      Begin VB.Label lblHOME 
         BackStyle       =   0  'Transparent
         Caption         =   "HOME"
         BeginProperty Font 
            Name            =   "Small Fonts"
            Size            =   6.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00404080&
         Height          =   150
         Left            =   585
         TabIndex        =   22
         ToolTipText     =   "Dome.AtHome is True"
         Top             =   60
         Width           =   465
      End
      Begin VB.Label lblPARK 
         BackStyle       =   0  'Transparent
         Caption         =   "PARK"
         BeginProperty Font 
            Name            =   "Small Fonts"
            Size            =   6.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00404080&
         Height          =   150
         Left            =   1185
         TabIndex        =   21
         ToolTipText     =   "Dome.AtPark is True"
         Top             =   60
         Width           =   450
      End
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "Close"
      Height          =   255
      Index           =   8
      Left            =   1275
      TabIndex        =   5
      Top             =   3030
      Width           =   540
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "Open"
      Height          =   255
      Index           =   7
      Left            =   165
      TabIndex        =   3
      Top             =   3030
      Width           =   540
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "£"
      BeginProperty Font 
         Name            =   "Wingdings 3"
         Size            =   14.25
         Charset         =   2
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   510
      Index           =   5
      Left            =   780
      TabIndex        =   4
      Top             =   3030
      Width           =   420
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "¤"
      BeginProperty Font 
         Name            =   "Wingdings 3"
         Size            =   14.25
         Charset         =   2
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   510
      Index           =   6
      Left            =   780
      TabIndex        =   10
      Top             =   4350
      Width           =   420
   End
   Begin VB.CommandButton cmdHome 
      Caption         =   "Home"
      Height          =   450
      Left            =   1080
      TabIndex        =   13
      Top             =   5070
      Width           =   765
   End
   Begin VB.CommandButton cmdTraffic 
      Caption         =   "Traffic"
      Height          =   330
      Left            =   1080
      TabIndex        =   15
      Top             =   5670
      Width           =   765
   End
   Begin VB.CommandButton cmdGoto 
      Caption         =   "Sync:"
      Height          =   330
      Index           =   1
      Left            =   180
      TabIndex        =   1
      Top             =   2430
      Width           =   705
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "step"
      Height          =   255
      Index           =   2
      Left            =   180
      TabIndex        =   9
      Top             =   4110
      Width           =   540
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "step"
      Height          =   255
      Index           =   4
      Left            =   1275
      TabIndex        =   11
      Top             =   4110
      Width           =   540
   End
   Begin VB.CommandButton cmdGoto 
      Caption         =   "Goto:"
      Height          =   330
      Index           =   0
      Left            =   180
      TabIndex        =   0
      Top             =   2070
      Width           =   705
   End
   Begin VB.TextBox txtNewAz 
      Height          =   315
      Left            =   1020
      TabIndex        =   2
      Top             =   2250
      Width           =   810
   End
   Begin VB.Timer timer 
      Enabled         =   0   'False
      Interval        =   250
      Left            =   0
      Top             =   0
   End
   Begin VB.CommandButton cmdSetup 
      Caption         =   "Setup"
      Height          =   330
      Left            =   180
      TabIndex        =   14
      Top             =   5670
      Width           =   765
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
      Height          =   630
      Index           =   10
      Left            =   780
      TabIndex        =   7
      Top             =   3630
      Width           =   420
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "P"
      BeginProperty Font 
         Name            =   "Wingdings 3"
         Size            =   14.25
         Charset         =   2
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   510
      Index           =   1
      Left            =   180
      TabIndex        =   6
      Top             =   3555
      Width           =   540
   End
   Begin VB.CommandButton cmdSlew 
      Caption         =   "Q"
      BeginProperty Font 
         Name            =   "Wingdings 3"
         Size            =   14.25
         Charset         =   2
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   510
      Index           =   3
      Left            =   1275
      TabIndex        =   8
      Top             =   3540
      Width           =   540
   End
   Begin VB.CommandButton cmdPark 
      Caption         =   "Park"
      Height          =   450
      Left            =   180
      TabIndex        =   12
      Top             =   5070
      Width           =   765
   End
   Begin VB.Label txtShutter 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
      Caption         =   "----"
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
      Left            =   780
      TabIndex        =   19
      Top             =   840
      Width           =   1035
   End
   Begin VB.Label lblShutter 
      BackColor       =   &H00000000&
      Caption         =   "Shutter:"
      ForeColor       =   &H00FFFFFF&
      Height          =   195
      Left            =   180
      TabIndex        =   18
      Top             =   840
      Width           =   570
   End
   Begin VB.Image imgBrewster 
      Height          =   555
      Left            =   405
      MouseIcon       =   "frmHandBox.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "frmHandBox.frx":0152
      ToolTipText     =   "Click to go to astro.brewsters.net "
      Top             =   120
      Width           =   1170
   End
   Begin VB.Label txtDomeAz 
      Alignment       =   1  'Right Justify
      Appearance      =   0  'Flat
      BackColor       =   &H00000000&
      Caption         =   "---.-"
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
      Left            =   900
      TabIndex        =   17
      Top             =   1245
      Width           =   915
   End
   Begin VB.Label lblDomeAz 
      BackColor       =   &H00000000&
      Caption         =   "Dome Az:"
      ForeColor       =   &H00FFFFFF&
      Height          =   465
      Left            =   165
      TabIndex        =   16
      Top             =   1140
      Width           =   570
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
' ASCOM Dome Simulator hand box form
'
' Written:  20-Jun-03   Jon Brewster
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 20-Jun-03 jab     Initial edit
' 27-Jun-03 jab     Initial release
' 10-Sep-03 jab     label the traffic window
' 06-Dec-04 rbd     4.0.2 - New annunciator for SLEW, park and home now
'                   textual annunciator instead of LED.
' 21-Dec-04 rbd     4.0.3 - Increase journalling buffer size.
' 12-Jun-07 jab     cleaned up enabling of GUI buttons wrt Can flags
' -----------------------------------------------------------------------------
Option Explicit

Private BtnState As Integer

' ======
' EVENTS
' ======

Private Sub Form_Load()

    BtnState = 0
    FloatWindow Me.hwnd, False
    
End Sub

Private Sub Form_Unload(Cancel As Integer)

    On Error Resume Next
    Unload frmSetup
    If Not g_show Is Nothing Then _
        Unload g_show
    DoShutdown
    
End Sub

Private Sub cmdGoto_Click(index As Integer)
    Dim Az As Double
    
    Az = INVALID_COORDINATE
    On Error Resume Next
    Az = CDbl(txtNewAz.Text)
    On Error GoTo 0
    If Az < -360 Or Az > 360 Then
        MsgBox "Input value must be between" & _
            vbCrLf & "+/- 360", vbExclamation
        Exit Sub
    End If
    
    Az = AzScale(Az)
    
    If index = 0 Then
        HW_Move Az
    Else
        HW_Sync Az
    End If
    
End Sub

Private Sub cmdHome_Click()

    If g_bAtHome Then
        MsgBox "Already at home.", vbExclamation
        Exit Sub
    End If
    
    If g_bCanFindHome Then _
        HW_FindHome
    
End Sub

Private Sub cmdPark_Click()

    If g_bAtPark Then
        MsgBox "Already parked.", vbExclamation
        Exit Sub
    End If
        
    If g_bCanPark Then
        If g_dSetPark < -360 Or g_dSetPark > 360 Then
            MsgBox "Park location must be between +/- 360." & vbCrLf & _
                "Click on [Setup] to change it.", vbExclamation
            Exit Sub
        End If
    
        HW_Park
    End If
    
End Sub

Private Sub cmdSetup_Click()

    DoSetup                         ' May change our topmost state
    
End Sub

Private Sub cmdSlew_MouseDown(index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
    
    If g_eShutterState = shutterError Then
        If index = 5 Or index = 6 Or index = 7 Then
            MsgBox "Shutter must be Closed to clear the error.", vbExclamation
            Exit Sub
        End If
    End If
    
    If index = 5 Or index = 6 Then
        If g_eShutterState = shutterClosed Then
            MsgBox "Shutter must be open first.", vbExclamation
            Exit Sub
        End If
    End If
        
    BtnState = index

End Sub

Private Sub cmdTraffic_Click()

    If g_show Is Nothing Then _
        Set g_show = New frmShow
    
    g_show.Caption = "Dome Simulator ASCOM Traffic"
    g_show.Show
    
End Sub

Private Sub imgBrewster_Click()

    DisplayWebPage "http://astro.brewsters.net/"
    
End Sub

Private Sub timer_Timer()

    timer_tick
    
End Sub

' =================
' PUBLIC PROPERTIES
' =================

Public Property Get ButtonState() As Integer

    ButtonState = BtnState
    BtnState = 0
        
End Property

Public Property Let DomeAz(Az As Double)

    If Az = INVALID_COORDINATE Then
        txtDomeAz.Caption = "---.-"
    Else
        Az = AzScale(Az)
        txtDomeAz.Caption = Format$(Az, "000.0")
    End If

End Property

Public Property Let Shutter(Alt As Double)

    If Alt = INVALID_COORDINATE Or Not g_bCanSetShutter Then
        txtShutter.Caption = "----"
    Else
        Select Case g_eShutterState
            Case shutterOpen:
                If g_bCanSetAltitude Then
                    txtShutter.Caption = Format$(Alt, "0.0")
                Else
                    txtShutter.Caption = "Open"
                End If
            Case shutterClosed: txtShutter.Caption = "Closed"
            Case shutterOpening: txtShutter.Caption = "Opening"
            Case shutterClosing: txtShutter.Caption = "Closing"
            Case shutterError: txtShutter.Caption = "Error"
        End Select
    End If

End Property

' ==============
' LOCAL ROUTINES
' ==============

Public Sub LabelButtons()
  
    If g_bCanPark Then
        If g_dSetPark = INVALID_COORDINATE Then
            cmdPark.Enabled = False
            cmdPark.Caption = "Park"
        Else:
            cmdPark.Enabled = True
            cmdPark.Caption = "Park: " & Format$(g_dSetPark, "000.0") & "°"
        End If
    Else
        cmdPark.Enabled = False
        cmdPark.Caption = "Park"
    End If
    
    cmdHome.Enabled = g_bCanFindHome

    cmdGoto(0).Enabled = g_bCanSetAzimuth
    cmdGoto(1).Enabled = g_bCanSyncAzimuth
    txtNewAz.Enabled = g_bCanSetAzimuth Or g_bCanSyncAzimuth
    
    cmdSlew(1).Enabled = g_bCanSetAzimuth
    cmdSlew(2).Enabled = g_bCanSetAzimuth
    cmdSlew(3).Enabled = g_bCanSetAzimuth
    cmdSlew(4).Enabled = g_bCanSetAzimuth
    cmdSlew(5).Enabled = g_bCanSetAltitude And g_bCanSetShutter
    cmdSlew(6).Enabled = g_bCanSetAltitude And g_bCanSetShutter
    cmdSlew(7).Enabled = g_bCanSetShutter
    cmdSlew(8).Enabled = g_bCanSetShutter
    
End Sub

Public Sub RefreshLEDs()
    
    lblPARK.ForeColor = IIf(g_bAtPark, &HFF00&, &H404080)   ' Green
    lblHOME.ForeColor = IIf(g_bAtHome, &HFF00&, &H404080)   ' Green
    lblSLEW.ForeColor = IIf(HW_Slewing, &HFFFF&, &H404080)  ' Yellow
    
End Sub
