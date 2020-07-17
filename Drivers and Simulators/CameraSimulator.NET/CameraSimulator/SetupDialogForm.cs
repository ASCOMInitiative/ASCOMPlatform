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

        private bool validationErrorsOccurred = false; // Initialise to the success condition (Used to stop the form closing if there are validation errors in text box values)

        public SetupDialogForm()
        {
            InitializeComponent();

            this.FormClosing += SetupDialogForm_FormClosing;

            // Set a common event handler for the gain radio buttons
            radioButtonNoGain.CheckedChanged += GainRadioButton_CheckedChanged;
            radioButtonUseGains.CheckedChanged += GainRadioButton_CheckedChanged;
            radioButtonUseMinAndMax.CheckedChanged += GainRadioButton_CheckedChanged;

            // Set a common event handler for the offset radio buttons
            RadNoOffset.CheckedChanged += OffsetRadioButton_CheckedChanged;
            RadOffsets.CheckedChanged += OffsetRadioButton_CheckedChanged;
            RadOffsetMinMax.CheckedChanged += OffsetRadioButton_CheckedChanged;

        }

        private void SetupDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (validationErrorsOccurred) e.Cancel = true; // Cancel the close if validation errors occurred
        }

        private void OffsetRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            switch (rb.Name)
            {
                case "RadNoOffset":
                    TxtOffsetMin.Enabled = false;
                    TxtOffsetMax.Enabled = false;
                    break;
                case "RadOffsets":
                    TxtOffsetMin.Enabled = false;
                    TxtOffsetMax.Enabled = false;
                    break;
                case "RadOffsetMinMax":
                    TxtOffsetMin.Enabled = true;
                    TxtOffsetMax.Enabled = true;
                    break;
            }
        }

        private void GainRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            switch (rb.Name)
            {
                case "radioButtonNoGain":
                    textBoxGainMin.Enabled = false;
                    textBoxGainMax.Enabled = false;
                    break;
                case "radioButtonUseGains":
                    textBoxGainMin.Enabled = false;
                    textBoxGainMax.Enabled = false;
                    break;
                case "radioButtonUseMinAndMax":
                    textBoxGainMin.Enabled = true;
                    textBoxGainMax.Enabled = true;
                    break;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            okButtonPressed = true;
            validationErrorsOccurred = false; // Initialise to the success condition
            SaveProperties(); // If validation fails the valiudation flag will be set false and the form close will be cancelled
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            okButtonPressed = false;
            this.Dispose();
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

        internal void InitProperties(Camera theCamera)
        {

            checkBoxLogging.Checked = Log.Enabled;
            textBoxPixelSizeX.Text = theCamera.pixelSizeX.ToString(STR_N2, CultureInfo.CurrentCulture);
            textBoxPixelSizeY.Text = theCamera.pixelSizeY.ToString(STR_N2, CultureInfo.CurrentCulture);
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

            // Set Gain configuration
            switch (theCamera.gainMode)
            {
                case Camera.GainMode.None:
                    radioButtonNoGain.Checked = true;
                    break;
                case Camera.GainMode.GainMinMax:
                    radioButtonUseMinAndMax.Checked = true;
                    break;
                case Camera.GainMode.Gains:
                    radioButtonUseGains.Checked = true;
                    break;
                default:
                    radioButtonNoGain.Checked = true;
                    break;
            }

            textBoxGainMin.Text = theCamera.gainMin.ToString(CultureInfo.CurrentCulture);
            textBoxGainMax.Text = theCamera.gainMax.ToString(CultureInfo.CurrentCulture);

            // Set Offset configuration
            switch (theCamera.offsetMode)
            {
                case Camera.OffsetMode.None:
                    RadNoOffset.Checked = true;
                    break;
                case Camera.OffsetMode.OffsetMinMax:
                    RadOffsetMinMax.Checked = true;
                    break;
                case Camera.OffsetMode.Offsets:
                    RadOffsets.Checked = true;
                    break;
                default:
                    radioButtonNoGain.Checked = true;
                    break;
            }

            TxtOffsetMin.Text = theCamera.offsetMin.ToString(CultureInfo.CurrentCulture);
            TxtOffsetMax.Text = theCamera.offsetMax.ToString(CultureInfo.CurrentCulture);

            // Set the sub exposure configuration
            TxtSubExposure.Text = theCamera.subExposureInterval.ToString(CultureInfo.CurrentCulture);
            ChkHasSubExposure.Checked = theCamera.hasSubExposure;
            TxtSubExposure.Enabled = ChkHasSubExposure.Checked;

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

            // Set interface version last so that appropriate text box controls are enabled and disabled correctly per what is available in each interface version
            NumInterfaceVersion.Value = theCamera.interfaceVersion;

            camera = theCamera;
        }

        /// <summary>
        /// Save the new configuration
        /// </summary>
        /// <returns>True if there are no validation errors, otherwise false</returns>
        private void SaveProperties()
        {
            Log.Enabled = checkBoxLogging.Checked;
            camera.pixelSizeX = double.Parse(textBoxPixelSizeX.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
            camera.pixelSizeY = double.Parse(textBoxPixelSizeY.Text, NumberStyles.Number, CultureInfo.CurrentCulture);
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

            // Save Gain parameters
            if (short.TryParse(textBoxGainMin.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out short gainMinParsed)) // Validate GainMin
            {
                camera.gainMin = gainMinParsed;
            }
            else
            {
                validationErrorsOccurred = true;
                MessageBox.Show($"The minimum gain value is invalid: '{textBoxGainMin.Text}'");
            }

            if (short.TryParse(textBoxGainMax.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out short gainMaxParsed)) // Validate GainMax
            {
                camera.gainMax = gainMaxParsed;
            }
            else
            {
                validationErrorsOccurred = true;
                MessageBox.Show($"The maximum gain value is invalid: '{textBoxGainMax.Text}'");
            }

            if (camera.gainMin > camera.gainMax) // Validate that GainMin is not greater than GainMax 
            {
                validationErrorsOccurred = true;
                MessageBox.Show($"The maximum gain value {camera.gainMin} is greater than the minimum gain value: {camera.gainMax}");
            }

            if (radioButtonNoGain.Checked)
            {
                camera.gainMode = Camera.GainMode.None;
            }
            else if (radioButtonUseGains.Checked)
            {
                camera.gainMode = Camera.GainMode.Gains;
                if ((camera.gain < 0) | (camera.gain > camera.gains.Count - 1)) camera.gain = 0; // initialise to first item if the current Gain is out of the Gains array size

            }
            else if (radioButtonUseMinAndMax.Checked)
            {
                camera.gainMode = Camera.GainMode.GainMinMax;
                if ((camera.gain < camera.gainMin) | (camera.gain > camera.gainMax)) camera.gain = camera.gainMin; // initialise to GainMin if the current Gain is out side the min to max range
            }

            // Save Offset parameters
            if (int.TryParse(TxtOffsetMin.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out int offsetMinParsed)) // Validate OffsetMin
            {
                camera.offsetMin = offsetMinParsed;
            }
            else
            {
                validationErrorsOccurred = true;
                MessageBox.Show($"The minimum offset value is invalid: '{TxtOffsetMin.Text}'");
            }

            if (int.TryParse(TxtOffsetMax.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out int offsetMaxParsed)) // Validate OffsetMax
            {
                camera.offsetMax = offsetMaxParsed;
            }
            else
            {
                validationErrorsOccurred = true;
                MessageBox.Show($"The maximum offset value is invalid: '{TxtOffsetMax.Text}'");
            }

            if (camera.offsetMin > camera.offsetMax) // Validate that OffsetMin is not greater than OffsetMax 
            {
                validationErrorsOccurred = true;
                MessageBox.Show($"The minimum offset value {camera.offsetMin} is greater than the maximum offset value: {camera.offsetMax}");
            }

            if (RadNoOffset.Checked)
            {
                camera.offsetMode = Camera.OffsetMode.None;
            }
            else if (RadOffsets.Checked)
            {
                camera.offsetMode = Camera.OffsetMode.Offsets;
                if ((camera.offset < 0) | (camera.offset > camera.offsets.Count - 1)) camera.offset = 0; // initialise to first item if the current Offset is out of the Offsets array size

            }
            if (RadOffsetMinMax.Checked)
            {
                camera.offsetMode = Camera.OffsetMode.OffsetMinMax;
                if ((camera.offset < camera.offsetMin) | (camera.offset > camera.offsetMax)) camera.offset = camera.offsetMin; // initialise to OffsetMin if the current Offset is out side the min to max range
            }

            // Save sub exposure configuration
            camera.hasSubExposure = ChkHasSubExposure.Checked;
            if (double.TryParse(TxtSubExposure.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out double subExposureParsed)) // Validate OffsetMin
            {
                camera.subExposureInterval = subExposureParsed;
            }
            else
            {
                validationErrorsOccurred = true;
                MessageBox.Show($"The sub exposure value is invalid: '{TxtSubExposure.Text}'");
            }

            if (camera.subExposureInterval <= 0.0) // Validate that sub exposure is not negative or zero
            {
                validationErrorsOccurred = true;
                MessageBox.Show($"The sub exposure interval: {camera.subExposureInterval} must be positive and greater than 0.0");
            }

            camera.interfaceVersion = Decimal.ToInt16(NumInterfaceVersion.Value);

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

        private void NumInterfaceVersion_ValueChanged(object sender, EventArgs e)
        {
            // Enable the IcameraV2 properties if required
            textBoxBayerOffsetX.Enabled = NumInterfaceVersion.Value >= 2;
            textBoxBayerOffsetY.Enabled = NumInterfaceVersion.Value >= 2;
            textBoxGainMax.Enabled = NumInterfaceVersion.Value >= 2;
            textBoxGainMin.Enabled = NumInterfaceVersion.Value >= 2;
            textBoxMaxExposure.Enabled = NumInterfaceVersion.Value >= 2;
            textBoxMinExposure.Enabled = NumInterfaceVersion.Value >= 2;
            textBoxSensorName.Enabled = NumInterfaceVersion.Value >= 2;
            comboBoxSensorType.Enabled = NumInterfaceVersion.Value >= 2;
            radioButtonNoGain.Enabled = NumInterfaceVersion.Value >= 2;
            radioButtonUseGains.Enabled = NumInterfaceVersion.Value >= 2;
            radioButtonUseMinAndMax.Enabled = NumInterfaceVersion.Value >= 2;

            // Enable the ICameraV3 properties if required
            RadNoOffset.Enabled = NumInterfaceVersion.Value >= 3;
            RadOffsetMinMax.Enabled = NumInterfaceVersion.Value >= 3;
            RadOffsets.Enabled = NumInterfaceVersion.Value >= 3;
            TxtOffsetMax.Enabled = NumInterfaceVersion.Value >= 3;
            TxtOffsetMin.Enabled = NumInterfaceVersion.Value >= 3;
            ChkHasSubExposure.Enabled= NumInterfaceVersion.Value >= 3;
            TxtSubExposure.Enabled= (NumInterfaceVersion.Value >= 3) & ChkHasSubExposure.Checked;
        }

        private void ChkHasSubExposure_CheckedChanged(object sender, EventArgs e)
        {
            TxtSubExposure.Enabled = ChkHasSubExposure.Checked;
        }
    }
}