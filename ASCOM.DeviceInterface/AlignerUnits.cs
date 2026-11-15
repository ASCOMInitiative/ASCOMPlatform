using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// The units of movement for the device's axes.
    /// </summary>
    [Guid("A2E2F67A-1644-4C2B-97A5-5C150FA1C136")]
    [ComVisible(true)]
    public enum AlignerUnits
    {
        /// <summary>
        /// Default value allowing clients to confirm that the device has set a valid units value. Clients should report an error if a device returns this value.
        /// </summary>
        Unset = 0,

        /// <summary>
        /// Movement is measured in degrees
        /// </summary>
        Degrees = 1,

        /// <summary>
        /// Movement is measured in millimetres
        /// </summary>
        Millimeters = 2,

        /// <summary>
        /// Movement is measured in arbitrary position counts
        /// </summary>
        /// <remarks>Select this unit if the device is uncalibrated and responds to motor step counts or encoder position values.</remarks>
        Counts = 3
    }
}
