using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ASCOM.DeviceInterface;
using System.Collections;

namespace ASCOM.Simulator
{
	[ComVisible(false)]					// Form not registered for COM!
	public partial class SetupDialogForm : Form
	{
        //private const string STR_N0 = "N0";
        private const string STR_N2 = "N2";
        private Camera camera;

        internal bool okButtonPressed = false;

		public SetupDialogForm()
		{
			InitializeComponent();
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
            okButtonPressed = true;
            SaveProperties();
			Dispose();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
            okButtonPressed = false;
			Dispose();
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void BrowseToAscom(object sender, EventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start("http://ascom-standards.org/");
			}
			catch (System.ComponentModel.Win32Exception noBrowser)
			{
				if (noBrowser.ErrorCode == -2147467259)
					MessageBox.Show(noBrowser.Message);
			}
			catch (System.Exception other)
			{
				MessageBox.Show(other.Message);
			}
		}

        internal void InitProperties(Camera theCamera)
        {
            this.checkBoxInterfaceVersion.Checked = (theCamera.interfaceVersion == 2);
            this.textBoxPixelSizeX.Text = theCamera.pixelSizeX.ToString(STR_N2, CultureInfo.CurrentCulture);
            this.textBoxPixelSizeY.Text = theCamera.pixelSizeY.ToString(STR_N2, CultureInfo.CurrentCulture);
            //this.textBoxFullWellCapacity.Text = camera.fullWellCapacity.ToString();
            this.textBoxMaxADU.Text = theCamera.maxADU.ToString(CultureInfo.CurrentCulture);
            this.textBoxElectronsPerADU.Text = theCamera.electronsPerADU.ToString(STR_N2, CultureInfo.CurrentCulture);

            this.textBoxCameraXSize.Text = theCamera.cameraXSize.ToString(CultureInfo.CurrentCulture);
            this.textBoxCameraYSize.Text = theCamera.cameraYSize.ToString(CultureInfo.CurrentCulture);
            this.checkBoxCanAsymmetricBin.Checked = theCamera.canAsymmetricBin;
            this.textBoxMaxBinX.Text = theCamera.maxBinX.ToString(CultureInfo.CurrentCulture);
            this.textBoxMaxBinY.Text = theCamera.maxBinY.ToString(CultureInfo.CurrentCulture);
            this.checkBoxHasShutter.Checked = theCamera.hasShutter;
            this.textBoxSensorName.Text = theCamera.sensorName;
            this.comboBoxSensorType.SelectedIndex = (int)theCamera.sensorType;
            this.textBoxBayerOffsetX.Text = theCamera.bayerOffsetX.ToString(CultureInfo.CurrentCulture);
            this.textBoxBayerOffsetY.Text = theCamera.bayerOffsetY.ToString(CultureInfo.CurrentCulture);
            this.checkBoxOmitOddBins.Checked = theCamera.omitOddBins;

            this.checkBoxHasCooler.Checked = theCamera.hasCooler;
            this.checkBoxCanSetCCDTemperature.Checked = theCamera.canSetCcdTemperature;
            this.checkBoxCanGetCoolerPower.Checked = theCamera.canGetCoolerPower;

            this.checkBoxCanAbortExposure.Checked = theCamera.canAbortExposure;
            this.checkBoxCanStopExposure.Checked = theCamera.canStopExposure;
            this.textBoxMaxExposure.Text = theCamera.exposureMax.ToString(CultureInfo.CurrentCulture);
            this.textBoxMinExposure.Text = theCamera.exposureMin.ToString(CultureInfo.CurrentCulture);

            if (theCamera.gains != null && theCamera.gains.Count > 0)
            {
                radioButtonUseGains.Checked = true;
            }
            else if (theCamera.gainMax > theCamera.gainMin)
            {
                radioButtonUseMinAndMax.Checked = true;
            }
            else
            {
                radioButtonNoGain.Checked = true;
            }
            this.textBoxGainMin.Text = theCamera.gainMin.ToString(CultureInfo.CurrentCulture);
            this.textBoxGainMax.Text = theCamera.gainMax.ToString(CultureInfo.CurrentCulture);
            this.checkBoxApplyNoise.Checked = theCamera.applyNoise;

            this.checkBoxCanPulseGuide.Checked = theCamera.canPulseGuide;

            this.camera = theCamera;
        }

