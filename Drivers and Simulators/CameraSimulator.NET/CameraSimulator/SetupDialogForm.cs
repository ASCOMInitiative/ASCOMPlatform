using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ASCOM.DeviceInterface;
using System.Collections;

namespace ASCOM.Simulator
{
    [ComVisible(false)]                 // Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private const string STR_N2 = "N2";
        private Camera camera;

        private CoolerSetupForm coolerSetupForm; // Variable to hold an instance of the cooler configuration form
        internal bool okButtonPressed = false;

        public SetupDialogForm()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            okButtonPressed = true;
            SaveProperties();
            this.Dispose();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            okButtonPressed = false;
            this.Dispose();
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
            checkBoxLogging.Checked = Log.Enabled;
            checkBoxInterfaceVersion.Checked = (theCamera.interfaceVersion == 2);
            textBoxPixelSizeX.Text = theCamera.pixelSizeX.ToString(STR_N2, CultureInfo.CurrentCulture);
            textBoxPixelSizeY.Text = theCamera.pixelSizeY.ToString(STR_N2, CultureInfo.CurrentCulture);
            //textBoxFullWellCapacity.Text = camera.fullWellCapacity.ToString();
            textBoxMaxADU.Text = theCamera.maxADU.ToString(CultureInfo.CurrentCulture);
            textBoxElectronsPerADU.Text = theCamera.electronsPerADU.ToString(STR_N2, CultureInfo.CurrentCulture);

            textBoxCameraXSize.Text = theCamera.cameraXSize.ToString(CultureInfo.CurrentCulture);
            textBoxCameraYSize.Text = theCamera.cameraYSize.ToString(CultureInfo.CurrentCulture);
            checkBoxCanAsymmetricBin.Checked = theCamera.canAsymmetricBin;
            textBoxMaxBinX.Text = theCamera.maxBinX.ToString(CultureInfo.CurrentCulture);
            textBoxMaxBinY.Text = theCamera.maxBinY.ToString(CultureInfo.CurrentCulture);
            checkBoxHasShutter.Checked = theCamera.hasShutter;
            textBoxSensorName.Text = theCamera.sensorName;
            comboBoxSensorType.SelectedIndex = (int)theCamera.sensorType;
            textBoxBayerOffsetX.Text = theCamera.bayerOffsetX.ToString(CultureInfo.CurrentCulture);
            textBoxBayerOffsetY.Text = theCamera.bayerOffsetY.ToString(CultureInfo.CurrentCulture);
            checkBoxOmitOddBins.Checked = theCamera.omitOddBins;

            checkBoxHasCooler.Checked = theCamera.hasCooler;
            checkBoxCanSetCCDTemperature.Checked = theCamera.canSetCcdTemperature;
            checkBoxCanGetCoolerPower.Checked = theCamera.canGetCoolerPower;

            checkBoxCanAbortExposure.Checked = theCamera.canAbortExposure;
            checkBoxCanStopExposure.Checked = theCamera.canStopExposure;
            textBoxMaxExposure.Text = theCamera.exposureMax.ToString(CultureInfo.CurrentCulture);
            textBoxMinExposure.Text = theCamera.exposureMin.ToString(CultureInfo.CurrentCulture);

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
            textBoxGainMin.Text = theCamera.gainMin.ToString(CultureInfo.CurrentCulture);
            textBoxGainMax.Text = theCamera.gainMax.ToString(CultureInfo.CurrentCulture);
            checkBoxApplyNoise.Checked = theCamera.applyNoise;

            checkBoxCanPulseGuide.Checked = theCamera.canPulseGuide;

            checkBoxCanFastReadout.Checked = theCamera.canFastReadout;
            if (theCamera.canFastReadout)
            {
                checkBoxUseReadoutModes.Enabled = false;
            }
            else
            {
                checkBoxUseReadoutModes.Checked = theCamera.readoutModes.Count > 1;
            }

