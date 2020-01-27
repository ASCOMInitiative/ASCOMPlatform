using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
		#region Private Properties

		private bool IsActivityLogVisible
		{
			get => ActivityLogVm.IsActive;
		}

		#endregion Private Properties

		public MainWindowViewModel()
		{
			// Inject the TelescopeManager into the telescope view model at creation so that
			// we can inject a mock manager for testing; same for the Dome and Focuser.

			TelescopeVm = new TelescopeViewModel( TelescopeManager.Instance );
			DomeVm = new DomeViewModel( DomeManager.Instance );
			FocuserVm = new FocuserViewModel( FocuserManager.Instance );
			Globals.ActiveDevice = DeviceTypeEnum.Telescope;

			string themeName = ( Globals.UseCustomTheme ) ? Strings.ThemeNameCustom : Strings.ThemeNameStandard;
			InitializeThemes( themeName );

			UseExpandedScreenLayout = Globals.UseExpandedScreenLayout;

			Messenger.Default.Register<ObjectCountMessage>( this, ( action ) => UpdateObjectsCount( action ) );
		}

		#region Public Properties

		public TelescopeViewModel TelescopeVm { get; private set; }
		public DomeViewModel DomeVm { get; private set; }
		public FocuserViewModel FocuserVm { get; private set; }
		public SetupViewModel SetupVm { get; set; }
		public ActivityLogViewModel ActivityLogVm { get; set; }
		public List<Theme> Themes { get; set; }

		#endregion Public Properties

		#region Change Notification Properties

		private int _objectCount;

		public int ObjectCount
		{
			get { return _objectCount; }
			set
			{
				if ( value != _objectCount )
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
				if ( value != _hasActiveClients )
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
				if ( value != _useExpandedScreenLayout )
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

		private void InitializeThemes( string themeName )
		{
			// Initialize the visual themes.

			if ( ThemeManager.ThemeList == null )
			{
				ThemeManager.InitializeThemes( null );
			}

			Themes = new List<Theme>( ThemeManager.ThemeList );

			ChangeTheme( themeName );
		}

		private void ChangeTheme( string themeName )
		{
			if ( Themes == null )
			{
				throw new InvalidOperationException( "Themes have not been initialized!!!" );
			}

			// Don't change the theme if we can't find the requested theme to change to.

			Theme newTheme = Themes.Where( t => t.Name.Equals( themeName, StringComparison.CurrentCultureIgnoreCase ) ).FirstOrDefault();

			if ( newTheme == null )
			{
				return;
			}

			// Remove each resource dictionary for the current theme.

			Theme currentTheme = Themes.Where( t => t.IsSelected ).FirstOrDefault();

			if ( currentTheme != null )
			{
				Application.Current.Resources.MergedDictionaries.Remove( currentTheme.Resource );
				currentTheme.IsSelected = false;
			}

			// Add the resource dictionary from the new theme to the applications's merged dictionaries.

			Application.Current.Resources.MergedDictionaries.Add( newTheme.Resource );
			newTheme.IsSelected = true;
		}

		protected override void DoDispose()
		{
			Messenger.Default.Unregister<ObjectCountMessage>( this );

			_showSetupCommand = null;
			_showLogCommand = null;
			TelescopeVm.Dispose();
			TelescopeVm = null;
			DomeVm.Dispose();
			DomeVm = null;
			FocuserVm.Dispose();
			FocuserVm = null;

			if ( SetupVm != null )
			{
				SetupVm.Dispose();
				SetupVm = null;
			}

			if ( ActivityLogVm != null && ActivityLogVm.IsActive )
			{
				ActivityLogVm.CloseLog();

				if ( ActivityLogVm != null )
				{
					ActivityLogVm.Dispose();
					ActivityLogVm = null;
				}
			}
		}

		private void UpdateObjectsCount( ObjectCountMessage msg )
		{
			Task.Factory.StartNew( () =>
			{
				ObjectCount = msg.ScopeCount + msg.DomeCount + msg.FocuserCount;
				HasActiveClients = ObjectCount > 0;
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		#endregion Helper Methods

		#region Relay Commands

		#region ShowSetupCommand

		private ICommand _showSetupCommand;

		public ICommand ShowSetupCommand
		{
			get
			{
				if ( _showSetupCommand == null )
				{
					_showSetupCommand = new RelayCommand(
						param => this.ShowSetup() );
				}

				return _showSetupCommand;
			}
		}

		private void ShowSetup()
		{
			SetupVm = new SetupViewModel();
			SetupVm.InitializeCurrentTelescope( TelescopeManager.TelescopeID );
			SetupVm.InitializeCurrentDome( DomeManager.DomeID );
			SetupVm.InitializeCurrentFocuser( FocuserManager.FocuserID );
			SetupVm.UseExpandedScreenLayout = UseExpandedScreenLayout;

			IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
			bool? result = svc.ShowDialog( SetupVm );

			if ( result.HasValue && result.Value )
			{
				Globals.UseExpandedScreenLayout = SetupVm.UseExpandedScreenLayout;
				AppSettingsManager.SaveAppSettings();
			}

			SetupVm.Dispose();
			SetupVm = null;

			string themeName = ( Globals.UseCustomTheme ) ? Strings.ThemeNameCustom : Strings.ThemeNameStandard;
			ChangeTheme( themeName );

			UseExpandedScreenLayout = Globals.UseExpandedScreenLayout;
		}

		#endregion ShowSetupCommand

		#region ShowLogCommand

		private ICommand _showLogCommand;

		public ICommand ShowLogCommand
		{
			get
			{
				if ( _showLogCommand == null )
				{
					_showLogCommand = new RelayCommand(
						param => this.ShowLog(),
						param => this.CanShowLog() );
				}

				return _showLogCommand;
			}
		}

		private void ShowLog()
		{
			Messenger.Default.Register<ActivityLogClosedMessage>( this, ( action ) => ActivityLogClosed() );
			ActivityLogVm = new ActivityLogViewModel();
			IActivityLogService svc = ServiceContainer.Instance.GetService<IActivityLogService>();
			svc.Show( ActivityLogVm, Globals.ActivityWindowLeft, Globals.ActivityWindowTop
						, Globals.ActivityWindowWidth, Globals.ActivityWindowHeight );
		}

		private void ActivityLogClosed()
		{
			ActivityLogVm = null;

			Messenger.Default.Unregister<ActivityLogClosedMessage>( this );
		}

		private bool CanShowLog()
		{
			return ( ActivityLogVm == null );
		}

		#endregion ShowLogCommand

		#region AboutCommand

		private ICommand _showAboutCommand;

		public ICommand ShowAboutCommand
		{
			get
			{
				if ( _showAboutCommand == null )
				{
					_showAboutCommand = new RelayCommand(
						param => this.ShowAbout() );
				}

				return _showAboutCommand;
			}
		}

		private void ShowAbout()
		{
			var aboutVm = new AboutViewModel();

			IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
			svc.ShowDialog( aboutVm );

			aboutVm.Dispose();
			aboutVm = null;
		}

		#endregion AboutCommand

		#region ViewHelpCommand

		private ICommand _viewHelpCommand;

		public ICommand ViewHelpCommand
		{
			get
			{
				if ( _viewHelpCommand == null )
				{
					_viewHelpCommand = new RelayCommand(
						param => this.ViewHelp() );
				}

				return _viewHelpCommand;
			}
		}

		private void ViewHelp()
		{
			string folderPath = Directory.GetCurrentDirectory();
			string helpPath = Path.Combine( folderPath, Properties.Settings.Default.HelpFileName );
			Process.Start( helpPath );
		}

		#endregion


		#endregion Relay Commands
	}
}
