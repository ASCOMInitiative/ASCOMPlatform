using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.LocalServer
{
    public partial class FrmMain : Form
    {
        private delegate void SetTextCallback(string text);

        public FrmMain()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Visible = false;
            this.WindowState= FormWindowState.Minimized;
        }

    }
}