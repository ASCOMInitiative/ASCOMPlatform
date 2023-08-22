﻿//tabs=4
// --------------------------------------------------------------------------------
//
// Astronomy Functions
//
// Description:	Astronomy functions class that wraps up the NOVAS fucntions in a
//              quick way to call them. 
//
// Author:		(rbt) Robert Turner <robert@robertturnerastro.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 08-JUL-2009	rbt	1.0.0	Initial edit
// --------------------------------------------------------------------------------
//

using System;
using System.Windows;

namespace ASCOM.Simulator
{
    public static class AstronomyFunctions
    {
        private const double DEG_RAD = 0.01745329251994329;
        private const double RAD_DEG = 57.29577951308232;
        private const double HRS_RAD = 0.2617993877991494;
        private const double RAD_HRS = 3.819718634205; // DO NOT USE 12.0 / Math.Pi FOR THIS CONSTANT! IT CAUSES THE SIMULATOR TO LOCK UP ON WINDOWS 7 64BIT UGGGHHHH!!!!!
        private const double HOURS_TO_DEGREES = 15.0;
        private const double SIDEREAL_SECONDS_TO_SI_SECONDS = 0.99726956631945; // Based on earth sidereal rotation period of 23 hours 56 minutes 4.09053 seconds

        private static Utilities.Util util = new ASCOM.Utilities.Util();

        //----------------------------------------------------------------------------------------
        // Calculate Precession
        //----------------------------------------------------------------------------------------
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Precess")]
        public static double Precess(DateTime dateTime)
        {
            int y = dateTime.Year + 1900;
            if (y >= 3900) { y = y - 1900; }
            int p = y - 1;
            int r = p / 1000;
            int s = 2 - r + r / 4;
            int t = (int)Math.Truncate(365.25 * p);
            double r1 = (s + t - 693597.5) / 36525;
            double s1 = 6.646 + 2400.051 * r1;

            return 24 - s1 + (24 * (y - 1900));
        }

        /// <summary>
        /// Calculate the hour angle of the current pointing direction in hours
        /// </summary>
        /// <param name="rightAscension"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static double HourAngle(double rightAscension, double longitude)
        {
            return RangeHA(TelescopeHardware.SiderealTime - rightAscension);  // Hours
        }

        /// <summary>
        /// Current Local Apparent Sidereal Time in hours for Longitude
        /// </summary>
        /// <param name="longitude">degrees</param>
        /// <returns></returns>
        public static double LocalSiderealTime(double longitude)
        {
            double days_since_j_2000 = util.JulianDate - 2451545.0;
            double t = days_since_j_2000 / 36525;
            double l1mst = 280.46061837 + 360.98564736629 * days_since_j_2000 + longitude;
            if (l1mst < 0.0)
            {
                while (l1mst < 0.0)
                {
                    l1mst = l1mst + 360.0;
                }
            }
            else
            {
                while (l1mst > 360.0)
                {
                    l1mst = l1mst - 360.0;
                }
            }
            //calculate OM
            double om1 = 125.04452 - 1934.136261 * t;
            if (om1 < 0.0)
            {
                while (om1 < 0.0)
                {
                    om1 = om1 + 360.0;
                }
            }
            else
            {
                while (om1 > 360.0)
                {
                    om1 = om1 - 360.0;
                }
            }
            //calculat L
            double La = 280.4665 + 36000.7698 * t;
            if (La < 0.0)
            {
                while (La < 0.0)
                {
                    La = La + 360.0;
                }
            }
            else
            {
                while (La > 360.0)
                {
                    La = La - 360.0;
                }
            }
            //calculate L1
            double L11 = 218.3165 + 481267.8813 * t;
            if (L11 < 0.0)
            {
                while (L11 < 0)
                {
                    L11 = L11 + 360.0;
                }
            }
            else
            {
                while (L11 > 360.0)
                {
                    L11 = L11 - 360.0;
                }
            }
            //calculate e
            double ea1 = 23.439 - 0.0000004 * t;
            if (ea1 < 0.0)
            {
                while (ea1 < 0.0)
                {
                    ea1 = ea1 + 360.0;
                }
            }
            else
            {
                while (ea1 > 360.0)
                {
                    ea1 = ea1 - 360.0;
                }
            }

            double dp1 = (-17.2 * Math.Sin(om1)) - (1.32 * Math.Sin(2 * La)) - (0.23 * Math.Sin(2 * L11)) + (0.21 * Math.Sin(2 * om1));
            //double de1 = (9.2 * Math.Cos(om1)) + (0.57 * Math.Cos(2 * La)) + (0.1 * Math.Cos(2 * L11)) - (0.09 * Math.Cos(2 * om1));
            //double eps1 = ea1 + de1;
            double correction1 = (dp1 * Math.Cos(ea1)) / 3600;
            l1mst = l1mst + correction1;

            return RangeRA(l1mst * 24.0 / 360.0);
        }

