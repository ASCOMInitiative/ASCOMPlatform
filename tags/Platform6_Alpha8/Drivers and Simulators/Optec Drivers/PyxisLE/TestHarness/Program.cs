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
            try
            {
                var progId = Rotator.Choose(string.Empty);
                var driver = new Rotator(progId);
                var thing = driver.CanReverse;					// Just to give a place to set a breakpoint.
                //driver.SetupDialog();
                driver.Connected = true;
                for (int i = 0; i < 10000; i++)
                {
                    driver.Move((float).05);
                   // System.Threading.Thread.Sleep(250);
                    double j = (double)i/(double)20;
                    Console.WriteLine(j.ToString() + " degrees");




               }
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
            }
		}
	}
}
