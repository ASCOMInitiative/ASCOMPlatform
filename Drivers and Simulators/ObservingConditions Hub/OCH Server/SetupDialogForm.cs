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
        //private const string UPDOWN_PREFIX = "upDown";
        private const string CMB_PREFIX = "cmb";
        private const string SENSORVIEW_PREFIX = "sensorView";

        private const string NO_DEVICE_DESCRIPTION = "No device";
        private const string LOW_SIMULATOR_PREFIX = "txtLow";
        private const string HIGH_SIMULATOR_PREFIX = "txtHigh";

        // Variables
        private Util util;
        private TraceLoggerPlus TL;

        // make this static so the SensorView controls can use it
        internal static List<KeyValuePair> allDevices = new List<KeyValuePair>();

        //private Dictionary<string, List<string>> sensorDescriptions = new Dictionary<string, List<string>>();
        //private Dictionary<string, short> switchMax = new Dictionary<string, short>();

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

                // Add event handlers for the UI components
                //cmbAveragePeriod.SelectedIndexChanged += CmbAveragePeriod_SelectedIndexChanged;
                //cmbCloudCover.SelectedIndexChanged += CmbCloudCover_SelectedIndexChanged;
                //cmbDewPoint.SelectedIndexChanged += CmbDewPoint_SelectedIndexChanged;
                //cmbHumidity.SelectedIndexChanged += CmbHumidity_SelectedIndexChanged;
                //cmbPressure.SelectedIndexChanged += CmbPressure_SelectedIndexChanged;
                //cmbRainRate.SelectedIndexChanged += CmbRainRate_SelectedIndexChanged;
                //cmbSkyBrightness.SelectedIndexChanged += CmbSkyBrightness_SelectedIndexChanged;
                //cmbSkyQuality.SelectedIndexChanged += CmbSkyQuality_SelectedIndexChanged;
                //cmbSkySeeing.SelectedIndexChanged += CmbSkySeeing_SelectedIndexChanged;
                //cmbSkyTemperature.SelectedIndexChanged += CmbSkyTemperature_SelectedIndexChanged;
                //cmbTemperature.SelectedIndexChanged += CmbTemperature_SelectedIndexChanged;
                //cmbWindDirection.SelectedIndexChanged += CmbWindDirection_SelectedIndexChanged;
                //cmbWindGust.SelectedIndexChanged += CmbWindGust_SelectedIndexChanged;
                //cmbWindSpeed.SelectedIndexChanged += CmbWindSpeed_SelectedIndexChanged;

                //cmbAveragePeriodSwitch.SelectedIndexChanged += CmbAveragePeriodSwitch_SelectedIndexChanged;
                //cmbCloudCoverSwitch.SelectedIndexChanged += CmbCloudCoverSwitch_SelectedIndexChanged;
                //cmbDewPointSwitch.SelectedIndexChanged += CmbDewPointSwitch_SelectedIndexChanged;
                //cmbHumiditySwitch.SelectedIndexChanged += CmbHumiditySwitch_SelectedIndexChanged;
                //cmbPressureSwitch.SelectedIndexChanged += CmbPressureSwitch_SelectedIndexChanged;
                //cmbRainRateSwitch.SelectedIndexChanged += CmbRainRateSwitch_SelectedIndexChanged;
                //cmbSkyBrightnessSwitch.SelectedIndexChanged += CmbSkyBrightnessSwitch_SelectedIndexChanged;
                //cmbSkyQualitySwitch.SelectedIndexChanged += CmbSkyQualitySwitch_SelectedIndexChanged;
                //cmbSkySeeingSwitch.SelectedIndexChanged += CmbSkySeeingSwitch_SelectedIndexChanged;
                //cmbSkyTemperatureSwitch.SelectedIndexChanged += CmbSkyTemperatureSwitch_SelectedIndexChanged;
                //cmbTemperatureSwitch.SelectedIndexChanged += CmbTemperatureSwitch_SelectedIndexChanged;
                //cmbWindDirectionSwitch.SelectedIndexChanged += CmbWindDirectionSwitch_SelectedIndexChanged;
                //cmbWindGustSwitch.SelectedIndexChanged += CmbWindGustSwitch_SelectedIndexChanged;
                //cmbWindSpeedSwitch.SelectedIndexChanged += CmbWindSpeedSwitch_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //private void CmbWindSpeed_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        //private void CmbWindGust_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        //private void CmbWindDirection_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        //private void CmbTemperature_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        //private void CmbSkyTemperature_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        //private void CmbSkySeeing_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        //private void CmbSkyQuality_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        //private void CmbSkyBrightness_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        //private void CmbRainRate_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        //private void CmbPressure_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        //private void CmbHumidity_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        //private void CmbDewPoint_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        //private void CmbCloudCover_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }
        //private void CmbAveragePeriod_SelectedIndexChanged(object sender, EventArgs e) { EnableUpDown(sender); }

        //private void EnableUpDown(object sender)
        //{
        //    TL.LogMessage("EnableUpDown", "Event has fired");
        //    ComboBox cmb = (ComboBox)sender;
        //    if (cmdCancel == null)
        //        return;

        //    NumericUpDown upDown = (NumericUpDown)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[UPDOWN_PREFIX + cmb.Name.Substring(3)];
        //    ComboBox cmbSwitch = null;
        //    try { cmbSwitch = (ComboBox)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[cmb.Name + "Switch"]; }
        //    catch { }

        //    if (allDevices[cmb.SelectedIndex].Key.EndsWith("." + Hub.SWITCH_DEVICE_NAME, StringComparison.InvariantCultureIgnoreCase)) // Enable or disable the property's Switch number spin control 
        //    {

        //        if (cmbSwitch != null)
        //        {
        //            if (upDown != null)
        //                upDown.Enabled = true;

        //            using (var s = new ASCOM.DriverAccess.Switch(allDevices[cmb.SelectedIndex].Key))
        //            {
        //                try
        //                {
        //                    s.Connected = true;
        //                    var max = s.MaxSwitch;

        //                    if (upDown != null)
        //                        upDown.Maximum = max;

        //                    cmbSwitch.Items.Clear();
        //                    for (short i = 0; i < max; i++)
        //                    {
        //                        var str = string.Format("{0}: {1}", i, s.GetSwitchName(i));
        //                        cmbSwitch.Items.Add(str);
        //                    }
        //                    cmbSwitch.SelectedIndex = Hub.Sensors[cmb.Name.Substring(3)].SwitchNumber;
        //                    cmbSwitch.Enabled = true;
        //                }
        //                catch
        //                {

        //                    if (upDown != null)
        //                        upDown.Maximum = 100;
        //                    cmbSwitch.Enabled = false;
        //                }
        //                finally
        //                {
        //                    if (upDown != null)
        //                        upDown.Value = Hub.Sensors[cmb.Name.Substring(3)].SwitchNumber;
        //                    s.Connected = false;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (upDown != null)
        //        {
        //            upDown.Value = 0;
        //            upDown.Enabled = false;
        //        }
        //        if (cmbSwitch != null)
        //            cmbSwitch.Enabled = false;
        //    }

        //    switch (allDevices[cmb.SelectedIndex].Key)  // Set the device mode flag
        //    {
        //        case Hub.NO_DEVICE_PROGID: // No device
        //            {
        //                Hub.Sensors[cmb.Name.Substring(3)].DeviceMode = Hub.ConnectionType.None;
        //                Hub.Sensors[cmb.Name.Substring(3)].ProgID = Hub.NO_DEVICE_PROGID;
        //            }
        //            break;
        //        case Hub.DRIVER_PROGID: // Simulate the device
        //            {
        //                Hub.Sensors[cmb.Name.Substring(3)].DeviceMode = Hub.ConnectionType.Simulation;
        //                Hub.Sensors[cmb.Name.Substring(3)].ProgID = Hub.DRIVER_PROGID;
        //                break;
        //            }
        //        default: // Real driver
        //            {
        //                Hub.Sensors[cmb.Name.Substring(3)].DeviceMode = Hub.ConnectionType.Real;
        //                Hub.Sensors[cmb.Name.Substring(3)].ProgID = allDevices[cmb.SelectedIndex].Key;
        //                break;
        //            }
        //    }
        //}

        //private void CmbAveragePeriodSwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }
        //private void CmbCloudCoverSwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }
        //private void CmbDewPointSwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }
        //private void CmbHumiditySwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }
        //private void CmbPressureSwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }
        //private void CmbRainRateSwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }
        //private void CmbSkyBrightnessSwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }
        //private void CmbSkyQualitySwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }
        //private void CmbSkySeeingSwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }
        //private void CmbSkyTemperatureSwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }
        //private void CmbTemperatureSwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }
        //private void CmbWindDirectionSwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }
        //private void CmbWindGustSwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }
        //private void CmbWindSpeedSwitch_SelectedIndexChanged(object sender, EventArgs e) { SynchSwitchNumber(sender); }

        /// <summary>
        /// This event handler sets the switch updown spin control the the correct value based on the the switch's description
        /// The event is fired whenever a new sensor description is selected
        /// This event can only be fired when the user has elected to connect and read information from devices
        /// </summary>
        /// <param name="sender">The description dropdown combo box where the user has selected a new value</param>
        //private void SynchSwitchNumber(object sender)
        //{
        //    try
        //    {
        //        // Get a shorthand variable that refers to the control the user changed and create the name of the switch updown control that needs to be synced 
        //        ComboBox cmbSensorDescription = (ComboBox)sender;
        //        string switchUpDownControlName = UPDOWN_PREFIX + cmbSensorDescription.Name.Substring(CMB_PREFIX.Length, cmbSensorDescription.Name.Length - CMB_PREFIX.Length - Hub.SWITCH_DEVICE_NAME.Length);
        //        TL.LogMessage("SynchSwitchNumber", "Event has fired, accessing control: " + cmbSensorDescription.Name + " " + cmbSensorDescription.Name.Substring(CMB_PREFIX.Length) + " " + switchUpDownControlName);

        //        // Set the switch updown spin control if required
        //        if (cmbSensorDescription.Name.ToLower().Contains(Hub.AVERAGE_PERIOD.ToLower())) // Special handling for AveragePeriod that is only supported by ObservingConditions devices
        //        {
        //            // Average period is only available in ObservingConditions devices so this description never needs to be synced to a switch number
        //            // No action required
        //        }
        //        else // Normal processing for all other properties
        //        {
        //            // Create a shorthand variable that refers to the updown control and set its new value
        //            NumericUpDown num = (NumericUpDown)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[switchUpDownControlName];
        //            num.Value = cmbSensorDescription.SelectedIndex;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TL.LogMessageCrLf("SynchSwitchDescription", ex.ToString());
        //        MessageBox.Show("SynchSwitchDescription exception: " + ex.ToString());
        //    }
        //}

        /// <summary>
        /// This event sets the switch updown spin control or sensor description to the correct value based on the selected device
        /// The event is fired whenever a new source device is selected
        /// </summary>
        /// <param name="sender"></param>
        //private void NewDeviceSelected(object sender)
        //{
        //    try
        //    {
        //        // Get shorthand variables that refer to the control the user changed and the controls that need to be synced 
        //        ComboBox cmbDevice = (ComboBox)sender;
        //        NumericUpDown upDownSwitchNumber = (NumericUpDown)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[UPDOWN_PREFIX + cmbDevice.Name.Substring(3)];
        //        ComboBox cmbSensorDescription = (ComboBox)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[cmbDevice.Name + Hub.SWITCH_DEVICE_NAME];

        //        // Set the device mode flag depending on whether the user has selected no device, a simulated device or a real device

        //        // Set the new switch updown control value and sensor description
        //        if (cmbDevice.Name.Substring(3).ToLower() == Hub.AVERAGE_PERIOD.ToLower()) // Special handling for AveragePeriod that is only supported by ObservingConditions devices
        //        {
        //            TL.LogMessage("NewDeviceSelected", "Event has fired for AveragePeriod: " + cmbDevice.Name);
        //            // Create subsets of the device and description lists that only contain ObservingConditions devices 
        //            List<KeyValuePair> observingConditionsDevices = new List<KeyValuePair>();
        //            // Add the "no Device" entry as the first entry in the list of devices
        //            KeyValuePair noDevice = new KeyValuePair(Hub.NO_DEVICE_PROGID, NO_DEVICE_DESCRIPTION);
        //            observingConditionsDevices.Add(noDevice);
        //            observingConditionsDevices.AddRange(allDevices.Where(kvp => kvp.Key.EndsWith(Hub.DEVICE_TYPE, StringComparison.InvariantCultureIgnoreCase)).ToList());
        //            TL.LogMessage("NewDeviceSelected", "Found " + observingConditionsDevices.Count + " ObservingConditions devices");

        //            Dictionary<string, List<string>> observingConditionsDevicesDescriptions = new Dictionary<string, List<string>>();
        //            observingConditionsDevicesDescriptions.Add(Hub.NO_DEVICE_PROGID, new List<string>() { NO_DEVICE_DESCRIPTION });
        //            observingConditionsDevicesDescriptions.AddRange(sensorDescriptions.Where(kvp => kvp.Key.EndsWith(Hub.DEVICE_TYPE, StringComparison.InvariantCultureIgnoreCase)).ToDictionary(p => p.Key, p => p.Value));
        //            TL.LogMessage("NewDeviceSelected", "Found " + observingConditionsDevicesDescriptions.Count + " ObservingConditions descriptions");

        //            int index = cmbAveragePeriod.SelectedIndex <= 0 ? 0 : cmbAveragePeriod.SelectedIndex;
        //            TL.LogMessage("NewDeviceSelected", "Selected index: " + index);

        //            switch (observingConditionsDevices[index].Key)
        //            {
        //                case Hub.NO_DEVICE_PROGID: // No device
        //                    {
        //                        Hub.Sensors[cmbDevice.Name.Substring(3)].DeviceMode = Hub.ConnectionType.None;
        //                        Hub.Sensors[cmbDevice.Name.Substring(3)].ProgID = Hub.NO_DEVICE_PROGID;
        //                    }
        //                    break;
        //                case Hub.DRIVER_PROGID: // Simulate the device
        //                    {
        //                        Hub.Sensors[cmbDevice.Name.Substring(3)].DeviceMode = Hub.ConnectionType.Simulation;
        //                        Hub.Sensors[cmbDevice.Name.Substring(3)].ProgID = Hub.DRIVER_PROGID;
        //                        break;
        //                    }
        //                default: // Real driver
        //                    {
        //                        Hub.Sensors[cmbDevice.Name.Substring(3)].DeviceMode = Hub.ConnectionType.Real;
        //                        Hub.Sensors[cmbDevice.Name.Substring(3)].ProgID = observingConditionsDevices[index].Key;
        //                        break;
        //                    }
        //            }


        //            if (chkConnectToDrivers.Checked) // If we are connecting to drivers to get descriptions then populate the descriptions drop down 
        //            {
        //                cmbSensorDescription.Items.Clear();
        //                cmbSensorDescription.Enabled = true;
        //                cmbSensorDescription.Items.Add(observingConditionsDevicesDescriptions[observingConditionsDevices[cmbDevice.SelectedIndex].Key][0]); // -1 because there is no "No device" device in the filtered list
        //                cmbSensorDescription.SelectedIndex = 0;
        //            }
        //            else // We are not connecting to devices so have no sensor descriptions to populate
        //            {
        //                cmbSensorDescription.Items.Clear();
        //                cmbSensorDescription.Enabled = false;
        //            }
        //        }
        //        else // Process properties other than AveragePeriod
        //        {
        //            TL.LogMessage("NewDeviceSelected", "Event has fired for a property other than AveragePeriod: " + cmbDevice.Name);
        //            switch (allDevices[cmbDevice.SelectedIndex].Key)
        //            {
        //                case Hub.NO_DEVICE_PROGID: // No device
        //                    {
        //                        Hub.Sensors[cmbDevice.Name.Substring(3)].DeviceMode = Hub.ConnectionType.None;
        //                        Hub.Sensors[cmbDevice.Name.Substring(3)].ProgID = Hub.NO_DEVICE_PROGID;
        //                    }
        //                    break;
        //                case Hub.DRIVER_PROGID: // Simulate the device
        //                    {
        //                        Hub.Sensors[cmbDevice.Name.Substring(3)].DeviceMode = Hub.ConnectionType.Simulation;
        //                        Hub.Sensors[cmbDevice.Name.Substring(3)].ProgID = Hub.DRIVER_PROGID;
        //                        break;
        //                    }
        //                default: // Real driver
        //                    {
        //                        Hub.Sensors[cmbDevice.Name.Substring(3)].DeviceMode = Hub.ConnectionType.Real;
        //                        Hub.Sensors[cmbDevice.Name.Substring(3)].ProgID = allDevices[cmbDevice.SelectedIndex].Key;
        //                        break;
        //                    }
        //            }

        //            // Populate the property's Switch number spin control if necessary
        //            if (allDevices[cmbDevice.SelectedIndex].Key.EndsWith("." + Hub.SWITCH_DEVICE_NAME, StringComparison.InvariantCultureIgnoreCase)) // A Switch device has been selected
        //            {
        //                TL.LogMessage("NewDeviceSelected", "Switch device has been selected: " + cmbDevice.Name);
        //                if (chkConnectToDrivers.Checked) // If we are connecting to drivers to get descriptions then populate the descriptions drop down rather than use the updown spin controls
        //                {
        //                    // Set the switch number
        //                    upDownSwitchNumber.Enabled = false; // The user cannot change the updown switch spin control but it needs to show the correct value if a switch is selected to avoid confusion
        //                    int switchNumber = Hub.Sensors[cmbDevice.Name.Substring(3)].SwitchNumber;
        //                    if (switchNumber < 0) switchNumber = 0;
        //                    TL.LogMessage("NewDeviceSelected", "Setting switch spin control " + upDownSwitchNumber.Name + " value to: " + switchNumber);
        //                    upDownSwitchNumber.Value = switchNumber;

        //                    // Populate the sensor description list and display the selected sensor description
        //                    cmbSensorDescription.Enabled = true;
        //                    cmbSensorDescription.Items.Clear();
        //                    foreach (string description in sensorDescriptions[allDevices[cmbDevice.SelectedIndex].Key])
        //                    {
        //                        cmbSensorDescription.Items.Add(description);
        //                    }
        //                    TL.LogMessage("NewDeviceSelected", "Setting switch combo box selected index to: " + Hub.Sensors[cmbDevice.Name.Substring(3)].SwitchNumber);
        //                    cmbSensorDescription.SelectedIndex = Hub.Sensors[cmbDevice.Name.Substring(3)].SwitchNumber;
        //                }
        //                else // We are not connecting to drivers using the updown spin controls
        //                {
        //                    upDownSwitchNumber.Enabled = true;
        //                    cmbSensorDescription.Enabled = false;
        //                    int switchNumber = Hub.Sensors[cmbDevice.Name.Substring(3)].SwitchNumber;
        //                    if (switchNumber < 0) switchNumber = 0;
        //                    TL.LogMessage("NewDeviceSelected", "Setting switch spin control " + upDownSwitchNumber.Name + " value to: " + switchNumber);
        //                    upDownSwitchNumber.Value = switchNumber;
        //                }
        //            }
        //            else // An ObservingConditions device has been selected
        //            {
        //                TL.LogMessage("NewDeviceSelected", "ObservingConditions device has been selected: " + cmbDevice.Name);
        //                if (chkConnectToDrivers.Checked) // If we are connecting to drivers to get descriptions then populate the descriptions drop down rather than use the updown spin controls
        //                {
        //                    // Set the switch number
        //                    upDownSwitchNumber.Value = 0;
        //                    upDownSwitchNumber.Enabled = false;

        //                    // Set the sensor description
        //                    cmbSensorDescription.Enabled = true;
        //                    cmbSensorDescription.Items.Clear();
        //                    int sensorIndex = Hub.ValidProperties.IndexOf(cmbDevice.Name.Substring(3)); // Find the index of this property in the hub descriptions list
        //                    cmbSensorDescription.Items.Add(sensorDescriptions[allDevices[cmbDevice.SelectedIndex].Key][sensorIndex]);

        //                    cmbSensorDescription.SelectedIndex = 0;
        //                }
        //                else // Not conecting so use the spin controls
        //                {
        //                    // Set the switch number
        //                    upDownSwitchNumber.Enabled = true;
        //                    int switchNumber = Hub.Sensors[cmbDevice.Name.Substring(3)].SwitchNumber;
        //                    if (switchNumber < 0) switchNumber = 0;
        //                    TL.LogMessage("NewDeviceSelected", "Setting switch spin control " + upDownSwitchNumber.Name + " value to: " + switchNumber);
        //                    upDownSwitchNumber.Value = switchNumber;

        //                    // Set the sensor description
        //                    cmbSensorDescription.Items.Clear();
        //                    cmbSensorDescription.Enabled = false;
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        TL.LogMessageCrLf("NewDeviceSelected", ex.ToString());
        //        MessageBox.Show("NewDeviceSelected exception: " + ex.ToString());
        //    }
        //}

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            Hub.TraceState = chkTrace.Checked;
            Hub.DebugTraceState = debugTrace.Checked;
            Hub.ConnectToDrivers = chkConnectToDrivers.Checked;

            foreach (Control item in this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls)
            {
                SensorView view = item as SensorView;
                if (view != null)
                {
                    var sensorname = view.SensorName;
                    Hub.Sensors[sensorname].ProgID = view.SelectedSensor.ProgID;
                    Hub.Sensors[sensorname].SwitchNumber = view.SelectedSensor.SwitchNumber;

                    // update the simulator values
                    TextBox lowSimulator = (TextBox)this.Controls[tabControl1.Name].Controls[tabPage2.Name].Controls[LOW_SIMULATOR_PREFIX + sensorname];
                    TextBox highSimulator = (TextBox)this.Controls[tabControl1.Name].Controls[tabPage2.Name].Controls[HIGH_SIMULATOR_PREFIX + sensorname];
                    Hub.Sensors[sensorname].SimLowValue = Convert.ToDouble(lowSimulator.Text);
                    Hub.Sensors[sensorname].SimHighValue = Convert.ToDouble(highSimulator.Text);
                }
            }

            // update Average Period separately
            List<KeyValuePair> observingConditionsDevices = allDevices.Where(kvp => kvp.Key.EndsWith(Hub.DEVICE_TYPE, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (cmbAveragePeriod.SelectedIndex == 0)
                Hub.Sensors[Hub.AVERAGE_PERIOD].ProgID = Hub.NO_DEVICE_PROGID;
            else
            {
                int index = cmbAveragePeriod.SelectedIndex >= 1 ? cmbAveragePeriod.SelectedIndex - 1 : 0;
                Hub.Sensors[Hub.AVERAGE_PERIOD].ProgID = observingConditionsDevices[index].Key; // -1 because there is no "No device" device in the filtered list
            }


            //Type type = typeof(Hub);
            //foreach (string property in Hub.ValidProperties)
            //{
            //    TL.LogMessage("Setup OK", "Processing: \"" + property + "\"");

            //    //_MethodInfo methodInfo = type.GetMethod(property);
            //    //double retval = (double)type.InvokeMember(property, System.Reflection.BindingFlags.InvokeMethod, null, methodInfo, new object[] { 0 });
            //    try
            //    {
            //        // Get pointers to setup dialogue controls
            //        ComboBox cmb = (ComboBox)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[CMB_PREFIX + property];
            //        //if (cmb == null)
            //        //    continue;

            //        //NumericUpDown upDown = (NumericUpDown)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[UPDOWN_PREFIX + property];
            //        //ComboBox cmbSwitch = (ComboBox)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[cmb.Name + Hub.SWITCH_DEVICE_NAME];
            //        TextBox lowSimulator = (TextBox)this.Controls[tabControl1.Name].Controls[tabPage2.Name].Controls[LOW_SIMULATOR_PREFIX + property];
            //        TextBox highSimulator = (TextBox)this.Controls[tabControl1.Name].Controls[tabPage2.Name].Controls[HIGH_SIMULATOR_PREFIX + property];

            //        // Save new values to Sensors collection
            //        if (property == Hub.AVERAGE_PERIOD) // Special handling for AveragePeriod
            //        {
            //            List<KeyValuePair> observingConditionsDevices = allDevices.Where(kvp => kvp.Key.EndsWith(Hub.DEVICE_TYPE, StringComparison.InvariantCultureIgnoreCase)).ToList();
            //            int index = cmb.SelectedIndex >= 1 ? cmb.SelectedIndex - 1 : 0;
            //            Hub.Sensors[property].ProgID = observingConditionsDevices[index].Key; // -1 because there is no "No device" device in the filtered list
            //        }
            //        else // All other properties
            //        {
            //            //Hub.Sensors[property].ProgID = allDevices[cmb.SelectedIndex].Key;
            //            //Hub.Sensors[property].SwitchNumber = (int)upDown.Value;
            //            Hub.Sensors[property].SimLowValue = Convert.ToDouble(lowSimulator.Text);
            //            Hub.Sensors[property].SimHighValue = Convert.ToDouble(highSimulator.Text);
            //        }

            //        TL.LogMessage("Setup OK", "Saving new values for " + property +
            //            ": ProgId: " + Hub.Sensors[property].ProgID +
            //            ", Switch Number: " + Hub.Sensors[property].SwitchNumber +
            //            ", Simulator Low Value: " + Hub.Sensors[property].SimLowValue +
            //            ", Simulator High Value: " + Hub.Sensors[property].SimHighValue
            //            );

            //    }
            //    catch (Exception ex)
            //    {
            //        TL.LogMessageCrLf("Setup OK", property + " exception: " + ex.Message);
            //        MessageBox.Show("Setup OK exception: " + ex.ToString());
            //    }
            //}
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
            chkTrace.Checked = Hub.TraceState;
            debugTrace.Checked = Hub.DebugTraceState;
            chkConnectToDrivers.Checked = Hub.ConnectToDrivers;

            if (allDevices.Count == 0) ReadDeviceInformation(); // Read device information if not already read by the ConnectToDrivers.CheckedChanged event

            // Set up the simulator values
            foreach (string property in Hub.ValidProperties) // Iterate over all the Hub properties
            {
                TL.LogMessage("Load", "Processing: \"" + property + "\" simulator values");
                try
                {
                    foreach (KeyValuePair device in allDevices) // Iterate over all the available drivers
                    {
                        // Set values for the simulator UI controls for this ObservingConditions Hub property
                        TextBox lowSimulator = (TextBox)this.Controls[tabControl1.Name].Controls[tabPage2.Name].Controls[LOW_SIMULATOR_PREFIX + property];
                        lowSimulator.Text = Hub.Sensors[property].SimLowValue.ToString();
                        TextBox highSimulator = (TextBox)this.Controls[tabControl1.Name].Controls[tabPage2.Name].Controls[HIGH_SIMULATOR_PREFIX + property];
                        highSimulator.Text = Hub.Sensors[property].SimHighValue.ToString();
                    }
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("Setup Load", property + " exception: " + ex.ToString());
                    MessageBox.Show("Setup Load exception 1: " + ex.ToString());
                }
            }
        }

        private void ReadDeviceInformation()
        {
            try
            {
                TL.LogMessage("ReadDeviceInformation", "Start");
                Type type = typeof(Hub);
                Profile profile = new Profile();

                // Get the list of Switch drivers
                ArrayList switchDevices = profile.RegisteredDevices(Hub.SWITCH_DEVICE_NAME);
                TL.LogMessage("ReadDeviceInformation", "Found " + switchDevices.Count + "  switch devices");

                // Get the list of ObservingConditions drivers
                ArrayList observingConditionsDevices = profile.RegisteredDevices(Hub.DEVICE_TYPE);
                TL.LogMessage("ReadDeviceInformation", "Found " + observingConditionsDevices.Count + "  ObservingConditions devices");

                // Construct a complete list of all drivers
                allDevices.Clear();
                // Add the "no Device" entry as the first entry in the list of devices
                KeyValuePair noDevice = new KeyValuePair(Hub.NO_DEVICE_PROGID, NO_DEVICE_DESCRIPTION);
                allDevices.Add(noDevice);
                //sensorDescriptions.Add(Hub.NO_DEVICE_PROGID, new List<string>() { NO_DEVICE_DESCRIPTION });

                // Add the Switch devices to the overall drivers list
                foreach (KeyValuePair device in switchDevices)
                {
                    device.Value = "Switch: " + device.Value;
                    allDevices.Add(device);
                    //if (chkConnectToDrivers.Checked) // We need to connect to the switches to get their descriptions
                    //{
                    //    using (var switchDevice = new DriverAccess.Switch(device.Key))
                    //    {
                    //        switchDevice.Connected = true;
                    //        short max = switchDevice.MaxSwitch;
                    //        //switchMax.Add(device.Key, max); // Save the switch's MaxSwitch value

                    //        List<string> descriptions = new List<string>(); // Get the sensor descriptions into a list
                    //        for (short i = 0; i < max; i++)
                    //        {
                    //            string switchDescription;
                    //            if (switchDevice.InterfaceVersion == 1) // Use switch name for ISwitch V1 devices
                    //            {
                    //                switchDescription = string.Format("{0}: {1}", i, switchDevice.GetSwitchName(i));
                    //            }
                    //            else
                    //            {
                    //                switchDescription = string.Format("{0}: {1}", i, switchDevice.GetSwitchDescription(i)); // Use switch description for ISwitch V2 and newer devices
                    //            }

                    //            descriptions.Add(switchDescription); // Add the sensor description to the device list
                    //            TL.LogMessage("ReadDeviceInformation", "Found switch sensor: " + device.Key + " " + switchDescription);
                    //        }

                    //        //sensorDescriptions.Add(device.Key, descriptions); // Add the device's sensor description list to sensorDescriptions 
                    //        switchDevice.Connected = false;
                    //    }
                    //}
                }

                try
                {
                    // Add the ObservingConditions devices to the overall drivers list
                    foreach (KeyValuePair device in observingConditionsDevices)
                    {
                        device.Value = "ObservingConditions: " + device.Value;
                        allDevices.Add(device);
                        //if (chkConnectToDrivers.Checked) // We need to connect to the switches to get their descriptions
                        //{
                        //    //List<string> descriptions;
                        //    switch (device.Key)
                        //    {
                        //        case Hub.NO_DEVICE_PROGID:
                        //            TL.LogMessage("ReadDeviceInformation", "Processing NO_DEVICE: " + device.Key);
                        //            //descriptions = new List<string>(); // Get the sensor descriptions into a list
                        //            foreach (string property in Hub.ValidProperties) // Iterate over all the Hub properties
                        //            {
                        //                string sensorDescription = NO_DEVICE_DESCRIPTION;
                        //                //descriptions.Add(sensorDescription);
                        //                TL.LogMessage("ReadDeviceInformation", "Found no device sensor: " + sensorDescription);
                        //            }
                        //            //sensorDescriptions.Add(device.Key, descriptions); // Add the device's sensor description list to sensorDescriptions 
                        //            break;
                        //        case Hub.DRIVER_PROGID:
                        //            TL.LogMessage("ReadDeviceInformation", "Processing DRIVER_PROGID: " + device.Key);
                        //            //descriptions = new List<string>(); // Get the sensor descriptions into a list
                        //            foreach (string property in Hub.ValidProperties) // Iterate over all the Hub properties
                        //            {
                        //                string sensorDescription;
                        //                sensorDescription = "OCH simulated " + property + " sensor";
                        //                //descriptions.Add(sensorDescription);
                        //                TL.LogMessage("ReadDeviceInformation", "Found observing conditions sensor: " + device.Key + " " + sensorDescription);
                        //            }
                        //            //sensorDescriptions.Add(device.Key, descriptions); // Add the device's sensor description list to sensorDescriptions 
                        //            break;
                        //        default:
                        //            TL.LogMessage("ReadDeviceInformation", "Processing REAL device: " + device.Key);

                        //            using (var observingConditionsDevice = new DriverAccess.ObservingConditions(device.Key))
                        //            {
                        //                //observingConditionsDevice.Connected = true;
                        //                //descriptions = new List<string>(); // Get the sensor descriptions into a list
                        //                foreach (string property in Hub.ValidProperties) // Iterate over all the Hub properties
                        //                {
                        //                    try
                        //                    {
                        //                        string sensorDescription = observingConditionsDevice.SensorDescription(property);
                        //                        //descriptions.Add(sensorDescription);
                        //                        TL.LogMessage("ReadDeviceInformation", "Found observing conditions sensor: " + device.Key + " " + sensorDescription);
                        //                    }
                        //                    catch (Exception ex)
                        //                    {
                        //                        TL.LogMessage("ReadDeviceInformation", "Found observing conditions sensor: " + device.Key + " Error: " + ex.Message);
                        //                    }
                        //                }
                        //                //sensorDescriptions.Add(device.Key, descriptions); // Add the device's sensor description list to sensorDescriptions 
                        //                observingConditionsDevice.Connected = false;
                        //            }
                        //            break;
                        //    }
                        //}
                    }
                    TL.LogMessage("ReadDeviceInformation", "Found " + allDevices.Count + " devices");
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("ReadDeviceInformation 1", "ObservingConditions description exception: " + ex.ToString());
                    MessageBox.Show("ReadDeviceInformatio 1: " + ex.ToString());
                }

                // Log the combined list of ObservingConditions and Switch drivers 
                foreach (KeyValuePair device in allDevices)
                {
                    TL.LogMessage("ReadDeviceInformation", "Found device: : \"" + device.Key + "\" \"" + device.Value + "\"");
                }

                // initialise the SensorView objects
                foreach (Control item in this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls)
                {
                    SensorView view = item as SensorView;
                    if (view != null)
                    {
                        view.ConnectToDriver = chkConnectToDrivers.Checked;
                        view.InitUI();
                    }
                }

                // handle AveragePeriod separately, just add the OC devices
                cmbAveragePeriod.Items.Clear();
                int currentIndex = 0; // Counter to record where a particular driver will appear in the drop down list
                foreach (KeyValuePair device in allDevices) // Iterate over all the available drivers
                {
                    if (!device.Key.EndsWith(Hub.SWITCH_DEVICE_NAME))
                    {
                        TL.LogMessage("ReadDeviceInformation", "Processing: \"AveragePeriod\", Device: " + device.Key + ", Adding device: " + device.Value + ", Property ProgID: " + Hub.Sensors["AveragePeriod"].ProgID);
                        cmbAveragePeriod.Items.Add(device.Value); // Otherwise add the driver description to the available drivers combo box

                        if (device.Key == Hub.Sensors[Hub.AVERAGE_PERIOD].ProgID) // If this driver's ProgID has been selected by the user in the past and recorded in the Profile, select it in the available drivers combo list
                        {
                            cmbAveragePeriod.SelectedIndex = currentIndex; // This will fire the SelectedIndexChanged event (which calls EnableUpdDown) to set values and enable the switch number as required
                        }
                        currentIndex++;
                    }
                }

                return;

                // Populate the UI controls for each ObservingConditions Hub property with information from the relevant driver
                //foreach (string property in Hub.ValidProperties) // Iterate over all the Hub properties
                //{
                //    TL.LogMessage("ReadDeviceInformation", "Processing: \"" + property + "\"");
                //    try
                //    {
                //        // Create shortcuts to the UI controls for this ObservingConditions Hub property
                //        ComboBox cmbDriver = (ComboBox)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[CMB_PREFIX + property];
                //        if (cmbDriver == null)
                //            continue;

                //        //NumericUpDown upDown = (NumericUpDown)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[UPDOWN_PREFIX + property];
                //        //ComboBox cmbSwitch = (ComboBox)this.Controls[tabControl1.Name].Controls[tabPage1.Name].Controls[CMB_PREFIX + property + Hub.SWITCH_DEVICE_NAME];

                //        // Populate the available drivers combo box for this Hub property and select the relevant entry in the drop down list
                //        currentIndex = 0; // Counter to record where a particular driver will appear in the drop down list
                //        foreach (KeyValuePair device in allDevices) // Iterate over all the available drivers
                //        {
                //            TL.LogMessage("ReadDeviceInformation", "Processing: \"" + property + "\", Device: " + device.Key);
                //            // Special handling for AveragePeriod because this is only availble in Hub deviecs
                //            if (property == Hub.AVERAGE_PERIOD && device.Key.EndsWith(Hub.SWITCH_DEVICE_NAME))// Do nothing i.e. omit Switch devices from Averageperiod
                //            {
                //                TL.LogMessage("ReadDeviceInformation", "Processing: \"" + property + "\", Device: " + device.Key + ", Switch in Averageperiod - IGNORING IT!");
                //            }
                //            else // Add the device to the list 
                //            {
                //                TL.LogMessage("ReadDeviceInformation", "Processing: \"" + property + "\", Device: " + device.Key + ", Adding device: " + device.Value + ", Property ProgID: " + Hub.Sensors[property].ProgID);
                //                cmbDriver.Items.Add(device.Value); // Otherwise add the driver description to the available drivers combo box

                //                if (device.Key == Hub.Sensors[property].ProgID) // If this driver's ProgID has been selected by the user in the past and recorded in the Profile, select it in the available drivers combo list
                //                {
                //                    cmbDriver.SelectedIndex = currentIndex; // This will fire the SelectedIndexChanged event (which calls EnableUpdDown) to set values and enable the switch number as required
                //                }
                //                currentIndex++;
                //            }
                //        }

                //        // Enable or disable the combo boxes that list information read from devices by connecting to them depending on whether the user has configured the Hub to Connect to drivers while loading this form
                //        //if (chkConnectToDrivers.Checked) // We do need to connect to each driver to retrieve its sensor descriptions and, for Switches, its list of switch devices
                //        //{
                //        //    if (upDown != null)
                //        //        upDown.Enabled = false;
                //        //    if (cmbSwitch != null)
                //        //        cmbSwitch.Enabled = true;

                //        //}
                //        //else // No need to connect so enable the switch number selection numeric updown controls and disable the sensor description combo boxes
                //        //{
                //        //    if (upDown != null)
                //        //        upDown.Enabled = true;
                //        //    if (cmbSwitch != null)
                //        //        cmbSwitch.Enabled = false;
                //        //}
                //    }
                //    catch (Exception ex)
                //    {
                //        TL.LogMessageCrLf("ReadDeviceInformation1", property + " exception: " + ex.ToString());
                //        MessageBox.Show("ReadDeviceInformation 1: " + ex.ToString());
                //    }
                //}
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("ReadDeviceInformation2", ex.ToString());
                MessageBox.Show("ReadDeviceInformation2: " + ex.ToString());
            }

        }

        private void chkConnectToDrivers_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox connectToDrivers = (CheckBox)sender;
            TL.LogMessage("ConnectToDrivers", "Event fired");
            //allDevices.Clear();
            //sensorDescriptions.Clear();
            //switchMax.Clear();
            //cmbAveragePeriod.Items.Clear();

            //foreach (string property in Hub.ValidProperties)
            //{
                //cmbCloudCover.Items.Clear();
                //cmbDewPoint.Items.Clear();
                //cmbHumidity.Items.Clear();
                //cmbPressure.Items.Clear();
                //cmbRainRate.Items.Clear();
                //cmbSkyBrightness.Items.Clear();
                //cmbSkyQuality.Items.Clear();
                //cmbSkySeeing.Items.Clear();
                //cmbSkyTemperature.Items.Clear();
                //cmbTemperature.Items.Clear();
                //cmbWindDirection.Items.Clear();
                //cmbWindGust.Items.Clear();
                //cmbWindSpeed.Items.Clear();

                //cmbAveragePeriodSwitch.Items.Clear();
                //cmbCloudCoverSwitch.Items.Clear();
                //cmbDewPointSwitch.Items.Clear();
                //cmbHumiditySwitch.Items.Clear();
                //cmbPressureSwitch.Items.Clear();
                //cmbRainRateSwitch.Items.Clear();
                //cmbSkyBrightnessSwitch.Items.Clear();
                //cmbSkyQualitySwitch.Items.Clear();
                //cmbSkySeeingSwitch.Items.Clear();
                //cmbSkyTemperatureSwitch.Items.Clear();
                //cmbTemperatureSwitch.Items.Clear();
                //cmbWindDirectionSwitch.Items.Clear();
                //cmbWindGustSwitch.Items.Clear();
                //cmbWindSpeedSwitch.Items.Clear();

                //upDownAveragePeriod.Value = 0;
                //upDownCloudCover.Value = 0;
                //upDownDewPoint.Value = 0;
                //upDownHumidity.Value = 0;
                //upDownPressure.Value = 0;
                //upDownRainRate.Value = 0;
                //upDownSkyBrightness.Value = 0;
                //upDownSkyQuality.Value = 0;
                //upDownSkySeeing.Value = 0;
                //upDownSkyTemperature.Value = 0;
                //upDownTemperature.Value = 0;
                //upDownWindDirection.Value = 0;
                //upDownWindGust.Value = 0;
                //upDownWindSpeed.Value = 0;
            //}

            ReadDeviceInformation();
        }


    }
}