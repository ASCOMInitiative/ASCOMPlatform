using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using PyxisLE_API;

namespace ASCOM.PyxisLE_ASCOM
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        Rotators RotatorManager;
        private string selectedSerialNumber = "0";

        public SetupDialogForm()
        {
            InitializeComponent();
            RotatorManager = new Rotators();
            RotatorManager.RotatorAttached += new EventHandler(RotatorManager_DeviceListChanged);
            RotatorManager.RotatorRemoved += new EventHandler(RotatorManager_DeviceListChanged);
        }

        void RotatorManager_DeviceListChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NoParmDel(UpdateDeviceList));
            }
            else UpdateDeviceList();
        }

        private delegate void NoParmDel();

        private void UpdateDeviceList()
        {
            this.AttachedDevices_CB.DataSource = null;
            this.AttachedDevices_CB.Items.Clear();
            this.AttachedDevices_CB.DataSource = RotatorManager.RotatorList;
            this.AttachedDevices_CB.DisplayMember = "SerialNumber";
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            RotatorManager = null;
            ASCOM.Utilities.Profile myProfile = new ASCOM.Utilities.Profile();
            myProfile.DeviceType = "Rotator";
            myProfile.WriteValue(Rotator.s_csDriverID, "SelectedSerialNumber", selectedSerialNumber);
            myProfile.Dispose();
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
            UpdateDeviceList();
            MessageBox.Show("Form is loading");
        }

        private void AttachedDevices_CB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox sndr = sender as ComboBox;
            string newSN = sndr.SelectedItem.ToString();
            if ((newSN != "0") && (newSN != ""))
            {
                selectedSerialNumber = newSN;
            }
        }
    }
}