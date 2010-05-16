using System;
namespace ASCOM.Interfaces
{
    public interface IRotator : IAscomDriver, IDeviceControl
    {
        bool CanReverse { get; }
        void Dispose();
        void Halt();
        bool IsMoving { get; }
        void Move(float Position);
        void MoveAbsolute(float Position);
        float Position { get; }
        bool Reverse { get; set; }
        float StepSize { get; }
        float TargetPosition { get; }
    }
}
