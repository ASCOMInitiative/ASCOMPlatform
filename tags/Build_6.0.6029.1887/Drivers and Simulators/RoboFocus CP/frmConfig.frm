VERSION 5.00
Begin VB.Form frmConfig 
   Caption         =   "RoboFocus Configuration"
   ClientHeight    =   5865
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   6060
   FillColor       =   &H80000012&
   Icon            =   "frmConfig.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   6369.835
   ScaleMode       =   0  'User
   ScaleWidth      =   6060
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox uPerStep 
      Alignment       =   1  'Right Justify
      Height          =   285
      Left            =   4208
      TabIndex        =   10
      Top             =   4305
      Width           =   765
   End
   Begin VB.CheckBox chkWarnBeforeLoad 
      Caption         =   "Warn Before Loading Temp. Comp. File"
      Height          =   285
      Left            =   2880
      TabIndex        =   22
      Top             =   5520
      Width           =   3135
   End
   Begin VB.Frame showDataFrame 
      Caption         =   " Show Data "
      Height          =   705
      Left            =   3090
      TabIndex        =   42
      Top             =   4680
      Width           =   2895
      Begin VB.CommandButton cmdShowData 
         Caption         =   "Show Data Traffic"
         Height          =   315
         Left            =   540
         TabIndex        =   21
         Top             =   270
         Width           =   1905
      End
   End
   Begin VB.Frame setupFrame 
      Caption         =   " Focus Setup "
      Height          =   705
      Left            =   120
      TabIndex        =   41
      Top             =   4680
      Width           =   2895
      Begin VB.CommandButton cmdFocusSetup 
         Caption         =   "Display Focus Setup List"
         Height          =   315
         Left            =   540
         TabIndex        =   11
         Top             =   270
         Width           =   1905
      End
   End
   Begin VB.Frame motorConfig 
      Caption         =   " Motor Configuration "
      Height          =   1665
      Left            =   90
      TabIndex        =   34
      Top             =   2520
      Width           =   2895
      Begin VB.TextBox txtStepSize 
         Height          =   255
         Left            =   2220
         TabIndex        =   8
         Top             =   930
         Width           =   525
      End
      Begin VB.TextBox txtDelayPerStep 
         Height          =   255
         Left            =   2220
         TabIndex        =   7
         Top             =   600
         Width           =   525
      End
      Begin VB.TextBox txtDutyCycle 
         Height          =   255
         Left            =   2220
         TabIndex        =   6
         Top             =   270
         Width           =   525
      End
      Begin VB.CommandButton cmdGetMotorConfig 
         Caption         =   "Get Current Settings"
         Height          =   315
         Left            =   540
         TabIndex        =   9
         Top             =   1260
         Width           =   1905
      End
      Begin VB.Label Label10 
         Caption         =   "0-100%"
         Height          =   225
         Left            =   1560
         TabIndex        =   40
         Top             =   300
         Width           =   585
      End
      Begin VB.Label Label12 
         Caption         =   "1-255"
         Height          =   225
         Left            =   1560
         TabIndex        =   39
         Top             =   960
         Width           =   585
      End
      Begin VB.Label Label11 
         Caption         =   "1-20"
         Height          =   225
         Left            =   1560
         TabIndex        =   38
         Top             =   630
         Width           =   585
      End
      Begin VB.Label Label9 
         Caption         =   "StepSize"
         Height          =   225
         Left            =   300
         TabIndex        =   37
         Top             =   960
         Width           =   1215
      End
      Begin VB.Label Label8 
         Caption         =   "MicrostepPause"
         Height          =   225
         Left            =   300
         TabIndex        =   36
         Top             =   630
         Width           =   1185
      End
      Begin VB.Label Label7 
         Caption         =   "Duty Cycle"
         Height          =   225
         Left            =   300
         TabIndex        =   35
         Top             =   300
         Width           =   1125
      End
   End
   Begin VB.CheckBox chkKeepOnTop 
      Caption         =   "Keep Main Form on Top"
      Height          =   285
      Left            =   90
      TabIndex        =   12
      Top             =   5520
      Width           =   2325
   End
   Begin VB.Frame buttonFrame 
      Caption         =   "User Button Captions"
      Height          =   1665
      Left            =   3090
      TabIndex        =   29
      Top             =   2520
      Width           =   2895
      Begin VB.TextBox buttonCaption 
         Height          =   255
         Index           =   1
         Left            =   960
         TabIndex        =   17
         Top             =   270
         Width           =   1200
      End
      Begin VB.TextBox buttonCaption 
         Height          =   255
         Index           =   2
         Left            =   960
         TabIndex        =   18
         Top             =   600
         Width           =   1200
      End
      Begin VB.TextBox buttonCaption 
         Height          =   255
         Index           =   3
         Left            =   960
         TabIndex        =   19
         Top             =   960
         Width           =   1200
      End
      Begin VB.TextBox buttonCaption 
         Height          =   255
         Index           =   4
         Left            =   960
         TabIndex        =   20
         Top             =   1260
         Width           =   1200
      End
      Begin VB.Label Label1 
         Caption         =   "1"
         Height          =   285
         Left            =   660
         TabIndex        =   33
         Top             =   300
         Width           =   165
      End
      Begin VB.Label Label4 
         Caption         =   "2"
         Height          =   285
         Left            =   660
         TabIndex        =   32
         Top             =   630
         Width           =   165
      End
      Begin VB.Label Label5 
         Caption         =   "3"
         Height          =   285
         Left            =   660
         TabIndex        =   31
         Top             =   960
         Width           =   165
      End
      Begin VB.Label Label6 
         Caption         =   "4"
         Height          =   285
         Left            =   660
         TabIndex        =   30
         Top             =   1290
         Width           =   165
      End
   End
   Begin VB.Frame confComm 
      Caption         =   " Communications"
      Height          =   1005
      Left            =   90
      TabIndex        =   27
      Top             =   90
      Width           =   2895
      Begin VB.CommandButton commSettings 
         Caption         =   "Adjust Comm Settings"
         Height          =   315
         Left            =   540
         MaskColor       =   &H8000000F&
         TabIndex        =   1
         Top             =   270
         Width           =   1905
      End
      Begin VB.CheckBox commAutoOpen 
         Caption         =   "Open Comm Port on Startup"
         Height          =   285
         Left            =   450
         TabIndex        =   2
         Top             =   630
         Width           =   2295
      End
   End
   Begin VB.Frame getSetTravel 
      Caption         =   " Get or Set Maximum Travel "
      Height          =   1215
      Left            =   3090
      TabIndex        =   25
      Top             =   1200
      Width           =   2895
      Begin VB.CommandButton displayMaxTravel 
         Caption         =   "Get Current Max. Travel"
         Enabled         =   0   'False
         Height          =   315
         Left            =   540
         MaskColor       =   &H8000000F&
         TabIndex        =   16
         Top             =   750
         Width           =   1905
      End
      Begin VB.TextBox maxTravel 
         Alignment       =   1  'Right Justify
         Enabled         =   0   'False
         Height          =   285
         Left            =   1860
         TabIndex        =   15
         Top             =   300
         Width           =   765
      End
      Begin VB.Label Label3 
         Caption         =   "Enter Max. Travel"
         Height          =   255
         Left            =   450
         TabIndex        =   26
         Top             =   360
         Width           =   1305
      End
   End
   Begin VB.Frame getSetPosition 
      Caption         =   " Get or Set Position "
      Height          =   1005
      Left            =   3090
      TabIndex        =   24
      Top             =   90
      Width           =   2895
      Begin VB.TextBox txtPosition 
         Alignment       =   1  'Right Justify
         Enabled         =   0   'False
         Height          =   285
         Left            =   1860
         TabIndex        =   13
         Top             =   240
         Width           =   765
      End
      Begin VB.CommandButton displayPos 
         Caption         =   "Get Current Position"
         Enabled         =   0   'False
         Height          =   315
         Left            =   540
         MaskColor       =   &H8000000F&
         TabIndex        =   14
         Top             =   570
         Width           =   1905
      End
      Begin VB.Label Label2 
         Caption         =   "Enter Position"
         Height          =   255
         Left            =   390
         TabIndex        =   45
         Top             =   270
         Width           =   1095
      End
   End
   Begin VB.Frame Backlash 
      Caption         =   " Backlash Compensation "
      Height          =   1215
      Left            =   90
      TabIndex        =   0
      Top             =   1200
      Width           =   2895
      Begin VB.CommandButton displayBacklash 
         Caption         =   "Get Current Settings"
         Enabled         =   0   'False
         Height          =   315
         Left            =   1050
         MaskColor       =   &H8000000F&
         TabIndex        =   5
         Top             =   720
         Width           =   1665
      End
      Begin VB.OptionButton backlashdir 
         Caption         =   "Off"
         Enabled         =   0   'False
         Height          =   315
         Index           =   1
         Left            =   930
         TabIndex        =   3
         Top             =   300
         Visible         =   0   'False
         Width           =   495
      End
      Begin VB.TextBox backlashSteps 
         Enabled         =   0   'False
         Height          =   285
         Left            =   2190
         TabIndex        =   4
         Top             =   300
         Width           =   525
      End
      Begin VB.Frame Frame1 
         Caption         =   "Final Dir"
         Height          =   855
         Left            =   120
         TabIndex        =   28
         Top             =   240
         Width           =   795
         Begin VB.OptionButton backlashdir 
            Caption         =   "Out"
            Height          =   315
            Index           =   3
            Left            =   90
            TabIndex        =   44
            Top             =   240
            Width           =   585
         End
         Begin VB.OptionButton backlashdir 
            Caption         =   "In"
            Enabled         =   0   'False
            Height          =   315
            Index           =   2
            Left            =   90
            TabIndex        =   43
            Top             =   480
            Width           =   585
         End
      End
      Begin VB.Label steps 
         Caption         =   "Steps"
         Height          =   255
         Left            =   1560
         TabIndex        =   23
         Top             =   330
         Width           =   555
      End
   End
   Begin VB.Label Label13 
      Caption         =   "Enter Microns per Position Unit (Optional)"
      Height          =   255
      Left            =   1088
      TabIndex        =   46
      Top             =   4335
      Width           =   3015
   End
