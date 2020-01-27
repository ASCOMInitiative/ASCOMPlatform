using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASCOM.DeviceHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ASCOM.DeviceInterface;
using System.Windows;
using ASCOM.Astrometry.Transform;

namespace Unit_Tests.Telescope
{
	[TestClass]
	public class TelescopeMotionViewModelTests
	{
		private const string ScopeID = "Mock Telescope";
		private const double _tolerance = 0.00001;
		private const double _saneAltitude = 50.0;
		private const double _saneAzimuth = 135.0;
		private const double _siteLatitude = 31.5;
		private const double _siteLongitude = -110.33;
		private const double _siteElevation = 1300;

		private MockTelescopeManager _mgr = null;
		private TelescopeMotionViewModel _vm = null;
		private PrivateObject _prVm = null;

		[TestInitialize]
		public void TestInit()
		{
			ServiceContainer.Instance.ClearAllServices();
			ServiceContainer.Instance.AddService<IMessageBoxService>( new MockMessageBoxService() );
			_mgr = new MockTelescopeManager();

			_vm = new TelescopeMotionViewModel( _mgr );
			_prVm = new PrivateObject( _vm );

			Globals.UISyncContext = TaskScheduler.Default;
		}

		[TestMethod]
		public void ChangeTrackingTest()
		{
			_vm.Status = DevHubTelescopeStatus.GetEmptyStatus();
			_vm.Status.Connected = true;
			_vm.Status.Tracking = false;
			_prVm.SetFieldOrProperty( "_isTracking", false );

			_prVm.Invoke( "RegisterStatusUpdateMessage", true );

			_vm.IsTracking = true;
			_prVm.Invoke( "ChangeTracking" );
			
			Thread.Sleep( 2000 );

			Assert.IsTrue( _vm.Status.Tracking );
			Assert.IsTrue( _vm.IsTracking );

			_vm.IsTracking = false;
			_prVm.Invoke( "ChangeTracking" );

			Thread.Sleep( 500 );

			Assert.IsFalse( _vm.Status.Tracking );
			Assert.IsFalse( _vm.IsTracking );
					   
			_prVm.Invoke( "RegisterStatusUpdateMessage", false );
		}

		[TestMethod]
		public void CanChangeParkState()
		{
			TelescopeCapabilities caps = TelescopeCapabilities.GetFullCapabilities();
			DevHubTelescopeStatus status = DevHubTelescopeStatus.GetEmptyStatus();

			_vm.Status = null;
			_vm.Capabilities = caps;

			bool result = (bool)_prVm.Invoke( "CanChangeParkState" );
			Assert.IsFalse( result ); // No device status

			_vm.Status = status;
			_vm.Capabilities = null;

			result = (bool)_prVm.Invoke( "CanChangeParkState" );
			Assert.IsFalse( result ); // No device capabilities

			_vm.Capabilities = caps;

			result = (bool)_prVm.Invoke( "CanChangeParkState" );
			Assert.IsFalse( result ); // Not connected

			_vm.Status.Connected = true;

			// Set the state to Parked and test if we are allowed to unpark the scope.

			_vm.Status.ParkingState = ParkingStateEnum.IsAtPark;

			_vm.Capabilities.CanUnpark = false;

			result = (bool)_prVm.Invoke( "CanChangeParkState" );
			Assert.IsFalse( result ); // Not capable

			_vm.Capabilities.CanUnpark = true;

			result = (bool)_prVm.Invoke( "CanChangeParkState" );
			Assert.IsTrue( result ); // Can unpark

			// Set the state to Unparked and test if we are allowed to park the scope.

			_vm.Status.ParkingState = ParkingStateEnum.Unparked;
			_vm.Capabilities.CanPark = false;

			result = (bool)_prVm.Invoke( "CanChangeParkState" );
			Assert.IsFalse( result ); // Not capable
			
			_vm.Capabilities.CanPark = true;
			_vm.Status.Slewing = true;

			result = (bool)_prVm.Invoke( "CanChangeParkState" );
			Assert.IsFalse( result ); // Scope is already slewing

			_vm.Status.Slewing = false;

			result = (bool)_prVm.Invoke( "CanChangeParkState" );
			Assert.IsTrue( result ); // Can park
		}

