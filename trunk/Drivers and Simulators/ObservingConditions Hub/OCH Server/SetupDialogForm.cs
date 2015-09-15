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
        private const string SIMULATOR_DESCRIPTION = "Simulate control";
        private const string LOW_SIMULATOR_PREFIX = "txtLow";
        private const string HIGH_SIMULATOR_PREFIX = "txtHigh";

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
                TL = Hub.TL;
                // Initialise current values of user settings from the ASCOM Profile 
                chkTrace.Checked = Hub.TraceState;
                debugTrace.Checked = Hub.DebugTraceState;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"SetupDialogForm");
            }
        }

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
                    Hub.Sensors[sensorname].DeviceMode = view.SelectedSensor.DeviceMode;

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
            {
                Hub.Sensors[Hub.AVERAGE_PERIOD].ProgID = Hub.NO_DEVICE_PROGID;  // no device handled separately
                Hub.Sensors[Hub.AVERAGE_PERIOD].DeviceMode = Hub.ConnectionType.None;
            }
            else if (cmbAveragePeriod.SelectedIndex > observingConditionsDevices.Count)
            {
                Hub.Sensors[Hub.AVERAGE_PERIOD].ProgID = Hub.SIMULATOR_PROGID;
                Hub.Sensors[Hub.AVERAGE_PERIOD].DeviceMode = Hub.ConnectionType.Simulation;
            }
            else
            {
                int index = cmbAveragePeriod.SelectedIndex >= 1 ? cmbAveragePeriod.SelectedIndex - 1 : 0;
                if (index < 0 || index >= observingConditionsDevices.Count)
                {
                    MessageBox.Show("Average period index out of range");
                }
                Hub.Sensors[Hub.AVERAGE_PERIOD].ProgID = observingConditionsDevices[index].Key; // -1 because there is no "No device" device in the filtered list
                Hub.Sensors[Hub.AVERAGE_PERIOD].DeviceMode = Hub.ConnectionType.Real;
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
            // Initialise current values of user settings from the ASCOM Profile 
            chkTrace.Checked = Hub.TraceState;
            debugTrace.Checked = Hub.DebugTraceState;
            chkConnectToDrivers.Checked = Hub.ConnectToDrivers;

            ReadDeviceInformation(); // Read device information

            // Set up the simulator values
            foreach (string property in Hub.ValidProperties) // Iterate over all the Hub properties
            {
                TL.LogMessage("Load", "Processing: \"{0}\" simulator values", property);
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
                    TL.LogMessageCrLf("Setup Load", "{0} exception: {1}", property, ex);
                    MessageBox.Show("Setup Load exception 1: " + ex.ToString(), "Setup Load");
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
                TL.LogMessage("ReadDeviceInformation", "Found {0} switch devices", switchDevices.Count);

                // Get the list of ObservingConditions drivers
                ArrayList observingConditionsDevices = profile.RegisteredDevices(Hub.DEVICE_TYPE);
                TL.LogMessage("ReadDeviceInformation", "Found {0} ObservingConditions devices", observingConditionsDevices.Count);

                // Construct a complete list of all drivers
                allDevices.Clear();
                // Add the "no Device" entry as the first entry in the list of devices
                KeyValuePair noDevice = new KeyValuePair(Hub.NO_DEVICE_PROGID, NO_DEVICE_DESCRIPTION);
                allDevices.Add(noDevice);

                // Add the Switch devices to the overall drivers list
                foreach (KeyValuePair device in switchDevices)
                {
                    device.Value = "Switch: " + device.Value;
                    allDevices.Add(device);
                }

                try
                {
                    // Add the ObservingConditions devices to the overall drivers list
                    foreach (KeyValuePair device in observingConditionsDevices)
                    {
                        if (device.Key == Hub.DRIVER_PROGID)
                            continue;

                        device.Value = "ObservingConditions: " + device.Value;
                        allDevices.Add(device);
                    }

                    // add the Simulator entry at the end
                    allDevices.Add(new KeyValuePair(Hub.SIMULATOR_PROGID, SIMULATOR_DESCRIPTION));

                    TL.LogMessage("ReadDeviceInformation", "Found {0} devices", allDevices.Count);
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("ReadDeviceInformation 1", "ObservingConditions description exception: {0}", ex);
                    MessageBox.Show("ReadDeviceInformatio 1: " + ex.ToString(),"ReadDeviceInformation 1");
                }

                // Log the combined list of ObservingConditions and Switch drivers 
                foreach (KeyValuePair device in allDevices)
                {
                    TL.LogMessage("ReadDeviceInformation", "Found device: \"{0}\": \"{1}\"", device.Key, device.Value);
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
                        TL.LogMessage("ReadDeviceInformation", "Processing: \"AveragePeriod\", Device: {0}, Adding device: {1}, Property ProgID: {2}", device.Key, device.Value, Hub.Sensors["AveragePeriod"].ProgID);
                        cmbAveragePeriod.Items.Add(device.Value); // Otherwise add the driver description to the available drivers combo box

                        if (device.Key == Hub.Sensors[Hub.AVERAGE_PERIOD].ProgID) // If this driver's ProgID has been selected by the user in the past and recorded in the Profile, select it in the available drivers combo list
                        {
                            cmbAveragePeriod.SelectedIndex = currentIndex; // This will fire the SelectedIndexChanged event (which calls EnableUpdDown) to set values and enable the switch number as required
                        }
                        currentIndex++;
                    }
                }
                if (cmbAveragePeriod.SelectedIndex < 0 || cmbAveragePeriod.SelectedIndex >= currentIndex)
                    cmbAveragePeriod.SelectedIndex = 0;     // set the average period device to none

                return;
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("ReadDeviceInformation2", ex.ToString());
                MessageBox.Show("ReadDeviceInformation2: " + ex.ToString(), "ReadDeviceInformation2");
            }
        }

        private void chkConnectToDrivers_CheckedChanged(object sender, EventArgs e)
        {
            TL.LogMessage("ConnectToDrivers", "Event fired");
            ReadDeviceInformation();
        }
    }
}