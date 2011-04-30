Imports System.Runtime.InteropServices

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
    ReadOnly Property Maximum() As Double

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
    ''' Readonly Collection of ControllerDeviceState objects providing names for specific device states. These are simple triplets of 
    ''' lower Value, upper Value and state name.
    ''' If CanReturnState is TRUE then get is MANDATORY, otherwise OPTIONAL
    ''' </summary>
    ReadOnly Property NamedDeviceStates() As ArrayList

End Interface