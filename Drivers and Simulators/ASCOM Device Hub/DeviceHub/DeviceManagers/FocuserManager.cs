using System;
using System.Threading;
using System.Threading.Tasks;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public partial class FocuserManager : DeviceManagerBase, IFocuserManager, IDisposable
	{
		#region Static Constructor, Properties, Fields, and Methods

		private const int POLLING_INTERVAL_NORMAL = 5000;	// once every 5 seconds
		private const int POLLING_INTERVAL_FAST = 500;		// once every 1/2 second
		private const int POLLING_INTERVAL_SLOW = 10000;	// once every 10 seconds

		public static string FocuserID { get; set; }

		private static FocuserManager _instance = null;

		public static FocuserManager Instance
		{
			get
			{
				if ( _instance == null )
				{
					_instance = new FocuserManager();
				}

				return _instance;
			}

			private set => _instance = value;
		}

		static FocuserManager()
		{
			FocuserID = "";
		}

		public static void SetFocuserID( string id )
		{
			FocuserID = id;
			Messenger.Default.Send( new FocuserIDChangedMessage( id ) );
		}

		#endregion Static Constructor, Properties, Fields, and Methods

		#region Private Properties

		private bool DeviceCreated => Service.DeviceCreated;
		private bool DeviceAvailable => Service.DeviceAvailable;

		private CancellationTokenSource PollingTokenSource { get; set; }
		private Task PollingTask { get; set; }
		private bool IsPolling { get; set; }
		private int PollingInterval { get; set; }
		private ManualResetEvent PollingWake { get; set; }

		private CancellationTokenSource MoveCancelTokenSource { get; set; }
		private CancellationTokenSource PollRateChangeTokenSource { get; set; }
		private bool ReEnableTempComp { get; set; }
		private bool MoveInProgress { get; set; }

		#endregion Private Properties

		#region Instance Constructor

		public FocuserManager()
				: base( DeviceTypeEnum.Focuser )
		{
			IsConnected = false;
			Parameters = null;
			Status = null;
			PollingTask = null;
			ReEnableTempComp = false;
			MoveInProgress = false;
			PollingInterval = POLLING_INTERVAL_NORMAL;
			PollingWake = new ManualResetEvent( false );
		}

		#endregion Instance Constructor

		#region Public Properties

		public bool IsConnected { get; private set; }

		public string ConnectError { get; protected set; }
		public Exception ConnectException { get; protected set; }

		public FocuserParameters Parameters { get; private set; }
		public DevHubFocuserStatus Status { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public bool Connect()
		{
			return Connect( FocuserID );
		}

		public bool Connect( string focuserID )
		{
			ConnectError = "";
			ConnectException = null;

			// Don't re-create the service if it is already created for the same device.

			if ( Service == null || !Service.DeviceCreated || focuserID != FocuserID )
			{
				try
				{
					InitializeFocuserService( focuserID );
					SetFocuserID( focuserID );
				}
				catch ( Exception xcp )
				{
					ConnectError = "Unable to create focuser object.";
					ConnectException = xcp;
					ReleaseFocuserService();

					return false;
				}
			}

			// If another client app is active, then the focuser may already be connected!!

			Exception tempXcp = null;

			try
			{
				// Are we already connected?

				if ( !Connected )
				{
					// Not connected, so set connected to true.

					Connected = true;
				}

				IsConnected = Connected;
			}
			catch ( Exception xcp )
			{
				IsConnected = false;
				tempXcp = xcp;
			}

			if ( !IsConnected )
			{
				ConnectError = "Unable to connect to the focuser!";
				ConnectException = tempXcp;
				ReleaseFocuserService();

				return false;
			}

			// According to the ASCOM docs, IsConnected should never be false it this point.  If there was
			// a problem connecting an exception should have been thrown!

			bool retval = false;

			if ( IsConnected )
			{
				try
				{
					Task task = Task.Run( () =>
					{
						ReadInitialFocuserDataTask();
						StartDevicePolling();
					} );

					task.Wait();
				}
				catch ( Exception )
				{
					Connected = false;
					IsConnected = false;
					ReleaseFocuserService();
				}

				retval = Connected;
			}

			return retval;
		}

		public void Disconnect()
		{
			if ( !DeviceCreated )
			{
				return;
			}

			if ( IsConnected )
			{
				StopDevicePolling();

				try
				{
					Connected = false;
				}
				catch ( Exception )
				{ }
				finally
				{
					IsConnected = false;
					Messenger.Default.Send( new DeviceDisconnectedMessage( DeviceTypeEnum.Focuser ) );
					ReleaseFocuserService();
				}
			}
		}

		public void MoveFocuserBy( int amount )
		{
			int maxIncrement = Parameters.MaxIncrement;
			int maxStep = Parameters.MaxStep;
			short interfaceVersion = Parameters.InterfaceVersion;
			bool tempCompOn = Status.TempComp;

			// Ensure that our move amount does not exceed the MaxIncrement.

			int moveAmount = Math.Max( Math.Min( amount, maxIncrement ), -MaxIncrement );

			if ( Parameters.Absolute )
			{
				moveAmount += Position;

				// Make sure that we do not exceed the maximum allowable position (either positive or negative).

				moveAmount = Math.Max( Math.Min( moveAmount, maxStep ), -maxStep );

				if ( PollingInterval != POLLING_INTERVAL_FAST )
				{
					PollingInterval = POLLING_INTERVAL_FAST;
					PollingWake.Set();
				}
			}

			// If the driver is earlier than IFocuserV3 and temp comp is on then we need to disable it for the move.

			if ( interfaceVersion < 3 && tempCompOn )
			{
				Service.TempComp = false;
				ReEnableTempComp = true;
			}

			Move( moveAmount );

			Status.IsMoving = true;

			MoveInProgress = true;
		}

		public void HaltFocuser()
		{
			if ( IsMoving )
			{
				// Stop the focuser and cancel any move that we have in progress.

				Halt();
				Status.IsMoving = false;

				if ( MoveCancelTokenSource != null )
				{
					MoveCancelTokenSource.Cancel();
				}
			}
		}

		public void SetTemperatureCompensation( bool state )
		{
			if ( Parameters.TempCompAvailable )
			{
				TempComp = state;
			}
		}

		#endregion Public Methods

		#region Helper Methods

		private void ReadInitialFocuserDataTask()
		{
			// This task talks to the focuser from a worker thread, but updates the U/I on the main thread.

			// Wait a second for the focuser to settle before reading the data.

			Thread.Sleep( 1000 );

			Parameters = new FocuserParameters();
			Parameters.InitializeFromManager( this );

			Status = new DevHubFocuserStatus( this );

			Messenger.Default.Send( new FocuserParametersUpdatedMessage( Parameters.Clone() ) );
			Messenger.Default.Send( new FocuserStatusUpdatedMessage( Status ) );
		}

		private void StartDevicePolling()
		{
			if ( !DeviceAvailable )
			{
				return;
			}

			if ( !IsPolling )
			{
				// We need to be able to interrupt the polling task whenever the polling rate changes,
				// so add a cancellation token for that.

				PollingTokenSource = new CancellationTokenSource();
				PollRateChangeTokenSource = new CancellationTokenSource();

				CancellationToken[] tokens = new CancellationToken[]
				{
					PollingTokenSource.Token
					, PollRateChangeTokenSource.Token
				};

				CancellationTokenSource ctsLinked = CancellationTokenSource.CreateLinkedTokenSource( tokens );
				PollingTask = Task.Factory.StartNew( () => PollFocuserTask( PollingTokenSource.Token )
												, ctsLinked.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default );
			}
		}

		private void PollFocuserTask( CancellationToken token )
		{
			IsPolling = true;
			bool taskCancelled = token.IsCancellationRequested;
			WaitHandle[] waitHandles = new WaitHandle[] { token.WaitHandle, PollingWake };

			while ( !taskCancelled )
			{
				if ( Service.DeviceAvailable )
				{
					if ( MoveInProgress && !Status.IsMoving )
					{
						MoveInProgress = false;

						if ( ReEnableTempComp )
						{
							Service.TempComp = true;
							ReEnableTempComp = false;
						}

						UpdateFocuserStatusTask();

						Messenger.Default.Send( new FocuserMoveCompletedMessage() );
					}
					else
					{
						UpdateFocuserStatusTask();
					}
				}

				PollingInterval = MoveInProgress ? POLLING_INTERVAL_FAST : POLLING_INTERVAL_NORMAL;

				// Wait until the polling interval has expired, we have been cancelled, or we have been
				// awakened early because the polling interval has been changed.

				int index = WaitHandle.WaitAny( waitHandles, PollingInterval );

				if ( index == 0 )
				{
					taskCancelled = true;
				}
				else if ( index == 1 )
				{
					// We have been awakened externally; perhaps just to change the polling rate.

					PollingWake.Reset();
				}
				else if ( index == WaitHandle.WaitTimeout )
				{
					// The polling interval has expired.
				}
			}

			IsPolling = false;
		}

		private void UpdateFocuserStatusTask()
		{
			DevHubFocuserStatus sts = null;
			Exception except = null;

			try
			{
				sts = new DevHubFocuserStatus( this );
			}
			catch ( Exception xcp )
			{
				string msg = xcp.ToString();
				except = xcp;
			}

			if ( sts == null )
			{
				LogActivityLine( ActivityMessageTypes.Status, "Attempting to update the focuser status caught an exception. Details follow:" );
				LogActivityLine( ActivityMessageTypes.Status, except.Message );
			}
			else
			{
				Status = sts;

				Messenger.Default.Send( new FocuserStatusUpdatedMessage( sts ) );
			}
		}

		private void StopDevicePolling()
		{
			if ( !Service.DeviceAvailable || !Connected )
			{
				return;
			}

			if ( IsPolling )
			{
				PollingTokenSource.Cancel();
				PollingTask.Wait();
				PollingTokenSource.Dispose();
				PollingTokenSource = null;
				PollRateChangeTokenSource.Dispose();
				PollRateChangeTokenSource = null;
			}
		}

		#endregion Helper Methods
	}
}
