using System;
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
            this.Text = $"ASCOM JustAHub ({(Environment.Is64BitProcess?"64":"32")}bit)";
        }

    }
}