using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Provides universal access to CoverCalibrator drivers
    /// </summary>
    public class CoverCalibrator : AscomDriver, ICoverCalibratorV1, ICoverCalibratorV2
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

        /// <inheritdoc/>
        public CoverStatus CoverState
        {
            get { return (CoverStatus)(memberFactory.CallMember(1, "CoverState", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public void OpenCover()
        {
            memberFactory.CallMember(3, "OpenCover", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
        public void CloseCover()
        {
            memberFactory.CallMember(3, "CloseCover", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
        public void HaltCover()
        {
            memberFactory.CallMember(3, "HaltCover", new Type[] { }, new object[] { });
        }

        /// <inheritdoc/>
        public CalibratorStatus CalibratorState
        {
            get { return (CalibratorStatus)(memberFactory.CallMember(1, "CalibratorState", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public int Brightness
        {
            get { return Convert.ToInt32(memberFactory.CallMember(1, "Brightness", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public int MaxBrightness
        {
            get { return Convert.ToInt32(memberFactory.CallMember(1, "MaxBrightness", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public void CalibratorOn(int Brightness)
        {
            memberFactory.CallMember(3, "CalibratorOn", new Type[] { typeof(int) }, new object[] { Brightness });
        }

        /// <inheritdoc/>
        public void CalibratorOff()
        {
            memberFactory.CallMember(3, "CalibratorOff", new Type[] { }, new object[] { });
        }

        #endregion

        #region ICoverCalibratorV2 members

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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