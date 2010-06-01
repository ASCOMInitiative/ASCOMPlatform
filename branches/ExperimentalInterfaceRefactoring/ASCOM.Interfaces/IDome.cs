//-----------------------------------------------------------------------
// <summary>Defines the IDome Interface</summary>
//-----------------------------------------------------------------------
using System;
namespace ASCOM.Interfaces
{
    public interface IDome : IAscomDriver, IDeviceControl
    {
        void AbortSlew();
        double Altitude { get; }
        bool AtHome { get; }
        bool AtPark { get; }
        double Azimuth { get; }
        bool CanFindHome { get; }
        bool CanPark { get; }
        bool CanSetAltitude { get; }
        bool CanSetAzimuth { get; }
        bool CanSetPark { get; }
        bool CanSetShutter { get; }
        bool CanSlave { get; }
        bool CanSyncAzimuth { get; }
        void CloseShutter();
        void Dispose();
        void FindHome();
        void OpenShutter();
        void Park();
        void SetPark();
        ShutterState ShutterStatus { get; }
        bool Slaved { get; set; }
        bool Slewing { get; }
        void SlewToAltitude(double Altitude);
        void SlewToAzimuth(double Azimuth);
        void SyncToAzimuth(double Azimuth);
    }
}