        /// <summary>
        /// Calculate RA in sidereal hours
        /// </summary>
        /// <param name="altitude"></param>
        /// <param name="azimuth"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public static double CalculateRA(double altitude, double azimuth, double latitude)
        {
            var alt = altitude * DEG_RAD;
            var azm = azimuth * DEG_RAD;
            var lat = latitude * DEG_RAD;
            double hourAngle = Math.Atan2(-Math.Sin(azm) * Math.Cos(alt),
                                          -Math.Cos(azm) * Math.Sin(lat) * Math.Cos(alt) + Math.Sin(alt) * Math.Cos(lat))
                                          * RAD_HRS;

            return RangeRA(TelescopeHardware.SiderealTime - hourAngle);
        }

        /// <summary>
        /// Calculate declination in degrees
        /// </summary>
        /// <param name="altitude"></param>
        /// <param name="azimuth"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public static double CalculateDec(double altitude, double azimuth, double latitude)
        {
            var alt = altitude * DEG_RAD;
            var azm = azimuth * DEG_RAD;
            var lat = latitude * DEG_RAD;
            var dec = Math.Asin(Math.Cos(azm) * Math.Cos(lat) * Math.Cos(alt) + Math.Sin(lat) * Math.Sin(alt)) * RAD_DEG;
            return RangeDec(dec);
        }


        /// <summary>
        /// Calculate Altitude and Azimuth From Ra/Dec and Site, ra in hours, the rest degrees
        /// </summary>
        /// <param name="rightAscension"></param>
        /// <param name="declination"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        //private static double CalculateAltitude(double rightAscension, double declination, double latitude, double longitude)
        //{
        //    double azimuth;
        //    return CalculateAltAzm(rightAscension, declination, latitude, longitude, out azimuth);

        //    //double lst = LocalSiderealTime(longitude * SharedResources.RAD_DEG); // Hours
        //    //double ha = (lst - rightAscension * SharedResources.RAD_HRS) * SharedResources.HRS_RAD; //Radians

        //    //double sh = Math.Sin(ha);
        //    //double ch = Math.Cos(ha);
        //    //double sd = Math.Sin(declination);
        //    //double cd = Math.Cos(declination);
        //    //double sl = Math.Sin(latitude);
        //    //double cl = Math.Cos(latitude);

        //    //double x = (sd * cl) - (ch * cd * sl);
        //    //double y = -(sh * cd);
        //    //double z = (ch * cd * cl) + (sd * sl);
        //    //double r = Math.Sqrt((x * x) + (y * y));

        //    //return RangeAlt(Math.Atan2(z, r) * SharedResources.RAD_DEG);
        //}

        //private static double CalculateAzimuth(double rightAscension, double declination, double latitude, double longitude)
        //{
        //    double azimuth;
        //    CalculateAltAzm(rightAscension, declination, latitude, longitude, out azimuth);
        //    return azimuth;

        //    //double lst = LocalSiderealTime(longitude * SharedResources.RAD_DEG); // Hours
        //    //double ha = (lst - rightAscension * SharedResources.RAD_HRS) * SharedResources.HRS_RAD;  // Radians

        //    //double sh = Math.Sin(ha);
        //    //double ch = Math.Cos(ha);
        //    //double sd = Math.Sin(declination);
        //    //double cd = Math.Cos(declination);
        //    //double sl = Math.Sin(latitude);
        //    //double cl = Math.Cos(latitude);

        //    //double x =  (sd * cl) - (ch * cd * sl);
        //    //double y = -(sh * cd);

        //    //return RangeAzimuth(Math.Atan2(y, x) * SharedResources.RAD_DEG);
        //}

        /// <summary>
        /// calculate the altitude and azimuth at the current time, units are hours and degrees
        /// </summary>
        /// <param name="rightAscension">hours</param>
        /// <param name="declination">degrees</param>
        /// <param name="latitude">degrees</param>
        /// <param name="longitude">degrees</param>
        /// <param name="azimuth">out degrees</param>
        /// <returns></returns>
        public static double CalculateAltAzm(double rightAscension, double declination, double latitude, double longitude, out double azimuth)
        {
            double ha = (TelescopeHardware.SiderealTime - rightAscension) * HRS_RAD;  // Radians
            double dec = declination * DEG_RAD;
            double lat = latitude * DEG_RAD;

            double sh = Math.Sin(ha);
            double ch = Math.Cos(ha);
            double sd = Math.Sin(dec);
            double cd = Math.Cos(dec);
            double sl = Math.Sin(lat);
            double cl = Math.Cos(lat);

            double x = (sd * cl) - (ch * cd * sl);
            double y = -(sh * cd);
            double z = (ch * cd * cl) + (sd * sl);
            double r = Math.Sqrt((x * x) + (y * y));

            azimuth = RangeAzimuth(Math.Atan2(y, x) * RAD_DEG);
            return RangeAlt(Math.Atan2(z, r) * RAD_DEG);
        }

