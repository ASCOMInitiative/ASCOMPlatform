using System;
using System.Collections.Generic;
using System.Globalization;
using ASCOM.Utilities;

namespace ASCOM.Simulator
{
    /// <summary>
    /// Observing conditions simulator definition
    /// </summary>
    public class Sensor
    {

        public Sensor()
        {
            Readings = new List<OCSimulator.TimeValue>();
        }

        public Sensor(string Name)
        {
            Readings = new List<OCSimulator.TimeValue>();
            SensorName = Name;
        }

        public string SensorName { get; set; }
        public double SimCurrentValue { get; set; }
        public double SimToValue { get; set; }
        public double SimFromValue { get; set; }
        public double ValueCycleTime { get; set; }
        public bool IsImplemented { get; set; }
        public bool ShowNotReady { get; set; }
        public double NotReadyDelay { get; set; }
        public DateTime TimeOfLastUpdate { get; set; }
        public List<OCSimulator.TimeValue> Readings { get; set; }
        public OCSimulator.ValueCycleDirections ValueCycleDirection { get; set; }
        public bool Override { get; set; }
        public double OverrideValue { get; set; }

        public void ReadProfile(Profile driverProfile)
        {
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", "Starting to read profile values");

            SimFromValue = Convert.ToDouble(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, OCSimulator.SIMFROMVALUE_PROFILENAME, SensorName, OCSimulator.SimulatorDefaultFromValues[SensorName].ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " SimFromValue: " + SimFromValue);

            SimToValue = Convert.ToDouble(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, OCSimulator.SIMTOVALUE_PROFILENAME, SensorName, OCSimulator.SimulatorDefaultToValues[SensorName].ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " SimToValue: " + SimToValue);

            IsImplemented = Convert.ToBoolean(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, OCSimulator.IS_IMPLEMENTED_PROFILENAME, SensorName, OCSimulator.IS_IMPLEMENTED_DEFAULT), CultureInfo.InvariantCulture);
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " Is Implemented: " + IsImplemented.ToString());

            ShowNotReady = Convert.ToBoolean(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, OCSimulator.SHOW_NOT_READY_PROFILENAME, SensorName, OCSimulator.SHOW_NOT_READY_DEFAULT), CultureInfo.InvariantCulture);
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " ShowNotReady: " + ShowNotReady.ToString());

            NotReadyDelay = Convert.ToDouble(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, OCSimulator.NOT_READY_DELAY_PROFILENAME, SensorName, OCSimulator.NOT_READY_DELAY_DEFAULT), CultureInfo.InvariantCulture);
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " NotReadyDelay: " + NotReadyDelay.ToString());

            ValueCycleTime = Convert.ToDouble(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, OCSimulator.VALUE_CYCLE_TIME_PROFILE_NAME, SensorName, OCSimulator.VALUE_CYCLE_TIME_DEFAULT), CultureInfo.InvariantCulture);
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " Value CycleTime: " + ValueCycleTime.ToString());

            Override = Convert.ToBoolean(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, OCSimulator.OVERRIDE_PROFILENAME, SensorName, OCSimulator.OVERRIDE_DEFAULT), CultureInfo.InvariantCulture);
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " Override: " + Override.ToString());

            OverrideValue = Convert.ToDouble(driverProfile.GetValue(OCSimulator.DRIVER_PROGID, OCSimulator.OVERRIDE_VALUE_PROFILENAME, SensorName, OCSimulator.SimulatorDefaultFromValues[SensorName].ToString(CultureInfo.InvariantCulture)), CultureInfo.InvariantCulture);
            OCSimulator.TL.LogMessage("Sensor.ReadProfile", SensorName + " OverrideValue: " + OverrideValue.ToString());

            OCSimulator.TL.LogMessage("Sensor.ReadProfile", "Completed reading profile values");
        }

        public void WriteProfile(Profile driverProfile)
        {
            OCSimulator.TL.LogMessage("Sensor.WriteProfile", "Starting to write profile values");

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " SimFromValue: " + SimFromValue);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, OCSimulator.SIMFROMVALUE_PROFILENAME, SimFromValue.ToString(CultureInfo.InvariantCulture), SensorName);

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " SimToValue: " + SimToValue);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, OCSimulator.SIMTOVALUE_PROFILENAME, SimToValue.ToString(CultureInfo.InvariantCulture), SensorName);

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " Is Implemented: " + IsImplemented);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, OCSimulator.IS_IMPLEMENTED_PROFILENAME, IsImplemented.ToString(CultureInfo.InvariantCulture), SensorName);

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " ShowNotReady: " + ShowNotReady);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, OCSimulator.SHOW_NOT_READY_PROFILENAME, ShowNotReady.ToString(CultureInfo.InvariantCulture), SensorName);

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " NotReadyDelay: " + NotReadyDelay);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, OCSimulator.NOT_READY_DELAY_PROFILENAME, NotReadyDelay.ToString(CultureInfo.InvariantCulture), SensorName);

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " Value Cycle Time: " + ValueCycleTime);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, OCSimulator.VALUE_CYCLE_TIME_PROFILE_NAME, ValueCycleTime.ToString(CultureInfo.InvariantCulture), SensorName);

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " Override: " + Override);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, OCSimulator.OVERRIDE_PROFILENAME, Override.ToString(CultureInfo.InvariantCulture), SensorName);

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " OverrideValue: " + OverrideValue);
            driverProfile.WriteValue(OCSimulator.DRIVER_PROGID, OCSimulator.OVERRIDE_VALUE_PROFILENAME, OverrideValue.ToString(CultureInfo.InvariantCulture), SensorName);

            OCSimulator.TL.LogMessage("Sensor.WriteProfile", SensorName + " Completed writing profile values");
        }
    }
}
