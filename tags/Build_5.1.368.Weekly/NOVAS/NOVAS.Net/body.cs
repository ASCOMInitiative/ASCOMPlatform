using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Usno
	{
	/// <summary>
	///	 Designates a celestial object.
	/// </summary>
	class body
		{
		/// <summary>
		/// Type of body: 0 = major planet, Sun, or Moon; 1 = minor planet.
		/// </summary>
		short type { get; set; }
		/// <summary>
		/// body number. for  <see cref="type"/>=0: Mercury = 1, ..., Pluto = 9, 
		/// Sun = 10, Moon = 11.
		/// For <see cref="type"/>=1: minor planet number.
		/// </summary>
		short number { get; set; }
		/// <summary>
		/// Name of the body.
		/// </summary>
		string name { get; set; }
		}
	}