		[TestMethod]
		public void ChangeParkState()
		{
			DevHubTelescopeStatus sts = DevHubTelescopeStatus.GetEmptyStatus();
			sts.ParkingState = ParkingStateEnum.IsAtPark;
			sts.AtPark = true;
			_mgr.Status = sts;
			_vm.Status = sts;

			_prVm.Invoke( "RegisterStatusUpdateMessage", true );

			_prVm.Invoke( "ChangeParkState" );
			Thread.Sleep( 2500 );
			Assert.IsFalse( _vm.Status.AtPark );

			_prVm.Invoke( "ChangeParkState" );

			Thread.Sleep( 2500 );
			Assert.AreEqual( ParkingStateEnum.IsAtPark, _vm.Status.ParkingState );

			_prVm.Invoke( "RegisterStatusUpdateMessage", false );
		}

		[TestMethod]
		public void CanDoMeridianFlip()
		{
			TelescopeParameters parms = new TelescopeParameters();
			DevHubTelescopeStatus status = DevHubTelescopeStatus.GetEmptyStatus();
			TelescopeCapabilities caps = TelescopeCapabilities.GetFullCapabilities();

			_vm.Status = null;
			_vm.Parameters = parms;
			_vm.Capabilities = caps;

			bool result = (bool)_prVm.Invoke( "CanDoMeridianFlip" );
			Assert.IsFalse( result ); // No device status

			_vm.Status = status;
			_vm.Parameters = null;

			result = (bool)_prVm.Invoke( "CanDoMeridianFlip" );
			Assert.IsFalse( result ); // No device parameters

			_vm.Parameters = parms;
			_vm.Capabilities = null;

			result = (bool)_prVm.Invoke( "CanDoMeridianFlip" );
			Assert.IsFalse( result ); // No capabilities

			_vm.Capabilities = caps;

			result = (bool)_prVm.Invoke( "CanDoMeridianFlip" );
			Assert.IsFalse( result ); // Not connected

			_vm.Status.Connected = true;

			_vm.Capabilities.CanSetPierSide = false;
			_vm.Capabilities.CanSlewAsync = false;

			result = (bool)_prVm.Invoke( "CanDoMeridianFlip" );
			Assert.IsFalse( result ); // Not capable

			_vm.Capabilities.CanSetPierSide = true;
			_vm.Capabilities.CanSlewAsync = true;

			_vm.Status.Tracking = false;
			_vm.Status.AtPark = true;

			result = (bool)_prVm.Invoke( "CanDoMeridianFlip" );
			Assert.IsFalse( result ); // Scope is parked

			_vm.Status.Tracking = true;
			_vm.Status.AtPark = false;

			_vm.Status.Slewing = true;

			result = (bool)_prVm.Invoke( "CanDoMeridianFlip" );
			Assert.IsFalse( result ); // Already slewing

			_vm.Status.Slewing = false;
			_vm.Parameters.AlignmentMode = AlignmentModes.algAltAz;

			result = (bool)_prVm.Invoke( "CanDoMeridianFlip" );
			Assert.IsFalse( result ); // Not a German Equatorial mount

			_vm.Parameters.AlignmentMode = AlignmentModes.algGermanPolar;
			_vm.Status.IsCounterWeightUp = false;

			result = (bool)_prVm.Invoke( "CanDoMeridianFlip" );
			Assert.IsFalse( result ); // Not CW Up

			_vm.Status.IsCounterWeightUp = true;
			_vm.Status.SideOfPier = PierSide.pierEast;

			result = (bool)_prVm.Invoke( "CanDoMeridianFlip" );
			Assert.IsFalse( result ); // Not on the west side of the pier

			_vm.Status.SideOfPier = PierSide.pierWest;

			result = (bool)_prVm.Invoke( "CanDoMeridianFlip" );
			Assert.IsTrue( result ); // Ready to flip
		}

