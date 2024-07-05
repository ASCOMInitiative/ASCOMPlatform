// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Astrometry.NOVAS;
using ASCOM.Astrometry;

static class TelescopeHardware
{
    static Util utilities = new Util();
    static AstroUtils astroUtilities = new AstroUtils();

    #region ITelescope Implementation

    /// <summary>
    /// Stops a slew in progress.
    /// </summary>
    internal static void AbortSlew()
    {
        LogMessage("AbortSlew", "Not implemented");
        throw new MethodNotImplementedException("AbortSlew");
    }

    /// <summary>
    /// The alignment mode of the mount (Alt/Az, Polar, German Polar).
    /// </summary>
    internal static AlignmentModes AlignmentMode
    {
        get
        {
            LogMessage("AlignmentMode Get", "Not implemented");
            throw new PropertyNotImplementedException("AlignmentMode", false);
        }
    }

    /// <summary>
    /// The Altitude above the local horizon of the telescope's current position (degrees, positive up)
    /// </summary>
    internal static double Altitude
    {
        get
        {
            LogMessage("Altitude", "Not implemented");
            throw new PropertyNotImplementedException("Altitude", false);
        }
    }

    /// <summary>
    /// The area of the telescope's aperture, taking into account any obstructions (square meters)
    /// </summary>
    internal static double ApertureArea
    {
        get
        {
            LogMessage("ApertureArea Get", "Not implemented");
            throw new PropertyNotImplementedException("ApertureArea", false);
        }
    }

    /// <summary>
    /// The telescope's effective aperture diameter (meters)
    /// </summary>
    internal static double ApertureDiameter
    {
        get
        {
            LogMessage("ApertureDiameter Get", "Not implemented");
            throw new PropertyNotImplementedException("ApertureDiameter", false);
        }
    }

