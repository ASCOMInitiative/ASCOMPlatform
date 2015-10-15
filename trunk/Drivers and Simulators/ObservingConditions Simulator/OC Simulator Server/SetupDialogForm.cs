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
        // Variables
        private Util util;
        private TraceLoggerPlus TL;

        // make this static so the SensorView controls can use it
        internal static List<KeyValuePair> allDevices = new List<KeyValuePair>();

        #region Initialiser and form load
        public SetupDialogForm()
        {
            try
            {
                InitializeComponent();

                util = new Util();
                TL = OCSimulator.TL;

                numAveragePeriod.ValueChanged += numAveragePeriod_ValueChanged;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "SetupDialogForm");
            }
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            // Initialise current values of user settings from the ASCOM Profile 
            chkTrace.Checked = OCSimulator.TraceState;
            chkDebugTrace.Checked = OCSimulator.DebugTraceState;
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

        #endregion

        #region Event handlers

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            OCSimulator.TraceState = chkTrace.Checked;
            OCSimulator.DebugTraceState = chkDebugTrace.Checked;
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

        private void numAveragePeriod_ValueChanged(object sender, EventArgs e)
        {
            EnableNumberOfReadingsToAverage();
        }

        /// <summary>
        /// Event handler for the Trace checkbox when the checked state changes
        /// </summary>
        /// <param name="sender">Trace Checkbox</param>
        /// <param name="e">Contextual information</param>
        private void chkTrace_CheckedChanged(object sender, EventArgs e)
        {
            OCSimulator.TL.LogMessage("chkTraceChanged", "chkTrace: {0}, chkDebugTrace: {1}", chkTrace.Checked, chkDebugTrace.Checked);
            CheckBox chkBox = (CheckBox)sender;
            if (!chkBox.Checked & chkDebugTrace.Checked)
            {
                OCSimulator.TL.LogMessage("chkTraceChanged", "Disabling chkDebugTrace");
                chkDebugTrace.Checked = false; // Trace has been turned off so turn off debug trace as well
            }
            chkDebugTrace.Enabled = chkTrace.Checked; // Enable or disable the debug trace box as required
            OCSimulator.TL.LogMessage("chkTraceChanged", "chkTrace.Enabled: {0}, chkTrace.Checked: {1}, chkDebugTrace.Enabled: {2}, chkDebugTrace.Checked: {3}", chkTrace.Enabled, chkTrace.Checked, chkDebugTrace.Enabled, chkDebugTrace.Checked);
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

        #endregion

        #region Support code

        /// <summary>
        /// Enable or disable the number of readings to average control depending on whether or not we are averaging
        /// </summary>
        private void EnableNumberOfReadingsToAverage()
        {
            numNumberOfReadingsToAverage.Enabled = numAveragePeriod.Value > 0.0M;
        }

        #endregion

    }
}