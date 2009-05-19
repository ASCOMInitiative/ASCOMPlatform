using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Usno
	{
	/// <summary>
	/// data for the observer's location.  The atmospheric
	/// parameters are used only by the refraction 
	/// function called from function 'equ_to_hor'.
	/// Additional parameters can be added to this 
	/// structure if a more sophisticated refraction model 
	/// is employed.
	/// </summary>
	public class site_info
		{
		/// <summary>
		/// geodetic latitude in degrees; north positive.
		/// </summary>
		double latitude { get; set; }
		/// <summary>
		/// geodetic longitude in degrees; east positive.
		/// </summary>
		double longitude { get; set; }
		/// <summary>
		/// height of the observer in meters.
		/// </summary>
		double height { get; set; }
		/// <summary>
		/// temperature (degrees Celsius).
		/// </summary>
		double temperature { get; set; }
		/// <summary>
		/// atmospheric pressure (millibars)
		/// </summary>
		double pressure { get; set; }
		}
	}
