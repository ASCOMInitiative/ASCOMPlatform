using ASCOM.DeviceHub.MvvmMessenger;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Input;

namespace ASCOM.DeviceHub
{
    public class SetupViewModel : DeviceHubDialogViewModelBase
    {

        #region variables

        private string TelescopeID { get; set; }
        private string DomeID { get; set; }
        private string FocuserID { get; set; }

        private bool _showActivityLogWhenStarted;
        private bool _writeActivityLogToDisk;
        private bool _suppressTrayBubble;
        private bool _useCustomTheme;
        private bool _useExpandedScreenLayout;
        private bool _keepWindowOnTop;
        private bool _useCompositeSlewingFlag;
        private bool _isTelescopeActive;
        private bool _isDomeActive;
        private bool _isFocuserActive;

        #endregion variables

        #region Constructor and Dispose

        public SetupViewModel()
            : base("Device Hub Setup")
        {
            SuppressTrayBubble = Globals.SuppressTrayBubble;
            UseCustomTheme = Globals.UseCustomTheme;
            UseExpandedScreenLayout = Globals.UseExpandedScreenLayout;
            KeepWindowOnTop = Globals.AlwaysOnTop;
            UseCompositeSlewingFlag = Globals.UseCompositeSlewingFlag;
            ShowActivityLogWhenStarted = Globals.ShowActivityLogWhenStarted;
            WriteActivityLogToDisk = Globals.WriteLogActivityToDisk;
            TelescopeSetupVm = new TelescopeSetupViewModel();
            DomeSetupVm = new DomeSetupViewModel();
            DomeOffsetsVm = new DomeOffsetsViewModel();
            FocuserSetupVm = new FocuserSetupViewModel();
        }

        protected override void DoDispose()
        {
            _setupOKCommand = null;
            _setupCancelCommand = null;

            TelescopeSetupVm.Dispose();
            TelescopeSetupVm = null;
            DomeSetupVm.Dispose();
            DomeSetupVm = null;
            DomeOffsetsVm.Dispose();
            DomeOffsetsVm = null;
            FocuserSetupVm.Dispose();
            FocuserSetupVm = null;
        }

        #endregion Constructor and Dispose

        #region Public  Properties

        public TelescopeSetupViewModel TelescopeSetupVm { get; set; }

        public DomeSetupViewModel DomeSetupVm { get; set; }

        public DomeOffsetsViewModel DomeOffsetsVm { get; set; }

        public FocuserSetupViewModel FocuserSetupVm { get; set; }

