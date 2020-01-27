using System;
using System.Linq;
using System.Threading;
using System.Windows;
using ASCOM.Astrometry.Transform;
using ASCOM.DeviceHub;
using ASCOM.DeviceInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_Tests.Telescope
{
	[TestClass]
	public class TelescopeManagerTests
	{
		private const string ScopeID = "Mock Telescope";
		private const double _tolerance = 0.00001;

		private TelescopeManager _mgr;
		private MockTelescopeService _svc;
		private int _startupDelayMs = 3000;

		[ClassInitialize]
		public static void ClassInit( TestContext context )
		{
		}

		[TestInitialize]
		public void TestInit()
		{
			ServiceContainer.Instance.ClearAllServices();
			ServiceContainer.Instance.AddService<ITelescopeService>( new MockTelescopeService() );
			_mgr = TelescopeManager.Instance;
			_svc = (MockTelescopeService)ServiceContainer.Instance.GetService<ITelescopeService>();

			bool retval = _mgr.Connect( ScopeID );
			Assert.AreEqual( true, retval );

			Thread.Sleep( _startupDelayMs );
		}

		[TestCleanup]
		public void TestCleanup()
		{
			_mgr.Disconnect();
			Assert.IsFalse( _svc.Connected );

			_mgr.Dispose();
		}

		[TestMethod]
		public void ConnectTest()
		{
			Assert.AreEqual( true, _mgr.Connected );
			Assert.AreEqual( true, _mgr.Capabilities.CanFindHome );
			Assert.AreEqual( true, _mgr.Capabilities.CanMovePrimaryAxis );
			Assert.AreEqual( true, _mgr.Capabilities.CanMoveSecondaryAxis );
			Assert.AreEqual( false, _mgr.Capabilities.CanMoveTertiaryAxis );
			Assert.AreEqual( true, _mgr.Capabilities.CanPark );
			Assert.AreEqual( true, _mgr.Capabilities.CanPulseGuide );
			Assert.AreEqual( true, _mgr.Capabilities.CanSetDeclinationRate );
			Assert.AreEqual( true, _mgr.Capabilities.CanSetGuideRates );
			Assert.AreEqual( true, _mgr.Capabilities.CanSetPark );
			Assert.AreEqual( true, _mgr.Capabilities.CanSetPierSide );
			Assert.AreEqual( true, _mgr.Capabilities.CanSetRightAscensionRate );
			Assert.AreEqual( true, _mgr.Capabilities.CanSetTracking );
			Assert.AreEqual( true, _mgr.Capabilities.CanSlew );
			Assert.AreEqual( true, _mgr.Capabilities.CanSlewAltAz );
			Assert.AreEqual( true, _mgr.Capabilities.CanSlewAltAz );
			Assert.AreEqual( true, _mgr.Capabilities.CanSlewAltAzAsync );
			Assert.AreEqual( true, _mgr.Capabilities.CanSlewAsync );
			Assert.AreEqual( true, _mgr.Capabilities.CanSync );
			Assert.AreEqual( true, _mgr.Capabilities.CanSyncAltAz );
			Assert.AreEqual( true, _mgr.Capabilities.CanUnpark );
			Assert.AreEqual( _svc.MockAlignmentMode, _mgr.Parameters.AlignmentMode );
			Assert.AreEqual( _svc.ApertureArea, _mgr.Parameters.ApertureArea );
			Assert.AreEqual( _svc.ApertureDiameter, _mgr.Parameters.ApertureDiameter );
			Assert.AreEqual( _svc.DoesRefraction, _mgr.Parameters.DoesRefraction );
			Assert.AreEqual( _svc.DriverVersion, _mgr.Parameters.DriverVersion );
			Assert.AreEqual( _svc.EquatorialSystem, _mgr.Parameters.EquatorialSystem );
			Assert.AreEqual( _svc.FocalLength, _mgr.Parameters.FocalLength );
			Assert.AreEqual( 3, _mgr.Parameters.InterfaceVersion );
			Assert.AreEqual( _svc.SiteElevation, _mgr.Parameters.SiteElevation );
			Assert.AreEqual( _svc.SiteLatitude, _mgr.Parameters.SiteLatitude );
			Assert.AreEqual( _svc.SiteLongitude, _mgr.Parameters.SiteLongitude );
			//Be sure that all the drive rates are present, regardless of the order.
			Assert.AreEqual( 4, _mgr.Parameters.TrackingRates.Count() );
			Assert.IsNotNull( _mgr.Parameters.TrackingRates.Where( r => r.Rate == DriveRates.driveKing ).FirstOrDefault() );
			Assert.IsNotNull( _mgr.Parameters.TrackingRates.Where( r => r.Rate == DriveRates.driveLunar ).FirstOrDefault() );
			Assert.IsNotNull( _mgr.Parameters.TrackingRates.Where( r => r.Rate == DriveRates.driveSidereal ).FirstOrDefault() );
			Assert.IsNotNull( _mgr.Parameters.TrackingRates.Where( r => r.Rate == DriveRates.driveSolar ).FirstOrDefault() );
		}

		[TestMethod]
		public void DisconnectTest()
		{
			// Disconnection is tested in the TestCleanup method.
		}

		[TestMethod]
		public void JogScopePrimaryTest()
		{
			IRate[] rateArr = GetAxisRateArrayFromService( TelescopeAxes.axisPrimary );
			JogRates jogRates = JogRates.FromAxisRates( rateArr );
			Assert.IsTrue( jogRates.Count > 0 );

			// Start moving the scope to the west.

			_mgr.StartJogScope( MoveDirections.West, jogRates[0] );

			double actualRate = _svc.MockMoveAxisRates.X;
			Assert.AreEqual( jogRates[0].Rate, actualRate );

			_mgr.StopJogScope( MoveDirections.West );

			actualRate = _svc.MockMoveAxisRates.X;
			Assert.AreEqual( 0.0, actualRate );

			// Start moving the scope to the east.

			_mgr.StartJogScope( MoveDirections.East, jogRates[0] );

			actualRate = _svc.MockMoveAxisRates.X;
			Assert.AreEqual( jogRates[0].Rate, actualRate * -1 );

			_mgr.StopJogScope( MoveDirections.East );

			actualRate = _svc.MockMoveAxisRates.X;
			Assert.AreEqual( 0.0, actualRate );
		}

		[TestMethod]
		public void JogScopeSecondaryTest()
		{
			IRate[] rateArr = GetAxisRateArrayFromService( TelescopeAxes.axisSecondary );
			JogRates jogRates = JogRates.FromAxisRates( rateArr );
			Assert.IsTrue( jogRates.Count > 0 );

			// Start moving the scope to the north.

			_mgr.StartJogScope( MoveDirections.North, jogRates[0] );

			double actualRate = _svc.MockMoveAxisRates.Y;
			Assert.AreEqual( jogRates[0].Rate, actualRate );

			_mgr.StopJogScope( MoveDirections.North );

			actualRate = _svc.MockMoveAxisRates.Y;
			Assert.AreEqual( 0.0, actualRate );

			// Start moving the scope to the south.

			_mgr.StartJogScope( MoveDirections.South, jogRates[0] );

			actualRate = _svc.MockMoveAxisRates.Y;
			Assert.AreEqual( jogRates[0].Rate, actualRate * -1 );

			_mgr.StopJogScope( MoveDirections.South );

			actualRate = _svc.MockMoveAxisRates.Y;
			Assert.AreEqual( 0.0, actualRate );
		}

		[TestMethod]
		public void StartFixedSlewTest()
		{
			// Unpark the scope and turn tracking on.

			_mgr.SetParkingState( ParkingStateEnum.Unparked );
			_mgr.Tracking = true;

			// Give the service a sane starting position.

			Vector targetRaDec = GetTargetRaDec();

			// Give the scope a good pointing position and move the scope east.

			_svc.MockRaDec = targetRaDec;

			ForceTelescopeStatusUpdate();
			_mgr.StartFixedSlew( MoveDirections.East, 4.0 );

			// Wait until it stops slewing.

			while ( _mgr.Slewing )
			{
				Thread.Sleep( 250 );
			}

			// Verify that it moved.

			Assert.AreNotEqual( targetRaDec.X, _svc.RightAscension, _tolerance );
			Assert.AreNotEqual( targetRaDec.X, _svc.TargetRightAscension, _tolerance );
			Assert.AreEqual( targetRaDec.Y, _svc.Declination, _tolerance );
			Assert.AreEqual( targetRaDec.Y, _svc.TargetDeclination, _tolerance );

			// Restore the original position and move the scope west.

			_svc.MockRaDec = targetRaDec;

			ForceTelescopeStatusUpdate();
			_mgr.StartFixedSlew( MoveDirections.West, 4.0 );

			// Wait until it stops slewing.

			while ( _mgr.Slewing )
			{
				Thread.Sleep( 250 );
			}

			// Verify that it moved.

			Assert.AreNotEqual( targetRaDec.X, _svc.RightAscension, _tolerance );
			Assert.AreNotEqual( targetRaDec.X, _svc.TargetRightAscension, _tolerance );
			Assert.AreEqual( targetRaDec.Y, _svc.Declination, _tolerance );
			Assert.AreEqual( targetRaDec.Y, _svc.TargetDeclination, _tolerance );

			// Restore the original position and move the scope North.

			_svc.MockRaDec = targetRaDec;

			ForceTelescopeStatusUpdate();
			_mgr.StartFixedSlew( MoveDirections.North, 4.0 );

			// Wait until it stops slewing.

			while ( _mgr.Slewing )
			{
				Thread.Sleep( 250 );
			}

			// Verify that it moved.

			Assert.AreEqual( targetRaDec.X, _svc.RightAscension, _tolerance );
			Assert.AreEqual( targetRaDec.X, _svc.TargetRightAscension, _tolerance );
			Assert.AreNotEqual( targetRaDec.Y, _svc.Declination, _tolerance );
			Assert.AreNotEqual( targetRaDec.Y, _svc.TargetDeclination, _tolerance );

			// Restore the original target position and move the scope South.

			_svc.MockRaDec = targetRaDec;

			ForceTelescopeStatusUpdate();
			_mgr.StartFixedSlew( MoveDirections.South, 4.0 );

			// Wait until it stops slewing.

			while ( _mgr.Slewing )
			{
				Thread.Sleep( 250 );
			}

			// Verify that it moved.

			Assert.AreEqual( targetRaDec.X, _svc.RightAscension, _tolerance );
			Assert.AreEqual( targetRaDec.X, _svc.TargetRightAscension, _tolerance );
			Assert.AreNotEqual( targetRaDec.Y, _svc.Declination, _tolerance );
			Assert.AreNotEqual( targetRaDec.Y, _svc.TargetDeclination, _tolerance );
		}

		[TestMethod]
		public void AbortSlewTest()
		{
			// Put the service in a Slewing state.

			_svc.MockSlewing = true;
			Assert.IsTrue( _mgr.Slewing );

			_mgr.AbortSlew();
			Assert.IsFalse( _svc.MockSlewing );
			Assert.IsFalse( _mgr.Slewing );
		}

		[TestMethod]
		public void SetParkingStateTest()
		{
			_svc.MockAtPark = true;
			Assert.IsTrue( _mgr.AtPark );

			_mgr.SetParkingState( ParkingStateEnum.Unparked );

			Thread.Sleep( _startupDelayMs );

			Assert.IsFalse( _svc.MockAtPark );
			Assert.IsFalse( _mgr.AtPark );

			_mgr.SetParkingState( ParkingStateEnum.IsAtPark );

			Thread.Sleep( _startupDelayMs );

			Assert.IsTrue( _svc.MockAtPark );
			Assert.IsTrue( _mgr.AtPark );
		}

		[TestMethod]
		public void DirectSlewTest()
		{
			// Unpark the scope and start tracking.

			_svc.MockAtPark = false;
			_svc.MockTracking = true;

			Thread.Sleep( 1000 );

			Assert.IsFalse( _mgr.AtPark );
			Assert.IsTrue( _mgr.Tracking );

			// Give the service a sane target position.

			Vector targetRaDec = GetTargetRaDec();

			_mgr.BeginSlewToCoordinatesAsync( targetRaDec.X, targetRaDec.Y );

			while( _mgr.Slewing )
			{
				Thread.Sleep( 1000 );
			}

			Assert.AreEqual( _svc.MockRaDec.X, targetRaDec.X, _tolerance );
			Assert.AreEqual( _svc.MockRaDec.Y, targetRaDec.Y, _tolerance );
		}

		[TestMethod]
		public void SetTrackingTest()
		{
			// Unpark the scope and start tracking.

			_svc.MockAtPark = false;
			_svc.MockTracking = true;

			Thread.Sleep( 1000 );

			Assert.IsFalse( _mgr.AtPark );
			Assert.IsTrue( _mgr.Tracking );

			_mgr.SetTracking( false );

			Thread.Sleep( 1000 );

			Assert.IsFalse( _svc.Tracking );
			Assert.IsFalse( _mgr.Tracking );

			_mgr.SetTracking( true );

			Thread.Sleep( 1000 );

			Assert.IsTrue( _svc.Tracking );
			Assert.IsTrue( _mgr.Tracking );
		}

		[TestMethod]
		public void MeridianFlipTest()
		{
			// Unpark the scope and start tracking.

			_svc.MockAtPark = false;
			_svc.MockTracking = true;
			_svc.MockSideOfPier = PierSide.pierWest;

			Thread.Sleep( 1000 );

			Assert.IsFalse( _mgr.AtPark );
			Assert.IsTrue( _mgr.Tracking );
			Assert.IsTrue( _mgr.Capabilities.CanSetPierSide );

			// Verify that both the Manager and the Service have the same side-of-pier and that
			// it is not Unknown.

			PierSide sop = _mgr.SideOfPier;
			Assert.IsTrue( sop != PierSide.pierUnknown );
			Assert.IsTrue( sop == _svc.SideOfPier );

			// Do the flip.

			_mgr.StartMeridianFlip();

			// Figure out which side we should end up on after the flip.

			PierSide targetSop = sop == PierSide.pierEast ? PierSide.pierWest : PierSide.pierEast;

			// Wait for the flip.

			Thread.Sleep( 1500 );

			// Make sure that we are done and on the expected side of the mount.

			Assert.IsFalse( _mgr.Slewing );
			Assert.AreEqual( targetSop, _mgr.SideOfPier );
		}

		[TestMethod]
		public void SetRaOffsetTrackingRateTest()
		{
			double raRate = 0.95;

			Assert.IsTrue( _mgr.Connected );
			Assert.IsTrue( _mgr.Capabilities.CanSetRightAscensionRate );

			_mgr.SetRaOffsetTrackingRate( raRate );

			Assert.AreEqual( _svc.RightAscensionRate, raRate, 0.0001 );

			_mgr.SetRaOffsetTrackingRate( 0.0 );

			Assert.AreEqual( _svc.RightAscensionRate, 0.0, 0.0001 );
		}

		[TestMethod]
		public void SetDecOffsetTrackingRateTest()
		{
			double decRate = 0.95;

			Assert.IsTrue( _mgr.Connected );
			Assert.IsTrue( _mgr.Capabilities.CanSetDeclinationRate );

			_mgr.SetDecOffsetTrackingRate( decRate );

			Assert.AreEqual( _svc.DeclinationRate, decRate, 0.0001 );

			_mgr.SetDecOffsetTrackingRate( 0.0 );

			Assert.AreEqual( _svc.DeclinationRate, 0.0, 0.0001 );
		}

		#region Helper Methods

		private Vector GetTargetRaDec()
		{
			// Return a sane slew target.

			double altitude = 50.0;
			double azimuth = 135.0;
			double targetRightAscension = Double.NaN;
			double targetDeclination = Double.NaN;

			try
			{
				Transform xform = new Transform
				{
					SiteElevation = _svc.SiteElevation,
					SiteLatitude = _svc.SiteLatitude,
					SiteLongitude = _svc.SiteLongitude
				};

				xform.SetAzimuthElevation( azimuth, altitude );

				if ( _svc.EquatorialSystem == EquatorialCoordinateType.equTopocentric )
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

		private void ForceTelescopeStatusUpdate()
		{
			// Call the private method to force the Status property to be updated.

			PrivateObject pobj = new PrivateObject( _mgr );
			pobj.Invoke( "UpdateScopeStatusTask" );
		}

		private IRate[] GetAxisRateArrayFromService( TelescopeAxes axis )
		{
			AxisRates rates;

			if ( axis == TelescopeAxes.axisPrimary )
			{
				rates = (AxisRates)_svc.MockPrimaryAxisRates;
			}
			else if ( axis == TelescopeAxes.axisSecondary )
			{
				rates = (AxisRates)_svc.MockSecondaryAxisRates;
			}
			else
			{
				rates = (AxisRates)_svc.MockTertiaryAxisRates;
			}

			IRate[] rateArr = new IRate[rates.Count];
			int i = 0;

			foreach ( IRate rate in rates )
			{
				rateArr[i++] = rate;
			}

			return rateArr;
		}

		#endregion
	}
}