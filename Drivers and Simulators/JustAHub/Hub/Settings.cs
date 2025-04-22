using ASCOM.Utilities;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace ASCOM.JustAHub
{
    internal static class Settings
    {
        // Common settings for all drivers
        private const string LOCAL_SERVER_LOGGING_PROFILE_NAME = "Local Server Logging"; internal const string LOCAL_SERVER_LOGGING_DEFAULT = "False";
        private const string DRIVER_LOGGING_PROFILE_NAME = "Driver Logging"; internal const string DRIVER_LOGGING_DEFAULT = "False";
        private const string HARDWARE_LOGGING_PROFILE_NAME = "Hardware Logging"; internal const string HARDWARE_LOGGING_DEFAULT = "False";
        private const string PROGID_PROFILE_NAME = "Hosted ProgID";

        // Per driver settings
        private const string CAMERA_PROGID_DEFAULT = "ASCOM.Simulator.Camera";
        private const string COVERCALIBRATOR_PROGID_DEFAULT = "ASCOM.Simulator.CoverCalibrator";
        private const string DOME_PROGID_DEFAULT = "ASCOM.Simulator.Dome";
        private const string FILTERWHEEL_PROGID_DEFAULT = "ASCOM.Simulator.FilterWheel";
        private const string FOCUSER_PROGID_DEFAULT = "ASCOM.Simulator.Focuser";
        private const string OBSERVINGCONDITIONS_PROGID_DEFAULT = "ASCOM.Simulator.ObservingConditions";
        private const string ROTATOR_PROGID_DEFAULT = "ASCOM.Simulator.Rotator";
        private const string SAFETYMONITOR_PROGID_DEFAULT = "ASCOM.Simulator.SafetyMonitor";
        private const string SWITCH_PROGID_DEFAULT = "ASCOM.Simulator.Switch";
        private const string TELESCOPE_PROGID_DEFAULT = "ASCOM.Simulator.Telescope";

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
                    CameraHostedProgId = profile.GetValue(Camera.ProgId, PROGID_PROFILE_NAME, string.Empty, CAMERA_PROGID_DEFAULT);
                    CameraDriverLogging = Convert.ToBoolean(profile.GetValue(Camera.ProgId, DRIVER_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    CameraHardwareLogging = Convert.ToBoolean(profile.GetValue(Camera.ProgId, HARDWARE_LOGGING_PROFILE_NAME, string.Empty, HARDWARE_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

                    // Load settings stored in the cover calibrator profile
                    profile.DeviceType = "CoverCalibrator";

                    if (!profile.IsRegistered(CoverCalibrator.ProgId))
                    {
                        profile.Register(CoverCalibrator.ProgId, CoverCalibrator.ChooserDescription);
                    }

                    CoverCalibratorHostedProgId = profile.GetValue(CoverCalibrator.ProgId, PROGID_PROFILE_NAME, string.Empty, COVERCALIBRATOR_PROGID_DEFAULT);
                    CoverCalibratorDriverLogging = Convert.ToBoolean(profile.GetValue(CoverCalibrator.ProgId, DRIVER_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    CoverCalibratorHardwareLogging = Convert.ToBoolean(profile.GetValue(CoverCalibrator.ProgId, HARDWARE_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

                    // Load settings stored in the dome profile
                    profile.DeviceType = "Dome";

                    if (!profile.IsRegistered(Dome.ProgId))
                    {
                        profile.Register(Dome.ProgId, Dome.ChooserDescription);
                    }

                    DomeHostedProgId = profile.GetValue(Dome.ProgId, PROGID_PROFILE_NAME, string.Empty, DOME_PROGID_DEFAULT);
                    DomeDriverLogging = Convert.ToBoolean(profile.GetValue(Dome.ProgId, DRIVER_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    DomeHardwareLogging = Convert.ToBoolean(profile.GetValue(Dome.ProgId, HARDWARE_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

                    // Load settings stored in the filter wheel profile
                    profile.DeviceType = "FilterWheel";

                    if (!profile.IsRegistered(FilterWheel.ProgId))
                    {
                        profile.Register(FilterWheel.ProgId, FilterWheel.ChooserDescription);
                    }

                    FilterWheelHostedProgId = profile.GetValue(FilterWheel.ProgId, PROGID_PROFILE_NAME, string.Empty, FILTERWHEEL_PROGID_DEFAULT);
                    FilterWheelDriverLogging = Convert.ToBoolean(profile.GetValue(FilterWheel.ProgId, DRIVER_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    FilterWheelHardwareLogging = Convert.ToBoolean(profile.GetValue(FilterWheel.ProgId, HARDWARE_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

                    // Load settings stored in the focuser profile
                    profile.DeviceType = "Focuser";

                    if (!profile.IsRegistered(Focuser.ProgId))
                    {
                        profile.Register(Focuser.ProgId, Focuser.ChooserDescription);
                    }

                    FocuserHostedProgId = profile.GetValue(Focuser.ProgId, PROGID_PROFILE_NAME, string.Empty, FOCUSER_PROGID_DEFAULT);
                    FocuserDriverLogging = Convert.ToBoolean(profile.GetValue(Focuser.ProgId, DRIVER_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    FocuserHardwareLogging = Convert.ToBoolean(profile.GetValue(Focuser.ProgId, HARDWARE_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

                    // Load settings stored in the observing conditions profile
                    profile.DeviceType = "ObservingConditions";

                    if (!profile.IsRegistered(ObservingConditions.ProgId))
                    {
                        profile.Register(ObservingConditions.ProgId, ObservingConditions.ChooserDescription);
                    }

                    ObservingConditionsHostedProgId = profile.GetValue(ObservingConditions.ProgId, PROGID_PROFILE_NAME, string.Empty, OBSERVINGCONDITIONS_PROGID_DEFAULT);
                    ObservingConditionsDriverLogging = Convert.ToBoolean(profile.GetValue(ObservingConditions.ProgId, DRIVER_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    ObservingConditionsHardwareLogging = Convert.ToBoolean(profile.GetValue(ObservingConditions.ProgId, HARDWARE_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

                    // Load settings stored in the rotator profile
                    profile.DeviceType = "Rotator";

                    if (!profile.IsRegistered(Rotator.ProgId))
                    {
                        profile.Register(Rotator.ProgId, Rotator.ChooserDescription);
                    }

                    RotatorHostedProgId = profile.GetValue(Rotator.ProgId, PROGID_PROFILE_NAME, string.Empty, ROTATOR_PROGID_DEFAULT);
                    RotatorDriverLogging = Convert.ToBoolean(profile.GetValue(Rotator.ProgId, DRIVER_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    RotatorHardwareLogging = Convert.ToBoolean(profile.GetValue(Rotator.ProgId, HARDWARE_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

                    // Load settings stored in the safety monitor profile
                    profile.DeviceType = "SafetyMonitor";

                    if (!profile.IsRegistered(SafetyMonitor.ProgId))
                    {
                        profile.Register(SafetyMonitor.ProgId, SafetyMonitor.ChooserDescription);
                    }

                    SafetyMonitorHostedProgId = profile.GetValue(SafetyMonitor.ProgId, PROGID_PROFILE_NAME, string.Empty, SAFETYMONITOR_PROGID_DEFAULT);
                    SafetyMonitorDriverLogging = Convert.ToBoolean(profile.GetValue(SafetyMonitor.ProgId, DRIVER_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    SafetyMonitorHardwareLogging = Convert.ToBoolean(profile.GetValue(SafetyMonitor.ProgId, HARDWARE_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

                    // Load settings stored in the switch profile
                    profile.DeviceType = "Switch";

                    if (!profile.IsRegistered(Switch.ProgId))
                    {
                        profile.Register(Switch.ProgId, Switch.ChooserDescription);
                    }

                    SwitchHostedProgId = profile.GetValue(Switch.ProgId, PROGID_PROFILE_NAME, string.Empty, SWITCH_PROGID_DEFAULT);
                    SwitchDriverLogging = Convert.ToBoolean(profile.GetValue(Switch.ProgId, DRIVER_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    SwitchHardwareLogging = Convert.ToBoolean(profile.GetValue(Switch.ProgId, HARDWARE_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

                    // Load settings stored in the switch profile
                    profile.DeviceType = "Telescope";

                    if (!profile.IsRegistered(Telescope.ProgId))
                    {
                        profile.Register(Telescope.ProgId, Telescope.ChooserDescription);
                    }

                    TelescopeHostedProgId = profile.GetValue(Telescope.ProgId, PROGID_PROFILE_NAME, string.Empty, TELESCOPE_PROGID_DEFAULT);
                    TelescopeDriverLogging = Convert.ToBoolean(profile.GetValue(Telescope.ProgId, DRIVER_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    TelescopeHardwareLogging = Convert.ToBoolean(profile.GetValue(Telescope.ProgId, HARDWARE_LOGGING_PROFILE_NAME, string.Empty, DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);

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
                profile.WriteValue(Camera.ProgId, PROGID_PROFILE_NAME, CameraHostedProgId);
                profile.WriteValue(Camera.ProgId, DRIVER_LOGGING_PROFILE_NAME, CameraDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(Camera.ProgId, HARDWARE_LOGGING_PROFILE_NAME, CameraHardwareLogging.ToString(CultureInfo.InvariantCulture));

                // Save cover calibrator specific values
                profile.DeviceType = "CoverCalibrator";
                profile.WriteValue(CoverCalibrator.ProgId, PROGID_PROFILE_NAME, CoverCalibratorHostedProgId);
                profile.WriteValue(CoverCalibrator.ProgId, DRIVER_LOGGING_PROFILE_NAME, CoverCalibratorDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(CoverCalibrator.ProgId, HARDWARE_LOGGING_PROFILE_NAME, CoverCalibratorHardwareLogging.ToString(CultureInfo.InvariantCulture));

                // Save dome specific values
                profile.DeviceType = "Dome";
                profile.WriteValue(Dome.ProgId, PROGID_PROFILE_NAME, DomeHostedProgId);
                profile.WriteValue(Dome.ProgId, DRIVER_LOGGING_PROFILE_NAME, DomeDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(Dome.ProgId, HARDWARE_LOGGING_PROFILE_NAME, DomeHardwareLogging.ToString(CultureInfo.InvariantCulture));

                // Save filter wheel specific values
                profile.DeviceType = "FilterWheel";
                profile.WriteValue(FilterWheel.ProgId, PROGID_PROFILE_NAME, FilterWheelHostedProgId);
                profile.WriteValue(FilterWheel.ProgId, DRIVER_LOGGING_PROFILE_NAME, FilterWheelDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(FilterWheel.ProgId, HARDWARE_LOGGING_PROFILE_NAME, FilterWheelHardwareLogging.ToString(CultureInfo.InvariantCulture));

                // Save focuser specific values
                profile.DeviceType = "Focuser";
                profile.WriteValue(Focuser.ProgId, PROGID_PROFILE_NAME, FocuserHostedProgId);
                profile.WriteValue(Focuser.ProgId, DRIVER_LOGGING_PROFILE_NAME, FocuserDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(Focuser.ProgId, HARDWARE_LOGGING_PROFILE_NAME, FocuserHardwareLogging.ToString(CultureInfo.InvariantCulture));

                // Save observing conditions specific values
                profile.DeviceType = "ObservingConditions";
                profile.WriteValue(ObservingConditions.ProgId, PROGID_PROFILE_NAME, ObservingConditionsHostedProgId);
                profile.WriteValue(ObservingConditions.ProgId, DRIVER_LOGGING_PROFILE_NAME, ObservingConditionsDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(ObservingConditions.ProgId, HARDWARE_LOGGING_PROFILE_NAME, ObservingConditionsHardwareLogging.ToString(CultureInfo.InvariantCulture));

                // Save rotator specific values
                profile.DeviceType = "Rotator";
                profile.WriteValue(Rotator.ProgId, PROGID_PROFILE_NAME, RotatorHostedProgId);
                profile.WriteValue(Rotator.ProgId, DRIVER_LOGGING_PROFILE_NAME, RotatorDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(Rotator.ProgId, HARDWARE_LOGGING_PROFILE_NAME, RotatorHardwareLogging.ToString(CultureInfo.InvariantCulture));

                // Save safety monitor specific values
                profile.DeviceType = "SafetyMonitor";
                profile.WriteValue(SafetyMonitor.ProgId, PROGID_PROFILE_NAME, SafetyMonitorHostedProgId);
                profile.WriteValue(SafetyMonitor.ProgId, DRIVER_LOGGING_PROFILE_NAME, SafetyMonitorDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(SafetyMonitor.ProgId, HARDWARE_LOGGING_PROFILE_NAME, SafetyMonitorHardwareLogging.ToString(CultureInfo.InvariantCulture));

                // Save switch specific values
                profile.DeviceType = "Switch";
                profile.WriteValue(Switch.ProgId, PROGID_PROFILE_NAME, SwitchHostedProgId);
                profile.WriteValue(Switch.ProgId, DRIVER_LOGGING_PROFILE_NAME, SwitchDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(Switch.ProgId, HARDWARE_LOGGING_PROFILE_NAME, SwitchHardwareLogging.ToString(CultureInfo.InvariantCulture));

                // Save telescope specific values
                profile.DeviceType = "Telescope";
                profile.WriteValue(Telescope.ProgId, PROGID_PROFILE_NAME, TelescopeHostedProgId);
                profile.WriteValue(Telescope.ProgId, DRIVER_LOGGING_PROFILE_NAME, TelescopeDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(Telescope.ProgId, HARDWARE_LOGGING_PROFILE_NAME, TelescopeHardwareLogging.ToString(CultureInfo.InvariantCulture));
            }
        }

        // Common properties
        internal static bool LocalServerLogging { get; set; }

        // Camera properties
        internal static string CameraHostedProgId { get; set; }
        internal static bool CameraDriverLogging { get; set; }
        internal static bool CameraHardwareLogging { get; set; }

        // CoverCalibrator properties
        internal static string CoverCalibratorHostedProgId { get; set; }
        internal static bool CoverCalibratorDriverLogging { get; set; }
        internal static bool CoverCalibratorHardwareLogging { get; set; }

        // Dome properties
        internal static string DomeHostedProgId { get; set; }
        internal static bool DomeDriverLogging { get; set; }
        internal static bool DomeHardwareLogging { get; set; }

        // FilterWheel properties
        internal static string FilterWheelHostedProgId { get; set; }
        internal static bool FilterWheelDriverLogging { get; set; }
        internal static bool FilterWheelHardwareLogging { get; set; }

        // Focuser properties
        internal static string FocuserHostedProgId { get; set; }
        internal static bool FocuserDriverLogging { get; set; }
        internal static bool FocuserHardwareLogging { get; set; }

        // ObservingConditions properties
        internal static string ObservingConditionsHostedProgId { get; set; }
        internal static bool ObservingConditionsDriverLogging { get; set; }
        internal static bool ObservingConditionsHardwareLogging { get; set; }

        // Rotator properties
        internal static string RotatorHostedProgId { get; set; }
        internal static bool RotatorDriverLogging { get; set; }
        internal static bool RotatorHardwareLogging { get; set; }

        // SafetyMonitor properties
        internal static string SafetyMonitorHostedProgId { get; set; }
        internal static bool SafetyMonitorDriverLogging { get; set; }
        internal static bool SafetyMonitorHardwareLogging { get; set; }

        // Switch properties
        internal static string SwitchHostedProgId { get; set; }
        internal static bool SwitchDriverLogging { get; set; }
        internal static bool SwitchHardwareLogging { get; set; }

        // Telescope properties
        internal static string TelescopeHostedProgId { get; set; }
        internal static bool TelescopeDriverLogging { get; set; }
        internal static bool TelescopeHardwareLogging { get; set; }

    }
}
