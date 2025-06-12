using System;
using System.Globalization;
using ASCOM.Utilities;

namespace ASCOM.DeviceHub
{
    public class DomeSettings
    {
        private const string _domeIDProfileName = "Dome ID";
        private const string _domeIDDefault = "ASCOM.Simulator.Dome";
        private const string _traceStateProfileName = "Trace Level";
        private const string _traceStateDefault = "false";
        private const string _xOffsetProfileName = "X Offset";
        private const string _yOffsetProfileName = "Y Offset";
        private const string _zOffsetProfileName = "Z Offset";
        private const string _offsetDefault = "0.0";
        private const string _radiusProfileName = "Dome Radius";
        private const string _radiusDefault = "2000";
        private const string _gemAxisOffsetProfileName = "GEM Axis Offset";
        private const string _gemAxisOffsetDefault = "0";
        private const string _opticalOffsetProfileName = "Optical Offset";
        private const string _opticalOffsetDefault = "0";
        private const string _azimuthAccuracyProfileName = "Azimuth Accuracy";
        private const string _azimuthAccuracyDefault = "2";
        private const string _azimuthAdjustmentProfileName = "Azimuth Adjustment";
        private const string _azimuthAdjustmentDefault = "0";
        private const string _slaveIntervalProfileName = "Slave Interval";
        private const string _slaveIntervalDefault = "30";
        private const string _usePOTHSlavingCalculationProfileName = "Use POTH Slaving Calculation";
        private const string _usePOTHSlavingCalculationDefault = "false";
        private const string _fastUpdateProfileName = "Fast Update Period";
        private static readonly string _fastUpdateDefault = Globals.DOME_FAST_UPDATE_MIN.ToString();


        private static string DriverID => Globals.DevHubDomeID;

        public static DomeSettings FromProfile()
        {
            string domeID;
            double layoutXOffset;
            double layoutYOffset;
            double layoutZOffset;
            int domeRadius;
            int gemAxisOffset;
            int opticalOffset;
            int azimuthAccuracy;
            double azimuthAdjustment;
            int slaveInterval;
            bool loggerEnabled;
            bool usePOTHCalculation;
            double fastUpdatePeriod;

            using (Profile profile = new Profile())
            {
                profile.DeviceType = "Dome";
                domeID = profile.GetValue(DriverID, _domeIDProfileName, String.Empty, _domeIDDefault);
                layoutXOffset = Convert.ToDouble(profile.GetValue(DriverID, _xOffsetProfileName, String.Empty, _offsetDefault), CultureInfo.InvariantCulture);
                layoutYOffset = Convert.ToDouble(profile.GetValue(DriverID, _yOffsetProfileName, String.Empty, _offsetDefault), CultureInfo.InvariantCulture);
                layoutZOffset = Convert.ToDouble(profile.GetValue(DriverID, _zOffsetProfileName, String.Empty, _offsetDefault), CultureInfo.InvariantCulture);
                domeRadius = Convert.ToInt32(profile.GetValue(DriverID, _radiusProfileName, String.Empty, _radiusDefault));
                gemAxisOffset = Convert.ToInt32(profile.GetValue(DriverID, _gemAxisOffsetProfileName, String.Empty, _gemAxisOffsetDefault));
                opticalOffset = Convert.ToInt32(profile.GetValue(DriverID, _opticalOffsetProfileName, String.Empty, _opticalOffsetDefault));
                azimuthAccuracy = Convert.ToInt32(profile.GetValue(DriverID, _azimuthAccuracyProfileName, String.Empty, _azimuthAccuracyDefault));
                azimuthAdjustment = Convert.ToDouble(profile.GetValue(DriverID, _azimuthAdjustmentProfileName, String.Empty, _azimuthAdjustmentDefault), CultureInfo.InvariantCulture);
                slaveInterval = Convert.ToInt32(profile.GetValue(DriverID, _slaveIntervalProfileName, String.Empty, _slaveIntervalDefault));
                loggerEnabled = Convert.ToBoolean(profile.GetValue(DriverID, _traceStateProfileName, String.Empty, _traceStateDefault));
                usePOTHCalculation = Convert.ToBoolean(profile.GetValue(DriverID, _usePOTHSlavingCalculationProfileName, String.Empty, _usePOTHSlavingCalculationDefault));
                fastUpdatePeriod = Convert.ToDouble(profile.GetValue(DriverID, _fastUpdateProfileName, String.Empty, _fastUpdateDefault), CultureInfo.InvariantCulture);
            }

            // Prevent the user from circumventing the valid fast update by setting the value in the profile store directly.

            fastUpdatePeriod = Math.Max(Globals.DOME_FAST_UPDATE_MIN, Math.Min(fastUpdatePeriod, Globals.DOME_FAST_UPDATE_MAX));

            DomeLayoutSettings layoutSettings = new DomeLayoutSettings
            {
                DomeScopeOffset = new System.Windows.Media.Media3D.Point3D(layoutXOffset, layoutYOffset, layoutZOffset),
                DomeRadius = domeRadius,
                GemAxisOffset = gemAxisOffset,
                AzimuthAccuracy = azimuthAccuracy,
                SlaveInterval = slaveInterval,
                OpticalOffset = opticalOffset
            };

            DomeSettings settings = new DomeSettings
            {
                DomeID = domeID,
                DomeLayout = layoutSettings,
                AzimuthAdjustment = azimuthAdjustment,
                UsePOTHDomeSlaveCalculation = usePOTHCalculation,
                IsLoggingEnabled = loggerEnabled,
                FastUpdatePeriod = fastUpdatePeriod
            };

            return settings;
        }

        public DomeSettings()
        { }

        public string DomeID { get; set; }
        public DomeLayoutSettings DomeLayout { get; set; }
        public bool IsLoggingEnabled { get; set; }
        public double AzimuthAdjustment { get; set; }
        public bool UsePOTHDomeSlaveCalculation { get; set; }
        public double FastUpdatePeriod { get; set; }

        public void ToProfile()
        {
            using (Profile profile = new Profile())
            {
                profile.DeviceType = "Dome";
                profile.WriteValue(DriverID, _domeIDProfileName, DomeID);
                profile.WriteValue(DriverID, _xOffsetProfileName, DomeLayout.DomeScopeOffset.X.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(DriverID, _yOffsetProfileName, DomeLayout.DomeScopeOffset.Y.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(DriverID, _zOffsetProfileName, DomeLayout.DomeScopeOffset.Z.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(DriverID, _radiusProfileName, DomeLayout.DomeRadius.ToString());
                profile.WriteValue(DriverID, _gemAxisOffsetProfileName, DomeLayout.GemAxisOffset.ToString());
                profile.WriteValue(DriverID, _opticalOffsetProfileName, DomeLayout.OpticalOffset.ToString());
                profile.WriteValue(DriverID, _azimuthAccuracyProfileName, DomeLayout.AzimuthAccuracy.ToString());
                profile.WriteValue(DriverID, _azimuthAdjustmentProfileName, AzimuthAdjustment.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(DriverID, _slaveIntervalProfileName, DomeLayout.SlaveInterval.ToString());
                profile.WriteValue(DriverID, _traceStateProfileName, IsLoggingEnabled.ToString());
                profile.WriteValue(DriverID, _usePOTHSlavingCalculationProfileName, UsePOTHDomeSlaveCalculation.ToString());
                profile.WriteValue(DriverID, _fastUpdateProfileName, FastUpdatePeriod.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
