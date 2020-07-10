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

		#region Test Methods

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

			Assert.IsTrue( _mgr.Status.Slewing );
			Assert.IsTrue( _mgr.Status.ShutterStatus == ShutterState.shutterOpening );

			Thread.Sleep( 3000 );

			Assert.IsTrue( _mgr.Status.ShutterStatus == ShutterState.shutterOpen );
			Assert.IsFalse( _svc.Slewing );
		}

		[TestMethod]
		public void CloseShutter()
		{
			_svc.MockShutterStatus = ShutterState.shutterOpen;

			// The DomeManager polls every 5 seconds so we need to wait for the shutter status to change.

			while ( _mgr.Status.ShutterStatus != ShutterState.shutterOpen )
			{
				Thread.Sleep( 2000 );
			}

			_mgr.CloseDomeShutter();

			Assert.IsTrue( _mgr.Status.Slewing );
			Assert.IsTrue( _mgr.Status.ShutterStatus == ShutterState.shutterClosing );

			Thread.Sleep( 3000 );

			Assert.IsFalse( _mgr.Status.Slewing );
			Assert.IsTrue( _mgr.Status.ShutterStatus == ShutterState.shutterClosed );
		}

		[TestMethod]
		public void ParkDome()
		{
			_svc.MockAtPark = false;
			_svc.MockAzimuth = 90.0;

			_mgr.ParkTheDome();

			Assert.IsTrue( _mgr.Status.Slewing, "The dome has not started slewing to Park." );

			DateTime timeoutTime = DateTime.Now.AddMinutes( 1.0 );
			bool timedout = false;
			DevHubDomeStatus sts = _mgr.Status;

			while ( !timedout )
			{
				if ( !_mgr.Status.Slewing && _mgr.Status.AtPark )
				{
					break;
				}
				else
				{
					if ( DateTime.Now > timeoutTime )
					{
						timedout = true;
					}
					else
					{
						Thread.Sleep( 500 );

						sts = _mgr.Status;
					}
				}
			}

			Assert.IsFalse( timedout );
			Assert.IsFalse( _mgr.Status.Slewing );
			Assert.IsTrue( sts.AtPark );
			Assert.AreEqual( _svc.ParkAzimuth, _mgr.Status.Azimuth );
		}

		[TestMethod]
		public void FindHome()
		{
			_svc.MockAtHome = false;
			_svc.MockAzimuth = 270.0;

			_mgr.FindHomePosition();

			Assert.IsTrue( _mgr.Status.Slewing, "The dome has not started slewing to Home!" );

			DateTime timeoutTime = DateTime.Now.AddMinutes( 1.0 );
			bool timedout = false;
			DevHubDomeStatus sts = _mgr.Status;

			while ( !timedout )
			{
				if ( !_mgr.Status.Slewing && _mgr.Status.AtHome )
				{
					break;
				}
				else
				{
					if ( DateTime.Now > timeoutTime )
					{
						timedout = true;
					}
					else
					{
						Thread.Sleep( 500 );

						sts = _mgr.Status;
					}
				}
			}

			Assert.IsFalse( timedout );
			Assert.IsFalse( sts.Slewing );
			Assert.IsTrue( sts.AtHome );
			Assert.AreEqual( _svc.HomeAzimuth, _mgr.Azimuth );
		}

		[TestMethod]
		public void SlewShutter()
		{
			// Start with the shutter open at altitude 0 deg.
			_svc.MockShutterStatus = ShutterState.shutterOpen;
			_svc.MockAltitude = 0.0;

			// Slew to a random number between 20 and 90 degrees.

			Random rnd = new Random();
			double altitude = rnd.NextDouble() * 70.0 + 20.0;

			_mgr.SlewDomeShutter( altitude );

			Assert.IsTrue( _mgr.Status.Slewing, "The dome has not started slewing the shutter!" );

			DateTime lastStatusUpdate = _mgr.Status.LastUpdateTime;

			while ( _mgr.Status.LastUpdateTime == lastStatusUpdate )
			{
				Thread.Sleep( 500 );
			}

			while ( _mgr.Status.Slewing )
			{
				Thread.Sleep( 500 );
			}

			Assert.IsFalse( _mgr.Status.Slewing );

			string msg = String.Format( "Expected altitude of {0}, but got {1}.", altitude, _mgr.Status.Altitude );
			Assert.AreEqual( altitude, _mgr.Status.Altitude, _tolerance, msg );
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

				// Pick a random azimuth that is at least 30 degrees from the park azimuth.

				azimuth = ( Math.Abs( azimuth - _svc.MockAzimuth ) < 30.0 ) ? Double.NaN : azimuth;
			}
			while ( Double.IsNaN( azimuth ) );

			_mgr.SlewDomeToAzimuth( azimuth );

			// If this fails, it is most likely due to the fact that the slew is already complete
			// before we get here. That is why we pick a destination azimuth that is at least 30
			// degrees away from the starting azimuth.

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

		#endregion Test Methods

		#region Helper Methods

		private void UpdateDomeSlavedState( DomeSlavedChangedMessage action )
		{
			Globals.IsDomeSlaved = action.State;
		}

		#endregion Helper Methods
	}
}
