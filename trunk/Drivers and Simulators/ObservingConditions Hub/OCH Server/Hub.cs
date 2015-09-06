//
// ================
// Shared Resources
// ================
//
// This class is a container for all shared resources that may be needed
// by the drivers served by the Local Server. 
//
// NOTES:
//
//	* ALL DECLARATIONS MUST BE STATIC HERE!! INSTANCES OF THIS CLASS MUST NEVER BE CREATED!
//
// Written by:	Bob Denny	29-May-2007
// Modified by Chris Rowland and Peter Simpson to hamdle multiple hardware devices March 2011
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.DriverAccess;
namespace ASCOM.Simulator
{
    /// <summary>
    /// The resources shared by all drivers and devices, in this example it's a serial port with a shared SendMessage method
    /// an idea for locking the message and handling connecting is given.
    /// In reality extensive changes will probably be needed.
    /// Multiple drivers means that several applications connect to the same hardware device, aka a hub.
    /// Multiple devices means that there are more than one instance of the hardware, such as two focusers.
    /// In this case there needs to be multiple instances of the hardware connector, each with it's own connection count.
    /// </summary>
    public static class Hub
    {
        #region Public variables and constants

        public const string DRIVER_PROGID = "ASCOM.OCH.ObservingConditions";
        public const string DRIVER_DISPLAY_NAME = "ASCOM Observing Conditions Hub (OCH)"; // Driver description that displays in the ASCOM Chooser.
        public const string DEVICE_TYPE = "ObservingConditions";
        public const string SWITCH_DEVICE_NAME = "Switch";
        public const string OBSERVINGCONDITIONS_DEVICE_NAME = "ObservingConditions";
        public const string NO_DEVICE_PROGID = "";

        public static TraceLoggerPlus TL;
        public static bool DebugTraceState;

        // Setup dialogue configuration variables
        public static bool TraceState;
        public static int CacheTime;

        // List of valid ObservingConditions properties
        public static List<string> ValidProperties = new List<string> { // Aray containing a list of all valid properties
            PROPERTY_AVERAGEPERIOD,
            PROPERTY_CLOUDCOVER,
            PROPERTY_DEWPOINT,
            PROPERTY_HUMIDITY,
            PROPERTY_PRESSURE,
            PROPERTY_RAINRATE,
            PROPERTY_SKYBRIGHTNESS,
            PROPERTY_SKYQUALITY,
            PROPERTY_SKYSEEING,
            PROPERTY_SKYTEMPERATURE,
            PROPERTY_TEMPERATURE,
            PROPERTY_WINDDIRECTION,
            PROPERTY_WINDGUST,
            PROPERTY_WINDSPEED };

        // Hub simulator information
        public static Dictionary<string, double> SimulatorDefaultLowValues = new Dictionary<string, double>()
        {
            {"AveragePeriod", 0.0},
            {"CloudCover", 0.0},
            {"DewPoint", 0.0},
            {"Humidity", 50.0},
            {"Pressure", 1020.5},
            {"RainRate", 0.0},
            {"SkyBrightness", 85.0},
            {"SkyQuality", 18.0},
            {"SkySeeing", 0.85},
            {"SkyTemperature", -28.0},
            {"Temperature", 5.4},
            {"WindDirection", 174.4},
            {"WindGust", 2.34},
            {"WindSpeed", 0.29}
        };
        public static Dictionary<string, double> SimulatorDefaultHighValues = new Dictionary<string, double>()
        {
            {"AveragePeriod", 0.0},
            {"CloudCover", 5.0},
            {"DewPoint", 3.0},
            {"Humidity", 55.0},
            {"Pressure", 1032.5},
            {"RainRate", 0.0},
            {"SkyBrightness", 95.0},
            {"SkyQuality", 20.0},
            {"SkySeeing", 1.35},
            {"SkyTemperature", -25.0},
            {"Temperature", 8.8},
            {"WindDirection", 253.8},
            {"WindGust", 5.45},
            {"WindSpeed", 2.38}
        };

        #endregion

        #region Public Enums

        public enum ConnectionType
        {
            None,
            Simulation,
            Real
        }

        public enum DeviceType
        {
            ObservingConditions,
            Switch
        }

        #endregion

        #region Private variables and constants

