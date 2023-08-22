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
        SlewRight,
        SlewNone
    }

    public static class SharedResources
    {
        private static TrafficForm m_trafficForm;               // Traffic Form 

        //Constant Definitions
        public const string PROGRAM_ID = "ASCOM.Simulator.Telescope";  //Key used to store the settings
        public const string REGISTRATION_VERSION = "1";

        public const double STATE_UPDATE_TIMER_INTERVAL = 0.1; // 10 ticks per second
        public const double HANDBOX_UPDATE_TIMER_INTERVAL = 0.25; // 4 ticks per second

        // ---------------------
        // Simulation Parameters
        // ---------------------
        internal const double INSTRUMENT_APERTURE = 0.2;            // 8 inch = 20 cm
        internal const double INSTRUMENT_APERTURE_AREA = 0.0269;    // 3 inch obstruction
        internal const double INSTRUMENT_FOCAL_LENGTH = 1.26;      // f/6.3 instrument
        public const string INSTRUMENT_NAME = "Simulator";       // Our name
        public const string INSTRUMENT_DESCRIPTION = "Software Telescope Simulator for ASCOM";

        internal const uint ERROR_BASE = 0x80040400;
        internal static readonly uint SCODE_NO_TARGET_COORDS = (uint)ASCOM.ErrorCodes.InvalidOperationException; //ERROR_BASE + 0x404;
        internal const string MSG_NO_TARGET_COORDS = "Target coordinates have not yet been set";
        internal static uint SCODE_VAL_OUTOFRANGE = (uint)ASCOM.ErrorCodes.InvalidValue;// ERROR_BASE + 0x405;
        internal const string MSG_VAL_OUTOFRANGE = "The property value is out of range";
        internal static uint SCOPE_PROP_NOT_SET = (uint)ASCOM.ErrorCodes.ValueNotSet;// ERROR_BASE + 0x403;
        internal const string MSG_PROP_NOT_SET = "The property has not yet been set";
        internal static uint INVALID_AT_PARK = (uint)ASCOM.ErrorCodes.InvalidWhileParked; //ERROR_BASE + 0x404;
        internal const string MSG_INVALID_AT_PARK = "Invalid while parked";
        //
        // Public access to shared resources
        //

        //public static int z { get { return s_z++; } }

        public static TrafficForm TrafficForm
        {
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
            none,
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
            lastMsg = msgType;
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
            lastMsg = msgType;
            if (CheckMsg(msgType))
            {
                m_trafficForm.TrafficStart(msg);
            }
        }

        public static void TrafficStart(string msg)
        {
            if (CheckMsg(lastMsg))
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
            lastMsg = msgType;
        }

        public static void TrafficEnd(string msg)
        {
            if (CheckMsg(lastMsg))
            {
                m_trafficForm.TrafficEnd(msg);
            }
            lastMsg = MessageType.none;
        }

        private static MessageType lastMsg = MessageType.none;

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

        /// <summary>
        /// Extension method to format a double in sesegesimal HH:MM:SS.sss format
        /// </summary>
        /// <param name="value">Value to be presented</param>
        /// <returns>HH:MM:SS.sss formatted string</returns>
        public static string ToHMS(this double value)
        {
            return DoubleToSexagesimalSeconds(value, ":", ":", "", 3);
        }

        /// <summary>
        /// Extension method to format a double in sesegesimal DD:MM:SS.sss format
        /// </summary>
        /// <param name="value">Value to be presented</param>
        /// <returns>DDD:MM:SS.sss formatted string</returns>
        public static string ToDMS(this double value)
        {
            return DoubleToSexagesimalSeconds(value, ":", ":", "", 3);
        }

        private static string DoubleToSexagesimalSeconds(double Units, string DegDelim, string MinDelim, string SecDelim, int SecDecimalDigits)
        {
            string wholeUnits, wholeMinutes, seconds, secondsFormatString;
            bool inputIsNegative;

            // Deal with NaN and infinity values by returning string representations of these states.
            if (double.IsNaN(Units) | double.IsInfinity(Units))
                return Units.ToString();

            // Convert the input value to a positive number if required and store the sign
            if (Units < 0.0)
            {
                Units = -Units;
                inputIsNegative = true;
            }
            else inputIsNegative = false;

            // Extract the number of whole units and save the remainder
            wholeUnits = Math.Floor(Units).ToString().PadLeft(2, '0');
            double remainderInMinutes = (Units - Convert.ToDouble(wholeUnits)) * 60.0; // Remainder in minutes

            // Extract the number of whole minutes and save the remainder
            wholeMinutes = Math.Floor(remainderInMinutes).ToString().PadLeft(2, '0');// Integral minutes
            double remainderInSeconds = (remainderInMinutes - System.Convert.ToDouble(wholeMinutes)) * 60.0; // Remainder in seconds

            if (SecDecimalDigits == 0) secondsFormatString = "00"; // No decimal point or decimal digits
            else secondsFormatString = "00." + new String('0', SecDecimalDigits); // Format$ string of form 00.0000

            seconds = remainderInSeconds.ToString(secondsFormatString); // Format seconds with requested number of decimal digits

            // Check to see whether rounding has pushed the number of whole seconds or minutes to 60
            if (seconds.Substring(0, 2) == "60") // The rounded seconds value is 60 so we need to add one to the minutes count and make the seconds 0
            {
                seconds = 0.0.ToString(secondsFormatString); // Seconds are 0.0 formatted as required
                wholeMinutes = (Convert.ToInt32(wholeMinutes) + 1).ToString("00"); // Add one to the to the minutes count
                if (wholeMinutes == "60")// The minutes value is 60 so we need to add one to the units count and make the minutes 0
                {
                    wholeMinutes = "00"; // Minutes are 0.0
                    wholeUnits = (Convert.ToInt32(wholeUnits) + 1).ToString("00"); // Add one to the units count
                }
            }

            // Create the full formatted string from the units, minute and seconds parts and add a leading negative sign if required
            string returnValue = wholeUnits + DegDelim + wholeMinutes + MinDelim + seconds + SecDelim;
            if (inputIsNegative) returnValue = $"-{returnValue}";

            return returnValue;
        }



    }
}
