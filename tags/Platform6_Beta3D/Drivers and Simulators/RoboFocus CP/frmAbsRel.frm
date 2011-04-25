VERSION 5.00
Begin VB.Form frmAbsRel 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Mode"
   ClientHeight    =   5265
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4245
   Icon            =   "frmAbsRel.frx":0000
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5265
   ScaleWidth      =   4245
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txtManualStepSize 
      Alignment       =   1  'Right Justify
      Height          =   285
      Left            =   1260
      TabIndex        =   7
      Top             =   4830
      Width           =   885
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "Cancel"
      Height          =   345
      Left            =   2460
      TabIndex        =   5
      Top             =   4830
      Width           =   765
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   345
      Left            =   3330
      TabIndex        =   1
      Top             =   4830
      Width           =   765
   End
   Begin VB.TextBox txtManualSlope 
      Alignment       =   1  'Right Justify
      Height          =   285
      Left            =   1260
      TabIndex        =   0
      Top             =   4380
      Width           =   885
   End
   Begin VB.Label lblAbsPosition 
      Caption         =   "At the current temperature reading (! units), the Focuser will move to position ? when you click the ""Auto"" or ""Manual"" buttons. "
      Height          =   645
      Left            =   210
      TabIndex        =   6
      Top             =   3420
      Width           =   3855
   End
   Begin VB.Label Label2 
      Caption         =   "Step Size"
      Height          =   225
      Left            =   270
      TabIndex        =   8
      Top             =   4860
      Width           =   885
   End
   Begin VB.Label lblInstruct 
      Caption         =   "Instructions"
      Height          =   1965
      Left            =   210
      TabIndex        =   4
      Top             =   2040
      Width           =   3855
   End
   Begin VB.Label Label1 
      Caption         =   "Slope"
      Height          =   225
      Left            =   270
      TabIndex        =   3
      Top             =   4410
      Width           =   525
   End
   Begin VB.Label lblDesc 
      Caption         =   "Description"
      Height          =   1635
      Left            =   210
      TabIndex        =   2
      Top             =   120
      Width           =   3855
   End
End
Attribute VB_Name = "frmAbsRel"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Dim baselinePos As Integer, baselineTemp As Integer  ' use in rel mode
Dim forceRunLine As Boolean     ' set true to force entry of a line into the run grid
Dim dataline As String

Private Sub cmdCancel_Click()
  
  DoEvents
  
  If gblAbsRelMode <> gblPrevAbsRelMode Then
    gblAbsRelMode = gblPrevAbsRelMode
    gblAbsRelCancel = True
  End If
  
  Unload Me

  frmTemp!optAbsRel(gblAbsRelMode) = True

End Sub

Private Sub cmdOK_Click()
  
  Dim start
  Dim modeText(0 To 2) As String
  Dim slopeManualValue As Single
  
  modeText(0) = "Absolute"
  modeText(1) = "Relative Computed"
  modeText(2) = "Relative Specified"
  
  gblPrevAbsRelMode = gblAbsRelMode
  gblAbsRelCancel = False
  
  If gblAbsRelMode = 2 Then
    ' relative specified
    
    ' convert the entered slope to absolute units
    slopeManualValue = convertSlopeToRaw(txtManualSlope.Text)
    If slopeManualValue < -100 Or slopeManualValue > 100 Then
      MsgBox "Valid slope range is -100 to +100 raw units."
      Exit Sub
    End If
    
    gblSlopeManual = slopeManualValue
     
    ' and save it in the ini file
    Call iniPutData("Temp Comp", "Manual Slope", CStr(gblSlopeManual), glIniFile)

    gblStepSize = txtManualStepSize.Text
    ' save the global step size
    Call iniPutData("Config", "Step Size", CStr(gblStepSize), glIniFile)
    ' and the temp comp step size, which can be different
    Call iniPutData("Temp Comp", "Step Size", CStr(gblStepSize), glIniFile)
    
    Call setMotorConfigParams
    
    ' set baseline position and temperature
    baselinePos = absolutePos
    baselineTemp = gblTemp
      
    ' don't allow changing the step size while in rel spec mode
    frmConfig!txtStepSize.Enabled = False
    
  Else
    frmConfig!txtStepSize.Enabled = True
  End If
    
  If gblAbsRelMode = 0 Then
    ' absolute mode
    
    If gblShowingRelative Then
      frmMain.cmdAbsRel_Click
    End If
    
    forceRunLine = True
    
    frmTemp!txtIntercept.Visible = True