        /// <summary>
        /// Calculate Altitude and Azimuth from RA and declination
        /// </summary>
        /// <param name="rightAscension"></param>
        /// <param name="declination"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public static Vector CalculateAltAzm(double rightAscension, double declination, double latitude)
        {

            double lst = TelescopeHardware.SiderealTime;      // Hours
            double ha = (lst - rightAscension) * HRS_RAD;  // Radians
            double dec = declination * DEG_RAD;
            double lat = latitude * DEG_RAD;

            double sh = Math.Sin(ha);
            double ch = Math.Cos(ha);
            double sd = Math.Sin(dec);
            double cd = Math.Cos(dec);
            double sl = Math.Sin(lat);
            double cl = Math.Cos(lat);

            double x = (sd * cl) - (ch * cd * sl);
            double y = -(sh * cd);
            double z = (ch * cd * cl) + (sd * sl);
            double r = Math.Sqrt((x * x) + (y * y));

            return new Vector(RangeAzimuth(Math.Atan2(y, x) * RAD_DEG), RangeAlt(Math.Atan2(z, r) * RAD_DEG));
        }

        /// <summary>
        /// Return an hour angle in the range -12 to +12 hours
        /// </summary>
        /// <param name="hourAngle">Value to range</param>
        /// <returns>Hour angle in the range -12 to +12 hours</returns>
        public static double RangeHA(double hourAngle)
        {
            while ((hourAngle >= 12.0) || (hourAngle <= -12.0))
            {
                if (hourAngle <= -12.0) hourAngle += 24.0;
                if (hourAngle >= 12.0) hourAngle -= 24.0;
            }

            return hourAngle;
        }


        //----------------------------------------------------------------------------------------
        // Range RA and DEC
        //----------------------------------------------------------------------------------------
        /// <summary>
        /// return right ascension in the range 0 to 24.0 hr
        /// </summary>
        /// <param name="rightAscension">The right ascension.</param>
        /// <returns></returns>
        public static double RangeRA(double rightAscension)
        {
            while ((rightAscension >= 24.0) || (rightAscension < 0.0))
            {
                if (rightAscension < 0.0) rightAscension += 24.0;
                if (rightAscension >= 24.0) rightAscension -= 24.0;
            }

            return rightAscension;
        }

        /// <summary>
        /// returns the dec in the range -90 to 90
        /// </summary>
        /// <param name="declination">The declination.</param>
        /// <returns></returns>
        public static double RangeDec(double declination)
        {
            while ((declination > 90.0) || (declination < -90.0))
            {
                if (declination < -90.0) declination += 180.0;
                if (declination > 90.0) declination = 180.0 - declination;
            }
            return declination;
        }

        public static double RangeAlt(double altitude)
        {
            return RangeDec(altitude);
        }

        public static double RangeAzimuth(double azimuth)
        {
            while ((azimuth >= 360.0) || (azimuth < 0.0))
            {
                if (azimuth < 0.0) azimuth += 360.0;
                if (azimuth >= 360.0) azimuth -= 360.0;
            }

            return azimuth;
        }

        /// <summary>
        /// Calculate RA/Dec vector from AltAz vector - Return RA and Dec in degrees
        /// </summary>
        /// <param name="targetAltAzm"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns>RA/Dec vector in degrees</returns>
        internal static Vector CalculateRaDec(Vector targetAltAzm, double latitude)
        {
            Vector raDec = new Vector();
            double ra = AstronomyFunctions.CalculateRA(targetAltAzm.Y, targetAltAzm.X, latitude);  // Returned hour angle is in sidereal hours

            // Convert RA in sidereal hours to degrees
            raDec.X = ra * HOURS_TO_DEGREES; // Degrees per SI second

            raDec.Y = AstronomyFunctions.CalculateDec(targetAltAzm.Y, targetAltAzm.X, latitude);
            return raDec;
        }

        /// <summary>
        /// Calculate HA/Dec vector from AltAz vector - Return RA and Dec in degrees
        /// </summary>
        /// <param name="targetAltAzm"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns>HA/Dec vector in degrees</returns>
        internal static Vector CalculateHaDec(Vector targetAltAzm, double latitude, double longitude)
        {
            Vector raDec = new Vector();
            double ra = AstronomyFunctions.CalculateRA(targetAltAzm.Y, targetAltAzm.X, latitude);

            raDec.X = AstronomyFunctions.HourAngle(ra, longitude) * HOURS_TO_DEGREES * SIDEREAL_SECONDS_TO_SI_SECONDS;

            raDec.Y = AstronomyFunctions.CalculateDec(targetAltAzm.Y, targetAltAzm.X, latitude);
            return raDec;
        }
    }
}
