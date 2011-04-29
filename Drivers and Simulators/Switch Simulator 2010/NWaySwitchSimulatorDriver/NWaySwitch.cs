using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.DeviceInterface;
using System.Runtime.InteropServices;

namespace ASCOM.Simulator
{
    [Guid("33A846B1-63EE-44B3-937C-5678E0F5B295"), ClassInterface(ClassInterfaceType.None), ComVisible(true)]
    class NWaySwitch
   {
        private string DeviceName;
        private double CurrentValue = 0.0;
        private double MinimumValue = 0.0;
        private double MaximumValue = 10.0;
        private ArrayList DeviceStates = new ArrayList();

        public NWaySwitch()
        {
            SetRangeValues();
        }

        internal NWaySwitch(string Name, double Minimum, double Maximum, double Current)
        {
            DeviceName = Name; // Set the device name as supplied
            CurrentValue = Current; // Default the current state to off
            MinimumValue = Minimum;
            MaximumValue = Maximum;
            SetRangeValues();
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

        public double Maximim
        {
            get { return MaximumValue; }
        }

        public double Minimum
        {
            get { return MinimumValue; }
        }

        public string Name
        {
            get { return DeviceName; }
        }

        public ArrayList NamedDeviceStates
        {
            get 
            {
                return DeviceStates; 
            }
        }

        public bool On
        {
            get
            {
                return CurrentValue == 1.0;
            }
            set
            {
                if (value) CurrentValue = 1.0; // If we are setting on then set state to 1.0
                else CurrentValue = 0.0; //Otherwise we are settig off so set CurrentState to 0.0
            }
        }

        public double PresentSetPoint
        {
            get { return CurrentValue; }
        }

        public double PresentValue
        {
            get { return CurrentValue; }
        }

        public bool SetValue(double NewValue)
        {
            CurrentValue = NewValue ;
            return true; // OUr change is instantaneous so return true
        }

        public string StateName
        {
            get 
            {
                string NamedState = "Unknown"; // Initialise the value

                foreach (ControllerDeviceState State in NamedDeviceStates) // Check each named state and save the namecorresponding to the current StartValue
                {
                    if ((CurrentValue >= State.StartValue) & (CurrentValue <= State.EndValue)) NamedState = State.Name;
                }
                return NamedState;
            }
        }

        public double StepSize
        {
            get { return 1.0; }
        }

        #endregion

       #region Private Members

        /// <summary>
        /// Internal mechanic to change the minimum and maximum values when changed through the UI
        /// </summary>
        /// <param name="Minimum">New minimum value</param>
        /// <param name="Maximum">New maximum value</param>
        internal void SetMinMax(double Minimum, double Maximum)
        {
            MinimumValue = Minimum;
            MaximumValue = Maximum;
            if (CurrentValue < MinimumValue) CurrentValue = MinimumValue;
            if (CurrentValue > MaximumValue) CurrentValue = MaximumValue;
            SetRangeValues();
        }

        private void SetRangeValues()
        {
            DeviceStates.Clear();
            DeviceStates.Add(new ControllerDeviceState(MinimumValue, MinimumValue + 0.5 * (MaximumValue - MinimumValue), "LOW!"));
            DeviceStates.Add(new ControllerDeviceState(MinimumValue + 0.5 * (MaximumValue - MinimumValue), MinimumValue + 0.7 * (MaximumValue - MinimumValue), "Normal")); 
            DeviceStates.Add(new ControllerDeviceState(MinimumValue + 0.7 * (MaximumValue - MinimumValue), MinimumValue + 0.85 * (MaximumValue - MinimumValue), "High"));
            DeviceStates.Add(new ControllerDeviceState(MinimumValue + 0.85 * (MaximumValue - MinimumValue), MaximumValue, "WARNING!"));
        }

       #endregion
   }
}
