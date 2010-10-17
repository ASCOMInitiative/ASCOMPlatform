using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.GeminiTelescope
{
    public partial class frmNetworkSettings : Form
    {
        public frmNetworkSettings()
        {
            InitializeComponent();


            txtIP.Culture = GeminiHardware.m_GeminiCulture;
            System.Net.IPAddress ip;
            if (!System.Net.IPAddress.TryParse(GeminiHardware.EthernetIP, out ip))
                ip = System.Net.IPAddress.Parse("192.168.000.100");

            txtIP.Text = string.Format("{0:000}.{1:000}.{2:000}.{3:000}",
                ip.GetAddressBytes()[0],
                ip.GetAddressBytes()[1],
                ip.GetAddressBytes()[2],
                ip.GetAddressBytes()[3]);

                        
            txtUser.Text = GeminiHardware.EthernetUser;
            txtPassword.Text = GeminiHardware.EthernetPassword;
            chkNoProxy.Checked = GeminiHardware.BypassProxy;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string tIP = txtIP.Text;
            tIP = tIP.Replace(" ", "");
            tIP = tIP.Trim();

            System.Net.IPAddress ip;
            if (!System.Net.IPAddress.TryParse(tIP, out ip))
            {
                MessageBox.Show("Invalid IP address: " + txtIP.Text);
                txtIP.Focus();
                return;
            }

            GeminiHardware.EthernetIP = ip.ToString();
            GeminiHardware.EthernetUser = txtUser.Text;
            GeminiHardware.EthernetPassword = txtPassword.Text;
            GeminiHardware.BypassProxy = chkNoProxy.Checked;
            DialogResult = DialogResult.OK;
            this.Close();
        }       
    }
}
