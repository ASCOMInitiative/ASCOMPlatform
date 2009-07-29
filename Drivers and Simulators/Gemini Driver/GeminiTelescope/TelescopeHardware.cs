//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Gemini Telescope Hardware
//
// Description:	This implements a simulated Telescope Hardware
//
// Implements:	ASCOM Telescope interface version: 2.0
// Author:		(rbt) Robert Turner <robert@robertturnerastro.com>
//              (pk) Paul Kanevsky <paul@pk.darkhorizons.org>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 07-JUL-2009	rbt	1.0.0	Initial edit, from ASCOM Telescope Driver template
// 28-JUL-2009  pk  1.0.1   Initial implementation of Gemini hardware layer and command processor
// --------------------------------------------------------------------------------
//

using System;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.Timers;


namespace ASCOM.GeminiTelescope
{
    /// <summary>
    /// Single serial command to be delivered to Gemini Hardware through worker thread queue
    /// </summary>
    public class CommandItem
    {
        internal string m_Command;  //actual serial command to be sent, not including ending '#' or the native checksum
        int m_ThreadID;             //this will record thread id of the calling thread
        internal int m_Timeout;     //timeout value for this command in msec, -1 if no timeout wanted

        internal System.Threading.ManualResetEvent m_WaitForResultHandle = null; // wait handle set by worker thread when result is received

        /// <summary>
        /// result produced by Gemini, or null if no result. Ending '#' is always stripped off
        /// </summary>
        public string m_Result { get; set; }    
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">actual serial command to be sent, not including ending '#' or the native checksum</param>
        /// <param name="timeout">timeout value for this command in msec, -1 if no timeout wanted</param>
        /// <param name="wantResult">does the caller want the result returned by Gemini?</param>
        public CommandItem(string command, int timeout, bool wantResult)
        {
            m_Command = command;
            m_ThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            m_Timeout = timeout;

            // create a wait handle if result is desired
            if (wantResult) 
                m_WaitForResultHandle = new System.Threading.ManualResetEvent(false);
            m_Result = null;
        }

        /// <summary>
        ///     Wait on the synchronization wait handle to signal that the result is now available
        ///     result is placed into m_sResult by the worker thread and the event is then signaled
        /// </summary>
        /// <returns>result produced by Gemini as after executing this command or null if timeout expired</returns>
        public string WaitForResult()
        {
            if (m_WaitForResultHandle != null)
            {
                if (m_Timeout > 0)
                {
                    if (m_WaitForResultHandle.WaitOne(m_Timeout))
                        return m_Result;
                    else
                        return null;
                }
                else
                    m_WaitForResultHandle.WaitOne();  // no timeout specified, wait indefinitely
            }
            return null;
        }
    }

    delegate void SystemMessageDelegate(string message);

    /// <summary>
    /// Class encapsulating all serial communications with Gemini
    /// </summary>
    public class TelescopeHardware
    {
        
        private static HelperNET.Profile m_Profile;
        private static HelperNET.Util m_Util;

        private static Queue m_CommandQueue; //Queue used for messages to the gemini
        private static System.ComponentModel.BackgroundWorker m_BackgroundWorker; // Thread to run for communications
      
        //Telescope Implementation
        
        private static double m_Latitude;
        private static double m_Longitude;
        private static double m_Elevation;
        
        private static double m_RightAscension;
        private static double m_Declination;


        private static bool m_Tracking;

        private static bool m_AtPark;

        private static bool m_SouthernHemisphere = false;

        private static string m_ComPort;
        private static int m_BaudRate;
        private static System.IO.Ports.Parity m_Parity;
        private static int m_DataBits;
        private static System.IO.Ports.StopBits m_StopBits;

        private static ASCOM.HelperNET.Serial m_SerialPort;

        private static bool m_Connected = false; //Keep track of the connection status of the hardware

        private static bool m_Started = false;  //need to know if profile variables have been read already

        private static int m_Clients;

        private static DateTime m_LastUpdate;
        private static object m_ConnectLock = new object();

        private static int m_QueryInterval = 1000;   // query mount for status this often, in msecs.

