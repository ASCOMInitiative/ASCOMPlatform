Imports System.Net

''' <summary>
''' Description of an ASCOM device that is served by an Alpaca device that is discovered by the <see cref="AlpacaDiscovery"/> component.
''' </summary>
Public Class AscomDevice
    ''' <summary>
    ''' Initialises the class with default values
    ''' </summary>
    ''' <remarks>COM clients should use this initialiser and set the properties individually because COM only supports parameterless initialisers.</remarks>
    Public Sub New()
    End Sub


    ''' <summary>
    ''' Initialise the ASCOM device name, ASCOM device type and ASCOM device unique ID, plus
    ''' the Alpaca API device number, unique ID, device IP endpoint, Alpaca unique ID, interface version and status message
    ''' </summary>
    ''' <paramname="ascomDdeviceName">ASCOM device name</param>
    ''' <paramname="ascomDeviceType">ASCOM device type</param>
    ''' <paramname="alpacaDeviceNumber">Alpaca API device number</param>
    ''' <paramname="uniqueId">ASCOM device unique ID</param>
    ''' <paramname="ipEndPoint">Alpaca device IP endpoint</param>
    ''' <paramname="hostName">ALapca device host name</param>
    ''' <paramname="alpacaUniqueId">Alpaca device unique ID</param>
    ''' <paramname="interfaceVersion">Supported Alpaca interface version</param>
    ''' <paramname="statusMessage">ALapca device status message</param>
    ''' <remarks>This can only be used by .NET clients because COM only supports parameterless initialisers.</remarks>
    Public Sub New(ByVal ascomDdeviceName As String, ByVal ascomDeviceType As String, ByVal alpacaDeviceNumber As Integer, ByVal uniqueId As String, ByVal ipEndPoint As IPEndPoint, ByVal hostName As String, ByVal interfaceVersion As Integer, ByVal statusMessage As String)
        AscomDeviceName = ascomDdeviceName
        Me.AscomDeviceType = ascomDeviceType
        Me.AlpacaDeviceNumber = alpacaDeviceNumber
        Me.UniqueId = uniqueId
        Me.IPEndPoint = ipEndPoint
        Me.HostName = hostName
        Me.InterfaceVersion = interfaceVersion
        Me.StatusMessage = statusMessage
    End Sub

    ''' <summary>
    ''' ASCOM device name
    ''' </summary>
    Public Property AscomDeviceName As String

    ''' <summary>
    ''' ASCOM device type
    ''' </summary>
    Public Property AscomDeviceType As String

    ''' <summary>
    ''' Alpaca API device number
    ''' </summary>
    Public Property AlpacaDeviceNumber As Integer

    ''' <summary>
    ''' ASCOM device unique ID
    ''' </summary>
    Public Property UniqueId As String

    ''' <summary>
    ''' Alpaca device IP endpoint
    ''' </summary>
    Public Property IPEndPoint As IPEndPoint

    ''' <summary>
    ''' Alpaca device host name
    ''' </summary>
    Public Property HostName As String

    ''' <summary>
    ''' SUpported Alpaca interface version
    ''' </summary>
    Public Property InterfaceVersion As Integer

    ''' <summary>
    ''' Alpaca device status message
    ''' </summary>
    Public Property StatusMessage As String
End Class