'    Label4.Visible = True   '  the plus sign
'    Label10.Visible = True  '  "Y-Intercept"
    
    doCompute  ' loads correct intercept and slope for absolute mode
    
    frmTemp!txtIntercept.Text = gblIntercept
    frmTemp!txtSlope.Text = displaySlope(gblSlope)
    
  Else
  
    ' relative mode
    
    ' get baseline position and temperature
    baselinePos = absolutePos
    
    frmTemp!txtTemp.Text = ""
    gblTemp = ""
    
    forceRunLine = False
  
    If frmComm!commControl.PortOpen Then
      ' request temperature reading
      cmdString = "FT000000"
      cmdString = appendChecksum(cmdString)
      Call sendCommand(cmdString, True)
      
      ' wait until the temp data appears
      start = Timer
      While Timer < start + 0.2 And gblTemp = ""
        DoEvents
      Wend
    End If
    
    If gblTemp <> "" Then
    
      frmTemp!txtTemp.Text = displayTemp(gblTemp)
     'frmTemp!txtTemp.Text = gblTemp
      
      ' set baseline position and temperature
      baselinePos = absolutePos
      baselineTemp = gblTemp
      
      ' set main window to relative and position to zero
      If Not gblShowingRelative Then
        frmMain.cmdAbsRel_Click
      End If
      
      frmMain!actualPos.ForeColor = Red
      frmMain!actualPos.Text = "+0"
    
    End If
  
    frmTemp!txtIntercept.Visible = False
    frmTemp!Label4.Visible = False    '  the plus sign
    frmTemp!Label10.Visible = False   '  "Y-Intercept" label


    doCompute  ' loads correct slope
    
    If gblAbsRelMode = 1 Then
      frmTemp!txtSlope.Text = displaySlope(gblSlope)
    Else
      frmTemp!txtSlope.Text = displaySlope(gblSlopeManual)
    End If
  
  End If

  ' clear the previous data
  frmTemp.mnuClearMH_Click
  
  ' display in the Run Grid
  With frmTemp!rData
     
    ' add a blank data row
    .Rows = gblNextRowRun + 1

    ' and fill it
    .Row = gblNextRowRun
    
    .Col = 0
    .CellForeColor = Blue
    .Text = Format(Date, "mm/dd/yy")
    
    .Col = 1
    .CellForeColor = Blue
    .Text = Format(Time, "hh:mm:ss")
    
    .Col = 2
    runTempArray(gblNextRowRun) = gblTemp
    .CellForeColor = Blue
    .Text = displayTemp(gblTemp)
    
    .Col = 3
    .CellForeColor = Blue
    .Text = absolutePos
  
  End With
  
  gblNextRowRun = gblNextRowRun + 1

  ' add to log file
  dataline = Format(Date, "mm/dd/yy") & " " & _
             Format(Time, "hh:mm:ss") & " " & _
             Right("     " & tempEditVal, 5) & " " & _
             Right("     " & displayTemp(CStr(tempEditVal)), 5) & " " & _
             absolutePos & " - Start " & _
             modeText(gblAbsRelMode)
 
  Open App.Path & "\RFTrack.log" For Append As #1
  Print #1, dataline
  Close #1

''''''''''''''
  Unload Me

End Sub