            camera = theCamera;

        }

        private void SaveProperties()
        {
            Log.Enabled = checkBoxLogging.Checked;
            camera.pixelSizeX = double.Parse(textBoxPixelSizeX.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.pixelSizeY = double.Parse(textBoxPixelSizeY.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            //camera.fullWellCapacity = Convert.ToDouble(textBoxFullWellCapacity.Text, CultureInfo.InvariantCulture);
            camera.maxADU = int.Parse(textBoxMaxADU.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.electronsPerADU = double.Parse(textBoxElectronsPerADU.Text, NumberStyles.Number, CultureInfo.CurrentCulture);

            camera.cameraXSize = int.Parse(textBoxCameraXSize.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.cameraYSize = int.Parse(textBoxCameraYSize.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.canAsymmetricBin = checkBoxCanAsymmetricBin.Checked;
            camera.maxBinX = short.Parse(textBoxMaxBinX.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.maxBinY = short.Parse(textBoxMaxBinY.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.hasShutter = checkBoxHasShutter.Checked;
            camera.sensorName = textBoxSensorName.Text;
            camera.sensorType = (SensorType)comboBoxSensorType.SelectedIndex;
            camera.bayerOffsetX = short.Parse(textBoxBayerOffsetX.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.bayerOffsetY = short.Parse(textBoxBayerOffsetY.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.omitOddBins = checkBoxOmitOddBins.Checked;

            camera.hasCooler = checkBoxHasCooler.Checked;
            camera.canSetCcdTemperature = checkBoxCanSetCCDTemperature.Checked;
            camera.canGetCoolerPower = checkBoxCanGetCoolerPower.Checked;

            camera.canAbortExposure = checkBoxCanAbortExposure.Checked;
            camera.canStopExposure = checkBoxCanStopExposure.Checked;
            camera.exposureMin = double.Parse(textBoxMinExposure.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.exposureMax = double.Parse(textBoxMaxExposure.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.applyNoise = checkBoxApplyNoise.Checked;

            camera.canPulseGuide = checkBoxCanPulseGuide.Checked;

            if (radioButtonNoGain.Checked)
            {
                camera.gainMin = camera.gainMax = 0;
                camera.gains = null;
            }
            else if (radioButtonUseGains.Checked)
            {
                camera.gains = new ArrayList { "ISO 100", "ISO 200", "ISO 400", "ISO 800", "ISO 1600" };
                camera.gainMin = (short)0;
                camera.gainMax = (short)(camera.gains.Count - 1);
            }
            if (radioButtonUseMinAndMax.Checked)
            {
                camera.gains = null;
                camera.gainMin = short.Parse(textBoxGainMin.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
                camera.gainMax = short.Parse(textBoxGainMax.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            }
            camera.interfaceVersion = (short)(checkBoxInterfaceVersion.Checked ? 2 : 1);

            camera.canFastReadout = checkBoxCanFastReadout.Checked;
            if (checkBoxUseReadoutModes.Checked)
            {
                camera.readoutModes = new ArrayList { "Raw Monochrome", "Live View", "Raw To Hard Drive" };
            }
            else
            {
                camera.readoutModes = new ArrayList { "Default" };
            }
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
            textBoxBayerOffsetX.Enabled = checkBoxInterfaceVersion.Checked;
            textBoxBayerOffsetY.Enabled = checkBoxInterfaceVersion.Checked;
            textBoxGainMax.Enabled = checkBoxInterfaceVersion.Checked;
            textBoxGainMin.Enabled = checkBoxInterfaceVersion.Checked;
            textBoxMaxExposure.Enabled = checkBoxInterfaceVersion.Checked;
            textBoxMinExposure.Enabled = checkBoxInterfaceVersion.Checked;
            textBoxSensorName.Enabled = checkBoxInterfaceVersion.Checked;
            comboBoxSensorType.Enabled = checkBoxInterfaceVersion.Checked;
            radioButtonNoGain.Enabled = checkBoxInterfaceVersion.Checked;
            radioButtonUseGains.Enabled = checkBoxInterfaceVersion.Checked;
            radioButtonUseMinAndMax.Enabled = checkBoxInterfaceVersion.Checked;
        }

        private void checkBoxCanFastReadout_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxUseReadoutModes.Enabled = !(sender as CheckBox).Checked;
        }

        private void comboBoxSensorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var si = (sender as ComboBox).SelectedItem as string;
            labelBayerOffsetX.Enabled =
                labelBayerOffsetY.Enabled =
                textBoxBayerOffsetX.Enabled =
                textBoxBayerOffsetY.Enabled = (si != "Monochrome" && si != "Color");
        }

        private void checkBoxLogging_CheckedChanged(object sender, EventArgs e)
        {
            Log.Enabled = checkBoxLogging.Checked;
        }

        /// <summary>
        /// Cooler configuration button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCoolerConfiguration_Click(object sender, EventArgs e)
        {
            // Create and initialise the cooling configuration form
            coolerSetupForm = new CoolerSetupForm(); // Create the cooler configuration form
            coolerSetupForm.InitProperties(camera); // Initialise the form

            coolerSetupForm.ShowDialog(); // Display the form - Any changes will be saved by the form when its OK button is pressed.
            coolerSetupForm.Dispose();
            coolerSetupForm = null;
        }

}