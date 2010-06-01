//-----------------------------------------------------------------------
// <summary>Defines the ICamera Interface</summary>
//-----------------------------------------------------------------------
using System;
namespace ASCOM.Interfaces
{
    /// <summary>
    /// Defines the ICamera Interface
    /// </summary>
    public interface ICamera : IAscomDriver, IDeviceControl
    {
        /// <summary>
        /// Aborts the current exposure, if any, and returns the camera to Idle state.
        /// Must throw exception if camera is not idle and abort is
        /// unsuccessful (or not possible, e.g. during download).
        /// Must throw exception if hardware or communications error
        /// occurs.
        /// Must NOT throw an exception if the camera is already idle.
        /// </summary>
        void AbortExposure();

        /// <summary>
        /// Sets the binning factor for the X axis.  Also returns the current value.  Should
        /// default to 1 when the camera link is established.  Note:  driver does not check
        /// for compatible subframe values when this value is set; rather they are checked
        /// upon StartExposure.
        /// </summary>
        /// <value>BinX sets/gets the X binning value</value>
        /// <exception>Must throw an exception for illegal binning values</exception>
        short BinX { get; set; }

        /// <summary>
        /// Sets the binning factor for the Y axis  Also returns the current value.  Should
        /// default to 1 when the camera link is established.  Note:  driver does not check
        /// for compatible subframe values when this value is set; rather they are checked
        /// upon StartExposure.
        /// </summary>
        /// <value>The bin Y.</value>
        /// <exception>Must throw an exception for illegal binning values</exception>
        short BinY { get; set; }

        /// <summary>
        /// Returns one of the following status information:
        /// <list type="bullet">
        /// 		<listheader>
        /// 			<description>Value  State          Meaning</description>
        /// 		</listheader>
        /// 		<item>
        /// 			<description>0      CameraIdle      At idle state, available to start exposure</description>
        /// 		</item>
        /// 		<item>
        /// 			<description>1      CameraWaiting   Exposure started but waiting (for shutter, trigger,
        /// filter wheel, etc.)</description>
        /// 		</item>
        /// 		<item>
        /// 			<description>2      CameraExposing  Exposure currently in progress</description>
        /// 		</item>
        /// 		<item>
        /// 			<description>3      CameraReading   CCD array is being read out (digitized)</description>
        /// 		</item>
        /// 		<item>
        /// 			<description>4      CameraDownload  Downloading data to PC</description>
        /// 		</item>
        /// 		<item>
        /// 			<description>5      CameraError     Camera error condition serious enough to prevent
        /// further operations (link fail, etc.).</description>
        /// 		</item>
        /// 	</list>
        /// </summary>
        /// <value>The state of the camera.</value>
        /// <exception cref="System.Exception">Must return an exception if the camera status is unavailable.</exception>
        CameraStates CameraState { get; }

        /// <summary>
        /// Returns the width of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <value>The size of the camera X.</value>
        /// <exception cref="System.Exception">Must throw exception if the value is not known</exception>
        int CameraXSize { get; }

        /// <summary>
        /// Returns the height of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <value>The size of the camera Y.</value>
        /// <exception cref="System.Exception">Must throw exception if the value is not known</exception>
        int CameraYSize { get; }

        /// <summary>
        /// Returns True if the camera can abort exposures; False if not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can abort exposure; otherwise, <c>false</c>.
        /// </value>
        bool CanAbortExposure { get; }

        /// <summary>
        /// If True, the camera can have different binning on the X and Y axes, as
        /// determined by BinX and BinY. If False, the binning must be equal on the X and Y
        /// axes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can asymmetric bin; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="System.Exception">Must throw exception if the value is not known (n.b. normally only
        /// occurs if no link established and camera must be queried)</exception>
        bool CanAsymmetricBin { get; }

        /// <summary>
        /// If True, the camera's cooler power setting can be read.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can get cooler power; otherwise, <c>false</c>.
        /// </value>
        bool CanGetCoolerPower { get; }

