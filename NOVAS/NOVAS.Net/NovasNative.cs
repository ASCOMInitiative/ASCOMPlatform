using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Usno
	{
	public class Novas
		{
		public static double PSI_COR = 0.0;
		public static double EPS_COR = 0.0;
		/********cal_date */

		/*
		------------------------------------------------------------------------

		   PURPOSE:    

		   REFERENCES: 
			  Fliegel & Van Flandern, Comm. of the ACM, Vol. 11, No. 10,
				 October 1968, p. 657.

		   INPUT
		   ARGUMENTS:
			  tjd (double)
				 Julian date.

		   OUTPUT
		   ARGUMENTS:
			  *year (short int)
				 Year.
			  *month (short int)
				 Month number.
			  *day (short int)
				 Day-of-month.
			  *hour (double)
				 Hour-of-day.

		   RETURNED
		   VALUE:
			  None.

		   GLOBALS
		   USED:
			  None.

		   FUNCTIONS
		   CALLED:
			  fmod     math.h

		   VER./DATE/
		   PROGRAMMER:
			  V1.0/06-98/JAB (USNO/AA)

		   NOTES:
			  1. This routine valid for any 'jd' greater than zero.
			  2. Input julian date can be based on any UT-like time scale
			  (UTC, UT1, TT, etc.) - output time value will have same basis.
			  3. This function is the "C" version of Fortran NOVAS routine
			  'caldat'.


		------------------------------------------------------------------------
		*/

		/// <summary>
		/// Computes a date on the Gregorian calendar given the Julian date.
		/// </summary>
		/// <param name="tjd">The julian date.</param>
		/// <returns>
		/// Returns a <see cref="DateTime"/> containing the equivalent Gregorian
		/// date with its <see cref="DateTime.Kind"/> field
		/// set to <see cref="DateTimeKind.Utc"/>.
		/// </returns>
		public DateTime cal_date(double tjd)
			{
			long jd;
			long k;
			long m;
			long n;

			double djd;

			djd = tjd + 0.5;
			jd = (long)djd;

			double hour = Math.IEEERemainder(djd, 1.0) * 24.0;

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
		}
	}
