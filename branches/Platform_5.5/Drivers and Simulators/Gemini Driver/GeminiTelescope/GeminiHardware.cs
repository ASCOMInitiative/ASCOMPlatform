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
// 07-JUL-2009	rbt	1.0.0a	Initial edit, from ASCOM Telescope Driver template
// 28-JUL-2009  pk  1.0.1a  Initial implementation of Gemini hardware layer and command processor
// 29-JUL-2009  pk  1.0.1a  Added DoCommandAsync asynchronous call-back version of the command processor
//                          Added array versions of DoCommand functions for multiple command execution
// 31-JUL-2009  rbt 1.0.1a  Added Focuser Implementations and Settings
// 29-MAR-2010  pk  1.0.3   Changed Profile access to allow the release of the object when not needed
//                          to ensure data is written to disk in case of an improper shut-down at a later point
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
    /// Async delegate callback for DoCommandAsync
    /// </summary>
    /// <param name="cmd">original command string passed to DoCommandAsync</param>
    /// <param name="result">return result from Gemini, or null if timeout exceeded</param>
    public delegate void HardwareAsyncDelegate(string cmd, string result);

    public delegate void ConnectDelegate(bool Connected, int Clients);

    public delegate void ErrorDelegate(string from, string msg);

    public delegate void SafetyDelegate();

    
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


    /// <summary>
    /// Class encapsulating all serial communications with Gemini
    /// </summary>
    public static class GeminiHardware
    {
#region Member Variables

        private static ASCOM.Utilities.Profile m_Profile;

        public static ASCOM.Utilities.Profile Profile
        {
            get
            {
                if (m_Profile == null) m_Profile = new ASCOM.Utilities.Profile();
                return m_Profile;
            }
            set
            {
                if (value == null && m_Profile != null)
                {
                    m_Profile.Dispose();
                    m_Profile = null;
                }
                else
                    m_Profile = value;
            }
        }

        public static ASCOM.Utilities.Util m_Util;
        public static ASCOM.Astrometry.Transform.Transform  m_Transform;

        // culture used to store ASCOM profile data:
        public static System.Globalization.CultureInfo m_GeminiCulture = new System.Globalization.CultureInfo("en-US");

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
        public static string TargetName { get; set; }

        private static double m_TargetRightAscension = SharedResources.INVALID_DOUBLE;
        private static double m_TargetDeclination = SharedResources.INVALID_DOUBLE;
        private static double m_SiderealTime;
        private static string m_Velocity;
        private static string m_SideOfPier;
        private static double m_TargetAltitude = SharedResources.INVALID_DOUBLE;
        private static double m_TargetAzimuth= SharedResources.INVALID_DOUBLE;

        private static bool m_AdditionalAlign;

        public static bool SwapSyncAdditionalAlign
        {
            get { return GeminiHardware.m_AdditionalAlign; }
            set { GeminiHardware.m_AdditionalAlign = value; }
        }

        private static bool m_Precession;
        private static bool m_Refraction;
        private static bool m_ShowHandbox;
        private static bool m_UseDriverSite;
        private static bool m_UseDriverTime;


        private static bool m_SendAdvancedSettings;

        public static bool SendAdvancedSettings
        {
            get { return GeminiHardware.m_SendAdvancedSettings; }
            set { 
                GeminiHardware.m_SendAdvancedSettings = value;
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "SendAdvancedSettings", value.ToString());               
            }
        }


        private static bool m_Tracking;

        private static bool m_AtPark;
        private static bool m_AtHome;
        private static string m_ParkState = "";
        private static bool m_ParkWasExecuted = false;

        private static System.Threading.ManualResetEvent m_SlewAborted = new System.Threading.ManualResetEvent(false);

        private static bool m_SouthernHemisphere = false;

        private static string m_GeminiVersion = "";

        private static TimeSpan m_GPSTimeDifference = TimeSpan.Zero;    // GPS UTC time - PC clock UTC time

        private static int m_SlewSettleTime = 0;

        public static int SlewSettleTime
        {
            get { return GeminiHardware.m_SlewSettleTime; }
            set { GeminiHardware.m_SlewSettleTime = value;
            Profile.DeviceType = "Telescope";
            Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "SlewSettleTime", value.ToString());
            }
        }


        private static double m_FocalLength;

        public static double FocalLength
        {
            get { return GeminiHardware.m_FocalLength; }
            set {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "FocalLength", value.ToString(GeminiHardware.m_GeminiCulture));
                GeminiHardware.m_FocalLength = value; 
            }
        }

        private static double m_ApertureArea;

        public static double ApertureArea
        {
            get { return GeminiHardware.m_ApertureArea; }
            set {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "ApertureArea", value.ToString(GeminiHardware.m_GeminiCulture));
                GeminiHardware.m_ApertureArea = value;
            }
        }


        private static double m_ApertureDiameter;

        public static double ApertureDiameter
        {
            get { return GeminiHardware.m_ApertureDiameter; }
            set {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "ApertureDiameter", value.ToString(GeminiHardware.m_GeminiCulture));
                GeminiHardware.m_ApertureDiameter = value;
            }
        }
        

        private static string m_ComPort;
        private static int m_BaudRate;
        private static ASCOM.Utilities.SerialParity m_Parity;
        private static int m_DataBits;
        private static ASCOM.Utilities.SerialStopBits m_StopBits;

        private static string m_GpsComPort;
        private static int m_GpsBaudRate;
        private static bool m_GpsUpdateClock;

        private static string m_PassThroughComPort;

        private static bool m_ScanCOMPorts;

        public static bool ScanCOMPorts
        {
            get { return GeminiHardware.m_ScanCOMPorts; }
            set {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "ScanCOMPorts", value.ToString(GeminiHardware.m_GeminiCulture));                
                GeminiHardware.m_ScanCOMPorts = value; 
            }
        }



        public static string PassThroughComPort
        {
            get { return GeminiHardware.m_PassThroughComPort; }
            set { 
                GeminiHardware.m_PassThroughComPort = value;
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "PassThroughComPort", value);
            }
        }

        private static int m_PassThroughBaudRate;

        public static int PassThroughBaudRate
        {
            get { return GeminiHardware.m_PassThroughBaudRate; }
            set {
                GeminiHardware.m_PassThroughBaudRate = value;
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "PassThroughBaudRate", value.ToString());
            }
        }
        private static bool m_PassThroughPortEnabled;

        public static bool PassThroughPortEnabled
        {
            get { return m_PassThroughPortEnabled; }
            set { 
                m_PassThroughPortEnabled = value;
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "PassThroughPortEnabled", value.ToString());
            }
        }

        private static bool m_AsyncPulseGuide = true;

        public static bool AsyncPulseGuide
        {
            get { return GeminiHardware.m_AsyncPulseGuide; }
            set { 
                GeminiHardware.m_AsyncPulseGuide = value;
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "AsyncPulseGuide", value.ToString());
            }
        }
    

        private static System.Threading.AutoResetEvent m_WaitForCommand;
        private static System.Threading.AutoResetEvent m_DataReceived;

        private static System.Threading.ManualResetEvent m_AbortConnect = new System.Threading.ManualResetEvent(false);

        private static string m_PolledVariablesString = ":GR#:GD#:GA#:GZ#:Gv#:GS#:Gm#:h?#<99:F#";
        private static string m_ShortPolledVariablesString1 = ":GR#:GD#:GA#:GZ#:Gv#";
        private static string m_ShortPolledVariablesString2 = ":GS#:Gm#:h?#<99:F#";

        public static int MAX_TIMEOUT = 10000; //max default timeout for all commands

        static System.Timers.Timer tmrReadTimeout = new System.Timers.Timer();
        static System.Threading.AutoResetEvent m_SerialTimeoutExpired = new System.Threading.AutoResetEvent(false);
        static System.Threading.AutoResetEvent m_SerialErrorOccurred = new System.Threading.AutoResetEvent(false);

        static private int m_TraceLevel = -1;

        /// <summary>
        /// Trace level, if set at or above zero, will create a new tracer object
        /// </summary>
        static public int TraceLevel
        {
            get { return m_TraceLevel; }    
            set {
                if (value >= 0)
                    m_Trace.Start(SharedResources.TELESCOPE_DRIVER_NAME, value);
                else
                    m_Trace.Stop();
                m_TraceLevel = value;
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "TraceLevel", value.ToString());
            }
        }

        static private Tracer m_Trace = new Tracer();

        /// <summary>
        /// Tracer object for all tracing needs
        /// </summary>
        static public Tracer Trace
        {
            get { return m_Trace; }
        }


        private static bool m_UseJoystick = false;

        /// <summary>
        /// Does the user want to use joystick?
        /// </summary>
        public static bool UseJoystick
        {
            get { return GeminiHardware.m_UseJoystick; }
            set {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "UseJoystick", value.ToString());
                GeminiHardware.m_UseJoystick = value; 
            }
        }

        private static string m_JoystickName = null;
        /// <summary>
        /// Name of the configured joystick driver
        /// </summary>
        public static string JoystickName
        {
            get { return GeminiHardware.m_JoystickName; }
            set {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickName", value.ToString());
                GeminiHardware.m_JoystickName = value; 
            }
        }

        private static double m_JoystickSensitivity = 0;

        /// <summary>
        /// Scaling factor to apply to both joystick axis
        /// </summary>
        public static double JoystickSensitivity
        {
            get { return GeminiHardware.m_JoystickSensitivity; }
            set {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickSensitivity", value.ToString(GeminiHardware.m_GeminiCulture));
                GeminiHardware.m_JoystickSensitivity = value;
            }
        }

        private static bool m_JoystickFlipRA = false;
        /// <summary>
        /// Flip RA position on the joystick
        /// </summary>
        public static bool JoystickFlipRA
        {
            get { return GeminiHardware.m_JoystickFlipRA; }
            set {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickFlipRA", value.ToString());
                GeminiHardware.m_JoystickFlipRA = value;
            }
        }
        private static bool m_JoystickFlipDEC = false;

        /// <summary>
        /// Flip DEC direction on the joystick
        /// </summary>
        public static bool JoystickFlipDEC
        {
            get { return GeminiHardware.m_JoystickFlipDEC; }
            set {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickFlipDEC", value.ToString());
                GeminiHardware.m_JoystickFlipDEC = value;
            }
        }


        private static double m_ParkAlt;

        /// <summary>
        /// If Alt/Az coordinates for parking selected, this is the Alt position where to park
        /// </summary>
        public static double ParkAlt
        {
            get { return GeminiHardware.m_ParkAlt; }
            set { 
                GeminiHardware.m_ParkAlt = value;
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "ParkAlt", value.ToString(GeminiHardware.m_GeminiCulture));
            }
        }
        private static double m_ParkAz;

        /// <summary>
        /// If Alt/Az coordinates for parking selected, this is the Az position where to park
        /// </summary>
        public static double ParkAz
        {
            get { return GeminiHardware.m_ParkAz; }
            set { 
                GeminiHardware.m_ParkAz = value;
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "ParkAz", value.ToString(GeminiHardware.m_GeminiCulture));
            }
        }

        private static GeminiParkMode m_ParkPosition;
        /// <summary>
        /// Where to park
        /// </summary>
        public static GeminiParkMode ParkPosition
        {
            get { return m_ParkPosition; }
            set
            {
                GeminiHardware.m_ParkPosition  = value;
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "ParkPosition", ((int)value).ToString());
            }

        }
        private static int m_JoystickAxisRA;
        /// <summary>
        /// Which joystick axis to assign to RA axis
        /// </summary>
        public static int JoystickAxisRA
        {
            get { return GeminiHardware.m_JoystickAxisRA; }
            set {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickAxisRA", value.ToString());
                GeminiHardware.m_JoystickAxisRA = value;
            }
        }

        private static int m_JoystickAxisDEC;
        /// <summary>
        /// Which joystick axis to assign to DEC axis
        /// </summary>
        public static int JoystickAxisDEC
        {
            get { return GeminiHardware.m_JoystickAxisDEC; }
            set
            {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickAxisDEC", value.ToString());
                GeminiHardware.m_JoystickAxisDEC = value;
            }
        }

        private static UserFunction[] m_JoystickButtonMap;
        /// <summary>
        /// Mapping between joystick buttons and user functions to execute
        /// </summary>
        public static UserFunction[] JoystickButtonMap
        {
            get { return m_JoystickButtonMap; }
            set { 
                m_JoystickButtonMap = value;
                GeminiHardware.Profile.DeviceType = "Telescope";
                for (int i = 0; i < value.Length; ++i)
                {
                    int v = (int)value[i];
                    GeminiHardware.Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Button " + (i+1).ToString(), v.ToString());
                }
            }
        }

        private static bool m_JoystickIsAnalog = true;

        public static bool JoystickIsAnalog
        {
            get { return GeminiHardware.m_JoystickIsAnalog; }
            set { 
                GeminiHardware.m_JoystickIsAnalog = value;
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickIsAnalog", value.ToString());
                m_JoystickFixedSpeed = 0;   //reset this to guiding speed
            }
        }

        private static int m_JoystickFixedSpeed = 0; // 0 = guide, 1=center, 2 = slew
        /// <summary>
        /// Joystick fixed speed: 0 = guide, 1=center, 2 = slew
        /// </summary>
        public static int JoystickFixedSpeed
        {
            get { return GeminiHardware.m_JoystickFixedSpeed; }
            set { GeminiHardware.m_JoystickFixedSpeed = value; }
        }


        private static bool m_UseSpeech = false;
        /// <summary>
        /// Use announcer or do not
        /// </summary>
        public static bool UseSpeech
        {
            get { return GeminiHardware.m_UseSpeech; }
            set { 
                GeminiHardware.m_UseSpeech = value; 
                GeminiHardware.Profile.DeviceType = "Telescope";
                GeminiHardware.Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "UseSpeech", value.ToString());
            }
        }


        private static Speech.SpeechType m_SpeechFilter = 0;

        /// <summary>
        /// Filter what kind of announcements to announce
        /// </summary>
        internal static Speech.SpeechType SpeechFilter
        {
            get { return GeminiHardware.m_SpeechFilter; }
            set { 
                
                GeminiHardware.m_SpeechFilter = value;
                GeminiHardware.Profile.DeviceType = "Telescope";
                GeminiHardware.Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "SpeechFilter", ((int)value).ToString());
            }
        }

        private static string m_SpeechVoice = "";
        /// <summary>
        /// Voice selection
        /// </summary>
        public static string SpeechVoice
        {
            get { return GeminiHardware.m_SpeechVoice; }
            set { 
                GeminiHardware.m_SpeechVoice = value;
                GeminiHardware.Profile.DeviceType = "Telescope";
                GeminiHardware.Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "SpeechVoice", value.ToString());
            }
        }


        private static bool m_KeepMainFormOnTop = false;
        /// <summary>
        /// Keep main handbox form topmost
        /// </summary>
        public static bool KeepMainFormOnTop
        {
            get { return GeminiHardware.m_KeepMainFormOnTop; }
            set
            {
                GeminiHardware.m_KeepMainFormOnTop = value;
                GeminiHardware.Profile.DeviceType = "Telescope";
                GeminiHardware.Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "KeepMainFormOnTop", value.ToString());
                Profile = null;
            }
        }


        private static Point m_MainFormPosition = Point.Empty;
        /// <summary>
        /// Where to position the main handbox form
        /// </summary>
        public static Point MainFormPosition
        {
            get { return GeminiHardware.m_MainFormPosition; }
            set { 
                GeminiHardware.m_MainFormPosition = value;
                GeminiHardware.Profile.DeviceType = "Telescope";
                GeminiHardware.Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "MainFormPositionX", value.X.ToString());
                GeminiHardware.Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "MainFormPositionY", value.Y.ToString());
            }
        }


        private static bool m_NudgeFromSafety = false;

        /// <summary>
        /// Is NudgeFromSafety feature enabled?
        /// </summary>
        public static bool NudgeFromSafety
        {
            get { return GeminiHardware.m_NudgeFromSafety; }
            set
            {
                GeminiHardware.m_NudgeFromSafety = value;
                GeminiHardware.Profile.DeviceType = "Telescope";
                GeminiHardware.Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "NudgeFromSafety", value.ToString());
            }
        }


        public enum GeminiBootMode
        {
            Prompt = 0,
            ColdStart = 1,
            WarmStart = 2,
            WarmRestart = 3,
        }
        ;

        public enum GeminiParkMode
        {
            NoSlew = 0,
            SlewHome = 1,
            SlewCWD = 2,
            SlewAltAz = 3,
        }

        private static GeminiBootMode m_BootMode = GeminiBootMode.Prompt; 

        private static SerialPort m_SerialPort; // main physical port

        private static PassThroughPort m_PassThroughPort = null; // a secondary port (virtual) for connecting non-ASCOM compliant Gemini applications

        private static bool m_Connected = false; //Keep track of the connection status of the hardware

        private static int m_Clients;

        private static DateTime m_LastUpdate;
        private static object m_ConnectLock = new object();

        private static int m_QueryInterval = SharedResources.GEMINI_POLLING_INTERVAL;     // query mount for status this often, in msecs.

        private static int m_TotalErrors = 0;              //total number of errors encountered since m_FirstErrorTick count
        private static int m_FirstErrorTick = 0;           //

        private static DateTime m_LastDataTick;              // tick of when the last successful data was received from the mount

        private static int m_GeminiStatusByte;              // result of <99: native command, polled on an interval
        private static bool m_SafetyNotified;               // true if safety limit notification was already sent


        private static frmStatus m_StatusForm = null;

        //Focuser Private Data
        private static bool m_AbsoluteFocuser=true;
        private static int m_MaxIncrement = 0;
        private static int m_MaxStep = 0;
        private static int m_StepSize = 0;
        private static bool m_ReverseDirection = false;
        private static int m_BacklashDirection = 0;
        private static int m_BacklashSize = 0;
        private static int m_BrakeSize = 0;
        private static int m_Speed = 1;
