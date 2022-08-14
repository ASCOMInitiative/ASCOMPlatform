using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class FocuserViewModel : DeviceHubViewModelBase
	{
		private IFocuserManager FocuserManager { get; }

		public FocuserViewModel( IFocuserManager focuserManager )
		{
			string caller = "FocuserViewModel ctor";
			LogAppMessage( "Initializing Instance constructor", caller );

			FocuserManager = focuserManager;
			_isConnected = false;
			_status = null;

			LogAppMessage( "Creating child view models", caller );

			ParametersVm = new FocuserParametersViewModel();
			ControlVm = new FocuserControlViewModel( FocuserManager );

			LogAppMessage( "Registering message handlers", caller );

			Messenger.Default.Register<ObjectCountMessage>( this, ( action ) => UpdateObjectsCount( action ) );
			Messenger.Default.Register<FocuserIDChangedMessage>( this, ( action ) => FocuserIDChanged( action ) );
			Messenger.Default.Register<DeviceDisconnectedMessage>( this, ( action ) => DeviceDisconnected( action ) );
			RegisterStatusUpdateMessage( true );

			LogAppMessage( "Initialization complete", caller );
		}

		#region Public Properties

		public FocuserParametersViewModel ParametersVm { get; private set; }
		public FocuserControlViewModel ControlVm { get; private set; }

		public bool StartedByCOM => Server.StartedByCOM;

		#endregion Public Properties

		#region Change Notification Properties

		private int _objectCount;

		public int ObjectCount
		{
			get { return _objectCount; }
			set
			{
				if ( value != _objectCount )
				{
					_objectCount = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _hasActiveClients;

		public bool HasActiveClients
		{
			get { return _hasActiveClients; }
			set
			{
				if ( value != _hasActiveClients )
				{
					_hasActiveClients = value;
					OnPropertyChanged();
				}
			}
		}

		private string _focuserID;

		public string FocuserID
		{
			get { return _focuserID; }
			set
			{
				if ( value != _focuserID )
				{
					_focuserID = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isConnected;

		public bool IsConnected
		{
			get { return _isConnected; }
			set
			{
				if ( value != _isConnected )
				{
					_isConnected = value;
					OnPropertyChanged();
				}
			}
		}

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
				}
			}
		}

		#endregion Change Notification Properties

		#region Helper Methods

		private void FocuserIDChanged( FocuserIDChangedMessage msg )
		{
			// Make sure that we update the ID on the U/I thread.

			Task.Factory.StartNew( () => FocuserID = msg.ID, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void ConnectFocuser()
		{
			string message = null;

			try
			{
				// Attempt to connect with the scope.

				RegisterStatusUpdateMessage( true );

				bool success;

				// Connect can take  a few seconds, so signal the U/I to show a wait cursor.

				SignalWait( true );

				// Now do the connect.

				success = FocuserManager.Connect( FocuserID );

				if ( success )
				{
					IsConnected = true;
				}
				else
				{
					// No exception, but did not connect!

					message = "Use the Activity Log to view any errors!";

					if ( FocuserManager.ConnectException != null )
					{
						message = FocuserManager.ConnectException.Message;
					}
				}
			}
			catch ( Exception xcp )
			{
				// Connection attempt caused exception.

				message = $"{FocuserManager.ConnectError}\r\n{xcp.Message}";
			}
			finally
			{
				SignalWait( false );

				if ( message != null )
				{
					ShowMessage( message, "Telescope Connection Error" );
				}
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
				Status = action.Status;
				IsConnected = Status.Connected;
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void DisconnectFocuser()
		{
			// This is only called from the U/I

			IsConnected = false;

			try
			{
				SignalWait( true );
				FocuserManager.Disconnect( true );
			}
			finally
			{
				SignalWait( false );
			}

			Status = null;
		}

		private void UpdateObjectsCount( ObjectCountMessage msg )
		{
			Task.Factory.StartNew( () =>
			{
				ObjectCount = msg.FocuserCount;
				HasActiveClients = ObjectCount > 0;
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void DeviceDisconnected( DeviceDisconnectedMessage action )
		{
			if ( action.DeviceType == DeviceTypeEnum.Focuser )
			{
				Task.Factory.StartNew( () =>
				{
					IsConnected = false;
				}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
			}
		}

		#endregion Helper Methods

		#region Relay Commands

		#region ToggleFocuserConnectedCommand

		private ICommand _toggleFocuserConnectedCommand;

		public ICommand ToggleFocuserConnectedCommand
		{
			get
			{
				if ( _toggleFocuserConnectedCommand == null )
				{
					_toggleFocuserConnectedCommand = new RelayCommand(
						param => this.ToggleFocuserConnected(),
						param => this.CanToggleFocuserConnected() );
				}

				return _toggleFocuserConnectedCommand;
			}
		}

		private void ToggleFocuserConnected()
		{
			if ( !IsConnected )
			{
				ConnectFocuser();
			}
			else
			{
				DisconnectFocuser();
			}
		}

		private bool CanToggleFocuserConnected()
		{
			bool retval = false;

			if ( !IsConnected )
			{
				retval = FocuserID != null;
			}
			else
			{
				retval = Server.FocusersInUse == 0;
			}

			return retval;
		}

		#endregion ToggleFocuserConnectedCommand

		#endregion Relay Commands

		#region IDisposable override

		protected override void DoDispose()
		{
			Messenger.Default.Unregister<ObjectCountMessage>( this );
			Messenger.Default.Unregister<FocuserIDChangedMessage>( this );
			RegisterStatusUpdateMessage( false );

			_toggleFocuserConnectedCommand = null;

			ParametersVm.Dispose();
			ParametersVm = null;
			ControlVm.Dispose();
			ControlVm = null;
		}

		#endregion IDisposable override
	}
}
