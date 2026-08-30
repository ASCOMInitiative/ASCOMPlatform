using System.Drawing;
using Simulator.VideoCameraImpl;
using Xunit;

namespace PlatformUnitTests
{
    public class BitmapHelperTests
    {
        // Helper: create a 24bpp bitmap and set every pixel to the given Color.
        private static Bitmap SolidBitmap(int width, int height, Color color)
        {
            var bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(bmp))
                g.Clear(color);
            return bmp;
        }

        [Fact]
        public void BlackBitmap_AllPixelsZero()
        {
            using (var bmp = SolidBitmap(4, 4, Color.Black))
            {
                int[,] pixels = BitmapHelper.CopyBitmapPixels(bmp);

                for (int y = 0; y < 4; y++)
                    for (int x = 0; x < 4; x++)
                        Assert.Equal(0, pixels[y, x]);
            }
        }

        [Fact]
        public void WhiteBitmap_AllPixels255()
        {
            using (var bmp = SolidBitmap(4, 4, Color.White))
            {
                int[,] pixels = BitmapHelper.CopyBitmapPixels(bmp);

                for (int y = 0; y < 4; y++)
                    for (int x = 0; x < 4; x++)
                        Assert.Equal(255, pixels[y, x]);
            }
        }

        [Fact]
        public void PureRedBitmap_AllPixels255()
        {
            // Red channel = 255, green = 0, blue = 0
            using (var bmp = SolidBitmap(4, 4, Color.FromArgb(255, 0, 0)))
            {
                int[,] pixels = BitmapHelper.CopyBitmapPixels(bmp);

                for (int y = 0; y < 4; y++)
                    for (int x = 0; x < 4; x++)
                        Assert.Equal(255, pixels[y, x]);
            }
        }

        [Fact]
        public void PureBlueBitmap_AllPixelsZero()
        {
            // Red channel = 0, green = 0, blue = 255 — only red is extracted
            using (var bmp = SolidBitmap(4, 4, Color.FromArgb(0, 0, 255)))
            {
                int[,] pixels = BitmapHelper.CopyBitmapPixels(bmp);

                for (int y = 0; y < 4; y++)
                    for (int x = 0; x < 4; x++)
                        Assert.Equal(0, pixels[y, x]);
            }
        }

        [Fact]
        public void SpecificRedValue_CorrectlyExtracted()
        {
            const int expectedRed = 123;
            using (var bmp = SolidBitmap(4, 4, Color.FromArgb(expectedRed, 45, 67)))
            {
                int[,] pixels = BitmapHelper.CopyBitmapPixels(bmp);

                for (int y = 0; y < 4; y++)
                    for (int x = 0; x < 4; x++)
                        Assert.Equal(expectedRed, pixels[y, x]);
            }
        }

        [Fact]
        public void ResultArray_DimensionsAreHeightByWidth()
        {
            // Deliberately non-square so [height, width] order is verified
            using (var bmp = SolidBitmap(10, 3, Color.White))
            {
                int[,] pixels = BitmapHelper.CopyBitmapPixels(bmp);

                Assert.Equal(3, pixels.GetLength(0));   // height (rows)
                Assert.Equal(10, pixels.GetLength(1));  // width (columns)
            }
        }

        [Fact]
        public void NonStrideAlignedWidth_CorrectValues()
        {
            // Width=5: stride = ceil(5*3/4)*4 = 16, so there are 1 byte padding per row.
            // Verifies that stride padding does not corrupt pixel values.
            const int expectedRed = 200;
            using (var bmp = SolidBitmap(5, 4, Color.FromArgb(expectedRed, 10, 20)))
            {
                int[,] pixels = BitmapHelper.CopyBitmapPixels(bmp);

                Assert.Equal(4, pixels.GetLength(0));
                Assert.Equal(5, pixels.GetLength(1));
                for (int y = 0; y < 4; y++)
                    for (int x = 0; x < 5; x++)
                        Assert.Equal(expectedRed, pixels[y, x]);
            }
        }

        [Fact]
        public void IndividualPixels_IndexedByRowThenColumn()
        {
            // Draw a 2x2 bitmap where each pixel has a distinct red value,
            // then verify pixels[row, col] maps correctly.
            var bmp = new Bitmap(2, 2, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            // (col=0,row=0)=red 10, (col=1,row=0)=red 20, (col=0,row=1)=red 30, (col=1,row=1)=red 40
            bmp.SetPixel(0, 0, Color.FromArgb(10, 0, 0));
            bmp.SetPixel(1, 0, Color.FromArgb(20, 0, 0));
            bmp.SetPixel(0, 1, Color.FromArgb(30, 0, 0));
            bmp.SetPixel(1, 1, Color.FromArgb(40, 0, 0));

            int[,] pixels = BitmapHelper.CopyBitmapPixels(bmp);

            Assert.Equal(10, pixels[0, 0]);
            Assert.Equal(20, pixels[0, 1]);
            Assert.Equal(30, pixels[1, 0]);
            Assert.Equal(40, pixels[1, 1]);

            bmp.Dispose();
        }
    }
}
