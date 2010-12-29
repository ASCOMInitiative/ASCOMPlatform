//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Simulated Telescope Hardware
//
// Description:	This implements a simulated Telescope Hardware
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
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Timers;
using System.Runtime.InteropServices;
using System.Drawing;
using ASCOM.DeviceInterface;
using System.Globalization;

namespace ASCOM.Simulator
{
    public static class TelescopeHardware
    {
        // TODO replace with a .NET function, such as environment.TickCount or
        // the DateTime functions
        //[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        //public static extern long GetTickCount();

        public static int GetTickCount()
        {
            return Environment.TickCount;
        }

        //private static Timer m_Timer = new Timer(); // Simulated Hardware by running a Timer

        // change to using a Windows timer to avoid threading problems
        private static System.Windows.Forms.Timer s_wTimer;

        private static Utilities.Profile s_Profile;
        private static bool m_OnTop;
        private static ASCOM.Utilities.TraceLogger TL;

        //Capabilities
        private static bool m_CanFindHome;
        private static bool m_CanPark;
        private static bool m_VersionOne;
        private static int m_NumberMoveAxis;
        private static bool m_CanPulseGuide;
        private static bool m_CanDualAxisPulseGuide;
        private static bool m_CanSetEquatorialRates;
        private static bool m_CanSetGuideRates;
        private static bool m_CanSetPark;
        private static bool m_CanSetPierSide;
        private static bool m_CanSetTracking;
        private static bool m_CanSlew;
        private static bool m_CanSlewAltAz;
        private static bool m_CanAlignmentMode;
        private static bool m_CanOptics;
        private static bool m_CanSlewAltAzAsync;
        private static bool m_CanSlewAsync;
        private static bool m_CanSync;
        private static bool m_CanSyncAltAz;
        private static bool m_CanUnpark;
        private static bool m_CanAltAz;
        private static bool m_CanDateTime;
        private static bool m_CanDoesRefraction;
        private static bool m_CanEquatorial;
        private static bool m_CanLatLongElev;
        private static bool m_CanSiderealTime;
        private static bool m_CanPierSide;
        private static bool m_CanTrackingRates;

        //Telescope Implementation
        private static ASCOM.DeviceInterface.AlignmentModes m_AlignmentMode;
        private static double m_ApertureArea;
        private static double m_ApertureDiameter;
        private static double m_FocalLength;
        private static bool m_AutoTrack;
        private static bool m_DisconnectOnPark;
        private static bool m_Refraction;
        private static int m_EquatorialSystem;
        private static bool m_NoCoordinatesAtPark;
        private static double m_Latitude;
        private static double m_Longitude;
        private static double m_Elevation;
        private static int m_MaximumSlewRate;

        private static double m_Altitude;
        private static double m_Azimuth;
        private static double m_ParkAltitude;
        private static double m_ParkAzimuth;

        private static double m_RightAscension;
        private static double m_Declination;
        private static double m_SiderealTime;

        private static double m_TargetRightAscension = SharedResources.INVALID_COORDINATE;
        private static double m_TargetDeclination = SharedResources.INVALID_COORDINATE;

        private static bool m_Tracking;
        //private static bool m_AtHome;
        private static bool m_AtPark;

        public static double m_DeclinationRate;
        public static double m_RightAscensionRate;
        public static double m_GuideRateDeclination;
        public static double m_GuideRateRightAscension;


        private static int m_TrackingRate;

        private static SlewType m_SlewState = SlewType.SlewNone;
        private static SlewDirection m_SlewDirection;
        private static SlewSpeed m_SlewSpeed;

        private static double m_SlewSpeedFast;
        private static double m_SlewSpeedMedium;
        private static double m_SlewSpeedSlow;

        private static double m_SlewSettleTime;

        private static bool m_SouthernHemisphere = false;

        private static int m_DateDelta;

        //public static long m_PulseGuideTixRa = 0;
        //public static long m_PulseGuideTixDec = 0;

        public static bool isPulseGuidingRa;
        public static bool isPulseGuidingDec;
        public static DateTime pulseGuideRaEndTime;
        public static DateTime pulseGuideDecEndTime;


        public static double m_DeltaAz = 0;
        public static double m_DeltaAlt = 0;
        public static double m_DeltaRa = 0;
        public static double m_DeltaDec = 0;

        private static DateTime settleTime;

        public static PierSide m_SideOfPier;

        private static bool m_Connected = false; //Keep track of the connection status of the hardware


