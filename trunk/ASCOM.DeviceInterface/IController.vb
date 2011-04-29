Imports System.Runtime.InteropServices
Imports System.Collections

''' <summary>
''' This proposal has just one device type that can represent both digital and analogue devices. This makes the interface slightly more complex
''' but means we loose the complexity of supporting many differnt ASCOM device types.
''' 
''' The ASCOM Controller interface has all the common members plus a single method to get a collection of ControllerDevices
''' 
''' Each device is described by three properties:
'''     CanReturnState - Is able to say what the device state actually is
'''     CanSetState - Can directly control the device
''' 
''' This allows three generic types of device to be supported:
'''     BLIND Controller - Can set values but does not provide feedback on device state
'''     FULL Controller - Can set values and return current device state
'''     READOUT Controller - Can not set values but does return the state of a device (I appreciate this isn't control, but request some license on naming!)
''' 
''' ControllerDevices can be either analogue or digital in nature as indicated by the IsDigital flag. (See IsDigital description for further detail on this)
''' 
''' Thus, one device driver can return a mixture of digital and analogue devices with varying capabilties as described above and implementation
''' detail described below.
''' 
''' The interface specification for setting values supports both blocking behaviour non-blocking / polling behaviuour for changes that 
''' could take an extended period to take effect.
''' 
''' To remove the need for different controller interfaces for digital and analogue devices, all state values: Maximum, Minimum, Value etc.
''' are defined as doubles. It is expected that the Driver will be implemented only to respond to discrete values of these when the
''' device flags itself as a digital device. i.e. a ToggleSwitch device would be expected to respond just to values of 0.0 and 1.0, if it has set these
''' as the Minimum and Maximum values. It has the choice of either rounding the input to nearest value or throwing an InvalidValueException.
''' 
''' Devices with explicit state values or state ranges can retun these in the DeviceStates collection (see the DeviceState interface entry for more detail)
''' Each device has a minimum and maximum value and a State corresponding to its current setting in the range between min and max.
''' 
''' ** New Polymorphic View Idea** There is a boolean "On" property that provides a simple On/Off VIEW of the device. 
''' When set to TRUE the device will set state to its MAXIMUM value and when set to FALSE will set to its MINIMUM value.
''' When read, it will return FALSE if the State is at the MINIMUM value and TRUE under all other circumstances.
''' 
''' This provides a very simply view of a digital toggle switch for on/off purposes.
''' 
''' MANDATORY methods can't throw NotImplemented exceptions
''' OPTIONAL methods can throw NotImplemented exceptions.
''' </summary>
<ComVisible(True), Guid("C06EBEDA-FAC9-4904-BF8D-A95D72926079"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface IController

#Region "Common Methods"
    ''' <summary>
    ''' Set True to connect to the device. Set False to disconnect from the device.
    ''' You can also read the property to check whether the device is connected.
    ''' </summary>
    ''' <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
    ''' <exception cref="DriverException">Must throw exception if unsuccessful.</exception>
    Property Connected() As Boolean

    ''' <summary>
    ''' Returns a description of the driver, such as manufacturer and model
    ''' number. Any ASCII characters may be used. The string shall not exceed 68
    ''' characters (for compatibility with FITS headers).
    ''' </summary>
    ''' <value>The description.</value>
    ''' <exception cref="DriverException">Must throw an exception if description unavailable</exception>
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
    ''' Not to be confused with the InterfaceVersion property, which is the version of the ASCOM specification supported by the driver.
    ''' This is only available for telescope InterfaceVersions 2 and 3
    ''' </summary>
    ReadOnly Property DriverVersion() As String

    ''' <summary>
    ''' The ASCOM device interface version that this driver supports.
    ''' This is not implemented in Telescope Interface version 1, these will raise an error,
    ''' this should be interpreted as InterfaceVersion 1
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
    ''' <exception cref="MethodNotImplementedException">Must throw an exception if Setup dialog is unavailable.</exception>
    Sub SetupDialog()

    ''' <summary>
    ''' Invokes the specified device-specific action.
    ''' This is only available for telescope InterfaceVersion 3
    ''' </summary>
    ''' <param name="ActionName">
    ''' A well known name agreed by interested parties that represents the action to be carried out. 
    ''' </param>
    ''' <param name="ActionParameters">
    ''' List of required parameters in an agreed format or an empty string <see cref="String.Empty"/> if none are required.
    ''' </param>
    ''' <returns>A string response in an agreed format</returns>
    ''' <example>Suppose filter wheels start to appear with automatic wheel changers; new actions could 
    ''' be “FilterWheel:QueryWheels” and “FilterWheel:SelectWheel”. The former returning a 
    ''' formatted list of wheel names and the second taking a wheel name and making the change.
    ''' </example>
    Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String

    ''' <summary>
    ''' Gets a list of supported actions.
    ''' This is only available for telescope InterfaceVersion 3
    ''' </summary>
    ''' <value>A collection of string values, the supported action names.</value>
    ''' <remarks>This is a mandatory parameter and cannot return a PropertyNotImplemented exception. If your driver does not support any 
    ''' actions then you should return an empty collection.
    ''' <para>An array list collection has been selected as the vehicle for the action names in order to make it easier for clients to
    ''' determine whether a particular action is supported. This is easily done through the Contains method. Since the
    ''' collection is also ennumerable it is easy to use constructs such as For Each ... to operate on members without having to be concerned 
    ''' about hom many members are in the collection. </para>
    ''' <para>Collections have been used in the Telescope specification for a number of years and are known to be compatible with COM. Within .NET
    ''' the ArrayList is the correct implementation to use as the .NET Generic methods are not compatible with COM.</para></remarks>
    ReadOnly Property SupportedActions() As ArrayList

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
    ''' Returns a collection of ControllerDevice devices
    ''' </summary>
    ''' <value>An <c>Arraylist</c> collection of IControllerDevice objects</value>
    ''' <remarks></remarks>
    ReadOnly Property ControllerDevices() As ArrayList

    ''' <summary>
    ''' Returns a named ControllerDevice object
    ''' </summary>
    ''' <param name="Name">Name=name of ControllerDevice to return</param>
    ''' <returns>Returns a specific ControllerDevice object</returns> 
    Function GetControl(ByVal Name As String) As Object

    ''' <summary>
    ''' Sets a named device to a particular value or state
    ''' </summary>
    ''' <param name="Name">Name of the controller to set on or off</param> 
    ''' <param name="Value">Value to be assigned to the controller</param> 
    Sub SetControl(ByVal Name As String, ByVal Value As Double)

#End Region

End Interface