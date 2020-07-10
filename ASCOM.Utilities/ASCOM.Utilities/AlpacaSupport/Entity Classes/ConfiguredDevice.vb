Imports System.Runtime.InteropServices
Imports ASCOM.Utilities.Interfaces

''' <summary>
''' Description of a single ASCOM device as returned within the <see cref="AlpacaDevice"/> class.
''' </summary>
<Guid("AAB2619C-69FC-4717-8F01-10228D59E99E"),
ComVisible(True),
ClassInterface(ClassInterfaceType.None)>
Public Class ConfiguredDevice
    Implements IConfiguredDevice

    ' State variables
    Dim deviceNameValue, deviceTypeValue, uniqueIDValue As String

    ''' <summary>
    ''' Initialises the class with default values
    ''' </summary>
    Public Sub New()
        ' Initialise all strings to empty values
        FixNullStrings()
    End Sub

    ''' <summary>
    ''' Initialise the device name, device type, device number and ASCOM device unique ID
    ''' </summary>
    ''' <paramname="deviceName">ASCOM device name</param>
    ''' <paramname="deviceType">ASCOM device type</param>
    ''' <paramname="deviceNumber">Alpaca API device number</param>
    ''' <paramname="uniqueID">ASCOM device unique ID</param>
    Friend Sub New(ByVal deviceName As String, ByVal deviceType As String, ByVal deviceNumber As Integer, ByVal uniqueID As String)
        deviceNameValue = deviceName
        deviceTypeValue = deviceType
        Me.DeviceNumber = deviceNumber
        uniqueIDValue = uniqueID

        ' Ensure that none of the strings are null
        FixNullStrings()
    End Sub

    ''' <summary>
    ''' ASCOM device name
    ''' </summary>
    Public Property DeviceName As String Implements IConfiguredDevice.DeviceName
        Get
            Return deviceNameValue
        End Get
        Set(value As String)
            deviceNameValue = value
            ' Protect against null values being assigned
            If String.IsNullOrEmpty(deviceNameValue) Then deviceNameValue = ""
        End Set
    End Property

    ''' <summary>
    ''' ASCOM device type
    ''' </summary>
    Public Property DeviceType As String Implements IConfiguredDevice.DeviceType
        Get
            Return deviceTypeValue
        End Get
        Set(value As String)
            deviceTypeValue = value
            ' Protect against null values being assigned
            If String.IsNullOrEmpty(deviceTypeValue) Then deviceTypeValue = ""
        End Set
    End Property

    ''' <summary>
    ''' Device number used to access the device through the Alpaca API
    ''' </summary>
    Public Property DeviceNumber As Integer Implements IConfiguredDevice.DeviceNumber

    ''' <summary>
    ''' ASCOM device unique ID
    ''' </summary>
    Public Property UniqueID As String Implements IConfiguredDevice.UniqueID
        Get
            Return uniqueIDValue
        End Get
        Set(value As String)
            uniqueIDValue = value
            ' Protect against null values being assigned
            If String.IsNullOrEmpty(uniqueIDValue) Then uniqueIDValue = ""
        End Set
    End Property

    ''' <summary>
    ''' Make sure that all properties return a string i.e. that they do not return "null" or an unhelpful empty string
    ''' </summary>
    Private Sub FixNullStrings()
        If String.IsNullOrEmpty(deviceNameValue) Then deviceNameValue = ""
        If String.IsNullOrEmpty(deviceTypeValue) Then deviceTypeValue = ""
        If String.IsNullOrEmpty(uniqueIDValue) Then uniqueIDValue = ""
    End Sub
End Class
