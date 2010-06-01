//-----------------------------------------------------------------------
// <copyright company="TiGra Astronomy">
//     Copyright © 2010 TiGra Astronomy, All Rights Reserved
// </copyright>
// <author>Tim Long</author>
// <license>
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </license>
// <summary>Licensing statement</summary>
//-----------------------------------------------------------------------

using System;
using System.Text;
using System.Diagnostics;

namespace TiGra
{
	/// <summary>
	/// The Diagnostics class provides a few helper methods that make it easier to produce coherent
	/// debugging output. The class is implemented as a singleton that is created as soon as the assembly
	/// is loaded. The level of trace output that is produced is controlled by a <see cref="TraceSwitch"/>
	/// that in turn loads its configuration from the App.config file. If there is no App.Config file,
	/// the default is to produce verbose output for debug builds and errors/warnings for release builds.
	/// <para>
	/// We recommend SysInternals DbgView for viewing and capturing the trace output.
	/// See http://www.sysinternals.com
	/// </para>
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
		/// <summary>
		/// Construct and initialise diagnostics.
		/// </summary>
		protected Diagnostics()
		{
#if DEBUG
			string strLevel = "Verbose";
#else
			string strLevel = "Warning";
#endif
			ts = new TraceSwitch("TiGra.ASCOM", "TiGra.ASCOM", strLevel);
			Trace.WriteLine(String.Format("===== TiGra.ASCOM Start Diagnostics: TraceLevel = {0} =====", ts.Level));
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
		static public void TraceError(object msg)
		{
			Trace.WriteLineIf(ts.TraceError, msg, String.Format("{0}[Error]", ts.Description));
		}
		/// <summary>
		/// Format and send a list of objects to the trace channel at severity level Error.
		/// </summary>
		/// <param name="format">Format string used to format the objects.</param>
		/// <param name="items">List of objects to be displayed.</param>
		static public void TraceError(string format, params object[] items)
		{
			Trace.WriteLineIf(ts.TraceError, String.Format(format, items), String.Format("{0}[Error]", ts.Description));
		}
		/// <summary>
		/// Send an object to the trace channel at severity level Warning.
		/// </summary>
		/// <param name="msg">The object (which may be a string) to display.</param>
		static public void TraceWarning(object msg)
		{
			Trace.WriteLineIf(ts.TraceWarning, msg, String.Format("{0}[Warn]", ts.Description));
		}
		/// <summary>
		/// Format and send a list of objects to the trace channel at severity level Warning.
		/// </summary>
		/// <param name="format">Format string used to format the objects.</param>
		/// <param name="items">List of objects to be displayed.</param>
		static public void TraceWarning(string format, params object[] items)
		{
			Trace.WriteLineIf(ts.TraceWarning, String.Format(format, items), String.Format("{0}[Warn]", ts.Description));
		}
		/// <summary>
		/// Send an object to the trace channel at severity level Information.
		/// </summary>
		/// <param name="msg">The object (which may be a string) to display.</param>
		static public void TraceInfo(object msg)
		{
			Trace.WriteLineIf(ts.TraceInfo, msg, String.Format("{0}[Info]", ts.Description));
		}
		/// <summary>
		/// Format and send a list of objects to the trace channel at severity level Information.
		/// </summary>
		/// <param name="format">Format string used to format the objects.</param>
		/// <param name="items">List of objects to be displayed.</param>
		static public void TraceInfo(string format, params object[] items)
		{
			Trace.WriteLineIf(ts.TraceInfo, String.Format(format, items), String.Format("{0}[Info]", ts.Description));
		}
		/// <summary>
		/// Send an object to the trace channel at severity level Verbose Information.
		/// </summary>
		/// <param name="msg">The object (which may be a string) to display.</param>
		static public void TraceVerbose(object msg)
		{
			Trace.WriteLineIf(ts.TraceVerbose, msg, String.Format("{0}[Verb]", ts.Description));
		}
		/// <summary>
		/// Format and send a list of objects to the trace channel at severity level Verbose Information.
		/// </summary>
		/// <param name="format">Format string used to format the objects.</param>
		/// <param name="items">List of objects to be displayed.</param>
		static public void TraceVerbose(string format, params object[] items)
		{
			Trace.WriteLineIf(ts.TraceVerbose, String.Format(format, items), String.Format("{0}[Verb]", ts.Description));
		}
		/// <summary>
		/// Utility function. Expands non-printable ASCII characters into mnemonic human-readable form.
		/// </summary>
		/// <returns>
		/// Returns a new string with non-printing characters replaced by human-readable mnemonics.
		/// </returns>
		static public string ExpandASCII(string strTrace)
		{
			StringBuilder expanded = new StringBuilder(Math.Max(64, strTrace.Length * 3));
			foreach (char c in strTrace)
			{
				byte b = (byte)c;
				string strASCII = Enum.GetName(typeof(ASCII), b);
				if (strASCII != null)
					expanded.Append(String.Format("<{0}>", strASCII));
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
