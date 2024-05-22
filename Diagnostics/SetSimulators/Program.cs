using ASCOM.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SetSimulators
{
    internal class Program
    {
        const int UNRECOGNISED_PARAMETER_ERROR = 1;
        const int CANNOT_SET_SIMULATORS_ERROR = 2;

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
                Console.WriteLine("    Platform6                      - Enables the Platform 6 simulators.");
                Console.WriteLine("    OmniSimulators                 - Enables the Omni-Simulators.");
                Console.WriteLine("    Platform6-RespectExisting      - Enables the Platform 6 simulators on first use but otherwise leaves the current setting.");
                Console.WriteLine("    OmniSimulators-RespectExisting - Enables the Omni-Simulators on first use but otherwise leaves the current setting.");
                return 0;
            }

            // If we get here there must be at least one argument so act on it

            using (TraceLogger TL = new TraceLogger("SetSimulators"))
            {
                try
                {
                    // Act depending on the supplied parameter
                    switch (args[0].ToUpperInvariant())
                    {
                        case "PLATFORM6":
                            SimulatorManager.SetPlatform6Simulators(false, TL);
                            return 0;

                        case "OMNISIMULATORS":
                            SimulatorManager.SetOmniSimulators(false, TL);
                            return 0;

                        case "PLATFORM6-RESPECTEXISTING":
                            SimulatorManager.SetPlatform6Simulators(true, TL);
                            return 0;

                        case "OMNISIMULATORS-RESPECTEXISTING":
                            SimulatorManager.SetOmniSimulators(true, TL);
                            return 0;

                        default:
                            Console.WriteLine($"\r\nASCOM EnableSims - Unrecognised parameter value: '{args[0]}'. Valid values are: Platform6 and OmniSimulators");
                            return UNRECOGNISED_PARAMETER_ERROR;
                    }
                }
                catch (Exception ex)
                {
                    TL.LogMessageCrLf("Exception", $"{ex.Message}\r\n{ex}");
                    return CANNOT_SET_SIMULATORS_ERROR;
                }
            }
        }
    }
}
