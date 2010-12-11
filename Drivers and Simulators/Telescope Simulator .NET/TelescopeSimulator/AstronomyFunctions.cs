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
    public class AstronomyFunctions
    {
        private static Utilities.Util m_Util = new ASCOM.Utilities.Util();

        static AstronomyFunctions()
        { }

        //----------------------------------------------------------------------------------------
        // Calculate Precession
        //----------------------------------------------------------------------------------------
        public static double Precess(DateTime datetime)
        {
            int y = datetime.Year + 1900;
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
        /// <param name="RightAscension"></param>
        /// <param name="Longitude"></param>
        /// <returns></returns>
        public static double HourAngle(double RightAscension, double Longitude)
        {
            return RangeHa(LocalSiderealTime(Longitude) - RightAscension) ;  // Hours
        }


        //----------------------------------------------------------------------------------------
        // Current Local Apparent Sidereal Time for Longitude
        //----------------------------------------------------------------------------------------
        public static double LocalSiderealTime(double longitude)
        {
            double days_since_j_2000 = m_Util.JulianDate - 2451545.0;
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
        public static double CalculateRa(double Altitude, double Azimuth, double Latitude, double Longitude)
        {

            double hourAngle = Math.Atan2(-Math.Sin(Azimuth) * Math.Cos(Altitude),
                                          -Math.Cos(Azimuth) * Math.Sin(Latitude) * Math.Cos(Altitude) + Math.Sin(Altitude) * Math.Cos(Latitude))
                                          * SharedResources.RAD_HRS; 

            double lst = LocalSiderealTime(Longitude * SharedResources.RAD_DEG); 

            return RangeRa(lst - hourAngle);
        }

        public static double CalculateDec(double Altitude, double Azimuth, double Latitude)
        {
            
            return Math.Asin(Math.Cos(Azimuth) * Math.Cos(Latitude) * Math.Cos(Altitude) + Math.Sin(Latitude) * Math.Sin(Altitude)) * SharedResources.RAD_DEG;
        }

        //----------------------------------------------------------------------------------------
        // Calculate Altitude and Azimuth From Ra/Dec and Site
        //----------------------------------------------------------------------------------------
        public static double CalculateAltitude(double RightAscension, double Declination, double Latitude, double Longitude)
        {

            double lst = LocalSiderealTime(Longitude * SharedResources.RAD_DEG); // Hours
            double ha = (lst - RightAscension * SharedResources.RAD_HRS) * SharedResources.HRS_RAD; //Radians

            double sh = Math.Sin(ha);
            double ch = Math.Cos(ha);
            double sd = Math.Sin(Declination);
            double cd = Math.Cos(Declination);
            double sl = Math.Sin(Latitude);
            double cl = Math.Cos(Latitude);

            double x = (sd * cl) - (ch * cd * sl);
            double y = -(sh * cd);
            double z = (ch * cd * cl) + (sd * sl);
            double r = Math.Sqrt((x * x) + (y * y));

            return RangeAlt(Math.Atan2(z, r) * SharedResources.RAD_DEG);
        }

        public static double CalculateAzimuth(double RightAscension, double Declination, double Latitude, double Longitude)
        {
            
            double lst = LocalSiderealTime(Longitude * SharedResources.RAD_DEG); // Hours
            double ha = (lst - RightAscension * SharedResources.RAD_HRS) * SharedResources.HRS_RAD;  // Radians

            double sh = Math.Sin(ha);
            double ch = Math.Cos(ha);
            double sd = Math.Sin(Declination);
            double cd = Math.Cos(Declination);
            double sl = Math.Sin(Latitude);
            double cl = Math.Cos(Latitude);

            double x =  (sd * cl) - (ch * cd * sl);
            double y = -(sh * cd);

            return RangeAzimuth(Math.Atan2(y, x) * SharedResources.RAD_DEG);
        }

        /// <summary>
        /// Return an hour angle in the range -12 to +12 hours
        /// </summary>
        /// <param name="HourAngle">Value to range</param>
        /// <returns>Hour angle in the range -12 to +12 hours</returns>
        public static double RangeHa(double HourAngle)
        {
            while ((HourAngle >= 12.0) || (HourAngle <= -12.0))
            {
                if (HourAngle <= -12.0) HourAngle += 24.0;
                if (HourAngle >= 12.0) HourAngle -= 24.0;
            }

            return HourAngle;
        }
        
        
        //----------------------------------------------------------------------------------------
        // Range RA and DEC
        //----------------------------------------------------------------------------------------
        public static double RangeRa(double RightAscension)
        {
            while ((RightAscension >= 24.0) || (RightAscension < 0.0))
            {
                if (RightAscension < 0.0) RightAscension += 24.0;
                if (RightAscension >= 24.0) RightAscension -= 24.0;
            }

            return RightAscension;
        }
        public static double RangeDec(double Declination)
        {
            while ((Declination > 90.0) || (Declination < -90.0))
            {
                if (Declination < -90.0) Declination += 90.0;
                if (Declination > 90.0) Declination -= 90.0;
            }

            return Declination;
        }
        public static double RangeAlt(double Alitude)
        {
            while ((Alitude > 90.0) || (Alitude < -90.0))
            {
                if (Alitude < -90.0) Alitude += 90.0;
                if (Alitude > 90.0) Alitude -= 90.0;
            }

            return Alitude;
        }
        public static double RangeAzimuth(double Azimuth)
        {
            while ((Azimuth >= 360.0) || (Azimuth < 0.0))
            {
                if (Azimuth < 0.0) Azimuth += 360.0;
                if (Azimuth >= 360.0) Azimuth -= 360.0;
            }

            return Azimuth;
        }
    }
}
