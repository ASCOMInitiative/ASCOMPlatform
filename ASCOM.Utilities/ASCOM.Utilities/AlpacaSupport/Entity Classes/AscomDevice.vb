Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.InteropServices
Imports ASCOM.Utilities.Interfaces

''' <summary>
''' Data object describing an ASCOM device that is served by an Alpaca device as returned by the <see cref="AlpacaDiscovery"/> component.
''' </summary>
<Guid("E768E0BB-D795-4CAE-95D0-9D0173BF57BC"),
ComVisible(True),
ClassInterface(ClassInterfaceType.None)>
Public Class AscomDevice
    Implements IAscomDevice, IAscomDeviceExtra

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
    ''' <param name="ascomDdeviceName">ASCOM device name</param>
    ''' <param name="ascomDeviceType">ASCOM device type</param>
    ''' <param name="alpacaDeviceNumber">Alpaca API device number</param>
    ''' <param name="uniqueId">ASCOM device unique ID</param>
    ''' <param name="ipEndPoint">Alpaca device IP endpoint</param>
    ''' <param name="hostName">ALapca device host name</param>
    ''' <param name="interfaceVersion">Supported Alpaca interface version</param>
    ''' <param name="statusMessage">Alpaca device status message</param>
    ''' <remarks>This can only be used by .NET clients because COM only supports parameterless initialisers.</remarks>
    Friend Sub New(ByVal ascomDdeviceName As String, ByVal ascomDeviceType As String, ByVal alpacaDeviceNumber As Integer, ByVal uniqueId As String, ByVal ipEndPoint As IPEndPoint, ByVal hostName As String, ByVal interfaceVersion As Integer, ByVal statusMessage As String)
        AscomDeviceName = ascomDdeviceName
        Me.AscomDeviceType = ascomDeviceType
        Me.AlpacaDeviceNumber = alpacaDeviceNumber
        Me.UniqueId = uniqueId
        Me.IPEndPoint = ipEndPoint
        Me.HostName = hostName
        Me.InterfaceVersion = interfaceVersion
        Me.StatusMessage = statusMessage

        ' Populate the IP address based on the supplied IPEndPoint value and address type
        If ipEndPoint.AddressFamily = AddressFamily.InterNetwork Then ' IPv4 address
            Me.IpAddress = ipEndPoint.Address.ToString()
        ElseIf ipEndPoint.AddressFamily = AddressFamily.InterNetworkV6 Then ' IPv6 address so save it in canonical form
            Me.IpAddress = $"[{ipEndPoint.Address}]"
        Else
            Throw New ASCOM.InvalidValueException($"Unsupported network type {ipEndPoint.AddressFamily} when creating a new ASCOMDevice")
        End If

    End Sub

    ''' <summary>
    ''' ASCOM device name
    ''' </summary>
    Public Property AscomDeviceName As String Implements IAscomDevice.AscomDeviceName

    ''' <summary>
    ''' ASCOM device type
    ''' </summary>
    Public Property AscomDeviceType As String Implements IAscomDevice.AscomDeviceType

    ''' <summary>
    ''' Alpaca API device number
    ''' </summary>
    Public Property AlpacaDeviceNumber As Integer Implements IAscomDevice.AlpacaDeviceNumber

    ''' <summary>
    ''' ASCOM device unique ID
    ''' </summary>
    Public Property UniqueId As String Implements IAscomDevice.UniqueId

    ''' <summary>
    ''' The ASCOM device's DNS host name, if available, otherwise its IP address. IPv6 addresses will be in canonical form.
    ''' </summary>
    Public Property HostName As String Implements IAscomDevice.HostName

    ''' <summary>
    ''' The ASCOM device's IP address. IPv6 addresses will be in canonical form.
    ''' </summary>
    Public Property IpAddress As String Implements IAscomDevice.IpAddress

    ''' <summary>
    ''' Supported Alpaca interface version
    ''' </summary>
    Public Property InterfaceVersion As Integer Implements IAscomDevice.InterfaceVersion

    ''' <summary>
    ''' Alpaca device status message
    ''' </summary>
    Public Property StatusMessage As String Implements IAscomDevice.StatusMessage

    ''' <summary>
    ''' Alpaca device IP endpoint
    ''' </summary>
    Friend Property IPEndPoint As IPEndPoint Implements IAscomDeviceExtra.IPEndPoint

End Class
