using System;
using System.Windows.Forms;

namespace ASCOM.SettingsProviderSample.CSharp
{
    public partial class SetupDialog : Form
    {
        public SetupDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            Close();
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            Close();
        }
    }
}
