// This implements a console application that can be used to test an ASCOM driver
//

// This is used to define code in the template that is specific to one class implementation
// unused code can be deleted and this definition removed.
#define TEMPLATEDEVICECLASS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEMPLATEDEVICENAME
{
    class Program
    {
        static void Main(string[] args)
        {
            // Uncomment the code that's required
            // choose the device
            string id = ASCOM.DriverAccess.Dome.Choose("");
            if (string.IsNullOrEmpty(id))
                return;
            // create this device
            ASCOM.DriverAccess.Dome device = new ASCOM.DriverAccess.Dome(id);

            // this can be replaced by this code, it avoids the chooser and creates the driver class directly.
            //ASCOM.DriverAccess.Telescope device = new ASCOM.DriverAccess.Telescope("ASCOM.TEMPLATEDEVICENAME.TEMPLATEDEVICECLASS");


            // now run some tests, adding code to your driver so that the tests will pass.
            // these first tests are common to all drivers.
            Console.WriteLine("name " + device.Name);
            Console.WriteLine("description " + device.Description);
            Console.WriteLine("DriverInfo " + device.DriverInfo);
            Console.WriteLine("driverVersion " + device.DriverVersion);

            // TODO add more code to test the driver.


            Console.WriteLine("Press Enter to finish");
            Console.ReadLine();
        }
    }
}