        /// <summary>
        /// Returns True if the camera can send autoguider pulses to the telescope mount;
        /// False if not.  (Note: this does not provide any indication of whether the
        /// autoguider cable is actually connected.)
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can pulse guide; otherwise, <c>false</c>.
        /// </value>
        bool CanPulseGuide { get; }

        /// <summary>
        /// If True, the camera's cooler setpoint can be adjusted. If False, the camera
        /// either uses open-loop cooling or does not have the ability to adjust temperature
        /// from software, and setting the TemperatureSetpoint property has no effect.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can set CCD temperature; otherwise, <c>false</c>.
        /// </value>
        bool CanSetCCDTemperature { get; }

        /// <summary>
        /// Some cameras support StopExposure, which allows the exposure to be terminated
        /// before the exposure timer completes, but will still read out the image.  Returns
        /// True if StopExposure is available, False if not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the camera can stop the exposure; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref=" System.Exception">not supported</exception>
        /// <exception cref=" System.Exception">an error condition such as link failure is present</exception>
        bool CanStopExposure { get; }

        /// <summary>
        /// Returns the current CCD temperature in degrees Celsius. Only valid if
        /// CanControlTemperature is True.
        /// </summary>
        /// <value>The CCD temperature.</value>
        /// <exception>Must throw exception if data unavailable.</exception>
        double CCDTemperature { get; }

        /// <summary>
        /// Turns on and off the camera cooler, and returns the current on/off state.
        /// Warning: turning the cooler off when the cooler is operating at high delta-T
        /// (typically &gt;20C below ambient) may result in thermal shock.  Repeated thermal
        /// shock may lead to damage to the sensor or cooler stack.  Please consult the
        /// documentation supplied with the camera for further information.
        /// </summary>
        /// <value><c>true</c> if [cooler on]; otherwise, <c>false</c>.</value>
        /// <exception cref=" System.Exception">not supported</exception>
        /// <exception cref=" System.Exception">an error condition such as link failure is present</exception>
        bool CoolerOn { get; set; }

