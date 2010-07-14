using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Interface;

namespace ASCOM.DriverAccess
{
    class TelescopeV4a : ITelescopeV4
    {
        #region ITelescopeV4 Members

        string ITelescopeV4.ANewTelescopeV4Property
        {
            get { throw new System.NotImplementedException(); }
        }

        void ITelescopeV4.ANewTelescopeV4Method(string NewParameter1, double NewParameter2)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region ITelescopeCore Members

        AlignmentModes ITelescope.AlignmentMode
        {
            get { throw new System.NotImplementedException(); }
        }

        double ITelescope.Altitude
        {
            get { throw new System.NotImplementedException(); }
        }

        double ITelescope.ApertureArea
        {
            get { throw new System.NotImplementedException(); }
        }

        double ITelescope.ApertureDiameter
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.AtHome
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.AtPark
        {
            get { throw new System.NotImplementedException(); }
        }

        double ITelescope.Azimuth
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanFindHome
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanPark
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanPulseGuide
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanSetDeclinationRate
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanSetGuideRates
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanSetPark
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanSetRightAscensionRate
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanSetPierSide
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanSetTracking
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanSlew
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanSlewAltAz
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanSlewAltAzAsync
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanSlewAsync
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanSync
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanSyncAltAz
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ITelescope.CanUnpark
        {
            get { throw new System.NotImplementedException(); }
        }

        double ITelescope.Declination
        {
            get { throw new System.NotImplementedException(); }
        }

        double ITelescope.DeclinationRate
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        bool ITelescope.DoesRefraction
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        EquatorialCoordinateType ITelescope.EquatorialSystem
        {
            get { throw new System.NotImplementedException(); }
        }

        double ITelescope.FocalLength
        {
            get { throw new System.NotImplementedException(); }
        }

        double ITelescope.GuideRateDeclination
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        double ITelescope.GuideRateRightAscension
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        bool ITelescope.IsPulseGuiding
        {
            get { throw new System.NotImplementedException(); }
        }

        double ITelescope.RightAscension
        {
            get { throw new System.NotImplementedException(); }
        }

        double ITelescope.RightAscensionRate
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        PierSide ITelescope.SideOfPier
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        double ITelescope.SiderealTime
        {
            get { throw new System.NotImplementedException(); }
        }

        double ITelescope.SiteElevation
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        double ITelescope.SiteLatitude
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        double ITelescope.SiteLongitude
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        bool ITelescope.Slewing
        {
            get { throw new System.NotImplementedException(); }
        }

        short ITelescope.SlewSettleTime
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        double ITelescope.TargetDeclination
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        double ITelescope.TargetRightAscension
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        bool ITelescope.Tracking
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        DriveRates ITelescope.TrackingRate
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        ITrackingRates ITelescope.TrackingRates
        {
            get { throw new System.NotImplementedException(); }
        }

        DateTime ITelescope.UTCDate
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        void ITelescope.AbortSlew()
        {
            throw new System.NotImplementedException();
        }

        IAxisRates ITelescope.AxisRates(TelescopeAxes Axis)
        {
            throw new System.NotImplementedException();
        }

        bool ITelescope.CanMoveAxis(TelescopeAxes Axis)
        {
            throw new System.NotImplementedException();
        }

        PierSide ITelescope.DestinationSideOfPier(double RightAscension, double Declination)
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.FindHome()
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.MoveAxis(TelescopeAxes Axis, double Rate)
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.Park()
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.PulseGuide(GuideDirections Direction, int Duration)
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.SetPark()
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.SlewToAltAz(double Azimuth, double Altitude)
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.SlewToCoordinates(double RightAscension, double Declination)
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.SlewToTarget()
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.SlewToTargetAsync()
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.SyncToAltAz(double Azimuth, double Altitude)
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.SyncToCoordinates(double RightAscension, double Declination)
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.SyncToTarget()
        {
            throw new System.NotImplementedException();
        }

        void ITelescope.Unpark()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IAscomDriverV2 Members

        string IAscomDriverV2.ANewIAscomDriverV2Property
        {
            get { throw new System.NotImplementedException(); }
        }

        void IAscomDriverV2.ANewIAscomDriverV2Method(string NewParameter1, double NewParameter2)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IAscomDriverV1 Members

        bool IAscomDriver.Connected
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        string IAscomDriver.Description
        {
            get { throw new System.NotImplementedException(); }
        }

        string IAscomDriver.DriverInfo
        {
            get { throw new System.NotImplementedException(); }
        }

        string IAscomDriver.DriverVersion
        {
            get { throw new System.NotImplementedException(); }
        }

        short IAscomDriver.InterfaceVersion
        {
            get { throw new System.NotImplementedException(); }
        }

        string IAscomDriver.Name
        {
            get { throw new System.NotImplementedException(); }
        }

        void IAscomDriver.SetupDialog()
        {
            throw new System.NotImplementedException();
        }

        string IAscomDriver.Configuration
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        string[] IAscomDriver.SupportedConfigurations
        {
            get { throw new System.NotImplementedException(); }
        }

        string IAscomDriver.Action(string ActionName, string ActionParameters)
        {
            throw new System.NotImplementedException();
        }

        string IAscomDriver.LastResult
        {
            get { throw new System.NotImplementedException(); }
        }

        string[] IAscomDriver.SupportedActions
        {
            get { throw new System.NotImplementedException(); }
        }

        void IAscomDriver.CommandBlind(string Command, bool Raw)
        {
            throw new System.NotImplementedException();
        }

        bool IAscomDriver.CommandBool(string Command, bool Raw)
        {
            throw new System.NotImplementedException();
        }

        string IAscomDriver.CommandString(string Command, bool Raw)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
