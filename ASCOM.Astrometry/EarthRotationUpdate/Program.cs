using System;
using System.IO;
using System.Net;
using System.Text;
using ASCOM.Utilities;
using ASCOM.Astrometry;
using System.Diagnostics;
using System.Security.Principal;
using System.Globalization;
using System.Threading;
using static ASCOM.Utilities.Global;

namespace EarthRotationUpdate
{
    class Program
    {
        private static TraceLogger TL;
        private static bool DownloadComplete;
        private static double DownloadTimeout;
        private static string hostURIString;
        private static EarthRotationParameters parameters;
        private static CultureInfo invariantCulture;
        private static TextInfo invariantTextInfo;
        private static Exception downloadError;
        private static string[] monthAbbrev;
        private static int ReturnCode;

        private static readonly DateTime UNKNOWN_DATE = DateTime.MinValue; // Value for dates which have not yet been determined
        private static readonly double UNKNOWN_LEAP_SECONDS = double.MinValue; // Value for dates which have not yet been determined

        static void Main(string[] args)
        {
            try
            {
                // In December 2020, ascom-standards.org minimum SSL protocol version is TLS1.2
                // Normally, this application will use the OS default SSL protocol, but Windows 7 default protocol is earlier that TLS1.2 and connection cannot be established to ascom-standards.org from this OS.
                // The following command ensures that TLS1.2 will be tried in addition to the OS default protocol.
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.SystemDefault;

                // Get some basic details for this run
                string runBy = WindowsIdentity.GetCurrent().Name; // Get the name of the user executing this program
                bool isSystem = WindowsIdentity.GetCurrent().IsSystem;
                DateTime now = DateTime.Now;

                ReturnCode = 0; // Initialise return code to success

                // Get access to the ASCOM Profile store and retrieve the trace logger configuration
                RegistryAccess profile = new RegistryAccess();
                string traceFileName = "";
                string traceBasePath = "";
                if (isSystem) // If we are running as user SYSTEM, create our own trace file name so that all scheduled job trace files end up in the same directory
                {
                    // Get the configured trace file directory and make sure that it exists
                    traceBasePath = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY,
                                                              GlobalItems.DOWNLOAD_TASK_TRACE_PATH_VALUE_NAME,
                                                              string.Format(GlobalItems.DOWNLOAD_TASK_TRACE_DEFAULT_PATH_FORMAT, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
                                                              ).TrimEnd('\\');
                    Directory.CreateDirectory(traceBasePath);
                    // Now make the full trace file name from the path above and the file name format template
                    traceFileName = string.Format(GlobalItems.DOWNLOAD_TASK_TRACE_FILE_NAME_FORMAT, traceBasePath, now.Year, now.Month, now.Day, now.Hour.ToString("00"), now.Minute.ToString("00"), now.Second.ToString("00"));
                }

                // Get the trace state from the Profile
                string DownloadTaskTraceEnabledString = profile.GetProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.DOWNLOAD_TASK_TRACE_ENABLED_VALUE_NAME, GlobalItems.DOWNLOAD_TASK_TRACE_ENABLED_DEFAULT.ToString());
                if (!Boolean.TryParse(DownloadTaskTraceEnabledString, out bool DownloadTaskTraceEnabledValue)) //' String parsed OK so no further action
                { //'Returned string doesn't represent a boolean so use the default
                    DownloadTaskTraceEnabledValue = GlobalItems.DOWNLOAD_TASK_TRACE_ENABLED_DEFAULT;
                }

                // Create the trace logger with either the supplied fully qualified name if running as SYSTEM or an automatic file name if running as a normal user
                try
                {
                    TL = new TraceLogger(traceFileName, GlobalItems.DOWNLOAD_TASK_TRACE_LOG_FILETYPE);
                    TL.Enabled = DownloadTaskTraceEnabledValue; // Set the trace state
                    TL.IdentifierWidth = GlobalItems.DOWNLOAD_TASK_TRACE_LOGGER_IDENTIFIER_FIELD_WIDTH;
                }
                catch (Exception ex)
                {
                    LogMessage("EarthRotationUpdate", $"Unable to create Trace Logger: {ex}");
                }

                invariantCulture = new CultureInfo("");
                invariantCulture.Calendar.TwoDigitYearMax = 2117; // Specify that two digit years will be converted to the range 2018-2117 - Likely to outlast ASCOM Platform and me!
                monthAbbrev = invariantCulture.DateTimeFormat.AbbreviatedMonthNames; // Get the 12 three letter abbreviated month names of the year
                invariantTextInfo = invariantCulture.TextInfo;

                LogMessage("EarthRotationUpdate", string.Format("InstalledUICulture: {0}, CurrentUICulture: {1}, CurrentCulture: {2}", CultureInfo.InstalledUICulture.Name, CultureInfo.CurrentUICulture, CultureInfo.CurrentCulture));

                parameters = new EarthRotationParameters(TL); // Get configuration from the Profile store
                string runDate = now.ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT, CultureInfo.CurrentUICulture); // Get today's date and time in a locally readable format
                hostURIString = parameters.DownloadTaskDataSource; // Initialise the data source URI

