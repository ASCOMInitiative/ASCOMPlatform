Imports System.Runtime.InteropServices
'Imports ASCOM.Conform
'-----------------------------------------------------------------------
' <summary>Defines the IFilterWheel Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Defines the IFilterWheel Interface
''' </summary>
<Guid("DCF3858D-D68E-45ed-8141-1C899B4B432A"), ComVisible(True), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface IFilterWheelV2 '756FD725-A6E2-436F-8C7A-67E358622027
    '    Inherits IAscomDriver
    '    Inherits IDeviceControl

#Region "Common Methods"
    'IAscomDriver Methods

    ''' <summary>
    ''' Set True to connect to the device. Set False to disconnect from the device.
    ''' You can also read the property to check whether it is connected.
    ''' </summary>
    ''' <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p>Do not use a NotConnectedException here, that exception is for use in other methods that require a connection in order to succeed.</remarks>
    Property Connected() As Boolean

    ''' <summary>
    ''' Returns a description of the device, such as manufacturer and modelnumber. Any ASCII characters may be used. 
    ''' </summary>
    ''' <value>The description.</value>
    ''' <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
    ReadOnly Property Description() As String

    ''' <summary>
    ''' Descriptive and version information about this ASCOM driver.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented</b></p> This string may contain line endings and may be hundreds to thousands of characters long.
    ''' It is intended to display detailed information on the ASCOM driver, including version and copyright data.
    ''' See the <see cref="Description" /> property for information on the device itself.
    ''' To get the driver version in a parseable string, use the <see cref="DriverVersion" /> property.
    ''' </remarks>
    ReadOnly Property DriverInfo() As String

    ''' <summary>
    ''' A string containing only the major and minor version of the driver.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> This must be in the form "n.n".
    ''' It should not to be confused with the <see cref="InterfaceVersion" /> property, which is the version of this specification supported by the 
    ''' driver.
    ''' </remarks>
    ReadOnly Property DriverVersion() As String

    ''' <summary>
    ''' The interface version number that this device supports. Should return 2 for this interface version.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> Clients can detect legacy V1 drivers by trying to read ths property.
    ''' If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1. 
    ''' In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver.
    ''' </remarks>
    ReadOnly Property InterfaceVersion() As Short

    ''' <summary>
    ''' The short name of the driver, for display purposes
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
    ReadOnly Property Name() As String

    ''' <summary>
    ''' Launches a configuration dialog box for the driver.  The call will not return
    ''' until the user clicks OK or cancel manually.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
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
    ''' <exception cref="ASCOM.MethodNotImplementedException">Throws this exception if no actions are suported.</exception>
    ''' <exception cref="ASCOM.ActionNotImplementedException">It is intended that the SupportedActions method will inform clients 
    ''' of driver capabilities, but the driver must still throw an ASCOM.ActionNotImplemented exception if it is asked to 
    ''' perform an action that it does not support.</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <example>Suppose filter wheels start to appear with automatic wheel changers; new actions could 
    ''' be “FilterWheel:QueryWheels” and “FilterWheel:SelectWheel”. The former returning a 
    ''' formatted list of wheel names and the second taking a wheel name and making the change, returning appropriate 
    ''' values to indicate success or failure.
    ''' </example>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> 
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
    ''' <para>The names of all supported actions must be returned in the <see cref="SupportedActions"/> property.</para>
    ''' </remarks>
    Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String

    ''' <summary>
    ''' Returns the list of action names supported by this driver.
    ''' </summary>
    ''' <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented</b></p> This method must return an empty arraylist if no actions are supported. Please do not throw a 
    ''' <see cref="ASCOM.PropertyNotImplementedException" />.
    ''' <para>This is an aid to client authors and testers who would otherwise have to repeatedly poll the driver to determine its capabilities. 
    ''' Returned action names may be in mixed case to enhance presentation but  will be recognised case insensitively in 
    ''' the <see cref="Action">Action</see> method.</para>
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
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
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
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
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
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
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
    ''' Focus offset of each filter in the wheel
    '''</summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' For each valid slot number (from 0 to N-1), reports the focus offset for the given filter position.  These values are focuser and filter dependent, and  would usually be set up by the user via 
    ''' the SetupDialog. The number of slots N can be determined from the length of the array. If focuser offsets are not available, then it should report back 0 for all array values.
    ''' </remarks>
    ReadOnly Property FocusOffsets() As Integer()

    ''' <summary>
    ''' Name of each filter in the wheel
    '''</summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' For each valid slot number (from 0 to N-1), reports the name given to the filter position.  These names would usually be set up by the user via the
    ''' SetupDialog.  The number of slots N can be determined from the length of the array.  If filter names are not available, then it should report back "Filter 1", "Filter 2", etc.
    ''' </remarks>
    ReadOnly Property Names() As String()

    ''' <summary>
    ''' Sets or returns the current filter wheel position
    ''' </summary>
    ''' <exception cref="InvalidValueException">Must throw an InvalidValueException if an invalid position is set</exception>
    ''' <exception cref="NotConnectedException">Must throw an exception if the Filter Wheel is not connected</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' Write a position number between 0 and N-1, where N is the number of filter slots (see <see cref="Names"/>). Starts filter wheel rotation immediately when written. Reading
    ''' the property gives current slot number (if wheel stationary) or -1 if wheel is moving. 
    ''' <para>Returning a position of -1 is <b>mandatory</b> while the filter wheel is in motion; valid slot numbers must not be reported back while the filter wheel is rotating past filter positions.</para>
    ''' <para><b>Note</b></para>
    ''' <para>Some filter wheels are built into the camera (one driver, two interfaces).  Some cameras may not actually rotate the wheel until the exposure is triggered.  In this case, the written value is available
    ''' immediately as the read value, and -1 is never produced.</para>
    ''' </remarks>
    Property Position() As Short
#End Region

End Interface