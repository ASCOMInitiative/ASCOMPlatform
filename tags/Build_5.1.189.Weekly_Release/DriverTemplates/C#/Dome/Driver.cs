//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Dome driver for $safeprojectname$
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Dome interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Dome Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Helper;
using ASCOM.Helper2;
using ASCOM.Interface;

namespace ASCOM.$safeprojectname$
{
	//
	// Your driver's ID is ASCOM.$safeprojectname$.Dome
	//
	// The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Dome
	// The ClassInterface/None addribute prevents an empty interface called
	// _Dome from being created and used as the [default] interface
	//
	[Guid("$guid2$")]
	[ClassInterface(ClassInterfaceType.None)]
	public class Dome : IDome
	{
		//
		// Driver ID and descriptive string that shows in the Chooser
		//
		private static string s_csDriverID = "ASCOM.$safeprojectname$.Dome";
		// TODO Change the descriptive string for your driver then remove this line
		private static string s_csDriverDescription = "$safeprojectname$ Dome";

		//
		// Constructor - Must be public for COM registration!
		//
		public Dome()
		{
			// TODO Implement your additional construction here
		}

		#region ASCOM Registration
		//
		// Register or unregister driver for ASCOM. This is harmless if already
		// registered or unregistered. 
		//
		private static void RegUnregASCOM(bool bRegister)
		{
			Helper.Profile P = new Helper.Profile();
			P.DeviceTypeV = "Dome";					//  Requires Helper 5.0.3 or later
			if (bRegister)
				P.Register(s_csDriverID, s_csDriverDescription);
			else
				P.Unregister(s_csDriverID);
			try										// In case Helper becomes native .NET
			{
				Marshal.ReleaseComObject(P);
			}
			catch (Exception) { }
			P = null;
		}

		[ComRegisterFunction]
		public static void RegisterASCOM(Type t)
		{
			RegUnregASCOM(true);
		}

		[ComUnregisterFunction]
		public static void UnregisterASCOM(Type t)
		{
			RegUnregASCOM(false);
		}
		#endregion

		//
		// PUBLIC COM INTERFACE IDome IMPLEMENTATION
		//

		#region IDome Members

		public void AbortSlew()
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("AbortSlew");
		}

		public double Altitude
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("Altitude", false); }
		}

		public bool AtHome
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("AtHome", false); }
		}

		public bool AtPark
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("AtPark", false); }
		}

		public double Azimuth
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("Azimuth", false); }
		}

		public bool CanFindHome
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("CanFindHome", false); }
		}

		public bool CanPark
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("CanPark", false); }
		}

		public bool CanSetAltitude
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("CanSetAltitude", false); }
		}

		public bool CanSetAzimuth
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("CanSetAzimuth", false); }
		}

		public bool CanSetPark
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("CanSetPark", false); }
		}

		public bool CanSetShutter
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("CanSetShutter", false); }
		}

		public bool CanSlave
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("CanSlave", false); }
		}

		public bool CanSyncAzimuth
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("CanSyncAzimuth", false); }
		}

		public void CloseShutter()
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("CloseShutter");
		}

		public void CommandBlind(string Command)
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("CommandBlind");
		}

		public bool CommandBool(string Command)
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("CommandBool");
		}

		public string CommandString(string Command)
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("CommandString");
		}

		public bool Connected
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("Connected", false); }
			set { throw new PropertyNotImplementedException("Connected", true); }
		}

		public string Description
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("Description", false); }
		}

		public string DriverInfo
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("DriverInfo", false); }
		}

		public void FindHome()
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("FindHome");
		}

		public short InterfaceVersion
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("InterfaceVersion", false); }
		}

		public string Name
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("Name", false); }
		}

		public void OpenShutter()
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("OpenShutter");
		}

		public void Park()
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("Park");
		}

		public void SetPark()
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("SetPark");
		}

		public void SetupDialog()
		{
			SetupDialogForm F = new SetupDialogForm();
			F.ShowDialog();
		}

		public ShutterState ShutterStatus
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("ShutterStatus", false); }
		}

		public bool Slaved
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("Slaved", false); }
			set { throw new PropertyNotImplementedException("Slaved", true); }
		}

		public void SlewToAltitude(double Altitude)
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("SlewToAltitude");
		}

		public void SlewToAzimuth(double Azimuth)
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("SlewToAzimuth");
		}

		public bool Slewing
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("Slewing", false); }
		}

		public void SyncToAzimuth(double Azimuth)
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("SyncToAzimuth");
		}

		#endregion
	}
}
