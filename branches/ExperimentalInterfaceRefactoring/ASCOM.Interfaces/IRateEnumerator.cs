//-----------------------------------------------------------------------
// <summary>Defines the IRateEnumerator Interface</summary>
//-----------------------------------------------------------------------
using System;
namespace ASCOM.Interfaces
{
    ///<summary>
    /// Strongly typed enumerator for late bound Rate
    /// objects being enumarated
    ///</summary>
    public interface IRateEnumerator
    {
        object Current { get; }
        void Dispose();
        bool MoveNext();
        void Reset();
    }
}
