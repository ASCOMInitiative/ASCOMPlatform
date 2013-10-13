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

    internal class NativeHelpers : IDisposable
    {
        private const string VIDEOUTILS32_DLL_NAME = "ASCOM.NativeVideo32.dll";
        private const string VIDEOUTILS64_DLL_NAME = "ASCOM.NativeVideo64.dll";

        private const string VIDEOUTILS_DLL_LOCATION = @"\ASCOM\VideoUtilities"; //This is appended to the Common Files path
        private TraceLogger TL;

        private int rc;
        private uint urc;

        #region Initialisers and Dispose

        public NativeHelpers()
        {
            TL = new TraceLogger("NativeHelpers");
            TL.Enabled = RegistryCommonCode.GetBool(GlobalConstants.TRACE_UTIL, GlobalConstants.TRACE_UTIL_DEFAULT);
            try
            {
                bool rc = false;
                string CommonProgramFilesPath = null;
                System.Text.StringBuilder ReturnedPath = new System.Text.StringBuilder(260);

                //Find the root location of the common files directory containing the ASCOM support files.
                if (Is64Bit()) // 64bit application so find the 32bit folder location, usually Program Files (x86)\Common Files
                {
                    CommonProgramFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86);
                }
                else //32bit application so just go with the .NET returned value usually, Program Files\Common Files
                {
                    CommonProgramFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
                }

                //Add the ASCOM\.net directory to the DLL search path so that the NOVAS C 32 and 64bit DLLs can be found
                rc = SetDllDirectory(CommonProgramFilesPath + VIDEOUTILS_DLL_LOCATION);
                TL .LogMessage("NativeHelpers", "Created");
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("NativeHelpers ctor", ex.ToString());
            }
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                    if ((TL != null))
                    {
                        TL.Enabled = false;
                        TL.Dispose();
                        TL = null;
                    }
                }
                // Free your own state (unmanaged objects) and set large fields to null.
                try
                {

                }
                catch
                {
                }
            }
            this.disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Public NativeHelpers Interface

        internal int GetBitmapPixels(int width, int height, int bpp, FlipMode flipMode, int[,] pixels, ref byte[] bitmapBytes)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = GetBitmapPixels64(width, height, bpp, flipMode, pixels, bitmapBytes);
            }
            else // 32bit call
            {
                rc = GetBitmapPixels32(width, height, bpp, flipMode, pixels, bitmapBytes);
            }
            return rc;
        }

        internal int GetColourBitmapPixels(int width, int height, int bpp, FlipMode flipMode, int[, ,] pixels, ref byte[] bitmapBytes)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = GetColourBitmapPixels64(width, height, bpp, flipMode, pixels, bitmapBytes);
            }
            else // 32bit call
            {
                rc = GetColourBitmapPixels32(width, height, bpp, flipMode, pixels, bitmapBytes);
            }
            return rc;
        }

        internal int GetMonochromePixelsFromBitmap(int width, int height, int bpp, FlipMode flipMode, IntPtr hBitmap, ref int[,] bitmapPixels, ref byte[] bitmapBytes, int mode)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = GetMonochromePixelsFromBitmap64(width, height, bpp, flipMode, hBitmap, bitmapPixels, bitmapBytes, mode);
            }
            else // 32bit call
            {
                rc = GetMonochromePixelsFromBitmap32(width, height, bpp, flipMode, hBitmap, bitmapPixels, bitmapBytes, mode);
            }
            return rc;
        }

        internal int GetColourPixelsFromBitmap(int width, int height, int bpp, FlipMode flipMode, IntPtr hBitmap, ref int[, ,] bitmapPixels, ref byte[] bitmapBytes)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = GetColourPixelsFromBitmap64(width, height, bpp, flipMode, hBitmap, bitmapPixels, bitmapBytes);
            }
            else // 32bit call
            {
                rc = GetColourPixelsFromBitmap32(width, height, bpp, flipMode, hBitmap, bitmapPixels, bitmapBytes);
            }
            return rc;
        }
        internal int SetGamma(double gamma)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = SetGamma64(gamma);
            }
            else // 32bit call
            {
                rc = SetGamma32(gamma);
            }
            return rc;
        }

        internal int ApplyGammaBrightness(int width, int height, int bpp, ref int[,] pixelsIn, ref int[,] pixelsOut, short brightness)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = ApplyGammaBrightness64(width, height, bpp, pixelsIn, pixelsOut, brightness);
            }
            else // 32bit call
            {
                rc = ApplyGammaBrightness32(width, height, bpp, pixelsIn, pixelsOut, brightness);
            }
            return rc;
        }

        internal int GetBitmapBytes(int width, int height, IntPtr hBitmap, ref byte[] bitmapBytes)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = GetBitmapBytes64(width, height, hBitmap, bitmapBytes);
            }
            else // 32bit call
            {
                rc = GetBitmapBytes32(width, height, hBitmap, bitmapBytes);
            }
            return rc;
        }

        internal int InitFrameIntegration(int width, int height, int bpp)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = InitFrameIntegration64(width, height, bpp);
            }
            else // 32bit call
            {
                rc = InitFrameIntegration32(width, height, bpp);
            }
            return rc;
        }

        internal int AddFrameForIntegration(ref int[,] pixels)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = AddFrameForIntegration64(pixels);
            }
            else // 32bit call
            {
                rc = AddFrameForIntegration64(pixels);
            }
            return rc;
        }

        internal int GetResultingIntegratedFrame(ref int[,] pixels)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = GetResultingIntegratedFrame64(pixels);
            }
            else // 32bit call
            {
                rc = GetResultingIntegratedFrame32(pixels);
            }
            return rc;
        }

        internal int CreateNewAviFile(string fileName, int width, int height, int bpp, double fps, bool showCompressionDialog)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = CreateNewAviFile64(fileName, width, height, bpp, fps, showCompressionDialog);
            }
            else // 32bit call
            {
                rc = CreateNewAviFile32(fileName, width, height, bpp, fps, showCompressionDialog);
            }
            return rc;
        }

        internal int AviFileAddFrame(int[,] pixels)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = AviFileAddFrame64(pixels);
            }
            else // 32bit call
            {
                rc = AviFileAddFrame32(pixels);
            }
            return rc;
        }

        internal int AviFileClose()
        {
            if (Is64Bit()) // 64bit call
            {
                rc = AviFileClose64();
            }
            else // 32bit call
            {
                rc = AviFileClose32();
            }
            return rc;
        }

        internal int GetLastAviFileError(IntPtr errorMessage)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = GetLastAviFileError64(errorMessage);
            }
            else // 32bit call
            {
                rc = GetLastAviFileError32(errorMessage);
            }
            return rc;
        }

        internal uint GetUsedAviCompression()
        {
            if (Is64Bit()) // 64bit call
            {
                urc = GetUsedAviCompression64();
            }
            else // 32bit call
            {
                urc = GetUsedAviCompression32();
            }
            return urc;
        }

        internal int SetWhiteBalance(int newWhiteBalance)
        {
            if (Is64Bit()) // 64bit call
            {
                rc = SetWhiteBalance64(newWhiteBalance);
            }
            else // 32bit call
            {
                rc = SetWhiteBalance32(newWhiteBalance);
            }
            return rc;
        }

        #endregion

        #region DLL entry points to 32bit DLL

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetBitmapPixels")]
        private static extern int GetBitmapPixels32(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In, MarshalAs(UnmanagedType.LPArray)] int[,] pixels,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetColourBitmapPixels")]
        private static extern int GetColourBitmapPixels32(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In, MarshalAs(UnmanagedType.LPArray)] int[, ,] pixels,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetMonochromePixelsFromBitmap")]
        private static extern int GetMonochromePixelsFromBitmap32(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In] IntPtr hBitmap,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[,] bitmapPixels,
            [In, Out] byte[] bitmapBytes,
            int mode);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetColourPixelsFromBitmap")]
        private static extern int GetColourPixelsFromBitmap32(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In] IntPtr hBitmap,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[, ,] bitmapPixels,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetGamma")]
        private static extern int SetGamma32(double gamma);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ApplyGammaBrightness")]
        private static extern int ApplyGammaBrightness32(
            int width,
            int height,
            int bpp,
            [In, Out] int[,] pixelsIn,
            [In, Out] int[,] pixelsOut,
            short brightness);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetBitmapBytes")]
        private static extern int GetBitmapBytes32(
            int width,
            int height,
            [In] IntPtr hBitmap,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "InitFrameIntegration")]
        private static extern int InitFrameIntegration32(int width, int height, int bpp);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "AddFrameForIntegration")]
        private static extern int AddFrameForIntegration32([In, Out] int[,] pixels);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetResultingIntegratedFrame32")]
        private static extern int GetResultingIntegratedFrame32([In, Out] int[,] pixels);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateNewAviFile")]
        private static extern int CreateNewAviFile32([MarshalAs(UnmanagedType.LPStr)]string fileName, int width, int height, int bpp, double fps, bool showCompressionDialog);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "AviFileAddFrame")]
        private static extern int AviFileAddFrame32([In, MarshalAs(UnmanagedType.LPArray)] int[,] pixels);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "AviFileClose")]
        private static extern int AviFileClose32();

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetLastAviFileError")]
        private static extern int GetLastAviFileError32(IntPtr errorMessage);

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetUsedAviCompression")]
        private static extern uint GetUsedAviCompression32();

        [DllImport(VIDEOUTILS32_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetWhiteBalance")]
        private static extern int SetWhiteBalance32(int newWhiteBalance);

        #endregion

        #region DLL entry points to 64bit DLL

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetBitmapPixels")]
        private static extern int GetBitmapPixels64(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In, MarshalAs(UnmanagedType.LPArray)] int[,] pixels,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetColourBitmapPixels")]
        private static extern int GetColourBitmapPixels64(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In, MarshalAs(UnmanagedType.LPArray)] int[, ,] pixels,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetMonochromePixelsFromBitmap")]
        private static extern int GetMonochromePixelsFromBitmap64(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In] IntPtr hBitmap,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[,] bitmapPixels,
            [In, Out] byte[] bitmapBytes,
            int mode);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetColourPixelsFromBitmap")]
        private static extern int GetColourPixelsFromBitmap64(
            int width,
            int height,
            int bpp,
            FlipMode flipMode,
            [In] IntPtr hBitmap,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] int[, ,] bitmapPixels,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetGamma")]
        private static extern int SetGamma64(double gamma);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ApplyGammaBrightness")]
        private static extern int ApplyGammaBrightness64(
            int width,
            int height,
            int bpp,
            [In, Out] int[,] pixelsIn,
            [In, Out] int[,] pixelsOut,
            short brightness);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetBitmapBytes")]
        private static extern int GetBitmapBytes64(
            int width,
            int height,
            [In] IntPtr hBitmap,
            [In, Out] byte[] bitmapBytes);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "InitFrameIntegration")]
        private static extern int InitFrameIntegration64(int width, int height, int bpp);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "AddFrameForIntegration")]
        private static extern int AddFrameForIntegration64([In, Out] int[,] pixels);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetResultingIntegratedFrame64")]
        private static extern int GetResultingIntegratedFrame64([In, Out] int[,] pixels);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateNewAviFile")]
        private static extern int CreateNewAviFile64([MarshalAs(UnmanagedType.LPStr)]string fileName, int width, int height, int bpp, double fps, bool showCompressionDialog);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "AviFileAddFrame")]
        private static extern int AviFileAddFrame64([In, MarshalAs(UnmanagedType.LPArray)] int[,] pixels);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "AviFileClose")]
        private static extern int AviFileClose64();

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetLastAviFileError")]
        private static extern int GetLastAviFileError64(IntPtr errorMessage);

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetUsedAviCompression")]
        private static extern uint GetUsedAviCompression64();

        [DllImport(VIDEOUTILS64_DLL_NAME, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetWhiteBalance")]
        private static extern int SetWhiteBalance64(int newWhiteBalance);

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
                GetMonochromePixelsFromBitmap(bitmap.Width, bitmap.Height, 8, flipMode, hBitmap, ref bitmapPixels, ref rawBitmapBytes, (int)conversionMode);
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
                GetColourPixelsFromBitmap(bitmap.Width, bitmap.Height, 8, flipMode, hBitmap, ref bitmapPixels, ref rawBitmapBytes);
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
                GetBitmapBytes(bitmap.Width, bitmap.Height, hBitmap, ref rawBitmapBytes);
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

            GetBitmapPixels(width, height, (int)8, flipMode, pixels, ref rawBitmapBytes);

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

            GetColourBitmapPixels(width, height, (int)8, flipMode, pixels, ref rawBitmapBytes);

            return rawBitmapBytes;
        }

        #endregion

        #region Private Utility code

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
