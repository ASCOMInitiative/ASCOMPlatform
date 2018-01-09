using System;

namespace TiGra.Astronomy
{
	/// <summary>
	/// Geographical longitude represented in degrees, minutes and seconds of arc,
	/// in the range -179°59'59" to +180°00'00". West is negative, East is positive.
	/// </summary>
	public class Longitude : Bearing
	{
		/// <summary>
		/// Default constructor for longitude
		/// </summary>
		public Longitude()
		{
			m_Angle = 0.0;
		}

		/// <summary>
		/// Construct a longitude from a decimal angle.
		/// </summary>
		/// <param name="dAngle">Arbitrary decimal angle in degrees</param>
		public Longitude(double dAngle)
		{
			m_Angle = Normalize(dAngle);
		}

		/// <summary>
		/// Convert an arbitrary angle to an East/West longitude in the range -179 to +180.
		/// All angles are referenced to the prime meridian at Greenwich.
		/// </summary>
		/// <param name="dAngle">Arbitrary angle in decimal degrees</param>
		/// <returns>Longitude in decimal degrees east or west of the prime meridian at Greenwich</returns>
		public override double Normalize(double dAngle)
		{
			dAngle = base.Normalize(dAngle);	// Convert to positive angle 0..360 degrees
			if (dAngle > 180.0)
			{
				dAngle = -180.0 + (dAngle - 180.0);
			}
			return dAngle;
		}
	
		/// <summary>
		/// Convert an abitrary integral angle to an integral East/West longitude in range -179 to +180
		/// All angles are referenced to the prime meridian at Greenwich.
		/// </summary>
		/// <param name="nDegrees">Arbitrary angle in whole degrees</param>
		/// <returns>Whole degrees east (+ve) or west (-ve) of Greenwich</returns>
		public override int Normalize(int nDegrees)
		{
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
		/// True if the lattitude is in the Northern hemisphere
		/// </summary>
		public bool IsEast
		{
			get
			{
				return this.m_Angle >= 0.0;
			}
		}

		/// <summary>
		/// True if the lattitude is in the Southern hemisphere
		/// </summary>
		public bool IsWest
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
			if (this.IsEast)
			{
				strLat = "E ";
				nDegrees = this.Degrees;
			}
			else
			{
				strLat = "W ";
				nDegrees = -this.Degrees;
			}
			strLat += nDegrees.ToString("D2") +"°" +
				this.Minutes.ToString("D2") +"'" +
				this.Seconds.ToString("D2") +"\"";
			return strLat;
		}
	}
}
