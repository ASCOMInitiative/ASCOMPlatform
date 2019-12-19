using System;

namespace ASCOM.Utilities.Support
{
    public class AlpacaDescriptionResponse : RestResponseBase
    {
        public AlpacaDescriptionResponse()
        {
            Value = new AlpacaDeviceDescription();
        }

        public AlpacaDescriptionResponse(uint clientTransactionID, uint transactionID, AlpacaDeviceDescription value)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
            Value = value;
        }

        public AlpacaDeviceDescription Value { get; set; }
    }
}
