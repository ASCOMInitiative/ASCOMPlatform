using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.GeminiTelescope
{
    public partial class frmGps : Form
    {
        public frmGps()
        {
            InitializeComponent();

            comboBoxComPort.Items.Add("");
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxComPort.Items.Add(s);
            }
        }

        private void frmGps_Load(object sender, EventArgs e)
        {

        }

        public double Latitude
        {
            get
            {
                double lat = 0;
                try
                {
                    
                }
                catch { }
                return lat;
            }
        }
        public double Longitude
        {
            get
            {
                double log = 0;
                try
                {

                }
                catch { }
                return log;
            }
        }
    }
}