        // private constants
        private const string TRACE_LEVEL_PROFILENAME = "Trace Level"; private const string TRACE_LEVEL_DEFAULT = "true";
        private const string DEBUG_TRACE_PROFILENAME = "Include Debug Trace"; private const string DEBUG_TRACE_DEFAULT = "true";
        private const string CACHE_TIME_PROFILENAME = "Cache Time"; private const int CACHE_TIME_DEFAULT = 500;

        // Cache management constants
        private const string CACHE_ATPARK = "AtPark";
        private static string[] CACHE_ITEMS = { CACHE_ATPARK };

        // Valid properties constants
        private const string PROPERTY_AVERAGEPERIOD = "AveragePeriod";
        private const string PROPERTY_CLOUDCOVER = "CloudCover";
        private const string PROPERTY_DEWPOINT = "DewPoint";
        private const string PROPERTY_HUMIDITY = "Humidity";
        private const string PROPERTY_PRESSURE = "Pressure";
        private const string PROPERTY_RAINRATE = "RainRate";
        private const string PROPERTY_SKYBRIGHTNESS = "SkyBrightness";
        private const string PROPERTY_SKYQUALITY = "SkyQuality";
        private const string PROPERTY_SKYSEEING = "SkySeeing";
        private const string PROPERTY_SKYTEMPERATURE = "SkyTemperature";
        private const string PROPERTY_TEMPERATURE = "Temperature";
        private const string PROPERTY_WINDDIRECTION = "WindDirection";
        private const string PROPERTY_WINDGUST = "WindGust";
        private const string PROPERTY_WINDSPEED = "WindSpeed";

        // Miscellaneous variables
        private static Util util; // Private variable to hold an ASCOM Utilities object
        private static int uniqueClientNumber = 0; // Unique number that increements on each call to UniqueClientNumber
        private readonly static object commLockObject = new object();
        private readonly static object connectLockObject = new object();
        private static ConcurrentDictionary<long, bool> connectStates;

        //Sensor information and devices
        public static Dictionary<string, Sensor> Sensors = new Dictionary<string, Sensor>();
        public static Dictionary<string, ObservingConditions> ObservingConditionsDevices = new Dictionary<string, ObservingConditions>();
        public static Dictionary<string, Switch> SwitchDevices = new Dictionary<string, Switch>();

        #endregion

        #region Initialiser
        /// <summary>
        /// Static initialiser to set up the objects we need at run time
        /// </summary>
        static Hub()
        {
            // Create sensor objects ready to be populated from the Profile
            // This must be done before reading the Profile
            foreach (string Property in ValidProperties)
            {
                Sensors.Add(Property, new Sensor(Property));
            }

            TL = new TraceLoggerPlus("", "OCH"); // Trace state is set in ReadProfile, immediately after nbeing read fomr the Profile
            ReadProfile(); // Read device configuration from the ASCOM Profile store
            TL.LogMessage("Hub", "Hub initialising");
            connectStates = new ConcurrentDictionary<long, bool>();
            util = new Util(); // Create an  ASCOM Utilities object
            TL.LogMessage("Hub", "Setting sensor initial values");
            foreach (string Property in ValidProperties)
            {
                Sensors[Property].SimCurrentValue = Sensors[Property].SimLowValue;
            }

            TL.LogMessage("Hub", "Hub initialisation complete.");
        }
        #endregion

        #region ASCOM Common Methods

