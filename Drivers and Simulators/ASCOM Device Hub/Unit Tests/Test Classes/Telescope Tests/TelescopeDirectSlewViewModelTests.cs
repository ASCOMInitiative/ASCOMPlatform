using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ASCOM.Astrometry.Transform;
using ASCOM.DeviceHub;
using ASCOM.DeviceInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_Tests.Telescope
{ 
	[TestClass]
	public class TelescopeDirectSlewViewModelTests
	{
		private const string ScopeID = "Mock Telescope";
		private const double _tolerance = 0.00001;
		private const double _saneAltitude = 50.0;
		private const double _saneAzimuth = 135.0;
		private const double _siteLatitude = 31.5;
		private const double _siteLongitude = -110.33;
		private const double _siteElevation = 1300;

		private MockTelescopeManager _mgr = null;
		private TelescopeDirectSlewViewModel _vm = null;
		private PrivateObject _prVm = null;

		[TestInitialize]
		public void TestInit()
		{
			ServiceContainer.Instance.ClearAllServices();
			ServiceContainer.Instance.AddService<IMessageBoxService>( new MockMessageBoxService() );
			_mgr = new MockTelescopeManager();

			_vm = new TelescopeDirectSlewViewModel( _mgr );
			_prVm = new PrivateObject( _vm );

			Globals.UISyncContext = TaskScheduler.Default;
		}

		[TestMethod]
		public void BeginDirectSlewTest()
		{
			Vector startPosition = GetSaneRaDec();
			Vector targetPosition = GetSaneRaDec( -4.0, 5.0 );

			_mgr.Capabilities = InitializeFullCapabilities();
			_vm.Capabilities = _mgr.Capabilities;
			_vm.UseDecimalHours = true;
			_vm.Status = new DevHubTelescopeStatus();
			_vm.Status.Tracking = true;


			Vector finalPosition = DoDirectSlewRaDec( startPosition, targetPosition );

			Assert.IsFalse( _vm.Status.Slewing );
			Assert.AreEqual( targetPosition.X, finalPosition.X, 0.00001 );
			Assert.AreEqual( targetPosition.Y, finalPosition.Y, 0.00001 );
		}

		[TestMethod]
		public void AbortDirectSlewTest()
		{
			Vector startPosition = GetSaneRaDec();
			Vector targetPosition = GetSaneRaDec( -4.0, 5.0 );

			_mgr.MockIsConnected = true;
			_mgr.Capabilities = InitializeFullCapabilities();
			_mgr.Parameters = InitializeTestDefaultParameters();

			DevHubTelescopeStatus sts = DevHubTelescopeStatus.GetEmptyStatus();
			sts.ParkingState = ParkingStateEnum.Unparked;
			sts.Tracking = true;
			sts.Slewing = true;
			_mgr.Status = sts;

			_prVm.Invoke( "RegisterStatusUpdateMessage", true );
			_prVm.Invoke( "AbortDirectSlew" );

			Thread.Sleep( 2000 );

			_prVm.Invoke( "RegisterStatusUpdateMessage", false );

			Assert.IsFalse( _vm.Status.Slewing );
		}

		#region Helper Methods

		private Vector DoDirectSlewRaDec( Vector startPosition, Vector targetPosition )
		{
			_mgr.MockIsConnected = true;
			_mgr.Parameters = InitializeTestDefaultParameters();

			DevHubTelescopeStatus sts = DevHubTelescopeStatus.GetEmptyStatus();
			sts.ParkingState = ParkingStateEnum.Unparked;
			sts.Tracking = true;
			sts.Slewing = false;
			sts.RightAscension = startPosition.X;
			sts.Declination = startPosition.Y;
			_mgr.Status = sts;

			_vm.TargetRightAscension = targetPosition.X;
			_vm.TargetDeclination = targetPosition.Y;

			_prVm.Invoke( "RegisterStatusUpdateMessage", true );
			_prVm.Invoke( "BeginDirectSlew" );

			Thread.Sleep( 3500 );

			_prVm.Invoke( "RegisterStatusUpdateMessage", false );

			return new Vector( _vm.Status.RightAscension, _vm.Status.Declination );
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
