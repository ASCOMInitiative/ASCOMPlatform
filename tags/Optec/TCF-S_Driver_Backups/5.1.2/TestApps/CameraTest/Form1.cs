using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ASCOM.DriverAccess;
using ASCOM.DeviceInterface;
using CameraTest.Properties;
using nom.tam.fits;
using nom.tam.util;

namespace CameraTest
{
    public partial class Form1 : Form
    {
        private Camera oCamera;     // camera object
        private string CameraID;    // camera ID string
        private Bitmap img;         // bitmap for the image
        private Array iarr;         // array for the image
        // start position of the image
        private decimal startX;
        private decimal startY;
        // size of the image
        private decimal numX;
        private decimal numY;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            if (oCamera == null || oCamera.Connected == false)
            {
                try
                {
                    CameraID = Camera.Choose(CameraID);
                    lblCameraName.Text = CameraID;
                }
                catch (Exception ex)
                {
                    String msg = ex.Message;
                    if (ex.InnerException != null)
                        msg += " - " + ex.InnerException.Message;
                    MessageBox.Show("Choose failed with error " + msg);
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == "Connect")
            {
                if (CameraID != "")
                {
                    try
                    {
                        oCamera = new Camera(CameraID);
                        oCamera.Connected = true;
                    }
                    catch (Exception ex)
                    {
                        String msg = ex.Message;
                        if (ex.InnerException != null)
                            msg += " - " + ex.InnerException.Message;
                        MessageBox.Show("Connect failed with error " + msg);
                    }
                }
            }
            else
            {
                oCamera.Connected = false;
            }
            if (oCamera.Connected)
            {
                timer1.Enabled = true;
                btnConnect.Text = "Disconnect";
                ShowParameters();
                numBinY.Value = 1;
                numBinX.Value = 1;
                numStartX.Value = 0;
                numStartY.Value = 0;
                numNumX.Value = oCamera.CameraXSize;
                numNumY.Value = oCamera.CameraYSize;
                this.Text = "Camera Test - " + oCamera.Description;
            }
            else
            {
                timer1.Enabled = false;
                btnConnect.Text = "Connect";
                this.Text = "Camera Test - No Camera";
            }
        }

        private void ShowParameters()
        {
            if (oCamera == null) return;
            lblSizeX.Text = oCamera.CameraXSize.ToString();
            lblSizeY.Text = oCamera.CameraYSize.ToString();
            lblPixelSizeX.Text = oCamera.PixelSizeX.ToString("F2");
            lblPixelSizeY.Text = oCamera.PixelSizeY.ToString("F2");
            lblMaxBinX.Text = oCamera.MaxBinX.ToString();
            lblMaxBinY.Text = oCamera.MaxBinY.ToString();

            try
            {
                lblElectronsPerADU.Text = oCamera.ElectronsPerADU.ToString();
            }
            catch
            {
                lblElectronsPerADU.Text = @"N/A";
            }
            try
            {
                lblFullWellCapacity.Text = oCamera.FullWellCapacity.ToString();
            }
            catch
            {
                lblFullWellCapacity.Text = @"N/A";
            }

            try
            {
                lblMaxADU.Text = oCamera.MaxADU.ToString();
            }
            catch
            {
                lblMaxADU.Text = @"n/a";
            }
            chkCanStopExposure.Checked = oCamera.CanStopExposure;
            chkCanAbortExposure.Checked = oCamera.CanAbortExposure;
            chkHasShutter.Checked = oCamera.HasShutter;
            chkCanAsymetricBin.Checked = oCamera.CanAsymmetricBin;
            chkCanPulseGuide.Checked = oCamera.CanPulseGuide;

            try
            {
                chkCanSetCCDTemp.Checked = oCamera.CanSetCCDTemperature;
                chkCanGetCoolerPower.Checked = oCamera.CanGetCoolerPower;
            }
            catch
            {
                chkCanSetCCDTemp.Enabled = false;
                chkCanGetCoolerPower.Enabled = false;
            }
            numStartX.Maximum = oCamera.CameraXSize - 1;
            numStartY.Maximum = oCamera.CameraYSize - 1;
            numNumX.Maximum = oCamera.CameraXSize;
            numNumY.Maximum = oCamera.CameraYSize;
            numBinX.Maximum = oCamera.MaxBinX;
            numBinY.Maximum = oCamera.MaxBinY;
            numBinY.Enabled = oCamera.CanAsymmetricBin;
            try
            {
                imageControl.Maximum = oCamera.MaxADU;
            }
            catch
            {
                imageControl.Maximum = 65535;
            }
            checkBoxDarkFrame.Enabled = oCamera.HasShutter;

            // set Camera Version 2 properties, the client should return this correctly, even for an unversioned driver.
            if (oCamera.InterfaceVersion >= 2)
            {
                groupBoxV2.Visible = true;
                labelSensorName.Text = oCamera.SensorName;
                labelSensorType.Text = (oCamera.SensorType).ToString();
                labelBayerOffsetX.Text = oCamera.BayerOffsetX.ToString();
                labelBayerOffsetY.Text = Convert.ToString(oCamera.BayerOffsetY);
                labelDriverVersion.Text = oCamera.DriverVersion;
                labelDriverName.Text = oCamera.Name;
                //DriverInfo - long
                numExposure.Minimum = (decimal)oCamera.ExposureMin;
                numExposure.Maximum = (decimal)oCamera.ExposureMax;
                checkBoxFastReadout.Enabled = checkBoxCanFastReadout.Checked;
                checkBoxCanFastReadout.Checked = oCamera.CanFastReadout;
                checkBoxFastReadout.Visible = checkBoxCanFastReadout.Checked;
                if (checkBoxCanFastReadout.Checked)
                {
                    checkBoxFastReadout.Checked = oCamera.FastReadout;
                }
            }
            else
            {
                groupBoxV2.Visible = false;
            }
        }

