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

        void refreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                SetControlPropertyThreadSafe(lblNumberOfConnections, "Text", "Number of connections: " + OCSimulator.ConnectionCount.ToString() + ", Objects: " + Server.ObjectsCount + ", ServerLocks: " + Server.ServerLockCount + ", Started by COM: " + Server.StartedByCOM);
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

        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);

        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Minimise or bring the window into view as required
            if (OCSimulator.MinimiseOnStart) this.WindowState = FormWindowState.Minimized;
            else this.WindowState = FormWindowState.Normal;
            chkMinimise.Checked = OCSimulator.MinimiseOnStart;

            foreach (string property in OCSimulator.SimulatedProperties)
            {
                OverrideView over = (OverrideView)Controls.Find("overrideView" + property, false)[0];
                over.InitUI(property);
            }
            Application.ThreadException += Application_ThreadException;
        }

        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            MessageBox.Show("Application Thread Exception: " + e.Exception.ToString());
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            // Get the control values into variables in the Sensors array
            foreach (string property in OCSimulator.SimulatedProperties)
            {
                OverrideView over = (OverrideView)Controls.Find("overrideView" + property, false)[0];
                over.SaveUI(property);
            }

            OCSimulator.MinimiseOnStart = chkMinimise.Checked;

            // Write the Sensors array values to the Profile
            OCSimulator.WriteProfile();
        }
    }
}