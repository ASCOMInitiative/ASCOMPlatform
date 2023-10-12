using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Diagnostics;

namespace ASCOM.Simulator
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        TraceLogger TL;

        public SetupDialogForm(TraceLogger traceLogger)
        {
            InitializeComponent();
            TL = traceLogger;

            this.Load += SetupDialogForm_Load;
            // Initialise current values of user settings from the ASCOM Profile
            InitUI();

        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            // Bring this form to the front of the screen
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void CmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Update the state variables with results from the dialogue
            TL.Enabled = chkTrace.Checked;
            CoverCalibrator.MaxBrightnessValue = Decimal.ToInt32(NumMaxBrightness.Value);
            CoverCalibrator.CalibratorStablisationTimeValue = Decimal.ToDouble(NumCalibratorStablisationTime.Value);
            CoverCalibrator.CoverOpeningTimeValue = Decimal.ToDouble(NumCoverOpeningTime.Value);
            Enum.TryParse<CalibratorStatus>(CmbCalibratorInitialisationState.SelectedItem.ToString(), out CoverCalibrator.CalibratorStateInitialisationValue);
            Enum.TryParse<CoverStatus>(CmbCoverInitialisationState.SelectedItem.ToString(), out CoverCalibrator.CoverStateInitialisationValue);
        }

        private void CmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                Process.Start("https://ascom-standards.org/");
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
            chkTrace.Checked = TL.Enabled;
            NumMaxBrightness.Value = (decimal)CoverCalibrator.MaxBrightnessValue;
            NumCalibratorStablisationTime.Value = (decimal)CoverCalibrator.CalibratorStablisationTimeValue;
            NumCoverOpeningTime.Value = (decimal)CoverCalibrator.CoverOpeningTimeValue;
            CmbCalibratorInitialisationState.SelectedItem = CoverCalibrator.CalibratorStateInitialisationValue.ToString();
            CmbCoverInitialisationState.SelectedItem = CoverCalibrator.CoverStateInitialisationValue.ToString();

            LblSynchBehaviourTime.Text = $"* Methods will be synchronous from 0.0 and {CoverCalibrator.SYNCHRONOUS_BEHAVIOUR_LIMIT:0.0} seconds and asynchronous above this.";
        }

    }
}