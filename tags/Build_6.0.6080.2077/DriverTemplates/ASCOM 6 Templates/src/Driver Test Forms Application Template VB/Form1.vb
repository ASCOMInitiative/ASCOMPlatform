Public Class Form1

    Private driver As TEMPLATEDEVICECLASS

    Private Sub buttonChoose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonChoose.Click
        My.Settings.DriverId = TEMPLATEDEVICECLASS.Choose(My.Settings.DriverId)
        SetUIState()
    End Sub

    Private Sub buttonConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonConnect.Click
        If (IsConnected) Then
            driver.Connected = False
        Else
            driver = New TEMPLATEDEVICECLASS(My.Settings.DriverId)
            driver.Connected = True
        End If
        SetUIState()
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If IsConnected Then
            driver.Connected = False
        End If

        My.Settings.Save()
    End Sub

    Private Sub SetUIState()
        buttonConnect.Enabled = Not String.IsNullOrEmpty(My.Settings.DriverId)
        buttonChoose.Enabled = Not IsConnected
        buttonConnect.Text = IIf(IsConnected, "Disconnect", "Connect")
    End Sub

    Private ReadOnly Property IsConnected() As Boolean
        Get
            If Me.driver Is Nothing Then Return False
            Return driver.Connected
        End Get
    End Property

End Class
