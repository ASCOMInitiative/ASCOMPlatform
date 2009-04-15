Set B = CreateObject("BaseCOM.Test")
B.Name = "Joe Smith"
B.Age = 46
If B.PopUp("How are you?") Then
    WScript.Echo "He answered Yes"
Else
    WScript.Echo "He answered No"
End If
