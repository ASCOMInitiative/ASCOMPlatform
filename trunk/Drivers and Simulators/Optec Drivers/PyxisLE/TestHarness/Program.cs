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
			var thing = driver.CanReverse;					// Just to give a place to set a breakpoint.
			//driver.SetupDialog();
            driver.Connected = true;
            for (int i = 0; i < 1000; i++)
            {
                Trace.WriteLine("Number " + i.ToString());
                driver.MoveAbsolute(0);
                driver.MoveAbsolute(30);
                driver.MoveAbsolute(60);
                driver.MoveAbsolute(330);
                driver.MoveAbsolute(300);
                driver.MoveAbsolute(330);
                driver.MoveAbsolute(30);
               // System.Threading.Thread.Sleep(1);
                

            }
			Console.WriteLine("Press any key to exit");
			Console.ReadLine();
		}
	}
}
