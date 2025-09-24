using ASCOM.Utilities;
using System;
using System.Collections;
using Xunit;

namespace PlatformUnitTests
{
    public class UtilTests : IDisposable
    {
        private readonly Util util;

        public UtilTests()
        {
            util = new Util();
        }

        public void Dispose()
        {
            util.Dispose();
        }

        [Fact]
        public void WaitForMilliseconds_DoesNotThrow()
        {
            util.WaitForMilliseconds(10);
        }

        [Fact]
        public void DMSToDegrees_ValidInput_ReturnsExpected()
        {
            double result = util.DMSToDegrees("12:34:56");
            Assert.InRange(result, 12.0, 13.0);
        }

        [Fact]
        public void HMSToHours_ValidInput_ReturnsExpected()
        {
            double result = util.HMSToHours("12:34:56");
            Assert.InRange(result, 12.0, 13.0);
        }

        [Fact]
        public void HMSToDegrees_ValidInput_ReturnsExpected()
        {
            double result = util.HMSToDegrees("1:0:0");
            Assert.Equal(15.0, result, 2);
        }

        [Fact]
        public void DegreesToDMS_Default_ReturnsString()
        {
            string result = util.DegreesToDMS(12.3456);
            Assert.Contains("°", result);
        }

        [Fact]
        public void DegreesToDMS_Custom_ReturnsString()
        {
            string result = util.DegreesToDMS(12.3456, " deg ", " min ", " sec ", 2);
            Assert.Contains("deg", result);
        }

        [Fact]
        public void HoursToHMS_Default_ReturnsString()
        {
            string result = util.HoursToHMS(5.25);
            Assert.Contains(":", result);
        }

        [Fact]
        public void HoursToHMS_Custom_ReturnsString()
        {
            string result = util.HoursToHMS(5.25, "h ", "m ", "s ", 1);
            Assert.Contains("h", result);
        }

        [Fact]
        public void DegreesToHMS_Default_ReturnsString()
        {
            string result = util.DegreesToHMS(15.0);
            Assert.Contains(":", result);
        }

        [Fact]
        public void DegreesToHMS_Custom_ReturnsString()
        {
            string result = util.DegreesToHMS(15.0, "h ", "m ", "s ", 1);
            Assert.Contains("h", result);
        }

        [Fact]
        public void DegreesToDM_Default_ReturnsString()
        {
            string result = util.DegreesToDM(12.3456);
            Assert.Contains("°", result);
        }

        [Fact]
        public void DegreesToDM_Custom_ReturnsString()
        {
            string result = util.DegreesToDM(12.3456, " deg ", " min ", 2);
            Assert.Contains("deg", result);
        }

        [Fact]
        public void HoursToHM_Default_ReturnsString()
        {
            string result = util.HoursToHM(5.25);
            Assert.Contains(":", result);
        }

        [Fact]
        public void HoursToHM_Custom_ReturnsString()
        {
            string result = util.HoursToHM(5.25, "h ", "m ", 1);
            Assert.Contains("h", result);
        }

        [Fact]
        public void DegreesToHM_Default_ReturnsString()
        {
            string result = util.DegreesToHM(15.0);
            Assert.Contains(":", result);
        }

        [Fact]
        public void DegreesToHM_Custom_ReturnsString()
        {
            string result = util.DegreesToHM(15.0, "h ", "m ", 1);
            Assert.Contains("h", result);
        }

