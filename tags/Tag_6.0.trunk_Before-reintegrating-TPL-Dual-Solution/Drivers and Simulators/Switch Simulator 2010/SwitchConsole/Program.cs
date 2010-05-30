using System;
using System.Collections;
using ASCOM.Simulator;



namespace SwitchConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            //Switch obj = (Switch)Activator.CreateInstance(Type.GetTypeFromProgID("ASCOM.Simulator.Switch"));
            //ArrayList list = obj.SwitchDevices;
            //foreach (ASCOM.Simulator.ISwitchDevice item in list)
            //{
            //    Console.WriteLine("Item name: " + item.Name + " & " + item.State);
            //}
            //string test = obj.DriverInfo;

           
            ASCOM.Utilities.Chooser chooser = new ASCOM.Utilities.Chooser();
            chooser.DeviceType = "Switch";
            chooser.Choose();

        }
    }
}
 