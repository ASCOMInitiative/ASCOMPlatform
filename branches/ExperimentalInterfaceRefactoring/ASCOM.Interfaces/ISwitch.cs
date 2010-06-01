//-----------------------------------------------------------------------
// <summary>Defines the ISwitch Interface</summary>
//-----------------------------------------------------------------------

using System;
using System.Collections;
namespace ASCOM.Interfaces
{
    /// <summary>
    /// Defines the ISwitch Interface
    /// </summary> 
    public interface ISwitch : IDeviceControl, IAscomDriver
    {
        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Yields a collection of ISwitchDevice objects.
        /// </summary>
        ArrayList Switches { get; }

        /// <summary>
        /// Sets a switch to on or off
        /// </summary>
        /// <param name="Name">Name=name of switch to set</param> 
        /// <param name="State">True=On, False=Off</param> 
        void SetSwitch(string Name, bool State);
    }
}
