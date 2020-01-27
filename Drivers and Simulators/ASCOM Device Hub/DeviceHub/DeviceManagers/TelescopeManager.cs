using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ASCOM.Astrometry.Transform;
using ASCOM.DeviceInterface;

using ASCOM.DeviceHub.MvvmMessenger;

namespace ASCOM.DeviceHub
{
	public partial class TelescopeManager : DeviceManagerBase, ITelescopeManager, IDisposable
	{
		#region Static Constructor, Properties, Fields, and Methods

		private const int POLLING_INTERVAL_NORMAL = 3000;   // once per second
		private const int POLLING_INTERVAL_FAST = 1500;      // twice per second
		private const int POLLING_INTERVAL_SLOW = 2000;     // once every 2 seconds

		public static string TelescopeID { get; set; }

		private static TelescopeManager _instance = null;

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
		private int PollingInterval { get; set; }
		private bool ForceParameterUpdate { get; set; }
		private CancellationTokenSource PulseGuideCancelTokenSource { get; set; }

		private object _pulseGuideLock = new object();

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

		private bool IsSlaved { get; set; }

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
			IsSlaved = false;
			PulseGuideEnd = DateTime.MinValue;
			PollingTask = null;

			PollingInterval = POLLING_INTERVAL_NORMAL;
			PreviousSlewInProgressMessage = new SlewInProgressMessage( false );
		}

		#endregion Instance Constructor

		#region Public Properties

		public bool IsConnected { get; private set; }

		public string ConnectError { get; protected set; }
		public Exception ConnectException { get; protected set; }
		public ParkingStateEnum ParkingState { get; private set; }

		public TelescopeCapabilities Capabilities { get; private set; }
		public TelescopeParameters Parameters { get; private set; }
		public DevHubTelescopeStatus Status { get; private set; }

		#endregion Public Properties

		#region Public Methods

		public bool Connect()
		{
			return Connect( TelescopeID );
		}

