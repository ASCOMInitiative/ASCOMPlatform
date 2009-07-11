using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.FilterWheelSim
{
    public partial class frmTraffic : Form
    {
      
        private const int LOGLENGTH = 2000;
        private bool m_bDisable;
        private bool m_bBOL;                    // tracks if we are at the beginning of a line

        
        public frmTraffic()
        {
            System.EventArgs e = System.EventArgs.Empty;

            InitializeComponent();

            ToolTip aTooltip = new ToolTip();
            aTooltip.SetToolTip(picASCOM, "Visit the ASCOM website");

            btnClear_Click(this, e);
            m_bDisable = false;
            btnDisable.Text = "Disable";

        }

#region Event Handlers

        private void btnDisable_Click(object sender, EventArgs e)
        {
            m_bDisable = !m_bDisable;
            btnDisable.Text = m_bDisable ? "Enable" : "Disable";

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            m_bBOL = true;
            txtTraffic.Text = "";
        }

        private void picASCOM_Click(object sender, EventArgs e)
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

#endregion

#region Properties and Methods

        public void TrafficChar(string val)
        {
            if (m_bDisable) return;

            if (m_bBOL)
            {
                m_bBOL = false;
                txtTraffic.Text = txtTraffic.Text + val;
                try { txtTraffic.Text.Substring(txtTraffic.Text.Length - LOGLENGTH); }
                catch { }
            }
            else
            {
                txtTraffic.Text = txtTraffic.Text + " " + val;
                try { txtTraffic.Text.Substring(txtTraffic.Text.Length - LOGLENGTH); }
                catch { }
            }
            txtTraffic.SelectionStart = txtTraffic.Text.Length;
            txtTraffic.ScrollToCaret();
        }

        public void TrafficLine(string val)
        {
            if (m_bDisable) return;

            if (m_bBOL)
                val = val + "\r\n";
            else
                val = "\r\n" + val + "\r\n";

            m_bBOL = true;

            txtTraffic.Text = txtTraffic.Text + val;
            try { txtTraffic.Text.Substring(txtTraffic.Text.Length - LOGLENGTH); }
            catch { }
            txtTraffic.SelectionStart = txtTraffic.Text.Length;
            txtTraffic.ScrollToCaret();
        }

        public void TrafficStart(string val)
        {
            if (m_bDisable) return;

            if (!m_bBOL) val = "\r\n" + val;

            m_bBOL = false;

            txtTraffic.Text = txtTraffic.Text + val;
            try { txtTraffic.Text.Substring(txtTraffic.Text.Length - LOGLENGTH); }
            catch { }
            txtTraffic.SelectionStart = txtTraffic.Text.Length;
            txtTraffic.ScrollToCaret();
        }

        public void TrafficEnd(string val)
        {
            if (m_bDisable) return;

            val = val + "\r\n";

            m_bBOL = true;

            txtTraffic.Text = txtTraffic.Text + val;
            try { txtTraffic.Text.Substring(txtTraffic.Text.Length - LOGLENGTH); }
            catch { }
            txtTraffic.SelectionStart = txtTraffic.Text.Length;
            txtTraffic.ScrollToCaret();
        }

#endregion
    }

}
