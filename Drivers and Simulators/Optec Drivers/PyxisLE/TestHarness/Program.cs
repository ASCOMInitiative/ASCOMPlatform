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
                    Trace.WriteLine("Number " + i.ToString());
                    Console.WriteLine("Moving to position 0 at " + DateTime.Now.ToLongTimeString());
                    driver.MoveAbsolute(10);
                    Console.WriteLine("Moving to position 30 at " + DateTime.Now.ToLongTimeString());
                    driver.MoveAbsolute(30);
                    Console.WriteLine("Moving to position 60 at " + DateTime.Now.ToLongTimeString());
                    driver.MoveAbsolute(60);
                    Console.WriteLine("Moving to position 330 at " + DateTime.Now.ToLongTimeString());
                    driver.MoveAbsolute(330);
                    Console.WriteLine("Moving to position 300 at " + DateTime.Now.ToLongTimeString());
                    driver.MoveAbsolute(300);
                    Console.WriteLine("Moving to position 330 at " + DateTime.Now.ToLongTimeString());
                    driver.MoveAbsolute(330);
                    Console.WriteLine("Moving to position 30 at " + DateTime.Now.ToLongTimeString());
                    driver.MoveAbsolute(30);
                    Console.WriteLine("Reached position 30 at " + DateTime.Now.ToLongTimeString());
                    // System.Threading.Thread.Sleep(1);


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
