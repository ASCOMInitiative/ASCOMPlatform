Imports System.Collections.Generic
Imports System.Net

''' <summary>
''' Overall description of an Alpaca device that supports discovery as returned by the <see cref="AlpacaDiscovery"/> component.
''' </summary>
Public Class AlpacaDevice

    ''' <summary>
    ''' Initialises the class with default values
    ''' </summary>
    Public Sub New()
        Me.New(New IPEndPoint(IPAddress.Any, 0), "")
    End Sub


    ''' <summary>
    ''' Initialise IP end point, Alpaca unique ID and Status message - Can only be used from .NET clients
    ''' </summary>
    ''' <paramname="ipEndPoint">Alpaca device IP endpoint</param>
    ''' <paramname="alpacaUniqueId">Alpaca device unique ID</param>
    ''' <paramname="statusMessage">Device status message</param>
    Friend Sub New(ByVal ipEndPoint As IPEndPoint, ByVal statusMessage As String)
        Me.IPEndPoint = ipEndPoint
        HostName = ipEndPoint.Address.ToString() ' Initialise the host name to the IP address in case there is no DNS name resolution or in case this fails
        Port = ipEndPoint.Port ' Initialise the port number to the port set in the IPEndPoint
        ConfiguredDevices = New ConfiguredDevice() {} ' List(Of ConfiguredDevice)()
        SupportedInterfaceVersions = New Integer() {}
        Me.StatusMessage = statusMessage
    End Sub

#Region "Public Properties"

    ''' <summary>
    ''' Alpaca device's host name or IP address
    ''' </summary>
    Public Property HostName As String

    ''' <summary>
    ''' Alpaca device's IP port number
    ''' </summary>
    Public Property Port As Integer

    ''' <summary>
    ''' Array of ASCOM devices available on this Alpaca device
    ''' </summary>
    Public Property ConfiguredDevices As ConfiguredDevice()

    ''' <summary>
    ''' Array of supported Alpaca interface version numbers
    ''' </summary>
    Public Property SupportedInterfaceVersions As Integer()

    ''' <summary>
    ''' The Alpaca device's configured name
    ''' </summary>
    ''' <returns></returns>
    Public Property ServerName As String = ""

    ''' <summary>
    ''' The device manufacturer's name
    ''' </summary>
    ''' <returns></returns>
    Public Property Manufacturer As String = ""

    ''' <summary>
    ''' The device's version as set by the manufacturer
    ''' </summary>
    ''' <returns></returns>
    Public Property ManufacturerVersion As String = ""

    ''' <summary>
    ''' The Alpaca device's configured location
    ''' </summary>
    ''' <returns></returns>
    Public Property Location As String = ""

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