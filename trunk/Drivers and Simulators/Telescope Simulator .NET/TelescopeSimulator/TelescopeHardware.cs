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
        private static bool m_CanSetEquatorialRates;
        private static bool m_CanSetGuideRates;
        private static bool m_CanSetPark;
        private static bool m_CanSetPierSide;
        private static bool m_CanSetTracking;
        private static bool m_CanSlew;
        private static bool m_CanSlewAltAz;
        private static bool m_CanAlignmentMode;
        private static bool m_CanOptics;

        //Telescope Implementation
        private static int m_AlignmentMode;
        private static double m_ApertureArea;
        private static double m_ApertureDiameter;

        private static bool m_Connected = false; //Keep track of the connection status of the hardware


        static TelescopeHardware()
        {
            m_Profile = new HelperNET.Profile();
            m_Timer.Elapsed += new ElapsedEventHandler(TimerEvent);
            m_Timer.Start();

            if (m_Profile.GetValue(SharedResources.PROGRAM_ID, "RegVer", "") != SharedResources.REGISTRATION_VERSION)
            {
                //Main Driver Settings
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "RegVer", SharedResources.REGISTRATION_VERSION);
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlwaysOnTop", "false");

                //Telescope Implementions
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", "1");
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "ApertureArea", SharedResources.INSTRUMENT_APERTURE_AREA.ToString());
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Aperture", SharedResources.INSTRUMENT_APERTURE.ToString());

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


                

                
            }

            //Load up the values from saved
            m_OnTop = bool.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID,"AlwaysOnTop"));
            m_AlignmentMode = int.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "AlignMode"));
            m_ApertureArea = double.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "ApertureArea"));
            m_ApertureArea = double.Parse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "Aperture"));

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
            

            //Set the form setting for the Always On Top Value
            TelescopeSimulator.m_MainForm.TopMost = m_OnTop;
        }

        //Update the Telescope Based on Timed Events
        private static void TimerEvent(object source, ElapsedEventArgs e)
        {

        }

        #region Properties For Settings

        //I used some of these as dual purpose if the driver uses the same exact property
        public static bool OnTop
        {
            get { return m_OnTop; }
            set
            {
                m_OnTop = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "V1", value.ToString(), "");
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

        public static bool CanSlew
        {
            get{return m_CanSlew;}
            set
            {
                m_CanSlew = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanSlew", value.ToString(), "Capabilities");
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
        public static bool CanAlignmentMode
        {
            get { return m_CanAlignmentMode; }
            set
            {
                m_CanAlignmentMode = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "CanAlignMode", value.ToString(), "Capabilities");
            }
        }
        #endregion

        #region Telescope Implementation
        public static int AlignmentMode
        {
            get { return m_AlignmentMode; }
            set
            {
                m_AlignmentMode = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "AlignMode", value.ToString());
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

        #endregion
    }
}
