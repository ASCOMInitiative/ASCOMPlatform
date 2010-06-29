//-----------------------------------------------------------------------
// <summary>Defines the ITrackingRates Interface</summary>
//-----------------------------------------------------------------------
using System;
namespace ASCOM.Interface
{
    /// <summary>
    /// Defines the ITrackingRates Interface
    /// </summary>
    public interface ITrackingRates
    {
        int Count { get; }
        void Dispose();
        System.Collections.IEnumerator GetEnumerator();
        ASCOM.Interface.DriveRates this[int index] { get; }
    }
}
