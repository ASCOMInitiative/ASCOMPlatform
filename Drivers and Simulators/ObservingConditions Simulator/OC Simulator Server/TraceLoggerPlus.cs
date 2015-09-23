using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ASCOM.Utilities;

namespace ASCOM.Simulator
{
    public class TraceLoggerPlus : TraceLogger
    {
        public TraceLoggerPlus(string fileName, string logName) :base(fileName,logName)
        {

        }

        public void LogMessage(int instance, string prefix, string message)
        {
            base.LogMessage(prefix + " " + instance.ToString(), message);
        }

        public void LogMessageCrLf(int instance, string prefix, string message)
        {
            base.LogMessageCrLf(prefix + " " + instance.ToString(), message);
        }

        public void LogMessage(string identifier, string message, params object[] args)
        {
            base.LogMessage(identifier, string.Format(message, args));
        }

        public void LogMessageCrLf(string identifier, string message, params object[] args)
        {
            base.LogMessageCrLf(identifier, string.Format(message, args));
        }
    }


}
