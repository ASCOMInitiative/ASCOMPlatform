// This is a console application that can be used to test an ASCOM driver

// Remove the "#define UseChooser" line to bypass the code that uses the chooser to select the driver and replace it with code that accesses the driver directly via its ProgId.
#define UseChooser

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM
{
    class Program
    {
        static void Main(string[] args)
        {
#if UseChooser
            // Choose the device
            string id = ASCOM.DriverAccess.TEMPLATEDEVICECLASS.Choose("");

            // Exit if no device was selected
            if (string.IsNullOrEmpty(id))
                return;

            // Create this device
            ASCOM.DriverAccess.TEMPLATEDEVICECLASS device = new ASCOM.DriverAccess.TEMPLATEDEVICECLASS(id);
#else
            // Create the driver class directly.
            ASCOM.DriverAccess.TEMPLATEDEVICECLASS device = new ASCOM.DriverAccess.TEMPLATEDEVICECLASS("ASCOM.TEMPLATEDEVICENAME.TEMPLATEDEVICECLASS");
#endif

            // Connect to the device
            device.Connected = true;

            // Now exercise some calls that are common to all drivers.
            Console.WriteLine($"Name: {device.Name}" );
            Console.WriteLine($"Description: {device.Description}");
            Console.WriteLine($"DriverInfo: {device.DriverInfo}");
            Console.WriteLine($"DriverVersion: {device.DriverVersion}");
            Console.WriteLine($"InterfaceVersion: {device.InterfaceVersion}");

            //
            // TODO add more code to test your driver.
            //

            // Disconnect from the device
            device.Connected = false;

            Console.WriteLine("Press Enter to finish");
            Console.ReadLine();
        }
    }
}
