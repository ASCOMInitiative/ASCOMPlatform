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

                    // Load settings stored in the filter wheel profile
                    profile.DeviceType = "FilterWheel";

                    if (!profile.IsRegistered(FilterWheel.ProgId))
                    {
                        profile.Register(FilterWheel.ProgId, FilterWheel.ChooserDescription);
                    }

                    FilterWheelHostedProgId = profile.GetValue(FilterWheel.ProgId, FILTERWHEEL_PROGID_PROFILE_NAME, string.Empty, FILTERWHEEL_PROGID_DEFAULT);
                    FilterWheelDriverLogging = Convert.ToBoolean(profile.GetValue(FilterWheel.ProgId, FILTERWHEEL_DRIVER_LOGGING_PROFILE_NAME, string.Empty, FILTERWHEEL_DRIVER_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
                    FilterWheelHardwareLogging = Convert.ToBoolean(profile.GetValue(FilterWheel.ProgId, FILTERWHEEL_HARDWARE_LOGGING_PROFILE_NAME, string.Empty, FILTERWHEEL_HARDWARE_LOGGING_DEFAULT), CultureInfo.InvariantCulture);
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

                // Save filter wheel specific values
                profile.DeviceType = "FilterWheel";
                profile.WriteValue(FilterWheel.ProgId, FILTERWHEEL_PROGID_PROFILE_NAME, FilterWheelHostedProgId);
                profile.WriteValue(FilterWheel.ProgId, FILTERWHEEL_DRIVER_LOGGING_PROFILE_NAME, FilterWheelDriverLogging.ToString(CultureInfo.InvariantCulture));
                profile.WriteValue(FilterWheel.ProgId, FILTERWHEEL_HARDWARE_LOGGING_PROFILE_NAME, FilterWheelHardwareLogging.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal static bool LocalServerLogging { get; set; }
        internal static string CameraHostedProgId { get; set; }
        internal static bool CameraDriverLogging { get; set; }
        internal static bool CameraHardwareLogging { get; set; }

        internal static string FilterWheelHostedProgId { get; set; }
        internal static bool FilterWheelDriverLogging { get; set; }
        internal static bool FilterWheelHardwareLogging { get; set; }

    }
}
