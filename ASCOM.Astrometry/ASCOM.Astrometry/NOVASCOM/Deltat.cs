using System;
using static System.Math;

namespace ASCOM.Astrometry
{

    static class DeltatCode
    {
        /// <summary>
    /// Calculates the value of DeltaT over a wide range of historic and future Julian dates
    /// </summary>
    /// <param name="JulianDateUTC">Julian Date of interest</param>
    /// <returns>DelatT value at the given Julian date</returns>
    /// <remarks>
    /// Post 2011, calculation is effected through a 2nd order polynomial best fit to real DeltaT data from: http://maia.usno.navy.mil/ser7/deltat.data 
    /// together with projections of DeltaT from: http://maia.usno.navy.mil/ser7/deltat.preds
    /// The analysis spreadsheets for DeltaT values at dates post 2011 are stored in the \NOVAS\DeltaT Predictions folder of the ASCOM source tree.
    /// 
    /// To ensure that leap second and DeltaUT1 transitions are handled correctly and occur at 00:00:00 UTC, the supplied Julian date should be in UTC time
    /// </remarks>
        public static double DeltaTCalc(double JulianDateUTC)
        {
            double YearFraction, B, Retval, ModifiedJulianDay;
            DateTime UTCDate;

            double LastJulianDateUTC = GlobalItems.DOUBLE_VALUE_NOT_AVAILABLE;
            var LastDeltaTValue = default(double);
            var DeltaTCalcLockObject = new object();

            const double TABSTART1620 = 1620.0d;
            const int TABSIZ = 392;

            lock (DeltaTCalcLockObject)
            {
                if (Truncate(JulianDateUTC - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET) == Truncate(LastJulianDateUTC - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET)) // Return the cached value if its available otherwise calculate it and save the value for the next call
                {
                    return LastDeltaTValue;
                }
            }

            UTCDate = DateTime.FromOADate(JulianDateUTC - GlobalItems.OLE_AUTOMATION_JULIAN_DATE_OFFSET); // Convert the Julian day into a DateTime
            YearFraction = 2000.0d + (JulianDateUTC - GlobalItems.J2000BASE) / GlobalItems.TROPICAL_YEAR_IN_DAYS; // This calculation is accurate enough for our purposes here (T0 = 2451545.0 is TDB Julian date of epoch J2000.0)
            ModifiedJulianDay = JulianDateUTC - GlobalItems.MODIFIED_JULIAN_DAY_OFFSET;

            // NOTE: Starting April 2018 - Please note the use of modified Julian date in the formula rather than year fraction as in previous formulae

            // DATE RANGE 31st December 2024 onwards - This is beyond the sensible extrapolation range of the most recent data analysis so revert to the basic formula: DeltaT = LeapSeconds + 32.184
            if (YearFraction >= 2025.0d)
            {

                // Create an EarthRotationParameters object and retrieve the current leap second value. If something goes wrong return the fall-back value
                try
                {
                    using (var rp = new EarthRotationParameters())
                    {
                        Retval = rp.LeapSeconds() + GlobalItems.TT_TAI_OFFSET; // Get today's leap second value using whatever mechanic the user has configured and convert to DeltaT
                    }
                }
                catch (Exception ex)
                {
                    // Ultimate fallback value if all else fails!
                    Retval = GlobalItems.LEAP_SECOND_ULTIMATE_FALLBACK_VALUE + GlobalItems.TT_TAI_OFFSET;
                }
            }

            // DATE RANGE 20th August 2023 Onwards - The analysis was performed on 20th August 2023 and creates values within 0.01 of a second of the projections to 19th August 2024.
            else if (YearFraction >= 2023.6d)
            {
                Retval = +0.0d * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay + (+0.0d * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay) + -0.00000000836552733660643d * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay + (+0.00151338479660039d * ModifiedJulianDay * ModifiedJulianDay) + -91.2604650974829d * ModifiedJulianDay + (+1834465.8890493d);




            }

            // DATE RANGE 18th July 2022 Onwards - The analysis was performed on 18th July 2022 and creates values within 0.01 of a second of the projections to 17th July 2023.
            else if (YearFraction >= 2022.55d)
            {
                Retval = -0.000000000000528908084762244d * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay + (+0.000000158529137391645d * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay) + -0.0190063060965729d * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay + (+1139.34719487418d * ModifiedJulianDay * ModifiedJulianDay) + -34149488.355673d * ModifiedJulianDay + (+409422822837.639d);




            }

            // DATE RANGE October 17th 2021 Onwards - The analysis was performed on 17th October 2021 and creates values within 0.01 of a second of the projections to the end of October 2022.
            else if (YearFraction >= 2021.79d)
            {
                Retval = 0.000000000000926333089959963d * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay + -0.000000276351646101278d * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay + 0.0329773938043592d * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay + -1967.61450470546d * ModifiedJulianDay * ModifiedJulianDay + 58699325.5212533d * ModifiedJulianDay - 700463653286.072d;




            }

            // DATE RANGE October 17th 2020 Onwards - The analysis was performed on 17th July 2020 and creates values within 0.01 of a second of the projections to October 2021 and sensible extrapolation to the end of 2021
            else if (YearFraction >= 2020.79d)
            {
                Retval = 0.0000000000526391114738186d * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay + -0.0000124987447353606d * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay + 1.1128953517557d * ModifiedJulianDay * ModifiedJulianDay + -44041.1402447551d * ModifiedJulianDay + 653571203.42671d;



            }

            // DATE RANGE July 2020 Onwards - The analysis was performed on 10th July 2020 and creates values within 0.01 of a second of the projections to Q2 2021 and sensible extrapolation to the end of 2021
            else if (YearFraction >= 2020.5d)
            {
                Retval = 0.0000000000234066661113585d * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay - 0.00000555556956413194d * ModifiedJulianDay * ModifiedJulianDay * ModifiedJulianDay + 0.494477925757861d * ModifiedJulianDay * ModifiedJulianDay - 19560.53496991d * ModifiedJulianDay + 290164271.563078d;
            }

            // DATE RANGE April 2018 Onwards - The analysis was performed on 25th April 2018 and creates values within 0.03 of a second of the projections to Q4 2018 and sensible extrapolation to 2021
            else if (YearFraction >= 2018.3d & YearFraction < double.MaxValue)
            {
                Retval = 0.00000161128367083801d * ModifiedJulianDay * ModifiedJulianDay + -0.187474214389602d * ModifiedJulianDay + 5522.26034874982d;
            }

            // DATE RANGE January 2018 Onwards - The analysis was performed on 28th December 2017 and creates values within 0.03 of a second of the projections to Q4 2018 and sensible extrapolation to 2021
            else if (YearFraction >= 2018d & YearFraction < double.MaxValue)
            {
                Retval = 0.0024855297566049d * YearFraction * YearFraction * YearFraction + -15.0681141702439d * YearFraction * YearFraction + 30449.647471213d * YearFraction - 20511035.5077593d;
            }

            // DATE RANGE January 2017 Onwards - The analysis was performed on 29th December 2016 and creates values within 0.12 of a second of the projections to Q3 2019
            else if (YearFraction >= 2017.0d & YearFraction < double.MaxValue)
            {
                Retval = 0.02465436d * YearFraction * YearFraction + -98.92626556d * YearFraction + 99301.85784308d;
            }

            // DATE RANGE October 2015 Onwards - The analysis was performed on 24th October 2015 and creates values within 0.05 of a second of the projections to Q2 2018
            else if (YearFraction >= 2015.75d & YearFraction < double.MaxValue)
            {
                Retval = 0.02002376d * YearFraction * YearFraction + -80.27921003d * YearFraction + 80529.32d;
            }

            // DATE RANGE October 2011 to September 2015 - The analysis was performed on 6th February 2014 and creates values within 0.2 of a second of the projections to Q1 2016
            else if (YearFraction >= 2011.75d & YearFraction < 2015.75d)
            {
                Retval = 0.00231189d * YearFraction * YearFraction + -8.85231952d * YearFraction + 8518.54d;
            }

            // DATE RANGE January 2011 to September 2011
            else if (YearFraction >= 2011.0d & YearFraction < 2011.75d)
            {
                // Following now superseded by above for 2012-16, this is left in for consistency with previous behaviour
                // Use polynomial given at http://sunearth.gsfc.nasa.gov/eclipse/SEcat5/deltatpoly.html as retrieved on 11-Jan-2009
                B = YearFraction - 2000.0d;
                Retval = 62.92d + B * (0.32217d + B * 0.005589d);
            }
            else // Bob's original code
            {

                // Setup for pre 2011 calculations using Bob Denny's original code

                // /* Note, Stephenson and Morrison's table starts at the year 1630.
                // * The Chapronts' table does not agree with the Almanac prior to 1630.
                // * The actual accuracy decreases rapidly prior to 1780.
                // */
                // static short dt[] = {
                short[] dt = new short[] { 12400, 11900, 11500, 11000, 10600, 10200, 9800, 9500, 9100, 8800, 8500, 8200, 7900, 7700, 7400, 7200, 7000, 6700, 6500, 6300, 6200, 6000, 5800, 5700, 5500, 5400, 5300, 5100, 5000, 4900, 4800, 4700, 4600, 4500, 4400, 4300, 4200, 4100, 4000, 3800, 3700, 3600, 3500, 3400, 3300, 3200, 3100, 3000, 2800, 2700, 2600, 2500, 2400, 2300, 2200, 2100, 2000, 1900, 1800, 1700, 1600, 1500, 1400, 1400, 1300, 1200, 1200, 1100, 1100, 1000, 1000, 1000, 900, 900, 900, 900, 900, 900, 900, 900, 900, 900, 900, 900, 900, 900, 900, 900, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1100, 1100, 1100, 1100, 1100, 1100, 1100, 1100, 1100, 1100, 1100, 1100, 1100, 1100, 1100, 1100, 1100, 1200, 1200, 1200, 1200, 1200, 1200, 1200, 1200, 1200, 1200, 1300, 1300, 1300, 1300, 1300, 1300, 1300, 1400, 1400, 1400, 1400, 1400, 1400, 1400, 1500, 1500, 1500, 1500, 1500, 1500, 1500, 1600, 1600, 1600, 1600, 1600, 1600, 1600, 1600, 1600, 1600, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1700, 1600, 1600, 1600, 1600, 1500, 1500, 1400, 1400, 1370, 1340, 1310, 1290, 1270, 1260, 1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250, 1240, 1230, 1220, 1200, 1170, 1140, 1110, 1060, 1020, 960, 910, 860, 800, 750, 700, 660, 630, 600, 580, 570, 560, 560, 560, 570, 580, 590, 610, 620, 630, 650, 660, 680, 690, 710, 720, 730, 740, 750, 760, 770, 770, 780, 780, 788, 782, 754, 697, 640, 602, 541, 410, 292, 182, 161, 10, -102, -128, -269, -324, -364, -454, -471, -511, -540, -542, -520, -546, -546, -579, -563, -564, -580, -566, -587, -601, -619, -664, -644, -647, -609, -576, -466, -374, -272, -154, -2, 124, 264, 386, 537, 614, 775, 913, 1046, 1153, 1336, 1465, 1601, 1720, 1824, 1906, 2025, 2095, 2116, 2225, 2241, 2303, 2349, 2362, 2386, 2449, 2434, 2408, 2402, 2400, 2387, 2395, 2386, 2393, 2373, 2392, 2396, 2402, 2433, 2483, 2530, 2570, 2624, 2677, 2728, 2778, 2825, 2871, 2915, 2957, 2997, 3036, 3072, 3107, 3135, 3168, 3218, 3268, 3315, 3359, 3400, 3447, 3503, 3573, 3654, 3743, 3829, 3920, 4018, 4117, 4223, 4337, 4449, 4548, 4646, 4752, 4853, 4959, 5054, 5138, 5217, 5296, 5379, 5434, 5487, 5532, 5582, 5630, 5686, 5757, 5831, 5912, 5998, 6078, 6163, 6230, 6296, 6347, 6383, 6409, 6430, 6447, 6457, 6469, 6485, 6515, 6546, 6570, 6650, 6710 };
                // Change TABEND and TABSIZ if you add/delete anything

                // Calculate  DeltaT = ET - UT in seconds.  Describes the irregularities of the Earth rotation rate in the ET time scale.
                double p;
                var d = new int[7];
                int i, iy, k;

                // DATE RANGE <1620
                if (YearFraction < TABSTART1620)
                {
                    if (YearFraction >= 948.0d)
                    {
                        // /* Stephenson and Morrison, stated domain is 948 to 1600:
                        // * 25.5(centuries from 1800)^2 - 1.9159(centuries from 1955)^2
                        // */
                        B = 0.01d * (YearFraction - 2000.0d);
                        Retval = (23.58d * B + 100.3d) * B + 101.6d;
                    }
                    else
                    {
                        // /* Borkowski */
                        B = 0.01d * (YearFraction - 2000.0d) + 3.75d;
                        Retval = 35.0d * B * B + 40.0d;
                    }
                }
                else
                {

                    // DATE RANGE 1620 to 2011

                    // Besselian interpolation from tabulated values. See AA page K11.
                    // Index into the table.
                    p = Floor(YearFraction);
                    iy = (int)Round(p - TABSTART1620);            // // rbd - added cast
                                                                  // /* Zeroth order estimate is value at start of year */
                    Retval = dt[iy];
                    k = iy + 1;
                    if (k >= TABSIZ)
                        goto done; // /* No data, can't go on. */

                    // /* The fraction of tabulation interval */
                    p = YearFraction - p;

                    // /* First order interpolated value */
                    Retval += p * (dt[k] - dt[iy]);
                    if (iy - 1 < 0 | iy + 2 >= TABSIZ)
                        goto done; // /* can't do second differences */

                    // /* Make table of first differences */
                    k = iy - 2;
                    for (i = 0; i <= 4; i++)
                    {
                        if (k < 0 | k + 1 >= TABSIZ)
                        {
                            d[i] = 0;
                        }
                        else
                        {
                            d[i] = dt[k + 1] - dt[k];
                        }
                        k += 1;
                    }
                    // /* Compute second differences */
                    for (i = 0; i <= 3; i++)
                        d[i] = d[i + 1] - d[i];
                    B = 0.25d * p * (p - 1.0d);
                    Retval += B * (d[1] + d[2]);
                    if (iy + 2 >= TABSIZ)
                        goto done;

                    // /* Compute third differences */
                    for (i = 0; i <= 2; i++)
                        d[i] = d[i + 1] - d[i];
                    B = 2.0d * B / 3.0d;
                    Retval += (p - 0.5d) * B * d[1];
                    if (iy - 2 < 0 | iy + 3 > TABSIZ)
                        goto done;

                    // /* Compute fourth differences */
                    for (i = 0; i <= 1; i++)
                        d[i] = d[i + 1] - d[i];
                    B = 0.125d * B * (p + 1.0d) * (p - 2.0d);
                    Retval += B * (d[0] + d[1]);

                // /* Astronomical Almanac table is corrected by adding the expression
                // *     -0.000091 (ndot + 26)(year-1955)^2  seconds
                // * to entries prior to 1955 (AA page K8), where ndot is the secular
                // * tidal term in the mean motion of the Moon.
                // *
                // * Entries after 1955 are referred to atomic time standards and
                // * are not affected by errors in Lunar or planetary theory.
                // */
                done:
                    ;
                    Retval *= 0.01d;
                    if (YearFraction < 1955.0d)
                    {
                        B = YearFraction - 1955.0d;
                        Retval += -0.000091d * (-25.8d + 26.0d) * B * B;
                    }
                }
            }

            lock (DeltaTCalcLockObject)
            {
                LastDeltaTValue = Retval;
                LastJulianDateUTC = JulianDateUTC;
                return Retval;
            }

        }
    }
}