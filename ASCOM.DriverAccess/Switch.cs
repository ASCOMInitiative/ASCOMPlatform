//-----------------------------------------------------------------------
// <summary>Defines the Switch class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Collections;
using System.Globalization;

namespace ASCOM.DriverAccess
{
    #region Switch wrapper
    /// <summary>
    /// Provides universal access to Switch drivers
    /// </summary>
    public class Switch : AscomDriver, ISwitchV2
    {
        #region Switch constructors

        private readonly MemberFactory _memberFactory;

        /// <summary>
        /// Creates a Switch object with the given Prog ID
        /// </summary>
        /// <param name="switchId">ProgID of the device to be accessed.</param>
        public Switch(string switchId) : base(switchId)
		{
            _memberFactory = MemberFactory;
		}
        #endregion

        #region Convenience Members
        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a Switch
        /// </summary>
        /// <param name="switchId">Switch Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen Switch or null for none</returns>
        public static string Choose(string switchId)
        {
            using(var chooser = new Chooser())
            {
                chooser.DeviceType = "Switch";
                return chooser.Choose(switchId);
            }
        }

        #endregion

        #region ISwitch Members

        /// <summary>
        /// Sets a switch to on or off
        /// </summary>
        /// <param name="name">Name=name of switch to set</param> 
        /// <param name="parameters">On or Off</param> 
        public void SetSwitch(string name, string[] parameters)
        {
            _memberFactory.CallMember(3, "SetSwitch", new[] { typeof(string), typeof(string[]) }, new object[] { name, parameters });
        }

        /// <summary>
        /// Returns a switch by name
        /// </summary>
        /// <param name="name">Name=name of switch</param> 
        public object GetSwitch(string name)
        {
            return _memberFactory.CallMember(3, "GetSwitch", new[] { typeof(string) }, new object[] { name });
        }

        /// <summary>
        /// Yields a collection of strings in an arraylist.
        /// </summary>
        public ArrayList Switches
        {
            get { return (ArrayList)_memberFactory.CallMember(1, "Switches", new Type[] { }, new object[] { }); }
        }


        /// <summary>
        /// Returns a string which identifies the type of switch
        /// </summary>
        public string SwitchType
        {
            get { return (String)_memberFactory.CallMember(1, "SwitchType", new Type[] { }, new object[] { }); }
        }

        #endregion
    }
    #endregion

}       
