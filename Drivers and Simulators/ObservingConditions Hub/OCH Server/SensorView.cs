using System;
using System.Drawing;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.DriverAccess;

namespace ASCOM.Simulator
{
    /// <summary>
    /// Handles selecting the Switch or ObservingConditions driver and, for the switch,
    /// selecting the switch id either using a combo of switch names or a spin button to
    /// select the switch id.
    /// Also allows calling the setup dialogue for the selected driver.
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

                    default: // Real driver
                        sensor.DeviceMode = Hub.ConnectionType.Real;
                        sensor.ProgID = ProgId;
                        break;
                }
                if (ProgId.EndsWith("." + Hub.SWITCH_DEVICE_NAME, StringComparison.OrdinalIgnoreCase))
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

            // Device combo box is self painted because the DropDownStyle is DropDownList to make the list read only, and this changes the background colour to grey!
            cmbDevice.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSwitch.DrawMode = DrawMode.OwnerDrawFixed;
            cmbDevice.DrawItem += new DrawItemEventHandler(comboBox_DrawItem);
            cmbSwitch.DrawItem += new DrawItemEventHandler(comboBox_DrawItem);
        }

        /// <summary>
        /// Event handler to paint the device list combo box in the "DropDown" rather than "DropDownList" style
        /// </summary>
        /// <param name="sender">Device to be painted</param>
        /// <param name="e">Draw event arguments object</param>
        void comboBox_DrawItem(object sender, DrawItemEventArgs e)
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
                if (device.Key == Hub.Sensors[SensorName].ProgID)
                {
                    lastIdx = count;
                    cmbDevice.SelectedIndex = count; // This will fire the SelectedIndexChanged event to set values and enable the switch number as required
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

            // get the selected device Id from allDevices
            if (cmbDevice.SelectedIndex < 0 || cmbDevice.SelectedIndex >= SetupDialogForm.allDevices.Count)
            {
                MessageBox.Show("cmbDevice_SelectedIndexChanged - index out of range");
            }
            var progId = SetupDialogForm.allDevices[cmbDevice.SelectedIndex].Key;
            Hub.TL.LogMessage("cmbDevice_SelectedIndexChanged", "{0} Event has fired, ProgID: {1}", SensorName, progId);

            if (progId.EndsWith("." + Hub.SWITCH_DEVICE_NAME, StringComparison.OrdinalIgnoreCase)) // Enable or disable the property's Switch name combo 
            {
                // handle switch drivers
                Hub.TL.LogMessage("cmbDevice_Index", "Switch found - ProgID: {0}", progId);

                var switchId = Hub.Sensors[SensorName].SwitchNumber;

                buttonSetup.Enabled = true;

                // populate the switch combo
                cmbSwitch.Items.Clear();

                try
                {
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
                            catch { }    // could set ConnectToDriver to false
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
                catch (Exception ex)
                {
                    Hub.TL.LogMessageCrLf("cmbDevice_Index", "Select Switch device exception: {0}", ex.ToString());
                    MessageBox.Show("Exception: " + ex.Message);
                }
            }
            else if (progId.EndsWith("." + Hub.OBSERVING_CONDITIONS_DEVICE_TYPE, StringComparison.OrdinalIgnoreCase))
            {
                Hub.TL.LogMessage("cmbDevice_Index", "ObservingConditions device found - ProgID: {0}", progId);

                // checks that the selected OC driver implements this property by
                // trying to read the sensor description.
                // can we specify that the description is available even when not connected?
                try
                {
                    using (var oc = new ObservingConditions(progId))
                    {

                        if (ConnectToDriver)
                        {
                            // try to connect, ignore error.  This can block if connecting takes a while
                            this.Cursor = Cursors.WaitCursor;
                            try { oc.Connected = true; }
                            catch { }    // could set ConnectToDriver to false
                            this.Cursor = Cursors.Default;
                        }

                        try
                        {
                            Hub.TL.LogMessage("cmbDevice_Index", "Getting description");
                            string description = oc.SensorDescription(SensorName);
                            Hub.TL.LogMessage("cmbDevice_Index", "Found description: {0}", description);
                            buttonSetup.Enabled = true;
                            upDownSwitch.Visible = false;
                            cmbSwitch.Visible = false;
                            labelDescription.Visible = true;
                            labelDescription.Text = description;
                        }
                        catch (Exception)
                        {
                            // not sure what to do, can we specify that the description is available even
                            // if not connected?
                            // MessageBox.Show(string.Format("ObservingConditions.GetSensorDescription({0}) Exception {1}", SensorName, ex.Message));
                            buttonSetup.Enabled = true;
                            upDownSwitch.Visible = false;
                            cmbSwitch.Visible = false;
                            labelDescription.Visible = false;
                        }
                        // set up the UI
                    }
                }
                catch (Exception ex)
                {
                    Hub.TL.LogMessageCrLf("cmbDevice_Index", "Select ObservingConditions device exception: {0}", ex.ToString());
                    MessageBox.Show("Exception: " + ex.Message);
                }

            }
            else
            {
                Hub.TL.LogMessage("cmbDevice_Index", "No device found - ProgID: {0}", progId);

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
        /// runs the setup dialogue for the current driver
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
            try
            {
                using (var dev = new ASCOM.DriverAccess.AscomDriver(ProgId))
                {
                    dev.SetupDialog();
                }
            }
            catch (Exception ex)
            {
                Hub.TL.LogMessageCrLf("buttonSetup_Click", "Exception opening Setup Dialogue: {0}", ex.ToString());
                MessageBox.Show("Unable to open Setup Dialogue: " + ex.Message);
            }

        }
    }
}
