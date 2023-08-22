namespace ASCOM.DynamicRemoteClients
{
    public class RestResponseBaseConcreteClass : RestResponseBase
    {
        public RestResponseBaseConcreteClass() { }

        public RestResponseBaseConcreteClass(uint clientTransactionID, uint transactionID)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID;
        }
    }
}
