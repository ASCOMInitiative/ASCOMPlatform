' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports System.Collections
Imports ASCOM
Imports ASCOM.Utilities

Class DeviceVideo
	Implements IVideo
	Private m_util As New Util()
	Private TL As New TraceLogger()

#Region "IVideo Implementation"
	Private Const _deviceWidth As Integer = 720 'Constants to define the ccd pixel dimensions
	Private Const _deviceHeight As Integer = 480
	Private Const _bitDepth As Integer = 8
	Private Const _videoFileFormat As String = "AVI"

	''' <summary>
	''' Reports the bit depth the camera can produce.
	''' </summary>
	''' <value>The bit depth per pixel. Typical analogue videos are 8-bit while some digital cameras can provide 12, 14 or 16-bit images.</value>
	Public ReadOnly Property BitDepth As Integer Implements IVideo.BitDepth
		Get
			TL.LogMessage("BitDepth Get", "Not implemented")
			Return _bitDepth
		End Get
	End Property

	''' <summary>
	''' Returns the current camera operational state.
	''' </summary>
	Public ReadOnly Property CameraState As VideoCameraState Implements IVideo.CameraState
		Get
			TL.LogMessage("CameraState Get", "Not implemented")
			Return VideoCameraState.videoCameraError
		End Get
	End Property

	''' <summary>
	''' Returns True if the driver supports custom device properties configuration via the <see cref="M:ASCOM.DeviceInterface.IVideo.ConfigureDeviceProperties"/> method.
	''' </summary>
	Public ReadOnly Property CanConfigureDeviceProperties As Boolean Implements IVideo.CanConfigureDeviceProperties
		Get
			Dim retval As Boolean = False
			TL.LogMessage("CanConfigureDeviceProperties Get", $"{retval}")
			Return retval
		End Get
	End Property


	''' <summary>
	''' Displays a device properties configuration dialog that allows the configuration of specialized settings.
	''' </summary>
	Public Sub ConfigureDeviceProperties() Implements IVideo.ConfigureDeviceProperties
		TL.LogMessage("ConfigureDeviceProperties", "Not implemented")
		Throw New MethodNotImplementedException()
	End Sub


	''' <summary>
	''' The maximum supported exposure (integration time) in seconds.
	''' </summary>
	Public ReadOnly Property ExposureMax As Double Implements IVideo.ExposureMax
		Get
			' Standard NTSC frame duration
			Dim retval As Double = 0.03337
			TL.LogMessage("ExposureMax Get", $"{retval:f5}")
			Return retval
		End Get
	End Property

	''' <summary>
	''' The minimum supported exposure (integration time) in seconds.
	''' </summary>
	Public ReadOnly Property ExposureMin As Double Implements IVideo.ExposureMin
		Get
			' Standard NTSC frame duration
			Dim retval As Double = 0.03337
			TL.LogMessage("ExposureMin Get", $"{retval:f5}")
			Return retval
		End Get
	End Property

	''' <summary>
	''' The frame rate at which the camera is running.
	''' </summary>
	Public ReadOnly Property FrameRate As VideoCameraFrameRate Implements IVideo.FrameRate
		Get
			TL.LogMessage("FrameRate Get", "VideoCameraFrameRate.NTSC")
			Return VideoCameraFrameRate.NTSC
		End Get
	End Property

	''' <summary>
	''' Index into the <see cref="P:ASCOM.DeviceInterface.IVideo.Gains"/> array for the selected camera gain.
	''' </summary>
	''' <value>Short integer index for the current camera gain in the <see cref="P:ASCOM.DeviceInterface.IVideo.Gains"/> string array.</value>
	''' <returns>Index into the Gains array for the selected camera gain</returns>
	Public Property Gain As Short Implements IVideo.Gain
		Get
			TL.LogMessage("Gain Get", "Not implemented")
			Throw New PropertyNotImplementedException("Gain", False)
		End Get
		Set(value As Short)
			TL.LogMessage("Gain Set", "Not implemented")
			Throw New PropertyNotImplementedException("Gain", True)
		End Set
	End Property

	''' <summary>
	''' Maximum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/>.
	''' </summary>
	''' <returns>The maximum gain value that this camera supports</returns>
	Public ReadOnly Property GainMax As Short Implements IVideo.GainMax
		Get
			TL.LogMessage("GainMax Get", "Not implemented")
			Throw New PropertyNotImplementedException("GainMax", False)
		End Get
	End Property

	''' <summary>
	''' Minimum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/>.
	''' </summary>
	''' <returns>The minimum gain value that this camera supports</returns>
	Public ReadOnly Property GainMin As Short Implements IVideo.GainMin
		Get
			TL.LogMessage("GainMin Get", "Not implemented")
			Throw New PropertyNotImplementedException("GainMin", False)
		End Get
	End Property

	''' <summary>
	''' Gains supported by the camera.
	''' </summary>
	''' <returns>An ArrayList of gain names or values</returns>
	Public ReadOnly Property Gains As ArrayList Implements IVideo.Gains
		Get
			TL.LogMessage("Gains Get", "Not implemented")
			Throw New PropertyNotImplementedException("Gains", False)
		End Get
	End Property

	''' <summary>
	''' Index into the <see cref="P:ASCOM.DeviceInterface.IVideo.Gammas"/> array for the selected camera gamma.
	''' </summary>
	''' <value>Short integer index for the current camera gamma in the <see cref="P:ASCOM.DeviceInterface.IVideo.Gammas"/> string array.</value>
	''' <returns>Index into the Gammas array for the selected camera gamma</returns>
	Public Property Gamma As Short Implements IVideo.Gamma
		Get
			TL.LogMessage("Gamma Get", "Not implemented")
			Throw New PropertyNotImplementedException("Gamma", False)
		End Get
		Set(value As Short)
			TL.LogMessage("Gamma Set", "Not implemented")
			Throw New PropertyNotImplementedException("Gamma", True)
		End Set
	End Property

	''' <summary>
	''' Maximum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/>.
	''' </summary>
	''' <value>Short integer representing the maximum gamma value supported by the camera.</value>
	''' <returns>The maximum gamma value that this camera supports</returns>
	Public ReadOnly Property GammaMax As Short Implements IVideo.GammaMax
		Get
			TL.LogMessage("GammaMax Get", "Not implemented")
			Throw New PropertyNotImplementedException("GammaMax", False)
		End Get
	End Property

	''' <summary>
	''' Minimum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/>.
	''' </summary>
	''' <returns>The minimum gamma value that this camera supports</returns>
	Public ReadOnly Property GammaMin As Short Implements IVideo.GammaMin
		Get
			TL.LogMessage("GammaMin Get", "Not implemented")
			Throw New PropertyNotImplementedException("GammaMin", False)
		End Get
	End Property

	''' <summary>
	''' Gammas supported by the camera.
	''' </summary>
	''' <returns>An ArrayList of gamma names or values</returns>
	Public ReadOnly Property Gammas As ArrayList Implements IVideo.Gammas
		Get
			TL.LogMessage("Gammas Get", "Not implemented")
			Throw New PropertyNotImplementedException("Gammas", False)
		End Get
	End Property

	''' <summary>
	''' Returns the height of the video frame in pixels.
	''' </summary>
	''' <value>The video frame height.</value>
	Public ReadOnly Property Height As Integer Implements IVideo.Height
		Get
			TL.LogMessage("Height Get", $"{_deviceHeight}")
			Return _deviceHeight
		End Get
	End Property

	''' <summary>
	''' Index into the <see cref="P:ASCOM.DeviceInterface.IVideo.SupportedIntegrationRates"/> array for the selected camera integration rate.
	''' </summary>
	''' <value>Integer index for the current camera integration rate in the <see cref="P:ASCOM.DeviceInterface.IVideo.SupportedIntegrationRates"/> string array.</value>
	''' <returns>Index into the SupportedIntegrationRates array for the selected camera integration rate.</returns>
	Public Property IntegrationRate As Integer Implements IVideo.IntegrationRate
		Get
			TL.LogMessage("IntegrationRate Get", "Not implemented")
			Throw New PropertyNotImplementedException("IntegrationRate", False)
		End Get
		Set(value As Integer)
			TL.LogMessage("IntegrationRate Set", "Not implemented")
			Throw New PropertyNotImplementedException("IntegrationRate", True)
		End Set
	End Property

	''' <summary>
	''' Returns an <see cref="DeviceInterface.IVideoFrame"/> with its <see cref="P:ASCOM.DeviceInterface.IVideoFrame.ImageArray"/> property populated.
	''' </summary>
	''' <value>The current video frame.</value>
	Public ReadOnly Property LastVideoFrame As IVideoFrame Implements IVideo.LastVideoFrame
		Get
			Dim msg As String = "There are no video frames available."
			TL.LogMessage("LastVideoFrame Get", msg)
			Throw New InvalidOperationException(msg)
		End Get
	End Property

	''' <summary>
	''' Returns the width of the CCD chip pixels in microns.
	''' </summary>
	Public ReadOnly Property PixelSizeX As Double Implements IVideo.PixelSizeX
		Get
			TL.LogMessage("PixelSizeX Get", "Not implemented")
			Throw New PropertyNotImplementedException("PixelSizeX", False)
		End Get
	End Property

	''' <summary>
	''' Returns the height of the CCD chip pixels in microns.
	''' </summary>
	''' <value>The pixel size Y if known.</value>
	Public ReadOnly Property PixelSizeY As Double Implements IVideo.PixelSizeY
		Get
			TL.LogMessage("PixelSizeY Get", "Not implemented")
			Throw New PropertyNotImplementedException("PixelSizeY", False)
		End Get
	End Property

	''' <summary>
	''' Sensor name.
	''' </summary>
	''' <returns>The name of sensor used within the camera.</returns>
	Public ReadOnly Property SensorName As String Implements IVideo.SensorName
		Get
			TL.LogMessage("SensorName Get", "Not implemented")
			Throw New PropertyNotImplementedException("SensorName", False)
		End Get
	End Property

	''' <summary>
	''' Type of colour information returned by the the camera sensor.
	''' </summary>
	''' <returns>The <see cref="DeviceInterface.SensorType"/> enum value of the camera sensor</returns>
	Public ReadOnly Property SensorType As SensorType Implements IVideo.SensorType
		Get
			TL.LogMessage("SensorType Get", "Monochrome")
			Return SensorType.Monochrome
		End Get
	End Property

	''' <summary>
	''' Starts recording a new video file.
	''' </summary>
	''' <param name="PreferredFileName">The file name requested by the client. Some systems may not allow the file name to be controlled directly and they should ignore this parameter.</param>
	''' <returns>The actual file name of the file that is being recorded.</returns>
	Public Function StartRecordingVideoFile(PreferredFileName As String) As String Implements IVideo.StartRecordingVideoFile
		Dim msg As String = "Cannot start recording a video file right now."
		TL.LogMessage("StartRecordingVideoFile", msg)
		Throw New InvalidOperationException(msg)
	End Function

	''' <summary>
	''' Stops the recording of a video file.
	''' </summary>
	Public Sub StopRecordingVideoFile() Implements IVideo.StopRecordingVideoFile
		Dim msg As String = "Cannot stop recording right now."
		TL.LogMessage("StopRecordingVideoFile", msg)
		Throw New InvalidOperationException(msg)
	End Sub

	''' <summary>
	''' Returns the list of integration rates supported by the video camera.
	''' </summary>
	''' <value>The list of supported integration rates in seconds.</value>
	Public ReadOnly Property SupportedIntegrationRates As ArrayList Implements IVideo.SupportedIntegrationRates
		Get
			TL.LogMessage("SupportedIntegrationRates Get", "Not implemented")
			Throw New PropertyNotImplementedException("SupportedIntegrationRates", False)
		End Get
	End Property

	''' <summary>
	''' The name of the video capture device when such a device is used.
	''' </summary>
	Public ReadOnly Property VideoCaptureDeviceName As String Implements IVideo.VideoCaptureDeviceName
		Get
			TL.LogMessage("VideoCaptureDeviceName Get", "Returning an empty string")
			Return String.Empty
		End Get
	End Property

	''' <summary>
	''' Returns the video codec used to record the video file.
	''' </summary>
	Public ReadOnly Property VideoCodec As String Implements IVideo.VideoCodec
		Get
			TL.LogMessage("VideoCodec Get", "Returning an empty string")
			Return String.Empty
		End Get
	End Property

	''' <summary>
	''' Returns the file format of the recorded video file, e.g. AVI, MPEG, ADV etc.
	''' </summary>
	Public ReadOnly Property VideoFileFormat As String Implements IVideo.VideoFileFormat
		Get
			TL.LogMessage("VideoFileFormat Get", _videoFileFormat)
			Return _videoFileFormat
		End Get
	End Property

	''' <summary>
	''' The size of the video frame buffer.
	''' </summary>
	Public ReadOnly Property VideoFramesBufferSize As Integer Implements IVideo.VideoFramesBufferSize
		Get
			Dim retval As Integer = 0
			TL.LogMessage("VideoFramesBufferSize Get", $"{retval}")
			Return retval
		End Get
	End Property

	''' <summary>
	''' Returns the width of the video frame in pixels.
	''' </summary>
	''' <value>The video frame width.</value>
	Public ReadOnly Property Width As Integer Implements IVideo.Width
		Get
			TL.LogMessage("Width Get", $"{_deviceWidth}")
			Return _deviceWidth
		End Get
	End Property
#End Region

	'//ENDOFINSERTEDFILE
End Class