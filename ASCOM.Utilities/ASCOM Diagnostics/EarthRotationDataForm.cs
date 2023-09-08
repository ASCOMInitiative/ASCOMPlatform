using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using ASCOM.Astrometry;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ASCOM.Utilities
{

    public partial class EarthRotationDataForm
    {
        private const int TRACE_LOGGER_IDENTIFIER_FIELD_WIDTH = 35;
        private const double UPDATE_DATA_PROCESS_TIMEOUT = 60.0d; // Timeout for the "Update now" function provided by this form
        private const int REFRESH_TIMER_INTERVAL = 1000; // Refresh interval (milliseconds) for the current deltaUT1 and leap second values displayed on the form
        private const double DELTAUT1_ACCEPTABLE_RANGE = 0.9d; // Acceptable range for manual deltaut1 values is +- this value
        private const double MINIMUM_UPDATE_RUN_TIME = 5.0d; // Minimum acceptable time (seconds)  for the time allowed for a manually triggered update task to run

        private const string PLATFORM_HELP_FILE = @"\ASCOM\Platform 7\Docs\PlatformHelp.chm";
        private const string EARTH_ROTATION_HELP_TOPIC = "/html/98976954-6a00-4864-a223-7b3b25ffaaf1.htm";
        private TraceLogger TL;
        private EarthRotationParameters Parameters;
        private System.Windows.Forms.Timer _NowTimer;

        private System.Windows.Forms.Timer NowTimer
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _NowTimer;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_NowTimer != null)
                {
                    _NowTimer.Tick -= UpdateCurrentLeapSecondsAndDeltaUT1;
                }

                _NowTimer = value;
                if (_NowTimer != null)
                {
                    _NowTimer.Tick += UpdateCurrentLeapSecondsAndDeltaUT1;
                }
            }
        }
        private Astrometry.AstroUtils.AstroUtils aUtils;

        private string EarthRotationDataUpdateType, AutomaticScheduleJobRepeatFrequency, EarthRotationDataSource, AutomaticScheduleJobLastUpdateTime, TraceFilePath, CurrentLeapSeconds, NextLeapSeconds, NextLeapSecondsDate;
        private double ManualLeapSeconds, DownloadTimeout, ManualDeltaUT1Value;
        private DateTime AutomaticScheduleJobRunTime;
        private bool TraceEnabled;
        private double LeapSecondMinimumValue;

        // Initialise drop-down list options
        private List<string> dataDownloadSources = new List<string>() { Astrometry.GlobalItems.EARTH_ROTATION_INTERNET_DATA_SOURCE_0 };
        // EARTH_ROTATION_INTERNET_DATA_SOURCE_0,
        // EARTH_ROTATION_INTERNET_DATA_SOURCE_1,
        // EARTH_ROTATION_INTERNET_DATA_SOURCE_2,
        // EARTH_ROTATION_INTERNET_DATA_SOURCE_3,
        // EARTH_ROTATION_INTERNET_DATA_SOURCE_4

        private List<string> ut1Sources = new List<string>() { Astrometry.GlobalItems.UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1, Astrometry.GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1, Astrometry.GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1, Astrometry.GlobalItems.UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1, Astrometry.GlobalItems.UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1 };

        private List<string> scheduleRepeatOptions = new List<string>() { Astrometry.GlobalItems.SCHEDULE_REPEAT_NONE, Astrometry.GlobalItems.SCHEDULE_REPEAT_DAILY, Astrometry.GlobalItems.SCHEDULE_REPEAT_WEEKLY, Astrometry.GlobalItems.SCHEDULE_REPEAT_MONTHLY };

        public EarthRotationDataForm()
        {
            InitializeComponent();
        }

        #region New and form load

        private void EarthRotationDataForm_Load(object sender, EventArgs e)
        {
            DateTime AutomaticScheduleTimeDefault; // 

            try
            {
                TL = new TraceLogger("", "EarthRotation");
                TL.Enabled = Utilities.Global.GetBool(Utilities.Global.TRACE_EARTHROTATION_DATA_FORM, Utilities.Global.TRACE_EARTHROTATION_DATA_FORM_DEFAULT);
                TL.IdentifierWidth = TRACE_LOGGER_IDENTIFIER_FIELD_WIDTH;

                TL.LogMessage("Form Load", "Start of form load");
                TL.LogMessage("Form Load", string.Format("Log file name: {0}", TL.LogFileName));

                Parameters = new EarthRotationParameters(TL);
                TL.LogMessage("Form Load", "Calling ManageScheduledTask");
                Parameters.ManageScheduledTask();
                TL.LogMessage("Form Load", "Finished ManageScheduledTask");
                aUtils = new Astrometry.AstroUtils.AstroUtils(TL);

                // Start a timer to periodically update the current DeltaUT1 and leap second values on the form
                NowTimer = new System.Windows.Forms.Timer();
                NowTimer.Interval = REFRESH_TIMER_INTERVAL;
                NowTimer.Start();

                // Specify that these combo boxes will be painted by code in this form so that the backgrounds will be white rather than grey 
                CmbUpdateType.DrawMode = DrawMode.OwnerDrawFixed;
                CmbScheduleRepeat.DrawMode = DrawMode.OwnerDrawFixed;

                // Create a default schedule time for use in case a time hasn't been set yet. Either noon today (local time) if we are before noon or noon tomorrow if we are after noon today.
                if (DateTime.Now.Hour < 12)
                {
                    AutomaticScheduleTimeDefault = DateTime.Today.AddHours(12d);
                }
                else
                {
                    AutomaticScheduleTimeDefault = DateTime.Today.AddHours(36d);
                }

                // Get a value to use as the lowest valid value for leap seconds during validation
                LeapSecondMinimumValue = Parameters.CurrentBuiltInLeapSeconds;

                // Populate the combo box lists
                CmbDataSource.Items.Clear();
                CmbDataSource.Items.AddRange(dataDownloadSources.ToArray());

                CmbUpdateType.Items.Clear();
                CmbUpdateType.Items.AddRange(ut1Sources.ToArray());

                CmbScheduleRepeat.Items.Clear();
                CmbScheduleRepeat.Items.AddRange(scheduleRepeatOptions.ToArray());

                TL.LogMessage("Form Load", string.Format("Current UI culture: {0}, Current culture: {1}", CultureInfo.CurrentUICulture.Name, CultureInfo.CurrentCulture.Name));

                // Get the current state from the Profile
                EarthRotationDataUpdateType = Parameters.UpdateType;
                ManualLeapSeconds = Parameters.ManualLeapSeconds;
                ManualDeltaUT1Value = Parameters.ManualDeltaUT1;
                DownloadTimeout = Parameters.DownloadTaskTimeOut;
                EarthRotationDataSource = Parameters.DownloadTaskDataSource;
                AutomaticScheduleJobRunTime = Parameters.DownloadTaskScheduledTime;
                AutomaticScheduleJobRepeatFrequency = Parameters.DownloadTaskRepeatFrequency;
                AutomaticScheduleJobLastUpdateTime = Parameters.EarthRotationDataLastUpdatedString;
                TraceFilePath = Parameters.DownloadTaskTracePath;
                TraceEnabled = Parameters.DownloadTaskTraceEnabled;
                CurrentLeapSeconds = Parameters.AutomaticLeapSecondsString;
                NextLeapSeconds = Parameters.NextLeapSecondsString;
                NextLeapSecondsDate = Parameters.NextLeapSecondsDateString;

                TL.LogMessage("Form Load", "Current Earth rotation data update type: " + EarthRotationDataUpdateType);
                TL.LogMessage("Form Load", "Current manual leap seconds: " + ManualLeapSeconds);
                TL.LogMessage("Form Load", "Current manual delta UT1 value: " + ManualDeltaUT1Value);
                TL.LogMessage("Form Load", "Current download timeout: " + DownloadTimeout);
                TL.LogMessage("Form Load", "Current data download source: " + EarthRotationDataSource);
                TL.LogMessage("Form Load", "Current schedule job run time: " + AutomaticScheduleJobRunTime.ToString(Astrometry.GlobalItems.DOWNLOAD_TASK_TIME_FORMAT));
                TL.LogMessage("Form Load", "Current schedule job repeat frequency: " + AutomaticScheduleJobRepeatFrequency);
                TL.LogMessage("Form Load", "Current schedule job last updated: " + AutomaticScheduleJobLastUpdateTime);
                TL.LogMessage("Form Load", "Current schedule job trace path: " + TraceFilePath);
                TL.LogMessage("Form Load", "Current schedule job trace enabled: " + TraceEnabled.ToString());
                TL.LogMessage("Form Load", "Current leap seconds: " + CurrentLeapSeconds);
                TL.LogMessage("Form Load", "Current next leap seconds: " + NextLeapSeconds);
                TL.LogMessage("Form Load", string.Format("Current next leap seconds date string: {0}", NextLeapSecondsDate));

                foreach (string dataSource in dataDownloadSources)
                    TL.LogMessage("Form Load", "Available data source: " + dataSource);

                // Initialise display controls
                TL.LogMessage("Form Load", string.Format("About to set CmbUpdatetype to: {0}", EarthRotationDataUpdateType));
                CmbUpdateType.SelectedItem = EarthRotationDataUpdateType;
                TL.LogMessage("Form Load", string.Format("Completed setting CmbUpdatetype to: {0}", EarthRotationDataUpdateType));

                TxtManualDeltaUT1.Text = ManualDeltaUT1Value.ToString("0.000", CultureInfo.CurrentUICulture);
                TxtManualLeapSeconds.Text = ManualLeapSeconds.ToString("0.0", CultureInfo.CurrentUICulture);
                if (!dataDownloadSources.Contains(EarthRotationDataSource))
                {
                    TL.LogMessage("Form Load", string.Format("Specified data source is not one of the built-in sources so add adding it to the list: {0}", EarthRotationDataSource));
                    CmbDataSource.Items.Add(EarthRotationDataSource);
                }
                else
                {
                    TL.LogMessage("Form Load", string.Format("Specified data source is one of the built-in sources: {0}", EarthRotationDataSource));
                }
                CmbDataSource.SelectedItem = EarthRotationDataSource;
                TxtDownloadTimeout.Text = DownloadTimeout.ToString("0.0", CultureInfo.CurrentUICulture);
                DateScheduleRun.Value = AutomaticScheduleJobRunTime;
                CmbScheduleRepeat.SelectedItem = AutomaticScheduleJobRepeatFrequency;
                TxtTraceFilePath.Text = TraceFilePath;
                ChkTraceEnabled.Checked = TraceEnabled;

                UpdateStatus();
                EnableControlsAsRequired();
                UpdateCurrentLeapSecondsAndDeltaUT1(new object(), new EventArgs()); // Update the current leap second and deltaUT1 displays so they have current values when the form appears
            }

            catch (Exception ex)
            {
                TL.LogMessageCrLf("Form Load", ex.ToString());
                MessageBox.Show(@"Something went wrong when loading the configuration form, please report this on the ASCOM Talk Groups.IO forum, including the ASCOM.EarthRotation.xx.yy.txt log file from your Documents\ASCOM\Logs yyyy-mm-dd folder." + "\r\n" + ex.ToString());
            }
        }

        #endregion

        private void EnableControlsAsRequired()
        {
            switch (EarthRotationDataUpdateType ?? "")
            {
                case Astrometry.GlobalItems.UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1:
                    {
                        TxtManualLeapSeconds.Enabled = false;
                        TxtManualDeltaUT1.Enabled = false;
                        TxtDownloadTimeout.Enabled = false;
                        CmbDataSource.Enabled = false;
                        DateScheduleRun.Enabled = false;
                        CmbScheduleRepeat.Enabled = false;
                        TxtTraceFilePath.Enabled = false;
                        ChkTraceEnabled.Enabled = false;
                        BtnSetTraceDirectory.Enabled = false;
                        TxtRunStatus.Enabled = false;
                        BtnRunAutomaticUpdate.Enabled = false;
                        TxtLastRun.Enabled = false;
                        LblTraceEnabled.Enabled = false;
                        LblLastRun.Enabled = false;
                        LblRunStatus.Enabled = false;
                        break;
                    }
                case Astrometry.GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1:
                    {
                        TxtManualLeapSeconds.Enabled = true;
                        TxtManualDeltaUT1.Enabled = true;
                        TxtDownloadTimeout.Enabled = false;
                        CmbDataSource.Enabled = false;
                        DateScheduleRun.Enabled = false;
                        CmbScheduleRepeat.Enabled = false;
                        TxtTraceFilePath.Enabled = false;
                        ChkTraceEnabled.Enabled = false;
                        BtnSetTraceDirectory.Enabled = false;
                        TxtRunStatus.Enabled = false;
                        BtnRunAutomaticUpdate.Enabled = false;
                        TxtLastRun.Enabled = false;
                        LblTraceEnabled.Enabled = false;
                        LblLastRun.Enabled = false;
                        LblRunStatus.Enabled = false;
                        break;
                    }
                case Astrometry.GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1:
                    {
                        TxtManualLeapSeconds.Enabled = true;
                        TxtManualDeltaUT1.Enabled = false;
                        TxtDownloadTimeout.Enabled = false;
                        CmbDataSource.Enabled = false;
                        DateScheduleRun.Enabled = false;
                        CmbScheduleRepeat.Enabled = false;
                        TxtTraceFilePath.Enabled = false;
                        ChkTraceEnabled.Enabled = false;
                        BtnSetTraceDirectory.Enabled = false;
                        TxtRunStatus.Enabled = false;
                        BtnRunAutomaticUpdate.Enabled = false;
                        TxtLastRun.Enabled = false;
                        LblTraceEnabled.Enabled = false;
                        LblLastRun.Enabled = false;
                        LblRunStatus.Enabled = false;
                        break;
                    }
                case Astrometry.GlobalItems.UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1:
                    {
                        TxtManualLeapSeconds.Enabled = false;
                        TxtManualDeltaUT1.Enabled = false;
                        TxtDownloadTimeout.Enabled = true;
                        CmbDataSource.Enabled = true;
                        DateScheduleRun.Enabled = false;
                        CmbScheduleRepeat.Enabled = false;
                        TxtTraceFilePath.Enabled = true;
                        ChkTraceEnabled.Enabled = true;
                        BtnSetTraceDirectory.Enabled = true;
                        TxtRunStatus.Enabled = true;
                        BtnRunAutomaticUpdate.Enabled = true;
                        TxtLastRun.Enabled = true;
                        LblTraceEnabled.Enabled = true;
                        LblLastRun.Enabled = true;
                        LblRunStatus.Enabled = true;
                        break;
                    }

                case Astrometry.GlobalItems.UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1:
                    {
                        TxtManualLeapSeconds.Enabled = false;
                        TxtManualDeltaUT1.Enabled = false;
                        TxtDownloadTimeout.Enabled = true;
                        CmbDataSource.Enabled = true;
                        DateScheduleRun.Enabled = true;
                        CmbScheduleRepeat.Enabled = true;
                        TxtTraceFilePath.Enabled = true;
                        ChkTraceEnabled.Enabled = true;
                        BtnSetTraceDirectory.Enabled = true;
                        TxtRunStatus.Enabled = true;
                        BtnRunAutomaticUpdate.Enabled = true;
                        TxtLastRun.Enabled = true;
                        LblTraceEnabled.Enabled = true;
                        LblLastRun.Enabled = true;
                        LblRunStatus.Enabled = true;
                        break;
                    }

                default:
                    {
                        Interaction.MsgBox("Unknown EarthRotationDataUpdateType: " + EarthRotationDataUpdateType);
                        break;
                    }
            }
            GrpOnDemandAndAutomaticUpdateConfiguration.Refresh();
            GrpManualUpdate.Refresh();
            GrpUpdateType.Refresh();
            GrpStatus.Refresh();
            GrpScheduleTime.Refresh();
        }

        private void UpdateStatus()
        {
            DateTime DisplayDate;
            double jdUtc;

            aUtils.Refresh(); // Ensure that our astro utils object is using the latest data

            // Calculate the display date, allowing for development test offsets if present. In production offsets will be 0 so DisplayDate will have a value of DateTime.Now as a UTC
            DisplayDate = DateTime.UtcNow.Subtract(new TimeSpan(Astrometry.GlobalItems.TEST_UTC_DAYS_OFFSET, Astrometry.GlobalItems.TEST_UTC_HOURS_OFFSET, Astrometry.GlobalItems.TEST_UTC_MINUTES_OFFSET, 0));
            TxtNow.Text = string.Format("{0} {1}", DisplayDate.ToString(Astrometry.GlobalItems.DOWNLOAD_TASK_TIME_FORMAT, CultureInfo.CurrentUICulture), DisplayDate.Kind.ToString().ToUpperInvariant());
            jdUtc = DateTime.UtcNow.ToOADate() + Astrometry.GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET;
            TxtEffectiveDeltaUT1.Text = aUtils.DeltaUT(jdUtc).ToString("+0.000;-0.000;0.000", CultureInfo.CurrentUICulture);
            TxtEffectiveLeapSeconds.Text = aUtils.LeapSeconds.ToString("0.0", CultureInfo.CurrentUICulture);

            Parameters.RefreshState(); // Make sure we have the latest values, in case any have been updated
            TxtLastRun.Text = Parameters.EarthRotationDataLastUpdatedString;
            TxtNextLeapSeconds.Text = Parameters.NextLeapSecondsString;
            TxtNextLeapSecondsDate.Text = Conversions.ToString(Operators.ConcatenateObject(Parameters.NextLeapSecondsDateString, Interaction.IIf((Parameters.NextLeapSecondsDateString ?? "") == Astrometry.GlobalItems.DOWNLOAD_TASK_NEXT_LEAP_SECONDS_NOT_PUBLISHED_MESSAGE | (Parameters.NextLeapSecondsDateString ?? "") == Astrometry.GlobalItems.NEXT_LEAP_SECONDS_DATE_NOT_AVAILABLE_DEFAULT, "", "UTC")));
            TxtLastRun.Text = Parameters.EarthRotationDataLastUpdatedString;

        }

        private void LogRunMessage(string message)
        {
            TL.LogMessageCrLf("RunAutomaticUpdate", message);
            TxtRunStatus.Text = message;
        }

        private void ValidateURI()
        {
            bool UriValid;

            UriValid = false; // Set the valid flag false, then set to true if the download source starts with a supported URI prefix
            if (CmbDataSource.Text.StartsWith(Astrometry.GlobalItems.URI_PREFIX_HTTP, StringComparison.OrdinalIgnoreCase))
                UriValid = true;
            if (CmbDataSource.Text.StartsWith(Astrometry.GlobalItems.URI_PREFIX_HTTPS, StringComparison.OrdinalIgnoreCase))
                UriValid = true;
            if (CmbDataSource.Text.StartsWith(Astrometry.GlobalItems.URI_PREFIX_FTP, StringComparison.OrdinalIgnoreCase))
                UriValid = true;
            if (UriValid)
            {
                ErrorProvider1.SetError(CmbDataSource, "");
                EarthRotationDataSource = CmbDataSource.Text;
                Parameters.DownloadTaskDataSource = EarthRotationDataSource;
                TL.LogMessage("EarthRotationDataSource", string.Format("Data source updated to: {0}", EarthRotationDataSource));
                BtnClose.Enabled = true;
            }
            else
            {
                BtnClose.Enabled = false;
                ErrorProvider1.SetError(CmbDataSource, "Must start with http:// or https:// or ftp://");
            }
        }

        #region Timer event handler

        private void UpdateCurrentLeapSecondsAndDeltaUT1(object myObject, EventArgs myEventArgs)
        {
            UpdateStatus();
        }

        #endregion

        #region Button and event handlers

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnRunAutomaticUpdate_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi;
            Process proc;
            bool CancelButtonState = default, OKButtonState = default, UpdateCompleted;
            Stopwatch RunTimer;
            try
            {
                RunTimer = new Stopwatch();
                // CancelButtonState = BtnCancel.Enabled ' Save the current button enabled states so they can be restored later
                OKButtonState = BtnClose.Enabled;
                LogRunMessage(string.Format("Cancel button state: {0}, OK button state: {1}", CancelButtonState, OKButtonState));
                // BtnCancel.Enabled = False
                BtnClose.Enabled = false;
                BtnRunAutomaticUpdate.Enabled = false;

                LogRunMessage(string.Format("Creating process info"));
                psi = new ProcessStartInfo();

                if (Environment.Is64BitOperatingSystem)
                {
                    psi.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + Astrometry.GlobalItems.DOWNLOAD_TASK_EXECUTABLE_NAME;
                    LogRunMessage(string.Format("Running on a 64bit OS. Executable path: {0}", psi.FileName));
                }
                else
                {
                    psi.FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + Astrometry.GlobalItems.DOWNLOAD_TASK_EXECUTABLE_NAME;
                    LogRunMessage(string.Format("Running on a 32bit OS. Executable path: {0}", psi.FileName));
                }

                psi.Arguments = "/datasource " + CmbDataSource.Text;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                LogRunMessage(string.Format("ProcessInfo Filename: {0}, Arguments: {1}", psi.FileName, psi.Arguments));

                RunTimer.Start();
                proc = Process.Start(psi);
                LogRunMessage(string.Format("Started process on {0} as {1}", proc.MachineName, proc.ProcessName));

                UpdateCompleted = false;
                do
                {
                    UpdateCompleted = proc.WaitForExit(200);
                    Application.DoEvents();
                    LogRunMessage(string.Format("Job running - {0} / {1} seconds", RunTimer.Elapsed.TotalSeconds.ToString("0"), UPDATE_DATA_PROCESS_TIMEOUT));
                }
                while (!(UpdateCompleted | RunTimer.Elapsed.TotalSeconds > UPDATE_DATA_PROCESS_TIMEOUT));
                RunTimer.Stop();

                proc.WaitForExit(); // Ensure that all processing is complete before proceeding
                if (UpdateCompleted)
                {
                    if (proc.ExitCode == 0)
                    {
                        LogRunMessage(string.Format("Job completed OK in {0} seconds.", RunTimer.Elapsed.TotalSeconds.ToString("0.0")));
                    }
                    else
                    {
                        LogRunMessage(string.Format("Job failed with return code {0} after {1} seconds.", proc.ExitCode, RunTimer.Elapsed.TotalSeconds.ToString("0.0")));
                    }
                }

                else
                {
                    LogRunMessage(string.Format("Job timed out after {0} seconds, data not updated", RunTimer.Elapsed.TotalSeconds.ToString("0.0")));
                    LogRunMessage(string.Format("Killing process"));
                    try
                    {
                        proc.Kill();
                    }
                    catch (Exception ex)
                    {
                        LogRunMessage("Exception killing process: " + ex.ToString());
                    }
                }
                Parameters.RefreshState();
            }

            catch (Exception ex)
            {
                LogRunMessage(string.Format("Exception: {0}", ex.Message));
                TL.LogMessageCrLf("RunAutomaticUpdate", "Exception running process: " + ex.ToString());
            }
            finally
            {
                // BtnCancel.Enabled = CancelButtonState ' Ensure that the original button states are restored
                BtnClose.Enabled = OKButtonState;
                BtnRunAutomaticUpdate.Enabled = true;
                UpdateStatus();
            }
        }

        private void BtnSetTraceDirectory_Click(object sender, EventArgs e)
        {
            DialogResult result;
            FolderBrowser.RootFolder = Environment.SpecialFolder.Desktop;
            FolderBrowser.SelectedPath = Parameters.DownloadTaskTracePath;
            result = FolderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                Parameters.DownloadTaskTracePath = FolderBrowser.SelectedPath;
                TraceFilePath = FolderBrowser.SelectedPath;
                TxtTraceFilePath.Text = TraceFilePath;
                Parameters.DownloadTaskTracePath = TraceFilePath;
                TL.LogMessage("TraceFilePath", string.Format("Trace file path updated to: {0}", TraceFilePath));
            }
        }

        private void CmbUpdateType_Changed(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string originalValue, newValue;

            originalValue = EarthRotationDataUpdateType;
            newValue = Conversions.ToString(comboBox.SelectedItem);
            if (!string.IsNullOrEmpty(newValue))
            {
                EarthRotationDataUpdateType = newValue;
                Parameters.UpdateType = EarthRotationDataUpdateType;
                Parameters.ManageScheduledTask(); // Create, update or remove the scheduled task as appropriate
                TL.LogMessage("UpdateTypeEvent", string.Format("Changing current value: '{0}' to: '{1}'", originalValue, EarthRotationDataUpdateType));
            }
            else
            {
                TL.LogMessage("UpdateTypeEvent", string.Format("New value is null or empty, ignoring change"));
            }

            TL.BlankLine();
            TL.LogMessage("UpdateTypeEvent", string.Format("Earth rotation data update configuration changes completed."));

            EnableControlsAsRequired();

        }

        private void CmbSchedulRepeat_Changed(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string orig;

            orig = AutomaticScheduleJobRepeatFrequency;
            AutomaticScheduleJobRepeatFrequency = Conversions.ToString(comboBox.SelectedItem);
            Parameters.DownloadTaskRepeatFrequency = AutomaticScheduleJobRepeatFrequency;
            Parameters.ManageScheduledTask();
            EnableControlsAsRequired();
            TL.LogMessage("DownloadTaskRepeatFrequency", string.Format("Schedule job repeat frequency updated from: {0} to: {1}", orig, AutomaticScheduleJobRepeatFrequency));
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            Button Btn;
            string HelpFilePath;
            Btn = (Button)sender;

            if (Environment.Is64BitOperatingSystem)
            {
                HelpFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + PLATFORM_HELP_FILE;
            }
            else
            {
                HelpFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + PLATFORM_HELP_FILE;
            }

            Help.ShowHelp(Btn.Parent, HelpFilePath, HelpNavigator.Topic, EARTH_ROTATION_HELP_TOPIC);
        }

        private void ChkTraceEnabled_CheckedChanged(object sender, EventArgs e)
        {
            TraceEnabled = ChkTraceEnabled.Checked;
            Parameters.DownloadTaskTraceEnabled = TraceEnabled;
            TL.LogMessage("TraceEnabled", string.Format("Trace enabled updated to: {0}", TraceEnabled));
        }

        private void DateScheduleRun_ValueChanged(object sender, EventArgs e)
        {
            AutomaticScheduleJobRunTime = DateScheduleRun.Value;
            Parameters.DownloadTaskScheduledTime = AutomaticScheduleJobRunTime;
            Parameters.ManageScheduledTask();
            TL.LogMessage("AutomaticScheduleJobRunTime", string.Format("Schedule job run time updated to: {0}", AutomaticScheduleJobRunTime.ToString(Astrometry.GlobalItems.DOWNLOAD_TASK_TIME_FORMAT)));
        }

        #endregion

        #region Input validation routines

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateURI();
        }

        private void TxtDownloadTimeout_Validating(object sender, KeyEventArgs e)
        {
            double DoubleValue = 0.0d;
            bool IsDouble;

            IsDouble = double.TryParse(TxtDownloadTimeout.Text, NumberStyles.Float, CultureInfo.CurrentUICulture, out DoubleValue);
            if (IsDouble & DoubleValue >= MINIMUM_UPDATE_RUN_TIME)
            {
                ErrorProvider1.SetError(TxtDownloadTimeout, "");
                DownloadTimeout = DoubleValue;
                Parameters.DownloadTaskTimeOut = DownloadTimeout;
                TL.LogMessage("DownloadTimeout", string.Format("Download timeout updated to: {0}", DownloadTimeout));
                BtnClose.Enabled = true;
            }
            else
            {
                BtnClose.Enabled = false;
                ErrorProvider1.SetError(TxtDownloadTimeout, string.Format("Must be a number >= {0}!", MINIMUM_UPDATE_RUN_TIME.ToString("0.0", CultureInfo.CurrentUICulture)));
            }
        }

        private void TxtManualLeapSeconds_Validating(object sender, KeyEventArgs e)
        {
            double DoubleValue = 0.0d;
            bool IsDouble;

            IsDouble = double.TryParse(TxtManualLeapSeconds.Text, NumberStyles.Float, CultureInfo.CurrentUICulture, out DoubleValue);
            if (IsDouble & DoubleValue >= LeapSecondMinimumValue)
            {
                ErrorProvider1.SetError(TxtManualLeapSeconds, "");
                ManualLeapSeconds = DoubleValue;
                Parameters.ManualLeapSeconds = ManualLeapSeconds;
                TL.LogMessage("ManualLeapSeconds", string.Format("Manual leap seconds updated to: {0}", ManualLeapSeconds));
                BtnClose.Enabled = true;
            }
            else
            {
                BtnClose.Enabled = false;
                ErrorProvider1.SetError(TxtManualLeapSeconds, string.Format("Must be a number >= {0}!", LeapSecondMinimumValue.ToString("0.0", CultureInfo.CurrentUICulture)));
            }
        }

        private void CmbDataSource_Validating(object sender, KeyEventArgs e)
        {
            ValidateURI();
        }

        private void TxtDeltaUT1Manuals_Validating(object sender, KeyEventArgs e)
        {
            double DoubleValue = 0.0d;
            bool IsDouble;

            IsDouble = double.TryParse(TxtManualDeltaUT1.Text, NumberStyles.Float, CultureInfo.CurrentUICulture, out DoubleValue);
            if (IsDouble & DoubleValue >= -DELTAUT1_ACCEPTABLE_RANGE & DoubleValue <= +DELTAUT1_ACCEPTABLE_RANGE)
            {
                ErrorProvider1.SetError(TxtManualDeltaUT1, "");
                ManualDeltaUT1Value = DoubleValue;
                Parameters.ManualDeltaUT1 = ManualDeltaUT1Value;
                TL.LogMessage("ManualDeltaUT1Value", string.Format(CultureInfo.CurrentUICulture, "Manual DeltaUT1 value updated to: {0}", ManualDeltaUT1Value));
                BtnClose.Enabled = true;
            }
            else
            {
                BtnClose.Enabled = false;
                ErrorProvider1.SetError(TxtManualDeltaUT1, string.Format(CultureInfo.CurrentUICulture, "Must be in the range -{0} to +{0}!", DELTAUT1_ACCEPTABLE_RANGE));
            }
        }

        #endregion

        #region Display drawing event handlers

        private void GroupBox_Paint(object sender, PaintEventArgs e)
        {
            const int HEIGHT_OFFSET = 8;
            const int WIDTH_OFFSET = 1;
            const int PEN_WIDTH = 1;

            GroupBox thisControl;
            Graphics gfx;
            Pen p;
            Size tSize;
            Color activeBorder, inactiveBorder, borderColour;

            activeBorder = SystemColors.Highlight;
            inactiveBorder = Color.LightGray;

            thisControl = (GroupBox)sender;
            gfx = e.Graphics;
            tSize = TextRenderer.MeasureText(thisControl.Text, thisControl.Font);

            switch (thisControl.Name ?? "")
            {
                case var @case when @case == (GrpOnDemandAndAutomaticUpdateConfiguration.Name ?? ""):
                    {
                        switch (EarthRotationDataUpdateType ?? "")
                        {
                            case Astrometry.GlobalItems.UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1:
                            case Astrometry.GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1:
                            case Astrometry.GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1:
                                {
                                    borderColour = inactiveBorder;
                                    break;
                                }
                            case Astrometry.GlobalItems.UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1:
                            case Astrometry.GlobalItems.UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1:
                                {
                                    borderColour = activeBorder;
                                    break;
                                }

                            default:
                                {
                                    borderColour = Color.Red; // Warning colour that something has gone wrong in the code!
                                    break;
                                }
                        }
                        LblAutoDataSource.Enabled = borderColour == activeBorder;
                        LblAutoTimeout.Enabled = borderColour == activeBorder;
                        LblAutoSeconds.Enabled = borderColour == activeBorder;
                        break;
                    }

                case var case1 when case1 == (GrpManualUpdate.Name ?? ""):
                    {
                        switch (EarthRotationDataUpdateType ?? "")
                        {
                            case Astrometry.GlobalItems.UPDATE_BUILTIN_LEAP_SECONDS_PREDICTED_DELTAUT1:
                            case Astrometry.GlobalItems.UPDATE_ON_DEMAND_LEAP_SECONDS_AND_DELTAUT1:
                            case Astrometry.GlobalItems.UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1:
                                {
                                    borderColour = inactiveBorder;
                                    break;
                                }
                            case Astrometry.GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_MANUAL_DELTAUT1:
                            case Astrometry.GlobalItems.UPDATE_MANUAL_LEAP_SECONDS_PREDICTED_DELTAUT1:
                                {
                                    borderColour = activeBorder;
                                    break;
                                }

                            default:
                                {
                                    borderColour = Color.Red; // Warning colour that something has gone wrong in the code!
                                    break;
                                }
                        }
                        LblManualDeltaUT1.Enabled = borderColour == activeBorder;
                        LblManualLeapSeconds.Enabled = borderColour == activeBorder;
                        break;
                    }

                case var case2 when case2 == (GrpScheduleTime.Name ?? ""):
                    {
                        if ((EarthRotationDataUpdateType ?? "") == Astrometry.GlobalItems.UPDATE_AUTOMATIC_LEAP_SECONDS_AND_DELTAUT1)
                        {
                            borderColour = activeBorder;
                        }
                        else
                        {
                            borderColour = inactiveBorder;
                        }
                        LblAutoDownloadTime.Enabled = borderColour == activeBorder;
                        LblAutoRepeatFrequency.Enabled = borderColour == activeBorder;
                        break;
                    }

                default:
                    {
                        borderColour = activeBorder;
                        break;
                    }
            }

            p = new Pen(borderColour, PEN_WIDTH);
            thisControl.ForeColor = borderColour;

            gfx.DrawLine(p, 0, HEIGHT_OFFSET, 0, e.ClipRectangle.Height - 2); // left vertical
            gfx.DrawLine(p, 0, HEIGHT_OFFSET, 7, HEIGHT_OFFSET); // Top left part
            gfx.DrawLine(p, tSize.Width + 7, HEIGHT_OFFSET, e.ClipRectangle.Width - WIDTH_OFFSET, HEIGHT_OFFSET); // Top right part
            gfx.DrawLine(p, e.ClipRectangle.Width - WIDTH_OFFSET, HEIGHT_OFFSET, e.ClipRectangle.Width - WIDTH_OFFSET, e.ClipRectangle.Height - 2); // Right vertical
            gfx.DrawLine(p, e.ClipRectangle.Width - WIDTH_OFFSET, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2); // Bottom

        }

        /// <summary>
    /// Event handler to paint the device list combo box in the "DropDown" rather than "DropDownList" style
    /// </summary>
    /// <param name="sender">Device to be painted</param>
    /// <param name="e">Draw event arguments object</param>
        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color DisabledForeColour, DisabledBackColour;
            ComboBox combo = (ComboBox)sender;

            if (e.Index < 0)
                return;

            DisabledForeColour = SystemColors.GrayText;
            DisabledBackColour = SystemColors.ButtonFace;


            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) // Draw Then the selected item In menu highlight colour
            {

                if (combo.Enabled)
                {
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.MenuHighlight), e.Bounds);
                    e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(SystemColors.HighlightText), new Point(e.Bounds.X, e.Bounds.Y));
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(DisabledBackColour), e.Bounds);
                    e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(DisabledForeColour), new Point(e.Bounds.X, e.Bounds.Y));
                }
            }

            else if (combo.Enabled)
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window), e.Bounds);
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(combo.ForeColor), new Point(e.Bounds.X, e.Bounds.Y));
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(DisabledBackColour), e.Bounds);
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(DisabledForeColour), new Point(e.Bounds.X, e.Bounds.Y));

            }

            e.DrawFocusRectangle();
        }

        #endregion

    }
}