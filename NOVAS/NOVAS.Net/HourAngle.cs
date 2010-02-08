using System;

namespace TiGra.Astronomy
{
	/// <summary>
	/// HourAngle - angular measurement expressed in hours minutes and seconds.
	/// 24 hours is equivalent to 360 degrees.
	/// Internally, all angles are stored in degrees.
	/// </summary>
	public class HourAngle : Bearing
	{
        /// <summary>
        /// Default constructor.
        /// </summary>
		public HourAngle() : this(0.0)
		{
		}
        /// <summary>
        /// Construct and HourAngle object from degrees, minutes and seconds.
        /// </summary>
        /// <param name="d">Number of whole hours, range 0..23.</param>
        /// <param name="m">Number of minutes, range 0..59.</param>
        /// <param name="s">Number of seconds, range 0..59.</param>
		public HourAngle(int hours, int minutes, int seconds)
		{
		if (hours < 0 || hours > 23)
			throw new ArgumentOutOfRangeException("h", "Hours must be in the range 0 to 23");
		if (minutes < 0 || minutes > 59)
			throw new ArgumentOutOfRangeException("m", "Minutes must be in the range 0 to 59");
		if (seconds < 0 || seconds > 59)
			throw new ArgumentOutOfRangeException("s", "Seconds must be in the range 0 to 59");
		this.DecimalHours = hours + (minutes / 60.0) + (seconds / 3600.0);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="HourAngle"/> class using the
		/// supplied number of decimal hours.
		/// </summary>
		/// <param name="hours">The number of decimal hours and fractions of hours.</param>
		public HourAngle(double hours)
			{
			m_Angle = this.Normalize(hours) * Constants.HoursToDegrees;
			}

		/// <summary>
		/// Converts an arbitrary hour angle into the equivalent positive angle modulo 24
		/// </summary>
		/// <param name="dAngle">Arbitrary hour angle expressed in decimal hours</param>
		/// <returns>Orthogonal hour angle, positive value modulo 24.0</returns>
		public override double Normalize(double dAngle)
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
		public override int Normalize(int nHours)
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
				return Degrees * Constants.DegreesToHours;
			}
			set
			{
				this.SetDMS(value * Constants.HoursToDegrees, this.Minutes, this.Seconds);
			}
		}

		/// <summary>
		/// Gets or sets the hour angle, in hours, expressed as a decimal.
		/// </summary>
		public double DecimalHours
		{
			get
			{
				return DecimalDegrees / 15.0;
			}
			set
			{
				this.DecimalDegrees = Normalize(value) * Constants.HoursToDegrees;
			}
		}


	}
}
