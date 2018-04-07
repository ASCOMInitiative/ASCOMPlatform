﻿// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM.DeviceInterface;
using System;
using ASCOM;
using ASCOM.Utilities;

class DeviceTelescope
{
    Util utilities = new Util();
    TraceLogger tl = new TraceLogger();

    #region ITelescope Implementation
    public void AbortSlew()
    {
        tl.LogMessage("AbortSlew", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("AbortSlew");
    }

    public AlignmentModes AlignmentMode
    {
        get
        {
            tl.LogMessage("AlignmentMode Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("AlignmentMode", false);
        }
    }

    public double Altitude
    {
        get
        {
            tl.LogMessage("Altitude", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Altitude", false);
        }
    }

    public double ApertureArea
    {
        get
        {
            tl.LogMessage("ApertureArea Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("ApertureArea", false);
        }
    }

    public double ApertureDiameter
    {
        get
        {
            tl.LogMessage("ApertureDiameter Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("ApertureDiameter", false);
        }
    }

    public bool AtHome
    {
        get
        {
            tl.LogMessage("AtHome", "Get - " + false.ToString());
            return false;
        }
    }

    public bool AtPark
    {
        get
        {
            tl.LogMessage("AtPark", "Get - " + false.ToString());
            return false;
        }
    }

    public IAxisRates AxisRates(TelescopeAxes Axis)
    {
        tl.LogMessage("AxisRates", "Get - " + Axis.ToString());
        return new AxisRates(Axis);
    }

    public double Azimuth
    {
        get
        {
            tl.LogMessage("Azimuth Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Azimuth", false);
        }
    }

    public bool CanFindHome
    {
        get
        {
            tl.LogMessage("CanFindHome", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanMoveAxis(TelescopeAxes Axis)
    {
        tl.LogMessage("CanMoveAxis", "Get - " + Axis.ToString());
        switch (Axis)
        {
            case TelescopeAxes.axisPrimary: return false;
            case TelescopeAxes.axisSecondary: return false;
            case TelescopeAxes.axisTertiary: return false;
            default: throw new InvalidValueException("CanMoveAxis", Axis.ToString(), "0 to 2");
        }
    }

    public bool CanPark
    {
        get
        {
            tl.LogMessage("CanPark", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanPulseGuide
    {
        get
        {
            tl.LogMessage("CanPulseGuide", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanSetDeclinationRate
    {
        get
        {
            tl.LogMessage("CanSetDeclinationRate", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanSetGuideRates
    {
        get
        {
            tl.LogMessage("CanSetGuideRates", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanSetPark
    {
        get
        {
            tl.LogMessage("CanSetPark", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanSetPierSide
    {
        get
        {
            tl.LogMessage("CanSetPierSide", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanSetRightAscensionRate
    {
        get
        {
            tl.LogMessage("CanSetRightAscensionRate", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanSetTracking
    {
        get
        {
            tl.LogMessage("CanSetTracking", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanSlew
    {
        get
        {
            tl.LogMessage("CanSlew", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanSlewAltAz
    {
        get
        {
            tl.LogMessage("CanSlewAltAz", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanSlewAltAzAsync
    {
        get
        {
            tl.LogMessage("CanSlewAltAzAsync", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanSlewAsync
    {
        get
        {
            tl.LogMessage("CanSlewAsync", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanSync
    {
        get
        {
            tl.LogMessage("CanSync", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanSyncAltAz
    {
        get
        {
            tl.LogMessage("CanSyncAltAz", "Get - " + false.ToString());
            return false;
        }
    }

    public bool CanUnpark
    {
        get
        {
            tl.LogMessage("CanUnpark", "Get - " + false.ToString());
            return false;
        }
    }

    public double Declination
    {
        get
        {
            double declination = 0.0;
            tl.LogMessage("Declination", "Get - " + utilities.DegreesToDMS(declination, ":", ":"));
            return declination;
        }
    }

    public double DeclinationRate
    {
        get
        {
            double declination = 0.0;
            tl.LogMessage("DeclinationRate", "Get - " + declination.ToString());
            return declination;
        }
        set
        {
            tl.LogMessage("DeclinationRate Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("DeclinationRate", true);
        }
    }

    public PierSide DestinationSideOfPier(double RightAscension, double Declination)
    {
        tl.LogMessage("DestinationSideOfPier Get", "Not implemented");
        throw new ASCOM.PropertyNotImplementedException("DestinationSideOfPier", false);
    }

    public bool DoesRefraction
    {
        get
        {
            tl.LogMessage("DoesRefraction Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("DoesRefraction", false);
        }
        set
        {
            tl.LogMessage("DoesRefraction Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("DoesRefraction", true);
        }
    }

    public EquatorialCoordinateType EquatorialSystem
    {
        get
        {
            EquatorialCoordinateType equatorialSystem = EquatorialCoordinateType.equLocalTopocentric;
            tl.LogMessage("DeclinationRate", "Get - " + equatorialSystem.ToString());
            return equatorialSystem;
        }
    }

    public void FindHome()
    {
        tl.LogMessage("FindHome", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("FindHome");
    }

    public double FocalLength
    {
        get
        {
            tl.LogMessage("FocalLength Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("FocalLength", false);
        }
    }

    public double GuideRateDeclination
    {
        get
        {
            tl.LogMessage("GuideRateDeclination Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("GuideRateDeclination", false);
        }
        set
        {
            tl.LogMessage("GuideRateDeclination Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("GuideRateDeclination", true);
        }
    }

    public double GuideRateRightAscension
    {
        get
        {
            tl.LogMessage("GuideRateRightAscension Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("GuideRateRightAscension", false);
        }
        set
        {
            tl.LogMessage("GuideRateRightAscension Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("GuideRateRightAscension", true);
        }
    }

    public bool IsPulseGuiding
    {
        get
        {
            tl.LogMessage("IsPulseGuiding Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("IsPulseGuiding", false);
        }
    }

    public void MoveAxis(TelescopeAxes Axis, double Rate)
    {
        tl.LogMessage("MoveAxis", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("MoveAxis");
    }

    public void Park()
    {
        tl.LogMessage("Park", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("Park");
    }

    public void PulseGuide(GuideDirections Direction, int Duration)
    {
        tl.LogMessage("PulseGuide", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("PulseGuide");
    }

    public double RightAscension
    {
        get
        {
            double rightAscension = 0.0;
            tl.LogMessage("RightAscension", "Get - " + utilities.HoursToHMS(rightAscension));
            return rightAscension;
        }
    }

    public double RightAscensionRate
    {
        get
        {
            double rightAscensionRate = 0.0;
            tl.LogMessage("RightAscensionRate", "Get - " + rightAscensionRate.ToString());
            return rightAscensionRate;
        }
        set
        {
            tl.LogMessage("RightAscensionRate Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("RightAscensionRate", true);
        }
    }

    public void SetPark()
    {
        tl.LogMessage("SetPark", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SetPark");
    }

    public PierSide SideOfPier
    {
        get
        {
            tl.LogMessage("SideOfPier Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SideOfPier", false);
        }
        set
        {
            tl.LogMessage("SideOfPier Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SideOfPier", true);
        }
    }

    public double SiderealTime
    {
        get
        {
            // get greenwich sidereal time: https://en.wikipedia.org/wiki/Sidereal_time
            //double siderealTime = (18.697374558 + 24.065709824419081 * (utilities.DateUTCToJulian(DateTime.UtcNow) - 2451545.0));

            // alternative using NOVAS 3.1
            double siderealTime = 0.0;
            using (var novas = new ASCOM.Astrometry.NOVAS.NOVAS31())
            {
                var jd = utilities.DateUTCToJulian(DateTime.UtcNow);
                novas.SiderealTime(jd, 0, novas.DeltaT(jd),
                    ASCOM.Astrometry.GstType.GreenwichApparentSiderealTime,
                    ASCOM.Astrometry.Method.EquinoxBased,
                    ASCOM.Astrometry.Accuracy.Reduced, ref siderealTime);
            }
            // allow for the longitude
            siderealTime += SiteLongitude / 360.0 * 24.0;
            // reduce to the range 0 to 24 hours
            siderealTime = siderealTime % 24.0;
            tl.LogMessage("SiderealTime", "Get - " + siderealTime.ToString());
            return siderealTime;
        }
    }

    public double SiteElevation
    {
        get
        {
            tl.LogMessage("SiteElevation Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SiteElevation", false);
        }
        set
        {
            tl.LogMessage("SiteElevation Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SiteElevation", true);
        }
    }

    public double SiteLatitude
    {
        get
        {
            tl.LogMessage("SiteLatitude Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SiteLatitude", false);
        }
        set
        {
            tl.LogMessage("SiteLatitude Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SiteLatitude", true);
        }
    }

    public double SiteLongitude
    {
        get
        {
            tl.LogMessage("SiteLongitude Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SiteLongitude", false);
        }
        set
        {
            tl.LogMessage("SiteLongitude Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SiteLongitude", true);
        }
    }

    public short SlewSettleTime
    {
        get
        {
            tl.LogMessage("SlewSettleTime Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SlewSettleTime", false);
        }
        set
        {
            tl.LogMessage("SlewSettleTime Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("SlewSettleTime", true);
        }
    }

    public void SlewToAltAz(double Azimuth, double Altitude)
    {
        tl.LogMessage("SlewToAltAz", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SlewToAltAz");
    }

    public void SlewToAltAzAsync(double Azimuth, double Altitude)
    {
        tl.LogMessage("SlewToAltAzAsync", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SlewToAltAzAsync");
    }

    public void SlewToCoordinates(double RightAscension, double Declination)
    {
        tl.LogMessage("SlewToCoordinates", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SlewToCoordinates");
    }

    public void SlewToCoordinatesAsync(double RightAscension, double Declination)
    {
        tl.LogMessage("SlewToCoordinatesAsync", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SlewToCoordinatesAsync");
    }

    public void SlewToTarget()
    {
        tl.LogMessage("SlewToTarget", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SlewToTarget");
    }

    public void SlewToTargetAsync()
    {
        tl.LogMessage("SlewToTargetAsync", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SlewToTargetAsync");
    }

    public bool Slewing
    {
        get
        {
            tl.LogMessage("Slewing Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Slewing", false);
        }
    }

    public void SyncToAltAz(double Azimuth, double Altitude)
    {
        tl.LogMessage("SyncToAltAz", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SyncToAltAz");
    }

    public void SyncToCoordinates(double RightAscension, double Declination)
    {
        tl.LogMessage("SyncToCoordinates", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SyncToCoordinates");
    }

    public void SyncToTarget()
    {
        tl.LogMessage("SyncToTarget", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("SyncToTarget");
    }

    public double TargetDeclination
    {
        get
        {
            tl.LogMessage("TargetDeclination Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("TargetDeclination", false);
        }
        set
        {
            tl.LogMessage("TargetDeclination Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("TargetDeclination", true);
        }
    }

    public double TargetRightAscension
    {
        get
        {
            tl.LogMessage("TargetRightAscension Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("TargetRightAscension", false);
        }
        set
        {
            tl.LogMessage("TargetRightAscension Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("TargetRightAscension", true);
        }
    }

    public bool Tracking
    {
        get
        {
            bool tracking = true;
            tl.LogMessage("Tracking", "Get - " + tracking.ToString());
            return tracking;
        }
        set
        {
            tl.LogMessage("Tracking Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("Tracking", true);
        }
    }

    public DriveRates TrackingRate
    {
        get
        {
            tl.LogMessage("TrackingRate Get", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("TrackingRate", false);
        }
        set
        {
            tl.LogMessage("TrackingRate Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("TrackingRate", true);
        }
    }

    public ITrackingRates TrackingRates
    {
        get
        {
            ITrackingRates trackingRates = new TrackingRates();
            tl.LogMessage("TrackingRates", "Get - ");
            foreach (DriveRates driveRate in trackingRates)
            {
                tl.LogMessage("TrackingRates", "Get - " + driveRate.ToString());
            }
            return trackingRates;
        }
    }

    public DateTime UTCDate
    {
        get
        {
            DateTime utcDate = DateTime.UtcNow;
            tl.LogMessage("TrackingRates", "Get - " + String.Format("MM/dd/yy HH:mm:ss", utcDate));
            return utcDate;
        }
        set
        {
            tl.LogMessage("UTCDate Set", "Not implemented");
            throw new ASCOM.PropertyNotImplementedException("UTCDate", true);
        }
    }

    public void Unpark()
    {
        tl.LogMessage("Unpark", "Not implemented");
        throw new ASCOM.MethodNotImplementedException("Unpark");
    }

    #endregion

    //ENDOFINSERTEDFILE
}