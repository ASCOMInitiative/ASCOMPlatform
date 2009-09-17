using System;
using System.Runtime.InteropServices;

namespace ASCOM.$safeprojectname$
{
	[ComVisible(false)]
	public class ReferenceCountedObjectBase
	{
		public ReferenceCountedObjectBase()
		{
			// We increment the global count of objects.
			$safeprojectname$.CountObject();
		}

		~ReferenceCountedObjectBase()
		{
			// We decrement the global count of objects.
			$safeprojectname$.UncountObject();
			// We then immediately test to see if we the conditions
			// are right to attempt to terminate this server application.
			$safeprojectname$.ExitIf();
		}
	}
}
