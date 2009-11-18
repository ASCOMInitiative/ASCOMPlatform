Module Program

	Sub Main()
		Dim ans As String
		Dim progId As String
		Dim O As Object
		Dim S As IAscomSample
		Dim isCOM As Boolean

		Console.WriteLine("Early binding to .NET or to COM:")
		Console.WriteLine("")
		Do
			Console.WriteLine("-----------------------------------------------------")
			Console.WriteLine("Pick one of the components that supports IAscomSample")
			Console.WriteLine("  1 - ASCOM.InprocServerCOM.CSharpSample (.NET)")
			Console.WriteLine("  2 - ASCOM.InprocServerCOM.VBSample (.NET)")
			Console.WriteLine("  3 - ASCOM.LocalServerCOM.CSharpSample (COM)")
			Console.WriteLine("  4 - InprocServerCOM.VB6Sample (COM)")
			Console.Write("Enter the number (1-4, 0 to exit):")
			ans = Console.ReadLine()
			Select Case ans
				Case "0" : Exit Sub
				Case "1" : progId = "ASCOM.InprocServerCOM.CSharpSample"
				Case "2" : progId = "ASCOM.InprocServerCOM.VBSample"
				Case "3" : progId = "ASCOM.LocalServerCOM.CSharpSample"
				Case "4" : progId = "InprocServerCOM.VB6Sample"
				Case Else : progId = ""
			End Select

			If progId = "" Then
				Console.WriteLine("Not 1-4, re-run and try again")
				Console.Write("Press enter to quit...")
				Console.ReadLine()
				Exit Sub
			End If

			Try
				'
				' The magic is here: This talks direct to the .NET implementation of
				' the two inproc servers, and via COM to the LocalServer and the 
				' VB6 inproc server! This demonstrates interface-strict use of 
				' both native .NET and COM components!
				'
				' (below is to determine if COM used)
				O = CreateObject(progId)
				S = O
				isCOM = O.GetType().Name.Equals("__ComObject")
				'
				Console.WriteLine("")
				If isCOM Then
					Console.WriteLine("== Using early bound COM ==")
				Else
					Console.WriteLine("== Using native .NET ==")
				End If
			Catch
				Console.WriteLine("Failed to create " + progId)
				Continue Do
			End Try
			'
			' Same test routine as in the SimpleTest scripts
			'
			Console.WriteLine("Using properties:")
			S.X = 2
			S.Y = 3
			Console.WriteLine("  X=" & S.X & " Y=" & S.Y & " Diagonal=" & S.Diagonal)
			Console.WriteLine("Using CalculateDiagonal:")
			Console.WriteLine("  CalculateDiagonal(4,5)=" & S.CalculateDiagonal(4, 5))
			Console.WriteLine("Enum test:")
			S.EnumTest = 1
			Console.WriteLine("  Enum value is " & S.EnumTest)
			Console.WriteLine("")

			S = Nothing
			If isCOM Then Marshal.ReleaseComObject(O)
			O = Nothing
		Loop Until False
	End Sub

End Module
