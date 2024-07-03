//-----------------------------------------------------------------------
// <summary>Defines the Focuser class.</summary>
//-----------------------------------------------------------------------
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
// 29-May-10  	rem     6.0.0 - Added memberFactory.

using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Provides universal access to Focuser drivers
    /// </summary>
    public class Focuser : AscomDriver, IFocuserV4
    {
        private MemberFactory memberFactory;

        #region Focuser constructors

        /// <summary>
        /// Creates a focuser object with the given Prog ID
        /// </summary>
        /// <param name="focuserId">ProgID of the focuser device to be accessed.</param>
        public Focuser(string focuserId)
            : base(focuserId)
        {
            memberFactory = base.MemberFactory;
        }
		#endregion

		#region Convenience Members
		/// <summary>
		/// Brings up the ASCOM Chooser Dialogue to choose a Focuser
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

        /// <summary>
        /// Focuser device state
        /// </summary>
        public FocuserState FocuserState
        {
            get
            {
                // Create a state object to return.
                FocuserState focuserState = new FocuserState(DeviceState, TL);
                TL.LogMessage(nameof(FocuserState), $"Returning: '{focuserState.IsMoving}' '{focuserState.Position}' '{focuserState.Temperature}' '{focuserState.TimeStamp}'");

                // Return the device specific state class
                return focuserState;
            }
        }

        #endregion

        #region IFocuser Members

        /// <summary>
        /// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
        /// </summary>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        public bool Absolute
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "Absolute", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        /// Immediately stop any focuser motion due to a previous <see cref="Move" /> method call.
        /// </summary>
        /// <exception cref="MethodNotImplementedException">Focuser does not support this method.</exception>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <para>This method must be short-lived; it is defined as synchronous in this specification.</para>
        /// <para>Some focusers may not support this function, in which case an exception will be raised.</para>
        /// <para><b>Recommendation:</b> Host software should call this method upon initialization and,
        /// if it fails, disable the Halt button in the user interface.</para>
        /// </remarks>
        public void Halt()
        {
            memberFactory.CallMember(3, "Halt", new Type[] { }, new object[] { });
        }

		/// <summary>
		/// True if the focuser is currently moving to a new position. False if the focuser is stationary.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		public bool IsMoving
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "IsMoving", new Type[] { }, new object[] { })); }
        }
		/// <summary>
		/// State of the connection to the focuser.
		/// </summary>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <para>Set True to start the connection to the focuser; set False to terminate the connection. 
		/// The current connection status can also be read back through this property. 
		/// An exception will be raised if the link fails to change state for any reason. </para>
		/// <para><b>Note</b></para>
		/// <para>The FocuserV1 interface was the only interface to name its <i>"Connection"</i> property "Link" all others named 
		/// their <i>"Connection"</i> property as "Connected". All interfaces including Focuser now have a <see cref="AscomDriver.Connected" /> method and this is 
		/// the recommended method to use to <i>"Connect"</i> to Focusers exposing the V2 and later interfaces.</para>
		/// <para>Applications using DriverAccess can always use Connected because DriverAccess implements Connected using the Link property for V1 drivers but
		/// applications that call the driver directly should check the InterfaceVersion property and call Link for V1 drivers.</para>
		/// </remarks>
		public bool Link
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "Link", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "Link", new Type[] { }, new object[] { value }); }
        }

		/// <summary>
		/// Maximum increment size allowed by the focuser; 
		/// i.e. the maximum number of steps allowed in one move operation.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// For most focusers this is the same as the <see cref="MaxStep" /> property. This is normally used to limit the Increment 
		/// display in the host software.
		/// </remarks>
		public int MaxIncrement
        {
            get { return Convert.ToInt32(memberFactory.CallMember(1, "MaxIncrement", new Type[] { }, new object[] { })); }
        }

		/// <summary>
		/// Maximum step position permitted.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// The focuser can step between 0 and <see cref="MaxStep" />. If an attempt is made to move the focuser beyond these limits,
		/// it will automatically stop at the limit.
		/// </remarks>
		public int MaxStep
        {
            get { return Convert.ToInt32(memberFactory.CallMember(1, "MaxStep", new Type[] { }, new object[] { })); }
        }

        /// <summary>
        ///  Moves the focuser by the specified amount or to the specified position depending on the value of the <see cref="Absolute" /> property.
        /// </summary>
        /// <param name="Position">Step distance or absolute position, depending on the value of the <see cref="Absolute" /> property.</param>
        /// <exception cref="NotConnectedException">If the device is not connected.</exception>
        /// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
        /// <remarks>
        /// <para>This is <see langword="and"/>asynchronous method and must return as soon as the focus change operation has been successfully started, with the <see cref="IsMoving"/> property = True.
        /// After the requested position is successfully reached and motion stops, the <see cref="IsMoving"/> property becomes False.</para>
        /// <para>
        /// If the <see cref="Absolute" /> property is True, then this is an absolute positioning focuser. 
        /// The <see cref="Move">Move</see> command tells the focuser to move to an exact step position, and the Position parameter 
        /// of the <see cref="Move">Move</see> method is an integer between 0 and <see cref="MaxStep" />.</para>
        /// <para>If the <see cref="Absolute" /> property is False, then this is a relative positioning focuser. The <see cref="Move">Move</see> command tells 
        /// the focuser to move in a relative direction, and the Position parameter of the <see cref="Move">Move</see> method (in this case, step distance) 
        /// is an integer between minus <see cref="MaxIncrement" /> and plus <see cref="MaxIncrement" />.</para>
        /// <para><b>BEHAVIOURAL CHANGE - Platform 6.4</b></para>
        /// <para>Prior to Platform 6.4, the interface specification mandated that drivers must throw an <see cref="InvalidOperationException"/> if a move was attempted when <see cref="TempComp"/> was True, even if the focuser 
        /// was able to execute the move safely without disrupting temperature compensation.</para>
        /// <para>Following discussion on ASCOM-Talk in January 2018, the Focuser interface specification has been revised to IFocuserV3, removing the requirement to throw the InvalidOperationException exception. IFocuserV3 compliant drivers 
        /// are expected to execute Move requests when temperature compensation is active and to hide any specific actions required by the hardware from the client. For example this could be achieved by disabling temperature compensation, moving the focuser and re-enabling 
        /// temperature compensation or simply by moving the focuser with compensation enabled if the hardware supports this.</para>
        /// <para>Conform will continue to pass IFocuserV2 drivers that throw InvalidOperationException exceptions. However, Conform will now fail IFocuserV3 drivers that throw InvalidOperationException exceptions, in line with this revised specification.</para>
        ///</remarks>
        public void Move(int Position)
        {
            memberFactory.CallMember(3, "Move", new Type[] { typeof(int) }, new object[] { Position });
        }

		/// <summary>
		/// Current focuser position, in steps.
		/// </summary>
		/// <exception cref="PropertyNotImplementedException">Raises a PropertyNotImplemented if the focuser does not intrinsically know what the position is.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// Valid only for absolute positioning focusers (see the <see cref="Absolute" /> property).
		/// An exception will be raised for relative positioning focusers.
		/// </remarks>
		public int Position
        {
            get { return Convert.ToInt32(memberFactory.CallMember(1, "Position", new Type[] { }, new object[] { })); }
        }

		/// <summary>
		/// Step size (microns) for the focuser.
		/// </summary>
		/// <exception cref="PropertyNotImplementedException">Raises a PropertyNotImplemented if the focuser does not intrinsically know what the step size is.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// Must throw an exception if the focuser does not intrinsically know what the step size is.
		/// </remarks>
		public double StepSize
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "StepSize", new Type[] { }, new object[] { })); }
        }

		/// <summary>
		/// The state of temperature compensation mode (if available), else always False.
		/// </summary>
		/// <exception cref="PropertyNotImplementedException">If <see cref="TempCompAvailable" /> is False and an attempt is made to set <see cref="TempComp" /> to true.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		///<para>If the <see cref="TempCompAvailable" /> property is True, then setting <see cref="TempComp" /> to True puts the focuser into temperature tracking mode; setting it to False will turn off temperature tracking.
		/// An exception will be raised if <see cref="TempCompAvailable" /> is False and an attempt is made to set <see cref="TempComp" /> to true.</para>
		/// <para><b>BEHAVIOURAL CHANGE - Platform 6.4</b></para>
		/// <para>Prior to Platform 6.4, the interface specification mandated that drivers must throw an <see cref="InvalidOperationException"/> if a move was attempted when <see cref="TempComp"/> was True, even if the focuser 
		/// was able to execute the move safely without disrupting temperature compensation.</para>
		/// <para>Following discussion on ASCOM-Talk in January 2018, the Focuser interface specification has been revised to IFocuserV3, removing the requirement to throw the InvalidOperationException exception. IFocuserV3 compliant drivers 
		/// are expected to execute Move requests when temperature compensation is active and to hide any specific actions required by the hardware from the client. For example this could be achieved by disabling temperature compensation, moving the focuser and re-enabling 
		/// temperature compensation or simply by moving the focuser with compensation enabled if the hardware supports this.</para>
		/// <para>Conform will continue to pass IFocuserV2 drivers that throw InvalidOperationException exceptions. However, Conform will now fail IFocuserV3 drivers that throw InvalidOperationException exceptions, in line with this revised specification.</para>
		/// </remarks>
		public bool TempComp
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "TempComp", new Type[] { }, new object[] { })); }
            set { memberFactory.CallMember(2, "TempComp", new Type[] { }, new object[] { value }); }
        }

		/// <summary>
		/// True if focuser has temperature compensation available.
		/// </summary>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// Will be True only if the focuser's temperature compensation can be turned on and off via the <see cref="TempComp" /> property. 
		/// </remarks>
		public bool TempCompAvailable
        {
            get { return Convert.ToBoolean(memberFactory.CallMember(1, "TempCompAvailable", new Type[] { }, new object[] { })); }
        }

		/// <summary>
		/// Current ambient temperature in degrees Celsius as measured by the focuser.
		/// </summary>
		/// <exception cref="PropertyNotImplementedException">If the property is not available for this device.</exception>
		/// <exception cref="NotConnectedException">If the device is not connected.</exception>
		/// <exception cref="DriverException">An error occurred that is not described by one of the more specific ASCOM exceptions. The device did not successfully complete the request.</exception> 
		/// <remarks>
		/// <para>Raises an exception if ambient temperature is not available. Commonly available on focusers with a built-in temperature compensation mode.</para>
		/// <para><b>Clarification - October 2019</b></para>
		/// <para>Historically no units were specified for this property. Henceforth, if applications need to process the supplied temperature, they should proceed on the basis that the 
		/// units are degrees Celsius for consistency with <see cref="IObservingConditionsV2.Temperature"/>. Conversion to other temperature units can be achieved through the <see cref="Util.ConvertUnits"/> utility method.</para>
		/// </remarks>
		public double Temperature
        {
            get { return Convert.ToDouble(memberFactory.CallMember(1, "Temperature", new Type[] { }, new object[] { })); }
        }

        #endregion

    }
}
