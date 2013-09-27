// This implements a console application that can be used to test an ASCOM driver
//

// This is used to define code in the template that is specific to one class implementation
// unused code can be deleted and this definition removed.

#define Switch
// remove this to bypass the code that uses the chooser to select the driver
#define UseChooser

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.DriverAccess;

namespace ASCOM
{
    class Program
    {
        static void Main(string[] args)
        {
            // Uncomment the code that's required
#if UseChooser
            // choose the device
            string id = Switch.Choose("");
            if (string.IsNullOrEmpty(id))
                return;
            // create this device
            ASCOM.DriverAccess.Switch device = new Switch(id);
#else
            // this can be replaced by this code, it avoids the chooser and creates the driver class directly.
            ASCOM.DriverAccess.Switch device = new ASCOM.DriverAccess.Switch("ASCOM.Simulator.Switch");
#endif
            // now run some tests, adding code to your driver so that the tests will pass.
            // these first tests are common to all drivers.
            Console.WriteLine("name " + device.Name);
            Console.WriteLine("description " + device.Description);
            Console.WriteLine("DriverInfo " + device.DriverInfo);
            Console.WriteLine("driverVersion " + device.DriverVersion);

            device.SetupDialog();

            // TODO add more code to test the driver.
            device.Connected = true;

            Console.WriteLine("Number " + device.MaxSwitch);
            for (short i = 0; i < device.MaxSwitch; i++)
            {
                Console.WriteLine("Id {0}, Name {1}, Min {2}, max {3}, step {4}, value {5}, CanWrite {6}",
                    i, device.GetSwitchName(i),
                    device.MinSwitchValue(i), device.MaxSwitchValue(i),
                    device.SwitchStep(i), device.GetSwitchValue(i),
                    device.CanWrite(i));
            }

            device.Connected = false;
            Console.WriteLine("Press Enter to finish");
            Console.ReadLine();
        }
    }
}
