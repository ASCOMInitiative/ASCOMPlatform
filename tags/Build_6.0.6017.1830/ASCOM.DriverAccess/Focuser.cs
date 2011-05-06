//-----------------------------------------------------------------------
// <summary>Defines the Focuser class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Globalization;

namespace ASCOM.DriverAccess
{
	#region Focuser wrapper
	/// <summary>
	/// Provides universal access to Focuser drivers
	/// </summary>
    public class Focuser : AscomDriver, IFocuserV2
    {
        #region Focuser constructors
        private MemberFactory memberFactory;

        /// <summary>
        /// Creates a focuser object with the given Prog ID
        /// </summary>
        /// <param name="focuserId">ProgID of the focuser device to be accessed.</param>
        public Focuser(string focuserId) : base(focuserId)
		{
            memberFactory = base.MemberFactory;
		}
        #endregion

        #region Convenience Members
        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a Focuser
        /// </summary>
        /// <param name="focuserId">Focuser Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen focuser or null for none</returns>
        public static string Choose(string focuserId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Focuser";
                return chooser.Choose(focuserId);
            }
        }

	    #endregion

        #region IFocuser Members

        /// <summary>
        /// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
        /// </summary>
        public bool Absolute
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "Absolute", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Immediately stop any focuser motion due to a previous Move() method call.
        /// </summary>
        /// <remarks>
        /// Some focusers may not support this function, in which case an exception will be raised. 
        /// Recommendation: Host software should call this method upon initialization and,
        /// if it fails, disable the Halt button in the user interface. 
        /// </remarks>
        public void Halt()
        {
            memberFactory.CallMember(3, "Halt", new Type[] { }, new object[] { });
        }

        /// <summary>
        /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
        /// </summary>
        public bool IsMoving
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "IsMoving", new Type[] { }, new object[] { })); }
        }
        /// <summary>
        /// State of the connection to the focuser.
        /// </summary>
        /// <remarks>
        /// Set True to start the link to the focuser; set False to terminate the link. 
        /// The current link status can also be read back as this property. 
        /// An exception will be raised if the link fails to change state for any reason. 
        /// </remarks>
        public bool Link
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "Link", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "Link", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// Maximum increment size allowed by the focuser; 
        /// </summary>
        /// <remarks>i.e. the maximum number of steps allowed in one move operation.
        /// For most focusers this is the same as the MaxStep property.
        /// This is normally used to limit the Increment display in the host software. 
        /// </remarks>
        public int MaxIncrement
        {
            get { return Convert.ToInt32(memberFactory.CallMember(1, "MaxIncrement", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Maximum step position permitted.
        /// </summary>
        /// <remarks>The focuser can step between 0 and MaxStep. 
        /// If an attempt is made to move the focuser beyond these limits,
        /// it will automatically stop at the limit. 
        /// </remarks>
        public int MaxStep
        {
            get { return Convert.ToInt32(memberFactory.CallMember(1, "MaxStep", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        ///  Moves the focuser by the specified amount or to the specified position depending on the value of the Absolute property.
        /// </summary>
        /// <param name="Value">Step distance or absolute position, depending on the value of the Absolute property.</param>
        /// <remarks>If the Absolute property is True, then this is an absolute positioning focuser. The Move command tells the focuser 
        /// to move to an exact step position, and the Position parameter of the Move() method is an integer between 0 and MaxStep.
        /// <para>If the Absolute property is False, then this is a relative positioning focuser. The Move command tells the focuser to move in a relative direction, and the Position parameter of the Move() method (in this case, step distance) is an integer between minus MaxIncrement and plus MaxIncrement.</para>
        ///</remarks>
        public void Move(int Value)
        {
            memberFactory.CallMember(3, "Move", new Type[] { typeof(int) }, new object[] { Value });
        }

        /// <summary>
        /// Current focuser position, in steps.
        /// </summary>
        /// <remarks>Valid only for absolute positioning focusers (see the Absolute property).
        /// An exception will be raised for relative positioning focusers.   
        /// </remarks>
        public int Position
        {
            get { return Convert.ToInt32(memberFactory.CallMember(1, "Position", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Step size (microns) for the focuser.
        /// </summary>
        /// <remarks>Raises an exception if the focuser does not intrinsically know what the step size is. </remarks>
        public double StepSize
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "StepSize", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// The state of temperature compensation mode (if available), else always False.
        /// </summary>
        /// <remarks>If the TempCompAvailable property is True, then setting TempComp to True
        /// puts the focuser into temperature tracking mode. While in temperature tracking mode,
        /// Move commands will be rejected by the focuser. Set to False to turn off temperature tracking.
        /// An exception will be raised if TempCompAvailable is False and an attempt is made to set TempComp to true. </remarks>
        public bool TempComp
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "TempComp", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "TempComp", new Type[] { }, new object[] { value }); }
        }

        /// <summary>
        /// True if focuser has temperature compensation available.
        /// </summary>
        /// <remarks>Will be True only if the focuser's temperature compensation can be turned on and off via the TempComp property.</remarks>
        public bool TempCompAvailable
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "TempCompAvailable", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Current ambient temperature as measured by the focuser.
        /// </summary>
        /// <remarks>Raises an exception if ambient temperature is not available.
        /// Commonly available on focusers with a built-in temperature compensation mode.</remarks>
        public double Temperature
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Temperature", new Type[] { }, new object[] { })); }
        }

        #endregion
	}
	#endregion
}
