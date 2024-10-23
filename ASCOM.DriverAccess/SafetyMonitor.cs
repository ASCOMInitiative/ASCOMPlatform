//-----------------------------------------------------------------------
// <summary>Defines the SafetyMonitor class.</summary>
//-----------------------------------------------------------------------
// 25-Sept-10  	rem     6.0.0 - Initial draft of the class

using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Provides universal access to SafetyMonitor drivers
    /// </summary>
    public class SafetyMonitor : AscomDriver, ISafetyMonitor, ISafetyMonitorV3
    {
        private readonly MemberFactory _memberFactory;

        #region SafetyMonitor constructors

        /// <summary>
        /// Creates a SafetyMonitor object with the given Prog ID
        /// </summary>
        /// <param name="safetyMonitorId">ProgID of the device to be accessed.</param>
        public SafetyMonitor(string safetyMonitorId)
            : base(safetyMonitorId)
        {
            _memberFactory = MemberFactory;
        }
        #endregion

        #region Convenience Members
        /// <summary>
        /// Brings up the ASCOM Chooser Dialogue to choose a SafetyMonitor
        /// </summary>
        /// <param name="safetyMonitorId">SafetyMonitor Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen SafetyMonitor or null for none</returns>
        public static string Choose(string safetyMonitorId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "SafetyMonitor";
                return chooser.Choose(safetyMonitorId);
            }
        }

        /// <summary>
        /// SafetyMonitor device state
        /// </summary>
        /// <remarks>
        /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on using the class returned by this property.</para>
        /// </remarks>
        public SafetyMonitorState SafetyMonitorState
        {
            get
            {
                // Create a state object to return.
                SafetyMonitorState safetyMonitorState = new SafetyMonitorState(DeviceState, TL);
                TL.LogMessage(nameof(SafetyMonitorState), $"Returning: " +
                    $"Cloud cover: '{safetyMonitorState.IsSafe}', " +
                    $"Time stamp: '{safetyMonitorState.TimeStamp}'");

                // Return the device specific state class
                return safetyMonitorState;
            }
        }

        #endregion

        #region ISafetyMonitor Members

        /// <inheritdoc/>
        public bool IsSafe
        {
            get { return (bool)_memberFactory.CallMember(1, "IsSafe", new Type[] { }, new object[] { }); }
        }

        #endregion

    }
}
