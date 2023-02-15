using ASCOM.Astrometry.AstroUtils;
using ASCOM.Astrometry.Transform;
using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace TeleSimTester
{
    internal static class Program
    {

        const double ARCSEC_TO_DEGREES = 360.0 / (24.0 * 60.0 * 60.0);

        static TraceLogger TL;

        static AstroUtils astroUtils;
        static Util util;
        static Telescope telStatic;
        enum Outcome
        {
            OK,
            FAILED,
            ERROR,
            INFO
        }
        static void Main(string[] args)
        {
            TL = new TraceLogger("TeleSimTester");
            TL.Enabled = true;
            try
            {
                util = new Util();
                astroUtils = new AstroUtils();
                using (Telescope tel = new Telescope("ASCOM.Simulator.Telescope"))
                // using (Telescope tel = new Telescope("ASCOM.AlpacaDynamic2.Telescope"))
                {
                    try
                    {
                        telStatic = tel;

                        tel.Connected = true;
                        LogMessage("Description", Outcome.INFO, tel.Description);
                        LogMessage("Description", Outcome.INFO, tel.AlignmentMode.ToString());

                        //SlewCoordinateTests(tel);

                        //SlewTests(tel);

                        //MoveAxisTests(tel);

                        RaDecOffsetRateTests(tel);

                        LogMessage("All tests", Outcome.INFO, "COMPLETED");
                        Console.ReadLine();
                    }

                    catch (Exception ex)
                    {
                        LogMessage("Using", Outcome.ERROR, ex.ToString());
                    }
                    finally
                    {
                        tel.Connected = false;
                    }
                }
            }
            catch (Exception ex1)
            {
                LogMessage("Main", Outcome.ERROR, ex1.ToString());
            }

        }

        private static void SlewCoordinateTests(Telescope tel)
        {
            ValidateCoordinates(tel, -3.0, 85.0);
            ValidateCoordinates(tel, -9.0, 85.0);
            ValidateCoordinates(tel, +3.0, 85.0);
            ValidateCoordinates(tel, +9.0, 85.0);

            ValidateCoordinates(tel, -3.0, 60.0);
            ValidateCoordinates(tel, -9.0, 60.0);
            ValidateCoordinates(tel, +3.0, 60.0);
            ValidateCoordinates(tel, +9.0, 60.0);

            ValidateCoordinates(tel, -3.0, 30.0);
            ValidateCoordinates(tel, -9.0, 30.0);
            ValidateCoordinates(tel, +3.0, 30.0);
            ValidateCoordinates(tel, +9.0, 30.0);

            ValidateCoordinates(tel, -3.0, 10.0);
            ValidateCoordinates(tel, -9.0, 10.0);
            ValidateCoordinates(tel, +3.0, 10.0);
            ValidateCoordinates(tel, +9.0, 10.0);
        }

        private static void ValidateCoordinates(Telescope tel, double targetHa, double targetDec)
        {
            Transform transform = new Transform();
            transform.SiteTemperature = 10.0;
            transform.SitePressure = 0;
            transform.SiteElevation = tel.SiteElevation;
            transform.SiteLatitude = tel.SiteLatitude;
            transform.SiteLongitude = tel.SiteLongitude;
            transform.Refraction = false;
            //transform.JulianDateUTC = util.JulianDate;

            tel.Tracking = true;
            SlewToHaDec(targetHa, targetDec);

            double ra = tel.RightAscension;
            transform.SetTopocentric(ra, targetDec);

            double declination = tel.Declination;
            double azimuth = tel.Azimuth;
            double elevation = tel.Altitude;

            double tra = transform.RATopocentric;
            double tdeclination = transform.DECTopocentric;
            double tazimuth = transform.AzimuthTopocentric;
            double televation = transform.ElevationTopocentric;

            LogMessage("ValidateCoordinates", Outcome.INFO, $"Julian date: {(transform.JulianDateUTC == 0.0 ? "0.0" : util.DateJulianToLocal(transform.JulianDateUTC).ToLongTimeString())}");
            LogMessage("ValidateCoordinates", Outcome.INFO, $"Telescope  - RA: {ra.ToHMS()}, Declination: {declination.ToDMS()}, Azimuth: {azimuth.ToDMS()}, Elevation: {elevation.ToDMS()}");
            LogMessage("ValidateCoordinates", Outcome.INFO, $"Transform  - RA: {tra.ToHMS()}, Declination: {tdeclination.ToDMS()}, Azimuth: {tazimuth.ToDMS()}, Elevation: {televation.ToDMS()}");
            LogMessage("ValidateCoordinates", Outcome.INFO, $"Difference - RA: {(tra - ra).ToHMS()}, Declination: {(tdeclination - declination).ToDMS()}, Azimuth: {(tazimuth - azimuth).ToDMS()}, Elevation: {(televation - elevation).ToDMS()}");

            TestDecDifference("ValidateCoordinates", "Azimuth", azimuth, tazimuth, util.DMSToDegrees("00:00:10"));
            TestDecDifference("ValidateCoordinates", "Elevation", elevation, televation, util.DMSToDegrees("00:00:10"));

            WaitFor(2.00);

            LogBlankLine();


        }

        private static void RaDecOffsetRateTests(Telescope tel)
        {
            tel.Tracking = true;
            LogMessage("SetRaDecOffsetRates", Outcome.INFO, "HA = +9.0");
            double OFFSET_HA = +9.0;
            const double OFFSET_DEC = 40.0;
            // RA offsets only
            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(15.0, 0.0);
            // Dec offsets only
            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(0.0, 15.0);

            LogMessage("SetRaDecOffsetRates", Outcome.INFO, "HA = +3.0");
            OFFSET_HA = +3.0;
            // RA offsets only
            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(15.0, 0.0);
            // Dec offsets only
            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(0.0, 15.0);

            LogMessage("SetRaDecOffsetRates", Outcome.INFO, "HA = -9.0");
            OFFSET_HA = -9.0;
            // RA offsets only
            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(15.0, 0.0);
            // Dec offsets only
            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(0.0, 15.0);

            LogMessage("SetRaDecOffsetRates", Outcome.INFO, "HA = -3.0");
            OFFSET_HA = -3.0;
            // RA offsets only
            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(15.0, 0.0);
            // Dec offsets only
            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(0.0, 15.0);

#if false
            // No offsets
            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(0.0, 0.0);

            // RA offsets only
            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(15.0, 0.0);

            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(-15.0, 0.0);

            // Dec offsets only
            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(0.0, 15.0);

            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(0.0, -15.0);

            // RA and Dec offsets in combination
            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(15.0, 15.0);

            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(-15.0, -15.0);

            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(5.0, 5.0);

            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(-5.0, -5.0);

            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(1.0, 1.0);

            SlewToHaDec(OFFSET_HA, OFFSET_DEC);
            SetRaDecOffsetRates(-1.0, -1.0);
#endif
            LogMessage("SetRaDecOffsetRates", Outcome.INFO, "FINISHED");
        }

        private static void MoveAxisTests(Telescope tel)
        {
            // Apply appropriate test depending on alignment mode
            if (tel.AlignmentMode == AlignmentModes.algAltAz)
            {
                tel.Tracking = false;
                SlewToAltAz(25.0, 25.0);
                LogBlankLine();

                MoveAxesAltAz(3.0, 3.0);
                MoveAxesAltAz(-3.0, -3.0);

                MoveAxesAltAz(2.0, 2.0);
                MoveAxesAltAz(-2.0, -2.0);

                MoveAxesAltAz(1.0, 1.0);
                MoveAxesAltAz(-1.0, -1.0);

                MoveAxesAltAz(0.0, 0.0);
                LogMessage("MoveAxesAltAz", Outcome.INFO, "FINISHED");
            }
            else
            {
                tel.Tracking = true;

                SlewToHaDec(3.0, 30.0);
                MoveAxesRaDec(3.0, 0.0);

                SlewToHaDec(3.0, 30.0);
                MoveAxesRaDec(-3.0, 0.0);

                SlewToHaDec(3.0, 30.0);
                MoveAxesRaDec(0.0, 3.0);

                SlewToHaDec(3.0, 30.0);
                MoveAxesRaDec(0.0, -3.0);

                SlewToHaDec(3.0, 30.0);
                MoveAxesRaDec(3.0, 3.0);

                SlewToHaDec(3.0, 30.0);
                MoveAxesRaDec(-3.0, -3.0);

                SlewToHaDec(3.0, 30.0);
                MoveAxesRaDec(2.0, 2.0);

                SlewToHaDec(3.0, 30.0);
                MoveAxesRaDec(-2.0, -2.0);

                SlewToHaDec(3.0, 30.0);
                MoveAxesRaDec(1.0, 1.0);

                SlewToHaDec(3.0, 30.0);
                MoveAxesRaDec(-1.0, -1.0);

                SlewToHaDec(3.0, 30.0);
                MoveAxesRaDec(0.0, 0.0);
                LogMessage("MoveAxesRaDec", Outcome.INFO, "FINISHED");
            }
        }

        private static void SlewTests(Telescope tel)
        {
            tel.Tracking = true;
            SlewToHaDec(-3.0, 50.0, true);
            WaitFor(5);
            SlewToHaDec(-2.0, 40.0, true);
            WaitFor(5);
            SlewToHaDec(-4.0, 80.0, true);
            WaitFor(5);
            SlewToHaDec(3.0, 50.0, true);
            WaitFor(5);
            SlewToHaDec(2.0, 40.0, true);
            WaitFor(5);
            SlewToHaDec(4.0, 80.0, true);
            LogMessage("SlewToRaDec", Outcome.INFO, "FINISHED");

            tel.Tracking = false;
            SlewToAltAz(40.0, 40.0);
            SlewToAltAz(135.0, 50.0);
            SlewToAltAz(225.0, 60.0);
            SlewToAltAz(315.0, 70.0);
            LogMessage("SlewToAltAz", Outcome.INFO, "FINISHED");
        }

        #region Slew support and logging

        internal static void SetRaDecOffsetRates(double expectedRaRate, double expectedDeclinationRate)
        {
            const double DURATION = 10.0; // Seconds

            LogMessage("SetRaDecOffsetRates", Outcome.INFO, $"Testing Primary rate: {expectedRaRate}, Secondary rate: {expectedDeclinationRate}, SideofPier: {telStatic.SideOfPier}");

            double priStart = telStatic.RightAscension;
            double secStart = telStatic.Declination;

            telStatic.RightAscensionRate = expectedRaRate;
            telStatic.DeclinationRate = expectedDeclinationRate;

            WaitFor(DURATION);

            double priEnd = telStatic.RightAscension;
            double secEnd = telStatic.Declination;

            // Restore previous state
            telStatic.RightAscensionRate = 0.0;
            telStatic.DeclinationRate = 0.0;

            LogMessage("SetRaDecOffsetRates", Outcome.INFO, $"Start      - : {priStart.ToHMS()}, {secStart.ToDMS()}");
            LogMessage("SetRaDecOffsetRates", Outcome.INFO, $"Finish     - : {priEnd.ToHMS()}, {secEnd.ToDMS()}");
            LogMessage("SetRaDecOffsetRates", Outcome.INFO, $"Difference - : {(priEnd - priStart).ToHMS()}, {(secEnd - secStart).ToDMS()}, {priEnd - priStart:N10}, {secEnd - secStart:N10}");

            // Condition results
            double actualPriRate = (priEnd - priStart) / DURATION; // Calculate offset rate in RA hours per SI second
            actualPriRate = actualPriRate * 60.0 * 60.0; // Convert rate in RA hours per SI second to RA seconds per SI second
            actualPriRate *= TelescopeHardware.SIDEREAL_SECONDS_TO_SI_SECONDS; // Convert rate in RA seconds per SI second to RA seconds per sidereal second (which is slightly less than RA seconds per SI second)

            double actualSecRate = (secEnd - secStart) / DURATION * 60.0 * 60.0;

            LogMessage("SetRaDecOffsetRates", Outcome.INFO, $"Actual primary rate: {actualPriRate}, Expected rate: {expectedRaRate}, Ratio: {actualPriRate / expectedRaRate}, Actual secondary rate: {actualSecRate}, Expected rate: {expectedDeclinationRate}, Ratio: {actualSecRate / expectedDeclinationRate}");
            TestDouble("SetRaDecOffsetRates", "Primary Axis", actualPriRate, expectedRaRate);
            TestDouble("SetRaDecOffsetRates", "Secondary Axis", actualSecRate, expectedDeclinationRate);
            LogBlankLine();
        }

        internal static void MoveAxesAltAz(double expectedPrimaryRate, double expectedSecondaryRate)
        {
            const double DURATION = 10.0; // Seconds

            LogMessage("MoveAxesAltAz", Outcome.INFO, $"Moving primary axis at: {expectedPrimaryRate}, Moving secondary axis at: {expectedSecondaryRate}");

            double priStart = telStatic.Azimuth;
            double secStart = telStatic.Altitude;

            telStatic.MoveAxis(TelescopeAxes.axisPrimary, expectedPrimaryRate);
            telStatic.MoveAxis(TelescopeAxes.axisSecondary, expectedSecondaryRate);

            WaitFor(DURATION);

            double priEnd = telStatic.Azimuth;
            double secEnd = telStatic.Altitude;

            // Restore previous state
            telStatic.MoveAxis(TelescopeAxes.axisPrimary, 0.0);
            telStatic.MoveAxis(TelescopeAxes.axisSecondary, 0.0);

            double actualPriRate = (priEnd - priStart) / DURATION;
            double actualSecRate = (secEnd - secStart) / DURATION;

            LogMessage("MoveAxesAltAz", Outcome.INFO, $"Actual primary rate: {actualPriRate}, Expected rate: {expectedPrimaryRate}, Ratio: {actualPriRate / expectedPrimaryRate}, Actual secondary rate: {actualSecRate}, Expected rate: {expectedSecondaryRate}, Ratio: {actualSecRate / expectedSecondaryRate}");

            TestDouble("MoveAxesAltAz", "Primary Axis", actualPriRate, expectedPrimaryRate);
            TestDouble("MoveAxesAltAz", "Secondary Axis", actualSecRate, expectedSecondaryRate);
            LogBlankLine();
        }

        internal static void MoveAxesRaDec(double expectedPrimaryRate, double expectedSecondaryRate)
        {
            const double DURATION = 10.0; // Seconds
            double siderealTime = telStatic.SiderealTime;
            double rightAscension = telStatic.RightAscension;
            double declination = telStatic.Declination;

            double priStart = (siderealTime - rightAscension) * 15.0;
            double secStart = declination;

            LogMessage("MoveAxesRaDec", Outcome.INFO, $"Moving primary axis at: {expectedPrimaryRate}, Moving secondary axis at: {expectedSecondaryRate}");
            LogMessage("MoveAxesRaDec", Outcome.INFO, $"Initial RA: : {rightAscension.ToHMS()}, Initial declination: {declination.ToDMS()}, LST: {siderealTime}");

            telStatic.MoveAxis(TelescopeAxes.axisPrimary, expectedPrimaryRate);
            telStatic.MoveAxis(TelescopeAxes.axisSecondary, expectedSecondaryRate);

            WaitFor(DURATION);

            rightAscension = telStatic.RightAscension;
            declination = telStatic.Declination;
            siderealTime = telStatic.SiderealTime;

            double priEnd = (siderealTime - rightAscension) * 15.0;
            double secEnd = declination;

            LogMessage("MoveAxesRaDec", Outcome.INFO, $"Final RA: : {rightAscension.ToHMS()}, Final declination: {declination.ToDMS()}, LST: {siderealTime}");

            // Restore previous state
            telStatic.MoveAxis(TelescopeAxes.axisPrimary, 0.0);
            telStatic.MoveAxis(TelescopeAxes.axisSecondary, 0.0);

            // Condition results
            //if (priEnd < priStart) priEnd += 360.0;

            // 
            double actualPriRate = (priEnd - priStart) / DURATION; // Subtraction has to be this way round because positive axis movement (EAST => WEST) leads to smaller RA values
            double actualSecRate = (secEnd - secStart) / DURATION; // Mount has to be on the EAST side for this equation to hold
            LogMessage("MoveAxesRaDec", Outcome.INFO, $"Primary start: {priStart}, Primary end: {priEnd}, Secondary start: {secStart}, Secondary end: {secEnd}");

            LogMessage("MoveAxesRaDec", Outcome.INFO, $"Actual primary rate: {actualPriRate}, Expected rate: {expectedPrimaryRate}, Ratio: {actualPriRate / expectedPrimaryRate}, Actual secondary rate: {actualSecRate}, Expected rate: {expectedSecondaryRate}, Ratio: {actualSecRate / expectedSecondaryRate}");
            TestDouble("MoveAxesRaDec", "Primary Axis", actualPriRate, expectedPrimaryRate);
            TestDouble("MoveAxesRaDec", "Secondary Axis", actualSecRate, expectedSecondaryRate);
            LogBlankLine();

        }

        /// <summary>
        /// Wait for given number of seconds
        /// </summary>
        /// <param name="interval"></param>
        static void WaitFor(double interval)
        {
            Stopwatch sw = Stopwatch.StartNew();

            LogMessage("WaitFor", Outcome.INFO, $"Waiting for {interval} seconds...");
            do
            {
                Thread.Sleep(100);
            } while (sw.Elapsed.TotalSeconds < interval);
        }

        static string ToHMS(this double rightascension)
        {
            return util.HoursToHMS(rightascension, ":", ":", "", 3);
        }

        static string ToDMS(this double declination)
        {
            return util.DegreesToDMS(declination, ":", ":", "", 3);
        }

        static void SlewToHaDec(double ha, double declination, bool report = false)
        {
            double ra = astroUtils.ConditionRA(telStatic.SiderealTime - ha);

            LogMessage("SlewToHaDec", Outcome.INFO, $"Slewing to RA: {ra.ToHMS()}, Dec: {declination.ToDMS()}");
            telStatic.SlewToCoordinatesAsync(ra, declination);
            WaitForSlew();
            LogMessage("SlewToHaDec", Outcome.INFO, $"Slewed to RA:  {telStatic.RightAscension.ToHMS()}, Dec: {telStatic.Declination.ToDMS()}");
            if (report) TestHaDifference("SlewToHaDec", "RA", telStatic.RightAscension, ra, 1.0 / (60.0 * 60.0));
            if (report) TestHaDifference("SlewToHaDec", "Declination", telStatic.Declination, declination, 1.0 / (60.0 * 60.0));
            LogBlankLine();
        }

        static void SlewToRaDec(double ra, double declination)
        {
            LogMessage("SlewToRaDec", Outcome.INFO, $"Slewing to RA: {ra.ToHMS()}, Dec: {declination.ToDMS()}");
            telStatic.SlewToCoordinatesAsync(ra, declination);
            WaitForSlew();
            LogMessage("SlewToRaDec", Outcome.INFO, $"Slewed to RA:  {telStatic.RightAscension.ToHMS()}, Dec: {telStatic.Declination.ToDMS()}");
            TestDouble("SlewToRaDec", "RA", telStatic.RightAscension, ra);
            TestDouble("SlewToRaDec", "Declination", telStatic.Declination, declination);
            LogBlankLine();
        }

        static void TestDouble(string method, string name, double actualValue, double expectedValue, double tolerance = 0.0)
        {
            // Tolerance 0 = 2%
            const double TOLERANCE = 0.05; // 2%

            if (tolerance == 0.0)
            {
                tolerance = TOLERANCE;
            }

            if (expectedValue == 0.0)
            {
                if (Math.Abs(actualValue - expectedValue) <= tolerance)
                {
                    LogMessage(method, Outcome.OK, $"{name} is within expected tolerance. Expected: {expectedValue}, Actual: {actualValue} = {Math.Abs((actualValue - expectedValue) * 100.0 / tolerance):N2}%, Tolerance:{tolerance * 100:N2}.");
                }
                else
                {
                    LogMessage(method, Outcome.FAILED, $"{name} is outside the expected tolerance. Expected: {expectedValue}, Actual: {actualValue} = {Math.Abs((actualValue - expectedValue) * 100.0 / tolerance):N2}%, Tolerance:{tolerance * 100:N2}.");
                }
            }
            else
            {
                if (Math.Abs(Math.Abs(actualValue - expectedValue) / expectedValue) <= tolerance)
                {
                    LogMessage(method, Outcome.OK, $"{name} is within expected tolerance. Expected: {expectedValue}, Actual: {actualValue} = {Math.Abs((actualValue - expectedValue) * 100.0 / expectedValue):N2}%, Tolerance:{tolerance * 100:N2}.");
                }
                else
                {
                    LogMessage(method, Outcome.FAILED, $"{name} is outside the expected tolerance. Expected: {expectedValue}, Actual: {actualValue} = {Math.Abs((actualValue - expectedValue) * 100.0 / expectedValue):N2}%, Tolerance:{tolerance * 100:N2}.");
                }
            }
        }

        static void TestHaDifference(string method, string name, double actualValue, double expectedValue, double difference)
        {
            if (Math.Abs(actualValue - expectedValue) <= difference)
            {
                LogMessage(method, Outcome.OK, $"{name} is within expected tolerance. Expected: {expectedValue.ToHMS()}, Actual: {actualValue.ToHMS()}, difference: {Math.Abs(actualValue - expectedValue).ToHMS()}, tolerance: {difference.ToHMS()}.");
            }
            else
            {
                LogMessage(method, Outcome.FAILED, $"{name} is outside the expected tolerance. Expected: {expectedValue.ToHMS()}, Actual: {actualValue.ToHMS()}, difference: {Math.Abs(actualValue - expectedValue).ToHMS()}, tolerance: {difference.ToHMS()}.");
            }
        }

        static void TestDecDifference(string method, string name, double actualValue, double expectedValue, double difference)
        {
            if (Math.Abs(actualValue - expectedValue) <= difference)
            {
                LogMessage(method, Outcome.OK, $"{name} is within expected tolerance. Expected: {expectedValue.ToDMS()}, Actual: {actualValue.ToDMS()}, difference: {Math.Abs(actualValue - expectedValue).ToDMS()}, tolerance: {difference.ToDMS()}.");
            }
            else
            {
                LogMessage(method, Outcome.FAILED, $"{name} is outside the expected tolerance. Expected: {expectedValue.ToDMS()}, Actual: {actualValue.ToDMS()}, difference: {Math.Abs(actualValue - expectedValue).ToDMS()}, tolerance: {difference.ToDMS()}.");
            }
        }

        static void LogBlankLine()
        {
            Console.WriteLine($" ");
            TL.LogMessage("", "");
        }

        static void SlewToAltAz(double azimuth, double altitude)
        {
            LogMessage("SlewToAltAz", Outcome.INFO, $"Slewing to Azimuth: {azimuth.ToDMS()}, Altitude: {altitude.ToDMS()}");
            telStatic.SlewToAltAzAsync(azimuth, altitude);
            WaitForSlew();

            LogMessage("SlewToAltAz", Outcome.INFO, $"Slewed to Azimuth:  {telStatic.Azimuth.ToDMS()}, Altitude: {telStatic.Altitude.ToDMS()}");
            TestDouble("SlewToAltAz", "Azimuth", telStatic.Azimuth, azimuth);
            TestDouble("SlewToAltAz", "Altitude", telStatic.Altitude, altitude);
            LogBlankLine();
        }

        static void WaitForSlew()
        {
            do
            {
                //LogMessage("WaitForSlew", Outcome.INFO, $"RA: {util.HoursToHMS(telStatic.RightAscension, ":", ":", "", 3)}, Dec: {util.DegreesToDMS(telStatic.Declination, ":", ":", "", 3)}");
                Thread.Sleep(500);
            } while (telStatic.Slewing);

            //LogMessage("WaitForSlew", Outcome.INFO, $"RA: {util.HoursToHMS(telStatic.RightAscension, ":", ":", "", 3)}, Dec: {util.DegreesToDMS(telStatic.Declination, ":", ":", "", 3)}");
        }

        static void LogMessage(string test, Outcome outcome, string message)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} {test,-25}{outcome,-8}{message}");
            TL.LogMessageCrLf(test, $"{outcome,-8}{message}");
        }

        #endregion

        #region AxisCalcTests
        internal static Vector ConvertRaDecToAxes(Vector raDec, bool preserveSop = false)
        {
            LogMessage("ConvertRaDecToAxes", Outcome.INFO, $"Received RA: {raDec.X.ToHMS()}, Declination: {raDec.Y.ToDMS()}");

            Vector axes = new Vector();
            PierSide sop = TelescopeHardware.SideOfPier;
            axes.X = (TelescopeHardware.SiderealTime - raDec.X) * 15.0 / TelescopeHardware.SIDEREAL_SECONDS_TO_SI_SECONDS;
            LogMessage("ConvertRaDecToAxes 1", Outcome.INFO, $"Axis X: {axes.X.ToDMS()} ({axes.X})");

            //axes.X = RangeAzm(axes.X);
            LogMessage("ConvertRaDecToAxes 2", Outcome.INFO, $"Axis X: {axes.X.ToDMS()} ({axes.X})");

            axes.Y = (TelescopeHardware.Latitude >= 0) ? raDec.Y : -raDec.Y;

            if (axes.X > 180.0 || axes.X < 0)
            {
                // adjust the targets to be through the pole
                axes.X += 180;
                axes.Y = 180 - axes.Y;
            }
            LogMessage("ConvertRaDecToAxes 3", Outcome.INFO, $"Axis X: {axes.X.ToDMS()} ({axes.X}) {(150.0 - axes.X).ToDMS()}");

            axes = RangeAxes(axes);
            LogMessage("ConvertRaDecToAxes 4", Outcome.INFO, $"Axis X: {axes.X.ToDMS()} ({axes.X}) {(150.0 - axes.X).ToDMS()}");
            LogMessage("", Outcome.INFO, "");
            return axes;
        }

        internal static Vector ConvertAxesToRaDec(Vector axes)
        {
            LogMessage("ConvertAxesToRaDec", Outcome.INFO, $"Received Primary axis: {axes.X.ToDMS()}, Secondary axis: {axes.Y.ToDMS()}");

            Vector raDec = new Vector();
            // undo through the pole
            if (axes.Y > 90)
            {
                axes.X += 180.0;
                LogMessage("ConvertAxesToRaDec 0a", Outcome.INFO, $"Primary axis: {axes.X.ToDMS()} ({axes.X}), Secondary axis: {axes.Y.ToDMS()} ({axes.Y})");
                //axes.X = RangeHaDegrees(axes.X);
                axes.X = astroUtils.Range(axes.X, -180.0, false, 180.0, true);
                LogMessage("ConvertAxesToRaDec 0b", Outcome.INFO, $"Primary axis: {axes.X.ToDMS()} ({axes.X}), Secondary axis: {axes.Y.ToDMS()} ({axes.Y})");

                axes.Y = 180 - axes.Y;
                axes.Y = RangeDec(axes.Y);
                //axes = RangeAltAzm(axes);
            }
            LogMessage("ConvertAxesToRaDec 1", Outcome.INFO, $"Primary axis: {axes.X.ToDMS()} ({axes.X}), Secondary axis: {axes.Y.ToDMS()} ({axes.Y})");

            raDec.X = TelescopeHardware.SiderealTime - (axes.X * TelescopeHardware.SIDEREAL_SECONDS_TO_SI_SECONDS / 15.0); //* TelescopeHardware.SIDEREAL_SECONDS_TO_SI_SECONDS / 15.0) ;
            LogMessage("ConvertAxesToRaDec 2", Outcome.INFO, $"RA: {raDec.X.ToHMS()} ({raDec.X}), Declination: {raDec.Y.ToDMS()} ({raDec.Y})");
            raDec.Y = (TelescopeHardware.Latitude >= 0) ? axes.Y : -axes.Y;

            raDec = RangeRaDec(raDec);
            LogMessage("ConvertAxesToRaDec 3", Outcome.INFO, $"RA: {raDec.X.ToHMS()} ({raDec.X}), Declination: {raDec.Y.ToDMS()} ({raDec.Y})");

            return raDec;
        }

        /// <summary>
        ///forces the azm to be in the range 0 to 360
        /// </summary>
        /// <param name="ha">The azm.</param>
        /// <returns></returns>
        private static double RangeHaDegrees(double ha)
        {
            while ((ha >= 180.0) || (ha < -180.0))
            {
                if (ha < -180.0) ha += 360.0;
                if (ha >= 180.0) ha -= 360.0;
            }
            return ha;
        }


        /// <summary>
        /// forces a ra dec value to the range 0 to 24.0 and -90 to 90
        /// </summary>
        /// <param name="raDec">The ra dec.</param>
        private static Vector RangeRaDec(Vector raDec)
        {
            return new Vector(RangeHa(raDec.X), RangeDec(raDec.Y));
        }

        /// <summary>
        /// forces an altz value the the range 0 to 360 for azimuth and -90 to 90 for altitude
        /// </summary>
        /// <param name="altAzm"></param>
        private static Vector RangeAltAzm(Vector altAzm)
        {
            return new Vector(RangeAzm(altAzm.X), RangeDec(altAzm.Y));
        }

        /// <summary>
        /// forces axis values to the range 0 to 360 and -90 to 270
        /// </summary>
        /// <param name="axes"></param>
        private static Vector RangeAxes(Vector axes)
        {
            return new Vector(RangeAzm(axes.X), RangeDecx(axes.Y));
        }

        /// <summary>
        ///forces the azm to be in the range 0 to 360
        /// </summary>
        /// <param name="azm">The azm.</param>
        /// <returns></returns>
        private static double RangeAzm(double azm)
        {
            while ((azm >= 360.0) || (azm < 0.0))
            {
                if (azm < 0.0) azm += 360.0;
                if (azm >= 360.0) azm -= 360.0;
            }
            return azm;
        }

        /// <summary>
        /// Forces the dec to be in the range -90 to 0 to 90 to 180 to 270.
        /// </summary>
        /// <param name="dec">The dec in degrees.</param>
        /// <returns></returns>
        internal static double RangeDecx(double dec)
        {
            while ((dec >= 270) || (dec < -90))
            {
                if (dec < -90) dec += 360.0;
                if (dec >= 270) dec -= 360.0;
            }
            return dec;
        }

        /// <summary>
        /// forces the Dec to the range -90 to 0 to +90
        /// </summary>
        /// <param name="dec">The dec.</param>
        /// <returns></returns>
        private static double RangeDec(double dec)
        {
            while ((dec > 90.0) || (dec < -90.0))
            {
                if (dec < -90.0) dec += 180.0;
                if (dec > 90.0) dec = 180.0 - dec;
            }
            return dec;
        }

        /// <summary>
        /// forces the ha/ra value to the range 0 to 24
        /// </summary>
        /// <param name="ha">The ha.</param>
        /// <returns></returns>
        private static double RangeHa(double ha)
        {
            while ((ha >= 24.0) || (ha < 0.0))
            {
                if (ha < 0.0) ha += 24.0;
                if (ha >= 24.0) ha -= 24.0;
            }
            return ha;
        }


        #endregion

        //Vector inputDecVector = new Vector(18.0, 50.0); // LST fixed at 15.0
        //LogMessage("InputVector", Outcome.INFO, $"RA: {inputDecVector.X.ToHMS()}, Dec: {inputDecVector.Y.ToDMS()}");
        //Vector axesVector = ConvertRaDecToAxes(inputDecVector, false);
        //Vector convertedRaDecVector = ConvertAxesToRaDec(axesVector);
        //LogMessage("ConvertedVector", Outcome.INFO, $"RA: {convertedRaDecVector.X.ToHMS()}, Dec: {convertedRaDecVector.Y.ToDMS()}");
        //LogMessage("", Outcome.INFO, "");

        //Vector inputDecVector2 = new Vector(12.0, 50.0); // LST fixed at 15.0
        //LogMessage("InputVector", Outcome.INFO, $"RA: {inputDecVector2.X.ToHMS()}, Dec: {inputDecVector2.Y.ToDMS()}");
        //Vector axesVector2 = ConvertRaDecToAxes(inputDecVector2, false);
        //Vector convertedRaDecVector2 = ConvertAxesToRaDec(axesVector2);
        //LogMessage("ConvertedVector", Outcome.INFO, $"RA: {convertedRaDecVector2.X.ToHMS()}, Dec: {convertedRaDecVector2.Y.ToDMS()}");


    }
}
