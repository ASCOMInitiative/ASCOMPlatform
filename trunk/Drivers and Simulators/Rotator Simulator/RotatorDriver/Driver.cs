//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Rotator driver for RotatorSimulator
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
using System.Runtime.InteropServices;
using ASCOM.Interface;

namespace ASCOM.Simulator
{
	//
	// Your driver's ID is ASCOM.Simulator.Rotator
	//
	// The Guid attribute sets the CLSID for ASCOM.Simulator.Rotator
	// The ClassInterface/None addribute prevents an empty interface called
	// _Rotator from being created and used as the [default] interface
	//
	[Guid("e3244961-cb52-437d-aa9b-e5324db8f388"), ClassInterface(ClassInterfaceType.None), ComVisible(true)] 
	public class Rotator : ReferenceCountedObjectBase, IRotator
	{

		//
		// Constructor - Must be public for COM registration!
		//
		public Rotator() { }

		//
		// PUBLIC COM INTERFACE IRotator IMPLEMENTATION
        //
        #region IDeviceControl Members
        public void Action(string cmd,string parms)
        {
            throw new MethodNotImplementedException("Action");
        }

        public string[] SupportedActions()
        {
            throw new MethodNotImplementedException("SupportedActions");
        }
        #endregion

        #region IRotator Members

        public bool CanReverse
		{
			get { return RotatorHardware.CanReverse; }
		}

		public bool Connected
		{
			get { return RotatorHardware.Connected; }
			set { RotatorHardware.Connected = value; }
		}

		public void Halt()
		{
			RotatorHardware.Halt();
		}

		public bool IsMoving
		{
			get { return RotatorHardware.Moving; }
		}

		public void Move(float Position)
		{
			RotatorHardware.Move(Position);
		}

		public void MoveAbsolute(float Position)
		{
			RotatorHardware.MoveAbsolute(Position);
		}

		public float Position
		{
			get { return RotatorHardware.Position; }
		}

		public bool Reverse
		{
			get { return RotatorHardware.Reverse; }
			set { RotatorHardware.Reverse = value; }
		}

		public void SetupDialog()
		{
			if(RotatorHardware.Connected)
				throw new DriverException("The rotator is connected, cannot do SetupDialog()",
									unchecked(ErrorCodes.DriverBase + 4));
			RotatorSimulator.m_MainForm.DoSetupDialog();			// Kinda sleazy
		}

		public float StepSize
		{
			get { return RotatorHardware.StepSize; }
		}

		public float TargetPosition
		{
			get { return RotatorHardware.TargetPosition; }
		}

		#endregion
	}
}
