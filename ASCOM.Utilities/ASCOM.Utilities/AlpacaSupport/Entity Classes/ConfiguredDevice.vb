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

    ''' <summary>
    ''' Initialises the class with default values
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
    Friend Sub New(ByVal deviceName As String, ByVal deviceType As String, ByVal deviceNumber As Integer, ByVal uniqueID As String)
        Me.DeviceName = deviceName
        Me.DeviceType = deviceType
        Me.DeviceNumber = deviceNumber
        Me.UniqueID = uniqueID
    End Sub

    ''' <summary>
    ''' ASCOM device name
    ''' </summary>
    Public Property DeviceName As String Implements IConfiguredDevice.DeviceName

    ''' <summary>
    ''' ASCOM device type
    ''' </summary>
    Public Property DeviceType As String Implements IConfiguredDevice.DeviceType

    ''' <summary>
    ''' Device number used to access the device through the Alpaca API
    ''' </summary>
    Public Property DeviceNumber As Integer Implements IConfiguredDevice.DeviceNumber

    ''' <summary>
    ''' ASCOM device unique ID
    ''' </summary>
    Public Property UniqueID As String Implements IConfiguredDevice.UniqueID

End Class
