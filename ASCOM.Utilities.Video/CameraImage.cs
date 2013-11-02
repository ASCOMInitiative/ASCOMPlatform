//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video
//
// Description:	Helper class to manipulate video ImageArray data
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ASCOM.DeviceInterface;

namespace ASCOM.Utilities.Video
{
    [ComVisible(true)]
    [Guid("289848FE-D368-4235-BBC6-42B4BCB66F66")]
    public enum VideoFrameLayout
    {
        Monochrome,
        Color,
        BayerRGGB
    }

    [ComVisible(true)]
    [Guid("6C02144E-641B-4E7D-9661-51FEFD48A068")]
    public enum LumaConversionMode
    {
        R = 0,
        G = 1,
        B = 2,
        GrayScale = 3
    }

    /// <summary>
    /// Defines the ICameraImage interface. This is a helper interface to be used to work with ImageArray frames, including conversion of the image array to a displayable Bitmap.
    /// </summary>
    [Guid("BD925113-3B58-4C5F-984E-FBCE7C6A93BE")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ICameraImage
    {
        /// <summary>
        /// Sets the image array which is typically returned by <see cref="P:ASCOM.DeviceInterface.IVideoFrame.ImageArray"/>.
        /// </summary>
        /// <param name="imageArray">The image array object.</param>
        /// <param name="imageWidth">The width of the image contained in the image array data.</param>
        /// <param name="imageHeight">The height of the image contained in the image array data.</param>
        /// <param name="sensorType">The sensor type for which the image array data has been encoded.</param>
        /// <exception cref="NotSupportedException">Throws this exception if the sensorType is not supported. Only <b>Monochrome</b> and <b>Color</b> sensor types are supported.</exception>
        void SetImageArray(object imageArray, int imageWidth, int imageHeight, SensorType sensorType);

        /// <summary>
        /// Returns the value of a single pixel from the image array of a Monochrome sensor.
        /// </summary>
        /// <param name="x">The X coordinate of the pixel starting from left.</param>
        /// <param name="y">The Y coordinate of the pixel starting from top.</param>
        /// <returns>The value of the specified pixel.</returns>
        /// <exception cref="InvalidOperationException">Throws this exception if the image array data hasn't been set ot is not corresponding to a Monochrome sensor.</exception>
        int GetPixel(int x, int y);

        /// <summary>
        /// Returns the value of a single pixel from the image array of a Colour sensor.
        /// </summary>
        /// <param name="x">The X coordinate of the pixel starting from left.</param>
        /// <param name="y">The Y coordinate of the pixel starting from top.</param>
        /// <param name="plain">The colour plain from which the pixel needs to be returned. </param>
        /// <returns>The value of the specified pixel.</returns>
        /// <exception cref="InvalidOperationException">Throws this exception if the image array data hasn't been set ot is not corresponding to a Colour sensor.</exception>
	    int GetPixel(int x, int y, int plain);

        /// <summary>
        /// Returns a display bitmap corresponding to the image array that has been set.
        /// </summary>
        /// <example> The following code can be used to create a Bitmap from the returned byte array
        /// <code lang="cs">
        /// using (var memStr = new MemoryStream(cameraImage.GetDisplayBitmapBytes()))
        /// {
        /// 	bmp = (Bitmap)Image.FromStream(memStr);
        /// }
        /// </code>
        /// <code lang="VB">
        /// Using memStr = New MemoryStream(cameraImage.GetDisplayBitmapBytes())
        /// 	bmp = DirectCast(Image.FromStream(memStr), Bitmap)
        /// End Using
        /// </code>
        /// </example>
        /// <returns>Returns a byte array of the byte content of the 24bit RBG bitmap. The byte array also contains all relevant bitmap headers.</returns>
        byte[] GetDisplayBitmapBytes();


        /// <summary>
        /// Sets the desired horizontal image flip mode.
        /// </summary>
        /// <remarks>
        /// Sets the desired horizontal image flip mode. By default no horizontal flipping is applied.
        /// </remarks>
		bool FlipHorizontally { get; set; }


        /// <summary>
        /// Sets the desired vertical image flip mode.
        /// </summary>
        /// <remarks>
        /// Sets the desired vertical image flip mode. By default no vertical flipping is applied.
        /// </remarks>
		bool FlipVertically { get; set; }
    }

    /// <summary>
    /// The class implements the <see cref="T:ASCOM.Utilities.Video.ICameraImage"/> interface and also provides two additional methods for pure .NET clients.
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(ICameraImage))]
    [Guid("41244296-BED8-4AC9-AA24-D4E90C6C95FA")]
    [ProgId("ASCOM.Utilities.CameraImage")]
    public class CameraImage : ICameraImage
    {
        private object imageArray;
        private int[,] intPixelArray;
        private object[,] objPixelArray;
        private int[, ,] intColourPixelArray;
        private object[, ,] objColourPixelArray;
        private int imageWidth;
        private int imageHeight;
        private SensorType sensorType;

        private bool isColumnMajor;
        private bool isRowMajor;

        private NativeHelpers nativeHelpers;

        public CameraImage()
        {
           nativeHelpers = new NativeHelpers();
        }

        /// <summary>
        /// Sets the image array which is typically returned by <see cref="P:ASCOM.DeviceInterface.IVideoFrame.ImageArray"/>.
        /// </summary>
        /// <param name="imageArray">The image array object.</param>
        /// <param name="imageWidth">The width of the image contained in the image array data.</param>
        /// <param name="imageHeight">The height of the image contained in the image array data.</param>
        /// <param name="sensorType">The sensor type for which the image array data has been encoded.</param>
        /// <exception cref="NotSupportedException">Throws this exception if the sensorType is not supported. Only <b>Monochrome</b> and <b>Color</b> sensor types are supported.</exception>
        public void SetImageArray(object imageArray, int imageWidth, int imageHeight, SensorType sensorType)
        {
            this.imageArray = imageArray;
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.sensorType = sensorType;

            objPixelArray = null;
            intPixelArray = null;
            intColourPixelArray = null;
            objColourPixelArray = null;

            if (sensorType == SensorType.Monochrome)
            {
                if (imageArray is int[,])
                {
                    intPixelArray = (int[,])imageArray;
                    isColumnMajor = intPixelArray.GetLength(0) == imageWidth;
                    isRowMajor = intPixelArray.GetLength(0) == imageHeight;
                    return;
                }
                else if (imageArray is object[,])
                {
                    objPixelArray = (object[,])imageArray;
                    isColumnMajor = objPixelArray.GetLength(0) == imageWidth;
                    isRowMajor = objPixelArray.GetLength(0) == imageHeight;
                    return;
                }
            }
            else if (sensorType == SensorType.Color)
            {
                // Color sensor type is represented as 3-dimentional array that can be either: [3, height, width], [width, height, 3]
                if (imageArray is int[,,])
                {
	                intColourPixelArray = (int[,,]) imageArray;
					isColumnMajor = intColourPixelArray.GetLength(0) == imageWidth;
					isRowMajor = intColourPixelArray.GetLength(0) == 3;
					return;
                }
                else if (imageArray is object[, ,])
                {
					objColourPixelArray = (object[, ,])imageArray;
					isColumnMajor = objColourPixelArray.GetLength(0) == imageWidth;
					isRowMajor = objColourPixelArray.GetLength(0) == 3;
					return;
                }
            }

            throw new NotSupportedException("Only Monochrome and Color sensor types are supported.");
        }

        /// <summary>
        /// Sets the desired horizontal image flip mode.
        /// </summary>
        /// <remarks>
        /// Sets the desired horizontal image flip mode. By default no horizontal flipping is applied.
        /// </remarks>
        public bool FlipHorizontally { get; set; }

        /// <summary>
        /// Sets the desired vertical image flip mode.
        /// </summary>
        /// <remarks>
        /// Sets the desired vertical image flip mode. By default no vertical flipping is applied.
        /// </remarks>
		public bool FlipVertically { get; set; }

		private FlipMode GetFlipMode()
		{
            if (FlipHorizontally && FlipVertically)
				return FlipMode.FlipBoth;
            else if (FlipHorizontally)
				return FlipMode.FlipHorizontally;
			else if (FlipVertically)
				return FlipMode.FlipVertically;
			else
				return FlipMode.None;
		}

        /// <summary>
        /// Returns a display bitmap corresponding to the image array that has been set.
        /// </summary>
        /// <example> The following code can be used to create a Bitmap from the returned byte array
        /// <code lang="cs">
        /// using (var memStr = new MemoryStream(cameraImage.GetDisplayBitmapBytes()))
        /// {
        /// 	bmp = (Bitmap)Image.FromStream(memStr);
        /// }
        /// </code>
        /// <code lang="VB">
        /// Using memStr = New MemoryStream(cameraImage.GetDisplayBitmapBytes())
        /// 	bmp = DirectCast(Image.FromStream(memStr), Bitmap)
        /// End Using
        /// </code>
        /// </example>
        /// <returns>Returns a byte array of the byte content of the 24bit RBG bitmap. The byte array also contains all relevant bitmap headers.</returns>
		public byte[] GetDisplayBitmapBytes()
        {
			if (sensorType == SensorType.Monochrome)
			{
				if (intPixelArray != null)
                    return nativeHelpers.PrepareBitmapForDisplay(intPixelArray, imageWidth, imageHeight, GetFlipMode());
				else if (objPixelArray != null)
                    return nativeHelpers.PrepareBitmapForDisplay(objPixelArray, imageWidth, imageHeight, GetFlipMode());
			}
			else if (sensorType == SensorType.Color)
			{
				if (intColourPixelArray != null)
                    return nativeHelpers.PrepareColourBitmapForDisplay(intColourPixelArray, imageWidth, imageHeight, GetFlipMode());
				else if (objColourPixelArray != null)
                    return nativeHelpers.PrepareColourBitmapForDisplay(objColourPixelArray, imageWidth, imageHeight, GetFlipMode());
			}
			else
				throw new NotSupportedException(string.Format("Sensor type {0} is not currently supported.", sensorType));

            return null;
        }

        /// <summary>
        /// Returns the value of a single pixel from the image array of a Monochrome sensor.
        /// </summary>
        /// <param name="x">The X coordinate of the pixel starting from left.</param>
        /// <param name="y">The Y coordinate of the pixel starting from top.</param>
        /// <returns>The value of the specified pixel.</returns>
        /// <exception cref="InvalidOperationException">Throws this exception if the image array data hasn't been set ot is not corresponding to a Monochrome sensor.</exception>
        public int GetPixel(int x, int y)
        {
            if (intPixelArray != null)
            {
                if (isRowMajor)
                    return intPixelArray[y, x];
                else if (isColumnMajor)
                    return intPixelArray[x, y];
            }
            else if (objPixelArray != null)
            {
                if (isRowMajor)
                    return (int)objPixelArray[y, x];
                else if (isColumnMajor)
                    return (int)objPixelArray[x, y];
            }
			else if (intColourPixelArray != null || objColourPixelArray != null)
			{
				throw new InvalidOperationException("Use the GetPixel(int, int, int) overload to get a pixel value in a colour image.");
			}
			
			throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns the value of a single pixel from the image array of a Colour sensor.
        /// </summary>
        /// <param name="x">The X coordinate of the pixel starting from left.</param>
        /// <param name="y">The Y coordinate of the pixel starting from top.</param>
        /// <param name="plain">The colour plain from which the pixel needs to be returned. </param>
        /// <returns>The value of the specified pixel.</returns>
        /// <exception cref="InvalidOperationException">Throws this exception if the image array data hasn't been set ot is not corresponding to a Colour sensor.</exception>
	    public int GetPixel(int x, int y, int plain)
	    {
			if (intPixelArray != null || objPixelArray != null)
			{
				throw new InvalidOperationException("Use the GetPixel(int, int) overload to get a pixel value in a monochrome image.");
			}
			else if (intColourPixelArray != null)
			{
				if (isRowMajor)
					return intColourPixelArray[plain, y, x];
				else if (isColumnMajor)
					return intColourPixelArray[x, y, plain];
			}
			else if (objColourPixelArray != null)
			{
				if (isRowMajor)
					return (int)objColourPixelArray[plain, y, x];
				else if (isColumnMajor)
					return (int)objColourPixelArray[x, y, plain];
			}

			throw new InvalidOperationException();
	    }

        /// <summary>
        /// Returns and image array and preview bitmap bytes in the format defined by  <see cref="T:ASCOM.DeviceInterface.IVideoFrame"/>.
        /// </summary>
        /// <param name="bmp">The bitmap image to use as a source.</param>
        /// <param name="sensorType">The type of the sensor to emulate. Only <b>Color</b> and <b>Monochrome</b> sensors are supported.</param>
        /// <param name="conversionMode">When the specified <b>SensorType</b> is not a colour sensor, this is the mode to use to convert colour pixels to monochrome.</param>
        /// <param name="bitmapBytes">The byte array of the corresponding preview bitmap.</param>
        /// <returns>An array of int32 that represents the pixels of the bitmap.</returns>
        /// <exception cref="NotSupportedException">Throws this exception if the requested sentor type is not supported..</exception>
		public object GetImageArray(Bitmap bmp, SensorType sensorType, LumaConversionMode conversionMode, out byte[] bitmapBytes)
		{
			this.imageWidth = bmp.Width;
			this.imageHeight = bmp.Height;

			if (sensorType == SensorType.Monochrome)
                return nativeHelpers.GetMonochromePixelsFromBitmap(bmp, conversionMode, GetFlipMode(), out bitmapBytes);
			else if (sensorType == SensorType.Color)
                return nativeHelpers.GetColourPixelsFromBitmap(bmp, GetFlipMode(), out bitmapBytes);
			else
				throw new NotSupportedException(string.Format("Sensor type {0} is not currently supported.", sensorType));
		}

        /// <summary>
        /// Returns the preview bitmap bytes of a given bitmap image.
        /// </summary>
        /// <param name="bmp">The bitmap image to use as a source.</param>
        /// <returns>The preview bitmap bytes of the bitmap.</returns>
		public byte[] GetBitmapBytes(Bitmap bmp)
		{
            return nativeHelpers.GetBitmapBytes(bmp);
		}
    }
}
