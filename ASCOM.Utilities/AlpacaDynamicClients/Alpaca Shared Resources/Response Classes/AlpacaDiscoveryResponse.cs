using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.Remote
{
    public class AlpacaDiscoveryResponse
    {
        public AlpacaDiscoveryResponse() { }

        public AlpacaDiscoveryResponse(int alpacaPort, string alpacaUniqueId)
        {
            AlpacaPort = alpacaPort;
            AlpacaUniqueId = alpacaUniqueId;
        }

        public int AlpacaPort { get; set; }
        public string AlpacaUniqueId { get; set; }
    }
}
