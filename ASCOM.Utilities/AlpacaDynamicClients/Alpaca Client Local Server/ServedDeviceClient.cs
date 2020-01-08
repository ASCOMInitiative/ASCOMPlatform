using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ASCOM.Utilities;
using System.Collections;

namespace ASCOM.DynamicRemoteClients
{
    public partial class ServedDeviceClient : UserControl
    {
        int deviceNumber = 0;
        string description = "";
        string progID = "";

        Profile profile;
        List<string> deviceTypes = new List<string>();
        Dictionary<string, string> deviceDictionary = new Dictionary<string, string>();
        bool recalculate = false;
        TraceLoggerPlus TL;

        #region Initialisers
        public ServedDeviceClient()
        {
            InitializeComponent();
            cmbDeviceType.MouseUp += CmbDeviceType_MouseUp;
        }
        #endregion

        #region Data accessor properties
        public int DeviceNumber
        {
            get
            {
                return deviceNumber;
            }
            set
            {
                deviceNumber = value;
                txtDeviceNumber.Text = value.ToString();
            }
        }

        public string DeviceType
        {
            get
            {
                try
                {
                    return cmbDeviceType.SelectedItem.ToString();
                }
                catch
                {
                    return "None";
                }
            }
            set
            {
                try
                {
                    cmbDeviceType.SelectedItem = value;
                }
                catch
                {
                    cmbDeviceType.SelectedIndex = -1; ;
                }
            }
        }

        public string Description
        {
            get
            {
                try
                {
                    return description;
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    description = value;
                    cmbDevice.SelectedItem = value.ToString();
                }
                catch { }
            }
        }

        public string ProgID
        {
            get
            {
                return progID;
            }
            set
            {
                progID = value;
                if (!DesignMode)
                {
                    try
                    {
                        if (TL != null) TL.LogMessage(0, 0, 0, "ServedDeviceClient.ProgID", "Set ProgID to: " + progID);
                        switch (progID)
                        {
                            case "":
                                cmbDevice.SelectedIndex = -1;
                                break;
                            case SharedConstants.DEVICE_NOT_CONFIGURED:
                                TL.LogMessage(0, 0, 0, "ServedDeviceClient.ProgID", "Description: " + SharedConstants.DEVICE_NOT_CONFIGURED);
                                cmbDevice.SelectedItem = 0;
                                break;
                            default:
                                TL.LogMessage(0, 0, 0, "ServedDeviceClient.ProgID", "Description: " + deviceDictionary[progID]);
                                cmbDevice.SelectedItem = deviceDictionary[progID];
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf(0, 0, 0, "ServedDeviceClient.ProgID", ex.ToString());
                        cmbDevice.SelectedIndex = -1;
                    }
                }
            }
        }

        public bool AllowConnectedSetFalse
        {
            get
            {
                return chkAllowSetConnectedFalse.Checked;
            }
            set
            {
                chkAllowSetConnectedFalse.Checked = value;
            }
        }

        public bool AllowConnectedSetTrue
        {
            get
            {
                return chkAllowSetConnectedTrue.Checked;
            }
            set
            {
                chkAllowSetConnectedTrue.Checked = value;
            }
        }
        #endregion

        #region Event handlers
        public void InitUI(TraceLoggerPlus Logger)
        {
            TL = Logger;
            TL.LogMessage(0, 0, 0, "InitUI", "Start");
            profile = new Profile();
            TL.LogMessage(0, 0, 0, "InitUI", "Created Profile");

            cmbDeviceType.Items.Add(SharedConstants.DEVICE_NOT_CONFIGURED);
            TL.LogMessage(0, 0, 0, "InitUI", "Added Device not configured");


            foreach (string deviceType in profile.RegisteredDeviceTypes)
            {
                TL.LogMessage(0, 0, 0, "InitUI", "Adding item: " + deviceType);
                cmbDeviceType.Items.Add(deviceType);
                deviceTypes.Add(deviceType); // Remember the device types on this system
            }
            TL.LogMessage(0, 0, 0, "InitUI", "Setting selected index to 0");

            cmbDeviceType.SelectedIndex = 0;
            TL.LogMessage(0, 0, 0, "InitUI", "Finished");

        }

        private void CmbDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                TL.LogMessage(0, 0, 0, "cmbDeviceType_Changed", "Clearing items");
                cmbDevice.Items.Clear();

                TL.LogMessage(0, 0, 0, "cmbDeviceType_Changed", "Setting selected index to -1");
                cmbDevice.SelectedIndex = -1;

                TL.LogMessage(0, 0, 0, "cmbDeviceType_Changed", "Resetting instance number");
                DeviceNumber = 0;

                if (cmbDeviceType.SelectedItem.ToString() == SharedConstants.DEVICE_NOT_CONFIGURED)
                {
                    TL.LogMessage(0, 0, 0, "cmbDeviceType_Changed", "\"None\" device type selected");
                    cmbDevice.Items.Clear();
                    cmbDevice.SelectedIndex = -1;
                    cmbDevice.ResetText();
                    cmbDevice.Enabled = false;
                    description = "";
                    progID = SharedConstants.DEVICE_NOT_CONFIGURED;

                }
                else
                {
                    cmbDevice.Enabled = true;
                    TL.LogMessage(0, 0, 0, "cmbDeviceType_Changed", "Real device type has been selected");
                }


                if (recalculate)
                {
                    recalculate = false;
                }

                // Set up device list so we can translate ProgID to description

                ArrayList devices = profile.RegisteredDevices(cmbDeviceType.SelectedItem.ToString());
                TL.LogMessage(0, 0, 0, "cmbDeviceType_Changed", "Created registered device arraylist");

                deviceDictionary.Clear();
                foreach (KeyValuePair kvp in devices)
                {
                    if (!deviceDictionary.ContainsKey(kvp.Value)) deviceDictionary.Add(kvp.Key, kvp.Value);
                    cmbDevice.Items.Add(kvp.Value);
                }
                if (cmbDevice.Items.Count > 0) cmbDevice.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void CmbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            description = cmbDevice.SelectedItem.ToString();
            foreach (KeyValuePair<string, string> kvp in deviceDictionary)
            {
                if (kvp.Value == description)
                {
                    progID = kvp.Key;
                }
            }
        }

        private void CmbDeviceType_MouseUp(object sender, MouseEventArgs e)
        {
            recalculate = true;
        }
        #endregion
    }
}
