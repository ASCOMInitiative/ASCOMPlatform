//
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
//
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Interface;
using ASCOM.Helper;

namespace ASCOM.DriverAccess
{
    #region Rotator wrapper
    /// <summary>
    /// Provides universal access to Rotator drivers
    /// </summary>
    public class Rotator : ASCOM.Interface.IRotator, IDisposable
    {
        object objRotatorLateBound;
        ASCOM.Interface.IRotator IRotator;
        Type objTypeRotator;

        /// <summary>
        /// Creates a rotator object with the given Prog ID
        /// </summary>
        /// <param name="rotatorID"></param>
        public Rotator(string rotatorID)
		{
			// Get Type Information 
            objTypeRotator = Type.GetTypeFromProgID(rotatorID);

            // Create an instance of the Rotator object
            objRotatorLateBound = Activator.CreateInstance(objTypeRotator);

            // Try to see if this driver has an ASCOM.Rotator interface
			try
			{
                IRotator = (ASCOM.Interface.IRotator)objRotatorLateBound;
			}
			catch (Exception)
			{
                IRotator = null;
			}

		}

        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a Rotator
        /// </summary>
        /// <param name="rotatorID">Focuser Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen Rotator or null for none</returns>
        public static string Choose(string rotatorID)
        {
            try
            {
                Chooser oChooser = new Chooser();
                oChooser.DeviceTypeV = "Rotator";			// Requires Helper 5.0.3 (May '07)
                return oChooser.Choose(rotatorID);
            }
            catch
            {
                return "";
            }
        }

        #region IRotator Members

        /// <summary>
        /// Returns True if the Rotator supports the Rotator.Reverse() method.
        /// </summary>
        public bool CanReverse
        {
            get
            {
                if (IRotator != null)
                    return IRotator.CanReverse;
                else
                    return Convert.ToBoolean(objTypeRotator.InvokeMember("CanReverse",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Set True to start the Connected to the Rotator; set False to terminate the Connected.
        /// The current Connected status can also be read back as this property.
        /// An exception will be raised if the Connected fails to change state for any reason.
        /// </summary>
        public bool Connected
        {
            get
            {
                if (IRotator != null)
                    return IRotator.Connected;
                else
                    return Convert.ToBoolean(objTypeRotator.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { }));
            }
            set
            {
                if (IRotator != null)
                    IRotator.Connected = value;
                else
                    objTypeRotator.InvokeMember("Connected",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objRotatorLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Immediately stop any Rotator motion due to a previous Move() or MoveAbsolute() method call.
        /// </summary>
        public void Halt()
        {
            if (IRotator != null)
                IRotator.Halt();
            else
                objTypeRotator.InvokeMember("Halt",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objRotatorLateBound, new object[] { });
        }

        /// <summary>
        /// True if the Rotator is currently moving to a new position. False if the Rotator is stationary.
        /// </summary>
        public bool IsMoving
        {
            get
            {
                if (IRotator != null)
                    return IRotator.IsMoving;
                else
                    return Convert.ToBoolean(objTypeRotator.InvokeMember("IsMoving",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Causes the rotator to move Position degrees relative to the current Position value.
        /// </summary>
        /// <param name="Position">Relative position to move in degrees from current Position.</param>
        public void Move(float Position)
        {
            if (IRotator != null)
                IRotator.Move(Position);
            else
                objTypeRotator.InvokeMember("Move",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objRotatorLateBound, new object[] { Position });
        }

        /// <summary>
        /// Causes the rotator to move the absolute position of Position degrees.
        /// </summary>
        /// <param name="Position">absolute position in degrees.</param>
        public void MoveAbsolute(float Position)
        {
            if (IRotator != null)
                IRotator.MoveAbsolute(Position);
            else
                objTypeRotator.InvokeMember("MoveAbsolute",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objRotatorLateBound, new object[] { Position });
        }

        /// <summary>
        /// Current instantaneous Rotator position, in degrees.
        /// </summary>
        public float Position
        {
            get
            {
                if (IRotator != null)
                    return IRotator.Position;
                else
                    return Convert.ToSingle(objTypeRotator.InvokeMember("Position",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Sets or Returns the rotator’s Reverse state.
        /// </summary>
        public bool Reverse
        {
            get
            {
                if (IRotator != null)
                    return IRotator.Reverse;
                else
                    return Convert.ToBoolean(objTypeRotator.InvokeMember("Reverse",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { }));
            }
            set
            {
                if (IRotator != null)
                    IRotator.Connected = value;
                else
                    objTypeRotator.InvokeMember("Reverse",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objRotatorLateBound, new object[] { value });
            }
        }

        /// <summary>
        /// Display a dialog box for the user to enter in custom setup parameters, such as a COM port selection.
        /// </summary>
        public void SetupDialog()
        {
            if (IRotator != null)
                IRotator.SetupDialog();
            else
                objTypeRotator.InvokeMember("SetupDialog",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objRotatorLateBound, new object[] { });
        }

        /// <summary>
        /// The minimum StepSize, in degrees.
        /// </summary>
        public float StepSize
        {
            get
            {
                if (IRotator != null)
                    return IRotator.StepSize;
                else
                    return Convert.ToSingle(objTypeRotator.InvokeMember("StepSize",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Current Rotator target position, in degrees.
        /// </summary>
        public float TargetPosition
        {
            get
            {
                if (IRotator != null)
                    return IRotator.TargetPosition;
                else
                    return Convert.ToSingle(objTypeRotator.InvokeMember("TargetPosition",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { }));
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
			if (this.objRotatorLateBound != null)
			{
				try { Marshal.ReleaseComObject(objRotatorLateBound); }
				catch (Exception) { }
				objRotatorLateBound = null;
			}
		}

        #endregion
    }
    #endregion
}
