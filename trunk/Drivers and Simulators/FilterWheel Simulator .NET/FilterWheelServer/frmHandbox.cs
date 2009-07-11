using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.FilterWheelSim
{
    public partial class frmHandbox : Form
    {
        delegate void SetTextCallback(string text);

#region variables

        private static bool m_bConnected;               // Tracked connected state
        private static short m_sPosition;               // Current filter position
        private static bool m_bMoving;                  // FilterWheel in motion?
        private static bool m_bMoveNext = true;         // Next or Prev button?
        private static int m_iSlots;                    // Number of filter wheel positions
        private static string[] m_asFilterNames;        // Array of filter name strings
        private static int[] m_aiFocusOffsets;          // Array of focus offsets
        private static Color[] m_acFilterColours;       // Array of filter colours
        private static bool m_bImplementsNames;         // Return filter names?
        private static bool m_bImplementsOffsets;       // Return Offsets?
        //private static int m_iTimeInterval;             // Time to move between filters
        private static frmTraffic m_trafficDialog = null;   // API traffic display form
        private static SimulatedHardware m_Hardware;        // Simulated wire
        private const string m_sRegVer = "1";                // Used to track id registry entries exist or need updating
      
        public const string g_csDriverID = "ASCOM.FilterWheelSim.FilterWheel";
        public const string g_csDriverDescription = "FilterWheelSimulator FilterWheel";
        public static HelperNET.Profile g_Profile;

        public const string ERR_SOURCE  = "ASCOM FilterWheel Simulator";

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


#endregion

        public frmHandbox()
        {
            InitializeComponent();
            
            m_asFilterNames = new string[8];
            m_aiFocusOffsets = new int[8];
            m_acFilterColours = new Color[8];
            g_Profile = new HelperNET.Profile();
            g_Profile.DeviceType = "FilterWheel";     // We're a filter wheel driver
            m_Hardware = new SimulatedHardware();
            ToolTip aTooltip = new ToolTip();
            aTooltip.SetToolTip(picASCOM, "Visit the ASCOM website");
            aTooltip.SetToolTip(btnTraffic, "Monitor ASCOM API traffic");
            aTooltip.SetToolTip(btnConnect, "Connect/Disconnect filterwheel");
            aTooltip.SetToolTip(btnPrev, "Move position to previous filter");
            aTooltip.SetToolTip(btnNext, "Move position to next filter");
            
            m_Hardware.ReadConfig();
            UpdateConfig();
            UpdateLabels();
        }


#region Properties and Methods

        public bool Connected
        {
            get
            {
                if (m_trafficDialog != null && m_trafficDialog.chkOther.Checked)
                    m_trafficDialog.TrafficLine("Get Connected = " + m_bConnected);
                return m_bConnected;
            }
            set
            {
                if (m_trafficDialog != null && m_trafficDialog.chkOther.Checked)
                    m_trafficDialog.TrafficStart("Set Connected = " + m_bConnected + " -> " + value);
                
                // We are always connected to the hardware in the simulator
                // So just keep a record of the state change
                m_bConnected = value;

                if (m_trafficDialog != null && m_trafficDialog.chkOther.Checked)
                    m_trafficDialog.TrafficEnd(" (done)");

                UpdateLabels();
            }
        }

        public short Position
        {
            get
            {
                short ret;
                if (m_trafficDialog != null && m_trafficDialog.chkPosition.Checked)
                    m_trafficDialog.TrafficStart("Get Position = ");

                if (m_bMoving)  
                    ret = -1;                       // Spec. says we must return -1 if position not determined
                else
                    ret = m_sPosition;      // Otherwise return position

                if (m_trafficDialog != null && m_trafficDialog.chkPosition.Checked)
                    m_trafficDialog.TrafficEnd(Convert.ToString(ret));

                return ret;
            }
            set
            {
                if (m_trafficDialog != null && m_trafficDialog.chkPosition.Checked)
                    m_trafficDialog.TrafficStart("Set Position = " + value + " ...");

                // position range check
                if (value >= m_iSlots || value < 0)
                {
                    if (m_trafficDialog != null && m_trafficDialog.chkPosition.Checked)
                        m_trafficDialog.TrafficEnd(" (aborting, range)");

                    throw new DriverException("Position: " + MSG_VAL_OUTOFRANGE, SCODE_VAL_OUTOFRANGE);
                }

                // check if we are already there!
                if (value == m_sPosition)
                {
                    if (m_trafficDialog != null && m_trafficDialog.chkPosition.Checked)
                        m_trafficDialog.TrafficEnd(" (no move required)");
                    return;
                }

                // check if we are already moving
                if (m_bMoving)
                {
                    if (m_trafficDialog != null && m_trafficDialog.chkPosition.Checked)
                        m_trafficDialog.TrafficEnd(" (aborting, moving)");
                    try
                    {
                        throw new DriverException("Position: " + MSG_MOVING, SCODE_MOVING);
                    }
                    catch { } // We don't want to abort the Handbox if it was started as an EXE
                }

                // trigger the "motor"
                m_Hardware.Position = value;
                TimerMove.Enabled = true;        // Start the Timer so we keep checking for move complete

                // update the handbox
                this.Moving = true;

                // log action
                if (m_trafficDialog != null && m_trafficDialog.chkPosition.Checked)
                    m_trafficDialog.TrafficEnd(" (started)");
            }
        }

        public void DoSetup()
        {
            frmSetupDialog SetupDialog = new frmSetupDialog();

            // Do we need to log this?
            if (m_trafficDialog != null && m_trafficDialog.chkOther.Checked)
                m_trafficDialog.TrafficStart("SetupDialog");

            SetupDialog.Slots = m_iSlots;
            SetupDialog.Time = m_Hardware.Interval;
            SetupDialog.Names = m_asFilterNames;
            SetupDialog.Offsets = m_aiFocusOffsets;
            SetupDialog.Colours = m_acFilterColours;
            SetupDialog.ImplementsNames = m_bImplementsNames;
            SetupDialog.ImplementsOffsets = m_bImplementsOffsets;


            this.Visible = false;         // May float over setup
            SetupDialog.TopMost = true;   // The ASCOM chooser dialog sits on top if we don't do this :(

            if (SetupDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Update the hardware config
                m_Hardware.ReadConfig();
                // Read new values from registry
                UpdateConfig();
                // Fix up position if required
                if (m_sPosition >= m_iSlots) m_sPosition = 0;  // User reduced the number of slots?
                // Update Handbox
                UpdateLabels();
            }

            this.Visible = true;
            this.BringToFront();

            SetupDialog = null;

            // Do we need to log this?
            if (m_trafficDialog != null && m_trafficDialog.chkOther.Checked)
                m_trafficDialog.TrafficEnd(" (done)");

        }

        public int Slots { get { return m_iSlots; } }

        public string[] FilterNames
        {
            get
            {
                string[] temp;

                if (m_trafficDialog != null && m_trafficDialog.chkName.Checked)
                        m_trafficDialog.TrafficLine("Names = ");

                temp = m_asFilterNames;
                Array.Resize(ref temp, m_iSlots);

                for (int i=0; i<m_iSlots; i++)
                {
                    if (!m_bImplementsNames)
                        temp[i] = "Filter " + (i + 1);         // Spec. says we return "Filter 1" etc if names not supported

                    if (m_trafficDialog != null && m_trafficDialog.chkName.Checked)
                        m_trafficDialog.TrafficLine("  Filter(" + i + ") = " + temp[i]);
                }
                return temp;
            }
            set
            {
                m_asFilterNames = value;
                UpdateLabels();
            }
        }

        public int[] FocusOffsets
        {
            get
            {
                int[] temp;

                if (m_trafficDialog != null && m_trafficDialog.chkName.Checked)
                    m_trafficDialog.TrafficLine("FocusOffsets = ");

                if (!m_bImplementsOffsets)
                    throw new PropertyNotImplementedException("FocusOffsets", false);
                else
                {
                    temp = m_aiFocusOffsets;
                    // Only return an array with the values used
                    Array.Resize(ref temp, m_iSlots);
                    return temp;
                }
            }
            set
            {
                m_aiFocusOffsets = value;
                UpdateLabels();
            }
        }

        public Color[] FilterColour
        {
            get
            {
                Color[] temp = m_acFilterColours;
                Array.Resize(ref temp, m_iSlots);
                return temp;
            }
            set
            {
                m_acFilterColours = value;
                UpdateLabels();
            }
        }

        public bool Moving
        {
            get { return m_bMoving; }

            set
            {
                m_bMoving = value;
                if (m_bMoving)
                {
                    
                    if (m_bMoveNext)
                        picFilter.Image = Properties.Resources.Filter_Next;
                    else
                        picFilter.Image = Properties.Resources.Filter_Prev;
                }
                else
                    picFilter.Image = Properties.Resources.FilterStop;
                
                UpdateLabels();
            }
        }


#endregion

#region EventHandlers

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (!m_bMoving && m_bConnected) PrevNext(false);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!m_bMoving && m_bConnected) PrevNext(true);
        }

        private void picASCOM_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void btnTraffic_Click(object sender, EventArgs e)
        {
            if (m_trafficDialog == null)
                m_trafficDialog = new frmTraffic();

            m_trafficDialog.Show();

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            m_bConnected = !m_bConnected;

            if (m_trafficDialog != null && m_trafficDialog.chkOther.Checked)
                m_trafficDialog.TrafficLine("Connected = " + m_bConnected);

            if (m_bMoving && !m_bConnected)
            { // If still moving when we disconnect...
                // Stop polling the hardware - we're disconnected!
                TimerMove.Enabled = false;
                // Assume no movement
                m_bMoving = false;
            }

            if (m_bConnected)
            { // OK, we've connected, check the status of the hardware
                m_bMoving = m_Hardware.Moving;
                m_sPosition = m_Hardware.Position;

                // If the hardware is moving, check when it stops
                if (m_bMoving) TimerMove.Enabled = true;
            }

            UpdateLabels();
 
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            this.DoSetup();
        }

        private void TimerMove_Tick(object sender, EventArgs e)
        {
            // Check if the filter wheel has stopped moving yet
            if (!m_Hardware.Moving)
            {
                // Yes it has so stop the timer
                TimerMove.Enabled = false;
                // Get the new position
                m_sPosition = m_Hardware.Position;
                // Update the form
                this.Moving = false;
                UpdateLabels();
                // Log this?
                if (m_trafficDialog != null && m_trafficDialog.chkMoving.Checked)
                    m_trafficDialog.TrafficEnd(" (done)");

            }
            // If still moving wait for the next timer tick
        }

        private void frmHandbox_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_bMoving = false;
            m_bConnected = false;

            // g_Profile.WriteValue(g_csDriverID, "Position", Convert.ToString(m_sPosition));
            try
            {
                // Close the traffic dialog if it is open
                if (m_trafficDialog != null)
                {
                    m_trafficDialog.Close();
                    m_trafficDialog = null;
                }
                // Disconnect the hardware
                m_Hardware = null;
            }
            catch { }

            //this.Visible = true;
            //this.WindowState = FormWindowState.Normal;
            //g_Profile.WriteValue(g_csDriverID, "Left", Convert.ToString(this.Left));
            //g_Profile.WriteValue(g_csDriverID, "Top", Convert.ToString(this.Top));

        }

