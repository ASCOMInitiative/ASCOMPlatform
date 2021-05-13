using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
			_jogDirections = null;
			_hasAsymmetricJogRates = false;

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

		private bool _canMoveAxis;

		public bool CanMoveAxis
		{
			get { return _canMoveAxis; }
			set
			{
				if ( value != _canMoveAxis )
				{
					_canMoveAxis = value;
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
				}
			}
		}

		// This is set by radio buttons on the View.

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
					// SetSlewDirections(); ??? Do we really need to do this?
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

		private JogRates _secondaryJogRates;

		public JogRates SecondaryJogRates
		{
			get { return _secondaryJogRates; }
			set
			{
				if ( value != _secondaryJogRates )
				{
					_secondaryJogRates = value;
					OnPropertyChanged();
				}
			}
		}

		private JogRate _selectedSecondaryJogRate;

		public JogRate SelectedSecondaryJogRate
		{
			get { return _selectedSecondaryJogRate; }
			set
			{
				if ( value != _selectedSecondaryJogRate )
				{
					_selectedSecondaryJogRate = value;
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

		private JogAmount _selectedSlewAmount;

		public JogAmount SelectedSlewAmount
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

		private ObservableCollection<JogDirection> _jogDirections;

		public ObservableCollection<JogDirection> JogDirections
		{
			get
			{
				if ( TelescopeManager != null && _jogDirections == null )
				{
					_jogDirections = TelescopeManager.JogDirections;
				}

				return _jogDirections; 
			}
			set
			{
				if ( value != _jogDirections )
				{
					_jogDirections = value;
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

		private bool _hasAsymmetricJogRates;

		public bool HasAsymmetricJogRates
		{
			get { return _hasAsymmetricJogRates; }
			set
			{
				if ( value != _hasAsymmetricJogRates )
				{
					_hasAsymmetricJogRates = value;
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
						bool canMoveAxis = Capabilities.CanMovePrimaryAxis && Capabilities.CanMoveSecondaryAxis
									&& Capabilities.PrimaryAxisRates != null && Capabilities.PrimaryAxisRates.Length > 0
									&& Capabilities.SecondaryAxisRates != null && Capabilities.SecondaryAxisRates.Length > 0;

						CanMoveAxis = canMoveAxis;

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

		private void BuildJogRatesLists()
		{
			JogRates jogRates = null;
			JogRates secondaryJogRates = null;

			bool asymmetricRates = false;

			if ( Capabilities != null )
			{
				// We need to be careful because the jog rates can be null if the telescope does
				// not support MoveAxis!

				IRate[] primaryAxisRates = Capabilities.PrimaryAxisRates;
				jogRates = JogRates.FromAxisRates( primaryAxisRates );

				IRate[] secondaryAxisRates = Capabilities.SecondaryAxisRates;
				secondaryJogRates = JogRates.FromAxisRates( secondaryAxisRates );

				// If we cannot move both primary and secondary axes then we cannot have asymmetric rates.
				// This shows up in the jog rates being null.

				if ( jogRates != null && secondaryJogRates != null )
				{
					if ( jogRates.Count == secondaryJogRates.Count )
					{
						for ( int i = 0; i < jogRates.Count; ++i )
						{
							if ( jogRates[i].Rate != secondaryJogRates[i].Rate )
							{
								asymmetricRates = true;
								break;
							}
						}
					}
					else
					{
						asymmetricRates = true;
					}
				}
			}

			if ( jogRates == null )
			{
				JogRates = null;
				SelectedJogRate = null;

				SecondaryJogRates = null;
				SelectedSecondaryJogRate = null;

				HasAsymmetricJogRates = false;
			}
			else
			{
				JogRates = jogRates;
				SelectedJogRate = jogRates[0];

				if ( asymmetricRates )
				{
					SecondaryJogRates = secondaryJogRates;
					SelectedSecondaryJogRate = secondaryJogRates[0];
				}
				else
				{
					SecondaryJogRates = null;
					SelectedSecondaryJogRate = null;
				}

				HasAsymmetricJogRates = asymmetricRates;
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
					BuildJogRatesLists();
				}

				if ( JogDirections == null )
				{
					Task.Factory.StartNew( () =>
					{
						JogDirections = TelescopeManager.JogDirections;
					}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
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
					BuildJogRatesLists();
					IsTracking = false;
					JogDirections = null;
				}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
			}
		}

		private void SetSlewDirections()
		{
			JogDirections = new ObservableCollection<JogDirection>()
			{
				new JogDirection { Name = "N", Description = "North", MoveDirection = MoveDirections.North, Axis = TelescopeAxes.axisSecondary, RateSign = 1.0 },
				new JogDirection { Name = "S", Description = "South", MoveDirection = MoveDirections.South, Axis = TelescopeAxes.axisSecondary, RateSign = -1.0  },
				new JogDirection { Name = "W", Description = "West", MoveDirection = MoveDirections.West, Axis = TelescopeAxes.axisPrimary, RateSign = 1.0  },
				new JogDirection { Name = "E", Description = "East", MoveDirection = MoveDirections.East, Axis = TelescopeAxes.axisPrimary, RateSign = -1.0  }
			};

			try
			{
				if ( TelescopeManager.IsConnected )
				{
					if ( Parameters.AlignmentMode == AlignmentModes.algAltAz && IsVariableJog )
					{
						JogDirections.Clear();

						JogDirections.Add( new JogDirection { Name = "U", Description = "Up", MoveDirection = MoveDirections.Up, Axis = TelescopeAxes.axisSecondary, RateSign = 1.0 } );
						JogDirections.Add( new JogDirection { Name = "D", Description = "Down", MoveDirection = MoveDirections.Down, Axis = TelescopeAxes.axisSecondary, RateSign = -1.0 } );
						JogDirections.Add( new JogDirection { Name = "L", Description = "Left", MoveDirection = MoveDirections.Left, Axis = TelescopeAxes.axisPrimary, RateSign = -1.0 } );
						JogDirections.Add( new JogDirection { Name = "R", Description = "Right", MoveDirection = MoveDirections.Right, Axis = TelescopeAxes.axisPrimary, RateSign = 1.0 } );
					}
					else if ( Parameters.SiteLatitude < 0 )
					{
						JogDirections.Clear();

						JogDirections.Add( new JogDirection { Name = "S", Description = "South", MoveDirection = MoveDirections.South, Axis = TelescopeAxes.axisSecondary, RateSign = 1.0 } );
						JogDirections.Add( new JogDirection { Name = "N", Description = "North", MoveDirection = MoveDirections.North, Axis = TelescopeAxes.axisSecondary, RateSign = -1.0 } );
						JogDirections.Add( new JogDirection { Name = "W", Description = "West", MoveDirection = MoveDirections.West, Axis = TelescopeAxes.axisPrimary, RateSign = -1.0 } );
						JogDirections.Add( new JogDirection { Name = "E", Description = "East", MoveDirection = MoveDirections.East, Axis = TelescopeAxes.axisPrimary, RateSign = 1.0 } );
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
			int ndx = Int32.Parse( (string)param );
			Debug.WriteLine( $"Start Move parameter is {param}." );
			MoveDirections direction = JogDirections[ndx].MoveDirection;
			Debug.WriteLine( $"Start Move direction is {direction}." );

			double rate;

			if ( IsVariableJog )
			{
				rate = SelectedJogRate.Rate;

				// If the telescope has different rates for the secondary (Dec or Altitude) axis, use them.

				if ( HasAsymmetricJogRates &&
					(   direction == MoveDirections.Up || direction == MoveDirections.Down
					 || direction == MoveDirections.North || direction == MoveDirections.South ) )
				{
					rate = SelectedSecondaryJogRate.Rate;
				}

				try
				{
					TelescopeManager.StartJogScope( ndx, rate );
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
			int ndx = Int32.Parse( (string)param );

			if ( IsVariableJog && _jogInProgress )
			{
				TelescopeManager.StopJogScope( ndx );
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
			// Param is index into JogDirections collection, as a string.

			if ( !IsVariableJog )
			{
				//MoveDirections direction = ( param == null ) ? MoveDirections.None : (MoveDirections)param;

				if ( param != null )
				{
					int ndx = Int32.Parse( (string)param );

					TelescopeManager.StartFixedSlew( ndx, SelectedSlewAmount.Amount );

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
			TelescopeManager.SetParkPosition();

			IMessageBoxService msgSvc = ServiceContainer.Instance.GetService<IMessageBoxService>();

			string text = "The current telescope position has been remembered as the Park position.";
			string title = "Set ParkPosition";

			msgSvc.Show( text, title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None );
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