using System;
using SafetyMonitor = ASCOM.Simulator.SafetyMonitor;
using Switch = ASCOM.Simulator.Switch;
using Focuser = ASCOM.Simulator.Focuser;
using ASCOM.Utilities;

namespace Simulator_Testing_Project
{
    class Program
    {
        static void Main(string[] args)
        {

            var chooser = new ASCOM.Utilities.Chooser { DeviceType = "Switch" };
            chooser.Choose();
        }
    }
}
