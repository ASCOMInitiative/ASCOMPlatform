using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.HelperNET;
using System.Diagnostics;

namespace ASCOM.Controls.Demo
	{
	public partial class frmDemo : Form
		{
		public frmDemo()
			{
			InitializeComponent();
			}

		private void ctlChooser_SelectionChanged(object sender, EventArgs e)
			{
			Trace.WriteLine(
				String.Format("New device selection is: {0} | {1}", ctlTelescopeChooser.SelectedDevice.DeviceID, ctlTelescopeChooser.SelectedDevice.Description)
				);
			}

		private void frmDemo_Load(object sender, EventArgs e)
			{
			Trace.WriteLine("Loading frmDemo");
			}

		private void btnOK_Click(object sender, EventArgs e)
			{
			this.Close();
			}

		private void btnCancel_Click(object sender, EventArgs e)
			{
			this.Close();
			}

		private void ctlCameraChooser_SelectionChanged(object sender, EventArgs e)
			{
			Trace.WriteLine("Camera selection changed");
			ledCameraChosen.Cadence = CadencePattern.BlinkSlow;
			ledCameraChosen.Green = true;
			ledCameraChosen.Red = false;
			ledCameraConfigured.Red = true;
			ledCameraConfigured.Green = false;
			ledCameraConfigured.Cadence = CadencePattern.BlinkAlarm;
			}

		private void ctlTelescopeChooser_SelectionChanged(object sender, EventArgs e)
			{
			Trace.WriteLine("Telescope selection changed");
			ledTelescopeChosen.Cadence = CadencePattern.BlinkSlow;
			ledTelescopeChosen.Green = true;
			ledTelescopeChosen.Red = false;
			ledTelescopeConfigured.Red = true;
			ledTelescopeConfigured.Green = false;
			ledTelescopeConfigured.Cadence = CadencePattern.BlinkAlarm;
			}
		}
	}
