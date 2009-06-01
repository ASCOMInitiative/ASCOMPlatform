using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using TiGra.ExtensionMethods;

namespace Usno
	{
	public sealed class Novas
		{
		public static double PSI_COR = 0.0;
		public static double EPS_COR = 0.0;

		/// <summary>
		/// Computes atmospheric refraction in zenith distance, given atmospheric Temperature and Pressure.
		/// This version computes approximate refraction for optical wavelengths.
		/// Refraction value is calculates for zenith angles from 0.1 to 91.0 degrees.
		/// Outside of that range, refraction is defined to be zero.
		/// </summary>
		/// <param name="zenithDistanceObserved">Observed zenith distance, in degrees.</param>
		/// <param name="Temperature">Atmospheric Temperature at observing site, in degrees Celsius.</param>
		/// <param name="Pressure">Atmospheric Pressure at observing site, in ???.</param>
		/// <returns>Atmospheric refraction, in degrees.</returns>
		/// <remarks>
		/// <list type="numbered">
		/// <listheader>REFERENCES</listheader>
		/// <item>
		///	 Explanatory Supplement to the Astronomical Almanac, p. 144.
		///		Bennett (1982), Journal of Navigation (Royal Institute) 35, 
		///		pp. 255-259.
		///	</item>
		///	</list>
		/// <list type="numbered">
		/// <listheader>NOTES</listheader>
		/// <item>
		/// This function can be used for planning observations or 
		/// telescope pointing, but should not be used for the reduction
		/// of precise observations.
		/// </item>
		/// <item>
		/// This function is derived from the "C" version of Fortran NOVAS routine
		/// 'refrac' written by G. H. Kaplan (USNO).
		/// </item>
		///	</list>
		/// </remarks>
		public static double refract(double zenithDistanceObserved, double temperature, double pressure)
			{
			// Validity checks. zenithDistanceObserved must be positive.
			if (zenithDistanceObserved < 0.0)
				throw new ArgumentOutOfRangeException("zd_obs", "Zenith angle must be positive");
			// Compute refraction only for zenith distances between 0.1 and 91 degrees.
			if ((zenithDistanceObserved < 0.1) || (zenithDistanceObserved > 91.0))
				return 0.0;

			double horizonDistance = 90.0 - zenithDistanceObserved;
			double r = (0.016667 / Math.Tan((horizonDistance + 7.31 / (horizonDistance + 4.4)))).DegreesToRadians();
			double refraction = r * (0.28 * pressure / (temperature + 273.0));
			return refraction;
			}
		/// <summary>
		/// Computes atmospheric refraction in zenith distance, assuming
		/// standard atmospheric Temperature and Pressure for a site at sea level.
		/// This version computes approximate refraction for optical wavelengths.
		/// Refraction value is calculates for zenith angles from 0.1 to 91.0 degrees.
		/// Outside of that range, refraction is defined to be zero.
		/// </summary>
		/// <param name="zenithDistanceObserved">Observed zenith distance, in degrees.</param>
		/// <returns>Atmospheric refraction, in degrees.</returns>
		/// <remarks>
		/// <seealso cref="refract(double, double, double)"/>
		/// <list type="numbered">
		/// <listheader>NOTES</listheader>
		/// <item>
		/// This function can be used for planning observations or 
		/// telescope pointing, but should not be used for the reduction
		/// of precise observations.
		/// </item>
		///	</list>
		/// </remarks>
		public static double refract(double zenithDistanceObserved)
			{
			return refract(zenithDistanceObserved, Constants.StandardTemperature, Constants.StandardPressure);
			}
		/// <summary>
		/// Computes atmospheric refraction in zenith distance, for a specified observing site.
		/// the observing site supplies Temperature and Pressure data for the refraction calculation.
		/// This version computes approximate refraction for optical wavelengths.
		/// Refraction value is calculates for zenith angles from 0.1 to 91.0 degrees.
		/// Outside of that range, refraction is defined to be zero.
		/// </summary>
		/// <param name="zenithDistanceObserved">Observed zenith distance, in degrees.</param>
		/// <param name="location">Site information which must contain valid Temperature and Pressure readings.</param>
		/// <returns>Atmospheric refraction, in degrees.</returns>
		/// <remarks>
		/// <seealso cref="refract(double, double, double)"/>
		/// <list type="numbered">
		/// <listheader>NOTES</listheader>
		/// <item>
		/// This function can be used for planning observations or 
		/// telescope pointing, but should not be used for the reduction
		/// of precise observations.
		/// </item>
		///	</list>
		/// </remarks>
		public static double refract(double zenithDistanceObserved, site_info location)
			{
			return refract(zenithDistanceObserved, location.Temperature, location.Pressure);
			}
		/// <summary>
		/// Computes atmospheric refraction in zenith distance, assuming
		/// standard atmospheric Temperature and Pressure for a site at
		/// <paramref name="Height"/> metres above sea level.
		/// This version computes approximate refraction for optical wavelengths.
		/// Refraction value is calculates for zenith angles from 0.1 to 91.0 degrees.
		/// Outside of that range, refraction is defined to be zero.
		/// </summary>
		/// <param name="zenithDistanceObserved">Observed zenith distance, in degrees.</param>
		/// <param name="Height">The Height of the observing site above sea level, in metres.</param>
		/// <returns>Atmospheric refraction, in degrees.</returns>
		/// <remarks>
		/// <seealso cref="refract(double, double, double)"/>
		/// <list type="numbered">
		/// <listheader>NOTES</listheader>
		/// <item>
		/// This function can be used for planning observations or 
		/// telescope pointing, but should not be used for the reduction
		/// of precise observations.
		/// </item>
		///	</list>
		/// </remarks>
		public static double refract(double zenithDistanceObserved, double height)
			{
			double locationPressure = Constants.StandardPressure * Math.Exp(-height / Constants.AtmosphereScaleHeight);
			return refract(zenithDistanceObserved, Constants.StandardTemperature, locationPressure);
			}

		/// <summary>
		/// Computes a date on the Gregorian calendar given the Julian date.
		/// </summary>
		/// <param name="tjd">The julian date.</param>
		/// <returns>
		/// Returns a <see cref="DateTime"/> containing the equivalent Gregorian
		/// date with its <see cref="DateTime.Kind"/> field
		/// set to <see cref="DateTimeKind.Utc"/>.
		/// </returns>
		/// <remarks>
		/// <list type="numbered">
		/// <listheader>REFERENCES</listheader>
		/// <item>
		///	  Fliegel & Van Flandern, Comm. of the ACM, Vol. 11, No. 10,
		///		 October 1968, p. 657.
		///	</item>
		///	</list>
		/// <list type="numbered">
		/// <listheader>NOTES</listheader>
		/// <item>This routine valid for any 'jd' greater than zero.</item>
		/// <item>Input julian date can be based on any UT-like time scale
		///	  (UTC, UT1, TT, etc.) - output time value will have same basis.</item>
		///	  <item>This function is the "C#" version of Fortran NOVAS routine 'caldat'.</item>
		/// </list>
		/// </remarks>
		public static DateTime cal_date(double tjd)
			{
			long jd;
			long k;
			long m;
			long n;

			double djd;

			djd = tjd + 0.5;
			jd = (long)djd;

			//double hour = Math.IEEERemainder(djd, 1.0) * 24.0;
			double hour = djd % 1.0 * 24.0;

			k = jd + 68569L;
			n = 4L * k / 146097L;

			k = k - (146097L * n + 3L) / 4L;
			m = 4000L * (k + 1L) / 1461001L;
			k = k - 1461L * m / 4L + 31L;

			short month = (short)(80L * k / 2447L);
			short day = (short)(k - 2447L * (long)month / 80L);
			k = (long)month / 11L;

			month = (short)((long)month + 2L - 12L * k);
			short year = (short)(100L * (n - 49L) + m + k);
			// Create a DateTime object relative to UTC and return it.
			DateTime greg = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
			return greg.AddHours(hour);
			}

		/// <summary>
		/// Computes the Julian date for a given gregorian calendar date and time.
		/// </summary>
		/// <param name="dt">
		/// A <see cref="DateTime"/> containing the calendar date to be converted.
		/// If the date is in local time, it is converted to UTC prior to calculating
		/// the Julian date.
		/// </param>
		/// <returns>Returns the corresponding Julian date.</returns>
		/// <remarks>
		/// <list type="numbered">
		/// <listheader>REFERENCES</listheader>
		/// <item>
		///	  Fliegel & Van Flandern, Comm. of the ACM, Vol. 11, No. 10,
		///		 October 1968, p. 657.
		///	</item>
		/// <list type="numbered">
		/// <listheader>NOTES</listheader>
		///	  <item>This function is the "C#" version of Fortran NOVAS routine 'juldat'.</item>
		///	  <item>No checks are made for validity on the supplied calendar date.</item>
		/// </list>
		/// </remarks>
		public static double julian_date(DateTime dt)
			{
			DateTime utc = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
			long jd12h;
			double tjd;
			double hour = utc.TimeOfDay.TotalHours;
			//double altHour = ((double)(utc.Ticks - utc.Date.Ticks)) / (double)TimeSpan.TicksPerHour;

			jd12h = (long)utc.Day - 32075L + 1461L * ((long)utc.Year + 4800L
			   + ((long)utc.Month - 14L) / 12L) / 4L
			   + 367L * ((long)utc.Month - 2L - ((long)utc.Month - 14L) / 12L * 12L)
			   / 12L - 3L * (((long)utc.Year + 4900L + ((long)utc.Month - 14L) / 12L)
			   / 100L) / 4L;
			tjd = (double)jd12h - 0.5 + hour / 24.0;

			return (tjd);
			}
		}
	}
