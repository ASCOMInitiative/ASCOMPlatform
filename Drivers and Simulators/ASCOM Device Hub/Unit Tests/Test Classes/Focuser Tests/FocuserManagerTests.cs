using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ASCOM.DeviceHub;
using ASCOM.DeviceHub.MvvmMessenger;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_Tests.Focuser
{
	[TestClass]
	public class FocuserManagerTests
	{
		private const string FocuserID = "Mock Focuser";

		private FocuserManager _mgr;
		private MockFocuserService _svc;

		private int _startupDelayMs = 8000;
		private bool _moveCompleted;

		[ClassInitialize]
		public static void ClassInit( TestContext context )
		{
		}

		[TestInitialize]
		public void TestInit()
		{
			ServiceContainer.Instance.ClearAllServices();
			ServiceContainer.Instance.AddService<IFocuserService>( new MockFocuserService() );
			_mgr = FocuserManager.Instance;
			_svc = (MockFocuserService)ServiceContainer.Instance.GetService<IFocuserService>();

			Globals.UISyncContext = TaskScheduler.Default;
		}

		[TestCleanup]
		public void TestCleanup()
		{
			_mgr.Disconnect();
			Assert.IsFalse( _svc.Connected );

			_mgr.Dispose();
		}

		[TestMethod]
		public void Connect()
		{
			bool retval = _mgr.Connect( FocuserID );
			Assert.AreEqual( true, retval );

			Thread.Sleep( _startupDelayMs );

			Assert.AreEqual( true, _mgr.Connected );

			Assert.AreEqual( _svc.Description, _mgr.Parameters.Description );
			Assert.AreEqual( _svc.DriverInfo, _mgr.Parameters.DriverInfo );
			Assert.AreEqual( _svc.DriverVersion, _mgr.Parameters.DriverVersion );
			Assert.AreEqual( _svc.InterfaceVersion, _mgr.Parameters.InterfaceVersion );
			Assert.AreEqual( _svc.SupportedActions.Count, _mgr.Parameters.SupportedActions.Length );
		}

		[TestMethod]
		public void Disconnect()
		{
			bool retval = _mgr.Connect( FocuserID );
			Assert.AreEqual( true, retval );

			// Disconnection is tested in the TestCleanup method.
		}

		[TestMethod]
		public void MoveAbsolutePositive()
		{
			double timeoutSeconds = 30;
			int maxIncrement = 10000;
			int requestedMove;

			_svc.MockAbsolute = true;
			_svc.MockMaxStep = 25000;
			_svc.MockMaxIncrement = maxIncrement;
			_svc.MockTempComp = false;

			Messenger.Default.Register<FocuserMoveCompletedMessage>
							( this, (action) => MoveCompleted( action ) );

			bool retval = _mgr.Connect( FocuserID );
			Assert.AreEqual( true, retval );

			Thread.Sleep( _startupDelayMs );

			Assert.IsTrue( _mgr.Parameters.Absolute, "Parameters did not get updated after Connect" );

			// Test a positive move.

			_moveCompleted = false;
			_svc.MockPosition = 20000;
			_svc.MockIsMoving = false;
			requestedMove = 1000;

			//what do I have to do to reset from this move to be able to do the next one????


			_mgr.MoveFocuserBy( requestedMove );
			Assert.IsTrue( _mgr.Status.IsMoving, "Positive move did not start." );

			MonitorMovement( timeoutSeconds );

			//Assert.IsFalse( _mgr.Status.IsMoving, "Positive move did not complete." );
			Assert.AreEqual( 21000, _mgr.Status.Position, "Positive move not at expected position." );

			// Test a positive move that should be limited by the MaxIncrement.

			_moveCompleted = false;
			_svc.MockPosition = 9000;
			_svc.MockIsMoving = false;
			requestedMove = maxIncrement + 1000;

			// The expected final position should be 9000 + 10000 rather than
			// 9000 + 11000 as we requested.

			_mgr.MoveFocuserBy( requestedMove );
			Assert.IsTrue( _mgr.Status.IsMoving, "Limited positive move did not start." );

			MonitorMovement( timeoutSeconds );

			Assert.AreEqual( 19000, _mgr.Status.Position, "Limited positive move not at expected position." );

			// Test a positive move that should be limited by the MaxStep.

			_svc.MockPosition = 19000;
			requestedMove = 7000;

			// The expected final position should be 25000 rather than
			// 19000 + 7000 as we requested.

			_moveCompleted = false;
			_mgr.MoveFocuserBy( requestedMove );
			Assert.IsTrue( _mgr.Status.IsMoving, "Limited step positive move did not start." );

			MonitorMovement( timeoutSeconds );

			//Assert.IsFalse( _mgr.Status.IsMoving, "Limited step positive move did not complete." );
			Assert.AreEqual( 25000, _mgr.Status.Position, "Limited step positive move not at expected position." );

			Messenger.Default.Unregister<FocuserMoveCompletedMessage>( this );
		}
		[TestMethod]
		public void MoveAbsoluteNegative()
		{
			double timeoutSeconds = 30;
			int maxIncrement = 10000;
			int requestedMove;

			_svc.MockAbsolute = true;
			_svc.MockMaxStep = 25000;
			_svc.MockMaxIncrement = maxIncrement;
			_svc.MockTempComp = false;

			Messenger.Default.Register<FocuserMoveCompletedMessage>
							( this, ( action ) => MoveCompleted( action ) );

			bool retval = _mgr.Connect( FocuserID );
			Assert.AreEqual( true, retval );

			Thread.Sleep( _startupDelayMs );

			Assert.IsTrue( _mgr.Parameters.Absolute, "Parameters did not get updated after Connect" );

			// Test a negative move.

			//_moveCompleted = false;
			//_svc.MockPosition = 21000;
			//_svc.MockIsMoving = false;
			//_mgr.MoveFocuserBy( -1000 );
			//Assert.IsTrue( _mgr.Status.IsMoving, "Negative move did not start." );

			//MonitorMovement( timeoutSeconds );

			////Assert.IsFalse( _mgr.Status.IsMoving, "Negative move did not complete." );
			//Assert.AreEqual( 20000, _mgr.Status.Position, "Negative move not at expected position." );

			// Test a negative move that should be limited by the MaxIncrement.

			_moveCompleted = false;
			_svc.MockIsMoving = false;
			_svc.MockPosition = 19000;
			requestedMove = -( maxIncrement + 1000 );

			// The expected final position should be 19000 - 10000 rather than
			// 19000 - 11000 as we requested.

			_mgr.MoveFocuserBy( requestedMove );
			Assert.IsTrue( _mgr.Status.IsMoving, "Limited negative move did not start." );

			MonitorMovement( timeoutSeconds );

			//Assert.IsFalse( _mgr.Status.IsMoving, "Limited negative move did not complete." );
			//Assert.AreEqual( 9000, _mgr.Status.Position, "Limited negative move not at expected position." );

			// Test a negative move that should be limited by the MaxStep.

			_moveCompleted = false;
			_svc.MockIsMoving = false;
			_svc.MockPosition = -22000;
			requestedMove = -7000;

			// The expected final position should be -25000 rather than
			// -22000 - 7000 as we requested.

			_mgr.MoveFocuserBy( requestedMove );
			Assert.IsTrue( _mgr.Status.IsMoving, "Limited step negative move did not start." );

			MonitorMovement( timeoutSeconds );

			//Assert.IsFalse( _mgr.Status.IsMoving, "Limited step negative move did not complete." );
			Assert.AreEqual( -25000, _mgr.Status.Position, "Limited step negative move not at expected position." );

			Messenger.Default.Unregister<FocuserMoveCompletedMessage>( this );
		}

		[TestMethod]
		public void MoveRelative()
		{
			double timeoutSeconds = 30;

			_svc.MockAbsolute = false;
			_svc.MockMaxIncrement = 5000;
			_svc.MockTempComp = false;

			Messenger.Default.Register<FocuserMoveCompletedMessage>
				( this, ( action ) => MoveCompleted( action ) );

			bool retval = _mgr.Connect( FocuserID );
			Assert.AreEqual( true, retval, "Connect MoveRelative failed." );

			Thread.Sleep( _startupDelayMs );

			_moveCompleted = false;
			_mgr.MoveFocuserBy( 1000 );
			Assert.IsTrue( _mgr.Status.IsMoving, "Positive relative move did not start." );

			MonitorMovement( timeoutSeconds );

			Assert.IsFalse( _mgr.Status.IsMoving, "Positive relative move did not complete." );

			_moveCompleted = false;
			_mgr.MoveFocuserBy( -1000 );
			Assert.IsTrue( _mgr.Status.IsMoving, "Negative relative move did not start." );

			MonitorMovement( timeoutSeconds );

			Assert.IsFalse( _mgr.Status.IsMoving, "Negative relative move did not complete." );

			_moveCompleted = false;
			_mgr.MoveFocuserBy( -6000 );
			Assert.IsTrue( _mgr.Status.IsMoving, "Limited negative relative move did not start." );

			MonitorMovement( timeoutSeconds );

			Assert.IsFalse( _mgr.Status.IsMoving, "Limited negative relative move did not complete." );

			_moveCompleted = false;
			_mgr.MoveFocuserBy( 6000 );
			Assert.IsTrue( _mgr.Status.IsMoving, "Limited positive relative move did not start." );

			MonitorMovement( timeoutSeconds );

			Assert.IsFalse( _mgr.Status.IsMoving, "Limited positive relative move did not complete." );

			Messenger.Default.Unregister<FocuserMoveCompletedMessage>( this );
		}

		[TestMethod]
		public void HaltFocuser()
		{
			int requestedMove;

			_svc.MockAbsolute = true;
			_svc.MockMaxStep = 25000;
			_svc.MockMaxIncrement = 25000;
			_svc.MockTempComp = false;

			bool retval = _mgr.Connect( FocuserID );
			Assert.AreEqual( true, retval );

			Thread.Sleep( _startupDelayMs );

			Assert.IsTrue( _mgr.Parameters.Absolute, "Parameters did not get updated after Connect" );

			// Test a positive move that should be limited by the MaxIncrement.

			_svc.MockPosition = 2000;
			requestedMove = 20000;

			_mgr.MoveFocuserBy( requestedMove );
			Assert.IsTrue( _mgr.Status.IsMoving, "Positive move did not start." );

			Thread.Sleep( 2000 );
			Assert.IsTrue( _mgr.Status.IsMoving, "Positive move ended too soon." );

			_mgr.HaltFocuser();
			
			Assert.IsFalse( _mgr.Status.IsMoving, "Positive move was not halted." );
			Assert.AreNotEqual( 22000, _mgr.Status.Position, "Positive move was not halted." );
		}

		#region Helper Methods

		private void MonitorMovement( double timeoutSeconds )
		{
			DateTime timeoutTime = DateTime.Now.AddSeconds( timeoutSeconds );

			while ( !_moveCompleted || _mgr.Status.IsMoving )
			{
				Thread.Sleep( 500 );

				if ( DateTime.Now > timeoutTime )
				{
					break;
				}
			}
		}

		private void MoveCompleted( FocuserMoveCompletedMessage action )
		{
			Task.Factory.StartNew( () => _moveCompleted = true, CancellationToken.None, TaskCreationOptions.None, Globals.UISyncContext );
		}

		#endregion Helper Methods
	}
}
