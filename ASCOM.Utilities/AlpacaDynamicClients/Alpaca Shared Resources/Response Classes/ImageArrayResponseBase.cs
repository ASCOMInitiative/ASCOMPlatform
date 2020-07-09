using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ASCOM.DynamicRemoteClients
{
    public class ImageArrayResponseBase : RestResponseBase
    {
        private int rank = 0;
        private SharedConstants.ImageArrayElementTypes type = SharedConstants.ImageArrayElementTypes.Unknown;

        [JsonProperty(Order = -3)]
        public int Type
        {
            get { return (int)type; }
            set { type = (SharedConstants.ImageArrayElementTypes)value; }
        }

        [JsonProperty(Order = -2)]
        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        }

    }
}