#endregion

#region Public Events
        /// <summary>
        /// OnConnect is fired when a new client is connected or disconnected
        ///   OnConnect(Connected, Clients) - Connected is true if a client conects, false if disconnects
        ///   Clients is the total number of remaining attached clients after the connect/disconnect
        ///   if Connected is false, and Clients is 0, then the driver is disconnected from the serial port/gemini
        /// </summary>
        public static ConnectDelegate OnConnect;

        /// <summary>
        /// OnError is fired when a serious serial error occurs, such as a command timeout
        ///  checksum error, or other failure to complete a request
        /// </summary>
        public static ErrorDelegate OnError;


        /// <summary>
        /// OnInfo is fired when a UI notification to the user is needed
        /// </summary>
        public static ErrorDelegate OnInfo;

        private static bool m_AllowErrorNotify = true;

        /// <summary>
        /// Fired when safety limit is reached
        /// </summary>
        public static SafetyDelegate OnSafetyLimit;

#endregion

        #region Initializers
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
            Trace.Enter(0, "GeminiHardware", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, DateTime.Now.ToString(GeminiHardware.m_GeminiCulture), "Trace="+m_TraceLevel.ToString());

            m_SerialPort.DataReceived += new SerialDataReceivedEventHandler(m_SerialPort_DataReceived);
            m_DataReceived = new System.Threading.AutoResetEvent(false);

            Trace.Exit(0, "GeminiHardware");
        }

        static void m_SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            m_DataReceived.Set();
        }


        /// <summary>
        ///  reloads all the variables from the profile
        /// </summary>
        private static void GetProfileSettings() 
        {
            Trace.Enter("GetProfileSettings");

            //Telescope Settings
            Profile.DeviceType = "Telescope";
            if (Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "RegVer", "") != SharedResources.REGISTRATION_VERSION)
            {
                Trace.Info(2, "New Profile version");

                //Main Driver Settings
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "RegVer", SharedResources.REGISTRATION_VERSION);

                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "ComPort", "COM1");
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "BaudRate", "9600");

                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "GpsComPort", "COM1");
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "GpsBaudRate", "4800");
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "GpsUpdateClock", true.ToString());

                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "AdditionalAlign", false.ToString());
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Precession", false.ToString());
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Refraction", false.ToString());
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Show Handbox", false.ToString());
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Use Driver Site", false.ToString());
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Use Driver Time", false.ToString());

                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "PassThroughPortEnabled", false.ToString());
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "PassThroughComPort", "COM10");
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "PassThroughBaudRate", "9600");

                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "BootMode", "0");

                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "AsyncPulseGuide", true.ToString());

            }
                
            //Load up the values from saved
            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "AdditionalAlign", ""), out m_AdditionalAlign))
                m_AdditionalAlign = false;

            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Precession", ""), out m_Precession))
                m_Precession = false;
            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Refraction", ""), out m_Refraction))
                m_Refraction = false;
            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Show Handbox", ""), out m_ShowHandbox))
                m_ShowHandbox = false;
            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Use Driver Site", ""), out m_UseDriverSite))
                m_UseDriverSite= false;
            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Use Driver Time", ""), out m_UseDriverTime))
                m_UseDriverTime = false;

            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "TraceLevel", ""), out m_TraceLevel))
                m_TraceLevel = 3;

            TraceLevel = m_TraceLevel;

            Trace.Info(2, "User Settings", m_AdditionalAlign, m_Precession, m_Refraction, m_ShowHandbox, m_UseDriverSite, m_UseDriverTime);

            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "SlewSettleTime", ""), out m_SlewSettleTime))
                m_SlewSettleTime = 2;

            m_ComPort = Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "ComPort", "");
            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "BaudRate", ""), out m_BaudRate))
                m_BaudRate = 9600;


            m_GpsComPort = Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "GpsComPort", "");
            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "GpsBaudRate", ""), out m_GpsBaudRate))
                m_GpsBaudRate = 4800;
            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "GpsUpdateClock", ""), out m_GpsUpdateClock))
                m_GpsUpdateClock = false;

            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "DataBits", ""), out m_DataBits))
                m_DataBits = 8;

            int _parity = 0;
            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Parity", ""), out _parity))
                _parity = 0;

            m_Parity = (ASCOM.Utilities.SerialParity)_parity;

            int _stopbits = 8;
            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "StopBits", ""), out _stopbits))
                _stopbits = 1;

            m_StopBits = (ASCOM.Utilities.SerialStopBits)_stopbits;

            Trace.Info(2, "Comm Settings", m_ComPort, m_BaudRate, m_DataBits, m_Parity, m_StopBits);

            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "ScanCOMPorts", ""), out m_ScanCOMPorts))
                m_ScanCOMPorts = true;

            Trace.Info(2, "Scan COM ports", m_ScanCOMPorts);


            if (!double.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Latitude", ""), System.Globalization.NumberStyles.Float, m_GeminiCulture, out m_Latitude))
                m_Latitude = 0.0;

            if (!double.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Longitude", ""), System.Globalization.NumberStyles.Float, m_GeminiCulture, out m_Longitude))
                m_Longitude = 0.0;

            if (!double.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Elevation", ""), System.Globalization.NumberStyles.Float, m_GeminiCulture, out m_Elevation))
                m_Elevation = 0.0;

            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "UTCOffset", ""), out m_UTCOffset))
                m_UTCOffset = -(int)(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours);

            if (!double.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "ApertureArea", ""), System.Globalization.NumberStyles.Float, m_GeminiCulture, out m_ApertureArea))
                m_ApertureArea = 0.0;

            if (!double.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "ApertureDiameter", ""), System.Globalization.NumberStyles.Float, m_GeminiCulture, out m_ApertureDiameter))
                m_ApertureDiameter = 0.0;

            if (!double.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "FocalLength", ""), System.Globalization.NumberStyles.Float, m_GeminiCulture, out m_FocalLength))
                m_FocalLength = 0.0;

            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "AsyncPulseGuide", ""), out m_AsyncPulseGuide))
                m_AsyncPulseGuide = true;

            Trace.Info(2, "Geo Settings", m_Latitude, m_Longitude, m_Elevation);

            int x, y;

            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "MainFormPositionX", ""), System.Globalization.NumberStyles.Float, m_GeminiCulture, out x))
                x = 0;

            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "MainFormPositionY", ""), System.Globalization.NumberStyles.Float, m_GeminiCulture, out y))
                y = 0;

            m_MainFormPosition = new Point(x, y);

            m_PassThroughComPort = Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "PassThroughComPort", "");
            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "PassThroughBaudRate", ""), out m_PassThroughBaudRate))
                m_PassThroughBaudRate = 9600;

            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "PassThroughPortEnabled", ""), out m_PassThroughPortEnabled))
                m_PassThroughPortEnabled = false;

            if (m_PassThroughPortEnabled && m_PassThroughComPort.Equals(m_ComPort, StringComparison.CurrentCultureIgnoreCase))
            {
                Trace.Error("Pass-through port is invalid", m_PassThroughComPort);
                if (OnError != null) OnError(Resources.PTPDisabled, Resources.PTPInvalid + m_PassThroughComPort);
                PassThroughPortEnabled = false;
            }

            if (m_ComPort != "")
            {
                m_SerialPort.PortName = m_ComPort;
            }

            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "SendAdvancedSettings", ""), out m_SendAdvancedSettings))
                m_SendAdvancedSettings = false;

            Trace.Info(2, "Pass Through Port", m_PassThroughComPort, m_PassThroughBaudRate, m_PassThroughPortEnabled);

            if (!double.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "ParkAlt", ""), System.Globalization.NumberStyles.Float, m_GeminiCulture, out m_ParkAlt))
                m_ParkAlt= 0.0;

            if (!double.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "ParkAz", ""), System.Globalization.NumberStyles.Float, m_GeminiCulture, out m_ParkAz))
                m_ParkAz = 0.0;

            int prk = 0;
            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "ParkPosition", ""), out prk))
                prk = 0;

            m_ParkPosition = (GeminiParkMode)prk;

            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "UseJoystick", ""), out m_UseJoystick))
                m_UseJoystick = false;


            m_JoystickName = Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickName", "");

            m_JoystickButtonMap = JoystickButtonMapFromProfile();

            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickAxisRA", ""), out m_JoystickAxisRA))
                m_JoystickAxisRA = 0;

            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickAxisDEC", ""), out m_JoystickAxisDEC))
                m_JoystickAxisDEC = 1;

            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickFlipRA", ""), out m_JoystickFlipRA))
                m_JoystickFlipRA = false;

            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickFlipDEC", ""), out m_JoystickFlipDEC))
                m_JoystickFlipDEC = false;

            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickIsAnalog", ""), out m_JoystickIsAnalog))
                m_JoystickIsAnalog = true;

            if (!double.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "JoystickSensitivity", ""), System.Globalization.NumberStyles.Float, m_GeminiCulture, out m_JoystickSensitivity))
                m_JoystickSensitivity = 100;

            m_SpeechVoice = Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "SpeechVoice", "");

            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "UseSpeech", ""), out m_UseSpeech))
                m_UseSpeech = false;

            int filter = 0;
            if (!int.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "SpeechFilter", ""), out filter))
                m_SpeechFilter = 0;
            else
                m_SpeechFilter = (Speech.SpeechType)filter;

            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "KeepMainFormOnTop", ""), out m_KeepMainFormOnTop))
                m_KeepMainFormOnTop = false;

            if (!bool.TryParse(Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "NudgeFromSafety", ""), out m_NudgeFromSafety))
                m_NudgeFromSafety= false;

            //Get the Boot Mode from settings
            try
            {
                switch (Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "BootMode", ""))
                {
                    case "0":
                        m_BootMode = GeminiBootMode.Prompt;
                        break;
                    case "1":
                        m_BootMode = GeminiBootMode.ColdStart;
                        break;
                    case "2":
                        m_BootMode = GeminiBootMode.WarmStart;
                        break;
                    case "3":
                        m_BootMode = GeminiBootMode.WarmRestart;
                        break;
                    default:
                        m_BootMode = GeminiBootMode.Prompt;
                        break;
                }
            }
            catch
            {
                m_BootMode = GeminiBootMode.Prompt;
            }

            Trace.Info(2, "Boot Settings", m_BootMode);


            //Focuser Settings
            Profile.DeviceType = "Focuser";
            
            if (Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "RegVer", "") != SharedResources.REGISTRATION_VERSION)
            {
                Trace.Info(2, "New Focuser version");

                //Main Driver Settings
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "RegVer", SharedResources.REGISTRATION_VERSION);
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "MaxIncrement", "5000");
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "StepSize", "100");
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "ReverseDirection", false.ToString());
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "BacklashDirection", "0");
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "BacklashSize", "50");
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "BrakeSize", "0");
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "Speed", "1");
            }

            string s = Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "MaxIncrement");
            if (!int.TryParse(s, out m_MaxIncrement) || m_MaxIncrement <= 0)
                m_MaxIncrement = 5000;

            s = Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "MaxStep");
            //if (!int.TryParse(s, out m_MaxStep) || m_MaxStep <= 0)
            m_MaxStep = 0x7fffffff;

            s = Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "StepSize");
            if (!int.TryParse(s, out m_StepSize) || m_StepSize <= 0)
                m_StepSize = 100;

            if (!bool.TryParse(Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "AbsoluteFocuser", ""), out m_AbsoluteFocuser))
                m_AbsoluteFocuser = true;

            s = Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "ReverseDirection");
            if (!bool.TryParse(s, out m_ReverseDirection))
                m_ReverseDirection = false;

            s = Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "BacklashDirection");
            if (!int.TryParse(s, out m_BacklashDirection) || m_BacklashDirection < -1 || m_BacklashDirection > 1)
                m_BacklashDirection = 0;

            s = Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "BacklashSize");
            if (!int.TryParse(s, out m_BacklashSize) || m_BacklashSize < 0)
                m_BacklashSize = 0;

            s = Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "BrakeSize");
            if (!int.TryParse(s, out m_BrakeSize) || m_BrakeSize < 0)
                m_BrakeSize = 0;

            s = Profile.GetValue(SharedResources.FOCUSER_PROGRAM_ID, "Speed");
            if (!int.TryParse(s, out m_Speed) || m_Speed < 1 || m_Speed > 3)
                m_Speed = 1;

            Trace.Info(2, "Focuser Settings", m_MaxIncrement, m_MaxStep, m_StepSize, m_ReverseDirection, m_BacklashDirection, m_BacklashSize, m_BrakeSize, m_Speed);

            //Profile.DeviceType = "Telescope";

            Profile = null; //dispose profile object to ensure everything gets written to disk

            Trace.Exit("GetProfileSettings");

        }

        private static UserFunction[] JoystickButtonMapFromProfile()
        {
            UserFunction [] funcs = new UserFunction[36];
            
            for (int i = 0; i < 36; ++i)
            {
                string value = GeminiHardware.Profile.GetValue(SharedResources.TELESCOPE_PROGRAM_ID, "Button " + (i + 1).ToString(), "");
                int val = 0;
                int.TryParse(value, out val);
                funcs[i] = (UserFunction)val;
            }
            return funcs;
        }

