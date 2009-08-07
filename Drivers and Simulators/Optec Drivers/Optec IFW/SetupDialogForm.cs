using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.Optec_IFW
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private static object CommLock = new object();
        public SetupDialogForm()
        {
            InitializeComponent();
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

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            lock (CommLock)
            {
                DeviceComm.ConnectToDevice();
            }
        }

        private void DissConnBtn_Click(object sender, EventArgs e)
        {
            lock (CommLock)
            {
                DeviceComm.DisconnectDevice();
            }
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            lock (CommLock)
            {
                DeviceComm.HomeDevice();
            }
            this.WheelId_TB.Text = DeviceComm.WheelID.ToString();
        }

        private void ReadNames_Btn_Click(object sender, EventArgs e)
        {
            lock (CommLock)
            {
               // this.FilterNames_TB.Text = DeviceComm.ReadAllNames();
            }
            
        }

        private void CheckConn_Btn_Click(object sender, EventArgs e)
        {
            lock (CommLock)
            {
                if (DeviceComm.CheckForConnection()) this.ConnStatus_TB.Text = "Yes";
                else this.ConnStatus_TB.Text = "NO";
            }

        }

        private void GoTo_Btn_Click(object sender, EventArgs e)
        {
            lock (CommLock)
            {
                DeviceComm.GoToPosition(Int32.Parse(this.GoToPos_CB.Text));
            }
        }

      

    }
}