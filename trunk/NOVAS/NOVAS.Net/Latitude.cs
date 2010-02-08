using System;

namespace TiGra.Astronomy
{
	/// <summary>
	/// Geographical lattitude, in degrees from the equator, in the range +90 to -90.
	/// Northern lattitudes are positive, southern lattitudes are negative.
	/// Stored internally as a double, in the range +90 to -90.
	/// </summary>
	public class Latitude : Bearing
	{
		/// <summary>
		/// Default constructor for lattitude
		/// </summary>
		public Latitude()
		{
			m_Angle = 0.0;
		}

		/// <summary>
		/// Construct a lattitude from decimal degrees.
		/// </summary>
		/// <param name="dAngle">Angle used to initialise the lattitude.</param>
		public Latitude(double dAngle)
		{
			m_Angle = Normalize(dAngle);
		}
	    /// <summary>
	    /// Ensures that the supplied argument is in the correct range for a geographic lattitude.
	    /// </summary>
	    /// <param name="dAngle">Angular distance from the equator.</param>
	    /// <returns>
        /// An orthoganal value in the correct range for geographic latitudes
        /// (North positive, range +/- 90 degrees).
        /// </returns>
		public override double Normalize(double dAngle)
		{
			dAngle %= 180.0;	// Rationalise the angle to the correct hemisphere.
			if (dAngle > 90.0)
			{
				dAngle = 90 - (dAngle - 90);
			}
			if (dAngle < -90.0)
			{
				dAngle = -90 - (90 + dAngle);
			}
			return dAngle;
		}
	    /// <summary>
        /// Ensures that the supplied argument is in the correct range for a geographic lattitude.
	    /// </summary>
	    /// <param name="nDegrees">Angular distance from the equator in whole degrees.</param>
	    /// <returns>
        /// The geographic Latitude corresponding to the supplied angle, North positive, range +/- 90 degrees.
        /// </returns>
		public override int Normalize(int nDegrees)
		{
			// TODO:  Add Lattitude.Normalize implementation
			double dAngle = this.Normalize((double)nDegrees);
			
			// Truncate towards zero ( 1.5 -> 1.0; -1.5 -> -1.0)
			if (dAngle >= 0)	// Positive angles
			{
				return (int)Math.Floor(dAngle);
			}
			else
			{
				return (int)Math.Ceiling(dAngle);
			}
		}

		/// <summary>
		/// True if the lattitude is in the Northern hemisphere.
        /// Latitudes exactly on the equator are considered to be in the Northern hemisphere.
		/// </summary>
		public bool IsNorth
		{
			get
			{
				return this.m_Angle >= 0.0;
			}
		}

		/// <summary>
		/// True if the lattitude is in the Southern hemisphere
		/// </summary>
		public bool IsSouth
		{
			get
			{
				return this.m_Angle < 0.0;
			}
		}
	
		/// <summary>
		/// Output the lattitude as a formatted string h dd°mm'ss"
		/// (h is the hemisphere, N or S, dd is positive degrees).
		/// </summary>
		/// <returns>string object containing formatted result</returns>
		public override string ToString()
		{
			string strLat;
			int nDegrees;
			if (this.IsNorth)
			{
				strLat = "N ";
				nDegrees = this.Degrees;
			}
			else
			{
				strLat = "S ";
				nDegrees = -this.Degrees;
			}
			strLat += nDegrees.ToString("D2") +"°" +
				this.Minutes.ToString("D2") +"'" +
				this.Seconds.ToString("D2") +"\"";
			return strLat;
		}
	}
}
