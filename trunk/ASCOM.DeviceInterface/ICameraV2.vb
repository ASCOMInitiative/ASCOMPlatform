Imports System.Runtime.InteropServices
Imports ASCOM

'-----------------------------------------------------------------------
' <summary>Defines the ICamera Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Defines the ICamera Interface
''' </summary>
''' <remarks>The camera state diagram is shown here: <img src="../media/Camerav2 State Diagram.png"/></remarks>
<Guid("972CEBC6-0EBE-4efc-99DD-CC5293FDE77B"), ComVisible(True),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)>
Public Interface ICameraV2
    'D95FBC6E-0705-458B-84C0-57E3295DBCCE
    'Inherits IAscomDriver
    'Inherits IDeviceControl

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
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p> </remarks>
    ReadOnly Property Description() As String

    ''' <summary>
    ''' Descriptive and version information about this ASCOM driver.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p> This string may contain line endings and may be hundreds to thousands of characters long.
    ''' It is intended to display detailed information on the ASCOM driver, including version and copyright data.
    ''' See the <see cref="Description" /> property for information on the device itself.
    ''' To get the driver version in a parseable string, use the <see cref="DriverVersion" /> property.
    ''' </remarks>
    ReadOnly Property DriverInfo() As String

    ''' <summary>
    ''' A string containing only the major and minor version of the driver.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p> This must be in the form "n.n".
    ''' It should not to be confused with the <see cref="InterfaceVersion" /> property, which is the version of this specification supported by the 
    ''' driver.
    ''' </remarks>
    ReadOnly Property DriverVersion() As String

    ''' <summary>
    ''' The interface version number that this device supports. Should return 2 for this interface version.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p> Clients can detect legacy V1 drivers by trying to read ths property.
    ''' If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1. 
    ''' In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver.
    ''' </remarks>
    ReadOnly Property InterfaceVersion() As Short

    ''' <summary>
    ''' The short name of the driver, for display purposes
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p> </remarks>
    ReadOnly Property Name() As String

    ''' <summary>
    ''' Launches a configuration dialog box for the driver.  The call will not return
    ''' until the user clicks OK or cancel manually.
    ''' </summary>
    ''' <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw a MethodNotImplementedException.</b></p> </remarks>
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
    ''' <remarks><p style="color:red"><b>May throw a MethodNotImplementedException if the device does not support any actions.</b></p> 
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
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p> This method must return an empty arraylist if no actions are supported.
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
    ''' <remarks><p style="color:red"><b>May throw a MethodNotImplementedException.</b></p> </remarks>
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
    ''' <remarks><p style="color:red"><b>May throw a MethodNotImplementedException.</b></p> </remarks>
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
    ''' <remarks><p style="color:red"><b>May throw a MethodNotImplementedException.</b></p> </remarks>
    Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String

    ''' <summary>
    ''' Dispose the late-bound interface, if needed. Will release it via COM
    ''' if it is a COM object, else if native .NET will just dereference it
    ''' for GC.
    ''' </summary>
    Sub Dispose()

#End Region

