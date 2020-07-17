using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class TelescopeDirectSlewViewModel : DeviceHubViewModelBase
	{
		private bool _isActive;
		private bool _isSlewInProgress;

		public ITelescopeManager TelescopeManager { get; private set; }

		public TelescopeDirectSlewViewModel( ITelescopeManager telescopeManager )
		{
			_isActive = false;
			_isSlewInProgress = false;
			_useDecimalHours = true;

			TelescopeManager = telescopeManager;
			Status = null;

			Messenger.Default.Register<TelescopeCapabilitiesUpdatedMessage>( this, ( action ) => UpdateCapabilities( action ) );
			Messenger.Default.Register<DeviceDisconnectedMessage>( this, ( action ) => InvalidateDeviceData( action ) );
			RegisterStatusUpdateMessage( true );
		}

		#region Change Notification Properties

		private TelescopeCapabilities _capabilities;

		public TelescopeCapabilities Capabilities
		{
			get { return _capabilities; }
			set
			{
				if ( value != _capabilities )
				{
					_capabilities = value;
					OnPropertyChanged();
				}
			}
		}
		
		private DevHubTelescopeStatus _status;

		public DevHubTelescopeStatus Status
		{
			get { return _status; }
			set
			{
				if ( value != _status )
				{
					_status = value;
					OnPropertyChanged();
					RelayCommand.RaiseCanExecuteChanged();
				}
			}
		}

		private bool _useDecimalHours;

		public bool UseDecimalHours
		{
			get { return _useDecimalHours; }
			set
			{
				if ( value != _useDecimalHours )
				{
					_useDecimalHours = value;
					OnPropertyChanged();
				}
			}
		}

		private double _targetRightAscension;

		public double TargetRightAscension
		{
			get { return _targetRightAscension; }
			set
			{
				if ( value != _targetRightAscension )
				{
					_targetRightAscension = value;
					OnPropertyChanged();
				}
			}
		}

		private double _targetDeclination;

		public double TargetDeclination
		{
			get { return _targetDeclination; }
			set
			{
				if ( value != _targetDeclination )
				{
					_targetDeclination = value;
					OnPropertyChanged();
				}
			}
		}

		private double _targetAzimuth;

		public double TargetAzimuth
		{
			get { return _targetAzimuth; }
			set
			{
				if ( value != _targetAzimuth )
				{
					_targetAzimuth = value;
					OnPropertyChanged();
				}
			}
		}

		private double _targetAltitude;

		public double TargetAltitude
		{
			get { return _targetAltitude; }
			set
			{
				if ( value != _targetAltitude )
				{
					_targetAltitude = value;
					OnPropertyChanged();
				}
			}
		}


		private bool _canDoDirectSlew;

		public bool CanDoDirectSlew
		{
			get { return _canDoDirectSlew; }
			set
			{
				if ( value != _canDoDirectSlew )
				{
					_canDoDirectSlew = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Public Methods

		public void SetActive( bool isActive )
		{
			// Called from code behind whenever the selected tab is changed...true if we are active.

			_isActive = isActive;
		}

		#endregion Public Methods

		#region Helper Methods

		private void SetDirectCoordinates( double ra, double dec, double az, double alt )
		{
			TargetRightAscension = ra;
			TargetDeclination = dec;
			TargetAzimuth = az;
			TargetAltitude = alt;
		}

		private void UpdateCapabilities( TelescopeCapabilitiesUpdatedMessage action )
		{
			SetCapabilities( action.Capabilities );
		}

		private void InvalidateDeviceData( DeviceDisconnectedMessage action )
		{
			if ( action.DeviceType == DeviceTypeEnum.Telescope )
			{
				SetStatus( null );
				SetCapabilities( null );
				TargetRightAscension = Double.NaN;
				TargetDeclination = Double.NaN;
				TargetAzimuth = Double.NaN;
				TargetAltitude = Double.NaN;
			}
		}

		private void UpdateCanDoDirectSlew()
		{
			bool canSlew = false;

			if ( Status != null && Capabilities != null && Status.IsReadyToSlew )
			{
				if ( Status.Tracking && Capabilities.CanSlew )
				{
					canSlew = true;
				}
				else if ( !Status.Tracking && Capabilities.CanSlewAltAz )
				{
					canSlew = true;
				}
			}

			CanDoDirectSlew = canSlew;
		}

		private void SetCapabilities( TelescopeCapabilities capabilities )
		{
			// Make sure that we update the Capabilities on the U/I thread.

			Task.Factory.StartNew( () =>
			{
				Capabilities = capabilities;
				UpdateCanDoDirectSlew();
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void UpdateStatus( TelescopeStatusUpdatedMessage action )
		{
			SetStatus( action.Status );
		}

		private void SetStatus( DevHubTelescopeStatus status )
		{
			// Make sure that we update the Status on the U/I thread.

			Task.Factory.StartNew( () =>
			{
				Status = status;
				UpdateCanDoDirectSlew();

				if ( !_isActive )
				{
					double ra = ( Status != null ) ? Status.RightAscension : Double.NaN;
					double dec = ( Status != null ) ? Status.Declination : Double.NaN;
					double az = ( Status != null ) ? Status.Azimuth : Double.NaN;
					double alt = ( Status != null ) ? Status.Altitude : Double.NaN;

					SetDirectCoordinates( ra, dec, az, alt );
				}

				if ( _isActive && _isSlewInProgress && !Status.Slewing )
				{
					_isSlewInProgress = false;
					Messenger.Default.Send( new SlewInProgressMessage( false ) );
				}
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void RegisterStatusUpdateMessage( bool regUnreg )
		{
			if ( regUnreg )
			{
				Messenger.Default.Register<TelescopeStatusUpdatedMessage>( this, ( action ) => UpdateStatus( action ) );
			}
			else
			{
				Messenger.Default.Unregister<TelescopeStatusUpdatedMessage>( this );

			}
		}

		protected override void DoDispose()
		{
			Messenger.Default.Unregister<TelescopeCapabilitiesUpdatedMessage>( this );
			RegisterStatusUpdateMessage( false );

			_beginDirectSlewCommand = null;
			_abortDirectSlewCommand = null;
		}

		#endregion Helper Methods

		#region Relay Commands

		#region EditDeclinationCommand

		private ICommand _editDeclinationCommand;

		public ICommand EditDeclinationCommand
		{
			get
			{
				if ( _editDeclinationCommand == null )
				{
					_editDeclinationCommand = new RelayCommand(
						param => this.EditDeclination() );
				}

				return _editDeclinationCommand;
			}
		}

		private void EditDeclination()
		{
			decimal dec = Double.IsNaN(TargetDeclination) ? 0.0m : (decimal)TargetDeclination;
			DeclinationConverter converter = new DeclinationConverter( dec );

			DeclinationValuesEntryViewModel vm = new DeclinationValuesEntryViewModel();
			vm.InitializeValues( converter.Values );

			IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
			bool? result = svc.ShowDialog( vm );

			if ( result.HasValue && result.Value )
			{
				int[] values = vm.GetValues();

				converter = new DeclinationConverter( values );
				TargetDeclination = (double)converter.Value;
			}

			vm.Dispose();
			vm = null;
		}

		#endregion

		#region EditRightAscensionCommand

		private ICommand _editRightAscensionCommand;

		public ICommand EditRightAscensionCommand
		{
			get
			{
				if ( _editRightAscensionCommand == null )
				{
					_editRightAscensionCommand = new RelayCommand(
						param => this.EditRightAscension() );
				}

				return _editRightAscensionCommand;
			}
		}

		private void EditRightAscension()
		{
			decimal ra = Double.IsNaN( TargetRightAscension ) ? 0.0m : (decimal)TargetRightAscension;
			RightAscensionConverter  converter = new RightAscensionConverter( ra );

			RightAscensionValuesEntryViewModel vm = new RightAscensionValuesEntryViewModel();
			vm.InitializeValues( converter.Values );

			IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
			bool? result = svc.ShowDialog( vm );

			if ( result.HasValue && result.Value )
			{
				int[] values = vm.GetValues();

				converter = new RightAscensionConverter( values );
				TargetRightAscension = (double)converter.Value;
			}

			vm.Dispose();
			vm = null;
		}

		#endregion

		#region EditAltitudeCommand

		private ICommand _editAltitudeCommand;

		public ICommand EditAltitudeCommand
		{
			get
			{
				if ( _editAltitudeCommand == null )
				{
					_editAltitudeCommand = new RelayCommand(
						param => this.EditAltitude() );
				}

				return _editAltitudeCommand;
			}
		}

		private void EditAltitude()
		{
			decimal alt = Double.IsNaN( TargetAltitude ) ? 0.0m : (decimal)TargetAltitude;
			AltitudeConverter converter = new AltitudeConverter( alt );

			AltitudeValuesEntryViewModel vm = new AltitudeValuesEntryViewModel();
			vm.InitializeValues( converter.Values );

			IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
			bool? result = svc.ShowDialog( vm );

			if ( result.HasValue && result.Value )
			{
				int[] values = vm.GetValues();

				converter = new AltitudeConverter( values );
				TargetAltitude = (double)converter.Value;
			}

			vm.Dispose();
			vm = null;
		}

		#endregion

		#region EditAzimuthCommand

		private ICommand _editAzimuthCommand;

		public ICommand EditAzimuthCommand
		{
			get
			{
				if ( _editAzimuthCommand == null )
				{
					_editAzimuthCommand = new RelayCommand(
						param => this.EditAzimuth() );
				}

				return _editAzimuthCommand;
			}
		}

		private void EditAzimuth()
		{
			decimal az = Double.IsNaN( TargetAzimuth ) ? 0.0m : (decimal)TargetAzimuth;
			AzimuthConverter converter = new AzimuthConverter( az );

			AzimuthValuesEntryViewModel vm = new AzimuthValuesEntryViewModel();
			vm.InitializeValues( converter.Values );

			IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
			bool? result = svc.ShowDialog( vm );

			if ( result.HasValue && result.Value )
			{
				int[] values = vm.GetValues();

				converter = new AzimuthConverter( values );
				TargetAzimuth = (double)converter.Value;
			}

			vm.Dispose();
			vm = null;
		}

		#endregion

		#region BeginDirectSlewCommand

		private RelayCommand _beginDirectSlewCommand;

		public RelayCommand BeginDirectSlewCommand
		{
			get
			{
				if ( _beginDirectSlewCommand == null )
				{
					_beginDirectSlewCommand = new RelayCommand(
						param => this.BeginDirectSlew(),
						param => this.CanBeginDirectSlew() );
				}

				return _beginDirectSlewCommand;
			}
		}

		private void BeginDirectSlew()
		{
			if ( Status.Tracking )
			{
				if ( Capabilities.CanSlewAsync )
				{
					TelescopeManager.BeginSlewToCoordinatesAsync( TargetRightAscension, TargetDeclination );
				}
				else
				{
					TelescopeManager.DoSlewToCoordinates( TargetRightAscension, TargetDeclination, false );
				}
			}
			else
			{
				if ( Capabilities.CanSlewAltAzAsync )
				{
					TelescopeManager.BeginSlewToAltAzAsync( TargetAzimuth, TargetAltitude );
				}
				else
				{
					TelescopeManager.DoSlewToAltAz( TargetAzimuth, TargetAltitude, false );
				}
			}

			_isSlewInProgress = true;			
		}

		private bool CanBeginDirectSlew()
		{
			bool retval = CanDoDirectSlew;

			return retval;
		}

		#endregion BeginDirectSlewCommand

		#region AbortDirectSlewCommand

		private RelayCommand _abortDirectSlewCommand;

		public RelayCommand AbortDirectSlewCommand
		{
			get
			{
				if ( _abortDirectSlewCommand == null )
				{
					_abortDirectSlewCommand = new RelayCommand(
						param => this.AbortDirectSlew(),
						param => this.CanAbortDirectSlew() );
				}

				return _abortDirectSlewCommand;
			}
		}

		private void AbortDirectSlew()
		{
			TelescopeManager.AbortDirectSlew();
		}

		private bool CanAbortDirectSlew()
		{
			bool retval = Capabilities != null && Status != null;

			if ( retval )
			{
				retval = Capabilities.CanSlew && Status.Slewing;
			}

			return retval;
		}

		#endregion AbortDirectSlewCommand

		#endregion Relay Commands
	}
}
