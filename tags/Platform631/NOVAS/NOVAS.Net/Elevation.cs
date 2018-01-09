using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiGra.Astronomy
	{
	/// <summary>
	/// Describes an angular elevation above the theoretical horizon.
	/// </summary>
	class Elevation : Bearing
		{
		/// <summary>
		/// Initializes a new instance of the <see cref="Elevation"/> class
		/// and sets the elevation angle to zero (the horizon).
		/// </summary>
		public Elevation()
			{
			this.m_Angle = 0.0;
			}
		/// <summary>
		/// Initializes a new instance of the <see cref="Elevation"/> class
		/// and sets the elevation to <paramref name="elevation"/> decimal degrees above the horizon.
		/// </summary>
		/// <param name="elevation">The elevation, in decimal degrees above the horizon.</param>
		public Elevation(double elevation)
			{
			this.m_Angle = elevation;
			}
		/// <summary>
		/// Initializes a new instance of the <see cref="Elevation"/> class
		/// by converting from the specified <see cref="ZenithDistance"/> object.
		/// </summary>
		/// <param name="zenithDistance">The zenith distance.</param>
		public Elevation(ZenithDistance zenithDistance)
			{
			this.m_Angle = 90.0 - zenithDistance.DecimalDegrees;
			}
		/// <summary>
		/// Takes an angle (which can be any double number) and returns the
		/// equivalent angle in the range +0.0 to +90
		/// </summary>
		/// <param name="dAngle">The angular distance from the theoretical horizon, in degrees.</param>
		/// <returns>
		/// Returns the orthogonal elevation in the range 0 to +90 degrees.
		/// </returns>
		public override double Normalize(double dAngle)
			{
			if (dAngle < 0.0)
				throw new ArgumentOutOfRangeException("dAngle", "Can't construct an elevation from a negative number");
			dAngle = base.Normalize(dAngle);	// Range 0 to 360 degrees.
			if (dAngle > 180.0)
				throw new ArgumentOutOfRangeException("dAngle", "Can't construct an elevation from angles greater than 180 degrees");
			// Once the bearing from the horizon exceeds 90 degrees,
			// then the elevation begins to decrease as the angle increases.
			if (dAngle > 90.0)
				dAngle = 180.0 - dAngle;
			return dAngle;
			}
		}
	}
