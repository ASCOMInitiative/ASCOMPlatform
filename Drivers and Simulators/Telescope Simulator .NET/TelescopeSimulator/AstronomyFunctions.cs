//tabs=4
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
using System.Collections.Generic;
using System.Text;

namespace ASCOM.Simulator
{
    public static class AstronomyFunctions
    {
        private static Utilities.Util util = new ASCOM.Utilities.Util();


        //static AstronomyFunctions()
        //{
        //}

        //----------------------------------------------------------------------------------------
        // Calculate Precession
        //----------------------------------------------------------------------------------------
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
        /// Calculate the hour angle of the current pointing direction
        /// </summary>
        /// <param name="rightAscension"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static double HourAngle(double rightAscension, double longitude)
        {
            return RangeHa(LocalSiderealTime(longitude) - rightAscension) ;  // Hours
        }


        //----------------------------------------------------------------------------------------
        // Current Local Apparent Sidereal Time for Longitude
        //----------------------------------------------------------------------------------------
        public static double LocalSiderealTime(double longitude)
        {
            double days_since_j_2000 = util.JulianDate - 2451545.0;
            double t = days_since_j_2000 / 36525;
            double l1mst = 280.46061837 + 360.98564736629 * days_since_j_2000 + longitude ;
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


            double dp1 = (-17.2 * Math.Sin(om1)) - (1.32 * Math.Sin(2 * La)) - (0.23 *Math.Sin(2 * L11)) + (0.21 * Math.Sin(2 * om1));
            double de1 = (9.2 * Math.Cos(om1)) + (0.57 * Math.Cos(2 * La)) + (0.1 * Math.Cos(2 * L11)) - (0.09 * Math.Cos(2 * om1));
            double eps1 = ea1 + de1;
            double correction1 = (dp1 * Math.Cos(ea1)) / 3600;
            l1mst = l1mst + correction1;

            return RangeRa(l1mst * 24.0 / 360.0);

        }
            
        //----------------------------------------------------------------------------------------
        // Calculate RA and Dec From Altitude and Azimuth and Site
        //----------------------------------------------------------------------------------------
        public static double CalculateRa(double altitude, double azimuth, double latitude, double longitude)
        {

            double hourAngle = Math.Atan2(-Math.Sin(azimuth) * Math.Cos(altitude),
                                          -Math.Cos(azimuth) * Math.Sin(latitude) * Math.Cos(altitude) + Math.Sin(altitude) * Math.Cos(latitude))
                                          * SharedResources.RAD_HRS; 

            double lst = LocalSiderealTime(longitude * SharedResources.RAD_DEG); 

            return RangeRa(lst - hourAngle);
        }

        public static double CalculateDec(double altitude, double azimuth, double latitude)
        {
            
            return Math.Asin(Math.Cos(azimuth) * Math.Cos(latitude) * Math.Cos(altitude) + Math.Sin(latitude) * Math.Sin(altitude)) * SharedResources.RAD_DEG;
        }

        //----------------------------------------------------------------------------------------
        // Calculate Altitude and Azimuth From Ra/Dec and Site
        //----------------------------------------------------------------------------------------
        public static double CalculateAltitude(double rightAscension, double declination, double latitude, double longitude)
        {

            double lst = LocalSiderealTime(longitude * SharedResources.RAD_DEG); // Hours
            double ha = (lst - rightAscension * SharedResources.RAD_HRS) * SharedResources.HRS_RAD; //Radians

            double sh = Math.Sin(ha);
            double ch = Math.Cos(ha);
            double sd = Math.Sin(declination);
            double cd = Math.Cos(declination);
            double sl = Math.Sin(latitude);
            double cl = Math.Cos(latitude);

            double x = (sd * cl) - (ch * cd * sl);
            double y = -(sh * cd);
            double z = (ch * cd * cl) + (sd * sl);
            double r = Math.Sqrt((x * x) + (y * y));

            return RangeAlt(Math.Atan2(z, r) * SharedResources.RAD_DEG);
        }

        public static double CalculateAzimuth(double rightAscension, double declination, double latitude, double longitude)
        {
            
            double lst = LocalSiderealTime(longitude * SharedResources.RAD_DEG); // Hours
            double ha = (lst - rightAscension * SharedResources.RAD_HRS) * SharedResources.HRS_RAD;  // Radians

            double sh = Math.Sin(ha);
            double ch = Math.Cos(ha);
            double sd = Math.Sin(declination);
            double cd = Math.Cos(declination);
            double sl = Math.Sin(latitude);
            double cl = Math.Cos(latitude);

            double x =  (sd * cl) - (ch * cd * sl);
            double y = -(sh * cd);

            return RangeAzimuth(Math.Atan2(y, x) * SharedResources.RAD_DEG);
        }

        /// <summary>
        /// Return an hour angle in the range -12 to +12 hours
        /// </summary>
        /// <param name="hourAngle">Value to range</param>
        /// <returns>Hour angle in the range -12 to +12 hours</returns>
        public static double RangeHa(double hourAngle)
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
        public static double RangeRa(double rightAscension)
        {
            while ((rightAscension >= 24.0) || (rightAscension < 0.0))
            {
                if (rightAscension < 0.0) rightAscension += 24.0;
                if (rightAscension >= 24.0) rightAscension -= 24.0;
            }

            return rightAscension;
        }
        public static double RangeDec(double declination)
        {
            while ((declination > 90.0) || (declination < -90.0))
            {
                if (declination < -90.0) declination += 90.0;
                if (declination > 90.0) declination -= 90.0;
            }

            return declination;
        }

        public static double RangeAlt(double altitude)
        {
            while ((altitude > 90.0) || (altitude < -90.0))
            {
                if (altitude < -90.0) altitude += 90.0;
                if (altitude > 90.0) altitude -= 90.0;
            }

            return altitude;
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
    }
}
