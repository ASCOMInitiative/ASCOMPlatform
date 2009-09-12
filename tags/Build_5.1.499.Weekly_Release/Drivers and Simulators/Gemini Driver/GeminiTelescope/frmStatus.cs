﻿using System;
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

        public bool AutoHide = true;

        public frmStatus()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        internal void ShowMe(object sender, EventArgs e)
        {
            geminiPropertiesBindingSource.ResetBindings(false);
            this.Show();
            AutoHide = true;
            tmrUpdate.Interval = 1500;
            tmrUpdate.Start();
        }

        internal void ShutDown(object sender, EventArgs e)
        {
            this.Hide();
            tmrUpdate.Stop();
            this.Close();
            this.Dispose();
        }


        private void frmStatus_Load(object sender, EventArgs e)
        {
            this.geminiPropertiesBindingSource.Add(new GeminiProperties());
            tmrUpdate.Interval = 1500;
            tmrUpdate.Tick += new EventHandler(tmrUpdate_Tick);
            tmrUpdate.Start();
            SharedResources.SetTopWindow(this);
            this.Move += new System.EventHandler(this.frmStatus_Move);
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
        }

        private void frmStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmrUpdate.Stop();
            this.Hide();
            e.Cancel = true;
        }

    }
}
