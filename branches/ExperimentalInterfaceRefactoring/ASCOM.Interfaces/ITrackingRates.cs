using System;
namespace ASCOM.Interfaces
{
    public interface ITrackingRates
    {
        int Count { get; }
        void Dispose();
        System.Collections.IEnumerator GetEnumerator();
        ASCOM.Interfaces.DriveRates this[int index] { get; }
    }
}
