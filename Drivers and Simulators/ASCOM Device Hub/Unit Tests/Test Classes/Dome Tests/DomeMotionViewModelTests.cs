using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ASCOM.DeviceInterface;

using ASCOM.DeviceHub;
using System;

namespace Unit_Tests.Dome
{
	[TestClass]
	public class DomeMotionViewModelTests
	{
		private MockDomeManager _mgr = null;
		private DomeMotionViewModel _vm = null;
		private PrivateObject _prVm = null;

		[TestInitialize]
		public void TestInit()
		{
			ServiceContainer.Instance.ClearAllServices();
			ServiceContainer.Instance.AddService<IMessageBoxService>( new MockMessageBoxService() );
			_mgr = new MockDomeManager();

			_vm = new DomeMotionViewModel( _mgr );
			_prVm = new PrivateObject( _vm );

			Globals.UISyncContext = TaskScheduler.Default;
		}

		[TestCleanup]
		public void TestCleanup()
		{
			_prVm = null;
			_vm.Dispose();
			_vm = null;
			_mgr = null;
		}

		[TestMethod]
		public void CanToggleShutterState()
		{
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();
			status.Connected = true;
			status.ShutterStatus = ShutterState.shutterClosed;
			_vm.Status = status;

			bool result;

			_vm.Status.Slewing = true;

			result = (bool)_prVm.Invoke( "CanToggleShutterState" );
			Assert.IsFalse( result );

			_vm.Status.Slewing = false;

			result = (bool)_prVm.Invoke( "CanToggleShutterState" );
			Assert.IsTrue( result );

			_vm.IsSlaved = true;

			result = (bool)_prVm.Invoke( "CanToggleShutterState" );
			Assert.IsFalse( result );

			_vm.IsSlaved = false;

			result = (bool)_prVm.Invoke( "CanToggleShutterState" );
			Assert.IsTrue( result );
		}

		[TestMethod]
		public void ToggleShutterState()
		{
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();
			status.Connected = true;
			status.ShutterStatus = ShutterState.shutterClosed;
			_vm.Status = status;

			_prVm.Invoke( "RegisterStatusUpdateMessage", true );

			_prVm.Invoke( "ToggleShutterState" );

			Thread.Sleep( 2000 );

			Assert.AreEqual( ShutterState.shutterOpen, _vm.Status.ShutterStatus );

			_prVm.Invoke( "ToggleShutterState" );

			Thread.Sleep( 2000 );

			Assert.AreEqual( ShutterState.shutterClosed, _vm.Status.ShutterStatus );

			_prVm.Invoke( "RegisterStatusUpdateMessage", false );
		}

		[TestMethod]
		public void CanParkDome()
		{
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();
			status.Connected = true;
			status.AtPark = false;
			_vm.Status = status;

			DomeCapabilities caps = DomeCapabilities.GetFullCapabilities();
			_vm.Capabilities = caps;

			bool result;

			_vm.Status.Slewing = true;

			result = (bool)_prVm.Invoke( "CanParkDome" );
			Assert.IsFalse( result ); // because the dome is slewing

			_vm.Status.Slewing = false;

			result = (bool)_prVm.Invoke( "CanParkDome" );
			Assert.IsTrue( result );

			_vm.IsSlaved = true;

			result = (bool)_prVm.Invoke( "CanParkDome" );
			Assert.IsFalse( result ); // because the dome is slaved to the scope

			_vm.IsSlaved = false;

			result = (bool)_prVm.Invoke( "CanParkDome" );
			Assert.IsTrue( result );

			_vm.Capabilities.CanPark = false;

			result = (bool)_prVm.Invoke( "CanParkDome" );
			Assert.IsFalse( result ); // because CanPark is false

			_vm.Capabilities.CanPark = true;
			_vm.Status.AtPark = true;

			result = (bool)_prVm.Invoke( "CanParkDome" );
			Assert.IsFalse( result ); // because the dome is already parked
		}

		[TestMethod]
		public void ParkDome()
		{
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();
			status.Connected = true;
			_vm.Status = status;

			DomeCapabilities caps = DomeCapabilities.GetFullCapabilities();
			_mgr.Capabilities = caps;

			_prVm.Invoke( "RegisterStatusUpdateMessage", true );

			_vm.Status.AtPark = false;
			_mgr.MockAzimuth = 90.0;
			_prVm.Invoke( "ParkDome" );

			Thread.Sleep( 2000 );

			Assert.IsTrue( _vm.Status.AtPark );
			Assert.AreEqual( _mgr.ParkAzimuth, _vm.Status.Azimuth );

			_prVm.Invoke( "RegisterStatusUpdateMessage", false );
		}

