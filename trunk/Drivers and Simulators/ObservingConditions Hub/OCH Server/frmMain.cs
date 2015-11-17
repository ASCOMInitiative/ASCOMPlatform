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
                try // Something went wrong so try and log the issue, if we can't then don't do anything
                {
                    Hub.TL.LogMessageCrLf("RefreshTimer", "Exception: " + ex.ToString());
                    WaitFor(500); // Wait for a short while so we don't flood the log with exception messages
                }
                catch { }
            }
        }

        private void WaitFor(double duration)
        {
            DateTime startTime = DateTime.Now; // Save the wait's start time
            do
            {
                System.Threading.Thread.Sleep(20); // Have a short sleep
                Application.DoEvents(); // Keep the UI alive
            } while (DateTime.Now.Subtract(startTime).TotalMilliseconds < duration); // Wait until the durationhas elapsed
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
            try // Something went wrong so try and log the issue, if we can't then don't do anything
            {
                Hub.TL.LogMessageCrLf("Application", "Thread Exception: " + e.Exception.ToString());
                WaitFor(500); // Wait for a short while so we don't flood the log with exception messages
            }
            catch { }
        }
    }
}