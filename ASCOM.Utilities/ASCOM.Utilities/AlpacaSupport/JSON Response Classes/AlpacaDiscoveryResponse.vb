Friend Class AlpacaDiscoveryResponse
    Public Sub New()
    End Sub

    Public Sub New(ByVal alpacaPort As Integer)
        Me.AlpacaPort = alpacaPort
    End Sub

    Public Property AlpacaPort As Integer = 11111
End Class
