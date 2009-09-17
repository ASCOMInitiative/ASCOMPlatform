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

namespace ASCOM.FocuserSimulator
{
    public class SharedResources
    {
        private static ASCOM.HelperNET.Serial m_SharedSerial;		// Shared serial port
        private static int s_z;

        private SharedResources() { }							// Prevent creation of instances

        static SharedResources()								// Static initialization
        {
            m_SharedSerial = new ASCOM.HelperNET.Serial();
            s_z = 0;
        }

        //
        // Public access to shared resources
        //

        // Shared serial port 
        public static ASCOM.HelperNET.Serial SharedSerial { get { return m_SharedSerial; } }
        public static int z { get { return s_z++; } }
    }
}
