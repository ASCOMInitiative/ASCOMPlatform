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

                numAveragePeriod.ValueChanged += NumAveragePeriod_ValueChanged;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "SetupDialogForm");
            }
        }

        private void NumAveragePeriod_ValueChanged(object sender, EventArgs e)
        {
            EnableNumberOfReadingsToAverage();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            OCSimulator.TraceState = chkTrace.Checked;
            OCSimulator.DebugTraceState = debugTrace.Checked;
            OCSimulator.SensorQueryInterval = Convert.ToDouble(numSensorQueryInterval.Value);
            OCSimulator.AveragePeriod = Convert.ToDouble(numAveragePeriod.Value);
            OCSimulator.NumberOfReadingsToAverage = Convert.ToInt32(numNumberOfReadingsToAverage.Value);

            foreach (string sensor in OCSimulator.SimulatedProperties)
            {
                SensorView sv = (SensorView)this.Controls[OCSimulator.SENSORVIEW_CONTROL_PREFIX + sensor];
                OCSimulator.Sensors[sensor].SimFromValue = sv.MinValue;
                OCSimulator.Sensors[sensor].SimToValue = sv.MaxValue;
                OCSimulator.Sensors[sensor].IsImplemented = sv.SensorEnabled;
                OCSimulator.Sensors[sensor].NotReadyDelay = sv.NotReadyDelay;
                OCSimulator.Sensors[sensor].ValueCycleTime = sv.ValueCycleTime;
            }

            OCSimulator.Sensors[OCSimulator.PROPERTY_DEWPOINT].SimFromValue = OCSimulator.Sensors[OCSimulator.PROPERTY_HUMIDITY].SimFromValue;
            OCSimulator.Sensors[OCSimulator.PROPERTY_DEWPOINT].SimToValue = OCSimulator.Sensors[OCSimulator.PROPERTY_HUMIDITY].SimToValue;
            OCSimulator.Sensors[OCSimulator.PROPERTY_DEWPOINT].IsImplemented = OCSimulator.Sensors[OCSimulator.PROPERTY_HUMIDITY].IsImplemented;
            OCSimulator.Sensors[OCSimulator.PROPERTY_DEWPOINT].NotReadyDelay = OCSimulator.Sensors[OCSimulator.PROPERTY_HUMIDITY].NotReadyDelay;
            OCSimulator.Sensors[OCSimulator.PROPERTY_DEWPOINT].ValueCycleTime = OCSimulator.Sensors[OCSimulator.PROPERTY_HUMIDITY].ValueCycleTime;

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
            numSensorQueryInterval.Value = Convert.ToDecimal(OCSimulator.SensorQueryInterval);
            numAveragePeriod.Value = Convert.ToDecimal(OCSimulator.AveragePeriod);
            numNumberOfReadingsToAverage.Value = Convert.ToDecimal(OCSimulator.NumberOfReadingsToAverage);
            EnableNumberOfReadingsToAverage();
            // Initialise sensorview items here

            foreach (string sensor in OCSimulator.SimulatedProperties)
            {
                SensorView thisSensorView = (SensorView)this.Controls[OCSimulator.SENSORVIEW_CONTROL_PREFIX + sensor];
                thisSensorView.MinValue = OCSimulator.Sensors[sensor].SimFromValue;
                thisSensorView.MaxValue = OCSimulator.Sensors[sensor].SimToValue;
                thisSensorView.SensorEnabled = OCSimulator.Sensors[sensor].IsImplemented;
                thisSensorView.NotReadyControlsEnabled = OCSimulator.Sensors[sensor].IsImplemented;
                thisSensorView.NotReadyDelay = OCSimulator.Sensors[sensor].NotReadyDelay;
                thisSensorView.ValueCycleTime = OCSimulator.Sensors[sensor].ValueCycleTime;
            }

            // Initialise current values of user settings from the ASCOM Profile 

        }

        /// <summary>
        /// Enable or disable the number of readings to average control depending on whether or not we are averaging
        /// </summary>
        private void EnableNumberOfReadingsToAverage()
        {
            numNumberOfReadingsToAverage.Enabled = numAveragePeriod.Value > 0.0M;
        }
    }
}