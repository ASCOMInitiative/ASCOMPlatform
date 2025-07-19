using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.DeviceHub
{
    /// <summary>
    /// Represents the name and axis offsets of a telescope.
    /// </summary>
    /// <remarks>This structure encapsulates the name of the telescope and its offsets from two specific axes:
    /// the axis intersection and the declination/altitude axis.</remarks>
    struct TelescopeOffsets
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TelescopeOffsets"/> class with the specified name and offset
        /// values.
        /// </summary>
        /// <param name="name">The name of the telescope.</param>
        /// <param name="offsetFromAxisIntersection">The offset in mm from the axis intersection point.</param>
        /// <param name="offsetFromDecAltAxis">The offset, in mm, from the declination or altitude axis. </param>
        public TelescopeOffsets(string name, int offsetFromAxisIntersection, int offsetFromDecAltAxis)
        {
            TelescopeName = name;
            OffsetFromAxisIntersection = offsetFromAxisIntersection;
            OffsetFromDecAltAxis = offsetFromDecAltAxis;
        }

        /// <summary>
        /// Gets or sets the name of the telescope.
        /// </summary>
        public string TelescopeName;

        /// <summary>
        /// Represents the offsets of the telescope from the axis intersection and declination/altitude axis.
        /// </summary>
        public int OffsetFromAxisIntersection;

        /// <summary>
        /// Represents the offset value from the Altitude or Declination axis.
        /// </summary>
        public int OffsetFromDecAltAxis;
    }
}
