using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiGra.Astronomy
	{
	class EquatorialSphericalCoordinate
		{
		/// <summary>
		/// Gets or sets the right ascension component.
		/// </summary>
		/// <value>The right ascension.</value>
		public RightAscension RightAscension { get; set; }

		/// <summary>
		/// Gets or sets the declination component.
		/// </summary>
		/// <value>The declination.</value>
		public Declination Declination { get; set; }
		}
	}