End
Attribute VB_Name = "frmConfig"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private p_bUnloading As Boolean     ' manage lose focus race (jab)

Public Sub backlashDir_Click(Index As Integer)
  
  On Error GoTo errHandler
  
  backlashDirSetting = Index
  Call iniPutData("Config", "BacklashDir", Str(backlashDirSetting), glIniFile)
  If backlashDirSetting = 1 Then  ' compensation is disabled
    backlashSteps.Visible = False
    steps.Visible = False
    backlashSteps.Text = 0
    Call iniPutData("Config", "BacklashSteps", backlashSteps.Text, glIniFile)
  Else
    backlashSteps.Visible = True
    steps.Visible = True
  End If

  backlashSteps.Text = IniGetData("Config", "BacklashSteps", 20, glIniFile)
  If frmComm!commControl.PortOpen Then
    cmdString = "FB" & Format(Index) & Format(backlashSteps.Text, "00000")
    cmdString = appendChecksum(cmdString)
    Call sendCommand(cmdString, True)
  End If
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmConfig backlashDir_Click")
  Resume Next
Return

End Sub

Private Sub backlashSteps_KeyPress(KeyAscii As Integer)
  
  Dim backlashStepsVal As Long, passed As Boolean
  
  On Error GoTo errHandler
  
  If KeyAscii = Asc(vbCrLf) Then
    KeyAscii = 0            ' so it doesn't beep
  
