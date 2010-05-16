using System;
namespace ASCOM.Interfaces
{
    public interface IFocuser : IAscomDriver, IDeviceControl
    {
        bool Absolute { get; }
        void Dispose();
        void Halt();
        bool IsMoving { get; }
        bool Link { get; set; }
        int MaxIncrement { get; }
        int MaxStep { get; }
        void Move(int val);
        int Position { get; }
        double StepSize { get; }
        bool TempComp { get; set; }
        bool TempCompAvailable { get; }
        double Temperature { get; }
    }
}
