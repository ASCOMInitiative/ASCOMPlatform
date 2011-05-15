VERSION 5.00
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "msflxgrd.ocx"
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "Comdlg32.ocx"
Begin VB.Form frmTemp 
   Caption         =   "Temp. Comp."
   ClientHeight    =   5190
   ClientLeft      =   165
   ClientTop       =   855
   ClientWidth     =   3945
   BeginProperty Font 
      Name            =   "Small Fonts"
      Size            =   6
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "frmTemp.frx":0000
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   5190
   ScaleWidth      =   3945
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame Frame2 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   795
      Left            =   900
      TabIndex        =   22
      Top             =   4350
      Width           =   2145
      Begin VB.TextBox txtIntercept 
         Alignment       =   1  'Right Justify
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   225
         Left            =   90
         TabIndex        =   11
         Top             =   240
         Width           =   855
      End
      Begin VB.TextBox txtSlope 
         Alignment       =   1  'Right Justify
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   225
         Left            =   1140
         TabIndex        =   12
         Top             =   240
         Width           =   855
      End
      Begin VB.TextBox txtCalcPos 
         Alignment       =   1  'Right Justify
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   225
         Left            =   2910
         TabIndex        =   14
         Top             =   240
         Visible         =   0   'False
         Width           =   615
      End
      Begin VB.TextBox txtEnterTemp 
         Alignment       =   1  'Right Justify
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   225
         Left            =   2190
         TabIndex        =   13
         Top             =   240
         Visible         =   0   'False
         Width           =   525
      End
      Begin VB.Label Label12 
         AutoSize        =   -1  'True
         BackColor       =   &H00E0E0E0&
         Caption         =   "Slope"
         BeginProperty Font 
            Name            =   "Small Fonts"
            Size            =   6.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   165
         Left            =   1380
         TabIndex        =   28
         Top             =   510
         Width           =   360
      End
      Begin VB.Label Label10 
         AutoSize        =   -1  'True
         BackColor       =   &H00E0E0E0&
         Caption         =   "Y-Intercept "
         BeginProperty Font 
            Name            =   "Small Fonts"
            Size            =   6.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   165
         Left            =   180
         TabIndex        =   27
         Top             =   510
         Width           =   705
      End
      Begin VB.Label Label3 
         Caption         =   "="
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   2760
         TabIndex        =   26
         Top             =   240
         Visible         =   0   'False
         Width           =   105
      End
      Begin VB.Label Label4 
         Caption         =   "+"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   990
         TabIndex        =   25
         Top             =   240
         Visible         =   0   'False
         Width           =   105
      End
      Begin VB.Label Label5 
         Caption         =   "x"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   2040
         TabIndex        =   24
         Top             =   240
         Visible         =   0   'False
         Width           =   105
      End
      Begin VB.Label lblEnterTemp 
         AutoSize        =   -1  'True
         BackColor       =   &H00E0E0E0&
         Caption         =   "Temp. "
         BeginProperty Font 
            Name            =   "Small Fonts"
            Size            =   6.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   165
         Left            =   2190
         TabIndex        =   23
         Top             =   510
         Visible         =   0   'False
         Width           =   420
      End
   End
   Begin VB.Timer tmrAutorun 
      Interval        =   1000
      Left            =   2910
      Top             =   3210
   End
   Begin VB.Frame Frame1 
      Caption         =   " Run Parameters "
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1125
      Left            =   150
      TabIndex        =   15
      Top             =   90
      Width           =   3675
      Begin VB.OptionButton optAbsRel 
         Caption         =   "Rel. Specified"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   2
         Left            =   120
         TabIndex        =   29
         Top             =   810
         Width           =   1335
      End
      Begin VB.CommandButton cmdAuto 
         Caption         =   "Auto"
         Enabled         =   0   'False
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
         Left            =   1650
         MaskColor       =   &H00FFFFFF&
         Style           =   1  'Graphical
         TabIndex        =   3
         Top             =   570
         Width           =   495
      End
      Begin VB.TextBox txtDeadZone 
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
         Left            =   2910
         TabIndex        =   5
         Top             =   600
         Width           =   405
      End
      Begin VB.TextBox txtAutoRate 
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
         Left            =   2190
         TabIndex        =   4
         Top             =   600
         Width           =   345
      End
      Begin VB.CommandButton cmdManual 
         Caption         =   "Manual"
         Enabled         =   0   'False
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
         Left            =   1650
         Style           =   1  'Graphical
         TabIndex        =   2
         Top             =   240
         Width           =   885
      End
      Begin VB.OptionButton optAbsRel 
         Caption         =   "Rel. Computed"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   1
         Left            =   120
         TabIndex        =   1
         Top             =   540
         Width           =   1365
      End
      Begin VB.OptionButton optAbsRel 
         Caption         =   "Absolute"
         Enabled         =   0   'False
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
         Index           =   0
         Left            =   120
         TabIndex        =   0
         Top             =   270
         Value           =   -1  'True
         Width           =   1125
      End
      Begin VB.Label Label2 
         AutoSize        =   -1  'True
         Caption         =   "Pos. Units"
         BeginProperty Font 
            Name            =   "Small Fonts"
            Size            =   6.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   165
         Left            =   2790
         TabIndex        =   19
         Top             =   870
         Width           =   645
      End
      Begin VB.Line Line2 
         BorderColor     =   &H80000005&
         X1              =   2640
         X2              =   2640
         Y1              =   180
         Y2              =   1050
      End
      Begin VB.Line Line1 
         BorderColor     =   &H80000005&
         X1              =   1560
         X2              =   1560
         Y1              =   210
         Y2              =   1050
      End
      Begin VB.Label Label1 
         Caption         =   "Minutes"
         BeginProperty Font 
            Name            =   "Small Fonts"
            Size            =   6.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   165
         Left            =   2070
         TabIndex        =   18
         Top             =   900
         Width           =   525
      End
      Begin VB.Label Label8 
         Caption         =   "Dead Zone"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Left            =   2730
         TabIndex        =   17
         Top             =   300
         Width           =   825
      End
   End
   Begin MSComDlg.CommonDialog CommDialog 
      Left            =   3450
      Top             =   3210
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      CancelError     =   -1  'True
      Filter          =   "Temp Comp Data (*.tcd)|*.tcd|All Files (*.*)|*.*"
   End
   Begin VB.TextBox txtPos 
      Alignment       =   1  'Right Justify
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
      Left            =   2760
      TabIndex        =   10
      Top             =   3900
      Width           =   795
   End
   Begin VB.TextBox txtTemp 
      Alignment       =   1  'Right Justify
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
      Left            =   1890
      TabIndex        =   9
      Top             =   3900
      Width           =   855
   End
   Begin VB.TextBox txtTime 
      Alignment       =   2  'Center
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
      Left            =   1080
      TabIndex        =   8
      Top             =   3900
      Width           =   825
   End
   Begin VB.TextBox txtDate 
      Alignment       =   2  'Center
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
      Left            =   270
      TabIndex        =   7
      Top             =   3900
      Width           =   825
   End
   Begin VB.CommandButton cmdGetTemp 
      Caption         =   "Get Focus Position"
      Enabled         =   0   'False
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   315
      Left            =   1155
      TabIndex        =   6
      Top             =   3240
      Width           =   1665
   End
   Begin MSFlexGridLib.MSFlexGrid rData 
      Height          =   1515
      Left            =   135
      TabIndex        =   16
      Top             =   1590
      Width           =   3675
      _ExtentX        =   6482
      _ExtentY        =   2672
      _Version        =   393216
      Rows            =   1000
      Cols            =   4
      FixedCols       =   0
      ScrollTrack     =   -1  'True
      ScrollBars      =   2
      FormatString    =   "<Date        |<Time         |>       Temp.|>    Position"
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
   End
   Begin VB.Label Label11 
      AutoSize        =   -1  'True
      BackColor       =   &H00E0E0E0&
      Caption         =   " Latest Position "
      BeginProperty Font 
         Name            =   "Small Fonts"
         Size            =   6.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   165
      Left            =   270
      TabIndex        =   21
      Top             =   3690
      Width           =   1020
   End
   Begin VB.Label Label9 
      AutoSize        =   -1  'True
      BackColor       =   &H00E0E0E0&
      Caption         =   "Temp. Comp. Movement History (RFTrack.log) "
      BeginProperty Font 
         Name            =   "Small Fonts"
         Size            =   6.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   165
      Left            =   135
      TabIndex        =   20
      Top             =   1380
      Width           =   2970
   End
   Begin VB.Menu mnuFile 
      Caption         =   "&File"
      Begin VB.Menu mnuNew 
         Caption         =   "&New"
      End
      Begin VB.Menu mnuOpen 
         Caption         =   "&Open"
      End
      Begin VB.Menu mnuSave 
         Caption         =   "&Save"
      End
      Begin VB.Menu mnuSaveAs 
         Caption         =   "Save &As"
      End
      Begin VB.Menu bar2 
         Caption         =   "-"
      End
      Begin VB.Menu mnuClearMH 
         Caption         =   "&Clear Movement History"
      End
      Begin VB.Menu bar 
         Caption         =   "-"
      End
      Begin VB.Menu mnuExit 
         Caption         =   "&Exit"
      End
   End
   Begin VB.Menu mnuView 
      Caption         =   "&View"
      Begin VB.Menu mnuViewTraining 
         Caption         =   "&Training Window"
      End
      Begin VB.Menu mnuViewCal 
         Caption         =   "Temperature &Calibration"
      End
      Begin VB.Menu bar3 
         Caption         =   "-"
      End
      Begin VB.Menu mnuTempUnits 
         Caption         =   "Raw Temperature Units"
         Checked         =   -1  'True
         Index           =   0
      End
      Begin VB.Menu mnuTempUnits 
         Caption         =   "Fahrenheit"
         Index           =   1
      End
      Begin VB.Menu mnuTempUnits 
         Caption         =   "Celsius"
         Index           =   2
      End
   End
