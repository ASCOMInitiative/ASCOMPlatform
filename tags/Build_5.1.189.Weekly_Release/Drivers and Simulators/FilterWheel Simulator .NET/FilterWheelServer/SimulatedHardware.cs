using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;

namespace ASCOM.FilterWheelSim
{
    // 
    // Implements the simulated hardware
    //
    public class SimulatedHardware
    {

#region variables

        // Timer variables
        private static int m_iTimeInterval;             // Time to move between filter positions (miilisecs)
        private static int m_iTimeToMove;               // Time required to complete the current Move
        private static int m_iTimeElapsed;              // Keeps track of the elapsed time of the current Move
        private static int m_iTimerTickInterval = 100;  // How often we pump the hardware

        public static bool m_bConnected;                // Tracked connected state, public so the handbox can test without
                                                        // invoking the full method
        public static short m_sPosition;                // Current filter position, public so the handbox can test without
                                                        // invoking the full method
        private static short m_sTargetPosition;         // Filter position we want to get to
        private static bool m_bMoving;                  // FilterWheel in motion?
        private static int m_iSlots;                    // Number of filter wheel positions
        private static string[] m_asFilterNames;        // Array of filter name strings
        private static int[] m_aiFocusOffsets;          // Array of focus offsets
        private static Color[] m_acFilterColours;       // Array of filter colours
        private static bool m_bImplementsNames;         // Return filter names?
        private static bool m_bImplementsOffsets;       // Return Offsets?
        private static bool m_bPreemptMoves;            // Driver can interupt moves

        private const string m_sRegVer = "1";             // Used to track id registry entries exist or need updating

        public static bool m_bLogTraffic;                 // Do we log traffic?

        //
        // Sync object
        //
        private static object s_objSync = new object();	// Better than lock(this) - Jeffrey Richter, MSDN Jan 2003

        public const string g_csDriverID = "ASCOM.FilterWheelSim.FilterWheel";
        private const string g_csDriverDescription = "FilterWheelSimulator FilterWheel";
        public static HelperNET.Profile g_Profile;


        // Exception codes/messages
        public const string ERR_SOURCE = "ASCOM FilterWheel Simulator";

        public static int SCODE_NOT_IMPLEMENTED = ErrorCodes.DriverBase + 0x400;
        public const string MSG_NOT_IMPLEMENTED = " is not implemented by this filter wheel driver.";

        public static int SCODE_DLL_LOADFAIL = ErrorCodes.DriverBase + 0x401;
        // Error message for above generated at run time

        public static int SCODE_NOT_CONNECTED = ErrorCodes.DriverBase + 0x402;
        public const string MSG_NOT_CONNECTED = "The filter wheel is not connected";

        public static int SCODE_VAL_OUTOFRANGE = ErrorCodes.DriverBase + 0x404;
        public const string MSG_VAL_OUTOFRANGE = "The value is out of range";

        public static int SCODE_MOVING = ErrorCodes.DriverBase + 0x405;
        public const string MSG_MOVING = "The filterwheel is already moving";

        private static int SCODE_SETUP_NOT_ALLOWED = ErrorCodes.DriverBase + 0x406;
        private const string MSG_SETUP_NOT_ALLOWED = "Setup not allowed whilst connected";

#endregion

        //
        // Constructor - initialize state
        //
        static SimulatedHardware()
        {
            m_bMoving = false;

            m_asFilterNames = new string[8];
            m_aiFocusOffsets = new int[8];
            m_acFilterColours = new Color[8];
            g_Profile = new HelperNET.Profile();
            g_Profile.DeviceType = "FilterWheel";     // We're a filter wheel driver

        }

#region Properties and Methods

        //
        // Initialize/finalize for server startup/shutdown
        //
        public static void Initialize()
        {
            UpdateSettings();
        }

        public static void Finalize_()	// "Finalize" exists in parent
        {
            //Profile.WriteValue(s_sProgID, "Position", s_fPosition.ToString(), "");
            //Profile.WriteValue(s_sProgID, "RotationRate", RotationRate.ToString(), "");
            //Profile.WriteValue(s_sProgID, "CanReverse", s_bCanReverse.ToString(), "");
            //Profile.WriteValue(s_sProgID, "Reverse", s_bReverse.ToString(), "");
        }

        public static bool Connected
        {
            get
            {
                lock (s_objSync)
                {
                    LogTraffic("Get Connected = " + m_bConnected);
                    return m_bConnected;
                }
            }
            set
            {
                lock (s_objSync)
                {
                    LogTraffic("Set Connected = " + m_bConnected + " -> " + value);

                    // We are always connected to the hardware in the simulator
                    // So just keep a record of the state change
                    m_bConnected = value;

                    LogTraffic("  (set connected, done)");
                }
            }
        }

