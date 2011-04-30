using System;
using System.Collections;
using System.Linq;
using System.Text;
using ASCOM.DeviceInterface;
using System.Runtime.InteropServices;

namespace ASCOM.Simulator
{
    [Guid("B02BEFFC-6373-441A-A4E3-9A82165E1A0F"), ClassInterface(ClassInterfaceType.None), ComVisible(true)]
    class ToggleSwitch : IControllerDevice
    {
        private string DeviceName;
        private double CurrentState;

        public ToggleSwitch()
        {
        }

        internal ToggleSwitch(string Name)
        {
            DeviceName = Name; // Set the device name as supplied
            CurrentState = 0.0; // Default the current state to off
        }

        #region IControllerDevice Members

        public bool CanReturnState
        {
            get { return true; }
        }

        public bool CanSetState
        {
            get { return true; }
        }

        public double Maximum
        {
            get { return 1.0; }
        }

        public double Minimum
        {
            get { return 0.0; }
        }

        public string Name
        {
            get { return DeviceName; }
        }

        public ArrayList NamedDeviceStates
        {
            get 
            {
                ArrayList DeviceStates = new ArrayList();
                DeviceStates.Add(new ControllerDeviceState(0.0, 0.0, "Off"));
                DeviceStates.Add(new ControllerDeviceState(1.0, 1.0, "On"));
                return DeviceStates; 
            }
        }

        public bool On
        {
            get
            {
                return CurrentState == 1.0;
            }
            set
            {
                if (value) CurrentState = 1.0; // If we are setting on then set state to 1.0
                else CurrentState = 0.0; //Otherwise we are settig off so set CurrentState to 0.0
            }
        }

        public double PresentSetPoint
        {
            get { return CurrentState; }
        }

        public double PresentValue
        {
            get { return CurrentState; }
        }

        public bool SetValue(double NewValue)
        {
            CurrentState = NewValue ;
            return true; // OUr change is instantaneous so return true
        }

        public string StateName
        {
            get 
            {
                string NamedState = "Unknown"; // Initialise the value

                foreach (ControllerDeviceState State in NamedDeviceStates) // Check each named state and save the namecorresponding to the current StartValue
                {
                    if (State.StartValue == CurrentState) NamedState = State.Name;
                }
                return NamedState;
            }
        }

        public double StepSize
        {
            get { return 1.0; }
        }

        #endregion
    }
}
