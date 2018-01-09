//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - Simulator
//
// Description:	A helper class for debug tracing
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ASCOM.Utilities;

namespace ASCOM.Simulator.Utils
{
	public static class DebugTrace
	{
		private static string TRACE_CATEGORY_ERROR = "VideoCameraSimulator.ASCOM[Error]";
		private static string TRACE_CATEGORY_WARNING = "VideoCameraSimulator.ASCOM[Warn]";
		private static string TRACE_CATEGORY_INFO = "VideoCameraSimulator.ASCOM[Info]";
		private static string TRACE_CATEGORY_VERBOSE = "VideoCameraSimulator.ASCOM[Verb]";
		private static string TRACE_IDENTIFIER = "ASCOM.Simulator.Video";

		private static TraceLogger s_TraceLogger;

		private static object s_SyncLock = new object();

		static DebugTrace()
		{
			lock (s_SyncLock)
			{
				s_TraceLogger = new TraceLogger("VideoSimulator");
				s_TraceLogger.Enabled = true;
			}
		}

		public static void TraceError(Exception ex)
		{
			var fullErrorMessage = new StringBuilder();

			Exception innerEx = ex;
			while (innerEx != null)
			{
				fullErrorMessage.AppendFormat("{0} : {1}\r\n{2}\r\n-----------------------------------------------------------\r\n", ex.GetType(), ex.Message, ex.StackTrace);

				innerEx = innerEx.InnerException;
			}

			TraceError(fullErrorMessage.ToString());
		}

		public static void TraceError(string error)
		{
			if (TraceSwitches.DebugTracing.TraceError)
			{
				s_TraceLogger.LogMessage(TRACE_IDENTIFIER, error);
				Trace.WriteLine(error, TRACE_CATEGORY_ERROR);
			}
		}

		public static void TraceWarning(string warning)
		{
			if (TraceSwitches.DebugTracing.TraceWarning)
			{
				s_TraceLogger.LogMessage(TRACE_IDENTIFIER, warning);
				Trace.WriteLine(warning, TRACE_CATEGORY_WARNING);
			}
		}

		public static void TraceInfo(string info)
		{
			if (TraceSwitches.DebugTracing.TraceInfo)
			{
				s_TraceLogger.LogMessage(TRACE_IDENTIFIER, info);
				Trace.WriteLine(info, TRACE_CATEGORY_INFO);
			}
		}

		public static void TraceVerbose(string detail)
		{
			if (TraceSwitches.DebugTracing.TraceVerbose)
			{
				s_TraceLogger.LogMessage(TRACE_IDENTIFIER, detail);
				Trace.WriteLine(detail, TRACE_CATEGORY_VERBOSE);
			}
		}
	}
}
