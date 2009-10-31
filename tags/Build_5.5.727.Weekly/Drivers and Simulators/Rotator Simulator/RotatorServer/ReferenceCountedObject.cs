using System;
using System.Runtime.InteropServices;
//using System.Diagnostics;

namespace ASCOM.Simulator
{
	[ComVisible(false)]
	public class ReferenceCountedObjectBase
	{
		public ReferenceCountedObjectBase()
		{
			// We increment the global count of objects.
			RotatorSimulator.CountObject();
		}

		~ReferenceCountedObjectBase()
		{
			// We decrement the global count of objects.
			RotatorSimulator.UncountObject();
			// We then immediately test to see if we the conditions
			// are right to attempt to terminate this server application.
			RotatorSimulator.ExitIf();
		}
	}
}