'    backlashStepsVal = Val(backlashSteps)
'    If backlashStepsVal < 0 Or backlashStepsVal > totalSteps Then
'      MsgBox "Steps must be between 0 and totalSteps."
'      backlashSteps.SetFocus
'    Else
    
    Call validateLong(backlashSteps.Text, backlashStepsSetting, "Backlash Steps", 0, totalSteps, _
                                                            frmConfig!backlashSteps, passed)
    If passed Then
    
      Call iniPutData("Config", "Backlashsteps", backlashSteps.Text, glIniFile)
  
      If frmComm!commControl.PortOpen Then
        cmdString = "FB" & Format(backlashDirSetting) & Format(backlashSteps.Text, "00000")
        cmdString = appendChecksum(cmdString)
        Call sendCommand(cmdString, True)
      End If
     
      displayBacklash.SetFocus       ' move to the next field
    
    End If
    
  End If
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmConfig backlashSteps_KeyPress")
  Resume Next
Return

End Sub

Private Sub chkKeepOnTop_Click()

  On Error GoTo errHandler
  
  If chkKeepOnTop.Value = 1 Then
    Call SetWindowPos(frmMain.hWnd, HWND_TOPMOST, 0, 0, 0, 0, FLAGS)
  Else
    Call SetWindowPos(frmMain.hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, FLAGS)
    Me.Show
  End If
  
  Call iniPutData("Config", "KeepOnTop", chkKeepOnTop.Value, glIniFile)
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmConfig chkKeepOnTop_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub chkWarnBeforeLoad_Click()
  
  On Error GoTo errHandler
  
  Call iniPutData("Temp Comp", "Warn Before Load", chkWarnBeforeLoad.Value, glIniFile)
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmConfig chkWarnBeforeLoad_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next

