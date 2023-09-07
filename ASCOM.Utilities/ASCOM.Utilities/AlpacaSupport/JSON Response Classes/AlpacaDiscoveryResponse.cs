
namespace ASCOM.Utilities
{
    internal class AlpacaDiscoveryResponse
    {
        public AlpacaDiscoveryResponse()
        {
        }

        public AlpacaDiscoveryResponse(int alpacaPort)
        {
            AlpacaPort = alpacaPort;
        }

        public int AlpacaPort { get; set; } = 11111;
    }
}