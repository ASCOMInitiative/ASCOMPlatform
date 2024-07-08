using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace ASCOM.Utilities
{
    internal static class SimulatorManager
    {
        const string COM_SIMULATORS_VALUE_NAME = "COMSimulators";
        const string OMNI_SIMULATORS_NAME = "OmniSimulators";
        const string OMNI_SIMULATORS_NAME_UPPERCASE = "OMNISIMULATORS";
        const string PLATFORM6_SIMULATORS_NAME = "Platform6Simulators";
        const string PLATFORM6_SIMULATORS_NAME_UPPERCASE = "PLATFORM6SIMULATORS";
        const string STATUS_KEY = @"SOFTWARE\ASCOM\Platform";

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

    // List of standard simulator names
    private static Dictionary<string, string> standardSimulatorNames = new Dictionary<string, string>()
        {
            { @"Camera Drivers\ASCOM.Simulator.Camera","ASCOM Camera Simulator"},
            { @"CoverCalibrator Drivers\ASCOM.Simulator.CoverCalibrator","ASCOM Cover Calibrator Simulator"},
            { @"Dome Drivers\ASCOM.Simulator.Dome","ASCOM Dome Simulator"},
            { @"FilterWheel Drivers\ASCOM.Simulator.FilterWheel","ASCOM Filter Wheel Simulator"},
            { @"Focuser Drivers\ASCOM.Simulator.Focuser","ASCOM Focuser Simulator"},
            { @"ObservingConditions Drivers\ASCOM.Simulator.ObservingConditions","ASCOM Observing Conditions Simulator"},
            { @"Rotator Drivers\ASCOM.Simulator.Rotator","ASCOM Rotator Simulator"},
            { @"SafetyMonitor Drivers\ASCOM.Simulator.SafetyMonitor","ASCOM Safety Monitor Simulator"},
            { @"Switch Drivers\ASCOM.Simulator.Switch","ASCOM Switch Simulator"},
            { @"Telescope Drivers\ASCOM.Simulator.Telescope","ASCOM Telescope Simulator"},
            { @"Video Drivers\ASCOM.Simulator.Video","ASCOM Video Simulator"}
        };

        // List of standard simulator names
        private static Dictionary<string, string> originalSimulatorNames = new Dictionary<string, string>()
        {
            { @"Camera Drivers\ASCOM.Simulator.Camera","Camera V3 simulator"},
            { @"CoverCalibrator Drivers\ASCOM.Simulator.CoverCalibrator","ASCOM CoverCalibrator Simulator"},
            { @"Dome Drivers\ASCOM.Simulator.Dome","Dome Simulator .NET"},
            { @"FilterWheel Drivers\ASCOM.Simulator.FilterWheel","Filter Wheel Simulator [.Net]"},
            { @"Focuser Drivers\ASCOM.Simulator.Focuser","ASCOM Simulator Focuser Driver"},
            { @"ObservingConditions Drivers\ASCOM.Simulator.ObservingConditions","ASCOM Observing Conditions Simulator"},
            { @"Rotator Drivers\ASCOM.Simulator.Rotator","Rotator Simulator .NET"},
            { @"SafetyMonitor Drivers\ASCOM.Simulator.SafetyMonitor","ASCOM Simulator SafetyMonitor Driver"},
            { @"Switch Drivers\ASCOM.Simulator.Switch","ASCOM SwitchV2 Simulator Driver"},
            { @"Telescope Drivers\ASCOM.Simulator.Telescope","Telescope Simulator for .NET"},
            { @"Video Drivers\ASCOM.Simulator.Video","Video Simulator"}
        };

        /// <summary>
        /// Restore the Platform 6 COM ProgIDs to point at their Platform 6 values
        /// </summary>
        internal static void SetPlatform6Simulators(bool respectExisting, TraceLogger TL)
        {
            LogMessage("", " ", TL);
            // Check whether we need to respect any existing setting
            if (respectExisting) // We must respect any existing setting
            {
                // Check whether a simulator has already been selected
                LogMessage("SetPlatform6Simulators", "Checking whether a simulator type has already been set", TL);
                if (HaveSimulatorsAlreadyBeenSet(TL))  // A simulator has already been selected so respect it and don't change the current value
                {
                    LogMessage("SetPlatform6Simulators", "A simulator has already been selected, leaving that in place", TL);
                    return;
                }
                else
                {
                    LogMessage("SetPlatform6Simulators", "No simulator has been selected, configuring Platform 6 Simulators", TL);
                    LogMessage("", " ", TL);
                }
            }

            // Set the Platform 6 simulators as default
            LogMessage("SetPlatform6Simulators", $"Setting Platform 6 Simulator ProgIDs...", TL);
            LogMessage("", " ", TL);

            // Set the Platform 6 ProgID values
            foreach (KeyValuePair<string, string> simulator in platform6Simulators)
            {
                SetProgId(simulator.Key, simulator.Value, TL);
            }

            SetSimulatorState(PLATFORM6_SIMULATORS_NAME, TL);

            LogMessage("", " ", TL);
            LogMessage("SetPlatform6Simulators", $"Platform 6 Simulator ProgIDs set.", TL);
            LogMessage("", " ", TL);
        }

        /// <summary>
        /// Hijack the Platform 6 ProgIDs and point them to the Omni Simulator devices
        /// </summary>
        internal static void SetOmniSimulators(bool respectExisting, TraceLogger TL)
        {
            LogMessage("", " ", TL);
            // Check whether we need to respect any existing setting
            if (respectExisting) // We must respect any existing setting
            {
                // Check whether a simulator has already been selected
                LogMessage("SetOmniSimulators", "Checking whether a simulator type has already been set", TL);
                if (HaveSimulatorsAlreadyBeenSet(TL))  // A simulator has already been selected so respect it and don't change the current value
                {
                    LogMessage("SetOmniSimulators", "A simulator has already been selected, leaving that in place", TL);
                    return;
                }
                else
                {
                    LogMessage("SetOmniSimulators", "No simulator has been selected, configuring Omni-Simulators", TL);
                    LogMessage("", " ", TL);
                }
            }

            // Set the Omni-Simulator simulators as default
            LogMessage("SetOmniSimulators", $"Setting Omni Simulator ProgIDs...", TL);
            LogMessage("", " ", TL);

            // Set the OmniSim ProgID values
            foreach (KeyValuePair<string, string> simulator in omniSimulators)
            {
                SetProgId(simulator.Key, simulator.Value, TL);
            }

            SetSimulatorState(OMNI_SIMULATORS_NAME, TL);

            LogMessage("", " ", TL);
            LogMessage("SetOmniSimulators", $"Omni Simulator ProgIDs set.", TL);
            LogMessage("", " ", TL);
        }

        internal static bool IsUsingOmniSimulators(TraceLogger TL)
        {
            try
            {
                // Assume TRUE and iterate over the well known simulator ProgIDs comparing GUIDs to the OmniSim values. Any mismatch will result in a FALSE return
                bool omnisimsAreConfigured = true;

                foreach (KeyValuePair<string, string> simulator in omniSimulators)
                {
                    string guid = (string)RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Default).OpenSubKey($"{simulator.Key}\\CLSID").GetValue(null);
                    if (string.IsNullOrEmpty(guid))
                    {
                        LogMessage("IsUsingOmniSimulators", $"CLSID is null or empty for key: {simulator.Key}\\CLSID", TL);
                        omnisimsAreConfigured = false;
                    }
                    else
                    {
                        if (guid != simulator.Value)
                        {
                            LogMessage("IsUsingOmniSimulators", $"CLSID mismatch - Actual: {guid}, Expected: {simulator.Value}", TL);
                            omnisimsAreConfigured = false;
                        }
                        else
                        {
                            LogMessage("IsUsingOmniSimulators", $"CLSIDs match OK - Actual: {guid}, Expected: {simulator.Value}", TL);
                        }
                    }
                }

                // Report outcome
                if (omnisimsAreConfigured)
                    LogMessage("IsUsingOmniSimulators", $"OmniSims are configured - returning {omnisimsAreConfigured}", TL);
                else
                    LogMessage("IsUsingOmniSimulators", $"OmniSims are NOT configured - returning {omnisimsAreConfigured}", TL);

                return omnisimsAreConfigured;
            }
            catch (Exception ex)
            {
                LogMessage("IsUsingOmniSimulators", $"Exception - {ex.Message}\r\n{ex}", TL);
                return false;
            }
        }

        internal static void SetOriginalSimulatorNames(TraceLogger TL)
        {
            foreach (KeyValuePair<string, string> simulator in originalSimulatorNames)
            {
                SetSimulatorName(simulator.Key, simulator.Value, TL);
            }

        }

        internal static void SetStandardSimulatorNames(TraceLogger TL)
        {
            foreach (KeyValuePair<string, string> simulator in standardSimulatorNames)
            {
                SetSimulatorName(simulator.Key, simulator.Value, TL);
            }
        }

        #region Support Code

        /// <summary>
        /// Set the CLSID GUID value of a given COM ProgID to a new value
        /// </summary>
        /// <param name="profileKey"></param>
        /// <param name="name"></param>
        /// <param name="TL"></param>
        private static void SetSimulatorName(string profileKey, string name, TraceLogger TL)
        {
            LogMessage("SetProgId", $"Setting simulator name for Profile key {profileKey} to {name}", TL);

            try
            {
                using (RegistryKey progIdRegistryKey = Registry.LocalMachine.CreateSubKey($@"SOFTWARE\ASCOM\{profileKey}"))
                {
                    LogMessage("SetSimulatorName", $"Created Profile registry key", TL);
                    progIdRegistryKey.SetValue(null, name);
                    LogMessage("SetSimulatorName", $"Set simulator name OK", TL);
                    LogMessage("SetSimulatorName", $"", TL);
                }
            }
            catch (Exception ex)
            {
                LogMessage("SetSimulatorName", $"Exception - {ex.Message}\r\n{ex}", TL);
                throw;
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
            LogMessage("SetProgId", $"Setting ProgID: {progId} to {classGuid}", TL);

            try
            {
                using (RegistryKey progIdRegistryKey = Registry.ClassesRoot.CreateSubKey(progId))
                {
                    LogMessage("SetProgId", $"Created ProgID registry key", TL);
                    progIdRegistryKey.SetValue(null, progId);
                    LogMessage("SetProgId", $"Set ProgID OK", TL);

                    using (RegistryKey classIdRegistryKey = progIdRegistryKey.CreateSubKey("CLSID"))
                    {
                        LogMessage("SetProgId", $"Created ClassId registry key", TL);
                        classIdRegistryKey.SetValue(null, classGuid);
                        LogMessage("SetProgId", $"Set Class GUID OK", TL);
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage("SetProgId", $"Exception - {ex.Message}\r\n{ex}", TL);
                throw;
            }
        }

        /// <summary>
        /// Log a message to the console and trace logger file
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="TL"></param>
        private static void LogMessage(string source, string message, TraceLogger TL)
        {
            Console.WriteLine($"{source} - {message}");
            TL?.LogMessageCrLf(source, message);
        }

        /// <summary>
        /// Determine whether the COM simulators have already been set to either Platform 6 or the Omni-simulators
        /// </summary>
        /// <param name="TL"></param>
        /// <returns>True if either simulator has already been set, otherwise false</returns>
        private static bool HaveSimulatorsAlreadyBeenSet(TraceLogger TL)
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
                            LogMessage("SimulatorsHaveBeenSet", $"The Omni-Simulators are already selected", TL);
                            return true;

                        // Handle Platform 6 Simulators have been selected
                        case PLATFORM6_SIMULATORS_NAME_UPPERCASE:
                            LogMessage("SimulatorsHaveBeenSet", $"The Platform 6 Simulators are already selected", TL);
                            return true;

                        // All other values are reported as no simulators have been selected
                        default:
                            LogMessage("SimulatorsHaveBeenSet", $"Unrecognised install status: '{installStatus}', returning false indicating that no simulators have yet been selected.", TL);
                            return false;
                    }
                }
                else // No value has been set so no simulators have been selected so return false
                {
                    LogMessage("SimulatorsHaveBeenSet", $"No install status value, returning false indicating that no simulators have yet been selected.", TL);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogMessage("SimulatorsHaveBeenSet", $"Exception - {ex.Message}\r\n{ex}", TL);
                throw;
            }
        }

        /// <summary>
        /// Set the summary simulator state
        /// </summary>
        /// <param name="simulatorName"></param>
        /// <param name="TL"></param>
        private static void SetSimulatorState(string simulatorName, TraceLogger TL)
        {
            try
            {
                // Write the simulator name to the status value
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(STATUS_KEY, true).SetValue(COM_SIMULATORS_VALUE_NAME, simulatorName);
            }
            catch (Exception ex)
            {
                LogMessage("SetSimulatorState", $"Exception - {ex.Message}\r\n{ex}", TL);
                throw;
            }
        }

        ///// <summary>
        ///// Swap the currently selected Platform 6 or Omni-Simulators to the other
        ///// </summary>
        ///// <param name="simulatorName"></param>
        //private static void SetSimulatorsTask(string simulatorName, TraceLogger TL)
        //{
        //    // Create a path to the SetSimulators executable
        //    string setSimulatorsPath = Path.Combine(Application.StartupPath, SET_SIMULATORS_EXE_RELATIVE_PATH);
        //    LogMessage("SetSimulator", $"Path to SetSimulators executable: '{setSimulatorsPath}', Current directory: '{Environment.CurrentDirectory}'", TL);

        //    // Check whether the executable exists
        //    if (File.Exists(setSimulatorsPath)) // SetSimulators executable exists
        //    {
        //        LogMessage("SetSimulator", $"SetSimulators exists", TL);

        //        // Create a task to run the SetSimulator executable
        //        Task swapSimTask = new Task(() =>
        //        {
        //            Process swapSimulatorProcess = new Process();
        //            swapSimulatorProcess.StartInfo.FileName = setSimulatorsPath; // Set the path to the executable
        //            swapSimulatorProcess.StartInfo.Arguments = simulatorName; // Set the parameter to be passed to the executable
        //            swapSimulatorProcess.StartInfo.UseShellExecute = false;
        //            swapSimulatorProcess.StartInfo.CreateNoWindow = true;
        //            swapSimulatorProcess.Start(); // Start the process
        //            LogMessage("SetSimulatorTask", $"Started...", TL);

        //            // Wait for the process to complete
        //            swapSimulatorProcess.WaitForExit();
        //            LogMessage("SetSimulatorTask", $"Completed", TL);
        //        });
        //        swapSimTask.Start();

        //        // Create a task that waits for a time-out period before completing
        //        Task timeoutTask = new Task(() =>
        //        {
        //            LogMessage("SetSimulatorTimeout", $"Started...", TL);

        //            // Wait for the timeout period
        //            Thread.Sleep(SET_SIMULATORS_TASK_TIMEOUT);
        //            LogMessage("SetSimulatorTimeout", $"Completed", TL);
        //        });
        //        timeoutTask.Start();

        //        // Wait for either the swap simulator task or the timeout task to complete
        //        if (Task.WhenAny(swapSimTask, timeoutTask).Result == swapSimTask) // The swapSimTask completed
        //        {
        //            LogMessage("SetSimulator", $"Simulators set to {simulatorName} OK.", TL);
        //        }
        //        else // The timeout task completed first
        //        {
        //            LogMessage("SetSimulator", $"SetSimulators Task timed out!", TL);
        //            MessageBox.Show($"The SetSimulators task timed out and the new simulators were not enabled.", "SetSimulator Issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else // SetSimulators executable can not be found
        //    {
        //        LogMessage("SetSimulator", $"SetSimulators DOES NOT exist", TL);
        //        MessageBox.Show($"Unable to find the SetSimulators executable at expected location: {setSimulatorsPath}", "SetSimulator Issue", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        #endregion

    }
}
