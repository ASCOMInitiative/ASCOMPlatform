//-----------------------------------------------------------------------
// <summary>Defines the IAxisRates Interface</summary>
//-----------------------------------------------------------------------
using System;
namespace ASCOM.Interface
{
    public interface IAxisRates
    {
        int Count { get; }
        void Dispose();
        System.Collections.IEnumerator GetEnumerator();
        IRate this[int index] { get; }
    }

    public interface __IAxisRates
    {
        int Count { get; }
        System.Collections.IEnumerator GetEnumerator();
        IRate this[int index] { get; }
        void Add(double Minimum, double Maximum);
    }

}
