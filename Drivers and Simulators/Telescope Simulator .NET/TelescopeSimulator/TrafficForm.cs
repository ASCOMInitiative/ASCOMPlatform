using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.TelescopeSimulator
{
    public partial class TrafficForm : Form
    {
        private bool m_Disable = false;

        public TrafficForm()
        {
            InitializeComponent();
        }

        private void TrafficForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SharedResources.TrafficForm = null;

        }


        private void ButtonDisable_Click(object sender, EventArgs e)
        {
            m_Disable = !m_Disable;
             
            if (m_Disable) 
            {
                ButtonDisable.Text ="Enable";
            } else {
                ButtonDisable.Text ="Disable";
            }
        }
        public void TrafficLine(string Message)
        {
            if (m_Disable) { return; }
            txtTraffic.Text += Message + Environment.NewLine;
        }
        public void TrafficStart(string Message)
        {
            if (m_Disable) { return; }
            txtTraffic.Text += Message;
        }
        public void TrafficEnd(string Message)
        {
            if (m_Disable) { return; }
            txtTraffic.Text += Message + Environment.NewLine;
        }

        public bool Capabilities
        {
            get { return checkBoxCapabilities.Checked; }
        }
        public bool Other
        { get { return checkBoxAllOther.Checked; } }
    }
}
