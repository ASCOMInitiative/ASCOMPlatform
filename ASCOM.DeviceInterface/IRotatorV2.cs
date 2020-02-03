using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    // -----------------------------------------------------------------------
    // <summary>Defines the IRotator Interface</summary>
    // -----------------------------------------------------------------------
    /// <summary>
    /// Defines the IRotator Interface
    /// </summary>
    [Guid("692FA48C-4A30-4543-8681-DA0733758F11")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IRotatorV2 // "49003324-8DE2-4986-BC7D-4D85E1C4CF6B
    {

        // IAscomDriver Methods

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p>Do not use a NotConnectedException here, that exception is for use in other methods that require a connection in order to succeed.
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
        /// <exception cref="NotConnectedException">If the device is not connected and this information is only available when connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
        string Description { get; }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
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
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> This must be in the form "n.n".
        /// It should not to be confused with the <see cref="InterfaceVersion" /> property, which is the version of this specification supported by the
        /// driver.
        /// </remarks>
        string DriverVersion { get; }

        /// <summary>
        /// The interface version number that this device supports. Should return 2 for this interface version.
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> Clients can detect legacy V1 drivers by trying to read ths property.
        /// If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1.
        /// In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver.
        /// </remarks>
        short InterfaceVersion { get; }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
        string Name { get; }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Must be implemented</b></p> </remarks>
        void SetupDialog();

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.</returns>
        /// <exception cref="ASCOM.MethodNotImplementedException">Thrown if no actions are supported.</exception>
        /// <exception cref="ASCOM.ActionNotImplementedException">It is intended that the <see cref="SupportedActions"/> method will inform clients of driver capabilities, but the driver must still throw 
        /// an <see cref="ASCOM.ActionNotImplementedException"/> exception  if it is asked to perform an action that it does not support.</exception>
        /// <exception cref="NotConnectedException">If the driver is not connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful.</exception>
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// <remarks><p style="color:red"><b>Must be implemented.</b></p>
        /// <para>Action names are case insensitive, so SelectWheel, selectwheel and SELECTWHEEL all refer to the same action.</para>
        /// <para>The names of all supported actions must be returned in the <see cref="SupportedActions" /> property.</para>
        /// </remarks>
        string Action(string ActionName, string ActionParameters);

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Must be implemented</b></p>
        /// <para>This method must return an empty <see cref="ArrayList" /> if no actions are supported. Do not throw a <see cref="ASCOM.PropertyNotImplementedException" />.</para>
        /// <para>This is an aid to client authors and testers who would otherwise have to repeatedly poll the driver to determine its capabilities. Returned action names may be in mixed case to
        /// enhance presentation but the <see cref="Action" /> method is case insensitive.</para>
        /// <para>Collections have been used in the Telescope specification for a number of years and are known to be compatible with COM. Within .NET, <see cref="ArrayList" /> is the correct
        /// implementation to use because the .NET Generic methods are not compatible with COM.</para>
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
        /// <exception cref="NotConnectedException">If the driver is not connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
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
        /// <exception cref="NotConnectedException">If the driver is not connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
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
        /// <exception cref="NotConnectedException">If the driver is not connected.</exception>
        /// <exception cref="DriverException">Must throw an exception if the call was not successful</exception>
        /// <remarks><p style="color:red"><b>Can throw a not implemented exception</b></p> </remarks>
        string CommandString(string Command, bool Raw = false);

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        void Dispose();


        /// <summary>
        /// Indicates whether the Rotator supports the <see cref="Reverse" /> method.
        /// </summary>
        /// <returns>
        /// True if the Rotator supports the <see cref="Reverse" /> method.
        /// </returns>
        /// <remarks>
        /// <p style="color:red;margin-bottom:0"><b>Must be implemented and must not throw a PropertyNotImplementedException. </b></p>
        /// </remarks>
        bool CanReverse { get; }

        /// <summary>
        /// Immediately stop any Rotator motion due to a previous <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see> method call.
        /// </summary>
        /// <remarks>This is an optional method. Raises an error if not supported.</remarks>
        /// <exception cref="MethodNotImplementedException">Throw a MethodNotImplementedException if the rotator cannot halt.</exception>
        void Halt();

        /// <summary>
        /// Indicates whether the rotator is currently moving
        /// </summary>
        /// <returns>True if the Rotator is moving to a new position. False if the Rotator is stationary.</returns>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
        /// <remarks>Rotation is asynchronous, that is, when the <see cref="Move">Move</see> method is called, it starts the rotation, then returns
        /// immediately. During rotation, <see cref="IsMoving" /> must be True, else it must be False.</remarks>
        bool IsMoving { get; }

        /// <summary>
        /// Causes the rotator to move Position degrees relative to the current <see cref="Position" /> value.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented.</exception>
        /// <exception cref="InvalidValueException">If Position is invalid.</exception>
        /// <param name="Position">Relative position to move in degrees from current <see cref="Position" />.</param>
        /// <remarks>
        /// Calling <see cref="Move">Move</see> causes the <see cref="TargetPosition" /> property to change to the sum of the current angular position
        /// and the value of the <see cref="Position" /> parameter (modulo 360 degrees), then starts rotation to <see cref="TargetPosition" />.
        /// </remarks>
        void Move(float Position);

        /// <summary>
        /// Causes the rotator to move the absolute position of <see cref="Position" /> degrees.
        /// </summary>
        /// <param name="Position">Absolute position in degrees.</param>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented.</exception>
        /// <exception cref="InvalidValueException">If Position is invalid.</exception>
        /// <remarks>Calling <see cref="MoveAbsolute">MoveAbsolute</see> causes the <see cref="TargetPosition" /> property to change to the value of the
        /// <see cref="Position" /> parameter, then starts rotation to <see cref="TargetPosition" />. </remarks>
        void MoveAbsolute(float Position);

        /// <summary>
        /// Current instantaneous Rotator position, in degrees.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
        /// <remarks>
        /// The position is expressed as an angle from 0 up to but not including 360 degrees, counter-clockwise against the
        /// sky. This is the standard definition of Position Angle. However, the rotator does not need to (and in general will not)
        /// report the true Equatorial Position Angle, as the attached imager may not be precisely aligned with the rotator's indexing.
        /// It is up to the client to determine any offset between mechanical rotator position angle and the true Equatorial Position
        /// Angle of the imager, and compensate for any difference.
        /// <para>The optional <see cref="Reverse" /> property is provided in order to manage rotators being used on optics with odd or
        /// even number of reflections. With the Reverse switch in the correct position for the optics, the reported position angle must
        /// be counter-clockwise against the sky.</para>
        /// </remarks>
        float Position { get; }

        /// <summary>
        /// Sets or Returns the rotator’s Reverse state.
        /// </summary>
        /// <value>True if the rotation and angular direction must be reversed for the optics</value>
        /// <exception cref="PropertyNotImplementedException">Throw a PropertyNotImplementedException if the rotator cannot reverse and CanReverse is False.</exception>
        /// <remarks>See the definition of <see cref="Position" />. Raises an error if not supported. </remarks>
        bool Reverse { get; set; }

        /// <summary>
        /// The minimum StepSize, in degrees.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">Throw a PropertyNotImplementedException if the rotator does not know its step size.</exception>
        /// <remarks>
        /// Raises an exception if the rotator does not intrinsically know what the step size is.
        /// </remarks>
        float StepSize { get; }

        /// <summary>
        /// The destination position angle for Move() and MoveAbsolute().
        /// </summary>
        /// <value>The destination position angle for<see cref="Move">Move</see> and <see cref="MoveAbsolute">MoveAbsolute</see>.</value>
        /// <exception cref="PropertyNotImplementedException">If the property is not implemented.</exception>
        /// <remarks>
        /// Upon calling <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see>, this property immediately changes to the position angle to which the rotator is moving. The value is retained until a subsequent call
        /// to <see cref="Move">Move</see> or <see cref="MoveAbsolute">MoveAbsolute</see>.
        /// </remarks>
        float TargetPosition { get; }
    }
}
