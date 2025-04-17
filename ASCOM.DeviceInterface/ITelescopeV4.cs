using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Defines the ITelescope Interface
    /// </summary>
    [ComVisible(true)]
    [Guid("D43BC69B-C9FA-47CC-A346-13B6FDC8AF71")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ITelescopeV4
    {

        #region ITelescopeV1 members

        /// <summary>
        /// Stops a slew in progress.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="ParkedException">If the telescope is parked</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// Effective only after a call to <see cref="SlewToTargetAsync" />, <see cref="SlewToCoordinatesAsync" />, <see cref="SlewToAltAzAsync" />, or <see cref="MoveAxis" />.
        /// Does nothing if no slew/motion is in progress. Tracking is returned to its pre-slew state. Raises an error if <see cref="AtPark" /> is true.
        /// <para>
        /// <legacyBold>ITelescopeV4 Behaviour Clarification</legacyBold> - Historically this method could operate synchronously or asynchronously with <see cref="Slewing"/> = <see langword="false"/> indicating that movement had ceased.
        /// In ITelescopeV4 and later, <see cref="AbortSlew"/> is required to operate asynchronously using <see cref="Slewing"/> as the completion property. Synchronous behaviour is no longer supported and will be flagged 
        /// as an issue by Conform Universal.
        /// </para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">AbortSlew must behave asynchronously, see note above.</revision>
        /// </revisionHistory>
        void AbortSlew();

        /// <summary>
        /// The alignment mode of the mount (Alt/Az, Polar, German Polar).
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        AlignmentModes AlignmentMode { get; }

        /// <summary>
        /// The Altitude above the local horizon of the telescope's current position (degrees, positive up)
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double Altitude { get; }

        /// <summary>
        /// The telescope's effective aperture diameter (meters)
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// This is only available for telescope InterfaceVersions 2 and later.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double ApertureDiameter { get; }

        /// <summary>
        /// The azimuth at the local horizon of the telescope's current position (degrees, North-referenced, positive East/clockwise).
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double Azimuth { get; }

        /// <summary>
        /// True if this telescope is capable of programmed finding its home position (<see cref="FindHome" /> method).
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// May raise an error if the telescope is not connected.
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanFindHome { get; }

        /// <summary>
        /// True if this telescope is capable of programmed parking (<see cref="Park" />method)
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// May raise an error if the telescope is not connected.
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanPark { get; }

        /// <summary>
        /// True if this telescope is capable of software-pulsed guiding (via the <see cref="PulseGuide" /> method)
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// May raise an error if the telescope is not connected.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanPulseGuide { get; }

        /// <summary>
        /// True if this telescope is capable of programmed setting of its park position (<see cref="SetPark" /> method)
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// May raise an error if the telescope is not connected.
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetPark { get; }

        /// <summary>
        /// True if the <see cref="Tracking" /> property can be changed, turning telescope sidereal tracking on and off.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// May raise an error if the telescope is not connected.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetTracking { get; }

        /// <summary>
        /// True if this telescope is capable of programmed slewing (synchronous or asynchronous) to equatorial coordinates
        /// </summary>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a NotImplementedException.</b></p>
        /// <para>May raise an error if the telescope is not connected.</para>
        /// <para>
        /// <legacyBold>ITelescopeV4 clarification</legacyBold> - Synchronous methods are deprecated in ITelescopeV4 onward. Clients and Alpaca devices should not use / implement them. However, COM Driver authors must 
        /// implement synchronous methods if the mount can slew, to ensure backward compatibility. 
        /// </para>
        /// <para>
        /// See <conceptualLink target="c37349a5-a535-47d6-8c30-11c620932213"/> for more information.
        /// </para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Synchronous slewing is deprecated, see note above.</revision>
        /// </revisionHistory>
        bool CanSlew { get; }

        /// <summary>
        /// True if this telescope is capable of programmed asynchronous slewing to equatorial coordinates.
        /// </summary>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be 
        /// accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a NotImplementedException.</b></p>
        /// <para>May raise an error if the telescope is not connected.</para>
        /// <para>
        /// Further explanation is available in this link: <a href="https://ascom-standards.org/newdocs/telescope.html#telescope-canslewasync" target="_blank">Master Interface Document</a>.
        /// </para>
        /// <legacyBold>ITelescoipeV4 clarification</legacyBold> - If the mount can slew, driver authors must implement both synchronous and asynchronous slewing. Alpaca devices must only implement asynchronous slewing.
        /// See <conceptualLink target="c37349a5-a535-47d6-8c30-11c620932213"/> for more information.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Synchronous slewing is deprecated, see note above.</revision>
        /// </revisionHistory>
        bool CanSlewAsync { get; }

        /// <summary>
        /// True if this telescope is capable of programmed syncing to equatorial coordinates.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// May raise an error if the telescope is not connected.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSync { get; }

        /// <summary>
        /// True if this telescope is capable of programmed unparking (<see cref="Unpark" /> method).
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// If this is true, then <see cref="CanPark" /> will also be true. May raise an error if the telescope is not connected.
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanUnpark { get; }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>May throw a NotImplementedException.</b></p>
        /// <para>The CommandXXX methods are a historic mechanic that provides clients with direct and unimpeded access to change device hardware configuration. While highly enabling for clients, this mechanic is inherently risky
        /// because clients can fundamentally change hardware operation without the driver being aware that a change is taking / has taken place.</para>
        /// <para>The newer Action / SupportedActions mechanic provides discrete, named, functions that can deliver any functionality required.They do need driver authors to make provision for them within the 
        /// driver, but this approach is much lower risk than using the CommandXXX methods because it enables the driver to resolve conflicts between standard device interface commands and extended commands 
        /// provided as Actions.The driver is always aware of what is happening and can adapt more effectively to client needs.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member present.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Deprecated, see note above.</revision>
        /// </revisionHistory>
        void CommandBlind(string Command, bool Raw = false);

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>May throw a NotImplementedException.</b></p>
        /// <para>The CommandXXX methods are a historic mechanic that provides clients with direct and unimpeded access to change device hardware configuration. While highly enabling for clients, this mechanic is inherently risky
        /// because clients can fundamentally change hardware operation without the driver being aware that a change is taking / has taken place.</para>
        /// <para>The newer Action / SupportedActions mechanic provides discrete, named, functions that can deliver any functionality required.They do need driver authors to make provision for them within the 
        /// driver, but this approach is much lower risk than using the CommandXXX methods because it enables the driver to resolve conflicts between standard device interface commands and extended commands 
        /// provided as Actions.The driver is always aware of what is happening and can adapt more effectively to client needs.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member present.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Deprecated, see note above.</revision>
        /// </revisionHistory>
        bool CommandBool(string Command, bool Raw = false);

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>May throw a NotImplementedException.</b></p>
        /// <para>The CommandXXX methods are a historic mechanic that provides clients with direct and unimpeded access to change device hardware configuration. While highly enabling for clients, this mechanic is inherently risky
        /// because clients can fundamentally change hardware operation without the driver being aware that a change is taking / has taken place.</para>
        /// <para>The newer Action / SupportedActions mechanic provides discrete, named, functions that can deliver any functionality required.They do need driver authors to make provision for them within the 
        /// driver, but this approach is much lower risk than using the CommandXXX methods because it enables the driver to resolve conflicts between standard device interface commands and extended commands 
        /// provided as Actions.The driver is always aware of what is happening and can adapt more effectively to client needs.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member present.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Deprecated, see note above.</revision>
        /// </revisionHistory>
        string CommandString(string Command, bool Raw = false);

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p>
        /// <para>Do not use a NotConnectedException here. That exception is for use in other methods that require a connection in order to succeed.</para>
        /// <para>The Connected property sets and reports the state of connection to the device hardware.
        /// For a hub this means that Connected will be true when the first driver connects and will only be set to false
        /// when all drivers have disconnected.  A second driver may find that Connected is already true and
        /// setting Connected to false does not report Connected as false.  This is not an error because the physical state is that the
        /// hardware connection is still true.</para>
        /// <para>Multiple calls setting Connected to true or false will not cause an error.</para>
        /// <para><legacyBold>ICameraV4 Behaviour Clarification</legacyBold> - <see cref="ITelescopeV4"/> and later clients should use the asynchronous <see cref="Connect"/> / <see cref="Disconnect"/> mechanic 
        /// rather than setting Connected <see langword="true"/> when communicating with <see cref="ITelescopeV4"/> or later devices.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Clients should use the Connect() / Disconnect() mechanic rather than setting Connected TRUE when accessing ITelescopeV4 or later devices.</revision>
        /// </revisionHistory>
        bool Connected { get; set; }

        /// <summary>
        /// The declination (degrees) of the telescope's current equatorial coordinates, in the coordinate system given by the <see cref="EquatorialSystem" /> property.
        /// Reading the property will raise an error if the value is unavailable.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double Declination { get; }

        /// <summary>
        /// The declination tracking rate (arcseconds per SI second, default = 0.0)
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If DeclinationRate Write is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid DeclinationRate is specified</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red;margin-bottom:0"><b>DeclinationRate Read must be implemented and must not throw a PropertyNotImplementedException. </b></p>
        /// <p style="color:red;margin-top:0"><b>DeclinationRate Write can throw a PropertyNotImplementedException.</b></p>
        /// This property, together with <see cref="RightAscensionRate" />, provides support for "offset tracking".
        /// Offset tracking is used primarily for tracking objects that move relatively slowly against the equatorial coordinate system.
        /// It also may be used by a software guiding system that controls rates instead of using the <see cref="PulseGuide">PulseGuide</see> method.
        /// <para>
        /// <b>NOTES:</b>
        /// <list type="bullet">
        /// <list></list>
        /// <item><description>The property value represents an offset from zero motion.</description></item>
        /// <item><description>If <see cref="CanSetDeclinationRate" /> is False, this property will always return 0.</description></item>
        /// <item><description>To discover whether this feature is supported, test the <see cref="CanSetDeclinationRate" /> property.</description></item>
        /// <item><description>The supported range of this property is telescope specific, however, if this feature is supported,
        /// it can be expected that the range is sufficient to allow correction of guiding errors caused by moderate misalignment
        /// and periodic error.</description></item>
        /// <item><description>If this property is non-zero when an equatorial slew is initiated, the telescope should continue to update the slew
        /// destination coordinates at the given offset rate.</description></item>
        /// <item><description>This will allow precise slews to a fast-moving target with a slow-slewing telescope.</description></item>
        /// <item><description>When the slew completes, the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> properties should reflect the final (adjusted) destination.</description></item>
        /// <item><description>The units of this property are arcseconds per SI (atomic) second. Please note that for historic reasons the units of the <see cref="RightAscensionRate" /> property are seconds of RA per sidereal second.</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// <legacyBold>ITelescopeV4 clarification</legacyBold> - DeclinationRate is an offset from sidereal tracking rate and must only be applied when tracking at sidereal rate. It must not be applied when 
        /// tracking at solar, lunar, King or other non-sidereal rates.
        /// </para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">This only applies when tracking at sidereal rate, see note above.</revision>
        /// </revisionHistory>
        double DeclinationRate { get; set; }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p> 
        /// <para>The description length must be a maximum of 64 characters so that it can be used in FITS image headers, which are limited to 80 characters including the header name.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        string Description { get; }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p> This string may contain line endings and may be hundreds to thousands of characters long.
        /// It is intended to display detailed information on the ASCOM driver, including version and copyright data.
        /// See the <see cref="Description" /> property for information on the device itself.
        /// To get the driver version in a parseable string, use the <see cref="DriverVersion" /> property.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        string DriverInfo { get; }

        /// <summary>
        /// Locates the telescope's "home" position
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanFindHome" /> is False</exception>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <para><legacyBold>ITelescopeV4 clarification</legacyBold> - This <see langword="method"/> must act asynchronously using <see cref="Slewing"/> as the completion variable.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Formally defined as operating asynchronously, see note above.</revision>
        /// </revisionHistory>
        void FindHome();

        /// <summary>
        /// The telescope's focal length, meters
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// This property may be used by clients to calculate telescope field of view and plate scale when combined with detector pixel size and geometry.
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double FocalLength { get; }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        string Name { get; }

        /// <summary>
        /// Move the telescope to its park position, stop all motion (or restrict to a small safe range), and set <see cref="AtPark" /> to True.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanPark" /> is False</exception>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <para>
        /// This is an asynchronous method: Use the <see cref="Slewing"/> property to monitor the operation's progress. 
        /// </para>
        /// <para><legacyBold>ITelescopeV4 clarification</legacyBold> - This <see langword="method"/> must act asynchronously using <see cref="Slewing"/> as the completion variable.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Formally defined as operating asynchronously, see note above.</revision>
        /// </revisionHistory>
        void Park();

        /// <summary>
        /// Moves the scope in the given direction for the given interval or time at 
        /// the rate given by the corresponding guide rate property 
        /// </summary>
        /// <param name="Direction">The direction in which the guide-rate motion is to be made</param>
        /// <param name="Duration">The duration of the guide-rate motion (milliseconds)</param>
        /// <exception cref="PropertyNotImplementedException">If the method is not implemented and <see cref="CanPulseGuide" /> is False</exception>
        /// <exception cref="InvalidValueException">If an invalid direction or duration is given.</exception>
        /// <exception cref="InvalidOperationException">If the pulse guide cannot be effected e.g. if the telescope is slewing or is not tracking or a pulse guide is already in progress and a second cannot be started asynchronously.</exception>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <para>
        /// <para><legacyBold>ITelescopeV4 clarification</legacyBold> - This <see langword="method"/> must act asynchronously using <see cref="IsPulseGuiding"/> as the completion variable.</para>
        /// </para>
        /// <para>
        /// If the device cannot have simultaneous PulseGuide operations in both RightAscension and Declination, it must throw InvalidOperationException when the overlapping operation is attempted.
        /// </para>
        /// <para>
        /// <para>Further explanation is available in this link: <a href="https://ascom-standards.org/newdocs/telescope.html#telescope-pulseguide" target="_blank">Master Interface Document</a>.</para>
        /// </para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Formally defined as operating asynchronously, see note above.</revision>
        /// </revisionHistory>
        void PulseGuide(GuideDirections Direction, int Duration);

        /// <summary>
        /// The right ascension (hours) of the telescope's current equatorial coordinates,
        /// in the coordinate system given by the EquatorialSystem property
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// Reading the property will raise an error if the value is unavailable.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double RightAscension { get; }

        /// <summary>
        /// The right ascension tracking rate offset from sidereal (seconds per sidereal second, default = 0.0)
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If RightAscensionRate Write is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid rate is set.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red;margin-bottom:0"><b>RightAscensionRate Read must be implemented and must not throw a PropertyNotImplementedException. </b></p>
        /// <p style="color:red;margin-top:0"><b>RightAscensionRate Write can throw a PropertyNotImplementedException.</b></p>
        /// This property, together with <see cref="DeclinationRate" />, provides support for "offset tracking". Offset tracking is used primarily for tracking objects that move relatively slowly
        /// against the equatorial coordinate system. It also may be used by a software guiding system that controls rates instead of using the <see cref="PulseGuide">PulseGuide</see> method.
        /// <para>
        /// <b>NOTES:</b>
        /// The property value represents an offset from the currently selected <see cref="TrackingRate" />.
        /// <list type="bullet">
        /// <item><description>If this property is zero, tracking will be at the selected <see cref="TrackingRate" />.</description></item>
        /// <item><description>If <see cref="CanSetRightAscensionRate" /> is False, this property must always return 0.</description></item>
        /// To discover whether this feature is supported, test the <see cref="CanSetRightAscensionRate" />property.
        /// <item><description>The units of this property are seconds of right ascension per sidereal second. Please note that for historic reasons the units of the <see cref="DeclinationRate" /> property are arcseconds per SI second.</description></item>
        /// <item><description>To convert a given rate in (the more common) units of sidereal seconds per UTC (clock) second, multiply the value by 0.9972695677
        /// (the number of UTC seconds in a sidereal second) then set the property. Please note that these units were chosen for the Telescope V1 standard,
        /// and in retrospect, this was an unfortunate choice. However, to maintain backwards compatibility, the units cannot be changed.
        /// A simple multiplication is all that's needed, as noted. The supported range of this property is telescope specific, however,
        /// if this feature is supported, it can be expected that the range is sufficient to allow correction of guiding errors
        /// caused by moderate misalignment and periodic error. </description></item>
        /// <item><description>If this property is non-zero when an equatorial slew is initiated, the telescope should continue to update the slew destination coordinates
        /// at the given offset rate. This will allow precise slews to a fast-moving target with a slow-slewing telescope. When the slew completes,
        /// the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> properties should reflect the final (adjusted) destination. This is not a required
        /// feature of this specification, however it is desirable. </description></item>
        /// <item><description>Use the <see cref="Tracking" /> property to enable and disable sidereal tracking (if supported). </description></item>
        /// </list>
        /// </para>
        /// <para>
        /// <legacyBold>ITelescopeV4 clarification</legacyBold> - DeclinationRate is an offset from sidereal tracking rate and must only be applied when tracking at sidereal rate. It must not be applied when 
        /// tracking at solar, lunar, King or other non-sidereal rates.
        /// </para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">This only applies when tracking at sidereal rate, see note above.</revision>
        /// </revisionHistory>
        double RightAscensionRate { get; set; }

        /// <summary>
        /// Sets the telescope's park position to be its current position.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanPark" /> is False</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void SetPark();

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void SetupDialog();

        /// <summary>
        /// The local apparent sidereal time from the telescope's internal clock (hours, sidereal)
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// It is required for a driver to calculate this from the system clock if the telescope
        /// has no accessible source of sidereal time. Local Apparent Sidereal Time is the sidereal
        /// time used for pointing telescopes, and thus must be calculated from the Greenwich Mean
        /// Sidereal time, longitude, nutation in longitude and true ecliptic obliquity.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double SiderealTime { get; }

        /// <summary>
        /// The elevation above mean sea level (meters) of the site at which the telescope is located
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid elevation is set.</exception>
        /// <exception cref="InvalidOperationException">If the application must set the elevation before reading it, but has not.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// Setting this property will raise an error if the given value is outside the range -300 through +10000 metres.
        /// Reading the property will raise an error if the value has never been set or is otherwise unavailable.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double SiteElevation { get; set; }

        /// <summary>
        /// The geodetic(map) latitude (degrees, positive North, WGS84) of the site at which the telescope is located.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid latitude is set.</exception>
        /// <exception cref="InvalidOperationException">If the application must set the latitude before reading it, but has not.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// Setting this property will raise an error if the given value is outside the range -90 to +90 degrees.
        /// Reading the property will raise an error if the value has never been set or is otherwise unavailable.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double SiteLatitude { get; set; }

        /// <summary>
        /// The longitude (degrees, positive East, WGS84) of the site at which the telescope is located.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid longitude is set.</exception>
        /// <exception cref="InvalidOperationException">If the application must set the longitude before reading it, but has not.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// Setting this property will raise an error if the given value is outside the range -180 to +180 degrees.
        /// Reading the property will raise an error if the value has never been set or is otherwise unavailable.
        /// Note that West is negative!
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double SiteLongitude { get; set; }

        /// <summary>
        /// True if telescope is in the process of moving in response to one of the
        /// Slew methods or the <see cref="MoveAxis" /> method, False at all other times.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// Reading the property will raise an error if the value is unavailable. If the telescope is not capable of asynchronous slewing, this property will always be False.
        /// The definition of "slewing" excludes motion caused by sidereal tracking, <see cref="PulseGuide">PulseGuide</see>, <see cref="RightAscensionRate" />, and <see cref="DeclinationRate" />.
        /// It reflects only motion caused by one of the Slew commands, flipping caused by changing the <see cref="SideOfPier" /> property, or <see cref="MoveAxis" />.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool Slewing { get; }

        /// <summary>
        /// Specifies a post-slew settling time (sec.).
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid settle time is set.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// Adds additional time to slew operations. Slewing methods will not return,
        /// and the <see cref="Slewing" /> property will not become False, until the slew completes and the SlewSettleTime has elapsed.
        /// This feature (if supported) may be used with mounts that require extra settling time after a slew.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        short SlewSettleTime { get; set; }

        /// <summary>
        /// Move the telescope to the given equatorial coordinates, return when slew is complete
        /// </summary>
        /// <exception cref="InvalidValueException">If an invalid right ascension or declination is given.</exception>
        /// <param name="RightAscension">The destination right ascension (hours). Copied to <see cref="TargetRightAscension" />.</param>
        /// <param name="Declination">The destination declination (degrees, positive North). Copied to <see cref="TargetDeclination" />.</param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSlew" /> is False</exception>
        /// <exception cref="ParkedException">If the telescope is parked</exception>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <p style="color:red"><b>Deprecated for client applications.</b></p>
        /// <para>
        /// <legacyBold>ITelescopeV4 clarification</legacyBold> - Synchronous methods are deprecated in ITelescopeV4 onward. Clients and Alpaca devices should not use / implement them. However, COM Driver authors must 
        /// implement synchronous methods if the mount can slew, to ensure backward compatibility. 
        /// </para>
        /// <para>
        /// See <conceptualLink target="c37349a5-a535-47d6-8c30-11c620932213"/> for more information.
        /// </para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Synchronous slewing is deprecated, see note above.</revision>
        /// </revisionHistory>
        void SlewToCoordinates(double RightAscension, double Declination);

        /// <summary>
        /// Move the telescope to the given equatorial coordinates, return immediately after starting the slew.
        /// </summary>
        /// <param name="RightAscension">The destination right ascension (hours). Copied to <see cref="TargetRightAscension" />.</param>
        /// <param name="Declination">The destination declination (degrees, positive North). Copied to <see cref="TargetDeclination" />.</param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSlewAsync" /> is False</exception>
        /// <exception cref="ParkedException">If the telescope is parked</exception>
        /// <exception cref="InvalidValueException">If an invalid right ascension or declination is given.</exception>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <para>
        /// Further explanation is available in this link: <a href="https://ascom-standards.org/newdocs/telescope.html#telescope-canslewasync" target="_blank">Master Interface Document</a>.
        /// </para>
        /// <legacyBold>ITelescoipeV4 clarification</legacyBold> - If the mount can slew, driver authors must implement both synchronous and asynchronous slewing. Alpaca devices must only implement asynchronous slewing.
        /// See <conceptualLink target="c37349a5-a535-47d6-8c30-11c620932213"/> for more information.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Synchronous slewing is deprecated, see note above.</revision>
        /// </revisionHistory>
        void SlewToCoordinatesAsync(double RightAscension, double Declination);

        /// <summary>
        /// Move the telescope to the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> coordinates, return when slew complete.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSlew" /> is False</exception>
        /// <exception cref="ParkedException">If the telescope is parked</exception>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <p style="color:red"><b>Deprecated for client applications.</b></p>
        /// <para>
        /// <legacyBold>ITelescopeV4 clarification</legacyBold> - Synchronous methods are deprecated in ITelescopeV4 onward. Clients and Alpaca devices should not use / implement them. However, COM Driver authors must 
        /// implement synchronous methods if the mount can slew, to ensure backward compatibility. 
        /// </para>
        /// <para>
        /// See <conceptualLink target="c37349a5-a535-47d6-8c30-11c620932213"/> for more information.
        /// </para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Synchronous slewing is deprecated, see note above.</revision>
        /// </revisionHistory>
        void SlewToTarget();

        /// <summary>
        /// Move the telescope to the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" />  coordinates,
        /// returns immediately after starting the slew.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSlewAsync" /> is False</exception>
        /// <exception cref="ParkedException">If the telescope is parked</exception>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <para>
        /// Further explanation is available in this link: <a href="https://ascom-standards.org/newdocs/telescope.html#telescope-canslewasync" target="_blank">Master Interface Document</a>.
        /// </para>
        /// <legacyBold>ITelescoipeV4 clarification</legacyBold> - If the mount can slew, driver authors must implement both synchronous and asynchronous slewing. Alpaca devices must only implement asynchronous slewing.
        /// See <conceptualLink target="c37349a5-a535-47d6-8c30-11c620932213"/> for more information.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Synchronous slewing is deprecated, see note above.</revision>
        /// </revisionHistory>
        void SlewToTargetAsync();

        /// <summary>
        /// Matches the scope's equatorial coordinates to the given equatorial coordinates.
        /// </summary>
        /// <param name="RightAscension">The corrected right ascension (hours). Copied to the <see cref="TargetRightAscension" /> property.</param>
        /// <param name="Declination">The corrected declination (degrees, positive North). Copied to the <see cref="TargetDeclination" /> property.</param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSync" /> is False</exception>
        /// <exception cref="ParkedException">If the telescope is parked</exception>
        /// <exception cref="InvalidValueException">If an invalid right ascension or declination is given.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// This must be implemented if the <see cref="CanSync" /> property is True. Raises an error if matching fails.
        /// Raises an error if <see cref="AtPark" /> AtPark is True, or if <see cref="Tracking" /> is False.
        /// The way that Sync is implemented is mount dependent and it should only be relied on to improve pointing for positions close to
        /// the position at which the sync is done.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void SyncToCoordinates(double RightAscension, double Declination);

        /// <summary>
        /// Matches the scope's equatorial coordinates to the target equatorial coordinates.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSync" /> is False</exception>
        /// <exception cref="ParkedException">If the telescope is parked</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// This must be implemented if the <see cref="CanSync" /> property is True. Raises an error if matching fails.
        /// Raises an error if <see cref="AtPark" /> AtPark is True, or if <see cref="Tracking" /> is False.
        /// The way that Sync is implemented is mount dependent and it should only be relied on to improve pointing for positions close to
        /// the position at which the sync is done.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void SyncToTarget();

        /// <summary>
        /// The declination (degrees, positive North) for the target of an equatorial slew or sync operation
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid declination is set.</exception>
        /// <exception cref="InvalidOperationException">If the property is read before being set for the first time.</exception>
        /// <exception cref="ParkedException">If the telescope is parked</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// Setting this property will raise an error if the given value is outside the range -90 to +90 degrees. Reading the property will raise an error if the value has never been set or is otherwise unavailable.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double TargetDeclination { get; set; }

        /// <summary>
        /// The right ascension (hours) for the target of an equatorial slew or sync operation
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid right ascension is set.</exception>
        /// <exception cref="InvalidOperationException">If the property is read before being set for the first time.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// Setting this property will raise an error if the given value is outside the range 0 to 24 hours. Reading the property will raise an error if the value has never been set or is otherwise unavailable.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double TargetRightAscension { get; set; }

        /// <summary>
        /// The state of the telescope's sidereal tracking drive.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If Tracking Write is not implemented.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="ParkedException">When <see cref="Tracking"/> is set True and the telescope is parked (<see cref="AtPark"/> is True). Added in ITelescopeV4</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red;margin-bottom:0"><b>Tracking Read must be implemented and must not throw a PropertyNotImplementedException. </b></p>
        /// <p style="color:red;margin-top:0"><b>Tracking Write can throw a PropertyNotImplementedException.</b></p>
        /// Changing the value of this property will turn the sidereal drive on and off.
        /// However, some telescopes may not support changing the value of this property
        /// and thus may not support turning tracking on and off.
        /// See the <see cref="CanSetTracking" /> property.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool Tracking { get; set; }

        /// <summary>
        /// Takes telescope out of the Parked state.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanUnpark" /> is False</exception>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <para>
        /// This is an asynchronous method and <see cref="Slewing"/> must be set True while the mount is unparking and False when the operation is complete. 
        /// <see cref="AtPark"/> and <see cref="Slewing"/> will be set False when the mount has unparked successfully.
        /// </para>
        /// <para><legacyBold>ITelescopeV4 clarification</legacyBold> - This <see langword="method"/> must act asynchronously using <see cref="Slewing"/> as the completion variable.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Formally defined as operating asynchronously, see note above.</revision>
        /// </revisionHistory>
        void Unpark();

        /// <summary>
        /// The UTC date/time of the telescope's internal clock
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If UTCDate Write is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid <see cref="DateTime" /> is set.</exception>
        /// <exception cref="InvalidOperationException">When UTCDate is read and the mount cannot provide this property itself and a value has not yet be established by writing to the property.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red;margin-bottom:0"><b>UTCDate Read must be implemented and must not throw a PropertyNotImplementedException. </b></p>
        /// <p style="color:red;margin-top:0"><b>UTCDate Write can throw a PropertyNotImplementedException.</b></p>
        /// The driver must calculate this from the system clock if the telescope has no accessible source of UTC time. In this case, the property must not be writeable (this would change the system clock!) and will instead raise an error.
        /// However, it is permitted to change the telescope's internal UTC clock if it is being used for this property. This allows clients to adjust the telescope's UTC clock as needed for accuracy. Reading the property
        /// will raise an error if the value has never been set or is otherwise unavailable.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        DateTime UTCDate { get; set; }

        #endregion

        #region ITelescopeV2 members

        /// <summary>
        /// The area of the telescope's aperture, taking into account any obstructions (square meters)
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// This is only available for telescope InterfaceVersions 2 and later.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        double ApertureArea { get; }

        /// <summary>
        /// True if the telescope is stopped in the Home position. Set only following a <see cref="FindHome"></see> operation,
        /// and reset with any slew operation. This property must be False if the telescope does not support homing. 
        /// </summary>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a NotImplementedException.</b></p>
        /// <para>Further explanation is available in this link: <a href="https://ascom-standards.org/newdocs/telescope.html#telescope-athome" target="_blank">Master Interface Document</a>.</para>
        /// <para>This is only available for telescope Interface Versions 2 and later.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool AtHome { get; }

        /// <summary>
        /// True if the telescope has been put into the parked state by the <see cref="Park" /> method. Set False by calling the Unpark() method.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// <para>AtPark is True when the telescope is in the parked state. This is achieved by calling the <see cref="Park" /> method. When AtPark is true,
        /// the telescope movement is stopped (or restricted to a small safe range of movement) and all calls that would cause telescope
        /// movement (e.g. slewing, changing Tracking state) must not do so, and must raise an error.</para>
        /// <para>The telescope is taken out of parked state by calling the <see cref="Unpark" /> method. If the telescope cannot be parked,
        /// then AtPark must always return False.</para>
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool AtPark { get; }

        /// <summary>
        /// Determine the rates at which the telescope may be moved about the specified axis by the <see cref="MoveAxis" /> method.
        /// </summary>
        /// <param name="Axis">The axis about which rate information is desired (TelescopeAxes value)</param>
        /// <returns>Collection of <see cref="IRate" /> rate objects</returns>
        /// <exception cref="InvalidValueException">If an invalid Axis is specified.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a MethodNotImplementedException.</b></p>
        /// See the description of <see cref="MoveAxis" /> for more information. This method must return an empty collection if <see cref="MoveAxis" /> is not supported.
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// <para>
        /// Please note that the rate objects must contain absolute non-negative values only. Applications determine the direction by applying a
        /// positive or negative sign to the rates provided. This obviates the need for the driver to present a duplicate set of negative rates
        /// as well as the positive rates.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        IAxisRates AxisRates(TelescopeAxes Axis);

        /// <summary>
        /// True if this telescope can move the requested axis
        /// </summary>
        /// <param name="Axis">Primary, Secondary or Tertiary axis</param>
        /// <returns>Boolean indicating can or can not move the requested axis</returns>
        /// <exception cref="InvalidValueException">If an invalid Axis is specified.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a MethodNotImplementedException.</b></p>
        /// This is only available for telescope InterfaceVersions 2 and later.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool CanMoveAxis(TelescopeAxes Axis);

        /// <summary>
        /// True if the <see cref="DeclinationRate" /> property can be changed to provide offset tracking in the declination axis.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// May raise an error if the telescope is not connected.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetDeclinationRate { get; }

        /// <summary>
        /// True if the guide rate properties used for <see cref="PulseGuide" /> can be adjusted.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// May raise an error if the telescope is not connected.
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetGuideRates { get; }

        /// <summary>
        /// True if the <see cref="SideOfPier" /> property can be set, meaning that the mount can be forced to flip.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// This will always return False for mounts that do not have to be flipped.
        /// May raise an error if the telescope is not connected.
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetPierSide { get; }

        /// <summary>
        /// True if the <see cref="RightAscensionRate" /> property can be changed to provide offset tracking in the right ascension axis.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// May raise an error if the telescope is not connected.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetRightAscensionRate { get; }

        /// <summary>
        /// True if this telescope is capable of programmed slewing (synchronous or asynchronous) to local horizontal coordinates
        /// </summary>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a NotImplementedException.</b></p>
        /// <para>May raise an error if the telescope is not connected.</para>
        /// <para>
        /// <legacyBold>ITelescopeV4 clarification</legacyBold> - Synchronous methods are deprecated in ITelescopeV4 onward. Clients and Alpaca devices should not use / implement them. However, COM Driver authors must 
        /// implement synchronous methods if the mount can slew, to ensure backward compatibility. 
        /// </para>
        /// <para>
        /// See <conceptualLink target="c37349a5-a535-47d6-8c30-11c620932213"/> for more information.
        /// </para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Synchronous slewing is deprecated, see note above.</revision>
        /// </revisionHistory>
        bool CanSlewAltAz { get; }

        /// <summary>
        /// True if this telescope is capable of programmed asynchronous slewing to local horizontal coordinates
        /// </summary>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a NotImplementedException.</b></p>
        /// <para>May raise an error if the telescope is not connected.</para>
        /// <legacyBold>ITelescoipeV4 clarification</legacyBold> - If the mount can slew, driver authors must implement both synchronous and asynchronous slewing. Alpaca devices must only implement asynchronous slewing.
        /// See <conceptualLink target="c37349a5-a535-47d6-8c30-11c620932213"/> for more information.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Synchronous slewing is deprecated, see note above.</revision>
        /// </revisionHistory>
        bool CanSlewAltAzAsync { get; }

        /// <summary>
        /// True if this telescope is capable of programmed syncing to local horizontal coordinates
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// May raise an error if the telescope is not connected.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSyncAltAz { get; }

        /// <summary>
        /// Predict side of pier for German equatorial mounts
        /// </summary>
        /// <param name="RightAscension">The destination right ascension (hours).</param>
        /// <param name="Declination">The destination declination (degrees, positive North).</param>
        /// <returns>The pointing state that a German equatorial telescope would have if a slew to the given equatorial coordinates is performed at the current instant of time.
        /// If the driver implements the ASCOM convention described in <see cref="SideOfPier"/>, it will also indicate the physical side of the pier on which the telescope will be.</returns>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="InvalidValueException">If an invalid RightAscension or Declination is specified.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <para>This is only available for telescope interface version 2 and later.</para>
        /// <para>Please see <see cref="SideOfPier"/> for more information on pointing state and physical side of pier for German equatorial mounts.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        PierSide DestinationSideOfPier(double RightAscension, double Declination);

        /// <summary>
        /// True if the telescope or driver applies atmospheric refraction to coordinates.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">Either read or write or both properties can throw PropertyNotImplementedException if not implemented</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// If this property is True, the coordinates sent to, and retrieved from, the telescope are unrefracted.
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// <para>
        /// <b>NOTES:</b>
        /// <list type="bullet">
        /// <item><description>If the driver does not know whether the attached telescope does its own refraction, and if the driver does not itself calculate
        /// refraction, this property (if implemented) must raise an error when read.</description></item>
        /// <item><description>Writing to this property is optional. Often, a telescope (or its driver) calculates refraction using standard atmospheric parameters.</description></item>
        /// <item><description>If the client wishes to calculate a more accurate refraction, then this property could be set to False and these
        /// client-refracted coordinates used.</description></item>
        /// <item><description>If disabling the telescope or driver's refraction is not supported, the driver must raise an error when an attempt to set
        /// this property to False is made.</description></item>
        /// <item><description>Setting this property to True for a telescope or driver that does refraction, or to False for a telescope or driver that
        /// does not do refraction, shall not raise an error. It shall have no effect.</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool DoesRefraction { get; set; }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> This must be in the form "n.n".
        /// It should not to be confused with the <see cref="InterfaceVersion" /> property, which is the version of this specification supported by the
        /// driver.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        string DriverVersion { get; }

        /// <summary>
        /// The interface version number that this device supports. Should return 4 for this interface version.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> Clients can detect legacy V1 drivers by trying to read this property.
        /// If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1.
        /// In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        short InterfaceVersion { get; }

        /// <summary>
        /// Equatorial coordinate system used by this telescope (e.g. Topocentric or J2000).
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p>
        /// Most amateur telescopes use topocentric coordinates. This coordinate system is simply the apparent position in the sky
        /// (possibly uncorrected for atmospheric refraction) for "here and now", thus these are the coordinates that one would use with digital setting
        /// circles and most amateur scopes. More sophisticated telescopes use one of the standard reference systems established by professional astronomers.
        /// The most common is the Julian Epoch 2000 (J2000). These instruments apply corrections for precession,nutation, aberration, etc. to adjust the coordinates
        /// from the standard system to the pointing direction for the time and location of "here and now".
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        EquatorialCoordinateType EquatorialSystem { get; }

        /// <summary>
        /// The current Declination movement rate offset for telescope guiding (degrees/sec)
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
        /// <exception cref="InvalidValueException">If an invalid guide rate is set.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// This is the rate for both hardware/relay guiding and the PulseGuide() method.
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// <para>
        /// <b>NOTES:</b>
        /// <list type="bullet">
        /// <item><description>To discover whether this feature is supported, test the <see cref="CanSetGuideRates" /> property.</description></item>
        /// <item><description>The supported range of this property is telescope specific, however, if this feature is supported, it can be expected that the range is sufficient to
        /// allow correction of guiding errors caused by moderate misalignment and periodic error.</description></item>
        /// <item><description>If a telescope does not support separate guiding rates in Right Ascension and Declination, then it is permissible for <see cref="GuideRateRightAscension" /> and GuideRateDeclination to be tied together.
        /// In this case, changing one of the two properties will cause a change in the other.</description></item>
        /// <item><description>Mounts must start up with a known or default declination guide rate, and this property must return that known/default guide rate until changed.</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        double GuideRateDeclination { get; set; }

        /// <summary>
        /// The current Right Ascension movement rate offset for telescope guiding (degrees/sec)
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented</exception>
        /// <exception cref="InvalidValueException">If an invalid guide rate is set.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// This is the rate for both hardware/relay guiding and the PulseGuide() method.
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// <para>
        /// <b>NOTES:</b>
        /// <list type="bullet">
        /// <item><description>To discover whether this feature is supported, test the <see cref="CanSetGuideRates" /> property.</description></item>
        /// <item><description>The supported range of this property is telescope specific, however, if this feature is supported, it can be expected that the range is sufficient to allow correction of guiding errors caused by moderate
        /// misalignment and periodic error.</description></item>
        /// <item><description>If a telescope does not support separate guiding rates in Right Ascension and Declination, then it is permissible for GuideRateRightAscension and <see cref="GuideRateDeclination" /> to be tied together.
        /// In this case, changing one of the two properties will cause a change in the other.</description></item>
        /// <item><description> Mounts must start up with a known or default right ascension guide rate, and this property must return that known/default guide rate until changed.</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        double GuideRateRightAscension { get; set; }

        /// <summary>
        /// True if a <see cref="PulseGuide" /> command is in progress, False otherwise
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If <see cref="CanPulseGuide" /> is False</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// Raises an error if the value of the <see cref="CanPulseGuide" /> property is false (the driver does not support the <see cref="PulseGuide" /> method).
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool IsPulseGuiding { get; }

        /// <summary>
        /// Move the telescope in one axis at the given rate.
        /// </summary>
        /// <param name="Axis">The physical axis about which movement is desired</param>
        /// <param name="Rate">The rate of motion (deg/sec) about the specified axis</param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid axis or rate is given.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// This method supports control of the mount about its mechanical axes.
        /// The telescope will start moving at the specified rate about the specified axis and continue indefinitely.
        /// This method can be called for each axis separately, and have them all operate concurrently at separate rates of motion.
        /// Set the rate for an axis to zero to restore the motion about that axis to the rate set by the <see cref="Tracking"/> property.
        /// Tracking motion (if enabled, see note below) is suspended during this mode of operation.
        /// <para>
        /// Raises an error if <see cref="AtPark" /> is true.
        /// This must be implemented for the if the <see cref="CanMoveAxis" /> property returns True for the given axis.</para>
        /// <para>This is only available for telescope Interface version 2 and later.
        /// </para>
        /// <para>
        /// <see cref="MoveAxis" /> is best seen as an override to however the mount is configured for Tracking, including its enabled/disabled state and any 
        /// current RightAscensionRate and DeclinationRate offsets. 
        /// </para>
        /// <para>
        /// While MoveAxis is in effect, TrackingRate, RightAscensionRate and DeclinationRate should retain their current values and will become 
        /// effective again when MoveAxis is set to zero for the relevant axis.
        /// </para>
        /// <para>
        /// A <see cref="MoveAxis" /> call with Rate = 0 is required to stop motion and return to the previous tracking state of that axis.
        /// </para>
        /// <b>NOTES:</b>
        /// <list type="bullet">
        /// <item><description>The Slewing property must remain <see langword="true"/> whenever <legacyBold>any</legacyBold> axis has a non-zero MoveAxis rate. E.g. Suppose
        /// MoveAxis is used to make both the RA and declination axes move at valid axis rates. If the declination axis rate is then set to zero, Slewing must remain
        /// <see langword="true"/> because the RA axis is still moving at a non-zero axis rate.</description></item>
        /// <item><description>The movement rate must be within the value(s) obtained from a <see cref="IRate" /> object in the
        /// the <see cref="AxisRates" /> collection. This is a signed value with negative rates moving in the opposite direction to positive rates.</description></item>
        /// <item><description>The values specified in <see cref="AxisRates" /> are absolute, unsigned values and apply to both directions, determined by the sign used in this command.</description></item>
        /// <item><description>MoveAxis can be used to simulate a hand-box by initiating motion with the MouseDown event and stopping the motion with the MouseUp event.</description></item>
        /// <item><description>It may be possible to implement satellite tracking by using the <see cref="MoveAxis" /> method to move the scope in the required manner to track a satellite.</description></item>
        /// </list>
        /// <para><legacyBold>ITelescopeV4 clarification</legacyBold> - This <see langword="method"/> must act asynchronously using <see cref="Slewing"/> as the completion variable. This means that 
        /// <see cref="Slewing"/> must remain <see langword="true"/> until the movement rate is cancelled by setting a MoveAxis rate of 0.0 or an error condition occurs.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Formally defined as operating asynchronously, see note above.</revision>
        /// </revisionHistory>
        void MoveAxis(TelescopeAxes Axis, double Rate);

        /// <summary>
        /// Indicates the pointing state of the mount.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid side of pier is set.</exception>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <para>SideOfPier SET is an asynchronous method and <see cref="Slewing"/> should be set True while the operation is in progress.</para>
        /// <para>Please note that "SideofPier" is a misnomer and that this method actually refers to the mount's pointing state. For German Equatorial mounts there is a complex
        /// relationship between pointing state and the physical side of the pier on which the mount resides.</para>
        /// <para>
        /// For example, suppose the mount is tracking on the east side of the pier, counterweights down, 
        /// observing a target on the celestial equator at hour angle +3.0.Now suppose that the observer
        /// wishes to observe a new target at hour angle -9.0. All the mount needs to do is to rotate the declination axis, 
        /// through the celestial pole where the hour angle will change from +3.0 to -9.0, and keep going until it gets
        /// to the required declination at hour angle -9.0. Other than tracking, the right ascension  axis has not moved.
        /// </para>
        /// <para>
        /// In this example the mount is still physically on the east side of the pier but the pointing state
        /// will have changed when the declination axis moved through the celestial pole.
        /// </para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        PierSide SideOfPier { get; set; }

        /// <summary>
        /// Move the telescope to the given local horizontal coordinates, return when slew is complete
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSlewAltAz" /> is False</exception>
        /// <exception cref="InvalidValueException">If an invalid azimuth or elevation is given.</exception>
        /// <exception cref="ParkedException">If the telescope is parked</exception>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <param name="Azimuth">Target azimuth (degrees, North-referenced, positive East/clockwise).</param>
        /// <param name="Altitude">Target altitude (degrees, positive up)</param>
        /// <remarks>
        /// <p style="color:red"><b>Deprecated for client applications.</b></p>
        /// <para>This method must not be used by applications, use the asynchronous <see cref="SlewToAltAzAsync(double, double)"/> method instead.</para>
        /// <para>
        /// <legacyBold>ITelescopeV4 clarification</legacyBold> - Synchronous methods are deprecated in ITelescopeV4 onward. Clients and Alpaca devices should not use / implement them. However, COM Driver authors must 
        /// implement synchronous methods if the mount can slew, to ensure backward compatibility. 
        /// </para>
        /// <para>
        /// See <conceptualLink target="c37349a5-a535-47d6-8c30-11c620932213"/> for more information.
        /// </para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Synchronous slewing is deprecated, see note above.</revision>
        /// </revisionHistory>
        void SlewToAltAz(double Azimuth, double Altitude);

        /// <summary>
        /// This Method must be implemented if <see cref="CanSlewAltAzAsync" /> returns True.
        /// </summary>
        /// <param name="Azimuth">Azimuth to which to move</param>
        /// <param name="Altitude">Altitude to which to move to</param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSlewAltAzAsync" /> is False</exception>
        /// <exception cref="InvalidValueException">If an invalid azimuth or elevation is given.</exception>
        /// <exception cref="ParkedException">If the telescope is parked</exception>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <para>
        /// Further explanation is available in this link: <a href="https://ascom-standards.org/newdocs/telescope.html#telescope-canslewasync" target="_blank">Master Interface Document</a>.
        /// </para>
        /// <legacyBold>ITelescoipeV4 clarification</legacyBold> - If the mount can slew, driver authors must implement both synchronous and asynchronous slewing. Alpaca devices must only implement asynchronous slewing.
        /// See <conceptualLink target="c37349a5-a535-47d6-8c30-11c620932213"/> for more information.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Synchronous slewing is deprecated, see note above.</revision>
        /// </revisionHistory>
        void SlewToAltAzAsync(double Azimuth, double Altitude);

        /// <summary>
        /// Matches the scope's local horizontal coordinates to the given local horizontal coordinates.
        /// </summary>
        /// <param name="Azimuth">Target azimuth (degrees, North-referenced, positive East/clockwise)</param>
        /// <param name="Altitude">Target altitude (degrees, positive up)</param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented and <see cref="CanSyncAltAz" /> is False</exception>
        /// <exception cref="InvalidValueException">If an invalid azimuth or altitude is given.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <para>This must be implemented if the <see cref="CanSyncAltAz" /> property is True. Raises an error if matching fails.</para>
        /// <para>Raises an error if <see cref="AtPark" /> is True, or if <see cref="Tracking" /> is True.</para>
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        void SyncToAltAz(double Azimuth, double Altitude);

        /// <summary>
        /// The current tracking rate of the telescope's sidereal drive
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If TrackingRate Write is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid drive rate is set.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red;margin-bottom:0"><b>TrackingRate Read must be implemented and must not throw a PropertyNotImplementedException. </b></p>
        /// <p style="color:red;margin-top:0"><b>TrackingRate Write can throw a PropertyNotImplementedException.</b></p>
        /// Supported rates (one of the <see cref="DriveRates" />  values) are contained within the <see cref="TrackingRates" /> collection.
        /// Values assigned to TrackingRate must be one of these supported rates. If an unsupported value is assigned to this property, it will raise an error.
        /// The currently selected tracking rate can be further adjusted via the <see cref="RightAscensionRate" /> and <see cref="DeclinationRate" /> properties. These rate offsets are applied to the currently
        /// selected tracking rate. Mounts must start up with a known or default tracking rate, and this property must return that known/default tracking rate until changed.
        /// <para>If the mount's current tracking rate cannot be determined (for example, it is a write-only property of the mount's protocol),
        /// it is permitted for the driver to force and report a default rate on connect. In this case, the preferred default is Sidereal rate.</para>
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        DriveRates TrackingRate { get; set; }

        /// <summary>
        /// Returns a collection of supported <see cref="DriveRates" /> values that describe the permissible
        /// values of the <see cref="TrackingRate" /> property for this telescope type.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented and must not throw a PropertyNotImplementedException.</b></p>
        /// At a minimum, this must contain an item for <see cref="DriveRates.driveSidereal" />.
        /// <para>This is only available for telescope InterfaceVersions 2 and later.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        ITrackingRates TrackingRates { get; }

        #endregion

        #region ITelescopeV3 members

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        /// <exception cref="ASCOM.MethodNotImplementedException">Thrown if no actions are supported.</exception>
        /// <exception cref="ASCOM.ActionNotImplementedException">It is intended that the <see cref="SupportedActions"/> method will inform clients of driver capabilities, but the driver must still throw 
        /// an <see cref="ASCOM.ActionNotImplementedException"/> exception  if it is asked to perform an action that it does not support.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented.</b></p>
        /// <para>Action names are case insensitive, so SelectWheel, selectwheel and SELECTWHEEL all refer to the same action.</para>
        /// <para>The names of all supported actions must be returned in the <see cref="SupportedActions" /> property.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV3" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Action(string ActionName, string ActionParameters);

        /// <summary>
        /// This method is a "clean-up" method that is primarily of use to drivers that are written in languages such as C# and VB.NET where resource clean-up is initially managed by the language's 
        /// runtime garbage collection mechanic. Driver authors should take care to ensure that a client or runtime calling Dispose() does not adversely affect other connected clients.
        /// Applications should not call this method.
        /// </summary>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV3" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        void Dispose();

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented</b></p>
        /// <para>This method must return an empty <see cref="ArrayList" /> if no actions are supported. Do not throw a <see cref="ASCOM.PropertyNotImplementedException" />.</para>
        /// <para>SupportedActions is a "discovery" mechanism that enables clients to know which Actions a device supports without having to exercise the Actions themselves. This mechanism is necessary because there could be
        /// people / equipment safety issues if actions are called unexpectedly or out of a defined process sequence.
        /// It follows from this that SupportedActions must return names that match the spelling of Action names exactly, without additional descriptive text. However, returned names may use any casing
        /// because the <see cref="Action" /> ActionName parameter is case insensitive.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV3" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        ArrayList SupportedActions { get; }

        #endregion

        #region ITelescopeV4 members

        /// <summary>
        /// Connect to the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>This is a mandatory method and must not throw a <see cref="MethodNotImplementedException"/>.</b></p></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Connect();

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>This is a mandatory method and must not throw a <see cref="MethodNotImplementedException"/>.</b></p></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Disconnect();

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>This is a mandatory property and must not throw a <see cref="PropertyNotImplementedException"/>.</b></p></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        bool Connecting { get; }

        /// <summary>
        /// Returns the device's operational state in a single call.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>This is a mandatory property and must not throw a <see cref="PropertyNotImplementedException"/>.</b></p>
        /// <para><b>Devices</b></para>
        /// <para>Devices must return all operational values that are definitively known but can omit entries where values are unknown.
        /// Devices must not throw exceptions / return errors when values are not known.</para>
        /// <para>An empty list must be returned if no values are known.</para>
        /// <para><b>Client Applications</b></para>
        /// <para>
        /// Applications must expect that, from time to time, some operational state values may not be present in the device response and must be prepared to handle “missing” values.
        /// </para>
        /// <para><b>Further Information</b></para>
        /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on how to implement DeviceState, which properties to include, and the implementation support provided by the Platform.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        IStateValueCollection DeviceState { get; }

        #endregion

    }
}
