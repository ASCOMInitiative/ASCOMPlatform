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

        private static string c_ProgramID = "ASCOM.TelescopeSimulator.Telescope";

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

        private static bool m_Connected = false; //Keep track of the connection status of the hardware


        static TelescopeHardware()
        {
            m_Profile = new HelperNET.Profile();
            m_Timer.Elapsed += new ElapsedEventHandler(TimerEvent);
            m_Timer.Start();

            if (m_Profile.GetValue(c_ProgramID, "RegVer", "") != "1")
            {
                //Main Driver Settings
                m_Profile.WriteValue(c_ProgramID, "RegVer", "1");
                m_Profile.WriteValue(c_ProgramID, "AlwaysOnTop", "false");

                //Capabilities Settings
                m_Profile.WriteValue(c_ProgramID, "V1", "false", "Capabilities");
                m_Profile.WriteValue(c_ProgramID, "CanFindHome", "true", "Capabilities");
                m_Profile.WriteValue(c_ProgramID, "CanPark", "true", "Capabilities");
                m_Profile.WriteValue(c_ProgramID, "NumMoveAxis", "2", "Capabilities");
                m_Profile.WriteValue(c_ProgramID, "CanPulseGuide", "true", "Capabilities");
                m_Profile.WriteValue(c_ProgramID, "CanSetEquRates", "true", "Capabilities");
                m_Profile.WriteValue(c_ProgramID, "CanSetGuideRates", "true", "Capabilities");
                m_Profile.WriteValue(c_ProgramID, "CanSetPark", "true", "Capabilities");
                m_Profile.WriteValue(c_ProgramID, "CanSetPierSide", "true", "Capabilities");
                m_Profile.WriteValue(c_ProgramID, "CanSetTracking", "true", "Capabilities");
                m_Profile.WriteValue(c_ProgramID, "CanSlew", "true", "Capabilities");
                m_Profile.WriteValue(c_ProgramID, "CanSlewAltAz", "true", "Capabilities");

                
            }

            //Load up the values from saved
            m_OnTop = bool.Parse(m_Profile.GetValue(c_ProgramID,"AlwaysOnTop"));


            m_VersionOne = bool.Parse(m_Profile.GetValue(c_ProgramID, "V1", "Capabilities"));
            m_CanFindHome = bool.Parse(m_Profile.GetValue(c_ProgramID, "CanFindHome", "Capabilities"));
            m_CanPark = bool.Parse(m_Profile.GetValue(c_ProgramID, "CanPark", "Capabilities"));
            m_NumberMoveAxis = int.Parse(m_Profile.GetValue(c_ProgramID, "NumMoveAxis", "Capabilities"));
            m_CanPulseGuide = bool.Parse(m_Profile.GetValue(c_ProgramID, "CanPulseGuide", "Capabilities"));
            m_CanSetEquatorialRates = bool.Parse(m_Profile.GetValue(c_ProgramID, "CanSetEquRates", "Capabilities"));
            m_CanSetGuideRates = bool.Parse(m_Profile.GetValue(c_ProgramID, "CanSetGuideRates", "Capabilities"));
            m_CanSetPark = bool.Parse(m_Profile.GetValue(c_ProgramID, "CanSetPark", "Capabilities"));
            m_CanSetPierSide = bool.Parse(m_Profile.GetValue(c_ProgramID, "CanSetPierSide", "Capabilities"));
            m_CanSetTracking = bool.Parse(m_Profile.GetValue(c_ProgramID, "CanSetTracking", "Capabilities"));
            m_CanSlew = bool.Parse(m_Profile.GetValue(c_ProgramID, "CanSlew", "Capabilities"));
            m_CanSlewAltAz = bool.Parse(m_Profile.GetValue(c_ProgramID, "CanSlewAltAz", "Capabilities"));


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
                m_Profile.WriteValue(c_ProgramID, "V1", value.ToString(), "");
            }
        }
        public static bool VersionOneOnly
        {
            get { return m_VersionOne; }
            set 
            {
                m_Profile.WriteValue(c_ProgramID, "V1", value.ToString(), "Capabilities");
                m_VersionOne = value; 
            }
        }
        public static bool CanFindHome
        {
            get {return m_CanFindHome;}
            set
            {
                m_CanFindHome = value;
                m_Profile.WriteValue(c_ProgramID,  "CanFindHome", value.ToString(), "Capabilities");
            }
        }

        public static bool CanPark
        {
            get {return m_CanPark;}
            set
            {
                m_CanPark = value;
                m_Profile.WriteValue(c_ProgramID, "CanPark", value.ToString(), "Capabilities");
            }
        }
        public static int NumberMoveAxis
        {
            get { return m_NumberMoveAxis; }
            set
            {
                m_NumberMoveAxis = value;
                m_Profile.WriteValue(c_ProgramID, "NumMoveAxis", value.ToString(), "Capabilities");
            }
        }

        public static bool CanPulseGuide
        {
            get{return m_CanPulseGuide;}
            set
            {
                m_CanPulseGuide = value;
                m_Profile.WriteValue(c_ProgramID, "CanPulseGuide", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetEquatorialRates
        {
            get{return m_CanSetEquatorialRates;}
            set
            {
                m_CanSetEquatorialRates = value;
                m_Profile.WriteValue(c_ProgramID, "CanSetEquRates", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSetGuideRates
        {
            get{return m_CanSetGuideRates;}
            set
            {
                m_CanSetGuideRates = value;
                m_Profile.WriteValue(c_ProgramID, "CanSetGuideRates", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSetPark
        {
            get {return m_CanSetPark;}
            set
            {
                m_CanSetPark = value;
                m_Profile.WriteValue(c_ProgramID, "CanSetPark", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSetPierSide
        {
            get{return m_CanSetPierSide;}
            set
            {
                m_CanSetPierSide = value;
                m_Profile.WriteValue(c_ProgramID, "CanSetPierSide", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetTracking
        {
            get{return m_CanSetTracking;}
            set
            {
                m_CanSetTracking = value;
                m_Profile.WriteValue(c_ProgramID, "CanSetTracking", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSlew
        {
            get{return m_CanSlew;}
            set
            {
                m_CanSlew = value;
                m_Profile.WriteValue(c_ProgramID, "CanSlew", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSlewAltAz
        {
            get{return m_CanSlewAltAz;}
            set
            {
                m_CanSlewAltAz = value;
                m_Profile.WriteValue(c_ProgramID, "CanSlewAltAz", value.ToString(), "Capabilities");
            }
        }
        #endregion

        #region Telescope Implementation
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

        #endregion
    }
}
