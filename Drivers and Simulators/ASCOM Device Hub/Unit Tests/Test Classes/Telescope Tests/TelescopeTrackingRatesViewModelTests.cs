using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ASCOM.DeviceHub;
using System.Threading.Tasks;
using ASCOM.DeviceInterface;
using System.Windows;
using System.Collections.Generic;
using ASCOM.Astrometry.Transform;

namespace Unit_Tests.Telescope
{
	[TestClass]
	public class TelescopeTrackingRatesViewModelTests
	{
		private MockTelescopeManager _mgr = null;

		private TelescopeTrackingRatesViewModel _vm = null;
		private PrivateObject _prVm = null;

		[TestInitialize]
		public void TestInit()
		{
			ServiceContainer.Instance.ClearAllServices();
			ServiceContainer.Instance.AddService<IMessageBoxService>( new MockMessageBoxService() );
			_mgr = new MockTelescopeManager();

			_vm = new TelescopeTrackingRatesViewModel( _mgr );
			_prVm = new PrivateObject( _vm );

			Globals.UISyncContext = TaskScheduler.Default;
		}


		[TestMethod]
		public void ApplySiderealTrackingTest()
		{
			_vm.IsConnected = true;
			_vm.Capabilities = TelescopeCapabilities.GetFullCapabilities();
			_vm.Status = DevHubTelescopeStatus.GetEmptyStatus();
			//_mgr.MockTrackingRate = DriveRates.driveLunar;

			_prVm.Invoke( "ApplyLunarTracking" );

			Assert.AreEqual( DriveRates.driveLunar, _mgr.MockTrackingRate );

			_prVm.Invoke( "ApplySiderealTracking" );

			Assert.AreEqual( DriveRates.driveSidereal, _mgr.MockTrackingRate );
			Assert.AreEqual( 0.0, _mgr.MockRightAscensionOffsetRate );
			Assert.AreEqual( 0.0, _mgr.MockDeclinationOffsetRate );

			_mgr.MockTrackingRate = DriveRates.driveSidereal;
			_mgr.MockRightAscensionOffsetRate = 200.0;
			_mgr.MockDeclinationOffsetRate = -200.0;

			_prVm.Invoke( "ApplySiderealTracking" );

			Assert.AreEqual( DriveRates.driveSidereal, _mgr.MockTrackingRate );
			Assert.AreEqual( 0.0, _mgr.MockRightAscensionOffsetRate );
			Assert.AreEqual( 0.0, _mgr.MockDeclinationOffsetRate );
		}

		[TestMethod]
		public void ApplyLunarTrackingTest()
		{
			_vm.IsConnected = true;
			_vm.Capabilities = TelescopeCapabilities.GetFullCapabilities();
			_vm.Status = DevHubTelescopeStatus.GetEmptyStatus();
			_vm.RaOffsetRate = 200.0;
			_vm.DecOffsetRate = -200.0;

			_mgr.MockTrackingRate = DriveRates.driveSidereal;

			_prVm.Invoke( "ApplySiderealTracking" );

			Assert.AreEqual( DriveRates.driveSidereal, _mgr.MockTrackingRate );
			Assert.AreEqual( 0.0, _mgr.MockRightAscensionOffsetRate );
			Assert.AreEqual( 0.0, _mgr.MockDeclinationOffsetRate );

			_prVm.Invoke( "ApplyLunarTracking" );

			Assert.AreEqual( DriveRates.driveLunar, _mgr.MockTrackingRate );
			Assert.AreEqual( 0.0, _mgr.MockRightAscensionOffsetRate );
			Assert.AreEqual( 0.0, _mgr.MockDeclinationOffsetRate );
		}

		[TestMethod]
		public void ApplyOffsetTrackingTest()
		{
			double targetRaOffsetRate = 200.0;
			double targetDecOffsetRate = -200.0;

			_vm.IsConnected = true;
			_vm.Capabilities = TelescopeCapabilities.GetFullCapabilities();
			_vm.Status = DevHubTelescopeStatus.GetEmptyStatus();
			_vm.RaOffsetRate = targetRaOffsetRate;
			_vm.DecOffsetRate = targetDecOffsetRate;

			_mgr.MockTrackingRate = DriveRates.driveSidereal;

			_prVm.Invoke( "ApplyOffsetTracking" );

			double expectedRaRate = targetRaOffsetRate * Globals.UTC_SECS_PER_SIDEREAL_SEC / ( 15.0 * 3600.0 );
			double expectedDecRate = targetDecOffsetRate / 3600.0;
			Assert.AreEqual( DriveRates.driveSidereal, _mgr.MockTrackingRate );
			Assert.AreEqual( expectedRaRate, _mgr.MockRightAscensionOffsetRate, 0.0001 );
			Assert.AreEqual( expectedDecRate, _mgr.MockDeclinationOffsetRate );

			_prVm.Invoke( "ApplySiderealTracking" );

			Assert.AreEqual( DriveRates.driveSidereal, _mgr.MockTrackingRate );
			Assert.AreEqual( 0.0, _mgr.MockRightAscensionOffsetRate );
			Assert.AreEqual( 0.0, _mgr.MockDeclinationOffsetRate );
		}
	}
}
