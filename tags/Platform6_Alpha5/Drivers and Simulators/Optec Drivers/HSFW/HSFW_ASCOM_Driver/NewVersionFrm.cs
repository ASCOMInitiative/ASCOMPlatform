using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.HSFW_ASCOM_Driver
{
    public partial class NewVersionFrm : Form
    {
        private string url_string = "";
        public NewVersionFrm(string CurrentVer, string LatestVer, string url)
        {
            InitializeComponent();
            InstalledVer_lbl.Text = CurrentVer;
            LatestVersion_Lbl.Text = LatestVer;
            url_string = url;
        }

        private void NewVersionFrm_Load(object sender, EventArgs e)
        {
            
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void url_link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(url_string);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }
    }
}
