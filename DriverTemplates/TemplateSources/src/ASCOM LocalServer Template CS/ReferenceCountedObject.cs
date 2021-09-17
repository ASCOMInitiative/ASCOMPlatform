using System;
using System.Runtime.InteropServices;

namespace ASCOM.LocalServer
{
	[ComVisible(false)]
	public class ReferenceCountedObjectBase
	{
		public ReferenceCountedObjectBase()
		{
			// We increment the global count of objects.
			Server.IncrementObjectCount();
		}

		~ReferenceCountedObjectBase()
		{
			// We decrement the global count of objects.
			Server.DecrementObjectCount();
			// We then immediately test to see if we the conditions
			// are right to attempt to terminate this server application.
			Server.ExitIf();
		}
	}
}
