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
using ASCOM.Utilities.Video.DirectShowVideo;

namespace ASCOM.Utilities.Video
{
    [Guid("BD925113-3B58-4C5F-984E-FBCE7C6A93BE")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ICameraImage
    {
        void SetImageArray(object imageArray, int imageWidth, int imageHeight, SensorType sensorType);
        int GetPixel(int x, int y);
	    int GetPixel(int x, int y, int plain);
        byte[] GetDisplayBitmapBytes();
		bool FlipHorizontaly { get; set; }
		bool FlipVertically { get; set; }
    }

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
	    private int[,,] intColourPixelArray;
		private object[,,] objColourPixelArray;
        private int imageWidth;
        private int imageHeight;
        private SensorType sensorType;

        private bool isColumnMajor;
        private bool isRowMajor;

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
            else
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

            throw new ArgumentException();
        }

		public bool FlipHorizontaly { get; set; }

		public bool FlipVertically { get; set; }

		private FlipMode GetFlipMode()
		{
			if (FlipHorizontaly && FlipVertically)
				return FlipMode.FlipBoth;
			else if (FlipHorizontaly)
				return FlipMode.FlipHorizontally;
			else if (FlipVertically)
				return FlipMode.FlipVertically;
			else
				return FlipMode.None;
		}

		public byte[] GetDisplayBitmapBytes()
        {
			if (sensorType == SensorType.Monochrome)
			{
				if (intPixelArray != null)
					return NativeHelpers.PrepareBitmapForDisplay(intPixelArray, imageWidth, imageHeight, GetFlipMode());
				else if (objPixelArray != null)
					return NativeHelpers.PrepareBitmapForDisplay(objPixelArray, imageWidth, imageHeight, GetFlipMode());				
			}
			else if (sensorType == SensorType.Color)
			{
				if (intColourPixelArray != null)
					return NativeHelpers.PrepareColourBitmapForDisplay(intColourPixelArray, imageWidth, imageHeight, GetFlipMode());
				else if (objColourPixelArray != null)
					return NativeHelpers.PrepareColourBitmapForDisplay(objColourPixelArray, imageWidth, imageHeight, GetFlipMode());
			}
			else
				throw new NotSupportedException(string.Format("Sensor type {0} is not currently supported.", sensorType));

            return null;
        }

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

		public object GetImageArray(Bitmap bmp, SensorType sensorType, LumaConversionMode conversionMode, out byte[] bitmapBytes)
		{
			this.imageWidth = bmp.Width;
			this.imageHeight = bmp.Height;

			if (sensorType == SensorType.Monochrome)
				return NativeHelpers.GetMonochromePixelsFromBitmap(bmp, conversionMode, GetFlipMode(), out bitmapBytes);
			else if (sensorType == SensorType.Color)
				return NativeHelpers.GetColourPixelsFromBitmap(bmp, GetFlipMode(), out bitmapBytes);
			else
				throw new NotSupportedException(string.Format("Sensor type {0} is not currently supported.", sensorType));
		}

		public byte[] GetBitmapBytes(Bitmap bmp)
		{
			return NativeHelpers.GetBitmapBytes(bmp);
		}
    }
}
