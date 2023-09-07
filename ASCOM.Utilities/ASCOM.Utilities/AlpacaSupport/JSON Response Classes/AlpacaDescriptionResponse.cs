
namespace ASCOM.Utilities
{
    internal class AlpacaDescriptionResponse : RestResponseBase
    {

        public AlpacaDescriptionResponse()
        {
            Value = new AlpacaDeviceDescription();
        }

        public AlpacaDescriptionResponse(uint clientTransactionID, uint transactionID, AlpacaDeviceDescription value)
        {
            ServerTransactionID = transactionID;
            ClientTransactionID = clientTransactionID;
            Value = value;
        }

        public AlpacaDeviceDescription Value { get; set; }
    }
}