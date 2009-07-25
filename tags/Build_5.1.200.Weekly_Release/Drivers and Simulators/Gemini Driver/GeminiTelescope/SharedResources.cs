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
        public static string PROGRAM_ID = "ASCOM.GeminiTelescope.Telescope";  //Key used to store the settings
        public static string REGISTRATION_VERSION = "1";

        public static int GEMINI_POLLING_INTERVAL = 1;             //Seconds to use for Polling Gemini status

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
