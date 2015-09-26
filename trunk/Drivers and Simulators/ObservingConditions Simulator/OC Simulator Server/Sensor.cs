using System;
using ASCOM.Utilities;

namespace ASCOM.Simulator
{
    public class Sensor
    {
        const string SIMFROMVALUE_PROFILENAME = "Simulated From Value";
        const string SIMTOVALUE_PROFILENAME = "Simulated To Value";
        const string IS_IMPLEMENTED_PROFILENAME = "Is Implemented";
        const string SHOW_NOT_READY_PROFILENAME = "Show NotReady";
        const string NOT_READY_DELAY_PROFILENAME = "Not Ready Delay";

        public Sensor()
        {

        }

        public Sensor(string Name)
        {
            SensorName = Name;
        }

        public string SensorName { get; set; }
        public double SimCurrentValue { get; set; }
        public double SimToValue { get; set; }
        public double SimFromValue { get; set; }
        public bool IsImplemented { get; set; }
        public bool ShowNotReady { get; set; }
        public double NotReadyDelay{ get; set; }
        public double TimeSinceLastUpdate { get; set; }

        public void ReadProfile(Profile driverProfile)
        {
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", "Starting to read profile values");

            SimFromValue = Convert.ToDouble(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, SIMFROMVALUE_PROFILENAME, SensorName, OCSimulator.SimulatorDefaultFromValues[SensorName].ToString()));
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " SimFromValue: " + SimFromValue);

            SimToValue = Convert.ToDouble(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, SIMTOVALUE_PROFILENAME, SensorName, OCSimulator.SimulatorDefaultToValues[SensorName].ToString()));
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " SimToValue: " + SimToValue);

            IsImplemented = Convert.ToBoolean(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, IS_IMPLEMENTED_PROFILENAME, SensorName, "true"));
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " Is Implemented: " + IsImplemented.ToString());

            ShowNotReady = Convert.ToBoolean(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, SHOW_NOT_READY_PROFILENAME, SensorName, "false"));
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " ShowNotReady: " + ShowNotReady.ToString());

            NotReadyDelay = Convert.ToDouble(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, NOT_READY_DELAY_PROFILENAME, SensorName, "0.0"));
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " NotReadyDelay: " + NotReadyDelay.ToString());

            OCSimulator.TL.LogMessage("Sensor.ReadProfile", "Completed reading profile values");
        }

        public void WriteProfile(Profile driverProfile)
        {
            OCSimulator.TL.LogMessage("Sensor.WriteProfile", "Starting to write profile values");

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " SimFromValue: " + SimFromValue);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, SIMFROMVALUE_PROFILENAME, SimFromValue.ToString(), SensorName);

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " SimToValue: " + SimToValue);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, SIMTOVALUE_PROFILENAME, SimToValue.ToString(), SensorName);

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " Is Implemented: " + IsImplemented);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, IS_IMPLEMENTED_PROFILENAME, IsImplemented.ToString(), SensorName);

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " ShowNotReady: " + ShowNotReady);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, SHOW_NOT_READY_PROFILENAME, ShowNotReady.ToString(), SensorName);

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " NotReadyDelay: " + NotReadyDelay);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, NOT_READY_DELAY_PROFILENAME, NotReadyDelay.ToString(), SensorName);

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " Completed writing profile values");
        }
    }
}