Private Sub Form_Load()
  Dim oarIndex As Integer
  Dim arCaption(0 To 2) As String, descText(0 To 2) As String, instText(0 To 2) As String
  Dim cmdString As String, newPosition As Integer, start
  On Error GoTo errHandler
  
  If gblAbsRelCancel = True Then
    gblAbsRelCancel = False
    Exit Sub
  End If
  
  Call setFormPositionIni(Me, "0 2000 4365 5670")
  
  arCaption(0) = "Absolute"
  arCaption(1) = "Relative - Computed Coefficients"
  arCaption(2) = "Relative - Specified Coefficients"
  
  descText(0) = "Absolute Mode" & vbCrLf & vbCrLf & _
            "In this mode RoboFocus will compute the temperature coefficients from the dataset (new or loaded file). " & _
            "Once you hit OK, Robofocus will use these coefficients to calculate the absolute position of the focuser based on the data. Note that use of incorrect data may cause the focuser to move in error."
  descText(1) = "Relative Mode - Computed" & vbCrLf & vbCrLf & _
            "In this mode RoboFocus will compute the temperature coefficient from the dataset (new or loaded file). " & _
            "Once you hit OK, RoboFocus will use this coefficient to calculate future changes to make in the focuser position relative to the starting position."
  descText(2) = "Relative Mode - Specified" & vbCrLf & vbCrLf & _
            "In this mode you manually enter the temperature coefficient to be used. " & _
            "Once you hit OK, RoboFocus will use this coefficient to calculate future changes to make in the focuser position relative to the starting position."

  instText(0) = "1) Click OK to begin session, or Cancel to escape." & vbCrLf & _
                "2) Select Manual or Auto to invoke the focuser temperature corrections."
  instText(1) = "1) Focus the scope." & vbCrLf & _
                "2) Click OK to register the starting position and begin session, or Cancel to escape." & vbCrLf & _
                "3) Select Manual or Auto to invoke the focuser temperature corrections."
  instText(2) = "1) Focus the scope." & vbCrLf & _
                "2) Enter the desired slope below. Slope will equal the desired position change per temperature degree in the currently selected temperature units (e.g., -2.3 steps per degree)." & vbCrLf & _
                "3) Click OK to register the starting position and begin session, or Cancel to escape." & vbCrLf & _
                "4) Select Manual or Auto to invoke the focuser temperature corrections."
  
  If frmTemp!optAbsRel(0).Value = True Then
    oarIndex = 0
  End If
  
  If frmTemp!optAbsRel(1).Value = True Then
    oarIndex = 1
  End If
  
  If frmTemp!optAbsRel(2).Value = True Then
    oarIndex = 2
  End If
  
  Caption = arCaption(oarIndex)
  lblDesc.Caption = descText(oarIndex)
  lblInstruct.Caption = instText(oarIndex)
  
  If oarIndex = 0 Then
    ' absolute - show position that focuser will move to
    
    lblAbsPosition.Visible = True
    
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
      newPosition = gblIntercept + gblSlope * gblTemp
      lblAbsPosition.Caption = "At the current temperature reading (" & gblTemp & " units), " & _
                            "the Focuser will move to position " & newPosition & " when you " & _
                            "click the ""Auto"" or ""Manual"" buttons."
    Else
      
    End If
    
  Else
    lblAbsPosition.Visible = False
  End If
  
  If oarIndex = 2 Then
    ' rel specified - show slope entry box
    
    txtManualSlope.Enabled = True
    txtManualSlope.Text = displaySlope(gblSlopeManual)
    Label1.Visible = True
    
    txtManualStepSize.Visible = True
    txtManualStepSize.Text = IniGetData("Temp Comp", "Step Size", 1, glIniFile)
    Label2.Visible = True
  
  Else
    
    txtManualSlope.Enabled = False
    txtManualSlope.Text = displaySlope(gblSlope)
    
    txtManualStepSize.Visible = False
    
    Label1.Visible = True
    Label2.Visible = False
  
  End If
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmAbsRel Form_Load: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub Form_Unload(Cancel As Integer)
    
  On Error GoTo errHandler
  
  ' if we're in "relative specified" mode, save the entered value
  If txtManualSlope.Visible = True Then
  
    If txtManualSlope.Text = "" Then
     
      frmTemp!cmdManual.Enabled = False
      frmTemp!cmdAuto.Enabled = False
    
      gblSlopeManual = 0
      
    Else
     
      If frmComm!commControl.PortOpen Then
        frmTemp!cmdManual.Enabled = True
        frmTemp!cmdAuto.Enabled = True
'       frmTemp!optAbsRel(0).Enabled = True
'       frmTemp!optAbsRel(1).Enabled = True
        frmTemp!optAbsRel(2).Enabled = True
      End If
      
    ' gblSlopeManual = txtManualSlope.Text
    ' frmTemp!txtSlope.Text = Format(txtManualSlope.Text, "##0.000")
      
    End If
  
  End If
  
  Call saveFormPosition(Me)

Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmAbsRel Form_Unload: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub txtManualslope_KeyPress(KeyAscii As Integer)
  
  On Error GoTo errHandler
  
  If KeyAscii = Asc(vbCrLf) Then
    
    KeyAscii = 0 ' so it doesn't beep
    
    If txtManualSlope.Text = "" Then
     
      frmTemp!cmdManual.Enabled = False
      frmTemp!cmdAuto.Enabled = False
    
      gblSlopeManual = 0
      
    Else
     
      If frmComm!commControl.PortOpen Then
        frmTemp!cmdManual.Enabled = True
        frmTemp!cmdAuto.Enabled = True
        frmTemp!optAbsRel(0).Enabled = True
        frmTemp!optAbsRel(1).Enabled = True
        frmTemp!optAbsRel(2).Enabled = True
      End If
      
      gblSlopeManual = txtManualSlope.Text
      frmTemp!txtSlope.Text = txtManualSlope.Text
      
    End If
  
  End If
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmAbsRel txtManualSlope_KeyPress: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