#Region "Camera V1 methods"

    ''' <summary>
    ''' Aborts the current exposure, if any, and returns the camera to Idle state.
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a MethodNotImplementedException.</b></p>
    ''' <para><b>NOTES:</b>
    ''' <list type="bullet">
    ''' <item><description>Must throw exception if camera is not idle and abort is unsuccessful (or not possible, e.g. during download).</description></item>
    ''' <item><description>Must throw exception if hardware or communications error occurs.</description></item>
    ''' <item><description>Must NOT throw an exception if the camera is already idle.</description></item>
    ''' </list> </para>
    ''' </remarks>
    ''' <exception cref="T:ASCOM.NotConnectedException">Thrown if the driver is not connected.</exception>
    ''' <exception cref="T:ASCOM.InvalidOperationException">Thrown if abort is not currently possible (e.g. during download).</exception>
    ''' <exception cref="T:ASCOM.DriverException">Thrown if a communications error occurs, or if the abort fails.</exception>
    Sub AbortExposure()

    ''' <summary>
    ''' Sets the binning factor for the X axis, also returns the current value.  
    ''' </summary>
    ''' <remarks>
    ''' Should default to 1 when the camera connection is established.  Note:  driver does not check
    ''' for compatible subframe values when this value is set; rather they are checked upon <see cref="StartExposure">StartExposure</see>.
    ''' </remarks>
    ''' <value>The X binning value</value>
    ''' <exception cref="T:ASCOM.InvalidValueException">Must throw an exception for illegal binning values</exception>
    Property BinX() As Short

    ''' <summary>
    ''' Sets the binning factor for the Y axis, also returns the current value. 
    ''' </summary>
    ''' <remarks>
    ''' Should default to 1 when the camera connection is established.  Note:  driver does not check
    ''' for compatible subframe values when this value is set; rather they are checked upon <see cref="StartExposure">StartExposure</see>.
    ''' </remarks>
    ''' <value>The Y binning value.</value>
    ''' <exception cref="InvalidValueException">Must throw an exception for illegal binning values</exception>
    Property BinY() As Short

    ''' <summary>
    ''' Returns the current camera operational state
    ''' </summary>
    ''' <remarks>
    ''' Returns one of the following status information:
    ''' <list type="bullet">
    ''' <listheader><description>Value  State           Meaning</description></listheader>
    ''' <item><description>0      CameraIdle      At idle state, available to start exposure</description></item>
    ''' <item><description>1      CameraWaiting   Exposure started but waiting (for shutter, trigger, filter wheel, etc.)</description></item>
    ''' <item><description>2      CameraExposing  Exposure currently in progress</description></item>
    ''' <item><description>3      CameraReading   CCD array is being read out (digitized)</description></item>
    ''' <item><description>4      CameraDownload  Downloading data to PC</description></item>
    ''' <item><description>5      CameraError     Camera error condition serious enough to prevent further operations (connection fail, etc.).</description></item>
    ''' </list>
    ''' </remarks>
    ''' <value>The state of the camera.</value>
    ''' <exception cref="NotConnectedException">Must return an exception if the camera status is unavailable.</exception>
    ReadOnly Property CameraState() As CameraStates

    ''' <summary>
    ''' Returns the width of the CCD camera chip in unbinned pixels.
    ''' </summary>
    ''' <value>The size of the camera X.</value>
    ''' <exception cref="NotConnectedException">Must throw exception if the value is not known</exception>
    ReadOnly Property CameraXSize() As Integer

    ''' <summary>
    ''' Returns the height of the CCD camera chip in unbinned pixels.
    ''' </summary>
    ''' <value>The size of the camera Y.</value>
    ''' <exception cref="NotConnectedException">Must throw exception if the value is not known</exception>
    ReadOnly Property CameraYSize() As Integer

    ''' <summary>
    ''' Returns <c>true</c> if the camera can abort exposures; <c>false</c> if not.
    ''' </summary>
    ''' <value>
    ''' 	<c>true</c> if this instance can abort exposure; otherwise, <c>false</c>.
    ''' </value>
    ''' <exception cref="NotConnectedException">Thrown if the driver is not connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' </remarks>
    ReadOnly Property CanAbortExposure() As Boolean

    ''' <summary>
    ''' Returns a flag showing whether this camera supports asymmetric binning
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' <para>If <c>true</c>, the camera can have different binning on the X and Y axes, as
    ''' determined by <see cref="BinX" /> and <see cref="BinY" />. If <c>false</c>, the binning must be equal on the X and Y axes.</para>
    ''' </remarks>
    ''' <value>
    ''' 	<c>true</c> if this instance can asymmetric bin; otherwise, <c>false</c>.
    ''' </value>
    ''' <exception cref="NotConnectedException">Must throw exception if the value is not known (n.b. normally only
    ''' occurs if no connection established and camera must be queried)</exception>
    ReadOnly Property CanAsymmetricBin() As Boolean

    ''' <summary>
    ''' If <c>true</c>, the camera's cooler power setting can be read.
    ''' </summary>
    ''' <value>
    ''' 	<c>true</c> if this instance can get cooler power; otherwise, <c>false</c>.
    ''' </value>
    ''' <exception cref="NotConnectedException">Thrown if the driver is not connected.</exception>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' </remarks>
    ReadOnly Property CanGetCoolerPower() As Boolean

    ''' <summary>
    ''' Returns a flag indicating whether this camera supports pulse guiding
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' <para>Returns <c>true</c> if the camera can send autoguider pulses to the telescope mount; <c>false</c> if not.  
    ''' Note: this does not provide any indication of whether the autoguider cable is actually connected.</para>
    ''' </remarks>
    ''' <value>
    ''' 	<c>true</c> if this instance can pulse guide; otherwise, <c>false</c>.
    ''' </value>
    ''' <exception cref="NotConnectedException">Thrown if the driver is not connected.</exception>
    ReadOnly Property CanPulseGuide() As Boolean

    ''' <summary>
    ''' Returns a flag indicatig whether this camera supports setting the CCD temperature
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' <para>If <c>true</c>, the camera's cooler setpoint can be adjusted. If <c>false</c>, the camera
    ''' either uses open-loop cooling or does not have the ability to adjust temperature
    ''' from software, and setting the <see cref="SetCCDTemperature" /> property has no effect.</para>
    ''' </remarks>
    ''' <value>
    ''' 	<c>true</c> if this instance can set CCD temperature; otherwise, <c>false</c>.
    ''' </value>
    ''' <exception cref="NotConnectedException">Thrown if the driver is not connected.</exception>
    ReadOnly Property CanSetCCDTemperature() As Boolean

    ''' <summary>
    ''' Returns a flag indicating whether this camera can stop an exposure that is in progress
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' <para>Some cameras support <see cref="StopExposure" />, which allows the exposure to be terminated
    ''' before the exposure timer completes, but will still read out the image.  Returns
    ''' <c>true</c> if <see cref="StopExposure" /> is available, <c>false</c> if not.</para>
    ''' </remarks>
    ''' <value>
    ''' 	<c>true</c> if the camera can stop the exposure; otherwise, <c>false</c>.
    ''' </value>
    ''' <exception cref=" PropertyNotImplementedException">not supported</exception>
    ''' <exception cref=" NotConnectedException">an error condition such as connection failure is present</exception>
    ReadOnly Property CanStopExposure() As Boolean

    ''' <summary>
    ''' Returns the current CCD temperature in degrees Celsius.
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' <para>Only valid if  <see cref="CanSetCCDTemperature" /> is <c>true</c>.</para>
    ''' </remarks>
    ''' <value>The CCD temperature.</value>
    ''' <exception cref="InvalidValueException">Must throw exception if data unavailable.</exception>
    ReadOnly Property CCDTemperature() As Double

    ''' <summary>
    ''' Turns on and off the camera cooler, and returns the current on/off state.
    ''' </summary>
    ''' <remarks>
    ''' <b>Warning:</b> turning the cooler off when the cooler is operating at high delta-T
    ''' (typically &gt;20C below ambient) may result in thermal shock.  Repeated thermal
    ''' shock may lead to damage to the sensor or cooler stack.  Please consult the
    ''' documentation supplied with the camera for further information.
    ''' </remarks>
    ''' <value><c>true</c> if the cooler is on; otherwise, <c>false</c>.</value>
    ''' <exception cref=" PropertyNotImplementedException">not supported</exception>
    ''' <exception cref=" NotConnectedException">an error condition such as connection failure is present</exception>
    Property CoolerOn() As Boolean

    ''' <summary>
    ''' Returns the present cooler power level, in percent.
    ''' </summary>
    ''' <remarks>
    ''' Returns zero if <see cref="CoolerOn" /> is <c>false</c>.
    ''' </remarks>
    ''' <value>The cooler power.</value>
    ''' <exception cref=" PropertyNotImplementedException">not supported</exception>
    ''' <exception cref=" NotConnectedException">an error condition such as connection failure is present</exception>
    ReadOnly Property CoolerPower() As Double

    ''' <summary>
    ''' Returns the gain of the camera in photoelectrons per A/D unit.
    ''' </summary>
    ''' <remarks>
    ''' Some cameras have multiple gain modes; these should be selected via the  <see cref="SetupDialog" /> and thus are
    ''' static during a session.
    ''' </remarks>
    ''' <value>The electrons per ADU.</value>
    ''' <exception cref=" NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property ElectronsPerADU() As Double

    ''' <summary>
    ''' Reports the full well capacity of the camera in electrons, at the current camera settings (binning, SetupDialog settings, etc.)
    ''' </summary>
    ''' <value>The full well capacity.</value>
    ''' <exception cref=" NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property FullWellCapacity() As Double

    ''' <summary>
    ''' Returns a flag indicating whether this camera has a mechanical shutter
    ''' </summary>
    ''' <remarks>
    ''' If <c>true</c>, the camera has a mechanical shutter. If <c>false</c>, the camera does not have
    ''' a shutter.  If there is no shutter, the  <see cref="StartExposure">StartExposure</see> command will ignore the
    ''' Light parameter.
    ''' </remarks>
    ''' <value>
    ''' 	<c>true</c> if this instance has shutter; otherwise, <c>false</c>.
    ''' </value>
    ''' <exception cref="NotConnectedException">Thrown if the driver is not connected.</exception>
    ReadOnly Property HasShutter() As Boolean

    ''' <summary>
    ''' Returns the current heat sink temperature (called "ambient temperature" by some manufacturers) in degrees Celsius. 
    ''' </summary>
    ''' <remarks>
    ''' Only valid if  <see cref="CanSetCCDTemperature" /> is <c>true</c>.
    ''' </remarks>
    ''' <value>The heat sink temperature.</value>
    ''' <exception cref=" NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property HeatSinkTemperature() As Double

    ''' <summary>
    ''' Returns a safearray of int of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure. 
    ''' </summary>
    ''' <remarks>
    ''' The application must inspect the Safearray parameters to determine the dimensions. 
    ''' <para>Note: if <see cref="NumX" /> or <see cref="NumY" /> is changed after a call to <see cref="StartExposure">StartExposure</see> it will 
    ''' have no effect on the size of this array. This is the preferred method for programs (not scripts) to download 
    ''' iamges since it requires much less memory.</para>
    ''' <para>For color or multispectral cameras, will produce an array of  <see cref="NumX" /> * <see cref="NumY" /> *
    ''' NumPlanes.  If the application cannot handle multispectral images, it should use just the first plane.</para>
    ''' </remarks>
    ''' <value>The image array.</value>
    ''' <exception cref=" NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property ImageArray() As Object

    ''' <summary>
    ''' Returns a safearray of Variant of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure. 
    ''' </summary>
    ''' <remarks>
    ''' The application must inspect the Safearray parameters to
    ''' determine the dimensions. Note: if <see cref="NumX" /> or <see cref="NumY" /> is changed after a call to
    ''' <see cref="StartExposure">StartExposure</see> it will have no effect on the size of this array. This property
    ''' should only be used from scripts due to the extremely high memory utilization on
    ''' large image arrays (26 bytes per pixel). Pixels values should be in Short, int,
    ''' or Double format.
    ''' <para>For color or multispectral cameras, will produce an array of <see cref="NumX" /> * <see cref="NumY" /> *
    ''' NumPlanes.  If the application cannot handle multispectral images, it should use
    ''' just the first plane.</para>
    ''' </remarks>
    ''' <value>The image array variant.</value>
    ''' <exception cref=" NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property ImageArrayVariant() As Object

    ''' <summary>
    ''' Returns a flag indicating whether the image is ready to be downloaded fom the camera
    ''' </summary>
    ''' <remarks>
    ''' If <c>true</c>, there is an image from the camera available. If <c>false</c>, no image
    ''' is available and attempts to use the <see cref="ImageArray" /> method will produce an exception
    ''' </remarks>.
    ''' <value><c>true</c> if [image ready]; otherwise, <c>false</c>.</value>
    ''' <exception cref=" NotConnectedException">hardware or communications connection error has occurred.</exception>
    ReadOnly Property ImageReady() As Boolean

    ''' <summary>
    ''' Returns a flag indicating whether the camera is currrently in a <see cref="PulseGuide">PulseGuide</see> operation.
    ''' </summary>
    ''' <remarks>
    ''' If <c>true</c>, pulse guiding is in progress. Required if the <see cref="PulseGuide">PulseGuide</see> method
    ''' (which is non-blocking) is implemented. See the <see cref="PulseGuide">PulseGuide</see> method.
    ''' </remarks>
    ''' <value>
    ''' 	<c>true</c> if this instance is pulse guiding; otherwise, <c>false</c>.
    ''' </value> 
    ''' <exception cref=" NotConnectedException">hardware or communications connection error has occurred.</exception>
    ReadOnly Property IsPulseGuiding() As Boolean

    ''' <summary>
    ''' Reports the actual exposure duration in seconds (i.e. shutter open time).  
    ''' </summary>
    ''' <remarks>
    ''' This may differ from the exposure time requested due to shutter latency, camera timing precision, etc.
    ''' </remarks>
    ''' <value>The last duration of the exposure.</value>
    ''' <exception cref="PropertyNotImplementedException">Must throw an exception if not supported</exception>
    ''' <exception cref="InvalidOperationException">If called before any exposure has been taken</exception>
    ReadOnly Property LastExposureDuration() As Double

    ''' <summary>
    ''' Reports the actual exposure start in the FITS-standard CCYY-MM-DDThh:mm:ss[.sss...] format.
    ''' The start time must be UTC.
    ''' </summary>
    ''' <value>The last exposure start time in UTC.</value>
    ''' <exception cref="PropertyNotImplementedException">Must throw an exception if not supported</exception>
    ''' <exception cref="InvalidOperationException">If called before any exposure has been taken</exception>
    ReadOnly Property LastExposureStartTime() As String

    ''' <summary>
    ''' Reports the maximum ADU value the camera can produce.
    ''' </summary>
    ''' <value>The maximum ADU.</value>
    ''' <exception cref="NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property MaxADU() As Integer

    ''' <summary>
    ''' Returns the maximum allowed binning for the X camera axis
    ''' </summary>
    ''' <remarks>
    ''' If <see cref="CanAsymmetricBin" /> = <c>false</c>, returns the maximum allowed binning factor. If
    ''' <see cref="CanAsymmetricBin" /> = <c>true</c>, returns the maximum allowed binning factor for the X axis.
    ''' </remarks>
    ''' <value>The max bin X.</value>
    ''' <exception cref="NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property MaxBinX() As Short

    ''' <summary>
    ''' Returns the maximum allowed binning for the Y camera axis
    ''' </summary>
    ''' <remarks>
    ''' If <see cref="CanAsymmetricBin" /> = <c>false</c>, equals <see cref="MaxBinX" />. If <see cref="CanAsymmetricBin" /> = <c>true</c>,
    ''' returns the maximum allowed binning factor for the Y axis.
    ''' </remarks>
    ''' <value>The max bin Y.</value>
    ''' <exception cref="NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property MaxBinY() As Short

    ''' <summary>
    ''' Sets the subframe width. Also returns the current value.  
    ''' </summary>
    ''' <remarks>
    ''' If binning is active, value is in binned pixels.  No error check is performed when the value is set. 
    ''' Should default to <see cref="CameraXSize" />.
    ''' </remarks>
    ''' <value>The num X.</value>
    Property NumX() As Integer

    ''' <summary>
    ''' Sets the subframe height. Also returns the current value.
    ''' </summary>
    ''' <remarks>
    ''' If binning is active,
    ''' value is in binned pixels.  No error check is performed when the value is set.
    ''' Should default to <see cref="CameraYSize" />.
    ''' </remarks>
    ''' <value>The num Y.</value>
    Property NumY() As Integer

    ''' <summary>
    ''' Returns the width of the CCD chip pixels in microns.
    ''' </summary>
    ''' <value>The pixel size X.</value>
    ''' <exception cref="NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property PixelSizeX() As Double

    ''' <summary>
    ''' Returns the height of the CCD chip pixels in microns.
    ''' </summary>
    ''' <value>The pixel size Y.</value>
    ''' <exception cref="NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property PixelSizeY() As Double

    ''' <summary>
    ''' Activates the Camera's mount control sytem to instruct the mount to move in a particular direction for a given period of time
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>May throw a not implemented exception if this camera does not support PulseGuide</b></p>
    ''' <para>This method returns only after the move has completed.</para>
    ''' <para>
    ''' The (symbolic) values for GuideDirections are:
    ''' <list type="bullet">
    ''' <listheader><description>Constant     Value      Description</description></listheader>
    ''' <item><description>guideNorth     0        North (+ declination/elevation)</description></item>
    ''' <item><description>guideSouth     1        South (- declination/elevation)</description></item>
    ''' <item><description>guideEast      2        East (+ right ascension/azimuth)</description></item>
    ''' <item><description>guideWest      3        West (+ right ascension/azimuth)</description></item>
    ''' </list>
    ''' </para>
    ''' <para>Note: directions are nominal and may depend on exact mount wiring.  
    ''' <see cref="GuideDirections.guideNorth" /> must be opposite <see cref="GuideDirections.guideSouth" />, and 
    ''' <see cref="GuideDirections.guideEast" /> must be opposite <see cref="GuideDirections.guideWest" />.</para>
    ''' </remarks>
    ''' <param name="Direction">The direction of movement.</param>
    ''' <param name="Duration">The duration of movement in milli-seconds.</param>
    ''' <exception cref="MethodNotImplementedException">PulseGuide command is unsupported</exception>
    ''' <exception cref=" DriverException">PulseGuide command is unsuccessful</exception>
    ''' <exception cref="NotConnectedException">Thrown if the driver is not connected.</exception>
    Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Integer)

    ''' <summary>
    ''' Sets the camera cooler setpoint in degrees Celsius, and returns the current setpoint.
    ''' </summary>
    ''' <remarks>
    ''' <para>The driver should throw an <see cref="InvalidValueException" /> if an attempt is made to set <see cref="SetCCDTemperature" /> 
    ''' outside the valid range for the camera. As an assitance to driver authors, to protect equipment and prevent harm to individuals, 
    ''' Conform will report an issue if it is possible to set <see cref="SetCCDTemperature" /> below -280C or above +100C.</para>
    ''' <b>Note:</b>  Camera hardware and/or driver should perform cooler ramping, to prevent
    ''' thermal shock and potential damage to the CCD array or cooler stack.
    ''' </remarks>
    ''' <value>The set CCD temperature.</value>
    ''' <exception cref="DriverException">Must throw exception if command not successful.</exception>
    ''' <exception cref="InvalidValueException">Must throw an InvalidValueException if an attempt is made to set a value is outside the 
    ''' camera's valid termperature setpoint range.</exception>
    ''' <exception cref="PropertyNotImplementedException">Must throw exception if <see cref="CanSetCCDTemperature" /> is <c>false</c>.</exception>
    ''' <exception cref="NotConnectedException">Thrown if the driver is not connected.</exception>
    Property SetCCDTemperature() As Double

    ''' <summary>
    ''' Starts an exposure. Use <see cref="ImageReady" /> to check when the exposure is complete.
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a MethodNotImplementedException.</b></p>
    ''' <para>A dark frame or bias exposure may be shorter than the V2 <see cref="ExposureMin" /> value and for a bias frame can be zero.
    ''' Check the value of <see cref="StartExposure">Light</see> and allow exposures down to 0 seconds 
    ''' if <see cref="StartExposure">Light</see> is <c>false</c>.  If the hardware will not
    ''' support an exposure duration of zero then, for dark and bias frames, set it to the minimum that is possible.</para>
    ''' <para>Some applications will set an exposure time of zero for bias frames so it's important that the driver allows this.</para>
    ''' </remarks>
    ''' <param name="Duration">Duration of exposure in seconds, can be zero if <see cref="StartExposure">Light</see> is <c>false</c></param>
    ''' <param name="Light"><c>true</c> for light frame, <c>false</c> for dark frame (ignored if no shutter)</param>
    ''' <exception cref=" InvalidValueException"><see cref="NumX" />, <see cref="NumY" />, <see cref="BinX" />, 
    ''' <see cref="BinY" />, <see cref="StartX" />, <see cref="StartY" />, or <see cref="StartExposure">Duration</see> parameters are invalid.</exception>
    ''' <exception cref=" InvalidOperationException"><see cref="CanAsymmetricBin" /> is <c>false</c> and <see cref="BinX" /> != <see cref="BinY" /></exception>
    ''' <exception cref="NotConnectedException">the exposure cannot be started for any reason, such as a hardware or communications error</exception>
    Sub StartExposure(ByVal Duration As Double, ByVal Light As Boolean)

    ''' <summary>
    ''' Sets the subframe start position for the X axis (0 based) and returns the current value.
    ''' </summary>
    ''' <remarks> 
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' <para>If binning is active, value is in binned pixels.</para>
    ''' </remarks>
    ''' <value>The start X.</value>
    ''' <exception cref="NotConnectedException">Thrown if the driver is not connected.</exception>
    Property StartX() As Integer

    ''' <summary>
    ''' Sets the subframe start position for the Y axis (0 based). Also returns the current value.  
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' <para>If binning is active, value is in binned pixels.</para>
    ''' </remarks>
    ''' <value>The start Y.</value>
    ''' <exception cref="NotConnectedException">Thrown if the driver is not connected.</exception>
    Property StartY() As Integer

    ''' <summary>
    ''' Stops the current exposure, if any.
    ''' </summary>
    ''' <remarks>
    ''' <p style="color:red"><b>May throw a not implemented exception</b></p>
    ''' <para>If an exposure is in progress, the readout process is initiated.  Ignored if readout is already in process.</para>
    ''' </remarks>
    ''' <exception cref="MethodNotImplementedException">Must throw an exception if CanStopExposure is <c>false</c></exception>
    ''' <exception cref="NotConnectedException">Must throw an exception if the camera or connection has an error condition</exception>
    ''' <exception cref="DriverException">Must throw an exception if for any reason no image readout will be available.</exception>
    Sub StopExposure()

