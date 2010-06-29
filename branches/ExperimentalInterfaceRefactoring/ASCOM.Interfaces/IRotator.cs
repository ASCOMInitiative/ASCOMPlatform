//-----------------------------------------------------------------------
// <summary>Defines the IRotator Interface</summary>
//-----------------------------------------------------------------------
using System;
namespace ASCOM.Interface
{
    /// <summary>
    /// Defines the IRotator Interface
    /// </summary>
    public interface IRotator : IAscomDriver, IDeviceControl
    {
        /// <summary>
        /// Returns True if the Rotator supports the Rotator.Reverse() method.
        /// </summary>
        bool CanReverse { get; }

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Immediately stop any Rotator motion due to a previous Move() or MoveAbsolute() method call.
        /// </summary>
        void Halt();

        /// <summary>
        /// True if the Rotator is currently moving to a new position. False if the Rotator is stationary.
        /// </summary>
        bool IsMoving { get; }

        /// <summary>
        /// Causes the rotator to move Position degrees relative to the current Position value.
        /// </summary>
        /// <param name="Position">Relative position to move in degrees from current Position.</param>
        void Move(float Position);

        /// <summary>
        /// Causes the rotator to move the absolute position of Position degrees.
        /// </summary>
        /// <param name="Position">absolute position in degrees.</param>
        void MoveAbsolute(float Position);

        /// <summary>
        /// Current instantaneous Rotator position, in degrees.
        /// </summary>
        float Position { get; }

        /// <summary>
        /// Sets or Returns the rotator’s Reverse state.
        /// </summary>
        bool Reverse { get; set; }

        /// <summary>
        /// The minimum StepSize, in degrees.
        /// </summary>
        float StepSize { get; }

        /// <summary>
        /// Current Rotator target position, in degrees.
        /// </summary>
        float TargetPosition { get; }

    }
}