                if (args.Length > 0)
                {
                    foreach (string arg in args)
                    {
                        LogMessage("EarthRotationUpdate", string.Format("Received parameter: {0}", arg));
                    }
                }

                // If we have been provided with an "Initialise" parameter then stop here after having set up all the default profile values by creating the EarthRotationParameters object
                if (args.Length == 1)
                {
                    if (args[0].Trim(' ', '-', '\\', '/').Equals("INITIALISE", StringComparison.OrdinalIgnoreCase)) // Test for the presence of and act on the initialise argument ignoring everything else
                    {
                        LogMessage("EarthRotationUpdate", $"Earth rotation parameter initialisation run on {runDate} by {runBy}, IsSystem: {isSystem}");
                        LogEvent(string.Format("Earth rotation parameter initialisation run on {0} by {1}, IsSystem: {2}", runDate, runBy, isSystem), EventLogEntryType.Information);

                        LogMessage("EarthRotationUpdate", "Calling ManageScheduledTask");
                        parameters.ManageScheduledTask();
                        LogMessage("EarthRotationUpdate", "Completed ManageScheduledTask");

                        LogMessage("EarthRotationUpdate", "Initialise complete.");

                        Environment.Exit(0);
                    }
                    else if (args[0].Trim(' ', '-', '\\', '/').Equals("REMOVETASK", StringComparison.OrdinalIgnoreCase)) // Test for the presence of and act on the remove task argument ignoring everything else
                    {
                        LogMessage("EarthRotationUpdate", $"Earth rotation task removal run on {runDate} by {runBy}, IsSystem: {isSystem}");
                        LogEvent(string.Format("Earth rotation task removal run on {0} by {1}, IsSystem: {2}", runDate, runBy, isSystem), EventLogEntryType.Information);

                        LogMessage("EarthRotationUpdate", "Calling RemoveScheduledTask");
                        parameters.RemoveScheduledTask();
                        LogMessage("EarthRotationUpdate", "Completed RemoveScheduledTask");

                        LogMessage("EarthRotationUpdate", "Removal complete.");

                        Environment.Exit(0);
                    }
                }

                // If we have been provided with a "DataSource" override parameter then apply the new URI otherwise read it from the Profile
                if (args.Length == 2)
                {
                    if (args[0].Trim(' ', '-', '\\', '/').Equals("DATASOURCE", StringComparison.OrdinalIgnoreCase)) // Test for the presence of and act on the data source argument ignoring everything else
                    {
                        LogMessage("EarthRotationUpdate", string.Format("Data source override parameter provided: {0}", args[1]));
                        string overrideDataSource = args[1].Trim(' ', '"');
                        bool UriValid = false; // Set the valid flag false, then set to true if the download source starts with a supported URI prefix
                        if (overrideDataSource.StartsWith(GlobalItems.URI_PREFIX_HTTP, StringComparison.OrdinalIgnoreCase)) UriValid = true;
                        if (overrideDataSource.StartsWith(GlobalItems.URI_PREFIX_HTTPS, StringComparison.OrdinalIgnoreCase)) UriValid = true;
                        if (overrideDataSource.StartsWith(GlobalItems.URI_PREFIX_FTP, StringComparison.OrdinalIgnoreCase)) UriValid = true;

                        if (UriValid)
                        {
                            hostURIString = overrideDataSource;
                            LogMessage("EarthRotationUpdate", string.Format("Data source override parameter is valid and will be used: {0}", hostURIString));
                        }
                        else
                        {
                            LogMessage("EarthRotationUpdate", string.Format("Data source override parameter {0} is not valid and the Profile data source will be used instead: {1}", overrideDataSource, hostURIString));
                        }
                    }
                }

