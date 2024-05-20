using ASCOM.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EnableSims
{
    internal class Program
    {

        internal static int rc = 0; // Return code

        /// <summary>
        /// Enable the Platform 6 simulator COM ProgIDs to start either the Platform 6 devices or the OmniSimulators
        /// </summary>
        /// <param name="args">Parameter (Platform6 or OmniSimulators) specifying whether to enable the Platform 6 or Omni Simulator simulators.</param>
        static int Main(string[] args)
        {

            // Check whether the required parameter was provided
            if (args.Length == 0) // No parameters so present some help text and end
            {
                Console.WriteLine($"\r\nEnableSims - Switches the Platform 6 simulator COM ProgIDs to run either the Platform 6 simulators or the Omni Simulators");
                Console.WriteLine($"");
                Console.WriteLine($"Requires a single, case insensitive, parameter.");
                Console.WriteLine($"");
                Console.WriteLine($"Valid parameter values:");
                Console.WriteLine("    Platform6");
                Console.WriteLine("    OmniSimulators");
                return 0;
            }

            // If we get here there must be at least one argument so act on it

            switch (args[0].ToUpperInvariant())
            {
                case "PLATFORM6":
                    RestorePlatform6Simulators();
                    return rc;

                case "OMNISIMULATORS":
                    SetOmniSimulators();
                    return rc;

                default:
                    Console.WriteLine($"\r\nASCOM EnableSims - Unrecognised parameter value: '{args[0]}'. Valid values are: Platform6 and OmniSimulators");
                    return 99;
            }
        }

        /// <summary>
        /// Restore the Platform 6 COM ProgIDs to point at their Platform 6 values
        /// </summary>
        private static void RestorePlatform6Simulators()
        {
            using (TraceLogger TL = new TraceLogger("EnableSims-Platform6"))
            {
                LogMessage($"Setting Platform 6 Simulator ProgIDs...", TL);
                LogMessage(" ", TL);

                // Set the Platform 6 ProgID values
                SetProgId("ASCOM.Simulator.Camera", "{12229C31-E7D6-49E8-9C5D-5D7FF05C3BFE}", TL);
                SetProgId("ASCOM.Simulator.CoverCalibrator", "{096BB159-95FD-4963-809F-1F8FB6E7F833}", TL);
                SetProgId("ASCOM.Simulator.Dome", "{70896AE0-B6C4-4303-A945-01219BF40BB4}", TL);
                SetProgId("ASCOM.Simulator.FilterWheel", "{f9043c88-f6f2-101a-a3c9-08002b2f49fc}", TL);
                SetProgId("ASCOM.Simulator.Focuser", "{24C040F2-2FA5-4DA4-B87B-6C1101828D2A}", TL);
                SetProgId("ASCOM.Simulator.ObservingConditions", "{3b0ce559-c92f-46a4-bcd8-1ccec6e33f58}", TL);
                SetProgId("ASCOM.Simulator.Rotator", "{347b5004-3662-42c0-96b8-3f8f6f0467d2}", TL);
                SetProgId("ASCOM.Simulator.SafetyMonitor", "{0EF59E5C-2715-4E91-8A5E-38FE388B4F00}", TL);
                SetProgId("ASCOM.Simulator.Switch", "{602b2780-d8fe-438b-a11a-e45a8df6e7c8}", TL);
                SetProgId("ASCOM.Simulator.Telescope", "{86931eac-1f52-4918-b6aa-7e9b0ff361bd}", TL);

                LogMessage(" ", TL);
                LogMessage($"Platform 6 Simulator ProgIDs set.", TL);
            }
        }

        /// <summary>
        /// Hijack the Platform 6 ProgIDs and point them to the Omni Simulator devices
        /// </summary>
        private static void SetOmniSimulators()
        {
            using (TraceLogger TL = new TraceLogger("EnableSims-OmniSimulators"))
            {
                LogMessage($"Setting Omni Simulator ProgIDs...", TL);
                LogMessage(" ", TL);

                // Set the OmniSim ProgID values
                SetProgId("ASCOM.Simulator.Camera", "{de992041-27fc-45ca-bc58-7507994973ea}", TL);
                SetProgId("ASCOM.Simulator.CoverCalibrator", "{97a847f6-2522-4007-842a-ae2339b1d70d}", TL);
                SetProgId("ASCOM.Simulator.Dome", "{1e074ddb-d020-4045-8db0-cfbba81a6172}", TL);
                SetProgId("ASCOM.Simulator.FilterWheel", "{568961e4-0d98-4b9f-947e-b467c4aac5fc}", TL);
                SetProgId("ASCOM.Simulator.Focuser", "{a8904146-656b-4852-96cb-53c1229ff0e8}", TL);
                SetProgId("ASCOM.Simulator.ObservingConditions", "{38620ae3-e175-4153-8871-85ee1023c5ad}", TL);
                SetProgId("ASCOM.Simulator.Rotator", "{23b464ed-b86a-4276-ab2c-69ff65df9477}", TL);
                SetProgId("ASCOM.Simulator.SafetyMonitor", "{269f2a82-98b6-46ee-88f7-5a6c794e5d9a}", TL);
                SetProgId("ASCOM.Simulator.Switch", "{d0efcd2b-00d6-42b6-9f16-795cf09fbee8}", TL);
                SetProgId("ASCOM.Simulator.Telescope", "{124d5b35-2435-43c5-bb02-1af3edfa6dbe}", TL);

                LogMessage(" ", TL);
                LogMessage($"Omni Simulator ProgIDs set.", TL);
            }
        }

        /// <summary>
        /// Set the CLSID GUID value of a given COM ProgID to a new value
        /// </summary>
        /// <param name="progId"></param>
        /// <param name="classGuid"></param>
        /// <param name="TL"></param>
        private static void SetProgId(string progId, string classGuid, TraceLogger TL)
        {
            LogMessage($"Setting ProgID: {progId} to {classGuid}", TL);

            try
            {
                using (RegistryKey progIdRegistryKey = Registry.ClassesRoot.CreateSubKey(progId))
                {
                    LogMessage($"Created ProgID registry key", TL);
                    progIdRegistryKey.SetValue(null, progId);
                    LogMessage($"Set ProgID OK", TL);

                    using (RegistryKey classIdRegistryKey = progIdRegistryKey.CreateSubKey("CLSID"))
                    {
                        LogMessage($"Created ClassId registry key", TL);
                        classIdRegistryKey.SetValue(null, classGuid);
                        LogMessage($"Set Class GUID OK", TL);
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Exception - {ex.Message}\r\n{ex}", TL);
                rc = ex.HResult;
            }
        }

        /// <summary>
        /// Log a message to the console and trace logger file
        /// </summary>
        /// <param name="message"></param>
        /// <param name="TL"></param>
        private static void LogMessage(string message, TraceLogger TL)
        {
            Console.WriteLine($"EnableSims - {message}");
            TL.LogMessageCrLf("EnableSims", message);
        }
    }
}
