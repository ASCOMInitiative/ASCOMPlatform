using System;
using System.Collections.Generic;

namespace ASCOM.DynamicRemoteClients
{
    public class AxisRatesResponse : RestResponseBase
    {
        public AxisRatesResponse() { }

        public AxisRatesResponse(uint clientTransactionID, uint transactionID, List<RateResponse> value)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
            Value = value;
        }

        public List<RateResponse> Value { get; set; }
    }
}