Namespace ASCOM.Utilities.Support
    ''' <summary>
    ''' Configured device entity class
    ''' </summary>
    Public Class ConfiguredDevice
        ''' <summary>
        ''' Default initialiser
        ''' </summary>
        Public Sub New()
        End Sub


        ''' <summary>
        ''' Initialise the device name, device type, device number and ASCOM device unique ID
        ''' </summary>
        ''' <paramname="deviceName">ASCOM device name</param>
        ''' <paramname="deviceType">ASCOM device type</param>
        ''' <paramname="deviceNumber">Alpaca API device number</param>
        ''' <paramname="uniqueID">ASCOM device unique ID</param>
        Public Sub New(ByVal deviceName As String, ByVal deviceType As String, ByVal deviceNumber As Integer, ByVal uniqueID As String)
            Me.DeviceName = deviceName
            Me.DeviceType = deviceType
            Me.DeviceNumber = deviceNumber
            Me.UniqueID = uniqueID
        End Sub


        ''' <summary>
        ''' ASCOM device name
        ''' </summary>
        Public Property DeviceName As String

        ''' <summary>
        ''' ASCOM device type
        ''' </summary>
        Public Property DeviceType As String

        ''' <summary>
        ''' Alpaca API device number
        ''' </summary>
        Public Property DeviceNumber As Integer

        ''' <summary>
        ''' ASCOM device unique ID
        ''' </summary>
        Public Property UniqueID As String
    End Class
End Namespace
