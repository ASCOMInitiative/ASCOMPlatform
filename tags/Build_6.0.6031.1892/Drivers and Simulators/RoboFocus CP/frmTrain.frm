VERSION 5.00
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "MSFLXGRD.OCX"
Begin VB.Form frmTrain 
   Caption         =   "Training"
   ClientHeight    =   5505
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4095
   Icon            =   "frmTrain.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   5505
   ScaleWidth      =   4095
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraTrain 
      Caption         =   " Training Controls "
      Height          =   1275
      Left            =   120
      TabIndex        =   8
      Top             =   60
      Width           =   3855
      Begin VB.TextBox txtPos 
         Alignment       =   1  'Right Justify
         Height          =   285
         Left            =   2700
         TabIndex        =   14
         Top             =   870
         Width           =   795
      End
      Begin VB.TextBox txtTemp 
         Alignment       =   1  'Right Justify
         Height          =   285
         Left            =   1860
         TabIndex        =   13
         Top             =   870
         Width           =   855
      End
      Begin VB.TextBox txtTime 
         Height          =   285
         Left            =   1050
         TabIndex        =   12
         Top             =   870
         Width           =   825
      End
      Begin VB.TextBox txtDate 
         Height          =   285
         Left            =   240
         TabIndex        =   11
         Top             =   870
         Width           =   825
      End
      Begin VB.CommandButton cmdApplyData 
         Caption         =   "Add to Dataset"
         Enabled         =   0   'False
         Height          =   315
         Left            =   1920
         TabIndex        =   10
         Top             =   270
         Width           =   1365
      End
      Begin VB.CommandButton cmdGetTemp 
         Caption         =   "Get Data Point"
         Enabled         =   0   'False
         Height          =   315
         Left            =   390
         TabIndex        =   9
         Top             =   270
         Width           =   1365
      End
      Begin VB.Label lblEdit 
         AutoSize        =   -1  'True
         BackColor       =   &H00E0E0E0&
         Height          =   195
         Left            =   270
         TabIndex        =   15
         Top             =   660
         Visible         =   0   'False
         Width           =   45
      End
   End
   Begin VB.TextBox txtStats 
      Height          =   675
      Left            =   120
      MultiLine       =   -1  'True
      TabIndex        =   4
      Top             =   4710
      Width           =   3855
   End
   Begin VB.TextBox txtNotes 
      Height          =   495
      Left            =   120
      MultiLine       =   -1  'True
      TabIndex        =   3
      Top             =   3900
      Width           =   3855
   End
   Begin VB.ComboBox cmbSort 
      BeginProperty Font 
         Name            =   "Small Fonts"
         Size            =   6.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   285
      Left            =   2730
      TabIndex        =   2
      Top             =   1470
      Width           =   1215
   End
   Begin MSFlexGridLib.MSFlexGrid gData 
      Height          =   1785
      Left            =   120
      TabIndex        =   0
      Top             =   1770
      Width           =   3855
      _ExtentX        =   6800
      _ExtentY        =   3149
      _Version        =   393216
      Rows            =   1000
      Cols            =   7
      FixedCols       =   0
      ScrollTrack     =   -1  'True
      ScrollBars      =   2
      FormatString    =   "<X|<Date        |<Time         |>       Temp.|>   Position|>                                                         |>        "
   End
   Begin VB.Label Label3 
      AutoSize        =   -1  'True
      BackColor       =   &H00E0E0E0&
      Caption         =   " Stats "
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
      Left            =   150
      TabIndex        =   7
      Top             =   4500
      Width           =   390
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackColor       =   &H00E0E0E0&
      Caption         =   " Sort Order "
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
      Left            =   1920
      TabIndex        =   6
      Top             =   1530
      Width           =   705
   End
   Begin VB.Label Label1 
      AutoSize        =   -1  'True
      BackColor       =   &H00E0E0E0&
      Caption         =   " Dataset Notes "
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
      Left            =   150
      TabIndex        =   5
      Top             =   3690
      Width           =   975
   End
   Begin VB.Label Label10 
      AutoSize        =   -1  'True
      BackColor       =   &H00E0E0E0&
      Caption         =   " Temp. Comp. Dataset "
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
      Left            =   120
      TabIndex        =   1
      Top             =   1530
      Width           =   1425
   End
