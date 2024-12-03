using ASCOM.Utilities;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace ASCOM.JustAHub
{
    internal static class Settings
    {
        private const string LOCAL_SERVER_LOGGING_PROFILE_NAME = "Local Server Logging"; internal const string LOCAL_SERVER_LOGGING_DEFAULT = "False";

        private const string CAMERA_PROGID_PROFILE_NAME = "Hosted ProgID"; internal const string CAMERA_PROGID_DEFAULT = "ASCOM.Simulator.Camera";
        private const string CAMERA_DRIVER_LOGGING_PROFILE_NAME = "Driver Logging"; internal const string CAMERA_DRIVER_LOGGING_DEFAULT = "False";
        private const string CAMERA_HARDWARE_LOGGING_PROFILE_NAME = "Hardware Logging"; internal const string CAMERA_HARDWARE_LOGGING_DEFAULT = "False";

        private const string FILTERWHEEL_PROGID_PROFILE_NAME = "Hosted ProgID"; internal const string FILTERWHEEL_PROGID_DEFAULT = "ASCOM.Simulator.FilterWheel";
        private const string FILTERWHEEL_DRIVER_LOGGING_PROFILE_NAME = "Driver Logging"; internal const string FILTERWHEEL_DRIVER_LOGGING_DEFAULT = "False";
        private const string FILTERWHEEL_HARDWARE_LOGGING_PROFILE_NAME = "Hardware Logging"; internal const string FILTERWHEEL_HARDWARE_LOGGING_DEFAULT = "False";

        private const string SAFETYMONITOR_PROGID_PROFILE_NAME = "Hosted ProgID"; internal const string SAFETYMONITOR_PROGID_DEFAULT = "ASCOM.Simulator.SafetyMonitor";
        private const string SAFETYMONITOR_DRIVER_LOGGING_PROFILE_NAME = "Driver Logging"; internal const string SAFETYMONITOR_DRIVER_LOGGING_DEFAULT = "False";
        private const string SAFETYMONITOR_HARDWARE_LOGGING_PROFILE_NAME = "Hardware Logging"; internal const string SAFETYMONITOR_HARDWARE_LOGGING_DEFAULT = "False";

        private const string COVERCALIBRATOR_PROGID_PROFILE_NAME = "Hosted ProgID"; internal const string COVERCALIBRATOR_PROGID_DEFAULT = "ASCOM.Simulator.CoverCalibrator";
        private const string COVERCALIBRATOR_DRIVER_LOGGING_PROFILE_NAME = "Driver Logging"; internal const string COVERCALIBRATOR_DRIVER_LOGGING_DEFAULT = "False";
        private const string COVERCALIBRATOR_HARDWARE_LOGGING_PROFILE_NAME = "Hardware Logging"; internal const string COVERCALIBRATOR_HARDWARE_LOGGING_DEFAULT = "False";

        static Settings()
        {
            using (Profile profile = new Profile())
            {
                try
                {
                    // Load settings stored in the camera profile
                    profile.DeviceType = "Camera";

                    // Register the drivers in the Profile in case they are not already registered
                    if (!profile.IsRegistered(Camera.ProgId))
                    {
                        profile.Register(Camera.ProgId, Camera.ChooserDescription);
                    }

                    // Get global values that are stored in the camera profile
                    LocalServerLogging = Convert.ToBoolean(profile.GetValue(Camera.ProgId, LOCAL_SERVER_LOGGING_PROFILE_NAME, string.Empty, LOCAL_SERVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

                    // Get camera specific values
                    CameraHostedProgId = profile.GetValue(Camera.ProgId, CAMERA_PROGID_PROFILE_NAME, string.Empty, CAMERA_PROGID_DEFAULT);
                    CameraDriverLogging = Convert.ToBoolean(profile.GetValue(Camera.ProgId, CAMERA_DRIVER_LOGGING_PROFILE_NAME, string.Empty, CAMERA_DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    CameraHardwareLogging = Convert.ToBoolean(profile.GetValue(Camera.ProgId, CAMERA_HARDWARE_LOGGING_PROFILE_NAME, string.Empty, CAMERA_HARDWARE_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

                    // Load settings stored in the cover calibrator profile
                    profile.DeviceType = "CoverCalibrator";

                    if (!profile.IsRegistered(CoverCalibrator.ProgId))
                    {
                        profile.Register(CoverCalibrator.ProgId, CoverCalibrator.ChooserDescription);
                    }

                    CoverCalibratorHostedProgId = profile.GetValue(CoverCalibrator.ProgId, COVERCALIBRATOR_PROGID_PROFILE_NAME, string.Empty, COVERCALIBRATOR_PROGID_DEFAULT);
                    CoverCalibratorDriverLogging = Convert.ToBoolean(profile.GetValue(CoverCalibrator.ProgId, COVERCALIBRATOR_DRIVER_LOGGING_PROFILE_NAME, string.Empty, COVERCALIBRATOR_DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    CoverCalibratorHardwareLogging = Convert.ToBoolean(profile.GetValue(CoverCalibrator.ProgId, COVERCALIBRATOR_HARDWARE_LOGGING_PROFILE_NAME, string.Empty, COVERCALIBRATOR_HARDWARE_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

                    // Load settings stored in the filter wheel profile
                    profile.DeviceType = "FilterWheel";

                    if (!profile.IsRegistered(FilterWheel.ProgId))
                    {
                        profile.Register(FilterWheel.ProgId, FilterWheel.ChooserDescription);
                    }

                    FilterWheelHostedProgId = profile.GetValue(FilterWheel.ProgId, FILTERWHEEL_PROGID_PROFILE_NAME, string.Empty, FILTERWHEEL_PROGID_DEFAULT);
                    FilterWheelDriverLogging = Convert.ToBoolean(profile.GetValue(FilterWheel.ProgId, FILTERWHEEL_DRIVER_LOGGING_PROFILE_NAME, string.Empty, FILTERWHEEL_DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    FilterWheelHardwareLogging = Convert.ToBoolean(profile.GetValue(FilterWheel.ProgId, FILTERWHEEL_HARDWARE_LOGGING_PROFILE_NAME, string.Empty, FILTERWHEEL_HARDWARE_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

                    // Load settings stored in the safety monitor profile
                    profile.DeviceType = "SafetyMonitor";

                    if (!profile.IsRegistered(SafetyMonitor.ProgId))
                    {
                        profile.Register(SafetyMonitor.ProgId, SafetyMonitor.ChooserDescription);
                    }

                    SafetyMonitorHostedProgId = profile.GetValue(SafetyMonitor.ProgId, SAFETYMONITOR_PROGID_PROFILE_NAME, string.Empty, SAFETYMONITOR_PROGID_DEFAULT);
                    SafetyMonitorDriverLogging = Convert.ToBoolean(profile.GetValue(SafetyMonitor.ProgId, SAFETYMONITOR_DRIVER_LOGGING_PROFILE_NAME, string.Empty, SAFETYMONITOR_DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    SafetyMonitorHardwareLogging = Convert.ToBoolean(profile.GetValue(SafetyMonitor.ProgId, SAFETYMONITOR_HARDWARE_LOGGING_PROFILE_NAME, string.Empty, SAFETYMONITOR_HARDWARE_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Settings exception: {ex.Message}\r\n{ex}");
                }
            }
        }

        internal static void SaveSettings()
        {
            using (Profile profile = new Profile())
            {
                profile.DeviceType = "Camera";

                // Save global values
                profile.WriteValue(Camera.ProgId, LOCAL_SERVER_LOGGING_PROFILE_NAME, LocalServerLogging.ToString(CultureInfo.InvariantCulture));

                // Save camera specific values
                profile.WriteValue(Camera.ProgId, CAMERA_PROGID_PROFILE_NAME, CameraHostedProgId);
                profile.WriteValue(Camera.ProgId, CAMERA_DRIVER_LOGGING_PROFILE_NAME, CameraDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(Camera.ProgId, CAMERA_HARDWARE_LOGGING_PROFILE_NAME, CameraHardwareLogging.ToString(CultureInfo.InvariantCulture));

                // Save cover calibrator specific values
                profile.DeviceType = "CoverCalibrator";
                profile.WriteValue(CoverCalibrator.ProgId, COVERCALIBRATOR_PROGID_PROFILE_NAME, CoverCalibratorHostedProgId);
                profile.WriteValue(CoverCalibrator.ProgId, COVERCALIBRATOR_DRIVER_LOGGING_PROFILE_NAME, CoverCalibratorDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(CoverCalibrator.ProgId, COVERCALIBRATOR_HARDWARE_LOGGING_PROFILE_NAME, CoverCalibratorHardwareLogging.ToString(CultureInfo.InvariantCulture));

                // Save filter wheel specific values
                profile.DeviceType = "FilterWheel";
                profile.WriteValue(FilterWheel.ProgId, FILTERWHEEL_PROGID_PROFILE_NAME, FilterWheelHostedProgId);
                profile.WriteValue(FilterWheel.ProgId, FILTERWHEEL_DRIVER_LOGGING_PROFILE_NAME, FilterWheelDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(FilterWheel.ProgId, FILTERWHEEL_HARDWARE_LOGGING_PROFILE_NAME, FilterWheelHardwareLogging.ToString(CultureInfo.InvariantCulture));

                // Save safety monitor specific values
                profile.DeviceType = "SafetyMonitor";
                profile.WriteValue(SafetyMonitor.ProgId, SAFETYMONITOR_PROGID_PROFILE_NAME, SafetyMonitorHostedProgId);
                profile.WriteValue(SafetyMonitor.ProgId, SAFETYMONITOR_DRIVER_LOGGING_PROFILE_NAME, SafetyMonitorDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(SafetyMonitor.ProgId, SAFETYMONITOR_HARDWARE_LOGGING_PROFILE_NAME, SafetyMonitorHardwareLogging.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal static bool LocalServerLogging { get; set; }

        internal static string CameraHostedProgId { get; set; }
        internal static bool CameraDriverLogging { get; set; }
        internal static bool CameraHardwareLogging { get; set; }

        internal static string FilterWheelHostedProgId { get; set; }
        internal static bool FilterWheelDriverLogging { get; set; }
        internal static bool FilterWheelHardwareLogging { get; set; }

        internal static string SafetyMonitorHostedProgId { get; set; }
        internal static bool SafetyMonitorDriverLogging { get; set; }
        internal static bool SafetyMonitorHardwareLogging { get; set; }

        internal static string CoverCalibratorHostedProgId { get; set; }
        internal static bool CoverCalibratorDriverLogging { get; set; }
        internal static bool CoverCalibratorHardwareLogging { get; set; }
    }
}
