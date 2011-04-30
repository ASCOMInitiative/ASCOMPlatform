using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.DeviceInterface;
using System.Runtime.InteropServices;

namespace ASCOM.MultiDeviceSimulator
{
    /// <summary>
    /// Class to rspresent a controlled device
    /// </summary>
    [Guid("33A846B1-63EE-44B3-937C-5678E0F5B295"), ClassInterface(ClassInterfaceType.None), ComVisible(true)]
    [ProgId("ASCOM.NWaySwitchSimulator.Device")] //Force the ProgID we want to have
    public class Device : IControllerDevice
   {
        private string DeviceName;
        private double CurrentValue = 0.0;
        private double MinimumValue = 0.0;
        private double MaximumValue = 10.0;
        private ArrayList DeviceStates = new ArrayList();

        /// <summary>
        /// Initialiser for the class
        /// </summary>
        public Device()
        {
            SetRangeValues();
        }

        internal Device(string Name, double Minimum, double Maximum, double Current)
        {
            DeviceName = Name; // Set the device name as supplied
            CurrentValue = Current; // Default the current state to off
            MinimumValue = Minimum;
            MaximumValue = Maximum;
            SetRangeValues();
        }

        #region IControllerDevice Members

        /// <summary>
        /// Flag indicating whether this class can return state
        /// </summary>
        public bool CanReturnState
        {
            get { return true; }
        }

        /// <summary>
        /// Flag indicating that the state can be set on this device
        /// </summary>
        public bool CanSetState
        {
            get { return true; }
        }

        /// <summary>
        /// Maximum value to which this device can be set
        /// </summary>
        public double Maximum
        {
            get { return MaximumValue; }
        }

        /// <summary>
        /// Minimum value to which this device can be set
        /// </summary>
        public double Minimum
        {
            get { return MinimumValue; }
        }

       /// <summary>
       /// Name of this device
       /// </summary>
        public string Name
        {
            get { return DeviceName; }
        }

        /// <summary>
        /// Collection of device state objects describing the sates that this device can have
        /// </summary>
        public ArrayList NamedDeviceStates
        {
            get 
            {
                return DeviceStates; 
            }
        }

        /// <summary>
        /// Simple boolean on/off method for this device
        /// </summary>
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

        /// <summary>
        /// Required value for this device as last set
        /// </summary>
        public double PresentSetPoint
        {
            get { return CurrentValue; }
        }

        /// <summary>
        /// Actual value of this device right now
        /// </summary>
        public double PresentValue
        {
            get { return CurrentValue; }
        }

       /// <summary>
       /// Set the required value for this device
       /// </summary>
       /// <param name="NewValue"></param>
       /// <returns></returns>
        public bool SetValue(double NewValue)
        {
            CurrentValue = NewValue ;
            return true; // OUr change is instantaneous so return true
        }

        /// <summary>
        /// Name of the current state for this device
        /// </summary>
        public string StateName
        {
            get 
            {
                string NamedState = "Unknown"; // Initialise the value

                foreach (ASCOM.Simulator.ControllerDeviceState State in NamedDeviceStates) // Check each named state and save the namecorresponding to the current StartValue
                {
                    if ((CurrentValue >= State.StartValue) & (CurrentValue <= State.EndValue)) NamedState = State.Name;
                }
                return NamedState;
            }
        }

        /// <summary>
        /// Size of the steps supported by this device
        /// </summary>
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
            DeviceStates.Add(new ASCOM.Simulator.ControllerDeviceState(MinimumValue, MinimumValue + 0.5 * (MaximumValue - MinimumValue), "LOW!"));
            DeviceStates.Add(new ASCOM.Simulator.ControllerDeviceState(MinimumValue + 0.5 * (MaximumValue - MinimumValue), MinimumValue + 0.7 * (MaximumValue - MinimumValue), "Normal"));
            DeviceStates.Add(new ASCOM.Simulator.ControllerDeviceState(MinimumValue + 0.7 * (MaximumValue - MinimumValue), MinimumValue + 0.85 * (MaximumValue - MinimumValue), "High"));
            DeviceStates.Add(new ASCOM.Simulator.ControllerDeviceState(MinimumValue + 0.85 * (MaximumValue - MinimumValue), MaximumValue, "WARNING!"));
        }

       #endregion

        #region IDisposable Members

        /// <summary>
        /// Cleans up this object prior to disposal
        /// </summary>
        public void Dispose()
        {
        }

        #endregion
   }
}
