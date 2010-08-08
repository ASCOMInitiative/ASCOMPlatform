//-----------------------------------------------------------------------
// <summary>Defines the Focuser class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using ASCOM.Interface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{

    #region Focuser wrapper

    /// <summary>
    /// Provides universal access to Focuser drivers
    /// </summary>
    public class Focuser : IFocuser, IDisposable
    {
        #region IFocuser constructors

        private readonly MemberFactory _memberFactory;

        /// <summary>
        /// Creates a focuser object with the given Prog ID
        /// </summary>
        /// <param name="focuserId"></param>
        public Focuser(string focuserId)
        {
            _memberFactory = new MemberFactory(focuserId);
        }

        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a Focuser
        /// </summary>
        /// <param name="focuserId">Focuser Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen focuser or null for none</returns>
        public static string Choose(string focuserId)
        {
            var oChooser = new Chooser {DeviceType = "Focuser"};
            return oChooser.Choose(focuserId);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        public void Dispose()
        {
            _memberFactory.Dispose();
        }

        #endregion

        #region IFocuser Members

        /// <summary>
        /// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
        /// </summary>
        public bool Absolute
        {
            get { return (bool) _memberFactory.CallMember(1, "Absolute", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        /// Immediately stop any focuser motion due to a previous Move() method call.
        /// Some focusers may not support this function, in which case an exception will be raised. 
        /// Recommendation: Host software should call this method upon initialization and,
        /// if it fails, disable the Halt button in the user interface. 
        /// </summary>
        public void Halt()
        {
            _memberFactory.CallMember(3, "Halt", new Type[] {}, new object[] {});
        }

        /// <summary>
        /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
        /// </summary>
        public bool IsMoving
        {
            get { return (bool) _memberFactory.CallMember(1, "IsMoving", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        /// State of the connection to the focuser.
        /// et True to start the link to the focuser; set False to terminate the link. 
        /// The current link status can also be read back as this property. 
        /// An exception will be raised if the link fails to change state for any reason. 
        /// </summary>
        public bool Link
        {
            get { return (bool) _memberFactory.CallMember(1, "Link", new Type[] {}, new object[] {}); }
            set { _memberFactory.CallMember(2, "Link", new Type[] {}, new object[] {value}); }
        }

        /// <summary>
        /// Maximum increment size allowed by the focuser; 
        /// i.e. the maximum number of steps allowed in one move operation.
        /// For most focusers this is the same as the MaxStep property.
        /// This is normally used to limit the Increment display in the host software. 
        /// </summary>
        public int MaxIncrement
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "MaxIncrement", new Type[] {}, new object[] {})); }
        }

        /// <summary>
        /// Maximum step position permitted.
        /// The focuser can step between 0 and MaxStep. 
        /// If an attempt is made to move the focuser beyond these limits,
        /// it will automatically stop at the limit. 
        /// </summary>
        public int MaxStep
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "MaxStep", new Type[] {}, new object[] {})); }
        }

        /// <summary>
        /// Step size (microns) for the focuser.
        /// Raises an exception if the focuser does not intrinsically know what the step size is. 
        /// </summary>
        /// <param name="val"></param>
        public void Move(int val)
        {
            _memberFactory.CallMember(3, "Move", new[] {typeof (int)}, new object[] {val});
        }

        /// <summary>
        /// Current focuser position, in steps.
        /// Valid only for absolute positioning focusers (see the Absolute property).
        /// An exception will be raised for relative positioning focusers.   
        /// </summary>
        public int Position
        {
            get { return Convert.ToInt32(_memberFactory.CallMember(1, "Position", new Type[] {}, new object[] {})); }
        }

        /// <summary>
        /// Display a dialog box for the user to enter in custom setup parameters, such as a COM port selection.
        /// If no dialog is required or supported, the method shall return immediately. 
        /// </summary>
        public void SetupDialog()
        {
            _memberFactory.CallMember(3, "SetupDialog", new Type[] {}, new object[] {});
        }

        /// <summary>
        /// Step size (microns) for the focuser.
        /// Raises an exception if the focuser does not intrinsically know what the step size is. 
        /// 
        /// </summary>
        public double StepSize
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "StepSize", new Type[] {}, new object[] {})); }
        }

        /// <summary>
        /// The state of temperature compensation mode (if available), else always False.
        /// If the TempCompAvailable property is True, then setting TempComp to True
        /// puts the focuser into temperature tracking mode. While in temperature tracking mode,
        /// Move commands will be rejected by the focuser. Set to False to turn off temperature tracking.
        /// An exception will be raised if TempCompAvailable is False and an attempt is made to set TempComp to true. 
        /// 
        /// </summary>
        public bool TempComp
        {
            get { return (bool) _memberFactory.CallMember(1, "TempComp", new Type[] {}, new object[] {}); }
            set { _memberFactory.CallMember(2, "TempComp", new Type[] {}, new object[] {value}); }
        }

        /// <summary>
        /// True if focuser has temperature compensation available.
        /// Will be True only if the focuser's temperature compensation can be turned on and off via the TempComp property. 
        /// </summary>
        public bool TempCompAvailable
        {
            get { return (bool) _memberFactory.CallMember(1, "TempCompAvailable", new Type[] {}, new object[] {}); }
        }

        /// <summary>
        /// Current ambient temperature as measured by the focuser.
        /// Raises an exception if ambient temperature is not available.
        /// Commonly available on focusers with a built-in temperature compensation mode. 
        /// </summary>
        public double Temperature
        {
            get { return Convert.ToDouble(_memberFactory.CallMember(1, "Temperature", new Type[] {}, new object[] {})); }
        }

        #endregion
    }

    #endregion
}