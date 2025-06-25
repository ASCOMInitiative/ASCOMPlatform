using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// Logs the specified component name.
        /// </summary>
        /// <remarks>The log entry includes the component name. Ensure that the <paramref name="componentName"/> parameter is meaningful to help identify the
        /// source of the log.</remarks>
        /// <param name="assemblyName">The component's assembly.</param>
        /// <param name="componentName">The name of the component to include in the log entry. Cannot be null or empty.</param>
        public static void Component(string assemblyName, string componentName)
        {
            TraceLogger TL = new TraceLogger("", "Log");
            TL.Enabled = true;
            try
            {

                if (string.IsNullOrEmpty(assemblyName))
                {
                    TL.LogMessage("Component", $"Assembly name cannot be null or empty - {assemblyName}");
                }
                if (string.IsNullOrEmpty(componentName))
                {
                    TL.LogMessage("Component", $"Component name cannot be null or empty - {componentName}");
                }

                string[] assemblyNameParts = assemblyName.Split(',').Select(part => part.Trim()).Select(part2 => part2.Replace("Version=", "")).ToArray();


                TL.LogMessage("Component", $"Assembly: {assemblyNameParts[0]}, Version: {assemblyNameParts[1]} - {componentName}");
                TL.LogMessage("Component", $"Friendly name: {System.AppDomain.CurrentDomain.FriendlyName}");
                TL.LogMessage("Component", $"Process name: {System.Diagnostics.Process.GetCurrentProcess().ProcessName}");
                TL.LogMessage("Component", $"File name: {System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName}");

                string keyName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Replace(@"\", "/");

                using (RegistryAccess ra = new RegistryAccess())
                {
                    ra.WriteProfile(@$".NET35\{keyName}\{assemblyNameParts[0]}\{assemblyNameParts[1]}", componentName, componentName);
                }
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf(componentName, $"Exception in Log.Component: {ex.Message}\r\n{ex}");
            }
            finally
            {
                TL.Enabled = false;
                TL.Dispose();
            }

            TL.Enabled = false;
            TL.Dispose();
        }
    }
}
