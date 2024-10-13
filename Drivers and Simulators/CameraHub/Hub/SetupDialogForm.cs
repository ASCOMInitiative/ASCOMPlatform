using ASCOM.Utilities;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCOM.CameraHub
{
    [ComVisible(false)] // Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        readonly TraceLogger tl; // Holder for a reference to the driver's trace logger
        string newProgId;

        #region Initialisation and form load

        public SetupDialogForm(TraceLogger tlDriver)
        {
            InitializeComponent();

            // Save the provided trace logger for use within the setup dialogue
            tl = tlDriver;
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Set the trace checkbox
                chkTrace.Checked = tl.Enabled;
                tl.LogMessage("SetForm_Load", $"Set UI controls to Trace: {chkTrace.Checked}");

                LblCurrentCameraDevice.Text = $"{CameraHardware.hostedCameraProgId}";
                tl.LogMessage("SetForm_Load", $"Hosted camera device ProgID: {CameraHardware.hostedCameraProgId}");

                LblCurrentFilterWheelDevice.Text = $"{FilterWheelHardware.hostedFilterWheelProgId}";
                tl.LogMessage("SetForm_Load", $"Hosted filter wheel device ProgID: {FilterWheelHardware.hostedFilterWheelProgId}");

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
                MessageBox.Show($"CameraHub Setup Form-Load exception: {ex.Message}\r\n{ex}");
            }
        }

        #endregion

        #region Event handlers

        private void CmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here and update the state variables with results from the dialogue

            tl.Enabled = chkTrace.Checked; // Update the trace state

            if (!string.IsNullOrEmpty(newProgId)) // Update the camera ProgID if a new one has been chosen.
                CameraHardware.hostedCameraProgId = newProgId;
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
                CameraHardware.LogMessage("BtnChooseCamera_Click", $"Device type: {chooser.DeviceType}, Current ProgID: {CameraHardware.hostedCameraProgId}");
                newProgId = chooser.Choose(CameraHardware.hostedCameraProgId);

                // Update the setup UI with the new ProgID
                if (!string.IsNullOrEmpty(newProgId))
                    LblCurrentCameraDevice.Text = $"{newProgId}";

                CameraHardware.LogMessage("BtnChooseCamera_Click", $"Selection made: {newProgId}");
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
                CameraHardware.LogMessage("BtnChooseFilterWheel_Click", $"Device type: {chooser.DeviceType}, Current ProgID: {FilterWheelHardware.hostedFilterWheelProgId}");
                newProgId = chooser.Choose(FilterWheelHardware.hostedFilterWheelProgId);

                // Update the setup UI with the new ProgID
                if (!string.IsNullOrEmpty(newProgId))
                    LblCurrentFilterWheelDevice.Text = $"{newProgId}";

                FilterWheelHardware.LogMessage("BtnChooseFilterWheel_Click", $"Selection made: {newProgId}");
            }
            FilterWheelHardware.LogMessage("BtnChooseFilterWheel_Click", $"Exited");
        }
    }
}