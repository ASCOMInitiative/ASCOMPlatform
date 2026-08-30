using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Simulator.VideoCameraImpl
{
    /// <summary>
    /// Utility methods for working with bitmap pixel data.
    /// </summary>
    public static class BitmapHelper
    {
        /// <summary>
        /// Copies the red channel of each pixel in a 24bpp bitmap into a [height, width] integer array.
        /// </summary>
        public static int[,] CopyBitmapPixels(Bitmap bmp)
        {
            var pixels = new int[bmp.Height, bmp.Width];

            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            try
            {
                int stride = bmData.Stride;
                byte[] rgbValues = new byte[stride * bmp.Height];
                Marshal.Copy(bmData.Scan0, rgbValues, 0, rgbValues.Length);

                for (int y = 0; y < bmp.Height; ++y)
                    for (int x = 0; x < bmp.Width; ++x)
                        pixels[y, x] = rgbValues[y * stride + x * 3 + 2]; // red channel
            }
            finally
            {
                bmp.UnlockBits(bmData);
            }

            return pixels;
        }
    }
}
