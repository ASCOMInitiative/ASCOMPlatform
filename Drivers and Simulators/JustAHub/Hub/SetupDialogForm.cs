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
        string callingDeviceType;

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

                // Set the hosted driver labels
                LblCurrentCameraDevice.Text = $"{Settings.CameraHostedProgId}";
                tl.LogMessage("SetForm_Load", $"Hosted camera device ProgID: {Settings.CameraHostedProgId}");

                LblCurrentFilterWheelDevice.Text = $"{Settings.FilterWheelHostedProgId}";
                tl.LogMessage("SetForm_Load", $"Hosted filter wheel device ProgID: {Settings.FilterWheelHostedProgId}");

                // Set the trace checkboxes
                ChkLogDriverCallsCamera.Checked = Settings.CameraDriverLogging;
                tl.LogMessage("SetForm_Load", $"Log driver calls: {ChkLogDriverCallsCamera.Checked}");

                ChkDebugLoggingCamera.Checked = Settings.CameraHardwareLogging;
                tl.LogMessage("SetForm_Load", $"Log hardware calls: {ChkLogDriverCallsCamera.Checked}");

                ChkLogDriverCallsFilterWheel.Checked = Settings.FilterWheelDriverLogging;
                tl.LogMessage("SetForm_Load", $"Log driver calls: {ChkLogDriverCallsFilterWheel.Checked}");

                ChkDebugLoggingFilterWheel.Checked = Settings.FilterWheelHardwareLogging;
                tl.LogMessage("SetForm_Load", $"Log hardware calls: {ChkLogDriverCallsFilterWheel.Checked}");

                switch (callingDeviceType.ToUpperInvariant())
                {
                    case "CAMERA":
                        TabDevices.SelectTab("Camera");
                        break;

                    case "FILTERWHEEL":
                        TabDevices.SelectTab("FilterWheel");
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

        #region Event handlers

        private void CmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Save global values
            Settings.LocalServerLogging=ChkLocalServerDebugLog.Checked;

            // Save camera settings
            Settings.CameraDriverLogging= ChkLogDriverCallsCamera.Checked;
            Settings.CameraHardwareLogging = ChkDebugLoggingCamera.Checked;
            if (!string.IsNullOrEmpty(newCameraProgId)) // Update the camera ProgID if a new one has been chosen.
            {
                Settings.CameraHostedProgId = newCameraProgId;
            }

            // Save filter wheel settings
            Settings.FilterWheelDriverLogging = ChkLogDriverCallsFilterWheel.Checked;
            Settings.FilterWheelHardwareLogging = ChkDebugLoggingFilterWheel.Checked;
            if (!string.IsNullOrEmpty(newFilterWheelProgId)) // Update the camera ProgID if a new one has been chosen.
            {
                Settings.FilterWheelHostedProgId = newFilterWheelProgId;
            }
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

        private void BtnChooseCamera_Click(object sender, EventArgs e)
        {
            using (Chooser chooser = new Chooser())
            {
                CameraHardware.LogMessage("BtnChooseCamera_Click", $"Entered");
                chooser.DeviceType = "Camera";
                CameraHardware.LogMessage("BtnChooseCamera_Click", $"Device type: {chooser.DeviceType}, Current ProgID: {Settings.CameraHostedProgId}");
                newCameraProgId = chooser.Choose(Settings.CameraHostedProgId);

                // Update the setup UI with the new ProgID
                if (!string.IsNullOrEmpty(newCameraProgId))
                    LblCurrentCameraDevice.Text = $"{newCameraProgId}";

                CameraHardware.LogMessage("BtnChooseCamera_Click", $"Selection made: {newCameraProgId}");
            }
            CameraHardware.LogMessage("BtnChooseCamera_Click", $"Exited");
        }

        #endregion

        private void BtnChooseFilterWheel_Click(object sender, EventArgs e)
        {
            using (Chooser chooser = new Chooser())
            {
                CameraHardware.LogMessage("BtnChooseFilterWheel_Click", $"Entered");
                chooser.DeviceType = "FilterWheel";
                CameraHardware.LogMessage("BtnChooseFilterWheel_Click", $"Device type: {chooser.DeviceType}, Current ProgID: {Settings.FilterWheelHostedProgId}");
                newFilterWheelProgId = chooser.Choose(Settings.FilterWheelHostedProgId);

                // Update the setup UI with the new ProgID
                if (!string.IsNullOrEmpty(newFilterWheelProgId))
                    LblCurrentFilterWheelDevice.Text = $"{newFilterWheelProgId}";

                FilterWheelHardware.LogMessage("BtnChooseFilterWheel_Click", $"Selection made: {newFilterWheelProgId}");
            }
            FilterWheelHardware.LogMessage("BtnChooseFilterWheel_Click", $"Exited");
        }
    }
}