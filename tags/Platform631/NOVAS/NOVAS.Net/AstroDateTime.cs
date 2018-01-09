using System;
using System.Globalization;


namespace TiGra.Astronomy
{
	/// <summary>
	/// Class AstroDateTime provides conversion functions from a DateTime object
	/// (containing a time in UTC) to various other formats.
	/// </summary>
	public class AstroDateTime
	{
		public const double JAN_1970 = 2440587.5;
		public const double SecondsPerDay = 86400.0;
		public const double JD2000 = 2451545.0;

		/// <summary>
		/// Longditude in decimal degrees
		/// </summary>
		private double m_dLongditude;
		/// <summary>
		/// Offset (in decimal hours) from Greenwich Mean Siderial Time of this longditude
		/// </summary>
		private double m_dOffset;

		public AstroDateTime()
		{
			m_dtUTC = DateTime.UtcNow;
			m_dLongditude = 0.0;
			m_dOffset = 0.0;
		}

		public AstroDateTime(Longitude lnSite)
		{
			m_dLongditude = lnSite.DecimalDegrees;
			m_dOffset = m_dLongditude / 15.0;
			m_dtUTC = DateTime.UtcNow;
		}

		public AstroDateTime(Longitude lnSite, DateTime dtUTC)
		{
			m_dLongditude = lnSite.DecimalDegrees;
			m_dOffset = m_dLongditude / 15.0;
			m_dtUTC = dtUTC;
		}


		/// <summary>
		/// Gets the current local siderial time
		/// </summary>
		public double LSTNow
		{
			get
			{
				return UTCtoLocalSiderialTime(DateTime.UtcNow);
			}
		}

		/// <summary>
		/// Takes a DateTime object representing a UTC time and returns
		/// the number of days since Noon, 1 Jan 4713 BC (Julian Days).
		/// 1 Jan 4713 0.5 is the fundamental epoch.
		/// </summary>
		/// <remarks>
		/// This code is based in part on the book "Astronomy With Your Personal Computer"
		/// ISBN 0-521-31976-5, Peter Duffett-Smith
		/// </remarks>
		public static double UTCtoJulianDays(DateTime dtUTC)
		{
			// A DateTime structure records the number of 100-nanosecond intervals that
			// have elapsed since 12:00 A.M., January 1, 0001.

			int nYears;
			int nMonths;
			double dDays;

			if (dtUTC.Month < 3)
			{
				nYears = dtUTC.Year - 1;
				nMonths = dtUTC.Month + 12;
			}
			else
			{
				nYears = dtUTC.Year;
				nMonths = dtUTC.Month;
			}
	
			double dA = Math.Floor((double)(nYears / 100));	// Number of centuries
	
			// Adjust for transition from Julian to Gregorian calendars.
			// Gregorian calendar was introduced 4th October 1582, but Pope
			// Gregory abolished the days 5th to 14th October 1582 inclusive.

			double dB = 0.0;
			if (dtUTC >= dtJulianGregorianTransition)
			{
				// Gregorian date    
				dB = 2 - dA + Math.Floor(dA / 4);
			}
	
			/* add a fraction of hours, minutes and secs to days*/
			dDays = dtUTC.Day + (double)dtUTC.Hour / 24.0 + (double)dtUTC.Minute / 1440.0 + (double)(dtUTC.Second / 86400.0);

			/* now get the JD */
			double dJulian =	Math.Floor(365.25 * (nYears + 4716.0)) + 
				Math.Floor(30.6001 * (nMonths + 1)) + dDays + dB - 1524.5;
			return (dJulian);
		}

		/// <summary>
		/// Calculate the Greenwich Mean Siderial Time for the specified Julian Date
		/// </summary>
		public static double JDtoGMST(double dJulianDays)
		{
			double sidereal;
			double dT = (dJulianDays - JD2000) / 36525.0;
        
			// GMST expressed as an angle
			sidereal = 280.46061837 + (360.98564736629 * (dJulianDays - 2451545.0)) + (0.000387933 * dT * dT) - (dT * dT * dT / 38710000.0);
    
			// Convert the angle to the equivalent positive angle [0..360]
			Bearing b = new Bearing(sidereal);
			sidereal = b.DecimalDegrees;
    
			sidereal *= 24.0 / 360.0;	// Change from degrees to hours.
        
			return(sidereal);
		}

		/// <summary>
		/// The date when the Gregorian calendar was introduced, replacing the Julian calendar (expressed as the Julian date)
		/// </summary>
		public static DateTime dtJulianGregorianTransition = new DateTime(1582, 10, 4, 0, 0, 0);

		/// <summary>
		/// Convert a DateTime object representing a UTC time to Local Siderial Time for the current longditude.
		/// </summary>
		public double UTCtoLocalSiderialTime(DateTime dtUTC)
		{
			double dJD = AstroDateTime.UTCtoJulianDays(dtUTC);
			double dGMST = AstroDateTime.JDtoGMST(dJD);
			// TODO: Account for NUTATION
			double dLST = dGMST + (m_dLongditude / 15.0);
			while (dLST < 0.0)
			{	// Guard against getting a -ve LST
				dLST += 24.0;
			}
			dLST %= 24.0;	// One final safety net, take the modulo 24 hours.
			return dLST;
		}

		/// <summary>
		/// The date and time in UTC
		/// </summary>
		public DateTime m_dtUTC;
	}
}
