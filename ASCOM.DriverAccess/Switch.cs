//-----------------------------------------------------------------------
// <summary>Defines the Switch class.</summary>
//-----------------------------------------------------------------------
// 17-Sep-13  	cdr     6.1.0   Initial definition
using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Provides universal access to Switch drivers
    /// </summary>
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
        /// The number of switches managed by this driver. Switch numbers are 0 to
        /// MaxSwitch - 1
        /// </summary>
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
        public void SetSwitchName(short id, string name)
        {
            memberFactory.CallMember(3, "SetSwitchName", new Type[] { typeof(short), typeof(string) }, new object[] { id, name });
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
        /// <param name="id"></param>
        /// <param name="state"></param>
        public void SetSwitch(short id, bool state)
        {
            memberFactory.CallMember(3, "SetSwitch", new Type[] { typeof(short), typeof(bool) }, new object[] { id, state });
        }

        #endregion

        #region analogue members

        /// <summary>
        /// returns the maximum value for this switch.
        /// boolean switches must return 1.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double MaxSwitchValue(short id)
        {
            try { return (double)memberFactory.CallMember(3, "MaxSwitchValue", new Type[] { typeof(short) }, new object[] { id }); }
            catch (System.NotImplementedException) { return 1.0; }
        }

        /// <summary>
        /// returns the minimum value for this switch.
        /// boolean switches must return 0.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double MinSwitchValue(short id)
        {
            try { return (double)memberFactory.CallMember(3, "MinSwitchValue", new Type[] { typeof(short) }, new object[] { id }); }
            catch (System.NotImplementedException) { return 1.0; }
        }

        /// <summary>
        /// returns the step size that this switch supports. This gives the difference between
        /// successive values of the switch.
        /// The number of values is ((MaxSwitchValue - MinSwitchValue) / SwitchStep) + 1
        /// boolean switches must return 1.0, giving two states.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double SwitchStep(short id)
        {
            try { return (double)memberFactory.CallMember(3, "SwitchStep", new Type[] { typeof(short) }, new object[] { id }); }
            catch (System.NotImplementedException) { return 1.0; }
        }

        /// <summary>
        /// returns the analogue switch value for switch id
        /// boolean switches will return 1.0 or 0.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetSwitchValue(short id)
        {
            try
            {
                return (double)memberFactory.CallMember(3, "GetSwitchValue", new Type[] { typeof(short) }, new object[] { id });
            }
            catch (System.NotImplementedException) { return this.GetSwitch(id) ? 1.0 : 0.0; }
        }

        /// <summary>
        /// set the analogue value for this switch.
        /// If the switch cannot be set then throws a MethodNotImplementedException.
        /// If the value is not between the maximum and minimum then throws an InvalidValueException
        /// boolean switches will be set to true if the value is closer to the maximum than the minimum.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
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
