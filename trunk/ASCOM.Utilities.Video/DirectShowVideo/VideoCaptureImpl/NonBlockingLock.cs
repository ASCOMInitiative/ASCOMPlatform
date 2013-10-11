using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ASCOM.Utilities.Video.DirectShowVideo.VideoCaptureImpl
{
	internal class NonBlockingLock
	{
		public static int LOCK_ID_BufferCB = 2;
		public static int LOCK_ID_GetNextFrame = 3;
		public static int LOCK_ID_CloseInterfaces = 4;

		private static int currentlyHeldLockId = 0;
		private static bool exclusiveLockActive = false;

		public static void Lock(int lockId, Action method)
		{
			try
			{
				var spinWait = new SpinWait();
				while (true)
				{
					if (!exclusiveLockActive)
					{
						int updVal = Interlocked.CompareExchange(ref currentlyHeldLockId, lockId, 0);
						if (0 != updVal) break;
					}
					spinWait.SpinOnce();
				}

				if (currentlyHeldLockId == lockId && !exclusiveLockActive)
					method();
			}
			finally
			{
				if (currentlyHeldLockId == lockId)
					currentlyHeldLockId = 0;
			}
		}

		public static void ExclusiveLock(int lockId, Action method)
		{
			try
			{
				var spinWait = new SpinWait();
				while (true)
				{
					int updVal = Interlocked.CompareExchange(ref currentlyHeldLockId, lockId, 0);
					if (0 != updVal) break;
					spinWait.SpinOnce();
				}
				exclusiveLockActive = true;

				if (currentlyHeldLockId == lockId)
					method();

			}
			finally
			{
				exclusiveLockActive = false;

				if (currentlyHeldLockId == lockId)
					currentlyHeldLockId = 0;
			}
		}
	}
}
