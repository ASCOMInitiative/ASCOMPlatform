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
using ASCOM.Utilities;
using ASCOM.Interface;
using ASCOM.GeminiTelescope;

/// <summary>
/// Need to add CommandNative to standard ITelescope interface for backward compatibility with
/// old Gemini ASCOM driver
/// </summary>
/// 
[Guid("EF0C67AD-A9D3-4F7B-A635-CD2095517633")]
[TypeLibType(4288)]
public interface IGeminiTelescope
{
    [DispId(101)]
    AlignmentModes AlignmentMode { get; }
    [DispId(102)]
    double Altitude { get; }
    [DispId(103)]
    double ApertureArea { get; }
    [DispId(104)]
    double ApertureDiameter { get; }
    [DispId(105)]
    bool AtHome { get; }
    [DispId(106)]
    bool AtPark { get; }
    [DispId(107)]
    double Azimuth { get; }
    [DispId(108)]
    bool CanFindHome { get; }
    [DispId(109)]
    bool CanPark { get; }
    [DispId(110)]
    bool CanPulseGuide { get; }
    [DispId(111)]
    bool CanSetDeclinationRate { get; }
    [DispId(112)]
    bool CanSetGuideRates { get; }
    [DispId(113)]
    bool CanSetPark { get; }
    [DispId(115)]
    bool CanSetPierSide { get; }
    [DispId(114)]
    bool CanSetRightAscensionRate { get; }
    [DispId(116)]
    bool CanSetTracking { get; }
    [DispId(117)]
    bool CanSlew { get; }
    [DispId(118)]
    bool CanSlewAltAz { get; }
    [DispId(119)]
    bool CanSlewAltAzAsync { get; }
    [DispId(120)]
    bool CanSlewAsync { get; }
    [DispId(121)]
    bool CanSync { get; }
    [DispId(122)]
    bool CanSyncAltAz { get; }
    [DispId(123)]
    bool CanUnpark { get; }
    [DispId(124)]
    bool Connected { get; set; }
    [DispId(125)]
    double Declination { get; }
    [DispId(126)]
    double DeclinationRate { get; set; }
    [DispId(127)]
    string Description { get; }
    [DispId(128)]
    bool DoesRefraction { get; set; }
    [DispId(129)]
    string DriverInfo { get; }
    [DispId(130)]
    string DriverVersion { get; }
    [DispId(131)]
    EquatorialCoordinateType EquatorialSystem { get; }
    [DispId(132)]
    double FocalLength { get; }
    [DispId(133)]
    double GuideRateDeclination { get; set; }
    [DispId(134)]
    double GuideRateRightAscension { get; set; }
    [DispId(135)]
    short InterfaceVersion { get; }
    [DispId(136)]
    bool IsPulseGuiding { get; }
    [DispId(137)]
    string Name { get; }
    [DispId(138)]
    double RightAscension { get; }
    [DispId(139)]
    double RightAscensionRate { get; set; }
    [DispId(140)]
    PierSide SideOfPier { get; set; }
    [DispId(141)]
    double SiderealTime { get; }
    [DispId(142)]
    double SiteElevation { get; set; }
    [DispId(143)]
    double SiteLatitude { get; set; }
    [DispId(144)]
    double SiteLongitude { get; set; }
    [DispId(145)]
    bool Slewing { get; }
    [DispId(146)]
    short SlewSettleTime { get; set; }
    [DispId(147)]
    double TargetDeclination { get; set; }
    [DispId(148)]
    double TargetRightAscension { get; set; }
    [DispId(149)]
    bool Tracking { get; set; }
    [DispId(150)]
    DriveRates TrackingRate { get; set; }
    [DispId(151)]
    ITrackingRates TrackingRates { get; }
    [DispId(152)]
    DateTime UTCDate { get; set; }

    [DispId(401)]
    void AbortSlew();
    [DispId(402)]
    IAxisRates AxisRates(TelescopeAxes Axis);
    [DispId(403)]
    bool CanMoveAxis(TelescopeAxes Axis);
    [DispId(421)]

