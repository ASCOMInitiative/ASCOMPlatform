using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.Remote
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
