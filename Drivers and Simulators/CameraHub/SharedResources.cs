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

using ASCOM;
using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ASCOM.LocalServer
{
    /// <summary>
    /// Add and manage resources that are shared by all drivers served by this local server here.
    /// In this example it's a serial port with a shared SendMessage method an idea for locking the message and handling connecting is given.
    /// In reality extensive changes will probably be needed. 
    /// Multiple drivers means that several drivers connect to the same hardware device, aka a hub.
    /// Multiple devices means that there are more than one instance of the hardware, such as two focusers. In this case there needs to be multiple instances
    /// of the hardware connector, each with it's own connection count.
    /// </summary>
    [HardwareClass]
    public static class SharedResources
    {
        // Object used for locking to prevent multiple drivers accessing common code at the same time
        private static readonly object lockObject = new object();

        // Shared serial port. This will allow multiple drivers to use one single serial port.
        private static Serial sharedSerial = new Serial();      // Shared serial port
        private static int serialConnectionCount = 0;     // counter for the number of connections to the serial port

        // Public access to shared resources

        #region Dispose method to clean up resources before close
        /// <summary>
        /// Deterministically release both managed and unmanaged resources that are used by this class.
        /// </summary>
        /// <remarks>
        /// TODO: Release any managed or unmanaged resources that are used in this class.
        /// 
        /// Do not call this method from the CameraHardware.Dispose() method in your hardware class.
        ///
        /// This is because this shared resources class is decorated with the <see cref="HardwareClassAttribute"/> attribute and this Dispose() method will be called 
        /// automatically by the local server executable when it is irretrievably shutting down. This gives you the opportunity to release managed and unmanaged resources
        /// in a timely fashion and avoid any time delay between local server close down and garbage collection by the .NET runtime.
        ///
        /// </remarks>
        public static void Dispose()
        {
            try
            {
                if (sharedSerial != null)
                {
                    sharedSerial.Dispose();
                }
            }
            catch
            {
            }

        }
        #endregion

        #region Single serial port connector

        // This region shows a way that a single serial port could be connected to by multiple drivers.
        // Connected is used to handle the connections to the port.
        // SendMessage is a way that messages could be sent to the hardware without conflicts between different drivers.
        //
        // All this is for a single connection, multiple connections would need multiple ports and a way to handle connecting and disconnection from them - see the multi driver handling section for ideas.

        /// <summary>
        /// Shared serial port
        /// </summary>
        public static Serial SharedSerial
        {
            get
            {
                return sharedSerial;
            }
        }

        /// <summary>
        /// Number of connections to the shared serial port
        /// </summary>
        public static int Connections
        {
            get
            {
                return serialConnectionCount;
            }

            set
            {
                serialConnectionCount = value;
            }
        }

        /// <summary>
        /// Example of a shared SendMessage method
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <remarks>
        /// The lock prevents different drivers tripping over one another. It needs error handling and assumes that the message will be sent unchanged and that the reply will always be terminated by a "#" character.
        /// </remarks>
        public static string SendMessage(string message)
        {
            // TODO update this with your requirements
            lock (lockObject)
            {
                SharedSerial.Transmit(message);
                return SharedSerial.ReceiveTerminated("#");
            }
        }

        /// <summary>
        /// Example of handling connecting to and disconnection from the shared serial port.
        /// </summary>
        /// <remarks>
        /// Needs error handling, the port name etc. needs to be set up first, this could be done by the driver checking Connected and if it's false setting up the port before setting connected to true.
        /// It could also be put here.
        /// </remarks>
        public static bool Connected
        {
            set
            {
                lock (lockObject)
                {
                    if (value)
                    {
                        if (serialConnectionCount == 0)
                        {
                            SharedSerial.Connected = true;
                        }
                        serialConnectionCount++;
                    }
                    else
                    {
                        serialConnectionCount--;
                        if (serialConnectionCount <= 0)
                        {
                            SharedSerial.Connected = false;
                        }
                    }
                }
            }
            get { return SharedSerial.Connected; }
        }

        #endregion

    }

}
