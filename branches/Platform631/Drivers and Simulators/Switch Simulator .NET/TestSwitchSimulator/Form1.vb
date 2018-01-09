Public Class Form1
    Private sw As New ASCOM.SwitchSimulator.Switch
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

        sw.Connected = True
        Try
            MsgBox(" state of switch 8 = " & CBool(sw.GetSwitch(8)))
        Catch ex As Exception
            MsgBox(ex.Source & ex.Message)
        End Try
        

    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        sw.Connected = True
        sw.SetSwitchName(3, "Scope on")
        sw.SetSwitch(3, True)
    End Sub

    Private Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
        sw.Connected = True
        sw.SetupDialog()
    End Sub
End Class
