using ASCOM.DeviceInterface;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASCOM.DriverAccess;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit;

namespace PlatformUnitTests
{
    //[TestClass]
    public class ComDeviceStateTests
    {
        private readonly ITestOutputHelper output;

        public ComDeviceStateTests(ITestOutputHelper output)
        {
            this.output = output;
        }



        [Fact]
        public void StateCollection_ForEach()
        {
            const string STATE_NAME_0 = "State name 1";
            const string STATE_NAME_1 = "State name 2";
            const int STATE_VALUE_0 = 1;
            const double STATE_VALUE_1 = 3.14159;

            StateValueCollection collection = new StateValueCollection
            {
                new StateValue(STATE_NAME_0, STATE_VALUE_0)
            };
            collection.Add(STATE_NAME_1, STATE_VALUE_1);

            output.WriteLine($"Collection count: {collection.Count}");
            Assert.Equal(2, collection.Count);

            output.WriteLine($"ForEach test");
            foreach (IStateValue value in collection)
            {
                if (value == null)
                {
                    output.WriteLine($"ForEach - Returned value is null");
                }
                else
                {
                    output.WriteLine($"Found {value.Name} = {value.Value}");
                }
            }

            collection.Dispose();

        }

        [Fact]
        public void StateCollection_ForNext()
        {
            const string STATE_NAME_0 = "State name 1";
            const string STATE_NAME_1 = "State name 2";
            const int STATE_VALUE_0 = 1;
            const double STATE_VALUE_1 = 3.14159;

            StateValueCollection collection = new StateValueCollection
            {
                new StateValue(STATE_NAME_0, STATE_VALUE_0)
            };
            collection.Add(STATE_NAME_1, STATE_VALUE_1);

            output.WriteLine($"Collection count: {collection.Count}");
            Assert.Equal(2, collection.Count);

            output.WriteLine($"For..Next test");
            for (int i = 0; i <= 1; i++)
            {
                output.WriteLine($"Found {collection[i].Name} = {collection[i].Value}");

                if (i == 0)
                {
                    Assert.Equal(STATE_NAME_0, collection[i].Name);
                    Assert.Equal(STATE_VALUE_0, collection[i].Value);
                }
                else
                {
                    Assert.Equal(STATE_NAME_1, collection[i].Name);
                    Assert.Equal(STATE_VALUE_1, collection[i].Value);
                }
            }

            collection.Dispose();

        }

        [Fact]
        public void StateCollection_Using()
        {
            const string STATE_NAME_0 = "State name 1";
            const string STATE_NAME_1 = "State name 2";
            const int STATE_VALUE_0 = 1;
            const double STATE_VALUE_1 = 3.14159;

            output.WriteLine($"Using test");

            using (StateValueCollection collection1 = new StateValueCollection())
            {
                collection1.Add(STATE_NAME_0, STATE_VALUE_0);
                collection1.Add(STATE_NAME_1, STATE_VALUE_1);

                foreach (IStateValue value in collection1)
                {
                    if (value == null)
                    {
                        output.WriteLine($"ForEach - Returned value is null");
                    }
                    else
                    {
                        output.WriteLine($"Found {value.Name} = {value.Value}");
                    }
                }
            }
        }

