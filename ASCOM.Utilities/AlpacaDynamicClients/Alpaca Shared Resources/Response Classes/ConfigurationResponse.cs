using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ASCOM.DynamicRemoteClients
{
    public class ConfigurationResponse : RestResponseBase
    {
        private ConcurrentDictionary<string, ConfiguredDevice> list;

        public ConfigurationResponse() { }

        public ConfigurationResponse(uint clientTransactionID, uint transactionID, ConcurrentDictionary<string, ConfiguredDevice> value)
        {
            base.ClientTransactionID = clientTransactionID;
            base.ServerTransactionID = transactionID;
            list = value;
        }

        public ConcurrentDictionary<string, ConfiguredDevice> Value
        {
            get { return list; }
            set { list = value; }
        }
    }
}