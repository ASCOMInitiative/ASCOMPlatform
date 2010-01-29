VERSION 5.00
Begin VB.Form frmMain 
   Caption         =   "RoboFocus"
   ClientHeight    =   1575
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4440
   Icon            =   "frmMain.frx":0000
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   1575
   ScaleWidth      =   4440
   StartUpPosition =   3  'Windows Default
   Begin VB.Timer tmrShowTemp 
      Interval        =   1000
      Left            =   1890
      Top             =   660
   End
   Begin VB.TextBox txtTemp 
      Alignment       =   2  'Center
      Appearance      =   0  'Flat
      BackColor       =   &H8000000F&
      Height          =   255
      Left            =   1890
      TabIndex        =   16
      Text            =   "9999 units"
      Top             =   90
      Visible         =   0   'False
      Width           =   795
   End
   Begin VB.TextBox actualPos 
      Alignment       =   1  'Right Justify
      BackColor       =   &H8000000E&
      Enabled         =   0   'False
      Height          =   255
      Left            =   3210
      TabIndex        =   14
      Top             =   870
      Width           =   690
   End
   Begin VB.CommandButton deltaButton 
      Caption         =   "OUT"
      Enabled         =   0   'False
      Height          =   345
      Index           =   1
      Left            =   1290
      MaskColor       =   &H8000000F&
      TabIndex        =   6
      Top             =   450
      Width           =   495
   End
   Begin VB.CommandButton openClosePort 
      Caption         =   "COM 1"
      Height          =   315
      Left            =   30
      MaskColor       =   &H8000000F&
      Style           =   1  'Graphical
      TabIndex        =   0
      ToolTipText     =   "Click to toggle the comm port state."
      Top             =   60
      Width           =   735
   End
   Begin VB.CommandButton cmdTemp 
      Caption         =   "&Temp. Comp."
      Height          =   315
      Left            =   2700
      TabIndex        =   2
      Top             =   60
      Width           =   1095
   End
   Begin VB.CommandButton cmdAbsRel 
      Enabled         =   0   'False
      Height          =   315
      Left            =   3945
      Picture         =   "frmMain.frx":0E42
      Style           =   1  'Graphical
      TabIndex        =   9
      Top             =   840
      Width           =   465
   End
   Begin VB.CommandButton cmdHelp 
      Caption         =   "&Help"
      Height          =   315
      Left            =   3855
      MaskColor       =   &H8000000F&
      TabIndex        =   3
      Top             =   60
      Width           =   555
   End
   Begin VB.CommandButton userButton 
      Caption         =   "user(4)"
      Enabled         =   0   'False
      Height          =   315
      Index           =   4
      Left            =   3315
      MaskColor       =   &H8000000F&
      Style           =   1  'Graphical
      TabIndex        =   13
      Top             =   1230
      Width           =   1095
   End
   Begin VB.TextBox deltaAmount 
      Alignment       =   1  'Right Justify
      BackColor       =   &H8000000E&
      Height          =   315
      Left            =   750
      TabIndex        =   5
      Top             =   450
      Width           =   495
   End
   Begin VB.CommandButton deltaButton 
      Caption         =   "IN"
      Enabled         =   0   'False
      Height          =   345
      Index           =   0
      Left            =   240
      MaskColor       =   &H8000000F&
      TabIndex        =   4
      Top             =   450
      Width           =   495
   End
   Begin VB.CommandButton cmdRefresh 
      Caption         =   "&Refresh"
      Enabled         =   0   'False
      Height          =   315
      Left            =   2460
      MaskColor       =   &H8000000F&
      Style           =   1  'Graphical
      TabIndex        =   8
      Top             =   840
      Width           =   735
   End
   Begin VB.CommandButton StopButton 
      Caption         =   "&STOP"
      Enabled         =   0   'False
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   345
      Left            =   240
      MaskColor       =   &H8000000F&
      TabIndex        =   7
      Top             =   810
      Width           =   1545
   End
   Begin VB.CommandButton config 
      Caption         =   "&Config."
      Height          =   315
      Left            =   810
      MaskColor       =   &H8000000F&
      TabIndex        =   1
      Top             =   60
      Width           =   1065
   End
   Begin VB.CommandButton userButton 
      Caption         =   "user(1)"
      Enabled         =   0   'False
      Height          =   315
      Index           =   1
      Left            =   30
      MaskColor       =   &H8000000F&
      Style           =   1  'Graphical
      TabIndex        =   10
      Top             =   1230
      Width           =   1095
   End
   Begin VB.CommandButton userButton 
      Caption         =   "user(2)"
      Enabled         =   0   'False
      Height          =   315
      Index           =   2
      Left            =   1125
      MaskColor       =   &H8000000F&
      Style           =   1  'Graphical
      TabIndex        =   11
      Top             =   1230
      Width           =   1095
   End
   Begin VB.CommandButton userButton 
      Caption         =   "user(3)"
      Enabled         =   0   'False
      Height          =   315
      Index           =   3
      Left            =   2220
      MaskColor       =   &H8000000F&
      Style           =   1  'Graphical
      TabIndex        =   12
      Top             =   1230
      Width           =   1095
   End
   Begin VB.Timer captionTimer 
      Interval        =   200
      Left            =   2070
      Top             =   1650
   End
   Begin VB.Label lblTempUnits 
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
      Left            =   2640
      TabIndex        =   17
      Top             =   480
      Width           =   315
   End
   Begin VB.Label Label1 
      Alignment       =   2  'Center
      Caption         =   "Position"
      Height          =   195
      Left            =   3240
      TabIndex        =   15
      Top             =   630
      Width           =   630
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public currentDelta As Integer

