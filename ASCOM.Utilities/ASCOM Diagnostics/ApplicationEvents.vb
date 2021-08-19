﻿Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        ' Catch an unhandled exception.
        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            ' If the user clicks No, then exit.
            e.ExitApplication =
                MessageBox.Show(e.Exception.ToString() &
                        vbCrLf & "Continue?", "Continue?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) _
                        = DialogResult.No
        End Sub
    End Class


End Namespace

