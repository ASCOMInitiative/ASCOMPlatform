using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.Simulator
{
    public partial class frmMain : Form
    {

        private int updateInterval;
        private bool flasher;

        public frmMain()
        {
            InitializeComponent();
        }

        //
        // This is also used by the driver class to access the shared
        // setup dialogue. If the driver is asked for this while connected, 
        // it will trap there.
        //
        public void DoSetupDialog()
        {
            frmSetup setupForm = new frmSetup();

            setupForm.CanReverse = RotatorHardware.CanReverse;
            setupForm.Reverse = RotatorHardware.Reverse;
            setupForm.RotationRate = RotatorHardware.RotationRate;
            setupForm.CanSync = RotatorHardware.CanSync;
            setupForm.SyncOffset = RotatorHardware.SyncOffset;

            setupForm.UpdateInterval = updateInterval;

            DialogResult ans = setupForm.ShowDialog(this);

            if (ans == DialogResult.OK)
            {
                RotatorHardware.CanReverse = setupForm.CanReverse;
                RotatorHardware.Reverse = setupForm.Reverse;
                RotatorHardware.RotationRate = setupForm.RotationRate;
                RotatorHardware.CanSync = setupForm.CanSync;
                RotatorHardware.SyncOffset = setupForm.SyncOffset;
            }

            setupForm.Dispose();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // the RotatorHardware engine is already initialized
            updateInterval = 250;
            RotatorHardware.UpdateInterval = updateInterval;
            tmrMain.Enabled = true;
            flasher = false;

            // Bring this form to the front of the screen
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;

        }

        private void cmdSetup_Click(object sender, EventArgs e)
        {
            DoSetupDialog();                        // Shared with driver class
        }

        private void cmdMove_Click(object sender, EventArgs e)
        {
            if (float.TryParse(this.txtPosition.Text, out float newPA))
            {
                RotatorHardware.MoveAbsolute(newPA);
            }
        }

        private void cmdHalt_Click(object sender, EventArgs e)
        {
            RotatorHardware.Halt();
        }

        private void chkConnected_CheckedChanged(object sender, EventArgs e)
        {
            RotatorHardware.Connected = this.chkConnected.Checked;
        }

        private void tmrMain_Tick(object sender, EventArgs e)
        {
            RotatorHardware.UpdateState();                      // Pump the machine
                                                                //
                                                                // Read the state of the rotator machine. No shortcuts, it's state
                                                                // is also affected by ASCOM clients via the IRotator interface!
                                                                //
            this.chkConnected.Checked = RotatorHardware.Connected;
            if (RotatorHardware.Connected)
            {
                this.LblSkyPosition.Text = $" {RotatorHardware.Position.ToString("000.0")}";
                this.LblSkyPosition.ForeColor = Color.White;
                this.LblMechanicalPosition.Text = $" {RotatorHardware.InstrumentalPosition.ToString("000.0")}";
                this.LblMechanicalPosition.ForeColor = Color.White;

                if (RotatorHardware.CanSync) // If the rotator is able to sync display the sync offset
                {
                    this.LblSyncOffset.Text = RotatorHardware.SyncOffset.ToString("+000.0;-000.0");
                    this.LblSyncOffset.ForeColor = Color.White;
                }
                else // We can't sync so set the value to N/A and disable the display
                {
                    this.LblSyncOffset.ForeColor = Color.Red;
                    this.LblSyncOffset.Text = "N/A";
                }
            }
            else
            {
                this.LblSkyPosition.Text = "---.-";
                this.LblSkyPosition.ForeColor = Color.FromArgb(128, 4, 4);
                this.LblMechanicalPosition.Text = "---.-";
                this.LblMechanicalPosition.ForeColor = Color.FromArgb(128, 4, 4);
                this.LblSyncOffset.Text = "---.-";
                this.LblSyncOffset.ForeColor = Color.FromArgb(128, 4, 4);
            }

            this.cmdSetup.Enabled = !RotatorHardware.Connected;
            this.cmdMove.Enabled = RotatorHardware.Connected && !RotatorHardware.IsMoving;
            this.cmdHalt.Enabled = RotatorHardware.Connected && RotatorHardware.IsMoving;

            this.lblREV.ForeColor = (RotatorHardware.Reverse ? Color.Cyan : Color.FromArgb(128, 4, 4));
            this.lblCON.ForeColor = (RotatorHardware.Connected ? Color.LightGreen : Color.FromArgb(128, 4, 4));
            if (RotatorHardware.Connected && RotatorHardware.IsMoving && flasher)
                this.lblMOVE.ForeColor = Color.Yellow;
            else
                this.lblMOVE.ForeColor = Color.FromArgb(128, 4, 4);
            flasher = !flasher;
        }
    }
}