Private Sub actualPos_KeyPress(KeyAscii As Integer)

  Dim noResend As Boolean, passed As Boolean, notused As Long
  Dim movingTo As String
  Dim lowerLimit As Long, upperLimit As Long
  
  On Error GoTo errHandler

  If KeyAscii = Asc(vbCrLf) Then
    KeyAscii = 0 ' so it doesn't beep
  
    movingTo = actualPos.Text
    
    If gblShowingRelative Then
      lowerLimit = absolutePos * (-1)
      upperLimit = (maxTravelSetting - absolutePos)
      Call validateLong(movingTo, notused, "Position", _
                                        lowerLimit, upperLimit, _
                                        frmMain!actualPos, passed)
      If Not passed Then
        actualPos.Text = fixRelPos(relPos)
      End If
      
    Else
      lowerLimit = 1
      upperLimit = maxTravelSetting
      Call validateLong(movingTo, notused, "Position", _
                                        lowerLimit, upperLimit, _
                                        frmMain!actualPos, passed)
      If Not passed Then
        actualPos.Text = absolutePos
      End If
      
    End If
    
    
    If passed Then
    
      If gblShowingRelative Then
        movingTo = (movingTo - relPos) + absolutePos
      End If
      
      If val(frmConfig!maxTravel.Text) < val(movingTo) Then
        
        Call showMsg("Position may not be larger than Maximum Travel.", "", 2)
        
        If gblShowingRelative Then
          actualPos.Text = fixRelPos(relPos)
        End If
        
        actualPos.SetFocus
        
      Else
      
        Label1.Caption = "Moving to " & Format(movingTo)
        
        cmdString = "FG" & Format(movingTo, "000000")
        cmdString = appendChecksum(cmdString)
        
        noResend = True
        Call sendCommand(cmdString, noResend)
        
        actualPos.SetFocus
      
      End If
    
    End If
    
  End If
  
  On Error GoTo 0
  
  Exit Sub

errHandler:
  Call unknownErrorHandler("Main - actualPos_KeyPress")
  Resume Next
End Sub

Public Sub cmdAbsRel_Click()
  
  gblShowingRelative = Not gblShowingRelative
  
  If gblShowingRelative Then
    
    actualPos.ForeColor = Red
    startingAbsPos = absolutePos
    relPos = 0
    actualPos.Text = fixRelPos(absolutePos - startingAbsPos)
  
    frmConfig!txtPosition.Enabled = False
    
  Else
    
    actualPos.ForeColor = Black
    actualPos.Text = absolutePos
  
    frmConfig!txtPosition.Enabled = True
    
  End If
  
End Sub

Private Sub cmdHelp_Click()
  frmHelp.Show
End Sub

Public Sub cmdRefresh_Click()
  Dim i As Integer
  
  On Error GoTo errHandler
  
  ' light up while refreshing
  cmdRefresh.BackColor = Green
  
  ' refresh position
  cmdString = "FG000000"
  cmdString = appendChecksum(cmdString)
  Call sendCommand(cmdString, True)

  ' refresh power settings
  For i = 1 To 4
    userButton(i).Enabled = True
  Next i
  cmdString = "FP000000"
  cmdString = appendChecksum(cmdString)
  Call sendCommand(cmdString, True)
  
  ' refresh backlash setting
  cmdString = "FB000000"
  cmdString = appendChecksum(cmdString)
  Call sendCommand(cmdString, True)
  
  ' refresh Max Travel setting
  cmdString = "FL000000"
  cmdString = appendChecksum(cmdString)
  Call sendCommand(cmdString, True)

  ' refresh Motor config
  cmdString = "FC000000"
  cmdString = appendChecksum(cmdString)
  Call sendCommand(cmdString, True)

  ' back to gray
  cmdRefresh.BackColor = LightGray
    
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("Main - cmdRefresh")
  Resume Next
