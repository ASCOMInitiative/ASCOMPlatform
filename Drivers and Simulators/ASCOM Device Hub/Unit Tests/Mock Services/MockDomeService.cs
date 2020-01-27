using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

using ASCOM;
using ASCOM.DeviceHub;
using ASCOM.DeviceInterface;

namespace Unit_Tests.Dome
{
	public class MockDomeService : IDomeService
	{
		private string DeviceID { get; set; }

		public double HomeAzimuth => 180.0;
		public double ParkAzimuth => 0.0;
		public double MockAltitude { get; set; }
		public bool MockAtHome { get; set; }
		public bool MockAtPark { get; set; }
		public double MockAzimuth { get; set; }
		public ShutterState MockShutterStatus { get; set; }
		public bool MockSlewing { get; set; }
		public bool MockSlaved { get; set; }

		public MockDomeService()
		{
			Initialized = false;
			Connected = false;
			MockAltitude = Double.NaN;
			MockAtHome = false;
			MockAtPark = true;
			MockAzimuth = Double.NaN;
			MockShutterStatus = ShutterState.shutterClosed;
			MockSlaved = false;
		}

		public bool Initialized { get; private set; }
		public bool DeviceCreated => Initialized;
		public bool DeviceAvailable => DeviceCreated && Connected;

		public bool Connected { get; set; }
		public string Description => "This is a mock ASCOM dome";
		public string DriverInfo => "Mock ASCOM Dome for Device Hub Unit Testing";
		public string DriverVersion => "1.0";
		public short InterfaceVersion => 2;
		public string Name => "ASCOM.Mock.Dome";
		public ArrayList SupportedActions => new ArrayList();

