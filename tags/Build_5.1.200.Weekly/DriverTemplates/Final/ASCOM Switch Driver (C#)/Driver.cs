//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Switch driver for $safeprojectname$
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Switch interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Switch Driver template
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
	// Your driver's ID is ASCOM.$safeprojectname$.Switch
	//
	// The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Switch
	// The ClassInterface/None addribute prevents an empty interface called
	// _Switch from being created and used as the [default] interface
	//
	[Guid("$guid2$")]
	[ClassInterface(ClassInterfaceType.None)]
	public class Switch : ISwitch
	{
		//
		// Driver ID and descriptive string that shows in the Chooser
		//
		private static string s_csDriverID = "ASCOM.$safeprojectname$.Switch";
		// TODO Change the descriptive string for your driver then remove this line
		private static string s_csDriverDescription = "$safeprojectname$ Switch";

		//
		// Constructor - Must be public for COM registration!
		//
		public Switch()
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
			P.DeviceTypeV = "Switch";					//  Requires Helper 5.0.3 or later
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
		// PUBLIC COM INTERFACE ISwitch IMPLEMENTATION
		//

        #region ISwitch Members

        public bool GetSwitch(short ID)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("GetSwitch");
        }
        public void SetSwitch(short ID,bool State)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SetSwitch");
        }
        public string GetSwitchName(short ID)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("GetSwitchName");
        }
        public void SetSwitchName(short ID, string Name)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SetSwitchName");
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
        public string DriverVersion
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("DriverVersion", false); }
        }
        public short InterfaceVersion
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("InterfaceVersion", false); }
        }
        public short MaxSwitch
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("MaxSwitch", false); }
        }
        public string Name
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Name", false); }
        }
        public void SetupDialog()
        {
            SetupDialogForm F = new SetupDialogForm();
            F.ShowDialog();
        }
        #endregion
	}
}
