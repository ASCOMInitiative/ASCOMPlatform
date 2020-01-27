using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class DomeViewModel : DeviceHubViewModelBase
    {
		private IDomeManager DomeManager { get; }

		public DomeViewModel( IDomeManager domeManager )
		{
			DomeManager = domeManager;
			_isConnected = false;
			_status = null;

			ParametersVm = new DomeParametersViewModel();
			CapabilitiesVm = new DomeCapabilitiesViewModel();
			MotionVm = new DomeMotionViewModel( DomeManager );

			Messenger.Default.Register<ObjectCountMessage>( this, ( action ) => UpdateObjectsCount( action ) );
			Messenger.Default.Register<DomeIDChangedMessage>( this, ( action ) => DomeIDChanged( action ) );
			RegisterStatusUpdateMessage( true );

		}

		#region Public Properties

		public DomeParametersViewModel ParametersVm { get; private set; }
		public DomeCapabilitiesViewModel CapabilitiesVm { get; private set; }
		public DomeMotionViewModel MotionVm { get; private set; }

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

		private string _domeID;

		public string DomeID
		{
			get { return _domeID; }
			set
			{
				if ( value != _domeID )
				{
					_domeID = value;
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

		private AscomDomeStatus _status;

		public AscomDomeStatus Status
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

		private void DomeIDChanged( DomeIDChangedMessage msg )
		{
			// Make sure that we update the ID on the U/I thread.

			Task.Factory.StartNew( () => DomeID = msg.ID, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void ConnectDome()
		{
			try
			{
				// Attempt to connect with the scope.

				RegisterStatusUpdateMessage( true );

				bool success = DomeManager.Connect( DomeID );

				if ( success )
				{
					IsConnected = true;
				}
				else
				{
					// No exception, but did not connect!

					//string message = String.Format( "{0}\r\n{1}", DomeManager.ConnectError, DomeManager.ConnectException.Message );
					//ShowMessage( message, "Dome Connection Error" );
					string message = "Use the Activity Log to view any errors!";

					if ( DomeManager.ConnectException != null )
					{
						message = DomeManager.ConnectException.Message;
					}

					ShowMessage( message, "Dome Connection Error" );
				}
			}
			catch ( Exception xcp )
			{
				// Connection attempt caused exception.

				string message = String.Format( "{0}\r\n{1}", DomeManager.ConnectError, xcp.Message );
				ShowMessage( message, "Dome Connection Error" );
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
				IsConnected = Status.Connected;
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void DisconnectDome()
		{
			IsConnected = false;
			DomeManager.Disconnect();
			Status = null;
		}

		private void UpdateObjectsCount( ObjectCountMessage msg )
		{
			Task.Factory.StartNew( () =>
			{
				ObjectCount = msg.DomeCount;
				HasActiveClients = ObjectCount > 0;
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		#endregion Helper Methods

		#region Relay Commands

		#region ToggleDomeConnectedCommand

		private ICommand _toggleDomeConnectedCommand;

		public ICommand ToggleDomeConnectedCommand
		{
			get
			{
				if ( _toggleDomeConnectedCommand == null )
				{
					_toggleDomeConnectedCommand = new RelayCommand(
						param => this.ToggleDomeConnected(),
						param => this.CanToggleDomeConnected() );
				}

				return _toggleDomeConnectedCommand;
			}
		}

		private void ToggleDomeConnected()
		{
			if ( !IsConnected )
			{
				ConnectDome();
			}
			else
			{
				DisconnectDome();
			}
		}

		private bool CanToggleDomeConnected()
		{
			return ( DomeID != null );
		}

		#endregion ToggleDomeConnectedCommand

		#endregion Relay Commands

		#region IDisposable override

		protected override void DoDispose()
		{
			Messenger.Default.Unregister<ObjectCountMessage>( this );
			Messenger.Default.Unregister<DomeIDChangedMessage>( this );
			Messenger.Default.Unregister<DomeCapabilitiesUpdatedMessage>( this );
			Messenger.Default.Unregister<DomeParametersUpdatedMessage>( this );
			RegisterStatusUpdateMessage( false );

			_toggleDomeConnectedCommand = null;

			ParametersVm.Dispose();
			ParametersVm = null;
			CapabilitiesVm.Dispose();
			CapabilitiesVm = null;
			MotionVm.Dispose();
			MotionVm = null;
		}

		#endregion IDisposable override
	}
}