        [Fact]
        public void IsMinimumRequiredVersion_ReturnsTrueOrFalse()
        {
            bool result = util.IsMinimumRequiredVersion(1, 0);
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void SerialTraceFile_GetSet_Works()
        {
            string original = util.SerialTraceFile;
            util.SerialTraceFile = "test.txt";
            Assert.Equal("test.txt", util.SerialTraceFile);
            util.SerialTraceFile = original;
        }

        [Fact]
        public void SerialTrace_GetSet_Works()
        {
            bool original = util.SerialTrace;
            util.SerialTrace = true;
            Assert.True(util.SerialTrace);
            util.SerialTrace = false;
            Assert.False(util.SerialTrace);
            util.SerialTrace = original;
        }

        [Fact]
        public void TimeZoneName_ReturnsString()
        {
            string name = util.TimeZoneName;
            Assert.False(string.IsNullOrEmpty(name));
        }

        [Fact]
        public void TimeZoneOffset_ReturnsDouble()
        {
            double offset = util.TimeZoneOffset;
            Assert.InRange(offset, -15, 15);
        }

        [Fact]
        public void UTCDate_ReturnsDateTime()
        {
            DateTime dt = util.UTCDate;
            Assert.True(dt.Kind == DateTimeKind.Utc || dt.Kind == DateTimeKind.Unspecified);
        }

        [Fact]
        public void JulianDate_ReturnsDouble()
        {
            double jd = util.JulianDate;
            Assert.True(jd > 2000000);
        }

        [Fact]
        public void DateLocalToJulian_ReturnsDouble()
        {
            double jd = util.DateLocalToJulian(DateTime.Now);
            Assert.True(jd > 2000000);
        }

        [Fact]
        public void DateJulianToLocal_ReturnsDateTime()
        {
            DateTime dt = util.DateJulianToLocal(2451545.0);
            Assert.IsType<DateTime>(dt);
        }

        [Fact]
        public void DateUTCToJulian_ReturnsDouble()
        {
            double jd = util.DateUTCToJulian(DateTime.UtcNow);
            Assert.True(jd > 2000000);
        }

        [Fact]
        public void DateJulianToUTC_ReturnsDateTime()
        {
            DateTime dt = util.DateJulianToUTC(2451545.0);
            Assert.IsType<DateTime>(dt);
        }

        [Fact]
        public void DateUTCToLocal_ReturnsDateTime()
        {
            DateTime dt = util.DateUTCToLocal(DateTime.UtcNow);
            Assert.IsType<DateTime>(dt);
        }

        [Fact]
        public void DateLocalToUTC_ReturnsDateTime()
        {
            DateTime dt = util.DateLocalToUTC(DateTime.Now);
            Assert.IsType<DateTime>(dt);
        }

        [Fact]
        public void ToStringCollection_ReturnsArrayList()
        {
            var arr = util.ToStringCollection(new[] { "a", "b" });
            Assert.Equal(2, arr.Count);
        }

        [Fact]
        public void ToIntegerCollection_ReturnsArrayList()
        {
            var arr = util.ToIntegerCollection(new[] { 1, 2 });
            Assert.Equal(2, arr.Count);
        }

        [Fact]
        public void ConvertUnits_SpeedConversion_Works()
        {
            double mph = util.ConvertUnits(1.0, Units.metresPerSecond, Units.milesPerHour);
            Assert.InRange(mph, 2.0, 3.0);
        }

        [Fact]
        public void Humidity2DewPoint_ValidInput_ReturnsDouble()
        {
            double dew = util.Humidity2DewPoint(50.0, 20.0);
            Assert.InRange(dew, -20.0, 20.0);
        }

        [Fact]
        public void DewPoint2Humidity_ValidInput_ReturnsDouble()
        {
            double rh = util.DewPoint2Humidity(10.0, 20.0);
            Assert.InRange(rh, 0.0, 100.0);
        }

        [Fact]
        public void ConvertPressure_ValidInput_ReturnsDouble()
        {
            double p = util.ConvertPressure(1013.25, 0.0, 100.0);
            Assert.True(p < 1013.25);
        }

        [Fact]
        public void ArrayToVariantArray_OneDimIntArray_ReturnsObjectArray()
        {
            int[] arr = new int[] { 1, 2, 3 };
            var result = util.ArrayToVariantArray(arr);
            Assert.IsType<object[]>(result);
        }

        [Fact]
        public void MajorVersion_ReturnsInt()
        {
            int v = util.MajorVersion;
            Assert.True(v > 0);
        }

        [Fact]
        public void MinorVersion_ReturnsInt()
        {
            int v = util.MinorVersion;
            Assert.True(v >= 0);
        }

        [Fact]
        public void ServicePack_ReturnsInt()
        {
            int v = util.ServicePack;
            Assert.True(v >= 0);
        }

        [Fact]
        public void BuildNumber_ReturnsInt()
        {
            int v = util.BuildNumber;
            Assert.True(v >= 0);
        }
    }
}