using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Utilities;
using ASCOM.DriverAccess;
using System.Diagnostics;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var progId = Rotator.Choose(string.Empty);
            var driver = new Rotator(progId);


            driver.Connected = true;


            driver.Connected = false;
        }
    }
}
