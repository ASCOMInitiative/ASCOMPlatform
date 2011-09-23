using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiGra.ExtensionMethods;

namespace Usno
	{
	/// <summary>
	/// the astrometric catalog data for a star; equator
	/// and equinox and units will depend on the catalog.
	/// While this structure can be used as a generic
	/// container for catalog data, all high-level 
	/// NOVAS-C functions require J2000.0 catalog data 
	/// with FK5-type units (shown in square brackets
	/// below).
	/// </summary>
	class cat_entry
		{
		/// <summary>
		/// 3-character catalog designator.
		/// </summary>
		string catalog { get; set; }
		/// <summary>
		/// name of star.
		/// </summary>
		string starname { get; set; }
		/// <summary>
		/// integer identifier assigned to star.
		/// </summary>
		long starnumber { get; set; }
		/// <summary>
		/// mean right ascension [hours].
		/// </summary>
		double ra { get; set; }
		/// <summary>
		/// mean declination [degrees].
		/// </summary>
		double dec { get; set; }
		/// <summary>
		/// proper motion in RA [seconds of time per century].
		/// </summary>
		double promora { get; set; }
		/// <summary>
		/// proper motion in declination [arcseconds per century].
		/// </summary>
		double promodec { get; set; }
		/// <summary>
		/// parallax [arcseconds].
		/// </summary>
		double parallax { get; set; }
		/// <summary>
		/// radial velocity [kilometers per second].
		/// </summary>
		double radialvelocity { get; set; }
		}
	}
