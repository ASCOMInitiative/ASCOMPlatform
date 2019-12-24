using System;

namespace ASCOM.Remote
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
