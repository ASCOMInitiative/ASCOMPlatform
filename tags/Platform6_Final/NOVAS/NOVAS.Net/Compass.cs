using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace TiGra.Astronomy
{
	
	/// <summary>
	/// Enumerate the sixteen major compass points
	/// </summary>
	public enum Point
	{
		North, East, South, West,
		NorthEast, SouthEast, SouthWest, NorthWest,
		NorthNorthEast, EastNorthEast, NorthSouthEast, SouthSouthEast,
		SouthSouthWest, WestSouthWest, WestNorthWest, NorthNorthWest
	}

	/// <summary>
	/// Implements a Compass bearing.
	/// Angles are stored internally as double preceision floating point,
	/// values are positive, in the range 0 to 360 degrees.
	/// The angle can be initialised or set to a negative value, but it is always reduced
	/// to an equivalent positive angle before it is stored. Thus, the retrieved value
	/// must always be positive and in the range 0..360
	/// </summary>
	public class Bearing
	{
		/// <summary>
		/// Default constructor, set the angle to 0.0
		/// </summary>
		public Bearing()
		{
			m_Angle = 0.0;
		}
		/// <summary>
		/// Construct a bearing from decimal degrees.
		/// Any valid decimal value may be supplied, and it will be converted to the
		/// equivalent positive angle in the range 0..360 degrees.
		/// </summary>
		/// <param name="dAngle">The angle value in decimal degrees.</param>
		public Bearing(double dAngle)
		{
			m_Angle = Normalize(dAngle);
		}
		/// <summary>
		/// Construct a bearing from degrees, minutes and seconds.
		/// The sign of the angle is contained in the degrees. All components are assumed
		/// to have the same sign as the degrees.
		/// </summary>
		/// <param name="d">signed integer number of degrees. Any integer value,
		/// the value will be converted to an equivalent positive angle in the range [0..359]</param>
		/// <param name="m">unsigned integer number of minutes [0..59]</param>
		/// <param name="s">unsigned integer number of seconds [0..59]</param>
		public Bearing(int d, int m, int s)
		{
			if (m > 59 || m < 0)
				throw new ArgumentException("Value out of range (0..59)", "m");
			if (s > 59 || s < 0)
				throw new ArgumentException("Value out of range (0..59)", "s");
			SetDMS(d, m, s);
		}

		/// <summary>
		/// Construct a bearing from degrees, minutes and decimal seconds.
		/// The sign of the angle is contained in the degrees; minutes and seconds are
		/// always expressed as positive values, or an ArgumentException is thrown.
		/// Degrees can be any valid integer, minutes and seconds must be positive
		/// integers in the range [0..59].
		/// </summary>
		/// <param name="d">Degrees, signed integer.</param>
		/// <param name="m">Integral number of minutes in the range [0..59]</param>
		/// <param name="s">Decimal seconds, positive integer in the range [0 .. 59.9999]</param>
		public Bearing(int d, int m, double s)
		{
			if (m > 59 || m < 0)
				throw new ArgumentException("Value out of range (0..59)", "m");
			if (s >= 60.0 || s < 0.0)
				throw new ArgumentException("Value out of range (0..59)", "s");
			SetDMS(d, m, s);
		}

		/// <summary>
		/// An angular measurement
		/// </summary>
		protected double m_Angle = 0.0;

		/// <summary>
		/// Gets or Sets the angle or compass bearing in decimal degrees.
		/// If the angle specified is not within the range 0 to 360 degrees,
		/// it is converted to an equavalent positive angle in the range 0 to 360.
		/// </summary>
		public double DecimalDegrees
		{
			get
			{
				return m_Angle;
			}
			set
			{
				m_Angle = Normalize(value);
			}
		}

		/// <summary>
		/// Gets or Sets the number of degrees as an integer in the range 0..360.
		/// If value is outside of the range [0..360] it is first converted to
		/// the equivalent positive angle in the range [0..360].
		/// When setting the Degrees property, the minutes and seconds properties
		/// remain unaffected.
		/// </summary>
		[XmlIgnoreAttribute]
		public int Degrees
		{
			get
			{
				// Return the degrees, truncated towards zero.
				// Bearings cannot have negative values, but the functionality is provided
				// here for classes that inherit from Bearing.
				return (int)Bearing.Truncate(m_Angle);
			}
			set
			{
				int nDegrees = Normalize(value);
				m_Angle = (double)nDegrees + Math.Abs(m_Angle - Bearing.Truncate(m_Angle));
			}
		}

		/// <summary>
		/// Takes an angle (which can be any double number) and returns the
		/// equivalent angle in the range +0.0 to +360.0
		/// </summary>
		/// <param name="dAngle">Any double-precision value representing an angle or bearing.</param>
		/// <returns>Equivalent angle in the range +0.0 to +360.0</returns>
		public virtual double Normalize(double dAngle)
		{
			dAngle %= 360.0;	// All angles are modulo 360 degrees
			if (dAngle < 0.0)	// -ve angles are subtracted from 360.
				dAngle = 360.0 + dAngle;
			return dAngle;
		}

		/// <summary>
		/// Takes an angle expressed as a signed integer in degrees, and returns the
		/// equivalent positive angle in the range [0..360].
		/// <seealso cref="Normalize(double)"/>
		/// </summary>
		/// <param name="nDegrees">An angle in degrees</param>
		/// <returns>Equivalent positive angle in the range [0..360]</returns>
		public virtual int Normalize(int nDegrees)
		{
			nDegrees %= 360;	// All angles are modulo 360 degrees.
			if (nDegrees < 0)	// -ve angles are subtracted from 360.
				nDegrees = 360 + nDegrees;
			return nDegrees;
		}



		/// <summary>
		/// Output the compass bearing as a string ddd°mm'ss"
		/// </summary>
		/// <returns>string containing formatted result</returns>
		public override string ToString()
		{
			string strBearing = this.Degrees.ToString("D3") +"°" +
								this.Minutes.ToString("D2") +"'" +
								this.Seconds.ToString("D2") +"\"";
			return strBearing;
		}

		/// <summary>
		/// Gets or sets the minutes component of the bearing.
		/// Degrees and seconds remain unaffected.
		/// The value returned is truncated to the nearest lower integer.
		/// </summary>
		[XmlIgnoreAttribute]
		public int Minutes
		{
			get
			{
				return (int)Math.Abs(Bearing.Truncate((m_Angle * 60) % 60));
			}
			set
			{
				SetDMS(this.Degrees, value, this.Seconds);
			}
		}

		/// <summary>
		/// Gets or Sets the seconds component of the bearing.
		/// The degrees and minutes component remains unaffected.
		/// The value returned is truncated the the nearest lower integer.
		/// </summary>
		[XmlIgnoreAttribute]
		public int Seconds
		{
			get
			{
				return (int)Math.Abs(Math.Round(m_Angle * 3600) % 60);
			}
			set
			{
				SetDMS(this.Degrees, this.Minutes, value);
			}
		}

		/// <summary>
		/// Set the bearing from degrees, minutes and seconds. If degrees is negative
		/// or greater than 359, it will be converted to an equivalent positive angle.
		/// </summary>
		/// <param name="d">Whole degrees</param>
		/// <param name="m">Whole minutes</param>
		/// <param name="s">Whole seconds</param>
		public virtual void SetDMS(int d, int m, int s)
		{
			double ds = (double)s;
			SetDMS(d, m, ds);
		}

		/// <summary>
		/// Set the bearing from integral degrees and minutes, and
		/// decimal seconds.
		/// </summary>
		/// <param name="d">Whole degrees</param>
		/// <param name="m">Whole seconds, in the range [0..59]</param>
		/// <param name="s">Decimal seconds, positive, in the range [0..60].</param>
		public virtual void SetDMS(int d, int m, double s)
		{
			//Debug.WriteLine("SetDMS d="+d.ToString()+" m="+m.ToString()+" s="+s.ToString(), "Compass");
			if (m > 59 || m < 0)
			{
				Debug.WriteLine("Bad minutes parameter", "Compass");
				throw new ArgumentException("Parameter out of range [0..59]", "m");
			}
			if (s >= 60.0 || s < 0.0)
			{
				Debug.WriteLine("Bad seconds parameter","Compass");
				throw new ArgumentException("Parameter out of range [0..60]", "s");
			}
			bool bNegative = Math.Sign(d) < 0;

			double dd = (double)d;
			double dm = (double)m / 60;
			double ds = s / 3600;

			if (bNegative)	// If the degrees were negative, then...
			{
				dm = -dm;	// negate the minutes...
				ds = -ds;	// ...and the seconds.
			}
			m_Angle = Normalize(dd + dm + ds);
		}

		/// <summary>
		/// Gets the number of minutes in the angle as a decimal fraction.
		/// Degrees are not included in the result.
		/// The value returned is always positive (since the sign is considered to belong to the degrees)
		/// </summary>
		public double DecimalMinutes
		{
			get
			{
				return (Math.Abs(m_Angle) * 60.0) % 60.0;
			}
		}

		/// <summary>
		/// Gets the number of seconds in the angle as a decimal fraction.
		/// Degrees and minutes are not included in the result.
		/// </summary>
		public double DecimalSeconds
		{
			get
			{
				return (m_Angle * 3600.0) % 60.0;
			}
		}

		/// <summary>
		/// Truncate a decimal number to the nearest integer towards zero (i.e. remove the fractional part)
		/// </summary>
		protected static double Truncate(double d)
		{
			if (d < 0.0)
				return Math.Ceiling(d);
			else
				return Math.Floor(d);
		}
	}
}
