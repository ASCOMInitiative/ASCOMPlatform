using System.Windows.Input;

namespace ASCOM.DeviceHub
{
	public class FocuserDriverSetupDialogViewModel : DeviceHubDialogViewModelBase
	{
		public FocuserDriverSetupDialogViewModel()
			: base( "Driver Setup Dialog" )
		{
			_focuserSetupVm = new FocuserSetupViewModel();
		}

		#region Change Notification Properties


		private bool _isLoggingEnabled;

		public bool IsLoggingEnabled
		{
			get { return _isLoggingEnabled; }
			set
			{
				if ( value != _isLoggingEnabled )
				{
					_isLoggingEnabled = value;
					OnPropertyChanged();
				}
			}
		}

		private FocuserSetupViewModel _focuserSetupVm;

		public FocuserSetupViewModel FocuserSetupVm
		{
			get { return _focuserSetupVm; }
			set
			{
				if ( value != _focuserSetupVm )
				{
					_focuserSetupVm = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Public Methods

		public void InitializeCurrentFocuser( string focuserID )
		{
			FocuserSetupVm.FocuserID = focuserID;
			FocuserSetupVm.Initialize( Globals.FocuserTemperatureOffset );
		}

		#endregion Public Methods

		#region Relay Commands

		#region SelectDriverCommand

		private ICommand _selectDriverCommand;

		public ICommand SelectDriverCommand
		{
			get
			{
				if ( _selectDriverCommand == null )
				{
					_selectDriverCommand = new RelayCommand(
						param => this.SelectDriver() );
				}

				return _selectDriverCommand;
			}
		}

		private void SelectDriver()
		{
			OnRequestClose( true );
		}

		#endregion SelectDriverCommand

		#region AbortSelectDriverCommand

		private ICommand _abortSelectDriverCommand;

		public ICommand AbortSelectDriverCommand
		{
			get
			{
				if ( _abortSelectDriverCommand == null )
				{
					_abortSelectDriverCommand = new RelayCommand(
						param => this.AbortSelectDriver() );
				}

				return _abortSelectDriverCommand;
			}
		}

		private void AbortSelectDriver()
		{
			OnRequestClose( false );
		}

		#endregion AbortSelectDriverCommand

		#endregion Relay Commands
	}
}
