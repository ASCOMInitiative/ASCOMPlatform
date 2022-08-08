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

class DeviceTelescope
{
    #region ITelescope Implementation

    /// <summary>
    /// Stops a slew in progress.
    /// </summary>
    public void AbortSlew()
    {
        LogMessage("AbortSlew", $"Calling method.");
        TelescopeHardware.AbortSlew();
        LogMessage("AbortSlew", $"Completed.");
    }

    /// <summary>
    /// The alignment mode of the mount (Alt/Az, Polar, German Polar).
    /// </summary>
    public AlignmentModes AlignmentMode
    {
        get
        {
            AlignmentModes alignmentMode = TelescopeHardware.AlignmentMode;
            LogMessage("AlignmentMode Get", alignmentMode.ToString());
            return alignmentMode;
        }
    }

    /// <summary>
    /// The Altitude above the local horizon of the telescope's current position (degrees, positive up)
    /// </summary>
    public double Altitude
    {
        get
        {
            double altitude = TelescopeHardware.Altitude;
            LogMessage("Altitude Get", altitude.ToString());
            return altitude;
        }
    }

    /// <summary>
    /// The area of the telescope's aperture, taking into account any obstructions (square meters)
    /// </summary>
    public double ApertureArea
    {
        get
        {
            double apertureArea = TelescopeHardware.ApertureArea;
            LogMessage("ApertureArea Get", apertureArea.ToString());
            return apertureArea;
        }
    }

    /// <summary>
    /// The telescope's effective aperture diameter (meters)
    /// </summary>
    public double ApertureDiameter
    {
        get
        {
            double apertureArea = TelescopeHardware.ApertureDiameter;
            LogMessage("ApertureDiameter Get", apertureArea.ToString());
            return apertureArea;
        }
    }

    /// <summary>
    /// True if the telescope is stopped in the Home position. Set only following a <see cref="FindHome"></see> operation,
    /// and reset with any slew operation. This property must be False if the telescope does not support homing.
    /// </summary>
    public bool AtHome
    {
        get
        {
            bool atHome = TelescopeHardware.AtHome;
            LogMessage("AtHome Get", atHome.ToString());
            return atHome;
        }
    }

    /// <summary>
    /// True if the telescope has been put into the parked state by the see <see cref="Park" /> method. Set False by calling the Unpark() method.
    /// </summary>
    public bool AtPark
    {
        get
        {
            bool atPark = TelescopeHardware.AtPark;
            LogMessage("AtPark Get", atPark.ToString());
            return atPark;
        }
    }

    /// <summary>
    /// Determine the rates at which the telescope may be moved about the specified axis by the <see cref="MoveAxis" /> method.
    /// </summary>
    /// <param name="axis">The axis about which rate information is desired (TelescopeAxes value)</param>
    /// <returns>Collection of <see cref="IRate" /> rate objects</returns>
    public IAxisRates AxisRates(TelescopeAxes axis)
    {
        IAxisRates axisRates = TelescopeHardware.AxisRates(axis);
        LogMessage("AxisRates Get", $"Axis: {axis} returned {axisRates.Count} rates.");
        return axisRates;
    }

