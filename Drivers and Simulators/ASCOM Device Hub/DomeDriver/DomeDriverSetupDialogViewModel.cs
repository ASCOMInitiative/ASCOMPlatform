using System.Windows.Input;

namespace ASCOM.DeviceHub
{
	public class DomeDriverSetupDialogViewModel : DeviceHubDialogViewModelBase
	{
		public DomeDriverSetupDialogViewModel()
			: base( "Driver Setup Dialog")
		{
			_domeSetupVm = new DomeSetupViewModel();
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

		private DomeSetupViewModel _domeSetupVm;

		public DomeSetupViewModel DomeSetupVm
		{
			get { return _domeSetupVm; }
			set
			{
				if ( value != _domeSetupVm )
				{
					_domeSetupVm = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Public Methods

		public void InitializeCurrentDome( string domeID )
		{
			DomeSetupVm.DomeID = domeID;
			DomeSetupVm.Initialize( Globals.DomeLayout );
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

		#endregion

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

		#endregion

		#endregion Relay Commands
	}
}
