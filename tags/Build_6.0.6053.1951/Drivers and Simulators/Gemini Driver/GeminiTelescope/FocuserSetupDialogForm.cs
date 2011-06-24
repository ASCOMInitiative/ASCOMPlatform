using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.GeminiTelescope
{
    public partial class FocuserSetupDialogForm : Form
    {
        public FocuserSetupDialogForm()
        {
            InitializeComponent();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxComPort.Items.Add(s);
            }

            Version version = new Version(Application.ProductVersion);
            labelVersion.Text = "ASCOM Gemini Telescope .NET " + string.Format("Version {0}.{1}.{2}", version.Major, version.Minor, version.Build);
            TimeZone localZone = TimeZone.CurrentTimeZone;

            labelTime.Text = "Time zone is " + (localZone.IsDaylightSavingTime(DateTime.Now) ? localZone.DaylightName : localZone.StandardName);
        }

        #region Properties for Settings
        public string ComPort
        {
            get { return comboBoxComPort.SelectedItem.ToString(); }
            set { comboBoxComPort.SelectedItem = value; }
        }

        public string BaudRate
        {
            get { return comboBoxBaudRate.SelectedItem.ToString(); }
            set { comboBoxBaudRate.SelectedItem = value; }
        }

        public bool ReverseDirection
        {
            get { return checkBoxReverseDirection.Checked; }
            set { checkBoxReverseDirection.Checked = value; }
        }
        public int MaxIncrement
        {
            get { return (int)numericUpDownMaxIncrement.Value; }
            set { numericUpDownMaxIncrement.Value = value; }
        }
        public int StepSize
        {
            get { return (int)numericUpDownStepSize.Value; }
            set { numericUpDownStepSize.Value = value; }
        }
        public int BrakeSize
        {
            get { return (int)numericUpDownBrakeSize.Value; }
            set { numericUpDownBrakeSize.Value = value; }
        }
        public int BacklashSize
        {
            get { return (int)numericUpDownBacklashSize.Value; }
            set { numericUpDownBacklashSize.Value = value; }
        }
        public int Speed
        {
            get
            {
                if (radioButtonSlow.Checked) { return 1; }
                if (radioButtonMedium.Checked) { return 2; }
                if (radioButtonFast.Checked) { return 3; }
                return 1;
            }
            set
            {
                switch (value)
                {
                    case 1:
                        radioButtonSlow.Checked = true;
                        radioButtonMedium.Checked =false;
                        radioButtonFast.Checked = false;
                        break;
                    case 2:
                        radioButtonSlow.Checked = false;
                        radioButtonMedium.Checked = true;
                        radioButtonFast.Checked = false;
                        break;
                    case 3:
                        radioButtonSlow.Checked = false;
                        radioButtonMedium.Checked = false;
                        radioButtonFast.Checked = true;
                        break;

                }
            }
        }
        public int BacklashDirection
        {
            get
            {
                if (radioButtonIn.Checked) { return -1; }
                if (radioButtonNone.Checked) { return 0; }
                if (radioButtonOut.Checked) { return 1; }
                return 0;
            }
            set
            {
                switch (value)
                {
                    case -1:
                        radioButtonIn.Checked = true;
                        radioButtonNone.Checked = false;
                        radioButtonOut.Checked = false;
                        break;
                    case 0:
                        radioButtonIn.Checked = false;
                        radioButtonNone.Checked = true;
                        radioButtonOut.Checked = false;
                        break;
                    case 1:
                        radioButtonIn.Checked = false;
                        radioButtonNone.Checked = false;
                        radioButtonOut.Checked = true;
                        break;

                }
            }
        }
        public bool AbsoluteFocuser
        {
            get
            {
                return checkBoxAbsolute.Checked;
            }
            set
            {
                checkBoxAbsolute.Checked = value;
            }
        }
        #endregion

        private void FocuserSetupDialogForm_Load(object sender, EventArgs e)
        {
            SharedResources.SetTopWindow(this);
        }
    }
}
