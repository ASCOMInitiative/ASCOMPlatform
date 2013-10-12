//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video
//
// Description:	Managed PInvoke definitions over the ASCOM.Unmanaged library 
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using ASCOM.Utilities.Video.DirectShowVideo;

namespace ASCOM.Utilities.Video
{
    internal enum FlipMode
    {
        None = 0,
        FlipHorizontally = 1,
        FlipVertically = 2,
        FlipBoth = 3,
    }

    internal class NativeHelpers
    {
        private const string VIDEOUTILS32_DLL_NAME = "ASCOM.Native.6.1.dll";
        private const string VIDEOUTILS64_DLL_NAME = "ASCOM.Native.6.1.dll";

        private const string VIDEOUTILS_DLL_LOCATION = @"\ASCOM\VideoUtils"; //This is appended to the Common Files path
        private TraceLogger TL;

        public NativeHelpers()
        {
            bool rc = false;
            string CommonProgramFilesPath = null;
            System.Text.StringBuilder ReturnedPath = new System.Text.StringBuilder(260);

            //Find the root location of the common files directory containing the ASCOM support files.
            //On a 32bit system this is \Program Files\Common Files
            //On a 64bit system this is \Program Files (x86)\Common Files
            
            if (Is64Bit()) // 64bit application so find the 32bit folder location
            {
                rc = SHGetSpecialFolderPath(IntPtr.Zero, ReturnedPath, CSIDL_PROGRAM_FILES_COMMONX86, false);
                CommonProgramFilesPath = ReturnedPath.ToString();
            }
            else //32bit application so just go with the .NET returned value
            {
                CommonProgramFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
            }

            //Add the ASCOM\.net directory to the DLL search path so that the NOVAS C 32 and 64bit DLLs can be found
            rc = SetDllDirectory(CommonProgramFilesPath + VIDEOUTILS_DLL_LOCATION);

            TL = new TraceLogger("NativeHelpers");
            TL.Enabled = RegistryCommonCode.GetBool(GlobalConstants.TRACE_UTIL, GlobalConstants.TRACE_UTIL_DEFAULT);

        }

        #region Public NativeHelpers Interface

        private int GetBitmapPixels(int width, int height, int bpp, FlipMode flipMode, int[,] pixels, byte[] bitmapBytes)
        {
            return 0;
        }

        private int GetColourBitmapPixels(int width, int height, int bpp, FlipMode flipMode, int[, ,] pixels, byte[] bitmapBytes)
        {
            return 0;
        }

        private int GetMonochromePixelsFromBitmap(int width, int height, int bpp, FlipMode flipMode, IntPtr hBitmap, int[,] bitmapPixels, byte[] bitmapBytes, int mode)
        {
            return 0;
        }

        private int GetColourPixelsFromBitmap(int width, int height, int bpp, FlipMode flipMode, IntPtr hBitmap, int[, ,] bitmapPixels, byte[] bitmapBytes)
        {
            return 0;
        }
        internal int SetGamma(double gamma)
        {
            return 0;
        }

        internal int ApplyGammaBrightness(int width, int height, int bpp, int[,] pixelsIn, int[,] pixelsOut, short brightness)
        {
            return 0;
        }

        private int GetBitmapBytes(int width, int height, IntPtr hBitmap, byte[] bitmapBytes)
        {
            return 0;
        }

        internal int InitFrameIntegration(int width, int height, int bpp)
        {
            return 0;
        }

        internal int AddFrameForIntegration(int[,] pixels)
        {
            return 0;
        }

        internal int GetResultingIntegratedFrame(int[,] pixels)
        {
            return 0;
        }

        internal int CreateNewAviFile(string fileName, int width, int height, int bpp, double fps, bool showCompressionDialog)
        {
            return 0;
        }

        internal int AviFileAddFrame(int[,] pixels)
        {
            return 0;
        }

        internal int AviFileClose()
        {
            return 0;
        }

        internal int GetLastAviFileError(IntPtr errorMessage)
        {
            return 0;
        }

        internal uint GetUsedAviCompression()
        {
            return 0;
        }

        internal int SetWhiteBalance(int newWhiteBalance)
        {
            return 0;
        }

        #endregion

