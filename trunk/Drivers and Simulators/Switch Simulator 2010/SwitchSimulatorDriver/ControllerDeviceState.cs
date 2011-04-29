using ASCOM.DeviceInterface;
using System.Runtime.InteropServices;

namespace ASCOM.Simulator
{
    [Guid("BE3C8C90-98F8-4B8B-8C12-C99C8CEC0B7F"), ClassInterface(ClassInterfaceType.None), ComVisible(true)]
    class ControllerDeviceState : IControllerDeviceState
    {
        private double DeviceStartValue;
        private double DeviceEndValue;
        private string DeviceName;
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

        public double EndValue
        {
            get { return DeviceEndValue; }
        }

        public string Name
        {
            get { return DeviceName; }
        }

        public double StartValue
        {
            get { return DeviceStartValue; }
        }

        #endregion
    }
}
