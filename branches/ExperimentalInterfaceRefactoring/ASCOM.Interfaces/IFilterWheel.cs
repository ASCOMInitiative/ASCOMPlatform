using System;
namespace ASCOM.Interfaces
{
    public interface IFilterWheel : IAscomDriver, IDeviceControl
    {
        void Dispose();
        int[] FocusOffsets { get; }
        string[] Names { get; }
        short Position { get; set; }
    }
}
