using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ASCOM.DeviceInterface;

using ASCOM.DeviceHub;
using ASCOM.DeviceHub.MvvmMessenger;

namespace Unit_Tests.Dome
{
	[TestClass]
	public class DomeManagerTests
	{
		private const string DomeID = "Mock Dome";
		private const double _tolerance = 0.00001;

		private DomeManager _mgr;
		private MockDomeService _svc;
		private int _startupDelayMs = 3000;

		[ClassInitialize]
		public static void ClassInit( TestContext context )
		{
		}

		[TestInitialize]
		public void TestInit()
		{
			ServiceContainer.Instance.ClearAllServices();
			ServiceContainer.Instance.AddService<IDomeService>( new MockDomeService() );
			_mgr = DomeManager.Instance;
			_svc = (MockDomeService)ServiceContainer.Instance.GetService<IDomeService>();

			Messenger.Default.Register<DomeSlavedChangedMessage>( this, ( action ) => UpdateDomeSlavedState( action ) );

			bool retval = _mgr.Connect( DomeID );
			Assert.AreEqual( true, retval );

			Thread.Sleep( _startupDelayMs );
		}

		[TestCleanup]
		public void TestCleanup()
		{
			Messenger.Default.Unregister<DomeSlavedChangedMessage>( this );

			_mgr.Disconnect();
			Assert.IsFalse( _svc.Connected );

			_mgr.Dispose();
		}


		[TestMethod]
		public void Connect()
		{
			Assert.AreEqual( true, _mgr.Connected );
			Assert.AreEqual( true, _mgr.Capabilities.CanFindHome );
			Assert.AreEqual( true, _mgr.Capabilities.CanPark );
			Assert.AreEqual( true, _mgr.Capabilities.CanSetAltitude );
			Assert.AreEqual( true, _mgr.Capabilities.CanSetAzimuth );
			Assert.AreEqual( true, _mgr.Capabilities.CanSetPark );
			Assert.AreEqual( true, _mgr.Capabilities.CanSetShutter );
			Assert.AreEqual( true, _mgr.Capabilities.CanSlave );
			Assert.AreEqual( true, _mgr.Capabilities.CanSyncAzimuth );

			Assert.AreEqual( _svc.Description, _mgr.Parameters.Description );
			Assert.AreEqual( _svc.DriverInfo, _mgr.Parameters.DriverInfo );
			Assert.AreEqual( _svc.DriverVersion, _mgr.Parameters.DriverVersion );
			Assert.AreEqual( _svc.InterfaceVersion, _mgr.Parameters.InterfaceVersion );
			Assert.AreEqual( _svc.SupportedActions.Count, _mgr.Parameters.SupportedActions.Length );
		}

		[TestMethod]
		public void Disconnect()
		{
			// Disconnection is tested in the TestCleanup method.
		}

		[TestMethod]
		public void OpenShutter()
		{
			_svc.MockShutterStatus = ShutterState.shutterClosed;

			// The DomeManager polls every 5 seconds so we need to wait for the shutter status to change.

			while ( _mgr.Status.ShutterStatus != ShutterState.shutterClosed )
			{
				Thread.Sleep( 500 );
			}

			_mgr.OpenDomeShutter();

			Thread.Sleep( 2000 );

			Assert.IsTrue( _svc.ShutterStatus == ShutterState.shutterOpen );
		}

		[TestMethod]
		public void CloseShutter()
		{
			_svc.MockShutterStatus = ShutterState.shutterOpen;

			// The DomeManager polls every 5 seconds so we need to wait for the shutter status to change.

			while ( _mgr.Status.ShutterStatus != ShutterState.shutterOpen )
			{
				Thread.Sleep( 500 );
			}

			_mgr.CloseDomeShutter();

			Thread.Sleep( 2000 );

			Assert.IsTrue( _svc.ShutterStatus == ShutterState.shutterClosed );
		}

