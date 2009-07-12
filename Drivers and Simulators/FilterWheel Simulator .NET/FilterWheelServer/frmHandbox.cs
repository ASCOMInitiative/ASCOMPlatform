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

        public static frmTraffic m_trafficDialog = null;  // API traffic display form
        //public static SimulatedHardware m_Hardware;     // Simulated wire

        private static int m_iTimerTickInterval = 100;    // How often we pump the hardware
        private static bool m_bMoveInProgress = false;    // Tracks if a move is in progress - used for animation
      
#endregion

        public frmHandbox()
        {
            InitializeComponent();
            
            ToolTip aTooltip = new ToolTip();
            aTooltip.SetToolTip(picASCOM, "Visit the ASCOM website");
 //           aTooltip.SetToolTip(btnTraffic, "Monitor ASCOM API traffic");
            aTooltip.SetToolTip(chkConnected, "Connect/Disconnect filterwheel");
            aTooltip.SetToolTip(btnPrev, "Move position to previous filter");
            aTooltip.SetToolTip(btnNext, "Move position to next filter");

            SimulatedHardware.Initialize();
            SimulatedHardware.TimerTickInverval = m_iTimerTickInterval;

            this.Timer.Enabled = true;
        }


#region Properties and Methods

        public void DoSetup()
        {
            frmSetupDialog SetupDialog = new frmSetupDialog();

            // Stop the timer
            this.Timer.Enabled = false;

            // Do we need to log this?
            if (m_trafficDialog != null && m_trafficDialog.chkOther.Checked)
                m_trafficDialog.TrafficStart("SetupDialog");

            SetupDialog.Slots = SimulatedHardware.Slots;
            SetupDialog.Time = SimulatedHardware.Interval;
            SetupDialog.Names = SimulatedHardware.FullFilterNames;
            SetupDialog.Offsets = SimulatedHardware.FullFocusOffsets;
            SetupDialog.Colours = SimulatedHardware.FullFilterColours;
            SetupDialog.ImplementsNames = SimulatedHardware.ImplementsNames;
            SetupDialog.ImplementsOffsets = SimulatedHardware.ImplementsOffsets;
            SetupDialog.PreemptsMoves = SimulatedHardware.PreemptMoves;


            this.Visible = false;         // May float over setup
            SetupDialog.TopMost = true;   // The ASCOM chooser dialog sits on top if we don't do this :(

            if (SetupDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Update the hardware config
                SimulatedHardware.Initialize();
                // and the form
                this.Timer_Tick(null,null);
            }

            this.Visible = true;
            this.BringToFront();
            this.Timer.Enabled = true;
            SetupDialog = null;

            // Do we need to log this?
            if (m_trafficDialog != null && m_trafficDialog.chkOther.Checked)
                m_trafficDialog.TrafficEnd(" (done)");

        }

#endregion

#region EventHandlers

        private void chkConnected_CheckedChanged(object sender, EventArgs e)
        {
            SimulatedHardware.Connected = chkConnected.Checked;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            PrevNext(false);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PrevNext(true);
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

 /*       private void btnTraffic_Click(object sender, EventArgs e)
        {
            if (m_trafficDialog == null)
                m_trafficDialog = new frmTraffic();

            m_trafficDialog.Show();

        }
*/
        private void btnSetup_Click(object sender, EventArgs e)
        {
            SimulatedHardware.DoSetup();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SimulatedHardware.UpdateState();						// Pump the machine
            //
            // Read the state of the Simulated hardware. No shortcuts, it's state
            // is also affected by ASCOM clients via the IFilterWheel interface!
            //

            this.chkConnected.Checked = SimulatedHardware.m_bConnected;
            this.btnNext.Enabled = SimulatedHardware.m_bConnected;
            this.btnPrev.Enabled = SimulatedHardware.m_bConnected;
            this.btnSetup.Enabled = !SimulatedHardware.m_bConnected;

            if (SimulatedHardware.m_bConnected && !SimulatedHardware.Moving)
            { // Connected, not Moving
                this.lblPosition.Text = Convert.ToString(SimulatedHardware.m_sPosition);
                this.lblName.Text = Convert.ToString(SimulatedHardware.CurrFilterName);
                this.lblOffset.Text = Convert.ToString(SimulatedHardware.CurrFilterOffset);
                this.picFilter.BackColor = SimulatedHardware.CurrFilterColour;
                this.picFilter.Image = Properties.Resources.FilterStop;
                m_bMoveInProgress = false;
            }
            else if (SimulatedHardware.m_bConnected && SimulatedHardware.Moving)
            { // Conncted, Moving
                this.lblPosition.Text = "Moving";
                this.lblName.Text = "";
                this.lblOffset.Text = "";
                if (!m_bMoveInProgress)
                {
                    this.picFilter.Image = Properties.Resources.Filter_Next;
                    m_bMoveInProgress = true;
                }
            }
            else
            { // Not connected
                this.lblPosition.Text = "?";
                this.lblName.Text = "";
                this.lblOffset.Text = "";
                this.picFilter.BackColor = Color.DimGray;
                this.picFilter.Image = Properties.Resources.FilterStop;
                m_bMoveInProgress = false;
            }


        }

        private void frmHandbox_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Close the traffic dialog if it is open
                if (m_trafficDialog != null)
                {
                    m_trafficDialog.Close();
                    m_trafficDialog = null;
                }
            }
            catch { }


        }

#endregion

#region Private Functions

        private void PrevNext(bool nxt)
        {
            short newPosition = SimulatedHardware.m_sPosition;
            if (nxt)
                newPosition++;
            else
                newPosition--;

            // make sure position stays in range
            if (newPosition >= SimulatedHardware.Slots)
                newPosition = 0;
            else if (newPosition < 0)
                newPosition = (short)(SimulatedHardware.Slots - 1);

            try { SimulatedHardware.Position = newPosition; }
            catch { }
        }

#endregion


    }
}
