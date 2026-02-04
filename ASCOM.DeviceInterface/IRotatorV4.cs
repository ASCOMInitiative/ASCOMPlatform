using System.Collections;
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.Connected">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.SetupDialog">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.Halt">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.IsMoving">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.Move">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.MoveAbsolute">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.Position">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotator" version="Platform 5.0">Member added.</revision>
        /// <revision visible="true" date="IRotatorV3" version="Platform 6.5">Implementation is now mandatory, see note above.</revision>
        /// </revisionHistory>
        float Position { get; }

        /// <summary>
        /// Sets or Returns the rotator's Reverse state.
        /// </summary>
        /// <value>True if the rotation and angular direction must be reversed for the optics</value>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.Reverse">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.StepSize">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.TargetPosition">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.Action">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.CanReverse">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.CommandBlind">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.CommandBool">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.CommandString">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.Description">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Description { get; }

        /// <summary>
        /// This method is a "clean-up" method that is primarily of use to drivers that are written in languages such as C# and VB.NET where resource clean-up is initially managed by the language's 
        /// runtime garbage collection mechanic. Driver authors should take care to ensure that a client or runtime calling Dispose() does not adversely affect other connected clients.
        /// Applications should not call this method.
        /// </summary>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.Dispose">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        void Dispose();

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.DriverInfo">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string DriverInfo { get; }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.DriverVersion">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string DriverVersion { get; }

        /// <summary>
        /// The interface version number that this device supports. Should return 4 for this interface version.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.InterfaceVersion">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        short InterfaceVersion { get; }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.Name">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Name { get; }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.SupportedActions">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.MechanicalPosition">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.MoveMechanical">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.Sync">Canonical Definition</see></remarks>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.Connect">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Connect();

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.Disconnect">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Disconnect();

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.Connecting">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        bool Connecting { get; }

        /// <summary>
        /// Returns the device's operational state in a single call.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/rotator.html#Rotator.DeviceState">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IRotatorV4" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        IStateValueCollection DeviceState { get; }

        #endregion

    }
}