                // Ensure that the host URI string, wherever it has come from, ends with a single backslash otherwise the URI will be incorrect when the file name is formed
                hostURIString = hostURIString.TrimEnd(' ', '-', '\\', '/') + "/";

                LogEvent(string.Format("Run on {0} by {1}, IsSystem: {2}", runDate, runBy, isSystem), EventLogEntryType.Information);
                LogMessage("EarthRotationUpdate", string.Format("Run on {0} by {1}, IsSystem: {2}", runDate, runBy, isSystem));
                BlankLine();

                if (TL is null) // Do not try to display TraceLogger configuration
                {
                    LogMessage("EarthRotationUpdate", $"Log file could not be created, Trace state: {parameters.DownloadTaskTraceEnabled}");
                }
                else // Include TraceLogger configuration
                {
                    LogMessage("EarthRotationUpdate", $"Log file: {TL.LogFileName}, Trace state: {parameters.DownloadTaskTraceEnabled}, Log file path: {TL.LogFilePath}");
                }
                LogMessage("EarthRotationUpdate", string.Format("Earth rotation data last updated: {0}", parameters.EarthRotationDataLastUpdatedString));
                LogMessage("EarthRotationUpdate", string.Format("Data source: {0}", hostURIString));

                DownloadTimeout = parameters.DownloadTaskTimeOut;

                WebClient client = new WebClient();

                client.DownloadProgressChanged += Client_DownloadProgressChanged;
                client.DownloadFileCompleted += Client_DownloadFileCompleted;

                Uri hostURI = new Uri(hostURIString);

                if (WebRequest.DefaultWebProxy.GetProxy(hostURI) == hostURI)
                {
                    LogMessage("EarthRotationUpdate", "No proxy server detected, going directly to Internet"); // No proxy is in use so go straight out
                }
                else // Proxy is in use so set it and apply credentials
                {
                    LogMessage("EarthRotationUpdate", "Setting default proxy");
                    client.Proxy = WebRequest.DefaultWebProxy;
                    LogMessage("EarthRotationUpdate", "Setting default credentials");
                    client.Proxy.Credentials = CredentialCache.DefaultCredentials;
                    LogMessage("EarthRotationUpdate", "Using proxy server: " + WebRequest.DefaultWebProxy.GetProxy(hostURI).ToString());
                }

                client.Headers.Add("user-agent", GlobalItems.DOWNLOAD_TASK_USER_AGENT);
                client.Headers.Add("Accept", "text/plain");
                client.Encoding = Encoding.ASCII;
                client.BaseAddress = hostURIString;
                NetworkCredential credentials = new NetworkCredential("anonymous", "guest"); // Apply some standard credentials for FTP sites
                client.Credentials = credentials;
                BlankLine();

