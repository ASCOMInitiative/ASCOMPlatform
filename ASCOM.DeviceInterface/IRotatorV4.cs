﻿using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Defines the IRotatorV4 Interface.
    /// </summary>
    /// <remarks>
    /// The IRotatorV3 interface was introduced in Platform 6.5 to add support for rotator synchronisation through these new methods:
    /// <list type="bullet">
    /// <item><see cref="MechanicalPosition"/></item>
    /// <item><see cref="Sync(float)"/></item>
    /// <item><see cref="MoveMechanical(float)"/></item>
    /// </list>
    /// <para>
    /// The rotator system (including its firmware and driver) is responsible for managing cable wrap prevention behaviour. Client applications must be able to position 
    /// the rotator from any angle (0 &lt;= angle &lt; 360) to any angle (0 &lt;= angle &lt; 360) without regard to equipment clearance or cable considerations. If needed, the rotator 
    /// may provide a user interface to configure specific behaviour in the driver's SetupDialog.
    /// </para>
    /// </remarks>
    [Guid("4ECA03B5-225D-43E5-A725-9BD6506824CB")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IRotatorV4
    {

        #region IRotatorV1 members

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p>Do not use a NotConnectedException here. That exception is for use in other methods that require a connection in order to succeed.
        /// <para>The Connected property sets and reports the state of connection to the device hardware.
        /// For a hub this means that Connected will be true when the first driver connects and will only be set to false
        /// when all drivers have disconnected.  A second driver may find that Connected is already true and
        /// setting Connected to false does not report Connected as false.  This is not an error because the physical state is that the
        /// hardware connection is still true.</para>
        /// <para>Multiple calls setting Connected to true or false will not cause an error.</para>
        /// <para><legacyBold>ICameraV4 Behaviour Clarification</legacyBold> - <see cref="IRotatorV4"/> and later clients should use the asynchronous <see cref="Connect"/> / <see cref="Disconnect"/> mechanic 
        /// rather than setting Connected <see langword="true"/> when communicating with <see cref="IRotatorV4"/> or later devices.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotator" version="Platform 5.0">Member added.</revision>
        /// <revision visible="true" date="IRotatorV4" version="Platform 7.0">Clients should use the Connect() / Disconnect() mechanic rather than setting Connected TRUE when accessing IRotatorV4 or later devices.</revision>
        /// </revisionHistory>
        bool Connected { get; set; }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotator" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        void SetupDialog();

        /// <summary>
        /// Immediately stop any Rotator motion due to a previous <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see> method call.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">Throw a MethodNotImplementedException if the rotator cannot halt.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>Optional - can throw a not implemented exception</b></p> </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotator" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        void Halt();

        /// <summary>
        /// Indicates whether the rotator is currently moving
        /// </summary>
        /// <returns>True if the Rotator is moving to a new position. False if the Rotator is stationary.</returns>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red;margin-bottom:0"><b>Must be implemented.</b></p>
        /// <para>During rotation, <see cref="IsMoving" /> must be True, otherwise it must be False.</para>
        /// <para><b>NOTE</b></para>
        /// <para>IRotatorV3, released in Platform 6.5, and later interface version require this method to be implemented, in IRotatorV2 and earlier interface versions implementation was optional.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotator" version="Platform 5.0">Member added.</revision>
        /// <revision visible="true" date="IRotatorV3" version="Platform 6.5">Implementation is now mandatory, see note above.</revision>
        /// </revisionHistory>
        bool IsMoving { get; }

        /// <summary>
        /// Causes the rotator to move Position degrees relative to the current <see cref="Position" /> value.
        /// </summary>
        /// <param name="Position">Relative position to move in degrees from current <see cref="Position" />.</param>
        /// <exception cref="InvalidValueException">If Position is invalid.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red;margin-bottom:0"><b>Must be implemented.</b></p>
        /// <para>This is an asynchronous method that returns as soon as the rotation operation has been successfully started, with the
        /// <see cref="IsMoving"/> property True (unless already at the requested position). After the requested angle is successfully reached and motion stops, 
        /// the <see cref="IsMoving"/> property must become False.</para>
        /// <para>Calling <see cref="Move">Move</see> causes the <see cref="TargetPosition" /> property to change to the sum of the current angular position
        /// and the value of the <see cref="Position" /> parameter (modulo 360 degrees), then starts rotation to <see cref="TargetPosition" />.</para>
        /// <para><b>NOTE</b></para>
        /// <para>IRotatorV3, released in Platform 6.5, and later interface version require this method to be implemented, in IRotatorV2 and earlier interface versions implementation was optional.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotator" version="Platform 5.0">Member added.</revision>
        /// <revision visible="true" date="IRotatorV3" version="Platform 6.5">Implementation is now mandatory, see note above.</revision>
        /// </revisionHistory>
        void Move(float Position);

        /// <summary>
        /// Causes the rotator to move the absolute position of <see cref="Position" /> degrees.
        /// </summary>
        /// <param name="Position">Absolute position in degrees.</param>
        /// <exception cref="InvalidValueException">If Position is invalid.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red;margin-bottom:0"><b>Must be implemented.</b></p>
        /// <para>This is an asynchronous method that returns as soon as the rotation operation has been successfully started, with the
        /// <see cref="IsMoving"/> property True (unless already at the requested position). After the requested angle is successfully reached and motion stops, 
        /// the <see cref="IsMoving"/> property must become False.</para>
        /// <para>
        /// Calling <see cref="MoveAbsolute"/> causes the <see cref="TargetPosition" /> property to change to the value of the
        /// <see cref="Position" /> parameter, then starts rotation to <see cref="TargetPosition" />. 
        /// </para>
        /// <para><b>NOTE</b></para>
        /// <para>IRotatorV3, released in Platform 6.5, and later interface version require this method to be implemented, in IRotatorV2 and earlier interface versions implementation was optional.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotator" version="Platform 5.0">Member added.</revision>
        /// <revision visible="true" date="IRotatorV3" version="Platform 6.5">Implementation is now mandatory, see note above.</revision>
        /// </revisionHistory>
        void MoveAbsolute(float Position);

        /// <summary>
        /// Current instantaneous Rotator position, allowing for any sync offset, in degrees.
        /// </summary>
        /// <exception cref="InvalidValueException">If Position is invalid.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red;margin-bottom:0"><b>Must be implemented.</b></p>
        /// <p style="color:red"><b>SPECIFICATION REVISION - IRotatorV3 - Platform 6.5</b></p>
        /// <para>
        /// Position reports the synced position rather than the mechanical position. The synced position is defined 
        /// as the mechanical position plus an offset. The offset is determined when the <see cref="Sync(float)"/> method is called and must be persisted across driver starts and device reboots.
        /// </para>
        /// <para>
        /// The position is expressed as an angle from 0 up to but not including 360 degrees, counter-clockwise against the
        /// sky. This is the standard definition of Position Angle. However, the rotator does not need to (and in general will not)
        /// report the true Equatorial Position Angle, as the attached imager may not be precisely aligned with the rotator's indexing.
        /// It is up to the client to determine any offset between mechanical rotator position angle and the true Equatorial Position
        /// Angle of the imager, and compensate for any difference.
        /// </para>
        /// <para>
        /// The <see cref="Reverse" /> property is provided in order to manage rotators being used on optics with odd or
        /// even number of reflections. With the Reverse switch in the correct position for the optics, the reported position angle must
        /// be counter-clockwise against the sky.
        /// </para>
        /// <para><b>NOTE</b></para>
        /// <para>IRotatorV3, released in Platform 6.5, and later interface version require this method to be implemented, in IRotatorV2 and earlier interface versions implementation was optional.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotator" version="Platform 5.0">Member added.</revision>
        /// <revision visible="true" date="IRotatorV3" version="Platform 6.5">Implementation is now mandatory, see note above.</revision>
        /// </revisionHistory>
        float Position { get; }

        /// <summary>
        /// Sets or Returns the rotator’s Reverse state.
        /// </summary>
        /// <value>True if the rotation and angular direction must be reversed for the optics</value>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red;margin-bottom:0"><b>Must be implemented.</b></p>
        /// <para>See the definition of <see cref="Position" />.</para>
        /// <para><b>NOTE</b></para>
        /// <para>IRotatorV3, released in Platform 6.5, and later interface version require this method to be implemented, in IRotatorV2 and earlier interface versions implementation was optional.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotator" version="Platform 5.0">Member added.</revision>
        /// <revision visible="true" date="IRotatorV3" version="Platform 6.5">Implementation is now mandatory, see note above.</revision>
        /// </revisionHistory>
        bool Reverse { get; set; }

        /// <summary>
        /// The minimum StepSize, in degrees.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">Throw a PropertyNotImplementedException if the rotator does not know its step size.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Optional - can throw a not implemented exception</b></p>
        /// <para>Raises an exception if the rotator does not intrinsically know what the step size is.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotator" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        float StepSize { get; }

        /// <summary>
        /// The destination position angle for Move() and MoveAbsolute().
        /// </summary>
        /// <value>The destination position angle for<see cref="Move">Move</see> and <see cref="MoveAbsolute">MoveAbsolute</see>.</value>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red;margin-bottom:0"><b>Must be implemented.</b></p>
        /// <para>Upon calling <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see>, this property immediately changes to the position angle to which the rotator is moving. 
        /// The value is retained until a subsequent call to <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see>.</para>
        /// <para><b>NOTE</b></para>
        /// <para>IRotatorV3, released in Platform 6.5, and later interface version require this method to be implemented, in IRotatorV2 and earlier interface versions implementation was optional.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotator" version="Platform 5.0">Member added.</revision>
        /// <revision visible="true" date="IRotatorV3" version="Platform 6.5">Implementation is now mandatory, see note above.</revision>
        /// </revisionHistory>
        float TargetPosition { get; }

        #endregion

        #region IRotatorV2 members

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
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Action(string ActionName, string ActionParameters);

        /// <summary>
        /// Indicates whether the Rotator supports the <see cref="Reverse" /> method.
        /// </summary>
        /// <returns>
        /// True if the Rotator supports the <see cref="Reverse" /> method.
        /// </returns>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red;margin-bottom:0"><b>Must be implemented and must always return True for the IRotatorV3 interface or later.</b></p>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        bool CanReverse { get; }

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
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// <revision visible="true" date="IRotatorV2" version="Platform 7.0">Deprecated, see note above.</revision>
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
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// <revision visible="true" date="IRotatorV2" version="Platform 7.0">Deprecated, see note above.</revision>
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
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// <revision visible="true" date="IRotatorV2" version="Platform 7.0">Deprecated, see note above.</revision>
        /// </revisionHistory>
        string CommandString(string Command, bool Raw = false);

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
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Description { get; }

        /// <summary>
        /// This method is a "clean-up" method that is primarily of use to drivers that are written in languages such as C# and VB.NET where resource clean-up is initially managed by the language's 
        /// runtime garbage collection mechanic. Driver authors should take care to ensure that a client or runtime calling Dispose() does not adversely affect other connected clients.
        /// Applications should not call this method.
        /// </summary>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        void Dispose();

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
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string DriverInfo { get; }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> This must be in the form "n.n".
        /// It should not to be confused with the <see cref="InterfaceVersion" /> property, which is the version of this specification supported by the
        /// driver.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
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
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        short InterfaceVersion { get; }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Name { get; }

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
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        ArrayList SupportedActions { get; }

        #endregion

        #region IRotatorV3 members

        /// <summary>
        /// This returns the raw mechanical position of the rotator in degrees.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented.</b></p>
        /// <p style="color:red"><b>Introduced in IRotatorV3.</b></p>
        /// Returns the mechanical position of the rotator, which is equivalent to the IRotatorV2 <see cref="Position"/> property. Other clients (beyond the one that performed the sync) 
        /// can calculate the current offset using this and the <see cref="Position"/> value.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV3" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        float MechanicalPosition { get; }

        /// <summary>
        /// Moves the rotator to the specified mechanical angle. 
        /// </summary>
        /// <param name="Position">Mechanical rotator position angle.</param>
        /// <exception cref="InvalidValueException">If Position is invalid.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented.</b></p>
        /// <para>This is an asynchronous method that returns as soon as the rotation operation has been successfully started, with the
        /// <see cref="IsMoving"/> property True (unless already at the requested position). After the requested angle is successfully reached and motion stops, 
        /// the <see cref="IsMoving"/> property must become False.</para>
        /// <para>Moves the rotator to the requested mechanical angle, independent of any sync offset that may have been set. This method is to address requirements that need a physical rotation
        /// angle such as taking sky flats.</para>
        /// <para>Client applications should use the <see cref="MoveAbsolute(float)"/> method in preference to this method when imaging.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV3" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        void MoveMechanical(float Position);

        /// <summary>
        /// Syncs the rotator to the specified position angle without moving it. 
        /// </summary>
        /// <param name="Position">Synchronised rotator position angle.</param>
        /// <exception cref="InvalidValueException">If Position is invalid.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented.</b></p>
        /// <p style="color:red"><b>Introduced in IRotatorV3.</b></p>
        /// Once this method has been called and the sync offset determined, both the <see cref="MoveAbsolute(float)"/> method and the <see cref="Position"/> property must function in synced coordinates 
        /// rather than mechanical coordinates. The sync offset must persist across driver starts and device reboots.
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV3" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        void Sync(float Position);

        #endregion

        #region IRotatorV4 members

        /// <summary>
        /// Connect to the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>This is a mandatory method and must not throw a <see cref="MethodNotImplementedException"/>.</b></p></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Connect();

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>This is a mandatory method and must not throw a <see cref="MethodNotImplementedException"/>.</b></p></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Disconnect();

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks><p style="color:red"><b>This is a mandatory property and must not throw a <see cref="PropertyNotImplementedException"/>.</b></p></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV4" version="Platform 7.0">Member added.</revision>
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
        /// <revision visible="true" date="IRotatorV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        IStateValueCollection DeviceState { get; }

        #endregion

    }
}
