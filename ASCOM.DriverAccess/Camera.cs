using System;
using System.Collections;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Implements a camera class to access any registered ASCOM Camera
    /// </summary>
    public class Camera : AscomDriver, ICameraV2, ICameraV3, ICameraV4
    {
        private readonly MemberFactory _memberFactory;

        #region Camera constructors

        /// <summary>
        /// Creates an instance of the camera class.
        /// </summary>
        /// <param name="cameraId">The ProgID for the camera</param>
        public Camera(string cameraId)
            : base(cameraId)
        {
            _memberFactory = base.MemberFactory;
        }
        #endregion

        #region Convenience Members

        /// <summary>
        /// The Choose() method returns the DriverID of the selected driver.
        /// Choose() allows you to optionally pass the DriverID of a "current" driver,
        /// and the corresponding camera type is pre-selected in the Chooser///s list.
        /// In this case, the OK button starts out enabled (lit-up); the assumption is that the pre-selected driver has already been configured.
        /// </summary>
        /// <param name="cameraId">Optional DriverID of the previously selected camera that is to be the pre-selected camera in the list.</param>
        /// <returns>
        /// The DriverID of the user selected camera. Null if the dialogue is cancelled.
        /// </returns>
        public static string Choose(string cameraId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Camera";
                return chooser.Choose(cameraId);
            }
        }

        /// <summary>
        /// Camera device state
        /// </summary>
        /// <remarks>
        /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on using the class returned by this property.</para>
        /// </remarks>
        public CameraDeviceState CameraDeviceState
        {
            get
            {
                // Create a state object to return.
                CameraDeviceState cameraDeviceState = new CameraDeviceState(DeviceState, TL);
                TL.LogMessage(nameof(CameraDeviceState), $"Returning: '{cameraDeviceState.CameraState}' '{cameraDeviceState.CCDTemperature}' '{cameraDeviceState.CoolerPower}' '{cameraDeviceState.HeatSinkTemperature}' '{cameraDeviceState.ImageReady}' '{cameraDeviceState.PercentCompleted}' '{cameraDeviceState.TimeStamp}'");

                // Return the device specific state class
                return cameraDeviceState;
            }
        }

        #endregion

        #region ICamera Members

        /// <inheritdoc/>
        public void AbortExposure()
        {
            _memberFactory.CallMember(3, "AbortExposure", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
        public short BinX
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "BinX", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "BinX", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public short BinY
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "BinY", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "BinY", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public double CCDTemperature
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "CCDTemperature", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public CameraStates CameraState
        {
            get { return (CameraStates)_memberFactory.CallMember(1, "CameraState", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public int CameraXSize
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "CameraXSize", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public int CameraYSize
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "CameraYSize", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public bool CanAbortExposure
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CanAbortExposure", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public bool CanAsymmetricBin
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CanAsymmetricBin", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public bool CanGetCoolerPower
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CanGetCoolerPower", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public bool CanPulseGuide
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CanPulseGuide", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public bool CanSetCCDTemperature
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CanSetCCDTemperature", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public bool CanStopExposure
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CanStopExposure", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public bool CoolerOn
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "CoolerOn", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "CoolerOn", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public double CoolerPower
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "CoolerPower", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public double ElectronsPerADU
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "ElectronsPerADU", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public double FullWellCapacity
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "FullWellCapacity", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public bool HasShutter
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "HasShutter", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public double HeatSinkTemperature
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "HeatSinkTemperature", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public object ImageArray
        {
            get { return _memberFactory.CallMember(1, "ImageArray", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public object ImageArrayVariant
        {
            get { return _memberFactory.CallMember(1, "ImageArrayVariant", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
        public bool ImageReady
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "ImageReady", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public bool IsPulseGuiding
        {
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "IsPulseGuiding", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public double LastExposureDuration
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "LastExposureDuration", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public string LastExposureStartTime
        {
            get { return Convert.ToString(_memberFactory.CallMember(1, "LastExposureStartTime", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public int MaxADU
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "MaxADU", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public short MaxBinX
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "MaxBinX", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public short MaxBinY
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "MaxBinY", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public int NumX
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "NumX", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "NumX", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public int NumY
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "NumY", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "NumY", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public double PixelSizeX
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "PixelSizeX", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public double PixelSizeY
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "PixelSizeY", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            _memberFactory.CallMember(3, "PulseGuide", new[] { typeof(GuideDirections), typeof(int) }, new object[] { Direction, Duration });
        }

        /// <inheritdoc/>
        public double SetCCDTemperature
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "SetCCDTemperature", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "SetCCDTemperature", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public void StartExposure(double Duration, bool Light)
        {
            _memberFactory.CallMember(3, "StartExposure", new[] { typeof(double), typeof(bool) }, new object[] { Duration, Light });
        }

        /// <inheritdoc/>
        public int StartX
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "StartX", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "StartX", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public int StartY
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "StartY", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "StartY", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public void StopExposure()
        {
            _memberFactory.CallMember(3, "StopExposure", new Type[] { }, new object[] { });
        }

        #endregion

        #region ICameraV2 members

        /// <inheritdoc/>
        public short BayerOffsetX
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "BayerOffsetX", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public short BayerOffsetY
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "BayerOffsetY", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public bool CanFastReadout
        {
            get
            {
                if (this.DriverInterfaceVersion > 1)
                {
                    return Convert.ToBoolean(_memberFactory.CallMember(1, "CanFastReadout", new Type[] { }, new object[] { }));
                }
                return false;
            }
        }

        /// <inheritdoc/>
        public double ExposureMax
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "ExposureMax", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public double ExposureMin
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "ExposureMin", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public double ExposureResolution
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "ExposureResolution", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public bool FastReadout
        {
            set { _memberFactory.CallMember(2, "FastReadout", new Type[] { }, new object[] { value }); }
            get { return Convert.ToBoolean(_memberFactory.CallMember(1, "FastReadout", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public short Gain
        {
            set { _memberFactory.CallMember(2, "Gain", new Type[] { }, new object[] { value }); }
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "Gain", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public short GainMax
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "GainMax", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public short GainMin
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "GainMin", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public ArrayList Gains
        {
            get { return _memberFactory.CallMember(1, "Gains", new Type[] { }, new object[] { }).ComObjToArrayList(); }
        }

        /// <inheritdoc/>
        public short PercentCompleted
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "PercentCompleted", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public short ReadoutMode
        {
            get { return Convert.ToInt16(_memberFactory.CallMember(1, "ReadoutMode", new Type[] { }, new object[] { })); }
            set { _memberFactory.CallMember(2, "ReadoutMode", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
        public ArrayList ReadoutModes
        {
            get { return _memberFactory.CallMember(1, "ReadoutModes", new Type[] { }, new object[] { }).ComObjToArrayList(); }
        }

        /// <inheritdoc/>
        public string SensorName
        {
            get { return Convert.ToString(_memberFactory.CallMember(1, "SensorName", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public SensorType SensorType
        {
            get { return (SensorType)_memberFactory.CallMember(1, "SensorType", new Type[] { }, new object[] { }); }
        }

        #endregion

        #region ICameraV3 members

        /// <inheritdoc/>
        public int Offset
        {
            get
            {
                if (DriverInterfaceVersion < 3) throw new PropertyNotImplementedException("Offset", "Offset Get is not implemented because the driver interface is ICameraV2 or earlier.");
                return Convert.ToInt32(_memberFactory.CallMember(1, "Offset", new Type[] { }, new object[] { }));
            }
            set
            {
                if (DriverInterfaceVersion < 3) throw new PropertyNotImplementedException("Offset", "Offset Set is not implemented because the driver interface is ICameraV2 or earlier.");
                _memberFactory.CallMember(2, "Offset", new Type[] { }, new object[] { value });
            }
        }

        /// <inheritdoc/>
        public int OffsetMax
        {
            get
            {
                if (DriverInterfaceVersion < 3) throw new PropertyNotImplementedException("OffsetMax", "OffsetMax Get is not implemented because the driver interface is ICameraV2 or earlier.");
                return Convert.ToInt32(_memberFactory.CallMember(1, "OffsetMax", new Type[] { }, new object[] { }));
            }
        }

        /// <inheritdoc/>
        public int OffsetMin
        {
            get
            {
                if (DriverInterfaceVersion < 3) throw new PropertyNotImplementedException("OffsetMin", "OffsetMin Get is not implemented because the driver interface is ICameraV2 or earlier.");
                return Convert.ToInt32(_memberFactory.CallMember(1, "OffsetMin", new Type[] { }, new object[] { }));
            }
        }

        /// <inheritdoc/>
        public ArrayList Offsets
        {
            get
            {
                if (DriverInterfaceVersion < 3) throw new PropertyNotImplementedException("Offsets", "Offsets Get is not implemented because the driver interface is ICameraV2 or earlier.");
                return _memberFactory.CallMember(1, "Offsets", new Type[] { }, new object[] { }).ComObjToArrayList();
            }
        }

        /// <inheritdoc/>
        public double SubExposureDuration
        {
            get
            {
                if (DriverInterfaceVersion < 3) throw new PropertyNotImplementedException("SubExposureDuration", "SubExposureDuration Get is not implemented because the driver interface is ICameraV2 or earlier.");
                return Convert.ToDouble(_memberFactory.CallMember(1, "SubExposureDuration", new Type[] { }, new object[] { }));
            }
            set
            {
                if (DriverInterfaceVersion < 3) throw new PropertyNotImplementedException("SubExposureDuration", "SubExposureDuration Set is not implemented because the driver interface is ICameraV2 or earlier.");
                _memberFactory.CallMember(2, "SubExposureDuration", new Type[] { }, new object[] { value });
            }
        }

        #endregion

    }
}
