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
        private static bool m_CanSetSideOfPier;

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
                m_Profile.WriteValue(c_ProgramID, "CanSetSOP", "true", "Capabilities");
                
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
            m_CanSetSideOfPier = bool.Parse(m_Profile.GetValue(c_ProgramID, "CanSetSOP", "Capabilities"));


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
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    SharedResources.TrafficForm.TrafficLine("CanFindHome: " + m_CanFindHome);
                }
                return m_CanFindHome;
            }
            set
            {
                m_CanFindHome = value;
                m_Profile.WriteValue(c_ProgramID,  "CanFindHome", value.ToString(), "Capabilities");
            }
        }

        public static bool CanPark
        {
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanPark: " + m_CanPark);
                    }
                }
                return m_CanPark;
            }
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
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanPulseGuide: " + m_CanPulseGuide);
                    }
                }
                return m_CanPulseGuide;
            }
            set
            {
                m_CanPulseGuide = value;
                m_Profile.WriteValue(c_ProgramID, "CanPulseGuide", value.ToString(), "Capabilities");
            }
        }

        public static bool CanSetEquatorialRates
        {
            get
            {
                return m_CanSetEquatorialRates;
            }
            set
            {
                m_CanSetEquatorialRates = value;
                m_Profile.WriteValue(c_ProgramID, "CanSetEquRates", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSetGuideRates
        {
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSetGuideRates: " + m_CanSetGuideRates);
                    }
                }
                return m_CanSetGuideRates;
            }
            set
            {
                m_CanSetGuideRates = value;
                m_Profile.WriteValue(c_ProgramID, "CanSetGuideRates", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSetPark
        {
            get
            {
                if (SharedResources.TrafficForm != null)
                {
                    if (SharedResources.TrafficForm.Capabilities)
                    {
                        SharedResources.TrafficForm.TrafficLine("CanSetPark: " + m_CanSetPark);
                    }
                }
                return m_CanSetPark;
            }
            set
            {
                m_CanSetPark = value;
                m_Profile.WriteValue(c_ProgramID, "CanSetPark", value.ToString(), "Capabilities");
            }
        }
        public static bool CanSetSideOfPier
        {
            get
            {
                return m_CanSetSideOfPier;
            }
            set
            {
                m_CanSetSideOfPier = value;
                m_Profile.WriteValue(c_ProgramID, "CanSetSOP", value.ToString(), "Capabilities");
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
       public static bool CanMoveAxis(ASCOM.Interface.TelescopeAxes Axis)
        {
            if (SharedResources.TrafficForm != null)
            {
                if (SharedResources.TrafficForm.Capabilities)
                {
                    switch (Axis)
                    {
                        case ASCOM.Interface.TelescopeAxes.axisPrimary:
                            SharedResources.TrafficForm.TrafficStart("CanMoveAxis Primary: ");
                            break;
                        case ASCOM.Interface.TelescopeAxes.axisSecondary:
                            SharedResources.TrafficForm.TrafficStart("CanMoveAxis Secondary: ");
                            break;
                        case ASCOM.Interface.TelescopeAxes.axisTertiary:
                            SharedResources.TrafficForm.TrafficStart("CanMoveAxis Tertiary: ");
                            break;
                    }
                    
                }
            }
            if (m_VersionOne) 
            {
                SharedResources.TrafficForm.TrafficEnd("false");
                throw new MethodNotImplementedException("CanMoveAxis"); 
            }

            if (int.Parse(Axis.ToString()) < 0 || int.Parse(Axis.ToString()) > m_NumberMoveAxis)
            {
                SharedResources.TrafficForm.TrafficEnd("false");
                return false;
            }
            else
            {
                SharedResources.TrafficForm.TrafficEnd("true");
                return true;
            }
            
        }
       public static bool CanSetDeclinationRate
       {
           
           get 
           {
               if (m_VersionOne)
               {
                   SharedResources.TrafficForm.TrafficLine("CanSetDeclinationRate: false");
                   throw new MethodNotImplementedException("CanSetDeclinationRate");
               }
               if (SharedResources.TrafficForm != null)
               {
                   if (SharedResources.TrafficForm.Capabilities)
                   {
                       SharedResources.TrafficForm.TrafficLine("CanSetDeclinationRate: " + m_CanSetEquatorialRates);
                   }
               }
                return m_CanSetEquatorialRates; 
           }
       }

       public static bool CanSetRightAscensionRate
       {

           get
           {
               if (m_VersionOne)
               {
                   SharedResources.TrafficForm.TrafficLine("CanSetRightAscensionRate: false");
                   throw new MethodNotImplementedException("CanSetRightAscensionRate");
               }
               if (SharedResources.TrafficForm != null)
               {
                   if (SharedResources.TrafficForm.Capabilities)
                   {
                       SharedResources.TrafficForm.TrafficLine("CanSetRightAscensionRate: " + m_CanSetEquatorialRates);
                   }
               }
               return m_CanSetEquatorialRates;
           }
       }

       public static bool CanSetPierSide
       {

           get
           {
               if (m_VersionOne)
               {
                   SharedResources.TrafficForm.TrafficLine("CanSetPierSide: false");
                   throw new MethodNotImplementedException("CanSetPierSide");
               }
               if (SharedResources.TrafficForm != null)
               {
                   if (SharedResources.TrafficForm.Capabilities)
                   {
                       SharedResources.TrafficForm.TrafficLine("CanSetPierSide: " + m_CanSetSideOfPier);
                   }
               }
               return m_CanSetSideOfPier;
           }
       }
        #endregion
    }
}
