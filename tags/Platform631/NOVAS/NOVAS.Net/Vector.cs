using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiGra.Astronomy
	{
	/// <summary>
	/// Describes a point in arbitrary 3D coordinate space. Distances are usually (but not necessarily)
	/// expressed in Astronomical Units (AU).
	/// </summary>
	public class Vector
		{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector"/> class.
		/// Sets all coordinates to the Origin (0,0,0).
		/// </summary>
		public Vector()
			{
			X = 0;
			Y = 0;
			Z = 0;
			}
		}
	}
