using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM;
using ASCOM.SwitchSimulator;

namespace SwitchConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //ASCOM.Utilities.Chooser chooser = new ASCOM.Utilities.Chooser();
            //chooser.DeviceType = "Switch";
            //chooser.Choose();

            Switch switchA = new Switch();
            SwitchDevice switchDeviceA = new SwitchDevice();

            Console.WriteLine("Connected: " + switchA.IsConnected);
            Console.WriteLine("Driver Name: " + switchA.DriverName);
            Console.WriteLine("Driver Description: " + switchA.Description);
            Console.WriteLine("Driver Information: " + switchA.DriverInfo);
            Console.WriteLine("Driver Version: " + switchA.DriverVersion);
            Console.WriteLine("Author Name: " + switchA.AuthorName);
            Console.WriteLine("Author Email: " + switchA.AuthorEmail);

            ArrayList a = switchA.GetSwitchDevices();
            foreach (SwitchDevice switchDevice in a)
            {
                Console.WriteLine("Switch Device Name: " + switchDevice.Name);
                Console.WriteLine("Switch Device State: " + switchDevice.On);
            }

            //switchA.SetupDialog();          
                

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();

        }
    }
}
