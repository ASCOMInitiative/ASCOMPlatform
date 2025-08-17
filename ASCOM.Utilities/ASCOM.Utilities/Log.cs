using System;
using System.Collections;
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
        static string _netVersion = "Unknown";

        /// <summary>
        /// Initializes static members of the <see cref="Log"/> class.
        /// </summary>
        /// <remarks>This static constructor retrieves the .NET runtime version from the environment and
        /// assigns it to an internal field. If an error occurs during initialization, the exception is ignored.</remarks>
        static Log()
        {
            try
            {
                // Retrieve the .NET runtime version from the environment
                _netVersion = System.Environment.Version.ToString();
            }
            catch { }
        }

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
                        TL.Enabled = Global.GetBool(Global.DOTNET35_COMPONENT_USE_LOGGING, Global.DOTNET35_COMPONENT_USE_LOGGING_DEFAULT);
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

                string processName = @$"Applications\{System.Diagnostics.Process.GetCurrentProcess().ProcessName} - {System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Replace(@"\", "/")}";
                //string keyName = assembly.Location.Replace(@"\", "/");
                string keyName = assembly.GetName().Name;  //Location.Replace(@"\", "/");

                StackFrame[] frames = new StackTrace().GetFrames();
                TL.LogMessage("Component", $"Number of frames: {frames.Length}");

                string declaringMethodName = "";
                string methodName = "";
                string lastDeclaringMethodName = "";
                foreach (StackFrame frame in frames)
                {
                    frame.GetType();
                    declaringMethodName = frame.GetMethod().DeclaringType.FullName;
                    methodName = frame.GetMethod().Name;
                    TL.LogMessage("StackTrace", $"Declaring type full name: {declaringMethodName} method: {methodName}");

                    if (methodName == "CreateInstance")
                    {
                        //processName = lastDeclaringMethodName;
                        try
                        {
                            Type type = Type.GetTypeFromProgID(lastDeclaringMethodName);
                            if (type != null)
                            {
                                TL.LogMessage("StackTrace", $"Type from ProgID: {type.FullName}");
                                string location = Assembly.GetAssembly(type).Location;

                                if (location != null)
                                {
                                    if (location.ToLowerInvariant().Contains(@"mscorlib"))
                                    {
                                        location = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                                    }
                                    processName = @$"Drivers\{lastDeclaringMethodName} - {location.Replace(@"\", "/")}";
                                }
                            }
                            else
                            {
                                TL.LogMessage("StackTrace", $"Type from ProgID not found: {lastDeclaringMethodName}");
                            }
                        }
                        catch (Exception ex)
                        {
                            TL.LogMessage("StackTrace", $"Exception while getting type from ProgID: {lastDeclaringMethodName} - {ex.Message}");
                        }
                        break;
                    }

                    lastDeclaringMethodName = declaringMethodName;
                }
                TL.LogMessage("StackTrace", $"Process name is: {processName}");

                // Write the log entry to the registry
                using (RegistryAccess ra = new())
                {
                    // string key = @$"{Global.NET35_REGISTRY_BASE}\{processName}\{keyName}\{assemblyNameElements[0]} - {assemblyNameElements[1]}";
                    string key = @$"{Global.NET35_REGISTRY_BASE}\{processName}\{assemblyNameElements[0]} - {assemblyNameElements[1]}";
                    TL.LogMessage("Registry", $"Writing to registry key: {key}");
                    ra.WriteProfile(key, componentName, componentName);

                    // Add the currently running Framework CLR version
                    string frameworkKey = @$"{Global.NET35_REGISTRY_BASE}\{processName}";
                    TL.LogMessage("Registry", $"Writing CLR version to registry key: {frameworkKey}");
                    ra.WriteProfile(frameworkKey, "Framework", _netVersion);
                }
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf(componentName, $"Exception in Log.Component: {ex.Message}\r\n{ex}");
            }
        }
    }
}