                // Get the latest delta UT1 values
                try
                {
                    string dUT1fileName = DownloadFile("DeltaUT1", GlobalItems.DELTAUT1_FILE, client); // Download the latest delta UT1 values and receive the filename holding the data
                    FileInfo info = new FileInfo(dUT1fileName); // Find out if we have any data
                    if (info.Length > 0) // We actually received some data so process it
                    {
                        // List the data position parameters that will be used to extract the delta UT1 data from the file
                        LogMessage("DeltaUT1", string.Format("Expected file format for the {0} file", GlobalItems.DELTAUT1_FILE));
                        LogMessage("DeltaUT1", string.Format("Year string start position: {0}, Year string length: {1}", GlobalItems.DELTAUT1_YEAR_START, GlobalItems.DELTAUT1_YEAR_LENGTH));
                        LogMessage("DeltaUT1", string.Format("Month string start position: {0}, Month string length: {1}", GlobalItems.DELTAUT1_MONTH_START, GlobalItems.DELTAUT1_MONTH_LENGTH));
                        LogMessage("DeltaUT1", string.Format("Day string start position: {0}, Day string length: {1}", GlobalItems.DELTAUT1_DAY_START, GlobalItems.DELTAUT1_DAY_LENGTH));
                        LogMessage("DeltaUT1", string.Format("Julian date start position: {0}, Julian date string length: {1}", GlobalItems.DELTAUT1_JULIAN_DATE_START, GlobalItems.DELTAUT1_JULIAN_DATE_LENGTH));
                        LogMessage("DeltaUT1", string.Format("Delta UT1 start position: {0}, Delta UT1 string length: {1}", GlobalItems.DELTAUT1_START, GlobalItems.DELTAUT1_LENGTH));
                        BlankLine();

                        profile.DeleteKey(GlobalItems.AUTOMATIC_UPDATE_DELTAUT1_SUBKEY_NAME); // Clear out old delta UT1 values
                        profile.CreateKey(GlobalItems.AUTOMATIC_UPDATE_DELTAUT1_SUBKEY_NAME);

                        // Process the data file
                        using (var filestream = new FileStream(dUT1fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (var file = new StreamReader(filestream, Encoding.ASCII, true, 4096))
                            {
                                string lineOfText;
                                DateTime date;

                                while ((lineOfText = file.ReadLine()) != null) // Get lines of text one at a time and parse them 
                                {
                                    try
                                    {
                                        // Extract string values for data items
                                        string yearString = lineOfText.Substring(GlobalItems.DELTAUT1_YEAR_START, GlobalItems.DELTAUT1_YEAR_LENGTH);
                                        string monthString = lineOfText.Substring(GlobalItems.DELTAUT1_MONTH_START, GlobalItems.DELTAUT1_MONTH_LENGTH);
                                        string dayString = lineOfText.Substring(GlobalItems.DELTAUT1_DAY_START, GlobalItems.DELTAUT1_DAY_LENGTH);
                                        string julianDateString = lineOfText.Substring(GlobalItems.DELTAUT1_JULIAN_DATE_START, GlobalItems.DELTAUT1_JULIAN_DATE_LENGTH);
                                        string dUT1String = lineOfText.Substring(GlobalItems.DELTAUT1_START, GlobalItems.DELTAUT1_LENGTH);

                                        // Validate that the data items are parse-able
                                        bool yearOK = int.TryParse(yearString, NumberStyles.Integer, CultureInfo.InvariantCulture, out int year);
                                        bool monthOK = int.TryParse(monthString, NumberStyles.Integer, CultureInfo.InvariantCulture, out int month);
                                        bool dayOK = int.TryParse(dayString, NumberStyles.Integer, CultureInfo.InvariantCulture, out int day);
                                        bool julianDateOK = double.TryParse(julianDateString, NumberStyles.Float, CultureInfo.InvariantCulture, out double julianDate);
                                        bool dut1OK = double.TryParse(dUT1String, NumberStyles.Float, CultureInfo.InvariantCulture, out double dUT1);

                                        if (yearOK & monthOK & dayOK & julianDateOK & dut1OK) // We have good values for all data items so save these to the Profile
                                        {
                                            year = invariantCulture.Calendar.ToFourDigitYear(year); // Convert the two digit year to a four digit year
                                            date = new DateTime(year, month, day);

                                            // Only save the item if it is from a few days back or is a future prediction
                                            if (date.Date >= DateTime.Now.Date.Subtract(new TimeSpan(GlobalItems.DOWNLOAD_TASK_NUMBER_OF_BACK_DAYS_OF_DELTAUT1_DATA_TO_LOAD, 0, 0, 0)))
                                            {
                                                string deltaUT1ValueName = string.Format(GlobalItems.DELTAUT1_VALUE_NAME_FORMAT,
                                                                                         date.Year.ToString(GlobalItems.DELTAUT1_VALUE_NAME_YEAR_FORMAT),
                                                                                         date.Month.ToString(GlobalItems.DELTAUT1_VALUE_NAME_MONTH_FORMAT),
                                                                                         date.Day.ToString(GlobalItems.DELTAUT1_VALUE_NAME_DAY_FORMAT));
                                                LogMessage("DeltaUT1", string.Format("Setting {0}, JD = {1} - DUT1 = {2} with key: {3}", date.ToLongDateString(), julianDate, dUT1, deltaUT1ValueName));
                                                profile.WriteProfile(GlobalItems.AUTOMATIC_UPDATE_DELTAUT1_SUBKEY_NAME, deltaUT1ValueName, dUT1.ToString("0.000", CultureInfo.InvariantCulture));
                                            }
                                        }
                                        else
                                        {
                                            LogMessage("DeltaUT1", string.Format("Unable to parse Delta UT1 values from the line below - Year: {0}, Month: {1}, Day: {2}, Julian Day: {3},Delta UT1: {4}",
                                                          yearString, monthString, dayString, julianDateString, dUT1String));
                                            LogMessage("DeltaUT1", string.Format("Corrupt line: {0}", lineOfText));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LogMessage("DeltaUT1", string.Format("Unexpected exception: {0}, parsing line: ", ex.Message, lineOfText));
                                        LogMessage("DeltaUT1", ex.ToString());
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        LogMessage("DeltaUT1", string.Format("Downloaded file size was zero so nothing to process!"));
                    }
                    File.Delete(dUT1fileName);
                    BlankLine();
                }
                catch (WebException ex) // An issue occurred with receiving the leap second file over the network 
                {
                    LogMessage("DeltaUT1", string.Format("Error: {0} - delta UT1 data not updated", ex.Message));
                    ReturnCode = 1;
                }
                catch (Exception ex)
                {
                    LogMessage("DeltaUT1", ex.ToString());
                    ReturnCode = 2;
                }

                // Get the latest leap second values
                try
                {
                    string leapSecondsfileName = DownloadFile("LeapSeconds", GlobalItems.LEAP_SECONDS_FILE, client); // Download the latest leap second values and receive the filename holding the data
                    FileInfo info = new FileInfo(leapSecondsfileName); // Find out if we have any data
                    if (info.Length > 0) // We actually received some data so process it
                    {
                        // List the data position parameters that will be used to extract the delta UT1 data from the file
                        LogMessage("LeapSeconds", string.Format("Expected file format for the {0} file", GlobalItems.DELTAUT1_FILE));
                        LogMessage("LeapSeconds", string.Format("Year string start position: {0}, Year string length: {1}", GlobalItems.LEAP_SECONDS_YEAR_START, GlobalItems.LEAP_SECONDS_YEAR_LENGTH));
                        LogMessage("LeapSeconds", string.Format("Month string start position: {0}, Month string length: {1}", GlobalItems.LEAP_SECONDS_MONTH_START, GlobalItems.LEAP_SECONDS_MONTH_LENGTH));
                        LogMessage("LeapSeconds", string.Format("Day string start position: {0}, Day string length: {1}", GlobalItems.LEAP_SECONDS_DAY_START, GlobalItems.LEAP_SECONDS_DAY_LENGTH));
                        LogMessage("LeapSeconds", string.Format("Julian date start position: {0}, Julian date string length: {1}", GlobalItems.LEAP_SECONDS_JULIAN_DATE_START, GlobalItems.LEAP_SECONDS_JULIAN_DATE_LENGTH));
                        LogMessage("LeapSeconds", string.Format("Leap seconds start position: {0}, Leap seconds string length: {1}", GlobalItems.LEAP_SECONDS_LEAPSECONDS_START, GlobalItems.LEAP_SECONDS_LEAPSECONDS_LENGTH));
                        BlankLine();

                        profile.DeleteKey(GlobalItems.AUTOMATIC_UPDATE_LEAP_SECOND_HISTORY_SUBKEY_NAME); ; // Clear out old leap second values
                        profile.CreateKey(GlobalItems.AUTOMATIC_UPDATE_LEAP_SECOND_HISTORY_SUBKEY_NAME);

                        // Process the data file
                        using (var filestream = new FileStream(leapSecondsfileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            double currentLeapSeconds = UNKNOWN_LEAP_SECONDS;
                            double nextLeapSeconds = 0.0;
                            DateTime leapSecondDate = UNKNOWN_DATE;
                            DateTime nextleapSecondsDate = UNKNOWN_DATE;

                            using (var file = new StreamReader(filestream, Encoding.ASCII, true, 4096))
                            {
                                string lineOfText;
                                DateTime latestLeapSecondDate = UNKNOWN_DATE;
                                while ((lineOfText = file.ReadLine()) != null) // Get lines of text one at a time and parse them 
                                {
                                    try
                                    {
                                        // Read values from the file as strings based on their position within the file
                                        string yearString = lineOfText.Substring(GlobalItems.LEAP_SECONDS_YEAR_START, GlobalItems.LEAP_SECONDS_YEAR_LENGTH);
                                        string monthString = lineOfText.Substring(GlobalItems.LEAP_SECONDS_MONTH_START, GlobalItems.LEAP_SECONDS_MONTH_LENGTH);
                                        string dayString = lineOfText.Substring(GlobalItems.LEAP_SECONDS_DAY_START, GlobalItems.LEAP_SECONDS_DAY_LENGTH);
                                        string julianDateString = lineOfText.Substring(GlobalItems.LEAP_SECONDS_JULIAN_DATE_START, GlobalItems.LEAP_SECONDS_JULIAN_DATE_LENGTH);
                                        string leapSecondsString = lineOfText.Substring(GlobalItems.LEAP_SECONDS_LEAPSECONDS_START, GlobalItems.LEAP_SECONDS_LEAPSECONDS_LENGTH);

                                        // Validate that the data items are parse-able
                                        bool yearOK = int.TryParse(yearString, NumberStyles.Integer, CultureInfo.InvariantCulture, out int year);
                                        bool dayOK = int.TryParse(dayString, NumberStyles.Integer, CultureInfo.InvariantCulture, out int day);
                                        bool julianDateOK = double.TryParse(julianDateString, NumberStyles.Float, CultureInfo.InvariantCulture, out double julianDate);
                                        bool leapSecondsOK = double.TryParse(leapSecondsString, NumberStyles.Float, CultureInfo.InvariantCulture, out double leapSeconds);

                                        // Get the month number by trimming the month string, converting to lower case then title case then looking up the index in the abbreviated months array
                                        int month = Array.IndexOf(monthAbbrev, invariantTextInfo.ToTitleCase(monthString.Trim(' ').ToLower(CultureInfo.InvariantCulture))) + 1; // If IndexOf fails, it returns -1 so the resultant month number will be zero and this is checked below

                                        if (yearOK & (month > 0) & dayOK & julianDateOK & leapSecondsOK) // We have good values for all data items so save these to the Profile
                                        {
                                            double modifiedJulianDate = julianDate - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET;
                                            leapSecondDate = new DateTime(year, month, day);

                                            // Write all leap second values and Julian dates that they become effective to the leap second history subkey
                                            profile.WriteProfile(GlobalItems.AUTOMATIC_UPDATE_LEAP_SECOND_HISTORY_SUBKEY_NAME, julianDate.ToString(CultureInfo.InvariantCulture), leapSeconds.ToString(CultureInfo.InvariantCulture));

                                            if ((leapSecondDate.Date >= latestLeapSecondDate) & (leapSecondDate.Date <= DateTime.UtcNow.Date.Subtract(new TimeSpan(GlobalItems.TEST_HISTORIC_DAYS_OFFSET, 0, 0, 0)))) currentLeapSeconds = leapSeconds;
                                            if ((leapSecondDate.Date > DateTime.UtcNow.Date.Subtract(new TimeSpan(GlobalItems.TEST_HISTORIC_DAYS_OFFSET, 0, 0, 0))) & (nextleapSecondsDate == UNKNOWN_DATE)) // Record the next leap seconds value in the file
                                            {
                                                nextLeapSeconds = leapSeconds;
                                                nextleapSecondsDate = leapSecondDate;
                                            }

                                            LogMessage("LeapSeconds", string.Format("Leap second takes effect on: {0}, Modified JD = {1} - Current Leap Seconds = {2}, Latest Leap Seconds: {3}, Next Leap Seconds: {4} on {5}", leapSecondDate.ToLongDateString(), modifiedJulianDate, leapSeconds, currentLeapSeconds, nextLeapSeconds, nextleapSecondsDate.ToLongDateString()));
                                        }
                                        else
                                        {
                                            LogMessage("LeapSeconds", string.Format("Unable to parse leap second values from the line below - Year: {0}, Month: {1}, Day: {2}, Julian Day: {3},Leap seconds: {4}",
                                                          yearString, monthString, dayString, julianDateString, leapSecondsString));
                                            LogMessage("LeapSeconds", string.Format("Corrupt line: {0}", lineOfText));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LogMessage("LeapSeconds", ex.ToString());
                                    }
                                }
                            }
                            BlankLine();

                            if (currentLeapSeconds == UNKNOWN_LEAP_SECONDS) // No valid leap seconds were found so indicate that this
                            {
                                profile.WriteProfile(GlobalItems.ASTROMETRY_SUBKEY, GlobalItems.AUTOMATIC_LEAP_SECONDS_VALUENAME, GlobalItems.AUTOMATIC_LEAP_SECONDS_NOT_AVAILABLE_DEFAULT);
                            }
                            else  // Persist the current leap second value to the Profile
                            {
                                parameters.AutomaticLeapSecondsString = currentLeapSeconds.ToString(CultureInfo.InvariantCulture);
                                // Also include a value that is in the SOFA library defaults but is not in the USNO files. It pre-dates the start of UTC but I am assuming that IAU is correct on this occasion
                                profile.WriteProfile(GlobalItems.AUTOMATIC_UPDATE_LEAP_SECOND_HISTORY_SUBKEY_NAME, double.Parse("2436934.5", CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture), double.Parse("1.4178180", CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture));

                            }

                            // Persist the next leap second value and its implementation date if these have been announced
                            if (nextleapSecondsDate == UNKNOWN_DATE) // No announcement has been made
                            {
                                parameters.NextLeapSecondsString = GlobalItems.DOWNLOAD_TASK_NEXT_LEAP_SECONDS_NOT_PUBLISHED_MESSAGE;
                                parameters.NextLeapSecondsDateString = GlobalItems.DOWNLOAD_TASK_NEXT_LEAP_SECONDS_NOT_PUBLISHED_MESSAGE;
                            }
                            else // A future leap second has been announced
                            {
                                parameters.NextLeapSecondsString = nextLeapSeconds.ToString(CultureInfo.InvariantCulture);
                                parameters.NextLeapSecondsDateString = nextleapSecondsDate.ToString(GlobalItems.DOWNLOAD_TASK_TIME_FORMAT, CultureInfo.InvariantCulture);
                            }
                            BlankLine();
                            LogMessage("LeapSeconds", string.Format("Current Leap Seconds = {0}, Next Leap Seconds: {1} on {2}", currentLeapSeconds, nextLeapSeconds, nextleapSecondsDate.ToLongDateString()));
                        }
                    }
                    else
                    {
                        LogMessage("LeapSeconds", string.Format("Downloaded file size was zero so nothing to process!"));
                    }

                    parameters.EarthRotationDataLastUpdatedString = runDate; // Save a new last run time to the Profile

                    BlankLine();
                    LogMessage("LeapSeconds", string.Format("Task completed."));

                    File.Delete(leapSecondsfileName);
                    parameters.Dispose();
                    parameters = null;

                }
                catch (WebException ex) // An issue occurred with receiving the leap second file over the network 
                {
                    LogMessage("LeapSeconds", string.Format("Error: {0} - leap second data not updated.", ex.Message));
                    ReturnCode = 3;
                }
                catch (Exception ex)
                {
                    LogMessage("LeapSeconds", ex.ToString());
                    ReturnCode = 4;
                }

                if (!(TL is null))
                {
                    TL.Enabled = false;
                    TL.Dispose();
                    TL = null;
                }
            }
            catch (Exception ex)
            {
                try { LogMessage("EarthRotationUpdate", ex.ToString()); } catch { }

                Global.LogEvent("EarthRotationUpdate",
                                      string.Format("EarthRotationUpdate - Unexpected exception: {0}", ex.Message),
                                      EventLogEntryType.Error,
                                      EventLogErrors.EarthRotationUpdate,
                                      ex.ToString());
                ReturnCode = 5;
            }
            Environment.Exit(ReturnCode);
        }

        #region Event handlers

        private static void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try // Ignore any errors here
            {
                if (e.Cancelled)
                {
                    LogMessage("Download Status", string.Format("Download timed out"));
                }
                else if (e.Error != null)
                {
                    downloadError = e.Error;
                    LogMessage("Download Error", string.Format("Error: {0}", downloadError.Message));
                }
                else
                {
                    LogMessage("Download Status", string.Format("Download Completed OK,"));
                }

                DownloadComplete = true;
            }
            catch { }
        }

        private static void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try // Ignore any errors here
            {
                LogMessage("Download Progress", string.Format("Progress %: {0}, {1} / {2} bytes, Complete: {3}", e.ProgressPercentage.ToString(), e.BytesReceived, e.TotalBytesToReceive, DownloadComplete));
            }
            catch { }
        }

        #endregion

        #region Support code

        private static void LogEvent(string message, EventLogEntryType severity)
        {
            Global.LogEvent("EarthRotationUpdate", message, severity, EventLogErrors.EarthRotationUpdate, Except: "");
        }

        private static string DownloadFile(string Function, string DataFile, WebClient Client)
        {
            string tempFileName;
            Stopwatch timeOutTimer;

            tempFileName = Path.GetTempFileName();
            timeOutTimer = new Stopwatch();

            LogMessage(Function, string.Format("About to download {0} from {1} as {2}", DataFile, Client.BaseAddress, tempFileName));

            Client.DownloadFileAsync(new Uri(DataFile, UriKind.Relative), tempFileName);

            int printCount = 0;
            //DateTime timeOut = DateTime.Now.AddSeconds(DownloadTimeout);
            DownloadComplete = false;
            downloadError = null;
            timeOutTimer.Start();
            do
            {
                if (printCount == 9)
                {
                    LogMessage(Function, string.Format("Waiting for download to complete...{0} / {1} seconds", timeOutTimer.Elapsed.TotalSeconds.ToString("0"), parameters.DownloadTaskTimeOut));
                    printCount = 0;
                }
                else printCount += 1;
                Thread.Sleep(100);
            } while (!DownloadComplete & (timeOutTimer.Elapsed.TotalSeconds < parameters.DownloadTaskTimeOut));

            if (DownloadComplete)
            {
                if (downloadError == null) // No error occurred
                {
                    LogMessage(Function, "Response headers");
                    WebHeaderCollection responseHeaders = Client.ResponseHeaders;
                    if (!(responseHeaders is null))
                    {
                        foreach (string header in responseHeaders.AllKeys)
                        {
                            LogMessage(Function, string.Format("Response header {0} = {1}", header, responseHeaders[header]));
                        }
                    }
                    FileInfo info = new FileInfo(tempFileName);
                    LogMessage(Function, string.Format("Successfully downloaded {0} from {1} as {2}. Size: {3}", DataFile, Client.BaseAddress, tempFileName, info.Length));
                }
                else // An error occurred
                {
                    // Just throw the error, which will be reported in the calling routine
                    throw downloadError;
                }
            }
            else
            {
                LogMessage(Function, string.Format("Download cancelled because of {0} second timeout", DownloadTimeout));
                try
                {
                    Client.CancelAsync();
                }
                catch (Exception ex)
                {
                    LogMessage("DownloadFile", "Exception cancelling download: " + ex.ToString());
                }

                throw new TimeoutException(string.Format("Timed out downloading {0} from {1} as {2}", DataFile, Client.BaseAddress, tempFileName));
            }
            BlankLine();

            return tempFileName;
        }

        private static void LogMessage(string identifier, string message)
        {
            if (!(TL is null)) // TraceLogger is available so try to use it
            {
                try
                {
                    // Log the message to the TraceLogger
                    TL.LogMessageCrLf(identifier, message);
                }
                catch (Exception ex)
                {
                    // Something went wrong when trying to log the message to the TraceLogger so log this fact to console and event log 
                    Console.WriteLine($"EarthRotationUpdate.LogMessage - TraceLogger threw an exception when writing '{message}', abandoning trace logging: {ex.Message}");
                    Global.LogEvent("EarthRotationUpdate.LogMessage", $"TraceLogger threw an exception when writing '{message}', abandoning trace logging: {ex.Message}", EventLogEntryType.Error, EventLogErrors.EarthRotationUpdate, "");

                    // Disable and dispose of the TraceLogger, setting it to null so that it can't be used in future
                    try { TL.Enabled = false; } catch { }
                    try { TL.Dispose(); } catch { }
                    try { TL = null; } catch { }

                    // Log that the disposal was successful
                    Console.WriteLine($"EarthRotationUpdate.LogMessage - TraceLogger Disposed and set to null");
                    Global.LogEvent("EarthRotationUpdate.LogMessage", $"TraceLogger Disposed and set to null", EventLogEntryType.Information, EventLogErrors.EarthRotationUpdate, "");
                }
            }

            // Log the message to console and event log if it was not logged to the TraceLogger
            if (TL is null)
            {
                Console.WriteLine($"{identifier} - {message}");
                Global.LogEvent(identifier, message, EventLogEntryType.Information, EventLogErrors.EarthRotationUpdate, "");
            }
        }

        private static void BlankLine()
        {
            LogMessage("", "");
        }
        #endregion
    }
}
