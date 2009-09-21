using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.Optec
{
	[ComVisible(false)]					// Form not registered for COM!
	public partial class SetupDialogForm : Form
	{
		public SetupDialogForm()
		{
			InitializeComponent();
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			Dispose();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			Dispose();
		}

		private void BrowseToAscom(object sender, EventArgs e)
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

        private void Connect_Btn_Click(object sender, EventArgs e)
        {
            //DeviceComm.Connect();
            //DeviceComm.ComState = 2;
            //MessageBox.Show(DeviceComm.ComState.ToString());

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (CurrentPosition_CB.SelectedIndex)
            {
                case 0:
                    PortPicture.Image = Properties.Resources.Rotator1;
                    break;
                case 1:
                    PortPicture.Image = Properties.Resources.Rotator2;
                    break;
                case 2:
                    PortPicture.Image = Properties.Resources.Rotator3;
                    break;
                case 3:
                    PortPicture.Image = Properties.Resources.Rotator4;
                    break;
            }
            
        }

        private void LED_CheckedChanged(object sender, EventArgs e)
        {
            if (LEDOn_RB.Checked)
            {
                LEDPicture.Image = Properties.Resources.LEDOn;
            }
            if (LEDOff_RB.Checked)
            {
                LEDPicture.Image = Properties.Resources.LEDOff;
            }
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {

        }

        private void Offsets_Btn_Click(object sender, EventArgs e)
        {
            
        }
        private void SetupOffsets(object sender, EventArgs e)
        {
            OffsetsForm OffsetSetup = new OffsetsForm(1);
            OffsetSetup.StartPosition = FormStartPosition.CenterParent;
            OffsetSetup.ShowDialog();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceComm.Connect();
            EnableDisableControls();

        }
        private void EnableDisableControls()
        {
            if (DeviceComm.ComState == 2)
            {
                //device is connected
                connectToolStripMenuItem.Enabled = false;
                disconnectToolStripMenuItem.Enabled = true;
                CurrentPosition_CB.Enabled = true;
                PortPicture.Enabled = true;
                LED_GB.Enabled = true;
                LEDOff_RB.Enabled = true;
                LEDOn_RB.Enabled = true;
                setupOffsetsToolStripMenuItem.Enabled = true;
            }
            else
            {
                //device is disconnected
                connectToolStripMenuItem.Enabled = true;
                disconnectToolStripMenuItem.Enabled = false;
                CurrentPosition_CB.Enabled = false;
                PortPicture.Enabled = false;
                LED_GB.Enabled = false;
                LEDOff_RB.Enabled = false;
                LEDOn_RB.Enabled = false;
                setupOffsetsToolStripMenuItem.Enabled = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetupDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DeviceComm.ComState != 0)
            {
                DeviceComm.Disconnect();
                
            }
        }
	}
}