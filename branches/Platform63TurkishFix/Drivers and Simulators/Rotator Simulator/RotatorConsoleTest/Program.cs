using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Simulator;

namespace RotatorConsoleTest
{
    class Program
    {
        static void Main()
        {

            var obj = new Rotator();
            //Activator.CreateInstance(Type.GetTypeFromProgID("ASCOM.Simulator.Rotator"));
            Console.WriteLine(obj.DriverInfo);
            Console.WriteLine(obj.Name);
            Console.WriteLine(obj.InterfaceVersion);
 
            
            var chooser = new ASCOM.Utilities.Chooser { DeviceType = "Rotator" };
            chooser.Choose();

        }
    }
}
