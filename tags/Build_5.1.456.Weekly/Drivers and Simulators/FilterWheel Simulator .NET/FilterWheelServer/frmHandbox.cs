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

        private static int m_iTimerTickInterval = 100;    // How often we pump the hardware
        private static bool m_bMoveInProgress = false;    // Tracks if a move is in progress - used for animation
        private static frmTraffic m_trafficDialog = null; // API traffic display form
        private static short m_sTargetPosition;
      
#endregion

        public frmHandbox()
        {
            InitializeComponent();
            
            ToolTip aTooltip = new ToolTip();
            aTooltip.SetToolTip(picASCOM, "Visit the ASCOM website");
            aTooltip.SetToolTip(btnTraffic, "Monitor ASCOM API traffic");
            aTooltip.SetToolTip(chkConnected, "Connect/Disconnect filterwheel");
            aTooltip.SetToolTip(btnPrev, "Move position to previous filter");
            aTooltip.SetToolTip(btnNext, "Move position to next filter");

            SimulatedHardware.Initialize();
            SimulatedHardware.TimerTickInverval = m_iTimerTickInterval;

            this.Timer.Enabled = true;
        }


#region Properties and Methods


#endregion

#region EventHandlers

        private void chkConnected_MouseClick(object sender, MouseEventArgs e)
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

        private void btnTraffic_Click(object sender, EventArgs e)
        {
            SimulatedHardware.m_bLogTraffic = !SimulatedHardware.m_bLogTraffic;

            if (SimulatedHardware.m_bLogTraffic)
            {
                if (m_trafficDialog == null)
                {
                    m_trafficDialog = new frmTraffic();
                    // Catch FormClosing Events, to Hide() form instead
                    m_trafficDialog.Closing += new CancelEventHandler(frmTraffic_Closing);
                }
                m_trafficDialog.Show();
            }
            else
                m_trafficDialog.Hide();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            SimulatedHardware.DoSetup();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SimulatedHardware.UpdateState();			// Pump the machine
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
                this.lblPosition.Text = SimulatedHardware.m_sPosition.ToString();
                this.lblName.Text = SimulatedHardware.CurrFilterName.ToString();
                this.lblOffset.Text = SimulatedHardware.CurrFilterOffset.ToString();
                this.picFilter.BackColor = SimulatedHardware.CurrFilterColour;
                this.picFilter.Image = Properties.Resources.FilterStop;
                m_bMoveInProgress = false;
                m_sTargetPosition = SimulatedHardware.m_sPosition;
            }
            else if (SimulatedHardware.m_bConnected && SimulatedHardware.Moving)
            { // Conncted, Moving
                this.lblPosition.Text = "Moving";
                this.lblName.Text = "-";
                this.lblOffset.Text = "-";
                if (!m_bMoveInProgress)
                {
                    this.picFilter.Image = Properties.Resources.Filter_Next;
                    m_bMoveInProgress = true;
                }
            }
            else
            { // Not connected
                this.lblPosition.Text = "-";
                this.lblName.Text = "-";
                this.lblOffset.Text = "-";
                this.picFilter.BackColor = Color.DimGray;
                this.picFilter.Image = Properties.Resources.FilterStop;
                m_bMoveInProgress = false;
            }


        }

        private void DoSetup()
        {
            this.Timer.Enabled = false;     // Stop the timer
            this.Visible = false;           // May float over setup
            SimulatedHardware.DoSetup();    // Show the setup dialog
            this.Timer.Enabled = true;      // Restart form updates
            this.Visible = true;            // Show ourself again
        }


        private void frmHandbox_FormClosing(object sender, FormClosingEventArgs e)
        {
            // TODO: Save form postion
        }

#endregion

#region Private Functions

        private void PrevNext(bool nxt)
        {
            short savePos = m_sTargetPosition;

            if (nxt)
                m_sTargetPosition++;
            else
                m_sTargetPosition--;

            // make sure position stays in range
            if (m_sTargetPosition >= SimulatedHardware.Slots)
                m_sTargetPosition = 0;
            else if (m_sTargetPosition < 0)
                m_sTargetPosition = (short)(SimulatedHardware.Slots - 1);

            try { SimulatedHardware.Position = m_sTargetPosition; }
            catch { m_sTargetPosition = savePos;  }
        }

        //
        // Overrides the Traffic Dialog close event.
        // Traps when the traffic form is closed, and hides the form instead.
        //
        private void frmTraffic_Closing(object sender, CancelEventArgs e)
        {
            SimulatedHardware.m_bLogTraffic = false;
            m_trafficDialog.Hide();
            // Cancel the Closing event from closing the form.
            e.Cancel = true;
        }

#endregion


    }
}
