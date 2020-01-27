using System;
using System.Windows.Input;

namespace ASCOM.DeviceHub
{
	public class SetupViewModel : DeviceHubDialogViewModelBase
    {
		private string TelescopeID { get; set; }
		private string DomeID { get; set; }
		private string FocuserID { get; set; }

		public SetupViewModel()
			: base( "Device Hub Setup")
		{
			SuppressTrayBubble = Globals.SuppressTrayBubble;
			UseCustomTheme = Globals.UseCustomTheme;
			UseExpandedScreenLayout = Globals.UseExpandedScreenLayout;

			TelescopeSetupVm = new TelescopeSetupViewModel();
			DomeSetupVm = new DomeSetupViewModel();
			FocuserSetupVm = new FocuserSetupViewModel();
		}

		public TelescopeSetupViewModel TelescopeSetupVm { get; set; }
		public DomeSetupViewModel DomeSetupVm { get; set; }
		public FocuserSetupViewModel FocuserSetupVm { get; set; }


		private bool _suppressTrayBubble;

		public bool SuppressTrayBubble
		{
			get { return _suppressTrayBubble; }
			set
			{
				if ( value != _suppressTrayBubble )
				{
					_suppressTrayBubble = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _useCustomTheme;

		public bool UseCustomTheme
		{
			get { return _useCustomTheme; }
			set
			{
				if ( value != _useCustomTheme )
				{
					_useCustomTheme = value;
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

		private bool _isTelescopeActive;

		public bool IsTelescopeActive
		{
			get { return _isTelescopeActive; }
			set
			{
				if ( value != _isTelescopeActive )
				{
					_isTelescopeActive = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isDomeActive;

		public bool IsDomeActive
		{
			get { return _isDomeActive; }
			set
			{
				if ( value != _isDomeActive )
				{
					_isDomeActive = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isFocuserActive;

		public bool IsFocuserActive
		{
			get { return _isFocuserActive; }
			set
			{
				if ( value != _isFocuserActive )
				{
					_isFocuserActive = value;
					OnPropertyChanged();
				}
			}
		}

		public void ChangeActiveFunction( string functionName )
		{
			// Called from code-behind when the active tab item is changed.

			if ( functionName == "Device Hub Setup" )
			{}
			else if ( functionName == "Telescope Setup" )
			{}
			else if ( functionName == "Dome Setup" )
			{}
			else if ( functionName == "Focuser Setup" )
			{}
			else
			{
				string msg = String.Format( "SetupViewModel.ChangeActiveFunction called with invalid function Name - {0}", functionName );
				throw new ArgumentException( msg );
			}
		}

		public void InitializeCurrentTelescope( string telescopeID )
		{
			TelescopeID = telescopeID;
			TelescopeSetupVm.TelescopeID = telescopeID;
			IsTelescopeActive = TelescopeManager.Instance.IsConnected;
		}

		public void InitializeCurrentDome( string domeID )
		{
			DomeID = domeID;
			DomeSetupVm.DomeID = domeID;
			DomeSetupVm.Initialize( Globals.DomeLayout );
			IsDomeActive = DomeManager.Instance.IsConnected;
		}

		public void InitializeCurrentFocuser( string focuserID )
		{
			FocuserID = focuserID;
			FocuserSetupVm.FocuserID = focuserID;
			FocuserSetupVm.Initialize( Globals.FocuserTemperatureOffset );
			IsFocuserActive = FocuserManager.Instance.IsConnected;
		}

		protected override void DoDispose()
		{
			_setupOKCommand = null;
			_setupCancelCommand = null;

			TelescopeSetupVm.Dispose();
			TelescopeSetupVm = null;
			DomeSetupVm.Dispose();
			DomeSetupVm = null;
			FocuserSetupVm.Dispose();
			FocuserSetupVm = null;
		}

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
			AppSettingsManager.SaveAppSettings();
		}

		private void SaveTelescopeSettings()
		{
			// Read the current settings and update them with the changes
			// to preserve the logging flag.

			TelescopeSettings settings = TelescopeSettings.FromProfile();
			settings.TelescopeID = TelescopeManager.TelescopeID;
			settings.ToProfile();
		}

		private void SaveDomeSettings()
		{
			// Read the current settings and update them with the changes
			// to preserve the logging flag.

			DomeSettings settings = DomeSettings.FromProfile();
			settings.DomeID = DomeManager.DomeID;
			settings.DomeLayout = Globals.DomeLayout;
			settings.ToProfile();
		}

		private void SaveFocuserSettings()
		{
			FocuserSettings settings = FocuserSettings.FromProfile();
			settings.FocuserID = FocuserManager.FocuserID;
			settings.TemperatureOffset = Globals.FocuserTemperatureOffset;
			settings.ToProfile();
		}

		#region Relay Commands

		#region SetupOKCommand

		private ICommand _setupOKCommand;

		public ICommand SetupOKCommand
		{
			get
			{
				if ( _setupOKCommand == null )
				{
					_setupOKCommand = new RelayCommand(
						param => this.SetupOK() );
				}

				return _setupOKCommand;
			}
		}

		private void SetupOK()
		{
			if ( !IsTelescopeActive && TelescopeID != TelescopeSetupVm.TelescopeID )
			{
				TelescopeID = TelescopeSetupVm.TelescopeID;
				TelescopeManager.SetTelescopeID( TelescopeID );
			}

			if ( !IsDomeActive && DomeID != DomeSetupVm.DomeID )
			{
				DomeID = DomeSetupVm.DomeID;
				DomeManager.SetDomeID( DomeID );
			}

			Globals.DomeLayout = DomeSetupVm.GetLayout();

			if ( !IsFocuserActive && FocuserID != FocuserSetupVm.FocuserID )
			{
				FocuserID = FocuserSetupVm.FocuserID;
				FocuserManager.SetFocuserID( FocuserID );
			}

			Globals.SuppressTrayBubble = SuppressTrayBubble;
			Globals.UseCustomTheme = UseCustomTheme;
			Globals.FocuserTemperatureOffset = FocuserSetupVm.TemperatureOffset;

			SaveSettings();

			OnRequestClose( true );
		}

		#endregion SetupOKCommand

		#region SetupCancelCommand

		private ICommand _setupCancelCommand;

		public ICommand SetupCancelCommand
		{
			get
			{
				if ( _setupCancelCommand == null )
				{
					_setupCancelCommand = new RelayCommand(
						param => this.SetupCancel() );
				}

				return _setupCancelCommand;
			}
		}

		private void SetupCancel()
		{
			OnRequestClose( false );
		}

		#endregion SetupCancelCommand

		#endregion Relay Commands
	}
}