End Sub

Private Sub cmdFocusSetup_Click()
  
  Dim response
  
  On Error GoTo errHandler
  
  If gblShowingRelative Then
    
    response = MsgBox("Continuing will switch the program from Relative Mode." & _
                            vbCrLf & "Click Cancel to abort.", vbOKCancel + vbQuestion, _
                            "Continue?")
    If response = vbOK Then
      frmMain.cmdAbsRel_Click
      frmSetupList.Show
      SendKeys "{ENTER}"
    End If
  
  Else
    frmSetupList.Show
  End If

Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmConfig cmdFocusSetup_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Sub cmdGetMotorConfig_Click()

  On Error GoTo errHandler
  
  cmdString = "FC000000"
  cmdString = appendChecksum(cmdString)
  Call sendCommand(cmdString, True)
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmConfig getMotorConfig_Click")
  Resume Next

End Sub

Private Sub cmdShowData_Click()
  
  On Error GoTo errHandler
  
  frmShow.Show
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmConfig cmdShowData_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub commAutoOpen_Click()
  
  On Error GoTo errHandler
  
  ' sets whether to open the comm port automatically when program starts
  Call iniPutData("Config", "commAutoOpen", commAutoOpen.Value, glIniFile)
    
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmConfig commAutoOpen_Click")
  Resume Next
Return

End Sub

Private Sub commSettings_Click()
  
  On Error GoTo errHandler
  
  frmComm.Show
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmConfig commSettings_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub displayBacklash_Click()
  
  On Error GoTo errHandler
  
  cmdString = "FB000000"
  cmdString = appendChecksum(cmdString)
  Call sendCommand(cmdString, True)
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmConfig displayBacklash_Click")
  Resume Next

End Sub

Private Sub displayMaxTravel_Click()

  On Error GoTo errHandler
  
  cmdString = "FL000000"
  cmdString = appendChecksum(cmdString)
  Call sendCommand(cmdString, True)

  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmConfig displayMaxTravel")
  Resume Next

End Sub

Private Sub displayPos_Click()

  On Error GoTo errHandler

  cmdString = "FS000000"
  cmdString = appendChecksum(cmdString)
  Call sendCommand(cmdString, True)

  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmConfig displayPos_Click")
  Resume Next

End Sub

Private Sub Form_Load()
  Dim portStatus As Boolean, i As Integer
  
  On Error GoTo errHandler
  
  p_bUnloading = False      ' not unloadng (jab)
  
  ' reload saved window position
  Call setFormPositionIni(Me, "0 2000 6180 6375")  '  7035 6345 (old w 6210 h 5850 jab)
  
  ' prevent sending commands during form load
  portStatus = frmComm!commControl.PortOpen
  If frmComm!commControl.PortOpen Then
    frmComm!commControl.PortOpen = False
  End If
  
  ' get the INI settings.
  backlashDirSetting = IniGetData("Config", "BacklashDir", 0, glIniFile)
  backlashdir(backlashDirSetting) = True
  
  backlashStepsSetting = IniGetData("Config", "BacklashSteps", 0, glIniFile)
  backlashSteps.Text = backlashStepsSetting
  If backlashDirSetting = 1 Then  ' compensation is disabled
    backlashSteps.Visible = False
  End If
  
  txtPosition.Text = absolutePos
  
  maxTravelSetting = IniGetData("Config", "maxTravel", 0, glIniFile)
  maxTravel.Text = maxTravelSetting
  
  commAutoOpen.Value = IniGetData("Config", "commAutoOpen", 0, glIniFile)
  
  For i = 1 To 4
    buttonCaption(i) = frmMain!userButton(i).Caption
  Next i

  ' microns per step (jab)
  g_uPerStep = CDbl(IniGetData("Config", "uPerStep", 0, glIniFile))
  uPerStep.Text = CStr(g_uPerStep)
  
  ' motor config items
  txtDutyCycle.Text = IniGetData("Config", "Duty Cycle", 0, glIniFile)
  txtDelayPerStep.Text = IniGetData("Config", "Delay per Step", 1, glIniFile)
  txtStepSize.Text = IniGetData("Config", "Step Size", 1, glIniFile)
  
  ' keep form on top (default: No)
  chkKeepOnTop.Value = IniGetData("Config", "KeepOnTop", 0, glIniFile)
  
  ' warn before loading TC file (default: Yes)
  chkWarnBeforeLoad.Value = IniGetData("Temp Comp", "Warn Before Load", 1, glIniFile)
  
  ' restore the port status
  DoEvents
  If portStatus Then
    frmComm!commControl.PortOpen = True
  End If
  
  ' enable or disable config controls
  Call enableButtons(portStatus)
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmConfig Form_Load")
  Resume Next
