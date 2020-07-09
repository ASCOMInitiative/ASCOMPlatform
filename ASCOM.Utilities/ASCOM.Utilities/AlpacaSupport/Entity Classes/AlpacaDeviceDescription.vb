''' <summary>
''' Description of an Alpaca device that is is discovered by the <see cref="AlpacaDiscovery"/> component.
''' </summary>
Friend Class AlpacaDeviceDescription
    ''' <summary>
    ''' Class initialiser
    ''' </summary>
    ''' <remarks>COM clients should use this initialiser and set the properties individually because COM only supports parameterless initialisers.</remarks>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Class initialiser that sets all properties in one call
    ''' </summary>
    ''' <param name="serverName">The Alpaca device's configured name</param>
    ''' <param name="manufacturer">The device manufacturer's name</param>
    ''' <param name="manufacturerVersion">The device's version as set by the manufacturer</param>
    ''' <param name="location">The Alpaca device's configured location</param>
    ''' <param name="alpacaUniqueId"></param>
    ''' <remarks>This can only be used by .NET clients because COM only supports parameterless initialisers.</remarks>
    Friend Sub New(ByVal serverName As String, ByVal manufacturer As String, ByVal manufacturerVersion As String, ByVal location As String)
        Me.ServerName = serverName
        Me.Manufacturer = manufacturer
        Me.ManufacturerVersion = manufacturerVersion
        Me.Location = location
    End Sub

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

End Class
