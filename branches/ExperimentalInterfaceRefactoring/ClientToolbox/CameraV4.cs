using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Interface;

namespace ASCOM.DriverAccess
{
    public class CameraV4 :ICameraV4, IDisposable
    {
        #region IDisposable Members

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region ICameraV4 Members

        public string ANewCameraV4Property
        {
            get { throw new System.NotImplementedException(); }
        }

        public void ANewCameraV4Method()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region ICameraV3 Members

        public string ANewCameraV3Property
        {
            get { throw new System.NotImplementedException(); }
        }

        public void ANewCameraV3Method()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region ICameraV2 Members

        public short BinX
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

        public short BinY
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

        public CameraStates CameraState
        {
            get { throw new System.NotImplementedException(); }
        }

        public int CameraXSize
        {
            get { throw new System.NotImplementedException(); }
        }

        public int CameraYSize
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanAbortExposure
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanAsymmetricBin
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanGetCoolerPower
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanPulseGuide
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanSetCCDTemperature
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanStopExposure
        {
            get { throw new System.NotImplementedException(); }
        }

        public double CCDTemperature
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CoolerOn
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

        public double CoolerPower
        {
            get { throw new System.NotImplementedException(); }
        }

        public double ElectronsPerADU
        {
            get { throw new System.NotImplementedException(); }
        }

        public double FullWellCapacity
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool HasShutter
        {
            get { throw new System.NotImplementedException(); }
        }

        public double HeatSinkTemperature
        {
            get { throw new System.NotImplementedException(); }
        }

        public object ImageArray
        {
            get { throw new System.NotImplementedException(); }
        }

        public object ImageArrayVariant
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool ImageReady
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool IsPulseGuiding
        {
            get { throw new System.NotImplementedException(); }
        }

        public string LastError
        {
            get { throw new System.NotImplementedException(); }
        }

        public double LastExposureDuration
        {
            get { throw new System.NotImplementedException(); }
        }

        public string LastExposureStartTime
        {
            get { throw new System.NotImplementedException(); }
        }

        public int MaxADU
        {
            get { throw new System.NotImplementedException(); }
        }

        public short MaxBinX
        {
            get { throw new System.NotImplementedException(); }
        }

        public short MaxBinY
        {
            get { throw new System.NotImplementedException(); }
        }

        public int NumX
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

        public int NumY
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

        public double PixelSizeX
        {
            get { throw new System.NotImplementedException(); }
        }

        public double PixelSizeY
        {
            get { throw new System.NotImplementedException(); }
        }

        public double SetCCDTemperature
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

        public int StartX
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

        public int StartY
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

        public void AbortExposure()
        {
            throw new System.NotImplementedException();
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            throw new System.NotImplementedException();
        }

        public void StartExposure(double Duration, bool Light)
        {
            throw new System.NotImplementedException();
        }

        public void StopExposure()
        {
            throw new System.NotImplementedException();
        }

        public short BayerOffsetX
        {
            get { throw new System.NotImplementedException(); }
        }

        public short BayerOffsetY
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanFastReadout
        {
            get { throw new System.NotImplementedException(); }
        }

        public double ExposureMax
        {
            get { throw new System.NotImplementedException(); }
        }

        public double ExposureMin
        {
            get { throw new System.NotImplementedException(); }
        }

        public double ExposureResolution
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool FastReadout
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

        public short Gain
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

        public short GainMax
        {
            get { throw new System.NotImplementedException(); }
        }

        public short GainMin
        {
            get { throw new System.NotImplementedException(); }
        }

        public string[] Gains
        {
            get { throw new System.NotImplementedException(); }
        }

        public short PercentCompleted
        {
            get { throw new System.NotImplementedException(); }
        }

        public short ReadoutMode
        {
            get { throw new System.NotImplementedException(); }
        }

        public string[] ReadoutModes
        {
            get { throw new System.NotImplementedException(); }
        }

        public string SensorName
        {
            get { throw new System.NotImplementedException(); }
        }

        public SensorType SensorType
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region IAscomDriver Members

        public bool Connected
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

        public string Description
        {
            get { throw new System.NotImplementedException(); }
        }

        public string DriverInfo
        {
            get { throw new System.NotImplementedException(); }
        }

        public string DriverVersion
        {
            get { throw new System.NotImplementedException(); }
        }

        public short InterfaceVersion
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Name
        {
            get { throw new System.NotImplementedException(); }
        }

        public void SetupDialog()
        {
            throw new System.NotImplementedException();
        }

        public string Configuration
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

        public string[] SupportedConfigurations
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region IDeviceControl Members

        public string Action(string ActionName, string ActionParameters)
        {
            throw new System.NotImplementedException();
        }

        public string LastResult
        {
            get { throw new System.NotImplementedException(); }
        }

        public string[] SupportedActions
        {
            get { throw new System.NotImplementedException(); }
        }

        public void CommandBlind(string Command, bool Raw)
        {
            throw new System.NotImplementedException();
        }

        public bool CommandBool(string Command, bool Raw)
        {
            throw new System.NotImplementedException();
        }

        public string CommandString(string Command, bool Raw)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IDontKnowYet Members

        public string ADontKnowYetProperty
        {
            get { throw new System.NotImplementedException(); }
        }

        public void ADontKnowYetMethod()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
