using ASCOM.DeviceInterface;
using System.Runtime.InteropServices;

namespace ASCOM.Simulator
{
    /// <summary>
    /// Class to hold a description of a device state
    /// </summary>
    [Guid("BE3C8C90-98F8-4B8B-8C12-C99C8CEC0B7F"), ClassInterface(ClassInterfaceType.None), ComVisible(true)]
    public class ControllerDeviceState : IControllerDeviceState
    {
        private double DeviceStartValue;
        private double DeviceEndValue;
        private string DeviceName;
        /// <summary>
        /// Initialiser for the class
        /// </summary>
        public ControllerDeviceState()
        {
        }

        internal ControllerDeviceState(double startValue, double endValue, string name)
        {
            DeviceStartValue = startValue;
            DeviceEndValue = endValue;
            DeviceName = name;
        }
        
        #region IDeviceState Members

        /// <summary>
        /// Value representing the end of this state. Make this the same as start to define a single valued state.
        /// </summary>
        public double EndValue
        {
            get { return DeviceEndValue; }
        }

        /// <summary>
        /// Name of this state
        /// </summary>
        public string Name
        {
            get { return DeviceName; }
        }

        /// <summary>
        /// Value representing the start of this state
        /// </summary>
        public double StartValue
        {
            get { return DeviceStartValue; }
        }

        #endregion
    }
}
