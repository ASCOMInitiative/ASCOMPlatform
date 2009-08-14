//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Telescope Hardware
//
// Description:	This implements a feed-through (virtual) serial port for supporting non-ASCOM gemini applications
//              that require exclusive access to the physical COM port.
//              The pass-through port must be another physical port or a virtual port set up using a third-party tool
//
//                  e.g.: http://www.eterlogic.com/Products.VSPE.html
//
// Implements:	ASCOM Telescope PassThroughPort class
// Author:		(pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//              
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 11-AUG-2009  pk 1.0.1    Created feed-through physcal/virtual port implementation
// --------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.Timers;
using System.IO.Ports;

namespace ASCOM.GeminiTelescope
{
    /// <summary>
    /// This class uses a pass-through (physical or virtual) serial port for supporting non-ASCOM gemini applications
    /// that require exclusive access to the physical COM port.
    /// The pass-through port must be another physical port or a virtual port set up using a third-party tool
    ///   e.g.: http://www.eterlogic.com/Products.VSPE.html
    /// </summary>
    class PassThroughPort
    {
        SerialPort m_SerialPort = new SerialPort();
        System.Threading.Thread m_ListenerThread;
        System.Threading.ManualResetEvent m_CancelAsync = new System.Threading.ManualResetEvent(false);
        System.Threading.ManualResetEvent m_DisplayDataAvailable = new System.Threading.ManualResetEvent(false);
        System.Threading.ManualResetEvent m_SerialDataAvailable = new System.Threading.ManualResetEvent(false);
        StringBuilder m_DisplayString = new StringBuilder();
        object m_LockDisplayData = new object();
        bool m_PortActive = false;



        public PassThroughPort()
        {

        }

        /// <summary>
        /// Set up and open the pass-through port, create listening background thread
        /// </summary>
        /// <param name="port"></param>
        /// <param name="speed"></param>
        public void Initialize(string port, int speed)
        {
            if (m_SerialPort.IsOpen) m_SerialPort.Close();  //reset the port if already open

            m_SerialPort.PortName = port;
            m_SerialPort.BaudRate = speed;
            m_SerialPort.Parity = Parity.None;
            m_SerialPort.DataBits = 8;
            m_SerialPort.StopBits = (System.IO.Ports.StopBits)1;
            m_SerialPort.Handshake = System.IO.Ports.Handshake.None;
            //m_SerialPort.ReadBufferSize = 256;
            m_SerialPort.Open();

            m_SerialPort.DtrEnable = true;
            m_SerialPort.ReceivedBytesThreshold = 1;
            m_DisplayString = new StringBuilder();
            m_DisplayDataAvailable.Reset();
            m_SerialDataAvailable.Reset();

            m_CancelAsync.Reset();
            m_ListenerThread = new System.Threading.Thread(ListenUp);
            m_ListenerThread.Start();
        }

        /// <summary>
        /// Stop the background thread, close pass-through port
        /// </summary>
        public void Stop()
        {
            try
            {
                m_CancelAsync.Set();
                lock (m_SerialPort) m_SerialPort.Close();

                if (m_ListenerThread != null && !m_ListenerThread.Join(5000))
                    m_ListenerThread.Abort();

                m_ListenerThread = null;
            }
            catch { }
        }
        
        /// <summary>
        /// send string to port, usually used when an unsolicited binary stream
        /// was received from Gemini to pass it through to the 
        /// listening application
        /// </summary>
        /// <param name="s"></param>
        public void PassStringToPort(StringBuilder s)
        {
            lock (m_LockDisplayData)
            {
                if (s[0] == 0x8c)
                    m_DisplayString = s;
                else
                    if (!m_DisplayString.ToString().Contains("\x99"))
                        m_DisplayString.Append(s);
                    else
                        m_DisplayString = s;

                if (!m_DisplayString.ToString().Contains("\x99") && m_DisplayString[0] != 0x8c) return;
                m_DisplayDataAvailable.Set();
                m_SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            }
        }

        /// <summary>
        /// true if serial port is open and some data has been received on it from the other side
        /// </summary>
        public bool PortActive
        {
            get { return m_SerialPort.IsOpen && m_PortActive; }
            set { m_PortActive = value; }
        }


        void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            m_SerialDataAvailable.Set();
        }


