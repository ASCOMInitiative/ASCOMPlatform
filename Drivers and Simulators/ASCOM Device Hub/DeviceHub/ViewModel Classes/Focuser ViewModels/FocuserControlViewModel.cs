using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class FocuserControlViewModel : DeviceHubViewModelBase
    {
		private readonly IFocuserManager _focuserManager;
		private IFocuserManager FocuserManager => _focuserManager;
		private Dictionary<double, int> FocuserMovements { get; set; }

		public FocuserControlViewModel( IFocuserManager focuserManager )
		{
			_focuserManager = focuserManager;
			_status = null;
			_temperatureDisplayDegF = false;

			FocuserMovements = new Dictionary<double, int>
			{
				{ 6.0, 1000 },
				{ 5.0, 400 },
				{ 4.0, 100 },
				{ 3.0, 40 },
				{ 2.0, 10 },
				{ 1.0, 4 },
				{ 0.0, 1 }
			};

			MoveIndex = FocuserMovements.Count - 1;
			TemperatureOffset = Globals.FocuserTemperatureOffset;
			TargetPosition = "0";

			Messenger.Default.Register<FocuserParametersUpdatedMessage>( this, ( action ) => FocuserParametersUpdated( action ) );
			Messenger.Default.Register<DeviceDisconnectedMessage>( this, ( action ) => InvalidateDeviceData( action ) );
			Messenger.Default.Register<FocuserMoveCompletedMessage>( this, ( action ) => FocuserMoveCompleted() );
			RegisterStatusUpdateMessage( true );
		}

		#region Change Notification Properties

		private DevHubFocuserStatus _status;

		public DevHubFocuserStatus Status
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

		private FocuserParameters _parameters;

		public FocuserParameters Parameters
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

		private bool? _temperatureDisplayDegF;

		public bool? TemperatureDisplayDegF
		{
			get { return _temperatureDisplayDegF; }
			set
			{
				if ( value != _temperatureDisplayDegF )
				{
					_temperatureDisplayDegF = value;
					OnPropertyChanged();
				}
			}
		}

		private double _maximumMoveIndex;

		public double MaximumMoveIndex
		{
			get { return _maximumMoveIndex; }
			set
			{
				if ( value != _maximumMoveIndex )
				{
					_maximumMoveIndex = value;
					OnPropertyChanged();
				}
			}
		}

		private double _moveIndex;

		public double MoveIndex
		{
			get { return _moveIndex; }
			set
			{
				if ( value != _moveIndex )
				{
					_moveIndex = value;
					OnPropertyChanged();

					MoveIncrement = GetMoveIncrement( _moveIndex );
				}
			}
		}

		private int _moveIncrement;

		public int MoveIncrement
		{
			get { return _moveIncrement; }
			set
			{
				if ( value != _moveIncrement )
				{
					_moveIncrement = value;
					OnPropertyChanged();
				}
			}
		}

		private string _targetPosition;

		public string TargetPosition
		{
			get { return _targetPosition; }
			set
			{
				if ( value != _targetPosition )
				{
					_targetPosition = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _focuserBusy;

		public bool FocuserBusy
		{
			get { return _focuserBusy; }
			set
			{
				if ( value != _focuserBusy )
				{
					_focuserBusy = value;
					OnPropertyChanged();
				}
			}
		}

		private double _temperatureOffset;

		public double TemperatureOffset
		{
			get { return _temperatureOffset; }
			set
			{
				if ( value != _temperatureOffset )
				{
					_temperatureOffset = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Helper Methods

		private void InvalidateDeviceData( DeviceDisconnectedMessage action )
		{
			if ( action.DeviceType == DeviceTypeEnum.Focuser )
			{
				Task.Factory.StartNew( () =>
				{
					Status = null;
					Parameters = null;
				}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
			}
		}

		private void RegisterStatusUpdateMessage( bool regUnreg )
		{
			if ( regUnreg )
			{
				Messenger.Default.Register<FocuserStatusUpdatedMessage>( this, ( action ) => UpdateStatus( action ) );
			}
			else
			{
				Messenger.Default.Unregister<FocuserStatusUpdatedMessage>( this );
			}
		}

		private void UpdateStatus( FocuserStatusUpdatedMessage action )
		{
			// This is a registered message handler. It could be called from a worker thread
			// and we need to be sure that the work is done on the U/I thread.

			Task.Factory.StartNew( () =>
			{
				if ( Status == null )
				{
					// This should happen once, after connection.

					TargetPosition = action.Status.Position.ToString();
					TemperatureOffset = Globals.FocuserTemperatureOffset;
				}

				Status = action.Status;
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void FocuserParametersUpdated( FocuserParametersUpdatedMessage action )
		{
			// Make sure that we update the Parameters on the U/I thread.

			Task.Factory.StartNew( () => Parameters = action.Parameters, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private int GetMoveIncrement( double moveIndex )
		{
			int moveAmount = FocuserMovements[0.0];

			if ( FocuserMovements.ContainsKey( moveIndex ) )
			{
				moveAmount = FocuserMovements[moveIndex];
			}

			return moveAmount;
		}

		protected void RequestFocuserMove( int delta )
		{
			FocuserBusy = true;

			int moveAmount = delta;

			FocuserManager.MoveFocuserBy( delta );
		}

		private void FocuserMoveCompleted()
		{
			Task.Factory.StartNew( () =>
			{
				FocuserBusy = false;
				TargetPosition = Status.Position.ToString();
				RelayCommand.RaiseCanExecuteChanged();
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void AdjustTargetPosition( int oldPosition )
		{
			if ( Status == null )
			{
				TargetPosition = String.Empty;
			}
			else if ( Status.Position != oldPosition && !Status.IsMoving )
			{
				TargetPosition = Status.Position.ToString();
			}
		}

		protected override void DoDispose()
		{
			_saveOffsetCommand = null;
			_moveFocuserInwardCommand = null;
			_haltFocuserCommand = null;

			Messenger.Default.Unregister<FocuserParametersUpdatedMessage>( this );
			Messenger.Default.Unregister<DeviceDisconnectedMessage>( this );
			Messenger.Default.Unregister<FocuserMoveCompletedMessage>( this );
			RegisterStatusUpdateMessage( false );
		}

		private bool IsValidTarget
		{
			get
			{
				bool retval = false;
				string target = TargetPosition;

				if ( !String.IsNullOrEmpty( target ) )
				{
					int temp;

					if ( Int32.TryParse( target, out temp ) )
					{
						retval = true;
					}
				}

				return retval;
			}
		}

		#endregion Helper Methods

		#region Relay Commands

		#region SaveOffsetCommand

		private ICommand _saveOffsetCommand;

		public ICommand SaveOffsetCommand
		{
			get
			{
				if ( _saveOffsetCommand == null )
				{
					_saveOffsetCommand = new RelayCommand(
						param => this.SaveOffset(),
						param => this.CanSaveOffset() );
				}

				return _saveOffsetCommand;
			}
		}

		private void SaveOffset()
		{
			Globals.FocuserTemperatureOffset = TemperatureOffset;
			AppSettingsManager.SaveAppSettings();
		}

		private bool CanSaveOffset()
		{
			return TemperatureOffset != Globals.FocuserTemperatureOffset;
		}

		#endregion  SaveOffsetCommand

		#region MoveFocuserInwardCommand

		private ICommand _moveFocuserInwardCommand;

		public ICommand MoveFocuserInwardCommand
		{
			get
			{
				if ( _moveFocuserInwardCommand == null )
				{
					_moveFocuserInwardCommand = new RelayCommand(
						param => this.MoveFocuserInward(),
						param => this.CanMoveFocuserInward() );
				}

				return _moveFocuserInwardCommand;
			}
		}

		private void MoveFocuserInward()
		{
			RequestFocuserMove( -MoveIncrement );
		}

		private bool CanMoveFocuserInward()
		{
			return FocuserManager.IsConnected && !FocuserBusy && Status != null;
		}

		#endregion MoveFocuserInwardCommand 

		#region MoveFocuserOutwardCommand

		private ICommand _moveFocuserOutwardCommand;

		public ICommand MoveFocuserOutwardCommand
		{
			get
			{
				if ( _moveFocuserOutwardCommand == null )
				{
					_moveFocuserOutwardCommand = new RelayCommand(
						param => this.MoveFocuserOutward(),
						param => this.CanMoveFocuserOutward() );
				}

				return _moveFocuserOutwardCommand;
			}
		}

		private void MoveFocuserOutward()
		{
			RequestFocuserMove( MoveIncrement );
		}

		private bool CanMoveFocuserOutward()
		{
			return FocuserManager.IsConnected && !FocuserBusy && Status != null;
		}

		#endregion MoveFocuserOutwardCommand

		#region MoveFocuserToPositionCommand

		private ICommand _moveFocuserToPositionCommand;

		public ICommand MoveFocuserToPositionCommand
		{
			get
			{
				if ( _moveFocuserToPositionCommand == null )
				{
					_moveFocuserToPositionCommand = new RelayCommand(
						param => this.MoveFocuserToPosition(),
						param => this.CanMoveFocuserToPosition() );
				}

				return _moveFocuserToPositionCommand;
			}
		}

		private void MoveFocuserToPosition()
		{
			int target = Int32.Parse( TargetPosition );
			int delta = target - Status.Position;

			RequestFocuserMove( delta );
		}

		private bool CanMoveFocuserToPosition()
		{
			return FocuserManager.IsConnected && !FocuserBusy && IsValidTarget;

		}

		#endregion MoveFocuserToPositionCommand

		#region HaltFocuserCommand

		private ICommand _haltFocuserCommand;

		public ICommand HaltFocuserCommand
		{
			get
			{
				if ( _haltFocuserCommand == null )
				{
					_haltFocuserCommand = new RelayCommand(
						param => this.HaltFocuser(),
						param => this.CanHaltFocuser() );
				}

				return _haltFocuserCommand;
			}
		}

		private void HaltFocuser()
		{
			FocuserManager.HaltFocuser();
		}

		private bool CanHaltFocuser()
		{
			return FocuserManager.IsConnected && FocuserBusy;
		}

		#endregion HaltFocuserCommand

		#region ToggleTempCompCommand

		private ICommand _toggleTempCompCommand;

		public ICommand ToggleTempCompCommand
		{
			get
			{
				if ( _toggleTempCompCommand == null )
				{
					_toggleTempCompCommand = new RelayCommand(
						param => this.ToggleTempComp( param ),
						param => this.CanToggleTempComp() );
				}

				return _toggleTempCompCommand;
			}
		}

		private void ToggleTempComp( object param)
		{
			bool state = (bool)param;

			FocuserManager.SetTemperatureCompensation( state );
		}

		private bool CanToggleTempComp()
		{
			bool retval = Status != null &&  Status.Connected 
						&& Parameters != null && Parameters.TempCompAvailable;

			return retval;
		}

		#endregion  ToggleTempCompCommand

		#endregion Relay Commands
	}
}