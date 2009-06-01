using System;

namespace TiGra.Astronomy
{
	/// <summary>
	/// HourAngle - angular measurement expressed in hours minutes and seconds.
	/// 24 hours is equivalent to 360 degrees.
	/// </summary>
	public class HourAngle : Bearing
	{
        /// <summary>
        /// Default constructor.
        /// </summary>
		public HourAngle()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        /// <summary>
        /// Construct and HourAngle object from degrees, minutes and seconds.
        /// </summary>
        /// <param name="d">Number of whole degrees, range 0..359.</param>
        /// <param name="m">Number of minutes, range 0..59.</param>
        /// <param name="s">Number of seconds, range 0..59.</param>
		public HourAngle(int d, int m, int s)
		{
			this.SetDMS(d, m, s);
		}

		/// <summary>
		/// Converts an arbitrary hour angle into the equivalent positive angle modulo 24
		/// </summary>
		/// <param name="dAngle">Arbitrary hour angle expressed in decimal hours</param>
		/// <returns>Orthogonal hour angle, positive value modulo 24.0</returns>
		public override double MakeOrthogonal(double dAngle)
		{
			dAngle %= 24.0;		// All angles are modulo 24 hours
			if (dAngle < 0.0)	// -ve angles are subtracted from 24.
				dAngle = 24.0 + dAngle;
			return dAngle;
		}

		/// <summary>
		/// Converts an arbitrary number of whole hours to the equivalent positive angle modulo 24
		/// </summary>
		/// <param name="nHours">Arbitrary hour angle expressed as whole hours</param>
		/// <returns>Orthogonal hour angle, positive value modulo 24.</returns>
		public override int MakeOrthogonal(int nHours)
		{
			nHours %= 24;		// All angles are modulo 360 degrees.
			if (nHours < 0)	// -ve angles are subtracted from 360.
				nHours = 24 + nHours;
			return nHours;
		}

	
		/// <summary>
		/// Convert an hour angle to a string representation HHh MMm SSs
		/// where HH, MM and SS are the hour, minute and second values respectively
		/// and h, m and s are literal characters.
		/// </summary>
		/// <returns>string representation of an hour angle</returns>
		public override string ToString()
		{
			string strHrAng = this.Hours.ToString("D2") +"h " +
				this.Minutes.ToString("D2") +"m " +
				this.Seconds.ToString("D2") +"s";
			return strHrAng;
		}

		/// <summary>
		/// Gets or Sets the whole hours component of the hour angle. When setting
		/// the hours, the minutes and seconds components remain unaffected.
		/// </summary>
		public int Hours
		{
			get
			{
				return base.Degrees;
			}
			set
			{
				base.Degrees = value;
			}
		}

		/// <summary>
		/// Gets or sets the hour angle, in hours, expressed as a decimal.
		/// </summary>
		public double DecimalHours
		{
			get
			{
				return base.DecimalDegrees;
			}
			set
			{
				this.DecimalDegrees = MakeOrthogonal(value);
			}
		}


	}
}