End Sub

Private Sub Command1_Click()
  ' test bogus command rejection
  Call sendCommand("FF123456?")
End Sub

Private Sub cmdTemp_Click()

  ' don't allow changing the step size while in rel spec mode
  frmConfig!txtStepSize.Enabled = False

  frmTemp.Show
  
  If Not gblShowingTraining Then
    frmTrain.Hide
    frmTemp!mnuViewTraining.Checked = False
  Else
    frmTrain.Show
    frmTemp!mnuViewTraining.Checked = True
  End If

End Sub

Private Sub config_Click()
 frmConfig.Show
End Sub

Private Sub deltaAmount_KeyPress(KeyAscii As Integer)
  
  Dim notused As Integer, passed As Boolean
  
  If KeyAscii = Asc(vbCrLf) Then
    KeyAscii = 0 ' so it doesn't beep
    
    Call validateInt(deltaAmount.Text, notused, "Delta Amount", 1, 9999, _
                                                                  frmMain!deltaAmount, passed)
    If passed Then
      deltaButton(1).SetFocus
    End If
    
  End If
  
End Sub

Public Sub deltaButton_Click(Index As Integer)
  Dim noResend As Boolean, notused As Integer, passed As Boolean
  
  On Error GoTo errHandler
 
  Call validateInt(deltaAmount.Text, notused, "Delta Amount", 1, 9999, _
                                                                  frmMain!deltaAmount, passed)
  If passed Then
    
      Select Case Index
        Case 0           ' In
          ' Don't allow movement below zero
          If absolutePos - val(deltaAmount.Text) < 0 Then
            Call showMsg("This would cause movement below zero.", "Invalid Request", 2)
            cmdString = ""
            
          Else
            cmdString = "FI"
          End If
          
        Case 1           ' Out
          ' Don't allow movement past max travel
          If absolutePos + val(deltaAmount.Text) > maxTravelSetting Then
            Call showMsg("This would cause movement past the current Maximum Travel setting.", _
                                                                    "Invalid Request", 2)
            cmdString = ""
            
          Else
            cmdString = "FO"
          End If
          
      End Select
      
      cmdActive = True
      
      cmdString = cmdString & Format(currentDelta, "000000")
      cmdString = appendChecksum(cmdString)
      
      noResend = True
      Call sendCommand(cmdString, noResend)
      
    End If
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("Main - deltaButton_Click")
  Resume Next
End Sub

