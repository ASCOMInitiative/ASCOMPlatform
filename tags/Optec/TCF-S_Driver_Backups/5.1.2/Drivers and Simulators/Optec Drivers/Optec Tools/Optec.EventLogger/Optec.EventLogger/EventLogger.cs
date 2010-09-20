using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Optec
{
    public static class EventLogger
    {
        private static EventLog myEventLog;
        private static EventLogTraceListener myEventLogTraceListener;
        private const string eventLogSource = "Optec Inc.";
        private const string eventLogName = "Optec Device Log";
        private static TraceSwitch myTraceSwitch;

        static EventLogger()
        {
            
            myTraceSwitch = new TraceSwitch("OptecLogger", "Trace logging for Optec Inc. applications");
            myTraceSwitch.Level = TraceLevel.Warning;
            SetupEventLogTraceListener();
            Trace.WriteLine("Optec EventLogger Initialized - Trace Level = " + LoggingLevel.ToString());
        }

        public static TraceLevel LoggingLevel
        {
            get { return myTraceSwitch.Level; }
            set { myTraceSwitch.Level = value; 
                Trace.WriteLine("Trace level set to " +  value.ToString());
            }
        }

        private static void SetupEventLogTraceListener()
        {
            if (!EventLog.SourceExists(eventLogSource))
            {
                EventLog.CreateEventSource(eventLogSource, eventLogName);
            }
            myEventLog = new EventLog();
            myEventLog.Source = eventLogSource;
            myEventLogTraceListener = new EventLogTraceListener(myEventLog);
            Trace.Listeners.Add(myEventLogTraceListener);
        }

        public static void LogMessage(string message, TraceLevel messageType)
        {  
            switch (messageType)
            {
                case TraceLevel.Off:
                    break;
                case TraceLevel.Error:
                    if (myTraceSwitch.TraceError) WriteTrace(message);
                    break;
                case TraceLevel.Warning:
                    if (myTraceSwitch.TraceWarning) WriteTrace(message);
                    break;
                case TraceLevel.Info:
                    if (myTraceSwitch.TraceInfo) WriteTrace(message);
                    break;
                case TraceLevel.Verbose:
                    if (myTraceSwitch.TraceVerbose) WriteTrace(message);
                    break;

            }
        }

        private static void WriteTrace(string msg)
        {
            CallerInfo caller = GetCallerInfo();
            string logstring = caller.AssemblyName + "." + caller.ClassName + "." + caller.MethodName + " " +
                msg;
            Trace.WriteLine(logstring);
        }

        public static void LogMessage(Exception ex)
        {
            if (myTraceSwitch.Level == TraceLevel.Off) return;
            CallerInfo caller = GetCallerInfo();
           
            Trace.WriteLine("***EXCEPTION OCCURRED ********EXCEPTION OCCURRED ********EXCEPTION OCCURRED ********");
            Trace.WriteLine("***The exception was caught by: " + caller.CallerInfoCombined);
            Trace.WriteLine("***The exception message was: " + ex.Message);
            Trace.WriteLine("***Stack Trace Data: " + ex.StackTrace);
            Trace.WriteLine("************************************************************************************");
            
        }

        private static CallerInfo GetCallerInfo()
        {
            var stackFrame = new StackFrame(3, true);
            var callingMethod = stackFrame.GetMethod();
            var callingtype = callingMethod.DeclaringType;
            var callingAssembly = callingtype.Assembly;
            CallerInfo c = new CallerInfo();
            c.AssemblyName = callingAssembly.GetName().Name;
            c.ClassName = callingtype.Name;
            c.MethodName = callingMethod.Name;
            c.CallerInfoCombined =
                c.AssemblyName + "." +
                c.ClassName + "." +
                c.MethodName;
            return c;
        }

        private static CallerInfo GetCallerInfo_External(int offset)
        {
            var stackFrame = new StackFrame(2 + offset, true);
            var callingMethod = stackFrame.GetMethod();
            var callingtype = callingMethod.DeclaringType;
            var callingAssembly = callingtype.Assembly;
            CallerInfo c = new CallerInfo();
            c.AssemblyName = callingAssembly.GetName().Name;
            c.ClassName = callingtype.Name;
            c.MethodName = callingMethod.Name;
            c.CallerInfoCombined =
                c.AssemblyName + "." +
                c.ClassName + "." +
                c.MethodName;
            return c;
        }
    }

    

    struct CallerInfo
    {
        public string MethodName;
        public string AssemblyName;
        public string ClassName;
        public string CallerInfoCombined;
    }
}
