using System.Collections.Generic;

namespace ASCOM.Utilities
{

    internal class AlpacaConfiguredDevicesResponse : RestResponseBase
    {

        public AlpacaConfiguredDevicesResponse()
        {
            Value = new List<ConfiguredDevice>();
        }

        public AlpacaConfiguredDevicesResponse(uint clientTransactionID, uint transactionID, List<ConfiguredDevice> value)
        {
            ServerTransactionID = transactionID;
            ClientTransactionID = clientTransactionID;
            Value = value;
        }

        public List<ConfiguredDevice> Value { get; set; }
    }
}