End
Attribute VB_Name = "frmTrain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmbSort_Click()
  
  Dim sortType As Integer
  
  On Error GoTo errHandler
  
  If gblValidDataSet Then
    ' unhighlight if necessary
    Call highlightRow(selectedRow, False)
    
    With gData
    ' sort the grid
    Select Case cmbSort.ListIndex
      
      Case 0  ' unsorted
        sortType = flexSortNumericAscending
        .Col = 6  '  original position
        
      Case 1  ' asc date=time
        sortType = flexSortGenericAscending
        .Col = 5
        
      Case 2  ' asc temp
        sortType = flexSortNumericAscending
        .Col = 3
        
      Case 3  ' asc pos
        sortType = flexSortNumericAscending
        .Col = 4
        
      Case 4  ' desc date=time
        sortType = flexSortGenericDescending
        .Col = 5
        
      Case 5  ' desc temp
        sortType = flexSortNumericDescending
        .Col = 3
        
      Case 6  ' desc pos
        sortType = flexSortNumericDescending
        .Col = 4
        
    End Select
    
    .Row = 1
    .RowSel = gblNextRow - 1
    .Sort = sortType
    .RowSel = 1
    
    End With
  End If

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTrain cmbSort_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub cmdApplyData_Click()
  
  Dim lineDate As String, lineTime As String
  
  On Error GoTo errHandler
  
  If autoCorrect Then
    Exit Sub
  End If
  
  With gData
  
    If tableInsertFlag = False Then
      
      ' append
      gData.Rows = gData.Rows + 1
      
      If gblNextRow = 0 Then
        gblNextRow = 1
      End If
      .Row = gblNextRow
      
    End If
    
    .Col = 1
    .Text = txtDate.Text
    
    .Col = 2
    .Text = txtTime.Text
    
    .Col = 3
    trainTempArray(.Row, 3) = convertToRaw(txtTemp.Text)
    .Text = displayTemp(CStr(trainTempArray(.Row, 3)))
'    trainTempArray(.Row, 3) = gblTemp
'    .Text = displayTemp(CStr(gblTemp))

    .Col = 4
    .Text = txtPos.Text
    
    ' store the date and time in a format for sorting
    lineDate = Format(txtDate.Text, "yyyymmdd")
    lineTime = Format(txtTime.Text, "hhmmss")
    .Col = 5
    .Text = lineDate & lineTime

    ' store the position for restoring original order
    .Col = 6
    .Text = .Row
    
    If .Row = gblNextRow Then
      gblNextRow = gblNextRow + 1
    End If
  
    ' keep the last reading in view
    If gblNextRow > 3 Then
      .TopRow = gblNextRow - 3
    End If
  
  End With
  
  unsavedData = True
  
  doCompute
  
  txtDate.Text = ""
  txtTime.Text = ""
  txtTemp.Text = ""
  txtPos.Text = ""

  cmdApplyData.Enabled = False

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTrain cmdApplyData_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub cmdGetTemp_Click()
  
  Dim start As Single
  
  On Error GoTo errHandler
  
  If autoCorrect Then
    Exit Sub
  End If
  
'  If gblTempConvBeta = 0 Then
'    Call showMsg("You must calibrate before getting data points.", "", 2)
'    Exit Sub
'  End If

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
    
    txtTemp.Text = displayTemp(gblTemp)
  ' frmMain!txtTemp.Text = displayTemp(gblTemp) & " " & tempDisplayUnitsText
    trainEditVal = gblTemp
    
    ' fill the other edit boxes
    txtDate.Text = Format(Date, "mm/dd/yy")
    txtTime.Text = Format(Time, "hh:mm:ss")
    txtPos.Text = absolutePos
      
    ' fill the formula temp field
    calcEditVal = trainEditVal
    frmTemp!txtEnterTemp.Text = displayTemp(CStr(calcEditVal))
    frmTemp!txtCalcPos.Text = Int(computeY(CDbl(calcEditVal)))
    
    tableInsertFlag = False  '  append to table
    
    lblEdit.Visible = True
    lblEdit.Caption = " Latest Data from RoboFocus "
    
    cmdApplyData.Enabled = True
  
  End If
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTrain cmdGetTemp_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub Form_Load()

  On Error GoTo errHandler
  
  Call setFormPositionIni(Me, "0 2000 4185 5970")

  With cmbSort
  .AddItem "Unsorted"
  .AddItem "Asc. Date"
  .AddItem "Asc. Temp"
  .AddItem "Asc. Pos"
  .AddItem "Desc. Date"
  .AddItem "Desc. Temp"
  .AddItem "Desc. Pos"
  .ListIndex = 0
  End With
  
  If gblValidDataSet = True Then
    gData.Rows = maxRows
    
    ' display data
      Open initDir & defFile For Input As #2
      Call loadTCdata
      Close #2
    
  Else
    gData.Rows = 2
    gblNextRow = 1
    
  End If
  
  ' if comm port is open, enable "Get Data" button
  If frmComm!commControl.PortOpen = True Then
    cmdGetTemp.Enabled = True
  Else
    cmdGetTemp.Enabled = False
  End If
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTrain Form_Load: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub Form_Paint()

  Dim boxMargin As Integer, winCoords() As String
  
  ' draw a box around the edge
  ' left, top, width, height
  winCoords = Split(dfltWin(7))
  
  boxMargin = 0

  Me.FillStyle = 1
  Me.Line (boxMargin, boxMargin)- _
                (winCoords(2) - boxMargin - 125, winCoords(3) - boxMargin - 405), Gray, B