        private void ShowVariables()
        {
            try
            {
                lblCCDTemp.Text = oCamera.CCDTemperature.ToString();
            }
            catch
            {
                lblCCDTemp.Text = @"N/A";
            }
            try
            {
                lblHeatSinkTemp.Text = oCamera.HeatSinkTemperature.ToString();
            }
            catch
            {
                lblHeatSinkTemp.Text = @"N/a";
            }
            try
            {
                lblCoolerPower.Text = oCamera.CoolerPower.ToString();
            }
            catch
            {
                 lblCoolerPower.Text = @"N/a";
           }
            try
            {
                tsStatus.Text = oCamera.CameraState.ToString();
            }
            catch
            {
                tsStatus.Text = @"Unknown";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (CheckConnected)
            {
                ShowVariables();
            }
        }

        private void chkCoolerOn_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckConnected)
            {
                try
                {
                    oCamera.CoolerOn = chkCoolerOn.Checked;
                    if (oCamera.CoolerOn)
                        oCamera.SetCCDTemperature = (double)numericUpDownSetCCDTemperature.Value;
                }
                catch (Exception ex)
                {
                    tsError.Text = "CoolerOn error: " +  ex.Message;
                }
            }
        }

        private bool CheckConnected
        {
            get
            {
                if (oCamera == null) return false;
                return oCamera.Connected;
            }
        }

        private decimal LastExposure;

        private void numExposure_ValueChanged(object sender, EventArgs e)
        {
            ProcessExposure();
        }

        private void btnFullFrame_Click(object sender, EventArgs e)
        {
            if (CheckConnected)
            {
                numBinY.Value = 1;
                numBinX.Value = 1;
                numStartX.Value = 0;
                numStartY.Value = 0;
                numNumX.Value = oCamera.CameraXSize;
                numNumY.Value = oCamera.CameraYSize;
            }
        }

        private void numBinX_ValueChanged(object sender, EventArgs e)
        {
            if (!oCamera.CanAsymmetricBin)
                numBinY.Value=numBinX.Value;

            numNumX.Maximum = numStartX.Maximum = oCamera.CameraXSize / numBinX.Value;
            //numNumX.Increment =numStartX.Increment = numBinX.Value;
            numStartX.Value = startX / numBinX.Value;
            numNumX.Value = Math.Truncate(numX / numBinX.Value);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (CheckConnected)
            {
                tsError.Text = "";
                try
                {
                    oCamera.StartX = (int)numStartX.Value;
                    oCamera.StartY = (int)numStartY.Value;
                    oCamera.NumX = (int)numNumX.Value;
                    oCamera.NumY = (int)numNumY.Value;
                    oCamera.BinX = (short)numBinX.Value;
                    oCamera.BinY = (short)numBinY.Value;
                    bool light = (oCamera.HasShutter) ? !checkBoxDarkFrame.Checked : true;
                    oCamera.StartExposure((double)numExposure.Value, light);
                    ExposureTimer.Enabled = true;
                }
                catch (Exception ex)
                {
                    tsError.Text = "Start Error: " + ex.Message;
                }
            }
        }

        #region Show Image