#endregion


#region Properties For Settings
        /// <summary>
        /// Get/Set Boot Mode 
        /// </summary>
        public static GeminiBootMode BootMode
        {
            get { return m_BootMode; }
            set 
            { 
                m_BootMode = value;
                string bootMode = "0";
                switch (value)
                {
                    case GeminiBootMode.Prompt:
                        bootMode = "0";
                        break;
                    case GeminiBootMode.ColdStart:
                        bootMode = "1";
                        break;
                    case GeminiBootMode.WarmStart:
                        bootMode = "2";
                        break;
                    case GeminiBootMode.WarmRestart:
                        bootMode = "3";
                        break;
                }
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "BootMode", bootMode);
            }

        }

        /// <summary>
        /// Returns Gemini version, level number digit, followed by two version digits
        /// </summary>
        public static string Version
        {
            get { return m_GeminiVersion; }

        }

        /// <summary>
        /// Gemini-defined UTC offset (timezone)
        /// </summary>
        public static int UTCOffset
        {
            get { return GeminiHardware.m_UTCOffset; }
            set {
                GeminiHardware.m_UTCOffset = value;
                string result = DoCommandResult(":SG" + (value > 0 ? "+" : "") + (value).ToString("0"), MAX_TIMEOUT, false);
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "UTCOffset", value.ToString());
            }
        }

        /// <summary>
        /// Get/Set Hanbox Form Setting 
        /// </summary>
        public static bool ShowHandbox
        {
            get { return m_ShowHandbox; }
            set
            {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Show Handbox", value.ToString());
                m_ShowHandbox = value;
            }
        }

        /// <summary>
        /// Get/Set Use Gemini Site 
        /// </summary>
        public static bool UseDriverSite
        {
            get { return m_UseDriverSite; }
            set
            {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Use Driver Site", value.ToString());
                m_UseDriverSite = value;
            }
        }
        /// <summary>
        /// Get/Set Use Gemini Time 
        /// </summary>
        public static bool UseDriverTime
        {
            get { return m_UseDriverTime; }
            set
            {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Use Driver Time", value.ToString());
                m_UseDriverTime = value;
            }
        }
        /// <summary>
        /// Get/Set serial comm port 
        /// </summary>
        public static string ComPort
        {
            get { return m_ComPort; }
            set 
            {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "ComPort", value.ToString());
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
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "BaudRate", value.ToString());
                m_BaudRate = value;
            }
        }
        /// <summary>
        /// Get/Set serial comm port 
        /// </summary>
        public static string GpsComPort
        {
            get { return m_GpsComPort; }
            set
            {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "GpsComPort", value.ToString());
                m_GpsComPort = value;
            }
        }
        /// <summary>
        /// Get/Set baud rate
        /// </summary>
        public static int GpsBaudRate
        {
            get { return m_GpsBaudRate; }
            set
            {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "GpsBaudRate", value.ToString());
                m_GpsBaudRate = value;
            }
        }
        /// <summary>
        /// Get/Set Gps Updates Clock
        /// </summary>
        public static bool GpsUpdateClock
        {
            get { return m_GpsUpdateClock; }
            set 
            {
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "GpsUpdateClock", value.ToString());
                m_GpsUpdateClock = value; 
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
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Parity", value.ToString());
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
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "StopBits", value.ToString());
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
                Profile.DeviceType = "Telescope";
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "DataBits", value.ToString());
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
                Profile.DeviceType = "Telescope";
                m_Elevation = value;
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Elevation", value.ToString(GeminiHardware.m_GeminiCulture));
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
                
                Trace.Enter("Latitude", value);
                if (value < -90 || value > 90) throw new ASCOM.InvalidValueException("Latitude", value.ToString(), "-90..90");
                Profile.DeviceType = "Telescope";
                m_Latitude = value;
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Latitude", value.ToString(GeminiHardware.m_GeminiCulture));
                m_SouthernHemisphere = (m_Latitude < 0);
                m_Transform.SiteLatitude = m_Latitude;
                Trace.Exit("Latitude", value);
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
                Trace.Enter("Longitude", value);
                if (value < -180 || value > 180) throw new ASCOM.InvalidValueException("Longitude", value.ToString(), "-180..180");
                Profile.DeviceType = "Telescope";
                m_Longitude = value;
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Longitude", value.ToString(GeminiHardware.m_GeminiCulture));
                m_Transform.SiteLongitude = m_Longitude;
                Trace.Enter("Longitude", value);

            }
        }

        /// <summary>
        /// Get/Set Equatorial system type: true => Gemini computes precession from J2000, false => coordinates are already precessed
        ///   current Refraction setting is also updated to the mount, as that's the only way Gemini takes
        ///   these settings: together.
        /// </summary>
        public static bool Precession
        {
            get
            {
                Trace.Enter("Pecession.Get", (m_GeminiStatusByte & 32) == 0 ? false : true);
                return (m_GeminiStatusByte & 32) == 0 ?  false : true; 
            }
            set {
                if (value == false) //JNOW 
                {
                    Trace.Enter("Precession.Set", value);
                    if (m_Refraction)
                        DoCommandResult(":p2", MAX_TIMEOUT, false);
                    else
                        DoCommandResult(":p0", MAX_TIMEOUT, false);
                }
                else //J2000
                {
                    if (m_Refraction)
                        DoCommandResult(":p3", MAX_TIMEOUT, false);
                    else
                        DoCommandResult(":p1", MAX_TIMEOUT, false);
                }
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Precession", m_Precession.ToString());
            }
        }

        /// <summary>
        /// Get/Set whether Gemini should apply refraction correction: true = Gemini calculates refraction, false = it doesn't
        ///   current precession setting is also updated to the mount, as that's the only way Gemini takes
        ///   these settings: together.
        /// </summary>
        public static bool Refraction
        {
            get
            {
                Trace.Enter("Refraction.Get", m_Refraction);
                return (m_Refraction);
            }
            set
            {
                Trace.Enter("Refraction.Set", m_Refraction);
                m_Refraction = value;
                Precession = Precession; // this updates the mount with refraction and precession settings
                Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Refraction", m_Refraction.ToString());
            }
        }

        /// <summary>
        /// Sets both, precession and refraction in a single command
        /// </summary>
        /// <param name="precess"></param>
        /// <param name="refract"></param>
        /// <returns></returns>
        public static bool SetPrecessionRefraction(bool precess, bool refract)
        {
            Trace.Enter("SetPrecessionRefraction", precess, refract);
            m_Refraction = refract;
            Precession = precess; // this updates the mount with refraction and precession settings
            Profile.WriteValue(SharedResources.TELESCOPE_PROGRAM_ID, "Refraction", m_Refraction.ToString());
            Trace.Exit("SetPrecessionRefraction", precess, refract);
            return true;
        }

        public static bool AtSafetyLimit
        {
            get
            {
                Trace.Enter(4, "AtSafetyLimit.Get", (m_GeminiStatusByte & 16) != 0);
                return (m_GeminiStatusByte & 16) != 0;
            }
        }

        public static SiteInfo[] Sites
        {
            get
            {
                SiteInfo[] inf = new SiteInfo[5];
                string[] cmd = { ":GM", ":GN", ":GO", ":GP" };
                for (int i = 0; i < 4; ++i)
                {
                    string s1 = DoCommandResult(cmd[i], MAX_TIMEOUT, false);
                    inf[i] = new SiteInfo { Name = s1, Latitude=0.0, Longitude=0.0, UTCOffset=0 };
                }
                inf[4] = new SiteInfo { Name = "(Restore Previous)" };
                return inf;
            }
        }

        /// <summary>
        /// select one of 4 pre-defined sites in Gemini memory
        /// </summary>
        /// <param name="siteNumber"></param>
        /// <returns></returns>
        public static bool SetSiteNumber( int siteNumber )
        {
            if (siteNumber < 0 || siteNumber >= 4) return false; // only 4 supported by Gemini
            DoCommandResult(":W" + siteNumber.ToString(), MAX_TIMEOUT, false);
            UpdateSiteInfo();
            return true;
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
        /// <param name="timeout">timeout per command in the whole sequence, in msec, or -1 for no timeout</param>
        /// <returns>the result of the last command in the array if the sequence was successfully completed,
        /// otherwise 'null'.
        /// </returns>
        /// <param name="bRaw">command is a raw string to be passed to the device unmodified</param>
        public static string DoCommandResult(string [] cmd, int timeout, bool bRaw)
        {
            if (!m_Connected) return null;

            int total_timeout = 0;

            CommandItem[] ci = new CommandItem[cmd.Length];

            for (int i = 0; i < ci.Length; ++i)
                ci[i] = new CommandItem(cmd[i], timeout, true, bRaw); //initialize all CommandItem objects

            if (!QueueCommands(ci)) return null;  // queue them all at once

            // construct an array of all the wait handles
            System.Threading.ManualResetEvent[] events = new System.Threading.ManualResetEvent[ci.Length];
            for (int i = 0; i < ci.Length; ++i)
            {
                events[i] = ci[i].WaitObject;
                total_timeout += timeout;
            }

            // success only if all wait handles are signalled by the worker thread. Return result from the last command:

            //  NOTE: WaitAll will not work on an STAThread, so instead, we wait on the last object, as these will all 
            //    be signaled in order: //was: if (System.Threading.ManualResetEvent.WaitAll(events, timeout < 0? -1 : total_timeout)) 

            if (events[ci.Length-1].WaitOne(timeout < 0 ? -1 : total_timeout)) 
                return ci[ci.Length - 1].m_Result;

            return null;
        }

        /// <summary>
        /// Execute a sequence of commands guaranteed not to be interrupted by another ASCOM client or another thread
        ///   
        ///   
        ///   The result in the case of successfull completion is an array  of Gemini generated results for
        ///   each of the commands. For commands that don't produce a result or timed out, the corresponding result 
        ///   value will be null.
        /// 
        /// </summary>
        /// <param name="cmd">an array of commands to execute</param>
        /// <param name="timeout">total timeout for the whole sequence, in msec, or -1 for no timeout</param>
        /// <param name="bRaw">command is a raw string to be passed to the device unmodified</param>
        /// <param name="result">out parameter, contains array of results for all the commands</param>
        public static void DoCommandResult(string[] cmd, int timeout, bool bRaw, out string [] result)
        {
            result = null;

            if (!m_Connected) return;

            CommandItem[] ci = new CommandItem[cmd.Length];

            for (int i = 0; i < ci.Length; ++i)
                ci[i] = new CommandItem(cmd[i], timeout, true, bRaw); //initialize all CommandItem objects

            if (!QueueCommands(ci)) return;  // queue them all at once

            int total_timeout = 0;
            // construct an array of all the wait handles
            System.Threading.ManualResetEvent[] events = new System.Threading.ManualResetEvent[ci.Length];
            for (int i = 0; i < ci.Length; ++i)
            {
                total_timeout += timeout;
                events[i] = ci[i].WaitObject;
            }

            // success only if all wait handles are signalled by the worker thread. Return result from the last command:

            //  NOTE: WaitAll will not work on an STAThread, so instead, we wait on the last object, as these will all 
            //    be signaled in order: //was: if (System.Threading.ManualResetEvent.WaitAll(events, timeout < 0? -1 : total_timeout)) 
            if (events[ci.Length - 1].WaitOne(timeout < 0 ? -1 : total_timeout)) 
            {
                result = new string [ci.Length];
                for (int i = 0; i < ci.Length; ++i)
                    result[i] = ci[i].m_Result;
            }
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
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            CommandItem ci = (CommandItem)command_item;

            if (!QueueCommand(ci))
            {
                if (ci.m_AsyncDelegate != null)
                        ci.m_AsyncDelegate(ci.m_Command, null);
                return;
            }

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
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            CommandItem [] ci = (CommandItem[])command_items;

            if (!QueueCommands(ci))
            {
                CommandItem last_ci = ci[ci.Length - 1];
                last_ci.m_AsyncDelegate(last_ci.m_Command, null);
                return;
            }

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

        /// <summary>
        /// True while PulseGuiding command is in progress
        /// </summary>
        public static bool IsPulseGuiding
        {
            get 
            {
                string l_Velocity = DoCommandResult(":Gv", MAX_TIMEOUT, false); //Get current velocity
                if (l_Velocity.ToUpper() == "G") return true; // If G then we are pulse guiding 
                else return false; // Some other rate so we are not pulse guiding
            }
        }

        /// <summary>
        /// Number of connected clients
        /// </summary>
        public static int Clients
        {
            get { return m_Clients; }
        }


        /// <summary>
        /// Returns number of commands pending in the queue
        /// </summary>
        public static int QueueDepth
        {
            get { return m_CommandQueue.Count; }
        }

        /// <summary>
        /// Wait for completion of a goto home or park at cwd operation
        /// </summary>
        /// <param name="where">'home' or 'park' for logging and exception reporting purposes</param>
        public static void WaitForHomeOrPark(string where)
        {
            Trace.Enter(4, "WaitForHomeOrPark", where);

            int count = 0;

            // wait for parking move to begin, wait for a maximum of 16*250ms = 4 seconds
            while (ParkState != "2" && count < 16) { System.Threading.Thread.Sleep(250); count++; }
            //            if (count == 16) throw new TimeoutException(where + " operation didn't start");

            // now wait for it to end
            while (ParkState == "2") { System.Threading.Thread.Sleep(1000); };

            // 0 => didn't park.
            //if (GeminiHardware.ParkState == "0") throw new DriverException("Failed to " + where, (int)SharedResources.ERROR_BASE);
            Trace.Exit(4, "WaitForHomeOrPark", where);
        }


        /// <summary>
        /// Wait for mount to stop moving and tracking
        /// </summary>
        /// <param name="timeout">how long to wait in seconds</param>
        public static bool WaitForStop(int timeout)
        {
            Trace.Enter(4, "WaitForStop");
            int count = 0;

            // wait for parking move to begin, wait for a maximum of 16*250ms = 4 seconds
            while (m_Velocity != "N" && count < timeout*4) { System.Threading.Thread.Sleep(250); count++; }
            if (m_Velocity == "N") return true;
            return false;
        }

        /// <summary>
        /// Wait for completion of asynchronous slew operation, at end wait out the slewsettle time
        /// </summary>
        public static void WaitForSlewToEnd()
        {
            Trace.Enter(4, "WaitForSlewToEnd");

            Velocity = "";

            int when = System.Environment.TickCount + 5000;
            while (System.Environment.TickCount < when && !(Velocity == "S" || Velocity == "C"))
                System.Threading.Thread.Sleep(500);

            while ((Velocity == "S" || Velocity == "C") && !m_SlewAborted.WaitOne(0)) System.Threading.Thread.Sleep(500);

            System.Threading.Thread.Sleep((SlewSettleTime + 2) * 1000);

            Trace.Exit(4, "WaitForSlewToEnd");
        }


        /// <summary>
        /// wait for one or more possible velocity states of the mount
        /// </summary>
        /// <param name="p">contains one or more letters representing velocities we are waiting for: N, T, G, C, S</param>
        /// <param name="tmout">how long to wait or -1 to wait indefinitely</param>
        /// <returns></returns>
        public static bool WaitForVelocity(string p, int tmout)
        {
            Trace.Enter(4, "WaitForVelocity", p, tmout);

            int timeout = System.Environment.TickCount + tmout;
            while ((tmout <= 0 || System.Environment.TickCount < timeout) && !p.Contains(Velocity)) System.Threading.Thread.Sleep(500);

            Trace.Exit(4, "WaitForVelocity", p, tmout, Velocity);
            if (p.Contains(Velocity)) return true;
            return false;
        }


        static private void StartStatus(object arg)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            Point pt = (Point)arg;
            Screen scr = Screen.FromPoint(pt);

            m_StatusForm = new frmStatus();
            m_StatusForm.AutoHide = true;

            Point top = (pt);
            top.Y -= m_StatusForm.Bounds.Height + 32;
            top.X -= 32;

            top.Y = Math.Min(top.Y, scr.WorkingArea.Height - m_StatusForm.Bounds.Height - 32);
            top.X = Math.Min(top.X, scr.WorkingArea.Width - m_StatusForm.Bounds.Width - 32);

            m_StatusForm.Location = top;

            m_StatusForm.Visible = true;
            m_StatusForm.Show();
            Application.Run(m_StatusForm);
        }

        private static System.Threading.Thread statusThread = null;

        public static void ShowStatus(Point pt, bool autoHide)
        {
            if (statusThread != null)
            {
                if (m_StatusForm != null && m_StatusForm.InvokeRequired)
                    m_StatusForm.BeginInvoke(new EventHandler(m_StatusForm.ShowMe));
                return;
            }
            // Create a new thread from which to start the status screen form
            statusThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(StartStatus));
            statusThread.Start(pt);

        }


        public static SerializableDictionary<string, CatalogObject> GetUserCatalog
        {
            get
            {
                SerializableDictionary<string, CatalogObject> uc = new SerializableDictionary<string, CatalogObject>();
                string snbr = DoCommandResult(":On", MAX_TIMEOUT, false);
                int nbr;
                DoCommandResult(":Os", MAX_TIMEOUT, false);
                if (!int.TryParse(snbr, out nbr)) return null;

                double incr = 1;
                if (nbr !=0) incr= 100.0 / nbr;

                for (int i = 0; i < nbr; ++i)
                {
                    frmProgress.Update(incr, null);

                    string line = DoCommandResult(":Or", MAX_TIMEOUT, false);
                    if (line == null) return null;
                    CatalogObject obj = null;
                    if (!CatalogObject.TryParse(line, "From Gemini", out obj)) return null;
                    if (!uc.ContainsKey(obj.Name))
                        uc.Add(obj.Name, obj);
                }
                return uc;
            }
        }

        public static System.Collections.Generic.List<CatalogObject> SetUserCatalog
        {
            set
            {
                DoCommandResult(":Oc", MAX_TIMEOUT, false);
                int count = 0;
                double incr = 1;
                int nbr = value.Count;
                if (nbr != 0) incr = 100.0 / nbr;

                foreach (CatalogObject obj in value)
                {
                    frmProgress.Update(incr, null);
                    string sline;
                    sline = obj.Name.Substring(0, Math.Min(obj.Name.Length, 10)) + "," + obj.RA.ToString(":", ":")+ "," + obj.DEC.ToString(":", ":");
                    DoCommandResult(":Od" + sline, MAX_TIMEOUT, false);
                    if (++count >= 4096) break; // no more!
                }
            }
        }

        public static System.Collections.Generic.List<string> ObservationLog
        {
            get
            {
                System.Collections.Generic.List<string> l = new System.Collections.Generic.List<string>();
                DoCommandResult(":OS", MAX_TIMEOUT, false);
                int cnt = 0;
                int incr = 1;
                do
                {
                    cnt += incr;

                    frmProgress.Update(incr, null);
                    string line = DoCommandResult(":OR", MAX_TIMEOUT, false);
                    if (string.IsNullOrEmpty(line) || line.Equals("END", StringComparison.InvariantCultureIgnoreCase)) break;
                    l.Add(line);
                } while (true);
                return l;
            }
            set
            {
                //clear only
                DoCommandResult(":OC", MAX_TIMEOUT, false);
            }
        }

        /// <summary>
        /// get: true if currently in the middle of executing Connect()
        /// set: false to abort connection
        /// </summary>
        public static bool IsConnecting
        {
            get
            {
                if (System.Threading.Monitor.TryEnter(m_ConnectLock))
                {
                    System.Threading.Monitor.Exit(m_ConnectLock);
                    return false;
                }
                return true;
            }
            set
            {            
                m_AbortConnect.Set();
                m_SerialTimeoutExpired.Set();
            }
        }


        public static string DoMeridianFlip()
        {
            m_SlewAborted.Reset();
            GeminiHardware.Velocity = "S";
            string[] res = null;

            // note that if Gemini executes :Mf successfully, it doesn't return a value at all, 
            // this is not as documented in the manual. So, in case of a timeout of :Mf# command
            // we'll get back the result of the '\x6' command instead, (should be 'G')

            GeminiHardware.DoCommandResult(new string[] { ":Mf#", "\x6" }, 2000, true, out res);
            if (res[0] == "G#")    // no result was returned for :Mf# --> means successful
                return "0";
            else
                if (res[1] == null) return null; // if it's really null, then both commands timed out
                else
                {
                    return res[0].TrimEnd('#'); // else it's an error code from :Mf command, return it
                }
        }



        public static void ReportAlignResult(string op)
        {
            string a, e;
            a = GeminiHardware.DoCommandResult("<201:", GeminiHardware.MAX_TIMEOUT, false);
            e = GeminiHardware.DoCommandResult("<202:", GeminiHardware.MAX_TIMEOUT, false);

            int a_err, e_err;

            if (!int.TryParse(a, out a_err)) return;
            if (!int.TryParse(e, out e_err)) return;
            if (GeminiHardware.OnInfo != null)
                GeminiHardware.OnInfo(op+ " Result", string.Format("A:{0:0.0}' E:{1:0.0}'", ((double)a_err)/60.0, ((double)e_err)/60.0));
        }