		public bool Connect( string scopeID )
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
						StartDevicePolling();
					} );

					task.Wait();
				}
				catch ( Exception )
				{
					Connected = false;
					IsConnected = false;
					ReleaseTelescopeService();
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
					Messenger.Default.Send( new DeviceDisconnectedMessage( DeviceTypeEnum.Telescope ) );
					ReleaseTelescopeService();
				}
			}
		}

		public void StartJogScope( MoveDirections jogDirection, JogRate jogRate )
		{
			if ( Capabilities.CanMovePrimaryAxis && Capabilities.CanMoveSecondaryAxis )
			{
				StartJogMoveAxis( jogDirection, jogRate );
			}
			else if ( Capabilities.CanPulseGuide )
			{
				StartJogPulseGuide( jogDirection, jogRate );
			}
		}

		public void StopJogScope( MoveDirections jogDirection )
		{
			if ( Capabilities.CanMovePrimaryAxis && Capabilities.CanMoveSecondaryAxis )
			{
				StopJogMoveAxis( jogDirection );
			}
			else if ( Capabilities.CanPulseGuide )
			{
				StopJogPulseGuide();
			}
		}

		public void StartFixedSlew( MoveDirections direction, double distance )
		{
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
				// Unpark the scope.

				Unpark();

				if ( !Status.AtPark )
				{
					ParkingState = ParkingStateEnum.Unparked;
				}
			}
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

				SendSlewMessage( Status.TargetRightAscension, Status.TargetDeclination );

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

				SendSlewMessage( Status.TargetRightAscension, Status.Declination );
				SlewToTargetAsync();
				slewed = true;
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
				double ra, dec;
				CalculateRaAndDec( azimuth, altitude, out ra, out dec );
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
				double ra, dec;
				CalculateRaAndDec( azimuth, altitude, out ra, out dec );
				SendSlewMessage( ra, dec );

				SlewToAltAzAsync( azimuth, altitude );
				slewed = true;
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
			if ( Status != null && Status.Connected )
			{
				if ( state != Tracking )
				{
					Tracking = state;
				}
			}
		}

		public void StartMeridianFlip()
		{
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
					SideOfPier = newSide;
				}
				else
				{
					SlewToCoordinatesAsync( ra, dec );
				}

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

			while ( !taskCancelled )
			{
				if ( Service.DeviceAvailable )
				{
					if ( ForceParameterUpdate )
					{
						ForceParameterUpdate = false;
						UpdateScopeParametersTask();
					}

					UpdateScopeStatusTask();

					if ( PreviousSlewInProgressMessage.IsSlewInProgress && !Status.Slewing )
					{
						SlewInProgressMessage msg = new SlewInProgressMessage( false );
						Messenger.Default.Send( msg );
						PreviousSlewInProgressMessage = msg;
					}

					PollingInterval = ( Status.Slewing ) ? POLLING_INTERVAL_FAST : POLLING_INTERVAL_NORMAL;
				}

				if ( token.WaitHandle.WaitOne( PollingInterval ) )
				{
					taskCancelled = true;
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
				Status = sts;
				Messenger.Default.Send( new TelescopeStatusUpdatedMessage( sts ) );
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

				PierSide sideOfPier = Service.DestinationSideOfPier( targetRA, targetDec );
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

				PierSide sideOfPier = Service.DestinationSideOfPier( targetRA, targetDec );
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

				double targetRa;
				double targetDec;

				CalculateRaAndDec( targetAz, targetAlt, out targetRa, out targetDec );
				PierSide sideOfPier = Service.DestinationSideOfPier( targetRa, targetDec );
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

		private string GetPierSideName( PierSide sideOfPier )
		{
			string name = "";

			switch ( sideOfPier )
			{
				case PierSide.pierUnknown: name = "unknown"; break;
				case PierSide.pierEast: name = "East"; break;
				case PierSide.pierWest: name = "West"; break;
			}

			return name;
		}

		private string GetGuideDirectionName( GuideDirections direction )
		{
			string name = "";

			switch ( direction )
			{
				case GuideDirections.guideEast: name = "East"; break;
				case GuideDirections.guideNorth: name = "North"; break;
				case GuideDirections.guideSouth: name = "South"; break;
				case GuideDirections.guideWest: name = "North"; break;
				default: name = "unknown"; break;
			}

			return name;
		}

		private void ParkScopeTask()
		{
			// This method is called as a Task to run on a worker thread.

			ParkingState = ParkingStateEnum.ParkInProgress;

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
					Thread.Sleep( 500 );
				}
			}
		}

		private void StartJogMoveAxis( MoveDirections jogDirection, JogRate jogRate )
		{
			TelescopeAxes axis = GetJogAxis( jogDirection );

			ValidateMoveAxisRate( axis, jogRate );
			double trueRate = jogRate.Rate * GetJogSign( jogDirection );

			try
			{
				MoveAxis( axis, trueRate );
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
		}

		private void StopJogMoveAxis( MoveDirections jogDirection )
		{
			if ( jogDirection == MoveDirections.None )
			{
				// If we get here we don't know which axis to stop moving, so stop both axes, if supported.

				if ( Capabilities.CanMovePrimaryAxis )
				{
					MoveAxis( TelescopeAxes.axisPrimary, 0.0 );
				}

				if ( Capabilities.CanMoveSecondaryAxis )
				{
					MoveAxis( TelescopeAxes.axisSecondary, 0.0 );
				}
			}
			else
			{
				TelescopeAxes axis = GetJogAxis( jogDirection );

				try
				{
					MoveAxis( axis, 0.0 );
				}
				catch ( Exception xcp )
				{
					throw xcp;
				}
			}
		}

		private void StartJogPulseGuide( MoveDirections jogDirection, JogRate jogRate )
		{
			GuideDirections direction = GetPulseGuideDirection( jogDirection, jogRate );

			// This will throw an exception if the rate is too large!

			ValidatePulseRate( direction, jogRate );

			try
			{
				PulseGuideCancelTokenSource = new CancellationTokenSource();

				Task.Factory.StartNew( () => DoPulseGuideTask( direction, jogRate.Rate, PulseGuideCancelTokenSource.Token ), PulseGuideCancelTokenSource.Token, TaskCreationOptions.None, TaskScheduler.Default );

			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
		}

		private GuideDirections GetPulseGuideDirection( MoveDirections jogDirection, JogRate jogRate )
		{
			TelescopeAxes axis = GetJogAxis( jogDirection );

			double signedRate = jogRate.Rate * GetJogSign( jogDirection );

			GuideDirections direction = GuideDirections.guideWest;

			if ( axis == TelescopeAxes.axisPrimary && signedRate < 0.0 )
			{
				direction = GuideDirections.guideEast;
			}
			else if ( axis == TelescopeAxes.axisSecondary && signedRate >= 0.0 )
			{
				direction = GuideDirections.guideNorth;
			}
			else if ( axis == TelescopeAxes.axisSecondary && signedRate < 0.0 )
			{
				direction = GuideDirections.guideSouth;
			}

			return direction;
		}

		private void StopJogPulseGuide()
		{
			PulseGuideCancelTokenSource.Cancel();
		}

		private const int PulseGuideDuration = 200; // milli-seconds
		private const int PulseGuidePollRate = 220; // milli-seconds

		private void DoPulseGuideTask( GuideDirections direction, double jogRate, CancellationToken cancelToken )
		{
			double originalGuideRate = Double.NaN;

			try
			{
				if ( direction == GuideDirections.guideEast || direction == GuideDirections.guideWest )
				{
					originalGuideRate = GuideRateRightAscension;
					GuideRateRightAscension = jogRate;
				}
				else if ( direction == GuideDirections.guideNorth || direction == GuideDirections.guideSouth )
				{
					originalGuideRate = GuideRateDeclination;
					GuideRateDeclination = jogRate;
				}
				else
				{
					return;
				}

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
			finally
			{
				if ( !Double.IsNaN( originalGuideRate ) )
				{
					if ( direction == GuideDirections.guideEast || direction == GuideDirections.guideWest )
					{
						GuideRateRightAscension = originalGuideRate;
					}
					else
					{
						GuideRateDeclination = originalGuideRate;
					}
				}
			}
		}

		private TelescopeAxes GetJogAxis( MoveDirections direction )
		{
			return ( direction == MoveDirections.East
					|| direction == MoveDirections.West
					|| direction == MoveDirections.Left
					|| direction == MoveDirections.Right ) ? TelescopeAxes.axisPrimary : TelescopeAxes.axisSecondary;
		}

		private double GetJogSign( MoveDirections direction )
		{
			return ( direction == MoveDirections.South
					|| direction == MoveDirections.East
					|| direction == MoveDirections.Down
					|| direction == MoveDirections.Left ) ? -1.0 : 1.0;
		}

		private void ValidateMoveAxisRate( TelescopeAxes axis, JogRate rate )
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

				if ( rate.Rate >= minimum && rate.Rate <= maximum )
				{
					rateValid = true;
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

		private void ValidatePulseRate( GuideDirections guideDirection, JogRate rate )
		{
			// It is assumed that CanPulseGuide is true when this method is called!!!

			bool rateValid = false;
			Exception rateException = null;

			// Check the requested rate against the valid rates.

			double currentGuideRate = 0.00417807; // 1X sidereal

			if ( guideDirection == GuideDirections.guideEast || guideDirection == GuideDirections.guideWest )
			{
				currentGuideRate = GuideRateRightAscension;

				try
				{
					GuideRateRightAscension = rate.Rate;
					rateValid = true;
				}
				catch ( Exception xcp )
				{
					rateException = xcp;
				}

				GuideRateRightAscension = currentGuideRate;
			}
			else if ( guideDirection == GuideDirections.guideNorth || guideDirection == GuideDirections.guideSouth )
			{
				currentGuideRate = GuideRateDeclination;

				try
				{
					GuideRateDeclination = rate.Rate;
					rateValid = true;
				}
				catch ( Exception xcp )
				{
					rateException = xcp;
				}

				GuideRateDeclination = currentGuideRate;
			}

			if ( !rateValid )
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine( "The requested move rate is invalid. Please choose a slower rate!" );
				sb.AppendLine( "Error message follows:" );
				sb.AppendLine( rateException.Message );

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

		private double NormalizeAzimuth( double dblValue )
		{
			return NormalizeValue( dblValue, 0.0, 360.0 );
		}

		private void MonitorPulseGuidingTask( DateTime endTime )
		{
			// This task runs on a worker thread to monitor pulse guiding.

			if (PulseGuideEnd == DateTime.MinValue )
			{
				// Here, set the end time and start monitoring.

				PulseGuideEnd = endTime;

				while ( PulseGuideEnd != DateTime.MinValue )
				{
					if ( DateTime.Now > PulseGuideEnd && !Service.IsPulseGuiding )
					{
						PulseGuideEnd = DateTime.MinValue;
						Status.IsPulseGuiding = false;
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
			PierSide sop = DestinationSideOfPier( ra, dec );
			SlewInProgressMessage msg = new SlewInProgressMessage( true, ra, dec, sop );
			Messenger.Default.Send( msg );
			PreviousSlewInProgressMessage = msg;
		}

		#endregion Helper Methods
	}
}
