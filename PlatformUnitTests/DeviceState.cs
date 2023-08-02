using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PlatformUnitTests
{
    [TestClass]
    public class DeviceStateTests
    {
        [TestMethod]
        public void Camera()
        {
            using (Camera device = new Camera("ASCOM.Simulator.Camera"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.IsTrue(device.Connected);

                CameraDeviceState deviceState = device.CameraDeviceState;

                Assert.IsTrue(deviceState.CameraState.HasValue);
                Assert.IsTrue(deviceState.CCDTemperature.HasValue);
                Assert.IsTrue(deviceState.CoolerPower.HasValue);
                Assert.IsTrue(deviceState.HeatSinkTemperature.HasValue);
                Assert.IsTrue(deviceState.ImageReady.HasValue);
                Assert.IsTrue(deviceState.IsPulseGuiding.HasValue);
                Assert.IsTrue(deviceState.PercentCompleted.HasValue);
                Assert.IsTrue(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [TestMethod]
        public void CoverCalibrator()
        {
            using (CoverCalibrator device = new CoverCalibrator("ASCOM.Simulator.CoverCalibrator"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.IsTrue(device.Connected);

                CoverCalibratorState deviceState = device.CoverCalibratorState;

                Assert.IsTrue(deviceState.Brightness.HasValue);
                Assert.IsTrue(deviceState.CalibratorReady.HasValue);
                Assert.IsTrue(deviceState.CalibratorState.HasValue);
                Assert.IsTrue(deviceState.CoverMoving.HasValue);
                Assert.IsTrue(deviceState.CoverState.HasValue);
                Assert.IsTrue(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [TestMethod]
        public void Dome()
        {
            using (Dome device = new Dome("ASCOM.Simulator.Dome"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.IsTrue(device.Connected);

                DomeState deviceState = device.DomeState;

                Assert.IsTrue(deviceState.Altitude.HasValue);
                Assert.IsTrue(deviceState.AtHome.HasValue);
                Assert.IsTrue(deviceState.AtPark.HasValue);
                Assert.IsTrue(deviceState.Azimuth.HasValue);
                Assert.IsTrue(deviceState.ShutterStatus.HasValue);
                Assert.IsTrue(deviceState.Slewing.HasValue);
                Assert.IsTrue(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }
        [TestMethod]
        public void FilterWheel()
        {
            using (FilterWheel device = new FilterWheel("ASCOM.Simulator.FilterWheel"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.IsTrue(device.Connected);

                FilterWheelState deviceState = device.FilterWheelState;

                Assert.IsTrue(deviceState.Position.HasValue);
                Assert.IsTrue(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }
        [TestMethod]
        public void Focuser()
        {
            using (Focuser device = new Focuser("ASCOM.Simulator.Focuser"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.IsTrue(device.Connected);

                FocuserState deviceState = device.FocuserState;

                Assert.IsTrue(deviceState.IsMoving.HasValue);
                Assert.IsTrue(deviceState.Position.HasValue);
                Assert.IsTrue(deviceState.Temperature.HasValue);
                Assert.IsTrue(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [TestMethod]
        public void ObservingConditions()
        {
            using (ObservingConditions device = new ObservingConditions("ASCOM.Simulator.ObservingConditions"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.IsTrue(device.Connected);

                ObservingConditionsState deviceState = device.ObservingConditionsState;

                Assert.IsTrue(deviceState.CloudCover.HasValue);
                Assert.IsTrue(deviceState.DewPoint.HasValue);
                Assert.IsTrue(deviceState.Humidity.HasValue);
                Assert.IsTrue(deviceState.Pressure.HasValue);
                Assert.IsTrue(deviceState.RainRate.HasValue);
                Assert.IsTrue(deviceState.SkyBrightness.HasValue);
                Assert.IsTrue(deviceState.SkyQuality.HasValue);
                Assert.IsTrue(deviceState.SkyTemperature.HasValue);
                Assert.IsTrue(deviceState.StarFWHM.HasValue);
                Assert.IsTrue(deviceState.Temperature.HasValue);
                Assert.IsTrue(deviceState.WindDirection.HasValue);
                Assert.IsTrue(deviceState.WindGust.HasValue);
                Assert.IsTrue(deviceState.WindSpeed.HasValue);
                Assert.IsTrue(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [TestMethod]
        public void Rotator()
        {
            using (Rotator device = new Rotator("ASCOM.Simulator.Rotator"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.IsTrue(device.Connected);

                RotatorState deviceState = device.RotatorState;

                Assert.IsTrue(deviceState.IsMoving.HasValue);
                Assert.IsTrue(deviceState.MechanicalPosition.HasValue);
                Assert.IsTrue(deviceState.Position.HasValue);
                Assert.IsTrue(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [TestMethod]
        public void SafetyMonitor()
        {
            using (SafetyMonitor device = new SafetyMonitor("ASCOM.Simulator.SafetyMonitor"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.IsTrue(device.Connected);

                SafetyMonitorState deviceState = device.SafetyMonitorState;

                Assert.IsTrue(deviceState.IsSafe.HasValue);
                Assert.IsTrue(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [TestMethod]
        public void Video()
        {
            using (Video device = new Video("ASCOM.Simulator.Video"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.IsTrue(device.Connected);

                VideoState deviceState = device.VideoState;

                Assert.IsTrue(deviceState.CameraState.HasValue);
                Assert.AreEqual(nameof(IVideoV2.CameraState), nameof(VideoState.CameraState));
                Assert.IsTrue(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [TestMethod]
        public void Telescope()
        {
            using (Telescope device = new Telescope("ASCOM.Simulator.Telescope"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.IsTrue(device.Connected);

                TelescopeState deviceState = device.TelescopeState;

                Assert.IsTrue(deviceState.Altitude.HasValue);
                Assert.IsTrue(deviceState.AtHome.HasValue);
                Assert.IsTrue(deviceState.AtPark.HasValue);
                Assert.IsTrue(deviceState.Azimuth.HasValue);
                Assert.IsTrue(deviceState.Declination.HasValue);
                Assert.IsTrue(deviceState.IsPulseGuiding.HasValue);
                Assert.IsTrue(deviceState.RightAscension.HasValue);
                Assert.IsTrue(deviceState.SideOfPier.HasValue);
                Assert.IsTrue(deviceState.SiderealTime.HasValue);
                Assert.IsTrue(deviceState.Slewing.HasValue);
                Assert.IsTrue(deviceState.Tracking.HasValue);
                Assert.IsTrue(deviceState.UTCDate.HasValue);
                Assert.IsTrue(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }
    }
}
