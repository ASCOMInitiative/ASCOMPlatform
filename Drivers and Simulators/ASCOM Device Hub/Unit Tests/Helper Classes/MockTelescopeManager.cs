using System;
using System.Threading;
using System.Threading.Tasks;
using ASCOM.Astrometry.Transform;
using ASCOM.DeviceHub;
using ASCOM.DeviceInterface;
using ASCOM.DeviceHub.MvvmMessenger;
using System.Collections.ObjectModel;
using System.Linq;

namespace Unit_Tests
{
	public class MockTelescopeManager : ITelescopeManager
	{
		private const int PulseGuidePollRate = 220; // milli-seconds

		private CancellationTokenSource PulseGuideCancelTokenSource { get; set; }

		public MockTelescopeManager()
		{
			IsConnected = false;
			MockJogWithPulseGuide = false;
			MockTrackingRate = DriveRates.driveSidereal;
			MockRightAscensionOffsetRate = 0.0;
			MockDeclinationOffsetRate = 0.0;
			_jogDirections = null;
			PulseGuideCancelTokenSource = null;
			MockIsPulseGuiding = false;
		}

		public string MockTelescopeID { get; set; }
		public string MockTelescopeName { get; set; }

		public bool MockIsConnected { get; set; }
		public bool MockIsPulseGuiding { get; set; }
		public double FastPollingPeriod { get; set; }

		public TelescopeCapabilities Capabilities { get; set; }
		public TelescopeParameters Parameters { get; set; }
		public DevHubTelescopeStatus Status { get; set; }

		ObservableCollection<JogDirection> _jogDirections;
		public ObservableCollection<JogDirection> JogDirections
		{
			get
			{
				if ( _jogDirections == null )
				{
					SetJogDirections();
				}

				return _jogDirections;
			}
		}

		public ParkingStateEnum MockParkingState
		{
			get => Status.ParkingState;
			set => Status.ParkingState = value;
		}

		public PierSide MockSideOfPier { get; set; }
		public bool MockJogWithPulseGuide { get; set; }
		public double MockRightAscensionAxisRate { get; set; }
		public double MockDeclinationAxisRate { get; set; }
		public double MockRightAscensionOffsetRate { get; set; }
		public double MockDeclinationOffsetRate { get; set; }
		public DriveRates MockTrackingRate { get; set; }

		public string TelescopeID => MockTelescopeID;
		public string TelescopeName => MockTelescopeName;

		public string ConnectError { get; private set; }
		public Exception ConnectException { get; private set; }

		public bool IsConnected
		{
			get => MockIsConnected;
			private set => MockIsConnected = value;
		}

		public void AbortDirectSlew()
		{
			Status.Slewing = false;

			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );
		}

		public void DoSlewToCoordinates( double ra, double dec, bool useSynchronousMethodCall = false )
		{
			Status = DevHubTelescopeStatus.GetEmptyStatus();
			Status.TargetRightAscension = ra;
			Status.TargetDeclination = dec;
			Status.Slewing = true;

			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );

			Thread.Sleep( 1500 );

