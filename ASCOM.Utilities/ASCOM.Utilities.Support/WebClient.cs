using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ASCOM.Utilities.Support
{

    internal class WebClient : System.Net.WebClient
    {
        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest webRequest = base.GetWebRequest(uri);
            webRequest.Timeout = Timeout;
            ((HttpWebRequest)webRequest).ReadWriteTimeout = Timeout;
            return webRequest;
        }
    }
}
