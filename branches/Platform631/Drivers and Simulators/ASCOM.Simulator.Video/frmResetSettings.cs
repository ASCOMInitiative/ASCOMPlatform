//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - Simulator
//
// Description:	This simple dialog is used to control the resetting of the Video 
//              Driver Simujlator settings
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.Simulator.Properties;

namespace ASCOM.Simulator
{
	public partial class frmResetSettings : Form
	{
		public frmResetSettings()
		{
			InitializeComponent();
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			if (rbDriverDefaults.Checked)
			{
				Settings.Default.ResetDefaults();
                Settings.Default.Save();
			}
			else
			{
				SimulatedCameraType cameraType = SimulatedCameraType.AnalogueNonIntegrating;

				if (rbAnalogueNonIntegrating.Checked)
					cameraType = SimulatedCameraType.AnalogueNonIntegrating;
				else if (rbAnalogueIntegrating.Checked)
					cameraType = SimulatedCameraType.AnalogueIntegrating;
				else if (rbDigitalVideoCamera.Checked)
					cameraType = SimulatedCameraType.Digital;
				else if (rbVideoSystem.Checked)
					cameraType = SimulatedCameraType.VideoSystem;

				Settings.Default.Reset(cameraType);
				Settings.Default.Save();
			}

			DialogResult = DialogResult.OK;

			Close();
		}
	}
}