        public static short Position
        {
            get
            {
                lock (s_objSync)
                {
                    short ret;

                    CheckConnected();

                    if (m_bMoving)
                        ret = -1;                       // Spec. says we must return -1 if position not determined
                    else
                        ret = m_sPosition;      // Otherwise return position

                    LogTraffic("Get Position = " + ret.ToString());

                    return ret;
                }
            }
            set
            {
                lock (s_objSync)
                {
                    int Jumps;      // number of slot positions we have to move

                    LogTraffic("Set Position = " + value + " ...");

                    // position range check
                    if (value >= m_iSlots || value < 0)
                    {
                        LogTraffic("  (set postion failed, out of range)");

                        throw new DriverException("Position: " + MSG_VAL_OUTOFRANGE, SCODE_VAL_OUTOFRANGE);
                    }

                    m_sTargetPosition = value;

                    // check if we are already there!
                    if (value == m_sPosition)
                    {
                        LogTraffic("  (set position, no move required)");
                        return;
                    }

                    // check if we are already moving
                    if (m_bMoving)
                    {
                        if (m_bPreemptMoves)
                            AbortMove();   // Stop the motor
                        else
                        {
                            LogTraffic("  (set position failed, already moving)");

                            throw new DriverException("Position: " + MSG_MOVING, SCODE_MOVING);
                        }
                    }

                    // Find the shortest distance between two filter positions
                    Jumps = Math.Min(Math.Abs(m_sPosition - m_sTargetPosition), m_iSlots - Math.Abs(m_sPosition - m_sTargetPosition));
                    m_iTimeToMove = Jumps * m_iTimeInterval;

                    // trigger the "motor"
                    m_bMoving = true;

                    // log action
                    LogTraffic(" (set position in progress)...");
                }
            }
        }

        public static string[] FilterNames
        {
            get
            {
                LogTraffic("Get FilterNames...");
                string[] temp = m_asFilterNames;
                Array.Resize(ref temp, m_iSlots);
                if (m_bLogTraffic)
                    for (int i = 0; i < m_iSlots; i++)
                        LogTraffic(" Filter " + i.ToString() + " = " + temp[i].ToString());
                LogTraffic("  (filternames done)");
                return temp;
            }
        }

        public static int[] FocusOffsets
        {
            get
            {
                LogTraffic("Get FocusOffsets..." + Environment.NewLine);
                int[] temp = m_aiFocusOffsets;
                Array.Resize(ref temp, m_iSlots);
                if (m_bLogTraffic)
                    for (int i = 0; i < m_iSlots; i++)
                        LogTraffic(" Filter " + i.ToString() + " = " + temp[i].ToString() + Environment.NewLine);
                LogTraffic(" (focusoffsets done)" + Environment.NewLine);
                return temp;
            }
        }

        // For the setup dialog
        public static string[] FullFilterNames { get { return m_asFilterNames; } }

        // For the setup dialog
        public static int[] FullFocusOffsets { get { return m_aiFocusOffsets; } }

        // For the setup dialog
        public static Color[] FullFilterColours { get { return m_acFilterColours; } }

        // For the setup dialog
        public static bool ImplementsNames { get { return m_bImplementsNames; } }

        // For the setup dialog
        public static bool ImplementsOffsets { get { return m_bImplementsOffsets; } }

        // For the setup dialog
        public static bool PreemptMoves { get { return m_bPreemptMoves; } }

        // For the Handbox
        public static int Slots { get { return m_iSlots; } }

        // For the Handbox
        public static bool Moving { get { return m_bMoving; } }
        
        // For the Handbox
        public static int Interval { get { return m_iTimeInterval; } }

        // For the Handbox
        public static string CurrFilterName { get { return m_asFilterNames[m_sPosition]; } }

        // For the Handbox
        public static int CurrFilterOffset { get { return m_aiFocusOffsets[m_sPosition]; } }

        // For the Handbox
        public static Color CurrFilterColour { get { return m_acFilterColours[m_sPosition]; } }

         // Sends the Pump interval from the Handbox
        public static int TimerTickInverval { set { m_iTimerTickInterval = value; } }

        public static void AbortMove()
        {
            LogTraffic("Abort move");
            // Clear the elapsed time
            m_iTimeElapsed = 0;
            // Stop moving
            m_bMoving = false;
            // Set the postion intermediate between start and end
            m_sPosition = (short)Math.Floor(Math.Abs(m_sTargetPosition - m_sPosition) / 2.0);
            LogTraffic("  (abort done)");
        }

        public static void UpdateState()
        {
            lock (s_objSync)
            {
                if (m_bMoving)
                {
                    // We are moving so increment the elapsed move time
                    m_iTimeElapsed += m_iTimerTickInterval;

                    // Have we reached the filter position yet?
                    if (m_iTimeElapsed >= m_iTimeToMove)
                    {
                        // Clear the elapsed time
                        m_iTimeElapsed = 0;
                        // Stop moving
                        m_bMoving = false;
                        // Set the new position
                        m_sPosition = m_sTargetPosition;
                        // log action
                        LogTraffic("  (position done)");
                    }
                }
            }
        }

