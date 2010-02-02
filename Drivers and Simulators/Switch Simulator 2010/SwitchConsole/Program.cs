using System;
using System.Collections;
using ASCOM.SwitchSimulator;


namespace SwitchConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            Switch obj = (Switch)Activator.CreateInstance(Type.GetTypeFromProgID("ASCOM.SwitchSimulator.Switch"));

            //ArrayList list = obj.SwitchDevices;
            
            //string test = obj.DriverInfo;

            //obj.SetupDialog();

            //ASCOM.Utilities.Chooser chooser = new ASCOM.Utilities.Chooser();
            //chooser.DeviceType = "Switch";
            //chooser.Choose();

            //ASCOM.SwitchSimulator.Switch aSwitch = new Switch();
            //aSwitch.SetupDialog();
        }
    }
}
 