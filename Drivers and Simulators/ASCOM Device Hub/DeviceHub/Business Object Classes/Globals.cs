using System;
using System.Threading.Tasks;

using ASCOM.Utilities;

namespace ASCOM.DeviceHub
{
    public static class Globals
    {
        static Globals()
        {
            RegistryVersion = 6.0;
        }

        public const string DevHubTelescopeID = "ASCOM.DeviceHub.Telescope";
        public const string DevHubDomeID = "ASCOM.DeviceHub.Dome";
        public const string DevHubFocuserID = "ASCOM.DeviceHub.Focuser";

        public const string NO_DATA_MESSAGE = "Unavailable";

        public const double DegRad = Math.PI / 180.0;
        public const double UTC_SECS_PER_SIDEREAL_SEC = 0.997269566334879;

        public const double DEG_TO_RAD = (2.0 * Math.PI) / 360.0;     // 0.017453 radians per degree
        public const double RAD_TO_DEG = 1.0 / DEG_TO_RAD;              // 57.29578 degrees per radian
        public const double HRS_TO_RAD = (2.0 * Math.PI) / 24.0;      // 0.26180 radians per hour
        public const double RAD_TO_HRS = 1.0 / HRS_TO_RAD;              // 3.81972 hours per radian
        public const double HRS_TO_DEG = 15.0;                          // 15 degrees per hour

        public const double SCOPE_FAST_UPDATE_MIN = 0.5;
        public const double SCOPE_FAST_UPDATE_MAX = 1.5;

        public const double DOME_FAST_UPDATE_MIN = 1.0;
        public const double DOME_FAST_UPDATE_MAX = 3.0;

        public const double FOCUSER_FAST_UPDATE_MIN = 0.5;
        public const double FOCUSER_FAST_UPDATE_MAX = 5.0;

        public const int GARBAGE_COLLECT_TIME_BETWEEN_ATTEMPTS = 100; // Time (milliseconds) between quick garbage collections when processing the Disconnect() method
        public const int GARBAGE_COLLECT_ATTEMPTS = 20; // Number of attempts to collect garbage when processing the Disconnect() method. 20 attempts allows 2 seconds of trying with a 100ms wait between attempts

        private static bool? _latestSupportMultipleTelescopesState;

        /// <summary>
        /// Return the latest state of the SupportMultipleTelescopes flag. (This may not have been persisted yet if user is in the setup dialogue)
        /// </summary>
        public static bool LatestSupportMultipleTelescopesState
        {
            get
            {
                if (!_latestSupportMultipleTelescopesState.HasValue)
                {
                    // Default to value in Profile if the value is not already known

                    DomeSettings domeSettings = DomeSettings.FromProfile();
                    _latestSupportMultipleTelescopesState = domeSettings.DomeLayoutSettings.SupportMultipleTelescopes;
                }

                return _latestSupportMultipleTelescopesState.Value;
            }
            set
            {
                _latestSupportMultipleTelescopesState = value;
            }
        }

        public static double RegistryVersion { get; set; }

        public static DeviceTypeEnum ActiveDevice { get; set; }

        public static double MainWindowTop { get; set; }
        public static double MainWindowLeft { get; set; }

        public static double ActivityWindowTop { get; set; }
        public static double ActivityWindowLeft { get; set; }
        public static double ActivityWindowHeight { get; set; }
        public static double ActivityWindowWidth { get; set; }
        public static bool AlwaysOnTop { get; set; }
        public static bool IsDomeExpanded { get; set; }
        public static bool IsFocuserExpanded { get; set; }

        public static DomeLayoutSettings DomeLayoutSettings { get; set; }
        public static bool IsDomeSlaved { get; set; }
        public static bool DomeSyncError { get; set; }

        public static bool SuppressTrayBubble { get; set; }
        public static bool UseCustomTheme { get; set; }
        public static double FocuserTemperatureOffset { get; set; }
        public static bool UsePOTHDomeSlaveCalculation { get; set; }
        public static bool UseRevisedDomeSlaveCalculation { get; set; }
        public static bool UseExpandedScreenLayout { get; set; }
        public static double DomeAzimuthAdjustment { get; set; }
        public static bool UseCompositeSlewingFlag { get; set; }
        public static bool ShowActivityLogWhenStarted { get; set; }
        public static bool WriteLogActivityToDisk { get; set; }

        // Activity log settings
        public static bool ActivityLogTelescopeDevice { get; set; }
        public static bool ActivityLogDomeDevice { get; set; }
        public static bool ActivityLogFocuserDevice { get; set; }
        public static bool ActivityLogCommands { get; set; }
        public static bool ActivityLogStatus { get; set; }
        public static bool ActivityLogParameters { get; set; }
        public static bool ActivityLogCapabilities { get; set; }
        public static bool ActivityLogOtherActivity { get; set; }

        // The raw statuses for telescope and dome are maintained here in order to
        // correctly set and reset the Composite Slewing Flag.

        public static DevHubDomeStatus LatestRawDomeStatus { get; set; }
        public static DevHubTelescopeStatus LatestRawTelescopeStatus { get; set; }

