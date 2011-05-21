using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Utilities;
using ASCOM.DriverAccess;
using System.Diagnostics;
using Optec;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var progId = Focuser.Choose(string.Empty);
                var driver = new Focuser(progId);
                var thing = driver.IsMoving;				// Just to give a place to set a breakpoint.
                driver.SetupDialog();
                while (Console.ReadLine() != "e")
                {
                    driver.Link = true;
                    driver.TempComp = true;
                    Console.WriteLine("In temp comp mode");

                    Console.WriteLine("IsMoving = " + driver.IsMoving.ToString());
                    Console.WriteLine("Moving to 3500...");
                    try{
                    driver.Move((int)3500);
                    }
                    catch{
                        Console.WriteLine("Exception thrown as expected!");
                    }
                    driver.TempComp = false;


                    Console.WriteLine("Moving to 3500...");
                    driver.Move((int)3500);
                    Console.WriteLine("IsMoving = " + driver.IsMoving.ToString());
                    Console.WriteLine("Current Position = " + driver.Position.ToString());
                    Console.WriteLine("Moving to 1000...");
                    driver.Move((int)1000);
                    Console.WriteLine("IsMoving = " + driver.IsMoving.ToString());
                    Console.WriteLine("Current Position = " + driver.Position.ToString());
                    Console.WriteLine("Moving to 3000...");
                    driver.Move((int)3000);
                    Console.WriteLine("IsMoving = " + driver.IsMoving.ToString());
                    Console.WriteLine("Current Position = " + driver.Position.ToString());
                    while (driver.IsMoving) { }
                    driver.Link = false;
                    Console.WriteLine("Current Position = " + driver.Position.ToString());
                    Console.WriteLine("IsMoving = " + driver.IsMoving.ToString());
                    Console.WriteLine("Press ESC key to exit");
                }
                
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
                Debug.Print(ex.Message);
            }
        }
    }
}
