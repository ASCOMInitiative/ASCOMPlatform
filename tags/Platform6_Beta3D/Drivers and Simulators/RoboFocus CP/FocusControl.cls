VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "FocusControl"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Attribute VB_Description = "Original RoboFocus API"
'==================
' FocusControl.cls
'==================
'
' Implementation of the RoboFocus FocusControl class
'
' Initial code by RoboFocus team...
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 05-Apr-03 jab     ASCOM additions (refrence counting in Initialize and
'                   Terminate
' 14-Apr-03 jab     Change getTemperature to return last stored value so
'                   focus movement won't be affected
'---------------------------------------------------------------------
Option Explicit
  
' The Terminate event occurs when the ActiveX document is about to be destroyed. You can use
' the Termination event to clean up any object references by setting all global object
' references to Nothing.
Public Sub Class_Initialize()

    ' count references
    g_iFCConnections = g_iFCConnections + 1

End Sub

Public Sub Class_Terminate()

    If g_iFCConnections > 0 Then
        g_iFCConnections = g_iFCConnections - 1
        
        ' if launched as server then reference counting will determine behavior
        If Not g_bRunExecutable Then
            If g_iFCConnections <= 0 Then
                End     'last reference, so shut down
            End If
        End If
    End If

 ' oldfragment: Call frmMain.Form_QueryUnload(0, 1)
 
End Sub

''''''''''''''''''''''''
' Get Info Functions:  '
''''''''''''''''''''''''
Public Function getFirmwareVersion() As String

  If getCommStatus = False Then
    getFirmwareVersion = "Not Connected"
  Else
    getFirmwareVersion = gblFirmwareVersion
  End If
  
End Function

Public Function getCmdActive() As Boolean
  getCmdActive = cmdActive
End Function

'Public Function getPosition() As Integer
'
'  If getCommStatus = False Then
'    getPosition = -1
'  Else
'    getPosition = frmMain.actualPos.Text
'  End If
'
'End Function

Public Function getPosition() As Long

  If getCommStatus = False Then
    getPosition = -1
  Else
    getPosition = absolutePos  '  frmMain.actualPos.Text
  End If

End Function

Public Function getRelativePosition() As Long

  If getCommStatus = False Then
    getRelativePosition = -1
  Else
    getRelativePosition = relPos
  End If

End Function

Public Function getDelta() As Integer

  If getCommStatus = False Then
    getDelta = -1
  Else
    getDelta = frmMain.deltaAmount.Text
  End If
  
End Function

Public Function getUserButtonState(ByVal Button As Integer) As Boolean
  getUserButtonState = currentUBState(Button)
End Function

Public Function getUserButtonCaption(ByVal Button As Integer) As String
  getUserButtonCaption = frmConfig.buttonCaption(Button).Text
End Function

Public Function getCommStatus() As Boolean
  getCommStatus = frmComm.commControl.PortOpen
End Function

Public Function getOpenCommPortOnStartup() As Boolean

  If frmConfig.commAutoOpen.Value = 1 Then
    getOpenCommPortOnStartup = True
  Else
    getOpenCommPortOnStartup = False
  End If
  
End Function

Public Function getBacklashDirection() As Integer
  
  If getCommStatus = False Then
    getBacklashDirection = -1
  Else
    getBacklashDirection = backlashDirSetting
  End If
  
End Function

Public Function getBacklashSteps() As Integer

  If getCommStatus = False Then
    getBacklashSteps = -1
  Else
    getBacklashSteps = backlashStepsSetting
  End If
  
End Function

Public Function getMaxTravel() As Long

  If getCommStatus = False Then
    getMaxTravel = -1
  Else
    getMaxTravel = maxTravelSetting
  End If
  
End Function

Public Function getKeepMainOnTop() As Boolean
  getKeepMainOnTop = frmConfig!chkKeepOnTop.Value
End Function

Public Function getDataTraffic(ByVal Direction As Integer) As String
  
  ' direction: 1 = Sent, 2 = Received
  If Direction = 1 Then
    getDataTraffic = frmShow!txtSent.Text
  Else
    getDataTraffic = frmShow!txtReceived.Text
  End If
  
End Function

Public Function getTempCompAbsRel() As Integer
  
  ' returns 0 if in absolute mode, 1 if relative computed, 2 if relative specified,
  ' -1 if not connected or no dataset open
  
  If getCommStatus() And gblValidDataSet = True Then
    
    If frmTemp!optAbsRel(0).Value = True Then
      getTempCompAbsRel = 0
    End If
    
    If frmTemp!optAbsRel(1).Value = True Then
      getTempCompAbsRel = 1
    End If
    
    If frmTemp!optAbsRel(2).Value = True Then
      getTempCompAbsRel = 2
    End If
  
  Else
    getTempCompAbsRel = -1
  End If
  
End Function

Public Function getTemperature() As Integer
' returns the temperature in raw units, or -1 if offline or no temp returned

  Dim start As Single
  
  If getCommStatus Then
      getTemperature = gblLastTemp
      
' the code below is old, it did not check for activity first
' and therefore killed any movement in progress (jab)
'
' get temperature string
'    cmdString = "FT000000"
'    cmdString = appendChecksum(cmdString)
'    Call sendCommand(cmdString, True)
'
'    ' wait until the temp data appears
'    start = Timer
'    While Timer < start + 0.2 And gblTemp = ""
'      DoEvents
'    Wend
'
'    If gblTemp <> "" Then
'      getTemperature = CInt(gblTemp)
'    Else
'      getTemperature = -1
'    End If
    
  Else
    getTemperature = -1
  End If

End Function

Public Function getAutoRate() As Single
  ' returns the autorate, or -1 if no dataset loaded
  
  If gblValidDataSet Then
    getAutoRate = CSng(frmTemp!txtAutoRate.Text)
  Else
    getAutoRate = -1
  End If

End Function

Public Function getDeadZone() As Integer
  ' returns the autorate, or -1 if no dataset loaded
  
  If gblValidDataSet Then
    getDeadZone = CInt(frmTemp!txtDeadZone.Text)
  Else
    getDeadZone = -1
  End If
  
End Function

Public Function getStepSize() As Integer

  getStepSize = frmConfig!txtStepSize.Text
  
End Function

'''''''''''''''''''''''
' Set Info Functions: '
'''''''''''''''''''''''

Public Function setPosition(ByVal Position As Long) As Long

  Dim positionText As String, notused As Long, passed As Boolean
  
  If getCommStatus() Then

    positionText = CStr(Position)
    Call validateLong(positionText, notused, "Position", 0, 65535, _
                                                         frmConfig!Position, passed)
    If passed Then

      frmConfig.txtPosition.Text = positionText
      
      frmConfig.Show
      Call frmConfig.txtPosition_LostFocus
      frmConfig.Hide
      
      ' return the new position
      setPosition = getPosition()

    Else
      setPosition = -1
    End If
  Else
    ' no comm established
    setPosition = -1
  End If
  
End Function

Public Function setDelta(ByVal Delta As Long) As Long

  Dim deltaText As String, notused As Long, passed As Boolean
  
  deltaText = CStr(Delta)
  Call validateLong(deltaText, notused, "delta", 0, totalSteps, _
                                                               frmMain!deltaAmount, passed)
  If passed Then
    frmMain!deltaAmount.Text = deltaText
    
    ' return the new amount
    setDelta = getDelta()
  Else
    setDelta = -1
  End If
  
End Function

Public Function setUserButtonCaption(ByVal Button As Integer, ByVal Caption As String) As String

  frmConfig.buttonCaption(Button) = Caption
  frmMain.userButton(Button).Caption = Caption
  
  ' return the new caption
  setUserButtonCaption = getUserButtonCaption(Button)
  
End Function

Public Function setOpenCommPortOnStartup(ByVal OpenPort As Boolean) As Boolean
  
  If OpenPort Then
    frmConfig.commAutoOpen.Value = 1
  Else
    frmConfig.commAutoOpen.Value = 0
  End If
  
  ' return the current setting (to be sure it worked)
  setOpenCommPortOnStartup = getOpenCommPortOnStartup()
  
End Function

Public Function setBacklashDirection(ByVal Dir As Integer) As Integer

  If getCommStatus() Then
    Call frmConfig.backlashDir_Click(Dir)
    setBacklashDirection = getBacklashDirection
  Else
    setBacklashDirection = 0
  End If
  
End Function

Public Function setBacklashSteps(ByVal backlashSteps As Long) As Long
  Dim backlashStepsText As String, notused As Long, passed As Boolean
  
  If getCommStatus() Then
    backlashStepsText = CStr(backlashSteps)
    Call validateLong(backlashStepsText, notused, "backlashSteps", 0, totalSteps, _
                                                                 frmConfig!backlashSteps, passed)
    If passed Then
      frmConfig.backlashSteps.Text = backlashStepsText
            
      ' return the new backlashSteps
      setBacklashSteps = getBacklashSteps()
    Else
      setBacklashSteps = -1
    End If
  Else
    setBacklashSteps = -1
  End If
  
End Function

Public Function setMaxTravel(ByVal maxTravel As Long) As Long
  Dim maxTravelText As String, notused As Long, passed As Boolean
  
  If getCommStatus() Then
'    maxTravelText = CStr(maxTravel)
'    Call validateLong(maxTravelText, notused, "maxTravel", 0, totalSteps, _
'                                                                 frmConfig!maxTravel, passed)
'    If passed Then
'      frmConfig.maxTravel.Text = maxTravelText
'
'      ' return the new maxTravel
'      setMaxTravel = getMaxTravel()
'    Else
'      setMaxTravel = -1
'    End If

    frmConfig!maxTravel.Text = maxTravel
    frmConfig.maxTravel_LostFocus
    
  Else
    setMaxTravel = -1
  End If
  
End Function

Public Function setKeepMainOnTop(ByVal KeepMainOnTop As Boolean) As Boolean
  
  If KeepMainOnTop Then
    frmConfig!chkKeepOnTop.Value = 1
  Else
    frmConfig!chkKeepOnTop.Value = 0
  End If
          
  ' return the new KeepMainOnTop
  setKeepMainOnTop = getKeepMainOnTop()
  
End Function

Public Function setTempCompAbsRel(ByVal absRel As Integer) As Integer
' 0 sets absolute mode, 1 sets relative
  
  If getCommStatus() And gblValidDataSet Then
  
    frmTemp.optAbsRel(absRel) = True
    setTempCompAbsRel = getTempCompAbsRel()
    
  Else
    setTempCompAbsRel = -1
  End If
  
End Function

Public Function setAutoRate(ByVal autoRate As Single) As Single
  ' returns the passed Auto Rate Interval if successful, or -1 if no dataset loaded
  
  If gblValidDataSet Then
  
    frmTemp!txtAutoRate.Text = CStr(autoRate)
    
    ' return the new setting
    setAutoRate = getAutoRate()
    
  Else
    setAutoRate = -1
  End If
  
End Function

Public Function setDeadZone(ByVal deadZone As Integer) As Integer
  ' returns the passed Dead Zone setting if successful, or -1 if no dataset loaded
  
  If gblValidDataSet Then
  
    frmTemp!txtDeadZone.Text = CStr(deadZone)
    
    ' return the new setting
    setDeadZone = getDeadZone()
    
  Else
    setDeadZone = -1
  End If
End Function

Public Function setStepSize(ByVal StepSize As Integer) As Integer

  Dim stepSizeText As String, notused As Integer, passed As Boolean
  
  stepSizeText = CStr(StepSize)
  Call validateInt(stepSizeText, notused, "stepsize", 1, 64, _
                                                               frmConfig!txtStepSize, passed)
  If passed Then
    frmConfig!txtStepSize.Text = stepSizeText
    
    ' return the new amount
    setStepSize = getStepSize()
  Else
    setStepSize = -1
  End If
  
End Function

'''''''''''''''''''''
' Action Functions: '
'''''''''''''''''''''

Public Function actGoToPosition(ByVal Position As Long) As Boolean
  Dim positionText As String, notused As Long, passed As Boolean, noResend As Boolean
  
  positionText = CStr(Position)
  
  If getCommStatus() Then
    Call validateLong(positionText, notused, "Position", 1, totalSteps, _
                                                frmMain!actualPos, passed)
    
    If passed Then
    
      If val(frmConfig!maxTravel.Text) < val(frmMain!actualPos.Text) Then
        ' Position may not be larger than Maximum Travel.
        
        actGoToPosition = False
        frmMain!actualPos.SetFocus
        
      Else
        
        frmMain!actualPos.Text = positionText
        
        frmMain!Label1.Caption = "Moving to " & Format(frmMain!actualPos.Text)
        
        cmdString = "FG" & Format(frmMain!actualPos.Text, "000000")
        cmdString = appendChecksum(cmdString)
        
        noResend = True
        Call sendCommand(cmdString, noResend)
        
        frmMain!actualPos.SetFocus
        
        actGoToPosition = True
        
      End If
    
    End If
  Else
    actGoToPosition = False
  End If

End Function

Public Function actStop() As Boolean
  
  If getCommStatus() Then
    Call frmMain.StopButton_Click
    actStop = True
  Else
    actStop = False
  End If

End Function

Public Function actIn() As Boolean
  
  If getCommStatus() Then
    Call frmMain.deltaButton_Click(0)
    actIn = True
  Else
    actIn = False
  End If

End Function

Public Function actOut() As Boolean

  If getCommStatus() Then
    Call frmMain.deltaButton_Click(1)
    actOut = True
  Else
    actOut = False
  End If
  
End Function

Public Function actRefresh() As Boolean
  
  If getCommStatus() Then
    Call frmMain.cmdRefresh_Click
    actRefresh = True
  Else
    actRefresh = False
  End If

End Function

Public Function actUserButtonToggle(ByVal Button As Integer) As Boolean

  Call frmMain.userButton_Click(Button)
  
  ' return the button state after the toggle
  actUserButtonToggle = getUserButtonState(Button)

End Function

Public Function actOpenComm() As Boolean
  
  If getCommStatus() Then
    ' already open
  Else
    Call frmComm.openPort_Click
  End If
  
  actOpenComm = getCommStatus()

End Function

Public Function actCloseComm() As Boolean
  
  If getCommStatus() Then
    Call frmComm.closePort_Click
  Else
    ' already closed
  End If
  
  actCloseComm = getCommStatus()

End Function

Public Function actLoadTempCompFile(ByVal TCFile As String) As Boolean
 ' returns true if file loaded successfully, false if not
 
  On Error GoTo errorHandler
  
  Open TCFile For Input As #2
  Call loadTCdata
  Close #2
  
  On Error GoTo 0
  
  actLoadTempCompFile = True
  
  Exit Function
  
errorHandler:
  Call showMsg("Error opening data file.", "", 2)
  
  On Error GoTo 0
  
  actLoadTempCompFile = False
  
End Function

Public Function actTempCompManual() As Boolean
' returns true if click worked, or false if offline or no dataset

  If getCommStatus() And gblValidDataSet Then
    Call frmTemp.cmdManual_Click
    actTempCompManual = True
  Else
    actTempCompManual = False
  End If
  
End Function

Public Function actTempCompAuto() As Boolean
' returns true if successful, or false if offline or no dataset

  If getCommStatus() And gblValidDataSet Then
    Call frmTemp.cmdAuto_Click
    actTempCompAuto = True
  Else
    actTempCompAuto = False
  End If
  
End Function

