//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM SafetyMonitor driver for $safeprojectname$
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM SafetyMonitor interface version: 1.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM SafetyMonitor Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Globalization;
using System.Windows.Forms;

namespace ASCOM.$safeprojectname$
{
    //
    // Your driver's ID is ASCOM.$safeprojectname$.SafetyMonitor
    //
    // The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.SafetyMonitor
    // The ClassInterface/None addribute prevents an empty interface called
    // _SafetyMonitor from being created and used as the [default] interface
    //
    [Guid("$guid2$")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class SafetyMonitor : ISafetyMonitor, IDisposable
    {
        #region Constants
        //
        // Driver ID and descriptive string that shows in the Chooser
        // Set the driver Id to match the namespace.class
        //
        private const string driverId = "ASCOM.$safeprojectname$.SafetyMonitor";
        // TODO Change the descriptive string for your driver then remove this line
        private const string driverDescription = "$safeprojectname$ SafetyMonitor";
        #endregion

        #region ASCOM Registration

        // Constructor - Must be public for COM registration!
        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var p = new Profile())
            {
                p.DeviceType = "SafetyMonitor";
                if (bRegister)
                {
                    p.Register(driverId, driverDescription);
                    p.CreateSubKey(driverId, "SafetyMonitores"); 
                }
                else
                {
                    p.Unregister(driverId);
                }
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

        #region IDisposable Members

        public void SetupDialog()
        {
		// TODO the setup dialog may need only to be shown when the device is not connected.
		using (var f = new SetupDialogForm())
		{
			DialogResult result = f.ShowDialog();
			if (result == DialogResult.OK)
			{
				// TODO add code to be executed if the settings have been changed
			}
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

		public bool Connected
		{
			get { throw new System.NotImplementedException(); }
			set 
			{ 
				if (value)
				{
					// TODO connect to the safety monitor
					string comPort = Properties.Settings.Default.ComPort;
				}
				else
				{
					// TODO disconnect from the safety monitor
				}
				throw new System.NotImplementedException(); 
			}
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

        public bool IsSafe
        {
            get { throw new System.NotImplementedException(); }
        }

        void IDisposable.Dispose()
        {
            throw new System.NotImplementedException();
        }

        #endregion


    }
}