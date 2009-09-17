using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ASCOM.FilterWheelSim
{
    public partial class frmTraffic : Form
    {
      
        private const int LOGLENGTH = 2000;
        private static bool m_bDisable;
        public TextBoxTraceListener TraceLogger;

        
        public frmTraffic()
        {
            System.EventArgs e = System.EventArgs.Empty;

            InitializeComponent();

            ToolTip aTooltip = new ToolTip();
            aTooltip.SetToolTip(picASCOM, "Visit the ASCOM website");

            btnClear_Click(this, e);
            m_bDisable = false;
            btnDisable.Text = "Disable Logging";

            TraceLogger = new TextBoxTraceListener(this.txtTraffic);
            Trace.Listeners.Add(TraceLogger);

        }

#region Event Handlers

        private void btnDisable_Click(object sender, EventArgs e)
        {
            m_bDisable = !m_bDisable;
            btnDisable.Text = m_bDisable ? "Enable Logging" : "Disable Logging";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
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

        private void txtTraffic_TextChanged(object sender, EventArgs e)
        {
            // Limit the length of the logged text
            try { txtTraffic.Text.Substring(txtTraffic.Text.Length - LOGLENGTH); }
            catch { }
                txtTraffic.SelectionStart = txtTraffic.Text.Length;
                txtTraffic.ScrollToCaret();
        }

        private void frmTraffic_Shown(object sender, EventArgs e)
        {
            btnClear_Click(this, e);
        }

#endregion


#region TextBox TraceListener
        // Code by Adam Crawford, MIT License

        public class TextBoxTraceListener : TraceListener
        {
            private System.Windows.Forms.TextBox _target;

            private StringSendDelegate _invokeWrite;

            public TextBoxTraceListener(System.Windows.Forms.TextBox target)
            {
                _target = target;
                _invokeWrite = new StringSendDelegate(SendString);
            }

            public override void Write(string message)
            {
                if (!m_bDisable)
                    _target.Invoke(_invokeWrite, new object[] { message });
            }

            public override void WriteLine(string message)
            {
                if (!m_bDisable)
                    _target.Invoke(_invokeWrite, new object[] { message + Environment.NewLine });
            }

            private delegate void StringSendDelegate(string message);

            private void SendString(string message)
            {
                if (!m_bDisable)
                    _target.Text += message;
            }
        }
 #endregion

 
    }

}
