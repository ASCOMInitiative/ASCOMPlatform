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

namespace ASCOM.TelescopeSimulator
{
    public class SharedResources
    {
        
        private static int s_z;
        private static TrafficForm m_trafficForm;               // Traffic Form 

        private SharedResources() { }							// Prevent creation of instances

        static SharedResources()								// Static initialization
        {
            
            s_z = 0;
        }

        //Constant Definitions
        public static string PROGRAM_ID = "ASCOM.TelescopeSimulator.Telescope";  //Key used to store the settings
        public static string REGISTRATION_VERSION = "1";

        // ---------------------
        // Simulation Parameters
        // ---------------------
        public static double INSTRUMENT_APERTURE = 0.2;            // 8 inch = 20 cm
        public static double INSTRUMENT_APERTURE_AREA= 0.0269;    // 3 inch obstruction
        public static double INSTRUMENT_FOCAL_LENGTH = 1.26 ;      // f/6.3 instrument
        public static string INSTRUMENT_NAME = "Simulator" ;       // Our name
        public static string INSTRUMENT_DESCRIPTION = "Software Telescope Simulator for ASCOM";
        public static double INVALID_COORDINATE = 100000;

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
    }
}
