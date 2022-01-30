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


using System;
using System.Collections.Generic;
using System.Text;
using ASCOM;
using ASCOM.Utilities;

namespace ASCOM.LocalServer
{
    /// <summary>
    /// The resources shared by all drivers and devices, in this example it's a serial port with a shared SendMessage method an idea for locking the message and handling connecting is given.
    /// In reality extensive changes will probably be needed. Multiple drivers means that several applications connect to the same hardware device, aka a hub.
    /// Multiple devices means that there are more than one instance of the hardware, such as two focusers. In this case there needs to be multiple instances of the hardware connector, each with it's own connection count.
    /// </summary>
    public static class SharedResources
    {
        // Object used for locking to prevent multiple drivers accessing common code at the same time
        private static readonly object lockObject = new object();

        // Shared serial port. This will allow multiple drivers to use one single serial port.
        private static Serial sharedSerial = new Serial();      // Shared serial port
        private static int serialConnectionCount = 0;     // counter for the number of connections to the serial port

        // Public access to shared resources

        #region single serial port connector

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
            lock (lockObject)
            {
                SharedSerial.Transmit(message);
                // TODO replace this with your requirements
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

        #region Multi Driver handling

        // This section illustrates how multiple drivers could be handled, it's for drivers where multiple connections to the hardware can be made and ensures that the
        // hardware is only disconnected from when all the connected devices have disconnected.
        // It is NOT a complete solution!  This is to give ideas of what can - or should be done.
        // An alternative would be to move the hardware control here, handle connecting and disconnecting, and provide the device with a suitable connection to the hardware.

        /// <summary>
        /// Dictionary carrying device connections.
        /// </summary>
        /// <remarks>
        /// The Key is the connection number that identifies the device, it could be the COM port name, USB ID or IP Address, the Value is the DeviceHardware class
        /// </remarks>
        private static Dictionary<string, DeviceHardware> connectedDevices = new Dictionary<string, DeviceHardware>();

        /// <summary>
        /// Add the device id to the list of devices, if it's not there, and increment the device count.
        /// </summary>
        /// <remarks>
        /// This is called in the driver Connect(true) property,
        /// </remarks>
        public static void Connect(string deviceId)
        {
            lock (lockObject)
            {
                if (!connectedDevices.ContainsKey(deviceId))
                    connectedDevices.Add(deviceId, new DeviceHardware());
                connectedDevices[deviceId].Count++;       // increment the value
            }
        }

        public static void Disconnect(string deviceId)
        {
            lock (lockObject)
            {
                if (connectedDevices.ContainsKey(deviceId))
                {
                    connectedDevices[deviceId].Count--;
                    if (connectedDevices[deviceId].Count <= 0)
                        connectedDevices.Remove(deviceId);
                }
            }
        }

        public static bool IsConnected(string deviceId)
        {
            if (connectedDevices.ContainsKey(deviceId))
                return (connectedDevices[deviceId].Count > 0);
            else
                return false;
        }

        #endregion

    }

    /// <summary>
    /// Skeleton of a hardware class, all this does is hold a count of the connections, in reality extra code will be needed to handle the hardware in some way
    /// </summary>
    public class DeviceHardware
    {
        internal int Count { set; get; }

        internal DeviceHardware()
        {
            Count = 0;
        }
    }
}
