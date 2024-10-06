using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using ASCOM.DeviceHub.MvvmMessenger;
using ASCOM.DeviceHub.Resources;

namespace ASCOM.DeviceHub
{
    public class MainWindowViewModel : DeviceHubViewModelBase
    {
        private IActivityLogService ActivityLogService { get; set; }

        public MainWindowViewModel()
        {
            string caller = "MainWindowViewModel ctor";
            AssemblyName asblyName = Assembly.GetExecutingAssembly().GetName();
            string name = asblyName.FullName;
            ActivityLogService = ServiceContainer.Instance.GetService<IActivityLogService>();

            LogAppMessage($"Starting application initialization of {asblyName}.", caller);

            // Inject the TelescopeManager into the telescope view model at creation so that
            // we can inject a mock manager for testing; same for the Dome and Focuser.

            LogAppMessage("============================== Telescope Initialization ============================== ", caller);
            LogAppMessage("Creating the TelescopeManager", caller);
            string className = "TelescopeManager";

            try
            {
                TelescopeManager tm = TelescopeManager.Instance;

                className = "TelescopeViewModel";
                LogAppMessage($"Creating the {className}", caller);
                TelescopeVm = new TelescopeViewModel(tm);

                LogAppMessage("============================== Dome Initialization ============================== ", caller);
                className = "DomeManager";
                LogAppMessage($"Creating the {className}", caller);
                DomeManager dm = DomeManager.Instance;

                className = "DomeViewModel";
                LogAppMessage($"Creating the {className}", caller);
                DomeVm = new DomeViewModel(dm);

                LogAppMessage("============================== Focuser Initialization ============================== ", caller);
                className = "FocuserManager";
                LogAppMessage($"Creating the {className}", caller);
                FocuserManager fm = FocuserManager.Instance;

                className = "FocuserViewModel";
                LogAppMessage($"Creating the {className}", caller);
                FocuserVm = new FocuserViewModel(fm);

                LogAppMessage("ViewModel and device manager creation complete.", caller);
            }
            catch (Exception xcp)
            {
                // Log the exception in the application log.

                StringBuilder sb = new StringBuilder($"Caught an exception while creating an instance of the {className} class.");
                sb.Append(" Details follow:\r\n\r\n");
                sb.Append(xcp.ToString());
                LogAppMessage(sb.ToString());

                // Tell the user what happened.

                sb = new StringBuilder($"Device Hub failed to initialize the {className} class and must shut down.\r\n");
                sb.Append($"Please post on the ASCOM Talk Groups.Io forum for assistance.\r\n\r\n{xcp}");
                ShowMessage(sb.ToString(), "Fatal Device Hub Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);

                throw;
            }

            LogAppMessage("Setting the active device to Telescope", caller);

            Globals.ActiveDevice = DeviceTypeEnum.Telescope;

            LogAppMessage("Initializing the visual theme.", caller);

            string themeName = (Globals.UseCustomTheme) ? Strings.ThemeNameCustom : Strings.ThemeNameStandard;
            InitializeThemes(themeName);

            UseExpandedScreenLayout = Globals.UseExpandedScreenLayout;

            LogAppMessage("Registering message handlers", caller);

            Messenger.Default.Register<ObjectCountMessage>(this, (action) => UpdateObjectsCount(action));

            LogAppMessage("Application Initializaiton is complete.", caller);
            Globals.CloseAppLogger();
        }

        #region Public Properties

        public TelescopeViewModel TelescopeVm { get; private set; }
        public DomeViewModel DomeVm { get; private set; }
        public FocuserViewModel FocuserVm { get; private set; }
        public SetupViewModel SetupVm { get; set; }
        public List<Theme> Themes { get; set; }

        #endregion Public Properties

        #region Change Notification Properties

        private int _objectCount;

        public int ObjectCount
        {
            get { return _objectCount; }
            set
            {
                if (value != _objectCount)
                {
                    _objectCount = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _hasActiveClients;

        public bool HasActiveClients
        {
            get { return _hasActiveClients; }
            set
            {
                if (value != _hasActiveClients)
                {
                    _hasActiveClients = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _useExpandedScreenLayout;

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

        #endregion Change Notification Properties

        #region Public Methods

        #endregion

        #region Helper Methods

        private void InitializeThemes(string themeName)
        {
            // Initialize the visual themes.

            if (ThemeManager.ThemeList == null)
            {
                ThemeManager.InitializeThemes(null);
            }

            Themes = new List<Theme>(ThemeManager.ThemeList);

            ChangeTheme(themeName);
        }

        private void ChangeTheme(string themeName)
        {
            if (Themes == null)
            {
                throw new InvalidOperationException("Themes have not been initialized!!!");
            }

            // Don't change the theme if we can't find the requested theme to change to.

            Theme newTheme = Themes.Where(t => t.Name.Equals(themeName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (newTheme == null)
            {
                return;
            }

            // Remove each resource dictionary for the current theme.

            Theme currentTheme = Themes.Where(t => t.IsSelected).FirstOrDefault();

            if (currentTheme != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(currentTheme.Resource);
                currentTheme.IsSelected = false;
            }

            // Add the resource dictionary from the new theme to the applications's merged dictionaries.

            Application.Current.Resources.MergedDictionaries.Add(newTheme.Resource);
            newTheme.IsSelected = true;
        }

        protected override void DoDispose()
        {
            Messenger.Default.Unregister<ObjectCountMessage>(this);

            _showSetupCommand = null;
            _showLogCommand = null;

            if (TelescopeVm != null)
            {
                TelescopeVm.Dispose();
                TelescopeVm = null;
            }

            if (DomeVm != null)
            {
                DomeVm.Dispose();
                DomeVm = null;
            }

            if (FocuserVm != null)
            {
                FocuserVm.Dispose();
                FocuserVm = null;
            }

            if (SetupVm != null)
            {
                SetupVm.Dispose();
                SetupVm = null;
            }
        }

        private void UpdateObjectsCount(ObjectCountMessage msg)
        {
            Task.Factory.StartNew(() =>
            {
                ObjectCount = msg.ScopeCount + msg.DomeCount + msg.FocuserCount;
                HasActiveClients = ObjectCount > 0;
            }, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext);
        }

        #endregion Helper Methods

        #region Relay Commands

        #region ShowSetupCommand

        private ICommand _showSetupCommand;

        public ICommand ShowSetupCommand
        {
            get
            {
                if (_showSetupCommand == null)
                {
                    _showSetupCommand = new RelayCommand(
                        param => this.ShowSetup());
                }

                return _showSetupCommand;
            }
        }

        private void ShowSetup()
        {
            SetupVm = new SetupViewModel();
            SetupVm.InitializeCurrentTelescope(TelescopeManager.TelescopeID, TelescopeManager.Instance.FastPollingPeriod);
            SetupVm.InitializeCurrentDome(DomeManager.DomeID, DomeManager.Instance.FastPollingPeriod);
            SetupVm.InitializeCurrentFocuser(FocuserManager.FocuserID, FocuserManager.Instance.FastPollingPeriod);
            SetupVm.UseExpandedScreenLayout = UseExpandedScreenLayout;

            IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
            bool? result = svc.ShowDialog(SetupVm);

            if (result.HasValue && result.Value)
            {
                Globals.UseExpandedScreenLayout = SetupVm.UseExpandedScreenLayout;
                Globals.AlwaysOnTop = SetupVm.KeepWindowOnTop;
                AppSettingsManager.SaveAppSettings();
            }

            SetupVm.Dispose();
            SetupVm = null;

            string themeName = (Globals.UseCustomTheme) ? Strings.ThemeNameCustom : Strings.ThemeNameStandard;
            ChangeTheme(themeName);

            UseExpandedScreenLayout = Globals.UseExpandedScreenLayout;
        }

        #endregion ShowSetupCommand

        #region ShowLogCommand

        private ICommand _showLogCommand;

        public ICommand ShowLogCommand
        {
            get
            {
                if (_showLogCommand == null)
                {
                    _showLogCommand = new RelayCommand(
                        param => this.ShowLog(),
                        param => true);
                }

                return _showLogCommand;
            }
        }

        private void ShowLog()
        {
            ActivityLogService.ShowActivityLog();
        }

        #endregion ShowLogCommand

        #region AboutCommand

        private ICommand _showAboutCommand;

        public ICommand ShowAboutCommand
        {
            get
            {
                if (_showAboutCommand == null)
                {
                    _showAboutCommand = new RelayCommand(
                        param => this.ShowAbout());
                }

                return _showAboutCommand;
            }
        }

        private void ShowAbout()
        {
            var aboutVm = new AboutViewModel();

            IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
            svc.ShowDialog(aboutVm);

            aboutVm.Dispose();
        }

        #endregion AboutCommand

        #region ViewHelpCommand

        private ICommand _viewHelpCommand;

        public ICommand ViewHelpCommand
        {
            get
            {
                if (_viewHelpCommand == null)
                {
                    _viewHelpCommand = new RelayCommand(
                        param => this.ViewHelp());
                }

                return _viewHelpCommand;
            }
        }

        private void ViewHelp()
        {
            // Get the full file name of the assembly containing the main application window
            string fullPath = typeof(MainWindow).Assembly.Location;

            // Extract the path of the folder containing the assembly
            string folderPath = Path.GetDirectoryName(fullPath);

            // Construct the full file name of the help file and display it
            string helpPath = Path.Combine(folderPath, Properties.Settings.Default.HelpFileName);
            Process.Start(helpPath);
        }

        #endregion


        #endregion Relay Commands
    }
}
