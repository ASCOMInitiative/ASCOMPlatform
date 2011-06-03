using System;
using System.Runtime.InteropServices;

namespace TEMPLATENAMESPACE
{
	[ComVisible(false)]
	public class ReferenceCountedObjectBase
	{
		public ReferenceCountedObjectBase()
		{
			// We increment the global count of objects.
			TEMPLATEDEVICENAME.CountObject();
		}

		~ReferenceCountedObjectBase()
		{
			// We decrement the global count of objects.
			TEMPLATEDEVICENAME.UncountObject();
			// We then immediately test to see if we the conditions
			// are right to attempt to terminate this server application.
			TEMPLATEDEVICENAME.ExitIf();
		}
	}
}
