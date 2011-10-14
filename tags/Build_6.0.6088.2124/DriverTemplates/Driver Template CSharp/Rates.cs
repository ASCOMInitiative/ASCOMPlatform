using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using System.Collections;

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
        private double _maximum = 0;
        private double _minimum = 0;

        //
        // Default constructor - Internal prevents public creation
        // of instances. These are values for AxisRates.
        //
        internal Rate(double minimum, double maximum)
        {
            _maximum = maximum;
            _minimum = minimum;
        }

        #region Implementation of IRate

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public double Maximum
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public double Minimum
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
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
        private TelescopeAxes _axis;
        private readonly Rate[] _rates;

        //
        // Constructor - Internal prevents public creation
        // of instances. Returned by Telescope.AxisRates.
        //
        internal AxisRates(TelescopeAxes axis)
        {
            _axis = axis;
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
                    _rates = new Rate[0];
                    break;
                case TelescopeAxes.axisSecondary:
                    // TODO Initialize this array with any Secondary axis rates that your driver may provide
                    _rates = new Rate[0];
                    break;
                case TelescopeAxes.axisTertiary:
                    // TODO Initialize this array with any Tertiary axis rates that your driver may provide
                    _rates = new Rate[0];
                    break;
            }
        }

        #region IAxisRates Members

        public int Count
        {
            get { return _rates.Length; }
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return _rates.GetEnumerator();
        }

        public IRate this[int index]
        {
            get { return _rates[index - 1]; }	// 1-based
        }

        #endregion

        System.Collections.IEnumerator IAxisRates.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
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
    [Guid("49A4CA43-46B2-4D66-B9D3-FBE3ABE13DEB")]
	[ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
	public class TrackingRates : ITrackingRates, IEnumerable
	{
		private readonly DriveRates[] _trackingRates;

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
            _trackingRates = new[] { DriveRates.driveSidereal };	
			// TODO Initialize this array with any additional tracking rates that your driver may provide
		}

		#region ITrackingRates Members

		public int Count
		{
            get { return _trackingRates.Length; }
		}

		public IEnumerator GetEnumerator()
		{
            return _trackingRates.GetEnumerator();
		}

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public DriveRates this[int index]
		{
            get { return _trackingRates[index - 1]; }	// 1-based
		}

		#endregion
    }
    #endregion
}
