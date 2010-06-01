//-----------------------------------------------------------------------
// <summary>Defines the IAxisRates Interface</summary>
//-----------------------------------------------------------------------
using System;
namespace ASCOM.Interfaces
{
    public interface IAxisRates
    {
        int Count { get; }
        void Dispose();
        System.Collections.IEnumerator GetEnumerator();
        IRate this[int index] { get; }
    }
}
