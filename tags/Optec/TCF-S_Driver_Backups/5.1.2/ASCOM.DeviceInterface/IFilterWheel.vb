Imports System.Runtime.InteropServices
Imports ASCOM.Conform
'-----------------------------------------------------------------------
' <summary>Defines the IFilterWheel Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Defines the IFilterWheel Interface
''' </summary>
<Guid("DCF3858D-D68E-45ed-8141-1C899B4B432A"), ComVisible(True), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface IFilterWheel '756FD725-A6E2-436F-8C7A-67E358622027
    '    Inherits IAscomDriver
    '    Inherits IDeviceControl
#Region "Common Methods"
    'IAscomDriver Methods

    ''' <summary>
    ''' Set True to enable the link. Set False to disable the link.
    ''' You can also read the property to check whether it is connected.
    ''' </summary>
    ''' <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
    ''' <exception cref=" System.Exception">Must throw exception if unsuccessful.</exception>
    Property Connected() As Boolean

    ''' <summary>
    ''' Returns a description of the driver, such as manufacturer and model
    ''' number. Any ASCII characters may be used. The string shall not exceed 68
    ''' characters (for compatibility with FITS headers).
    ''' </summary>
    ''' <value>The description.</value>
    ''' <exception cref=" System.Exception">Must throw exception if description unavailable</exception>
    ReadOnly Property Description() As String

    ''' <summary>
    ''' Descriptive and version information about this ASCOM driver.
    ''' This string may contain line endings and may be hundreds to thousands of characters long.
    ''' It is intended to display detailed information on the ASCOM driver, including version and copyright data.
    ''' See the Description property for descriptive info on the telescope itself.
    ''' To get the driver version in a parseable string, use the DriverVersion property.
    ''' </summary>
    ReadOnly Property DriverInfo() As String

    ''' <summary>
    ''' A string containing only the major and minor version of the driver.
    ''' This must be in the form "n.n".
    ''' Not to be confused with the InterfaceVersion property, which is the version of this specification supported by the driver (currently 2). 
    ''' </summary>
    ReadOnly Property DriverVersion() As String

    ''' <summary>
    ''' The version of this interface. Will return 2 for this version.
    ''' Clients can detect legacy V1 drivers by trying to read ths property.
    ''' If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1. 
    ''' In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver. 
    ''' </summary>
    ReadOnly Property InterfaceVersion() As Short

    ''' <summary>
    ''' The short name of the driver, for display purposes
    ''' </summary>
    ReadOnly Property Name() As String

    ''' <summary>
    ''' Launches a configuration dialog box for the driver.  The call will not return
    ''' until the user clicks OK or cancel manually.
    ''' </summary>
    ''' <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
    Sub SetupDialog()

    'DeviceControl Methods

    ''' <summary>
    ''' Invokes the specified device-specific action.
    ''' </summary>
    ''' <param name="ActionName">
    ''' A well known name agreed by interested parties that represents the action
    ''' to be carried out. 
    ''' <example>suppose filter wheels start to appear with automatic wheel changers; new actions could 
    ''' be “FilterWheel:QueryWheels” and “FilterWheel:SelectWheel”. The former returning a 
    ''' formatted list of wheel names and the second taking a wheel name and making the change.
    ''' </example>
    ''' </param>
    ''' <param name="ActionParameters">
    ''' List of required parameters or <see cref="String.Empty"/>  if none are required.
    ''' </param>
    ''' <returns>A string response if successful or an exception if not.</returns>
    Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String

    ''' <summary>
    ''' Gets the supported actions.
    ''' </summary>
    ''' <value>The supported actions.</value>
    ReadOnly Property SupportedActions() As String()

    ''' <summary>
    ''' Transmits an arbitrary string to the device and does not wait for a response.
    ''' Optionally, protocol framing characters may be added to the string before transmission.
    ''' </summary>
    ''' <param name="Command">The literal command string to be transmitted.</param>
    ''' <param name="Raw">
    ''' if set to <c>true</c> the string is transmitted 'as-is'.
    ''' If set to <c>false</c> then protocol framing characters may be added prior to transmission.
    ''' </param>
    Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False)

    ''' <summary>
    ''' Transmits an arbitrary string to the device and waits for a boolean response.
    ''' Optionally, protocol framing characters may be added to the string before transmission.
    ''' </summary>
    ''' <param name="Command">The literal command string to be transmitted.</param>
    ''' <param name="Raw">
    ''' if set to <c>true</c> the string is transmitted 'as-is'.
    ''' If set to <c>false</c> then protocol framing characters may be added prior to transmission.
    ''' </param>
    ''' <returns>
    ''' Returns the interpreted boolean response received from the device.
    ''' </returns>
    Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean

    ''' <summary>
    ''' Transmits an arbitrary string to the device and waits for a string response.
    ''' Optionally, protocol framing characters may be added to the string before transmission.
    ''' </summary>
    ''' <param name="Command">The literal command string to be transmitted.</param>
    ''' <param name="Raw">
    ''' if set to <c>true</c> the string is transmitted 'as-is'.
    ''' If set to <c>false</c> then protocol framing characters may be added prior to transmission.
    ''' </param>
    ''' <returns>
    ''' Returns the string response received from the device.
    ''' </returns>
    Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String

    ''' <summary>
    ''' Dispose the late-bound interface, if needed. Will release it via COM
    ''' if it is a COM object, else if native .NET will just dereference it
    ''' for GC.
    ''' </summary>
    Sub Dispose()

#End Region

#Region "Device Methods"
    ''' <summary>
    ''' For each valid slot number (from 0 to N-1), reports the focus offset for
    ''' the given filter position.  These values are focuser- and filter
    ''' -dependent, and  would usually be set up by the user via the SetupDialog.
    ''' The number of slots N can be determined from the length of the array.
    ''' If focuser offsets are not available, then it should report back 0 for all
    ''' array values.
    ''' </summary>
    ReadOnly Property FocusOffsets() As Integer()

    ''' <summary>
    ''' For each valid slot number (from 0 to N-1), reports the name given to the
    ''' filter position.  These names would usually be set up by the user via the
    ''' SetupDialog.  The number of slots N can be determined from the length of
    ''' the array.  If filter names are not available, then it should report back
    ''' "Filter 1", "Filter 2", etc.
    ''' </summary>
    ReadOnly Property Names() As String()

    ''' <summary>
    ''' Write number between 0 and N-1, where N is the number of filter slots (see
    ''' Filter.Names). Starts filter wheel rotation immediately when written*. Reading
    ''' the property gives current slot number (if wheel stationary) or -1 if wheel is
    ''' moving. This is mandatory; valid slot numbers shall not be reported back while
    ''' the filter wheel is rotating past filter positions.
    ''' 
    ''' Note that some filter wheels are built into the camera (one driver, two
    ''' interfaces).  Some cameras may not actually rotate the wheel until the
    ''' exposure is triggered.  In this case, the written value is available
    ''' immediately as the read value, and -1 is never produced.
    ''' </summary>
    Property Position() As Short
#End Region

End Interface