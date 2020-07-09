using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using ASCOM;
using ASCOM.DeviceHub;

namespace Unit_Tests
{
	public class MockFocuserService : IFocuserService
	{
		private string DeviceID { get; set; }
		private CancellationTokenSource MoveTaskTokenSource { get; set; }

		public bool MockAbsolute { get; set; }
		public bool MockIsMoving { get; set; }
		public int MockMaxIncrement { get; set; }
		public int MockMaxStep { get; set; }
		public int MockPosition { get; set; }

		public Task MockMoveTask { get; set; }

		public MockFocuserService()
		{
			Initialized = false;
			Connected = false;
			MoveTaskTokenSource = null;
		}

		public bool Initialized { get; private set; }
		public bool DeviceCreated => Initialized;
		public bool DeviceAvailable => DeviceCreated && Connected;

		private bool _connected;

		public bool Connected
		{
			get { return _connected; }
			set { _connected = value; }
		}

		public string Description => "This is a mock ASCOM focuser";
		public string DriverInfo => "Mock ASCOM Focuser for Device Hub Unit Testing";
		public string DriverVersion => "1.0";
		public short InterfaceVersion => 3;
		public string Name => "ASCOM.Mock.Focuser";
		public ArrayList SupportedActions => new ArrayList();

		public bool Absolute
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockAbsolute;
			}
		}

		public bool IsMoving
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockIsMoving;
			}
		}

		public bool Link
		{
			get { return _connected; }
			set { _connected = value; }
		}

		public int MaxIncrement => MockMaxIncrement;
		public int MaxStep => MockMaxStep;

		public int Position
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockPosition;
			}
		}

		public double StepSize => MockStepSize;

		public bool TempComp
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockTempComp;
			}
			set
			{
				MockTempComp = value;
			}
		}

		public double MockStepSize { get; set; }
		public bool MockTempComp { get; set; }
		public bool MockTempCompAvailable { get; set; }
		public double MockTemperature { get; set; }

		public bool TempCompAvailable => MockTempCompAvailable;

		public double Temperature
		{
			get
			{
				if ( !DeviceAvailable )
				{
					throw new NotConnectedException();
				}

				return MockTemperature;
			}
		}

		public string Action( string actionName, string actionParameters )
		{
			throw new MethodNotImplementedException();
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
				throw new Exception( "The focuser service attempted to re-initialize the dome." );
			}

			if ( String.IsNullOrEmpty( id ) )
			{
				throw new Exception( "The focuser service is unable to create a focuser until a Dome has been chosen." );
			}

			DeviceID = id;
			Initialized = true;
		}

		public void Dispose()
		{			
		}

		public void Halt()
		{
			if ( MoveTaskTokenSource != null )
			{
				MoveTaskTokenSource.Cancel();
			}
		}

		public void Move( int position )
		{
			MockIsMoving = true;

			MoveTaskTokenSource = new CancellationTokenSource();
			MockMoveTask = Task.Factory.StartNew( () => { MoveTask( position, MoveTaskTokenSource.Token ); }, MoveTaskTokenSource.Token  );
		}

		private void MoveTask( int position, CancellationToken token )
		{
			bool tempComp = TempComp;
			TempComp = false;

			if ( Absolute )
			{
				MoveAbsolute( position, token );
			}
			else
			{
				MoveRelative( position );
			}

			MockIsMoving = false;
			TempComp = tempComp;
		}

		public void SetupDialog()
		{
			throw new MethodNotImplementedException();
		}

		private void MoveAbsolute( int newPosition, CancellationToken token )
		{
			int oldPosition = Position;

			// Adjust the move target so we don't exceed the MaxIncrement setting;

			newPosition = ClampToMaxMove( newPosition );
			newPosition = ClampToLimit( newPosition );

			int moveSign = Math.Sign( newPosition - oldPosition );

			int interval = 100; // milliseconds
			int maxMovePerIteration = 50;

			int pos = oldPosition;

			bool done = false;

			while ( !done )
			{
				// We will adjust the position by 50 each time through the loop.

				pos = pos + maxMovePerIteration * moveSign;

				if ( moveSign > 0 && pos > newPosition )
				{
					pos = newPosition;
				}
				else if ( moveSign < 0 && pos < newPosition )
				{
					pos = newPosition;
				}

				MockPosition = pos;

				done = ( Position == newPosition ) || token.IsCancellationRequested;

				if ( !done )
				{
					Thread.Sleep( interval );
				}
			}
		}

		private int ClampToLimit( int newPosition )
		{
			newPosition = Math.Min( newPosition, MaxStep );
			newPosition = Math.Max( newPosition, -MaxStep );

			return newPosition;
		}

		private int ClampToMaxMove( int newPosition )
		{
			int pos = newPosition;

			int sign = ( pos - Position > 0 ) ? 1 : -1;
			int delta = Math.Abs( pos - Position );
			delta = Math.Min( delta, MaxIncrement );

			return Position + sign * delta;
		}

		private void MoveRelative( int amount )
		{
			MockIsMoving = true;

			Thread.Sleep( 1000 );

			MockIsMoving = false;
		}
	}
}
