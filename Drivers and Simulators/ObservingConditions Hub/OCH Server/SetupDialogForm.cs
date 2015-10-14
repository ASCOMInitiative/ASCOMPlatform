using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

        #region Initialiser and Form load

        public SetupDialogForm()
        {
            try
            {
                InitializeComponent();

                numAveragePeriod.ValueChanged += NumAveragePeriod_ValueChanged;
                numNumberOfReadingsToAverage.ValueChanged += NumNumberOfReadingsToAverage_ValueChanged;

                util = new Util();
                TL = Hub.TL;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "SetupDialogForm");
            }
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            // Initialise current values of user settings from the ASCOM Profile 
            chkTrace.Checked = Hub.TraceState;
            debugTrace.Checked = Hub.DebugTraceState;
            chkConnectToDrivers.Checked = Hub.ConnectToDrivers;
            chkOverrideSafetyLimits.Checked = Hub.overrideUISafetyLimits;

            numAveragePeriod.Value = Convert.ToDecimal(Hub.averagePeriod);
            EnableNumberOfReadingsToAverage();
            numNumberOfReadingsToAverage.Value = Math.Min(Convert.ToDecimal(Hub.numberOfMeasurementsPerAveragePeriod), numNumberOfReadingsToAverage.Maximum); // Set the value to the lower of the profile value or the control maximum

            ReadDeviceInformation(); // Read device information
        }

        #endregion

        private void ReadDeviceInformation()
        {
            try
            {
                TL.LogMessage("ReadDeviceInformation", "Start");
                Type type = typeof(Hub);
                Profile profile = new Profile();

                // Get the list of ObservingConditions drivers
                ArrayList observingConditionsDevices = profile.RegisteredDevices(Hub.DEVICE_TYPE);
                TL.LogMessage("ReadDeviceInformation", "Found {0} ObservingConditions devices", observingConditionsDevices.Count);

                // Get the list of Switch drivers
                ArrayList switchDevices = profile.RegisteredDevices(Hub.SWITCH_DEVICE_NAME);
                TL.LogMessage("ReadDeviceInformation", "Found {0} switch devices", switchDevices.Count);

                // Construct a complete list of all drivers
                allDevices.Clear();
                // Add the "no Device" entry as the first entry in the list of devices
                KeyValuePair noDevice = new KeyValuePair(Hub.NO_DEVICE_PROGID, Hub.NO_DEVICE_DESCRIPTION);
                allDevices.Add(noDevice);

                try
                {
                    // Add the ObservingConditions devices to the overall drivers list
                    foreach (KeyValuePair device in observingConditionsDevices)
                    {
                        if (device.Key == Hub.DRIVER_PROGID) continue;

                        device.Value = Hub.OBSERVING_CONDITIONS_NAME_PREFIX + ": " + device.Value;
                        allDevices.Add(device);
                    }

                    TL.LogMessage("ReadDeviceInformation", "Found {0} devices", allDevices.Count);
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("ReadDeviceInformation 1", "ObservingConditions description exception: {0}", ex);
                    MessageBox.Show("ReadDeviceInformatio 1: " + ex.ToString(), "ReadDeviceInformation 1");
                }

                // Add the Switch devices to the overall drivers list
                foreach (KeyValuePair device in switchDevices)
                {
                    device.Value = Hub.SWITCH_NAME_PREFIX + ": " + device.Value;
                    allDevices.Add(device);
                }

                // Log the combined list of ObservingConditions and Switch drivers 
                foreach (KeyValuePair device in allDevices)
                {
                    TL.LogMessage("ReadDeviceInformation", "Found device: \"{0}\": \"{1}\"", device.Key, device.Value);
                }

                // initialise the SensorView objects
                foreach (Control item in this.Controls.OfType<SensorView>())
                {
                    SensorView view = item as SensorView;
                    if (view != null)
                    {
                        view.ConnectToDriver = chkConnectToDrivers.Checked;
                        view.InitUI();
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("ReadDeviceInformation2", ex.ToString());
                MessageBox.Show("ReadDeviceInformation2: " + ex.ToString(), "ReadDeviceInformation2");
            }
        }

        private void EnableNumberOfReadingsToAverage()
        {
            string warningMessage = "";

            bool enableControl = numAveragePeriod.Value != 0.0M; // Enable the number of readings control and label when the average period is > 0.0
            numNumberOfReadingsToAverage.Enabled = enableControl;
            lblNumberOfReadingsToAverage.Enabled = enableControl;

            if (!chkOverrideSafetyLimits.Checked) // Safety limit is in effect so ensure that we do not go to more than 60 readings in the list
            {
                if (numAveragePeriod.Value > 0.0M) // We are averaging
                {
                    int maxReadings = (int)Math.Min(numAveragePeriod.Value * 60.0M, Hub.MAX_QUERIES_PER_PERIOD); // Get the lower of the number of 1 per second queries or 60 per interval
                    numNumberOfReadingsToAverage.Maximum = maxReadings; // Limit the maximum number of readings 
                }
                if (numNumberOfReadingsToAverage.Value >= numNumberOfReadingsToAverage.Maximum) // Correct value > maximium if required
                {
                    numNumberOfReadingsToAverage.Value = numNumberOfReadingsToAverage.Maximum;
                }

                if (numNumberOfReadingsToAverage.Value == numNumberOfReadingsToAverage.Maximum) // Now check if any warning messages should be shown
                {
                    if (numNumberOfReadingsToAverage.Maximum == Hub.MAX_QUERIES_PER_PERIOD) // We are at the 60 per second limit
                    {
                        warningMessage = "( Query rate limited to " + Hub.MAX_QUERIES_PER_PERIOD.ToString() + " per average period )";
                    }
                    else // We are at a lower limit determined by 1 per second
                    {
                        warningMessage = "( Query rate limited to 1 per second )";
                    }
                }
                else // We are not averaging so no warning messages required
                {
                }
            }
            else // No safety limit so the sky's the limit
            {
                numNumberOfReadingsToAverage.Maximum = Decimal.MaxValue; // No limit to the maximum number of readings per interval
            }

            if (numAveragePeriod.Value > 0.0M) lblWarning.Text = warningMessage; // Display the warning message, if any
            else lblWarning.Text = "";
        }

        #region Event handlers

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Check whether both dew point and humidy are either set or unset. It is not allowed for one to be available and not the other
            Hub.ConnectionType connectionTypeDewPoint = this.Controls.OfType<SensorView>().First(sv => sv.Name == "sensorView" + Hub.PROPERTY_DEWPOINT).SelectedSensor.DeviceMode;
            Hub.ConnectionType connectionTypeHumidity = this.Controls.OfType<SensorView>().First(sv => sv.Name == "sensorView" + Hub.PROPERTY_HUMIDITY).SelectedSensor.DeviceMode;

            if (((connectionTypeDewPoint == Hub.ConnectionType.None) & (connectionTypeHumidity == Hub.ConnectionType.Real)) || ((connectionTypeDewPoint == Hub.ConnectionType.Real) & (connectionTypeHumidity == Hub.ConnectionType.None)))
            {
                // We have one of dew point or humidity set to a device and the other is set to "No Device" - this violates the ASCOM spec so flash a warning
                MessageBox.Show("Dew point and Humidity must both be implemented or both must be not implemented. The ASCOM specification does not allow one to be implemented and the other not.\r\n\r\nPlease ensure that the configured Dew point and Humidity implementations match.", "DewPoint and Humnidty Issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else // Both humidity and dew point are either no device or real device
            {
                // Save the UI control states
                Hub.TraceState = chkTrace.Checked;
                Hub.DebugTraceState = debugTrace.Checked;
                Hub.ConnectToDrivers = chkConnectToDrivers.Checked;
                Hub.averagePeriod = Convert.ToDouble(numAveragePeriod.Value);
                Hub.numberOfMeasurementsPerAveragePeriod = Convert.ToInt32(numNumberOfReadingsToAverage.Value);
                Hub.overrideUISafetyLimits = chkOverrideSafetyLimits.Checked;

                // Save updated sensor control values to Hub.Sensors 
                foreach (Control item in this.Controls.OfType<SensorView>())
                {
                    SensorView view = item as SensorView;
                    if (view != null)
                    {
                        var sensorname = view.SensorName;
                        Hub.Sensors[sensorname].ProgID = view.SelectedSensor.ProgID;
                        Hub.Sensors[sensorname].SwitchNumber = view.SelectedSensor.SwitchNumber;
                        Hub.Sensors[sensorname].DeviceMode = view.SelectedSensor.DeviceMode;
                    }
                }

                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void NumNumberOfReadingsToAverage_ValueChanged(object sender, EventArgs e)
        {
            string warningMessage = "";

            // Set background colour whenver the value changes to give a warning if the user selects a large number of readings that wil slow the hub down when calaculating average values
            if ((numNumberOfReadingsToAverage.Value <= 30.0M) || (chkOverrideSafetyLimits.Checked)) // SHow normal black on white text if we are safe or we have warnings turned off
            {
                numNumberOfReadingsToAverage.ForeColor = Color.Black;
                numNumberOfReadingsToAverage.BackColor = Color.White;
            }
            else if ((numNumberOfReadingsToAverage.Value > 30.0M) & (numNumberOfReadingsToAverage.Value <= 60.0M))
            {
                numNumberOfReadingsToAverage.ForeColor = Color.Black;
                numNumberOfReadingsToAverage.BackColor = Color.Yellow;
            }
            else
            {
                numNumberOfReadingsToAverage.ForeColor = Color.White;
                numNumberOfReadingsToAverage.BackColor = Color.Red;
            }

            if (!chkOverrideSafetyLimits.Checked)// Safety limit is in effect so colour contrl as appropriate and ensure that the selected rate is between 1 and the maximum permitted value
            {
                if (numNumberOfReadingsToAverage.Value == numNumberOfReadingsToAverage.Minimum) warningMessage = "( Query rate must be at least 1 per average period )";
                if (numNumberOfReadingsToAverage.Value == numNumberOfReadingsToAverage.Maximum) // We are at the maximum rate so workj out which message to show
                {
                    if (numNumberOfReadingsToAverage.Maximum == Hub.MAX_QUERIES_PER_PERIOD) // We are at the 60 per second limit
                    {
                        warningMessage = "( Query rate limited to " + Hub.MAX_QUERIES_PER_PERIOD.ToString() + " per average period )";
                    }
                    else // We are at a lower limit determined by 1 per second
                    {
                        warningMessage = "( Query rate limited to 1 per second )";
                    }
                }

                if (numAveragePeriod.Value > 0.0M) lblWarning.Text = warningMessage; // Display the warning message, if any
                else lblWarning.Text = "";
            }
        }

        private void NumAveragePeriod_ValueChanged(object sender, EventArgs e)
        {
            EnableNumberOfReadingsToAverage();
        }

        private void chkConnectToDrivers_CheckedChanged(object sender, EventArgs e)
        {
            TL.LogMessage("ConnectToDrivers", "Event fired");
            ReadDeviceInformation();
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

        /// <summary>
        /// Event handler to paint the average period combo box in the "DropDown" rather than "DropDownList" style
        /// </summary>
        /// <param name="sender">Device to be painted</param>
        /// <param name="e">Draw event arguments object</param>
        void DropDownListComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox combo = sender as ComboBox;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) // Draw the selected item in menu highlight colour
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.MenuHighlight), e.Bounds);
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(SystemColors.HighlightText), new Point(e.Bounds.X, e.Bounds.Y));
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window), e.Bounds);
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(combo.ForeColor), new Point(e.Bounds.X, e.Bounds.Y));
            }

            e.DrawFocusRectangle();
        }

        #endregion

        private void chkOverrideSafetyLimits_CheckedChanged(object sender, EventArgs e)
        {
            EnableNumberOfReadingsToAverage();
            NumNumberOfReadingsToAverage_ValueChanged(new object(), new EventArgs());
        }
    }
}