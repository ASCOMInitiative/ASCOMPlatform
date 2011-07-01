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
using System.Collections;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.Globalization;

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
    [ComVisible(true)]
	public class Dome : IDomeV2
    {
        #region Constants
        //
		// Driver ID and descriptive string that shows in the Chooser
		//
		private const string driverId = "ASCOM.$safeprojectname$.Dome";
		// TODO Change the descriptive string for your driver then remove this line
		private const string driverDescription = "$safeprojectname$ Dome";
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
                p.DeviceType = "Dome";
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

	    #region Implementation of IDomeV2

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

	    public void AbortSlew()
	    {
	        throw new System.NotImplementedException();
	    }

	    public void CloseShutter()
	    {
	        throw new System.NotImplementedException();
	    }

	    public void FindHome()
	    {
	        throw new System.NotImplementedException();
	    }

	    public void OpenShutter()
	    {
	        throw new System.NotImplementedException();
	    }

	    public void Park()
	    {
	        throw new System.NotImplementedException();
	    }

	    public void SetPark()
	    {
	        throw new System.NotImplementedException();
	    }

	    public void SlewToAltitude(double altitude)
	    {
	        throw new System.NotImplementedException();
	    }

	    public void SlewToAzimuth(double azimuth)
	    {
	        throw new System.NotImplementedException();
	    }

	    public void SyncToAzimuth(double azimuth)
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

	    public double Altitude
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    public bool AtHome
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    public bool AtPark
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    public double Azimuth
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    public bool CanFindHome
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    public bool CanPark
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    public bool CanSetAltitude
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    public bool CanSetAzimuth
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    public bool CanSetPark
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    public bool CanSetShutter
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    public bool CanSlave
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    public bool CanSyncAzimuth
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    public ShutterState ShutterStatus
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    public bool Slaved
	    {
	        get { throw new System.NotImplementedException(); }
	        set { throw new System.NotImplementedException(); }
	    }

	    public bool Slewing
	    {
	        get { throw new System.NotImplementedException(); }
	    }

	    #endregion
	}
}
