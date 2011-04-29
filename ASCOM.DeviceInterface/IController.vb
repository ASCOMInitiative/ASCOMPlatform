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
    Function GetSwitch(ByVal Name As String) As Object

    ''' <summary>
    ''' Sets a controller on or off
    ''' </summary>
    ''' <param name="Name">Name of the controller to set on or off</param> 
    ''' <param name="State">True or False for On or Off</param> 
    Sub SetSwitch(ByVal Name As String, ByVal State As Boolean)

#End Region

End Interface

''' <summary>
''' Controller device description supporting capabilities described in the Controller interface
''' </summary>
<ComVisible(True), Guid("8D032FCB-FDA1-49CC-B687-715A18B10731"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface IControllerDevice
    ''' <summary>
    ''' MANDATORY - Readonly Name of this controller e.g. "Dome Lights Off or Power Down Scope" or "Lighting Level" or "Cooler Temperature"
    ''' </summary>
    ReadOnly Property Name() As String

    ''' <summary>
    ''' MANDATORY - Readonly Flag indicating whether this controller can return the device's state
    ''' If TRUE then Value is mandatory
    ''' </summary>
    ReadOnly Property CanReturnState() As Boolean

    ''' <summary>
    ''' MANDATORY - Readonly Flag indicating whether this controller can set the device's state
    ''' </summary>
    ReadOnly Property CanSetState() As Boolean

    ''' <summary>
    ''' Readonly Boolean on/off view of this controller. Returns FALSE if device state is at MINIMUM value otherwise returns TRUE
    ''' When set to TRUE the controller value will be set to its MAXIMUM value, when set FALSE it will be set to its MINIMUM value.
    ''' If CanReturnState is TRUE then get is MANDATORY, otherwise OPTIONAL
    ''' if CanSetState is TRUE then set is MANDATORY, otherwise OPTIONAL
    ''' </summary>
    Property [On]() As Boolean

    ''' <summary>
    ''' Readonly Lowest state value that this controller supports. e.g. 0 for a toggle or nway switch or an "off" power controller
    ''' If CanReturnState is TRUE then get is MANDATORY, otherwise OPTIONAL
    ''' </summary>
    ReadOnly Property Minimum() As Double

    ''' <summary>
    ''' Readonly Highest Value that this controller presents e.g. "1" for a toggle switch or "6" for a 6-way switch that has a 
    ''' minimum value of "1" or "100" for a "full on" power controller.
    ''' If CanReturnState is TRUE then get is MANDATORY, otherwise OPTIONAL
    ''' </summary>
    ReadOnly Property Maximim() As Double

    ''' <summary>
    ''' MANDATORY - Readonly Flag indicating whether this controller has a continuous range i.e. is analogue in nature or whether it
    ''' is digital in nature supporting a number of discrete values represented by integer values for Max, Min and Value
    ''' </summary>
    ReadOnly Property StepSize() As Double

    ''' <summary>
    ''' Sets the required value of the device, the value should be between Min and Max. e.g. 1 for an "On" toggleswitch or 3 for position 3/6 
    ''' for the 6-way switch having positions 1 to 6. Values outside this range will throw an InvalidValueException
    ''' 
    ''' I would expect this call to block for short periods where change can be effected quickly e.g. a toggleswitch and on completion 
    ''' should return a value of TRUE. 
    ''' 
    ''' Some devices  could take an extended period to respond if the attribute you are measuring is not what you are controlling. E.g. a 
    ''' camera cooler control could set and measure chip temperature but may actually control cooler power input. So, you could 
    ''' change the set point very quickly but the cooler temperature may take some minutes to get to the required value.
    ''' 
    ''' When a change has been made, but may take an extended period to come into effect, this call should not block but should 
    ''' exit with a return value of FALSE. FALSE indicates that PresentValue should be polled to determine the current state of the 
    ''' controller.
    ''' 
    ''' if CanSetState is TRUE, this method is MANDATORY, otherwise OPTIONAL
    ''' </summary>
    ''' <param name="NewValue">The new required value of the controller</param>
    ''' <returns>Boolean TRUE if the device is now in the expected state, FALSE if PresentValue should be polled.</returns>
    Function SetValue(ByVal NewValue As Double) As Boolean

    ''' <summary>
    ''' Readonly Returns the present set point or value for the device as set by the last call to SetValue or as initialised by the 
    ''' controller at startup.
    ''' If CanReturnState is TRUE then get is MANDATORY, otherwise OPTIONAL
    ''' </summary>
    ReadOnly Property PresentSetPoint() As Double

    ''' <summary>
    ''' Readonly Present value of the device, a value between Min and Max. e.g. 1 for an "On" toggleswitch or 3 for position 3/5 
    ''' for the 6-way switch.
    ''' If CanReturnState is TRUE then get is MANDATORY, otherwise OPTIONAL
    ''' </summary>
    ReadOnly Property PresentValue() As Double

    ''' <summary>
    ''' Readonly Name of this state, if it has one. Should return an empty string or one of the states returned in the DeviceStates collection.
    ''' </summary>
    ''' If CanReturnState is TRUE then get is MANDATORY, otherwise OPTIONAL
    ReadOnly Property StateName() As String

    ''' <summary>
    ''' Readonly Collection of DeviceState objects providing names for specific device states. These are simple triplets of 
    ''' lower Value, upper Value and state name.
    ''' If CanReturnState is TRUE then get is MANDATORY, otherwise OPTIONAL
    ''' </summary>
    ReadOnly Property NamedDeviceStates() As ArrayList

End Interface

''' <summary>
''' A simple class relating a Value range to its descriptive name. 
''' 
''' Digital devices should return the same value in both ValueStart and ValueEnd to indicate that this name applies just to that discrete Value.
''' 
''' All devices should return descriptions for their MINIMUM and MAXIMUM Values. Other entries are optional.
''' 
''' It is not mandatory to have a description for every state, only MINIMUM and MAXIMUM are mandatory. This opens the way to a 
''' digital rheostat device that could have a thousand states most of which don't have special names. 
''' 
''' For analogue devices this could return a description such as "Quarter Power" for a range of values such as when the value is 
''' in the range 1-25% of max.
''' </summary>
<ComVisible(True), Guid("F92B5110-57EA-4B9E-8C3F-3A45F25B54DE"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface IDeviceState
    ''' <summary>
    ''' MANDATORY - Starting value of this named range
    ''' </summary>
    ReadOnly Property StartValue() As Double

    ''' <summary>
    ''' MANDATORY - Ending value of this named range. Can be the same as StartValue to create a single valued state
    ''' </summary>
    ReadOnly Property EndValue() As Double

    ''' <summary>
    ''' MANDATORY - Name of this state or range
    ''' </summary>
    ReadOnly Property Name() As String
End Interface