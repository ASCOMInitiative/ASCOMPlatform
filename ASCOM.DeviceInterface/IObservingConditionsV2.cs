// -----------------------------------------------------------------------
// <summary>Defines the IObservingConditions Interface</summary>
// -----------------------------------------------------------------------
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;


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
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p>
        /// <para>Do not use a NotConnectedException here. That exception is for use in other methods that require a connection in order to succeed.</para>
        /// <para>The Connected property sets and reports the state of connection to the device hardware.
        /// For a hub this means that Connected will be true when the first driver connects and will only be set to false
        /// when all drivers have disconnected.  A second driver may find that Connected is already true and
        /// setting Connected to false does not report Connected as false.  This is not an error because the physical state is that the
        /// hardware connection is still true.</para>
        /// <para>Multiple calls setting Connected to true or false will not cause an error.</para>
        /// </remarks>
        bool Connected { get; set; }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented, must not throw a PropertyNotImplementedException.</b></p> 
        /// <para>The description length must be a maximum of 64 characters so that it can be used in FITS image headers, which are limited to 80 characters including the header name.</para>
        /// </remarks>
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
        string DriverInfo { get; }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> This must be in the form "n.n".
        /// It should not be confused with the <see cref="InterfaceVersion" /> property, which is the version of this specification supported by the
        /// driver.
        /// </remarks>
        string DriverVersion { get; }

        /// <summary>
        /// The interface version number that this device supports. Must return 2 for this interface version.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p>This value will be incremented if the interface specification is extended in the future.
        /// </remarks>
        short InterfaceVersion { get; }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
        string Name { get; }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
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
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented.</b></p>
        /// <para>Action names are case insensitive, so SelectWheel, selectwheel and SELECTWHEEL all refer to the same action.</para>
        /// <para>The names of all supported actions must be returned in the <see cref="SupportedActions" /> property.</para>
        /// </remarks>
        string Action(string ActionName, string ActionParameters);

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p>
        /// <para>This method must return an empty <see cref="ArrayList" /> if no actions are supported. Do not throw a <see cref="ASCOM.PropertyNotImplementedException" />.</para>
        /// <para>SupportedActions is a "discovery" mechanism that enables clients to know which Actions a device supports without having to exercise the Actions themselves. This mechanism is necessary because there could be
        /// people / equipment safety issues if actions are called unexpectedly or out of a defined process sequence.
        /// It follows from this that SupportedActions must return names that match the spelling of Action names exactly, without additional descriptive text. However, returned names may use any casing
        /// because the <see cref="Action" /> ActionName parameter is case insensitive.</para>
        /// </remarks>
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
        /// <remarks><p style="color:red"><b>May throw a NotImplementedException.</b></p>
        /// <para>The CommandXXX methods are a historic mechanic that provides clients with direct and unimpeded access to change device hardware configuration. While highly enabling for clients, this mechanic is inherently risky
        /// because clients can fundamentally change hardware operation without the driver being aware that a change is taking / has taken place.</para>
        /// <para>The newer Action / SupportedActions mechanic provides discrete, named, functions that can deliver any functionality required.They do need driver authors to make provision for them within the 
        /// driver, but this approach is much lower risk than using the CommandXXX methods because it enables the driver to resolve conflicts between standard device interface commands and extended commands 
        /// provided as Actions.The driver is always aware of what is happening and can adapt more effectively to client needs.</para>
        /// </remarks>
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
        /// <remarks><p style="color:red"><b>May throw a NotImplementedException.</b></p>
        /// <para>The CommandXXX methods are a historic mechanic that provides clients with direct and unimpeded access to change device hardware configuration. While highly enabling for clients, this mechanic is inherently risky
        /// because clients can fundamentally change hardware operation without the driver being aware that a change is taking / has taken place.</para>
        /// <para>The newer Action / SupportedActions mechanic provides discrete, named, functions that can deliver any functionality required.They do need driver authors to make provision for them within the 
        /// driver, but this approach is much lower risk than using the CommandXXX methods because it enables the driver to resolve conflicts between standard device interface commands and extended commands 
        /// provided as Actions.The driver is always aware of what is happening and can adapt more effectively to client needs.</para>
        /// </remarks>
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
        /// <remarks><p style="color:red"><b>May throw a NotImplementedException.</b></p>
        /// <para>The CommandXXX methods are a historic mechanic that provides clients with direct and unimpeded access to change device hardware configuration. While highly enabling for clients, this mechanic is inherently risky
        /// because clients can fundamentally change hardware operation without the driver being aware that a change is taking / has taken place.</para>
        /// <para>The newer Action / SupportedActions mechanic provides discrete, named, functions that can deliver any functionality required.They do need driver authors to make provision for them within the 
        /// driver, but this approach is much lower risk than using the CommandXXX methods because it enables the driver to resolve conflicts between standard device interface commands and extended commands 
        /// provided as Actions.The driver is always aware of what is happening and can adapt more effectively to client needs.</para>
        /// </remarks>
        string CommandString(string Command, bool Raw = false);

        /// <summary>
        /// This method is a "clean-up" method that is primarily of use to drivers that are written in languages such as C# and VB.NET where resource clean-up is initially managed by the language's 
        /// runtime garbage collection mechanic. Driver authors should take care to ensure that a client or runtime calling Dispose() does not adversely affect other connected clients.
        /// Applications should not call this method.
        /// </summary>
		void Dispose();

        /// <summary>
        /// Gets And sets the time period over which observations will be averaged
        /// </summary>
        /// <value>Time period (hours) over which to average sensor readings</value>
        /// <exception cref="ASCOM.InvalidValueException">If the value set is not available for this driver. All drivers must accept 0.0 to specify that
        /// an instantaneous value is available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Mandatory property, must be implemented, can NOT throw a PropertyNotImplementedException</b></p>
        /// <para>This property should return the time period (hours) over which sensor readings will be averaged. If your driver is delivering instantaneous sensor readings this property should return a value of 0.0.</para>
        /// <para>Please resist the temptation to throw exceptions when clients query sensor properties when insufficient time has passed to get a true average reading.
        /// A best estimate of the average sensor value should be returned in these situations. </para>
        /// </remarks>
        double AveragePeriod { get; set; }

        /// <summary>
        /// Amount of sky obscured by cloud
        /// </summary>
        /// <value>percentage of the sky covered by cloud</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
        /// This property should return a value between 0.0 and 100.0 where 0.0 = clear sky and 100.0 = 100% cloud coverage
        /// </remarks>
        double CloudCover { get; }

        /// <summary>
        /// Atmospheric dew point at the observatory
        /// </summary>
        /// <value>Atmospheric dew point reported in °C.</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException when the <see cref="Humidity"/> property also throws a PropertyNotImplementedException.</b></p>
        /// <p style="color:red"><b>Mandatory property, must NOT throw a PropertyNotImplementedException when the <see cref="Humidity"/> property is implemented.</b></p>
        /// <para>The units of this property are degrees Celsius. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
        /// to convert these units to and from degrees Fahrenheit.</para>
        /// <para>The ASCOM specification requires that DewPoint and Humidity are either both implemented or both throw PropertyNotImplementedExceptions. It is not allowed for
        /// one to be implemented and the other to throw a PropertyNotImplementedException. The Utilities component contains methods (<see cref="Util.DewPoint2Humidity"/> and
        /// <see cref="Util.Humidity2DewPoint"/>) to convert DewPoint to Humidity and vice versa given the ambient temperature.</para>
        /// </remarks>
        double DewPoint { get; }

        /// <summary>
        /// Atmospheric humidity at the observatory
        /// </summary>
        /// <value>Atmospheric humidity (%)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException when the <see cref="DewPoint"/> property also throws a PropertyNotImplementedException.</b></p>
        /// <p style="color:red"><b>Mandatory property, must NOT throw a PropertyNotImplementedException when the <see cref="DewPoint"/> property is implemented.</b></p>
        /// <para>The ASCOM specification requires that DewPoint and Humidity are either both implemented or both throw PropertyNotImplementedExceptions. It is not allowed for
        /// one to be implemented and the other to throw a PropertyNotImplementedException. The Utilities component contains methods (<see cref="Util.DewPoint2Humidity"/> and
        /// <see cref="Util.Humidity2DewPoint"/>) to convert DewPoint to Humidity and vice versa given the ambient temperature.</para>
        /// <para>This property should return a value between 0.0 and 100.0 where 0.0 = 0% relative humidity and 100.0 = 100% relative humidity.</para>
        /// </remarks>
        double Humidity { get; }

        /// <summary>
        /// Atmospheric pressure at the observatory
        /// </summary>
        /// <value>Atmospheric pressure at the observatory (hPa)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
        /// <para>The units of this property are hectoPascals. Client and driver authors can use the method <see cref="Util.ConvertUnits"/>
        /// to convert these units to and from milliBar, mm of mercury and inches of mercury.</para>
        /// <para>This must be the pressure at the observatory altitude and not the adjusted pressure at sea level.
        /// Please check whether your pressure sensor delivers local observatory pressure or sea level pressure and, if it returns sea level pressure,
        /// adjust this to actual pressure at the observatory's altitude before returning a value to the client.
        /// The <see cref="Util.ConvertPressure"/> method can be used to effect this adjustment.
        /// </para>
        /// </remarks>
        double Pressure { get; }

        /// <summary>
        /// Rain rate at the observatory
        /// </summary>
        /// <value>Rain rate (mm / hour)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
        /// <para>The units of this property are millimetres per hour. Client and driver authors can use the method <see cref="Util.ConvertUnits"/>
        /// to convert these units to and from inches per hour.</para>
        /// <para>This property can be interpreted as 0.0 = Dry any positive nonzero value = wet.</para>
        /// <para>Rainfall intensity is classified according to the rate of precipitation:</para>
        /// <list type="bullet">
        /// <item><description>Light rain — when the precipitation rate is less than 2.5 mm (0.098 in) per hour</description></item>
        /// <item><description>Moderate rain — when the precipitation rate is between 2.5 mm (0.098 in) and 10 mm (0.39 in) per hour</description></item>
        /// <item><description>Heavy rain — when the precipitation rate is between 10 mm (0.39 in) and 50 mm (2.0 in) per hour</description></item>
        /// <item><description>Violent rain — when the precipitation rate is &gt; 50 mm (2.0 in) per hour</description></item>
        /// </list>
        /// </remarks>
        double RainRate { get; }

        /// <summary>
        /// Sky brightness at the observatory
        /// </summary>
        /// <value>Sky brightness (Lux)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
        /// This property returns the sky brightness measured in Lux.
        /// <para>Luminance Examples in Lux</para>
        /// <list type="table">
        /// <listheader>
        /// <term>Illuminance</term><term>Surfaces illuminated by:</term>
        /// </listheader>
        /// <item><description>0.0001 lux</description><description>Moonless, overcast night sky (starlight)</description></item>
        /// <item><description>0.002 lux</description><description>Moonless clear night sky with airglow</description></item>
        /// <item><description>0.27–1.0 lux</description><description>Full moon on a clear night</description></item>
        /// <item><description>3.4 lux</description><description>Dark limit of civil twilight under a clear sky</description></item>
        /// <item><description>50 lux</description><description>Family living room lights (Australia, 1998)</description></item>
        /// <item><description>80 lux</description><description>Office building hallway/toilet lighting</description></item>
        /// <item><description>100 lux</description><description>Very dark overcast day</description></item>
        /// <item><description>320–500 lux</description><description>Office lighting</description></item>
        /// <item><description>400 lux</description><description>Sunrise or sunset on a clear day.</description></item>
        /// <item><description>1000 lux</description><description>Overcast day; typical TV studio lighting</description></item>
        /// <item><description>10000–25000 lux</description><description>Full daylight (not direct sun)</description></item>
        /// <item><description>32000–100000 lux</description><description>Direct sunlight</description></item>
        /// </list>
        /// </remarks>
        double SkyBrightness { get; }

        /// <summary>
        /// Sky quality at the observatory
        /// </summary>
        /// <value>Sky quality measured in magnitudes per square arc second</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
        /// <para>Sky quality is typically measured in units of magnitudes per square arc second. A sky quality of 20 magnitudes per square arc second means that the
        /// overall sky appears with a brightness equivalent to having 1 magnitude 20 star in each square arc second of sky.</para>
        /// <para >Examples of typical sky quality values were published by Sky and Telescope (<a href="http://www.skyandtelescope.com/astronomy-resources/rate-your-skyglow/">http://www.skyandtelescope.com/astronomy-resources/rate-your-skyglow/</a>) and, in slightly adapted form, are reproduced below:</para>
        /// <para>
        /// <table style="width:80.0%;" cellspacing="0" width="80.0%">
        /// <col style="width: 20.0%;"></col>
        /// <col style="width: 80.0%;"></col>
        /// <tr>
        /// <td colspan="1" rowspan="1" style="width: 20.0%; padding-right: 10px; padding-left: 10px;
        /// text-align: center;vertical-align: middle;
        /// border-left-color: #000000; border-left-style: Solid;
        /// border-top-color: #000000; border-top-style: Solid;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px;
        /// background-color: #00ffff;" width="10.0">
        /// <b>Sky Quality (mag/arcsecond<sup>2</sup>)</b></td>
        /// <td colspan="1" rowspan="1" style="width: 80.0%; padding-right: 10px; padding-left: 10px;
        /// text-align: center;vertical-align: middle;
        /// border-top-color: #000000; border-top-style: Solid;
        /// border-right-style: Solid; border-right-color: #000000;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px;
        /// background-color: #00ffff;" width="90.0">
        /// <b>Description</b></td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// text-align: center;vertical-align: middle;
        /// border-left-color: #000000; border-left-style: Solid;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 22.0</td>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// By convention, this is often assumed to be the average brightness of a moonless night sky that's completely free of artificial light pollution.</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// text-align: center;vertical-align: middle;
        /// border-left-color: #000000; border-left-style: Solid;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 21.0</td>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// This is typical for a rural area with a medium-sized city not far away. It's comparable to the glow of the brightest section of the northern Milky Way, from Cygnus through Perseus. </td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// text-align: center;vertical-align: middle;
        /// border-left-color: #000000; border-left-style: Solid;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 20.0</td>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// This is typical for the outer suburbs of a major metropolis. The summer Milky Way is readily visible but severely washed out.</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// text-align: center;vertical-align: middle;
        /// border-left-color: #000000; border-left-style: Solid;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 19.0</td>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Typical for a suburb with widely spaced single-family homes. It's a little brighter than a remote rural site at the end of nautical twilight, when the Sun is 12° below the horizon.</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// text-align: center;vertical-align: middle;
        /// border-left-color: #000000; border-left-style: Solid;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 18.0</td>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Bright suburb or dark urban neighborhood. It's also a typical zenith skyglow at a rural site when the Moon is full. The Milky Way is invisible, or nearly so.</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// text-align: center;vertical-align: middle;
        /// border-left-color: #000000; border-left-style: Solid;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 17.0</td>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// Typical near the center of a major city.</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// text-align: center;vertical-align: middle;
        /// border-left-color: #000000; border-left-style: Solid;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 13.0</td>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// The zenith skyglow at the end of civil twilight, roughly a half hour after sunset, when the Sun is 6° below the horizon. Venus and Jupiter are easy to see, but bright stars are just beginning to appear.</td>
        /// </tr>
        /// <tr>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// text-align: center;vertical-align: middle;
        /// border-left-color: #000000; border-left-style: Solid;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// 7.0</td>
        /// <td style="padding-right: 10px; padding-left: 10px;
        /// border-right-color: #000000; border-right-style: Solid;
        /// border-bottom-color: #000000; border-bottom-style: Solid;
        /// border-right-width: 1px; border-left-width: 1px; border-top-width: 1px; border-bottom-width: 1px; ">
        /// The zenith skyglow at sunrise or sunset</td>
        /// </tr>
        /// </table>
        /// </para>
        /// </remarks>
        double SkyQuality { get; }

        /// <summary>
        /// Seeing at the observatory measured as star full width half maximum (FWHM) in arc secs.
        /// </summary>
        /// <value>Seeing reported as star full width half maximum (arc seconds)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
        /// </remarks>
        double StarFWHM { get; }

        /// <summary>
        /// Sky temperature at the observatory
        /// </summary>
        /// <value>Sky temperature in °C</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
        /// <para>The units of this property are degrees Celsius. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
        /// to convert these units to and from degrees Fahrenheit.</para>
        /// <para>This is expected to be returned by an infra-red sensor looking at the sky. The lower the temperature the more the sky is likely to be clear.</para>
        /// </remarks>
        double SkyTemperature { get; }

        /// <summary>
        /// Temperature at the observatory
        /// </summary>
        /// <value>Temperature in °C</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
        /// <para>The units of this property are degrees Celsius. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
        /// to convert these units to and from degrees Fahrenheit.</para>
        /// <para>This is expected to be the ambient temperature at the observatory.</para>
        /// </remarks>
        double Temperature { get; }

        /// <summary>
        /// Wind direction at the observatory
        /// </summary>
        /// <value>Wind direction (degrees, 0..360.0)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
        /// The returned value must be between 0.0 and 360.0, interpreted according to the meteorological standard, where a special value of 0.0 is returned when the wind speed is 0.0.
        /// Wind direction is measured clockwise from north, through east, where East=90.0, South=180.0, West=270.0 and North=360.0.
        /// </remarks>
        double WindDirection { get; }

        /// <summary>
        /// Peak 3 second wind gust at the observatory over the last 2 minutes
        /// </summary>
        /// <value>Wind gust (m/s) Peak 3 second wind speed over the last 2 minutes</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
        /// The units of this property are metres per second. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
        /// to convert these units to and from miles per hour or knots.
        /// </remarks>
        double WindGust { get; }

        /// <summary>
        /// Wind speed at the observatory
        /// </summary>
        /// <value>Wind speed (m/s)</value>
        /// <exception cref="PropertyNotImplementedException">If this property is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional property, can throw a PropertyNotImplementedException</b></p>
        /// The units of this property are metres per second. Driver and application authors can use the <see cref="Util.ConvertUnits"/> method
        /// to convert these units to and from miles per hour or knots.
        /// </remarks>
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
        /// <remarks>
        /// <p style="color:red"><b>Must Not throw a MethodNotImplementedException when the specified sensor Is implemented but must throw a MethodNotImplementedException when the specified sensor Is Not implemented.</b></p>
        /// <para>PropertyName must be the name of one of the sensor properties specified in the <see cref="IObservingConditionsV2"/> interface. If the caller supplies some other value, throw an InvalidValueException.</para>
        /// <para>Return a negative value to indicate that no valid value has ever been received from the hardware.</para>
        /// <para>If an empty string is supplied as the PropertyName, the driver must return the time since the most recent update of any sensor. A MethodNotImplementedException must not be thrown in this circumstance.</para>
        /// </remarks>
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
        /// <remarks>
        /// <p style="color:red"><b>Must Not throw a MethodNotImplementedException when the specified sensor Is implemented
        /// but must throw a MethodNotImplementedException when the specified sensor Is Not implemented.</b></p>
        /// <para>PropertyName must be the name of one of the sensor properties specified in the <see cref="IObservingConditionsV2"/> interface. If the caller supplies some other value, throw an InvalidValueException.</para>
        /// <para>If the sensor is implemented, this must return a valid string, even if the driver is not connected, so that applications can use this to determine what sensors are available.</para>
        /// <para>If the sensor is not implemented, this must throw a MethodNotImplementedException.</para>
        /// </remarks>
        string SensorDescription(string PropertyName);

        /// <summary>
        /// Forces the driver to immediately query its attached hardware to refresh sensor values
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If this method is not available.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional method, can throw a MethodNotImplementedException</b></p>
        /// <para>This must be a short-lived synchronous call that triggers a refresh. It must not wait for long running processes to complete. 
		/// It is the client's responsibility to poll , <see cref="TimeSinceLastUpdate(string)"/> to determine whether / when the data has been refreshed.</para>
		/// </remarks>
		void Refresh();

        #endregion

        #region IObservingConditionsV2 members

        /// <summary>
        /// Connect to the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>This is a mandatory method and must not throw a <see cref="MethodNotImplementedException"/>.</b></p></remarks>
        void Connect();

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>This is a mandatory method and must not throw a <see cref="MethodNotImplementedException"/>.</b></p></remarks>
        void Disconnect();

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>This is a mandatory property and must not throw a <see cref="PropertyNotImplementedException"/>.</b></p></remarks>
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
        IStateValueCollection DeviceState { get; }

        #endregion

    }

}
