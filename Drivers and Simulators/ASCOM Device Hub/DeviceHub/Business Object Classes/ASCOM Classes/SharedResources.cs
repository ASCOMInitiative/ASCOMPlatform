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
// Modified by Chris Rowland and Peter Simpson to hamdle multiple hardware devices March 2011
//
using System;
using System.Collections.Generic;
using System.Text;
using ASCOM;

namespace ASCOM.DeviceHub
{
	/// <summary>
	/// The resources shared by all drivers and devices, in this example it's a serial port with a shared SendMessage method
	/// an idea for locking the message and handling connecting is given.
	/// In reality extensive changes will probably be needed.
	/// Multiple drivers means that several applications connect to the same hardware device, aka a hub.
	/// Multiple devices means that there are more than one instance of the hardware, such as two focusers.
	/// In this case there needs to be multiple instances of the hardware connector, each with it's own connection count.
	/// </summary>
	public static class SharedResources
	{
		// object used for locking to prevent multiple drivers accessing common code at the same time
		private static readonly object _lockObject = new object();

		// Shared serial port. This will allow multiple drivers to use one single serial port.
		//private static ASCOM.Utilities.Serial s_sharedSerial = new ASCOM.Utilities.Serial();        // Shared serial port
		//private static int s_z = 0;     // counter for the number of connections to the serial port

		//
		// Public access to shared resources
		//

		#region Single serial port connector
		//
		// this region shows a way that a single serial port could be connected to by multiple 
		// drivers.
		//
		// Connected is used to handle the connections to the port.
		//
		// SendMessage is a way that messages could be sent to the hardware without
		// conflicts between different drivers.
		//
		// All this is for a single connection, multiple connections would need multiple ports
		// and a way to handle connecting and disconnection from them - see the
		// multi driver handling section for ideas.
		//

		/// <summary>
		/// Shared serial port
		/// </summary>
		//public static ASCOM.Utilities.Serial SharedSerial { get { return s_sharedSerial; } }

		/// <summary>
		/// number of connections to the shared serial port
		/// </summary>
		//public static int connections { get { return s_z; } set { s_z = value; } }

		/// <summary>
		/// Example of a shared SendMessage method, the lock
		/// prevents different drivers tripping over one another.
		/// It needs error handling and assumes that the message will be sent unchanged
		/// and that the reply will always be terminated by a "#" character.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		//public static string SendMessage( string message )
		//{
		//	lock ( _lockObject )
		//	{
		//		SharedSerial.Transmit( message );
		//		// TODO replace this with your requirements
		//		return SharedSerial.ReceiveTerminated( "#" );
		//	}
		//}

		/// <summary>
		/// Example of handling connecting to and disconnection from the
		/// shared serial port.
		/// Needs error handling
		/// the port name etc. needs to be set up first, this could be done by the driver
		/// checking Connected and if it's false setting up the port before setting connected to true.
		/// It could also be put here.
		/// </summary>
		//public static bool Connected
		//{
		//	set
		//	{
		//		lock ( _lockObject )
		//		{
		//			if ( value )
		//			{
		//				if ( s_z == 0 )
		//				{
		//					SharedSerial.Connected = true;
		//				}
		//				s_z++;
		//			}
		//			else
		//			{
		//				s_z--;
		//				if ( s_z <= 0 )
		//				{
		//					SharedSerial.Connected = false;
		//				}
		//			}
		//		}
		//	}
		//	get { return SharedSerial.Connected; }
		//}

		#endregion Single serial port connector

		#region Multi Driver handling
		// this section illustrates how multiple drivers could be handled,
		// it's for drivers where multiple connections to the hardware can be made and ensures that the
		// hardware is only disconnected from when all the connected devices have disconnected.

		// It is NOT a complete solution!  This is to give ideas of what can - or should be done.
		//
		// An alternative would be to move the hardware control here, handle connecting and disconnecting,
		// and provide the device with a suitable connection to the hardware.
		//
		/// <summary>
		/// dictionary carrying device connections.
		/// The Key is the connection number that identifies the device, it could be the COM port name,
		/// USB ID or IP Address, the Value is the DeviceHardware class
		/// </summary>
		//private static Dictionary<string, DeviceHardware> connectedDevices = new Dictionary<string, DeviceHardware>();

		/// <summary>
		/// This is called in the driver Connect(true) property,
		/// it add the device id to the list of devices if it's not there and increments the device count.
		/// </summary>
		/// <param name="deviceId"></param>
		//public static void Connect( string deviceId )
		//{
		//	lock ( _lockObject )
		//	{
		//		if ( !connectedDevices.ContainsKey( deviceId ) )
		//			connectedDevices.Add( deviceId, new DeviceHardware() );
		//		connectedDevices[deviceId].count++;       // increment the value
		//	}
		//}

		//public static void Disconnect( string deviceId )
		//{
		//	lock ( _lockObject )
		//	{
		//		if ( connectedDevices.ContainsKey( deviceId ) )
		//		{
		//			connectedDevices[deviceId].count--;
		//			if ( connectedDevices[deviceId].count <= 0 )
		//				connectedDevices.Remove( deviceId );
		//		}
		//	}
		//}

		//public static bool IsConnected( string deviceId )
		//{
		//	if ( connectedDevices.ContainsKey( deviceId ) )
		//		return ( connectedDevices[deviceId].count > 0 );
		//	else
		//		return false;
		//}

		#endregion Multi Driver handling
	}

	/// <summary>
	/// Skeleton of a hardware class, all this does is hold a count of the connections,
	/// in reality extra code will be needed to handle the hardware in some way
	/// </summary>
	//public class DeviceHardware
	//{
	//	internal int Count { set; get; }

	//	internal DeviceHardware()
	//	{
	//		Count = 0;
	//	}
	//}

	//#region ServedClassName attribute
	///// <summary>
	///// This is only needed if the driver is targeted at  platform 5.5, it is included with Platform 6
	///// </summary>
	//[global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	//public sealed class ServedClassNameAttribute : Attribute
	//{
	//    // See the attribute guidelines at 
	//    //  http://go.microsoft.com/fwlink/?LinkId=85236

	//    /// <summary>
	//    /// Gets or sets the 'friendly name' of the served class, as registered with the ASCOM Chooser.
	//    /// </summary>
	//    /// <value>The 'friendly name' of the served class.</value>
	//    public string DisplayName { get; private set; }
	//    /// <summary>
	//    /// Initializes a new instance of the <see cref="ServedClassNameAttribute"/> class.
	//    /// </summary>
	//    /// <param name="servedClassName">The 'friendly name' of the served class.</param>
	//    public ServedClassNameAttribute(string servedClassName)
	//    {
	//        DisplayName = servedClassName;
	//    }
	//}
	//#endregion ServedClassName attribute
}
