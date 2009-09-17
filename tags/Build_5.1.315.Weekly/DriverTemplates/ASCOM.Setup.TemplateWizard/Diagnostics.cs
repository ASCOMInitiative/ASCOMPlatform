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
		private static string[] TraceLevels = new string[]
		{
			TraceLevel.Off.ToString(),
			TraceLevel.Error.ToString(),
			TraceLevel.Warning.ToString(),
			TraceLevel.Info.ToString(),
			TraceLevel.Verbose.ToString()
		};
		static private Diagnostics theOne = new Diagnostics();	// Don't defer creating the singleton.
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
		static internal void TraceError(object msg)
			{
			Trace.WriteLineIf(ts.TraceError, msg, ts.Description + "[Error]");
			}
		/// <summary>
		/// Format and send a list of objects to the trace channel at severity level Error.
		/// </summary>
		/// <param name="format">Format string used to format the objects.</param>
		/// <param name="items">List of objects to be displayed.</param>
		static internal void TraceError(string format, params object[] items)
			{
			Trace.WriteLineIf(ts.TraceError, String.Format(format, items), ts.Description + "[Error]");
			}
		/// <summary>
		/// Send an object to the trace channel at severity level Warning.
		/// </summary>
		/// <param name="msg">The object (which may be a string) to display.</param>
		static internal void TraceWarning(object msg)
			{
			Trace.WriteLineIf(ts.TraceWarning, msg, ts.Description + "[Warn]");
			}
		/// <summary>
		/// Format and send a list of objects to the trace channel at severity level Warning.
		/// </summary>
		/// <param name="format">Format string used to format the objects.</param>
		/// <param name="items">List of objects to be displayed.</param>
		static internal void TraceWarning(string format, params object[] items)
			{
			Trace.WriteLineIf(ts.TraceWarning, String.Format(format, items), ts.Description + "[Warn]");
			}
		/// <summary>
		/// Send an object to the trace channel at severity level Information.
		/// </summary>
		/// <param name="msg">The object (which may be a string) to display.</param>
		static internal void TraceInfo(object msg)
			{
			Trace.WriteLineIf(ts.TraceInfo, msg, ts.Description + "[Info]");
			}
		/// <summary>
		/// Format and send a list of objects to the trace channel at severity level Information.
		/// </summary>
		/// <param name="format">Format string used to format the objects.</param>
		/// <param name="items">List of objects to be displayed.</param>
		static internal void TraceInfo(string format, params object[] items)
			{
			Trace.WriteLineIf(ts.TraceInfo, String.Format(format, items), ts.Description + "[Info]");
			}
		/// <summary>
		/// Send an object to the trace channel at severity level Verbose Information.
		/// </summary>
		/// <param name="msg">The object (which may be a string) to display.</param>
		static internal void TraceVerbose(object msg)
			{
			Trace.WriteLineIf(ts.TraceVerbose, msg, ts.Description + "[Verb]");
			}
		/// <summary>
		/// Format and send a list of objects to the trace channel at severity level Verbose Information.
		/// </summary>
		/// <param name="format">Format string used to format the objects.</param>
		/// <param name="items">List of objects to be displayed.</param>
		static internal void TraceVerbose(string format, params object[] items)
			{
			Trace.WriteLineIf(ts.TraceVerbose, String.Format(format, items), ts.Description + "[Verb]");
			}
		/// <summary>
		/// Produces a trace of entry into a method.
		/// Obtains the calling method name from the stack trace.
		/// </summary>
		static internal void Enter()
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
		static internal void Exit()
		{
			// Jump up the stack frame one level and locate the
			// calling method.
			StackFrame stackFrame = new StackFrame(1);
			MethodBase callingMethod = stackFrame.GetMethod();
			// Build a string containing the namespace and method name
			string caller = callingMethod.DeclaringType.FullName + '.' + callingMethod.Name;
			TraceInfo("Exit " + caller);
		}

		/// <summary>
		/// Utility function. Expands non-printable ASCII characters into mnemonic human-readable form.
		/// </summary>
		/// <returns>
		/// Returns a new string with non-printing characters replaced by human-readable mnemonics.
		/// </returns>
		static internal string ExpandASCII(string strTrace)
			{
			StringBuilder expanded = new StringBuilder(Math.Max(64, strTrace.Length * 3));
			foreach (char c in strTrace)
				{
				byte b = (byte)c;
				string strASCII = Enum.GetName(typeof(ASCII), b);
				if (strASCII != null)
					expanded.Append("<" + strASCII + ">");
				else
					expanded.Append(c);
				}
			return expanded.ToString();
			}
		}
#pragma warning disable 1591	// No XML documentation is required for this enum
	/// <summary>
	/// RawWord type with enumeration constants for ASCII control codes.
	/// </summary>
	public enum ASCII : byte
		{
		NULL = 0x00, SOH = 0x01, STH = 0x02, ETX = 0x03, EOT = 0x04, ENQ = 0x05, ACK = 0x06, BELL = 0x07,
		BS = 0x08, HT = 0x09, LF = 0x0A, VT = 0x0B, FF = 0x0C, CR = 0x0D, SO = 0x0E, SI = 0x0F, DC1 = 0x11,
		DC2 = 0x12, DC3 = 0x13, DC4 = 0x14, NAK = 0x15, SYN = 0x16, ETB = 0x17, CAN = 0x18, EM = 0x19,
		SUB = 0x1A, ESC = 0x1B, FS = 0x1C, GS = 0x1D, RS = 0x1E, US = 0x1F, /*SP = 0x20,*/ DEL = 0x7F
		}

}
