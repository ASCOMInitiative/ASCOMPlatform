Attribute VB_Name = "CommonModule"
Option Explicit

Public commPortSetting As Integer, commPortPrevSetting As Integer
Public commControlPrevSettings As String
Public commInputString As String, commReceivedString As String

Public glIniFile As String

Declare Function GetPrivateProfileStringA& Lib "kernel32" _
  (ByVal sect$, ByVal entry$, ByVal default$, ByVal buff$, ByVal buffsize&, ByVal fname$)

Declare Function GetPrivateProfileSectionA& Lib "kernel32" Alias "GetPrivateProfileString" _
  (ByVal sect$, ByVal entry&, ByVal default$, ByVal buff$, ByVal buffsize&, ByVal fname$)

Declare Function WritePrivateProfileStringA& Lib "kernel32" _
  (ByVal sect$, ByVal entry$, ByVal dat$, ByVal fname$)

Declare Function GetTickCount& Lib "kernel32" ()

Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" _
                              (ByVal hWnd As Long, ByVal lpOperation As String, _
                              ByVal lpFile As String, ByVal lpParameters As String, _
                              ByVal lpDirectory As String, ByVal nShowCmd As Long) As Long

Declare Function ShellExecuteForExplore Lib "shell32.dll" Alias "ShellExecuteA" _
                              (ByVal hWnd As Long, ByVal lpOperation As String, _
                              ByVal lpFile As String, lpParameters As Any, _
                              lpDirectory As Any, ByVal nShowCmd As Long) As Long
   
Declare Function GetWindowsDirectory Lib "kernel32" Alias "GetWindowsDirectoryA" _
                              (ByVal lpBuffer As String, ByVal nSize As Long) As Long

Declare Function GetSystemDirectory Lib "kernel32" Alias "GetSystemDirectoryA" _
                              (ByVal lpBuffer As String, ByVal nSize As Long) As Long

Public cmdString As String
Public cmdActive As Boolean

Public firmwareResponded As Boolean

Public gblParamError As Integer

Public Const Black As Long = &H80000008
Public Const White As Long = &HFFFFFF
Public Const LightGray As Long = &H8000000F
Public Const LighterGray As Long = &HE0E0E0

Public Const Red As Long = &HFF&
Public Const DarkRed As Long = &HC0&

Public Const Green As Long = &HFF00&
Public Const LightGreen As Long = &H80FF80
Public Const DarkGreen As Long = 49152

Public Const Blue As Long = &HFF0000
Public Const Yellow As Long = &HFFFF&

Public gblDegree As Integer          '  degree of equation for Nth-order regression
Public gblR(1 To 7, 1 To 8) As Double   '  constant and coefficients for Nth-order regression

Public Function appendChecksum(cmd As String) As String

  ' checksum is just the truncated sum of the bytes in the command
  Dim checksumChar As String
  
  On Error GoTo errHandler

  checksumChar = computeChecksum(cmd)
  appendChecksum = cmd & checksumChar
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule appendCheckSum")
  Resume Next
  
End Function

Function computeChecksum(cmd As String)

  Dim i As Integer, byteSum As Integer
  
  On Error GoTo errHandler
  
  byteSum = 0

  For i = 1 To Len(cmd)
    byteSum = byteSum + Asc(Mid(cmd, i, 1))
  Next i

  computeChecksum = Chr(byteSum Mod 256)
  
  On Error GoTo 0
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule - computeCheckSum")
  Resume Next
  
End Function

Function iniGetBool%(sect$, entry$, def%, INIFile$)

  Dim a$
  
  On Error GoTo errHandler
  
  a$ = IniGetData(sect$, entry$, IIf(def, "True", "False"), INIFile$)
  If InStr("YT", left(a$, 1)) > 0 Or Val(a$) <> 0 Then
    iniGetBool = True
  Else
    iniGetBool = False
  End If
  
  On Error GoTo 0
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule - iniGetBool")
  Resume Next

End Function

Function IniGetData$(sect$, entry$, default$, INIFile$)

  Dim a$, i&
  
  On Error GoTo errHandler
  
  a$ = Space(256)
  i = GetPrivateProfileStringA(sect$, entry$, default$, a$, 256, INIFile)
  a$ = left(a$, i)
  IniGetData = a$
  
  On Error GoTo 0
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule - iniGetData")
  Resume Next

End Function

Sub iniPutBool(sect$, entry$, v%, INIFile$)

  Dim a$
  
  On Error GoTo errHandler
  
  a$ = IIf(v, "True", "False")
  Call iniPutData(sect$, entry$, a$, INIFile$)
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule - iniPutBool")
  Resume Next

End Sub

Sub iniPutData(sect$, entry$, dat$, INIFile$)

  Dim i%
  
  On Error GoTo errHandler
  
  sect$ = Trim(sect$)
  entry$ = Trim(entry$)
  dat$ = Trim(dat$)
  
  i = WritePrivateProfileStringA(sect$, entry$, dat$, INIFile$)
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule - iniPutData")
  Resume Next

