//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM $deviceclass$ driver for $devicename$
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM $deviceclass$ interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from Platform 5.1 template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Controls;
using ASCOM.Interface;
using ASCOM.Utilities;

namespace ASCOM.$devicename$
{
	//
	// Your driver's ID is ASCOM.DeviceName.$safeprojectname$
	//
	// The Guid attribute sets the CLSID for ASCOM.DeviceName.$safeprojectname$
	// The ClassInterface/None addribute prevents an empty interface called
	// _$safeprojectname$ from being created and used as the [default] interface
	//
	[Guid("GUIDSUBST")]
	[ClassInterface(ClassInterfaceType.None)]
	public class $deviceclass$ : I$deviceclass$
	{
		//
		// Driver ID and descriptive string that shows in the Chooser
		//
		private static string s_csDriverID = "$deviceid$";
		// TODO Change the descriptive string for your driver then remove this line
		private static string s_csDriverDescription = "$devicename$";

		//
		// Constructor - Must be public for COM registration!
		//
		public $deviceclass$()
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
			var P = new Profile { DeviceType = "$deviceclass$" };
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
		// PUBLIC COM INTERFACE I$deviceclass$ IMPLEMENTATION
		//

		#region I$deviceclass$ Members

		public void SetupDialog()
		{
			SetupDialogForm F = new SetupDialogForm();
			F.ShowDialog();
		}

		#endregion
	}
}
