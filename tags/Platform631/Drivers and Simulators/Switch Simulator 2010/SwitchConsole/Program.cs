using System;
using SafetyMonitor = ASCOM.Simulator.SafetyMonitor;
using Switch = ASCOM.Simulator.Switch;
using ASCOM.Utilities;


namespace SwitchConsole
{
    class Program
    {
        static void Main()
        {

            //var obj = new SafetyMonitor();
            // //Activator.CreateInstance(Type.GetTypeFromProgID("ASCOM.Simulator.Switch")));
            //var list = obj.Switches;
            //foreach (string[,] swithItems in list.Cast<string[,]>().Where(swithItems => swithItems != null))
            //{
            //    Console.WriteLine(swithItems[0, 0] + @" = " + swithItems[0, 1]);
            //}
            //Console.WriteLine(obj.DriverInfo);
            //Console.WriteLine(obj.Name);
            //Console.WriteLine(obj.InterfaceVersion);
            //Console.WriteLine(obj.LastResult);


            //var p = new Profile();
            //p.Register();

            var chooser = new ASCOM.Utilities.Chooser {DeviceType = "Switch"};
            chooser.Choose();


        }
    }
}
 