using ASCOM;
using ASCOM.DriverAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace PlatformUnitTests
{
    /// <summary>
    /// Summary description for UnitTest2
    /// </summary>
    [TestClass]
    public class SwitchAsync
    {
        [TestMethod]
        public void SetSwitch()
        {
            // Create a Switch simulator device
            Type switchType = Type.GetTypeFromProgID("ASCOM.Simulator.Switch");
            dynamic switchSim = Activator.CreateInstance(switchType);

            // Connect
            switchSim.Connected = true;
            Assert.IsTrue(switchSim.Connected);

            // Confirm that some devices are configured
            Assert.AreEqual<short>(11, switchSim.MaxSwitch);

            // Confirm that device 10 is async
            Assert.IsFalse(switchSim.CanAsync(9));
            Assert.IsTrue(switchSim.CanAsync(10));

            // Confirm that the current value is false
            Assert.IsFalse(switchSim.GetSwitch(10));

            // Set the value true synchronously (should take 3 seconds to complete)
            switchSim.SetSwitch(10, true);

            // Confirm the expected outcomes
            Assert.IsTrue(switchSim.StateChangeComplete(10));
            Assert.IsTrue(switchSim.GetSwitch(10));

            switchSim.Connected = false;
        }

        [TestMethod]
        public void SetAsync()
        {
            // Create a Switch simulator device
            Type switchType = Type.GetTypeFromProgID("ASCOM.Simulator.Switch");
            dynamic switchSim = Activator.CreateInstance(switchType);

            // Connect
            switchSim.Connected = true;
            Assert.IsTrue(switchSim.Connected);

            // Confirm that some devices are configured
            Assert.AreEqual<short>(11, switchSim.MaxSwitch);

            // Confirm that device 10 is async
            Assert.IsFalse(switchSim.CanAsync(9));
            Assert.IsTrue(switchSim.CanAsync(10));

            // Confirm that the current value is false
            Assert.IsFalse(switchSim.GetSwitch(10));

            // Set the value true asynchronously (should take 3 seconds to complete)
            switchSim.SetAsync(10, true);
            Assert.IsFalse(switchSim.StateChangeComplete(10));
            Assert.IsFalse(switchSim.GetSwitch(10));

            // Wait for most of 3 seconds and make sure the operation is still running
            Thread.Sleep(2950);
            Assert.IsFalse(switchSim.StateChangeComplete(10));
            Assert.IsFalse(switchSim.GetSwitch(10));

            // Wait a short while longer to make sure that the new value is in effect
            Thread.Sleep(150);
            Assert.IsTrue(switchSim.StateChangeComplete(10));
            Assert.IsTrue(switchSim.GetSwitch(10));

            switchSim.Connected = false;
        }

        [TestMethod]
        public void SetSwitchValue()
        {
            // Create a Switch simulator device
            Type switchType = Type.GetTypeFromProgID("ASCOM.Simulator.Switch");
            dynamic switchSim = Activator.CreateInstance(switchType);

            // Connect
            switchSim.Connected = true;
            Assert.IsTrue(switchSim.Connected);

            // Confirm that some devices are configured
            Assert.AreEqual<short>(11, switchSim.MaxSwitch);

            // Confirm that device 10 is async
            Assert.IsFalse(switchSim.CanAsync(9));
            Assert.IsTrue(switchSim.CanAsync(10));

            // Confirm that the current value is false
            Assert.IsFalse(switchSim.GetSwitch(10));

            // Set the value true synchronously (should take 3 seconds to complete)
            switchSim.SetSwitchValue(10, 5.0);

            // Confirm the expected outcomes
            Assert.IsTrue(switchSim.StateChangeComplete(10));
            Assert.AreEqual<double>(5.0, switchSim.GetSwitchValue(10));

            switchSim.Connected = false;
        }

        [TestMethod]
        public void SetAsyncValue()
        {
            // Create a Switch simulator device
            Type switchType = Type.GetTypeFromProgID("ASCOM.Simulator.Switch");
            dynamic switchSim = Activator.CreateInstance(switchType);

            // Connect
            switchSim.Connected = true;
            Assert.IsTrue(switchSim.Connected);

            // Confirm that some devices are configured
            Assert.AreEqual<short>(11, switchSim.MaxSwitch);

            // Confirm that device 10 is async
            Assert.IsFalse(switchSim.CanAsync(9));
            Assert.IsTrue(switchSim.CanAsync(10));

            // Confirm that the current value is 0.0
            Assert.AreEqual<double>(0.0, switchSim.GetSwitchValue(10));

            // Set the value true asynchronously (should take 3 seconds to complete)
            switchSim.SetAsyncValue(10, 5.0);
            Assert.IsFalse(switchSim.StateChangeComplete(10));
            Assert.AreEqual<double>(0.0, switchSim.GetSwitchValue(10));

            // Wait for most of 3 seconds and make sure the operation is still running
            Thread.Sleep(2950);
            Assert.IsFalse(switchSim.StateChangeComplete(10));
            Assert.AreEqual<double>(0.0, switchSim.GetSwitchValue(10));

            // Wait a short while longer to make sure that the new value is in effect
            Thread.Sleep(150);
            Assert.IsTrue(switchSim.StateChangeComplete(10));
            Assert.AreEqual<double>(5.0, switchSim.GetSwitchValue(10));

            switchSim.Connected = false;
        }

        [TestMethod]
        public void CancelAsync()
        {
            // Create a Switch simulator device
            Type switchType = Type.GetTypeFromProgID("ASCOM.Simulator.Switch");
            dynamic switchSim = Activator.CreateInstance(switchType);

            // Connect
            switchSim.Connected = true;
            Assert.IsTrue(switchSim.Connected);

            // Confirm that some devices are configured
            Assert.AreEqual<short>(11, switchSim.MaxSwitch);

            // Confirm that device 10 is async
            Assert.IsFalse(switchSim.CanAsync(9));
            Assert.IsTrue(switchSim.CanAsync(10));

            // Confirm that the current value is false
            Assert.IsFalse(switchSim.GetSwitch(10));

            // Set the value true asynchronously (should take 3 seconds to complete)
            switchSim.SetAsync(10, true);
            Assert.IsFalse(switchSim.StateChangeComplete(10));
            Assert.IsFalse(switchSim.GetSwitch(10));

            // Wait for 2 seconds and then cancel the operation
            Thread.Sleep(2000);
            Assert.IsFalse(switchSim.StateChangeComplete(10));
            Assert.IsFalse(switchSim.GetSwitch(10));
            switchSim.CancelAsync(10);

            // Wait a short while before checking that the operation is cancelled
            Thread.Sleep(100);
            Assert.ThrowsException<COMException>(() => switchSim.StateChangeComplete(10));
            Assert.IsFalse(switchSim.GetSwitch(10));

            // Wait until after 3 seconds and make sure that the outcome is still the same
            Thread.Sleep(1000);
            Assert.ThrowsException<COMException>(() => switchSim.StateChangeComplete(10));
            Assert.IsFalse(switchSim.GetSwitch(10));

            switchSim.Connected = false;
        }

        [TestMethod]
        public void SetAsyncDriverAccess()
        {
            // Create a Switch simulator device
            ASCOM.DriverAccess.Switch switchSim = new ASCOM.DriverAccess.Switch("ASCOM.Simulator.Switch");

            // Connect
            switchSim.Connected = true;
            Assert.IsTrue(switchSim.Connected);

            // Confirm that some devices are configured
            Assert.AreEqual<short>(11, switchSim.MaxSwitch);

            // Confirm that device 10 is async
            Assert.IsFalse(switchSim.CanAsync(9));
            Assert.IsTrue(switchSim.CanAsync(10));

            // Confirm that the current value is false
            Assert.IsFalse(switchSim.GetSwitch(10));

            // Set the value true asynchronously (should take 3 seconds to complete)
            switchSim.SetAsync(10, true);
            Assert.IsFalse(switchSim.StateChangeComplete(10));
            Assert.IsFalse(switchSim.GetSwitch(10));

            // Wait for most of 3 seconds and make sure the operation is still running
            Thread.Sleep(2950);
            Assert.IsFalse(switchSim.StateChangeComplete(10));
            Assert.IsFalse(switchSim.GetSwitch(10));

            // Wait a short while longer to make sure that the new value is in effect
            Thread.Sleep(150);
            Assert.IsTrue(switchSim.StateChangeComplete(10));
            Assert.IsTrue(switchSim.GetSwitch(10));

            switchSim.Connected = false;
        }

        [TestMethod]
        public void SetAsyncValueDriverAccess()
        {
            // Create a Switch simulator device
            Switch switchSim = new Switch("ASCOM.Simulator.Switch");

            // Connect
            switchSim.Connected = true;
            Assert.IsTrue(switchSim.Connected);

            // Confirm that some devices are configured
            Assert.AreEqual<short>(11, switchSim.MaxSwitch);

            // Confirm that device 10 is async
            Assert.IsFalse(switchSim.CanAsync(9));
            Assert.IsTrue(switchSim.CanAsync(10));

            // Confirm that the current value is 0.0
            Assert.AreEqual<double>(0.0, switchSim.GetSwitchValue(10));

            // Set the value true asynchronously (should take 3 seconds to complete)
            switchSim.SetAsyncValue(10, 5.0);
            Assert.IsFalse(switchSim.StateChangeComplete(10));
            Assert.AreEqual<double>(0.0, switchSim.GetSwitchValue(10));

            // Wait for most of 3 seconds and make sure the operation is still running
            Thread.Sleep(2950);
            Assert.IsFalse(switchSim.StateChangeComplete(10));
            Assert.AreEqual<double>(0.0, switchSim.GetSwitchValue(10));

            // Wait a short while longer to make sure that the new value is in effect
            Thread.Sleep(150);
            Assert.IsTrue(switchSim.StateChangeComplete(10));
            Assert.AreEqual<double>(5.0, switchSim.GetSwitchValue(10));

            switchSim.Connected = false;
        }

        [TestMethod]
        public void CancelAsyncDriverAccess()
        {
            // Create a Switch simulator device
            Switch switchSim = new Switch("ASCOM.Simulator.Switch");

            // Connect
            switchSim.Connected = true;
            Assert.IsTrue(switchSim.Connected);

            // Confirm that some devices are configured
            Assert.AreEqual<short>(11, switchSim.MaxSwitch);

            // Confirm that device 10 is async
            Assert.IsFalse(switchSim.CanAsync(9));
            Assert.IsTrue(switchSim.CanAsync(10));

            // Confirm that the current value is false
            Assert.IsFalse(switchSim.GetSwitch(10));

            // Set the value true asynchronously (should take 3 seconds to complete)
            switchSim.SetAsync(10, true);
            Assert.IsFalse(switchSim.StateChangeComplete(10));
            Assert.IsFalse(switchSim.GetSwitch(10));

            // Wait for 2 seconds and then cancel the operation
            Thread.Sleep(2000);
            Assert.IsFalse(switchSim.StateChangeComplete(10));
            Assert.IsFalse(switchSim.GetSwitch(10));
            switchSim.CancelAsync(10);

            // Wait a short while before checking that the operation is cancelled and an OperationCancelledException is thrown
            Thread.Sleep(100);
            Exception exception = Assert.ThrowsException<DriverAccessCOMException>(() => switchSim.StateChangeComplete(10));
            Assert.AreEqual<int>(ErrorCodes.OperationCancelled, exception.HResult);

            Assert.IsFalse(switchSim.GetSwitch(10));

            // Wait until after 3 seconds and make sure that the outcome is still the same
            Thread.Sleep(1000);
            Assert.ThrowsException<DriverAccessCOMException>(() => switchSim.StateChangeComplete(10));
            exception = Assert.ThrowsException<DriverAccessCOMException>(() => switchSim.StateChangeComplete(10));
            Assert.IsFalse(switchSim.GetSwitch(10));

            switchSim.Connected = false;
        }
    }
}
