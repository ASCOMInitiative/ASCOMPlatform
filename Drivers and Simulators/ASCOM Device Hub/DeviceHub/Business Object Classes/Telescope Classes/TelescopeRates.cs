using System.Collections;
using System.Threading;
using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	class ScopeAxisRates : IAxisRates, IEnumerable
	{
		private readonly IRate[] _rates;

		// Constructor - Internal prevents public creation
		// of instances. Returned by Telescope.AxisRates.

		internal ScopeAxisRates( IRate[] rates )
		{
			_rates = rates;
		}

		#region IAxisRates Members

		public int Count
		{
			get { return _rates.Length; }
		}

		public void Dispose()
		{
			// Add any required object cleanup here
		}

		public IEnumerator GetEnumerator()
		{
			return _rates.GetEnumerator();
		}

		public IRate this[int index]
		{
			get { return this._rates[index - 1]; }   // 1-based
		}

		#endregion
	}

	public class ScopeTrackingRates : ITrackingRates, IEnumerable, IEnumerator
	{
		private readonly DriveRates[] _trackingRates;

		// this is used to make the index thread safe
		private readonly ThreadLocal<int> pos = new ThreadLocal<int>( () => { return -1; } );
		private static readonly object lockObj = new object();

		// Default constructor - Internal prevents public creation
		// of instances. Returned by Telescope.AxisRates.

		internal ScopeTrackingRates( DriveRates[] trackingRates )
		{
			_trackingRates = trackingRates;
		}

		#region ITrackingRates Members

		public int Count
		{
			get { return this._trackingRates.Length; }
		}

		public IEnumerator GetEnumerator()
		{
			pos.Value = -1;
			return this as IEnumerator;
		}

		public void Dispose()
		{
			// Add any required object cleanup here
		}

		public DriveRates this[int index]
		{
			get { return this._trackingRates[index - 1]; }   // 1-based
		}

		#endregion

		#region IEnumerable members

		public object Current
		{
			get
			{
				lock ( lockObj )
				{
					if ( pos.Value < 0 || pos.Value >= _trackingRates.Length )
					{
						throw new System.InvalidOperationException();
					}

					return _trackingRates[pos.Value];
				}
			}
		}

		public bool MoveNext()
		{
			lock ( lockObj )
			{
				if ( ++pos.Value >= _trackingRates.Length )
				{
					return false;
				}

				return true;
			}
		}

		public void Reset()
		{
			pos.Value = -1;
		}

		#endregion
	}
}
