Imports System.Collections.Generic
Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.InteropServices
Imports ASCOM.Utilities.Interfaces

''' <summary>
''' Overall description of an Alpaca device that supports discovery as returned by the <see cref="AlpacaDiscovery"/> component.
''' </summary>
<Guid("D572145F-E4CF-4A9E-B2AE-A0D32604E20C"),
ComVisible(True),
ClassInterface(ClassInterfaceType.None)>
Public Class AlpacaDevice
    Implements ASCOM.Utilities.Interfaces.IAlpacaDevice, ASCOM.Utilities.Interfaces.IAlpacaDeviceExtra

    Dim configuredDevicesValue As List(Of ConfiguredDevice)
    Dim configuredDevicesAsArrayListValue As ArrayList

    ''' <summary>
    ''' Initialises the class with default values
    ''' </summary>
    Public Sub New()
        Me.New(New IPEndPoint(0, 0), "")
    End Sub

    ''' <summary>
    ''' Initialise IP end point, Alpaca unique ID and Status message - Can only be used from .NET clients
    ''' </summary>
    ''' <paramname="ipEndPoint">Alpaca device IP endpoint</param>
    ''' <paramname="alpacaUniqueId">Alpaca device unique ID</param>
    ''' <paramname="statusMessage">Device status message</param>
    Friend Sub New(ByVal ipEndPoint As IPEndPoint, ByVal statusMessage As String)
        ' Initialise internal storage variables
        configuredDevicesValue = New List(Of ConfiguredDevice)
        configuredDevicesAsArrayListValue = New ArrayList
        SupportedInterfaceVersions = New Integer() {}

        Me.IPEndPoint = ipEndPoint ' Set the IPEndpoint to the supplied value

        ' Initialise the Host name to the IP address using the normal ToString version if IPv4 or the canonical form if IPv6
        If ipEndPoint.AddressFamily = AddressFamily.InterNetworkV6 Then
            IpAddress = $"[{ipEndPoint.Address}]"  ' Set the IPv6 canonical IP host address
            HostName = IpAddress ' Initialise the host name to the IPv6 address in case there is no DNS name resolution or in case this fails
        Else
            IpAddress = ipEndPoint.Address.ToString()  ' Set the IPv4 IP host address
            HostName = IpAddress ' Initialise the host name to the IPv4 address in case there is no DNS name resolution or in case this fails
        End If

        Port = ipEndPoint.Port ' Set the port number to the port set in the IPEndPoint

        Me.StatusMessage = statusMessage ' Set the status message to the supplied value

    End Sub

#Region "Public Properties"

    ''' <summary>
    ''' The Alpaca device's DNS host name, if available, otherwise its IP address. IPv6 addresses will be in canonical form.
    ''' </summary>
    Public Property HostName As String Implements IAlpacaDevice.HostName

    ''' <summary>
    ''' The Alpaca device's IP address. IPv6 addresses will be in canonical form.
    ''' </summary>
    Public Property IpAddress As String Implements IAlpacaDevice.IpAddress

    ''' <summary>
    ''' Alpaca device's IP port number
    ''' </summary>
    Public Property Port As Integer Implements IAlpacaDevice.Port

    ''' <summary>
    ''' List of ASCOM devices available on this Alpaca device as an ArrayList for COM clients
    ''' </summary>
    ''' <remarks>
    ''' This method is primarily to support COM clients because COM does not support generic lists. .NET clients should use the <see cref="ConfiguredDevices"/> property instead.
    ''' </remarks>
    ReadOnly Property ConfiguredDevicesAsArrayList As ArrayList Implements IAlpacaDevice.ConfiguredDevicesAsArrayList
        Get
            Return configuredDevicesAsArrayListValue ' Return the array-list of devices that was populated by the set ConfiguredDevices method
        End Get
    End Property

    ''' <summary>
    ''' Array of supported Alpaca interface version numbers
    ''' </summary>
    Public Property SupportedInterfaceVersions As Integer() Implements IAlpacaDevice.SupportedInterfaceVersions

    ''' <summary>
    ''' The Alpaca device's configured name
    ''' </summary>
    ''' <returns></returns>
    Public Property ServerName As String = "" Implements IAlpacaDevice.ServerName

    ''' <summary>
    ''' The device manufacturer's name
    ''' </summary>
    ''' <returns></returns>
    Public Property Manufacturer As String = "" Implements IAlpacaDevice.Manufacturer

    ''' <summary>
    ''' The device's version as set by the manufacturer
    ''' </summary>
    ''' <returns></returns>
    Public Property ManufacturerVersion As String = "" Implements IAlpacaDevice.ManufacturerVersion

    ''' <summary>
    ''' The Alpaca device's configured location
    ''' </summary>
    ''' <returns></returns>
    Public Property Location As String = "" Implements IAlpacaDevice.Location

    ''' <summary>
    ''' List of ASCOM devices available on this Alpaca device
    ''' </summary>
    ''' <remarks>
    ''' This method can only be used by .NET clients. COM clients should use the <see cref="ConfiguredDevicesAsArrayList"/> property.
    ''' </remarks>
    Public Property ConfiguredDevices As List(Of ConfiguredDevice) Implements IAlpacaDeviceExtra.ConfiguredDevices
        Get
            Return configuredDevicesValue ' Return the list of configured devices
        End Get

        Set(ByVal value As List(Of ConfiguredDevice))
            ' Save the supplied list of configured devices 
            configuredDevicesValue = value

            ' Populate the read-only arraylist with the same data
            configuredDevicesAsArrayListValue.Clear()
            For Each configuredDevice As ConfiguredDevice In configuredDevicesValue
                configuredDevicesAsArrayListValue.Add(configuredDevice)
            Next
        End Set
    End Property
#End Region

#Region "Internal Properties"

    ''' <summary>
    ''' Alpaca device's status message
    ''' </summary>
    ''' <remarks>This should be an empty string in normal operation when there are no issues but should be changed to an error message when an issue occurs.</remarks>
    Friend Property StatusMessage As String = ""

    ''' <summary>
    ''' Alpaca device's IP endpoint
    ''' </summary>
    Friend Property IPEndPoint As IPEndPoint

#End Region

End Class