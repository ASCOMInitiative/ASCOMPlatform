using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Utilities;

namespace ASCOM.Simulator
{
    internal static class Log
    {
        private static TraceLogger log = new TraceLogger(null , "Camera Simulator");
        private static bool started;

        internal static bool Enabled
        {
            get { return log.Enabled; }
            set { log.Enabled = value; }
        }

        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            if (started)
                Log.LogFinish(" ...");
            log.LogMessage(identifier, string.Format(message, args));
        }

        internal static void LogStart(string identifier, string message, params object[] args)
        {
            log.LogStart(identifier, string.Format(message, args));
            started = true;
        }

        internal static void LogFinish(string message, params object[] args)
        {
            started = false;
            log.LogFinish(string.Format(message, args));
        }

        internal static void LogMessageCrLf(string identifier, string message, params object[] args)
        {
            if (started)
                Log.LogFinish(" ...");
            log.LogMessageCrLf(identifier, string.Format(message, args));
        }
    }
}
