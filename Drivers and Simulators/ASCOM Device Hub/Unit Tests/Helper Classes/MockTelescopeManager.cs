using System;
using System.Threading;
using System.Threading.Tasks;
using ASCOM.Astrometry.Transform;
using ASCOM.DeviceHub;
using ASCOM.DeviceInterface;
using ASCOM.DeviceHub.MvvmMessenger;

namespace Unit_Tests
{
	public class MockTelescopeManager : ITelescopeManager
	{
		public MockTelescopeManager()
		{
			IsConnected = false;
			MockJogWithPulseGuide = false;
			MockTrackingRate = DriveRates.driveSidereal;
			MockRightAscensionOffsetRate = 0.0;
			MockDeclinationOffsetRate = 0.0;
		}

		public string MockTelescopeID { get; set; }
		public string MockTelescopeName { get; set; }

		public bool MockIsConnected { get; set; }

		public TelescopeCapabilities Capabilities { get; set; }
		public TelescopeParameters Parameters { get; set; }
		public DevHubTelescopeStatus Status { get; set; }

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
				Task.Run( () => ParkScopeTask() );
			}
			else
			{
				MockParkingState = ParkingStateEnum.Unparked;

				Status = DevHubTelescopeStatus.GetEmptyStatus();
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

		public void StartFixedSlew( MoveDirections direction, double distance )
		{
			if ( IsRaDecSlew( direction ) )
			{
				StartFixedSlewRaDec( direction, distance );
			}
			else
			{
				StartFixedSlewAltAz( direction, distance );
			}
		}

		public void StartJogScope( MoveDirections jogDirection, JogRate jogRate )
		{
			if ( MockJogWithPulseGuide )
			{
				StartJogPulseGuide( jogDirection, jogRate );
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

		public void StopJogScope( MoveDirections jogDirection )
		{
			if ( MockJogWithPulseGuide )
			{
				throw new NotImplementedException();
			}
			else
			{
				StopJogMoveAxis( jogDirection );
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

			Status.ParkingState = MockParkingState;

			Messenger.Default.Send( new TelescopeStatusUpdatedMessage( Status ) );
		}

		private void StartJogPulseGuide( MoveDirections jogDirection, JogRate jogRate )
		{
			throw new NotImplementedException();
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

		private void StopJogMoveAxis( MoveDirections jogDirection )
		{
			TelescopeAxes axis = GetJogAxis( jogDirection );

			if ( axis == TelescopeAxes.axisPrimary )
			{
				MockRightAscensionAxisRate = 0.0;
			}
			else if ( axis == TelescopeAxes.axisSecondary )
			{
				MockDeclinationAxisRate = 0.0;
			}
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
					Task.Run ( () => SlewScopeToAltAz( targetAz, targetAlt ) );
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

		#endregion
	}
}
