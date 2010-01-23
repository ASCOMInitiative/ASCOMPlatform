using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM;

namespace SwitchConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ASCOM.Utilities.Chooser chooser = new ASCOM.Utilities.Chooser();
            chooser.DeviceType = "Switch";
            chooser.Choose();
                
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();

        }
    }
}