    void CommandBlind(string Command, [DefaultParameterValue(false)] bool Raw);
    [DispId(422)]
    bool CommandBool(string Command, [DefaultParameterValue(false)]bool Raw);
    [DispId(423)]
    string CommandString(string Command, [DefaultParameterValue(false)]bool Raw);
    [DispId(404)]
    PierSide DestinationSideOfPier(double RightAscension, double Declination);
    [DispId(405)]
    void FindHome();
    [DispId(406)]
    void MoveAxis(TelescopeAxes Axis, double Rate);
    [DispId(407)]
    void Park();
    [DispId(408)]
    void PulseGuide(GuideDirections Direction, int Duration);
    [DispId(409)]
    void SetPark();
    [DispId(410)]
    void SetupDialog();
    [DispId(411)]
    void SlewToAltAz(double Azimuth, double Altitude);
    [DispId(412)]
    void SlewToAltAzAsync(double Azimuth, double Altitude);
    [DispId(413)]
    void SlewToCoordinates(double RightAscension, double Declination);
    [DispId(414)]
    void SlewToCoordinatesAsync(double RightAscension, double Declination);
    [DispId(415)]
    void SlewToTarget();
    [DispId(416)]
    void SlewToTargetAsync();
    [DispId(417)]
    void SyncToAltAz(double Azimuth, double Altitude);
    [DispId(418)]
    void SyncToCoordinates(double RightAscension, double Declination);
    [DispId(419)]
    void SyncToTarget();
    [DispId(420)]
    void Unpark();
    [DispId(424)]
    string CommandNative(string Command);
}

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
    public class Telescope : ReferenceCountedObjectBase, IGeminiTelescope
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
            GeminiHardware.DoCommandResult(":Q", 1000, false);
        }

        public AlignmentModes AlignmentMode
        {
            get { return AlignmentModes.algGermanPolar; }
        }

        public double Altitude
        {
            get { return GeminiHardware.Altitude; }
        }

        public double ApertureArea
        {
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
            get { return false; }
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

        public string CommandNative(string Command)
        {
            string result = GeminiHardware.DoCommandResult(Command, 1000, false);

            if (result == null) return "";
            else            
            if (result.EndsWith("#")) return result.Substring(result.Length-1);

            return result;
        }

        public void CommandBlind(string Command, bool Raw)
        {
            GeminiHardware.DoCommandResult(Command, 10000, Raw);
        }

        public bool CommandBool(string Command, bool Raw)
        {
            if (Command == "") throw new InvalidValueException("CommandBool", "", "");
            string result = GeminiHardware.DoCommandResult(Command, 1000, Raw);
            if (result!=null && result.StartsWith("1")) return true;
            return false;
        }

        public string CommandString(string Command, bool Raw)
        {
            string result = GeminiHardware.DoCommandResult(Command, 1000, Raw);
            if (result == null) return null;
            if (result.EndsWith("#")) return result.Substring(result.Length - 1);
            return result;
        }

        public bool Connected
        {
            get { return GeminiHardware.Connected; }
            set { 
                GeminiHardware.Connected = value;
                if (value && !GeminiHardware.Connected) throw new ASCOM.Utilities.Exceptions.SerialPortInUseException("Connect");
            }
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
                string rate = GeminiHardware.DoCommandResult("<412:", 2000, false);
                if (rate != null) return int.Parse(rate);
                throw new TimeoutException("DeclinationRate");
            }

            set 
            {
                int val = (int)value;
                if (val < 0 || val > 65535) throw new InvalidValueException("DeclinationRate", value.ToString(), "0..65535");
                string cmd = ">412:" + ((int)(value)).ToString();
                GeminiHardware.DoCommandResult(cmd, 1000, false);
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


        private void WaitForHomeOrPark(string where)
        {
            int count = 0;

            // wait for parking move to begin, wait for a maximum of 16*250ms = 4 seconds
            while (GeminiHardware.ParkState != "2" && count < 16) { System.Threading.Thread.Sleep(250); count++; }
            if (count == 16) throw new TimeoutException(where + " operation didn't start");

            // now wait for it to end
            while (GeminiHardware.ParkState == "2") { System.Threading.Thread.Sleep(1000); };

            // 0 => didn't park.
            if (GeminiHardware.ParkState == "0") throw new DriverException("Failed to " + where, (int)SharedResources.ERROR_BASE);
        }

        public void FindHome()
        {
            if (GeminiHardware.AtHome) return;

            if (GeminiHardware.AtPark)
                throw new DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);

            GeminiHardware.DoCommandResult(":hP", 1000, false);
            WaitForHomeOrPark("Home");
            GeminiHardware.DoCommandResult(":hN", 1000, false); //resume tracking, as FindHome isn't supposed to stop the mount
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
                string result = GeminiHardware.DoCommandResult("<150:", 2000, false);
                if (result == null) throw new TimeoutException("GuideRateRightAscention");
                return double.Parse(result) * SharedResources.EARTH_ANG_ROT_DEG_MIN / 60.0;
            }
            set 
            {
                double val = value/(SharedResources.EARTH_ANG_ROT_DEG_MIN / 60.0) ;
                if (val < 0.2 || val > 0.8) throw new InvalidValueException("GuideRate", value.ToString(),"");
                string cmd = ">150:" + val.ToString("0.0");    //internationalization issues?
                GeminiHardware.DoCommandResult(cmd, 1000, false);                
            }
        }

        public short InterfaceVersion
        {
            
            get { return 2; }
        }

        public bool IsPulseGuiding
        {
            get { return GeminiHardware.IsPulseGuiding; }
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
            if (GeminiHardware.AtPark) return;  // already there

           // string[] cmd = { ":hP", ":hN" };
           // GeminiHardware.DoCommand(cmd);
            GeminiHardware.DoCommandResult(":hC", 1000, false);

            WaitForHomeOrPark("Park");
            GeminiHardware.DoCommandResult(":hN", 1000, false);
        }

        /// <summary>
        /// Send pulse-guide commands to the mount in the required direction, for the required duration
        /// </summary>
        /// <param name="Direction"></param>
        /// <param name="Duration"></param>
        public void PulseGuide(GuideDirections Direction, int Duration)
        {

            if (GeminiHardware.AtPark)
                throw new DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);

            string cmd = String.Empty;

            switch (Direction)
            {
                case GuideDirections.guideEast:
                    cmd = ":Mge";
                    break;
                case GuideDirections.guideNorth:
                    cmd = ":Mgn";
                    break;
                case GuideDirections.guideSouth:
                    cmd = ":Mgs";
                    break;
                case GuideDirections.guideWest:
                    cmd = ":Mgw";
                    break;
            }

            if (Duration > 10000 || Duration < 0)  // too large or negative...
                throw new InvalidValueException("PulseGuide" , Duration.ToString(), "0..10000");

            string[] cmds = new string[1 + Duration / 256];

            int count = Duration;
            for (int idx = 0; count > 0; ++idx, count -= 255)
            {
                int d = (count > 255 ? 255 : count);
                cmds[idx] = cmd + d.ToString();
                count -= d;
            }
            GeminiHardware.IsPulseGuiding = true;
            GeminiHardware.DoCommandResult(cmds, Duration + 1000, false);
            GeminiHardware.IsPulseGuiding = false;
        }

        public double RightAscension
        {
            
            get { return GeminiHardware.RightAscension; }
        }

        public double RightAscensionRate
        {
            // TODO Replace this with your implementation
            get { return 0;  }
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
                    GeminiHardware.DoCommandResult(":Mf", 1000, false);
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
                    throw new InvalidValueException("SiteElevation", value.ToString(), "-300...10000");
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
                    throw new InvalidValueException("SiteLatitude", value.ToString(), "-90..90");
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
                    throw new InvalidValueException("SiteLongitude",value.ToString(), "-180..180");
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
            GeminiHardware.SyncHorizonCoordinates(Azimuth, Altitude);
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            if (GeminiHardware.AtPark)
                    throw new DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);

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
                    throw new ValueNotSetException("TargetDeclination");
                return val;
            }
            set
            {
                if (value < -90 || value > 90)
                {
                    throw new InvalidValueException("TargetDeclination", value.ToString(), "-90..90");
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
                    throw new ValueNotSetException("TargetRightAscension is not set");
                return val;            
            }
            set 
            {
                if (value < 0 || value > 24)
                {
                    throw new InvalidValueException("TargetRightAscension", value.ToString(), "0..24");
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
                    GeminiHardware.DoCommandResult(":hW", 1000, false);
                }
                if (!value && !GeminiHardware.Tracking)
                {
                    GeminiHardware.DoCommandResult(":hN", 1000, false);
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
            GeminiHardware.DoCommandResult(":hW", 5000, false);
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
