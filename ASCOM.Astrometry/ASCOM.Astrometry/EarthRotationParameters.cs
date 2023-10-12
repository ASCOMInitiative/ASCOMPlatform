using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;
using ASCOM.Utilities;
using Microsoft.Win32.TaskScheduler;

namespace ASCOM.Astrometry
{
    /// <summary>
    /// EarthRotationParameters class
    /// </summary>
    public class EarthRotationParameters : IDisposable
    {

        #region Variables and constants

        private string UpdateTypeValue;
        private double ManualDeltaUT1Value;
        private double ManualLeapSecondsValue;
        private double AutomaticLeapSecondsValue;
        private string AutomaticLeapSecondsStringValue;
        private double NextLeapSecondsValue;
        private string NextLeapSecondsStringValue;
        private DateTime NextLeapSecondsDateValue;
        private string NextLeapSecondsDateStringValue;
        private string DownloadTaskDataSourceValue;
        private double DownloadTaskTimeOutValue;
        private string DownloadTaskRepeatFrequencyValue;
        private bool DownloadTaskTraceEnabledValue;
        private string DownloadTaskTracePathValue;
        private DateTime DownloadTaskScheduledTimeValue;
        private string EarthRotationDataLastUpdatedValue;
        private SortedList<double, double> BuiltInLeapSecondsValues;

        private TraceLogger TL;
        private bool DebugTraceEnabled = Utilities.Global.GetBool(Utilities.Global.ASTROUTILS_TRACE, Utilities.Global.ASTROUTILS_TRACE_DEFAULT);
        private RegistryAccess profile;
        private bool disposedValue; // To detect redundant calls

        // Lock objects and caching control variables
        private static object LeapSecondLockObject;
        private static double LastLeapSecondJulianDate = GlobalItems.DOUBLE_VALUE_NOT_AVAILABLE;
        private static double LastLeapSecondValue;

        private static object DeltaTLockObject;
        private static double LastDeltaTJulianDate = GlobalItems.DOUBLE_VALUE_NOT_AVAILABLE;
        private static double LastDeltaTValue;

        private static object DeltaUT1LockObject;
        private static double LastDeltaUT1JulianDate = GlobalItems.DOUBLE_VALUE_NOT_AVAILABLE;
        private static double LastDeltaUT1Value;

        // Constants for the minimum and maximum Julian date values that are accepted by the DeltaT function
        private const double DELTAT_JULIAN_DATE_MINIMUM = 1757582.5d;
        private const double DELTAT_JULIAN_DATE_MAXIMUM = 5373484.499999d;

        // Name of the Scheduler Task process, this should be the same on all Windows versions regardless of localisation
        private const string SCHEDULER_SERVICE_NAME = "Schedule";

        // Downloaded leap second data. Format:  JulianDate, Year, Month, Day LeapSeconds
        private SortedList<double, double> DownloadedLeapSecondValues = new(); // Initialise to an empty list

        #endregion

        #region New and IDisposable Support
        /// <summary>
        /// EarthRotationParameters initiator
        /// </summary>
        public EarthRotationParameters() : this(null) // Call the main initialisation routine with no trace logger reference
        {
        }

        /// <summary>
        /// EarthRotationParameters initiator with TraceLogger
        /// </summary>
        /// <param name="SuppliedTraceLogger"></param>
        public EarthRotationParameters(TraceLogger SuppliedTraceLogger)
        {
            DateTime LeapSecondDate;

            TL = SuppliedTraceLogger; // Save the reference to the caller's trace logger so we can write to it
            profile = new RegistryAccess();
            DebugTraceEnabled = Utilities.Global.GetBool(Utilities.Global.ASTROUTILS_TRACE, Utilities.Global.ASTROUTILS_TRACE_DEFAULT); // Get our debug trace value

            LogMessage("EarthRotationParameters", "Getting built-in leap second values");
            BuiltInLeapSecondsValues = SOFA.SOFA.BuiltInLeapSeconds();
            LogMessage("EarthRotationParameters", string.Format("Received {0 }leap second values", BuiltInLeapSecondsValues.Count));

            foreach (KeyValuePair<double, double> record in BuiltInLeapSecondsValues)
            {
                LeapSecondDate = DateTime.FromOADate(record.Key - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET);
                LogMessage("EarthRotationParameters", string.Format("Received leap second - DMY: {0} {1} {2}, Leap seconds: {3}, ({4})", LeapSecondDate.Day, LeapSecondDate.Month, LeapSecondDate.Year, record.Value, LeapSecondDate.ToLongDateString()));
            }

            // Initialise lock objects
            LeapSecondLockObject = new object();
            DeltaTLockObject = new object();
            DeltaUT1LockObject = new object();

            RefreshState();
        }

        // IDisposable
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected virtual void Dispose(bool disposing)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                if (profile is not null)
                {
                    profile.Dispose();
                    profile = (RegistryAccess)null;
                }
            }
            disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        /// <summary>
        /// DIspose of the EarthRotationParameters class
        /// </summary>
        public void Dispose()
        {
            // Do not change this code.  Put clean-up code in Dispose(disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// EarthRotationParameters destructor
        /// </summary>
        ~EarthRotationParameters()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(false);
        }
        #endregion

        #region Public properties

