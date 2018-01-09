using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiGra.Astronomy
	{
	public static class Constants
		{
		/// <summary>
		/// Multiply radians by this factor to convert to arc seconds.
		/// </summary>
		public const double RadiansToSeconds = 206264.806247096355;
		/// <summary>
		/// Multiply degrees by this factor to get radians.
		/// </summary>
		public const double DegreesToRadians = 0.017453292519943296;
		/// <summary>
		/// Multiply radians by this factor to get degrees.
		/// </summary>
		public const double RadiansToDegrees = 57.295779513082321;
		/// <summary>
		/// Multiply hours by this factor to get degrees.
		/// </summary>
		public const double HoursToDegrees = 15.0;
		/// <summary>
		/// Multiply degrees by this factor to get hours.
		/// </summary>
		public const double DegreesToHours = 1 / HoursToDegrees;

		/// <summary>
		/// Assumed standard atmospheric Pressure at mean sea level.
		/// This value is generally used when no Pressure data is available.
		/// </summary>
		public const double StandardPressure = 1010.0;
		/// <summary>
		/// Assumed standard atmospheric Temperature at mean sea level, in degrees Celsius.
		/// This value is generally used when no Temperature data is available.
		/// </summary>
		public const double StandardTemperature = 10.0;
		/// <summary>
		/// The approximate scale Height of the atmosphere in metres.
		/// Used in refraction calculations.
		/// </summary>
		public const double AtmosphereScaleHeight = 9.1e3;
		}
	/// <summary>
	/// Coordinate system origins.
	/// </summary>
	public enum Origin
		{
		/// <summary>
		/// Barycentric
		/// </summary>
		BARYC = 0,
		/// <summary>
		/// Heliocentric
		/// </summary>
		HELIOC = 1
		}
	/// <summary>
	/// Controls the type of atmospheric conditions used when calculating refraction.
	/// </summary>
	public enum RefractionOptions
		{
		/// <summary>
		/// Use 'standard' atmospheric conditions.
		/// </summary>
		Standard = 1,
		/// <summary>
		/// Use the atmospheric conditions associated with the <see cref="site_info"/>.
		/// </summary>
		FromSiteInfo = 2
		}
	}
