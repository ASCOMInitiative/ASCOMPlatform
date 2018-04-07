//-----------------------------------------------------------------------
// <summary>Defines the Camera class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - ImageArray returns type object, remove ImageArrayV which
//						was a Chris Rowland test/experiment. Can cast the object returned
//						by ImageArray into int[,]. Add COM releasing to Dispose().
// 01-Jan-10  	cdr     1.0.6 - Add Camera V2 properties as late bound properties.
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using System.Collections;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
//using ASCOM.Conform;
using System.Globalization;

namespace ASCOM.DriverAccess
{
    #region Camera Wrapper
    /// <summary>
    /// Implements a camera class to access any registered ASCOM Camera
    /// </summary>
    public class Camera : AscomDriver, ICameraV2
    {
        #region Camera constructors
        private readonly MemberFactory _memberFactory;
        private readonly short _driverInterfaceVersion;

        /// <summary>
        /// Creates an instance of the camera class.
        /// </summary>
        /// <param name="cameraId">The ProgID for the camera</param>
        public Camera(string cameraId)
            : base(cameraId)
        {
            _memberFactory = base.MemberFactory;
            try
            {
                _driverInterfaceVersion = InterfaceVersion;
            }
            catch (PropertyNotImplementedException)
            {
                _driverInterfaceVersion = 1;
            }
        }
        #endregion

        #region Convenience Members
        /// <summary>
        /// The Choose() method returns the DriverID of the selected driver.
        /// Choose() allows you to optionally pass the DriverID of a "current" driver,
        /// and the corresponding camera type is pre-selected in the Chooser///s list.
        /// In this case, the OK button starts out enabled (lit-up); the assumption is that the pre-selected driver has already been configured.
        /// </summary>
        /// <param name="cameraId">Optional DriverID of the previously selected camera that is to be the pre-selected camera in the list.</param>
        /// <returns>
        /// The DriverID of the user selected camera. Null if the dialog is canceled.
        /// </returns>
        public static string Choose(string cameraId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Camera";
                return chooser.Choose(cameraId);
            }
        }

        #endregion

        #region ICamera Members