        /// <summary>
        /// Returns the present cooler power level, in percent.  Returns zero if CoolerOn is
        /// False.
        /// </summary>
        /// <value>The cooler power.</value>
        /// <exception cref=" System.Exception">not supported</exception>
        /// <exception cref=" System.Exception">an error condition such as link failure is present</exception>
        double CoolerPower { get; }

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Returns the gain of the camera in photoelectrons per A/D unit. (Some cameras have
        /// multiple gain modes; these should be selected via the SetupDialog and thus are
        /// static during a session.)
        /// </summary>
        /// <value>The electrons per ADU.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        double ElectronsPerADU { get; }

        /// <summary>
        /// Reports the full well capacity of the camera in electrons, at the current camera
        /// settings (binning, SetupDialog settings, etc.)
        /// </summary>
        /// <value>The full well capacity.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        double FullWellCapacity { get; }

        /// <summary>
        /// If True, the camera has a mechanical shutter. If False, the camera does not have
        /// a shutter.  If there is no shutter, the StartExposure command will ignore the
        /// Light parameter.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has shutter; otherwise, <c>false</c>.
        /// </value>
        bool HasShutter { get; }

        /// <summary>
        /// Returns the current heat sink temperature (called "ambient temperature" by some
        /// manufacturers) in degrees Celsius. Only valid if CanControlTemperature is True.
        /// </summary>
        /// <value>The heat sink temperature.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        double HeatSinkTemperature { get; }

        /// <summary>
        /// Returns a safearray of int of size NumX * NumY containing the pixel values from
        /// the last exposure. The application must inspect the Safearray parameters to
        /// determine the dimensions. Note: if NumX or NumY is changed after a call to
        /// StartExposure it will have no effect on the size of this array. This is the
        /// preferred method for programs (not scripts) to download iamges since it requires
        /// much less memory.
        /// For color or multispectral cameras, will produce an array of NumX * NumY *
        /// NumPlanes.  If the application cannot handle multispectral images, it should use
        /// just the first plane.
        /// </summary>
        /// <value>The image array.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        object ImageArray { get; }

        /// <summary>
        /// Returns a safearray of Variant of size NumX * NumY containing the pixel values
        /// from the last exposure. The application must inspect the Safearray parameters to
        /// determine the dimensions. Note: if NumX or NumY is changed after a call to
        /// StartExposure it will have no effect on the size of this array. This property
        /// should only be used from scripts due to the extremely high memory utilization on
        /// large image arrays (26 bytes per pixel). Pixels values should be in Short, int,
        /// or Double format.
        /// For color or multispectral cameras, will produce an array of NumX * NumY *
        /// NumPlanes.  If the application cannot handle multispectral images, it should use
        /// just the first plane.
        /// </summary>
        /// <value>The image array variant.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        object ImageArrayVariant { get; }

        /// <summary>
        /// If True, there is an image from the camera available. If False, no image
        /// is available and attempts to use the ImageArray method will produce an
        /// exception.
        /// </summary>
        /// <value><c>true</c> if [image ready]; otherwise, <c>false</c>.</value>
        /// <exception cref=" System.Exception">hardware or communications link error has occurred.</exception>
        bool ImageReady { get; }

        /// <summary>
        /// If True, pulse guiding is in progress. Required if the PulseGuide() method
        /// (which is non-blocking) is implemented. See the PulseGuide() method.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is pulse guiding; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref=" System.Exception">hardware or communications link error has occurred.</exception>
        bool IsPulseGuiding { get; }

        /// <summary>
        /// Reports the last error condition reported by the camera hardware or communications
        /// link.  The string may contain a text message or simply an error code.  The error
        /// value is cleared the next time any method is called.
        /// </summary>
        /// <value>The last error.</value>
        /// <exception cref=" System.Exception">Must throw exception if no error condition.</exception>
        string LastError { get; }


        /// <summary>
        /// Reports the actual exposure duration in seconds (i.e. shutter open time).  This
        /// may differ from the exposure time requested due to shutter latency, camera timing
        /// precision, etc.
        /// </summary>
        /// <value>The last duration of the exposure.</value>
        /// <exception cref=" System.Exception">Must throw exception if not supported or no exposure has been taken</exception>
        double LastExposureDuration { get; }

        /// <summary>
        /// Reports the actual exposure start in the FITS-standard
        /// CCYY-MM-DDThh:mm:ss[.sss...] format.
        /// </summary>
        /// <value>The last exposure start time.</value>
        /// <exception cref=" System.Exception">Must throw exception if not supported or no exposure has been taken</exception>
        string LastExposureStartTime { get; }

        /// <summary>
        /// Reports the maximum ADU value the camera can produce.
        /// </summary>
        /// <value>The max ADU.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        int MaxADU { get; }

        /// <summary>
        /// If AsymmetricBinning = False, returns the maximum allowed binning factor. If
        /// AsymmetricBinning = True, returns the maximum allowed binning factor for the X
        /// axis.
        /// </summary>
        /// <value>The max bin X.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        short MaxBinX { get; }

        /// <summary>
        /// If AsymmetricBinning = False, equals MaxBinX. If AsymmetricBinning = True,
        /// returns the maximum allowed binning factor for the Y axis.
        /// </summary>
        /// <value>The max bin Y.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        short MaxBinY { get; }

        /// <summary>
        /// Sets the subframe width. Also returns the current value.  If binning is active,
        /// value is in binned pixels.  No error check is performed when the value is set.
        /// Should default to CameraXSize.
        /// </summary>
        /// <value>The num X.</value>
        int NumX { get; set; }

        /// <summary>
        /// Sets the subframe height. Also returns the current value.  If binning is active,
        /// value is in binned pixels.  No error check is performed when the value is set.
        /// Should default to CameraYSize.
        /// </summary>
        /// <value>The num Y.</value>
        int NumY { get; set; }

        /// <summary>
        /// Returns the width of the CCD chip pixels in microns, as provided by the camera
        /// driver.
        /// </summary>
        /// <value>The pixel size X.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        double PixelSizeX { get; }

        /// <summary>
        /// Returns the height of the CCD chip pixels in microns, as provided by the camera
        /// driver.
        /// </summary>
        /// <value>The pixel size Y.</value>
        /// <exception cref=" System.Exception">Must throw exception if data unavailable.</exception>
        double PixelSizeY { get; }

        /// <summary>
        /// This method returns only after the move has completed.
        /// symbolic Constants
        /// The (symbolic) values for GuideDirections are:
        /// Constant     Value      Description
        /// --------     -----      -----------
        /// guideNorth     0        North (+ declination/elevation)
        /// guideSouth     1        South (- declination/elevation)
        /// guideEast      2        East (+ right ascension/azimuth)
        /// guideWest      3        West (+ right ascension/azimuth)
        /// Note: directions are nominal and may depend on exact mount wiring.  guideNorth
        /// must be opposite guideSouth, and guideEast must be opposite guideWest.
        /// </summary>
        /// <param name="Direction">The direction.</param>
        /// <param name="Duration">The duration.</param>
        /// <exception cref=" System.Exception">PulseGuide command is unsupported</exception>
        /// <exception cref=" System.Exception">PulseGuide command is unsuccessful</exception>
        void PulseGuide(GuideDirections Direction, int Duration);

        /// <summary>
        /// Sets the camera cooler setpoint in degrees Celsius, and returns the current
        /// setpoint.
        /// Note:  camera hardware and/or driver should perform cooler ramping, to prevent
        /// thermal shock and potential damage to the CCD array or cooler stack.
        /// </summary>
        /// <value>The set CCD temperature.</value>
        /// <exception cref=" System.Exception">Must throw exception if command not successful.</exception>
        /// <exception cref=" System.Exception">Must throw exception if CanSetCCDTemperature is False.</exception>
        double SetCCDTemperature { get; set; }

        /// <summary>
        /// Starts an exposure. Use ImageReady to check when the exposure is complete.
        /// </summary>
        /// <param name="Duration">Duration of exposure in seconds</param>
        /// <param name="Light">True for light frame, False for dark frame (ignored if no shutter)</param>
        /// <exception cref=" System.Exception">NumX, NumY, XBin, YBin, StartX, StartY, or Duration parameters are invalid.</exception>
        /// <exception cref=" System.Exception">CanAsymmetricBin is False and BinX != BinY</exception>
        /// <exception cref=" System.Exception">the exposure cannot be started for any reason, such as a hardware or communications error</exception>
        void StartExposure(double Duration, bool Light);

        /// <summary>
        /// Sets the subframe start position for the X axis (0 based). Also returns the
        /// current value.  If binning is active, value is in binned pixels.
        /// </summary>
        /// <value>The start X.</value>
        int StartX { get; set; }

        /// <summary>
        /// Sets the subframe start position for the Y axis (0 based). Also returns the
        /// current value.  If binning is active, value is in binned pixels.
        /// </summary>
        /// <value>The start Y.</value>
        int StartY { get; set; }

        /// <summary>
        /// Stops the current exposure, if any.  If an exposure is in progress, the readout
        /// process is initiated.  Ignored if readout is already in process.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if CanStopExposure is False</exception>
        /// <exception cref=" System.Exception">Must throw an exception if no exposure is in progress</exception>
        /// <exception cref=" System.Exception">Must throw an exception if the camera or link has an error condition</exception>
        /// <exception cref=" System.Exception">Must throw an exception if for any reason no image readout will be available.</exception>
        void StopExposure();

    }
}
