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
        ASCOM.Utilities.Serial ASerialPort;

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


            bool PortsFound = GetAvailablePorts();
        }
        private bool GetAvailablePorts()
        {  
            string[] PortList = ASerialPort.AvailableCOMPorts;
            if (PortList.Length < 1)    //if no ports are found...
            {
                MessageBox.Show("No COM Ports were found available \n" +
                    "Do you need to plug in a serial adapter? \n");
                return false;
            }
            else
            {
                foreach (string PortName in PortList)
                {
                    COMPort_CB.Items.Add(PortName);
                }
                return true;
                }
        }
        private void OK_Btn_Click(object sender, EventArgs e)
        {
            try
            {

                if (COMPort_CB.Text.Length > 2)
                {
                    //Properties.Defaults.Default.COMPort = short.Parse(COMPort_CB.Text.Substring(3));
                    //Properties.Defaults.Default.Save();
                }
                else
                {
                    //Properties.Defaults.Default.COMPort = 0;
                    //Properties.Defaults.Default.Save();
                }
                this.Close();
            }
            catch (NullReferenceException)
            {
                //Properties.Defaults.Default.COMPort = 0;
                //Properties.Defaults.Default.Save();
                //this.Close();
            }
        }
        private void COMPort_CB_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

    }
}
