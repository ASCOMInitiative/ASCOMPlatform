using System;
using System.Collections;
using System.Runtime.InteropServices;
using ASCOM.Common;
using ASCOM.Common.Interfaces;
using ASCOM.DeviceInterface;

namespace ASCOM.DynamicClients
{
    //
    // AxisRates is a strongly-typed collection that must be enumerable by
    // both COM and .NET. The IAxisRates and IEnumerable interfaces provide
    // this polymorphism. 
    //
    // The Guid attribute sets the CLSID for ADynamicRemoteClients AxisRates class
    // The ClassInterface/None attribute prevents an empty interface called
    // _AxisRates from being created and used as the [default] interface
    //
    [Guid("29F65429-4492-4323-9746-F718EBCD8FF3")]
    [ClassInterface(ClassInterfaceType.None), ComVisible(true)]
    public class AxisRates : IAxisRates, IEnumerable, IEnumerator, IDisposable
    {
        private TelescopeAxes m_axis;
        private Rate[] m_Rates;
        private int pos;

        //
        // Constructor - Internal prevents public creation
        // of instances. Returned by Telescope.AxisRates.
        //
        internal AxisRates(TelescopeAxes Axis)
        {
            m_axis = Axis;
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
            switch (m_axis)
            {
                case TelescopeAxes.axisPrimary:
                case TelescopeAxes.axisSecondary:
                case TelescopeAxes.axisTertiary:
                    m_Rates = new Rate[] { };
                    break;
            }
            pos = -1;
        }

        internal void Add(double Minium, double Maximum, ILogger TL)
        {
            TL.LogMessage(LogLevel.Debug, "AxisRates.Add", "Before m_Rates.Length: " + m_Rates.Length, includeLib: false);
            Rate r = new Rate(Minium, Maximum); // Create a new rate to add to the new array
            Rate[] NewRateArray = new Rate[m_Rates.Length + 1]; // Create a new Rate array to replace the current one
            TL.LogMessage(LogLevel.Debug, "AxisRates.Add", "NewRateArray.Length: " + NewRateArray.Length, includeLib: false);
            Array.Copy(m_Rates, NewRateArray, m_Rates.Length); // Copy the current contents of the m_Rated array to the new array
            NewRateArray[m_Rates.Length] = r; // Add the new rate the new Rates array.
            m_Rates = NewRateArray; // Make m_Rates point at the new larger array
            TL.LogMessage(LogLevel.Debug, "AxisRates.Add", "After m_Rates.Length: " + m_Rates.Length, includeLib: false);
        }
        #region IAxisRates Members

        public int Count
        {
            get { return m_Rates.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            pos = -1; //Reset pointer as this is assumed by .NET enumeration
            return this as IEnumerator;
        }

        public IRate this[int index]
        {
            get
            {
                if (index < 1 || index > this.Count)
                    throw new InvalidValueException("AxisRates.Index", index.ToString(), string.Format("1 to {0}", this.Count));
                return m_Rates[index - 1]; 	// 1-based
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                m_Rates = null;
            }
        }

        #endregion

        #region IEnumerator implementation

        public bool MoveNext()
        {
            if (++pos >= m_Rates.Length) return false;
            return true;
        }

        public void Reset()
        {
            pos = -1;
        }

        public object Current
        {
            get
            {
                if (pos < 0 || pos >= m_Rates.Length) throw new System.InvalidOperationException(string.Format("ASCOM DynamicRemoteClient AxisRates.Current - Pointer value {0} is outside expected range 0..{1}", pos, m_Rates.Length));
                return m_Rates[pos];
            }
        }

        #endregion
    }
}
