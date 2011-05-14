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
                driver.Move(100);
                
                Console.WriteLine("IsMoving = " + driver.IsMoving.ToString());
                while (driver.IsMoving)
                {
                    Console.WriteLine("IsMoving = " + driver.IsMoving.ToString());
                    Console.WriteLine("TargetPosition = " + driver.TargetPosition.ToString());
                    Console.WriteLine("CurrentPosition = " + driver.Position.ToString());
                    System.Threading.Thread.Sleep(100);
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
