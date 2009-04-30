'
' Simple test of LocalServerCOM.CSharpSample
'
Dim S

Set S = CreateObject("ASCOM.LocalServerCOM.CSharpSample")
WScript.Echo "Using properties:"
S.X = 2
S.Y = 3
WScript.Echo "  X=" & S.X & " Y=" & S.Y & " Diagonal=" & S.Diagonal
WScript.Echo "Using CalculateDiagonal:"
WScript.Echo "  CalculateDiagonal(4,5)=" & S.CalculateDiagonal(4, 5)
