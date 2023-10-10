using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.Utilities
{
    /// <summary>
    /// ASCOM device type
    /// </summary>
    [Guid("2CD0676B-BEE9-412F-BC74-977E002AA12A")]
    public enum AscomDeviceTypes
    {
        /// <summary>
        /// Camera device
        /// </summary>
        Camera,

        /// <summary>
        /// CoverCalibrator device
        /// </summary>
        CoverCalibrator,

        /// <summary>
        /// Dome device
        /// </summary>
        Dome,

        /// <summary>
        /// FilterWheel device
        /// </summary>
        FilterWheel,

        /// <summary>
        /// Focuser device
        /// </summary>
        Focuser,

        /// <summary>
        /// ObservingConditions device
        /// </summary>
        ObservingConditions,

        /// <summary>
        /// Rotator device
        /// </summary>
        Rotator,

        /// <summary>
        /// SafetyMonitor device
        /// </summary>
        SafetyMonitor,

        /// <summary>
        /// Switch device
        /// </summary>
        Switch,

        /// <summary>
        /// Telescope device
        /// </summary>
        Telescope,

        /// <summary>
        /// Video device
        /// </summary>
        Video
    }
}
