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
        const string COM_SIMULATORS_VALUE_NAME = "COMSimulators";
        const string OMNI_SIMULATORS_NAME = "OmniSimulators";
        const string OMNI_SIMULATORS_NAME_UPPERCASE = "OMNISIMULATORS";
        const string PLATFORM6_SIMULATORS_NAME = "Platform6Simulators";
        const string PLATFORM6_SIMULATORS_NAME_UPPERCASE = "PLATFORM6SIMULATORS";
        const string STATUS_KEY = @"SOFTWARE\ASCOM\Platform";
        const int CANNOT_DETERMINE_INSTALL_STATUS = 1;
        const int CANNOT_SET_STATUS = 2;

        internal static int rc = 0; // Return code

        // List of simulator COM ProgIDs and corresponding Platform 6 simulator GUIDs
        private static Dictionary<string, string> platform6Simulators = new Dictionary<string, string>()
        {
            {"ASCOM.Simulator.Camera", "{12229C31-E7D6-49E8-9C5D-5D7FF05C3BFE}"},
            {"ASCOM.Simulator.CoverCalibrator", "{096BB159-95FD-4963-809F-1F8FB6E7F833}"},
            {"ASCOM.Simulator.Dome", "{70896AE0-B6C4-4303-A945-01219BF40BB4}"},
            {"ASCOM.Simulator.FilterWheel", "{f9043c88-f6f2-101a-a3c9-08002b2f49fc}"},
            {"ASCOM.Simulator.Focuser", "{24C040F2-2FA5-4DA4-B87B-6C1101828D2A}"},
            {"ASCOM.Simulator.ObservingConditions", "{3b0ce559-c92f-46a4-bcd8-1ccec6e33f58}"},
            {"ASCOM.Simulator.Rotator", "{347b5004-3662-42c0-96b8-3f8f6f0467d2}"},
            {"ASCOM.Simulator.SafetyMonitor", "{0EF59E5C-2715-4E91-8A5E-38FE388B4F00}"},
            {"ASCOM.Simulator.Switch", "{602b2780-d8fe-438b-a11a-e45a8df6e7c8}"},
            {"ASCOM.Simulator.Telescope", "{86931eac-1f52-4918-b6aa-7e9b0ff361bd}"}
        };

        // List of simulator COM ProgIDs and corresponding Omni-Simulator GUIDs
        private static Dictionary<string, string> omniSimulators = new Dictionary<string, string>()
        {
            {"ASCOM.Simulator.Camera", "{de992041-27fc-45ca-bc58-7507994973ea}"},
            {"ASCOM.Simulator.CoverCalibrator", "{97a847f6-2522-4007-842a-ae2339b1d70d}"},
            {"ASCOM.Simulator.Dome", "{1e074ddb-d020-4045-8db0-cfbba81a6172}"},
            {"ASCOM.Simulator.FilterWheel", "{568961e4-0d98-4b9f-947e-b467c4aac5fc}"},
            {"ASCOM.Simulator.Focuser", "{a8904146-656b-4852-96cb-53c1229ff0e8}"},
            {"ASCOM.Simulator.ObservingConditions", "{38620ae3-e175-4153-8871-85ee1023c5ad}"},
            {"ASCOM.Simulator.Rotator", "{23b464ed-b86a-4276-ab2c-69ff65df9477}"},
            {"ASCOM.Simulator.SafetyMonitor", "{269f2a82-98b6-46ee-88f7-5a6c794e5d9a}"},
            {"ASCOM.Simulator.Switch", "{d0efcd2b-00d6-42b6-9f16-795cf09fbee8}"},
            {"ASCOM.Simulator.Telescope", "{124d5b35-2435-43c5-bb02-1af3edfa6dbe}"}
        };

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

            // Act depending on the supplied parameter
            switch (args[0].ToUpperInvariant())
            {
                case "PLATFORM6":
                    SetPlatform6Simulators(false);
                    return rc;

                case "OMNISIMULATORS":
                    SetOmniSimulators(false);
                    return rc;

                case "PLATFORM6-RESPECTEXISTING":
                    SetPlatform6Simulators(true);
                    return rc;

                case "OMNISIMULATORS-RESPECTEXISTING":
                    SetOmniSimulators(true);
                    return rc;

                default:
                    Console.WriteLine($"\r\nASCOM EnableSims - Unrecognised parameter value: '{args[0]}'. Valid values are: Platform6 and OmniSimulators");
                    return 99;
            }
        }

        /// <summary>
        /// Restore the Platform 6 COM ProgIDs to point at their Platform 6 values
        /// </summary>
        private static void SetPlatform6Simulators(bool respectExisting)
        {
            using (TraceLogger TL = new TraceLogger("EnableSims-Platform6"))
            {
                // Check whether we need to respect any existing setting
                if (respectExisting) // We must respect any existing setting
                {
                    // Check whether a simulator has already been selected
                    LogMessage("Checking whether a simulator type has already been set", TL);
                    if (SimulatorsHaveBeenSet(TL))  // A simulator has already been selected so respect it and don't change the current value
                    {
                        LogMessage("A simulator has already been selected, leaving that in place", TL);
                        return;
                    }
                    else
                    {
                        LogMessage("No simulator has been selected, configuring Platform 6 Simulators", TL);
                        LogMessage(" ", TL);
                    }
                }

                // Set the Platform 6 simulators as default
                LogMessage($"Setting Platform 6 Simulator ProgIDs...", TL);
                LogMessage(" ", TL);

                // Set the Platform 6 ProgID values
                foreach (KeyValuePair<string, string> simulator in platform6Simulators)
                {
                    SetProgId(simulator.Key, simulator.Value, TL);
                }

                SetSimulator(PLATFORM6_SIMULATORS_NAME, TL);

                LogMessage(" ", TL);
                LogMessage($"Platform 6 Simulator ProgIDs set.", TL);
            }
        }

        /// <summary>
        /// Hijack the Platform 6 ProgIDs and point them to the Omni Simulator devices
        /// </summary>
        private static void SetOmniSimulators(bool respectExisting)
        {
            using (TraceLogger TL = new TraceLogger("EnableSims-OmniSimulators"))
            {
                // Check whether we need to respect any existing setting
                if (respectExisting) // We must respect any existing setting
                {
                    // Check whether a simulator has already been selected
                    LogMessage("Checking whether a simulator type has already been set", TL);
                    if (SimulatorsHaveBeenSet(TL))  // A simulator has already been selected so respect it and don't change the current value
                    {
                        LogMessage("A simulator has already been selected, leaving that in place", TL);
                        return;
                    }
                    else
                    {
                        LogMessage("No simulator has been selected, configuring Omni-Simulators", TL);
                        LogMessage(" ", TL);
                    }
                }

                // Set the Omni-Simulator simulators as default
                LogMessage($"Setting Omni Simulator ProgIDs...", TL);
                LogMessage(" ", TL);

                // Set the OmniSim ProgID values
                foreach (KeyValuePair<string, string> simulator in omniSimulators)
                {
                    SetProgId(simulator.Key, simulator.Value, TL);
                }

                SetSimulator(OMNI_SIMULATORS_NAME, TL);

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

        /// <summary>
        /// Determine whether the COM simulators have already been set to either Platform 6 or the Omni-simulators
        /// </summary>
        /// <param name="TL"></param>
        /// <returns>True if either simulator has already been set, otherwise false</returns>
        private static bool SimulatorsHaveBeenSet(TraceLogger TL)
        {
            try
            {
                // Open the ASCOM Platform key where the install status is stored
                RegistryKey platformKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(STATUS_KEY);

                // Get the install status string value
                string installStatus = (string)platformKey.GetValue(COM_SIMULATORS_VALUE_NAME);

                // Check whether the value has been set
                if (installStatus != null) // A value has already been set so act on it
                {
                    switch (installStatus.ToUpperInvariant())
                    {
                        // Handle Omni-Simulators have been selected
                        case OMNI_SIMULATORS_NAME_UPPERCASE:
                            LogMessage($"The Omni-Simulators are already selected", TL);
                            return true;

                        // Handle Platform 6 Simulators have been selected
                        case PLATFORM6_SIMULATORS_NAME_UPPERCASE:
                            LogMessage($"The Platform 6 Simulators are already selected", TL);
                            return true;

                        // All other values are reported as no simulators have been selected
                        default:
                            LogMessage($"Unrecognised install status: '{installStatus}', returning false indicating that no simulators have yet been selected.", TL);
                            return false;
                    }
                }
                else // No value has been set so no simulators have been selected so return false
                {
                    LogMessage($"No install status value, returning false indicating that no simulators have yet been selected.", TL);
                    return false;
                }
            }
            catch (Exception ex)
            {
                rc = CANNOT_DETERMINE_INSTALL_STATUS;
                LogMessage($"Exception - {ex.Message}\r\n{ex}", TL);
                throw;
            }
        }

        private static void SetSimulator(string simulatorName, TraceLogger TL)
        {
            try
            {
                // Write the simulator name to the status value
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(STATUS_KEY, true).SetValue(COM_SIMULATORS_VALUE_NAME, simulatorName);
            }
            catch (Exception ex)
            {
                rc = CANNOT_SET_STATUS;
                LogMessage($"Exception - {ex.Message}\r\n{ex}", TL);
                throw;
            }
        }
    }
}
