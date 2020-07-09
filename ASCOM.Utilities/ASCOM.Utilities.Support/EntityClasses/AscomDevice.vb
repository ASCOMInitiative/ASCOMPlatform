Imports System.Net

Namespace ASCOM.Alpaca
    Public Class AscomDevice
        Public Sub New()
        End Sub

        Public Sub New(ByVal ascomDdeviceNameValue As String, ByVal ascomDeviceTypeValue As String, ByVal alpacaDeviceNumberValue As Integer, ByVal ascomDeviceUniqueIdValue As String, ByVal ipEndPointValue As IPEndPoint, ByVal hostNameValue As String, ByVal alpacaUniqueIdValue As String, ByVal interfaceVersionValue As Integer, ByVal statusMessageValue As String)
            AscomDeviceName = ascomDdeviceNameValue
            AscomDeviceType = ascomDeviceTypeValue
            AlpacaDeviceNumber = alpacaDeviceNumberValue
            AscomDeviceUniqueId = ascomDeviceUniqueIdValue
            IPEndPoint = ipEndPointValue
            HostName = hostNameValue
            AlpacaUniqueId = alpacaUniqueIdValue
            InterfaceVersion = interfaceVersionValue
            StatusMessage = statusMessageValue
        End Sub

        Public Property AscomDeviceName As String
        Public Property AscomDeviceType As String
        Public Property AlpacaDeviceNumber As Integer
        Public Property AscomDeviceUniqueId As String
        Public Property IPEndPoint As IPEndPoint
        Public Property HostName As String
        Public Property AlpacaUniqueId As String
        Public Property InterfaceVersion As Integer
        Public Property StatusMessage As String
    End Class
End Namespace