        private void ShowImage()
        {
            if (iarr == null) return;

            // generate gamma LUT
            gamma = new int[256];
            double g = (double)imageControl.Gamma.Value;
            for (int i = 0; i < 256; i++)
            {
                gamma[i] = (byte)(Math.Pow((double)i / 256.0, g) * 256.0);
            }

            int stepX = 1;
            int stepY = 1;

            unsafe
            {
                DisplayProcess displayProcess = new DisplayProcess(MonochromeProcess);
                int width = iarr.GetLength(0);
                int height = iarr.GetLength(1);
                int stepH = 1;
                int stepW = 1;

                if (oCamera.InterfaceVersion >= 2)
                {
                    switch (oCamera.SensorType)
                    {
                        case ASCOM.DeviceInterface.SensorType.Monochrome:
                            break;
                        case ASCOM.DeviceInterface.SensorType.RGGB:
                            displayProcess = new DisplayProcess(RGGBProcess);
                            stepX = 2;
                            stepY = 2;
                            break;
                        case ASCOM.DeviceInterface.SensorType.CMYG:
                            displayProcess = new DisplayProcess(CMYGProcess);
                            stepX = 2;
                            stepY = 2;
                            break;
                        case ASCOM.DeviceInterface.SensorType.LRGB:
                            displayProcess = new DisplayProcess(LRGBProcess);
                            stepX = 4;
                            stepY = 4;
                            stepH = 2;
                            stepW = 2;
                            break;
                        case ASCOM.DeviceInterface.SensorType.CMYG2:
                            displayProcess = new DisplayProcess(CMYG2Process);
                            stepX = 2;
                            stepY = 4;
                            stepH = 2;
                            break;
                        case ASCOM.DeviceInterface.SensorType.Color:
                            displayProcess = new DisplayProcess(ColourProcess);
                            break;
                        default:
                            break;
                    }
                    width /= (stepX/stepW);
                    height /= (stepY/stepH);
                    // set the bayer offsets
                    x0 = (oCamera.BayerOffsetX + oCamera.StartX * oCamera.BinX) & (stepX - 1);
                    x1 = (x0 + 1) & (stepX - 1);
                    x2 = (x0 + 2) & (stepX - 1);
                    x3 = (x0 + 3) & (stepX - 1);
                    y0 = (oCamera.BayerOffsetY + oCamera.StartY * oCamera.BinY) & (stepY - 1);
                    y1 = (y0 + 1) & (stepY - 1);
                    y2 = (y0 + 2) & (stepY - 1);
                    y3 = (y0 + 3) & (stepY - 1);
                }

                img = new Bitmap(width, height, PixelFormat.Format24bppRgb);

                BitmapData data = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                try
                {
                    // pointer to locked bitmap data
                    byte* imgPtr = (byte*)(data.Scan0);
                    // black level
                    blackLevel = (int)imageControl.MinValue;
                    // scale, white-black
                    scale = (int)imageControl.MaxValue - blackLevel;
                    stride = data.Stride;

                    int yy = 0;
                    for (int y = 0; y < height; y+= stepH)
                    {
                        int xx = 0;
                        for (int x = 0; x < width; x+=stepW)
                        {
                            displayProcess(xx, yy, imgPtr);
                            xx += stepX;
                            imgPtr += (3 * stepW);
                        }
                        imgPtr += data.Stride - data.Width * 3 + (stepH - 1) * data.Stride;
                        yy += stepY;
                    }
                }
                finally
                {
                    img.UnlockBits(data);
                }
            }
            imageControl.Histogram(histogram);
            zoom = (float)Math.Pow(10, trkZoom.Value / 100.0);
            splitContainer1.Panel2.AutoScrollMinSize = new Size((int)(img.Width * zoom), (int)(img.Height * zoom));
            splitContainer1.Panel2.Invalidate();
        }

        int[] gamma;
        int blackLevel;
        int scale;
        int stride;

        // bayer offsets
        int x0;
        int x1;
        int x2;
        int x3;
        int y0;
        int y1;
        int y2;
        int y3;

        // use delegates to select display process
        private unsafe delegate void DisplayProcess(int x, int y, byte* imgPtr);

        // these processes take one cell of the image and generate the rgb values from the contents of the cell
        // then use loadRGB to put the RGB values in the image

        private unsafe void MonochromeProcess(int x, int y, byte* imgPtr)
        {
            int k = Convert.ToInt32(iarr.GetValue(x, y));
            loadRGB(k, k, k, imgPtr);
        }

        private unsafe void RGGBProcess(int x, int y, byte* imgPtr)
        {
            int r = Convert.ToInt32(iarr.GetValue(x+x0, y+y0));
            int g = Convert.ToInt32(iarr.GetValue(x+x0, y+y1));
            int b = Convert.ToInt32(iarr.GetValue(x+x1, y+y1));
            g += Convert.ToInt32(iarr.GetValue(x+x1, y+y0));
            g /= 2;
            loadRGB(r, g, b, imgPtr);
        }

