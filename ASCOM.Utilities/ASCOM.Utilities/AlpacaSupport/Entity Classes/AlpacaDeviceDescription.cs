
namespace ASCOM.Utilities
{
    /// <summary>
/// Description of an Alpaca device that is discovered by the <see cref="AlpacaDiscovery"/> component.
/// </summary>
/// <remarks>This class is used in JSON de-serialisation </remarks>
    internal class AlpacaDeviceDescription
    {
        /// <summary>
    /// Class initialiser
    /// </summary>
    /// <remarks>COM clients should use this initialiser and set the properties individually because COM only supports parameterless initialisers.</remarks>
        public AlpacaDeviceDescription()
        {
        }

        /// <summary>
    /// Class initialiser that sets all properties in one call
    /// </summary>
    /// <param name="serverName">The Alpaca device's configured name</param>
    /// <param name="manufacturer">The device manufacturer's name</param>
    /// <param name="manufacturerVersion">The device's version as set by the manufacturer</param>
    /// <param name="location">The Alpaca device's configured location</param>
    /// <remarks>This can only be used by .NET clients because COM only supports parameterless initialisers.</remarks>
        internal AlpacaDeviceDescription(string serverName, string manufacturer, string manufacturerVersion, string location)
        {
            ServerName = serverName;
            Manufacturer = manufacturer;
            ManufacturerVersion = manufacturerVersion;
            Location = location;
        }

        /// <summary>
    /// The Alpaca device's configured name
    /// </summary>
    /// <returns></returns>
        public string ServerName { get; set; } = "";

        /// <summary>
    /// The device manufacturer's name
    /// </summary>
    /// <returns></returns>
        public string Manufacturer { get; set; } = "";

        /// <summary>
    /// The device's version as set by the manufacturer
    /// </summary>
    /// <returns></returns>
        public string ManufacturerVersion { get; set; } = "";

        /// <summary>
    /// The Alpaca device's configured location
    /// </summary>
    /// <returns></returns>
        public string Location { get; set; } = "";

    }
}