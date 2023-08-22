namespace ASCOM.DynamicRemoteClients
{
    public class StringArrayResponse : RestResponseBase
    {
        private string[] stringArray;

        public StringArrayResponse() { }

        public StringArrayResponse(uint clientTransactionID, uint transactionID, string[] value)
        {
            base.ServerTransactionID = transactionID;
            stringArray = value;
            base.ClientTransactionID = clientTransactionID;
        }

        public string[] Value
        {
            get { return stringArray; }
            set { stringArray = value; }
        }
    }
}
