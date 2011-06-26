//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Telescope CommandItem
//
// Description:	This implements command item object to describe each serial command
//
// Author:      (pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 24-MAY-2011	pk	1.0.0	Initial edit, moved from GeminiHardware.Instance.cs into its own file
// --------------------------------------------------------------------------------
//

using System;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.Timers;
using System.IO.Ports;
using System.Windows.Forms;
using System.Drawing;
using ASCOM.GeminiTelescope.Properties;


namespace ASCOM.GeminiTelescope
{
    /// <summary>
    /// Single serial command to be delivered to Gemini Hardware through worker thread queue
    /// </summary>
    internal class CommandItem
    {

        internal string m_Command;  //actual serial command to be sent, not including ending '#' or the native checksum
        int m_ThreadID;             //this will record thread id of the calling thread
        internal int m_Timeout;     //timeout value for this command in msec, -1 if no timeout wanted

        private System.Threading.ManualResetEvent m_WaitForResultHandle = null; // wait handle set by worker thread when result is received
        internal HardwareAsyncDelegate m_AsyncDelegate = null;  // call-back delegate for asynchronous operation
        /// <summary>
        /// result produced by Gemini, or null if no result. Ending '#' is always stripped off
        /// </summary>
        internal string m_Result { get; set; }
        internal bool m_Raw = false;

        internal bool m_UpdateRequired { get; set; } //true if this command updates a polled status variable, and an update is needed ASAP

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">actual serial command to be sent, not including ending '#' or the native checksum</param>
        /// <param name="timeout">timeout value for this command in msec, -1 if no timeout wanted</param>
        /// <param name="wantResult">does the caller want the result returned by Gemini?</param>
        /// <param name="bRaw">command is a raw string to be passed to the device unmodified</param>
        internal CommandItem(string command, int timeout, bool wantResult, bool bRaw)
        {
            m_Command = command;
            m_ThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            m_Timeout = timeout;

            // create a wait handle if result is desired
            if (wantResult)
                m_WaitForResultHandle = new System.Threading.ManualResetEvent(false);
            m_Result = null;
            m_Raw = bRaw;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">actual serial command to be sent, not including ending '#' or the native checksum</param>
        /// <param name="timeout">timeout value for this command in msec, -1 if no timeout wanted</param>
        /// <param name="wantResult">does the caller want the result returned by Gemini?</param>
        internal CommandItem(string command, int timeout, bool wantResult)
            : this(command, timeout, wantResult, false)
        {
        }

        /// <summary>
        ///  Initialize with an asynchrounous call-back delegate and a timeout
        /// </summary>
        /// <param name="command">actual serial command to be sent, not including ending '#' or the native checksum</param>
        /// <param name="timeout">timeout value for this command in msec, -1 if no timeout wanted</param>
        /// <param name="callback">asynchronous callback delegate to call on completion
        ///        public delegate void HardwareAsyncDelegate(string cmd, string result);
        /// </param>
        /// <param name="bRaw">command is a raw string to be passed to the device unmodified</param>
        internal CommandItem(string command, int timeout, HardwareAsyncDelegate callback, bool bRaw)
            : this(command, timeout, true, bRaw)
        {
            m_AsyncDelegate = callback;
        }

        /// <summary>
        /// Return WaitHandle object to be set on receipt of the result for this command
        /// </summary>
        internal System.Threading.ManualResetEvent WaitObject
        {
            get { return m_WaitForResultHandle; }
        }

        /// <summary>
        ///     Wait on the synchronization wait handle to signal that the result is now available
        ///     result is placed into m_sResult by the worker thread and the event is then signaled
        /// </summary>
        /// <returns>result produced by Gemini as after executing this command or null if timeout expired</returns>
        internal string WaitForResult()
        {
            if (m_WaitForResultHandle != null)
            {
                if (m_Timeout > 0)
                {
                    if (m_WaitForResultHandle.WaitOne(m_Timeout))
                        return m_Result;
                    else
                    {
                        GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Time out occurred after " + m_Timeout.ToString() + "msec processing command '" + m_Command + "'");
                        return null;
                    }
                }
                else
                    m_WaitForResultHandle.WaitOne();  // no timeout specified, wait indefinitely
            }
            return null;
        }
    }

}