		[TestMethod]
		public void ParkDome()
		{
			_svc.MockAtPark = false;
			_svc.MockAzimuth = 90.0;

			_mgr.ParkTheDome();

			Thread.Sleep( 100 );

			Assert.IsTrue( _mgr.Status.Slewing, "The dome has not started slewing to Park!" );

			while ( _mgr.Status.Slewing )
			{
				Thread.Sleep( 100 );
			}

			Assert.IsFalse( _mgr.Status.Slewing );
			Assert.IsTrue( _mgr.Status.AtPark );
			Assert.AreEqual( _svc.ParkAzimuth, _mgr.Azimuth );
		}

		[TestMethod]
		public void FindHome()
		{
			_svc.MockAtHome = false;
			_svc.MockAzimuth = 270.0;

			_mgr.FindHomePosition();

			Thread.Sleep( 100 );

			Assert.IsTrue( _mgr.Status.Slewing, "The dome has not started slewing to Home!" );

			while ( _mgr.Status.Slewing )
			{
				Thread.Sleep( 100 );
			}

			Assert.IsFalse( _mgr.Status.Slewing );
			Assert.IsTrue( _mgr.Status.AtHome );
			Assert.AreEqual( _svc.HomeAzimuth, _mgr.Azimuth );
		}

		[TestMethod]
		public void SlewShutter()
		{
			// Start with the shutter open at altitude 0 deg.
			_svc.MockShutterStatus = ShutterState.shutterOpen;
			_svc.MockAltitude = 0.0;

			Random rnd = new Random();
			double altitude = rnd.NextDouble() * 89.0 + 1.0;

			_mgr.SlewDomeShutter( altitude );

			Thread.Sleep( 100 );

			Assert.IsTrue( _mgr.Status.Slewing, "The dome has not started slewing the shutter!" );

			while ( _mgr.Status.Slewing )
			{
				Thread.Sleep( 1000 );
			}

			Assert.IsFalse( _mgr.Status.Slewing );
			Assert.AreEqual( altitude, _mgr.Status.Altitude, _tolerance );
		}

		[TestMethod]
		public void SlewDomeAzimuth()
		{
			// Start at Park.
			_svc.MockAtPark = true;
			_svc.MockAzimuth = _svc.ParkAzimuth;

			Random rnd = new Random();
			double azimuth = Double.NaN;

			do
			{
				azimuth = rnd.NextDouble() * 360.0;
				azimuth = ( azimuth == _svc.ParkAzimuth ) ? Double.NaN : azimuth;
			}
			while ( Double.IsNaN( azimuth ) );

			_mgr.SlewDomeToAzimuth( azimuth );

			Thread.Sleep( 100 );

			Assert.IsTrue( _mgr.Status.Slewing, "The dome has not started slewing to Home!" );

			while ( _mgr.Status.Slewing )
			{
				Thread.Sleep( 100 );
			}

			Assert.IsFalse( _mgr.Status.Slewing );
			Assert.AreEqual( azimuth, _mgr.Azimuth, _tolerance );
		}

		[TestMethod]
		public void SyncDomeToAzimuth()
		{
			double targetAzimuth = 265.0;
			_svc.MockAzimuth = 270.0;

			_mgr.SyncDomeToAzimuth( targetAzimuth );

			Thread.Sleep( 6000 );

			Assert.AreEqual( targetAzimuth, _mgr.Status.Azimuth );

		}

		[TestMethod]
		public void SetSlavedState()
		{
			Globals.IsDomeSlaved = false;

			_mgr.SetSlavedState( true );

			Assert.IsTrue(Globals.IsDomeSlaved );

			_mgr.SetSlavedState( false );

			Assert.IsFalse( Globals.IsDomeSlaved );
		}

		private void UpdateDomeSlavedState( DomeSlavedChangedMessage action )
		{
			Globals.IsDomeSlaved = action.State;
		}
	}
}
