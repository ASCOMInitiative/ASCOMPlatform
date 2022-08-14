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

			_prVm.Invoke( "ApplyLunarTracking" );

			Assert.AreEqual( DriveRates.driveLunar, _mgr.MockTrackingRate );

			_prVm.Invoke( "ApplySiderealTracking" );

			Assert.AreEqual( DriveRates.driveSidereal, _mgr.MockTrackingRate );
		}

		[TestMethod]
		public void ApplyLunarTrackingTest()
		{
			_vm.IsConnected = true;
			_vm.Capabilities = TelescopeCapabilities.GetFullCapabilities();
			_vm.Status = DevHubTelescopeStatus.GetEmptyStatus();

			_prVm.Invoke( "ApplySiderealTracking" );

			Assert.AreEqual( DriveRates.driveSidereal, _mgr.MockTrackingRate );

			_prVm.Invoke( "ApplyLunarTracking" );

			Assert.AreEqual( DriveRates.driveLunar, _mgr.MockTrackingRate );
		}

		[TestMethod]
		public void ApplySolarTrackingTest()
		{
			_vm.IsConnected = true;
			_vm.Capabilities = TelescopeCapabilities.GetFullCapabilities();
			_vm.Status = DevHubTelescopeStatus.GetEmptyStatus();

			_prVm.Invoke( "ApplySiderealTracking" );

			Assert.AreEqual( DriveRates.driveSidereal, _mgr.MockTrackingRate );

			_prVm.Invoke( "ApplySolarTracking" );

			Assert.AreEqual( DriveRates.driveSolar, _mgr.MockTrackingRate );
		}

		[TestMethod]
		public void ApplyKingTrackingTest()
		{
			_vm.IsConnected = true;
			_vm.Capabilities = TelescopeCapabilities.GetFullCapabilities();
			_vm.Status = DevHubTelescopeStatus.GetEmptyStatus();

			_prVm.Invoke( "ApplySiderealTracking" );

			Assert.AreEqual( DriveRates.driveSidereal, _mgr.MockTrackingRate );

			_prVm.Invoke( "ApplyKingTracking" );

			Assert.AreEqual( DriveRates.driveKing, _mgr.MockTrackingRate );
		}

		[TestMethod]
		public void ApplyTrackingOffsetsTest()
		{
			double targetRaOffsetRate = -0.00098;
			double targetDecOffsetRate = -0.01748;

			_vm.IsConnected = true;
			_vm.Capabilities = TelescopeCapabilities.GetFullCapabilities();
			_vm.Status = DevHubTelescopeStatus.GetEmptyStatus();
			_vm.UseNasaJplUnits = false;
			_vm.NewRaOffsetRate = targetRaOffsetRate;
			_vm.NewDecOffsetRate = targetDecOffsetRate;

			_prVm.Invoke( "CommitNewRates" );

			Assert.AreEqual( targetRaOffsetRate, _mgr.MockRightAscensionOffsetRate, 0.0001 );
			Assert.AreEqual( targetDecOffsetRate, _mgr.MockDeclinationOffsetRate, 0.0001 );

			targetRaOffsetRate = -53.1724;
			targetDecOffsetRate = -62.9412;
			_vm.UseNasaJplUnits = true;
			_prVm.Invoke( "CommitNewRates" );

			double expectedRaRate = targetRaOffsetRate * Globals.UTC_SECS_PER_SIDEREAL_SEC / ( 15.0 * 3600.0 );
			double expectedDecRate = targetDecOffsetRate / 3600.0;
			Assert.AreEqual( expectedRaRate, _mgr.MockRightAscensionOffsetRate, 0.00001 );
			Assert.AreEqual( expectedDecRate, _mgr.MockDeclinationOffsetRate, 0.00001 );
		}
	}
}
