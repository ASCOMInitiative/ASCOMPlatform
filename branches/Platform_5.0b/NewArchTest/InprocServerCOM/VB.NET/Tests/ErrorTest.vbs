'
' Error tests of InprocServerCOM.VBSample
'
Dim S, X, D

Set S = CreateObject("ASCOM.InprocServerCOM.VBSample")
WScript.Echo "Retrieve X before setting it:"
On Error Resume Next
X = S.X
If Err.Number <> 0 Then
    WScript.Echo "  Raised error " & Err.Number & " " & Err.Description
Else
    WScript.Echo "X=" & S.X
End If
Err.Clear
S.Y = 3
WScript.Echo "Get diagonal without setting X:"
D = S.Diagonal
If Err.Number <> 0 Then
    WScript.Echo "  Raised error " & Err.Number & " " & Err.Description
Else
    WScript.Echo "  Diagonal=" & S.Diagonal
End If

