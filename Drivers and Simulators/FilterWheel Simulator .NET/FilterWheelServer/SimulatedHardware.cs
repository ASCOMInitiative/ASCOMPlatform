using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace ASCOM.FilterWheelSim
{
    // 
    // Implements the simulated hardware
    //
    // There should not be any throwing of exceptions in here as that is the responsibility of the driver
    // The hardware is at the end of a bit of wire, we just simulate what a likely eletro-mechanical device
    // would do.
    //
    class SimulatedHardware
    {

#region variables

        // State variables
        private static bool m_bMoving;          // FilterWheel in motion?

        // Persistent settings
        private static short m_sPosition;       // Current filter position
        private static short m_sTargetPosition; // Filter position we want to get to
        private static int m_iSlots;            // Number of filter wheel positions
        private static int m_iTimeInterval;     // Time to move between filter positions (miilisecs)

        // Timer object for the moves
        private static Timer m_Timer = new Timer();

#endregion

        //
        // Constructor - initialize state
        //
        public SimulatedHardware()
        {
            m_bMoving = false;
            m_Timer.Enabled = false;
            m_Timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Tick);
        }

#region Properties and Methods

        public short Position
        {
            get
            {
                if (m_bMoving)   // Spec. says we must return -1 if position not determined
                    return -1;
                else
                    return m_sPosition;
            }
            set
            {
                int Jumps;      // number of slot positions we have to move

                m_sTargetPosition = value;

                // position range check
                if (m_sTargetPosition > m_iSlots - 1 || m_sTargetPosition < 0)
                    return;     // do nothing - it isn't a valid request

                // check if we are already there!
                if (m_sTargetPosition == m_sPosition)
                    return;

                // do the move

                // Find the shortest distance between two filter positions
                Jumps = Math.Min(Math.Abs(m_sPosition - m_sTargetPosition), m_iSlots - Math.Abs(m_sPosition - m_sTargetPosition));
                m_Timer.Interval = Jumps * m_iTimeInterval;
                m_Timer.Enabled = true;

                // trigger the "motor"
                m_bMoving = true;
            }
        }

        public int Slots { get {return m_iSlots;} }

        public bool Moving { get {return m_bMoving;} }

        public int Interval { get { return m_iTimeInterval; } }

        public void ReadConfig() { UpdateSettings(); }

        public void AbortMove()
        {
            // Stop the motor!
            m_Timer.Enabled = false;
            // Set the postion intermediate between start and end
            m_sPosition = (short)Math.Floor(Math.Abs(m_sTargetPosition - m_sPosition) / 2.0);
            m_bMoving = false;
        }

#endregion


#region Event Handlers
        //
        // Called when the move timer fires event
        //
        // Implements moving delay.
        //
        private void Timer_Tick(Object sender, System.EventArgs e)
        {
            // disable the timer now it has fired
            m_Timer.Enabled = false;

            //
            // move complete, update everything
            //
            if (m_bMoving)
                m_bMoving = false;

            m_sPosition = m_sTargetPosition;

            // no feedback to the driver when this is done
            // the driver will have to poll us to find out when we are there
        }

#endregion


#region private utilities
    
        //
        // Get Settings from Registry
        //
        private void UpdateSettings()
        {
            // get some basic info about the hardware
            m_iSlots = Convert.ToInt32(GetSetting("Slots", "8"));
            if (m_iSlots < 1 || m_iSlots > 8) m_iSlots = 4;
            m_sPosition = Convert.ToInt16(GetSetting("Position", "0"));
            if (m_sPosition < 0 || m_sPosition >= m_iSlots) m_sPosition = 0;
            m_iTimeInterval = Convert.ToInt32(GetSetting("Time", "1000"));
        }

        //
        // Settings support
        //
        private string GetSetting(string Name, string DefValue)
        {
            string s = frmHandbox.g_Profile.GetValue(frmHandbox.g_csDriverID, Name, "");
            if (s == "") s = DefValue;
            return s;
        }

#endregion

    }
}
