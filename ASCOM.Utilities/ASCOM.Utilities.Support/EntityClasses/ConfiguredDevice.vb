Namespace ASCOM.Alpaca
    Public Class ConfiguredDevice
        Public Sub New()
        End Sub

        Public Sub New(ByVal deviceNameValue As String, ByVal deviceTypeValue As String, ByVal deviceNumberValue As Integer, ByVal deviceUniqueIDValue As String)
            DeviceName = deviceNameValue
            DeviceType = deviceTypeValue
            DeviceNumber = deviceNumberValue
            DeviceUniqueID = deviceUniqueIDValue
        End Sub

        Public Property DeviceName As String
        Public Property DeviceType As String
        Public Property DeviceNumber As Integer
        Public Property DeviceUniqueID As String
    End Class
End Namespace
