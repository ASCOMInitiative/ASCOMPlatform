using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using ASCOM.Utilities;

namespace ASCOM.OptecTCF_Driver
{
    public partial class COMPortForm : Form
    {
        public string LastComPort;

        ASCOM.Utilities.Serial ASerialPort;
        ASCOM.Utilities.TraceLogger TLogger;
        const string TL = "COM Port Form";

        public COMPortForm()
        {
            InitializeComponent();
        }

        private void COMPortForm_Load(object sender, EventArgs e)
        {
            #region Set the serial port properties...
            ASerialPort = new ASCOM.Utilities.Serial();
            ASerialPort.DataBits = 8;
            ASerialPort.Parity = SerialParity.None;
            ASerialPort.StopBits = SerialStopBits.None;
            ASerialPort.Speed = SerialSpeed.ps19200;
            #endregion

            TLogger = new TraceLogger("", "Focuser");
            TLogger.Enabled = true;
            TLogger.LogMessage(TL, "Test Log");
            bool PortsFound = GetAvailablePorts();
            
            // TODO: add consequences here for if ports are not found...
            
        }
        private bool GetAvailablePorts()
        {  

            string[] PortList = ASerialPort.AvailableCOMPorts;
            
            if (PortList.Length < 1)    //if no ports are found...
            {
                MessageBox.Show("No COM Ports were found available \n" +
                    "Do you need to plug in a serial adapter? \n");
                TLogger.LogMessage(TL, "No usable COM ports found...");
                return false;
            }
            else
            {
                TLogger.LogStart(TL, "Found usable COM ports: ");
                foreach (string PortName in PortList)
                {
                    COMPort_CB.Items.Add(PortName);
                    TLogger.LogContinue(PortName + " ");

                }
                TLogger.LogFinish(".");

                LastComPort = DeviceSettings.GetComPort();
                foreach(string x in COMPort_CB.Items)
                {
                    if (x == LastComPort)
                    {
                        COMPort_CB.SelectedIndex = COMPort_CB.Items.IndexOf(x);
                    }
                }
                return true;
            }
        }
        private void OK_Btn_Click(object sender, EventArgs e)
        {     
            if (COMPort_CB.Text.Length > 2)
            {
                DeviceSettings.SetCOMPort(COMPort_CB.Text);

            }
            else
            {
                DeviceSettings.SetCOMPort("");
            }
            this.Close();   
        }
        private void COMPort_CB_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

    }
}
