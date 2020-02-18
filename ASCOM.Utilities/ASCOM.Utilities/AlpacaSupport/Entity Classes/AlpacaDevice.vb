Imports System.Collections.Generic
Imports System.Net

''' <summary>
''' Alpaca device entity class
''' </summary>
Public Class AlpacaDevice
    ''' <summary>
    ''' Default initialiser
    ''' </summary>
    Public Sub New()
        Me.New(New IPEndPoint(IPAddress.Any, 0), "")
    End Sub


    ''' <summary>
    ''' Initialise IP end point, Alpaca unique ID and Status message
    ''' </summary>
    ''' <paramname="ipEndPoint">Alpaca device IP endpoint</param>
    ''' <paramname="alpacaUniqueId">Alpaca device unique ID</param>
    ''' <paramname="statusMessage">Device status message</param>
    Public Sub New(ByVal ipEndPoint As IPEndPoint, ByVal statusMessage As String)
        Me.IPEndPoint = ipEndPoint
        HostName = Me.IPEndPoint.Address.ToString() ' Initialise the host name to the IP address in case there is no DNS name resolution or in case this fails
        AlpacaDeviceDescription = New AlpacaDeviceDescription()
        ConfiguredDevices = New List(Of ConfiguredDevice)()
        SupportedInterfaceVersions = New Integer() {}
        Me.StatusMessage = statusMessage
    End Sub


    ''' <summary>
    ''' Alpaca device IP endpoint
    ''' </summary>
    Public Property IPEndPoint As IPEndPoint

    ''' <summary>
    ''' Alpaca device host name
    ''' </summary>
    Public Property HostName As String

    ''' <summary>
    ''' Alpaca device description
    ''' </summary>
    Public Property AlpacaDeviceDescription As AlpacaDeviceDescription

    ''' <summary>
    ''' List of ASCOM devices available on this Alpaca device
    ''' </summary>
    Public Property ConfiguredDevices As List(Of ConfiguredDevice)

    ''' <summary>
    ''' Array of supported Alpaca interface version numbers
    ''' </summary>
    Public Property SupportedInterfaceVersions As Integer()

    ''' <summary>
    ''' Alpaca device status message
    ''' </summary>
    ''' <remarks>This should be an empty string in normal operation when there are no issues but should be changed to an error message when an issue occurs.</remarks>
    Public Property StatusMessage As String = ""
End Class