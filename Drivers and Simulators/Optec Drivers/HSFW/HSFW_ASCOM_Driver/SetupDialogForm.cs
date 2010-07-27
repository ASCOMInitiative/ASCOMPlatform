using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Data;
using System.Reflection;

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

        
        private bool CheckAtLeastOneDevice()
        {
            if (myHandler.AttachedDeviceCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    
        private void UpdateDisplay()
        {
            if (CheckAtLeastOneDevice())
                {
                    if (myHandler.myDevice == null)
                    {
                        UpdateDisplay_NoFW();
                        this.AttDev_CB.Enabled = true;
                    }
                    else
                    {
                        if (myHandler.myDevice.ErrorState == 0) UpdateDisplay_GoodFW();
                        else UpdateDisplay_ErroredFW();
                    }
                }
                else
                {
                    UpdateDisplay_NoFW();
                    MessageBox.Show("No High Speed Filter Wheels are connected to this machine.");
                }
        }

        private void SetupDialogForm_Shown(object sender, EventArgs e)
        {
            try
            {
                this.NewVersionCheckerBGW.RunWorkerAsync();
                myHandler = HSFW_Handler.GetInstance();
                myHandler.DeviceListChanged += new EventHandler(myHandler_DeviceListChanged);
                UpdateDisplay();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private delegate void UI_Update();
        void myHandler_DeviceListChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UI_Update(UpdateDisplay));
            }
            else UpdateDisplay();
        }



        private void UpdateDisplay_GoodFW()
        {
            UpdateAttachedDevice_CB();
            this.AttDev_CB.Enabled = true;
            this.SettingsBTN.Enabled = true;
            this.CurrentWheelID_LBL.Enabled = true;
            this.CurrWheName_LBL.Enabled = true;
            this.ReHome_Btn.Visible = true;

            this.CurrWheName_LBL.Text = myHandler.myDevice.
                GetWheelNames()[myHandler.myDevice.WheelID - 'A'].ToString();
            this.CurrentWheelID_LBL.Text = myHandler.myDevice.WheelID.ToString(); 
            this.ReHome_Btn.Text = "Home Device";
            this.Status_LBL.Text = "Device is functioning properly and ready for action!";
            // Update the filter names
            for (int i = 0; i < myHandler.myDevice.NumberOfFilters; i++)
            {
                this.CurrentFilter_CB.Items.Add(myHandler.myDevice.GetFilterNames(myHandler.myDevice.WheelID)[i]);
            }
            CurrentFilter_CB.SelectedIndex = myHandler.myDevice.CurrentPosition - 1;


            Application.DoEvents();
        }

        private void UpdateAttachedDevice_CB()
        {
            AttDev_CB.Items.Clear();
            foreach (string s in myHandler.AttachedDeviceList)
            {
                AttDev_CB.Items.Add(s);
            }
        }

        private void UpdateDisplay_ErroredFW()
        {
            RefreshAttachedDevice_CB();
            this.AttDev_CB.Enabled = true;
            this.SettingsBTN.Enabled = false;
            this.CurrWheName_LBL.Enabled = false;
            this.CurrentWheelID_LBL.Enabled = false;
            this.CurrentWheelID_LBL.Text = "--";
            this.CurrWheName_LBL.Text = "--";
            this.CurrentFilter_CB.Items.Clear();
            this.CurrentFilter_CB.Text = "--";
            this.CurrentFilter_CB.Enabled = false;
            this.ReHome_Btn.Visible = true;
            this.ReHome_Btn.Text = "Refresh Device";
            this.Status_LBL.Text = "An Error has occurred in the selected device. \n Error Message: " + 
                myHandler.myDevice.GetErrorMessage(myHandler.myDevice.ErrorState);
            Application.DoEvents();
        }

        private void UpdateDisplay_NoFW()
        {
            this.AttDev_CB.DataSource = myHandler.AttachedDeviceList;
            this.AttDev_CB.Refresh();
            this.AttDev_CB.Text = "None";
            this.AttDev_CB.Enabled = false;
            this.SettingsBTN.Enabled = false;

            this.CurrWheName_LBL.Enabled = false;
            this.CurrentWheelID_LBL.Enabled = false;
            this.CurrentWheelID_LBL.Text = "--";
            this.CurrWheName_LBL.Text = "--";
            this.CurrentFilter_CB.Items.Clear();
            this.CurrentFilter_CB.Text = "--";
            this.CurrentFilter_CB.Enabled = false;
            this.ReHome_Btn.Visible = false;
            this.Status_LBL.Text = "No Devices Found";
            Application.DoEvents();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Dispose();
            this.Close();
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

        private void ReHome_Btn_Click(object sender, EventArgs e)
        {
            myHandler.myDevice.ClearErrorState();
            myHandler.myDevice.HomeDevice();
            UpdateDisplay();
        }

        private void SetupDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            HSFW_Handler.DeleteInstance();
            
        }

        private void NewVersionCheckerBGW_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Check For A newer verison of the driver
                if (NewVersionChecker.CheckLatestVerisonNumber(NewVersionChecker.ProductType.HSFW_ASCOM_Driver))
                {
                    //Found a VersionNumber, now check if it's newer
                    Assembly asm = Assembly.GetExecutingAssembly();
                    AssemblyName asmName = asm.GetName();
                    NewVersionChecker.CompareToLatestVersion(asmName.Version);
                    if (NewVersionChecker.NewerVersionAvailable)
                    {
                        NewVersionFrm nvf = new NewVersionFrm(asmName.Version.ToString(),
                            NewVersionChecker.NewerVersionNumber, NewVersionChecker.NewerVersionURL);
                        nvf.ShowDialog();
                    }
                }
            }
            catch { } // Just ignore all errors. They mean the computer isn't connected to internet.
        }


        private void AttDev_CB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string newSN = AttDev_CB.Text;
            myHandler.ChangeMyDevice(newSN);
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {

        }

       

        

    }
}