		[TestMethod]
		public void DoMeridianFlip()
		{
			DevHubTelescopeStatus sts = DevHubTelescopeStatus.GetEmptyStatus();
			sts.SideOfPier = PierSide.pierWest;

			_vm.Status = sts;
			_mgr.Status = sts;
			_mgr.MockSideOfPier = PierSide.pierWest;
			_prVm.Invoke( "RegisterStatusUpdateMessage", true );
			_prVm.Invoke( "DoMeridianFlip" );

			Thread.Sleep( 1500 );

			while( _vm.Status.Slewing )
			{
				Thread.Sleep( 100 );
			}

			Assert.AreEqual( PierSide.pierEast, _vm.Status.SideOfPier );
		}

		[TestMethod]
		public void CanJogScope()
		{
			TelescopeParameters parms = new TelescopeParameters
			{
				AlignmentMode = AlignmentModes.algGermanPolar
			};
			DevHubTelescopeStatus status = DevHubTelescopeStatus.GetEmptyStatus();
			TelescopeCapabilities caps = TelescopeCapabilities.GetFullCapabilities();

			_vm.Status = null;
			_vm.Parameters = parms;
			_vm.Capabilities = caps;

			_prVm.Invoke( "UpdateCanStartMove" );
			Assert.IsFalse( _vm.CanStartMoveTelescope ); // No device status

			_vm.Status = status;
			_vm.Parameters = null;

			_prVm.Invoke( "UpdateCanStartMove" );
			Assert.IsFalse( _vm.CanStartMoveTelescope ); // No device parameters

			_vm.Parameters = parms;
			_vm.Capabilities = null;

			_prVm.Invoke( "UpdateCanStartMove" );
			Assert.IsFalse( _vm.CanStartMoveTelescope ); // No capabilities

			_vm.Capabilities = caps;

			// Test variable jogs with MoveAxis and PulseGuide

			_prVm.Invoke( "UpdateCanStartMove" );
			Assert.IsFalse( _vm.CanStartMoveTelescope ); // Not connected

			_vm.Status.Connected = true;

			// Test variable jogs with MoveAxis and PulseGuide

			_vm.IsVariableJog = true;
			_vm.Capabilities.CanMovePrimaryAxis = false;
			_vm.Capabilities.CanPulseGuide = false;

			_prVm.Invoke( "UpdateCanStartMove" );
			Assert.IsFalse( _vm.CanStartMoveTelescope ); // Can't MoveAxis or PulseGuide

			_vm.Capabilities.CanMovePrimaryAxis = true;
			_vm.Capabilities.CanMoveSecondaryAxis = false;

			_prVm.Invoke( "UpdateCanStartMove" );
			Assert.IsFalse( _vm.CanStartMoveTelescope ); // Can't MoveAxis or PulseGuide

			_vm.Capabilities.CanMoveSecondaryAxis = true;

			_prVm.Invoke( "UpdateCanStartMove" );
			Assert.IsTrue( _vm.CanStartMoveTelescope ); // Not Tracking

			_vm.Status.Tracking = true;

			_prVm.Invoke( "UpdateCanStartMove" ); // Ready to do variable slews (Move Axis or PulseGuide)
			Assert.IsTrue( _vm.CanStartMoveTelescope );

			// Now test fixed RA/Dec jogs

			_vm.IsVariableJog = false;

			_vm.Capabilities = TelescopeCapabilities.GetFullCapabilities();

			_prVm.Invoke( "UpdateCanStartMove" );
			Assert.IsTrue( _vm.CanStartMoveTelescope ); // Ready to do fixed RA/Dec slews (sync or async)

			_vm.Capabilities.CanSlewAsync = false;

			_prVm.Invoke( "UpdateCanStartMove" );
			Assert.IsTrue( _vm.CanStartMoveTelescope ); // Ready to do fixed sync RA/Dec slews

			_vm.Capabilities.CanSlew = false;
			_vm.Capabilities.CanSlewAltAz = false;
			_prVm.Invoke( "UpdateCanStartMove" );
			Assert.IsFalse ( _vm.CanStartMoveTelescope ); // Can't do fixed sync or async RA/Dec or Alt/Az slews

			// Finally test jogging via alt-az slews.

			_vm.Parameters.EquatorialSystem = EquatorialCoordinateType.equTopocentric;
			_vm.Status.Tracking = false;

			_vm.Capabilities.CanSlewAltAz = true;
			_prVm.Invoke( "UpdateCanStartMove" );
			Assert.IsTrue( _vm.CanStartMoveTelescope ); // Can slew either sync or async Alt-Az.

			_vm.Capabilities.CanSlewAltAzAsync = false;

			_prVm.Invoke( "UpdateCanStartMove" );
			Assert.IsTrue( _vm.CanStartMoveTelescope ); // Can slew sync Alt-Az.

			_vm.Capabilities.CanSlewAltAz = false;
			_prVm.Invoke( "UpdateCanStartMove" );
			Assert.IsFalse( _vm.CanStartMoveTelescope ); // Can't slew either sync or async Alt-Az.
		}

