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
	public class TrackingRates : ITrackingRates, IEnumerable, IEnumerator
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

		public void Dispose()
		{
			// Add any required object cleanup here
		}

		#region IEnumerator implementation

		public IEnumerator GetEnumerator()
		{
			_pos = -1;

			return ( _trackingRates as IEnumerable ).GetEnumerator();
		}

		#endregion

		#region IEnumerable implementation

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
	}
}
