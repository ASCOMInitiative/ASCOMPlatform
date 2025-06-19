using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.DeviceHub
{
    public class DomeSyncErrorStateMessage
    {
        public DomeSyncErrorStateMessage(bool state)
        {
            State = state;
        }

        public bool State { get; private set; }
    }
}