		[TestMethod]
		public void CanJogAltitude()
		{
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();
			_vm.Status = status;

			DomeCapabilities caps = DomeCapabilities.GetFullCapabilities();
			_vm.Capabilities = caps;

			bool result = (bool)_prVm.Invoke( "CanJogAltitude" );
			Assert.IsFalse( result ); // Not connected

			_vm.Status.Connected = true;

			result = (bool)_prVm.Invoke( "CanJogAltitude" );
			Assert.IsFalse( result ); // Shutter not open

			_vm.Status.ShutterStatus = ShutterState.shutterOpen;

			result = (bool)_prVm.Invoke( "CanJogAltitude" );
			Assert.IsTrue( result );

			_vm.Status.Slewing = true;

			result = (bool)_prVm.Invoke( "CanJogAltitude" );
			Assert.IsFalse( result ); // Dome is slewing

			_vm.Status.Slewing = false;
			_vm.Capabilities.CanSetAltitude = false;

			result = (bool)_prVm.Invoke( "CanJogAltitude" );
			Assert.IsFalse( result ); // Not capable
		}

		[TestMethod]
		public void JogAltitude()
		{
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();
			status.Connected = true;
			status.ShutterStatus = ShutterState.shutterOpen;
			status.Slewing = false;
			_vm.Status = status;

			DomeCapabilities caps = DomeCapabilities.GetFullCapabilities();
			_mgr.Capabilities = caps;

			_prVm.Invoke( "RegisterStatusUpdateMessage", true );

			double slewValue = 4.0;
			double startingAltitude = 45.0;

			_vm.SelectedSlewAmount = new SlewAmount( "4 degrees", slewValue );
			_vm.Status.Altitude = startingAltitude;
			_mgr.MockAltitude = startingAltitude;

			_prVm.Invoke( "JogAltitude", MoveDirections.Up );

			Thread.Sleep( 2000 );

			Assert.AreEqual( startingAltitude + slewValue, _vm.Status.Altitude );

			_vm.Status.Altitude = startingAltitude;
			_mgr.MockAltitude = startingAltitude;

			_prVm.Invoke( "JogAltitude", MoveDirections.Down );

			Thread.Sleep( 2000 );

			Assert.AreEqual( startingAltitude - slewValue, _vm.Status.Altitude );

			// Make sure we handle min and max altitude correctly.

			startingAltitude = 88.0;
			_vm.Status.Altitude = startingAltitude;
			_mgr.MockAltitude = startingAltitude;

			_prVm.Invoke( "JogAltitude", MoveDirections.Up );

			Thread.Sleep( 2000 );

			Assert.AreEqual( 90.0, _vm.Status.Altitude );

			startingAltitude = 2.0;
			_vm.Status.Altitude = startingAltitude;
			_mgr.MockAltitude = startingAltitude;

			_prVm.Invoke( "JogAltitude", MoveDirections.Down );

			Thread.Sleep( 2000 );

			Assert.AreEqual( 0.0, _vm.Status.Altitude );

			_prVm.Invoke( "RegisterStatusUpdateMessage", false );
		}

		[TestMethod]
		public void CanJogAzimuth()
		{
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();
			_vm.Status = status;

			DomeCapabilities caps = DomeCapabilities.GetFullCapabilities();
			_vm.Capabilities = caps;

			bool result = (bool)_prVm.Invoke( "CanJogAzimuth" );
			Assert.IsFalse( result ); // Not connected

			_vm.Status.Connected = true;

			result = (bool)_prVm.Invoke( "CanJogAzimuth" );
			Assert.IsTrue( result );

			_vm.Status.Slewing = true;

			result = (bool)_prVm.Invoke( "CanJogAzimuth" );
			Assert.IsFalse( result ); // Dome is slewing

			_vm.Status.Slewing = false;
			_vm.Capabilities.CanSetAzimuth = false;

			result = (bool)_prVm.Invoke( "CanJogAzimuth" );
			Assert.IsFalse( result ); // Not capable
		}

