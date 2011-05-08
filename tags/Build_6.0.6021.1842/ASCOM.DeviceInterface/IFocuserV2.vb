Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the IFocuser interface.</summary>
'-----------------------------------------------------------------------

''' <summary>
''' Provides universal access to Focuser drivers
''' </summary>
<Guid("E430C8A8-539E-4558-895D-A2F293D946E7"), ComVisible(True), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface IFocuserV2 'C2E3FE9C-01CD-440C-B8E3-C56EE9E4EDBC
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
    ''' number. Any ASCII characters may be used. For Camera devices, the string shall not exceed 68
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
    ''' A well known name agreed by interested parties that represents the action to be carried out. 
    ''' </param>
    ''' <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.
    ''' </param>
    ''' <returns>A string response. The meaning of returned strings is set by the driver author.</returns>
    ''' <example>Suppose filter wheels start to appear with automatic wheel changers; new actions could 
    ''' be “FilterWheel:QueryWheels” and “FilterWheel:SelectWheel”. The former returning a 
    ''' formatted list of wheel names and the second taking a wheel name and making the change, returning appropriate 
    ''' values to indicate success or failure.
    ''' </example>
    ''' <remarks>
    ''' This method is intended for use in all current and future device types and to avoid name clashes, management of action names 
    ''' is important from day 1. A two-part naming convention will be adopted - <b>DeviceType:UniqueActionName</b> where:
    ''' <list type="bullet">
    ''' <item><description>DeviceType is the same value as would be used by <see cref="ASCOM.Utilities.Chooser.DeviceType"/> e.g. Telescope, Camera, Switch etc.</description></item>
    ''' <item><description>UniqueActionName is a single word, or multiple words joined by underscore characters, that sensibly describes the action to be performed.</description></item>
    ''' </list>
    ''' <para>
    ''' It is recommended that UniqueActionNames should be a maximum of 16 characters for legibility.
    ''' Should the same function and UniqueActionName be supported by more than one type of device, the reserved DeviceType of 
    ''' “General” will be used. Action names will be case insensitive, so FilterWheel:SelectWheel, filterwheel:selectwheel 
    ''' and FILTERWHEEL:SELECTWHEEL will all refer to the same action.</para>
    ''' <para>The names of all supported actions must bre returned in the <see cref="SupportedActions"/> property.</para>
    ''' </remarks>
    ''' <exception cref="ASCOM.MethodNotImplementedException">Throws this exception if no actions are suported.</exception>
    ''' <exception cref="ASCOM.ActionNotImplementedException">It is intended that the SupportedActions method will inform clients 
    ''' of driver capabilities, but the driver must still throw an ASCOM.ActionNotImplemented exception if it is asked to 
    ''' perform an action that it does not support.</exception>
    Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String

    ''' <summary>
    ''' Returns the list of action names supported by this driver.
    ''' </summary>
    ''' <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
    ''' <remarks>This method must return an empty arraylist if no actions are supported. Please do not throw a 
    ''' <see cref="ASCOM.PropertyNotImplementedException" />.
    ''' <para>This is an aid to client authors and testers who would otherwise have to repeatedly poll the driver to determine its capabilities. 
    ''' Returned action names may be in mixed case to enhance presentation but  will be recognised case insensitively in 
    ''' the <see cref="Action"/> method.</para>
    '''<para>An array list collection has been selected as the vehicle for  action names in order to make it easier for clients to
    ''' determine whether a particular action is supported. This is easily done through the Contains method. Since the
    ''' collection is also ennumerable it is easy to use constructs such as For Each ... to operate on members without having to be concerned 
    ''' about hom many members are in the collection. </para>
    ''' <para>Collections have been used in the Telescope specification for a number of years and are known to be compatible with COM. Within .NET
    ''' the ArrayList is the correct implementation to use as the .NET Generic methods are not compatible with COM.</para>
    ''' </remarks>
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
    '''  Moves the focuser by the specified amount or to the specified position depending on the value of the Absolute property.
    ''' </summary>
    ''' <param name="Value">Step distance or absolute position, depending on the value of the Absolute property.</param>
    ''' <remarks>If the Absolute property is True, then this is an absolute positioning focuser. The Move command tells the focuser 
    ''' to move to an exact step position, and the Position parameter of the Move() method is an integer between 0 and MaxStep.
    ''' <para>If the Absolute property is False, then this is a relative positioning focuser. The Move command tells the focuser to move in a relative direction, and the Position parameter of the Move() method (in this case, step distance) is an integer between minus MaxIncrement and plus MaxIncrement.</para>
    '''</remarks>
    Sub Move(ByVal Value As Integer)

    ''' <summary>
    ''' Current focuser position, in steps.
    ''' Valid only for absolute positioning focusers (see the Absolute property).
    ''' An exception will be raised for relative positioning focusers.   
    ''' </summary>
    ReadOnly Property Position() As Integer

    ''' <summary>
    ''' Step size (microns) for the focuser.
    ''' Raises an exception if the focuser does not intrinsically know what the step size is. 
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