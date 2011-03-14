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

namespace ASCOM.LocalServerName
{
    /// <summary>
    /// The resources shared by all drivers and devices, in this example it's a serial port with a shared SendMessage method
    /// an idea for locking the message and handling connecting is given.
    /// In reality extensive changes will probably be needed.
    /// </summary>
	public static class SharedResources
	{
// Shared serial port
        private static ASCOM.Utilities.Serial s_sharedSerial = new ASCOM.Utilities.Serial();
		private static int s_z = 0;

        private static readonly object lockObject = new object();

		//
		// Public access to shared resources
		//

		// Shared serial port 
        public static ASCOM.Utilities.Serial SharedSerial { get { return s_sharedSerial; } }
        /// <summary>
        /// number of connections
        /// </summary>
		public static int connections { get { return s_z++; } }

        /// <summary>
        /// Example of a shared sendmessage method, the lock
        /// prevents different drivers to trip over one another.
        /// It needs error handling
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SendMessage(string message)
        {
            lock (lockObject)
            {
                SharedSerial.Transmit(message);
                // TODO replace this with your requirements
                return SharedSerial.ReceiveTerminated("#");
            }
        }
         
        /// <summary>
        /// Example of handling connecting to and disconnection from the
        /// shared serial port.
        /// Needs error handling
        /// </summary>
        public static bool Connected
        {
            set
            {
                lock (lockObject)
                {
                    if (SharedSerial.Connected == value)
                        return;
                    if (value)
                    {
                        if (s_z == 0)
                            SharedSerial.Connected = true;
                        s_z++;
                    }
                    else
                    {
                        s_z--;
                        if (s_z <= 0)
                        {
                            SharedSerial.Connected = false;
                        }
                    }
                }
            }
            get { return SharedSerial.Connected; }
        }
	}
}