        public static string Action(int clientNumber, string actionName, string actionParameters)
        {
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public static void CommandBlind(int clientNumber, string command)
        {
            CommandBlind(clientNumber, command, false);
        }

        public static void CommandBlind(int clientNumber, string command, bool raw)
        {
            lock (commLockObject)
            {
                if (raw)
                { // Send string as received and return result as received
                    if (DebugTraceState) TL.LogMessage(clientNumber, "CommandBlindRaw", "Sending: " + command);
                }
                else
                { // Pad with leading : and trainling # and strip trailing # from returned string
                    command = ":" + command + "#";
                    if (DebugTraceState) TL.LogMessage(clientNumber, "CommandBlind", "Sending: " + command);
                }
            }
        }

        public static bool CommandBool(int clientNumber, string command)
        {
            return CommandBool(clientNumber, command, false);
        }

        public static bool CommandBool(int clientNumber, string command, bool raw)
        {
            string returnMessage = "";
            lock (commLockObject)
            {
                if (raw)
                {
                    // Send supplied string
                    if (DebugTraceState) TL.LogMessage(clientNumber, "CommandBoolRaw", "Sending: " + command);
                    if (DebugTraceState) TL.LogMessage(clientNumber, "CommandBoolRaw", "Response: " + returnMessage);
                }
                else
                { // Pad supplied string with leading : and trailing # string
                    command = ":" + command + "#";
                    if (DebugTraceState) TL.LogMessage(clientNumber, "CommandBool", "Sending: " + command);
                    if (DebugTraceState) TL.LogMessage(clientNumber, "CommandBool", "Response: " + returnMessage);
                }

                return false;
            }
        }

        public static string CommandString(int clientNumber, string command)
        {
            return CommandString(clientNumber, command, false);
        }

        public static string CommandString(int clientNumber, string command, bool raw)
        {
            lock (commLockObject)
            {
                if (raw)
                { // Send string as received and return result as received
                    if (DebugTraceState) TL.LogMessage(clientNumber, "CommandStringRaw", "Sending: " + command);
                    return "";
                }
                else
                { // Pad with leading : and trainling # and strip trailing # from returned string
                    if (DebugTraceState) TL.LogMessage(clientNumber, "CommandString", "Sending: " + command);
                    return "";
                }
            }
        }

        public static bool IsConnected(int clientNumber)
        {
            if (DebugTraceState) TL.LogMessage("Hardware.Connected Get", "Number of connected devices: " + connectStates.Count + ", Returning: " + (connectStates.Count > 0).ToString());
            return connectStates.Count > 0;
        }

        public static int ConnectionCount
        {
            get { return connectStates.Count; }
        }

        public static void Connect(int clientNumber)
        {
            if (DebugTraceState) TL.LogMessage(clientNumber, "Connect", "Acquiring connection lock");
            lock (connectLockObject) // Esnure that only one connection attempt can happen at a time
            {
                TL.LogMessage(clientNumber, "Connect", "Has connection lock");
                if (IsConnected(clientNumber)) // If we are already connected then just log this 
                {
                    TL.LogMessage(clientNumber, "Connect", "Already connected, just incrementing connection count.");
                }
                else // We are not connected so connect now
                {
                    try
                    {
                        TL.LogMessage(clientNumber, "Connect", "Attempting to connect to devices");

                        // Work out which devices we actually need to connect to
                        foreach (KeyValuePair<string, Sensor> sensor in Sensors)
                        {
                            switch (sensor.Value.ProgID)
                            {
                                case NO_DEVICE_PROGID:
                                    // No action required as there is no device
                                    break;
                                case DRIVER_PROGID:
                                    // No action required as this is the Hub simulator
                                    break;
                                default: // Must be a real device so we need to connect to it
                                    switch (sensor.Value.DeviceType)
                                    {
                                        case DeviceType.ObservingConditions:
                                            if (!ObservingConditionsDevices.ContainsKey(sensor.Value.ProgID))
                                            {
                                                TL.LogMessage(clientNumber, "Connect", "Adding new ObservingConditions ProgID: " + sensor.Value.ProgID);
                                                ObservingConditionsDevices.Add(sensor.Value.ProgID, new ObservingConditions(sensor.Value.ProgID));
                                            }
                                            else
                                            {
                                                TL.LogMessage(clientNumber, "Connect", "Skipping this ObservingConditions ProgID, it already exists: " + sensor.Value.ProgID);
                                            }
                                            break;
                                        case DeviceType.Switch:
                                            if (!SwitchDevices.ContainsKey(sensor.Value.ProgID))
                                            {
                                                TL.LogMessage(clientNumber, "Connect", "Adding new Switch ProgID: " + sensor.Value.ProgID);
                                                SwitchDevices.Add(sensor.Value.ProgID, new Switch(sensor.Value.ProgID));
                                            }
                                            else
                                            {
                                                TL.LogMessage(clientNumber, "Connect", "Skipping this Switch ProgID, it already exists: " + sensor.Value.ProgID);
                                            }
                                            break;
                                        default:
                                            break;
                                    }

                                    break;
                            }
                        }

                        // Now try to connect to ObservingConditions devices
                        foreach (KeyValuePair<string, ObservingConditions> observingConditionsDevice in ObservingConditionsDevices)
                        {
                            TL.LogMessage(clientNumber, "Connect", "Connecting to: " + observingConditionsDevice.Key);
                            try
                            {
                                observingConditionsDevice.Value.Connected = true;
                                TL.LogMessage(clientNumber, "Connect", "Successfully connected to: " + observingConditionsDevice.Key);
                            }
                            catch (Exception ex)
                            {
                                TL.LogMessage(clientNumber, "Connect", "Failed to connect: " + ex.ToString());
                            }

                        }

                        // Now try to connect to Switch devices
                        foreach (KeyValuePair<string, Switch> switchDevice in SwitchDevices)
                        {
                            TL.LogMessage(clientNumber, "Connect", "Connecting to: " + switchDevice.Key);
                            try
                            {
                                switchDevice.Value.Connected = true;
                                TL.LogMessage(clientNumber, "Connect", "Successfully connected to: " + switchDevice.Key);
                            }
                            catch (Exception ex)
                            {
                                TL.LogMessage(clientNumber, "Connect", "Failed to connect: " + ex.ToString());
                            }

                        }

                        bool notAlreadyPresent = connectStates.TryAdd(clientNumber, true);
                        TL.LogMessage(clientNumber, "Connect", "Successfully connected, AlreadyConnected: " + (!notAlreadyPresent).ToString());
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf(clientNumber, "Connect", "Exception: " + ex.ToString());
                        throw;
                    }
                }
            }
        }

        public static void Disconnect(int clientNumber)
        {
            bool lastValue;
            bool successfullyRemoved = connectStates.TryRemove(clientNumber, out lastValue);
            TL.LogMessage("Hardware.Connected Set", "Set Connected to: False, Successfully removed: " + successfullyRemoved.ToString());

            // Now try to disconnect ObservingConditions devices
            foreach (KeyValuePair<string, ObservingConditions> observingConditionsDevice in ObservingConditionsDevices)
            {
                TL.LogMessage(clientNumber, "Disconnect", "Disconnecting: " + observingConditionsDevice.Key);
                try { observingConditionsDevice.Value.Connected = false; } catch (Exception ex) { TL.LogMessageCrLf(clientNumber, "Disconnect", "Connected = false: " + ex.ToString()); }
                try { observingConditionsDevice.Value.Dispose(); } catch (Exception ex) { TL.LogMessageCrLf(clientNumber, "Disconnect", "Dispose(): " + ex.ToString()); }
                TL.LogMessage(clientNumber, "Disconnect", "Successfully disconnected: " + observingConditionsDevice.Key);
            }
            ObservingConditionsDevices.Clear();

            // Now try to disconnect Switch devices
            foreach (KeyValuePair<string, Switch> switchDevice in SwitchDevices)
            {
                TL.LogMessage(clientNumber, "Disconnect", "Disconnecting: " + switchDevice.Key);
                try { switchDevice.Value.Connected = false; } catch (Exception ex) { TL.LogMessageCrLf(clientNumber, "Disconnect", "Connected = false: " + ex.ToString()); }
                try { switchDevice.Value.Dispose(); } catch (Exception ex) { TL.LogMessageCrLf(clientNumber, "Disconnect", "Dispose(): " + ex.ToString()); }
                TL.LogMessage(clientNumber, "Disconnect", "Successfully disconnected: " + switchDevice.Key);
            }
            SwitchDevices.Clear();

        }

        public static string Description(int clientNumber)
        {
            TL.LogMessage(clientNumber, "Description", DRIVER_DISPLAY_NAME);
            return DRIVER_DISPLAY_NAME;
        }

        public static string DriverInfo(int clientNumber)
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string driverInfo = DRIVER_DISPLAY_NAME + ". Version: " + version.ToString();
            TL.LogMessage(clientNumber, "DriverInfo", driverInfo);
            return driverInfo;
        }

