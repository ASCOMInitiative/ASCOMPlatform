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
			FocuserManager = focuserManager;
			_isConnected = false;
			_status = null;

			ParametersVm = new FocuserParametersViewModel();
			ControlVm = new FocuserControlViewModel( FocuserManager );

			Messenger.Default.Register<ObjectCountMessage>( this, ( action ) => UpdateObjectsCount( action ) );
			Messenger.Default.Register<FocuserIDChangedMessage>( this, ( action ) => FocuserIDChanged( action ) );
			RegisterStatusUpdateMessage( true );

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
			try
			{
				// Attempt to connect with the scope.

				RegisterStatusUpdateMessage( true );

				bool success = FocuserManager.Connect( FocuserID );

				if ( success )
				{
					IsConnected = true;
				}
				else
				{
					// No exception, but did not connect!

					string message = "Use the Activity Log to view any errors!";

					if ( FocuserManager.ConnectException != null )
					{
						message = FocuserManager.ConnectException.Message;
					}

					ShowMessage( message, "Focuser Connection Error" );
				}
			}
			catch ( Exception xcp )
			{
				// Connection attempt caused exception.

				string message = String.Format( "{0}\r\n{1}", FocuserManager.ConnectError, xcp.Message );
				ShowMessage( message, "Focuser Connection Error" );
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
			IsConnected = false;
			FocuserManager.Disconnect();
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
			return ( FocuserID != null );
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
