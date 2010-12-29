//
// ================
// Shared Resources
// ================
//
// This class is a container for all shared resources that may be needed
// by the drivers served by the Local Server. 
//
// NOTES:
//
//	* ALL DECLARATIONS MUST BE STATIC HERE!! INSTANCES OF THIS CLASS MUST NEVER BE CREATED!
//
// Written by:	Bob Denny	29-May-2007
//
using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.Simulator
{
    public enum SlewType
    {
        SlewNone,
        SlewSettle,
        SlewMoveAxis,
        SlewRaDec,
        SlewAltAz,
        SlewPark,
        SlewHome,
        SlewHandpad
    }
    public enum SlewSpeed
    {
        SlewSlow,
        SlewMedium,
        SlewFast
    }
    public enum SlewDirection
    {
        SlewNorth,
        SlewSouth,
        SlewEast,
        SlewWest,
        SlewUp,
        SlewDown,
        SlewLeft,
        SlewRight
    }

    public static class SharedResources
    {
        
        private static int s_z = 0;
        private static TrafficForm m_trafficForm;               // Traffic Form 

        //private SharedResources() { }							// Prevent creation of instances

        //static SharedResources()								// Static initialization
        //{
        //}

        //Constant Definitions
        public const string PROGRAM_ID = "ASCOM.Simulator.Telescope";  //Key used to store the settings
        public const string REGISTRATION_VERSION = "1";

        //public static double DEG_RAD = 0.0174532925;
        public const double DEG_RAD = Math.PI / 180;
        public const double RAD_DEG =  180.0 / Math.PI;        //57.2957795;
        public const double HRS_RAD = 0.2617993881;
        public const double RAD_HRS = 3.81971863;
        public const double EARTH_ANG_ROT_DEG_MIN = 0.25068447733746215; //Angular rotation of earth in degrees/min

        public const double SIDRATE = 0.9972695677;

        public const double TIMER_INTERVAL = .25; //4 ticks per second

        // ---------------------
        // Simulation Parameters
        // ---------------------
        public const double INSTRUMENT_APERTURE = 0.2;            // 8 inch = 20 cm
        public const double INSTRUMENT_APERTURE_AREA= 0.0269;    // 3 inch obstruction
        public const double INSTRUMENT_FOCAL_LENGTH = 1.26 ;      // f/6.3 instrument
        public const string INSTRUMENT_NAME = "Simulator" ;       // Our name
        public const string INSTRUMENT_DESCRIPTION = "Software Telescope Simulator for ASCOM";
        public const double INVALID_COORDINATE = 100000;

        public const uint ERROR_BASE = 0x80040400;
        public static readonly uint SCODE_NO_TARGET_COORDS = (uint)ASCOM.ErrorCodes.InvalidOperationException; //ERROR_BASE + 0x404;
        public const string MSG_NO_TARGET_COORDS = "Target coordinates have not yet been set";
        public static uint SCODE_VAL_OUTOFRANGE = (uint)ASCOM.ErrorCodes.InvalidValue;// ERROR_BASE + 0x405;
        public const string MSG_VAL_OUTOFRANGE = "The property value is out of range";
        public static uint SCOPE_PROP_NOT_SET = (uint)ASCOM.ErrorCodes.ValueNotSet;// ERROR_BASE + 0x403;
        public const string MSG_PROP_NOT_SET = "The property has not yet been set";
        public static uint INVALID_AT_PARK = (uint)ASCOM.ErrorCodes.InvalidWhileParked; //ERROR_BASE + 0x404;
        public const string MSG_INVALID_AT_PARK = "Invalid while parked";
        //
        // Public access to shared resources
        //

        public static int z { get { return s_z++; } }

        public static TrafficForm TrafficForm { 
            get 
            {
                if (m_trafficForm == null)
                {
                    m_trafficForm = new TrafficForm();
                }
                
                return m_trafficForm; 
            }
            set
            {
                m_trafficForm = value;
            }
        }

        /// <summary>
        /// specifies the type of the traffic message, the message
        /// is only displayed if the corresponding checkbox is checked.
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// Capabilities: Can Flags, Alignment Mode
            /// </summary>
            Capabilities,
            /// <summary>
            /// Slew, Sync, Park/Unpark, Find Home
            /// </summary>
            Slew,
            /// <summary>
            /// Get: Alt/Az, RA/Dec, Target RA/Dec
            /// </summary>
            Gets,
            /// <summary>
            /// Polls: Tracking, Slewing, At's
            /// </summary>
            Polls,
            /// <summary>
            /// UTC, Siderial Time
            /// </summary>
            Time,
            /// <summary>
            /// All Other messages
            /// </summary>
            Other
        }

        /// <summary>
        /// Write a line to the traffis form
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="msg"></param>
        public static void TrafficLine(MessageType msgType, string msg)
        {
            if (CheckMsg(msgType))
            {
                m_trafficForm.TrafficLine(msg);
            }
        }

        /// <summary>
        /// Start a line to the traffic form, must be finished by TrafficLine or TrafficEnd
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="msg"></param>
        public static void TrafficStart(MessageType msgType, string msg)
        {
            if (CheckMsg(msgType))
            {
                m_trafficForm.TrafficStart(msg);
            }
        }

        /// <summary>
        /// Finish writing a line to the traffic form
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="msg"></param>
        public static void TrafficEnd(MessageType msgType, string msg)
        {
            if (CheckMsg(msgType))
            {
                m_trafficForm.TrafficEnd(msg);
            }
        }

        /// <summary>
        /// Returns true if the message type has been checked
        /// </summary>
        /// <param name="msgType"></param>
        /// <returns></returns>
        private static bool CheckMsg(MessageType msgType)
        {
            if (TrafficForm == null)
                return false;
            switch (msgType)
            {
                case MessageType.Capabilities:
                    return m_trafficForm.Capabilities;
                case MessageType.Slew:
                    return m_trafficForm.Slew;
                case MessageType.Gets:
                    return m_trafficForm.Gets;
                case MessageType.Polls:
                    return m_trafficForm.Polls;
                case MessageType.Time:
                    return m_trafficForm.Time;
                case MessageType.Other:
                    return m_trafficForm.Other;
                default:
                    return false;
            }
        }
    }
}
