using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using ASCOM.DeviceHub.MvvmMessenger;

using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	public class TelescopeMotionViewModel : DeviceHubViewModelBase
	{
		private bool _jogInProgress;
		private bool _isActive;
		private bool _isFixedSlewInProgress;

		private readonly ITelescopeManager _telescopeManager;

		private ITelescopeManager TelescopeManager
		{
			get
			{
				return _telescopeManager;
			}
		}

		public TelescopeMotionViewModel( ITelescopeManager telescopeManager )
		{
			_isActive = false;
			_telescopeManager = telescopeManager;
			_isVariableJog = true;
			_jogInProgress = false;
			_slewAmounts = new TelescopeSlewAmounts();
			_selectedSlewAmount = _slewAmounts[0];
			_status = null;
			_slewDirections = null;

			SetParkCommandAction();

			Messenger.Default.Register<TelescopeCapabilitiesUpdatedMessage>( this, ( action ) => TelescopeCapabilitiesUpdated( action ) );
			Messenger.Default.Register<TelescopeParametersUpdatedMessage>( this, ( action ) => TelescopeParametersUpdated( action ) );
			Messenger.Default.Register<DeviceDisconnectedMessage>( this, ( action ) => InvalidateDeviceData( action ) );
			RegisterStatusUpdateMessage( true );
		}

		#region Change Notification Properties

		private string _parkCommandAction;

		public string ParkCommandAction
		{
			get { return _parkCommandAction; }
			set
			{
				if ( value != _parkCommandAction )
				{
					_parkCommandAction = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _canStartMoveTelescope;

		public bool CanStartMoveTelescope
		{
			get { return _canStartMoveTelescope; }
			set
			{
				if ( value != _canStartMoveTelescope )
				{
					_canStartMoveTelescope = value;
					OnPropertyChanged();

					RelayCommand.RaiseCanExecuteChanged();
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
				}
			}
		}

		private bool _isVariableJog;

		public bool IsVariableJog
		{
			get { return _isVariableJog; }
			set
			{
				if ( value != _isVariableJog )
				{
					_isVariableJog = value;
					OnPropertyChanged();
					SetSlewDirections();
				}
			}
		}

		private JogRates _jogRates;

		public JogRates JogRates
		{
			get { return _jogRates; }
			set
			{
				if ( value != _jogRates )
				{
					_jogRates = value;
					OnPropertyChanged();
				}
			}
		}

		private JogRate _selectedJogRate;

		public JogRate SelectedJogRate
		{
			get { return _selectedJogRate; }
			set
			{
				if ( value != _selectedJogRate )
				{
					_selectedJogRate = value;
					OnPropertyChanged();
				}
			}
		}

		private TelescopeSlewAmounts _slewAmounts;

		public TelescopeSlewAmounts SlewAmounts
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

		private bool _isTracking;

		public bool IsTracking
		{
			get { return _isTracking; }
			set
			{
				if ( value != _isTracking )
				{
					_isTracking = value;
					OnPropertyChanged();
				}
			}
		}

		private ObservableCollection<SlewDirection> _slewDirections;

		public ObservableCollection<SlewDirection> SlewDirections
		{
			get { return _slewDirections; }
			set
			{
				if ( value != _slewDirections )
				{
					_slewDirections = value;
					OnPropertyChanged();
				}
			}
		}

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

		private TelescopeParameters _parameters;

		public TelescopeParameters Parameters
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

		#endregion Change Notification Properties

		#region Public Methods

		public void SetActive( bool isActive )
		{
			_isActive = isActive;
		}

		#endregion Public Methods

		#region Helper Methods

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

		private void UpdateCanStartMove()
		{
			// This sets the property that enables/disables the motion buttons.

			// Be careful to not set this to false while jogging!!!
			// To do so will disable the mouse up event!!!

			if ( _jogInProgress && CanStartMoveTelescope )
			{
				return;
			}

			bool canMove = false;

			if ( Status != null && Capabilities != null && Parameters != null && Status.Connected )
			{
				if ( Status.IsReadyToSlew )
				{
					if ( IsVariableJog )
					{
						bool canMoveAxis = Capabilities.CanMovePrimaryAxis && Capabilities.CanMoveSecondaryAxis;
									
						bool canMovePulse = Capabilities.CanPulseGuide && !Double.IsNaN( Status.GuideRateDeclination )
								&& !Double.IsNaN( Status.GuideRateRightAscension );

						canMove = canMoveAxis || canMovePulse;
					}
					else
					{
						canMove = Status.Tracking ? Capabilities.CanSlew : Capabilities.CanSlewAltAz;
					}
				}
			}

			CanStartMoveTelescope = canMove;
		}

		private void BuildJogRatesList()
		{
			JogRates jogRates = null;

			if ( Capabilities != null )
			{
				IRate[] primaryAxisRates = Capabilities.PrimaryAxisRates;
				jogRates = JogRates.FromAxisRates( primaryAxisRates );
			}

			if ( jogRates == null )
			{
				JogRates = null;
				SelectedJogRate = null;
			}
			else
			{
				JogRates = jogRates;
				SelectedJogRate = jogRates[0];
			}
		}

		private void SetParkCommandAction()
		{
			ParkCommandAction = ( Status != null && Status.ParkingState == ParkingStateEnum.IsAtPark ) ? "Unpark" : "Park";
		}

		private void UpdateStatus( TelescopeStatusUpdatedMessage action )
		{
			// This is a registered message handler. It could be called from a worker thread
			// and we need to be sure that the work is done on the U/I thread.

			Task.Factory.StartNew( () =>
			{
				Status = action.Status;

				if ( JogRates == null )
				{
					BuildJogRatesList();
				}

				if ( SlewDirections == null )
				{
					SetSlewDirections();
				}

				UpdateCanStartMove();

				IsTracking = ( Status!= null && Status.Connected ) ? Status.Tracking : false;

				if ( _isActive && _isFixedSlewInProgress && !Status.Slewing )
				{
					_isFixedSlewInProgress = false;
					Messenger.Default.Send( new SlewInProgressMessage( false ) );
				}

				SetParkCommandAction();

				RelayCommand.RaiseCanExecuteChanged();
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void TelescopeCapabilitiesUpdated( TelescopeCapabilitiesUpdatedMessage msg )
		{
			// Make sure that we update the Capabilities on the U/I thread.

			Task.Factory.StartNew( () => Capabilities = msg.Capabilities, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void TelescopeParametersUpdated( TelescopeParametersUpdatedMessage action )
		{
			// Make sure that we update the Parameters on the U/I thread.

			Task.Factory.StartNew( () => Parameters = action.Parameters, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void InvalidateDeviceData( DeviceDisconnectedMessage action )
		{
			if ( action.DeviceType == DeviceTypeEnum.Telescope )
			{
				Task.Factory.StartNew( () =>
				{
					Status = null;
					Capabilities = null;
					Parameters = null;
					CanStartMoveTelescope = false;
					BuildJogRatesList();
					IsTracking = false;
					SlewDirections = null;
				}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
			}
		}

		private void SetSlewDirections()
		{
			SlewDirections = new ObservableCollection<SlewDirection>()
			{
				new SlewDirection { Name = "N", Description = "North", Direction = MoveDirections.North },
				new SlewDirection { Name = "S", Description = "South", Direction = MoveDirections.South },
				new SlewDirection { Name = "W", Description = "West", Direction = MoveDirections.West },
				new SlewDirection { Name = "E", Description = "East", Direction = MoveDirections.East }
			};

			try
			{
				if ( TelescopeManager.IsConnected )
				{
					if ( Parameters.AlignmentMode == AlignmentModes.algAltAz && IsVariableJog )
					{
						SlewDirections.Clear();

						SlewDirections.Add( new SlewDirection { Name = "U", Description = "Up", Direction = MoveDirections.Up } );
						SlewDirections.Add( new SlewDirection { Name = "D", Description = "Down", Direction = MoveDirections.Down } );
						SlewDirections.Add( new SlewDirection { Name = "L", Description = "Left", Direction = MoveDirections.Left } );
						SlewDirections.Add( new SlewDirection { Name = "R", Description = "Right", Direction = MoveDirections.Right } );
					}
					else if ( Parameters.SiteLatitude < 0 )
					{
						SlewDirections.Clear();

						SlewDirections.Add( new SlewDirection { Name = "S", Description = "South", Direction = MoveDirections.South } );
						SlewDirections.Add( new SlewDirection { Name = "N", Description = "North", Direction = MoveDirections.North } );
						SlewDirections.Add( new SlewDirection { Name = "W", Description = "West", Direction = MoveDirections.West } );
						SlewDirections.Add( new SlewDirection { Name = "E", Description = "East", Direction = MoveDirections.East } );
					}
				}
			}
			catch ( Exception )
			{ }
		}

		#endregion Helper Methods

		#region Relay Commands

		#region ChangeTrackingCommand

		private ICommand _changeTrackingCommand;

		public ICommand ChangeTrackingCommand
		{
			get
			{
				if ( _changeTrackingCommand == null )
				{
					_changeTrackingCommand = new RelayCommand(
						param => this.ChangeTracking(),
						param => this.CanChangeTracking() );
				}

				return _changeTrackingCommand;
			}
		}

		private void ChangeTracking()
		{
			Status.Tracking = _isTracking;
			TelescopeManager.SetTracking( _isTracking );
		}

		private bool CanChangeTracking()
		{
			bool retval = false;

			if  ( Status != null && Status.Connected && Status.ParkingState == ParkingStateEnum.Unparked )
			{
				retval = true;
			}

			return retval;
		}

		#endregion

		#region ChangeParkStateCommand

		private ICommand _changeParkStateCommand;

		public ICommand ChangeParkStateCommand
		{
			get
			{
				if ( _changeParkStateCommand == null )
				{
					_changeParkStateCommand = new RelayCommand(
						param => this.ChangeParkState(),
						param => this.CanChangeParkState() );
				}

				return _changeParkStateCommand;
			}
		}

		private void ChangeParkState()
		{
			ParkingStateEnum newState = ( Status.ParkingState == ParkingStateEnum.Unparked ) ? ParkingStateEnum.IsAtPark : ParkingStateEnum.Unparked;

			TelescopeManager.SetParkingState( newState );
		}

		private bool CanChangeParkState()
		{
			bool retval = false;

			if ( Status != null && Capabilities != null && Status.Connected )
			{
				bool canPark = !Status.Slewing && Capabilities.CanPark && Status.ParkingState == ParkingStateEnum.Unparked;
				bool canUnpark = Capabilities.CanUnpark && Status.ParkingState == ParkingStateEnum.IsAtPark;

				retval = canPark || canUnpark;
			}

			return retval;
		}

		#endregion ChangeParkStateCommand

		#region DoMeridianFlipCommand

		private ICommand _doMeridianFlipCommand;

		public ICommand DoMeridianFlipCommand
		{
			get
			{
				if ( _doMeridianFlipCommand == null )
				{
					_doMeridianFlipCommand = new RelayCommand(
						param => this.DoMeridianFlip(),
						param => this.CanDoMeridianFlip() );
				}

				return _doMeridianFlipCommand;
			}
		}

		private void DoMeridianFlip()
		{
			TelescopeManager.StartMeridianFlip();
		}

		private bool CanDoMeridianFlip()
		{

			bool canFlip = Status != null && Parameters != null && Capabilities != null && Status.Connected;

			if ( canFlip )
			{
				canFlip &= Capabilities.CanSetPierSide || Capabilities.CanSlewAsync;
				canFlip &= Status.IsReadyToSlew && Parameters.AlignmentMode == AlignmentModes.algGermanPolar;
				canFlip &= Status.IsCounterWeightUp && Status.SideOfPier == PierSide.pierWest;
			}

			return canFlip;
		}

		#endregion DoMeridianFlipCommand

		#region StartMoveCommand

		private ICommand _startMoveCommand;

		public ICommand StartMoveCommand
		{
			get
			{
				if ( _startMoveCommand == null )
				{
					_startMoveCommand = new RelayCommand(
						param => this.StartMove( param ) );
				}

				return _startMoveCommand;
			}
		}

		private void StartMove( object param )
		{
			MoveDirections direction = (MoveDirections)param;

			if ( IsVariableJog )
			{
				try
				{
					TelescopeManager.StartJogScope( direction, SelectedJogRate );
					_jogInProgress = true;
				}
				catch ( Exception xcp )
				{
					ShowMessage( "Unable to use the requested Jog rate.\r\n\r\n"
						+ xcp.Message, "Unsupported Jog Rate Selected" );
				}
			}
		}

		#endregion StartMoveCommand

		#region StopMotionCommand

		private ICommand _stopMotionCommand;

		public ICommand StopMotionCommand
		{
			get
			{
				if ( _stopMotionCommand == null )
				{
					_stopMotionCommand = new RelayCommand(
						param => this.StopMotion( param ) );
				}

				return _stopMotionCommand;
			}
		}

		private void StopMotion( object param )
		{
			if ( IsVariableJog && _jogInProgress )
			{
				MoveDirections direction = ( param == null ) ? MoveDirections.None : (MoveDirections)param;
				TelescopeManager.StopJogScope( direction );
			}
		}

		#endregion StopMotionCommand

		#region DoFixedSlewCommand

		private ICommand _doFixedSlewCommand;

		public ICommand DoFixedSlewCommand
		{
			get
			{
				if ( _doFixedSlewCommand == null )
				{
					_doFixedSlewCommand = new RelayCommand(
						param => this.DoFixedSlew( param ) );
				}

				return _doFixedSlewCommand;
			}
		}

		private void DoFixedSlew( object param )
		{
			if ( !IsVariableJog )
			{
				MoveDirections direction = ( param == null ) ? MoveDirections.None : (MoveDirections)param;

				if ( direction != MoveDirections.None )
				{
					TelescopeManager.StartFixedSlew( direction, SelectedSlewAmount.Amount );

					_isFixedSlewInProgress = true;
				}
			}
		}

		#endregion DoFixedSlewCommand

		#region SetParkPositionCommand

		private ICommand _setParkPositionCommand;

		public ICommand SetParkPositionCommand
		{
			get
			{
				if ( _setParkPositionCommand == null )
				{
					_setParkPositionCommand = new RelayCommand(
						param => this.SetParkPosition(),
						param => this.CanSetParkPosition() );
				}

				return _setParkPositionCommand;
			}
		}

		private void SetParkPosition()
		{
		}

		private bool CanSetParkPosition()
		{
			bool retval = false;

			if ( Status != null && Capabilities != null && Status.Connected )
			{
				bool canSetPark = !Status.Slewing && Capabilities.CanSetPark && Status.ParkingState == ParkingStateEnum.Unparked;

				retval = canSetPark;
			}

			return retval;
		}

		#endregion

		#endregion Relay Commands

		#region IDisposable override

		protected override void DoDispose()
		{
			Messenger.Default.Unregister<TelescopeCapabilitiesUpdatedMessage>( this );
			Messenger.Default.Unregister<TelescopeParametersUpdatedMessage>( this );
			Messenger.Default.Unregister<DeviceDisconnectedMessage>( this );
			RegisterStatusUpdateMessage( false );

			_changeParkStateCommand = null;
			_doMeridianFlipCommand = null;
			_startMoveCommand = null;
			_stopMotionCommand = null;
			_doFixedSlewCommand = null;
		}

		#endregion IDisposable override
	}
}
