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
        /// </summary>
        public bool ConnectToDriver { get; set; }

        /// <summary>
        /// returns the currently selected sensor ProgId and Switch number
        /// </summary>
        public Sensor SelectedSensor
        {
            get
            {
                var sensor = new Sensor(this.SensorName);

                // get the selected device Id from allDevices
                var ProgId = SetupDialogForm.allDevices[cmbDevice.SelectedIndex].Key;

                switch (ProgId)  // Set the device mode flag and the progId
                {
                    case Hub.NO_DEVICE_PROGID: // No device
                        sensor.DeviceMode = Hub.ConnectionType.None;
                        sensor.ProgID = Hub.NO_DEVICE_PROGID;
                        break;
                    case Hub.DRIVER_PROGID: // Simulate the device
                        sensor.DeviceMode = Hub.ConnectionType.Simulation;
                        sensor.ProgID = Hub.DRIVER_PROGID;
                        break;
                    default: // Real driver
                        sensor.DeviceMode = Hub.ConnectionType.Real;
                        sensor.ProgID = ProgId;
                        break;
                }
                if (ProgId.EndsWith(".Switch"))
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
                //Hub.TL.LogMessage("Setup Load", "Found device: : \"" + device.Value + "\", Count: " + count);
                cmbDevice.Items.Add(device.Value);
                //Hub.TL.LogMessage("Setup Load", "ProgID comparison: : " + device.Key + " " + Hub.Sensors[PropertyName].ProgID);
                if (device.Key == Hub.Sensors[SensorName].ProgID )
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
            Hub.TL.LogMessage("cmbDevice_SelectedIndexChanged", SensorName + " Event has fired");

            // get the selected device Id from allDevices
            var progId = SetupDialogForm.allDevices[cmbDevice.SelectedIndex].Key;

            var switchId = Hub.Sensors[SensorName].SwitchNumber;

            if (progId.EndsWith("." + Hub.SWITCH_DEVICE_NAME, StringComparison.InvariantCultureIgnoreCase)) // Enable or disable the property's Switch name combo 
            {
                // handle switch drivers

                buttonSetup.Enabled = true;

                // populate the switch combo
                cmbSwitch.Items.Clear();

                using (var s = new Switch(progId))
                {
                    if (ConnectToDriver)
                    {
                        cmbSwitch.Enabled = true;
                        upDownSwitch.Enabled = false;
                        try { s.Connected = true; }
                        catch { }
                        int max = 100;
                        try { max = s.MaxSwitch; }
                        catch { }

                        for (short i = 0; i < max; i++)
                        {
                            string str = string.Format("{0}: unknown", i);
                            try
                            {
                                str = string.Format("{0}: {1}", i, s.GetSwitchName(i));
                            }
                            catch {}
                            finally { cmbSwitch.Items.Add(str); }
                        }
                        cmbSwitch.SelectedIndex = switchId;
                        cmbSwitch.Enabled = true;
                        s.Connected = false;
                        upDownSwitch.Maximum = max - 1;
                        upDownSwitch.Value = switchId;
                    }
                    else
                    {
                        cmbSwitch.SelectedIndex = -1;
                        cmbSwitch.Enabled = false;
                        upDownSwitch.Enabled = true;
                        try { upDownSwitch.Maximum = s.MaxSwitch; }
                        catch { upDownSwitch.Maximum = 100; }
                        upDownSwitch.Value = switchId;
                    }
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
                        upDownSwitch.Enabled = false;
                        cmbSwitch.Enabled = false;
                        lastIdx = cmbDevice.SelectedIndex;
                    }
                    catch (PropertyNotImplementedException)
                    {
                        // this property isn't implemented so revert to the previous driver
                        cmbDevice.SelectedIndex = lastIdx;
                    }
                    //catch(NotConnectedException)
                    //{
                    //    // not sure what to do, can we specify that the description is available even
                    //    // if not connected?
                    //}
                }
            }
            else
            {
                cmbSwitch.SelectedIndex = -1;
                //upDownSwitch.Value = 0;
                buttonSetup.Enabled = false;
                cmbSwitch.Enabled = false;
                upDownSwitch.Enabled = false;
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
            upDownSwitch.Value = id >= 0 ? id : 0;
            //if (Hub.Sensors[PropertyName].SwitchNumber == id)
            //    return;
            //Hub.Sensors[PropertyName].SwitchNumber = cmbSwitch.Enabled ? id : 0;
        }

        private void upDownSwitch_ValueChanged(object sender, EventArgs e)
        {
            var id = (int)upDownSwitch.Value;
            if (cmbSwitch.Enabled && cmbSwitch.Items.Count < id)
                cmbSwitch.SelectedIndex = id;
            //if (Hub.Sensors[PropertyName].SwitchNumber == id)
            //    return;
            //Hub.Sensors[PropertyName].SwitchNumber = id;
        }

        /// <summary>
        /// runs the setup dialog for the current driver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSetup_Click(object sender, EventArgs e)
        {
            Hub.TL.LogMessage("buttonSetup_Click", "Event has fired");
            var ProgId = SetupDialogForm.allDevices[cmbDevice.SelectedIndex].Key;
            using (var dev = new ASCOM.DriverAccess.AscomDriver(ProgId))
            {
                dev.SetupDialog();
            }
        }
    }
}