        public static void DoSetup()
        {
            //lock (s_objSync)
            {
                LogTraffic("SetupDialog...");

                if (m_bConnected)
                    throw new ASCOM.DriverException(MSG_SETUP_NOT_ALLOWED, SCODE_SETUP_NOT_ALLOWED);
                else
                    ShowSetup();
            }
        }


#endregion


#region private utilities
    
        //
        // Get Settings from Registry
        //
        private static void UpdateSettings()
        {
            int i;

            if (g_Profile.GetValue(g_csDriverID, "RegVer") != m_sRegVer)
            {
                //
                // initialize variables that are not present
                //
                // Create some 'realistic' defaults
                //
                Color[] colours = new Color[8] {Color.Red, Color.Green, Color.Blue, Color.Gray,
                                                Color.DarkRed, Color.Teal, Color.Violet, Color.Black};
                string[] names = new string[8] { "Red", "Green", "Blue", "Clear", "Ha", "OIII", "LPR", "Dark" };
                Random rand = new Random();

                g_Profile.WriteValue(g_csDriverID, "RegVer", m_sRegVer);
                g_Profile.WriteValue(g_csDriverID, "Position", "0");
                g_Profile.WriteValue(g_csDriverID, "Slots", "4");
                g_Profile.WriteValue(g_csDriverID, "Time", "1000");
                g_Profile.WriteValue(g_csDriverID, "ImplementsNames", "true");
                g_Profile.WriteValue(g_csDriverID, "ImplementsOffsets", "true");
                g_Profile.WriteValue(g_csDriverID, "PreemptMoves", "false");
                for (i = 0; i < 8; i++)
                {
                    g_Profile.WriteValue(g_csDriverID, i.ToString(), names[i], "FilterNames");
                    g_Profile.WriteValue(g_csDriverID, i.ToString(), rand.Next(10000).ToString(), "FocusOffsets");
                    g_Profile.WriteValue(g_csDriverID, i.ToString(), ColorTranslator.ToWin32(colours[i]).ToString(), "FilterColours");
                }
            }

            // Read the hardware & driver config
            m_iSlots = Convert.ToInt32(GetSetting("Slots", "8"));
            if (m_iSlots < 1 || m_iSlots > 8) m_iSlots = 4;
            m_sPosition = Convert.ToInt16(GetSetting("Position", "0"));
            if (m_sPosition < 0 || m_sPosition >= m_iSlots) m_sPosition = 0;
            m_iTimeInterval = Convert.ToInt32(GetSetting("Time", "1000"));
            m_bImplementsNames = Convert.ToBoolean(GetSetting("ImplementsNames", "true"));
            m_bImplementsOffsets = Convert.ToBoolean(GetSetting("ImplementsOffsets", "true"));
            m_bPreemptMoves = Convert.ToBoolean(GetSetting("PreemptMoves", "false"));
            for (i = 0; i <= 7; i++)
            {
                m_asFilterNames[i] = g_Profile.GetValue(g_csDriverID, i.ToString(), "FilterNames");
                m_aiFocusOffsets[i] = Convert.ToInt32(g_Profile.GetValue(g_csDriverID, i.ToString(), "FocusOffsets"));
                m_acFilterColours[i] = ColorTranslator.FromWin32(Convert.ToInt32(g_Profile.GetValue(g_csDriverID, i.ToString(), "FilterColours")));
            }

        }

        //
        // Settings support
        //
        private static string GetSetting(string Name, string DefValue)
        {
            string s = g_Profile.GetValue(g_csDriverID, Name, "");
            if (s == "") s = DefValue;
            return s;
        }

        private static void CheckConnected()
        {
            if (!m_bConnected) throw new ASCOM.DriverException(MSG_NOT_CONNECTED, SCODE_NOT_CONNECTED);
        }


        private static void ShowSetup()
        {
            frmSetupDialog SetupDialog = new frmSetupDialog();

            SetupDialog.Slots = SimulatedHardware.Slots;
            SetupDialog.Time = SimulatedHardware.Interval;
            SetupDialog.Names = SimulatedHardware.FullFilterNames;
            SetupDialog.Offsets = SimulatedHardware.FullFocusOffsets;
            SetupDialog.Colours = SimulatedHardware.FullFilterColours;
            SetupDialog.ImplementsNames = SimulatedHardware.ImplementsNames;
            SetupDialog.ImplementsOffsets = SimulatedHardware.ImplementsOffsets;
            SetupDialog.PreemptsMoves = SimulatedHardware.PreemptMoves;

            SetupDialog.TopMost = true;   // The ASCOM chooser dialog sits on top if we don't do this :(

            if (SetupDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Update the hardware config
                UpdateSettings();
                LogTraffic("  (setup OK)");
            }
            else
                LogTraffic("  (setup Cancel)");

        }
            
        private static void LogTraffic(string Text)
        {
            if (m_bLogTraffic)
                Trace.WriteLine(Text);
        }

#endregion

    }
}
