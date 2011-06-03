//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for $safeprojectname$
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Focuser interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Focuser Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Globalization;

namespace ASCOM.$safeprojectname$
{
	//
	// Your driver's ID is ASCOM.$safeprojectname$.Focuser
	//
	// The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Focuser
	// The ClassInterface/None addribute prevents an empty interface called
	// _Focuser from being created and used as the [default] interface
	//
    [Guid("$guid2$")]
	[ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
	public class Focuser : IFocuserV2
    {
        #region Constants
        //
		// Driver ID and descriptive string that shows in the Chooser
		//
		private const string driverId = "ASCOM.$safeprojectname$.Focuser";
		// TODO Change the descriptive string for your driver then remove this line
		private const string driverDescription = "$safeprojectname$ Focuser";
        #endregion

        #region ASCOM Registration
        //
		// Register or unregister driver for ASCOM. This is harmless if already
		// registered or unregistered. 
		//
		private static void RegUnregASCOM(bool bRegister)
		{
            using (var p = new Profile())
            {
                p.DeviceType = "Focuser";
                if (bRegister)
                    p.Register(driverId, driverDescription);
                else
                    p.Unregister(driverId);
            }
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

        #region Implementation of IFocuserV2

        public void SetupDialog()
        {
            using (var f = new SetupDialogForm())
            {
                f.ShowDialog();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            throw new ASCOM.MethodNotImplementedException("Action");
        }

        public void CommandBlind(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void Halt()
        {
            throw new System.NotImplementedException();
        }

        public void Move(int value)
        {
            throw new System.NotImplementedException();
        }

        public bool Connected
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Description
        {
            get { throw new System.NotImplementedException(); }
        }

        public string DriverInfo
        {
            get { throw new System.NotImplementedException(); }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
            }
        }

        public short InterfaceVersion
        {
            get { return 2; }
        }

        public string Name
        {
            get { throw new System.NotImplementedException(); }
        }

        public ArrayList SupportedActions
        {
            get { return new ArrayList(); }
        }

        public bool Absolute
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool IsMoving
        {
            get { throw new System.NotImplementedException(); }
        }

        // use the V2 connected property
        public bool Link
        {
            get { return this.Connected; }
            set { this.Connected = value; }
        }

        public int MaxIncrement
        {
            get { throw new System.NotImplementedException(); }
        }

        public int MaxStep
        {
            get { throw new System.NotImplementedException(); }
        }

        public int Position
        {
            get { throw new System.NotImplementedException(); }
        }

        public double StepSize
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool TempComp
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool TempCompAvailable
        {
            get { throw new System.NotImplementedException(); }
        }

        public double Temperature
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
	}
}
