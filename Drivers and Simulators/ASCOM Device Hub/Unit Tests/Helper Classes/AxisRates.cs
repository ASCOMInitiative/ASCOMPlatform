using System;
using System.Collections;
using System.Globalization;

using ASCOM;
using ASCOM.DeviceInterface;

namespace Unit_Tests
{
	public class AxisRates : IAxisRates, IEnumerable, IEnumerator, IDisposable
	{
		private static AxisRate[] PrimaryAxisRates;
		private static AxisRate[] SecondaryAxisRates;
		private static AxisRate[] TertiaryAxisRates;

		static AxisRates()
		{
			ResetRates();
		}

		internal static void ResetRates()
		{
			PrimaryAxisRates = new AxisRate[] { new AxisRate( 0.0, 5.0 ) };
			SecondaryAxisRates = new AxisRate[] { new AxisRate( 0.0, 5.0 ) };
			TertiaryAxisRates = new AxisRate[0];
		}

		internal static void SetRates( TelescopeAxes axis, AxisRate[] rates )
		{
			if ( axis == TelescopeAxes.axisPrimary )
			{
				PrimaryAxisRates = rates;
			}
			else if ( axis == TelescopeAxes.axisSecondary )
			{
				SecondaryAxisRates = rates;
			}
			else if ( axis == TelescopeAxes.axisTertiary )
			{
				TertiaryAxisRates = rates;
			}
		}

		private TelescopeAxes _axis;
		private AxisRate[] _rates;
		private int _pos;

		internal AxisRates( TelescopeAxes Axis )
		{
			_axis = Axis;

			// This collection must hold zero or more Rate objects describing the 
			// rates of motion ranges for the Telescope.MoveAxis() method
			// that are supported by your driver. It is OK to leave this 
			// array empty, indicating that MoveAxis() is not supported.

			switch ( _axis )
			{
				case TelescopeAxes.axisPrimary:
					_rates = PrimaryAxisRates;
					break;

				case TelescopeAxes.axisSecondary:
					_rates = SecondaryAxisRates;
					break;

				case TelescopeAxes.axisTertiary:
					_rates = TertiaryAxisRates;
					break;
			}

			_pos = -1;
		}

		#region IAxisRates Members

		public int Count => _rates.Length;

		public IEnumerator GetEnumerator()
		{
			_pos = -1; //Reset pointer as this is assumed by .NET enumeration

			return ( _rates as IEnumerable ).GetEnumerator();
		}

		public IRate this[int index]
		{
			get
			{
				if ( index < 1 || index > this.Count )
				{
					throw new InvalidValueException( "AxisRates.index", index.ToString( CultureInfo.CurrentCulture ), string.Format( CultureInfo.CurrentCulture, "1 to {0}", this.Count ) );
				}

				return (IRate)_rates[index - 1];   // 1-based
			}
		}

		#endregion

		#region IEnumerator implementation

		public bool MoveNext()
		{
			if ( ++_pos >= _rates.Length )
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
				if ( _pos < 0 || _pos >= _rates.Length )
				{
					throw new System.InvalidOperationException();
				}

				return _rates[_pos];
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
				_rates = null;
			}
		}

		#endregion
	}

	internal class AxisRate : IRate, IDisposable, IEnumerable
	{
		private double _maximum = 0;
		private double _minimum = 0;

		internal AxisRate( double minimum, double maximum )
		{
			_maximum = maximum;
			_minimum = minimum;
		}

		#region IRate Members

		public IEnumerator GetEnumerator()
		{
			return null;
		}

		public double Maximum
		{
			get { return _maximum; }
			set { _maximum = value; }
		}

		public double Minimum
		{
			get { return _minimum; }
			set { _minimum = value; }
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
		{}

		#endregion

	}
}
