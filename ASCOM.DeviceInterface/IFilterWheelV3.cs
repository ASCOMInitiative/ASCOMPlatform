using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Defines the IFilterWheel Interface
    /// </summary>
    [Guid("CCA7DD0D-819B-4CA8-9E9B-7EE0FA8EC14B")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IFilterWheelV3
    {

        #region IFilterWheel members

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.Connected">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheel" version="Platform 5.0">Member added.</revision>
        /// <revision visible="true" date="IFilterWheelV3" version="Platform 7.0">Clients should use the Connect() / Disconnect() mechanic rather than setting Connected TRUE when accessing IFilterWheelV3 or later devices.</revision>
        /// </revisionHistory>
        bool Connected { get; set; }

        /// <summary>
        /// Focus offset of each filter in the wheel
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.FocusOffsets">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheel" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        int[] FocusOffsets { get; }

        /// <summary>
        /// Name of each filter in the wheel
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.Names">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheel" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        string[] Names { get; }

        /// <summary>
        /// Sets or returns the current filter wheel position
        /// </summary>
        /// <exception cref="InvalidValueException">Must throw an InvalidValueException if an invalid position is set</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.&#80;osition">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheel" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
		short Position { get; set; }

        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.SetupDialog">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheel" version="Platform 5.0">Member added.</revision>
        /// </revisionHistory>
        void SetupDialog();

        #endregion

        #region IFilerWheelV2 members

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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.Action">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Action(string ActionName, string ActionParameters);

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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.CommandBlind">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV2" version="Platform 6.0">Member added.</revision>
        /// <revision visible="true" date="IFilterWheelV3" version="Platform 7.0">Deprecated, see note above.</revision>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.CommandBool">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV2" version="Platform 6.0">Member added.</revision>
        /// <revision visible="true" date="IFilterWheelV3" version="Platform 7.0">Deprecated, see note above.</revision>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.CommandString">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV2" version="Platform 6.0">Member added.</revision>
        /// <revision visible="true" date="IFilterWheelV3" version="Platform 7.0">Deprecated, see note above.</revision>
        /// </revisionHistory>
        string CommandString(string Command, bool Raw = false);

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.Description">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Description { get; }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.DriverInfo">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string DriverInfo { get; }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.DriverVersion">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string DriverVersion { get; }

        /// <summary>
        /// This method is a "clean-up" method that is primarily of use to drivers that are written in languages such as C# and VB.NET where resource clean-up is initially managed by the language's 
        /// runtime garbage collection mechanic. Driver authors should take care to ensure that a client or runtime calling Dispose() does not adversely affect other connected clients.
        /// Applications should not call this method.
        /// </summary>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.Dispose">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        void Dispose();

        /// <summary>
        /// The interface version number that this device supports. Should return 3 for this interface version.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.InterfaceVersion">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        short InterfaceVersion { get; }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.Name">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        string Name { get; }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.SupportedActions">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV2" version="Platform 6.0">Member added.</revision>
        /// </revisionHistory>
        ArrayList SupportedActions { get; }

        #endregion

        #region IFilerWheelV3 members

        /// <summary>
        /// Connect to the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.Connect">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Connect();

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.Disconnect">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Disconnect();

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.Connecting">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        bool Connecting { get; }

        /// <summary>
        /// Returns the device's operational state in a single call.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/filterwheel.html#FilterWheel.DeviceState">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IFilterWheelV3" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        IStateValueCollection DeviceState { get; }

        #endregion

    }
}
