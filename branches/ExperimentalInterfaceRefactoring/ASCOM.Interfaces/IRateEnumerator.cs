//-----------------------------------------------------------------------
// <summary>Defines the IRateEnumerator Interface</summary>
//-----------------------------------------------------------------------
using System;
namespace ASCOM.Interfaces
{
    public interface IRateEnumerator
    {
        object Current { get; }
        void Dispose();
        bool MoveNext();
        void Reset();
    }
}
