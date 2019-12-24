using System;

namespace ASCOM.Remote
{
    public class DoubleResponse : RestResponseBase
    {
        public DoubleResponse() { }

        public DoubleResponse(uint clientTransactionID, uint transactionID, double value)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
            Value = value;
        }

        public double Value { get; set; }
    }
}
