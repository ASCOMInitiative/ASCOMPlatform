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
            Console.WriteLine("Connected to Rotator Driver");

            driver.Move(5);
            while (driver.IsMoving)
            {
                Console.WriteLine("Moving...");
            }
            Console.WriteLine("Moved 5 degrees Relative");

            //driver.Move(5);
            //while (driver.IsMoving)
            //{
            //    Console.WriteLine("Moving...");
            //}
            //Console.WriteLine("Moved 5 degrees relative");

            //driver.Move(5);
            //while (driver.IsMoving)
            //{
            //    Console.WriteLine("Moving...");
            //}
            //Console.WriteLine("Moved 5 degrees Relative");


            //Console.WriteLine("Moving to Absolute Pos 5");
            //while (driver.IsMoving)
            //{
            //    Console.WriteLine("Moving...");
            //}

            Console.WriteLine("Moving to Absolute Pos 5");
            driver.MoveAbsolute(5.9F);
            while (driver.IsMoving)
            {
                Console.WriteLine("Moving... Current Position = " + driver.Position.ToString());
                Console.WriteLine("Moving... Target Position = " + driver.TargetPosition.ToString());
            }

            Console.WriteLine("Moving to Absolute Pos 15");
            driver.MoveAbsolute(75.125F);
            while (driver.IsMoving)
            {
                Console.WriteLine("Moving... Current Position = " + driver.Position.ToString() );
                Console.WriteLine("Moving... Target Position = " + driver.TargetPosition.ToString());
            }
            Console.WriteLine("Finished Move! Current Position = " + driver.Position.ToString());


            Console.WriteLine("Moving to Absolute Pos 5");
            driver.MoveAbsolute(5.9F);
            while (driver.IsMoving)
            {
                Console.WriteLine("Moving... Current Position = " + driver.Position.ToString());
                Console.WriteLine("Moving... Target Position = " + driver.TargetPosition.ToString());
            }

            Console.WriteLine("Moving... Current Position = " + driver.Position.ToString());
            Console.WriteLine("Moving... Target Position = " + driver.TargetPosition.ToString());

            driver.Connected = false;
            Console.WriteLine("Disconnected to Rotator Driver");

            Console.ReadLine();
        }
    }
}
