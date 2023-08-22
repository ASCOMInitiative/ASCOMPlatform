using System.Collections.Generic;

namespace ASCOM.DynamicRemoteClients
{
    public class StringListResponse:RestResponseBase
    {
        private List<string> list;

        public StringListResponse() { }

        public StringListResponse(uint clientTransactionID, uint transactionID, List<string> value)
        {
            base.ServerTransactionID = transactionID;
            list = value;
            base.ClientTransactionID = clientTransactionID;
        }

        public List<string> Value
        {
            get { return list; }
            set { list = value; }
        }
    }
}
