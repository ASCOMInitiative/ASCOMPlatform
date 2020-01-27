using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASCOM;
using ASCOM.DeviceInterface;

namespace Unit_Tests
{
	public class TrackingRates : ITrackingRates, IEnumerable, IEnumerator, IDisposable
	{
		private DriveRates[] _trackingRates;
		private int _pos = -1;

		internal TrackingRates()
			: this( new DriveRates[] { DriveRates.driveSidereal, DriveRates.driveKing, DriveRates.driveLunar, DriveRates.driveSolar } )
		{ }

		internal TrackingRates( DriveRates[] allowedRates )
		{
			_trackingRates = allowedRates;

			_pos = -1;
		}

		#region ITrackingRates Members

		public int Count => _trackingRates.Length;

		public DriveRates this[int index]
		{
			get
			{
				if ( index < 1 || index > this.Count )
				{
					throw new InvalidValueException( "TrackingRates.this", index.ToString( CultureInfo.CurrentCulture ), string.Format( CultureInfo.CurrentCulture, "1 to {0}", this.Count ) );
				}

				return _trackingRates[index - 1]; }   // 1-based
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator GetEnumerator()
		{
			_pos = -1;

			return ( _trackingRates as IEnumerable ).GetEnumerator();
		}

		#endregion

		#region IEnumerator implementation

		public bool MoveNext()
		{
			if ( ++_pos >= _trackingRates.Length )
			{
				return false;
			}

			return true;
		}

		public void Reset()
		{
			_pos = -1;
		}

		public object Current
		{
			get
			{
				if ( _pos < 0 || _pos >= _trackingRates.Length )
				{
					throw new System.InvalidOperationException();
				}

				return _trackingRates[_pos];
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		// The bulk of the clean-up code is implemented in Dispose(bool)
		protected virtual void Dispose( bool disposing )
		{
			if ( disposing )
			{
				// free managed resources

				_trackingRates = null;
			}
		}

		#endregion
	}
}