        /// <summary>
        /// Background listener thread. Waits for commands to be received on the pass-through port,
        /// forwards them to GeminiHardware using DoCommandResult(...)
        /// The result returned from DoCommand is sent back through the pass-through port
        /// 
        /// If the port is idle, checks to see if any binary data has been received from GeminiHardware,
        ///  and if so, passes it on to the pass-through port (display data stream)
        ///  
        /// </summary>
        private void ListenUp()
        {
            string incoming = "";
            System.Threading.ManualResetEvent [] evts = {m_SerialDataAvailable, m_DisplayDataAvailable, m_CancelAsync};

            m_SerialPort.DiscardInBuffer();
            m_SerialPort.DiscardOutBuffer();


            while (!m_CancelAsync.WaitOne(0))
            {
                if (incoming.Length > 0 || m_SerialPort.BytesToRead > 0)
                {
                    m_PortActive = true;    // if at some data has been received over this port, consider it active

                    while (m_SerialPort.BytesToRead > 0)
                        incoming += Convert.ToChar(m_SerialPort.ReadByte());


                    string[] cmds = incoming.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);


                    if (cmds[0] == "\x6")   // treat ^G as a reset
                    {
                        incoming = "";
                        cmds = new string[] { "\x6" };
                        m_SerialPort.DiscardInBuffer();
                        m_SerialPort.DiscardOutBuffer();
                    }
                    else
                    if (incoming.EndsWith("#"))
                        incoming = "";
                    else 
                    {
                        incoming = cmds[cmds.Length - 1]; // last command is not terminated yet, leave it in the buffer
                        Array.Resize<string>(ref cmds, cmds.Length - 1);    //remove last, unterminated item from array
                    }

                    //re-terminate all commands after split:
                    for (int i = 0; i < cmds.Length; ++i)
                        cmds[i] += "#";

                    if (cmds.Length == 0) continue;

                    string[] res = null;
                    GeminiHardware.DoCommandResult(cmds, GeminiHardware.MAX_TIMEOUT, true, out res);

                    if (res != null)
                        foreach (string r in res)
                        {
                            if (r != null)
                            {
                                //writing too quick, or nobody's listening?
                                while (m_SerialPort.BytesToWrite + r.Length >= m_SerialPort.WriteBufferSize)
                                    System.Threading.Thread.Sleep(500);

                                lock (m_SerialPort)
                                {
                                    m_SerialPort.Write(Encoding.GetEncoding("Latin1").GetBytes(r), 0, r.Length);
                                    m_SerialPort.BaseStream.Flush();
                                    System.Threading.Thread.Sleep(0);
                                }
                            }
                        }

                } 
                else // all is quiet. no pending commands to execute, check if a new display stream is ready
                {
                    m_SerialDataAvailable.Reset();

                    if (m_DisplayDataAvailable.WaitOne(0))
                    {
                        m_DisplayDataAvailable.Reset();

                        if (m_SerialPort.IsOpen)
                        {
                            // port is backed up, wait a while...
                            while (m_SerialPort.BytesToWrite + m_DisplayString.Length >= m_SerialPort.WriteBufferSize)
                                System.Threading.Thread.Sleep(100);

                            lock (m_LockDisplayData)
                            {
                                lock (m_SerialPort)
                                {
                                    m_SerialPort.Write(Encoding.GetEncoding("Latin1").GetBytes(m_DisplayString.ToString()), 0, m_DisplayString.Length);
//                                    m_SerialPort.Write(m_DisplayString.ToString());
                                    m_SerialPort.BaseStream.Flush();
                                }
#if DEBUG
                                System.Diagnostics.Trace.Write("PassStringToVirtualPort: ");
                                string txt_out = "";
                                string hx_out = "";
                                for (int i = 0; i < m_DisplayString.Length; ++i)
                                {
                                    txt_out += Convert.ToChar(((byte)m_DisplayString[i]) & 0x7f);
                                    hx_out += (((int)m_DisplayString[i] & 0x7f)).ToString("x") + " ";
                                }
                                System.Diagnostics.Trace.WriteLine(" " + txt_out + " " + hx_out);
#endif
                                m_DisplayString.Length = 0;
                            }
                        }
                    }

                }
                System.Threading.WaitHandle.WaitAny(evts, 50); //wait for display or serial command data to become available, or just sleep for a while
            }
        }
    }
}
