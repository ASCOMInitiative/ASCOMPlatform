using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM.Interface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{

	#region Switch wrapper

	/// <summary>
	///   Provides universal access to Switch drivers
	/// </summary>
	public class Switch : ISwitch, IDisposable
	{
		private readonly ISwitch ISwitch;
		private readonly Type objTypeSwitch;
		private object objSwitchLateBound;

		/// <summary>
		///   Creates a Switch object with the given Prog ID
		/// </summary>
		/// <param name = "switchID"></param>
		public Switch(string switchID)
		{
			// Get Type Information 
			objTypeSwitch = Type.GetTypeFromProgID(switchID);

			// Create an instance of the Switch object
			objSwitchLateBound = Activator.CreateInstance(objTypeSwitch);

			// Try to see if this driver has an ASCOM.Switch interface
			try
			{
				ISwitch = (ISwitch) objSwitchLateBound;
			}
			catch (Exception)
			{
				ISwitch = null;
			}
		}


		#region IDisposable Members

		/// <summary>
		///   Dispose the late-bound interface, if needed. Will release it via COM
		///   if it is a COM object, else if native .NET will just dereference it
		///   for GC.
		/// </summary>
		public void Dispose()
		{
			if (objSwitchLateBound != null)
			{
				try
				{
					Marshal.ReleaseComObject(objSwitchLateBound);
				}
				catch (Exception)
				{
				}
				objSwitchLateBound = null;
			}
		}

		#endregion

		#region ISwitch Members

		/// <summary>
		///   Yields a collection of ISwitchController objects.
		/// </summary>
		public ArrayList SwitchCollection
		{
			get
			{
				if (ISwitch != null)
					return ISwitch.SwitchCollection;
				else
				{
					return (ArrayList) (objTypeSwitch.InvokeMember("SwitchCollection",
					                                               BindingFlags.Default | BindingFlags.GetProperty,
					                                               null, objSwitchLateBound, new object[] {}));
				}
			}
		}


		/// <summary>
		///   Set True to Connect to the switches; set False to terminate the Connection.
		///   The current Connected status can also be read back as this property.
		///   An exception will be raised if the Connected fails to change state for any reason.
		/// </summary>
		public bool Connected
		{
			get
			{
				if (ISwitch != null)
					return ISwitch.Connected;
				else
					return Convert.ToBoolean(objTypeSwitch.InvokeMember("Connected",
					                                                    BindingFlags.Default | BindingFlags.GetProperty,
					                                                    null, objSwitchLateBound, new object[] {}));
			}
			set
			{
				if (ISwitch != null)
					ISwitch.Connected = value;
				else
					objTypeSwitch.InvokeMember("Connected",
					                           BindingFlags.Default | BindingFlags.SetProperty,
					                           null, objSwitchLateBound, new object[] {value});
			}
		}

		/// <summary>
		///   Returns a description of the Switch controller
		/// </summary>
		public string Description
		{
			get
			{
				if (ISwitch != null)
					return ISwitch.Description;
				else
					return Convert.ToString(objTypeSwitch.InvokeMember("Description",
					                                                   BindingFlags.Default | BindingFlags.GetProperty,
					                                                   null, objSwitchLateBound, new object[] {}));
			}
		}

		/// <summary>
		///   Returns the driver info
		/// </summary>
		public string DriverInfo
		{
			get
			{
				if (ISwitch != null)
					return ISwitch.DriverInfo;
				else
					return Convert.ToString(objTypeSwitch.InvokeMember("DriverInfo",
					                                                   BindingFlags.Default | BindingFlags.GetProperty,
					                                                   null, objSwitchLateBound, new object[] {}));
			}
		}

		/// <summary>
		///   Returns the driver version
		/// </summary>
		public string DriverVersion
		{
			get
			{
				if (ISwitch != null)
					return ISwitch.DriverVersion;
				else
					return Convert.ToString(objTypeSwitch.InvokeMember("DriverVersion",
					                                                   BindingFlags.Default | BindingFlags.GetProperty,
					                                                   null, objSwitchLateBound, new object[] {}));
			}
		}

		/// <summary>
		///   Returns the Switch Interface version
		/// </summary>
		public short InterfaceVersion
		{
			get
			{
				if (ISwitch != null)
					return ISwitch.InterfaceVersion;
				else
					return Convert.ToInt16(objTypeSwitch.InvokeMember("InterfaceVersion",
					                                                  BindingFlags.Default | BindingFlags.GetProperty,
					                                                  null, objSwitchLateBound, new object[] {}));
			}
		}

		/// <summary>
		///   returns the name of the switch interface
		/// </summary>
		public string Name
		{
			get
			{
				if (ISwitch != null)
					return ISwitch.Name;
				else
					return Convert.ToString(objTypeSwitch.InvokeMember("Name",
					                                                   BindingFlags.Default | BindingFlags.GetProperty,
					                                                   null, objSwitchLateBound, new object[] {}));
			}
		}

		///<summary>
		///  Launches a configuration dialog box for the driver.  The call will not return
		///  until the user clicks OK or cancel manually.
		///</summary>
		///<exception cref = " System.Exception">Must throw an exception if Setup dialog is unavailable.</exception>
		public void SetupDialog()
		{
			if (ISwitch != null)
				ISwitch.SetupDialog();
			else
				objTypeSwitch.InvokeMember("SetupDialog",
				                           BindingFlags.Default | BindingFlags.InvokeMethod,
				                           null, objSwitchLateBound, new object[] {});
		}

		#endregion

		/// <summary>
		///   Brings up the ASCOM Chooser Dialog to choose a Switch
		/// </summary>
		/// <param name = "switchID">Switch Prog ID for default or null for None</param>
		/// <returns>Prog ID for chosen Switch or null for none</returns>
		public static string Choose(string switchID)
		{
			Chooser oChooser = new Chooser();
			oChooser.DeviceType = "Switch"; // Requires Helper 5.0.3 (May '07)
			return oChooser.Choose(switchID);
		}

	}

	#endregion
}