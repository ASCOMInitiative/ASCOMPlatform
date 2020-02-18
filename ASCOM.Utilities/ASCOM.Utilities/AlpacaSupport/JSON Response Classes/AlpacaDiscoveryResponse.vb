Public Class AlpacaDiscoveryResponse
    Public Sub New()
    End Sub

    Public Sub New(ByVal alpacaPort As Integer, ByVal alpacaUniqueId As String)
        Me.AlpacaPort = alpacaPort
        Me.AlpacaUniqueId = alpacaUniqueId
    End Sub

    Public Property AlpacaPort As Integer = 11111
    Public Property AlpacaUniqueId As String = ""
End Class
