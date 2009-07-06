using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.TelescopeSimulator
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);


        public frmMain()
        {
            InitializeComponent();
        }
        public void DoSetupDialog()
        {
            SetupDialogForm setupForm = new SetupDialogForm();

            
            DialogResult ans = setupForm.ShowDialog(this);

            if (ans == DialogResult.OK)
            {
                
            }

            setupForm.Dispose();
        }

        private void buttonSetup_Click(object sender, EventArgs e)
        {
            DoSetupDialog();
        }

        private void buttonTraffic_Click(object sender, EventArgs e)
        {
            SharedResources.TrafficForm.Show();
        }
    }
}