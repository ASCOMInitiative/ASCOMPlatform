using ASCOM;
using ASCOM.DeviceInterface;
using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Provides support for polar axis aligners and optical axis aligners that measure movement in degrees, millimeters and counts. 
    /// </summary>
    /// <remarks>
    /// <para>The interface is intentionally flexible to support up to three degrees of freedom and allow for future aligner types.</para>
    /// <para>In addition, all position related properties are optional to provide support for a wide range of devices from those that only have relative movement and no knowledge of their position 
    /// to those that know exactly where they are and can move to any specified coordinates within their supported range.</para>
    /// </remarks>
    [ComVisible(true)]
    [Guid("95A06551-E8FF-4B4A-9B06-6A039C54778B")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IAlignerV2
    {
        #region Common Methods

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.Connected">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        bool Connected { get; set; }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.Description">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        string Description { get; }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.DriverInfo">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        string DriverInfo { get; }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.DriverVersion">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        string DriverVersion { get; }

        /// <summary>
        /// The interface version number that this device supports. Should return 1 for this interface version.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.InterfaceVersion">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        short InterfaceVersion { get; }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.Name">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        string Name { get; }

        /// <summary>
        /// Launches a configuration dialogue box for the driver (COM only).  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.SetupDialog">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        void SetupDialog();

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="string.Empty">Empty String</see> if none are required.</param>
        /// <returns>
        /// <para>A string response. The meaning of returned strings is set by the driver author.</para>
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        /// <exception cref="MethodNotImplementedException">Thrown if no actions are supported.</exception>
        /// <exception cref="ActionNotImplementedException">It is intended that the <see cref="SupportedActions"/> method will inform clients of driver capabilities, but the driver must still throw 
        /// an <see cref="ASCOM.ActionNotImplementedException"/> exception  if it is asked to perform an action that it does not support.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.Action">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        string Action(string ActionName, string ActionParameters);

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.SupportedActions">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.CommandBlind">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.CommandBool">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
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
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.CommandString">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        string CommandString(string Command, bool Raw = false);

        /// <summary>
        /// This method is a "clean-up" method that is primarily of use to drivers that are written in languages such as C# and VB.NET where resource clean-up is initially managed by the language's 
        /// runtime garbage collection mechanic. Driver authors should take care to ensure that a client or runtime calling Dispose() does not adversely affect other connected clients.
        /// Applications should not call this method.
        /// </summary>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.Dispose">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        void Dispose();

        #endregion

        #region Device Properties

        /// <summary>
        /// Returns a configuration object that implements <see cref="IAlignerConfigurationV1"/> and contains all configuration values for the device.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// <para><b>TRIAL - </b >Historically we received feedback that retrieving many individual configuration values is inefficient; this new approach is an attempt to address that feedback. Instead of individual properties for each configuration value, the <see cref="Configuration"/> property 
        /// returns an object that contains all configuration values. In the case of the <see cref="IAlignerV1"/> interface it reduces eighteen individual calls to one.</para>
        /// <para><b>NOTE - </b>The common properties such as <see cref="DriverInfo"/> are intentionally omitted from this property to maintain consistency with other device types.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        IAlignerConfigurationV2 Configuration { get; }

        /// <summary>
        /// Returns the configuration object's interface version.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// <para>Must be 1 in <see cref="IAlignerV1"/>.</para>
        /// <para>This is independent of the overall device interface version. E.g. The device interface version 2 may could retain the original configuration version 1 used in device interface version 1.</para>
        /// </remarks>
        int ConfigurationVersion { get; }

        /// <summary>
        /// Returns the device's operational state in a single call.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <remarks>
        /// <para>As in other device types, DeviceState only includes operational properties that may change during observation: IsMoving, XPosition, YPosition, ZPosition. Other properties are configuration related and will not change once operation has commenced.</para>
        /// <para>Please note that this is a "best endeavours" call. Values are simply omitted from the response when the device cannot determine their value.</para>
        /// <para>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#AxisAligner.DeviceState">Canonical Definition</see></para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        IStateValueCollection DeviceState { get; }

        /// <summary>
        /// True while any move or homing is in progress.
        /// </summary>
        /// <exception cref="NotConnectedException">When the device is not connected.</exception>
        /// <exception cref="DriverException">When a driver error occurs.</exception>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// <para>This is the completion property for the <see cref="Move"/>, <see cref="MoveAbsolute"/> and <see cref="FindHome"/> methods.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        bool IsMoving { get; }

        /// <summary>
        /// Returns the position of the X axis from the device's zero in <see cref="AlignerUnits"/>.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not track its position or can not control that axis.</exception>
        /// <exception cref="NotConnectedException">When the device is not connected.</exception>
        /// <exception cref="DriverException">When a driver error occurs.</exception>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        double XPosition { get; }

        /// <summary>
        /// Returns the position of the Y axis from the device's zero in <see cref="AlignerUnits"/>.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not track its position or can not control that axis.</exception>
        /// <exception cref="NotConnectedException">When the device is not connected.</exception>
        /// <exception cref="DriverException">When a driver error occurs.</exception>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        double YPosition { get; }

        /// <summary>
        /// Returns the position of the Z axis from the device's zero in <see cref="AlignerUnits"/>.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When the device does not track its position or can not control that axis.</exception>
        /// <exception cref="NotConnectedException">When the device is not connected.</exception>
        /// <exception cref="DriverException">When a driver error occurs.</exception>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        double ZPosition { get; }

        #endregion

        #region Device methods

        /// <summary>
        /// Starts movement of all axes to their home positions.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="IAlignerConfigurationV1.CanFindHome"/> is false.</exception>
        /// <exception cref="NotConnectedException">When the device is not connected.</exception>
        /// <exception cref="DriverException">When a driver error occurs.</exception>
        /// <remarks>
        /// <para>Operates asynchronously, setting IsMoving true before returning. On completion IsMoving becomes false. A device that cannot move axes concurrently moves them one after another within the same operation.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        void FindHome();

        /// <summary>
        /// Halts movement on all axes.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="IAlignerConfigurationV1.CanHalt"/> is false.</exception>
        /// <exception cref="NotConnectedException">When the device is not connected.</exception>
        /// <exception cref="DriverException">When a driver error occurs.</exception>
        /// <remarks><para>Operates synchronously, returning when all motion has stopped. IsMoving must be False on return. Calling <see cref="Halt"/> while idle does nothing and does not throw.</para></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        void Halt();

        /// <summary>
        /// Starts a relative move of every axis by the given amounts in <see cref="AlignerUnits"/> units.
        /// </summary>
        /// <param name="X">The amount to move the X axis in <see cref="AlignerUnits"/> units. Use 0.0 if no movement in this direction is desired.</param>
        /// <param name="Y">The amount to move the Y axis in <see cref="AlignerUnits"/> units. Use 0.0 if no movement in this direction is desired.</param>
        /// <param name="Z">The amount to move the Z axis in <see cref="AlignerUnits"/> units. Use 0.0 if no movement in this direction is desired.</param>
        /// <exception cref="InvalidValueException">When any of the requested movements cannot be achieved.</exception>
        /// <exception cref="NotConnectedException">When the device is not connected.</exception>
        /// <exception cref="DriverException">When a driver error occurs.</exception>
        /// <remarks>
        /// <para><b style="color:red">Must be implemented</b> Cannot throw a <see cref="MethodNotImplementedException"/>.</para>
        /// <para>Operates asynchronously, setting IsMoving true before returning. On completion IsMoving becomes false. A device that cannot move axes concurrently moves them one after another within the same operation.</para>
        /// </remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        void Move(double X, double Y, double Z);

        /// <summary>
        /// Starts a move of every axis to the given positions in <see cref="AlignerUnits"/> units.
        /// </summary>
        /// <param name="X">The amount to move the X axis in <see cref="AlignerUnits"/> units. Use 0.0 if no movement in this direction is desired.</param>
        /// <param name="Y">The amount to move the Y axis in <see cref="AlignerUnits"/> units. Use 0.0 if no movement in this direction is desired.</param>
        /// <param name="Z">The amount to move the Z axis in <see cref="AlignerUnits"/> units. Use 0.0 if no movement in this direction is desired.</param>
        /// <exception cref="MethodNotImplementedException">When <see cref="IAlignerConfigurationV1.CanMoveAbsolute"/> is false.</exception>
        /// <exception cref="InvalidValueException">When any of the requested positions cannot be reached.</exception>
        /// <exception cref="NotConnectedException">When the device is not connected.</exception>
        /// <exception cref="DriverException">When a driver error occurs.</exception>
        /// <para>Operates asynchronously, setting IsMoving true before returning. On completion IsMoving becomes false. A device that cannot move axes concurrently moves them one after another within the same operation.</para>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        void MoveAbsolute(double X, double Y, double Z);

        /// <summary>
        /// Sets the device coordinates to the provided values in <see cref="AlignerUnits"/> without moving the device.
        /// </summary>
        /// <param name="X">The X axis value of the current device position in <see cref="AlignerUnits"/> units.</param>
        /// <param name="Y">The Y axis value of the current device position in <see cref="AlignerUnits"/> units.</param>
        /// <param name="Z">The Z axis value of the current device position in <see cref="AlignerUnits"/> units.</param>
        /// <exception cref="MethodNotImplementedException">When <see cref="IAlignerConfigurationV1.CanSync"/> is false.</exception>
        /// <exception cref="InvalidValueException">When any of the requested coordinates cannot be set.</exception>
        /// <exception cref="NotConnectedException">When the device is not connected.</exception>
        /// <exception cref="DriverException">When a driver error occurs.</exception>
        /// <remarks><para>Operates synchronously. IsMoving must be False on return.</para></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="IAlignerV1" version="Platform 7.4">Member added.</revision>
        /// </revisionHistory>
        void Sync(double X, double Y, double Z);

        #endregion
    }
}
