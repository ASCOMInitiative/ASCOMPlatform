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
//              (pk)  Paul Kanevsky <paul@pk.darkhorizons.org>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 15-JUL-2009	rbt	1.0.0	Initial edit, from ASCOM Telescope Driver template
// 08-JUL-2009  pk  1.0.1   Full implementation of ITelescope interface, passing Conform test.
// --------------------------------------------------------------------------------
//
using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

using ASCOM;
using ASCOM.Utilities;
using ASCOM.Interface;
using ASCOM.GeminiTelescope;
using ASCOM.Conform;
using System.IO;

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
    [DispId(425)]
    IConformCommandStrings ConformCommands{ get; }
    [DispId(426)]
    IConformCommandStrings ConformCommandsRaw{ get; }
    [DispId(427)]
    IConformErrorNumbers ConformErrors{ get; }

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
        private AxisRates[] m_AxisRates = null;
        private TrackingRates m_TrackingRates;

        private bool m_FoundHome = false;

        private bool m_AsyncSlewStarted = false;

        //
        // Constructor - Must be public for COM registration!
        //
        public Telescope()
        {/*
            m_AxisRates = new AxisRates[3];
            m_AxisRates[0] = new AxisRates(TelescopeAxes.axisPrimary);
            m_AxisRates[1] = new AxisRates(TelescopeAxes.axisSecondary);
            m_AxisRates[2] = new AxisRates(TelescopeAxes.axisTertiary);
          */
            m_TrackingRates = new TrackingRates();
            // TODO Implement your additional construction here
        }

        #region Private Code

        /// <summary>
        /// Processes a command string that comes from outside the driver and may have its leading 
        /// colon missing. It appends one if necessary which ensures correct processing elsewhere 
        /// in the driver
        /// </summary>
        /// <param name="cmd">The command string to be checked for leading colon</param>
        /// <returns>A command string with leading colon where required.</returns>
        internal string PrepareCommand(string cmd)
        {
            switch (cmd.Substring(0, 1))
            {
                case "<": //Do nothing
                    break;
                case ">": //Do nothing
                    break;
                case ":": //Do nothing
                    break;
                default: //Prepend :
                    cmd = ":" + cmd;
                    break;
            }
            return cmd;
        }
        #endregion
        //
        // PUBLIC COM INTERFACE ITelescope IMPLEMENTATION
        //

        #region ITelescope Members

        public void AbortSlew()
        {
            GeminiHardware.Trace.Enter("IT:IT:AbortSlew");
            if (GeminiHardware.AtHome || GeminiHardware.AtPark)              
                throw new DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);
            GeminiHardware.DoCommandResult(":Q", GeminiHardware.MAX_TIMEOUT, false);
            GeminiHardware.Trace.Exit("IT:AbortSlew");
        }

        public AlignmentModes AlignmentMode
        {

            get {
                GeminiHardware.Trace.Enter("IT:IT:AlignmentMode.Get", AlignmentModes.algGermanPolar);
                return AlignmentModes.algGermanPolar; 
            }
        }

        public double Altitude
        {
            get {
                GeminiHardware.Trace.Enter("IT:IT:Altitude.Get", GeminiHardware.Altitude);
                return GeminiHardware.Altitude;
            }
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
            get {
                GeminiHardware.Trace.Enter("IT:AtHome.Get", m_FoundHome);                
                return m_FoundHome; }
        }

        public bool AtPark
        {
            get {
                GeminiHardware.Trace.Enter("IT:AtPark.Get", GeminiHardware.AtPark);
                return GeminiHardware.AtPark;
            }
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
            GeminiHardware.Trace.Enter("IT:AxisRates");

            if (m_AxisRates == null)
            {
                if (GeminiHardware.Connected)
                {
                    m_AxisRates = new AxisRates[3];
                    m_AxisRates[0] = new AxisRates(TelescopeAxes.axisPrimary);
                    m_AxisRates[1] = new AxisRates(TelescopeAxes.axisSecondary);
                    m_AxisRates[2] = new AxisRates(TelescopeAxes.axisTertiary);
                }
                else
                    return null;
            }

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
            get {
                GeminiHardware.Trace.Enter("IT:Azimuth.Get", GeminiHardware.Azimuth);
                return GeminiHardware.Azimuth; }
        }

        public bool CanFindHome
        {
            get {
                GeminiHardware.Trace.Enter("IT:CanFindHome.Get", true);
                return true;
            }
        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary:
                    GeminiHardware.Trace.Enter("IT:CanMoveAxis.Get", Axis, true);
                    return true;
                case TelescopeAxes.axisSecondary:
                    GeminiHardware.Trace.Enter("IT:CanMoveAxis.Get", Axis, true);
                    return true;
                case TelescopeAxes.axisTertiary:
                    GeminiHardware.Trace.Enter("IT:CanMoveAxis.Get", Axis, false);
                    return false;
                default:
                    GeminiHardware.Trace.Enter("IT:CanMoveAxis.Get", Axis, false);
                    return false;
            }
        }

        public bool CanPark
        {
            get {
                GeminiHardware.Trace.Enter("IT:CanPark.Get", true);
                return true;
            }
        }

        public bool CanPulseGuide
        {
            get {
                GeminiHardware.Trace.Enter("IT:CanPulseGuide.Get", true);
                return true;
            }
        }

        public bool CanSetDeclinationRate
        {
            get {
                GeminiHardware.Trace.Enter("IT:CanSetDeclinationRate.Get", true);
                return true; }
        }

        public bool CanSetGuideRates
        {
            get {
                GeminiHardware.Trace.Enter("IT:CanSetGuideRates.Get", true);
                return true;
            }
        }

        public bool CanSetPark
        {
            get {
                GeminiHardware.Trace.Enter("IT:CanSetPark.Get", true);                
                return true; }
        }

        public bool CanSetPierSide
        {
            get {
                GeminiHardware.Trace.Enter("IT:CanSetPierSide.Get", true);                
                return true; }
        }

        public bool CanSetRightAscensionRate
        {
            get {
                GeminiHardware.Trace.Enter("IT:CanSetRightAscensionRate.Get", false);
                return false;
            }
        }

        public bool CanSetTracking
        {
            get {
                GeminiHardware.Trace.Enter("IT:CanSetTracking.Get", true);
                return true; }
        }

        public bool CanSlew
        {
            get {
                GeminiHardware.Trace.Enter("IT:CanSlew.Get", true);
                return true;
            }
        }

        public bool CanSlewAltAz
        {
            get {
                GeminiHardware.Trace.Enter("IT:CanSlewAltAz.Get", true);
                return true;
            }
        }

        public bool CanSlewAltAzAsync
        {
            get {
                GeminiHardware.Trace.Enter("IT:CanSlewAltAzAsync.Get", true);
                return true;
            }
        }

        public bool CanSlewAsync
        {
            get {
                GeminiHardware.Trace.Enter("IT:CanSlewAsync.Get", true);
                return true;
            }
        }

        public bool CanSync
        {
            
            get {
                GeminiHardware.Trace.Enter("IT:CanSync.Get", true);
                return true;
            }
        }

        public bool CanSyncAltAz
        {
            
            get {
                GeminiHardware.Trace.Enter("IT:CanSyncAltAz.Get", true);
                return true;
            }
        }

        public bool CanUnpark
        {
            
            get {
                GeminiHardware.Trace.Enter("IT:CanUnpark.Get", true);
                return true;
            }
        }

        public string CommandNative(string Command)
        {
            GeminiHardware.Trace.Enter("IT:CommandNative", Command);

            if (Command == String.Empty) throw new ASCOM.InvalidValueException("CommandNative", Command, "valid Gemini command");
            string result = GeminiHardware.DoCommandResult(Command, 1000, false);

            if (result == null) return "";
            else            
            if (result.EndsWith("#")) return result.Substring(result.Length-1);

            GeminiHardware.Trace.Exit("IT:CommandNative", Command, result);
            return result;
        }

        public void CommandBlind(string Command, bool Raw)
        {
            GeminiHardware.Trace.Enter("IT:CommandBlind", Command, Raw);

            if (Command == String.Empty) throw new ASCOM.InvalidValueException("CommandBlind", Command, "valid Gemini command");
            Command = PrepareCommand(Command); // Add leading colon if required
            GeminiHardware.DoCommandResult(Command, GeminiHardware.MAX_TIMEOUT, Raw);
            GeminiHardware.Trace.Exit("IT:CommandBlind", Command);
        }

        public bool CommandBool(string Command, bool Raw)
        {
            GeminiHardware.Trace.Enter("IT:CommandBool", Command, Raw);
            if (Command == "") throw new InvalidValueException("CommandBool", "", "valid Gemini command");
            Command = PrepareCommand(Command); // Add leading colon if required
            string result = GeminiHardware.DoCommandResult(Command, GeminiHardware.MAX_TIMEOUT, Raw);

            bool bRes = (result!=null && result.StartsWith("1"));
            GeminiHardware.Trace.Exit("IT:CommandBool", Command, bRes);
            return bRes;
        }

        public string CommandString(string Command, bool Raw)
        {

            GeminiHardware.Trace.Enter("IT:CommandString", Command, Raw);
            if (Command == String.Empty) throw new ASCOM.InvalidValueException("CommandString", Command, "valid Gemini command");
            Command = PrepareCommand(Command); // Add leading colon if required
            string result = GeminiHardware.DoCommandResult(Command, GeminiHardware.MAX_TIMEOUT, Raw);
            if (result == null) return null;
            if (!Raw & result.EndsWith("#")) return result.Substring(1,result.Length - 1);//Added Start value substring parameter and handling of Raw values
            GeminiHardware.Trace.Exit("IT:CommandString", Command, result);
            return result;
        }

        public bool Connected
        {
            get {
                GeminiHardware.Trace.Enter("IT:Connected.Get", GeminiHardware.Connected);                
                return GeminiHardware.Connected; }
            set {
                GeminiHardware.Trace.Enter("IT:Connected.Set", value);                
                GeminiHardware.Connected = value;
                if (value && !GeminiHardware.Connected) throw new ASCOM.Utilities.Exceptions.SerialPortInUseException("Connect");
                GeminiHardware.Trace.Exit("IT:Connected.Set", value);                
            }
        }

        public double Declination
        {
            get {
                GeminiHardware.Trace.Enter("IT:Connected.Set", GeminiHardware.Declination);                 
                return GeminiHardware.Declination; }
        }

        /// <summary>
        /// Set comet-tracking declination rate. This is the Gemini divisor value, valid 
        /// range is 0...65535
        /// </summary>
        public double DeclinationRate
        {
            get 
            {
                GeminiHardware.Trace.Enter("IT:DeclinationRate.Get");
                string rate = GeminiHardware.DoCommandResult("<412:", GeminiHardware.MAX_TIMEOUT, false);
                if (rate != null)
                {
                    GeminiHardware.Trace.Exit("IT:DeclinationRate.Get", rate);
                    return int.Parse(rate);
                }
                throw new TimeoutException("DeclinationRate");
            }

            set 
            {
                GeminiHardware.Trace.Enter("IT:DeclinationRate.Set", value);
                int val = (int)value;
                if (val < 0 || val > 65535) throw new InvalidValueException("DeclinationRate", value.ToString(), "0..65535");
                string cmd = ">412:" + ((int)(value)).ToString();
                GeminiHardware.DoCommandResult(cmd, GeminiHardware.MAX_TIMEOUT, false);
                GeminiHardware.Trace.Exit("IT:DeclinationRate.Set", value);
            }
        }

        public string Description
        {
            get {
                GeminiHardware.Trace.Enter("IT:Descriptoin.Get", SharedResources.TELESCOPE_DRIVER_DESCRIPTION);
                return SharedResources.TELESCOPE_DRIVER_DESCRIPTION;
            }
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            throw new ASCOM.MethodNotImplementedException("DestinationSideOfPier"); // Was PropertyNotImplementedException
        }

        public bool DoesRefraction
        {
            get {
                bool bRef = GeminiHardware.Refraction;
                GeminiHardware.Trace.Enter("IT:DoesRefraction.Get", bRef);
                return bRef; 
            }
            set {
                GeminiHardware.Trace.Enter("IT:DoesRefraction.Set", value);
                GeminiHardware.Refraction = value;
            }
        }

        public string DriverInfo
        {
            get 
            {

                Version GeminiVersion = Assembly.GetExecutingAssembly().GetName().Version;

                FileInfo oMyFile = new FileInfo(Assembly.GetExecutingAssembly().Location);
                DateTime oBuildDate = oMyFile.LastWriteTime;
                string res = SharedResources.TELESCOPE_DRIVER_INFO + " Version " + GeminiVersion.ToString() + " dated " +oBuildDate.ToLongDateString() + " " + oBuildDate.ToLongTimeString(); 

                GeminiHardware.Trace.Enter("IT:DriverInfo.Get", res);
                return res;

            }
        }

        public string DriverVersion
        {
            get {
                Version GeminiVersion = Assembly.GetExecutingAssembly().GetName().Version;
                string res = GeminiVersion.ToString(2); //Return just the major and minor version numbers
                GeminiHardware.Trace.Enter("IT:DriverVersion.Get", res);
                return res;
            }
        }

        public EquatorialCoordinateType EquatorialSystem
        {
            get { 
                
                EquatorialCoordinateType res = GeminiHardware.Precession ? EquatorialCoordinateType.equJ2000 : EquatorialCoordinateType.equLocalTopocentric;
                GeminiHardware.Trace.Enter("IT:EquatorialSystem.Get", res);
                return res; 
            }
            set
            {
                GeminiHardware.Trace.Enter("IT:EquatorialSystem.Set", value);
                if (value == EquatorialCoordinateType.equLocalTopocentric)
                    GeminiHardware.Precession = false;
                else
                    if (value == EquatorialCoordinateType.equJ2000)
                        GeminiHardware.Precession = true;
                    else
                        throw new InvalidValueException("EquatorialSystem", value.ToString(), "equLocalTopocentric (1), or equJ2000 (2)");
                GeminiHardware.Trace.Exit("IT:EquatorialSystem.Set", value);
            }

        }

        private void WaitForHomeOrPark(string where)
        {
            GeminiHardware.Trace.Enter(4, "WaitForHomeOrPark", where);

            int count = 0;

            // wait for parking move to begin, wait for a maximum of 16*250ms = 4 seconds
            while (GeminiHardware.ParkState != "2" && count < 16) { System.Threading.Thread.Sleep(250); count++; }
//            if (count == 16) throw new TimeoutException(where + " operation didn't start");

            // now wait for it to end
            while (GeminiHardware.ParkState == "2") { System.Threading.Thread.Sleep(1000); };

            // 0 => didn't park.
            //if (GeminiHardware.ParkState == "0") throw new DriverException("Failed to " + where, (int)SharedResources.ERROR_BASE);
            GeminiHardware.Trace.Exit(4, "WaitForHomeOrPark", where);
        }

        public void FindHome()
        {
            GeminiHardware.Trace.Enter("IT:FindHome");

            if (GeminiHardware.AtPark)
                throw new DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);

            if (GeminiHardware.AtHome) return;

            GeminiHardware.DoCommandResult(":hP", GeminiHardware.MAX_TIMEOUT, false);
            WaitForHomeOrPark("Home");
            GeminiHardware.DoCommandResult(":hW", GeminiHardware.MAX_TIMEOUT, false); //resume tracking, as FindHome isn't supposed to stop the mount
            WaitForVelocity("TG", GeminiHardware.MAX_TIMEOUT);
            m_FoundHome = true;
            GeminiHardware.Trace.Exit("IT:FindHome");
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
            get {
                GeminiHardware.Trace.Enter("IT:GuideRateDeclination.Get", GuideRateRightAscension);
                return GuideRateRightAscension; }
            set {
                GeminiHardware.Trace.Enter("IT:GuideRateDeclination.Set", value);
                GuideRateRightAscension = value;
                GeminiHardware.Trace.Exit("IT:GuideRateDeclination.Set", value);
            }
        }

        /// <summary>
        /// Get/Set guiding rate in degrees/second
        /// Actual Gemini rates are 0.2 - 0.8x Sidereal
        /// </summary>
        public double GuideRateRightAscension
        {         
            get {
                GeminiHardware.Trace.Enter("IT:GuideRateRightAscesion.Get");

                string result = GeminiHardware.DoCommandResult("<150:", GeminiHardware.MAX_TIMEOUT, false);
                if (result == null) throw new TimeoutException("GuideRateRightAscention");
                double res = double.Parse(result) * SharedResources.EARTH_ANG_ROT_DEG_MIN / 60.0;
                GeminiHardware.Trace.Exit("IT:GuideRateRightAscesion.Get", res);
                return res;
            }
            set 
            {
                GeminiHardware.Trace.Enter("IT:GuideRateRightAscesion.Set", value);

                double val = value/(SharedResources.EARTH_ANG_ROT_DEG_MIN / 60.0) ;
                if (val < 0.2 || val > 0.8) throw new InvalidValueException("GuideRate", value.ToString(),"");
                string cmd = ">150:" + val.ToString("0.0");    //internationalization issues?
                GeminiHardware.DoCommandResult(cmd, GeminiHardware.MAX_TIMEOUT, false);
                GeminiHardware.Trace.Exit("IT:GuideRateRightAscesion.Set", value);
            }
        }

        public short InterfaceVersion
        {
            
            get {
                GeminiHardware.Trace.Enter("IT:InterfaceVersion.Get", 2);
                                return 2; }
        }

        public bool IsPulseGuiding
        {
            get { 
                bool res = GeminiHardware.IsPulseGuiding;
                GeminiHardware.Trace.Enter("IT:IsPulseGuiding.Get", res);
                return res;
            }
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            GeminiHardware.Trace.Enter("IT:MoveAxis", Axis, Rate);

            if (GeminiHardware.AtPark) throw new DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);


            string[] cmds = { null, null };

            switch (Axis)
            {
                case TelescopeAxes.axisPrimary: //RA
                    if (Rate < 0) cmds[1] = ":Me";
                    else if (Rate > 0)
                        cmds[1] = ":Mw";
                    else
                    {
                        GeminiHardware.DoCommandResult(new string[] { ":Qe", ":Qw" }, GeminiHardware.MAX_TIMEOUT/2, false); //stop motion in RA
                        WaitForVelocity("T", GeminiHardware.MAX_TIMEOUT);
                        GeminiHardware.Trace.Exit("IT:MoveAxis", Axis, Rate);
                        return;
                    }
                    break;
                case TelescopeAxes.axisSecondary: //DEC
                    if (Rate < 0) cmds[1] = ":Ms";
                    else if (Rate > 0)
                        cmds[1] = ":Mn";
                    else
                    {
                        GeminiHardware.DoCommandResult(new string[] { ":Qn", ":Qs" }, GeminiHardware.MAX_TIMEOUT/2, false); //stop motion in DEC
                        WaitForVelocity("T", GeminiHardware.MAX_TIMEOUT);
                        GeminiHardware.Trace.Exit("IT:MoveAxis", Axis, Rate);
                        return;
                    }
                    break;
                default:
                    throw new ASCOM.InvalidValueException("MoveAxis", Axis.ToString(), "Primary,Secondary");
            }

            Rate = Math.Abs(Rate);

            const double RateTolerance = 1e-5;  // 1e-6 is 0.036 arcseconds/second

            // find the rate in the list of rates. The position will determine if it's
            // guiding, slewing, or centering rate:
            int cnt = 0;
            foreach (Rate r in AxisRates(Axis))
            {
                if (r.Minimum >= Rate-RateTolerance && r.Minimum <= Rate+RateTolerance) // use tolerance to ensure doubles compare properly
                    break;
                cnt++;
            }

            switch (cnt)
            {
                case 0: // slew rate
                    cmds[0] = ":RS"; break;
                case 1: // center rate
                    cmds[0] = ":RC"; break;
                case 2: // guide rate
                    cmds[0] = ":RG"; break;

                default:
                    throw new ASCOM.InvalidValueException("MoveAxis", Axis.ToString(), "guiding, centering, or slewing speeds");
            }

            GeminiHardware.DoCommandResult(cmds, GeminiHardware.MAX_TIMEOUT/2, false);
            WaitForVelocity("GCS", GeminiHardware.MAX_TIMEOUT);
            GeminiHardware.Trace.Exit("IT:MoveAxis", Axis, Rate);
        }

        public string Name
        {
            
            get {
                GeminiHardware.Trace.Enter("IT:Name.Get", SharedResources.TELESCOPE_DRIVER_NAME);
                return SharedResources.TELESCOPE_DRIVER_NAME;
            }
        }

        public void Park()
        {
            GeminiHardware.Trace.Enter("IT:Park");

            if (GeminiHardware.AtPark) return;  // already there

           // string[] cmd = { ":hP", ":hN" };
           // GeminiHardware.DoCommand(cmd);
            GeminiHardware.DoCommandResult(":hC", GeminiHardware.MAX_TIMEOUT, false);

            WaitForHomeOrPark("Park");
            GeminiHardware.Trace.Exit("IT:Park");
        }

        /// <summary>
        /// Send pulse-guide commands to the mount in the required direction, for the required duration
        /// </summary>
        /// <param name="Direction"></param>
        /// <param name="Duration"></param>
        public void PulseGuide(GuideDirections Direction, int Duration)
        {

            GeminiHardware.Trace.Enter("IT:PulseGuide", Direction, Duration);

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
            }
            GeminiHardware.DoCommandResult(cmds, Duration + GeminiHardware.MAX_TIMEOUT, false);
            GeminiHardware.Trace.Exit("IT:PulseGuide", Direction, Duration);
        
        }

        public double RightAscension
        {
            
            get {
                double res = GeminiHardware.RightAscension;
                GeminiHardware.Trace.Enter("IT:RightAscention", res);
                return res; }
        }

        public double RightAscensionRate
        {
            // TODO Replace this with your implementation
            get { return 0;  }
            set { throw new PropertyNotImplementedException("RightAscensionRate", true); }
        }

        public void SetPark()
        {
            throw new MethodNotImplementedException("SetPark");
        }

        public void SetupDialog()
        {
            GeminiHardware.Trace.Enter("IT:SetupDialog");

            if (GeminiHardware.Connected)
            {
                throw new DriverException("The hardware is connected, cannot do SetupDialog()",
                                    unchecked(ErrorCodes.DriverBase + 4));
            }
            GeminiTelescope.m_MainForm.DoTelescopeSetupDialog();
            GeminiHardware.Trace.Exit("IT:SetupDialog");
        }

        public PierSide SideOfPier
        {
            get 
            {
                if (GeminiHardware.SideOfPier == "E")
                {
                    GeminiHardware.Trace.Enter("IT:SideOfPier.Get", PierSide.pierEast);
                    return PierSide.pierEast;
                }
                else if (GeminiHardware.SideOfPier == "W")
                {
                    GeminiHardware.Trace.Enter("IT:SideOfPier.Get", PierSide.pierWest);
                    return PierSide.pierWest;
                }
                else
                {
                    GeminiHardware.Trace.Enter("IT:SideOfPier.Get", PierSide.pierUnknown);
                    return PierSide.pierUnknown;
                }
            }
            set 
            {
                GeminiHardware.Trace.Enter("IT:SideOfPier.Set", value);

                if ((value == PierSide.pierEast && GeminiHardware.SideOfPier == "W") || (value == PierSide.pierWest && GeminiHardware.SideOfPier == "E"))
                {
                    GeminiHardware.DoCommandResult(":Mf", -1 , false);
                    WaitForVelocity("S", GeminiHardware.MAX_TIMEOUT);
                    WaitForVelocity("TN", GeminiHardware.MAX_TIMEOUT);  // shouldn't this be waiting forever??? depends on whether :Mf is synchronous or not: need to check
                }
                GeminiHardware.Trace.Exit("IT:SideOfPier.Set", value);

            }
        }

        public double SiderealTime
        {
            get
            {
                double res = GeminiHardware.SiderealTime;
                GeminiHardware.Trace.Enter("IT:SiderealTime.Get", res);
                return res;
            }
        }

        public double SiteElevation
        {
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
            get { 
                
                double res = GeminiHardware.Latitude;
                GeminiHardware.Trace.Enter("IT:SiteLatitude.Get", res);
                return res;            
            }
            set 
            {
                GeminiHardware.Trace.Enter("IT:SiteLatitude.Set", value);

                if (value < -90 || value > 90)
                {
                    throw new ASCOM.InvalidValueException("SiteLatitude", value.ToString(), "-90..90");
                }
                GeminiHardware.SetLatitude(value);
                GeminiHardware.Trace.Exit("IT:SiteLatitude.Set", value);
            }
        }

        public double SiteLongitude
        {
            get { 
                double res = GeminiHardware.Longitude;
                GeminiHardware.Trace.Enter("IT:SiteLongitude.Get", res);
                return res;            
            }
            set
            {
                GeminiHardware.Trace.Enter("IT:SiteLongitude.Set", value);

                if (value < -180 || value > 180)
                {
                    throw new ASCOM.InvalidValueException("SiteLongitude",value.ToString(), "-180..180");
                }
                GeminiHardware.SetLongitude(value);
                GeminiHardware.Trace.Exit("IT:SiteLongitude.Set", value);
            }
        }

        public short SlewSettleTime
        {
            get {
                GeminiHardware.Trace.Enter("IT:SlewSettleTime.Get", GeminiHardware.SlewSettleTime);
                return (short)GeminiHardware.SlewSettleTime;  
            }
            set {
                GeminiHardware.Trace.Enter("IT:SlewSettleTime.Set", value);

                if (value < 1 || value > 120) throw new ASCOM.InvalidValueException("SlewSettleTime", value.ToString(), "1-120 seconds");    
                GeminiHardware.SlewSettleTime = (int)value;
                GeminiHardware.Trace.Exit("IT:SlewSettleTime.Set", value);
            }
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            GeminiHardware.Trace.Enter("IT:SlewToAltAz", Azimuth, Altitude);
            GeminiHardware.TargetAzimuth = Azimuth;
            GeminiHardware.TargetAltitude = Altitude;
            GeminiHardware.Velocity = "S";
            GeminiHardware.SlewHorizon();
            WaitForSlewToEnd();
            GeminiHardware.Trace.Exit("IT:SlewToAltAz", Azimuth, Altitude);
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            GeminiHardware.Trace.Enter("IT:SlewToAltAzAsync", Azimuth, Altitude);

            GeminiHardware.TargetAzimuth = Azimuth;
            GeminiHardware.TargetAltitude = Altitude;
            if (Slewing) AbortSlew();
            GeminiHardware.Velocity = "S";
            GeminiHardware.SlewHorizonAsync();
            WaitForVelocity("SC", GeminiHardware.MAX_TIMEOUT);
            m_AsyncSlewStarted = true;
            GeminiHardware.Trace.Exit("IT:SlewToAltAzAsync", Azimuth, Altitude);
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            GeminiHardware.Trace.Enter("IT:SlewToCoordinates", RightAscension, Declination);

            if (GeminiHardware.AtHome || GeminiHardware.AtPark)
                throw new DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);

            GeminiHardware.TargetRightAscension = RightAscension;
            GeminiHardware.TargetDeclination = Declination;
            if (Slewing) AbortSlew();
            GeminiHardware.Velocity = "S";
            GeminiHardware.SlewEquatorial();

            WaitForSlewToEnd();
            GeminiHardware.Trace.Exit("IT:SlewToCoordinates", RightAscension, Declination);
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            GeminiHardware.Trace.Enter("IT:SlewToCoordinatesAsync", RightAscension, Declination);

            if (GeminiHardware.AtHome || GeminiHardware.AtPark)
                throw new ASCOM.DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);

            GeminiHardware.TargetRightAscension = RightAscension;
            GeminiHardware.TargetDeclination = Declination;
            if (Slewing) AbortSlew();
            GeminiHardware.Velocity = "S";
            GeminiHardware.SlewEquatorialAsync();
            WaitForVelocity("SC", GeminiHardware.MAX_TIMEOUT);
            m_AsyncSlewStarted = true;
            GeminiHardware.Trace.Exit("IT:SlewToCoordinatesAsync", RightAscension, Declination);
        }

        private void WaitForSlewToEnd()
        {
            GeminiHardware.Trace.Enter(4, "WaitForSlewToEnd");

            GeminiHardware.Velocity = "";

            int when = System.Environment.TickCount + 5000;
            while (System.Environment.TickCount < when && !(GeminiHardware.Velocity=="S"  || GeminiHardware.Velocity=="C"))
                System.Threading.Thread.Sleep(500);

            while (GeminiHardware.Velocity=="S"  || GeminiHardware.Velocity=="C") System.Threading.Thread.Sleep(500);

            System.Threading.Thread.Sleep(GeminiHardware.SlewSettleTime * 1000);

            GeminiHardware.Trace.Exit(4, "WaitForSlewToEnd");
        }

        public void SlewToTarget()
        {
            GeminiHardware.Trace.Enter("IT:SlewToTarget", GeminiHardware.TargetRightAscension, GeminiHardware.TargetDeclination);

            if (GeminiHardware.AtHome || GeminiHardware.AtPark)
                throw new ASCOM.DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);

            if (Slewing) AbortSlew();
            GeminiHardware.Velocity = "S";
            GeminiHardware.SlewEquatorial();
            WaitForSlewToEnd();
            GeminiHardware.Trace.Exit("IT:SlewToTarget", GeminiHardware.TargetRightAscension, GeminiHardware.TargetDeclination);

        }

        public void SlewToTargetAsync()
        {
            GeminiHardware.Trace.Enter("IT:SlewToTargetAsync", GeminiHardware.TargetRightAscension, GeminiHardware.TargetDeclination);

            if (GeminiHardware.AtHome || GeminiHardware.AtPark)
                throw new ASCOM.DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);

            if (Slewing) AbortSlew();
            GeminiHardware.Velocity = "S";
            GeminiHardware.SlewEquatorialAsync();
            WaitForVelocity("SC", GeminiHardware.MAX_TIMEOUT);
            m_AsyncSlewStarted = true;
            GeminiHardware.Trace.Exit("IT:SlewToTargetAsync", GeminiHardware.TargetRightAscension, GeminiHardware.TargetDeclination);
        }

        public bool Slewing
        {
            

            get 
            {                
                if (GeminiHardware.Velocity == "S" || GeminiHardware.Velocity == "C")
                {
                    GeminiHardware.Trace.Enter("IT:Slewing.Get", true);
                    return true;
                }
                else
                {
                    if (m_AsyncSlewStarted) // need to wait out the slewsettletime here...
                    {
                        System.Threading.Thread.Sleep(GeminiHardware.SlewSettleTime * 1000);
                        m_AsyncSlewStarted = false;
                    }
                    GeminiHardware.Trace.Enter("IT:Slewing.Get", false);
                    return false;
                }
            }
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            GeminiHardware.Trace.Enter("IT:SyncToAltAz", Azimuth, Altitude);

            GeminiHardware.SyncHorizonCoordinates(Azimuth, Altitude);

            GeminiHardware.Trace.Exit("IT:SyncToAltAz", Azimuth, Altitude);
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            GeminiHardware.Trace.Enter("IT:SyncToCoordinates", RightAscension, Declination);

            if (GeminiHardware.AtPark)
                    throw new ASCOM.DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);

            GeminiHardware.SyncToEquatorialCoords(RightAscension, Declination);
            GeminiHardware.Trace.Exit("IT:SyncToCoordinates", RightAscension, Declination);
        }

        public void SyncToTarget()
        {
            GeminiHardware.Trace.Enter("IT:SyncToTarget", GeminiHardware.TargetRightAscension, GeminiHardware.TargetDeclination);

            if (GeminiHardware.AtPark)
                throw new ASCOM.DriverException(SharedResources.MSG_INVALID_AT_PARK, (int)SharedResources.INVALID_AT_PARK);

            if (TargetDeclination == SharedResources.INVALID_DOUBLE || TargetRightAscension == SharedResources.INVALID_DOUBLE)
                throw new ASCOM.DriverException(SharedResources.MSG_PROP_NOT_SET, (int)SharedResources.SCODE_PROP_NOT_SET);
            GeminiHardware.SyncEquatorial();
            GeminiHardware.Trace.Exit("IT:SyncToTarget", GeminiHardware.TargetRightAscension, GeminiHardware.TargetDeclination);
        }

        public double TargetDeclination
        {
            get {
                double val = GeminiHardware.TargetDeclination;
                GeminiHardware.Trace.Enter("IT:TargetDeclination.Get", val);

                if (val == SharedResources.INVALID_DOUBLE)
                    throw new ASCOM.ValueNotSetException("TargetDeclination");
                return val;
            }
            set
            {
                GeminiHardware.Trace.Enter("IT:TargetDeclination.Set", value);
                if (value < -90 || value > 90)
                {
                    throw new ASCOM.InvalidValueException("TargetDeclination", value.ToString(), "-90..90");
                }
                GeminiHardware.TargetDeclination = value;
            }
        }

        public double TargetRightAscension
        {
            // TODO Replace this with your implementation
            get { 

                double val = GeminiHardware.TargetRightAscension;
                GeminiHardware.Trace.Enter("IT:TargetRightAscension.Get", val);

                if (val == SharedResources.INVALID_DOUBLE)
                    throw new ValueNotSetException("TargetRightAscension");
                return val;            
            }
            set 
            {
                GeminiHardware.Trace.Enter("IT:TargetRightAscension.Set", value);

                if (value < 0 || value > 24)
                {
                    throw new InvalidValueException("TargetRightAscension", value.ToString(), "0..24");
                }
                GeminiHardware.TargetRightAscension=value; 
            }
        }

        public bool Tracking
        {
            get { bool res = GeminiHardware.Tracking;
                GeminiHardware.Trace.Enter("IT:Tracking.Get", res);
                return res;
            }
            set 
            {
                GeminiHardware.Trace.Enter("IT:Tracking.Set", value);

                if (value && !GeminiHardware.Tracking)
                {
                    GeminiHardware.DoCommandResult(":hW", GeminiHardware.MAX_TIMEOUT, false);
                    WaitForVelocity("TG", GeminiHardware.MAX_TIMEOUT);
                }
                if (!value && GeminiHardware.Tracking)
                {
                    GeminiHardware.DoCommandResult(":hN", GeminiHardware.MAX_TIMEOUT, false);
                    WaitForVelocity("N", GeminiHardware.MAX_TIMEOUT);
                }
                GeminiHardware.Trace.Exit("IT:Tracking.Set", value);
            }
        }

        /// <summary>
        /// wait for one or more possible velocity states of the mount
        /// </summary>
        /// <param name="p">contains one or more letters representing velocities we are waiting for: N, T, G, C, S</param>
        /// <param name="tmout">how long to wait or -1 to wait indefinitely</param>
        /// <returns></returns>
        private bool WaitForVelocity(string p, int tmout)
        {
            GeminiHardware.Trace.Enter(4, "WaitForVelocity", p, tmout);

            int timeout = System.Environment.TickCount + tmout;
            while ((tmout<=0 || System.Environment.TickCount < timeout) && !p.Contains(GeminiHardware.Velocity)) System.Threading.Thread.Sleep(500);

            GeminiHardware.Trace.Exit(4, "WaitForVelocity", p, tmout, GeminiHardware.Velocity);
            if (p.Contains(GeminiHardware.Velocity)) return true;
            return false;
        }

        public DriveRates TrackingRate
        {
            get {
                GeminiHardware.Trace.Enter("IT:TrackingRate.Get");

                string res = GeminiHardware.DoCommandResult("<130:", GeminiHardware.MAX_TIMEOUT, false);
                GeminiHardware.Trace.Exit("IT:TrackingRate.Get", res);
                switch (res) 
                {
                    case "131": return DriveRates.driveSidereal;
                    case "132": return DriveRates.driveKing;
                    case "133": return DriveRates.driveLunar;
                    case "134": return DriveRates.driveSolar;
                    case null: throw new TimeoutException("Get TrackingRate");
                    default: throw new ASCOM.PropertyNotImplementedException("TrackingRate for custom rate", false);
                }
            }
            set {

                GeminiHardware.Trace.Enter("IT:TrackingRate.Set", value);

                string cmd = "";

                switch (value)
                {
                    case DriveRates.driveSidereal : cmd = ">131:"; break;
                    case DriveRates.driveKing : cmd = ">132:"; break;
                    case DriveRates.driveLunar: cmd = ">133:"; break;
                    case DriveRates.driveSolar: cmd = ">134:"; break;
                }
                GeminiHardware.DoCommandResult(cmd, GeminiHardware.MAX_TIMEOUT, false);
                GeminiHardware.Trace.Exit("IT:TrackingRate.Set", value);
            }
        }

        public ITrackingRates TrackingRates
        {
            get {
                GeminiHardware.Trace.Enter("IT:TrackingRates.Get");
                return m_TrackingRates; }
        }

        public DateTime UTCDate
        {
            
            get { 
                DateTime res = GeminiHardware.UTCDate;
                GeminiHardware.Trace.Enter("IT:UTCDate.Get", res);
                return res;
            }
            set {
                GeminiHardware.Trace.Enter("IT:UTCDate.Set", value);
                GeminiHardware.UTCDate = value;
                GeminiHardware.Trace.Exit("IT:UTCDate.Set", value);
            }
        }

        public void Unpark()
        {
            GeminiHardware.Trace.Enter("IT:Unpark");

            GeminiHardware.DoCommandResult(":hW", GeminiHardware.MAX_TIMEOUT, false);
            WaitForVelocity("T", GeminiHardware.MAX_TIMEOUT);
            GeminiHardware.Trace.Exit("IT:Unpark");
        }

        #endregion

        #region IConform Members

        public IConformCommandStrings ConformCommands
        {
            get 
            { 
                return new ConformCommandStrings("Gc", "(24)", "Q", "Sz045:00:00", true);
            }
        }

        public IConformCommandStrings ConformCommandsRaw
        {
            get 
            {
                return new ConformCommandStrings(":Gc#", "(24)#", ":Q#", ":Sz045:00:00#", true);
            }
        }

        public IConformErrorNumbers ConformErrors
        {
            get 
            { 
                return new ConformErrorNumbers(new int[] {ErrorCodes.NotImplemented}, 
                                               new int[] {ErrorCodes.InvalidValue}, 
                                               new int[] {ErrorCodes.ValueNotSet});
            }
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

            if (Axis == TelescopeAxes.axisTertiary)
            {
                m_Rates = new Rate[0];
                return;
            }

            // goto slew, centering, and guiding speeds from the mount
            string[] get_rates = { "<140:", "<170:", "<150:"  };
            string[] result = null;
          
            GeminiHardware.DoCommandResult(get_rates, 3000, false, out result);

            // if didn't get a result or one of the results timed out, throw an error:
            if (result == null) throw new TimeoutException("AxisRates");
            foreach (string s in result)
                if (s == null) throw new TimeoutException("AxisRates");

            switch (Axis)
            {
                case TelescopeAxes.axisPrimary:
                case TelescopeAxes.axisSecondary:
                    m_Rates = new Rate[result.Length];
                    for(int idx=0; idx<result.Length; ++idx)
                    {
                        double rate = 0;
                        if (!double.TryParse(result[idx], out rate)) throw new TimeoutException("AxisRates");
                        rate = rate * SharedResources.EARTH_ANG_ROT_DEG_MIN / 60.0;  // convert to rate in deg/sec
                        m_Rates[idx] = new Rate(rate,rate);
                    }
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
    public class TrackingRates : ITrackingRates, IEnumerable, IEnumerator
    {
        public DriveRates [] m_TrackingRates;
        private static int _pos = -1;

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
            m_TrackingRates =  new DriveRates[] { DriveRates.driveSidereal, DriveRates.driveKing, DriveRates.driveLunar, DriveRates.driveSolar };
        }

        #region ITrackingRates Members

        public int Count
        {
            get { return m_TrackingRates.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            return this as IEnumerator;
        }

        
        public DriveRates this[int Index]
        {
            get { return m_TrackingRates[Index - 1]; }	// 1-based
        }
        #endregion

        #region IEnumerator implementation

        public bool MoveNext()
        {
            if (++_pos >= m_TrackingRates.Length) return false;
            return true;
        }

        public void Reset()
        {
            _pos = -1;
        }

        public object Current
        {
            get
            {
                if (_pos < 0 || _pos >= m_TrackingRates.Length) throw new System.InvalidOperationException();
                return m_TrackingRates[_pos];
            }
        }

        #endregion
    }
}