        /// <summary>
        /// Aborts the current exposure, if any, and returns the camera to Idle state.
        /// </summary>
        /// <remarks>
        /// <b>NOTES:</b>
        /// <list type="bullet">
        /// <item><description>Must throw exception if camera is not idle and abort is unsuccessful (or not possible, e.g. during download).</description></item>
        /// <item><description>Must throw exception if hardware or communications error occurs.</description></item>
        /// <item><description>Must NOT throw an exception if the camera is already idle.</description></item>
        /// </list>
        /// </remarks>
        public void AbortExposure()
        {
            _memberFactory.CallMember(3, "AbortExposure", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Sets the binning factor for the X axis, also returns the current value.  
        /// </summary>
        /// <remarks>
        /// Should default to 1 when the camera connection is established.  Note:  driver does not check
        /// for compatible subframe values when this value is set; rather they are checked upon <see cref="StartExposure">StartExposure</see>.
        /// </remarks>
        /// <value>The X binning value</value>
        /// <exception cref="InvalidValueException">Must throw an exception for illegal binning values</exception>
        public short BinX
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "BinX", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "BinX", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Sets the binning factor for the Y axis, also returns the current value. 
        /// </summary>
        /// <remarks>
        /// Should default to 1 when the camera connection is established.  Note:  driver does not check
        /// for compatible subframe values when this value is set; rather they are checked upon <see cref="StartExposure">StartExposure</see>.
        /// </remarks>
        /// <value>The Y binning value.</value>
        /// <exception cref="InvalidValueException">Must throw an exception for illegal binning values</exception>
        public short BinY
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "BinY", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "BinY", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Returns the current CCD temperature in degrees Celsius.
        /// </summary>
        /// <remarks>
        /// Only valid if  <see cref="CanSetCCDTemperature" /> is True.
        /// </remarks>
        /// <value>The CCD temperature.</value>
        /// <exception cref="InvalidValueException">Must throw exception if data unavailable.</exception>
        public double CCDTemperature
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "CCDTemperature", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns the current camera operational state
        /// </summary>
        /// <remarks>
        /// Returns one of the following status information:
        /// <list type="bullet">
        /// <listheader><description>Value  State           Meaning</description></listheader>
        /// <item><description>0      CameraIdle      At idle state, available to start exposure</description></item>
        /// <item><description>1      CameraWaiting   Exposure started but waiting (for shutter, trigger, filter wheel, etc.)</description></item>
        /// <item><description>2      CameraExposing  Exposure currently in progress</description></item>
        /// <item><description>3      CameraReading   CCD array is being read out (digitized)</description></item>
        /// <item><description>4      CameraDownload  Downloading data to PC</description></item>
        /// <item><description>5      CameraError     Camera error condition serious enough to prevent further operations (connection fail, etc.).</description></item>
        /// </list>
        /// </remarks>
        /// <value>The state of the camera.</value>
        /// <exception cref="NotConnectedException">Must return an exception if the camera status is unavailable.</exception>
        public CameraStates CameraState
        {
            get { return (CameraStates)_memberFactory.CallMember(1, "CameraState", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Returns the width of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <value>The size of the camera X.</value>
        /// <exception cref="NotConnectedException">Must throw exception if the value is not known</exception>
        public int CameraXSize
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "CameraXSize", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns the height of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <value>The size of the camera Y.</value>
        /// <exception cref="NotConnectedException">Must throw exception if the value is not known</exception>
        public int CameraYSize
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "CameraYSize", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns True if the camera can abort exposures; False if not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can abort exposure; otherwise, <c>false</c>.
        /// </value>
        public bool CanAbortExposure
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CanAbortExposure", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns a flag showing whether this camera supports asymmetric binning
        /// </summary>
        /// <remarks>
        /// If True, the camera can have different binning on the X and Y axes, as
        /// determined by <see cref="BinX" /> and <see cref="BinY" />. If False, the binning must be equal on the X and Y axes.
        /// </remarks>
        /// <value>
        /// 	<c>true</c> if this instance can asymmetric bin; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="NotConnectedException">Must throw exception if the value is not known (n.b. normally only
        /// occurs if no connection established and camera must be queried)</exception>
        public bool CanAsymmetricBin
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CanAsymmetricBin", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// If True, the camera's cooler power setting can be read.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can get cooler power; otherwise, <c>false</c>.
        /// </value>
        public bool CanGetCoolerPower
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CanGetCoolerPower", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera supports pulse guiding
        /// </summary>
        /// <remarks>
        /// Returns True if the camera can send autoguider pulses to the telescope mount; False if not.  
        /// Note: this does not provide any indication of whether the autoguider cable is actually connected.
        /// </remarks>
        /// <value>
        /// 	<c>true</c> if this instance can pulse guide; otherwise, <c>false</c>.
        /// </value>
        public bool CanPulseGuide
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CanPulseGuide", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns a flag indicatig whether this camera supports setting the CCD temperature
        /// </summary>
        /// <remarks>
        /// If True, the camera's cooler setpoint can be adjusted. If False, the camera
        /// either uses open-loop cooling or does not have the ability to adjust temperature
        /// from software, and setting the <see cref="SetCCDTemperature" /> property has no effect.
        /// </remarks>
        /// <value>
        /// 	<c>true</c> if this instance can set CCD temperature; otherwise, <c>false</c>.
        /// </value>
        public bool CanSetCCDTemperature
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CanSetCCDTemperature", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera can stop an exposure that is in progress
        /// </summary>
        /// <remarks>
        /// Some cameras support <see cref="StopExposure" />, which allows the exposure to be terminated
        /// before the exposure timer completes, but will still read out the image.  Returns
        /// True if  <see cref="StopExposure" /> is available, False if not.
        /// </remarks>
        /// <value>
        /// 	<c>true</c> if the camera can stop the exposure; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref=" PropertyNotImplementedException">not supported</exception>
        /// <exception cref=" NotConnectedException">an error condition such as connection failure is present</exception>
        public bool CanStopExposure
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CanStopExposure", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Turns on and off the camera cooler, and returns the current on/off state.
        /// </summary>
        /// <remarks>
        /// <b>Warning:</b> turning the cooler off when the cooler is operating at high delta-T
        /// (typically &gt;20C below ambient) may result in thermal shock.  Repeated thermal
        /// shock may lead to damage to the sensor or cooler stack.  Please consult the
        /// documentation supplied with the camera for further information.
        /// </remarks>
        /// <value><c>true</c> if the cooler is on; otherwise, <c>false</c>.</value>
        /// <exception cref=" PropertyNotImplementedException">not supported</exception>
        /// <exception cref=" NotConnectedException">an error condition such as connection failure is present</exception>
        public bool CoolerOn
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CoolerOn", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "CoolerOn", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Returns the present cooler power level, in percent.
        /// </summary>
        /// <remarks>
        /// Returns zero if <see cref="CoolerOn" /> is False.
        /// </remarks>
        /// <value>The cooler power.</value>
        /// <exception cref=" PropertyNotImplementedException">not supported</exception>
        /// <exception cref=" NotConnectedException">an error condition such as connection failure is present</exception>
        public double CoolerPower
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "CoolerPower", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns the gain of the camera in photoelectrons per A/D unit.
        /// </summary>
        /// <remarks>
        /// Some cameras have multiple gain modes; these should be selected via the  <see cref="AscomDriver.SetupDialog" /> and thus are
        /// static during a session.
        /// </remarks>
        /// <value>The electrons per ADU.</value>
        /// <exception cref=" NotConnectedException">Must throw exception if data unavailable.</exception>
        public double ElectronsPerADU
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "ElectronsPerADU", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Reports the full well capacity of the camera in electrons, at the current camera settings (binning, SetupDialog settings, etc.)
        /// </summary>
        /// <value>The full well capacity.</value>
        /// <exception cref=" NotConnectedException">Must throw exception if data unavailable.</exception>
        public double FullWellCapacity
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "FullWellCapacity", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera has a mechanical shutter
        /// </summary>
        /// <remarks>
        /// If True, the camera has a mechanical shutter. If False, the camera does not have
        /// a shutter.  If there is no shutter, the  <see cref="StartExposure">StartExposure</see> command will ignore the
        /// Light parameter.
        /// </remarks>
        /// <value>
        /// 	<c>true</c> if this instance has shutter; otherwise, <c>false</c>.
        /// </value>
        public bool HasShutter
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "HasShutter", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns the current heat sink temperature (called "ambient temperature" by some manufacturers) in degrees Celsius. 
        /// </summary>
        /// <remarks>
        /// Only valid if  <see cref="CanSetCCDTemperature" /> is True.
        /// </remarks>
        /// <value>The heat sink temperature.</value>
        /// <exception cref=" NotConnectedException">Must throw exception if data unavailable.</exception>
        public double HeatSinkTemperature
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "HeatSinkTemperature", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns a safearray of int of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure. 
        /// </summary>
        /// <remarks>
        /// The application must inspect the Safearray parameters to determine the dimensions. 
        /// <para>Note: if <see cref="NumX" /> or <see cref="NumY" /> is changed after a call to <see cref="StartExposure">StartExposure</see> it will 
        /// have no effect on the size of this array. This is the preferred method for programs (not scripts) to download 
        /// iamges since it requires much less memory.</para>
        /// <para>For color or multispectral cameras, will produce an array of  <see cref="NumX" /> * <see cref="NumY" /> *
        /// NumPlanes.  If the application cannot handle multispectral images, it should use just the first plane.</para>
        /// </remarks>
        /// <value>The image array.</value>
        /// <exception cref=" NotConnectedException">Must throw exception if data unavailable.</exception>
        public object ImageArray
        {
            get { return _memberFactory.CallMember(1, "ImageArray", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Returns a safearray of Variant of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure. 
        /// </summary>
        /// <remarks>
        /// The application must inspect the Safearray parameters to
        /// determine the dimensions. Note: if <see cref="NumX" /> or <see cref="NumY" /> is changed after a call to
        /// <see cref="StartExposure">StartExposure</see> it will have no effect on the size of this array. This property
        /// should only be used from scripts due to the extremely high memory utilization on
        /// large image arrays (26 bytes per pixel). Pixels values should be in Short, int,
        /// or Double format.
        /// <para>For color or multispectral cameras, will produce an array of <see cref="NumX" /> * <see cref="NumY" /> *
        /// NumPlanes.  If the application cannot handle multispectral images, it should use
        /// just the first plane.</para>
        /// </remarks>
        /// <value>The image array variant.</value>
        /// <exception cref=" NotConnectedException">Must throw exception if data unavailable.</exception>
        public object ImageArrayVariant
        {
            get { return _memberFactory.CallMember(1, "ImageArrayVariant", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Returns a flag indicating whether the image is ready to be downloaded fom the camera
        /// </summary>
        /// <remarks>
        /// If True, there is an image from the camera available. If False, no image
        /// is available and attempts to use the <see cref="ImageArray" /> method will produce an exception
        /// </remarks>.
        /// <value><c>true</c> if [image ready]; otherwise, <c>false</c>.</value>
        /// <exception cref=" NotConnectedException">hardware or communications connection error has occurred.</exception>
        public bool ImageReady
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "ImageReady", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns a flag indicating whether the camera is currrently in a <see cref="PulseGuide">PulseGuide</see> operation.
        /// </summary>
        /// <remarks>
        /// If True, pulse guiding is in progress. Required if the <see cref="PulseGuide">PulseGuide</see> method
        /// (which is non-blocking) is implemented. See the <see cref="PulseGuide">PulseGuide</see> method.
        /// </remarks>
        /// <value>
        /// 	<c>true</c> if this instance is pulse guiding; otherwise, <c>false</c>.
        /// </value> 
        /// <exception cref=" NotConnectedException">hardware or communications connection error has occurred.</exception>
        public bool IsPulseGuiding
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "IsPulseGuiding", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Reports the actual exposure duration in seconds (i.e. shutter open time).  
        /// </summary>
        /// <remarks>
        /// This may differ from the exposure time requested due to shutter latency, camera timing precision, etc.
        /// </remarks>
        /// <value>The last duration of the exposure.</value>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if not supported</exception>
        /// <exception cref="InvalidOperationException">If called before any exposure has been taken</exception>
        public double LastExposureDuration
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "LastExposureDuration", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Reports the actual exposure start in the FITS-standard CCYY-MM-DDThh:mm:ss[.sss...] format.
        /// The time must be UTC.
        /// </summary>
        /// <value>The last exposure start time in UTC.</value>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if not supported</exception>
        /// <exception cref="InvalidOperationException">If called before any exposure has been taken</exception>
        public string LastExposureStartTime
        {
            get { return Convert.ToString(_memberFactory.CallMember(1, "LastExposureStartTime", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Reports the maximum ADU value the camera can produce.
        /// </summary>
        /// <value>The maximum ADU.</value>
        /// <exception cref="NotConnectedException">Must throw exception if data unavailable.</exception>
        public int MaxADU
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "MaxADU", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns the maximum allowed binning for the X camera axis
        /// </summary>
        /// <remarks>
        /// If <see cref="CanAsymmetricBin" /> = False, returns the maximum allowed binning factor. If
        /// <see cref="CanAsymmetricBin" /> = True, returns the maximum allowed binning factor for the X axis.
        /// </remarks>
        /// <value>The max bin X.</value>
        /// <exception cref="NotConnectedException">Must throw exception if data unavailable.</exception>
        public short MaxBinX
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "MaxBinX", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns the maximum allowed binning for the Y camera axis
        /// </summary>
        /// <remarks>
        /// If <see cref="CanAsymmetricBin" /> = False, equals <see cref="MaxBinX" />. If <see cref="CanAsymmetricBin" /> = True,
        /// returns the maximum allowed binning factor for the Y axis.
        /// </remarks>
        /// <value>The max bin Y.</value>
        /// <exception cref="NotConnectedException">Must throw exception if data unavailable.</exception>
        public short MaxBinY
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "MaxBinY", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Sets the subframe width. Also returns the current value.  
        /// </summary>
        /// <remarks>
        /// If binning is active, value is in binned pixels.  No error check is performed when the value is set. 
        /// Should default to <see cref="CameraXSize" />.
        /// </remarks>
        /// <value>The num X.</value>
        public int NumX
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "NumX", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "NumX", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Sets the subframe height. Also returns the current value.
        /// </summary>
        /// <remarks>
        /// If binning is active,
        /// value is in binned pixels.  No error check is performed when the value is set.
        /// Should default to <see cref="CameraYSize" />.
        /// </remarks>
        /// <value>The num Y.</value>
        public int NumY
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "NumY", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "NumY", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Returns the width of the CCD chip pixels in microns.
        /// </summary>
        /// <value>The pixel size X.</value>
        /// <exception cref="NotConnectedException">Must throw exception if data unavailable.</exception>
        public double PixelSizeX
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "PixelSizeX", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns the height of the CCD chip pixels in microns.
        /// </summary>
        /// <value>The pixel size Y.</value>
        /// <exception cref="NotConnectedException">Must throw exception if data unavailable.</exception>
        public double PixelSizeY
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "PixelSizeY", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Activates the Camera's mount control sytem to instruct the mount to move in a particular direction for a given period of time
        /// </summary>
        /// <remarks>
        /// This method returns only after the move has completed.
        /// <para>
        /// The (symbolic) values for GuideDirections are:
        /// <list type="bullet">
        /// <listheader><description>Constant     Value      Description</description></listheader>
        /// <item><description>guideNorth     0        North (+ declination/elevation)</description></item>
        /// <item><description>guideSouth     1        South (- declination/elevation)</description></item>
        /// <item><description>guideEast      2        East (+ right ascension/azimuth)</description></item>
        /// <item><description>guideWest      3        West (+ right ascension/azimuth)</description></item>
        /// </list>
        /// </para>
        /// <para>Note: directions are nominal and may depend on exact mount wiring.  
        /// <see cref="GuideDirections.guideNorth" /> must be opposite <see cref="GuideDirections.guideSouth" />, and 
        /// <see cref="GuideDirections.guideEast" /> must be opposite <see cref="GuideDirections.guideWest" />.</para>
        /// </remarks>
        /// <param name="Direction">The direction of movement.</param>
        /// <param name="Duration">The duration of movement in milli-seconds.</param>
        /// <exception cref="MethodNotImplementedException">PulseGuide command is unsupported</exception>
        /// <exception cref=" DriverException">PulseGuide command is unsuccessful</exception>
        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            _memberFactory.CallMember(3, "PulseGuide", new[] { typeof(GuideDirections), typeof(int) }, new object[] { Direction, Duration });
        }

        /// <summary>
        /// Sets the camera cooler setpoint in degrees Celsius, and returns the current setpoint.
        /// </summary>
        /// <remarks>
        /// <para>The driver should throw an <see cref="InvalidValueException" /> if an attempt is made to set <see cref="SetCCDTemperature" /> 
        /// outside the valid range for the camera. As an assitance to driver authors, to protect equipment and prevent harm to individuals, 
        /// Conform will report an issue if it is possible to set <see cref="SetCCDTemperature" /> below -280C or above +100C.</para>
        /// <b>Note:</b>  Camera hardware and/or driver should perform cooler ramping, to prevent
        /// thermal shock and potential damage to the CCD array or cooler stack.
        /// </remarks>
        /// <value>The set CCD temperature.</value>
        /// <exception cref="DriverException">Must throw exception if command not successful.</exception>
        /// <exception cref="InvalidValueException">Must throw an InvalidValueException if an attempt is made to set a value is outside the 
        /// camera's valid termperature setpoint range.</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw exception if <see cref="CanSetCCDTemperature" /> is False.</exception>
        public double SetCCDTemperature
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "SetCCDTemperature", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "SetCCDTemperature", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Starts an exposure. Use <see cref="ImageReady" /> to check when the exposure is complete.
        /// </summary>
        /// <remarks>
        /// <para>A dark frame or bias exposure may be shorter than the V2 <see cref="ExposureMin" /> value and for a bias frame can be zero.
        /// Check the value of <see cref="StartExposure">Light</see> and allow exposures down to 0 seconds 
        /// if <see cref="StartExposure">Light</see> is False.  If the hardware will not
        /// support an exposure duration of zero then, for dark and bias frames, set it to the minimum that is possible.</para>
        /// <para>Some applications will set an exposure time of zero for bias frames so it's important that the driver allows this.</para>
        /// </remarks>
        /// <param name="Duration">Duration of exposure in seconds, can be zero if <see cref="StartExposure">Light</see> is false</param>
        /// <param name="Light">True for light frame, False for dark frame (ignored if no shutter)</param>
        /// <exception cref=" InvalidValueException"><see cref="NumX" />, <see cref="NumY" />, <see cref="BinX" />, 
        /// <see cref="BinY" />, <see cref="StartX" />, <see cref="StartY" />, or <see cref="StartExposure">Duration</see> parameters are invalid.</exception>
        /// <exception cref=" InvalidOperationException"><see cref="CanAsymmetricBin" /> is False and <see cref="BinX" /> != <see cref="BinY" /></exception>
        /// <exception cref="NotConnectedException">the exposure cannot be started for any reason, such as a hardware or communications error</exception>
        public void StartExposure(double Duration, bool Light)
        {
            _memberFactory.CallMember(3, "StartExposure", new[] { typeof(double), typeof(bool) }, new object[] { Duration, Light });
        }

        /// <summary>
        /// Sets the subframe start position for the X axis (0 based) and returns the current value.
        /// </summary>
        /// <remarks>
        /// If binning is active, value is in binned pixels.
        /// </remarks>
        /// <value>The start X.</value>
        public int StartX
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "StartX", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "StartX", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Sets the subframe start position for the Y axis (0 based). Also returns the current value.  
        /// </summary>
        /// <remarks>
        /// If binning is active, value is in binned pixels.
        /// </remarks>
        /// <value>The start Y.</value>
        public int StartY
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "StartY", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "StartY", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Stops the current exposure, if any.
        /// </summary>
        /// <remarks>
        /// If an exposure is in progress, the readout process is initiated.  Ignored if readout is already in process.
        /// </remarks>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if CanStopExposure is False</exception>
        /// <exception cref="NotConnectedException">Must throw an exception if the camera or connection has an error condition</exception>
        /// <exception cref="DriverException">Must throw an exception if for any reason no image readout will be available.</exception>
        public void StopExposure()
        {
            _memberFactory.CallMember(3, "StopExposure", new Type[] { }, new object[] { });
        }

        #endregion

        #region ICameraV2 members
        /// <summary>
        /// Bayer X offset index, Interface Version 2 only
        /// </summary>
        /// <returns>The Bayer colour matrix X offset, as defined in <see cref="SensorType" />.</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
        /// <remarks>Returns the X offset of the Bayer matrix, as defined in <see cref="SensorType" />. Value returned must be in the range 0 to M-1, 
        /// where M is the width of the Bayer matrix. The offset is relative to the 0,0 pixel in the sensor array, and does not change to 
        /// reflect subframe settings.
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public short BayerOffsetX
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "BayerOffsetX", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns the Y offset of the Bayer matrix, as defined in <see cref="SensorType" />, Interface Version 2 only
        /// </summary>
        /// <returns>The Bayer colour matrix Y offset.</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
        /// <remarks>The offset is relative to the 0,0 pixel in the sensor array, and does not change to reflect subframe settings. 
        /// <para>It is recommended that this function be called only after a <see cref="AscomDriver.Connected">connection</see> is established with 
        /// the camera hardware, to ensure that the driver is aware of the capabilities of the specific camera model.</para>
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public short BayerOffsetY
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "BayerOffsetY", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Camera has a fast readout mode, Interface Version 2 only
        /// </summary>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <returns><c>true</c> when the camera supports a fast readout mode</returns>
        /// <remarks>It is recommended that this function be called only after a <see cref="AscomDriver.Connected">connection</see> is established with the camera hardware, to 
        /// ensure that the driver is aware of the capabilities of the specific camera model.
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public bool CanFastReadout
        {
            get
            {
                if (_driverInterfaceVersion > 1)
                {
                    return Convert.ToBoolean(_memberFactory.CallMember(1, "CanFastReadout", new Type[] { }, new object[] { }));
                }
                return false;
            }
        }

        /// <summary>
        /// Returns the maximum exposure time supported by <see cref="StartExposure">StartExposure</see>, Interface Version 2 only
        /// </summary>
        /// <returns>The maximum exposure time, in seconds, that the camera supports</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
        /// <remarks>It is recommended that this function be called only after 
        /// a <see cref="AscomDriver.Connected">connection</see> is established with the camera hardware, to ensure that the driver is aware of the capabilities of the 
        /// specific camera model.
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public double ExposureMax
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "ExposureMax", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Minimium exposure time, Interface Version 2 only
        /// </summary>
        /// <returns>The minimum exposure time, in seconds, that the camera supports through <see cref="StartExposure">StartExposure</see></returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
        /// <remarks>This must be a non-zero number representing the shortest possible exposure time supported by the camera model.
        /// <para>Please note that for bias frame acquisition an even shorter exposure may be possible; please see <see cref="StartExposure">StartExposure</see> 
        /// for more information.</para>
        /// <para>It is recommended that this function be called only after a <see cref="AscomDriver.Connected">connection</see> is established with the camera hardware, to ensure 
        /// that the driver is aware of the capabilities of the specific camera model.</para>
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public double ExposureMin
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "ExposureMin", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Exposure resolution, Interface Version 2 only
        /// </summary>
        /// <returns>The smallest increment in exposure time supported by <see cref="StartExposure">StartExposure</see>.</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
        /// <remarks>This can be used, for example, to specify the resolution of a user interface "spin control" used to dial in the exposure time.
        /// <para>Please note that the Duration provided to <see cref="StartExposure">StartExposure</see> does not have to be an exact multiple of this number; 
        /// the driver should choose the closest available value. Also in some cases the resolution may not be constant over the full range 
        /// of exposure times; in this case the smallest increment would be appropriate. </para>
        /// <para>It is recommended that this function be called only after a <see cref="AscomDriver.Connected">connection</see> is established with the camera hardware, to ensure 
        /// that the driver is aware of the capabilities of the specific camera model.</para>
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public double ExposureResolution
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "ExposureResolution", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Fast readout mode, Interface Version 2 only
        /// </summary>
        /// <value>True sets fast readout mode, false sets normal mode</value>
        /// <returns>True when the current readout mode is fast and false when the readout mode is normal.</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if <see cref="CanFastReadout" /> returns False.</exception>
        /// <remarks>Must thrown an exception if no <see cref="AscomDriver.Connected">connection</see> is established to the camera. Must throw 
        /// an exception if <see cref="CanFastReadout" /> returns False.
        /// <para>Many cameras have a "fast mode" intended for use in focusing. When set to True, the camera will operate in Fast mode; when 
        /// set False, the camera will operate normally. This property should default to False.</para>
        /// <para>Please note that this function may in some cases interact with <see cref="ReadoutModes" />; for example, there may be modes where 
        /// the Fast/Normal switch is meaningless. In this case, it may be preferable to use the <see cref="ReadoutModes" /> function to control 
        /// fast/normal switching.</para>
        /// <para>If this feature is not available, then <see cref="CanFastReadout" /> must return False.</para>
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public bool FastReadout
        {
            set { _memberFactory.CallMember(2, "FastReadout", new Type[] { }, new object[] { value }); }
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "FastReadout", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Index into the <see cref="Gains" /> array for the selected camera gain, Interface Version 2 only
        /// </summary>
        /// <value>Short integer index for the current camera gain in the <see cref="Gains" /> string array.</value>
        /// <returns>Index into the Gains array for the selected camera gain</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if gain is not supported</exception>
        /// <remarks>
        /// <see cref="Gain" /> can be used to adjust the gain setting of the camera, if supported. There are two typical usage scenarios:
        /// <ul>
        /// <li>DSLR Cameras - <see cref="Gains" /> will return a 0-based array of strings, which correspond to different gain settings such as 
        /// "ISO 800". <see cref="Gain" /> must be set to an integer in this range. <see cref="GainMin" /> and <see cref="GainMax" /> must thrown an exception if 
        /// this mode is used.</li>
        /// <li>Adjustable gain CCD cameras - <see cref="GainMin" /> and <see cref="GainMax" /> return integers, which specify the valid range for <see cref="GainMin" /> and <see cref="Gain" />.</li>
        /// </ul>
        ///<para>The driver must default <see cref="Gain" /> to a valid value. </para>
        ///<para>Please note that <see cref="ReadoutMode" /> may in some cases affect the gain of the camera; if so the driver must be written such 
        /// that the two properties do not conflict if both are used.</para>
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public short Gain
        {
            set { _memberFactory.CallMember(2, "Gain", new Type[] { }, new object[] { value }); }
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "Gain", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Maximum value of <see cref="Gain" />, Interface Version 2 only
        /// </summary>
        /// <value>Short integer representing the maximum gain value supported by the camera.</value>
        /// <returns>The maximum gain value that this camera supports</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if gainmax is not supported</exception>
        /// <remarks>When specifying the gain setting with an integer value, <see cref="GainMax" /> is used in conjunction with <see cref="GainMin" /> to 
        /// specify the range of valid settings.
        /// <para><see cref="GainMax" /> shall be greater than <see cref="GainMin" />. If either is available, then both must be available.</para>
        /// <para>Please see <see cref="Gain" /> for more information.</para>
        /// <para>It is recommended that this function be called only after a <see cref="AscomDriver.Connected">connection</see> is established with the camera hardware, to ensure 
        /// that the driver is aware of the capabilities of the specific camera model.</para>
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public short GainMax
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "GainMax", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Minimum value of <see cref="Gain" />, Interface Version 2 only
        /// </summary>
        /// <returns>The minimum gain value that this camera supports</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
        /// <remarks>When specifying the gain setting with an integer value, <see cref="GainMin" /> is used in conjunction with <see cref="GainMax" /> to 
        /// specify the range of valid settings.
        /// <para><see cref="GainMax" /> shall be greater than <see cref="GainMin" />. If either is available, then both must be available.</para>
        /// <para>Please see <see cref="Gain" /> for more information.</para>
        /// <para>It is recommended that this function be called only after a <see cref="AscomDriver.Connected">connection</see> is established with the camera hardware, to ensure 
        /// that the driver is aware of the capabilities of the specific camera model.</para>
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public short GainMin
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "GainMin", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Gains supported by the camera, Interface Version 2 only
        /// </summary>
        /// <returns>An ArrayList of gain names </returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
        /// <remarks><see cref="Gains" /> provides a 0-based array of available gain settings.  This is often used to specify ISO settings for DSLR cameras.  
        /// Typically the application software will display the available gain settings in a drop list. The application will then supply 
        /// the selected index to the driver via the <see cref="Gain" /> property. 
        /// <para>The <see cref="Gain" /> setting may alternatively be specified using integer values; if this mode is used then <see cref="Gains" /> is invalid 
        /// and must throw an exception. Please see <see cref="GainMax" /> and <see cref="GainMin" /> for more information.</para>
        /// <para>It is recommended that this function be called only after a <see cref="AscomDriver.Connected">connection</see> is established with the camera hardware, 
        /// to ensure that the driver is aware of the capabilities of the specific camera model.</para>
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public ArrayList Gains
        {
            get { return (ArrayList)_memberFactory.CallMember(1, "Gains", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Percent conpleted, Interface Version 2 only
        /// </summary>
        /// <returns>A value between 0 and 100% indicating the completeness of this operation</returns>
        /// <exception cref="InvalidOperationException">Thrown when it is inappropriate to call <see cref="PercentCompleted" /></exception>
        /// <remarks>If valid, returns an integer between 0 and 100, where 0 indicates 0% progress (function just started) and 
        /// 100 indicates 100% progress (i.e. completion).
        /// <para>At the discretion of the driver author, <see cref="PercentCompleted" /> may optionally be valid 
        /// when <see cref="CameraState" /> is in any or all of the following 
        /// states: <see cref="CameraStates.cameraExposing" />, 
        /// <see cref="CameraStates.cameraWaiting" />, <see cref="CameraStates.cameraReading" /> 
        /// or <see cref="CameraStates.cameraDownload" />. In all other states an exception shall be thrown.</para>
        /// <para>Typically the application user interface will show a progress bar based on the <see cref="PercentCompleted" /> value.</para>
        /// <para>Please note that client applications are not required to use this value, and in some cases may display status 
        /// information based on other information, such as time elapsed.</para>
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public short PercentCompleted
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "PercentCompleted", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Readout mode, Interface Version 2 only
        /// </summary>
        /// <value></value>
        /// <returns>Short integer index into the <see cref="ReadoutModes">ReadoutModes</see> array of string readout mode names indicating 
        /// the camera's current readout mode.</returns>
        /// <exception cref="InvalidValueException">Must throw an exception if set to an illegal or unavailable mode.</exception>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <remarks><see cref="ReadoutMode" /> is an index into the array <see cref="ReadoutModes" />, and selects the desired readout mode for the camera.  
        /// Defaults to 0 if not set.  Throws an exception if the selected mode is not available.
        /// <para>It is strongly recommended, but not required, that driver authors make the 0-index mode suitable for standard imaging operations, 
        /// since it is the default.</para>
        /// <para>Please see <see cref="ReadoutModes" /> for additional information.</para>
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public short ReadoutMode
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "ReadoutMode", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "ReadoutMode", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// List of available readout modes, Interface Version 2 only
        /// </summary>
        /// <returns>An ArrayList of readout mode names</returns>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if gainmin is not supported</exception>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <remarks>This property provides an array of strings, each of which describes an available readout mode of the camera.  
        /// At least two strings must be present in the list. The user interface of a control application will typically present to the 
        /// user a drop-list of modes.  The choice of available modes made available is entirely at the discretion of the driver author. 
        /// Please note that if the camera has many different modes of operation, then the most commonly adjusted settings should be in 
        /// <see cref="ReadoutModes" />; additional settings may be provided using <see cref="AscomDriver.SetupDialog" />.
        /// <para>To select a mode, the application will set <see cref="ReadoutMode" /> to the index of the desired mode.  The index is zero-based.</para>
        /// <para>This property should only be read while a <see cref="AscomDriver.Connected">connection</see> to the camera is actually established.  Drivers often support 
        /// multiple cameras with different capabilities, which are not known until the <see cref="AscomDriver.Connected">connection</see> is made.  If the available readout modes 
        /// are not known because no <see cref="AscomDriver.Connected">connection</see> has been established, this property shall throw an exception.</para>
        /// <para>Please note that the default <see cref="ReadoutMode" /> setting is 0. It is strongly recommended, but not required, that 
        /// driver authors use the 0-index mode for standard imaging operations, since it is the default.</para>
        /// <para>This feature may be used in parallel with <see cref="FastReadout" />; however, care should be taken to ensure that the two 
        /// features work together consistently. If there are modes that are inconsistent having a separate fast/normal switch, then it 
        /// may be better to simply list Fast as one of the <see cref="ReadoutModes" />.</para>
        /// <para>It is recommended that this function be called only after a <see cref="AscomDriver.Connected">connection</see> is established with 
        /// the camera hardware, to ensure that the driver is aware of the capabilities of the specific camera model.</para>
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public ArrayList ReadoutModes
        {
            get { return (ArrayList)_memberFactory.CallMember(1, "ReadoutModes", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// Sensor name, Interface Version 2 only
        /// </summary>
        /// <returns>The name of sensor used within the camera</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <remarks>Returns the name (datasheet part number) of the sensor, e.g. ICX285AL.  The format is to be exactly as shown on 
        /// manufacturer data sheet, subject to the following rules. All letter shall be uppercase.  Spaces shall not be included.
        /// <para>Any extra suffixes that define region codes, package types, temperature range, coatings, grading, color/monochrome, 
        /// etc. shall not be included. For color sensors, if a suffix differentiates different Bayer matrix encodings, it shall be 
        /// included.</para>
        /// <para>Examples:</para>
        /// <list type="bullet">
        /// <item><description>ICX285AL-F shall be reported as ICX285</description></item>
        /// <item><description>KAF-8300-AXC-CD-AA shall be reported as KAF-8300</description></item>
        /// </list>
        /// <para><b>Note:</b></para>
        /// <para>The most common usage of this property is to select approximate color balance parameters to be applied to 
        /// the Bayer matrix of one-shot color sensors.  Application authors should assume that an appropriate IR cutoff filter is 
        /// in place for color sensors.</para>
        /// <para>It is recommended that this function be called only after a <see cref="AscomDriver.Connected">connection</see> is established with 
        /// the camera hardware, to ensure that the driver is aware of the capabilities of the specific camera model.</para>
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// </remarks>
        public string SensorName
        {
            get { return Convert.ToString(_memberFactory.CallMember(1, "SensorName", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Type of colour information returned by the the camera sensor, Interface Version 2 only
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="ASCOM.DeviceInterface.SensorType" /> enum value of the camera sensor</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an 
        /// active <see cref="AscomDriver.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <remarks>
        /// <para>This is only available for the Camera Interface Version 2</para>
        /// <para><see cref="SensorType" /> returns a value indicating whether the sensor is monochrome, or what Bayer matrix it encodes.  
        /// The following values are defined:</para>
        /// <para>
        /// <table style="width:76.24%;" cellspacing="0" width="76.24%">
        /// <col style="width: 11.701%;"></col>
        /// <col style="width: 20.708%;"></col>
        /// <col style="width: 67.591%;"></col>
        /// <tr>
        /// <td colspan="1" rowspan="1" style="width: 11.701%; padding-right: 10px; padding-left: 10px; 
        /// border-left-color: #000000; border-left-style: Solid; 
        /// border-top-color: #000000; border-top-style: Solid; 
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        /// background-color: #00ffff;" width="11.701%">
        /// <b>Value</b></td>
        /// <td colspan="1" rowspan="1" style="width: 20.708%; padding-right: 10px; padding-left: 10px; 
        /// border-top-color: #000000; border-top-style: Solid; 
        /// border-right-style: Solid; border-right-color: #000000; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        /// background-color: #00ffff;" width="20.708%">
        /// <b>Enumeration</b></td>
        /// <td colspan="1" rowspan="1" style="width: 67.591%; padding-right: 10px; padding-left: 10px; 
        /// border-top-color: #000000; border-top-style: Solid; 
        /// border-right-style: Solid; border-right-color: #000000; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; 
        /// background-color: #00ffff;" width="67.591%">
        /// <b>Meaning</b></td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-left-color: #000000; border-left-style: Solid; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 0</td>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Monochrome</td>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Camera produces monochrome array with no Bayer encoding</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-left-color: #000000; border-left-style: Solid; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 1</td>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Colour</td>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Camera produces color image directly, requiring not Bayer decoding</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-left-color: #000000; border-left-style: Solid; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 2</td>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// RGGB</td>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Camera produces RGGB encoded Bayer array images</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-left-color: #000000; border-left-style: Solid; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 3</td>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// CMYG</td>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Camera produces CMYG encoded Bayer array images</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-left-color: #000000; border-left-style: Solid; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 4</td>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// CMYG2</td>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Camera produces CMYG2 encoded Bayer array images</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-left-color: #000000; border-left-style: Solid; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 5</td>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// LRGB</td>
        /// <td style="padding-right: 10px; padding-left: 10px; 
        /// border-right-color: #000000; border-right-style: Solid; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; 
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Camera produces Kodak TRUESENSE Bayer LRGB array images</td>
        /// </tr>
        /// </table>
        /// </para>
        /// <para>Please note that additional values may be defined in future updates of the standard, as new Bayer matrices may be created 
        /// by sensor manufacturers in the future.  If this occurs, then a new enumeration value shall be defined. The pre-existing enumeration 
        /// values shall not change.
        /// <para><see cref="SensorType" /> can possibly change between exposures, for example if <see cref="ReadoutMode">Camera.ReadoutMode</see> is changed, and should always be checked after each exposure.</para>
        /// <para>In the following definitions, R = red, G = green, B = blue, C = cyan, M = magenta, Y = yellow.  The Bayer matrix is 
        /// defined with X increasing from left to right, and Y increasing from top to bottom. The pattern repeats every N x M pixels for the 
        /// entire pixel array, where N is the height of the Bayer matrix, and M is the width.</para>
        /// <para>RGGB indicates the following matrix:</para>
        /// </para>
        /// <para>
        /// <table style="width:41.254%;" cellspacing="0" width="41.254%">
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #ffffff" width="10%">
        /// </td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>X = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>X = 1</b></td>
        /// </tr>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff" width="10%">
        /// <b>Y = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// R</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// " width="10%">
        /// G</td>
        /// </tr>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// background-color: #00ffff;" width="10%">
        /// <b>Y = 1</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// " width="10%">
        /// G</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// " width="10%">
        /// B</td>
        /// </tr>
        /// </table>
        /// </para>
        /// 
        /// <para>CMYG indicates the following matrix:</para>
        /// <para>
        /// <table style="width:41.254%;" cellspacing="0" width="41.254%">
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #ffffff" width="10%">
        /// </td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>X = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>X = 1</b></td>
        /// </tr>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff" width="10%">
        /// <b>Y = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// Y</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// " width="10%">
        /// C</td>
        /// </tr>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// background-color: #00ffff;" width="10%">
        /// <b>Y = 1</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// " width="10%">
        /// G</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// " width="10%">
        /// M</td>
        /// </tr>
        /// 
        /// </table>
        /// </para>
        /// <para>CMYG2 indicates the following matrix:</para>
        /// <para>
        /// <table style="width:41.254%;" cellspacing="0" width="41.254%">
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #ffffff" width="10%">
        /// </td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>X = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>X = 1</b></td>
        /// </tr>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff" width="10%">
        /// <b>Y = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// C</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// " width="10%">
        /// Y</td>
        /// </tr>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>Y = 1</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// M</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// " width="10%">
        /// G</td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff" width="10%">
        /// <b>Y = 2</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// C</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// " width="10%">
        /// Y</td>
        /// </tr>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// background-color: #00ffff;" width="10%">
        /// <b>Y = 3</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// " width="10%">
        /// G</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// " width="10%">
        /// M</td>
        /// </tr>
        /// </table>
        /// </para>
        /// 
        /// <para>LRGB indicates the following matrix (Kodak TRUESENSE):</para>
        /// <para>
        /// <table style="width:68.757%;" cellspacing="0" width="68.757%">
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #ffffff" width="10%">
        /// </td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>X = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>X = 1</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>X = 2</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>X = 3</b></td>
        /// </tr>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff" width="10%">
        /// <b>Y = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// L</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// R</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// L</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// " width="10%">
        /// G</td>
        /// </tr>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>Y = 1</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// R</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// L</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// G</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// " width="10%">
        /// L</td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff" width="10%">
        /// <b>Y = 2</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// L</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// G</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// L</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// " width="10%">
        /// B</td>
        /// </tr>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// background-color: #00ffff;" width="10%">
        /// <b>Y = 3</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// " width="10%">
        /// G</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// " width="10%">
        /// L</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// " width="10%">
        /// B</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// " width="10%">
        /// L</td>
        /// </tr>
        /// </table>
        /// </para>
        /// 
        /// <para>The alignment of the array may be modified by <see cref="BayerOffsetX" /> and <see cref="BayerOffsetY" />. 
        /// The offset is measured from the 0,0 position in the sensor array to the upper left corner of the Bayer matrix table. 
        /// Please note that the Bayer offset values are not affected by subframe settings.</para>
        /// <para>For example, if a CMYG2 sensor has a Bayer matrix offset as shown below, <see cref="BayerOffsetX" /> is 0 and <see cref="BayerOffsetY" /> is 1:</para>
        ///<para>
        /// <table style="width:41.254%;" cellspacing="0" width="41.254%">
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #ffffff" width="10%">
        /// </td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>X = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>X = 1</b></td>
        /// </tr>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff" width="10%">
        /// <b>Y = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// G</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// " width="10%">
        /// M</td>
        /// </tr>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff;" width="10%">
        /// <b>Y = 1</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// C</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// " width="10%">
        /// Y</td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// background-color: #00ffff" width="10%">
        /// <b>Y = 2</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// " width="10%">
        /// M</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// " width="10%">
        /// G</td>
        /// </tr>
        /// 
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// background-color: #00ffff;" width="10%">
        /// <b>Y = 3</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// " width="10%">
        /// C</td>
        /// <td colspan="1" rowspan="1" style="width:10%; 
        /// border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; 
        /// border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; 
        /// border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; 
        /// border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;
        /// " width="10%">
        /// Y</td>
        /// </tr>
        /// </table>
        /// </para>
        /// <para>It is recommended that this function be called only after a <see cref="AscomDriver.Connected">connection</see> is established with the camera hardware, to ensure that 
        /// the driver is aware of the capabilities of the specific camera model.</para>
        /// </remarks>
        public SensorType SensorType
        {
            get { return (SensorType)_memberFactory.CallMember(1, "SensorType", new Type[] { }, new object[] { }); }
        }
        #endregion
    }

    #endregion
}
