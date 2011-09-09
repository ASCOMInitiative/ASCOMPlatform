// Settings class for the $safeprojectname$ camera driver
// there is no need to edit this file

using System.Configuration;

namespace ASCOM.$safeprojectname$.Properties
{
    [DeviceId("ASCOM.$safeprojectname$.Camera", DeviceName = "$safeprojectname$ Camera")]
    [SettingsProvider(typeof(ASCOM.SettingsProvider))]
    internal partial class Settings
    {
    }
}
