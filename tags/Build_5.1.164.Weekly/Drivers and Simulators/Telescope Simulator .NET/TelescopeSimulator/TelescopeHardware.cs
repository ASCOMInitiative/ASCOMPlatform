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

namespace ASCOM.TelescopeSimulator
{
    public class TelescopeHardware
    {
        private static Timer m_Timer = new Timer(); //Simulated Hardware by running a Timer
        private static HelperNET.Profile m_Profile;

        

        private static bool m_OnTop;

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
        private static int m_AlignmentMode;
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

        private static double m_DateDelta;

        private static bool m_Tracking;
        private static bool m_AtHome;
        private static bool m_AtPark;

        private static bool m_SouthernHemisphere = false;


        private static bool m_Connected = false; //Keep track of the connection status of the hardware


        static TelescopeHardware()
        {
            m_Profile = new HelperNET.Profile();
            m_Timer.Elapsed += new ElapsedEventHandler(TimerEvent);
            m_Timer.Interval = SharedResources.TIMER_INTERVAL * 1000;
            

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
            m_AlignmentMode = int.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "AlignMode"));
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

            if (m_Latitude < 0) { m_SouthernHemisphere = true; }

            //Set the form setting for the Always On Top Value
            TelescopeSimulator.m_MainForm.TopMost = m_OnTop;

        }
        public static void Start() 
        {
            m_Connected = false;
            m_Tracking = false;
            m_AtHome = false;
            m_AtPark = false;


            m_Timer.Start(); 
        }

        //Update the Telescope Based on Timed Events
        private static void TimerEvent(object source, ElapsedEventArgs e)
        {

            m_Declination = AstronomyFunctions.CalculateDec(m_Altitude * SharedResources.DEG_RAD, m_Azimuth * SharedResources.DEG_RAD, m_Latitude * SharedResources.DEG_RAD);
            m_RightAscension = AstronomyFunctions.CalculateRa(m_Altitude* SharedResources.DEG_RAD, (360 - m_Azimuth)*SharedResources.DEG_RAD, m_Latitude, m_Longitude, m_Declination);
            

            TelescopeSimulator.m_MainForm.SiderealTime = AstronomyFunctions.LocalSiderealTime(m_Longitude);
            TelescopeSimulator.m_MainForm.Altitude = m_Altitude;
            TelescopeSimulator.m_MainForm.Azimuth = m_Azimuth;
            TelescopeSimulator.m_MainForm.RightAscension = m_RightAscension;
            TelescopeSimulator.m_MainForm.Declination = m_Declination*SharedResources.RAD_DEG;
        }

        #region Properties For Settings

        //I used some of these as dual purpose if the driver uses the same exact property
        public static int AlignmentMode
        {
            get { return m_AlignmentMode; }
            set
            {
                m_AlignmentMode = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", value.ToString());
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
       public static bool CanMoveAxis(int Axis)
        {
            if (Axis < 0 || Axis > m_NumberMoveAxis)
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
        #endregion

        #region Helper Functions

        #endregion

    }
}
