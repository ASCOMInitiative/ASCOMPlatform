//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Video Driver - Simulator
//
// Description:	This is a simple "Wait Form" during the loading of the video frames
//              which may take a minute or so
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

namespace ASCOM.Simulator.VideoCameraImpl
{
	public partial class frmLoadingImages : Form
	{
		public frmLoadingImages()
		{
			InitializeComponent();
		}

		public void SetProgress(int current, int max)
		{
			pbar.Maximum = max;
			pbar.Value = Math.Min(current, max);
			pbar.Update();
		}
	}
}
