using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;

namespace ASCOM.Simulator
{
	[ComVisible(false)]					// Form not registered for COM!
	public partial class SetupDialogForm : Form
	{
        private const string STR_N0 = "N0";
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

        internal void InitProperties(Camera camera)
        {
            this.checkBoxInterfaceVersion.Checked = (camera.interfaceVersion == 2);
            this.textBoxPixelSizeX.Text = camera.pixelSizeX.ToString(STR_N2);
            this.textBoxPixelSizeY.Text = camera.pixelSizeY.ToString(STR_N2);
            //this.textBoxFullWellCapacity.Text = camera.fullWellCapacity.ToString();
            this.textBoxMaxADU.Text = camera.maxADU.ToString();
            this.textBoxElectronsPerADU.Text = camera.electronsPerADU.ToString(STR_N2);

            this.textBoxCameraXSize.Text = camera.cameraXSize.ToString(STR_N0);
            this.textBoxCameraYSize.Text = camera.cameraYSize.ToString(STR_N0);
            this.checkBoxCanAsymmetricBin.Checked = camera.canAsymmetricBin;
            this.textBoxMaxBinX.Text = camera.maxBinX.ToString();
            this.textBoxMaxBinY.Text = camera.maxBinY.ToString();
            this.checkBoxHasShutter.Checked = camera.hasShutter;
            this.textBoxSensorName.Text = camera.sensorName;
            this.comboBoxSensorType.SelectedIndex = (int)camera.sensorType;
            this.textBoxBayerOffsetX.Text = camera.bayerOffsetX.ToString();
            this.textBoxBayerOffsetY.Text = camera.bayerOffsetY.ToString();

            this.checkBoxHasCooler.Checked = camera.hasCooler;
            this.checkBoxCanSetCCDTemperature.Checked = camera.canSetCcdTemperature;
            this.checkBoxCanGetCoolerPower.Checked = camera.canGetCoolerPower;

            this.checkBoxCanAbortExposure.Checked = camera.canAbortExposure;
            this.checkBoxCanStopExposure.Checked = camera.canStopExposure;
            this.textBoxMaxExposure.Text = camera.exposureMax.ToString();
            this.textBoxMinExposure.Text = camera.exposureMin.ToString();
            //this.textboxExposureResolution.Text = camera.exposureResolution.ToString();

            if (camera.gains != null && camera.gains.Length > 0)
            {
                radioButtonUseGains.Checked = true;
            }
            else if (camera.gainMax > camera.gainMin)
            {
                radioButtonUseMinAndMax.Checked = true;
            }
            else
            {
                radioButtonNoGain.Checked = true;
            }
            this.textBoxGainMin.Text = camera.gainMin.ToString();
            this.textBoxGainMax.Text = camera.gainMax.ToString();
            this.checkBoxApplyNoise.Checked = camera.applyNoise;

            this.camera = camera;
        }

        private void SaveProperties()
        {
            camera.pixelSizeX = Convert.ToDouble(this.textBoxPixelSizeX.Text, CultureInfo.InvariantCulture);
            camera.pixelSizeY = Convert.ToDouble(this.textBoxPixelSizeY.Text, CultureInfo.InvariantCulture);
            //camera.fullWellCapacity = Convert.ToDouble(this.textBoxFullWellCapacity.Text, CultureInfo.InvariantCulture);
            camera.maxADU = Convert.ToInt32(this.textBoxMaxADU.Text, CultureInfo.InvariantCulture);
            camera.electronsPerADU = Convert.ToDouble(this.textBoxElectronsPerADU.Text, CultureInfo.InvariantCulture);

            camera.cameraXSize = Convert.ToInt32(this.textBoxCameraXSize.Text, CultureInfo.InvariantCulture);
            camera.cameraYSize = Convert.ToInt32(this.textBoxCameraYSize.Text, CultureInfo.InvariantCulture);
            camera.canAsymmetricBin = this.checkBoxCanAsymmetricBin.Checked;
            camera.maxBinX = Convert.ToInt16(this.textBoxMaxBinX.Text, CultureInfo.InvariantCulture);
            camera.maxBinY = Convert.ToInt16(this.textBoxMaxBinY.Text, CultureInfo.InvariantCulture);
            camera.hasShutter = this.checkBoxHasShutter.Checked;
            camera.sensorName = this.textBoxSensorName.Text;
            camera.sensorType = (SensorType)this.comboBoxSensorType.SelectedIndex;
            camera.bayerOffsetX = Convert.ToInt16(this.textBoxBayerOffsetX.Text, CultureInfo.InvariantCulture);
            camera.bayerOffsetY = Convert.ToInt16(this.textBoxBayerOffsetY.Text, CultureInfo.InvariantCulture);

            camera.hasCooler = this.checkBoxHasCooler.Checked;
            camera.canSetCcdTemperature = this.checkBoxCanSetCCDTemperature.Checked;
            camera.canGetCoolerPower = this.checkBoxCanGetCoolerPower.Checked;

            camera.canAbortExposure = this.checkBoxCanAbortExposure.Checked;
            camera.canStopExposure = this.checkBoxCanStopExposure.Checked;
            camera.exposureMin = Convert.ToDouble(this.textBoxMinExposure.Text, CultureInfo.InvariantCulture);
            camera.exposureMax = Convert.ToDouble(this.textBoxMaxExposure.Text, CultureInfo.InvariantCulture);
            //camera.exposureResolution = Convert.ToDouble(this.textBoxExposureResolution.Text, CultureInfo.InvariantCulture);
            camera.applyNoise = this.checkBoxApplyNoise.Checked;

            if (this.radioButtonNoGain.Checked)
            {
                camera.gainMin = camera.gainMax = 0;
                camera.gains = null;
            }
            else if (this.radioButtonUseGains.Checked)
            {
                camera.gains= new string[]{ "ISO 100", "ISO 200", "ISO 400", "ISO 800", "ISO 1600"};
                camera.gainMin = (short)camera.gains.GetLowerBound(0);
                camera.gainMax = (short)(camera.gains.GetUpperBound(0));
            }
            if (this.radioButtonUseMinAndMax.Checked)
            {
                camera.gains = null;
                camera.gainMin = Convert.ToInt16(textBoxGainMin.Text, CultureInfo.InvariantCulture);
                camera.gainMax = Convert.ToInt16(textBoxGainMax.Text, CultureInfo.InvariantCulture);
            }
            camera.interfaceVersion = (short)(checkBoxInterfaceVersion.Checked ? 2 : 1);
        }

        private void buttonSetImageFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = camera.imagePath;
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