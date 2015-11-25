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
using System.Runtime.InteropServices;
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
        #region Public constants

        public const string DRIVER_PROGID = "ASCOM.OCH.ObservingConditions";
        public const string DRIVER_DISPLAY_NAME = "ASCOM Observing Conditions Hub (OCH)"; // Driver description that displays in the ASCOM Chooser.
        public const string DEVICE_TYPE = "ObservingConditions";
        public const string SWITCH_DEVICE_NAME = "Switch";
        public const string OBSERVING_CONDITIONS_DEVICE_TYPE = "ObservingConditions";
        public const string NO_DEVICE_PROGID = "";
        public const int MAX_QUERIES_PER_PERIOD = 60; // Maximum number of device queries in 1 average period
        public const double BAD_VALUE = double.NaN; // This must remain as Double.NaN because Double.IsNaN is used within the code to test for bad values
        public static readonly DateTime BAD_DATETIME = DateTime.MinValue; // Can't be a const because DateTime.MinValue is not a constant, a read only variable is close though!
        public const string NOT_CONNECTED_MESSAGE = DRIVER_DISPLAY_NAME + " is not connected.";

        // Profile constants
        public const string TRACE_LEVEL_PROFILENAME = "Trace Level"; private const string TRACE_LEVEL_DEFAULT = "False";
        public const string DEBUG_TRACE_PROFILENAME = "Include Debug Trace"; private const string DEBUG_TRACE_DEFAULT = "False";
        public const string CONNECT_TO_DRIVERS_PROFILENAME = "Connect To Drivers"; private const string CONNECT_TO_DRIVERS_DEFAULT = "False";
        public const string AVERAGE_PERIOD_PROFILENAME = "Average Period"; public const string AVERAGE_PERIOD_DEFAULT = "0.0";
        public const string NUMBER_OF_READINGS_PROFILENAME = "Number Of Readings"; public const string NUMBER_OF_READINGS_DEFAULT = "10";
        public const string OVERRIDE_UI_SAFETY_LIMITS_PROFILENAME = "Override UI Safety Limits"; public const string OVERRIDE_UI_SAFETY_LIMITS_DEFAULT = "False";

        // Setup dialogue device selection combo box drop down constants 
        public const string SWITCH_NAME_PREFIX = "SWITCH"; // Prefix used when displaying a switch device in the Setup dropdown list 
        public const string OBSERVING_CONDITIONS_NAME_PREFIX = "OBSCON"; // Prefix used when displaying an observing conditions device in the Setup dropdown list 
        public const string NO_DEVICE_DESCRIPTION = "No device";

        // Valid properties constants
        public const string PROPERTY_AVERAGEPERIOD = "AveragePeriod";
        public const string PROPERTY_CLOUDCOVER = "CloudCover";
        public const string PROPERTY_DEWPOINT = "DewPoint";
        public const string PROPERTY_HUMIDITY = "Humidity";
        public const string PROPERTY_PRESSURE = "Pressure";
        public const string PROPERTY_RAINRATE = "RainRate";
        public const string PROPERTY_SKYBRIGHTNESS = "SkyBrightness";
        public const string PROPERTY_SKYQUALITY = "SkyQuality";
        public const string PROPERTY_STARFWHM = "StarFWHM";
        public const string PROPERTY_SKYTEMPERATURE = "SkyTemperature";
        public const string PROPERTY_TEMPERATURE = "Temperature";
        public const string PROPERTY_WINDDIRECTION = "WindDirection";
        public const string PROPERTY_WINDGUST = "WindGust";
        public const string PROPERTY_WINDSPEED = "WindSpeed";

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
            PROPERTY_STARFWHM,
            PROPERTY_SKYTEMPERATURE,
            PROPERTY_TEMPERATURE,
            PROPERTY_WINDDIRECTION,
            PROPERTY_WINDGUST,
            PROPERTY_WINDSPEED };

        // List of all sensors that can be queried from connected devices
        // AveragePeriod is missing from the list because it is handled by this hub rather than by one of the drivers it is proxying
        public static List<string> ValidSensors = new List<string> {
            PROPERTY_CLOUDCOVER,
            PROPERTY_DEWPOINT,
            PROPERTY_HUMIDITY,
            PROPERTY_PRESSURE,
            PROPERTY_RAINRATE,
            PROPERTY_SKYBRIGHTNESS,
            PROPERTY_SKYQUALITY,
            PROPERTY_STARFWHM,
            PROPERTY_SKYTEMPERATURE,
            PROPERTY_TEMPERATURE,
            PROPERTY_WINDDIRECTION,
            PROPERTY_WINDGUST,
            PROPERTY_WINDSPEED };

        #endregion

        #region Public variables

        public static TraceLoggerPlus TL;
        public static bool DebugTraceState;

        // Setup dialogue configuration variables
        public static bool TraceState;
        public static bool ConnectToDrivers;
        public static double averagePeriod;
        public static int numberOfMeasurementsPerAveragePeriod;
        public static bool overrideUISafetyLimits;

        //Sensor information and devices
        public static Dictionary<string, Sensor> Sensors = new Dictionary<string, Sensor>();
        public static Dictionary<string, ObservingConditions> ObservingConditionsDevices = new Dictionary<string, ObservingConditions>();
        public static Dictionary<string, Switch> SwitchDevices = new Dictionary<string, Switch>();

        #endregion

        #region Public Enums and Structs

        public enum ConnectionType
        {
            None,
            Real
        }

        public enum DeviceType
        {
            ObservingConditions,
            Switch
        }

        public struct TimeValue
        {
            public TimeValue(DateTime obsTime, double sensValue)
            {
                ObservationTime = obsTime;
                SensorValue = sensValue;
            }
            public DateTime ObservationTime;
            public double SensorValue;
        }

        #endregion

        #region Private variables and constants

        // Miscellaneous variables
        private static int uniqueClientNumber = 0; // Unique number that increements on each call to UniqueClientNumber
        private readonly static object connectLockObject = new object();
        private readonly static object deviceAccessLockObject = new object();
        private static ConcurrentDictionary<long, bool> connectStates;
        private static DateTime timeOfLastUpdate = BAD_DATETIME; // Iniitalise to a bad value so we can tell whether a sensor has ever been accessed
        private static System.Timers.Timer averagePeriodTimer;

        #endregion

        #region Initialiser

        /// <summary>
        /// Static initialiser to set up the objects we need at run time
        /// </summary>
        static Hub()
        {
            try
            {
                // Create sensor objects ready to be populated from the Profile
                // This must be done before reading the Profile
                foreach (string Property in ValidSensors)
                {
                    Sensors.Add(Property, new Sensor(Property));
                }

                TL = new TraceLoggerPlus("", "OCH"); // Trace state is set in ReadProfile, immediately after being read from the Profile
                ReadProfile(); // Read device configuration from the ASCOM Profile store
                TL.LogMessage("Hub", "Hub initialising");
                connectStates = new ConcurrentDictionary<long, bool>();

                // Set up average period timer
                averagePeriodTimer = new System.Timers.Timer();
                averagePeriodTimer.Elapsed += AveragePeriodTimer_Elapsed;

                TL.LogMessage("Hub", "Hub initialisation complete.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error initialising the Observing Conditions Hub", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ASCOM Common Methods

        public static string Action(int clientNumber, string actionName, string actionParameters)
        {
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public static void CommandBlind(int clientNumber, string command, bool raw)
        {
            {
                throw new MethodNotImplementedException("CommandBlind");
            }
        }

        public static bool CommandBool(int clientNumber, string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandBool");
        }

        public static string CommandString(int clientNumber, string command, bool raw)
        {
            throw new MethodNotImplementedException("CommandString");
        }

        public static void Connect(int clientNumber)
        {
            if (DebugTraceState) TL.LogMessage(clientNumber, "Connect", "Acquiring connection lock");

            lock (connectLockObject) // Ensure that only one connection attempt can happen at a time
            {
                TL.LogMessage(clientNumber, "Connect", "Has connection lock");
                try
                {
                    if (!IsHardwareConnected(clientNumber)) // We are not physically connected so connect all devices now
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
                                default: // Must be a real device so we need to connect to it
                                    switch (sensor.Value.DeviceType)
                                    {
                                        case DeviceType.ObservingConditions:
                                            if (!ObservingConditionsDevices.ContainsKey(sensor.Value.ProgID))
                                            {
                                                TL.LogMessage(clientNumber, "Connect", "Adding new ObservingConditions ProgID: " + sensor.Value.ProgID);
                                                try
                                                {
                                                    ObservingConditionsDevices.Add(sensor.Value.ProgID, new ObservingConditions(sensor.Value.ProgID));
                                                }
                                                catch (Exception ex)
                                                {
                                                    TL.LogMessageCrLf(clientNumber, "Connect", "Exception adding ObservingConditions Device " + sensor.Value.ProgID + ": " + ex.ToString());
                                                    MessageBox.Show("Unable to connect to ObservingConditions device " + sensor.Value.ProgID + ": " + ex.Message);
                                                }
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
                                                try
                                                {
                                                    SwitchDevices.Add(sensor.Value.ProgID, new Switch(sensor.Value.ProgID));
                                                }
                                                catch (Exception ex)
                                                {
                                                    TL.LogMessageCrLf(clientNumber, "Connect", "Exception adding Switch Device " + sensor.Value.ProgID + ": " + ex.ToString());
                                                    MessageBox.Show("Unable to connect to Switch device " + sensor.Value.ProgID + ": " + ex.Message);
                                                }
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
                                bool notAlreadyPresent = connectStates.TryAdd(clientNumber, true); // Add this client to the list of connected clients
                                TL.LogMessage(clientNumber, "Connect", "Successfully connected to: " + observingConditionsDevice.Key.ToString() + ", AlreadyConnected: " + (!notAlreadyPresent).ToString());
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
                                bool notAlreadyPresent = connectStates.TryAdd(clientNumber, true); // Add this client to the list of connected clients
                                TL.LogMessage(clientNumber, "Connect", "Successfully connected to: " + switchDevice.Key.ToString() + ", AlreadyConnected: " + (!notAlreadyPresent).ToString());
                            }
                            catch (Exception ex)
                            {
                                TL.LogMessage(clientNumber, "Connect", "Failed to connect: " + ex.ToString());
                            }

                        }

                        ConfigureAveragePeriodTimer();
                    }
                    else // We are already physically connected so just add this client
                    {
                        if (IsClientConnected(clientNumber)) // If this client is already connected then just log this 
                        {
                            TL.LogMessage(clientNumber, "Connect", "This client is already connected - no action required.");
                        }
                        else // This client is not connected so connect now
                        {
                            bool notAlreadyPresent = connectStates.TryAdd(clientNumber, true); // Add this client to the list of connected clients
                            TL.LogMessage(clientNumber, "Connect", "Successfully added client to the connected clients list, AlreadyConnected: " + (!notAlreadyPresent).ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf(clientNumber, "Connect", "Unhandled Exception: " + ex.ToString());
                    throw;
                }
            }
        }

        public static void Disconnect(int clientNumber)
        {
            bool lastValue;
            bool successfullyRemoved = connectStates.TryRemove(clientNumber, out lastValue);
            TL.LogMessage(clientNumber, "Disconnect", "Successfully removed entry from list of connected states: " + successfullyRemoved.ToString());

            if (ConnectionCount == 0) // The last connection has dropped so stop the timer and disconnect connected devices
            {
                averagePeriodTimer.Stop(); // Stop the timer and wait for anything in progress to complete
                Thread.Sleep(100);

                // Now try to disconnect ObservingConditions devices
                foreach (KeyValuePair<string, ObservingConditions> observingConditionsDevice in ObservingConditionsDevices)
                {
                    TL.LogMessage(clientNumber, "Disconnect", "Disconnecting: " + observingConditionsDevice.Key);
                    try { observingConditionsDevice.Value.Connected = false; } catch (Exception ex) { TL.LogMessageCrLf(clientNumber, "Disconnect", "Connected = false: " + ex.ToString()); }
                    try { observingConditionsDevice.Value.Dispose(); } catch (Exception ex) { TL.LogMessageCrLf(clientNumber, "Disconnect", "Dispose(): " + ex.ToString()); }
                    TL.LogMessage(clientNumber, "Disconnect", "Disconnected: " + observingConditionsDevice.Key);
                }
                ObservingConditionsDevices.Clear();

                // Now try to disconnect Switch devices
                foreach (KeyValuePair<string, Switch> switchDevice in SwitchDevices)
                {
                    TL.LogMessage(clientNumber, "Disconnect", "Disconnecting: " + switchDevice.Key);
                    try { switchDevice.Value.Connected = false; } catch (Exception ex) { TL.LogMessageCrLf(clientNumber, "Disconnect", "Connected = false: " + ex.ToString()); }
                    try { switchDevice.Value.Dispose(); } catch (Exception ex) { TL.LogMessageCrLf(clientNumber, "Disconnect", "Dispose(): " + ex.ToString()); }
                    TL.LogMessage(clientNumber, "Disconnect", "Disconnected: " + switchDevice.Key);
                }
                SwitchDevices.Clear();

                TL.LogMessage(clientNumber, "Disconnect", "Clearing time of last update and sensors");
                timeOfLastUpdate = BAD_DATETIME; // Set the last update time to a bad value

                // Clear the sensor values ready to start again if reconnected
                foreach (string PropertyName in ValidSensors)
                {
                    Sensors[PropertyName].Readings.Clear();
                    Sensors[PropertyName].LastPeriodAverage = BAD_VALUE;
                    Sensors[PropertyName].TimeOfLastUpdate = BAD_DATETIME;
                }
            }
        }

        public static string Description(int clientNumber)
        {
            CheckConnected("Description");

            TL.LogMessage(clientNumber, "Description", DRIVER_DISPLAY_NAME);
            return DRIVER_DISPLAY_NAME;
        }

        public static string DriverInfo(int clientNumber)
        {
            CheckConnected("DriverInfo");

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string driverInfo = DRIVER_DISPLAY_NAME + ". Version: " + version.ToString();
            TL.LogMessage(clientNumber, "DriverInfo", driverInfo);
            return driverInfo;
        }

        public static string DriverVersion(int clientNumber)
        {
            CheckConnected("DriverVersion");

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
            TL.LogMessage(clientNumber, "DriverVersion", driverVersion);
            return driverVersion;
        }

        public static short InterfaceVersion(int clientNumber)
        {
            CheckConnected("InterfaceVersion");

            short interfaceVersion = 1;
            TL.LogMessage(clientNumber, "InterfaceVersion", interfaceVersion.ToString());
            return interfaceVersion;
        }

        public static string Name(int clientNumber)
        {
            CheckConnected("Name");

            string name = DRIVER_DISPLAY_NAME;
            TL.LogMessage(clientNumber, "Name", name);
            return name;
        }

        public static void SetupDialog(int clientNumber)
        {
            TL.LogMessage(clientNumber, "SetupDialog", "Connected: " + IsHardwareConnected(clientNumber).ToString());
            if (IsHardwareConnected(clientNumber))
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

        public static double AveragePeriodGet(int clientNumber)
        {
            CheckConnected("AveragePeriodGet");

            TL.LogMessage(clientNumber, "AveragePeriodGet", averagePeriod.ToString());
            return averagePeriod;
        }

        public static void AveragePeriodSet(int clientNumber, double value)
        {
            CheckConnected("AveragePeriodSet");

            if (value >= 0.0)
            {
                averagePeriod = value;

                ConfigureAveragePeriodTimer();
                TL.LogMessage(clientNumber, "AveragePeriodSet", value.ToString());
            }
            else
            {
                TL.LogMessage(clientNumber, "AveragePeriodSet", "Bad value: " + value.ToString() + ", throwing InvalidValueException");
                throw new InvalidValueException("AveragePeriod Set", value.ToString(), "0.0 updwards");
            }
        }

        public static double CloudCover(int clientNumber)
        {
            CheckConnected("CloudCover");

            double cloudCover = GetPropertyValue(PROPERTY_CLOUDCOVER);
            TL.LogMessage(clientNumber, "CloudCover", cloudCover.ToString());
            return cloudCover;
        }

        public static double DewPoint(int clientNumber)
        {
            CheckConnected("DewPoint");

            double dewPoint = GetPropertyValue(PROPERTY_DEWPOINT); ;
            TL.LogMessage(clientNumber, "DewPoint", dewPoint.ToString());
            return dewPoint;
        }

        public static double Humidity(int clientNumber)
        {
            CheckConnected("Humidity");

            double humidity = GetPropertyValue(PROPERTY_HUMIDITY); ;
            TL.LogMessage(clientNumber, "Humidity", humidity.ToString());
            return humidity;
        }

        public static double Pressure(int clientNumber)
        {
            CheckConnected("Pressure");

            double pressure = GetPropertyValue(PROPERTY_PRESSURE); ;
            TL.LogMessage(clientNumber, "Pressure", pressure.ToString());
            return pressure;
        }

        public static double RainRate(int clientNumber)
        {
            CheckConnected("RainRate");

            double rainRate = GetPropertyValue(PROPERTY_RAINRATE); ;
            TL.LogMessage(clientNumber, "RainRate", rainRate.ToString());
            return rainRate;
        }

        public static string SensorDescription(int clientNumber, string PropertyName)
        {
            CheckConnected("SensorDescription");

            if (IsValidProperty(PropertyName))
            {
                switch (Sensors[PropertyName].DeviceMode)
                {
                    case ConnectionType.None:
                        throw new MethodNotImplementedException(PropertyName + " is not implemented in the ObservingConditions hub");

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
                                MessageBox.Show("SensorDescription: Unknown DeviceType Enum value: " + Sensors[PropertyName].DeviceType.ToString());
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
            CheckConnected("Refresh");

            UpdateAllSensors("Refresh");
        }

        public static double SkyBrightness(int clientNumber)
        {
            CheckConnected("SkyBrightness");

            double skyBrightness = GetPropertyValue(PROPERTY_SKYBRIGHTNESS); ;
            TL.LogMessage(clientNumber, "SkyBrightness", skyBrightness.ToString());
            return skyBrightness;
        }

        public static double SkyQuality(int clientNumber)
        {
            CheckConnected("SkyQuality");

            double skyQuality = GetPropertyValue(PROPERTY_SKYQUALITY); ;
            TL.LogMessage(clientNumber, "SkyQuality", skyQuality.ToString());
            return skyQuality;
        }

        public static double StarFWHM(int clientNumber)
        {
            CheckConnected("StarFWHM");

            double starFWHM = GetPropertyValue(PROPERTY_STARFWHM); ;
            TL.LogMessage(clientNumber, "StarFWHM", starFWHM.ToString());
            return starFWHM;
        }

        public static double SkyTemperature(int clientNumber)
        {
            CheckConnected("SkyTemperature");

            double skyTemperature = GetPropertyValue(PROPERTY_SKYTEMPERATURE); ;
            TL.LogMessage(clientNumber, "SkyTemperature", skyTemperature.ToString());
            return skyTemperature;
        }

        public static double Temperature(int clientNumber)
        {
            CheckConnected("Temperature");

            double temperature = GetPropertyValue(PROPERTY_TEMPERATURE); ;
            TL.LogMessage(clientNumber, "Temperature", temperature.ToString());
            return temperature;
        }

        public static double TimeSinceLastUpdate(int clientNumber, string PropertyName)
        {
            CheckConnected("TimeSinceLastUpdate");

            if (PropertyName == "") // Return the most recent update time of any sensor
            {
                double timeSinceLastUpdate;

                if (timeOfLastUpdate != BAD_DATETIME) timeSinceLastUpdate = DateTime.Now.Subtract(timeOfLastUpdate).TotalSeconds; // Calculate elapsed time since any sensor's last update time
                else timeSinceLastUpdate = -1.0; // No sensor has been updated so return -1
                TL.LogMessage(clientNumber, "TimeSinceLastUpdate", "Most recent sensor update time: " + timeSinceLastUpdate.ToString());

                return timeSinceLastUpdate;
            }
            else
            {
                if (IsValidProperty(PropertyName))
                {
                    double timeSinceLastUpdate;

                    if (Sensors[PropertyName].TimeOfLastUpdate != BAD_DATETIME) timeSinceLastUpdate = DateTime.Now.Subtract(Sensors[PropertyName].TimeOfLastUpdate).TotalSeconds; // Calculate elapsed time since the sensor's last update time
                    else timeSinceLastUpdate = -1.0; // No sensor has been updated so return -1
                    TL.LogMessage(clientNumber, "TimeSinceLastUpdate", PropertyName + ": " + timeSinceLastUpdate);

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
            CheckConnected("WindDirection");

            double windDirection = GetPropertyValue(PROPERTY_WINDDIRECTION); ;
            TL.LogMessage(clientNumber, "WindDirection", windDirection.ToString());
            return windDirection;
        }

        public static double WindGust(int clientNumber)
        {
            CheckConnected("WindGust");

            double windGust = GetPropertyValue(PROPERTY_WINDGUST); ;
            TL.LogMessage(clientNumber, "WindGust", windGust.ToString());
            return windGust;
        }

        public static double WindSpeed(int clientNumber)
        {
            CheckConnected("WindSpeed");

            double windSpeed = GetPropertyValue(PROPERTY_WINDSPEED); ;
            TL.LogMessage(clientNumber, "WindSpeed", windSpeed.ToString());
            return windSpeed;
        }

        #endregion

        #region Support code

        /// <summary>
        /// Tests whether the hub is already conected
        /// </summary>
        /// <param name="clientNumber">Number of the client making the call</param>
        /// <returns>Boolean true if the hub is already connected</returns>
        public static bool IsHardwareConnected(int clientNumber)
        {
            if (DebugTraceState) TL.LogMessage(clientNumber, "IsConnected", "Number of connected devices: " + connectStates.Count + ", Returning: " + (connectStates.Count > 0).ToString());
            return connectStates.Count > 0;
        }

        /// <summary>
        /// Returns the number of connected clients
        /// </summary>
        public static int ConnectionCount
        {
            get { return connectStates.Count; }
        }

        /// <summary>
        /// Returns a unique client numnber to the calling instance
        /// </summary>
        public static int GetUniqueClientNumber()
        {
            Interlocked.Increment(ref uniqueClientNumber);
            TL.LogMessage("UniqueClientNumber", "Generated new ID: " + uniqueClientNumber.ToString());
            return uniqueClientNumber;
        }

        /// <summary>
        /// Determine whether a supplied property name is one of the valid property names
        /// </summary>
        /// <param name="PropertyName">Property name to test</param>
        /// <returns>Boolean true if the property name is valid</returns>
        internal static bool IsValidProperty(string PropertyName)
        {
            return ValidProperties.Contains(PropertyName.Trim(), StringComparer.OrdinalIgnoreCase); // Make the test case insensitive as well as leading and trailing space insensitive
        }

        /// <summary>
        /// Returns the instant or average value of a sensor depending on whether AveragePeriod is zero or greater than zero
        /// </summary>
        /// <param name="PropertyName">Sensor property to return</param>
        /// <returns>The instant or average value of a sensor depending on whether AveragePeriod is zero or greater than zero</returns>
        private static double GetPropertyValue(string PropertyName)
        {
            double returnValue = BAD_VALUE; // Initialise return value to a bad value in case things go wrong!

            if (averagePeriod == 0.0) // No averaging so return the current sensor value
            {
                returnValue = GetDeviceSensorValue(PropertyName);
            }
            else // An average period has been set 
            {
                if (double.IsNaN(Sensors[PropertyName].LastPeriodAverage)) // We have not yet calculated the period average so calculate it now and save the value so that it does not have to be calculated again until the list of values changes
                {
                    double averageValue = 0.0;
                    int numberOfSensorReadings = Sensors[PropertyName].Readings.Count;

                    if (numberOfSensorReadings > 0) // There are one or more readings so calculate the average value
                    {
                        foreach (TimeValue tv in Sensors[PropertyName].Readings) // Add the sensor readings
                        {
                            averageValue += tv.SensorValue;
                        }
                        averageValue = averageValue / numberOfSensorReadings; // Calcualte the average sensor reading
                    }
                    else // There are no readings so just get and return the current value
                    {
                        averageValue = GetDeviceSensorValue(PropertyName);
                    }

                    Sensors[PropertyName].LastPeriodAverage = averageValue;
                    returnValue = averageValue; // Return the average sensor value
                    if (DebugTraceState) TL.LogMessage("GetPropertyValue", "{0} returning new average value: {1}, from {2} readings, AveragePeriod: {3} minutes. Number of readings per average period: {4}", PropertyName, averageValue, Sensors[PropertyName].Readings.Count, averagePeriod, numberOfMeasurementsPerAveragePeriod);
                }
                else // The period average has not changed since the last call so just return this instead of calculating it again!
                {
                    returnValue = Sensors[PropertyName].LastPeriodAverage;
                    if (DebugTraceState) TL.LogMessage("GetPropertyValue", PropertyName + " returning cached value: {0}", returnValue);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Reads a sensor value from a device
        /// </summary>
        /// <param name="PropertyName">Name of the property to read</param>
        /// <returns>Double value read from device</returns>
        private static double GetDeviceSensorValue(string PropertyName)
        {
            if (DebugTraceState) TL.LogMessage("GetDeviceSensorValue", PropertyName + " device mode: " + Sensors[PropertyName].DeviceMode.ToString());
            double returnValue = BAD_VALUE;

            lock (deviceAccessLockObject)
            {
                if (DebugTraceState) TL.LogMessage("GetDeviceSensorValue", PropertyName + " - got device access lock");

                switch (Sensors[PropertyName].DeviceMode)
                {
                    case ConnectionType.None:
                        throw new PropertyNotImplementedException(PropertyName, false);

                    case ConnectionType.Real:
                        try
                        {
                            switch (Sensors[PropertyName].DeviceType)
                            {
                                case DeviceType.ObservingConditions:
                                    // Device interrogation is done inside a try catch because we are using reflection to call the property
                                    // If the device throws an exception the outer exception we get will be "TargetInvocationException" with the actual exception as the inner exception
                                    Type type = typeof(ObservingConditions);
                                    PropertyInfo propertyInfo = type.GetProperty(PropertyName);
                                    double observingConditionsValue = (double)propertyInfo.GetValue(ObservingConditionsDevices[Sensors[PropertyName].ProgID], null);
                                    TL.LogMessage("GetDeviceSensorValue", "Got value: " + observingConditionsValue + " from device " + Sensors[PropertyName].ProgID + " method " + PropertyName);
                                    returnValue = observingConditionsValue;
                                    break;

                                case DeviceType.Switch:
                                    double switchValue;
                                    if (SwitchDevices[Sensors[PropertyName].ProgID].InterfaceVersion <= 1) // Switch V1 does not have a switch description so get the switch name instead
                                    {
                                        bool switchBool = SwitchDevices[Sensors[PropertyName].ProgID].GetSwitch((short)Sensors[PropertyName].SwitchNumber);
                                        switchValue = Convert.ToDouble(switchBool);
                                        TL.LogMessage("GetDeviceSensorValue", "Got value: " + switchValue + " from Switch V1 device " + Sensors[PropertyName].ProgID + " switch number " + Sensors[PropertyName].SwitchNumber + ", device returned: " + switchBool.ToString());
                                    }
                                    else // Switch V2 does have a switch description
                                    {
                                        switchValue = SwitchDevices[Sensors[PropertyName].ProgID].GetSwitchValue((short)Sensors[PropertyName].SwitchNumber);
                                        TL.LogMessage("GetDeviceSensorValue", "Got value: " + switchValue + " from Switch V2 device " + Sensors[PropertyName].ProgID + " switch number " + Sensors[PropertyName].SwitchNumber);
                                    }
                                    returnValue = switchValue;
                                    break;

                                default:
                                    MessageBox.Show("GetDeviceSensorValue: Unknown DeviceType Enum value: " + Sensors[PropertyName].DeviceType.ToString());
                                    break;
                            }
                            Sensors[PropertyName].TimeOfLastUpdate = DateTime.Now;
                            timeOfLastUpdate = DateTime.Now;
                            if (averagePeriod > 0.0) Sensors[PropertyName].Readings.Add(new TimeValue(DateTime.Now, returnValue)); // Save the value to the sensor readings collection if we are reporting average values over a period
                        }
                        catch (COMException ex)
                        {
                            if (ex.ErrorCode == ErrorCodes.InvalidOperationException)
                            {
                                TL.LogMessageCrLf("GetDeviceSensorValue", "Received a COM invalid operation exception reading {0} {1}: {2}", Sensors[PropertyName].ProgID, PropertyName, ex.Message);
                            }
                            else
                            {
                                TL.LogMessageCrLf("GetDeviceSensorValue", "Received a COM exception ({0}) {1} reading {2}: {3}", ex.ErrorCode.ToString("X8"), Sensors[PropertyName].ProgID, PropertyName, ex.ToString());
                            }
                            throw;

                        }
                        catch (TargetInvocationException ex)
                        {
                            if (ex.InnerException is InvalidOperationException)
                            {
                                TL.LogMessageCrLf("GetDeviceSensorValue", "Received an InvalidOperationException reading {0} {1}: {2}", Sensors[PropertyName].ProgID, PropertyName, ex.InnerException.Message);
                            }
                            else if (ex.InnerException is COMException)
                            {
                                COMException exCom = (COMException)ex.InnerException;
                                if (exCom.ErrorCode == ErrorCodes.InvalidOperationException)
                                {
                                    TL.LogMessageCrLf("GetDeviceSensorValue", "Received a COM inner invalid operation exception reading {0} {1}: {2}", Sensors[PropertyName].ProgID, PropertyName, ex.InnerException.Message);
                                }
                                else
                                {
                                    TL.LogMessageCrLf("GetDeviceSensorValue", "Received a COM inner exception ({0}) {1} reading {2}: {3}", exCom.ErrorCode.ToString("X8"), Sensors[PropertyName].ProgID, PropertyName, ex.InnerException.ToString());
                                }
                            }
                            else
                            {
                                TL.LogMessageCrLf("GetDeviceSensorValue", "Received a target invocation exception reading {0} {1}: {2}", Sensors[PropertyName].ProgID, PropertyName, ex.InnerException.ToString());
                            }
                            throw ex.InnerException;

                        }
                        catch (Exception ex)
                        {
                            // Driver threw an exception so log this and throw it
                            TL.LogMessage("GetDeviceSensorValue", "Driver : " + Sensors[PropertyName].ProgID + " method " + PropertyName + " threw an exception.");
                            TL.LogMessageCrLf("GetDeviceSensorValue", ex.ToString());
                            throw;

                        }
                        break;

                    default:
                        MessageBox.Show("GetDeviceSensorValue: Unknown DeviceMode Enum value: " + Sensors[PropertyName].DeviceMode.ToString());
                        break;
                }
            } // End of device access lock

            if (DebugTraceState) TL.LogMessage("GetDeviceSensorValue", PropertyName + " - released device access lock");

            return returnValue;
        }

        /// <summary>
        /// Remove stale values from the collection of sensor measurements when they exceed the average period time
        /// </summary>
        /// <param name="timevalue">The time value to test</param>
        /// <returns>Boolean value indicating whether a particular time value should be removed from the collection</returns>
        private static bool TimeRemovePredicate(TimeValue timevalue)
        {
            return DateTime.Now.Subtract(timevalue.ObservationTime).TotalMinutes > averagePeriod;
        }

        /// <summary>
        /// Configure and enabled the average period timer
        /// </summary>
        private static void ConfigureAveragePeriodTimer()
        {
            if (averagePeriodTimer.Enabled) averagePeriodTimer.Stop();
            if (averagePeriod > 0.0)
            {
                averagePeriodTimer.Interval = averagePeriod * 60000.0 / numberOfMeasurementsPerAveragePeriod; // Average period in minutes, convert to milliseocnds and divide by the number of readings required
                if (IsHardwareConnected(0)) averagePeriodTimer.Enabled = true;
            }
        }

        /// <summary>
        /// Iterates over all sensors and updates their values.
        /// </summary>
        /// <param name="caller">Name of the method requesting sensor update</param>
        /// <remarks>Called by Refresh() and the average period timer event</remarks>
        private static void UpdateAllSensors(string caller)
        {
            DateTime now = DateTime.Now;
            double sensorValue;

            TL.LogMessage("UpdateAllSensors", "Called by {0}", caller);

            foreach (string PropertyName in ValidSensors)
            {
                if (Sensors[PropertyName].DeviceMode == ConnectionType.Real) // Only process sensors that have been configured
                {
                    try
                    {
                        sensorValue = GetDeviceSensorValue(PropertyName); // This will get the new sensor reading and add it to the Readings collection
                    }
                    catch (COMException ex)
                    {
                        if (ex.ErrorCode == ErrorCodes.InvalidOperationException)
                        {
                            TL.LogMessageCrLf(caller, "Received a COM invalid operation exception reading {0}: {1}", PropertyName, ex.Message);
                        }
                        else
                        {
                            TL.LogMessageCrLf(caller, "Received a COM exception ({0}) reading {1}: {2}", ex.ErrorCode.ToString("X8"), PropertyName, ex.ToString());
                        }
                    }
                    catch (TargetInvocationException ex)
                    {
                        if (ex.InnerException is InvalidOperationException)
                        {
                            TL.LogMessageCrLf(caller, "Received an InvalidOperationException reading {0}: {1}", PropertyName, ex.InnerException.Message);
                        }
                        else if (ex.InnerException is COMException)
                        {
                            COMException exCom = (COMException)ex.InnerException;
                            if (exCom.ErrorCode == ErrorCodes.InvalidOperationException)
                            {
                                TL.LogMessageCrLf(caller, "Received a COM inner invalid operation exception reading {0}: {1}", PropertyName, ex.InnerException.Message);
                            }
                            else
                            {
                                TL.LogMessageCrLf(caller, "Received a COM inner exception ({0}) reading {1}: {2}", exCom.ErrorCode.ToString("X8"), PropertyName, ex.InnerException.ToString());
                            }
                        }
                        else
                        {
                            TL.LogMessageCrLf(caller, "Received a target invocation exception reading {0}: {1}", PropertyName, ex.InnerException.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        TL.LogMessageCrLf(caller, "Received an exception reading {0}: {1}", PropertyName, ex.ToString());
                    }
                }

                // Remove excess values beyond the average time
                int beforeTrim = Sensors[PropertyName].Readings.Count;
                Sensors[PropertyName].Readings.RemoveAll(TimeRemovePredicate);
                int afterTrim = Sensors[PropertyName].Readings.Count;
                if (DebugTraceState) TL.LogMessage("AveragePeriodTimer", "{0} readings grooming - Before trim: {1}, After trim: {2}", PropertyName, beforeTrim, afterTrim);

                Sensors[PropertyName].LastPeriodAverage = BAD_VALUE; // Invalidate the last calculated average because we have added a value and possibly trimmed some values from the list
            }
        }

        /// <summary>
        /// Test whether a particular client is already connected
        /// </summary>
        /// <param name="clientNumber">Number of the calling client</param>
        /// <returns></returns>
        public static bool IsClientConnected(int clientNumber)
        {
            TL.LogMessage(clientNumber, "IsClientConnected", "Number of connected devices: " + connectStates.Count + ", Returning: " + connectStates.ContainsKey(clientNumber).ToString());

            return connectStates.ContainsKey(clientNumber);
        }

        /// <summary>
        /// Test whether the we are connected, if not throw a NotConnectedException
        /// </summary>
        /// <param name="MethodName">Name of the calling method</param>
        static void CheckConnected(string MethodName)
        {
            if (!IsHardwareConnected(0)) throw new NotConnectedException(MethodName + " - " + NOT_CONNECTED_MESSAGE);
        }

        #endregion

        #region Profile management

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        public static void ReadProfile()
        {
            TL.LogMessage("ReadProfile", "Reading profile");

            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = DEVICE_TYPE;

                // Initialise the logging trace state from the Profile
                TraceState = Convert.ToBoolean(driverProfile.GetValue(DRIVER_PROGID, TRACE_LEVEL_PROFILENAME, string.Empty, TRACE_LEVEL_DEFAULT));
                TL.Enabled = TraceState; // Set the logging state immediately after this has been retrieved from Profile

                // Initialise other variables from the Profile
                DebugTraceState = Convert.ToBoolean(driverProfile.GetValue(DRIVER_PROGID, DEBUG_TRACE_PROFILENAME, string.Empty, DEBUG_TRACE_DEFAULT));
                ConnectToDrivers = Convert.ToBoolean(driverProfile.GetValue(DRIVER_PROGID, CONNECT_TO_DRIVERS_PROFILENAME, string.Empty, CONNECT_TO_DRIVERS_DEFAULT));
                averagePeriod = Convert.ToDouble(driverProfile.GetValue(DRIVER_PROGID, AVERAGE_PERIOD_PROFILENAME, string.Empty, AVERAGE_PERIOD_DEFAULT.ToString()));
                numberOfMeasurementsPerAveragePeriod = Convert.ToInt32(driverProfile.GetValue(DRIVER_PROGID, NUMBER_OF_READINGS_PROFILENAME, string.Empty, NUMBER_OF_READINGS_DEFAULT.ToString()));
                overrideUISafetyLimits = Convert.ToBoolean(driverProfile.GetValue(DRIVER_PROGID, OVERRIDE_UI_SAFETY_LIMITS_PROFILENAME, string.Empty, OVERRIDE_UI_SAFETY_LIMITS_DEFAULT.ToString())); // This cannot be changed through the setup UI so it is only read in

                // Initialise the sensor collection from the Profile
                foreach (string Property in ValidSensors)
                {
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
                TL.LogMessage("WriteProfile", "Writing profile");
                driverProfile.DeviceType = DEVICE_TYPE;

                // Save the variable state to the Profile
                driverProfile.WriteValue(DRIVER_PROGID, TRACE_LEVEL_PROFILENAME, TraceState.ToString());
                driverProfile.WriteValue(DRIVER_PROGID, DEBUG_TRACE_PROFILENAME, DebugTraceState.ToString());
                driverProfile.WriteValue(DRIVER_PROGID, CONNECT_TO_DRIVERS_PROFILENAME, ConnectToDrivers.ToString());
                driverProfile.WriteValue(DRIVER_PROGID, AVERAGE_PERIOD_PROFILENAME, averagePeriod.ToString());
                driverProfile.WriteValue(DRIVER_PROGID, NUMBER_OF_READINGS_PROFILENAME, numberOfMeasurementsPerAveragePeriod.ToString());
                driverProfile.WriteValue(DRIVER_PROGID, OVERRIDE_UI_SAFETY_LIMITS_PROFILENAME, overrideUISafetyLimits.ToString());

                // Save the sensor collection to the Profile
                foreach (string Property in ValidSensors)
                {
                    Sensors[Property].WriteProfile(driverProfile);
                }
            }
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Event handler for the average period timer, which queries devices regularly as determined by the hub setup.
        /// </summary>
        /// <param name="sender">Control causing the event</param>
        /// <param name="e">Contextual information about the event</param>
        private static void AveragePeriodTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            UpdateAllSensors("AveragePeriodTimer");
        }

        #endregion

    }

}