		[TestMethod]
		public void JogAzimuth()
		{
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();
			status.Connected = true;
			status.Slewing = false;
			_vm.Status = status;

			DomeCapabilities caps = DomeCapabilities.GetFullCapabilities();
			_mgr.Capabilities = caps;

			_prVm.Invoke( "RegisterStatusUpdateMessage", true );

			double slewValue = 10.0;
			double startingAzimuth = 225.0;

			_vm.SelectedSlewAmount = new SlewAmount( "10 degrees", slewValue );
			_vm.Status.Azimuth = startingAzimuth;
			_mgr.MockAzimuth = startingAzimuth;

			_prVm.Invoke( "JogAzimuth", MoveDirections.Clockwise );

			Thread.Sleep( 2000 );

			Assert.AreEqual( startingAzimuth + slewValue, _vm.Status.Azimuth );

			_vm.Status.Azimuth = startingAzimuth;
			_mgr.MockAzimuth = startingAzimuth;

			_prVm.Invoke( "JogAzimuth", MoveDirections.CounterClockwise );

			Thread.Sleep( 2000 );

			Assert.AreEqual( startingAzimuth - slewValue, _vm.Status.Azimuth );

			// Make sure that wraparound works, in both directions.

			startingAzimuth = 354.0;
			_vm.Status.Azimuth = startingAzimuth;
			_mgr.MockAzimuth = startingAzimuth;

			_prVm.Invoke( "JogAzimuth", MoveDirections.Clockwise );

			Thread.Sleep( 2000 );

			Assert.AreEqual( startingAzimuth + slewValue - 360.0, _vm.Status.Azimuth );

			startingAzimuth = 3.0;
			_vm.Status.Azimuth = startingAzimuth;
			_mgr.MockAzimuth = startingAzimuth;

			_prVm.Invoke( "JogAzimuth", MoveDirections.CounterClockwise );

			Thread.Sleep( 2000 );

			Assert.AreEqual( startingAzimuth - slewValue + 360.0, _vm.Status.Azimuth );

			_prVm.Invoke( "RegisterStatusUpdateMessage", false );
		}

		[TestMethod]
		public void CanGotoDirectAzimuth()
		{
			DomeCapabilities caps = DomeCapabilities.GetFullCapabilities();
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();

			_vm.Status = null;
			_vm.Capabilities = caps;

			bool result = (bool)_prVm.Invoke( "CanGotoDirectAzimuth" );
			Assert.IsFalse( result ); // No device status

			_vm.Status = status;
			_vm.Capabilities = null;

			result = (bool)_prVm.Invoke( "CanGotoDirectAzimuth" );
			Assert.IsFalse( result ); // No device capabilities

			_vm.Capabilities = caps;

			result = (bool)_prVm.Invoke( "CanGotoDirectAzimuth" );
			Assert.IsFalse( result ); // Not connected

			_vm.Status.Connected = true;
			_vm.Status.Slewing = true;

			result = (bool)_prVm.Invoke( "CanGotoDirectAzimuth" );
			Assert.IsFalse( result ); // Dome is slewing

			_vm.Status.Slewing = false;
			_vm.Capabilities.CanSetAzimuth = false;

			result = (bool)_prVm.Invoke( "CanGotoDirectAzimuth" );
			Assert.IsFalse( result ); // Not capable

			_vm.Capabilities.CanSetAzimuth = true;
			_vm.DirectTargetAzimuth = Double.NaN;

			result = (bool)_prVm.Invoke( "CanGotoDirectAzimuth" );
			Assert.IsFalse( result ); // No target azimuth

			_vm.DirectTargetAzimuth = 90.0;

			result = (bool)_prVm.Invoke( "CanGotoDirectAzimuth" );
			Assert.IsTrue( result );
		}

		[TestMethod]
		public void GotoDirectAzimuth()
		{
			double startingAzimuth = 135.0;
			double targetAzimuth = 225.0;

			DomeCapabilities caps = DomeCapabilities.GetFullCapabilities();
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();

			status.Connected = true;
			status.ShutterStatus = ShutterState.shutterOpen;
			status.Slewing = false;
			status.Azimuth = startingAzimuth;

			_vm.Status = status;
			_vm.Capabilities = caps;

			_mgr.Status = status;
			_mgr.Capabilities = caps;
			_mgr.MockAzimuth = startingAzimuth;

			_prVm.Invoke( "RegisterStatusUpdateMessage", true );

			//_mgr.SlewDomeToAzimuth( targetAzimuth );
			_vm.DirectTargetAzimuth = targetAzimuth;
			_prVm.Invoke( "GotoDirectAzimuth" );

			Thread.Sleep( 2000 );

			Assert.AreEqual( targetAzimuth, _vm.Status.Azimuth );

			_prVm.Invoke( "RegisterStatusUpdateMessage", false );
		}