#endregion

#region Private Functions

        private void PrevNext(bool nxt)
        {
            short newPosition = m_sPosition;
            if (nxt)
            {
                newPosition++;
                m_bMoveNext = true;
            }
            else
            {
                newPosition--;
                m_bMoveNext = false;
            }

            // make sure position stays in range
            if (newPosition >= m_iSlots)
                newPosition = 0;
            else if (newPosition < 0)
                newPosition = (short)(m_iSlots - 1);

            this.Position = newPosition;
        }

        private void UpdateLabels()
        {
            if (m_Hardware.Moving)
            {
                lblPosition.Text = "moving";
                lblName.Text = "";
                lblOffset.Text = "";
                picFilter.BackColor = Color.DimGray;
            }
            else if (m_bConnected)
            {
                lblConnect.Text = "Connected";
                lblPosition.Text = m_sPosition.ToString();
                lblName.Text = m_asFilterNames[m_sPosition];
                lblOffset.Text = m_aiFocusOffsets[m_sPosition].ToString();
                picFilter.Image = Properties.Resources.FilterStop;
                picFilter.BackColor = m_acFilterColours[m_sPosition];
                btnConnect.Text = "Disconnect";
                btnConnect.BackColor = System.Drawing.Color.DarkGreen;
            }
            else
            {
                lblConnect.Text = "Disconnected";
                lblPosition.Text = "?";
                lblName.Text = "?";
                lblOffset.Text = "?";
                picFilter.BackColor = Color.DimGray;
                btnConnect.Text = "Connect";
                btnConnect.BackColor = System.Drawing.Color.Maroon;
            }
            btnNext.Enabled = m_bConnected;
            btnPrev.Enabled = m_bConnected;
        }

        private void UpdateConfig()
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
                string[] names = new string[8] {"Red", "Green", "Blue", "Clear", "Ha", "OIII", "LPR", "Dark"};
                Random rand = new Random();

                g_Profile.WriteValue(g_csDriverID, "RegVer", m_sRegVer);
                g_Profile.WriteValue(g_csDriverID, "Position", "0");
                g_Profile.WriteValue(g_csDriverID, "Slots", "4");
                g_Profile.WriteValue(g_csDriverID, "Time", "1000");
                g_Profile.WriteValue(g_csDriverID, "ImplementsNames", "true");
                g_Profile.WriteValue(g_csDriverID, "ImplementsOffsets", "true");
                for (i=0; i<8; i++)
                {
                    g_Profile.WriteValue(g_csDriverID, Convert.ToString(i), names[i], "FilterNames");
                    g_Profile.WriteValue(g_csDriverID, Convert.ToString(i), Convert.ToString(rand.Next(10000)), "FocusOffsets");
                    g_Profile.WriteValue(g_csDriverID, Convert.ToString(i), Convert.ToString(ColorTranslator.ToWin32(colours[i])), "FilterColours");
                }
            }

            // Read the hardware config
            m_iSlots = m_Hardware.Slots;
            m_sPosition = m_Hardware.Position;
            m_bImplementsNames = Convert.ToBoolean(GetSetting("ImplementsNames", "true"));
            m_bImplementsOffsets = Convert.ToBoolean(GetSetting("ImplementsOffsets", "true"));
            for (i = 0; i <= 7; i++)
            {
                m_asFilterNames[i] = g_Profile.GetValue(g_csDriverID, Convert.ToString(i), "FilterNames");
                m_aiFocusOffsets[i] = Convert.ToInt32(g_Profile.GetValue(g_csDriverID, Convert.ToString(i), "FocusOffsets"));
                m_acFilterColours[i] = System.Drawing.ColorTranslator.FromWin32(Convert.ToInt32(g_Profile.GetValue(g_csDriverID, Convert.ToString(i), "FilterColours")));
            }
        }

        //
        // Settings support
        //
        private string GetSetting(string Name, string DefValue)
        {
            string s = g_Profile.GetValue(g_csDriverID, Name, "");
            if (s == "") s = DefValue;
            return s;
        }

#endregion

    }
}
