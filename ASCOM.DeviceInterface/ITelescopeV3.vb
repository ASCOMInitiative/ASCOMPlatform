Imports System.Runtime.InteropServices
Imports System.Collections

'-----------------------------------------------------------------------
' <summary>Defines the ITelescope Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Defines the ITelescope Interface
''' </summary>
<ComVisible(True), Guid("A007D146-AE3D-4754-98CA-199FEC03CF68"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface ITelescopeV3 ' EF0C67AD-A9D3-4f7b-A635-CD2095517633

#Region "Common Methods"
    'IAscomDriver Methods

    ''' <summary>
    ''' Set True to connect to the device hardware. Set False to disconnect from the device hardware.
    ''' You can also read the property to check whether it is connected. This reports the current hardware state.
    ''' </summary>
    ''' <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented</b></p>Do not use a NotConnectedException here, that exception is for use in other methods that require a connection in order to succeed.
    ''' <para>The Connected property sets and reports the state of connection to the device hardware.
    ''' For a hub this means that Connected will be true when the first driver connects and will only be set to false
    ''' when all drivers have disconnected.  A second driver may find that Connected is already true and
    ''' setting Connected to false does not report Connected as false.  This is not an error because the physical state is that the
    ''' hardware connection is still true.</para>
    ''' <para>Multiple calls setting Connected to true or false will not cause an error.</para>
    ''' </remarks>
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
    ''' The interface version number that this device supports. Should return 3 for this interface version.
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
    ''' Stops a slew in progress.
    ''' </summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <remarks>
    ''' Effective only after a call to <see cref="SlewToTargetAsync" />, <see cref="SlewToCoordinatesAsync" />, <see cref="SlewToAltAzAsync" />, or <see cref="MoveAxis" />.
    ''' Does nothing if no slew/motion is in progress. Tracking is returned to its pre-slew state. Raises an error if <see cref="AtPark" /> is true. 
    ''' </remarks>
    Sub AbortSlew()

    ''' <summary>
    ''' The alignment mode of the mount (Alt/Az, Polar, German Polar).
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
    ''' <remarks>
    ''' This is only available for telescope InterfaceVersions 2 and 3
    ''' </remarks>
    ReadOnly Property AlignmentMode() As AlignmentModes

    ''' <summary>
    ''' The Altitude above the local horizon of the telescope's current position (degrees, positive up)
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
    ReadOnly Property Altitude() As Double

    ''' <summary>
    ''' The area of the telescope's aperture, taking into account any obstructions (square meters)
    ''' </summary>
    ''' <remarks>
    ''' This is only available for telescope InterfaceVersions 2 and 3
    ''' </remarks>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
    ReadOnly Property ApertureArea() As Double

    ''' <summary>
    ''' The telescope's effective aperture diameter (meters)
    ''' </summary>
    ''' <remarks>
    ''' This is only available for telescope InterfaceVersions 2 and 3
    ''' </remarks>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
    ReadOnly Property ApertureDiameter() As Double

    ''' <summary>
    ''' True if the telescope is stopped in the Home position. Set only following a <see cref="FindHome"></see> operation,
    '''  and reset with any slew operation. This property must be False if the telescope does not support homing. 
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' This is only available for telescope InterfaceVersions 2 and 3
    ''' </remarks>
    ReadOnly Property AtHome() As Boolean

    ''' <summary>
    ''' True if the telescope has been put into the parked state by the seee <see cref="Park" /> method. Set False by calling the Unpark() method.
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' <para>AtPark is True when the telescope is in the parked state. This is achieved by calling the <see cref="Park" /> method. When AtPark is true, 
    ''' the telescope movement is stopped (or restricted to a small safe range of movement) and all calls that would cause telescope 
    ''' movement (e.g. slewing, changing Tracking state) must not do so, and must raise an error.</para>
    ''' <para>The telescope is taken out of parked state by calling the <see cref="UnPark" /> method. If the telescope cannot be parked, 
    ''' then AtPark must always return False.</para>
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    ReadOnly Property AtPark() As Boolean

    ''' <summary>
    ''' Determine the rates at which the telescope may be moved about the specified axis by the <see cref="MoveAxis" /> method.
    ''' </summary>
    ''' <param name="Axis">The axis about which rate information is desired (TelescopeAxes value)</param>
    ''' <returns>Collection of <see cref="IRate" /> rate objects</returns>
    ''' <exception cref="InvalidValueException">If an invalid Axis is specified.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a MethodNotImplementedException.</b></p>
    ''' See the description of <see cref="MoveAxis" /> for more information. This method must return an empty collection if <see cref="MoveAxis" /> is not supported. 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' <para>
    ''' Please note that the rate objects must contain absolute non-negative values only. Applications determine the direction by applying a
    ''' positive or negative sign to the rates provided. This obviates the need for the driver to to present a duplicate set of negative rates 
    ''' as well as the positive rates.</para>
    ''' </remarks>
    Function AxisRates(ByVal Axis As TelescopeAxes) As IAxisRates

    ''' <summary>
    ''' The azimuth at the local horizon of the telescope's current position (degrees, North-referenced, positive East/clockwise).
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
    ReadOnly Property Azimuth() As Double

    ''' <summary>
    ''' True if this telescope is capable of programmed finding its home position (<see cref="FindHome" /> method).
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' May raise an error if the telescope is not connected. 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    ReadOnly Property CanFindHome() As Boolean

    ''' <summary>
    ''' True if this telescope can move the requested axis
    ''' </summary>
    ''' <param name="Axis">Primary, Secondary or Tertiary axis</param>
    ''' <returns>Boolean indicating can or can not move the requested axis</returns>
    ''' <exception cref="InvalidValueException">If an invalid Axis is specified.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a MethodNotImplementedException.</b></p>
    ''' This is only available for telescope InterfaceVersions 2 and 3
    ''' </remarks>
    Function CanMoveAxis(ByVal Axis As TelescopeAxes) As Boolean

    ''' <summary>
    ''' True if this telescope is capable of programmed parking (<see cref="Park" />method)
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' May raise an error if the telescope is not connected. 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    ReadOnly Property CanPark() As Boolean

    ''' <summary>
    ''' True if this telescope is capable of software-pulsed guiding (via the <see cref="PulseGuide" /> method)
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' May raise an error if the telescope is not connected. 
    ''' </remarks>
    ReadOnly Property CanPulseGuide() As Boolean

    ''' <summary>
    ''' True if the <see cref="DeclinationRate" /> property can be changed to provide offset tracking in the declination axis.
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' May raise an error if the telescope is not connected. 
    ''' </remarks>
    ReadOnly Property CanSetDeclinationRate() As Boolean

    ''' <summary>
    ''' True if the guide rate properties used for <see cref="PulseGuide" /> can ba adjusted.
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' May raise an error if the telescope is not connected. 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    ReadOnly Property CanSetGuideRates() As Boolean

    ''' <summary>
    ''' True if this telescope is capable of programmed setting of its park position (<see cref="SetPark" /> method)
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' May raise an error if the telescope is not connected. 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    ReadOnly Property CanSetPark() As Boolean

    ''' <summary>
    ''' True if the <see cref="SideOfPier" /> property can be set, meaning that the mount can be forced to flip.
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' This will always return False for non-German-equatorial mounts that do not have to be flipped. 
    ''' May raise an error if the telescope is not connected. 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    ReadOnly Property CanSetPierSide() As Boolean

    ''' <summary>
    ''' True if the <see cref="RightAscensionRate" /> property can be changed to provide offset tracking in the right ascension axis.
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' May raise an error if the telescope is not connected. 
    ''' </remarks>
    ReadOnly Property CanSetRightAscensionRate() As Boolean

    ''' <summary>
    ''' True if the <see cref="Tracking" /> property can be changed, turning telescope sidereal tracking on and off.
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' May raise an error if the telescope is not connected. 
    ''' </remarks>
    ReadOnly Property CanSetTracking() As Boolean

    ''' <summary>
    ''' True if this telescope is capable of programmed slewing (synchronous or asynchronous) to equatorial coordinates
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' If this is true, then only the synchronous equatorial slewing methods are guaranteed to be supported.
    ''' See the <see cref="CanSlewAsync" /> property for the asynchronous slewing capability flag. 
    ''' May raise an error if the telescope is not connected. 
    ''' </remarks>
    ReadOnly Property CanSlew() As Boolean

    ''' <summary>
    ''' True if this telescope is capable of programmed slewing (synchronous or asynchronous) to local horizontal coordinates
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' If this is true, then only the synchronous local horizontal slewing methods are guaranteed to be supported.
    ''' See the <see cref="CanSlewAltAzAsync" /> property for the asynchronous slewing capability flag. 
    ''' May raise an error if the telescope is not connected. 
    ''' </remarks>
    ReadOnly Property CanSlewAltAz() As Boolean

    ''' <summary>
    ''' True if this telescope is capable of programmed asynchronous slewing to local horizontal coordinates
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' This indicates the the asynchronous local horizontal slewing methods are supported.
    ''' If this is True, then <see cref="CanSlewAltAz" /> will also be true. 
    ''' May raise an error if the telescope is not connected. 
    ''' </remarks>
    ReadOnly Property CanSlewAltAzAsync() As Boolean

    ''' <summary>
    ''' True if this telescope is capable of programmed asynchronous slewing to equatorial coordinates.
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' This indicates the the asynchronous equatorial slewing methods are supported.
    ''' If this is True, then <see cref="CanSlew" /> will also be true.
    ''' May raise an error if the telescope is not connected. 
    ''' </remarks>
    ReadOnly Property CanSlewAsync() As Boolean

    ''' <summary>
    ''' True if this telescope is capable of programmed synching to equatorial coordinates.
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' May raise an error if the telescope is not connected. 
    ''' </remarks>
    ReadOnly Property CanSync() As Boolean

    ''' <summary>
    ''' True if this telescope is capable of programmed synching to local horizontal coordinates
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' May raise an error if the telescope is not connected. 
    ''' </remarks>
    ReadOnly Property CanSyncAltAz() As Boolean

    ''' <summary>
    ''' True if this telescope is capable of programmed unparking (<see cref="Unpark" /> method).
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' If this is true, then <see cref="CanPark" /> will also be true. May raise an error if the telescope is not connected.
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    ReadOnly Property CanUnpark() As Boolean

    ''' <summary>
    ''' The declination (degrees) of the telescope's current equatorial coordinates, in the coordinate system given by the <see cref="EquatorialSystem" /> property.
    ''' Reading the property will raise an error if the value is unavailable. 
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' </remarks>
    ReadOnly Property Declination() As Double

    ''' <summary>
    ''' The declination tracking rate (arcseconds per second, default = 0.0)
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If DeclinationRate Write is not implemented.</exception>
    ''' <remarks>
    ''' <p style="color:red;margin-bottom:0"><b>DeclinationRate Read must be implemented and must not throw a PropertyNotImplementedException. </b></p>
    ''' <p style="color:red;margin-top:0"><b>DeclinationRate Write can throw a PropertyNotImplementedException.</b></p>
    ''' This property, together with <see cref="RightAscensionRate" />, provides support for "offset tracking".
    ''' Offset tracking is used primarily for tracking objects that move relatively slowly against the equatorial coordinate system.
    ''' It also may be used by a software guiding system that controls rates instead of using the <see cref="PulseGuide">PulseGuide</see> method. 
    ''' <para>
    ''' <b>NOTES:</b>
    ''' <list type="bullet">
    ''' <list></list>
    ''' <item><description>The property value represents an offset from zero motion.</description></item>
    ''' <item><description>If <see cref="CanSetDeclinationRate" /> is False, this property will always return 0.</description></item>
    ''' <item><description>To discover whether this feature is supported, test the <see cref="CanSetDeclinationRate" /> property.</description></item>
    ''' <item><description>The supported range of this property is telescope specific, however, if this feature is supported,
    ''' it can be expected that the range is sufficient to allow correction of guiding errors caused by moderate misalignment 
    ''' and periodic error.</description></item>
    ''' <item><description>If this property is non-zero when an equatorial slew is initiated, the telescope should continue to update the slew 
    ''' destination coordinates at the given offset rate.</description></item>
    ''' <item><description>This will allow precise slews to a fast-moving target with a slow-slewing telescope.</description></item>
    ''' <item><description>When the slew completes, the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> properties should reflect the final (adjusted) destination.</description></item>
    ''' </list>
    ''' </para>
    ''' <para>
    '''This is not a required feature of this specification, however it is desirable. 
    ''' </para>
    ''' </remarks>
    Property DeclinationRate() As Double

    ''' <summary>
    ''' Predict side of pier for German equatorial mounts
    ''' </summary>
    ''' <param name="RightAscension">The destination right ascension (hours).</param>
    ''' <param name="Declination">The destination declination (degrees, positive North).</param>
    ''' <returns>The side of the pier on which the telescope would be on if a slew to the given equatorial coordinates is performed at the current instant of time.</returns>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="InvalidValueException">If an invalid RightAscension or Declination is specified.</exception>
    ''' <remarks>
    ''' This is only available for telescope InterfaceVersions 2 and 3
    ''' </remarks>
    Function DestinationSideOfPier(ByVal RightAscension As Double, ByVal Declination As Double) As PierSide

    ''' <summary>
    ''' True if the telescope or driver applies atmospheric refraction to coordinates.
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">Either read or write or both properties can throw PropertyNotImplementedException if not implemented</exception>
    ''' <remarks>
    ''' If this property is True, the coordinates sent to, and retrieved from, the telescope are unrefracted. 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' <para>
    ''' <b>NOTES:</b>
    ''' <list type="bullet">
    ''' <item><description>If the driver does not know whether the attached telescope does its own refraction, and if the driver does not itself calculate 
    ''' refraction, this property (if implemented) must raise an error when read.</description></item>
    ''' <item><description>Writing to this property is optional. Often, a telescope (or its driver) calculates refraction using standard atmospheric parameters.</description></item>
    ''' <item><description>If the client wishes to calculate a more accurate refraction, then this property could be set to False and these 
    ''' client-refracted coordinates used.</description></item>
    ''' <item><description>If disabling the telescope or driver's refraction is not supported, the driver must raise an error when an attempt to set 
    ''' this property to False is made.</description></item> 
    ''' <item><description>Setting this property to True for a telescope or driver that does refraction, or to False for a telescope or driver that 
    ''' does not do refraction, shall not raise an error. It shall have no effect.</description></item> 
    ''' </list>
    ''' </para>
    ''' </remarks>
    Property DoesRefraction() As Boolean

    ''' <summary>
    ''' Equatorial coordinate system used by this telescope (e.g. Topocentric or J2000).
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' Most amateur telescopes use local topocentric coordinates. This coordinate system is simply the apparent position in the sky
    ''' (possibly uncorrected for atmospheric refraction) for "here and now", thus these are the coordinates that one would use with digital setting
    ''' circles and most amateur scopes. More sophisticated telescopes use one of the standard reference systems established by professional astronomers.
    ''' The most common is the Julian Epoch 2000 (J2000). These instruments apply corrections for precession,nutation, abberration, etc. to adjust the coordinates 
    ''' from the standard system to the pointing direction for the time and location of "here and now". 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    ReadOnly Property EquatorialSystem() As EquatorialCoordinateType

    ''' <summary>
    ''' Locates the telescope's "home" position (synchronous)
    ''' </summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanFindHome" /> is False</exception>
    ''' <remarks>
    ''' Returns only after the home position has been found.
    ''' At this point the <see cref="AtHome" /> property will be True.
    ''' Raises an error if there is a problem. 
    ''' Raises an error if AtPark is true. 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    Sub FindHome()

    ''' <summary>
    ''' The telescope's focal length, meters
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
    ''' <remarks>
    ''' This property may be used by clients to calculate telescope field of view and plate scale when combined with detector pixel size and geometry. 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    ReadOnly Property FocalLength() As Double

    ''' <summary>
    ''' The current Declination movement rate offset for telescope guiding (degrees/sec)
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
    ''' <exception cref="InvalidValueException">If an invalid guide rate is set.</exception>
    ''' <remarks> 
    ''' This is the rate for both hardware/relay guiding and the PulseGuide() method. 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' <para>
    ''' <b>NOTES:</b>
    ''' <list type="bullet">
    ''' <item><description>To discover whether this feature is supported, test the <see cref="CanSetGuideRates" /> property.</description></item> 
    ''' <item><description>The supported range of this property is telescope specific, however, if this feature is supported, it can be expected that the range is sufficient to
    ''' allow correction of guiding errors caused by moderate misalignment and periodic error.</description></item> 
    ''' <item><description>If a telescope does not support separate guiding rates in Right Ascension and Declination, then it is permissible for <see cref="GuideRateRightAscension" /> and GuideRateDeclination to be tied together.
    ''' In this case, changing one of the two properties will cause a change in the other.</description></item> 
    ''' <item><description>Mounts must start up with a known or default declination guide rate, and this property must return that known/default guide rate until changed.</description></item> 
    ''' </list>
    ''' </para>
    ''' </remarks>
    Property GuideRateDeclination() As Double

    ''' <summary>
    ''' The current Right Ascension movement rate offset for telescope guiding (degrees/sec)
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
    ''' <exception cref="InvalidValueException">If an invalid guide rate is set.</exception>
    ''' <remarks>
    ''' This is the rate for both hardware/relay guiding and the PulseGuide() method. 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' <para>
    ''' <b>NOTES:</b>
    ''' <list type="bullet">
    ''' <item><description>To discover whether this feature is supported, test the <see cref="CanSetGuideRates" /> property.</description></item>  
    ''' <item><description>The supported range of this property is telescope specific, however, if this feature is supported, it can be expected that the range is sufficient to allow correction of guiding errors caused by moderate
    ''' misalignment and periodic error.</description></item>  
    ''' <item><description>If a telescope does not support separate guiding rates in Right Ascension and Declination, then it is permissible for GuideRateRightAscension and <see cref="GuideRateDeclination" /> to be tied together. 
    ''' In this case, changing one of the two properties will cause a change in the other.</description></item>  
    '''<item><description> Mounts must start up with a known or default right ascension guide rate, and this property must return that known/default guide rate until changed.</description></item>  
    ''' </list>
    ''' </para>
    ''' </remarks>
    Property GuideRateRightAscension() As Double

    ''' <summary>
    ''' True if a <see cref="PulseGuide" /> command is in progress, False otherwise
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If <see cref="CanPulseGuide" /> is False</exception>
    ''' <remarks>
    ''' Raises an error if the value of the <see cref="CanPulseGuide" /> property is false (the driver does not support the <see cref="PulseGuide" /> method). 
    ''' </remarks>
    ReadOnly Property IsPulseGuiding() As Boolean

    ''' <summary>
    ''' Move the telescope in one axis at the given rate.
    ''' </summary>
    ''' <param name="Axis">The physical axis about which movement is desired</param>
    ''' <param name="Rate">The rate of motion (deg/sec) about the specified axis</param>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented.</exception>
    ''' <exception cref="InvalidValueException">If an invalid axis or rate is given.</exception>
    ''' <remarks>
    ''' This method supports control of the mount about its mechanical axes.
    ''' The telescope will start moving at the specified rate about the specified axis and continue indefinitely.
    ''' This method can be called for each axis separately, and have them all operate concurrently at separate rates of motion. 
    ''' Set the rate for an axis to zero to restore the motion about that axis to the rate set by the <see cref="Tracking"/> property.
    ''' Tracking motion (if enabled, see note below) is suspended during this mode of operation. 
    ''' <para>
    ''' Raises an error if <see cref="AtPark" /> is true. 
    ''' This must be implemented for the if the <see cref="CanMoveAxis" /> property returns True for the given axis.</para>
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' <para>
    ''' <b>NOTES:</b>
    ''' <list type="bullet">
    ''' <item><description>The movement rate must be within the value(s) obtained from a <see cref="IRate" /> object in the 
    ''' the <see cref="AxisRates" /> collection. This is a signed value with negative rates moving in the oposite direction to positive rates.</description></item>
    ''' <item><description>The values specified in <see cref="AxisRates" /> are absolute, unsigned values and apply to both directions, determined by the sign used in this command.</description></item>
    ''' <item><description>The value of <see cref="Slewing" /> must be True if the telescope is moving about any of its axes as a result of this method being called. 
    ''' This can be used to simulate a handbox by initiating motion with the MouseDown event and stopping the motion with the MouseUp event.</description></item>
    ''' <item><description>When the motion is stopped by setting the rate to zero the scope will be set to the previous <see cref="TrackingRate" /> or to no movement, depending on the state of the <see cref="Tracking" /> property.</description></item>
    ''' <item><description>It may be possible to implement satellite tracking by using the <see cref="MoveAxis" /> method to move the scope in the required manner to track a satellite.</description></item>
    ''' </list>
    ''' </para>
    ''' </remarks>
    Sub MoveAxis(ByVal Axis As TelescopeAxes, ByVal Rate As Double)

    ''' <summary>
    ''' Move the telescope to its park position, stop all motion (or restrict to a small safe range), and set <see cref="AtPark" /> to True.
    ''' </summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanPark" /> is False</exception>
    ''' <remarks>
    ''' Raises an error if there is a problem communicating with the telescope or if parking fails. Parking should put the telescope into a state where its pointing accuracy 
    ''' will not be lost if it is power-cycled (without moving it).Some telescopes must be power-cycled before unparking. Others may be unparked by simply calling the <see cref="UnPark" /> method.
    ''' Calling this with <see cref="AtPark" /> = True does nothing (harmless) 
    ''' </remarks>
    Sub Park()

    ''' <summary>
    ''' Moves the scope in the given direction for the given interval or time at 
    ''' the rate given by the corresponding guide rate property 
    ''' </summary>
    ''' <param name="Direction">The direction in which the guide-rate motion is to be made</param>
    ''' <param name="Duration">The duration of the guide-rate motion (milliseconds)</param>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanPulseGuide" /> is False</exception>
    ''' <exception cref="InvalidValueException">If an invalid direction or duration is given.</exception>
    ''' <remarks>
    ''' This method returns immediately if the hardware is capable of back-to-back moves,
    ''' i.e. dual-axis moves. For hardware not having the dual-axis capability,
    ''' the method returns only after the move has completed. 
    ''' <para>
    ''' <b>NOTES:</b>
    ''' <list type="bullet">
    ''' <item><description>Raises an error if <see cref="AtPark" /> is true.</description></item>
    ''' <item><description>The <see cref="IsPulseGuiding" /> property must be be True during pulse-guiding.</description></item>
    ''' <item><description>The rate of motion for movements about the right ascension axis is 
    ''' specified by the <see cref="GuideRateRightAscension" /> property. The rate of motion
    ''' for movements about the declination axis is specified by the 
    ''' <see cref="GuideRateDeclination" /> property. These two rates may be tied together
    ''' into a single rate, depending on the driver's implementation
    ''' and the capabilities of the telescope.</description></item>
    ''' </list>
    ''' </para>
    ''' </remarks>
    Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Integer)

    ''' <summary>
    ''' The right ascension (hours) of the telescope's current equatorial coordinates,
    ''' in the coordinate system given by the EquatorialSystem property
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' Reading the property will raise an error if the value is unavailable. 
    ''' </remarks>
    ReadOnly Property RightAscension() As Double

    ''' <summary>
    ''' The right ascension tracking rate offset from sidereal (seconds per sidereal second, default = 0.0)
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If RightAscensionRate Write is not implemented.</exception>
    ''' <exception cref="InvalidValueException">If an invalid drive rate is set.</exception>
    ''' <exception cref="InvalidValueException">If an invalid rate is set.</exception>
    ''' <remarks>
    ''' <p style="color:red;margin-bottom:0"><b>RightAscensionRate Read must be implemented and must not throw a PropertyNotImplementedException. </b></p>
    ''' <p style="color:red;margin-top:0"><b>RightAscensionRate Write can throw a PropertyNotImplementedException.</b></p>
    ''' This property, together with <see cref="DeclinationRate" />, provides support for "offset tracking". Offset tracking is used primarily for tracking objects that move relatively slowly
    ''' against the equatorial coordinate system. It also may be used by a software guiding system that controls rates instead of using the <see cref="PulseGuide">PulseGuide</see> method.
    ''' <para>
    ''' <b>NOTES:</b>
    ''' The property value represents an offset from the currently selected <see cref="TrackingRate" />. 
    ''' <list type="bullet">
    ''' <item><description>If this property is zero, tracking will be at the selected <see cref="TrackingRate" />.</description></item>
    ''' <item><description>If <see cref="CanSetRightAscensionRate" /> is False, this property must always return 0.</description></item> 
    ''' To discover whether this feature is supported, test the <see cref="CanSetRightAscensionRate" />property. 
    ''' <item><description>The property value is in in seconds of right ascension per sidereal second.</description></item> 
    ''' <item><description>To convert a given rate in (the more common) units of sidereal seconds per UTC (clock) second, multiply the value by 0.9972695677 
    ''' (the number of UTC seconds in a sidereal second) then set the property. Please note that these units were chosen for the Telescope V1 standard,
    ''' and in retrospect, this was an unfortunate choice. However, to maintain backwards compatibility, the units cannot be changed.
    ''' A simple multiplication is all that's needed, as noted. The supported range of this property is telescope specific, however,
    ''' if this feature is supported, it can be expected that the range is sufficient to allow correction of guiding errors
    ''' caused by moderate misalignment and periodic error. </description></item>
    ''' <item><description>If this property is non-zero when an equatorial slew is initiated, the telescope should continue to update the slew destination coordinates 
    ''' at the given offset rate. This will allow precise slews to a fast-moving target with a slow-slewing telescope. When the slew completes, 
    ''' the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> properties should reflect the final (adjusted) destination. This is not a required
    ''' feature of this specification, however it is desirable. </description></item>
    ''' <item><description>Use the <see cref="Tracking" /> property to enable and disable sidereal tracking (if supported). </description></item>
    ''' </list>
    ''' </para>
    ''' </remarks>
    Property RightAscensionRate() As Double

    ''' <summary>
    ''' Sets the telescope's park position to be its current position.
    ''' </summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanPark" /> is False</exception>
    Sub SetPark()

    ''' <summary>
    ''' Indicates the pointing state of the mount.
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
    ''' <exception cref="InvalidValueException">If an invalid side of pier is set.</exception>
    ''' <remarks>
    ''' <para>For historical reasons, this property's name does not reflect its true meaning. The name will not be changed (so as to preserve 
    ''' compatibility), but the meaning has since become clear. All conventional mounts have two pointing states for a given equatorial (sky) position. 
    ''' Mechanical limitations often make it impossible for the mount to position the optics at given HA/Dec in one of the two pointing 
    ''' states, but there are places where the same point can be reached sensibly in both pointing states (e.g. near the pole and 
    ''' close to the meridian). In order to understand these pointing states, consider the following (thanks to Patrick Wallace for this info):</para>
    ''' <para>All conventional telescope mounts have two axes nominally at right angles. For an equatorial, the longitude axis is mechanical 
    ''' hour angle and the latitude axis is mechanical declination. Sky coordinates and mechanical coordinates are two completely separate arenas. 
    ''' This becomes rather more obvious if your mount is an altaz, but it's still true for an equatorial. Both mount axes can in principle 
    ''' move over a range of 360 deg. This is distinct from sky HA/Dec, where Dec is limited to a 180 deg range (+90 to -90).  Apart from 
    ''' practical limitations, any point in the sky can be seen in two mechanical orientations. To get from one to the other the HA axis 
    ''' is moved 180 deg and the Dec axis is moved through the pole a distance twice the sky codeclination (90 - sky declination).</para>
    ''' <para>Mechanical zero HA/Dec will be one of the two ways of pointing at the intersection of the celestial equator and the local meridian. 
    ''' In order to support Dome slaving, where it is important to know which side of the pier the mount is actually on, ASCOM has adopted the 
    ''' convention that the Normal pointing state will be the state where a German Equatorial mount is on the East side of the pier, looking West, with the 
    ''' counterweights below the optical assembly and that <see cref="PierSide.pierEast"></see> will represent this pointing state.</para>
    ''' <para>Move your scope to this position and consider the two mechanical encoders zeroed. The two pointing states are, then:
    ''' <list type="table">
    ''' <item><term><b>Normal (<see cref="PierSide.pierEast"></see>)</b></term><description>Where the mechanical Dec is in the range -90 deg to +90 deg</description></item>
    ''' <item><term><b>Beyond the pole (<see cref="PierSide.pierWest"></see>)</b></term><description>Where the mechanical Dec is in the range -180 deg to -90 deg or +90 deg to +180 deg.</description></item>
    ''' </list>
    ''' </para>
    ''' <para>"Side of pier" is a "consequence" of the former definition, not something fundamental. 
    ''' Apart from mechanical interference, the telescope can move from one side of the pier to the other without the mechanical Dec 
    ''' having changed: you could track Polaris forever with the telescope moving from west of pier to east of pier or vice versa every 12h. 
    ''' Thus, "side of pier" is, in general, not a useful term (except perhaps in a loose, descriptive, explanatory sense). 
    ''' All this applies to a fork mount just as much as to a GEM, and it would be wrong to make the "beyond pole" state illegal for the 
    ''' former. Your mount may not be able to get there if your camera hits the fork, but it's possible on some mounts. Whether this is useful 
    ''' depends on whether you're in Hawaii or Finland.</para>
    ''' <para>To first order, the relationship between sky and mechanical HA/Dec is as follows:</para>
    ''' <para><b>Normal state:</b>
    ''' <list type="bullet">
    ''' <item><description>HA_sky  = HA_mech</description></item>
    ''' <item><description>Dec_sky = Dec_mech</description></item>
    ''' </list>
    ''' </para>
    ''' <para><b>Beyond the pole</b>
    ''' <list type="bullet">
    ''' <item><description>HA_sky  = HA_mech + 12h, expressed in range ± 12h</description></item>
    ''' <item><description>Dec_sky = 180d - Dec_mech, expressed in range ± 90d</description></item>
    ''' </list>
    ''' </para>
    ''' <para>Astronomy software often needs to know which which pointing state the mount is in. Examples include setting guiding polarities 
    ''' and calculating dome opening azimuth/altitude. The meaning of the SideOfPier property, then is:
    ''' <list type="table">
    ''' <item><term><b>pierEast</b></term><description>Normal pointing state</description></item>
    ''' <item><term><b>pierWest</b></term><description>Beyond the pole pointing state</description></item>
    ''' </list>
    ''' </para>
    ''' <para>If the mount hardware reports neither the true pointing state (or equivalent) nor the mechanical declination axis position 
    ''' (which varies from -180 to +180), a driver cannot calculate the pointing state, and *must not* implement SideOfPier.
    ''' If the mount hardware reports only the mechanical declination axis position (-180 to +180) then a driver can calculate SideOfPier as follows:
    ''' <list type="bullet">
    ''' <item><description>pierEast = abs(mechanical dec) &lt;= 90 deg</description></item>
    ''' <item><description>pierWest = abs(mechanical Dec) &gt; 90 deg</description></item>
    ''' </list>
    ''' </para>
    ''' <para>It is allowed (though not required) that this property may be written to force the mount to flip. Doing so, however, may change 
    ''' the right ascension of the telescope. During flipping, Telescope.Slewing must return True.</para>
    ''' <para>This property is only available in telescope InterfaceVersions 2 and 3.</para>
    ''' <para><b>Pointing State and Side of Pier - Help for Driver Developers</b></para>
    ''' <para>A further document, "Pointing State and Side of Pier", is installed in the Developer Documentation folder by the ASCOM Developer 
    ''' Components installer. This further explains the pointing state concept and includes diagrams illustrating how it relates 
    ''' to physical side of pier for German equatorial telescopes. It also includes details of the tests performed by Conform to determine whether 
    ''' the driver correctly reports the pointing state as defined above.</para>
    ''' </remarks>
    Property SideOfPier() As PierSide

    ''' <summary>
    ''' The local apparent sidereal time from the telescope's internal clock (hours, sidereal)
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' It is required for a driver to calculate this from the system clock if the telescope 
    ''' has no accessible source of sidereal time. Local Apparent Sidereal Time is the sidereal 
    ''' time used for pointing telescopes, and thus must be calculated from the Greenwich Mean
    ''' Sidereal time, longitude, nutation in longitude and true ecliptic obliquity. 
    ''' </remarks>
    ReadOnly Property SiderealTime() As Double

    ''' <summary>
    ''' The elevation above mean sea level (meters) of the site at which the telescope is located
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
    ''' <exception cref="InvalidValueException">If an invalid elevation is set.</exception>
    ''' <exception cref="InvalidOperationException">If the application must set the elevation before reading it, but has not.</exception>
    ''' <remarks>
    ''' Setting this property will raise an error if the given value is outside the range -300 through +10000 metres.
    ''' Reading the property will raise an error if the value has never been set or is otherwise unavailable. 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    Property SiteElevation() As Double

    ''' <summary>
    ''' The geodetic(map) latitude (degrees, positive North, WGS84) of the site at which the telescope is located.
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
    ''' <exception cref="InvalidValueException">If an invalid latitude is set.</exception>
    ''' <exception cref="InvalidOperationException">If the application must set the latitude before reading it, but has not.</exception>
    ''' <remarks>
    ''' Setting this property will raise an error if the given value is outside the range -90 to +90 degrees.
    ''' Reading the property will raise an error if the value has never been set or is otherwise unavailable. 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    Property SiteLatitude() As Double

    ''' <summary>
    ''' The longitude (degrees, positive East, WGS84) of the site at which the telescope is located.
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
    ''' <exception cref="InvalidValueException">If an invalid longitude is set.</exception>
    ''' <exception cref="InvalidOperationException">If the application must set the longitude before reading it, but has not.</exception>
    ''' <remarks>
    ''' Setting this property will raise an error if the given value is outside the range -180 to +180 degrees.
    ''' Reading the property will raise an error if the value has never been set or is otherwise unavailable.
    ''' Note that West is negative! 
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    Property SiteLongitude() As Double

    ''' <summary>
    ''' True if telescope is currently moving in response to one of the
    ''' Slew methods or the <see cref="MoveAxis" /> method, False at all other times.
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
    ''' <remarks>
    ''' Reading the property will raise an error if the value is unavailable. If the telescope is not capable of asynchronous slewing, this property will always be False. 
    ''' The definition of "slewing" excludes motion caused by sidereal tracking, <see cref="PulseGuide">PulseGuide</see>, <see cref="RightAscensionRate" />, and <see cref="DeclinationRate" />.
    ''' It reflects only motion caused by one of the Slew commands, flipping caused by changing the <see cref="SideOfPier" /> property, or <see cref="MoveAxis" />. 
    ''' </remarks>
    ReadOnly Property Slewing() As Boolean

    ''' <summary>
    ''' Specifies a post-slew settling time (sec.).
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
    ''' <exception cref="InvalidValueException">If an invalid settle time is set.</exception>
    ''' <remarks>
    ''' Adds additional time to slew operations. Slewing methods will not return, 
    ''' and the <see cref="Slewing" /> property will not become False, until the slew completes and the SlewSettleTime has elapsed.
    ''' This feature (if supported) may be used with mounts that require extra settling time after a slew. 
    ''' </remarks>
    Property SlewSettleTime() As Short

    ''' <summary>
    ''' Move the telescope to the given local horizontal coordinates, return when slew is complete
    ''' </summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSlewAltAz" /> is False</exception>
    ''' <exception cref="InvalidValueException">If an invalid azimuth or elevation is given.</exception>
    ''' <remarks>
    ''' This Method must be implemented if <see cref="CanSlewAltAz" /> returns True. Raises an error if the slew fails. The slew may fail if the target coordinates are beyond limits imposed within the driver component.
    ''' Such limits include mechanical constraints imposed by the mount or attached instruments, building or dome enclosure restrictions, etc.
    ''' <para>The <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> properties are not changed by this method. 
    ''' Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is True. This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    ''' <param name="Azimuth">Target azimuth (degrees, North-referenced, positive East/clockwise).</param>
    ''' <param name="Altitude">Target altitude (degrees, positive up)</param>
    Sub SlewToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double)

    ''' <summary>
    ''' This Method must be implemented if <see cref="CanSlewAltAzAsync" /> returns True.
    ''' </summary>
    ''' <param name="Azimuth">Azimuth to which to move</param>
    ''' <param name="Altitude">Altitude to which to move to</param>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSlewAltAzAsync" /> is False</exception>
    ''' <exception cref="InvalidValueException">If an invalid azimuth or elevation is given.</exception>
    ''' <remarks>
    ''' This method should only be implemented if the properties <see cref="Altitude" />, <see cref="Azimuth" />,
    ''' <see cref="RightAscension" />, <see cref="Declination" /> and <see cref="Slewing" /> can be read while the scope is slewing. Raises an error if starting the slew fails. Returns immediately after starting the slew.
    ''' The client may monitor the progress of the slew by reading the <see cref="Azimuth" />, <see cref="Altitude" />, and <see cref="Slewing" /> properties during the slew. When the slew completes, Slewing becomes False. 
    ''' The slew may fail if the target coordinates are beyond limits imposed within the driver component. Such limits include mechanical constraints imposed by the mount or attached instruments, building or dome enclosure restrictions, etc. 
    ''' The <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> properties are not changed by this method. 
    ''' <para>Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is True.</para>
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    Sub SlewToAltAzAsync(ByVal Azimuth As Double, ByVal Altitude As Double)

    ''' <summary>
    ''' Move the telescope to the given equatorial coordinates, return when slew is complete
    ''' </summary>
    ''' <exception cref="InvalidValueException">If an invalid right ascension or declination is given.</exception>
    ''' <param name="RightAscension">The destination right ascension (hours). Copied to <see cref="TargetRightAscension" />.</param>
    ''' <param name="Declination">The destination declination (degrees, positive North). Copied to <see cref="TargetDeclination" />.</param>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSlew" /> is False</exception>
    ''' <remarks>
    ''' This Method must be implemented if <see cref="CanSlew" /> returns True. Raises an error if the slew fails. 
    ''' The slew may fail if the target coordinates are beyond limits imposed within the driver component.
    ''' Such limits include mechanical constraints imposed by the mount or attached instruments,
    ''' building or dome enclosure restrictions, etc. The target coordinates are copied to
    ''' <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> whether or not the slew succeeds. 
    ''' <para>Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is False.</para>
    ''' </remarks>
    Sub SlewToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double)

    ''' <summary>
    ''' Move the telescope to the given equatorial coordinates, return immediately after starting the slew.
    ''' </summary>
    ''' <param name="RightAscension">The destination right ascension (hours). Copied to <see cref="TargetRightAscension" />.</param>
    ''' <param name="Declination">The destination declination (degrees, positive North). Copied to <see cref="TargetDeclination" />.</param>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSlewAsync" /> is False</exception>
    ''' <exception cref="InvalidValueException">If an invalid right ascension or declination is given.</exception>
    ''' <remarks>
    ''' This method must be implemented if <see cref="CanSlewAsync" /> returns True. Raises an error if starting the slew failed. 
    ''' Returns immediately after starting the slew. The client may monitor the progress of the slew by reading
    ''' the <see cref="RightAscension" />, <see cref="Declination" />, and <see cref="Slewing" /> properties during the slew. When the slew completes,
    ''' <see cref="Slewing" /> becomes False. The slew may fail to start if the target coordinates are beyond limits
    ''' imposed within the driver component. Such limits include mechanical constraints imposed
    ''' by the mount or attached instruments, building or dome enclosure restrictions, etc. 
    ''' <para>The target coordinates are copied to <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" />
    ''' whether or not the slew succeeds. 
    ''' Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is False.</para>
    ''' </remarks>
    Sub SlewToCoordinatesAsync(ByVal RightAscension As Double, ByVal Declination As Double)

    ''' <summary>
    ''' Move the telescope to the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> coordinates, return when slew complete.
    ''' </summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSlew" /> is False</exception>
    ''' <remarks>
    ''' This Method must be implemented if <see cref="CanSlew" /> returns True. Raises an error if the slew fails. 
    ''' The slew may fail if the target coordinates are beyond limits imposed within the driver component.
    ''' Such limits include mechanical constraints imposed by the mount or attached
    ''' instruments, building or dome enclosure restrictions, etc. 
    ''' Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is False. 
    ''' </remarks>
    Sub SlewToTarget()

    ''' <summary>
    ''' Move the telescope to the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" />  coordinates,
    ''' returns immediately after starting the slew.
    ''' </summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSlewAsync" /> is False</exception>
    ''' <remarks>
    ''' This Method must be implemented if  <see cref="CanSlewAsync" /> returns True.
    ''' Raises an error if starting the slew failed. Returns immediately after starting the slew. The client may monitor the progress of the slew by reading the RightAscension, Declination,
    ''' and Slewing properties during the slew. When the slew completes,  <see cref="Slewing" /> becomes False. The slew may fail to start if the target coordinates are beyond limits imposed within 
    ''' the driver component. Such limits include mechanical constraints imposed by the mount or attached instruments, building or dome enclosure restrictions, etc. 
    ''' Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is False. 
    ''' </remarks>
    Sub SlewToTargetAsync()

    ''' <summary>
    ''' Matches the scope's local horizontal coordinates to the given local horizontal coordinates.
    ''' </summary>
    ''' <param name="Azimuth">Target azimuth (degrees, North-referenced, positive East/clockwise)</param>
    ''' <param name="Altitude">Target altitude (degrees, positive up)</param>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSyncAltAz" /> is False</exception>
    ''' <exception cref="InvalidValueException">If an invalid azimuth or altitude is given.</exception>
    ''' <remarks>
    ''' This must be implemented if the <see cref="CanSyncAltAz" /> property is True. Raises an error if matching fails. 
    ''' <para>Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is True.</para>
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    Sub SyncToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double)

    ''' <summary>
    ''' Matches the scope's equatorial coordinates to the given equatorial coordinates.
    ''' </summary>
    ''' <param name="RightAscension">The corrected right ascension (hours). Copied to the <see cref="TargetRightAscension" /> property.</param>
    ''' <param name="Declination">The corrected declination (degrees, positive North). Copied to the <see cref="TargetDeclination" /> property.</param>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSync" /> is False</exception>
    ''' <exception cref="InvalidValueException">If an invalid right ascension or declination is given.</exception>
    ''' <remarks>
    ''' This must be implemented if the <see cref="CanSync" /> property is True. Raises an error if matching fails. 
    ''' Raises an error if <see cref="AtPark" /> AtPark is True, or if <see cref="Tracking" /> is False. 
    ''' The way that Sync is implemented is mount dependent and it should only be relied on to improve pointing for positions close to
    ''' the position at which the sync is done.
    ''' </remarks>
    Sub SyncToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double)

    ''' <summary>
    ''' Matches the scope's equatorial coordinates to the given equatorial coordinates.
    ''' </summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSync" /> is False</exception>
    ''' <remarks>
    ''' This must be implemented if the <see cref="CanSync" /> property is True. Raises an error if matching fails. 
    ''' Raises an error if <see cref="AtPark" /> AtPark is True, or if <see cref="Tracking" /> is False. 
    ''' The way that Sync is implemented is mount dependent and it should only be relied on to improve pointing for positions close to
    ''' the position at which the sync is done.
    ''' </remarks>
    Sub SyncToTarget()

    ''' <summary>
    ''' The declination (degrees, positive North) for the target of an equatorial slew or sync operation
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
    ''' <exception cref="InvalidValueException">If an invalid declination is set.</exception>
    ''' <exception cref="InvalidOperationException">If the property is read before being set for the first time.</exception>
    ''' <remarks>
    ''' Setting this property will raise an error if the given value is outside the range -90 to +90 degrees. Reading the property will raise an error if the value has never been set or is otherwise unavailable. 
    ''' </remarks>
    Property TargetDeclination() As Double

    ''' <summary>
    ''' The right ascension (hours) for the target of an equatorial slew or sync operation
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
    ''' <exception cref="InvalidValueException">If an invalid right ascension is set.</exception>
    ''' <exception cref="InvalidOperationException">If the property is read before being set for the first time.</exception>
    ''' <remarks>
    ''' Setting this property will raise an error if the given value is outside the range 0 to 24 hours. Reading the property will raise an error if the value has never been set or is otherwise unavailable. 
    ''' </remarks>
    Property TargetRightAscension() As Double

    ''' <summary>
    ''' The state of the telescope's sidereal tracking drive.
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If Tracking Write is not implemented.</exception>
    ''' <remarks>
    ''' <p style="color:red;margin-bottom:0"><b>Tracking Read must be implemented and must not throw a PropertyNotImplementedException. </b></p>
    ''' <p style="color:red;margin-top:0"><b>Tracking Write can throw a PropertyNotImplementedException.</b></p>
    ''' Changing the value of this property will turn the sidereal drive on and off.
    ''' However, some telescopes may not support changing the value of this property
    ''' and thus may not support turning tracking on and off.
    ''' See the <see cref="CanSetTracking" /> property. 
    ''' </remarks>
    Property Tracking() As Boolean

    ''' <summary>
    ''' The current tracking rate of the telescope's sidereal drive
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If TrackingRate Write is not implemented.</exception>
    ''' <exception cref="InvalidValueException">If an invalid drive rate is set.</exception>
    ''' <remarks>
    ''' <p style="color:red;margin-bottom:0"><b>TrackingRate Read must be implemented and must not throw a PropertyNotImplementedException. </b></p>
    ''' <p style="color:red;margin-top:0"><b>TrackingRate Write can throw a PropertyNotImplementedException.</b></p>
    ''' Supported rates (one of the <see cref="DriveRates" />  values) are contained within the <see cref="TrackingRates" /> collection.
    ''' Values assigned to TrackingRate must be one of these supported rates. If an unsupported value is assigned to this property, it will raise an error. 
    ''' The currently selected tracking rate can be further adjusted via the <see cref="RightAscensionRate" /> and <see cref="DeclinationRate" /> properties. These rate offsets are applied to the currently 
    ''' selected tracking rate. Mounts must start up with a known or default tracking rate, and this property must return that known/default tracking rate until changed.
    ''' <para>If the mount's current tracking rate cannot be determined (for example, it is a write-only property of the mount's protocol), 
    ''' it is permitted for the driver to force and report a default rate on connect. In this case, the preferred default is Sidereal rate.</para>
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    Property TrackingRate() As DriveRates

    ''' <summary>
    ''' Returns a collection of supported <see cref="DriveRates" /> values that describe the permissible
    ''' values of the <see cref="TrackingRate" /> property for this telescope type.
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented and must not throw a PropertyNotImplementedException.</b></p>
    ''' At a minimum, this must contain an item for <see cref="DriveRates.driveSidereal" />.
    ''' <para>This is only available for telescope InterfaceVersions 2 and 3</para>
    ''' </remarks>
    ReadOnly Property TrackingRates() As ITrackingRates

    ''' <summary>
    ''' Takes telescope out of the Parked state.
    ''' </summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanUnpark" /> is False</exception>
    ''' <remarks>
    ''' The state of <see cref="Tracking" /> after unparking is undetermined. Valid only after <see cref="Park" />. Applications must check and change Tracking as needed after unparking. 
    ''' Raises an error if unparking fails. Calling this with <see cref="AtPark" /> = False does nothing (harmless) 
    ''' </remarks>
    Sub Unpark()

    ''' <summary>
    ''' The UTC date/time of the telescope's internal clock
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If UTCDate Write is not implemented.</exception>
    ''' <exception cref="InvalidValueException">If an invalid <see cref="DateTime" /> is set.</exception>
    ''' <exception cref="InvalidOperationException">When UTCDate is read and the mount cannot provide this property itslef and a value has not yet be established by writing to the property.</exception>
    ''' <remarks>
    ''' <p style="color:red;margin-bottom:0"><b>UTCDate Read must be implemented and must not throw a PropertyNotImplementedException. </b></p>
    ''' <p style="color:red;margin-top:0"><b>UTCDate Write can throw a PropertyNotImplementedException.</b></p>
    ''' The driver must calculate this from the system clock if the telescope has no accessible source of UTC time. In this case, the property must not be writeable (this would change the system clock!) and will instead raise an error.
    ''' However, it is permitted to change the telescope's internal UTC clock if it is being used for this property. This allows clients to adjust the telescope's UTC clock as needed for accuracy. Reading the property
    ''' will raise an error if the value has never been set or is otherwise unavailable. 
    ''' </remarks>
    Property UTCDate() As DateTime
#End Region

End Interface