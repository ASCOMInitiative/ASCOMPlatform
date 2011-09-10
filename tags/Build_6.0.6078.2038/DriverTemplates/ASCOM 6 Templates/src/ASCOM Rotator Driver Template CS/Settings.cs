// Settings class for the $safeprojectname$ Rotator driver
// there is no need to edit this file

using System.Configuration;

namespace ASCOM.$safeprojectname$.Properties
{
    [DeviceId("ASCOM.$safeprojectname$.Rotator", DeviceName = "$safeprojectname$ Rotator")]
    [SettingsProvider(typeof(ASCOM.SettingsProvider))]
    internal partial class Settings { }
}
