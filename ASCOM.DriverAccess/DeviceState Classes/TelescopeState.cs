using ASCOM.DeviceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class TelescopeState
    {
        /// <summary>
        /// 
        /// </summary>
        public TelescopeState() { }

        /// <summary>
        /// Telescope device state
        /// </summary>
        public TelescopeState(double? altitude, bool? atHome, bool? atPark, double? azimuth, double? declination, bool? isPulseGuiding, double? rightAscension, PierSide? sideOfPier, double? siderealTime, bool? slewing, bool? tracking, DateTime? utcDate, DateTime? timeStamp)
        {
            Altitude = altitude;
            AtHome = atHome;
            AtPark = atPark;
            Azimuth = azimuth;
            Declination = declination;
            IsPulseGuiding = isPulseGuiding;
            RightAscension = rightAscension;
            SideOfPier = sideOfPier;
            SiderealTime = siderealTime;
            Slewing = slewing;
            Tracking = tracking;
            UTCDate = utcDate;
            TimeStamp = timeStamp;
        }

        /// <summary>
        /// Telescope altitude
        /// </summary>
        public double? Altitude { get; set; } = null;

        /// <summary>
        /// Telescope is at home
        /// </summary>
        public bool? AtHome { get; set; } = null;

        /// <summary>
        /// Telescope is parked
        /// </summary>
        public bool? AtPark { get; set; } = null;

        /// <summary>
        /// Telescope azimuth
        /// </summary>
        public double? Azimuth { get; set; } = null;

        /// <summary>
        /// Telescope declination
        /// </summary>
        public double? Declination { get; set; } = null;

        /// <summary>
        /// Telescope is pulse guiding
        /// </summary>
        public bool? IsPulseGuiding { get; set; } = null;

        /// <summary>
        /// Telescope right ascension
        /// </summary>
        public double? RightAscension { get; set; } = null;

        /// <summary>
        /// Telescope pointing state
        /// </summary>
        public PierSide? SideOfPier { get; set; } = null;

        /// <summary>
        /// Telescope sidereal time
        /// </summary>
        public double? SiderealTime { get; set; } = null;

        /// <summary>
        /// Telescope is slewing
        /// </summary>
        public bool? Slewing { get; set; } = null;

        /// <summary>
        /// Telescope  is tracking
        /// </summary>
        public bool? Tracking { get; set; } = null;

        /// <summary>
        /// Telescope UTC date and time
        /// </summary>
        public DateTime? UTCDate { get; set; } = null;

        /// <summary>
        /// Time stamp of these status values
        /// </summary>
        public DateTime? TimeStamp { get; set; } = null;
    }
}

