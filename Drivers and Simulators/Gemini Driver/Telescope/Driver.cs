//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Gemini driver for Telescope
//
// Description:	
//
// Implements:	ASCOM Telescope interface version: 2.0
// Author:		(rbt) Robert Turner <robert@robertturnerastro.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 15-JUL-2009	rbt	1.0.0	Initial edit, from ASCOM Telescope Driver template
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.HelperNET;
using ASCOM.Interface;
using ASCOM.GeminiTelescope;

namespace ASCOM.GeminiTelescope
{
    //
    // Your driver's ID is ASCOM.Telescope.Telescope
    //
    // The Guid attribute sets the CLSID for ASCOM.Telescope.Telescope
    // The ClassInterface/None addribute prevents an empty interface called
    // _Telescope from being created and used as the [default] interface
    //
    [Guid("7e30c546-1a9a-4ed4-98d7-03eb167e2c9a")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Telescope : ReferenceCountedObjectBase, ITelescope
    {
        //
        // Driver ID and descriptive string that shows in the Chooser
        //
        //private static string s_csDriverID = "ASCOM.Telescope.Telescope";
        // TODO Change the descriptive string for your driver then remove this line
        //private static string s_csDriverDescription = "Telescope Telescope";

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
            if (GeminiHardware.AtHome || GeminiHardware.AtPark)              
                throw new DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);
            GeminiHardware.DoCommand(":Q");
        }

