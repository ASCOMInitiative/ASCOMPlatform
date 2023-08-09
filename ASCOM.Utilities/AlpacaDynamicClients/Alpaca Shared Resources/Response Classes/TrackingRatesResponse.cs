using System.Collections.Generic;
using ASCOM.DeviceInterface;

namespace ASCOM.DynamicRemoteClients
{
    public class TrackingRatesResponse : RestResponseBase
    {
        private List<DriveRates> rates;

        public TrackingRatesResponse() { }

        public TrackingRatesResponse(uint clientTransactionID, uint transactionID)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
        }

        public List<DriveRates> Value
        {
            get { return rates; }
            set { rates = value; }
        }
    }
}
