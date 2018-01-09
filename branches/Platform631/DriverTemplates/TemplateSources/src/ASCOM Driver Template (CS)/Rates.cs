using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using System.Collections;
using System.Threading;

namespace ASCOM.TEMPLATEDEVICENAME
{
    #region Rate class
    //
	// The Rate class implements IRate, and is used to hold values
	// for AxisRates. You do not need to change this class.
	//
    // The Guid attribute sets the CLSID for ASCOM.TEMPLATEDEVICENAME.Rate
	// The ClassInterface/None addribute prevents an empty interface called
	// _Rate from being created and used as the [default] interface
	//
    [Guid("AD6248B3-3F51-4FFF-B62B-E3E942DD817E")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Rate : ASCOM.DeviceInterface.IRate
    {
        private double maximum = 0;
        private double minimum = 0;

        //
        // Default constructor - Internal prevents public creation
        // of instances. These are values for AxisRates.
        //
        internal Rate(double minimum, double maximum)
        {
            this.maximum = maximum;
            this.minimum = minimum;
        }

        #region Implementation of IRate

        public void Dispose()
        {
            throw new System.NotImplementedException();
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

        #endregion
    }
    #endregion

    #region AxisRates
    //
	// AxisRates is a strongly-typed collection that must be enumerable by
	// both COM and .NET. The IAxisRates and IEnumerable interfaces provide
	// this polymorphism. 
	//
    // The Guid attribute sets the CLSID for ASCOM.TEMPLATEDEVICENAME.AxisRates
	// The ClassInterface/None addribute prevents an empty interface called
	// _AxisRates from being created and used as the [default] interface
	//
    [Guid("99DB28A6-0132-43BF-91C0-D723124813C8")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class AxisRates : IAxisRates, IEnumerable
    {
        private TelescopeAxes axis;
        private readonly Rate[] rates;

        //
        // Constructor - Internal prevents public creation
        // of instances. Returned by Telescope.AxisRates.
        //
        internal AxisRates(TelescopeAxes axis)
        {
            this.axis = axis;
            //
            // This collection must hold zero or more Rate objects describing the 
            // rates of motion ranges for the Telescope.MoveAxis() method
            // that are supported by your driver. It is OK to leave this 
            // array empty, indicating that MoveAxis() is not supported.
            //
            // Note that we are constructing a rate array for the axis passed
            // to the constructor. Thus we switch() below, and each case should 
            // initialize the array for the rate for the selected axis.
            //
            switch (axis)
            {
                case TelescopeAxes.axisPrimary:
                    // TODO Initialize this array with any Primary axis rates that your driver may provide
                    // Example: m_Rates = new Rate[] { new Rate(10.5, 30.2), new Rate(54.0, 43.6) }
                    this.rates = new Rate[0];
                    break;
                case TelescopeAxes.axisSecondary:
                    // TODO Initialize this array with any Secondary axis rates that your driver may provide
                    this.rates = new Rate[0];
                    break;
                case TelescopeAxes.axisTertiary:
                    // TODO Initialize this array with any Tertiary axis rates that your driver may provide
                    this.rates = new Rate[0];
                    break;
            }
        }

        #region IAxisRates Members

        public int Count
        {
            get { return this.rates.Length; }
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return rates.GetEnumerator();
        }

        public IRate this[int index]
        {
            get { return this.rates[index - 1]; }	// 1-based
        }

        #endregion
    }
    #endregion

    #region TrackingRates
	//
	// TrackingRates is a strongly-typed collection that must be enumerable by
	// both COM and .NET. The ITrackingRates and IEnumerable interfaces provide
	// this polymorphism. 
	//
    // The Guid attribute sets the CLSID for ASCOM.TEMPLATEDEVICENAME.TrackingRates
	// The ClassInterface/None addribute prevents an empty interface called
	// _TrackingRates from being created and used as the [default] interface
	//
    // This class is implemented in this way so that applications based on .NET 3.5
    // will work with this .NET 4.0 object.  Changes to this have proved to be challenging
    // and it is strongly suggested that it isn't changed.
    //
    [Guid("49A4CA43-46B2-4D66-B9D3-FBE3ABE13DEB")]
	[ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
	public class TrackingRates : ITrackingRates, IEnumerable, IEnumerator
	{
		private readonly DriveRates[] trackingRates;

        // this is used to make the index thread safe
        private readonly ThreadLocal<int> pos = new ThreadLocal<int>(() => { return -1; });
        private static readonly object lockObj = new object();

		//
		// Default constructor - Internal prevents public creation
		// of instances. Returned by Telescope.AxisRates.
		//
		internal TrackingRates() 
		{
			//
			// This array must hold ONE or more DriveRates values, indicating
			// the tracking rates supported by your telescope. The one value
			// (tracking rate) that MUST be supported is driveSidereal!
			//
            this.trackingRates = new[] { DriveRates.driveSidereal };	
			// TODO Initialize this array with any additional tracking rates that your driver may provide
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
            throw new System.NotImplementedException();
        }

        public DriveRates this[int index]
		{
            get { return this.trackingRates[index - 1]; }	// 1-based
		}

		#endregion

        #region IEnumerable members

        public object Current
        {
            get
            {
                lock (lockObj)
                {
                    if (pos.Value < 0 || pos.Value >= trackingRates.Length)
                    {
                        throw new System.InvalidOperationException();
                    }
                    return trackingRates[pos.Value];
                }
            }
        }

        public bool MoveNext()
        {
            lock (lockObj)
            {
                if (++pos.Value >= trackingRates.Length)
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
    #endregion
}
