// Settings class for the $safeprojectname$ focuser driver
// there is no need to edit this file

using System.Configuration;

namespace ASCOM.$safeprojectname$.Properties
{
    [DeviceId("ASCOM.$safeprojectname$.Focuser", DeviceName = "$safeprojectname$ Focuser")]
    [SettingsProvider(typeof(ASCOM.SettingsProvider))]
    internal partial class Settings { }
}
