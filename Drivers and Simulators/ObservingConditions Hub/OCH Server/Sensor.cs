using System;
using ASCOM.Utilities;

namespace ASCOM.Simulator
{
    public class Sensor
    {
        const string DEVICEMODE_PROFILENAME = "Device Mode"; const string DEVICEMODE_PROFILENAME_DEFAULT = "None";
        const string PROGID_PROFILENAME = "ProgID"; const string PROGID_PROFILENAME_DEFAULT = "";
        const string SENSORNAME_PROFILENAME = "Sensor Name"; const string SENSORNAME_PROFILENAME_DEFAULT = "Sensor name not set!";
        const string SIMHIGHVALUE_PROFILENAME = "Simulator High Value";
        const string SIMLOWVALUE_PROFILENAME = "Simulator Low Value";
        const string SWITCHNUMBER_PROFILENAME = "Switch Number"; const string SWITCHNUMBER_PROFILENAME_DEFAULT = "0";

        public Sensor()
        {

        }

        public Sensor(string Name)
        {
            SensorName = Name;
        }

        public bool Connected { get; set; }
        public Hub.DeviceType DeviceType
        {
            get
            {
                if (ProgID.EndsWith(Hub.SWITCH_DEVICE_NAME, StringComparison.InvariantCultureIgnoreCase)) return Hub.DeviceType.Switch;
                if (ProgID.EndsWith(Hub.OBSERVINGCONDITIONS_DEVICE_NAME, StringComparison.InvariantCultureIgnoreCase)) return Hub.DeviceType.ObservingConditions;
                if (ProgID=="") throw new InvalidValueException("Sensor.DeviceType - DeviceType called before ProgID has not been set");
                throw new InvalidValueException("Sensor.DeviceType - Unknown device type: " + ProgID);
            }
        }
        public Hub.ConnectionType DeviceMode { get; set; }
        public bool ErrorOnConnect { get; set; }
        public string ProgID { get; set; }
        public string SensorName { get; set; }
        public double SimCurrentValue { get; set; }
        public double SimHighValue { get; set; }
        public double SimLowValue { get; set; }
        public int SwitchNumber { get; set; }

        public void WriteProfile(Profile driverProfile)
        {
            Hub.TL.LogMessage("Sensor.WriteProfile", "Starting to write profile values");
            Hub.TL.LogMessage("Sensor.WriteProfile", SensorName + " DeviceMode: " + DeviceMode.ToString());
            driverProfile.WriteValue(Hub.DRIVER_PROGID, DEVICEMODE_PROFILENAME, DeviceMode.ToString(), SensorName);
            Hub.TL.LogMessage("Sensor.WriteProfile", SensorName + " ProgID: " + ProgID);
            driverProfile.WriteValue(Hub.DRIVER_PROGID, PROGID_PROFILENAME, ProgID, SensorName);
            Hub.TL.LogMessage("Sensor.WriteProfile", SensorName + " SimHighValue: " + SimHighValue);
            driverProfile.WriteValue(Hub.DRIVER_PROGID, SIMHIGHVALUE_PROFILENAME, SimHighValue.ToString(), SensorName);
            Hub.TL.LogMessage("Sensor.WriteProfile", SensorName + " SimLowValue: " + SimLowValue);
            driverProfile.WriteValue(Hub.DRIVER_PROGID, SIMLOWVALUE_PROFILENAME, SimLowValue.ToString(), SensorName);
            Hub.TL.LogMessage("Sensor.WriteProfile", SensorName + " SwitchNumber: " + SwitchNumber.ToString());
            driverProfile.WriteValue(Hub.DRIVER_PROGID, SWITCHNUMBER_PROFILENAME, SwitchNumber.ToString(), SensorName);
            Hub.TL.LogMessage("Sensor.WriteProfile", SensorName + " Completed writing profile values");
        }

        public void ReadProfile(Profile driverProfile)
        {
            Hub.TL.LogMessage("Sensor.ReadProfile", "Starting to read profile values");
            string devmode = driverProfile.GetValue(Hub.DRIVER_PROGID, DEVICEMODE_PROFILENAME, SensorName, DEVICEMODE_PROFILENAME_DEFAULT);
            Hub.TL.LogMessage("Sensor.ReadProfile", SensorName + " DeviceMode: " + devmode);
            DeviceMode = (Hub.ConnectionType)Enum.Parse(typeof(Hub.ConnectionType), driverProfile.GetValue(Hub.DRIVER_PROGID, DEVICEMODE_PROFILENAME, SensorName, DEVICEMODE_PROFILENAME_DEFAULT));
            ProgID = driverProfile.GetValue(Hub.DRIVER_PROGID, PROGID_PROFILENAME, SensorName, PROGID_PROFILENAME_DEFAULT);
            Hub.TL.LogMessage("Sensor.ReadProfile", SensorName + " ProgID: " + ProgID);
            SimHighValue = Convert.ToDouble(driverProfile.GetValue(Hub.DRIVER_PROGID, SIMHIGHVALUE_PROFILENAME, SensorName, Hub.SimulatorDefaultHighValues[SensorName].ToString()));
            Hub.TL.LogMessage("Sensor.ReadProfile", SensorName + " SimHighValue: " + SimHighValue);
            SimLowValue = Convert.ToDouble(driverProfile.GetValue(Hub.DRIVER_PROGID, SIMLOWVALUE_PROFILENAME, SensorName, Hub.SimulatorDefaultLowValues[SensorName].ToString()));
            Hub.TL.LogMessage("Sensor.ReadProfile", SensorName + " SimLowValue: " + SimLowValue);
            SwitchNumber = Convert.ToInt32(driverProfile.GetValue(Hub.DRIVER_PROGID, SWITCHNUMBER_PROFILENAME, SensorName, SWITCHNUMBER_PROFILENAME_DEFAULT));
            Hub.TL.LogMessage("Sensor.ReadProfile", SensorName + " SwitchNumber: " + SwitchNumber.ToString());
            Hub.TL.LogMessage("Sensor.ReadProfile", "Completed reading profile values");
        }
    }
}
