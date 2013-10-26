//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - DirectShow
//
// Description:	This is the set up form for the Video Driver DirectShow 
//
// Author:		(HDP) Hristo Pavlov <hristo_dpavlov@yahoo.com>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 13-Mar-2013	HDP	6.0.0	Initial commit
// 21-Mar-2013	HDP	6.0.0.	Implemented monochrome and colour grabbing
// 22-Mar-2013	HDP	6.0.0	Added support for XviD and Huffyuv codecs
// --------------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities.Video.DirectShowVideo;
using ASCOM.Utilities.Video.DirectShowVideo.VideoCaptureImpl;
using ASCOM.Utilities.Video;
using DirectShowLib;

namespace ASCOM.Utilities.Video.DirectShowVideo
{
	[ComVisible(false)]
	internal partial class frmSetupDialog : Form
	{
		private string driverVersion;

		public frmSetupDialog(Settings settings, string driverVersion)
		{
			InitializeComponent();

			this.driverVersion = driverVersion;
			ucDirectShowVideoSettings.Initialize(settings);
		}

		private void frmSetupDialog_Load(object sender, EventArgs e)
		{
			lblVersion.Text = "v" + driverVersion;
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			if (ucDirectShowVideoSettings.SaveSettings() == DialogResult.OK)
				Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			Close();
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
	}
}