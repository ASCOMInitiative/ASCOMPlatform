using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace ASCOM.Simulator
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);

        private System.Timers.Timer refreshTimer = new System.Timers.Timer(1000);

        public frmMain()
        {
            InitializeComponent();

            refreshTimer.Elapsed += refreshTimer_Elapsed;
            refreshTimer.Enabled = true;

        }


        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblNumberOfConnections.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblNumberOfConnections.Text = text;
            }
        }

        void refreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                SetText("Number of connections: " + Hub.ConnectionCount.ToString() + ", Objects: " + Server.ObjectsCount + ", ServerLocks: " + Server.ServerLockCount + ", Started by COM: " + Server.StartedByCOM);
            }
            catch (Exception ex)
            {
                MessageBox.Show("RefreshTimer: " + ex.ToString());
            }
        }

        private void btnShutDown_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private delegate void SetControlPropertyThreadSafeDelegate(
            Control control,
            string propertyName,
            object propertyValue);

        public static void SetControlPropertyThreadSafe(
            Control control,
            string propertyName,
            object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate
                (SetControlPropertyThreadSafe),
                new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(
                    propertyName,
                    BindingFlags.SetProperty,
                    null,
                    control,
                    new object[] { propertyValue });
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            Application.ThreadException += Application_ThreadException;
        }

        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            MessageBox.Show("Application Thread Exception: " + e.Exception.ToString());
        }
    }
}