using ASCOM.DeviceInterface;
using System.Collections.Generic;

namespace ASCOM.DynamicRemoteClients
{
    public class DeviceStateResponse : RestResponseBase
    {
        private List<StateValue> list;

        public DeviceStateResponse() { }

        public DeviceStateResponse(uint clientTransactionID, uint transactionID, List<StateValue> value)
        {
            base.ServerTransactionID = transactionID;
            list = value;
            base.ClientTransactionID = clientTransactionID;
        }

        public List<StateValue> Value
        {
            get { return list; }
            set { list = value; }
        }
    }
}
