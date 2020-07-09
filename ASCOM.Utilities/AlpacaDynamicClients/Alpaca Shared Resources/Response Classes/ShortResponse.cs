using System;

namespace ASCOM.DynamicRemoteClients
{
    public class ShortResponse : RestResponseBase
    {
        public ShortResponse() { }

        public ShortResponse(uint clientTransactionID, uint transactionID, short value)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
            Value = value;
        }

        public short Value { get; set; }
    }
}
