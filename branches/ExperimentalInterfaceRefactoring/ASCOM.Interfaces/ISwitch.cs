using System;
using System.Collections;
namespace ASCOM.Interfaces
{
    public interface ISwitch : IDeviceControl, IAscomDriver
    {
        void Dispose();
        /// <summary>
        /// Yields a collection of ISwitchDevice objects.
        /// </summary>
        ArrayList Switches { get; }
        /// <summary>
        /// True = on, False = off.
        /// </summary>
        void SetSwitch(string Name);
    }
}
