//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Telescope driver for Telescope
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Telescope interface version: 2.0
// Author:		(rbt) Robert Turner <robert@robertturnerastro.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 07-JUL-2009	rbt	1.0.0	Initial edit, from ASCOM Telescope Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.HelperNET;
using ASCOM.Interface;

namespace ASCOM.TelescopeSimulator
{
    //
    // Your driver's ID is ASCOM.Telescope.Telescope
    //
    // The Guid attribute sets the CLSID for ASCOM.Telescope.Telescope
    // The ClassInterface/None addribute prevents an empty interface called
    // _Telescope from being created and used as the [default] interface
    //
    [Guid("86931eac-1f52-4918-b6aa-7e9b0ff361bd")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Telescope : ReferenceCountedObjectBase, ITelescope
    {
        //
        // Driver private data (rate collections)
        //
        private AxisRates[] m_AxisRates;
        private TrackingRates m_TrackingRates;

        //
        // Constructor - Must be public for COM registration!
        //
        public Telescope()
        {
            m_AxisRates = new AxisRates[3];
            m_AxisRates[0] = new AxisRates(TelescopeAxes.axisPrimary);
            m_AxisRates[1] = new AxisRates(TelescopeAxes.axisSecondary);
            m_AxisRates[2] = new AxisRates(TelescopeAxes.axisTertiary);
            m_TrackingRates = new TrackingRates();
            // TODO Implement your additional construction here
        }


        //
        // PUBLIC COM INTERFACE ITelescope IMPLEMENTATION
        //

        #region ITelescope Members

        public void AbortSlew()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("AbortSlew");
        }

        public AlignmentModes AlignmentMode
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("AlignmentMode", false); }
        }

        public double Altitude
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Altitude", false); }
        }

        public double ApertureArea
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("ApertureArea", false); }
        }

        public double ApertureDiameter
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("ApertureDiameter", false); }
        }

        public bool AtHome
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("AtHome", false); }
        }

        public bool AtPark
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("AtPark", false); }
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary:
                    return m_AxisRates[0];
                case TelescopeAxes.axisSecondary:
                    return m_AxisRates[1];
                case TelescopeAxes.axisTertiary:
                    return m_AxisRates[2];
                default:
                    return null;
            }
        }

        public double Azimuth
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Azimuth", false); }
        }

        public bool CanFindHome
        {
            // TODO Replace this with your implementation
            get { return TelescopeHardware.CanFindHome; }
        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            // TODO Replace this with your implementation
            return TelescopeHardware.CanMoveAxis(Axis);
        }

        public bool CanPark
        {
            // TODO Replace this with your implementation
            get { return TelescopeHardware.CanPark; }
        }

        public bool CanPulseGuide
        {
            // TODO Replace this with your implementation
            get { return TelescopeHardware.CanPulseGuide; }
        }

        public bool CanSetDeclinationRate
        {
            // TODO Replace this with your implementation
            get { return TelescopeHardware.CanSetDeclinationRate; }
        }

        public bool CanSetGuideRates
        {
            // TODO Replace this with your implementation
            get { return TelescopeHardware.CanSetGuideRates; }
        }

        public bool CanSetPark
        {
            // TODO Replace this with your implementation
            get { return TelescopeHardware.CanSetPark; }
        }

        public bool CanSetPierSide
        {
            // TODO Replace this with your implementation
            get { return TelescopeHardware.CanSetPierSide; }
        }

        public bool CanSetRightAscensionRate
        {
            // TODO Replace this with your implementation
            get { return TelescopeHardware.CanSetRightAscensionRate; }
        }

        public bool CanSetTracking
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("CanSetTracking", false); }
        }

        public bool CanSlew
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("CanSlew", false); }
        }

        public bool CanSlewAltAz
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("CanSlewAltAz", false); }
        }

        public bool CanSlewAltAzAsync
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("CanSlewAltAzAsync", false); }
        }

        public bool CanSlewAsync
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("CanSlewAsync", false); }
        }

        public bool CanSync
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("CanSync", false); }
        }

        public bool CanSyncAltAz
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("CanSyncAltAz", false); }
        }

        public bool CanUnpark
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("CanUnpark", false); }
        }

        public void CommandBlind(string Command, bool Raw)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string Command, bool Raw)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string Command, bool Raw)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("CommandString");
        }

        public bool Connected
        {
            // TODO Replace this with your implementation
            get { return TelescopeHardware.Connected; }
            set { TelescopeHardware.Connected = value; }
        }

        public double Declination
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Declination", false); }
        }

        public double DeclinationRate
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("DeclinationRate", false); }
            set { throw new PropertyNotImplementedException("DeclinationRate", true); }
        }

        public string Description
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Description", false); }
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("DestinationSideOfPier");
        }

        public bool DoesRefraction
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("DoesRefraction", false); }
            set { throw new PropertyNotImplementedException("DoesRefraction", true); }
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

        public EquatorialCoordinateType EquatorialSystem
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("EquatorialCoordinateType", false); }
        }

        public void FindHome()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("FindHome");
        }

        public double FocalLength
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("FocalLength", false); }
        }

        public double GuideRateDeclination
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("GuideRateDeclination", false); }
            set { throw new PropertyNotImplementedException("GuideRateDeclination", true); }
        }

        public double GuideRateRightAscension
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("GuideRateRightAscension", false); }
            set { throw new PropertyNotImplementedException("GuideRateRightAscension", true); }
        }

        public short InterfaceVersion
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("InterfaceVersion", false); }
        }

        public bool IsPulseGuiding
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("IsPulseGuiding", false); }
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("MoveAxis");
        }

        public string Name
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Name", false); }
        }

        public void Park()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("Park");
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("PulseGuide");
        }

        public double RightAscension
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("RightAscension", false); }
        }

        public double RightAscensionRate
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("RightAscensionRate", false); }
            set { throw new PropertyNotImplementedException("RightAscensionRate", true); }
        }

        public void SetPark()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SetPark");
        }

        public void SetupDialog()
        {
            if (TelescopeHardware.Connected)
                throw new DriverException("The hardware is connected, cannot do SetupDialog()",
                                    unchecked(ErrorCodes.DriverBase + 4));
            TelescopeSimulator.m_MainForm.DoSetupDialog();
        }

        public PierSide SideOfPier
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("SideOfPier", false); }
            set { throw new PropertyNotImplementedException("SideOfPier", true); }
        }

        public double SiderealTime
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("SiderealTime", false); }
        }

        public double SiteElevation
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("SiteElevation", false); }
            set { throw new PropertyNotImplementedException("SiteElevation", true); }
        }

        public double SiteLatitude
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("SiteLatitude", false); }
            set { throw new PropertyNotImplementedException("SiteLatitude", true); }
        }

        public double SiteLongitude
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("SiteLongitude", false); }
            set { throw new PropertyNotImplementedException("SiteLongitude", true); }
        }

        public short SlewSettleTime
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("SlewSettleTime", false); }
            set { throw new PropertyNotImplementedException("SlewSettleTime", true); }
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SlewToAltAz");
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SlewToAltAzAsync");
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SlewToCoordinates");
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SlewToCoordinatesAsync");
        }

        public void SlewToTarget()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SlewToTarget");
        }

        public void SlewToTargetAsync()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SlewToTargetAsync");
        }

        public bool Slewing
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Slewing", false); }
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SyncToAltAz");
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SyncToCoordinates");
        }

        public void SyncToTarget()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SyncToTarget");
        }

        public double TargetDeclination
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("TargetDeclination", false); }
            set { throw new PropertyNotImplementedException("TargetDeclination", true); }
        }

        public double TargetRightAscension
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("TargetRightAscension", false); }
            set { throw new PropertyNotImplementedException("TargetRightAscension", true); }
        }

        public bool Tracking
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("Tracking", false); }
            set { throw new PropertyNotImplementedException("Tracking", true); }
        }

        public DriveRates TrackingRate
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("TrackingRate", false); }
            set { throw new PropertyNotImplementedException("TrackingRate", true); }
        }

        public ITrackingRates TrackingRates
        {
            get { return m_TrackingRates; }
        }

        public DateTime UTCDate
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("UTCDate", false); }
            set { throw new PropertyNotImplementedException("UTCDate", true); }
        }

        public void Unpark()
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("Unpark");
        }

        #endregion
    }

    //
    // The Rate class implements IRate, and is used to hold values
    // for AxisRates. You do not need to change this class.
    //
    // The Guid attribute sets the CLSID for ASCOM.Telescope.Rate
    // The ClassInterface/None addribute prevents an empty interface called
    // _Rate from being created and used as the [default] interface
    //
    [Guid("d0acdb0f-9c7e-4c53-abb7-576e9f2b8225")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Rate : IRate
    {
        private double m_dMaximum = 0;
        private double m_dMinimum = 0;

        //
        // Default constructor - Internal prevents public creation
        // of instances. These are values for AxisRates.
        //
        internal Rate(double Minimum, double Maximum)
        {
            m_dMaximum = Maximum;
            m_dMinimum = Minimum;
        }

        #region IRate Members

        public double Maximum
        {
            get { return m_dMaximum; }
            set { m_dMaximum = value; }
        }

        public double Minimum
        {
            get { return m_dMinimum; }
            set { m_dMinimum = value; }
        }

        #endregion
    }

    //
    // AxisRates is a strongly-typed collection that must be enumerable by
    // both COM and .NET. The IAxisRates and IEnumerable interfaces provide
    // this polymorphism. 
    //
    // The Guid attribute sets the CLSID for ASCOM.Telescope.AxisRates
    // The ClassInterface/None addribute prevents an empty interface called
    // _AxisRates from being created and used as the [default] interface
    //
    [Guid("af5510b9-3108-4237-83da-ae70524aab7d")]
    [ClassInterface(ClassInterfaceType.None)]
    public class AxisRates : IAxisRates, IEnumerable
    {
        private TelescopeAxes m_Axis;
        private Rate[] m_Rates;

        //
        // Constructor - Internal prevents public creation
        // of instances. Returned by Telescope.AxisRates.
        //
        internal AxisRates(TelescopeAxes Axis)
        {
            m_Axis = Axis;
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
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary:
                    // TODO Initialize this array with any Primary axis rates that your driver may provide
                    // Example: m_Rates = new Rate[] { new Rate(10.5, 30.2), new Rate(54.0, 43.6) }
                    m_Rates = new Rate[0];
                    break;
                case TelescopeAxes.axisSecondary:
                    // TODO Initialize this array with any Secondary axis rates that your driver may provide
                    m_Rates = new Rate[0];
                    break;
                case TelescopeAxes.axisTertiary:
                    // TODO Initialize this array with any Tertiary axis rates that your driver may provide
                    m_Rates = new Rate[0];
                    break;
            }
        }

        #region IAxisRates Members

        public int Count
        {
            get { return m_Rates.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            return m_Rates.GetEnumerator();
        }

        public IRate this[int Index]
        {
            get { return (IRate)m_Rates[Index - 1]; }	// 1-based
        }

        #endregion

    }

    //
    // TrackingRates is a strongly-typed collection that must be enumerable by
    // both COM and .NET. The ITrackingRates and IEnumerable interfaces provide
    // this polymorphism. 
    //
    // The Guid attribute sets the CLSID for ASCOM.Telescope.TrackingRates
    // The ClassInterface/None addribute prevents an empty interface called
    // _TrackingRates from being created and used as the [default] interface
    //
    [Guid("4bf5c72a-8491-49af-8668-626eac765e91")]
    [ClassInterface(ClassInterfaceType.None)]
    public class TrackingRates : ITrackingRates, IEnumerable
    {
        private DriveRates[] m_TrackingRates;

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
            m_TrackingRates = new DriveRates[] { DriveRates.driveSidereal };
            // TODO Initialize this array with any additional tracking rates that your driver may provide
        }

        #region ITrackingRates Members

        public int Count
        {
            get { return m_TrackingRates.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            return m_TrackingRates.GetEnumerator();
        }

        public DriveRates this[int Index]
        {
            get { return m_TrackingRates[Index - 1]; }	// 1-based
        }

        #endregion
    }
}
