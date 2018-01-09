Attribute VB_Name = "Module1"
Public Declare Function unuran_init Lib "unuran-0.6.0.dll" (ByVal s As String) As Long
Public Declare Function unuran_sample Lib "unuran-0.6.0.dll" () As Double
Public Declare Function unuran_set_seed Lib "unuran-0.6.0.dll" (ByVal seed As Long) As Long