End
Attribute VB_Name = "frmTemp"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Const maxRowsRun As Integer = 1001

Dim lastAutoReading As Single
Dim lastAutoReadingDate As Date
Dim newFileFlag As Boolean
Dim formLoadingFlag As Boolean  ' true when this form is loading

Dim forceRunLine As Boolean     ' set true to force entry of a line into the run grid

Public Sub cmdAuto_Click()
  
  Dim autoRate As Double
  
  ' toggle auto-correct state
  If autoCorrect = True Then
    
    autoCorrect = False
    cmdAuto.BackColor = LightGray
    
  ' tmrAutorun.interval = 0
    
    ' allow dataset changes
    frmTrain!fraTrain.Caption = " Training Controls "
    frmTrain!fraTrain.ForeColor = Black
    
  Else
  
    autoCorrect = True
    cmdAuto.BackColor = Green
    
    If gblNextRowRun > maxRowsRun Then
      Call disableAutoRun
    Else
    ' tmrAutorun.interval = 1000
      lastAutoReading = Timer
      lastAutoReadingDate = Date
    End If
    
    ' no dataset changes during auto mode
    frmTrain!fraTrain.Caption = " Dataset Locked during Auto Mode "
    frmTrain!fraTrain.ForeColor = DarkRed
    
  End If
  
