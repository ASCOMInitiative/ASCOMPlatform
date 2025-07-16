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
        private const string _gemAxisOffset2ProfileName = "GEM Axis Offset 2";
        private const string _gemAxisOffset3ProfileName = "GEM Axis Offset 3";
        private const string _gemAxisOffset4ProfileName = "GEM Axis Offset 4";
        private const string _gemAxisOffset5ProfileName = "GEM Axis Offset 5";
        private const string _gemAxisOffsetDefault = "0";

        private const string _opticalOffsetProfileName = "Optical Offset";
        private const string _opticalOffset2ProfileName = "Optical Offset 2";
        private const string _opticalOffset3ProfileName = "Optical Offset 3";
        private const string _opticalOffset4ProfileName = "Optical Offset 4";
        private const string _opticalOffset5ProfileName = "Optical Offset 5";
        private const string _opticalOffsetDefault = "0";

        private const string _telescopeNameProfileName = "Telescope Name";
        private const string _telescopeName2ProfileName = "Telescope Name 2";
        private const string _telescopeName3ProfileName = "Telescope Name 3";
        private const string _telescopeName4ProfileName = "Telescope Name 4";
        private const string _telescopeName5ProfileName = "Telescope Name 5";
        private const string _telescope1NameDefault = "Main Telescope";
        private const string _telescopeNameDefault = "";

        private const string _azimuthAccuracyProfileName = "Azimuth Accuracy";
        private const string _azimuthAccuracyDefault = "2";
        private const string _azimuthAdjustmentProfileName = "Azimuth Adjustment";
        private const string _azimuthAdjustmentDefault = "0";
        private const string _slaveIntervalProfileName = "Slave Interval";
        private const string _slaveIntervalDefault = "30";
        private const string _usePOTHSlavingCalculationProfileName = "Use POTH Slaving Calculation";
        private const string _usePOTHSlavingCalculationDefault = "false";
        private const string _useRevisedSlavingCalculationProfileName = "Use Revised Slaving Calculation";
        private const string _useRevisedSlavingCalculationDefault = "false";
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

            int gemAxisOffset2;
            int gemAxisOffset3;
            int gemAxisOffset4;
            int gemAxisOffset5;

            int opticalOffset2;
            int opticalOffset3;
            int opticalOffset4;
            int opticalOffset5;

            string telescopeName;
            string telescopeName2;
            string telescopeName3;
            string telescopeName4;
            string telescopeName5;

            int opticalOffset;
            int azimuthAccuracy;
            double azimuthAdjustment;
            int slaveInterval;
            bool loggerEnabled;
            bool usePOTHCalculation;
            bool useRevisedCalculation;
            double fastUpdatePeriod;

            using (Profile profile = new Profile())
            {
                profile.DeviceType = "Dome";
                domeID = profile.GetValue(DriverID, _domeIDProfileName, String.Empty, _domeIDDefault);
                layoutXOffset = Convert.ToDouble(profile.GetValue(DriverID, _xOffsetProfileName, String.Empty, _offsetDefault), CultureInfo.InvariantCulture);
                layoutYOffset = Convert.ToDouble(profile.GetValue(DriverID, _yOffsetProfileName, String.Empty, _offsetDefault), CultureInfo.InvariantCulture);
                layoutZOffset = Convert.ToDouble(profile.GetValue(DriverID, _zOffsetProfileName, String.Empty, _offsetDefault), CultureInfo.InvariantCulture);
                domeRadius = Convert.ToInt32(profile.GetValue(DriverID, _radiusProfileName, String.Empty, _radiusDefault));
                azimuthAccuracy = Convert.ToInt32(profile.GetValue(DriverID, _azimuthAccuracyProfileName, String.Empty, _azimuthAccuracyDefault));
                azimuthAdjustment = Convert.ToDouble(profile.GetValue(DriverID, _azimuthAdjustmentProfileName, String.Empty, _azimuthAdjustmentDefault), CultureInfo.InvariantCulture);
                slaveInterval = Convert.ToInt32(profile.GetValue(DriverID, _slaveIntervalProfileName, String.Empty, _slaveIntervalDefault));
                loggerEnabled = Convert.ToBoolean(profile.GetValue(DriverID, _traceStateProfileName, String.Empty, _traceStateDefault));
                usePOTHCalculation = Convert.ToBoolean(profile.GetValue(DriverID, _usePOTHSlavingCalculationProfileName, String.Empty, _usePOTHSlavingCalculationDefault));
                useRevisedCalculation = Convert.ToBoolean(profile.GetValue(DriverID, _useRevisedSlavingCalculationProfileName, String.Empty, _useRevisedSlavingCalculationDefault));
                fastUpdatePeriod = Convert.ToDouble(profile.GetValue(DriverID, _fastUpdateProfileName, String.Empty, _fastUpdateDefault), CultureInfo.InvariantCulture);

                gemAxisOffset = Convert.ToInt32(profile.GetValue(DriverID, _gemAxisOffsetProfileName, String.Empty, _gemAxisOffsetDefault));
                gemAxisOffset2 = Convert.ToInt32(profile.GetValue(DriverID, _gemAxisOffset2ProfileName, String.Empty, _gemAxisOffsetDefault));
                gemAxisOffset3 = Convert.ToInt32(profile.GetValue(DriverID, _gemAxisOffset3ProfileName, String.Empty, _gemAxisOffsetDefault));
                gemAxisOffset4 = Convert.ToInt32(profile.GetValue(DriverID, _gemAxisOffset4ProfileName, String.Empty, _gemAxisOffsetDefault));
                gemAxisOffset5 = Convert.ToInt32(profile.GetValue(DriverID, _gemAxisOffset5ProfileName, String.Empty, _gemAxisOffsetDefault));

                opticalOffset = Convert.ToInt32(profile.GetValue(DriverID, _opticalOffsetProfileName, String.Empty, _opticalOffsetDefault));
                opticalOffset2 = Convert.ToInt32(profile.GetValue(DriverID, _opticalOffset2ProfileName, String.Empty, _opticalOffsetDefault));
                opticalOffset3 = Convert.ToInt32(profile.GetValue(DriverID, _opticalOffset3ProfileName, String.Empty, _opticalOffsetDefault));
                opticalOffset4 = Convert.ToInt32(profile.GetValue(DriverID, _opticalOffset4ProfileName, String.Empty, _opticalOffsetDefault));
                opticalOffset5 = Convert.ToInt32(profile.GetValue(DriverID, _opticalOffset5ProfileName, String.Empty, _opticalOffsetDefault));

                telescopeName = profile.GetValue(DriverID, _telescopeNameProfileName, String.Empty, _telescope1NameDefault);
                telescopeName2 = profile.GetValue(DriverID, _telescopeName2ProfileName, String.Empty, _telescopeNameDefault);
                telescopeName3 = profile.GetValue(DriverID, _telescopeName3ProfileName, String.Empty, _telescopeNameDefault);
                telescopeName4 = profile.GetValue(DriverID, _telescopeName4ProfileName, String.Empty, _telescopeNameDefault);
                telescopeName5 = profile.GetValue(DriverID, _telescopeName5ProfileName, String.Empty, _telescopeNameDefault);
            }

            // Prevent the user from circumventing the valid fast update by setting the value in the profile store directly.

            fastUpdatePeriod = Math.Max(Globals.DOME_FAST_UPDATE_MIN, Math.Min(fastUpdatePeriod, Globals.DOME_FAST_UPDATE_MAX));

            DomeLayoutSettings layoutSettings = new DomeLayoutSettings
            {
                DomeScopeOffset = new System.Windows.Media.Media3D.Point3D(layoutXOffset, layoutYOffset, layoutZOffset),
                DomeRadius = domeRadius,
                AzimuthAccuracy = azimuthAccuracy,
                SlaveInterval = slaveInterval,

                GemAxisOffset = gemAxisOffset,
                GemAxisOffset2 = gemAxisOffset2,
                GemAxisOffset3 = gemAxisOffset3,
                GemAxisOffset4 = gemAxisOffset4,
                GemAxisOffset5 = gemAxisOffset5,

                OpticalOffset = opticalOffset,
                OpticalOffset2 = opticalOffset2,
                OpticalOffset3 = opticalOffset3,
                OpticalOffset4 = opticalOffset4,
                OpticalOffset5 = opticalOffset5,

                TelescopeName = telescopeName,
                TelescopeName2 = telescopeName2,
                TelescopeName3 = telescopeName3,
                TelescopeName4 = telescopeName4,
                TelescopeName5 = telescopeName5,
            };

            DomeSettings settings = new DomeSettings
            {
                DomeID = domeID,
                DomeLayout = layoutSettings,
                AzimuthAdjustment = azimuthAdjustment,
                UsePOTHDomeSlaveCalculation = usePOTHCalculation,
                UseRevisedDomeSlaveCalculation = useRevisedCalculation,
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
        public bool UseRevisedDomeSlaveCalculation { get; set; }
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
                profile.WriteValue(DriverID, _azimuthAccuracyProfileName, DomeLayout.AzimuthAccuracy.ToString());
                profile.WriteValue(DriverID, _azimuthAdjustmentProfileName, AzimuthAdjustment.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(DriverID, _slaveIntervalProfileName, DomeLayout.SlaveInterval.ToString());
                profile.WriteValue(DriverID, _traceStateProfileName, IsLoggingEnabled.ToString());
                profile.WriteValue(DriverID, _usePOTHSlavingCalculationProfileName, UsePOTHDomeSlaveCalculation.ToString());
                profile.WriteValue(DriverID, _useRevisedSlavingCalculationProfileName, UseRevisedDomeSlaveCalculation.ToString());
                profile.WriteValue(DriverID, _fastUpdateProfileName, FastUpdatePeriod.ToString(CultureInfo.InvariantCulture));

                profile.WriteValue(DriverID, _gemAxisOffsetProfileName, DomeLayout.GemAxisOffset.ToString());
                profile.WriteValue(DriverID, _gemAxisOffset2ProfileName, DomeLayout.GemAxisOffset2.ToString());
                profile.WriteValue(DriverID, _gemAxisOffset3ProfileName, DomeLayout.GemAxisOffset3.ToString());
                profile.WriteValue(DriverID, _gemAxisOffset4ProfileName, DomeLayout.GemAxisOffset4.ToString());
                profile.WriteValue(DriverID, _gemAxisOffset5ProfileName, DomeLayout.GemAxisOffset5.ToString());

                profile.WriteValue(DriverID, _opticalOffsetProfileName, DomeLayout.OpticalOffset.ToString());
                profile.WriteValue(DriverID, _opticalOffset2ProfileName, DomeLayout.OpticalOffset2.ToString());
                profile.WriteValue(DriverID, _opticalOffset3ProfileName, DomeLayout.OpticalOffset3.ToString());
                profile.WriteValue(DriverID, _opticalOffset4ProfileName, DomeLayout.OpticalOffset4.ToString());
                profile.WriteValue(DriverID, _opticalOffset5ProfileName, DomeLayout.OpticalOffset5.ToString());

                profile.WriteValue(DriverID, _telescopeNameProfileName, DomeLayout.TelescopeName);
                profile.WriteValue(DriverID, _telescopeName2ProfileName, DomeLayout.TelescopeName2);
                profile.WriteValue(DriverID, _telescopeName3ProfileName, DomeLayout.TelescopeName3);
                profile.WriteValue(DriverID, _telescopeName4ProfileName, DomeLayout.TelescopeName4);
                profile.WriteValue(DriverID, _telescopeName5ProfileName, DomeLayout.TelescopeName5);
            }
        }
    }
}
