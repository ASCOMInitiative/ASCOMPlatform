//-----------------------------------------------------------------------
// <summary>Defines the IAxisRates Interface</summary>
//-----------------------------------------------------------------------
using System;
using ASCOM.Interface;

namespace ASCOM.Interface
{
    public interface IAxisRatesV2 : ASCOM.Interface.IAxisRates
    {
        int Count { get; }
        void Dispose();
        System.Collections.IEnumerator GetEnumerator();
        IRatev2 this[int index] { get; }
    }

    public interface __IAxisRates
    {

        int Count { get; }
        System.Collections.IEnumerator GetEnumerator();
        IRatev2 this[int index] { get; }
        void Add(double Minimum, double Maximum);
    }

}