End Sub

Private Sub Form_Paint()

  Dim boxMargin As Integer, winCoords() As String
  
  ' draw a box around the edge
  ' left, top, width, height
  winCoords = Split(dfltWin(3))
  
  boxMargin = 0

  Me.FillStyle = 1
  ' new box numbers for new form size (jab)
  Me.Line (boxMargin, boxMargin)- _
                (winCoords(2) - boxMargin - 125, winCoords(3) - boxMargin + 600), Gray, B
                
End Sub

Private Sub Form_Unload(Cancel As Integer)

  ' validate uPerStep (jab)
  p_bUnloading = True   ' block focus change, else infinite loop
  If Not uPerStep_test Then
    Cancel = -1         ' don't allow unload
    Exit Sub
  End If
  p_bUnloading = False

  On Error GoTo errHandler
  
  ' force enter to save any changed fields
  If frmConfig.Visible Then
    If backlashSteps.Enabled And backlashSteps.Visible Then
      backlashSteps.SetFocus
      SendKeys "{ENTER}", True
    End If
  End If
  
  Call saveFormPosition(Me)
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmConfig Form_Unload: " & Err.Description, "", 3)
  Resume Next
  
End Sub

Private Sub maxTravel_KeyPress(KeyAscii As Integer)
  
  Dim passed As Boolean
  
  On Error GoTo errHandler
      
  If KeyAscii = Asc(vbCrLf) Then
  
    KeyAscii = 0 ' so it doesn't beep
    
    displayMaxTravel.SetFocus
    
  End If
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmConfig maxTravel_KeyPress")
  Resume Next
End Sub

Public Sub maxTravel_LostFocus()
  
  Dim passed As Boolean
  
  On Error GoTo errHandler
  
  Call validateLong(maxTravel.Text, maxTravelSetting, "Maximum Travel", _
                                          CLng(txtPosition.Text), totalSteps, _
                                          frmConfig!maxTravel, passed)
  
  If passed Then

    maxTravel.Text = CStr(Val(maxTravel.Text))
    Call iniPutData("Config", "maxTravel", maxTravel.Text, glIniFile)
    
    If frmComm!commControl.PortOpen Then
      cmdString = "FL" & Format(maxTravel.Text, "000000")
      cmdString = appendChecksum(cmdString)
      Call sendCommand(cmdString, True)
    End If
    
  End If
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmConfig maxTravel_LostFocus: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub txtPosition_KeyPress(KeyAscii As Integer)
  
  On Error GoTo errHandler
  
  If KeyAscii = Asc(vbCrLf) Then
    KeyAscii = 0 ' so it doesn't beep
    
    displayPos.SetFocus
    
  End If

  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmConfig txtPosition_KeyPress")
  Resume Next
End Sub

Public Sub txtPosition_LostFocus()
  
  Dim passed As Boolean, response
  
  On Error GoTo errHandler
  
  If CStr(absolutePos) = txtPosition.Text Then
    Exit Sub
  End If
  
  If gblShowingRelative Then
    If App.StartMode = vbSModeAutomation Then
      response = vbYes
    Else
      response = MsgBox("OK to switch to Absolute Mode?", vbYesNo + vbQuestion, "Switch to Absolute?")
    End If
    
    If response = vbYes Then
      
