using System;
using System.Runtime.InteropServices;

namespace ASCOM.LocalServerCOM
{
	[ComVisible(false)]  // This ComVisibleAttribute is set to false so that TLBEXP and REGASM will not expose it nor COM-register it.
	public class ReferenceCountedObjectBase
	{
		public ReferenceCountedObjectBase()
		{
			LocalServerCOM.Trace("ReferenceCountedObjectBase contructor.");
			// We increment the global count of objects.
			LocalServerCOM.CountObject();
		}

		~ReferenceCountedObjectBase()
		{
			LocalServerCOM.Trace("ReferenceCountedObjectBase destructor.");
			// We decrement the global count of objects.
			LocalServerCOM.UncountObject();
			// We then immediately test to see if we the conditions
			// are right to attempt to terminate this server application.
			LocalServerCOM.ExitIf();
		}
	}
}
