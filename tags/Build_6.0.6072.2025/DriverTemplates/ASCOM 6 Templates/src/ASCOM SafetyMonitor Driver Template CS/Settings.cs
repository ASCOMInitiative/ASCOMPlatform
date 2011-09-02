// Settings class for the $safeprojectname$ SafetyMonitor driver
// there is no need to edit this file

using System.Configuration;

namespace ASCOM.$safeprojectname$.Properties
{
    [DeviceId("ASCOM.$safeprojectname$.SafetyMonitor", DeviceName = "$safeprojectname$ SafetyMonitor")]
    [SettingsProvider(typeof(ASCOM.SettingsProvider))]
    internal partial class Settings { }
}
