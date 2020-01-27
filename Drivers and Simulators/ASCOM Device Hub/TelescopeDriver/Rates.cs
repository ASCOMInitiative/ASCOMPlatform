using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

using ASCOM.DeviceInterface;

namespace ASCOM.DeviceHub
{
	#region Rate class

	// The Rate class implements IRate, and is used to hold values
	// for AxisRates. You do not need to change this class.
	//
	// The Guid attribute sets the CLSID for ASCOM.DeviceHub.Rate
	// The ClassInterface/None addribute prevents an empty interface called
	// _Rate from being created and used as the [default] interface

	[Guid( "bbb05daa-5a12-44f8-9c0b-f28ef8134ebd" )]
	[ClassInterface( ClassInterfaceType.None )]
	[ComVisible( true )]
	public class Rate : ASCOM.DeviceInterface.IRate
	{
		private double maximum = 0;
		private double minimum = 0;

		// Default constructor - Internal prevents public creation
		// of instances. These are values for AxisRates.

		internal Rate( double minimum, double maximum )
		{
			this.maximum = maximum;
			this.minimum = minimum;
		}

		#region Implementation of IRate

		public void Dispose()
		{
			// Add any required object cleanup here
		}

		public double Maximum
		{
			get { return this.maximum; }
			set { this.maximum = value; }
		}

		public double Minimum
		{
			get { return this.minimum; }
			set { this.minimum = value; }
		}

		#endregion Implementation of IRate
	}

	#endregion

	#region AxisRates

	// AxisRates is a strongly-typed collection that must be enumerable by
	// both COM and .NET. The IAxisRates and IEnumerable interfaces provide
	// this polymorphism. 

	// The Guid attribute sets the CLSID for ASCOM.DeviceHub.AxisRates
	// The ClassInterface/None addribute prevents an empty interface called
	// _AxisRates from being created and used as the [default] interface

	[Guid( "fd8657cf-a98f-4462-bb48-102d93480980" )]
	[ClassInterface( ClassInterfaceType.None )]
	[ComVisible( true )]
	public class AxisRates : IAxisRates, IEnumerable
	{
		private readonly TelescopeAxes _axis;
		private readonly Rate[] _rates;

		// Constructor - Internal prevents public creation
		// of instances. Returned by Telescope.AxisRates.

		internal AxisRates( TelescopeAxes axis )
		{
			IRate[] iRates = new IRate[0];

			this._axis = axis;

			// This collection must hold zero or more Rate objects describing the 
			// rates of motion ranges for the Telescope.MoveAxis() method
			// that are supported by your driver. It is OK to leave this 
			// array empty, indicating that MoveAxis() is not supported.
			//
			// Note that we are constructing a rate array for the axis passed
			// to the constructor. Thus we switch() below, and each case should 
			// initialize the array for the rate for the selected axis.

			switch ( axis )
			{
				case TelescopeAxes.axisPrimary:
					iRates = TelescopeManager.Instance.Capabilities.PrimaryAxisRates;

					break;

				case TelescopeAxes.axisSecondary:
					iRates = TelescopeManager.Instance.Capabilities.SecondaryAxisRates;

					break;

				case TelescopeAxes.axisTertiary:
					iRates = TelescopeManager.Instance.Capabilities.TertiaryAxisRates;

					break;
			}

			_rates = new Rate[iRates.Length];
			int ndx = 0;

			foreach ( IRate iRate in iRates )
			{
				_rates[ndx++] = new Rate( iRate.Minimum, iRate.Maximum );
			}
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

	#endregion AxisRates

	#region TrackingRates

	// TrackingRates is a strongly-typed collection that must be enumerable by
	// both COM and .NET. The ITrackingRates and IEnumerable interfaces provide
	// this polymorphism. 

	// The Guid attribute sets the CLSID for ASCOM.DeviceHub.TrackingRates
	// The ClassInterface/None addribute prevents an empty interface called
	// _TrackingRates from being created and used as the [default] interface
	//
	// This class is implemented in this way so that applications based on .NET 3.5
	// will work with this .NET 4.0 object.  Changes to this have proved to be challenging
	// and it is strongly suggested that it isn't changed.

	[Guid( "9923aa43-288e-495b-b821-d4f804f48a5a" )]
	[ClassInterface( ClassInterfaceType.None )]
	[ComVisible( true )]
	public class TrackingRates : ITrackingRates, IEnumerable, IEnumerator
	{
		private readonly DriveRates[] trackingRates;

		// this is used to make the index thread safe
		private readonly ThreadLocal<int> pos = new ThreadLocal<int>( () => { return -1; } );
		private static readonly object lockObj = new object();

		// Default constructor - Internal prevents public creation
		// of instances. Returned by Telescope.AxisRates.

		internal TrackingRates()
		{
			// This array must hold ONE or more DriveRates values, indicating
			// the tracking rates supported by your telescope. The one value
			// (tracking rate) that MUST be supported is driveSidereal!

			List<DriveRates> ratesList = new List<DriveRates>();

			foreach ( TrackingRateItem rateItem in TelescopeManager.Instance.Parameters.TrackingRates)
			{
				ratesList.Add( rateItem.Rate );
			}

			this.trackingRates = ratesList.ToArray();
		}

		#region ITrackingRates Members

		public int Count
		{
			get { return this.trackingRates.Length; }
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
			get { return this.trackingRates[index - 1]; }   // 1-based
		}

		#endregion

		#region IEnumerable members

		public object Current
		{
			get
			{
				lock ( lockObj )
				{
					if ( pos.Value < 0 || pos.Value >= trackingRates.Length )
					{
						throw new System.InvalidOperationException();
					}

					return trackingRates[pos.Value];
				}
			}
		}

		public bool MoveNext()
		{
			lock ( lockObj )
			{
				if ( ++pos.Value >= trackingRates.Length )
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

	#endregion Tracking Rates
}
