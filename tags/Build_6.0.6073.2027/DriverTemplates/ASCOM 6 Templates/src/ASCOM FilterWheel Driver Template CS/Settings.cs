// Settings class for the $safeprojectname$ filterwheel driver
// there is no need to edit this file

using System.Configuration;

namespace ASCOM.$safeprojectname$.Properties
{
    [DeviceId("ASCOM.$safeprojectname$.FilterWheel", DeviceName = "$safeprojectname$ Filterwheel")]
    [SettingsProvider(typeof(ASCOM.SettingsProvider))]
    internal partial class Settings { }
}
