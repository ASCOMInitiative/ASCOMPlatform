using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace ASCOM.Setup
{
    /// <summary>
    /// The Diagnostics class provides a few helper methods that make it easier to produce coherent
    /// debugging output. The class is implemented as a singleton that is created as soon as the assembly
    /// is loaded. The level of trace output that is produced is controlled by a <see cref="TraceSwitch"/>
    /// that in turn loads its configuration from the App.config file. If there is no App.Config file,
    /// the default is to produce trace output for errors only.
    /// </summary>
    internal class Diagnostics
    {
        /// <summary>
        /// Text versions of the various trace levels.
        /// </summary>
        static string[] TraceLevels = new string[]
                                          {
                                              TraceLevel.Off.ToString(),
                                              TraceLevel.Error.ToString(),
                                              TraceLevel.Warning.ToString(),
                                              TraceLevel.Info.ToString(),
                                              TraceLevel.Verbose.ToString()
                                          };

        static Diagnostics theOne = new Diagnostics(); // Don't defer creating the singleton.
        static TraceSwitch ts;

        protected Diagnostics()
        {
#if DEBUG
			int level = 4;
#else
            int level = 2;
#endif
            string strLevel = Diagnostics.TraceLevels[level];
            ts = new TraceSwitch("ASCOM.Setup", "ASCOM.Setup", strLevel);
            Trace.WriteLine("===== Start Diagnostics: TraceLevel = " + ts.Level.ToString() + " =====");
        }

        /// <summary>
        /// Gets a reference to the one and only instance of this singleton class.
        /// </summary>
        /// <returns>a reference to the one and only instance of this singleton class.</returns>
        public static Diagnostics GetInstance()
        {
            if (theOne == null)
                theOne = new Diagnostics();
            return theOne;
        }

        /// <summary>
        /// Send an object to the trace channel at severity level Error.
        /// </summary>
        /// <param name="msg">The object (which may be a string) to display.</param>
        internal static void TraceError(object msg)
        {
            Trace.WriteLineIf(ts.TraceError, msg, ts.Description + "[Error]");
        }

        /// <summary>
        /// Format and send a list of objects to the trace channel at severity level Error.
        /// </summary>
        /// <param name="format">Format string used to format the objects.</param>
        /// <param name="items">List of objects to be displayed.</param>
        internal static void TraceError(string format, params object[] items)
        {
            Trace.WriteLineIf(ts.TraceError, String.Format(format, items), ts.Description + "[Error]");
        }

        /// <summary>
        /// Send an object to the trace channel at severity level Warning.
        /// </summary>
        /// <param name="msg">The object (which may be a string) to display.</param>
        internal static void TraceWarning(object msg)
        {
            Trace.WriteLineIf(ts.TraceWarning, msg, ts.Description + "[Warn]");
        }

        /// <summary>
        /// Format and send a list of objects to the trace channel at severity level Warning.
        /// </summary>
        /// <param name="format">Format string used to format the objects.</param>
        /// <param name="items">List of objects to be displayed.</param>
        internal static void TraceWarning(string format, params object[] items)
        {
            Trace.WriteLineIf(ts.TraceWarning, String.Format(format, items), ts.Description + "[Warn]");
        }

        /// <summary>
        /// Send an object to the trace channel at severity level Information.
        /// </summary>
        /// <param name="msg">The object (which may be a string) to display.</param>
        internal static void TraceInfo(object msg)
        {
            Trace.WriteLineIf(ts.TraceInfo, msg, ts.Description + "[Info]");
        }

        /// <summary>
        /// Format and send a list of objects to the trace channel at severity level Information.
        /// </summary>
        /// <param name="format">Format string used to format the objects.</param>
        /// <param name="items">List of objects to be displayed.</param>
        internal static void TraceInfo(string format, params object[] items)
        {
            Trace.WriteLineIf(ts.TraceInfo, String.Format(format, items), ts.Description + "[Info]");
        }

        /// <summary>
        /// Send an object to the trace channel at severity level Verbose Information.
        /// </summary>
        /// <param name="msg">The object (which may be a string) to display.</param>
        internal static void TraceVerbose(object msg)
        {
            Trace.WriteLineIf(ts.TraceVerbose, msg, ts.Description + "[Verb]");
        }

        /// <summary>
        /// Format and send a list of objects to the trace channel at severity level Verbose Information.
        /// </summary>
        /// <param name="format">Format string used to format the objects.</param>
        /// <param name="items">List of objects to be displayed.</param>
        internal static void TraceVerbose(string format, params object[] items)
        {
            Trace.WriteLineIf(ts.TraceVerbose, String.Format(format, items), ts.Description + "[Verb]");
        }

        /// <summary>
        /// Produces a trace of entry into a method.
        /// Obtains the calling method name from the stack trace.
        /// </summary>
        internal static void Enter()
        {
            // Jump up the stack frame one level and locate the
            // calling method.
            StackFrame stackFrame = new StackFrame(1);
            MethodBase callingMethod = stackFrame.GetMethod();
            // Build a string containing the namespace and method name
            string caller = callingMethod.DeclaringType.FullName + '.' + callingMethod.Name;
            TraceInfo("Enter " + caller);
        }

        /// <summary>
        /// Produces a trace of exit from a method.
        /// Obtains the calling method name from the stack trace.
        /// </summary>
        internal static void Exit()
        {
            // Jump up the stack frame one level and locate the
            // calling method.
            StackFrame stackFrame = new StackFrame(1);
            MethodBase callingMethod = stackFrame.GetMethod();
            // Build a string containing the namespace and method name
            string caller = callingMethod.DeclaringType.FullName + '.' + callingMethod.Name;
            TraceInfo("Exit " + caller);
        }
    }
}
