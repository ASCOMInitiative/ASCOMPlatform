using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    // -----------------------------------------------------------------------
    // <summary>Defines the ISwitchV2 Interface</summary>
    // -----------------------------------------------------------------------
    /// <summary>
    /// Defines the ISwitchV2 Interface
    /// </summary>
    /// <remarks>
    /// <para>The Switch interface is used to define a number of 'switch devices'. A switch device can be used to control something, such as a power switch
    /// or may be used to sense the state of something, such as a limit switch.</para>
    /// <para>This SwitchV2 interface is an extension of the original Switch interface.  The changes allow devices to have more than two states and
    /// to distinguish between devices that are writable and those that are read only.</para>
    /// <para><b>Compatibility between Switch and SwitchV2 interfaces:</b></para>
    /// <list type="bullet"><item>Switch devices that implemented the original Switch interface and
    /// client applications that use the original interface will still work together.</item>
    /// <item>Client applications that implement the original
    /// Switch interface should still work with drivers that implement the new interface.</item>
    /// <item>Client applications that use the new features in this interface
    /// will not work with drivers that do not implement the new interface.</item>
    /// </list>
    /// <para>Each device has a CanWrite method, this is true if it can be written to or false if the device can only be read.</para>
    /// <para>The new MinSwitchValue, MaxSwitchValue and SwitchStep methods are used to define the range and values that a device can handle.
    /// This also defines the number of different values - states - that a device can have, from two for a traditional on-off switch, through
    /// those with a small number of states to those which have many states.</para>
    /// <para>The SetSwitchValue and GetSwitchValue methods are used to set and get the value of a device as a double.</para>
    /// <para>There is no fundamental difference between devices with different numbers of states.</para>
    /// <para><b>Naming Conventions</b></para>
    /// <para>Each device handled by a Switch is known as a device or switch device for general cases,
    /// a controller device if it can alter the state of the device and a sensor device if it can only be read.</para>
    /// <para>For convenience devices are referred to as boolean if the device can only have two states, and multi-state if it can have more than two values.
    /// <b>These are treated the same in the interface definition</b>.</para>
    /// </remarks>
    [Guid("71A6CA6B-A86B-4EBB-8DA3-6D91705177A3")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ISwitchV2
    {

		// IAscomDriver Methods

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
		/// </remarks>
		bool Connected { get; set; }

		/// <summary>
		/// Returns a description of the device, such as manufacturer and modelnumber. Any ASCII characters may be used.
		/// </summary>
		/// <value>The description.</value>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
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
		/// It should not to be confused with the <see cref="InterfaceVersion" /> property, which is the version of this specification supported by the
		/// driver.
		/// </remarks>
		string DriverVersion { get; }

		/// <summary>
		/// The interface version number that this device supports. Must return 2 for this interface version.
		/// </summary>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Must be implemented</b></p> Clients can detect legacy V1 drivers by trying to read ths property.
		/// If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1.
		/// In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver.
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
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Must be implemented.</b></p>
		/// <para>Action names are case insensitive, so SelectWheel, selectwheel and SELECTWHEEL all refer to the same action.</para>
		/// <para>The names of all supported actions must be returned in the <see cref="SupportedActions" /> property.</para>
		/// </remarks>
		string Action(string ActionName, string ActionParameters);

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
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
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
		/// <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
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
		/// <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
		string CommandString(string Command, bool Raw = false);

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
 		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
       void Dispose();


		/// <summary>
		/// The number of switch devices managed by this driver
		/// </summary>
		/// <returns>The number of devices managed by this driver.</returns>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Must be implemented, must not throw a <see cref="PropertyNotImplementedException"/></b></p>
		/// <p>Devices are numbered from 0 to <see cref="MaxSwitch"/> - 1</p></remarks>
		short MaxSwitch { get; }

		/// <summary>
		/// Return the name of switch device n.
		/// </summary>
		/// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
		/// <returns>The name of the device</returns>
		/// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p>
		/// <para>Devices are numbered from 0 to <see cref="MaxSwitch"/> - 1</para></remarks>
		string GetSwitchName(short id);

		/// <summary>
		/// Set a switch device name to a specified value.
		/// </summary>
		/// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
		/// <param name="name">The name of the device</param>
		/// <exception cref="MethodNotImplementedException">If the device name cannot be set in the application code.</exception>
		/// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Can throw a <see cref="ASCOM.MethodNotImplementedException"/> if the device name can not be set by the application.</b></p>
		/// <para>Devices are numbered from 0 to <see cref="MaxSwitch"/> - 1</para>
		/// </remarks>
		void SetSwitchName(short id, string name);

		/// <summary>
		/// Gets the description of the specified switch device. This is to allow a fuller description of
		/// the device to be returned, for example for a tool tip.
		/// </summary>
		/// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
		/// <returns>
		/// String giving the device description.
		/// </returns>
		/// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Must be implemented, must not throw an ASCOM.MethodNotImplementedException</b></p>
		/// <para>Decices are numbered from 0 to <see cref="MaxSwitch"/> - 1</para>
		/// <para>This is a Version 2 method.</para>
		/// </remarks>
		string GetSwitchDescription(short id);

		/// <summary>
		/// Reports if the specified switch device can be written to, default true.
		/// This is false if the device cannot be written to, for example a limit switch or a sensor.
		/// </summary>
		/// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
		/// <returns>
		/// <c>true</c> if the device can be written to, otherwise <c>false</c>.
		/// </returns>
		/// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks>
		/// <para>Devices are numbered from 0 to <see cref="MaxSwitch"/> - 1</para>
		/// <para>This is a Version 2 method, version 1 switch devices can be assumed to be writable.</para>
		/// </remarks>
		bool CanWrite(short id);

		/// <summary>
		/// Return the state of switch device id as a boolean
		/// </summary>
		/// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
		/// <returns>True or false</returns>
		/// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
		/// <exception cref="InvalidOperationException">If there is a temporary condition that prevents the device value being returned.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Must be implemented, must not throw a <see cref="ASCOM.MethodNotImplementedException"/>.</b></p>
		/// <para>All devices must implement this. A multi-state device will return true if the device is at the maximum value, false if the value is at the minumum
		/// and either true or false as specified by the driver developer for intermediate values.</para>
		/// <para>Some devices do not support reading their state although they do allow state to be set. In these cases, on startup, the driver can not know the hardware state and it is recommended that the
		/// driver either:</para>
		/// <list type="bullet">
		/// <item><description>Sets the device to a known state on connection</description></item>
		/// <item><description>Throws an <see cref="ASCOM.InvalidOperationException"/> until the client software has set the device state for the first time</description></item>
		/// </list>
		/// <para>In both cases the driver should save a local copy of the state which it last set and return this through <see cref="GetSwitch" /> and <see cref="GetSwitchValue" /></para>
		/// <para>Devices are numbered from 0 to <see cref="MaxSwitch"/> - 1</para></remarks>
		bool GetSwitch(short id);

		/// <summary>
		/// Sets a switch controller device to the specified state, true or false.
		/// </summary>
		/// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
		/// <param name="state">The required control state</param>
		/// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
		/// <exception cref="MethodNotImplementedException">If <see cref="CanWrite"/> is false.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Can throw a <see cref="ASCOM.MethodNotImplementedException"/> if <see cref="CanWrite"/> is False.</b></p>
		/// <para><see cref="GetSwitchValue"/> must return <see cref="MaxSwitchValue" /> if the set state is true and <see cref="MinSwitchValue" /> if the set state is false.</para>
		/// <para>Devices are numbered from 0 to <see cref="MaxSwitch"/> - 1</para></remarks>
		void SetSwitch(short id, bool state);

		/// <summary>
		/// Returns the maximum value for this switch device, this must be greater than <see cref="MinSwitchValue"/>.
		/// </summary>
		/// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
		/// <returns>The maximum value to which this device can be set or which a read only sensor will return.</returns>
		/// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Must be implemented, must not throw a <see cref="ASCOM.MethodNotImplementedException"/>.</b></p>
		/// <para>If a two state device cannot report its state,  <see cref="MaxSwitchValue"/> must return the value 1.0.</para>
		/// <para> Devices are numbered from 0 to <see cref="MaxSwitch"/> - 1.</para>
		/// <para>This is a Version 2 method.</para>
		/// </remarks>
		double MaxSwitchValue(short id);

		/// <summary>
		/// Returns the minimum value for this switch device, this must be less than <see cref="MaxSwitchValue"/>
		/// </summary>
		/// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
		/// <returns>The minimum value to which this device can be set or which a read only sensor will return.</returns>
		/// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Must be implemented, must not throw a <see cref="ASCOM.MethodNotImplementedException"/>.</b></p>
		/// <para>If a two state device cannot report its state, <see cref="MinSwitchValue"/> must return the value 0.0.</para>
		/// <para> Devices are numbered from 0 to <see cref="MaxSwitch"/> - 1.</para>
		/// <para>This is a Version 2 method.</para>
		/// </remarks>
		double MinSwitchValue(short id);

		/// <summary>
		/// Returns the step size that this device supports (the difference between successive values of the device).
		/// </summary>
		/// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
		/// <returns>The step size for this device.</returns>
		/// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Must be implemented, must not throw <see cref="ASCOM.MethodNotImplementedException"/>.</b></p>
		/// <para>SwitchStep, MinSwitchValue and MaxSwitchValue can be used to determine the way the device is controlled and/or displayed,
		/// for example by setting the number of decimal places or number of states for a display.</para>
		/// <para><see cref="SwitchStep"/> must be greater than zero and the number of steps can be calculated as:
		/// ((<see cref="MaxSwitchValue"/> - <see cref="MinSwitchValue"/>) / <see cref="SwitchStep"/>) + 1.</para>
		/// <para>The switch range (<see cref="MaxSwitchValue"/> - <see cref="MinSwitchValue"/>) must be an exact multiple of <see cref="SwitchStep"/>.</para>
		/// <para>If a two state device cannot report its state, <see cref="SwitchStep"/> must return the value 1.0.</para>
		/// <para>Devices are numbered from 0 to <see cref="MaxSwitch"/> - 1.</para>
		/// <para>This is a Version 2 method.</para>
		/// </remarks>
		double SwitchStep(short id);

		/// <summary>
		/// Returns the value for switch device id as a double
		/// </summary>
		/// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
		/// <returns>The value for this switch, this is expected to be between <see cref="MinSwitchValue"/> and
		/// <see cref="MaxSwitchValue"/>.</returns>
		/// <exception cref="InvalidOperationException">If there is a temporary condition that prevents the device value being returned.</exception>
		/// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Must be implemented, must not throw a <see cref="ASCOM.MethodNotImplementedException"/>.</b></p>
		/// <para>Some devices do not support reading their state although they do allow state to be set. In these cases, on startup, the driver can not know the hardware state and it is recommended that the
		/// driver either:</para>
		/// <list type="bullet">
		/// <item><description>Sets the device to a known state on connection</description></item>
		/// <item><description>Throws an <see cref="ASCOM.InvalidOperationException"/> until the client software has set the device state for the first time</description></item>
		/// </list>
		/// <para>In both cases the driver should save a local copy of the state which it last set and return this through <see cref="GetSwitch" /> and <see cref="GetSwitchValue" /></para>
		/// <para>Devices are numbered from 0 to <see cref="MaxSwitch"/> - 1.</para>
		/// <para>This is a Version 2 method.</para>
		/// </remarks>
		double GetSwitchValue(short id);

		/// <summary>
		/// Set the value for this device as a double.
		/// </summary>
		/// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
		/// <param name="value">The value to be set, between <see cref="MinSwitchValue"/> and <see cref="MaxSwitchValue"/></param>
		/// <exception cref="ASCOM.InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
		/// <exception cref="ASCOM.InvalidValueException">If value is outside the range <see cref="MinSwitchValue"/> to <see cref="MaxSwitchValue"/></exception>
		/// <exception cref="ASCOM.MethodNotImplementedException">If <see cref="CanWrite"/> is false.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
		/// <remarks><p style="color:red"><b>Can throw a <see cref="ASCOM.MethodNotImplementedException"/> if <see cref="CanWrite"/> is False.</b></p>
		/// <para>If the value is more than <see cref="MaxSwitchValue"/> or less than <see cref="MinSwitchValue"/>
		/// then the method must throw an <see cref="ASCOM.InvalidValueException"/>.</para>
		/// <para>A set value that is intermediate between the values specified by <see cref="SwitchStep"/> must result in the device being set to an achievable value close to the requested set value.</para>
		/// <para>Devices are numbered from 0 to <see cref="MaxSwitch"/> - 1.</para>
		/// <para>This is a Version 2 method.</para>
		/// </remarks>
		void SetSwitchValue(short id, double value);
    }
}
