Attribute VB_Name = "Module1"
'=============
' Module1.bas
'=============
'
' RoboFocus startup module
'
' Initial code by RoboFocus team...
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 05-Apr-03 jab     ASCOM additions
' 14-Apr-03 jab     Added global for getTemperture so as to not terminate moves
' 01-Feb-07 jab     Added "switch" support
' 26-Sep-08 jab     Deal with multiple focusers
'---------------------------------------------------------------------

Option Explicit

Public gblDateLastModified As String
Public gblFirmwareVersion As String

'
' ASCOM'ification (jab)
'
'Public Const id As String = "RoboFocusServer.Focuser"    ' distinct from ASCOM dll
'Public Const idsw As String = "RoboFocusServer.Switch"
Public DESC As String
Public Const RegVer As String = "1.0"
Public g_Profile As DriverHelper.Profile
Public g_bRunExecutable As Boolean      ' launched via "doubleclick" ?
Public g_iFCConnections As Integer      ' reference count
Public g_iFocuserConnections As Integer ' reference count
Public g_iSwitchConnections As Integer  ' reference count
Public fc As FocusControl               ' access to original API;s
Public g_uPerStep As Double

'''''''''''''''''''''''''''
'Constants for topmost.
Public Const HWND_TOPMOST = -1
Public Const HWND_NOTOPMOST = -2
Public Const SWP_NOMOVE = &H2
Public Const SWP_NOSIZE = &H1
Public Const SWP_NOACTIVATE = &H10
Public Const SWP_SHOWWINDOW = &H40
Public Const FLAGS = SWP_NOMOVE Or SWP_NOSIZE
Public Declare Function SetWindowPos Lib "user32" _
                          (ByVal hWnd As Long, ByVal hWndInsertAfter As Long, _
                          ByVal X As Long, ByVal Y As Long, _
                          ByVal cx As Long, ByVal cy As Long, _
                          ByVal wFlags As Long) As Long

' How to use from current form:
' Call SetWindowPos(Me.hwnd, HWND_TOPMOST, 0, 0, 0, 0, FLAGS)

''''''''''''''''''''''''''''''''''''
Public Const maxRows As Integer = 1001

Public runTempArray(1 To 1000) As Integer   ' storage for temperature values in raw units
Public trainTempArray(1 To 1000, 0 To 6) As Single   ' "
Public tempEditVal As Single                         ' "
Public trainEditVal As Single                        ' "
Public calcEditVal As Single                         ' "

Public currentUserButtonText(1 To 4) As String
Public backlashDirSetting As Integer
Public backlashStepsSetting As Long, maxTravelSetting As Long
Public endOfTravelInSetting As Integer, endOfTravelOutSetting As Integer
Public totalSteps As Long   '   stepper motor steps

Public gblDutyCycle As Long
Public gblDelayPerStep As Long
Public gblStepSize As Long
Public gblFileStepSize As Long

Public absolutePos As Long
Public relPos As Long
Public startingAbsPos As Long
Public gblShowingRelative As Boolean ' Integer
Public gblShowingTraining As Boolean
Public gblShowingCal As Boolean

Public currentUBState(1 To 4) As Integer  ' the state of the user buttons

Public gblNextRow As Integer     ' next row in the data grid
Public gblNextRowRun As Integer  ' next row in the run grid

Public selectedRow As Integer

Public tableInsertFlag As Boolean  ' true for insert into edited line;
                                   ' false for append (transfer from Get Temp info)
Public Notes As String

Public autoCorrect As Boolean
Public autoRate As Single          ' interval for auto-correct

Public gblValidDataSet As Boolean  '  true when data is in the grid
  
Public unsavedData As Boolean
Public defFile As String, initDir As String

Public savedStepSize As String

Public gblTemp As String              ' raw units
Public tempDisplayUnits As Integer    ' 0 for raw units, 1 for Farenheit, 2 for Centigrade
Public tempDisplayUnitsText As String ' "raw", " F.", or " C."
Public gblLastTemp As Integer         ' most recent temperature (gbdTemp is nulled a lot, jab)

Public gblTempConvBeta As Double      ' constant for computing temperature from raw units

Public charReceived As Boolean        ' flag used in sendCommand

Public gblStopUnload As Boolean       ' flag to stop unloading if cancel was selected

Public gblIntercept As Double         ' storage for computed coefficients
Public gblSlope As Double

Public gblInterceptManual As Double   ' storage for manually-entered coefficients
Public gblSlopeManual As Double

Public gblOpeningCom As Boolean       ' flag to stop frmMain_Unload if comm open is in progress

Public waitingForComm As Boolean      ' flag for monitoring comm connection

Public gblAbsRelMode As Integer         ' keeps track of which mode we're in, for use when canceling
Public gblPrevAbsRelMode As Integer     ' the previous mode, which is saved ingblAbsRelMode when Cancel is clicked
Public gblAbsRelCancel As Boolean       ' flag indicating that cancel was clicked, don't do normal click processing

Public debugging As Boolean             ' flag for avoiding timeout when stepping thru cmds

Public gblMotorConfigCommand As String  ' motor config command is saved here if port is closed

Public loadingFlag As Boolean           ' true when we're loading a data file

Public dfltWin(1 To 10) As String       ' stores the default window locations and sizes.

Public baselinePos As Integer, baselineTemp As Integer  ' used in rel mode

Public warnResult As Boolean            ' result of the WarnBeforeLoad window
Public resetStepSize As Boolean         ' other result of the WarnBeforeLoad window

Sub Main()

    On Error GoTo errHandler
  
    ' ASCOM set up (jab)
    ERR_SOURCE = App.Title
    DESC = App.Title
    
    Set g_Profile = New DriverHelper.Profile
    g_Profile.DeviceType = "Focuser"        ' We're a Focuser driver
    g_Profile.Register App.EXEName + ".Focuser", DESC             ' Self reg (skips if already reg)
    g_Profile.DeviceType = "Switch"         ' We're a Switch driver
    g_Profile.Register App.EXEName + ".Switch", DESC           ' Self reg (skips if already reg)
    
    ' check if all we're doing is registering with ASCOM
    If InStr(Command$, "-r") >= 1 Then
        Exit Sub
    End If

    If App.StartMode = vbSModeStandalone Then
        g_bRunExecutable = True             ' launched via double click
    Else
        g_bRunExecutable = False            ' running as server only
    End If
    
    g_iFCConnections = 0                    ' zero connections currently
    g_iFocuserConnections = 0               ' zero connections currently
    g_iSwitchConnections = 0                ' zero connections currently
    
    gblLastTemp = -1                        ' no valid temperature yet (jab)
    
    frmMain.Show
    
    Exit Sub
  
errHandler:
  
    Call showMsg("Error in Module1 Main: " & Err.Description, "", 3)
    On Error GoTo 0
    Resume Next
  
End Sub

Public Sub findHardware()
  
  Dim currentCom As String, i As Integer
  
  On Error GoTo errHandler
  
  currentCom = IniGetData("Comm", "commport", "1", glIniFile)

  ' get firmware version
  firmwareResponded = False

  cmdString = "FV000000"
  cmdString = appendChecksum(cmdString)
  Call sendCommand(cmdString, True)

  If firmwareResponded Then
  
    ' refresh the current position
    frmMain!cmdRefresh.Enabled = True
    frmMain!actualPos.Enabled = True
    frmMain!cmdAbsRel.Enabled = True
    
    Call frmMain.cmdRefresh_Click
'    absolutePos = frmMain!actualPos.Text
    
    For i = 0 To 1
      frmMain!deltaButton(i).Enabled = True
    Next i
    frmMain!StopButton.Enabled = True
    
    frmMain!openClosePort.Caption = "COM " & currentCom
    frmMain!openClosePort.BackColor = Green

    frmComm!OpenPort.Enabled = False
    frmComm!closePort.Enabled = True
    
    ' enable config and setupList controls
    Call enableButtons(True)
    
    ' enable some Temp Comp buttons
    Call enableTCButtons
    
    ' point to the first row in the run grid
    gblNextRowRun = 1
    
  Else
  
    Call showMsg("No Response from RoboFocus!", "", 2)
    
    frmComm!OpenPort.Enabled = True
    frmComm!closePort.Enabled = False
    frmComm!commControl.PortOpen = False

  End If

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in Module1 findHardware: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Sub releaseHardware()
  
  Dim i As Integer, dashPos As Integer
  
  On Error GoTo errHandler
  
  ' turn off and disable power indicators
  For i = 1 To 4
    frmMain!userButton(i).BackColor = LightGray
    frmMain!userButton(i).Enabled = False
  Next i
  For i = 0 To 1
    frmMain!deltaButton(i).Enabled = False
  Next i
  frmMain!StopButton.Enabled = False
  frmMain!cmdRefresh.Enabled = False
  frmMain!cmdAbsRel.Enabled = False
    
  ' turn off Com button color
  frmMain!openClosePort.BackColor = LightGray
  
  ' gray-out the data field
  frmMain!actualPos.Enabled = False
   
  ' disable config controls
  Call enableButtons(False)
  
  ' remove firmware from captions
'  dashPos = InStr(frmMain.Caption, "-")
'  If dashPos > 0 Then
'    frmMain.Caption = left(frmMain.Caption, dashPos - 2)
'  End If
'
'  dashPos = InStr(frmComm.Caption, "-")
'  If dashPos > 0 Then
'    frmComm.Caption = left(frmComm.Caption, dashPos - 2)
'  End If
  
  ' blank out the field in the help window
  frmHelp!lblFirmwareDesc.Caption = ""

  ' force to absolute mode for next startup
  gblShowingRelative = False
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in Module1 releaseHardware: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Sub checkFirmware(s As String)
' this interprets the strings received from the firmware

  Dim lastChar As String, powerStatus As String, i As Integer

  On Error GoTo errHandler
  
  Debug.Print s
  
  If Len(s) > 0 Then
    lastChar = Right(s, 1)
  
    ' update the current position display
    If lastChar = "I" Or lastChar = "O" Then
    
      ' focuser sends ticks as it moves;
      ' change display one unit for each tick
      If lastChar = "I" Then
        absolutePos = absolutePos - 1
        relPos = relPos - 1
      Else
        absolutePos = absolutePos + 1
        relPos = relPos + 1
      End If
        
      If gblShowingRelative Then
        frmMain!actualPos.Text = fixRelPos(absolutePos - startingAbsPos)
      Else
        frmMain!actualPos.Text = CStr(absolutePos)
      End If
      
      Call iniPutData("Config", "absolutePos", CStr(absolutePos), glIniFile)

    End If

    ' FD gets current position from focuser
    ' added string legth check (jab)
    If left(s, 2) = "FD" And Len(s) >= 8 Then
      
      absolutePos = val(Mid(s, 3, 6))
      
      frmMain!Label1.Caption = "Position"
      
      ' first time thru - set starting pos to pos we just got
      If startingAbsPos = 0 Then
        startingAbsPos = absolutePos
      End If
      
      If gblShowingRelative Then
        relPos = absolutePos - startingAbsPos
        frmMain!actualPos.Text = fixRelPos(relPos)
        frmMain!actualPos.ForeColor = Red
        
      Else
        frmMain!actualPos.Text = Format(absolutePos)
        frmMain!actualPos.ForeColor = Black
      End If
      
      frmConfig!txtPosition.Text = Format(absolutePos)
      
      ' handle In, Out, and Go (FI, FO, and FG) commands
      cmdActive = False
      frmShow.Caption = "Show Data Traffic"
    End If

    ' FB gets current backlash settings
    If left(s, 2) = "FB" And Len(s) >= 8 Then

      backlashDirSetting = CInt(Mid(s, 3, 1))
      frmConfig!backlashdir(backlashDirSetting) = True
      
      backlashStepsSetting = CInt(Mid(s, 4, 5))
      frmConfig!backlashSteps.Text = CStr(backlashStepsSetting)
      
      ' save settings to the ini file
      Call iniPutData("Config", "BacklashDir", CStr(backlashDirSetting), glIniFile)
      Call iniPutData("Config", "BacklashSteps", CStr(backlashStepsSetting), glIniFile)

    End If
    
    ' FL is maximum travel from focuser
    ' added string length check (jab)
    If left(s, 2) = "FL" And Len(s) >= 8 Then

      frmConfig!maxTravel.Text = CStr(val(Mid(s, 3, 6)))
      Call iniPutData("Config", "maxTravel", frmConfig!maxTravel.Text, glIniFile)
    End If
    
    ' firmware version number
    If left(s, 2) = "FV" Then

      ' set global firmware var
      gblFirmwareVersion = Mid(s, 5, 4)
      
      If Len(gblFirmwareVersion) = 4 Then
      
        ' if the firmware version isn't already in the caption ...
        If InStr(frmComm.Caption, "Firmware") = 0 Then
'          frmComm.Caption = frmComm.Caption & " - Firmware V" & gblFirmwareVersion
'          frmMain.Caption = frmMain.Caption & " - Firmware V" & gblFirmwareVersion
        End If
        
        frmHelp!lblFirmwareDesc.Caption = "Firmware Version " & gblFirmwareVersion
                  
        firmwareResponded = True
  
      Else
        frmHelp!lblFirmwareDesc.Caption = ""
      End If
    End If
  
    ' user power status
    ' added string length check (jab)
    If left(s, 2) = "FP" And Len(s) >= 8 Then

      powerStatus = Mid(s, 5, 4)
  
      For i = 1 To 4
  
        If Mid(powerStatus, i, 1) = 2 Then
  
          frmMain!userButton(i).BackColor = &H80FF&
          currentUBState(i) = -1
  
        Else
  
          frmMain!userButton(i).BackColor = LightGray
          currentUBState(i) = 0
          
        End If
  
      Next i
    End If

    ' added temp compensation code; jgd 2/15/01
    ' added string length check (jab)
    If left(s, 2) = "FT" And Len(s) >= 8 Then
    
      ' gblTemp = Mid(s, 6, 3)
      gblTemp = Mid(s, 5, 4)          ' according to docs (jab)
      frmTemp!txtTemp.Text = displayTemp(gblTemp)
      frmMain!txtTemp.Text = displayTemp(gblTemp) & " " & tempDisplayUnitsText
      
      If IsNumeric(gblTemp) Then      '   test added 11/29/01
        tempEditVal = CSng(gblTemp)   '   CSng added 11/29/01
        gblLastTemp = CInt(gblTemp)   '   returnable global for API's (jab)
      End If
      
    End If

    ' added motor config code; jgd 2/15/01
    ' changed string length check (jab)
    If left(s, 2) = "FC" And Len(s) >= 8 Then

      gblDutyCycle = Asc(Mid(s, 3, 1))
      frmConfig!txtDutyCycle.Text = CStr(gblDutyCycle)   '  CStr added 11/29/01
      
      gblDelayPerStep = Asc(Mid(s, 4, 1))
      frmConfig!txtDelayPerStep.Text = CStr(gblDelayPerStep)   '  CStr added 11/29/01
      
      gblStepSize = Asc(Mid(s, 5, 1))
      frmConfig!txtStepSize.Text = CStr(gblStepSize)   '  CStr added 11/29/01

    End If

    If left(s, 1) <> "I" And left(s, 1) <> "O" Then
      cmdActive = False
      frmShow.Caption = "Show Data Traffic"
    End If

  End If
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in Module1 checkFirmware: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Sub parseSerialData()

  Dim firstChar As String, cmd As String, actualPos As Long
  Dim checksum As String, computedChecksum As String
  Dim formattedCmd As String
  
  On Error GoTo errHandler
  
  If Len(commInputString) > 0 Then
    firstChar = left(commInputString, 1)
    
    Do While (firstChar = "O" Or firstChar = "I") And Len(commInputString) > 0
    
      ' remove a char and update the screen
      commInputString = Right(commInputString, Len(commInputString) - 1)
      
      actualPos = absolutePos
      
      If firstChar = "O" Then
'        absolutePos = absolutePos + 1
'        relPos = relPos + 1
      Else
'        absolutePos = absolutePos - 1
'        relPos = relPos - 1
      End If
    
      ' truncate to the last 1000 characters
      frmShow!txtReceived.Text = Right(frmShow!txtReceived.Text & firstChar & vbCrLf, 1000)
      ' put cursor at end of text
      frmShow!txtReceived.SelStart = Len(frmShow!txtReceived.Text)
      
      If gblShowingRelative Then
        frmMain!actualPos.Text = fixRelPos(absolutePos - startingAbsPos)
      Else
        frmMain!actualPos.Text = absolutePos
      End If
      
      firstChar = left(commInputString, 1)
    
    Loop
    
    Do While Len(commInputString) >= 9
    
      cmd = left(commInputString, 9)
      checksum = Right(cmd, 1)
      computedChecksum = computeChecksum(left(cmd, 8))
      
      If checksum = computedChecksum Then
      
        Call checkFirmware(cmd)
    
        ' truncate to the last 1000 characters
        formattedCmd = Replace(cmd, Chr(0), "*")
        frmShow!txtReceived.Text = Right(frmShow!txtReceived.Text & formattedCmd & vbCrLf, 1000)
        ' put cursor at end of text
        frmShow!txtReceived.SelStart = Len(frmShow!txtReceived.Text)
    
        commInputString = Right(commInputString, Len(commInputString) - 9)
      
      Else
        ' added 10/24/00
        commInputString = Right(commInputString, Len(commInputString) - 1)
                
      End If
    
    Loop
    
  End If
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in Module1 parseSerialData: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Sub enableButtons(newstate As Boolean)
  
  On Error GoTo errHandler
  
  ' called from shared window frmComm (closePort_click)
 'frmConfig!backlashDir(1).Enabled = newState  '  backlash off - disabled
  frmConfig!backlashdir(2).Enabled = newstate
  frmConfig!backlashdir(3).Enabled = newstate
  frmConfig!backlashSteps.Enabled = newstate
  frmConfig!displayBacklash.Enabled = newstate
  
  frmConfig!maxTravel.Enabled = newstate
  frmConfig!displayMaxTravel.Enabled = newstate
  frmConfig!displayPos.Enabled = newstate
  
  frmConfig!txtDutyCycle.Enabled = newstate
  frmConfig!txtDelayPerStep.Enabled = newstate
  frmConfig!txtStepSize.Enabled = newstate
  frmConfig!cmdGetMotorConfig.Enabled = newstate
      
  frmSetupList!cmdSave.Enabled = newstate
  frmSetupList!cmdUse.Enabled = newstate

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in Module1 enableButtons: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Sub enableTCButtons()
  
  On Error GoTo errHandler
  
  ' Temperature Compensation buttons
  Dim commStatus As Boolean, commAndData As Boolean
  
  commStatus = frmComm!commControl.PortOpen
  commAndData = frmComm!commControl.PortOpen And gblValidDataSet
  
  frmTemp!cmdGetTemp.Enabled = commStatus
  frmTrain!cmdGetTemp.Enabled = commStatus
  
  frmTemp!optAbsRel(0).Enabled = commAndData
  frmTemp!optAbsRel(1).Enabled = commAndData
  frmTemp!optAbsRel(2).Enabled = commStatus
  
  frmTemp!cmdManual.Enabled = commStatus And (gblValidDataSet Or frmTemp!optAbsRel(2).Value)
  frmTemp!cmdAuto.Enabled = commStatus And (gblValidDataSet Or frmTemp!optAbsRel(2).Value)
  
  frmTemp!mnuViewCal.Enabled = commStatus
  
  frmCal!cmdCal.Enabled = commStatus
    
  frmMain!txtTemp.Text = ""
  frmMain!txtTemp.Visible = commStatus
  frmMain!lblTempUnits.Visible = commStatus
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in Module1 enableTCButtons: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Sub setMotorConfigParams()
  
  On Error GoTo errHandler
  
  ' build the motor config string
  cmdString = "FC"
  
  cmdString = cmdString & Chr(gblDutyCycle)
  cmdString = cmdString & Chr(gblDelayPerStep)
  cmdString = cmdString & Chr(gblStepSize)
  cmdString = cmdString & "000"   '  filler for future parameters
  
  cmdString = appendChecksum(cmdString)
  
  If frmComm!commControl.PortOpen Then
  
    Call sendCommand(cmdString, True)
    gblMotorConfigCommand = ""
    
  Else
  
    ' port is closed - save the command string for sending when it's opened
    gblMotorConfigCommand = cmdString
    
  End If

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in Module1 setMotorConfigParams: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Function fixRelPos(pos As Long) As String

  On Error GoTo errHandler
  
  If pos > -1 Then
    fixRelPos = "+" & CStr(pos)
  Else
    fixRelPos = pos
  End If

  Exit Function
  
errHandler:
  
  Call showMsg("Error in Module1 fixRelPos: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Function

Public Sub highlightRow(rowNum As Integer, state As Boolean)

  Dim C As Integer
  
  On Error GoTo errHandler
  
  If rowNum > 0 Then
    
    With frmTrain!gData
    
    .Row = rowNum
    
    For C = 1 To 4
      .Col = C
      If state Then
        .CellForeColor = White
        .CellBackColor = Blue
      Else
        .CellForeColor = Black
        .CellBackColor = White
      End If
    Next C
  
    End With
    
  End If
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in Module1 highlightRow: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Sub doCompute()
  Dim X(1 To 1000) As Double, Y(1 To 1000) As Double
  Dim R As Integer, numPoints As Integer
  Dim Intercept As String, slope As String, Stats As String
  
  On Error GoTo errHandler
  
  ' put the grid values in the arrays
  numPoints = 0
  For R = 1 To gblNextRow - 1
    With frmTrain!gData
    
    .Row = R
    .Col = 0
    
    If .Text <> "X" Then  '  ignore X'ed lines
      
      numPoints = numPoints + 1
      
      .Col = 3
    ' X(numPoints) = convertToRaw(.Text)
      X(numPoints) = trainTempArray(numPoints, 3)
      .Col = 4
      Y(numPoints) = .Text
      
    End If
    
    End With
  Next R
  
  If numPoints > 1 Then
    ' pass degree of equation, number of points, x-array and y-array.
    ' returns intercept, slope and stats.
    Call NthOrderRegression(1, numPoints, X, Y, Intercept, slope, Stats)
    
    If Intercept = "" Then
      Intercept = 0
    End If
    If slope = "" Then
      slope = 0
    End If
    
  Else
    Intercept = "0"
    slope = "0"
  End If
  
  gblIntercept = Intercept
  gblSlope = slope

  If frmTemp!optAbsRel(2).Value <> True Then
    frmTemp!txtIntercept.Text = Intercept
    frmTemp!txtSlope.Text = displaySlope(CDbl(slope))
  End If
  
  frmTrain!txtStats.Text = Stats
    
  If Intercept = "0" Or Intercept = "" And slope = "0" Or slope = "" Then
'    frmTemp!optCoeffSelect(0).Enabled = False
    gblValidDataSet = False
    Call enableTCButtons
    
  Else
  
    If frmTemp!optAbsRel(2).Value <> True Or loadingFlag = True Then
      gblIntercept = Intercept
      gblSlope = slope
    End If
    
'    frmTemp!optCoeffSelect(0).Enabled = True
    
    gblValidDataSet = True
    Call enableTCButtons
    
  End If
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in Module1 doCompute: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Sub UnloadAllForms()

  Dim frmWork As Form, i As Integer
  Dim iniString As String, iniParam As String, windowStatus As String
  Dim left As Integer, top As Integer
  
  On Error GoTo errHandler
  
  For Each frmWork In Forms
  
    With frmWork
      Debug.Print .Name
      iniString = .Name
      iniParam = .left & " " & .top & " " & .width & " " & .height
      iniParam = iniParam & " " & windowStatus
      If .width > 0 And .height > 0 Then
        Call iniPutData("Window Layout", iniString, iniParam, glIniFile)
      End If
      
    End With
    
    Unload frmWork
    Set frmWork = Nothing
  
  Next frmWork
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("Module1 UnloadAllForms")
  Resume Next
End Sub

Public Sub loadTCdata()
    
' reads temp comp data from the file opened as #2

  Dim slashPos As Long
  Dim dataline As String, dataElements() As String
  Dim R As Integer, C As Integer
  Dim response
  Dim StepSize As Integer, absRel As String
  Dim inputString As String, inputArray() As String
  Dim lineDate As String, lineTime As String
    
  On Error GoTo errHandler
  
  frmTemp.MousePointer = 11  '  hourglass

  frmAbsRel!txtManualSlope.Text = ""
  
  ' get the notes, default interval and dead zone values, microsteps/step & abs/rel state
  Line Input #2, Notes
  Notes = Replace(Notes, Chr(255), vbCrLf)
  frmTrain!txtNotes.Text = Notes

  Input #2, inputString
  inputArray = Split(inputString)
  frmTemp!txtAutoRate.Text = inputArray(3)
  
  Input #2, inputString
  inputArray = Split(inputString)
  frmTemp!txtDeadZone.Text = inputArray(3)
  
  Input #2, absRel
'  If absRel = "Absolute Mode" Then
'
'    If gblValidDataSet = True Then
'      frmTemp!optAbsRel(0).Value = True
'    Else
'      frmTemp!optAbsRel(0).Value = False
'    End If
'
'  End If
'
'  If absRel = "Relative Computed" Then
'
'    If gblValidDataSet = True Then
'      frmTemp!optAbsRel(1).Value = True
'    Else
'      frmTemp!optAbsRel(1).Value = False
'    End If
'
'  End If
'
'  If absRel = "Relative Specified" Then
'    frmTemp!optAbsRel(2).Value = True
'  End If
  
  
  Input #2, inputString
  inputArray = Split(inputString)
  StepSize = inputArray(3)
  
  frmTrain!gData.TopRow = 1
  
  R = 0
  While Not EOF(2)
  
    Input #2, dataline
    If dataline = "~~~" Then   '   end-of-data-points flag
      GoTo LoadMoreItems
    End If
      
    R = R + 1
    frmTrain!gData.Rows = R + 1
    frmTrain!gData.Row = R
    
    dataElements = Split(dataline)
    
    ' display date and time
    For C = 1 To 2
      frmTrain!gData.Col = C
      frmTrain!gData.Text = dataElements(C - 1)
    Next C
    
    ' display temperature (in specified units)
    frmTrain!gData.Col = 3
    frmTrain!gData.Text = displayTemp(CStr(dataElements(2)))
    
    ' store the raw temperature units in the array
    trainTempArray(R, 3) = dataElements(2)
    
    ' display focus position
    frmTrain!gData.Col = 4
    frmTrain!gData.Text = dataElements(3)
    
    ' the X column
    If UBound(dataElements) > 3 Then
      frmTrain!gData.Col = 0
      frmTrain!gData.Text = dataElements(4)
    End If
    
    ' store the date and time in a format for sorting
    lineDate = Format(dataElements(0), "yyyymmdd")
    lineTime = Format(dataElements(1), "hhmmss")
    frmTrain!gData.Col = 5
    frmTrain!gData.Text = lineDate & lineTime

    ' store the position for restoring original order
    frmTrain!gData.Col = 6
    frmTrain!gData = R
    
  Wend
  
LoadMoreItems:

'  If R > 1 Then
'    frmTemp!optCoeffSelect(0).Enabled = True
'  End If
  
'  If gblSlopeManual <> 0 Then
'    frmTemp!optCoeffSelect(1).Enabled = True
'    frmTemp!txtManualSlope.Text = gblSlopeManual
'  End If
  
  frmTemp.MousePointer = 0  '  normal
    
  ' fill the arrays and compute the linear regression
  gblNextRow = R + 1
  If R > 1 Then
    doCompute
  End If
  
  frmTrain!gData.Rows = gblNextRow + 1
  
  selectedRow = -1  '  -1 signifies no row is selected
  
  unsavedData = False
  
  frmTemp!txtCalcPos.Text = ""
  frmTemp!txtEnterTemp.Text = ""
  
  ' store the computed values for use when the user selects a Relative mode
  If IsNumeric(frmTemp!txtIntercept.Text) Then
    gblIntercept = frmTemp!txtIntercept.Text
'  Else
'    gblIntercept = 0
  End If

  If resetStepSize = True Then
    gblFileStepSize = StepSize
  End If
  
  If gblFileStepSize <> frmConfig!txtStepSize.Text And _
                                    gblFileStepSize > 0 And gblFileStepSize < 256 Then
    
    frmConfig!txtStepSize.Text = gblFileStepSize
    gblStepSize = gblFileStepSize
    Call setMotorConfigParams
   
  ' Call showMsg("Step Size has been set to " & gblFileStepSize & ".", _
                  "", 3) ' , frmTemp.left + 1000, frmTemp.top + 1000)
  End If
  
  savedStepSize = StepSize
  
  ' write info to log file
  Open App.Path & "\RFTrack.log" For Append As #1
  
  Print #1, Format(Date, "mm/dd/yy") & " " & Format(Time(), "hh:mm:ss") & _
                                                      " Loaded " & initDir & defFile
  Print #1, "                  Step Size = " & StepSize
  Print #1, "                  Auto Rate = " & frmTemp!txtAutoRate.Text
  Print #1, "                  Dead Zone = " & frmTemp!txtDeadZone.Text
  
  If CInt(frmTemp!optAbsRel(0).Value) = 0 Then
    Print #1, "                  Relative Mode"
  Else
    Print #1, "                  Absolute Mode"
  End If
  
  Close #1

  Call enableTCButtons
  
  Exit Sub
  
errHandler:
  
  Call unknownErrorHandler("Module1 LoadTCData - Bad Data File")
  On Error GoTo 0
  
  ' start over by forcing File | New
  unsavedData = False
  frmTemp.mnuNew_Click
  
  Exit Sub

End Sub

Public Function displayTemp(rawUnits As String) As String
  
  On Error GoTo errHandler
  If rawUnits = "" Then
    Exit Function
  End If
  
  ' takes a raw-units value and displays it in the selected units
  
  Select Case tempDisplayUnits
  
    Case 0
      ' raw units - no conversion
      displayTemp = Round(rawUnits, 0)
    
    Case 1
      ' Farenheit
    ' displayTemp = CStr(degreesF(Int(CSng(rawUnits) * gblTempConvBeta)) - 273)
      displayTemp = CStr(Round(degreesF(CSng(rawUnits) * gblTempConvBeta - 273), 1))
      displayTemp = Format(displayTemp, "##0.0")
    
    Case 2
      ' Celsius
    ' displayTemp = CStr(Round(CSng(rawUnits) * gblTempConvBeta, 1) - 273)
      displayTemp = CStr(Round(CSng(rawUnits) * gblTempConvBeta - 273, 1))
      displayTemp = Format(displayTemp, "##0.0")
   
  End Select
  
  Exit Function
  
errHandler:
  
  Call showMsg("Error in Module1 displayTemp: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Function

Public Sub redisplayTemps()
  ' redisplay the temperature fields whenever the temp units changes.
  
  Dim R As Integer, chkDate As String
  
  On Error GoTo errHandler
  
  ' Movement history table
  With frmTemp!rData
    .Row = 1
    .Col = 0
    chkDate = .Text
    If chkDate <> "" Then
      .Col = 2
      For R = 1 To .Rows - 1
        .Row = R
        .Text = displayTemp(CStr(runTempArray(R)))
      Next R
    End If
  End With
  
  ' frmTemp edit field
'  If tempEditVal > 0 Then
  If frmTemp!txtTemp.Text <> "" Then
    frmTemp!txtTemp.Text = displayTemp(CStr(tempEditVal))
  End If
  
  ' calculator edit field
  If calcEditVal > 0 Then
    frmTemp!txtEnterTemp.Text = displayTemp(CStr(calcEditVal))
  End If
  
  ' frmTrain edit field
  If frmTrain!txtDate.Text <> "" And trainEditVal > 0 Then
    frmTrain!txtTemp.Text = displayTemp(CStr(trainEditVal))
  End If
  
  ' frmTrain table
  With frmTrain!gData
    .Row = 1
    .Col = 1
    chkDate = .Text
    If chkDate <> "" Then
      .Col = 3
      For R = 1 To .Rows - 2
        .Row = R
        .Text = displayTemp(CStr(trainTempArray(R, 3)))
      Next R
    End If
  End With
  
  ' frmMain
  frmMain!txtTemp.Text = displayTemp(CStr(tempEditVal)) & " " & tempDisplayUnitsText
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in Module1 redisplayTemps: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Function convertToRaw(tempValue As String) As String
' takes a temperature and converts it to raw units

  On Error GoTo errHandler
  
  Select Case tempDisplayUnits
    Case 0
      ' already raw
      convertToRaw = tempValue
      
    Case 1
      ' farenheit
      convertToRaw = (((tempValue - 32) / 1.8) + 273) / gblTempConvBeta
  
    Case 2
      ' celsius
      convertToRaw = (tempValue + 273) / gblTempConvBeta
  End Select
  
  Exit Function
  
errHandler:
  
  Call showMsg("Error in Module1 convertToRaw: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Function

Public Function convertSlopeToRaw(slope As Single) As Single
' takes the value entered in the frmAbsRel slope box and converts it to
' the equivalent slope in raw units
  
  On Error GoTo errHandler
  
  Select Case tempDisplayUnits
    Case 0
      ' already raw
      convertSlopeToRaw = slope
      
    Case 1
      ' farenheit
      convertSlopeToRaw = slope / (gblTempConvBeta * 9 / 5)
      
    Case 2
      ' celsius
      convertSlopeToRaw = slope / gblTempConvBeta
      
  End Select
  
  Exit Function
  
errHandler:
  
  Call showMsg("Error in Module1 convertSlopeToRaw: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Function

Public Function displaySlope(slope As Double) As String
' displays the slope adjusted for the current temperature units
  
  On Error GoTo errHandler
  
  Select Case tempDisplayUnits

    Case 0
      ' raw units - no conversion
      displaySlope = CStr(slope)
      
    Case 1
      ' Farenheit
      displaySlope = slope * gblTempConvBeta * 9 / 5
      
    Case 2
      ' Celsius
      displaySlope = slope * gblTempConvBeta
      
  End Select
      
  displaySlope = Format(displaySlope, "##0.000")
  
  Exit Function
  
errHandler:
  
  Call showMsg("Error in Module1 displaySlope: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Function