        static TelescopeHardware()
        {
            s_Profile = new Utilities.Profile();
            //m_Timer.Elapsed += new ElapsedEventHandler(TimerEvent);
            //m_Timer.Interval = SharedResources.TIMER_INTERVAL * 1000;

            s_wTimer = new System.Windows.Forms.Timer();
            s_wTimer.Interval = (int)(SharedResources.TIMER_INTERVAL * 1000);
            s_wTimer.Tick += new EventHandler(m_wTimer_Tick);

            TL = new ASCOM.Utilities.TraceLogger("", "SimTelescopeHardware");
            TL.Enabled = true;

            // check if the profile settings are correct 
            if (s_Profile.GetValue(SharedResources.PROGRAM_ID, "RegVer", "") != SharedResources.REGISTRATION_VERSION)
            {
                // load the default settings
                //Main Driver Settings
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "RegVer", SharedResources.REGISTRATION_VERSION);
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlwaysOnTop", "false");

                //Telescope Implementions
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "1");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ApertureArea", SharedResources.INSTRUMENT_APERTURE_AREA.ToString(CultureInfo.InvariantCulture));
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Aperture", SharedResources.INSTRUMENT_APERTURE.ToString(CultureInfo.InvariantCulture));
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "FocalLength", SharedResources.INSTRUMENT_FOCAL_LENGTH.ToString(CultureInfo.InvariantCulture));
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AutoTrack", "false");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "DiscPark", "false");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "NoCoordAtPark", "false");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Refraction", "true");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "EquatorialSystem", "1");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "MaxSlewRate", "20");

                //' Geography
                //'
                //' Based on the UTC offset, create a longitude somewhere in
                //' the time zone, a latitude between 0 and 60 and a site
                //' elevation between 0 and 1000 metres. This gives the
                //' client some geo position without having to open the
                //' Setup dialog.
                Random r = new Random();
                TimeZone localZone = TimeZone.CurrentTimeZone;
                double latitude = 51.07861;// (r.NextDouble() * 60); lock for testing
                double longitude = (((-(double)(localZone.GetUtcOffset(DateTime.Now).Seconds) / 3600) + r.NextDouble() - 0.5) * 15);
                if (localZone.GetUtcOffset(DateTime.Now).Seconds == 0) longitude = -0.29444; //lock for testing
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Elevation", Math.Round((r.NextDouble() * 1000), 0).ToString(CultureInfo.InvariantCulture));
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Longitude", longitude.ToString(CultureInfo.InvariantCulture));
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Latitude", latitude.ToString(CultureInfo.InvariantCulture));

                //Start the scope in parked position
                if (latitude >= 0)
                {
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAzimuth", "180");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAzimuth", "180");
                }
                else
                {
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAzimuth", "90");
                    s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAzimuth", "90");
                }
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAltitude", (90 - Math.Abs(latitude)).ToString(CultureInfo.InvariantCulture));
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAltitude", (90 - Math.Abs(latitude)).ToString(CultureInfo.InvariantCulture));

                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "DateDelta", "0");

                //Capabilities Settings
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "V1", "false", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanFindHome", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPark", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "NumMoveAxis", "2", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPulseGuide", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetEquRates", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetGuideRates", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetPark", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetPierSide", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetTracking", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlew", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAltAz", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanAlignMode", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanOptics", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAltAzAsync", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAsync", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSync", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSyncAltAz", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanUnpark", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanAltAz", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDateTime", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDoesRefraction", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanEquatorial", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanLatLongElev", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSiderealTime", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPierSide", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanTrackingRates", "true", "Capabilities");
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDualAxisPulseGuide", "true", "Capabilities");
            }

            //Load up the values from saved
            m_OnTop = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "AlwaysOnTop"));

            switch (int.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "AlignMode"), CultureInfo.InvariantCulture))
            {
                case 0:
                    m_AlignmentMode = ASCOM.DeviceInterface.AlignmentModes.algAltAz;
                    break;
                case 1:
                    m_AlignmentMode = ASCOM.DeviceInterface.AlignmentModes.algGermanPolar;
                    break;
                case 2:
                    m_AlignmentMode = ASCOM.DeviceInterface.AlignmentModes.algPolar;
                    break;
                default:
                    m_AlignmentMode = ASCOM.DeviceInterface.AlignmentModes.algGermanPolar;
                    break;
            }

            m_ApertureArea = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "ApertureArea"), CultureInfo.InvariantCulture);
            m_ApertureDiameter = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "Aperture"), CultureInfo.InvariantCulture);
            m_FocalLength = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "FocalLength"), CultureInfo.InvariantCulture);
            m_AutoTrack = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "AutoTrack"));
            m_DisconnectOnPark = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "DiscPark"));
            m_Refraction = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "Refraction"));
            m_EquatorialSystem = int.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "EquatorialSystem"), CultureInfo.InvariantCulture);
            m_NoCoordinatesAtPark = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "NoCoordAtPark"));
            m_Elevation = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "Elevation"), CultureInfo.InvariantCulture);
            m_Latitude = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "Latitude"), CultureInfo.InvariantCulture);
            m_Longitude = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "Longitude"), CultureInfo.InvariantCulture);
            m_MaximumSlewRate = int.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "MaxSlewRate"), CultureInfo.InvariantCulture);

            m_Altitude = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "StartAltitude"), CultureInfo.InvariantCulture);
            m_Azimuth = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "StartAzimuth"), CultureInfo.InvariantCulture);
            m_ParkAltitude = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "ParkAltitude"), CultureInfo.InvariantCulture);
            m_ParkAzimuth = double.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "ParkAzimuth"), CultureInfo.InvariantCulture);

            //TODO allow for version 1, 2 or 3
            m_VersionOne = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "V1", "Capabilities"));
            m_CanFindHome = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanFindHome", "Capabilities"));
            m_CanPark = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanPark", "Capabilities"));
            m_NumberMoveAxis = int.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "NumMoveAxis", "Capabilities"), CultureInfo.InvariantCulture);
            m_CanPulseGuide = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanPulseGuide", "Capabilities"));
            m_CanSetEquatorialRates = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetEquRates", "Capabilities"));
            m_CanSetGuideRates = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetGuideRates", "Capabilities"));
            m_CanSetPark = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetPark", "Capabilities"));
            m_CanSetPierSide = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetPierSide", "Capabilities"));
            m_CanSetTracking = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetTracking", "Capabilities"));
            m_CanSlew = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSlew", "Capabilities"));
            m_CanSlewAltAz = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSlewAltAz", "Capabilities"));
            m_CanAlignmentMode = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanAlignMode", "Capabilities"));
            m_CanOptics = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanOptics", "Capabilities"));
            m_CanSlewAltAzAsync = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSlewAltAzAsync", "Capabilities"));
            m_CanSlewAsync = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSlewAsync", "Capabilities"));
            m_CanSync = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSync", "Capabilities"));
            m_CanSyncAltAz = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSyncAltAz", "Capabilities"));
            m_CanUnpark = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanUnpark", "Capabilities"));
            m_CanAltAz = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanAltAz", "Capabilities"));
            m_CanDateTime = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanDateTime", "Capabilities"));
            m_CanDoesRefraction = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanDoesRefraction", "Capabilities"));
            m_CanEquatorial = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanEquatorial", "Capabilities"));
            m_CanLatLongElev = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanLatLongElev", "Capabilities"));
            m_CanSiderealTime = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSiderealTime", "Capabilities"));
            m_CanPierSide = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanPierSide", "Capabilities"));
            m_CanTrackingRates = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanTrackingRates", "Capabilities"));
            m_CanDualAxisPulseGuide = bool.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "CanDualAxisPulseGuide", "Capabilities"));

            m_DateDelta = int.Parse(s_Profile.GetValue(SharedResources.PROGRAM_ID, "DateDelta"), CultureInfo.InvariantCulture);

            if (m_Latitude < 0) { m_SouthernHemisphere = true; }

            //Set the form setting for the Always On Top Value
            TelescopeSimulator.m_MainForm.TopMost = m_OnTop;

            m_SlewSpeedFast = m_MaximumSlewRate * SharedResources.TIMER_INTERVAL;
            m_SlewSpeedMedium = m_SlewSpeedFast * 0.1;
            m_SlewSpeedSlow = m_SlewSpeedFast * 0.02;

            m_GuideRateRightAscension = 15 * (1 / 3600) / SharedResources.SIDRATE;
            m_GuideRateDeclination = m_GuideRateRightAscension;
            m_DeclinationRate = 0;
            m_RightAscensionRate = 0;

            //King=0
            //Lunar=1
            //Sidereal=2
            //Solar=3

            m_TrackingRate = 2;
            m_SlewSettleTime = 0;
            ChangePark(m_AtPark);
        }

        private static void m_wTimer_Tick(object sender, EventArgs e)
        {
            HardwareEvent();
        }

        public static void Start()
        {
            m_Connected = false;
            m_Tracking = false;
            m_AtPark = false;

            if (m_Tracking)
            {
                CalculateAltAz();
            }
            else
            {
                CalculateRaDec();
            }
            m_SiderealTime = AstronomyFunctions.LocalSiderealTime(m_Longitude);

            s_wTimer.Start();
        }

        //Update the Telescope Based on Timed Events
        private static void TimerEvent(object source, ElapsedEventArgs e)
        {
            HardwareEvent();
        }

        private static void HardwareEvent()
        {
            double step;
            double z;
            double y;
            double raRate;
            double decRate;

            if (m_SlewState == SlewType.SlewNone) //Not slewing the scope 
            {
                if (m_Tracking)
                {
                    CalculateAltAz();
                }
                else
                {
                    CalculateRaDec();
                }
            }
            else //Process the movement
            {
                // CR divide by 15 to get the movement rate in hours per second
                // z is the scaling applied to the Ra movement as the pole is approached
                z = Math.Cos(m_Declination * SharedResources.DEG_RAD);// / 15;
                if (z < 0.001) { z = 0.001; }

                if (m_SlewState == SlewType.SlewHandpad) //We are doing a slew with the handpad buttons
                {
                    // adjust step according to the slew speed, it's in deg/sec.
                    if (m_SlewSpeed == SlewSpeed.SlewFast)
                    {
                        step = m_SlewSpeedFast;
                    }
                    else if (m_SlewSpeed == SlewSpeed.SlewMedium)
                    {
                        step = m_SlewSpeedMedium;
                    }
                    else
                    {
                        step = m_SlewSpeedSlow;
                    }
                    switch (m_SlewDirection)
                    {
                        // altaz scope HC buttons adjust Altitude and azimuth
                        case SlewDirection.SlewUp:
                            m_Altitude += step;
                            m_Declination = AstronomyFunctions.CalculateDec(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD);
                            m_RightAscension = AstronomyFunctions.CalculateRA(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewDown:
                            m_Altitude -= step;
                            m_Declination = AstronomyFunctions.CalculateDec(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD);
                            m_RightAscension = AstronomyFunctions.CalculateRA(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewRight:
                            m_Azimuth += step;
                            m_Declination = AstronomyFunctions.CalculateDec(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD);
                            m_RightAscension = AstronomyFunctions.CalculateRA(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewLeft:
                            m_Azimuth -= step;
                            m_Declination = AstronomyFunctions.CalculateDec(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD);
                            m_RightAscension = AstronomyFunctions.CalculateRA(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
                        // equatorial mount scopes adjust the Ra and Dec
                        case SlewDirection.SlewNorth:
                            m_Declination += step;
                            m_Declination = AstronomyFunctions.RangeDec(m_Declination);
                            m_Altitude = AstronomyFunctions.CalculateAltitude(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            m_Azimuth = AstronomyFunctions.CalculateAzimuth(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewSouth:
                            m_Declination -= step;
                            m_Declination = AstronomyFunctions.RangeDec(m_Declination);
                            m_Altitude = AstronomyFunctions.CalculateAltitude(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            m_Azimuth = AstronomyFunctions.CalculateAzimuth(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewEast:
                            m_RightAscension += step * (z / 15);    // Ra is in hours
                            m_RightAscension = AstronomyFunctions.RangeRA(m_RightAscension);
                            m_Altitude = AstronomyFunctions.CalculateAltitude(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            m_Azimuth = AstronomyFunctions.CalculateAzimuth(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewWest:
                            m_RightAscension -= step * (z / 15);
                            m_RightAscension = AstronomyFunctions.RangeRA(m_RightAscension);
                            m_Altitude = AstronomyFunctions.CalculateAltitude(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            m_Azimuth = AstronomyFunctions.CalculateAzimuth(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
                    }
                }
                else if (m_SlewState == SlewType.SlewRaDec)
                {
                    settleTime = DateTime.Now + TimeSpan.FromSeconds(m_SlewSettleTime);
                    TL.LogMessage("Fast, Med, Slow, Target RA, Dec", m_SlewSpeedFast + " " + m_SlewSpeedMedium + " " + m_SlewSpeedSlow + " " + m_TargetRightAscension + " " + m_TargetDeclination);

                    // determine the RA Step in degrees, adjusted by the dec factor z
                    y = Math.Abs(m_DeltaRa * 360.0 / 24.0); // In degrees
                    step = GetStepSize(y);
                    //if (m_SlewSpeedFast / SharedResources.TIMER_INTERVAL >= 50)
                    //{
                    //    step = y * z;
                    //    TL.LogStart("RA Speed z y step", "Maximum");
                    //}
                    //else if (y > 2 * m_SlewSpeedFast)
                    //{
                    //    step = m_SlewSpeedFast * z;
                    //    TL.LogStart("RA Speed z y step", "Fast");
                    //}
                    //else if (y > 2 * m_SlewSpeedMedium)
                    //{
                    //    step = m_SlewSpeedMedium * z;
                    //    TL.LogStart("RA Speed z y step", "Medium");
                    //}
                    //else if (y > 2 * m_SlewSpeedSlow)
                    //{
                    //    step = m_SlewSpeedSlow * z;
                    //    TL.LogStart("RA Speed z y step", "Slow");
                    //}
                    //else
                    //{
                    //    step = y * z;
                    //    TL.LogStart("RA Speed z y step", "Minimum");
                    //}

                    step *= Math.Sign(m_DeltaRa);
                    TL.LogFinish(" " + z + " " + y + " " + step);

                    // step is in degrees but the Ra values are in hours
                    m_RightAscension += step / 15.0;
                    m_DeltaRa -= step / 15.0;

                    //Dec Step
                    y = Math.Abs(m_DeltaDec);
                    step = GetStepSize(y);
                    //if (m_SlewSpeedFast / SharedResources.TIMER_INTERVAL >= 50)
                    //{
                    //    step = y;
                    //    TL.LogStart("Dec Speed z y step", "Maximum");
                    //}
                    //else if (y > 2 * m_SlewSpeedFast)
                    //{
                    //    step = m_SlewSpeedFast;
                    //    TL.LogStart("Dec Speed z y step", "Fast");
                    //}
                    //else if (y > 2 * m_SlewSpeedMedium)
                    //{
                    //    step = m_SlewSpeedMedium;
                    //    TL.LogStart("Dec Speed z y step", "Medium");
                    //}
                    //else if (y > 2 * m_SlewSpeedSlow)
                    //{
                    //    step = m_SlewSpeedSlow;
                    //    TL.LogStart("Dec Speed z y step", "Slow");
                    //}
                    //else
                    //{
                    //    step = y;
                    //    TL.LogStart("Dec Speed z y step", "Minimum");
                    //}

                    step *= Math.Sign(m_DeltaDec);
                    TL.LogFinish(" " + z + " " + y + " " + step);

                    m_Declination += step;
                    m_DeltaDec -= step;

                    m_Declination = AstronomyFunctions.RangeDec(m_Declination);
                    m_RightAscension = AstronomyFunctions.RangeRA(m_RightAscension);
                    TL.LogMessage("RA, Dec", m_RightAscension + " " + m_DeltaRa + " " + m_Declination + " " + m_DeltaDec);
                    CalculateAltAz();

                    if (Math.Abs(m_DeltaRa) < 0.0003 && Math.Abs(m_DeltaDec) < 0.0003)
                    {
                        TL.LogMessage("Settle", "Moved from slew to settle");
                        m_SlewState = SlewType.SlewSettle;
                    }
                    TL.BlankLine();

                }
                else if (m_SlewState == SlewType.SlewAltAz || m_SlewState == SlewType.SlewHome || m_SlewState == SlewType.SlewPark)
                {
                    settleTime = DateTime.Now + TimeSpan.FromSeconds(m_SlewSettleTime);

                    //Altitude Step
                    y = Math.Abs(m_DeltaAlt);
                    step = GetStepSize(y);
                    //if (m_SlewSpeedFast / SharedResources.TIMER_INTERVAL >= 50) { step = y; TL.LogStart("Alt Speed z y step", "Maximum"); }
                    //else if (y > 2 * m_SlewSpeedFast) { step = m_SlewSpeedFast; TL.LogStart("Alt Speed z y step", "Fast"); }
                    //else if (y > 2 * m_SlewSpeedMedium) { step = m_SlewSpeedMedium; TL.LogStart("Alt Speed z y step", "Medium"); }
                    //else if (y > 2 * m_SlewSpeedSlow) { step = m_SlewSpeedSlow; TL.LogStart("Alt Speed z y step", "Slow"); }
                    //else { step = y; TL.LogStart("Alt Speed z y step", "Minimum"); }

                    step *= Math.Sign(m_DeltaAlt);

                    m_Altitude += step;
                    m_DeltaAlt -= step;
                    TL.LogFinish(" " + y + " " + step + " " + m_Altitude + " " + m_DeltaAlt);

                    //Azimuth Step
                    y = Math.Abs(m_DeltaAz);
                    step = GetStepSize(y);
                    //if (m_SlewSpeedFast / SharedResources.TIMER_INTERVAL >= 50) { step = y; TL.LogStart("Az Speed", "Maximum"); }
                    //else if (y > 2 * m_SlewSpeedFast) { step = m_SlewSpeedFast; TL.LogStart("Az Speed", "Fast"); }
                    //else if (y > 2 * m_SlewSpeedMedium) { step = m_SlewSpeedMedium; TL.LogStart("Az Speed", "Medium"); }
                    //else if (y > 2 * m_SlewSpeedSlow) { step = m_SlewSpeedSlow; TL.LogStart("Az Speed", "Slow"); }
                    //else { step = y; TL.LogStart("Az Speed", "Minimum"); }

                    step *= Math.Sign(m_DeltaAz);

                    m_Azimuth += step;
                    m_DeltaAz -= step;
                    TL.LogFinish(" " + y + " " + step + " " + m_Azimuth + " " + m_DeltaAz);

                    m_Azimuth = AstronomyFunctions.RangeAzimuth(m_Azimuth);
                    m_Altitude = AstronomyFunctions.RangeAlt(m_Altitude);
                    CalculateRaDec();

                    if (Math.Abs(m_DeltaAz) < 0.0000001 && Math.Abs(m_DeltaAlt) < 0.0000001)
                    {
                        if (m_SlewState == SlewType.SlewPark)
                        {
                            m_SlewState = SlewType.SlewNone;
                            ChangePark(true);
                        }
                        else if (m_SlewState == SlewType.SlewHome)
                        {
                            m_SlewState = SlewType.SlewNone;
                            //ChangeHome(true);
                        }
                        else m_SlewState = SlewType.SlewSettle;
                    }

                }
                else if (m_SlewState == SlewType.SlewMoveAxis)
                {
                    if (m_AlignmentMode == ASCOM.DeviceInterface.AlignmentModes.algAltAz)
                    {
                        m_Azimuth = m_Azimuth + m_DeltaAz * SharedResources.TIMER_INTERVAL;
                        m_Altitude = m_Altitude + m_DeltaAlt * SharedResources.TIMER_INTERVAL;
                        m_Azimuth = AstronomyFunctions.RangeAzimuth(m_Azimuth);
                        m_Altitude = AstronomyFunctions.RangeAlt(m_Altitude);
                        CalculateRaDec();
                    }
                    else
                    {
                        m_RightAscension = m_RightAscension + (m_DeltaAz * SharedResources.TIMER_INTERVAL) / 15;
                        m_Declination = m_Declination + m_DeltaAlt * SharedResources.TIMER_INTERVAL;

                        m_Declination = AstronomyFunctions.RangeDec(m_Declination);
                        m_RightAscension = AstronomyFunctions.RangeRA(m_RightAscension);

                        CalculateAltAz();
                    }
                }
            }
            if (isPulseGuidingRa || isPulseGuidingDec)
            {
                // do pulse guiding
                ChangePark(false);
                if (pulseGuideRaEndTime >= DateTime.Now)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Slew, "(PulseGuide in RA complete)");
                    isPulseGuidingRa = false;
                }

                if (pulseGuideDecEndTime > DateTime.Now)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Slew, "(PulseGuide in Dec complete)");
                    isPulseGuidingDec = false;
                }

                if (m_Tracking) raRate = m_RightAscensionRate;
                else raRate = 15;
                raRate = (raRate / SharedResources.SIDRATE) / 3600;
                if (isPulseGuidingRa) raRate = raRate + (m_GuideRateRightAscension / 15);

                decRate = m_DeclinationRate / 3600;
                if (isPulseGuidingDec) decRate = decRate + m_GuideRateDeclination;

                m_RightAscension += raRate * SharedResources.TIMER_INTERVAL;
                m_Declination += decRate * SharedResources.TIMER_INTERVAL;

                m_Declination = AstronomyFunctions.RangeDec(m_Declination);
                m_RightAscension = AstronomyFunctions.RangeRA(m_RightAscension);

                CalculateAltAz();
            }

            if (m_SlewState == SlewType.SlewSettle)
            {
                if (DateTime.Now >= settleTime)
                {
                    SharedResources.TrafficLine(SharedResources.MessageType.Slew, "(Slew Complete)");
                    m_SlewState = SlewType.SlewNone;
                }
            }

            //Calculate Current SideOfPier
            m_SiderealTime = AstronomyFunctions.LocalSiderealTime(m_Longitude);
            m_SideOfPier = SideOfPierRaDec(m_RightAscension, m_Declination);

            TelescopeSimulator.m_MainForm.SiderealTime(m_SiderealTime);
            TelescopeSimulator.m_MainForm.Altitude(m_Altitude);
            TelescopeSimulator.m_MainForm.Azimuth(m_Azimuth);
            TelescopeSimulator.m_MainForm.RightAscension(m_RightAscension);
            TelescopeSimulator.m_MainForm.Declination(m_Declination);
            TelescopeSimulator.m_MainForm.Tracking();
            TelescopeSimulator.m_MainForm.LedPier(m_SideOfPier);

            if (m_AtPark) TelescopeSimulator.m_MainForm.lblPARK.ForeColor = Color.Red;
            else TelescopeSimulator.m_MainForm.lblPARK.ForeColor = Color.SaddleBrown;
            if (AtHome) TelescopeSimulator.m_MainForm.lblHOME.ForeColor = Color.Red;
            else TelescopeSimulator.m_MainForm.lblHOME.ForeColor = Color.SaddleBrown;
            if (m_SlewState == SlewType.SlewNone) TelescopeSimulator.m_MainForm.labelSlew.ForeColor = Color.SaddleBrown;
            else TelescopeSimulator.m_MainForm.labelSlew.ForeColor = Color.Red;
        }

        /// <summary>
        /// Returns the step size, adjusted for the current slew speed
        /// </summary>
        /// <param name="fullStepSize"></param>
        /// <returns></returns>
        private static double GetStepSize(double fullStepSize)
        {
            double step = 0;
            if (m_SlewSpeedFast / SharedResources.TIMER_INTERVAL >= 50)
            {
                step = fullStepSize;
                TL.LogStart("Speed z y step", "Maximum");
            }
            else if (fullStepSize > 2 * m_SlewSpeedFast)
            {
                step = m_SlewSpeedFast;
                TL.LogStart("Speed z y step", "Fast");
            }
            else if (fullStepSize > 2 * m_SlewSpeedMedium)
            {
                step = m_SlewSpeedMedium;
                TL.LogStart("Speed z y step", "Medium");
            }
            else if (fullStepSize > 2 * m_SlewSpeedSlow)
            {
                step = m_SlewSpeedSlow;
                TL.LogStart("Speed z y step", "Slow");
            }
            else
            {
                step = fullStepSize;
                TL.LogStart("Speed z y step", "Minimum");
            }
            return step;
        }

        #region Properties For Settings

        //I used some of these as dual purpose if the driver uses the same exact property
        public static ASCOM.DeviceInterface.AlignmentModes AlignmentMode
        {
            get { return m_AlignmentMode; }
            set
            {
                m_AlignmentMode = value;
                switch (value)
                {
                    case ASCOM.DeviceInterface.AlignmentModes.algAltAz:
                        s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "0");
                        break;
                    case ASCOM.DeviceInterface.AlignmentModes.algGermanPolar:
                        s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "1");
                        break;
                    case ASCOM.DeviceInterface.AlignmentModes.algPolar:
                        s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "2");
                        break;
                }
            }
        }

        public static bool OnTop
        {
            get { return m_OnTop; }
            set
            {
                m_OnTop = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "OnTop", value.ToString(), "");
            }
        }
        public static bool AutoTrack
        {
            get { return m_AutoTrack; }
            set
            {
                m_AutoTrack = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "AutoTrack", value.ToString(), "");
            }
        }

        public static bool NoCoordinatesAtPark
        {
            get { return m_NoCoordinatesAtPark; }
            set
            {
                m_NoCoordinatesAtPark = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "NoCoordAtPark", value.ToString());
            }
        }

        public static bool VersionOneOnly
        {
            get { return m_VersionOne; }
            set
            {
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "V1", value.ToString(), "Capabilities");
                m_VersionOne = value;
            }
        }

        public static bool DisconnectOnPark
        {
            get { return m_DisconnectOnPark; }
            set
            {
                m_DisconnectOnPark = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "DiscPark", value.ToString(), "");
            }
        }

        public static bool Refraction
        {
            get { return m_Refraction; }
            set
            {
                m_Refraction = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Refraction", value.ToString(), "");
            }
        }

        public static int EquatorialSystem
        {
            get { return m_EquatorialSystem; }
            set
            {
                m_EquatorialSystem = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "EquatorialSystem", value.ToString(CultureInfo.InvariantCulture), "");
            }
        }

        public static double Elevation
        {
            get { return m_Elevation; }
            set
            {
                m_Elevation = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Elevation", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double Latitude
        {
            get { return m_Latitude; }
            set
            {
                m_Latitude = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Latitude", value.ToString(CultureInfo.InvariantCulture));
                if (m_Latitude < 0) { m_SouthernHemisphere = true; }
            }
        }

        public static double Longitude
        {
            get { return m_Longitude; }
            set
            {
                m_Longitude = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Longitude", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static int MaximumSlewRate
        {
            get { return m_MaximumSlewRate; }
            set
            {
                m_MaximumSlewRate = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "MaxSlewRate", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static bool CanFindHome
        {
            get { return m_CanFindHome; }
            set
            {
                m_CanFindHome = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanFindHome", value.ToString(), "Capabilities");
            }
        }

        public static bool CanOptics
        {
            get { return m_CanOptics; }
            set
            {
                m_CanOptics = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanOptics", value.ToString(), "Capabilities");
            }
        }

        public static bool CanPark
        {
            get { return m_CanPark; }
            set
            {
                m_CanPark = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPark", value.ToString(), "Capabilities");
            }
        }

        public static int NumberMoveAxis
        {
            get { return m_NumberMoveAxis; }
            set
            {
                m_NumberMoveAxis = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "NumMoveAxis", value.ToString(CultureInfo.InvariantCulture), "Capabilities");
            }
        }

        public static bool CanPulseGuide
        {
            get { return m_CanPulseGuide; }
            set
            {
                m_CanPulseGuide = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPulseGuide", value.ToString(), "Capabilities");
            }
        }

        public static bool CanDualAxisPulseGuide
        {
            get { return m_CanDualAxisPulseGuide; }
            set
            {
                m_CanDualAxisPulseGuide = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDualAxisPulseGuide", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetEquatorialRates
        {
            get { return m_CanSetEquatorialRates; }
            set
            {
                m_CanSetEquatorialRates = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetEquRates", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetGuideRates
        {
            get { return m_CanSetGuideRates; }
            set
            {
                m_CanSetGuideRates = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetGuideRates", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetPark
        {
            get { return m_CanSetPark; }
            set
            {
                m_CanSetPark = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetPark", value.ToString(), "Capabilities");
            }
        }

        public static bool CanPierSide
        {
            get { return m_CanPierSide; }
            set
            {
                m_CanPierSide = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPierSide", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetPierSide
        {
            get { return m_CanSetPierSide; }
            set
            {
                m_CanSetPierSide = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetPierSide", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetTracking
        {
            get { return m_CanSetTracking; }
            set
            {
                m_CanSetTracking = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetTracking", value.ToString(), "Capabilities");
            }
        }

        public static bool CanTrackingRates
        {
            get { return m_CanTrackingRates; }
            set
            {
                m_CanTrackingRates = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanTrackingRates", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSlew
        {
            get { return m_CanSlew; }
            set
            {
                m_CanSlew = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlew", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSync
        {
            get { return m_CanSync; }
            set
            {
                m_CanSync = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSync", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSlewAsync
        {
            get { return m_CanSlewAsync; }
            set
            {
                m_CanSlewAsync = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAsync", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSlewAltAz
        {
            get { return m_CanSlewAltAz; }
            set
            {
                m_CanSlewAltAz = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAltAz", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSyncAltAz
        {
            get { return m_CanSyncAltAz; }
            set
            {
                m_CanSyncAltAz = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSyncAltAz", value.ToString(), "Capabilities");
            }
        }

        public static bool CanAltAz
        {
            get { return m_CanAltAz; }
            set
            {
                m_CanAltAz = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanAltAz", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSlewAltAzAsync
        {
            get { return m_CanSlewAltAzAsync; }
            set
            {
                m_CanSlewAltAzAsync = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAltAzAsync", value.ToString(), "Capabilities");
            }
        }

        public static bool CanAlignmentMode
        {
            get { return m_CanAlignmentMode; }
            set
            {
                m_CanAlignmentMode = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanAlignMode", value.ToString(), "Capabilities");
            }
        }

        public static bool CanUnpark
        {
            get { return m_CanUnpark; }
            set
            {
                m_CanUnpark = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanUnpark", value.ToString(), "Capabilities");
            }
        }

        public static bool CanDateTime
        {
            get { return m_CanDateTime; }
            set
            {
                m_CanDateTime = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDateTime", value.ToString(), "Capabilities");
            }
        }

        public static bool CanDoesRefraction
        {
            get { return m_CanDoesRefraction; }
            set
            {
                m_CanDoesRefraction = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDoesRefraction", value.ToString(), "Capabilities");
            }
        }

        public static bool CanEquatorial
        {
            get { return m_CanEquatorial; }
            set
            {
                m_CanEquatorial = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanEquatorial", value.ToString(), "Capabilities");
            }
        }

        public static bool CanLatLongElev
        {
            get { return m_CanLatLongElev; }
            set
            {
                m_CanLatLongElev = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanLatLongElev", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSiderealTime
        {
            get { return m_CanSiderealTime; }
            set
            {
                m_CanSiderealTime = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSiderealTime", value.ToString(), "Capabilities");
            }
        }

        #endregion

        #region Telescope Implementation
        public static bool Tracking
        {
            get
            { return m_Tracking; }
            set
            { m_Tracking = value; }
        }
        public static double Altitude
        {
            get { return m_Altitude; }
            set { m_Altitude = value; }
        }
        public static double Azimuth
        {
            get { return m_Azimuth; }
            set { m_Azimuth = value; }
        }
        public static double ParkAltitude
        {
            get { return m_ParkAltitude; }
            set
            {
                m_ParkAltitude = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAltitude", value.ToString(CultureInfo.InvariantCulture));
            }
        }
        public static double ParkAzimuth
        {
            get { return m_ParkAzimuth; }
            set
            {
                m_ParkAzimuth = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAzimuth", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static bool Connected
        {
            get
            { return m_Connected; }
            set
            { m_Connected = value; }
        }
        public static bool CanMoveAxis(ASCOM.DeviceInterface.TelescopeAxes Axis)
        {
            int axis = 0;
            switch (Axis)
            {
                case ASCOM.DeviceInterface.TelescopeAxes.axisPrimary:
                    axis = 1;
                    break;
                case ASCOM.DeviceInterface.TelescopeAxes.axisSecondary:
                    axis = 2;
                    break;
                case ASCOM.DeviceInterface.TelescopeAxes.axisTertiary:
                    axis = 3;
                    break;
            }


            if (axis == 0 || axis > m_NumberMoveAxis)
            { return false; }
            else
            { return true; }
        }

        public static bool CanSetDeclinationRate
        { get { return m_CanSetEquatorialRates; } }

        public static bool CanSetRightAscensionRate
        { get { return m_CanSetEquatorialRates; } }

        public static double ApertureArea
        {
            get { return m_ApertureArea; }
            set
            {
                m_ApertureArea = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "ApertureArea", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double ApertureDiameter
        {
            get { return m_ApertureDiameter; }
            set
            {
                m_ApertureDiameter = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "Aperture", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double FocalLength
        {
            get { return m_FocalLength; }
            set
            {
                m_FocalLength = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "FocalLength", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static bool SouthernHemisphere
        { get { return m_SouthernHemisphere; } }

        public static double RightAscension
        {
            get { return m_RightAscension; }
            set { m_RightAscension = value; }
        }

        public static double Declination
        {
            get { return m_Declination; }
            set { m_Declination = value; }
        }

        public static bool AtPark
        { get { return m_AtPark; } }

        public static SlewType SlewState
        {
            get { return m_SlewState; }
            set { m_SlewState = value; }
        }

        public static SlewSpeed SlewSpeed
        {
            get { return m_SlewSpeed; }
            set { m_SlewSpeed = value; }
        }

        public static SlewDirection SlewDirection
        {
            get { return m_SlewDirection; }
            set { m_SlewDirection = value; }
        }

        /// <summary>
        /// report if the mount is at the home position by comparing it's position with the home position.
        /// </summary>
        public static bool AtHome
        {
            get
            {
                return (Math.Abs(m_Azimuth - 180.0) < 0.01 && Math.Abs(m_Altitude - (90 - m_Latitude)) < 0.01);
            }
        }

        public static double SiderealTime
        { get { return m_SiderealTime; } }


        public static double TargetRightAscension
        {
            get { return m_TargetRightAscension; }
            set { m_TargetRightAscension = value; }
        }
        public static double TargetDeclination
        {
            get { return m_TargetDeclination; }
            set { m_TargetDeclination = value; }
        }
        public static int DateDelta
        {
            get { return m_DateDelta; }
            set
            {
                m_DateDelta = value;
                s_Profile.WriteValue(SharedResources.PROGRAM_ID, "DateDelta", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static double DeclinationRate
        {
            get { return m_DeclinationRate; }
            set { m_DeclinationRate = value; }
        }
        public static double RightAscensionRate
        {
            get { return m_RightAscensionRate; }
            set { m_RightAscensionRate = value; }
        }

        public static double GuideRateDeclination
        {
            get { return m_GuideRateDeclination; }
            set { m_GuideRateDeclination = value; }
        }
        public static double GuideRateRightAscension
        {
            get { return m_GuideRateRightAscension; }
            set { m_GuideRateRightAscension = value; }
        }
        public static int TrackingRate
        {
            get { return m_TrackingRate; }
            set { m_TrackingRate = value; }
        }
        public static double SlewSettleTime
        {
            get { return m_SlewSettleTime; }
            set { m_SlewSettleTime = value; }
        }

        public static bool IsPulseGuiding
        {
            get
            {
                return (isPulseGuidingDec || isPulseGuidingRa);
            }

        }
        public static bool IsParked
        {
            get { return m_AtPark; }
        }
        #endregion

        #region Helper Functions
        public static PierSide SideOfPierRaDec(double rightAscension, double declination)
        {
            PierSide SideOfPier;
            //double hourAngle;
            if (m_AlignmentMode != ASCOM.DeviceInterface.AlignmentModes.algGermanPolar)
            {
                return ASCOM.DeviceInterface.PierSide.pierUnknown;
            }
            else
            {
                double Ha = AstronomyFunctions.HourAngle(rightAscension, m_Longitude);
                if (Ha < 0.0 && Ha >= -12.0) SideOfPier = PierSide.pierWest;
                //else if (Ha < -6.0 && Ha >= -12.0) return  PierSide.pierWest;
                else if (Ha >= 0.0 && Ha <= 12.0) SideOfPier = PierSide.pierEast;
                //else if (Ha > 6.0 && Ha <= 12.0) return PierSide.pierEast;
                else SideOfPier = PierSide.pierUnknown;
                TL.LogMessage("SideOfPierRaDec", rightAscension + " " + declination + " " + Ha + " " + SideOfPier.ToString());

                return SideOfPier;

                //hourAngle = AstronomyFunctions.RangeHa(AstronomyFunctions.LocalSiderealTime(m_Longitude) - RightAscension);
                //hourAngle = AstronomyFunctions.LocalSiderealTime(m_Longitude) - RightAscension;
                //TL.LogMessage("SideOfPierRaDec", "Longitude: " + m_Longitude + "LST: " + AstronomyFunctions.LocalSiderealTime(m_Longitude) + "HA: " + hourAngle + " RA: " + RightAscension);
                //if (hourAngle >=0) return ASCOM.DeviceInterface.PierSide.pierEast;
                //else return ASCOM.DeviceInterface.PierSide.pierWest;

            }
        }

        public static ASCOM.DeviceInterface.PierSide SideOfPier(double azimuth)
        {
            if (m_AlignmentMode != ASCOM.DeviceInterface.AlignmentModes.algGermanPolar)
            {
                return ASCOM.DeviceInterface.PierSide.pierUnknown;
            }
            if (azimuth >= 180) return ASCOM.DeviceInterface.PierSide.pierEast;
            else return ASCOM.DeviceInterface.PierSide.pierWest;
        }

        public static void StartSlewRaDec(double rightAscension, double declination, bool doSideOfPier)
        {
            //ASCOM.DeviceInterface.PierSide targetSideOfPier;
            m_SlewState = SlewType.SlewNone;

            /*if (DoSideOfPier) targetSideOfPier = SideOfPierRaDec(RightAscension, Declination);
            else targetSideOfPier = m_SideOfPier;

            if (targetSideOfPier != m_SideOfPier)
            {
                if (RightAscension >= 12) m_RightAscension = RightAscension - 12;
                else m_RightAscension = RightAscension + 12;

                CalculateAltAz();
                m_SideOfPier = targetSideOfPier;
            } */
            m_DeltaRa = rightAscension - m_RightAscension;
            m_DeltaDec = declination - m_Declination;
            m_DeltaAlt = 0;
            m_DeltaAz = 0;
            TL.LogMessage("StartSlewRaDec", rightAscension + " " + declination + " " + doSideOfPier + " " + m_DeltaRa + " " + m_DeltaDec);

            if (m_DeltaRa < -12) m_DeltaRa = m_DeltaRa + 24;
            else if (m_DeltaRa > 12) m_DeltaRa = m_DeltaRa - 24;
            TL.LogMessage("StartSlewRaDec", rightAscension + " " + declination + " " + doSideOfPier + " " + m_DeltaRa + " " + m_DeltaDec);

            ChangePark(false);

            m_SlewState = SlewType.SlewRaDec;
        }

        public static void StartSlewAltAz(double altitude, double azimuth, bool doSideOfPier, SlewType slew)
        {
            TL.LogMessage("StartSlewAltAz", altitude + " " + azimuth + " " + doSideOfPier + " " + Enum.GetName(typeof(SlewType), slew));
            //ASCOM.DeviceInterface.PierSide targetSideOfPier;
            m_SlewState = SlewType.SlewNone;

            // this seems to do a pier flip by changing the azimuth by 180 degrees if it's neccessary
            /*if (DoSideOfPier) targetSideOfPier = SideOfPier(Azimuth);
            else targetSideOfPier = m_SideOfPier;

            if (targetSideOfPier != m_SideOfPier)
            {
                if (Azimuth >= 180) m_Azimuth = Azimuth -180;
                else m_Azimuth = Azimuth + 180;

                CalculateRaDec();
                m_SideOfPier = targetSideOfPier;
            }*/
            m_DeltaRa = 0;
            m_DeltaDec = 0;
            m_DeltaAlt = altitude - m_Altitude;
            m_DeltaAz = azimuth - m_Azimuth;

            if (m_DeltaAz < -180.0) m_DeltaAz += 360.0;
            if (m_DeltaAz >= 180.0) m_DeltaAz -= 360.0;

            ChangePark(false);

            m_SlewState = slew;
        }

        public static void Park()
        {
            m_Tracking = false;
            TelescopeSimulator.m_MainForm.Tracking();

            StartSlewAltAz(m_ParkAltitude, m_ParkAzimuth, true, SlewType.SlewPark);

        }

        public static void FindHome()
        {
            double altitude;
            double azimuth;
            if (m_AtPark) throw new ParkedException();
            if (m_Latitude >= 0) azimuth = 180;
            else azimuth = 0;

            altitude = 90 - m_Latitude;

            m_Tracking = false;
            TelescopeSimulator.m_MainForm.Tracking();

            StartSlewAltAz(altitude, azimuth, true, SlewType.SlewHome);
        }

        //public static void ChangeHome(bool NewValue)
        //{
        //    m_AtHome = NewValue;
        //}

        public static void ChangePark(bool newValue)
        {
            m_AtPark = newValue;
            if (m_AtPark) TelescopeSimulator.m_MainForm.ParkButton("Unpark");
            else TelescopeSimulator.m_MainForm.ParkButton("Park");
        }

        public static void CalculateAltAz()
        {
            m_Altitude = AstronomyFunctions.CalculateAltitude(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
            m_Azimuth = AstronomyFunctions.CalculateAzimuth(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
            TL.LogMessage("TimerEvent:CalcAltAz", m_Altitude + " " + m_DeltaAlt + " " + m_Azimuth + " " + m_DeltaAz);
        }
        public static void CalculateRaDec()
        {
            m_Declination = AstronomyFunctions.CalculateDec(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD);
            m_RightAscension = AstronomyFunctions.CalculateRA(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
            TL.LogMessage("TimerEvent:CalcRADec", m_Altitude + " " + m_Azimuth + " " + m_Latitude + " " + m_Longitude);
        }
        #endregion

    }

}
