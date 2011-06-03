VERSION 5.00
Begin VB.Form frmCal 
   Caption         =   "Temperature Calibration"
   ClientHeight    =   1425
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4065
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   ScaleHeight     =   1425
   ScaleWidth      =   4065
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.OptionButton optTempUnits 
      Alignment       =   1  'Right Justify
      Caption         =   "Celsius"
      Height          =   225
      Index           =   1
      Left            =   2910
      TabIndex        =   5
      Top             =   450
      Width           =   975
   End
   Begin VB.OptionButton optTempUnits 
      Alignment       =   1  'Right Justify
      Caption         =   "Farenheit"
      Height          =   225
      Index           =   0
      Left            =   2910
      TabIndex        =   3
      Top             =   180
      Value           =   -1  'True
      Width           =   975
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "Cancel"
      Height          =   315
      Left            =   2970
      TabIndex        =   2
      Top             =   990
      Width           =   975
   End
   Begin VB.CommandButton cmdCal 
      Caption         =   "Calibrate"
      Default         =   -1  'True
      Height          =   315
      Left            =   1890
      TabIndex        =   1
      Top             =   990
      Width           =   975
   End
   Begin VB.TextBox txtTemp 
      Alignment       =   1  'Right Justify
      Height          =   255
      Left            =   2010
      TabIndex        =   0
      Top             =   150
      Width           =   375
   End
   Begin VB.Label Label2 
      Caption         =   "o"
      Height          =   195
      Left            =   2430
      TabIndex        =   6
      Top             =   90
      Width           =   135
   End
   Begin VB.Label Label1 
      Caption         =   "Enter Current Temperature, select Units, then Click ""Calibrate""."
      Height          =   825
      Left            =   150
      TabIndex        =   4
      Top             =   150
      Width           =   1545
   End
End
Attribute VB_Name = "frmCal"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdCal_Click()

  Dim cmdString As String, start As Single, degreesEntered As String

  On Error GoTo errHandler
  
  If Not IsNumeric(txtTemp.Text) Then
    Call showMsg("Please enter a temperature value.", "", 1)
    txtTemp.SetFocus
    Exit Sub
  End If
  
  ' get the current reading from the sensor
  cmdString = "FT000000"
  cmdString = appendChecksum(cmdString)
  Call sendCommand(cmdString, True)

  ' wait until the temp data appears
'  gblTemp = ""
  start = Timer
  While Timer < start + 1# And gblTemp = ""
    DoEvents
  Wend

  If gblTemp <> "" Then

'    If optTempUnits(0) = True Then
'      MsgBox txtTemp.Text & "°F. = " & degreesC(CSng(txtTemp.Text)) & "°C."
'    Else
'      MsgBox txtTemp.Text & "°C. = " & degreesF(CSng(txtTemp.Text)) & "°F."
'    End If
    
    degreesEntered = txtTemp.Text
    
    ' if farenheit is selected, convert to Celsius
    If optTempUnits(0) = True Then
      degreesEntered = CStr(degreesC(CSng(txtTemp.Text)))
    End If
    
    gblTempConvBeta = (degreesEntered + 273) / gblTemp
    Call iniPutData("Temp Comp", "TempConvBeta", CStr(gblTempConvBeta), glIniFile)
    
    ' update the temp window on the main form
    frmMain!txtTemp.Text = displayTemp(gblTemp) & " " & tempDisplayUnitsText
    
  ' Call showMsg("Calibrated." & vbCrLf & vbCrLf & "(Beta = " & gblTempConvBeta & ".)", "", 1)
    Call showMsg("Calibrated.", "", 1)
    
    ' enable Fahrenheit and Celcius
    frmTemp!mnuTempUnits(1).Enabled = True
    frmTemp!mnuTempUnits(2).Enabled = True
    
    Unload Me
  End If
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmCal cmdCal_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub cmdCancel_Click()

  On Error GoTo errHandler
  
  Unload Me
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmCal cmdClose_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next

End Sub

Private Sub Form_Load()

  On Error GoTo errHandler
  
  Call setFormPositionIni(Me, "0 2000 4185 1830")
  
Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmCal Form_Load: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub Form_Unload(Cancel As Integer)
  
  On Error GoTo errHandler
  
  If gblShowingCal = True Then
    frmTemp!mnuViewCal.Checked = False
    gblShowingCal = False
  End If
  
  Call saveFormPosition(Me)
  
  If gblShowingCal = True Then
    Me.Hide
    Cancel = 1
  End If

Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmCal Form_Unload: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub
