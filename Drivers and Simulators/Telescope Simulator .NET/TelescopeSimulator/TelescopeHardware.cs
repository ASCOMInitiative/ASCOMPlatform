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

        private static bool m_CanFindHome;
        private static bool m_CanPark;
        private static bool m_VersionOne;
        private static int m_NumberMoveAxis;


        static TelescopeHardware()
        {
            m_Profile = new HelperNET.Profile();
            m_Timer.Elapsed += new ElapsedEventHandler(TimerEvent);
            m_Timer.Start();

            //Load up the values from saved
            m_VersionOne = bool.Parse(m_Profile.GetValue(c_ProgramID, "V1", "Capabilities"));
            m_CanFindHome = bool.Parse(m_Profile.GetValue(c_ProgramID, "CanFindHome", "Capabilities"));
            m_CanPark = bool.Parse(m_Profile.GetValue(c_ProgramID, "CanPark", "Capabilities"));
            m_NumberMoveAxis = int.Parse(m_Profile.GetValue(c_ProgramID, "NumMoveAxis", "Capabilities"));



        }

        //Update the Telescope Based on Timed Events
        private static void TimerEvent(object source, ElapsedEventArgs e)
        {

        }

        #region Properties For Settings
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
                m_Profile.WriteValue(c_ProgramID, "CanFindHome", "Capabilities");
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
                m_Profile.WriteValue(c_ProgramID, "CanPark", "Capabilities");
            }
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


        #endregion

    }
}