		public double Altitude
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockAltitude;
			}
		}

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

				return MockAzimuth;
			}
		}

		public bool CanFindHome => true;
		public bool CanPark => true;
		public bool CanSetAltitude => true;
		public bool CanSetAzimuth => true;
		public bool CanSetPark => true;
		public bool CanSetShutter => true;
		public bool CanSlave => true;
		public bool CanSyncAzimuth => true;

		public ShutterState ShutterStatus
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockShutterStatus;
			}
		}

		public bool Slaved
		{
			get => MockSlaved;
			set => MockSlaved = value;
		}

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

		public string Action( string actionName, string actionParameters )
		{
			throw new MethodNotImplementedException();
		}

		public void CloseShutter()
		{
			if ( !DeviceAvailable )
			{
				throw new NotConnectedException();
			}

			if ( ShutterStatus == ShutterState.shutterClosed )
			{
				return;
			}

			MockSlewing = true;
			MockShutterStatus = ShutterState.shutterClosing;

			Task.Run( () =>
			{
				Thread.Sleep( 1500 );
				MockShutterStatus = ShutterState.shutterClosed;
				MockSlewing = false;
			} );
		}

		public void CommandBlind( string command, bool raw = false )
		{
			throw new MethodNotImplementedException();
		}

		public bool CommandBool( string command, bool raw = false )
		{
			throw new MethodNotImplementedException();
		}

		public string CommandString( string command, bool raw = false )
		{
			throw new MethodNotImplementedException();
		}

		public void CreateDevice( string id )
		{
			if ( Initialized )
			{
				throw new Exception( "The dome service attempted to re-initialize the dome." );
			}

			if ( String.IsNullOrEmpty( id ) )
			{
				throw new Exception( "The dome service is unable to create a dome until a Dome has been chosen." );
			}

			DeviceID = id;
			Initialized = true;
		}

		public void Dispose()
		{
		}

		public void FindHome()
		{
			if ( !DeviceAvailable )
			{
				throw new NotConnectedException();
			}

			SlewDomeToAzimuth( HomeAzimuth );
		}

		public void OpenShutter()
		{
			if ( !DeviceAvailable )
			{
				throw new NotConnectedException();
			}

			if ( ShutterStatus != ShutterState.shutterClosed )
			{
				throw new DriverException( "Attempted to open a shutter that was not closed." );
			}

			MockSlewing = true;
			MockShutterStatus = ShutterState.shutterOpening;

			Task.Run( () =>
			{
				Thread.Sleep( 1500 );
				MockShutterStatus = ShutterState.shutterOpen;
				MockSlewing = false;
			} );
		}

		public void Park()
		{
			if ( !DeviceAvailable )
			{
				throw new NotConnectedException();
			}

			SlewDomeToAzimuth( ParkAzimuth );
		}

		public void SetPark()
		{
			throw new MethodNotImplementedException();
		}

		public void SetupDialog()
		{
			throw new MethodNotImplementedException();
		}

		public void SlewToAltitude( double targetAltitude )
		{
			if ( !DeviceAvailable )
			{
				throw new NotConnectedException();
			}

			if ( Slewing )
			{
				throw new ASCOM.InvalidOperationException( "Unable to adjust the shutter altitude while the dome is slewing!" );
			}

			if ( !CanSetAltitude )
			{
				throw new ASCOM.InvalidOperationException( "Driver does not support adjusting the shutter altitude!" );
			}

			if ( ShutterStatus != ShutterState.shutterOpen )
			{
				throw new ASCOM.InvalidOperationException( "Unable to adjust the shutter altitude when the shutter is not open!" );
			}

			if ( targetAltitude < 0 || targetAltitude > 90 )
			{
				throw new InvalidValueException( "Altitude must be between 0 and 90 degrees." );
			}

			if ( MockAltitude == targetAltitude )
			{
				return;
			}

			double moveRate = 30; // deg/sec
			double totalMove = targetAltitude - MockAltitude;

			int updateCount = 5;        // Report the intermediate position 5 times during the slew.
			double moveAmount = totalMove / updateCount;
			double moveTime = totalMove / moveRate; // Seconds to complete the slew.

			if ( moveTime < 1.0 )
			{
				// Only report once for a short move.

				updateCount = 1;
			}

			int updateInterval = (int)moveTime * 1000 / updateCount;

			MockSlewing = true;

			Task.Run( () =>
			{
				double updatesReported = 0;

				while ( updatesReported < updateCount )
				{
					double altitude = MockAltitude + ( totalMove / updateCount );

					MockAzimuth = altitude;
					++updatesReported;

					Thread.Sleep( updateInterval );
				}

				MockAltitude = targetAltitude;
				MockSlewing = false;
			} );
		}

		public void SlewToAzimuth( double azimuth )
		{
			if ( !DeviceAvailable )
			{
				throw new NotConnectedException();
			}

			SlewDomeToAzimuth( azimuth );
		}

		public void SyncToAzimuth( double azimuth )
		{
			if ( !DeviceAvailable )
			{
				throw new NotConnectedException();
			}

			MockAzimuth = azimuth;
		}

		#region Helper Methods

		private void SlewDomeToAzimuth( double targetAzimuth )
		{
			// This method performs a simulated slew to a specified azimuth
			// The slew rate is hard-coded below.
			// Slews are always in a clockwise direction.

			if ( MockAzimuth == targetAzimuth )
			{
				return;
			}

			double moveRate = 30; // deg/sec
			double totalMove = targetAzimuth - MockAzimuth;

			if ( totalMove < 0 )
			{
				totalMove += 360.0;
			}

			int updateCount = 5;        // Report the intermediate position 5 times during the slew.
			double moveAmount = totalMove / updateCount;
			double moveTime = totalMove / moveRate; // Seconds to complete the slew.

			if ( moveTime < 1.0 )
			{
				// Only report once for a short move.
				updateCount = 1;
			}

			int updateInterval = (int)moveTime * 1000 / updateCount;

			MockSlewing = true;

			Task.Run( () =>
			 {
				 double updatesReported = 0;

				 while ( updatesReported < updateCount )
				 {
					 double azimuth = MockAzimuth + ( totalMove / updateCount );

					 if ( azimuth > 360.0 )
					 {
						 azimuth -= 360.0;
					 }

					 MockAzimuth = azimuth;
					 ++updatesReported;

					 Thread.Sleep( updateInterval );
				 }

				 MockAzimuth = targetAzimuth;
				 MockSlewing = false;

				 if ( targetAzimuth == HomeAzimuth )
				 {
					 MockAtHome = true;
				 }
				 else if ( targetAzimuth == ParkAzimuth )
				 {
					 MockAtPark = true;
				 }
			 } );
		}

		#endregion Helper Methods
	}
}
