Namespace ASCOM.Utilities.Support
    Public Class AlpacaDeviceDescription
        Public Sub New()
        End Sub

        Public Sub New(ByVal serverName As String, ByVal manufacturer As String, ByVal manufacturerVersion As String, ByVal location As String, ByVal alpacaUniqueId As String)
            Me.ServerName = serverName
            Me.Manufacturer = manufacturer
            Me.ManufacturerVersion = manufacturerVersion
            Me.Location = location
            Me.AlpacaUniqueId = alpacaUniqueId
        End Sub

        Public Property ServerName As String = ""
        Public Property Manufacturer As String = ""
        Public Property ManufacturerVersion As String = ""
        Public Property Location As String = ""
        Public Property AlpacaUniqueId As String = ""
    End Class
End Namespace
