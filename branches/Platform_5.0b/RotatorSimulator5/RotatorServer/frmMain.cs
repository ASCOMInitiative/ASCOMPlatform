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
		//delegate void SetTextCallback(string text);

		private int m_iUpdateInterval;
		private bool m_bFlasher;
		public frmMain()
		{
			InitializeComponent();
		}

		//
		// This is also used by the driver class to access the shared
		// setup dialog. If the driver is asked for this while connected, 
		// it will trap there.
		//
		public void DoSetupDialog()
		{
			frmSetup setupForm = new frmSetup();

			setupForm.CanReverse = RotatorHardware.CanReverse;
			setupForm.Reverse = RotatorHardware.Reverse;
			setupForm.RotationRate = RotatorHardware.RotationRate;
			setupForm.UpdateInterval = m_iUpdateInterval;

			DialogResult ans = setupForm.ShowDialog(this);

			if (ans == DialogResult.OK)
			{
				RotatorHardware.CanReverse = setupForm.CanReverse;
				RotatorHardware.Reverse = setupForm.Reverse;
				RotatorHardware.RotationRate = setupForm.RotationRate;
			}

			setupForm.Dispose();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			// the RotatorHardware engine is already initialized
			m_iUpdateInterval = 250;
			RotatorHardware.UpdateInterval = m_iUpdateInterval;
			tmrMain.Enabled = true;
			m_bFlasher = false;
            lblVersion.Text = "Version " + Application.ProductVersion.ToString();
		}

		private void cmdSetup_Click(object sender, EventArgs e)
		{
			DoSetupDialog();						// Shared with driver class
		}

		private void cmdMove_Click(object sender, EventArgs e)
		{
			float newPA;
			try
			{
				newPA = Convert.ToSingle(this.txtPosition.Text);
			}
			catch (Exception)
			{
				this.txtPosition.Text = "";
				return;
			}
			RotatorHardware.MoveAbsolute(newPA);
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
			RotatorHardware.UpdateState();						// Pump the machine
			//
			// Read the state of the rotator machine. No shortcuts, it's state
			// is also affected by ASCOM clients via the IRotator interface!
			//
			this.chkConnected.Checked = RotatorHardware.Connected;
			if (RotatorHardware.Connected)
			{
				this.lblPosition.Text = RotatorHardware.Position.ToString("000.0");
				this.lblPosition.ForeColor = Color.White;
			}
			else
			{
				this.lblPosition.Text = "---.-";
				this.lblPosition.ForeColor = Color.FromArgb(128, 4, 4);
			}

			this.cmdSetup.Enabled = !RotatorHardware.Connected;
			this.cmdMove.Enabled = RotatorHardware.Connected && !RotatorHardware.Moving;
			this.cmdHalt.Enabled = RotatorHardware.Connected && RotatorHardware.Moving;

			this.lblREV.ForeColor = (RotatorHardware.Reverse ? Color.Cyan : Color.FromArgb(128, 4, 4));
			this.lblCON.ForeColor = (RotatorHardware.Connected ? Color.LightGreen : Color.FromArgb(128, 4, 4));
			if (RotatorHardware.Connected && RotatorHardware.Moving && m_bFlasher)
				this.lblMOVE.ForeColor = Color.Yellow;
			else
				this.lblMOVE.ForeColor = Color.FromArgb(128, 4, 4);
			m_bFlasher = !m_bFlasher;
		}
	}
}