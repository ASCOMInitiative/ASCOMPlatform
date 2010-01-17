using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.Simulator.Switch
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);

        public frmMain()
        {
            InitializeComponent();
        }
        public void SetupDialogForm()
        {
            frmSetup setupForm = new frmSetup();
            DialogResult ans = setupForm.ShowDialog(this);
            if (ans == DialogResult.OK)
            {
            }
            setupForm.Dispose();
        }

    }
}