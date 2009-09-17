'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM Camera driver for $safeprojectname$
'
' Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
'				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
'				erat, sed diam voluptua. At vero eos et accusam et justo duo 
'				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
'				sanctus est Lorem ipsum dolor sit amet.
'
' Implements:	ASCOM Camera interface version: 1.0
' Author:		(XXX) Your N. Here <your@email.here>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	1.0.0	Initial edit, from Camera template
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.$safeprojectname$.Camera
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Camera
' The ClassInterface/None addribute prevents an empty interface called
' _Camera from being created and used as the [default] interface
'
<Guid("$guid2$")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class Camera
    '	==========
    Implements ICamera  ' Early-bind interface implemented by this driver
    '	==========
    '
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Private Shared s_csDriverID As String = "ASCOM.$safeprojectname$.Camera"
    ' TODO Change the descriptive string for your driver then remove this line
    Private Shared s_csDriverDescription As String = "$safeprojectname$ Camera"

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()
        ' TODO Implement your additional construction here
    End Sub

#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Dim P As New Helper.Profile()
        P.DeviceTypeV = "Camera"            '  Requires Helper 5.0.3 or later
        If bRegister Then
            P.Register(s_csDriverID, s_csDriverDescription)
        Else
            P.Unregister(s_csDriverID)
        End If
        Try                                 ' In case Helper becomes native .NET
            Marshal.ReleaseComObject(P)
        Catch ex As Exception
            ' Ignore exception
        End Try
        P = Nothing

    End Sub

    <ComRegisterFunction()> _
    Public Shared Sub RegisterASCOM(ByVal T As Type)

        RegUnregASCOM(True)

    End Sub

    <ComUnregisterFunction()> _
    Public Shared Sub UnregisterASCOM(ByVal T As Type)

        RegUnregASCOM(False)

    End Sub

#End Region

    '
    ' PUBLIC COM INTERFACE ICamera IMPLEMENTATION
    '