End Sub

Public Sub saveFormPosition(thisForm As Form)

  Dim iniString As String, iniParam As String, windowStatus As String
  
  On Error GoTo errHandler

  With thisForm
    
    If .Visible Then
      windowStatus = "1"
    Else
      windowStatus = "0"
    End If
    iniString = .Name
    iniParam = .left & " " & .top & " " & .width & " " & .height
    iniParam = iniParam & " " & windowStatus
    If .width > 0 And .height > 0 Then
      Call iniPutData("Window Layout", iniString, iniParam, glIniFile)
    End If
    
  End With

  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule - saveFormPosition")
  Resume Next
  
End Sub

Public Sub setFormPositionIni(thisForm As Form, layoutDefault As String)

  Dim layoutString As String, layoutValues() As String, boxMargin As Integer
  Dim a
  
  On Error GoTo errHandler
  
  layoutString = IniGetData("Window Layout", thisForm.Name, layoutDefault, glIniFile)
  layoutValues() = Split(layoutString)
  
  With thisForm
    .left = CSng(layoutValues(0))
    .top = CSng(layoutValues(1))
    .width = CSng(layoutValues(2))
    .height = CSng(layoutValues(3))
  
    ' check to be sure we're not loading off-the-screen values
    If .left > (Screen.width - 500) Then
      .left = 0
    End If
    If .top > (Screen.height - 500) Then
      .top = 2000
    End If
    
  End With
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule - setFormPositionIni")
  Resume Next

End Sub

Public Sub setFormPosition(Form)

  On Error GoTo errHandler
  
  Form.top = frmMain.top + frmMain.height + 1
  Form.left = frmMain.left
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule - setFormPosition")
  Resume Next

End Sub

Public Sub commErrorHandler()

  Select Case Err.Number
    Case 8005
      Call showMsg("COM" & commPortSetting & " is already in use.", "", 2)
      
    Case 8002
      Call showMsg("This machine has no COM" & commPortSetting & ".", "", 2)
      
    Case 8018
      Call showMsg("Port is closed.", "", 2)
      
    Case 8015
      Call showMsg("Com Port error. It may be in use by another application.", "", 2)
      
    Case Else
      Call showMsg("commErrorHandler: Error Number " & Err.Number & ": " & Err.Description, "", 5)
      
  End Select

End Sub

Public Sub sendCommand(cmdString As String, Optional noResend As Boolean)

  Dim starttime As Single, pauseTime As Single, endtime As Single
  Dim i As Integer
  
  Dim opMode As Integer      ' 0 = delay mode, 1 = instant mode
  
  Dim formattedCmdString As String
  
  On Error GoTo sendError
  
  opMode = 0
  
  cmdActive = True
  frmShow.Caption = "Show Data Traffic   (Command in Progress)"
  Debug.Print "SendCommand: " & cmdString
  
  pauseTime = 0.5
  i = 1
  
  waitingForComm = True   '  flag to ensure comm is connected (reset by onComm receive event)
  
  Do While i <= 2 And cmdActive
    
    frmComm!commControl.Output = cmdString                  ' & Chr$(13)
    formattedCmdString = Replace(cmdString, Chr(0), "*")
    frmShow!txtSent.Text = frmShow!txtSent.Text & formattedCmdString & vbCrLf
    frmShow!txtSent.SelStart = Len(frmShow!txtSent.Text)
    
    starttime = Timer
    ' if within 3 seconds of midnight, just ignore command, to
    ' avoid an infinite loop
    If starttime < 86397 Then
      
      endtime = starttime + pauseTime

'      charReceived = False
      Do While Timer < endtime And cmdActive
        DoEvents
      Loop
        
      If opMode = 1 Then
        ' instant mode; force loop exit
        i = i + 2
      Else
        If noResend Then
          ' In, Out, or GoTo command
          i = i + 2
        Else
          i = i + 1
        End If
        
        ' wait for hardware to settle
        starttime = Timer
        endtime = starttime + pauseTime

        Do While Timer < endtime
          DoEvents
        Loop

      End If
    End If
  Loop

  If waitingForComm And cmdString <> "FV000000¼" And Not debugging Then
    Call showMsg("Comm connection lost; closing port.", "", 2)
    If autoCorrect = True Then
      frmTemp.cmdAuto_Click
    End If
    frmComm.closePort_Click
  End If
  
  If left(cmdString, 2) <> "FI" And left(cmdString, 2) <> "FO" And left(cmdString, 2) <> "FG" Then
    cmdActive = False
    frmShow.Caption = "Show Data Traffic"
  End If
  
  On Error GoTo 0

  Exit Sub
  
sendError:
  commErrorHandler
  On Error GoTo 0
  
