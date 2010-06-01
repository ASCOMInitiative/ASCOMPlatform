//-----------------------------------------------------------------------
// <summary>Defines the ICamera Interface</summary>
//-----------------------------------------------------------------------
using System;
namespace ASCOM.Interfaces
{
    public interface ICamera : IAscomDriver, IDeviceControl
    {
        void AbortExposure();
        short BinX { get; set; }
        short BinY { get; set; }
        CameraStates CameraState { get; }
        int CameraXSize { get; }
        int CameraYSize { get; }
        bool CanAbortExposure { get; }
        bool CanAsymmetricBin { get; }
        bool CanGetCoolerPower { get; }
        bool CanPulseGuide { get; }
        bool CanSetCCDTemperature { get; }
        bool CanStopExposure { get; }
        double CCDTemperature { get; }
        bool CoolerOn { get; set; }
        double CoolerPower { get; }
        void Dispose();
        double ElectronsPerADU { get; }
        double FullWellCapacity { get; }
        bool HasShutter { get; }
        double HeatSinkTemperature { get; }
        object ImageArray { get; }
        object ImageArrayVariant { get; }
        bool ImageReady { get; }
        bool IsPulseGuiding { get; }
        string LastError { get; }
        double LastExposureDuration { get; }
        string LastExposureStartTime { get; }
        int MaxADU { get; }
        short MaxBinX { get; }
        short MaxBinY { get; }
        int NumX { get; set; }
        int NumY { get; set; }
        double PixelSizeX { get; }
        double PixelSizeY { get; }
        void PulseGuide(GuideDirections Direction, int Duration);
        double SetCCDTemperature { get; set; }
        void StartExposure(double Duration, bool Light);
        int StartX { get; set; }
        int StartY { get; set; }
        void StopExposure();
    }
}
