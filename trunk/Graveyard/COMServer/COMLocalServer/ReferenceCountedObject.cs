using System;
using System.Runtime.InteropServices;

namespace ManagedCOMLocalServer_Impl01
{
	[ComVisible(false)]  // This ComVisibleAttribute is set to false so that TLBEXP and REGASM will not expose it nor COM-register it.
	public class ReferenceCountedObjectBase
	{
		public ReferenceCountedObjectBase()
		{
			Console.WriteLine("ReferenceCountedObjectBase contructor.");
			// We increment the global count of objects.
			ManagedCOMLocalServer_Impl01.InterlockedIncrementObjectsCount();
		}

		~ReferenceCountedObjectBase()
		{
			Console.WriteLine("ReferenceCountedObjectBase destructor.");
			// We decrement the global count of objects.
			ManagedCOMLocalServer_Impl01.InterlockedDecrementObjectsCount();
			// We then immediately test to see if we the conditions
			// are right to attempt to terminate this server application.
			ManagedCOMLocalServer_Impl01.AttemptToTerminateServer();
		}
	}
}
