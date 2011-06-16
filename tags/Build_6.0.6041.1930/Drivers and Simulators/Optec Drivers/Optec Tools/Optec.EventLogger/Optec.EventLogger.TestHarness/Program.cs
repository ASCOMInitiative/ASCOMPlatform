using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Optec.EventLoggerTestHarness
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                EventLogger.LogMessage("An Error occurred", System.Diagnostics.TraceLevel.Error);
                
                SomeOtherAssembly.SomeOtherClass.SomeOtherMethod();

                BadMethod();
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(ex);
            }
        }

        public static void BadMethod()
        {
            string x = "this will cause an exception";
            int num = int.Parse(x);
        }

        public static void LogMessage(string message, TraceLevel type)
        {
            //EventLogger.LogMessageFromExternalAssembly(message, type, 1);
        }
    }
}


namespace SomeOtherAssembly
{
    class SomeOtherClass
    {

        public static void SomeOtherMethod()
        {
            Optec.EventLoggerTestHarness.Program.LogMessage("This is from external place", TraceLevel.Error);
        }

        
    }
}
