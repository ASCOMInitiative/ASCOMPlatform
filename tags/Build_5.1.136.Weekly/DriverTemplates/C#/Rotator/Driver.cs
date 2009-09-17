//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Rotator driver for $safeprojectname$
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Rotator interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Rotator Driver template
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
	// Your driver's ID is ASCOM.$safeprojectname$.Rotator
	//
	// The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Rotator
	// The ClassInterface/None addribute prevents an empty interface called
	// _Rotator from being created and used as the [default] interface
	//
	[Guid("$guid2$")]
	[ClassInterface(ClassInterfaceType.None)]
	public class Rotator : IRotator
	{
		//
		// Driver ID and descriptive string that shows in the Chooser
		//
		private static string s_csDriverID = "ASCOM.$safeprojectname$.Rotator";
		// TODO Change the descriptive string for your driver then remove this line
		private static string s_csDriverDescription = "$safeprojectname$ Rotator";

		//
		// Constructor - Must be public for COM registration!
		//
		public Rotator()
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
			P.DeviceTypeV = "Rotator";					//  Requires Helper 5.0.3 or later
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
		// PUBLIC COM INTERFACE IRotator IMPLEMENTATION
		//

		#region IRotator Members

		public bool CanReverse
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("CanReverse", false); }
		}

		public bool Connected
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("Connected", false); }
			set { throw new PropertyNotImplementedException("Connected", true); }
		}

		public void Halt()
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("Halt");
		}

		public bool IsMoving
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("IsMoving", false); }
		}

		public void Move(float Position)
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("Move");
		}

		public void MoveAbsolute(float Position)
		{
			// TODO Replace this with your implementation
			throw new MethodNotImplementedException("MoveAbsolute");
		}

		public float Position
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("Position", false); }
		}

		public bool Reverse
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("Reverse", false); }
			set { throw new PropertyNotImplementedException("Reverse", true); }
		}

		public void SetupDialog()
		{
			SetupDialogForm F = new SetupDialogForm();
			F.ShowDialog();
		}

		public float StepSize
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("StepSize", false); }
		}

		public float TargetPosition
		{
			// TODO Replace this with your implementation
			get { throw new PropertyNotImplementedException("TargetPosition", false); }
		}

		#endregion
	}
}
