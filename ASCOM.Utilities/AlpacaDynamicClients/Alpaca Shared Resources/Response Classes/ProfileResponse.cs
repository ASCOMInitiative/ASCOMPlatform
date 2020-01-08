using System.Collections.Generic;

namespace ASCOM.DynamicRemoteClients
{
    public class ProfileResponse : RestResponseBase
    {
        private List<ProfileDevice> list;

        public ProfileResponse() { }

        public ProfileResponse(uint clientTransactionID, uint transactionID, List<ProfileDevice> value)
        {
            base.ServerTransactionID = transactionID;
            list = value;
            base.ClientTransactionID = clientTransactionID;
        }

        public List<ProfileDevice> Value
        {
            get { return list; }
            set { list = value; }
        }
    }
}