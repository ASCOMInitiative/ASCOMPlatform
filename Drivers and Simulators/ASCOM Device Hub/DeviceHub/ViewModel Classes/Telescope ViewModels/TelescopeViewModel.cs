using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public class TelescopeViewModel : DeviceHubViewModelBase
    {
		private readonly ITelescopeManager _telescopeManager;
        private ITelescopeManager TelescopeManager { get => _telescopeManager; }

		public TelescopeViewModel( ITelescopeManager telescopeManager )
		{
			_telescopeManager = telescopeManager;
			_isConnected = false;
			_status = null;
			_isSlewInProgress = false;

			ParametersVm = new TelescopeParametersViewModel();
			CapabilitiesVm = new TelescopeCapabilitiesViewModel();
			TrackingRatesVm = new TelescopeTrackingRatesViewModel( telescopeManager );
			DirectSlewVm = new TelescopeDirectSlewViewModel( telescopeManager );
			MotionVm = new TelescopeMotionViewModel( telescopeManager );

			Messenger.Default.Register<ObjectCountMessage>( this, ( action ) => UpdateObjectsCount( action ) );
			Messenger.Default.Register<TelescopeIDChangedMessage>( this, ( action ) => TelescopeIDChanged( action ) );
			Messenger.Default.Register<SlewInProgressMessage>( this, ( action ) => UpdateSlewInProgress( action ) );
			RegisterStatusUpdateMessage( true );
		}

		#region Public Properties

		public TelescopeParametersViewModel ParametersVm { get; private set; }
		public TelescopeCapabilitiesViewModel CapabilitiesVm { get; private set; }
		public TelescopeTrackingRatesViewModel TrackingRatesVm { get; private set; }
		public TelescopeDirectSlewViewModel DirectSlewVm { get; private set; }
		public TelescopeMotionViewModel MotionVm { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public void ChangeActiveFunction( string functionName )
		{
			// Called from code-behind when the active tab item is changed.

			if ( functionName == "Motion" )
			{
				MotionVm.SetActive( true );
				DirectSlewVm.SetActive( false );
			}
			else if ( functionName == "Direct Slew" )
			{
				// When the user switches to the Direct Slew tab, we must syncronize the displayed RA and DEC
				// with the current RA and DEC.

				MotionVm.SetActive( false );
				DirectSlewVm.SetActive( true );
			}
			else if ( functionName == "Capabilities" )
			{
				MotionVm.SetActive( false );
				DirectSlewVm.SetActive( false );
			}
			else if ( functionName == "Static Properties" )
			{
				MotionVm.SetActive( false );
				DirectSlewVm.SetActive( false );
			}
			else if ( functionName == "Tracking Rates" )
			{
				MotionVm.SetActive( false );
				DirectSlewVm.SetActive( false );
			}
			else
			{
				string msg = String.Format( "TelescopeViewModel.ChangeActiveFunction called with invalid function Name - {0}", functionName );
				throw new ArgumentException( msg );
			}
		}

		#endregion Public Methods

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

		private string _telescopeID;

        public string TelescopeID
        {
            get { return _telescopeID; }
            set
            {
                if ( value != _telescopeID )
                {
                    _telescopeID = value;
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

		private bool _isSlewInProgress;

		public bool IsSlewInProgress
		{
			get { return _isSlewInProgress; }
			set
			{
				if ( value != _isSlewInProgress )
				{
					_isSlewInProgress = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Change Notification Properties

		#region Helper Methods

		private void TelescopeIDChanged( TelescopeIDChangedMessage msg )
        {
            // Make sure that we update the ID on the U/I thread.

            Task.Factory.StartNew( () => TelescopeID = msg.ID, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
        }

        private void ConnectTelescope()
        {
            try
			{
				// Attempt to connect with the scope.

				RegisterStatusUpdateMessage( true );

				bool success = TelescopeManager.Connect( TelescopeID );

				if ( success )
				{
					IsConnected = true;
				}
				else
				{
					// Init default message.

					string message = "Use the Activity Log to view any errors!";

					if ( TelescopeManager.ConnectException != null )
					{
						// Report the exception.

						message = TelescopeManager.ConnectException.Message;
					}

					ShowMessage( message, "Telescope Connection Error" );
				}
			}
			catch ( Exception xcp )
			{
				// Connection attempt caused exception.

				string message = String.Format( "{0}\r\n{1}", TelescopeManager.ConnectError, xcp.Message );
				ShowMessage( message, "Telescope Connection Error" );
			}
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

        private void UpdateStatus( TelescopeStatusUpdatedMessage action )
        {
			// This is a registered message handler. It could be called from a worker thread
			// and we need to be sure that the work is done on the U/I thread.

			Task.Factory.StartNew( () =>
			{
				Status = action.Status;
				IsConnected = Status.Connected;
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
        }

        private void DisconnectTelescope()
        {
			IsConnected = false;
			IsSlewInProgress = false;
			TelescopeManager.Disconnect();
			Status = null;
        }

		private void UpdateObjectsCount( ObjectCountMessage msg )
		{
			Task.Factory.StartNew( () =>
			{
				ObjectCount = msg.ScopeCount;
				HasActiveClients = msg.ScopeCount > 0;
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		private void UpdateSlewInProgress( SlewInProgressMessage action )
		{
			Task.Factory.StartNew( () =>
			{
				IsSlewInProgress = action.IsSlewInProgress;
			}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}


		#endregion Helper Methods

		#region Relay Commands

		#region ToggleTelescopeConnectedCommand

		private ICommand _toggleTelescopeConnectedCommand;

        public ICommand ToggleTelescopeConnectedCommand
        {
            get
            {
                if ( _toggleTelescopeConnectedCommand == null )
                {
                    _toggleTelescopeConnectedCommand = new RelayCommand(
                        param => this.ToggleTelescopeConnected(),
                        param => this.CanToggleTelescopeConnected() );
                }

                return _toggleTelescopeConnectedCommand;
            }
        }

        private void ToggleTelescopeConnected()
        {
            if ( !IsConnected )
            {
                ConnectTelescope();
            }
            else
            {
                DisconnectTelescope();
            }
        }

        private bool CanToggleTelescopeConnected()
        {
            return ( TelescopeID != null );
        }

		#endregion ToggleTelescopeConnectedCommand

		#endregion Relay Commands

		#region IDisposable override

		protected override void DoDispose()
		{
			Messenger.Default.Unregister<ObjectCountMessage>( this );
			Messenger.Default.Unregister<TelescopeIDChangedMessage>( this );
			Messenger.Default.Unregister<SlewInProgressMessage>( this );
			RegisterStatusUpdateMessage( false );

			_toggleTelescopeConnectedCommand = null;

			ParametersVm.Dispose();
			ParametersVm = null;
			CapabilitiesVm.Dispose();
			CapabilitiesVm = null;
			TrackingRatesVm.Dispose();
			TrackingRatesVm = null;
			DirectSlewVm.Dispose();
			DirectSlewVm = null;
			MotionVm.Dispose();
			MotionVm = null;
		}

		#endregion IDisposable override
	}
}