        public bool ShowActivityLogWhenStarted
        {
            get { return _showActivityLogWhenStarted; }
            set
            {
                if (value != _showActivityLogWhenStarted)
                {
                    _showActivityLogWhenStarted = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool WriteActivityLogToDisk
        {
            get { return _writeActivityLogToDisk; }
            set
            {
                if (value != _writeActivityLogToDisk)
                {
                    _writeActivityLogToDisk = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool SuppressTrayBubble
        {
            get { return _suppressTrayBubble; }
            set
            {
                if (value != _suppressTrayBubble)
                {
                    _suppressTrayBubble = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseCustomTheme
        {
            get { return _useCustomTheme; }
            set
            {
                if (value != _useCustomTheme)
                {
                    _useCustomTheme = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseExpandedScreenLayout
        {
            get { return _useExpandedScreenLayout; }
            set
            {
                if (value != _useExpandedScreenLayout)
                {
                    _useExpandedScreenLayout = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool KeepWindowOnTop
        {
            get { return _keepWindowOnTop; }
            set
            {
                if (value != _keepWindowOnTop)
                {
                    _keepWindowOnTop = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseCompositeSlewingFlag
        {
            get { return _useCompositeSlewingFlag; }
            set
            {
                if (value != _useCompositeSlewingFlag)
                {
                    _useCompositeSlewingFlag = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsTelescopeActive
        {
            get { return _isTelescopeActive; }
            set
            {
                if (value != _isTelescopeActive)
                {
                    _isTelescopeActive = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsDomeActive
        {
            get { return _isDomeActive; }
            set
            {
                if (value != _isDomeActive)
                {
                    _isDomeActive = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsFocuserActive
        {
            get { return _isFocuserActive; }
            set
            {
                if (value != _isFocuserActive)
                {
                    _isFocuserActive = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void ChangeActiveFunction(string functionName)
        {
            // Called from code-behind when the active tab item is changed.

            if (functionName == "Device Hub Setup") { }
            else if (functionName == "Telescope Setup") { }
            else if (functionName == "Dome Setup") { }
            else if (functionName == "Dome Offsets") { }
            else if (functionName == "Focuser Setup") { }
            else
            {
                string msg = $"SetupViewModel.ChangeActiveFunction called with invalid function Name - {functionName}.";
                throw new ArgumentException(msg);
            }
        }

        public void InitializeCurrentTelescope(string telescopeID, double fastPollingPeriod)
        {
            TelescopeID = telescopeID;
            TelescopeSetupVm.TelescopeID = telescopeID;
            TelescopeSetupVm.FastUpdatePeriod = fastPollingPeriod;
            IsTelescopeActive = TelescopeManager.Instance.IsConnected;
        }

        public void InitializeCurrentDome(string domeID, double fastUpdatePeriod)
        {
            DomeID = domeID;
            DomeSetupVm.DomeID = domeID;
            DomeSetupVm.InitializeLayout(Globals.DomeLayoutSettings);
            DomeSetupVm.FastUpdatePeriod = fastUpdatePeriod;
            IsDomeActive = DomeManager.Instance.IsConnected;
            DomeOffsetsVm.InitializeLayout(Globals.DomeLayoutSettings);
        }

        public void InitializeCurrentFocuser(string focuserID, double fastUpdatePeriod)
        {
            FocuserID = focuserID;
            FocuserSetupVm.FocuserID = focuserID;
            FocuserSetupVm.Initialize(Globals.FocuserTemperatureOffset);
            FocuserSetupVm.FastUpdatePeriod = fastUpdatePeriod;
            IsFocuserActive = FocuserManager.Instance.IsConnected;
        }

        #endregion Public Methods

        #region Helper Methods

        private void SaveSettings()
        {
            SaveApplicationSettings();
            SaveTelescopeSettings();
            SaveDomeSettings();
            SaveFocuserSettings();
        }

        private void SaveApplicationSettings()
        {
            Globals.SuppressTrayBubble = SuppressTrayBubble;
            Globals.UseCustomTheme = UseCustomTheme;
            Globals.AlwaysOnTop = KeepWindowOnTop;
            Globals.UseCompositeSlewingFlag = UseCompositeSlewingFlag;
            Globals.ShowActivityLogWhenStarted = ShowActivityLogWhenStarted;
            Globals.WriteLogActivityToDisk = WriteActivityLogToDisk;
            AppSettingsManager.SaveAppSettings();
            Messenger.Default.Send(new ApplicationSettingsUpdatedMessage());
        }

        private void SaveTelescopeSettings()
        {
            // Read the current settings and update them with the changes
            // to preserve the logging flag.

            TelescopeSettings settings = TelescopeSettings.FromProfile();
            settings.TelescopeID = TelescopeManager.TelescopeID;
            settings.FastUpdatePeriod = TelescopeManager.Instance.FastPollingPeriod;
            settings.ToProfile();
        }

        private void SaveDomeSettings()
        {
            // Read the current settings and update them with the changes
            // to preserve the logging flag.

            DomeSettings settings = DomeSettings.FromProfile();
            settings.DomeID = DomeManager.DomeID;
            settings.DomeLayoutSettings = Globals.DomeLayoutSettings;
            settings.FastUpdatePeriod = DomeManager.Instance.FastPollingPeriod;
            settings.ToProfile();
        }

        private void SaveFocuserSettings()
        {
            FocuserSettings settings = FocuserSettings.FromProfile();
            settings.FocuserID = FocuserManager.FocuserID;
            settings.TemperatureOffset = Globals.FocuserTemperatureOffset;
            settings.FastUpdatePeriod = FocuserManager.Instance.FastPollingPeriod;
            settings.ToProfile();
        }

        #endregion Helper Methods

        #region Relay Commands

        #region SetupOKCommand

        private ICommand _setupOKCommand;

        public ICommand SetupOKCommand
        {
            get
            {
                if (_setupOKCommand == null)
                {
                    _setupOKCommand = new RelayCommand(
                        param => this.SetupOK());
                }

                return _setupOKCommand;
            }
        }

        private void SetupOK()
        {
            if (!IsTelescopeActive)
            {
                if (TelescopeID != TelescopeSetupVm.TelescopeID)
                {
                    TelescopeID = TelescopeSetupVm.TelescopeID;
                    TelescopeManager.SetTelescopeID(TelescopeID);
                }

                TelescopeManager.Instance.SetFastUpdatePeriod(TelescopeSetupVm.FastUpdatePeriod);
            }

            if (!IsDomeActive)
            {
                if (DomeID != DomeSetupVm.DomeID)
                {
                    DomeID = DomeSetupVm.DomeID;
                    DomeManager.SetDomeID(DomeID);
                }

                DomeManager.Instance.SetFastUpdatePeriod(DomeSetupVm.FastUpdatePeriod);

                // Get the current dome layout settings from the dome setup tab.
                DomeLayoutSettings domeLayoutSettings = DomeSetupVm.GetDomeLayoutSettings();

                // Get the current dome offsets from the offsets tab.
                DomeLayoutSettings domeOffsets = DomeOffsetsVm.GetDomeOffsets();

                // Update with current values
                domeLayoutSettings.SupportMultipleTelescopes = domeOffsets.SupportMultipleTelescopes;

                // Update the offsets in the dome layout settings.
                domeLayoutSettings.ProfileIndex = domeOffsets.ProfileIndex; // Must include this because it is mastered on the dome offsets dialogue

                domeLayoutSettings.GemAxisOffset0 = domeOffsets.GemAxisOffset0;
                domeLayoutSettings.GemAxisOffset1 = domeOffsets.GemAxisOffset1;
                domeLayoutSettings.GemAxisOffset2 = domeOffsets.GemAxisOffset2;
                domeLayoutSettings.GemAxisOffset3 = domeOffsets.GemAxisOffset3;
                domeLayoutSettings.GemAxisOffset4 = domeOffsets.GemAxisOffset4;

                domeLayoutSettings.OpticalOffset0 = domeOffsets.OpticalOffset0;
                domeLayoutSettings.OpticalOffset1 = domeOffsets.OpticalOffset1;
                domeLayoutSettings.OpticalOffset2 = domeOffsets.OpticalOffset2;
                domeLayoutSettings.OpticalOffset3 = domeOffsets.OpticalOffset3;
                domeLayoutSettings.OpticalOffset4 = domeOffsets.OpticalOffset4;

                domeLayoutSettings.TelescopeName0 = domeOffsets.TelescopeName0;
                domeLayoutSettings.TelescopeName1 = domeOffsets.TelescopeName1;
                domeLayoutSettings.TelescopeName2 = domeOffsets.TelescopeName2;
                domeLayoutSettings.TelescopeName3 = domeOffsets.TelescopeName3;
                domeLayoutSettings.TelescopeName4 = domeOffsets.TelescopeName4;

                // Update current offsets if multiple telescopes are supported
                if (domeOffsets.SupportMultipleTelescopes) // Multiple telescope support is enabled
                {
                    // Create a list of TelescopeOffsets based on the latest settings from the setup dialogue.
                    List<TelescopeOffsets> offsets = new List<TelescopeOffsets>
                    {
                        new TelescopeOffsets(domeLayoutSettings.TelescopeName0, domeLayoutSettings.GemAxisOffset0, domeLayoutSettings.OpticalOffset0),
                        new TelescopeOffsets(domeLayoutSettings.TelescopeName1, domeLayoutSettings.GemAxisOffset1, domeLayoutSettings.OpticalOffset1),
                        new TelescopeOffsets(domeLayoutSettings.TelescopeName2, domeLayoutSettings.GemAxisOffset2, domeLayoutSettings.OpticalOffset2),
                        new TelescopeOffsets(domeLayoutSettings.TelescopeName3, domeLayoutSettings.GemAxisOffset3, domeLayoutSettings.OpticalOffset3),
                        new TelescopeOffsets(domeLayoutSettings.TelescopeName4, domeLayoutSettings.GemAxisOffset4, domeLayoutSettings.OpticalOffset4)
                    };

                    // Update the current offset settings in case the user changed the values in the setup dialogue.
                    domeLayoutSettings.GemAxisOffset = offsets[domeLayoutSettings.ProfileIndex].OffsetFromAxisIntersection;
                    domeLayoutSettings.OpticalOffset = offsets[domeLayoutSettings.ProfileIndex].OffsetFromDecAltAxis;
                }

                // Save the composite dome layout settings.
                Globals.DomeLayoutSettings = domeLayoutSettings;

                // Notify the main UI that the dome layout settings have changed.
                Messenger.Default.Send(new DomeLayoutSettingsChangedMessage(domeLayoutSettings.Clone()));
            }

            if (!IsFocuserActive)
            {
                if (FocuserID != FocuserSetupVm.FocuserID)
                {
                    FocuserID = FocuserSetupVm.FocuserID;
                    FocuserManager.SetFocuserID(FocuserID);
                }

                FocuserManager.Instance.SetFastUpdatePeriod(FocuserSetupVm.FastUpdatePeriod);
            }

            Globals.SuppressTrayBubble = SuppressTrayBubble;
            Globals.UseCustomTheme = UseCustomTheme;
            Globals.FocuserTemperatureOffset = FocuserSetupVm.TemperatureOffset;

            SaveSettings();

            OnRequestClose(true);
        }

        #endregion SetupOKCommand

        #region SetupCancelCommand

        private ICommand _setupCancelCommand;

        public ICommand SetupCancelCommand
        {
            get
            {
                if (_setupCancelCommand == null)
                {
                    _setupCancelCommand = new RelayCommand(
                        param => this.SetupCancel());
                }

                return _setupCancelCommand;
            }
        }

        private void SetupCancel()
        {
            OnRequestClose(false);
        }

        #endregion SetupCancelCommand

        #endregion Relay Commands

    }
}
