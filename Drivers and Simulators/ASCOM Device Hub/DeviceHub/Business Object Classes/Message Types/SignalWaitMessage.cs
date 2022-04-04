using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.DeviceHub
{
	internal class SignalWaitMessage
	{
		public SignalWaitMessage( bool wait )
		{
			Wait = wait;
		}

		public bool Wait { get; private set; }
	}
}
