using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Globalization;

namespace ASCOM.Simulator
{
    public partial class frmMain : Form
    {
        delegate void SetTextCallback(string text);

        private System.Timers.Timer refreshTimer = new System.Timers.Timer(1000);

        #region Initialiser and form load

        public frmMain()
        {
            InitializeComponent();
            Application.ThreadException += Application_ThreadException;
            refreshTimer.Elapsed += refreshTimer_Elapsed;
            refreshTimer.Enabled = true;
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

            DeactivateEnableButton(); // Deactivate the enable button        
        }

        #endregion

        #region Event handlers

        void refreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                SetControlPropertyThreadSafe(lblNumberOfConnections, "Text", "Number of connections: " + OCSimulator.ConnectionCount.ToString() + ", Objects: " + Server.ObjectsCount + ", ServerLocks: " + Server.ServerLockCount + ", Started by COM: " + Server.StartedByCOM);
            }
            catch (Exception ex)
            {
                try // Something went wrong so try and log the issue, if we can't then don't do anything
                {
                    OCSimulator.TL.LogMessageCrLf("RefreshTimer", "Exception: " + ex.ToString());
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

        private void btnEnable_Click(object sender, EventArgs e)
        {
            Button btnEnable = (Button)sender;
            // Get the control values into variables in the Sensors array
            foreach (string property in OCSimulator.SimulatedProperties)
            {
                OverrideView over = (OverrideView)Controls.Find("overrideView" + property, false)[0];
                over.SaveUI(property);
            }

            // Provide an overridden dew point calculation
            OCSimulator.Sensors[OCSimulator.PROPERTY_DEWPOINT].OverrideValue = OCSimulator.util.Humidity2DewPoint(OCSimulator.Humidity(0), OCSimulator.Temperature(0));

            OCSimulator.MinimiseOnStart = chkMinimise.Checked;

            // Write the Sensors array values to the Profile
            OCSimulator.WriteProfile();

            DeactivateEnableButton(); // Deactivate the enable button        
        }
        #endregion

        #region Support code

        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);

        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue },CultureInfo.InvariantCulture);
            }
        }

        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try // Something went wrong so try and log the issue, if we can't then don't do anything
            {
                OCSimulator.TL.LogMessageCrLf("Application", "Thread Exception: " + e.Exception.ToString());
                WaitFor(500); // Wait for a short while so we don't flood the log with exception messages
            }
            catch { }
        }

        private void DeactivateEnableButton()
        {
            btnEnable.Enabled = false; // Set the enabled button to false ready to be enabed by the next change
            btnEnable.ForeColor = Color.Black;
        }

        #endregion

    }
}