using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestHarness
{
    public partial class HubSetupForm : Form
    {
        public HubSetupForm()
        {
            InitializeComponent();
        }

        private void HubSetupForm_Load(object sender, EventArgs e)
        {
            this.propertyGrid1.SelectedObject = new HubSettings();
        }

        private void HubSetupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }


    }

    public class HubSettings
    {
        public HubSettings()
        {
        }

        [Category("Hub Settings")]
        [DisplayName("Show Focuser 2")]
        [Description("Set False to hide Focuser 2 controls")]
        public bool ShowFocuser2
        {
            get { return !Properties.Settings.Default.Focuser2Disabled; }
            set { Properties.Settings.Default.Focuser2Disabled = !value;  }
        }

        [Category("Hub Settings")]
        [DisplayName("Switch Focusers")]
        [Description("Set True to move the Focuser 2 controls to the left side and Focuser 1 Controls to the right")]
        public bool ShitchFocusers
        {
            get { return Properties.Settings.Default.SwitchF1F2; }
            set { Properties.Settings.Default.SwitchF1F2 = value;  }
        }


    }
}
