using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Usno.Test
	{
	public static class ExtensionMethods
		{
		/// <summary>
		/// Determines if two <see cref="DateTime"/> objects are within
		/// a <see cref="TimeSpan"/> of <c>delta</c> of each other.
		/// The chronological order of the comparison objects is not significant.
		/// </summary>
		/// <param name="dt1">The first date & time for comparison.</param>
		/// <param name="dt2">The second date & time for comparison.</param>
		/// <param name="delta">The tolerance window within which the arguments will be considered equal.</param>
		/// <returns><c>false</c> if the difference in the two input arguments is more than delta.</returns>
		public static bool IsWithin(this DateTime dt1, DateTime dt2, long delta)
			{
			// Find the magnitude of the difference (difference is always positive or zero).
			TimeSpan difference = dt1 > dt2 ? (dt1 - dt2) : (dt2 - dt1);
			if (difference.Ticks > delta)
				return false;
			return true;
			}
		}
	}
