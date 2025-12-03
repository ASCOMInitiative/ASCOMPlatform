using ASCOM.DeviceHub.MvvmMessenger;
using ASCOM.DeviceInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ASCOM.DeviceHub
{
    public class DomeMotionViewModel : DeviceHubViewModelBase
    {
        #region Constants

        private const string REGISTRY_PATH = @"SOFTWARE\ASCOM\DeviceHub";
        private const string REGISTRY_PATH_POTH = @"UsePOTHSlaveCalculation";
        private const string REGISTRY_PATH_ONE_AXIS = @"UseOneAxisDomeSlaveCalculation";

        #endregion Constants

        #region Variables

        private readonly IDomeManager _domeManager;
        private IDomeManager DomeManager => _domeManager;

        // Backing fields for properties
        private bool _supportMultipleTelescopes;
        private object _selectedtelescopeIndex;
        private object _selectedtelescope;
        private bool _domeSyncErrorState;
        private DomeSlewAmounts _slewAmounts;
        private JogAmount _selectedSlewAmount;
        private DevHubDomeStatus _status;
        private bool _isSlewing;
        private double _directTargetAzimuth;
        private double _syncTargetAzimuth;
        private string _shutterCommandAction;
        private DomeCapabilities _capabilities;
        private DomeParameters _parameters;

        private ObservableCollection<string> _telescopeNames;

        #endregion Variables

        #region Constructor and Dispose

        public DomeMotionViewModel(IDomeManager domeManager)
        {
            string caller = "DomeMotionViewModel ctor";
            LogAppMessage("Initializing Instance constructor", caller);

            _domeManager = domeManager;
            _status = null;
            _slewAmounts = new DomeSlewAmounts();
            _selectedSlewAmount = _slewAmounts[0];

            LogAppMessage("Registering message handlers", caller);

            Messenger.Default.Register<DomeCapabilitiesUpdatedMessage>(this, (action) => DomeCapabilitiesUpdated(action));
            Messenger.Default.Register<DomeParametersUpdatedMessage>(this, (action) => DomeParametersUpdated(action));
            Messenger.Default.Register<DeviceDisconnectedMessage>(this, (action) => InvalidateDeviceData(action));
            Messenger.Default.Register<DomeSlavedChangedMessage>(this, (action) => ChangeSlavedState(action));
            Messenger.Default.Register<DomeSyncErrorStateMessage>(this, (action) => DomeSyncErrorStateUpdated(action));
            Messenger.Default.Register<DomeLayoutSettingsChangedMessage>(this, (action) => DomeLayoutSettingsChanged(action));

            RegisterStatusUpdateMessage(true);

            // Initialize the properties that can change
            UsePOTHSlaveCalculation = GetRegistryValueDefaultFalse(REGISTRY_PATH_POTH);
            UseOneAxisSlaveCalculation = GetRegistryValueDefaultFalse(REGISTRY_PATH_ONE_AXIS);

            // Update the current layout settings
            DomeSettings domeSettings = DomeSettings.FromProfile();
            _supportMultipleTelescopes = domeSettings.DomeLayoutSettings.SupportMultipleTelescopes;
            _selectedtelescope = domeSettings.DomeLayoutSettings.ProfileIndex;
            _telescopeNames = GetMultipleTelescopeNames(domeSettings.DomeLayoutSettings);
            LogAppMessage("Initialization complete", caller);
        }

        protected override void DoDispose()
        {
            Messenger.Default.Unregister<DomeCapabilitiesUpdatedMessage>(this);
            Messenger.Default.Unregister<DomeParametersUpdatedMessage>(this);
            Messenger.Default.Unregister<DeviceDisconnectedMessage>(this);
            RegisterStatusUpdateMessage(false);

            _toggleShutterStateCommand = null;
            _parkDomeCommand = null;
            _jogAltitudeCommand = null;
            _jogAzimuthCommand = null;
            _stopMotionCommand = null;
            _gotoDirectAzimuthCommand = null;
            _syncAzimuthCommand = null;
            _findHomeCommand = null;
        }

        #endregion Constructor

        #region Change Notification Properties

        public bool SupportMultipleTelescopes
        {
            //get { return Globals.DomeLayoutSettings.SupportMultipleTelescopes; }
            get { return _supportMultipleTelescopes; }
            set
            {
                if (value != _supportMultipleTelescopes)
                {
                    _supportMultipleTelescopes = value;
                    OnPropertyChanged();
                }
            }
        }

        public object SelectedTelescopeIndex
        {
            get { return _selectedtelescopeIndex; }
            set
            {
                if (value != _selectedtelescopeIndex)
                {
                    _selectedtelescopeIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        public object SelectedTelescope
        {
            get { return _selectedtelescope; }
            set
            {
                if (value != _selectedtelescope)
                {
                    _selectedtelescope = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> TelescopeNames
        {
            get { return _telescopeNames; }
            set
            {
                if (value != _telescopeNames)
                {
                    _telescopeNames = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool DomeSyncErrorState
        {
            get { return _domeSyncErrorState; }
            set
            {
                if (value != _domeSyncErrorState)
                {
                    _domeSyncErrorState = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ShutterCommandAction
        {
            get { return _shutterCommandAction; }
            set
            {
                if (value != _shutterCommandAction)
                {
                    _shutterCommandAction = value;
                    OnPropertyChanged();
                }
            }
        }

        public DomeSlewAmounts SlewAmounts
        {
            get { return _slewAmounts; }
            set
            {
                if (value != _slewAmounts)
                {
                    _slewAmounts = value;
                    OnPropertyChanged();
                }
            }
        }

        public JogAmount SelectedSlewAmount
        {
            get { return _selectedSlewAmount; }
            set
            {
                if (value != _selectedSlewAmount)
                {
                    _selectedSlewAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSlaved
        {
            get { return Globals.IsDomeSlaved; }
            set
            {
                if (value != Globals.IsDomeSlaved)
                {
                    Globals.IsDomeSlaved = value;
                    OnPropertyChanged();

                    // Reset the dome sync error state whenever the slaved checkbox changes
                    DomeSyncErrorState = false;
                    Globals.DomeSyncError = false;
                }
            }
        }

        public DevHubDomeStatus Status
        {
            get { return _status; }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged();
                    RelayCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsSlewing
        {
            get { return _isSlewing; }
            set
            {
                if (value != _isSlewing)
                {
                    _isSlewing = value;
                    OnPropertyChanged();
                }
            }
        }

        public double DirectTargetAzimuth
        {
            get { return _directTargetAzimuth; }
            set
            {
                if (value != _directTargetAzimuth)
                {
                    _directTargetAzimuth = value;
                    OnPropertyChanged();
                }
            }
        }

        public double SyncTargetAzimuth
        {
            get { return _syncTargetAzimuth; }
            set
            {
                if (value != _syncTargetAzimuth)
                {
                    _syncTargetAzimuth = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool UseOneAxisSlaveCalculation
        {
            get { return Globals.UseOneAxisDomeSlaveCalculation; }
            set
            {
                if (value != Globals.UseOneAxisDomeSlaveCalculation)
                {
                    // Update the global Revised setting 
                    LogAppMessage($"Setting UseOneAxisDomeSlaveCalculation to {value}");
                    Globals.UseOneAxisDomeSlaveCalculation = value;

                    // Save the value in the registry
                    SetRegistryValue(REGISTRY_PATH_ONE_AXIS, value);

                    OnPropertyChanged();
                }
            }
        }

        public bool UsePOTHSlaveCalculation
        {
            get { return Globals.UsePOTHDomeSlaveCalculation; }
            set
            {
                if (value != Globals.UsePOTHDomeSlaveCalculation)
                {
                    LogAppMessage($"Setting UsePOTHSlaveCalculation to {value}");
                    Globals.UsePOTHDomeSlaveCalculation = value;

                    // Save the value in the registry
                    SetRegistryValue(REGISTRY_PATH_POTH, value);

                    OnPropertyChanged();
                }
            }
        }

        public double DomeAzimuthAdjustment
        {
            get { return Globals.DomeAzimuthAdjustment; }
            set
            {
                if (value != Globals.DomeAzimuthAdjustment)
                {
                    Globals.DomeAzimuthAdjustment = value;
                    OnPropertyChanged();
                }
            }
        }

        public DomeCapabilities Capabilities
        {
            get { return _capabilities; }
            set
            {
                if (value != _capabilities)
                {
                    _capabilities = value;
                    OnPropertyChanged();
                }
            }
        }

        public DomeParameters Parameters
        {
            get { return _parameters; }
            set
            {
                if (value != _parameters)
                {
                    _parameters = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Change Notification Properties

        #region Helper Methods

        /// <summary>
        /// Retrieves the value of the specified registry key, returning <see langword="false"/> if the key does not exist.
        /// </summary>
        /// <param name="key">The name of the registry key to retrieve.</param>
        /// <returns>The value of the registry key and <see langword="false"/> if it does not.</returns>
        private bool GetRegistryValueDefaultFalse(string key)
        {
            return GetRegistryValue(key, false);
        }

        /// <summary>
        /// Retrieves the value of the specified registry key, returning <see langword="true"/> if the key does not exist.
        /// </summary>
        /// <param name="key">The name of the registry key to retrieve.</param>
        /// <returns>The value of the registry key and <see langword="true"/> if it does not.</returns>
        private bool GetRegistryValueDefaultTrue(string key)
        {
            return GetRegistryValue(key, true);
        }

        /// <summary>
        /// Retrieves a boolean value from the registry for the specified key. If the key does not exist, the specified
        /// default value is used, and the key is created with this value.
        /// </summary>
        /// <remarks>This method accesses the registry under the current user's hive at the predefined
        /// path. If the key is not found, it creates the key with the specified default value.</remarks>
        /// <param name="key">The name of the registry key to retrieve.</param>
        /// <param name="defaultValue">The default value to use and set if the key does not exist.</param>
        /// <returns><see langword="true"/> if the registry value exists and is set to 1; otherwise, the specified default value.</returns>
        private bool GetRegistryValue(string key, bool defaultValue)
        {
            using (var regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(REGISTRY_PATH))
            {
                if (regKey != null)
                {
                    object value = regKey.GetValue(key);
                    if (value != null && value is int intValue)
                    {
                        return intValue == 1;
                    }
                }
            }

            // Set to default value and return this if not found
            SetRegistryValue(key, defaultValue);
            return defaultValue;
        }

        private void SetRegistryValue(string key, bool value)
        {
            using (var regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(REGISTRY_PATH))
            {
                if (regKey != null)
                {
                    regKey.SetValue(key, value ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord);
                }
            }
        }

        private static ObservableCollection<string> GetMultipleTelescopeNames(DomeLayoutSettings domeLayoutSettings)
        {
            ObservableCollection<string> telescopeNames = new ObservableCollection<string>();

            if (!string.IsNullOrEmpty(domeLayoutSettings.TelescopeName0.Trim()))
                telescopeNames.Add(domeLayoutSettings.TelescopeName0);
            if (!string.IsNullOrEmpty(domeLayoutSettings.TelescopeName1.Trim()))
                telescopeNames.Add(domeLayoutSettings.TelescopeName1);
            if (!string.IsNullOrEmpty(domeLayoutSettings.TelescopeName2.Trim()))
                telescopeNames.Add(domeLayoutSettings.TelescopeName2);
            if (!string.IsNullOrEmpty(domeLayoutSettings.TelescopeName3.Trim()))
                telescopeNames.Add(domeLayoutSettings.TelescopeName3);
            if (!string.IsNullOrEmpty(domeLayoutSettings.TelescopeName4.Trim()))
                telescopeNames.Add(domeLayoutSettings.TelescopeName4);

            return telescopeNames;
        }

        private void RegisterStatusUpdateMessage(bool regUnreg)
        {
            if (regUnreg)
            {
                Messenger.Default.Register<DomeStatusUpdatedMessage>(this, (action) => UpdateStatus(action));
            }
            else
            {
                Messenger.Default.Unregister<DomeStatusUpdatedMessage>(this);
            }
        }

        #endregion Helper Methods

        #region Event Handlers

        private void UpdateStatus(DomeStatusUpdatedMessage action)
        {
            // This is a registered message handler. It could be called from a worker thread
            // and we need to be sure that the work is done on the U/I thread.

            Task.Factory.StartNew(() =>
            {
                Status = action.Status;
                IsSlewing = Status.Slewing;

                if (!Capabilities.CanSetShutter || Status.ShutterStatus == ShutterState.shutterClosed)
                {
                    ShutterCommandAction = "Open Shutter";
                }
                else if (Status.ShutterStatus == ShutterState.shutterOpen || Status.ShutterStatus == ShutterState.shutterError)
                {
                    ShutterCommandAction = "Close Shutter";
                }
                else if (Status.ShutterStatus == ShutterState.shutterOpening)
                {
                    ShutterCommandAction = "Open In Progress";
                }
                else
                {
                    ShutterCommandAction = "Close In Progress";
                }
            }, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext);
        }

        private void InvalidateDeviceData(DeviceDisconnectedMessage action)
        {
            if (action.DeviceType == DeviceTypeEnum.Dome)
            {
                Task.Factory.StartNew(() =>
                {
                    Status = null;
                    Capabilities = null;
                    Parameters = null;
                    IsSlaved = false;
                }, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext);
            }
        }

        private void DomeParametersUpdated(DomeParametersUpdatedMessage action)
        {
            // Make sure that we update the Parameters on the U/I thread.

            Task.Factory.StartNew(() => Parameters = action.Parameters, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext);
        }

        private void DomeCapabilitiesUpdated(DomeCapabilitiesUpdatedMessage action)
        {
            // Make sure that we update the Capabilities on the U/I thread.

            Task.Factory.StartNew(() => Capabilities = action.Capabilities, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext);
        }

        private void ChangeSlavedState(DomeSlavedChangedMessage action)
        {
            // Make sure that we update the IsSlaved property on the U/I thread.

            Task.Factory.StartNew(() => IsSlaved = action.State, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext);
        }

        private void DomeLayoutSettingsChanged(DomeLayoutSettingsChangedMessage action)
        {
            // Make sure that we update the Capabilities on the U/I thread.
            Task.Factory.StartNew(() => SupportMultipleTelescopes = action.Settings.SupportMultipleTelescopes, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext);
            Task.Factory.StartNew(() => SelectedTelescope = action.Settings.ProfileIndex, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext);
            Task.Factory.StartNew(() => TelescopeNames = GetMultipleTelescopeNames(DomeSettings.FromProfile().DomeLayoutSettings), CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext);
        }

        private void DomeSyncErrorStateUpdated(DomeSyncErrorStateMessage action)
        {
            LogAppMessage($"Received DomeSyncErrorStateMessage: {action.State}");
            DomeSyncErrorState = action.State;
        }

        #endregion Event Handlers

        #region Relay Commands

        #region ToggleShutterStateCommand

        private ICommand _toggleShutterStateCommand;

        public ICommand ToggleShutterStateCommand
        {
            get
            {
                if (_toggleShutterStateCommand == null)
                {
                    _toggleShutterStateCommand = new RelayCommand(
                        param => this.ToggleShutterState(),
                        param => this.CanToggleShutterState());
                }

                return _toggleShutterStateCommand;
            }
        }

        private bool ToggleShutterState()
        {
            bool retval = true;

            ShutterState state = Status.ShutterStatus;

            switch (state)
            {
                case ShutterState.shutterOpen:
                case ShutterState.shutterOpening:
                case ShutterState.shutterError:
                    try
                    {
                        DomeManager.CloseDomeShutter();
                    }
                    catch (Exception xcp)
                    {
                        string msg = "The dome driver returned an error when attempting to close the shutter. "
                            + $"Details follow:\r\n\r\n{xcp}";
                        ShowMessage(msg, "Dome Driver Error");

                        retval = false;
                    }

                    break;

                case ShutterState.shutterClosing:
                case ShutterState.shutterClosed:
                    try
                    {
                        DomeManager.OpenDomeShutter();
                    }
                    catch (Exception xcp)
                    {
                        string msg = "The dome driver returned an error when attempting to open the shutter. "
                            + $"Details follow:\r\n\r\n{xcp}";
                        ShowMessage(msg, "Dome Driver Error");

                        retval = false;
                    }

                    break;
            }

            return retval;  // We only care about the return value when unit testing.
        }

        private bool CanToggleShutterState()
        {
            bool retval = false;

            if (Status != null && Capabilities != null
                && Status.Connected && !Status.Slewing
                && Capabilities.CanSetShutter)
            {
                retval = Status.ShutterStatus != ShutterState.shutterClosing && Status.ShutterStatus != ShutterState.shutterOpening;
            }

            return retval; // We only care about the return value when unit testing.
        }

        #endregion

        #region ParkDomeCommand

        private ICommand _parkDomeCommand;

        public ICommand ParkDomeCommand
        {
            get
            {
                if (_parkDomeCommand == null)
                {
                    _parkDomeCommand = new RelayCommand(
                        param => this.ParkDome(),
                        param => this.CanParkDome());
                }

                return _parkDomeCommand;
            }
        }

        private bool ParkDome()
        {
            bool retval = true;

            try
            {
                DomeManager.ParkTheDome();
            }
            catch (Exception xcp)
            {
                string msg = "The dome driver returned an error when attempting to park the dome. "
                    + $"Details follow:\r\n\r\n{xcp}";
                ShowMessage(msg, "Dome Driver Error");

                retval = false;
            }

            return retval;  // We only care about the return value when unit testing.
        }

        private bool CanParkDome()
        {
            bool retval = false;

            if (Status != null && Status.Connected && Capabilities != null && !IsSlaved && !Status.Slewing)
            {
                if (Capabilities.CanPark && !Status.AtPark)
                {
                    retval = true;
                }
            }

            return retval;
        }

        #endregion

        #region JogAltitudeCommand

        private ICommand _jogAltitudeCommand;

        public ICommand JogAltitudeCommand
        {
            get
            {
                if (_jogAltitudeCommand == null)
                {
                    _jogAltitudeCommand = new RelayCommand(
                        param => this.JogAltitude(param),
                        param => this.CanJogAltitude());
                }

                return _jogAltitudeCommand;
            }
        }

        private bool JogAltitude(object param)
        {
            bool retval = true;

            MoveDirections direction = (MoveDirections)param;
            bool moveValid = (direction == MoveDirections.Up || direction == MoveDirections.Down);

            if (!moveValid)
            {
                return retval;
            }

            double newAltitude = Double.NaN;

            if (direction == MoveDirections.Up)
            {
                newAltitude = Status.Altitude + SelectedSlewAmount.Amount;
                newAltitude = Math.Min(newAltitude, 90.0); // Clamp to the zenith
            }
            else if (direction == MoveDirections.Down)
            {
                newAltitude = Status.Altitude - SelectedSlewAmount.Amount;
                newAltitude = Math.Max(newAltitude, 0.0); // Clamp to the horizon
            }

            if (!Double.IsNaN(newAltitude))
            {
                try
                {
                    DomeManager.SlewDomeShutter(newAltitude);
                }
                catch (Exception xcp)
                {
                    string msg = "The dome driver returned an error when attempting to slew the shutter. "
                        + $"Details follow:\r\n\r\n{xcp}";
                    ShowMessage(msg, "Dome Driver Error");

                    retval = false;
                }
            }

            return retval; // We only care about the return value when unit testing.
        }

        private bool CanJogAltitude()
        {
            bool retval = false;

            if (Status != null && Capabilities != null
                && Status.Connected && !Status.Slewing && Capabilities.CanSetAltitude)
            {
                retval = Status.ShutterStatus == ShutterState.shutterOpen;
            }

            return retval;
        }

        #endregion JogAltitudeCommand

        #region JogAzimuthCommand

        private ICommand _jogAzimuthCommand;

        public ICommand JogAzimuthCommand
        {
            get
            {
                if (_jogAzimuthCommand == null)
                {
                    _jogAzimuthCommand = new RelayCommand(
                        param => this.JogAzimuth(param),
                        param => this.CanJogAzimuth());
                }

                return _jogAzimuthCommand;
            }
        }

        private bool JogAzimuth(object param)
        {
            bool retval = true;

            MoveDirections direction = (MoveDirections)param;
            bool moveValid = (direction == MoveDirections.Clockwise || direction == MoveDirections.CounterClockwise);

            if (!moveValid)
            {
                return retval;
            }

            double newAzimuth = Double.NaN;

            if (direction == MoveDirections.Clockwise)
            {
                newAzimuth = Status.Azimuth + SelectedSlewAmount.Amount;

                if (newAzimuth >= 360.0)
                {
                    newAzimuth -= 360.0;
                }
            }
            else if (direction == MoveDirections.CounterClockwise)
            {
                newAzimuth = Status.Azimuth - SelectedSlewAmount.Amount;

                if (newAzimuth < 0.0)
                {
                    newAzimuth += 360.0;
                }
            }

            if (!Double.IsNaN(newAzimuth))
            {
                try
                {
                    DomeManager.SlewDomeToAzimuth(newAzimuth);
                }
                catch (Exception xcp)
                {
                    string msg = "The dome driver returned an error when attempting to rotate the dome. "
                        + $"Details follow:\r\n\r\n{xcp}";
                    ShowMessage(msg, "Dome Driver Error");

                    retval = false;
                }
            }

            return retval; // We only care about the return value when unit testing.
        }

        private bool CanJogAzimuth()
        {
            bool retval = false;

            if (Status != null && Capabilities != null && Status.Connected && !Status.Slewing)
            {
                retval = Capabilities.CanSetAzimuth;
            }

            return retval;
        }

        #endregion JogAzimuthCommand

        #region StopMotionCommand

        private ICommand _stopMotionCommand;

        public ICommand StopMotionCommand
        {
            get
            {
                if (_stopMotionCommand == null)
                {
                    _stopMotionCommand = new RelayCommand(
                        param => this.StopMotion());
                }

                return _stopMotionCommand;
            }
        }

        private bool StopMotion()
        {
            bool retval = true;

            try
            {
                DomeManager.StopDomeMotion();
            }
            catch (Exception xcp)
            {
                string msg = "The dome driver returned an error when attempting to stop all motion. "
                    + $"Details follow:\r\n\r\n{xcp}";
                ShowMessage(msg, "Dome Driver Error");

                retval = false;
            }

            return retval; // We only care about the return value when unit testing.
        }

        #endregion

        #region GotoDirectAzimuthCommand

        private ICommand _gotoDirectAzimuthCommand;

        public ICommand GotoDirectAzimuthCommand
        {
            get
            {
                if (_gotoDirectAzimuthCommand == null)
                {
                    _gotoDirectAzimuthCommand = new RelayCommand(
                        param => this.GotoDirectAzimuth(),
                        param => this.CanGotoDirectAzimuth());
                }

                return _gotoDirectAzimuthCommand;
            }
        }

        private bool GotoDirectAzimuth()
        {
            bool retVal = true;

            try
            {
                DomeManager.SlewDomeToAzimuth(DirectTargetAzimuth);
            }
            catch (Exception xcp)
            {
                string msg = "The dome driver returned an error when attempting to rotate the dome. "
                    + $"Details follow:\r\n\r\n{xcp}";
                ShowMessage(msg, "Dome Driver Error");

                retVal = false;
            }

            return retVal;  // We only care about the return value when unit testing.
        }

        private bool CanGotoDirectAzimuth()
        {
            bool retval = false;

            if (Status != null && Capabilities != null && Status.Connected && !Status.Slewing
                && !Double.IsNaN(DirectTargetAzimuth))
            {
                retval = Capabilities.CanSetAzimuth;
            }

            return retval;
        }

        #endregion

        #region SyncAzimuthCommand

        private ICommand _syncAzimuthCommand;

        public ICommand SyncAzimuthCommand
        {
            get
            {
                if (_syncAzimuthCommand == null)
                {
                    _syncAzimuthCommand = new RelayCommand(
                        param => this.SyncAzimuth(),
                        param => this.CanSyncAzimuth());
                }

                return _syncAzimuthCommand;
            }
        }

        private bool SyncAzimuth()
        {
            bool retval = true;

            try
            {
                DomeManager.SyncDomeToAzimuth(SyncTargetAzimuth);
            }
            catch (Exception xcp)
            {
                string msg = "The dome driver returned an error when attempting to synchronize the azimuth. "
                    + $"Details follow:\r\n\r\n{xcp}";
                ShowMessage(msg, "Dome Driver Error");

                retval = false;
            }

            return retval;  // We only care about the return value when unit testing.
        }

        private bool CanSyncAzimuth()
        {
            bool retval = false;

            if (Status != null && Capabilities != null && Status.Connected && !Status.Slewing
                && !IsSlaved && !Double.IsNaN(SyncTargetAzimuth))
            {
                retval = Capabilities.CanSyncAzimuth;
            }

            return retval;
        }

        #endregion

        #region FindHomeCommand

        private ICommand _findHomeCommand;

        public ICommand FindHomeCommand
        {
            get
            {
                if (_findHomeCommand == null)
                {
                    _findHomeCommand = new RelayCommand(
                        param => this.FindHome(),
                        param => this.CanFindHome());
                }

                return _findHomeCommand;
            }
        }

        private bool FindHome()
        {
            bool retval = true;

            try
            {
                DomeManager.FindHomePosition();
            }
            catch (Exception xcp)
            {
                string msg = "The dome driver returned an error when attempting to find the home position. "
                    + $"Details follow:\r\n\r\n{xcp}";
                ShowMessage(msg, "Dome Driver Error");

                retval = false;
            }

            return retval; // We only care about the return value when unit testing.
        }

        private bool CanFindHome()
        {
            bool retval = false;

            if (Status != null && Capabilities != null && Status.Connected && !IsSlaved
                && !Status.Slewing && Capabilities.CanFindHome)
            {
                retval = true;
            }

            return retval;
        }

        #endregion

        #region SlaveToScopeCommand

        private ICommand _slaveToScopeCommand;

        public ICommand SlaveToScopeCommand
        {
            get
            {
                if (_slaveToScopeCommand == null)
                {
                    _slaveToScopeCommand = new RelayCommand(
                        param => this.SlaveToScope(param),
                        param => this.CanSlaveToScope());
                }

                return _slaveToScopeCommand;
            }
        }

        private void SlaveToScope(object param)
        {
            bool slaved = (bool)param;

            Globals.IsDomeSlaved = slaved;
        }

        private bool CanSlaveToScope()
        {
            bool retval = true;

            if (!Globals.IsDomeSlaved)
            {
                // Verify that we are ready to slave the dome to the scope.

                retval = DomeManager.IsScopeReadyToSlave && DomeManager.IsDomeReadyToSlave;
            }

            return retval;
        }

        #endregion

        #region TelescopeSelectionChangedCommand

        private ICommand _telescopeSelectionChangedCommand;

        public ICommand TelescopeSelectionChangedCommand
        {
            get
            {
                if (_telescopeSelectionChangedCommand == null)
                {
                    _telescopeSelectionChangedCommand = new RelayCommand(
                        param => this.TelescopeSelectionChanged((ComboBoxSelectionChangedEvent)param));
                }

                return _telescopeSelectionChangedCommand;
            }
        }

        private void TelescopeSelectionChanged(ComboBoxSelectionChangedEvent comboBoxSelection)
        {
            // Get the newly selected telescope name from the first added item
            if (comboBoxSelection.SelectionChangedEventArgs.AddedItems.Count > 0 && comboBoxSelection.SelectionChangedEventArgs.AddedItems[0] is string selectedTelescope)
            {
                SelectedTelescope = selectedTelescope;
                LogAppMessage($"Selected telescope changed to: {selectedTelescope}");
                List<TelescopeOffsets> offsets = new List<TelescopeOffsets>
                {
                    new TelescopeOffsets(Globals.DomeLayoutSettings.TelescopeName0, Globals.DomeLayoutSettings.GemAxisOffset0, Globals.DomeLayoutSettings.OpticalOffset0),
                    new TelescopeOffsets(Globals.DomeLayoutSettings.TelescopeName1, Globals.DomeLayoutSettings.GemAxisOffset1, Globals.DomeLayoutSettings.OpticalOffset1),
                    new TelescopeOffsets(Globals.DomeLayoutSettings.TelescopeName2, Globals.DomeLayoutSettings.GemAxisOffset2, Globals.DomeLayoutSettings.OpticalOffset2),
                    new TelescopeOffsets(Globals.DomeLayoutSettings.TelescopeName3, Globals.DomeLayoutSettings.GemAxisOffset3, Globals.DomeLayoutSettings.OpticalOffset3),
                    new TelescopeOffsets(Globals.DomeLayoutSettings.TelescopeName4, Globals.DomeLayoutSettings.GemAxisOffset4, Globals.DomeLayoutSettings.OpticalOffset4)
                };

                // Check whether the user operated the combo box or whether the change was generated programmatically.
                if (comboBoxSelection.ComboBox.IsDropDownOpen) // User changed the selected item in the list
                {
                    TelescopeOffsets selectedOffset = offsets[comboBoxSelection.ComboBox.SelectedIndex];

                    Globals.DomeLayoutSettings.ProfileIndex = comboBoxSelection.ComboBox.SelectedIndex;
                    Globals.DomeLayoutSettings.GemAxisOffset = selectedOffset.OffsetFromAxisIntersection;
                    Globals.DomeLayoutSettings.OpticalOffset = selectedOffset.OffsetFromDecAltAxis;

                    // Write the newly selected values to the profile
                    DomeSettings domeSettings = DomeSettings.FromProfile();
                    domeSettings.DomeLayoutSettings = Globals.DomeLayoutSettings;
                    domeSettings.ToProfile();
                }
                else // Drop-down change was programmatic
                {
                    SelectedTelescopeIndex = Globals.DomeLayoutSettings.ProfileIndex;
                }
            }
            else
            {
                LogAppMessage("No valid telescope selected.");
            }

        }

        #endregion

        #endregion Relay Commands

    }
}