        public AlignmentModes AlignmentMode
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("AlignmentMode", false); }
        }

        public double Altitude
        {
            get { return GeminiHardware.Altitude; }
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
            get { return GeminiHardware.AtHome; }
        }

        public bool AtPark
        {
            get { return GeminiHardware.AtPark; }
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
            get { return GeminiHardware.Azimuth; }
        }

        public bool CanFindHome
        {
            get { return true; }
        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary:
                    return true;
                case TelescopeAxes.axisSecondary:
                    return true;
                case TelescopeAxes.axisTertiary:
                    return false;
                default:
                    return false;
            }
        }

        public bool CanPark
        {
            get { return true; }
        }

        public bool CanPulseGuide
        {
            get { return true; }
        }

        public bool CanSetDeclinationRate
        {
            get { return true; }
        }

        public bool CanSetGuideRates
        {
            get { return true; }
        }

        public bool CanSetPark
        {
            get { return true; }
        }

        public bool CanSetPierSide
        {
            get { return true; }
        }

        public bool CanSetRightAscensionRate
        {
            get { return true; }
        }

        public bool CanSetTracking
        {
            get { return true; }
        }

        public bool CanSlew
        {
            get { return true; }
        }

        public bool CanSlewAltAz
        {
            get { return true; }
        }

        public bool CanSlewAltAzAsync
        {
            get { return true; }
        }

        public bool CanSlewAsync
        {
            get { return true; }
        }

        public bool CanSync
        {
            
            get { return true; }
        }

        public bool CanSyncAltAz
        {
            
            get { return true; }
        }

        public bool CanUnpark
        {
            
            get { return true; }
        }

        public void CommandBlind(string Command, bool Raw)
        {
            GeminiHardware.DoCommand(Command);
        }

        public bool CommandBool(string Command, bool Raw)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string Command, bool Raw)
        {
            return GeminiHardware.DoCommandResult(Command, 2000);
        }

        public bool Connected
        {
            get { return GeminiHardware.Connected; }
            set { GeminiHardware.Connected = value; }
        }

        public double Declination
        {
            get { return GeminiHardware.Declination; }
        }

        /// <summary>
        /// Set comet-tracking declination rate. This is the Gemini divisor value, valid 
        /// range is 0...65535
        /// </summary>
        public double DeclinationRate
        {
            get 
            { 
                string rate = GeminiHardware.DoCommandResult("<412:", 2000);
                if (rate != null) return int.Parse(rate);
                throw new TimeoutException("DeclinationRate");
            }

            set 
            {
                int val = (int)value;
                if (val < 0 || val > 65535) throw new HelperNET.Exceptions.InvalidValueException("DeclinationRate");
                string cmd = ">412:" + ((int)(value)).ToString();
                GeminiHardware.DoCommand(cmd);
            }
        }

        public string Description
        {
            get { return SharedResources.TELESCOPE_DRIVER_DESCRIPTION; }
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
            get { return SharedResources.TELESCOPE_DRIVER_INFO; }
        }

        public string DriverVersion
        {
            get { return "1"; }
        }

        public EquatorialCoordinateType EquatorialSystem
        {
            
            get { return EquatorialCoordinateType.equJ2000; }
        }

        public void FindHome()
        {
            
            GeminiHardware.DoCommand(":hP");
        }

        public double FocalLength
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("FocalLength", false); }
        }

        /// <summary>
        /// Same guide rate as RightAscension;
        /// </summary>
        public double GuideRateDeclination
        {
            get { return GuideRateRightAscension; }
            set { GuideRateRightAscension = value;  }
        }

        /// <summary>
        /// Get/Set guiding rate in degrees/second
        /// Actual Gemini rates are 0.2 - 0.8x Sidereal
        /// </summary>
        public double GuideRateRightAscension
        {         
            get {
                string result = GeminiHardware.DoCommandResult("<150:", 2000);
                if (result == null) throw new TimeoutException("GuideRateRightAscention");
                return double.Parse(result) * SharedResources.EARTH_ANG_ROT_DEG_MIN/60.0;    //may need to process this differently if int'l settings have ',' as decimal point.!!!
            }
            set 
            {
                double val = value/(SharedResources.EARTH_ANG_ROT_DEG_MIN / 60.0) ;

                if (val < 0.2 || val > 0.8) throw new HelperNET.Exceptions.InvalidValueException("GuideRate out of range 0.2-0.8x Sidereal, value: " + val.ToString("0.0"));
                string cmd = ">150:" + value.ToString("0.0");    //internationalization issues?
                GeminiHardware.DoCommand(cmd);                
            }
        }

        public short InterfaceVersion
        {
            
            get { return 2; }
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
            
            get { return SharedResources.TELESCOPE_DRIVER_NAME; }
        }

        public void Park()
        {
           // string[] cmd = { ":hP", ":hN" };
           // GeminiHardware.DoCommand(cmd);
            GeminiHardware.DoCommand(":hP");
            while (GeminiHardware.ParkState == "2") { };
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("PulseGuide");
        }

        public double RightAscension
        {
            
            get { return GeminiHardware.RightAscension; }
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
            if (GeminiHardware.Connected)
            {
                throw new DriverException("The hardware is connected, cannot do SetupDialog()",
                                    unchecked(ErrorCodes.DriverBase + 4));
            }
            GeminiTelescope.m_MainForm.DoTelescopeSetupDialog();
        }

        public PierSide SideOfPier
        {
            // TODO Replace this with your implementation
            get 
            {
                if (GeminiHardware.SideOfPier == "E")
                {
                    return PierSide.pierEast;
                }
                else if (GeminiHardware.SideOfPier == "W")
                {
                    return PierSide.pierWest;
                }
                else
                {
                    return PierSide.pierUnknown;
                }
            }
            set 
            {
                if ((value == PierSide.pierEast && GeminiHardware.SideOfPier == "W") || (value == PierSide.pierWest && GeminiHardware.SideOfPier == "E"))
                {
                    GeminiHardware.DoCommand(":Mf");
                }
            }
        }

        public double SiderealTime
        {
            get { return GeminiHardware.SiderealTime; }
        }

        public double SiteElevation
        {
            // TODO Replace this with your implementation
            get { return GeminiHardware.Elevation; }
            set 
            {
                if (value < -300 || value > 10000)
                {
                    throw new HelperNET.Exceptions.InvalidValueException("SiteElevation out of range: " + value.ToString());
                }
                GeminiHardware.Elevation = value; 
            }
        }

        public double SiteLatitude
        {
            // TODO Replace this with your implementation
            get { return GeminiHardware.Latitude; }
            set 
            {
                if (value < -90 || value > 90)
                {
                    throw new HelperNET.Exceptions.InvalidValueException("SiteLatitude out of range: "  + value.ToString());
                }
                GeminiHardware.SetLatitude(value);

            }
        }

        public double SiteLongitude
        {
            // TODO Replace this with your implementation
            get { return GeminiHardware.Longitude; }
            set
            {
                if (value < -180 || value > 180)
                {
                    throw new HelperNET.Exceptions.InvalidValueException("SiteLongitude out of range: "+ value.ToString());
                }
                GeminiHardware.SetLongitude(value);

            }
        }

        public short SlewSettleTime
        {
            // TODO Replace this with your implementation
            get { throw new PropertyNotImplementedException("SlewSettleTime", false); }
            set { throw new PropertyNotImplementedException("SlewSettleTime", true); }
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            GeminiHardware.TargetAzimuth = Azimuth;
            GeminiHardware.TargetAltitude = Altitude;
            GeminiHardware.SlewHorizon();
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            GeminiHardware.TargetAzimuth = Azimuth;
            GeminiHardware.TargetAltitude = Altitude;
            GeminiHardware.SlewHorizonAsync();
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            if (GeminiHardware.AtHome || GeminiHardware.AtPark)
                throw new DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);

            GeminiHardware.TargetRightAscension = RightAscension;
            GeminiHardware.TargetDeclination = Declination;
            GeminiHardware.SlewEquatorial();
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            if (GeminiHardware.AtHome || GeminiHardware.AtPark)
                throw new DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);

            GeminiHardware.TargetRightAscension = RightAscension;
            GeminiHardware.TargetDeclination = Declination;
            GeminiHardware.SlewEquatorialAsync();
        }

        public void SlewToTarget()
        {
            if (GeminiHardware.AtHome || GeminiHardware.AtPark)
                throw new DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);

            GeminiHardware.SlewEquatorial();
        }

        public void SlewToTargetAsync()
        {
            if (GeminiHardware.AtHome || GeminiHardware.AtPark)
                throw new DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);
            GeminiHardware.SlewEquatorialAsync();
        }

        public bool Slewing
        {
            
            get 
            {
                
                if (GeminiHardware.Velocity == "S")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            // TODO Replace this with your implementation
            throw new MethodNotImplementedException("SyncToAltAz");
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            GeminiHardware.SyncToEquatorialCoords(RightAscension, Declination);
        }

        public void SyncToTarget()
        {
            if (TargetDeclination == SharedResources.INVALID_DOUBLE || TargetRightAscension == SharedResources.INVALID_DOUBLE)
                throw new DriverException(SharedResources.MSG_PROP_NOT_SET, (int)SharedResources.SCOPE_PROP_NOT_SET);
            GeminiHardware.SyncEquatorial();
        }

        public double TargetDeclination
        {
            get { 
                double val = GeminiHardware.TargetDeclination;
                if (val == SharedResources.INVALID_DOUBLE)
                    throw new HelperNET.Exceptions.ValueNotSetException("TargetDeclination is not set");
                return val;
            }
            set
            {
                if (value < -90 || value > 90)
                {
                    throw new HelperNET.Exceptions.InvalidValueException("TargetDeclination out of range: " + value.ToString());
                }
                GeminiHardware.TargetDeclination = value;
            }
        }

        public double TargetRightAscension
        {
            // TODO Replace this with your implementation
            get { 
                double val = GeminiHardware.TargetRightAscension;
                if (val == SharedResources.INVALID_DOUBLE)
                    throw new HelperNET.Exceptions.ValueNotSetException("TargetRightAscension is not set");
                return val;            
            }
            set 
            {
                if (value < 0 || value > 24)
                {
                    throw new HelperNET.Exceptions.InvalidValueException("TargetRightAscension is not set");
                }
                GeminiHardware.TargetRightAscension=value; 
            }
        }

        public bool Tracking
        {
            get { return GeminiHardware.Tracking; }
            set 
            {
                if (value && !GeminiHardware.Tracking)
                {
                    GeminiHardware.DoCommand(":hW");
                }
                if (!value && !GeminiHardware.Tracking)
                {
                    GeminiHardware.DoCommand(":hN");
                }
            }
        }

        public DriveRates TrackingRate
        {
            // TODO Replace this with your implementation
            get { return DriveRates.driveSidereal; }
            set {  }
        }

        public ITrackingRates TrackingRates
        {
            get { return m_TrackingRates; }
        }

        public DateTime UTCDate
        {
            // TODO Replace this with your implementation
            get { return DateTime.UtcNow; }
            set {  }
        }

        public void Unpark()
        {
            GeminiHardware.DoCommand(":hW");
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
    [Guid("84f0df4b-8d54-41d6-be96-1b8a3c98ef03")]
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
    [Guid("c9809d46-a7aa-4876-8631-47222a34707f")]
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
    [Guid("e1108c19-e0bb-472e-8bb2-29263f331971")]
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
            m_TrackingRates = new DriveRates[] { DriveRates.driveSidereal, DriveRates.driveKing, DriveRates.driveLunar, DriveRates.driveSolar };
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
