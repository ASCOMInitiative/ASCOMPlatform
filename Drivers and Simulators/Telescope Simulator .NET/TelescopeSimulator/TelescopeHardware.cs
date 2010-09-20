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

namespace ASCOM.TelescopeSimulator
{
    public class TelescopeHardware
    {
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern long GetTickCount();

        private static Timer m_Timer = new Timer(); //Simulated Hardware by running a Timer
        private static Utilities.Profile m_Profile;
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
        private static bool m_AtHome;
        private static bool m_AtPark;

        public static double m_DeclinationRate;
        public static double  m_RightAscensionRate;
        public static double  m_GuideRateDeclination;
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

        public static long m_PulseGuideTixRa = 0;
        public static long m_PulseGuideTixDec = 0;

        public static double m_DeltaAz = 0;
        public static double m_DeltaAlt = 0;
        public static double m_DeltaRa = 0;
        public static double m_DeltaDec = 0;

        private static double m_SettleTix;

        private static ASCOM.DeviceInterface.PierSide m_SideOfPier;

        private static bool m_Connected = false; //Keep track of the connection status of the hardware


        static TelescopeHardware()
        {
            m_Profile = new Utilities.Profile();
            m_Timer.Elapsed += new ElapsedEventHandler(TimerEvent);
            m_Timer.Interval = SharedResources.TIMER_INTERVAL * 1000;
            TL = new ASCOM.Utilities.TraceLogger("", "SimTelescopeHardware");
            TL.Enabled = true;

            if (m_Profile.GetValue(SharedResources.PROGRAM_ID, "RegVer", "") != SharedResources.REGISTRATION_VERSION)
            {
                //Main Driver Settings
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "RegVer", SharedResources.REGISTRATION_VERSION);
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlwaysOnTop", "false");

                //Telescope Implementions
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "1");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "ApertureArea", SharedResources.INSTRUMENT_APERTURE_AREA.ToString());
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Aperture", SharedResources.INSTRUMENT_APERTURE.ToString());
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "FocalLength", SharedResources.INSTRUMENT_FOCAL_LENGTH.ToString());
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "AutoTrack", "false");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "DiscPark", "false");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "NoCoordAtPark", "false");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Refraction", "true");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "EquatorialSystem", "1");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "MaxSlewRate", "20");

                //' Geography
                //'
                //' Based on the UTC offset, create a longitude somewhere in
                //' the time zone, a latitude between 0 and 60 and a site
                //' elevation between 0 and 1000 metres. This gives the
                //' client some geo position without having to open the
                //' Setup dialog.
                Random r = new Random();
                TimeZone localZone = TimeZone.CurrentTimeZone;
                double latitude = (r.NextDouble() * 60);
                double longitude = (((-(double)(localZone.GetUtcOffset(DateTime.Now).Seconds) / 3600) + r.NextDouble() - 0.5) * 15);
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Elevation", Math.Round((r.NextDouble()*1000),0).ToString());
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Longitude", longitude.ToString());
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Latitude", latitude.ToString());

                //Start the scope in parked position
                if (latitude >= 0)
                {
                    m_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAzimuth", "180");
                    m_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAzimuth", "180");
                    
                }
                else
                {
                    m_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAzimuth", "90");
                    m_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAzimuth", "90");
                }
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "StartAltitude", (90-Math.Abs(latitude)).ToString());
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAltitude", (90 - Math.Abs(latitude)).ToString());

                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "DateDelta", "0");

                //Capabilities Settings
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "V1", "false", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanFindHome", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPark", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "NumMoveAxis", "2", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPulseGuide", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetEquRates", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetGuideRates", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetPark", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetPierSide", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetTracking", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlew", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAltAz", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanAlignMode", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanOptics", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAltAzAsync", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAsync", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSync", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSyncAltAz", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanUnpark", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanAltAz", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDateTime", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDoesRefraction", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanEquatorial", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanLatLongElev", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSiderealTime", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPierSide", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanTrackingRates", "true", "Capabilities");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDualAxisPulseGuide", "true", "Capabilities");


                

                
            }

            //Load up the values from saved
            m_OnTop = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID,"AlwaysOnTop"));

            switch (int.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "AlignMode")))
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
                
           



            m_ApertureArea = double.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "ApertureArea"));
            m_ApertureArea = double.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "Aperture"));
            m_FocalLength = double.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "FocalLength"));
            m_AutoTrack = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "AutoTrack"));
            m_DisconnectOnPark = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "DiscPark"));
            m_Refraction = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "Refraction"));
            m_EquatorialSystem = int.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "EquatorialSystem"));
            m_NoCoordinatesAtPark = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "NoCoordAtPark"));
            m_Elevation = double.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "Elevation"));
            m_Latitude = double.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "Latitude"));
            m_Longitude = double.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "Longitude"));
            m_MaximumSlewRate = int.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "MaxSlewRate"));

            m_Altitude = double.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "StartAltitude"));
            m_Azimuth = double.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "StartAzimuth"));
            m_ParkAltitude = double.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "ParkAltitude"));
            m_ParkAzimuth = double.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "ParkAzimuth"));

            m_VersionOne = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "V1", "Capabilities"));
            m_CanFindHome = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanFindHome", "Capabilities"));
            m_CanPark = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanPark", "Capabilities"));
            m_NumberMoveAxis = int.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "NumMoveAxis", "Capabilities"));
            m_CanPulseGuide = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanPulseGuide", "Capabilities"));
            m_CanSetEquatorialRates = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetEquRates", "Capabilities"));
            m_CanSetGuideRates = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetGuideRates", "Capabilities"));
            m_CanSetPark = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetPark", "Capabilities"));
            m_CanSetPierSide = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetPierSide", "Capabilities"));
            m_CanSetTracking = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSetTracking", "Capabilities"));
            m_CanSlew = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSlew", "Capabilities"));
            m_CanSlewAltAz = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSlewAltAz", "Capabilities"));
            m_CanAlignmentMode = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanAlignMode", "Capabilities"));
            m_CanOptics = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanOptics", "Capabilities"));
            m_CanSlewAltAzAsync = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSlewAltAzAsync", "Capabilities"));
            m_CanSlewAsync = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSlewAsync", "Capabilities"));
            m_CanSync = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSync", "Capabilities"));
            m_CanSyncAltAz = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSyncAltAz", "Capabilities"));
            m_CanUnpark = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanUnpark", "Capabilities"));
            m_CanAltAz = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanAltAz", "Capabilities"));
            m_CanDateTime = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanDateTime", "Capabilities"));
            m_CanDoesRefraction = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanDoesRefraction", "Capabilities"));
            m_CanEquatorial = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanEquatorial", "Capabilities"));
            m_CanLatLongElev = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanLatLongElev", "Capabilities"));
            m_CanSiderealTime = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanSiderealTime", "Capabilities"));
            m_CanPierSide = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanPierSide", "Capabilities"));
            m_CanTrackingRates = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanTrackingRates", "Capabilities"));
            m_CanDualAxisPulseGuide = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "CanDualAxisPulseGuide", "Capabilities"));

            m_DateDelta = int.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "DateDelta"));

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
            
        }
        public static void Start() 
        {
            m_Connected = false;
            m_Tracking = false;
            m_AtHome = false;
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

            m_Timer.Start(); 
        }

        //Update the Telescope Based on Timed Events
        private static void TimerEvent(object source, ElapsedEventArgs e)
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
                z = Math.Cos(m_Declination * SharedResources.DEG_RAD) * 2; // *15;
                if (z < 0.001) {z = 0.001;}

                if (m_SlewState == SlewType.SlewHandpad) //We are doing a slew with the handpad buttons
                {
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
                        case SlewDirection.SlewUp:
                            m_Altitude += step;
                            m_Declination = AstronomyFunctions.CalculateDec(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD);
                            m_RightAscension = AstronomyFunctions.CalculateRa(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewDown:
                            m_Altitude -= step;
                            m_Declination = AstronomyFunctions.CalculateDec(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD);
                            m_RightAscension = AstronomyFunctions.CalculateRa(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewRight:
                            m_Azimuth += step;
                            m_Declination = AstronomyFunctions.CalculateDec(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD);
                            m_RightAscension = AstronomyFunctions.CalculateRa(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewLeft:
                            m_Azimuth -= step;
                            m_Declination = AstronomyFunctions.CalculateDec(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD);
                            m_RightAscension = AstronomyFunctions.CalculateRa(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
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
                            m_RightAscension += step*z;
                            m_RightAscension = AstronomyFunctions.RangeHa(m_RightAscension);
                            m_Altitude = AstronomyFunctions.CalculateAltitude(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            m_Azimuth = AstronomyFunctions.CalculateAzimuth(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
                        case SlewDirection.SlewWest:
                            m_RightAscension -= step*z;
                            m_RightAscension = AstronomyFunctions.RangeHa(m_RightAscension);
                            m_Altitude = AstronomyFunctions.CalculateAltitude(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            m_Azimuth = AstronomyFunctions.CalculateAzimuth(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
                            break;
                    }
                }
                else if (m_SlewState == SlewType.SlewRaDec)
                {
                    m_SettleTix = GetTickCount() + m_SlewSettleTime;
                    TL.LogMessage("Fast, Med, Slow, Target RA, Dec", m_SlewSpeedFast + " " + m_SlewSpeedMedium + " " + m_SlewSpeedSlow + " " + m_TargetRightAscension + " " + m_TargetDeclination);

                    //RA Step
                    y = Math.Abs(m_DeltaRa);
                    if (m_SlewSpeedFast / SharedResources.TIMER_INTERVAL >= 50) { step = y / z; TL.LogStart("RA Speed z y step", "Maximum"); }
                    else if (y > 2 * m_SlewSpeedFast) { step = m_SlewSpeedFast / z; TL.LogStart("RA Speed z y step", "Fast"); }
                    else if (y > 2 * m_SlewSpeedMedium) { step = m_SlewSpeedMedium / z; TL.LogStart("RA Speed z y step", "Medium"); }
                    else if (y > 2 * m_SlewSpeedSlow) { step = m_SlewSpeedSlow / z; TL.LogStart("RA Speed z y step", "Slow"); }
                    else { step = y / z; TL.LogStart("RA Speed z y step", "Minimum"); }

                    step = step * Math.Sign(m_DeltaRa);

                    TL.LogFinish(" " + z + " " + y + " " + step);

                    m_RightAscension += step;
                    m_DeltaRa -= step;

                    //Dec Step
                    y = Math.Abs(m_DeltaDec);
                    if (m_SlewSpeedFast / SharedResources.TIMER_INTERVAL >= 50) { step = y; TL.LogStart("Dec Speed z y step", "Maximum"); }
                    else if (y > 2 * m_SlewSpeedFast) { step = m_SlewSpeedFast; TL.LogStart("Dec Speed z y step", "Fast"); }
                    else if (y > 2 * m_SlewSpeedMedium) { step = m_SlewSpeedMedium; TL.LogStart("Dec Speed z y step", "Medium"); }
                    else if (y > 2 * m_SlewSpeedSlow) { step = m_SlewSpeedSlow; TL.LogStart("Dec Speed z y step", "Slow"); }
                    else { step = y; TL.LogStart("Dec Speed z y step", "Minimum"); }

                    step = step * Math.Sign(m_DeltaDec);
                    TL.LogFinish( " " + z + " " + y + " " + step);

                    m_Declination += step;
                    m_DeltaDec -= step;

                    m_Declination = AstronomyFunctions.RangeDec(m_Declination);
                    m_RightAscension = AstronomyFunctions.RangeHa(m_RightAscension);
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
                    m_SettleTix = GetTickCount() + m_SlewSettleTime;

                    //Altitude Step
                    y = Math.Abs(m_DeltaAlt);
                    if (m_SlewSpeedFast / SharedResources.TIMER_INTERVAL >= 50) step = y;
                    else if (y > 2 * m_SlewSpeedFast) step = m_SlewSpeedFast;
                    else if (y > 2 * m_SlewSpeedMedium) step = m_SlewSpeedMedium;
                    else if (y > 2 * m_SlewSpeedSlow) step = m_SlewSpeedSlow;
                    else step = y;

                    step = step * Math.Sign(m_DeltaAlt);

                    m_Altitude += step;
                    m_DeltaAlt -= step;

                    //Azimuth Step
                    y = Math.Abs(m_DeltaAz);
                    if (m_SlewSpeedFast / SharedResources.TIMER_INTERVAL >= 50) step = y;
                    else if (y > 2 * m_SlewSpeedFast) step = m_SlewSpeedFast;
                    else if (y > 2 * m_SlewSpeedMedium) step = m_SlewSpeedMedium;
                    else if (y > 2 * m_SlewSpeedSlow) step = m_SlewSpeedSlow;
                    else step = y;

                    step = step * Math.Sign(m_DeltaAz);

                    m_Azimuth += step;
                    m_DeltaAz -= step;

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
                            ChangeHome(true);
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
                        m_RightAscension = AstronomyFunctions.RangeHa(m_RightAscension);

                        CalculateAltAz();
                    }

                }
            }
            if (m_PulseGuideTixRa > 0 || m_PulseGuideTixDec > 0)
            {
                ChangeHome(false);
                ChangePark(false);
                if (m_PulseGuideTixRa > 0)
                {
                    if (m_PulseGuideTixRa + (SharedResources.TIMER_INTERVAL / 2) <= GetTickCount())
                    {
                        if (SharedResources.TrafficForm != null)
                        {
                            if (SharedResources.TrafficForm.Slew)
                            {
                                SharedResources.TrafficForm.TrafficLine("(PulseGuide in RA complete)");

                            }
                        }
                        m_PulseGuideTixRa = 0;
                    }
                }

                if (m_PulseGuideTixDec > 0)
                {
                    if (m_PulseGuideTixDec + (SharedResources.TIMER_INTERVAL / 2) <= GetTickCount())
                    {
                        if (SharedResources.TrafficForm != null)
                        {
                            if (SharedResources.TrafficForm.Slew)
                            {
                                SharedResources.TrafficForm.TrafficLine("(PulseGuide in Dec complete)");

                            }
                        }
                        m_PulseGuideTixDec = 0;
                    }
                }

                if (m_Tracking) raRate = m_RightAscensionRate;
                else raRate = 15;
                raRate = (raRate / SharedResources.SIDRATE) / 3600;
                if (m_PulseGuideTixRa > 0) raRate = raRate + (m_GuideRateRightAscension / 15);

                decRate = m_DeclinationRate / 3600;
                if (m_PulseGuideTixDec > 0) decRate = decRate + m_GuideRateDeclination;

                m_RightAscension += raRate * SharedResources.TIMER_INTERVAL;
                m_Declination += decRate * SharedResources.TIMER_INTERVAL;

                m_Declination = AstronomyFunctions.RangeDec(m_Declination);
                m_RightAscension = AstronomyFunctions.RangeHa(m_RightAscension);

                CalculateAltAz();
            }

            if (m_SlewState == SlewType.SlewSettle)
            {
                if (GetTickCount() > m_SettleTix)
                {
                    if (SharedResources.TrafficForm != null)
                    {
                        if (SharedResources.TrafficForm.Slew)
                        {
                            SharedResources.TrafficForm.TrafficLine("(Slew Complete)");

                        }
                    }
                    m_SlewState = SlewType.SlewNone;
                }
            }

            m_SiderealTime = AstronomyFunctions.LocalSiderealTime(m_Longitude);
            TelescopeSimulator.m_MainForm.SiderealTime = m_SiderealTime;
            TelescopeSimulator.m_MainForm.Altitude = m_Altitude;
            TelescopeSimulator.m_MainForm.Azimuth = m_Azimuth;
            TelescopeSimulator.m_MainForm.RightAscension = m_RightAscension;
            TelescopeSimulator.m_MainForm.Declination = m_Declination;
            TelescopeSimulator.m_MainForm.Tracking();
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
                        m_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "0");
                        break;
                    case ASCOM.DeviceInterface.AlignmentModes.algGermanPolar:
                        m_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "1");
                        break;
                    case ASCOM.DeviceInterface.AlignmentModes.algPolar:
                        m_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "2");
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
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "OnTop", value.ToString(), "");
            }
        }
        public static bool AutoTrack
        {
            get { return m_AutoTrack; }
            set
            {
                m_AutoTrack = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "AutoTrack", value.ToString(), "");
            }
        }
        public static bool NoCoordinatesAtPark
        {
            get { return m_NoCoordinatesAtPark; }
            set
            {
                m_NoCoordinatesAtPark = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "NoCoordAtPark", value.ToString());
            }
        }
        public static bool VersionOneOnly
        {
            get { return m_VersionOne; }
            set 
            {
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "V1", value.ToString(), "Capabilities");
                m_VersionOne = value; 
            }
        }
        public static bool DisconnectOnPark
        {
            get { return m_DisconnectOnPark; }
            set
            {
                m_DisconnectOnPark = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "DiscPark", value.ToString(), "");
            }
        }
        public static bool Refraction
        {
            get { return m_Refraction; }
            set
            {
                m_Refraction = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Refraction", value.ToString(), "");
            }
        }
        public static int EquatorialSystem
        {
            get { return m_EquatorialSystem; }
            set
            {
                m_EquatorialSystem = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "EquatorialSystem", value.ToString(), "");
            }
        }
        public static double Elevation
        {
            get { return m_Elevation; }
            set
            {
                m_Elevation = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Elevation", value.ToString());
            }
        }
        public static double Latitude
        {
            get { return m_Latitude; }
            set
            {
                m_Latitude = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Latitude", value.ToString());
                if (m_Latitude < 0) { m_SouthernHemisphere = true; }
            }
        }
        public static double Longitude
        {
            get { return m_Longitude; }
            set
            {
                m_Longitude = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Longitude", value.ToString());
            }
        }
        public static int MaximumSlewRate
        {
            get { return m_MaximumSlewRate; }
            set
            {
                m_MaximumSlewRate = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "MaxSlewRate", value.ToString());
            }
        }


        public static bool CanFindHome
        {
            get {return m_CanFindHome;}
            set
            {
                m_CanFindHome = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID,  "CanFindHome", value.ToString(), "Capabilities");
            }
        }
        public static bool CanOptics
        {
            get { return m_CanOptics; }
            set
            {
                m_CanOptics = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanOptics", value.ToString(), "Capabilities");
            }
        }
        public static bool CanPark
        {
            get {return m_CanPark;}
            set
            {
                m_CanPark = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPark", value.ToString(), "Capabilities");
            }
        }
        public static int NumberMoveAxis
        {
            get { return m_NumberMoveAxis; }
            set
            {
                m_NumberMoveAxis = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "NumMoveAxis", value.ToString(), "Capabilities");
            }
        }

        public static bool CanPulseGuide
        {
            get{return m_CanPulseGuide;}
            set
            {
                m_CanPulseGuide = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPulseGuide", value.ToString(), "Capabilities");
            }
        }
        public static bool CanDualAxisPulseGuide
        {
            get { return m_CanDualAxisPulseGuide; }
            set
            {
                m_CanDualAxisPulseGuide = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDualAxisPulseGuide", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetEquatorialRates
        {
            get{return m_CanSetEquatorialRates;}
            set
            {
                m_CanSetEquatorialRates = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetEquRates", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSetGuideRates
        {
            get{return m_CanSetGuideRates;}
            set
            {
                m_CanSetGuideRates = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetGuideRates", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSetPark
        {
            get {return m_CanSetPark;}
            set
            {
                m_CanSetPark = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetPark", value.ToString(), "Capabilities");
            }
        }
        public static bool CanPierSide
        {
            get { return m_CanPierSide; }
            set
            {
                m_CanPierSide = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanPierSide", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSetPierSide
        {
            get{return m_CanSetPierSide;}
            set
            {
                m_CanSetPierSide = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetPierSide", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetTracking
        {
            get{return m_CanSetTracking;}
            set
            {
                m_CanSetTracking = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSetTracking", value.ToString(), "Capabilities");
            }
        }
        public static bool CanTrackingRates
        {
            get { return m_CanTrackingRates; }
            set
            {
                m_CanTrackingRates = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanTrackingRates", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSlew
        {
            get{return m_CanSlew;}
            set
            {
                m_CanSlew = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlew", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSync
        {
            get { return m_CanSync; }
            set
            {
                m_CanSync = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSync", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSlewAsync
        {
            get { return m_CanSlewAsync; }
            set
            {
                m_CanSlewAsync = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAsync", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSlewAltAz
        {
            get{return m_CanSlewAltAz;}
            set
            {
                m_CanSlewAltAz = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAltAz", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSyncAltAz
        {
            get { return m_CanSyncAltAz; }
            set
            {
                m_CanSyncAltAz = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSyncAltAz", value.ToString(), "Capabilities");
            }
        }
        public static bool CanAltAz
        {
            get { return m_CanAltAz; }
            set
            {
                m_CanAltAz = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanAltAz", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSlewAltAzAsync
        {
            get { return m_CanSlewAltAzAsync; }
            set
            {
                m_CanSlewAltAzAsync = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlewAltAzAsync", value.ToString(), "Capabilities");
            }
        }
        public static bool CanAlignmentMode
        {
            get { return m_CanAlignmentMode; }
            set
            {
                m_CanAlignmentMode = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanAlignMode", value.ToString(), "Capabilities");
            }
        }
        public static bool CanUnpark
        {
            get { return m_CanUnpark; }
            set
            {
                m_CanUnpark = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanUnpark", value.ToString(), "Capabilities");
            }
        }
        public static bool CanDateTime
        {
            get { return m_CanDateTime; }
            set
            {
                m_CanDateTime = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDateTime", value.ToString(), "Capabilities");
            }
        }
        public static bool CanDoesRefraction
        {
            get { return m_CanDoesRefraction; }
            set
            {
                m_CanDoesRefraction = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanDoesRefraction", value.ToString(), "Capabilities");
            }
        }
        public static bool CanEquatorial
        {
            get { return m_CanEquatorial; }
            set
            {
                m_CanEquatorial = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanEquatorial", value.ToString(), "Capabilities");
            }
        }
        public static bool CanLatLongElev
        {
            get { return m_CanLatLongElev; }
            set
            {
                m_CanLatLongElev = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanLatLongElev", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSiderealTime
        {
            get { return m_CanSiderealTime; }
            set
            {
                m_CanSiderealTime = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSiderealTime", value.ToString(), "Capabilities");
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
            set {m_Altitude = value;}
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
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAltitude", value.ToString());
            }
        }
        public static double ParkAzimuth
        {
            get { return m_ParkAzimuth; }
            set 
            { 
                m_ParkAzimuth = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "ParkAzimuth", value.ToString());
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
            int axis =0;
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


            if (axis ==0 || axis > m_NumberMoveAxis)
            {return false;}
            else
            {return true;}
        }
       public static bool CanSetDeclinationRate
       {get {return m_CanSetEquatorialRates;}}

       public static bool CanSetRightAscensionRate
       {get{return m_CanSetEquatorialRates;}}

       public static double ApertureArea
       {
           get { return m_ApertureArea; }
           set
           {
               m_ApertureArea = value;
               m_Profile.WriteValue(SharedResources.PROGRAM_ID, "ApertureArea", value.ToString());
           }
       }
       public static double ApertureDiameter
       {
           get { return m_ApertureDiameter; }
           set
           {
               m_ApertureDiameter = value;
               m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Aperture", value.ToString());
           }
       }
       public static double FocalLength
       {
           get { return m_FocalLength; }
           set
           {
               m_FocalLength = value;
               m_Profile.WriteValue(SharedResources.PROGRAM_ID, "FocalLength", value.ToString());
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
       public static bool AtHome
       { get { return m_AtHome; } }
      
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
               m_Profile.WriteValue(SharedResources.PROGRAM_ID, "DateDelta", value.ToString());
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
               return ((m_PulseGuideTixDec > 0) || (m_PulseGuideTixRa > 0));
           }

       }
       public static bool IsParked
       {
           get { return m_AtPark; }
       }
       #endregion

       #region Helper Functions
       public static ASCOM.DeviceInterface.PierSide SideOfPierRaDec(double RightAscension, double Declination)
       {
           double hourAngle;
           if (m_AlignmentMode != ASCOM.DeviceInterface.AlignmentModes.algGermanPolar)
           {
               return ASCOM.DeviceInterface.PierSide.pierUnknown;
           }
           else
           {
               //hourAngle = AstronomyFunctions.RangeHa(AstronomyFunctions.LocalSiderealTime(m_Longitude) - RightAscension);
               hourAngle = AstronomyFunctions.LocalSiderealTime(m_Longitude) - RightAscension;
               TL.LogMessage("SideOfPierRaDec", "Longitude: " + m_Longitude + "LST: " + AstronomyFunctions.LocalSiderealTime(m_Longitude) + "HA: " + hourAngle + " RA: " + RightAscension);
               if (hourAngle >=0) return ASCOM.DeviceInterface.PierSide.pierEast;
               else return ASCOM.DeviceInterface.PierSide.pierWest;

           }
       }
       public static ASCOM.DeviceInterface.PierSide SideOfPier(double Azimuth)
       {
           if (m_AlignmentMode != ASCOM.DeviceInterface.AlignmentModes.algGermanPolar)
           {
               return ASCOM.DeviceInterface.PierSide.pierUnknown;
           }
           if (Azimuth >= 180) return ASCOM.DeviceInterface.PierSide.pierEast;
           else return ASCOM.DeviceInterface.PierSide.pierWest;
       }
       public static void StartSlewRaDec(double RightAscension, double Declination, bool DoSideOfPier)
       {
           ASCOM.DeviceInterface.PierSide targetSideOfPier;
           m_SlewState = SlewType.SlewNone;

           if (DoSideOfPier) targetSideOfPier = SideOfPierRaDec(RightAscension, Declination);
           else targetSideOfPier = m_SideOfPier;

           if (targetSideOfPier != m_SideOfPier)
           {
               if (RightAscension >= 12) m_RightAscension = RightAscension - 12;
               else m_RightAscension = RightAscension + 12;

               CalculateAltAz();
               m_SideOfPier = targetSideOfPier;
               TelescopeSimulator.m_MainForm.LEDPier(m_SideOfPier);
           }
           m_DeltaRa = RightAscension - m_RightAscension;
           m_DeltaDec = Declination - m_Declination;
           m_DeltaAlt = 0;
           m_DeltaAz = 0;

           if (m_DeltaRa < -12) m_DeltaRa = m_DeltaRa + 24;
           else if (m_DeltaRa > 12) m_DeltaRa = m_DeltaRa - 24;

           ChangeHome(false);
           ChangePark(false);

           m_SlewState = SlewType.SlewRaDec;
       }
       public static void StartSlewAltAz(double Altitude, double Azimuth, bool DoSideOfPier, SlewType Slew)
       {
           ASCOM.DeviceInterface.PierSide targetSideOfPier;
           m_SlewState = SlewType.SlewNone;

           if (DoSideOfPier) targetSideOfPier = SideOfPier(Azimuth);
           else targetSideOfPier = m_SideOfPier;

           if (targetSideOfPier != m_SideOfPier)
           {
               if (Azimuth >= 180) m_Azimuth = Azimuth -180;
               else m_Azimuth = Azimuth + 180;

               CalculateRaDec();
               m_SideOfPier = targetSideOfPier;
               TelescopeSimulator.m_MainForm.LEDPier(m_SideOfPier);
           }
           m_DeltaRa = 0;
           m_DeltaDec = 0;
           m_DeltaAlt = Altitude - m_Altitude;
           m_DeltaAz = Azimuth - m_Azimuth;

           if (m_DeltaAz < -180) m_DeltaAz = m_DeltaAz +360;
           else if (m_DeltaAz > 180) m_DeltaAz = m_DeltaAz -360;

           ChangeHome(false);
           ChangePark(false);

           m_SlewState = Slew;
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
       public static void ChangeHome(bool NewValue)
       {
           m_AtHome = NewValue;
       }
       public static void ChangePark(bool NewValue)
       {
           m_AtPark = NewValue;
       }
       public static void CalculateAltAz()
       {
           m_Altitude = AstronomyFunctions.CalculateAltitude(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
           m_Azimuth = AstronomyFunctions.CalculateAzimuth(m_RightAscension * SharedResources.HRS_RAD, m_Declination * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
       }
       public static void CalculateRaDec()
       {
           m_Declination = AstronomyFunctions.CalculateDec(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD);
           m_RightAscension = AstronomyFunctions.CalculateRa(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD, m_Longitude * SharedResources.DEG_RAD);
       }
        #endregion



    }
    
}
