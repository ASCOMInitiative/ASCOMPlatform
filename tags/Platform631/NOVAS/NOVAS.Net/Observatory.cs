using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiGra.Astronomy;
using Usno;

namespace TiGra.Astronomy
	{
	/// <summary>
	/// data for the observer's location.  The atmospheric
	/// parameters are used only by the refraction 
	/// function called from function 'equ_to_hor'.
	/// Additional parameters can be added to this 
	/// structure if a more sophisticated refraction model 
	/// is employed.
	/// </summary>
	public class Observatory
		{
		/// <summary>
		/// Creates a new instance of the <see cref="site_info"/> class initialised with default values.
		/// The site is initialised for a theoretical observatory at sea level,
		/// at the intersection of the equator and the prime meridian, and having 'standard'
		/// atmospheric Temperature and Pressure.
		/// </summary>
		public static Observatory Default
			{
			get
				{
				Observatory defaultSite = new Observatory();
				defaultSite.Location = new GeographicCoordinates();
				defaultSite.Location.Lattitude = new Latitude(0.0);
				defaultSite.Location.Longitude = new Longitude(0.0);
				defaultSite.Height = 0;
				defaultSite.Temperature = Constants.StandardTemperature;
				defaultSite.Pressure = Constants.StandardPressure;
				return defaultSite;
				}
			}
		/// <summary>
		/// Geodetic coordinates, north & east positive; south & west negative.
		/// </summary>
		public TiGra.Astronomy.GeographicCoordinates Location { get; set; }
		/// <summary>
		/// Height of the observer in meters.
		/// </summary>
		public double Height { get; set; }
		/// <summary>
		/// Temperature (degrees Celsius).
		/// </summary>
		public double Temperature { get; set; }
		/// <summary>
		/// atmospheric Pressure (millibars)
		/// </summary>
		public double Pressure { get; set; }
		}
	}
