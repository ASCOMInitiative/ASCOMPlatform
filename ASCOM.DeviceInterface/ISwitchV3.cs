using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Defines the ISwitchV3 Interface
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
    [Guid("E856A9AB-E67B-4449-9177-D7593BA6AF7E")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ISwitchV3
    {

        #region ISwitch members

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.Connected">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitch" version="Platform 5.0">Member added.</revision>
        /// <revision visible="true" date="ISwitchV3" version="Platform 7.0">Clients should use the Connect() / Disconnect() mechanic rather than setting Connected TRUE when accessing ISwitchV3 or later devices.</revision>
        /// </revisionHistory>
        bool Connected { get; set; }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.Description">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitch" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        string Description { get; }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.DriverInfo">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitch" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        string DriverInfo { get; }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.DriverVersion">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitch" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        string DriverVersion { get; }

        /// <summary>
        /// The interface version number that this device supports. Must return 3 for this interface version.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.InterfaceVersion">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitch" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        short InterfaceVersion { get; }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.Name">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitch" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        string Name { get; }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.SetupDialog">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitch" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        void SetupDialog();

        /// <summary>
        /// The number of switch devices managed by this driver
        /// </summary>
        /// <returns>The number of devices managed by this driver.</returns>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.MaxSwitch">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitch" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        short MaxSwitch { get; }

        /// <summary>
        /// Return the name of switch device n.
        /// </summary>
        /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
        /// <returns>The name of the device</returns>
        /// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.GetSwitchName">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitch" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.SetSwitchName">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// </revisionHistory>
        void SetSwitchName(short id, string name);

        /// <summary>
        /// Return the state of switch device id as a boolean
        /// </summary>
        /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
        /// <returns>True or false</returns>
        /// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <exception cref="InvalidOperationException">If there is a temporary condition that prevents the device value being returned.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.GetSwitch">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitch" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.SetSwitch">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitch" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        void SetSwitch(short id, bool state);

        #endregion

        #region ISwitchV2 members

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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.Action">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// </revisionHistory>
        string Action(string ActionName, string ActionParameters);

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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.CanWrite">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// </revisionHistory>
        bool CanWrite(short id);

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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.CommandBlind">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// <revision visible="true" date="ISwitchV3" version="Platform 7.0">Deprecated, see note above.</revision>
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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.CommandBool">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// <revision visible="true" date="ISwitchV3" version="Platform 7.0">Deprecated, see note above.</revision>
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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.CommandString">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// <revision visible="true" date="ISwitchV3" version="Platform 7.0">Deprecated, see note above.</revision>
        /// </revisionHistory>
        string CommandString(string Command, bool Raw = false);

        /// <summary>
        /// This method is a "clean-up" method that is primarily of use to drivers that are written in languages such as C# and VB.NET where resource clean-up is initially managed by the language's 
        /// runtime garbage collection mechanic. Driver authors should take care to ensure that a client or runtime calling Dispose() does not adversely affect other connected clients.
        /// Applications should not call this method.
        /// </summary>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.Dispose">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// </revisionHistory>
        void Dispose();

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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.GetSwitchDescription">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// </revisionHistory>
        string GetSwitchDescription(short id);

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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.GetSwitchValue">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// </revisionHistory>
        double GetSwitchValue(short id);

        /// <summary>
        /// Returns the maximum value for this switch device, this must be greater than <see cref="MinSwitchValue"/>.
        /// </summary>
        /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
        /// <returns>The maximum value to which this device can be set or which a read only sensor will return.</returns>
        /// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.MaxSwitchValue">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// </revisionHistory>
        double MaxSwitchValue(short id);

        /// <summary>
        /// Returns the minimum value for this switch device, this must be less than <see cref="MaxSwitchValue"/>
        /// </summary>
        /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
        /// <returns>The minimum value to which this device can be set or which a read only sensor will return.</returns>
        /// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.MinSwitchValue">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// </revisionHistory>
        double MinSwitchValue(short id);

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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.SetSwitchValue">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// </revisionHistory>
        void SetSwitchValue(short id, double value);

        /// <summary>
        /// Returns the step size that this device supports (the difference between successive values of the device).
        /// </summary>
        /// <param name="id">The device number (0 to <see cref="MaxSwitch"/> - 1)</param>
        /// <returns>The step size for this device.</returns>
        /// <exception cref="InvalidValueException">If id is outside the range 0 to <see cref="MaxSwitch"/> - 1</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.SwitchStep">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// </revisionHistory>
        double SwitchStep(short id);

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.SupportedActions">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV2" version="Platform 6.1">Member added.</revision>
        /// </revisionHistory>
        ArrayList SupportedActions { get; }

        #endregion

        #region ISwitchV3  members

        /// <summary>
        /// Connect to the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.Connect">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Connect();

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.Disconnect">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Disconnect();

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.Connecting">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        bool Connecting { get; }

        /// <summary>
        /// Returns the device's operational state in a single call.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.DeviceState">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        IStateValueCollection DeviceState { get; }

        /// <summary>
        /// Set a boolean switch's state asynchronously
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When CanAsync(id) is false.</exception>
        /// <param name="id">Switch number.</param>
        /// <param name="state">New boolean state.</param>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.SetAsync">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void SetAsync(short id, bool state);

        /// <summary>
        /// Set a switch's value asynchronously
        /// </summary>
        /// <param name="id">Switch number.</param>
        /// <param name="value">New double value.</param>
        /// <p style="color:red"><b>This is an optional method and can throw a <see cref="MethodNotImplementedException"/> when <see cref="CanAsync(short)"/> is <see langword="false"/>.</b></p>
        /// <exception cref="MethodNotImplementedException">When CanAsync(id) is false.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.SetAsyncValue">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void SetAsyncValue(short id, double value);

        /// <summary>
        /// Flag indicating whether this switch can operate asynchronously.
        /// </summary>
        /// <param name="id">Switch number.</param>
        /// <returns>True if the switch can operate asynchronously.</returns>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.CanAsync">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        bool CanAsync(short id);

        /// <summary>
        /// Completion variable for asynchronous switch state change operations.
        /// </summary>
        /// <param name="id">Switch number.</param>
        /// <exception cref="OperationCancelledException">When an in-progress operation is cancelled by the <see cref="CancelAsync(short)"/> method.</exception>
        /// <returns>False while an asynchronous operation is underway and true when it has completed.</returns>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.StateChangeComplete">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        bool StateChangeComplete(short id);

        /// <summary>
        /// Cancels an in-progress asynchronous state change operation.
        /// </summary>
        /// <param name="id">Switch number.</param>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/switch.html#Switch.CancelAsync">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ISwitchV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void CancelAsync(short id);

        #endregion

    }
}