End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
  
  Dim response
  
  On Error GoTo errHandler
  
  If unsavedData Then
    response = MsgBox("You haven't saved your Temperature Compensation Data." & vbCrLf & _
                                        "Save before exiting?", vbYesNoCancel + vbExclamation)
    If response = vbCancel Then
      Cancel = 1
      gblStopUnload = True
      Exit Sub
    End If
    
    If response = vbYes Then
      frmTemp.mnuSaveAs_Click
    Else
      unsavedData = False
      gblValidDataSet = False
    End If
  End If

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTrain Form_QueryUnload: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Public Sub Form_Unload(Cancel As Integer)
  
  On Error GoTo errHandler
  
  If gblShowingTraining = True Then
    frmTemp!mnuViewTraining.Checked = False
    gblShowingTraining = False
  End If
  
  Call saveFormPosition(Me)
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTrain Form_Unload: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub gData_Click()
  
  Dim saveRow As Integer, saveCol As Integer, C As Integer, sortCol As Integer

  On Error GoTo errHandler
  
  If autoCorrect Then
    Exit Sub
  End If
  
  With gData
  If .Row <= gblNextRow Then
    
    If .Col = 0 Then
      
      ' it's the Flag column; toggle the X
      If .Text = "" Then
        .Text = "X"
      Else
        .Text = ""
      End If
      ' and recompute least squares
      doCompute
      
      unsavedData = True
      
    Else
      ' it's a data column
      ' unhighlight the previously-selected line
      If selectedRow > 0 Then
        saveRow = .Row
        Call highlightRow(selectedRow, False)
        .Row = saveRow
      End If
      
      ' copy data to edit boxes
      ' save the clicked column
      saveCol = .Col
      .Col = 1
      txtDate.Text = gData.Text
      
      .Col = 2
      txtTime.Text = gData.Text
      
      .Col = 3
      trainEditVal = trainTempArray(.Row, 3)
      txtTemp.Text = displayTemp(CStr(trainEditVal))
      gblTemp = trainEditVal
      
      .Col = 4
      txtPos.Text = gData.Text
      
      .Col = saveCol
      selectedRow = gData.Row
      
      If selectedRow > 0 Then
        Call highlightRow(selectedRow, True)
      End If
      
      tableInsertFlag = True  '  insert (replace edited line)
      
      lblEdit.Visible = True
      lblEdit.Caption = " Edit Data from Dataset "
      
      cmdApplyData.Enabled = True
      
    End If

  End If
  End With
  
  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTrain gData_Click: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub gData_KeyDown(KeyCode As Integer, Shift As Integer)

  Dim response, R As Integer, RR As Integer
  Dim saveX As String, saveDate As String, saveTime As String, saveTemp As String, saveFocus As String
  
  On Error GoTo errHandler
  
  If KeyCode = 46 Then    '  delete key
    If selectedRow > 0 And selectedRow < gblNextRow Then
      ' remove highlighting and clear edit boxes
      Call highlightRow(selectedRow, False)
      txtDate.Text = ""
      txtTime.Text = ""
      txtTemp.Text = ""
      txtPos.Text = ""
      
      gData.Rows = gData.Rows + 1
      ' delete selected line by moving each following line up one
      For R = selectedRow + 1 To gblNextRow
        
        gData.Row = R
        gData.Col = 0
        saveX = gData.Text
        gData.Col = 1
        saveDate = gData.Text
        gData.Col = 2
        saveTime = gData.Text
        gData.Col = 3
        saveTemp = gData.Text
        gData.Col = 4
        saveFocus = gData.Text
        
        gData.Row = R - 1
        gData.Col = 0
        gData.Text = saveX
        gData.Col = 1
        gData.Text = saveDate
        gData.Col = 2
        gData.Text = saveTime
        gData.Col = 3
        gData.Text = saveTemp
        If saveTemp <> "" Then
          trainTempArray(gData.Row, 3) = CSng(convertToRaw(saveTemp))
        End If
        gData.Col = 4
        gData.Text = saveFocus
            
      Next R
      
      gData.Rows = gData.Rows - 2

      gblNextRow = gblNextRow - 1
      If gblNextRow = 0 Then
        gblNextRow = 1
      End If
      
      unsavedData = True
      
      selectedRow = -1
      
      doCompute
    
    End If
  End If

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTrain gData_KeyDown: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub

Private Sub txtNotes_Change()
  
  On Error GoTo errHandler
  
  unsavedData = True

  Exit Sub
  
errHandler:
  
  Call showMsg("Error in frmTrain txtNotes_Change: " & Err.Description, "", 3)
  
  On Error GoTo 0
  Resume Next
  
End Sub
