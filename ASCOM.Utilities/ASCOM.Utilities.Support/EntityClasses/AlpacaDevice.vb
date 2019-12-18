Imports System.Collections.Generic
Imports System.Net

Namespace ASCOM.Alpaca
    Public Class AlpacaDevice
        Public Sub New()
            Me.New(New IPEndPoint(IPAddress.Any, 0), "", "")
        End Sub

        Public Sub New(ByVal ipEndPointValue As IPEndPoint, ByVal alpacaUniqueIdValue As String, ByVal statusMessageValue As String)
            IPEndPoint = ipEndPointValue
            HostName = ""
            AlpacaUniqueId = alpacaUniqueIdValue
            AlpacaDeviceDescription = New AlpacaDeviceDescription
            ConfiguredDevices = New List(Of ConfiguredDevice)
            SupportedInterfaceVersions = New Integer() {}
            StatusMessage = statusMessageValue
        End Sub

        Public Property IPEndPoint As IPEndPoint
        Public Property HostName As String
        Public Property AlpacaUniqueId As String
        Public Property AlpacaDeviceDescription As AlpacaDeviceDescription
        Public Property ConfiguredDevices As List(Of ConfiguredDevice)
        Public Property SupportedInterfaceVersions As Integer()
        Public Property StatusMessage As String = ""
    End Class
End Namespace
