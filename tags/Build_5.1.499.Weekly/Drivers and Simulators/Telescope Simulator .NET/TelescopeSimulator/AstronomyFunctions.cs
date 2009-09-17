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

namespace ASCOM.TelescopeSimulator
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
        //----------------------------------------------------------------------------------------
        // Current Local Apparent Sidereal Time for Longitude
        //----------------------------------------------------------------------------------------

        public static double LocalSiderealTime(double longitude)
        {
            //double days_since_j_2000 = DateUtcToJulian(DateTime.Now.ToUniversalTime()) - 2451545.0;
            double days_since_j_2000 = m_Util.DateUTCToJulian(DateTime.Now.ToUniversalTime()) - 2451545.0;
            double t = days_since_j_2000 / 36525;
            double l1mst = 280.46061837 + 360.98564736629 * days_since_j_2000 + longitude;
            if (l1mst < 0)
            {
                while (l1mst < 0)
                {
                    l1mst = l1mst + 360;
                }
            }
            else
            {
                while (l1mst > 360)
                {
                    l1mst = l1mst - 360;
                }
            }
            //calculate OM
            double om1 = 125.04452 - 1934.136261 * t;
            if (om1 < 0)
            {
                while (om1 < 0)
                {
                    om1 = om1 + 360;
                }
            }
            else
            {
                while (om1 > 360)
                {
                    om1 = om1 - 360;
                }
            }
            //calculat L
            double La = 280.4665 + 36000.7698 * t;
            if (La < 0)
            {
                while (La < 0)
                {
                    La = La + 360;
                }
            }
            else
            {
                while (La > 360)
                {
                    La = La - 360;
                }
            }
            //calculate L1
            double L11 = 218.3165 + 481267.8813 * t;
            if (L11 < 0)
            {
                while (L11 < 0)
                {
                    L11 = L11 + 360;
                }
            }
            else
            {
                while (L11 > 360)
                {
                    L11 = L11 - 360;
                }
            }
            //calculate e
            double ea1 = 23.439 - 0.0000004 * t;
            if (ea1 < 0)
            {
                while (ea1 < 0)
                {
                    ea1 = ea1 + 360;
                }
            }
            else
            {
                while (ea1 > 360)
                {
                    ea1 = ea1 - 360;
                }
            }


            double dp1 = (-172.2 * (Math.Sin(om1))) - (1.32 * (Math.Sin(2 * La))) + (0.21 * Math.Sin(2 * om1));
            double de1 = (9.2 * (Math.Cos(om1))) + (0.57 * (Math.Cos(2 * La))) + (0.1 * (Math.Cos(2 * La))) - (0.09 * (Math.Cos(2 * om1)));
            double eps1 = ea1 + de1;
            double correction1 = (dp1 * Math.Cos(ea1)) / 3600;
            //l1mst = l1mst + correction1;

            return l1mst;

        }

       
        
        //----------------------------------------------------------------------------------------
        // Calculate RA and Dec From Altitude and Azimuth and Site
        //----------------------------------------------------------------------------------------
        public static double CalculateRa(double Altitude, double Azimuth, double Latitude, double Longitude)
        {

            double hourAngle = Math.Atan2(-Math.Sin(Azimuth) * Math.Cos(Altitude), - Math.Cos(Azimuth) * Math.Sin(Latitude) * Math.Cos(Altitude) + Math.Sin(Altitude) * Math.Cos(Latitude)) * SharedResources.RAD_DEG;
            if (hourAngle < 0)
            { hourAngle += 360; }
            else if (hourAngle >= 360)
            { hourAngle -= 360; }
            double lst = LocalSiderealTime(Longitude * SharedResources.RAD_DEG);
            double ra =  lst - hourAngle;
            
            return ra;
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
            double lst = LocalSiderealTime(Longitude * SharedResources.RAD_DEG);
            double ha = lst * SharedResources.DEG_RAD - RightAscension;
            return Math.Asin(Math.Sin(Declination) * Math.Sin(Latitude) + Math.Cos(Declination) * Math.Cos(ha) * Math.Cos(Latitude)) * SharedResources.RAD_DEG;

        }
        public static double CalculateAzimuth(double RightAscension, double Declination, double Latitude, double Longitude)
        {
            double lst = LocalSiderealTime(Longitude * SharedResources.RAD_DEG);
            double ha = lst * SharedResources.DEG_RAD - RightAscension;

            double A1 = -Math.Cos(Declination) * Math.Sin(ha) / Math.Cos(Math.Asin(Math.Sin(Declination) * Math.Sin(Latitude) + Math.Cos(Declination) * Math.Cos(ha) * Math.Cos(Latitude)));
            double A2 = (Math.Sin(Declination) * Math.Cos(Latitude) - Math.Cos(Declination) * Math.Cos(ha) * Math.Sin(Latitude)) / Math.Cos(Math.Asin(Math.Sin(Declination) * Math.Sin(Latitude) + Math.Cos(Declination) * Math.Cos(ha) * Math.Cos(Latitude)));

            double azimuth  = Math.Atan2(A1,A2);
            if (azimuth<0)
            {
              azimuth = 2*Math.PI + azimuth;
            }

            return azimuth*SharedResources.RAD_DEG;
        }

        //----------------------------------------------------------------------------------------
        // Range RA and DEC
        //----------------------------------------------------------------------------------------
        public static double RangeHa(double RightAscension)
        {
            if (RightAscension < 0)
            {
                return 24 + RightAscension;
            }
            else if (RightAscension >= 24)
            {
                return RightAscension - 24;
            }
            else
            {
                return RightAscension;
            }
        }
        public static double RangeDec(double Declination)
        {
            if (Declination > 90)
            {
                return 90;
            }
            else if (Declination < -90)
            {
                return -90;
            }
            else
            {
                return Declination;
            }
        }
    }
}
