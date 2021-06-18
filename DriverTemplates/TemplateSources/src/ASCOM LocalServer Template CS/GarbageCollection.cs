using System;
using System.Threading;

namespace ASCOM.TEMPLATEDEVICENAME.Server
{
    /// <summary>
    /// Summary description for GarbageCollection.
    /// </summary>
    class GarbageCollection
    {
        protected bool continueThread;
        protected bool gCWatchStopped;
        protected int interval;
        protected ManualResetEvent eventThreadEnded;

        public GarbageCollection(int iInterval)
        {
            continueThread = true;
            gCWatchStopped = false;
            interval = iInterval;
            eventThreadEnded = new ManualResetEvent(false);
        }

        public void GCWatch()
        {
            // Pause for a moment to provide a delay to make threads more apparent.
            while (ContinueThread())
            {
                GC.Collect();
                Thread.Sleep(interval);
            }
            eventThreadEnded.Set();
        }

        protected bool ContinueThread()
        {
            lock (this)
            {
                return continueThread;
            }
        }

        public void StopThread()
        {
            lock (this)
            {
                continueThread = false;
            }
        }

        public void WaitForThreadToStop()
        {
            eventThreadEnded.WaitOne();
            eventThreadEnded.Reset();
        }
    }
}
