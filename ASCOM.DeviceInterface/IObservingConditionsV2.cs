using System.Collections;
using System;
using System.Runtime.InteropServices;

#if NET35
using ASCOM.Utilities;
#elif NET472
using ASCOM.Utilities;
#else
#endif

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Defines the IObservingConditions Interface.
    /// This interface provides a limited set of values that are useful
    /// for astronomical purposes for things such as determining if it is safe to open or operate the observing system,
    /// for recording astronomical data or determining refraction corrections.
    /// </summary>
    /// <remarks>It is NOT intended as a general purpose environmental sensor system. The <see cref="Action">Action</see> method and
    /// <see cref="SupportedActions">SupportedActions</see> property can be used to extend your driver to present any further sensors that you need.
    /// </remarks>
    [ComVisible(true)]
    [Guid("4A0FB13E-D2DC-40CB-A5EA-0EA04CEC4D56")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IObservingConditionsV2
    {

        #region IObservingConditionsV1 members

        /// <summary>
        /// Set to True to connect to the device hardware. Set to False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.Connected">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// <revision visible="true" date="IObservingConditionsV2" version="Platform 7.0">Clients should use the Connect() / Disconnect() mechanic rather than setting Connected TRUE when accessing IObservingConditionsV2 or later devices.</revision>
        /// </revisionHistory>
        bool Connected { get; set; }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.Description">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        string Description { get; }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.DriverInfo">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        string DriverInfo { get; }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.DriverVersion">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        string DriverVersion { get; }

        /// <summary>
        /// The interface version number that this device supports. Must return 2 for this interface version.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.InterfaceVersion">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        short InterfaceVersion { get; }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.Name">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        string Name { get; }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.SetupDialog">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        void SetupDialog();

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
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.Action">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        string Action(string ActionName, string ActionParameters);

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.SupportedActions">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        ArrayList SupportedActions { get; }

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
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.CommandBlind">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// <revision visible="true" date="IObservingConditionsV2" version="Platform 7.0">Deprecated, see note above.</revision>
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
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.CommandBool">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// <revision visible="true" date="IObservingConditionsV2" version="Platform 7.0">Deprecated, see note above.</revision>
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
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.CommandString">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// <revision visible="true" date="IObservingConditionsV2" version="Platform 7.0">Deprecated, see note above.</revision>
        /// </revisionHistory>
        string CommandString(string Command, bool Raw = false);

        /// <summary>
        /// This method is a "clean-up" method that is primarily of use to drivers that are written in languages such as C# and VB.NET where resource clean-up is initially managed by the language's 
        /// runtime garbage collection mechanic. Driver authors should take care to ensure that a client or runtime calling Dispose() does not adversely affect other connected clients.
        /// Applications should not call this method.
        /// </summary>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.Dispose">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        void Dispose();

        /// <summary>
        /// Gets And sets the time period over which observations will be averaged
        /// </summary>
        /// <value>Time period (hours) over which to average sensor readings</value>
        /// <exception cref="ASCOM.InvalidValueException">If the value set is not available for this driver. All drivers must accept 0.0 to specify that
        /// an instantaneous value is available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.AveragePeriod">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double AveragePeriod { get; set; }

        /// <summary>
        /// Amount of sky obscured by cloud
        /// </summary>
        /// <value>percentage of the sky covered by cloud</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.CloudCover">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double CloudCover { get; }

        /// <summary>
        /// Atmospheric dew point at the observatory
        /// </summary>
        /// <value>Atmospheric dew point reported in °C.</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.DewPoint">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double DewPoint { get; }

        /// <summary>
        /// Atmospheric humidity at the observatory
        /// </summary>
        /// <value>Atmospheric humidity (%)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.Humidity">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double Humidity { get; }

        /// <summary>
        /// Atmospheric pressure at the observatory
        /// </summary>
        /// <value>Atmospheric pressure at the observatory (hPa)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.Pressure">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double Pressure { get; }

        /// <summary>
        /// Rain rate at the observatory
        /// </summary>
        /// <value>Rain rate (mm / hour)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.RainRate">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double RainRate { get; }

        /// <summary>
        /// Sky brightness at the observatory
        /// </summary>
        /// <value>Sky brightness (Lux)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.SkyBrightness">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double SkyBrightness { get; }

        /// <summary>
        /// Sky quality at the observatory
        /// </summary>
        /// <value>Sky quality measured in magnitudes per square arc second</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.SkyQuality">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double SkyQuality { get; }

        /// <summary>
        /// Seeing at the observatory measured as star full width half maximum (FWHM) in arc secs.
        /// </summary>
        /// <value>Seeing reported as star full width half maximum (arc seconds)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.StarFWHM">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double StarFWHM { get; }

        /// <summary>
        /// Sky temperature at the observatory
        /// </summary>
        /// <value>Sky temperature in °C</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.SkyTemperature">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double SkyTemperature { get; }

        /// <summary>
        /// Temperature at the observatory
        /// </summary>
        /// <value>Temperature in °C</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.Temperature">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double Temperature { get; }

        /// <summary>
        /// Wind direction at the observatory
        /// </summary>
        /// <value>Wind direction (degrees, 0..360.0)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.WindDirection">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double WindDirection { get; }

        /// <summary>
        /// Peak 3 second wind gust at the observatory over the last 2 minutes
        /// </summary>
        /// <value>Wind gust (m/s) Peak 3 second wind speed over the last 2 minutes</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.WindGust">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double WindGust { get; }

        /// <summary>
        /// Wind speed at the observatory
        /// </summary>
        /// <value>Wind speed (m/s)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.WindSpeed">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double WindSpeed { get; }

        /// <summary>
        /// Provides the time since the sensor value was last updated
        /// </summary>
        /// <param name="PropertyName">Name of the property whose time since last update is required</param>
        /// <returns>Time in seconds since the last sensor update for this property</returns>
        /// <exception cref="MethodNotImplementedException">If the sensor is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid property name parameter is supplied.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.TimeSinceLastUpdate">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        double TimeSinceLastUpdate(string PropertyName);

        /// <summary>
        /// Provides a description of the sensor providing the requested property
        /// </summary>
        /// <param name="PropertyName">Name of the sensor whose description is required</param>
        /// <returns>The description of the specified sensor.</returns>
        /// <exception cref="MethodNotImplementedException">If the sensor is not implemented.</exception>
        /// <exception cref="InvalidValueException">If an invalid property name parameter is supplied.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.SensorDescription">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        string SensorDescription(string PropertyName);

        /// <summary>
        /// Forces the driver to immediately query its attached hardware to refresh sensor values
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If this method is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.Refresh">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 6.2">Member added.</revision>
        /// </revisionHistory>
        void Refresh();

        #endregion

        #region IObservingConditionsV2 members

        /// <summary>
        /// Connect to the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.Connect">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Connect();

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.Disconnect">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Disconnect();

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.Connecting">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        bool Connecting { get; }

        /// <summary>
        /// Returns the device's operational state in a single call.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/observingconditions.html#ObservingConditions.DeviceState">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IObservingConditions" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        IStateValueCollection DeviceState { get; }

        #endregion

    }

}