End Sub

Public Sub unknownErrorHandler(ByVal callingRoutine As String)

  Call showMsg(callingRoutine & vbCrLf & ": Error Number " & Err.Number & ": " & _
                                                Err.Description, "Unknown Error", 5)
  
End Sub

Public Sub validateSingle(testValue As String, ByRef gblVariable As Single, _
  fieldName As String, minAllowed As Single, maxAllowed As Single, _
  currentControl As Control, passed As Boolean)

  Dim valid As Integer, testSingle As Single
  
  On Error GoTo errHandler

  If App.StartMode = vbSModeAutomation Then
    passed = True
    Exit Sub
  End If
  
  valid = 1
  
  If Not (IsNumeric(testValue)) Then
    MsgBox fieldName + " must be numeric."
    valid = 0
  Else
    testSingle = Val(testValue)
    If (testSingle < minAllowed Or testSingle > maxAllowed) Then
      MsgBox "Valid " + fieldName + " range is " + _
        CStr(minAllowed) + " to " + CStr(maxAllowed) + "."
      valid = 0
    End If
  End If
  
  If valid = 0 Then
    
    currentControl.SetFocus
    gblParamError = 1
    
  Else
    passed = True
    gblVariable = testSingle
  End If
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule - validateSingle")
  Resume Next
  
End Sub

Public Sub validateDouble(testValue As String, ByRef gblVariable As Double, _
  fieldName As String, minAllowed As Double, maxAllowed As Double, _
  currentControl As Control, passed As Boolean)

  Dim valid As Integer, testDouble As Double
    
  On Error GoTo errHandler

  If App.StartMode = vbSModeAutomation Then
    passed = True
    Exit Sub
  End If
  
  valid = 1
  
  If Not (IsNumeric(testValue)) Then
    MsgBox fieldName + " must be numeric."
    valid = 0
  Else
    testDouble = Val(testValue)
    If (testDouble < minAllowed Or testDouble > maxAllowed) Then
      MsgBox "Valid " + fieldName + " range is " + _
        CStr(minAllowed) + " to " + CStr(maxAllowed) + "."
      valid = 0
    End If
  End If
  
  If valid = 0 Then
    
    currentControl.SetFocus
    gblParamError = 1
    
  Else
    passed = True
    gblVariable = testDouble
  End If
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule validateDouble")
  Resume Next
  
End Sub

Public Sub validateInt(testValue As String, ByRef gblVariable As Integer, _
  fieldName As String, minAllowed As Integer, maxAllowed As Integer, _
  currentControl As Control, passed As Boolean)

  Dim valid As Integer, testInt As Integer
  
  On Error GoTo errHandler

  If App.StartMode = vbSModeAutomation Then
    passed = True
    Exit Sub
  End If
  
  valid = 1
  
  If Not (IsNumeric(testValue)) Then
    MsgBox fieldName + " must be numeric."
    valid = 0
  Else
    testInt = Val(testValue)
    If (testInt < minAllowed Or testInt > maxAllowed) Then
      MsgBox "Valid " + fieldName + " range is " + _
        CStr(minAllowed) + " to " + CStr(maxAllowed) + "."
      valid = 0
    End If
  End If
  
  If valid = 0 Then
    
    currentControl.SetFocus
    gblParamError = 1
    
  Else
    passed = True
    gblVariable = testInt
  End If
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule validateInt")
  Resume Next
  
End Sub

Public Sub validateLong(testValue As String, ByRef gblVariable As Long, _
  fieldName As String, minAllowed As Long, maxAllowed As Long, _
  currentControl As Control, passed As Boolean)

  Dim valid As Long, testlong As Long
  
  On Error GoTo errHandler

  If App.StartMode = vbSModeAutomation Then
    passed = True
    Exit Sub
  End If
  
  valid = 1
  
  If Not (IsNumeric(testValue)) Then
    MsgBox fieldName + " must be numeric."
    valid = 0
  Else
    testlong = Val(testValue)
    If (testlong < minAllowed Or testlong > maxAllowed) Then
      MsgBox "Valid " + fieldName + " range is " + _
        CStr(minAllowed) + " to " + CStr(maxAllowed) + "."
      valid = 0
    End If
  End If
  
  If valid = 0 Then
    
    currentControl.Text = CStr(gblVariable)
    gblParamError = 1
    
  Else
    passed = True
    gblVariable = testlong
  End If
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule validateLong")
  Resume Next
  
End Sub

