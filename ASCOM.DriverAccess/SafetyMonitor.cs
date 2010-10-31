//-----------------------------------------------------------------------
// <summary>Defines the SafetyMonitor class.</summary>
//-----------------------------------------------------------------------
// 25-Sept-10  	rem     6.0.0 - Initial draft of the class

using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    #region SafetyMonitor wrapper
    /// <summary>
    /// Provides universal access to SafetyMonitor drivers
    /// </summary>
    public class SafetyMonitor : AscomDriver, ISafetyMonitor
    {
        #region SafetyMonitor constructors

        private readonly MemberFactory _memberFactory;

        /// <summary>
        /// Creates a SafetyMonitor object with the given Prog ID
        /// </summary>
        /// <param name="safetyMonitorId"></param>
        public SafetyMonitor(string safetyMonitorId)
            : base(safetyMonitorId)
        {
            _memberFactory = MemberFactory;
        }
        #endregion

        #region ISafetyMonitor Members

        /// <summary>
        /// Tells is the SafetyMonitor is set to safe
        /// </summary>
        /// <value></value>
        public bool IsSafe
        {
            get { return (bool)_memberFactory.CallMember(1, "IsSafe", new Type[] { }, new object[] { }); }
        }

        #endregion

    }
    #endregion
}
