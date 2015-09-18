using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.DriverAccess;

namespace ASCOM.Simulator
{
    /// <summary>
    /// Handles selecting the Switch or ObservingConditions driver and, for the switch,
    /// selecting the swtch id either using a combo of switch names or a spin button to
    /// select the switch id.
    /// Also allows calling the setup dialog for the selected driver.
    /// </summary>
    public partial class SensorView : UserControl
    {
        private int lastIdx;

        /// <summary>
        /// The sensor name must match the name in Hub.Sensors[property]
        /// </summary>
        public string SensorName { get; set; }

        /// <summary>
        /// should be done with an event, at present it's assumed that this is set
        /// before InitUi is called.
        /// Or could be in this control so each device can have it's own state.
        /// *** currently not used, we attempt to connect and set the UI depending on what can be read.
        /// </summary>
        public bool ConnectToDriver { get; set; }

        /// <summary>
        /// returns the currently selected sensor ProgId, DeviceMode and SwitchNumber
        /// </summary>
        public Sensor SelectedSensor
        {
            get
            {
                var sensor = new Sensor(this.SensorName);
                if (DesignMode)
                    return sensor;

                // get the selected device Id from allDevices
                if (cmbDevice.SelectedIndex < 0 || cmbDevice.SelectedIndex >= SetupDialogForm.allDevices.Count)
                {
                    MessageBox.Show("SelectedSensor - index out of range");
                }
                var ProgId = SetupDialogForm.allDevices[cmbDevice.SelectedIndex].Key;

                switch (ProgId)  // Set the device mode flag and the progId
                {
                    case Hub.NO_DEVICE_PROGID: // No device
                        sensor.DeviceMode = Hub.ConnectionType.None;
                        sensor.ProgID = Hub.NO_DEVICE_PROGID;
                        break;
                    //case Hub.DRIVER_PROGID: // Simulate the device
                    case Hub.SIMULATOR_PROGID: // Simulate the device
                        sensor.DeviceMode = Hub.ConnectionType.Simulation;
                        sensor.ProgID = Hub.DRIVER_PROGID;
                        break;
                    default: // Real driver
                        sensor.DeviceMode = Hub.ConnectionType.Real;
                        sensor.ProgID = ProgId;
                        break;
                }
                if (ProgId.EndsWith("." + Hub.SWITCH_DEVICE_NAME, StringComparison.InvariantCultureIgnoreCase))
                {
                    sensor.SwitchNumber = (int)upDownSwitch.Value;
                }
                else
                    sensor.SwitchNumber = 0;

                return sensor;
            }
        }

        public SensorView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// sets up the combo boxes and selects the current driver and device Id.
        /// </summary>
        public void InitUI()
        {
            // Fill the device combo box drop down list and set the appropriate value
            cmbDevice.Items.Clear();
            int count = 0;
            foreach (KeyValuePair device in SetupDialogForm.allDevices)
            {
                Hub.TL.LogMessage("Setup Load", "Found device: : \"" + device.Value + "\", Count: " + count);
                cmbDevice.Items.Add(device.Value);
                //Hub.TL.LogMessage("Setup Load", "ProgID comparison: : " + device.Key + " " + Hub.Sensors[PropertyName].ProgID);
                if (device.Key == Hub.Sensors[SensorName].ProgID ||
                    Hub.Sensors[SensorName].DeviceMode == Hub.ConnectionType.Simulation && device.Key == Hub.SIMULATOR_PROGID)
                {
                    //Hub.TL.LogMessage("Setup Load", "Before setting index to: : " + count);
                    lastIdx = count;
                    cmbDevice.SelectedIndex = count; // This will fire the SelectedIndexChanged event to set values and enable the switch number as required
                    //Hub.TL.LogMessage("Setup Load", "After setting index to: : " + count);
                }
                count++;
            }
        }

        /// <summary>
        /// update the deviceId and configures this view as required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            Hub.TL.LogMessage("cmbDevice_SelectedIndexChanged", "{0} Event has fired", SensorName);

            // get the selected device Id from allDevices
            if (cmbDevice.SelectedIndex < 0 || cmbDevice.SelectedIndex >= SetupDialogForm.allDevices.Count)
            {
                MessageBox.Show("cmbDevice_SelectedIndexChanged - index out of range");
            }
            var progId = SetupDialogForm.allDevices[cmbDevice.SelectedIndex].Key;

