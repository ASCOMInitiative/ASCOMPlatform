//
// 10-Jul-08	rbd		1.0.5 - Release COM on Dispose().
//
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Interfaces;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    #region Rotator wrapper
    /// <summary>
    /// Provides universal access to Rotator drivers
    /// </summary>
    public class Rotator : IRotator, IDisposable, IDeviceControl, IAscomDriver
    {
        object objRotatorLateBound;
        IRotator IRotator;
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
                IRotator = (IRotator)objRotatorLateBound;
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
                oChooser.DeviceType = "Rotator";			// Requires Helper 5.0.3 (May '07)
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
                    IRotator.Reverse = value;
                else
                    objTypeRotator.InvokeMember("Reverse",
                        BindingFlags.Default | BindingFlags.SetProperty,
                        null, objRotatorLateBound, new object[] { value });
            }
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

        #region IAscomDriver Members

        /// <summary>
        /// Set True to enable the
        /// link. Set False to disable the link (this does not switch off the cooler).
        /// You can also read the property to check whether it is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        /// <exception cref=" System.Exception">Must throw exception if unsuccessful.</exception>
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
        /// Returns a description of the driver, such as manufacturer and model
        /// number. Any ASCII characters may be used. The string shall not exceed 68
        /// characters (for compatibility with FITS headers).
        /// </summary>
        /// <value>The description.</value>
        /// <exception cref=" System.Exception">Must throw exception if description unavailable</exception>
        public string Description
        {
            get
            {
                if (IRotator != null)
                    return IRotator.Description;
                else
                    return Convert.ToString(objTypeRotator.InvokeMember("Description",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { }));
            }
        }
        /// <summary>
        /// Descriptive and version information about this ASCOM Telescope driver.
        /// This string may contain line endings and may be hundreds to thousands of characters long.
        /// It is intended to display detailed information on the ASCOM driver, including version and copyright data.
        /// See the Description property for descriptive info on the telescope itself.
        /// To get the driver version in a parseable string, use the DriverVersion property.
        /// </summary>
        public string DriverInfo
        {
            get
            {
                if (IRotator != null)
                    return IRotator.DriverInfo;
                else
                    return (string)objTypeRotator.InvokeMember("DriverInfo",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { });
            }
        }
        /// <summary>
        /// A string containing only the major and minor version of the driver.
        /// This must be in the form "n.n".
        /// Not to be confused with the InterfaceVersion property, which is the version of this specification supported by the driver (currently 2). 
        /// </summary>
        public string DriverVersion
        {
            get
            {
                if (IRotator != null)
                    return IRotator.DriverVersion;
                else
                    return (string)objTypeRotator.InvokeMember("DriverVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { });
            }
        }
        /// <summary>
        /// The version of this interface. Will return 2 for this version.
        /// Clients can detect legacy V1 drivers by trying to read ths property.
        /// If the driver raises an error, it is a V1 driver. V1 did not specify this property. A driver may also return a value of 1. 
        /// In other words, a raised error or a return value of 1 indicates that the driver is a V1 driver. 
        /// </summary>
        public short InterfaceVersion
        {
            get
            {
                if (IRotator != null)
                    return IRotator.InterfaceVersion;
                else
                    return (short)objTypeRotator.InvokeMember("InterfaceVersion",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { });
            }
        }

        /// <summary>
        /// Gets the last result.
        /// </summary>
        /// <value>
        /// The result of the last executed action, or <see cref="String.Empty"	/>
        /// if no action has yet been executed.
        /// </value>
        public string LastResult
        {
            get
            {
                if (IRotator != null)
                    return IRotator.LastResult;
                else
                    return Convert.ToString(objTypeRotator.InvokeMember("LastResult",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// The short name of the telescope, for display purposes
        /// </summary>
        public string Name
        {
            get
            {
                if (IRotator != null)
                    return IRotator.Name;
                else
                    return (string)objTypeRotator.InvokeMember("Name",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { });
            }
        }
        /// <summary>
        /// Launches a configuration dialog box for the driver.  The call will not return
        /// until the user clicks OK or cancel manually.
        /// </summary>
        /// <exception cref=" System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
        public void SetupDialog()
        {
            if (IRotator != null)
                IRotator.SetupDialog();
            else
                objTypeRotator.InvokeMember("SetupDialog",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objRotatorLateBound, new object[] { });
        }
        #endregion

        #region IDeviceControl Members

        /// <summary>
        /// Invokes the specified device-specific action.
        /// </summary>
        /// <param name="ActionName">
        /// A well known name agreed by interested parties that represents the action
        /// to be carried out. 
        /// <example>suppose filter wheels start to appear with automatic wheel changers; new actions could 
        /// be “FilterWheel:QueryWheels” and “FilterWheel:SelectWheel”. The former returning a 
        /// formatted list of wheel names and the second taking a wheel name and making the change.
        /// </example>
        /// </param>
        /// <param name="ActionParameters">
        /// List of required parameters or <see cref="String.Empty"/>  if none are required.
        /// </param>
        /// <returns>A string response and sets the <c>IDeviceControl.LastResult</c> property.</returns>
        public string Action(string ActionName, string ActionParameters)
        {
            if (IRotator != null)
                return IRotator.Action(ActionName, ActionParameters);
            else
                return (string)objTypeRotator.InvokeMember("Action",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objRotatorLateBound, new object[] { });
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        /// <value>The supported actions.</value>
        public string[] SupportedActions
        {
            get
            {
                if (IRotator != null)
                    return IRotator.SupportedActions;
                else
                    return (string[])(objTypeRotator.InvokeMember("SupportedActions",
                        BindingFlags.Default | BindingFlags.GetProperty,
                        null, objRotatorLateBound, new object[] { }));
            }
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        public void CommandBlind(string Command, bool Raw)
        {
            if (IRotator != null)
                IRotator.CommandBlind(Command, Raw);
            else
                objTypeRotator.InvokeMember("CommandBlind",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objRotatorLateBound, new object[] { Command, Raw });
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        public bool CommandBool(string Command, bool Raw)
        {
            if (IRotator != null)
                return IRotator.CommandBool(Command, Raw);
            else
                return (bool)objTypeRotator.InvokeMember("CommandBool",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objRotatorLateBound, new object[] { Command, Raw });
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        public string CommandString(string Command, bool Raw)
        {
            if (IRotator != null)
                return IRotator.CommandString(Command, Raw);
            else
                return (string)objTypeRotator.InvokeMember("CommandString",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null, objRotatorLateBound, new object[] { Command, Raw });
        }

        #endregion
    }
    #endregion
}
