Public Class TraceLoggerExamples
    Sub Example()
        'This code will create a trace file in My Documents\ASCOM\Logs with the following 2 lines:
        '   HH:MM:SS.fff Test Log          Whole message in one go
        '   HH:MM:SS.fff Test Log          Start of line, continue 1, continue 2, continue 3, end of line

        Dim TrLog As ASCOM.Utilities.TraceLogger 'Define cariable as type ILogger
        Const TL As String = "Test Log"

        'Create a new trace logger and enable it
        TrLog = New ASCOM.Utilities.TraceLogger("", "TestLogger")
        TrLog.Enabled = True

        'Create the first line
        TrLog.LogMessage(TL, "Whole message in one go")

        'Create the start of the second line
        TrLog.LogStart(TL, "Start of line, ")

        For i = 1 To 3
            TrLog.LogContinue("continue " & i & ", ") 'Append the middle parts of the second line
        Next

        'Finish the second line
        TrLog.LogFinish("end of line")

        'Stop logging and clean up the logger
        TrLog.Enabled = False
        TrLog.Dispose()
        TrLog = Nothing
    End Sub
End Class