		[TestMethod]
		public void JogMoveAxis()
		{
			double moveRate = 1.0;
			_mgr.MockRightAscensionAxisRate = 0.0;

			_vm.IsVariableJog = true;
			_vm.SelectedJogRate = new JogRate { Name = "1 deg/sec", Rate = moveRate };

			_prVm.Invoke( "StartMove", MoveDirections.East );

			Assert.AreEqual( -moveRate, _mgr.MockRightAscensionAxisRate );

			_prVm.Invoke( "StopMotion", MoveDirections.East );

			Assert.AreEqual( 0.0, _mgr.MockRightAscensionAxisRate );

			_prVm.Invoke( "StartMove", MoveDirections.North );

			Assert.AreEqual( moveRate, _mgr.MockDeclinationAxisRate );

			_prVm.Invoke( "StopMotion", MoveDirections.North );

			Assert.AreEqual( 0.0, _mgr.MockDeclinationAxisRate );
		}

		[TestMethod]
		public void JogPulseGuide()
		{
			double moveRate = 1.0;
			_mgr.MockRightAscensionAxisRate = 0.0;

			_vm.IsVariableJog = true;
			_vm.SelectedJogRate = new JogRate { Name = "1 deg/sec", Rate = moveRate };

			_prVm.Invoke( "StartMove", MoveDirections.East );

			Assert.AreEqual( -moveRate, _mgr.MockRightAscensionAxisRate );

			_prVm.Invoke( "StopMotion", MoveDirections.East );

			Assert.AreEqual( 0.0, _mgr.MockRightAscensionAxisRate );

			_prVm.Invoke( "StartMove", MoveDirections.North );

			Assert.AreEqual( moveRate, _mgr.MockDeclinationAxisRate );

			_prVm.Invoke( "StopMotion", MoveDirections.North );

			Assert.AreEqual( 0.0, _mgr.MockDeclinationAxisRate );
		}

		[TestMethod]
		public void DoFixedSlewEastRaDec()
		{
			Vector startPosition = GetSaneRaDec();
			SlewDirection direction = SlewDirection.SlewEast;
			SlewAmount amount = new SlewAmount( "4°", 4.0 );

			Vector finalPosition = DoFixedSlewRaDec( startPosition, direction, amount );

			Assert.AreNotEqual( startPosition.X, finalPosition.X, 0.00001 );
			Assert.AreEqual( startPosition.Y, finalPosition.Y, 0.00001 );
		}

		[TestMethod]
		public void DoFixedSlewWestRaDec()
		{
			Vector startPosition = GetSaneRaDec();
			SlewDirection direction = SlewDirection.SlewWest;
			SlewAmount amount = new SlewAmount( "4°", 4.0 );

			Vector finalPosition = DoFixedSlewRaDec( startPosition, direction, amount );

			Assert.AreNotEqual( startPosition.X, finalPosition.X, 0.00001 );
			Assert.AreEqual( startPosition.Y, finalPosition.Y, 0.00001 );
		}

