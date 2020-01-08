using System.Collections.Specialized;
using System.Net;

namespace ASCOM.DynamicRemoteClients
{
    public class RequestData
    {
        public RequestData()
        {
        }

        public RequestData(string clientIpAddress, uint clientID, uint clientTransactionID, uint serverTransactionID, NameValueCollection suppliedParameters, HttpListenerRequest request, HttpListenerResponse response, string[] elements, string deviceKey)
        {
            ClientIpAddress = clientIpAddress;
            ClientID = clientID;
            ClientTransactionID = clientTransactionID;
            ServerTransactionID = serverTransactionID;
            SuppliedParameters = suppliedParameters;
            Request = request;
            Response = response;
            Elements = elements;
            DeviceKey = deviceKey;
        }

        public uint ClientID { get; set; }
        public uint ClientTransactionID { get; set; }
        public uint ServerTransactionID { get; set; }
        public NameValueCollection SuppliedParameters { get; set; }
        public HttpListenerRequest Request { get; set; }
        public HttpListenerResponse Response { get; set; }
        public string[] Elements { get; set; }
        public string DeviceKey { get; set; }
        public string ClientIpAddress { get; set; }
    }
}
