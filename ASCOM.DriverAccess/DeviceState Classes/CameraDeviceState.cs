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
    public class CameraDeviceState
    {
        /// <summary>
        /// 
        /// </summary>
        public CameraDeviceState() { }

        /// <summary>
        /// 
        /// </summary>
        public CameraDeviceState(CameraStates? cameraState, double? ccdTemperature, double? coolerPower, double? heatSinkTemperature, bool? imageReady, bool? isPulseGuiding, short? percentCompleted, DateTime? timeStamp)
        {
            CameraState = cameraState;
            CCDTemperature = ccdTemperature;
            CoolerPower = coolerPower;
            HeatSinkTemperature = heatSinkTemperature;
            ImageReady = imageReady;
            IsPulseGuiding = isPulseGuiding;
            PercentCompleted = percentCompleted;
            TimeStamp = timeStamp;
        }

        /// <summary>
        /// 
        /// </summary>
        public CameraStates? CameraState { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public double? CCDTemperature { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public double? CoolerPower { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public double? HeatSinkTemperature { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public bool? ImageReady { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public bool? IsPulseGuiding { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public double? PercentCompleted { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public DateTime? TimeStamp { get; set; } = null;
    }
}
