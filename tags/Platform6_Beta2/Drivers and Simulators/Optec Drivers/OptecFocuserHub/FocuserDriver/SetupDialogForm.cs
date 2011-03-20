using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.OptecFocuserHubTools;
using System.IO.Ports;

namespace ASCOM.OptecFocuserHub
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
            if (EthernetRB.Checked)
            {
                SharedResources.SharedFocuserManager.ConnectionMethod = ConnectionMethods.WiredEthernet;
                SharedResources.SharedFocuserManager.IPAddress = IPAddrTB.Text;
                SharedResources.SharedFocuserManager.TCPIPPort = TcpipPortNumberTB.Text;
            }
            else if(SerialRB.Checked)
            {
                SharedResources.SharedFocuserManager.ConnectionMethod = ConnectionMethods.Serial;
                SharedResources.SharedFocuserManager.COMPortName = ComPortNameCB.Text;
            }
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
            if (SharedResources.SharedFocuserManager.ConnectionMethod == ConnectionMethods.Serial)
                SerialRB.Checked = true;
            else if (SharedResources.SharedFocuserManager.ConnectionMethod == ConnectionMethods.WiredEthernet)
                EthernetRB.Checked = true;
        }

        private void SerialRB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                ComPortNameCB.Enabled = true;
                ComPortNameCB.Items.Clear();
                ComPortNameCB.Items.AddRange(SerialPort.GetPortNames());
                for (int i = 0; i < ComPortNameCB.Items.Count; i++)
                {
                    if (ComPortNameCB.Items[i].ToString() == SharedResources.SharedFocuserManager.COMPortName)
                    {
                        ComPortNameCB.SelectedIndex = i;
                        break;
                    }
                }

                IPAddrTB.Enabled = false;
                TcpipPortNumberTB.Enabled = false;
            }
        }

        private void EthernetRB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                IPAddrTB.Enabled = true;
                IPAddrTB.Text = SharedResources.SharedFocuserManager.IPAddress;
                TcpipPortNumberTB.Enabled = true;
                TcpipPortNumberTB.Text = SharedResources.SharedFocuserManager.TCPIPPort;

                ComPortNameCB.Enabled = false;              
            }
        }

    }

    

}