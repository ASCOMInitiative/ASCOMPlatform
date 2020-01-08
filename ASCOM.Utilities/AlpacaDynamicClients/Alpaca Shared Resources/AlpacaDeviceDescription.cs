using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.DynamicRemoteClients
{
    public class AlpacaDeviceDescription
    {
        public AlpacaDeviceDescription() { }

        public AlpacaDeviceDescription(string serverName, string manufacturer, string manufacturerVersion, string location,string alpacaUniqueId)
        {
            ServerName = serverName;
            Manufacturer = manufacturer;
            ManufacturerVersion = manufacturerVersion;
            Location = location;
            AlpacaUniqueId = alpacaUniqueId;
        }

        public string ServerName { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string ManufacturerVersion { get; set; } = "";
        public string Location { get; set; } = "";
        public string AlpacaUniqueId{ get; set; } = "";
    }
}
