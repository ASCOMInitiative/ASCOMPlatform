using System;

namespace TiGra.Astronomy
{
	/// <summary>
	/// Summary description for GeographicCoordinates.
	/// </summary>
	public class GeographicCoordinates
	{
		/// <summary>
		/// Default constructor for geographical coordinates
		/// </summary>
		public GeographicCoordinates()
		{
			Lattitude = new Latitude(0.0);
			Longitude = new Longitude(0.0);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="GeographicCoordinates"/> class,
		/// from the specified decimal latitude and longitude values.
		/// </summary>
		/// <param name="latitude">The latitude.</param>
		/// <param name="longitude">The longitude.</param>
		public GeographicCoordinates(double latitude, double longitude)
			{
			this.Lattitude = new Latitude(latitude);
			this.Longitude = new Longitude(longitude);
			}
		/// <summary>
		/// Initializes a new instance of the <see cref="GeographicCoordinates"/> class
		/// from the given <see cref="Latitude"/> and <see cref="Longitude"/> objects.
		/// </summary>
		/// <param name="latitude">The geodetic latitude.</param>
		/// <param name="longitude">The geodetic longitude.</param>
		public GeographicCoordinates(Latitude latitude, Longitude longitude)
			{
			this.Lattitude = latitude;
			this.Longitude = longitude;
			}

		/// <summary>
		/// Geodetic lattitude, measured from the equator, north positive.
		/// </summary>
		public Latitude Lattitude { get; set; }
		/// <summary>
		/// Geodetic longitude, measured from the prime meridian, east positive.
		/// </summary>
		public Longitude Longitude { get; set; }
		/// <summary>
		/// Convert a geographical coordinate to string representation.
		/// </summary>
		/// <returns>string, formatted representation of the coordinates.</returns>
		public override string ToString()
		{
			string strLatLong = Lattitude.ToString() + " " + Longitude.ToString();
			return strLatLong;
		}
	}
}
