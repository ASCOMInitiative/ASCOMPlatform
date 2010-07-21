using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Data;

namespace ASCOM.HSFW_ASCOM_Driver
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private HSFW_Handler myHandler;
      
        
        public SetupDialogForm()
        {
            InitializeComponent();
            
        }

        private void DisplayNoFWs()
        {
            this.AttDev_CB.Items.Clear();
            this.AttDev_CB.Text = "None";
            this.AttDev_CB.Enabled = false;

          
            this.CurrWheName_LBL.Enabled = false;
            this.CurrentWheelID_LBL.Enabled = false;    
            this.CurrentWheelID_LBL.Text = "--";
            this.CurrWheName_LBL.Text = "--";
            this.CurrentFilter_CB.Items.Clear();
            this.CurrentFilter_CB.Text = "--";
            this.CurrentFilter_CB.Enabled = false;
            Application.DoEvents();
        }

        private void UpdateDisplay()
        {
            if (myHandler.AttachedDeviceCount == 0) DisplayNoFWs();
            else DisplayFWAttached();
        }

        private void DisplayFWAttached()
        {
            if (myHandler.myDevice.ErrorState != 0)
                throw new ApplicationException("Device is errored");

            this.AttDev_CB.DataSource = myHandler.AttachedDeviceList;
            this.AttDev_CB.Enabled = true;

       
            this.CurrWheName_LBL.Text = myHandler.myDevice.
                GetWheelNames()[myHandler.myDevice.WheelID - 'A'].ToString();
            this.CurrentWheelID_LBL.Text = myHandler.myDevice.WheelID.ToString(); ;
            this.CurrentFilter_CB.Items.Clear();
            this.CurrentFilter_CB.Text = "--";
            this.CurrentFilter_CB.Enabled = true;
            this.CurrentFilter_CB.Items.Clear();
            // Update the filter names
            for (int i = 0; i < myHandler.myDevice.NumberOfFilters; i++)
            {
                this.CurrentFilter_CB.Items.Add(myHandler.myDevice.GetFilterNames(myHandler.myDevice.WheelID)[i]);
            }
            CurrentFilter_CB.SelectedIndex = myHandler.myDevice.CurrentPosition - 1;


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


        private void RefreshAttachedDevice_CB()
        {
            AttDev_CB.DataSource = myHandler.AttachedDeviceList;
        }

        private void CurrentFilter_CB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            myHandler.myDevice.CurrentPosition = (short)(this.CurrentFilter_CB.SelectedIndex + 1);
        }

        private void SettingsBTN_Click(object sender, EventArgs e)
        {
            OptecHID_FilterWheelAPI.FilterWheel fw = myHandler.myDevice;
            SetupForm s = new SetupForm(fw);
            s.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string msg = "This box lists the serial number of all Optec High Speed Filter Wheel " +
                "devices that are currently attached to your PC. Selecting a device here will make " +
                "it your \"preferred\" device; i.e., this driver will control it.";
            MessageBox.Show(msg, "Available Devices Help",MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SetupDialogForm_Shown(object sender, EventArgs e)
        {
            try
            {
                myHandler = HSFW_Handler.GetInstance();
                UpdateDisplay();
                RefreshAttachedDevice_CB();
                if (myHandler.myDevice == null)
                {
                    MessageBox.Show("No High Speed Filter Wheels are connected to this machine.");
                }
                this.Status_LBL.Text = "No Devices Found";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}