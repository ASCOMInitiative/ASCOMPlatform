using System.Windows.Input;

namespace ASCOM.DeviceHub
{
	public class DriverSetupDialogViewModel : DeviceHubDialogViewModelBase
	{
		public DriverSetupDialogViewModel()
			: base( "Driver Setup Dialog")
		{
			_telescopeSetupVm = new TelescopeSetupViewModel();
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

		private TelescopeSetupViewModel _telescopeSetupVm;

		public TelescopeSetupViewModel TelescopeSetupVm
		{
			get { return _telescopeSetupVm; }
			set
			{
				if ( value != _telescopeSetupVm )
				{
					_telescopeSetupVm = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Public Methods

		public void InitializeCurrentTelescope( string telescopeID )
		{
			TelescopeSetupVm.TelescopeID = telescopeID;
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