        private unsafe void CMYGProcess(int x, int h, byte* imgPtr)
        {
            // get the cmyg values
            int y = Convert.ToInt32(iarr.GetValue(x+x0, h+y0));
            int c = Convert.ToInt32(iarr.GetValue(x+x1, h+y0));
            int g = Convert.ToInt32(iarr.GetValue(x+x0, h+y1));
            int m = Convert.ToInt32(iarr.GetValue(x+x1, h+y1));
            // convert to rgb, c = g + b, y = r + g, m = r + b
            int r = y + m - c;
            int b = c + m - y;
            g += (c + y - m);
            loadRGB(r, g/2, b, imgPtr);
        }

        private unsafe void CMYG2Process(int x, int h, byte* imgPtr)
        {
            // get the cmyg values for the top pixel
            int g = Convert.ToInt32(iarr.GetValue(x+x0, h+y0));
            int m = Convert.ToInt32(iarr.GetValue(x+x1, h+y0));
            int c = Convert.ToInt32(iarr.GetValue(x+x0, h+y1));
            int y = Convert.ToInt32(iarr.GetValue(x+x1, h+y1));
            // convert to rgb, c = g + b, y = r + g, m = r + b
            int r = y + m - c;
            int b = c + m - y;
            g += (c + y - m);
            loadRGB(r, g/2, b, imgPtr);
            // and the bottom pixel
            m = Convert.ToInt32(iarr.GetValue(x+x0, h+y2));
            g = Convert.ToInt32(iarr.GetValue(x+x1, h+y2));
            c = Convert.ToInt32(iarr.GetValue(x+x0, h+y3));
            y = Convert.ToInt32(iarr.GetValue(x+x1, h+y3));
            // convert to rgb, c = g + b, y = r + g, m = r + b
            r = y + m - c;
            b = c + m - y;
            g += (c + y - m);
            loadRGB(r, g/2, b, imgPtr + stride);
        }

        private unsafe void LRGBProcess(int x, int y, byte* imgPtr)
        {
            // convert a 4 x 4 grid of input pixels to a 2 x2 grid of output pixels
            // get the lrgb values
            int l = Convert.ToInt32(iarr.GetValue(x+x0, y+y0));
            l += Convert.ToInt32(iarr.GetValue(x+x1, y+y1));
            int r = Convert.ToInt32(iarr.GetValue(x+x1, y+y0));
            r += Convert.ToInt32(iarr.GetValue(x+x0, y+y1));
            int g = Convert.ToInt32(iarr.GetValue(x+x0, y+y3));
            g += Convert.ToInt32(iarr.GetValue(x+x2, y+y1));
            int b = l - r - g;
            loadRGB(r/2, g/2, b/2, imgPtr);     // top left
            l = Convert.ToInt32(iarr.GetValue(x+x2, y+y0));
            l += Convert.ToInt32(iarr.GetValue(x+x3, y+y1));
            b = l - r - g;
            loadRGB(r/2, g/2, b/2, imgPtr+3);     // top right
            l = Convert.ToInt32(iarr.GetValue(x+x0, y+y2));
            l += Convert.ToInt32(iarr.GetValue(x+x1, y+y3));
            g = Convert.ToInt32(iarr.GetValue(x+x1, y+y2));
            g += Convert.ToInt32(iarr.GetValue(x+x0, y+y3));
            b = Convert.ToInt32(iarr.GetValue(x+x3, y+y2));
            b += Convert.ToInt32(iarr.GetValue(x+x2, y+y3));
            r = l - g - b;
            loadRGB(r/2, g/2, b/2, imgPtr+stride);     // bottom left
            l = Convert.ToInt32(iarr.GetValue(x+x2, y+y2));
            l += Convert.ToInt32(iarr.GetValue(x+x3, y+y3));
            r = l - b - g;
            loadRGB(r/2, g/2, b/2, imgPtr+stride+3);     // bottom right
        }

        private unsafe void ColourProcess(int w, int h, byte* imgPtr)
        {
            // get the rgb values from the three image planes
            int r = Convert.ToInt32(iarr.GetValue(w, h, 0));
            int g = Convert.ToInt32(iarr.GetValue(w, h, 1));
            int b = Convert.ToInt32(iarr.GetValue(w, h, 2));
            loadRGB(r, g, b, imgPtr);
        }