		[TestMethod]
		public void DoFixedSlewNorthRaDec()
		{
			Vector startPosition = GetSaneRaDec();
			SlewDirection direction = SlewDirection.SlewNorth;
			SlewAmount amount = new SlewAmount( "4°", 4.0 );

			Vector finalPosition = DoFixedSlewRaDec( startPosition, direction, amount );

			Assert.AreEqual( startPosition.X, finalPosition.X, 0.00001 );
			Assert.AreNotEqual( startPosition.Y, finalPosition.Y, 0.00001 );
		}

		[TestMethod]
		public void DoFixedSlewSouthRaDec()
		{
			Vector startPosition = GetSaneRaDec();
			SlewDirection direction = SlewDirection.SlewSouth;
			SlewAmount amount = new SlewAmount( "4°", 4.0 );

			Vector finalPosition = DoFixedSlewRaDec( startPosition, direction, amount );

			Assert.AreEqual( startPosition.X, finalPosition.X, 0.00001 );
			Assert.AreNotEqual( startPosition.Y, finalPosition.Y, 0.00001 );
		}

		[TestMethod]
		public void DoFixedSlewUpAltAz()
		{
			Vector startPosition = GetSaneAltAz();
			SlewDirection direction = SlewDirection.SlewUp;
			SlewAmount amount = new SlewAmount( "4°", 4.0 );

			Vector finalPosition = DoFixedSlewAltAz( startPosition, direction, amount );

			Assert.AreEqual( startPosition.X, finalPosition.X, 0.00001 );
			Assert.AreEqual( startPosition.Y + amount.Amount, finalPosition.Y, 0.00001 );
		}

		[TestMethod]
		public void DoFixedSlewDownAltAz()
		{
			Vector startPosition = GetSaneAltAz();
			SlewDirection direction = SlewDirection.SlewDown;
			SlewAmount amount = new SlewAmount( "4°", 4.0 );

			Vector finalPosition = DoFixedSlewAltAz( startPosition, direction, amount );

			Assert.AreEqual( startPosition.X, finalPosition.X, 0.00001 );
			Assert.AreEqual( startPosition.Y - amount.Amount, finalPosition.Y, 0.00001 );
		}

		[TestMethod]
		public void DoFixedSlewLeftAltAz()
		{
			Vector startPosition = GetSaneAltAz();
			SlewDirection direction = SlewDirection.SlewLeft;
			SlewAmount amount = new SlewAmount( "4°", 4.0 );

			Vector finalPosition = DoFixedSlewAltAz( startPosition, direction, amount );

			Assert.AreEqual( startPosition.X - amount.Amount, finalPosition.X, 0.00001 );
			Assert.AreEqual( startPosition.Y, finalPosition.Y, 0.00001 );
		}

		[TestMethod]
		public void DoFixedSlewRightAltAz()
		{
			Vector startPosition = GetSaneAltAz();
			SlewDirection direction = SlewDirection.SlewRight;
			SlewAmount amount = new SlewAmount( "4°", 4.0 );

			Vector finalPosition = DoFixedSlewAltAz( startPosition, direction, amount );

			Assert.AreEqual( startPosition.X + amount.Amount, finalPosition.X, 0.00001 );
			Assert.AreEqual( startPosition.Y, finalPosition.Y, 0.00001 );
		}

		#region Helper Methods

		private Vector DoFixedSlewRaDec( Vector startPosition, SlewDirection direction, SlewAmount amount )
		{
			_vm.IsVariableJog = false;
			_mgr.MockIsConnected = true;
			_mgr.Capabilities = InitializeFullCapabilities();
			_mgr.Parameters = InitializeTestDefaultParameters();

			_vm.SelectedSlewAmount = amount;

			DevHubTelescopeStatus sts = DevHubTelescopeStatus.GetEmptyStatus();
			sts.ParkingState = ParkingStateEnum.Unparked;
			sts.Tracking = true;
			sts.Slewing = false;
			sts.RightAscension = startPosition.X;
			sts.Declination = startPosition.Y;
			_mgr.Status = sts;

			_prVm.Invoke( "RegisterStatusUpdateMessage", true );
			_prVm.Invoke( "DoFixedSlew", direction );

			Thread.Sleep( 3500 );

			_prVm.Invoke( "RegisterStatusUpdateMessage", false );

			return new Vector( _vm.Status.RightAscension, _vm.Status.Declination );
		}