End Sub

Public Sub cmdGetTemp_Click()
  
  Dim start As Single
  
  ' clear previous values
  txtDate.Text = ""
  txtTime.Text = ""
  txtTemp.Text = ""
  txtPos.Text = ""
  gblTemp = ""
  
  ' get temperature string
  cmdString = "FT000000"
  cmdString = appendChecksum(cmdString)
  Call sendCommand(cmdString, True)
  
  ' wait until the temp data appears
  start = Timer
  While Timer < start + 0.2 And gblTemp = ""
    DoEvents
  Wend
  
  If gblTemp <> "" Then
  
'    tempEditVal = gblTemp
    txtTemp.Text = displayTemp(gblTemp)
    
    ' fill the other edit boxes
    txtDate.Text = Format(Date, "mm/dd/yy")
    txtTime.Text = Format(Time, "hh:mm:ss")
    txtPos.Text = absolutePos
      
'    ' fill the formula temp field
'    calcEditVal = tempEditVal
'    txtEnterTemp.Text = displayTemp(CStr(calcEditVal))
'
'    tableInsertFlag = False  '  append to table
'
'  ' txtCalcPos.Text = CLng(computeY(CDbl(txtEnterTemp.Text)))
'    txtCalcPos.Text = CLng(computeY(CDbl(calcEditVal)))
    
  End If
  
End Sub

Public Sub cmdManual_Click()

  Dim newFocus As Long, dataline As String
  Dim tempDiff As Long, posDiff As Long
  Dim slope As Double
  Dim currentFocus As Long
  Dim start As Single, C As Integer
  Dim currentDate, currentTime
  
  On Error GoTo errHandler
  
  If cmdActive Then
    ' another click will interrupt the current command
    Exit Sub
  End If
  
  If autoCorrect = False Then
    ' if not in auto mode, light up while refreshing
    cmdManual.BackColor = Green
  End If
  
  txtDate.Text = ""
  txtTime.Text = ""
  txtTemp.Text = ""
  txtPos.Text = ""
  
  Call highlightRow(selectedRow, False)
  
  If gblNextRowRun > maxRowsRun Then
    Call disableAutoRun
  Else
  
