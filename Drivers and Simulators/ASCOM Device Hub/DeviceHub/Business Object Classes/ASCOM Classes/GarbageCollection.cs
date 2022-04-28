using System;
using System.Threading;

namespace ASCOM.DeviceHub
{
	/// <summary>
	/// Summary description for GarbageCollection.
	/// </summary>
	public class GarbageCollection
	{
		private int Interval { get; set; }
		private ManualResetEvent ForceCollect { get; set; }

		public GarbageCollection( int interval )
		{
			Interval = interval;
		}

		public void GCWatch( CancellationToken token )
		{
			if ( token == null )
			{
				throw new ArgumentException( "GCWatch was called with a null cancellation token!" );
			}

			bool taskCancelled = false;

			ForceCollect = new ManualResetEvent( false );
			WaitHandle[] waitHandles = new WaitHandle[] { token.WaitHandle, ForceCollect };

			while ( !taskCancelled )
			{
				GC.Collect();

				// Sleep until the interval expires or we are cancelled.

				int index = WaitHandle.WaitAny( waitHandles, Interval );

				if ( index == 0 )
				{
					taskCancelled = true;
				}
				else if ( index == 1 )
				{
					// We have been awakened externally to force garbage to be collected.

					Thread.Sleep( 100 ); // Give the objects a chance to be released.
					ForceCollect.Reset();
				}
				else if ( index == WaitHandle.WaitTimeout )
				{
					// The polling interval has expired.
				}
			}

			// Collect garbage one more time.

			GC.Collect();
		}

		public void ForceGCNow()
		{
			ForceCollect.Set();

		}
	}
}