Public Function angleConvert(Angle As String, Direction As String, _
  Optional longitudeFlag As Boolean)

  ' checks the passed Angle for *'s and :'s to determine RA or Dec
  ' Converts between long form ( HH:MM:SS or DD*MM.M ) and short form ( HH:MM.M or DD*MM )
  
  Dim delimPos As Long, decimalPos As Long
  Dim hours As Integer, minutes As Integer, intSeconds As Integer, seconds As Single
  Dim degrees As Integer, decSign As String
  
  On Error GoTo errHandler

  delimPos = InStr(Angle, "*")
  If delimPos = 0 Then
    ' it's RA
    
    'hours = CInt(left(Angle, 2))
    hours = Split(Angle, ":", -1)(0)
    ' minutes = CInt(Mid(Angle, 4, 2))
    minutes = Split(Angle, ":", -1)(1)
    
    decimalPos = InStr(Angle, ".")
    If decimalPos > 0 Then
      ' it's short. if IN, convert to long.
      If Direction = "In" Then
        seconds = CInt(Right(Angle, 1))
        
        seconds = seconds * 0.1 * 60
        angleConvert = Format(CStr(hours), "00") & ":" & Format(CStr(minutes), "00") & _
          ":" & Format(CStr(seconds), "00")
      Else
        angleConvert = Angle
      End If
      ' end of short-to-long section
      
    Else
      ' it's long. If "Out", convert to short
      If Direction = "Out" Then
        seconds = Right(Angle, 2)
        seconds = seconds / 60 + 0.05
        seconds = CSng(Int(seconds * 10))
        
        If seconds > 9 Then
          ' round up minutes
          seconds = 0
          minutes = minutes + 1
          If minutes = 60 Then
            ' round up hours
            minutes = 0
            hours = hours + 1
            If hours > 23 Then
              hours = 0
            End If
          End If
        End If
        '  seconds = CStr(CInt(seconds / 60 + 0.05))
        angleConvert = Format(CStr(hours), "00") & ":" & Format(CStr(minutes), "00") & _
          "." & CStr(seconds)
      Else
        angleConvert = Angle
      End If
      
      ' force to long format 12/15/00
      angleConvert = Angle
      
    End If
    
  Else
    
    delimPos = InStr(Angle, "*")
    If delimPos > 0 Then
      ' it's Dec
      degrees = left(Angle, delimPos - 1)
      decSign = left(degrees, 1)
      If decSign = "+" Or decSign = "-" Then
        degrees = Right(degrees, Len(degrees) - 1)
        Angle = Right(Angle, Len(Angle) - 1)
      Else
        decSign = "+"
      End If
      'degrees = CInt(Mid(Angle, 1, 2))
      minutes = CInt(Mid(Angle, delimPos + 1, 2))
      
      delimPos = InStr(Angle, ":")
      If delimPos > 0 Then
        ' it's long. If "Out", convert to short
        If Direction = "Out" Then
          seconds = Right(Angle, 2)
          If seconds >= 30 Then
            minutes = minutes + 1
            If minutes > 59 Then
              minutes = 0
              degrees = degrees + 1
              If degrees > 359 Then
                degrees = 0
              End If
            End If
          End If
          If longitudeFlag Then
            angleConvert = Format(degrees, "000") & "*" & Format(minutes, "00")
          Else
            angleConvert = Format(degrees, "00") & "*" & Format(minutes, "00")
          End If
        Else
          angleConvert = Angle
        End If
      Else
        ' it's short. If "In", convert to long
        If Direction = "In" Then
          angleConvert = Angle & ".0"
        Else
          angleConvert = Angle
        End If
      End If
      '''''
      '      If decSign = "+" Then
      '        decSign = " "
      '      End If
      '''''
      If Not (longitudeFlag) Then
        angleConvert = decSign & angleConvert
      End If
      '''''' testing
      '  If frmMain.optParkPos(0).Value = True Or frmMain.optParkPos(2).Value = True Then
      '    angleConvert = "-90*00"
      '  End If
      '''''
    Else
      ' it's not recognizable RA or Dec. return it unchanged
      angleConvert = Angle
    End If
  End If
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule angleConvert")
  Resume Next
  
End Function

Public Sub validateRA(rA As String, fieldName As String, _
  currentControl As Control, valid As Boolean, Optional verbose As Boolean)

  ' verify that the format is xx:xx:xx
  
  Dim hours As Integer, minutes As Integer, seconds As Integer
  
  On Error GoTo errHandler

  valid = True
  
  If Len(rA) <> 8 Or Mid(rA, 3, 1) <> ":" Or Mid(rA, 6, 1) <> ":" Then
    If verbose Then MsgBox "Valid RA format is HH:MM:SS."
    valid = False
  Else
    
    hours = CInt(left(rA, 2))
    minutes = CInt(Mid(rA, 4, 2))
    seconds = CInt(Right(rA, 2))
    
    If hours < 0 Or hours > 23 Then
      If verbose Then MsgBox "Hours must be between 0 and 23."
      valid = False
    End If
    
    If minutes < 0 Or minutes > 59 Then
      If verbose Then MsgBox "Minutes must be between 0 and 59."
      valid = False
    End If
    
    If seconds < 0 Or seconds > 59 Then
      If verbose Then MsgBox "Seconds must be between 0 and 59."
      valid = False
    End If
    
  End If
  
  If Not (valid) Then
    If currentControl.Visible Then
'      currentControl.SetFocus
    End If
  End If
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule validateRA")
  Resume Next
  
End Sub

Public Sub validateRAshort(rA As String, fieldName As String, _
  currentControl As Control, valid As Boolean, Optional verbose As Boolean)

  ' verify that the format is HH:MM.T
  
  Dim hours As Integer, minutes As Single
  
  On Error GoTo errHandler

  valid = True
  
  If Len(rA) <> 7 Or Mid(rA, 3, 1) <> ":" Or Mid(rA, 6, 1) <> "." Then
    If verbose Then MsgBox "Valid RA format is HH:MM.T"
    valid = False
  Else
    
    hours = CInt(left(rA, 2))
    minutes = CSng(Right(rA, 4))
    
    If hours < 0 Or hours > 23 Then
      If verbose Then MsgBox "Hours must be between 0 and 23."
      valid = False
    End If
    
    If minutes < 0 Or minutes > 59.9 Then
      If verbose Then MsgBox "Minutes must be between 0 and 59.9."
      valid = False
    End If
    
  End If
  
  If Not (valid) Then
    If currentControl.Visible Then
'      currentControl.SetFocus
    End If
  End If
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule validateRAShort")
  Resume Next
  
End Sub

Public Sub validateDec(Dec As String, fieldName As String, _
  currentControl As Control, valid As Boolean, Optional verbose As Boolean)

  ' verify format is ±xx*xx:xx
  
  Dim degrees As Integer, minutes As Integer, seconds As Integer
  Dim maxMinSec As Integer
  
  On Error GoTo errHandler

  valid = True
  
  If left(Dec, 1) <> "+" And left(Dec, 1) <> "-" Then
    Dec = " " & Dec
  End If
  
  If Len(Dec) <> 9 Or Mid(Dec, 4, 1) <> "*" Or Mid(Dec, 7, 1) <> ":" Then
    If verbose Then MsgBox "Valid Dec format is ±DD*MM:SS "
    valid = False
  End If
  
  If valid Then
    degrees = CInt(Mid(Dec, 2, 2))
    minutes = CInt(Mid(Dec, 5, 2))
    seconds = CInt(Mid(Dec, 8, 2))
    
    If degrees < -90 Or degrees > 90 Then
      If verbose Then MsgBox "Degrees must be between -90 and 90."
      valid = False
    End If
    
    If degrees = 90 Or degrees = -90 Then
      ' disallow things like 90*12:34
      maxMinSec = 0
    Else
      maxMinSec = 59
    End If
    
    If minutes < 0 Or minutes > maxMinSec Then
      
      If maxMinSec = 59 Then
        If verbose Then MsgBox "Minutes must be between 0 and 59."
      Else
        If verbose Then MsgBox "Minutes must be 0 when degrees = 90 or -90."
      End If
      
      valid = False
      
    End If
    
    If seconds < 0 Or seconds > maxMinSec Then
      
      If maxMinSec = 59 Then
        If verbose Then MsgBox "Seconds must be between 0 and 59."
      Else
        If verbose Then MsgBox "Seconds must be 0 when degrees = 90 or -90."
      End If
      
      valid = False
    End If
    
  End If
  
  If Not (valid) Then
    If currentControl.Visible Then
    ' currentControl.SetFocus
    End If
  End If
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule validateDec")
  Resume Next
  
End Sub

Public Sub validateDecShort(Dec As String, fieldName As String, _
  currentControl As Control, valid As Boolean, Optional verbose As Boolean)

  ' verify format is ±xx*xx
  
  Dim degrees As Integer, minutes As Integer, seconds As Integer
  Dim maxMinSec As Integer
  
  On Error GoTo errHandler

  valid = True
  
  If left(Dec, 1) <> "+" And left(Dec, 1) <> "-" Then
    Dec = " " & Dec
  End If
  
  If Len(Dec) <> 6 Or Mid(Dec, 4, 1) <> "*" Then
    If verbose Then MsgBox "Valid Dec format is ±DD*MM "
    valid = False
  End If
  
  If valid Then
    degrees = CInt(Mid(Dec, 2, 2))
    minutes = CInt(Mid(Dec, 5, 2))
    
    If degrees < -90 Or degrees > 90 Then
      If verbose Then MsgBox "Degrees must be between -90 and 90."
      valid = False
    End If
    
    If degrees = 90 Or degrees = -90 Then
      ' disallow things like 90*12
      maxMinSec = 0
    Else
      maxMinSec = 59
    End If
    
    If minutes < 0 Or minutes > maxMinSec Then
      
      If maxMinSec = 59 Then
        If verbose Then MsgBox "Minutes must be between 0 and 59."
      Else
        If verbose Then MsgBox "Minutes must be 0 when degrees = 90 or -90."
      End If
      
      valid = False
      
    End If
  
  End If
  
  If Not (valid) Then
    If currentControl.Visible Then
    ' currentControl.SetFocus
    End If
  End If
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule validateDecShort")
  Resume Next
  
End Sub

Public Sub Delay(interval As Single, Optional doEventsFlag As Boolean)

  Dim start As Single
  
  On Error GoTo errHandler

  start = Timer
  
  Do While Timer < start + interval
    If doEventsFlag Then
      DoEvents
    End If
  Loop

  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule Delay")
  Resume Next
  
End Sub

Public Function convertRAtoHours(rA As String) As Double

  Dim hours As Integer, minutes As Single, seconds As Integer, result As Double
  
  On Error GoTo errHandler

  If Len(rA) = 8 Then
    hours = left(rA, 2)
    minutes = Mid(rA, 4, 2)
    seconds = Right(rA, 2)
  Else
    hours = left(rA, 2)
    minutes = Right(rA, 4)
    seconds = 0
  End If

  result = hours + minutes / 60 + seconds / 3600
  If result > 24 Then
    result = result - 24
  End If
  
  convertRAtoHours = result
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule convertRAtoHours")
  Resume Next
  
End Function

Public Function convertDecToDegrees(Dec As String) As Double

  Dim degrees As Integer, minutes As Integer, seconds As Integer
  Dim sign As String
  
  On Error GoTo errHandler

  sign = left(Dec, 1)
  If sign = "-" Or sign = "+" Then
    Dec = Right(Dec, Len(Dec) - 1)
  End If
  
  degrees = left(Dec, 2)
  minutes = Mid(Dec, 4, 2)
  seconds = Right(Dec, 2)
  
  convertDecToDegrees = degrees + minutes / 60 + seconds / 3600
  
  If sign = "-" Then
    convertDecToDegrees = convertDecToDegrees * -1
  End If
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule convertDecToDegrees")
  Resume Next
  
End Function

Public Function convertDecToDMS(Dec As Double) As String

  Dim degrees As Integer, minutes As Integer, seconds As Integer
  Dim tempDec As String, colonPos As Integer
  Dim negFlag As Boolean
  
  On Error GoTo errHandler

  If Dec < 0 Then
    negFlag = True
    Dec = Dec * -1
  End If
  
  degrees = Int(Dec)
  
  Dec = (Dec - degrees) * 60
  
  minutes = Int(Dec)
  
  Dec = (Dec - minutes) * 60
  seconds = CInt(Dec)
  
  If seconds > 59 Then
    seconds = seconds - 60
    minutes = minutes + 1
  End If
  
  If minutes > 59 Then
    minutes = minutes - 60
    degrees = degrees + 1
  End If
  
  If degrees > 90 Then
    degrees = 90
    minutes = 0
    seconds = 0
  End If
  
  '  If degrees < -90 Then
  '    degrees = -90
  '    minutes = 0
  '    seconds = 0
  '  End If
  
  tempDec = Format(degrees, "00") & "*" & Format(minutes, "00") _
    & ":" & Format(seconds, "00")
  
  If negFlag And tempDec <> "00*00:00" Then
    tempDec = "-" & tempDec
  Else
    tempDec = "+" & tempDec
  End If
  
  convertDecToDMS = tempDec
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule convertDecToDMS")
  Resume Next
  
End Function

Public Function convertDecToDMShort(Dec As String) As String
  ' takes +DD*MM:SS, returns +DD*MM
  
  Dim sign As String, degrees As Integer, minutes As Integer, seconds As Integer
  
  On Error GoTo errHandler

  sign = left(Dec, 1)
  degrees = CInt(Mid(Dec, 2, 2))
  minutes = CInt(Mid(Dec, 5, 2))
  seconds = CInt(Right(Dec, 2))
  
  If seconds > 29 Then
  
    ' round off to next minute
    minutes = minutes + 1
    
    If minutes > 59 Then
      
      minutes = 0
      degrees = degrees + 1
      
      If degrees > 90 Then
        degrees = 90
      End If
    
    End If
  
  End If
  
  convertDecToDMShort = sign & Format(degrees, "00") & "*" & Format(minutes, "00")
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule convertDecToDMShort")
  Resume Next
  
End Function
Public Function convertRAtoDegrees(rA As Double) As Double

  On Error GoTo errHandler

  convertRAtoDegrees = rA * 15
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule convertRAtoDegrees")
  Resume Next
  
End Function

Public Function convertRAtoHMS(rA As Double) As String

  Dim hours As Integer, minutes As Integer, seconds As Integer
  
  On Error GoTo errHandler

  hours = Int(rA)
  
  rA = (rA - hours) * 60
  ' now RA contains the minutes and seconds
  minutes = Int(rA)
  
  rA = (rA - minutes) * 60
  seconds = CInt(rA)
  
  If seconds > 59 Then
    seconds = seconds - 60
    minutes = minutes + 1
  End If
  
  If minutes > 59 Then
    minutes = minutes - 60
    hours = hours + 1
  End If
  
  If hours > 23 Then
    hours = hours - 24
  End If
  
  convertRAtoHMS = Format(hours, "00") & ":" _
    & Format(minutes, "00") & ":" _
    & Format(seconds, "00")
    
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule convertRAtoHMS")
  Resume Next
  
End Function

Public Function convertRAtoHMSshort(rA As String) As String
  ' takes HH:MM:SS, returns HH:MM.M
  
  Dim hours As String, minutes As String, seconds As String, sMinutes As Single
  
  On Error GoTo errHandler

  hours = left(rA, 2)
  minutes = Mid(rA, 4, 2)
  seconds = Right(rA, 2)
  sMinutes = CSng(minutes) + (CSng(seconds) / 60)
  If sMinutes >= 59.95 Then
    sMinutes = 0
    hours = hours + 1
    If hours > 23 Then
      hours = 0
    End If
  End If
  
  convertRAtoHMSshort = Format(hours, "00") & ":" & Format(sMinutes, "00.0")
    
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule convertRAtoHMSshort")
  Resume Next
  
End Function

Public Function convertRAdegreesToHours(rA As Double) As Double

  On Error GoTo errHandler

  If rA > 360 Then
    rA = rA - 360
  End If
  
  If rA < 0 Then
    rA = rA + 360
  End If
  
  convertRAdegreesToHours = rA / 15
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule convertRAdegreesToHours")
  Resume Next
  
End Function

Public Sub NthOrderRegression(D As Integer, N As Integer, _
                              X() As Double, Y() As Double, _
                              Intercept As String, slope As String, Stats As String)
                              
  ' d is the degree of the equation
  ' n is the number of known points
  ' x and y are the arrays of data points
  
  ' set limits on degree of equation to a(2d+1), r(d+1, d+2), t(d+2)
  ' it's currently set to a limit of six degrees
  Dim a(1 To 13) As Double
  Dim R(1 To 7, 1 To 8) As Double
  Dim T(1 To 8)
  
  Dim s As Double, Z As Double, P As Double, Q As Double
  Dim i As Integer, J As Integer, K As Integer
  Dim coeffdeterm As Double
  
  On Error GoTo errHandler

  If D > 6 Then
    Call showMsg("Currently limited to 6-degree equation.", "", 2)
    Exit Sub
  Else
    gblDegree = D
  End If
  
  a(1) = N
  
  ' clear gblR array
  For i = 1 To 7
    For J = 1 To 8
      gblR(i, J) = 0
    Next J
  Next i
  
  For i = 1 To N
    
   'Debug.Print "data points", i, x(i), y(i)
  
    ' populate matrices with a system of equations
    For J = 2 To 2 * D + 1
      a(J) = a(J) + X(i) ^ (J - 1)
     'Debug.Print J, "A(J)", A(J)
    Next J
    
    For K = 1 To D + 1
      R(K, D + 2) = T(K) + Y(i) * X(i) ^ (K - 1)
      T(K) = T(K) + Y(i) * X(i) ^ (K - 1)
      Debug.Print K, "R(K, D + 2), T(K)", R(K, D + 2), T(K)
    Next K
    
    T(D + 2) = T(D + 2) + Y(i) ^ 2
  
  Next i
  
  ' solve the system of equations in the matrices
  For J = 1 To D + 1
    For K = 1 To D + 1
      R(J, K) = a(J + K - 1)
      Debug.Print K, "r(j,k)", R(J, K)
    Next K
  Next J
  
  For J = 1 To D + 1
    K = J
280   If R(K, J) <> 0 Then GoTo 320
    K = K + 1
    If K <= D + 1 Then GoTo 280
    
'    frmTrain!txtStats.Text = "No unique solution."
'    frmTemp!txtIntercept.Text = ""
'    frmTemp!txtSlope.Text = ""
    
    Intercept = ""
    slope = ""
    Stats = "No unique solution."
    
    Exit Sub
    
320   For i = 1 To D + 2
      s = R(J, i)
      R(J, i) = R(K, i)
      R(K, i) = s
    Next i
    
    Z = 1 / R(J, J)
    
    For i = 1 To D + 2
      R(J, i) = Z * R(J, i)
    Next i
    
    For K = 1 To D + 1
      If K = J Then GoTo 470
      Z = R(K, J) * -1
      
      For i = 1 To D + 2
        R(K, i) = R(K, i) + Z * R(J, i)
      Next i
470   Next K
  Next J
  
'  Debug.Print "constant = "; R(1, D + 2)
  
  For J = 1 To D
    Debug.Print J; "degree coefficient = "; R(J + 1, D + 2)
  Next J
  
  Intercept = Int(R(1, D + 2) * 1000) / 1000
  slope = Int(R(2, D + 2) * 1000) / 1000
  
  P = 0
  For J = 2 To D + 1
    P = P + R(J, D + 2) * (T(J) - a(J) * T(1) / N)
  Next J
  Q = T(D + 2) - T(1) ^ 2 / N
  Z = Q - P
  i = N - D - 1
  
  If Q <> 0 Then
    
    coeffdeterm = P / Q
    
   'Debug.Print "coefficient of determination (r^2) = "; CDbl(Int(coeffdeterm * 10000)) / 10000
    If J >= 0 Then
     'Debug.Print "coefficient of correlation = "; CDbl(Int(Sqr(coeffdeterm) * 10000)) / 10000
    End If
    If i <> 0 Then
      If (Z / i) > 0 Then
       'Debug.Print "standard error of estimate = "; CDbl(Int(Sqr(Z / i) * 10000)) / 10000
      End If
    End If
    
    
    Stats = "coefficient of determination (r^2) = " & CDbl(Int(coeffdeterm * 10000)) / 10000 & vbCrLf
    If J >= 0 Then
      Stats = Stats & "coefficient of correlation = " & CDbl(Int(Sqr(coeffdeterm) * 10000)) / 10000 & vbCrLf
    End If
    If i <> 0 Then
      If (Z / i) > 0 Then
        Stats = Stats & "standard error of estimate = " & CDbl(Int(Sqr(Z / i) * 10000)) / 10000
      End If
    End If
'    frmTrain!txtStats.Text = Stats
    
    ' copy R array to gblR
    For i = 1 To 7
      For J = 1 To 8
        gblR(i, J) = R(i, J)
      Next J
    Next i
  
  End If

  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule NthOrderRegression")
  Resume Next
  
End Sub

Public Function computeY(X As Double)
' use global degree and coefficients to interpolate y based on x
  
  Dim D As Double, Y As Double, J As Integer
  
  On Error GoTo errHandler

  D = gblDegree
  
  ' The next four lines work for the auto coefficients at any degree. They're commented out
  ' to enable calculation based on the values in the text fields, so that the user may
  ' change those values manually.
'  y = gblR(1, D + 2)   '  intercept
'
'  For J = 1 To D
'    y = y + gblR(J + 1, D + 2) * x ^ J   '  slope
'  Next J
  
  If IsNumeric(frmTemp!txtIntercept.Text) And IsNumeric(frmTemp!txtSlope.Text) Then
   'Y = CDbl(frmTemp!txtIntercept.Text) + (CDbl(frmTemp!txtSlope.Text) * X)
    Y = gblIntercept + gblSlope * X
    computeY = Y
'  ElseIf IsNumeric(frmTemp!txtSlope.Text) Then
'    computeY = CDbl(frmTemp!txtSlope.Text) * X
  ElseIf IsNumeric(gblSlopeManual) Then
    computeY = CDbl(gblSlopeManual) * X
  Else
    computeY = 0
  End If
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule computeY")
  Resume Next
  
End Function

Public Function isFile(sPath As String) As Boolean

  On Error GoTo errHandler

  isFile = CBool(Len(Dir(sPath)))

  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule isFile")
  Resume Next
  
End Function

Public Sub showMsg(msgString As String, titleString As String, duration As Single, _
                   Optional xPos As Integer, Optional yPos As Integer)
' shows a window containing msgString for duration seconds, at xpos, ypos or centered

  Dim start As Single
  
  On Error GoTo errHandler

  frmMessage.Caption = titleString
  frmMessage!lblMessage.Caption = msgString
  
  If xPos = 0 Then
    frmMessage.left = Screen.width / 2 - frmMessage.width / 2
    frmMessage.top = Screen.height / 2 - frmMessage.height / 2
  Else
    frmMessage.left = xPos
    frmMessage.top = yPos
  End If
  
  frmMessage.Show
  start = Timer
  While Timer < start + duration
    DoEvents
  Wend
  Unload frmMessage
        
  Open App.Path & "\RFTrack.log" For Append As #1
  Print #1, Format(Date, "mm/dd/yy") & " " & _
                        Format(Time(), "hh:mm:ss") & " " & msgString
  Close #1
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("CommonModule showMsg")
  Resume Next
  
End Sub

Public Function degreesC(degreesF As Single) As Single
  ' converts Farenheit to Celsius
  
  On Error GoTo errHandler

  degreesC = (degreesF - 32) / 1.8
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule degreesC")
  Resume Next
  
End Function

Public Function degreesF(degreesC As Single) As Single
  ' converts Celsius to Farenheit
  
  On Error GoTo errHandler

  degreesF = (degreesC * 1.8) + 32  '  .5
  
  Exit Function
  
errHandler:
  Call unknownErrorHandler("CommonModule degreesF")
  Resume Next
  
End Function

