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

            ASCOM.SwitchSimulator.Switch aSwitch = new Switch();
            aSwitch.SetupDialog();
            
  

        }
    }
}
