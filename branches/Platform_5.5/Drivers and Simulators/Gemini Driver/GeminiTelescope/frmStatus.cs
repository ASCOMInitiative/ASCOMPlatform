using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.GeminiTelescope
{
    public partial class frmStatus : Form
    {
        Timer tmrUpdate = new Timer();


        private bool m_AutoHide = true;

        public bool AutoHide
        {
            get { return m_AutoHide; }
            set { 
                m_AutoHide = value;
                if (m_AutoHide) pbPin.ImageIndex = 0;
                else pbPin.ImageIndex = 1;
            }
        }        

        private DateTime m_previousActive = DateTime.Now;

        public frmStatus()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        internal void ShowMe(object sender, EventArgs e)
        {            
            if (!this.Visible)
            {
                geminiPropertiesBindingSource.ResetBindings(false);
                this.Show();
                SharedResources.SetTopWindow(this);
            }
            else
            {
                if (DateTime.Now - m_previousActive < TimeSpan.FromSeconds(1)) return;  // don't respond more than once a second!
                Win32API.SetForegroundWindow(this.Handle);
            }

            tmrUpdate.Stop();
            tmrUpdate.Interval = 2000;
            tmrUpdate.Start();
            m_previousActive = DateTime.Now;
        }

        internal void ShutDown(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                tmrUpdate.Stop();
                this.Close();
                this.Dispose();
            }
            catch { }
        }


        private void frmStatus_Load(object sender, EventArgs e)
        {
            try
            {
                this.geminiPropertiesBindingSource.Add(new GeminiProperties());
                tmrUpdate.Interval = 1500;
                tmrUpdate.Tick += new EventHandler(tmrUpdate_Tick);
                tmrUpdate.Start();
                SharedResources.SetTopWindow(this);
                this.Move += new System.EventHandler(this.frmStatus_Move);
            }
            catch { }
        }

        void tmrUpdate_Tick(object sender, EventArgs e)
        {
            tmrUpdate.Stop();
            geminiPropertiesBindingSource.ResetBindings(false);
            if (AutoHide)
            {
                if (Cursor.Position.X < this.Left || Cursor.Position.Y < this.Top)
                {
                    tmrUpdate.Stop();
                    this.Hide();
                    return;
                }
            }
            tmrUpdate.Start();
        }

        private void geminiPropertiesBindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
        }

        private void frmStatus_Move(object sender, EventArgs e)
        {
            AutoHide = false;   //once the user moves this, it will no longer go away automatically
        }

        private void frmStatus_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible) tmrUpdate.Stop();
            else
                tmrUpdate.Start();
        }

        private void frmStatus_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmrUpdate.Stop();
            GeminiHardware.Instance.Profile = null;
        }

        private void frmStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmrUpdate.Stop();
            this.Hide();
            e.Cancel = true;
        }

        private void pbPin_Click(object sender, EventArgs e)
        {

        }

        private void pbPin_MouseDown(object sender, MouseEventArgs e)
        {
            AutoHide = !AutoHide;
        }

    }
}
