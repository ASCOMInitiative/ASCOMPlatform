using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.DynamicRemoteClients
{
    public class AlpacaConfiguredDevicesResponse : RestResponseBase
    {
        public AlpacaConfiguredDevicesResponse()
        {
            Value = new List<AlpacaConfiguredDevice>();
        }

        public AlpacaConfiguredDevicesResponse(uint clientTransactionID, uint transactionID, List<AlpacaConfiguredDevice> value)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
            Value = value;
        }

        public List<AlpacaConfiguredDevice> Value { get; set; }
    }
}
