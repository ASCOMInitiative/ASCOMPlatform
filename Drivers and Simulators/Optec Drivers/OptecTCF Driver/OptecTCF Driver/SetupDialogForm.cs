using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows;

namespace ASCOM.OptecTCF_Driver
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
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

        private void chooseCOMPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            COMPortForm CPFrm = new COMPortForm();
            CPFrm.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 AboxForm = new AboutBox1();
            AboxForm.ShowDialog();
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            try
            {
                DeviceComm.Connect();
                UpdateControls();

            }
            catch
            {
                UpdateControls(); ;
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceComm.Connect();
                UpdateControls();
            }
            catch(Exception Ex)
            {
                UpdateControls();
                DialogResult Result;
                Result = MessageBox.Show("Could not connect to device.\n" + 
                            "This may result from not selecting the correct COM port.\n" + 
                            "Would you like to see the exception data?", 
                            "Connection Failed" ,MessageBoxButtons.YesNo);
                if (Result == DialogResult.Yes)
                    MessageBox.Show("Error Message: \n" + Ex.ToString());
                else
                {
                    //don't display anything....
                }
            }
        }
        internal void UpdateControls()
        {
            if (DeviceComm.GetConnectionState())
            {
                StatusLabel.Text = "Connected successfully!";
                connectToolStripMenuItem.Enabled = false;
                disconnectToolStripMenuItem.Enabled = true;
            }
            else
            {
                StatusLabel.Text = "Device is not connected";
                connectToolStripMenuItem.Enabled = true;
                disconnectToolStripMenuItem.Enabled = false;
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceComm.Disconnect();
            UpdateControls();
        }

        private void SetupDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DeviceComm.Disconnect();
        }
    }
}