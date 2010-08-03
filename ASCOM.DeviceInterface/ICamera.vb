Imports System.Runtime.InteropServices

'-----------------------------------------------------------------------
' <summary>Defines the ICamera Interface</summary>
'-----------------------------------------------------------------------
''' <summary>
''' Defines the ICamera Interface
''' </summary>
<Guid("972CEBC6-0EBE-4efc-99DD-CC5293FDE77B"), ComVisible(True)> _
Public Interface ICamera 'D95FBC6E-0705-458B-84C0-57E3295DBCCE
    Inherits IAscomDriver
    Inherits IDeviceControl
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
    ''' <exception cref="System.Exception">Must return an exception if the camera status is unavailable.</exception>
    ReadOnly Property CameraState() As CameraStates

    ''' <summary>
    ''' Returns the width of the CCD camera chip in unbinned pixels.
    ''' </summary>
    ''' <value>The size of the camera X.</value>
    ''' <exception cref="System.Exception">Must throw exception if the value is not known</exception>
    ReadOnly Property CameraXSize() As Integer

    ''' <summary>
    ''' Returns the height of the CCD camera chip in unbinned pixels.
    ''' </summary>
    ''' <value>The size of the camera Y.</value>
    ''' <exception cref="System.Exception">Must throw exception if the value is not known</exception>
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
    ''' <exception cref="System.Exception">Must throw exception if the value is not known (n.b. normally only
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
    ''' <exception cref=" System.Exception">not supported</exception>
    ''' <exception cref=" System.Exception">an error condition such as link failure is present</exception>
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
    ''' <exception cref=" System.Exception">not supported</exception>
    ''' <exception cref=" System.Exception">an error condition such as link failure is present</exception>
    Property CoolerOn() As Boolean

    ''' <summary>
    ''' Returns the present cooler power level, in percent.  Returns zero if CoolerOn is
    ''' False.
    ''' </summary>
    ''' <value>The cooler power.</value>
    ''' <exception cref=" System.Exception">not supported</exception>
    ''' <exception cref=" System.Exception">an error condition such as link failure is present</exception>
    ReadOnly Property CoolerPower() As Double

    ''' <summary>
    ''' Dispose the late-bound interface, if needed. Will release it via COM
    ''' if it is a COM object, else if native .NET will just dereference it
    ''' for GC.
    ''' </summary>
    Sub Dispose()

    ''' <summary>
    ''' Returns the gain of the camera in photoelectrons per A/D unit. (Some cameras have
    ''' multiple gain modes; these should be selected via the SetupDialog and thus are
    ''' static during a session.)
    ''' </summary>
    ''' <value>The electrons per ADU.</value>
    ''' <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
    ReadOnly Property ElectronsPerADU() As Double

    ''' <summary>
    ''' Reports the full well capacity of the camera in electrons, at the current camera
    ''' settings (binning, SetupDialog settings, etc.)
    ''' </summary>
    ''' <value>The full well capacity.</value>
    ''' <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
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
    ''' <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
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
    ''' <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
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
    ''' <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
    ReadOnly Property ImageArrayVariant() As Object

    ''' <summary>
    ''' If True, there is an image from the camera available. If False, no image
    ''' is available and attempts to use the ImageArray method will produce an
    ''' exception.
    ''' </summary>
    ''' <value><c>true</c> if [image ready]; otherwise, <c>false</c>.</value>
    ''' <exception cref=" System.Exception">hardware or communications link error has occurred.</exception>
    ReadOnly Property ImageReady() As Boolean

    ''' <summary>
    ''' If True, pulse guiding is in progress. Required if the PulseGuide() method
    ''' (which is non-blocking) is implemented. See the PulseGuide() method.
    ''' </summary>
    ''' <value>
    ''' 	<c>true</c> if this instance is pulse guiding; otherwise, <c>false</c>.
    ''' </value>
    ''' <exception cref=" System.Exception">hardware or communications link error has occurred.</exception>
    ReadOnly Property IsPulseGuiding() As Boolean

    ''' <summary>
    ''' Reports the last error condition reported by the camera hardware or communications
    ''' link.  The string may contain a text message or simply an error code.  The error
    ''' value is cleared the next time any method is called.
    ''' </summary>
    ''' <value>The last error.</value>
    ''' <exception cref=" System.Exception">Must throw exception if no error condition.</exception>
    ReadOnly Property LastError() As String


    ''' <summary>
    ''' Reports the actual exposure duration in seconds (i.e. shutter open time).  This
    ''' may differ from the exposure time requested due to shutter latency, camera timing
    ''' precision, etc.
    ''' </summary>
    ''' <value>The last duration of the exposure.</value>
    ''' <exception cref=" System.Exception">Must throw exception if not supported or no exposure has been taken</exception>
    ReadOnly Property LastExposureDuration() As Double

    ''' <summary>
    ''' Reports the actual exposure start in the FITS-standard
    ''' CCYY-MM-DDThh:mm:ss[.sss...] format.
    ''' </summary>
    ''' <value>The last exposure start time.</value>
    ''' <exception cref=" System.Exception">Must throw exception if not supported or no exposure has been taken</exception>
    ReadOnly Property LastExposureStartTime() As String

    ''' <summary>
    ''' Reports the maximum ADU value the camera can produce.
    ''' </summary>
    ''' <value>The max ADU.</value>
    ''' <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
    ReadOnly Property MaxADU() As Integer

    ''' <summary>
    ''' If AsymmetricBinning = False, returns the maximum allowed binning factor. If
    ''' AsymmetricBinning = True, returns the maximum allowed binning factor for the X
    ''' axis.
    ''' </summary>
    ''' <value>The max bin X.</value>
    ''' <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
    ReadOnly Property MaxBinX() As Short

    ''' <summary>
    ''' If AsymmetricBinning = False, equals MaxBinX. If AsymmetricBinning = True,
    ''' returns the maximum allowed binning factor for the Y axis.
    ''' </summary>
    ''' <value>The max bin Y.</value>
    ''' <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
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
    ''' <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
    ReadOnly Property PixelSizeX() As Double

    ''' <summary>
    ''' Returns the height of the CCD chip pixels in microns, as provided by the camera
    ''' driver.
    ''' </summary>
    ''' <value>The pixel size Y.</value>
    ''' <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
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
    ''' <exception cref=" System.Exception">PulseGuide command is unsupported</exception>
    ''' <exception cref=" System.Exception">PulseGuide command is unsuccessful</exception>
    Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Integer)

    ''' <summary>
    ''' Sets the camera cooler setpoint in degrees Celsius, and returns the current
    ''' setpoint.
    ''' Note:  camera hardware and/or driver should perform cooler ramping, to prevent
    ''' thermal shock and potential damage to the CCD array or cooler stack.
    ''' </summary>
    ''' <value>The set CCD temperature.</value>
    ''' <exception cref=" System.Exception">Must throw exception if command not successful.</exception>
    ''' <exception cref=" System.Exception">Must throw exception if CanSetCCDTemperature is False.</exception>
    Property SetCCDTemperature() As Double

    ''' <summary>
    ''' Starts an exposure. Use ImageReady to check when the exposure is complete.
    ''' </summary>
    ''' <param name="Duration">Duration of exposure in seconds</param>
    ''' <param name="Light">True for light frame, False for dark frame (ignored if no shutter)</param>
    ''' <exception cref=" System.Exception">NumX, NumY, XBin, YBin, StartX, StartY, or Duration parameters are invalid.</exception>
    ''' <exception cref=" System.Exception">CanAsymmetricBin is False and BinX != BinY</exception>
    ''' <exception cref=" System.Exception">the exposure cannot be started for any reason, such as a hardware or communications error</exception>
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
    ''' <exception cref=" System.Exception">Must throw an exception if CanStopExposure is False</exception>
    ''' <exception cref=" System.Exception">Must throw an exception if no exposure is in progress</exception>
    ''' <exception cref=" System.Exception">Must throw an exception if the camera or link has an error condition</exception>
    ''' <exception cref=" System.Exception">Must throw an exception if for any reason no image readout will be available.</exception>
    Sub StopExposure()

    'ICameraV2 members

    ''' <summary>
    ''' Bayer X offset index
    ''' </summary>
    ''' <value>Sets the X offset of a Bayer colour matrix in pixels</value>
    ''' <returns>The Bayer colour matrix X offset.</returns>
    ''' <remarks></remarks>
    ReadOnly Property BayerOffsetX As Short

    ''' <summary>
    ''' Bayer Y offset index
    ''' </summary>
    ''' <value>Sets the Y offset of a Bayer colour matrix in pixels</value>
    ''' <returns>The Bayer colour matrix X offset.</returns>
    ''' <remarks></remarks>
    ReadOnly Property BayerOffsetY As Short

    ''' <summary>
    ''' Camera has a fast readout mode
    ''' </summary>
    ''' <value>Boolean true / false</value>
    ''' <returns>True when the camera supports a fast readout mode</returns>
    ''' <remarks></remarks>
    ReadOnly Property CanFastReadout As Boolean

    ''' <summary>
    ''' Maximum exposure time
    ''' </summary>
    ''' <value>Double value in seconds</value>
    ''' <returns>The maximum exposure time, in seconds, that the camera supports</returns>
    ''' <remarks></remarks>
    ReadOnly Property ExposureMax As Double

    ''' <summary>
    ''' Minimium exposure time
    ''' </summary>
    ''' <value>Double value in seconds</value>
    ''' <returns>The minimum exposure time, in seconds, that the camera supports</returns>
    ''' <remarks></remarks>
    ReadOnly Property ExposureMin As Double

    ''' <summary>
    ''' Exposure resolution
    ''' </summary>
    ''' <value>Double value in seconds</value>
    ''' <returns>The smallest increment in exposure time that the camera recognises</returns>
    ''' <remarks></remarks>
    ReadOnly Property ExposureResolution As Double

    ''' <summary>
    ''' Fast readout mode
    ''' </summary>
    ''' <value>True sets fast readout mode, false sets normal mode</value>
    ''' <returns>True when the current readout mode is fast and false when the readout mode is normal.</returns>
    ''' <remarks></remarks>
    Property FastReadout As Boolean

    ''' <summary>
    ''' Gain - Needs explanation!!
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Gain As Short

    ''' <summary>
    ''' Maximum value of gain
    ''' </summary>
    ''' <value></value>
    ''' <returns>The maximum gain value that this camera supports</returns>
    ''' <remarks></remarks>
    ReadOnly Property GainMax As Short

    ''' <summary>
    ''' Minimum value of gain
    ''' </summary>
    ''' <value></value>
    ''' <returns>The minimum gain value that this camera supports</returns>
    ''' <remarks></remarks>
    ReadOnly Property GainMin As Short

    ''' <summary>
    ''' Gains supported by the camera
    ''' </summary>
    ''' <value></value>
    ''' <returns>String array of gain names </returns>
    ''' <remarks></remarks>
    ReadOnly Property Gains As String()

    ''' <summary>
    ''' Percent conpleted
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value between 0 and 100% indicating the completeness of this operation</returns>
    ''' <remarks></remarks>
    ReadOnly Property PercentCompleted As Short

    ''' <summary>
    ''' Readout mode
    ''' </summary>
    ''' <value></value>
    ''' <returns>Short integer index into the ReadoutModes array of string readout mode names indicating the camera's current readout mode.</returns>
    ''' <remarks></remarks>
    ReadOnly Property ReadoutMode As Short

    ''' <summary>
    ''' List of available readout modes
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string array of readout mode names</returns>
    ''' <remarks></remarks>
    ReadOnly Property ReadoutModes As String()

    ''' <summary>
    ''' Sensor name
    ''' </summary>
    ''' <value></value>
    ''' <returns>The name of sensor used within the camera</returns>
    ''' <remarks></remarks>
    ReadOnly Property SensorName As String

    ''' <summary>
    ''' Sensor type
    ''' </summary>
    ''' <value></value>
    ''' <returns>The SensorType enum value of the camera sensor</returns>
    ''' <remarks></remarks>
    ReadOnly Property SensorType As SensorType

End Interface
