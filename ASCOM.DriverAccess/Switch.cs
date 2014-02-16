//-----------------------------------------------------------------------
// <summary>Defines the Switch class.</summary>
//-----------------------------------------------------------------------
// 17-Sep-13  	cdr     6.0.0   Initial definition
using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Defines the ISwitchV2 Interface
    /// </summary>
    /// <remarks>
    /// The Switch interface is used to define a number of 'switch devices'. A switch device can be used to control something, such as a power switch
    /// or may be used to sense the state of something, such as a limit switch. Each device has a CanWrite property, this is true if it
    /// can be written to or false if it can only be read and so is a sensor.
    /// <para>In addition a switch device may have multiple states, from two - a binary on/off switch device - through those with a small
    /// number of states to those which have many states.</para>
    /// <para>A multi-state device may be capable of changing and/or being set to a range of values, these are defined using the
    /// <see cref="MinSwitchValue"/>, <see cref="MaxSwitchValue"/> and <see cref="SwitchStep"/> methods.</para>
    /// <para><b>Naming Conventions</b></para>
    /// <para>Each device handled by a Switch is known as a device or switch device for general cases, a controller device if it can alter the state of the device
    /// and a sensor device if it can only be read.</para>
    /// <para>Devices can be of two types, boolean if the device can only have two states, and multi-state if it can have more thn two values.</para>
    /// </remarks>
    public class Switch : AscomDriver, ISwitchV2
    {
        private MemberFactory memberFactory;

        #region Switch constructors

        /// <summary>
        /// Creates a Switch object with the given Prog ID
        /// </summary>
        /// <param name="switchId">ProgID of the Switch device to be accessed.</param>
        public Switch(string switchId)
            : base(switchId)
        {
            memberFactory = base.MemberFactory; //Get the member factory created by the base class
        }

        #region IDisposable Members
        // No member here, we are relying on Dispose in the base class
        #endregion

        #endregion

        #region Convenience Members
        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a Switch
        /// </summary>
        /// <param name="switchId">FilterWheel Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen Switch or null for none</returns>
        public static string Choose(string switchId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Switch";
                return chooser.Choose(switchId);
            }
        }

        #endregion

        #region ISwitchV2 members

        /// <summary>
        /// Return the number of switch devices managed by this driver
        /// </summary>
        /// <returns>The number of devices managed by this driver, in the range 0 to <see cref="MaxSwitch"/> - 1.</returns>
        /// <remarks><p style="color:red"><b>Must be implemented, must not throw an <see cref="T:ASCOM.MethodNotImplementedException"/></b></p>
        /// <para>Devices are numbered from 0 to <see cref="MaxSwitch"/> - 1</para></remarks>
        public short MaxSwitch
        {
            get { return Convert.ToInt16(memberFactory.CallMember(1, "MaxSwitch", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Return the name of switch device n. This method is mandatory.
        /// </summary>
        /// <param name="id">The device number to return</param>
        /// <returns>
        /// The name of the device
        /// </returns>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        public string GetSwitchName(short id)
        {
            return (string)memberFactory.CallMember(3, "GetSwitchName", new Type[] { typeof(short) }, new object[] { id });
        }

        /// <summary>
        /// Sets a switch device name to a specified value.  If the device name cannot
        /// be set by the application this must return the <see cref="T:ASCOM.MethodNetImplementedException"/> .
        /// </summary>
        /// <param name="id">The number of the device whose name is to be set</param>
        /// <param name="name">The name of the device</param>
        /// <exception cref="T:ASCOM.MethodNotImplementedException">If the device name cannot be set in the application code.</exception>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        public void SetSwitchName(short id, string name)
        {
            memberFactory.CallMember(3, "SetSwitchName", new Type[] { typeof(short), typeof(string) }, new object[] { id, name });
        }

        /// <summary>
        /// Gets the description of the specified switch device. This is to allow a fuller description of
        /// the device to be returned, for example for a tool tip.
        /// </summary>
        /// <param name="id">The number of the device whose description is to be returned</param>
        /// <returns>
        ///   String giving the device description.
        /// </returns>
        /// <exception cref="T:ASCOM.MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p>
        /// <para>This method was first introduced in Version 2.</para>
        /// </remarks>
        public string GetSwitchDescription(short id)
        {
            return (string)memberFactory.CallMember(3, "GetSwitchDescription", new Type[] { typeof(short) }, new object[] { id });
        }

        /// <summary>
        /// Reports if the specified switch device can be written to, default true.
        /// This is false if the device cannot be written to, for example a limit switch or a sensor.
        /// </summary>
        /// <param name="id">The number of the device whose write state is to be returned</param>
        /// <returns>
        ///   <c>true</c> if the device can be written to, otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="T:AASCOM.InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p>
        /// <para>This method was first introduced in Version 2.</para>
        /// </remarks>
        public bool CanWrite(short id)
        {
            return (bool)memberFactory.CallMember(3, "CanWrite", new Type[] { typeof(short) }, new object[] { id });
        }

        #region boolean switch members

        /// <summary>
        /// Return the state of boolean switch device id.
        /// A multi-state device will throw the <see cref="T:ASCOM.MethodNotImplementedException"/>
        /// </summary>
        /// <param name="id">The switch number to return</param>
        /// <returns>
        /// True or false
        /// </returns>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <exception cref="T:ASCOM.MethodNotImplementedException">If the device cannot be read or is a multi-state device.</exception>
        public bool GetSwitch(short id)
        {
            return (bool)memberFactory.CallMember(3, "GetSwitch", new Type[] { typeof(short) }, new object[] { id });
        }

        /// <summary>
        /// Sets a switch device to the specified state
        /// If the device cannot be set then throws a <see cref="T:ASCOM.MethodNotImplementedException"/>.
        /// A multi-state device will throw the <see cref="T:ASCOM.MethodNotImplementedException"/>.
        /// </summary>
        /// <param name="id">The number of the device to set</param>
        /// <param name="state">The required boolean device state</param>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <exception cref="T:ASCOM.MethodNotImplementedException">If the device cannot be written to (<see cref="CanWrite"/> is false),
        /// or if the device is multi-state.</exception>
        public void SetSwitch(short id, bool state)
        {
            memberFactory.CallMember(3, "SetSwitch", new Type[] { typeof(short), typeof(bool) }, new object[] { id, state });
        }

        #endregion

        #region analogue members

        /// <summary>
        /// Returns the maximum value for this switch device, this must be greater than <see cref="MinSwitchValue"/>.
        /// </summary>
        /// <param name="id">The device whose value should be returned</param>
        /// <returns>
        /// The maximum value to which this device can be set.
        /// </returns>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <remarks>
        /// <para>Boolean devices must return 1.0 as their maximum value.</para>
        /// <para>This method was first introduced in Version 2.</para>
        /// </remarks>
        public double MaxSwitchValue(short id)
        {
            try { return (double)memberFactory.CallMember(3, "MaxSwitchValue", new Type[] { typeof(short) }, new object[] { id }); }
            catch (System.NotImplementedException) { return 1.0; }
        }

        /// <summary>
        /// Returns the minimum value for this switch device, this must be less than <see cref="MaxSwitchValue"/>.
        /// </summary>
        /// <param name="id">The device whose value should be returned</param>
        /// <returns>
        /// The minimum value to which this device can be set.
        /// </returns>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <remarks>
        /// <para>Boolean devices must return 0.0 as their minimum value.</para>
        /// <para>This method was first introduced in Version 2.</para>
        /// </remarks>
        public double MinSwitchValue(short id)
        {
            try { return (double)memberFactory.CallMember(3, "MinSwitchValue", new Type[] { typeof(short) }, new object[] { id }); }
            catch (System.NotImplementedException) { return 0.0; }
        }

        /// <summary>
        /// Returns the step size that this switch device supports. This gives the difference between
        /// successive values of the device.
        /// The number of values is ((<see cref="MaxSwitchValue"/> - <see cref="MinSwitchValue"/>) / <see cref="SwitchStep"/>) + 1
        /// boolean switches must return 1.0, giving two states.
        /// </summary>
        /// <param name="id">The device number whose value should be returned</param>
        /// <returns>
        /// The step size for this device.
        /// </returns>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <remarks>
        /// <para>The number of states is determined from (<see cref="MaxSwitchValue" /> - <see cref="MinSwitchValue" /> ) / <see cref="SwitchStep"/> + 1
        /// and must be an integer, value 2 for a boolean device and more than 2 for a multi-state device.</para>
        /// <para>SwitchStep can be used to determine the way the device is controlled and/or displayed, for example by setting the
        /// number of decimal places or number of states for a display.</para>
        /// <para>This method was first introduced in Version 2.</para>
        /// </remarks>
        public double SwitchStep(short id)
        {
            try { return (double)memberFactory.CallMember(3, "SwitchStep", new Type[] { typeof(short) }, new object[] { id }); }
            catch (System.NotImplementedException) { return 1.0; }
        }

        /// <summary>
        /// Returns the value for this multi-state switch device.
        /// </summary>
        /// <param name="id">The device number whose value should be returned</param>
        /// <returns>
        /// The analogue value for this device.
        /// </returns>
        /// <exception cref="T:ASCOM.MethodNotImplementedException">If the method is not implemented, boolean devices must throw this.</exception>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <remarks>
        /// <para>This method was first introduced in Version 2.</para>
        /// </remarks>
        public double GetSwitchValue(short id)
        {
            try
            {
                return (double)memberFactory.CallMember(3, "GetSwitchValue", new Type[] { typeof(short) }, new object[] { id });
            }
            catch (System.NotImplementedException) { return this.GetSwitch(id) ? 1.0 : 0.0; }
        }

        /// <summary>
        /// Set the analogue value for this multi-state switch device.
        /// If the switch cannot be set then throws a MethodNotImplementedException.
        /// If the value is not between the maximum and minimum then throws an InvalidValueException
        /// boolean switches will be set to true if the value is closer to the maximum than the minimum.
        /// </summary>
        /// <param name="id">The switch number whose value should be set</param>
        /// <param name="value">Value to be set, between <see cref="MinSwitchValue"/> and <see cref="MaxSwitchValue"/></param>
        /// <exception cref="T:ASCOM.InvalidValueException">If the value is not between the maximum and minimum.</exception>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <exception cref="T:ASCOM.MethodNotImplementedException">If the method is not implemented, boolean devices will throw this
        ///  and analogue switches will if <see cref="CanWrite"/> is false.</exception>
        /// <remarks>
        /// <para>This method was first introduced in Version 2.</para>
        /// </remarks>
        public void SetSwitchValue(short id, double value)
        {
            try
            {
                memberFactory.CallMember(3, "SetSwitchValue", new Type[] { typeof(short), typeof(double) }, new object[] { id, value });
            }
            catch (System.NotImplementedException)
            {
                bool bv = value >= 0.5 ? true : false;
                this.SetSwitch(id, bv);
            }
        }

        #endregion
        #endregion

    }
}
