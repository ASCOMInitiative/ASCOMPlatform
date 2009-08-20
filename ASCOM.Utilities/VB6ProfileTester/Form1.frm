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
Dim x As Profile
Dim sd As Scripting.Dictionary
On Error GoTo 1000

10 Set x = New Profile
20 Set sd = New Scripting.Dictionary
30 Dim MyName As String
40 MyName = "ProfileTest"
45 MsgBox "Before x.Devicetype"
50    x.DeviceType = "Telescope"

      Dim r As Boolean
      MsgBox "Before IsRegistered"
52      r = x.IsRegistered(MyName)
MsgBox "Is Registered " & r
55    MsgBox "Before x.Register"
60    Call x.Register(MyName, "Profile test telescope")
65    MsgBox "Before x.WriteValue"
70    Call x.WriteValue(MyName, "AName", "A Value")
75    MsgBox "Before x.CreateSubKey"
80    Call x.CreateSubKey(MyName, "ASubKey")

Dim s As String
MsgBox "Gefore x.getvalue"
85 s = x.GetValue(MyName, "NonExistentValue")
MsgBox "After x.getvalue"

90    Set sd = x.Values("ScopeSim.Telescope")
100    For Each Key In sd
110        Call LogMsg("ASCOM Root", "*" & Key & "*" & sd.Item(Key) & "*")
120    Next

122    Set sd = x.SubKeys("ScopeSim.Telescope")
124     For Each Key In sd
126        Call LogMsg("ScopeSim SubKeys", "*" & Key & "*" & sd.Item(Key) & "*")
128    Next

130    Set sd = Nothing
140    MsgBox msg
150    Call x.Unregister(MyName)
160    Set x = Nothing
170    End

1000 msg = msg & vbCrLf & Err.Number & " " & Err.Description & " at " & Erl
1010 MsgBox msg

    
    
End Sub
Sub LogMsg(ByVal p1 As String, ByVal p2 As String)
'MsgBox p1 & " " & p2
msg = msg & vbCrLf & p1 & " " & p2
End Sub

Private Sub Form_Load()
Call Command1_Click
End Sub
