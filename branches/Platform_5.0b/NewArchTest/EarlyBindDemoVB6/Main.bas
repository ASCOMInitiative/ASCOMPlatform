Attribute VB_Name = "MainProgram"
#Const ForV2 = True

Sub Main()
    '
    ' Make cheapo console app. Cannot be run in IDE!
    ' After making exe, drop it onto the MakeConsoleApp.vbs script
    ' to set the console bit in the PE file.
    '
    Dim FSO As New FileSystemObject
    Dim Console As TextStream
    Set Console = FSO.GetStandardStream(StdOut)
    
    Dim ans As String
    Dim progId As String
    Dim S As IAscomSampleLib.IAscomSample

#If ForV2 Then
    Console.WriteLine ("Early binding to .NET or to COM (V2 Interface):")
#Else
    Console.WriteLine ("Early binding to .NET or to COM (V1 Interface):")
#End If

    Console.WriteLine ("")
    Do
        Console.WriteLine ("-----------------------------------------------------")
        Console.WriteLine ("Pick one of the components that supports IAscomSample")
        Console.WriteLine ("  1 - ASCOM.InprocServerCOM.CSharpSample (.NET)")
        Console.WriteLine ("  2 - ASCOM.InprocServerCOM.VBSample (.NET)")
        Console.WriteLine ("  3 - ASCOM.LocalServerCOM.CSharpSample (COM)")
        Console.WriteLine ("  4 - InprocServerCOM.VB6Sample (COM)")
        Console.Write ("Enter the number (1-4, 0 to exit):")
        ans = FSO.GetStandardStream(StdIn).ReadLine
        Select Case ans
            Case "0": Exit Sub
            Case "1": progId = "ASCOM.InprocServerCOM.CSharpSample"
            Case "2": progId = "ASCOM.InprocServerCOM.VBSample"
            Case "3": progId = "ASCOM.LocalServerCOM.CSharpSample"
            Case "4": progId = "InprocServerCOM.VB6Sample"
            Case Else: progId = ""
        End Select

        If progId = "" Then
            Console.WriteLine ("Not 1-4, re-run and try again")
            Console.Write ("Press enter to quit...")
            FSO.GetStandardStream(StdIn).ReadLine
            Exit Sub
        End If
        
        Set S = CreateObject(progId)
        Console.WriteLine ("== Using early bound COM in VB6 ==")
        '
        ' Same test routine as in the SimpleTest scripts
        '
        Console.WriteLine ("Using properties:")
        S.X = 2
        S.Y = 3
        Console.WriteLine ("  X=" & S.X & " Y=" & S.Y & " Diagonal=" & S.Diagonal)
        Console.WriteLine ("Using CalculateDiagonal:")
        Console.WriteLine ("  CalculateDiagonal(4,5)=" & S.CalculateDiagonal(4, 5))
        Console.WriteLine ("Enum test:")
        S.EnumTest = 1
        Console.WriteLine ("  Enum value is " & S.EnumTest)
#If ForV2 Then
        Console.WriteLine ("  X=" & S.X & " Y=" & S.Y & " Diagonal=" & S.Area)
        Console.WriteLine ("Using CalculateArea:")
        Console.WriteLine ("  CalculateArea(4,5)=" & S.CalculateArea(4, 5))
#End If

        Set S = Nothing
    Loop Until False
    
    Exit Sub

Oops:
    Console.WriteLine ("Failed to create " + progId)
    Console.WriteLine (Err.Source & ": " & Err.Description)

End Sub
