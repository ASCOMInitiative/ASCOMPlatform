using System;
using ASCOM.Utilities;
using System.Globalization;

namespace ASCOM.OpenWeatherMap
{
    static class Log
    {
        /// <summary>
        /// Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        private static TraceLogger tl = new TraceLogger(null, "OpenWeatherMap");

        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            tl.LogMessage(identifier, string.Format(message, args));
        }

        internal static void ReadProfile(Profile profile)
        {
            tl.Enabled = Convert.ToBoolean(profile.GetValue(OpenWeatherMap.driverID, "Trace Level", string.Empty, bool.TrueString), CultureInfo.InvariantCulture);
        }

        internal static void WriteProfile(Profile profile)
        {
            profile.WriteValue(OpenWeatherMap.driverID, "Trace Level", tl.Enabled.ToString(CultureInfo.InvariantCulture));
        }

        public static bool Enabled
        {
            get { return tl.Enabled; }
            set { tl.Enabled = value; } 
        }
    }
}
