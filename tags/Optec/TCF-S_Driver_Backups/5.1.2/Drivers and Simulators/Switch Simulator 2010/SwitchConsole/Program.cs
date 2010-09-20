using System;
using System.Collections;
using System.Linq;
using ASCOM.Simulator;



namespace SwitchConsole
{
    class Program
    {
        static void Main()
        {

            var obj = new Switch();
            // Activator.CreateInstance(Type.GetTypeFromProgID("ASCOM.Simulator.Switch")));
            var list = obj.Switches;
            foreach (string[,] swithItems in list.Cast<string[,]>().Where(swithItems => swithItems != null))
            {
                Console.WriteLine(swithItems[0,0] + @" = " + swithItems[0,1]);
            }
            Console.WriteLine(obj.DriverInfo);
            Console.WriteLine(obj.Name);
            Console.WriteLine(obj.InterfaceVersion);
            Console.WriteLine(obj.LastResult);

           
            var chooser = new ASCOM.Utilities.Chooser {DeviceType = "Switch"};
            chooser.Choose();

        }
    }
}
 