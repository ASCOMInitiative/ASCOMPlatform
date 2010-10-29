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
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;

namespace ASCOM.Simulator
{
	//
	// Your driver's ID is ASCOM.Simulator.Rotator
	//
	// The Guid attribute sets the CLSID for ASCOM.Simulator.Rotator
	// The ClassInterface/None addribute prevents an empty interface called
	// _Rotator from being created and used as the [default] interface
	//
    [Guid("347B5004-3662-42C0-96B8-3F8F6F0467D2")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)] 
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
        /// <summary>
        /// Invokes the specified device-specific action.
        /// </summary>
        /// <exception cref="MethodNotImplementedException"></exception>
        public string Action(string actionName, string actionParameters)
        {
            throw new MethodNotImplementedException("Action");
        }

	    public void CommandBlind(string command, bool raw)
	    {
            throw new MethodNotImplementedException("CommandBlind");
	    }

	    public bool CommandBool(string command, bool raw)
	    {
            throw new MethodNotImplementedException("CommandBool");
	    }

	    public string CommandString(string command, bool raw)
	    {
            throw new MethodNotImplementedException("CommandString");
	    }

	    public void Dispose()
	    {
            throw new MethodNotImplementedException("Dispose");
	    }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        public string[] SupportedActions
        {
            // no supported actions, return empty array
            get { string[] sa = { }; return sa; }
            //get { throw new MethodNotImplementedException("SupportedActions"); }
        }

        
        
        
        
        
        #endregion


        
        
        #region IRotator Members
        public bool Connected
        {
            get { return RotatorHardware.Connected; }
            set { RotatorHardware.Connected = value; }
        }

        public string Description
        {
            get { return RotatorHardware.Description; }
        }

        public string DriverInfo
        {
            get { return RotatorHardware.DriverInfo; }
        }

        public string DriverVersion
        {
            get { return RotatorHardware.DriverVersion; }
        }

        public short InterfaceVersion
        {
            get { return RotatorHardware.InterfaceVersion; }
        }

        public string Name
        {
            get { return RotatorHardware.RotatorName; }
        }

        string[] IRotator.SupportedActions
	    {
            get { throw new MethodNotImplementedException("SupportedActions"); }
	    }

	    public bool CanReverse
		{
			get { return RotatorHardware.CanReverse; }
		}

	    public void Halt()
		{
			RotatorHardware.Halt();
		}

		public bool IsMoving
		{
			get { return RotatorHardware.Moving; }
		}

		public void Move(float position)
		{
			RotatorHardware.Move(position);
		}

		public void MoveAbsolute(float position)
		{
			RotatorHardware.MoveAbsolute(position);
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
            RotatorHardware.SetupDialog();

			//RotatorSimulator.m_MainForm.DoSetupDialog();			// Kinda sleazy
		}

	    string IRotator.Action(string actionName, string actionParameters)
	    {
	        throw new System.NotImplementedException();
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
