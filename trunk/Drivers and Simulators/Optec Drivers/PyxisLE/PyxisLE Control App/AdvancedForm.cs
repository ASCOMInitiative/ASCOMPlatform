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

        public short ZeroOffset
        {
            get { return myRotator.ZeroOffset; }
            set { myRotator.ZeroOffset = value; }
        }

        public bool Reverse
        {
            get { return myRotator.Reverse; }
            set { myRotator.Reverse = value; }
        }

        public bool ReturnToLast
        {
            get { return myRotator.ReturnToLastOnHome; }
            set { myRotator.ReturnToLastOnHome = value; }
        }
    }
}
