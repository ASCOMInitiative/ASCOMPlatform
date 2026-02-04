using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Reflection.Emit;

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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.AbortSlew">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.AlignmentMode">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Altitude">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.ApertureDiameter">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Azimuth">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double Azimuth { get; }

        /// <summary>
        /// True if this telescope is capable of programmed finding its home position (<see cref="FindHome" /> method).
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanFindHome">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanFindHome { get; }

        /// <summary>
        /// True if this telescope is capable of programmed parking (<see cref="Park" />method)
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanPark">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanPark { get; }

        /// <summary>
        /// True if this telescope is capable of software-pulsed guiding (via the <see cref="PulseGuide" /> method)
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanPulseGuide">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanPulseGuide { get; }

        /// <summary>
        /// True if this telescope is capable of programmed setting of its park position (<see cref="SetPark" /> method)
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanSetPark">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetPark { get; }

        /// <summary>
        /// True if the <see cref="Tracking" /> property can be changed, turning telescope sidereal tracking on and off.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanSetTracking">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetTracking { get; }

        /// <summary>
        /// True if this telescope is capable of programmed slewing (synchronous or asynchronous) to equatorial coordinates
        /// </summary>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanSlew">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanSlewAsync">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanSync">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSync { get; }

        /// <summary>
        /// True if this telescope is capable of programmed unparking (<see cref="Unpark" /> method).
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanUnpark">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CommandBlind">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CommandBool">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CommandString">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Connected">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Declination">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.DeclinationRate">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Description">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        string Description { get; }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.DriverInfo">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.FindHome">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.FocalLength">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        double FocalLength { get; }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Name">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Park">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.PulseGuide">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.RightAscension">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.RightAscensionRate">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SetPark">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void SetPark();

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SetupDialog">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescope" version="Platform 3.0">Member added.</revision>
        /// </revisionHistory>
        void SetupDialog();

        /// <summary>
        /// The local apparent sidereal time from the telescope's internal clock (hours, sidereal)
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SiderealTime">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SiteElevation">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SiteLatitude">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SiteLongitude">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Slewing">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SlewSettleTime">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SlewToCoordinates">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SlewToCoordinatesAsync">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SlewToTarget">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SlewToTargetAsync">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SyncToCoordinates">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SyncToTarget">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.TargetDeclination">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.TargetRightAscension">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Tracking">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Unpark">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.UTCDate">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.ApertureArea">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.AtHome">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool AtHome { get; }

        /// <summary>
        /// True if the telescope has been put into the parked state by the <see cref="Park" /> method. Set False by calling the Unpark() method.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.AtPark">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.AxisRates">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanMoveAxis">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool CanMoveAxis(TelescopeAxes Axis);

        /// <summary>
        /// True if the <see cref="DeclinationRate" /> property can be changed to provide offset tracking in the declination axis.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanSetDeclinationRate">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetDeclinationRate { get; }

        /// <summary>
        /// True if the guide rate properties used for <see cref="PulseGuide" /> can be adjusted.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanSetGuideRates">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetGuideRates { get; }

        /// <summary>
        /// True if the <see cref="SideOfPier" /> property can be set, meaning that the mount can be forced to flip.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanSetPierSide">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetPierSide { get; }

        /// <summary>
        /// True if the <see cref="RightAscensionRate" /> property can be changed to provide offset tracking in the right ascension axis.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanSetRightAscensionRate">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool CanSetRightAscensionRate { get; }

        /// <summary>
        /// True if this telescope is capable of programmed slewing (synchronous or asynchronous) to local horizontal coordinates
        /// </summary>
        /// <exception cref="NotConnectedException">When <see cref="Connected"/> is False.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanSlewAltAz">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanSlewAltAzAsync">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.CanSyncAltAz">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.DestinationSideOfPier">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.DoesRefraction">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        bool DoesRefraction { get; set; }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.DriverVersion">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        string DriverVersion { get; }

        /// <summary>
        /// The interface version number that this device supports. Should return 4 for this interface version.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.InterfaceVersion">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV2" version="Platform 4.0">Member added.</revision>
        /// </revisionHistory>
        short InterfaceVersion { get; }

        /// <summary>
        /// Equatorial coordinate system used by this telescope (e.g. Topocentric or J2000).
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.EquatorialSystem">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.GuideRateDeclination">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.GuideRateRightAscension">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.IsPulseGuiding">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.MoveAxis">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SideOfPier">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SlewToAltAz">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SlewToAltAzAsync">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SyncToAltAz">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.TrackingRate">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.TrackingRates">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Action">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV3" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Action(string ActionName, string ActionParameters);

        /// <summary>
        /// This method is a "clean-up" method that is primarily of use to drivers that are written in languages such as C# and VB.NET where resource clean-up is initially managed by the language's 
        /// runtime garbage collection mechanic. Driver authors should take care to ensure that a client or runtime calling Dispose() does not adversely affect other connected clients.
        /// Applications should not call this method.
        /// </summary>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Dispose">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV3" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        void Dispose();

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.SupportedActions">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Connect">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Connect();

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Disconnect">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Disconnect();

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.Connecting">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        bool Connecting { get; }

        /// <summary>
        /// Returns the device's operational state in a single call.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/telescope.html#Telescope.DeviceState">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ITelescopeV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        IStateValueCollection DeviceState { get; }

        #endregion

    }
}