        /// <summary>
        ///  TelescopeHadrware constructor
        ///     create serial port
        ///     start background worker thread
        /// </summary>
        static TelescopeHardware()
        {
            m_Profile = new HelperNET.Profile();
            m_Util = new ASCOM.HelperNET.Util();

            m_SerialPort = new ASCOM.HelperNET.Serial();

            m_BackgroundWorker = new BackgroundWorker();
            m_BackgroundWorker.WorkerSupportsCancellation = true;
            m_BackgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            m_CommandQueue = new Queue();
            m_Clients = 0;            
        }


        /// <summary>
        ///  resets the driver, disconnects from Gemini, reloads all the variables from the profile
        /// </summary>
        private static void GetProfileSettings() 
        {
            if (m_Connected) Disconnect();

            m_Profile.Register(SharedResources.PROGRAM_ID, "Gemini Telescope Driver .NET");
            if (m_Profile.GetValue(SharedResources.PROGRAM_ID, "RegVer", "") != SharedResources.REGISTRATION_VERSION)
            {
                //Main Driver Settings
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "RegVer", SharedResources.REGISTRATION_VERSION);
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "ComPort", "COM1");
            }

            //Load up the values from saved
            m_ComPort = m_Profile.GetValue(SharedResources.PROGRAM_ID, "ComPort", "");
            if (!int.TryParse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "BaudRate", ""), out m_BaudRate))
                m_BaudRate = 9600;

            if (!int.TryParse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "DataBits", ""), out m_DataBits))
                m_DataBits = 8;

            int _parity = 0;
            if (!int.TryParse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "Parity", ""), out _parity))
                _parity = 0;

            m_Parity = (System.IO.Ports.Parity)_parity;

            int _stopbits = 8;
            if (!int.TryParse(m_Profile.GetValue(SharedResources.PROGRAM_ID, "StopBits", ""), out _stopbits))
                _stopbits = 1;

            m_StopBits = (System.IO.Ports.StopBits)_stopbits;

            if (m_ComPort != "")
            {
                m_SerialPort.PortName = m_ComPort;
            }

            m_SerialPort.Speed = (ASCOM.HelperNET.Serial.PortSpeed)m_BaudRate;
            m_SerialPort.Parity = (System.IO.Ports.Parity)m_Parity;
            m_SerialPort.DataBits = m_DataBits;
            m_SerialPort.StopBits = m_StopBits;

            m_Started = true;
        }

 

        #region Properties For Settings

        /// <summary>
        /// Get/Set serial comm port 
        /// </summary>
        public static string ComPort
        {
            get { return m_ComPort; }
            set 
            {
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "ComPort", value.ToString());
                m_ComPort = value; 
            }
        }
        /// <summary>
        /// Get/Set baud rate
        /// </summary>
        public static int BaudRate
        {
            get { return m_BaudRate; }
            set
            {
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "BaudRate", value.ToString());
                m_BaudRate = value;
            }
        }

        /// <summary>
        /// Get/Set parity
        /// </summary>
        public static System.IO.Ports.Parity Parity
        {
            get { return m_Parity; }
            set
            {
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Parity", value.ToString());
                m_Parity = value;
            }
        }

        /// <summary>
        /// Get/Set # of stop bits
        /// </summary>
        public static System.IO.Ports.StopBits  StopBits
        {
            get { return m_StopBits; }
            set
            {
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "StopBits", value.ToString());
                m_StopBits = value;
            }
        }

        /// <summary>
        /// Get/Set # of data bits
        /// </summary>
        public static int DataBits
        {
            get { return m_DataBits; }
            set
            {
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "DataBits", value.ToString());
                m_DataBits = value;
            }
        }

        /// <summary>
        /// Get/Set Elevation
        /// </summary>
        public static double Elevation
        {
            get { return m_Elevation; }
            set
            {
                m_Elevation = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Elevation", value.ToString());
            }
        }
        /// <summary>
        /// Get/Set Latitude
        /// </summary>
        public static double Latitude
        {
            get { return m_Latitude; }
            set
            {
                m_Latitude = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Latitude", value.ToString());
                if (m_Latitude < 0) { m_SouthernHemisphere = true; }
            }
        }

        /// <summary>
        /// Get/Set Longitude
        /// </summary>
        public static double Longitude
        {
            get { return m_Longitude; }
            set
            {
                m_Longitude = value;
                m_Profile.WriteValue(SharedResources.PROGRAM_ID, "Longitude", value.ToString());
            }
        }

        /// <summary>
        /// Execute standard serial command, wait for response from the mount, return it 
        /// </summary>
        /// <example>
        /// <code>
        /// // Get Altitude from Gemini with a 1 second timeout:
        /// double dAltitude = 0;
        /// 
        /// string sAlt = TelescopeHardware.DoCommandResult(":GA", 1000);
        ///
        /// if (!string.IsNullOrEmpty(sAlt))
        ///     dAltitude = NETHelper.DMSToDegrees(sAlt);
        /// </code>
        /// </example>
        /// <param name="cmd">command string to send to Gemini</param>
        /// <param name="timeout">in msecs, -1 if no timeout</param>
        /// <returns>result received from Gemini, or null if no result, timeout, or bad result received</returns>
        public static string DoCommandResult(string cmd, int timeout)
        {
            if (!m_Connected) return null;
            CommandItem ci = new CommandItem(cmd, timeout, true);
            QueueCommand(ci);
            return ci.WaitForResult();
        }


        /// <summary>
        /// Execute standard command, no result
        /// </summary>
        /// <example>
        /// <code>
        /// // Move to Home Position
        /// TelescopeHardware.DoCommand(":hP");
        /// </code>
        /// </example>
        /// <param name="cmd">command string to send to Gemini</param>
        public static void DoCommand(string cmd)
        {
            if (!m_Connected) return;
            CommandItem ci = new CommandItem(cmd, -1, false);
            QueueCommand(ci);
        }

        /// <summary>
        /// Execute native Gemini serial command, wait for response from the mount, return it 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// // Get Gemini Status with a 1 second timeout:
        /// double dAltitude = 0;
        /// 
        /// string sStatus = TelescopeHardware.DoCommandResult("<99:", 1000);
        /// ]]></code>
        /// </example>
        /// <param name="cmd">command string to send to Gemini</param>
        /// <param name="timeout">in msecs, -1 if no timeout</param>
        /// <returns>result received from Gemini, or null if no result, timeout, or bad result received</returns>
        public static string DoNativeCommandResult(string cmd, int timeout)
        {
            if (!m_Connected) return null;
            CommandItem ci = new CommandItem(cmd, timeout, true);
            QueueCommand(ci);
            return ci.WaitForResult();
        }

        /// <summary>
        /// Execute native command, no result
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// // Set mount type to G11:
        /// TelescopeHardware.DoNativeCommand(">2:");
        /// ]]></code>
        /// </example>
        /// <param name="cmd">native command to send to Gemini</param>
        public static void DoNativeCommand(string cmd)
        {
            if (!m_Connected) return;
            CommandItem ci = new CommandItem(cmd, -1, false);
            QueueCommand(ci);
        }
      
        #endregion

        #region Telescope Implementation

        /// <summary>
        /// Establish connection with the mount, open serial port
        /// </summary>
        private static void Connect()
        {
            lock (m_ConnectLock)   // make sure only one connection goes through at a time
            {
                m_Clients += 1;
                if (!m_SerialPort.Connected)
                {
                    GetProfileSettings();
                    m_SerialPort.PortName = m_ComPort;
                    m_SerialPort.Speed = (ASCOM.HelperNET.Serial.PortSpeed)m_BaudRate;
                    m_SerialPort.Parity = m_Parity;
                    m_SerialPort.DataBits = m_DataBits;
                    m_SerialPort.StopBits = m_StopBits;

                    m_SerialPort.DTREnable = true; //Set the DTR line high
                    m_SerialPort.Handshake = System.IO.Ports.Handshake.None; //Don't use hardware or software flow control on the serial line


                    try
                    {
                        m_SerialPort.Connected = true;
                    }
                    catch
                    {
                        m_Connected = false;
                        return;
                    }

                    m_CommandQueue.Clear();
                    m_BackgroundWorker.RunWorkerAsync();
                }

                m_Connected = true;
            }
        }

        /// <summary>
        /// Disconnect this client. If no more clients, close the port, disconnect from the mount.
        /// </summary>
        private static void Disconnect()
        {
            lock (m_ConnectLock)
            {
                m_Clients -= 1;
                if (m_Clients == 0)
                {
                    m_BackgroundWorker.CancelAsync();

                    int cnt = 0;
                    // wait for background worker to finish current operation and process the cancel request
                    System.Threading.Thread.Sleep(500);

                    m_SerialPort.Connected = false;
                    m_Connected = false;
                    m_Started = false;
                    m_CommandQueue.Clear();
                }
            }
        }

        /// <summary>
        /// Process queued up commands in the sequence queued.
        /// </summary>
        /// <param name="sender">sender - not used</param>
        /// <param name="e">work to perform - not used</param>
        private static void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            //SystemMessageDelegate message = new SystemMessageDelegate(ProcessMessage);

            while (!worker.CancellationPending)
            {
                System.Threading.EventWaitHandle ewh = null;

                try
                {

                    CommandItem command = null;

                    lock (m_CommandQueue)
                    {
                        if (m_CommandQueue.Count > 0)
                            command = (CommandItem)m_CommandQueue.Dequeue();
                    }

                    if (command != null)    // got a new command, send it to the mount
                    {
                        ewh = command.m_WaitForResultHandle;
                        command.m_Result = null;

                        string serial_cmd = string.Empty;

                        // native Gemini command?
                        if (command.m_Command.StartsWith("<") || command.m_Command.StartsWith(">"))
                            serial_cmd = CompleteNativeCommand(command.m_Command);
                        else
                            serial_cmd = CompleteStandardCommand(command.m_Command);

                        m_SerialPort.ClearBuffers(); //clear all received data
                        m_SerialPort.Transmit(serial_cmd);

                        if (command.m_WaitForResultHandle != null)    // receive result, if one is expected
                        {
                            command.m_Result = GetCommandResult(command);
                            command.m_WaitForResultHandle.Set();   //release the wait handle so the calling thread can continue
                            ewh = null;
                        }
                    }
                    else
                    {
                        //Get RA and DEC etc
                        m_SerialPort.ClearBuffers(); //clear all received data
                        m_SerialPort.Transmit(":GR#:GD#");

                        command = new CommandItem(":GR", m_QueryInterval, true);
                        string RA = GetCommandResult(command);

                        command = new CommandItem(":GD", m_QueryInterval, true);
                        string DEC = GetCommandResult(command);

                        if (RA != null && DEC!=null) {
                            m_RightAscension = m_Util.HMSToDegrees(RA);
                            m_Declination = m_Util.DMSToDegrees(DEC);
                            m_LastUpdate = System.DateTime.Now;
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    if (ewh!=null) ewh.Set();   //release the wait handle so the calling thread can continue
                }

                if (m_CommandQueue.Count==0)    // wait specified interval before querying the mount if no more commands...
                    System.Threading.Thread.Sleep(m_QueryInterval); // don't tie up the CPU and serial port, sleep a little
            }
        }

        /// <summary>
        /// Wait for a proper response from Gemini for a given command. Command has already been sent.
        /// 
        /// </summary>
        /// <param name="command">actual command sent to Gemini</param>
        /// <returns>result received from Gemini, or null if no result, timeout, or bad result received</returns>
        private static string GetCommandResult(CommandItem command)
        {
            string result = null;

            try
            {

                //System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(100);

                GeminiCommand gmc = null;

                GeminiCommand.ResultType gemini_result = GeminiCommand.ResultType.HashChar;
                int char_count = 0;

                if (GeminiCommands.Commands.ContainsKey(command.m_Command))
                {
                    gmc = GeminiCommands.Commands[command.m_Command];
                    gemini_result = gmc.Type;
                    char_count = gmc.Chars;
                }

                if (command.m_Timeout > 0)
                    m_SerialPort.ReceiveTimeoutMs = command.m_Timeout;
                else
                    m_SerialPort.ReceiveTimeoutMs = System.IO.Ports.SerialPort.InfiniteTimeout;

                switch (gemini_result)
                {
                    // no result expected by this command
                    case GeminiCommand.ResultType.NoResult: return null;

                    // a specific number of characters expected as the return value
                    case GeminiCommand.ResultType.NumberofChars:
                        result = m_SerialPort.ReceiveCounted(char_count);
                        break;

                    // value '1' or a string terminated by '#'
                    case GeminiCommand.ResultType.OneOrHash:
                        result = m_SerialPort.ReceiveCounted(1);  // check if first character is 1, and return if it is, no hash expected
                        if (result != "1")
                            result += m_SerialPort.ReceiveTerminated("#");
                        break;

                    // value '0' or a string terminated by '#'
                    case GeminiCommand.ResultType.ZeroOrHash:
                        result = m_SerialPort.ReceiveCounted(1);
                        if (result != "0")
                            result += m_SerialPort.ReceiveTerminated("#");
                        break;

                    // value '0' or a string terminated by '#'
                    case GeminiCommand.ResultType.HashChar:
                        result = m_SerialPort.ReceiveTerminated("#");
                        break;
                }
            }
            catch (Exception e) //timeout or some other communication value
            {
                System.Diagnostics.Trace.WriteLine("Command timed out: " + command.m_Command);
                return null;
            }

            // return value for native commands has a checksum appended: validate it and remove it from the return string:
            if (command.m_Command.StartsWith("<") && result!=null && result.Length > 0)
            {
                char chksum = result[result.Length - 2];
                result = result.Substring(0, result.Length - 2); //remove checksum character

                if (chksum != ComputeChecksum(result))  // bad checksum -- ignore the return value!
                    result = null;
            }
            return result;
        }

        /// <summary>
        /// Get/Set serial port connection state
        /// </summary>
        public static bool Connected
        {
            get
            { 
                return m_Connected; 
            }
            set
            {
                if (value)
                    Connect();
                else
                    Disconnect();
            }
        }

        /// <summary>
        /// Get SouthernHemisphere state
        /// </summary>
       public static bool SouthernHemisphere
       { get { return m_SouthernHemisphere; } }


        /// <summary>
        /// Get current RightAscention propery
        /// retrieved from the latest polled value from the mount, no actual command is executed
        /// </summary>
        public static double RightAscension
       { get { return m_RightAscension; } }

        /// <summary>
        /// Get current Declination propery
        /// retrieved from the latest polled value from the mount, no actual command is executed
        /// </summary>
        public static double Declination
        { get { return m_Declination; } }
        #endregion

        /// <summary>
        /// Get the time the RA/DEC values were last updated from the mount
        /// </summary>
        public static DateTime LastUpdate
        { get { return m_LastUpdate; } }

        
        #region Helper Functions

        /// <summary>
        /// Finish off the formatting of a standard (LX200) command to be sent to the mount. Usually means appending '#' at the end.
        /// </summary>
        /// <param name="p">standard command to complete, not including '#' at the end</param>
        /// <returns>completed command to send to the mount</returns>
        private static string CompleteStandardCommand(string p)
        {
            return p + "#"; // standard commands end in '#' character, no checksum needed
        }

        /// <summary>
        /// Complete native Gemini command. Involves appending a checksum and a '#' at the end.
        /// </summary>
        /// <param name="p">command to complete</param>
        /// <returns>completed command to send to the mount</returns>
        private static string CompleteNativeCommand(string p)
        {
            return p + ComputeChecksum(p) + "#";
        }

        /// <summary>
        /// Gemini Native command checksum
        /// </summary>
        /// <param name="p">command to compute checksum for</param>
        /// <returns>Gemini checksum character</returns>
        private static char ComputeChecksum(string p)
        {
            int chksum = 0;

            for (int i = 0; i < p.Length; ++i)
                chksum ^= p[i];

            return (char)((chksum & 0x7f) + 0x40);
        }

        /// <summary>
        /// Add command item 'ci' to the queue for execution
        /// </summary>
        /// <param name="ci">actual command to queue</param>
        private static void QueueCommand(CommandItem ci)
        {
            lock (m_CommandQueue)
            {
                m_CommandQueue.Enqueue(ci);
            }
        }


        #endregion

    }
}
