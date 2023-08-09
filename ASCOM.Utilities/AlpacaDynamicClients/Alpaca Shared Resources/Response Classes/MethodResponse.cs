namespace ASCOM.DynamicRemoteClients
{
    public enum MethodTypes
    {
        PropertyGet,
        PropertySet,
        Method,
        Function
    }

    public class MethodResponse : RestResponseBase
    {
        public MethodResponse() { }
        public MethodResponse(uint clientTransactionID, uint transactionID)
        {
            base.ServerTransactionID = transactionID;
            base.ClientTransactionID = clientTransactionID; 
        }
        // No additional fields for this class
    }
}
