Imports ASCOM.Utilities
Public Class SerialForm
    Private Sub SerialForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SerPort As New ASCOM.Utilities.Serial, Ports() As String
        Ports = SerPort.AvailableCOMPorts
        SerPort.Dispose()
        SerPort = Nothing
        For Each Port As String In Ports
            lstSerialASCOM.Items.Add(Port)
        Next

    End Sub

    Private Sub btnSerialExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSerialExit.Click
        Me.Visible = False
        Me.Close()
    End Sub
End Class