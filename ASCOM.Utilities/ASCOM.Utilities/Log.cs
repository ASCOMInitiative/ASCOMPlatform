using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;

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
        private static TraceLogger logger = null;
        private static bool componentUseLoggingEnabled = false;
        private static string dotNetRuntimeVersion = "Unknown";

        /// <summary>
        /// Initializes static members of the <see cref="Log"/> class.
        /// </summary>
        /// <remarks>This static constructor retrieves the .NET runtime version from the environment and
        /// assigns it to an internal field. If an error occurs during initialization, the exception is ignored.</remarks>
        static Log()
        {
            // Attempt to get the .NET runtime version from the environment
            try
            {
                // Retrieve the .NET runtime version from the environment
                dotNetRuntimeVersion = $"{System.Environment.Version.ToString(3)}";
                if (dotNetRuntimeVersion is null)
                    dotNetRuntimeVersion = "Unknown";
            }
            catch { }

            // Attempt to get the .NET runtime version of the primary process from the entry assembly
            try
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();

                // Exit if the entry assembly returns null (can happen if the class is called from "unmanaged code" e.g. the .NET COM wrapper code.)
                if (entryAssembly is null)
                    dotNetRuntimeVersion = "Unmanaged code";
                else
                {
                    // Attempt to get the .NET runtime version from the entry assembly
                    try
                    {
                        string imageRuntimetVersion = $"{entryAssembly.ImageRuntimeVersion.Trim('v')}";
                        if (!string.IsNullOrEmpty(imageRuntimetVersion))
                            dotNetRuntimeVersion = imageRuntimetVersion;
                    }
                    catch { } // Ignore errors and just use the version retrieved from the environment, which is better than nothing.
                }
            }
            catch { }

            // Get the logging enabled/disabled state from the registry
            try
            {
                componentUseLoggingEnabled = Global.GetBool(Global.DOTNET35_COMPONENT_USE_LOGGING, Global.DOTNET35_COMPONENT_USE_LOGGING_DEFAULT);
            }
            catch { }

            // Enable logging and set up logging to file if configured to do so
            try
            {
                // Check whether we have to create a log file because Utilities trace is enabled
                if (componentUseLoggingEnabled) // Framework logging is enabled
                {
                    // Check whether Utilities logging is enabled
                    if (Global.GetBool(Global.TRACE_UTIL, Global.TRACE_UTIL_DEFAULT) == true) // Utilities logging is enabled, so create one.
                    {
                        try
                        {
                            // Create a new, enabled, TraceLogger instance using an internal constructor that avoids the infinite loop created if TraceLogger logged its own use.
                            logger = new TraceLogger("Net35use", true) { Enabled = true };
                            LogMessage("Initialiser", $"TraceLogger created OK. .NET runtime version: {dotNetRuntimeVersion}");
                            LogMessage("", $"");
                        }
                        catch (Exception ex)
                        {
                            Global.LogEvent("ASCOM.Utilities.Log", "Failed to create TraceLogger instance, logging is disabled.", EventLogEntryType.Error, Global.EventLogErrors.TraceLoggerException, ex.ToString());
                        }
                    }
                }
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
            if (!componentUseLoggingEnabled)
                return; // Logging is disabled so exit early

            // Logging is enabled, so attempt to log the component information, but catch and log any exceptions that occur to avoid breaking the calling code.
            try
            {
                // SPECIAL HANDLING FOR FINALISEINSTALL
                // For some unknown reason this reports as CLR2 even though it is actually CLR4 so fix that here...
                if (string.Equals(Process.GetCurrentProcess().ProcessName, "FinaliseInstall", StringComparison.OrdinalIgnoreCase))
                {
                    dotNetRuntimeVersion = "4.0.30319";
                }

                string fullAssemblyName = assembly.FullName;

                // Validate input parameters
                if (string.IsNullOrEmpty(fullAssemblyName))
                {
                    LogMessage("Component", $"Assembly name cannot be null or empty - {fullAssemblyName}");
                    return;
                }

                if (string.IsNullOrEmpty(componentName))
                {
                    LogMessage("Component", $"Component name cannot be null or empty - {componentName}");
                    return;
                }

                // Split the assembly name and extract version information, removing extraneous spaces and "Version=" prefix
                string[] assemblyNameElements = fullAssemblyName.Split(',').Select(part => part.Trim()).Select(part2 => part2.Replace("Version=", "")).ToArray();
                if (assemblyNameElements.Length < 2)
                {
                    LogMessage("Component", $"Unable to parse assembly name format - {fullAssemblyName}");
                    return;
                }

                // Ensure that the assembly name and version are not null or empty
                string assemblyName = assemblyNameElements[0] is not null ? assemblyNameElements[0] : "Unknown assembly name";
                string assemblyVersion = assemblyNameElements[1] is not null ? assemblyNameElements[1] : "Unknown Version";

                LogMessage("Component", $"Process name: {Process.GetCurrentProcess().ProcessName}");
                LogMessage("Component", $"Application domain name: {AppDomain.CurrentDomain.FriendlyName}");
                LogMessage("Component", $"File name: {Process.GetCurrentProcess().MainModule.FileName}");
                LogMessage("Component", $"Called assembly: {assemblyName}, Version: {assemblyVersion} - {componentName}");
                LogMessage("", "");

                string processName = @$"Applications\{Process.GetCurrentProcess().ProcessName} - {Process.GetCurrentProcess().MainModule.FileName.Replace(@"\", "/")}";
                string keyName = assembly.GetName().Name;

                StackFrame[] frames = new StackTrace().GetFrames();
                LogMessage("StackTrace", $"Number of frames: {frames.Length - 1}"); // -1 because we always take off the Log.Component frame at the end of the stack trace, which is not useful information and just adds noise to the log.
                LogMessage("", "");

                string declaringTypeName = "";
                string methodName = "";
                string lastDeclaringMethodName = "";
                foreach (StackFrame frame in frames.Reverse())
                {
                    frame.GetType();
                    declaringTypeName = frame.GetMethod().DeclaringType.FullName;
                    methodName = frame.GetMethod().Name;

                    // Ignore frames from the logging code itself
                    if (declaringTypeName == "ASCOM.Utilities.Log")
                        continue;

                    LogMessage("StackTrace", $"Declaring type: {declaringTypeName} method: {methodName}");

                    if (methodName == "CreateInstance")
                    {
                        //processName = lastDeclaringMethodName;
                        try
                        {
                            Type type = Type.GetTypeFromProgID(lastDeclaringMethodName);
                            if (type != null)
                            {
                                LogMessage("StackTrace", $"Type from ProgID: {type.FullName}");
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
                                LogMessage("StackTrace", $"Type from ProgID not found: {lastDeclaringMethodName}");
                            }
                        }
                        catch (Exception ex)
                        {
                            LogMessage("StackTrace", $"Exception while getting type from ProgID: {lastDeclaringMethodName} - {ex.Message}");
                        }
                        break;
                    }

                    lastDeclaringMethodName = declaringTypeName;
                }

                LogMessage("", "");
                LogMessage("StackTrace", $"Process name: {processName}");
                LogMessage("", "");

                // Write the log entry to the registry
                using (RegistryAccess ra = new())
                {
                    // string key = @$"{Global.NET35_REGISTRY_BASE}\{processName}\{keyName}\{assemblyNameElements[0]} - {assemblyNameElements[1]}";
                    string key = @$"{Global.NET35_REGISTRY_BASE}\{processName}\{assemblyNameElements[0]} - {assemblyNameElements[1]}";
                    LogMessage("Registry", $"Writing to registry key: {key}");
                    ra.WriteProfile(key, componentName, componentName);

                    // Add the currently running Framework CLR version
                    string frameworkKey = @$"{Global.NET35_REGISTRY_BASE}\{processName}";
                    LogMessage("Registry", $"Writing CLR version {dotNetRuntimeVersion} to registry key: {frameworkKey}");
                    ra.WriteProfile(frameworkKey, "Framework", dotNetRuntimeVersion);
                }


                // Attempt to get the .NET runtime version of the primary process from the entry assembly
                try
                {
                    LogMessage("Information", $"{System.Environment.Version.ToString()} (Environment)");

                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly is null)
                        LogMessage("Information", "Entry assembly is null");
                    else
                    {
                        LogMessage("Information", $"Entry assembly: {entryAssembly.FullName}");
                        LogMessage("Information", $"Entry assembly location: {entryAssembly.Location}");
                        LogMessage("Information", $"Entry assembly code base: {entryAssembly.CodeBase}");

                        // Retrieve the .NET runtime version from the entry assembly
                        object imageRunTimeversion = Assembly.GetEntryAssembly().ImageRuntimeVersion;
                        if (imageRunTimeversion is null)
                            LogMessage("Information", "ImageRuntimeVersion is null");
                        else
                            LogMessage("Information", $"{Assembly.GetEntryAssembly().ImageRuntimeVersion} (ImageRuntimeVersion)");
                    }
                    LogMessage("Information", $"System version: {RuntimeEnvironment.GetSystemVersion()}");
                }
                catch (Exception ex)
                {
                    LogMessage("Information", ex.Message);
                }
                LogMessage("", "");
                LogMessage("==========", "==================================================================");
                LogMessage("", "");
            }
            catch (Exception ex)
            {
                LogMessage(componentName, $"Exception in Log.Component: {ex.Message}\r\n{ex}");
            }
        }

        static void LogMessage(string source, string message)
        {
            logger?.LogMessageCrLf(source, message);
        }
    }
}
