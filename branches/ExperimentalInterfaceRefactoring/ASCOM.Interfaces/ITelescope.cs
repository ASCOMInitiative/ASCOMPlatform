using System;
namespace ASCOM.Interfaces
{
    public interface ITelescope : IAscomDriver, IDeviceControl
    {
        void AbortSlew();
        AlignmentModes AlignmentMode { get; }
        double Altitude { get; }
        double ApertureArea { get; }
        double ApertureDiameter { get; }
        bool AtHome { get; }
        bool AtPark { get; }
        IAxisRates AxisRates(TelescopeAxes Axis);
        double Azimuth { get; }
        bool CanFindHome { get; }
        bool CanMoveAxis(TelescopeAxes Axis);
        bool CanPark { get; }
        bool CanPulseGuide { get; }
        bool CanSetDeclinationRate { get; }
        bool CanSetGuideRates { get; }
        bool CanSetPark { get; }
        bool CanSetPierSide { get; }
        bool CanSetRightAscensionRate { get; }
        bool CanSetTracking { get; }
        bool CanSlew { get; }
        bool CanSlewAltAz { get; }
        bool CanSlewAltAzAsync { get; }
        bool CanSlewAsync { get; }
        bool CanSync { get; }
        bool CanSyncAltAz { get; }
        bool CanUnpark { get; }
        //string Configuration { get; set; }
        double Declination { get; }
        double DeclinationRate { get; set; }
        PierSide DestinationSideOfPier(double RightAscension, double Declination);
        void Dispose();
        bool DoesRefraction { get; set; }
        EquatorialCoordinateType EquatorialSystem { get; }
        void FindHome();
        double FocalLength { get; }
        double GuideRateDeclination { get; set; }
        double GuideRateRightAscension { get; set; }
        bool IsPulseGuiding { get; }
        void MoveAxis(TelescopeAxes Axis, double Rate);
        void Park();
        void PulseGuide(GuideDirections Direction, int Duration);
        double RightAscension { get; }
        double RightAscensionRate { get; set; }
        void SetPark();
        PierSide SideOfPier { get; set; }
        double SiderealTime { get; }
        double SiteElevation { get; set; }
        double SiteLatitude { get; set; }
        double SiteLongitude { get; set; }
        bool Slewing { get; }
        short SlewSettleTime { get; set; }
        void SlewToAltAz(double Azimuth, double Altitude);
        void SlewToAltAzAsync(double Azimuth, double Altitude);
        void SlewToCoordinates(double RightAscension, double Declination);
        void SlewToCoordinatesAsync(double RightAscension, double Declination);
        void SlewToTarget();
        void SlewToTargetAsync();
        //string[] SupportedConfigurations { get; }
        void SyncToAltAz(double Azimuth, double Altitude);
        void SyncToCoordinates(double RightAscension, double Declination);
        void SyncToTarget();
        double TargetDeclination { get; set; }
        double TargetRightAscension { get; set; }
        bool Tracking { get; set; }
        DriveRates TrackingRate { get; set; }
        ITrackingRates TrackingRates { get; }
        void Unpark();
        DateTime UTCDate { get; set; }
    }
}
