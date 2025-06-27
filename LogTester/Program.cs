using ASCOM.Utilities;
using System;

namespace LogTester
{
    internal class Program
    {
        static TraceLogger TL;

        static void Main(string[] args)
        {
			TL = new TraceLogger("LogTester");
            TL.Enabled = true;
            try
			{
                Type t = Type.GetTypeFromProgID("ASCOM.Simulator.Telescope");
                dynamic device = Activator.CreateInstance(t);
                LogMessage("Device created successfully.");
                LogMessage("Device Name: " + device.Name);
            }
			catch (Exception ex)
			{
                TL.LogMessageCrLf("Exception", ex.ToString());
            }
        }

        static void LogMessage(string message)
        {
            TL.LogMessageCrLf("LogTester", message);
            Console.WriteLine(message);
        }
    }
}
