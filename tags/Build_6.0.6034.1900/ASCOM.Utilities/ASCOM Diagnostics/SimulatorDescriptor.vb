Public Class SimulatorDescriptor
    Private _ProgID As String
    Private _SixtyFourBit As Boolean
    Private _IsPlatform5 As Boolean
    Private _Description As String
    Private _DeviceType As String
    Private _InterfaceVersion As Integer
    Private _DriverVersion As String
    Private _Name As String
    Private _AxisRates As Double(,)

    Public Property ProgID As String
        Get
            Return _ProgID
        End Get
        Set(ByVal value As String)
            _ProgID = value
        End Set
    End Property

    Public Property SixtyFourBit As Boolean
        Get
            Return _SixtyFourBit
        End Get
        Set(ByVal value As Boolean)
            _SixtyFourBit = value
        End Set
    End Property

    Public Property IsPlatform5 As Boolean
        Get
            Return _IsPlatform5
        End Get
        Set(ByVal value As Boolean)
            _IsPlatform5 = value
        End Set
    End Property

    Public Property Description As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property DeviceType As String
        Get
            Return _DeviceType
        End Get
        Set(ByVal value As String)
            _DeviceType = value
        End Set
    End Property

    Public Property InterfaceVersion As Integer
        Get
            Return _InterfaceVersion
        End Get
        Set(ByVal value As Integer)
            _InterfaceVersion = value
        End Set
    End Property

    Public Property DriverVersion As String
        Get
            Return _DriverVersion
        End Get
        Set(ByVal value As String)
            _DriverVersion = value
        End Set
    End Property

    Public Property Name As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property AxisRates As Double(,)
        Get
            Return _AxisRates
        End Get
        Set(ByVal value As Double(,))
            _AxisRates = value
        End Set
    End Property
End Class