        /// <summary>
        /// Number of leap seconds built into class.
        /// </summary>
        public double CurrentBuiltInLeapSeconds
        {
            get
            {
                double ReturnValue, RequiredLeapSecondJulianDate;

                RequiredLeapSecondJulianDate = DateTime.UtcNow.ToOADate() + GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET;
                ReturnValue = BuiltInLeapSeconds(RequiredLeapSecondJulianDate);
                LogDebugMessage("CurrentBuiltInLeapSeconds", string.Format("Returning current built-in leap seconds value: {0} for JD {1} ({2})", ReturnValue, RequiredLeapSecondJulianDate, DateTime.FromOADate(RequiredLeapSecondJulianDate - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                return ReturnValue;
            }
        }

        /// <summary>
        /// Download task scheduled time
        /// </summary>
        public DateTime DownloadTaskScheduledTime
        {
            get
            {
                return DownloadTaskScheduledTimeValue;
            }
            set
            {
                DownloadTaskScheduledTimeValue = value;
                LogDebugMessage("DownloadTaskRunTime Write", string.Format("DownloadTaskRunTime = {0}", DownloadTaskScheduledTimeValue.ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT, CultureInfo.InvariantCulture)));
                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.DOWNLOAD_TASK_SCHEDULED_TIME_VALUE_NAME, DownloadTaskScheduledTimeValue.ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Update type
        /// </summary>
        public string UpdateType
        {
            get
            {
                return UpdateTypeValue;
            }
            set
            {
                UpdateTypeValue = value;
                LogDebugMessage("UpdateType Write", string.Format("UpdateTypeValue = {0}", UpdateTypeValue));
                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.UPDATE_TYPE_VALUE_NAME, UpdateTypeValue);
            }
        }

        /// <summary>
        /// Manually entered  delta T value
        /// </summary>
        public double ManualDeltaUT1
        {
            get
            {
                return ManualDeltaUT1Value;
            }
            set
            {
                ManualDeltaUT1Value = value;
                LogDebugMessage("ManualDeltaUT1 Write", string.Format("ManualDeltaUT1Value = {0}", ManualDeltaUT1Value.ToString(CultureInfo.InvariantCulture)));
                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.MANUAL_DELTAUT1_VALUE_NAME, ManualDeltaUT1Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Manually entered leap seconds value
        /// </summary>
        public double ManualLeapSeconds
        {
            get
            {
                return ManualLeapSecondsValue;
            }
            set
            {
                ManualLeapSecondsValue = value;
                LogDebugMessage("ManualLeapSeconds Write", string.Format("ManualTaiUtcOffsetValue = {0}", ManualLeapSecondsValue.ToString(CultureInfo.InvariantCulture)));
                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.MANUAL_LEAP_SECONDS_VALUENAME, ManualLeapSecondsValue.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Number of leap seconds downloaded from the Internet.
        /// </summary>
        public double AutomaticLeapSeconds
        {
            get
            {
                return AutomaticLeapSecondsValue;
            }
        }

        /// <summary>
        /// Number of leap seconds downloaded from the Internet as a formatted string.
        /// </summary>
        public string AutomaticLeapSecondsString
        {
            get
            {
                return AutomaticLeapSecondsStringValue;
            }
            set
            {
                AutomaticLeapSecondsStringValue = value;
                LogDebugMessage("AutomaticLeapSeconds Write", string.Format("AutomaticLeapSeconds = {0}", AutomaticLeapSecondsStringValue));
                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.AUTOMATIC_LEAP_SECONDS_VALUENAME, AutomaticLeapSecondsStringValue);
            }
        }

        /// <summary>
        /// The next leap second value if specified by the IERS.
        /// </summary>
        public double NextLeapSeconds
        {
            get
            {
                return NextLeapSecondsValue;
            }
        }

        /// <summary>
        /// The next leap second value if specified by the IERS as a formatted string.
        /// </summary>
        public string NextLeapSecondsString
        {
            get
            {
                return NextLeapSecondsStringValue;
            }
            set
            {
                NextLeapSecondsStringValue = value;
                LogDebugMessage("NextLeapSeconds Write", string.Format("NextLeapSeconds = {0}", NextLeapSecondsStringValue));
                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.NEXT_LEAP_SECONDS_VALUENAME, NextLeapSecondsStringValue);
            }
        }

        /// <summary>
        /// Date on which the next leap second change will be effected, if specified by the IERS
        /// </summary>
        public DateTime NextLeapSecondsDate
        {
            get
            {
                return NextLeapSecondsDateValue;
            }
        }

        /// <summary>
        /// Date on which the next leap second change will be effected, if specified by the IERS as a formatted string
        /// </summary>
        public string NextLeapSecondsDateString
        {
            get
            {
                return NextLeapSecondsDateStringValue;
            }
            set
            {
                NextLeapSecondsDateStringValue = value;
                LogDebugMessage("NextLeapSecondsDate Write", string.Format("NextLeapSecondsDate = {0}", NextLeapSecondsDateStringValue));
                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.NEXT_LEAP_SECONDS_DATE_VALUENAME, NextLeapSecondsDateStringValue);
            }
        }

        /// <summary>
        /// Data source for leap second and delta T Internet updates
        /// </summary>
        public string DownloadTaskDataSource
        {
            get
            {
                return DownloadTaskDataSourceValue;
            }
            set
            {
                DownloadTaskDataSourceValue = value;
                LogDebugMessage("DownloadTaskDataSource Write", string.Format("DownloadTaskDataSourceValue = {0}", DownloadTaskDataSourceValue));
                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.DOWNLOAD_TASK_DATA_SOURCE_VALUE_NAME, DownloadTaskDataSourceValue);
            }
        }

        /// <summary>
        /// Response timeout for the Internet data download task.
        /// </summary>
        public double DownloadTaskTimeOut
        {
            get
            {
                return DownloadTaskTimeOutValue;
            }
            set
            {
                DownloadTaskTimeOutValue = value;
                LogDebugMessage("DownloadTaskTimeOut Write", string.Format("DownloadTaskTimeOutValue = {0}", DownloadTaskTimeOutValue.ToString(CultureInfo.InvariantCulture)));
                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.DOWNLOAD_TASK_TIMEOUT_VALUE_NAME, DownloadTaskTimeOutValue.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Flag indicating how frequently the internet data should be updated
        /// </summary>
        public string DownloadTaskRepeatFrequency
        {
            get
            {
                return DownloadTaskRepeatFrequencyValue;
            }
            set
            {
                DownloadTaskRepeatFrequencyValue = value;
                LogDebugMessage("DownloadTaskRepeatFrequency Write", string.Format("DownloadTaskRepeatFrequencyValue = {0}", DownloadTaskRepeatFrequencyValue));
                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.DOWNLOAD_TASK_REPEAT_FREQUENCY_VALUE_NAME, DownloadTaskRepeatFrequencyValue);
            }
        }

        /// <summary>
        /// Flag indicating whether the download task is enabled.
        /// </summary>
        public bool DownloadTaskTraceEnabled
        {
            get
            {
                return DownloadTaskTraceEnabledValue;
            }
            set
            {
                DownloadTaskTraceEnabledValue = value;
                LogDebugMessage("DownloadTaskTraceEnabled Write", string.Format("DownloadTaskTraceEnabledValue = {0}", DownloadTaskTraceEnabledValue.ToString(CultureInfo.InvariantCulture)));
                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.DOWNLOAD_TASK_TRACE_ENABLED_VALUE_NAME, DownloadTaskTraceEnabledValue.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Location where the download task will place its log files
        /// </summary>
        public string DownloadTaskTracePath
        {
            get
            {
                return DownloadTaskTracePathValue;
            }
            set
            {
                DownloadTaskTracePathValue = value;
                LogDebugMessage("DownloadTaskTracePath Write", string.Format("DownloadTaskTracePathValue = {0}", DownloadTaskTracePathValue));
                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.DOWNLOAD_TASK_TRACE_PATH_VALUE_NAME, DownloadTaskTracePathValue);
            }
        }

        /// <summary>
        /// Date and time when the internet data was last successfully refreshed.
        /// </summary>
        public string EarthRotationDataLastUpdatedString
        {
            get
            {
                return EarthRotationDataLastUpdatedValue;
            }
            set
            {
                EarthRotationDataLastUpdatedValue = value;
                LogDebugMessage("EarthRotationDataLastUpdated Write", string.Format("EarthRotationDataLastUpdatedValue = {0}", EarthRotationDataLastUpdatedValue));
                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.EARTH_ROTATION_DATA_LAST_UPDATED_VALUE_NAME, EarthRotationDataLastUpdatedValue);
            }
        }

        /// <summary>
        /// Return today's number of leap seconds
        /// </summary>
        /// <returns>Current leap seconds as a double</returns>
        public double LeapSeconds()
        {
            double CurrentJulianDate;
            CurrentJulianDate = DateTime.UtcNow.ToOADate() + GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET; // Calculate today's Julian date

            lock (LeapSecondLockObject)
            {
                if (Math.Truncate(CurrentJulianDate - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET) == Math.Truncate(LastLeapSecondJulianDate - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET)) // Return the cached value if its available otherwise calculate it and save the value for the next call
                {
                    LogDebugMessage("LeapSeconds", string.Format("Returning cached today's UTC leap second value: {0}", LastLeapSecondValue));
                    return LastLeapSecondValue;
                }
            }

            return LeapSeconds(CurrentJulianDate); // Return today's number of leap seconds

        }

        /// <summary>
        /// Return the specified Julian day's number of leap seconds
        /// </summary>
        /// <param name="RequiredLeapSecondJulianDate"></param>
        /// <returns>Leap seconds as a double</returns>
        public double LeapSeconds(double RequiredLeapSecondJulianDate)
        {
            DateTime EffectiveDate;
            double ReturnValue = default, TodayJulianDate;
            SortedList<double, double> ActiveLeapSeconds; // Variable to hold either downloaded or built-in leap second values

            lock (LeapSecondLockObject)
            {
                if (Math.Truncate(RequiredLeapSecondJulianDate - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET) == Math.Truncate(LastLeapSecondJulianDate - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET)) // Return the cached value if its available otherwise calculate it and save the value for the next call
                {
                    LogDebugMessage("LeapSeconds(JD)", string.Format("Returning cached leap second value: {0} for UTC Julian date: {1} ({2})", LastLeapSecondValue, RequiredLeapSecondJulianDate, DateTime.FromOADate(RequiredLeapSecondJulianDate - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                    return LastLeapSecondValue;
                }
            }

            TodayJulianDate = DateTime.UtcNow.ToOADate() + GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET;
            if (Math.Truncate(RequiredLeapSecondJulianDate - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET) == Math.Truncate(TodayJulianDate - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET)) // Request is for the current day so process using all the options
            {

                // Set the current number of leap seconds once for this instance using manual or automatic values as appropriate
                switch (UpdateTypeValue ?? "")
                {
                    case GlobalItems.UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1:
                    case GlobalItems.UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1:
                        {
                            // Approach to returning a leap second value:
                            // Test whether the Next Leap Second Date is available
                            // If yes then test whether we are past the next leap second date - measured in UTC time because leap seconds are applied at 00:00:00 UTC. 
                            // If yes Then test whether the Next Leap Seconds value Is available
                            // If yes then use it
                            // If no then fall back To the manual value.
                            // If no then test whether the Automatic Leap Seconds value is available
                            // If yes then use it
                            // If no then fall back To the manual value.
                            // If no then test whether the Automatic Leap Seconds value is available
                            // If yes then use it
                            // If no then fall back To the manual value.
                            if (NextLeapSecondsDateValue == GlobalItems.DATE_VALUE_NOT_AVAILABLE) // A future leap second change date has not been published
                            {
                                if (AutomaticLeapSecondsValue != GlobalItems.DOUBLE_VALUE_NOT_AVAILABLE) // We have a good automatic leap second value so use this
                                {
                                    ReturnValue = AutomaticLeapSecondsValue;
                                    LogDebugMessage("LeapSeconds(JD)", string.Format("Automatic leap seconds are required and a valid value is available: {0} for JD {1} ({2})", ReturnValue, RequiredLeapSecondJulianDate, DateTime.FromOADate(RequiredLeapSecondJulianDate - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                                }
                                else // We do not have a downloaded leap second value so fall back to the Manual value
                                {
                                    ReturnValue = ManualLeapSecondsValue;
                                    LogDebugMessage("LeapSeconds(JD)", string.Format("Automatic leap seconds are required but a valid value is not available - returning the manual leap seconds value instead: {0} for JD {1} ({2})", ReturnValue, RequiredLeapSecondJulianDate, DateTime.FromOADate(RequiredLeapSecondJulianDate - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                                }
                            }
                            else // A future leap second date has been published
                            {
                                EffectiveDate = DateTime.UtcNow.Subtract(new TimeSpan(GlobalItems.TEST_UTC_DAYS_OFFSET, GlobalItems.TEST_UTC_HOURS_OFFSET, GlobalItems.TEST_UTC_MINUTES_OFFSET, 0)); // This is used to support development testing
                                LogDebugMessage("LeapSeconds(JD)", string.Format("Effective date: {0}, NextLeapSecondsDate: {1}", EffectiveDate.ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT), NextLeapSecondsDateValue.ToUniversalTime().ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT), RequiredLeapSecondJulianDate, DateTime.FromOADate(RequiredLeapSecondJulianDate - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));

                                if (EffectiveDate > NextLeapSecondsDateValue.ToUniversalTime()) // We are beyond the next leap second implementation date/time so use the next leap second value
                                {
                                    if (NextLeapSecondsValue != GlobalItems.DOUBLE_VALUE_NOT_AVAILABLE) // We have a good next leap seconds value so use it
                                    {
                                        ReturnValue = NextLeapSecondsValue;
                                        LogDebugMessage("LeapSeconds(JD)", string.Format("Automatic leap seconds are required, current time is after the next leap second implementation time and a valid next leap seconds value is available: {0} for JD {1} ({2})", ReturnValue, RequiredLeapSecondJulianDate, DateTime.FromOADate(RequiredLeapSecondJulianDate - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                                    }
                                    else // We don't have a good next leap seconds value so fall back to the manual leap seconds value
                                    {
                                        ReturnValue = ManualLeapSecondsValue;
                                        LogDebugMessage("LeapSeconds(JD)", string.Format("Automatic leap seconds are required, current time is after the next leap second implementation time but a valid next leap seconds value is not available - returning the manual leap seconds value instead: {0} for JD {1} ({2})", ReturnValue, RequiredLeapSecondJulianDate, DateTime.FromOADate(RequiredLeapSecondJulianDate - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                                    }
                                }
                                else if (AutomaticLeapSecondsValue != GlobalItems.DOUBLE_VALUE_NOT_AVAILABLE) // We are not beyond the next leap second implementation date so use the automatic leap second value
                                                                                                              // We have a good automatic leap seconds value so use it
                                {
                                    ReturnValue = AutomaticLeapSecondsValue;
                                    LogDebugMessage("LeapSeconds(JD)", string.Format("Automatic leap seconds are required, current time is before the next leap second implementation time and a valid automatic leap seconds value is available: {0} for JD {1} ({2})", ReturnValue, RequiredLeapSecondJulianDate, DateTime.FromOADate(RequiredLeapSecondJulianDate - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                                }
                                else // We don't have a good automatic leap seconds value so fall back to the manual leap seconds value
                                {
                                    ReturnValue = ManualLeapSecondsValue;
                                    LogDebugMessage("LeapSeconds(JD)", string.Format("Automatic leap seconds are required, current time is before the next leap second implementation time but a valid automatic leap seconds value is not available - returning the manual leap seconds value instead: {0} for JD {1} ({2})", ReturnValue, RequiredLeapSecondJulianDate, DateTime.FromOADate(RequiredLeapSecondJulianDate - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                                }
                            }

                            break;
                        }
                    case GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1:
                        {
                            ReturnValue = ManualLeapSecondsValue;
                            LogDebugMessage("LeapSeconds(JD)", string.Format("Manual leap seconds and delta UT1 are required, returning the manual leap seconds value: {0} for JD {1} ({2})", ReturnValue, RequiredLeapSecondJulianDate, DateTime.FromOADate(RequiredLeapSecondJulianDate - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                            break;
                        }
                    case GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1:
                        {
                            ReturnValue = ManualLeapSecondsValue;
                            LogDebugMessage("LeapSeconds(JD)", string.Format("Manual leap seconds and predicted delta UT1 are required, returning the manual leap seconds value: {0} for JD {1} ({2})", ReturnValue, RequiredLeapSecondJulianDate, DateTime.FromOADate(RequiredLeapSecondJulianDate - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                            break;
                        }
                    case GlobalItems.UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1:
                        {
                            // Find the leap second value from the built-in table of historic values
                            ReturnValue = BuiltInLeapSeconds(RequiredLeapSecondJulianDate);
                            LogDebugMessage("LeapSeconds(JD)", string.Format("Built-in leap seconds and delta UT1 are required, returning the built-in leap seconds value: {0} for JD {1} ({2})", ReturnValue, RequiredLeapSecondJulianDate, DateTime.FromOADate(RequiredLeapSecondJulianDate - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                            break;
                        }

                    default:
                        {
                            LogDebugMessage("LeapSeconds(JD)", "Unknown UpdateTypeValue: " + UpdateTypeValue);
                            MessageBox.Show("EarthRotationParameters.LeapSeconds(JD) - Unknown UpdateTypeValue: " + UpdateTypeValue);
                            break;
                        }
                }
            }

            else // Request is not for today so find value from downloaded values, if available, or fall back to built-in values
            {

                if (DownloadedLeapSecondValues.Count > 0) // We have downloaded values so use them
                {
                    LogDebugMessage("LeapSeconds(JD)", string.Format("Historic leap second value required. Searching in {0} downloaded leap second values.", DownloadedLeapSecondValues.Count));
                    ActiveLeapSeconds = DownloadedLeapSecondValues;
                }
                else // No downloaded values so fall back to built-in values
                {
                    LogDebugMessage("LeapSeconds(JD)", string.Format("Historic leap second value required. Searching in {0} built-in leap second values.", BuiltInLeapSecondsValues.Count));
                    ActiveLeapSeconds = BuiltInLeapSecondsValues;
                }

                for (int i = ActiveLeapSeconds.Count - 1; i >= 0; i -= 1)
                {
                    LogDebugMessage("LeapSeconds(JD)", string.Format("Searching downloaded JD {0} with leap second: {1}", ActiveLeapSeconds.Keys[i], ActiveLeapSeconds.Values[i]));

                    if (Math.Truncate(RequiredLeapSecondJulianDate - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET) >= Math.Truncate(ActiveLeapSeconds.Keys[i] - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET)) // Found a match
                    {
                        ReturnValue = ActiveLeapSeconds.Values[i];
                        LogDebugMessage("LeapSeconds(JD)", string.Format("Found downloaded leap second: {0} set on JD {1}", ActiveLeapSeconds.Values[i], ActiveLeapSeconds.Keys[i]));
                        break;
                    }
                }
            }

            lock (LeapSecondLockObject)
            {
                LastLeapSecondJulianDate = RequiredLeapSecondJulianDate;
                LastLeapSecondValue = ReturnValue;
                return ReturnValue; // Return the assigned value
            }

        }

        /// <summary>
        /// Return today's DeltaT value
        /// </summary>
        /// <returns>DeltaT value as a double</returns>
        public double DeltaT()
        {
            double CurrentJulianDateUTC;
            CurrentJulianDateUTC = DateTime.UtcNow.ToOADate() + GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET; // Calculate today's Julian date

            lock (DeltaTLockObject)
            {
                if (Math.Truncate(CurrentJulianDateUTC - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET) == Math.Truncate(LastDeltaTJulianDate - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET)) // Return the cached value if its available otherwise calculate it and save the value for the next call
                {
                    LogDebugMessage("DeltaT", string.Format("Returning cached today's DeltaT value: {0}", LastDeltaTValue));
                    return LastDeltaTValue;
                }
            }

            return DeltaT(CurrentJulianDateUTC); // Return today's number of leap seconds

        }

        /// <summary>
        /// Return the specified Julian day's DeltaT value
        /// </summary>
        /// <param name="RequiredDeltaTJulianDateUTC"></param>
        /// <returns>DeltaT value as a double</returns>
        public double DeltaT(double RequiredDeltaTJulianDateUTC)
        {
            double DeltaUT1, ReturnValue;
            DateTime UTCDate;
            string DeltaUT1ValueName, DeltaUT1String;

            // Validate supplied Julian date - must be in the range [00:00:00 1 January 0100] to [23:59:59.999 31 December 9999]
            if (RequiredDeltaTJulianDateUTC < DELTAT_JULIAN_DATE_MINIMUM | RequiredDeltaTJulianDateUTC > DELTAT_JULIAN_DATE_MAXIMUM)
            {
                throw new InvalidValueException("The EarthRotationParameters.DeltaT(JD) Julian Date parameter", RequiredDeltaTJulianDateUTC.ToString(), DELTAT_JULIAN_DATE_MINIMUM.ToString(), DELTAT_JULIAN_DATE_MAXIMUM.ToString());
            }

            lock (DeltaTLockObject)
            {
                if (Math.Truncate(RequiredDeltaTJulianDateUTC - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET) == Math.Truncate(LastDeltaTJulianDate - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET)) // Return the cached value if its available otherwise calculate it and save the value for the next call
                {
                    LogDebugMessage("DeltaT(JD)", string.Format("Returning cached DeltaT value: {0} for UTC Julian date: {1} ({2})", LastDeltaTValue, RequiredDeltaTJulianDateUTC, DateTime.FromOADate(RequiredDeltaTJulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                    return LastDeltaTValue;
                }
            }

            // We don't have a cached value so compute one and save it for the next call
            switch (UpdateTypeValue ?? "")
            {
                case GlobalItems.UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1:

                    // Approach: calculate DELTA_T as =  CURRENT_LEAP_SECONDS + TT_TAI_OFFSET - DUT1
                    // Determine whether a downloaded DeltaUT1 value exists for the given UTC Julian date then perform the calculation above
                    // if yes then 
                    // Determine whether the value is a valid double number
                    // If it is then 
                    // Test whether the value is in the acceptable range -0.0 to +0.9
                    // If it is then return this value
                    // If not then fall back to the predicted approach
                    // If not then fall back to the predicted approach
                    // if no then fall back to the predicted approach

                    UTCDate = DateTime.FromOADate(RequiredDeltaTJulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET); // Convert the Julian day into a DateTime
                    DeltaUT1ValueName = string.Format(GlobalItems.DELTAUT1_VALUE_NAME_FORMAT, UTCDate.Year.ToString(GlobalItems.DELTAUT1_VALUE_NAME_YEAR_FORMAT), UTCDate.Month.ToString(GlobalItems.DELTAUT1_VALUE_NAME_MONTH_FORMAT), UTCDate.Day.ToString(GlobalItems.DELTAUT1_VALUE_NAME_DAY_FORMAT));
                    DeltaUT1String = profile.GetProfile(GlobalItems.AUTOMATIC_UPDATE_DELTAUT1_SUBKEY_NAME, DeltaUT1ValueName);
                    if (!string.IsNullOrEmpty(DeltaUT1String)) // We have got something back from the Profile so test whether it is a valid double number
                    {
                        if (double.TryParse(DeltaUT1String, NumberStyles.Float, CultureInfo.InvariantCulture, out DeltaUT1)) // We have a valid double number so check that it is the acceptable range
                        {
                            if (DeltaUT1 >= -GlobalItems.DELTAUT1_BOUND & DeltaUT1 <= GlobalItems.DELTAUT1_BOUND)
                            {

                                LogDebugMessage("DeltaT(JD)", string.Format("Automatic leap seconds and delta UT1 are required, found a good DeltaUT1 value so returning the calculated DeltaT value for Julian day: {0} ({1})", RequiredDeltaTJulianDateUTC, DateTime.FromOADate(RequiredDeltaTJulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                                ReturnValue = LeapSeconds() + GlobalItems.TT_TAI_OFFSET - DeltaUT1; // Calculate DeltaT using the valid DeltaUT1 value
                                LogDebugMessage("DeltaT(JD)", string.Format("Return value: {0} for Julian day: {1} ({2})", ReturnValue, RequiredDeltaTJulianDateUTC, DateTime.FromOADate(RequiredDeltaTJulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));

                                lock (DeltaTLockObject) // Update cache values and return the calculated value
                                {
                                    LastDeltaTJulianDate = RequiredDeltaTJulianDateUTC;
                                    LastDeltaTValue = ReturnValue;
                                    return ReturnValue;
                                }
                            }

                            else // We have a double value that is outside the expected range so fall through to the default predicted approach
                            {
                                LogDebugMessage("DeltaT(JD)", string.Format("Automatic leap seconds and delta UT1 are required, but the found DeltaUT1 value {0} from {1} was outside the correct range so falling through to the predicted approach for Julian day: {2} ({3})", DeltaUT1, DeltaUT1ValueName, RequiredDeltaTJulianDateUTC, DateTime.FromOADate(RequiredDeltaTJulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                            }
                        }
                        else // We have a profile value but it isn't a number so fall through to the default predicted approach
                        {
                            LogDebugMessage("DeltaT(JD)", string.Format("Automatic leap seconds and delta UT1 are required, but the Profile value {0} from {1} isn't a number so falling through to the predicted approach for Julian day: {2} ({3})", DeltaUT1String, DeltaUT1ValueName, RequiredDeltaTJulianDateUTC, DateTime.FromOADate(RequiredDeltaTJulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                        }
                    }
                    else // We have no profile value so fall through to the default predicted approach
                    {
                        LogDebugMessage("DeltaT(JD)", string.Format("Automatic leap seconds and delta UT1 are required, but no profile value was found for {0} so falling through to the predicted approach for Julian day: {1} ({2})", DeltaUT1ValueName, RequiredDeltaTJulianDateUTC, DateTime.FromOADate(RequiredDeltaTJulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                    }
                    break;

                case GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1:
                        // Approach: calculate DELTA_T as =  CURRENT_LEAP_SECONDS + TT_TAI_OFFSET - DUT1
                        // Determine whether the manual DeltaUT1 value is valid 
                        // if yes then use this value in the equation above
                        // if no then fall back to the predicted approach

                        if (ManualDeltaUT1Value != GlobalItems.DOUBLE_VALUE_NOT_AVAILABLE) // We have a valid manual delta UT1 value so use it 
                        {
                            LogDebugMessage("DeltaT(JD)", string.Format("Manual leap seconds and delta UT1 are required, found a good DeltaUT1 value so returning the calculated DeltaT value for Julian day: {0} ({1})", RequiredDeltaTJulianDateUTC, DateTime.FromOADate(RequiredDeltaTJulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                            ReturnValue = LeapSeconds() + GlobalItems.TT_TAI_OFFSET - ManualDeltaUT1Value; // Calculate DeltaT using the valid DeltaUT1 value
                            LogDebugMessage("DeltaT(JD)", string.Format("Return value: {0} for Julian day: {1} ({2})", ReturnValue, RequiredDeltaTJulianDateUTC, DateTime.FromOADate(RequiredDeltaTJulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));

                            lock (DeltaTLockObject) // Update cache values and return the calculated value
                            {
                                LastDeltaTJulianDate = RequiredDeltaTJulianDateUTC;
                                LastDeltaTValue = ReturnValue;
                                return ReturnValue;
                            }
                        }
                        else
                        {
                            LogDebugMessage("DeltaT(JD)", string.Format("Manual leap seconds and manual delta UT1 are required, but the DeltaUT1 value is not available or invalid so falling through to the predicted approach for Julian day: {0} ({1})", RequiredDeltaTJulianDateUTC, DateTime.FromOADate(RequiredDeltaTJulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                        }
                        break;

                case GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1:
                    {
                        LogDebugMessage("DeltaT(JD)", string.Format("Manual leap seconds and predicted delta UT1 are required, so falling through to the predicted approach for Julian day: {0} ({1})", RequiredDeltaTJulianDateUTC, DateTime.FromOADate(RequiredDeltaTJulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                        break;
                    }

                case GlobalItems.UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1:
                    {
                        LogDebugMessage("DeltaT(JD)", string.Format("Built-in leap seconds and predicted delta UT1 are required, so falling through to the predicted approach for Julian day: {0} ({1})", RequiredDeltaTJulianDateUTC, DateTime.FromOADate(RequiredDeltaTJulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                        break;
                    }

                default:
                    {
                        LogDebugMessage("DeltaT(JD)", "Unknown UpdateTypeValue: " + UpdateTypeValue);
                        MessageBox.Show("AstroUtils.DeltaT(JD) - Unknown UpdateTypeValue: " + UpdateTypeValue);
                        break;
                    }
            }

            // Calculate the predicted value and return it
            ReturnValue = DeltatCode.DeltaTCalc(RequiredDeltaTJulianDateUTC);
            LogDebugMessage("DeltaT(JD)", string.Format("Return value: {0} for Julian day: {1} ({2})", ReturnValue, RequiredDeltaTJulianDateUTC, DateTime.FromOADate(RequiredDeltaTJulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));

            lock (DeltaTLockObject)
            {
                LastDeltaTJulianDate = RequiredDeltaTJulianDateUTC;
                LastDeltaTValue = ReturnValue;
                return ReturnValue; // Return the assigned value
            }

        }

        /// <summary>
        /// Return today's DeltaUT1 value
        /// </summary>
        /// <returns>DeltaUT1 value as a double</returns>
        public double DeltaUT1()
        {
            double CurrentJulianDateUTC;
            CurrentJulianDateUTC = DateTime.UtcNow.ToOADate() + GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET; // Calculate today's Julian date

            lock (DeltaUT1LockObject)
            {
                if (Math.Truncate(CurrentJulianDateUTC - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET) == Math.Truncate(LastDeltaUT1JulianDate - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET)) // Return the cached value if its available otherwise calculate it and save the value for the next call
                {
                    LogDebugMessage("DeltaUT1", string.Format("Returning cached today's DeltaUT1 value: {0}", LastDeltaUT1Value));
                    return LastDeltaUT1Value;
                }
            }

            return DeltaUT1(CurrentJulianDateUTC); // Return today's delta UT1 value

        }

        /// <summary>
        /// Return the specified Julian day'DeltaUT1 value
        /// </summary>
        /// <param name="RequiredDeltaUT1JulianDateUTC"></param>
        /// <returns>DeltaUT1 value as a double</returns>
        public double DeltaUT1(double RequiredDeltaUT1JulianDateUTC)
        {
            double ReturnValue = default, ProfileValue;
            DateTime UTCDate;
            string DeltaUT1ValueName, DeltaUT1String;

            lock (DeltaTLockObject)
            {
                if (Math.Truncate(RequiredDeltaUT1JulianDateUTC - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET) == Math.Truncate(LastDeltaUT1JulianDate - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET)) // Return the cached value if its available otherwise calculate it and save the value for the next call
                {
                    LogDebugMessage("DeltaUT1(JD)", string.Format("Returning cached DeltaUT1 value: {0} for UTC Julian date: {1} ({2})", LastDeltaUT1Value, RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                    return LastDeltaUT1Value;
                }
            }

            // We don't have a cached value so compute one and save it for the next call

            switch (UpdateTypeValue ?? "")
            {
                case GlobalItems.UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1:
                case GlobalItems.UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1:
                    {
                        LogDebugMessage("DeltaUT1(JD)", string.Format("Automatic DeltaUT1 is required for Julian date: {0} ({1})", RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                        // Approach
                        // Determine whether a downloaded DeltaUT1 value exists for the specified Julian Day (in UTC time)
                        // if yes then 
                        // Determine whether the value is a valid double number
                        // If it is then 
                        // Test whether the value is in the acceptable range -0.0 to +0.9
                        // If it is then return this value
                        // If not then fall back to the predicted value
                        // If not then fall back to the predicted value
                        // if no then fall back to the predicted value
                        UTCDate = DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET); // Convert the Julian day into a DateTime
                        DeltaUT1ValueName = string.Format(GlobalItems.DELTAUT1_VALUE_NAME_FORMAT, UTCDate.Year.ToString(GlobalItems.DELTAUT1_VALUE_NAME_YEAR_FORMAT), UTCDate.Month.ToString(GlobalItems.DELTAUT1_VALUE_NAME_MONTH_FORMAT), UTCDate.Day.ToString(GlobalItems.DELTAUT1_VALUE_NAME_DAY_FORMAT));

                        DeltaUT1String = profile.GetProfile(GlobalItems.AUTOMATIC_UPDATE_DELTAUT1_SUBKEY_NAME, DeltaUT1ValueName);
                        if (!string.IsNullOrEmpty(DeltaUT1String)) // We have got something back from the Profile so test whether it is a valid double number
                        {
                            if (double.TryParse(DeltaUT1String, out ProfileValue)) // We have a valid double number so check that it is the acceptable range
                            {
                                if (ProfileValue >= -GlobalItems.DELTAUT1_BOUND & ProfileValue <= GlobalItems.DELTAUT1_BOUND)
                                {
                                    ReturnValue = ProfileValue;
                                    LogDebugMessage("DeltaUT1(JD)", string.Format("Automatic DeltaUT1 is required and a valid value has been found: {0} at Julian date: {1} ({2})", ReturnValue, RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                                }
                                else // We don't have a valid number so fall back to the predicted value
                                {
                                    LogDebugMessage("DeltaUT1(JD)", string.Format("Automatic DeltaUT1 is required but the Profile value {0} is outside the valid range: {1} - {2}, returning the predicted value at Julian date: {3} ({4})", ProfileValue, -GlobalItems.DELTAUT1_BOUND, GlobalItems.DELTAUT1_BOUND, RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                                    ReturnValue = LeapSeconds(RequiredDeltaUT1JulianDateUTC) + GlobalItems.TT_TAI_OFFSET - DeltaT(RequiredDeltaUT1JulianDateUTC);
                                    LogDebugMessage("DeltaUT1(JD)", string.Format("Return value: {0} for Julian day: {1} ({2})", ReturnValue, RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                                }
                            }
                            else // We have an invalid double value so fall back to the predicted value
                            {
                                LogDebugMessage("DeltaUT1(JD)", string.Format("Automatic DeltaUT1 is required but the Profile value {0} is not a valid double value, returning the predicted value at Julian date: {1} ({2})", ProfileValue, RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                                ReturnValue = LeapSeconds(RequiredDeltaUT1JulianDateUTC) + GlobalItems.TT_TAI_OFFSET - DeltaT(RequiredDeltaUT1JulianDateUTC);
                                LogDebugMessage("DeltaUT1(JD)", string.Format("Return value: {0} for Julian day: {1} ({2})", ReturnValue, RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                            }
                        }

                        else // No value for this date so fall back to the predicted value
                        {
                            LogDebugMessage("DeltaUT1(JD)", string.Format("Automatic DeltaUT1 is required but there is no value for the requested date in the Profile, returning the predicted value at Julian date: {0} ({1})", RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                            ReturnValue = LeapSeconds(RequiredDeltaUT1JulianDateUTC) + GlobalItems.TT_TAI_OFFSET - DeltaT(RequiredDeltaUT1JulianDateUTC);
                            LogDebugMessage("DeltaUT1(JD)", string.Format("Return value: {0} for Julian day: {1} ({2})", ReturnValue, RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                        }

                        break;
                    }

                case GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1: // This was the method in Platform 6.3 and earlier
                    {
                        LogDebugMessage("DeltaUT1(JD)", string.Format("Predicted DeltaUT1 is required so returning value determined from DeltaT calculation at Julian date: {0} ({1})", RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                        ReturnValue = LeapSeconds(RequiredDeltaUT1JulianDateUTC) + GlobalItems.TT_TAI_OFFSET - DeltaT(RequiredDeltaUT1JulianDateUTC);
                        LogDebugMessage("DeltaUT1(JD)", string.Format("Return value: {0} for Julian day: {1} ({2})", ReturnValue, RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                        break;
                    }

                case GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1:
                    {
                        ReturnValue = ManualDeltaUT1Value;
                        LogDebugMessage("DeltaUT1(JD)", string.Format("Manual DeltaUT1 is required so returning manually configured value {0} at Julian date: {1} ({2})", ReturnValue, RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                        break;
                    }

                case GlobalItems.UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1:
                    {
                        LogDebugMessage("DeltaUT1(JD)", string.Format("Built-in DeltaUT1 is required so returning value determined from DeltaT calculation at Julian date: {0} ({1})", RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                        ReturnValue = LeapSeconds(RequiredDeltaUT1JulianDateUTC) + GlobalItems.TT_TAI_OFFSET - DeltaT(RequiredDeltaUT1JulianDateUTC);
                        LogDebugMessage("DeltaUT1(JD)", string.Format("Return value: {0} for Julian day: {1} ({2})", ReturnValue, RequiredDeltaUT1JulianDateUTC, DateTime.FromOADate(RequiredDeltaUT1JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                        break;
                    }

                default:
                    {
                        LogMessage("DeltaUT1(JD)", "Unknown Parameters.UpdateType: " + UpdateTypeValue);
                        MessageBox.Show("AstroUtils.DeltaUT1 - Unknown Parameters.UpdateType: " + UpdateTypeValue);
                        break;
                    }

            }

            lock (DeltaUT1LockObject)
            {
                LastDeltaUT1JulianDate = RequiredDeltaUT1JulianDateUTC;
                LastDeltaUT1Value = ReturnValue;
                return ReturnValue; // Return the assigned value
            }

        }

        /// <summary>
        /// Refresh state values
        /// </summary>
        public void RefreshState()
        {
            string OriginalProfileValue;
            DateTime AutomaticScheduleTimeDefault;
            bool UriValid;

            var UpdateTypes = new List<string>() { GlobalItems.UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1, GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1, GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1, GlobalItems.UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1, GlobalItems.UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1 };

            var ScheduleRepeatOptions = new List<string>() { GlobalItems.SCHEDULE_REPEAT_NONE, GlobalItems.SCHEDULE_REPEAT_DAILY, GlobalItems.SCHEDULE_REPEAT_WEEKLY, GlobalItems.SCHEDULE_REPEAT_MONTHLY };

            LogDebugMessage("RefreshState", "");
            LogDebugMessage("RefreshState", "Start of Refresh");

            // Read all values from the Profile and validate them where possible. If they are corrupt then replace with default values

            if (DateTime.Now.Hour < 12) // Create a default schedule time for use in case a time hasn't been set yet
            {
                AutomaticScheduleTimeDefault = DateTime.Today.AddHours(12d);
            }
            else
            {
                AutomaticScheduleTimeDefault = DateTime.Today.AddHours(36d);
            }

            OriginalProfileValue = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.DOWNLOAD_TASK_SCHEDULED_TIME_VALUE_NAME, AutomaticScheduleTimeDefault.ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT, CultureInfo.InvariantCulture));
            if (DateTime.TryParseExact(OriginalProfileValue, GlobalItems.DOWNLOAD_TASK_TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DownloadTaskScheduledTimeValue))
            {
                LogDebugMessage("RefreshState", string.Format("DownloadTaskRunTimeValue = {0}", DownloadTaskScheduledTimeValue.ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT, CultureInfo.InvariantCulture)));
            }
            else
            {
                DownloadTaskScheduledTime = AutomaticScheduleTimeDefault;
                LogMessage("EarthRotParm CORRUPT!", string.Format("EarthRoationParameter DownloadTaskRunTimeValue is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskScheduledTimeValue.ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT, CultureInfo.InvariantCulture)));
                LogEvent(string.Format("EarthRoationParameter DownloadTaskRunTimeValue is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskScheduledTimeValue.ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT, CultureInfo.InvariantCulture)));
            }

            OriginalProfileValue = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.UPDATE_TYPE_VALUE_NAME, GlobalItems.UPDATE_TYPE_DEFAULT);
            if (UpdateTypes.Contains(OriginalProfileValue)) // The Profile value is one of the permitted values so we're done
            {
                UpdateTypeValue = OriginalProfileValue;
                LogDebugMessage("RefreshState", string.Format("UpdateTypeValue = {0}", UpdateTypeValue));
            }
            else // The Profile value is not a permitted value so replace it with the default value
            {
                UpdateType = GlobalItems.UPDATE_TYPE_DEFAULT;
                LogMessage("EarthRotParm CORRUPT!", string.Format("EarthRoationParameter UpdateType is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, UpdateTypeValue));
                LogEvent(string.Format("EarthRoationParameter UpdateType is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, UpdateTypeValue));
            }

            string ManualDeltaUT1String = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.MANUAL_DELTAUT1_VALUE_NAME, GlobalItems.MANUAL_DELTAUT1_DEFAULT.ToString(CultureInfo.InvariantCulture));
            if (double.TryParse(ManualDeltaUT1String, NumberStyles.Float, CultureInfo.InvariantCulture, out ManualDeltaUT1Value)) // String parsed OK so list value if debug is enabled
            {
                LogDebugMessage("RefreshState", string.Format("ManualDeltaUT1String = {0}, ManualDeltaUT1Value: {1}", ManualDeltaUT1String, ManualDeltaUT1Value));
            }
            else // Returned string doesn't represent a number so reapply the default
            {
                ManualDeltaUT1 = GlobalItems.MANUAL_DELTAUT1_DEFAULT;
                LogMessage("EarthRotParm CORRUPT!", string.Format("EarthRoationParameter ManualDeltaUT1 is corrupt: {0}, default value has been set: {1}", ManualDeltaUT1String, ManualDeltaUT1Value));
                LogEvent(string.Format("EarthRoationParameter ManualDeltaUT1 is corrupt: {0}, default value has been set: {1}", ManualDeltaUT1String, ManualDeltaUT1Value));
            }


            double NowJulian, CurrentLeapSeconds;
            NowJulian = DateTime.UtcNow.ToOADate() + GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET;

            // Find the leap second value from the built-in table of historic values
            CurrentLeapSeconds = CurrentBuiltInLeapSeconds;

            string ManualTaiUtcOffsetString = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.MANUAL_LEAP_SECONDS_VALUENAME, CurrentLeapSeconds.ToString(CultureInfo.InvariantCulture));
            if (double.TryParse(ManualTaiUtcOffsetString, NumberStyles.Float, CultureInfo.InvariantCulture, out ManualLeapSecondsValue)) // String parsed OK so list value if debug is enabled
            {
                LogDebugMessage("RefreshState", string.Format("ManualTaiUtcOffsetString = {0}, ManualTaiUtcOffsetValue: {1}", ManualTaiUtcOffsetString, ManualLeapSecondsValue));
            }
            else // Returned string doesn't represent a number so reapply the default
            {
                ManualLeapSeconds = CurrentLeapSeconds;
                LogMessage("EarthRotParm CORRUPT!", string.Format("EarthRoationParameter ManualTaiUtcOffset is corrupt: {0}, default value has been set: {1}", ManualTaiUtcOffsetString, ManualLeapSecondsValue));
                LogEvent(string.Format("EarthRoationParameter ManualTaiUtcOffset is corrupt: {0}, default value has been set: {1}", ManualTaiUtcOffsetString, ManualLeapSecondsValue));
            }

            EarthRotationDataLastUpdatedValue = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.EARTH_ROTATION_DATA_LAST_UPDATED_VALUE_NAME, GlobalItems.EARTH_ROTATION_DATA_LAST_UPDATED_DEFAULT);

            OriginalProfileValue = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.DOWNLOAD_TASK_DATA_SOURCE_VALUE_NAME, GlobalItems.DOWNLOAD_TASK_INTERNET_DATA_SOURCE_DEFAULT);
            UriValid = false; // Set the valid flag false, then set to true if the download source starts with a supported URI prefix
            if (OriginalProfileValue.StartsWith(GlobalItems.URI_PREFIX_HTTP, StringComparison.OrdinalIgnoreCase))
                UriValid = true;
            if (OriginalProfileValue.StartsWith(GlobalItems.URI_PREFIX_HTTPS, StringComparison.OrdinalIgnoreCase))
                UriValid = true;
            if (OriginalProfileValue.StartsWith(GlobalItems.URI_PREFIX_FTP, StringComparison.OrdinalIgnoreCase))
                UriValid = true;

            if (UriValid)
            {
                DownloadTaskDataSourceValue = OriginalProfileValue;
                LogDebugMessage("RefreshState", string.Format("DownloadTaskDataSourceValue = {0}", DownloadTaskDataSourceValue));
            }
            else
            {
                DownloadTaskDataSource = GlobalItems.DOWNLOAD_TASK_INTERNET_DATA_SOURCE_DEFAULT;
                LogMessage("EarthRotParm CORRUPT!", string.Format("EarthRoationParameter DownloadTaskDataSource is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskDataSourceValue));
                LogEvent(string.Format("EarthRoationParameter DownloadTaskDataSource is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskDataSourceValue));
            }

            OriginalProfileValue = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.DOWNLOAD_TASK_REPEAT_FREQUENCY_VALUE_NAME, GlobalItems.DOWNLOAD_TASK_REPEAT_DEFAULT);
            if (ScheduleRepeatOptions.Contains(OriginalProfileValue)) // The Profile value is one of the permitted values so we're done
            {
                DownloadTaskRepeatFrequencyValue = OriginalProfileValue;
                LogDebugMessage("RefreshState", string.Format("DownloadTaskRepeatFrequencyValue = {0}", DownloadTaskRepeatFrequencyValue));
            }
            else // The Profile value is not a permitted value so replace it with the default value
            {
                DownloadTaskRepeatFrequency = GlobalItems.DOWNLOAD_TASK_REPEAT_DEFAULT;
                LogMessage("EarthRotParm CORRUPT!", string.Format("EarthRoationParameter DownloadTaskRepeatFrequency is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskRepeatFrequencyValue));
                LogEvent(string.Format("EarthRoationParameter DownloadTaskRepeatFrequency is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskRepeatFrequencyValue));
            }

            string DownloadTaskTimeOutString = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.DOWNLOAD_TASK_TIMEOUT_VALUE_NAME, GlobalItems.DOWNLOAD_TASK_TIMEOUT_DEFAULT.ToString(CultureInfo.InvariantCulture));
            if (double.TryParse(DownloadTaskTimeOutString, NumberStyles.Float, CultureInfo.InvariantCulture, out DownloadTaskTimeOutValue)) // String parsed OK so list value if debug is enabled
            {
                LogDebugMessage("RefreshState", string.Format("DownloadTaskTimeOutString = {0}, DownloadTaskTimeOutValue: {1}", DownloadTaskTimeOutString, DownloadTaskTimeOutValue));
            }
            else // Returned string doesn't represent a number so reapply the default
            {
                DownloadTaskTimeOut = GlobalItems.DOWNLOAD_TASK_TIMEOUT_DEFAULT;
                LogMessage("EarthRotParm CORRUPT!", string.Format("EarthRoationParameter DownloadTaskTimeOut is corrupt: {0}, default value has been set: {1}", DownloadTaskTimeOutString, DownloadTaskTimeOutValue));
                LogEvent(string.Format("EarthRoationParameter DownloadTaskTimeOut is corrupt: {0}, default value has been set: {1}", DownloadTaskTimeOutString, DownloadTaskTimeOutValue));
            }

            string DownloadTaskTraceEnabledString = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.DOWNLOAD_TASK_TRACE_ENABLED_VALUE_NAME, GlobalItems.DOWNLOAD_TASK_TRACE_ENABLED_DEFAULT.ToString(CultureInfo.InvariantCulture));
            if (bool.TryParse(DownloadTaskTraceEnabledString, out DownloadTaskTraceEnabledValue)) // String parsed OK so list value if debug is enabled
            {
                LogDebugMessage("RefreshState", string.Format("DownloadTaskTraceEnabledString = {0}, DownloadTaskTraceEnabledValue: {1}", DownloadTaskTraceEnabledString, DownloadTaskTraceEnabledValue));
            }
            else // Returned string doesn't represent a boolean so reapply the default
            {
                DownloadTaskTraceEnabled = GlobalItems.DOWNLOAD_TASK_TRACE_ENABLED_DEFAULT;
                LogMessage("EarthRotParm CORRUPT!", string.Format("EarthRoationParameter DownloadTaskTraceEnabled is corrupt: {0}, default value has been set: {1}", DownloadTaskTraceEnabledString, DownloadTaskTraceEnabledValue));
                LogEvent(string.Format("EarthRoationParameter DownloadTaskTraceEnabled is corrupt: {0}, default value has been set: {1}", DownloadTaskTraceEnabledString, DownloadTaskTraceEnabledValue));
            }

            // Get the configured trace file directory And make sure that it exists
            OriginalProfileValue = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.DOWNLOAD_TASK_TRACE_PATH_VALUE_NAME, string.Format(GlobalItems.DOWNLOAD_TASK_TRACE_DEFAULT_PATH_FORMAT, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))).TrimEnd('\\');
            try
            {
                Directory.CreateDirectory(OriginalProfileValue); // Make sure we can create the directory or it already exists
                DownloadTaskTracePathValue = OriginalProfileValue;
                LogDebugMessage("RefreshState", string.Format("DownloadTaskTracePathValue = {0}", DownloadTaskTracePathValue));
            }
            catch (Exception ex) // Something went wrong so restore the default value
            {
                LogMessage("EarthRotParm CORRUPT!", string.Format("Exception thrown: {0}", ex.ToString()));
                LogMessage("EarthRotParm CORRUPT!", string.Format("EarthRoationParameter DownloadTaskTracePath is corrupt: {0}, default value will be set: {1}", OriginalProfileValue, DownloadTaskTracePathValue));
                LogEvent(string.Format("EarthRoationParameter DownloadTaskTracePath is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, DownloadTaskTracePathValue));
                DownloadTaskTracePath = string.Format(GlobalItems.DOWNLOAD_TASK_TRACE_DEFAULT_PATH_FORMAT, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).TrimEnd('\\'); // restore the default path
            }

            AutomaticLeapSecondsValue = GlobalItems.DOUBLE_VALUE_NOT_AVAILABLE; // Initialise value as not available
            OriginalProfileValue = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.AUTOMATIC_LEAP_SECONDS_VALUENAME, GlobalItems.AUTOMATIC_LEAP_SECONDS_NOT_AVAILABLE_DEFAULT);
            if ((OriginalProfileValue ?? "") == GlobalItems.AUTOMATIC_LEAP_SECONDS_NOT_AVAILABLE_DEFAULT) // Has the default value so is OK
            {
                AutomaticLeapSecondsStringValue = OriginalProfileValue;
                LogDebugMessage("RefreshState", string.Format("AutomaticLeapSecondsStringValue: {0}", AutomaticLeapSecondsStringValue));
            }
            else if (double.TryParse(OriginalProfileValue, NumberStyles.Float, CultureInfo.InvariantCulture, out AutomaticLeapSecondsValue)) // Not default so it should be parse-able
                                                                                                                                             // String parsed OK so save value and list it if debug is enabled
            {
                AutomaticLeapSecondsStringValue = OriginalProfileValue;
                LogDebugMessage("RefreshState", string.Format("AutomaticLeapSecondsStringValue: {0}, AutomaticLeapSecondsValue: {1}", AutomaticLeapSecondsStringValue, AutomaticLeapSecondsValue));
            }
            else // Returned string doesn't represent a number so reapply the default
            {
                AutomaticLeapSecondsString = GlobalItems.AUTOMATIC_LEAP_SECONDS_NOT_AVAILABLE_DEFAULT;
                LogMessage("EarthRotParm CORRUPT!", string.Format("EarthRoationParameter AutomaticLeapSecondsString is corrupt: {0}, default value has been set: {1}, AutomaticLeapSecondsValue: {2}", OriginalProfileValue, AutomaticLeapSecondsStringValue, AutomaticLeapSecondsValue.ToString()));
                LogEvent(string.Format("EarthRoationParameter AutomaticLeapSecondsString is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, AutomaticLeapSecondsStringValue));
            }

            NextLeapSecondsValue = GlobalItems.DOUBLE_VALUE_NOT_AVAILABLE; // Initialise value as not available
            OriginalProfileValue = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.NEXT_LEAP_SECONDS_VALUENAME, GlobalItems.NEXT_LEAP_SECONDS_NOT_AVAILABLE_DEFAULT);
            if ((OriginalProfileValue ?? "") == GlobalItems.NEXT_LEAP_SECONDS_NOT_AVAILABLE_DEFAULT | (OriginalProfileValue ?? "") == GlobalItems.DOWNLOAD_TASK_NEXT_LEAP_SECONDS_NOT_PUBLISHED_MESSAGE) // Has the default or not published value so is OK
            {
                NextLeapSecondsStringValue = OriginalProfileValue;
                LogDebugMessage("RefreshState", string.Format("NextLeapSecondsStringValue: {0}", NextLeapSecondsStringValue));
            }
            else if (double.TryParse(OriginalProfileValue, NumberStyles.Float, CultureInfo.InvariantCulture, out NextLeapSecondsValue)) // Not default so it should be parse-able
                                                                                                                                        // String parsed OK so save value and list it if debug is enabled
            {
                NextLeapSecondsStringValue = OriginalProfileValue;
                LogDebugMessage("RefreshState", string.Format("NextLeapSecondsStringValue: {0}, NextLeapSecondsValue: {1}", NextLeapSecondsStringValue, NextLeapSecondsValue));
            }
            else // Returned string doesn't represent a number so reapply the default
            {
                NextLeapSecondsString = GlobalItems.NEXT_LEAP_SECONDS_NOT_AVAILABLE_DEFAULT;
                LogMessage("EarthRotParm CORRUPT!", string.Format("EarthRoationParameter NextLeapSecondsString is corrupt: {0}, default value has been set: {1}, NextLeapSecondsValue: {2}", OriginalProfileValue, NextLeapSecondsStringValue, NextLeapSecondsValue));
                LogEvent(string.Format("EarthRoationParameter NextLeapSecondsString is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, NextLeapSecondsStringValue));
            }

            NextLeapSecondsDateValue = GlobalItems.DATE_VALUE_NOT_AVAILABLE; // Initialise value as not available
            OriginalProfileValue = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.NEXT_LEAP_SECONDS_DATE_VALUENAME, GlobalItems.NEXT_LEAP_SECONDS_DATE_NOT_AVAILABLE_DEFAULT);
            if ((OriginalProfileValue ?? "") == GlobalItems.NEXT_LEAP_SECONDS_DATE_NOT_AVAILABLE_DEFAULT | (OriginalProfileValue ?? "") == GlobalItems.DOWNLOAD_TASK_NEXT_LEAP_SECONDS_NOT_PUBLISHED_MESSAGE) // Has the default or not published value so is OK
            {
                NextLeapSecondsDateStringValue = OriginalProfileValue;
                LogDebugMessage("RefreshState", string.Format("AutomaticNextTaiUtcOffsetDateValue = {0}", NextLeapSecondsDateStringValue));
            }
            else if (DateTime.TryParseExact(OriginalProfileValue, GlobalItems.DOWNLOAD_TASK_TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out NextLeapSecondsDateValue)) // Not default so it should be parse-able
            {
                NextLeapSecondsDateStringValue = OriginalProfileValue;
                LogDebugMessage("RefreshState", string.Format("NextLeapSecondsDateStringValue = {0}, NextLeapSecondsDateValue: {1}", NextLeapSecondsDateStringValue, NextLeapSecondsDateValue.ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
            }
            else
            {
                NextLeapSecondsDateString = GlobalItems.DOWNLOAD_TASK_NEXT_LEAP_SECONDS_NOT_PUBLISHED_MESSAGE;
                LogMessage("EarthRotParm CORRUPT!", string.Format("EarthRoationParameter NextLeapSecondsDateStringValue is corrupt: {0}, default value has been set: {1}, NextLeapSecondsDateValue: {2}", OriginalProfileValue, NextLeapSecondsDateStringValue, NextLeapSecondsDateValue.ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
                LogEvent(string.Format("EarthRoationParameter NextLeapSecondsDateStringValue is corrupt: {0}, default value has been set: {1}", OriginalProfileValue, NextLeapSecondsDateStringValue));
            }

            // Read in the leap second history values
            var ProfileLeapSecondsValueStrings = new SortedList<string, string>();
            var ProfileLeapSecondsValues = new SortedList<double, double>();
            bool ProfileLeapSecondDateOk, ProfileLeapSecondValueOk;
            double ProfileLeapSecondDate, ProfileLeapSecondsValue;

            LogDebugMessage("RefreshState", "");
            LogDebugMessage("RefreshState", string.Format("Reading historic leap seconds from Profile"));

            try
            {
                ProfileLeapSecondsValueStrings = profile.EnumProfile(GlobalItems.AUTOMATIC_UPDATE_LEAP_SECOND_HISTORY_SUBKEY_NAME);
            }
            catch (NullReferenceException ) // Key does not exist so supply an empty sorted list
            {
                LogDebugMessage("RefreshState", string.Format("Profile key does not exist - there are no downloaded leap second values"));
                DownloadedLeapSecondValues = new SortedList<double, double>();
            }
            LogDebugMessage("RefreshState", string.Format("Found {0} leap second values in the Profile", ProfileLeapSecondsValueStrings.Count));

            // Parse the JulianDate-LeapSecond string pairs into double values and save them if they are valid
            foreach (KeyValuePair<string, string> ProfileLeapSecondKeyValuePair in ProfileLeapSecondsValueStrings)
            {
                ProfileLeapSecondDateOk = double.TryParse(ProfileLeapSecondKeyValuePair.Key, NumberStyles.Float, CultureInfo.InvariantCulture, out ProfileLeapSecondDate); // Validate the Julian date as a double
                if (ProfileLeapSecondDateOk) // Check that it is in the valid range to be use with dateTime.FromOADate
                {
                    if (ProfileLeapSecondDate < -657435.0d | ProfileLeapSecondDate >= 2958466.0d)
                    {
                        ProfileLeapSecondDateOk = false;
                        LogMessage("RefreshState", string.Format("Invalid leap second date: {0}, the valid range is -657435.0 to 2958465.999999", ProfileLeapSecondDate));
                    }
                }
                ProfileLeapSecondValueOk = double.TryParse(ProfileLeapSecondKeyValuePair.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out ProfileLeapSecondsValue); // Validate the leap seconds value as a double
                if (ProfileLeapSecondDateOk & ProfileLeapSecondValueOk) // Both values are valid doubles so add them to the collection
                {
                    ProfileLeapSecondsValues.Add(ProfileLeapSecondDate, ProfileLeapSecondsValue);
                }
                else
                {
                    LogMessage("RefreshState", string.Format("Omitted Profile leap Second value JD: {0}, LeapSeconds: {1}, {2} {3}", ProfileLeapSecondKeyValuePair.Key, ProfileLeapSecondKeyValuePair.Value, ProfileLeapSecondDateOk, ProfileLeapSecondValueOk));
                }
            }

            // List the current contents of the historic leap second list
            foreach (KeyValuePair<double, double> LeapSecond in DownloadedLeapSecondValues)
            {
                var LeapSecondDateTime = DateTime.FromOADate(LeapSecond.Key - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET);
                LogDebugMessage("RefreshState", string.Format("Found historic leap second value {0} implemented on JD {1} ({2})", LeapSecond.Value, LeapSecond.Key, LeapSecondDateTime.ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
            }

            // List the new values that will replace the current values
            foreach (KeyValuePair<double, double> LeapSecond in ProfileLeapSecondsValues)
            {
                var LeapSecondDateTime = DateTime.FromOADate(LeapSecond.Key - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET);
                LogDebugMessage("RefreshState", string.Format("Found profile leap second value {0} implemented on JD {1} ({2})", LeapSecond.Value, LeapSecond.Key, LeapSecondDateTime.ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
            }

            if (ProfileLeapSecondsValues.Count > 0) // If there are any values in the Profile
            {
                DownloadedLeapSecondValues = ProfileLeapSecondsValues; // Save the them for future use
                LogDebugMessage("RefreshState", string.Format("Profile values ({0}) saved to HistoricLeapSecondValues.", DownloadedLeapSecondValues.Count));
            }

            // Invalidate caches
            LastLeapSecondJulianDate = GlobalItems.DOUBLE_VALUE_NOT_AVAILABLE; // Invalidate the cache so that any new values will be used
            LastDeltaTJulianDate = GlobalItems.DOUBLE_VALUE_NOT_AVAILABLE;
            LastDeltaUT1JulianDate = GlobalItems.DOUBLE_VALUE_NOT_AVAILABLE;

            LogDebugMessage("RefreshState", "End of Refresh");
            LogDebugMessage("RefreshState", "");

        }

        /// <summary>
        /// Number of leap seconds as downloaded from the Internet
        /// </summary>
        public SortedList<double, double> DownloadedLeapSeconds
        {
            get
            {
                return DownloadedLeapSecondValues;
            }
        }

        /// <summary>
        /// Update the scheduled Windows task with new parameters specified by the user through the Diagnostics Earth Rotation Parameters dialogue.
        /// </summary>
        public void ManageScheduledTask()
        {
            TaskDefinition taskDefinition;
            TimeTrigger timeTrigger;
            DailyTrigger dailyTrigger;
            WeeklyTrigger weeklyTrigger;
            MonthlyTrigger monthlyTrigger;
            var dayOfMonth = new int[1];
            string executableName;

            try
            {
                LogScheduledTaskMessage("", "");
                LogScheduledTaskMessage("ManageScheduledTask", "Testing whether scheduler is running");

                // Test whether the Task Scheduler Service is running so that the ASCOM task can be managed
                using (var serviceController = new ServiceController(SCHEDULER_SERVICE_NAME)) // Create a new service controller for the scheduler service
                {

                    if (serviceController.Status == ServiceControllerStatus.Running) // The scheduler is running normally so proceed with creating or updating the ASCOM EarthRotation parameters update task
                    {
                        LogScheduledTaskMessage("ManageScheduledTask", $"Scheduler service is running OK - status: {serviceController.Status}. Obtaining Scheduler information...");
                        using (var service = new TaskService())
                        {

                            LogScheduledTaskMessage("ManageScheduledTask", string.Format("Highest supported scheduler version: {0}, Library version: {1}, Connected: {2}", service.HighestSupportedVersion, TaskService.LibraryVersion, service.Connected));

                            // List current task state if any
                            var ASCOMTask = service.GetTask(GlobalItems.DOWNLOAD_TASK_PATH);
                            if (ASCOMTask is not null)
                            {
                                LogScheduledTaskMessage("ManageScheduledTask", string.Format("Found ASCOM task {0} last run: {1}, State: {2}, Enabled: {3}", ASCOMTask.Path, ASCOMTask.LastRunTime, ASCOMTask.State, ASCOMTask.Enabled));
                            }
                            else
                            {
                                LogScheduledTaskMessage("ManageScheduledTask", "ASCOM task does not exist");
                            }
                            LogScheduledTaskMessage("", "");

                            switch (UpdateTypeValue ?? "")
                            {
                                case GlobalItems.UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1:
                                case GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1:
                                case GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1:
                                case GlobalItems.UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1: // Just remove the update job if it exists so that it can't run
                                    {
                                        if (ASCOMTask is not null)
                                        {
                                            LogScheduledTaskMessage("ManageScheduledTask", string.Format("Update type is {0} and {1} task exists so it will be deleted.", UpdateTypeValue, GlobalItems.DOWNLOAD_TASK_NAME));
                                            service.RootFolder.DeleteTask(GlobalItems.DOWNLOAD_TASK_NAME);
                                            LogScheduledTaskMessage("ManageScheduledTask", string.Format("Task {0} deleted OK.", GlobalItems.DOWNLOAD_TASK_NAME));
                                        }
                                        else
                                        {
                                            LogScheduledTaskMessage("ManageScheduledTask", string.Format("Update type is {0} and {1} task does not exist so no action.", UpdateTypeValue, GlobalItems.DOWNLOAD_TASK_NAME));
                                        }

                                        break;
                                    }

                                case GlobalItems.UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1: // Create a new or Update the existing scheduled job
                                    {
                                        // Get the task definition to work on, either a new one or the existing task, if it exists
                                        if (ASCOMTask is not null)
                                        {
                                            LogScheduledTaskMessage("ManageScheduledTask", string.Format("Update type is {0} and {1} task exists so it will be deleted.", UpdateTypeValue, GlobalItems.DOWNLOAD_TASK_NAME));
                                            service.RootFolder.DeleteTask(GlobalItems.DOWNLOAD_TASK_NAME);
                                            LogScheduledTaskMessage("ManageScheduledTask", string.Format("Task {0} deleted OK.", GlobalItems.DOWNLOAD_TASK_NAME));
                                        }
                                        LogScheduledTaskMessage("ManageScheduledTask", string.Format("{0} task will be created.", GlobalItems.DOWNLOAD_TASK_NAME));
                                        taskDefinition = service.NewTask();

                                        taskDefinition.RegistrationInfo.Description = "ASCOM scheduled job to update earth rotation data: leap seconds and delta UT1. This job is managed through the ASCOM Diagnostics application and should not be manually edited.";

                                        executableName = Process.GetCurrentProcess().MainModule.FileName; // Get the full path and name of the current executable
                                        LogScheduledTaskMessage("ManageScheduledTask", string.Format("Current Executable process full name and path: {0}", executableName));

                                        executableName = Path.GetDirectoryName(executableName); // Extract the path component of the full file name
                                        executableName += @"\EarthRotationUpdate.exe"; // Append the name of the earth rotation update executable
                                        LogScheduledTaskMessage("ManageScheduledTask", string.Format("EarthRotationUpdate process full name and path: {0}", executableName));

                                        taskDefinition.Actions.Clear(); // Remove any existing actions and add the current one
                                        taskDefinition.Actions.Add(new ExecAction(executableName, null, null)); // Add an action that will launch the updater application whenever the trigger fires
                                                                                                                // LogScheduledTaskMessage("UpdateTypeEvent", String.Format("", ))
                                        LogScheduledTaskMessage("ManageScheduledTask", string.Format("Added scheduled job action to run {0}", executableName));

                                        // Add settings appropriate to the task
                                        try
                                        {
                                            taskDefinition.Settings.AllowDemandStart = true; // Requires a V2 task library (XP is only V1)
                                            taskDefinition.Settings.StartWhenAvailable = true; // ' Requires a V2 task library (XP is only V1)
                                            LogScheduledTaskMessage("ManageScheduledTask", string.Format("Successfully added V2 AllowDemandStart and StartWhenAvailable settings."));
                                        }
                                        catch (NotV1SupportedException ) // Swallow the not supported exception on XP
                                        {
                                            LogScheduledTaskMessage("ManageScheduledTask", string.Format("This machine only has a V1 task scheduler - ignoring V2 AllowDemandStart and StartWhenAvailable settings."));
                                        }
                                        taskDefinition.Settings.ExecutionTimeLimit = new TimeSpan(0, 10, 0);
                                        taskDefinition.Settings.StopIfGoingOnBatteries = false;
                                        taskDefinition.Settings.DisallowStartIfOnBatteries = false;
                                        taskDefinition.Settings.Enabled = true;
                                        LogScheduledTaskMessage("ManageScheduledTask", string.Format("Allow demand on start: {0}, Start when available: {1}, Execution time limit: {2} minutes, Stop if going on batteries: {3}, Disallow start if on batteries: {4}, Enabled: {5}, Run only if logged on: {6}", taskDefinition.Settings.AllowDemandStart, taskDefinition.Settings.StartWhenAvailable, taskDefinition.Settings.ExecutionTimeLimit.TotalMinutes, taskDefinition.Settings.StopIfGoingOnBatteries, taskDefinition.Settings.DisallowStartIfOnBatteries, taskDefinition.Settings.Enabled, taskDefinition.Settings.RunOnlyIfLoggedOn));

                                        taskDefinition.Triggers.Clear(); // Remove any previous triggers and add the new trigger to the task as the only trigger
                                        switch (DownloadTaskRepeatFrequencyValue ?? "")
                                        {
                                            case GlobalItems.SCHEDULE_REPEAT_NONE: // Execute once at the specified day and time
                                                {
                                                    timeTrigger = new TimeTrigger();
                                                    timeTrigger.StartBoundary = DownloadTaskScheduledTimeValue; // Add the user supplied date / time to the trigger
                                                    taskDefinition.Triggers.Add(timeTrigger);
                                                    LogScheduledTaskMessage("ManageScheduledTask", string.Format("Set trigger to run the job once at the specified time."));
                                                    break;
                                                }

                                            case GlobalItems.SCHEDULE_REPEAT_DAILY: // Execute daily at the specified time
                                                {
                                                    dailyTrigger = new DailyTrigger();
                                                    dailyTrigger.StartBoundary = DownloadTaskScheduledTimeValue; // Add the user supplied date / time to the trigger
                                                    taskDefinition.Triggers.Add(dailyTrigger);
                                                    LogScheduledTaskMessage("ManageScheduledTask", string.Format("Set trigger to repeat the job daily at the specified time."));
                                                    break;
                                                }

                                            case GlobalItems.SCHEDULE_REPEAT_WEEKLY: // Execute once per week on the specified day of week
                                                {
                                                    weeklyTrigger = new WeeklyTrigger();
                                                    weeklyTrigger.StartBoundary = DownloadTaskScheduledTimeValue; // Add the user supplied date / time to the trigger
                                                    switch (DownloadTaskScheduledTimeValue.DayOfWeek)
                                                    {
                                                        case DayOfWeek.Sunday:
                                                            {
                                                                weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Sunday; // Set the specific day of the week when the task is required to run
                                                                break;
                                                            }
                                                        case DayOfWeek.Monday:
                                                            {
                                                                weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Monday; // Set the specific day of the week when the task is required to run
                                                                break;
                                                            }
                                                        case DayOfWeek.Tuesday:
                                                            {
                                                                weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Tuesday; // Set the specific day of the week when the task is required to run
                                                                break;
                                                            }
                                                        case DayOfWeek.Wednesday:
                                                            {
                                                                weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Wednesday; // Set the specific day of the week when the task is required to run
                                                                break;
                                                            }
                                                        case DayOfWeek.Thursday:
                                                            {
                                                                weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Thursday; // Set the specific day of the week when the task is required to run
                                                                break;
                                                            }
                                                        case DayOfWeek.Friday:
                                                            {
                                                                weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Friday; // Set the specific day of the week when the task is required to run
                                                                break;
                                                            }
                                                        case DayOfWeek.Saturday:
                                                            {
                                                                weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Saturday; // Set the specific day of the week when the task is required to run
                                                                break;
                                                            }
                                                    }
                                                    taskDefinition.Triggers.Add(weeklyTrigger);
                                                    LogScheduledTaskMessage("ManageScheduledTask", string.Format("Set trigger to repeat the job weekly on day {0} at the specified time.", DownloadTaskScheduledTimeValue.DayOfWeek.ToString()));
                                                    break;
                                                }

                                            case GlobalItems.SCHEDULE_REPEAT_MONTHLY: // Execute once per month on the specified day number of the month
                                                {
                                                    monthlyTrigger = new MonthlyTrigger();
                                                    monthlyTrigger.StartBoundary = DownloadTaskScheduledTimeValue; // Add the user supplied date / time to the trigger
                                                    dayOfMonth[0] = DownloadTaskScheduledTimeValue.Day; // Save the specific day on which the task is to run
                                                    monthlyTrigger.DaysOfMonth = dayOfMonth; // Set the specific day of the month when the task is required to run
                                                    monthlyTrigger.MonthsOfYear = MonthsOfTheYear.AllMonths;
                                                    taskDefinition.Triggers.Add(monthlyTrigger);
                                                    LogScheduledTaskMessage("ManageScheduledTask", string.Format("Set trigger to repeat the job monthly on day {0} of the month at the specified time.", dayOfMonth[0]));
                                                    break;
                                                }

                                            default:
                                                {
                                                    MessageBox.Show(string.Format("ManageScheduledTask - Unknown type of DownloadTaskRepeatFrequencyValue: {0}", DownloadTaskRepeatFrequencyValue));
                                                    break;
                                                }

                                        }

                                        // Implement the new task in the root folder either by updating the existing task or creating a new task
                                        LogScheduledTaskMessage("ManageScheduledTask", $"Registering the {GlobalItems.DOWNLOAD_TASK_NAME} task.");
                                        service.RootFolder.RegisterTaskDefinition(GlobalItems.DOWNLOAD_TASK_NAME, taskDefinition, TaskCreation.CreateOrUpdate, "SYSTEM", null, TaskLogonType.ServiceAccount);
                                        LogScheduledTaskMessage("ManageScheduledTask", string.Format("New task registered OK."));
                                        break;
                                    }

                                default:
                                    {
                                        MessageBox.Show(string.Format("UpdateType - Unknown type of EarthRotationDataUpdateType: {0}", UpdateTypeValue));
                                        break;
                                    }
                            }

                        }
                    }
                    else // The task scheduler is not running so provide a message
                    {
                        string message = $"The ASCOM EarthRotation scheduled task cannot be created / updated because your PC's task scheduler is in the: {serviceController.Status} state. Please ensure that this service is running correctly, then repair the ASCOM installation.";
                        LogScheduledTaskMessage("ManageScheduledTask", message);
                        MessageBox.Show(message);
                    }

                }
            }
            catch (Exception ex)
            {
                Utilities.Global.LogEvent("ManageScheduledTask", $"ManageScheduledTask - Unexpected exception: {ex}", EventLogEntryType.Error, Global.EventLogErrors.ManageScheduledTask, ex.ToString());
                try
                {
                    LogScheduledTaskMessage("ManageScheduledTask Exception", ex.ToString());
                }
                catch (Exception )
                {
                }

                try
                {
                    MessageBox.Show(@"Something went wrong with the update, please report this on the ASCOM Talk Groups.IO forum, including a zip of your entire Documents\ASCOM folder and sub-folders." + "\r\n" + ex.ToString());
                }
                catch (Exception)
                {
                }

                Console.WriteLine($"ManageScheduledTask Exception - {ex})");
            }

            LogScheduledTaskMessage("", "");
            LogScheduledTaskMessage("ManageScheduledTask", string.Format("Earth rotation data update configuration changes completed."));
        }

        #endregion

        #region Support code

        private void LogScheduledTaskMessage(string Source, string Message)
        {
            if (TL is not null)
            {
                TL.LogMessageCrLf(Source, Message);
            }
            else
            {
                Utilities.Global.LogEvent(Source, Message, EventLogEntryType.Information, Global.EventLogErrors.EarthRotationUpdate, "");
            }

        }

        private void LogMessage(string Source, string Message)
        {
            if (TL is not null)
                TL.LogMessage(Source, Message);
        }

        private void LogDebugMessage(string Source, string Message)
        {
            if (TL is not null & DebugTraceEnabled)
                TL.LogMessage(Source, Message);
        }

        private void LogEvent(string message)
        {
            Utilities.Global.LogEvent("EarthRotationUpdate", message, EventLogEntryType.Warning, Global.EventLogErrors.EarthRotationUpdate, "");
        }

        private double BuiltInLeapSeconds(double RequiredLeapSecondJulianDate)
        {
            var ReturnValue = default(double);

            // Find the leap second value from the built-in table of historic values
            for (int i = BuiltInLeapSecondsValues.Count - 1; i >= 0; i -= 1)
            {
                LogDebugMessage("BuiltInLeapSeconds(JD)", string.Format("Searching built-in JD {0} with leap second: {1}", BuiltInLeapSecondsValues.Keys[i], BuiltInLeapSecondsValues.Values[i]));

                if (Math.Truncate(RequiredLeapSecondJulianDate - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET) >= Math.Truncate(BuiltInLeapSecondsValues.Keys[i] - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET)) // Found a match
                {
                    ReturnValue = BuiltInLeapSecondsValues.Values[i];
                    LogDebugMessage("BuiltInLeapSeconds(JD)", string.Format("Found built-in leap second: {0} set on JD {1}", BuiltInLeapSecondsValues.Values[i], BuiltInLeapSecondsValues.Keys[i]));
                    break;
                }
            }
            LogDebugMessage("BuiltInLeapSeconds(JD)", string.Format("Returning built-in leap seconds value: {0} for JD {1} ({2})", ReturnValue, RequiredLeapSecondJulianDate, DateTime.FromOADate(RequiredLeapSecondJulianDate - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET).ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
            return ReturnValue;

        }

        #endregion

    }
}