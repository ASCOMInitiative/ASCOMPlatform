using System.Collections;
using System;
using System.Runtime.InteropServices;
using ASCOM.Utilities;

namespace ASCOM.DeviceInterface
{

    /// <summary>
    /// Provides universal access to Focuser drivers - Updated to IFocuserV3 - see remarks below
    /// </summary>
    /// <remarks>
    /// <para><b>SPECIFICATION REVISION - Platform 6.4</b></para>
    /// <para>The method signatures in the revised interface specification are identical to the preceeding IFocuserV2, however, the IFocuserV3.Move command must
    /// no longer throw an InvalidOperationException exception if a Move is attempted when temperature compensation is enabled.</para>
    /// </remarks>
    [Guid("4F93D41E-4139-499D-8804-8F2C1A6B9FC0")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IFocuserV4
    {

        #region IFocuserV3 members

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
        /// <para>The Connected property is not implemented in Version 1 drivers; these use the <see cref="Link"></see>
        /// property and will raise a Not Implemented exception for this property. Version 2 drivers must implement both Connected and Link.
        /// Applications should check that InterfaceVersion returns 2 or more before using Connected.</para>
        /// </remarks>
        bool Connected { get; set; }

		/// <summary>
		/// Returns a description of the device, such as manufacturer and modelnumber. Any ASCII characters may be used.
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
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented</b></p> This must be in the form "n.n".
		/// It should not to be confused with the <see cref="InterfaceVersion" /> property, which is the version of this specification supported by the
		/// driver.
		/// </remarks>
		string DriverVersion { get; }

		/// <summary>
		/// The interface version number that this device supports. Should return 3 for this interface version.
		/// </summary>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented</b></p> Clients can detect legacy V1 drivers by trying to read ths property.
		/// If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1.
		/// In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver.
		/// </remarks>
		short InterfaceVersion { get; }

		/// <summary>
		/// The short name of the driver, for display purposes
		/// </summary>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented</b></p>
		/// </remarks>
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
        /// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>
        /// <p style="color:red"><b>Must be implemented</b></p>
        /// </remarks>
        bool Absolute { get; }

		/// <summary>
		/// Immediately stop any focuser motion due to a previous <see cref="Move" /> method call.
		/// </summary>
		/// <exception cref="MethodNotImplementedException">Focuser does not support this method.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Can throw a not implemented exception</b></p>Some focusers may not support this function, in which case an exception will be raised.
		/// <para><b>Recommendation:</b> Host software should call this method upon initialization and,
		/// if it fails, disable the Halt button in the user interface.</para>
		/// </remarks>
		void Halt();

		/// <summary>
		/// True if the focuser is currently moving to a new position. False if the focuser is stationary.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented</b></p>
		/// </remarks>
		bool IsMoving { get; }

		/// <summary>
		/// State of the connection to the focuser.
		/// </summary>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented</b></p> Set True to start the connection to the focuser; set False to terminate the connection.
		/// The current connection status can also be read back through this property.
		/// An exception will be raised if the link fails to change state for any reason.
		/// <para><b>Note</b></para>
		/// <para>The FocuserV1 interface was the only interface to name its <i>"Connection"</i> p[roperty "Link" all others named
		/// their <i>"Connection"</i> property as "Connected". All interfaces including Focuser now have a <see cref="Connected"></see> method and this is
		/// the recommended method to use to <i>"Connect"</i> to Focusers exposing the V2 and later interfaces.</para>
		/// <para>Do not use a NotConnectedException here, that exception is for use in other methods that require a connection in order to succeed.</para>
		/// </remarks>
		bool Link { get; set; }

		/// <summary>
		/// Maximum increment size allowed by the focuser;
		/// i.e. the maximum number of steps allowed in one move operation.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented</b></p>
		/// For most focusers this is the same as the <see cref="MaxStep" /> property. This is normally used to limit the Increment display in the host software.
		/// </remarks>
		int MaxIncrement { get; }

		/// <summary>
		/// Maximum step position permitted.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented</b></p>
		/// The focuser can step between 0 and <see cref="MaxStep" />. If an attempt is made to move the focuser beyond these limits, it will automatically stop at the limit.
		/// </remarks>
		int MaxStep { get; }

		/// <summary>
		/// Moves the focuser by the specified amount or to the specified position depending on the value of the <see cref="Absolute" /> property.
		/// </summary>
		/// <param name="Position">Step distance or absolute position, depending on the value of the <see cref="Absolute" /> property.</param>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented</b></p>
		/// <para>If the <see cref="Absolute" /> property is True, then this is an absolute positioning focuser. The <see cref="Move">Move</see> command tells the focuser to move to an exact step position, and the Position parameter
		/// of the <see cref="Move">Move</see> method is an integer between 0 and <see cref="MaxStep" />.</para>
		/// <para>If the <see cref="Absolute" /> property is False, then this is a relative positioning focuser. The <see cref="Move">Move</see> command tells the focuser to move in a relative direction, and the Position parameter
		/// of the <see cref="Move">Move</see> method (in this case, step distance) is an integer between minus <see cref="MaxIncrement" /> and plus <see cref="MaxIncrement" />.</para>
		/// <para><b>BEHAVIOURAL CHANGE - Platform 6.4</b></para>
		/// <para>Prior to Platform 6.4, the interface specification mandated that drivers must throw an <see cref="InvalidOperationException"/> if a move was attempted when <see cref="TempComp"/> was True, even if the focuser
		/// was able to execute the move safely without disrupting temperature compensation.</para>
		/// <para>Following discussion on ASCOM-Talk in January 2018, the Focuser interface specification has been revised to IFocuserV3, removing the requrement to throw the InvalidOperationException exception. IFocuserV3 compliant drivers
		/// are expected to execute Move requests when temperature compensation is active and to hide any specific actions required by the hardware from the client. For example this could be achieved by disabling temperature compensation, moving the focuser and re-enabling
		/// temperature compensation or simply by moving the focuser with compensation enabled if the hardware supports this.</para>
		/// <para>Conform will continue to pass IFocuserV2 drivers that throw InvalidOperationException exceptions. However, Conform will now fail IFocuserV3 drivers that throw InvalidOperationException exceptions, in line with this revised specification.</para>
		/// </remarks>
		void Move(int Position);

		/// <summary>
		/// Current focuser position, in steps.
		/// </summary>
		/// <exception cref="PropertyNotImplementedException">Raises a PropertyNotImplemented if the focuser does not intrinsically know what the position is.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Can throw a not implemented exception</b></p> Valid only for absolute positioning focusers (see the <see cref="Absolute" /> property).
		/// A <see cref="PropertyNotImplementedException">PropertyNotImplementedException</see> exception must be thrown if this device is a relative positioning focuser rather than an absolute position focuser.
		/// </remarks>
		int Position { get; }

		/// <summary>
		/// Step size (microns) for the focuser.
		/// </summary>
		/// <exception cref= "PropertyNotImplementedException">If the focuser does not intrinsically know what the step size is.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Can throw a not implemented exception</b></p> Must throw an exception if the focuser does not intrinsically know what the step size is.
		/// </remarks>
		double StepSize { get; }

		/// <summary>
		/// The state of temperature compensation mode (if available), else always False.
		/// </summary>
		/// <exception cref="PropertyNotImplementedException">If <see cref="TempCompAvailable" /> is False and an attempt is made to set <see cref="TempComp" /> to true.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
		/// <remarks>
		/// <p style="color:red;margin-bottom:0"><b>TempComp Read must be implemented and must not throw a PropertyNotImplementedException. </b></p>
		/// <p style="color:red;margin-top:0"><b>TempComp Write can throw a PropertyNotImplementedException.</b></p>
		/// If the <see cref="TempCompAvailable" /> property is True, then setting <see cref="TempComp" /> to True puts the focuser into temperature tracking mode; setting it to False will turn off temperature tracking.
		/// <para>If temperature compensation is not available, this property must always return False.</para>
		/// <para> A <see cref="PropertyNotImplementedException" /> exception must be thrown if <see cref="TempCompAvailable" /> is False and an attempt is made to set <see cref="TempComp" /> to true.</para>
		/// <para><b>BEHAVIOURAL CHANGE - Platform 6.4</b></para>
		/// <para>Prior to Platform 6.4, the interface specification mandated that drivers must throw an <see cref="InvalidOperationException"/> if a move was attempted when <see cref="TempComp"/> was True, even if the focuser
		/// was able to execute the move safely without disrupting temperature compensation.</para>
		/// <para>Following discussion on ASCOM-Talk in January 2018, the Focuser interface specification has been revised to IFocuserV3, removing the requrement to throw the InvalidOperationException exception. IFocuserV3 compliant drivers
		/// are expected to execute Move requests when temperature compensation is active and to hide any specific actions required by the hardware from the client. For example this could be achieved by disabling temperature compensation, moving the focuser and re-enabling
		/// temperature compensation or simply by moving the focuser with compensation enabled if the hardware supports this.</para>
		/// <para>Conform will continue to pass IFocuserV2 drivers that throw InvalidOperationException exceptions. However, Conform will now fail IFocuserV3 drivers that throw InvalidOperationException exceptions, in line with this revised specification.</para>
		/// </remarks>
		bool TempComp { get; set; }

		/// <summary>
		/// True if focuser has temperature compensation available.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Must be implemented</b></p>
		/// Will be True only if the focuser's temperature compensation can be turned on and off via the <see cref="TempComp" /> property.
		/// </remarks>
		bool TempCompAvailable { get; }

		/// <summary>
		/// Current ambient temperature in degrees Celsius as measured by the focuser.
		/// </summary>
		/// <exception cref="PropertyNotImplementedException">If the property is not available for this device.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
		/// <remarks>
		/// <p style="color:red"><b>Can throw a not implemented exception</b></p>
		/// <para>Raises an exception if ambient temperature is not available. Commonly available on focusers with a built-in temperature compensation mode.</para>
		/// <para><b>Clarification - October 2019</b></para>
		/// <para>Historically no units were specified for this property. Henceforth, if applications need to process the supplied temperature, they should proceed on the basis that the
		/// units are degrees Celsius for consistency with <see cref="IObservingConditions.Temperature"/>. Conversion to other temperature units can be achieved through the <see cref="Util.ConvertUnits"/> utility method.</para>
		/// </remarks>
		double Temperature { get; }

        #endregion

        #region IFocuserV4 members

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
        /// </remarks>
        ArrayList DeviceState { get; }

        #endregion

    }
}
