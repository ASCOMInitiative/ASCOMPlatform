using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PyxisLE_API;

namespace PyxisLE_Control
{
    public partial class AdvancedForm : Form
    {
        private Rotator myRotator;

        public AdvancedForm(Rotator r)
        {
            InitializeComponent();
            myRotator = r;
        }

        private void AdvancedForm_Load(object sender, EventArgs e)
        {
            RotatorAdvancedSettingsUI UIClass = new RotatorAdvancedSettingsUI(myRotator);
            this.propertyGrid1.SelectedObject = UIClass;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    class RotatorAdvancedSettingsUI
    {
        private Rotator myRotator;

        public RotatorAdvancedSettingsUI(Rotator r)
        {
            myRotator = r;
        }

        [Category("Settings")]
        [DisplayName("Zero Offset")]
        [Description("Adjust the rotators zero point. This is used when homing the device. Changing this value will force the device to home.")]
        public short ZeroOffset
        {
            get { return myRotator.ZeroOffset; }
            set { myRotator.ZeroOffset = value; }
        }

        [Category("Settings")]
        public bool Reverse
        {
            get { return myRotator.Reverse; }
            set { myRotator.Reverse = value; }
        }

        [Category("Settings")]
        [DisplayName("Return to Last")]
        public bool ReturnToLast
        {
            get { return myRotator.ReturnToLastOnHome; }
            set { myRotator.ReturnToLastOnHome = value; }
        }

        [Category("Device Description")]
        [DisplayName("SerialNumber")]
        public string SerialNumber
        {
            get { return myRotator.SerialNumber; }
        }

        [Category("Device Description")]
        public string Resolution
        {
            get { return myRotator.StepsPerRev.ToString();}
        }

        [Category("Device Description")]
        [DisplayName("Firmware Version")]
        public string FirmwareVersion
        {
            get { return myRotator.FirmwareVersion; }
        }

    }
}
