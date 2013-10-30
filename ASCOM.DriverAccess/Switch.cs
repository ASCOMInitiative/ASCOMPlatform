//-----------------------------------------------------------------------
// <summary>Defines the Switch class.</summary>
//-----------------------------------------------------------------------
// 17-Sep-13  	cdr     6.0.0   Initial definition
using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    {
    /// <summary>
    /// Defines the ISwitchV2 Interface
    /// </summary>
    /// <remarks>
    /// The Switch interface is used to define a number of 'switches'. A switch can be used to control something, such as a power switch
    /// or may be used to sense the state of something, such as a limit switch. Each switch has a CanWrite property, this is true if the
    /// switch can be written to or false if it can only be read.
    /// <para>In addition a switch may have multiple states, from two - a binary on/off switch - through those with a small
    /// number of states to those which have many states - an analogue switch</para>
    /// <para>An Analogue switch may be capable of changing and/or being set to a range of values, these are defined using the MinSwitchValue, MaxSwitchValue and StepSize methods.</para>
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
        /// Return the name of switch n.
        /// </summary>
        /// <param name="id">The switch number</param>
        /// <returns>The name of the switch</returns>
        /// <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        /// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p>
        /// <para>Switches are numbered from 0 to MaxSwitch - 1</para></remarks>
        public short MaxSwitch
        {
            get { return Convert.ToInt16(memberFactory.CallMember(1, "MaxSwitch", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Return the name of switch n. This method is mandatory.
        /// </summary>
        /// <param name="id">The switch number to return</param>
        /// <returns>
        /// The name of the switch
        /// </returns>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        public string GetSwitchName(short id)
        {
            return (string)memberFactory.CallMember(3, "GetSwitchName", new Type[] { typeof(short) }, new object[] { id });
        }

        /// <summary>
        /// Sets a switch name to a specified value.  If the switch name cannot
        /// be set by the application this must return the MethodNetImplementedException.
        /// </summary>
        /// <param name="id">The number of the switch whose name is to be set</param>
        /// <param name="name">The name of the switch</param>
        /// <exception cref="MethodNotImplementedException">If the switch name cannot be set in the application code.</exception>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        public void SetSwitchName(short id, string name)
        {
            memberFactory.CallMember(3, "SetSwitchName", new Type[] { typeof(short), typeof(string) }, new object[] { id, name });
        }

        /// <summary>
        /// Gets the description of the specified switch. This is to allow a fuller description of
        /// the switch to be returned, for example for a tool tip.
        /// </summary>
        /// <param name="id">The number of the switch whose description is to be returned</param>
        /// <returns></returns>
        /// <exception cref="T:ASCOM.MethodNotImplementedException">If the method is not implemented</exception>
        /// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p></remarks>
        public string GetSwitchDescription(short id)
        {
            return (string)memberFactory.CallMember(3, "GetSwitchDescription", new Type[] { typeof(short) }, new object[] { id });
        }

        /// <summary>
        /// Reports if the specified switch can be written to, default true.
        /// This is false if the switch cannot be written to, for example a limit switch or a sensor.
        /// </summary>
        /// <param name="id">The number of the switch whose write state is to be returned</param>
        /// <returns>
        ///   <c>true</c> if the switch can be written to, otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        /// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p></remarks>
        public bool CanWrite(short id)
        {
            return (bool)memberFactory.CallMember(3, "CanWrite", new Type[] { typeof(short) }, new object[] { id });
        }

        #region boolean switch members

        /// <summary>
        /// Return the state of switch n
        /// An analogue switch will return true if the value is closer to the maximum than the minimum, otherwise false
        /// </summary>
        /// <param name="id">The switch number to return</param>
        /// <returns>
        /// True or false
        /// </returns>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        public bool GetSwitch(short id)
        {
            return (bool)memberFactory.CallMember(3, "GetSwitch", new Type[] { typeof(short) }, new object[] { id });
        }

        /// <summary>
        /// Sets a switch to the specified state
        /// If the switch cannot be set then throws a MethodNotImplementedException.
        /// Setting an analogue switch to true will set it to its maximim value and
        /// setting it to false will set it to its minimum value.
        /// </summary>
        /// <param name="id">The number of the switch to set</param>
        /// <param name="state">The required switch state</param>
        /// <exception cref="T:ASCOM.MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        public void SetSwitch(short id, bool state)
        {
            memberFactory.CallMember(3, "SetSwitch", new Type[] { typeof(short), typeof(bool) }, new object[] { id, state });
        }

        #endregion

        #region analogue members

        /// <summary>
        /// Returns the maximum value for this switch.
        /// boolean switches must return 1.0
        /// </summary>
        /// <param name="id">The switch whose value should be returned</param>
        /// <returns>
        /// The maximum value to which this switch can be set.
        /// </returns>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        public double MaxSwitchValue(short id)
        {
            try { return (double)memberFactory.CallMember(3, "MaxSwitchValue", new Type[] { typeof(short) }, new object[] { id }); }
            catch (System.NotImplementedException) { return 1.0; }
        }

        /// <summary>
        /// Returns the minimum value for this switch.
        /// boolean switches must return 0.0
        /// </summary>
        /// <param name="id">The switch whose value should be returned</param>
        /// <returns>
        /// The minimum value to which this switch can be set.
        /// </returns>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        public double MinSwitchValue(short id)
        {
            try { return (double)memberFactory.CallMember(3, "MinSwitchValue", new Type[] { typeof(short) }, new object[] { id }); }
            catch (System.NotImplementedException) { return 1.0; }
        }

        /// <summary>
        /// Returns the step size that this switch supports. This gives the difference between
        /// successive values of the switch.
        /// The number of values is ((MaxSwitchValue - MinSwitchValue) / SwitchStep) + 1
        /// boolean switches must return 1.0, giving two states.
        /// </summary>
        /// <param name="id">TThe switch whose value should be returned</param>
        /// <returns>
        /// The step size for this switch.
        /// </returns>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        public double SwitchStep(short id)
        {
            try { return (double)memberFactory.CallMember(3, "SwitchStep", new Type[] { typeof(short) }, new object[] { id }); }
            catch (System.NotImplementedException) { return 1.0; }
        }

        /// <summary>
        /// Returns the analogue switch value for switch id
        /// boolean switches will return 1.0 or 0.0
        /// </summary>
        /// <param name="id">The switch whose value should be returned</param>
        /// <returns>
        /// The analogue value for this switch.
        /// </returns>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        public double GetSwitchValue(short id)
        {
            try
            {
                return (double)memberFactory.CallMember(3, "GetSwitchValue", new Type[] { typeof(short) }, new object[] { id });
            }
            catch (System.NotImplementedException) { return this.GetSwitch(id) ? 1.0 : 0.0; }
        }

        /// <summary>
        /// Set the analogue value for this switch.
        /// If the switch cannot be set then throws a MethodNotImplementedException.
        /// If the value is not between the maximum and minimum then throws an InvalidValueException
        /// boolean switches will be set to true if the value is closer to the maximum than the minimum.
        /// </summary>
        /// <param name="id">The switch whose value should be set</param>
        /// <param name="value">Value to be set between MinSwitchValue and MaxSwitchValue</param>
        /// <exception cref="T:ASCOM.InvalidValueException">If the value is not between the maximum and minimum.</exception>
        /// <exception cref="T:ASCOM.MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="T:ASCOM.InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
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
