//-----------------------------------------------------------------------
// <summary>Defines the ITrackingRates Interface</summary>
//-----------------------------------------------------------------------
using System;
namespace ASCOM.Interfaces
{
    /// <summary>
    /// Defines the ITrackingRates Interface
    /// </summary>
    public interface ITrackingRates
    {
        int Count { get; }
        void Dispose();
        System.Collections.IEnumerator GetEnumerator();
        ASCOM.Interfaces.DriveRates this[int index] { get; }
    }
}