#End Region

#Region "ICameraV2 methods"

    'ICameraV2 members

    ''' <summary>
    ''' Returns the X offset of the Bayer matrix, as defined in <see cref="SensorType" />.
    ''' </summary>
    ''' <returns>The Bayer colour matrix X offset, as defined in <see cref="SensorType" />.</returns>
    ''' <exception cref="PropertyNotImplementedException">Monochrome cameras must throw this exception, colour cameras must not.</exception>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented by colour cameras, monochrome cameras must throw a PropertyNotImplementedException</b></p>
    ''' <para>Since monochrome cameras don't have a bayer colour matrix by definition, such cameras shold throw a <see cref="PropertyNotImplementedException" />.
    ''' Colour cameras should always return a value and must not throw a <see cref="PropertyNotImplementedException" /></para>
    ''' <para>The value returned must be in the range 0 to M-1 where M is the width of the Bayer matrix. The offset is relative to the 0,0 pixel in 
    ''' the sensor array, and does not change to reflect subframe settings.</para>
    ''' <para>It is recommended that this function be called only after a <see cref="Connected">connection</see> is established with 
    ''' the camera hardware, to ensure that the driver is aware of the capabilities of the specific camera model.</para>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    ReadOnly Property BayerOffsetX As Short

    ''' <summary>
    ''' Returns the Y offset of the Bayer matrix, as defined in <see cref="SensorType" />.
    ''' </summary>
    ''' <returns>The Bayer colour matrix Y offset, as defined in <see cref="SensorType" />.</returns>
    ''' <exception cref="PropertyNotImplementedException">Monochrome cameras must throw this exception, colour cameras must not.</exception>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented by colour cameras, monochrome cameras must throw a PropertyNotImplementedException</b></p>
    ''' <para>Since monochrome cameras don't have a bayer colour matrix by definition, such cameras shold throw a <see cref="PropertyNotImplementedException" />.
    ''' Colour cameras should always return a value and must not throw a <see cref="PropertyNotImplementedException" /></para>
    ''' <para>The value returned must be in the range 0 to M-1 where M is the width of the Bayer matrix. The offset is relative to the 0,0 pixel in 
    ''' the sensor array, and does not change to reflect subframe settings.</para>
    ''' <para>It is recommended that this function be called only after a <see cref="Connected">connection</see> is established with 
    ''' the camera hardware, to ensure that the driver is aware of the capabilities of the specific camera model.</para>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    ReadOnly Property BayerOffsetY As Short

    ''' <summary>
    ''' Camera has a fast readout mode
    ''' </summary>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <returns><c>true</c> when the camera supports a fast readout mode</returns>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' It is recommended that this function be called only after a <see cref="Connected">connection</see> is established with the camera hardware, to 
    ''' ensure that the driver is aware of the capabilities of the specific camera model.
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    ReadOnly Property CanFastReadout As Boolean

    ''' <summary>
    ''' Returns the maximum exposure time supported by <see cref="StartExposure">StartExposure</see>.
    ''' </summary>
    ''' <returns>The maximum exposure time, in seconds, that the camera supports</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' It is recommended that this function be called only after 
    ''' a <see cref="Connected">connection</see> is established with the camera hardware, to ensure that the driver is aware of the capabilities of the 
    ''' specific camera model.
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    ReadOnly Property ExposureMax As Double

    ''' <summary>
    ''' Minimium exposure time
    ''' </summary>
    ''' <returns>The minimum exposure time, in seconds, that the camera supports through <see cref="StartExposure">StartExposure</see></returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' This must be a non-zero number representing the shortest possible exposure time supported by the camera model.
    ''' <para>Please note that for bias frame acquisition an even shorter exposure may be possible; please see <see cref="StartExposure">StartExposure</see> 
    ''' for more information.</para>
    ''' <para>It is recommended that this function be called only after a <see cref="Connected">connection</see> is established with the camera hardware, to ensure 
    ''' that the driver is aware of the capabilities of the specific camera model.</para>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    ReadOnly Property ExposureMin As Double

    ''' <summary>
    ''' Exposure resolution
    ''' </summary>
    ''' <returns>The smallest increment in exposure time supported by <see cref="StartExposure">StartExposure</see>.</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
    ''' This can be used, for example, to specify the resolution of a user interface "spin control" used to dial in the exposure time.
    ''' <para>Please note that the Duration provided to <see cref="StartExposure">StartExposure</see> does not have to be an exact multiple of this number; 
    ''' the driver should choose the closest available value. Also in some cases the resolution may not be constant over the full range 
    ''' of exposure times; in this case the smallest increment would be appropriate. A value of 0.0 shall indicate that there is no minimum resulution
    ''' except that imposed by the resolution of the double value itself.</para>
    ''' <para>It is recommended that this function be called only after a <see cref="Connected">connection</see> is established with the camera hardware, to ensure 
    ''' that the driver is aware of the capabilities of the specific camera model.</para>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    ReadOnly Property ExposureResolution As Double

    ''' <summary>
    ''' Gets or sets Fast Readout Mode
    ''' </summary>
    ''' <value><c>true</c> for fast readout mode, <c>false</c> for normal mode</value>
    ''' <exception cref="PropertyNotImplementedException">Thrown if <see cref="CanFastReadout" /> is <c>false</c>.</exception>
    ''' <exception cref="NotConnectedException">Thrown if the driver is not connected and a connection is required to obtain this information.</exception>
    ''' <remarks><p style="color:red"><b>Must throw a PropertyNotImplementedException if CanFastReadout is false or 
    ''' return a boolean value if CanFastReadout is true.</b></p>
    ''' Must thrown an exception if no <see cref="Connected">connection</see> is established to the camera. Must throw 
    ''' a <see cref="PropertyNotImplementedException" /> if <see cref="CanFastReadout" /> returns <c>false</c>.
    ''' <para>Many cameras have a "fast mode" intended for use in focusing. When set to <c>true</c>, the camera will operate in Fast mode; when 
    ''' set <c>false</c>, the camera will operate normally. This property, if implemented, should default to <c>False</c>.</para>
    ''' <para>Please note that this function may in some cases interact with <see cref="ReadoutModes" />; for example, there may be modes where 
    ''' the Fast/Normal switch is meaningless. In this case, it may be preferable to use the <see cref="ReadoutModes" /> function to control 
    ''' fast/normal switching.</para>
    ''' <para>If this feature is not available, then <see cref="CanFastReadout" /> must return <c>false</c>.</para>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    Property FastReadout As Boolean

    ''' <summary>
    ''' Index into the <see cref="Gains" /> array for the selected camera gain
    ''' </summary>
    ''' <value>Short integer index for the current camera gain in the <see cref="Gains" /> string array.</value>
    ''' <returns>Index into the Gains array for the selected camera gain</returns>
    ''' <exception cref="PropertyNotImplementedException">Must throw an exception if gain is not supported</exception>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
    ''' <remarks><p style="color:red"><b>May throw a PropertyNotImplementedException if Gain is not supported by the camera.</b></p>
    ''' <see cref="Gain" /> can be used to adjust the gain setting of the camera, if supported. There are two typical usage scenarios:
    ''' <ul>
    ''' <li>DSLR Cameras - <see cref="Gains" /> will return a 0-based array of strings, which correspond to different gain settings such as 
    ''' "ISO 800". <see cref="Gain" /> must be set to an integer in this range. <see cref="GainMin" /> and <see cref="GainMax" /> must thrown an exception if 
    ''' this mode is used.</li>
    ''' <li>Adjustable gain CCD cameras - <see cref="GainMin" /> and <see cref="GainMax" /> return integers, which specify the valid range for <see cref="GainMin" /> and <see cref="Gain" />.</li>
    ''' </ul>
    '''<para>The driver must default <see cref="Gain" /> to a valid value. </para>
    '''<para>Please note that <see cref="ReadoutMode" /> may in some cases affect the gain of the camera; if so the driver must be written such 
    ''' that the two properties do not conflict if both are used.</para>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    Property Gain As Short

    ''' <summary>
    ''' Maximum value of <see cref="Gain" />
    ''' </summary>
    ''' <value>Short integer representing the maximum gain value supported by the camera.</value>
    ''' <returns>The maximum gain value that this camera supports</returns>
    ''' <exception cref="PropertyNotImplementedException">Must throw an exception if GainMax is not supported</exception>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <remarks><p style="color:red"><b>May throw a PropertyNotImplementedException if GainMax is not supported by the camera..</b></p>
    ''' When specifying the gain setting with an integer value, <see cref="GainMax" /> is used in conjunction with <see cref="GainMin" /> to 
    ''' specify the range of valid settings.
    ''' <para><see cref="GainMax" /> shall be greater than <see cref="GainMin" />. If either is available, then both must be available.</para>
    ''' <para>Please see <see cref="Gain" /> for more information.</para>
    ''' <para>It is recommended that this function be called only after a <see cref="Connected">connection</see> is established with the camera hardware, to ensure 
    ''' that the driver is aware of the capabilities of the specific camera model.</para>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    ReadOnly Property GainMax As Short

    ''' <summary>
    ''' Minimum value of <see cref="Gain" />
    ''' </summary>
    ''' <returns>The minimum gain value that this camera supports</returns>
    ''' <exception cref="PropertyNotImplementedException">Must throw an exception if GainMin is not supported</exception>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <remarks><p style="color:red"><b>May throw a PropertyNotImplementedException if GainMin is not supported by the camera.</b></p>
    ''' When specifying the gain setting with an integer value, <see cref="GainMin" /> is used in conjunction with <see cref="GainMax" /> to 
    ''' specify the range of valid settings.
    ''' <para><see cref="GainMax" /> shall be greater than <see cref="GainMin" />. If either is available, then both must be available.</para>
    ''' <para>Please see <see cref="Gain" /> for more information.</para>
    ''' <para>It is recommended that this function be called only after a <see cref="Connected">connection</see> is established with the camera hardware, to ensure 
    ''' that the driver is aware of the capabilities of the specific camera model.</para>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    ReadOnly Property GainMin As Short

    ''' <summary>
    ''' Gains supported by the camera
    ''' </summary>
    ''' <returns>An ArrayList of gain names </returns>
    ''' <exception cref="PropertyNotImplementedException">Must throw an exception if Gains is not supported</exception>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <remarks><p style="color:red"><b>May throw a PropertyNotImplementedException if Gains is not supported by the camera.</b></p>
    ''' <see cref="Gains" /> provides a 0-based array of available gain settings.  This is often used to specify ISO settings for DSLR cameras.  
    ''' Typically the application software will display the available gain settings in a drop list. The application will then supply 
    ''' the selected index to the driver via the <see cref="Gain" /> property. 
    ''' <para>The <see cref="Gain" /> setting may alternatively be specified using integer values; if this mode is used then <see cref="Gains" /> is invalid 
    ''' and must throw an exception. Please see <see cref="GainMax" /> and <see cref="GainMin" /> for more information.</para>
    ''' <para>It is recommended that this function be called only after a <see cref="Connected">connection</see> is established with the camera hardware, 
    ''' to ensure that the driver is aware of the capabilities of the specific camera model.</para>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    ReadOnly Property Gains As ArrayList

    ''' <summary>
    ''' Percent conpleted, Interface Version 2 only
    ''' </summary>
    ''' <returns>A value between 0 and 100% indicating the completeness of this operation</returns>
    ''' <exception cref="PropertyNotImplementedException">Must throw an exception if PercentCompleted is not supported</exception>
    ''' <exception cref="InvalidOperationException">Thrown when it is inappropriate to call <see cref="PercentCompleted" /></exception>
    ''' <remarks><p style="color:red"><b>May throw a PropertyNotImplementedException if PercentCompleted is not supported by the camera.</b></p>
    ''' If valid, returns an integer between 0 and 100, where 0 indicates 0% progress (function just started) and 
    ''' 100 indicates 100% progress (i.e. completion).
    ''' <para>At the discretion of the driver author, <see cref="PercentCompleted" /> may optionally be valid 
    ''' when <see cref="CameraState" /> is in any or all of the following 
    ''' states: <see cref="CameraStates.cameraExposing" />, 
    ''' <see cref="CameraStates.cameraWaiting" />, <see cref="CameraStates.cameraReading" /> 
    ''' or <see cref="CameraStates.cameraDownload" />. In all other states an exception shall be thrown.</para>
    ''' <para>Typically the application user interface will show a progress bar based on the <see cref="PercentCompleted" /> value.</para>
    ''' <para>Please note that client applications are not required to use this value, and in some cases may display status 
    ''' information based on other information, such as time elapsed.</para>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    ReadOnly Property PercentCompleted As Short

    ''' <summary>
    ''' Readout mode, Interface Version 2 only
    ''' </summary>
    ''' <value></value>
    ''' <returns>Short integer index into the <see cref="ReadoutModes">ReadoutModes</see> array of string readout mode names indicating 
    ''' the camera's current readout mode.</returns>
    ''' <exception cref="InvalidValueException">Must throw an exception if set to an illegal or unavailable mode.</exception>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented if CanFastReadout is false, must throw a PropertyNotImplementedException if 
    ''' CanFastReadout is true.</b></p>
    ''' <see cref="ReadoutMode" /> is an index into the array <see cref="ReadoutModes" />, and selects the desired readout mode for the camera.  
    ''' Defaults to 0 if not set.  Throws an exception if the selected mode is not available.
    ''' <para>It is strongly recommended, but not required, that driver authors make the 0-index mode suitable for standard imaging operations, 
    ''' since it is the default.</para>
    ''' <para>Please see <see cref="ReadoutModes" /> for additional information.</para>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    Property ReadoutMode As Short

    ''' <summary>
    ''' List of available readout modes, Interface Version 2 only
    ''' </summary>
    ''' <returns>An ArrayList of readout mode names</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <remarks><p style="color:red"><b>Must be implemented if CanFastReadout is false, must throw a PropertyNotImplementedException if 
    ''' CanFastReadout is true.</b></p>
    ''' This property provides an array of strings, each of which describes an available readout mode of the camera.  
    ''' At least one string must be present in the list. The user interface of a control application will typically present to the 
    ''' user a drop-list of modes.  The choice of available modes made available is entirely at the discretion of the driver author. 
    ''' Please note that if the camera has many different modes of operation, then the most commonly adjusted settings should be in 
    ''' <see cref="ReadoutModes" />; additional settings may be provided using <see cref="SetupDialog" />.
    ''' <para>To select a mode, the application will set <see cref="ReadoutMode" /> to the index of the desired mode.  The index is zero-based.</para>
    ''' <para>This property should only be read while a <see cref="Connected">connection</see> to the camera is actually established.  Drivers often support 
    ''' multiple cameras with different capabilities, which are not known until the <see cref="Connected">connection</see> is made.  If the available readout modes 
    ''' are not known because no <see cref="Connected">connection</see> has been established, this property shall throw an exception.</para>
    ''' <para>Please note that the default <see cref="ReadoutMode" /> setting is 0. It is strongly recommended, but not required, that 
    ''' driver authors use the 0-index mode for standard imaging operations, since it is the default.</para>
    ''' <para>This feature may be used in parallel with <see cref="FastReadout" />; however, care should be taken to ensure that the two 
    ''' features work together consistently. If there are modes that are inconsistent having a separate fast/normal switch, then it 
    ''' may be better to simply list Fast as one of the <see cref="ReadoutModes" />.</para>
    ''' <para>It is recommended that this function be called only after a <see cref="Connected">connection</see> is established with 
    ''' the camera hardware, to ensure that the driver is aware of the capabilities of the specific camera model.</para>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    ReadOnly Property ReadoutModes As ArrayList

    ''' <summary>
    ''' Sensor name, Interface Version 2 only
    ''' ## Mandatory must return an empty string if the sensor is unknown
    ''' </summary>
    ''' <returns>The name of the sensor used within the camera.</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <remarks><p style="color:red"><b>May throw a PropertyNotImplementedException if the sensor's name is not known.</b></p>
    ''' <para>Returns the name (datasheet part number) of the sensor, e.g. ICX285AL.  The format is to be exactly as shown on 
    ''' manufacturer data sheet, subject to the following rules:
    ''' <list type="bullet">
    ''' <item>All letters shall be uppercase.</item>
    ''' <item>Spaces shall not be included.</item>
    ''' <item>Any extra suffixes that define region codes, package types, temperature range, coatings, grading, color/monochrome, 
    ''' etc. shall not be included.</item>
    ''' <item>For color sensors, if a suffix differentiates different Bayer matrix encodings, it shall be included.</item>
    ''' <item>The call shall return an empty string if the sensor name is not known.</item>
    ''' </list>  </para>
    ''' <para>Examples:</para>
    ''' <list type="bullet">
    ''' <item><description>ICX285AL-F shall be reported as ICX285</description></item>
    ''' <item><description>KAF-8300-AXC-CD-AA shall be reported as KAF-8300</description></item>
    ''' </list>
    ''' <para><b>Note:</b></para>
    ''' <para>The most common usage of this property is to select approximate color balance parameters to be applied to 
    ''' the Bayer matrix of one-shot color sensors.  Application authors should assume that an appropriate IR cutoff filter is 
    ''' in place for color sensors.</para>
    ''' <para>It is recommended that this function be called only after a <see cref="Connected">connection</see> is established with 
    ''' the camera hardware, to ensure that the driver is aware of the capabilities of the specific camera model.</para>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' </remarks>
    ReadOnly Property SensorName As String

    ''' <summary>
    ''' Type of colour information returned by the the camera sensor, Interface Version 2 only
    ''' </summary>
    ''' <value></value>
    ''' <returns>The <see cref="ASCOM.DeviceInterface.SensorType" /> enum value of the camera sensor</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active <see cref="Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
    ''' <remarks><p style="color:red"><b>May throw a PropertyNotImplementedException if the sensor type is not known.</b></p>
    ''' <para>This is only available for the Camera Interface Version 2</para>
    ''' <para><see cref="SensorType" /> returns a value indicating whether the sensor is monochrome, or what Bayer matrix it encodes. If this value 
    ''' cannot be determined by interrogating the camera, the appropriate value may be set through the user setup dialogue or the property may
    ''' return a <see cref="PropertyNotImplementedException" />. Please note that for some cameras, changing <see cref="BinX" />,
    ''' <see cref="BinY" /> or <see cref="ReadoutMode" /> may change the apparent type of the sensor and so you should change the value returned here 
    ''' to match if this is the case for your camera.</para>
    ''' <para>The following values are defined:</para>
    ''' <para>
    ''' <table style="width:76.24%;" cellspacing="0" width="76.24%">
    ''' <col style="width: 11.701%;"></col>
    ''' <col style="width: 20.708%;"></col>
    ''' <col style="width: 67.591%;"></col>
    ''' <tr>
    ''' <td colspan="1" rowspan="1" style="width: 11.701%; padding-right: 10px; padding-left: 10px; 
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-top-color: #000000; border-top-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid;
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
    ''' background-color: #00ffff;" width="11.701%">
    ''' <b>Value</b></td>
    ''' <td colspan="1" rowspan="1" style="width: 20.708%; padding-right: 10px; padding-left: 10px; 
    ''' border-top-color: #000000; border-top-style: Solid; 
    ''' border-right-style: Solid; border-right-color: #000000; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
    ''' background-color: #00ffff;" width="20.708%">
    ''' <b>Enumeration</b></td>
    ''' <td colspan="1" rowspan="1" style="width: 67.591%; padding-right: 10px; padding-left: 10px; 
    ''' border-top-color: #000000; border-top-style: Solid; 
    ''' border-right-style: Solid; border-right-color: #000000; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
    ''' background-color: #00ffff;" width="67.591%">
    ''' <b>Meaning</b></td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 0</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' Monochrome</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' Camera produces monochrome array with no Bayer encoding</td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 1</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' Colour</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' Camera produces color image directly, requiring not Bayer decoding</td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 2</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' RGGB</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' Camera produces RGGB encoded Bayer array images</td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 3</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' CMYG</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' Camera produces CMYG encoded Bayer array images</td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 4</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' CMYG2</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' Camera produces CMYG2 encoded Bayer array images</td>
    ''' </tr>
    ''' <tr>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-left-color: #000000; border-left-style: Solid; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' 5</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' LRGB</td>
    ''' <td style="padding-right: 10px; padding-left: 10px; 
    ''' border-right-color: #000000; border-right-style: Solid; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; 
    ''' border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
    ''' Camera produces Kodak TRUESENSE Bayer LRGB array images</td>
    ''' </tr>
    ''' </table>
    ''' </para>
    ''' <para>Please note that additional values may be defined in future updates of the standard, as new Bayer matrices may be created 
    ''' by sensor manufacturers in the future.  If this occurs, then a new enumeration value shall be defined. The pre-existing enumeration 
    ''' values shall not change.
    ''' <para><see cref="SensorType" /> can possibly change between exposures, for example if <see cref="ReadoutMode">Camera.ReadoutMode</see> is changed, and should always be checked after each exposure.</para>
    ''' <para>In the following definitions, R = red, G = green, B = blue, C = cyan, M = magenta, Y = yellow.  The Bayer matrix is 
    ''' defined with X increasing from left to right, and Y increasing from top to bottom. The pattern repeats every N x M pixels for the 
    ''' entire pixel array, where N is the height of the Bayer matrix, and M is the width.</para>
    ''' <para>RGGB indicates the following matrix:</para>
    ''' </para>
    ''' <para>
    ''' <table style="width:41.254%;" cellspacing="0" width="41.254%">
    ''' <col style="width: 10%;"></col>
    ''' <col style="width: 10%;"></col>
    ''' <col style="width: 10%;"></col>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #ffffff" width="10%">
    ''' </td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>X = 0</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>X = 1</b></td>
    ''' </tr>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff" width="10%">
    ''' <b>Y = 0</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' R</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' " width="10%">
    ''' G</td>
    ''' </tr>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' background-color: #00ffff;" width="10%">
    ''' <b>Y = 1</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' " width="10%">
    ''' G</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' " width="10%">
    ''' B</td>
    ''' </tr>
    ''' </table>
    ''' </para>
    ''' 
    ''' <para>CMYG indicates the following matrix:</para>
    ''' <para>
    ''' <table style="width:41.254%;" cellspacing="0" width="41.254%">
    ''' <col style="width: 10%;"></col>
    ''' <col style="width: 10%;"></col>
    ''' <col style="width: 10%;"></col>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #ffffff" width="10%">
    ''' </td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>X = 0</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>X = 1</b></td>
    ''' </tr>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff" width="10%">
    ''' <b>Y = 0</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' Y</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' " width="10%">
    ''' C</td>
    ''' </tr>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' background-color: #00ffff;" width="10%">
    ''' <b>Y = 1</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' " width="10%">
    ''' G</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' " width="10%">
    ''' M</td>
    ''' </tr>
    ''' 
    ''' </table>
    ''' </para>
    ''' <para>CMYG2 indicates the following matrix:</para>
    ''' <para>
    ''' <table style="width:41.254%;" cellspacing="0" width="41.254%">
    ''' <col style="width: 10%;"></col>
    ''' <col style="width: 10%;"></col>
    ''' <col style="width: 10%;"></col>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #ffffff" width="10%">
    ''' </td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>X = 0</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>X = 1</b></td>
    ''' </tr>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff" width="10%">
    ''' <b>Y = 0</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' C</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' " width="10%">
    ''' Y</td>
    ''' </tr>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>Y = 1</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' M</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' " width="10%">
    ''' G</td>
    ''' </tr>
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff" width="10%">
    ''' <b>Y = 2</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' C</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' " width="10%">
    ''' Y</td>
    ''' </tr>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' background-color: #00ffff;" width="10%">
    ''' <b>Y = 3</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' " width="10%">
    ''' G</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' " width="10%">
    ''' M</td>
    ''' </tr>
    ''' </table>
    ''' </para>
    ''' 
    ''' <para>LRGB indicates the following matrix (Kodak TRUESENSE):</para>
    ''' <para>
    ''' <table style="width:68.757%;" cellspacing="0" width="68.757%">
    ''' <col style="width: 10%;"></col>
    ''' <col style="width: 10%;"></col>
    ''' <col style="width: 10%;"></col>
    ''' <col style="width: 10%;"></col>
    ''' <col style="width: 10%;"></col>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #ffffff" width="10%">
    ''' </td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>X = 0</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>X = 1</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>X = 2</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>X = 3</b></td>
    ''' </tr>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff" width="10%">
    ''' <b>Y = 0</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' L</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' R</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' L</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' " width="10%">
    ''' G</td>
    ''' </tr>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>Y = 1</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' R</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' L</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' G</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' " width="10%">
    ''' L</td>
    ''' </tr>
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff" width="10%">
    ''' <b>Y = 2</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' L</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' G</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' L</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' " width="10%">
    ''' B</td>
    ''' </tr>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' background-color: #00ffff;" width="10%">
    ''' <b>Y = 3</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' " width="10%">
    ''' G</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' " width="10%">
    ''' L</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' " width="10%">
    ''' B</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' " width="10%">
    ''' L</td>
    ''' </tr>
    ''' </table>
    ''' </para>
    ''' 
    ''' <para>The alignment of the array may be modified by <see cref="BayerOffsetX" /> and <see cref="BayerOffsetY" />. 
    ''' The offset is measured from the 0,0 position in the sensor array to the upper left corner of the Bayer matrix table. 
    ''' Please note that the Bayer offset values are not affected by subframe settings.</para>
    ''' <para>For example, if a CMYG2 sensor has a Bayer matrix offset as shown below, <see cref="BayerOffsetX" /> is 0 and <see cref="BayerOffsetY" /> is 1:</para>
    '''<para>
    ''' <table style="width:41.254%;" cellspacing="0" width="41.254%">
    ''' <col style="width: 10%;"></col>
    ''' <col style="width: 10%;"></col>
    ''' <col style="width: 10%;"></col>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #ffffff" width="10%">
    ''' </td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>X = 0</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>X = 1</b></td>
    ''' </tr>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff" width="10%">
    ''' <b>Y = 0</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' G</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' " width="10%">
    ''' M</td>
    ''' </tr>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff;" width="10%">
    ''' <b>Y = 1</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' C</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' " width="10%">
    ''' Y</td>
    ''' </tr>
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' background-color: #00ffff" width="10%">
    ''' <b>Y = 2</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' " width="10%">
    ''' M</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' " width="10%">
    ''' G</td>
    ''' </tr>
    ''' 
    ''' <tr valign="top" align="center">
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' background-color: #00ffff;" width="10%">
    ''' <b>Y = 3</b></td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' " width="10%">
    ''' C</td>
    ''' <td colspan="1" rowspan="1" style="width:10%; 
    ''' border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
    ''' border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
    ''' border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
    ''' border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
    ''' " width="10%">
    ''' Y</td>
    ''' </tr>
    ''' </table>
    ''' </para>
    ''' <para>It is recommended that this function be called only after a <see cref="Connected">connection</see> is established with the camera hardware, to ensure that 
    ''' the driver is aware of the capabilities of the specific camera model.</para>
    ''' </remarks>
    ReadOnly Property SensorType As SensorType

#End Region

End Interface
