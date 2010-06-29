//-----------------------------------------------------------------------
// <summary>Defines the IFocuser interface.</summary>
//-----------------------------------------------------------------------
using System;

namespace ASCOM.Interface
{
    /// <summary>
    /// Provides universal access to Focuser drivers
    /// </summary>
    public interface IFocuser : IAscomDriver, IDeviceControl
    {
        /// <summary>
        /// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
        /// </summary>
        bool Absolute { get; }

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Immediately stop any focuser motion due to a previous Move() method call.
        /// Some focusers may not support this function, in which case an exception will be raised. 
        /// Recommendation: Host software should call this method upon initialization and,
        /// if it fails, disable the Halt button in the user interface. 
        /// </summary>
        void Halt();

        /// <summary>
        /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
        /// </summary>
        bool IsMoving { get; }

        /// <summary>
        /// State of the connection to the focuser.
        /// et True to start the link to the focuser; set False to terminate the link. 
        /// The current link status can also be read back as this property. 
        /// An exception will be raised if the link fails to change state for any reason. 
        /// </summary>
        bool Link { get; set; }

        /// <summary>
        /// Maximum increment size allowed by the focuser; 
        /// i.e. the maximum number of steps allowed in one move operation.
        /// For most focusers this is the same as the MaxStep property.
        /// This is normally used to limit the Increment display in the host software. 
        /// </summary>
        int MaxIncrement { get; }

        /// <summary>
        /// Maximum step position permitted.
        /// The focuser can step between 0 and MaxStep. 
        /// If an attempt is made to move the focuser beyond these limits,
        /// it will automatically stop at the limit. 
        /// </summary>
        int MaxStep { get; }

        /// <summary>
        /// Step size (microns) for the focuser.
        /// Raises an exception if the focuser does not intrinsically know what the step size is. 
        /// </summary>
        /// <param name="val"></param>
        void Move(int val);

        /// <summary>
        /// Current focuser position, in steps.
        /// Valid only for absolute positioning focusers (see the Absolute property).
        /// An exception will be raised for relative positioning focusers.   
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Step size (microns) for the focuser.
        /// Raises an exception if the focuser does not intrinsically know what the step size is. 
        /// 
        /// </summary>
        double StepSize { get; }

        /// <summary>
        /// The state of temperature compensation mode (if available), else always False.
        /// If the TempCompAvailable property is True, then setting TempComp to True
        /// puts the focuser into temperature tracking mode. While in temperature tracking mode,
        /// Move commands will be rejected by the focuser. Set to False to turn off temperature tracking.
        /// An exception will be raised if TempCompAvailable is False and an attempt is made to set TempComp to true. 
        /// 
        /// </summary>
        bool TempComp { get; set; }

        /// <summary>
        /// True if focuser has temperature compensation available.
        /// Will be True only if the focuser's temperature compensation can be turned on and off via the TempComp property. 
        /// </summary>
        bool TempCompAvailable { get; }

        /// <summary>
        /// Current ambient temperature as measured by the focuser.
        /// Raises an exception if ambient temperature is not available.
        /// Commonly available on focusers with a built-in temperature compensation mode. 
        /// </summary>
        double Temperature { get; }
    }
}
