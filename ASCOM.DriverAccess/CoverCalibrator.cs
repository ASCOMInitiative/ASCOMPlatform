﻿using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Provides universal access to CoverCalibrator drivers
    /// </summary>
    public class CoverCalibrator : AscomDriver, ICoverCalibratorV2
    {
        private MemberFactory memberFactory;

        #region CoverCalibrator constructors

        /// <summary>
        /// Creates a CoverCalibrator object with the given ProgID
        /// </summary>
        /// <param name="coverCalibratorId">ProgID of the CoverCalibrator device to be accessed.</param>
        public CoverCalibrator(string coverCalibratorId)
            : base(coverCalibratorId)
        {
            memberFactory = base.MemberFactory;
        }
        #endregion

        #region Convenience Members
        /// <summary>
        /// Brings up the ASCOM Chooser Dialogue to choose a CoverCalibrator
        /// </summary>
        /// <param name="coverCalibratorId">CoverCalibrator ProgID for default or null for None</param>
        /// <returns>ProgID for chosen CoverCalibrator or null for none</returns>
        public static string Choose(string coverCalibratorId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "CoverCalibrator";
                return chooser.Choose(coverCalibratorId);
            }
        }

        /// <summary>
        /// CoverCalibrator device state
        /// <remarks>
        /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on using the class returned by this property.</para>
        /// </remarks>
        /// </summary>
        public CoverCalibratorState CoverCalibratorState
        {
            get
            {
                // Create a state object to return.
                CoverCalibratorState deviceState = new CoverCalibratorState(DeviceState, TL);
                TL.LogMessage(nameof(CoverCalibratorState), $"Returning: '{deviceState.Brightness}' '{deviceState.CalibratorChanging}' '{deviceState.CalibratorState}' '{deviceState.CoverMoving}' '{deviceState.CoverState}' '{deviceState.TimeStamp}'");

                // Return the device specific state class
                return deviceState;
            }
        }

        #endregion

        #region ICoverCalibratorV1 Members

        /// <summary>
        /// Returns the state of the device cover, if present, otherwise returns "NotPresent"
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <para>This is a mandatory property that must return a value, it must not throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// <para>Whenever the cover is moving both <see cref="CoverMoving"/> must be True and CoverState must be <see cref="CoverStatus.Moving"/>.</para>
        /// <para>The <see cref="CoverStatus.Unknown"/> state must only be returned if the device is unaware of the cover's state e.g. if the hardware does not report the open / closed state and the cover has just been powered on.
        /// Clients do not need to take special action if this state is returned, they must carry on as usual, issuing  <see cref="OpenCover"/> or <see cref="CloseCover"/> commands as required.</para>
        /// <para>If the cover hardware cannot report its state, the device could mimic this by recording the last configured state and returning this. Driver authors or device manufacturers may also wish to offer users
        /// the capability of powering up in a known state e.g. Open or Closed and driving the hardware to this state when Connected is set <see langword="true"/>.</para>
        /// <para>This property is intended to be available under all but the most disastrous driver conditions.If something has gone wrong, the CoverState must be <see cref="CoverStatus.Error"/>
        /// rather than throwing an exception.</para>
        /// </remarks>
        public CoverStatus CoverState
        {
            get { return (CoverStatus)(memberFactory.CallMember(1, "CoverState", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Initiates cover opening if a cover is present
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="CoverState"/> returns <see cref="CoverStatus.NotPresent"/>.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <para>While the cover is opening <see cref="CoverState"/> must return <see cref="CoverStatus.Moving"/>.</para>
        /// <para>When the cover is open <see cref="CoverState"/> must return <see cref="CoverStatus.Open"/>.</para>
        /// <para>If an error condition arises while moving between states, <see cref="CoverState"/> must be set to <see cref="CoverStatus.Error"/> rather than <see cref="CoverStatus.Unknown"/>.</para>
        /// </remarks>
        public void OpenCover()
        {
            memberFactory.CallMember(3, "OpenCover", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Initiates cover closing if a cover is present
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="CoverState"/> returns <see cref="CoverStatus.NotPresent"/>.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <para>While the cover is closing <see cref="CoverState"/> must return <see cref="CoverStatus.Moving"/>.</para>
        /// <para>When the cover is closed <see cref="CoverState"/> must return <see cref="CoverStatus.Closed"/>.</para>
        /// <para>If an error condition arises while moving between states, <see cref="CoverState"/> must be set to <see cref="CoverStatus.Error"/> rather than <see cref="CoverStatus.Unknown"/>.</para>
        /// </remarks>
        public void CloseCover()
        {
            memberFactory.CallMember(3, "CloseCover", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Stops any cover movement that may be in progress if a cover is present and cover movement can be interrupted.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="CoverState"/> returns <see cref="CoverStatus.NotPresent"/> or if cover movement cannot be interrupted.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <para>This must stop any cover movement quickly and set a <see cref="CoverState"/> of <see cref="CoverStatus.Open"/>, <see cref="CoverStatus.Closed"/> 
        /// or <see cref="CoverStatus.Unknown"/> as appropriate.</para>
        /// <para>If cover movement cannot be interrupted, a <see cref="MethodNotImplementedException"/> must be thrown.</para>
        /// </remarks>
        public void HaltCover()
        {
            memberFactory.CallMember(3, "HaltCover", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// Returns the state of the calibration device, if present, otherwise returns "NotPresent"
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <para>This is a mandatory property that must return a value, it must not throw a <see cref="PropertyNotImplementedException"/>.</para>
        /// <para>Whenever the calibrator is changing both <see cref="CalibratorChanging"/> must be True and CalibratorState must be <see cref="CalibratorStatus.NotReady"/>.</para>
        /// <para>The <see cref="CalibratorStatus.Unknown"/> state must only be returned if the device is unaware of the calibrator's state e.g. if the hardware does not report the device's state and 
        /// the calibrator has just been powered on. Clients do not need to take special action if this state is returned, they must carry on as usual, issuing <see cref="CalibratorOn(int)"/> and 
        /// <see cref="CalibratorOff"/> commands as required.</para>
        /// <para>If the calibrator hardware cannot report its state, the device could mimic this by recording the last configured state and returning this. Driver authors or device manufacturers may also wish to offer users
        /// the capability of powering up in a known state and driving the hardware to this state when Connected is set <see langword="true"/>.</para>
        /// <para>This property is intended to be available under all but the most disastrous driver conditions.If something has gone wrong, the CoverState must be <see cref="CalibratorStatus.Error"/>
        /// rather than throwing an exception.</para>
        /// </remarks>
        public CalibratorStatus CalibratorState
        {
            get { return (CalibratorStatus)(memberFactory.CallMember(1, "CalibratorState", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Returns the current calibrator brightness in the range 0 (completely off) to <see cref="MaxBrightness"/> (fully on)
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When <see cref="CalibratorState"/> returns <see cref="CalibratorStatus.NotPresent"/>.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <para>This is a mandatory property for a calibrator device </para>
        /// <para>The brightness value must be 0 when the <see cref="CalibratorState"/> is <see cref="CalibratorStatus.Off"/></para>
        /// </remarks>
        public int Brightness
        {
            get { return Convert.ToInt32(memberFactory.CallMember(1, "Brightness", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// The Brightness value that makes the calibrator deliver its maximum illumination.
        /// </summary>
        /// <exception cref="PropertyNotImplementedException">When <see cref="CalibratorState"/> returns <see cref="CalibratorStatus.NotPresent"/>.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <para>This is a mandatory property for a calibrator device and must be within the integer range 1 to 2,147,483,647</para>
        /// <para>A value of 1 indicates that the calibrator can only be "off" or "on".</para>
        /// <para>A value of 10 indicates that the calibrator has 10 discreet illumination levels in addition to "off".</para>
        /// <para>The value for this parameter should be determined by the driver author or device manufacturer based on the capabilities of the hardware used in the calibrator.</para>
        /// </remarks>
        public int MaxBrightness
        {
            get { return Convert.ToInt32(memberFactory.CallMember(1, "MaxBrightness", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Turns the calibrator on at the specified brightness if the device has calibration capability
        /// </summary>
        /// <param name="Brightness">Sets the required calibrator illumination brightness in the range 0 (fully off) to <see cref="MaxBrightness"/> (fully on).</param>
        /// <exception cref="InvalidValueException">When the supplied brightness parameter is outside the range 0 to <see cref="MaxBrightness"/>.</exception>
        /// <exception cref="MethodNotImplementedException">When <see cref="CalibratorState"/> returns <see cref="CalibratorStatus.NotPresent"/>.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <para>This is a mandatory method for a calibrator device that must be implemented.</para>
        /// <para>If the calibrator takes some time to stabilise, the <see cref="CalibratorState"/> must return <see cref="CalibratorStatus.NotReady"/>. When the 
        /// calibrator is ready for use <see cref="CalibratorState"/> must return <see cref="CalibratorStatus.Ready"/>.</para>
        /// <para>For devices with both cover and calibrator capabilities, this method may change the <see cref="CoverState"/>, if required.</para>
        /// <para>If an error condition arises while turning on the calibrator, <see cref="CalibratorState"/> must be set to <see cref="CalibratorStatus.Error"/> rather than <see cref="CalibratorStatus.Unknown"/>.</para>
        /// </remarks>
        public void CalibratorOn(int Brightness)
        {
            memberFactory.CallMember(3, "CalibratorOn", new Type[] { typeof(int) }, new object[] { Brightness });
        }

        /// <summary>
        /// Turns the calibrator off if the device has calibration capability
        /// </summary>
        /// <exception cref="MethodNotImplementedException">When <see cref="CalibratorState"/> returns <see cref="CalibratorStatus.NotPresent"/>.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <para>This is a mandatory method for a calibrator device.</para>
        /// <para>If the calibrator requires time to safely stabilise after use, <see cref="CalibratorState"/> must return <see cref="CalibratorStatus.NotReady"/>. When the 
        /// calibrator is safely off <see cref="CalibratorState"/> must return <see cref="CalibratorStatus.Off"/>.</para>
        /// <para>For devices with both cover and calibrator capabilities, this method will return the <see cref="CoverState"/> to its status prior to calling <see cref="CalibratorOn(int)"/>.</para>
        /// <para>If an error condition arises while turning off the calibrator, <see cref="CalibratorState"/> must be set to <see cref="CalibratorStatus.Error"/> rather than <see cref="CalibratorStatus.Unknown"/>.</para>
        /// </remarks>
        public void CalibratorOff()
        {
            memberFactory.CallMember(3, "CalibratorOff", new Type[] { }, new object[] { });
        }

        #endregion

        #region ICoverCalibratorV2 members

        /// <summary>
        /// Flag showing whether a calibrator brightness state change is in progress. 
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <returns>
        /// True while the calibrator brightness is not stable following a <see cref="CalibratorOn(int)"/> or <see cref="CalibratorOff"/> command.
        /// </returns>
        /// <remarks>
        /// <p style="color:red"><b>This is a mandatory property and must not throw a <see cref="PropertyNotImplementedException"/>.</b></p>
        /// <para>
        /// This property must throw an exception ff an issue arises while changing calibrator brightness. The exception must continue to be thrown until a new <see cref="CalibratorOn(int)"/> or
        /// <see cref="CalibratorOff"/> command is received.</para>
        /// </remarks>
        public bool CalibratorChanging
        {
            get
            {
                // Call the device's CalibratorChanging method if this is a Platform 7 or later device, otherwise use CalibratorState
                if (HasConnectAndDeviceState) // Platform 7 or later device
                {
                    TL.LogMessage("CalibratorChanging", "Issuing CalibratorChanging command");
                    return Convert.ToBoolean(memberFactory.CallMember(1, "CalibratorChanging", new Type[] { }, new object[] { }));
                }

                // Platform 6 or earlier device so use CalibratorState to determine the movement state.
                return (CalibratorStatus)memberFactory.CallMember(1, "CalibratorState", new Type[] { }, new object[] { }) == CalibratorStatus.NotReady;
            }
        }

        /// <summary>
        /// Flag showing whether the cover is moving. 
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. Include sufficient detail in the message text to enable the issue to be accurately diagnosed by someone other than yourself.</exception> 
        /// <returns>
        /// True while the cover is in motion following an <see cref="OpenCover"/> or <see cref="CloseCover"/> command.
        /// </returns>
        /// <remarks>
        /// <p style="color:red"><b>This is a mandatory property and must not throw a <see cref="PropertyNotImplementedException"/>.</b></p>
        /// </remarks>
        public bool CoverMoving
        {
            get
            {
                // Call the device's CoverMoving method if this is a Platform 7 or later device, otherwise use CoverState
                if (HasConnectAndDeviceState) // Platform 7 or later device
                {
                    TL.LogMessage("CoverMoving", "Issuing CoverMoving command");
                    return Convert.ToBoolean(memberFactory.CallMember(1, "CoverMoving", new Type[] { }, new object[] { }));
                }

                // Platform 6 or earlier device so use CoverState to determine the movement state.
                return (CoverStatus)memberFactory.CallMember(1, "CoverState", new Type[] { }, new object[] { }) == CoverStatus.Moving;
            }
        }

        #endregion
    }
}