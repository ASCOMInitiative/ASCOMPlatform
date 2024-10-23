//-----------------------------------------------------------------------
// <summary>Defines the Dome class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.
//
using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Provides universal access to ASCOM Dome drivers
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface can be used to control most types of observatory structure, including domes (with or without a controllable shutter), clamshells and roll off roofs.
    /// </para>
    /// <para><b>Dome Coordinates</b></para>
    /// <para>
    /// The azimuth and shutter altitude coordinates within this interface refer to positioning of the dome itself and are not sky coordinates from the perspective of the observing device. 
    /// Mount geometry and pier location often mean that the observing device is not located at the centre point of a hemispherical dome and this results in the dome needing to be positioned at different coordinates
    /// than those of the mount in order to expose the required part of the sky to the observing device.
    /// </para>
    /// <para>
    /// Calculating the required dome position is not the responsibility of the dome driver, this must be done by the client application or by an intermediary hub such as the Device Hub. The coordinates sent 
    /// to the dome driver must be those required to position the dome slit in the correct position after allowing for the geometry of the mount.
    /// </para>
    /// <para><b>Dome Slaving</b></para>
    /// <para>
    /// A dome is said to be slaved when its azimuth and shutter altitude are controlled by a "behind the scenes" controller that knows the required telescope observing coordinates and that can calculate the 
    /// required dome position after allowing for mount geometry, observing device orientation etc. When slaved and in operational use most of the dome interface methods are of little relevance 
    /// apart from the shutter control methods.
    /// </para>
    /// <para><b>How to use the Dome interface to implement a Roll-off Roof or Clamshell</b></para>
    /// <para>
    /// A roll off roof or clamshell is implemented using the shutter control as the roof. The properties and methods should be implemented as follows:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <see cref="OpenShutter" /> and <see cref="CloseShutter" /> open and close the roof
    /// </item>
    /// <item>
    /// <see cref="CanFindHome" />, <see cref="CanPark" />, <see cref="CanSetAltitude" />,
    /// <see cref="CanSetAzimuth" />, <see cref="CanSetPark" />, <see cref="CanSlave" /> and
    /// <see cref="CanSyncAzimuth" /> all return <see langword="false" />.
    /// </item>
    /// <item><see cref="CanSetShutter" /> returns <see langword="true" />.</item>
    /// <item><see cref="ShutterStatus" /> should be implemented.</item>
    /// <item>
    /// <see cref="AbortSlew" /> should stop the roof or shutter.
    /// </item>
    /// <item>
    /// <see cref="FindHome" />, <see cref="Park" />, <see cref="SetPark" />,
    /// <see cref="SlewToAltitude" />, <see cref="SlewToAzimuth" /> and
    /// <see cref="SyncToAzimuth" /> all throw <see cref="MethodNotImplementedException" />
    /// </item>
    /// <item>
    /// <see cref="Altitude" /> and <see cref="Azimuth" /> throw  <see cref="PropertyNotImplementedException" />
    /// </item>
    /// </list>
    /// </remarks>
    public class Dome : AscomDriver, IDomeV2, IDomeV3
    {
        private MemberFactory memberFactory;

        #region Dome constructors

        /// <summary>
        /// Constructor for Dome class. Creates a Dome based on the ProgID in the DomeID string.
        /// </summary>
        /// <param name="domeId">The progID of the dome to be instantiated</param>
        public Dome(string domeId)
            : base(domeId)
        {
            memberFactory = base.MemberFactory;
        }
        #endregion

        #region Convenience Members
        /// <summary>
        /// Shows the ASCOM Chooser to select a Dome.
        /// </summary>
        /// <param name="domeId">Prog ID of the default dome to select. Null if no default is to be set.</param>
        /// <returns>The Prog ID of the Dome chosen, or Null if no dome is chose, or the dialog is canceled.</returns>
        public static string Choose(string domeId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Dome";
                return chooser.Choose(domeId);
            }
        }

        /// <summary>
        /// Dome device state
        /// </summary>
        /// <remarks>
        /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on using the class returned by this property.</para>
        /// </remarks>
        public DomeState DomeState
        {
            get
            {
                // Create a state object to return.
                DomeState domeState = new DomeState(DeviceState, TL);
                TL.LogMessage(nameof(DomeState), $"Returning: '{domeState.Altitude}' '{domeState.AtHome}' '{domeState.AtPark}' '{domeState.Azimuth}' '{domeState.ShutterStatus}' '{domeState.Slewing}' '{domeState.TimeStamp}'");

                // Return the device specific state class
                return domeState;
            }
        }

        #endregion

        #region IDome Members

        /// <inheritdoc/>
        public void AbortSlew()
        {
            memberFactory.CallMember(3, "AbortSlew", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
		public double Altitude
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Altitude", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public bool AtHome
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "AtHome", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public bool AtPark
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "AtPark", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public double Azimuth
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Azimuth", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public bool CanFindHome
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanFindHome", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public bool CanPark
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanPark", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public bool CanSetAltitude
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSetAltitude", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public bool CanSetAzimuth
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSetAzimuth", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public bool CanSetPark
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSetPark", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public bool CanSetShutter
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSetShutter", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public bool CanSlave
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSlave", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public bool CanSyncAzimuth
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSyncAzimuth", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public void CloseShutter()
        {
            memberFactory.CallMember(3, "CloseShutter", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
		public void FindHome()
        {
            memberFactory.CallMember(3, "FindHome", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
		public void OpenShutter()
        {
            memberFactory.CallMember(3, "OpenShutter", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
		public void Park()
        {
            memberFactory.CallMember(3, "Park", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
		public void SetPark()
        {
            memberFactory.CallMember(3, "SetPark", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
		public ShutterState ShutterStatus
        {
            get { return (ShutterState)memberFactory.CallMember(1, "ShutterStatus", new Type[] { }, new object[] { }); }
        }

        /// <inheritdoc/>
		public bool Slaved
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "Slaved", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "Slaved", new Type[] { }, new object[] { value }); }
        }

        /// <inheritdoc/>
		public bool Slewing
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "Slewing", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
		public void SlewToAltitude(double Altitude)
        {
            memberFactory.CallMember(3, "SlewToAltitude", new Type[] { typeof(double) }, new object[] { Altitude });
        }

        /// <inheritdoc/>
		public void SlewToAzimuth(double Azimuth)
        {
            memberFactory.CallMember(3, "SlewToAzimuth", new Type[] { typeof(double) }, new object[] { Azimuth });
        }

        /// <inheritdoc/>
		public void SyncToAzimuth(double Azimuth)
        {
            memberFactory.CallMember(3, "SyncToAzimuth", new Type[] { typeof(double) }, new object[] { Azimuth });
        }

        #endregion
    }
}
