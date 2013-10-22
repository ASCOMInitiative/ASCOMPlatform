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
				SiumulatedCameraType cameraType = SiumulatedCameraType.AnalogueNonIntegrating;

				if (rbAnalogueNonIntegrating.Checked)
					cameraType = SiumulatedCameraType.AnalogueNonIntegrating;
				else if (rbAnalogueIntegrating.Checked)
					cameraType = SiumulatedCameraType.AnalogueIntegrating;
				else if (rbDigitalVideoCamera.Checked)
					cameraType = SiumulatedCameraType.Digital;
				else if (rbVideoSystem.Checked)
					cameraType = SiumulatedCameraType.VideoSystem;

				Settings.Default.Reset(cameraType);
				Settings.Default.Save();
			}

			DialogResult = DialogResult.OK;

			Close();
		}
	}
}