        [Fact]
        public void CameraTest()
        {
            using (Camera device = new Camera("ASCOM.Simulator.Camera"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.True(device.Connected);

                CameraDeviceState deviceState = device.CameraDeviceState;

                Assert.True(deviceState.CameraState.HasValue);
                Assert.True(deviceState.CCDTemperature.HasValue);
                Assert.True(deviceState.CoolerPower.HasValue);
                Assert.True(deviceState.HeatSinkTemperature.HasValue);
                Assert.True(deviceState.ImageReady.HasValue);
                Assert.True(deviceState.IsPulseGuiding.HasValue);
                Assert.True(deviceState.PercentCompleted.HasValue);
                Assert.True(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [Fact]
        public void CoverCalibratorTest()
        {
            using (CoverCalibrator device = new CoverCalibrator("ASCOM.Simulator.CoverCalibrator"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.True(device.Connected);

                CoverCalibratorState deviceState = device.CoverCalibratorState;

                Assert.True(deviceState.Brightness.HasValue);
                Assert.True(deviceState.CalibratorReady.HasValue);
                Assert.True(deviceState.CalibratorState.HasValue);
                Assert.True(deviceState.CoverMoving.HasValue);
                Assert.True(deviceState.CoverState.HasValue);
                Assert.True(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [Fact]
        public void DomeTest()
        {
            using (Dome device = new Dome("ASCOM.Simulator.Dome"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.True(device.Connected);

                DomeState deviceState = device.DomeState;

                Assert.True(deviceState.Altitude.HasValue);
                Assert.True(deviceState.AtHome.HasValue);
                Assert.True(deviceState.AtPark.HasValue);
                Assert.True(deviceState.Azimuth.HasValue);
                Assert.True(deviceState.ShutterStatus.HasValue);
                Assert.True(deviceState.Slewing.HasValue);
                Assert.True(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }
        [Fact]
        public void FilterWheelTest()
        {
            using (FilterWheel device = new FilterWheel("ASCOM.Simulator.FilterWheel"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.True(device.Connected);

                FilterWheelState deviceState = device.FilterWheelState;

                Assert.True(deviceState.Position.HasValue);
                Assert.True(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }
        [Fact]
        public void FocuserTest()
        {
            using (Focuser device = new Focuser("ASCOM.Simulator.Focuser"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.True(device.Connected);

                FocuserState deviceState = device.FocuserState;

                Assert.True(deviceState.IsMoving.HasValue);
                Assert.True(deviceState.Position.HasValue);
                Assert.True(deviceState.Temperature.HasValue);
                Assert.True(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [Fact]
        public void ObservingConditionsTest()
        {
            using (ObservingConditions device = new ObservingConditions("ASCOM.Simulator.ObservingConditions"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.True(device.Connected);

                ObservingConditionsState deviceState = device.ObservingConditionsState;

                Assert.True(deviceState.CloudCover.HasValue);
                Assert.True(deviceState.DewPoint.HasValue);
                Assert.True(deviceState.Humidity.HasValue);
                Assert.True(deviceState.Pressure.HasValue);
                Assert.True(deviceState.RainRate.HasValue);
                Assert.True(deviceState.SkyBrightness.HasValue);
                Assert.True(deviceState.SkyQuality.HasValue);
                Assert.True(deviceState.SkyTemperature.HasValue);
                Assert.True(deviceState.StarFWHM.HasValue);
                Assert.True(deviceState.Temperature.HasValue);
                Assert.True(deviceState.WindDirection.HasValue);
                Assert.True(deviceState.WindGust.HasValue);
                Assert.True(deviceState.WindSpeed.HasValue);
                Assert.True(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [Fact]
        public void RotatorTest()
        {
            using (Rotator device = new Rotator("ASCOM.Simulator.Rotator"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.True(device.Connected);

                RotatorState deviceState = device.RotatorState;

                Assert.True(deviceState.IsMoving.HasValue);
                Assert.True(deviceState.MechanicalPosition.HasValue);
                Assert.True(deviceState.Position.HasValue);
                Assert.True(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [Fact]
        public void SafetyMonitorTest()
        {
            using (SafetyMonitor device = new SafetyMonitor("ASCOM.Simulator.SafetyMonitor"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.True(device.Connected);

                SafetyMonitorState deviceState = device.SafetyMonitorState;

                Assert.True(deviceState.IsSafe.HasValue);
                Assert.True(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [Fact]
        public void VideoTest()
        {
            using (Video device = new Video("ASCOM.Simulator.Video"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.True(device.Connected);

                VideoState deviceState = device.VideoState;

                Assert.True(deviceState.CameraState.HasValue);
                Assert.Equal(nameof(IVideoV2.CameraState), nameof(VideoState.CameraState));
                Assert.True(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }

        [Fact]
        public void TelescopeTest()
        {
            using (Telescope device = new Telescope("ASCOM.Simulator.Telescope"))
            {
                device.Connect();
                do
                {
                    System.Threading.Thread.Sleep(100);
                } while (device.Connecting);
                Assert.True(device.Connected);

                TelescopeState deviceState = device.TelescopeState;

                Assert.True(deviceState.Altitude.HasValue);
                Assert.True(deviceState.AtHome.HasValue);
                Assert.True(deviceState.AtPark.HasValue);
                Assert.True(deviceState.Azimuth.HasValue);
                Assert.True(deviceState.Declination.HasValue);
                Assert.True(deviceState.IsPulseGuiding.HasValue);
                Assert.True(deviceState.RightAscension.HasValue);
                Assert.True(deviceState.SideOfPier.HasValue);
                Assert.True(deviceState.SiderealTime.HasValue);
                Assert.True(deviceState.Slewing.HasValue);
                Assert.True(deviceState.Tracking.HasValue);
                Assert.True(deviceState.UTCDate.HasValue);
                Assert.True(deviceState.TimeStamp.HasValue);

                device.Disconnect();
            }
        }
    }
}
