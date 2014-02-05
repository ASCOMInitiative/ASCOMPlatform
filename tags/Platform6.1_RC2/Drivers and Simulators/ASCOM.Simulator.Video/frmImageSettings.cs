using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Simulator.VideoCameraImpl;

namespace ASCOM.Simulator
{
	public partial class frmImageSettings : Form
	{
		internal VideoCamera Camera { get; set; }

		public frmImageSettings()
		{
			InitializeComponent();
		}


		private void frmImageSettings_Load(object sender, EventArgs e)
		{
			int balance = Camera.GetWhiteBalance();

			tbWhiteBalance.Value = Math.Min(255, Math.Max(0, balance));
		}

		private void tbWhiteBalance_ValueChanged(object sender, EventArgs e)
		{
			Camera.SetWhiteBalance(tbWhiteBalance.Value);
		}
	}
}