        public static string DriverVersion(int clientNumber)
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
            TL.LogMessage(clientNumber, "DriverVersion", driverVersion);
            return driverVersion;
        }

        public static short InterfaceVersion(int clientNumber)
        {
            short interfaceVersion = 3;
            TL.LogMessage(clientNumber, "InterfaceVersion", interfaceVersion.ToString());
            return interfaceVersion;
        }

        public static string Name(int clientNumber)
        {
            string name = DRIVER_DISPLAY_NAME;
            TL.LogMessage(clientNumber, "Name", name);
            return name;
        }

        public static void SetupDialog(int clientNumber)
        {
            TL.LogMessage(clientNumber, "SetupDialog", "Connected: " + IsConnected(clientNumber).ToString());
            if (IsConnected(clientNumber))
            {
                MessageBox.Show("Hub is connected, setup parameters cannot be changed, please press OK");
            }
            else
            {
                using (SetupDialogForm F = new SetupDialogForm())
                {
                    var result = F.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                    }
                }
            }
        }

        public static ArrayList SupportedActions(int clientNumber)
        {
            TL.LogMessage(clientNumber, "SupportedActions", "Returning empty arraylist");
            return new ArrayList();
        }

        #endregion

        #region ASCOM ObservingConditions Methods

        public static double AveragePeriod(int clientNumber)
        {
            double averagePeriod = GetDouble(PROPERTY_AVERAGEPERIOD);
            TL.LogMessage(clientNumber, "AveragePeriod", averagePeriod.ToString());
            return averagePeriod;
        }

        public static void AveragePeriodSet(int clientNumber, double value)
        {
            Sensors[PROPERTY_AVERAGEPERIOD].SimCurrentValue = value;
            TL.LogMessage(clientNumber, "AveragePeriodSet", value.ToString());
        }

        public static double CloudCover(int clientNumber)
        {
            double cloudCover = GetDouble(PROPERTY_CLOUDCOVER);
            TL.LogMessage(clientNumber, "CloudCover", cloudCover.ToString());
            return cloudCover;
        }

        public static double DewPoint(int clientNumber)
        {
            double dewPoint = GetDouble(PROPERTY_DEWPOINT); ;
            TL.LogMessage(clientNumber, "DewPoint", dewPoint.ToString());
            return dewPoint;
        }

        public static double Humidity(int clientNumber)
        {
            double humidity = GetDouble(PROPERTY_HUMIDITY); ;
            TL.LogMessage(clientNumber, "Humidity", humidity.ToString());
            return humidity;
        }

        public static double Pressure(int clientNumber)
        {
            double pressure = GetDouble(PROPERTY_PRESSURE); ;
            TL.LogMessage(clientNumber, "Pressure", pressure.ToString());
            return pressure;
        }

        public static double RainRate(int clientNumber)
        {
            double rainRate = GetDouble(PROPERTY_RAINRATE); ;
            TL.LogMessage(clientNumber, "RainRate", rainRate.ToString());
            return rainRate;
        }

        public static string SensorDescription(int clientNumber, string PropertyName)
        {
            if (IsValidProperty(PropertyName))
            {
                switch (Sensors[PropertyName].DeviceMode)
                {
                    case ConnectionType.None:
                        throw new MethodNotImplementedException(PropertyName + " is not implemented in the ObservingConditions hub");

                    case ConnectionType.Simulation:
                        return "ObservingConditions hub simulated " + PropertyName + " sensor";

                    case ConnectionType.Real:
                        switch (Sensors[PropertyName].DeviceType)
                        {
                            case DeviceType.ObservingConditions:
                                string sensorDescription = ObservingConditionsDevices[Sensors[PropertyName].ProgID].SensorDescription(PropertyName);
                                TL.LogMessage(clientNumber, "SensorDescription", "Got description: " + sensorDescription + " from device " + Sensors[PropertyName].ProgID + " method " + PropertyName);
                                return sensorDescription;

                            case DeviceType.Switch:
                                string switchDescription;
                                if (SwitchDevices[Sensors[PropertyName].ProgID].InterfaceVersion <= 1) // Switch V1 does not have a switch description so get the switch name instead
                                {
                                    switchDescription = SwitchDevices[Sensors[PropertyName].ProgID].GetSwitchName((short)Sensors[PropertyName].SwitchNumber);
                                }
                                else // Switch V2 does have a switch description
                                {
                                    switchDescription = SwitchDevices[Sensors[PropertyName].ProgID].GetSwitchDescription((short)Sensors[PropertyName].SwitchNumber);
                                }
                                TL.LogMessage(clientNumber, "SensorDescription", "Got description: " + switchDescription + " from device " + Sensors[PropertyName].ProgID + " switch number " + Sensors[PropertyName].SwitchNumber);
                                return switchDescription;

                            default:
                                MessageBox.Show("GetDouble: Unknown DeviceType Enum value: " + Sensors[PropertyName].DeviceType.ToString());
                                break;
                        }
                        break;
                    default:
                        MessageBox.Show("SensorDescription: Unknown DeviceMode Enum value: " + Sensors[PropertyName].DeviceMode.ToString());
                        break;
                }

                string propertyDescription = PropertyName + " description";
                TL.LogMessage(clientNumber, "SensorDescription", PropertyName + ": " + propertyDescription.ToString());
                return propertyDescription;
            }
            else
            {
                TL.LogMessage(clientNumber, "SensorDescription", PropertyName + " is invalid, throwing InvalidValueException");
                throw new InvalidValueException("SensorDescription: \"" + PropertyName + "\" is not a valid ObservingConditions property");
            }
        }

        public static void Refresh(int clientNumber)
        {
            // No action required for devices that are not connected
            // No action required for devices that are simulated
            ;
        }

        public static double SkyBrightness(int clientNumber)
        {
            double skyBrightness = GetDouble(PROPERTY_SKYBRIGHTNESS); ;
            TL.LogMessage(clientNumber, "SkyBrightness", skyBrightness.ToString());
            return skyBrightness;
        }

        public static double SkyQuality(int clientNumber)
        {
            double skyQuality = GetDouble(PROPERTY_SKYQUALITY); ;
            TL.LogMessage(clientNumber, "SkyQuality", skyQuality.ToString());
            return skyQuality;
        }

        public static double SkySeeing(int clientNumber)
        {
            double skySeeing = GetDouble(PROPERTY_SKYSEEING); ;
            TL.LogMessage(clientNumber, "SkySeeing", skySeeing.ToString());
            return skySeeing;
        }

        public static double SkyTemperature(int clientNumber)
        {
            double skyTemperature = GetDouble(PROPERTY_SKYTEMPERATURE); ;
            TL.LogMessage(clientNumber, "SkyTemperature", skyTemperature.ToString());
            return skyTemperature;
        }

        public static double Temperature(int clientNumber)
        {
            double temperature = GetDouble(PROPERTY_TEMPERATURE); ;
            TL.LogMessage(clientNumber, "Temperature", temperature.ToString());
            return temperature;
        }

        public static double TimeSinceLastUpdate(int clientNumber, string PropertyName)
        {
            if (PropertyName == "") // Return the most recent update time of any sensor
            {
                // For now just return 0.0 as this is true for simulated values
                double timeSinceLastUpdate = 0.0;
                TL.LogMessage(clientNumber, "TimeSinceLastUpdate", "Most recent sensor update time: " + timeSinceLastUpdate.ToString());
                return timeSinceLastUpdate;
            }
            else
            {
                if (IsValidProperty(PropertyName))
                {
                    // For now just return 0.0 as this is true for simulated values
                    double timeSinceLastUpdate = 0.0;
                    TL.LogMessage(clientNumber, "TimeSinceLastUpdate", PropertyName + ": " + timeSinceLastUpdate.ToString());
                    return timeSinceLastUpdate;
                }
                else
                {
                    TL.LogMessage(clientNumber, "TimeSinceLastUpdate", "\"" + PropertyName + "\" is invalid, throwing InvalidValueException");
                    throw new InvalidValueException("TimeSinceLastUpdate: \"" + PropertyName + "\" is not a valid ObservingConditions property");
                }
            }
        }

        public static double WindDirection(int clientNumber)
        {
            double windDirection = GetDouble(PROPERTY_WINDDIRECTION); ;
            TL.LogMessage(clientNumber, "WindDirection", windDirection.ToString());
            return windDirection;
        }

        public static double WindGust(int clientNumber)
        {
            double windGust = GetDouble(PROPERTY_WINDGUST); ;
            TL.LogMessage(clientNumber, "WindGust", windGust.ToString());
            return windGust;
        }

        public static double WindSpeed(int clientNumber)
        {
            double windSpeed = GetDouble(PROPERTY_WINDSPEED); ;
            TL.LogMessage(clientNumber, "WindSpeed", windSpeed.ToString());
            return windSpeed;
        }

        #endregion

        #region Basic infrastructure functions
        /// <summary>
        /// Returns a unique client numnber to the calling instance
        /// </summary>
        public static int GetUniqueClientNumber()
        {
            Interlocked.Increment(ref uniqueClientNumber);
            TL.LogMessage("UniqueClientNumber", "Generated new ID: " + uniqueClientNumber.ToString());
            return uniqueClientNumber;
        }

        internal static bool IsValidProperty(string PropertyName)
        {
            return ValidProperties.Contains(PropertyName.Trim(), StringComparer.OrdinalIgnoreCase); // Make the test case insensitive as well as leading and trailing space insensitive
        }

        private static double GetDouble(string PropertyName)
        {
            TL.LogMessage("GetDouble", PropertyName + " device mode: " + Sensors[PropertyName].DeviceMode.ToString());
            switch (Sensors[PropertyName].DeviceMode)
            {
                case ConnectionType.None:
                    throw new PropertyNotImplementedException(PropertyName, false);

                case ConnectionType.Simulation:
                    return Sensors[PropertyName].SimCurrentValue;

                case ConnectionType.Real:
                    switch (Sensors[PropertyName].DeviceType)
                    {
                        case DeviceType.ObservingConditions:
                            Type type = typeof(ObservingConditions);
                            PropertyInfo propertyInfo = type.GetProperty(PropertyName);
                            double observingConditionsValue = (double)propertyInfo.GetValue(ObservingConditionsDevices[Sensors[PropertyName].ProgID], null);
                            TL.LogMessage("GetDouble", "Got value: " + observingConditionsValue + " from device " + Sensors[PropertyName].ProgID + " method " + PropertyName);
                            return observingConditionsValue;

                        case DeviceType.Switch:
                            double switchValue;

                            if (SwitchDevices[Sensors[PropertyName].ProgID].InterfaceVersion <= 1) // Switch V1 does not have a switch description so get the switch name instead
                            {
                                bool switchBool = SwitchDevices[Sensors[PropertyName].ProgID].GetSwitch((short)Sensors[PropertyName].SwitchNumber);
                                switchValue = Convert.ToDouble(switchBool);
                                TL.LogMessage("GetDouble", "Got value: " + switchValue + " from Switch V1 device " + Sensors[PropertyName].ProgID + " switch number " + Sensors[PropertyName].SwitchNumber + ", device returned: " + switchBool.ToString());
                            }
                            else // Switch V2 does have a switch description
                            {
                                switchValue = SwitchDevices[Sensors[PropertyName].ProgID].GetSwitchValue((short)Sensors[PropertyName].SwitchNumber);
                                TL.LogMessage("GetDouble", "Got value: " + switchValue + " from Switch V2 device " + Sensors[PropertyName].ProgID + " switch number " + Sensors[PropertyName].SwitchNumber);
                            }

                            return switchValue;

                        default:
                            MessageBox.Show("GetDouble: Unknown DeviceType Enum value: " + Sensors[PropertyName].DeviceType.ToString());
                            break;
                    }
                    break;
                default:
                    MessageBox.Show("GetDouble: Unknown DeviceMode Enum value: " + Sensors[PropertyName].DeviceMode.ToString());
                    break;
            }
            return 0.0;
        }

        #endregion

        #region Profile management
        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        public static void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = DEVICE_TYPE;
                TraceState = Convert.ToBoolean(driverProfile.GetValue(DRIVER_PROGID, TRACE_LEVEL_PROFILENAME, string.Empty, TRACE_LEVEL_DEFAULT));
                TL.Enabled = TraceState; // Set the logging state immediately after this has been retrieved from Profile
                DebugTraceState = Convert.ToBoolean(driverProfile.GetValue(DRIVER_PROGID, DEBUG_TRACE_PROFILENAME, string.Empty, DEBUG_TRACE_DEFAULT));
                CacheTime = Convert.ToInt32(driverProfile.GetValue(DRIVER_PROGID, CACHE_TIME_PROFILENAME, string.Empty, CACHE_TIME_DEFAULT.ToString()));
                // Initialise the sensor collection
                foreach (string Property in ValidProperties)
                {
                    TL.LogMessage("ReadProfile", "Reading profile for: " + Property);
                    Sensors[Property].ReadProfile(driverProfile);
                }

            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        public static void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = DEVICE_TYPE;
                driverProfile.WriteValue(DRIVER_PROGID, TRACE_LEVEL_PROFILENAME, TraceState.ToString());
                driverProfile.WriteValue(DRIVER_PROGID, DEBUG_TRACE_PROFILENAME, DebugTraceState.ToString());
                driverProfile.WriteValue(DRIVER_PROGID, CACHE_TIME_PROFILENAME, CacheTime.ToString());
                foreach (string Property in ValidProperties)
                {
                    TL.LogMessage("WriteProfile", "Writing profile for: " + Property);
                    Sensors[Property].WriteProfile(driverProfile);
                }
            }
        }
        #endregion

    }

}
