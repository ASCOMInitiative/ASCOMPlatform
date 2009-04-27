//
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
//
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Interface;
using ASCOM.HelperNET;

namespace ASCOM.DriverAccess
{
	#region Focuser wrapper
	/// <summary>
	/// Provides universal access to Focuser drivers
	/// </summary>
	public class Focuser : ASCOM.Interface.IFocuser, IDisposable
    {

        object objFocuserLateBound;
		ASCOM.Interface.IFocuser IFocuser;
		Type objTypeFocuser;
        /// <summary>
        /// Creates a focuser object with the given Prog ID
        /// </summary>
        /// <param name="focuserID"></param>
        public Focuser(string focuserID)
		{
			// Get Type Information 
            objTypeFocuser = Type.GetTypeFromProgID(focuserID);
			
			// Create an instance of the Focuser object
            objFocuserLateBound = Activator.CreateInstance(objTypeFocuser);

			// Try to see if this driver has an ASCOM.Focuser interface
			try
			{
				IFocuser = (ASCOM.Interface.IFocuser)objFocuserLateBound;
			}
			catch (Exception)
			{
				IFocuser = null;
			}

		}

        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a Focuser
        /// </summary>
        /// <param name="focuserID">Focuser Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen focuser or null for none</returns>
        public static string Choose(string focuserID)
        {
			Chooser oChooser = new Chooser();
			oChooser.DeviceType = "Focuser";			// Requires Helper 5.0.3 (May '07)
			return oChooser.Choose(focuserID);
		}


        #region IFocuser Members

        /// <summary>
        /// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
        /// </summary>
        public bool Absolute
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.Absolute;
				else
					return (bool)objTypeFocuser.InvokeMember("Absolute", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Immediately stop any focuser motion due to a previous Move() method call.
        /// Some focusers may not support this function, in which case an exception will be raised. 
        /// Recommendation: Host software should call this method upon initialization and,
        /// if it fails, disable the Halt button in the user interface. 
        /// </summary>
        public void Halt()
        {
			if (IFocuser != null)
				IFocuser.Halt();
			else
				objTypeFocuser.InvokeMember("Halt", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objFocuserLateBound, new object[] { });
        }

        /// <summary>
        /// True if the focuser is currently moving to a new position. False if the focuser is stationary.
        /// </summary>
        public bool IsMoving
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.IsMoving;
				else
					return (bool)objTypeFocuser.InvokeMember("IsMoving", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
        }
        /// <summary>
        /// State of the connection to the focuser.
        /// et True to start the link to the focuser; set False to terminate the link. 
        /// The current link status can also be read back as this property. 
        /// An exception will be raised if the link fails to change state for any reason. 
        /// </summary>
        public bool Link
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.Link;
				else
					return (bool)objTypeFocuser.InvokeMember("Link", 
						BindingFlags.Default | BindingFlags.GetProperty,
                    null, objFocuserLateBound, new object[] { });
            }
            set
            {
				if (IFocuser != null)
					IFocuser.Link = value;
				else
					objTypeFocuser.InvokeMember("Link", 
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objFocuserLateBound, new object[] { value });

            }
        }

        /// <summary>
        /// Maximum increment size allowed by the focuser; 
        /// i.e. the maximum number of steps allowed in one move operation.
        /// For most focusers this is the same as the MaxStep property.
        /// This is normally used to limit the Increment display in the host software. 
        /// </summary>
        public int MaxIncrement
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.MaxIncrement;
				else
					return (int)objTypeFocuser.InvokeMember("MaxIncrement", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Maximum step position permitted.
        /// The focuser can step between 0 and MaxStep. 
        /// If an attempt is made to move the focuser beyond these limits,
        /// it will automatically stop at the limit. 
        /// </summary>
        public int MaxStep
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.MaxStep;
				else
					return (int)objTypeFocuser.InvokeMember("MaxStep", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { }); 
            }
        }

        /// <summary>
        /// Step size (microns) for the focuser.
        /// Raises an exception if the focuser does not intrinsically know what the step size is. 
        /// </summary>
        /// <param name="val"></param>
        public void Move(int val)
        {
			if (IFocuser != null)
				IFocuser.Move(val);
			else
				objTypeFocuser.InvokeMember("Move", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objFocuserLateBound, new object[] { val });
        }

        /// <summary>
        /// Current focuser position, in steps.
        /// Valid only for absolute positioning focusers (see the Absolute property).
        /// An exception will be raised for relative positioning focusers.   
        /// </summary>
        public int Position
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.Position;
				else
					return (int)objTypeFocuser.InvokeMember("Position", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Display a dialog box for the user to enter in custom setup parameters, such as a COM port selection.
        /// If no dialog is required or supported, the method shall return immediately. 
        /// </summary>
        public void SetupDialog()
        {
			if (IFocuser != null)
				IFocuser.SetupDialog();
			else
				objTypeFocuser.InvokeMember("SetupDialog", 
					BindingFlags.Default | BindingFlags.InvokeMethod,
					null, objFocuserLateBound, new object[] { });
        }
        /// <summary>
        /// Step size (microns) for the focuser.
        /// Raises an exception if the focuser does not intrinsically know what the step size is. 
        /// 
        /// </summary>

        public double StepSize
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.StepSize;
				else
					return (double)objTypeFocuser.InvokeMember("StepSize", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
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
            get
            {
				if (IFocuser != null)
					return IFocuser.TempComp;
				else
					return (bool)objTypeFocuser.InvokeMember("TempComp", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
            set
            {
				if (IFocuser != null)
					IFocuser.TempComp = value;
				else
					objTypeFocuser.InvokeMember("TempComp",
						BindingFlags.Default | BindingFlags.SetProperty,
						null, objFocuserLateBound, new object[] { value } );

            }
        }

        /// <summary>
        /// True if focuser has temperature compensation available.
        /// Will be True only if the focuser's temperature compensation can be turned on and off via the TempComp property. 
        /// </summary>
        public bool TempCompAvailable
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.TempCompAvailable;
				else
					return (bool)objTypeFocuser.InvokeMember("TempCompAvailable", 
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Current ambient temperature as measured by the focuser.
        /// Raises an exception if ambient temperature is not available.
        /// Commonly available on focusers with a built-in temperature compensation mode. 
        /// </summary>
        public double Temperature
        {
            get
            {
				if (IFocuser != null)
					return IFocuser.Temperature;
				else
					return (double)objTypeFocuser.InvokeMember("Temperature",
						BindingFlags.Default | BindingFlags.GetProperty,
						null, objFocuserLateBound, new object[] { });
            }
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
			if (this.objFocuserLateBound != null)
			{
				try { Marshal.ReleaseComObject(objFocuserLateBound); }
				catch (Exception) { }
				objFocuserLateBound = null;
			}
		}

		#endregion

	}
	#endregion
}
