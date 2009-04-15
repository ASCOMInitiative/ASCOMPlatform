'
' Simple test of InprocServerCOM.VBSample
'
Dim S

Set S = CreateObject("InprocServerCOM.VB6Sample")
WScript.Echo "Using properties:"
S.X = 2
S.Y = 3
WScript.Echo "  X=" & S.X & " Y=" & S.Y & " Diagonal=" & S.Diagonal
WScript.Echo "Using CalculateDiagonal:"
WScript.Echo "  CalculateDiagonal(4,5)=" & S.CalculateDiagonal(4, 5)
WScript.Echo "Enum test:"
S.EnumTest = 1
WScript.Echo "  Enum value is " & S.EnumTest
