using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using ASCOM.DeviceHub.MvvmMessenger;
using ASCOM.Utilities;

namespace ASCOM.DeviceHub
{
    public class ActivityLogViewModel : DeviceHubDialogViewModelBase
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetOpenClipboardWindow();

        [DllImport("user32")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        private object _bufferLock = new Object();
        private int _bufferInterval = 1000; // empty buffer to log once/second
        private TaskScheduler UISyncContext { get; set; }
        private StringBuilder DataBuffer { get; set; }
        private CancellationTokenSource BufferingCts { get; set; }
        private IActivityLogService ActivityLogService { get; set; }

        private TraceLogger TL;

        public ActivityLogViewModel() : base("Activity Log")
        {
            // Create an activity log file if required
            if (Globals.WriteLogActivityToDisk)
                CreateActivityLog();

            _isActive = false;
            _logContents = String.Empty;
            _allLoggingPaused = false;
            _pausedCommandText = "Pause";
            UpdateMemoryUsage();
            Messenger.Default.Register<ActivityMessage>(this, (action) => AppendToLog(action));
            Messenger.Default.Register<ApplicationSettingsUpdatedMessage>(this, (action) => ApplicationSettingsUpdatedEventHandler(action));
            UISyncContext = TaskScheduler.FromCurrentSynchronizationContext();
            DataBuffer = new StringBuilder();

            BufferingCts = new CancellationTokenSource();
            Task.Factory.StartNew(() => AppendBufferToLog(BufferingCts.Token), BufferingCts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            ActivityLogService = ServiceContainer.Instance.GetService<IActivityLogService>();

            // Set initial states for activity logging variables
            EnableTelescopeLogging = Globals.ActivityLogTelescopeDevice;
            EnableDomeLogging = Globals.ActivityLogDomeDevice;
            EnableFocuserLogging = Globals.ActivityLogFocuserDevice;
            EnableCommandLogging = Globals.ActivityLogCommands;
            EnableStatusLogging = Globals.ActivityLogStatus;
            EnableParametersLogging = Globals.ActivityLogParameters;
            EnableCapabilitiesLogging = Globals.ActivityLogCapabilities;
            EnableOthersLogging = Globals.ActivityLogOtherActivity;
        }

        private void CreateActivityLog()
        {
            TL = new TraceLogger("", "DeviceHub.Activity");
            TL.Enabled = Globals.WriteLogActivityToDisk;
            TL.LogMessage("CreateActivityLog", "Logger created");
            TL.LogMessage("", "");
        }

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (value != _isActive)
                {
                    _isActive = value;

                    if (_isActive)
                    {
                        // Clear the log when the log viewer is activated.

                        ClearLog();
                    }
                }
            }
        }

        #region Change Notification Properties

        private bool _enableTelescopeLogging;