    /// <summary>
    /// True if the telescope is stopped in the Home position. Set only following a <see cref="FindHome"></see> operation,
    /// and reset with any slew operation. This property must be False if the telescope does not support homing.
    /// </summary>
    internal static bool AtHome
    {
        get
        {
            LogMessage("AtHome", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if the telescope has been put into the parked state by the seee <see cref="Park" /> method. Set False by calling the Unpark() method.
    /// </summary>
    internal static bool AtPark
    {
        get
        {
            LogMessage("AtPark", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// Determine the rates at which the telescope may be moved about the specified axis by the <see cref="MoveAxis" /> method.
    /// </summary>
    /// <param name="Axis">The axis about which rate information is desired (TelescopeAxes value)</param>
    /// <returns>Collection of <see cref="IRate" /> rate objects</returns>
    internal static IAxisRates AxisRates(TelescopeAxes Axis)
    {
        LogMessage("AxisRates", "Get - " + Axis.ToString());
        return new AxisRates(Axis);
    }

    /// <summary>
    /// The azimuth at the local horizon of the telescope's current position (degrees, North-referenced, positive East/clockwise).
    /// </summary>
    internal static double Azimuth
    {
        get
        {
            LogMessage("Azimuth Get", "Not implemented");
            throw new PropertyNotImplementedException("Azimuth", false);
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed finding its home position (<see cref="FindHome" /> method).
    /// </summary>
    internal static bool CanFindHome
    {
        get
        {
            LogMessage("CanFindHome", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if this telescope can move the requested axis
    /// </summary>
    internal static bool CanMoveAxis(TelescopeAxes Axis)
    {
        LogMessage("CanMoveAxis", "Get - " + Axis.ToString());
        switch (Axis)
        {
            case TelescopeAxes.axisPrimary: return false;
            case TelescopeAxes.axisSecondary: return false;
            case TelescopeAxes.axisTertiary: return false;
            default: throw new InvalidValueException("CanMoveAxis", Axis.ToString(), "0 to 2");
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed parking (<see cref="Park" />method)
    /// </summary>
    internal static bool CanPark
    {
        get
        {
            LogMessage("CanPark", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if this telescope is capable of software-pulsed guiding (via the <see cref="PulseGuide" /> method)
    /// </summary>
    internal static bool CanPulseGuide
    {
        get
        {
            LogMessage("CanPulseGuide", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if the <see cref="DeclinationRate" /> property can be changed to provide offset tracking in the declination axis.
    /// </summary>
    internal static bool CanSetDeclinationRate
    {
        get
        {
            LogMessage("CanSetDeclinationRate", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if the guide rate properties used for <see cref="PulseGuide" /> can ba adjusted.
    /// </summary>
    internal static bool CanSetGuideRates
    {
        get
        {
            LogMessage("CanSetGuideRates", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed setting of its park position (<see cref="SetPark" /> method)
    /// </summary>
    internal static bool CanSetPark
    {
        get
        {
            LogMessage("CanSetPark", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if the <see cref="SideOfPier" /> property can be set, meaning that the mount can be forced to flip.
    /// </summary>
    internal static bool CanSetPierSide
    {
        get
        {
            LogMessage("CanSetPierSide", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if the <see cref="RightAscensionRate" /> property can be changed to provide offset tracking in the right ascension axis.
    /// </summary>
    internal static bool CanSetRightAscensionRate
    {
        get
        {
            LogMessage("CanSetRightAscensionRate", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if the <see cref="Tracking" /> property can be changed, turning telescope sidereal tracking on and off.
    /// </summary>
    internal static bool CanSetTracking
    {
        get
        {
            LogMessage("CanSetTracking", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed slewing (synchronous or asynchronous) to equatorial coordinates
    /// </summary>
    internal static bool CanSlew
    {
        get
        {
            LogMessage("CanSlew", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed slewing (synchronous or asynchronous) to local horizontal coordinates
    /// </summary>
    internal static bool CanSlewAltAz
    {
        get
        {
            LogMessage("CanSlewAltAz", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed asynchronous slewing to local horizontal coordinates
    /// </summary>
    internal static bool CanSlewAltAzAsync
    {
        get
        {
            LogMessage("CanSlewAltAzAsync", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed asynchronous slewing to equatorial coordinates.
    /// </summary>
    internal static bool CanSlewAsync
    {
        get
        {
            LogMessage("CanSlewAsync", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed syncing to equatorial coordinates.
    /// </summary>
    internal static bool CanSync
    {
        get
        {
            LogMessage("CanSync", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed syncing to local horizontal coordinates
    /// </summary>
    internal static bool CanSyncAltAz
    {
        get
        {
            LogMessage("CanSyncAltAz", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed unparking (<see cref="Unpark" /> method).
    /// </summary>
    internal static bool CanUnpark
    {
        get
        {
            LogMessage("CanUnpark", "Get - " + false.ToString());
            return false;
        }
    }

    /// <summary>
    /// The declination (degrees) of the telescope's current equatorial coordinates, in the coordinate system given by the <see cref="EquatorialSystem" /> property.
    /// Reading the property will raise an error if the value is unavailable.
    /// </summary>
    internal static double Declination
    {
        get
        {
            double declination = 0.0;
            LogMessage("Declination", "Get - " + utilities.DegreesToDMS(declination, ":", ":"));
            return declination;
        }
    }

    /// <summary>
    /// The declination tracking rate (arcseconds per SI second, default = 0.0)
    /// </summary>
    internal static double DeclinationRate
    {
        get
        {
            double declination = 0.0;
            LogMessage("DeclinationRate", "Get - " + declination.ToString());
            return declination;
        }
        set
        {
            LogMessage("DeclinationRate Set", "Not implemented");
            throw new PropertyNotImplementedException("DeclinationRate", true);
        }
    }

    /// <summary>
    /// Predict side of pier for German equatorial mounts at the provided coordinates
    /// </summary>
    internal static PierSide DestinationSideOfPier(double RightAscension, double Declination)
    {
        LogMessage("DestinationSideOfPier Get", "Not implemented");
        throw new PropertyNotImplementedException("DestinationSideOfPier", false);
    }

    /// <summary>
    /// True if the telescope or driver applies atmospheric refraction to coordinates.
    /// </summary>
    internal static bool DoesRefraction
    {
        get
        {
            LogMessage("DoesRefraction Get", "Not implemented");
            throw new PropertyNotImplementedException("DoesRefraction", false);
        }
        set
        {
            LogMessage("DoesRefraction Set", "Not implemented");
            throw new PropertyNotImplementedException("DoesRefraction", true);
        }
    }

    /// <summary>
    /// Equatorial coordinate system used by this telescope (e.g. Topocentric or J2000).
    /// </summary>
    internal static EquatorialCoordinateType EquatorialSystem
    {
        get
        {
            EquatorialCoordinateType equatorialSystem = EquatorialCoordinateType.equTopocentric;
            LogMessage("DeclinationRate", "Get - " + equatorialSystem.ToString());
            return equatorialSystem;
        }
    }

    /// <summary>
    /// Locates the telescope's "home" position (synchronous)
    /// </summary>
    internal static void FindHome()
    {
        LogMessage("FindHome", "Not implemented");
        throw new MethodNotImplementedException("FindHome");
    }

    /// <summary>
    /// The telescope's focal length, meters
    /// </summary>
    internal static double FocalLength
    {
        get
        {
            LogMessage("FocalLength Get", "Not implemented");
            throw new PropertyNotImplementedException("FocalLength", false);
        }
    }

    /// <summary>
    /// The current Declination movement rate offset for telescope guiding (degrees/sec)
    /// </summary>
    internal static double GuideRateDeclination
    {
        get
        {
            LogMessage("GuideRateDeclination Get", "Not implemented");
            throw new PropertyNotImplementedException("GuideRateDeclination", false);
        }
        set
        {
            LogMessage("GuideRateDeclination Set", "Not implemented");
            throw new PropertyNotImplementedException("GuideRateDeclination", true);
        }
    }

    /// <summary>
    /// The current Right Ascension movement rate offset for telescope guiding (degrees/sec)
    /// </summary>
    internal static double GuideRateRightAscension
    {
        get
        {
            LogMessage("GuideRateRightAscension Get", "Not implemented");
            throw new PropertyNotImplementedException("GuideRateRightAscension", false);
        }
        set
        {
            LogMessage("GuideRateRightAscension Set", "Not implemented");
            throw new PropertyNotImplementedException("GuideRateRightAscension", true);
        }
    }

    /// <summary>
    /// True if a <see cref="PulseGuide" /> command is in progress, False otherwise
    /// </summary>
    internal static bool IsPulseGuiding
    {
        get
        {
            LogMessage("IsPulseGuiding Get", "Not implemented");
            throw new PropertyNotImplementedException("IsPulseGuiding", false);
        }
    }

    /// <summary>
    /// Move the telescope in one axis at the given rate.
    /// </summary>
    /// <param name="Axis">The physical axis about which movement is desired</param>
    /// <param name="Rate">The rate of motion (deg/sec) about the specified axis</param>
    internal static void MoveAxis(TelescopeAxes Axis, double Rate)
    {
        LogMessage("MoveAxis", "Not implemented");
        throw new MethodNotImplementedException("MoveAxis");
    }


    /// <summary>
    /// Move the telescope to its park position, stop all motion (or restrict to a small safe range), and set <see cref="AtPark" /> to True.
    /// </summary>
    internal static void Park()
    {
        LogMessage("Park", "Not implemented");
        throw new MethodNotImplementedException("Park");
    }

    /// <summary>
    /// Moves the scope in the given direction for the given interval or time at
    /// the rate given by the corresponding guide rate property
    /// </summary>
    /// <param name="Direction">The direction in which the guide-rate motion is to be made</param>
    /// <param name="Duration">The duration of the guide-rate motion (milliseconds)</param>
    internal static void PulseGuide(GuideDirections Direction, int Duration)
    {
        LogMessage("PulseGuide", "Not implemented");
        throw new MethodNotImplementedException("PulseGuide");
    }

    /// <summary>
    /// The right ascension (hours) of the telescope's current equatorial coordinates,
    /// in the coordinate system given by the EquatorialSystem property
    /// </summary>
    internal static double RightAscension
    {
        get
        {
            double rightAscension = 0.0;
            LogMessage("RightAscension", "Get - " + utilities.HoursToHMS(rightAscension));
            return rightAscension;
        }
    }

    /// <summary>
    /// The right ascension tracking rate offset from sidereal (seconds per sidereal second, default = 0.0)
    /// </summary>
    internal static double RightAscensionRate
    {
        get
        {
            double rightAscensionRate = 0.0;
            LogMessage("RightAscensionRate", "Get - " + rightAscensionRate.ToString());
            return rightAscensionRate;
        }
        set
        {
            LogMessage("RightAscensionRate Set", "Not implemented");
            throw new PropertyNotImplementedException("RightAscensionRate", true);
        }
    }

    /// <summary>
    /// Sets the telescope's park position to be its current position.
    /// </summary>
    internal static void SetPark()
    {
        LogMessage("SetPark", "Not implemented");
        throw new MethodNotImplementedException("SetPark");
    }

    /// <summary>
    /// Indicates the pointing state of the mount. Read the articles installed with the ASCOM Developer
    /// Components for more detailed information.
    /// </summary>
    internal static PierSide SideOfPier
    {
        get
        {
            LogMessage("SideOfPier Get", "Not implemented");
            throw new PropertyNotImplementedException("SideOfPier", false);
        }
        set
        {
            LogMessage("SideOfPier Set", "Not implemented");
            throw new PropertyNotImplementedException("SideOfPier", true);
        }
    }

    /// <summary>
    /// The local apparent sidereal time from the telescope's internal clock (hours, sidereal)
    /// </summary>
    internal static double SiderealTime
    {
        get
        {
            double siderealTime = 0.0; // Sidereal time return value

            // Use NOVAS 3.1 to calculate the sidereal time
            using (var novas = new NOVAS31())
            {
                double julianDate = utilities.DateUTCToJulian(DateTime.UtcNow);
                novas.SiderealTime(julianDate, 0, novas.DeltaT(julianDate), GstType.GreenwichApparentSiderealTime, Method.EquinoxBased, Accuracy.Full, ref siderealTime);
            }

            // Adjust the calculated sidereal time for longitude using the value returned by the SiteLongitude property, allowing for the possibility that this property has not yet been implemented
            try
            {
                siderealTime += SiteLongitude / 360.0 * 24.0;
            }
            catch (PropertyNotImplementedException) // SiteLongitude hasn't been implemented
            {
                // No action, just return the calculated sidereal time unadjusted for longitude
            }
            catch (Exception) // Some other exception occurred so return it to the client
            {
                throw;
            }

            // Reduce sidereal time to the range 0 to 24 hours
            siderealTime = astroUtilities.ConditionRA(siderealTime);

            LogMessage("SiderealTime", "Get - " + siderealTime.ToString());
            return siderealTime;
        }
    }

    /// <summary>
    /// The elevation above mean sea level (meters) of the site at which the telescope is located
    /// </summary>
    internal static double SiteElevation
    {
        get
        {
            LogMessage("SiteElevation Get", "Not implemented");
            throw new PropertyNotImplementedException("SiteElevation", false);
        }
        set
        {
            LogMessage("SiteElevation Set", "Not implemented");
            throw new PropertyNotImplementedException("SiteElevation", true);
        }
    }

    /// <summary>
    /// The geodetic(map) latitude (degrees, positive North, WGS84) of the site at which the telescope is located.
    /// </summary>
    internal static double SiteLatitude
    {
        get
        {
            LogMessage("SiteLatitude Get", "Not implemented");
            throw new PropertyNotImplementedException("SiteLatitude", false);
        }
        set
        {
            LogMessage("SiteLatitude Set", "Not implemented");
            throw new PropertyNotImplementedException("SiteLatitude", true);
        }
    }

    /// <summary>
    /// The longitude (degrees, positive East, WGS84) of the site at which the telescope is located.
    /// </summary>
    internal static double SiteLongitude
    {
        get
        {
            LogMessage("SiteLongitude Get", "Returning 0.0 to ensure that SiderealTime method is functional out of the box.");
            return 0.0;
        }
        set
        {
            LogMessage("SiteLongitude Set", "Not implemented");
            throw new PropertyNotImplementedException("SiteLongitude", true);
        }
    }

    /// <summary>
    /// Specifies a post-slew settling time (sec.).
    /// </summary>
    internal static short SlewSettleTime
    {
        get
        {
            LogMessage("SlewSettleTime Get", "Not implemented");
            throw new PropertyNotImplementedException("SlewSettleTime", false);
        }
        set
        {
            LogMessage("SlewSettleTime Set", "Not implemented");
            throw new PropertyNotImplementedException("SlewSettleTime", true);
        }
    }

    /// <summary>
    /// Move the telescope to the given local horizontal coordinates, return when slew is complete
    /// </summary>
    internal static void SlewToAltAz(double Azimuth, double Altitude)
    {
        LogMessage("SlewToAltAz", "Not implemented");
        throw new MethodNotImplementedException("SlewToAltAz");
    }


    /// <summary>
    /// This Method must be implemented if <see cref="CanSlewAltAzAsync" /> returns True.
    /// It returns immediately, with Slewing set to True
    /// </summary>
    /// <param name="Azimuth">Azimuth to which to move</param>
    /// <param name="Altitude">Altitude to which to move to</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "internal static method name used for many years.")]
    internal static void SlewToAltAzAsync(double Azimuth, double Altitude)
    {
        LogMessage("SlewToAltAzAsync", "Not implemented");
        throw new MethodNotImplementedException("SlewToAltAzAsync");
    }

    /// <summary>
    /// This Method must be implemented if <see cref="CanSlewAltAzAsync" /> returns True.
    /// It does not return to the caller until the slew is complete.
    /// </summary>
    internal static void SlewToCoordinates(double RightAscension, double Declination)
    {
        LogMessage("SlewToCoordinates", "Not implemented");
        throw new MethodNotImplementedException("SlewToCoordinates");
    }

    /// <summary>
    /// Move the telescope to the given equatorial coordinates, return with Slewing set to True immediately after starting the slew.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "internal static method name used for many years.")]
    internal static void SlewToCoordinatesAsync(double RightAscension, double Declination)
    {
        LogMessage("SlewToCoordinatesAsync", "Not implemented");
        throw new MethodNotImplementedException("SlewToCoordinatesAsync");
    }

    /// <summary>
    /// Move the telescope to the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> coordinates, return when slew complete.
    /// </summary>
    internal static void SlewToTarget()
    {
        LogMessage("SlewToTarget", "Not implemented");
        throw new MethodNotImplementedException("SlewToTarget");
    }

    /// <summary>
    /// Move the telescope to the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" />  coordinates,
    /// returns immediately after starting the slew with Slewing set to True.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "internal static method name used for many years.")]
    internal static void SlewToTargetAsync()
    {
        LogMessage("SlewToTargetAsync", "Not implemented");
        throw new MethodNotImplementedException("SlewToTargetAsync");
    }

    /// <summary>
    /// True if telescope is in the process of moving in response to one of the
    /// Slew methods or the <see cref="MoveAxis" /> method, False at all other times.
    /// </summary>
    internal static bool Slewing
    {
        get
        {
            LogMessage("Slewing Get", "Not implemented");
            throw new PropertyNotImplementedException("Slewing", false);
        }
    }

    /// <summary>
    /// Matches the scope's local horizontal coordinates to the given local horizontal coordinates.
    /// </summary>
    internal static void SyncToAltAz(double Azimuth, double Altitude)
    {
        LogMessage("SyncToAltAz", "Not implemented");
        throw new MethodNotImplementedException("SyncToAltAz");
    }

    /// <summary>
    /// Matches the scope's equatorial coordinates to the given equatorial coordinates.
    /// </summary>
    internal static void SyncToCoordinates(double RightAscension, double Declination)
    {
        LogMessage("SyncToCoordinates", "Not implemented");
        throw new MethodNotImplementedException("SyncToCoordinates");
    }

    /// <summary>
    /// Matches the scope's equatorial coordinates to the target equatorial coordinates.
    /// </summary>
    internal static void SyncToTarget()
    {
        LogMessage("SyncToTarget", "Not implemented");
        throw new MethodNotImplementedException("SyncToTarget");
    }

    /// <summary>
    /// The declination (degrees, positive North) for the target of an equatorial slew or sync operation
    /// </summary>
    internal static double TargetDeclination
    {
        get
        {
            LogMessage("TargetDeclination Get", "Not implemented");
            throw new PropertyNotImplementedException("TargetDeclination", false);
        }
        set
        {
            LogMessage("TargetDeclination Set", "Not implemented");
            throw new PropertyNotImplementedException("TargetDeclination", true);
        }
    }

    /// <summary>
    /// The right ascension (hours) for the target of an equatorial slew or sync operation
    /// </summary>
    internal static double TargetRightAscension
    {
        get
        {
            LogMessage("TargetRightAscension Get", "Not implemented");
            throw new PropertyNotImplementedException("TargetRightAscension", false);
        }
        set
        {
            LogMessage("TargetRightAscension Set", "Not implemented");
            throw new PropertyNotImplementedException("TargetRightAscension", true);
        }
    }

    /// <summary>
    /// The state of the telescope's sidereal tracking drive.
    /// </summary>
    internal static bool Tracking
    {
        get
        {
            bool tracking = true;
            LogMessage("Tracking", "Get - " + tracking.ToString());
            return tracking;
        }
        set
        {
            LogMessage("Tracking Set", "Not implemented");
            throw new PropertyNotImplementedException("Tracking", true);
        }
    }

    /// <summary>
    /// The current tracking rate of the telescope's sidereal drive
    /// </summary>
    internal static DriveRates TrackingRate
    {
        get
        {
            const DriveRates DEFAULT_DRIVERATE = DriveRates.driveSidereal;
            LogMessage("TrackingRate Get", $"{DEFAULT_DRIVERATE}");
            return DEFAULT_DRIVERATE;
        }
        set
        {
            LogMessage("TrackingRate Set", "Not implemented");
            throw new PropertyNotImplementedException("TrackingRate", true);
        }
    }

    /// <summary>
    /// Returns a collection of supported <see cref="DriveRates" /> values that describe the permissible
    /// values of the <see cref="TrackingRate" /> property for this telescope type.
    /// </summary>
    internal static ITrackingRates TrackingRates
    {
        get
        {
            ITrackingRates trackingRates = new TrackingRates();
            LogMessage("TrackingRates", "Get - ");
            foreach (DriveRates driveRate in trackingRates)
            {
                LogMessage("TrackingRates", "Get - " + driveRate.ToString());
            }
            return trackingRates;
        }
    }

    /// <summary>
    /// The UTC date/time of the telescope's internal clock
    /// </summary>
    internal static DateTime UTCDate
    {
        get
        {
            DateTime utcDate = DateTime.UtcNow;
            LogMessage("UTCDate", "Get - " + String.Format("MM/dd/yy HH:mm:ss", utcDate));
            return utcDate;
        }
        set
        {
            LogMessage("UTCDate Set", "Not implemented");
            throw new PropertyNotImplementedException("UTCDate", true);
        }
    }

    /// <summary>
    /// Takes telescope out of the Parked state.
    /// </summary>
    internal static void Unpark()
    {
        LogMessage("Unpark", "Not implemented");
        throw new MethodNotImplementedException("Unpark");
    }

    #endregion

    //ENDOFINSERTEDFILE

    /// <summary>
    /// Dummy LogMessage class that removes compilation errors in the Platform source code and that will be omitted when the project is built
    /// </summary>
    static void LogMessage(string method, string message)
    {
    }
}