#Region "ICamera Members"

    ' Sets the binning factor for the X axis.  Also returns the current value.  Should
    ' default to 1 when the camera link is established.  Note:  driver does not check
    ' for compatible subframe values when this value is set; rather they are checked upon StartExposure.
    ' BinX sets/gets the X binning value
    ' Must throw an exception for illegal binning values
    Public Property BinX() As Short Implements ICamera.BinX
        Get
            Throw New PropertyNotImplementedException("BinX", False)
        End Get
        Set(ByVal value As Short)
            Throw New PropertyNotImplementedException("BinX", True)
        End Set
    End Property

    ' Sets the binning factor for the Y axis  Also returns the current value.  Should
    ' default to 1 when the camera link is established.  Note:  driver does not check
    ' for compatible subframe values when this value is set; rather they are checked upon StartExposure.
    ' Must throw an exception for illegal binning values
    Public Property BinY() As Short Implements ICamera.BinY
        Get
            Throw New PropertyNotImplementedException("BinY", False)
        End Get
        Set(ByVal value As Short)
            Throw New PropertyNotImplementedException("BinY", True)
        End Set
    End Property

    ' Returns the current CCD temperature in degrees Celsius. Only valid if
    ' CanControlTemperature is True.
    ' Must throw exception if data unavailable.
    Public ReadOnly Property CCDTemperature() As Double Implements ICamera.CCDTemperature
        Get
            Throw New PropertyNotImplementedException("CCDTemperature", False)
        End Get
    End Property

    ' Returns one of the following status information:
    '   Value  State           Meaning</description>
    '   0      CameraIdle      At idle state, available to start exposure</description>
    '   1      CameraWaiting   Exposure started but waiting (for shutter, trigger,
    '                          filter wheel, etc.)</description>
    '   2      CameraExposing  Exposure currently in progress</description>
    '   3      CameraReading   CCD array is being read out (digitized)</description>
    '   4      CameraDownload  Downloading data to PC</description>
    '   5      CameraError     Camera error condition serious enough to prevent
    '                          further operations (link fail, etc.).</description>
    ' Must return an exception if the camera status is unavailable.
    Public ReadOnly Property CameraState() As CameraStates Implements ICamera.CameraState
        Get
            Throw New PropertyNotImplementedException("CameraState", False)
        End Get
    End Property

    ' Returns the width of the CCD camera chip in unbinned pixels.
    ' Must throw exception if the value is not known
    Public ReadOnly Property CameraXSize() As Integer Implements ICamera.CameraXSize
        Get
            Throw New PropertyNotImplementedException("CameraXSize", False)
        End Get
    End Property

    ' Returns the height of the CCD camera chip in unbinned pixels.
    ' Must throw exception if the value is not known
    Public ReadOnly Property CameraYSize() As Integer Implements ICamera.CameraYSize
        Get
            Throw New PropertyNotImplementedException("CameraYSize", False)
        End Get
    End Property

    ' Returns True if the camera can abort exposures; False if not.
    Public ReadOnly Property CanAbortExposure() As Boolean Implements ICamera.CanAbortExposure
        Get
            Throw New PropertyNotImplementedException("CanAbortExposure", False)
        End Get
    End Property

    ' If True, the camera can have different binning on the X and Y axes, as
    ' determined by BinX and BinY. If False, the binning must be equal on the X and Y axes.
    ' Must throw exception if the value is not known (n.b. normally only occurs if no link established and camera must be queried)
    Public ReadOnly Property CanAsymmetricBin() As Boolean Implements ICamera.CanAsymmetricBin
        Get
            Throw New PropertyNotImplementedException("CanAsymmetricBin", False)
        End Get
    End Property

    ' If True, the camera's cooler power setting can be read.
    Public ReadOnly Property CanGetCoolerPower() As Boolean Implements ICamera.CanGetCoolerPower
        Get
            Throw New PropertyNotImplementedException("CanGetCoolerPower", False)
        End Get
    End Property

    ' Returns True if the camera can send autoguider pulses to the telescope mount;
    ' False if not.  (Note: this does not provide any indication of whether the
    ' autoguider cable is actually connected.)
    Public ReadOnly Property CanPulseGuide() As Boolean Implements ICamera.CanPulseGuide
        Get
            Throw New PropertyNotImplementedException("CanPulseGuide", False)
        End Get
    End Property

    ' If True, the camera's cooler setpoint can be adjusted. If False, the camera
    ' either uses open-loop cooling or does not have the ability to adjust temperature
    ' from software, and setting the TemperatureSetpoint property has no effect.
    Public ReadOnly Property CanSetCCDTemperature() As Boolean Implements ICamera.CanSetCCDTemperature
        Get
            Throw New PropertyNotImplementedException("CanSetCCDTemperature", False)
        End Get
    End Property

    ' Some cameras support StopExposure, which allows the exposure to be terminated
    ' before the exposure timer completes, but will still read out the image.  Returns
    ' True if StopExposure is available, False if not.
    ' 
    ' Must throw exception if not supported
    ' Must throw exception if an error condition such as link failure is present
    Public ReadOnly Property CanStopExposure() As Boolean Implements ICamera.CanStopExposure
        Get
            Throw New PropertyNotImplementedException("CanStopExposure", False)
        End Get
    End Property

    ' Controls the link between the driver and the camera. Set True to enable the
    ' link. Set False to disable the link (this does not switch off the cooler).
    ' You can also read the property to check whether it is connected.
    ' 
    ' Must throw exception if unsuccessful.
    Public Property Connected() As Boolean Implements ICamera.Connected
        Get
            Throw New PropertyNotImplementedException("Connected", False)
        End Get
        Set(ByVal value As Boolean)
            Throw New PropertyNotImplementedException("Connected", True)
        End Set
    End Property

    ' Turns on and off the camera cooler, and returns the current on/off state.
    ' Warning: turning the cooler off when the cooler is operating at high delta-T
    ' (typically >20C below ambient) may result in thermal shock.  Repeated thermal
    ' shock may lead to damage to the sensor or cooler stack.  Please consult the
    ' documentation supplied with the camera for further information.
    ' 
    ' Must throw exception if not supported
    ' Must throw exception if an error condition such as link failure is present
    Public Property CoolerOn() As Boolean Implements ICamera.CoolerOn
        Get
            Throw New PropertyNotImplementedException("CoolerOn", False)
        End Get
        Set(ByVal value As Boolean)
            Throw New PropertyNotImplementedException("CoolerOn", True)
        End Set
    End Property

    ' Returns the present cooler power level, in percent.  Returns zero if CoolerOn is
    ' False.
    ' Must throw exception if not supported
    ' Must throw exception if an error condition such as link failure is present
    Public ReadOnly Property CoolerPower() As Double Implements ICamera.CoolerPower
        Get
            Throw New PropertyNotImplementedException("CoolerPower", False)
        End Get
    End Property

    ' Returns a description of the camera model, such as manufacturer and model
    ' number. Any ASCII characters may be used. The string shall not exceed 68
    ' characters (for compatibility with FITS headers).
    ' 
    ' Must throw exception if description unavailable
    Public ReadOnly Property Description() As String Implements ICamera.Description
        Get
            Throw New PropertyNotImplementedException("Description", False)
        End Get
    End Property

    ' Returns the gain of the camera in photoelectrons per A/D unit. (Some cameras have
    ' multiple gain modes; these should be selected via the SetupDialog and thus are
    ' static during a session.)
    ' 
    ' Must throw exception if data unavailable.
    Public ReadOnly Property ElectronsPerADU() As Double Implements ICamera.ElectronsPerADU
        Get
            Throw New PropertyNotImplementedException("ElectronsPerADU", False)
        End Get
    End Property

    ' Reports the full well capacity of the camera in electrons, at the current camera
    ' settings (binning, SetupDialog settings, etc.)
    ' 
    ' Must throw exception if data unavailable.
    Public ReadOnly Property FullWellCapacity() As Double Implements ICamera.FullWellCapacity
        Get
            Throw New PropertyNotImplementedException("FullWellCapacity", False)
        End Get
    End Property

    ' If True, the camera has a mechanical shutter. If False, the camera does not have
    ' a shutter.  If there is no shutter, the StartExposure command will ignore the Light parameter.
    Public ReadOnly Property HasShutter() As Boolean Implements ICamera.HasShutter
        Get
            Throw New PropertyNotImplementedException("HasShutter", False)
        End Get
    End Property

    ' Returns the current heat sink temperature (called "ambient temperature" by some
    ' manufacturers) in degrees Celsius. Only valid if CanControlTemperature is True.
    ' Must throw exception if data unavailable.
    Public ReadOnly Property HeatSinkTemperature() As Double Implements ICamera.HeatSinkTemperature
        Get
            Throw New PropertyNotImplementedException("HeatSinkTemperature", False)
        End Get
    End Property

    ' Returns a safearray of int of size NumX * NumY containing the pixel values from
    ' the last exposure. The application must inspect the Safearray parameters to
    ' determine the dimensions. Note: if NumX or NumY is changed after a call to
    ' StartExposure it will have no effect on the size of this array. This is the
    ' preferred method for programs (not scripts) to download images since it requires
    ' much less memory.
    '
    ' For color or multispectral cameras, will produce an array of NumX * NumY *
    ' NumPlanes.  If the application cannot handle multispectral images, it should use
    ' just the first plane.
    ' 
    ' Must throw exception if data unavailable.
    Public ReadOnly Property ImageArray() As Object Implements ICamera.ImageArray
        Get
            Throw New PropertyNotImplementedException("ImageArray", False)
        End Get
    End Property

    ' Returns a safearray of Variant of size NumX * NumY containing the pixel values
    ' from the last exposure. The application must inspect the Safearray parameters to
    ' determine the dimensions. Note: if NumX or NumY is changed after a call to
    ' StartExposure it will have no effect on the size of this array. This property
    ' should only be used from scripts due to the extremely high memory utilization on
    ' large image arrays (26 bytes per pixel). Pixels values should be in Short, int,
    ' or Double format.
    '
    ' For color or multispectral cameras, will produce an array of NumX * NumY *
    ' NumPlanes.  If the application cannot handle multispectral images, it should use
    ' just the first plane.
    ' 
    ' Must throw exception if data unavailable.
    Public ReadOnly Property ImageArrayVariant() As Object Implements ICamera.ImageArrayVariant
        Get
            Throw New PropertyNotImplementedException("ImageArrayVariant", False)
        End Get
    End Property

    ' If True, there is an image from the camera available. If False, no image
    ' is available and attempts to use the ImageArray method will produce an exception.
    ' Must throw exception if hardware or communications link error has occurred.
    Public ReadOnly Property ImageReady() As Boolean Implements ICamera.ImageReady
        Get
            Throw New PropertyNotImplementedException("ImageReady", False)
        End Get
    End Property

    ' If True, pulse guiding is in progress. Required if the PulseGuide() method
    ' (which is non-blocking) is implemented. See the PulseGuide() method.
    ' Must throw exception if hardware or communications link error has occurred.
    Public ReadOnly Property IsPulseGuiding() As Boolean Implements ICamera.IsPulseGuiding
        Get
            Throw New PropertyNotImplementedException("IsPulseGuiding", False)
        End Get
    End Property

    ' Reports the last error condition reported by the camera hardware or communications
    ' link.  The string may contain a text message or simply an error code.  The error
    ' value is cleared the next time any method is called.
    ' Must throw exception if no error condition.
    Public ReadOnly Property LastError() As String Implements ICamera.LastError
        Get
            Throw New PropertyNotImplementedException("LastError", False)
        End Get
    End Property

    ' Reports the actual exposure duration in seconds (i.e. shutter open time).  This
    ' may differ from the exposure time requested due to shutter latency, camera timing
    ' precision, etc.
    ' Must throw exception if not supported or no exposure has been taken
    Public ReadOnly Property LastExposureDuration() As Double Implements ICamera.LastExposureDuration
        Get
            Throw New PropertyNotImplementedException("LastExposureDuration", False)
        End Get
    End Property

    ' Reports the actual exposure start in the FITS-standard
    ' CCYY-MM-DDThh:mm:ss[.sss...] format.
    ' 
    ' Must throw exception if not supported or no exposure has been taken
    Public ReadOnly Property LastExposureStartTime() As String Implements ICamera.LastExposureStartTime
        Get
            Throw New PropertyNotImplementedException("LastExposureStartTime", False)
        End Get
    End Property

    ' Reports the maximum ADU value the camera can produce.
    ' Must throw exception if data unavailable.
    Public ReadOnly Property MaxADU() As Integer Implements ICamera.MaxADU
        Get
            Throw New PropertyNotImplementedException("MaxADU", False)
        End Get
    End Property

    ' If AsymmetricBinning = False, returns the maximum allowed binning factor. If
    ' AsymmetricBinning = True, returns the maximum allowed binning factor for the X axis.
    ' Must throw exception if data unavailable.
    Public ReadOnly Property MaxBinX() As Short Implements ICamera.MaxBinX
        Get
            Throw New PropertyNotImplementedException("MaxBinX", False)
        End Get
    End Property

    ' If AsymmetricBinning = False, equals MaxBinX. If AsymmetricBinning = True,
    ' returns the maximum allowed binning factor for the Y axis.
    ' Must throw exception if data unavailable.
    Public ReadOnly Property MaxBinY() As Short Implements ICamera.MaxBinY
        Get
            Throw New PropertyNotImplementedException("MaxBinY", False)
        End Get
    End Property

    ' Sets the subframe width. Also returns the current value.  If binning is active,
    ' value is in binned pixels.  No error check is performed when the value is set.
    ' Should default to CameraXSize.
    Public Property NumX() As Integer Implements ICamera.NumX
        Get
            Throw New PropertyNotImplementedException("NumX", False)
        End Get
        Set(ByVal value As Integer)
            Throw New PropertyNotImplementedException("NumX", True)
        End Set
    End Property

    ' Sets the subframe height. Also returns the current value.  If binning is active,
    ' value is in binned pixels.  No error check is performed when the value is set.
    ' Should default to CameraYSize.
    Public Property NumY() As Integer Implements ICamera.NumY
        Get
            Throw New PropertyNotImplementedException("NumY", False)
        End Get
        Set(ByVal value As Integer)
            Throw New PropertyNotImplementedException("NumY", True)
        End Set
    End Property

    ' Returns the width of the CCD chip pixels in microns, as provided by the camera driver.
    ' Must throw exception if data unavailable.
    Public ReadOnly Property PixelSizeX() As Double Implements ICamera.PixelSizeX
        Get
            Throw New PropertyNotImplementedException("PixelSizeX", False)
        End Get
    End Property

    ' Returns the height of the CCD chip pixels in microns, as provided by the camera
    ' driver.
    ' Must throw exception if data unavailable.
    Public ReadOnly Property PixelSizeY() As Double Implements ICamera.PixelSizeY
        Get
            Throw New PropertyNotImplementedException("PixelSizeY", False)
        End Get
    End Property

    ' Sets the camera cooler setpoint in degrees Celsius, and returns the current setpoint.
    ' Note:  camera hardware and/or driver should perform cooler ramping, to prevent
    ' thermal shock and potential damage to the CCD array or cooler stack.
    ' Must throw exception if command not successful.
    ' Must throw exception if CanSetCCDTemperature is False.
    Public Property SetCCDTemperature() As Double Implements ICamera.SetCCDTemperature
        Get
            Throw New PropertyNotImplementedException("SetCCDTemperature", False)
        End Get
        Set(ByVal value As Double)
            Throw New PropertyNotImplementedException("SetCCDTemperature", True)
        End Set
    End Property

    ' Sets the subframe start position for the X axis (0 based). Also returns the
    ' current value.  If binning is active, value is in binned pixels.
    Public Property StartX() As Integer Implements ICamera.StartX
        Get
            Throw New PropertyNotImplementedException("StartX", False)
        End Get
        Set(ByVal value As Integer)
            Throw New PropertyNotImplementedException("StartX", True)
        End Set
    End Property

    ' Sets the subframe start position for the Y axis (0 based). Also returns the
    ' current value.  If binning is active, value is in binned pixels.
    Public Property StartY() As Integer Implements ICamera.StartY
        Get
            Throw New PropertyNotImplementedException("StartY", False)
        End Get
        Set(ByVal value As Integer)
            Throw New PropertyNotImplementedException("StartY", True)
        End Set
    End Property

    ' Aborts the current exposure, if any, and returns the camera to Idle state.
    ' Must throw exception if camera is not idle and abort is unsuccessful (or not possible, e.g. during download).
    ' Must throw exception if hardware or communications error occurs.
    ' Must NOT throw an exception if the camera is already idle.
    Public Sub AbortExposure() Implements ICamera.AbortExposure
        Throw New MethodNotImplementedException("AbortExposure")
    End Sub

    ' This method returns only after the move has completed.
    '
    ' symbolic Constants
    ' The (symbolic) values for GuideDirections are:
    ' Constant     Value      Description
    ' --------     -----      -----------
    ' guideNorth     0        North (+ declination/elevation)
    ' guideSouth     1        South (- declination/elevation)
    ' guideEast      2        East (+ right ascension/azimuth)
    ' guideWest      3        West (+ right ascension/azimuth)
    '
    ' Note: directions are nominal and may depend on exact mount wiring.  guideNorth
    ' must be opposite guideSouth, and guideEast must be opposite guideWest.
    ' 
    ' Parameter: Direction of guide command
    ' Parameter: Duration of guide in milliseconds
    ' Must throw exception if PulseGuide command is unsupported
    ' Must throw exception if PulseGuide command is unsuccessful
    Public Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Integer) Implements ICamera.PulseGuide
        Throw New MethodNotImplementedException("PulseGuide")
    End Sub

    ' Launches a configuration dialog box for the driver.  The call will not return
    ' until the user clicks OK or cancel manually.
    ' Must throw an exception if Setup dialog is unavailable.
    Public Sub SetupDialog() Implements ICamera.SetupDialog
        Dim F As SetupDialogForm
        F = New SetupDialogForm()
        F.ShowDialog()
        F.Dispose()
        F = Nothing
    End Sub

    ' Starts an exposure. Use ImageReady to check when the exposure is complete.
    ' Must throw exception if NumX, NumY, XBin, YBin, StartX, StartY, or Duration parameters are invalid.
    ' Must throw exception if CanAsymmetricBin is False and BinX != BinY
    ' Must throw exception if the exposure cannot be started for any reason, such as a hardware or communications error
    Public Sub StartExposure(ByVal Duration As Double, ByVal Light As Boolean) Implements ICamera.StartExposure
        Throw New MethodNotImplementedException("StartExposure")
    End Sub

    ' Stops the current exposure, if any.  If an exposure is in progress, the readout
    ' process is initiated.  Ignored if readout is already in process.
    ' Must throw an exception if CanStopExposure is False
    ' Must throw an exception if no exposure is in progress
    ' Must throw an exception if the camera or link has an error condition
    ' Must throw an exception if for any reason no image readout will be available.
    Public Sub StopExposure() Implements ICamera.StopExposure
        Throw New MethodNotImplementedException("StopExposure")
    End Sub
#End Region
End Class
