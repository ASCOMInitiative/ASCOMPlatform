Imports System.IO
Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Process("ear404")
        Process("jup404")
        Process("mar404")
        Process("mer404")
        Process("mlat404")
        Process("mlr404")
        Process("nep404")
        Process("plu404")
        Process("sat404")
        Process("ura404")
        Process("ven404")
    End Sub
    Sub Process(ByVal Fn As String)
        Dim fsin, fsout As FileStream
        Dim sin As StreamReader
        Dim sout As StreamWriter
        Dim l As String
        Try

            fsin = File.OpenRead(Fn & ".cpp")
            fsout = File.Create(Fn & ".txt")
            sin = New StreamReader(fsin)
            sout = New StreamWriter(fsout)

            Do
                l = Trim(sin.ReadLine())
                If l <> "" Then
                    If Microsoft.VisualBasic.Right(l, 1) = "," Then l = l & " _"
                    sout.WriteLine(l)
                End If
            Loop Until sin.EndOfStream
            sin.Close()
            sout.Close()
            sin.Dispose()
            sout.Dispose()

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class
