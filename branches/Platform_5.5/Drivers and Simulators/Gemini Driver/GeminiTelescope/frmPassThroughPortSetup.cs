using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.GeminiTelescope
{
    public partial class frmPassThroughPortSetup : Form
    {
        public frmPassThroughPortSetup()
        {
            InitializeComponent();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxComPort.Items.Add(s);
            }

            ComPort = GeminiHardware.Instance.PassThroughComPort;
            BaudRate = GeminiHardware.Instance.PassThroughBaudRate.ToString();
            VirtualPortEnabled = GeminiHardware.Instance.PassThroughPortEnabled;
        }

        #region Properties for Settings
        public string ComPort
        {
            get { return comboBoxComPort.SelectedItem.ToString(); }
            set { comboBoxComPort.SelectedItem = value; }
        }

        public string BaudRate
        {
            get { return comboBoxBaudRate.SelectedItem.ToString(); }
            set { comboBoxBaudRate.SelectedItem = value; }
        }

        public bool VirtualPortEnabled
        {
            get { return chkVirtualPortEnable.Checked; }
            set { chkVirtualPortEnable.Checked = value; }
        }

        #endregion

    }
}
