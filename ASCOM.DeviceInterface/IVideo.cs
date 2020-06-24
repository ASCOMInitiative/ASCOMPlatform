﻿// tabs=4
// --------------------------------------------------------------------------------
// 
// ASCOM Video
// 
// Description:	The IVideo and IVideoFrame interfaces ver 1
// 
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
// 
// --------------------------------------------------------------------------------
// 

using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// ASCOM Video Camera supported frame rates.
    /// </summary>
    [Guid("AECD630C-3A08-46A2-96D3-33F3CF461CBB")]
    [ComVisible(true)]
    public enum VideoCameraFrameRate
    {
        /// <summary>
        /// This is a video camera that supports variable frame rates.
        /// </summary>
        Variable = 0,

        /// <summary>
        /// 25 frames per second (fps) corresponding to a <b>PAL</b> (colour) or <b>CCIR</b> (black and white) video standard.
        /// </summary>
        PAL = 1,

        /// <summary>
        /// 29.97  frames per second (fps) corresponding to an <b>NTSC</b> (colour) or <b>EIA</b> (black and white) video standard.
        /// </summary>
        NTSC = 2
    }

    /// <summary>
    /// ASCOM Video Camera status values.
    /// </summary>
    [Guid("84422451-5D8E-4F5A-9A81-8E197AABF79B")]
    [ComVisible(true)]
    public enum VideoCameraState
    {
        /// <summary>
        /// Camera status running. The video is receiving signal and video frames are available for viewing or recording.
        /// </summary>
        videoCameraRunning = 0,

        /// <summary>
        /// Camera status recording. The video camera is recording video to the file system. Video frames are available for viewing.
        /// </summary>
        videoCameraRecording = 1,

        /// <summary>
        /// Camera status error. The video camera is in a state of an error and cannot continue its operation. Usually a reset will be required to resolve the error condition.
        /// </summary>
        videoCameraError = 2
    }

    /// <summary>
    /// Defines the IVideoFrame Interface.
    /// </summary>
    [Guid("EA1D5478-7263-43F8-B708-78783A48158C")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IVideoFrame
    {
        /// <summary>
        /// Returns a safearray of int32 containing the pixel values from the video frame. The array could be one of: ImageArray[Pixels], ImageArray[Height, Width], ImageArray[NumPlane, Pixels]
        /// or ImageArray[NumPlane, Height, Width].
        /// </summary>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// <para>The application must inspect the Safearray parameters to determine the dimensions and also the <see cref="P:ASCOM.DeviceInterface.IVideo.SensorType"/> to determine if the image is <b>Color</b> or not.
        /// The following table should be used to determine the format of the data:</para>
        /// <para>
        /// <table style="width:76.24%;" cellspacing="0" width="76.24%">
        /// <col style="width: 11.701%;"></col>
        /// <col style="width: 20.708%;"></col>
        /// <col style="width: 67.591%;"></col>
        /// <tr>
        /// <td colspan="1" rowspan="1" style="width: 11.701%; padding-right: 10px; padding-left: 10px; &#xA; border-left-color: #000000; border-left-style: Solid; &#xA; border-top-color: #000000; border-top-style: Solid; &#xA; border-right-color: #000000; border-right-style: Solid;&#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; &#xA; background-color: #00ffff;" width="11.701%">
        /// <b>Dimensions</b></td>
        /// <td colspan="1" rowspan="1" style="width: 20.708%; padding-right: 10px; padding-left: 10px; &#xA; border-top-color: #000000; border-top-style: Solid; &#xA; border-right-style: Solid; border-right-color: #000000; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; &#xA; background-color: #00ffff;" width="20.708%">
        /// <b>SensorType</b></td>
        /// <td colspan="1" rowspan="1" style="width: 67.591%; padding-right: 10px; padding-left: 10px; &#xA; border-top-color: #000000; border-top-style: Solid; &#xA; border-right-style: Solid; border-right-color: #000000; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; &#xA; background-color: #00ffff;" width="67.591%">
        /// <b>Array Format</b></td>
        /// </tr>
        /// <tr>
        /// <td>1; int[]</td>
        /// <td><b>Monochrome</b>, <b>RGGB</b>, <b>CMYG</b>, <b>CMYG2</b>, <b>LRGB</b></td>
        /// <td>A row major <b>ImageArray[Pixels]</b> of <see cref="P:ASCOM.DeviceInterface.IVideo.Height"/> * <see cref="P:ASCOM.DeviceInterface.IVideo.Width"/> elements. The pixels in the array start from the top left part of the image and are listed by horizontal lines/rows. The second pixel in the array is the second pixel from the first horizontal row
        /// and the second last pixel in the array is the second last pixels from the last horizontal row.</td>
        /// </tr>
        /// <tr>
        /// <td>1; int[]</td>
        /// <td><b>Color</b></td>
        /// <td><p style="color:red">Invalid configuration.</p></td>
        /// </tr>
        /// <tr>
        /// <td>2; int[,]</td>
        /// <td><b>Monochrome</b>, <b>RGGB</b>, <b>CMYG</b>, <b>CMYG2</b>, <b>LRGB</b></td>
        /// <td><b>ImageArray[Height, Width]</b> of <see cref="P:ASCOM.DeviceInterface.IVideo.Height"/> x <see cref="P:ASCOM.DeviceInterface.IVideo.Width"/> elements.</td>
        /// </tr>
        /// <tr>
        /// <td>2; int[,]</td>
        /// <td><b>Color</b></td>
        /// <td><b>ImageArray[NumPlane, Pixels]</b> of NumPlanes x <see cref="P:ASCOM.DeviceInterface.IVideo.Height"/> * <see cref="P:ASCOM.DeviceInterface.IVideo.Width"/> elements. The order of the three colour planes is
        /// first is <b>R</b>, the second is <b>G</b> and third is <b>B</b>. The pixels in second dimension of the array start from the top left part of the image and are listed by horizontal lines/rows. The second pixel is the second pixel from the first horizontal row
        /// and the second last pixel is the second last pixels from the last horizontal row.</td>
        /// </tr>
        /// <tr>
        /// <td>3; int[,,]</td>
        /// <td><b>Monochrome</b>, <b>RGGB</b>, <b>CMYG</b>, <b>CMYG2</b>, <b>LRGB</b></td>
        /// <td><p style="color:red">Invalid configuration.</p></td>
        /// </tr>
        /// <tr>
        /// <td>3; int[,,]</td>
        /// <td><b>Color</b></td>
        /// <td><b>ImageArray[NumPlane, Height, Width]</b> of NumPlanes x <see cref="P:ASCOM.DeviceInterface.IVideo.Height"/> x <see cref="P:ASCOM.DeviceInterface.IVideo.Width"/> elements. The order of the three colour planes is
        /// first is <b>R</b>, the second is <b>G</b> and third is <b>B</b>.</td>
        /// </tr>
        /// </table>
        /// </para>
        /// <para>In <b>Color</b> SensorType mode, if the application cannot handle multispectral images, it should use just the first plane.</para>
        /// </remarks>
        /// <value>The image array.</value>
        object ImageArray { get; }

        /// <summary>
        /// Returns a preview bitmap for the last video frame as an array of byte.
        /// </summary>
        /// <example> The following code can be used to create a Bitmap from the returned byte array
        /// <code lang="cs">
        /// using (var memStr = new MemoryStream(frame.PreviewBitmap))
        /// {
        /// bmp = (Bitmap)Image.FromStream(memStr);
        /// }
        /// </code>
        /// <code lang="VB">
        /// Using memStr = New MemoryStream(frame.PreviewBitmap)
        /// bmp = DirectCast(Image.FromStream(memStr), Bitmap)
        /// End Using
        /// </code>
        /// </example>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p> The application can use this bitmap to show a preview image of the last video frame when required. This is a convenience property for
        /// those applications that don't require to process the <see cref="P:ASCOM.DeviceInterface.IVideoFrame.ImageArray"/> but usually only adjust the video camera settings and then record a video file.
        /// <para>When a 24bit RGB image can be returned by the driver this should be the preferred format. </para>
        /// </remarks>
        /// <value>The preview bitmap image.</value>
        byte[] PreviewBitmap { get; }

        /// <summary>
        /// Returns the frame number.
        /// </summary>
        /// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// The frame number of the first exposed frame may not be zero and is dependent on the device and/or the driver. The frame number increases with each acquired frame not with each requested frame by the client.
        /// </remarks>
        /// <value>The frame number of the current video frame.</value>
        long FrameNumber { get; }

        /// <summary>
        /// Returns the actual exposure duration in seconds (i.e. shutter open time).
        /// </summary>
        /// <remarks>
        /// This may differ from the exposure time corresponding to the requested frame exposure due to shutter latency, camera timing precision, etc.
        /// </remarks>
        /// <value>The duration of the frame exposure.</value>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if not implemented.</exception>
        double ExposureDuration { get; }

        /// <summary>
        /// Returns the actual exposure start time in the FITS-standard CCYY-MM-DDThh:mm:ss[.sss...] format, if supported.
        /// </summary>
        /// <value>The frame exposure start time.</value>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if not implemented.</exception>
        string ExposureStartTime { get; }

        /// <summary>
        /// Returns additional information associated with the video frame as a list of named variables.
        /// </summary>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// <para>The returned object contains entries for each value. For each entry, the Key property is the value's name, and the Value property is the string value itself.</para>
        /// This property must return an empty list if no video frame metadata is provided.
        /// <para>The Keys is a single word, or multiple words joined by underscore characters, that sensibly describes the variable. It is recommended that Keys
        /// should be a maximum of 16 characters for legibility and all upper case.</para>
        /// <para>The KeyValuePair objects are instances of the <see cref="ASCOM.Utilities.KeyValuePair">KeyValuePair class</see></para>
        /// </remarks>
        /// <value>An ArrayList of KeyValuePair objects.</value>
        ArrayList ImageMetadata { get; }
    }

    /// <summary>
    /// Defines the IVideo Interface.
    /// </summary>
    [Guid("00A394A5-BCB0-449D-A46B-81A02824ADC5")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IVideo
    {
        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p>Do not use a NotConnectedException here, that exception is for use in other methods that require a connection in order to succeed.
        /// <para>The Connected property sets and reports the state of connection to the device hardware.
        /// For a hub this means that Connected will be true when the first driver connects and will only be set to false
        /// when all drivers have disconnected.  A second driver may find that Connected is already true and
        /// setting Connected to false does not report Connected as false.  This is not an error because the physical state is that the
        /// hardware connection is still true.</para>
        /// <para>Multiple calls setting Connected to true or false will not cause an error.</para>
        /// </remarks>
        bool Connected { get; set; }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p> 
        /// <para>The description length must be a maximum of 64 characters so that it can be used in FITS image headers, which are limited to 80 characters including the header name.</para>
        /// </remarks>
        string Description { get; }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p> This string may contain line endings and may be hundreds to thousands of characters long.
        /// It is intended to display detailed information on the ASCOM driver, including version and copyright data.
        /// See the <see cref="P:ASCOM.DeviceInterface.IVideo.Description"/> property for information on the device itself.
        /// To get the driver version in a parseable string, use the <see cref="P:ASCOM.DeviceInterface.IVideo.DriverVersion"/> property.
        /// </remarks>
        string DriverInfo { get; }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful.</exception>
        /// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p> This must be in the form "n.n".
        /// It should not to be confused with the <see cref="P:ASCOM.DeviceInterface.IVideo.InterfaceVersion"/> property, which is the version of this specification supported by the
        /// driver.
        /// </remarks>
        string DriverVersion { get; }

        /// <summary>
        /// The interface version number that this device supports. Should return 1 for this interface version.
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful.</exception>
        /// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// </remarks>
        short InterfaceVersion { get; }

        /// <summary>
        /// The short name of the driver, for display purposes.
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful.</exception>
        /// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// </remarks>
        string Name { get; }

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.</returns>
        /// <exception cref="ASCOM.MethodNotImplementedException">Thrown if no actions are supported.</exception>
        /// <exception cref="ASCOM.ActionNotImplementedException">It is intended that the <see cref="SupportedActions"/> method will inform clients of driver capabilities, but the driver must still throw 
        /// an <see cref="ASCOM.ActionNotImplementedException"/> exception  if it is asked to perform an action that it does not support.</exception>
        /// <exception cref="NotConnectedException">If the driver is not connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful.</exception>
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// <remarks><p style="color:red"><b>Must be implemented.</b></p>
        /// <para>Action names are case insensitive, so SelectWheel, selectwheel and SELECTWHEEL all refer to the same action.</para>
        /// <para>The names of all supported actions must be returned in the <see cref="SupportedActions" /> property.</para>
        /// </remarks>
        string Action(string ActionName, string ActionParameters);

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Must be implemented</b></p>
        /// <para>This method must return an empty <see cref="ArrayList" /> if no actions are supported. Do not throw a <see cref="ASCOM.PropertyNotImplementedException" />.</para>
        /// <para>This is an aid to client authors and testers who would otherwise have to repeatedly poll the driver to determine its capabilities. Returned action names may be in mixed case to
        /// enhance presentation but the <see cref="Action" /> method is case insensitive.</para>
        /// <para>Collections have been used in the Telescope specification for a number of years and are known to be compatible with COM. Within .NET, <see cref="ArrayList" /> is the correct
        /// implementation to use because the .NET Generic methods are not compatible with COM.</para>
        /// </remarks>
        ArrayList SupportedActions { get; }

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        void Dispose();

        /// <summary>
        /// The name of the video capture device when such a device is used.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if not implemented.</exception>
        /// <remarks>For analogue video this is usually the video capture card or dongle attached to the computer.
        /// </remarks>
        string VideoCaptureDeviceName { get; }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful.</exception>
        /// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// </remarks>
        void SetupDialog();

        /// <summary>
        /// The maximum supported exposure (integration time) in seconds.
        /// </summary>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// This value is for information purposes only. The exposure cannot be set directly in seconds, use <see cref="P:ASCOM.DeviceInterface.IVideo.IntegrationRate"/> property to change the exposure.
        /// </remarks>
        double ExposureMax { get; }

        /// <summary>
        /// The minimum supported exposure (integration time) in seconds.
        /// </summary>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// This value is for information purposes only. The exposure cannot be set directly in seconds, use <see cref="P:ASCOM.DeviceInterface.IVideo.IntegrationRate"/> property to change the exposure.
        /// </remarks>
        double ExposureMin { get; }

        /// <summary>
        /// The frame rate at which the camera is running.
        /// </summary>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// Analogue cameras run in one of the two fixed frame rates - 25fps for PAL video and 29.97fps for NTSC video.
        /// Digital cameras usually can run at a variable frame rate. This value is for information purposes only and cannot be set. The FrameRate has the same value during the entire operation of the device.
        /// Changing the <see cref="P:ASCOM.DeviceInterface.IVideo.IntegrationRate"/> property may change the actual variable frame rate but cannot changethe return value of this property.
        /// </remarks>
        VideoCameraFrameRate FrameRate { get; }

        /// <summary>
        /// Returns the list of integration rates supported by the video camera.
        /// </summary>
        /// <remarks>
        /// Digital and integrating analogue video cameras allow the effective exposure of a frame to be changed. If the camera supports setting the exposure directly i.e. 2.153 sec then the driver must only
        /// return a range of useful supported exposures. For many video cameras the supported exposures (integration rates) increase by a factor of 2 from a base exposure e.g. 1, 2, 4, 8, 16 sec or 0.04, 0.08, 0.16, 0.32, 0.64, 1.28, 2.56, 5.12, 10.24 sec.
        /// If the camers supports only one exposure that cannot be changed (such as all non integrating PAL or NTSC video cameras) then this property must throw <see cref="PropertyNotImplementedException"/>.
        /// </remarks>
        /// <value>The list of supported integration rates in seconds.</value>
        /// <exception cref="NotConnectedException">Must throw exception if data unavailable.</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw exception if camera supports only one integration rate (exposure) that cannot be changed.</exception>
        ArrayList SupportedIntegrationRates { get; }

        /// <summary>
        /// Index into the <see cref="P:ASCOM.DeviceInterface.IVideo.SupportedIntegrationRates"/> array for the selected camera integration rate.
        /// </summary>
        /// <value>Integer index for the current camera integration rate in the <see cref="P:ASCOM.DeviceInterface.IVideo.SupportedIntegrationRates"/> string array.</value>
        /// <returns>Index into the SupportedIntegrationRates array for the selected camera integration rate.</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an
        /// active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if the camera supports only one integration rate (exposure) that cannot be changed.</exception>
        /// <remarks>
        /// <see cref="P:ASCOM.DeviceInterface.IVideo.IntegrationRate"/> can be used to adjust the integration rate (exposure) of the camera, if supported. A 0-based array of strings - <see cref="P:ASCOM.DeviceInterface.IVideo.SupportedIntegrationRates"/>,
        /// which correspond to different discrete integration rate settings supported by the camera will be returned. <see cref="P:ASCOM.DeviceInterface.IVideo.IntegrationRate"/> must be set to an integer in this range.
        /// <para>The driver must default <see cref="P:ASCOM.DeviceInterface.IVideo.IntegrationRate"/> to a valid value when integration rate is supported by the camera. </para>
        /// </remarks>
        int IntegrationRate { get; set; }

        /// <summary>
        /// Returns an <see cref="DeviceInterface.IVideoFrame"/> with its <see cref="P:ASCOM.DeviceInterface.IVideoFrame.ImageArray"/> property populated.
        /// </summary>
        /// <value>The current video frame.</value>
        /// <exception cref="NotConnectedException">Must throw exception if data unavailable.</exception>
        /// <exception cref="InvalidOperationException">If called before any video frame has been taken.</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// </remarks>
        IVideoFrame LastVideoFrame { get; }

        /// <summary>
        /// Sensor name.
        /// </summary>
        /// <returns>The name of sensor used within the camera.</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an
        /// active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw exception if not implemented.</exception>
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
        /// <para>It is recommended that this function be called only after a <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> is established with
        /// the camera hardware, to ensure that the driver is aware of the capabilities of the specific camera model.</para>
        /// </remarks>
        string SensorName { get; }

        /// <summary>
        /// Type of colour information returned by the the camera sensor.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="DeviceInterface.SensorType"/> enum value of the camera sensor</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an
        /// active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// <para><see cref="P:ASCOM.DeviceInterface.IVideo.SensorType"/> returns a value indicating whether the sensor is monochrome, or what Bayer matrix it encodes.
        /// The following values are defined:</para>
        /// <para>
        /// <table style="width:76.24%;" cellspacing="0" width="76.24%">
        /// <col style="width: 11.701%;"></col>
        /// <col style="width: 20.708%;"></col>
        /// <col style="width: 67.591%;"></col>
        /// <tr>
        /// <td colspan="1" rowspan="1" style="width: 11.701%; padding-right: 10px; padding-left: 10px; &#xA; border-left-color: #000000; border-left-style: Solid; &#xA; border-top-color: #000000; border-top-style: Solid; &#xA; border-right-color: #000000; border-right-style: Solid;&#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; &#xA; background-color: #00ffff;" width="11.701%">
        /// <b>Value</b></td>
        /// <td colspan="1" rowspan="1" style="width: 20.708%; padding-right: 10px; padding-left: 10px; &#xA; border-top-color: #000000; border-top-style: Solid; &#xA; border-right-style: Solid; border-right-color: #000000; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; &#xA; background-color: #00ffff;" width="20.708%">
        /// <b>Enumeration</b></td>
        /// <td colspan="1" rowspan="1" style="width: 67.591%; padding-right: 10px; padding-left: 10px; &#xA; border-top-color: #000000; border-top-style: Solid; &#xA; border-right-style: Solid; border-right-color: #000000; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; &#xA; background-color: #00ffff;" width="67.591%">
        /// <b>Meaning</b></td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-left-color: #000000; border-left-style: Solid; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 0</td>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Monochrome</td>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Camera produces monochrome array with no Bayer encoding</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-left-color: #000000; border-left-style: Solid; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 1</td>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Colour</td>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Camera produces color image directly, requiring not Bayer decoding. The monochome pixels for the R, G and B channels are returned in this order in the <see cref="P:ASCOM.DeviceInterface.IVideoFrame.ImageArray"/>.</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-left-color: #000000; border-left-style: Solid; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 2</td>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// RGGB</td>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Camera produces RGGB encoded Bayer array images</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-left-color: #000000; border-left-style: Solid; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 3</td>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// CMYG</td>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Camera produces CMYG encoded Bayer array images</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-left-color: #000000; border-left-style: Solid; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 4</td>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// CMYG2</td>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Camera produces CMYG2 encoded Bayer array images</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-left-color: #000000; border-left-style: Solid; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 5</td>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// LRGB</td>
        /// <td style="padding-right: 10px; padding-left: 10px; &#xA; border-right-color: #000000; border-right-style: Solid; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; &#xA; border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Camera produces Kodak TRUESENSE Bayer LRGB array images</td>
        /// </tr>
        /// </table>
        /// </para>
        /// <para>Please note that additional values may be defined in future updates of the standard, as new Bayer matrices may be created
        /// by sensor manufacturers in the future.  If this occurs, then a new enumeration value shall be defined. The pre-existing enumeration
        /// values shall not change.
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
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #ffffff" width="10%">
        /// </td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;&#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff;" width="10%">
        /// <b>X = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; background-color: #00ffff;" width="10%">
        /// <b>X = 1</b></td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff" width="10%">
        /// <b>Y = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// R</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; " width="10%">
        /// G</td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; background-color: #00ffff;" width="10%">
        /// <b>Y = 1</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; " width="10%">
        /// G</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; " width="10%">
        /// B</td>
        /// </tr>
        /// </table>
        /// </para>
        /// <para>CMYG indicates the following matrix:</para>
        /// <para>
        /// <table style="width:41.254%;" cellspacing="0" width="41.254%">
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #ffffff" width="10%">
        /// </td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;&#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff;" width="10%">
        /// <b>X = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; background-color: #00ffff;" width="10%">
        /// <b>X = 1</b></td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff" width="10%">
        /// <b>Y = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// Y</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; " width="10%">
        /// C</td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; background-color: #00ffff;" width="10%">
        /// <b>Y = 1</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; " width="10%">
        /// G</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; " width="10%">
        /// M</td>
        /// </tr>
        /// </table>
        /// </para>
        /// <para>CMYG2 indicates the following matrix:</para>
        /// <para>
        /// <table style="width:41.254%;" cellspacing="0" width="41.254%">
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #ffffff" width="10%">
        /// </td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;&#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff;" width="10%">
        /// <b>X = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; background-color: #00ffff;" width="10%">
        /// <b>X = 1</b></td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff" width="10%">
        /// <b>Y = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// C</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; " width="10%">
        /// Y</td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff;" width="10%">
        /// <b>Y = 1</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// M</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; " width="10%">
        /// G</td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff" width="10%">
        /// <b>Y = 2</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// C</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; " width="10%">
        /// Y</td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; background-color: #00ffff;" width="10%">
        /// <b>Y = 3</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; " width="10%">
        /// G</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; " width="10%">
        /// M</td>
        /// </tr>
        /// </table>
        /// </para>
        /// <para>LRGB indicates the following matrix (Kodak TRUESENSE):</para>
        /// <para>
        /// <table style="width:68.757%;" cellspacing="0" width="68.757%">
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <col style="width: 10%;"></col>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #ffffff" width="10%">
        /// </td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px;&#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff;" width="10%">
        /// <b>X = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff;" width="10%">
        /// <b>X = 1</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff;" width="10%">
        /// <b>X = 2</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; background-color: #00ffff;" width="10%">
        /// <b>X = 3</b></td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff" width="10%">
        /// <b>Y = 0</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// L</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// R</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// L</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; " width="10%">
        /// G</td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff;" width="10%">
        /// <b>Y = 1</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// R</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// L</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// G</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; " width="10%">
        /// L</td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; background-color: #00ffff" width="10%">
        /// <b>Y = 2</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// L</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// G</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; " width="10%">
        /// L</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-top-color: #000000; border-top-style: Solid;  border-top-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; " width="10%">
        /// B</td>
        /// </tr>
        /// <tr valign="top" align="center">
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; background-color: #00ffff;" width="10%">
        /// <b>Y = 3</b></td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; " width="10%">
        /// G</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; " width="10%">
        /// L</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; " width="10%">
        /// B</td>
        /// <td colspan="1" rowspan="1" style="width:10%; &#xA; border-top-color: #000000; border-top-style: Solid; border-top-width: 1px; &#xA; border-left-color: #000000; border-left-style: Solid; border-left-width: 1px; &#xA; border-right-color: #000000; border-right-style: Solid; border-right-width: 1px; &#xA; border-bottom-color: #000000; border-bottom-style: Solid; border-bottom=width: 1px;&#xA; " width="10%">
        /// L</td>
        /// </tr>
        /// </table>
        /// </para>
        /// <para>It is recommended that this function be called only after a <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> is established with the camera hardware, to ensure that
        /// the driver is aware of the capabilities of the specific camera model.</para>
        /// </remarks>
        SensorType SensorType { get; }

        /// <summary>
        /// Returns the width of the video frame in pixels.
        /// </summary>
        /// <value>The video frame width.</value>
        /// <exception cref="NotConnectedException">Must throw exception if the value is not known.</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// For analogue video cameras working via a frame grabber the dimensions of the video frames may be different than the dimension of the CCD chip
        /// </remarks>
        int Width { get; }

        /// <summary>
        /// Returns the height of the video frame in pixels.
        /// </summary>
        /// <value>The video frame height.</value>
        /// <exception cref="NotConnectedException">Must throw exception if the value is not known.</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// For analogue video cameras working via a frame grabber the dimensions of the video frames may be different than the dimension of the CCD chip
        /// </remarks>
        int Height { get; }

        /// <summary>
        /// Returns the width of the CCD chip pixels in microns.
        /// </summary>
        /// <value>The pixel size X if known.</value>
        /// <exception cref="PropertyNotImplementedException">Must throw exception if not implemented.</exception>
        double PixelSizeX { get; }

        /// <summary>
        /// Returns the height of the CCD chip pixels in microns.
        /// </summary>
        /// <value>The pixel size Y if known.</value>
        /// <exception cref="PropertyNotImplementedException">Must throw exception if not implemented.</exception>
        double PixelSizeY { get; }

        /// <summary>
        /// Reports the bit depth the camera can produce.
        /// </summary>
        /// <value>The bit depth per pixel. Typical analogue videos are 8-bit while some digital cameras can provide 12, 14 or 16-bit images.</value>
        /// <exception cref="NotConnectedException">Must throw exception if data unavailable.</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// </remarks>
        int BitDepth { get; }

        /// <summary>
        /// Returns the video codec used to record the video file.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">Must throw exception if not implemented.</exception>
        /// <remarks>For AVI files this is usually the FourCC identifier of the codec- e.g. XVID, DVSD, YUY2, HFYU etc.
        /// If the recorded video file doesn't use codecs an empty string must be returned.
        /// </remarks>
        string VideoCodec { get; }

        /// <summary>
        /// Returns the file format of the recorded video file, e.g. AVI, MPEG, ADV etc.
        /// </summary>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// </remarks>
        string VideoFileFormat { get; }

        /// <summary>
        /// The size of the video frame buffer.
        /// </summary>
        /// <value>The size of the video frame buffer. </value>
        /// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p> When retrieving video frames using the <see cref="P:ASCOM.DeviceInterface.IVideo.LastVideoFrame" /> property
        /// the driver may use a buffer to queue the frames waiting to be read by the client. This property returns the size of the buffer in frames or
        /// if no buffering is supported then the value of less than 2 should be returned. The size of the buffer can be controlled by the end user from the driver setup dialog.
        /// </remarks>
        int VideoFramesBufferSize { get; }

        /// <summary>
        /// Starts recording a new video file.
        /// </summary>
        /// <param name="PreferredFileName">The file name requested by the client. Some systems may not allow the file name to be controlled directly and they should ignore this parameter.</param>
        /// <returns>The actual file name of the file that is being recorded.</returns>
        /// <exception cref="MethodNotImplementedException">Must throw exception if not implemented.</exception>
        /// <exception cref="NotConnectedException">Must throw exception if not connected.</exception>
        /// <exception cref="InvalidOperationException">Must throw exception if the current camera state doesn't allow to begin recording a file.</exception>
        /// <exception cref="DriverException">Must throw exception if there is any other problem as result of which the recording cannot begin.</exception>
        string StartRecordingVideoFile(string PreferredFileName);

        /// <summary>
        /// Stops the recording of a video file.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">Must throw exception if not implemented.</exception>
        /// <exception cref="NotConnectedException">Must throw exception if not connected.</exception>
        /// <exception cref="InvalidOperationException">Must throw exception if the current camera state doesn't allow to stop recording the file or no file is currently being recorded.</exception>
        /// <exception cref="DriverException">Must throw exception if there is any other problem as result of which the recording cannot stop.</exception>
        void StopRecordingVideoFile();

        /// <summary>
        /// Returns the current camera operational state.
        /// </summary>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// Returns one of the following status information:
        /// <list type="bullet">
        /// <listheader><description>Value  State           Meaning</description></listheader>
        /// <item><description>0      CameraRunning	  The camera is running and video frames are available for viewing and recording</description></item>
        /// <item><description>1      CameraRecording The camera is running and recording a video</description></item>
        /// <item><description>2      CameraError     Camera error condition serious enough to prevent further operations (connection fail, etc.).</description></item>
        /// </list>
        /// <para>CameraIdle and CameraBusy are optional states. Free running cameras cannot be stopped and don't have a CameraIdle state. When those cameras are powered they immediately enter CameraRunning state.
        /// Some digital cameras or vdeo systems may suport operations that take longer to complete. Whlie those longer operations are running the camera will remain in the state it was before the operation started.</para>
        /// <para>The video camera state diagram is shown below: </para>
        /// <para>
        /// <img src="../media/VideoCamera State Diagram.png"/></para>
        /// </remarks>
        /// <value>The state of the camera.</value>
        /// <exception cref="NotConnectedException">Must return an exception if the camera status is unavailable.</exception>
        VideoCameraState CameraState { get; }

        /// <summary>
        /// Maximum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/>.
        /// </summary>
        /// <value>Short integer representing the maximum gain value supported by the camera.</value>
        /// <returns>The maximum gain value that this camera supports</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an
        /// active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if GainMax is not supported.</exception>
        /// <remarks>When specifying the gain setting with an integer value, <see cref="P:ASCOM.DeviceInterface.IVideo.GainMax"/> is used in conjunction with <see cref="P:ASCOM.DeviceInterface.IVideo.GainMin"/> to
        /// specify the range of valid settings.
        /// <para><see cref="P:ASCOM.DeviceInterface.IVideo.GainMax"/> shall be greater than <see cref="P:ASCOM.DeviceInterface.IVideo.GainMin"/>. If either is available, then both must be available.</para>
        /// <para>Please see <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/> for more information.</para>
        /// <para>It is recommended that this function be called only after a <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> is established with the camera hardware, to ensure
        /// that the driver is aware of the capabilities of the specific camera model.</para>
        /// </remarks>
        short GainMax { get; }

        /// <summary>
        /// Minimum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/>.
        /// </summary>
        /// <returns>The minimum gain value that this camera supports</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an
        /// active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if GainMin is not supported.</exception>
        /// <remarks>When specifying the gain setting with an integer value, <see cref="P:ASCOM.DeviceInterface.IVideo.GainMin"/> is used in conjunction with <see cref="P:ASCOM.DeviceInterface.IVideo.GainMax"/> to
        /// specify the range of valid settings.
        /// <para><see cref="P:ASCOM.DeviceInterface.IVideo.GainMax"/> shall be greater than <see cref="P:ASCOM.DeviceInterface.IVideo.GainMin"/>. If either is available, then both must be available.</para>
        /// <para>Please see <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/> for more information.</para>
        /// <para>It is recommended that this function be called only after a <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> is established with the camera hardware, to ensure
        /// that the driver is aware of the capabilities of the specific camera model.</para>
        /// </remarks>
        short GainMin { get; }

        /// <summary>
        /// Index into the <see cref="P:ASCOM.DeviceInterface.IVideo.Gains"/> array for the selected camera gain.
        /// </summary>
        /// <value>Short integer index for the current camera gain in the <see cref="P:ASCOM.DeviceInterface.IVideo.Gains"/> string array.</value>
        /// <returns>Index into the Gains array for the selected camera gain</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an
        /// active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if gain is not supported.</exception>
        /// <remarks>
        /// <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/> can be used to adjust the gain setting of the camera, if supported. There are two typical usage scenarios:
        /// <ul>
        /// <li>Discrete gain video cameras will return a 0-based array of strings - <see cref="P:ASCOM.DeviceInterface.IVideo.Gains"/>, which correspond to different discrete gain settings supported by the camera. <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/> must be set to an integer in this range. <see cref="P:ASCOM.DeviceInterface.IVideo.GainMin"/> and <see cref="P:ASCOM.DeviceInterface.IVideo.GainMax"/> must thrown an exception if
        /// this mode is used.</li>
        /// <li>Adjustable gain video cameras - <see cref="P:ASCOM.DeviceInterface.IVideo.GainMin"/> and <see cref="P:ASCOM.DeviceInterface.IVideo.GainMax"/> return integers, which specify the valid range for <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/>.</li>
        /// </ul>
        /// <para>The driver must default <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/> to a valid value. </para>
        /// </remarks>
        short Gain { get; set; }

        /// <summary>
        /// Gains supported by the camera.
        /// </summary>
        /// <returns>An ArrayList of gain names or values</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an
        /// active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if Gains is not supported</exception>
        /// <remarks><see cref="P:ASCOM.DeviceInterface.IVideo.Gains"/> provides a 0-based array of available gain settings.
        /// Typically the application software will display the available gain settings in a drop list. The application will then supply
        /// the selected index to the driver via the <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/> property.
        /// <para>The <see cref="P:ASCOM.DeviceInterface.IVideo.Gain"/> setting may alternatively be specified using integer values; if this mode is used then <see cref="P:ASCOM.DeviceInterface.IVideo.Gains"/> is invalid
        /// and must throw an exception. Please see <see cref="P:ASCOM.DeviceInterface.IVideo.GainMax"/> and <see cref="P:ASCOM.DeviceInterface.IVideo.GainMin"/> for more information.</para>
        /// <para>It is recommended that this function be called only after a <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> is established with the camera hardware,
        /// to ensure that the driver is aware of the capabilities of the specific camera model.</para>
        /// </remarks>
        ArrayList Gains { get; }

        /// <summary>
        /// Maximum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/>.
        /// </summary>
        /// <value>Short integer representing the maximum gamma value supported by the camera.</value>
        /// <returns>The maximum gain value that this camera supports</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an
        /// active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if GammaMax is not supported</exception>
        /// <remarks>When specifying the gamma setting with an integer value, <see cref="P:ASCOM.DeviceInterface.IVideo.GammaMax"/> is used in conjunction with <see cref="P:ASCOM.DeviceInterface.IVideo.GammaMin"/> to
        /// specify the range of valid settings.
        /// <para><see cref="P:ASCOM.DeviceInterface.IVideo.GammaMax"/> shall be greater than <see cref="P:ASCOM.DeviceInterface.IVideo.GammaMin"/>. If either is available, then both must be available.</para>
        /// <para>Please see <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/> for more information.</para>
        /// <para>It is recommended that this function be called only after a <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> is established with the camera hardware, to ensure
        /// that the driver is aware of the capabilities of the specific camera model.</para>
        /// </remarks>
        short GammaMax { get; }

        /// <summary>
        /// Minimum value of <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/>.
        /// </summary>
        /// <returns>The minimum gamma value that this camera supports</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an
        /// active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if GammaMin is not supported.</exception>
        /// <remarks>When specifying the gamma setting with an integer value, <see cref="P:ASCOM.DeviceInterface.IVideo.GammaMin"/> is used in conjunction with <see cref="P:ASCOM.DeviceInterface.IVideo.GammaMax"/> to
        /// specify the range of valid settings.
        /// <para><see cref="P:ASCOM.DeviceInterface.IVideo.GammaMax"/> shall be greater than <see cref="P:ASCOM.DeviceInterface.IVideo.GammaMin"/>. If either is available, then both must be available.</para>
        /// <para>Please see <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/> for more information.</para>
        /// <para>It is recommended that this function be called only after a <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> is established with the camera hardware, to ensure
        /// that the driver is aware of the capabilities of the specific camera model.</para>
        /// </remarks>
        short GammaMin { get; }

        /// <summary>
        /// Index into the <see cref="P:ASCOM.DeviceInterface.IVideo.Gammas"/> array for the selected camera gamma.
        /// </summary>
        /// <value>Short integer index for the current camera gamma in the <see cref="P:ASCOM.DeviceInterface.IVideo.Gammas"/> string array.</value>
        /// <returns>Index into the Gammas array for the selected camera gamma</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an
        /// active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="InvalidValueException">Must throw an exception if not valid.</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if Gamma is not supported.</exception>
        /// <remarks>
        /// <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/> can be used to adjust the gamma setting of the camera, if supported. There are two typical usage scenarios:
        /// <ul>
        /// <li>Discrete gamma video cameras will return a 0-based array of strings - <see cref="P:ASCOM.DeviceInterface.IVideo.Gammas"/>, which correspond to different discrete gamma settings supported by the camera. <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/> must be set to an integer in this range. <see cref="P:ASCOM.DeviceInterface.IVideo.GammaMin"/> and <see cref="P:ASCOM.DeviceInterface.IVideo.GammaMax"/> must thrown an exception if
        /// this mode is used.</li>
        /// <li>Adjustable gain video cameras - <see cref="P:ASCOM.DeviceInterface.IVideo.GammaMin"/> and <see cref="P:ASCOM.DeviceInterface.IVideo.GammaMax"/> return integers, which specify the valid range for <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/>.</li>
        /// </ul>
        /// <para>The driver must default <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/> to a valid value. </para>
        /// </remarks>
        short Gamma { get; set; }

        /// <summary>
        /// Gammas supported by the camera.
        /// </summary>
        /// <returns>An ArrayList of gamma names or values</returns>
        /// <exception cref="NotConnectedException">Must throw an exception if the information is not available. (Some drivers may require an
        /// active <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> in order to retrieve necessary information from the camera.)</exception>
        /// <exception cref="PropertyNotImplementedException">Must throw an exception if Gammas is not supported</exception>
        /// <remarks><see cref="P:ASCOM.DeviceInterface.IVideo.Gammas"/> provides a 0-based array of available gamma settings. This list can contain the widely used values of <b>OFF</b>, <b>LO</b> and <b>HI</b> that correspond to gammas of <b>1.00</b>, <b>0.45</b> and <b>0.35</b> as well as other extended values.
        /// Typically the application software will display the available gamma settings in a drop list. The application will then supply
        /// the selected index to the driver via the <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/> property.
        /// <para>The <see cref="P:ASCOM.DeviceInterface.IVideo.Gamma"/> setting may alternatively be specified using integer values; if this mode is used then <see cref="P:ASCOM.DeviceInterface.IVideo.Gammas"/> is invalid
        /// and must throw an exception. Please see <see cref="P:ASCOM.DeviceInterface.IVideo.GammaMax"/> and <see cref="P:ASCOM.DeviceInterface.IVideo.GammaMin"/> for more information.</para>
        /// <para>It is recommended that this function be called only after a <see cref="P:ASCOM.DeviceInterface.IVideo.Connected">connection</see> is established with the camera hardware,
        /// to ensure that the driver is aware of the capabilities of the specific camera model.</para>
        /// </remarks>
        ArrayList Gammas { get; }

        /// <summary>
        /// Returns True if the driver supports custom device properties configuration via the <see cref="M:ASCOM.DeviceInterface.IVideo.ConfigureDeviceProperties"/> method.
        /// </summary>
        /// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// </remarks>
        bool CanConfigureDeviceProperties { get; }

        /// <summary>
        /// Displays a device properties configuration dialog that allows the configuration of specialized settings.
        /// </summary>
        /// <exception cref="NotConnectedException">Must throw an exception if the camera is not connected.</exception>
        /// <exception cref="MethodNotImplementedException">Must throw an exception if not implemented.</exception>
        /// <remarks>
        /// <para>The dialog could also provide buttons for cameras that can be controlled via 'on screen display' menues and a set of navigation buttons such as Up, Down, Left, Right and Enter.
        /// This dialog is not intended to be used in unattended mode but can give greater control over video cameras that provide special features. The dialog may also allow
        /// changing standard <see cref="DeviceInterface.IVideo"/> interface settings such as Gamma and Gain. If a client software
        /// displays any <see cref="DeviceInterface.IVideo"/> interface settings then it should take care to keep in sync the values changed by this method and those changed directly via the interface.</para>
        /// <para>To support automated and unattended control over the specialized device settings or functions available on this dialog the driver should also allow their control via <see cref="P:ASCOM.DeviceInterface.IVideo.SupportedActions"/>.
        /// This dialog is meant to be used by the applications to allow the user to adjust specialized device settings when those applications don't specifically use the specialized settings in their functionality.</para>
        /// <para>Examples for specialized settings that could be supported are white balance and sharpness.</para>
        /// </remarks>
        void ConfigureDeviceProperties();
    }
}
