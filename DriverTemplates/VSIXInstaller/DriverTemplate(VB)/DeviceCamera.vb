' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports System.Collections
Imports ASCOM
Imports ASCOM.Utilities

Class DeviceCamera
    Implements ICameraV3
    Private m_util As New Util()
    Private TL As New TraceLogger()

#Region "ICamera Implementation"

    Private Const ccdWidth As Integer = 1394 ' Constants to define the CCD pixel dimensions
    Private Const ccdHeight As Integer = 1040
    Private Const pixelSize As Double = 6.45 ' Constant for the pixel physical dimension

    Private cameraNumX As Integer = ccdWidth ' Initialise variables to hold values required for functionality tested by Conform
    Private cameraNumY As Integer = ccdHeight
    Private cameraStartX As Integer = 0
    Private cameraStartY As Integer = 0
    Private exposureStart As DateTime = DateTime.MinValue
    Private cameraLastExposureDuration As Double = 0.0
    Private cameraImageReady As Boolean = False
    Private cameraImageArray As Integer(,)
    Private cameraImageArrayVariant As Object(,)

	''' <summary>
	''' Aborts the current exposure, if any, And returns the camera to Idle state.
	''' </summary>
	Public Sub AbortExposure() Implements ICameraV3.AbortExposure
		TL.LogMessage("AbortExposure", "Not implemented")
		Throw New MethodNotImplementedException("AbortExposure")
	End Sub

	''' <summary>
	''' Returns the X offset of the Bayer matrix, as defined in <see cref="SensorType" />.
	''' </summary>
	''' <returns>The Bayer colour matrix X offset, as defined in <see cref="SensorType" />.</returns>
	Public ReadOnly Property BayerOffsetX() As Short Implements ICameraV3.BayerOffsetX
		Get
			TL.LogMessage("BayerOffsetX Get", "Not implemented")
			Throw New PropertyNotImplementedException("BayerOffsetX", False)
		End Get
	End Property

	''' <summary>
	''' Returns the Y offset of the Bayer matrix, as defined in <see cref="SensorType" />.
	''' </summary>
	''' <returns>The Bayer colour matrix Y offset, as defined in <see cref="SensorType" />.</returns>
	Public ReadOnly Property BayerOffsetY() As Short Implements ICameraV3.BayerOffsetY
		Get
			TL.LogMessage("BayerOffsetY Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("BayerOffsetY", False)
		End Get
	End Property

	''' <summary>
	''' Sets the binning factor for the X axis, also returns the current value.
	''' </summary>
	''' <value>The X binning value</value>
	Public Property BinX() As Short Implements ICameraV3.BinX
		Get
			TL.LogMessage("BinX Get", "1")
			Return 1
		End Get
		Set(value As Short)
			TL.LogMessage("BinX Set", value.ToString())
			If (Not (value = 1)) Then
				TL.LogMessage("BinX Set", "Value out of range, throwing InvalidValueException")
				Throw New ASCOM.InvalidValueException("BinX", value.ToString(), "1") ' Only 1 is valid in this simple template
			End If
		End Set
	End Property

	''' <summary>
	''' Sets the binning factor for the Y axis, also returns the current value.
	''' </summary>
	''' <value>The Y binning value.</value>
	Public Property BinY() As Short Implements ICameraV3.BinY
		Get
			TL.LogMessage("BinY Get", "1")
			Return 1
		End Get
		Set(value As Short)
			TL.LogMessage("BinY Set", value.ToString())
			If (Not (value = 1)) Then
				TL.LogMessage("BinX Set", "Value out of range, throwing InvalidValueException")
				Throw New ASCOM.InvalidValueException("BinY", value.ToString(), "1") ' Only 1 is valid in this simple template
			End If
		End Set
	End Property

	''' <summary>
	''' Returns the current CCD temperature in degrees Celsius.
	''' </summary>
	''' <value>The CCD temperature.</value>
	Public ReadOnly Property CCDTemperature() As Double Implements ICameraV3.CCDTemperature
		Get
			TL.LogMessage("CCDTemperature Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("CCDTemperature", False)
		End Get
	End Property

	''' <summary>
	''' Returns the current camera operational state
	''' </summary>
	''' <value>The state of the camera.</value>
	Public ReadOnly Property CameraState() As CameraStates Implements ICameraV3.CameraState
		Get
			TL.LogMessage("CameraState Get", CameraStates.cameraIdle.ToString())
			Return CameraStates.cameraIdle
		End Get
	End Property

	''' <summary>
	''' Returns the width of the CCD camera chip in unbinned pixels.
	''' </summary>
	''' <value>The size of the camera X.</value>
	Public ReadOnly Property CameraXSize() As Integer Implements ICameraV3.CameraXSize
		Get
			TL.LogMessage("CameraXSize Get", ccdWidth.ToString())
			Return ccdWidth
		End Get
	End Property

	''' <summary>
	''' Returns the height of the CCD camera chip in unbinned pixels.
	''' </summary>
	''' <value>The size of the camera Y.</value>
	Public ReadOnly Property CameraYSize() As Integer Implements ICameraV3.CameraYSize
		Get
			TL.LogMessage("CameraYSize Get", ccdHeight.ToString())
			Return ccdHeight
		End Get
	End Property

	''' <summary>
	''' Returns <c>True</c> if the camera can abort exposures; <c>False</c> if Not.
	''' </summary>
	''' <returns><c>True</c> when the camera can abort an exposure</returns>
	Public ReadOnly Property CanAbortExposure() As Boolean Implements ICameraV3.CanAbortExposure
		Get
			TL.LogMessage("CanAbortExposure Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' Returns a flag showing whether this camera supports asymmetric binning
	''' </summary>
	''' <value>
	''' <c>True</c> if this instance can asymmetric bin; otherwise, <c>False</c>.
	''' </value>
	Public ReadOnly Property CanAsymmetricBin() As Boolean Implements ICameraV3.CanAsymmetricBin
		Get
			TL.LogMessage("CanAsymmetricBin Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' Camera has a fast readout mode
	''' </summary>
	''' <returns><c>True</c> when the camera supports a fast readout mode</returns>
	Public ReadOnly Property CanFastReadout() As Boolean Implements ICameraV3.CanFastReadout
		Get
			TL.LogMessage("CanFastReadout Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' If <c>True</c>, the camera's cooler power setting can be read.
	''' </summary>
	''' <value>
	''' <c>True</c> if this instance can get cooler power; otherwise, <c>False</c>.
	''' </value>
	Public ReadOnly Property CanGetCoolerPower() As Boolean Implements ICameraV3.CanGetCoolerPower
		Get
			TL.LogMessage("CanGetCoolerPower Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' Returns a flag indicating whether this camera supports pulse guiding
	''' </summary>
	''' <value>
	''' <c>True</c> if this instance can pulse guide; otherwise, <c>False</c>.
	''' </value>
	Public ReadOnly Property CanPulseGuide() As Boolean Implements ICameraV3.CanPulseGuide
		Get
			TL.LogMessage("CanPulseGuide Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' Returns a flag indicating whether this camera supports setting the CCD temperature
	''' </summary>
	''' <value>
	''' <c>True</c> if this instance can set CCD temperature; otherwise, <c>False</c>.
	''' </value>
	Public ReadOnly Property CanSetCCDTemperature() As Boolean Implements ICameraV3.CanSetCCDTemperature
		Get
			TL.LogMessage("CanSetCCDTemperature Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' Returns a flag indicating whether this camera can stop an exposure that Is in progress
	''' </summary>
	''' <value>
	''' <c>True</c> if the camera can stop the exposure; otherwise, <c>False</c>.
	''' </value>
	Public ReadOnly Property CanStopExposure() As Boolean Implements ICameraV3.CanStopExposure
		Get
			TL.LogMessage("CanStopExposure Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' Turns on and off the camera cooler, And returns the current on/off state.
	''' </summary>
	''' <value><c>True</c> if the cooler Is on; otherwise, <c>False</c>.</value>
	Public Property CoolerOn() As Boolean Implements ICameraV3.CoolerOn
		Get
			TL.LogMessage("CoolerOn Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("CoolerOn", False)
		End Get
		Set(value As Boolean)
			TL.LogMessage("CoolerOn Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("CoolerOn", True)
		End Set
	End Property

	''' <summary>
	''' Returns the present cooler power level, in percent.
	''' </summary>
	''' <value>The cooler power.</value>
	Public ReadOnly Property CoolerPower() As Double Implements ICameraV3.CoolerPower
		Get
			TL.LogMessage("AbortExposure Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("CoolerPower", False)
		End Get
	End Property

	''' <summary>
	''' Returns the gain of the camera in photoelectrons per A/D unit.
	''' </summary>
	''' <value>The electrons per ADU.</value>
	Public ReadOnly Property ElectronsPerADU() As Double Implements ICameraV3.ElectronsPerADU
		Get
			TL.LogMessage("ElectronsPerADU Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("ElectronsPerADU", False)
		End Get
	End Property

	''' <summary>
	''' Returns the maximum exposure time supported by <see cref="StartExposure">StartExposure</see>.
	''' </summary>
	''' <returns>The maximum exposure time, in seconds, that the camera supports</returns>
	Public ReadOnly Property ExposureMax() As Double Implements ICameraV3.ExposureMax
		Get
			TL.LogMessage("ExposureMax Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("ExposureMax", False)
		End Get
	End Property

	''' <summary>
	''' Minimum exposure time
	''' </summary>
	''' <returns>The minimum exposure time, in seconds, that the camera supports through <see cref="StartExposure">StartExposure</see></returns>
	Public ReadOnly Property ExposureMin() As Double Implements ICameraV3.ExposureMin
		Get
			TL.LogMessage("ExposureMin Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("ExposureMin", False)
		End Get
	End Property

	''' <summary>
	''' Exposure resolution
	''' </summary>
	''' <returns>The smallest increment in exposure time supported by <see cref="StartExposure">StartExposure</see>.</returns>
	Public ReadOnly Property ExposureResolution() As Double Implements ICameraV3.ExposureResolution
		Get
			TL.LogMessage("ExposureResolution Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("ExposureResolution", False)
		End Get
	End Property

	''' <summary>
	''' Gets Or sets Fast Readout Mode
	''' </summary>
	''' <value><c>True</c> for fast readout mode, <c>False</c> for normal mode</value>
	Public Property FastReadout() As Boolean Implements ICameraV3.FastReadout
		Get
			TL.LogMessage("FastReadout Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("FastReadout", False)
		End Get
		Set(value As Boolean)
			TL.LogMessage("FastReadout Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("FastReadout", True)
		End Set
	End Property

	''' <summary>
	''' Reports the full well capacity of the camera in electrons, at the current camera settings (binning, SetupDialog settings, etc.)
	''' </summary>
	''' <value>The full well capacity.</value>
	Public ReadOnly Property FullWellCapacity() As Double Implements ICameraV3.FullWellCapacity
		Get
			TL.LogMessage("FullWellCapacity Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("FullWellCapacity", False)
		End Get
	End Property

	''' <summary>
	''' The camera's gain (GAIN VALUE MODE) OR the index of the selected camera gain description in the <see cref="Gains" /> array (GAINS INDEX MODE)
	''' </summary>
	''' <returns><para><b> GAIN VALUE MODE:</b> The current gain value.</para>
	''' <p style="color:red"><b>Or</b></p>
	''' <b>GAINS INDEX MODE:</b> Index into the Gains array for the current camera gain
	''' </returns>
	Public Property Gain() As Short Implements ICameraV3.Gain
		Get
			TL.LogMessage("Gain Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Gain", False)
		End Get
		Set(value As Short)
			TL.LogMessage("Gain Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Gain", True)
		End Set
	End Property

	''' <summary>
	''' Maximum <see cref="Gain" /> value of that this camera supports
	''' </summary>
	''' <returns>The maximum gain value that this camera supports</returns>
	Public ReadOnly Property GainMax() As Short Implements ICameraV3.GainMax
		Get
			TL.LogMessage("GainMax Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("GainMax", False)
		End Get
	End Property

	''' <summary>
	''' Minimum <see cref="Gain" /> value of that this camera supports
	''' </summary>
	''' <returns>The minimum gain value that this camera supports</returns>
	Public ReadOnly Property GainMin() As Short Implements ICameraV3.GainMin
		Get
			TL.LogMessage("GainMin Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("GainMin", False)
		End Get
	End Property

	''' <summary>
	''' List of Gain names supported by the camera
	''' </summary>
	''' <returns>The list of supported gain names as an ArrayList of strings</returns>
	Public ReadOnly Property Gains() As ArrayList Implements ICameraV3.Gains
		Get
			TL.LogMessage("Gains Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Gains", False)
		End Get
	End Property

	''' <summary>
	''' Returns a flag indicating whether this camera has a mechanical shutter
	''' </summary>
	''' <value>
	''' <c>True</c> if this instance has shutter; otherwise, <c>False</c>.
	''' </value>
	Public ReadOnly Property HasShutter() As Boolean Implements ICameraV3.HasShutter
		Get
			TL.LogMessage("HasShutter Get", False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' Returns the current heat sink temperature (called "ambient temperature" by some manufacturers) in degrees Celsius.
	''' </summary>
	''' <value>The heat sink temperature.</value>
	Public ReadOnly Property HeatSinkTemperature() As Double Implements ICameraV3.HeatSinkTemperature
		Get
			TL.LogMessage("HeatSinkTemperature Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("HeatSinkTemperature", False)
		End Get
	End Property

	''' <summary>
	''' Returns a safearray of int of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure.
	''' </summary>
	''' <value>The image array.</value>
	Public ReadOnly Property ImageArray() As Object Implements ICameraV3.ImageArray
		Get
			If (Not cameraImageReady) Then
				TL.LogMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken!")
				Throw New ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken!")
			End If

			ReDim cameraImageArray(cameraNumX - 1, cameraNumY - 1)
			Return cameraImageArray
		End Get
	End Property

	''' <summary>
	''' Returns a safearray of Variant of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure.
	''' </summary>
	''' <value>The image array variant.</value>
	Public ReadOnly Property ImageArrayVariant() As Object Implements ICameraV3.ImageArrayVariant
		Get
			If (Not cameraImageReady) Then
				TL.LogMessage("ImageArrayVariant Get", "Throwing InvalidOperationException because of a call to ImageArrayVariant before the first image has been taken!")
				Throw New ASCOM.InvalidOperationException("Call to ImageArrayVariant before the first image has been taken!")
			End If

			ReDim cameraImageArrayVariant(cameraNumX - 1, cameraNumY - 1)
			For i As Integer = 0 To cameraImageArray.GetLength(1) - 1
				For j As Integer = 0 To cameraImageArray.GetLength(0) - 1
					cameraImageArrayVariant(j, i) = cameraImageArray(j, i)
				Next
			Next

			Return cameraImageArrayVariant
		End Get
	End Property

	''' <summary>
	''' Returns a flag indicating whether the image is ready to be downloaded from the camera.
	''' </summary>
	''' <value><c>True</c> if [image ready]; otherwise, <c>False</c>.</value>
	Public ReadOnly Property ImageReady() As Boolean Implements ICameraV3.ImageReady
		Get
			TL.LogMessage("ImageReady Get", cameraImageReady.ToString())
			Return cameraImageReady
		End Get
	End Property

	''' <summary>
	''' Returns a flag indicating whether the camera Is currently in a <see cref="PulseGuide">PulseGuide</see> operation.
	''' </summary>
	''' <value>
	''' <c>True</c> if this instance Is pulse guiding; otherwise, <c>False</c>.
	''' </value>
	Public ReadOnly Property IsPulseGuiding() As Boolean Implements ICameraV3.IsPulseGuiding
		Get
			TL.LogMessage("IsPulseGuiding Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("IsPulseGuiding", False)
		End Get
	End Property

	''' <summary>
	''' Reports the actual exposure duration in seconds (i.e. shutter open time).
	''' </summary>
	''' <value>The last duration of the exposure.</value>
	Public ReadOnly Property LastExposureDuration() As Double Implements ICameraV3.LastExposureDuration
		Get
			If (Not cameraImageReady) Then
				TL.LogMessage("LastExposureDuration Get", "Throwing InvalidOperationException because of a call to LastExposureDuration before the first image has been taken!")
				Throw New ASCOM.InvalidOperationException("Call to LastExposureDuration before the first image has been taken!")
			End If
			TL.LogMessage("LastExposureDuration Get", cameraLastExposureDuration.ToString())
			Return cameraLastExposureDuration
		End Get
	End Property

	''' <summary>
	''' Reports the actual exposure start in the FITS-standard CCYY-MM-DDThh:mm:ss[.sss...] format.
	''' The start time must be UTC.
	''' </summary>
	''' <value>The last exposure start time in UTC.</value>
	Public ReadOnly Property LastExposureStartTime() As String Implements ICameraV3.LastExposureStartTime
		Get
			If (Not cameraImageReady) Then
				TL.LogMessage("LastExposureStartTime Get", "Throwing InvalidOperationException because of a call to LastExposureStartTime before the first image has been taken!")
				Throw New ASCOM.InvalidOperationException("Call to LastExposureStartTime before the first image has been taken!")
			End If
			Dim exposureStartString As String = exposureStart.ToString("yyyy-MM-ddTHH:mm:ss")
			TL.LogMessage("LastExposureStartTime Get", exposureStartString.ToString())
			Return exposureStartString
		End Get
	End Property

	''' <summary>
	''' Reports the maximum ADU value the camera can produce.
	''' </summary>
	''' <value>The maximum ADU.</value>
	Public ReadOnly Property MaxADU() As Integer Implements ICameraV3.MaxADU
		Get
			TL.LogMessage("MaxADU Get", "20000")
			Return 20000
		End Get
	End Property

	''' <summary>
	''' Returns the maximum allowed binning for the X camera axis
	''' </summary>
	''' <value>The max bin X.</value>
	Public ReadOnly Property MaxBinX() As Short Implements ICameraV3.MaxBinX
		Get
			TL.LogMessage("MaxBinX Get", "1")
			Return 1
		End Get
	End Property

	''' <summary>
	''' Returns the maximum allowed binning for the Y camera axis
	''' </summary>
	''' <value>The max bin Y.</value>
	Public ReadOnly Property MaxBinY() As Short Implements ICameraV3.MaxBinY
		Get
			TL.LogMessage("MaxBinY Get", "1")
			Return 1
		End Get
	End Property

	''' <summary>
	''' Sets the subframe width. Also returns the current value.
	''' </summary>
	''' <value>The num X.</value>
	Public Property NumX() As Integer Implements ICameraV3.NumX
		Get
			TL.LogMessage("NumX Get", cameraNumX.ToString())
			Return cameraNumX
		End Get
		Set(value As Integer)
			cameraNumX = value
			TL.LogMessage("NumX set", value.ToString())
		End Set
	End Property

	''' <summary>
	''' Sets the subframe height. Also returns the current value.
	''' </summary>
	''' <value>The num Y.</value>
	Public Property NumY() As Integer Implements ICameraV3.NumY
		Get
			TL.LogMessage("NumY Get", cameraNumY.ToString())
			Return cameraNumY
		End Get
		Set(value As Integer)
			cameraNumY = value
			TL.LogMessage("NumY set", value.ToString())
		End Set
	End Property

	''' <summary>
	''' The camera's offset (OFFSET VALUE MODE) OR the index of the selected camera offset description in the <see cref="Offsets" /> array (OFFSETS INDEX MODE)
	''' </summary>
	''' <returns><para><b> OFFSET VALUE MODE:</b> The current offset value.</para>
	''' <p style="color:red"><b>Or</b></p>
	''' <b>OFFSETS INDEX MODE:</b> Index into the Offsets array for the current camera offset
	''' </returns>
	Public Property Offset() As Integer Implements ICameraV3.Offset
		Get
			TL.LogMessage("Offset Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Offset", False)
		End Get
		Set(value As Integer)
			TL.LogMessage("Offset Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Offset", True)
		End Set
	End Property

	''' <summary>
	''' Maximum <see cref="Offset" /> value that this camera supports
	''' </summary>
	''' <returns>The maximum offset value that this camera supports</returns>
	Public ReadOnly Property OffsetMax() As Integer Implements ICameraV3.OffsetMax
		Get
			TL.LogMessage("OffsetMax Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("OffsetMax", False)
		End Get
	End Property

	''' <summary>
	''' Minimum <see cref="Offset" /> value that this camera supports
	''' </summary>
	''' <returns>The minimum offset value that this camera supports</returns>
	Public ReadOnly Property OffsetMin() As Integer Implements ICameraV3.OffsetMin
		Get
			TL.LogMessage("OffsetMin Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("OffsetMin", False)
		End Get
	End Property

	''' <summary>
	''' List of Offset names supported by the camera
	''' </summary>
	''' <returns>The list of supported offset names as an ArrayList of strings</returns>
	Public ReadOnly Property Offsets() As ArrayList Implements ICameraV3.Offsets
		Get
			TL.LogMessage("Offsets Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Offsets", False)
		End Get
	End Property

	''' <summary>
	''' Percent completed, Interface Version 2 And later
	''' </summary>
	''' <returns>A value between 0 And 100% indicating the completeness of this operation</returns>
	Public ReadOnly Property PercentCompleted() As Short Implements ICameraV3.PercentCompleted
		Get
			TL.LogMessage("PercentCompleted Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("PercentCompleted", False)
		End Get
	End Property

	''' <summary>
	''' Returns the width of the CCD chip pixels in microns.
	''' </summary>
	''' <value>The pixel size X.</value>
	Public ReadOnly Property PixelSizeX() As Double Implements ICameraV3.PixelSizeX
		Get
			TL.LogMessage("PixelSizeX Get", pixelSize.ToString())
			Return pixelSize
		End Get
	End Property

	''' <summary>
	''' Returns the height of the CCD chip pixels in microns.
	''' </summary>
	''' <value>The pixel size Y.</value>
	Public ReadOnly Property PixelSizeY() As Double Implements ICameraV3.PixelSizeY
		Get
			TL.LogMessage("PixelSizeY Get", pixelSize.ToString())
			Return pixelSize
		End Get
	End Property

	''' <summary>
	''' Activates the Camera's mount control system to instruct the mount to move in a particular direction for a given period of time
	''' </summary>
	''' <param name="Direction">The direction of movement.</param>
	''' <param name="Duration">The duration of movement in milliseconds.</param>
	Public Sub PulseGuide(Direction As GuideDirections, Duration As Integer) Implements ICameraV3.PulseGuide
		TL.LogMessage("PulseGuide", "Not implemented - " & Direction.ToString)
		Throw New ASCOM.MethodNotImplementedException("Direction")
	End Sub

	''' <summary>
	''' Readout mode, Interface Version 2 and later
	''' </summary>
	''' <value></value>
	''' <returns>Short integer index into the <see cref="ReadoutModes">ReadoutModes</see> array of string readout mode names indicating
	''' the camera's current readout mode.</returns>
	Public Property ReadoutMode() As Short Implements ICameraV3.ReadoutMode
		Get
			TL.LogMessage("ReadoutMode Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("ReadoutMode", False)
		End Get
		Set(value As Short)
			TL.LogMessage("ReadoutMode Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("ReadoutMode", True)
		End Set
	End Property

	''' <summary>
	''' List of available readout modes, Interface Version 2 and later
	''' </summary>
	''' <returns>An ArrayList of readout mode names</returns>
	Public ReadOnly Property ReadoutModes() As ArrayList Implements ICameraV3.ReadoutModes
		Get
			TL.LogMessage("ReadoutModes Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("ReadoutModes", False)
		End Get
	End Property

	''' <summary>
	''' Sensor name, Interface Version 2 and later
	''' </summary>
	''' <returns>The name of the sensor used within the camera.</returns>
	Public ReadOnly Property SensorName() As String Implements ICameraV3.SensorName
		Get
			TL.LogMessage("SensorName Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SensorName", False)
		End Get
	End Property

	''' <summary>
	''' Type of colour information returned by the camera sensor, Interface Version 2 and later
	''' </summary>
	''' <value>The type of sensor used by the camera.</value>
	Public ReadOnly Property SensorType() As SensorType Implements ICameraV3.SensorType
		Get
			TL.LogMessage("SensorType Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SensorType", False)
		End Get
	End Property

	''' <summary>
	''' Sets the camera cooler setpoint in degrees Celsius, And returns the current setpoint.
	''' </summary>
	''' <value>The set CCD temperature.</value>
	Public Property SetCCDTemperature() As Double Implements ICameraV3.SetCCDTemperature
		Get
			TL.LogMessage("SetCCDTemperature Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SetCCDTemperature", False)
		End Get
		Set(value As Double)
			TL.LogMessage("SetCCDTemperature Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SetCCDTemperature", True)
		End Set
	End Property

	''' <summary>
	''' Starts an exposure. Use <see cref="ImageReady" /> to check when the exposure Is complete.
	''' </summary>
	''' <param name="Duration">Duration of exposure in seconds, can be zero if <see cref="StartExposure">Light</see> Is <c>False</c></param>
	''' <param name="Light"><c>True</c> for light frame, <c>False</c> for dark frame (ignored if no shutter)</param>
	Public Sub StartExposure(Duration As Double, Light As Boolean) Implements ICameraV3.StartExposure
		If (Duration < 0.0) Then Throw New InvalidValueException("StartExposure", Duration.ToString(), "0.0 upwards")
		If (cameraNumX > ccdWidth) Then Throw New InvalidValueException("StartExposure", cameraNumX.ToString(), ccdWidth.ToString())
		If (cameraNumY > ccdHeight) Then Throw New InvalidValueException("StartExposure", cameraNumY.ToString(), ccdHeight.ToString())
		If (cameraStartX > ccdWidth) Then Throw New InvalidValueException("StartExposure", cameraStartX.ToString(), ccdWidth.ToString())
		If (cameraStartY > ccdHeight) Then Throw New InvalidValueException("StartExposure", cameraStartY.ToString(), ccdHeight.ToString())

		cameraLastExposureDuration = Duration
		exposureStart = DateTime.Now
		System.Threading.Thread.Sleep(Duration * 1000) ' Sleep for the duration to simulate exposure 
		TL.LogMessage("StartExposure", Duration.ToString() + " " + Light.ToString())
		cameraImageReady = True
	End Sub

	''' <summary>
	''' Sets the subframe start position for the X axis (0 based) And returns the current value.
	''' </summary>
	Public Property StartX() As Integer Implements ICameraV3.StartX
		Get
			TL.LogMessage("StartX Get", cameraStartX.ToString())
			Return cameraStartX
		End Get
		Set(value As Integer)
			cameraStartX = value
			TL.LogMessage("StartX set", value.ToString())
		End Set
	End Property

	''' <summary>
	''' Sets the subframe start position for the Y axis (0 based). Also returns the current value.
	''' </summary>
	Public Property StartY() As Integer Implements ICameraV3.StartY
		Get
			TL.LogMessage("StartY Get", cameraStartY.ToString())
			Return cameraStartY
		End Get
		Set(value As Integer)
			cameraStartY = value
			TL.LogMessage("StartY set", value.ToString())
		End Set
	End Property

	''' <summary>
	''' Stops the current exposure, if any.
	''' </summary>
	Public Sub StopExposure() Implements ICameraV3.StopExposure
		TL.LogMessage("StopExposure", "Not implemented")
		Throw New MethodNotImplementedException("StopExposure")
	End Sub

	''' <summary>
	''' Camera's sub-exposure interval
	''' </summary>
	Public Property SubExposureDuration() As Double Implements ICameraV3.SubExposureDuration
		Get
			TL.LogMessage("SubExposureDuration Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SubExposureDuration", False)
		End Get
		Set(value As Double)
			TL.LogMessage("SubExposureDuration Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SubExposureDuration", True)
		End Set
	End Property

#End Region

	'//ENDOFINSERTEDFILE
End Class