'    currentFocus = frmMain!actualPos.Text
    currentFocus = absolutePos

    ' if step size has changed in config screen, send the one from the datafile
    If gblFileStepSize <> CLng(frmConfig!txtStepSize.Text) And _
                                    gblFileStepSize > 0 And gblFileStepSize < 256 Then
    
      frmConfig!txtStepSize.Text = CStr(gblFileStepSize)
      gblStepSize = gblFileStepSize
      Call setMotorConfigParams
    
    ' Call showMsg("Step Size has been reset to " & gblFileStepSize & ".", "", 3) ' , frmTemp.left + 1000, frmTemp.top + 1000)
    
    End If
    
    If Not IsNumeric(gblTemp) Then
      ' avoid problems when gblTemp is not set due to delay from firmware
      ' take a temperature reading and bail out
      cmdString = "FT000000"
      cmdString = appendChecksum(cmdString)
      Call sendCommand(cmdString, True)
      Exit Sub
    End If
    
    gblTemp = ""
    
    ' request temperature reading
    cmdString = "FT000000"
    cmdString = appendChecksum(cmdString)
    Call sendCommand(cmdString, True)
    
    ' wait for the temp data to appear
    start = Timer
    While Timer < start + 0.3 And gblTemp = ""
      DoEvents
    Wend
    
    If gblTemp <> "" Then
      
      tempEditVal = CSng(gblTemp)
      txtTemp.Text = displayTemp(CStr(tempEditVal))
      
      ' fill the other edit boxes
      currentDate = Date
      currentTime = Time
      txtDate.Text = Format(currentDate, "mm/dd/yy")
      txtTime.Text = Format(currentTime, "hh:mm:ss")
      txtPos.Text = CStr(absolutePos)
      
      calcEditVal = tempEditVal
      txtEnterTemp.Text = displayTemp(CStr(calcEditVal))
      txtCalcPos.Text = CStr(Int(computeY(CDbl(calcEditVal))))
      
      ' absolute or relative?
      If optAbsRel(1).Value = True Or optAbsRel(2).Value = True Then
        
        ' relative
        tempDiff = CLng(CInt(tempEditVal) - baselineTemp)
        If optAbsRel(1).Value = True Then
          slope = gblSlope
        Else
          slope = gblSlopeManual
        End If
    '   posDiff = Int(Slope * tempDiff)
        posDiff = Round(CLng(slope) * tempDiff)
        newFocus = posDiff + CLng(baselinePos)
        
      Else
        
        ' absolute
        newFocus = CLng(Int(computeY(CDbl(tempEditVal))))
      
      End If
      
      tempDiff = Abs(currentFocus - newFocus)
      If newFocus > 0 And newFocus <= CLng(frmConfig!maxTravel.Text) Then
        If (tempDiff > CLng(Int(txtDeadZone.Text))) Or forceRunLine Then
        
          ' display in the Run Grid
          With rData
             
             ' add another data row
             .Rows = gblNextRowRun + 1
  
            .Row = gblNextRowRun
            
            .Col = 0
            .Text = Format(currentDate, "mm/dd/yy")
            
            .Col = 1
            .Text = Format(currentTime, "hh:mm:ss")
            
            .Col = 2
            runTempArray(gblNextRowRun) = CInt(tempEditVal)
            .Text = displayTemp(CStr(tempEditVal))
            
            .Col = 3
            .Text = CStr(newFocus)
          
            For C = 0 To 3
              .Col = C
              If optAbsRel(0).Value = True Then
                .CellForeColor = Black
              Else
                .CellForeColor = Red
              End If
            Next C
            
          End With
          
          gblNextRowRun = gblNextRowRun + 1
          If gblNextRowRun >= maxRowsRun Then
            autoCorrect = False
            Call disableAutoRun
          Else
            
            ' keep the last reading in view
            If gblNextRowRun > 3 Then
              rData.TopRow = CLng(gblNextRowRun) - 3
            End If
            
            ' set the focus to the computed value
            cmdString = "FG" & Format(newFocus, "000000")
            cmdString = appendChecksum(cmdString)
            
            Call sendCommand(cmdString, True)
            Debug.Print newFocus, cmdString
            
            dataline = Format(Date, "mm/dd/yy") & " " & _
                       Format(Time, "hh:mm:ss") & " " & _
                       Right("     " & CStr(tempEditVal), 5) & " " & _
                       Right("     " & displayTemp(CStr(tempEditVal)), 5) & " " & _
                       CStr(newFocus) & " " & _
                       IIf(optAbsRel(0).Value, "Abs", "Rel")
      
            Open App.Path & "\RFTrack.log" For Append As #1
            Print #1, dataline
            Close #1
            
          End If
          
          forceRunLine = False
          
        End If
      End If
    End If
  End If
  
  cmdManual.BackColor = LightGray

  On Error GoTo 0
  
  Exit Sub
  
errHandler:
    
  Call showMsg("Error in frmTemp cmdManual_Click:" & vbCrLf & vbCrLf & _
       "(" & Err.Description & ", #" & Err.Number & ".)", "", 5)
  Debug.Print "cmdManual_Click: " & Err.Description
  Resume Next
  
End Sub

Private Sub Form_Load()
  
  Dim R As Integer, start As Single
  
  On Error GoTo errHandler
  
  formLoadingFlag = True
  
  Call setFormPositionIni(Me, "0 2000 4065 6000") ' old h 6000 (jab)
  
  gblNextRowRun = 1
  
  If frmComm!commControl.PortOpen Then
    cmdGetTemp.Enabled = True
  End If
  
  unsavedData = False
  gblValidDataSet = False
  
  ' save the previous log file if it exists ...
  If Len(Dir(App.Path & "\RFTrack.log")) > 0 Then
    FileCopy App.Path & "\RFTrack.log", App.Path & "\RFTrack.bak"
  End If
  
  ' ... and create a new one.
  Open App.Path & "\RFTrack.log" For Output As #1
  Close #1
  
  rData.Rows = 2
  
  forceRunLine = True
  
  If gblTempConvBeta = 0 Then
    ' calibration has not been done
    
    tempDisplayUnits = 0
    
    mnuTempUnits(1).Enabled = False
    mnuTempUnits(2).Enabled = False
    
  Else
  
    mnuTempUnits(1).Enabled = True
    mnuTempUnits(2).Enabled = True
    
    tempDisplayUnits = CInt(IniGetData("Temp Comp", "Display Units", 0, glIniFile))
    
  End If
  
  Call mnuTempUnits_Click(tempDisplayUnits)
  Call mnuNew_Click
  
  gblShowingRelative = True
  
  ' for relative specified mode
  optAbsRel(2).Value = True
  gblAbsRelMode = 2
  
  gblSlopeManual = IniGetData("Temp Comp", "Manual Slope", 0, glIniFile)
  txtSlope.Text = displaySlope(gblSlopeManual)
  
