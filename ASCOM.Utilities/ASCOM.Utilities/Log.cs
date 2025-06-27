using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ASCOM.Utilities
{
    /// <summary>
    /// Provides methods for logging information about application components.
    /// </summary>
    /// <remarks>The <see cref="Utilities.Log"/> class is a utility for logging messages related to specific components of an application.
    /// It is designed to standardize log entries by including a time-stamp and a component name.
    /// </remarks>
    public static class Log
    {
        static TraceLogger TL;
        static bool? _enabled = null;

        /// <summary>
        /// Logs the specified component name.
        /// </summary>
        /// <remarks>The log entry includes the component name. Ensure that the <paramref name="componentName"/> parameter is meaningful to help identify the
        /// source of the log.</remarks>
        /// <param name="assembly">The calling assembly</param>
        /// <param name="componentName">The name of the component to include in the log entry. Cannot be null or empty.</param>
        public static void Component(Assembly assembly, string componentName)
        {
            // Check whether logging is enabled or disabled
            switch (_enabled)
            {
                case null: // First time use, determine whether logging is required and create logger if required.
                    lock (typeof(Log))
                    {
                        // Get the logging enabled/disabled state from the registry
                        _enabled = true;

                        // Exit if logging is disabled
                        if (!_enabled.Value)
                            return;

                        //Logging is enabled so create a new TraceLogger instance using an internal constructor that avoids the infinite loop created if TraceLogger logged its own use.
                        TL = new TraceLogger("Net35use", true);
                        TL.Enabled = true;
                    }
                    break;

                case false: // Second or later use, logging is disabled so exit early
                    return;

                case true: // Second or later use, logging is enabled, use the existing TraceLogger instance. No action required, just continue
                    break;
            }

            try
            {
                string fullAssemblyName = assembly.FullName;

                // Validate input parameters
                if (string.IsNullOrEmpty(fullAssemblyName))
                {
                    TL.LogMessage("Component", $"Assembly name cannot be null or empty - {fullAssemblyName}");
                    return;
                }

                if (string.IsNullOrEmpty(componentName))
                {
                    TL.LogMessage("Component", $"Component name cannot be null or empty - {componentName}");
                    return;
                }

                StackFrame[] frames = new StackTrace().GetFrames();
                TL.LogMessage("Component", $"Number of frames: {frames.Length}");

                foreach (StackFrame frame in frames)
                {
                    frame.GetType();
                    TL.LogMessage("Component", $"Calling Type: {frame.GetType().FullName}, Declaring type full name: {frame.GetMethod().DeclaringType.FullName} method: {frame.GetMethod().Name}");
                }

                // Split the assembly name and extract version information, removing extraneous spaces and "Version=" prefix
                string[] assemblyNameElements = fullAssemblyName.Split(',').Select(part => part.Trim()).Select(part2 => part2.Replace("Version=", "")).ToArray();
                if (assemblyNameElements.Length < 2)
                {
                    TL.LogMessage("Component", $"Unable to parse assembly name format - {fullAssemblyName}");
                    return;
                }

                // Ensure that the assembly name and version are not null or empty
                string assemblyName = assemblyNameElements[0] is not null ? assemblyNameElements[0] : "Unknown assembly name";
                string assemblyVersion = assemblyNameElements[1] is not null ? assemblyNameElements[1] : "Unknown Version";

                TL.LogMessage("Component", $"Assembly: {assemblyName}, Version: {assemblyVersion} - {componentName}");
                TL.LogMessage("Component", $"Friendly name: {System.AppDomain.CurrentDomain.FriendlyName}");
                TL.LogMessage("Component", $"Process name: {System.Diagnostics.Process.GetCurrentProcess().ProcessName}");
                TL.LogMessage("Component", $"File name: {System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName}");

                //string keyName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Replace(@"\", "/");
                string keyName = assembly.Location.Replace(@"\", "/");

                // Write the log entry to the registry
                using (RegistryAccess ra = new())
                {
                    ra.WriteProfile(@$"{Global.NET35_REGISTRY_BASE}\{keyName}\{assemblyNameElements[0]}\{assemblyNameElements[1]}", componentName, componentName);
                }
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf(componentName, $"Exception in Log.Component: {ex.Message}\r\n{ex}");
            }
        }
    }
}
