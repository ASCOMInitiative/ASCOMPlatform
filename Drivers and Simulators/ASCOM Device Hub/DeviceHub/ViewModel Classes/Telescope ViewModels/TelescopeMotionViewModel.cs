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
		private const string _errorCaption = "Telescope Driver Error";

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
			string caller = "TelescopeMotionViewModel ctor";

			LogAppMessage( "Initializing Instance constructor", caller );

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

			LogAppMessage( "Registering message handlers.", caller );

			Messenger.Default.Register<TelescopeCapabilitiesUpdatedMessage>( this, ( action ) => TelescopeCapabilitiesUpdated( action ) );
			Messenger.Default.Register<TelescopeParametersUpdatedMessage>( this, ( action ) => TelescopeParametersUpdated( action ) );
			Messenger.Default.Register<DeviceDisconnectedMessage>( this, ( action ) => InvalidateDeviceData( action ) );
			RegisterStatusUpdateMessage( true );

			LogAppMessage( "Instance constructor initialization complete.", caller );
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

				IsTracking = ( Status!= null && Status.Connected ) && Status.Tracking;

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

		private bool ChangeTracking()
		{
			bool retval = true;

			Status.Tracking = _isTracking;
			string desiredState = _isTracking ? "ON" : "OFF";

			try
			{
				TelescopeManager.SetTracking( _isTracking );
			}
			catch ( Exception xcp )
			{
				string msg = $"The telescope driver returned an error when attempting to set tracking to {desiredState}. ";
				msg += $"Details follow:\r\n\r\n{xcp}";

				ShowMessage( msg, _errorCaption );

				retval = false;
			}

			return retval;	// We only care about the return value when unit testing.
		}

		private bool CanChangeTracking()
		{
			bool retval = false;

			if  ( Capabilities != null && Capabilities.CanSetTracking
					&& Status != null && Status.Connected && Status.ParkingState == ParkingStateEnum.Unparked )
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

		private bool ChangeParkState()
		{
			bool retval = true;

			ParkingStateEnum newState = ( Status.ParkingState == ParkingStateEnum.Unparked ) ? ParkingStateEnum.IsAtPark : ParkingStateEnum.Unparked;
			string stateName = ( newState == ParkingStateEnum.Unparked ) ? "UNPARK" : "PARK";

			if ( newState == ParkingStateEnum.IsAtPark )
			{
				IMessageBoxService msgSvc = ServiceContainer.Instance.GetService<IMessageBoxService>();

				string text = "Once you start to park the telescope it will not be abortable and must be allowed to complete. \r\n\r\n"
					+ "Click OK to Park the telescope?";
				string title = "Park The Telescope";

				MessageBoxResult result = msgSvc.Show( text, title, MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel, MessageBoxOptions.None );

				if ( result != MessageBoxResult.OK )
				{
					return retval;
				}
			}

			try
			{
				TelescopeManager.SetParkingState( newState );
			}
			catch ( Exception xcp )
			{
				string msg = $"The telescope driver returned an error when attempting to {stateName} the scope. ";
				msg += $"Details follow:\r\n\r\n{xcp}";

				ShowMessage( msg, _errorCaption );

				retval = false;
			}

			return retval;  // We only care about the return value when unit testing.
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

		private bool DoMeridianFlip()
		{
			bool retval = true;

			try
			{
				TelescopeManager.StartMeridianFlip();
			}
			catch ( Exception xcp )
			{
				string msg = $"The telescope driver returned an error when attempting to perform a meridian flip. ";
				msg += $"Details follow:\r\n\r\n{xcp}";

				ShowMessage( msg, _errorCaption );

				retval = false;
			}

			return retval;
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

		private bool StartMove( object param )
		{
			bool retval = true;

			int ndx = Int32.Parse( (string)param );
			//Debug.WriteLine( $"Start Move parameter is {param}." );
			MoveDirections direction = JogDirections[ndx].MoveDirection;
			//Debug.WriteLine( $"Start Move direction is {direction}." );

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
				catch ( InvalidValueException xcp )
				{
					string msg = $"Unable to use the requested Jog rate.\r\n\r\n{xcp.Message}";
					ShowMessage( msg, "Unsupported Jog Rate Selected" );
				}
				catch ( Exception xcp )
				{
					string msg = "The telescope driver returned an error when trying to jog the scope. Details follow:\r\n\r\n"
						+ xcp.ToString();
					ShowMessage( msg, _errorCaption );

					retval = false;
				}
			}

			return retval;  // We only care about the return value when unit testing.
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

		private bool StopMotion( object param )
		{
			bool retval = true;

			int ndx = Int32.Parse( (string)param );

			if ( IsVariableJog && _jogInProgress )
			{
				try
				{
					TelescopeManager.StopJogScope( ndx );
				}
				catch ( Exception xcp )
				{
					string msg = "The telescope driver returned an error when attempting to stop jogging the telescope. "
								+ $"Details follow:\r\n\r\n{xcp}";
					ShowMessage( msg, _errorCaption );

					retval = false;
				}
			}

			return retval;  // We only care about the return value when unit testing.
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

		private bool DoFixedSlew( object param )
		{
			bool retval = true;

			// Param is index into JogDirections collection, as a string.

			if ( !IsVariableJog )
			{
				//MoveDirections direction = ( param == null ) ? MoveDirections.None : (MoveDirections)param;

				if ( param != null )
				{
					int ndx = Int32.Parse( (string)param );

					try
					{
						TelescopeManager.StartFixedSlew( ndx, SelectedSlewAmount.Amount );

						_isFixedSlewInProgress = true;
					}
					catch ( Exception xcp )
					{
						string msg = "The telescope driver returned an error when trying to start a fixed slew. "
							+ $"Details follow:\r\n\r\n{xcp}";
						ShowMessage( msg, _errorCaption );

						retval = false;
					}
				}
			}

			return retval;  // We only care about the return value when unit testing.
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

		private bool SetParkPosition()
		{
			bool retval = true;

			try
			{
				TelescopeManager.SetParkPosition();

				IMessageBoxService msgSvc = ServiceContainer.Instance.GetService<IMessageBoxService>();

				string text = "The current telescope position has been remembered as the Park position.";
				string title = "Set ParkPosition";

				msgSvc.Show( text, title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None );
			}
			catch ( Exception xcp )
			{
				string msg = "The telescope driver returned an error when trying to set the Park position. "
					+ $"Details follow:\r\n\r\n{xcp}";
				ShowMessage( msg, _errorCaption );

				retval = false;
			}

			return retval;  // We only care about the return value when unit testing.
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