//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Focuser driver for Gemini
//
// Description:	Gemini controlled focuser
//
// Implements:	ASCOM Focuser interface version: 1.0
// Author:		(rbt) Robert Turner <robert@robertturnerastro.com>
//              (pk)  Paul Kanevsky <pk.darkhorizons.org>
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 15-JUL-2009	rbt	1.0.0	Initial edit, from ASCOM Focuser Driver template
// 30-JUL-2009  pk  1.0.1   Basic driver and setup implementation
// --------------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.HelperNET;
using ASCOM.Interface;
using ASCOM.GeminiTelescope;

namespace ASCOM.GeminiFocuser
{
    //
    // Your driver's ID is ASCOM.Focuser.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.Focuser.Focuser
    // The ClassInterface/None addribute prevents an empty interface called
    // _Focuser from being created and used as the [default] interface
    //
    [Guid("3a22c443-4e46-4504-8cef-731095e51e1f")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : ReferenceCountedObjectBase, IFocuser
    {

        bool m_Connected = false;
        private static ASCOM.HelperNET.Profile m_Profile;
        private static ASCOM.HelperNET.Util m_Util;

        int m_MaxIncrement = 0;
        int m_MaxStep = 0;
        int m_StepSize = 0;
        bool m_ReverseDirection = false;
        int m_BacklashDirection = 0;
        int m_BacklashSize = 0;
        int m_BrakeSize = 0;

        int m_PreviousMove = 0;

        int m_Speed = 1;

        int m_Position = 0;

        Timer tmrFocus = new Timer();
        
        enum FocuserState {
            Backlash,
            Focusing,
            Braking,
            None
        };
        
        FocuserState m_State = FocuserState.None;

        //
        // Constructor - Must be public for COM registration!
        //
        public Focuser()
        {
            m_Profile = new HelperNET.Profile();
            m_Util = new ASCOM.HelperNET.Util();
            tmrFocus.Tick += new ASCOM.HelperNET.Interfaces.ITimer.TickEventHandler(tmrFocus_Tick);
        }

        /// <summary>
        /// Executed on a timer when waiting to complete a focuser move
        /// </summary>
        void tmrFocus_Tick()
        {
            tmrFocus.Enabled = false;
            int val = m_PreviousMove;

            // if we were taking up backlash prior to this,
            // move on to actual focusing command:
            if (m_State == FocuserState.Backlash)
            {
                m_State = FocuserState.Focusing;

                if ((val > 0 && !m_ReverseDirection) || (val < 0 && m_ReverseDirection))
                    GeminiHardware.DoCommand(":F+");
                else
                    GeminiHardware.DoCommand(":F-");
                tmrFocus.Interval = (m_StepSize * Math.Abs(val)) / 1000;
                tmrFocus.Enabled = true;
                return;
            }
                // if we are done with focusing, check if braking is enabled
                // and execute a break maneuver (move in the opposite direction for a bit)
            else if (m_State == FocuserState.Focusing)
            {
                if (m_BrakeSize > 0)
                {
                    // move in the opposite direction for specified step*interval
                    if ((val > 0 && !m_ReverseDirection) || (val < 0 && m_ReverseDirection))
                        GeminiHardware.DoCommand(":F-");
                    else
                        GeminiHardware.DoCommand(":F+");

                    tmrFocus.Interval = (m_StepSize * m_BrakeSize) / 1000;
                    tmrFocus.Enabled = true;
                    return;
                }
            }

            // at this point, we're done focusing!
            Halt();
            m_Position += m_PreviousMove * m_StepSize;  // new position 
        }


        //
        // PUBLIC COM INTERFACE IFocuser IMPLEMENTATION
        //

        #region IFocuser Members

        /// <summary>
        /// Gemini doesn't support absolute focusers, but we'll fake it by keeping track of current position
        /// based on executed commands
        /// </summary>
        public bool Absolute
        {
            get { return true; }
        }

        /// <summary>
        /// Stop focusing
        /// </summary>
        public void Halt()
        {
            tmrFocus.Enabled = false;
            m_State = FocuserState.None;

            GeminiHardware.DoCommand(":FQ");
        }

        /// <summary>
        /// Is the focuser moving?
        /// </summary>
        public bool IsMoving
        {
            get { return m_Connected && m_State!=FocuserState.None;  }
        }

        /// <summary>
        /// Get/Set connection property
        /// </summary>
        public bool Link
        {
            get { 
                return GeminiHardware.Connected && m_Connected;  
            }
            set {
                if (value && !m_Connected) 
                {
                    GeminiHardware.Connected = true;
                    if (!GeminiHardware.Connected)
                        throw new DriverException("Cannot connect to Gemini Focuser", -1);
                    else
                    {
                        GetProfileSettings();
                        m_State  = FocuserState.None;

                        // set the desired focuser speed:
                        string sCmd = ":FS";
                        if (m_Speed == 2) sCmd = ":FM";
                        else
                            if (m_Speed == 3) sCmd = ":FF";
                        GeminiHardware.DoCommand(sCmd);

                        m_Connected = true;
                    }
                }
                else
                    if (m_Connected) {
                        tmrFocus.Enabled = false;
                        GeminiHardware.Connected = false;
                        m_Connected = false;
                        m_State = FocuserState.None;
                    }                    
            }
        }

        /// <summary>
        ///  Reloads all the variables from the profile
        /// </summary>
        private void GetProfileSettings()
        {
            m_Profile.Register(FocuserResources.PROGRAM_ID, "Gemini Focuser Driver .NET");
            if (m_Profile.GetValue(SharedResources.PROGRAM_ID, "RegVer", "") != SharedResources.REGISTRATION_VERSION)
            {
                //Main Driver Settings
                m_Profile.WriteValue(FocuserResources.PROGRAM_ID, "RegVer", SharedResources.REGISTRATION_VERSION);
            }

            string s = m_Profile.GetValue(FocuserResources.PROGRAM_ID, "MaxIncrement");

            s = m_Profile.GetValue(FocuserResources.PROGRAM_ID, "MaxStep");
            if (!int.TryParse(s, out m_MaxStep) || m_MaxStep <= 0)
                m_MaxStep = 5000;

            s = m_Profile.GetValue(FocuserResources.PROGRAM_ID, "StepSize");
            if (!int.TryParse(s, out m_StepSize) || m_StepSize <= 0)
                m_StepSize = 100;

            s = m_Profile.GetValue(FocuserResources.PROGRAM_ID, "ReverseDirection");
            if (!bool.TryParse(s, out m_ReverseDirection))
                m_ReverseDirection = false;

            s = m_Profile.GetValue(FocuserResources.PROGRAM_ID, "BacklashDirection");
            if (!int.TryParse(s, out m_BacklashDirection) || m_BacklashDirection<-1 || m_BacklashDirection > 1)
                m_BacklashDirection = 0;

            s = m_Profile.GetValue(FocuserResources.PROGRAM_ID, "BacklashSize");
            if (!int.TryParse(s, out m_BacklashSize) || m_BacklashSize < 0)
                m_BacklashSize = 0;

            s = m_Profile.GetValue(FocuserResources.PROGRAM_ID, "BrakeSize");
            if (!int.TryParse(s, out m_BrakeSize) || m_BrakeSize < 0)
                m_BrakeSize = 0;

            s = m_Profile.GetValue(FocuserResources.PROGRAM_ID, "FocuserSpeed");
            if (!int.TryParse(s, out m_Speed) || m_Speed < 1 || m_Speed > 3)
                m_Speed = 1;
        }

        /// <summary>
        /// Maximum focuser increment
        /// </summary>
        public int MaxIncrement
        {
            get { return m_MaxIncrement; }
        }

        /// <summary>
        /// Maximum focuser step
        /// </summary>
        public int MaxStep
        {
            get { return m_MaxStep; }
        }

        /// <summary>
        /// Move focuser by a number of steps
        /// </summary>
        /// <param name="val">val is the number of steps, + or -, + means out, - means in</param>        
        public void Move(int val)
        {
            if (m_State != FocuserState.None) Halt();


            val = val*m_StepSize - m_Position; // how far to move from current position
            val /= m_StepSize;

            // limit the move to max increment setting
            if (Math.Abs(val) > m_MaxIncrement)
                val = m_MaxIncrement * Math.Sign(val);

            if (m_BacklashDirection != 0 && Math.Sign(m_BacklashDirection) == Math.Sign(val))
            {
                m_State = FocuserState.Backlash;
                m_PreviousMove = val;

                if ((val > 0 && !m_ReverseDirection) || (val < 0 && m_ReverseDirection))
                    GeminiHardware.DoCommand(":F+");
                else
                    GeminiHardware.DoCommand(":F-");

                tmrFocus.Interval = (m_StepSize * m_BacklashSize) / 1000;
                tmrFocus.Enabled = true;
            }
            else
            {
                m_State = FocuserState.Focusing;
                m_PreviousMove = val;
                if ((val > 0 && !m_ReverseDirection) || (val < 0 && m_ReverseDirection))
                    GeminiHardware.DoCommand(":F+");
                else
                    GeminiHardware.DoCommand(":F-");

                tmrFocus.Interval = (m_StepSize * Math.Abs(val)) / 1000;
                tmrFocus.Enabled = true;               
            }
 
        }

        /// <summary>
        /// Current focuser position
        /// </summary>
        public int Position
        {
            get { return m_Position; }
        }

        /// <summary>
        /// Bring up setup dialog
        /// </summary>
        public void SetupDialog()
        {
            if (GeminiHardware.Connected)
            {
                try
                {
                    GeminiTelescope.GeminiTelescope.m_MainForm.DoFocuserSetupDialog();
                }
                catch { }
            }
        }

        /// <summary>
        /// step size for the focuser, in micro-seconds
        /// </summary>
        public double StepSize
        {
            get { return m_StepSize;  }
        }

        /// <summary>
        /// Temp compensation is not supported by Gemini
        /// </summary>
        public bool TempComp
        {
            get { return false; }
            set { throw new DriverException("TempComp is not supported", -1); }
        }

        /// <summary>
        /// Temp compensation is not available
        /// </summary>
        public bool TempCompAvailable
        {
            get { return false;  }
        }

        /// <summary>
        /// Not supported by Gemini
        /// </summary>
        public double Temperature
        {
            get { throw new DriverException("Temperature is not supported", -1); }
        }

        #endregion
    }


    /// <summary>
    /// Private resources for the Focuser driver
    /// </summary>
    public class FocuserResources
    {

        //Constant Definitions
        public static string PROGRAM_ID = "ASCOM.GeminiTelescope.Focuser";  //Key used to store the settings
        public static string REGISTRATION_VERSION = "1";

        public static int GEMINI_POLLING_INTERVAL = 1;             //Seconds to use for Polling Gemini status

        public static string TELESCOPE_DRIVER_DESCRIPTION = "Gemini Focuser ASCOM Driver .NET";
        public static string TELESCOPE_DRIVER_NAME = "Gemini Focuser";
        public static string TELESCOPE_DRIVER_INFO = "Gemini Focuser Driver V1";

        public static uint ERROR_BASE = 0x80040400;
    }


}
