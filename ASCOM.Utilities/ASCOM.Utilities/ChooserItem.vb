
Imports System.Net
''' <summary>
''' Data class for items that are added to the Chooser display combo box
''' </summary>
Public Class ChooserItem
    Implements IComparable ' Provides support for the device list combo box to sort items into a display order

    ''' <summary>
    ''' Base initialiser, sets all properties to default values
    ''' </summary>
    Public Sub New()
        ChooserID = Guid.NewGuid
        IsComDriver = False
        ProgID = ""
        AscomName = ""
        DeviceNumber = 0
        HostName = IPAddress.Loopback.ToString()
        Port = 0
    End Sub

    ''' <summary>
    ''' Initialiser called to create an item for a COM driver
    ''' </summary>
    ''' <param name="progId">The driver's ProgID</param>
    ''' <param name="ascomName">The driver's display name</param>
    Public Sub New(progId As String, ascomName As String)
        Me.New()
        Me.ProgID = progId
        Me.AscomName = ascomName
        Me.IsComDriver = True
        Me.DisplayName = ascomName
    End Sub

    ''' <summary>
    ''' Initialiser called to create an item for a new Alpaca driver i.e. one that is not already fronted by a COM driver 
    ''' </summary>
    ''' <param name="deviceNumber">The Alpaca device access number</param>
    ''' <param name="hostName">The host name (or IP address) used to access the Alpaca device</param>
    ''' <param name="port">The Alpaca port number</param>
    ''' <param name="ascomName">The device's display name</param>
    Public Sub New(deviceUniqueId As String, deviceNumber As Integer, hostName As String, port As Integer, ascomName As String, displayName As String)
        Me.New()
        Me.DeviceUniqueID = deviceUniqueId
        Me.DeviceNumber = deviceNumber
        Me.HostName = hostName
        Me.Port = port
        Me.AscomName = ascomName
        Me.IsComDriver = False
        Me.DisplayName = displayName
    End Sub

    ''' <summary>
    ''' ID that is unique within this list of Chooser items, just used to ensure that drivers that have the same display name appear differently
    ''' </summary>
    ''' <returns>The Chooser item's unique ID</returns>
    Public Property ChooserID As Guid

    ''' <summary>
    ''' ID that is globally unique for this Alpaca device
    ''' </summary>
    ''' <returns>ASCOM device's unique ID</returns>
    Public Property DeviceUniqueID As String

    ''' <summary>
    ''' Flag indicating whether this is a COM or new Alpaca driver
    ''' </summary>
    ''' <returns>True if the item is a new Alpaca driver, False if the item is an existing COM driver</returns>
    ''' <remarks>Pre-existing COM drivers that front Alpaca devices are flagged as COM drivers. Only newly discovered Alpaca devices are flagged as such</remarks>
    Public ReadOnly Property IsComDriver As Boolean

    ''' <summary>
    ''' The COM ProgID
    ''' </summary>
    ''' <returns></returns>
    Public Property ProgID As String

    ''' <summary>
    ''' The device's ASCOM device name
    ''' </summary>
    ''' <returns></returns>
    Public Property AscomName As String

    ''' <summary>
    ''' The device's Display name that will be used in the Chooser list
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' For COM drivers this property has the same value as the AscomName property
    ''' For Alpaca devices this is the AscomName but may be prefixed with information about whether this is an existing or a new Alpaca device.
    ''' The Chooser combo box uses this property as the display variable rather than the AscomName property so that the additional Alpaca information can be included when required
    ''' </remarks>
    Public Property DisplayName As String

    ''' <summary>
    ''' The Alpaca device access number
    ''' </summary>
    ''' <returns></returns>
    Public Property DeviceNumber As Integer

    ''' <summary>
    ''' The host name or IP address of the Alpaca device
    ''' </summary>
    ''' <returns></returns>
    Public Property HostName As String

    ''' <summary>
    ''' The Alpaca IP port through which the device can be accessed
    ''' </summary>
    ''' <returns></returns>
    Public Property Port As Integer

    ''' <summary>
    ''' Compares two ChooserItems items based on the Name field concatenated with a unique GUID
    ''' </summary>
    ''' <param name="otherChooserItemAsObject"></param>
    ''' <returns>Less than zero if this instance precedes the other item in the sort order or
    '''          Zero if the items occupy the same position in the sort order or 
    '''          Greater than zero if this instance comes after the other item in the sort order</returns>
    ''' <remarks>The concatenation is used to ensure that tow entries with identical descriptive names can be seen as distinct devices.</remarks>
    Public Function CompareTo(otherChooserItemAsObject As Object) As Integer Implements IComparable.CompareTo
        Dim myNameId, otherNameId As String

        Dim otherChooserItem As ChooserItem = CType(otherChooserItemAsObject, ChooserItem)

        If Me.IsComDriver Then ' COM driver so sort based on name And ProgID
            myNameId = $"{Me.AscomName}{Me.ProgID}"
            otherNameId = $"{otherChooserItem.AscomName}{otherChooserItem.ProgID}"
        Else ' Alpaca device so sort based on name and ChooserID
            myNameId = $"{Me.AscomName}{Me.ChooserID}"
            otherNameId = $"{otherChooserItem.AscomName}{otherChooserItem.ChooserID}"
        End If

        Return myNameId.CompareTo(otherNameId)
    End Function
End Class
