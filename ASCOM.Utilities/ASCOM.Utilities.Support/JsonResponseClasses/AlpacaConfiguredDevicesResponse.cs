using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.Utilities.Support
{
    public class AlpacaConfiguredDevicesResponse : RestResponseBase
    {
        public AlpacaConfiguredDevicesResponse()
        {
            Value = new List<ConfiguredDevice>();
        }

        public AlpacaConfiguredDevicesResponse(uint clientTransactionID, uint transactionID, List<ConfiguredDevice> value)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
            Value = value;
        }

        public List<ConfiguredDevice> Value { get; set; }
    }
}
