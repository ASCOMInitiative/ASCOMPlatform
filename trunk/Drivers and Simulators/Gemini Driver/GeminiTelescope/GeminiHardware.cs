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
// 29-JUL-2009  pk  1.0.1   Added DoCommandAsync asynchronous call-back version of the command processor
//                          Added array versions of DoCommand functions for multiple command execution
// 31-JUL-2009  rbt 1.0.1   Added Focuser Implementations and Settings
// --------------------------------------------------------------------------------
//

using System;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.Timers;
using System.IO.Ports;

namespace ASCOM.GeminiTelescope
{

    /// <summary>
    /// Async delegate callback for DoCommandAsync
    /// </summary>
    /// <param name="cmd">original command string passed to DoCommandAsync</param>
    /// <param name="result">return result from Gemini, or null if timeout exceeded</param>
    public delegate void HardwareAsyncDelegate(string cmd, string result);


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
        internal CommandItem(string command, int timeout, bool wantResult) : this(command,timeout,wantResult, false)
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
                        return null;
                }
                else
                    m_WaitForResultHandle.WaitOne();  // no timeout specified, wait indefinitely
            }
            return null;
        }
    }


    /// <summary>
    /// Class encapsulating all serial communications with Gemini
    /// </summary>
    public static class GeminiHardware
    {
        
        public static ASCOM.Utilities.Profile m_Profile;
        public static ASCOM.Utilities.Util m_Util;
        public static ASCOM.Astrometry.Transform.Transform  m_Transform;

        private static Queue m_CommandQueue; //Queue used for messages to the gemini
        private static System.Threading.Thread m_BackgroundWorker; // Thread to run for communications

        private static bool m_CancelAsync = false; // when to stop the background thread
 

        //Telescope Implementation
        
        private static double m_Latitude;
        private static double m_Longitude;
        private static double m_Elevation;

        private static int m_UTCOffset;

        private static double m_RightAscension;
        private static double m_Declination;
        private static double m_Altitude;
        private static double m_Azimuth;
        private static double m_TargetRightAscension;
        private static double m_TargetDeclination = SharedResources.INVALID_DOUBLE;
        private static double m_SiderealTime;
        private static string m_Velocity;
        private static string m_SideOfPier;
        private static double m_TargetAltitude;
        private static double m_TargetAzimuth;

        private static bool m_AdditionalAlign;
        private static bool m_Precession;
        private static bool m_Refraction;
        private static bool m_GeminiTime;
        private static bool m_GeminiSite;
        private static bool m_AdvancedMode;


        private static bool m_Tracking;

        private static bool m_AtPark;
        private static bool m_AtHome;
        private static string m_ParkState = "";

        private static bool m_IsPulseGuiding = false;
     
        private static bool m_SouthernHemisphere = false;

        private static string m_ComPort;
        private static int m_BaudRate;
        private static ASCOM.Utilities.SerialParity m_Parity;
        private static int m_DataBits;
        private static ASCOM.Utilities.SerialStopBits m_StopBits;

        private static System.Threading.AutoResetEvent m_WaitForCommand;

        private static string m_PolledVariablesString = ":GR#:GD#:GA#:GZ#:Gv#:GS#:Gm#:h?#";
        
        static Timer tmrReadTimeout = new Timer();
        static System.Threading.AutoResetEvent m_SerialTimeoutExpired = new System.Threading.AutoResetEvent(false);
        static System.Threading.AutoResetEvent m_SerialErrorOccurred = new System.Threading.AutoResetEvent(false);

        public enum GeminiBootMode
        {
            Prompt = 0,
            ColdStart = 1,
            WarmStart = 2,
            WarmRestart = 3,
        }
        ;
        private static GeminiBootMode m_BootMode = GeminiBootMode.Prompt; 

        private static SerialPort m_SerialPort;

        private static bool m_Connected = false; //Keep track of the connection status of the hardware

        private static int m_Clients;

        private static DateTime m_LastUpdate;
        private static object m_ConnectLock = new object();

        private static int m_QueryInterval = 1000;   // query mount for status this often, in msecs.


        //Focuser Private Data
        private static int m_MaxIncrement = 0;
        private static int m_MaxStep = 0;
        private static int m_StepSize = 0;
        private static bool m_ReverseDirection = false;
        private static int m_BacklashDirection = 0;
        private static int m_BacklashSize = 0;
        private static int m_BrakeSize = 0;
        private static int m_Speed = 1;

        /// <summary>
        ///  TelescopeHadrware constructor
        ///     create serial port
        /// </summary>
        static GeminiHardware()
        {
            m_Profile = new ASCOM.Utilities.Profile();
            m_Util = new ASCOM.Utilities.Util();
            m_Transform = new ASCOM.Astrometry.Transform.Transform();

            m_SerialPort = new SerialPort();
            m_CommandQueue = new Queue();
            m_Clients = 0;


            m_WaitForCommand = new System.Threading.AutoResetEvent(false);

            tmrReadTimeout.AutoReset = false;            
            tmrReadTimeout.Elapsed += new ElapsedEventHandler(tmrReadTimeout_Elapsed);

            GetProfileSettings();
        }


        /// <summary>
        ///  reloads all the variables from the profile
        /// </summary>
        private static void GetProfileSettings() 
        {
            //Telescope Settings
            m_Profile.DeviceType = "Telescope";
            if (m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "RegVer", "") != SharedResources.REGISTRATION_VERSION)
            {
                //Main Driver Settings
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "RegVer", SharedResources.REGISTRATION_VERSION);
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "ComPort", "COM1");
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "BaudRate", "9600");
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "AdditionalAlign", "false");
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Precession", "false");
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Refraction", "false");
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "AdvancedMode", "false");
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "GeminiSite", "true");
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "GeminiTime", "true");
            }

            //Load up the values from saved
            if (!bool.TryParse(m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "AdditionalAlign", ""), out m_AdditionalAlign))
                m_AdditionalAlign = false;

            if (!bool.TryParse(m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Precession", ""), out m_Precession))
                m_Precession = false;
            if (!bool.TryParse(m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Refraction", ""), out m_Refraction))
                m_Refraction = false;
            if (!bool.TryParse(m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "AdvancedMode", ""), out m_AdvancedMode))
                m_AdvancedMode = false;
            if (!bool.TryParse(m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "GeminiSite", ""), out m_GeminiSite))
                m_GeminiSite= false;
            if (!bool.TryParse(m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "GeminiTime", ""), out m_GeminiTime))
                m_GeminiTime = false;

            m_ComPort = m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "ComPort", "");
            if (!int.TryParse(m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "BaudRate", ""), out m_BaudRate))
                m_BaudRate = 9600;

            if (!int.TryParse(m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "DataBits", ""), out m_DataBits))
                m_DataBits = 8;

            int _parity = 0;
            if (!int.TryParse(m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Parity", ""), out _parity))
                _parity = 0;

            m_Parity = (ASCOM.Utilities.SerialParity)_parity;

            int _stopbits = 8;
            if (!int.TryParse(m_Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "StopBits", ""), out _stopbits))
                _stopbits = 1;

            m_StopBits = (ASCOM.Utilities.SerialStopBits)_stopbits;


            if (m_ComPort != "")
            {
                m_SerialPort.PortName = m_ComPort;
            }

            //Focuser Settings
            m_Profile.DeviceType = "Focuser";
            
            if (m_Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "RegVer", "") != SharedResources.REGISTRATION_VERSION)
            {
                //Main Driver Settings
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "RegVer", SharedResources.REGISTRATION_VERSION);
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "MaxIncrement", "5000");
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "StepSize", "100");
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "ReverseDirection", "false");
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "BacklashDirection", "0");
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "BacklashSize", "50");
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "BrakeSize", "0");
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "Speed", "1");
            }

            string s = m_Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "MaxIncrement");
            if (!int.TryParse(s, out m_MaxIncrement) || m_MaxIncrement <= 0)
                m_MaxIncrement = 5000;

            s = m_Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "MaxStep");
            //if (!int.TryParse(s, out m_MaxStep) || m_MaxStep <= 0)
            m_MaxStep = 0x7fffffff;

            s = m_Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "StepSize");
            if (!int.TryParse(s, out m_StepSize) || m_StepSize <= 0)
                m_StepSize = 100;

            s = m_Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "ReverseDirection");
            if (!bool.TryParse(s, out m_ReverseDirection))
                m_ReverseDirection = false;

            s = m_Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "BacklashDirection");
            if (!int.TryParse(s, out m_BacklashDirection) || m_BacklashDirection < -1 || m_BacklashDirection > 1)
                m_BacklashDirection = 0;

            s = m_Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "BacklashSize");
            if (!int.TryParse(s, out m_BacklashSize) || m_BacklashSize < 0)
                m_BacklashSize = 0;

            s = m_Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "BrakeSize");
            if (!int.TryParse(s, out m_BrakeSize) || m_BrakeSize < 0)
                m_BrakeSize = 0;

            s = m_Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "Speed");
            if (!int.TryParse(s, out m_Speed) || m_Speed < 1 || m_Speed > 3)
                m_Speed = 1;

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
                m_Profile.DeviceType = "Telescope";
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "ComPort", value.ToString());
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
                m_Profile.DeviceType = "Telescope";
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "BaudRate", value.ToString());
                m_BaudRate = value;
            }
        }

        /// <summary>
        /// Get/Set parity
        /// </summary>
        public static ASCOM.Utilities.SerialParity Parity
        {
            get { return m_Parity; }
            set
            {
                m_Profile.DeviceType = "Telescope";
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Parity", value.ToString());
                m_Parity = value;
            }
        }

        /// <summary>
        /// Get/Set # of stop bits
        /// </summary>
        public static ASCOM.Utilities.SerialStopBits StopBits
        {
            get { return m_StopBits; }
            set
            {
                m_Profile.DeviceType = "Telescope";
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "StopBits", value.ToString());
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
                m_Profile.DeviceType = "Telescope";
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "DataBits", value.ToString());
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
                m_Profile.DeviceType = "Telescope";
                m_Elevation = value;
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Elevation", value.ToString());
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
                m_Profile.DeviceType = "Telescope";
                m_Latitude = value;
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Latitude", value.ToString());
                if (m_Latitude < 0) { m_SouthernHemisphere = true; }
                m_Transform.SiteLatitude = m_Latitude;
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
                m_Profile.DeviceType = "Telescope";
                m_Longitude = value;
                m_Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Longitude", value.ToString());
                m_Transform.SiteLongitude = m_Longitude;
            }
        }

        /// <summary>
        /// Execute a single serial command, block and wait for the response from the mount, return it 
        /// </summary>
        /// <example>
        /// <code>
        /// // Get Altitude from Gemini with a 1 second timeout:
        /// double dAltitude = 0;
        /// 
        /// string sAlt = GeminiHardware.DoCommandResult(":GA", 1000, false);
        ///
        /// if (!string.IsNullOrEmpty(sAlt))
        ///     dAltitude = NETHelper.DMSToDegrees(sAlt);
        /// </code>
        /// </example>
        /// <param name="cmd">command string to send to Gemini</param>
        /// <param name="timeout">in msecs, -1 if no timeout</param>
        /// <param name="bRaw">command is a raw string to be passed to the device unmodified</param>
        /// <returns>result received from Gemini, or null if no result, timeout, or bad result received</returns>
        public static string DoCommandResult(string cmd, int timeout, bool bRaw)
        {
            return DoCommandResult(new string[] { cmd }, timeout, bRaw);
        }


        /// <summary>
        /// Execute standard command, no result, no blocking
        /// </summary>
        /// <example>
        /// <code>
        /// // Move to Home Position
        /// GeminiHardware.DoCommand(":hP", false);
        /// </code>
        /// </example>
        /// <param name="cmd">command string to send to Gemini</param>
        /// <param name="bRaw">command is a raw string to be passed to the device unmodified</param>
        public static void DoCommand(string cmd, bool bRaw)
        {
            DoCommand(new string[] { cmd }, bRaw);
        }

        /// <summary>
        /// Execute an array of command in sequence, no return expected.  Commands guaranteed to be executed in the sequence specified, with no interruptions from other threads.
        /// </summary>
        /// <param name="cmd">array of commands to execute, element 0 will be executed first</param>
        /// <param name="bRaw">command is a raw string to be passed to the device unmodified</param>
        public static void DoCommand(string [] cmd, bool bRaw)
        {
            if (!m_Connected) return;
            CommandItem[] ci = new CommandItem[cmd.Length];

            for (int i = 0; i < ci.Length; ++i)
                ci[i] = new CommandItem(cmd[i], -1, false, bRaw);

            QueueCommands(ci);
        }


        /// <summary>
        /// Executes a command and returns immediately. The callback function is called 
        /// when Gemini returns a result, or when the timeout value expires.
        /// 
        /// Spawns a background thread that waits for the command execution result.
        /// Callback delegate is defined as:
        /// 
        ///    public delegate void HardwareAsyncDelegate(string cmd, string result);
        /// </summary>
        /// <param name="cmd">command to send to Gemini</param>
        /// <param name="timeout">timeout value in msec, or -1 for no timeout</param>
        /// <param name="callback">callback delegate will be called with the result
        ///     public delegate void HardwareAsyncDelegate(string cmd, string result);
        /// </param>
        /// <param name="bRaw">command is a raw string to be passed to the device unmodified</param>
        public static void DoCommandAsync(string cmd, int timeout, HardwareAsyncDelegate callback, bool bRaw)
        {
            CommandItem ci = new CommandItem(cmd, timeout, callback, bRaw);
            System.Threading.ThreadPool.QueueUserWorkItem(DoCommandAndWaitAsync, ci);
        }

        /// <summary>
        /// Execute a sequence of commands guaranteed not to be interrupted by another ASCOM client or another thread
        ///   
        ///   
        ///   The result in the case of successfull completion is the Gemini generated result for
        ///   the last command
        ///   
        ///   In the case of a timeout, the result passed into the callback is 'null'.        
        /// </summary>
        /// <param name="cmd">an array of commands to execute</param>
        /// <param name="timeout">total timeout for the whole sequence, in msec, or -1 for no timeout</param>
        /// <returns>the result of the last command in the array if the sequence was successfully completed,
        /// otherwise 'null'.
        /// </returns>
        /// <param name="bRaw">command is a raw string to be passed to the device unmodified</param>
        public static string DoCommandResult(string [] cmd, int timeout, bool bRaw)
        {
            if (!m_Connected) return null;

            CommandItem[] ci = new CommandItem[cmd.Length];

            for (int i = 0; i < ci.Length; ++i)
                ci[i] = new CommandItem(cmd[i], timeout, true, bRaw); //initialize all CommandItem objects

            QueueCommands(ci);  // queue them all at once

            // construct an array of all the wait handles
            System.Threading.ManualResetEvent[] events = new System.Threading.ManualResetEvent[ci.Length];
            for (int i = 0; i < ci.Length; ++i) events[i] = ci[i].WaitObject;

            // success only if all wait handles are signalled by the worker thread. Return result from the last command:
            if (System.Threading.ManualResetEvent.WaitAll(events, timeout)) 
                return ci[ci.Length-1].m_Result;

            return null;
        }


        /// <summary>
        /// Execute a sequence of commands guaranteed not to be interrupted by another ASCOM client or another thread
        ///   
        ///   the callback is called if the sequence times out, or if all the commands are performed successfully
        ///   the result passed back to the callback in the case of successfull completion is the command string for
        ///   the last command, and the result received from Gemini after executing this command
        ///   
        ///   in the case of a timeout, the result passed into the callback is 'null'.
        /// </summary>
        /// <param name="cmd">an array of commands to execute</param>
        /// <param name="timeout">total timeout for the whole sequence, in msec, or -1 for no timeout</param>
        /// <param name="callback">asynchrnous callback delegate to call when the sequence completes
        ///     public delegate void HardwareAsyncDelegate(string cmd, string result);
        /// </param>
        /// <param name="bRaw">command is a raw string to be passed to the device unmodified</param>
        public static void DoCommandAsync(string[] cmd, int timeout, HardwareAsyncDelegate callback, bool bRaw)
        {
            CommandItem[] ci = new CommandItem[cmd.Length];

            for (int i = 0; i < ci.Length; ++i)
                ci[i] = new CommandItem(cmd[i], timeout, callback, bRaw);

            System.Threading.ThreadPool.QueueUserWorkItem(DoCommandsAndWaitAsync, ci);
        }


        /// <summary>
        /// executed by a worker thread for an asynchronous command call-back
        /// </summary>
        /// <param name="command_item">CommandItem type containing command to execute</param>
        private static void DoCommandAndWaitAsync(object command_item)
        {
            CommandItem ci = (CommandItem)command_item;
            QueueCommand(ci);
            if (ci.m_AsyncDelegate!=null)
                try
                {
                    string result = ci.WaitForResult();
                    ci.m_AsyncDelegate(ci.m_Command, result);
                }
                catch { }
        }

        /// <summary>
        /// Executed by a worker thread for an asynchronous command call-back
        /// Executes and waits for multiple commands to complete, then fires a callback.
        /// 
        /// All commands are executed in sequence.
        /// If timeout is exceeded, the callback receives the last command string, along with 'null' for result
        /// if all commands successfully execute and all return proper values,
        ///   the callback will receive the last command string and the result produced by the last command
        /// </summary>
        /// <param name="command_items">CommandItems array containing commands to execute</param>
        private static void DoCommandsAndWaitAsync(object command_items)
        {
            CommandItem [] ci = (CommandItem[])command_items;

            QueueCommands(ci);

            System.Threading.ManualResetEvent[] events = new System.Threading.ManualResetEvent[ci.Length];

            for (int i = 0; i < ci.Length; ++i) events[i] = ci[i].WaitObject;

            bool bAllDone = (System.Threading.ManualResetEvent.WaitAll(events, ci[0].m_Timeout));

            try
            {
                CommandItem last_ci = ci[ci.Length - 1];
                if (bAllDone)
                    last_ci.m_AsyncDelegate(last_ci.m_Command, last_ci.m_Result);
                else
                    last_ci.m_AsyncDelegate(last_ci.m_Command, null);
            }
            catch { }
        }


        public static bool IsPulseGuiding
        {
            get { return m_IsPulseGuiding; }
            set
            {
                m_IsPulseGuiding = value;
            }
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
                if (!m_SerialPort.IsOpen)
                {                   
                    GetProfileSettings();
                    m_SerialPort.PortName = m_ComPort;
                    m_SerialPort.BaudRate = m_BaudRate;
                    m_SerialPort.Parity = (System.IO.Ports.Parity)m_Parity;
                    m_SerialPort.DataBits = m_DataBits;
                    m_SerialPort.StopBits = (System.IO.Ports.StopBits)m_StopBits;              
                    m_SerialPort.Handshake = System.IO.Ports.Handshake.None;
                    m_SerialPort.ReadBufferSize = 256;
                    //m_SerialPort.WriteBufferSize = 256;

                    m_SerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(m_SerialPort_ErrorReceived);

                    try
                    {
                        m_SerialPort.Open();
                        m_SerialPort.DtrEnable = true;
                    }
                    catch
                    {
                        m_Connected = false;
                        return;
                    }

                    lock(m_CommandQueue) 
                        m_CommandQueue.Clear();

                    // process initial ping to Gemini
                    // and take it through the boot process if it's not booted yet
                    // it all succeeds, set connected state and start worker thread

                    if (StartGemini())
                    {
                        m_Connected = true;
                        UpdateInitialVariables();

                        m_WaitForCommand.Reset();
                        m_BackgroundWorker = new System.Threading.Thread(BackgroundWorker_DoWork);
                        m_BackgroundWorker.Start();
                    }
                    else
                    {
                        Disconnect();
                    }

                    if (m_Connected)
                    {
                    }
                }

            }
        }

        static void m_SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            m_SerialErrorOccurred.Set();    //signal that an error has occurred
        }

        public static void Transmit(string s)
        {
            m_SerialErrorOccurred.Reset();

            if (m_SerialPort.IsOpen)
            {
                m_SerialPort.Write(s);
                //m_SerialPort.BaseStream.Flush();
            }
            if (m_SerialErrorOccurred.WaitOne(0)) throw new TimeoutException("Serial port transmission error");
        }


        /// <summary>
        /// Check and process the initialization status of Gemini on initial connect:
        ///   return true if already started and initialized,
        ///   otherwise, perform initialization based on configured options
        /// </summary>
        /// <param name="sRes">value returned by Gemini to the ^G command</param>
        /// <returns></returns>
        private static bool StartGemini()
        {
            Transmit("\x6");
            CommandItem ci = new CommandItem("\x6", 3000, true);
            string sRes = GetCommandResult(ci);

            // scrolling message? wait for it to end:
            while (sRes == "B")
            {
                System.Threading.Thread.Sleep(500);
                Transmit("\x6");
                ci = new CommandItem("\x6", 3000, true);
                sRes = GetCommandResult(ci); ;
            }

            if (sRes == "b")    //Gemini waiting for user to select the boot mode
            {

                GeminiBootMode bootMode = m_BootMode;

                //ask the user what mode to boot up in
                if (bootMode == GeminiBootMode.Prompt)
                {
                    frmBootMode frmBoot = new frmBootMode();
                    System.Windows.Forms.DialogResult res = frmBoot.ShowDialog();
                    if (res == System.Windows.Forms.DialogResult.OK)
                        bootMode = frmBoot.BootMode;
                    else
                        return false;   // not started, the user chose to cancel
                }


                switch (bootMode)
                {
                    case GeminiBootMode.ColdStart: Transmit("bC#"); break;
                    case GeminiBootMode.WarmRestart: Transmit("bR#"); break;
                    case GeminiBootMode.WarmStart: Transmit("bW#"); break;
                }
                sRes = "S"; // put it into "starting" mode, so the next loop will wait for full initialization
            }

            // processing Cold start mode -- wait for this to end
            while (sRes == "S")
            {
                System.Threading.Thread.Sleep(500);
                Transmit("\x6");
                ci = new CommandItem("\x6", 3000, true);
                sRes = GetCommandResult(ci); ;
            }

            return sRes == "G"; // true if startup completed, otherwise false
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
                    m_CancelAsync = true;

                    m_BackgroundWorker = null;
                    m_SerialPort.Close();
                    m_Connected = false;

                    lock (m_CommandQueue)
                        m_CommandQueue.Clear();

                    // wait for the thread to die for 2 seconds,
                    // then kill it -- we don't want to tie up the serial comm
                    if (m_BackgroundWorker != null)
                    {
                        m_WaitForCommand.Set(); // wake up the background thread
                        if (!m_BackgroundWorker.Join(5000))
                            m_BackgroundWorker.Abort();
                    }

                    lock (m_CommandQueue)
                        m_CommandQueue.Clear();
                }
            }
        }

        /// <summary>
        /// Process queued up commands in the sequence queued.
        /// </summary>
        /// <param name="sender">sender - not used</param>
        /// <param name="e">work to perform - not used</param>
        private static void BackgroundWorker_DoWork()
        {           
            while (!m_CancelAsync)
            {
                System.Threading.EventWaitHandle ewh = null;
                object [] commands = null;

                lock (m_CommandQueue)
                {
                    if (m_CommandQueue.Count > 0)
                    {
                        // gemini doesn't like too many long commands (buffer size problem?)
                        // remove up to x commands at a time
                        int cnt = Math.Min(1, m_CommandQueue.Count);

                        commands = new object[cnt]; // m_CommandQueue.ToArray();
                        for (int i = 0; i < cnt; ++i) commands[i] = m_CommandQueue.Dequeue();
                    }
                }

                try
                {
                    if (commands != null)    // got a new command, send it to the mount
                    {
                        string all_commands = String.Empty;

                        bool bNeedStatusUpdate = false;

                        foreach (CommandItem ci in commands)
                        {
                            ci.m_Result = null;
                            string serial_cmd = ci.m_Command;

                            // raw commands are passed to hardware unmodified:
                            if (!ci.m_Raw)
                            {
                                // native Gemini command?
                                if (ci.m_Command[0] == '<' || ci.m_Command[0] == '>')
                                    serial_cmd = CompleteNativeCommand(ci.m_Command);
                                else
                                    serial_cmd = CompleteStandardCommand(ci.m_Command);
                            }                            
                            all_commands += serial_cmd;
                            if (ci.WaitObject == null) ci.m_Timeout = 1000;    // default timeout of one second for requests where the user doesn't care
                        }

                        m_SerialPort.DiscardInBuffer(); //clear all received data

                        System.Diagnostics.Trace.Write("Command '" + all_commands + "' ");

#if DEBUG
                        int startTime = System.Environment.TickCount;
#endif

                        Transmit(all_commands);

                        foreach (CommandItem ci in commands)
                        {
                            if (ci.WaitObject != null)
                                System.Diagnostics.Trace.Write("..result expected..");
                            else
                                System.Diagnostics.Trace.Write("..result not expected..");

                            System.Diagnostics.Trace.Write("waiting... ");

                            // wait for the result whether or not the caller wants it
                            // otherwise delayed result from a previous command
                            // can be falsely returned for a later request:

                            string result = GetCommandResult(ci);
                            
                            System.Diagnostics.Trace.Write("result='" + (result ?? "null") + "'");

                            if (ci.WaitObject != null)    // receive result, if one is expected
                            {
                                ci.m_Result = result;
                                ci.WaitObject.Set();   //release the wait handle so the calling thread can continue
                            }

                            // if this command is one of the status variables to be polled, make a note to update 
                            // status ASAP!
                            if (ci.m_UpdateRequired) bNeedStatusUpdate = true;
                        }

#if DEBUG
                        System.Diagnostics.Trace.WriteLine(" done in " + (System.Environment.TickCount - startTime).ToString() + " msecs");
#else
                        System.Diagnostics.Trace.WriteLine(" done!");
#endif
                        if (bNeedStatusUpdate) UpdatePolledVariables(); //update variables if one of them was altered by a processed command
                    }
                    else
                        UpdatePolledVariables();

                }
                catch
                {
                }
                finally
                {
                    // release all pending commands
                    if (commands != null)
                        foreach (CommandItem ci in commands)
                            if (ci.WaitObject != null)
                                ci.WaitObject.Set();
                }

                // wait specified interval before querying the mount if no more commands, but
                // wake up immediately if a new command has been posted                    
                m_WaitForCommand.WaitOne(m_QueryInterval);
            }

            m_CancelAsync = false;
        }

        /// <summary>
        /// update all variable sthat are polled on an interval
        /// </summary>
        private static void UpdateInitialVariables()
        {
            UpdatePolledVariables();
            CommandItem command;

            //Get RA and DEC etc
            m_SerialPort.DiscardInBuffer(); //clear all received data
            //longitude, latitude, UTC offset
            Transmit(":Gg#:Gt#:GG#");

            command = new CommandItem(":Gg", 2000, true);
            string longitude = GetCommandResult(command);

            command = new CommandItem(":Gt", 2000, true);
            string latitude = GetCommandResult(command);

            command = new CommandItem(":GG", 2000, true);
            string UTC_Offset = GetCommandResult(command);


            if (longitude != null) Longitude = m_Util.DMSToDegrees(longitude);
            if (latitude != null) Latitude = m_Util.DMSToDegrees(latitude);
            if (UTC_Offset != null) int.TryParse(UTC_Offset, out m_UTCOffset);
           

            m_LastUpdate = System.DateTime.Now;
        }



        /// <summary>
        /// update all variable sthat are polled on an interval
        /// </summary>
        private static void UpdatePolledVariables()
        {
            CommandItem command;

            //Get RA and DEC etc
            m_SerialPort.DiscardInBuffer(); //clear all received data
            Transmit(m_PolledVariablesString);

            command = new CommandItem(":GR", m_QueryInterval, true);
            string RA = GetCommandResult(command);

            command = new CommandItem(":GD", m_QueryInterval, true);
            string DEC = GetCommandResult(command);

            command = new CommandItem(":GA", m_QueryInterval, true);
            string ALT = GetCommandResult(command);

            command = new CommandItem(":GZ", m_QueryInterval, true);
            string AZ = GetCommandResult(command);

            command = new CommandItem(":Gv", m_QueryInterval, true);
            string V = GetCommandResult(command);

            command = new CommandItem(":GS", m_QueryInterval, true);
            string ST = GetCommandResult(command);

            command = new CommandItem(":Gm", m_QueryInterval, true);
            string SOP = GetCommandResult(command);

            command = new CommandItem(":h?", m_QueryInterval, true);
            string HOME = GetCommandResult(command);

            if (RA != null) m_RightAscension = m_Util.HMSToHours(RA);

            if (DEC != null) m_Declination = m_Util.DMSToDegrees(DEC);

            if (ALT != null) m_Altitude = m_Util.DMSToDegrees(ALT);

            if (AZ != null) m_Azimuth = m_Util.DMSToDegrees(AZ);
            if (V != null) m_Velocity = V;

            if (Velocity != "T") m_Tracking = false;
            else
                m_Tracking = true;

            if (ST != null)
            {
                m_SiderealTime = m_Util.HMSToHours(ST);              
            }
            if (SOP != null) m_SideOfPier = SOP;

            if (HOME != null)
            {
                m_ParkState = HOME;
                if (HOME == "1")
                {
                    m_AtHome = true;
                    if (Velocity == "N") m_AtPark = true;
                    else
                    {
                        m_AtPark = false;
                    }
                }
                else
                {
                    m_AtHome = false;
                    m_AtPark = false;
                }
            }
            m_LastUpdate = System.DateTime.Now;
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

            if (!m_SerialPort.IsOpen) return null;

            GeminiCommand.ResultType gemini_result = GeminiCommand.ResultType.HashChar;
            int char_count = 0;
            GeminiCommand gmc = FindGeminiCommand(command.m_Command);

            if (gmc != null)
            {
                gemini_result = gmc.Type;
                char_count = gmc.Chars;
                command.m_UpdateRequired = gmc.UpdateStatus;
            }

            // no result expected by this command, just return;
            if (gemini_result == GeminiCommand.ResultType.NoResult) return null;

            m_SerialTimeoutExpired.Reset();
            m_SerialErrorOccurred.Reset();

            if (command.m_Timeout > 0)
            {
                tmrReadTimeout.Interval = command.m_Timeout;
                tmrReadTimeout.Start();
            }

            try
            {
                switch (gemini_result)
                {
                    // a specific number of characters expected as the return value
                    case GeminiCommand.ResultType.NumberofChars:
                        result = ReadNumber(char_count);
                        break;

                    // value '1' or a string terminated by '#'
                    case GeminiCommand.ResultType.OneOrHash:
                        result = ReadNumber(1); ;  // check if first character is 1, and return if it is, no hash expected
                        if (result != "1")
                            result += ReadTo('#');
                        break;

                    // value '0' or a string terminated by '#'
                    case GeminiCommand.ResultType.ZeroOrHash:
                        result = ReadNumber(1);
                        if (result != "0")
                            result += ReadTo('#');
                        break;

                    // value '0' or a string terminated by '#'
                    case GeminiCommand.ResultType.HashChar:
                        result = ReadTo('#');
                        break;
                }

            }
            catch
            {
                return null;
            }
            finally
            {
                tmrReadTimeout.Stop();
            }

            if (m_SerialErrorOccurred.WaitOne(0)) return null;  // error occurred!

            // return value for native commands has a checksum appended: validate it and remove it from the return string:
            if (!string.IsNullOrEmpty(result) && command.m_Command[0] == '<')
            {
                char chksum = result[result.Length - 1];
                result = result.Substring(0, result.Length - 1); //remove checksum character

                if (chksum != ComputeChecksum(result))  // bad checksum -- ignore the return value!
                    result = null;
            }

            return result;
        }

        static void tmrReadTimeout_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_SerialTimeoutExpired.Set();
        }

        /// <summary>
        /// Read serial port until the terminating character is encoutered. Don't include 
        /// terminating character in the result, honor readtimeout specified on the port.
        /// </summary>
        /// <param name="terminate"></param>
        /// <returns></returns>
        private static string ReadTo(char terminate)
        {
            string res = "";

            for (; ; )
            {
                if (m_SerialPort.BytesToRead > 0)
                {
                    char c = (char)m_SerialPort.ReadChar();
                    if (c != terminate)
                        res += c;
                    else
                        return res;
                }
                else
                    if (m_SerialTimeoutExpired.WaitOne(0)) return null;
            }
        }

        /// <summary>
        /// Read exact number of characters from the serial port, honoring the read timeout
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        private static string ReadNumber(int chars)
        {
            string res = "";
            for (; ; )
            {
                if (m_SerialPort.BytesToRead > 0)
                {
                    char c = (char)m_SerialPort.ReadChar();
                    res += c;
                    if (res.Length == chars) return res;
                }
                else
                    if (m_SerialTimeoutExpired.WaitOne(0)) return null;
            }
        }

        /// <summary>
        /// Find an entry in GeminiCommands collection for the full command
        /// string. The full command string can include parameters as part of the string
        /// </summary>
        /// <param name="full_cmd">full command, possibly including parameters</param>
        /// <returns>object describing return value for this Gemini command, or null if not found</returns>
        private static GeminiCommand FindGeminiCommand(string full_cmd)
        {

            if (full_cmd.StartsWith("<"))       // native get command is always '#' terminated
                return new GeminiCommand(GeminiCommand.ResultType.HashChar, 0);
            else if (full_cmd.StartsWith(">"))  // native set command always no return value
                return new GeminiCommand(GeminiCommand.ResultType.NoResult, 0);
            else
            {
                // try to match the longest string first. Maximum length
                // command is something like :GVD or four characters,
                // minimum length command is 2 characters:
                for (int i = Math.Min(4, full_cmd.Length); i >= 2; --i)
                {
                    string sub = full_cmd.Substring(0, i);
                    if (GeminiCommands.Commands.ContainsKey(sub))
                        return GeminiCommands.Commands[sub];
                }
            }

            return null;            
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


        /// <summary>
        /// Get current Altitude propery
        /// retrieved from the latest polled value from the mount, no actual command is executed
        /// </summary>
        public static double Altitude
        { get { return m_Altitude; } }

        /// <summary>
        /// Get current Azimuth propery
        /// retrieved from the latest polled value from the mount, no actual command is executed
        /// </summary>
        public static double Azimuth
        { get { return m_Azimuth; } }

        /// <summary>
        /// Get current AtHome propery
        /// returns whether the telescope is at the home position
        /// </summary>
        public static bool AtHome
        { get { return m_AtHome; } }

        /// <summary>
        /// Get current AtPark propery
        /// returns whether the telescope is parked
        /// </summary>
        public static bool AtPark
        { get { return m_AtPark; } }

        /// <summary>
        /// Get Set current TargetRightAscention propery
        /// </summary>
        public static double TargetRightAscension
        { 
            get { return m_TargetRightAscension; }
            set { m_TargetRightAscension = value; }
        }

        /// <summary>
        /// Get Set current TargetDeclination propery
        /// </summary>
        public static double TargetDeclination
        { 
            get { return m_TargetDeclination; }
            set { m_TargetDeclination = value; }
        }

        /// <summary>
        /// Get Set current TargetAltitude propery
        /// </summary>
        public static double TargetAltitude
        {
            get { return m_TargetAltitude; }
            set { m_TargetAltitude = value; }
        }

        /// <summary>
        /// Get Set current TargetAzimuth propery
        /// </summary>
        public static double TargetAzimuth
        {
            get { return m_TargetAzimuth; }
            set { m_TargetAzimuth = value; }
        }

        /// <summary>
        /// Get current SiderealTime propery
        /// retrieved from the latest polled value from the mount, no actual command is executed
        /// </summary>
        public static double SiderealTime
        { get { return m_SiderealTime; } }

        /// <summary>
        /// SetLatitude Method
        /// Stores the Latitude in the Gemini Computer
        /// </summary>
        public static void SetLatitude(double Latitude)
        {         
            m_Latitude = Latitude;
            string latitudedddmm = m_Util.DegreesToDM(Latitude, "*");
            DoCommandResult(":St" + latitudedddmm, 1000, false);
        }

        /// <summary>
        /// SetLatitude Method
        /// Stores the Latitude in the Gemini Computer
        /// </summary>
        public static void SetLongitude(double Longitude)
        {
            m_Longitude = Longitude;
            string longitudedddmm = m_Util.DegreesToDM(Longitude, "*");
            DoCommandResult(":Sg" + longitudedddmm, 1000, false);
        }

        /// <summary>
        /// Get current SiderealTime propery
        /// retrieved from the latest polled value from the mount, no actual command is executed
        /// </summary>
        public static string Velocity
        { get { return m_Velocity; } }

        /// <summary>
        /// Get current Status of Gemini Park
        /// retrieved from the latest polled value from the mount, no actual command is executed
        /// </summary>
        public static string ParkState
        { get { return m_ParkState; } }

        /// <summary>
        /// Get current SideOfPier propery
        /// retrieved from the latest polled value from the mount, no actual command is executed
        /// </summary>
        public static string SideOfPier
        { get { return m_SideOfPier; } }

        /// <summary>
        /// Get current Tracking propery
        /// returns whether the telescope is tracking
        /// </summary>
        public static bool Tracking
        { 
            get { return m_Tracking; }
        }

        /// <summary>
        /// Syncs the mount using Ra and Dec
        /// </summary>
        public static void SyncEquatorial()
        {
            string[] cmd = {":Sr" + m_Util.HoursToHMS(TargetRightAscension,":",":",""),":Sd" + m_Util.DegreesToDMS(TargetDeclination, ":", ":", ""),""};
            if (m_AdditionalAlign)
            {
                cmd[2] = ":Cm";
            }
            else
            {
                cmd[2] = ":CM";
            }
            DoCommandResult(cmd,5000, false);
        }


        /// <summary>
        /// Syncs the mount using Ra and Dec coordinates passed in
        /// </summary>
        public static void SyncToEquatorialCoords(double ra, double dec)
        {
            string[] cmd = { ":Sr" + m_Util.DegreesToHMS(ra, ":", ":", ""), ":Sd" + m_Util.DegreesToDMS(dec, ":", ":", ""), "" };
            if (m_AdditionalAlign)
            {
                cmd[2] = ":Cm";
            }
            else
            {
                cmd[2] = ":CM";
            }
            DoCommandResult(cmd, 5000, false);
        }


        /// <summary>
        /// Slews the mount using Ra and Dec
        /// </summary>
        public static void SlewEquatorial()
        {
            string[] cmd = { ":Sr" + m_Util.HoursToHMS(TargetRightAscension, ":", ":", ""), ":Sd" + m_Util.DegreesToDMS(TargetDeclination, ":", ":", ""), ":MS" };
           
            DoCommandResult(cmd,2000, false);
        }
        /// <summary>
        /// Slews the mount using Ra and Dec
        /// </summary>
        public static void SlewEquatorialAsync()
        {
            string[] cmd = { ":Sr" + m_Util.HoursToHMS(TargetRightAscension, ":", ":", ""), ":Sd" + m_Util.DegreesToDMS(TargetDeclination, ":", ":", ""), ":MS" };

            DoCommandResult(cmd,1000, false);
        }


        public static void SyncHorizonCoordinates(double azimuth, double altitude)
        {
            string[] cmd = { ":Sa" + m_Util.DegreesToDMS(TargetAltitude, ":", ":", ""), ":Sz" + m_Util.DegreesToDMS(TargetAzimuth, ":", ":", ""), "" };
            if (m_AdditionalAlign)
            {
                cmd[2] = ":Cm";
            }
            else
            {
                cmd[2] = ":CM";
            }
            DoCommandResult(cmd, 1000, false);

        }
        
        /// <summary>
        /// Syncs the mount using Alt and Az
        /// </summary>
        public static void SyncHorizon()
        {
            SyncHorizonCoordinates(TargetAzimuth, TargetAltitude);
        }

        /// <summary>
        /// Slews the mount using Alt and Az
        /// </summary>
        public static void SlewHorizon()
        {
            string[] cmd = { ":Sz" + m_Util.DegreesToDMS(TargetAzimuth, ":", ":", ""), ":Sa" + m_Util.DegreesToDMS(TargetAltitude, ":", ":", ""), ":MA" };
            DoCommandResult(cmd, 2000, false);
        }
        /// <summary>
        /// Slews the mount using Alt and Az
        /// </summary>
        public static void SlewHorizonAsync()
        {
            string[] cmd = { ":Sz" + m_Util.DegreesToDMS(TargetAzimuth, ":", ":", ""), ":Sa" + m_Util.DegreesToDMS(TargetAltitude, ":", ":", ""), ":MA" };
            DoCommandResult(cmd, 1000, false);
        }
        #endregion

        #region Focuser Implementation
        /// <summary>
        /// Focuser Reverse Directions
        /// </summary>
        public static bool ReverseDirection
        { 
            get { return m_ReverseDirection; }
            set
            {
                m_Profile.DeviceType = "Focuser";
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "ReverseDirection", value.ToString());
                m_ReverseDirection = value;
            }
        }

        /// <summary>
        /// Focuser Step Size
        /// </summary>
        public static int StepSize
        { 
            get { return m_StepSize; }
            set
            {
                m_Profile.DeviceType = "Focuser";
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "StepSize", value.ToString());
                m_StepSize = value;
            }
        }

        /// <summary>
        /// Focuser Brake Size
        /// </summary>
        public static int BrakeSize
        { 
            get { return m_BrakeSize; }
            set
            {
                m_Profile.DeviceType = "Focuser";
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "BrakeSize", value.ToString());
                m_BrakeSize = value;
            }
        }

        /// <summary>
        /// Focuser Speed
        /// </summary>
        public static int Speed
        { 
            get { return m_Speed; }
            set
            {
                m_Profile.DeviceType = "Focuser";
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "Speed", value.ToString());
                m_Speed = value;
            }
        }

        /// <summary>
        /// Focuser Maximum Increment
        /// </summary>
        public static int MaxIncrement
        { 
            get { return m_MaxIncrement; }
            set
            {
                m_Profile.DeviceType = "Focuser";
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "MaxIncrement", value.ToString());
                m_MaxIncrement = value;
            }
        }


        /// <summary>
        /// Focuser Maximum Step Size
        /// </summary>
        public static int MaxStep
        { 
            get { return m_MaxStep; }
            set
            {
                m_Profile.DeviceType = "Focuser";
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "MaxStep", value.ToString());
                m_MaxStep = value;
            }
        }


        /// <summary>
        /// Focuser Backlash Direction
        /// </summary>
        public static int BacklashDirection
        { 
            get { return m_BacklashDirection; }
            set
            {
                m_Profile.DeviceType = "Focuser";
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "BacklashDirection", value.ToString());
                m_BacklashDirection = value;
            }
        }

        /// <summary>
        /// Focuser Backlash Size
        /// </summary>
        public static int BacklashSize
        { 
            get { return m_BacklashSize; }
            set
            {
                m_Profile.DeviceType = "Focuser";
                m_Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "BacklashSize", value.ToString());
                m_BacklashSize = value;
            }
        }

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
            if (p[0]!=':') p = ":"+p;
            return p+"#"; // standard commands end in '#' character, no checksum needed
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
            m_WaitForCommand.Set();     //signal to the background worker that commands are queued up
        }

        /// <summary>
        /// Add all the command items in 'ci' to the queue for execution
        /// </summary>
        /// <param name="ci">array of commands to be executed in sequence</param>
        private static void QueueCommands(CommandItem[] ci)
        {
            lock (m_CommandQueue)
            {
                for(int i=0; i<ci.Length; ++i)
                    m_CommandQueue.Enqueue(ci[i]);
            }
            m_WaitForCommand.Set();     //signal to the background worker that commands are queued up
        }



        #endregion

    }
}
