using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ASCOM;
using ASCOM.Astrometry.Transform;
using ASCOM.DeviceHub;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace Unit_Tests
{
	public enum SlewType
	{
		SlewNone,
		SlewSettle,
		SlewMoveAxis,
		SlewRaDec,
		SlewAltAz,
		SlewPark,
		SlewHome,
		SlewHandpad
	}
	public enum SlewSpeed
	{
		SlewSlow,
		SlewMedium,
		SlewFast
	}
	public enum SlewDirection
	{
		SlewNorth,
		SlewSouth,
		SlewEast,
		SlewWest,
		SlewUp,
		SlewDown,
		SlewLeft,
		SlewRight,
		SlewNone
	}

	public class MockTelescopeService : ITelescopeService
	{
		private const double SIDEREAL_RATE_DEG_SEC = 15.041 / 3600;
		private const double SOLAR_RATE_DEG_SEC = 15.0 / 3600;
		private const double LUNAR_RATE_DEG_SEC = 14.515 / 3600;
		private const double KING_RATE_DEG_SEC = 15.037 / 3600;
		private const double SIDEREAL_RATE_SEC_SEC = 15.041 / 15.0;
		private const int TIMER_INTERVAL = 250; // 1/4 second
		private const double DEG_RAD = Math.PI / 180.0;

		private const double slewSpeedFast = 1.25;
		private const double slewSpeedMedium = 0.1;
		private const double slewSpeedSlow = 0.02;
		private const double hourAngleLimit = 20;     // the number of degrees a GEM can go past the meridian

		private CancellationTokenSource _cancelMoveTokenSource;

		private string DeviceID { get; set; }

		public AlignmentModes MockAlignmentMode { get; set; }
		public Vector MockAltAz { get; set; }
		public bool MockAtHome { get; set; }
		public bool MockAtPark { get; set; }
		public bool MockIsPulseGuiding { get; set; }
		public Vector MockRaDec { get; set; }
		public bool MockTracking { get; set; }
		public DriveRates MockTrackingRate { get; set; }
		public DateTime MockUTCDate { get; set; }
		public SlewType MockSlewState { get; set; }
		public PierSide MockSideOfPier { get; set; }
		public bool MockSlewing { get; set; }
		public Vector MockMoveAxisRates { get; set; }
		public Vector MockOffsetTrackingRates { get; set; }
		public bool[] MockCanMoveAxes { get; set; }
		public IAxisRates MockPrimaryAxisRates { get; set; }
		public IAxisRates MockSecondaryAxisRates { get; set; }
		public IAxisRates MockTertiaryAxisRates { get; set; }

		private Vector _targetAxes;
		private Vector _mountAxes;
		private DateTime _settleTime;
		public static bool isPulseGuidingRa;
		public static bool isPulseGuidingDec;
		public static Vector guideDuration = new Vector(); // In seconds

		// Guide rates, in deg/sec. X is RA or azimuth, Y is declination or altitude
		public static Vector guideRate;

		public MockTelescopeService()
		{
			Initialized = false;
			Connected = false;
			MockAlignmentMode = AlignmentModes.algGermanPolar;
			MockAltAz = new Vector { X = Double.NaN, Y = Double.NaN };
			MockAtHome = false;
			MockAtPark = true;
			MockIsPulseGuiding = false;
			MockRaDec = new Vector { X = Double.NaN, Y = Double.NaN };
			MockTracking = false;
			MockTrackingRate = DriveRates.driveSidereal;
			MockUTCDate = DateTime.Now;
			MockMoveAxisRates = new Vector { X = 0.0, Y = 0.0 };
			MockCanMoveAxes = new bool[] { true, true, false };
			DoesRefraction = false;
			GuideRateDeclination = 0.004178;
			GuideRateRightAscension = 0.004178;
			MockSideOfPier = PierSide.pierUnknown;
			SiteElevation = 1370;
			SiteLatitude = 31.48417;
			SiteLongitude = -110.19783;
			MockSlewing = false;
			SlewSettleTime = 0;
			_targetDeclination = Double.NaN;
			_targetRightAscension = Double.NaN;
			TrackingRates = new TrackingRates();
			guideRate = new Vector();

			Unit_Tests.AxisRates.ResetRates();
			MockPrimaryAxisRates = new AxisRates( TelescopeAxes.axisPrimary );
			MockSecondaryAxisRates = new AxisRates( TelescopeAxes.axisSecondary );
			MockTertiaryAxisRates = new AxisRates( TelescopeAxes.axisTertiary );
		}

		public bool Initialized { get; private set; }
		public bool DeviceCreated => Initialized;
		public bool DeviceAvailable => DeviceCreated && Connected;

		public bool Connected { get; set; }
		public string Description => "This is a mock ASCOM telescope";
		public string DriverInfo => "Mock Telescope Driver Version 1.0";
		public string DriverVersion => "1.0";
		public short InterfaceVersion => 3;
		public string Name => "ASCOM.Mock.Telescope";
		public ArrayList SupportedActions => new ArrayList();

		public AlignmentModes AlignmentMode
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockAlignmentMode;
			}
		}

		public double Altitude
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockAltAz.Y;
			}
		}

		public double ApertureArea => 0.01327;
		public double ApertureDiameter => 0.130;

		public bool AtHome
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockAtHome;
			}
		}

		public bool AtPark
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockAtPark;
			}
		}

		public double Azimuth
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockAltAz.X;
			}
		}

		public bool CanFindHome => true;
		public bool CanPark => true;
		public bool CanPulseGuide => true;
		public bool CanSetDeclinationRate => true;
		public bool CanSetGuideRates => true;
		public bool CanSetPark => true;
		public bool CanSetPierSide => true;
		public bool CanSetRightAscensionRate => true;
		public bool CanSetTracking => true;
		public bool CanSlew => true;
		public bool CanSlewAltAz => true;
		public bool CanSlewAltAzAsync => true;
		public bool CanSlewAsync => true;
		public bool CanSync => true;
		public bool CanSyncAltAz => true;
		public bool CanUnpark => true;

		public double Declination
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockRaDec.Y;
			}
		}

		public double DeclinationRate
		{
			get => MockMoveAxisRates.Y;
			set => MockMoveAxisRates = new Vector( MockMoveAxisRates.X, value );
		}

		public bool DoesRefraction
		{
			get
			{
				return false;
			}
			set
			{ }
		}

		public EquatorialCoordinateType EquatorialSystem => EquatorialCoordinateType.equTopocentric;
		public double FocalLength => 0.910;

		public double GuideRateDeclination { get; set; }
		public double GuideRateRightAscension { get; set; }

		public bool IsPulseGuiding
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockIsPulseGuiding;
			}
		}

		public double RightAscension
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockRaDec.X;
			}
		}

		public double RightAscensionRate
		{
			get => MockMoveAxisRates.X;
			set => MockMoveAxisRates = new Vector( value, MockMoveAxisRates.Y );
		}

		public PierSide SideOfPier
		{
			get => MockSideOfPier;
			set
			{
				if ( value != MockSideOfPier )
				{
					MockSlewing = true;

					Task.Run( () =>
					{
						Thread.Sleep( 1000 );

						MockSideOfPier = value;
						MockSlewing = false;
					} );
				}
			}
		}

		public double SiderealTime
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return GetSiderealTime();
			}
		}

		public double SiteElevation { get; set; }
		public double SiteLatitude { get; set; }
		public double SiteLongitude { get; set; }

		public bool Slewing
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockSlewing;
			}
		}

		public short SlewSettleTime { get; set; }

		private double _targetDeclination;

		public double TargetDeclination
		{
			get
			{
				if ( Double.IsNaN( _targetDeclination ) )
				{
					throw new ASCOM.InvalidOperationException( "An attempt was made to read the TargetDeclination before it was set." );
				}

				return _targetDeclination;
			}
			set { _targetDeclination = value; }
		}

		private double _targetRightAscension;

		public double TargetRightAscension
		{
			get
			{
				if ( Double.IsNaN( _targetRightAscension ) )
				{
					throw new ASCOM.InvalidOperationException( "An attempt was made to read the TargetRightAscension before it was set." );
				}

				return _targetRightAscension;
			}
			set { _targetRightAscension = value; }
		}

		public bool Tracking
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockTracking;
			}
			set
			{
				MockTracking = value;
			}
		}

		public DriveRates TrackingRate
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockTrackingRate;
			}
			set
			{
				MockTrackingRate = value;
			}
		}

		private ITrackingRates _trackingRates;

		public ITrackingRates TrackingRates
		{
			get
			{
				return _trackingRates;
			}
			set
			{
				_trackingRates = value;
			}
		}

		public DateTime UTCDate
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockUTCDate;
			}
			set
			{
				MockUTCDate = value;
			}
		}

		public void AbortSlew()
		{
			if ( !DeviceAvailable )
			{
				throw new NotConnectedException();
			}

			if ( !Slewing )
			{
				return;
			}

			MockSlewing = false;
		}

		public string Action( string ActionName, string ActionParameters )
		{
			throw new MethodNotImplementedException();
		}

		public IAxisRates AxisRates( TelescopeAxes axis )
		{
			if ( axis == TelescopeAxes.axisPrimary )
			{
				return MockPrimaryAxisRates;
			}
			else if ( axis == TelescopeAxes.axisSecondary )
			{
				return MockSecondaryAxisRates;
			}
			else
			{
				return MockTertiaryAxisRates;
			}
		}

		public bool CanMoveAxis( TelescopeAxes Axis )
		{
			bool retval = false;

			switch ( Axis )
			{
				case TelescopeAxes.axisPrimary:
				case TelescopeAxes.axisSecondary:
					retval = true;
					break;
			}

			return retval;
		}

		public void CommandBlind( string Command, bool Raw = false )
		{
			throw new MethodNotImplementedException();
		}

		public bool CommandBool( string Command, bool Raw = false )
		{
			throw new MethodNotImplementedException();
		}

		public string CommandString( string Command, bool Raw = false )
		{
			throw new MethodNotImplementedException();
		}

		public void CreateDevice( string id )
		{
			if ( Initialized )
			{
				throw new Exception( "The telescope service attempted to re-initialize the telescope." );
			}

			if ( String.IsNullOrEmpty( id ) )
			{
				throw new Exception( "The telescope service is unable to create a telescope until a Telescope has been chosen." );
			}

			DeviceID = id;
			Initialized = true;
		}

		public PierSide DestinationSideOfPier( double rightAscension, double declination )
		{
			return MockSideOfPier;
		}

		public void Dispose()
		{
			if ( TrackingRates != null )
			{
				if ( _cancelMoveTokenSource != null )
				{
					_cancelMoveTokenSource.Cancel();
					Thread.Sleep( TIMER_INTERVAL * 2 );
					_cancelMoveTokenSource = null;
				}

				TrackingRates.Dispose();
				TrackingRates = null;
			}
		}

		public void FindHome()
		{
			throw new MethodNotImplementedException();
		}

		public void MoveAxis( TelescopeAxes Axis, double Rate )
		{
			if ( InterfaceVersion == 1 )
			{
				throw new MethodNotImplementedException( "MoveAxis" );
			}

			CheckRate( Axis, Rate );

			if ( !CanMoveAxis( Axis ) )
			{
				throw new MethodNotImplementedException( "CanMoveAxis " + Enum.GetName( typeof( TelescopeAxes ), Axis ) );
			}

			switch ( Axis )
			{
				case TelescopeAxes.axisPrimary:
					MockMoveAxisRates = new Vector( Rate, MockMoveAxisRates.Y );
					break;

				case TelescopeAxes.axisSecondary:
					MockMoveAxisRates = new Vector( MockMoveAxisRates.X, Rate );
					break;

				case TelescopeAxes.axisTertiary:
					// Not implemented
					break;
			}
		}

		public void Park()
		{
			if ( !DeviceAvailable )
			{
				throw new NotConnectedException();
			}

			Tracking = false;
			MockAtPark = true;
		}

		public void PulseGuide( GuideDirections Direction, int Duration )
		{
			throw new System.NotImplementedException();
		}

		public void SetPark()
		{
			throw new System.NotImplementedException();
		}

		public void SetupDialog()
		{
			throw new System.NotImplementedException();
		}

		public void SlewToAltAz( double Azimuth, double Altitude )
		{
			throw new System.NotImplementedException();
		}

		public void SlewToAltAzAsync( double Azimuth, double Altitude )
		{
			throw new System.NotImplementedException();
		}

		public void SlewToCoordinates( double RightAscension, double Declination )
		{
			throw new System.NotImplementedException();
		}

		public void SlewToCoordinatesAsync( double rightAscension, double declination )
		{
			CheckCapability( CanSlewAsync, "SlewToCoordinatesAsync" );
			CheckRange( rightAscension, 0, 24, "SlewToCoordinatesAsync", "RightAscension" );
			CheckRange( declination, -90, 90, "SlewToCoordinatesAsync", "Declination" );
			CheckParked( "SlewToCoordinatesAsync" );
			CheckTracking( true, "SlewToCoordinatesAsync" );

			TargetRightAscension = rightAscension;
			TargetDeclination = declination;

			MockSlewing = true;

			Task.Run( () =>
			{
				Thread.Sleep( 1500 );

				MockRaDec = new Vector( rightAscension, declination );
				MockSlewing = false;
			} );
		}

		public void SlewToTarget()
		{
			throw new System.NotImplementedException();
		}

		public void SlewToTargetAsync()
		{
			if ( !DeviceAvailable )
			{
				throw new NotConnectedException();
			}

			MockSlewing = true;

			Task.Run( () =>
			{
				Thread.Sleep( 500 );

				MockRaDec = new Vector( TargetRightAscension, TargetDeclination );
				MockSlewing = false;
			} );
		}

		public void SyncToAltAz( double Azimuth, double Altitude )
		{
			throw new System.NotImplementedException();
		}

		public void SyncToCoordinates( double RightAscension, double Declination )
		{
			throw new System.NotImplementedException();
		}

		public void SyncToTarget()
		{
			throw new System.NotImplementedException();
		}

		public void Unpark()
		{
			MockAtPark = false;
		}

		#region Helper Methods

		private void CheckRate( TelescopeAxes axis, double rate )
		{
			IAxisRates rates = AxisRates( axis );
			string ratesStr = string.Empty;

			if ( rate == 0.0 )
			{
				return;
			}

			foreach ( AxisRate item in rates )
			{
				if ( Math.Abs( rate ) >= item.Minimum && Math.Abs( rate ) <= item.Maximum )
				{
					return;
				}

				ratesStr = string.Format( "{0}, {1} to {2}", ratesStr, item.Minimum, item.Maximum );
			}

			throw new InvalidValueException( "MoveAxis", rate.ToString( CultureInfo.InvariantCulture ), ratesStr );
		}

		private void CheckCapability( bool capability, string methodName )
		{
			if ( !capability )
			{
				throw new MethodNotImplementedException( methodName );
			}
		}

		private void CheckRange( double value, double min, double max, string propertyOrMethod, string valueName )
		{
			if ( Double.IsNaN( value ) )
			{
				throw new ValueNotSetException( propertyOrMethod + ":" + valueName );
			}
			if ( value < min || value > max )
			{
				throw new InvalidValueException( propertyOrMethod, value.ToString( CultureInfo.CurrentCulture )
												, String.Format( CultureInfo.CurrentCulture, "{0}, {1} to {2}", valueName, min, max ) );
			}
		}

		private void CheckParked( string methodName )
		{
			if ( AtPark )
			{
				throw new ParkedException( methodName );
			}
		}

		private void CheckTracking( bool raDecSlew, string method )
		{
			if ( raDecSlew != Tracking )
			{
				throw new ASCOM.InvalidOperationException( String.Format( "{0} is not allowed when tracking is {1}", method, Tracking ) );
			}
		}

		private Vector DoSlew()
		{
			Vector change = new Vector();

			if ( !Slewing )
			{
				return change;
			}

			// Move towards the target position

			double delta;
			bool finished = true;
			delta = _targetAxes.X - _mountAxes.X;

			while ( delta < -180 || delta > 180 )
			{
				if ( delta < -180 ) delta += 360;
				if ( delta > 180 ) delta -= 360;
			}

			int signDelta = delta < 0 ? -1 : +1;
			delta = Math.Abs( delta );

			if ( delta < slewSpeedSlow )
			{
				change.X = delta * signDelta;
			}
			else if ( delta < slewSpeedMedium * 2 )
			{
				change.X = slewSpeedSlow * signDelta;
				finished = false;
			}
			else if ( delta < slewSpeedFast * 2 )
			{
				change.X = slewSpeedMedium * signDelta;
				finished = false;
			}
			else
			{
				change.X = slewSpeedFast * signDelta;
				finished = false;
			}

			delta = _targetAxes.Y - _mountAxes.Y;

			while ( delta < -180 || delta > 180 )
			{
				if ( delta < -180 ) delta += 360;
				if ( delta > 180 ) delta -= 360;
			}

			signDelta = delta < 0 ? -1 : +1;
			delta = Math.Abs( delta );

			if ( delta < slewSpeedSlow )
			{
				change.Y = delta * signDelta;
			}
			else if ( delta < slewSpeedMedium * 2 )
			{
				change.Y = slewSpeedSlow * signDelta;
				finished = false;
			}
			else if ( delta < slewSpeedFast * 2 )
			{
				change.Y = slewSpeedMedium * signDelta;
				finished = false;
			}
			else
			{
				change.Y = slewSpeedFast * signDelta;
				finished = false;
			}
			if ( finished )
			{
				MockSlewing = false;

				switch ( MockSlewState )
				{
					case SlewType.SlewRaDec:
					case SlewType.SlewAltAz:
						MockSlewState = SlewType.SlewSettle;
						_settleTime = DateTime.Now + TimeSpan.FromSeconds( SlewSettleTime );
						break;

					case SlewType.SlewPark:
						MockSlewState = SlewType.SlewNone;
						MockAtPark = true;
						break;

					case SlewType.SlewHome:
						MockSlewState = SlewType.SlewNone;
						break;
					case SlewType.SlewNone:
						break;

					default:
						MockSlewState = SlewType.SlewNone;
						break;
				}
			}
			return change;
		}

		private Vector PulseGuide( double updateInterval )
		{
			Vector change = new Vector( 0.0, 0.0 );

			if ( isPulseGuidingRa )
			{
				if ( guideDuration.X <= 0.0 )
				{
					isPulseGuidingRa = false;
				}
				else
				{
					change.X = CalcRaPulseGuideChange( updateInterval );
				}
			}
			if ( isPulseGuidingDec )
			{
				if ( guideDuration.Y <= 0 )
				{
					isPulseGuidingDec = false;
				}
				else
				{
					change.Y = CalcDecPulseGuideChange( updateInterval );
				}
			}

			return change;
		}

		private double CalcRaPulseGuideChange( double updateInterval )
		{
			// Assume polar mount only

			double deltaTime = Math.Min( updateInterval, guideDuration.X );
			guideDuration.X -= deltaTime;

			// assumes guide rate is in deg/sec

			double guideChange = guideRate.X * deltaTime;

			return guideChange;
		}

		private double CalcDecPulseGuideChange( double updateInterval )
		{
			double deltaTime = Math.Min( updateInterval, guideDuration.Y );
			guideDuration.Y -= deltaTime;

			double guideChange = guideRate.Y * deltaTime;

			return guideChange;
		}

		private void CheckAxisLimits( double primaryChange )
		{
			// check the ranges of the axes
			// primary axis must be in the range 0 to 360 for AltAz or Polar
			// and -hourAngleLimit to 180 + hourAngleLimit for german polar
			switch ( MockAlignmentMode )
			{
				case AlignmentModes.algAltAz:
					// the primary axis must be in the range 0 to 360

					_mountAxes.X = NormalizeAzimuth( _mountAxes.X );
					break;

				case AlignmentModes.algGermanPolar:
					// the primary axis needs to be in the range -180 to +180 to correspond with hour angles of -12 to 12.
					// check if we have hit the hour angle limit

					if ( ( _mountAxes.X >= hourAngleLimit + 180 && primaryChange > 0 ) ||
						( _mountAxes.X <= -hourAngleLimit && primaryChange < 0 ) )
					{
						// undo the movement when the limit is hit
						_mountAxes.X -= primaryChange;
					}

					break;

				case AlignmentModes.algPolar:
					// the axis needs to be in the range -180 to +180 to correspond with hour angles
					// of -12 to 12.

					while ( _mountAxes.X <= -180.0 || _mountAxes.X > 180.0 )
					{
						if ( _mountAxes.X <= -180.0 ) _mountAxes.X += 360;
						if ( _mountAxes.X > 180 ) _mountAxes.X -= 360;
					}

					break;
			}

			// secondary must be in the range -90 to 0 to +90 for normal 
			// and +90 to 180 to 270 for through the pole.
			// rotation is continuous

			while ( _mountAxes.Y >= 270.0 || _mountAxes.Y < -90.0 )
			{
				if ( _mountAxes.Y >= 270 ) _mountAxes.Y -= 360.0;
				if ( _mountAxes.Y < -90 ) _mountAxes.Y += 360.0;
			}
		}

		private void UpdatePositions()
		{
			GetSiderealTime();

			MockAltAz = ConvertAxesToAltAzm( _mountAxes );
			MockRaDec = ConvertAxesToRaDec( _mountAxes );
		}

		private Vector ConvertAxesToAltAzm( Vector axes )
		{
			Vector altAzm = axes;

			switch ( MockAlignmentMode )
			{
				case ASCOM.DeviceInterface.AlignmentModes.algAltAz:
					break;

				case AlignmentModes.algGermanPolar:
					if ( axes.Y > 90 )
					{
						axes.X += 180;
						axes.Y = 180 - axes.Y;
					}

					if ( SiteLatitude < 0 )
					{
						axes.Y = -axes.Y;
					}

					double ra = SiderealTime - (axes.X / 15.0);
					altAzm = CalculateAltAzFromRaDec( ra, axes.Y );

					break;

				case ASCOM.DeviceInterface.AlignmentModes.algPolar:
					ra = SiderealTime - (axes.X / 15.0);

					if ( SiteLatitude < 0 )
					{
						axes.Y = -axes.Y;
					}

					altAzm = CalculateAltAzFromRaDec( ra, axes.Y );
					break;
			}

			return NormalizeAltAz( altAzm );
		}

		private Vector ConvertAxesToRaDec( Vector axes )
		{
			Vector raDec = new Vector();

			switch ( MockAlignmentMode )
			{
				case ASCOM.DeviceInterface.AlignmentModes.algAltAz:
					raDec = CalculateRaDecFromAltAz( axes );

					break;

				case ASCOM.DeviceInterface.AlignmentModes.algGermanPolar:
				case ASCOM.DeviceInterface.AlignmentModes.algPolar:
					// undo through the pole

					if ( axes.Y > 90 )
					{
						axes.X += 180.0;
						axes.Y = 180 - axes.Y;
						axes = NormalizeAltAz( axes );
					}

					raDec.X = SiderealTime - ( axes.X / 15.0 );
					raDec.Y = ( SiteLatitude >= 0 ) ? axes.Y : -axes.Y;

					break;
			}

			return NormalizeRaDec( raDec );
		}

		private Vector NormalizeRaDec( Vector raDec )
		{
			return new Vector( NormalizeRightAscension( raDec.X ), NormalizeDeclination( raDec.Y ) );
		}

		private Vector NormalizeAltAz( Vector altAzm )
		{
			return new Vector( NormalizeAzimuth( altAzm.X ), NormalizeAltitude( altAzm.Y ) );
		}

		private double NormalizeAzimuth( double azimuth )
		{
			return NormalizeValue( azimuth, 0.0, 360.0 );
		}

		private double NormalizeAltitude( double altitude )
		{
			return NormalizeValue( altitude, -90.0, 90.0 );
		}

		private double NormalizeDeclination( double declination )
		{
			return NormalizeValue( declination, -90.0, 90.0 );
		}

		private double NormalizeRightAscension( double rightAscension)
		{
			return NormalizeValue( rightAscension, 0.0, 24.0 );
		}

		private double NormalizeValue( double inValue, double minValue, double maxValue )
		{
			Debug.Assert( minValue < maxValue );

			double range = maxValue - minValue;

			while ( inValue < minValue )
			{
				inValue += range;
			}

			while ( inValue > maxValue )
			{
				inValue -= range;
			}

			return inValue;
		}

		private Vector CalculateAltAzFromRaDec( double ra, double dec )
		{
			Debug.Assert( EquatorialSystem == EquatorialCoordinateType.equTopocentric || EquatorialSystem == EquatorialCoordinateType.equJ2000 );
			double targetAltitude = Double.NaN;
			double targetAzimuth = Double.NaN;

			try
			{
				Transform xform = new Transform
				{
					SiteElevation = SiteElevation,
					SiteLatitude = SiteLatitude,
					SiteLongitude = SiteLongitude
				};


				if ( EquatorialSystem == EquatorialCoordinateType.equTopocentric )
				{
					xform.SetTopocentric( ra, dec );
				}
				else
				{
					xform.SetJ2000( ra, dec );
				}

				targetAltitude = xform.ElevationTopocentric;
				targetAzimuth = xform.AzimuthTopocentric;				
			}
			catch ( Exception )
			{}

			return new Vector( targetAzimuth, targetAltitude );
		}

		private Vector CalculateRaDecFromAltAz( Vector altAz )
		{
			Debug.Assert( EquatorialSystem == EquatorialCoordinateType.equTopocentric || EquatorialSystem == EquatorialCoordinateType.equJ2000 );

			double targetRightAscension = Double.NaN;
			double targetDeclination = Double.NaN;

			try
			{
				Transform xform = new Transform
				{
					SiteElevation = SiteElevation,
					SiteLatitude = SiteLatitude,
					SiteLongitude = SiteLongitude
				};

				xform.SetAzimuthElevation( altAz.X, altAz.Y );

				if ( EquatorialSystem == EquatorialCoordinateType.equTopocentric )
				{
					targetRightAscension = xform.RATopocentric;
					targetDeclination = xform.DECTopocentric;
				}
				else
				{
					targetRightAscension = xform.RAJ2000;
					targetDeclination = xform.DecJ2000;
				}

			}
			catch ( Exception )
			{ }

			return new Vector( targetRightAscension, targetDeclination );
		}

		private double GetSiderealTime()
		{
			double longitude = SiteLongitude;

			if ( double.IsNaN( longitude ) )
			{
				return Double.NaN;
			}

			DateTime targetTime = DateTime.Now;

			return LocalApparentSiderealTime( targetTime, longitude );
		}

		public static double LocalApparentSiderealTime( DateTime target, double longitude )
		{
			// Calculation from http://aa.usno.navy.mil/faq/docs/GAST.php

			if ( Double.IsNaN( longitude ) || longitude < -180.0 || longitude > 180.0 )
			{
				throw new ArgumentException( "Unable to calculate the Local Sidereal Time due to an invalid longitude", "longitude" );
			}

			double jdj2000 = 2451545.0;

			Util util = new Util();
			double jd = util.DateLocalToJulian( target );
			double d = jd - jdj2000;
			double GMST = 18.697374558 + (24.06570982441908 * d);

			// calculate the longitude of the ascending node of the moon.

			double omega = 125.04 - (.052954 * d);

			// calculate the mean longitude of the sun

			double longSun = 280.47 + (.98565 * d);

			// calculate the ubliquity

			double obliquity = 23.4393 - (0.0000004 * d);

			// calculate the nutation, in hours.

			double nutation = (-0.000319 * Math.Sin( omega * Math.PI / 180.0 )) - (0.000024 * Math.Sin( 2 * longSun * Math.PI / 180.0 ));

			// calculate the equation of the equinoxes

			double eqeq = nutation * Math.Cos( obliquity * Math.PI / 180.0 );

			// calculate the Greenwich Apparent Sidereal Time.

			double GAST = GMST + eqeq;

			// calculate the LocalApparentSiderealTime.

			double hourOffset = longitude / 15.0;   // Convert from degrees to hours.
			double LAST = GAST + hourOffset;

			// Normalize to the range of 0 to 24.

			double normalizedLAST = Math.IEEERemainder( LAST, 24.0 );

			return normalizedLAST;
		}

		#endregion
	}
}
