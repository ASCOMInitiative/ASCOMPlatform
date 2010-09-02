using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Utilities;
using ASCOM.DriverAccess;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var progId = Focuser.Choose(string.Empty);
            var driver = new Focuser(progId);
            var thing = driver.IsMoving;				// Just to give a place to set a breakpoint.
            driver.SetupDialog();
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
