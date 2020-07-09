using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using ASCOM.DeviceInterface;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class DomeMotionViewModel : DeviceHubViewModelBase
	{
		private readonly IDomeManager _domeManager;
		private IDomeManager DomeManager => _domeManager;

		public DomeMotionViewModel( IDomeManager domeManager )
		{
			_domeManager = domeManager;
			_status = null;
			_slewAmounts = new DomeSlewAmounts();
			_selectedSlewAmount = _slewAmounts[0];

			Messenger.Default.Register<DomeCapabilitiesUpdatedMessage>( this, ( action ) => DomeCapabilitiesUpdated( action ) );
			Messenger.Default.Register<DomeParametersUpdatedMessage>( this, ( action ) => DomeParametersUpdated( action ) );
			Messenger.Default.Register<DeviceDisconnectedMessage>( this, ( action ) => InvalidateDeviceData( action ) );
			Messenger.Default.Register<DomeSlavedChangedMessage>( this, ( action ) => ChangeSlavedState( action ) );
			RegisterStatusUpdateMessage( true );
		}

		#region Change Notification Properties

		private string _shutterCommandAction;

		public string ShutterCommandAction
		{
			get { return _shutterCommandAction; }
			set
			{
				if ( value != _shutterCommandAction )
				{
					_shutterCommandAction = value;
					OnPropertyChanged();
				}
			}
		}

		private DomeSlewAmounts _slewAmounts;

		public DomeSlewAmounts SlewAmounts
		{
			get { return _slewAmounts; }
			set
			{
				if ( value != _slewAmounts )
				{
					_slewAmounts = value;
					OnPropertyChanged();
				}
			}
		}

		private SlewAmount _selectedSlewAmount;

		public SlewAmount SelectedSlewAmount
		{
			get { return _selectedSlewAmount; }
			set
			{
				if ( value != _selectedSlewAmount )
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
				if ( value != Globals.IsDomeSlaved )
				{
					Globals.IsDomeSlaved = value;
					OnPropertyChanged();
				}
			}
		}

		private DevHubDomeStatus _status;

		public DevHubDomeStatus Status
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


		private bool _isSlewing;

		public bool IsSlewing
		{
			get { return _isSlewing; }
			set
			{
				if ( value != _isSlewing )
				{
					_isSlewing = value;
					OnPropertyChanged();
				}
			}
		}

		private double _directTargetAzimuth;

		public double DirectTargetAzimuth
		{
			get { return _directTargetAzimuth; }
			set
			{
				if ( value != _directTargetAzimuth )
				{
					_directTargetAzimuth = value;
					OnPropertyChanged();
				}
			}
		}

		private double _syncTargetAzimuth;

		public double SyncTargetAzimuth
		{
			get { return _syncTargetAzimuth; }
			set
			{
				if ( value != _syncTargetAzimuth )
				{
					_syncTargetAzimuth = value;
					OnPropertyChanged();
				}
			}
		}

		public bool UsePOTHSlaveCalculation
		{
			get { return Globals.UsePOTHDomeSlaveCalculation; }
			set
			{
				if ( value != Globals.UsePOTHDomeSlaveCalculation )
				{
					Globals.UsePOTHDomeSlaveCalculation = value;
					OnPropertyChanged();
				}
			}
		}

		public double DomeAzimuthAdjustment
		{
			get { return Globals.DomeAzimuthAdjustment; }
			set
			{
				if ( value != Globals.DomeAzimuthAdjustment )
				{
					Globals.DomeAzimuthAdjustment = value;
					OnPropertyChanged();
				}
			}
		}

		#region Capabilities Properties

		private DomeCapabilities _capabilities;

		public DomeCapabilities Capabilities
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

		#endregion Capabilities Properties

		#region Parameters Properties

		private DomeParameters _parameters;

		public DomeParameters Parameters
		{
			get { return _parameters; }
			set
			{
				if ( value != _parameters )
				{
					_parameters = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Parameters Properties

		#endregion Change Notification Properties

		#region Helper Methods

		private void InvalidateDeviceData( DeviceDisconnectedMessage action )
		{
			if ( action.DeviceType == DeviceTypeEnum.Dome )
			{
				Task.Factory.StartNew( () =>
				{
					Status = null;
					Capabilities = null;
					Parameters = null;
					IsSlaved = false;
				}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
			}
		}

		private void RegisterStatusUpdateMessage( bool regUnreg )
		{
			if ( regUnreg )
			{
				Messenger.Default.Register<DomeStatusUpdatedMessage>( this, ( action ) => UpdateStatus( action ) );
			}
			else
			{
				Messenger.Default.Unregister<DomeStatusUpdatedMessage>( this );
			}
		}

		private void UpdateStatus( DomeStatusUpdatedMessage action )
		{
			// This is a registered message handler. It could be called from a worker thread
			// and we need to be sure that the work is done on the U/I thread.

			Task.Factory.StartNew( () =>
			{
				Status = action.Status;
				IsSlewing = Status.Slewing;

				if ( Status.ShutterStatus == ShutterState.shutterClosed )
				{
					ShutterCommandAction = "Open Shutter";
				}
				else if ( Status.ShutterStatus == ShutterState.shutterOpen || Status.ShutterStatus == ShutterState.shutterError )
				{
					ShutterCommandAction = "Close Shutter";
				}
				else if ( Status.ShutterStatus == ShutterState.shutterOpening )
				{
					ShutterCommandAction = "Open In Progress";
				}
				else
				{
					ShutterCommandAction = "Close In Progress";
				}
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void DomeParametersUpdated( DomeParametersUpdatedMessage action )
		{
			// Make sure that we update the Parameters on the U/I thread.

			Task.Factory.StartNew( () => Parameters = action.Parameters, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void DomeCapabilitiesUpdated( DomeCapabilitiesUpdatedMessage action )
		{
			// Make sure that we update the Capabilities on the U/I thread.

			Task.Factory.StartNew( () => Capabilities = action.Capabilities, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void ChangeSlavedState( DomeSlavedChangedMessage action )
		{
			// Make sure that we update the IsSlaved property on the U/I thread.

			Task.Factory.StartNew( () => IsSlaved = action.State, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		#endregion Helper Methods

		#region Relay Commands

		#region ToggleShutterStateCommand

		private ICommand _toggleShutterStateCommand;

		public ICommand ToggleShutterStateCommand
		{
			get
			{
				if ( _toggleShutterStateCommand == null )
				{
					_toggleShutterStateCommand = new RelayCommand(
						param => this.ToggleShutterState(),
						param => this.CanToggleShutterState() );
				}

				return _toggleShutterStateCommand;
			}
		}

		private void ToggleShutterState()
		{
			ShutterState state = Status.ShutterStatus;

			switch (state)
			{
				case ShutterState.shutterOpen:
				case ShutterState.shutterOpening:
				case ShutterState.shutterError:
					DomeManager.CloseDomeShutter();
					break;

				case ShutterState.shutterClosing:
				case ShutterState.shutterClosed:
					DomeManager.OpenDomeShutter();
					break;
			}
		}

		private bool CanToggleShutterState()
		{
			bool retval = false;

			if ( Status != null && Status.Connected && !Status.Slewing 
				&& Status.ShutterStatus != ShutterState.shutterClosing && Status.ShutterStatus != ShutterState.shutterOpening
				&& !IsSlaved )
			{
				retval = true;
			}

			return retval;
		}

		#endregion

		#region ParkDomeCommand

		private ICommand _parkDomeCommand;

		public ICommand ParkDomeCommand
		{
			get
			{
				if ( _parkDomeCommand == null )
				{
					_parkDomeCommand = new RelayCommand(
						param => this.ParkDome(),
						param => this.CanParkDome() );
				}

				return _parkDomeCommand;
			}
		}

		private void ParkDome()
		{
			DomeManager.ParkTheDome();
		}

		private bool CanParkDome()
		{
			bool retval = false;

			if ( Status != null && Status.Connected && Capabilities != null && !IsSlaved && !Status.Slewing)
			{
				if ( Capabilities.CanPark && !Status.AtPark )
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
				if ( _jogAltitudeCommand == null )
				{
					_jogAltitudeCommand = new RelayCommand(
						param => this.JogAltitude(param),
						param => this.CanJogAltitude() );
				}

				return _jogAltitudeCommand;
			}
		}

		private void JogAltitude( object param )
		{
			MoveDirections direction = (MoveDirections)param;
			bool moveValid = ( direction == MoveDirections.Up || direction == MoveDirections.Down );

			if ( !moveValid )
			{
				return;
			}

			double newAltitude = Double.NaN;

			if ( direction == MoveDirections.Up )
			{
				newAltitude = Status.Altitude + SelectedSlewAmount.Amount;
				newAltitude = Math.Min( newAltitude, 90.0 ); // Clamp to the zenith
			}
			else if ( direction == MoveDirections.Down )
			{
				newAltitude = Status.Altitude - SelectedSlewAmount.Amount;
				newAltitude = Math.Max( newAltitude, 0.0 ); // Clamp to the horizon
			}

			if ( !Double.IsNaN( newAltitude ) )
			{
				DomeManager.SlewDomeShutter( newAltitude );
			}
		}

		private bool CanJogAltitude()
		{
			bool retval = false;

			if ( Status != null && Capabilities != null 
				&& Status.Connected && !Status.Slewing && Status.ShutterStatus == ShutterState.shutterOpen )
			{
				retval = Capabilities.CanSetAltitude;
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
				if ( _jogAzimuthCommand == null )
				{
					_jogAzimuthCommand = new RelayCommand(
						param => this.JogAzimuth( param ),
						param => this.CanJogAzimuth() );
				}

				return _jogAzimuthCommand;
			}
		}

		private void JogAzimuth( object param )
		{
			MoveDirections direction = (MoveDirections)param;
			bool moveValid = ( direction == MoveDirections.Clockwise || direction == MoveDirections.CounterClockwise );

			if ( !moveValid )
			{
				return;
			}

			double newAzimuth = Double.NaN;

			if ( direction == MoveDirections.Clockwise )
			{
				newAzimuth = Status.Azimuth + SelectedSlewAmount.Amount;

				if ( newAzimuth >= 360.0 )
				{
					newAzimuth -= 360.0;
				}
			}
			else if ( direction == MoveDirections.CounterClockwise )
			{
				newAzimuth = Status.Azimuth - SelectedSlewAmount.Amount;

				if ( newAzimuth < 0.0 )
				{
					newAzimuth += 360.0;
				}
			}

			if ( !Double.IsNaN( newAzimuth ) )
			{
				DomeManager.SlewDomeToAzimuth( newAzimuth );
			}
		}
	
		private bool CanJogAzimuth()
		{
			bool retval = false;

			if ( Status != null && Capabilities != null && Status.Connected && !Status.Slewing  )
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
				if ( _stopMotionCommand == null )
				{
					_stopMotionCommand = new RelayCommand(
						param => this.StopMotion() );
				}

				return _stopMotionCommand;
			}
		}

		private void StopMotion()
		{
			DomeManager.StopDomeMotion();
		}

		#endregion
		
		#region GotoDirectAzimuthCommand

		private ICommand _gotoDirectAzimuthCommand;

		public ICommand GotoDirectAzimuthCommand
		{
			get
			{
				if ( _gotoDirectAzimuthCommand == null )
				{
					_gotoDirectAzimuthCommand = new RelayCommand(
						param => this.GotoDirectAzimuth(),
						param => this.CanGotoDirectAzimuth() );
				}

				return _gotoDirectAzimuthCommand;
			}
		}

		private void GotoDirectAzimuth()
		{
			DomeManager.SlewDomeToAzimuth( DirectTargetAzimuth );
		}

		private bool CanGotoDirectAzimuth()
		{
			bool retval = false;

			if ( Status != null && Capabilities != null && Status.Connected && !Status.Slewing
				&& !Double.IsNaN( DirectTargetAzimuth ) )
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
				if ( _syncAzimuthCommand == null )
				{
					_syncAzimuthCommand = new RelayCommand(
						param => this.SyncAzimuth(),
						param => this.CanSyncAzimuth() );
				}

				return _syncAzimuthCommand;
			}
		}

		private void SyncAzimuth()
		{
			DomeManager.SyncDomeToAzimuth( SyncTargetAzimuth );
		}

		private bool CanSyncAzimuth()
		{
			bool retval = false;

			if ( Status != null && Capabilities != null && Status.Connected && !Status.Slewing
				&& !IsSlaved && !Double.IsNaN( SyncTargetAzimuth ) )
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
				if ( _findHomeCommand == null )
				{
					_findHomeCommand = new RelayCommand(
						param => this.FindHome(),
						param => this.CanFindHome() );
				}

				return _findHomeCommand;
			}
		}

		private void FindHome()
		{
			DomeManager.FindHomePosition();
		}

		private bool CanFindHome()
		{
			bool retval = false;

			if ( Status != null && Capabilities != null && Status.Connected && !IsSlaved 
				&& !Status.Slewing && Capabilities.CanFindHome )
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
				if ( _slaveToScopeCommand == null )
				{
					_slaveToScopeCommand = new RelayCommand(
						param => this.SlaveToScope( param ),
						param => this.CanSlaveToScope() );
				}

				return _slaveToScopeCommand;
			}
		}

		private void SlaveToScope( object param )
		{
			bool slaved = (bool)param;

			Globals.IsDomeSlaved = slaved;
		}

		private bool CanSlaveToScope()
		{
			bool retval = true;

			if ( !Globals.IsDomeSlaved )
			{
				// Verify that we are ready to slave the dome to the scope.

				retval =  DomeManager.IsScopeReadyToSlave && DomeManager.IsDomeReadyToSlave;
			}

			return retval;
		}

		#endregion

		#endregion Relay Commands

		protected override void DoDispose()
		{
			Messenger.Default.Unregister<DomeCapabilitiesUpdatedMessage>( this );
			Messenger.Default.Unregister<DomeParametersUpdatedMessage>( this );
			Messenger.Default.Unregister<DeviceDisconnectedMessage>( this );
			RegisterStatusUpdateMessage( false );

			_toggleShutterStateCommand = null;
			_parkDomeCommand = null;
			_jogAltitudeCommand = null;
			_jogAzimuthCommand = null;
			_stopMotionCommand = null;
			_gotoDirectAzimuthCommand = null;
			_syncAzimuthCommand = null;
			_findHomeCommand = null;
		}
	}
}