            if (progId.EndsWith("." + Hub.SWITCH_DEVICE_NAME, StringComparison.InvariantCultureIgnoreCase)) // Enable or disable the property's Switch name combo 
            {
                // handle switch drivers
                var switchId = Hub.Sensors[SensorName].SwitchNumber;

                buttonSetup.Enabled = true;

                // populate the switch combo
                cmbSwitch.Items.Clear();

                using (var s = new Switch(progId))
                {
                    // we try to connect and read the switch properties from the driver
                    // if it succeeds then we enable the switch name combo
                    // the spin button is always enabled.
                    var enableCmb = false;
                    //upDownSwitch.Enabled = true;
                    if (ConnectToDriver)
                    {
                        // try to connect, ignore error.  This can block if connecting takes a while
                        this.Cursor = Cursors.WaitCursor;
                        try { s.Connected = true; }
                        catch { }
                        this.Cursor = Cursors.Default;
                        
                    }
                    // try to set max switches from driver, set up combo as long as there is no error
                    int max = 100;
                    try
                    {
                        max = s.MaxSwitch;      // this may work, even if we can't read the switch names
                        for (short i = 0; i < max; i++)
                        {
                            string str;
                            try { str = s.GetSwitchDescription(i); }
                            catch { str = s.GetSwitchName(i); }     // for V1 switches which don't have a description
                            cmbSwitch.Items.Add(string.Format("{0}: {1}", i, str));
                        }
                        enableCmb = true;   // successfully populated combo so enable it
                    }
                    catch
                    {
                        cmbSwitch.Text = "";
                    }
                    // set up the UI
                    cmbSwitch.SelectedIndex = enableCmb ? switchId : -1;
                    cmbSwitch.Visible = enableCmb;
                    upDownSwitch.Visible = true;
                    labelDescription.Visible = false;
                    upDownSwitch.Maximum = max - 1;
                    upDownSwitch.Value = switchId;
                }
            }
            else if (progId.EndsWith("." + Hub.OBSERVINGCONDITIONS_DEVICE_NAME, StringComparison.InvariantCultureIgnoreCase))
            {
                // checks that the selected OC driver implements this property by
                // trying to read the sensor description.
                // can we specify that the description is available even when not connected?
                using (var oc = new ObservingConditions(progId))
                {
                    try
                    {
                        var d = oc.SensorDescription(SensorName);
                        buttonSetup.Enabled = true;
                        cmbSwitch.Text = "";
                        upDownSwitch.Value = 0;
                        upDownSwitch.Visible = false;
                        cmbSwitch.Visible = false;
                        labelDescription.Visible = true;
                        labelDescription.Text = d;
                        lastIdx = cmbDevice.SelectedIndex;
                    }
                    catch (PropertyNotImplementedException)
                    {
                        // this property isn't implemented in the selected driver so revert to the previous driver
                        cmbDevice.SelectedIndex = lastIdx;
                    }
                    //catch (Exception)
                    //{
                    //    // not sure what to do, can we specify that the description is available even
                    //    // if not connected?
                    //}
                }
            }
            else
            {
                // no device selected
                cmbSwitch.SelectedIndex = -1;
                //upDownSwitch.Value = 0;
                buttonSetup.Enabled = false;
                cmbSwitch.Visible = false;
                upDownSwitch.Visible = false;
                labelDescription.Visible = false;
            }
        }



        /// <summary>
        /// updates Hub.Sensors[propertyName].Switchnumber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSwitch_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = cmbSwitch.SelectedIndex;
            if (upDownSwitch.Value == id || id < 0 || id > upDownSwitch.Maximum)
                return;
            upDownSwitch.Value = id;
        }

        private void upDownSwitch_ValueChanged(object sender, EventArgs e)
        {
            var id = (int)upDownSwitch.Value;
            if (cmbSwitch.SelectedIndex == id)
                return;
            if (cmbSwitch.Visible && cmbSwitch.Items.Count >= id)
                cmbSwitch.SelectedIndex = id;
        }

        /// <summary>
        /// runs the setup dialog for the current driver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSetup_Click(object sender, EventArgs e)
        {
            Hub.TL.LogMessage("buttonSetup_Click", "{0} Event has fired", SensorName);
            if (cmbDevice.SelectedIndex < 0 || cmbDevice.SelectedIndex >= SetupDialogForm.allDevices.Count)
            {
                MessageBox.Show("buttonSetup_Click - index out of range");
            }
            var ProgId = SetupDialogForm.allDevices[cmbDevice.SelectedIndex].Key;
            using (var dev = new ASCOM.DriverAccess.AscomDriver(ProgId))
            {
                dev.SetupDialog();
            }
        }
    }
}
