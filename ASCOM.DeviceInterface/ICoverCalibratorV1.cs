using System;
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
    [Guid("879A2D28-3659-457A-B5E8-5CF7262975EB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ICoverCalibratorV1
    {
        /// <summary>
        /// Returns the state of the device cover, if present, otherwise returns "NotPresent"
        /// </summary>
        /// <remarks>
        /// <para>This is a mandatory property that must return a value, it must not throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// <para>The <see cref="CoverStatus.Unknown"/> state must only be returned if the device is unaware of the cover's state e.g. if the hardware does not report the open / closed state and the cover has just been powered on.
        /// Clients do not need to take special action if this state is returned, they must carry on as usual, issuing  <see cref="OpenCover"/> or <see cref="CloseCover"/> commands as required.</para>
        /// <para>If the cover hardware cannot report its state, the device could mimic this by recording the last configured state and returning this. Driver authors or device manufacturers may also wish to offer users
        /// the capability of powering up in a known state e.g. Open or Closed and driving the hardware to this state when Connected is set <see langword="true"/>.</para>
        /// </remarks>
        CoverStatus CoverState { get; }

        /// <summary>
        /// Initiates cover opening if a cover is present
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="CoverState"/> returns <see cref="CoverStatus.NotPresent"/>.</exception>
        /// <remarks>
        /// <para>While the cover is opening <see cref="CoverState"/> must return <see cref="CoverStatus.Moving"/>.</para>
        /// <para>When the cover is open <see cref="CoverState"/> must return <see cref="CoverStatus.Open"/>.</para>
        /// <para>If an error condition arises while moving between states, <see cref="CoverState"/> must be set to <see cref="CoverStatus.Error"/> rather than <see cref="CoverStatus.Unknown"/>.</para>
        /// </remarks>
        void OpenCover();

        /// <summary>
        /// Initiates cover closing if a cover is present
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="CoverState"/> returns <see cref="CoverStatus.NotPresent"/>.</exception>
        /// <remarks>
        /// <para>While the cover is closing <see cref="CoverState"/> must return <see cref="CoverStatus.Moving"/>.</para>
        /// <para>When the cover is closed <see cref="CoverState"/> must return <see cref="CoverStatus.Closed"/>.</para>
        /// <para>If an error condition arises while moving between states, <see cref="CoverState"/> must be set to <see cref="CoverStatus.Error"/> rather than <see cref="CoverStatus.Unknown"/>.</para>
        /// </remarks>
        void CloseCover();

        /// <summary>
        /// Stops any cover movement that may be in progress if a cover is present and cover movement can be interrupted.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="CoverState"/> returns <see cref="CoverStatus.NotPresent"/> or if cover movement cannot be interrupted.</exception>
        /// <remarks>
        /// <para>This must stop any cover movement as soon as possible and set a <see cref="CoverState"/> of <see cref="CoverStatus.Open"/>, <see cref="CoverStatus.Closed"/> 
        /// or <see cref="CoverStatus.Unknown"/> as appropriate.</para>
        /// <para>If cover movement cannot be interrupted, a <see cref="MethodNotImplementedException"/> must be thrown.</para>
        /// </remarks>
        void HaltCover();

        /// <summary>
        /// Returns the state of the calibration device, if present, otherwise returns "NotPresent"
        /// </summary>
        /// <remarks>
        /// <para>This is a mandatory property that must return a value, it must not throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// <para>The <see cref="CalibratorStatus.Unknown"/> state must only be returned if the device is unaware of the calibrator's state e.g. if the hardware does not report the device's state and 
        /// the calibrator has just been powered on. Clients do not need to take special action if this state is returned, they must carry on as usual, issuing <see cref="CalibratorOn(int)"/> and 
        /// <see cref="CalibratorOff"/> commands as required.</para>
        /// <para>If the calibrator hardware cannot report its state, the device could mimic this by recording the last configured state and returning this. Driver authors or device manufacturers may also wish to offer users
        /// the capability of powering up in a known state and driving the hardware to this state when Connected is set <see langword="true"/>.</para>
        /// </remarks>
        CalibratorStatus CalibratorState { get; }

        /// <summary>
        /// Returns the current calibrator brightness in the range 0 (completely off) to <see cref="MaxBrightness"/> (fully on)
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When <see cref="CalibratorState"/> returns <see cref="CalibratorStatus.NotPresent"/>.</exception>
        /// <remarks>
        /// <para>This is a mandatory property for a calibrator device </para>
        /// <para>The brightness value must be 0 when the <see cref="CalibratorState"/> is <see cref="CalibratorStatus.Off"/></para>
        /// </remarks>
        int Brightness { get; }

        /// <summary>
        /// The Brightness value that makes the calibrator deliver its maximum illumination.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When <see cref="CalibratorState"/> returns <see cref="CalibratorStatus.NotPresent"/>.</exception>
        /// <remarks>
        /// <para>This is a mandatory property for a calibrator device and must be within the integer range 1 to 2,147,483,647</para>
        /// <para>A value of 1 indicates that the calibrator can only be "off" or "on".</para>
        /// <para>A value of 10 indicates that the calibrator has 10 discreet illumination levels in addition to "off".</para>
        /// <para>The value for this parameter should be determined by the driver author or device manufacturer based on the capabilities of the hardware used in the calibrator.</para>
        /// </remarks>
        int MaxBrightness { get; }

        /// <summary>
        /// Turns the calibrator on at the specified brightness if the device has calibration capability
        /// </summary>
        /// <param name="Brightness">Sets the required calibrator illumination brightness in the range 0 (fully off) to <see cref="MaxBrightness"/> (fully on).</param>
        /// <exception cref="MethodNotImplementedException">When <see cref="CalibratorState"/> returns <see cref="CalibratorStatus.NotPresent"/>.</exception>
        /// <exception cref="InvalidValueException">When the supplied brightness parameter is outside the range 0 to <see cref="MaxBrightness"/>.</exception>
        /// <remarks>
        /// <para>This is a mandatory method for a calibrator device that must be implemented.</para>
        /// <para>If the calibrator takes some time to stabilise, the <see cref="CalibratorState"/> must return <see cref="CalibratorStatus.NotReady"/>. When the 
        /// calibrator is ready for use <see cref="CalibratorState"/> must return <see cref="CalibratorStatus.Ready"/>.</para>
        /// <para>For devices with both cover and calibrator capabilities, this method may change the <see cref="CoverState"/>, if required.</para>
        /// <para>If an error condition arises while turning on the calibrator, <see cref="CalibratorState"/> must be set to <see cref="CalibratorStatus.Error"/> rather than <see cref="CalibratorStatus.Unknown"/>.</para>
        /// </remarks>
        void CalibratorOn(int Brightness);

        /// <summary>
        /// Turns the calibrator off if the device has calibration capability
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="CalibratorState"/> returns <see cref="CalibratorStatus.NotPresent"/>.</exception>
        /// <remarks>
        /// <para>This is a mandatory method for a calibrator device.</para>
        /// <para>If the calibrator requires time to safely stabilise after use, <see cref="CalibratorState"/> must return <see cref="CalibratorStatus.NotReady"/>. When the 
        /// calibrator is safely off <see cref="CalibratorState"/> must return <see cref="CalibratorStatus.Off"/>.</para>
        /// <para>For devices with both cover and calibrator capabilities, this method will return the <see cref="CoverState"/> to its status prior to calling <see cref="CalibratorOn(int)"/>.</para>
        /// <para>If an error condition arises while turning off the calibrator, <see cref="CalibratorState"/> must be set to <see cref="CalibratorStatus.Error"/> rather than <see cref="CalibratorStatus.Unknown"/>.</para>
        /// </remarks>
        void CalibratorOff();
    }
}
