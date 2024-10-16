﻿using ASCOM.DeviceHub.MvvmMessenger;
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
			KeepWindowOnTop = Globals.AlwaysOnTop;
			UseCompositeSlewingFlag = Globals.UseCompositeSlewingFlag;
			ShowActivityLogWhenStarted = Globals.ShowActivityLogWhenStarted;
			WriteActivityLogToDisk=Globals.WriteLogActivityToDisk;
			TelescopeSetupVm = new TelescopeSetupViewModel();
			DomeSetupVm = new DomeSetupViewModel();
			FocuserSetupVm = new FocuserSetupViewModel();
		}

		public TelescopeSetupViewModel TelescopeSetupVm { get; set; }
		public DomeSetupViewModel DomeSetupVm { get; set; }
		public FocuserSetupViewModel FocuserSetupVm { get; set; }

        private bool _showActivityLogWhenStarted;

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

        private bool _writeActivityLogToDisk;

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

		private bool _keepWindowOnTop;

		public bool KeepWindowOnTop
		{
			get { return _keepWindowOnTop; }
			set
			{
				if ( value != _keepWindowOnTop )
				{
					_keepWindowOnTop = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _useCompositeSlewingFlag;

		public bool UseCompositeSlewingFlag
		{
			get { return _useCompositeSlewingFlag; }
			set
			{
				if ( value != _useCompositeSlewingFlag )
				{
					_useCompositeSlewingFlag = value;
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
				string msg = $"SetupViewModel.ChangeActiveFunction called with invalid function Name - {functionName}.";
				throw new ArgumentException( msg );
			}
		}

		public void InitializeCurrentTelescope( string telescopeID, double fastPollingPeriod )
		{
			TelescopeID = telescopeID;
			TelescopeSetupVm.TelescopeID = telescopeID;
			TelescopeSetupVm.FastUpdatePeriod = fastPollingPeriod;
			IsTelescopeActive = TelescopeManager.Instance.IsConnected;
		}

		public void InitializeCurrentDome( string domeID, double fastUpdatePeriod )
		{
			DomeID = domeID;
			DomeSetupVm.DomeID = domeID;
			DomeSetupVm.InitializeLayout( Globals.DomeLayout );
			DomeSetupVm.FastUpdatePeriod = fastUpdatePeriod;
			IsDomeActive = DomeManager.Instance.IsConnected;
		}

		public void InitializeCurrentFocuser( string focuserID, double fastUpdatePeriod )
		{
			FocuserID = focuserID;
			FocuserSetupVm.FocuserID = focuserID;
			FocuserSetupVm.Initialize( Globals.FocuserTemperatureOffset );
			FocuserSetupVm.FastUpdatePeriod = fastUpdatePeriod;
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
			Globals.AlwaysOnTop = KeepWindowOnTop;
			Globals.UseCompositeSlewingFlag = UseCompositeSlewingFlag;
			Globals.ShowActivityLogWhenStarted = ShowActivityLogWhenStarted;
			Globals.WriteLogActivityToDisk=WriteActivityLogToDisk;
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
			settings.DomeLayout = Globals.DomeLayout;
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
			if ( !IsTelescopeActive )
			{
				if ( TelescopeID != TelescopeSetupVm.TelescopeID )
				{
					TelescopeID = TelescopeSetupVm.TelescopeID;
					TelescopeManager.SetTelescopeID( TelescopeID );
				}

				TelescopeManager.Instance.SetFastUpdatePeriod( TelescopeSetupVm.FastUpdatePeriod );
			}

			if ( !IsDomeActive )
			{
				if ( DomeID != DomeSetupVm.DomeID )
				{
					DomeID = DomeSetupVm.DomeID;
					DomeManager.SetDomeID( DomeID );
				}

				DomeManager.Instance.SetFastUpdatePeriod( DomeSetupVm.FastUpdatePeriod );
				Globals.DomeLayout = DomeSetupVm.GetLayout();
			}

			if ( !IsFocuserActive )
			{
				if ( FocuserID != FocuserSetupVm.FocuserID )
				{
					FocuserID = FocuserSetupVm.FocuserID;
					FocuserManager.SetFocuserID( FocuserID );
				}

				FocuserManager.Instance.SetFastUpdatePeriod( FocuserSetupVm.FastUpdatePeriod );
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
