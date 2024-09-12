// tabs=4
// --------------------------------------------------------------------------------
// 
// ASCOM Video
// 
// Description:	The IVideo and IVideoFrame interfaces version 1
// 
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
// 
// --------------------------------------------------------------------------------
// 

using System.Collections;
using System;
using System.Runtime.InteropServices;

#if NET35
using ASCOM.Utilities;
#elif NET472
using ASCOM.Utilities;
#else
#endif

namespace ASCOM.DeviceInterface
{
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
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
		/// <para>The application must inspect the Safearray parameters to determine the dimensions and also the <see cref="P:ASCOM.DeviceInterface.IVideoV2.SensorType"/> to determine if the image is <b>Color</b> or not.
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
		/// <td>A row major <b>ImageArray[Pixels]</b> of <see cref="P:ASCOM.DeviceInterface.IVideoV2.Height"/> * <see cref="P:ASCOM.DeviceInterface.IVideoV2.Width"/> elements. The pixels in the array start from the top left part of the image and are listed by horizontal lines/rows. The second pixel in the array is the second pixel from the first horizontal row
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
		/// <td><b>ImageArray[Height, Width]</b> of <see cref="P:ASCOM.DeviceInterface.IVideoV2.Height"/> x <see cref="P:ASCOM.DeviceInterface.IVideoV2.Width"/> elements.</td>
		/// </tr>
		/// <tr>
		/// <td>2; int[,]</td>
		/// <td><b>Color</b></td>
		/// <td><b>ImageArray[NumPlane, Pixels]</b> of NumPlanes x <see cref="P:ASCOM.DeviceInterface.IVideoV2.Height"/> * <see cref="P:ASCOM.DeviceInterface.IVideoV2.Width"/> elements. The order of the three colour planes is
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
		/// <td><b>ImageArray[NumPlane, Height, Width]</b> of NumPlanes x <see cref="P:ASCOM.DeviceInterface.IVideoV2.Height"/> x <see cref="P:ASCOM.DeviceInterface.IVideoV2.Width"/> elements. The order of the three colour planes is
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
        /// bitmap = (Bitmap)Image.FromStream(memStr);
        /// }
        /// </code>
        /// <code lang="VB">
        /// Using memStr = New MemoryStream(frame.PreviewBitmap)
        /// bitmap = DirectCast(Image.FromStream(memStr), Bitmap)
        /// End Using
        /// </code>
        /// </example>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
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
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
		/// The frame number of the first exposed frame may not be zero and is dependent on the device and/or the driver. The frame number increases with each acquired frame not with each requested frame by the client.
		/// </remarks>
		/// <value>The frame number of the current video frame.</value>
		long FrameNumber { get; }

		/// <summary>
		/// Returns the actual exposure duration in seconds (i.e. shutter open time).
		/// </summary>
		/// <exception cref="PropertyNotImplementedException">Must throw an exception if not implemented.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks>
		/// This may differ from the exposure time corresponding to the requested frame exposure due to shutter latency, camera timing precision, etc.
		/// </remarks>
		/// <value>The duration of the frame exposure.</value>
		double ExposureDuration { get; }

		/// <summary>
		/// Returns the actual exposure start time in the FITS-standard CCYY-MM-DDThh:mm:ss[.sss...] format, if supported.
		/// </summary>
		/// <value>The frame exposure start time.</value>
		/// <exception cref="PropertyNotImplementedException">Must throw an exception if not implemented.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		string ExposureStartTime { get; }

		//Avoid reference to KeyValuePair class in .NET Standard 2.0 because the class doesn't exist in the ASCOM Library.
#if NET35
		/// <summary>
		/// Returns additional information associated with the video frame as a list of named variables.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
		/// <para>The returned object contains entries for each value. For each entry, the Key property is the value's name, and the Value property is the string value itself.</para>
		/// This property must return an empty list if no video frame metadata is provided.
		/// <para>The Keys is a single word, or multiple words joined by underscore characters, that sensibly describes the variable. It is recommended that Keys
		/// should be a maximum of 16 characters for legibility and all upper case.</para>
		/// <para>The KeyValuePair objects are instances of the <see cref="KeyValuePair">KeyValuePair class</see></para>
		/// </remarks>
		/// <value>An ArrayList of KeyValuePair objects.</value>
#else
        /// <summary>
        /// Returns additional information associated with the video frame as a list of named variables.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw an ASCOM.PropertyNotImplementedException.</b></p>
        /// <para>The returned object contains entries for each value. For each entry, the Key property is the value's name, and the Value property is the string value itself.</para>
        /// This property must return an empty list if no video frame metadata is provided.
        /// <para>The Keys is a single word, or multiple words joined by underscore characters, that sensibly describes the variable. It is recommended that Keys
        /// should be a maximum of 16 characters for legibility and all upper case.</para>
        /// <para>The KeyValuePair objects are instances of the KeyValuePair class</para>
        /// </remarks>
        /// <value>An ArrayList of KeyValuePair objects.</value>
#endif
        ArrayList ImageMetadata { get; }
    }
}
