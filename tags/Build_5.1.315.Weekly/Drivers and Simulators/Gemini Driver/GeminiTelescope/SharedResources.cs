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

namespace ASCOM.GeminiTelescope
{
    public class SharedResources
    {
        
        private static int s_z;

        //Constant Definitions
        public static string TELESCOPE_PROGRAM_ID = "ASCOM.GeminiTelescope.Telescope";  //Key used to store the settings
        public static string FOCUSER_PROGRAM_ID = "ASCOM.GeminiTelescope.Focuser";  //Key used to store the settings
        public static string REGISTRATION_VERSION = "1";

        public static int GEMINI_POLLING_INTERVAL = 1000;             //Seconds to use for Polling Gemini status

        public static string TELESCOPE_DRIVER_DESCRIPTION = "Gemini Telescope ASCOM Driver .NET";
        public static string TELESCOPE_DRIVER_NAME = "Gemini Telescope";
        public static string TELESCOPE_DRIVER_INFO = "Gemini Telescope Driver V1";

        public static string FOCUSER_DRIVER_DESCRIPTION = "Gemini Focuser ASCOM Driver .NET";
        public static string FOCUSER_DRIVER_NAME = "Gemini Focuser";
        public static string FOCUSER_DRIVER_INFO = "Gemini Focuser Driver V1";

        public static uint ERROR_BASE = 0x80040400;
        public static uint SCODE_NO_TARGET_COORDS = ERROR_BASE + 0x404;
        public static string MSG_NO_TARGET_COORDS = "Target coordinates have not yet been set";
        public static uint SCODE_VAL_OUTOFRANGE = ERROR_BASE + 0x405;
        public static string MSG_VAL_OUTOFRANGE = "The property value is out of range";
        public static uint SCOPE_PROP_NOT_SET = ERROR_BASE + 0x403;
        public static string MSG_PROP_NOT_SET = "The property has not yet been set";
        public static uint INVALID_AT_PARK = ERROR_BASE + 0x404;
        public static string MSG_INVALID_AT_PARK = "Invalid while parked";

        public static uint SCODE_NOT_IMPLEMENTED = ERROR_BASE + 0x400;
        public static string MSG_NOT_IMPLEMENTED = "Not implemented";

        public static uint SCODE_INVALID_VALUE = ERROR_BASE + 0x401;
        public static string MSG_INVALID_VALUE = "Invalid value";


        //Astronomy Releated Constants
        public static double DEG_RAD = Math.PI / 180;
        public static double RAD_DEG = 57.2957795;
        public static double HRS_RAD = 0.2617993881;
        public static double RAD_HRS = 3.81971863;
        public static double EARTH_ANG_ROT_DEG_MIN = 0.25068447733746215; //Angular rotation of earth in degrees/min

        public static double INVALID_DOUBLE = -33333333;    // invalid number, or value not set

        public static int MAXIMUM_ERRORS = 5   ;      // max communication errors tolerated within specified interval
        public static int MAXIMUM_ERROR_INTERVAL = 20000;   // in msecs. 
        public static int RECOVER_SLEEP = 3000;         // how long to sleep while waiting to recover from errors

        private SharedResources() { }							// Prevent creation of instances

        static SharedResources()								// Static initialization
        {
            
            s_z = 0;
        }

        //
        // Public access to shared resources
        //

        // Shared serial port 
        public static int z { get { return s_z++; } }
    }
}