		[TestMethod]
		public void CanSyncAzimuth()
		{
			DomeCapabilities caps = DomeCapabilities.GetFullCapabilities();
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();

			_vm.Status = null;
			_vm.Capabilities = caps;

			bool result = (bool)_prVm.Invoke( "CanSyncAzimuth" );
			Assert.IsFalse( result ); // No device status

			_vm.Status = status;
			_vm.Capabilities = null;

			result = (bool)_prVm.Invoke( "CanSyncAzimuth" );
			Assert.IsFalse( result ); // No device capabilities

			_vm.Capabilities = caps;

			result = (bool)_prVm.Invoke( "CanSyncAzimuth" );
			Assert.IsFalse( result ); // Not connected

			_vm.Status.Connected = true;
			_vm.Status.Slewing = true;

			result = (bool)_prVm.Invoke( "CanSyncAzimuth" );
			Assert.IsFalse( result ); // Dome is slewing

			_vm.Status.Slewing = false;
			_vm.Capabilities.CanSyncAzimuth = false;

			result = (bool)_prVm.Invoke( "CanSyncAzimuth" );
			Assert.IsFalse( result ); // Not capable

			_vm.Capabilities.CanSyncAzimuth = true;
			_vm.SyncTargetAzimuth = Double.NaN;

			result = (bool)_prVm.Invoke( "CanSyncAzimuth" );
			Assert.IsFalse( result ); // No target azimuth

			_vm.SyncTargetAzimuth = 90.0;

			result = (bool)_prVm.Invoke( "CanSyncAzimuth" );
			Assert.IsTrue( result );
		}

		[TestMethod]
		public void SyncAzimuth()
		{
			double startingAzimuth = 135.0;
			double targetAzimuth = 137.0;

			DomeCapabilities caps = DomeCapabilities.GetFullCapabilities();
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();

			status.Connected = true;
			status.ShutterStatus = ShutterState.shutterOpen;
			status.Slewing = false;
			status.Azimuth = startingAzimuth;

			_vm.Status = status;
			_vm.Capabilities = caps;

			_mgr.Status = status;
			_mgr.Capabilities = caps;
			_mgr.MockAzimuth = startingAzimuth;

			_prVm.Invoke( "RegisterStatusUpdateMessage", true );

			_vm.SyncTargetAzimuth = targetAzimuth;
			_prVm.Invoke( "SyncAzimuth" );

			Thread.Sleep( 2000 );

			Assert.AreEqual( targetAzimuth, _vm.Status.Azimuth );

			_prVm.Invoke( "RegisterStatusUpdateMessage", false );
		}

		[TestMethod]
		public void CanFindHome()
		{
			DomeCapabilities caps = DomeCapabilities.GetFullCapabilities();
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();
			_vm.IsSlaved = false;

			_vm.Status = null;
			_vm.Capabilities = caps;

			bool result = (bool)_prVm.Invoke( "CanFindHome" );
			Assert.IsFalse( result ); // No device status

			_vm.Status = status;
			_vm.Capabilities = null;

			result = (bool)_prVm.Invoke( "CanFindHome" );
			Assert.IsFalse( result ); // No device capabilities

			_vm.Capabilities = caps;

			result = (bool)_prVm.Invoke( "CanFindHome" );
			Assert.IsFalse( result ); // Not connected

			_vm.Status.Connected = true;
			_vm.IsSlaved = true;

			result = (bool)_prVm.Invoke( "CanFindHome" );
			Assert.IsFalse( result ); // Dome is slaved to scope

			_vm.IsSlaved = false;
			_vm.Status.Slewing = true;

			result = (bool)_prVm.Invoke( "CanFindHome" );
			Assert.IsFalse( result ); // Dome is slewing

			_vm.Status.Slewing = false;
			_vm.Capabilities.CanFindHome = false;

			result = (bool)_prVm.Invoke( "CanFindHome" );
			Assert.IsFalse( result ); // Not capable

			_vm.Capabilities.CanFindHome = true;

			result = (bool)_prVm.Invoke( "CanFindHome" );
			Assert.IsTrue( result );
		}

		[TestMethod]
		public void FindHome()
		{
			double startingAzimuth = 135.0;

			DomeCapabilities caps = DomeCapabilities.GetFullCapabilities();
			DevHubDomeStatus status = DevHubDomeStatus.GetEmptyStatus();

			status.Connected = true;
			status.ShutterStatus = ShutterState.shutterOpen;
			status.Slewing = false;
			status.Azimuth = startingAzimuth;

			_vm.Status = status;
			_vm.Capabilities = caps;

			_mgr.Status = status;
			_mgr.Capabilities = caps;
			_mgr.MockAzimuth = startingAzimuth;

			_prVm.Invoke( "RegisterStatusUpdateMessage", true );

			_prVm.Invoke( "FindHome" );

			Thread.Sleep( 2000 );

			Assert.AreEqual( _mgr.HomeAzimuth, _vm.Status.Azimuth );
			Assert.IsTrue( _vm.Status.AtHome );

			_prVm.Invoke( "RegisterStatusUpdateMessage", false );
		}
	}
}
