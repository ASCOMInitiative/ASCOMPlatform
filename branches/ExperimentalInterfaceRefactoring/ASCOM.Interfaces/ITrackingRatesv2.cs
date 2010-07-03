//-----------------------------------------------------------------------
// <summary>Defines the ITrackingRates Interface</summary>
//-----------------------------------------------------------------------
using System;
using ASCOM.Interface;

namespace ASCOM.Interface
{
    /// <summary>
    /// Defines the ITrackingRates Interface
    /// </summary>
    public interface ITrackingRatesv2 : ASCOM.Interface.ITrackingRates
    {
        int Count { get; }
        void Dispose();
        System.Collections.IEnumerator GetEnumerator();
        ASCOM.Interface.DriveRates this[int index] { get; }
    }
}
