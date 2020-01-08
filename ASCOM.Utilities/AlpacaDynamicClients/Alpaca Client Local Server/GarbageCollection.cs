using System;
using System.Threading;

namespace ASCOM.DynamicRemoteClients
{
    /// <summary>
    /// Summary description for GarbageCollection.
    /// </summary>
    class GarbageCollection:IDisposable
    {
        protected bool m_bContinueThread;
        protected bool m_GCWatchStopped;
        protected int m_iInterval;
        protected ManualResetEvent m_EventThreadEnded;

        public GarbageCollection(int iInterval)
        {
            m_bContinueThread = true;
            m_GCWatchStopped = false;
            m_iInterval = iInterval;
            m_EventThreadEnded = new ManualResetEvent(false);
        }

        public void GCWatch()
        {
            // Pause for a moment to provide a delay to make threads more apparent.
            while (ContinueThread())
            {
                GC.Collect();
                Thread.Sleep(m_iInterval);
            }
            m_EventThreadEnded.Set();
        }

        protected bool ContinueThread()
        {
            lock (this)
            {
                return m_bContinueThread;
            }
        }

        public void StopThread()
        {
            lock (this)
            {
                m_bContinueThread = false;
            }
        }

        public void WaitForThreadToStop()
        {
            m_EventThreadEnded.WaitOne();
            m_EventThreadEnded.Reset();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    m_EventThreadEnded.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put clean-up code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
