//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Telescope driver for $safeprojectname$
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Telescope interface version: 2.0
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	1.0.0	Initial edit, from ASCOM Telescope Driver template
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
	// Your driver's ID is ASCOM.$safeprojectname$.Telescope
	//
	// The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Telescope
	// The ClassInterface/None addribute prevents an empty interface called
	// _Telescope from being created and used as the [default] interface
	//
    [Guid("$guid2$")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Telescope : ITelescopeV3
    {
        #region Constants
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        private const string driverId = "ASCOM.$safeprojectname$.Telescope";
        // TODO Change the descriptive string for your driver then remove this line
        private const string driverDescription = "$safeprojectname$ Telescope";

        //
        // Driver private data (rate collections)
        //
        private readonly AxisRates[] _axisRates;
        #endregion

        #region Contructor
        //
        // Constructor - Must be public for COM registration!
        //
        public Telescope()
        {
            _axisRates = new AxisRates[3];
            _axisRates[0] = new AxisRates(TelescopeAxes.axisPrimary);
            _axisRates[1] = new AxisRates(TelescopeAxes.axisSecondary);
            _axisRates[2] = new AxisRates(TelescopeAxes.axisTertiary);
            // TODO Implement your additional construction here
        }
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
                p.DeviceType = "Telescope";
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

        #region Implementation of ITelescopeV3

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

        public IAxisRates AxisRates(TelescopeAxes axis)
        {
            return _axisRates[(int)axis];
        }

        public bool CanMoveAxis(TelescopeAxes axis)
        {
            throw new System.NotImplementedException();
        }

        public PierSide DestinationSideOfPier(double rightAscension, double declination)
        {
            throw new System.NotImplementedException();
        }

        public void FindHome()
        {
            throw new System.NotImplementedException();
        }

        public void MoveAxis(TelescopeAxes axis, double rate)
        {
            throw new System.NotImplementedException();
        }

        public void Park()
        {
            throw new System.NotImplementedException();
        }

        public void PulseGuide(GuideDirections direction, int duration)
        {
            throw new System.NotImplementedException();
        }

        public void SetPark()
        {
            throw new System.NotImplementedException();
        }

        public void SlewToAltAz(double azimuth, double altitude)
        {
            throw new System.NotImplementedException();
        }

        public void SlewToAltAzAsync(double azimuth, double altitude)
        {
            throw new System.NotImplementedException();
        }

        public void SlewToCoordinates(double rightAscension, double declination)
        {
            throw new System.NotImplementedException();
        }

        public void SlewToCoordinatesAsync(double rightAscension, double declination)
        {
            throw new System.NotImplementedException();
        }

        public void SlewToTarget()
        {
            throw new System.NotImplementedException();
        }

        public void SlewToTargetAsync()
        {
            throw new System.NotImplementedException();
        }

        public void SyncToAltAz(double azimuth, double altitude)
        {
            throw new System.NotImplementedException();
        }

        public void SyncToCoordinates(double rightAscension, double declination)
        {
            throw new System.NotImplementedException();
        }

        public void SyncToTarget()
        {
            throw new System.NotImplementedException();
        }

        public void Unpark()
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
            get { return 3; }
        }

        public string Name
        {
            get { throw new System.NotImplementedException(); }
        }

        public ArrayList SupportedActions
        {
            get { return new ArrayList(); }
        }

        public AlignmentModes AlignmentMode
        {
            get { throw new System.NotImplementedException(); }
        }

        public double Altitude
        {
            get { throw new System.NotImplementedException(); }
        }

        public double ApertureArea
        {
            get { throw new System.NotImplementedException(); }
        }

        public double ApertureDiameter
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

        public bool CanPulseGuide
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanSetDeclinationRate
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanSetGuideRates
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanSetPark
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanSetPierSide
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanSetRightAscensionRate
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanSetTracking
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanSlew
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanSlewAltAz
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanSlewAltAzAsync
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanSlewAsync
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanSync
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanSyncAltAz
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanUnpark
        {
            get { throw new System.NotImplementedException(); }
        }

        public double Declination
        {
            get { throw new System.NotImplementedException(); }
        }

        public double DeclinationRate
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool DoesRefraction
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public EquatorialCoordinateType EquatorialSystem
        {
            get { throw new System.NotImplementedException(); }
        }

        public double FocalLength
        {
            get { throw new System.NotImplementedException(); }
        }

        public double GuideRateDeclination
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public double GuideRateRightAscension
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsPulseGuiding
        {
            get { throw new System.NotImplementedException(); }
        }

        public double RightAscension
        {
            get { throw new System.NotImplementedException(); }
        }

        public double RightAscensionRate
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public PierSide SideOfPier
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public double SiderealTime
        {
            get { throw new System.NotImplementedException(); }
        }

        public double SiteElevation
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public double SiteLatitude
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public double SiteLongitude
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool Slewing
        {
            get { throw new System.NotImplementedException(); }
        }

        public short SlewSettleTime
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public double TargetDeclination
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public double TargetRightAscension
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool Tracking
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public DriveRates TrackingRate
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public ITrackingRates TrackingRates
        {
            get { return new TrackingRates(); }
        }

        public DateTime UTCDate
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        #endregion
    }
     
    #region Rate
   //
	// The Rate class implements IRate, and is used to hold values
	// for AxisRates. You do not need to change this class.
	//
	// The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Rate
	// The ClassInterface/None addribute prevents an empty interface called
	// _Rate from being created and used as the [default] interface
	//
    [Guid("$guid3$")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Rate : IRate
    {
        private double _maximum = 0;
        private double _minimum = 0;

        //
        // Default constructor - Internal prevents public creation
        // of instances. These are values for AxisRates.
        //
        internal Rate(double minimum, double maximum)
        {
            _maximum = maximum;
            _minimum = minimum;
        }

        #region Implementation of IRate

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public double Maximum
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public double Minimum
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        #endregion
    }
	#endregion

    #region AxisRates
    //
	// AxisRates is a strongly-typed collection that must be enumerable by
	// both COM and .NET. The IAxisRates and IEnumerable interfaces provide
	// this polymorphism. 
	//
	// The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.AxisRates
	// The ClassInterface/None addribute prevents an empty interface called
	// _AxisRates from being created and used as the [default] interface
	//
    [Guid("$guid4$")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class AxisRates : IAxisRates, IEnumerable
    {
        private TelescopeAxes _axis;
        private readonly Rate[] _rates;

        //
        // Constructor - Internal prevents public creation
        // of instances. Returned by Telescope.AxisRates.
        //
        internal AxisRates(TelescopeAxes axis)
        {
            _axis = axis;
            //
            // This collection must hold zero or more Rate objects describing the 
            // rates of motion ranges for the Telescope.MoveAxis() method
            // that are supported by your driver. It is OK to leave this 
            // array empty, indicating that MoveAxis() is not supported.
            //
            // Note that we are constructing a rate array for the axis passed
            // to the constructor. Thus we switch() below, and each case should 
            // initialize the array for the rate for the selected axis.
            //
            switch (axis)
            {
                case TelescopeAxes.axisPrimary:
                    // TODO Initialize this array with any Primary axis rates that your driver may provide
                    // Example: m_Rates = new Rate[] { new Rate(10.5, 30.2), new Rate(54.0, 43.6) }
                    _rates = new Rate[0];
                    break;
                case TelescopeAxes.axisSecondary:
                    // TODO Initialize this array with any Secondary axis rates that your driver may provide
                    _rates = new Rate[0];
                    break;
                case TelescopeAxes.axisTertiary:
                    // TODO Initialize this array with any Tertiary axis rates that your driver may provide
                    _rates = new Rate[0];
                    break;
            }
        }

        #region IAxisRates Members

        public int Count
        {
            get { return _rates.Length; }
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return _rates.GetEnumerator();
        }

        public IRate this[int index]
        {
            get { return _rates[index - 1]; }	// 1-based
        }

        #endregion



    }
    #endregion

    #region TrackingRates
	//
	// TrackingRates is a strongly-typed collection that must be enumerable by
	// both COM and .NET. The ITrackingRates and IEnumerable interfaces provide
	// this polymorphism. 
	//
	// The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.TrackingRates
	// The ClassInterface/None addribute prevents an empty interface called
	// _TrackingRates from being created and used as the [default] interface
	//
    [Guid("$guid5$")]
	[ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
	public class TrackingRates : ITrackingRates, IEnumerable
	{
		private readonly DriveRates[] _trackingRates;

		//
		// Default constructor - Internal prevents public creation
		// of instances. Returned by Telescope.AxisRates.
		//
		internal TrackingRates() 
		{
			//
			// This array must hold ONE or more DriveRates values, indicating
			// the tracking rates supported by your telescope. The one value
			// (tracking rate) that MUST be supported is driveSidereal!
			//
            _trackingRates = new[] { DriveRates.driveSidereal };	
			// TODO Initialize this array with any additional tracking rates that your driver may provide
		}

		#region ITrackingRates Members

		public int Count
		{
            get { return _trackingRates.Length; }
		}

		public IEnumerator GetEnumerator()
		{
            return _trackingRates.GetEnumerator();
		}

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public DriveRates this[int index]
		{
            get { return _trackingRates[index - 1]; }	// 1-based
		}

		#endregion

	}
    #endregion
}