        public static TaskScheduler UISyncContext { get; set; }

        public static bool ForceAppLogging { get; set; }

        private static TraceLogger _appLogger = null;

        public static TraceLogger AppLogger
        {
            get
            {
                if (_appLogger == null)
                {
                    _appLogger = new Utilities.TraceLogger("", "DeviceHub.App")
                    {
                        Enabled = Properties.Settings.Default.DeviceHubAppLoggingEnabled || ForceAppLogging,
                        IdentifierWidth = 50
                    };
                }

                return _appLogger;
            }
        }

        public static double ConditionHA(double ha)
        {
            double lowerBound = -12.0;
            double upperBound = 12.0;
            double range = upperBound - lowerBound;

            double retval = ha;

            while (retval < lowerBound)
            {
                retval += range;
            }

            while (retval > upperBound)
            {
                retval -= range;
            }

            return retval;
        }

        /// <summary>
        /// Condition an angle to the range 0..359.999...
        /// </summary>
        /// <param name="angle"></param>
        /// <returns>Angle in the range 0 to 359.999...</returns>
        public static double Condition0To359(double angle)
        {
            return Range(angle, 0.0, true, 360.0, false);
        }

        /// <summary>
        /// Flexible routine to range a number into a given range between a lower and an higher bound.
        /// </summary>
        /// <param name="Value">Value to be ranged</param>
        /// <param name="LowerBound">Lowest value of the range</param>
        /// <param name="LowerEqual">Boolean flag indicating whether the ranged value can have the lower bound value</param>
        /// <param name="UpperBound">Highest value of the range</param>
        /// <param name="UpperEqual">Boolean flag indicating whether the ranged value can have the upper bound value</param>
        /// <returns>The ranged nunmber as a double</returns>
        /// <exception cref="ASCOM.InvalidValueException">Thrown if the lower bound is greater than the upper bound.</exception>
        /// <exception cref="ASCOM.InvalidValueException">Thrown if LowerEqual and UpperEqual are both false and the ranged value equals
        /// one of these values. This is impossible to handle as the algorithm will always violate one of the rules!</exception>
        /// <remarks>
        /// UpperEqual and LowerEqual switches control whether the ranged value can be equal to either the upper and lower bounds. So, 
        /// to range an hour angle into the range 0 to 23.999999.. hours, use this call: 
        /// <code>RangedValue = Range(InputValue, 0.0, True, 24.0, False)</code>
        /// <para>The input value will be returned in the range where 0.0 is an allowable value and 24.0 is not i.e. in the range 0..23.999999..</para>
        /// <para>It is not permissible for both LowerEqual and UpperEqual to be false because it will not be possible to return a value that is exactly equal 
        /// to either lower or upper bounds. An exception is thrown if this scenario is requested.</para>
        /// </remarks>
        public static double Range(double Value, double LowerBound, bool LowerEqual, double UpperBound, bool UpperEqual)
        {
            double ModuloValue;
            if (LowerBound >= UpperBound)
                throw new InvalidValueException("Range", "LowerBound is >= UpperBound", "LowerBound must be less than UpperBound");

            ModuloValue = UpperBound - LowerBound;

            if (LowerEqual)
            {
                if (UpperEqual) // Lowest >= Highest <=
                {
                    do
                    {
                        if (Value < LowerBound)
                            Value += ModuloValue;
                        if (Value > UpperBound)
                            Value -= ModuloValue;
                    }
                    while (!(Value >= LowerBound & Value <= UpperBound));
                }
                else // Lowest >= Highest <
                {
                    do
                    {
                        if (Value < LowerBound)
                            Value += ModuloValue;
                        if (Value >= UpperBound)
                            Value -= ModuloValue;
                    }
                    while (!(Value >= LowerBound & Value < UpperBound));
                }
            }
            else if (UpperEqual) // Lowest > Highest<=
            {
                do
                {
                    if (Value <= LowerBound)
                        Value += ModuloValue;
                    if (Value > UpperBound)
                        Value -= ModuloValue;
                }
                while (!(Value > LowerBound & Value <= UpperBound));
            }
            else // Lowest > Highest <
            {
                if (Value == LowerBound)
                    throw new InvalidValueException("Range", "The supplied value equals the LowerBound. This can not be ranged when LowerEqual and UpperEqual are both false ", "LowerBound > Value < UpperBound");
                if (Value == UpperBound)
                    throw new InvalidValueException("Range", "The supplied value equals the UpperBound. This can not be ranged when LowerEqual and UpperEqual are both false ", "LowerBound > Value < UpperBound");
                do
                {
                    if (Value <= LowerBound)
                        Value += ModuloValue;
                    if (Value >= UpperBound)
                        Value -= ModuloValue;
                }
                while (!(Value > LowerBound & Value < UpperBound));
            }
            return Value;
        }

        public static void CloseAppLogger()
        {
            if (_appLogger != null)
            {
                _appLogger.Enabled = false;
                _appLogger.Dispose();
                _appLogger = null;
            }
        }
    }
}
