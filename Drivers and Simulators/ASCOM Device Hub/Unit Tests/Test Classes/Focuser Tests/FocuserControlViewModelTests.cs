using System;
using System.Threading;
using System.Threading.Tasks;
using ASCOM.DeviceHub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_Tests.Focuser
{
	[TestClass]
	public class FocuserControlViewModelTests
	{
		private MockFocuserManager _mgr = null;

		private FocuserControlViewModel _vm = null;
		private PrivateObject _prVm = null;

		[TestInitialize]
		public void TestInit()
		{
			ServiceContainer.Instance.ClearAllServices();
			ServiceContainer.Instance.AddService<IMessageBoxService>( new MockMessageBoxService() );
			ServiceContainer.Instance.AddService<IAppSettingsService>( new MockAppSettingsService() );
			_mgr = new MockFocuserManager();

			_vm = new FocuserControlViewModel( _mgr );
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
		public void SaveOffset()
		{
			// Initialize the mock settings service with some old value. We will test
			// That it is overwritten.

			double oldTemperatureOffset = 0.0;
			double targetTemperatureOffset = 4.5;

			MockAppSettingsService svc = (MockAppSettingsService)ServiceContainer.Instance.GetService<IAppSettingsService>();
			svc.MockTemperatureOffset = oldTemperatureOffset;

			// Here is our new value.

			_vm.TemperatureOffset = targetTemperatureOffset;

			_prVm.Invoke( "SaveOffset" );

			Assert.AreEqual( targetTemperatureOffset, svc.MockTemperatureOffset );
		}

		[TestMethod]
		public void MoveFocuserInward()
		{
			int initialPosition = 1700;
			int moveAmount = 1000;
			int finalPosition = initialPosition - moveAmount;

			FocuserParameters parms = new FocuserParameters
			{
				Absolute = true,
				MaxIncrement = 25000,
				MaxStep = 25000
			};

			_mgr.MockPosition = initialPosition;

			DevHubFocuserStatus sts = DevHubFocuserStatus.GetEmptyStatus();
			sts.Connected = true;
			sts.Link = true;
			sts.Position = initialPosition;
			sts.TempComp = false;
			sts.Temperature = 20.5;

			_mgr.Status = sts;
			_mgr.Parameters = parms;
			_vm.Parameters = parms;
			_vm.Status = sts;
			
			_vm.MoveIncrement = moveAmount;

			_prVm.Invoke( "MoveFocuserInward" );

			Thread.Sleep( 2500 );

			Assert.AreEqual( finalPosition, _vm.Status.Position );
		}

		[TestMethod]
		public void MoveFocuserOutward()
		{
			int initialPosition = 1700;
			int moveAmount = 1000;
			int finalPosition = initialPosition + moveAmount;

			FocuserParameters parms = new FocuserParameters
			{
				Absolute = true,
				MaxIncrement = 25000,
				MaxStep = 25000
			};

			_mgr.MockPosition = initialPosition;

			DevHubFocuserStatus sts = DevHubFocuserStatus.GetEmptyStatus();
			sts.Connected = true;
			sts.Link = true;
			sts.Position = initialPosition;
			sts.TempComp = false;
			sts.Temperature = 20.5;

			_mgr.Status = sts;
			_mgr.Parameters = parms;
			_vm.Parameters = parms;
			_vm.Status = sts;

			_vm.MoveIncrement = moveAmount;

			_prVm.Invoke( "MoveFocuserOutward" );

			Thread.Sleep( 2500 );

			Assert.AreEqual( finalPosition, _vm.Status.Position );
		}

		[TestMethod]
		public void MoveFocuserToPosition()
		{
			int initialPosition = 1700;
			int moveAmount = 12345;
			int finalPosition = initialPosition + moveAmount;

			FocuserParameters parms = new FocuserParameters
			{
				Absolute = true,
				MaxIncrement = 25000,
				MaxStep = 25000
			};

			_mgr.MockPosition = initialPosition;

			DevHubFocuserStatus sts = DevHubFocuserStatus.GetEmptyStatus();
			sts.Connected = true;
			sts.Link = true;
			sts.Position = initialPosition;
			sts.TempComp = false;
			sts.Temperature = 20.5;

			_mgr.Status = sts;
			_mgr.Parameters = parms;
			_vm.Parameters = parms;
			_vm.Status = sts;

			_vm.TargetPosition = finalPosition.ToString();

			_prVm.Invoke( "MoveFocuserToPosition" );

			Thread.Sleep( 2500 );

			Assert.AreEqual( finalPosition, _vm.Status.Position );
		}
	}
}