			Status.RightAscension = ra;
			Status.Declination = dec;
			Status.Slewing = false;

			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );
		}

		public void BeginSlewToCoordinatesAsync( double ra, double dec )
		{
			Status = DevHubTelescopeStatus.GetEmptyStatus();
			Status.TargetRightAscension = ra;
			Status.TargetDeclination = dec;
			Status.Slewing = true;

			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );

			Thread.Sleep( 1500 );

			Status.RightAscension = ra;
			Status.Declination = dec;
			Status.Slewing = false;

			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );
		}

		public void DoSlewToAltAz( double az, double alt, bool useSynchronousMethodCall = false )
		{
			throw new NotImplementedException();
		}

		public void BeginSlewToAltAzAsync( double az, double alt )
		{
			throw new NotImplementedException();
		}

		public void DoSlewToTarget( bool useSunchronousMethodCall = false )
		{
			throw new NotImplementedException();
		}

		public void BeginSlewToTargetAsync()
		{
			throw new NotImplementedException();
		}
		public bool Connect( string scopeID )
		{
			MockTelescopeID = scopeID;
			IsConnected = true;

			return IsConnected;
		}

		public void Disconnect()
		{
			IsConnected = false;
		}

		public void SetDecOffsetTrackingRate( double rate )
		{
			MockDeclinationOffsetRate = rate;
		}

		public void SetParkingState( ParkingStateEnum desiredState )
		{
			if ( desiredState == ParkingStateEnum.IsAtPark )
			{
				Task.Run( () => ParkScopeTask() ).Wait();
			}
			else
			{
				MockParkingState = ParkingStateEnum.Unparked;

				Status = DevHubTelescopeStatus.GetEmptyStatus();
				Status.AtPark = ( desiredState == ParkingStateEnum.IsAtPark );
				Status.ParkingState = desiredState;

				Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );
			}
		}

		public void SetRaOffsetTrackingRate( double rate )
		{
			MockRightAscensionOffsetRate = rate;
		}

		public void SetTracking( bool state )
		{
			Status = DevHubTelescopeStatus.GetEmptyStatus();
			Status.Connected = true;
			Status.Tracking = state;

			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );
		}

		public void SetTrackingRate( DriveRates rate )
		{
			MockTrackingRate = rate;
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
				StartFixedSlewAltAz( direction, distance );
			}
		}

		public void StartJogScope( int ndx, double rate )
		{
			MoveDirections jogDirection = JogDirections[ndx].MoveDirection;

			StartJogScope( jogDirection, rate );
		}

		public void StartJogScope( MoveDirections jogDirection, double rate )
		{
			JogRate jogRate = new JogRate { Name="", Rate = rate };

			StartJogScope( jogDirection, jogRate );
		}

		public void StartJogScope( MoveDirections jogDirection, JogRate jogRate )
		{
			if ( MockJogWithPulseGuide )
			{
				JogDirection direction = JogDirections.Where( j => j.MoveDirection == jogDirection).First();

				StartJogPulseGuide( direction.GuideDirection );
			}
			else
			{
				StartJogMoveAxis( jogDirection, jogRate );
			}
		}

		public void StartMeridianFlip()
		{
			// Change the Side of the Pier

			PierSide currentSide = Status.SideOfPier;
			PierSide newSide = PierSide.pierUnknown;

			newSide = ( currentSide == PierSide.pierEast ) ? PierSide.pierWest : newSide;
			newSide = ( currentSide == PierSide.pierWest ) ? PierSide.pierEast : newSide;

			if ( newSide != PierSide.pierUnknown )
			{
				MockSideOfPier = newSide;
				Status.Slewing = true;
				Status.SideOfPier = newSide;
				Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );

				Thread.Sleep( 1500 );

				Status.Slewing = false;
				Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );
			}
		}

		public void StopJogScope( int ndx )
		{
			StopJogScope( JogDirections[ndx].Axis );
		}

		public void StopJogScope( TelescopeAxes axis )
		{
			if ( MockJogWithPulseGuide )
			{
				StopJogPulseGuide();
			}
			else
			{
				StopJogMoveAxis( axis );
			}
		}

		public void SetParkPosition()
		{
			// Not used, but needed to satisfy the interface.
		}

		#region Helper Methods

		private void ParkScopeTask()
		{
			MockParkingState = ParkingStateEnum.ParkInProgress;

			Status = DevHubTelescopeStatus.GetEmptyStatus();
			Status.ParkingState = MockParkingState;

			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );

			Thread.Sleep( 1000 );

			MockParkingState = ParkingStateEnum.IsAtPark;

			Status.AtPark = true;
			Status.ParkingState = MockParkingState;

			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );
		}

		private void StartJogPulseGuide( GuideDirections? direction )
		{
			// Don't start jogging if we don't have a valid direction.

			if ( !direction.HasValue )
			{
				return;
			}

			try
			{
				MockIsPulseGuiding = true;

				PulseGuideCancelTokenSource = new CancellationTokenSource();

				Task.Factory.StartNew( () => DoPulseGuideTask( direction.Value, PulseGuideCancelTokenSource.Token ), PulseGuideCancelTokenSource.Token, TaskCreationOptions.None, TaskScheduler.Default );
			}
			catch ( Exception xcp )
			{
				throw xcp;
			}
		}

		private void DoPulseGuideTask( GuideDirections direction, CancellationToken cancelToken )
		{
			try
			{
				while ( !cancelToken.IsCancellationRequested )
				{
					while ( MockIsPulseGuiding )
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


		private void StartJogMoveAxis( MoveDirections jogDirection, JogRate jogRate )
		{
			TelescopeAxes axis = GetJogAxis( jogDirection );

			double trueRate = jogRate.Rate * GetJogSign( jogDirection );

			if ( axis == TelescopeAxes.axisPrimary )
			{
				MockRightAscensionAxisRate = trueRate;
			}
			else if ( axis == TelescopeAxes.axisSecondary )
			{
				MockDeclinationAxisRate = trueRate;
			}
		}

		private void StopJogMoveAxis( TelescopeAxes axis )
		{
			if ( axis == TelescopeAxes.axisPrimary )
			{
				MockRightAscensionAxisRate = 0.0;
			}
			else if ( axis == TelescopeAxes.axisSecondary )
			{
				MockDeclinationAxisRate = 0.0;
			}
		}

		private void StopJogPulseGuide()
		{
			MockIsPulseGuiding = false;

			PulseGuideCancelTokenSource.Cancel();
		}

		private TelescopeAxes GetJogAxis( MoveDirections direction )
		{
			return ( direction == MoveDirections.East || direction == MoveDirections.West ) ? TelescopeAxes.axisPrimary : TelescopeAxes.axisSecondary;
		}

		private double GetJogSign( MoveDirections direction )
		{
			return ( direction == MoveDirections.South || direction == MoveDirections.East ) ? -1.0 : 1.0;
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

				if ( Capabilities.CanSlew || Capabilities.CanSlewAsync )
				{
					Task.Run( () => SlewScopeToRaDec( targetRA, targetDec ) );
				}
				else if ( Capabilities.CanSlewAltAz || Capabilities.CanSlewAltAzAsync )
				{
					throw new NotImplementedException(
						"Unsupported telescope encountered...German equatorial that cannot be slewed to an RA and Dec position!!!" );
				}
			}
		}

		private void SlewScopeToRaDec( double targetRA, double targetDec )
		{
			Status = DevHubTelescopeStatus.GetEmptyStatus();
			Status.Slewing = true;
			Status.TargetRightAscension = targetRA;
			Status.TargetDeclination = targetDec;

			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );

			Thread.Sleep( 1500 );

			Status.Slewing = false;
			Status.RightAscension = targetRA;
			Status.Declination = targetDec;

			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );
		}

		private void StartFixedSlewAltAz( MoveDirections direction, double distance )
		{
			if ( IsConnected && Status.IsReadyToSlew )
			{
				distance = Math.Max( distance, 0.0 );

				double startAz = Status.Azimuth;
				double startAlt = Status.Altitude;

				double targetAz = startAz;
				double targetAlt = startAlt;

				switch ( direction )
				{
					case MoveDirections.Up:
						targetAlt = ( Parameters.SiteLatitude < 0.0 ) ? targetAlt - distance : targetAlt + distance;
						break;

					case MoveDirections.Down:
						targetAlt = ( Parameters.SiteLatitude < 0.0 ) ? targetAlt + distance : targetAlt - distance;
						break;

					case MoveDirections.Right:
						targetAz += distance;
						break;

					case MoveDirections.Left:
						targetAz -= distance;
						break;
				}

				// Clamp the declination between -90 and 90 degrees and normalize the RA from 0 to 24.

				targetAz = NormalizeAzimuth( targetAz );
				targetAlt = Clamp( targetAlt, 0.0, 90.0 );

				if ( Capabilities.CanSlewAltAz || Capabilities.CanSlewAltAzAsync )
				{
					Task.Run( () => SlewScopeToAltAz( targetAz, targetAlt ) );
				}
				else if ( Capabilities.CanSlew || Capabilities.CanSlewAsync )
				{
					throw new NotImplementedException(
						"Unsupported telescope encountered...An Alt-AZ scope that cannot be slewed to an altitude and azimuth position!!!" );
				}
			}
		}

		private void SlewScopeToAltAz( double targetAz, double targetAlt )
		{
			Transform xform = new Transform();

			xform.SiteElevation = Parameters.SiteElevation;
			xform.SiteLatitude = Parameters.SiteLatitude;
			xform.SiteLongitude = Parameters.SiteLongitude;

			xform.SetAzimuthElevation( targetAz, targetAlt );

			double targetRA = xform.RATopocentric;
			double targetDec = xform.DECTopocentric;

			Status = DevHubTelescopeStatus.GetEmptyStatus();
			Status.Slewing = true;
			Status.TargetRightAscension = targetRA;
			Status.TargetDeclination = targetDec;

			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );

			Thread.Sleep( 1500 );

			Status.Slewing = false;
			Status.RightAscension = targetRA;
			Status.Declination = targetDec;
			Status.Azimuth = targetAz;
			Status.Altitude = targetAlt;

			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );
		}

		private double Clamp( double dblValue, double minimum, double maximum )
		{
			double retval = Math.Max( Math.Min( dblValue, maximum ), minimum );

			return retval;
		}

		private double NormalizeRA( double dblValue )
		{
			return NormalizeValue( dblValue, 24.0 );
		}

		private double NormalizeAzimuth( double dblValue )
		{
			return NormalizeValue( dblValue, 360.0 );
		}

		private double NormalizeValue( double dblValue, double maxValue )
		{
			// Zero is the implied min value.

			double retval = dblValue;

			while ( retval < 0.0 )
			{
				retval += maxValue;
			}

			while ( retval >= maxValue )
			{
				retval -= maxValue;
			}

			return retval;
		}

		private void SetJogDirections()
		{
			_jogDirections = new ObservableCollection<JogDirection>()
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
						_jogDirections.Clear();

						_jogDirections.Add( new JogDirection { Name = "U", Description = "Up", MoveDirection = MoveDirections.Up, Axis = TelescopeAxes.axisSecondary, RateSign = 1.0 } );
						_jogDirections.Add( new JogDirection { Name = "D", Description = "Down", MoveDirection = MoveDirections.Down, Axis = TelescopeAxes.axisSecondary, RateSign = -1.0 } );
						_jogDirections.Add( new JogDirection { Name = "L", Description = "Left", MoveDirection = MoveDirections.Left, Axis = TelescopeAxes.axisPrimary, RateSign = -1.0 } );
						_jogDirections.Add( new JogDirection { Name = "R", Description = "Right", MoveDirection = MoveDirections.Right, Axis = TelescopeAxes.axisPrimary, RateSign = 1.0 } );
					}
					else if ( Parameters.SiteLatitude < 0 )
					{
						_jogDirections.Clear();

						_jogDirections.Add( new JogDirection { Name = "S", Description = "South", MoveDirection = MoveDirections.South, Axis = TelescopeAxes.axisSecondary, RateSign = 1.0, GuideDirection = GuideDirections.guideSouth } );
						_jogDirections.Add( new JogDirection { Name = "N", Description = "North", MoveDirection = MoveDirections.North, Axis = TelescopeAxes.axisSecondary, RateSign = -1.0, GuideDirection = GuideDirections.guideNorth } );
						_jogDirections.Add( new JogDirection { Name = "W", Description = "West", MoveDirection = MoveDirections.West, Axis = TelescopeAxes.axisPrimary, RateSign = -1.0, GuideDirection = GuideDirections.guideWest } );
						_jogDirections.Add( new JogDirection { Name = "E", Description = "East", MoveDirection = MoveDirections.East, Axis = TelescopeAxes.axisPrimary, RateSign = 1.0, GuideDirection = GuideDirections.guideEast } );
					}
				}
			}
			catch ( Exception )
			{ }
		}

		#endregion
	}
}