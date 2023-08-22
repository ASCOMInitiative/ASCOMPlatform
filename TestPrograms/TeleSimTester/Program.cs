using ASCOM.Astrometry.AstroUtils;
using ASCOM.Astrometry.Transform;
using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace TeleSimTester
{
    internal static class Program
    {

        const double ARCSEC_TO_DEGREES = 360.0 / (24.0 * 60.0 * 60.0);

        static TraceLogger TL;

        static AstroUtils astroUtils;
        static Util util;
        static Telescope telescope;

        static List<string> failures = new List<string>();
        static List<string> errors = new List<string>();

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
                //using (Telescope telescopeSimulator = new Telescope("ScopeSim.Telescope"))
                //using (Telescope telescopeSimulator = new Telescope("ASCOM.Simulator.Telescope"))
                using (Telescope telescopeSimulator = new Telescope("ASCOM.AlpacaDynamic1.Telescope"))
                {
                    try
                    {
                        telescope = telescopeSimulator;

                        telescopeSimulator.Connected = true;
                        LogMessage("Description", Outcome.INFO, $"Running on Platform: {util.MajorVersion}.{util.MinorVersion} SP: {util.ServicePack} Build: {util.BuildNumber}");
                        LogMessage("Description", Outcome.INFO, $"{telescopeSimulator.Description}");
                        LogMessage("Interface version", Outcome.INFO, $"{telescopeSimulator.InterfaceVersion}");
                        if (telescopeSimulator.InterfaceVersion == 2)
                        {
                            LogMessage("Description", Outcome.INFO, $"Site latitude: {telescopeSimulator.SiteLatitude}. Alignment mode: {telescopeSimulator.AlignmentMode}, Pointing state: {PierSide.pierUnknown}");
                        }
                        else
                        {
                            LogMessage("Description", Outcome.INFO, $"Site latitude: {telescopeSimulator.SiteLatitude}. Alignment mode: {telescopeSimulator.AlignmentMode}, Pointing state: {telescope.SideOfPier}");
                        }

                        SlewCoordinateTests();

                        MoveAxisTests();

                        RaDecOffsetRateTests();

                        PulseGuideTests();

                        LogMessage("All tests", Outcome.INFO, "COMPLETED");
                        LogBlankLine();

                        if (failures.Count > 0)
                        {
                            LogMessage("Failures", Outcome.INFO, $"Found {failures.Count} failures...");
                            foreach (string item in failures)
                            {
                                LogMessage("Failures", Outcome.INFO, item);
                            }
                        }
                        else
                        {
                            LogMessage("Failures", Outcome.INFO, "No failures found");
                        }

                        LogBlankLine();
                        if (errors.Count > 0)
                        {
                            LogMessage("Errors", Outcome.INFO, $"Found {errors.Count} errors...");
                            foreach (string item in errors)
                            {
                                LogMessage("Errors", Outcome.INFO, item);
                            }
                        }
                        else
                        {
                            LogMessage("Errors", Outcome.INFO, "No errors found");
                        }

                        Console.ReadLine();
                    }

                    catch (Exception ex)
                    {
                        LogMessage("Using", Outcome.ERROR, ex.ToString());
                    }
                    finally
                    {
                        telescope.Connected = false;
                    }
                }
            }
            catch (Exception ex1)
            {
                LogMessage("Main", Outcome.ERROR, ex1.ToString());
            }

        }

        private static void PulseGuideTests()
        {
            telescope.Tracking = true;
            LogMessage($"PulseGuide", Outcome.INFO, $"Guide rate RA: {telescope.GuideRateRightAscension.ToDMS()}, Guide rate declination: {telescope.GuideRateDeclination.ToDMS()}");

            SlewToHaDec(-9.0, telescope.SiteLatitude);
            LogMessage($"PulseGuide", Outcome.INFO, $"Slewed to HA Dec, testing pulse guiding...");

            TestPulseGuide(-9.0);
            SlewToHaDec(+9.0, telescope.SiteLatitude);
            TestPulseGuide(+9.0);
            SlewToHaDec(-3.0, telescope.SiteLatitude);
            TestPulseGuide(-3.0);
            SlewToHaDec(+3.0, telescope.SiteLatitude);
            TestPulseGuide(+3.0);
        }

        private static void TestPulseGuide(double ha)
        {
            LogMessage($"PulseGuide", Outcome.INFO, $"Testing at hour angle: {ha}");
            TestPulseGuideDirection(GuideDirections.guideNorth);
            TestPulseGuideDirection(GuideDirections.guideSouth);
            TestPulseGuideDirection(GuideDirections.guideEast);
            TestPulseGuideDirection(GuideDirections.guideWest);

            LogBlankLine();
            LogBlankLine();
        }

        private static void TestPulseGuideDirection(GuideDirections? direction)
        {
            const int GUIDE_DURATION = 5000; // Milli-seconds
            const double CHANGE_TOLERANCE_DEGREES = 1.0 / (3600.0); // Arc seconds in degrees
            const double CHANGE_TOLERANCE_HOURS = CHANGE_TOLERANCE_DEGREES / 15.0; // 1 Arc seconds in hours
            const double SIDEREAL_RATE = 15.041; //Arc-seconds per second

            double expectedDeclinationChange = (SIDEREAL_RATE * GUIDE_DURATION) / (1000.0 * 3600.0); // Degrees
            double expectedRAChange = (SIDEREAL_RATE * GUIDE_DURATION) / (15.0 * 1000.0 * 3600.0); // Hours

            double finalRACoordinate;
            double finalDeclinationCoordinate;

            double raChange;
            double declinationChange;
            double initialRACoordinate;
            double initialDeclinationCoordinate;

            LogMessage($"PulseGuide {direction}", Outcome.INFO, $"Test guiding direction: {direction}");

            initialRACoordinate = telescope.RightAscension;
            initialDeclinationCoordinate = telescope.Declination;

            if (direction != null) telescope.PulseGuide((GuideDirections)direction, GUIDE_DURATION);

            WaitForPulseGuide();

            finalRACoordinate = telescope.RightAscension;
            finalDeclinationCoordinate = telescope.Declination;
            raChange = finalRACoordinate - initialRACoordinate;
            declinationChange = finalDeclinationCoordinate - initialDeclinationCoordinate;

            LogMessage($"PulseGuide {direction}", Outcome.INFO, $"Primary axis change: {(raChange * 15.0).ToDMS()} degrees, Secondary axis change: {declinationChange.ToDMS()} degrees.");

            switch (direction)
            {
                case GuideDirections.guideNorth:
                    if (declinationChange > 0.0) // Moved north
                        LogMessage($"PulseGuide {direction}", Outcome.OK, $"Moved north as expected");
                    else
                        LogMessage($"PulseGuide {direction}", Outcome.FAILED, $"Moved south!");

                    if (Math.Abs(declinationChange - expectedDeclinationChange) <= CHANGE_TOLERANCE_DEGREES)
                        LogMessage($"PulseGuide {direction}", Outcome.OK, $"Moved north-south within expected tolerance");
                    else
                        LogMessage($"PulseGuide {direction}", Outcome.FAILED, $"Northward move outside tolerance: Declination change: {declinationChange.ToDMS()}, Expected: {expectedDeclinationChange.ToDMS()}, Difference: {Math.Abs(declinationChange - expectedDeclinationChange).ToDMS()}, Tolerance: {CHANGE_TOLERANCE_DEGREES.ToDMS()} degrees");

                    if (Math.Abs(raChange) <= CHANGE_TOLERANCE_HOURS)
                        LogMessage($"PulseGuide {direction}", Outcome.OK, $"No east-west movement as expected.");
                    else
                        LogMessage($"PulseGuide {direction}", Outcome.FAILED, $"East-west movement outside tolerance: RA change: {raChange.ToHMS()}, Expected: {0.0.ToHMS()}, Difference: {Math.Abs(raChange - expectedRAChange).ToHMS()}, Tolerance: {CHANGE_TOLERANCE_DEGREES.ToHMS()} hours");
                    break;

                case GuideDirections.guideSouth:
                    if (declinationChange < 0.0) // Moved south
                        LogMessage($"PulseGuide {direction}", Outcome.OK, $"Moved south as expected");
                    else
                        LogMessage($"PulseGuide {direction}", Outcome.FAILED, $"Moved north!");

                    if (Math.Abs(declinationChange + expectedDeclinationChange) <= CHANGE_TOLERANCE_DEGREES)
                        LogMessage($"PulseGuide {direction}", Outcome.OK, $"Moved north-south within expected tolerance");
                    else
                        LogMessage($"PulseGuide {direction}", Outcome.FAILED, $"Southward move outside tolerance: Declination change: {declinationChange.ToDMS()}, Expected: {expectedDeclinationChange.ToDMS()}, Difference: {Math.Abs(declinationChange + expectedDeclinationChange).ToDMS()}, Tolerance: {CHANGE_TOLERANCE_DEGREES.ToDMS()} degrees");

                    if (Math.Abs(raChange) <= CHANGE_TOLERANCE_HOURS)
                        LogMessage($"PulseGuide {direction}", Outcome.OK, $"No east-west movement as expected.");
                    else
                        LogMessage($"PulseGuide {direction}", Outcome.FAILED, $"East-west movement outside tolerance: RA change: {raChange.ToHMS()}, Expected: {0.0.ToHMS()}, Difference: {Math.Abs(raChange - expectedRAChange).ToHMS()}, Tolerance: {CHANGE_TOLERANCE_HOURS.ToHMS()} hours");
                    break;

                case GuideDirections.guideEast:
                    if (raChange > 0.0) // Moved east
                        LogMessage($"PulseGuide {direction}", Outcome.OK, $"Moved east as expected");
                    else // Moved west
                        LogMessage($"PulseGuide {direction}", Outcome.FAILED, $"Moved west!");

                    if (Math.Abs(raChange - expectedRAChange) <= CHANGE_TOLERANCE_HOURS)
                        LogMessage($"PulseGuide {direction}", Outcome.OK, $"Moved east-west within expected tolerance.");
                    else
                        LogMessage($"PulseGuide {direction}", Outcome.FAILED, $"East-west movement outside tolerance: RA change: {raChange.ToHMS()}, Expected: {expectedRAChange.ToHMS()}, Difference: {Math.Abs(raChange - expectedRAChange).ToHMS()}, Tolerance: {CHANGE_TOLERANCE_HOURS.ToHMS()} hours");

                    if (Math.Abs(declinationChange) <= CHANGE_TOLERANCE_DEGREES)
                        LogMessage($"PulseGuide {direction}", Outcome.OK, $"No north-south movement as expected");
                    else
                        LogMessage($"PulseGuide {direction}", Outcome.FAILED, $"North-south move outside tolerance: Declination change: {declinationChange.ToDMS()}, Expected: {0.0.ToDMS()}, Difference: {Math.Abs(declinationChange - expectedDeclinationChange).ToDMS()}, Tolerance: {CHANGE_TOLERANCE_DEGREES.ToDMS()} degrees");

                    break;

                case GuideDirections.guideWest:
                    if (raChange < 0.0) // Moved east
                        LogMessage($"PulseGuide {direction}", Outcome.OK, $"Moved west as expected");
                    else // Moved west
                        LogMessage($"PulseGuide {direction}", Outcome.FAILED, $"Moved east!");

                    if (Math.Abs(raChange + expectedRAChange) <= CHANGE_TOLERANCE_HOURS)
                        LogMessage($"PulseGuide {direction}", Outcome.OK, $"Moved east-west within expected tolerance.");
                    else
                        LogMessage($"PulseGuide {direction}", Outcome.FAILED, $"East-west movement outside tolerance: RA change: {raChange.ToHMS()}, Expected: {expectedRAChange.ToHMS()}, Difference: {Math.Abs(raChange + expectedRAChange).ToHMS()}, Tolerance: {CHANGE_TOLERANCE_HOURS.ToHMS()} hours");

                    if (Math.Abs(declinationChange) <= CHANGE_TOLERANCE_DEGREES)
                        LogMessage($"PulseGuide {direction}", Outcome.OK, $"No north-south movement as expected");
                    else
                        LogMessage($"PulseGuide {direction}", Outcome.FAILED, $"North-south move outside tolerance: Declination change: {declinationChange.ToDMS()}, Expected: {0.0.ToDMS()}, Difference: {Math.Abs(declinationChange - expectedDeclinationChange).ToDMS()}, Tolerance: {CHANGE_TOLERANCE_DEGREES.ToDMS()} degrees");
                    break;

                default:
                    break;
            }
        }

        private static void WaitForPulseGuide()
        {
            do
            {
                Thread.Sleep(100);
            } while (telescope.IsPulseGuiding);
        }

        private static void SlewCoordinateTests()
        {
            ValidateCoordinates(-3.0, 85.0);
            ValidateCoordinates(-3.0, 30.0);
            ValidateCoordinates(-3.0, 0.0);

            ValidateCoordinates(-9.0, 85.0);
            ValidateCoordinates(-9.0, 30.0);
            ValidateCoordinates(-9.0, 0.0);

            ValidateCoordinates(+3.0, 85.0);
            ValidateCoordinates(+3.0, 30.0);
            ValidateCoordinates(+3.0, 0.0);

            ValidateCoordinates(+9.0, 85.0);
            ValidateCoordinates(+9.0, 30.0);
            ValidateCoordinates(+9.0, 0.0);
        }

        private static void ValidateCoordinates(double targetHa, double targetDec)
        {
            // Calculate target RA from provided target HA
            double targetRaFromHa = astroUtils.ConditionRA(telescope.SiderealTime - targetHa);

            // Correct for southern hemisphere
            if (telescope.SiteLatitude < 0.0)
                targetDec = -targetDec;

            Transform transform = new Transform();
            transform.SiteTemperature = 10.0;
            transform.SitePressure = 0;
            transform.SiteElevation = telescope.SiteElevation;
            transform.SiteLatitude = telescope.SiteLatitude;
            transform.SiteLongitude = telescope.SiteLongitude;
            transform.Refraction = false;

            LogMessage("ValidateCoordinates", Outcome.INFO, $"Set Transform topocentric coordinates - RA: {targetRaFromHa.ToHMS()}, Declination: {targetDec.ToDMS()}");

            telescope.Tracking = true;
            SlewToRaDec(targetRaFromHa, targetDec);

            double ra = telescope.RightAscension;
            double declination = telescope.Declination;
            double azimuth = telescope.Azimuth;
            double elevation = telescope.Altitude;

            transform.SetTopocentric(targetRaFromHa, targetDec);
            double tra = transform.RATopocentric;
            double tdeclination = transform.DECTopocentric;
            double tazimuth = transform.AzimuthTopocentric;
            double televation = transform.ElevationTopocentric;

            LogMessage("ValidateCoordinates", Outcome.INFO, $"Julian date: {(transform.JulianDateUTC == 0.0 ? "0.0 (Automatic)" : util.DateJulianToLocal(transform.JulianDateUTC).ToLongTimeString())}");
            LogMessage("ValidateCoordinates", Outcome.INFO, $"Telescope  - RA: {ra.ToHMS()}, Declination: {declination.ToDMS()}, Azimuth: {azimuth.ToDMS()}, Elevation: {elevation.ToDMS()}");
            LogMessage("ValidateCoordinates", Outcome.INFO, $"Transform  - RA: {tra.ToHMS()}, Declination: {tdeclination.ToDMS()}, Azimuth: {tazimuth.ToDMS()}, Elevation: {televation.ToDMS()}");
            LogMessage("ValidateCoordinates", Outcome.INFO, $"Difference - RA: {(tra - ra).ToHMS()}, Declination: {(tdeclination - declination).ToDMS()}, Azimuth: {(tazimuth - azimuth).ToDMS()}, Elevation: {(televation - elevation).ToDMS()}");

            TestDecDifference("ValidateCoordinates", "Azimuth", azimuth, tazimuth, util.DMSToDegrees("00:00:15"));
            TestDecDifference("ValidateCoordinates", "Elevation", elevation, televation, util.DMSToDegrees("00:00:15"));

            WaitFor(2.00);

            LogBlankLine();
        }

        private static void RaDecOffsetRateTests()
        {
            telescope.Tracking = true;
            double testDeclination = telescope.SiteLatitude >= 0.0 ? +40.0 : -40.0;

            // RA offsets only
            SetRaDecOffsetRates(+9.0, 15.0, 0.0);
            // Dec offsets only
            SetRaDecOffsetRates(+9.0, 0.0, 15.0);

            // RA offsets only
            SetRaDecOffsetRates(+3.0, 15.0, 0.0);
            // Dec offsets only
            SetRaDecOffsetRates(+3.0, 0.0, 15.0);

            // RA offsets only
            SetRaDecOffsetRates(-9.0, 15.0, 0.0);
            // Dec offsets only
            SetRaDecOffsetRates(-9.0, 0.0, 15.0);

            // RA offsets only
            SetRaDecOffsetRates(-3.0, 15.0, 0.0);
            // Dec offsets only
            SetRaDecOffsetRates(-3.0, 0.0, 15.0);

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

        private static void MoveAxisTests()
        {
            // Apply appropriate test depending on alignment mode
            if (telescope.AlignmentMode == AlignmentModes.algAltAz)
            {
                telescope.Tracking = false;
                LogBlankLine();

                SlewToAltAz(45.0, 45.0);
                MoveAxesAltAz(3.0, 0.0);

                SlewToAltAz(45.0, 45.0);
                MoveAxesAltAz(0.0, 3.0);

                SlewToAltAz(45.0, 45.0);
                MoveAxesAltAz(3.0, 3.0);

                SlewToAltAz(45.0, 45.0);
                MoveAxesAltAz(-3.0, -3.0);

                MoveAxesAltAz(0.0, 0.0);
                LogMessage("MoveAxesAltAz", Outcome.INFO, "FINISHED");
            }
            else
            {
                telescope.Tracking = true;
                LogBlankLine();

                double testDeclination = telescope.SiteLatitude >= 0.0 ? 45.0 : -45.0;

                SlewToHaDec(3.0, testDeclination);
                MoveAxesRaDec(3.0, 0.0);

                SlewToHaDec(3.0, testDeclination);
                MoveAxesRaDec(0.0, 3.0);

                SlewToHaDec(3.0, testDeclination);
                MoveAxesRaDec(3.0, 3.0);

                SlewToHaDec(3.0, testDeclination);
                MoveAxesRaDec(-3.0, -3.0);


                SlewToHaDec(9.0, testDeclination);
                MoveAxesRaDec(3.0, 0.0);

                SlewToHaDec(9.0, testDeclination);
                MoveAxesRaDec(0.0, 3.0);

                SlewToHaDec(9.0, testDeclination);
                MoveAxesRaDec(3.0, 3.0);

                SlewToHaDec(9.0, testDeclination);
                MoveAxesRaDec(-3.0, -3.0);


                SlewToHaDec(-3.0, testDeclination);
                MoveAxesRaDec(3.0, 0.0);

                SlewToHaDec(-3.0, testDeclination);
                MoveAxesRaDec(0.0, 3.0);

                SlewToHaDec(-3.0, testDeclination);
                MoveAxesRaDec(3.0, 3.0);

                SlewToHaDec(-3.0, testDeclination);
                MoveAxesRaDec(-3.0, -3.0);


                SlewToHaDec(-9.0, testDeclination);
                MoveAxesRaDec(3.0, 0.0);

                SlewToHaDec(-9.0, testDeclination);
                MoveAxesRaDec(0.0, 3.0);

                SlewToHaDec(-9.0, testDeclination);
                MoveAxesRaDec(3.0, 3.0);

                SlewToHaDec(-9.0, testDeclination);
                MoveAxesRaDec(-3.0, -3.0);

                LogMessage("MoveAxesRaDec", Outcome.INFO, "FINISHED");
            }
        }

        #region Slew support and logging

        /// <summary>
        /// 
        /// </summary>
        /// <param name="test">Name  of test</param>
        /// <param name="expectedRaRate">Test RA rate (RA seconds per SI second)</param>
        /// <param name="expectedDeclinationRate">Test declination rate (Degrees per SI second)</param>
        internal static void SetRaDecOffsetRates(double testHa, double expectedRaRate, double expectedDeclinationRate)
        {
            const double DURATION = 10.0; // Seconds

            double testRa = astroUtils.ConditionRA(telescope.SiderealTime - testHa);
            double testDeclination = telescope.SiteLatitude >= 0.0 ? +40.0 : -40.0;

            SlewToRaDec(testRa, testDeclination);

            LogMessage("SetRaDecOffsetRates", Outcome.INFO, $"Testing hour angle: {testHa}");
            if (telescope.InterfaceVersion <= 2)
            {
                LogMessage("SetRaDecOffsetRates", Outcome.INFO, $"Testing Primary rate: {expectedRaRate}, Secondary rate: {expectedDeclinationRate}, SideofPier: {PierSide.pierUnknown}");
            }
            else
            {
                LogMessage("SetRaDecOffsetRates", Outcome.INFO, $"Testing Primary rate: {expectedRaRate}, Secondary rate: {expectedDeclinationRate}, SideofPier: {telescope.SideOfPier}");
            }

            double priStart = telescope.RightAscension;
            double secStart = telescope.Declination;

            telescope.RightAscensionRate = expectedRaRate * TelescopeHardware.SIDEREAL_SECONDS_TO_SI_SECONDS;
            telescope.DeclinationRate = expectedDeclinationRate;

            WaitFor(DURATION);

            double priEnd = telescope.RightAscension;
            double secEnd = telescope.Declination;

            // Restore previous state
            telescope.RightAscensionRate = 0.0;
            telescope.DeclinationRate = 0.0;

            LogMessage("SetRaDecOffsetRates", Outcome.INFO, $"Start      - : {priStart.ToHMS()}, {secStart.ToDMS()}");
            LogMessage("SetRaDecOffsetRates", Outcome.INFO, $"Finish     - : {priEnd.ToHMS()}, {secEnd.ToDMS()}");
            LogMessage("SetRaDecOffsetRates", Outcome.INFO, $"Difference - : {(priEnd - priStart).ToHMS()}, {(secEnd - secStart).ToDMS()}, {priEnd - priStart:N10}, {secEnd - secStart:N10}");

            // Condition results
            double actualPriRate = (priEnd - priStart) / DURATION; // Calculate offset rate in RA hours per SI second
            actualPriRate = actualPriRate * 60.0 * 60.0; // Convert rate in RA hours per SI second to RA seconds per SI second

            double actualSecRate = (secEnd - secStart) / DURATION * 60.0 * 60.0;

            LogMessage("SetRaDecOffsetRates", Outcome.INFO, $"Actual primary rate: {actualPriRate}, Expected rate: {expectedRaRate}, Ratio: {actualPriRate / expectedRaRate}, Actual secondary rate: {actualSecRate}, Expected rate: {expectedDeclinationRate}, Ratio: {actualSecRate / expectedDeclinationRate}");
            TestDouble("SetRaDecOffsetRates", "Primary Axis", actualPriRate, expectedRaRate);
            TestDouble("SetRaDecOffsetRates", "Secondary Axis", actualSecRate, expectedDeclinationRate);
            LogBlankLine();
        }

        internal static void MoveAxesAltAz(double expectedPrimaryRate, double expectedSecondaryRate)
        {
            const double DURATION = 5.0; // Seconds

            LogMessage("MoveAxesAltAz", Outcome.INFO, $"Moving primary axis at: {expectedPrimaryRate}, Moving secondary axis at: {expectedSecondaryRate}");

            double priStart = telescope.Azimuth;
            double secStart = telescope.Altitude;
            LogMessage("MoveAxesAltAz", Outcome.INFO, $"Initial Azimuth: {priStart.ToDMS()}, Initial Altitude: {secStart.ToDMS()}");

            telescope.MoveAxis(TelescopeAxes.axisPrimary, expectedPrimaryRate);
            telescope.MoveAxis(TelescopeAxes.axisSecondary, expectedSecondaryRate);

            WaitFor(DURATION);

            double priEnd = telescope.Azimuth;
            double secEnd = telescope.Altitude;
            LogMessage("MoveAxesAltAz", Outcome.INFO, $"Final Azimuth  : {priEnd.ToDMS()}, Initial Altitude: {secEnd.ToDMS()}");

            // Restore previous state
            telescope.MoveAxis(TelescopeAxes.axisPrimary, 0.0);
            telescope.MoveAxis(TelescopeAxes.axisSecondary, 0.0);

            double actualPriRate = (priEnd - priStart) / DURATION;
            double actualSecRate = (secEnd - secStart) / DURATION;

            LogMessage("MoveAxesAltAz", Outcome.INFO, $"Actual primary rate: {actualPriRate}, Expected rate: {expectedPrimaryRate}, Ratio: {actualPriRate / expectedPrimaryRate}, Actual secondary rate: {actualSecRate}, Expected rate: {expectedSecondaryRate}, Ratio: {actualSecRate / expectedSecondaryRate}");

            TestDouble("MoveAxesAltAz", "Primary Axis", actualPriRate, expectedPrimaryRate);
            TestDouble("MoveAxesAltAz", "Secondary Axis", actualSecRate, expectedSecondaryRate);
            LogBlankLine();
        }

        internal static void MoveAxesRaDec(double expectedPrimaryRate, double expectedSecondaryRate)
        {
            const double DURATION = 5.0; // Seconds

            double rightAscension = telescope.RightAscension;


            double priHaStart = astroUtils.ConditionHA(telescope.SiderealTime - rightAscension);
            double secStart = telescope.Declination;

            LogMessage("MoveAxesRaDec", Outcome.INFO, $"Moving primary axis at: {expectedPrimaryRate}, Moving secondary axis at: {expectedSecondaryRate}");
            LogMessage("MoveAxesRaDec", Outcome.INFO, $"Initial HA: {priHaStart.ToHMS()}, Initial RA: {rightAscension.ToHMS()}, Initial Declination: {secStart.ToDMS()}");

            telescope.MoveAxis(TelescopeAxes.axisPrimary, expectedPrimaryRate);
            telescope.MoveAxis(TelescopeAxes.axisSecondary, expectedSecondaryRate);

            WaitFor(DURATION);

            rightAscension = telescope.RightAscension;
            double priHaEnd = astroUtils.ConditionHA(telescope.SiderealTime - rightAscension);
            double secEnd = telescope.Declination;

            LogMessage("MoveAxesRaDec", Outcome.INFO, $"Final HA:   {priHaEnd.ToHMS()}, Final RA:   {rightAscension.ToHMS()}, Final Declination:   {secEnd.ToDMS()}");

            // Restore previous state
            telescope.MoveAxis(TelescopeAxes.axisPrimary, 0.0);
            telescope.MoveAxis(TelescopeAxes.axisSecondary, 0.0);

            double actualPriRate = astroUtils.ConditionHA(priHaEnd - priHaStart) * 15.0 / DURATION; // Multiply by 15 to convert hours to degrees.
            double actualSecRate = (secEnd - secStart) / DURATION;

            if (telescope.SideOfPier==PierSide.pierWest)
            {
                LogMessage("MoveAxesRaDec", Outcome.INFO, $"Swapping sense of declination change because the scope is through the pole.");
                actualSecRate = -actualSecRate;
            }

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
            double ra = astroUtils.ConditionRA(telescope.SiderealTime - ha);

            LogMessage("SlewToHaDec", Outcome.INFO, $"Slewing to HA: {ha.ToHMS()} (RA: {ra.ToHMS()}), Dec: {declination.ToDMS()}");
            telescope.SlewToCoordinatesAsync(ra, declination);
            WaitForSlew();
            if (telescope.InterfaceVersion <= 2)
            {
                LogMessage("SlewToHaDec", Outcome.INFO, $"Slewed to RA:  {telescope.RightAscension.ToHMS()}, Dec: {telescope.Declination.ToDMS()}, Pointing state: {PierSide.pierUnknown}");
            }
            else
            {
                LogMessage("SlewToHaDec", Outcome.INFO, $"Slewed to RA:  {telescope.RightAscension.ToHMS()}, Dec: {telescope.Declination.ToDMS()}, Pointing state: {telescope.SideOfPier}");
            }
            if (report) TestHaDifference("SlewToHaDec", "RA", telescope.RightAscension, ra, 1.0 / (60.0 * 60.0));
            if (report) TestHaDifference("SlewToHaDec", "Declination", telescope.Declination, declination, 1.0 / (60.0 * 60.0));
        }

        static void SlewToRaDec(double ra, double declination)
        {
            LogMessage("SlewToRaDec", Outcome.INFO, $"Slewing to RA: {ra.ToHMS()}, Dec: {declination.ToDMS()}");
            telescope.SlewToCoordinatesAsync(ra, declination);
            WaitForSlew();
            LogMessage("SlewToRaDec", Outcome.INFO, $"Slewed to RA:  {telescope.RightAscension.ToHMS()}, Dec: {telescope.Declination.ToDMS()}");
            TestDouble("SlewToRaDec", "RA", telescope.RightAscension, ra);
            TestDouble("SlewToRaDec", "Declination", telescope.Declination, declination);
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
            telescope.SlewToAltAzAsync(azimuth, altitude);
            WaitForSlew();

            LogMessage("SlewToAltAz", Outcome.INFO, $"Slewed to Azimuth:  {telescope.Azimuth.ToDMS()}, Altitude: {telescope.Altitude.ToDMS()}");
            TestDouble("SlewToAltAz", "Azimuth", telescope.Azimuth, azimuth);
            TestDouble("SlewToAltAz", "Altitude", telescope.Altitude, altitude);
            LogBlankLine();
        }

        static void WaitForSlew()
        {
            do
            {
                //LogMessage("WaitForSlew", Outcome.INFO, $"RA: {util.HoursToHMS(telStatic.RightAscension, ":", ":", "", 3)}, Dec: {util.DegreesToDMS(telStatic.Declination, ":", ":", "", 3)}");
                Thread.Sleep(500);
            } while (telescope.Slewing);

            //LogMessage("WaitForSlew", Outcome.INFO, $"RA: {util.HoursToHMS(telStatic.RightAscension, ":", ":", "", 3)}, Dec: {util.DegreesToDMS(telStatic.Declination, ":", ":", "", 3)}");
        }

        static void LogMessage(string test, Outcome outcome, string message)
        {
            string logMessage = $"{DateTime.Now:HH:mm:ss.fff} {test,-25}{outcome,-8}{message}";
            Console.WriteLine(logMessage);
            TL.LogMessageCrLf(test, $"{outcome,-8}{message}");

            if (outcome == Outcome.FAILED)
            {
                failures.Add(logMessage);
            }
            if (outcome == Outcome.ERROR)
            {
                errors.Add(logMessage);
            }
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
