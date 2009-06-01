using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiGra.Astronomy
	{
	class ZenithDistance : Bearing
		{
		public ZenithDistance(double degrees)
			{
			this.m_Angle = this.MakeOrthogonal(degrees);
			}
		/// <summary>
		/// Takes an angle (which can be any double number) and returns the
		/// equivalent angle in the range +0.0 to +180.0
		/// </summary>
		/// <param name="dAngle">Any double-precision value representing the angular distance from the zenith, in degrees.</param>
		/// <returns>
		/// Returns the orthogonal zenith distance in the range 0 to +180 degrees.
		/// </returns>
		public override double MakeOrthogonal(double dAngle)
			{
			dAngle = base.MakeOrthogonal(dAngle);	// Range 0 to 360 degrees.
			// Once the bearing from the zenith exceeds 180 degrees,
			// then the zenith distance begins to decrease as the andle increases.
			if (dAngle > 180.0)
				dAngle = 360.0 - dAngle;
			return dAngle;
			}
		}
	}