Private Sub Form_Load()

  Dim i As Integer
  Dim currentCom As String
  Dim frmWork As Form, windowStatus As String
  Dim sbuffer As String, iResult As Integer, iRetVal As Integer
  Dim sDir As String, Ret As Long
  Dim endChar As String
  
  On Error GoTo iniError
    
  debugging = True
  
  gblPrevAbsRelMode = 2       '   always starts in Relative Specified mode
  
  gblMotorConfigCommand = ""  '   motor config command is saved here if port is closed
  
  sDir = Space(255)
  Ret = GetWindowsDirectory(sDir, 255) ' GetSystemDirectory(sDir, 255)
  sDir = left$(sDir, Ret)

 'glIniFile = sDir & "\RoboFocus.ini"
 
  ' moved to app.path to allow multiple instances (jab)
  endChar = Right(App.EXEName, 1)
  
  If (endChar >= "0" And endChar <= "9") Then
      glIniFile = App.Path & "\RoboFocus" & endChar & ".ini"
      Caption = Caption & " " & endChar
  Else
      glIniFile = App.Path & "\RoboFocus.ini"
  End If
    
 'MsgBox glIniFile

  dfltWin(1) = "0 0 4560 2085"     ' frmMain
  dfltWin(2) = "0 2000 5235 1965"  ' frmComm
  dfltWin(3) = "0 2000 6210 5850"  ' frmConfig
  dfltWin(4) = "0 2000 5460 3105"  ' frmShow
  dfltWin(5) = "0 2000 5055 5370"  ' frmHelp
  dfltWin(6) = "0 2000 4065 6000"  ' frmTemp
  dfltWin(7) = "0 2000 4215 5910"  ' frmTrain
  dfltWin(8) = "0 2000 8100 5400"  ' frmSetupList
  dfltWin(9) = "0 2000 4185 1830"  ' frmCal
  dfltWin(10) = "0 2000 4365 5670" ' frmAbsRel
  
  Open glIniFile For Input As #1  ' make sure it's there; if it isn't, the error-handling
  Close #1                        ' routine will create a default ini file
  
  Call setFormPositionIni(Me, "0 0 4560 2085") ' old h 1980 (jab)
  Me.Show

  ' set revision date
  Dim fs, F, s
  Set fs = CreateObject("Scripting.FileSystemObject")
  Set F = fs.GetFile(App.Path & "\" & App.EXEName & ".exe")
  gblDateLastModified = F.DateLastModified
  
  ' refresh from firmware values
  currentDelta = IniGetData("Config", "currentDelta", 1, glIniFile)
  deltaAmount.Text = currentDelta

  For i = 1 To 4
    currentUserButtonText(i) = IniGetData("User Buttons", Str(i), "(unnamed)", glIniFile)
    userButton(i).Caption = currentUserButtonText(i)
    frmConfig!buttonCaption(i).Text = currentUserButtonText(i)
  Next i

' set caption of "Open Port" button
  currentCom = IniGetData("Comm", "commport", 1, glIniFile)
  openClosePort.Caption = "COM " & currentCom

  gblTempConvBeta = CDbl(IniGetData("Temp Comp", "TempConvBeta", "0", glIniFile))
  gblSlopeManual = CDbl(IniGetData("Temp Comp", "Manual Slope", "0", glIniFile))
  
  ' check whether to open the comm port automatically when program starts
  If IniGetData("Config", "commAutoOpen", 0, glIniFile) Then
    Call frmComm.openPort_Click
  End If

  ' check whether "Keep On Top" is set
  If IniGetData("Config", "KeepOnTop", 0, glIniFile) Then
    frmConfig.chkKeepOnTop.Value = 1
  End If
  
  gblDutyCycle = IniGetData("Config", "Duty Cycle", 0, glIniFile)
  gblDelayPerStep = IniGetData("Config", "Delay per Step", 1, glIniFile)
  gblStepSize = IniGetData("Config", "Step Size", 1, glIniFile)
  
  g_uPerStep = CDbl(IniGetData("Config", "uPerStep", 0, glIniFile))   ' jab
  
  ' added for version 2.14 (replaced 65535 with totalSteps)
  totalSteps = 999999
  
' tmrShowTemp.interval = IniGetData("Temp Comp", "Interval", 3, glIniFile) * 1000

  On Error GoTo 0
  
Exit Sub

iniError:
  Dim newIni As String
  
  If Err = 48 Or Err = 53 Then
  
    ' ini file was not found; create a new one
    newIni = "[Config]|" & _
              "absolutePos = 1000|" & _
              "currentDelta = 50|" & _
              "backlashDir = 2|" & _
              "backlashSteps = 20|" & _
              "maxTravel = 5000|" & _
              "endOfTravelIn = 0|" & _
              "endOfTravelOut = 0|" & _
              "commAutoOpen = 0|" & _
              "KeepOnTop = 0|" & _
              "Step Size = 2|" & _
              "Delay per Step = 5|" & _
              "Duty Cycle = 50|"

     newIni = newIni & _
              "|[User Buttons]|" & _
              "1=User Button 1|" & _
              "2=User Button 2|" & _
              "3=User Button 3|" & _
              "4=User Button 4|" & _
              "|[Comm]|" & _
              "baudRate=9600|" & _
              "parity=n|" & _
              "dataBits=8|" & _
              "stopBits=1|" & _
              "commport=1|" & _
              "|[Temp Comp]|" & _
              "Interval = 10|" & _
              "Dead Zone = 1|" & _
              "File Name=|" & _
              "Path Name=\|"

     newIni = newIni & _
              "|[Window Layout]|" & _
              "frmMain=" & dfltWin(1) & "|" & _
              "frmComm=" & dfltWin(2) & "|" & _
              "frmConfig=" & dfltWin(3) & "|" & _
              "frmShow=" & dfltWin(4) & "|" & _
              "frmHelp=" & dfltWin(5) & "|" & _
              "frmTemp=" & dfltWin(6) & "|" & _
              "frmTrain=" & dfltWin(7) & "|" & _
              "frmSetupList=" & dfltWin(8) & "|" & _
              "frmCal=" & dfltWin(9) & "|" & _
              "frmAbsRel=" & dfltWin(10)
    
    Debug.Print "newIni = " & newIni
    
    newIni = Replace(newIni, "|", vbCrLf)
    
    Open glIniFile For Output As #1
    Print #1, newIni

  Else
    Call showMsg("frmMain Load" & vbCrLf & "Error #" & Str(Err) & vbCrLf & Err.Description, "", 5)
  End If
    
  On Error GoTo 0
  Resume Next
  
End Sub

Public Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
  
  Dim Msg
  
  On Error GoTo errHandler
  
  If gblOpeningCom = True Then
    Call showMsg("Can't exit during Comm Port access.", "", 1)
    Cancel = 1
  Else
  
    If autoCorrect = True Then
      Call showMsg("Stop Auto-correct before exiting.", "", 2)
      Cancel = 1
    Else
    
      If frmComm!commControl.PortOpen = True Then
        Call frmComm.closePort_Click
      End If
      
      Call iniPutData("Config", "currentDelta", deltaAmount.Text, glIniFile)
    
      Call UnloadAllForms
      Call UnloadAllForms  ' to unload the ones that were reloaded during the first call
  
    End If
    
  End If
  
  On Error GoTo 0
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmMain FormQueryUnload")
  Resume Next
End Sub


Public Sub Form_Unload(Cancel As Integer)
  
  On Error GoTo errHandler
  
  If autoCorrect = True Then
    Cancel = 1
  Else
  
    If Forms.Count = 1 Then
      Unload Me
    Else
      Cancel = 0
    End If
    
  End If
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmMain Form_Unload: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub openClosePort_Click()
  
  On Error GoTo errHandler
 
  If frmComm.commControl.PortOpen = False Then
    Call frmComm.openPort_Click
  Else
    Call frmComm.closePort_Click
  End If
  
  If frmComm.commControl.PortOpen = False Then
    ' didn't open; leave focus on Open button
    openClosePort.SetFocus
  End If
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmMain openClosePortClick")
  Resume Next
End Sub

Public Sub StopButton_Click()

  On Error GoTo errHandler
  
  If frmComm!commControl.PortOpen Then
  
    cmdString = "F " & String(200, "V")
    cmdString = appendChecksum(cmdString)
    
    Call sendCommand(cmdString, True)
    
    Call cmdRefresh_Click
    
  End If
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmMain stopButton_Click")
  Resume Next
End Sub

Private Sub captionTimer_Timer()   ' refresh the button captions
  Dim i As Integer

  On Error GoTo errHandler
  
  ' delta field
  If Len(deltaAmount.Text) > 4 Then
    deltaAmount.Text = left(deltaAmount.Text, 4)
  End If
  
  currentDelta = val(deltaAmount.Text)
  
  ' user (power) buttons
  If frmConfig.Visible Then
    For i = 1 To 4
      userButton(i).Caption = frmConfig!buttonCaption(i).Text
      Call iniPutData("User Buttons", Str(i), frmConfig!buttonCaption(i).Text, glIniFile)
    Next i
  End If
    
  Call parseSerialData
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmMain captionTimer_Timer")
  Resume Next
End Sub

Private Sub tmrShowTemp_Timer()
  
  Static elapsedTime As Integer
  Dim intervalSeconds As Long, start As Long
  
  If autoRate <> 0 Then
    elapsedTime = elapsedTime + 1
  ' intervalSeconds = frmTemp!txtAutoRate.Text * 60
    intervalSeconds = autoRate * 60
    
    If elapsedTime >= intervalSeconds Then
    
      elapsedTime = 0
      If frmComm!commControl.PortOpen And Not cmdActive Then
      
  '      frmTemp.cmdGetTemp_Click
  
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
        
  '      If gblTemp <> "" Then
  '        txttemp.text =
  '      End If
  
      End If
    End If
  End If
  
End Sub

Public Sub userButton_Click(Index As Integer)
Dim i As Integer

  On Error GoTo errHandler
  
  ' cycle the state of the button (swap 1 for 0 and vice versa)
  currentUBState(Index) = IIf((currentUBState(Index)), 0, -1)

  ' build the command
  cmdString = "FP" & "00"  '  pad to six chars
  For i = 1 To 4
    ' ubState is 0 or -1, cmdString is 1 or 2
    cmdString = cmdString & Format(Abs(currentUBState(i)) + 1)
  Next i

  cmdString = appendChecksum(cmdString)

  Call sendCommand(cmdString, True)

  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmMain userButton_Click")
  Resume Next
End Sub

