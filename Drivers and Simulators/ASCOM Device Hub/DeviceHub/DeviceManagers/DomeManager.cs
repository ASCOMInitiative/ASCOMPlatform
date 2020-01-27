using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using ASCOM.DeviceInterface;
using ASCOM.Astrometry.Transform;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public partial class DomeManager : DeviceManagerBase, IDomeManager, IDisposable
	{
		#region Static Constructor, Properties, Fields, and Methods

		private const int POLLING_INTERVAL_NORMAL = 5000;   // once every 5 seconds
		private const int POLLING_INTERVAL_FAST = 1000;      // once every second
		private const int POLLING_INTERVAL_SLOW = 10000;     // once every 10 seconds

		public static string DomeID { get; set; }

		private static DomeManager _instance = null;

		public static DomeManager Instance
		{
			get
			{
				if ( _instance == null )
				{
					_instance = new DomeManager();
				}

				return _instance;
			}

			private set => _instance = value;
		}

		static DomeManager()
		{
			DomeID = "";
		}

		public static void SetDomeID( string id )
		{
			DomeID = id;
			Messenger.Default.Send( new DomeIDChangedMessage( id ) );
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

		// We need telescope info in order to slave our position with the telescope.

		private DevHubTelescopeStatus TelescopeStatus { get; set; }
		private TelescopeParameters TelescopeParameters { get; set; }
		private SlewInProgressMessage SlavedSlewState { get; set; }

		#endregion Private Properties

		#region Instance Constructor

		private DomeManager()
			: base( DeviceTypeEnum.Dome )
		{
			IsConnected = false;
			Capabilities = null;
			Parameters = null;
			Status = null;
			PollingTask = null;
			ParkingState = ParkingStateEnum.Unparked;
			HomingState = HomingStateEnum.NotAtHome;
			TelescopeParameters = null;
			TelescopeStatus = null;
			SlavedSlewState = new SlewInProgressMessage( false );

			PollingInterval = POLLING_INTERVAL_NORMAL;
			PollingWake = new ManualResetEvent( false );

			Messenger.Default.Register<TelescopeParametersUpdatedMessage>( this, ( action ) => UpdateTelescopeParameters( action ) );
			Messenger.Default.Register<TelescopeStatusUpdatedMessage>( this, ( action ) => UpdateTelescopeStatus( action ) );
			Messenger.Default.Register<SlewInProgressMessage>( this, ( action ) => InitiateSlavedSlew( action ) );
		}

		#endregion Instance Constructor

		#region Public Properties

		public bool IsConnected { get; private set; }

		public string ConnectError { get; protected set; }
		public Exception ConnectException { get; protected set; }
		public ParkingStateEnum ParkingState { get; private set; }
		public HomingStateEnum HomingState { get; private set; }

		public DomeCapabilities Capabilities { get; private set; }
		public DomeParameters Parameters { get; private set; }
		public DevHubDomeStatus Status { get; private set; }

		public bool IsScopeReadyToSlave
		{
			get
			{
				bool retval = false;

				if ( TelescopeStatus != null && TelescopeStatus.Connected )
				{
					if ( !Double.IsNaN( TelescopeStatus.Altitude ) && !Double.IsNaN( TelescopeStatus.Azimuth ) )
					{
						retval = true;
					}
				}
				return retval;
			}
		}

		public bool IsDomeReadyToSlave
		{
			get
			{
				bool retval = false;

				if ( Status != null && Status.Connected )
				{
					if ( Capabilities != null && ( Capabilities.CanSetAltitude || Capabilities.CanSetAzimuth ) )
					{
						retval = true;
					}
				}

				return retval;
			}
		}

		#endregion Public Properties

		#region Public Methods

		public bool Connect()
		{
			return Connect( DomeID );
		}

		public bool Connect( string domeID )
		{
			ConnectError = "";
			ConnectException = null;

			// Don't re-create the service if it is already created for the same device.

			if ( Service == null || !Service.DeviceCreated || domeID != DomeID )
			{
				try
				{
					InitializeDomeService( domeID );
					SetDomeID( domeID );
				}
				catch ( Exception xcp )
				{
					ConnectError = "Unable to create the dome object.";
					ConnectException = xcp;
					ReleaseDomeService();

					return false;
				}
			}

			// If another client app is active, then the dome may already be connected!!

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
				ConnectError = "Unable to connect to the dome!";
				ConnectException = tempXcp;
				ReleaseDomeService();

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
						ReadInitialDomeDataTask();
						StartDevicePolling();
					} );

					task.Wait();
				}
				catch ( Exception )
				{
					Connected = false;
					IsConnected = false;
					ReleaseDomeService();
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
					Messenger.Default.Send( new DomeSlavedChangedMessage( false ) );
					Messenger.Default.Send( new DeviceDisconnectedMessage( DeviceTypeEnum.Dome ) );
					ReleaseDomeService();
				}
			}
		}

		public void OpenDomeShutter()
		{
			// The name of this method must be different from the ASCOM method name.

			if ( Connected && Capabilities != null && Status != null
				&& Capabilities.CanSetShutter && Status.ShutterStatus != ShutterState.shutterOpen )
			{
				Task.Run( () => OpenShutterTask() );
			}
		}

		public void CloseDomeShutter()
		{
			// The name of this method must be different from the ASCOM method name.

			if ( Connected && Capabilities != null && Status != null
				&& Capabilities.CanSetShutter && Status.ShutterStatus != ShutterState.shutterClosed )
			{
				Task.Run( () => CloseShutterTask() );
			}
		}

		public void ParkTheDome()
		{
			if ( Connected && Capabilities != null && Capabilities.CanPark && !Slewing )
			{
				ParkingState = ParkingStateEnum.ParkInProgress;

				Task.Run( () => ParkDomeTask() );
			}
		}

		public void FindHomePosition()
		{
			if ( Connected && Capabilities != null && Capabilities.CanFindHome && !Slewing )
			{
				Task.Run( () => FindHomeTask() );
			}
		}

		public void SlewDomeShutter( double targetAltitude )
		{
			ShutterState status = ShutterStatus;

			if ( Connected && !Slewing && Capabilities.CanSetAltitude
				&& ( status != ShutterState.shutterClosed || status != ShutterState.shutterError ) )
			{
				Task.Run( () => SlewShutterTask( targetAltitude ) );
			}
		}

		public void SetSlavedState( bool state )
		{
			if ( state != Globals.IsDomeSlaved )
			{
				Messenger.Default.Send( new DomeSlavedChangedMessage( state ) );
			}
		}

		public void SlewDomeToAzimuth( double targetAzimuth )
		{
			if ( Connected && !Slewing && Capabilities.CanSetAzimuth )
			{
				Task.Run( () => SlewDomeTask( targetAzimuth ) );
			}
		}

		public void StopDomeMotion()
		{
			if ( Connected )
			{
				Task.Run( () => AbortSlew() );
			}
		}

		public void SyncDomeToAzimuth( double azimuth )
		{
			if ( Connected && !Slewing && Capabilities.CanSyncAzimuth )
			{
				SyncToAzimuth( azimuth );
			}
		}

		#endregion Public Methods

		#region Helper Methods

		private void StartDevicePolling()
		{
			if ( !DeviceAvailable )
			{
				return;
			}

			if ( !IsPolling )
			{
				PollingTokenSource = new CancellationTokenSource();
				PollingTask = Task.Factory.StartNew( () => PollDomeTask( PollingTokenSource.Token )
												, PollingTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default );
			}
		}

		private void PollDomeTask( CancellationToken token )
		{
			IsPolling = true;

			bool taskCancelled = token.IsCancellationRequested;
			DateTime nextSlaveAdjustmentTime = DateTime.MinValue;
			WaitHandle[] waitHandles = new WaitHandle[] { token.WaitHandle, PollingWake };

			while ( !taskCancelled )
			{
				if ( Service.DeviceAvailable )
				{
					UpdateDomeStatusTask();
					SlewTheSlavedDome( ref nextSlaveAdjustmentTime );
					bool shutterMoving = Status.ShutterStatus == ShutterState.shutterOpening
										|| Status.ShutterStatus == ShutterState.shutterClosing;
					PollingInterval = ( Status.Slewing || shutterMoving ) ? POLLING_INTERVAL_FAST : POLLING_INTERVAL_NORMAL;
				}

				// Wait until the polling interval has expired, we have been cancelled, or we have been
				// awakened early because the polling interval has been changed.

				int index = WaitHandle.WaitAny( waitHandles, PollingInterval );

				if ( index == 0 )
				{
					taskCancelled = true;
				}
				else if ( index == 1 )
				{
					// We have been awakened externally; presumably to change the polling interval.

					PollingWake.Reset();
				}
				else if ( index == WaitHandle.WaitTimeout )
				{
					// The polling interval has expired.
				}
			}

			IsPolling = false;
		}

		private void SlewTheSlavedDome( ref DateTime nextAdjustmentTime )
		{
			DateTime returnTime = nextAdjustmentTime;

			if ( Globals.IsDomeSlaved )
			{
				// Whenever the scope is slewed, we get a message with the target RA, DEC and side-of-pier.
				// We need to slew there immediately. We will get another message when the slew has finished, but
				// until then we need to suspend normal slaved adjustments.

				if ( SlavedSlewState.IsSlewInProgress )
				{
					if ( !Status.Slewing )
					{
						// The telescope is slewing and we are slaved but not slewing so initiate a dome slew
						// to the telescope's target position.

						LogActivityLine( ActivityMessageTypes.Commands
										, "Dome position recalculation due to telescope slew-in-progress." );

						Transform xform = new Transform
						{
							SiteElevation = TelescopeParameters.SiteElevation,
							SiteLatitude = TelescopeParameters.SiteLatitude,
							SiteLongitude = TelescopeParameters.SiteLongitude
						};

						if ( TelescopeParameters.EquatorialSystem == EquatorialCoordinateType.equJ2000 )
						{
							xform.SetJ2000( SlavedSlewState.RightAscension, SlavedSlewState.Declination );
						}
						else
						{
							xform.SetTopocentric( SlavedSlewState.RightAscension, SlavedSlewState.Declination );
						}

						Point scopeTargetPosition = new Point( xform.AzimuthTopocentric, xform.ElevationTopocentric );

						double localHourAngle = TelescopeStatus.CalculateHourAngle( SlavedSlewState.RightAscension );

						SlaveDomePointing( scopeTargetPosition, localHourAngle, SlavedSlewState.SideOfPier );
					}
				}
				else if ( DateTime.Now > nextAdjustmentTime )
				{
					if ( !Status.Slewing )
					{
						// Here is where we re-slew the dome to adjust for non-slew scope movement such as
						// tracking or jogging.

						LogActivityLine( ActivityMessageTypes.Commands
										, "Dome position recalculation due to periodic timer expiration." );

						Point scopePosition = new Point( TelescopeStatus.Azimuth, TelescopeStatus.Altitude );

						SlaveDomePointing( scopePosition, TelescopeStatus.LocalHourAngle, TelescopeStatus.SideOfPier );
					}

					TimeSpan syncSpan = new TimeSpan( 0, 0, Globals.DomeLayout.SlaveInterval );
					returnTime = DateTime.Now + syncSpan;
				}
			}
			else
			{
				// Set the time to min value to force us to sync the dome
				// as soon as the user enables slaving.

				returnTime = DateTime.MinValue;
			}

			nextAdjustmentTime = returnTime;
		}

		private void SlaveDomePointing( Point scopePosition, double localHourAngle, PierSide sideOfPier )
		{
			if ( sideOfPier == PierSide.pierUnknown )
			{
				// We don't have side-of-pier from the telescope, so calculate it.

				sideOfPier = ( localHourAngle < 0.0 ) ? PierSide.pierWest : PierSide.pierEast;

				LogActivityLine( ActivityMessageTypes.Commands
								, "Dome Slaving calculated pier side as {0}", sideOfPier );
			}

			if ( sideOfPier == PierSide.pierUnknown )
			{
				LogActivityLine( ActivityMessageTypes.Commands
								, "Unable to slave the dome; pier side is unknown." );

				return;
			}

			LogActivityLine( ActivityMessageTypes.Commands
							, "Slaving the dome to telescope Az: {0:f2}, Alt: {1:f2}, HA: {2:f5}, SOP: {3}."
							, scopePosition.X, scopePosition.Y, localHourAngle, sideOfPier );

			Point domeAltAz = GetDomeCoord( scopePosition, localHourAngle, sideOfPier );
			double targetAzimuth = domeAltAz.X;

			LogActivityLine( ActivityMessageTypes.Commands, "The calculated dome position is Az: {0:f2}, Alt: {1:f2}."
							, domeAltAz.X, domeAltAz.Y );

			if ( IsInRange( targetAzimuth, Status.Azimuth, Globals.DomeLayout.AzimuthAccuracy ) )
			{
				LogActivityLine( ActivityMessageTypes.Commands
								, "The dome azimuth is close to the target azimuth...no slew is needed." );
			}
			else
			{
				LogActivityLine( ActivityMessageTypes.Commands
								, "Slaving dome to target azimuth of {0:f2} degrees.", targetAzimuth );

				Task task = Task.Run( () =>
				{
					SlewToAzimuth( targetAzimuth );
					Status.Slewing = true;

					if ( PollingInterval != POLLING_INTERVAL_FAST )
					{
						PollingInterval = POLLING_INTERVAL_FAST;
						PollingWake.Set();
					}
				}, CancellationToken.None );
			}

			double targetAltitude = domeAltAz.Y;

			if ( Capabilities.CanSetAltitude && Status.Altitude < targetAltitude )
			{
				LogActivityLine( ActivityMessageTypes.Commands
								, "Synchronizing dome to target altitude of {0:f2} degrees.", targetAltitude );

				Task task = Task.Run( () =>
				{
					SlewToAltitude( targetAltitude );
				}, CancellationToken.None );
			}
		}

		/// <summary>
		/// Tests whether the target azimuth value is within the range of the current
		/// value +/- the accuracy. Also handles wraparound where the target and current values
		/// are on opposite sides of 0 degrees.
		/// </summary>
		/// <param name="targetValue">the target azimuth value</param>
		/// <param name="currentValue">the current azimuth value</param>
		/// <param name="accuracy">the range threshold</param>
		/// <returns> true if the target is in the range of current +/- accuracy</returns>
		private bool IsInRange( double targetValue, double currentValue, double accuracy )
		{
			bool retval = true;

			// First handle wraparound.

			if ( currentValue >= targetValue )
			{
				while ( currentValue - targetValue > 180.0 )
				{
					currentValue -= 360.0;
				}
			}
			else
			{
				while ( targetValue - currentValue > 180.0 )
				{
					currentValue += 360.0;
				}
			}

			retval = ( Math.Abs( targetValue - currentValue ) <= accuracy );

			return retval;
		}

		private void UpdateDomeStatusTask()
		{
			DevHubDomeStatus sts = null;
			Exception except = null;

			try
			{
				if ( HomingState != HomingStateEnum.HomingInProgress || !Slewing )
				{
					HomingState = ( AtHome ) ? HomingStateEnum.IsAtHome : HomingStateEnum.NotAtHome;
				}

				sts = new DevHubDomeStatus( this );
			}
			catch ( Exception xcp )
			{
				string msg = xcp.ToString();
				except = xcp;
			}

			if ( sts == null )
			{
				LogActivityLine( ActivityMessageTypes.Status, "Attempting to update the dome status caught an exception. Details follow:" );
				LogActivityLine( ActivityMessageTypes.Status, except.Message );
			}
			else
			{
				Status = sts;
				Messenger.Default.Send( new DomeStatusUpdatedMessage( sts ) );
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
			}
		}

		private void ReadInitialDomeDataTask()
		{
			// This task talks to the dome from a worker thread, but updates the U/I on the main thread.

			// Wait a second for the dome to settle before reading the data.

			Thread.Sleep( 1000 );

			Capabilities = new DomeCapabilities();
			Capabilities.InitializeFromManager( this );

			Parameters = new DomeParameters();
			Parameters.InitializeFromManager( this );

			DevHubDomeStatus status = new DevHubDomeStatus( this );

			Messenger.Default.Send( new DomeCapabilitiesUpdatedMessage( Capabilities.Clone() ) );
			Messenger.Default.Send( new DomeParametersUpdatedMessage( Parameters.Clone() ) );
			Status = status;
			Messenger.Default.Send( new DomeStatusUpdatedMessage( Status ) );
		}

		private void OpenShutterTask()
		{
			// This method is called as a Task to run on a worker thread. It issues an 
			// asynchronous command to open the shutter and monitors until the driver reports that
			// the shutter is open.

			CancellationTokenSource cts = new CancellationTokenSource( 90000 );

			Task task = Task.Run( () =>
			{
				OpenShutter();
				Status.ShutterStatus = ShutterState.shutterOpening;

				PollingInterval = POLLING_INTERVAL_FAST;
				PollingWake.Set();
			}, cts.Token );

			// Loop here until the ShutterStatus is open, timeout or exception.

			bool more = true;

			while ( more )
			{
				if ( Status.ShutterStatus == ShutterState.shutterOpen || Status.ShutterStatus == ShutterState.shutterError )
				{
					more = false;
				}
				else if ( task.IsCanceled ) // not parked in 90 seconds causes cancellation.
				{
					more = false;
				}
				else if ( task.IsFaulted )
				{
					more = false;
				}

				if ( more )
				{
					Thread.Sleep( 1000 );
				}
			}
		}

		private void CloseShutterTask()
		{
			// This method is called as a Task to run on a worker thread. It issues an 
			// asynchronous command to open the shutter and monitors until the driver reports that
			// the shutter is open.

			CancellationTokenSource cts = new CancellationTokenSource( 90000 );

			Task task = Task.Run( () =>
			{
				CloseShutter();

				Status.ShutterStatus = ShutterState.shutterClosing;

				PollingInterval = POLLING_INTERVAL_FAST;
				PollingWake.Set();
			}, cts.Token );

			// Loop here until the ShutterStatus is closed, timeout, or exception.

			bool more = true;

			while ( more )
			{
				if ( Status.ShutterStatus == ShutterState.shutterClosed || Status.ShutterStatus == ShutterState.shutterError )
				{
					more = false;
				}
				else if ( task.IsCanceled ) // not closed in 90 seconds causes cancellation.
				{
					more = false;
				}
				else if ( task.IsFaulted )
				{
					more = false;
				}

				if ( more )
				{
					Thread.Sleep( 1000 );
				}
			}
		}

		private void SlewShutterTask( double altitude )
		{
			// This method is called as a Task to run on a worker thread. It issues an 
			// asynchronous command to set the shutter altitude and monitors until the driver reports that
			// the shutter movement is complete.

			CancellationTokenSource cts = new CancellationTokenSource( 90000 );

			Task task = Task.Run( () =>
			{
				SlewToAltitude( altitude );

				Status.Slewing = true;
				PollingInterval = POLLING_INTERVAL_FAST;
				PollingWake.Set();

			}, cts.Token );

			MonitorUntilSlewComplete( task );
		}

		private void ParkDomeTask()
		{
			// This method is called as a Task to run on a worker thread.

			// Since we don't know whether Park is synchronous or asynchronous, call it on another thread.
			// which is set up to cancel after 90 seconds.
			
			CancellationTokenSource cts = new CancellationTokenSource( 90000 );

			Task task = Task.Run( () => Park(), cts.Token );

			Thread.Sleep( 250 );

			Status.Slewing = true;
			PollingInterval = POLLING_INTERVAL_FAST;
			PollingWake.Set();

			// Loop here until AtPark is true, timeout or exception.

			bool more = true;

			while ( more )
			{
				if ( Status.AtPark )
				{
					more = false;
					ParkingState = ParkingStateEnum.IsAtPark;
				}
				else if ( task.IsCanceled ) // not parked in 60 seconds causes cancellation.
				{
					more = false;
					ParkingState = ParkingStateEnum.ParkFailed;
				}
				else if ( task.IsFaulted )
				{
					more = false;
					ParkingState = ParkingStateEnum.ParkFailed;
				}

				if ( more )
				{
					Thread.Sleep( 1000 );
				}
			}
		}

		private void FindHomeTask()
		{
			// This method is called as a Task to run on a worker thread. It issues an 
			// asynchronous command to slew the dome to its home azimuth and monitors until the driver reports that
			// the movement is complete.

			CancellationTokenSource cts = new CancellationTokenSource( 90000 );

			Task task = Task.Run( () =>
			{
				FindHome();

				HomingState = HomingStateEnum.HomingInProgress;
				Status.Slewing = true;
				PollingInterval = POLLING_INTERVAL_FAST;
				PollingWake.Set();
			}, cts.Token );

			MonitorUntilSlewComplete( task );

			if ( AtHome )
			{
				HomingState = HomingStateEnum.IsAtHome;
			}
			else
			{
				HomingState = HomingStateEnum.HomeFailed;
			}
		}

		private void SlewDomeTask( double toAzimuth )
		{
			// This method is called as a Task to run on a worker thread. It issues an 
			// asynchronous command to slew the dome to a specified azimuth and monitors until the driver reports that
			// the movement is complete.

			CancellationTokenSource cts = new CancellationTokenSource( 90000 );

			Task task = Task.Run( () =>
			{
				SlewToAzimuth( toAzimuth );

				Status.Slewing = true;
				PollingInterval = POLLING_INTERVAL_FAST;
				PollingWake.Set();

			}, cts.Token );

			MonitorUntilSlewComplete( task );
		}

		private void MonitorUntilSlewComplete( Task task )
		{
			// Loop here until the motion has stopped, timeout or exception.

			bool more = true;

			while ( more )
			{
				if ( !Status.Slewing )
				{
					more = false;
				}
				else if ( task.IsCanceled ) // Still slewing after 90 seconds causes cancellation.
				{
					more = false;
				}
				else if ( task.IsFaulted )
				{
					more = false;
				}

				if ( more )
				{
					Thread.Sleep( 1000 );
				}
			}
		}

		/// <summary>
		/// Calculate the pointing position of the dome given the pointing position of the telescope, 
		/// the hour angle in hours, and the side-of-pier
		/// </summary>
		/// <param name="scopePosition">the azimuth and altitude of the telescope</param>
		/// <param name="hourAngle">the hour angle in decimal hours</param>
		/// <param name="sideOfPier">the side of pier of the telescope</param>
		/// <returns>Point struct containing the azimuth and altitude of the dome</returns>
		private Point GetDomeCoord( Point scopePosition, double hourAngle, PierSide sideOfPier )
		{
			Point domePosition = new Point(0,0);

			if ( Globals.UsePOTHDomeSlaveCalculation )
			{
				DomeControl dc = new DomeControl( Globals.DomeLayout, TelescopeParameters.SiteLatitude );
				domePosition = dc.DomePosition( scopePosition, hourAngle * Globals.HRS_TO_DEG, sideOfPier == PierSide.pierWest );
			}
			else
			{
				DomeSynchronize dsync = new DomeSynchronize( Globals.DomeLayout, TelescopeParameters.SiteLatitude );
				domePosition = dsync.DomePosition( scopePosition, hourAngle * Globals.HRS_TO_DEG, sideOfPier == PierSide.pierWest );
			}

			domePosition.X += Globals.DomeAzimuthAdjustment;
			
			return domePosition;
		}

		private void UpdateTelescopeParameters( TelescopeParametersUpdatedMessage action )
		{
			// Make sure that we update the Parameters on the U/I thread.

			Task.Factory.StartNew( () => TelescopeParameters = action.Parameters, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		// Changed 4/2/2019

		private void UpdateTelescopeStatus( TelescopeStatusUpdatedMessage action )
		{
			TelescopeStatus = action.Status;
		}

		private void InitiateSlavedSlew( SlewInProgressMessage action )
		{
			SlavedSlewState = action;
			PollingWake.Set();
		}

		#endregion Helper Methods
	}
}
