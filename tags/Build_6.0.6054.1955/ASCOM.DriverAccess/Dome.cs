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
    #region Dome wrapper
    /// <summary>
    /// Provides universal access to ASCOM Dome drivers
    /// </summary>
    public class Dome : AscomDriver, IDomeV2
    {
        #region Dome constructors

        private MemberFactory memberFactory;

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

        #endregion

        #region IDome Members

        /// <summary>
        /// Immediately cancel current dome operation.
        /// </summary>
        /// <remarks>
        /// Calling this method will immediately disable hardware slewing (<see cref="Slaved" /> will become False).
        /// Raises an error if a communications failure occurs, or if the command is known to have failed. 
        /// </remarks>
        public void AbortSlew()
        {
            memberFactory.CallMember(3, "AbortSlew", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// The dome altitude (degrees, horizon zero and increasing positive to 90 zenith).
        /// </summary>
        /// <remarks>
        /// Raises an error only if no altitude control. If actual dome altitude can not be read,
        /// then reports back the last slew position. 
        /// </remarks>
        public double Altitude
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Altitude", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        ///   Indicates whether the dome is in the home position.
        ///   Raises an error if not supported. 
        /// <para>
        ///   This is normally used following a <see cref="FindHome" /> operation. The value is reset with any azimuth
        ///   slew operation that moves the dome away from the home position.
        /// </para>
        /// <para>
        ///   <see cref="AtHome" /> may also become true durng normal slew operations, if the dome passes through the home position
        ///   and the dome controller hardware is capable of detecting that; or at the end of a slew operation if the dome
        ///   comes to rest at the home position.
        /// </para>
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     The home position is normally defined by a hardware sensor positioned around the dome circumference
        ///     and represents a fixed, known azimuth reference.
        ///   </para>
        ///   <para>
        ///     For some devices, the home position may represent a small range of azimuth values, rather than a discrete
        ///     value, since dome inertia, the resolution of the home position sensor and/or the azimuth encoder may be
        ///     insufficient to return the exact same azimuth value on each occasion. Some dome controllers, on the other
        ///     hand, will always force the azimuth reading to a fixed value whenever the home position sensor is active.
        ///     Because of these potential differences in behaviour, applications should not rely on the reported azimuth
        ///     position being identical each time <see cref="AtHome" /> is set <c>true</c>.
        ///   </para>
        /// </remarks>
        /// [ASCOM-135] TPL - Updated documentation
        public bool AtHome
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "AtHome", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// True if the dome is in the programmed park position.
        /// </summary>
        /// <remarks>
        /// Set only following a <see cref="Park" /> operation and reset with any slew operation.
        /// Raises an error if not supported. 
        /// </remarks>
        public bool AtPark
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "AtPark", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// The dome azimuth (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270 West)
        /// </summary>
        /// <remarks>Raises an error only if no azimuth control. If actual dome azimuth can not be read, then reports back last slew position</remarks>
        public double Azimuth
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Azimuth", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// True if driver can do a search for home position.
        /// </summary>
        public bool CanFindHome
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanFindHome", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// True if driver is capable of setting dome altitude.
        /// </summary>
        public bool CanPark
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanPark", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// True if driver is capable of setting dome altitude.
        /// </summary>
        public bool CanSetAltitude
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSetAltitude", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// True if driver is capable of setting dome azimuth.
        /// </summary>
        public bool CanSetAzimuth
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSetAzimuth", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// True if driver can set the dome park position.
        /// </summary>
        public bool CanSetPark
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSetPark", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// True if driver is capable of automatically operating shutter.
        /// </summary>
        public bool CanSetShutter
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSetShutter", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// True if the dome hardware supports slaving to a telescope.
        /// </summary>
        /// <remarks>See the notes for the <see cref="Slaved" /> property.</remarks>
        public bool CanSlave
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSlave", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// True if driver is capable of synchronizing the dome azimuth position using the Dome.SyncToAzimuth method.
        /// </summary>
        public bool CanSyncAzimuth
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "CanSyncAzimuth", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Close shutter or otherwise shield telescope from the sky.
        /// </summary>
        public void CloseShutter()
        {
            memberFactory.CallMember(3, "CloseShutter", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Start operation to search for the dome home position.
        /// </summary>
        /// <remarks>
        /// After Home position is established initializes <see cref="Azimuth" /> to the default value and sets the <see cref="AtHome" /> flag. 
        /// Exception if not supported or communications failure. Raises an error if <see cref="Slaved" /> is True.
        /// </remarks>
        public void FindHome()
        {
            memberFactory.CallMember(3, "FindHome", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Open shutter or otherwise expose telescope to the sky.
        /// </summary>
        /// <remarks>
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </remarks>
        public void OpenShutter()
        {
            memberFactory.CallMember(3, "OpenShutter", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Rotate dome in azimuth to park position.
        /// </summary>
        /// <remarks>
        /// After assuming programmed park position, sets <see cref="AtPark" /> flag. Raises an error if <see cref="Slaved" /> is True,
        /// or if not supported, or if a communications failure has occurred. 
        /// </remarks>
        public void Park()
        {
            memberFactory.CallMember(3, "Park", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Set the current azimuth, altitude position of dome to be the park position.
        /// </summary>
        /// <remarks>
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </remarks>
        public void SetPark()
        {
            memberFactory.CallMember(3, "SetPark", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Status of the dome shutter or roll-off roof.
        /// </summary>
        /// <remarks>
        /// Raises an error only if no shutter control.
        /// If actual shutter status can not be read, 
        /// then reports back the last shutter state. 
        /// </remarks>
        public ShutterState ShutterStatus
        {
            get { return (ShutterState)memberFactory.CallMember(1, "ShutterStatus", new Type[] { }, new object[] { }); }
        }

        /// <summary>
        /// True if the dome is slaved to the telescope in its hardware, else False.
        /// </summary>
        /// <remarks>
        /// Set this property to True to enable dome-telescope hardware slaving,
        /// if supported (see <see cref="CanSlave" />). Raises an exception on any attempt to set 
        /// this property if hardware slaving is not supported).
        /// Always returns False if hardware slaving is not supported. 
        /// </remarks>
        public bool Slaved
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "Slaved", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "Slaved", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Slew the dome to the given altitude position.
        /// </summary>
        /// <remarks>
        /// Raises an error if <see cref="Slaved" /> is True, if not supported, if a communications failure occurs,
        /// or if the dome can not reach indicated altitude. 
        /// </remarks>
        /// <param name="Altitude">Target dome altitude (degrees, horizon zero and increasing positive to 90 zenith)</param>
        public void SlewToAltitude(double Altitude)
        {
            memberFactory.CallMember(3, "SlewToAltitude", new Type[] { typeof(double) }, new object[] { Altitude });
        }

        /// <summary>
        /// Slew the dome to the given azimuth position.
        /// </summary>
        /// <remarks>
        /// Raises an error if <see cref="Slaved" /> is True, if not supported, if a communications failure occurs,
        /// or if the dome can not reach indicated azimuth. 
        /// </remarks>
        /// <param name="Azimuth">Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East, 180 South, 270 West)</param>
        public void SlewToAzimuth(double Azimuth)
        {
            memberFactory.CallMember(3, "SlewToAzimuth", new Type[] { typeof(double) }, new object[] { Azimuth });
        }

        /// <summary>
        /// True if any part of the dome is currently moving, False if all dome components are steady.
        /// </summary>
        /// <remarks>
        /// Raises an error if <see cref="Slaved" /> is True, if not supported, if a communications failure occurs,
        /// or if the dome can not reach indicated azimuth. 
        /// </remarks>
        public bool Slewing
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "Slewing", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Synchronize the current position of the dome to the given azimuth.
        /// </summary>
        /// <remarks>
        /// Raises an error if not supported or if a communications failure occurs. 
        /// </remarks>
        /// <param name="Azimuth">Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East, 180 South, 270 West)</param>
        public void SyncToAzimuth(double Azimuth)
        {
            memberFactory.CallMember(3, "SyncToAzimuth", new Type[] { typeof(double) }, new object[] { Azimuth });
        }

        #endregion
    }
    #endregion
}
