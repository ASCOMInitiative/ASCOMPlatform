using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using System.Windows.Forms;
using ASCOM.Utilities;

namespace ASCOM.Simulator
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        // Constants

        // Variables
        private Util util;
        private TraceLoggerPlus TL;

        // make this static so the SensorView controls can use it
        internal static List<KeyValuePair> allDevices = new List<KeyValuePair>();

        public SetupDialogForm()
        {
            try
            {
                InitializeComponent();

                util = new Util();
                TL = OCSimulator.TL;
                // Initialise current values of user settings from the ASCOM Profile 
                chkTrace.Checked = OCSimulator.TraceState;
                debugTrace.Checked = OCSimulator.DebugTraceState;

                // Initialise sensorview items here

                foreach (string sensor in OCSimulator.ValidProperties)
                {
                SensorView sv = (SensorView)this.Controls["sensorView"+ sensor];
                    sv.Units = "Peter's units!";
                    sv.LowValue = OCSimulator.Sensors[sensor].SimLowValue;
                    sv.MaxValue = OCSimulator.Sensors[sensor].SimHighValue;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"SetupDialogForm");
            }
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            OCSimulator.TraceState = chkTrace.Checked;
            OCSimulator.DebugTraceState = debugTrace.Checked;

   
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
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

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            // Initialise current values of user settings from the ASCOM Profile 
            chkTrace.Checked = OCSimulator.TraceState;
            debugTrace.Checked = OCSimulator.DebugTraceState;

        }

    }
}