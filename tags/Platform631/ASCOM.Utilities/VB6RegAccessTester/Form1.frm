VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   3195
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4680
   LinkTopic       =   "Form1"
   ScaleHeight     =   3195
   ScaleWidth      =   4680
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command1 
      Caption         =   "Test Helper RegAccess"
      Height          =   615
      Left            =   1320
      TabIndex        =   0
      Top             =   1320
      Width           =   2175
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Command1_Click()
Dim x As Object
Dim sd As Scripting.Dictionary
Set x = CreateObject("DriverHelper.RegAccess")
Set sd = New Scripting.Dictionary
Dim MyName As String
MyName = "RegAccess Tester"
    'Set sd = x.EnumProfile("PeterKey2\Peterkey3")
    
    'For Each Key In sd
    '        Call LogMsg("Read profile peterKey3", "#" & Key & "#" & sd.Item(Key) & "#")
   ' Next
    
    'MsgBox("after read")
On Error Resume Next
    Call x.DeleteKey("PeterKey1", MyName)
    Call x.DeleteKey("PeterKey2", MyName)
On Error GoTo 0

    Call x.CreateKey("", MyName)
    'MsgBox("After createKey")
    Call x.WriteProfile("", "PlatformVersion", "5", MyName)
    'MsgBox("After write platformversion")
    Call x.WriteProfile("", "SerTraceFile", "", MyName)
    'x.WriteProfile("", "", "Default value")
    
    Set sd = x.EnumProfile("", MyName)
    For Each Key In sd
        Call LogMsg("ASCOM Root", "*" & Key & "*" & sd.Item(Key) & "*")
    Next

    If True Then
        Call x.CreateKey("PeterKey1", MyName)
        Call x.CreateKey("PeterKey2\PeterKey3", MyName)
        Call x.CreateKey("PeterKey2\PeterKey4", MyName)
        Call x.CreateKey("PeterKey2\PeterKey5", MyName)
        Call x.WriteProfile("PeterKey2\PeterKey3", "Latitude", "51", MyName)
        Call x.WriteProfile("PeterKey2\PeterKey3", "Longitude", "0", MyName)
        Call x.WriteProfile("PeterKey2\PeterKey3", "Elevation", "80", MyName)
        Call x.WriteProfile("PeterKey2\PeterKey3", "COMPort", "3", MyName)
        Call x.WriteProfile("PeterKey2\PeterKey3", "", "Default name value", MyName)

        Call LogMsg("GetProfile", "Latitude " & x.GetProfile("PeterKey2\PeterKey3", "Latitude", MyName))
        Call LogMsg("GetProfile", "Elevation " & x.GetProfile("PeterKey2\PeterKey3", "Elevation", MyName))

        Set sd = x.EnumProfile("PeterKey2\PeterKey3", MyName)
        For Each Key In sd
            Call LogMsg("EnumProfile", Key & " " & sd.Item(Key))
        Next

        Call x.DeleteProfile("PeterKey2\PeterKey3", "Elevation", MyName)
        Set sd = x.EnumProfile("PeterKey2\PeterKey3", MyName)
        For Each Key In sd
            Call LogMsg("EnumProfileAfterDelete", Key & " " & sd.Item(Key))
        Next

        Set sd = x.EnumKeys("PeterKey2", MyName)
        For Each Key In sd
            Call LogMsg("EnumProfile", "*" & Key & "*" & sd.Item(Key) & "*")
        Next

        Call x.DeleteKey("PeterKey2\PeterKey4", MyName)
        Set sd = x.EnumKeys("PeterKey2", MyName)
        For Each Key In sd
            Call LogMsg("EnumProfileDeleteKey", "*" & Key & "*" & sd.Item(Key) & "*")
        Next
    End If
    Set sd = Nothing
    MsgBox msg
    End

1000
msg = msg & Err.Number & " " & Err.Description & " at " & Err.HelpContext & " " & Err.Source
MsgBox msg

    
    
End Sub
Sub LogMsg(ByVal p1 As String, ByVal p2 As String)
'MsgBox p1 & " " & p2
msg = msg & vbCrLf & p1 & " " & p2
End Sub

Private Sub Form_Load()
Call Command1_Click
End Sub
