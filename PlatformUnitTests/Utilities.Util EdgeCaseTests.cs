using ASCOM.Utilities;
using System;
using Xunit;

namespace PlatformUnitTests
{
    public class UtilEdgeCaseTests : IDisposable
    {
        private readonly Util util;

        public UtilEdgeCaseTests()
        {
            util = new Util();
        }

        public void Dispose()
        {
            util.Dispose();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void WaitForMilliseconds_EdgeCases(int ms)
        {
            util.WaitForMilliseconds(ms);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("12")]
        [InlineData("12:34")]
        [InlineData("-12:34:56")]
        [InlineData("abc")]
        [InlineData("12:34:56:78")]
        [InlineData("12 34 56")]
        public void DMSToDegrees_EdgeCases(string input)
        {
            if (input == null)
                Assert.Throws<ArgumentNullException>(() => util.DMSToDegrees(input));
            else if (input == "abc")
                Assert.ThrowsAny<Exception>(() => util.DMSToDegrees(input));
            else
                util.DMSToDegrees(input);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("1:30")]
        [InlineData("-1:30:30")]
        [InlineData("abc")]
        [InlineData("1:2:3:4")]
        public void HMSToHours_EdgeCases(string input)
        {
            if (input == null)
                Assert.Throws<ArgumentNullException>(() => util.HMSToHours(input));
            else if (input == "abc")
                Assert.ThrowsAny<Exception>(() => util.HMSToHours(input));
            else
                util.HMSToHours(input);
        }

        [Theory]
        [InlineData(-360.0)]
        [InlineData(0.0)]
        [InlineData(400.0)]
        [InlineData(12.999999)]
        public void DegreesToDMS_NumericEdgeCases(double degrees)
        {
            var result = util.DegreesToDMS(degrees);
            Assert.NotNull(result);
        }

        [Fact]
        public void DegreesToDMS_CustomDelimiters_EmptyString()
        {
            var result = util.DegreesToDMS(12.3456, "", "", "", 0);
            Assert.NotNull(result);
        }

        [Fact]
        public void DegreesToDMS_CustomDelimiters_SpecialChars()
        {
            var result = util.DegreesToDMS(12.3456, "@", "#", "$", 3);
            Assert.Contains("@", result);
        }

        [Fact]
        public void DegreesToDMS_LargeDecimalDigits()
        {
            var result = util.DegreesToDMS(12.3456, "°", "'", "\"", 10);
            Assert.Contains(".", result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        [InlineData(double.MaxValue)]
        public void ConvertUnits_EdgeCases(double value)
        {
            // Valid conversion
            util.ConvertUnits(value, Units.metresPerSecond, Units.milesPerHour);

            // Invalid conversion: speed to temperature
            Assert.ThrowsAny<Exception>(() => util.ConvertUnits(value, Units.metresPerSecond, Units.degreesCelsius));
        }

        [Theory]
        [InlineData(0, 20)]
        [InlineData(100, 20)]
        [InlineData(-1, 20)]
        [InlineData(101, 20)]
        [InlineData(50, -300)]
        [InlineData(50, 101)]
        public void Humidity2DewPoint_EdgeCases(double rh, double temp)
        {
            if (rh < 0 || rh > 100 || temp < -273.15 || temp > 100)
                Assert.ThrowsAny<Exception>(() => util.Humidity2DewPoint(rh, temp));
            else
                util.Humidity2DewPoint(rh, temp);
        }

        [Theory]
        [InlineData(-300, 20)]
        [InlineData(101, 20)]
        [InlineData(10, -300)]
        [InlineData(10, 101)]
        [InlineData(30, 20)] // DewPoint > AmbientTemperature
        public void DewPoint2Humidity_EdgeCases(double dew, double temp)
        {
            if (dew < -273.15 || dew > 100 || temp < -273.15 || temp > 100)
                Assert.ThrowsAny<Exception>(() => util.DewPoint2Humidity(dew, temp));
            else
                util.DewPoint2Humidity(dew, temp);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(-100, -100, -100)]
        [InlineData(1013.25, 0, 10000)]
        [InlineData(1013.25, -100, 100)]
        public void ConvertPressure_EdgeCases(double pressure, double fromAlt, double toAlt)
        {
            util.ConvertPressure(pressure, fromAlt, toAlt);
        }

        [Fact]
        public void ArrayToVariantArray_NullInput()
        {
            Assert.ThrowsAny<Exception>(() => util.ArrayToVariantArray(null));
        }

        [Fact]
        public void ArrayToVariantArray_NotArray()
        {
            Assert.ThrowsAny<Exception>(() => util.ArrayToVariantArray(123));
        }

        [Fact]
        public void ArrayToVariantArray_UnsupportedType()
        {
            var arr = new[] { new Util() };
            Assert.ThrowsAny<Exception>(() => util.ArrayToVariantArray(arr));
        }

        [Fact]
        public void ArrayToVariantArray_EmptyArray()
        {
            var arr = new int[0];
            var result = util.ArrayToVariantArray(arr);
            Assert.IsType<object[]>(result);
        }

        [Fact]
        public void ArrayToVariantArray_RankGreaterThan5()
        {
            var arr = Array.CreateInstance(typeof(int), new int[] { 1, 1, 1, 1, 1, 1 });
            Assert.ThrowsAny<Exception>(() => util.ArrayToVariantArray(arr));
        }

        [Fact]
        public void DateLocalToJulian_MinMaxDate()
        {
            util.DateLocalToJulian(DateTime.MinValue);
            util.DateLocalToJulian(DateTime.MaxValue);
        }

        [Fact]
        public void DateUTCToJulian_MinMaxDate()
        {
            util.DateUTCToJulian(DateTime.MinValue);
            util.DateUTCToJulian(DateTime.MaxValue);
        }

        [Fact]
        public void DateJulianToLocal_LeapYear()
        {
            // Julian date for 2000-02-29
            var dt = util.DateJulianToLocal(2451604.5);
            Assert.Equal(2000, dt.Year);
            Assert.Equal(2, dt.Month);
            Assert.Equal(29, dt.Day);
        }

        [Fact]
        public void ToStringCollection_EmptyArray()
        {
            var arr = util.ToStringCollection(new string[0]);
            Assert.Empty(arr);
        }

        [Fact]
        public void ToIntegerCollection_EmptyArray()
        {
            var arr = util.ToIntegerCollection(new int[0]);
            Assert.Empty(arr);
        }

        [Fact]
        public void SerialTraceFile_SetNullOrLongString()
        {
            util.SerialTraceFile = null;
            util.SerialTraceFile = new string('a', 1000);
        }

        [Fact]
        public void SerialTrace_ToggleRepeatedly()
        {
            for (int i = 0; i < 10; i++)
            {
                util.SerialTrace = !util.SerialTrace;
            }
        }
    }
}