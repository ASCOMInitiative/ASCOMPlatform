using System;
using System.Collections.Generic;
using ASCOM.Utilities;

namespace ASCOM.Simulator
{
    /// <summary>
    /// Observing conditions hub definition
    /// </summary>
    public class Sensor
    {
        const string DEVICEMODE_PROFILENAME = "Device Mode"; const string DEVICEMODE_PROFILENAME_DEFAULT = "None";
        const string PROGID_PROFILENAME = "ProgID"; const string PROGID_PROFILENAME_DEFAULT = "";
        const string SENSORNAME_PROFILENAME = "Sensor Name"; const string SENSORNAME_PROFILENAME_DEFAULT = "Sensor name not set!";
        const string SWITCHNUMBER_PROFILENAME = "Switch Number"; const string SWITCHNUMBER_PROFILENAME_DEFAULT = "0";

        #region Initialisers

        public Sensor()
        {
            Readings = new List<Hub.TimeValue>();
            LastPeriodAverage = Hub.BAD_VALUE; // Initialise to a value indicating no average is available
            TimeOfLastUpdate = Hub.BAD_DATETIME;
        }

        public Sensor(string Name)
        {
            SensorName = Name;
            Readings = new List<Hub.TimeValue>();
            LastPeriodAverage = Hub.BAD_VALUE; // Initialise to a value indicating no average is available
            TimeOfLastUpdate = Hub.BAD_DATETIME;
        }

        #endregion

        public bool Connected { get; set; }
        public Hub.DeviceType DeviceType
        {
            get
            {
                if (ProgID.EndsWith(Hub.SWITCH_DEVICE_NAME, StringComparison.InvariantCultureIgnoreCase)) return Hub.DeviceType.Switch;
                if (ProgID.EndsWith(Hub.DEVICE_TYPE, StringComparison.InvariantCultureIgnoreCase)) return Hub.DeviceType.ObservingConditions;
                if (ProgID == "") throw new InvalidValueException("Sensor.DeviceType - DeviceType called before ProgID has not been set");
                throw new InvalidValueException("Sensor.DeviceType - Unknown device type: " + ProgID);
            }
        }
        public Hub.ConnectionType DeviceMode { get; set; }
        public string ProgID { get; set; }
        public string SensorName { get; set; }
        public int SwitchNumber { get; set; }
        public double LastPeriodAverage { get; set; }
        public DateTime TimeOfLastUpdate { get; set; }
        public List<Hub.TimeValue> Readings { get; set; }

        public void WriteProfile(Profile driverProfile)
        {
            if (Hub.DebugTraceState) Hub.TL.LogMessage("Sensor.WriteProfile", SensorName + " DeviceMode: " + DeviceMode.ToString());
            driverProfile.WriteValue(Hub.DRIVER_PROGID, DEVICEMODE_PROFILENAME, DeviceMode.ToString(), SensorName);
            if (Hub.DebugTraceState) Hub.TL.LogMessage("Sensor.WriteProfile", SensorName + " ProgID: " + ProgID);
            driverProfile.WriteValue(Hub.DRIVER_PROGID, PROGID_PROFILENAME, ProgID, SensorName);
            if (Hub.DebugTraceState) Hub.TL.LogMessage("Sensor.WriteProfile", SensorName + " SwitchNumber: " + SwitchNumber.ToString());
            driverProfile.WriteValue(Hub.DRIVER_PROGID, SWITCHNUMBER_PROFILENAME, SwitchNumber.ToString(), SensorName);
            if (Hub.DebugTraceState) Hub.TL.LogMessage("Sensor.WriteProfile", SensorName + " Completed writing profile values");
        }

        public void ReadProfile(Profile driverProfile)
        {
            if (Hub.DebugTraceState) Hub.TL.LogMessage("Sensor.ReadProfile", "Starting to read profile values");
            string devmode = driverProfile.GetValue(Hub.DRIVER_PROGID, DEVICEMODE_PROFILENAME, SensorName, DEVICEMODE_PROFILENAME_DEFAULT);
            if (Hub.DebugTraceState) Hub.TL.LogMessage("Sensor.ReadProfile", SensorName + " DeviceMode: " + devmode);
            DeviceMode = (Hub.ConnectionType)Enum.Parse(typeof(Hub.ConnectionType), driverProfile.GetValue(Hub.DRIVER_PROGID, DEVICEMODE_PROFILENAME, SensorName, DEVICEMODE_PROFILENAME_DEFAULT));
            ProgID = driverProfile.GetValue(Hub.DRIVER_PROGID, PROGID_PROFILENAME, SensorName, PROGID_PROFILENAME_DEFAULT);
            if (Hub.DebugTraceState) Hub.TL.LogMessage("Sensor.ReadProfile", SensorName + " ProgID: " + ProgID);
            SwitchNumber = Convert.ToInt32(driverProfile.GetValue(Hub.DRIVER_PROGID, SWITCHNUMBER_PROFILENAME, SensorName, SWITCHNUMBER_PROFILENAME_DEFAULT));
            if (Hub.DebugTraceState) Hub.TL.LogMessage("Sensor.ReadProfile", SensorName + " SwitchNumber: " + SwitchNumber.ToString());
            if (Hub.DebugTraceState) Hub.TL.LogMessage("Sensor.ReadProfile", "Completed reading profile values");
        }
    }
}
