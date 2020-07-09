using System;

namespace ASCOM.DynamicRemoteClients
{
    public class BoolResponse : RestResponseBase
    {
        public BoolResponse() { }
        public BoolResponse(uint clientTransactionID, uint transactionID, bool value)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
            Value = value;
        }

        public bool Value { get; set; }
    }
}
