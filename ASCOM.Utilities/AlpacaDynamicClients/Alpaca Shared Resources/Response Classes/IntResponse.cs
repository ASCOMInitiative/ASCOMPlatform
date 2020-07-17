using System;

namespace ASCOM.DynamicRemoteClients
{
    public class IntResponse : RestResponseBase
    {
        public IntResponse() { }

        public IntResponse(uint clientTransactionID, uint transactionID, int value)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
            Value = value;
        }

        public int Value { get; set; }
    }
}
