using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.MultiInstanceSimulator
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private Focuser focuser;

        //private bool isAbsoluteTemp;
        //private int maxStepTemp;
        //private int maxIncrementTemp;
        //private double stepSizeTemp;
        //private bool tempCompAvailableTemp;
        //internal int roc = 100;
        //internal bool tempComp = false;
        //internal double temperature = 10;

        public SetupDialogForm(Focuser focuser)
        {
            InitializeComponent();
            this.focuser = focuser;
            radioButtonAbsolute.Checked = focuser.isAbsolute;
            radioButtonRelative.Checked = !focuser.isAbsolute;
            textBoxMaxIncrement.Text = focuser.maxIncrement.ToString();
            textBoxStepSize.Text = focuser.stepSize.ToString();
            textBoxMaxStep.Text = focuser.maxStep.ToString();
            checkBoxTempCompAvailable.Checked = focuser.tempCompAvailable;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            focuser.isAbsolute = radioButtonAbsolute.Checked;
            focuser.maxIncrement = Convert.ToInt32(textBoxMaxIncrement.Text);
            focuser.maxStep = Convert.ToInt32(textBoxMaxStep.Text);
            focuser.stepSize = Convert.ToDouble(textBoxStepSize.Text);
            focuser.tempCompAvailable = checkBoxTempCompAvailable.Checked;
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

    }
}