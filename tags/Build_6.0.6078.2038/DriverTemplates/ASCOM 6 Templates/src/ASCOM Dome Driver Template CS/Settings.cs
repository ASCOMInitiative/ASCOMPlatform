// Settings class for the $safeprojectname$ dome driver
// there is no need to edit this file

using System.Configuration;

namespace ASCOM.$safeprojectname$.Properties
{
    [DeviceId("ASCOM.$safeprojectname$.Dome", DeviceName = "$safeprojectname$ Dome")]
    [SettingsProvider(typeof(ASCOM.SettingsProvider))]
    internal partial class Settings { }
}