    /// <summary>
    /// The azimuth at the local horizon of the telescope's current position (degrees, North-referenced, positive East/clockwise).
    /// </summary>
    public double Azimuth
    {
        get
        {
            double azimuth = TelescopeHardware.Azimuth;
            LogMessage("Azimuth Get", azimuth.ToString());
            return azimuth;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed finding its home position (<see cref="FindHome" /> method).
    /// </summary>
    public bool CanFindHome
    {
        get
        {
            bool canFindHome = TelescopeHardware.CanFindHome;
            LogMessage("CanFindHome Get", canFindHome.ToString());
            return canFindHome;
        }
    }

    /// <summary>
    /// True if this telescope can move the requested axis
    /// </summary>
    public bool CanMoveAxis(TelescopeAxes axis)
    {
        bool canMoveAxis = TelescopeHardware.CanMoveAxis(axis);
        LogMessage("CanMoveAxis Get", $"Axis: {axis} returned {canMoveAxis}.");
        return canMoveAxis;
    }

    /// <summary>
    /// True if this telescope is capable of programmed parking (<see cref="Park" />method)
    /// </summary>
    public bool CanPark
    {
        get
        {
            bool canPark = TelescopeHardware.CanPark;
            LogMessage("CanPark Get", canPark.ToString());
            return canPark;
        }
    }

    /// <summary>
    /// True if this telescope is capable of software-pulsed guiding (via the <see cref="PulseGuide" /> method)
    /// </summary>
    public bool CanPulseGuide
    {
        get
        {
            bool canPulseGuide = TelescopeHardware.CanPulseGuide;
            LogMessage("CanPulseGuide Get", canPulseGuide.ToString());
            return canPulseGuide;
        }
    }

    /// <summary>
    /// True if the <see cref="DeclinationRate" /> property can be changed to provide offset tracking in the declination axis.
    /// </summary>
    public bool CanSetDeclinationRate
    {
        get
        {
            bool canSetDeclinationRate = TelescopeHardware.CanSetDeclinationRate;
            LogMessage("CanSetDeclinationRate Get", canSetDeclinationRate.ToString());
            return canSetDeclinationRate;
        }
    }

    /// <summary>
    /// True if the guide rate properties used for <see cref="PulseGuide" /> can be adjusted.
    /// </summary>
    public bool CanSetGuideRates
    {
        get
        {
            bool canSetGuideRates = TelescopeHardware.CanSetGuideRates;
            LogMessage("CanSetGuideRates Get", canSetGuideRates.ToString());
            return canSetGuideRates;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed setting of its park position (<see cref="SetPark" /> method)
    /// </summary>
    public bool CanSetPark
    {
        get
        {
            bool canSetPark = TelescopeHardware.CanSetPark;
            LogMessage("CanSetPark Get", canSetPark.ToString());
            return canSetPark;
        }
    }

    /// <summary>
    /// True if the <see cref="SideOfPier" /> property can be set, meaning that the mount can be forced to flip.
    /// </summary>
    public bool CanSetPierSide
    {
        get
        {
            bool canSetPierSide = TelescopeHardware.CanSetPierSide;
            LogMessage("CanSetPierSide Get", canSetPierSide.ToString());
            return canSetPierSide;
        }
    }

    /// <summary>
    /// True if the <see cref="RightAscensionRate" /> property can be changed to provide offset tracking in the right ascension axis.
    /// </summary>
    public bool CanSetRightAscensionRate
    {
        get
        {
            bool canSetRightAscensionRate = TelescopeHardware.CanSetRightAscensionRate;
            LogMessage("CanSetRightAscensionRate Get", canSetRightAscensionRate.ToString());
            return canSetRightAscensionRate;
        }
    }

    /// <summary>
    /// True if the <see cref="Tracking" /> property can be changed, turning telescope sidereal tracking on and off.
    /// </summary>
    public bool CanSetTracking
    {
        get
        {
            bool canSetTracking = TelescopeHardware.CanSetTracking;
            LogMessage("CanSetTracking Get", canSetTracking.ToString());
            return canSetTracking;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed slewing (synchronous or asynchronous) to equatorial coordinates
    /// </summary>
    public bool CanSlew
    {
        get
        {
            bool canSlew = TelescopeHardware.CanSlew;
            LogMessage("CanSlew Get", canSlew.ToString());
            return canSlew;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed slewing (synchronous or asynchronous) to local horizontal coordinates
    /// </summary>
    public bool CanSlewAltAz
    {
        get
        {
            bool canSlewAltAz = TelescopeHardware.CanSlewAltAz;
            LogMessage("CanSlewAltAz Get", canSlewAltAz.ToString());
            return canSlewAltAz;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed asynchronous slewing to local horizontal coordinates
    /// </summary>
    public bool CanSlewAltAzAsync
    {
        get
        {
            bool canSlewAltAzAsync = TelescopeHardware.CanSlewAltAzAsync;
            LogMessage("CanSlewAltAzAsync Get", canSlewAltAzAsync.ToString());
            return canSlewAltAzAsync;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed asynchronous slewing to equatorial coordinates.
    /// </summary>
    public bool CanSlewAsync
    {
        get
        {
            bool canSlewAsync = TelescopeHardware.CanSlewAsync;
            LogMessage("CanSlewAsync Get", canSlewAsync.ToString());
            return canSlewAsync;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed syncing to equatorial coordinates.
    /// </summary>
    public bool CanSync
    {
        get
        {
            bool canSync = TelescopeHardware.CanSync;
            LogMessage("CanSync Get", canSync.ToString());
            return canSync;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed syncing to local horizontal coordinates
    /// </summary>
    public bool CanSyncAltAz
    {
        get
        {
            bool canSyncAltAz = TelescopeHardware.CanSyncAltAz;
            LogMessage("CanSyncAltAz Get", canSyncAltAz.ToString());
            return canSyncAltAz;
        }
    }

    /// <summary>
    /// True if this telescope is capable of programmed unparking (<see cref="Unpark" /> method).
    /// </summary>
    public bool CanUnpark
    {
        get
        {
            bool canUnpark = TelescopeHardware.CanUnpark;
            LogMessage("CanUnpark Get", canUnpark.ToString());
            return canUnpark;
        }
    }

    /// <summary>
    /// The declination (degrees) of the telescope's current equatorial coordinates, in the coordinate system given by the <see cref="EquatorialSystem" /> property.
    /// Reading the property will raise an error if the value is unavailable.
    /// </summary>
    public double Declination
    {
        get
        {
            double declination = TelescopeHardware.Declination;
            LogMessage("Declination Get", declination.ToString());
            return declination;
        }
    }

    /// <summary>
    /// The declination tracking rate (arc-seconds per SI second, default = 0.0)
    /// </summary>
    public double DeclinationRate
    {
        get
        {
            double declinationRate = TelescopeHardware.DeclinationRate;
            LogMessage("DeclinationRate Get", declinationRate.ToString());
            return declinationRate;
        }
        set
        {
            LogMessage("DeclinationRate Set", value.ToString());
            TelescopeHardware.DeclinationRate = value;
        }
    }

    /// <summary>
    /// Predict side of pier for German equatorial mounts at the provided coordinates
    /// </summary>
    public PierSide DestinationSideOfPier(double rightAscension, double declination)
    {
        PierSide destinationSideOfPier = TelescopeHardware.DestinationSideOfPier(rightAscension, declination);
        LogMessage("DestinationSideOfPier Get", $"RA: {rightAscension}, Dec: {declination} - {destinationSideOfPier}.");
        return destinationSideOfPier;
    }

    /// <summary>
    /// True if the telescope or driver applies atmospheric refraction to coordinates.
    /// </summary>
    public bool DoesRefraction
    {
        get
        {
            bool doesRefraction = TelescopeHardware.DoesRefraction;
            LogMessage("DoesRefraction Get", doesRefraction.ToString());
            return doesRefraction;
        }
        set
        {
            LogMessage("DoesRefraction Set", value.ToString());
            TelescopeHardware.DoesRefraction = value;
        }
    }

    /// <summary>
    /// Equatorial coordinate system used by this telescope (e.g. Topocentric or J2000).
    /// </summary>
    public EquatorialCoordinateType EquatorialSystem
    {
        get
        {
            EquatorialCoordinateType equatorialSystem = TelescopeHardware.EquatorialSystem;
            LogMessage("EquatorialSystem Get", equatorialSystem.ToString());
            return equatorialSystem;
        }
    }

    /// <summary>
    /// Locates the telescope's "home" position (synchronous)
    /// </summary>
    public void FindHome()
    {
        LogMessage("FindHome", $"Calling method.");
        TelescopeHardware.FindHome();
        LogMessage("FindHome", $"Completed.");
    }

    /// <summary>
    /// The telescope's focal length, meters
    /// </summary>
    public double FocalLength
    {
        get
        {
            double focalLength = TelescopeHardware.FocalLength;
            LogMessage("FocalLength Get", focalLength.ToString());
            return focalLength;
        }
    }

    /// <summary>
    /// The current Declination movement rate offset for telescope guiding (degrees/sec)
    /// </summary>
    public double GuideRateDeclination
    {
        get
        {
            double guideRateDeclination = TelescopeHardware.GuideRateDeclination;
            LogMessage("GuideRateDeclination Get", guideRateDeclination.ToString());
            return guideRateDeclination;
        }
        set
        {
            LogMessage("GuideRateDeclination Set", value.ToString());
            TelescopeHardware.GuideRateDeclination = value;
        }
    }

    /// <summary>
    /// The current Right Ascension movement rate offset for telescope guiding (degrees/sec)
    /// </summary>
    public double GuideRateRightAscension
    {
        get
        {
            double guideRateRightAscension = TelescopeHardware.GuideRateRightAscension;
            LogMessage("GuideRateRightAscension Get", guideRateRightAscension.ToString());
            return guideRateRightAscension;
        }
        set
        {
            LogMessage("GuideRateRightAscension Set", value.ToString());
            TelescopeHardware.GuideRateRightAscension = value;
        }
    }

    /// <summary>
    /// True if a <see cref="PulseGuide" /> command is in progress, False otherwise
    /// </summary>
    public bool IsPulseGuiding
    {
        get
        {
            bool isPulseGuiding = TelescopeHardware.IsPulseGuiding;
            LogMessage("IsPulseGuiding Get", isPulseGuiding.ToString());
            return isPulseGuiding;
        }
    }

    /// <summary>
    /// Move the telescope in one axis at the given rate.
    /// </summary>
    /// <param name="axis">The physical axis about which movement is desired</param>
    /// <param name="rate">The rate of motion (deg/sec) about the specified axis</param>
    public void MoveAxis(TelescopeAxes axis, double rate)
    {
        LogMessage("MoveAxis", $"Calling method - Axis: {axis}, Rate: {rate}.");
        TelescopeHardware.MoveAxis(axis, rate);
        LogMessage("MoveAxis", $"Completed.");
    }


    /// <summary>
    /// Move the telescope to its park position, stop all motion (or restrict to a small safe range), and set <see cref="AtPark" /> to True.
    /// </summary>
    public void Park()
    {
        LogMessage("Park", $"Calling method.");
        TelescopeHardware.Park();
        LogMessage("Park", $"Completed.");
    }

    /// <summary>
    /// Moves the scope in the given direction for the given interval or time at
    /// the rate given by the corresponding guide rate property
    /// </summary>
    /// <param name="direction">The direction in which the guide-rate motion is to be made</param>
    /// <param name="duration">The duration of the guide-rate motion (milliseconds)</param>
    public void PulseGuide(GuideDirections direction, int duration)
    {
        LogMessage("PulseGuide", $"Calling method.");
        TelescopeHardware.PulseGuide(direction, duration);
        LogMessage("PulseGuide", $"Completed.");
    }

    /// <summary>
    /// The right ascension (hours) of the telescope's current equatorial coordinates,
    /// in the coordinate system given by the EquatorialSystem property
    /// </summary>
    public double RightAscension
    {
        get
        {
            double rightAscension = TelescopeHardware.RightAscension;
            LogMessage("RightAscension Get", rightAscension.ToString());
            return rightAscension;
        }
    }

    /// <summary>
    /// The right ascension tracking rate offset from sidereal (seconds per sidereal second, default = 0.0)
    /// </summary>
    public double RightAscensionRate
    {
        get
        {
            double rightAscensionRate = TelescopeHardware.RightAscensionRate;
            LogMessage("RightAscensionRate Get", rightAscensionRate.ToString());
            return rightAscensionRate;
        }
        set
        {
            LogMessage("RightAscensionRate Set", value.ToString());
            TelescopeHardware.RightAscensionRate = value;
        }
    }

    /// <summary>
    /// Sets the telescope's park position to be its current position.
    /// </summary>
    public void SetPark()
    {
        LogMessage("SetPark", $"Calling method.");
        TelescopeHardware.SetPark();
        LogMessage("SetPark", $"Completed.");
    }

    /// <summary>
    /// Indicates the pointing state of the mount. Read the articles installed with the ASCOM Developer
    /// Components for more detailed information.
    /// </summary>
    public PierSide SideOfPier
    {
        get
        {
            PierSide sideOfPier = TelescopeHardware.SideOfPier;
            LogMessage("SideOfPier Get", sideOfPier.ToString());
            return sideOfPier;
        }
        set
        {
            LogMessage("SideOfPier Set", value.ToString());
            TelescopeHardware.SideOfPier = value;
        }
    }

    /// <summary>
    /// The local apparent sidereal time from the telescope's internal clock (hours, sidereal)
    /// </summary>
    public double SiderealTime
    {
        get
        {
            double siderealTime = TelescopeHardware.SiderealTime;
            LogMessage("SiderealTime Get", siderealTime.ToString());
            return siderealTime;
        }
    }

    /// <summary>
    /// The elevation above mean sea level (meters) of the site at which the telescope is located
    /// </summary>
    public double SiteElevation
    {
        get
        {
            double siteElevation = TelescopeHardware.SiteElevation;
            LogMessage("SiteElevation Get", siteElevation.ToString());
            return siteElevation;
        }
        set
        {
            LogMessage("SiteElevation Set", value.ToString());
            TelescopeHardware.SiteElevation = value;
        }
    }

    /// <summary>
    /// The geodetic(map) latitude (degrees, positive North, WGS84) of the site at which the telescope is located.
    /// </summary>
    public double SiteLatitude
    {
        get
        {
            double siteLatitude = TelescopeHardware.SiteLatitude;
            LogMessage("SiteLatitude Get", siteLatitude.ToString());
            return siteLatitude;
        }
        set
        {
            LogMessage("SiteLatitude Set", value.ToString());
            TelescopeHardware.SiteLatitude = value;
        }
    }

    /// <summary>
    /// The longitude (degrees, positive East, WGS84) of the site at which the telescope is located.
    /// </summary>
    public double SiteLongitude
    {
        get
        {
            double siteLongitude = TelescopeHardware.SiteLongitude;
            LogMessage("SiteLongitude Get", siteLongitude.ToString());
            return siteLongitude;
        }
        set
        {
            LogMessage("SiteLongitude Set", value.ToString());
            TelescopeHardware.SiteLongitude = value;
        }
    }

    /// <summary>
    /// Specifies a post-slew settling time (sec.).
    /// </summary>
    public short SlewSettleTime
    {
        get
        {
            short slewSettleTime = TelescopeHardware.SlewSettleTime;
            LogMessage("SlewSettleTime Get", slewSettleTime.ToString());
            return slewSettleTime;
        }
        set
        {
            LogMessage("SlewSettleTime Set", value.ToString());
            TelescopeHardware.SlewSettleTime = value;
        }
    }

    /// <summary>
    /// Move the telescope to the given local horizontal coordinates, return when slew is complete
    /// </summary>
    public void SlewToAltAz(double azimuth, double altitude)
    {
        LogMessage("SlewToAltAz", $"Calling method - Azimuth: {azimuth}, Altitude: {altitude}.");
        TelescopeHardware.SlewToAltAz(azimuth, altitude);
        LogMessage("SlewToAltAz", $"Completed.");
    }

    /// <summary>
    /// This Method must be implemented if <see cref="CanSlewAltAzAsync" /> returns True.
    /// It returns immediately, with Slewing set to True
    /// </summary>
    /// <param name="Azimuth">Azimuth to which to move</param>
    /// <param name="Altitude">Altitude to which to move to</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "Public method name used for many years.")]
    public void SlewToAltAzAsync(double azimuth, double altitude)
    {
        LogMessage("SlewToAltAzAsync", $"Calling method - Azimuth: {azimuth}, Altitude: {altitude}.");
        TelescopeHardware.SlewToAltAzAsync(azimuth, altitude);
        LogMessage("SlewToAltAzAsync", $"Completed.");
    }

    /// <summary>
    /// This Method must be implemented if <see cref="CanSlewAltAzAsync" /> returns True.
    /// It does not return to the caller until the slew is complete.
    /// </summary>
    public void SlewToCoordinates(double rightAscension, double declination)
    {
        LogMessage("SlewToCoordinates", $"Calling method - RightAscension: {rightAscension}, Declination: {declination}.");
        TelescopeHardware.SlewToCoordinates(rightAscension, declination);
        LogMessage("SlewToCoordinates", $"Completed.");
    }

    /// <summary>
    /// Move the telescope to the given equatorial coordinates, return with Slewing set to True immediately after starting the slew.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "Public method name used for many years.")]
    public void SlewToCoordinatesAsync(double rightAscension, double declination)
    {
        LogMessage("SlewToCoordinatesAsync", $"Calling method - RightAscension: {rightAscension}, Declination: {declination}.");
        TelescopeHardware.SlewToCoordinatesAsync(rightAscension, declination);
        LogMessage("SlewToCoordinatesAsync", $"Completed.");
    }

    /// <summary>
    /// Move the telescope to the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> coordinates, return when slew complete.
    /// </summary>
    public void SlewToTarget()
    {
        LogMessage("SlewToTarget", $"Calling method.");
        TelescopeHardware.SlewToTarget();
        LogMessage("SlewToTarget", $"Completed.");
    }

    /// <summary>
    /// Move the telescope to the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" />  coordinates,
    /// returns immediately after starting the slew with Slewing set to True.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "Public method name used for many years.")]
    public void SlewToTargetAsync()
    {
        LogMessage("SlewToTargetAsync", $"Calling method.");
        TelescopeHardware.SlewToTargetAsync();
        LogMessage("SlewToTargetAsync", $"Completed.");
    }

    /// <summary>
    /// True if telescope is in the process of moving in response to one of the
    /// Slew methods or the <see cref="MoveAxis" /> method, False at all other times.
    /// </summary>
    public bool Slewing
    {
        get
        {
            bool slewing = TelescopeHardware.Slewing;
            LogMessage("Slewing Get", slewing.ToString());
            return slewing;
        }
    }

    /// <summary>
    /// Matches the scope's local horizontal coordinates to the given local horizontal coordinates.
    /// </summary>
    public void SyncToAltAz(double azimuth, double altitude)
    {
        LogMessage("SyncToAltAz", $"Calling method - Azimuth: {azimuth}, Altitude: {altitude}.");
        TelescopeHardware.SyncToAltAz(azimuth, altitude);
        LogMessage("SyncToAltAz", $"Completed.");
    }

    /// <summary>
    /// Matches the scope's equatorial coordinates to the given equatorial coordinates.
    /// </summary>
    public void SyncToCoordinates(double rightAscension, double declination)
    {
        LogMessage("SyncToCoordinates", $"Calling method - RightAscension: {rightAscension}, Declination: {declination}.");
        TelescopeHardware.SyncToCoordinates(rightAscension, declination);
        LogMessage("SyncToCoordinates", $"Completed.");
    }

    /// <summary>
    /// Matches the scope's equatorial coordinates to the target equatorial coordinates.
    /// </summary>
    public void SyncToTarget()
    {
        LogMessage("SyncToTarget", $"Calling method.");
        TelescopeHardware.SyncToTarget();
        LogMessage("SyncToTarget", $"Completed.");
    }

    /// <summary>
    /// The declination (degrees, positive North) for the target of an equatorial slew or sync operation
    /// </summary>
    public double TargetDeclination
    {
        get
        {
            double targetDeclination = TelescopeHardware.TargetDeclination;
            LogMessage("TargetDeclination Get", targetDeclination.ToString());
            return targetDeclination;
        }
        set
        {
            LogMessage("TargetDeclination Set", value.ToString());
            TelescopeHardware.TargetDeclination = value;
        }
    }

    /// <summary>
    /// The right ascension (hours) for the target of an equatorial slew or sync operation
    /// </summary>
    public double TargetRightAscension
    {
        get
        {
            double targetRightAscension = TelescopeHardware.TargetRightAscension;
            LogMessage("TargetRightAscension Get", targetRightAscension.ToString());
            return targetRightAscension;
        }
        set
        {
            LogMessage("TargetRightAscension Set", value.ToString());
            TelescopeHardware.TargetRightAscension = value;
        }
    }

    /// <summary>
    /// The state of the telescope's sidereal tracking drive.
    /// </summary>
    public bool Tracking
    {
        get
        {
            bool tracking = TelescopeHardware.Tracking;
            LogMessage("Tracking Get", tracking.ToString());
            return tracking;
        }
        set
        {
            LogMessage("Tracking Set", value.ToString());
            TelescopeHardware.Tracking = value;
        }
    }

    /// <summary>
    /// The current tracking rate of the telescope's sidereal drive
    /// </summary>
    public DriveRates TrackingRate
    {
        get
        {
            DriveRates trackingRate = TelescopeHardware.TrackingRate;
            LogMessage("TrackingRate Get", trackingRate.ToString());
            return trackingRate;
        }
        set
        {
            LogMessage("TrackingRate Set", value.ToString());
            TelescopeHardware.TrackingRate = value;
        }
    }

    /// <summary>
    /// Returns a collection of supported <see cref="DriveRates" /> values that describe the permissible
    /// values of the <see cref="TrackingRate" /> property for this telescope type.
    /// </summary>
    public ITrackingRates TrackingRates
    {
        get
        {
            ITrackingRates trackingRates = TelescopeHardware.TrackingRates;
            LogMessage("TrackingRates Get", trackingRates.ToString());
            return trackingRates;
        }
    }

    /// <summary>
    /// The UTC date/time of the telescope's internal clock
    /// </summary>
    public DateTime UTCDate
    {
        get
        {
            DateTime utcDate = TelescopeHardware.UTCDate;
            LogMessage("UTCDate Get", utcDate.ToString());
            return utcDate;
        }
        set
        {
            LogMessage("UTCDate Set", value.ToString());
            TelescopeHardware.UTCDate = value;
        }
    }

    /// <summary>
    /// Takes telescope out of the Parked state.
    /// </summary>
    public void Unpark()
    {
        LogMessage("Unpark", $"Calling method.");
        TelescopeHardware.Unpark();
        LogMessage("Unpark", $"Completed.");
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