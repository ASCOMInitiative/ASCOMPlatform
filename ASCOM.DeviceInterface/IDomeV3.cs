using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    /// <summary>Defines the IDome Interface</summary>
    /// <remarks>
    /// <para>
    /// This interface can be used to control most types of observatory structures, including domes (with or without a controllable shutter), clamshells, and roll off roofs.
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
    /// <see cref="CanSyncAzimuth" /> all return <c>false</c>.
    /// </item>
    /// <item><see cref="CanSetShutter" /> returns <c>true</c>.</item>
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
    [Guid("CCB802F1-32AF-4806-91BE-9ADF05477700")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IDomeV3 // CCDA0D85-474A-4775-8105-1D513ADC3896
    {
        #region IDome members

        /// <summary>
        /// Set <see langword="true" /> to connect to the device hardware. Set <see langword="false" /> to
        /// disconnect from the device hardware. You can also read the property to check whether it is
        /// connected. This reports the current hardware state.
        /// </summary>
        /// <value><see langword="true" /> if connected to the hardware; otherwise, <see langword="false" />.</value>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.Connected">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="IDomeV3" version="Platform 7.0">Clients should use the Connect() / Disconnect() mechanic rather than setting Connected TRUE when accessing IDomeV3 or later devices.</revision>
        /// </revisionHistory>
        bool Connected { get; set; }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII
        /// characters may be used.
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.Description">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        string Description { get; }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.DriverInfo">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        string DriverInfo { get; }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.DriverVersion">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        string DriverVersion { get; }

        /// <summary>
        /// The interface version number that this device supports. Should return 3 for this interface
        /// version.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.InterfaceVersion">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        short InterfaceVersion { get; }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.Name">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        string Name { get; }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return until the user
        /// clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.SetupDialog">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void SetupDialog();

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response. Optionally,
        /// protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <see langword="true" /> the string is transmitted 'as-is'. If set to
        /// <see langword="false" /> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.CommandBlind">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="IDomeV3" version="Platform 7.0">Deprecated, see note above.</revision>
        /// </revisionHistory>
        void CommandBlind(string Command, bool Raw = false);

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response. Optionally,
        /// protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <see langword="true" /> the string is transmitted 'as-is'. If set to
        /// <see langword="false" /> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>Returns the interpreted boolean response received from the device.</returns>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.CommandBool">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="IDomeV3" version="Platform 7.0">Deprecated, see note above.</revision>
        /// </revisionHistory>
        bool CommandBool(string Command, bool Raw = false);

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response. Optionally,
        /// protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <see langword="true" /> the string is transmitted 'as-is'. If set to
        /// <see langword="false" /> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>Returns the string response received from the device.</returns>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.CommandString">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="IDomeV3" version="Platform 7.0">Deprecated, see note above.</revision>
        /// </revisionHistory>
        string CommandString(string Command, bool Raw = false);

        /// <summary>
        /// Immediately stops any and all movement.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.AbortSlew">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="IDomeV3" version="Platform 7.0">AbortSlew must behave asynchronously, see note above.</revision>
        /// </revisionHistory>
        void AbortSlew();

        /// <summary>
        /// The altitude (degrees, horizon zero and increasing positive to 90 zenith) of the part of the sky that the observer wishes to observe.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.Altitude">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double Altitude { get; }

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
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.AtHome">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool AtHome { get; }

        /// <summary>
        /// <see langword="true" /> if the dome is in the programmed park position.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.AtPark">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool AtPark { get; }

        /// <summary>
        /// The dome azimuth (degrees, North zero and increasing clockwise, i.e., 90 East, 180 South, 270 West). North is true north and not magnetic north.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.Azimuth">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double Azimuth { get; }

        /// <summary>
        /// <see langword="true" /> if driver can perform a search for home position.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.CanFindHome">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanFindHome { get; }

        /// <summary>
        /// <see langword="true" /> if the driver is capable of parking the dome.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.CanPark">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanPark { get; }

        /// <summary>
        /// <see langword="true" /> if driver is capable of setting dome altitude.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.CanSetAltitude">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetAltitude { get; }

        /// <summary>
        /// <see langword="true" /> if driver is capable of rotating the dome (or controlling the roof
        /// mechanism) in order to observe at a given azimuth.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.CanSetAzimuth">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetAzimuth { get; }

        /// <summary>
        /// <see langword="true" /> if the driver can set the dome park position.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.CanSetPark">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetPark { get; }

        /// <summary>
        /// <see langword="true" /> if the driver is capable of opening and closing the shutter or roof
        /// mechanism.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.CanSetShutter">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetShutter { get; }

        /// <summary>
        /// <see langword="true" /> if the dome hardware supports slaving to a telescope.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.CanSlave">Canonical Definition</see></remarks>
        /// <seealso cref="Slaved" />
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSlave { get; }

        /// <summary>
        /// <see langword="true" /> if the driver is capable of synchronizing the dome azimuth position
        /// using the <see cref="SyncToAzimuth" /> method.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.CanSyncAzimuth">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSyncAzimuth { get; }

        /// <summary>
        /// Close the shutter or otherwise shield the telescope from the sky.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.CloseShutter">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void CloseShutter();

        /// <summary>
        /// Start operation to search for the dome home position.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="SlavedException">Thrown if slaving is enabled.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.FindHome">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void FindHome();

        /// <summary>
        /// Open shutter or otherwise expose telescope to the sky.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.OpenShutter">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void OpenShutter();

        /// <summary>
        /// Rotate dome in azimuth to park position.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.Park">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void Park();

        /// <summary>
        /// Set the current azimuth position of dome to the park position.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the operation cannot be completed.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.SetPark">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void SetPark();

        /// <summary>
        /// Gets the status of the dome shutter or roof structure.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.ShutterStatus">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        ShutterState ShutterStatus { get; }

        /// <summary>
        /// <see langword="true"/> if the dome is slaved to the telescope in its hardware, else <see langword="false"/>.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If Slaved can not be set.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.Slaved">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool Slaved { get; set; }

        /// <summary>
        /// <see langword="true" /> if any part of the dome is currently moving, <see langword="false" />
        /// if all dome components are stationary.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.Slewing">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool Slewing { get; }

        /// <summary>
        /// Ensure that the requested viewing altitude is available for observing.
        /// </summary>
        /// <param name="Altitude">
        /// The desired viewing altitude (degrees, horizon zero and increasing positive to 90 degrees at the zenith)
        /// </param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="InvalidValueException">If the supplied altitude is out of range.</exception>
        /// <exception cref="SlavedException">Thrown if slaving is enabled.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.SlewToAltitude">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void SlewToAltitude(double Altitude);

        /// <summary>
        /// Ensure that the requested viewing azimuth is available for observing.
        /// The method should not block and the slew operation should complete asynchronously.
        /// </summary>
        /// <param name="Azimuth">
        /// Desired viewing azimuth (degrees, North zero and increasing clockwise. i.e., 90 East,
        /// 180 South, 270 West)
        /// </param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="InvalidValueException">Thrown if the requested azimuth is greater than or equal to 360 or less than 0.</exception>
        /// <exception cref="SlavedException">Thrown if <see cref="Slaved" /> is <see langword="true" /></exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.SlewToAzimuth">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void SlewToAzimuth(double Azimuth);

        /// <summary>
        /// Synchronize the current position of the dome to the given azimuth.
        /// </summary>
        /// <param name="Azimuth">
        /// Target azimuth (degrees, North zero and increasing clockwise. i.e., 90 East,
        /// 180 South, 270 West)
        /// </param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="InvalidValueException">If the supplied azimuth is outside the range 0..360 degrees.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.SyncToAzimuth">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDome" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void SyncToAzimuth(double Azimuth);

        #endregion

        #region IDomeV2 members

        /// <summary>
        /// Invokes the specified device-specific custom action.
        /// </summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        /// <exception cref="MethodNotImplementedException">Thrown if no actions are supported.</exception>
        /// <exception cref="ActionNotImplementedException">It is intended that the <see cref="SupportedActions"/> method will inform clients of driver capabilities, but the driver must still throw 
        /// an <see cref="ActionNotImplementedException"/> exception  if it is asked to perform an action that it does not support.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.Action">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDomeV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Action(string ActionName, string ActionParameters);

        /// <summary>
        /// This method is a "clean-up" method that is primarily of use to drivers that are written in languages such as C# and VB.NET where resource clean-up is initially managed by the language's 
        /// runtime garbage collection mechanic. Driver authors should take care to ensure that a client or runtime calling Dispose() does not adversely affect other connected clients.
        /// Applications should not call this method.
        /// </summary>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.Dispose">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDomeV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        void Dispose();

        /// <summary>
        /// Returns the list of custom action names supported by this driver.
        /// </summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.SupportedActions">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDomeV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        ArrayList SupportedActions { get; }

        #endregion

        #region IDomeV3 members

        /// <summary>
        /// Connect to the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.Connect">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDomeV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Connect();

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.Disconnect">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDomeV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Disconnect();

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.Connecting">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDomeV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        bool Connecting { get; }

        /// <summary>
        /// Returns the device's operational state in a single call.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/dome.html#Dome.DeviceState">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IDomeV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        IStateValueCollection DeviceState { get; }

        #endregion

    }
}