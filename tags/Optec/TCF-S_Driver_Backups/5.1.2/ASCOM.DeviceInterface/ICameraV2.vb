Imports System.Runtime.InteropServices
Imports ASCOM.Conform

'-----------------------------------------------------------------------
' <summary>Defines the ICamera Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Defines the ICamera Interface
''' </summary>
<Guid("972CEBC6-0EBE-4efc-99DD-CC5293FDE77B"), ComVisible(True), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)> _
Public Interface ICameraV2 'D95FBC6E-0705-458B-84C0-57E3295DBCCE
    'Inherits IAscomDriver
    'Inherits IDeviceControl
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

#Region "Camera V1 methods"
    ''' <summary>
    ''' Aborts the current exposure, if any, and returns the camera to Idle state.
    ''' Must throw exception if camera is not idle and abort is
    ''' unsuccessful (or not possible, e.g. during download).
    ''' Must throw exception if hardware or communications error
    ''' occurs.
    ''' Must NOT throw an exception if the camera is already idle.
    ''' </summary>
    Sub AbortExposure()

    ''' <summary>
    ''' Sets the binning factor for the X axis.  Also returns the current value.  Should
    ''' default to 1 when the camera link is established.  Note:  driver does not check
    ''' for compatible subframe values when this value is set; rather they are checked
    ''' upon StartExposure.
    ''' </summary>
    ''' <value>BinX sets/gets the X binning value</value>
    ''' <exception cref="ASCOM.InvalidValueException">Must throw an exception for illegal binning values</exception>
    Property BinX() As Short

    ''' <summary>
    ''' Sets the binning factor for the Y axis  Also returns the current value.  Should
    ''' default to 1 when the camera link is established.  Note:  driver does not check
    ''' for compatible subframe values when this value is set; rather they are checked
    ''' upon StartExposure.
    ''' </summary>
    ''' <value>The bin Y.</value>
    ''' <exception cref="ASCOM.InvalidValueException">Must throw an exception for illegal binning values</exception>
    Property BinY() As Short

    ''' <summary>
    ''' Returns one of the following status information:
    ''' <list type="bullet">
    ''' 		<listheader>
    ''' 			<description>Value  State          Meaning</description>
    ''' 		</listheader>
    ''' 		<item>
    ''' 			<description>0      CameraIdle      At idle state, available to start exposure</description>
    ''' 		</item>
    ''' 		<item>
    ''' 			<description>1      CameraWaiting   Exposure started but waiting (for shutter, trigger,
    ''' filter wheel, etc.)</description>
    ''' 		</item>
    ''' 		<item>
    ''' 			<description>2      CameraExposing  Exposure currently in progress</description>
    ''' 		</item>
    ''' 		<item>
    ''' 			<description>3      CameraReading   CCD array is being read out (digitized)</description>
    ''' 		</item>
    ''' 		<item>
    ''' 			<description>4      CameraDownload  Downloading data to PC</description>
    ''' 		</item>
    ''' 		<item>
    ''' 			<description>5      CameraError     Camera error condition serious enough to prevent
    ''' further operations (link fail, etc.).</description>
    ''' 		</item>
    ''' 	</list>
    ''' </summary>
    ''' <value>The state of the camera.</value>
    ''' <exception cref="ASCOM.NotConnectedException">Must return an exception if the camera status is unavailable.</exception>
    ReadOnly Property CameraState() As CameraStates

    ''' <summary>
    ''' Returns the width of the CCD camera chip in unbinned pixels.
    ''' </summary>
    ''' <value>The size of the camera X.</value>
    ''' <exception cref="ASCOM.NotConnectedException">Must throw exception if the value is not known</exception>
    ReadOnly Property CameraXSize() As Integer

    ''' <summary>
    ''' Returns the height of the CCD camera chip in unbinned pixels.
    ''' </summary>
    ''' <value>The size of the camera Y.</value>
    ''' <exception cref="ASCOM.NotConnectedException">Must throw exception if the value is not known</exception>
    ReadOnly Property CameraYSize() As Integer

    ''' <summary>
    ''' Returns True if the camera can abort exposures; False if not.
    ''' </summary>
    ''' <value>
    ''' 	<c>true</c> if this instance can abort exposure; otherwise, <c>false</c>.
    ''' </value>
    ReadOnly Property CanAbortExposure() As Boolean

    ''' <summary>
    ''' If True, the camera can have different binning on the X and Y axes, as
    ''' determined by BinX and BinY. If False, the binning must be equal on the X and Y
    ''' axes.
    ''' </summary>
    ''' <value>
    ''' 	<c>true</c> if this instance can asymmetric bin; otherwise, <c>false</c>.
    ''' </value>
    ''' <exception cref="ASCOM.NotConnectedException">Must throw exception if the value is not known (n.b. normally only
    ''' occurs if no link established and camera must be queried)</exception>
    ReadOnly Property CanAsymmetricBin() As Boolean

    ''' <summary>
    ''' If True, the camera's cooler power setting can be read.
    ''' </summary>
    ''' <value>
    ''' 	<c>true</c> if this instance can get cooler power; otherwise, <c>false</c>.
    ''' </value>
    ReadOnly Property CanGetCoolerPower() As Boolean

    ''' <summary>
    ''' Returns True if the camera can send autoguider pulses to the telescope mount;
    ''' False if not.  (Note: this does not provide any indication of whether the
    ''' autoguider cable is actually connected.)
    ''' </summary>
    ''' <value>
    ''' 	<c>true</c> if this instance can pulse guide; otherwise, <c>false</c>.
    ''' </value>
    ReadOnly Property CanPulseGuide() As Boolean

    ''' <summary>
    ''' If True, the camera's cooler setpoint can be adjusted. If False, the camera
    ''' either uses open-loop cooling or does not have the ability to adjust temperature
    ''' from software, and setting the TemperatureSetpoint property has no effect.
    ''' </summary>
    ''' <value>
    ''' 	<c>true</c> if this instance can set CCD temperature; otherwise, <c>false</c>.
    ''' </value>
    ReadOnly Property CanSetCCDTemperature() As Boolean

    ''' <summary>
    ''' Some cameras support StopExposure, which allows the exposure to be terminated
    ''' before the exposure timer completes, but will still read out the image.  Returns
    ''' True if StopExposure is available, False if not.
    ''' </summary>
    ''' <value>
    ''' 	<c>true</c> if the camera can stop the exposure; otherwise, <c>false</c>.
    ''' </value>
    ''' <exception cref=" ASCOM.PropertyNotImplementedException">not supported</exception>
    ''' <exception cref=" ASCOM.NotConnectedException">an error condition such as link failure is present</exception>
    ReadOnly Property CanStopExposure() As Boolean

    ''' <summary>
    ''' Returns the current CCD temperature in degrees Celsius. Only valid if
    ''' CanControlTemperature is True.
    ''' </summary>
    ''' <value>The CCD temperature.</value>
    ''' <exception cref="ASCOM.InvalidValueException">Must throw exception if data unavailable.</exception>
    ReadOnly Property CCDTemperature() As Double

    ''' <summary>
    ''' Turns on and off the camera cooler, and returns the current on/off state.
    ''' Warning: turning the cooler off when the cooler is operating at high delta-T
    ''' (typically &gt;20C below ambient) may result in thermal shock.  Repeated thermal
    ''' shock may lead to damage to the sensor or cooler stack.  Please consult the
    ''' documentation supplied with the camera for further information.
    ''' </summary>
    ''' <value><c>true</c> if [cooler on]; otherwise, <c>false</c>.</value>
    ''' <exception cref=" ASCOM.PropertyNotImplementedException">not supported</exception>
    ''' <exception cref=" ASCOM.NotConnectedException">an error condition such as link failure is present</exception>
    Property CoolerOn() As Boolean

    ''' <summary>
    ''' Returns the present cooler power level, in percent.  Returns zero if CoolerOn is
    ''' False.
    ''' </summary>
    ''' <value>The cooler power.</value>
    ''' <exception cref=" ASCOM.PropertyNotImplementedException">not supported</exception>
    ''' <exception cref=" ASCOM.NotConnectedException">an error condition such as link failure is present</exception>
    ReadOnly Property CoolerPower() As Double

    ''' <summary>
    ''' Returns the gain of the camera in photoelectrons per A/D unit. (Some cameras have
    ''' multiple gain modes; these should be selected via the SetupDialog and thus are
    ''' static during a session.)
    ''' </summary>
    ''' <value>The electrons per ADU.</value>
    ''' <exception cref=" ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property ElectronsPerADU() As Double

    ''' <summary>
    ''' Reports the full well capacity of the camera in electrons, at the current camera
    ''' settings (binning, SetupDialog settings, etc.)
    ''' </summary>
    ''' <value>The full well capacity.</value>
    ''' <exception cref=" ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property FullWellCapacity() As Double

    ''' <summary>
    ''' If True, the camera has a mechanical shutter. If False, the camera does not have
    ''' a shutter.  If there is no shutter, the StartExposure command will ignore the
    ''' Light parameter.
    ''' </summary>
    ''' <value>
    ''' 	<c>true</c> if this instance has shutter; otherwise, <c>false</c>.
    ''' </value>
    ReadOnly Property HasShutter() As Boolean

    ''' <summary>
    ''' Returns the current heat sink temperature (called "ambient temperature" by some
    ''' manufacturers) in degrees Celsius. Only valid if CanControlTemperature is True.
    ''' </summary>
    ''' <value>The heat sink temperature.</value>
    ''' <exception cref=" ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property HeatSinkTemperature() As Double

    ''' <summary>
    ''' Returns a safearray of int of size NumX * NumY containing the pixel values from
    ''' the last exposure. The application must inspect the Safearray parameters to
    ''' determine the dimensions. Note: if NumX or NumY is changed after a call to
    ''' StartExposure it will have no effect on the size of this array. This is the
    ''' preferred method for programs (not scripts) to download iamges since it requires
    ''' much less memory.
    ''' For color or multispectral cameras, will produce an array of NumX * NumY *
    ''' NumPlanes.  If the application cannot handle multispectral images, it should use
    ''' just the first plane.
    ''' </summary>
    ''' <value>The image array.</value>
    ''' <exception cref=" ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property ImageArray() As Object

    ''' <summary>
    ''' Returns a safearray of Variant of size NumX * NumY containing the pixel values
    ''' from the last exposure. The application must inspect the Safearray parameters to
    ''' determine the dimensions. Note: if NumX or NumY is changed after a call to
    ''' StartExposure it will have no effect on the size of this array. This property
    ''' should only be used from scripts due to the extremely high memory utilization on
    ''' large image arrays (26 bytes per pixel). Pixels values should be in Short, int,
    ''' or Double format.
    ''' For color or multispectral cameras, will produce an array of NumX * NumY *
    ''' NumPlanes.  If the application cannot handle multispectral images, it should use
    ''' just the first plane.
    ''' </summary>
    ''' <value>The image array variant.</value>
    ''' <exception cref=" ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property ImageArrayVariant() As Object

    ''' <summary>
    ''' If True, there is an image from the camera available. If False, no image
    ''' is available and attempts to use the ImageArray method will produce an
    ''' exception.
    ''' </summary>
    ''' <value><c>true</c> if [image ready]; otherwise, <c>false</c>.</value>
    ''' <exception cref=" ASCOM.NotConnectedException">hardware or communications link error has occurred.</exception>
    ReadOnly Property ImageReady() As Boolean

    ''' <summary>
    ''' If True, pulse guiding is in progress. Required if the PulseGuide() method
    ''' (which is non-blocking) is implemented. See the PulseGuide() method.
    ''' </summary>
    ''' <value>
    ''' 	<c>true</c> if this instance is pulse guiding; otherwise, <c>false</c>.
    ''' </value>
    ''' <exception cref=" ASCOM.NotConnectedException">hardware or communications link error has occurred.</exception>
    ReadOnly Property IsPulseGuiding() As Boolean

    ''' <summary>
    ''' Reports the actual exposure duration in seconds (i.e. shutter open time).  This
    ''' may differ from the exposure time requested due to shutter latency, camera timing
    ''' precision, etc.
    ''' </summary>
    ''' <value>The last duration of the exposure.</value>
    ''' <exception cref="ASCOM.PropertyNotImplementedException">Must throw an exception if not supported</exception>
    ''' <exception cref="ASCOM.InvalidOperationException">If called before any exposure has been taken</exception>
    ReadOnly Property LastExposureDuration() As Double

    ''' <summary>
    ''' Reports the actual exposure start in the FITS-standard
    ''' CCYY-MM-DDThh:mm:ss[.sss...] format.
    ''' </summary>
    ''' <value>The last exposure start time.</value>
    ''' <exception cref="ASCOM.PropertyNotImplementedException">Must throw an exception if not supported</exception>
    ''' <exception cref="ASCOM.InvalidOperationException">If called before any exposure has been taken</exception>
    ReadOnly Property LastExposureStartTime() As String

    ''' <summary>
    ''' Reports the maximum ADU value the camera can produce.
    ''' </summary>
    ''' <value>The max ADU.</value>
    ''' <exception cref="ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property MaxADU() As Integer

    ''' <summary>
    ''' If AsymmetricBinning = False, returns the maximum allowed binning factor. If
    ''' AsymmetricBinning = True, returns the maximum allowed binning factor for the X
    ''' axis.
    ''' </summary>
    ''' <value>The max bin X.</value>
    ''' <exception cref="ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property MaxBinX() As Short

    ''' <summary>
    ''' If AsymmetricBinning = False, equals MaxBinX. If AsymmetricBinning = True,
    ''' returns the maximum allowed binning factor for the Y axis.
    ''' </summary>
    ''' <value>The max bin Y.</value>
    ''' <exception cref="ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property MaxBinY() As Short

    ''' <summary>
    ''' Sets the subframe width. Also returns the current value.  If binning is active,
    ''' value is in binned pixels.  No error check is performed when the value is set.
    ''' Should default to CameraXSize.
    ''' </summary>
    ''' <value>The num X.</value>
    Property NumX() As Integer

    ''' <summary>
    ''' Sets the subframe height. Also returns the current value.  If binning is active,
    ''' value is in binned pixels.  No error check is performed when the value is set.
    ''' Should default to CameraYSize.
    ''' </summary>
    ''' <value>The num Y.</value>
    Property NumY() As Integer

    ''' <summary>
    ''' Returns the width of the CCD chip pixels in microns, as provided by the camera
    ''' driver.
    ''' </summary>
    ''' <value>The pixel size X.</value>
    ''' <exception cref="ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property PixelSizeX() As Double

    ''' <summary>
    ''' Returns the height of the CCD chip pixels in microns, as provided by the camera
    ''' driver.
    ''' </summary>
    ''' <value>The pixel size Y.</value>
    ''' <exception cref="ASCOM.NotConnectedException">Must throw exception if data unavailable.</exception>
    ReadOnly Property PixelSizeY() As Double

    ''' <summary>
    ''' This method returns only after the move has completed.
    ''' symbolic Constants
    ''' The (symbolic) values for GuideDirections are:
    ''' Constant     Value      Description
    ''' --------     -----      -----------
    ''' guideNorth     0        North (+ declination/elevation)
    ''' guideSouth     1        South (- declination/elevation)
    ''' guideEast      2        East (+ right ascension/azimuth)
    ''' guideWest      3        West (+ right ascension/azimuth)
    ''' Note: directions are nominal and may depend on exact mount wiring.  guideNorth
    ''' must be opposite guideSouth, and guideEast must be opposite guideWest.
    ''' </summary>
    ''' <param name="Direction">The direction.</param>
    ''' <param name="Duration">The duration.</param>
    ''' <exception cref="ascom.MethodNotImplementedException">PulseGuide command is unsupported</exception>
    ''' <exception cref=" ASCOM.DriverException">PulseGuide command is unsuccessful</exception>
    Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Integer)

    ''' <summary>
    ''' Sets the camera cooler setpoint in degrees Celsius, and returns the current
    ''' setpoint.
    ''' Note:  camera hardware and/or driver should perform cooler ramping, to prevent
    ''' thermal shock and potential damage to the CCD array or cooler stack.
    ''' </summary>
    ''' <value>The set CCD temperature.</value>
    ''' <exception cref="ASCOM.DriverException">Must throw exception if command not successful.</exception>
    ''' <exception cref="ascom.PropertyNotImplementedException">Must throw exception if CanSetCCDTemperature is False.</exception>
    Property SetCCDTemperature() As Double

    ''' <summary>
    ''' Starts an exposure. Use ImageReady to check when the exposure is complete.
    ''' </summary>
    ''' <param name="Duration">Duration of exposure in seconds</param>
    ''' <param name="Light">True for light frame, False for dark frame (ignored if no shutter)</param>
    ''' <exception cref=" ASCOM.InvalidValueException">NumX, NumY, XBin, YBin, StartX, StartY, or Duration parameters are invalid.</exception>
    ''' <exception cref=" ASCOM.InvalidOperationException">CanAsymmetricBin is False and BinX != BinY</exception>
    ''' <exception cref="ASCOM.NotConnectedException">the exposure cannot be started for any reason, such as a hardware or communications error</exception>
    Sub StartExposure(ByVal Duration As Double, ByVal Light As Boolean)

    ''' <summary>
    ''' Sets the subframe start position for the X axis (0 based). Also returns the
    ''' current value.  If binning is active, value is in binned pixels.
    ''' </summary>
    ''' <value>The start X.</value>
    Property StartX() As Integer

    ''' <summary>
    ''' Sets the subframe start position for the Y axis (0 based). Also returns the
    ''' current value.  If binning is active, value is in binned pixels.
    ''' </summary>
    ''' <value>The start Y.</value>
    Property StartY() As Integer

    ''' <summary>
    ''' Stops the current exposure, if any.  If an exposure is in progress, the readout
    ''' process is initiated.  Ignored if readout is already in process.
    ''' </summary>
    ''' <exception cref="ASCOM.PropertyNotImplementedException">Must throw an exception if CanStopExposure is False</exception>
    ''' <exception cref="ASCOM.InvalidOperationException">Must throw an exception if no exposure is in progress</exception>
    ''' <exception cref="ASCOM.NotConnectedException">Must throw an exception if the camera or link has an error condition</exception>
    ''' <exception cref="ASCOM.DriverException">Must throw an exception if for any reason no image readout will be available.</exception>
    Sub StopExposure()
#End Region

#Region "ICameraV2 methods"
    'ICameraV2 members

    ''' <summary>
    ''' Bayer X offset index
    ''' </summary>
    ''' <returns>The Bayer colour matrix X offset.</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
    ''' <remarks>Returns the X offset of the Bayer matrix, as defined in Camera.SensorType. Value returned must be in the range 0 to M-1, 
    ''' where M is the width of the Bayer matrix. The offset is relative to the 0,0 pixel in the sensor array, and does not change to 
    ''' reflect subframe settings.</remarks>
    ReadOnly Property BayerOffsetX As Short

    ''' <summary>
    ''' Returns the Y offset of the Bayer matrix, as defined in Camera.SensorType.
    ''' </summary>
    ''' <returns>The Bayer colour matrix Y offset.</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
    ''' <remarks>The offset is relative to the 0,0 pixel in the sensor array, and does not change to reflect subframe settings. 
    ''' <para>It is recommended that this function be called only after a connection is established with the camera hardware, to ensure 
    ''' that the driver is aware of the capabilities of the specific camera model.</para></remarks>
    ReadOnly Property BayerOffsetY As Short

    ''' <summary>
    ''' Camera has a fast readout mode
    ''' </summary>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <returns>True when the camera supports a fast readout mode</returns>
    ''' <remarks>It is recommended that this function be called only after a connection is established with the camera hardware, to 
    ''' ensure that the driver is aware of the capabilities of the specific camera model.</remarks>
    ReadOnly Property CanFastReadout As Boolean

    ''' <summary>
    ''' Returns the maximum exposure time supported by Camera.StartExposure
    ''' </summary>
    ''' <returns>The maximum exposure time, in seconds, that the camera supports</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
    ''' <remarks>It is recommended that this function be called only after 
    ''' a connection is established with the camera hardware, to ensure that the driver is aware of the capabilities of the 
    ''' specific camera model.</remarks>
    ReadOnly Property ExposureMax As Double

    ''' <summary>
    ''' Minimium exposure time
    ''' </summary>
    ''' <returns>The minimum exposure time, in seconds, that the camera supports through Camera.StartExposure</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
    ''' <remarks>This must be a non-zero number representing the shortest possible exposure time supported by the camera model.
    ''' <para>Please note that for bias frame acquisition an even shorter exposure may be possible; please see Camera.StartExposure 
    ''' for more information.</para>
    ''' <para>It is recommended that this function be called only after a connection is established with the camera hardware, to ensure 
    ''' that the driver is aware of the capabilities of the specific camera model.</para></remarks>
    ReadOnly Property ExposureMin As Double

    ''' <summary>
    ''' Exposure resolution
    ''' </summary>
    ''' <returns>The smallest increment in exposure time supported by Camera.StartExposure.</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
    ''' <remarks>This can be used, for example, to specify the resolution of a user interface "spin control" used to dial in the exposure time.
    ''' <para>Please note that the Duration provided to Camera.StartExposure does not have to be an exact multiple of this number; 
    ''' the driver should choose the closest available value. Also in some cases the resolution may not be constant over the full range 
    ''' of exposure times; in this case the smallest increment would be appropriate. </para>
    ''' <para>It is recommended that this function be called only after a connection is established with the camera hardware, to ensure 
    ''' that the driver is aware of the capabilities of the specific camera model.</para></remarks>
    ReadOnly Property ExposureResolution As Double

    ''' <summary>
    ''' Fast readout mode
    ''' </summary>
    ''' <value>True sets fast readout mode, false sets normal mode</value>
    ''' <returns>True when the current readout mode is fast and false when the readout mode is normal.</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="PropertyNotImplementedException">Must throw an exception if Camera.CanFastReadout returns False.</exception>
    ''' <remarks>Must thrown an exception if no link is established to the camera. Must throw an exception if Camera.CanFastReadout 
    ''' returns False.
    ''' <para>Many cameras have a "fast mode" intended for use in focusing. When set to True, the camera will operate in Fast mode; when 
    ''' set False, the camera will operate normally. This property should default to False.</para>
    ''' <para>Please note that this function may in some cases interact with Camera.ReadoutModes; for example, there may be modes where 
    ''' the Fast/Normal switch is meaningless. In this case, it may be preferable to use the Camera.ReadoutModes function to control 
    ''' fast/normal switching.</para>
    ''' <para>If this feature is not available, then Camera.CanFastReadout must return False.</para></remarks>
    Property FastReadout As Boolean

    ''' <summary>
    ''' Index into the Gains array for the selected camera gain
    ''' </summary>
    ''' <value>Short integer index for the current camera gain in the gains string array.</value>
    ''' <returns>Index into the Gains array for the selected camera gain</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
    ''' <exception cref="PropertyNotImplementedException">Must throw an exception if gain is not supported</exception>
    ''' <remarks>
    ''' Camera.Gain can be used to adjust the gain setting of the camera, if supported. There are two typical usage scenarios:
    '''<ul>
    ''' <li>DSLR Cameras - Camera.Gains will return a 0-based array of strings, which correspond to different gain settings such as 
    ''' "ISO 800". Camera.Gain must be set to an integer in this range. Camera.GainMin and Camera.GainMax must thrown an exception if 
    ''' this mode is used.</li>
    ''' <li>Adjustable gain CCD cameras - Camera.GainMax and Camera.GainMin return integers, which specify the valid range for Camera.Gain.</li>
    ''' </ul>
    '''<para>The driver must default Camera.Gain to a valid value. </para>
    '''<para>Please note that Camera.ReadoutMode may in some cases affect the gain of the camera; if so the driver must be written such 
    ''' that the two properties do not conflict if both are used.</para></remarks>
    Property Gain As Short

    ''' <summary>
    ''' Maximum value of gain
    ''' </summary>
    ''' <value>Short integer representing the maximum gain value supported by the camera.</value>
    ''' <returns>The maximum gain value that this camera supports</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="PropertyNotImplementedException">Must throw an exception if gainmax is not supported</exception>
    ''' <remarks>When specifying the gain setting with an integer value, Camera.GainMax is used in conjunction with Camera.GainMin to 
    ''' specify the range of valid settings.
    ''' <para>Camera.GainMax shall be greater than Camera.GainMin. If either is available, then both must be available.</para>
    ''' <para>Please see Camera.Gain for more information.</para>
    ''' <para>It is recommended that this function be called only after a connection is established with the camera hardware, to ensure 
    ''' that the driver is aware of the capabilities of the specific camera model.</para></remarks>
    ReadOnly Property GainMax As Short

    ''' <summary>
    ''' Minimum value of gain
    ''' </summary>
    ''' <returns>The minimum gain value that this camera supports</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
    ''' <remarks>When specifying the gain setting with an integer value, Camera.GainMin is used in conjunction with Camera.GainMax to 
    ''' specify the range of valid settings.
    ''' <para>Camera.GainMax shall be greater than Camera.GainMin. If either is available, then both must be available.</para>
    ''' <para>Please see Camera.Gain for more information.</para>
    ''' <para>It is recommended that this function be called only after a connection is established with the camera hardware, to ensure 
    ''' that the driver is aware of the capabilities of the specific camera model.</para></remarks>

    ReadOnly Property GainMin As Short

    ''' <summary>
    ''' Gains supported by the camera
    ''' </summary>
    ''' <returns>String array of gain names </returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <exception cref="PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
    ''' <remarks>Gains provides a 0-based array of available gain settings.  This is often used to specify ISO settings for DSLR cameras.  
    ''' Typically the application software will display the available gain settings in a drop list. The application will then supply 
    ''' the selected index to the driver via the Camera.Gain property. 
    ''' <para>The Gain setting may alternatively be specified using integer values; if this mode is used then Gains is invalid 
    ''' and must throw an exception. Please see Camera.GainMax and Camera.GainMin for more information.</para>
    ''' <para>It is recommended that this function be called only after a connection is established with the camera hardware, 
    ''' to ensure that the driver is aware of the capabilities of the specific camera model.</para></remarks>
    ReadOnly Property Gains As String()

    ''' <summary>
    ''' Percent conpleted
    ''' </summary>
    ''' <returns>A value between 0 and 100% indicating the completeness of this operation</returns>
    ''' <exception cref="InvalidOperationException">Thrown when it is inappropriate to call Camera.percentCOmpleted</exception>
    ''' <remarks>If valid, returns an integer between 0 and 100, where 0 indicates 0% progress (function just started) and 
    ''' 100 indicates 100% progress (i.e. completion).
    ''' <para>At the discretion of the driver author, Camera.PercentCompleted may optionally be valid when <see cref="CameraState">Camera.CameraState</see> is 
    ''' in any or all of the following states: <see cref="CameraStates.cameraExposing">CameraExposing</see>, 
    ''' <see cref="CameraStates.cameraExposing">CameraWaiting</see>, <see cref="CameraStates.cameraExposing">CameraReading</see>, 
    ''' or <see cref="CameraStates.cameraExposing">CameraDownload</see>. In all other states an exception shall be thrown.</para>
    ''' <para>Typically the application user interface will show a progress bar based on the Camera.PercentCompleted value.</para>
    ''' <para>Please note that client applications are not required to use this value, and in some cases may display status 
    ''' information based on other information, such as time elapsed.</para>
    ''' </remarks>
    ReadOnly Property PercentCompleted As Short

    ''' <summary>
    ''' Readout mode
    ''' </summary>
    ''' <value></value>
    ''' <returns>Short integer index into the <see cref="ReadoutModes">ReadoutModes</see> array of string readout mode names indicating 
    ''' the camera's current readout mode.</returns>
    ''' <exception cref="InvalidValueException">Must throw an exception if set to an illegal or unavailable mode.</exception>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <remarks>ReadoutMode is an index into the array <see cref="ReadoutModes">Camera.ReadoutModes</see>, and selects the desired readout mode for the camera.  
    ''' Defaults to 0 if not set.  Throws an exception if the selected mode is not available.
    ''' <para>It is strongly recommended, but not required, that driver authors make the 0-index mode suitable for standard imaging operations, 
    ''' since it is the default.</para>
    ''' <para>Please see <see cref="ReadoutModes">Camera.ReadoutModes</see> for additional information.</para></remarks>
    ReadOnly Property ReadoutMode As Short

    ''' <summary>
    ''' List of available readout modes
    ''' </summary>
    ''' <returns>A string array of readout mode names</returns>
    ''' <exception cref="PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <remarks>This property provides an array of strings, each of which describes an available readout mode of the camera.  
    ''' At least two strings must be present in the list. The user interface of a control application will typically present to the 
    ''' user a drop-list of modes.  The choice of available modes made available is entirely at the discretion of the driver author. 
    ''' Please note that if the camera has many different modes of operation, then the most commonly adjusted settings should be in 
    ''' ReadoutModes; additional settings may be provided using <see cref="SetupDialog">Camera.SetupDialog</see>.
    ''' <para>To select a mode, the application will set <see cref="ReadoutMode">Camera.ReadoutMode</see> to index of the desired mode.  The index is zero-based.</para>
    ''' <para>This property should only be read while a connection to the camera is actually established.  Drivers often support 
    ''' multiple cameras with different capabilities, which are not known until the connection is made.  If the available readout modes 
    ''' are not known because no connection has been established, this property shall throw an exception.</para>
    ''' <para>Please note that the default Camera.ReadoutMode setting is 0. It is strongly recommended, but not required, that 
    ''' driver authors make the 0-index mode suitable for standard imaging operations, since it is the default.</para>
    ''' <para>This feature may be used in parallel with <see cref="FastReadout">Camera.FastReadout</see>; however, care should be taken to ensure that the two 
    ''' features work together consistently. If there are modes that are inconsistent having a separate fast/normal switch, then it 
    ''' may be better to simply list Fast as one of the ReadoutModes.</para><para>It is recommended that this function be called 
    ''' only after a connection is established with the camera hardware, to ensure that the driver is aware of the capabilities 
    ''' of the specific camera model.</para></remarks>
    ReadOnly Property ReadoutModes As String()

    ''' <summary>
    ''' Sensor name
    ''' </summary>
    ''' <returns>The name of sensor used within the camera</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <remarks>Returns the name (datasheet part number) of the sensor, e.g. ICX285AL.  The format is to be exactly as shown on 
    ''' manufacturer data sheet, subject to the following rules. All letter shall be uppercase.  Spaces shall not be included.
    ''' <para>Any extra suffixes that define region codes, package types, temperature range, coatings, grading, color/monochrome, 
    ''' etc. shall not be included. For color sensors, if a suffix differentiates different Bayer matrix encodings, it shall be 
    ''' included.</para>
    ''' <para>Examples:</para>
    ''' <ul>
    ''' <li>ICX285AL-F shall be reported as ICX285</li>
    ''' <li>KAF-8300-AXC-CD-AA shall be reported as KAF-8300</li>
    ''' </ul>
    ''' <para>Note:  The most common usage of this property is to select approximate color balance parameters to be applied to 
    ''' the Bayer matrix of one-shot color sensors.  Application authors should assume that an appropriate IR cutoff filter is 
    ''' in place for color sensors.</para><para>It is recommended that this function be called only after a connection is established with 
    ''' the camera hardware, to ensure that the driver is aware of the capabilities of the specific camera model.</para>
    ''' </remarks>
    ReadOnly Property SensorName As String

    ''' <summary>
    ''' Sensor type
    ''' </summary>
    ''' <value></value>
    ''' <returns>The SensorType enum value of the camera sensor</returns>
    ''' <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
    ''' active connection in order to retrieve necessary information from the camera.)</exception>
    ''' <remarks>
    ''' SensorType returns a value indicating whether the sensor is monochrome, or what Bayer matrix it encodes.  The following values are defined:
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
    ''' <para>SensorType can possibly change between exposures, for example if <see cref="ReadoutMode">Camera.ReadoutMode</see> is changed, and should always be checked after each exposure.</para>
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
    ''' <para>The alignment of the array may be modified by <see cref="BayerOffsetX">Camera.BayerOffsetX</see> and <see cref="BayerOffsetY">Camera.BayerOffsetY</see>. The offset is measured from the 
    ''' 0,0 position in the sensor array to the upper left corner of the Bayer matrix table. Please note that the Bayer offset values 
    ''' are not affected by subframe settings.</para>
    ''' <para>For example, if a CMYG2 sensor has a Bayer matrix offset as shown below, BayerOffsetX is 0 and BayerOffsetY is 1:</para>
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
    ''' <para>It is recommended that this function be called only after a connection is established with the camera hardware, to ensure that 
    ''' the driver is aware of the capabilities of the specific camera model.</para>
    ''' </remarks>
    ReadOnly Property SensorType As SensorType
#End Region

End Interface
