using ASCOM.Utilities;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCOM.CameraHub.Camera
{
    [ComVisible(false)] // Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        readonly TraceLogger tl; // Holder for a reference to the driver's trace logger
        string newProgId;

        public SetupDialogForm(TraceLogger tlDriver)
        {
            InitializeComponent();

            // Save the provided trace logger for use within the setup dialogue
            tl = tlDriver;

            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void CmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here and update the state variables with results from the dialogue

            tl.Enabled = chkTrace.Checked; // Update the trace state

            if (!string.IsNullOrEmpty(newProgId)) // Update the camera ProgID if a new one has been chosen.
                CameraHub.cameraProgId = newProgId;
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

        private void InitUI()
        {

            // Set the trace checkbox
            chkTrace.Checked = tl.Enabled;

            tl.LogMessage("InitUI", $"Set UI controls to Trace: {chkTrace.Checked}");
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
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

        private void BtnChooseCamera_Click(object sender, EventArgs e)
        {
            using (Chooser chooser = new Chooser())
            {
                CameraHub.LogMessage("BtnChooseCamera_Click", $"Entered");
                chooser.DeviceType = "Camera";
                CameraHub.LogMessage("BtnChooseCamera_Click", $"Device type: {chooser.DeviceType}, Current ProgID: {CameraHub.cameraProgId}");
                newProgId = chooser.Choose(CameraHub.cameraProgId);
                CameraHub.LogMessage("BtnChooseCamera_Click", $"Selection made: {newProgId}");
            }
            CameraHub.LogMessage("BtnChooseCamera_Click", $"Exited");
        }
    }
}