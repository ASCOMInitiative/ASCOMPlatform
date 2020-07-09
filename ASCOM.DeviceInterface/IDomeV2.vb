Imports System.Runtime.InteropServices
'-----------------------------------------------------------------------
' <summary>Defines the IDome Interface</summary>
'-----------------------------------------------------------------------
''' <summary>Defines the IDome Interface</summary>
''' <remarks>
'''     <para>
'''     This interface can be used to control most types of observatory structure, including a dome
'''     (with or without a controllable shutter), a clamshell and a roll off roof.
'''     </para>
'''     <para>
'''         The dome implementation should be self explanatory.
'''         A roll off roof or clamshell is implemented using the shutter control as the roof.
'''         The properties and methods should be implemented as follows:
'''     </para>
'''     <list type="bullet">
'''         <item>
'''             <see cref="IDomeV2.OpenShutter" /> and <see cref="IDomeV2.CloseShutter" /> open and close the roof
'''         </item>
'''         <item>
'''             <see cref="IDomeV2.CanFindHome" />, <see cref="IDomeV2.CanPark" />, <see cref="IDomeV2.CanSetAltitude" />,
'''             <see cref="IDomeV2.CanSetAzimuth" />, <see cref="IDomeV2.CanSetPark" />, <see cref="IDomeV2.CanSlave" /> and
'''             <see cref="IDomeV2.CanSyncAzimuth" /> all return <c>false</c>.
'''         </item>
'''         <item><see cref="IDomeV2.CanSetShutter" /> returns <c>true</c>.</item>
'''         <item><see cref="IDomeV2.ShutterStatus" /> should be implemented.</item>
'''         <item>
'''             <see cref="IDomeV2.AbortSlew" /> should stop the roof or shutter.
'''         </item>
'''         <item>
'''             <see cref="IDomeV2.FindHome" />, <see cref="IDomeV2.Park" />, <see cref="IDomeV2.SetPark" />,
'''             <see cref="IDomeV2.SlewToAltitude" />, <see cref="IDomeV2.SlewToAzimuth" /> and
'''             <see cref="IDomeV2.SyncToAzimuth" /> all throw <see cref="MethodNotImplementedException" />
'''         </item>
'''         <item>
'''             <see cref="IDomeV2.Altitude" /> and <see cref="IDomeV2.Azimuth" /> throw  <see cref="PropertyNotImplementedException" />
'''         </item>
'''     </list>
'''     <para>
'''         Some domes and possibly clamshells may be able to control the opening aperture and/or
'''         position. Observatories with this feature may wish to support the <see cref="IDomeV2.SlewToAltitude" /> method.
'''         In this case, the altitude specified is not the position of the shutter or roof, but the
'''         centre of an "aperture on the sky" that the observer wishes to view. The driver will have knowledge of the
'''         physical properties of the structure (possibly using data stored within the driver's settings) and iit is up to
'''         the driver to determine the best physical position of the roof apparatus to meet the observer's request.
'''     </para>
''' </remarks>

<Guid("88CFA00C-DDD3-4b42-A1F0-9387E6823832"), ComVisible(True), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)>
Public Interface IDomeV2 'CCDA0D85-474A-4775-8105-1D513ADC3896

#Region "Common Methods"
    'IAscomDriver Methods

    ''' <summary>
    '''     Set <see langword="true" /> to connect to the device hardware. Set <see langword="false" /> to
    '''     disconnect from the device hardware. You can also read the property to check whether it is
    '''     connected. This reports the current hardware state.
    ''' </summary>
    ''' <value><see langword="true" /> if connected to the hardware; otherwise, <see langword="false" />.</value>
    ''' <exception cref="DriverException">Must throw if the connection attempt was not successful</exception>
    ''' <remarks>
    '''     <para style="color: red;">
    '''         <b>Must be implemented.</b>
    '''     </para>
    '''     <para>
    '''         Do not use a <c>NotConnectedException</c> here, that exception is for use in other methods
    '''         that require a connection in order to succeed.
    '''     </para>
    '''     <para>
    '''         The Connected property sets and reports the state of the hardware connection. For a hub
    '''         this means that <see cref="Connected" /> will be <see langword="true" /> when the first
    '''         driver connects and will only return <see langword="false" /> when all drivers have
    '''         disconnected.  A second driver may find that Connected is already true and setting
    '''         <see cref="Connected" /> to <see langword="false" /> does not report
    '''         <see cref="Connected" /> as <see langword="false" />. This is not an error because the
    '''         physical state is that the hardware connection is still active.
    '''     </para>
    '''     <para>
    '''         The property is idempotent; writing to the property multiple times will not cause an
    '''         error.
    '''     </para>
    ''' </remarks>
    Property Connected() As Boolean

    ''' <summary>
    '''     Returns a description of the device, such as manufacturer and model number. Any ASCII
    '''     characters may be used.
    ''' </summary>
    ''' <value>The description.</value>
    ''' <exception cref="NotConnectedException">
    '''     thrown If the device is not connected and this information
    '''     is only available when connected.
    ''' </exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    '''     <para style="color: red;"><b>Must be implemented.</b></para>
    ''' </remarks>
    ReadOnly Property Description() As String

    ''' <summary>Descriptive and version information about this ASCOM driver.</summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    '''     <para style="color:red">
    '''         <b>Must be implemented</b>
    '''     </para>
    '''     <para>
    '''         This string may contain line endings and may be hundreds to thousands of characters long.
    '''         It is intended to display detailed information on the ASCOM driver, including version and
    '''         copyright data. See the <see cref="Description" /> property for information on the device
    '''         itself. To get the driver version in a parseable string, use the
    '''         <see cref="DriverVersion" /> property.
    '''     </para>
    ''' </remarks>
    ReadOnly Property DriverInfo() As String

    ''' <summary>A string containing only the major and minor version of the driver.</summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful.</exception>
    ''' <remarks>
    '''     <para style="color:red">
    '''         <b>Must be implemented.</b>
    '''     </para>
    '''     <para>
    '''         This must be in the form "n.n". The major and minor versions are positive decimal integers
    '''         and are separated by a period. Drivers should <b>not</b> format this as a real number with
    '''         a decimal point. The driver version should not to be confused with the
    '''         <see cref="InterfaceVersion" /> property, which is the version of this specification
    '''         supported by the driver.
    '''     </para>
    '''     <para>
    '''         Note: client applications should <b>NOT</b> treat this as a real number with a decimal
    '''         point, it is a string containing two integers with a separator character. Take care when
    '''         parsing this string, as a culture-sensitive parser may be confused by the period; Specify
    '''         <c>InvariantCulture</c> if necessary.
    '''     </para>
    ''' </remarks>
    ReadOnly Property DriverVersion() As String

    '[TPL] Removed requirement to throw DriverException if the call is not successful.
    'A property returning a constant should never throw an exception, and if it did,
    'the meaning would be ambiguous because throwing an exception means "treat as V1 legacy driver".
    ''' <summary>
    '''     The interface version number that this device supports. Should return 2 for this interface
    '''     version.
    ''' </summary>
    ''' <remarks>
    '''     <para style="color:red">
    '''         <b>Must be implemented</b>
    '''     </para>
    ''' <para>
    '''     Clients can detect legacy V1 drivers by trying to read ths property. If the driver raises an
    '''     error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of
    '''     1. In other words, a raised error or a return value of 1 indicates that the driver is a V1
    '''     driver.</para>
    ''' </remarks>
    ReadOnly Property InterfaceVersion() As Short

    ''' <summary>The short name of the driver, for display purposes</summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    '''     <para style="color:red">
    '''         <b>Must be implemented</b>
    '''     </para>
    ''' </remarks>
    ReadOnly Property Name() As String

    ''' <summary>
    '''     Launches a configuration dialog box for the driver.  The call will not return until the user
    '''     clicks OK or cancel manually.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    '''     <para style="color:red">
    '''         <b>Must be implemented</b>
    '''     </para>
    ''' </remarks>
    Sub SetupDialog()

    'DeviceControl Methods

    '[TPL]
    '[ToDo] The two-part naming scheme for actions is redundant and developers don't follow that guidance
    '[ToDo] in practice. Suggest removing this requirement. There is no need to include the device type in the action name
    '[ToDo] because we always know what type of device we are querying and thus no disambiguation is needed.
    ''' <summary>Invokes the specified device-specific custom action.</summary>
    ''' <param name="ActionName">
    '''     A well known name agreed by interested parties that represents the action
    '''     to be carried out.
    ''' </param>
    ''' <param name="ActionParameters">
    '''     List of required parameters or an
    '''     <see cref="String.Empty">Empty String</see> if none are required.
    ''' </param>
    ''' <returns>A string response. The meaning of returned strings is set by the driver author.</returns>
    ''' <exception cref="ASCOM.MethodNotImplementedException">
    '''     Thrown if no actions are supported.
    ''' </exception>
    ''' <exception cref="ASCOM.ActionNotImplementedException">
    '''     It is intended that the <see cref="SupportedActions"/> method will inform clients of driver capabilities, but
    '''     the driver must still throw an <see cref="ASCOM.ActionNotImplementedException"/> exception if it is asked to perform
    '''     an action that it does not support.
    ''' </exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful.</exception>
    ''' <example>
    '''     Suppose filter wheels start to appear with automatic wheel changers; new actions could be
    '''     <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
    '''     of wheel names and the second taking a wheel name and making the change, returning appropriate
    '''     values to indicate success or failure.
    ''' </example>
    ''' <remarks>
    '''     <para style="color:red">
    '''         <b>Can throw a not implemented exception</b>
    '''     </para>
    '''     This method is intended for use in all current and future device types and to avoid name
    '''     clashes, management of action names is important from day 1. A two-part naming convention will
    '''     be adopted - <b>DeviceType:UniqueActionName</b> where:
    '''     <list type="bullet">
    '''         <item>
    '''             <description>
    '''                 DeviceType is the same value as would be used by
    '''                 <see cref="ASCOM.Utilities.Chooser.DeviceType" /> e.g. Telescope, Camera, Switch
    '''                 etc.
    '''             </description>
    '''         </item>
    '''         <item>
    '''             <description>
    '''                 UniqueActionName is a single word, or multiple words joined by underscore
    '''                 characters, that sensibly describes the action to be performed.
    '''             </description>
    '''         </item>
    '''     </list>
    '''     <para>
    '''         It is recommended that UniqueActionNames should be a maximum of 16 characters for
    '''         legibility. Should the same function and UniqueActionName be supported by more than one
    '''         type of device, the reserved DeviceType of “General” will be used. Action names will be
    '''         case insensitive, so FilterWheel:SelectWheel, filterwheel:selectwheel and
    '''         FILTERWHEEL:SELECTWHEEL will all refer to the same action.
    '''     </para>
    '''     <para>
    '''         The names of all supported actions must be returned in the
    '''         <see cref="SupportedActions" /> property.
    '''     </para>
    ''' </remarks>
    Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String

    ''' <summary>Returns the list of custom action names supported by this driver.</summary>
    ''' <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    '''     <p style="color:red">
    '''         <b>Must be implemented</b>
    '''     </p>
    '''     This method must return an empty <see cref="ArrayList" /> if no actions are supported. Do not
    '''     throw a <see cref="ASCOM.PropertyNotImplementedException" />.
    '''     <para>
    '''         This is an aid to client authors and testers who would otherwise have to repeatedly poll
    '''         the driver to determine its capabilities. Returned action names may be in mixed case to
    '''         enhance presentation but  the <see cref="Action" /> method is case insensitive.
    '''     </para>
    '''     <para>
    '''         Collections have been used in the Telescope specification for a number of years and are
    '''         known to be compatible with COM. Within .NET, <see cref="ArrayList" /> is the correct
    '''         implementation to use as the .NET Generic methods are not compatible with COM.
    '''     </para>
    ''' </remarks>
    ReadOnly Property SupportedActions() As ArrayList

    ''' <summary>
    '''     Transmits an arbitrary string to the device and does not wait for a response. Optionally,
    '''     protocol framing characters may be added to the string before transmission.
    ''' </summary>
    ''' <param name="Command">The literal command string to be transmitted.</param>
    ''' <param name="Raw">
    '''     if set to <see langword="true" /> the string is transmitted 'as-is'. If set to
    '''     <see langword="false" /> then protocol framing characters may be added prior to transmission.
    ''' </param>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    '''     <p style="color:red">
    '''         <b>Can throw a not implemented exception</b>
    '''     </p>
    ''' </remarks>
    Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False)

    ''' <summary>
    '''     Transmits an arbitrary string to the device and waits for a boolean response. Optionally,
    '''     protocol framing characters may be added to the string before transmission.
    ''' </summary>
    ''' <param name="Command">The literal command string to be transmitted.</param>
    ''' <param name="Raw">
    '''     if set to <see langword="true" /> the string is transmitted 'as-is'. If set to
    '''     <see langword="false" /> then protocol framing characters may be added prior to transmission.
    ''' </param>
    ''' <returns>Returns the interpreted boolean response received from the device.</returns>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    '''     <p style="color:red">
    '''         <b>Can throw a not implemented exception</b>
    '''     </p>
    ''' </remarks>
    Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean

    ''' <summary>
    '''     Transmits an arbitrary string to the device and waits for a string response. Optionally,
    '''     protocol framing characters may be added to the string before transmission.
    ''' </summary>
    ''' <param name="Command">The literal command string to be transmitted.</param>
    ''' <param name="Raw">
    '''     if set to <see langword="true" /> the string is transmitted 'as-is'. If set to
    '''     <see langword="false" /> then protocol framing characters may be added prior to transmission.
    ''' </param>
    ''' <returns>Returns the string response received from the device.</returns>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="NotConnectedException">If the driver is not connected.</exception>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    '''     <p style="color:red">
    '''         <b>Can throw a not implemented exception</b>
    '''     </p>
    ''' </remarks>
    Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String

    ''' <summary>
    '''     Dispose the late-bound interface, if needed. Will release it via COM if it is a COM object,
    '''     else if native .NET will just dereference it for garbage collection.
    ''' </summary>
    ''' <remarks>It is best practice to always call this method when finished with a driver instance.</remarks>
    Sub Dispose()

#End Region

#Region "Device Methods"

    ''' <summary>Immediately stops any and all movement.</summary>
    ''' <remarks>
    '''     <p style="color:red">
    '''         <b>Must be implemented, must not throw a MethodNotImplementedException.</b>
    '''     </p>
    '''     Calling this method will immediately disable hardware slewing (<see cref="Slaved" /> will
    '''     become <see langword="false" />). Raises an error if a communications failure occurs, or if the
    '''     command is known to have failed.
    ''' </remarks>
    Sub AbortSlew()

    ''' <summary>
    '''     The altitude (degrees, horizon zero and increasing positive to 90 zenith) of the part of the
    '''     sky that the observer wishes to observe.
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
    ''' <remarks>
    '''     <para>
    '''         The specified altitude is not necessarily the intended position of the shutter or roof
    '''         mechanism. Rather, it is the position in the sky that the observer wishes to observe. The
    '''         driver will have enough information about the physical structure to determine the best
    '''         mechanical position that will satisfy the observers request. The driver must coordinate
    '''         multiple shutters, clamshell segments or roof mechanisms to provide the required aperture
    '''         on the sky.
    '''     </para>
    '''     <para>
    '''         Raises an error only if no altitude control. If actual dome altitude can not be read, then
    '''         reports back the last slew position.
    '''     </para>
    ''' </remarks>
    ReadOnly Property Altitude() As Double

    ''' <summary>
    '''     <para>Indicates whether the dome is in the home position. Raises an error if not supported.</para>
    '''     <para>
    '''         This is normally used following a <see cref="FindHome" /> operation. The value is reset
    '''         with any azimuth slew operation that moves the dome away from the home position.
    '''     </para>
    '''     <para>
    '''         <see cref="AtHome" /> may optionally also become true during normal slew operations, if the
    '''         dome passes through the home position and the dome controller hardware is capable of
    '''         detecting that; or at the end of a slew operation if the dome comes to rest at the home
    '''         position.
    '''     </para>
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
    ''' <remarks>
    '''     <para>
    '''         The home position is normally defined by a hardware sensor positioned around the dome
    '''         circumference and represents a fixed, known azimuth reference.
    '''     </para>
    '''     <para>
    '''         For some devices, the home position may represent a small range of azimuth values, rather
    '''         than a discrete value, since dome inertia, the resolution of the home position sensor
    '''         and/or the azimuth encoder may be insufficient to return the exact same azimuth value on
    '''         each occasion. Some dome controllers, on the other hand, will always force the azimuth
    '''         reading to a fixed value whenever the home position sensor is active. Because of these
    '''         potential differences in behaviour, applications should not rely on the reported azimuth
    '''         position being identical each time <see cref="AtHome" /> is set <see langword="true" />.
    '''     </para>
    ''' </remarks>
    ReadOnly Property AtHome() As Boolean

    '[ToDo] There is no Unpark method which is inconsistent with other interfaces. Consider adding to a future interface version.
    ''' <summary><see langword="true" /> if the dome is in the programmed park position.</summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
    ''' <remarks>
    '''     Set only following a <see cref="Park" /> operation and reset with any slew operation. Raises an
    '''     error if not supported.
    ''' </remarks>
    ReadOnly Property AtPark() As Boolean

    ''' <summary>
    '''     The dome azimuth (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270
    '''     West). North is true north and not magnetic north.
    ''' </summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
    ''' <remarks>
    '''     Raises an error only if no azimuth control. If actual dome azimuth can not be read, then
    '''     reports back the last slew position
    ''' </remarks>
    ReadOnly Property Azimuth() As Double

    ''' <summary><see langword="true" /> if driver can perform a search for home position.</summary>
    ''' <remarks>
    '''     <p style="color:red">
    '''         <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
    '''     </p>
    ''' </remarks>
    ReadOnly Property CanFindHome() As Boolean

    ''' <summary><see langword="true" /> if the driver is capable of parking the dome.</summary>
    ''' <remarks>
    '''     <p style="color:red">
    '''         <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
    '''     </p>
    ''' </remarks>
    ReadOnly Property CanPark() As Boolean

    ''' <summary><see langword="true" /> if driver is capable of setting dome altitude.</summary>
    ''' <remarks>
    '''     <p style="color:red">
    '''         <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
    '''     </p>
    ''' </remarks>
    ReadOnly Property CanSetAltitude() As Boolean

    ''' <summary>
    '''     <see langword="true" /> if driver is capable of rotating the dome (or controlling the roof
    '''     mechanism) in order to observe at a given azimuth.
    ''' </summary>
    ''' <remarks>
    '''     <p style="color:red">
    '''         <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
    '''     </p>
    '''     <para>
    '''         This property typically returns <see langword="true" /> for rotating structures and
    '''         <see langword="false" /> for non-rotating structures.
    '''     </para>
    ''' </remarks>
    ReadOnly Property CanSetAzimuth() As Boolean

    ''' <summary><see langword="true" /> if the driver can set the dome park position.</summary>
    ''' <remarks>
    '''     <p style="color:red">
    '''         <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
    '''     </p>
    ''' </remarks>
    ReadOnly Property CanSetPark() As Boolean

    ''' <summary>
    '''     <see langword="true" /> if the driver is capable of opening and closing the shutter or roof
    '''     mechanism.
    ''' </summary>
    ''' <remarks>
    '''     <p style="color:red">
    '''         <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
    '''     </p>
    ''' </remarks>
    ReadOnly Property CanSetShutter() As Boolean

    ''' <summary>True if the dome hardware supports slaving to a telescope.</summary>
    ''' <remarks>
    '''     <p style="color:red">
    '''         <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
    '''     </p>
    '''     <para>
    '''         See the notes for the <see cref="Slaved" /> property. This should only be
    '''         <see langword="true" /> if the dome hardware has its own built-in slaving mechanism. it is
    '''         not permitted for a dome driver to query a telescope driver directly.
    '''     </para>
    ''' </remarks>
    ''' <seealso cref="Slaved" />
    ReadOnly Property CanSlave() As Boolean

    ''' <summary>
    '''     <see langword="true" /> if the driver is capable of synchronizing the dome azimuth position
    '''     using the <see cref="SyncToAzimuth" /> method.
    ''' </summary>
    ''' <remarks>
    '''     <p style="color:red">
    '''         <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
    '''     </p>
    ''' </remarks>
    ReadOnly Property CanSyncAzimuth() As Boolean

    ''' <summary>Close the shutter or otherwise shield the telescope from the sky.</summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    Sub CloseShutter()

    ''' <summary>Start operation to search for the dome home position.</summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="DriverException">Must throw an exception if the operation cannot be started.</exception>
    ''' <exception cref="SlavedException">Thrown if slaving is enabled.</exception>
    ''' <remarks>
    '''     The method should not block and the homing operation should complete asynchronously. After the
    '''     home position is established, <see cref="Azimuth" /> is synchronized to the appropriate value
    '''     and the <see cref="AtHome" /> property becomes <see langword="true" />.
    ''' </remarks>
    Sub FindHome()

    ''' <summary>Open shutter or otherwise expose telescope to the sky.</summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="DriverException">Must throw an exception if the operation cannot be started.</exception>
    ''' <remarks>Raises an error if not supported or if a communications failure occurs.</remarks>
    Sub OpenShutter()

    '[TPL][ToDo] Discuss park operation, which has different semantics for domes compared to other interfaces.
    ' The purpose of Park and Unpark in other interfaces is:
    '   Park should "shut down the equipment and make it safe for storage".
    '   Unpark should "start up the equipment and make it ready for use".
    '
    ' A number of existing dome implementations automatically close the shutter when told
    ' to park, because (for example) this engages a battery charger to recharge the shutter battery. This is
    ' consistent with the normal semantics of "shutting down for storage" and in my opinion is already the
    ' _de facto_ implementation for domes.
    ' Suggest bringing this into line with the telescope definition of park and unpark in a future interface version
    ' such that it is clear what parking means and that unparking is necessary for a parked device.

    ''' <summary>Rotate dome in azimuth to park position.</summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <remarks>
    '''     After assuming programmed park position, sets <see cref="AtPark" /> flag. Raises an error if
    '''     <see cref="Slaved" /> is True, or if not supported, or if a communications failure has
    '''     occurred.
    ''' </remarks>
    Sub Park()

    '[TPL] removed reference to altitude as that is inconsistent with usage elsewhere.
    ''' <summary>Set the current azimuth position of dome to be the park position.</summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="DriverException">Must throw an exception if the operation cannot be completed.</exception>
    ''' <remarks>Raises an error if not supported or if a communications failure occurs.</remarks>
    Sub SetPark()

    ''' <summary>Gets the status of the dome shutter or roof structure.</summary>
    ''' <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
    ''' <remarks>
    '''     Raises an error only if no shutter control. If actual shutter status can not be read, then
    '''     reports back the last shutter state.
    ''' </remarks>
    ReadOnly Property ShutterStatus() As ShutterState

    ''' <summary><see langword="true"/> if the dome is slaved to the telescope in its hardware, else <see langword="false"/>.</summary>
    ''' <remarks>
    '''     <p style="color:red;margin-bottom:0">
    '''         <b>Slaved Read must be implemented and must not throw a PropertyNotImplementedException. </b>
    '''     </p>
    '''     <p style="color:red;margin-top:0">
    '''         <b>Slaved Write can throw a PropertyNotImplementedException.</b>
    '''     </p>
    '''     Set this property to <see langword="true"/> to enable dome-telescope hardware slaving, if supported (see
    '''     <see cref="CanSlave" />). Raises an exception on any attempt to set this property if hardware
    '''     slaving is not supported). Always returns <see langword="false"/> if hardware slaving is not supported.
    ''' </remarks>
    Property Slaved() As Boolean

    ''' <summary>
    '''     <see langword="true" /> if any part of the dome is currently moving, <see langword="false" />
    '''     if all dome components are stationary.
    ''' </summary>
    ''' <remarks>
    '''     <p style="color:red;margin-bottom:0">
    '''         <b>Slewing must be implemented and must not throw a PropertyNotImplementedException. </b>
    '''     </p>
    ''' </remarks>
    ''' <exception cref="DriverException">
    '''     Must raise an error if <see cref="Slaved" /> is <see langword="true" />, if not supported, if a
    '''     communications failure occurs, or if the dome can not reach the requested azimuth.
    ''' </exception>
    ReadOnly Property Slewing() As Boolean

    ''' <summary>Ensure that the requested viewing altitude is available for observing.</summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="InvalidValueException">If the supplied altitude is out of range.</exception>
    ''' <exception cref="DriverException">
    '''     Must raise an error if <see cref="Slaved" /> is <see langword="true" />, if not supported, if a
    '''     communications failure occurs, or if the dome can not reach indicated altitude.
    ''' </exception>
    ''' <param name="Altitude">
    '''     The desired viewing altitude (degrees, horizon zero and increasing positive to 90
    '''     degrees at the zenith)
    ''' </param>
    ''' <remarks>
    '''     The requested altitude should be interpreted by the driver as the position in the sky that the
    '''     observer wishes to observe. The driver has detailed knowledge of the physical structure and
    '''     must coordinate shutters, roofs or clamshell segments to open an aperture on the sky that
    '''     satisfies the observer's request.
    ''' </remarks>
    Sub SlewToAltitude(ByVal Altitude As Double)

    ''' <summary>
    '''     Ensure that the requested viewing azimuth is available for observing.
    ''' The method should not block and the slew operation should complete asynchronously.
    ''' </summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="InvalidValueException">
    '''     thrown if the requested azimuth is greater than or equal to 360 or less than 0.
    ''' </exception>
    ''' <exception cref="SlavedException">Thrown if <see cref="Slaved" /> is <see langword="true" /></exception>
    ''' <exception cref="DriverException">
    '''     Must raise an error if the operation cannot be started.
    ''' </exception>
    ''' <param name="Azimuth">
    '''     Desired viewing azimuth (degrees, North zero and increasing clockwise. i.e., 90 East,
    '''     180 South, 270 West)
    ''' </param>
    Sub SlewToAzimuth(ByVal Azimuth As Double)

    ''' <summary>Synchronize the current position of the dome to the given azimuth.</summary>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="InvalidValueException">
    '''     If the supplied azimuth is outside the range 0..360
    '''     degrees.
    ''' </exception>
    ''' <exception cref="DriverException">Must raise an error if the operation cannot be completed.</exception>
    ''' <param name="Azimuth">
    '''     Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East,
    '''     180 South, 270 West)
    ''' </param>
    Sub SyncToAzimuth(ByVal Azimuth As Double)
#End Region

End Interface