using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Windows.Forms;
using ASCOM.Utilities;

namespace ASCOM.Simulator
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        // Constants
        private const string UPDOWN_PREFIX = "upDown";
        private const string CMB_PREFIX = "cmb";
        private const string NO_DEVICE_DESCRIPTION = "No device";
        private const string LOW_SIMULATOR_PREFIX = "txtLow";
        private const string HIGH_SIMULATOR_PREFIX = "txtHigh";

        // Variables
        private Util util;
        private TraceLoggerPlus TL;

        private List<KeyValuePair> allDevices = new List<KeyValuePair>();
        public SetupDialogForm()
        {
            try
            {
                InitializeComponent();

                util = new Util();
                TL = Hub.TL;
                // Initialise current values of user settings from the ASCOM Profile 
                chkTrace.Checked = Hub.TraceState;
                debugTrace.Checked = Hub.DebugTraceState;

                cmbAveragePeriod.SelectedIndexChanged += CmbAveragePeriod_SelectedIndexChanged;
                cmbCloudCover.SelectedIndexChanged += CmbCloudCover_SelectedIndexChanged;
                cmbDewPoint.SelectedIndexChanged += CmbDewPoint_SelectedIndexChanged;
                cmbHumidity.SelectedIndexChanged += CmbHumidity_SelectedIndexChanged;
                cmbPressure.SelectedIndexChanged += CmbPressure_SelectedIndexChanged;
                cmbRainRate.SelectedIndexChanged += CmbRainRate_SelectedIndexChanged;
                cmbSkyBrightness.SelectedIndexChanged += CmbSkyBrightness_SelectedIndexChanged;
                cmbSkyQuality.SelectedIndexChanged += CmbSkyQuality_SelectedIndexChanged;
                cmbSkySeeing.SelectedIndexChanged += CmbSkySeeing_SelectedIndexChanged;
                cmbSkyTemperature.SelectedIndexChanged += CmbSkyTemperature_SelectedIndexChanged;
                cmbTemperature.SelectedIndexChanged += CmbTemperature_SelectedIndexChanged;
                cmbWindDirection.SelectedIndexChanged += CmbWindDirection_SelectedIndexChanged;
                cmbWindGust.SelectedIndexChanged += CmbWindGust_SelectedIndexChanged;
                cmbWindSpeed.SelectedIndexChanged += CmbWindSpeed_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CmbWindSpeed_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        private void CmbWindGust_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        private void CmbWindDirection_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        private void CmbTemperature_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        private void CmbSkyTemperature_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        private void CmbSkySeeing_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        private void CmbSkyQuality_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        private void CmbSkyBrightness_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        private void CmbRainRate_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        private void CmbPressure_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        private void CmbHumidity_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        private void CmbDewPoint_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        private void CmbCloudCover_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        private void CmbAveragePeriod_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }

        private void EnableUpDown(object sender)
        {
            TL.LogMessage("EnableUpDown", "Event has fired");
            ComboBox cmb = (ComboBox)sender;
            NumericUpDown upDown = (NumericUpDown)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[UPDOWN_PREFIX + cmb.Name.Substring(3)];

            if (allDevices[cmb.SelectedIndex].Key.EndsWith("." + Hub.SWITCH_DEVICE_NAME, StringComparison.InvariantCultureIgnoreCase)) // Enable or disable the property's Switch number spin control 
            {
                upDown.Enabled = true;
                upDown.Value = Hub.Sensors[cmb.Name.Substring(3)].SwitchNumber;
            }
            else
            {
                upDown.Value = 0;
                upDown.Enabled = false;
            }

            switch (allDevices[cmb.SelectedIndex].Key)  // Set the device mode flag
            {
                case Hub.NO_DEVICE_PROGID: // No device
                    {
                        Hub.Sensors[cmb.Name.Substring(3)].DeviceMode = Hub.ConnectionType.None;
                        Hub.Sensors[cmb.Name.Substring(3)].ProgID = Hub.NO_DEVICE_PROGID;
                    }
                    break;
                case Hub.DRIVER_PROGID: // Simulate the device
                    {
                        Hub.Sensors[cmb.Name.Substring(3)].DeviceMode = Hub.ConnectionType.Simulation;
                        Hub.Sensors[cmb.Name.Substring(3)].ProgID = Hub.DRIVER_PROGID;
                        break;
                    }
                default: // Real driver
                    {
                        Hub.Sensors[cmb.Name.Substring(3)].DeviceMode = Hub.ConnectionType.Real;
                        Hub.Sensors[cmb.Name.Substring(3)].ProgID = allDevices[cmb.SelectedIndex].Key;
                        break;
                    }
            }
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            Hub.TraceState = chkTrace.Checked;
            Hub.DebugTraceState = debugTrace.Checked;

            Type type = typeof(Hub);
            foreach (string property in Hub.ValidProperties)
            {
                TL.LogMessage("Setup OK", "Processing: \"" + property + "\"");

                //_MethodInfo methodInfo = type.GetMethod(property);
                //double retval = (double)type.InvokeMember(property, System.Reflection.BindingFlags.InvokeMethod, null, methodInfo, new object[] { 0 });
                try
                {
                    // Get pointers to setup dialogue controls
                    ComboBox cmb = (ComboBox)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[CMB_PREFIX + property];
                    NumericUpDown upDown = (NumericUpDown)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[UPDOWN_PREFIX + property];
                    TextBox lowSimulator = (TextBox)this.Controls[tabControl1.Name].Controls[tabPage2.Name].Controls[LOW_SIMULATOR_PREFIX + property];
                    TextBox highSimulator = (TextBox)this.Controls[tabControl1.Name].Controls[tabPage2.Name].Controls[HIGH_SIMULATOR_PREFIX + property];

                    // Save new values to Sensors collection
                    Hub.Sensors[property].ProgID = allDevices[cmb.SelectedIndex].Key;
                    Hub.Sensors[property].SwitchNumber = (int)upDown.Value;
                    Hub.Sensors[property].SimLowValue = Convert.ToDouble(lowSimulator.Text);
                    Hub.Sensors[property].SimHighValue = Convert.ToDouble(highSimulator.Text);
                    TL.LogMessage("Setup OK", "Saving new values for " + property +
                        ": ProgId: " + Hub.Sensors[property].ProgID +
                        ", Switch Number: " + Hub.Sensors[property].SwitchNumber +
                        ", Simulator Low Value: " + Hub.Sensors[property].SimLowValue +
                        ", Simulator High Value: " + Hub.Sensors[property].SimHighValue
                        );

                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("Setup OK", property + " exception: " + ex.Message);
                    MessageBox.Show("Setup OK exception: " + ex.ToString());

                }
            }
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
            try
            {
                Type type = typeof(Hub);

                Profile profile = new Profile();
                ArrayList switchDevices = profile.RegisteredDevices(Hub.SWITCH_DEVICE_NAME);
                TL.LogMessage("Switch", "Found " + switchDevices.Count + "  switch devices");

                ArrayList observingConditionsDevices = profile.RegisteredDevices(Hub.OBSERVINGCONDITIONS_DEVICE_NAME);
                TL.LogMessage("OC", "Found " + observingConditionsDevices.Count + "  ObservingConditions devices");

                KeyValuePair noDevice = new KeyValuePair(Hub.NO_DEVICE_PROGID, NO_DEVICE_DESCRIPTION);
                allDevices.Add(noDevice);

                foreach (KeyValuePair device in switchDevices)
                {
                    device.Value = "Switch: " + device.Value;
                    allDevices.Add(device);
                }
                foreach (KeyValuePair device in observingConditionsDevices)
                {
                    device.Value = "ObservingConditions: " + device.Value;
                    allDevices.Add(device);
                }
                TL.LogMessage("AllDevices", "Found " + allDevices.Count + " devices");

                foreach (KeyValuePair device in allDevices)
                {
                    TL.LogMessage("Setup Load", "Found device: : \"" + device.Key + "\" \"" + device.Value + "\"");
                }

                foreach (string property in Hub.ValidProperties)
                {
                    //object value = this.GetType().GetProperty(property).GetValue(this, null);

                    TL.LogMessage("Setup Load", "Processing: \"" + property + "\"");

                    //_MethodInfo methodInfo = type.GetMethod(property);
                    //double retval = (double)type.InvokeMember(property, System.Reflection.BindingFlags.InvokeMethod, null, methodInfo, new object[] { 0 });
                    //TL.LogMessage("Setup Load", property + " = " + retval.ToString());
                    try
                    {

                        // Set values for the device tab controls
                        ComboBox cmb = (ComboBox)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[CMB_PREFIX + property];
                        NumericUpDown upDown = (NumericUpDown)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[UPDOWN_PREFIX + property];

                        // Select the reelvant entry in the combo box drop down list
                        int count = 0;
                        foreach (KeyValuePair device in allDevices)
                        {
                            //TL.LogMessage("Setup Load", "Found device: : \"" + device.Value + "\", Count: " + count);
                            cmb.Items.Add(device.Value);
                            //TL.LogMessage("Setup Load", "ProgID comparison: : " + device.Key + " " + Hub.Sensors[property].ProgID);
                            if (device.Key == Hub.Sensors[property].ProgID)
                            {
                                TL.LogMessage("Setup Load", "Before setting index to: : " + count);
                                cmb.SelectedIndex = count; // This will fire the SelectedIndexChanged event (which calls EnableUpdDown) to set values and enable the switch number as required
                                TL.LogMessage("Setup Load", "After setting index to: : " + count);
                            }
                            count++;
                        }

                        // Set values for the simulator tab controls
                        TextBox lowSimulator = (TextBox)this.Controls[tabControl1.Name].Controls[tabPage2.Name].Controls[LOW_SIMULATOR_PREFIX + property];
                        lowSimulator.Text = Hub.Sensors[property].SimLowValue.ToString();
                        TextBox highSimulator = (TextBox)this.Controls[tabControl1.Name].Controls[tabPage2.Name].Controls[HIGH_SIMULATOR_PREFIX + property];
                        highSimulator.Text = Hub.Sensors[property].SimHighValue.ToString();
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf("Setup Load", property + " exception: " + ex.Message);
                        MessageBox.Show("Setup Load exception 1: " + ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("Setup Load", ex.ToString());
                MessageBox.Show("Setup Load exception 2: " + ex.ToString());
            }
        }

    }
}