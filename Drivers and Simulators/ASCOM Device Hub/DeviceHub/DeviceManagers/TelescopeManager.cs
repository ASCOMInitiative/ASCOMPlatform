using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ASCOM.Astrometry.AstroUtils;
using ASCOM.Astrometry.Transform;
using ASCOM.DeviceInterface;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public partial class TelescopeManager : DeviceManagerBase, ITelescopeManager, IDisposable
	{
		#region Static Constructor, Properties, Fields, and Methods

		private const int POLLING_PERIOD_NORMAL = 3000;   // once per 3 seconds

		public static string TelescopeID { get; set; }

		private static TelescopeManager _instance = null;

		private static AstroUtils AstroUtils { get; set; }

		public static TelescopeManager Instance
		{
			get
			{
				if ( _instance == null )
				{
					_instance = new TelescopeManager();
				}

				return _instance;
			}

			private set => _instance = value;
		}

		static TelescopeManager()
		{
			TelescopeID = "";

			AstroUtils = new AstroUtils();
		}

		public static void SetTelescopeID( string id )
		{
			TelescopeID = id;
			Messenger.Default.Send( new TelescopeIDChangedMessage( id ) );
		}

		#endregion Static Constructor, Properties, Fields, and Methods

		#region Private Properties

		private bool DeviceCreated => Service.DeviceCreated;
		private bool DeviceAvailable => Service.DeviceAvailable;

		private CancellationTokenSource PollingTokenSource { get; set; }
		private Task PollingTask { get; set; }

		private bool IsPolling { get; set; }
		private bool ForceParameterUpdate { get; set; }
		private CancellationTokenSource PulseGuideCancelTokenSource { get; set; }

		private readonly object _pulseGuideLock = new object();

		private DateTime _pulseGuideEnd;

		private DateTime PulseGuideEnd
		{
			get => _pulseGuideEnd;
			set
			{
				lock ( _pulseGuideLock )
				{
					_pulseGuideEnd = value;
				}
			}
		}

		private SlewInProgressMessage PreviousSlewInProgressMessage { get; set; }

		#endregion Private Properties

		#region Instance Constructor

		private TelescopeManager()
			: base( DeviceTypeEnum.Telescope )
		{
			IsConnected = false;
			Capabilities = null;
			Parameters = null;
			Status = null;
			JogDirections = null;
			PulseGuideEnd = DateTime.MinValue;
			PollingTask = null;

			PollingPeriod = POLLING_PERIOD_NORMAL;
			PollingChange = new ManualResetEvent( false );

			PreviousSlewInProgressMessage = new SlewInProgressMessage( false );
		}

		#endregion Instance Constructor

		#region Public Properties

		public bool IsConnected { get; private set; }
		public bool IsInteractivelyConnected { get; private set; }
		public string ConnectError { get; protected set; }
		public Exception ConnectException { get; protected set; }
		public TelescopeCapabilities Capabilities { get; private set; }
		public TelescopeParameters Parameters { get; private set; }
		public DevHubTelescopeStatus Status { get; private set; }

		private ObservableCollection<JogDirection> _jogDirections;

		public ObservableCollection<JogDirection> JogDirections
		{
			get { return _jogDirections; }
			set
			{
				if ( value != _jogDirections )
				{
					_jogDirections = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion Public Properties

		#region Public Methods

		public bool Connect()
		{
			// This is only called by the telescope driver.

			return Connect( TelescopeID, false );
		}

		public bool Connect( string scopeID, bool interactiveConnect = true )
		{
			ConnectError = "";
			ConnectException = null;

			// Don't re-create the service if it is already created for the same device.

			if ( Service == null || !Service.DeviceCreated || scopeID != TelescopeID )
			{
				try
				{
					InitializeTelescopeService( scopeID );
					SetTelescopeID( scopeID );

					if ( interactiveConnect )
					{
						IsInteractivelyConnected = interactiveConnect;
					}
				}
				catch ( Exception xcp )
				{
					ConnectError = "Unable to create the telescope object.";
					ConnectException = xcp;
					ReleaseTelescopeService();

					return false;
				}
			}

			// If another client app is active, then the telescope may already be connected!!

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
				ConnectError = "Unable to connect to the telescope!";
				ConnectException = tempXcp;
				ReleaseTelescopeService();

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
						ReadInitialTelescopeDataTask();
					} );

					task.Wait(); // This will propagate any unhandled exception

				}
				catch ( AggregateException xcp )
				{
					LogActivityLine( ActivityMessageTypes.Other, "Attempting to initialize telescope operation caught an unhandled exception. Details follow:" );
					LogActivityLine( ActivityMessageTypes.Other, xcp.InnerException.ToString() );

					Connected = false;
					IsConnected = false;
					ReleaseTelescopeService();
				}

				if ( Connected )
				{
					StartDevicePolling();
				}

				retval = Connected;
			}

			return retval;
		}

		public void Disconnect( bool interactiveDisconnect = false )
		{
			if ( !DeviceCreated )
			{
				return;
			}

			if ( IsConnected )
			{
				try
				{
					if ( ( Server.ScopesInUse == 1 && !IsInteractivelyConnected ) ||
						 ( Server.ScopesInUse == 0 && IsInteractivelyConnected && interactiveDisconnect ) )
					{
						// If we are not interactively connected and the last scope is disconnecting then
						//   stop polling and set Connected to false.
						// If there are no connected telescope clients and we are interactively connected and this is an interactive
						//    disconnect then stop polling and set Connected to false

						if ( interactiveDisconnect )
						{
							IsInteractivelyConnected = false;
						}
 
						StopDevicePolling();
						Connected = false;
					}
				}
				catch ( Exception )
				{ }
				finally
				{
					if ( !IsPolling )
					{
						IsConnected = false;
						Messenger.Default.Send( new DeviceDisconnectedMessage( DeviceTypeEnum.Telescope ) );
						ReleaseTelescopeService();
						Globals.LatestRawTelescopeStatus = null;
						_jogDirections = null;
					}
				}
			}
		}

		public void SetFastUpdatePeriod( double period )
		{
			FastPollingPeriod = period;
		}

		public void StartJogScope( int ndx, double rate )
		{
			JogDirection direction = JogDirections[ndx];

			if ( Capabilities.CanMovePrimaryAxis && Capabilities.CanMoveSecondaryAxis )
			{
				TelescopeAxes axis = direction.Axis;
				double trueRate = rate * direction.RateSign;

				StartJogMoveAxis( axis, trueRate );
			}
			else if ( Capabilities.CanPulseGuide )
			{
				// We don't use the rate for jogging with PulseGuide, we keep jogging until
				// The StopJogScope method is called.

				StartJogPulseGuide( direction.GuideDirection );
			}
		}

		public void StopJogScope( int ndx )
		{
			if ( ndx < 0 || ndx >= JogDirections.Count )
			{
				StopJogScope( TelescopeAxes.axisPrimary );
				StopJogScope( TelescopeAxes.axisSecondary );
			}
			else
			{
				StopJogScope( JogDirections[ndx].Axis );
			}
		}

		public void StopJogScope( TelescopeAxes axis )
		{
			if ( Capabilities.CanMovePrimaryAxis && Capabilities.CanMoveSecondaryAxis )
			{
				StopJogMoveAxis( axis );
			}
			else if ( Capabilities.CanPulseGuide )
			{
				StopJogPulseGuide();
			}
		}

		public void StartFixedSlew( int ndx, double distance )
		{
			MoveDirections direction = JogDirections[ndx].MoveDirection;

			if ( IsRaDecSlew( direction ) )
			{
				StartFixedSlewRaDec( direction, distance );
			}
			else
			{
				// At the initial release of Device Hub, fixed slews are always
				// RA/Dec slews, so we should never get here. But perhaps
				// in the future???
			}
		}

		public void AbortDirectSlew()
		{
			AbortSlew();
		}

		public void SetParkingState( ParkingStateEnum desiredState )
		{
			if ( !Connected )
			{
				return;
			}

			if ( desiredState == ParkingStateEnum.IsAtPark )
			{
				// Park the scope.

				SlewToPark();
			}
			else if ( desiredState == ParkingStateEnum.Unparked )
			{
				if ( Service.AtPark )
				{
					// Unpark the scope.

					Unpark();

					if ( !Service.AtPark )
					{
						Status.AtPark = false;
						Status.ParkingState = ParkingStateEnum.Unparked;
					}
				}
			}

			//Debug.WriteLine( $"ParkingState set to {ParkingState}." );
		}

		public void DoSlewToCoordinates( double ra, double dec, bool useSynchronousMethodCall = true )
		{
			if ( !IsConnected || !Capabilities.CanSlew || !IsValidRightAscension( ra ) || !IsValidDeclination( dec ) )
			{
				return;
			}

			bool slewed = false;

			try
			{
				// In case the dome is slaved to us, send it a message to start moving.

				SendSlewMessage( ra, dec );

				if ( useSynchronousMethodCall )
				{
					// Here we are called by the driver and need to be synchronous,
					// because the client app made the synchronous call and does not expect a quick return.

					SlewToCoordinates( ra, dec );
					slewed = true;
				}
				else
				{
					// Here we are called from the U/I and can do the synchronous slew on a worker thread
					// and return right away.

					Task.Run( () => SlewToCoordinates( ra, dec ) );
					slewed = true;

					SetFastPolling();
				}
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
			finally
			{
				if ( !slewed )
				{
					throw new Exception( "Unable to start the direct slew!!!" );
				}
			}
		}

		public void BeginSlewToCoordinatesAsync( double ra, double dec )
		{
			if ( !IsConnected || !Capabilities.CanSlewAsync || !IsValidRightAscension( ra ) || !IsValidDeclination( dec ) )
			{
				return;
			}

			bool slewed = false;

			try
			{
				// In case the dome is slaved to us, send it a message to start moving.

				SendSlewMessage( ra, dec );
				SlewToCoordinatesAsync( ra, dec );
				slewed = true;

				SetFastPolling();
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
			finally
			{
				if ( !slewed )
				{
					throw new Exception( "Unable to start the direct slew!!!" );
				}
			}
		}

		public void DoSlewToTarget( bool useSynchronousMethodCall = true )
		{
			if ( !IsConnected || !Capabilities.CanSlewAsync )
			{
				return;
			}

			bool slewed = false;

			try
			{
				// In case the dome is slaved to us, send it a message to start moving.

				SendSlewMessage( TargetRightAscension, TargetDeclination );

				if ( useSynchronousMethodCall )
				{
					// Here we are called by the driver and need to be synchronous,
					// because the client app made the synchronous call and does not expect a quick return.

					SlewToTarget();
					slewed = true;
				}
				else
				{
					// Here we are called from the U/I and can do the synchronous slew on a worker thread
					// and return right away.

					Task.Run( () => SlewToTarget() );
					slewed = true;

					SetFastPolling();
				}
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
			finally
			{
				if ( !slewed )
				{
					throw new Exception( "Unable to start the slew to target!!!" );
				}
			}
		}

		public void BeginSlewToTargetAsync()
		{
			if ( !IsConnected || !Capabilities.CanSlewAsync )
			{
				return;
			}

			bool slewed = false;

			try
			{
				// In case the dome is slaved to us, send it a message to start moving.

				SendSlewMessage( TargetRightAscension, TargetDeclination );
				SlewToTargetAsync();
				slewed = true;

				SetFastPolling();
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
			finally
			{
				if ( !slewed )
				{
					throw new Exception( "Unable to start the slew to target!!!" );
				}
			}
		}

		public void DoSlewToAltAz( double azimuth, double altitude, bool useSynchronousMethodCall = true )
		{
			if ( !IsConnected || !Capabilities.CanSlewAltAz || !IsValidAzimuth( azimuth ) || !IsValidAltitude( altitude ) )
			{
				return;
			}

			bool slewed = false;

			try
			{
				CalculateRaAndDec( azimuth, altitude, out double ra, out double dec );
				SendSlewMessage( ra, dec );

				if ( useSynchronousMethodCall )
				{
					SlewToAltAz( azimuth, altitude );
					slewed = true;
				}
				else
				{
					// Here we are called from the U/I and can do the synchronous slew on a worker thread
					// and return right away.

					Task.Run( () => SlewToAltAz( azimuth, altitude ) );
					slewed = true;

					SetFastPolling();
				}
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
			finally
			{
				if ( !slewed )
				{
					throw new Exception( "Unable to start the direct slew!!!" );
				}
			}
		}

		public void BeginSlewToAltAzAsync( double azimuth, double altitude )
		{
			if ( !IsConnected || !Capabilities.CanSlewAltAzAsync || !IsValidAzimuth( azimuth ) || !IsValidAltitude( altitude ) )
			{
				return;
			}

			bool slewed = false;

			try
			{
				CalculateRaAndDec( azimuth, altitude, out double ra, out double dec );
				SendSlewMessage( ra, dec );

				SlewToAltAzAsync( azimuth, altitude );
				slewed = true;

				SetFastPolling();
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
			finally
			{
				if ( !slewed )
				{
					throw new Exception( "Unable to start the direct slew!!!" );
				}
			}
		}

		public void SetTracking( bool state )
		{
			if ( state != Tracking )
			{
				Tracking = state;
			}
		}

		public void StartMeridianFlip()
		{
			if ( Parameters.AlignmentMode != AlignmentModes.algGermanPolar )
			{
				throw new InvalidOperationException( "Only German Equatorial telescopes can perform a meridian flip." );
			}

			// Change the Side of the Pier

			double ra = Status.RightAscension;
			double dec = Status.Declination;
			PierSide currentSide = Status.SideOfPier;
			PierSide newSide = PierSide.pierUnknown;

			newSide = ( currentSide == PierSide.pierEast ) ? PierSide.pierWest : newSide;
			newSide = ( currentSide == PierSide.pierWest ) ? PierSide.pierEast : newSide;

			if ( newSide != PierSide.pierUnknown )
			{
				if ( Capabilities.CanSetPierSide )
				{
					LogActivityLine( ActivityMessageTypes.Commands, "Starting Meridian Flip using Set SideOfPier" );

					SideOfPier = newSide;
				}
				else
				{
					LogActivityLine( ActivityMessageTypes.Commands, "Starting Meridian Flip using SlewToCoordinates" );

					DoSlewToCoordinates( ra, dec );
				}

				SetFastPolling();
				SlewInProgressMessage msg = new SlewInProgressMessage( true, ra, dec, newSide );
				Messenger.Default.Send( msg );
				PreviousSlewInProgressMessage = msg;
			}
		}

		public void SetRaOffsetTrackingRate( double rate )
		{
			if ( Capabilities.CanSetRightAscensionRate )
			{
				RightAscensionRate = rate;
			}
		}

		public void SetDecOffsetTrackingRate( double rate )
		{
			if ( Capabilities.CanSetDeclinationRate )
			{
				DeclinationRate = rate;
			}
		}

		public void SetTrackingRate( DriveRates rate )
		{
			if ( Parameters.TrackingRates.Where( r => r.Rate == rate ).FirstOrDefault() == null )
			{
				throw new InvalidValueException( "Requested to set invalid tracking rate." );
			}

			if ( Parameters.TrackingRates.Length > 1 && rate != TrackingRate )
			{
				TrackingRate = rate;
			}
		}

		public void SetParkPosition()
		{
			if ( Capabilities.CanSetPark )
			{
				SetPark();
			}
		}

		public PierSide GetTargetSideOfPier( double rightAscension, double declination )
		{
			PierSide retval = PierSide.pierUnknown;

			if ( Parameters.AlignmentMode == AlignmentModes.algGermanPolar )
			{
				retval = DestinationSideOfPier( rightAscension, declination );
			}

			if ( retval == PierSide.pierUnknown && Parameters.AlignmentMode == AlignmentModes.algGermanPolar )
			{
				// Unable to get side-of-pier for this German Equatorial Mount so we need to simulate it.

				double hourAngle = AstroUtils.ConditionHA( Status.SiderealTime - rightAscension );
				PierSide currentSOP = Status.SideOfPier;
				PierSide destinationSOP = currentSOP; // Favor the current side-of-pier for 0 hour angle;

				destinationSOP = ( hourAngle > 0 ) ? PierSide.pierEast : destinationSOP;
				destinationSOP = ( hourAngle < 0 ) ? PierSide.pierWest : destinationSOP;

				retval = destinationSOP;
				string name = GetPierSideName( retval, true );
				LogActivityLine( ActivityMessageTypes.Other, "Calculated Destination Side-Of-Pier as {0}.", name );
			}

			return retval;
		}

		public void SetTargetDeclination( double targetDec )
		{
			// This is called by the telescope driver. Update the current status before sending the new target dec to the downstream driver.

			Status.TargetDeclination = targetDec;
			TargetDeclination = targetDec;
		}

		public void SetTargetRightAscension( double targetRa )
		{
			// This is called by the telescope driver. Update the current status before sending the new target RA to the downstream driver.

			Status.TargetRightAscension = targetRa;
			TargetRightAscension = targetRa;
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
				PollingTask = Task.Factory.StartNew( () => PollScopeTask( PollingTokenSource.Token ), PollingTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default );
			}
		}

		private void PollScopeTask( CancellationToken token )
		{
			IsPolling = true;
			bool taskCancelled = token.IsCancellationRequested;
			WaitHandle[] waitHandles = new WaitHandle[] { token.WaitHandle, PollingChange };
			Stopwatch watch = new Stopwatch();
			double overhead = 0.0;

			while ( !taskCancelled )
			{
				DateTime wakeupTime = DateTime.Now;
				//Debug.WriteLine( $"Awakened @ {wakeupTime:hh:mm:ss.fff}." );

				if ( Service.DeviceAvailable )
				{
					if ( ForceParameterUpdate )
					{
						ForceParameterUpdate = false;
						UpdateScopeParametersTask();
					}

					UpdateScopeStatusTask();

					if ( IsConnected && JogDirections == null )
					{
						// Make sure that we update the JogDirections on the U/I thread.

						Task.Factory.StartNew( () =>
						{
							JogDirections = InitializeJogDirections();
						}, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
					}

					if ( PreviousSlewInProgressMessage.IsSlewInProgress && !Status.Slewing )
					{
						SlewInProgressMessage msg = new SlewInProgressMessage( false );
						Messenger.Default.Send( msg );
						PreviousSlewInProgressMessage = msg;
					}

					if ( !Status.Slewing && PollingPeriod != POLLING_PERIOD_NORMAL )
					{
						LogActivityLine( ActivityMessageTypes.Commands, $"Returning to normal polling every {POLLING_PERIOD_NORMAL} ms." );
					}

					PollingPeriod = ( Status.Slewing ) ? Convert.ToInt32( FastPollingPeriod * 1000.0 ) : POLLING_PERIOD_NORMAL;
				}

				TimeSpan waitInterval = wakeupTime.AddMilliseconds( (double)PollingPeriod ) - DateTime.Now;
				waitInterval -= TimeSpan.FromMilliseconds( overhead );

				if ( waitInterval.TotalMilliseconds < 0 )
				{
					waitInterval = TimeSpan.FromMilliseconds( 0 );
				}

				// Wait until the wait interval has expired, we have been cancelled, or we have been
				// awakened early because the polling interval has been changed.

				watch.Start();
				int index = WaitHandle.WaitAny( waitHandles, waitInterval );
				watch.Stop();

				// overhead is how much time it took us to get control back, in excess of the waitInterval.

				overhead = Convert.ToDouble( watch.ElapsedMilliseconds ) - waitInterval.TotalMilliseconds;
				watch.Reset();

				if ( index == 0 )
				{
					taskCancelled = true;
					break;
				}
				else if ( index == 1 )
				{
					// We have been awakened externally; presumably to change the polling interval or in response
					// to the scope being slewed.

					PollingChange.Reset();

					// Reset the overhead value since we were awakened early.

					overhead = 0.0;
				}
				else if ( index == WaitHandle.WaitTimeout )
				{
					// The polling interval has expired.
				}
			}

			IsPolling = false;
		}

		private void UpdateScopeParametersTask()
		{
			// For us, Parameters are read-only properties, but they are writeable to the driver
			// and an app that is connected to us could change them. Whenever one of
			// these properties is written to the telescope we force the Parameters to be
			// re-read so that the values that we are displaying are consistent with what
			// the telescope and the client app are working with.

			TelescopeParameters parameters = new TelescopeParameters( this );

			Messenger.Default.Send( new TelescopeParametersUpdatedMessage( parameters.Clone() ) );
		}

		private void UpdateScopeStatusTask()
		{
			DevHubTelescopeStatus sts = null;
			Exception except = null;

			try
			{
				sts = new DevHubTelescopeStatus( this );
			}
			catch ( Exception xcp )
			{
				string msg = xcp.ToString();
				except = xcp;
			}

			if ( sts == null )
			{
				LogActivityLine( ActivityMessageTypes.Status, "Attempting to update the telescope status caught an exception. Details follow:" );
				LogActivityLine( ActivityMessageTypes.Status, except.Message );
			}
			else
			{
				Globals.LatestRawTelescopeStatus = sts.Clone();
				AdjustForCompositeSlewing( sts );
				Status = sts;

				Messenger.Default.Send( new TelescopeStatusUpdatedMessage( sts ) );
			}
		}

		private void AdjustForCompositeSlewing( DevHubTelescopeStatus sts )
		{
			if ( Globals.LatestRawDomeStatus != null && Globals.IsDomeSlaved && Globals.UseCompositeSlewingFlag )
			{
				if ( Globals.LatestRawDomeStatus.Slewing )
				{
					sts.Slewing = true;
				}
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
				PollingTask.Dispose();
				PollingTask = null;
			}
		}

		private void ReadInitialTelescopeDataTask()
		{
			// This task talks to the telescope on a worker thread, but updates the U/I on the main thread.

			// Wait a second for the telescope to settle before reading the data.

			Thread.Sleep( 1000 );

			Capabilities = new TelescopeCapabilities( this );
			Parameters = new TelescopeParameters( this );
			DevHubTelescopeStatus status = new DevHubTelescopeStatus( this );

			Messenger.Default.Send( new TelescopeCapabilitiesUpdatedMessage( Capabilities.Clone() ) );
			Messenger.Default.Send( new TelescopeParametersUpdatedMessage( Parameters.Clone() ) );
			Status = status;
			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( status ) );
		}

		private void SlewScopeToRaDec( double targetRA, double targetDec )
		{
			if ( Capabilities.CanSlew )
			{
				if ( Capabilities.CanSlewAsync )
				{
					// Initiate the async slew.

					SlewToCoordinatesAsync( targetRA, targetDec );
				}
				else
				{
					// Do a synchronous slew on a worker thread so that we can return immediately.

					Task.Run( () => SlewToCoordinates( targetRA, targetDec ) );
				}

				PierSide sideOfPier = GetTargetSideOfPier( targetRA, targetDec );

				SlewInProgressMessage msg = new SlewInProgressMessage( true, targetRA, targetDec, sideOfPier );
				Messenger.Default.Send( msg );
				PreviousSlewInProgressMessage = msg;
			}
		}

		private bool IsRaDecSlew( MoveDirections direction )
		{
			return ( direction == MoveDirections.North || direction == MoveDirections.South
				|| direction == MoveDirections.East || direction == MoveDirections.West );
		}

		private void StartFixedSlewRaDec( MoveDirections direction, double distance )
		{
			if ( IsConnected && Status.IsReadyToSlew )
			{
				distance = Math.Max( distance, 0.0 );

				double startDec = Status.Declination;
				double startRA = Status.RightAscension;

				double targetDec = startDec;
				double targetRA = startRA;

				double z = Math.Cos( startDec * Globals.DegRad ) * 15.0; // Longitude adjustment for a given latitude.
				z = Math.Max( z, 0.001 );

				switch ( direction )
				{
					case MoveDirections.North:
						targetDec = ( Parameters.SiteLatitude < 0.0 ) ? targetDec - distance : targetDec + distance;
						break;

					case MoveDirections.South:
						targetDec = ( Parameters.SiteLatitude < 0.0 ) ? targetDec + distance : targetDec - distance;
						break;

					case MoveDirections.East:
						targetRA += distance / z;
						break;

					case MoveDirections.West:
						targetRA -= distance / z;
						break;
				}

				// Clamp the declination between -90 and 90 degrees and normalize the RA from 0 to 24.

				targetRA = NormalizeRA( targetRA );
				targetDec = Clamp( targetDec, -90.0, 90.0 );

				if ( Capabilities.CanSlew && Status.Tracking )
				{
					SlewScopeToRaDec( targetRA, targetDec );
				}
				else if ( Capabilities.CanSlewAltAz && !Status.Tracking )
				{
					Transform xform = new Transform
					{
						SiteElevation = Parameters.SiteElevation,
						SiteLatitude = Parameters.SiteLatitude,
						SiteLongitude = Parameters.SiteLongitude
					};

					if ( Parameters.EquatorialSystem == EquatorialCoordinateType.equJ2000 )
					{
						xform.SetJ2000( targetRA, targetDec );
					}
					else
					{
						xform.SetTopocentric( targetRA, targetDec );
					}

					double targetAz = xform.AzimuthTopocentric;
					double targetAlt = xform.ElevationTopocentric;

					SlewScopeToAltAz( targetAz, targetAlt );
				}
				else
				{
					// If we get here then the U/I buttons are enabled when they should be disabled!!!

					throw new Exception( "Unable to perform the requested slew." );
				}

				PierSide sideOfPier = GetTargetSideOfPier( targetRA, targetDec );
				SlewInProgressMessage msg = new SlewInProgressMessage( true, targetRA, targetDec, sideOfPier );
				Messenger.Default.Send( msg );
				PreviousSlewInProgressMessage = msg;
			}
		}

		private void SlewScopeToAltAz( double targetAz, double targetAlt )
		{
			if ( Capabilities.CanSlewAltAz )
			{
				// Do the slew asynchronously if we can, otherwise do a
				// synchronous slew on a worker thread.

				if ( Capabilities.CanSlewAltAzAsync )
				{
					SlewToAltAzAsync( targetAz, targetAlt );
				}
				else
				{
					Task.Run( () => SlewToAltAz( targetAz, targetAlt ) );
				}

				CalculateRaAndDec( targetAz, targetAlt, out double targetRa, out double targetDec );
				PierSide sideOfPier = GetTargetSideOfPier( targetRa, targetDec );
				Messenger.Default.Send( new SlewInProgressMessage( true, targetRa, targetDec, sideOfPier ) );
			}
		}

		private void CalculateRaAndDec( double azimuth, double altitude, out double ra, out double dec )
		{
			Transform xform = new Transform
			{
				SiteElevation = Parameters.SiteElevation,
				SiteLatitude = Parameters.SiteLatitude,
				SiteLongitude = Parameters.SiteLongitude
			};

			xform.SetAzimuthElevation( azimuth, altitude );

			if ( Parameters.EquatorialSystem == EquatorialCoordinateType.equJ2000 )
			{
				ra = xform.RAJ2000;
				dec = xform.DecJ2000;
			}
			else
			{
				ra = xform.RATopocentric;
				dec = xform.DECTopocentric;
			}
		}

		private void SlewToPark()
		{
			if ( Connected )
			{
				Task.Run( () => ParkScopeTask() );

				SetFastPolling();
			}
		}

		private string GetNameFromAxis( TelescopeAxes axis )
		{
			string axisName = "Unknown";

			switch ( axis )
			{
				case TelescopeAxes.axisPrimary: axisName = "Primary"; break;
				case TelescopeAxes.axisSecondary: axisName = "Secondary"; break;
				case TelescopeAxes.axisTertiary: axisName = "Tertiary"; break;
			}

			return axisName;
		}

		private string GetPierSideName( PierSide sideOfPier, bool isSimulated = false )
		{
			string name = "";

			switch ( sideOfPier )
			{
				case PierSide.pierUnknown: name = "unknown"; break;
				case PierSide.pierEast: name = "East"; break;
				case PierSide.pierWest: name = "West"; break;
			}

			if ( isSimulated )
			{
				name += " (S)";
			}

			return name;
		}

		private string GetGuideDirectionName( GuideDirections direction )
		{
			string name;

			switch ( direction )
			{
				case GuideDirections.guideEast: name = "East"; break;
				case GuideDirections.guideNorth: name = "North"; break;
				case GuideDirections.guideSouth: name = "South"; break;
				case GuideDirections.guideWest: name = "West"; break;
				default: name = "unknown"; break;
			}

			return name;
		}

		private void ParkScopeTask()
		{
			// This method is called as a Task to run on a worker thread.

			Status.ParkingState = ParkingStateEnum.ParkInProgress;

			// Since we don't know whether Park is synchronous or asynchronous, call it on another thread.
			// which is set it up to cancel after 60 seconds.
			// For the Astro-Physics driver, the Park() call is synchronous.

			CancellationTokenSource cts = new CancellationTokenSource( 60000 );

			Task task = Task.Run( () => Park(), cts.Token );

			// Loop here until AtPark is true, timeout or exception.

			bool more = true;

			while ( more )
			{
				if ( Status.AtPark )
				{
					more = false;
				}
				else if ( task.IsCanceled ) // not parked in 60 seconds causes cancellation.
				{
					more = false;
					Status.ParkingState = ParkingStateEnum.Unparked; // Err on the side of caution.
				}
				else if ( task.IsFaulted )
				{
					more = false;
					Status.ParkingState = ParkingStateEnum.Unparked; // Err on the side of caution.
				}

				if ( more )
				{
					Thread.Sleep( 500 );
				}
			}
		}

		public void StartJogMoveAxis( TelescopeAxes axis, double rate )
		{
			ValidateMoveAxisRate( axis, rate );

			try
			{
				MoveAxis( axis, rate );

				SetFastPolling();
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
		}

		public void StopJogMoveAxis( TelescopeAxes axis )
		{
			try
			{
				MoveAxis( axis, 0.0 );
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
		}

		private void StartJogPulseGuide( GuideDirections? guideDirection )
		{
			GuideDirections? direction = guideDirection;

			// Don't start jogging if we don't have a valid direction.

			if ( !direction.HasValue )
			{
				return;
			}

			try
			{
				PulseGuideCancelTokenSource = new CancellationTokenSource();

				Task.Factory.StartNew( () => DoPulseGuideTask( direction.Value, PulseGuideCancelTokenSource.Token ), PulseGuideCancelTokenSource.Token, TaskCreationOptions.None, TaskScheduler.Default );

				SetFastPolling();
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
		}

		private void StopJogPulseGuide()
		{
			PulseGuideCancelTokenSource.Cancel();
		}

		private const int PulseGuideDuration = 200; // milli-seconds
		private const int PulseGuidePollRate = 220; // milli-seconds

		private void DoPulseGuideTask( GuideDirections direction, CancellationToken cancelToken )
		{
			try
			{
				while ( !cancelToken.IsCancellationRequested )
				{
					PulseGuide( direction, PulseGuideDuration );

					while ( IsPulseGuiding )
					{
						Thread.Sleep( PulseGuidePollRate );
					}
				}
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
		}

		private void ValidateMoveAxisRate( TelescopeAxes axis, double rate )
		{
			bool rateValid = false;

			// Check the requested rate against the valid rates.

			IAxisRates axisRates = AxisRates( axis );
			double tolerance = 0.00001;

			for ( int i = 1; i <= axisRates.Count; ++i )
			{
				IRate axisRate = axisRates[i];

				double minimum = axisRate.Minimum - tolerance;
				double maximum = axisRate.Maximum + tolerance;

				if ( Math.Abs( rate ) >= minimum && Math.Abs( rate ) <= maximum )
				{
					rateValid = true;

					break;
				}
			}

			if ( !rateValid )
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine( "The requested move rate is invalid." );
				sb.AppendLine( "" );
				sb.AppendLine( "A valid rate must be in one of the following ranges:" );

				foreach ( IRate axisRate in axisRates )
				{
					sb.AppendFormat( "{0} - {1}\r\n", axisRate.Minimum, axisRate.Maximum );
				}

				throw new Exception( sb.ToString() );
			}
		}

		private bool IsValidRightAscension( double ra )
		{
			if ( Double.IsNaN( ra ) || ra < 0.0 || ra > 24.0 )
			{
				return false;
			}

			return true;
		}

		private bool IsValidDeclination( double dec )
		{
			if ( Double.IsNaN( dec ) || dec < -90.0 || dec > 90.0 )
			{
				return false;
			}

			return true;
		}

		private bool IsValidAzimuth( double azimuth )
		{
			if ( Double.IsNaN( azimuth ) || azimuth < 0.0 || azimuth >= 360.0 )
			{
				return false;
			}

			return true;
		}

		private bool IsValidAltitude( double altitude )
		{
			if ( Double.IsNaN( altitude ) || altitude < 0.0 || altitude > 90.0 )
			{
				return false;
			}

			return true;
		}

		private double Clamp( double dblValue, double minimum, double maximum )
		{
			double retval = Math.Max( Math.Min( dblValue, maximum ), minimum );

			return retval;
		}

		private double NormalizeRA( double dblValue )
		{
			return NormalizeValue( dblValue, 0.0, 24.0 );
		}

		private void MonitorPulseGuidingTask( DateTime endTime )
		{
			// This task runs on a worker thread to monitor pulse guiding.

			if ( PulseGuideEnd == DateTime.MinValue )
			{
				// Here, set the end time and start monitoring.

				PulseGuideEnd = endTime;

				while ( PulseGuideEnd != DateTime.MinValue )
				{
					if ( DateTime.Now > PulseGuideEnd && !Service.IsPulseGuiding )
					{
						PulseGuideEnd = DateTime.MinValue;
						Status.IsPulseGuiding = false;
						Status.Slewing = Service.Slewing;
					}
					else
					{
						Thread.Sleep( 50 );
					}
				}
			}
			else if ( endTime > PulseGuideEnd )
			{
				// Here we are already monitoring on another thread, so bump the
				// end time and return;

				PulseGuideEnd = endTime;
			}
		}

		private void SendSlewMessage( double ra, double dec )
		{
			PierSide sideOfPier = GetTargetSideOfPier( ra, dec );
			SlewInProgressMessage msg = new SlewInProgressMessage( true, ra, dec, sideOfPier );
			Messenger.Default.Send( msg );
			PreviousSlewInProgressMessage = msg;
		}

		private ObservableCollection<JogDirection> InitializeJogDirections()
		{
			ObservableCollection<JogDirection> directions = new ObservableCollection<JogDirection>()
			{
				new JogDirection { Name = "N", Description = "North", MoveDirection = MoveDirections.North, Axis = TelescopeAxes.axisSecondary, RateSign = 1.0, GuideDirection = GuideDirections.guideNorth },
				new JogDirection { Name = "S", Description = "South", MoveDirection = MoveDirections.South, Axis = TelescopeAxes.axisSecondary, RateSign = -1.0, GuideDirection = GuideDirections.guideSouth },
				new JogDirection { Name = "W", Description = "West", MoveDirection = MoveDirections.West, Axis = TelescopeAxes.axisPrimary, RateSign = 1.0, GuideDirection = GuideDirections.guideWest },
				new JogDirection { Name = "E", Description = "East", MoveDirection = MoveDirections.East, Axis = TelescopeAxes.axisPrimary, RateSign = -1.0, GuideDirection = GuideDirections.guideEast }
			};

			try
			{
				if ( IsConnected )
				{
					if ( Parameters.AlignmentMode == AlignmentModes.algAltAz )
					{
						directions.Clear();

						directions.Add( new JogDirection { Name = "U", Description = "Up", MoveDirection = MoveDirections.Up, Axis = TelescopeAxes.axisSecondary, RateSign = 1.0 } );
						directions.Add( new JogDirection { Name = "D", Description = "Down", MoveDirection = MoveDirections.Down, Axis = TelescopeAxes.axisSecondary, RateSign = -1.0 } );
						directions.Add( new JogDirection { Name = "L", Description = "Left", MoveDirection = MoveDirections.Left, Axis = TelescopeAxes.axisPrimary, RateSign = 1.0 } );
						directions.Add( new JogDirection { Name = "R", Description = "Right", MoveDirection = MoveDirections.Right, Axis = TelescopeAxes.axisPrimary, RateSign = -1.0 } );
					}
					else if ( Parameters.SiteLatitude < 0 )
					{
						directions.Clear();

						directions.Add( new JogDirection { Name = "S", Description = "South", MoveDirection = MoveDirections.South, Axis = TelescopeAxes.axisSecondary, RateSign = -1.0, GuideDirection = GuideDirections.guideSouth } );
						directions.Add( new JogDirection { Name = "N", Description = "North", MoveDirection = MoveDirections.North, Axis = TelescopeAxes.axisSecondary, RateSign = 1.0, GuideDirection = GuideDirections.guideNorth } );
						directions.Add( new JogDirection { Name = "W", Description = "West", MoveDirection = MoveDirections.West, Axis = TelescopeAxes.axisPrimary, RateSign = 1.0, GuideDirection = GuideDirections.guideWest } );
						directions.Add( new JogDirection { Name = "E", Description = "East", MoveDirection = MoveDirections.East, Axis = TelescopeAxes.axisPrimary, RateSign = -1.0, GuideDirection = GuideDirections.guideEast } );
					}
				}
			}
			catch ( Exception )
			{ }

			return directions;
		}

		#endregion Helper Methods
	}
}
