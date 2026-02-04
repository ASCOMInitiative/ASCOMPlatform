using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// Interface for devices that support one or both of two independent capabilities: Telescope Covers and Flat Field Calibrators.
    /// </summary>
    /// <remarks>
    /// <para>A device indicates whether it supports each capability through the CoverState and CalibratorState properties, which
    /// must return CoverStatus.<see cref="CoverStatus.NotPresent"/> or CalibratorStatus.<see cref="CalibratorStatus.NotPresent"/> as appropriate if the device does not implement that capability.</para>
    /// <para>This interface enables clients to control the cover and calibrator states to configure the device to take images, calibration light frames and, for shutterless cameras, 
    /// calibration dark/bias frames.</para>
    /// </remarks>
    [ComVisible(true)]
    [Guid("6A4677FF-4A0D-412A-88D5-2B7179306377")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ICoverCalibratorV2
    {
        #region Common Methods

        /// <summary>
        /// Set True to connect to the device hardware. Set False to disconnect from the device hardware.
        /// You can also read the property to check whether it is connected. This reports the current hardware state.
        /// </summary>
        /// <value><c>true</c> if connected to the hardware; otherwise, <c>false</c>.</value>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.Connected">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// <revision visible="true" date="ICoverCalibratorV2" version="Platform 7.0">Clients should use the Connect() / Disconnect() mechanic rather than setting Connected TRUE when accessing ICoverCalibratorV2 or later devices.</revision>
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
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        string Description { get; }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.DriverInfo">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        string DriverInfo { get; }

        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.DriverVersion">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        string DriverVersion { get; }

        /// <summary>
        /// The interface version number that this device supports. Should return 2 for this interface version.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.InterfaceVersion">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        short InterfaceVersion { get; }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.Name">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        string Name { get; }

        /// <summary>
        /// Launches a configuration dialogue box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.SetupDialog">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        void SetupDialog();

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
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
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        string Action(string ActionName, string ActionParameters);

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.SupportedActions">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
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
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// <revision visible="true" date="ICoverCalibratorV2" version="Platform 7.0">Deprecated, see note above.</revision>
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
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// <revision visible="true" date="ICoverCalibratorV2" version="Platform 7.0">Deprecated, see note above.</revision>
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
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// <revision visible="true" date="ICoverCalibratorV2" version="Platform 7.0">Deprecated, see note above.</revision>
        /// </revisionHistory>
        string CommandString(string Command, bool Raw = false);

        /// <summary>
        /// This method is a "clean-up" method that is primarily of use to drivers that are written in languages such as C# and VB.NET where resource clean-up is initially managed by the language's 
        /// runtime garbage collection mechanic. Driver authors should take care to ensure that a client or runtime calling Dispose() does not adversely affect other connected clients.
        /// Applications should not call this method.
        /// </summary>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.Dispose">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        void Dispose();

        #endregion

        #region ICoverCalibratorV1 members

        /// <summary>
        /// Returns the state of the device cover, if present, otherwise returns "NotPresent"
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.CoverState">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        CoverStatus CoverState { get; }

        /// <summary>
        /// Initiates cover opening if a cover is present
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="CoverState"/> returns <see cref="CoverStatus.NotPresent"/>.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.OpenCover">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        void OpenCover();

        /// <summary>
        /// Initiates cover closing if a cover is present
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="CoverState"/> returns <see cref="CoverStatus.NotPresent"/>.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.CloseCover">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        void CloseCover();

        /// <summary>
        /// Stops any cover movement that may be in progress if a cover is present and cover movement can be interrupted.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="CoverState"/> returns <see cref="CoverStatus.NotPresent"/> or if cover movement cannot be interrupted.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.HaltCover">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        void HaltCover();

        /// <summary>
        /// Returns the state of the calibration device, if present, otherwise returns "NotPresent"
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.CalibratorState">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        CalibratorStatus CalibratorState { get; }

        /// <summary>
        /// Returns the current calibrator brightness in the range 0 (completely off) to <see cref="MaxBrightness"/> (fully on)
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When <see cref="CalibratorState"/> returns <see cref="CalibratorStatus.NotPresent"/>.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.Brightness">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        int Brightness { get; }

        /// <summary>
        /// The Brightness value that makes the calibrator deliver its maximum illumination.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When <see cref="CalibratorState"/> returns <see cref="CalibratorStatus.NotPresent"/>.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.MaxBrightness">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        int MaxBrightness { get; }

        /// <summary>
        /// Turns the calibrator on at the specified brightness if the device has calibration capability
        /// </summary>
        /// <param name="Brightness">Sets the required calibrator illumination brightness in the range 0 (fully off) to <see cref="MaxBrightness"/> (fully on).</param>
        /// <exception cref="InvalidValueException">When the supplied brightness parameter is outside the range 0 to <see cref="MaxBrightness"/>.</exception>
        /// <exception cref="MethodNotImplementedException">When <see cref="CalibratorState"/> returns <see cref="CalibratorStatus.NotPresent"/>.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.CalibratorOn">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        void CalibratorOn(int Brightness);

        /// <summary>
        /// Turns the calibrator off if the device has calibration capability
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="CalibratorState"/> returns <see cref="CalibratorStatus.NotPresent"/>.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception>         
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.CalibratorOff">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV1" version="Platform 6.5">Member added.</revision>
        /// </revisionHistory>
        void CalibratorOff();

        #endregion

        #region ICoverCalibratorV2 members

        /// <summary>
        /// Connect to the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.Connect">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV2" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Connect();

        /// <summary>
        /// Disconnect from the device asynchronously
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.Disconnect">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV2" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        void Disconnect();

        /// <summary>
        /// Returns True while the device is undertaking an asynchronous connect or disconnect operation.
        /// </summary>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.Connecting">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV2" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        bool Connecting { get; }

        /// <summary>
        /// Returns the device's operational state in a single call.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.DeviceState">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV2" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        IStateValueCollection DeviceState { get; }

        /// <summary>
        /// Flag showing whether a calibrator brightness state change is in progress. 
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <returns>
        /// True while the calibrator brightness is not stable following a <see cref="CalibratorOn(int)"/> or <see cref="CalibratorOff"/> command.
        /// </returns>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.CalibratorChanging">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV2" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        bool CalibratorChanging { get; }

        /// <summary>
        /// Flag showing whether the cover is moving. 
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <returns>
        /// True while the cover is in motion following an <see cref="OpenCover"/> or <see cref="CloseCover"/> command.
        /// </returns>
        /// <remarks>See this link for the canonical definition, which may include further information and implementation requirements: <see href="https://ascom-standards.org/newdocs/covercalibrator.html#CoverCalibrator.CoverMoving">Canonical Definition</see></remarks>
        /// <revisionHistory visible="true">
        /// <revision visible="true" date="ICoverCalibratorV2" version="Platform 7.0">Member added.</revision>
        /// </revisionHistory>
        bool CoverMoving { get; }

        #endregion

    }
}