        #region DLL entry points to 32bit DLL

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetBitmapPixels32(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In, MarshalAs(UnmanagedType.LPArray)] int[,] pixels,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetColourBitmapPixels32(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In, MarshalAs(UnmanagedType.LPArray)] int[, ,] pixels,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetMonochromePixelsFromBitmap32(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In] IntPtr hBitmap,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[,] bitmapPixels,
            [In, Out] byte[] bitmapBytes,
            int mode);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetColourPixelsFromBitmap32(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In] IntPtr hBitmap,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[, ,] bitmapPixels,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetGamma32(double gamma);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ApplyGammaBrightness32(
            int width,
            int height,
            int bpp,
            [In, Out] int[,] pixelsIn,
            [In, Out] int[,] pixelsOut,
            short brightness);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetBitmapBytes32(
            int width,
            int height,
            [In] IntPtr hBitmap,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int InitFrameIntegration32(int width, int height, int bpp);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int AddFrameForIntegration32([In, Out] int[,] pixels);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetResultingIntegratedFrame32([In, Out] int[,] pixels);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CreateNewAviFile32([MarshalAs(UnmanagedType.LPStr)]string fileName, int width, int height, int bpp, double fps, bool showCompressionDialog);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int AviFileAddFrame32([In, MarshalAs(UnmanagedType.LPArray)] int[,] pixels);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int AviFileClose32();

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetLastAviFileError32(IntPtr errorMessage);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint GetUsedAviCompression32();

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetWhiteBalance32(int newWhiteBalance);

        #endregion

        #region DLL entry points to 64bit DLL

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetBitmapPixels64(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In, MarshalAs(UnmanagedType.LPArray)] int[,] pixels,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetColourBitmapPixels64(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In, MarshalAs(UnmanagedType.LPArray)] int[, ,] pixels,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetMonochromePixelsFromBitmap64(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In] IntPtr hBitmap,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[,] bitmapPixels,
            [In, Out] byte[] bitmapBytes,
            int mode);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetColourPixelsFromBitmap64(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In] IntPtr hBitmap,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[, ,] bitmapPixels,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetGamma64(double gamma);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ApplyGammaBrightness64(
            int width,
            int height,
            int bpp,
            [In, Out] int[,] pixelsIn,
            [In, Out] int[,] pixelsOut,
            short brightness);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetBitmapBytes64(
            int width,
            int height,
            [In] IntPtr hBitmap,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int InitFrameIntegration64(int width, int height, int bpp);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int AddFrameForIntegration64([In, Out] int[,] pixels);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetResultingIntegratedFrame64([In, Out] int[,] pixels);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CreateNewAviFile64([MarshalAs(UnmanagedType.LPStr)]string fileName, int width, int height, int bpp, double fps, bool showCompressionDialog);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int AviFileAddFrame64([In, MarshalAs(UnmanagedType.LPArray)] int[,] pixels);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int AviFileClose64();

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetLastAviFileError64(IntPtr errorMessage);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint GetUsedAviCompression64();

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetWhiteBalance64(int newWhiteBalance);

        #endregion

        #region Gneral Windows DLL calls

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        #endregion

        #region NativeHelper Support functions

        internal byte[] PrepareBitmapForDisplay(int[,] imageArray, int width, int height, FlipMode flipMode)
        {
            return PrepareBitmapForDisplay(imageArray, width, height, false, flipMode);
        }

        internal byte[] PrepareBitmapForDisplay(object[,] imageArray, int width, int height, FlipMode flipMode)
        {
            return PrepareBitmapForDisplay(imageArray, width, height, true, flipMode);
        }

        internal byte[] PrepareColourBitmapForDisplay(int[, ,] imageArray, int width, int height, FlipMode flipMode)
        {
            return PrepareColourBitmapForDisplay(imageArray, width, height, false, flipMode);
        }

        internal byte[] PrepareColourBitmapForDisplay(object[, ,] imageArray, int width, int height, FlipMode flipMode)
        {
            return PrepareColourBitmapForDisplay(imageArray, width, height, true, flipMode);
        }

        internal object GetMonochromePixelsFromBitmap(Bitmap bitmap, LumaConversionMode conversionMode, FlipMode flipMode, out byte[] rawBitmapBytes)
        {
            int[,] bitmapPixels = new int[bitmap.Width, bitmap.Height];
            rawBitmapBytes = new byte[(bitmap.Width * bitmap.Height * 3) + 40 + 14 + 1];

            IntPtr hBitmap = bitmap.GetHbitmap();
            try
            {
                GetMonochromePixelsFromBitmap(bitmap.Width, bitmap.Height, 8, flipMode, hBitmap, bitmapPixels, rawBitmapBytes, (int)conversionMode);
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return bitmapPixels;
        }

        internal object GetColourPixelsFromBitmap(Bitmap bitmap, FlipMode flipMode, out byte[] rawBitmapBytes)
        {
            int[, ,] bitmapPixels = new int[bitmap.Width, bitmap.Height, 3];
            rawBitmapBytes = new byte[(bitmap.Width * bitmap.Height * 3) + 40 + 14 + 1];

            IntPtr hBitmap = bitmap.GetHbitmap();
            try
            {
                GetColourPixelsFromBitmap(bitmap.Width, bitmap.Height, 8, flipMode, hBitmap, bitmapPixels, rawBitmapBytes);
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return bitmapPixels;
        }

        internal byte[] GetBitmapBytes(Bitmap bitmap)
        {
            byte[] rawBitmapBytes = new byte[(bitmap.Width * bitmap.Height * 3) + 40 + 14 + 1];

            IntPtr hBitmap = bitmap.GetHbitmap();
            try
            {
                GetBitmapBytes(bitmap.Width, bitmap.Height, hBitmap, rawBitmapBytes);
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return rawBitmapBytes;
        }

        private byte[] PrepareBitmapForDisplay(object imageArray, int width, int height, bool useVariantPixels, FlipMode flipMode)
        {
            int[,] pixels = new int[height, width];

            if (useVariantPixels)
            {
                Array safeArr = (Array)imageArray;
                Array.Copy(safeArr, pixels, pixels.Length);
            }
            else
            {
                Array safeArr = (Array)imageArray;
                Array.Copy(safeArr, pixels, pixels.Length);
            }

            byte[] rawBitmapBytes = new byte[(width * height * 3) + 40 + 14 + 1];

            GetBitmapPixels(width, height, (int)8, flipMode, pixels, rawBitmapBytes);

            return rawBitmapBytes;
        }

        private byte[] PrepareColourBitmapForDisplay(object imageArray, int width, int height, bool useVariantPixels, FlipMode flipMode)
        {
            int[, ,] pixels = new int[height, width, 3];

            if (useVariantPixels)
            {
                Array safeArr = (Array)imageArray;
                Array.Copy(safeArr, pixels, pixels.Length);
            }
            else
            {
                Array safeArr = (Array)imageArray;
                Array.Copy(safeArr, pixels, pixels.Length);
            }

            byte[] rawBitmapBytes = new byte[(width * height * 3) + 40 + 14 + 1];

            GetColourBitmapPixels(width, height, (int)8, flipMode, pixels, rawBitmapBytes);

            return rawBitmapBytes;
        }

        #endregion

        #region Private Utility code

        //Constants for SHGetSpecialFolderPath shell call
        //0x0026
        private const int CSIDL_PROGRAM_FILES = 38;
        //0x002a,
        private const int CSIDL_PROGRAM_FILESX86 = 42;
        // 0x0024,
        private const int CSIDL_WINDOWS = 36;
        // 0x002c,
        private const int CSIDL_PROGRAM_FILES_COMMONX86 = 44;

        //DLL to provide the path to Program Files(x86)\Common Files folder location that is not avialable through the .NET framework
        /// <summary>
        /// Get path to a system folder
        /// </summary>
        /// <param name="hwndOwner">Supply null / nothing to use "current user"</param>
        /// <param name="lpszPath">returned string folder path</param>
        /// <param name="nFolder">Folder Number from CSIDL enumeration e.g. CSIDL_PROGRAM_FILES_COMMONX86 = 44 = 0x2c</param>
        /// <param name="fCreate">Indicates whether the folder should be created if it does not already exist. If this value is nonzero, 
        /// the folder is created. If this value is zero, the folder is not created</param>
        /// <returns>TRUE if successful; otherwise, FALSE.</returns>
        /// <remarks></remarks>
        [DllImport("shell32.dll")]
        private static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, [Out()] System.Text.StringBuilder lpszPath, int nFolder, bool fCreate);

        //Declare the api call that sets the additional DLL search directory
        [DllImport("kernel32.dll", SetLastError = false)]
        private static extern bool SetDllDirectory(string lpPathName);

        private static bool Is64Bit()
        {
            //Check whether we are running on a 32 or 64bit system.
            if (IntPtr.Size == 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion
    }
}
