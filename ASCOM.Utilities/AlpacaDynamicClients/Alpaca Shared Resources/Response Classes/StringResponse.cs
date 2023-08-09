namespace ASCOM.DynamicRemoteClients
{
    public class StringResponse : RestResponseBase
    {
        public StringResponse() { }

        public StringResponse(uint clientTransactionID, uint transactionID, string value)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
            Value = value;
        }

        public string Value { get; set; }
    }
}
