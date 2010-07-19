using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.HSFW_ASCOM_Driver
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private HSFW_Handler myHSFW;
      
        
        public SetupDialogForm()
        {
            InitializeComponent();
            myHSFW = HSFW_Handler.GetInstance();
            if (myHSFW.AttachedDeviceCount == 0) DisplayNoFilters();
        }

        private void DisplayNoFilters()
        {
            this.AttDev_CB.Items.Clear();
            this.AttDev_CB.Text = "None";
            this.AttDev_CB.Enabled = false;
            this.PrefSN_LBL.Text = myHSFW.PreferredSerialNumber;
            this.PrefSN_LBL.Enabled = false;
            this.Homed_LBL.Text = "False";
            this.Homed_LBL.Enabled = false;
            this.CurrentWheel_LBL.Text = "--";
            this.CurrentFilter_CB.Items.Clear();
            this.CurrentFilter_CB.Text = "--";
            this.CurrentFilter_CB.Enabled = false;
            Application.DoEvents();
        }

        private void DisplayFWAttached()
        {
            this.AttDev_CB.Items.Clear();
            this.AttDev_CB.Text = "None";
            this.AttDev_CB.Enabled = true;
            this.PrefSN_LBL.Text = myHSFW.PreferredSerialNumber;
            this.PrefSN_LBL.Enabled = true;
            this.Homed_LBL.Text = "False";
            this.Homed_LBL.Enabled = true;
            this.CurrentWheel_LBL.Text = "--";
            this.CurrentFilter_CB.Items.Clear();
            this.CurrentFilter_CB.Text = "--";
            this.CurrentFilter_CB.Enabled = true;
            Application.DoEvents();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void BrowseToAscom(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
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

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {

            RefreshAttachedDevice_CB();

        }

        private void RefreshAttachedDevice_CB()
        {
            //List<string> ads = myHSFW.AttachedDeviceList;
            AttDev_CB.DataSource = myHSFW.AttachedDeviceList;
        }

    }
}