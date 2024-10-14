using System;
using System.Threading;

namespace ASCOM.LocalServer
{
    /// <summary>
    /// Summary description for GarbageCollection.
    /// </summary>
    internal class GarbageCollection
    {
        private readonly int _interval;

        public GarbageCollection(int interval)
        {
            _interval = interval;
        }

        public void GCWatch(CancellationToken token)
        {
            if (token == null)
            {
                throw new ArgumentException("GCWatch was called with a null cancellation token!");
            }

            bool taskCancelled = false;

            while (!taskCancelled)
            {
                GC.Collect();

                // Sleep until the interval expires or we are cancelled.

                taskCancelled = token.WaitHandle.WaitOne(_interval);
            }

            // Collect garbage one more time.

            GC.Collect();
        }
    }
}
