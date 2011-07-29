using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;


namespace ASCOM.Optec
{
    public partial class COMPortForm : Form
    {
        
        public COMPortForm()
        {
            InitializeComponent();
        }
        private void COMPortForm_Load(object sender, EventArgs e)
        {
            bool PortsFound = GetAvailablePorts();
        }
        private bool GetAvailablePorts()
        {
            SerialPort tempPort;
            bool available = false;
            foreach (string CPName in SerialPort.GetPortNames())
            {
                try
                {
                    tempPort = new SerialPort(CPName);
                    tempPort.Open();
                    if (tempPort.IsOpen)
                    {
                        COMPort_CB.Items.Add(CPName);
                        tempPort.Close();
                        available = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No COM Ports were found available \n" + 
                        "Do you need to plug in a serial adapter? \n" + ex.ToString());
                    available = false;
                }
            }
            return available;
        }
        private void OK_Btn_Click(object sender, EventArgs e)
        {
            try
            {

                if (COMPort_CB.Text.Length > 2)
                {
                    Properties.Defaults.Default.COMPort = short.Parse(COMPort_CB.Text.Substring(3));
                    Properties.Defaults.Default.Save();
                }
                else
                {
                    Properties.Defaults.Default.COMPort = 0;
                    Properties.Defaults.Default.Save();
                }
                this.Close();
            }
            catch (NullReferenceException)
            {
                Properties.Defaults.Default.COMPort = 0;
                Properties.Defaults.Default.Save();
                this.Close();
            }
        }
        private void COMPort_CB_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

    }
}