'      Call validateLong(txtPosition.Text, absolutePos, "Position", _
'                                              1, CLng(maxTravel.Text), txtPosition, passed)
      If passed Then
        frmMain.cmdAbsRel_Click
      Else
        response = vbNo
      End If
    
    Else
      txtPosition.Text = absolutePos
    End If
  
  Else
    response = vbYes
  End If
  
  If response = vbYes Then
    Call validateLong(txtPosition.Text, absolutePos, "Position", _
                                            1, CLng(maxTravel.Text), txtPosition, passed)
  
    If passed Then
      frmMain!actualPos.Text = txtPosition.Text
      Call iniPutData("Config", "absolutePos", txtPosition.Text, glIniFile)
    
      cmdString = "FS" & Format(txtPosition.Text, "000000")
      cmdString = appendChecksum(cmdString)
      Call sendCommand(cmdString, True)
    End If
  End If
    
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmConfig txtPosition_LostFocus: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub txtDutyCycle_KeyPress(KeyAscii As Integer)
  
  Dim passed As Boolean
  
  On Error GoTo errHandler
      
  If KeyAscii = Asc(vbCrLf) Then
  
    KeyAscii = 0 ' so it doesn't beep
    
    txtDelayPerStep.SetFocus
    
  End If
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmConfig txtDutyCycle_KeyPress")
  Resume Next
End Sub

Public Sub txtDutyCycle_LostFocus()
  
  Dim passed As Boolean
  
  On Error GoTo errHandler
  
  Call validateLong(txtDutyCycle.Text, gblDutyCycle, "Duty Cycle", _
                       0, 100, txtDutyCycle, passed)
  
  If passed Then

    txtDutyCycle.Text = CStr(Val(txtDutyCycle.Text))
    Call iniPutData("Config", "Duty Cycle", txtDutyCycle.Text, glIniFile)
    
    Call setMotorConfigParams
    
  End If
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmConfig txtDutyCycle_LostFocus: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub txtdelayperstep_KeyPress(KeyAscii As Integer)
  
  Dim passed As Boolean
  
  On Error GoTo errHandler
      
  If KeyAscii = Asc(vbCrLf) Then
  
    KeyAscii = 0 ' so it doesn't beep
    
    If gblAbsRelMode < 2 Then
      txtStepSize.SetFocus
    Else
      maxTravel.SetFocus
    End If
    
  End If
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmConfig txtDutyCycle_KeyPress")
  Resume Next
End Sub

Public Sub txtdelayperstep_LostFocus()
  
  Dim passed As Boolean
  
  On Error GoTo errHandler
  
  Call validateLong(txtDelayPerStep.Text, gblDelayPerStep, "Delay per Step", _
                       1, 20, txtDelayPerStep, passed)
  
  If passed Then

    txtDelayPerStep.Text = CStr(Val(txtDelayPerStep.Text))
    Call iniPutData("Config", "Delay per Step", txtDelayPerStep.Text, glIniFile)
    
    Call setMotorConfigParams
    
  End If
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmConfig txtDelayPerStep_LostFocus: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Sub txtstepsize_KeyPress(KeyAscii As Integer)
  
  Dim passed As Boolean
  
  On Error GoTo errHandler
      
  If KeyAscii = Asc(vbCrLf) Then
  
    KeyAscii = 0 ' so it doesn't beep
    
    If frmConfig.Visible And txtPosition.Enabled Then
      txtPosition.SetFocus
    End If

  End If
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmConfig txtDutyCycle_KeyPress")
  Resume Next
End Sub

Public Sub txtstepsize_LostFocus()
  
  Dim passed As Boolean
  
  On Error GoTo errHandler
  
  Call validateLong(txtStepSize.Text, gblStepSize, "Step Size", _
                       1, 255, txtStepSize, passed)
  
  If passed Then

    txtStepSize.Text = CStr(Val(txtStepSize.Text))
    Call iniPutData("Config", "Step size", txtStepSize.Text, glIniFile)
    
    Call setMotorConfigParams
    
  End If
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmConfig txtStepSize_LostFocus: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

'
' This variable is for ASCOM support (jab)
'
Private Sub uPerStep_LostFocus()

  Dim passed As Boolean

  If p_bUnloading Then
      p_bUnloading = False      ' don't trigger infinite loop
  Else
      passed = uPerStep_test
  End If

End Sub

'
' Validate uPerStep text field (jab)
'
Private Function uPerStep_test() As Boolean
 
  Dim passed As Boolean
  
  uPerStep_test = False
  On Error GoTo errHandler
  
  Call validateDouble(uPerStep.Text, g_uPerStep, "uPerStep", _
                                          0#, 1000000#, _
                                          frmConfig!uPerStep, passed)
  
  If passed Then
    uPerStep.Text = CStr(g_uPerStep)
    Call iniPutData("Config", "uPerStep", uPerStep.Text, glIniFile)
    uPerStep_test = True
  End If
  
Exit Function
  
errHandler:
  
  Call showMsg("Error in frmConfig uPerStep_Validate: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Function

