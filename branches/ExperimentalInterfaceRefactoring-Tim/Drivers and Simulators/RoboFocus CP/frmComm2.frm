VERSION 5.00
Object = "{648A5603-2C6E-101B-82B6-000000000014}#1.1#0"; "mscomm32.ocx"
Begin VB.Form frmComm 
   Caption         =   "Comm Settings"
   ClientHeight    =   1560
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   5115
   Icon            =   "frmComm2.frx":0000
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1560
   ScaleWidth      =   5115
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Tag             =   "ForeVB DB=C:\Program Files\Microsoft Visual Studio\VB98\Projects\Focuser\Focuser.dba"
   WhatsThisButton =   -1  'True
   WhatsThisHelp   =   -1  'True
   Begin VB.ComboBox comboStopBits 
      Height          =   315
      ItemData        =   "frmComm2.frx":0442
      Left            =   2760
      List            =   "frmComm2.frx":044F
      Style           =   2  'Dropdown List
      TabIndex        =   11
      Top             =   1080
      Width           =   945
   End
   Begin VB.ComboBox comboParity 
      Height          =   315
      ItemData        =   "frmComm2.frx":045C
      Left            =   1470
      List            =   "frmComm2.frx":0469
      Style           =   2  'Dropdown List
      TabIndex        =   9
      Top             =   1080
      Width           =   945
   End
   Begin VB.ComboBox comboDataBits 
      Height          =   315
      ItemData        =   "frmComm2.frx":047E
      Left            =   120
      List            =   "frmComm2.frx":0488
      Style           =   2  'Dropdown List
      TabIndex        =   7
      Top             =   1080
      Width           =   945
   End
   Begin VB.ComboBox comboBaud 
      Height          =   315
      ItemData        =   "frmComm2.frx":0492
      Left            =   1470
      List            =   "frmComm2.frx":04A8
      Style           =   2  'Dropdown List
      TabIndex        =   5
      Top             =   360
      Width           =   945
   End
   Begin VB.ComboBox comboComm 
      Height          =   315
      ItemData        =   "frmComm2.frx":04D0
      Left            =   120
      List            =   "frmComm2.frx":04EC
      Style           =   2  'Dropdown List
      TabIndex        =   3
      Top             =   360
      Width           =   945
   End
   Begin VB.CommandButton closePort 
      Caption         =   "Close Port"
      Enabled         =   0   'False
      Height          =   495
      Left            =   4290
      TabIndex        =   1
      Top             =   900
      Width           =   705
   End
   Begin VB.CommandButton OpenPort 
      BackColor       =   &H0000FF00&
      Caption         =   "Open Port"
      Height          =   495
      Left            =   4290
      TabIndex        =   0
      Top             =   390
      Width           =   705
   End
   Begin VB.CheckBox Docked 
      Caption         =   "Docked"
      Height          =   255
      Left            =   4230
      TabIndex        =   2
      Top             =   -30
      Value           =   1  'Checked
      Width           =   870
   End
   Begin MSCommLib.MSComm commControl 
      Left            =   3240
      Top             =   90
      _ExtentX        =   1005
      _ExtentY        =   1005
      _Version        =   393216
      DTREnable       =   -1  'True
   End
   Begin VB.Label Label5 
      Caption         =   "Stop Bits"
      Height          =   195
      Left            =   2760
      TabIndex        =   12
      Top             =   840
      Width           =   1095
   End
   Begin VB.Label Label4 
      Caption         =   "Parity"
      Height          =   195
      Left            =   1470
      TabIndex        =   10
      Top             =   840
      Width           =   1095
   End
   Begin VB.Label Label3 
      Caption         =   "Data Bits"
      Height          =   195
      Left            =   120
      TabIndex        =   8
      Top             =   840
      Width           =   1095
   End
   Begin VB.Label Label2 
      Caption         =   "Baud Rate"
      Height          =   195
      Left            =   1470
      TabIndex        =   6
      Top             =   120
      Width           =   1095
   End
   Begin VB.Label Label1 
      Caption         =   "Comm Port"
      Height          =   195
      Left            =   120
      TabIndex        =   4
      Top             =   120
      Width           =   1095
   End
End
Attribute VB_Name = "frmComm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Option Explicit

Dim baudRateSetting As String, paritySetting As String
Dim dataBitsSetting As String, stopBitsSetting As String

Public Sub closePort_Click()

  Dim currentCom As String, newCaption As String
  Dim i As Integer, dashPos As Integer

  On Error GoTo errHandler
  
  currentCom = IniGetData("Comm", "commport", "1", glIniFile)
  
  If commControl.PortOpen = True Then
  
    If autoCorrect = True Then
      Call showMsg("Stop Auto-correct before closing port.", "", 2)
    Else
      
      commControl.PortOpen = False
      
      OpenPort.Enabled = True
      closePort.Enabled = False
      
      Call enableButtons(False)
      Call enableTCButtons
      
      frmMain!openClosePort.Caption = "COM " & currentCom
      
      Call releaseHardware
    
    End If
    
  End If
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("Comm - closePort_Click")
  Resume Next
  
End Sub

Private Sub comboComm_Click()

  On Error GoTo errHandler
  
  commPortSetting = Right(comboComm.Text, 1)
  Call iniPutData("Comm", "CommPort", CStr(commPortSetting), glIniFile)
  
  frmMain.openClosePort.Caption = "COM " & commPortSetting
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmComm ComboComm_Click")
  Resume Next
  
End Sub

Private Sub comboBaud_Click()

  On Error GoTo errHandler
  
  baudRateSetting = comboBaud.Text
  Call iniPutData("Comm", "BaudRate", baudRateSetting, glIniFile)
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmComm ComboBaud_Click")
  Resume Next
  
