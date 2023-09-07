using System;
using System.Net;

namespace ASCOM.Utilities
{

    internal class WebClientWithTimeOut : WebClient
    {

        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            var webRequest = base.GetWebRequest(uri);
            webRequest.Timeout = Timeout;
            ((HttpWebRequest)webRequest).ReadWriteTimeout = Timeout;
            return webRequest;
        }
    }
}