#endregion

#region Telescope Implementation   

        /// <summary>
        /// Establish connection with the mount, open serial port
        /// </summary>
        private static void Connect()
        {
            Trace.Enter("Connect()");
            lock (m_ConnectLock)   // make sure only one connection goes through at a time
            {
                m_Clients += 1;

                Trace.Info(2, "Clients=", m_Clients);

                if (!m_SerialPort.IsOpen)
                {
                    Trace.Info(2, "SerialPort.IsOpen", "false");

                    m_AbortConnect.Reset();

                    GetProfileSettings();

                    m_SerialPort.PortName = m_ComPort;
                    m_SerialPort.BaudRate = m_BaudRate;
                    m_SerialPort.Parity = (System.IO.Ports.Parity)m_Parity;
                    m_SerialPort.DataBits = m_DataBits;
                    m_SerialPort.StopBits = (System.IO.Ports.StopBits)m_StopBits;              
                    m_SerialPort.Handshake = System.IO.Ports.Handshake.None;
                    m_SerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(m_SerialPort_ErrorReceived);

                    try
                    {
                        Trace.Info(2, "Before Port.Open");
                        m_SerialPort.Open();
                
                        m_SerialPort.DtrEnable = true;
                        m_SerialPort.Encoding = Encoding.GetEncoding("Latin1");
                        Trace.Info(2, "After Port.Open");
                    }
                    catch (Exception e)
                    {
                        if (!m_ScanCOMPorts || !HuntForGemini(null))
                        {
                            m_Clients -= 1;
                            Trace.Except(e);
                            GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Serial comm error connecting to port " + m_ComPort + ":" + e.Message);
                            if (OnError != null) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.ConnectionFailed + e.Message);
                            m_Connected = false;
                            throw e;    //rethrow the exception
                        }
                    }

                    m_TargetRightAscension = SharedResources.INVALID_DOUBLE;
                    m_TargetDeclination = SharedResources.INVALID_DOUBLE;
                    m_TargetAltitude = SharedResources.INVALID_DOUBLE;
                    m_TargetAzimuth = SharedResources.INVALID_DOUBLE;
                    TargetName = null;

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

                        Trace.Info(2, "Creating worker thread");
                        m_CancelAsync = false;

                        m_BackgroundWorker = new System.Threading.Thread(BackgroundWorker_DoWork);
                        m_BackgroundWorker.Start();


                        try
                        {
                            SendStartUpCommands();
                        }
                        catch { }

                        if (m_PassThroughPortEnabled)
                            try {
                                Trace.Info(2, "Open pass-through port");
                                m_PassThroughPort = new PassThroughPort();
                                m_PassThroughPort.Initialize(m_PassThroughComPort, m_PassThroughBaudRate);   
                            } 
                            catch (Exception ptp_e)
                            {
                                Trace.Except(ptp_e);
                                GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.CannotOpenPTP +ptp_e.Message);
                                if (OnError != null) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.CannotOpenPTP + ptp_e.Message);
                                m_PassThroughPort = null;                                
                            }
                        System.Threading.Thread.Sleep(1000);

                        if (SendAdvancedSettings)
                        {
                            SetGeminiAdvancedSettings();
                        }

                    }
                    else
                    {
                        Trace.Error("Gemini is not responding. Please check that it's connected");
                        if (OnError != null) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.GeminiNotResponding);

                        Disconnect();

                        throw new ASCOM.DriverException(SharedResources.MSG_NOT_CONNECTED, (int)SharedResources.SCODE_NOT_CONNECTED);
                    }
                }

            }

            if (OnConnect != null && m_Connected) OnConnect(true, m_Clients);

            Trace.Exit("Connect()");
        }

        private static void SetGeminiAdvancedSettings()
        {
            Trace.Enter("SetGeminiAdvancedSettings");
            GeminiProperties props = new GeminiProperties();
            if (props.Serialize(false, null))   //read default profile settings
            {
                Trace.Info(2, "Apply Advanced settings");
                props.SyncWithGemini(true); //send the default profile to Gemini
            }
        }

        private static void SendStartUpCommands()
        {
            Trace.Enter("SendStartUpCommands");
            // check that the precision is set to high, if not, set it:
            string precision = DoCommandResult(":P", MAX_TIMEOUT, false);
            Trace.Info(2, "Current precision", precision);

            if (precision != "HIGH PRECISION")
            {
                Trace.Info(2, "Setting HIGH PRECISION");
                DoCommandResult(":U", MAX_TIMEOUT, false);
            }

            Trace.Info(2, "Updating refraction");
            Refraction = m_Refraction;  // update this setting to the mount 

            //Set the site and time if required
            if (m_UseDriverSite && !(m_Longitude == 0 && m_Latitude == 0))
            {
                Trace.Info(2, "Set Long/Lat/UTCOffset from PC", m_Latitude, m_Longitude, m_UTCOffset);
        
                SetLatitude(m_Latitude);
                SetLongitude(m_Longitude);
                UTCOffset = m_UTCOffset;
            }
            
            if (m_UseDriverTime)
            {
                Trace.Info(2, "Set UTC from PC", (DateTime.UtcNow + m_GPSTimeDifference).ToString());

                UTCDate = DateTime.UtcNow + m_GPSTimeDifference;
            }
            Trace.Exit("SendStartUpCommands");
        }


        static void m_SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Trace.Error("Serial Port Error", e.EventType, e);
            m_SerialErrorOccurred.Set();    //signal that an error has occurred
        }

        public static void Transmit(string s)
        {
            Trace.Enter(4, "Transmit", s);

            if (s == String.Empty) return;

            m_SerialErrorOccurred.Reset();

            if (m_SerialPort.IsOpen)
            {
                Trace.Info(4, "Before Serial Transmit", s);
                lock (m_SerialPort)
                {
                    Trace.Info(0, "Serial Transmit", s);
                    m_SerialPort.Write(Encoding.GetEncoding("Latin1").GetBytes(s), 0, s.Length);
                    m_SerialPort.BaseStream.Flush();
                    Trace.Info(4, "Finished Port.Write");
                }
            }
            if (m_SerialErrorOccurred.WaitOne(0))
            {
                Trace.Error("Tramsmit timeout", s);
                throw new TimeoutException("Serial port transmission error: "+s);
            }
            Trace.Exit(4, "Transmit");
        }


        /// <summary>
        /// Check and process the initialization status of Gemini on initial connect:
        ///   return true if already started and initialized,
        ///   otherwise, perform initialization based on configured options
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// param name="sRes" value returned by Gemini to the ^G command /param
        ///</remarks>
        private static bool StartGemini()
        {
            Trace.Enter("StartGemini");
            if (OnInfo!=null) OnInfo(Resources.ConnectingToGemini+ m_SerialPort.PortName + ", " + m_SerialPort.BaudRate.ToString(), Resources.Connecting);
            Transmit("\x6");
            System.Threading.Thread.Sleep(0);
            
            CommandItem ci = new CommandItem("\x6", 500, true); // quick timeout, don't want to hang up the user for too long
            string sRes = GetCommandResult(ci, false);

            int timeout = (m_ScanCOMPorts ? 2000 : 600000); // if not scanning, wait for 10 minutes (the user can stop it at any time, and this may be needed for bluetooth stack connection)
            Transmit("\x6");
            ci = new CommandItem("\x6", timeout, true);
            sRes = GetCommandResult(ci, false);

            if (sRes == null)
            {
                if (!m_ScanCOMPorts || !HuntForGemini(null)) return false;
                Transmit("\x6");
                ci = new CommandItem("\x6", 10000, true);
                sRes = GetCommandResult(ci,false);
            }

            Trace.Info(2, "^G result", sRes);


            if (sRes == "B")
            {
                // scrolling message? wait for it to end:
                while (sRes == "B" || sRes==null)
                {
                    Trace.Info(4, "Waiting...");
                    if (m_AbortConnect.WaitOne(0))
                    {
                        return false;
                    }

                    System.Threading.Thread.Sleep(500);
                    Transmit("\x6");
                    ci = new CommandItem("\x6", 3000, true);
                    sRes = GetCommandResult(ci, false);
                    
                    // if no response, it could be because while we did
                    // HuntForGemini previously, we may have gotten the wrong baud rate.
                    // Gemini allows a connection at 9600 baud while the scrolling message (before the boot menu)
                    // is scrolling, so try one more time to scan the same port to find the correct rate:
                    if (sRes == null) if (!HuntForGemini(m_SerialPort.PortName)) return false;
                }
            }


            if (sRes == "b")    //Gemini waiting for user to select the boot mode
            {

                Trace.Info(2, "Gemini boot menu");

                GeminiBootMode bootMode = m_BootMode;

                //ask the user what mode to boot up in
                if (bootMode == GeminiBootMode.Prompt)
                {
                    Trace.Info(2, "Prompt boot mode");

                    frmBootMode frmBoot = new frmBootMode();
                    System.Windows.Forms.DialogResult res = frmBoot.ShowDialog();
                    if (res == System.Windows.Forms.DialogResult.OK)
                        bootMode = frmBoot.BootMode;
                    else {
                        Trace.Info(2, "User canceled boot");
                        return false;   // not started, the user chose to cancel
                    }
                }

                Trace.Info(2, "Booting Gemini", bootMode);

                switch (bootMode)
                {
                    case GeminiBootMode.ColdStart: Transmit("bC#"); break;
                    case GeminiBootMode.WarmRestart: Transmit("bR#"); break;
                    case GeminiBootMode.WarmStart: Transmit("bW#"); break;
                }
                sRes = "S"; // put it into "starting" mode, so the next loop will wait for full initialization
            }

            // processing Cold start mode -- wait for this to end
            while (sRes == "S" || sRes=="b")
            {
                Trace.Info(4, "Waiting for completion", sRes);
                if (m_AbortConnect.WaitOne(0))
                {
                    return false;
                }
                System.Threading.Thread.Sleep(500);
                Transmit("\x6");
                ci = new CommandItem("\x6", 3000, true);
                sRes = GetCommandResult(ci, false); ;
            }

            Trace.Exit("Start Gemini", sRes);

            return sRes == "G"; // true if startup completed, otherwise false
        }

        /// <summary>
        /// search through all defined COM ports for Gemini
        /// try various baud rates, 4800,9600,19200
        /// </summary>
        /// 
        /// <returns></returns>
        private static bool HuntForGemini(string one_port)
        {            
            Trace.Enter("HuntForGemini", m_SerialPort.PortName, m_SerialPort.BaudRate);
            
            m_AllowErrorNotify = false;

            try
            {
                if (m_SerialPort.IsOpen) m_SerialPort.Close();

                string[] ports = SerialPort.GetPortNames();

                // if port was specified, just look at that port:
                if (one_port != null) ports = new string[] { one_port };

                int[] rates = { 9600, 4800, 19200, 38400 };

                foreach (string p in ports)
                {

                    if (OnInfo != null) OnInfo(Resources.SearchForGemini, Resources.CheckPort + " " + p);

                    for (int i = 0; i < rates.Length; ++i)
                    {
                        // abort signaled, stop now!
                        if (m_AbortConnect.WaitOne(0))
                        {
                            if (OnInfo != null) OnInfo(Resources.SearchForGemini, Resources.Stop);
                            return false;
                        }
                        m_SerialPort.PortName = p;

                        m_SerialPort.BaudRate = rates[i];
                        try
                        {
                            m_SerialPort.Open();
                            m_SerialPort.DtrEnable = true;
                            m_SerialPort.Encoding = Encoding.GetEncoding("Latin1");

                            if (m_SerialPort.IsOpen)
                            {
                                Transmit("\x6");
                                System.Threading.Thread.Sleep(0);
                                CommandItem ci = new CommandItem("\x6", 100, true); // quick timeout, don't want to hang up the user for too long
                                string sRes = GetCommandResult(ci, false);

                                Transmit("\x6");
                                System.Threading.Thread.Sleep(0);
                                ci = new CommandItem("\x6", 800, true); // quick timeout, don't want to hang up the user for too long
                                sRes = GetCommandResult(ci,false);

                                
                                if (sRes == null)
                                {
                                    m_SerialPort.Close();
                                }
                                else
                                {
                                    Trace.Info(1, "Found Gemini!", p, rates[i]);
                                    GeminiHardware.ComPort = p;
                                    GeminiHardware.BaudRate = rates[i];
                                    return true;
                                }
                            }
                        }
                        catch (Exception ex1)
                        {
                            Trace.Except(ex1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Except(ex);
            }
            finally
            {
                m_AllowErrorNotify = true;
                Trace.Exit("HuntForGemini");
            }

            return false;
        }

        /// <summary>
        /// Disconnect this client. If no more clients, close the port, disconnect from the mount.
        /// </summary>
        private static void Disconnect()
        {
            Trace.Enter("Disconnect()");

            bool bMessage = m_Connected;    // if currently connected, fire the disconnect message at the end

            lock (m_ConnectLock)
            {
                Trace.Info(2, "Current connect state", m_Connected);

                m_Clients -= 1;
                if (m_Clients < 0) m_Clients = 0;

                Trace.Info(2, "Remaining clients", m_Clients);

                if (m_Clients <= 0)
                {
                    try
                    {
                        Trace.Info(2, "No more clients, disconnect");
                        m_CancelAsync = true;

                        m_Connected = false;
                        // no new commands will be queued after m_Connected is set to false, clear out
                        // anything remaining:
                        m_CommandQueue.Clear();

                        // wait for the thread to die for 5 seconds,
                        // then kill it -- we don't want to tie up the serial comm
                        if (m_BackgroundWorker != null)
                        {
                            Trace.Info(2, "Stopping bkgd thread");
                            m_WaitForCommand.Set(); // wake up the background thread
                            try
                            {
                                if (!m_BackgroundWorker.Join(2000))
                                    m_BackgroundWorker.Abort();
                            }
                            catch { }
                            Trace.Info(2, "Bkgd thread stopped");
                        }

                        //Transmit(":Q#"); // stop all slews, in case we are in the middle of one

                        Trace.Info(2, "Closing serial port");
                        m_SerialPort.Close();
                        Trace.Info(2, "Serial port closed");

                        m_BackgroundWorker = null;

                        Trace.Info(2, "Closing pass-through port");
                        if (m_PassThroughPort != null) m_PassThroughPort.Stop();
                        Trace.Info(2, "Pass-through port closed");
                    }
                    catch { }
                    CloseStatusForm();
                }
            }

            if (OnConnect != null && bMessage) OnConnect(false, m_Clients);

            Trace.Exit("Disconnect()");
        }

        public static void CloseStatusForm()
        {
            GeminiHardware.Trace.Info(4, "Before closing status form");

            if (m_StatusForm != null && m_StatusForm.InvokeRequired)
            {
                GeminiHardware.Trace.Info(4, "Before BeginInvoke status form");
                m_StatusForm.BeginInvoke(new EventHandler(m_StatusForm.ShutDown));

                GeminiHardware.Trace.Info(4, "After BeginInvoke status form");

                if (statusThread != null)
                {
                    if (!statusThread.Join(2000))
                    {
                        GeminiHardware.Trace.Info(4, "Failed to stop thread: status form");
//                        statusThread.Abort();
                    }
                    statusThread = null;

                }
                m_StatusForm = null;
                GeminiHardware.Trace.Info(4, "After closing status form");
            }
        }

        /// <summary>
        /// Process queued up commands in the sequence queued.
        /// </summary>
        private static void BackgroundWorker_DoWork()
        {

            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            Trace.Enter("BackgroundWorker thread");

            while (!m_CancelAsync && m_SerialPort.IsOpen)
            {
                object [] commands = null;

                lock (m_CommandQueue)
                {
                    // process multiple commands, if more than one is queued up:
                    if (m_CommandQueue.Count > 0)
                    {
                        Trace.Info(4, "Command queue depth", m_CommandQueue.Count);
                        // gemini doesn't like too many long commands (buffer size problem?)
                        // remove up to x commands at a time
                        int cnt = Math.Min(5, m_CommandQueue.Count);

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
                                if (ci.m_Command.Length > 0 && (ci.m_Command[0] == '<' || ci.m_Command[0] == '>'))
                                    serial_cmd = CompleteNativeCommand(ci.m_Command);
                                else
                                    serial_cmd = CompleteStandardCommand(ci.m_Command);
                            }
                            all_commands += serial_cmd;
                            if (ci.WaitObject == null) ci.m_Timeout = 2000;    // default timeout for requests where the user doesn't care
                        }

                        DiscardInBuffer();

                        Trace.Info(2, "Transmitting commands", all_commands);

                        int startTime = System.Environment.TickCount;

                        Transmit(all_commands);

                        Trace.Info(2, "Done transmitting");

                        foreach (CommandItem ci in commands)
                        {
                            Trace.Info(4, "Waiting for", ci.m_Command);

                            // wait for the result whether or not the caller wants it
                            // otherwise delayed result from a previous command
                            // can be falsely returned for a later request:

                            string result = GetCommandResult(ci);

                            Trace.Info(4, "Result", result);


                            if (ci.WaitObject != null)    // receive result, if one is expected
                            {
                                ci.m_Result = result;
                                ci.WaitObject.Set();   //release the wait handle so the calling thread can continue
                            }

                            // if this command is one of the status variables to be polled, make a note to update 
                            // status ASAP!
                            if (ci.m_UpdateRequired)
                            {
                                bNeedStatusUpdate = true;
                                Trace.Info(4, "Status Update requested");
                            }
                        }

                        if (bNeedStatusUpdate || (DateTime.Now - m_LastUpdate).TotalMilliseconds > SharedResources.GEMINI_POLLING_INTERVAL)
                        {
                            m_AllowErrorNotify = false; //don't bother the user with timeout errors during polling  -- these are not very important
                            UpdatePolledVariables(bNeedStatusUpdate); //update variables if one of them was altered by a processed command
                            m_AllowErrorNotify = true;
                        }
                    }
                    else
                    {
                        m_AllowErrorNotify = false; //don't bother the user with timeout errors during polling  -- these are not very important
                        UpdatePolledVariables(false);
                        m_AllowErrorNotify = true;
                    }
                }
                catch (Exception ex)
                {
                    Trace.Error("Unexpected exception", ex.ToString());
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
                int waitfor =  SharedResources.GEMINI_POLLING_INTERVAL - (int)(DateTime.Now - m_LastUpdate).TotalMilliseconds;
                Trace.Info(4, "Sleep (msec)", waitfor);
                if (waitfor > 0)
                    m_WaitForCommand.WaitOne(waitfor);
            }

            Trace.Exit("BackgroundWorker thread", m_CancelAsync, m_SerialPort.IsOpen);
            m_CancelAsync = false;

        }

        private static void UpdateSiteInfo()
        {
            string longitude = DoCommandResult(":Gg", MAX_TIMEOUT, false);
            string latitude = DoCommandResult(":Gt", MAX_TIMEOUT, false);
            string UTC_Offset = DoCommandResult(":GG", MAX_TIMEOUT, false);

            try
            {
                if (longitude != null) m_Longitude = -m_Util.DMSToDegrees(longitude);  // Gemini has the reverse notion of longitude sign: + for West, - for East}
            }
            catch (Exception ex)
            {
                Trace.Except(ex);
            }
            try
            {
                if (latitude != null) m_Latitude = m_Util.DMSToDegrees(latitude);
            }
            catch (Exception ex)
            {
                Trace.Except(ex);
            }

            try
            {
                if (UTC_Offset != null) int.TryParse(UTC_Offset, out m_UTCOffset);
            }
            catch (Exception ex)
            {
                Trace.Except(ex);
            }


        }
        /// <summary>
        /// Discard anything in the in-buffer, but keep all the binary 
        /// data, as it may be needed by the software on the other side of the
        /// pass-through port
        /// </summary>
        /// 
        private static void DiscardInBuffer()
        {
            StringBuilder sb = new StringBuilder();

            if (m_PassThroughPort!=null && m_PassThroughPort.PortActive)
            {
                while (m_SerialPort.BytesToRead > 0)
                {
                    int c = m_SerialPort.ReadByte();
                    if (c>=0x80) sb.Append(Convert.ToChar(c));
                }
                if (sb.Length > 0 && (m_PassThroughPort!=null && m_PassThroughPort.PortActive))
                    m_PassThroughPort.PassStringToPort(sb);
            }
            else m_SerialPort.DiscardInBuffer();
        }

        /// <summary>
        /// update all variable sthat are polled on an interval
        /// </summary>
        private static void UpdateInitialVariables()
        {
            CommandItem command;

            DiscardInBuffer(); //clear all received data

            //longitude, latitude, UTC offset
            Transmit(":GV#:Gg#:Gt#:GG#");

            //verify that Gemini is at least Level 4
            command = new CommandItem(":GV", MAX_TIMEOUT, true);
            string ver = GetCommandResult(command);
            if (ver != null)
            {
                if (ver.EndsWith("#")) ver = ver.Substring(0, ver.Length - 1);
                int v;
                if (int.TryParse(ver, out v))
                {
                    if (v / 100 < 4)   //level below 4!
                    {
                        Disconnect();

                        if (OnError != null) OnError(SharedResources.TELESCOPE_DRIVER_NAME, SharedResources.MSG_GEMINI_VERSION);
                        throw new DriverException(SharedResources.MSG_GEMINI_VERSION, (int)SharedResources.SCODE_GEMINI_VERSION);
                    }
                    m_GeminiVersion = ver;
                }
            }


            command = new CommandItem(":Gg", MAX_TIMEOUT, true);
            string longitude = GetCommandResult(command);

            command = new CommandItem(":Gt", MAX_TIMEOUT, true);
            string latitude = GetCommandResult(command);

            command = new CommandItem(":GG", MAX_TIMEOUT, true);
            string UTC_Offset = GetCommandResult(command);


            try
            {
                if (longitude != null && !UseDriverSite) Longitude = -m_Util.DMSToDegrees(longitude);  // Gemini has the reverse notion of longitude sign: + for West, - for East}
            }
            catch (Exception ex)
            {
                Trace.Except(ex);
            }
            try
            {
                if (latitude != null && !UseDriverSite) Latitude = m_Util.DMSToDegrees(latitude);
            }
            catch (Exception ex)
            {
                Trace.Except(ex);
            }
            try
            {
                if (UTC_Offset != null && !UseDriverSite) int.TryParse(UTC_Offset, out m_UTCOffset);
            }
            catch (Exception ex)
            {
                Trace.Except(ex);
            }


            try
            {
                //Get RA and DEC etc
                UpdatePolledVariables(true);
            }
            catch (Exception ex)
            {
                Trace.Except(ex);
            }
            m_LastUpdate = System.DateTime.Now;
        }


        static private int m_PollUpdateCount = 0;    // keep track of number of updates


        /// <summary>
        /// update all variable sthat are polled on an interval
        /// if UpdateAll is true, all polled variables are queries
        ///  otherwise, some variables are queried less frequently to
        ///  reduce serial port traffic and load on Gemini and PC
        /// </summary>
        /// 
        private static void UpdatePolledVariables(bool UpdateAll)
        {

            Trace.Enter("UpdatePolledVariables", UpdateAll);
            try
            {
                CommandItem command;

                // Gemini gets slow to respond when slewing, so increase timeout if we're in the middle of it:
                //                int timeout = (m_Velocity == "S" ? MAX_TIMEOUT*2 : MAX_TIMEOUT);

                int timeout = 3000; // polling should not hold up the queue for too long

                int level = 0;
                string vars;

                m_PollUpdateCount++;

                if (UpdateAll)
                {
                    level = 3;   // update all
                    vars = m_PolledVariablesString;
                }
                else
                    if ((m_PollUpdateCount & 1) == 0)   // update set #1
                    {
                        level = 1;
                        vars = m_ShortPolledVariablesString1;
                    }
                    else
                    {
                        level = 2;          // update set #2
                        vars = m_ShortPolledVariablesString2;
                    }

#if DEBUG
                System.Diagnostics.Trace.Write("Poll commands: " + vars + "\r\n");
#endif

                Trace.Info(4, "Poll commands", vars);
                //Get RA and DEC etc
                DiscardInBuffer(); //clear all received data
                Transmit(vars);

                string trc = "";


                if ((level & 1) != 0)
                {
                    command = new CommandItem(":GR", timeout, true);
                    string RA = GetCommandResult(command);
                    if (RA == null)
                    {
                        Trace.Error("timeout", command.m_Command);
                        return;
                    }

                    command = new CommandItem(":GD", timeout, true);
                    string DEC = GetCommandResult(command);
                    if (DEC == null)
                    {
                        Trace.Error("timeout", command.m_Command);
                        return;
                    }

                    command = new CommandItem(":GA", timeout, true);
                    string ALT = GetCommandResult(command);
                    if (ALT == null)
                    {
                        Trace.Error("timeout", command.m_Command);
                        return;
                    }


                    command = new CommandItem(":GZ", timeout, true);
                    string AZ = GetCommandResult(command);

                    if (AZ == null)
                    {
                        Trace.Error("timeout", command.m_Command);
                        return;
                    }

                    command = new CommandItem(":Gv", timeout, true);
                    string V = GetCommandResult(command);
                    if (V == null)
                    {
                        Trace.Error("timeout", command.m_Command);
                        return;
                    }

                    if (RA != null) m_RightAscension = m_Util.HMSToHours(RA);
                    if (DEC != null) m_Declination = m_Util.DMSToDegrees(DEC);
                    if (ALT != null) m_Altitude = m_Util.DMSToDegrees(ALT);
                    if (AZ != null) m_Azimuth = m_Util.DMSToDegrees(AZ);
                    if (V != null) m_Velocity = V;
                    trc = "RA=" + RA + ", DEC=" + DEC + "ALT=" + ALT + " AZ=" + AZ + " Velocity=" + Velocity;

                }

                if ((level & 2) != 0)
                {
                    command = new CommandItem(":GS", timeout, true);
                    string ST = GetCommandResult(command);
                    if (ST == null)
                    {
                        Trace.Error("timeout", command.m_Command);
                        return;
                    }
                    command = new CommandItem(":Gm", timeout, true);
                    string SOP = GetCommandResult(command);
                    if (SOP == null)
                    {
                        Trace.Error("timeout", command.m_Command);
                        return;
                    }
                    command = new CommandItem(":h?", timeout, true);
                    string HOME = GetCommandResult(command);
                    if (HOME == null)
                    {
                        Trace.Error("timeout", command.m_Command);
                        return;
                    }

                    command = new CommandItem("<99:", timeout, true);
                    string STATUS = GetCommandResult(command);
                    if (STATUS == null)
                    {
                        Trace.Error("timeout", command.m_Command);
                        return;
                    }

                    if (Velocity == "N") m_Tracking = false;
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

                    if (m_ParkWasExecuted && Velocity != "N") //unparked!
                    {
                        m_AtPark = false;   //not parked anymore
                        m_ParkWasExecuted = false;
                    }
                    else
                        if (m_ParkWasExecuted && Velocity == "N")
                            m_AtPark = true;

                    if (STATUS != null)
                    {
                        int.TryParse(STATUS, out m_GeminiStatusByte);

                        // if reached safety limit, send out one notification 
                        if ((m_GeminiStatusByte & 16) != 0 && !m_SafetyNotified)
                        {
                            if (OnSafetyLimit != null) OnSafetyLimit();
                            m_SafetyNotified = true;
                        }
                        else if ((m_GeminiStatusByte & 16) == 0) m_SafetyNotified = false;
                    }

                    trc += " SOP=" + SOP + " HOME=" + HOME + " Status=" + m_GeminiStatusByte.ToString();
                }


                Trace.Info(4, trc);

#if DEBUG
                System.Diagnostics.Trace.Write("Done polling: " + trc + "\r\n");
#endif

                m_LastUpdate = System.DateTime.Now;
            }
            catch (Exception e)
            {
                Trace.Except(e);
                m_SerialPort.DiscardOutBuffer();
                DiscardInBuffer();
            }

            Trace.Exit("UpdatePolledVariables");
        }

        /// <summary>
        /// After a timeout error, resync with the mount
        ///  by waiting for a proper response to ^G command
        ///  all in/out buffers are discarded to make sure
        ///  commands and their results are synchronized after 
        ///  this
        /// </summary>
        public static void Resync()
        {
            if (!m_Connected || m_BackgroundWorker== null || !m_BackgroundWorker.IsAlive) return;

            Trace.Enter("Resync");

            if (m_SerialPort.IsOpen)
            {
                lock (m_CommandQueue)
                {
                    string sRes = null;
                    int count = 3;
                    do
                    {
                        try
                        {
                            m_SerialPort.DiscardOutBuffer();
                            DiscardInBuffer();

                            Transmit("\x6");
                            CommandItem ci = new CommandItem("\x6", 1000, true);
                            sRes = GetCommandResult(ci, false);

                            // if Gemini is booting, up go ahead and
                            // process the boot start-up sequence, including prompt, if so configured:
                            if (sRes == "S" || sRes == "b" || sRes == "B")
                            {
                                ErrorDelegate temp = OnInfo;
                                OnInfo = null;  //disable connect notifications, don't want to confuse the user!

                                if (StartGemini())  // got a connection!
                                {
                                    OnInfo = temp;
                                    sRes = "G"; // done
                                    break;
                                }
                                OnInfo = temp;  // nope, false alarm. continue trying to sync.
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.Except(ex);
                        }

                    } while (sRes != "G" && --count > 0);

                    if (sRes=="G")
                    {
                        Trace.Info(2, "Got a sync");
                        System.Threading.Thread.Sleep(1000);
                    }
                    else
                        Trace.Info(2, "Didn't get a sync, giving up!");

                    m_SerialPort.DiscardOutBuffer();
                    DiscardInBuffer();
                }
            }
            Trace.Exit("Resync");
        }


        /// <summary>
        /// Wait for a proper response from Gemini for a given command. Command has already been sent.
        /// this version of the method will automatically force a ReSync if an error is detected
        /// </summary>
        /// <param name="command">actual command sent to Gemini</param>
        /// <returns>result received from Gemini, or null if no result, timeout, or bad result received</returns>
        private static string GetCommandResult(CommandItem command)
        {
            return GetCommandResult(command, true);
        }

        /// <summary>
        /// Wait for a proper response from Gemini for a given command. Command has already been sent.
        /// 
        /// </summary>
        /// <param name="command">actual command sent to Gemini</param>
        /// <param name="bResyncOnError">true if driver should resync if an error was detected</param>
        /// <returns>result received from Gemini, or null if no result, timeout, or bad result received</returns>
        private static string GetCommandResult(CommandItem command, bool bResyncOnError)
        {
            string result = null;

            if (!m_SerialPort.IsOpen) return null;

            Trace.Enter(4, "GetCommandResult", command.m_Command);

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
                Trace.Info(0, "Serial wait for response", command.m_Command);

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
                        {
                            result += ReadTo('#');
                            if (command.m_Raw) //Raw should return the full string including #
                                result += "#";
                        }
                        break;

                    // value '0' or a string terminated by '#'
                    case GeminiCommand.ResultType.ZeroOrHash:
                        result = ReadNumber(1);
                        if (result != "0")
                        {
                            result += ReadTo('#');
                            if (command.m_Raw) //Raw should return the full string including #
                                result += "#";
                        }
                        break;

                    // string terminated by '#'
                    case GeminiCommand.ResultType.HashChar:
                        result = ReadTo('#');
                        if (command.m_Raw) //Raw should return the full string including #
                            result += "#";
                        break;

                        // '0' or two strings, each terminated with '#' (:SC command)
                    case GeminiCommand.ResultType.ZeroOrTwoHash:
                        result = ReadNumber(1);
                        if (result != "0")
                        {
                            result += ReadTo('#');
                            if (command.m_Raw) result += '#';
                            result += ReadTo('#');
                            if (command.m_Raw) result += '#';
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                if (m_AllowErrorNotify)
                {
                    Trace.Except(ex);
                    GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Timeout error occurred after " + command.m_Timeout + "msec while processing command '" + command.m_Command + "'");
                    if (OnError != null && m_Connected) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.SerialTimeout);
                }

                AddOneMoreError();

                if (bResyncOnError) Resync();


                return null;
            }
            finally
            {
                tmrReadTimeout.Stop();
            }


            if (m_SerialErrorOccurred.WaitOne(0))
            {
                Trace.Error("Serial port error", command.m_Command);

                GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Serial comm error reported while processing command '" + command.m_Command + "'");
                if (OnError != null && m_Connected && m_AllowErrorNotify) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.SerialError);
                AddOneMoreError();
                return null;  // error occurred!
            }

            if (result!=null)
                m_LastDataTick = DateTime.Now;      // remember when last successfull data was received.

            // return value for native commands has a checksum appended: validate it and remove it from the return string:
            if (!string.IsNullOrEmpty(result) && (command.m_Command[0] == '<' || command.m_Command[0]=='>') && !command.m_Raw)
            {
                char chksum = result[result.Length - 1];
                result = result.Substring(0, result.Length - 1); //remove checksum character

                if ((((int)chksum) & 0x7f) != (ComputeChecksum(result)&0x7f))  // bad checksum -- ignore the return value! 
                {
                    Trace.Error("Bad Checksum", command.m_Command, result);

                    GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Serial comm error (bad checksum) while processing command '" + command.m_Command + "'");
                    if (OnError != null && m_Connected && m_AllowErrorNotify) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.SerialError);
                    AddOneMoreError();
                    result = null;
                }
            }

            Trace.Info(0, "Serial received:", command.m_Command, result);
            Trace.Exit(4, "GetCommandResult", command.m_Command, result);
            return result;
        }

        /// <summary>
        /// Add one more error to the total error tally
        /// if number of errors in the defined interval (MAXIMUM_ERROR_INTERVAL) exceeds specified number (MAXIMUM_ERRORS)
        ///   assume that Gemini is off-line or some other catastrophic failure has occurred.
        ///   Send a message to the user through OnError event to fix the problem,
        ///   reset pending communication queues, and wait a defined "cool-down" interval of (RECOVER_SLEEP)
        ///   then, resume processing.
        /// </summary>
        private static void AddOneMoreError()
        {
            Trace.Enter("AddOneMoreError", m_TotalErrors);

            if (!Connected || m_BackgroundWorker == null || !m_BackgroundWorker.IsAlive) return;    //not ready yet

            if (Connected && DateTime.Now - m_LastDataTick  > TimeSpan.FromSeconds(SharedResources.MAXIMUM_DISCONNECT_TIME))
            {
                string msg = "No response for " + (SharedResources.MAXIMUM_DISCONNECT_TIME).ToString() + " secs";
                Trace.Error(msg, "Teminating connection!");
                GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, msg + " Terminating connection!");
                if (OnError != null && m_Connected) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.TerminatingConnection );
                while (m_Connected) Disconnect();
                return;
            }

            // if this is the first error, or if it's been longer than maximum interval since the last error, start from scratch
            if (m_TotalErrors == 0)
                m_FirstErrorTick = System.Environment.TickCount;

            if (m_FirstErrorTick + SharedResources.MAXIMUM_ERROR_INTERVAL < System.Environment.TickCount)
            {
                m_FirstErrorTick = System.Environment.TickCount;
                m_TotalErrors = 0;
            }

            if (++m_TotalErrors > SharedResources.MAXIMUM_ERRORS)
            {
                Trace.Error("Too many errors");

                if (OnError != null && m_Connected && m_AllowErrorNotify) OnError(SharedResources.TELESCOPE_DRIVER_NAME, Resources.TooManyErrors);
                GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Too many serial port errors in the last " + SharedResources.MAXIMUM_ERROR_INTERVAL / 1000 + " seconds. Resetting serial port.");

                lock (m_CommandQueue) // remove all pending commands, keep the queue locked so that the worker thread can't process during port reset
                {
                    Trace.Info(0, "Resetting serial port");

                    m_CommandQueue.Clear();
                    DiscardInBuffer();
                    m_SerialPort.DiscardOutBuffer();
                    try
                    {
                        Trace.Info(2, "Closing port");
                        m_SerialPort.Close();
                        System.Threading.Thread.Sleep(SharedResources.RECOVER_SLEEP);

                        Trace.Info(0, "Opening port");
                        m_SerialPort.Open();
                        m_SerialPort.Encoding = Encoding.GetEncoding("Latin1");
                    }
                    catch (Exception ex)
                    {
                        Trace.Except(ex);
                        GeminiError.LogSerialError(SharedResources.TELESCOPE_DRIVER_NAME, "Cannot reset serial port after errors: " + ex.Message);
                    }
                }
                m_TotalErrors = 0;
                m_FirstErrorTick = 0;
            }
            //else Resync();

            Trace.Exit("AddOneMoreError");
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
            StringBuilder res = new StringBuilder();

            StringBuilder outp = new StringBuilder();

            for (; ; )
            {
                while (m_SerialPort.BytesToRead > 0)
                {
                    int b = m_SerialPort.ReadByte();
                    char c = (char)(b);

                    if (c != terminate)
                    {
                        // 223 = degree character, the only char > 0x80 that's used in normal
                        // response to commands (longitude, latitude, etc.) 
                        // it must occur inside the string to be a legitimate response,
                        // otherwise consider it part of a binary stream meant for the passthrough port
                        if ((int)c >= 0x80 && (c!=223 || res.Length==0)) outp.Append(c);
                        else
                            res.Append(c);
                    }
                    else
                    {
                        if (outp.Length > 0 && (m_PassThroughPort!=null && m_PassThroughPort.PortActive))
                            m_PassThroughPort.PassStringToPort(outp);
                        return res.ToString();
                    }
                }
                if (m_SerialTimeoutExpired.WaitOne(0))
                {
                    if (outp.Length > 0 && (m_PassThroughPort!=null && m_PassThroughPort.PortActive))
                        m_PassThroughPort.PassStringToPort(outp);
                    throw new TimeoutException("ReadTo");
                }

//                    System.Threading.Thread.Sleep(0);  //[pk] should instead wait on a waithandle set by serialdatareceived event...
                try { m_DataReceived.WaitOne(250); }
                catch { }
            }
        }

        /// <summary>
        /// Read exact number of characters from the serial port, honoring the read timeout
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        private static string ReadNumber(int chars)
        {
            StringBuilder res = new StringBuilder();
            StringBuilder outp = new StringBuilder();

            for (; ; )
            {
                while (m_SerialPort.BytesToRead > 0)
                {
                    byte c = (byte)m_SerialPort.ReadByte();
                    if ((int)c >= 0x80)
                        outp.Append(Convert.ToChar(c));
                    else
                        res.Append(Convert.ToChar(c));

                    if (res.Length == chars)
                    {
                        if (outp.Length > 0 && (m_PassThroughPort!=null && m_PassThroughPort.PortActive))
                            m_PassThroughPort.PassStringToPort(outp);
                        return res.ToString();
                    }
                }
                if (m_SerialTimeoutExpired.WaitOne(0))
                {
                    if (outp.Length > 0 && (m_PassThroughPort!=null && m_PassThroughPort.PortActive))
                        m_PassThroughPort.PassStringToPort(outp);
                    throw new TimeoutException("ReadNumber");
                }
//                    System.Threading.Thread.Sleep(0);   //[pk] should instead wait on a waithandle set by serialdatareceived event...
                try { m_DataReceived.WaitOne(250); }
                catch { }
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

            bool found = GeminiCommands.Commands.ContainsKey(full_cmd);

            if (full_cmd.StartsWith("<") && !found)       // native get command is always '#' terminated
                return new GeminiCommand(GeminiCommand.ResultType.HashChar, 0);
            else if (full_cmd.StartsWith(">"))  // native set command always no return value
            {
                int idx = full_cmd.IndexOf(':');
                if (idx > 0)
                {
                    string nc = full_cmd.Substring(0, idx + 1);
                    if (GeminiCommands.Commands.ContainsKey(nc))
                        return GeminiCommands.Commands[nc];
                }
                return new GeminiCommand(GeminiCommand.ResultType.NoResult, 0);
            }
            else
            {
                if (found)
                    return GeminiCommands.Commands[full_cmd];

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
                // make sure we are fully connected/disconnected before returning a status,
                // so that another worker thread doesn't come in with a request while a connect is 
                // still in progress [pk]
                if (!m_Connected) return false;

                lock (m_ConnectLock) { return m_Connected; }
            }
            set
            {
                if (value)
                {                    
                    Connect();
                }
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
        /// called when a stop is signaled by the user
        /// this will interrupt everything else the driver is doing
        /// if background worker is waiting for a command result, it will be discarded
        /// the background command queue is suspended while this command is issued
        /// </summary>        
        public static void AbortSlew()
        {
            Trace.Enter(4, "GeminiHardware:AbortSlew");
            lock (m_CommandQueue)
            {
                m_SlewAborted.Set();
                Transmit(":Q#");    //force the transmit ahead of anything in the queue
            }
            Trace.Exit(4, "GeminiHardware:AbortSlew");
        }

        /// <summary>
        /// Park using specified park position mode
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static void DoPark(object mode)
        {           
            m_Trace.Enter("DoPark", mode);

            if (!Connected) return;

            m_SlewAborted.Reset();

            if (((GeminiParkMode)mode) != GeminiParkMode.NoSlew)
            {
                if (GeminiHardware.NudgeFromSafety && GeminiHardware.AtSafetyLimit)
                    DoNudgeFromSafety();
            }

            bool wait = true;

            switch ((GeminiParkMode)mode)
            {
                case GeminiParkMode.NoSlew:
                    wait = false;
                    break;  // already there
                case GeminiParkMode.SlewCWD:
                    GeminiHardware.DoCommandResult(":hC", GeminiHardware.MAX_TIMEOUT, false);
                    break;
                case GeminiParkMode.SlewHome:
                    GeminiHardware.DoCommandResult(":hP", GeminiHardware.MAX_TIMEOUT, false);
                    break;
                case GeminiParkMode.SlewAltAz:
                    m_TargetAltitude = ParkAlt;
                    m_TargetAzimuth = ParkAz;
                    try
                    {
                        SlewHorizonAsync();
                    }
                    catch (Exception ex)
                    {
                        m_Trace.Error(ex.Message);
                        return;
                    }
                    break;
            }

            if (wait) WaitForSlewToEnd();

            if (m_SlewAborted.WaitOne(0))
            {
                m_ParkWasExecuted = false;
                m_Trace.Exit("DoPark", false);
                return;
            }

            DoCommandResult(":hN", MAX_TIMEOUT, false);

            if (!WaitForStop(120))    // wait for a stop for 2 minutes, then fail
            {
                m_Trace.Error("Failed to stop DoPark");
                return;
            }
            m_ParkWasExecuted = true;

            m_Trace.Exit("DoPark", true);
        }

        /// <summary>
        /// Perform park operation on a worker thread
        /// </summary>
        /// <returns></returns>
        public static bool DoParkAsync(GeminiParkMode mode)
        {
            if (!Connected) return false;
            System.Threading.ThreadPool.QueueUserWorkItem(DoPark, mode );
            return true;
        }



        /// <summary>
        /// Get Set current TargetRightAscention propery
        /// </summary>
        public static double TargetRightAscension
        { 
            get { return m_TargetRightAscension; }
            set { 
                m_TargetRightAscension = value;
                TargetName = null;
            }
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
            set {
                m_TargetAltitude = value;
                TargetName = null;
            }
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
        /// Get/Set current UTC propery
        /// </summary>
        public static DateTime UTCDate
        { 
            get 
            {
                try
                {
                    DateTime geminiDateTime = DateTime.Now ;
                    string l_Time = GeminiHardware.DoCommandResult(":GL", GeminiHardware.MAX_TIMEOUT, false);
                    string l_Date = GeminiHardware.DoCommandResult(":GC", GeminiHardware.MAX_TIMEOUT, false);
                    double l_TZOffset = double.Parse(GeminiHardware.DoCommandResult(":GG", GeminiHardware.MAX_TIMEOUT, false));
                    geminiDateTime = DateTime.ParseExact(l_Date + " " + l_Time, "MM/dd/yy HH:mm:ss", new System.Globalization.DateTimeFormatInfo()); // Parse to a local datetime using the given format
                    geminiDateTime = geminiDateTime.AddHours(l_TZOffset); // Add this to the local time to get a UTC date time
                    return geminiDateTime;
                }
                catch (Exception ex)
                { throw new ASCOM.DriverException("Error reading UTCDate: " + ex.ToString(), (int)SharedResources.SCODE_INVALID_VALUE); }; 
            }
            set 
            {
                int utc_offset_hours = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;

                // set timezone from PC (gemini seems to want a '+' or a '-' in front of the hours, so make sure positive
                // number gets a '+' in front:
                string result = DoCommandResult(":SG" + (utc_offset_hours < 0? "+":"") + (-utc_offset_hours).ToString("0"), MAX_TIMEOUT, false);

                // compute civil time using whole hours only, since Gemini doesn't take fractions:
                DateTime civil = value + TimeSpan.FromHours(utc_offset_hours);

                string localTime = civil.ToString("HH:mm:ss");
                string localDate = civil.ToString("MM/dd/yy");
                result = DoCommandResult(":SC" + localDate, MAX_TIMEOUT, false);
                result = DoCommandResult(":SL" + localTime, MAX_TIMEOUT, false);
            }
        }

        /// <summary>
        /// SetLatitude Method
        /// Stores the Latitude in the Gemini Computer
        /// </summary>
        public static void SetLatitude(double Latitude)
        {         
            m_Latitude = Latitude;
            string latitudedddmm = m_Util.DegreesToDM(Latitude, "*", "");
            string result = DoCommandResult(":St" + latitudedddmm, MAX_TIMEOUT, false);
            if (result == null || result != "1")
                throw new ASCOM.Utilities.Exceptions.InvalidValueException("Latitidue not set");
        }

        /// <summary>
        /// SetLatitude Method
        /// Stores the Latitude in the Gemini Computer
        /// </summary>
        public static void SetLongitude(double Longitude)
        {
            m_Longitude = Longitude;
            string longitudedddmm = m_Util.DegreesToDM(-Longitude, "*", "");  // Gemini has the reverse notion of longitude sign: + for West, - for East
            string result = DoCommandResult(":Sg" + longitudedddmm, MAX_TIMEOUT, false);
            if (result == null || result != "1")
                throw new ASCOM.Utilities.Exceptions.InvalidValueException("Longitude not set");
        }

        /// <summary>
        /// Get current SiderealTime propery
        /// retrieved from the latest polled value from the mount, no actual command is executed
        /// </summary>
        public static string Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }


        public static bool Slewing
        {
            get
            {
                string res = DoCommandResult("<99:", MAX_TIMEOUT, false);
                int status;
                if (int.TryParse(res, out status))
                {
                    if ((status & 8) != 0) return true;
                }
                else
                    throw new TimeoutException("Slewing property");

                return false;
            }
        }
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
            SyncToEquatorialCoords(m_TargetRightAscension, m_TargetDeclination);
        }


        /// <summary>
        /// Syncs the mount using Ra and Dec coordinates passed in
        /// </summary>
        public static void SyncToEquatorialCoords(double ra, double dec)
        {
            string[] cmd = { ":Sr" + m_Util.HoursToHMS(ra, ":", ":", ""), 
                             ":ON" + (TargetName??"PC Object"),
                             ":Sd" + m_Util.DegreesToDMS(dec, ":", ":", ""), "" };
            TargetName = null;

            if (m_AdditionalAlign)
            {
                cmd[3] = ":Cm";
            }
            else
            {
                cmd[3] = ":CM";
            }
            string[] result = null;
            DoCommandResult(cmd, MAX_TIMEOUT / 2, false, out result);
            if (result == null || result[0] == null || result[2] == null || result[3] == null)
                throw new TimeoutException((m_AdditionalAlign ? "Align to" : "Sync to ") + "RA/DEC");
            if (result[0] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("RA value is invalid");
            if (result[2] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("DEC value is invalid");
            if (result[3] == "No object!") throw new ASCOM.Utilities.Exceptions.InvalidValueException((m_AdditionalAlign ? "Align to" : "Sync to ") + "RA/DEC");
            
            m_RightAscension = ra;// Update state machine variables with new RA and DEC.
            m_Declination = dec;
        
        }

        /// <summary>
        /// Syncs the mount using Ra and Dec
        /// </summary>
        public static void AlignEquatorial()
        {
            AlignToEquatorialCoords(m_TargetRightAscension, m_TargetDeclination);
        }


        /// <summary>
        /// Syncs the mount using Ra and Dec coordinates passed in
        /// </summary>
        public static void AlignToEquatorialCoords(double ra, double dec)
        {
            string[] cmd = { ":Sr" + m_Util.HoursToHMS(ra, ":", ":", ""), 
                             ":ON" + (TargetName??"PC Object"),
                             ":Sd" + m_Util.DegreesToDMS(dec, ":", ":", ""), "" };
            TargetName = null;

            if (m_AdditionalAlign)
            {
                cmd[3] = ":CM";
            }
            else
            {
                cmd[3] = ":Cm";
            }
            string[] result = null;
            DoCommandResult(cmd, MAX_TIMEOUT / 2, false, out result);
            if (result == null || result[0] == null || result[2] == null || result[3] == null)
                throw new TimeoutException((m_AdditionalAlign ? "Align to" : "Sync to ") + "RA/DEC");
            if (result[0] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("RA value is invalid");
            if (result[2] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("DEC value is invalid");
            if (result[3] == "No object!") throw new ASCOM.Utilities.Exceptions.InvalidValueException((m_AdditionalAlign ? "Align to" : "Sync to ") + "RA/DEC");

            m_RightAscension = ra;// Update state machine variables with new RA and DEC.
            m_Declination = dec;

        }

        /// <summary>
        /// Distance from western safety limit, in clusters of 256 encoder ticks
        /// </summary>
        /// <returns> null if operation failed, else # of encoder clusters, + means before limit, - means after limit </returns>
        public static object ClustersFromSafetyLimit()
        {
            if (GeminiHardware.QueueDepth > 1) return null;  // Don't queue up if queue is large

            string safety = GeminiHardware.DoCommandResult("<230:", GeminiHardware.MAX_TIMEOUT, false);
            string position = GeminiHardware.DoCommandResult("<235:", GeminiHardware.MAX_TIMEOUT, false);
            string size = GeminiHardware.DoCommandResult("<237:", GeminiHardware.MAX_TIMEOUT, false);
            if (safety == null || position == null || size == null) return null; //???

            string[] sp = safety.Split(new char[] { ';' });
            if (sp == null || sp.Length != 2) return null;

            int west_limit = 0;

            // west limit in clusters of 256 motor encoder ticks
            if (!int.TryParse(sp[1], out west_limit)) return null;


            sp = position.Split(new char[] { ';' });
            if (sp == null || sp.Length != 2) return null;
            int ra_clusters = 0;

            // current RA position in clusters of 256 motor encoder ticks
            if (!int.TryParse(sp[0], out ra_clusters)) return null;

            sp = size.Split(new char[] { ';' });
            if (sp == null || sp.Length != 2) return null;
            int size_clusters = 0;

            // size of 1/2 a cirlce (180 degrees) in RA in clusters of 256 motor encoder ticks
            if (!int.TryParse(sp[0], out size_clusters)) return null;

            double rate = SharedResources.EARTH_ANG_ROT_DEG_MIN / 60.0; //sidereal rate per second

            // sidereal tracking rate in clusters per second:
            rate = (double)size_clusters * (SharedResources.EARTH_ANG_ROT_DEG_MIN / 60.0) / 180.0;

            return ra_clusters - west_limit;
        }


        static void DoNudgeFromSafety()
        {
            if (AtSafetyLimit)
            {
                string[] cmds = { ":RS", ":Me"};                
                if (GeminiHardware.SideOfPier == "E") cmds[1] = ":Mw";

                GeminiHardware.DoCommand(cmds, false);
                System.Threading.Thread.Sleep(1000);    // move for 1 second at slew speed 
                GeminiHardware.DoCommandResult(":Q", GeminiHardware.MAX_TIMEOUT, false);

                GeminiHardware.WaitForVelocity("TN", 2000); // shouldn't too long to stop, right?
            }
        }

        /// <summary>
        /// Slews the mount using Ra and Dec
        /// </summary>
        public static void SlewEquatorial()
        {

            string[] cmd = { ":Sr" + m_Util.HoursToHMS(TargetRightAscension, ":", ":", ""), 
                             ":ON" + (TargetName??"PC Object"),
                             ":Sd" + m_Util.DegreesToDMS(TargetDeclination, ":", ":", ""), ":MS"};

            TargetName = null;

            m_SlewAborted.Reset();

            if (GeminiHardware.NudgeFromSafety && GeminiHardware.AtSafetyLimit)
                DoNudgeFromSafety();

            string [] result= null;

            DoCommandResult(cmd, MAX_TIMEOUT/2, false, out result);

            if (result == null || result[0] == null || result[2] == null || result[3] == null) throw new TimeoutException("SlewEquatorial");
            if (result[3].StartsWith("1")) throw new ASCOM.Utilities.Exceptions.InvalidValueException("Slew to object below horizon");
            if (result[3].StartsWith("4")) throw new ASCOM.Utilities.Exceptions.InvalidValueException("Position unreachable");
            if (result[3].StartsWith("5"))
            {
                if (OnError != null && m_Connected && m_AllowErrorNotify) OnError("Please 'Synchronize' the mount before using Goto", "Mount Is Not Aligned");
                throw new ASCOM.Utilities.Exceptions.InvalidValueException("Mount not aligned");
            }
            if (result[3].StartsWith("6")) throw new ASCOM.Utilities.Exceptions.InvalidValueException("Slew to outside of safety limits");
            if (result[0] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("Invalid RA coordinate");                
            if (result[2] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("Invalid DEC coordinates");
            m_Velocity = "S";   //set the correct velocity until next poll update
        }

        /// <summary>
        /// Slews the mount using Ra and Dec
        /// </summary>
        public static void SlewEquatorialAsync()
        {
            string[] cmd = { ":Sr" + m_Util.HoursToHMS(TargetRightAscension, ":", ":", ""), 
                             ":ON" + (TargetName??"PC Object"),
                             ":Sd" + m_Util.DegreesToDMS(TargetDeclination, ":", ":", ""), ":MS" };

            TargetName = null;

            m_SlewAborted.Reset();

            if (GeminiHardware.NudgeFromSafety && GeminiHardware.AtSafetyLimit)
                DoNudgeFromSafety();

            string[] result = null;


            DoCommandResult(cmd, MAX_TIMEOUT/2, false, out result);

            if (result == null || result[0] == null || result[2] == null || result[3] == null) throw new TimeoutException("SlewEquatorialAsync");
            if (result[3].StartsWith("1")) throw new ASCOM.Utilities.Exceptions.InvalidValueException("Slew to object below horizon");
            if (result[3].StartsWith("4")) throw new ASCOM.Utilities.Exceptions.InvalidValueException("Position unreachable");
            if (result[3].StartsWith("5"))
            {
                if (OnError != null && m_Connected && m_AllowErrorNotify) OnError("Please 'Synchronize' the mount before using Goto", "Mount Is Not Aligned");
                throw new ASCOM.Utilities.Exceptions.InvalidValueException("Mount not aligned");
            }
            if (result[3].StartsWith("6")) throw new ASCOM.Utilities.Exceptions.InvalidValueException("Slew to outside of safety limits");
            if (result[0] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("Invalid RA coordinate");
            if (result[2] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("Invalid DEC coordinate");

            m_Velocity = "S";   //set the correct velocity until next poll update
        }


        public static void SyncHorizonCoordinates(double azimuth, double altitude)
        {
            string[] cmd = { ":Sz" + m_Util.DegreesToDMS(azimuth, ":", ":", ""), 
                             ":ON" + (TargetName??"PC Object"),
                             ":Sa" + m_Util.DegreesToDMS(altitude, ":", ":", ""), "" };

            TargetName = null;


            if (m_AdditionalAlign)
            {
                cmd[3] = ":Cm";
            }
            else
            {
                cmd[3] = ":CM";
            }
            string[] result = null;
            DoCommandResult(cmd, MAX_TIMEOUT / 2, false, out result);
            if (result == null || result[0] == null || result[2] == null || result[3] == null)
                throw new TimeoutException((m_AdditionalAlign ? "Align to" : "Sync to ") + "Alt/Az");
            if (result[0] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("Alt value is invalid");
            if (result[2] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("Az value is invalid");
            if (result[3] == "No object!") throw new ASCOM.Utilities.Exceptions.InvalidValueException((m_AdditionalAlign ? "Align to" : "Sync to ") + "Alt/Az");

            m_Azimuth = azimuth; // Update state machine variables
            m_Altitude = altitude;

            m_Velocity = "S";   //set the correct velocity until next poll update

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
            string[] cmd = { ":Sz" + m_Util.DegreesToDMS(TargetAzimuth, ":", ":", ""), 
                             ":ON" + (TargetName??"PC Object"),
                             ":Sa" + m_Util.DegreesToDMS(TargetAltitude, ":", ":", ""), ":MA" };

            TargetName = null;

            m_SlewAborted.Reset();

            if (GeminiHardware.NudgeFromSafety && GeminiHardware.AtSafetyLimit)
                DoNudgeFromSafety();

            string[] result = null;

            DoCommandResult(cmd, MAX_TIMEOUT / 2, false, out result);
            if (result == null || result[0] == null || result[2] == null || result[3] == null)
                throw new TimeoutException("Slew to Alt/Az");
            if (result[0] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("Alt value is invalid");
            if (result[2] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("Az value is invalid");
            if (result[3].StartsWith("1")) throw new ASCOM.Utilities.Exceptions.InvalidValueException("Slew to Alt/Az: Object below horizon");
            if (result[3].StartsWith("2")) throw new ASCOM.Utilities.Exceptions.InvalidValueException("Slew to Alt/Az: No object selected");
            if (result[3].StartsWith("3")) throw new ASCOM.Utilities.Exceptions.InvalidValueException("Slew to Alt/Az: Manual control");
            m_Velocity = "S";   //set the correct velocity until next poll update

        }

        /// <summary>
        /// Slews the mount using Alt and Az
        /// </summary>
        public static void SlewHorizonAsync()
        {
            string[] cmd = { ":Sz" + m_Util.DegreesToDMS(TargetAzimuth, ":", ":", ""), 
                             ":ON" + (TargetName??"PC Object"),
                             ":Sa" + m_Util.DegreesToDMS(TargetAltitude, ":", ":", ""), ":MA" };

            TargetName = null;

            m_SlewAborted.Reset();

            if (GeminiHardware.NudgeFromSafety && GeminiHardware.AtSafetyLimit)
                DoNudgeFromSafety();


            string[] result = null;
            DoCommandResult(cmd, MAX_TIMEOUT/2, false, out result);

            if (result==null || result[0] == null || result[2] == null || result[3] == null)
                throw new TimeoutException("Slew to Alt/Az");
            if (result[0] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("Alt value is invalid");
            if (result[2] != "1") throw new ASCOM.Utilities.Exceptions.InvalidValueException("Az value is invalid");
            if (result[3].StartsWith("1")) throw new ASCOM.Utilities.Exceptions.InvalidValueException("Slew to Alt/Az: Object below horizon");
            if (result[3].StartsWith("2")) throw new ASCOM.Utilities.Exceptions.InvalidValueException("Slew to Alt/Az: No object selected");
            if (result[3].StartsWith("3")) throw new ASCOM.Utilities.Exceptions.InvalidValueException("Slew to Alt/Az: Manual control");

            m_Velocity = "S";   //set the correct velocity until next poll update
        }

        /// <summary>
        /// Set/Get PEC Status byte from Gemini
        ///  PEC status. Decimal
        ///     1: PEC active,
        ///     2: freshly trained (not yet altered) PEC data are available as current PEC data,
        ///     4: PEC training in progress,
        ///     8: PEC training was just completed,
        ///     16: PEC training will start soon,
        ///   0xff: failed to get status
        /// </summary>
        public static byte PECStatus
        {
            get
            {
                string s = DoCommandResult("<509:", 2000, false);
                byte res = 0;
                if (!byte.TryParse(s, out res)) return 0xff;   
                return res;
            }
            set
            {
                DoCommand(">509:" + value.ToString(), false);            
            }
        }

        #endregion

        #region Focuser Implementation

        public static bool AbsoluteFocuser
        {
            get { return m_AbsoluteFocuser; }
            set
            {
                m_AbsoluteFocuser = value;
                Profile.DeviceType = "Focuser";
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "AbsoluteFocuser", value.ToString(GeminiHardware.m_GeminiCulture));
            }
        }


        /// <summary>
        /// Focuser Reverse Directions
        /// </summary>
        public static bool ReverseDirection
        { 
            get { return m_ReverseDirection; }
            set
            {
                Profile.DeviceType = "Focuser";
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "ReverseDirection", value.ToString());
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
                Profile.DeviceType = "Focuser";
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "StepSize", value.ToString());
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
                Profile.DeviceType = "Focuser";
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "BrakeSize", value.ToString());
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
                Profile.DeviceType = "Focuser";
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "Speed", value.ToString());
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
                Profile.DeviceType = "Focuser";
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "MaxIncrement", value.ToString());
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
                Profile.DeviceType = "Focuser";
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "MaxStep", value.ToString());
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
                Profile.DeviceType = "Focuser";
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "BacklashDirection", value.ToString());
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
                Profile.DeviceType = "Focuser";
                Profile.WriteValue(SharedResources.FOCUSER_PROGRAM_ID, "BacklashSize", value.ToString());
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
            // resolve some common mistakes in specifying Gemini commands:
            if (p[0]!=':') p = ":"+p;
            if (!p.EndsWith("#")) p = p + "#";
            return p; // standard commands end in '#' character, no checksum needed
        }

        /// <summary>
        /// Complete native Gemini command. Involves appending a checksum and a '#' at the end.
        /// </summary>
        /// <param name="p">command to complete</param>
        /// <returns>completed command to send to the mount</returns>
        private static string CompleteNativeCommand(string p)
        {
            // resolve some common mistakes in specifying Gemini commands:
            if (!p.Contains(":")) p = p + ":";  // no argument and not ':' terminated??
            return p + ComputeChecksum(p) + "#";
        }

        /// <summary>
        /// Gemini Native command checksum
        /// </summary>
        /// <param name="p">command to compute checksum for</param>
        /// <returns>Gemini checksum character</returns>
        private static char ComputeChecksum(string p)
        {
            byte chksum = 0;

            for (int i = 0; i < p.Length; ++i)
                chksum ^= (byte)p[i];

            return (char)((chksum & 0x7f) + 0x40);
        }

       
        /// <summary>
        /// Add command item 'ci' to the queue for execution
        /// </summary>
        /// <param name="ci">actual command to queue</param>
        private static bool QueueCommand(CommandItem ci)
        {
#if DEBUG
            System.Diagnostics.Trace.WriteLine("Queue command..."+ci.m_Command);
#endif
            lock (m_CommandQueue)
            {
                if (!m_Connected) return false;
                m_CommandQueue.Enqueue(ci);
            }
            m_WaitForCommand.Set();     //signal to the background worker that commands are queued up
            return true;
        }

        /// <summary>
        /// Add all the command items in 'ci' to the queue for execution
        /// </summary>
        /// <param name="ci">array of commands to be executed in sequence</param>
        private static bool QueueCommands(CommandItem[] ci)
        {
#if DEBUG
            System.Diagnostics.Trace.Write("Queue commands...");
            foreach(CommandItem c in ci) System.Diagnostics.Trace.Write(c.m_Command + ", ");
            System.Diagnostics.Trace.WriteLine("");
#endif
            lock (m_CommandQueue)
            {
                if (!m_Connected) return false;
                for(int i=0; i<ci.Length; ++i)
                    m_CommandQueue.Enqueue(ci[i]);
            }
            m_WaitForCommand.Set();     //signal to the background worker that commands are queued up
            return true;
        }

        #endregion


    }
}
