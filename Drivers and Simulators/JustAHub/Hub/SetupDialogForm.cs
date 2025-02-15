using ASCOM.Utilities;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCOM.JustAHub
{
    [ComVisible(false)] // Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        readonly TraceLogger tl; // Holder for a reference to the driver's trace logger
        string newCameraProgId;
        string newFilterWheelProgId;
        string newCoverCalibratorProgId;
        string newFocuserProgId;
        readonly string callingDeviceType;

        #region Initialisation and form load

        public SetupDialogForm(TraceLogger tlDriver, string callingDeviceType)
        {
            InitializeComponent();
            try
            {
                // Save the provided trace logger for use within the setup dialogue
                tl = tlDriver;
                this.callingDeviceType = callingDeviceType;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"JustAHub SetupDialogForm (calling device: {callingDeviceType}) exception: {ex.Message}\r\n{ex}");
            }

        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Global values
                ChkLocalServerDebugLog.Checked = Settings.LocalServerLogging;

                // Camera values
                LblCurrentCameraDevice.Text = $"{Settings.CameraHostedProgId}";
                tl.LogMessage("SetForm_Load", $"Hosted camera device ProgID: {Settings.CameraHostedProgId}");

                ChkDriverLoggingCamera.Checked = Settings.CameraDriverLogging;
                tl.LogMessage("SetForm_Load", $"Log driver calls: {Settings.CameraDriverLogging}");

                ChkHardwareLoggingCamera.Checked = Settings.CameraHardwareLogging;
                tl.LogMessage("SetForm_Load", $"Log hardware calls: {Settings.CameraHardwareLogging}");

                // Cover Calibrator values
                LblCurrentCoverCalibratorDevice.Text = $"{Settings.CoverCalibratorHostedProgId}";
                tl.LogMessage("SetForm_Load", $"Hosted cover calibrator device ProgID: {Settings.CoverCalibratorHostedProgId}");

                ChkDriverLoggingCoverCalibrator.Checked = Settings.CoverCalibratorDriverLogging;
                tl.LogMessage("SetForm_Load", $"Log driver calls: {Settings.CoverCalibratorDriverLogging}");

                ChkHardwareLoggingCoverCalibrator.Checked = Settings.CoverCalibratorHardwareLogging;
                tl.LogMessage("SetForm_Load", $"Log hardware calls: {Settings.CoverCalibratorHardwareLogging}");

                // Filter Wheel values
                LblCurrentFilterWheelDevice.Text = $"{Settings.FilterWheelHostedProgId}";
                tl.LogMessage("SetForm_Load", $"Hosted filter wheel device ProgID: {Settings.FilterWheelHostedProgId}");

                ChkDriverLoggingFilterWheel.Checked = Settings.FilterWheelDriverLogging;
                tl.LogMessage("SetForm_Load", $"Log driver calls: {Settings.FilterWheelDriverLogging}");

                ChkHardwareLoggingFilterWheel.Checked = Settings.FilterWheelHardwareLogging;
                tl.LogMessage("SetForm_Load", $"Log hardware calls: {Settings.FilterWheelHardwareLogging}");

                // Focuser values
                LblCurrentFocuserDevice.Text = $"{Settings.FocuserHostedProgId}";
                tl.LogMessage("SetForm_Load", $"Hosted focuser ProgID: {Settings.FocuserHostedProgId}");

                ChkDriverLoggingFocuser.Checked = Settings.FocuserDriverLogging;
                tl.LogMessage("SetForm_Load", $"Log driver calls: {Settings.FocuserDriverLogging}");

                ChkHardwareLogingFocuser.Checked = Settings.FocuserHardwareLogging;
                tl.LogMessage("SetForm_Load", $"Log hardware calls: {Settings.FocuserHardwareLogging}");

                // Select the appropriate tab
                switch (callingDeviceType.ToUpperInvariant())
                {
                    case "CAMERA":
                        TabDevices.SelectTab("Camera");
                        break;

                    case "COVERCALIBRATOR":
                        TabDevices.SelectTab("CoverCalibrator");
                        break;

                    case "FILTERWHEEL":
                        TabDevices.SelectTab("FilterWheel");
                        break;

                    case "FOCUSER":
                        TabDevices.SelectTab("Focuser");
                        break;

                    default:
                        throw new InvalidValueException($"SetupDialogueForm - Invalid device type: {callingDeviceType}.");
                }

                // Bring the setup dialogue to the front of the screen
                if (WindowState == FormWindowState.Minimized)
                    WindowState = FormWindowState.Normal;
                else
                {
                    TopMost = true;
                    Focus();
                    BringToFront();
                    TopMost = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"JustAHub Setup Form-Load exception: {ex.Message}\r\n{ex}");
            }
        }

        #endregion

        #region Common event handlers

        private void CmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Save global values
            Settings.LocalServerLogging = ChkLocalServerDebugLog.Checked;

            // Save camera settings
            Settings.CameraDriverLogging = ChkDriverLoggingCamera.Checked;
            Settings.CameraHardwareLogging = ChkHardwareLoggingCamera.Checked;
            if (!string.IsNullOrEmpty(newCameraProgId)) // Update the camera ProgID if a new one has been chosen.
                Settings.CameraHostedProgId = newCameraProgId;

            // Save cover calibrator settings
            Settings.CoverCalibratorDriverLogging = ChkDriverLoggingCoverCalibrator.Checked;
            Settings.CoverCalibratorHardwareLogging = ChkHardwareLoggingCoverCalibrator.Checked;
            if (!string.IsNullOrEmpty(newCoverCalibratorProgId)) // Update the camera ProgID if a new one has been chosen.
                Settings.CoverCalibratorHostedProgId = newCoverCalibratorProgId;

            // Save filter wheel settings
            Settings.FilterWheelDriverLogging = ChkDriverLoggingFilterWheel.Checked;
            Settings.FilterWheelHardwareLogging = ChkHardwareLoggingFilterWheel.Checked;
            if (!string.IsNullOrEmpty(newFilterWheelProgId)) // Update the camera ProgID if a new one has been chosen.
                Settings.FilterWheelHostedProgId = newFilterWheelProgId;

            // Save focuser settings
            Settings.FocuserDriverLogging = ChkDriverLoggingFocuser.Checked;
            Settings.FocuserHardwareLogging = ChkHardwareLogingFocuser.Checked;
            if (!string.IsNullOrEmpty(newFocuserProgId)) // Update the camera ProgID if a new one has been chosen.
                Settings.FocuserHostedProgId = newFocuserProgId;
        }

        private void CmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("https://ascom-standards.org/");
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        #endregion

        #region Device event handlers

        private void BtnChooseCamera_Click(object sender, EventArgs e)
        {
            newCameraProgId = HandleChooserClick(CameraHardware.TL, "Camera", Settings.CameraHostedProgId, LblCurrentCameraDevice);
        }


        private void BtnChooseCoverCalibrator_Click(object sender, EventArgs e)
        {
            newCoverCalibratorProgId = HandleChooserClick(CoverCalibratorHardware.TL, "CoverCalibrator", Settings.CoverCalibratorHostedProgId, LblCurrentCoverCalibratorDevice);
        }

        private void BtnChooseFilterWheel_Click(object sender, EventArgs e)
        {
            newFilterWheelProgId = HandleChooserClick(FilterWheelHardware.TL, "FilterWheel", Settings.FilterWheelHostedProgId, LblCurrentFilterWheelDevice);
        }

        private void BtnChooseFocuser_Click(object sender, EventArgs e)
        {
            newFocuserProgId = HandleChooserClick(FocuserHardware.TL, "Focuser", Settings.FocuserHostedProgId, LblCurrentFocuserDevice);
        }

        #endregion

        #region Support code

        private static string HandleChooserClick(TraceLogger TL, string deviceType, string currentProgId, Label label)
        {
            string newProgId = null;

            TL.LogMessage("HandleChooserClick", $"Device type: {deviceType}, Current ProgID: {currentProgId}");

            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = deviceType;
                newProgId = chooser.Choose(currentProgId);
            }

            // Update the setup UI with the new ProgID
            if (!string.IsNullOrEmpty(newProgId))
                label.Text = $"{newProgId}";

            TL.LogMessage("HandleChooserClick", $"Selected ProgID: '{newProgId}'");

            return newProgId;
        }

        #endregion

    }
}