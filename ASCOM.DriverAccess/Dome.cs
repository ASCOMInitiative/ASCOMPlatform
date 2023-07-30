//-----------------------------------------------------------------------
// <summary>Defines the Dome class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.
//
using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Globalization;

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
    public class Dome : AscomDriver, IDomeV2
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

        /// <summary>Immediately stops any and all movement.</summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <p style="color:red">
        /// <b>Must be implemented, must not throw a MethodNotImplementedException.</b>
        /// </p>
        /// Calling this method will immediately disable hardware slewing (<see cref="Slaved" /> will
        /// become <see langword="false" />). Raises an error if a communications failure occurs, or if the
        /// command is known to have failed.
        /// </remarks>
        public void AbortSlew()
        {
            memberFactory.CallMember(3, "AbortSlew", new Type[] { }, new object[] { });
        }

		/// <summary>
		/// The altitude (degrees, horizon zero and increasing positive to 90 zenith) of the part of the sky that the observer wishes to observe.
		/// </summary>
		/// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <para>
		/// The specified altitude is the position on the sky that the observer wishes to observe. It is up to the driver to determine how best to locate the dome aperture in order to expose that part of the sky to the telescope.
		/// This means that the mechanical position to which the Dome moves may not correspond exactly to requested observing altitude because the driver must coordinate
		/// multiple shutters, clamshell segments or roof mechanisms to provide the required aperture on the sky.
		/// </para>
		/// <para>
		/// Raises an error only if no altitude control. If actual dome altitude can not be read, then reports back the altitude of the last slew position.
		/// </para>
		/// </remarks>
		public double Altitude
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Altitude", new Type[] { }, new object[] { })); }
        }

		/// <summary>
		/// <para><see langword="true" /> when the dome is in the home position. Raises an error if not supported.</para>
		/// <para>
		/// This is normally used following a <see cref="FindHome" /> operation. The value is reset
		/// with any azimuth slew operation that moves the dome away from the home position.
		/// </para>
		/// <para>
		/// <see cref="AtHome" /> may optionally also become true during normal slew operations, if the
		/// dome passes through the home position and the dome controller hardware is capable of
		/// detecting that; or at the end of a slew operation if the dome comes to rest at the home
		/// position.
		/// </para>
		/// </summary>
		/// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <para>
		/// The home position is normally defined by a hardware sensor positioned around the dome circumference and represents a fixed, known azimuth reference.
		/// </para>
		/// <para>
		/// Applications should not rely on the reported azimuth position being identical each time <see cref="AtHome" /> is set <see langword="true" />.
		/// For some devices, the home position may encompass a small range of azimuth values, rather than a discrete value, since dome inertia, the resolution of the home position sensor
		/// and/or the azimuth encoder may be insufficient to return the exact same azimuth value on each occasion. On the other hand some dome controllers always force the azimuth
		/// reading to a fixed value whenever the home position sensor is active.
		/// </para>
		/// </remarks>
		public bool AtHome
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "AtHome", new Type[] { }, new object[] { })); }
        }

		/// <summary><see langword="true" /> if the dome is in the programmed park position.</summary>
		/// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <para>
		/// Set only following a <see cref="Park" /> operation and reset with any slew operation. Raises an error if not supported.
		/// </para>
		/// <para>
		/// Applications should not rely on the reported azimuth position being identical each time <see cref="AtPark" /> is set <see langword="true" />.
		/// For some devices, the park position may encompass a small range of azimuth values, rather than a discrete value, since dome inertia, the resolution of the park position sensor
		/// and/or the azimuth encoder may be insufficient to return the exact same azimuth value on each occasion. On the other hand some dome controllers always force the azimuth
		/// reading to a fixed value whenever the park position sensor is active.
		/// </para>
		/// </remarks>
		public bool AtPark
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "AtPark", new Type[] { }, new object[] { })); }
        }

		/// <summary>
		/// The dome azimuth (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270 West). North is true north and not magnetic north.
		/// </summary>
		/// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <para>
		/// The specified azimuth is the position on the sky that the observer wishes to observe. It is up to the driver to determine how best to locate the dome in order to expose that part of the sky to the telescope.
		/// This means that the mechanical position to which the Dome moves may not correspond exactly to requested observing azimuth because the driver must coordinate
		/// multiple shutters, clamshell segments or roof mechanisms to provide the required aperture on the sky.
		/// </para>
		/// <para>
		/// Raises an error only if no azimuth control. If actual dome azimuth can not be read, then reports back the azimuth of the last slew position.
		/// </para>
		/// <para>
		/// The supplied azimuth value is the final azimuth for the dome, not the telescope azimuth. ASCOM Dome drivers do not perform slaving calculations i.e. they do not take account of mount geometry and simply 
		/// move where they are instructed. Any such slaving calculations must be done by the application.
		/// </para>
		/// </remarks>
		public double Azimuth
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Azimuth", new Type[] { }, new object[] { })); }
        }

		/// <summary><see langword="true" /> if driver can perform a search for home position.</summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red">
		/// <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
		/// </p>
		/// </remarks>
		public bool CanFindHome
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanFindHome", new Type[] { }, new object[] { })); }
        }

		/// <summary><see langword="true" /> if the driver is capable of parking the dome.</summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red">
		/// <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
		/// </p>
		/// </remarks>
		public bool CanPark
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanPark", new Type[] { }, new object[] { })); }
        }

		/// <summary><see langword="true" /> if driver is capable of setting dome altitude.</summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red">
		/// <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
		/// </p>
		/// </remarks>
		public bool CanSetAltitude
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSetAltitude", new Type[] { }, new object[] { })); }
        }

		/// <summary>
		/// <see langword="true" /> if driver is capable of rotating the dome (or controlling the roof
		/// mechanism) in order to observe at a given azimuth.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red">
		/// <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
		/// </p>
		/// <para>
		/// This property typically returns <see langword="true" /> for rotating structures and <see langword="false" /> for non-rotating structures.
		/// </para>
		/// </remarks>
		public bool CanSetAzimuth
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSetAzimuth", new Type[] { }, new object[] { })); }
        }

		/// <summary><see langword="true" /> if the driver can set the dome park position.</summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red">
		/// <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
		/// </p>
		/// </remarks>
		public bool CanSetPark
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSetPark", new Type[] { }, new object[] { })); }
        }

		/// <summary>
		/// <see langword="true" /> if the driver is capable of opening and closing the shutter or roof
		/// mechanism.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red">
		/// <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
		/// </p>
		/// </remarks>
		public bool CanSetShutter
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSetShutter", new Type[] { }, new object[] { })); }
        }

		/// <summary><see langword="true" /> if the dome hardware supports slaving to a telescope.</summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red">
		/// <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
		/// </p>
		/// <para>
		/// See the notes for the <see cref="Slaved" /> property. This should only be <see langword="true" /> if the dome hardware has its own built-in slaving mechanism. 
		/// It is not permitted for a dome driver to query a telescope driver directly.
		/// </para>
		/// </remarks>
		/// <seealso cref="Slaved" />
		public bool CanSlave
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSlave", new Type[] { }, new object[] { })); }
        }

		/// <summary>
		/// <see langword="true" /> if the driver is capable of synchronizing the dome azimuth position
		/// using the <see cref="SyncToAzimuth" /> method.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red">
		/// <b>Must be implemented, must not throw a PropertyNotImplementedException.</b>
		/// </p>
		/// </remarks>
		public bool CanSyncAzimuth
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSyncAzimuth", new Type[] { }, new object[] { })); }
        }

		/// <summary>Close the shutter or otherwise shield the telescope from the sky.</summary>
		/// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		public void CloseShutter()
        {
            memberFactory.CallMember(3, "CloseShutter", new Type[] { }, new object[] { });
        }

		/// <summary>Start operation to search for the dome home position.</summary>
		/// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
		/// <exception cref="SlavedException">If slaving is enabled.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// The method should not block and the homing operation should complete asynchronously. After the
		/// home position is established, <see cref="Azimuth" /> is synchronized to the appropriate value
		/// and the <see cref="AtHome" /> property becomes <see langword="true" />.
		/// </remarks>
		public void FindHome()
        {
            memberFactory.CallMember(3, "FindHome", new Type[] { }, new object[] { });
        }

		/// <summary>Open shutter or otherwise expose telescope to the sky.</summary>
		/// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>Raises an error if not supported or if a communications failure occurs.</remarks>
		public void OpenShutter()
        {
            memberFactory.CallMember(3, "OpenShutter", new Type[] { }, new object[] { });
        }

		/// <summary>Rotate dome in azimuth to park position.</summary>
		/// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// After assuming the programmed park position, sets the <see cref="AtPark" /> flag. Raises an error if <see cref="Slaved" /> is <see langword="true" />, if not supported 
		/// or if a communications failure occurred.
		/// </remarks>
		public void Park()
        {
            memberFactory.CallMember(3, "Park", new Type[] { }, new object[] { });
        }

		/// <summary>Set the current azimuth position of dome to the park position.</summary>
		/// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		public void SetPark()
        {
            memberFactory.CallMember(3, "SetPark", new Type[] { }, new object[] { });
        }

		/// <summary>Gets the status of the dome shutter or roof structure.</summary>
		/// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// Raises an error only if no shutter control. If actual shutter status can not be read, then
		/// reports back the last shutter state.
		/// </remarks>
		public ShutterState ShutterStatus
        {
            get { return (ShutterState)memberFactory.CallMember(1, "ShutterStatus", new Type[] { }, new object[] { }); }
        }

		/// <summary><see langword="true"/> if the dome is slaved to the telescope in its hardware, else <see langword="false"/>.</summary>
		/// <exception cref="PropertyNotImplementedException">If Slaved can not be set.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red;margin-bottom:0">
		/// <b>Slaved Read must be implemented and must not throw a PropertyNotImplementedException. </b>
		/// </p>
		/// <p style="color:red;margin-top:0">
		/// <b>Slaved Write can throw a PropertyNotImplementedException.</b>
		/// </p>
		/// Set this property to <see langword="true"/> to enable dome-telescope hardware slaving, if supported (see
		/// <see cref="CanSlave" />). Raises an exception on any attempt to set this property if hardware
		/// slaving is not supported). Always returns <see langword="false"/> if hardware slaving is not supported.
		/// </remarks>
		public bool Slaved
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "Slaved", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "Slaved", new Type[] { }, new object[] { value }); }
        }

		/// <summary>
		/// <see langword="true" /> if any part of the dome is currently moving, <see langword="false" />
		/// if all dome components are stationary.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <p style="color:red;margin-bottom:0">
		/// <b>Slewing must be implemented and must not throw a PropertyNotImplementedException. </b>
		/// </p>
		/// </remarks>
		public bool Slewing
		{
			get { return Convert.ToBoolean( memberFactory.CallMember( 1, "Slewing", new Type[] { }, new object[] { } ) ); }
		}

		/// <summary>Ensure that the requested viewing altitude is available for observing.</summary>
		/// <param name="Altitude">
		/// The desired viewing altitude (degrees, horizon zero and increasing positive to 90 degrees at the zenith)
		/// </param>
		/// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
		/// <exception cref="InvalidValueException">If the supplied altitude is out of range.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// The requested altitude should be interpreted by the driver as the position on the sky that the observer wishes to observe. The driver has detailed knowledge of the physical structure and
		/// must coordinate shutters, roofs or clamshell segments to open an aperture on the sky that satisfies the observer's request.
		/// </remarks>
		public void SlewToAltitude(double Altitude)
        {
            memberFactory.CallMember(3, "SlewToAltitude", new Type[] { typeof(double) }, new object[] { Altitude });
        }

		/// <summary>
		/// Ensure that the requested viewing azimuth is available for observing.
		/// The method should not block and the slew operation should complete asynchronously.
		/// </summary>
		/// <param name="Azimuth">
		/// Desired viewing azimuth (degrees, North zero and increasing clockwise. i.e., 90 East,
		/// 180 South, 270 West)
		/// </param>
		/// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
		/// <exception cref="InvalidValueException">If the requested azimuth is greater than or equal to 360 or less than 0.</exception>
		/// <exception cref="SlavedException">Thrown if <see cref="Slaved" /> is <see langword="true" /></exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// The requested azimuth should be interpreted by the driver as the position on the sky that the observer wishes to observe. The driver has detailed knowledge of the physical structure and
		/// must coordinate shutters, roofs or clamshell segments to open an aperture on the sky that satisfies the observer's request.
		/// </remarks>
		public void SlewToAzimuth(double Azimuth)
        {
            memberFactory.CallMember(3, "SlewToAzimuth", new Type[] { typeof(double) }, new object[] { Azimuth });
        }

		/// <summary>Synchronize the current position of the dome to the given azimuth.</summary>
		/// <param name="Azimuth">Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East, 180 South, 270 West)</param>
		/// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
		/// <exception cref="InvalidValueException">If the supplied azimuth is outside the range 0..360 degrees.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">Must raise an error if the operation cannot be completed.</exception>
		public void SyncToAzimuth(double Azimuth)
        {
            memberFactory.CallMember(3, "SyncToAzimuth", new Type[] { typeof(double) }, new object[] { Azimuth });
        }

        #endregion
    }
}
