using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using System.Globalization;

namespace ASCOM.Simulator
{
	[ComVisible(false)]					// Form not registered for COM!
	public partial class SetupDialogForm : Form
	{
        private const string STR_N0 = "N0";
        private const string STR_N2 = "N2";
        private Camera camera;

		public SetupDialogForm()
		{
			InitializeComponent();
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
            SaveToProfile();
			Dispose();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
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
            this.textBoxBayerOffsetX.Text = camera.bayerOffsetX.ToString();
            this.textBoxBayerOffsetY.Text = camera.bayerOffsetY.ToString();
            this.textBoxMaxADU.Text = camera.maxADU.ToString();
            this.textBoxMaxBinX.Text = camera.maxBinX.ToString();
            this.textBoxMaxBinY.Text = camera.maxBinY.ToString();
            this.textBoxSensorName.Text = camera.sensorName;
            this.comboBoxSensorType.SelectedIndex = camera.sensorType;
            this.checkBoxCanAbortExposure.Checked = camera.canAbortExposure;
            this.checkBoxCanAsymmetricBin.Checked = camera.canAsymmetricBin;
            this.checkBoxCanGetCoolerPower.Checked = camera.canGetCoolerPower;
            this.checkBoxCanSetCCDTemperature.Checked = camera.canSetCcdTemperature;
            this.checkBoxCanStopExposure.Checked = camera.canStopExposure;
            this.checkBoxHasCooler.Checked = camera.hasCooler;
            this.checkBoxHasShutter.Checked = camera.hasShutter;
            this.textBoxCameraXSize.Text = camera.cameraXSize.ToString(STR_N0);
            this.textBoxCameraYSize.Text = camera.cameraYSize.ToString(STR_N0);
            this.textBoxPixelSizeX.Text = camera.pixelSizeX.ToString(STR_N2);
            this.textBoxPixelSizeY.Text = camera.pixelSizeY.ToString(STR_N2);
            this.textBoxElectronsPerADU.Text = camera.electronsPerADU.ToString(STR_N2);

            this.camera = camera;
        }

        private void SaveToProfile()
        {
            Profile profile = new Profile();
            profile.DeviceType = "Camera";

            profile.WriteValue(Camera.s_csDriverID, "BayerOffsetX", this.textBoxBayerOffsetX.Text.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "BayerOffsetY", this.textBoxBayerOffsetY.Text.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "MaxADU", this.textBoxMaxADU.Text.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "MaxBinX", this.textBoxMaxBinX.Text.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "MaxBinY", this.textBoxMaxBinY.Text.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "SensorName", this.textBoxSensorName.Text.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "SensorType", this.comboBoxSensorType.SelectedIndex.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "CanAbortExposure", this.checkBoxCanAbortExposure.Checked.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "CanAsymmetricBin", this.checkBoxCanAsymmetricBin.Checked.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "CanGetCoolerPower", this.checkBoxCanGetCoolerPower.Checked.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "CanSetCCDTemperature", this.checkBoxCanSetCCDTemperature.Checked.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "CanStopExposure", this.checkBoxCanStopExposure.Checked.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "HasCooler", this.checkBoxHasCooler.Checked.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "HasShutter", this.checkBoxHasShutter.Checked.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "CameraXSize", this.textBoxCameraXSize.Text.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "CameraYSize", this.textBoxCameraYSize.Text.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "PixelSizeX", this.textBoxPixelSizeX.Text.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "PixelSizeY", this.textBoxPixelSizeY.Text.ToString(CultureInfo.InvariantCulture));
            profile.WriteValue(Camera.s_csDriverID, "ElectronsPerADU", this.textBoxElectronsPerADU.Text.ToString(CultureInfo.InvariantCulture));
        }

	}
}