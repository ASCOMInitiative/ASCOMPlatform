//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Rotator driver for SampleLocalServer
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

namespace ASCOM.SampleLocalServer
{
	//
	// Your driver's ID is ASCOM.SampleLocalServer.Rotator
	//
	// The Guid attribute sets the CLSID for ASCOM.SampleLocalServer.Rotator
	// The ClassInterface/None addribute prevents an empty interface called
	// _Rotator from being created and used as the [default] interface
	//
	[Guid("4987D641-7E8B-41b9-AE0D-FB232FBBC1F0")]
	[ClassInterface(ClassInterfaceType.None)]
	public class Rotator : 
		ReferenceCountedObjectBase,
		IRotator
	{
		//
		// Constructor - Must be public for COM registration!
		//
		public Rotator()
		{
			// TODO Implement your additional construction here
		}

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
			get { return true; }
			set { ; }
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
			get { return SharedResources.z; }
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
