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
using System.IO.Ports;
using ASCOM.OptecFocuserHubTools;

namespace ASCOM.OptecFocuserHub
{
    public class SharedResources
    {
        private static FocuserManager myFocuserManager;

        private static int s_z;

        private SharedResources() { }							// Prevent creation of instances

        static SharedResources()								// Static initialization
        {
            myFocuserManager = FocuserManager.GetInstance();
            s_z = 0;
            //Focusers.
        }

        //
        // Public access to shared resources
        //

        // Shared serial port 
        public static FocuserManager SharedFocuserManager { get { return myFocuserManager; } }
        
        public static int z { get { return s_z++; } }
    }
}
