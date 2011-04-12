using System;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
    public partial class FocuserSettingsForm : Form
    {
        private Focuser _focuser; // = new Focuser();

        /// <summary>
        /// This is the method to run when the class is constructe
        /// </summary>
        public FocuserSettingsForm(Focuser MyFocuser)
        {
            _focuser = MyFocuser;
            _focuser.TL.LogMessage("FocusSettingsForm", "New");
            _focuser.TL.LogMessage("FocusSettingsForm", "Initialising component");
            InitializeComponent();
            _focuser.TL.LogMessage("FocusSettingsForm", "initialised OK");
        }

        /// <summary>
        /// This is the method to run when you want the form loaded with the class settings
        /// </summary>
        private void SettingsFormLoad(object sender, EventArgs e)
        {
            _focuser.TL.LogMessage("FocusSettingsForm", "Form loading");
            txtMaxStepPosition.Text = _focuser.MaxStep.ToString();
            txtStepSize.Text = _focuser.StepSize.ToString();
            txtMaxIncrement.Text = _focuser.MaxIncrement.ToString();
            txtCurrentTemperature.Text = _focuser.Temperature.ToString();
            txtMaximumTemperature.Text = _focuser.TempMax.ToString();
            txtMinimumTemperature.Text = _focuser.TempMin.ToString();
            txtUpdatePeriod.Text = _focuser.TempPeriod.ToString();
            txtStepsPerDegree.Text = _focuser.TempSteps.ToString();

            chkHasTempProbe.Checked = _focuser.TempProbe;
            chkHasTempComp.Checked = _focuser.TempCompAvailable;
            chkCanChangeStepSize.Checked = _focuser.CanStepSize;
            chkCanHalt.Checked = _focuser.CanHalt;
            chkIsSynchronous.Checked = _focuser.Synchronous;

            radAbsoluteFocuser.Checked = _focuser.Absolute;
            radRelativeFocuser.Checked = !radAbsoluteFocuser.Checked;
            this.BringToFront();
            _focuser.TL.LogMessage("FocusSettingsForm", "Form loaded");
        }

        /// <summary>
        /// This is the method to run when you want save the contents of the form to the profile
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            //save textboxes
            _focuser.MaxStep = Convert.ToInt32(txtMaxStepPosition.Text);
            _focuser.StepSize = Convert.ToDouble(txtStepSize.Text);
            _focuser.MaxIncrement = Convert.ToInt32(txtMaxIncrement.Text);
            _focuser.Temperature = Convert.ToDouble(txtCurrentTemperature.Text);
            _focuser.TempMax = Convert.ToDouble(txtMaximumTemperature.Text);
            _focuser.TempMin = Convert.ToDouble(txtMinimumTemperature.Text);
            _focuser.TempPeriod = Convert.ToDouble(txtUpdatePeriod.Text);
            _focuser.TempSteps = Convert.ToInt32(txtStepsPerDegree.Text);

            //save checkboxes
            _focuser.TempProbe = chkHasTempProbe.Checked;
            _focuser.TempCompAvailable = chkHasTempComp.Checked;
            _focuser.CanStepSize = chkCanChangeStepSize.Checked;
            _focuser.CanHalt = chkCanHalt.Checked;
            _focuser.Synchronous = chkIsSynchronous.Checked;

            //save radio button
            _focuser.Absolute = radAbsoluteFocuser.Checked;

            _focuser.SaveProfileSettings();
            Hide();
        }

        /// <summary>
        /// This is the method to run when you want to turn the tempature probe off or on 
        /// </summary>
        private void chkHasTempProbe_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkHasTempProbe.Checked)
            {
                chkHasTempProbe.Checked = false;
                chkHasTempComp.Enabled = false;
                chkHasTempComp.Checked = false;
                txtCurrentTemperature.Enabled = false;
                txtMaximumTemperature.Enabled = false;
                txtMinimumTemperature.Enabled = false;
                txtUpdatePeriod.Enabled = false;
                txtStepsPerDegree.Enabled = false;
            }
            else
            {
                chkHasTempProbe.Checked = true;
                chkHasTempComp.Enabled = true;
                txtCurrentTemperature.Enabled = true;
                txtMaximumTemperature.Enabled = true;
                txtMinimumTemperature.Enabled = true;
                txtUpdatePeriod.Enabled = true;
                txtStepsPerDegree.Enabled = true;
            }
        }

        /// <summary>
        /// This is the method to run when you want to turn the tempature compensator off or on
        /// </summary>
        private void chkHasTempComp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHasTempComp.Checked) chkHasTempProbe.Checked = true;
        }

        /// <summary>
        /// This is the method to run when you want to turn the step size off or on
        /// </summary>
        private void chkCanChangeStepSize_CheckedChanged(object sender, EventArgs e)
        {
            txtStepSize.Enabled = chkCanChangeStepSize.Checked;
        }
    }
}
