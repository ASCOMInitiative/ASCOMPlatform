using System.Net;
using System.Threading;

namespace ASCOM.Utilities
{

    /// <summary>
/// Define the state object for the callback. 
/// Use hostName to correlate calls with the proper result.
/// </summary>
    internal class DnsResponse
    {
        private IPHostEntry f_IpHostEntry;

        public DnsResponse()
        {
            CallComplete = new ManualResetEvent(false);
        }

        public IPHostEntry IpHostEntry
        {
            get
            {
                return f_IpHostEntry;
            }
            set
            {
                // Save the value and populate the other DnsResponse fields
                f_IpHostEntry = value;
                HostName = value.HostName;
                Aliases = value.Aliases;
                AddressList = value.AddressList;
            }
        }

        public ManualResetEvent CallComplete { get; set; }
        public string HostName { get; set; }
        public string[] Aliases { get; set; }
        public IPAddress[] AddressList { get; set; }
    }
}