		private Vector DoFixedSlewAltAz( Vector startPosition, SlewDirection direction, SlewAmount amount )
		{
			_vm.IsVariableJog = false;
			_mgr.MockIsConnected = true;
			_mgr.Capabilities = InitializeFullCapabilities();
			_mgr.Parameters = InitializeTestDefaultParameters( AlignmentModes.algAltAz );

			_vm.SelectedSlewAmount = amount;

			DevHubTelescopeStatus sts = DevHubTelescopeStatus.GetEmptyStatus();
			sts.ParkingState = ParkingStateEnum.Unparked;
			sts.Tracking = true;
			sts.Slewing = false;
			sts.Azimuth = startPosition.X;
			sts.Altitude = startPosition.Y;
			_mgr.Status = sts;

			_prVm.Invoke( "RegisterStatusUpdateMessage", true );
			_prVm.Invoke( "DoFixedSlew", direction );

			Thread.Sleep( 4000 );

			_prVm.Invoke( "RegisterStatusUpdateMessage", false );

			return new Vector( _vm.Status.Azimuth, _vm.Status.Altitude );
		}

		private TelescopeCapabilities InitializeFullCapabilities()
		{
			TelescopeCapabilities capabilities = TelescopeCapabilities.GetFullCapabilities();

			AxisRates.ResetRates();
			capabilities.PrimaryAxisRates = AxisRatesToArray( new AxisRates( TelescopeAxes.axisPrimary ) );
			capabilities.SecondaryAxisRates = AxisRatesToArray( new AxisRates( TelescopeAxes.axisSecondary ) );
			capabilities.TertiaryAxisRates = AxisRatesToArray( new AxisRates( TelescopeAxes.axisTertiary ) );

			return capabilities;
		}

		private IRate[] AxisRatesToArray( IAxisRates axisRates )
		{
			List<IRate> tempList = new List<IRate>();

			foreach ( IRate rate in axisRates )
			{
				tempList.Add( rate );
			}

			return tempList.ToArray();
		}

		private Vector GetSaneRaDec()
		{
			return GetSaneRaDec( 0.0, 0.0 );
		}

		private Vector GetSaneRaDec( double deltaRA, double deltaDec )
		{
			// Return a sane slew target.

			double targetRightAscension = Double.NaN;
			double targetDeclination = Double.NaN;

			try
			{
				Transform xform = new Transform
				{
					SiteElevation = _siteElevation,
					SiteLatitude = _siteLatitude,
					SiteLongitude = _siteLongitude
				};

				xform.SetAzimuthElevation( _saneAzimuth, _saneAltitude );

				targetRightAscension = xform.RATopocentric;
				targetDeclination = xform.DECTopocentric;
			}
			catch ( Exception )
			{ }

			targetRightAscension += deltaRA;
			targetDeclination += deltaDec;

			return new Vector( targetRightAscension, targetDeclination );
		}
		
		private Vector GetSaneAltAz()
		{
			return new Vector( _saneAzimuth, _saneAltitude );
		}

		private TelescopeParameters InitializeTestDefaultParameters()
		{
			return InitializeTestDefaultParameters( AlignmentModes.algGermanPolar );
		}

		private TelescopeParameters InitializeTestDefaultParameters( AlignmentModes alignmentMode )
		{
			TelescopeParameters parms = new TelescopeParameters();
			parms.InterfaceVersion = 3;
			parms.AlignmentMode = alignmentMode;
			parms.ApertureDiameter = 0.130;
			parms.ApertureArea = parms.ApertureDiameter * Math.PI / 4;
			parms.DoesRefraction = false;
			parms.DriverVersion = "1.0";
			parms.EquatorialSystem = EquatorialCoordinateType.equTopocentric;
			parms.FocalLength = 0.910;
			parms.SiteElevation = _siteElevation;
			parms.SiteLatitude = _siteLatitude;
			parms.SiteLongitude = _siteLongitude;
			parms.SlewSettleTime = 0;

			return parms;
		}

		#endregion
	}
}