        private unsafe void loadRGB(int r, int g, int b, byte *imgPtr)
        {
            // convert 16 bit signed to 16 bit unsigned
            if (r < 0) r += 65535;
            if (g < 0) g += 65535;
            if (b < 0) b += 65535;
            // scale to range 0 to scale
            r = r - blackLevel;
            g = g - blackLevel;
            b = b - blackLevel;
            // scale to 0 to 255
            r = (int)(r * 255.0 / scale);
            g = (int)(g * 255.0 / scale);
            b = (int)(b * 255.0 / scale);
            // truncate to byte range, apply gamma and put into the image
            *imgPtr = (byte) gamma[Math.Min(Math.Max(b, 0), 255)];
            imgPtr++;
            *imgPtr = (byte) gamma[Math.Min(Math.Max(g, 0), 255)];
            imgPtr++;
            *imgPtr = (byte) gamma[Math.Min(Math.Max(r, 0), 255)];
        }

        #endregion

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (CheckConnected)
                try
                {
                    oCamera.StopExposure();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        MessageBox.Show("Inner " + ex.InnerException.Message);
                    else
                        MessageBox.Show("Error " + ex.Message);
                } 
                finally
                {
                    ShowImage();
                } 
        }


        private void ExposureTimer_Tick(object sender, EventArgs e)
        {
            if (CheckConnected)
            {
                if (oCamera.ImageReady)
                {
                    try
                    {
                        if (chkVariant.Checked)
                        {
                            Array oArr = (Array)oCamera.ImageArrayVariant;
                            // cast the array to int
                            if (oCamera.SensorType == ASCOM.DeviceInterface.SensorType.Color)
                            {
                                iarr = new int[oArr.GetLength(0), oArr.GetLength(1), 3];
                                for (int i = 0; i < iarr.GetLength(0); i++)
                                {
                                    for (int j = 0; j < iarr.GetLength(1); j++)
                                    {
                                        iarr.SetValue(Convert.ToInt32(oArr.GetValue(i, j, 0)), i, j, 0);
                                        iarr.SetValue(Convert.ToInt32(oArr.GetValue(i, j, 1)), i, j, 1);
                                        iarr.SetValue(Convert.ToInt32(oArr.GetValue(i, j, 2)), i, j, 2);
                                    }
                                }
                            }
                            else
                            {
                                iarr = new int[oArr.GetLength(0), oArr.GetLength(1)];
                                for (int i = 0; i < iarr.GetLength(0); i++)
                                {
                                    for (int j = 0; j < iarr.GetLength(1); j++)
                                    {
                                        iarr.SetValue(Convert.ToInt32(oArr.GetValue(i, j)), i, j);
                                    }
                                }
                            }
                        }
                        else
                        {
                            iarr = (Array)oCamera.ImageArray;
                        }
                    }
                    catch (Exception ex)
                    {
                        toolStripStatusLabel1.Text = "ImageArray(Variant) failed " + ex.Message;
                    }
                    int Max = 0, Min = 0;
                    double Mean = 0;
                    ImageParameters(ref Min, ref Max, ref Mean);
                    if (chkAuto.Checked)
                    {
                        imageControl.Minimum = Min;
                        imageControl.Maximum = Max;
                        imageControl.MinValue = Min;
                        imageControl.MaxValue = Max;
                    }
                    else
                    {
                        imageControl.Maximum = oCamera.MaxADU;
                        imageControl.Minimum = 0;
                    }
                    txtExposureDuration.Text = (oCamera.LastExposureDuration).ToString();
                    txtExposureStartTime.Text = oCamera.LastExposureStartTime;
                    ShowImage();
                    imageControl.Histogram(histogram);
                    //toolStripSplitButton1.Text = "Image OK";
                    ExposureTimer.Enabled = false;
                }
                else
                {
                    try
                    {
                        toolStripProgressBar.Value = oCamera.PercentCompleted;
                    }
                    catch{}
                }
            }
        }

        private void ImageParameters(ref int Min, ref int Max, ref double Mean)
        {
            if (iarr == null) return;

            decimal sum = 0;
            decimal sumsq = 0;
            int max = 0;
            int min = oCamera.MaxADU;
            int num = 0;
            unsafe
            {
                if (iarr.Rank == 3)
                {
                    fixed (int* pArr = (int[,,])iarr)
                    {
                        int* pA = (int*)pArr;

                        for (int i = 0; i < iarr.Length; i++)
                        {
                            //int v = Convert.ToInt32(iarr.GetValue(i, j));
                            int v = *pA;
                            if (v < 0) v = 65536 + v;
                            if (max < v) max = v;
                            if (min > v) min = v;
                            sum += *pA;
                            sumsq += v * v;
                            num++;
                            pA++;
                        }
                    }
                }
                else
                {
                    fixed (int* pArr = (int[,])iarr)
                    {
                        int* pA = (int*)pArr;

                        for (int i = 0; i < iarr.GetLength(0) * iarr.GetLength(1); i++)
                        {
                            //int v = Convert.ToInt32(iarr.GetValue(i, j));
                            int v = *pA;
                            if (v < 0) v = 65536 + v;
                            if (max < v) max = v;
                            if (min > v) min = v;
                            sum += *pA;
                            sumsq += v * v;
                            num++;
                            pA++;
                        }
                    }
                }
            }
            decimal var = (sumsq - (sum * sum) / num) / num;
            double sd = Math.Sqrt((double)var);
            if (min < 0) min = 0;
            if (max > oCamera.MaxADU) max = oCamera.MaxADU;
            Max = max;
            Min = min;
            Mean = (int)(sum / num);
            MakeHistogram(min, max);
        }

        private int[] histogram;

        private void MakeHistogram(int min, int max)
        {
            histogram = new int[256];
            double s = (double)255/(max-min);
            if (max <= min) s = 1;
            unsafe
            {
                switch (iarr.Rank)
	            {
                    case 2:
                        fixed (int* pArr = (int[,])iarr)
                        {
                            int* pA = pArr;
                            for (int i = 0; i < iarr.GetLength(1) * iarr.GetLength(0); i++)
                            {
                                int v = *pA++;
                                if (v < 0) v = 65536 + v;
                                int idx = (int)((v - min) * s);
                                if (idx >= 0 && idx <= 255)
                                    histogram[idx]++;
                            }
                        }
                        break;
                    case 3:
                        fixed (int* pArr = (int[,,])iarr)
                        {
                            int* pA = pArr;
                            for (int i = 0; i < iarr.Length; i++)
                            {
                                int v = *pA++;
                                if (v < 0) v = 65536 + v;
                                int idx = (int)((v - min) * s);
                                if (idx >= 0 && idx <= 255)
                                    histogram[idx]++;
                            }
                        }
                        break;
                    default:
                        break;
	            } 

            }
        }

        private void numBinY_ValueChanged(object sender, EventArgs e)
        {
            numNumY.Maximum = numStartY.Maximum = oCamera.CameraYSize / numBinY.Value;
            //numNumY.Increment = numStartY.Increment = numBinY.Value;
            numNumY.Value = numY / numBinY.Value;
            numStartY.Value = startY / numBinY.Value;
        }

        private void numNumX_ValueChanged(object sender, EventArgs e)
        {
            numX = numNumX.Value * numBinX.Value;
        }

        private void numStartY_ValueChanged(object sender, EventArgs e)
        {
            startY = numStartY.Value * numBinY.Value;
        }

        private void numNumY_ValueChanged(object sender, EventArgs e)
        {
            numY = numNumY.Value * numBinY.Value;
        }

        private void numStartX_ValueChanged(object sender, EventArgs e)
        {
            startX = numStartX.Value * numBinX.Value;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CameraID = Settings.Default.CameraID;
            lblCameraName.Text = CameraID;
            this.Location = Settings.Default.Location;
            this.Size = Settings.Default.WindowSize;
            splitContainer1.SplitterDistance = Settings.Default.Dividerleft;
            LastExposure = numExposure.Value;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (oCamera != null)
            {
                oCamera.Connected = false;
            }
            Settings.Default.CameraID = CameraID;
            Settings.Default.Location=this.Location;
            Settings.Default.Dividerleft = splitContainer1.SplitterDistance;
            Settings.Default.WindowSize = this.Size;
            Settings.Default.Save();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            if (CheckConnected)
            {
                try
                {
                    oCamera.AbortExposure();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        MessageBox.Show("Inner " + ex.InnerException.Message);
                    else
                        MessageBox.Show("Error " + ex.Message);
                }
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            if (img == null) return;
            if (!CheckConnected) return;

            Graphics g = e.Graphics;
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            PointF[] pts = new PointF[3];
            int t = splitContainer1.Panel2.AutoScrollPosition.Y;
            int l = splitContainer1.Panel2.AutoScrollPosition.X;
            int bx = oCamera.BinX;
            int by = oCamera.BinY;
            pts[0] = new PointF(l, t);    // upper left
            pts[1] = new PointF(img.Width * zoom * bx + l, t);  // upper right
            pts[2] = new PointF(l, img.Height * zoom * by + t);  // lower left
            g.DrawImage(img, pts);
        }
        private float zoom;

        private void trkZoom_Scroll(object sender, EventArgs e)
        {
            zoom = (float)Math.Pow(10, (trkZoom.Value/100.0));

            ToolTip.SetToolTip(this.trkZoom, "Zoom: " + zoom.ToString("F2"));

            try
            {
                splitContainer1.Panel2.AutoScrollMinSize = new Size((int)(img.Width * zoom * oCamera.BinX), (int)(img.Height * zoom * oCamera.BinY));
                splitContainer1.Panel2.Invalidate();
            }
            catch { }
        }

        private void splitContainer1_Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (img == null) return;
            numStartX.Value = (e.X - splitContainer1.Panel2.AutoScrollPosition.X) / (numBinX.Value * (decimal)zoom);
            numStartY.Value = (e.Y - splitContainer1.Panel2.AutoScrollPosition.Y) / (numBinY.Value * (decimal)zoom);
        }

        private void splitContainer1_Panel2_MouseUp(object sender, MouseEventArgs e)
        {
            if (img == null) return;
            decimal w = (e.X - splitContainer1.Panel2.AutoScrollPosition.X) / (numBinX.Value * (decimal)zoom) - numStartX.Value;
            //decimal w = ((img.Width - e.X) + splitContainer1.Panel2.AutoScrollPosition.X) / numBinX.Value - numStartX.Value;
            if (w < 0)
            {
                numStartX.Value = (img.Width-e.X) / numBinX.Value;
                numNumX.Value = -w;
            }
            else
                numNumX.Value = w;

            //decimal h = ((img.Height - e.Y) + splitContainer1.Panel2.AutoScrollPosition.Y) / numBinY.Value - numStartY.Value;
            decimal h = (e.Y - splitContainer1.Panel2.AutoScrollPosition.Y) / (numBinY.Value * (decimal)zoom) - numStartY.Value;
            if (h < 0)
            {
                numStartY.Value = (img.Height - e.Y) / numBinY.Value;
                numNumY.Value = -h;
            }
            else
                numNumY.Value = h;
        }

        private void imageControl_Change(ImageControl source, EventArgs e)
        {
            ShowImage();
        }

        /// <summary>
        /// convert the exposure value to the correct value and set the increment
        /// according to its value.
        /// </summary>
        private void ProcessExposure()
        {
            decimal val = numExposure.Value;
            decimal resolution = (oCamera.InterfaceVersion >= 2) ? (decimal)oCamera.ExposureResolution : 0.001M;

            if (val >= LastExposure)
            {
                if (val > 1.0M)
                {
                    numExposure.Increment = Math.Max(resolution, 1.0M);
                    //numExposure.DecimalPlaces = 0;
                    numExposure.Value = Math.Round(numExposure.Value + 0.4M, 0);
                }
                else if (val > 0.1M)
                { 
                    numExposure.Increment = Math.Max(resolution, 0.1M);
                    //numExposure.DecimalPlaces = 1;
                    numExposure.Value = Math.Round(numExposure.Value + 0.04M, 1);
                }
                else if (val > 0.01M)
                {
                    numExposure.Increment = Math.Max(resolution, 0.01M);
                    //numExposure.DecimalPlaces = 2;
                    numExposure.Value = Math.Round(numExposure.Value + 0.004M, 2);
                }
                else
                {
                    numExposure.Increment = Math.Max(resolution, 0.001M);
                    //numExposure.DecimalPlaces = 3;
                }
            }
            else
            {
                if (val <= 0.01M)
                {
                    numExposure.Increment = Math.Max(resolution, 0.001M);
                    //numExposure.DecimalPlaces = 3;
                }
                else if (val <= 0.1M)
                {
                    numExposure.Increment = Math.Max(resolution, 0.01M);
                    numExposure.Value = Math.Round(numExposure.Value * 100) / 100.0M;
                    //numExposure.DecimalPlaces = 2;
                }
                else if (val <= 1.0M)
                {
                    numExposure.Increment = Math.Max(resolution, 0.1M);
                    numExposure.Value = Math.Round(numExposure.Value * 10) / 10.0M;
                    //numExposure.DecimalPlaces = 1;
                }
                else
                {
                    numExposure.Increment = Math.Max(resolution, 1.0M);
                    numExposure.Value = Math.Round(numExposure.Value);
                    //numExposure.DecimalPlaces = 0;
                }
            }
            LastExposure = numExposure.Value;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (iarr != null)
            {
                DialogResult ret = saveFileDialog.ShowDialog();
                if (ret == DialogResult.OK)
                {
                    SaveToFits(saveFileDialog.FileName);
                }
            }
        }

        private void numericUpDownSetCCDTemperature_ValueChanged(object sender, EventArgs e)
        {
            if (CheckConnected)
            {
                oCamera.SetCCDTemperature = (Double)numericUpDownSetCCDTemperature.Value;
            }
        }

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            oCamera.SetupDialog();
        }

        private void SaveToFits(string filepath)
        {
            // get the data in the right form
            Array imageData = (Array)ArrayFuncs.Flatten(oCamera.ImageArray);
            double bZero = 0;
            double bScale = 1.0;
            if (oCamera.MaxADU <= 65535)
            {
                //bZero = 32768;
                //imageData = (ushort[])ArrayFuncs.ConvertArray(imageData, typeof(ushort));
            }
            int[] dims = ArrayFuncs.GetDimensions(oCamera.ImageArray);
            //Array image = ArrayFuncs.Curl(imageData, dims);

            // put the image data in a basic HDU of the fits 
            BasicHDU imageHdu = FitsFactory.HDUFactory(ArrayFuncs.Curl(imageData, dims));

            // put the other data in the HDU
            imageHdu.AddValue("BZERO", bZero, "");
            imageHdu.AddValue("BSCALE", bScale, "");
            imageHdu.AddValue("DATAMIN", 0.0, "");
            imageHdu.AddValue("DATAMAX", oCamera.MaxADU, "");           // pixel values above this level are considered saturated.
            imageHdu.AddValue("INSTRUME", oCamera.Description, "");
            imageHdu.AddValue("EXPTIME", oCamera.LastExposureDuration, "");     // duration of exposure in seconds.
            imageHdu.AddValue("DATE-OBS", oCamera.LastExposureStartTime, "");
            imageHdu.AddValue("XPIXSZ", oCamera.PixelSizeX * oCamera.BinX, ""); // physical X dimension of the sensor's pixels in microns (present only if the information is provided by the camera driver). Includes binning.
            imageHdu.AddValue("YPIXSZ", oCamera.PixelSizeY * oCamera.BinY, ""); // physical Y dimension of the sensor's pixels in microns (present only if the information is provided by the camera driver). Includes binning.
            imageHdu.AddValue("XBINNING", oCamera.BinX, "");
            imageHdu.AddValue("YBINNING", oCamera.BinY, "");
            imageHdu.AddValue("XORGSUBF", oCamera.StartX, "");        // subframe origin on X axis in binned pixels
            imageHdu.AddValue("YORGSUBF", oCamera.StartY, "");        // subframe origin on Y axis in binned pixels
            //imageHdu.AddValue("XPOSSUBF", oCamera.StartX, "");
            //imageHdu.AddValue("YPOSSUBF", oCamera.StartY, "");
            imageHdu.AddValue("CBLACK", (double)imageControl.Minimum, "");
            imageHdu.AddValue("CWHITE", (double)imageControl.Maximum, "");
            imageHdu.AddValue("SWCREATE", "ASCOM Camera Test", "");     // string indicating the software used to create the file
            // extensions as specified by SBIG
            try
            {
                imageHdu.AddValue("CCD_TEMP", oCamera.CCDTemperature, "");  // actual measured sensor temperature at the start of exposure in degrees C. Absent if temperature is not available.
            }
            finally{}
            if (oCamera.CanSetCCDTemperature)
                imageHdu.AddValue("SET-TEMP", oCamera.SetCCDTemperature, "CCD temperature setpoint in degrees C");
            // OBJECT – name or catalog number of object being imaged

            //imageHdu.AddValue("TELESCOP", "", "");        // user-entered information about the telescope used.
            //imageHdu.AddValue("OBSERVER", "", "");        // user-entered information; the observer’s name.

      //DARKTIME – dark current integration time, if recorded. May be longer than exposure time.

      //IMAGETYP – type of image: Light Frame, Bias Frame, Dark Frame, Flat Frame, or Tricolor Image.
            //ISOSPEED – ISO camera setting, if camera uses ISO speeds.
            //JD_GEO – records the geocentric Julian Day of the start of exposure.
            //JD_HELIO – records the Heliocentric Julian Date at the exposure midpoint.
            //NOTES – user-entered information; free-form notes.
            //READOUTM – records the selected Readout Mode (if any) for the camera.

            imageHdu.AddValue("SBSTDVER", "SBFITSEXT Version 1.0", "version of the SBIG FITS extensions supported");

            // save it
            Fits fitsImage = new Fits();
            fitsImage.AddHDU(imageHdu);
            BufferedDataStream bds = new BufferedDataStream(new FileStream(filepath, FileMode.Create));
            fitsImage.Write(bds);
            bds.Close();
            bds.Dispose();
        }

        private enum SensorType
        {
            Monochrome,
            Color,
            RGGB,
            CMYG,
            CMYG2,
            LRGB
        }



    }
}