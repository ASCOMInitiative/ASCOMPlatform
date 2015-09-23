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
        public OCSimulator.DeviceType DeviceType
        {
            get
            {
                if (ProgID.EndsWith(OCSimulator.SWITCH_DEVICE_NAME, StringComparison.InvariantCultureIgnoreCase)) return OCSimulator.DeviceType.Switch;
                if (ProgID.EndsWith(OCSimulator.DEVICE_TYPE, StringComparison.InvariantCultureIgnoreCase)) return OCSimulator.DeviceType.ObservingConditions;
                if (ProgID=="") throw new InvalidValueException("Sensor.DeviceType - DeviceType called before ProgID has not been set");
                throw new InvalidValueException("Sensor.DeviceType - Unknown device type: " + ProgID);
            }
        }
        public OCSimulator.ConnectionType DeviceMode { get; set; }
        public bool ErrorOnConnect { get; set; }
        public string ProgID { get; set; }
        public string SensorName { get; set; }
        public double SimCurrentValue { get; set; }
        public double SimHighValue { get; set; }
        public double SimLowValue { get; set; }
        public int SwitchNumber { get; set; }

        public void WriteProfile(Profile driverProfile)
        {
            OCSimulator.TL.LogMessage("Sensor.WriteProfile", "Starting to write profile values");
            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " DeviceMode: " + DeviceMode.ToString());
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, DEVICEMODE_PROFILENAME, DeviceMode.ToString(), SensorName);
            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " ProgID: " + ProgID);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, PROGID_PROFILENAME, ProgID, SensorName);
            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " SimHighValue: " + SimHighValue);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, SIMHIGHVALUE_PROFILENAME, SimHighValue.ToString(), SensorName);
            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " SimLowValue: " + SimLowValue);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, SIMLOWVALUE_PROFILENAME, SimLowValue.ToString(), SensorName);
            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " SwitchNumber: " + SwitchNumber.ToString());
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, SWITCHNUMBER_PROFILENAME, SwitchNumber.ToString(), SensorName);
            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " Completed writing profile values");
        }

        public void ReadProfile(Profile driverProfile)
        {
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", "Starting to read profile values");
            string devmode = driverProfile.GetValue(OCSimulator.DRIVER_PROGID, DEVICEMODE_PROFILENAME, SensorName, DEVICEMODE_PROFILENAME_DEFAULT);
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " DeviceMode: " + devmode);
            DeviceMode = (OCSimulator.ConnectionType)Enum.Parse(typeof(OCSimulator.ConnectionType), driverProfile.GetValue(OCSimulator.DRIVER_PROGID, DEVICEMODE_PROFILENAME, SensorName, DEVICEMODE_PROFILENAME_DEFAULT));
            ProgID = driverProfile.GetValue(OCSimulator.DRIVER_PROGID, PROGID_PROFILENAME, SensorName, PROGID_PROFILENAME_DEFAULT);
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " ProgID: " + ProgID);
            SimHighValue = Convert.ToDouble(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, SIMHIGHVALUE_PROFILENAME, SensorName, OCSimulator.SimulatorDefaultToValues[SensorName].ToString()));
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " SimHighValue: " + SimHighValue);
            SimLowValue = Convert.ToDouble(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, SIMLOWVALUE_PROFILENAME, SensorName, OCSimulator.SimulatorDefaultFromValues[SensorName].ToString()));
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " SimLowValue: " + SimLowValue);
            SwitchNumber = Convert.ToInt32(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, SWITCHNUMBER_PROFILENAME, SensorName, SWITCHNUMBER_PROFILENAME_DEFAULT));
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " SwitchNumber: " + SwitchNumber.ToString());
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", "Completed reading profile values");
        }
    }
}
