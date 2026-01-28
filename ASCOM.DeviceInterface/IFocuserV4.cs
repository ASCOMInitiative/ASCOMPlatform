using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Xml.Linq;


#if NET35
using ASCOM.Utilities;
#elif NET472
using ASCOM.Utilities;
#endif

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Provides universal access to Focuser drivers - Updated to IFocuserV3 and later - see remarks below
    /// </summary>
    /// <remarks>
    /// <para><b>SPECIFICATION REVISION - Platform 6.4</b></para>
    /// <para>The method signatures in the revised interface specification are identical to the preceding IFocuserV2, however, the IFocuserV3.Move command must
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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Connected">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV2" version="Platform 6.0">Member added.</revision>
        /// <revision visible="true" date="IFocuserV4" version="Platform 7.0">Clients should use the Connect() / Disconnect() mechanic rather than setting Connected TRUE when accessing IFocuserV4 or later devices.</revision>
        /// </revisionHistory>
        bool Connected { get; set; }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Description">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Description { get; }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.DriverInfo">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string DriverInfo { get; }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.DriverVersion">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string DriverVersion { get; }

        /// <summary>
        /// The interface version number that this device supports. Should return 4 for this interface version.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.InterfaceVersion">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        short InterfaceVersion { get; }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Name">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Name { get; }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.SetupDialog">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuser" version="Platform 2.0">Member added.</revision>
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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Action">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Action(string ActionName, string ActionParameters);

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.SupportedActions">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV2" version="Platform 6.0">Member added.</revision>
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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.CommandBlind">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV2" version="Platform 6.0">Member added.</revision>
        /// <revision visible="true" date="IFocuserV4" version="Platform 7.0">Deprecated, see note above.</revision>
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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.CommandBool">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV2" version="Platform 6.0">Member added.</revision>
        /// <revision visible="true" date="IFocuserV4" version="Platform 7.0">Deprecated, see note above.</revision>
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
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.CommandString">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV2" version="Platform 6.0">Member added.</revision>
        /// <revision visible="true" date="IFocuserV4" version="Platform 7.0">Deprecated, see note above.</revision>
        /// </revisionHistory>
        string CommandString(string Command, bool Raw = false);

        /// <summary>
        /// This method is a "clean-up" method that is primarily of use to drivers that are written in languages such as C# and VB.NET where resource clean-up is initially managed by the language's 
        /// runtime garbage collection mechanic. Driver authors should take care to ensure that a client or runtime calling Dispose() does not adversely affect other connected clients.
        /// Applications should not call this method.
        /// </summary>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Dispose">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
		void Dispose();

        /// <summary>
        /// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Absolute">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuser" version="Platform 2.0">Member added.</revision>
        /// </revisionHistory>
        bool Absolute { get; }

        /// <summary>
        /// Immediately stop any focuser motion due to a previous <see cref="Move" /> method call.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">Focuser does not support this method.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Halt">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuser" version="Platform 2.0">Member added.</revision>
        /// </revisionHistory>
        void Halt();

        /// <summary>
        /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.IsMoving">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuser" version="Platform 2.0">Member added.</revision>
        /// </revisionHistory>
        bool IsMoving { get; }

        /// <summary>
        /// State of the connection to the focuser.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Link">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuser" version="Platform 2.0">Member added.</revision>
        /// <revision visible="true" date="IFocuserV2" version="Platform 6.0">Member deprecated in favour of Connected, see above.</revision>
        /// </revisionHistory>
        bool Link { get; set; }

        /// <summary>
        /// Maximum increment size allowed by the focuser;
        /// i.e. the maximum number of steps allowed in one move operation.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.MaxIncrement">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuser" version="Platform 2.0">Member added.</revision>
        /// </revisionHistory>
        int MaxIncrement { get; }

        /// <summary>
        /// Maximum step position permitted.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.MaxStep">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuser" version="Platform 2.0">Member added.</revision>
        /// </revisionHistory>
        int MaxStep { get; }

        /// <summary>
        /// Moves the focuser by the specified amount or to the specified position depending on the value of the <see cref="Absolute" /> property.
        /// </summary>
        /// <param name="Position">Step distance or absolute position, depending on the value of the <see cref="Absolute" /> property.</param>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Move">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuser" version="Platform 2.0">Member added.</revision>
        /// <revision visible="true" date="IFocuserV3" version="Platform 6.4">Move must not throw an exception when <see cref="TempComp"/> is <see langword="true"/>, see above.</revision>
        /// </revisionHistory>
        void Move(int Position);

        /// <summary>
        /// Current focuser position, in steps.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">Raises a PropertyNotImplemented if the focuser does not intrinsically know what the position is.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Position">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuser" version="Platform 2.0">Member added.</revision>
        /// </revisionHistory>
        int Position { get; }

        /// <summary>
        /// Step size (microns) for the focuser.
        /// </summary>
        /// <exception cref= "PropertyNotImplementedException">If the focuser does not intrinsically know what the step size is.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.StepSize">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuser" version="Platform 2.0">Member added.</revision>
        /// </revisionHistory>
        double StepSize { get; }

        /// <summary>
        /// The state of temperature compensation mode (if available), else always False.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If <see cref="TempCompAvailable" /> is False and an attempt is made to set <see cref="TempComp" /> to true.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.TempComp">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuser" version="Platform 2.0">Member added.</revision>
        /// </revisionHistory>
        bool TempComp { get; set; }

        /// <summary>
        /// True if focuser has temperature compensation available.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.TempCompAvailable">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuser" version="Platform 2.0">Member added.</revision>
        /// </revisionHistory>
        bool TempCompAvailable { get; }

        /// <summary>
        /// Current ambient temperature in degrees Celsius as measured by the focuser.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">If the property is not available for this device.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Temperature">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuser" version="Platform 2.0">Member added.</revision>
        /// </revisionHistory>
        double Temperature { get; }

        #endregion

        #region IFocuserV4 members

        /// <summary>
        /// Connect to the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Connect">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV4" version="Platform 7">Member added.</revision>
        /// </revisionHistory>
        void Connect();

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Disconnect">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV4" version="Platform 7">Member added.</revision>
        /// </revisionHistory>
        void Disconnect();

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.Connecting">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV4" version="Platform 7">Member added.</revision>
        /// </revisionHistory>
        bool Connecting { get; }

        /// <summary>
        /// Returns the device's operational state in a single call.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information: <see href="https://ascom-standards.org/newdocs/focuser.html#Focuser.DeviceState">Canonical definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFocuserV4" version="Platform 7">Member added.</revision>
        /// </revisionHistory>
        IStateValueCollection DeviceState { get; }

        #endregion

    }
}