        private void SaveProperties()
        {
            camera.pixelSizeX = double.Parse(this.textBoxPixelSizeX.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.pixelSizeY = double.Parse(this.textBoxPixelSizeY.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            //camera.fullWellCapacity = Convert.ToDouble(this.textBoxFullWellCapacity.Text, CultureInfo.InvariantCulture);
            camera.maxADU = int.Parse(this.textBoxMaxADU.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.electronsPerADU = double.Parse(this.textBoxElectronsPerADU.Text, NumberStyles.Number, CultureInfo.CurrentCulture);

            camera.cameraXSize = int.Parse(this.textBoxCameraXSize.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.cameraYSize = int.Parse(this.textBoxCameraYSize.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.canAsymmetricBin = this.checkBoxCanAsymmetricBin.Checked;
            camera.maxBinX = short.Parse(this.textBoxMaxBinX.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.maxBinY = short.Parse(this.textBoxMaxBinY.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.hasShutter = this.checkBoxHasShutter.Checked;
            camera.sensorName = this.textBoxSensorName.Text;
            camera.sensorType = (SensorType)this.comboBoxSensorType.SelectedIndex;
            camera.bayerOffsetX = short.Parse(this.textBoxBayerOffsetX.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.bayerOffsetY = short.Parse(this.textBoxBayerOffsetY.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.omitOddBins = this.checkBoxOmitOddBins.Checked;

            camera.hasCooler = this.checkBoxHasCooler.Checked;
            camera.canSetCcdTemperature = this.checkBoxCanSetCCDTemperature.Checked;
            camera.canGetCoolerPower = this.checkBoxCanGetCoolerPower.Checked;

            camera.canAbortExposure = this.checkBoxCanAbortExposure.Checked;
            camera.canStopExposure = this.checkBoxCanStopExposure.Checked;
            camera.exposureMin = double.Parse(this.textBoxMinExposure.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.exposureMax = double.Parse(this.textBoxMaxExposure.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.applyNoise = this.checkBoxApplyNoise.Checked;

            camera.canPulseGuide = this.checkBoxCanPulseGuide.Checked;

            if (this.radioButtonNoGain.Checked)
            {
                camera.gainMin = camera.gainMax = 0;
                camera.gains = null;
            }
            else if (this.radioButtonUseGains.Checked)
            {
                camera.gains= new ArrayList{ "ISO 100", "ISO 200", "ISO 400", "ISO 800", "ISO 1600"};
                camera.gainMin = (short)0;
                camera.gainMax = (short)(camera.gains.Count - 1);
            }
            if (this.radioButtonUseMinAndMax.Checked)
            {
                camera.gains = null;
                camera.gainMin = short.Parse(textBoxGainMin.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
                camera.gainMax = short.Parse(textBoxGainMax.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            }
            camera.interfaceVersion = (short)(checkBoxInterfaceVersion.Checked ? 2 : 1);
        }

        private void buttonSetImageFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(camera.imagePath);
            openFileDialog1.FileName = Path.GetFileName(camera.imagePath);
            openFileDialog1.ShowDialog();
            camera.imagePath = openFileDialog1.FileName;
        }

        private void checkBoxInterfaceVersion_CheckedChanged(object sender, EventArgs e)
        {
            // enable the V2 properties if checked
            this.textBoxBayerOffsetX.Enabled = checkBoxInterfaceVersion.Checked;
            this.textBoxBayerOffsetY.Enabled = checkBoxInterfaceVersion.Checked;
            this.textBoxGainMax.Enabled = checkBoxInterfaceVersion.Checked;
            this.textBoxGainMin.Enabled = checkBoxInterfaceVersion.Checked;
            this.textBoxMaxExposure.Enabled = checkBoxInterfaceVersion.Checked;
            this.textBoxMinExposure.Enabled = checkBoxInterfaceVersion.Checked;
            this.textBoxSensorName.Enabled = checkBoxInterfaceVersion.Checked;
            this.comboBoxSensorType.Enabled = checkBoxInterfaceVersion.Checked;
            this.radioButtonNoGain.Enabled = checkBoxInterfaceVersion.Checked;
            this.radioButtonUseGains.Enabled = checkBoxInterfaceVersion.Checked;
            this.radioButtonUseMinAndMax.Enabled = checkBoxInterfaceVersion.Checked;
        }
	}
}