End Sub

Private Sub comboDataBits_Click()

  On Error GoTo errHandler
  
  dataBitsSetting = comboDataBits.Text
  Call iniPutData("Comm", "DataBits", dataBitsSetting, glIniFile)
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmComm ComboDataBits_Click")
  Resume Next
  
End Sub

Private Sub comboParity_Click()

  On Error GoTo errHandler
  
  paritySetting = left(comboParity.Text, 1)
  Call iniPutData("Comm", "Parity", paritySetting, glIniFile)
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmComm ComboParity_Click")
  Resume Next
  
End Sub

Private Sub comboStopBits_Click()

  On Error GoTo errHandler
  
  stopBitsSetting = comboStopBits.Text
  Call iniPutData("Comm", "StopBits", stopBitsSetting, glIniFile)
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmComm stopBits_CLick")
  Resume Next
  
End Sub

Private Sub Form_Load()

  Dim iniParity As String
  Dim left As Integer, top As Integer, width As Integer, height As Integer
  
  On Error GoTo errHandler
  
  Docked.Visible = False
  
  Call setFormPositionIni(Me, "0 2000 5235 1965")
  
  ' get the default comm settings from the INI file
  baudRateSetting = IniGetData("Comm", "BaudRate", 0, glIniFile)
  comboBaud.Text = baudRateSetting
  
  paritySetting = IniGetData("Comm", "Parity", "N", glIniFile)

  Select Case paritySetting
    Case "N", "n"
      iniParity = "None"
    Case "E", "e"
      iniParity = "Even"
    Case "O", "o"
      iniParity = "Odd"
  End Select

  comboParity.Text = iniParity
  
  dataBitsSetting = IniGetData("Comm", "DataBits", 0, glIniFile)
  comboDataBits.Text = dataBitsSetting
  
  stopBitsSetting = IniGetData("Comm", "StopBits", 0, glIniFile)
  comboStopBits.Text = stopBitsSetting
  
  commPortSetting = IniGetData("Comm", "CommPort", 0, glIniFile)
  comboComm.Text = "COM" & commPortSetting
  
  commPortPrevSetting = commPortSetting
  
  On Error GoTo 0
  
  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmComm Form_Load")
  Resume Next

End Sub

Private Sub Form_Unload(Cancel As Integer)

  On Error GoTo errHandler
  
  If commControl.PortOpen = True Then
    frmComm.Hide
    Cancel = True
  Else
    Call saveFormPosition(Me)
  End If
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmComm Form_Unload: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Sub openPort_Click()
  Dim cmdResponse As String
  Dim start As Single
  Dim powerStatus As String
  Dim i As Integer
  Dim currentCom As String, newCaption As String
  
  On Error GoTo errHandler
  
  gblOpeningCom = True  '  flag to stop frmMain_Unload until comm is finished opening
  
  ' don't allow another click til we're done with this one
  OpenPort.Enabled = False
  frmMain!openClosePort.Enabled = False
  
  currentCom = IniGetData("Comm", "commport", "1", glIniFile)
  
  Call setCommPort
  Call findHardware
  
  If gblMotorConfigCommand <> "" Then
    
    ' motor config was entered when port was closed - send it now
    Call sendCommand(gblMotorConfigCommand, True)
    gblMotorConfigCommand = ""
    
  End If
  
  ' get temperature string
  cmdString = "FT000000"
  cmdString = appendChecksum(cmdString)
  Call sendCommand(cmdString, True)
  
  frmMain!openClosePort.Enabled = True
  cmdActive = False
  
  On Error GoTo 0
  
  gblOpeningCom = False
  
  Exit Sub
  
errHandler:
  If Err.Number <> 8012 Then
    Call unknownErrorHandler("frmComm OpenPort_Click")
  End If
  
  Resume Next

End Sub

Private Sub commControl_OnComm()

  Dim s As String, lastChar As String, formattedS As String
  Dim powerStatus As String, i As Integer
  Dim pauseTime As Integer, start As Integer
  Dim starttime As Single, endtime As Integer
  
  On Error GoTo errHandler
  
  Select Case commControl.CommEvent
      
    Case comEvSend
'      s = commControl.Input
'
'      If Asc(s) = 13 Then s = vbCrLf
'
'      ' truncate to last 1000 characters
'      formattedS = Replace(s, Chr(0), "*")
'      frmShow!txtSent.Text = Right(frmShow!txtSent.Text & formattedS, 1000)
'      ' put cursor at end of text
'      frmShow!txtSent.SelStart = Len(frmShow!txtSent.Text)
'
'      Call checkFirmware(s)
      
    Case comEvReceive
      
      waitingForComm = False
      
      ' append the latest serial data
      s = commControl.Input
      s = Replace(s, "ß", "*")
      commInputString = commInputString & s
      
      Call checkFirmware(s)
      commReceivedString = s
      
      charReceived = True
  
  End Select

  Exit Sub
  
errHandler:
  Call unknownErrorHandler("frmComm commControl_OnComm")
  Resume Next
  
End Sub

Private Sub setCommPort()

  Dim settingMsg As String
  Dim temp As String
  
  On Error GoTo commError
  
  If commControl.PortOpen = True Then
    commControl.PortOpen = False
  End If
  
  commPortPrevSetting = commControl.CommPort
  commControl.CommPort = commPortSetting
  
  commControlPrevSettings = commControl.Settings
  
  temp = baudRateSetting & "," & paritySetting & "," & _
    dataBitsSetting & "," & stopBitsSetting
  commControl.Settings = temp
  
  commControl.InputLen = 0
  commControl.RThreshold = 1
  
  commControl.PortOpen = True
  
  Exit Sub
  
commError:
  commErrorHandler
  Resume Next
End Sub