'  gblStepSize = IniGetData("Config", "Step Size", frmConfig!txtStepSize.Text, glIniFile)
  
  txtAutoRate.Text = IniGetData("Temp Comp", "Interval", 3, glIniFile)
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp Form_Load: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub Form_Paint()

  Dim boxMargin As Integer, winCoords() As String
  
  ' draw a box around the edge
  ' left, top, width, height
  winCoords = Split(dfltWin(6))
  
  boxMargin = 0

  Me.FillStyle = 1
  Me.Line (boxMargin, boxMargin)- _
                (winCoords(2) - boxMargin - 125, winCoords(3) - boxMargin - 810), Gray, B

End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
  
  Dim response
  
  On Error GoTo errHandler
  
  If gblStopUnload Then
    Cancel = 1
    Exit Sub
  Else
    If autoCorrect = True Then
      response = MsgBox("OK to stop Auto-correct?", vbYesNo + vbQuestion)
      If response = vbYes Then
       autoCorrect = False
       Call Delay(3, True)
      Else
        Cancel = 1
      End If
    End If
    
    If Cancel = 0 And unsavedData Then
      response = MsgBox("Some Temperature Compensation data or parameters have changed." & vbCrLf & _
                                          "Save before exiting?", vbYesNoCancel + vbExclamation)
      If response = vbCancel Then
        Cancel = 1
      ' gblStopUnload = True
        Exit Sub
      End If
  
      If response = vbYes Then
        mnuSaveAs_Click
      Else
        unsavedData = False
      End If
    End If
      
    gblShowingTraining = False
    Unload frmTrain
    
    gblShowingCal = False
    Unload frmCal
  
  End If
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp Form_QueryUnload: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub Form_Unload(Cancel As Integer)
  
  On Error GoTo errHandler
  
  gblValidDataSet = False
  tmrAutorun.interval = 0
  
  Call saveFormPosition(Me)
    
  frmConfig!txtStepSize.Enabled = True
    
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp Form_Unload: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Sub mnuClearMH_Click()

  Dim C As Integer
  
  On Error GoTo errHandler
  
  rData.Row = 1
  For C = 0 To 3
    rData.Col = C
    rData.Text = ""
  Next C
  rData.Rows = 2
  
  gblNextRowRun = 1

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp mnuClearMH_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub mnuExit_Click()
  
  On Error GoTo errHandler
  
  Unload Me
  
  gblStopUnload = False
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp mnuExit_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Sub mnuNew_Click()
  
  Dim R As Integer, C As Integer, response
  
  On Error GoTo errHandler
  
  If unsavedData Then
    response = MsgBox("Save current data?", vbYesNo + vbQuestion)
    If response = vbYes Then
      mnuSaveAs_Click
      mnuNew_Click
    End If
  End If
    
  frmTemp.MousePointer = 11  '  hourglass
  
  Call highlightRow(selectedRow, False)
  selectedRow = 1
  
  frmTrain!gData.Row = 1
  For C = 0 To 4
    frmTrain!gData.Col = C
    frmTrain!gData.Text = ""
  Next C
  frmTrain!gData.Rows = 2
  
  gblNextRow = 1
  
  txtIntercept.Text = ""
  txtSlope.Text = ""
  
  txtDate.Text = ""
  txtTime.Text = ""
  txtTemp.Text = ""
  txtPos.Text = ""
  
  txtCalcPos.Text = ""
  txtEnterTemp.Text = ""

  txtAutoRate.Text = IniGetData("Temp Comp", "Interval", 3, glIniFile)

  txtDeadZone.Text = 1
  frmTrain!txtNotes.Text = ""
  frmTrain!txtStats.Text = ""
  
  calcEditVal = 0
  tempEditVal = 0
  trainEditVal = 0
  
  frmTemp.MousePointer = 0  '  normal
  
  frmTemp.Caption = "Temp. Comp. - unsaved file"
  frmTrain.Caption = "Training - unsaved file"
  
  newFileFlag = True
  defFile = ""
  initDir = ""
  
  savedStepSize = frmConfig!txtStepSize.Text
  
  unsavedData = False
  gblValidDataSet = False
  
  Call enableTCButtons
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp mnuNew_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub mnuOpen_Click()
  Dim slashPos As Long
  Dim dataline As String, dataElements() As String
  Dim R As Integer, C As Integer
  Dim response
  Dim deadZone As Integer, StepSize As Integer, absRel As String
  Dim inputString As String, inputArray() As String
  Dim lineDate As String, lineTime As String
  
  On Error GoTo errorHandler
  
  loadingFlag = True
  newFileFlag = False
  
  Call highlightRow(selectedRow, False)
  
  If IniGetData("Temp Comp", "Warn Before Load", 0, glIniFile) = 1 Then
    ' warn the user that loading a file will override any step size currently in use.
    frmWarnBeforeLoad.Show vbModal
    If warnResult = False Then
      Exit Sub
    End If
  End If
  
  If unsavedData Then
    response = MsgBox("Save current data?", vbYesNo + vbQuestion)
    If response = vbYes Then
      mnuSaveAs_Click
      mnuNew_Click
    End If
  End If
    
  With CommDialog
    
    .CancelError = True
    
    .DialogTitle = "Open a Data Set"
    
    ' get the default file and path
    defFile = IniGetData("Temp Comp", "File Name", "", glIniFile)
    initDir = IniGetData("Temp Comp", "Path Name", initDir, glIniFile)
  
    .FileName = initDir & defFile
  
    ' handle the startup situation: no file info in ini
    If .FileName = "\" Then
      .FileName = ""
    End If
    
    .ShowOpen
  
    If .FileName <> "" Then
      
      Open .FileName For Input As #2
      Call loadTCdata
      Close #2
    
      ' save the new directory
      slashPos = InStrRev(.FileName, "\")
      initDir = left(.FileName, slashPos - 1) & "\"
      defFile = Right(.FileName, Len(.FileName) - slashPos)
  
      frmTemp.Caption = "Temp. Comp. - " & defFile
      frmTrain.Caption = "Training - " & defFile
    
      ' save the file and path as the new default
      Call iniPutData("Temp Comp", "File Name", defFile, glIniFile)
      Call iniPutData("Temp Comp", "Path Name", initDir, glIniFile)
    
    End If
      
      
    loadingFlag = False
    
    Unload frmAbsRel
    
  End With
  
  On Error GoTo 0
  
  Exit Sub
  
errorHandler:
  
  If Err.Number = 32755 Then
    ' cancel was pressed
    Exit Sub
  End If
    
  Call showMsg("This file is damaged or is not a Temp. Comp. Dataset." & vbCrLf & vbCrLf & _
         "(" & Err.Description & ", #" & Err.Number & ".)", "", 5)
    
  Close #2
  
  loadingFlag = False
  mnuNew_Click
  
  On Error GoTo 0
 'Resume Next
  
End Sub

Public Sub mnuSaveAs_Click()
  Dim slashPos As Long, response
  
  On Error GoTo errorHandler
  
  With CommDialog
  .CancelError = True
  .DialogTitle = "Save this Data Set"
  .FileName = defFile
  .initDir = initDir
     
  .ShowSave
  
  If .FileName <> "" Then
    
    ' if it exists, ask before overwriting
    If isFile(.FileName) Then
      response = MsgBox(.FileName & " already exists. OK to overwrite it?", _
                                                              vbYesNoCancel + vbQuestion)
    Else
      response = vbYes
    End If
    
    If response = vbCancel Then
      Exit Sub
    End If
    
    If response = vbYes Then
      ' save the new directory
      slashPos = InStrRev(.FileName, "\")
      initDir = left(.FileName, slashPos - 1) & "\"
      defFile = Right(.FileName, Len(.FileName) - slashPos)
  
      Call writeData
          
      ' save the file and path as the new default
      Call iniPutData("Temp Comp", "File Name", defFile, glIniFile)
      Call iniPutData("Temp Comp", "Path Name", initDir, glIniFile)
      
      frmTemp.Caption = "Temp. Comp. - " & defFile
      frmTrain.Caption = "Training - " & defFile
      
    End If
  
  End If
  
  End With
   
  unsavedData = False
  
  On Error GoTo 0
  Exit Sub
  
errorHandler:
  
  If Err.Number = 32755 Then
    ' cancel was pressed
    Exit Sub
  End If
  
  Call showMsg("Error opening file:" & vbCrLf & "Error number " & Err.Number & " - " & Err.Description & ".", "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub mnuSave_Click()
  Dim slashPos As Long
  
  On Error GoTo errHandler
  
  With CommDialog
    
  .FileName = initDir & defFile
  If defFile = "" Then
    mnuSaveAs_Click
  Else
    
    Call writeData
    
    ' save the new directory
    slashPos = InStrRev(.FileName, "\")
    initDir = left(.FileName, slashPos - 1) & "\"
    defFile = Right(.FileName, Len(.FileName) - slashPos)
  
    ' save the file and path as the new default
    Call iniPutData("Temp Comp", "File Name", defFile, glIniFile)
    Call iniPutData("Temp Comp", "Path Name", initDir, glIniFile)
  
    frmTemp.Caption = "Temp. Comp. - " & defFile
    frmTrain.Caption = "Training - " & defFile
    
  End If
  
  End With
  
 'MsgBox "Saved."
  
  unsavedData = False
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp mnuSave_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Sub writeData()
  Dim R As Integer, C As Integer, dataline As String
  
  On Error GoTo errHandler
  
  frmTemp.Caption = "Temp. Comp. - " & defFile
  frmTrain.Caption = "Training - " & defFile
  
  Open defFile For Output As #2
  
  ' save the notes, default interval and dead zone values, microsteps/step & abs/rel state
  Notes = Replace(frmTrain!txtNotes.Text, vbCrLf, Chr(255))
  Print #2, Notes
  
  Print #2, "Auto Rate = " & txtAutoRate.Text
  Print #2, "Dead Zone = " & txtDeadZone.Text
  
  Dim a
  a = CInt(optAbsRel(0).Value)
  If CInt(optAbsRel(0).Value) = 0 Then
    Print #2, "Absolute Mode"
  End If
  If CInt(optAbsRel(1).Value) = 1 Then
    Print #2, "Relative Computed"
  End If
  If CInt(optAbsRel(2).Value) = 2 Then
    Print #2, "Relative Specified"
  End If
  
  
  If newFileFlag = True Then
    ' save the step size the first time we save the file, but never again
    Print #2, "Step Size = " & frmConfig!txtStepSize.Text
    savedStepSize = frmConfig!txtStepSize.Text
    newFileFlag = False
  Else
    Print #2, "Step Size = " & savedStepSize
  End If
   
  For R = 1 To gblNextRow - 1

    frmTrain!gData.Row = R
    dataline = ""
    For C = 1 To 2
      frmTrain!gData.Col = C
      dataline = dataline & frmTrain!gData.Text & " "
    Next C
    
    frmTrain!gData.Col = 3
    dataline = dataline & trainTempArray(R, 3) & " "

    frmTrain!gData.Col = 4
    dataline = dataline & frmTrain!gData.Text & " "
    
    ' add the X column
    frmTrain!gData.Col = 0
    dataline = dataline & frmTrain!gData.Text & " "
    
    dataline = Trim(dataline)
    
    Print #2, dataline
    
  Next R
      
  ' write the end-of-data flag
  Print #2, "~~~"
  
'  ' save the manually-entered slope
'  Print #2, "Manual Slope = " & gblSlopeManual
  
  Close #2
  
  Call showMsg("Saved.", "", 2)
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp writeData: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub mnuTempUnits_Click(Index As Integer)
  Dim rtc
  On Error GoTo errHandler
  
  tempDisplayUnits = Index
  
  Select Case Index
    
    Case 0
      mnuTempUnits(0).Checked = True
      mnuTempUnits(1).Checked = False
      mnuTempUnits(2).Checked = False
      
'      frmMain!lblTempUnits.Caption = "units"
      tempDisplayUnitsText = "units"
      
    Case 1
      mnuTempUnits(0).Checked = False
      mnuTempUnits(1).Checked = True
      mnuTempUnits(2).Checked = False
      
    ' frmMain!lblTempUnits.Caption = "°F."
      tempDisplayUnitsText = "°F."
      
    Case 2
      mnuTempUnits(0).Checked = False
      mnuTempUnits(1).Checked = False
      mnuTempUnits(2).Checked = True
      
    ' frmMain!lblTempUnits.Caption = "°C."
      tempDisplayUnitsText = "°C."
      
  End Select
  
  If optAbsRel(2).Value = True Then
    txtSlope.Text = displaySlope(gblSlopeManual)
  Else
    txtSlope.Text = displaySlope(gblSlope)
  End If
  
  Call iniPutData("Temp Comp", "Display Units", CStr(tempDisplayUnits), glIniFile)
  
  Call redisplayTemps
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp mnuTmpUnits_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub mnuViewCal_Click()
  
  On Error GoTo errHandler
  
  If gblShowingCal Then
    gblShowingCal = False
    frmCal.Hide
    mnuViewCal.Checked = False
  Else
    gblShowingCal = True
    frmCal.Show
    mnuViewCal.Checked = True
  End If

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp mnuViewCal_Click" & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub mnuViewTraining_Click()
  
  On Error GoTo errHandler
  
  If gblShowingTraining Then
    gblShowingTraining = False
    frmTrain.Hide
    mnuViewTraining.Checked = False
  Else
    gblShowingTraining = True
    frmTrain.Show
    mnuViewTraining.Checked = True
  End If
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp mnuViewTraining_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Sub optAbsRel_Click(Index As Integer)

  Dim start As Single, newFocus As Integer, dataline As String
  
  On Error GoTo errHandler
  
  If loadingFlag Then
    Exit Sub
  End If
 
  If formLoadingFlag Then
    ' don't show the Abs/Rel form when loading the Temperature form.
    formLoadingFlag = False
  Else
  
    If gblAbsRelCancel = False Then
      
      gblAbsRelMode = Index
      frmAbsRel.Show vbModal
    
    Else
      
      gblAbsRelMode = Index
      gblAbsRelCancel = False
      
      Unload frmAbsRel
    
    End If
    
    doCompute
    
  End If
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp optAbsRel_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub optAbsRel_MouseDown(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)

  On Error GoTo errHandler
  
  If optAbsRel(Index).Value = True Then
    ' it's already selected so click event won't fire, so force AbsRel window
    frmAbsRel.Show vbModal
  End If
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp optAbsRel_MouseDown: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub optCoeffSelect_Click(Index As Integer)
  
  On Error GoTo errHandler
  
  If Index = 0 Then
  
    ' revert to the computed values
    txtIntercept.Text = gblIntercept
    txtSlope.Text = displaySlope(gblSlopeManual)
    
  End If
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp optCoeffSelect_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub tmrAutorun_Timer()
    
  Dim newDay As Boolean
  
  On Error GoTo errHandler
  
  If IsNumeric(txtAutoRate.Text) Then
  
    ' set a flag if the date has rolled over, because the time logic doesn't work then
    If Date > lastAutoReadingDate Then
      lastAutoReadingDate = Date
      newDay = True
    Else
      newDay = False
    End If

    ' if the date has rolled over, or the specified time has elapsed, take a reading.
    If newDay Or (Timer > lastAutoReading + (CSng(txtAutoRate.Text) * 60)) Then
    
      ' if we're in automatic mode, take a reading
      If autoCorrect = True Then
      
      ' cmdAuto.BackColor = DarkGreen
        cmdAuto.Enabled = False
        
        cmdManual_Click
        
      ' cmdAuto.BackColor = Green
        cmdAuto.Enabled = True
        
        lastAutoReading = Timer
        
      End If
      
    End If
    
  End If

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp tmrAutoRun_Timer: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub txtDeadZone_KeyPress(KeyAscii As Integer)
  
  On Error GoTo errHandler
  
  If KeyAscii = Asc(vbCrLf) Then
    KeyAscii = 0 ' so it doesn't beep
    
    txtDeadZone_LostFocus
    
  End If

  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmTemp txtDeadZone_KeyPress")
  Resume Next

End Sub

Public Sub txtDeadZone_LostFocus()
  
  Dim currentDZone As Integer, dZone As Integer
  
  On Error GoTo errHandler
  
  currentDZone = IniGetData("Temp Comp", "Dead Zone", "1", glIniFile)
  
  If IsNumeric(txtDeadZone.Text) Then
    dZone = CInt(txtDeadZone.Text)
    If dZone > 0 And dZone < 1000 Then
      Call iniPutData("Temp Comp", "Dead Zone", txtDeadZone.Text, glIniFile)
      unsavedData = True
    Else
      txtDeadZone.Text = currentDZone
    End If
  Else
    txtDeadZone.Text = currentDZone
  End If

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp txtDeadZone_LostFocus: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub txtEnterTemp_KeyPress(KeyAscii As Integer)
  On Error GoTo errHandler

  If KeyAscii = Asc(vbCrLf) Then
  
    KeyAscii = 0 ' so it doesn't beep
    
'    Select Case tempDisplayUnits
'
'      Case 0  ' raw
'        calcEditVal = txtEnterTemp.Text
'
'      Case 1  ' f
'        calcEditVal = (degreesC(txtEnterTemp.Text) + 273) / gblTempConvBeta
'
'      Case 2  ' c
'        calcEditVal = (txtEnterTemp.Text + 273) / gblTempConvBeta
'
'    End Select
    
'   calcEditVal = txtEnterTemp.Text
    txtCalcPos.Text = CLng(computeY(CDbl(calcEditVal)))
   
  End If
    
  On Error GoTo 0
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmTemp txtEnterTemp_KeyPress")
  Resume Next
End Sub

Private Sub disableAutoRun()
  
  On Error GoTo errHandler
  
  If App.StartMode <> vbSModeAutomation Then
    MsgBox "The Run Log has filled up (1000 entries max.)" & vbCrLf & _
          "Auto-run has been disabled." & vbCrLf & vbCrLf & _
          "(You may want to increase the correction interval.)"
  End If
  
  tmrAutorun.interval = 0
  cmdAuto.Enabled = False
  cmdManual.Enabled = False

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp disableAutoRun: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub txtAutoRate_Change()

  Dim checkAutoRate As Single
  
  On Error GoTo errHandler
  
' If autoCorrect = True And IsNumeric(txtAutoRate.Text) Then
  If IsNumeric(txtAutoRate.Text) Then
    checkAutoRate = CSng(txtAutoRate.Text)
  
    If checkAutoRate >= 0 And checkAutoRate < 100 Then
      autoRate = checkAutoRate
      Call iniPutData("Temp Comp", "Interval", CStr(autoRate), glIniFile)
      If gblValidDataSet Then
        unsavedData = True
      End If
    Else
      Call showMsg("Please enter a value between 0 and 99.", "", 1)
      txtAutoRate.Text = autoRate
    End If
    
  End If
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp txtAutoRate_Change: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub txtNotes_Change()
  
  On Error GoTo errHandler
  
  unsavedData = True

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTemp txtNotes_Change: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub txtSlope_Change()
  If txtEnterTemp.Text <> "" Then
    Call txtEnterTemp_KeyPress(Asc(vbCrLf))
  End If
End Sub
