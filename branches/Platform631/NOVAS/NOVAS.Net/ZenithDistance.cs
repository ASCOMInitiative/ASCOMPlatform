using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiGra.Astronomy
	{
	class ZenithDistance : Bearing
		{
		/// <summary>
		/// Initializes a new instance of the <see cref="ZenithDistance"/> class
		/// and sets the initial zenith distance to zero.
		/// </summary>
		public ZenithDistance()
			{
			this.m_Angle = 0.0;
			}
		/// <summary>
		/// Initializes a new instance of the <see cref="ZenithDistance"/> class
		/// and sets the initial zenith distance to <paramref name="degrees"/>.
		/// </summary>
		/// <param name="degrees">The angular distance from the zenith, in decimal degrees.</param>
		public ZenithDistance(double degrees)
			{
			this.m_Angle = this.Normalize(degrees);
			}
		/// <summary>
		/// Initializes a new instance of the <see cref="ZenithDistance"/> class by
		/// converting from the supplied <see cref="Elevation"/>.
		/// </summary>
		/// <param name="elevation">The elevation from the theoretical horizon, in degrees.</param>
		public ZenithDistance(Elevation elevation)
			{
			this.m_Angle = 90.0 - elevation.DecimalDegrees;
			}
		/// <summary>
		/// Takes an angle (which can be any double number) and returns the
		/// equivalent angle in the range +0.0 to +180.0
		/// </summary>
		/// <param name="dAngle">Any double-precision value representing the angular distance from the zenith, in degrees.</param>
		/// <returns>
		/// Returns the orthogonal zenith distance in the range 0 to +180 degrees.
		/// </returns>
		public override double Normalize(double dAngle)
			{
			dAngle = base.Normalize(dAngle);	// Range 0 to 360 degrees.
			// Once the bearing from the zenith exceeds 180 degrees,
			// then the zenith distance begins to decrease as the andle increases.
			if (dAngle > 180.0)
				dAngle = 360.0 - dAngle;
			return dAngle;
			}
		public ZenithDistance Refracted(Observatory location)
			{
			double zdRefracted = Usno.Novas.refract(this.m_Angle, location);
			return new ZenithDistance(this.m_Angle - zdRefracted);
			}
		}
	}
