Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the IFocuser interface.</summary>
'-----------------------------------------------------------------------

''' <summary>
''' Provides universal access to Focuser drivers
''' </summary>
<Guid("E430C8A8-539E-4558-895D-A2F293D946E7"), ComVisible(True)> _
Public Interface IFocuser 'C2E3FE9C-01CD-440C-B8E3-C56EE9E4EDBC
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
    ''' <returns>A string response and sets the <c>IDeviceControl.LastResult</c> property.</returns>
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
    ''' True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
    ''' </summary>
    ReadOnly Property Absolute() As Boolean

    ''' <summary>
    ''' Immediately stop any focuser motion due to a previous Move() method call.
    ''' Some focusers may not support this function, in which case an exception will be raised. 
    ''' Recommendation: Host software should call this method upon initialization and,
    ''' if it fails, disable the Halt button in the user interface. 
    ''' </summary>
    Sub Halt()

    ''' <summary>
    ''' True if the focuser is currently moving to a new position. False if the focuser is stationary.
    ''' </summary>
    ReadOnly Property IsMoving() As Boolean

    ''' <summary>
    ''' State of the connection to the focuser.
    ''' et True to start the link to the focuser; set False to terminate the link. 
    ''' The current link status can also be read back as this property. 
    ''' An exception will be raised if the link fails to change state for any reason. 
    ''' </summary>
    Property Link() As Boolean

    ''' <summary>
    ''' Maximum increment size allowed by the focuser; 
    ''' i.e. the maximum number of steps allowed in one move operation.
    ''' For most focusers this is the same as the MaxStep property.
    ''' This is normally used to limit the Increment display in the host software. 
    ''' </summary>
    ReadOnly Property MaxIncrement() As Integer

    ''' <summary>
    ''' Maximum step position permitted.
    ''' The focuser can step between 0 and MaxStep. 
    ''' If an attempt is made to move the focuser beyond these limits,
    ''' it will automatically stop at the limit. 
    ''' </summary>
    ReadOnly Property MaxStep() As Integer

    ''' <summary>
    ''' Step size (microns) for the focuser.
    ''' Raises an exception if the focuser does not intrinsically know what the step size is. 
    ''' </summary>
    ''' <param name="val"></param>
    Sub Move(ByVal val As Integer)

    ''' <summary>
    ''' Current focuser position, in steps.
    ''' Valid only for absolute positioning focusers (see the Absolute property).
    ''' An exception will be raised for relative positioning focusers.   
    ''' </summary>
    ReadOnly Property Position() As Integer

    ''' <summary>
    ''' Step size (microns) for the focuser.
    ''' Raises an exception if the focuser does not intrinsically know what the step size is. 
    ''' 
    ''' </summary>
    ReadOnly Property StepSize() As Double

    ''' <summary>
    ''' The state of temperature compensation mode (if available), else always False.
    ''' If the TempCompAvailable property is True, then setting TempComp to True
    ''' puts the focuser into temperature tracking mode. While in temperature tracking mode,
    ''' Move commands will be rejected by the focuser. Set to False to turn off temperature tracking.
    ''' An exception will be raised if TempCompAvailable is False and an attempt is made to set TempComp to true. 
    ''' 
    ''' </summary>
    Property TempComp() As Boolean

    ''' <summary>
    ''' True if focuser has temperature compensation available.
    ''' Will be True only if the focuser's temperature compensation can be turned on and off via the TempComp property. 
    ''' </summary>
    ReadOnly Property TempCompAvailable() As Boolean

    ''' <summary>
    ''' Current ambient temperature as measured by the focuser.
    ''' Raises an exception if ambient temperature is not available.
    ''' Commonly available on focusers with a built-in temperature compensation mode. 
    ''' </summary>
    ReadOnly Property Temperature() As Double
#End Region

End Interface