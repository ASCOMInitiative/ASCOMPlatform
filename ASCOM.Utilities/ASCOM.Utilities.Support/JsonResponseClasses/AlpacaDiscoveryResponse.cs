using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.Utilities.Support
{
    public class AlpacaDiscoveryResponse
    {
        public AlpacaDiscoveryResponse() { }

        public AlpacaDiscoveryResponse(int alpacaPort, string alpacaUniqueId)
        {
            AlpacaPort = alpacaPort;
            AlpacaUniqueId = alpacaUniqueId;
        }

        public int AlpacaPort { get; set; } = 11111;
        public string AlpacaUniqueId { get; set; } = "";
    }
}
