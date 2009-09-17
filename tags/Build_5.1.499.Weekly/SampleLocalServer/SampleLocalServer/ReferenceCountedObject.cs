using System;
using System.Runtime.InteropServices;

namespace ASCOM.SampleLocalServer
{
	[ComVisible(false)]  // This ComVisibleAttribute is set to false so that TLBEXP and REGASM will not expose it nor COM-register it.
	public class ReferenceCountedObjectBase
	{
		public ReferenceCountedObjectBase()
		{
			// We increment the global count of objects.
			SampleLocalServer.CountObject();
		}

		~ReferenceCountedObjectBase()
		{
			// We decrement the global count of objects.
			SampleLocalServer.UncountObject();
			// We then immediately test to see if we the conditions
			// are right to attempt to terminate this server application.
			SampleLocalServer.ExitIf();
		}
	}
}