        public bool EnableTelescopeLogging
        {
            get { return _enableTelescopeLogging; }
            set
            {
                if (value != _enableTelescopeLogging)
                {
                    _enableTelescopeLogging = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _enableDomeLogging;

        public bool EnableDomeLogging
        {
            get { return _enableDomeLogging; }
            set
            {
                if (value != _enableDomeLogging)
                {
                    _enableDomeLogging = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _enableFocuserLogging;

        public bool EnableFocuserLogging
        {
            get { return _enableFocuserLogging; }
            set
            {
                if (value != _enableFocuserLogging)
                {
                    _enableFocuserLogging = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _enableCommandLogging;

        public bool EnableCommandLogging
        {
            get { return _enableCommandLogging; }
            set
            {
                if (value != _enableCommandLogging)
                {
                    _enableCommandLogging = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _enableStatusLogging;

        public bool EnableStatusLogging
        {
            get { return _enableStatusLogging; }
            set
            {
                if (value != _enableStatusLogging)
                {
                    _enableStatusLogging = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _enableParametersLogging;

        public bool EnableParametersLogging
        {
            get { return _enableParametersLogging; }
            set
            {
                if (value != _enableParametersLogging)
                {
                    _enableParametersLogging = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _enableCapabilitiesLogging;

        public bool EnableCapabilitiesLogging
        {
            get { return _enableCapabilitiesLogging; }
            set
            {
                if (value != _enableCapabilitiesLogging)
                {
                    _enableCapabilitiesLogging = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _enableOthersLogging;

        public bool EnableOthersLogging
        {
            get { return _enableOthersLogging; }
            set
            {
                if (value != _enableOthersLogging)
                {
                    _enableOthersLogging = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _allLoggingPaused;

        public bool AllLoggingPaused
        {
            get { return _allLoggingPaused; }
            set
            {
                if (value != _allLoggingPaused)
                {
                    _allLoggingPaused = value;
                    OnPropertyChanged();
                    PausedCommandText = _allLoggingPaused ? "Resume" : "Pause";
                }
            }
        }

        private string _pausedCommandText;

        public string PausedCommandText
        {
            get { return _pausedCommandText; }
            set
            {
                if (value != _pausedCommandText)
                {
                    _pausedCommandText = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _logContents;

        public string LogContents
        {
            get { return _logContents; }
            set
            {
                if (value != _logContents)
                {
                    _logContents = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _memoryUsage;

        public double MemoryUsage
        {
            get { return _memoryUsage; }
            set
            {
                if (value != _memoryUsage)
                {
                    _memoryUsage = value;
                    OnPropertyChanged();
                }
            }
        }
        int _numberOfLogEntries;
        public int NumberOfLogEntries
        {
            get
            {
                return _numberOfLogEntries;
            }
            set
            {
                if (value != _numberOfLogEntries)
                {
                    _numberOfLogEntries = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Change Notification Properties

        #region Helper Methods

        private string FormatEntry(string msg)
        {
            return msg;
        }

        private void AppendToLog(ActivityMessage action)
        {
            if (!IsActive || AllLoggingPaused)
            {
                return;
            }

            if (action.DeviceType == DeviceTypeEnum.Telescope && EnableTelescopeLogging
                || action.DeviceType == DeviceTypeEnum.Dome && EnableDomeLogging
                || action.DeviceType == DeviceTypeEnum.Focuser && EnableFocuserLogging)
            {
                if (action.MessageType == ActivityMessageTypes.Capabilities && EnableCapabilitiesLogging
                    || action.MessageType == ActivityMessageTypes.Commands && EnableCommandLogging
                    || action.MessageType == ActivityMessageTypes.Other && EnableOthersLogging
                    || action.MessageType == ActivityMessageTypes.Parameters && EnableParametersLogging
                    || action.MessageType == ActivityMessageTypes.Status && EnableStatusLogging)
                {
                    string newText = FormatEntry(action.MessageText);

                    // UpdateTheLog( newText );

                    lock (_bufferLock)
                    {
                        DataBuffer.Append(newText);

                        // Write the message to the activity log file.
                        // The incoming message is parsed to extract the core message and discard the time stamp and device type prefixes before being logged

                        // Locate the position of the '-' separator character
                        int spacePosition = newText.IndexOf("-");

                        // Create a string builder from the message having stripped any trailing carriage return and line feed characters
                        StringBuilder textSubset = new StringBuilder(newText.TrimEnd('\r', '\n'));

                        // Discard everything before the '-' separator, the '-' separator itself, and the following space if these are present
                        if ((spacePosition >= 0) & (spacePosition <= textSubset.Length - 3))
                            textSubset = textSubset.Remove(0, spacePosition + 2);

                        // Write the message to the log file
                        TL?.LogMessageCrLf($"{action.DeviceType}", textSubset.ToString());
                    }
                }
            }
        }

        private void ApplicationSettingsUpdatedEventHandler(ApplicationSettingsUpdatedMessage message)
        {
            // Create the activity log if this is the first time that tracing has been enabled in this Device Hub session
            if (Globals.WriteLogActivityToDisk & (TL is null))
                CreateActivityLog();

            // Set the trace state if the logging is, or has been previously, enabled in this Device Hub session
            // TL will be null, not TraceLogger, if logging has never been enabled
            if (TL is TraceLogger)
                TL.Enabled = Globals.WriteLogActivityToDisk;
        }

        private void AppendBufferToLog(CancellationToken token)
        {
            bool taskCancelled = false;

            while (!taskCancelled)
            {
                string newData = "";

                lock (_bufferLock)
                {
                    if (DataBuffer.Length > 0)
                    {
                        newData = DataBuffer.ToString();
                        DataBuffer.Clear();
                    }
                }

                if (newData.Length > 0)
                {
                    UpdateTheLog(newData);
                }

                taskCancelled = token.WaitHandle.WaitOne(_bufferInterval);
            }
        }

        private async void UpdateTheLog(string newText)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                string contents = LogContents;

                // Figure out how many lines to delete to get the size under our limit;

                int lengthLimit = Properties.Settings.Default.ActivityLogCapacity;
                int charsToTrim = 0;

                // Trim whole lines from the LogContents so that when the new text is appended, the total
                // length is less than the allowed limit.

                if (contents.Length > 0)
                {
                    // We can only trim the log contents if it has data!

                    int minToDelete = contents.Length + newText.Length - lengthLimit;

                    if (minToDelete > 0)
                    {
                        // We need to delete chars from the start of contents to keep below the length limit.

                        if (minToDelete > contents.Length)
                        {
                            contents = String.Empty;
                        }
                        else
                        {
                            int index = contents.IndexOf(Environment.NewLine, minToDelete, StringComparison.InvariantCulture) + 1;

                            if (index >= 0)
                            {
                                charsToTrim = index;
                            }
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();

                if (contents.Length > 0)
                {
                    sb.Append(contents.Substring(charsToTrim));
                }

                sb.Append(newText);

                LogContents = sb.ToString();
                UpdateMemoryUsage();

                // Calculate the number of log entries by counting the number of line feed characters.
                // Scanning the string builder 'sb' variable rather than the string property 'LogContents' because it is way faster.
                // Using a for loop because it is faster than using LINQ.
                int count = 0;
                Stopwatch sw = Stopwatch.StartNew();
                for (int i = 0; i < sb.Length; i++)
                {
                    if (sb[i] == '\n') count++;
                }

                NumberOfLogEntries = count;

            }, CancellationToken.None, TaskCreationOptions.None, UISyncContext);

            await task;

            task.Dispose();
            task = null;
        }

        private void UpdateMemoryUsage()
        {
            MemoryUsage = (double)GC.GetTotalMemory(false) / Math.Pow(1024.0, 2.0);
        }

        protected override void DoDispose()
        {
            _pauseLoggingCommand = null;
            _clearLogCommand = null;
            _copyLogCommand = null;
            _hideLogCommand = null;

            try
            {
                TL?.LogMessage("Dispose", "Closing activity log");
                TL?.Dispose();
                TL = null;
            }
            catch (Exception) { }
        }

        #endregion Helper Methods

        #region Relay Commands

        #region PauseLoggingCommand

        private ICommand _pauseLoggingCommand;

        public ICommand PauseLoggingCommand
        {
            get
            {
                if (_pauseLoggingCommand == null)
                {
                    _pauseLoggingCommand = new RelayCommand(
                        param => this.PauseLogging());
                }

                return _pauseLoggingCommand;
            }
        }

        private void PauseLogging()
        {
            AllLoggingPaused = !AllLoggingPaused;
        }

        #endregion PauseLoggingCommand

        #region ClearLogCommand

        private ICommand _clearLogCommand;

        public ICommand ClearLogCommand
        {
            get
            {
                if (_clearLogCommand == null)
                {
                    _clearLogCommand = new RelayCommand(
                        param => this.ClearLog());
                }

                return _clearLogCommand;
            }
        }

        private void ClearLog()
        {
            LogContents = String.Empty;
        }

        #endregion ClearLogCommand

        #region CopyLogCommand

        private ICommand _copyLogCommand;

        public ICommand CopyLogCommand
        {
            get
            {
                if (_copyLogCommand == null)
                {
                    _copyLogCommand = new RelayCommand(
                        param => this.CopyLog(),
                        param => this.CanCopyLog());
                }

                return _copyLogCommand;
            }
        }

        private void CopyLog()
        {
            try
            {
                Clipboard.SetText(LogContents);
            }
            catch (COMException xcp)
            {
                string msg = "An unexpected error occurred when attempting to copy to the Windows Clipboard";

                if ((uint)xcp.HResult == 0x800401d0)
                {
                    string processName = GetClipboardLocker();
                    processName = String.IsNullOrEmpty(processName) ? "Unknown process" : processName;
                    msg = $"Control of the Windows Clipboard has been taken by a process named {processName}.\r\n\r\nPlease wait and try again or close {processName} to release the Clipboard..";
                }

                IMessageBoxService svc = ServiceContainer.Instance.GetService<IMessageBoxService>();
                svc.Show(msg, "Clipboard Copy Error", MessageBoxButton.OK, MessageBoxImage.Error
                            , MessageBoxResult.None, MessageBoxOptions.None);
            }
        }

        private string GetClipboardLocker()
        {
            string retval = String.Empty;

            IntPtr hwnd = GetOpenClipboardWindow();

            if (hwnd != IntPtr.Zero)
            {

                GetWindowThreadProcessId(hwnd, out int pid);

                if (pid > 0)
                {
                    retval = Process.GetProcessById(pid).ProcessName;
                }
            }

            return retval;
        }

        private bool CanCopyLog()
        {
            return !String.IsNullOrWhiteSpace(LogContents);
        }

        #endregion CopyLogCommand

        #region HideLogCommand

        private ICommand _hideLogCommand;

        public ICommand HideLogCommand
        {
            get
            {
                if (_hideLogCommand == null)
                {
                    _hideLogCommand = new RelayCommand(
                        param => HideLog(),
                        parm => true); // Always true because the close log button now only hides the dialogue rrather than closing it, and we always want to be able to open the dialogue again.
                }

                return _hideLogCommand;
            }
        }

        internal void HideLog()
        {
            // Hide the activity log dialogue
            ActivityLogService.HideActivityLog();
        }

        internal void CloseLog()
        {
            Messenger.Default.Unregister<ActivityMessage>(this);

            BufferingCts.Cancel();
            BufferingCts.Dispose();
            BufferingCts = null;
            DataBuffer.Clear();
            DataBuffer = null;
            OnRequestClose(true);
            IsActive = false;
        }

        #endregion HideLogCommand

        #region SaveSettingsCommand

        private ICommand _saveSettingsCommand;

        public ICommand SaveSettingsCommand
        {
            get
            {
                if (_saveSettingsCommand == null)
                {
                    _saveSettingsCommand = new RelayCommand(
                        param => this.SaveSettings());
                }

                return _saveSettingsCommand;
            }
        }

        public void SaveSettings()
        {
            // Update the global settings values
            Globals.ActivityLogTelescopeDevice = EnableTelescopeLogging;
            Globals.ActivityLogDomeDevice = EnableDomeLogging;
            Globals.ActivityLogFocuserDevice = EnableFocuserLogging;
            Globals.ActivityLogCommands = EnableCommandLogging;
            Globals.ActivityLogStatus = EnableStatusLogging;
            Globals.ActivityLogParameters = EnableParametersLogging;
            Globals.ActivityLogCapabilities = EnableCapabilitiesLogging;
            Globals.ActivityLogOtherActivity = EnableOthersLogging;

            // Save the global settings to disk
            AppSettingsManager.SaveAppSettings();
        }

        #endregion SaveSettingsCommand

        #endregion Relay Commands
    }
}
