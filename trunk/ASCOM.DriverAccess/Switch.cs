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
    public class Switch : AscomDriver, ISwitch
    {
        #region Switch constructors

        private readonly MemberFactory _memberFactory;

        /// <summary>
        /// Creates a Switch object with the given Prog ID
        /// </summary>
        /// <param name="switchId"></param>
        public Switch(string switchId) : base(switchId)
		{
            _memberFactory = base.MemberFactory;
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
            using(Chooser chooser = new Chooser())
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
        /// <param name="Name">Name=name of switch to set</param> 
        /// <param name="State">True=On, False=Off</param> 
        public void SetSwitch(string Name, bool State)
        {
            _memberFactory.CallMember(3, "SetSwitch", new[] { typeof(string), typeof(bool) }, new object[] { Name, State }, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Yields a collection of strings in an arraylist.
        /// </summary>
        /// <value></value>
        public ArrayList Switches
        {
            get { return (ArrayList)_memberFactory.CallMember(1, "Switches", new Type[] { }, new object[] { }); }
        }
     
        #endregion
    
    }
    #endregion
}
