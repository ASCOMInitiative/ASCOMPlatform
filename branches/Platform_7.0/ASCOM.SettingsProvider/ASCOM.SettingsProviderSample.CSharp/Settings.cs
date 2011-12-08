using System.Configuration;

namespace ASCOM.SettingsProviderSample.CSharp.Properties
{
    [DeviceId("ASCOM.SettingsProvider.Sample")]
    [SettingsProvider(typeof(ASCOM.SettingsProvider))]
    internal sealed partial class Settings
    {
    }
}